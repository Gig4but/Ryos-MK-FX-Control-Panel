using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using NAudio.Wave;

namespace RyosMKFXPanel {
	class Equalizer :LightningModule {
		public override string GetUid() {
			return "DefaultEqualizer";
		}
		public override string GetName() {
			return "Equalizer";
		}
		public override string GetCategory() {
			return "Effects";
		}
		public override string GetIcon() {
			return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAFqAAABagF7rdg2AAABQ0lEQVRYhe2X4XGDMAyFP3IMwAgegQ0gG3QEj9ANyAgdISN0g7gbMIJHYAP3B+bqK5JLQkP4wbvz4eMJ6yEk2RQhhICOM+Ay/GqU8VoI3FMdTzht4eQQsGsBUxI6gau3EnDO8P2zBRQhBJfh3xkjYRX+GsfDKIEGOQofQAUYwAuObORWIZcDQzL3gk271jnsoAoOAUv7gGX+zQ0rKwDGMvy9cIqen0qQ4ON4GCX5bPbRuWbjFggw6H2EE9ApnE2cSwK0+/cI6KYcuCgOJjjBRnpGg1fsu5dXwSFgyoFW4KpkbgQbw/ISrBQfm2zHNePOKuI/GtGAfnoaoo16uirij8mXwNXAG2PoLPNwG8a3d8An89NTFQVcFB6gyeWAS+ZX8n2gF9ZoExuJBwgvr4JDwG4EBGE0iV0n8Oku2gj87Q8+AHwDC5xhAnEsvS8AAAAASUVORK5CYII=";
		}
		public override SliderType GetSliderType() {
			return SliderType.delay;
		}
		public override double GetSliderMax() {
			return 3.0d;
		}
		public override double GetSliderMin() {
			return 1.0d;
		}
		public override double GetSliderTick() {
			return 1.0d;
		}
		public override double GetSliderTickFrequency() {
			return 1.0d;
		}
		public override LightningModuleControls[] GetControls() {
			return new LightningModuleControls[] {
				new CInterval("Frequencies: ", new TextChangedEventHandler(ChangeMinHz), new TextChangedEventHandler(ChangeMaxHz), 5, 5, "20", "4200"),
				new CCheckBox("Antialiasing", new RoutedEventHandler(OnAntialiasing), new RoutedEventHandler(OffAntialiasing), true),
				new CSpoiler(
						new CCheckBox("Static volume border", new RoutedEventHandler(OnStaticVolume), new RoutedEventHandler(OffStaticVolume), false),
						new CInterval("Average intesity of columns ("+Lightning.devices[0].GetWidth().ToString()+" = max): ", new TextChangedEventHandler(ChangeStartColumn), new TextChangedEventHandler(ChangeEndColumn), 2, 2, "2", "23"),
						new CInputBox("Max. volume: ", new TextChangedEventHandler(ChangeStaticVolume), 5, "100")
					)
			};
		}


		public int minHz = 20;
		public int maxHz = 2800;
		public int startColumn = 2;
		public int endColumn = 23;
		public bool staticVolume = false;
		public int staticVolumeSize = 10;
		public bool antialiasing = false;

		//public override Param[] GetParameters() {
		//	return new Param[] { 
		//		new Param(minHz.GetType(), nameof(minHz), "MinHz"),
		//		new Param(maxHz.GetType(), nameof(maxHz), "MaxHz"),
		//		new Param(startColumn.GetType(), nameof(startColumn), "StartColumn"),
		//		new Param(endColumn.GetType(), nameof(endColumn), "EndColumn"),
		//		new Param(staticVolume.GetType(), nameof(staticVolume), "StaticVolume"),
		//		new Param(staticVolumeSize.GetType(), nameof(staticVolumeSize), "StaticVolumeSize"),
		//		new Param(antialiasing.GetType(), nameof(antialiasing), "Antialiasing")
		//	};
		//}

		public Equalizer() { }



		private IWaveIn _waveIn;
		private int _fftLengthDefault = 8192;
		private SampleAggregator _sampleAggregator;

		public override void Start() {
			RecordStart();
			running = true;
		}
		public override void Stop() {
			RecordStop();
			running = false;
		}

		private void RecordStart() {
			_sampleAggregator = new SampleAggregator(Convert.ToInt32(_fftLengthDefault / Math.Pow(2, Lightning.devices[0].delay)));
			_sampleAggregator.FftCalculated += new EventHandler<FftEventArgs>(FFT);
			_sampleAggregator.PerformFFT = true;
			_waveIn = new WasapiLoopbackCapture(Audio.GetListenDevice());
			_waveIn.DataAvailable += OnDataAvailable;
			_waveIn.StartRecording();
		}
		private void RecordStop() {
			_waveIn.StopRecording();
			_sampleAggregator.FftCalculated -= FFT;
			_waveIn.DataAvailable -= OnDataAvailable;
		}
		private void OnDataAvailable(object sender, WaveInEventArgs e) {
			byte[] buffer = e.Buffer;
			int bytesRecorded = e.BytesRecorded;
			int bufferIncrement = _waveIn.WaveFormat.BlockAlign;

			if (bytesRecorded > 0) {
				for (int index = 0; index < bytesRecorded; index += bufferIncrement) {
					float sample32 = BitConverter.ToSingle(buffer, index);
					_sampleAggregator.Add(sample32);
				}
			} else {
				Restart();
			}
		}
		private void FFT(object sender, FftEventArgs e) {
			float binSize = 44100 / _fftLengthDefault;
			int minBin = ( (int)(minHz / (binSize * Lightning.devices[0].delay * 4)) );
			if (minBin < 1)
				minBin = 1;
			int maxBin = (int)(maxHz / (binSize * Lightning.devices[0].delay * 4));
			float[] intensity = new float[1 + maxBin - minBin];
			float[] frequency = new float[1 + maxBin - minBin];
			float v = (float)Audio.VolumeIs();
			for (int bin = minBin; bin <= maxBin; bin++) {
				float real = e.Result[bin * 2].X;
				float imaginary = e.Result[bin * 2 + 1].Y;
				intensity[bin - minBin] = (real * real + imaginary * imaginary) * 100_000_000f;
				frequency[bin - minBin] = binSize * bin;
			}
			//binSize * bin - frequency; intensity - power
			float groupSize = (1 + maxBin - minBin) / Lightning.devices[0].GetWidth();
			float[] intensityAverage = new float[Lightning.devices[0].GetWidth()];
			float[] frequencyAverage = new float[Lightning.devices[0].GetWidth()];
			float sumF = 0;
			float sumI = 0;
			int averageIndex = 0;

			for (int i = 0; i < frequency.Length; i++) {
				sumF += frequency[i];
				sumI += intensity[i];
				if ((i + 1) % groupSize == 0 && (i + 1) / groupSize <= Lightning.devices[0].GetWidth()) {
					frequencyAverage[averageIndex] = sumF / groupSize;
					intensityAverage[averageIndex] = sumI / groupSize * frequencyAverage[averageIndex];
					averageIndex++;
					sumF = 0;
					sumI = 0;
				}
			}

			ToMatrixColor(intensityAverage);
			Lightning.devices[0].SendPacket();
		}

		private void ToMatrixColor(float[] iA) {
			float[][] Matrix = new float[Lightning.devices[0].GetHeight()][];
			for (int i = 0; i < Lightning.devices[0].GetHeight(); i++) {
				Matrix[i] = new float[Lightning.devices[0].GetWidth()];
			}
			float point = 0;
			if (staticVolume) {
				point = staticVolumeSize * (float)Audio.VolumeIs() * 1000;
			} else {
				for (int i = startColumn - 1; i < endColumn; i++) {
					point += iA[i];
					if (i == iA.Length - 1) {
						point /= i;
					}
				}
			}
			for (int index = 0; index < Lightning.devices[0].GetWidth(); index++) {
				float power = iA[index];
				int row = 0;
				for (float i = 0; i <= power && row < 6; i += point) {
					Matrix[row][index] = ((i + point) < power) ? 1 : power / point;
					row++;
				}
			}
			if (antialiasing) {
				Lightning.devices[0].KeysColorChangeByMatrix(Matrix);
			} else {
				Lightning.devices[0].KeysLightChangeByMatrix(Matrix);
			}
		}



		private void ChangeMinHz(object sender, TextChangedEventArgs e) {
			minHz = IntervalLibrary.ChangeMinValue(minHz, maxHz, (TextBox)sender);
		}
		private void ChangeMaxHz(object sender, TextChangedEventArgs e) {
			maxHz = IntervalLibrary.ChangeMinValue(minHz, maxHz, (TextBox)sender);
		}
		private void OnAntialiasing(object sender, RoutedEventArgs e) {
			antialiasing = true;
		}
		private void OffAntialiasing(object sender, RoutedEventArgs e) {
			antialiasing = false;
		}
		private void OnStaticVolume(object sender, RoutedEventArgs e) {
			staticVolume = true;
		}
		private void OffStaticVolume(object sender, RoutedEventArgs e) {
			staticVolume = false;
		}
		private void ChangeStartColumn(object sender, TextChangedEventArgs e) {
			startColumn = IntervalLibrary.ChangeMinValue(startColumn, endColumn, (TextBox)sender);
		}
		private unsafe void ChangeEndColumn(object sender, TextChangedEventArgs e) {
			/*fixed (int* endColumn = &endColumn) {
				IntervalLibrary.ChangeMaxValueUnsafe(startColumn, endColumn, (TextBox)sender);
			}*/
			endColumn = IntervalLibrary.ChangeMaxValue(startColumn, endColumn, (TextBox)sender);
		}
		private void ChangeStaticVolume(object sender, TextChangedEventArgs e) {
			staticVolumeSize = TextBoxLibrary.InputIntParse((TextBox)sender, 1);
		}
	}
}

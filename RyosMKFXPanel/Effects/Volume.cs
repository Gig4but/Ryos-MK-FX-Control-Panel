using System.Windows;

namespace RyosMKFXPanel.Effects {
	class Volume :LightningModule {
		public override string GetUid() {
			return "DefaultVolume";
		}
		public override string GetName() {
			return "Volume";
		}
		public override string GetCategory() {
			return "Effects";
		}
		public override string GetIcon() {
			return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAFhAAABYQHynvE+AAABWklEQVRYhe2WwXHCMBREXzK54w7iDnAHUTogHaSElEAJSQekg3SAO8B0QDoQFWwOkgchrGBhm5N3RmOQbe2bFeh/JDHhMJJqSVZS2fXMI9OoBDbA1n9f+LkLTQGwBhrAAG/+mtaIca8kHXzca0lFcE9+Oy7eSy1WSGqUr01ir5MAT4lgKmAJvGZEb330WUoBtKpzF8zVVP+CGWAGmAFmgN5KnYTWX+uMtSynSjgYoMGV0ipjrRWwA748iP336VYjlmMkvftybCV9DCnHQ0Yh1w9Irj8w9wZoRynpJ+gVOgEeJGVs800yuN/SZ9fNewCkVAL2WkMypipcGsZ/fgb2UyZgODdcAEfc2dKO3VgARWT24ud/I8ND9J5uBSgjw6Wf30eG1w6j3gDh/hlOcTaRYa6SAKFZGGdomN2CpwDC/TOcxxkaHkYw7ASwuEhb09awXzEZpm0McG99/wEc9bh7SWk85AAAAABJRU5ErkJggg==";
		}
		public override SliderType GetSliderType() {
			return SliderType.delay;
		}
		public override double GetSliderMax() {
			return 30.0d;
		}
		public override double GetSliderMin() {
			return 15.0d;
		}
		public override double GetSliderTick() {
			return 1.0d;
		}
		public override double GetSliderTickFrequency() {
			return 1.0d;
		}
		public override LightningModuleControls[] GetControls() {
			return new LightningModuleControls[] {
				new CCheckBox("Simple volume", new RoutedEventHandler(OnSimple), new RoutedEventHandler(OffSimple))
			};
		}


		public bool simple = false;

		//public override Param[] GetParameters() {
		//	return new Param[] {
		//		new Param(simple.GetType(), nameof(simple), "Simple")
		//	};
		//}

		public Volume() { }



		public override void Work() {
			int volume = 0;
			while (true) {
				Lightning.devices[0].KeysLightAllOff();
				Lightning.devices[0].KeysColorUpdate();

				volume = (int)(Audio.VolumeIs() * Lightning.devices[0].GetKeysCount()) - 1;
				if (simple) {
					volume /= 11;
					Lightning.devices[0].KeysLightOn(18, volume +18);
				} else {
					Lightning.devices[0].KeysLightOn(0, volume);
				}
				Lightning.devices[0].SendPacket();
			}
		}

		private void OnSimple(object sender, RoutedEventArgs e) {
			simple = true;
		}
		private void OffSimple(object sender, RoutedEventArgs e) {
			simple = false;
		}
	}
}

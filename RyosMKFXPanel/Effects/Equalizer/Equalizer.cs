using System;
using System.Threading;
using NAudio.Wave;


namespace RyosMKFXPanel {
    class Equalizer : Lightning {
        private static bool run = false;
        public static bool getState() {
            return run;
        }
        private static bool changeState() {
            run = ((run == false) ? true : false);
            return true;
        }

        public static float minHz = 20f;
        public static float maxHz = 2000f;

        private static IWaveIn waveIn;
        private static int fftLength = 8192;
        private static SampleAggregator sampleAggregator = new SampleAggregator(fftLength);

        public static void start() {
            recordStart();
            changeState();
        }
        public static void stop() {
            recordStop();
            changeState();
        }

        private static void recordStart() {
            sampleAggregator.FftCalculated += new EventHandler<FftEventArgs>(fft);
            sampleAggregator.PerformFFT = true;
            waveIn = new WasapiLoopbackCapture();
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();
        }
        private static void recordStop() {
            waveIn.StopRecording();
        }
        private static void OnDataAvailable(object sender, WaveInEventArgs e) {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            int bufferIncrement = waveIn.WaveFormat.BlockAlign;

            for (int index = 0; index < bytesRecorded; index += bufferIncrement) {
                float sample32 = BitConverter.ToSingle(buffer, index);
                sampleAggregator.Add(sample32);
            }
        }
        private static void fft(object sender, FftEventArgs e) {
            float binSize = 44100 / 8192;
            int minBin = (int)(minHz / binSize);
            int maxBin = (int)(maxHz / binSize);
            float[] intensity = new float[1 + maxBin - minBin];
            float[] frequency = new float[1 + maxBin - minBin];
            for (int bin = minBin; bin <= maxBin;
                bin++) {
                float real = e.Result[bin * 2].X;
                float imaginary = e.Result[bin * 2 + 1].Y;
                intensity[bin - minBin] = (real * real + imaginary * imaginary) * 1000_000_000f;
                frequency[bin - minBin] = binSize * bin;
            }
            //binSize * bin - frequency; intensity - power
            float groupSize = (1 + maxBin - minBin) / kbw;
            float[] intensityAverage = new float[kbw];
            float[] frequencyAverage = new float[kbw];
            float sumF = 0;
            float sumI = 0;
            int averageIndex = 0;

            for (int i = 0; i < frequency.Length; i++) {
                sumF += frequency[i];
                sumI += intensity[i];
                if ((i + 1) % groupSize == 0 && (i + 1) / groupSize <= kbw) {
                    frequencyAverage[averageIndex] = sumF / groupSize;
                    intensityAverage[averageIndex] = sumI / groupSize;
                    averageIndex++;
                    sumF = 0;
                    sumI = 0;
                }
            }
            toMatrixColor(intensityAverage);
            //toMatrixLight(intensityAverage);
        }

        static void toMatrixColor(float[] iA) {
            float[][] Matrix = new float[kbh][];
            for (int i = 0; i < kbh; i++) {
                Matrix[i] = new float[kbw];
            }
            float x = 0;
            for (int i = 1; i < iA.Length; i++) {
                x += iA[i];
                if (i == iA.Length - 1) {
                    x /= i;
                }
            }
            float point = x / 2;
            for (int index = 0; index < kbw; index++) {
                float power = iA[index];
                int row = 0;
                for (float i = 0; i <= power && row < 6; i += point) {
                    Matrix[row][index] = ((i + point) < power) ? 1 : power / point;
                    row++;
                }
            }
            toKeyboardColor(Matrix);
        }
        static void toKeyboardColor(float[][] Matrix) {
            byte[] keys = new byte[110];
            for (int i = 0; i < 110; i++) {
                keys[i] = 1;
            }
            byte[] keysColor = new byte[110 * 3];

            int kbi = 0;
            for (int row = kbh - 1; row >= 0; row--) {
                for (int i = 0; i < kbw; i++) {
                    //non exist, skip / before & after ESC, above numpad, LED indicators, left & rigth button UP, bottom of numPLUS & numENTER
                    if ((row == 5 && i == 0) || (row == 5 && i == 2)
                        || (row == 5 && i == 19) || (row == 5 && i == 20) || (row == 5 && i == 21) || (row == 5 && i == 22)
                        || (row == 2 && i == 16) || (row == 2 && i == 17) || (row == 2 && i == 18)
                        || (row == 1 && i == 16) || (row == 1 && i == 18)
                        || (row == 2 && i == 22) || (row == 0 && i == 22)) {/*do nothing*/}

                    //in center horizontal / F5, F6, F7, Q, W, E, R, T, Y, U, I, O, P, [, ], Windows, FN, AppendMenu
                    else if ((row == 5 && i == 7) || (row == 5 && i == 8) || (row == 5 && i == 9)
                        || (row == 3 && i == 2) || (row == 3 && i == 3) || (row == 3 && i == 4) || (row == 3 && i == 5)
                        || (row == 3 && i == 6) || (row == 3 && i == 7) || (row == 3 && i == 8) || (row == 3 && i == 9)
                        || (row == 3 && i == 10) || (row == 3 && i == 11) || (row == 3 && i == 12) || (row == 3 && i == 13)
                        || (row == 0 && i == 2) || (row == 0 && i == 12) || (row == 0 && i == 13)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.5) + (red * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.5) + (green * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.5) + (blue * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                    }

                    //more right /  A, S, D, F, G, H, J, K, L, ;, ', leftALT
                    else if ((row == 2 && i == 2) || (row == 2 && i == 3) || (row == 2 && i == 4) || (row == 2 && i == 5)
                        || (row == 2 && i == 6) || (row == 2 && i == 7) || (row == 2 && i == 8) || (row == 2 && i == 9)
                        || (row == 2 && i == 10) || (row == 2 && i == 11) || (row == 2 && i == 12)
                        || (row == 0 && i == 3)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.25) + (red * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.25) + (green * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.25) + (blue * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                    }
                    //more left / Z, X, C, V, B, N, M, ,, ., /, ALTGR
                    else if ((row == 1 && i == 3) || (row == 1 && i == 4) || (row == 1 && i == 5) || (row == 1 && i == 6)
                        || (row == 1 && i == 7) || (row == 1 && i == 8) || (row == 1 && i == 9) || (row == 1 && i == 10)
                        || (row == 1 && i == 11) || (row == 1 && i == 12) || (row == 0 && i == 11)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.8) + (red * Matrix[row][i + 1] * 0.2)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.8) + (green * Matrix[row][i + 1] * 0.2)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.8) + (blue * Matrix[row][i + 1] * 0.2)) * maxBright);
                        kbi++;
                    }

                    //in center horizontal without next center & 2 column sized / F8, Backspace, num0
                    else if ((row == 5 && i == 10) || (row == 4 && i == 14) || (row == 0 && i == 19)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.5) + (red * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.5) + (green * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.5) + (blue * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                        i++;
                    }

                    //1.25 left sized / TAB, leftCTRL
                    else if ((row == 3 && i == 1) || (row == 0 && i == 1)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.75) + (red * Matrix[row][i + 1] * 0.25)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.75) + (green * Matrix[row][i + 1] * 0.25)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.75) + (blue * Matrix[row][i + 1] * 0.25)) * maxBright);
                        kbi++;
                    }

                    //1.25 right sized / \(above ENTER(US loadout)), rightCTRL
                    else if ((row == 3 && i == 14) || (row == 0 && i == 14)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.25) + (red * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.25) + (green * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.25) + (blue * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                        i++;
                    }

                    //2 row sized / numPLUS, numENTER
                    else if ((row == 3 && i == 22) || (row == 1 && i == 22)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.5) + (red * Matrix[row - 1][i] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.5) + (green * Matrix[row - 1][i] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.5) + (blue * Matrix[row - 1][i] * 0.5)) * maxBright);
                        kbi++;
                    }

                    //1,75 sized / CapsLock
                    else if ((row == 2 && i == 1)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.825) + (red * Matrix[row][i + 1] * 0.125)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.825) + (green * Matrix[row][i + 1] * 0.125)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.825) + (blue * Matrix[row][i + 1] * 0.125)) * maxBright);
                        kbi++;
                    }

                    //2,15 sized left / leftSHIFT
                    else if ((row == 1 && i == 1)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.45) + (red * Matrix[row][i + 1] * 0.45) + (red * Matrix[row][i + 2] * 0.10)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.45) + (green * Matrix[row][i + 1] * 0.45) + (green * Matrix[row][i + 2] * 0.10)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.45) + (blue * Matrix[row][i + 1] * 0.45) + (blue * Matrix[row][i + 2] * 0.10)) * maxBright);
                        kbi++;
                        kbi += 3;
                        i++;
                    }

                    //2,15 sized right / ENTER
                    else if ((row == 2 && i == 13)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.10) + (red * Matrix[row][i + 1] * 0.45) + (red * Matrix[row][i + 2] * 0.45)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.10) + (green * Matrix[row][i + 1] * 0.45) + (green * Matrix[row][i + 2] * 0.45)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.10) + (blue * Matrix[row][i + 1] * 0.45) + (blue * Matrix[row][i + 2] * 0.45)) * maxBright);
                        kbi++;
                        i++;
                        i++;
                    }

                    //2,50 sized right / rightSHIFT
                    else if ((row == 1 && i == 13)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.2) + (red * Matrix[row][i + 1] * 0.4) + (red * Matrix[row][i + 2] * 0.4)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.2) + (green * Matrix[row][i + 1] * 0.4) + (green * Matrix[row][i + 2] * 0.4)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.2) + (blue * Matrix[row][i + 1] * 0.4) + (blue * Matrix[row][i + 2] * 0.4)) * maxBright);
                        kbi++;
                        i++;
                        i++;
                    }

                    //6,50 sized / SPACE
                    else if ((row == 0 && i == 4)) {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.1) + (red * Matrix[row][i + 1] * 0.15) + (red * Matrix[row][i + 2] * 0.15) + (red * Matrix[row][i + 3] * 0.15) + (red * Matrix[row][i + 4] * 0.15) + (red * Matrix[row][i + 5] * 0.15) + (red * Matrix[row][i + 6] * 0.15) + (red * Matrix[row][i + 7] * 0.05)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.1) + (green * Matrix[row][i + 1] * 0.15) + (green * Matrix[row][i + 2] * 0.15) + (green * Matrix[row][i + 3] * 0.15) + (green * Matrix[row][i + 4] * 0.15) + (green * Matrix[row][i + 5] * 0.15) + (green * Matrix[row][i + 6] * 0.15) + (green * Matrix[row][i + 7] * 0.05)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.1) + (blue * Matrix[row][i + 1] * 0.15) + (blue * Matrix[row][i + 2] * 0.15) + (blue * Matrix[row][i + 3] * 0.15) + (blue * Matrix[row][i + 4] * 0.15) + (blue * Matrix[row][i + 5] * 0.15) + (blue * Matrix[row][i + 6] * 0.15) + (blue * Matrix[row][i + 7] * 0.05)) * maxBright);
                        kbi++;
                        i += 6;
                    }

                    //normal
                    else {
                        keysColor[kbi] = (byte)((red * Matrix[row][i]) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)((green * Matrix[row][i]) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)((blue * Matrix[row][i]) * maxBright);
                        kbi++;
                    }

                }
            }
            Thread.Sleep(delay);
            connection.SetMkFxKeyboardState(keys, keysColor, 1);
        }
    }
}

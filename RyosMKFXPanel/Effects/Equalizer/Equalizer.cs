using System;
using System.Threading;
using NAudio.Wave;
using RyosMKFXPanel.Effects;

namespace RyosMKFXPanel {
    class Equalizer : Lightning {
        private static bool run = false;
        public static bool getState() {
            return run;
        }
        private static bool changeState() {
            run = ((run) ? false : true);
            return true;
        }

        public static int minHz = 20;
        public static int maxHz = 2800;
        public static int startColumn = 2;
        public static int endColumn = 23;
        private static bool staticVolume = false;
        public static int staticVolumeSize = 10;
        private static bool antialiasing = false;

        public static void turnVolume() {
            staticVolume = ((staticVolume) ? false : true);
        }
        public static void turnAntialiasing() {
            antialiasing = ((antialiasing) ? false : true);
        }

        private static IWaveIn waveIn;
        private static int fftLengthDefault = 8192;
        private static SampleAggregator sampleAggregator;

        public static void start() {
            recordStart();
            changeState();
        }
        public static void stop() {
            recordStop();
            changeState();
        }
        public static void restart() {
            recordStop();
            recordStart();
        }

        private static void recordStart() {
            sampleAggregator = new SampleAggregator(Convert.ToInt32(fftLengthDefault / Math.Pow(2, Lightning.delay)));
            sampleAggregator.FftCalculated += new EventHandler<FftEventArgs>(fft);
            sampleAggregator.PerformFFT = true;
            waveIn = new WasapiLoopbackCapture(Volume.getListenDevice());
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();
        }
        private static void recordStop() {
            waveIn.StopRecording();
            sampleAggregator.FftCalculated -= fft;
            waveIn.DataAvailable -= OnDataAvailable;
        }
        private static void OnDataAvailable(object sender, WaveInEventArgs e) {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            int bufferIncrement = waveIn.WaveFormat.BlockAlign;

            if (bytesRecorded > 0) {
                for (int index = 0; index < bytesRecorded; index += bufferIncrement) {
                    float sample32 = BitConverter.ToSingle(buffer, index);
                    sampleAggregator.Add(sample32);
                }
            } else {
                restart();
            }
        }
        private static void fft(object sender, FftEventArgs e) {
            float binSize = 44100 / fftLengthDefault;
            int minBin = ( (int)(minHz / (binSize * Lightning.delay * 4)) );
            if (minBin < 1)
                minBin = 1;
            int maxBin = (int)(maxHz / (binSize * Lightning.delay * 4));
            float[] intensity = new float[1 + maxBin - minBin];
            float[] frequency = new float[1 + maxBin - minBin];
            float v = (float)Volume.volumeIs();
            for (int bin = minBin; bin <= maxBin; bin++) {
                float real = e.Result[bin * 2].X;
                float imaginary = e.Result[bin * 2 + 1].Y;
                intensity[bin - minBin] = (real * real + imaginary * imaginary) * 100_000_000f;
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
                    intensityAverage[averageIndex] = sumI / groupSize * frequencyAverage[averageIndex];
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
            float point = 0;
            if (staticVolume) {
                point = staticVolumeSize * (float)Volume.volumeIs() * 1000;
            } else {
                for (int i = startColumn - 1; i < endColumn; i++) {
                    point += iA[i];
                    if (i == iA.Length - 1) {
                        point /= i;
                    }
                }
            }
            for (int index = 0; index < kbw; index++) {
                float power = iA[index];
                int row = 0;
                for (float i = 0; i <= power && row < 6; i += point) {
                    Matrix[row][index] = ((i + point) < power) ? 1 : power / point;
                    row++;
                }
            }
            if (antialiasing) {
                toKeyboardColor(Matrix);
            } else {
                toKeyboardLight(Matrix);
            }
        }
        static void toKeyboardColor(float[][] Matrix) {
            Thread.Sleep(delay);
            keysLightAllOn();
            keysColorReset();
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
            optimizePacket();
            sendPacket();
        }
        static void toKeyboardLight(float[][] Matrix) {
            //(sampleAggregator.fftLength < 1024) ? delay * delay : delay
            Thread.Sleep(delay);
            keysLightReset();
            keysColorUpdate();
            int kbi = 0;
            for (int row = kbh - 1; row >= 0; row--) {
                for (int column = 0; column < kbw; column++) {
                    //non exist, skip / before & after ESC, above numpad, LED indicators, left & rigth button UP, bottom of numPLUS & numENTER
                    if ((row == 5 && column == 0) || (row == 5 && column == 2) || (row == 5 && column == 11)
                        || (row == 5 && column == 19) || (row == 5 && column == 20) || (row == 5 && column == 21) || (row == 5 && column == 22)
                        || (row == 2 && column == 16) || (row == 2 && column == 17) || (row == 2 && column == 18)
                        || (row == 1 && column == 16) || (row == 1 && column == 18)
                        || (row == 2 && column == 22) || (row == 0 && column == 22)) {/*do nothing*/}
                    //BACKSPACE
                    else if (column == 14 && row == 4) {
                        keysLight[kbi] = (byte)((Matrix[row][column] + Matrix[row][column + 1] >= 0.5f) ? 1 : 0);
                        kbi++;
                        column++;
                    }
                    //BACKSLASH
                    else if (column == 14 && row == 3) {
                        keysLight[kbi] = (byte)((Matrix[row][column] + Matrix[row][column + 1] >= 0.5f) ? 1 : 0);
                        kbi++;
                        column++;
                    }
                    //ENTER
                    else if (column == 13 && row == 2) {
                        keysLight[kbi] = (byte)((Matrix[row][column] + Matrix[row][column + 1] + Matrix[row][column + 2] >= 0.5f) ? 1 : 0);
                        kbi++;
                        column++;
                        column++;
                    }
                    //LEFTSHIFT
                    else if (column == 1 && row == 1) {
                        keysLight[kbi] = (byte)((Matrix[row][column] + Matrix[row][column + 1] >= 0.5f) ? 1 : 0);
                        kbi++;
                        kbi++;//79 key is a LIE! (not exist in US -_-)
                        column++;
                    }
                    //RIGHTSHIFT
                    else if (column == 13 && row == 1) {
                        keysLight[kbi] = (byte)((Matrix[row][column] + Matrix[row][column + 1] + Matrix[row][column + 2] >= 0.5f) ? 1 : 0);
                        kbi++;
                        column++;
                        column++;
                    }
                    //RIGHTCTRL
                    else if (column == 14 && row == 0) {
                        keysLight[kbi] = (byte)((Matrix[row][column] + Matrix[row][column + 1] >= 0.5f) ? 1 : 0);
                        kbi++;
                        column++;
                    }
                    //SPACE
                    else if (column == 4 && row == 0) {
                        keysLight[kbi] = (byte)((Matrix[row][column] + Matrix[row][column + 1] + Matrix[row][column + 2]
                             + Matrix[row][column + 3] + Matrix[row][column + 4] + Matrix[row][column + 5]
                              + Matrix[row][column + 6] >= 0.5f) ? 1 : 0);
                        kbi++;
                        column += 6;
                    }
                    //NUMINSERT
                    else if (column == 19 && row == 0) {
                        keysLight[kbi] = (byte)((Matrix[row][column] + Matrix[row][column + 1] >= 0.5f) ? 1 : 0);
                        kbi++;
                        column++;
                    }
                    //NUMPLUS
                    else if (column == 22 && row == 3) {
                        keysLight[kbi] = (byte)((Matrix[row-1][column]>= 0.5f) ? 1 : 0);
                        kbi++;
                    }
                    //NUMENTER
                    else if (column == 22 && row == 1) {
                        keysLight[kbi] = (byte)((Matrix[row-1][column] >= 0.5f) ? 1 : 0);
                        kbi++;
                    } else {
                        keysLight[kbi] = (byte)((Matrix[row][column] >= 0.5f) ? 1 : 0);
                        kbi++;
                    }
                }
            }
            sendPacket();
        }
    }
}

/*
    xx  :  00  xx  01  02  03  04 :  05  06  07  08 :  09  10  11  12  :  13  14  15  :  xx  xx  xx  xx
    ....:.........................:.................:..................:..............:................
    16  :  17  18  19  20  21  22  23  24  25  26  27  28  29  3____0  :  31  32  33  :  34  35  36  37
    38  :  3___9 40  41  42  43  44  45  46  47  48  49  50  51 5___2  :  53  54  55  :  56  57  58  5x
    60  :  6____1 62  63  64  65  66  67  68  69  70  71  72  7_____3  :  nm  sh  sc  :  74  75  76  x9
    77  :  7_____8  80  81  82  83  84  85  86  87  88  89  9_______0  :  xx  91  xx  :  92  93  94  9y
    96  :  9___7 98  99  1______________________00  101  102 103 1_04  :  105 106 107 :  1___08 109  y5 

    000  :  001  002  003  004  005  006  007  008  009  010  011  012  013  014  015  :  016  017  018  :  019  020  021  022
    .....:..............................:...................:..........................:.................:....................
    023  :  024  025  026  027  028  029  030  031  032  033  034  035  036  037  038  :  039  040  041  :  042  043  044  045
    046  :  047  048  049  050  051  052  053  054  055  056  057  058  059  060  061  :  062  063  064  :  065  066  067  068
    069  :  070  071  072  073  074  075  076  077  078  079  080  081  082  083  084  :  085  086  087  :  089  090  091  092
    093  :  094  095  096  097  098  099  100  101  102  103  104  105  106  107  108  :  109  110  111  :  112  113  114  115
    116  :  117  118  119  120  121  122  123  124  125  126  127  128  129  130  131  :  132  133  134  :  135  136  137  138

    005  :  015  025  035  045  055  065  075  085  095  105  115  125  135  145  155  :  165  175  185  :  195  205  215  225
    .....:..............................:...................:..........................:.................:....................
    004  :  014  024  034  044  054  064  074  084  094  104  114  124  134  144  154  :  164  174  184  :  194  204  214  224
    003  :  013  023  033  043  053  063  073  083  093  103  113  123  133  143  153  :  163  173  183  :  193  203  213  223
    002  :  012  022  032  042  052  062  072  082  092  102  112  122  132  142  152  :  162  172  182  :  192  202  212  222
    001  :  011  021  031  041  051  061  071  081  091  101  111  121  131  141  151  :  161  171  181  :  191  201  211  221
    000  :  010  020  030  040  050  060  070  080  090  100  110  120  130  140  150  :  160  170  180  :  190  200  210  220
*/

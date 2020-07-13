using System;
using System.Threading;
using System.Windows.Input;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Roccat_Talk.RyosTalkFX;

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

namespace RoccatTalk.Test
{
    class Program
    {
        public static readonly int kbw = 23; //keyboar width / must be 23
        public static readonly int kbh = 6; //keyboard height / must be 6

        public static readonly byte red = 0;
        public static readonly byte green = 255;
        public static readonly byte blue = 255;
        public static readonly float maxBright = 1f;
        public static readonly bool changeLEDs = false;

        public static readonly float minHz = 20f;
        public static readonly float maxHz = 2000f;
        public static RyosTalkFXConnection connection = new RyosTalkFXConnection();
        [STAThread]
        static void Main()
        {
            
            connection.Initialize();
            connection.EnterSdkMode();
            byte[] keys = new byte[110];
            byte[] keysR = new byte[110];
            byte[] keysG = new byte[110];
            byte[] keysB = new byte[110];
            for (int i = 0; i < 110; i++)
            {
                keys[i] = 1;
            }
            for (int i = 0; i < 110; i++)
            {
                if (i == 32) {
                    keysR[i] = (byte)((float)red * maxBright);
                    keysG[i] = (byte)((float)green * maxBright);
                    keysB[i] = (byte)((float)blue * maxBright);
                }
            }
            Thread.Sleep(100);
            connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
            record();
            while (true)
            {
                Thread.Sleep(10);
                //frequencyVisualizer(connection, keys);
                //volumeVisualizer(connection, keys, keysR, keysG, keysB);
                //randomVisualizer(connection, keys, keysR, keysG, keysB);

                if (Keyboard.IsKeyDown(System.Windows.Input.Key.Home))
                {
                    connection.ExitSdkMode();
                    waveIn.StopRecording();
                    break;
                }
            }

            Console.ReadLine();
        }
        static void frequencyVisualizer(RyosTalkFXConnection connection, byte[] keys)
        {
            for (int i = 0; i < 110; i++)
            {
                keys[i] = 1;
            }
            //sconnection.SetMkFxKeyboardState(keys, toKeyboardColor(toMatrix(averageIntensity)), 1);
        }
        static void volumeVisualizer(RyosTalkFXConnection connection, byte[] keys, byte[] keysR, byte[] keysG, byte[] keysB)
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            var device = devices[0];
            int x = Convert.ToInt32(device.AudioMeterInformation.MasterPeakValue * 100);
            for (int i = 0; i < 110; i++)
            {
                if (i < x)
                {
                    keys[i] = 1;
                }
                else
                {
                    break;
                }

            }
            connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
            for (int i = 0; i < 110; i++)
            {
                if (i < x)
                {
                    keys[i] = 0;
                }
            }
        }
        static void randomVisualizer(RyosTalkFXConnection connection, byte[] keys, byte[] keysR, byte[] keysG, byte[] keysB)
        {
            Random rnd = new Random();
            for (int i = 0; i < 110; i++)
            {
                keys[i] = Convert.ToByte(rnd.Next(2));
                keysR[i] = Convert.ToByte(rnd.Next((int)(255 * maxBright)));
                keysG[i] = Convert.ToByte(rnd.Next((int)(255 * maxBright)));
                keysB[i] = Convert.ToByte(rnd.Next((int)(255 * maxBright)));
            }
            connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
        }
        static void fallenEffect()
        {
            byte[] keys = new byte[110];
            byte[] keysColor = new byte[110 * 3];
            for (int i = 0; i < 110; i++){keys[i] = 1;}
            Random rnd = new Random();
            int[] column = new int[23];
            for (int i = 0; i < 23; i++) {
                column[i] = rnd.Next((int)23);
                for (int ii = 0; ii < i+1; ii++)
                {
                    if (column[i] == column[ii])
                    {
                        column[i] = rnd.Next((int)23);
                    }
                }
            }
            for (int i = 0; i < 23; i++)
            {
                for (int row = 0; row < 6; row++)
                {
                    if ((row == 5 && i == 0) || (row == 5 && i == 2)
                        || (row == 5 && i == 19) || (row == 5 && i == 20) || (row == 5 && i == 21) || (row == 5 && i == 22)
                        || (row == 2 && i == 16) || (row == 2 && i == 17) || (row == 2 && i == 18)
                        || (row == 1 && i == 16) || (row == 1 && i == 18)
                        || (row == 2 && i == 22) || (row == 0 && i == 22))
                    {/*do nothing*/}

                    //keysColor[][]
                }
            }           
        }

        static IWaveIn waveIn;
        static int fftLength = 8192;
        static SampleAggregator sampleAggregator = new SampleAggregator(fftLength);
        static void record()
        {
            sampleAggregator.FftCalculated += new EventHandler<FftEventArgs>(fft);
            sampleAggregator.PerformFFT = true;
            waveIn = new WasapiLoopbackCapture();
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();
        }
        static void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            int bufferIncrement = waveIn.WaveFormat.BlockAlign;

            for (int index = 0; index < bytesRecorded; index += bufferIncrement)
            {
                float sample32 = BitConverter.ToSingle(buffer, index);
                sampleAggregator.Add(sample32);
            }
        }
        static void fft(object sender, FftEventArgs e)
        {
            float binSize = 44100 / 8192;
            int minBin = (int)(minHz / binSize);
            int maxBin = (int)(maxHz / binSize);
            float[] intensity = new float[1+maxBin - minBin];
            float[] frequency = new float[1+maxBin - minBin];
            for (int bin = minBin; bin <= maxBin; 
                bin++)
            {
                float real = e.Result[bin * 2].X;
                float imaginary = e.Result[bin * 2 + 1].Y;
                intensity[bin - minBin] = (real * real + imaginary * imaginary)*1000_000_000f;
                frequency[bin - minBin] = binSize * bin;
            }
            //binSize * bin - frequency; intensity - power
            float groupSize = (1 + maxBin - minBin) / kbw;
            float[] intensityAverage = new float[kbw];
            float[] frequencyAverage = new float[kbw];
            float sumF = 0;
            float sumI = 0;
            int averageIndex = 0;
            
            for (int i = 0; i < frequency.Length; i++)
            {
                sumF += frequency[i];
                sumI += intensity[i];
                if ((i+1) % groupSize == 0 && (i+1)/groupSize <= kbw)
                {
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

        static void toMatrixLight(float[] iA)
        {
            byte[][] Matrix = new byte[kbh][];
            for (int i = 0; i < kbh; i++)
            {
                Matrix[i] = new byte[kbw];
            }

            float point = 200 / kbh;
            for (int index = 0; index < kbw; index++)
            {
                float power = iA[index] / kbh * 10;
                int row = 0;
                for (float i = 0; i <= power && row < 6; i += point)
                {
                    Matrix[row][index] = ((i + point) < power) ? (byte)1 : (byte)0;
                    row++;
                }
            }
            toKeyboardLight(Matrix);
        }
        static void toMatrixColor(float[] iA)
        {
            float[][] Matrix = new float[kbh][];
            for (int i = 0; i < kbh; i++)
            {
                Matrix[i] = new float[kbw];
            }
            /*MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            var device = devices[0];
            float x = device.AudioMeterInformation.MasterPeakValue;*/
            float x = 0;
            for (int i = 1; i < iA.Length; i++)
            {
                x += iA[i];
                if (i == iA.Length-1)
                {
                    x /= i;
                }
            }
            float point = x/2;
            for (int index = 0; index < kbw; index++)
            {
                float power = iA[index];
                int row = 0;
                for (float i = 0; i <= power && row < 6; i+=point)
                {
                    Matrix[row][index] = ((i + point) < power) ? 1 : power / point;
                    row++;
                }
            }
            toKeyboardColor(Matrix);
        }
        static void toKeyboardLight(byte[][] Matrix)
        {
            byte[] keys = new byte[110];

            int kbi = 0;
            for (int row = kbh - 1; row >= 0; row--)
            {
                for (int i = 0; i < kbw; i++)
                {
                    if (i == 0)
                    {
                        if (row != 0)
                            keys[kbi] = (byte)Matrix[row][i] >= 1 ? (byte)1 : (byte)0;
                    }

                    else if (i == 1)
                    {
                        keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }

                    else if (i == 2)
                    {
                        if (row != 0)
                            keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }

                    else if (i == 16)
                    {
                        if (row != 1 || row != 2)
                            keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }

                    else if (i == 17)
                    {
                        if (row != 2)
                            keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }

                    else if (i == 18)
                    {
                        if (row != 1 || row != 2)
                            keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }

                    else if (i == 19)
                    {
                        if (row != 0)
                            keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }

                    else if (i == 20)
                    {
                        if (row != 0)
                            keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }

                    else if (i == 21)
                    {
                        if (row != 0)
                            keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }

                    else if (i == 22)
                    {
                        if (row != 0)
                            keys[kbi] = ((byte)Matrix[row][i] >= 1) ? (byte)1 : (byte)0;
                    }
                }
            }
            //Console.WriteLine(kbi);
            byte[] keysColor = new byte[330];
            for (int i = 0; i < 330; i++)
            {
                keysColor[i] = (byte)(red * maxBright);
                i++;
                keysColor[i] = (byte)(green * maxBright);
                i++;
                keysColor[i] = (byte)(blue * maxBright);
            }
            connection.SetMkFxKeyboardState(keys, keysColor, 1);
            //Thread.Sleep(10);
        }
        static void toKeyboardColor(float[][] Matrix)
        {
            byte[] keys = new byte[110];
            for (int i = 0; i < 110; i++)
            {
                keys[i] = 1;
            }
            byte[] keysColor = new byte[110 * 3];

            int kbi = 0;
            for (int row = kbh-1; row >= 0; row--)
            {
                for (int i = 0; i < kbw; i++)
                {
                    //non exist, skip / before & after ESC, above numpad, LED indicators, left & rigth button UP, bottom of numPLUS & numENTER
                    if ((row == 5 && i == 0) || (row == 5 && i == 2) 
                        || (row == 5 && i == 19) || (row == 5 && i == 20) || (row == 5 && i == 21) || (row == 5 && i == 22) 
                        || (row == 2 && i == 16) || (row == 2 && i == 17) || (row == 2 && i == 18) 
                        || (row == 1 && i == 16) || (row == 1 && i == 18)
                        || (row == 2 && i == 22) || (row == 0 && i == 22))
                    {/*do nothing*/}

                    //in center horizontal / F5, F6, F7, Q, W, E, R, T, Y, U, I, O, P, [, ], Windows, FN, AppendMenu
                    else if ( (row == 5 && i == 7) || (row == 5 && i == 8) || (row == 5 && i == 9)
                        || (row == 3 && i == 2) || (row == 3 && i == 3) || (row == 3 && i == 4) || (row == 3 && i == 5)
                        || (row == 3 && i == 6) || (row == 3 && i == 7) || (row == 3 && i == 8) || (row == 3 && i == 9)
                        || (row == 3 && i == 10) || (row == 3 && i == 11) || (row == 3 && i == 12) || (row == 3 && i == 13)
                        || (row == 0 && i == 2) || (row == 0 && i == 12) || (row == 0 && i == 13))
                    {
                        keysColor[kbi] = (byte)( ((red * Matrix[row][i] * 0.5) + (red * Matrix[row][i+1] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)( ((green * Matrix[row][i] * 0.5) + (green * Matrix[row][i+1] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)( ((blue * Matrix[row][i] * 0.5) + (blue * Matrix[row][i+1] * 0.5)) * maxBright);
                        kbi++;
                    }

                    //more right /  A, S, D, F, G, H, J, K, L, ;, ', leftALT
                    else if ((row == 2 && i == 2) || (row == 2 && i == 3) || (row == 2 && i == 4) || (row == 2 && i == 5)
                        || (row == 2 && i == 6) || (row == 2 && i == 7) || (row == 2 && i == 8) || (row == 2 && i == 9)
                        || (row == 2 && i == 10) || (row == 2 && i == 11) || (row == 2 && i == 12)
                        || (row == 0 && i == 3))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.25) + (red * Matrix[row][i+1] * 0.75)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.25) + (green * Matrix[row][i+1] * 0.75)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.25) + (blue * Matrix[row][i+1] * 0.75)) * maxBright);
                        kbi++;
                    }
                    //more left / Z, X, C, V, B, N, M, ,, ., /, ALTGR
                    else if ((row == 1 && i == 3) || (row == 1 && i == 4) || (row == 1 && i == 5) || (row == 1 && i == 6)
                        || (row == 1 && i == 7) || (row == 1 && i == 8) || (row == 1 && i == 9) || (row == 1 && i == 10)
                        || (row == 1 && i == 11) || (row == 1 && i == 12) || (row == 0 && i == 11))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.8) + (red * Matrix[row][i+1] * 0.2)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.8) + (green * Matrix[row][i+1] * 0.2)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.8) + (blue * Matrix[row][i+1] * 0.2)) * maxBright);
                        kbi++;
                    }

                    //in center horizontal without next center & 2 column sized / F8, Backspace, num0
                    else if ((row == 5 && i == 10) || (row == 4 && i == 14) || (row == 0 && i == 19))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.5) + (red * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.5) + (green * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.5) + (blue * Matrix[row][i + 1] * 0.5)) * maxBright);
                        kbi++;
                        i++;
                    }

                    //1.25 left sized / TAB, leftCTRL
                    else if ((row == 3 && i == 1) || (row == 0 && i == 1))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.75) + (red * Matrix[row][i + 1] * 0.25)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.75) + (green * Matrix[row][i + 1] * 0.25)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.75) + (blue * Matrix[row][i + 1] * 0.25)) * maxBright);
                        kbi++;
                    }

                    //1.25 right sized / \(above ENTER(US loadout)), rightCTRL
                    else if ((row == 3 && i == 14) || (row == 0 && i == 14))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.25) + (red * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.25) + (green * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.25) + (blue * Matrix[row][i + 1] * 0.75)) * maxBright);
                        kbi++;
                        i++;
                    }

                    //2 row sized / numPLUS, numENTER
                    else if ((row == 3 && i == 22) || (row == 1 && i == 22))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.5) + (red * Matrix[row-1][i] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.5) + (green * Matrix[row-1][i] * 0.5)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.5) + (blue * Matrix[row-1][i] * 0.5)) * maxBright);
                        kbi++;
                    }

                    //1,75 sized / CapsLock
                    else if ((row == 2 && i == 1))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.825) + (red * Matrix[row][i+1] * 0.125)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.825) + (green * Matrix[row][i+1] * 0.125)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.825) + (blue * Matrix[row][i+1] * 0.125)) * maxBright);
                        kbi++;
                    }

                    //2,15 sized left / leftSHIFT
                    else if ((row == 1 && i == 1))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.45) + (red * Matrix[row][i+1] * 0.45) + (red * Matrix[row][i+2] * 0.10)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.45) + (green * Matrix[row][i+1] * 0.45) + (green * Matrix[row][i+2] * 0.10)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.45) + (blue * Matrix[row][i+1] * 0.45) + (blue * Matrix[row][i+2] * 0.10)) * maxBright);
                        kbi++;
                        kbi+=3;
                        i++;
                    }

                    //2,15 sized right / ENTER
                    else if ((row == 2 && i == 13))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.10) + (red * Matrix[row][i+1] * 0.45) + (red * Matrix[row][i+2] * 0.45)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.10) + (green * Matrix[row][i+1] * 0.45) + (green * Matrix[row][i+2] * 0.45)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.10) + (blue * Matrix[row][i+1] * 0.45) + (blue * Matrix[row][i+2] * 0.45)) * maxBright);
                        kbi++;
                        i++;
                        i++;
                    }

                    //2,50 sized right / rightSHIFT
                    else if ((row == 1 && i == 13))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.2) + (red * Matrix[row][i+1] * 0.4) + (red * Matrix[row][i+2] * 0.4)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.2) + (green * Matrix[row][i+1] * 0.4) + (green * Matrix[row][i+2] * 0.4)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.2) + (blue * Matrix[row][i+1] * 0.4) + (blue * Matrix[row][i+2] * 0.4)) * maxBright);
                        kbi++;
                        i++;
                        i++;
                    }

                    //6,50 sized / SPACE
                    else if ((row == 0 && i == 4))
                    {
                        keysColor[kbi] = (byte)(((red * Matrix[row][i] * 0.1) + (red * Matrix[row][i + 1] * 0.15) + (red * Matrix[row][i + 2] * 0.15) + (red * Matrix[row][i + 3] * 0.15) + (red * Matrix[row][i + 4] * 0.15) + (red * Matrix[row][i + 5] * 0.15) + (red * Matrix[row][i + 6] * 0.15) + (red * Matrix[row][i + 7] * 0.05)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((green * Matrix[row][i] * 0.1) + (green * Matrix[row][i + 1] * 0.15) + (green * Matrix[row][i + 2] * 0.15) + (green * Matrix[row][i + 3] * 0.15) + (green * Matrix[row][i + 4] * 0.15) + (green * Matrix[row][i + 5] * 0.15) + (green * Matrix[row][i + 6] * 0.15) + (green * Matrix[row][i + 7] * 0.05)) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)(((blue * Matrix[row][i] * 0.1) + (blue * Matrix[row][i + 1] * 0.15) + (blue * Matrix[row][i + 2] * 0.15) + (blue * Matrix[row][i + 3] * 0.15) + (blue * Matrix[row][i + 4] * 0.15) + (blue * Matrix[row][i + 5] * 0.15) + (blue * Matrix[row][i + 6] * 0.15) + (blue * Matrix[row][i + 7] * 0.05)) * maxBright);
                        kbi++;
                        i+=6;
                    }

                    //normal
                    else
                    {
                        keysColor[kbi] = (byte)((red * Matrix[row][i]) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)((green * Matrix[row][i]) * maxBright);
                        kbi++;
                        keysColor[kbi] = (byte)((blue * Matrix[row][i]) * maxBright);
                        kbi++;
                    }
                    
                }
            }
            //Console.WriteLine(kbi);
            connection.SetMkFxKeyboardState(keys, keysColor, 1);
            //Thread.Sleep(10);
        }
    }
}
using Roccat_Talk.RyosTalkFX;
using RyosMKFXPanel.Animations;
using RyosMKFXPanel.Effects;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace RyosMKFXPanel {
    class Lightning {
        private static bool connected = false;
        public static bool getStatus() {
            return connected;
        }
        public static bool changeStatus() {
            connected = ((connected == false) ? true : false);
            return true;
        }

        public static byte red = 0;
        public static byte green = 255;
        public static byte blue = 255;
        public static float maxBright = 0.75f;
        public static bool changeLEDs = false;

        public static readonly int kbw = 23;
        public static readonly int kbh = 6;
        public static readonly int kbc = 110;

        public static int delay = 10;
        public static float speed = 3f;

        public static byte[] keysLight = new byte[kbc];
        public static byte[] keysColor = new byte[kbc*3];

        private static RyosTalkFXConnection connection = new RyosTalkFXConnection();



        /// <summary>
        /// Initiate connection to device
        /// </summary>
        public static bool connect() {
            if (connection.Initialize()) {
                return connection.EnterSdkMode();
            }
            return false;
        }

        /// <summary>
        /// Release connection to device
        /// </summary>
        public static bool disconnect() {
            return connection.ExitSdkMode();
        }



        /// <summary>
        /// Turn off all device keys 
        /// </summary>
        public static void keysLightReset() {
            for (int i = 0; i < kbc; i++) {
                keysLight[i] = 0;
            }
        }

        /// <summary>
        /// Turn on all device keys
        /// </summary>
        public static void keysLightAllOn() {
            for (int i = 0; i < kbc; i++) {
                keysLight[i] = 1;
            }
        }

        /// <summary>
        /// Set color of all device keys to 0
        /// </summary>
        public static void keysColorReset() {
            for (int i = 0; i < kbc * 3; i+=3) {
                keysColor[i] = 0;
                keysColor[i + 1] = 0;
                keysColor[i + 2] = 0;
            }
        }

        /// <summary>
        /// Update color array of device keys from variables
        /// </summary>
        public static void keysColorUpdate() {
            for (int i = 0; i < kbc * 3; i+=3) {
                keysColor[i] = (byte)(red * maxBright);
                keysColor[i+1] = (byte)(green * maxBright);
                keysColor[i+2] = (byte)(blue * maxBright);
            }
        }



        /// <summary>
        /// Send keys packet to device
        /// </summary>
        public static void sendPacket() {
            if (connected) {
                connection.SetMkFxKeyboardState(keysLight, keysColor, 1);
            }
        }

        /// <summary>
        /// Optimize packets for device (off unused keys)
        /// </summary>
        public static void optimizePacket() {
            for (int i = 0; i < keysLight.Length; i++) {
                if (keysColor[i*3] < 5 && keysColor[i*3+1] < 5 && keysColor[i*3+2] < 5) {
                    keysLight[i] = 0;
                }
            }
        }



        public static int prei = 0;

        public static bool effects = true;
        public static bool animations = false;
        public static bool custom = false;
        public static bool games = false;

        /// index of effect
        /// 0 - Equalizer, 1 - Volume, 2 - Random
        public static int ei = 0;
        /// 0 - Timer, 1 - Smiles, 2 - Waterfall, 3 - NyanCat
        public static int ai = 0;
        public static int ci = 0;
        public static int gi = 0;

        /// <summary>
        /// Turn off all effects from selected mode
        /// </summary>
        public static void offLightAlgs() {
            if (effects) {
                if (Equalizer.getState())
                    Equalizer.stop();
                else if (Volume.getState())
                    Volume.stop();
                else if (Effects.Random.getState())
                    Effects.Random.stop();

            } else if (animations) {
                if (Animations.Timer.getState())
                    Animations.Timer.stop();
                else if (Smiles.getState())
                    Smiles.stop();
                else if (Waterfall.getState())
                    Waterfall.stop();
                else if (NyanCat.getState())
                    NyanCat.stop();

            }
            Thread.Sleep(100);
        }

        /// <summary>
        /// Turn on specified effect from selected mode
        /// </summary>
        public static void onLightAlg() {
            if (effects) {
                if (ei == 0)
                    Equalizer.start();
                else if (ei == 1)
                    Volume.start();
                else if (ei == 2)
                    Effects.Random.start();

            } else if (animations) {
                if (ai == 0)
                    Animations.Timer.start();
                else if (ai == 1)
                    Smiles.start();
                else if (ai == 2)
                    Waterfall.start();
                else if (ai == 3)
                    NyanCat.start();

            }
        }




        /// <summary>
        /// Change status of only one mode to true
        /// </summary>
        /// <param name="mi">index of mode (0 - Effects, 1 - Animations, 2 - Custom, 3 - Games)</param>
        public static void changeMode(int mi) {
            offLightAlgs();
            effects = animations = custom = games = false;
            if (mi == 0)
                effects = true;
            else if (mi == 1)
                animations = true;
            else if (mi == 2)
                custom = true;
            else if (mi == 3)
                games = true;
            onLightAlg();
        }

        /// <summary>
        /// Turn off actual effects and turn on new one with actual mode
        /// </summary>
        /// <param name="algi">index of effect (0 - Equalizer/Timer, 1 - Volume/Smiles, 2 - Random/Waterfall, 3 - null/NyanCat)</param>
        public static void runNewAlg(int x) {
            offLightAlgs();
            if (effects) {
                ei = x;
            } else if (animations) {
                ai = x;
            } else if (custom) {
                ci = x;
            } else if (games) {
                gi = x;
            }
            onLightAlg();
        }

        /// <summary>
        /// Restart actual effect
        /// </summary>
        public static void restartAlg() {
            offLightAlgs();
            onLightAlg();
        }




        /// <summary>
        /// Prepare to work with device
        /// </summary>
        /// <returns>False if was started or can not connect too device, true if connected</returns>
        public static bool startWork() {
            if (!getStatus()) {
                if (connect()) {
                    if (changeStatus()) {
                        return true;
                    }
                } else {
                    MessageBox.Show("Can't start a work\n with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        /// <summary>
        /// End work with device
        /// </summary>
        /// <returns>False if wasn't started or can not release device, true if disconnected</returns>
        public static bool stopWork() {
            if (getStatus()) {
                if (disconnect()) {
                    if (changeStatus()) {
                        return true;
                    }
                } else {
                    MessageBox.Show("Can't stop a work\n with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        /// <summary>
        /// Switch combination of startWork() & stopWork()
        /// </summary>
        /// <returns>False if wasn't started or can not release device, true if disconnected</returns>
        public static bool switchWork() {
            if (!getStatus()) {
                if (connect()) {
                    if (changeStatus()) {
                        return true;
                    }
                } else {
                    MessageBox.Show("Can't start a work\n with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else {
                if (disconnect()) {
                    if (changeStatus()) {
                        return false;
                    }
                } else {
                    MessageBox.Show("Can't stop a work\n with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }



        /// <summary>
        /// Change delay (delay applies before send new packet to keyboard)
        /// </summary>
        /// <param name="delay">Delay in miliseconds</param>
        public static void changeDelay(int delay) {
            Lightning.delay = delay;
        }

        /// <summary>
        /// Change speed (division of 1 second)
        /// </summary>
        /// <param name="speed">Speed multiplier</param>
        public static void changeSpeed(float speed) {
            Lightning.speed = speed;
        }

        /// <summary>
        /// Change brightness of keys
        /// </summary>
        /// <param name="brightness">Brightness multiplier to color</param>
        public static void changeBrightness(float brightness) {
            Lightning.maxBright = brightness;
        }

        /// <summary>
        /// Change red color intensity
        /// </summary>
        /// <param name="red">1 byte number</param>
        public static void changeColorRed(byte red) {
            Lightning.red = red;
        }

        /// <summary>
        /// Change green color intensity
        /// </summary>
        /// <param name="green">1 byte number</param>
        public static void changeColorGreen(byte green) {
            Lightning.green = green;
        }

        /// <summary>
        /// Change blue color intensity
        /// </summary>
        /// <param name="blue">1 byte number</param>
        public static void changeColorBlue(byte blue) {
            Lightning.blue = blue;
        }

        /// <summary>
        /// Applies RGB code
        /// </summary>
        /// <param name="rgb">string of rgb code (0, 0, 255)/(#0000FF)</param>
        public static void changeColorRGB(string rgb) {
            Color color = (Color)ColorConverter.ConvertFromString(rgb);
            Lightning.red = color.R;
            Lightning.green = color.G;
            Lightning.blue = color.B;
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
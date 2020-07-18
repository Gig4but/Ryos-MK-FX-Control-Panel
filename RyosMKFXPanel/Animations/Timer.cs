using System.Diagnostics;
using System.Threading;

namespace RyosMKFXPanel.Animations {
    class Timer :Lightning {
        private static bool run = false;
        public static bool getState() {
            return run;
        }
        private static bool changeState() {
            run = ((run) ? false : true);
            return true;
        }

        private static Thread thread;
        public static void start() {
            thread = new Thread(animationTimerColor);
            thread.Start();
            changeState();
        }
        public static void stop() {
            thread.Abort();
            changeState();
        }

        private static bool invert = false;
        private static bool simple = false;
        public static void invertColor() {
            invert = ((invert) ? false : true);
        }
        public static void simpleNumbers() {
            simple = ((simple) ? false : true);
        }

        private static void animationTimerColor() {
            byte[] keys = new byte[110];
            byte[] keysR = new byte[110];
            byte[] keysG = new byte[110];
            byte[] keysB = new byte[110];
            int s = 0;
            int ss = 0;
            for (int i = 0; i < 110; i++) {
                if ((i > 17 && i < 27)
                    || i == 34 || i == 35 || i == 36
                    || i == 56 || i == 57 || i == 58
                    || i == 74 || i == 75 || i == 76
                    || i == 92 || i == 93 || i == 94
                    || i == 108 || i == 109) {
                    keys[i] = 1;
                }
            }
            while (true) {
                byte[] pattern = new byte[23];
                s++;
                if (invert) {
                    for (int i = 0; i < pattern.Length; i++) {
                        pattern[i] = 2;
                    }
                    if (simple) {
                        pattern[9] = 0;
                        pattern[10] = 0;
                        pattern[11] = 0;
                        pattern[21] = 0;
                        pattern[22] = 0;
                    }
                }
                if (s == 0) {
                    if (simple) {
                        pattern[21] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[10] = 1;
                        pattern[11] = 1;

                        pattern[12] = 1;
                        pattern[14] = 1;

                        pattern[15] = 1;
                        pattern[17] = 1;

                        pattern[18] = 1;
                        pattern[20] = 1;

                        pattern[21] = 1;
                        pattern[22] = 1;
                    }
                } 
                else if (s == 1) {
                    if (simple) {
                        pattern[18] = 1;
                    } else {
                        pattern[11] = 1;

                        pattern[13] = 1;
                        pattern[14] = 1;

                        pattern[15] = 1;
                        pattern[17] = 1;

                        pattern[20] = 1;

                        pattern[22] = 1;
                    }
                } 
                else if (s == 2) {
                    if (simple) {
                        pattern[19] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[10] = 1;
                        pattern[11] = 1;

                        pattern[14] = 1;

                        pattern[15] = 1;
                        pattern[16] = 1;
                        pattern[17] = 1;

                        pattern[18] = 1;

                        pattern[21] = 1;
                        pattern[22] = 1;
                    }
                } 
                else if (s == 3) {
                    if (simple) {
                        pattern[20] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[10] = 1;
                        pattern[11] = 1;

                        pattern[14] = 1;

                        pattern[15] = 1;
                        pattern[16] = 1;
                        pattern[17] = 1;

                        pattern[20] = 1;

                        pattern[21] = 1;
                        pattern[22] = 1;
                    }
                } 
                else if (s == 4) {
                    if (simple) {
                        pattern[15] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[11] = 1;

                        pattern[12] = 1;
                        pattern[14] = 1;

                        pattern[15] = 1;
                        pattern[16] = 1;
                        pattern[17] = 1;

                        pattern[20] = 1;

                        pattern[22] = 1;
                    }
                } 
                else if (s == 5) {
                    if (simple) {
                        pattern[16] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[10] = 1;
                        pattern[11] = 1;

                        pattern[12] = 1;

                        pattern[15] = 1;
                        pattern[16] = 1;
                        pattern[17] = 1;

                        pattern[20] = 1;

                        pattern[21] = 1;
                        pattern[22] = 1;
                    }
                } 
                else if (s == 6) {
                    if (simple) {
                        pattern[17] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[10] = 1;
                        pattern[11] = 1;

                        pattern[12] = 1;

                        pattern[15] = 1;
                        pattern[16] = 1;
                        pattern[17] = 1;

                        pattern[18] = 1;
                        pattern[20] = 1;

                        pattern[21] = 1;
                        pattern[22] = 1;
                    }
                } 
                else if (s == 7) {
                    if (simple) {
                        pattern[12] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[10] = 1;
                        pattern[11] = 1;

                        pattern[14] = 1;

                        pattern[17] = 1;

                        pattern[20] = 1;

                        pattern[22] = 1;
                    }
                } 
                else if (s == 8) {
                    if (simple) {
                        pattern[13] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[10] = 1;
                        pattern[11] = 1;

                        pattern[12] = 1;
                        pattern[14] = 1;

                        pattern[15] = 1;
                        pattern[16] = 1;
                        pattern[17] = 1;

                        pattern[18] = 1;
                        pattern[20] = 1;

                        pattern[21] = 1;
                        pattern[22] = 1;
                    }
                } 
                else if (s == 9) {
                    if (simple) {
                        pattern[14] = 1;
                    } else {
                        pattern[9] = 1;
                        pattern[10] = 1;
                        pattern[11] = 1;

                        pattern[12] = 1;
                        pattern[14] = 1;

                        pattern[15] = 1;
                        pattern[16] = 1;
                        pattern[17] = 1;

                        pattern[20] = 1;

                        pattern[21] = 1;
                        pattern[22] = 1;
                    }
                } 
                else if (s == 10) {
                    if (simple) {
                        pattern[18] = 1;
                    } else {
                        pattern[11] = 1;

                        pattern[13] = 1;
                        pattern[14] = 1;

                        pattern[15] = 1;
                        pattern[17] = 1;

                        pattern[20] = 1;

                        pattern[22] = 1;
                    }
                    s = 1;
                    ss++;
                }
                if (ss > 9) {
                    ss = 0;
                }
                for (int i = 0; i < ss; i++) {
                    pattern[i] = 1;
                }
                Thread.Sleep((int)(1000 / speed));
                connection.SetMkFxKeyboardState(keys, colorer(pattern), 1);
            }
        }

        private static byte[] colorer(byte[] pattern) {
            byte[] keysRGB = new byte[330];
            byte[] keyCodes = { 18, 19, 20, 21, 22, 23, 24, 25, 26, 34, 35, 36, 56, 57, 58, 74, 75, 76, 92, 93, 94, 108, 109 };
            for (int i = 0; i < pattern.Length; i++) {
                if (pattern[i] == 1) {
                    keysRGB[(keyCodes[i] * 3)] = (byte)(red * maxBright);
                    keysRGB[(keyCodes[i] * 3) + 1] = (byte)(green * maxBright);
                    keysRGB[(keyCodes[i] * 3) + 2] = (byte)(blue * maxBright);
                } else if (pattern[i] == 2) {
                    keysRGB[(keyCodes[i] * 3)] = (byte)((255 - red) * maxBright);
                    keysRGB[(keyCodes[i] * 3) + 1] = (byte)((255 - green) * maxBright);
                    keysRGB[(keyCodes[i] * 3) + 2] = (byte)((255 - blue) * maxBright);
                }
            }
            return keysRGB;
        }
    }
}
/*  xx  :  00  xx  01  02  03  04 :  05  06  07  08 :  09  10  11  12  :  13  14  15  :  xx xx  xx xx
    ....:.........................:.................:..................:..............:................
    16  :  17  18  19  20  21  22  23  24  25  26  27  28  29  3____0  :  31  32  33  :  34  35  36  37
    38  :  3___9 40  41  42  43  44  45  46  47  48  49  50  51 5___2  :  53  54  55  :  56  57  58  5x
    60  :  6____1 62  63  64  65  66  67  68  69  70  71  72  7_____3  :  nm  sh  sc  :  74  75  76  x9
    77  :  7_____8  80  81  82  83  84  85  86  87  88  89  9_______0  :  xx  91  xx  :  92  93  94  9y
    96  :  9___7 98  99  1______________________00  101  102 103 1_04  :  105 106 107 :  1___08 109  y5
*/
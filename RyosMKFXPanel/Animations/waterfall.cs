using System.Threading;

namespace RyosMKFXPanel.Animations {
    class Waterfall :Lightning {
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
            thread = new Thread(animationWaterfallColor);
            thread.Start();
            changeState();
        }
        public static void stop() {
            thread.Abort();
            changeState();
        }

        private static bool tail = false;
        public static void turnTail() {
            tail = ((tail) ? false : true);
        }
        public static float tailLength = 5;

        private static bool invert = false;
        public static void turnInvert() {
            invert = ((invert) ? false : true);
        }

        private static bool horizontal = false;
        public static void turnHorizontal() {
            horizontal = ((horizontal) ? false : true);
        }

        private static bool diagonal = false;
        public static void turnDiagonal() {
            diagonal = ((diagonal) ? false : true);
        }

        private static void animationWaterfallColor() {
            keysLightAllOn();
            float[][] Matrix = new float[kbh][];
            for (int i = 0; i < kbh; i++) {
                Matrix[i] = new float[kbw];
            }
            float x = 1f;
            int line = 0;
            while (true) {
                float p = (1 / tailLength);
                for (int i = 0; i < kbh; i++) {
                    for (int ii = 0; ii < kbw; ii++) {
                        Matrix[i][ii] = 0;
                    }
                }
                if (diagonal) {
                    if (horizontal) {

                    } else {
                        if (invert) {

                        } else {
                            for (int i = line; i < kbh; i++) {
                                for (int ii = 0; ii < kbw; ii++) {
                                    Matrix[i][ii] = x;
                                }
                                if (tail) {
                                    x = (x >= 1f) ? 1 / tailLength : x + 1 / tailLength;
                                    line = 0;
                                } else {
                                    line = (line >= 1) ? line - 1 : kbh - 1;
                                    i = kbh;
                                }
                            }
                        }
                    }
                } else {
                    if (horizontal) {

                    } else {
                        if (invert) {
                            for (int i = line; i >= 0; i--) {
                                for (int ii = 0; ii < kbw; ii++) {
                                    Matrix[i][ii] = x;
                                }
                                if (tail) {
                                    x = (x - p < p) ? 1f : x - p;
                                    line = kbh-1;
                                } else {
                                    line = (line < kbh-1) ? line + 1 : 0;
                                    i = -1;
                                    x = 1f;
                                }
                            }
                        } else {
                            for (int i = line; i < kbh; i++) {
                                for (int ii = 0; ii < kbw; ii++) {
                                    Matrix[i][ii] = x;
                                }
                                if (tail) {
                                    x = (x-p < p) ? 1f : x - p;
                                    line = 0;
                                } else {
                                    line = (line >= 1) ? line - 1 : kbh - 1;
                                    i = kbh;
                                    x = 1f;
                                }
                            }
                        }
                    }
                }
                int kbi = 0;
                for (int row = kbh - 1; row >= 0; row--) { // matrixToColor
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
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
            }
        }
    }
}
/*
    005  :  015  025  035  045  055  065  075  085  095  105  115  125  135  145  155  :  165  175  185  :  195  205  215  225
    .....:..............................:...................:..........................:.................:....................
    004  :  014  024  034  044  054  064  074  084  094  104  114  124  134  144  154  :  164  174  184  :  194  204  214  224
    003  :  013  023  033  043  053  063  073  083  093  103  113  123  133  143  153  :  163  173  183  :  193  203  213  223
    002  :  012  022  032  042  052  062  072  082  092  102  112  122  132  142  152  :  162  172  182  :  192  202  212  222
    001  :  011  021  031  041  051  061  071  081  091  101  111  121  131  141  151  :  161  171  181  :  191  201  211  221
    000  :  010  020  030  040  050  060  070  080  090  100  110  120  130  140  150  :  160  170  180  :  190  200  210  220
*/

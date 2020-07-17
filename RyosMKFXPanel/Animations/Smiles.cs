using System.Threading;

namespace RyosMKFXPanel.Animations {
    class Smiles : Lightning {
        private static bool run = false;
        public static bool getState() {
            return run;
        }
        private static bool changeState() {
            run = ((run == false) ? true : false);
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

        private static void animationTimerColor() {
            byte[] keys = new byte[110];
            byte[] keysR = new byte[110];
            byte[] keysG = new byte[110];
            byte[] keysB = new byte[110];

            while (true) {
                for (int i = 0; i < 110; i++) {
                    keysR[i] = red;
                }
                for (int i = 0; i < 110; i++) {
                    keysG[i] = green;
                }
                for (int i = 0; i < 110; i++) {
                    keysB[i] = blue;
                }
                for (int i = 0; i < 110; i++) {
                    keys[i] = 0;
                }

                //Fun
                keys[42] = 1;
                keys[43] = 1;
                keys[47] = 1;
                keys[48] = 1;
                keys[80] = 1;
                keys[100] = 1;
                keys[88] = 1;
                connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
                Thread.Sleep((int)(1000 / speed));
                for (int i = 0; i < 110; i++) {
                    keys[i] = 0;
                }

                //Calm
                keys[42] = 1;
                keys[43] = 1;
                keys[47] = 1;
                keys[48] = 1;
                keys[99] = 1;
                keys[100] = 1;
                keys[101] = 1;
                connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
                Thread.Sleep((int)(1000 / speed));
                for (int i = 0; i < 110; i++) {
                    keys[i] = 0;
                }

                //Sad
                keys[42] = 1;
                keys[43] = 1;
                keys[47] = 1;
                keys[48] = 1;
                keys[99] = 1;
                keys[81] = 1;
                keys[82] = 1;
                keys[83] = 1;
                keys[84] = 1;
                keys[85] = 1;
                keys[86] = 1;
                keys[87] = 1;
                keys[101] = 1;
                connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
                Thread.Sleep((int)(1000 / speed));
                for (int i = 0; i < 110; i++) {
                    keys[i] = 0;
                }

                //Angry
                keys[21] = 1;
                keys[43] = 1;
                keys[47] = 1;
                keys[26] = 1;
                keys[99] = 1;
                keys[100] = 1;
                keys[101] = 1;
                connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
                Thread.Sleep((int)(1000 / speed));
                for (int i = 0; i < 110; i++) {
                    keys[i] = 0;
                }

                //Kind
                keys[42] = 1;
                keys[21] = 1;
                keys[43] = 1;
                keys[47] = 1;
                keys[26] = 1;
                keys[48] = 1;
                keys[99] = 1;
                keys[100] = 1;
                keys[101] = 1;
                connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
                Thread.Sleep((int)(1000 / speed));
                for (int i = 0; i < 110; i++) {
                    keys[i] = 0;
                }

                //Wow
                keys[21] = 1;
                keys[20] = 1;
                keys[42] = 1;
                keys[43] = 1;
                keys[64] = 1;
                keys[65] = 1;
                keys[82] = 1;
                keys[81] = 1;
                keys[25] = 1;
                keys[26] = 1;
                keys[47] = 1;
                keys[48] = 1;
                keys[69] = 1;
                keys[70] = 1;
                keys[87] = 1;
                keys[88] = 1;
                keys[100] = 1;
                connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
                Thread.Sleep((int)(1000 / speed));
                for (int i = 0; i < 110; i++) {
                    keys[i] = 0;
                }
            }
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
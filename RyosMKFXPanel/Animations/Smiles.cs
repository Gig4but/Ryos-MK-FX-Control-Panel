using System.Threading;

namespace RyosMKFXPanel.Animations {
    class Smiles : Lightning {
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

        private static void animationTimerColor() {
            while (true) {
                keysLightReset();
                keysColorUpdate();

                //Fun
                keysLight[42] = 1;
                keysLight[43] = 1;
                keysLight[47] = 1;
                keysLight[48] = 1;
                keysLight[80] = 1;
                keysLight[100] = 1;
                keysLight[88] = 1;
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysLightReset();

                //Calm
                keysLight[42] = 1;
                keysLight[43] = 1;
                keysLight[47] = 1;
                keysLight[48] = 1;
                keysLight[99] = 1;
                keysLight[100] = 1;
                keysLight[101] = 1;
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysLightReset();

                //Sad
                keysLight[42] = 1;
                keysLight[43] = 1;
                keysLight[47] = 1;
                keysLight[48] = 1;
                keysLight[99] = 1;
                keysLight[81] = 1;
                keysLight[82] = 1;
                keysLight[83] = 1;
                keysLight[84] = 1;
                keysLight[85] = 1;
                keysLight[86] = 1;
                keysLight[87] = 1;
                keysLight[101] = 1;
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysLightReset();

                //Angry
                keysLight[21] = 1;
                keysLight[43] = 1;
                keysLight[47] = 1;
                keysLight[26] = 1;
                keysLight[99] = 1;
                keysLight[100] = 1;
                keysLight[101] = 1;
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysLightReset();

                //Kind
                keysLight[42] = 1;
                keysLight[21] = 1;
                keysLight[43] = 1;
                keysLight[47] = 1;
                keysLight[26] = 1;
                keysLight[48] = 1;
                keysLight[99] = 1;
                keysLight[100] = 1;
                keysLight[101] = 1;
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysLightReset();

                //Wow
                keysLight[21] = 1;
                keysLight[20] = 1;
                keysLight[42] = 1;
                keysLight[43] = 1;
                keysLight[64] = 1;
                keysLight[65] = 1;
                keysLight[82] = 1;
                keysLight[81] = 1;
                keysLight[25] = 1;
                keysLight[26] = 1;
                keysLight[47] = 1;
                keysLight[48] = 1;
                keysLight[69] = 1;
                keysLight[70] = 1;
                keysLight[87] = 1;
                keysLight[88] = 1;
                keysLight[100] = 1;
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysLightReset();
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
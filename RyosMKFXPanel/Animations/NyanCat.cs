using System.Threading;

namespace RyosMKFXPanel.Animations {
    class NyanCat :Lightning {
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
            thread = new Thread(animationNyanCat);
            thread.Start();
            changeState();
        }
        public static void stop() {
            thread.Abort();
            changeState();
        }
        
        private static void animationNyanCat() {
            keysLightReset();
            keysColorReset();
            while (true) {
                int[] keys = {20,21,22,23,24,25,41,42,43,44,45,46,47,63,64,65,66,67,68,69,80,81,82,83,84,85,86};
                for (int i = 0; i < keys.Length; i++) {
                    keysLight[keys[i]] = 1;
                }
                //frame 1
                //____tail
                keysColor[20*3] = (byte)(200 * maxBright);
                keysColor[21*3] = (byte)(200 * maxBright);
                keysColor[22*3] = (byte)(200 * maxBright);
                keysColor[41*3+1] = (byte)(200 * maxBright);
                keysColor[42*3+1] = (byte)(200 * maxBright);
                keysColor[43*3+1] = (byte)(200 * maxBright);
                keysColor[63*3+2] = (byte)(200 * maxBright);
                keysColor[64*3+2] = (byte)(200 * maxBright);
                keysColor[65*3+2] = (byte)(200 * maxBright);

                //____body
                keysColor[23 * 3] = (byte)(219 * maxBright);
                keysColor[23 * 3+1] = (byte)(112 * maxBright);
                keysColor[23 * 3+2] = (byte)(147 * maxBright);
                keysColor[24 * 3] = (byte)(219 * maxBright);
                keysColor[24 * 3+1] = (byte)(112 * maxBright);
                keysColor[24 * 3+2] = (byte)(147 * maxBright);
                keysColor[25 * 3] = (byte)(219 * maxBright);
                keysColor[25 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[25 * 3 + 2] = (byte)(147 * maxBright);

                keysColor[44 * 3] = (byte)(219 * maxBright);
                keysColor[44 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[44 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[45 * 3] = (byte)(255 * maxBright);
                keysColor[45 * 3 + 1] = (byte)(20 * maxBright);
                keysColor[45 * 3 + 2] = (byte)(147 * maxBright);

                keysColor[66 * 3] = (byte)(219 * maxBright);
                keysColor[66 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[66 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[67 * 3] = (byte)(219 * maxBright);
                keysColor[67 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[67 * 3 + 2] = (byte)(147 * maxBright);
                
                //____head
                keysColor[46 * 3] = (byte)(192 * maxBright);
                keysColor[46 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[46 * 3 + 2] = (byte)(192 * maxBright);
                keysColor[47 * 3] = (byte)(192 * maxBright);
                keysColor[47 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[47 * 3 + 2] = (byte)(192 * maxBright);

                keysColor[68 * 3] = (byte)(192 * maxBright);
                keysColor[68 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[68 * 3 + 2] = (byte)(192 * maxBright);
                keysColor[69 * 3] = (byte)(192 * maxBright);
                keysColor[69 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[69 * 3 + 2] = (byte)(192 * maxBright);
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysColorReset();


                //frame 2
                //____tail
                keysColor[20 * 3] = (byte)(200 * maxBright);
                keysColor[21 * 3] = (byte)(200 * maxBright);
                keysColor[22 * 3] = (byte)(200 * maxBright);
                keysColor[41 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[42 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[43 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[63 * 3 + 2] = (byte)(200 * maxBright);
                keysColor[64 * 3 + 2] = (byte)(200 * maxBright);
                keysColor[65 * 3 + 2] = (byte)(200 * maxBright);

                //____body
                keysColor[44 * 3] = (byte)(219 * maxBright);
                keysColor[44 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[44 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[45 * 3] = (byte)(219 * maxBright);
                keysColor[45 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[45 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[46 * 3] = (byte)(219 * maxBright);
                keysColor[46 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[46 * 3 + 2] = (byte)(147 * maxBright);

                keysColor[66 * 3] = (byte)(219 * maxBright);
                keysColor[66 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[66 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[67 * 3] = (byte)(255 * maxBright);
                keysColor[67 * 3 + 1] = (byte)(20 * maxBright);
                keysColor[67 * 3 + 2] = (byte)(147 * maxBright);

                keysColor[83 * 3] = (byte)(219 * maxBright);
                keysColor[83 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[83 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[84 * 3] = (byte)(219 * maxBright);
                keysColor[84 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[84 * 3 + 2] = (byte)(147 * maxBright);

                //____head
                keysColor[68 * 3] = (byte)(192 * maxBright);
                keysColor[68 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[68 * 3 + 2] = (byte)(192 * maxBright);
                keysColor[69 * 3] = (byte)(192 * maxBright);
                keysColor[69 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[69 * 3 + 2] = (byte)(192 * maxBright);

                keysColor[85 * 3] = (byte)(192 * maxBright);
                keysColor[85 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[85 * 3 + 2] = (byte)(192 * maxBright);
                keysColor[86 * 3] = (byte)(192 * maxBright);
                keysColor[86 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[86 * 3 + 2] = (byte)(192 * maxBright);
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysColorReset();


                //frame3
                //____tail
                keysColor[41 * 3] = (byte)(200 * maxBright);
                keysColor[42 * 3] = (byte)(200 * maxBright);
                keysColor[43 * 3] = (byte)(200 * maxBright);
                keysColor[63 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[64 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[65 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[80 * 3 + 2] = (byte)(200 * maxBright);
                keysColor[81 * 3 + 2] = (byte)(200 * maxBright);
                keysColor[82 * 3 + 2] = (byte)(200 * maxBright);

                //____body
                keysColor[44 * 3] = (byte)(219 * maxBright);
                keysColor[44 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[44 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[45 * 3] = (byte)(219 * maxBright);
                keysColor[45 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[45 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[46 * 3] = (byte)(219 * maxBright);
                keysColor[46 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[46 * 3 + 2] = (byte)(147 * maxBright);

                keysColor[66 * 3] = (byte)(219 * maxBright);
                keysColor[66 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[66 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[67 * 3] = (byte)(255 * maxBright);
                keysColor[67 * 3 + 1] = (byte)(20 * maxBright);
                keysColor[67 * 3 + 2] = (byte)(147 * maxBright);

                keysColor[83 * 3] = (byte)(219 * maxBright);
                keysColor[83 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[83 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[84 * 3] = (byte)(219 * maxBright);
                keysColor[84 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[84 * 3 + 2] = (byte)(147 * maxBright);

                //____head
                keysColor[68 * 3] = (byte)(192 * maxBright);
                keysColor[68 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[68 * 3 + 2] = (byte)(192 * maxBright);
                keysColor[69 * 3] = (byte)(192 * maxBright);
                keysColor[69 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[69 * 3 + 2] = (byte)(192 * maxBright);

                keysColor[85 * 3] = (byte)(192 * maxBright);
                keysColor[85 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[85 * 3 + 2] = (byte)(192 * maxBright);
                keysColor[86 * 3] = (byte)(192 * maxBright);
                keysColor[86 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[86 * 3 + 2] = (byte)(192 * maxBright);
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysColorReset();


                //frame 4
                //____tail
                keysColor[41 * 3] = (byte)(200 * maxBright);
                keysColor[42 * 3] = (byte)(200 * maxBright);
                keysColor[43 * 3] = (byte)(200 * maxBright);
                keysColor[63 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[64 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[65 * 3 + 1] = (byte)(200 * maxBright);
                keysColor[80 * 3 + 2] = (byte)(200 * maxBright);
                keysColor[81 * 3 + 2] = (byte)(200 * maxBright);
                keysColor[82 * 3 + 2] = (byte)(200 * maxBright);

                //____body
                keysColor[23 * 3] = (byte)(219 * maxBright);
                keysColor[23 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[23 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[24 * 3] = (byte)(219 * maxBright);
                keysColor[24 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[24 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[25 * 3] = (byte)(219 * maxBright);
                keysColor[25 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[25 * 3 + 2] = (byte)(147 * maxBright);

                keysColor[44 * 3] = (byte)(219 * maxBright);
                keysColor[44 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[44 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[45 * 3] = (byte)(255 * maxBright);
                keysColor[45 * 3 + 1] = (byte)(20 * maxBright);
                keysColor[45 * 3 + 2] = (byte)(147 * maxBright);

                keysColor[66 * 3] = (byte)(219 * maxBright);
                keysColor[66 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[66 * 3 + 2] = (byte)(147 * maxBright);
                keysColor[67 * 3] = (byte)(219 * maxBright);
                keysColor[67 * 3 + 1] = (byte)(112 * maxBright);
                keysColor[67 * 3 + 2] = (byte)(147 * maxBright);

                //____head
                keysColor[46 * 3] = (byte)(192 * maxBright);
                keysColor[46 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[46 * 3 + 2] = (byte)(192 * maxBright);
                keysColor[47 * 3] = (byte)(192 * maxBright);
                keysColor[47 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[47 * 3 + 2] = (byte)(192 * maxBright);

                keysColor[68 * 3] = (byte)(192 * maxBright);
                keysColor[68 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[68 * 3 + 2] = (byte)(192 * maxBright);
                keysColor[69 * 3] = (byte)(192 * maxBright);
                keysColor[69 * 3 + 1] = (byte)(192 * maxBright);
                keysColor[69 * 3 + 2] = (byte)(192 * maxBright);
                sendPacket();
                Thread.Sleep((int)(1000 / speed));
                keysColorReset();
            }
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
*/

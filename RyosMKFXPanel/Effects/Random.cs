using System;
using System.Threading;

namespace RyosMKFXPanel.Effects {
    class Random : Lightning {
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
            thread = new Thread(effectRandom);
            thread.Start();
            changeState();
        }
        public static void stop() {
            thread.Abort();
            changeState();
        }
        private static bool turn = false;
        public static void LEDturn() {
            turn = ((turn) ? false : true);
        }

        private static void effectRandom() {
            System.Random rnd = new System.Random();
            while (true) {
                for (int i = 0; i < kbc; i++) {
                    if (turn) {
                        keysLight[i] = Convert.ToByte(rnd.Next(2));
                    } else {
                        keysLight[i] = 1;
                    }
                }
                for (int i = 0; i < kbc * 3; i+=3) {
                    keysColor[i] = Convert.ToByte(rnd.Next((int)(red * maxBright)));
                    keysColor[i+1] = Convert.ToByte(rnd.Next((int)(green * maxBright)));
                    keysColor[i+2] = Convert.ToByte(rnd.Next((int)(blue * maxBright)));
                }
                Thread.Sleep(delay);
                sendPacket();
            }
        }
    }
}

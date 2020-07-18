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
            byte[] keys = new byte[110];

            byte[] keysR = new byte[110];
            byte[] keysG = new byte[110];
            byte[] keysB = new byte[110];

            System.Random rnd = new System.Random();
            while (true) {
                for (int i = 0; i < 110; i++) {
                    if (turn) {
                        keys[i] = Convert.ToByte(rnd.Next(2));
                    } else {
                        keys[i] = 1;
                    }
                    keysR[i] = Convert.ToByte(rnd.Next((int)(red * maxBright)));
                    keysG[i] = Convert.ToByte(rnd.Next((int)(green * maxBright)));
                    keysB[i] = Convert.ToByte(rnd.Next((int)(blue * maxBright)));
                }
                Thread.Sleep(delay);
                connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
            }
        }
    }
}

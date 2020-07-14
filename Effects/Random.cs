using System;
using System.Threading;

namespace RyosMKFXPanel.Effects {
    class Random : Lightning {
        private static Thread thread;
        public static void start() {
            thread = new Thread(effectRandom);
            thread.Start();
        }
        public static void stop() {
            thread.Abort();
        }
        private static bool on = false;
        private static int r = 2;
        public static void LEDturn() {
            if (on) {
                on = false;
            } else {
                on = true;
            }
        }

        private static void effectRandom() {
            byte[] keys = new byte[110];

            byte[] keysR = new byte[110];
            byte[] keysG = new byte[110];
            byte[] keysB = new byte[110];

            System.Random rnd = new System.Random();
            while (true) {
                for (int i = 0; i < 110; i++) {
                    keys[i] = Convert.ToByte(rnd.Next(r));
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

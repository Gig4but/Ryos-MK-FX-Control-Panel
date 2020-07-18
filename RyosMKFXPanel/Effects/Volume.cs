using NAudio.CoreAudioApi;
using System;
using System.Threading;

namespace RyosMKFXPanel.Effects {
    class Volume : Lightning {
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
            thread = new Thread(effectVolume);
            thread.Start();
            changeState();
        }
        public static void stop() {
            thread.Abort();
            changeState();
        }

        private static bool simple = false;
        public static void simpleVolume() {
            simple = ((simple) ? false : true);
        }

        private static void effectVolume() {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            var device = devices[0];
            int v = 0;

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
                v = Convert.ToInt32(device.AudioMeterInformation.MasterPeakValue * 110);
                if (simple) {
                    v /= 11;
                    for (int i = 18; i < v+18; i++) {
                        keys[i] = 1;
                    }
                } else {
                    for (int i = 0; i < v; i++) {
                        keys[i] = 1;
                    }
                }
                Thread.Sleep(delay);
                connection.SetMkFxKeyboardState(keys, keysR, keysG, keysB, 1);
            }
        }
    }
}

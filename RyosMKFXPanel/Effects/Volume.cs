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

            while (true) {
                keysLightReset();
                keysColorUpdate();
                v = Convert.ToInt32(device.AudioMeterInformation.MasterPeakValue * kbc);
                if (simple) {
                    v /= 11;
                    for (int i = 18; i < v+18; i++) {
                        keysLight[i] = 1;
                    }
                } else {
                    for (int i = 0; i < v; i++) {
                        keysLight[i] = 1;
                    }
                }
                Thread.Sleep(delay);
                sendPacket();
            }
        }
    }
}

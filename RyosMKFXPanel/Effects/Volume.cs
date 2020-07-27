using NAudio.CoreAudioApi;
using System;
using System.Linq;
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
        private static int v = 0;
        
        private static void effectVolume() {
            while (true) {
                Thread.Sleep(delay);
                keysLightReset();
                keysColorUpdate();
                devicePlayCheck();
                

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
                sendPacket();
            }
        }

        private static int check = 0;
        private static int deviceID = 0;
        public static MMDevice devicePlayCheck() {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDeviceCollection devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            MMDevice device = devices[deviceID];
            v = Convert.ToInt32(device.AudioMeterInformation.MasterPeakValue * kbc);
            if (v == 0) {
                check++;
            } else {
                check = 0;
            }
            if (check >= 3) {
                if (deviceID < devices.Count - 1) {
                    deviceID++;
                    device = devices[deviceID];
                } else if (deviceID >= devices.Count - 1) {
                    deviceID = 0;
                    device = devices[deviceID];
                }
            }
            return device;
        }
    }
}

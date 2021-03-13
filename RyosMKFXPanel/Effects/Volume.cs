using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace RyosMKFXPanel.Effects {
    class Volume :Lightning {
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
        private static double v = 0;

        private static void effectVolume() {
            while (true) {
                Thread.Sleep(delay);
                keysLightReset();
                keysColorUpdate();

                //v = Math.Round(enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[deviceID].AudioMeterInformation.MasterPeakValue * kbc);
                v = Math.Round(volumeIs() * kbc);
                if (simple) {
                    v /= 11;
                    for (int i = 18; i < v + 18; i++) {
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

        private static bool autoDevice = true;
        private static MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        private static int deviceID = 0;
        
        public static MMDeviceCollection getAudioDevices() {
            return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        }
        public static void changeListenDeviceByIndex(int index) {
            deviceID = index;
        }
        public static bool changeListenDeviceById(string id) {
            MMDeviceCollection devices = getAudioDevices();
            for (int i = 0; i < devices.Count; i++) {
                if (devices[i].ID == id) {
                    deviceID = i;
                    return true;
                }
            }
            return false;
        }
        public static MMDevice getListenDevice() {
            MMDeviceCollection devices = getAudioDevices();
            if (deviceID < devices.Count && devices.Count > 0) {
                if (autoDevice) {
                    if (!devicePlayCheck()) {
                        deviceID = 0;
                    }
                }
                //return device;
                return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[deviceID];
            }
            Environment.Exit(1);
            return null;
        }
        public static void deviceAutoChange(bool auto) {
            autoDevice = auto;
        }
        public static bool deviceAutoChange() {
            return autoDevice;
        }
        public static bool devicePlayCheck() {
            MMDeviceCollection devices = getAudioDevices();
            for (int i = 0; i < devices.Count; i++) {
                if (devices[i].AudioMeterInformation.MasterPeakValue != 0) {
                    deviceID = i;
                    return true;
                }
            }
            return false;
        }
        public static double volumeIs() {
            return getListenDevice().AudioMeterInformation.MasterPeakValue;
            //return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[deviceID].AudioMeterInformation.MasterPeakValue;
        }
        public static List<string> getAudioDevicesNames() {
            List<string> names = new List<string>();
            foreach (MMDevice i in getAudioDevices()) {
                names.Add(i.FriendlyName);
            }
            return names;
        }
        public static int getAudioDeviceIndex() {
            return deviceID;
        }
        public static int getAudioDeviceIndexByID(string id) {
            MMDeviceCollection devices = getAudioDevices();
            for (int i = 0; i < devices.Count; i++) {
                if (devices[i].ID == id) {
                    return i;
                }
            }
            return 0;
        }
    }
}

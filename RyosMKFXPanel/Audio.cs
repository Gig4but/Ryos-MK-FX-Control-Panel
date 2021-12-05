using NAudio.CoreAudioApi;
using System.Collections.Generic;
using static RyosMKFXPanel.Settings;

namespace RyosMKFXPanel {
	static class Audio {
		private static MMDeviceEnumerator _enumerator = new MMDeviceEnumerator();
		/// <summary>
		/// Get a list of working devices
		/// </summary>
		public static MMDeviceCollection GetAudioDevices() {
			return _enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
		}
		public static List<string> GetAudioDevicesNames() {
			List<string> names = new List<string>();
			foreach (MMDevice i in GetAudioDevices()) {
				names.Add(i.FriendlyName);
			}
			return names;
		}

		private static int _deviceID = 0;
		public static int GetAudioDeviceIndex() {
			return _deviceID;
		}
		public static void ChangeListenDeviceByIndex(int index) {
			_deviceID = index;
		}


		/// <summary>
		/// Find working device via HW ID
		/// </summary>
		public static bool ChangeListenDeviceByHWID(string id) {
			MMDeviceCollection devices = GetAudioDevices();
			for (int i = 0; i < devices.Count; i++) {
				if (devices[i].ID == id) {
					_deviceID = i;
					return true;
				}
			}
			return false;
		}
		public static int GetAudioDeviceIndexByHWID(string id) {
			MMDeviceCollection devices = GetAudioDevices();
			for (int i = 1; i < devices.Count; i++) {
				if (devices[i].ID == id) {
					return i;
				}
			}
			return 0;
		}



		private static bool _autoDevice = true;
		public static void DeviceAutoChange(bool auto) {
			_autoDevice = auto;
		}
		public static bool DeviceAutoChange() {
			return _autoDevice;
		}

		/// <summary>
		/// Check if volume of actual device is zero
		/// </summary>
		public static bool DevicePlayCheck() {
			MMDeviceCollection devices = GetAudioDevices();
			for (int i = 0; i < devices.Count; i++) {
				if (devices[i].AudioMeterInformation.MasterPeakValue != 0) {
					_deviceID = i;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Get device for listening after variable volume check
		/// </summary>
		public static MMDevice GetListenDevice() {
			MMDeviceCollection devices = GetAudioDevices();
			if (_deviceID < devices.Count && devices.Count > 0) {
				if (_autoDevice) {
					if (!DevicePlayCheck()) {
						_deviceID = 0;
					}
				}
				return _enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[_deviceID];
			}
			ErrorProcessor.Error(10);
			return null;
		}
		


		public static double VolumeIs() {
			return GetListenDevice().AudioMeterInformation.MasterPeakValue;
		}



	}
}

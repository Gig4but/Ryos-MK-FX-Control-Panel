using NAudio.CoreAudioApi;

namespace RyosMKFXPanel {
	/// <summary>
	/// Represents work with audio devices in system.
	/// </summary>
	static class Audio {
		/// <summary>
		/// Get a list of working devices.
		/// </summary>
		/// <returns>
		/// A collection of devices.
		/// </returns>
		public static MMDeviceCollection GetAudioDevices() { 
			return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active); 
		}

		/// <summary>
		/// Get a list of working devices.
		/// </summary>
		/// <returns>
		/// An array of devices names.
		/// </returns>
		public static string[] GetAudioDevicesNames() {
			MMDeviceCollection devices = GetAudioDevices();
			string[] names = new string[devices.Count];
			for (int i = 0; i < names.Length; i++)
				names[i] = devices[i].FriendlyName;
			return names;
		}



		/// <summary>
		/// Index of device to be worked with.
		/// </summary>
		private static int _device = 0;

		/// <summary>
		/// Returns index of device to be worked with.
		/// </summary>
		/// <returns>
		/// The index of device to be worked with.
		/// </returns>
		public static int GetListeningDevice() { return _device; }

		/// <summary>
		/// Changes the index of device to be worked with.
		/// </summary>
		/// <param name="index">the new device index.</param>
		public static void ChangeListeningDevice(int index) { _device = index; }

		/// <summary>
		/// Finds index of device by id.
		/// </summary>
		/// <param name="id">the id of device.</param>
		/// <returns>
		/// The index of device to be worked with.
		/// -1 if not finded.
		/// </returns>
		public static int GetListeningDevice(string id) {
			MMDeviceCollection devices = GetAudioDevices();
			for (int i = 0; i < devices.Count; i++)
				if (devices[i].ID == id)
					return i;
			return -1;
		}

		/// <summary>
		/// Changes index of device by id.
		/// </summary>
		/// <param name="id">the id of device.</param>
		/// <returns>
		/// True if device id was finded.
		/// </returns>
		public static bool ChangeListeningDevice(string id) {
			MMDeviceCollection devices = GetAudioDevices();
			for (int i = 0; i < devices.Count; i++) {
				if (devices[i].ID == id) {
					_device = i;
					return true;
				}
			}
			return false;
		}



		/// <summary>
		/// Displays if algorithm can skip muted/zero volume device to active one.
		/// </summary>
		private static bool _auto = true;

		/// <summary>
		/// Returns the state of auto change variable.
		/// </summary>
		/// <returns>
		/// The state of auto change variable.
		/// </returns>
		public static bool DeviceAutoChange() { return _auto; }

		/// <summary>
		/// Changes the state of auto change variable.
		/// </summary>
		/// <param name="auto">the new state.</param>
		public static void DeviceAutoChange(bool auto) { _auto = auto; }



		/// <summary>
		/// Checks if volume of actual device isn't zero (if device is active).
		/// </summary>
		/// <returns>
		/// True if active device finded.
		/// </returns>
		public static bool DevicePlayCheck() {
			MMDeviceCollection devices = GetAudioDevices();
			for (int i = 0; i < devices.Count; i++) {
				if (devices[i].AudioMeterInformation.MasterPeakValue != 0) {
					_device = i;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Finds instance of listenig device.
		/// If devices aren't exist or device id is wrong, shows Error window with code 0x0A.
		/// </summary>
		/// <returns>
		/// Instance of listening device.
		/// </returns>
		public static MMDevice? GetActiveDevice() {
			MMDeviceCollection devices = GetAudioDevices();
			if (_auto && !DevicePlayCheck())
				_device = 0;
			if (devices.Count > 0 && _device < devices.Count)
				return devices[_device];
			ErrorProcessor.Error(0x0A);
			return null;
		}



		/// <summary>
		/// Gets volume of active device.
		/// </summary>
		/// <returns>
		/// The volume of active device.
		/// </returns>
		public static double GetVolume() {
			MMDevice? device = GetActiveDevice();
			return device == null ? 0 : device.AudioMeterInformation.MasterPeakValue;
		}
	}
}

using RyosMKFXPanel.Devices.Roccat;
using System.Threading;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RyosMKFXPanel {
	class Lightning :ISettings {
		/*
		public enum Device {
			RyosMKFX,
			CorsairK95
		}
		public Lightning.deviceType device;
		*/
		//fiction of future device manager
		private static Lightning[] GetAvailableDevices() {
			return new Lightning[] { new RyosMKFX() };
		}
		public static Lightning[] devices = GetAvailableDevices();

		public static IEnumerable<Type> modules = typeof(LightningModule).Assembly.GetTypes().Where(t => t.BaseType == typeof(LightningModule));
		public static LightningModule activeModule = (LightningModule)Activator.CreateInstance(modules.ElementAt(0));


		public virtual bool connected { get; set; }

		public float red = 0f;
		public float green = 255f;
		public float blue = 255f;
		public float bright = 0.75f;

		public int delay = 3;
		public float speed = 3f;

		public Dictionary<string, string> GetSettings() {
			return new Dictionary<string, string> {
				{ "red", devices[0].red.ToString() },
				{ "green", devices[0].green.ToString() },
				{ "blue", devices[0].blue.ToString() },
				{ "bright", devices[0].bright.ToString() },
				{ "speed", devices[0].speed.ToString() }
			};
		}
		public void LoadSettings(in Dictionary<string, string> settings) {
			red = float.Parse(settings["red"]);
			green = float.Parse(settings["green"]);
			blue = float.Parse(settings["blue"]);
			bright = float.Parse(settings["bright"]);
			speed = float.Parse(settings["speed"]);
		}



		public virtual int GetWidth() {
			return 22;
		}
		public virtual int GetHeight() {
			return 6;
		}
		public virtual int GetKeysCount() {
			return 105;
		}


		public byte[] keysLight;
		public byte[] keysLightOld;
		public byte[] keysColor;
		public byte[] keysColorOld;

		/// <summary>
		/// Return object name
		/// </summary>
		public virtual string GetDeviceUID() {
			return "unknown";
		}

		/// <summary>
		/// Initiate connection to device
		/// </summary>
		public virtual bool Connect() {
			return connected = true;
		}

		/// <summary>
		/// Release connection to device
		/// </summary>
		public virtual bool Disconnect() {
			connected = false;
			return true;
		}



		/// <summary>
		/// Turn off all device keys 
		/// </summary>
		public virtual void KeysLightAllOff() {
			for (int i = 0; i < keysLight.Length; i++)
				keysLight[i] = 0;
		}

		/// <summary>
		/// Turn on all device keys
		/// </summary>
		public virtual void KeysLightAllOn() {
			for (int i = 0; i < keysLight.Length; i++)
				keysLight[i] = 1;
		}
		public virtual void KeyLightSetState(int key, byte state) {
			keysLight[key] = state;
		}
		public virtual void KeyLightOff(int key) {
			keysLight[key] = 0;
		}
		public virtual void KeyLightOn(int key) {
			keysLight[key] = 1;
		}
		public virtual void KeysLightOn(int[] keys) {
			for (int i = 0; i < keys.Length; i++)
				keysLight[keys[i]] = 1;
		}
		public virtual void KeysLightOn(int fromkey, int tokey) {
			for (int i = fromkey; i < tokey+1; i++)
				keysLight[i] = 1;
		}
		public virtual void KeysLightChangeByMatrix(float[][] Matrix) {}

		/// <summary>
		/// Set color of all keys to 0 (black)
		/// </summary>
		public virtual void KeysColorReset() {
			for (int i = 0; i < GetKeysCount(); i++) {
				keysColor[i * 3] = 0;
				keysColor[i * 3 + 1] = 0;
				keysColor[i * 3 + 2] = 0;
			}
		}

		/// <summary>
		/// Update keys Color and Brightness
		/// </summary>
		public virtual void KeysColorUpdate() {
			for (int i = 0; i < GetKeysCount(); i++) {
				keysColor[i * 3] = (byte)(red * bright);
				keysColor[i * 3 + 1] = (byte)(green * bright);
				keysColor[i * 3 + 2] = (byte)(blue * bright);
			}
		}

		/// <summary>
		/// Update key Color and Brightness
		/// </summary>
		public virtual void KeyColorUpdate(int key) {
			keysColor[key * 3] = (byte)(red * bright);
			keysColor[key * 3 + 1] = (byte)(green * bright);
			keysColor[key * 3 + 2] = (byte)(blue * bright);
		}

		/// <summary>
		/// Change key color with actual bright. 
		/// Can jump to next key by bigger rgb array. {r1, g1, b1, r2...}
		/// </summary>
		public virtual void KeyColorChange(int key, float[] rgb) {
			for (int i = 0; i < rgb.Length; i++)
				keysColor[key*3 + i] = ToByte(rgb[i] * bright);
		}

		/// <summary>
		/// Change keys color with actual bright. 
		/// Only selected keys.
		/// </summary>
		public virtual void KeysColorChange(int[] keys, float[] rgb) {
			for (int i = 0; i < keys.Length; i++)
				for (int c = 0; c < 3; c++)
					keysColor[keys[i] * 3 + c] = ToByte(rgb[c] * bright);
		}
		public virtual void KeysColorChange(int[] keys, float rgb) {
			for (int i = 0; i < keys.Length; i++)
				for (int c = 0; c < 3; c++)
					keysColor[keys[i] * 3 + c] = ToByte(rgb * bright);
		}
		public virtual void KeysColorChange(int fromkey, int tokey, float[] rgb) {
			for (int i = fromkey; i < tokey+1; i++)
				for (int c = 0; c < 3; c++)
					keysColor[i * 3 + c] = ToByte(rgb[c] * bright);
		}
		public virtual void KeysColorChange(int fromkey, int tokey, float rgb) {
			for (int i = fromkey; i < tokey + 1; i++)
				for (int c = 0; c < 3; c++)
					keysColor[i * 3 + c] = ToByte(rgb * bright);
		}
		public virtual void KeysColorChangeR(int[] keys, float r) {
			for (int i = 0; i < keys.Length; i++)
				keysColor[keys[i] * 3] = ToByte(r * bright);
		}
		public virtual void KeysColorChangeR(int fromkey, int tokey, float r) {
			for (int i = fromkey; i < tokey + 1; i++)
				keysColor[i * 3] = ToByte(r * bright);
		}
		public virtual void KeysColorChangeG(int[] keys, float g) {
			for (int i = 0; i < keys.Length; i++)
				keysColor[keys[i] * 3 + 1] = ToByte(g * bright);
		}
		public virtual void KeysColorChangeG(int fromkey, int tokey, float g) {
			for (int i = fromkey; i < tokey + 1; i++)
				keysColor[i * 3 + 1] = ToByte(g * bright);
		}
		public virtual void KeysColorChangeB(int[] keys, float b) {
			for (int i = 0; i < keys.Length; i++)
				keysColor[keys[i] * 3 + 2] = ToByte(b * bright);
		}
		public virtual void KeysColorChangeB(int fromkey, int tokey, float b) {
			for (int i = fromkey; i < tokey + 1; i++)
				keysColor[i * 3 + 1] = ToByte(b * bright);
		}

		//TODO
		public virtual void KeysColorChangeByMatrix(float[][] Matrix) {}
		public virtual void KeysColorInvert(int[] keys) {
			KeysColorChange(keys, GetInvertedColor());
		}
		public virtual void KeysColorInvert(int fromkey, int tokey) {
			KeysColorChange(fromkey, tokey, GetInvertedColor());
		}
		public virtual void KeysColorInvertIfOff() {
			float[] rgb = GetInvertedColor();
			for (int i = 0; i < GetKeysCount(); i++)
				if (keysLight[i] == 0)
					KeyColorChange(i, rgb);
		}
		public virtual void KeysColorInvertIfOff(int[] keys) {
			float[] rgb = GetInvertedColor();
			for (int i = 0; i < keys.Length; i++)
				if (keysLight[keys[i]] == 0)
					KeyColorChange(i, rgb);
		}
		public virtual void KeysColorInvertIfOff(int fromkey, int tokey) {
			float[] rgb = GetInvertedColor();
			for (int i = fromkey; i < tokey + 1; i++)
				if (keysLight[i] == 0)
					KeyColorChange(i, rgb);
		}


		public static readonly float colorGain = 5;
		/// <summary>
		/// NOTPrevent before duplicate state sending.
		/// Off LEDs with weak bright or black color.
		/// </summary>
		public virtual bool OptimizePacket() {
			if (colorGain > 0)
				for (int i = 0; i < GetKeysCount(); i++)
					if (keysColor[i * 3] < colorGain && keysColor[i * 3 + 1] < colorGain && keysColor[i * 3 + 2] < colorGain)
						keysLight[i] = 0;
			return true;
		}

		/// <summary>
		/// Thread sleep by delay
		/// </summary>
		public void DelaySleep() { Thread.Sleep(delay+2); }

		/// <summary>
		/// Thread sleep by 1s / speed
		/// </summary>
		public void SpeedSleep() { Thread.Sleep((int)(1000 / speed)); }

		/// <summary>
		/// Send keys color/light to device
		/// </summary>
		public virtual bool SendPacket() { return connected; }



		public static void ModuleOff() {
			if (activeModule.running)
				activeModule.Stop();
			Thread.Sleep(100);
		}

		public static void ModuleOn() {
			if (!activeModule.running)
				activeModule.Start();
		}

		public static void ModuleRestart() {
			if (activeModule.running)
				activeModule.Stop();
			activeModule.Start();
		}



		/// <summary>
		/// Change status of only one mode to true
		/// </summary>
		public static void ModuleChangeByClass(Type moduleClass) {
			bool wasActive = false;
			if (activeModule != null && activeModule.running) {
				wasActive = true;
				activeModule.Stop();
				Settings.SaveModule();
			}
			try {
				activeModule = (LightningModule)Activator.CreateInstance(moduleClass);
			}
			catch {
				ErrorProcessor.Error(2);
			}
			if (wasActive) {
				Settings.Load();
				activeModule.Start();
			}
		}
		public static void ModuleChangeByUID(string uid) {
			bool wasActive = false;
			if (activeModule != null && activeModule.running) {
				wasActive = true;
				activeModule.Stop();
				Settings.SaveModule();
			}
			for (int i = 0; i < modules.Count(); i++) {
				try {
					activeModule = (LightningModule)Activator.CreateInstance(modules.ElementAt(i));
				}
				catch {
					ErrorProcessor.Error(2);
				}
				if (activeModule.GetUid() == uid) {
					break;
				}
				if (i >= modules.Count()-1) {
					activeModule = null;
					ErrorProcessor.Error(3);
				}
			}
			if (wasActive) {
				Settings.Load();
				activeModule.Start();
			}
		}
		public static void ModuleChangeByName(string name) {
			bool wasActive = false;
			if (activeModule != null) {
				if (activeModule.running) {
					wasActive = true;
					activeModule.Stop();
					Settings.SaveModule();
				}
			}
			for (int i = 0; i < modules.Count(); i++) {
				try {
					activeModule = (LightningModule)Activator.CreateInstance(modules.ElementAt(i));
				}
				catch {
					ErrorProcessor.Error(2);
				}
				if (activeModule.GetName() == name)
					break;
				if (i >= modules.Count() - 1) {
					activeModule = null;
					ErrorProcessor.Error(3);
				}
			}
			if (wasActive) {
				Settings.Load();
				activeModule.Start();
			}
		}



		/// <summary>
		/// Prepare to work with device
		/// </summary>
		/// <returns>0 = can't connect, 1 = connected successful, 2 = already connected</returns>
		public byte InitiateConnection() {
			if (!connected) {
				if (Connect()) {
					connected = true;
					return 1;
				} else {
					ErrorProcessor.Error(0);
					return 0;
				}
			}
			return 2;
		}

		/// <summary>
		/// End work with device
		/// </summary>
		/// <returns>0 = can't disconnect, 1 = disconnected successful, 2 = already disconnected</returns>
		public byte BreakConnection() {
			if (connected) {
				if (Disconnect()) {
					connected = false;
					return 1;
				} else {
					ErrorProcessor.Error(1);
					return 0;
				}
			}
			return 2;
		}

		/// <summary>
		/// Reinitialize work with device
		/// </summary>
		/// <returns>0 = can't connect, 1 = connected successful, 2 = can't disconnect/returns>
		public byte ReinitiateConnection() {
			if (!connected) {
				if (Disconnect()) {
					connected = false;
				} else {
					ErrorProcessor.Error(1);
					return 2;
				}
			}

			if (Connect()) {
				connected = true;
				return 1;
			} else {
				ErrorProcessor.Error(0);
				return 0;
			}

		}



		/// <summary>
		/// Prevent small byte from big input
		/// </summary>
		public static byte ToByte(float value) {
			if (value < 0)
				return 0;
			else if (value > 255)
				return 255;
			else
				return (byte)value;
		}

		/// <summary>
		/// Applies RGB code
		/// </summary>
		/// <param name="rgb">string of rgb code "0, 0, 255"/"#0000FF"</param>
		public void ChangeColorRGB(string rgb) {
			Color color = (Color)ColorConverter.ConvertFromString(rgb);
			this.red = color.R;
			this.green = color.G;
			this.blue = color.B;
		}
		public void ChangeColorRGB(float r, float g, float b) {
			this.red = r;
			this.green = g;
			this.blue = b;
		}
		public void ChangeColorRGB(float[] rgb) {
			this.red = rgb[0];
			this.green = rgb[1];
			this.blue = rgb[2];
		}

		public float[] GetInvertedColor() {
			return new float[] { 255 - red, 255 - green, 255 - blue };
		}
		public float[] GetInvertedColor(byte[] rgb) {
			return new float[] { 255-rgb[0], 255 - rgb[1], 255 - rgb[2] };
		}
    }
}

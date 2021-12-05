using System;
using System.IO;
using System.Reflection;

namespace RyosMKFXPanel {
	public static class Settings {
		public unsafe struct Param {
			public object owner {
				get {
					return owner;
				}
				private set {
					owner = value;
				}
			}

			public Type type {
				get {
					return type;
				}
				private set {
					type = value;
				}
			}

			public string value {
				get {
					return value;
				}
				set {
					this.value = value;
					owner.GetType().GetProperty(privateName).SetValue(owner, ParamConvertValue());
				}
			} //Want to work with adress :( ///I hate C# 

			public string privateName {
				get {
					return privateName;
				}
				private set {
					privateName = value;
				}
			} 


			public string publicName {
				get {
					return publicName;
				}
				private set {
					publicName = value;
				}
			}

			public Param(object owner, Type type, string value, string privateName) {
				this.owner = owner;
				this.type = type;
				this.value = value;
				this.privateName = privateName;
				this.publicName = privateName;
			}
			public Param(object owner, Type type, string value, string privateName, string publicName) {
				this.owner = owner;
				this.type = type;
				this.value = value;
				this.privateName = privateName;
				this.publicName = publicName;
			}
			public Param(object owner, PropertyInfo var) {
				this.owner = owner;
				this.type = var.PropertyType;
				this.value = var.GetValue(owner).ToString();
				this.privateName = var.Name;
				this.publicName = var.Name;
			}
			public Param(object owner, PropertyInfo var, string publicName) {
				this.owner = owner;
				this.type = var.PropertyType;
				this.value = var.GetValue(owner).ToString();
				this.privateName = var.Name;
				this.publicName = publicName;
			}

			public object ParamConvertValue() {
				if (type == typeof(bool))
					return value.ToLower() == "true" ? true : false;
				else if (type == typeof(int))
					return Int32.Parse(value);
				else if (type == typeof(float))
					return Single.Parse(value);
				else if (type == typeof(double))
					return Double.Parse(value);
				else if (type == typeof(string))
					return value;
				else
					ErrorProcessor.Error(0x80, type.ToString());
				return null;
			}

		}
		private static string PathCreate(string[] parts) {
			string buffer = parts[0];
			for (int i = 1; i < parts.Length; i++)
				buffer += @"\" + parts[i];
			return buffer;
		}

		private const string DirSettingsRoot = @"Settings\";
		private const string DirSettingsGeneral = DirSettingsRoot + @"General\";
		private const string DirSettingsDevices = DirSettingsRoot + @"Devices\";



		public static void Initialize() {
			DirectoryAdd(new string[] {
				DirSettingsRoot,
				DirSettingsGeneral,
				DirSettingsDevices
			});
			
			foreach (Lightning device in Lightning.devices) {
				DirectoryAdd(DirSettingsDevices + device.GetDeviceUID());
			}
		}



		public static bool DirectoryAdd(string path) {
			if (!Directory.Exists(path)) {
				if (!Directory.CreateDirectory(path).Exists) {
					ErrorProcessor.Error(0x90, "create " + path + " directory.");
					return false;
				}
			}

			return true;
		}

		public static bool DirectoryAdd(string[] paths) {
			foreach (string path in paths) {
				if (!Directory.Exists(path)) {
					if (!Directory.CreateDirectory(path).Exists) {
						ErrorProcessor.Error(0x90, "create " + path + " directory.");
						return false;
					}
				} 
			}

			return true;
		}


		private static void SettingsWriteParam(string path, Param i) {
			File.AppendAllText(path, i.publicName + " = " + i.value + "\n");
		}
		private static void SettingsWriteParamArray(string path, Param[] array) {
			foreach (Param i in array) {
				File.AppendAllText(path, i.publicName + " = " + i.value + "\n");
			}
		}


		private static byte IsSettingsModuleFileExist(string path) {
			if (!File.Exists(path)) {
				if (File.Create(path).CanWrite) {
					SettingsWriteParamArray(path, Lightning.activeModule.GetParametersForDevice());
					SettingsWriteParamArray(path, Lightning.activeModule.GetParameters());

					return 0x1;
				} else {
					ErrorProcessor.Error(0x90, "create " + path + " file.");

					return 0x2; 
				}
			}

			return 0x0;
		}



		public static bool SettingsRead(Param var) {
			string path = var.owner.GetType().BaseType == typeof(LightningModule) ? 
				PathCreate(new string[] { DirSettingsDevices, Lightning.devices[0].GetDeviceUID(), Lightning.activeModule.GetUid() + ".ini" }) 
				: PathCreate(new string[] { DirSettingsGeneral, var.publicName, ".ini" });			
			if (IsSettingsModuleFileExist(path) != 0x0) {
				
				return false;
			} else {
				if (File.OpenWrite(path).CanWrite) {
					string[] lines = File.ReadAllLines(path);
					for (int i = 0; i < lines.Length; i++) {
						if (lines[i].Substring(0, lines[i].IndexOf("=")-2) == var.publicName) {
							var.value = lines[i].Substring(lines[i].IndexOf("=")+2, lines[i].Length-2);

						} else if (i == lines.Length-1) {
							SettingsWriteParam(path, var);

							return false;
						}
					}
				} else {
					ErrorProcessor.Error(0x90, "read " + path + " file.");
				}
			}

			return true;
		}

		public static bool SettingsWrite(Param var) {
			string path = var.owner.GetType().BaseType == typeof(LightningModule) ? 
				PathCreate(new string[] { DirSettingsDevices, Lightning.devices[0].GetDeviceUID(), Lightning.activeModule.GetUid() + ".ini" }) 
				: PathCreate(new string[] { DirSettingsGeneral, var.publicName, ".ini" });
			if (IsSettingsModuleFileExist(path) == 0x02) {

				return false;
			} else {
				if (File.OpenWrite(path).CanWrite) {
					string[] lines = File.ReadAllLines(path);
					for (int i = 0; i < lines.Length; i++) {
						if (lines[i].Substring(0, lines[i].IndexOf("=") - 2) == var.publicName) {
							lines[i] = lines[i].Substring(0, lines[i].IndexOf("=") + 1) + var.value;

							break;
						} else if (i == lines.Length - 1) {
							SettingsWriteParam(path, var);

							return false;
						}
					}
					File.WriteAllLines(path, lines);
				} else {
					ErrorProcessor.Error(0x90, "write " + path + " file.");

					return false;
				}
			}

			return true;
		}

		public static string SettingsReadUnsafe(string var, string path = DirSettingsGeneral + ".ini") {
			if (IsSettingsModuleFileExist(path) != 0x0) {

				return "";
			} else {
				if (File.OpenWrite(path).CanWrite) {
					string[] lines = File.ReadAllLines(path);
					for (int i = 0; i < lines.Length; i++) {
						if (lines[i].Substring(0, lines[i].IndexOf("=") - 2) == var) {
							return lines[i].Substring(lines[i].IndexOf("=") + 2, lines[i].Length - 2);

						} else if (i == lines.Length - 1) {
							SettingsWriteParam(path, var);

							return false;
						}
					}
				} else {
					ErrorProcessor.Error(0x90, "read " + path + " file.");
				}
			}

			return true;
		}

		public static bool SettingsWriteUnsafe(Param var, bool targetIsModule = false) {
			string path = targetIsModule ? PathCreate(new string[] { DirSettingsDevices, Lightning.devices[0].GetDeviceUID(), Lightning.activeModule.GetUid() + ".ini" }) : PathCreate(new string[] { DirSettingsGeneral, var.publicName, ".ini" });
			if (IsSettingsModuleFileExist(path) == 0x02) {

				return false;
			} else {
				if (File.OpenWrite(path).CanWrite) {
					string[] lines = File.ReadAllLines(path);
					for (int i = 0; i < lines.Length; i++) {
						if (lines[i].Substring(0, lines[i].IndexOf("=") - 2) == var.publicName) {
							lines[i] = lines[i].Substring(0, lines[i].IndexOf("=") + 1) + var.value;

							break;
						} else if (i == lines.Length - 1) {
							SettingsWriteParam(path, var);

							return false;
						}
					}
					File.WriteAllLines(path, lines);
				} else {
					ErrorProcessor.Error(0x90, "write " + path + " file.");

					return false;
				}
			}

			return true;
		}
	}
}

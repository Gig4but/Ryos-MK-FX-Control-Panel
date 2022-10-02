using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RyosMKFXPanel {
	public static class Settings {
		const string FileSuffix = @".ini";
		const string DirRoot = @"Settings\";
		const string FileGeneral = DirRoot + @"General\" + FileSuffix;
		const string DirDevices = DirRoot + @"Devices\";

		static string PathCreate(in string[] parts) => string.Join(@"\", parts);
		static string PathCreate(in Type type) {
			return PathCreate(new string[] {
				DirDevices,
				Lightning.devices[0].GetDeviceUID(),
				type.FullName == null ? "" : type.FullName.Substring(type.FullName.IndexOf('.') + 1)
			}) + FileSuffix;
		}



		static bool DirectoryAdd(in string path) {
			if (!Directory.CreateDirectory(path).Exists) {
				ErrorProcessor.Error(0x90, "create " + path + " directory.");
				return false;
			}

			return true;
		}
		static bool DirectoryAdd(in string[] paths) {
			for (int i = 0; i < paths.Length; i++)
				if (DirectoryAdd(paths[i]))
					return false;

			return true;
		}



		public static Dictionary<string, string> ?General;
		public enum GeneralSettings {
			DeviceID,
			Category,
        }
		public static bool GeneralSettingsContain(GeneralSettings setting) {
			if (General == null)
				return false;
			return General.ContainsKey(GeneralSettings.DeviceID.ToString());
		}

		public static void Initialize() {
			DirectoryAdd(new string[] {
				DirRoot,
				DirDevices
			});

			for (int i = 0; i < Lightning.devices.Length; i++)
				DirectoryAdd(DirDevices + Lightning.devices[i].GetDeviceUID());

			Read(FileGeneral, out General);
		}


		static bool Write(in string path, in Dictionary<string, string> settings) {
			using (FileStream file = File.OpenWrite(path)) {
				if (!file.CanWrite) {
					ErrorProcessor.Error(0x90, "write " + path + " file.");
					return false;
				} else if (file.Length > int.MaxValue) {
					ErrorProcessor.Error(0x91, path + " is too big.");
					return false;
				}

				long len = 0;
				byte[] buffer;
				long size;
				StringBuilder variable = new StringBuilder(32);
				HashSet<string> settingsExist = new HashSet<string>(settings.Count);
				int @char = file.ReadByte();

				while (@char != -1) {
					if (@char != ' ') {
						variable.Append(@char);
					} else {
						if (settings.ContainsKey(variable.ToString())) {
							do
								@char = file.ReadByte();
							while (@char != ' ' || @char != -1);

							while (@char != '\n' || @char != -1) {
								@char = file.ReadByte();
								len++;
							}

							buffer = Encoding.UTF8.GetBytes(settings[variable.ToString()]);
							if (buffer.Length != len) {
								size = file.Length - file.Position;
								byte[] fileBuffer = new byte[size];
								file.Read(fileBuffer, (int)file.Position, (int)size);
								file.Seek(-size, SeekOrigin.Current);
								file.Write(buffer, 0, buffer.Length);
								file.Write(fileBuffer, 0, fileBuffer.Length);
								file.Seek(len - size, SeekOrigin.Current);
                            } else {
								file.Write(buffer, 0, buffer.Length);
                            }
							settingsExist.Add(variable.ToString());
						} else {
							while (@char != '\n' || @char != -1)
								@char = file.ReadByte();
						}
						variable.Clear();
						len = 0;
					}
					if (@char != -1)
						@char = file.ReadByte();
				}

				Dictionary<string, string>.KeyCollection keys = settings.Keys;
				foreach (string key in keys) {
					if (!settingsExist.Contains(key)) {
						buffer = Encoding.UTF8.GetBytes(key + " = " + settings[key] + "\n");
						file.Write(buffer, 0, buffer.Length);
					}
				}
			}

			return true;
		}

		public static bool SaveGeneral()
        {
			return General != null && Write(FileGeneral, General);
		}

		public static bool SaveDevice(int index = -1) {
			bool saved = true;
			if (index < 0) {
				for (index = 0; index < Lightning.devices.Length && saved; index++) {
					saved = Write(DirDevices + Lightning.devices[index].GetDeviceUID(), Lightning.devices[index].GetSettings());
				}
			} else {
				saved = Write(DirDevices + Lightning.devices[index].GetDeviceUID(), Lightning.devices[index].GetSettings());
			}

			return saved;
		}

		public static bool SaveModule() {
			return Write(PathCreate(Lightning.activeModule.GetType()), Lightning.activeModule.GetSettings());
		}

		public static bool SaveAll()
		{
			return SaveGeneral()
				&& SaveDevice()
				&& SaveModule();
		}
		public static bool Load()
		{
			string path = PathCreate(Lightning.activeModule.GetType());
			Dictionary<string, string> settings;

			if (!Read(path, out settings))
				return false;
			Lightning.devices[0].LoadSettings(settings);
			Lightning.activeModule.LoadSettings(settings);

			return true;
		}

		static byte ExistFile(in string path) {
			if (!File.Exists(path)) {
				if (SaveAll())
				{
					if (!File.Exists(path))
						return 0x3;

						return 0x0;
				}

				return 0x2;
			}

			return 0x1;
		}

		static bool Read(in string path, out Dictionary<string, string> settings) {
			byte state = ExistFile(path);
			settings = new Dictionary<string, string>();
			if (state == 0x0)
				return false;
			else if (state == 0x1)
				settings = File.ReadAllLines(path)
					.Select(line => line.Split(" = "))
					.ToDictionary(split => split[0], split => split[1]);

			return true;
		}
	}
}

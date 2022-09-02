using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RyosMKFXPanel {
	public static class Settings {
		private const string FileSuffix = @".ini";
		private const string DirRoot = @"Settings\";
		private const string FileGeneral = DirRoot + @"General\" + FileSuffix;
		private const string DirDevices = DirRoot + @"Devices\";

		private static string PathCreate(in string[] parts) => string.Join(@"\", parts);
		private static string PathCreate(in Type type) {
			return PathCreate(new string[] {
				DirDevices,
				Lightning.devices[0].GetDeviceUID(),
				type.FullName == null ? "" : type.FullName.Substring(type.FullName.IndexOf('.') + 1)
			}) + FileSuffix;
		}



		private static bool DirectoryAdd(in string path) {
			if (!Directory.CreateDirectory(path).Exists) {
				ErrorProcessor.Error(0x90, "create " + path + " directory.");
				return false;
			}

			return true;
		}
		private static bool DirectoryAdd(in string[] paths) {
			for (int i = 0; i < paths.Length; i++)
				if (DirectoryAdd(paths[i]))
					return false;

			return true;
		}



		public static Dictionary<string, string>? General;
		public static void Initialize() {
			DirectoryAdd(new string[] {
				DirRoot,
				DirDevices
			});

			for (int i = 0; i < Lightning.devices.Length; i++)
				DirectoryAdd(DirDevices + Lightning.devices[i].GetDeviceUID());

			Read(FileGeneral, out General);
		}


		private static bool Write(in string path, in Dictionary<string, string> settings, bool append = false) {
			if (!append)
				File.Delete(path);
			//if (!File.OpenWrite(path).CanWrite) {
			//	ErrorProcessor.Error(0x90, "write " + path + " file.");
			//	
			//	return false;
			//}
			File.AppendAllText(path, string.Join("\n", settings.Select(var => var.Key + " = " + var.Value)));
			return true;
		}

		private static byte ExistFile(in string path) {
			if (!File.Exists(path)) {
				if (Save())
					return 0x0;

				return 0x2;
			}

			return 0x1;
		}

		private static bool Read(in string path, out Dictionary<string, string> settings) {
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



		public static bool Save(bool all = false) {
			string path = PathCreate(Lightning.activeModule.GetType());
			return Write(path, Lightning.devices[0].SettingsGet()) 
				&& Write(path, Lightning.activeModule.SettingsGet(), append: true)
				&& (all ? Write(FileGeneral, General) : true);
		}
		public static bool Load() {
			string path = PathCreate(Lightning.activeModule.GetType());
			Dictionary<string, string> settings;

			if (!Read(path, out settings))
				return false;
			Lightning.devices[0].SettingsLoad(settings);
			Lightning.activeModule.SettingsLoad(settings);

			return true;
		}
	}
}

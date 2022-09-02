using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RyosMKFXPanel.Windows {
	public partial class ModernSettings :Window {
		private bool initialized = false;
		public ModernSettings() {
			InitializeComponent();
			string[] devices = Audio.GetAudioDevicesNames();
			for (int i = 0; i < devices.Length; i++) {
				comboBoxDevices.Items.Add((i+1)+" - "+devices[i]);
			}
			if (Settings.General.ContainsKey("DeviceId") || Settings.General["DeviceId"] == "" || Audio.DeviceAutoChange()) {
				comboBoxDevices.SelectedIndex = 0;
			} else {
				comboBoxDevices.SelectedIndex = Audio.GetListeningDevice(Settings.General["DeviceId"]) + 1;
			}
			initialized = true;
		}
		private void ButtonClose_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}
		private bool _windowMoveLock = false;
		private Point _pos = new Point(0, 0);
		private void MoveWindow(object sender, RoutedEventArgs e) {
			if (_windowMoveLock) {
				Point pos = Mouse.GetPosition(this);
				this.Left += pos.X - _pos.X;
				this.Top += pos.Y - _pos.Y;
				//MoveWindow();
			}
		}
		private void MoveWindowUnlock(object sender, RoutedEventArgs e) {
			_windowMoveLock = true;
			_pos = Mouse.GetPosition(this);
			//MoveWindow();
			MouseMove += new System.Windows.Input.MouseEventHandler(MoveWindow);
		}
		private void MoveWindowLock(object sender, RoutedEventArgs e) {
			_windowMoveLock = false;
			MouseMove -= new System.Windows.Input.MouseEventHandler(MoveWindow);
		}
		private void comboBoxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (initialized) {
				if (comboBoxDevices.SelectedIndex > 0) {
					Lightning.ModuleOff();
					Audio.DeviceAutoChange(false);
					Audio.ChangeListeningDevice(comboBoxDevices.SelectedIndex-1);
					Lightning.ModuleOn();
				} else {
					Lightning.ModuleOff();
					Settings.General["DeviceId"] = "";
					Audio.DeviceAutoChange(true);
					Audio.ChangeListeningDevice(0);
					Lightning.ModuleOn();
				}
			}
		}
		private void ButtonSettingsReset_Click(object sender, RoutedEventArgs e) {
			//Settings.Reset();
		}
	}
}

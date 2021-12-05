using RyosMKFXPanel.Effects;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RyosMKFXPanel.Windows {
	public partial class ModernSettings :Window {
		private bool initialized = false;
		public ModernSettings() {
			InitializeComponent();
			List<string> devices = Audio.GetAudioDevicesNames();
			for (int i = 0; i < devices.Count; i++) {
				comboBoxDevices.Items.Add((i+1)+" - "+devices[i]);
			}
			if (Properties.Settings.Default.DeviceID == null || Properties.Settings.Default.DeviceID == "" || Audio.DeviceAutoChange()) {
				comboBoxDevices.SelectedIndex = 0;
			} else {
				comboBoxDevices.SelectedIndex = Audio.GetAudioDeviceIndexByID(Properties.Settings.Default.DeviceID) + 1;
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
					Audio.ChangeListenDeviceByIndex(comboBoxDevices.SelectedIndex-1);
					Lightning.ModuleOn();
				} else {
					Lightning.ModuleOff();
					Properties.Settings.Default.DeviceID = "";
					Audio.DeviceAutoChange(true);
					Audio.ChangeListenDeviceByIndex(0);
					Lightning.ModuleOn();
				}
			}
		}
		private void ButtonSettingsReset_Click(object sender, RoutedEventArgs e) {
			Properties.Settings.Default.Reset();
			Properties.Settings.Default.reset = true;
		}
	}
}

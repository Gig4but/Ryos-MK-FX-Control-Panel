using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace RyosMKFXPanel.Windows {
	public partial class ModernMain :Window {
		private static bool initialized = false;
		public ModernMain() {
			InitializeComponent();
			Settings.Initialize();
			initialized = true;


			if (Settings.General.ContainsKey("DeviceId")) {
				Audio.DeviceAutoChange(false);
                Audio.GetListeningDevice(Settings.General["DeviceId"]);
			}

			for (int i = 0; i < Lightning.modules.Count(); i++) {
				LightningModule instance;
				try {
					instance = (LightningModule)Activator.CreateInstance(Lightning.modules.ElementAt(i));
				}
				catch {
					ErrorProcessor.Error(2);
					continue;
				}

				Button button = new Button();
				if (!new Regex(@"[\w]+[\w\d]*").IsMatch(instance.GetUid())) {
					ErrorProcessor.Error(5, instance.GetUid());
					continue;
				} else if (this.FindName(instance.GetUid()) == null) {
					button.Name = instance.GetUid();
					this.RegisterName(button.Name, button);
				} else {
					ErrorProcessor.Error(4, instance.GetUid());
					continue;
				}
				button.Margin = new Thickness(4);
				button.Style = (Style)Resources["EffectButton"];
				button.Click += new RoutedEventHandler(ChangeModule);

				Border border = new Border();

				border.Name = instance.GetUid() + "Border";
				this.RegisterName(border.Name, border);
				border.BorderThickness = new Thickness(2);
				border.BorderBrush = (Brush)Application.Current.Resources["DefaultColorBrushAA"];
				border.Width = border.Height = 64;

				StackPanel stack = new StackPanel();
				stack.VerticalAlignment = VerticalAlignment.Center;
				stack.HorizontalAlignment = HorizontalAlignment.Center;
				stack.Width = 64;

				Rectangle icon = new Rectangle();
				icon.Name = instance.GetUid() + "Icon";
				this.RegisterName(icon.Name, icon);
				icon.VerticalAlignment = VerticalAlignment.Bottom;
				icon.HorizontalAlignment = HorizontalAlignment.Center;
				icon.Width = icon.Height = 32;
				icon.Margin = new Thickness(0, 4, 0, -4);
				icon.Fill = (Brush)Application.Current.Resources["DefaultColorBrushFF"];
				BitmapImage image = new BitmapImage();
				image.BeginInit();
				//image.UriSource = new Uri(instance._icon, UriKind.Relative);
				image.StreamSource = new MemoryStream(Convert.FromBase64String(instance.GetIcon()));
				image.EndInit();
				icon.OpacityMask = new ImageBrush(image);

				Label text = new Label();
				text.Name = instance.GetUid() + "Text";
				this.RegisterName(text.Name, text);
				text.FontSize = 12;
				text.Foreground = (Brush)Application.Current.Resources["DefaultColorBrushAA"];
				text.FontWeight = FontWeights.Bold;
				text.Content = instance.GetName();
				text.VerticalAlignment = VerticalAlignment.Center;
				text.HorizontalAlignment = HorizontalAlignment.Center;

				stack.Children.Add(icon);
				stack.Children.Add(text);
				border.Child = stack;
				button.Content = border;

				WrapPanel panel = (WrapPanel)this.FindName("Panel" + instance.GetCategory());
				if (panel != null) {
					panel.Children.Add(button);
				} else {
					ErrorProcessor.Error(7, instance.GetUid());
					continue;
				}
			}

			((IInvokeProvider)(new ButtonAutomationPeer((Button)this.FindName("Category" + Settings.General["Category"])).GetPattern(PatternInterface.Invoke))).Invoke();

		}

		private void Window_Close(object sender, System.ComponentModel.CancelEventArgs e) {
			Lightning.ModuleOff();
			foreach (Lightning device in Lightning.devices) {
				device.BreakConnection();
			}
			Environment.Exit(0);
		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e) {
			//this.ButtonClose.  Property = "Background" Value = "#AAFF0000" TargetName = "ButtonCloseBackground"
			Window_Close(null, null);
		}
		private void ButtonMinimize_Click(object sender, RoutedEventArgs e) {
			this.WindowState = WindowState.Minimized;
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
			MouseMove += new MouseEventHandler(MoveWindow);
		}
		private void MoveWindowLock(object sender, RoutedEventArgs e) {
			_windowMoveLock = false;
			MouseMove -= new MouseEventHandler(MoveWindow);
		}



		private void ChangeModule(object sender, RoutedEventArgs e) {
			((Button)this.FindName(Lightning.activeModule.GetUid())).Style = (Style)Resources["EffectButton"];
			((Border)this.FindName(Lightning.activeModule.GetUid() + "Border")).BorderBrush = (Brush)Application.Current.Resources["DefaultColorBrushAA"];
			((Rectangle)this.FindName(Lightning.activeModule.GetUid() + "Icon")).Fill = (Brush)Application.Current.Resources["DefaultColorBrushFF"];
			((Label)this.FindName(Lightning.activeModule.GetUid() + "Text")).Foreground = (Brush)Application.Current.Resources["DefaultColorBrushAA"];

			Lightning.ModuleOff();

			Lightning.ModuleChangeByUID(((Button)sender).Name.Substring(1));

			DSSlider.Maximum = Lightning.activeModule.GetSliderMax();
			DSSlider.Minimum = Lightning.activeModule.GetSliderMin();
			DSSlider.SmallChange = Lightning.activeModule.GetSliderTick();
			DSSlider.TickFrequency = Lightning.activeModule.GetSliderTickFrequency();

			((Button)this.FindName(Lightning.activeModule.GetUid())).Style = (Style)Resources["EffectButtonSelected"];
			((Border)this.FindName(Lightning.activeModule.GetUid() + "Border")).BorderBrush = (Brush)Application.Current.Resources["ActualColorBrushAA"];
			((Rectangle)this.FindName(Lightning.activeModule.GetUid() + "Icon")).Fill = (Brush)Application.Current.Resources["ActualColorBrushFF"];
			((Label)this.FindName(Lightning.activeModule.GetUid() + "Text")).Foreground = (Brush)Application.Current.Resources["ActualColorBrushAA"];

			SettingsGrid.Children.Clear();
			for (int i = 0; i < Lightning.activeModule.GetControls().Length; i++) {
				SettingsGrid.Children.Add(Lightning.activeModule.GetControls()[i].stack);
				Lightning.activeModule.GetControls()[i].Show(null, null);
			}

			Lightning.ModuleOn();
		}



		private void ButtonEffects_Click(object sender, RoutedEventArgs e) {
			if (Lightning.activeModule.GetCategory() != "Effects") {
				//SettingsUpdate();
				
				this.LabelMode.Content = "Effect:";
				//updateColor();
				this.ButtonEffectsImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
				this.ButtonEffectsText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
				this.ModeLighter.SetValue(Grid.ColumnProperty, 0);
			}
		}
		private void ButtonAnimations_Click(object sender, RoutedEventArgs e) {
			if (Lightning.activeModule.GetCategory() != "Animations") {
				this.LabelMode.Content = "Animation:";
				//Lightning.ModuleChangeByUID(Properties.Settings.Default.AnimationsUID);

				ColorUpdate();
				this.ButtonAnimationsImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
				this.ButtonAnimationsText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
				this.ModeLighter.SetValue(Grid.ColumnProperty, 1);
			}
		}


		//Button for start work with device
		private void ButtonLever_Click(object sender, RoutedEventArgs e) {
			if (!Lightning.devices[0].connected) {
				Lightning.devices[0].InitiateConnection();
				this.ButtonLeverText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
				this.RectangleStatusColor.Color = (Color)ColorConverter.ConvertFromString("#FF008800");
			} else {
				Lightning.activeModule.Stop();
				Lightning.devices[0].BreakConnection();
				Lightning.activeModule.Start();
				this.ButtonLeverText.SetResourceReference(Label.ForegroundProperty, "BlackColorBrushAA");
				this.RectangleStatusColor.Color = (Color)ColorConverter.ConvertFromString("#FF880000");
			}
		}

		private void DSSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			switch (Lightning.activeModule.GetSliderType()) {
				case SliderType.delay:
					this.DSSliderLabel.Content = "Delay: " + this.DSSlider.Value.ToString() + "ms";
					Lightning.devices[0].delay = (int)this.DSSlider.Value;
					Lightning.devices[0].speed = 3f;
					break;
				case SliderType.speed:
					this.DSSliderLabel.Content = "Speed: " + this.DSSlider.Value.ToString() + "x";
					Lightning.devices[0].delay = 3;
					Lightning.devices[0].speed = (float)this.DSSlider.Value;
					break;
				default:
					ErrorProcessor.Error(6, Lightning.activeModule.GetUid());
					break;
			}
		}

		private void SliderBright_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			if (initialized) {
				if (this.SliderBright.Value < 10) {
					this.LabelBright.Content = "Brightness:	 " + this.SliderBright.Value.ToString() + "%";
				} else if (this.SliderBright.Value < 100) {
					this.LabelBright.Content = "Brightness:   " + this.SliderBright.Value.ToString() + "%";
				} else {
					this.LabelBright.Content = "Brightness: " + this.SliderBright.Value.ToString() + "%";
				}
				Lightning.devices[0].bright = ((float)this.SliderBright.Value / 100f);
				ColorUpdate();
			}
		}



		//Colors and Brushes
		//BrushConverter converterBrush = new BrushConverter();
		private void ColorUpdate() {
			Color c = new Color();
			c = Color.FromArgb((byte)(0xAA * Lightning.devices[0].bright), (byte)Lightning.devices[0].red, (byte)Lightning.devices[0].green, (byte)Lightning.devices[0].blue);
			this.Resources["ActualColorAA"] = c;
			this.Resources["ActualColorBrushAA"] = new SolidColorBrush(c);

			c = Color.FromArgb((byte)(0xFF * Lightning.devices[0].bright), (byte)Lightning.devices[0].red, (byte)Lightning.devices[0].green, (byte)Lightning.devices[0].blue);
			this.Resources["ActualColorFF"] = c;
			this.Resources["ActualColorBrushFF"] = new SolidColorBrush(c);
		}


		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
			TextBoxLibrary.NumberValidationTextBox(sender, e);
		}
		private void TextBoxRed_TextChanged(object sender, RoutedEventArgs e) {
			if (initialized) {
				if (TextBoxRed != null && TextBoxGreen != null && TextBoxBlue != null && TextBoxHex != null) {
					int x = 0;
					if (int.TryParse(this.TextBoxRed.Text, out x)) {
						if (Regex.IsMatch(this.TextBoxRed.Text, "0/d")) {
							this.TextBoxRed.Text = this.TextBoxRed.Text.Substring(1);
							this.TextBoxRed.CaretIndex = this.TextBoxRed.Text.Length;
						}
						if (x >= 0 && x <= 255) {
							Lightning.devices[0].red = x;
							ColorUpdate();

							string s = Convert.ToString(x, 16).ToUpper();
							if (!Regex.IsMatch(s, ".{2}")) {
								s = "0" + s;
							}
							s = s + TextBoxHex.Text.Substring(2, 4);
							if (TextBoxHex.Text != s)
								TextBoxHex.Text = s;
						} else {
							this.TextBoxRed.Text = Lightning.devices[0].red.ToString();
							this.TextBoxRed.CaretIndex = this.TextBoxRed.Text.Length;
						}
					}
				}
			}
		}
		private void TextBoxGreen_TextChanged(object sender, RoutedEventArgs e) {
			if (initialized) {
				if (TextBoxRed != null && TextBoxGreen != null && TextBoxBlue != null && TextBoxHex != null) {
					int x = 0;
					if (int.TryParse(this.TextBoxGreen.Text, out x)) {
						if (Regex.IsMatch(this.TextBoxGreen.Text, "0/d")) {
							this.TextBoxGreen.Text = this.TextBoxGreen.Text.Substring(1);
							this.TextBoxGreen.CaretIndex = this.TextBoxGreen.Text.Length;
						}
						if (x >= 0 && x <= 255) {
							Lightning.devices[0].green = x;
							ColorUpdate();

							string s = Convert.ToString(x, 16).ToUpper();
							if (!Regex.IsMatch(s, ".{2}")) {
								s = "0" + s;
							}
							s = TextBoxHex.Text.Substring(0, 2) + s + TextBoxHex.Text.Substring(4, 2);
							if (TextBoxHex.Text != s)
								TextBoxHex.Text = s;
						} else {
							this.TextBoxGreen.Text = Lightning.devices[0].green.ToString();
							this.TextBoxGreen.CaretIndex = this.TextBoxGreen.Text.Length;
						}
					}
				}
			}
		}
		private void TextBoxBlue_TextChanged(object sender, RoutedEventArgs e) {
			if (initialized) {
				if (TextBoxRed != null && TextBoxGreen != null && TextBoxBlue != null && TextBoxHex != null) {
					int x = 0;
					if (int.TryParse(this.TextBoxBlue.Text, out x)) {
						if (Regex.IsMatch(this.TextBoxBlue.Text, "0/d")) {
							this.TextBoxBlue.Text = this.TextBoxBlue.Text.Substring(1);
							this.TextBoxBlue.CaretIndex = this.TextBoxBlue.Text.Length;
						}
						if (x >= 0 && x <= 255) {
							Lightning.devices[0].blue = x;
							ColorUpdate();

							string s = Convert.ToString(x, 16).ToUpper();
							if (!Regex.IsMatch(s, ".{2}")) {
								s = "0" + s;
							}
							s = TextBoxHex.Text.Substring(0, 4) + s;
							if (TextBoxHex.Text != s)
								TextBoxHex.Text = s;
						} else {
							this.TextBoxBlue.Text = Lightning.devices[0].blue.ToString();
							this.TextBoxBlue.CaretIndex = this.TextBoxBlue.Text.Length;
						}
					}
				}
			}
		}

		private void HexValidationTextBox(object sender, TextCompositionEventArgs e) {
			TextBoxLibrary.HexValidationTextBox(sender, e);
		}
		private void TextBoxHex_TextChanged(object sender, RoutedEventArgs e) {
			if (initialized) {
				if (TextBoxRed != null && TextBoxGreen != null && TextBoxBlue != null && TextBoxHex != null) {
					if (Regex.IsMatch(this.TextBoxHex.Text, "[0-9a-fA-F]{6}")) {
						int r = Convert.ToInt32(this.TextBoxHex.Text.Substring(0, 2), 16);
						int g = Convert.ToInt32(this.TextBoxHex.Text.Substring(2, 2), 16);
						int b = Convert.ToInt32(this.TextBoxHex.Text.Substring(4, 2), 16);

						if (Lightning.devices[0].red != r) {
							this.TextBoxRed.Text = r.ToString();
							Lightning.devices[0].red = r;
						}

						if (Lightning.devices[0].green != g) {
							this.TextBoxGreen.Text = g.ToString();
							Lightning.devices[0].green = g;
						}

						if (Lightning.devices[0].blue != b) {
							this.TextBoxBlue.Text = b.ToString();
							Lightning.devices[0].blue = b;
						}

						ColorUpdate();
					}
				}
			}
		}

		private void ButtonSettings_Click(object sender, RoutedEventArgs e) {
			ModernSettings WindowSettings = new ModernSettings();
			//WindowSettings.comboBoxDevices.SelectionChanged += new SelectionChangedEventHandler(change);
			WindowSettings.Left = this.Left + this.Width / 2 - WindowSettings.Width / 2;
			WindowSettings.Top = this.Top + this.Height / 2 - WindowSettings.Height / 2;
			WindowSettings.Show();
		}
	}
}

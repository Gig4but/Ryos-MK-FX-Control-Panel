/*using RyosMKFXPanel.Animations;
using RyosMKFXPanel.Effects;
using System;
using System.Windows;
using System.Windows.Media;

namespace RyosMKFXPanel.windowStyles {
	public partial class styleLegacy :Window {
		public styleLegacy() {
			InitializeComponent();
		}

		private void ButtonEffects_Click(object sender, RoutedEventArgs e) {
			if (!Lightning.effects) {
				this.LabelMode.Content = "Effect:";
				hideAllModsOptions();
				Lightning.ChangeMode(0);
				showAllModsOptions();
				hideAllAlgsSettings();
				showAllAlgsSettings();
			}
		}
		private void ButtonAnimations_Click(object sender, RoutedEventArgs e) {
			if (!Lightning.animations) {
				this.LabelMode.Content = "Animation:";
				hideAllModsOptions();
				Lightning.ChangeMode(1);
				showAllModsOptions();
				hideAllAlgsSettings();
				showAllAlgsSettings();
			}
		}

		private void ButtonLever_Click(object sender, RoutedEventArgs e) {
			if (Lightning.SwitchWork()) {
				this.LabelStatus.Content = "ON";
				BrushConverter converter = new System.Windows.Media.BrushConverter();
				this.LabelStatus.Foreground = (Brush)converter.ConvertFromString("#FF008800");
				this.ButtonLever.Content = "Stop";
			} else {
				this.LabelStatus.Content = "OFF";
				BrushConverter converter = new System.Windows.Media.BrushConverter();
				this.LabelStatus.Foreground = (Brush)converter.ConvertFromString("#FFFF0000");
				this.ButtonLever.Content = "Start";
			}
		}

		private void SliderDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			if (this.ComboBoxEffects.SelectedIndex == 0) {
				this.LabelDelay.Content = "Speed: " + this.SliderDelay.Value.ToString() + "x";
				Lightning.ChangeSpeed(Convert.ToInt32(this.SliderDelay.Value));
				Lightning.ChangeDelay(0);
			} else {
				this.LabelDelay.Content = "Delay: " + this.SliderDelay.Value.ToString() + "ms";
				Lightning.ChangeDelay(Convert.ToInt32(this.SliderDelay.Value));
			}
		}
		private void SliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			this.LabelSpeed.Content = "Speed: " + this.SliderSpeed.Value.ToString() + "x";
			Lightning.ChangeSpeed((float)this.SliderSpeed.Value);
		}
		private void SliderBright_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			this.LabelBright.Content = "Brightness: " + this.SliderBright.Value.ToString() + "%";
			Lightning.ChangeBrightness((float)this.SliderBright.Value / 100f);
		}

		private void TextBoxRed_TextChanged(object sender, RoutedEventArgs e) {
			int x = 0;
			if (int.TryParse(this.TextBoxRed.Text, out x)) {
				if (x >= 0 && x <= 255) {
					Lightning.ChangeColorRed((byte)x);
				}
			}
		}
		private void TextBoxGreen_TextChanged(object sender, RoutedEventArgs e) {
			int x = 0;
			if (int.TryParse(this.TextBoxGreen.Text, out x)) {
				if (x >= 0 && x <= 255) {
					Lightning.ChangeColorGreen((byte)x);
				}
			}
		}
		private void TextBoxBlue_TextChanged(object sender, RoutedEventArgs e) {
			int x = 0;
			if (int.TryParse(this.TextBoxBlue.Text, out x)) {
				if (x >= 0 && x <= 255) {
					Lightning.ChangeColorBlue((byte)x);
				}
			}
		}

		private void Window_Close(object sender, System.ComponentModel.CancelEventArgs e) {
			Lightning.StopWork();
		}

		bool wasFirstRun = false;
		private void ComboBoxEffects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			if (this.ComboBoxEffects.SelectedIndex == 0) {
				//Equalizer
				if (wasFirstRun) {
					this.SliderDelay.Value = 3;
					this.SliderDelay.Minimum = 1;
					this.SliderDelay.Maximum = 4;
					hideAllAlgsSettings();
					this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Visible;
					Lightning.RunNewAlg(0);
				} else {
					wasFirstRun = true;
				}
			} else if (this.ComboBoxEffects.SelectedIndex == 1) {
				//Volume
				this.SliderDelay.Value = 20;
				this.SliderDelay.Minimum = 15;
				this.SliderDelay.Maximum = 30;
				hideAllAlgsSettings();
				this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Visible;
				Lightning.RunNewAlg(1);
			} else if (this.ComboBoxEffects.SelectedIndex == 2) {
				//Random
				this.SliderDelay.Value = 20;
				this.SliderDelay.Minimum = 15;
				this.SliderDelay.Maximum = 30;
				hideAllAlgsSettings();
				this.GroupBoxEffectRandomSettings.Visibility = Visibility.Visible;
				Lightning.RunNewAlg(2);
			}
		}

		private void ComboBoxAnimations_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {

			if (this.ComboBoxAnimations.SelectedIndex == 0 && Lightning.animations) {
				//Timer
				this.SliderSpeed.Value = 1;
				this.SliderSpeed.Minimum = 0.5;
				this.SliderSpeed.Maximum = 5;
				hideAllAlgsSettings();
				this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
				Lightning.RunNewAlg(0);
			} else if (this.ComboBoxAnimations.SelectedIndex == 1) {
				//Smiles
				this.SliderSpeed.Value = 1;
				this.SliderSpeed.Minimum = 0.5;
				this.SliderSpeed.Maximum = 5;
				hideAllAlgsSettings();
				Lightning.RunNewAlg(1);
			} else if (this.ComboBoxAnimations.SelectedIndex == 2) {
				//Waterfall
				this.SliderSpeed.Value = 10;
				this.SliderSpeed.Minimum = 10;
				this.SliderSpeed.Maximum = 20;
				hideAllAlgsSettings();
				this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Visible;
				Lightning.RunNewAlg(2);
			} else if (this.ComboBoxAnimations.SelectedIndex == 3) {
				//NyanCat
				this.SliderSpeed.Value = 1;
				this.SliderSpeed.Minimum = 0.5;
				this.SliderSpeed.Maximum = 5;
				hideAllAlgsSettings();
				Lightning.RunNewAlg(3);
			}
		}
		private void ChekBoxEffectsEqualizerStaticVolumeTurn_Checked(object sender, RoutedEventArgs e) {
			Equalizer.turnVolume();
			this.GroupBoxEffectsEqualizerColumnsSettings.Visibility = Visibility.Hidden;
			this.GroupBoxEffectsEqualizerStaticVolumeSettings.Visibility = Visibility.Visible;
		}
		private void ChekBoxEffectsEqualizerStaticVolumeTurn_UnChecked(object sender, RoutedEventArgs e) {
			Equalizer.turnVolume();
			this.GroupBoxEffectsEqualizerColumnsSettings.Visibility = Visibility.Visible;
			this.GroupBoxEffectsEqualizerStaticVolumeSettings.Visibility = Visibility.Hidden;
		}
		private void ChekBoxEffectsEqualizerAntialiasingTurn_Checked(object sender, RoutedEventArgs e) {
			Equalizer.turnAntialiasing();
		}
		private void TextBoxEffectsEqualizerVolumeBorderSize_TextChanged(object sender, RoutedEventArgs e) {
			int x = 1;
			if (int.TryParse(this.TextBoxEffectsEqualizerVolumeBorderSize.Text, out x)) {
				Equalizer.staticVolumeSize = x;
			}
		}
		private void TextBoxEffectsEqualizerStartColumn_TextChanged(object sender, RoutedEventArgs e) {
			int x = 1;
			if (int.TryParse(this.TextBoxEffectsEqualizerStartColumn.Text, out x)) {
				if (x < 1) {
					Equalizer.startColumn = 1;
				} else if (x > 23) {
					Equalizer.startColumn = 23;
				} else {
					Equalizer.startColumn = x;
				}
			}
		}
		private void TextBoxEffectsEqualizerEndColumn_TextChanged(object sender, RoutedEventArgs e) {
			int x = 2;
			if (int.TryParse(this.TextBoxEffectsEqualizerEndColumn.Text, out x)) {
				if (x < 2) {
					Equalizer.endColumn = 2;
				} else if (x > 24) {
					Equalizer.endColumn = 24;
				} else {
					Equalizer.endColumn = x;
				}
			}
		}
		private void TextBoxMaxFreq_TextChanged(object sender, RoutedEventArgs e) {
			int x = 0;
			if (int.TryParse(this.TextBoxMaxFreq.Text, out x)) {
				if (x < Equalizer.minHz + Lightning.GetWidth()) {
					Equalizer.maxHz = Equalizer.minHz + Lightning.GetWidth();
				} else {
					Equalizer.maxHz = x;
				}
			}
			TextBoxMinFreq_TextChanged();
		}
		private void TextBoxMaxFreq_TextChanged() {
			int x = 0;
			if (int.TryParse(this.TextBoxMaxFreq?.Text, out x)) {
				if (x < Equalizer.minHz + Lightning.GetWidth()) {
					Equalizer.maxHz = Equalizer.minHz + Lightning.GetWidth();
				} else {
					Equalizer.maxHz = x;
				}
			}
		}
		private void TextBoxMinFreq_TextChanged(object sender, RoutedEventArgs e) {
			int x = 0;
			if (int.TryParse(this.TextBoxMinFreq.Text, out x)) {
				if (x < 0) {
					Equalizer.minHz = 0;
				} else if (x > Equalizer.maxHz - Lightning.GetWidth()) {
					x = Equalizer.maxHz - Lightning.GetWidth();
				} else {
					Equalizer.minHz = x;
				}
			}
			TextBoxMaxFreq_TextChanged();
		}
		private void TextBoxMinFreq_TextChanged() {
			int x = 0;
			if (int.TryParse(this.TextBoxMinFreq?.Text, out x)) {
				if (x < 0) {
					Equalizer.minHz = 0;
				} else if (x > Equalizer.maxHz - Lightning.GetWidth()) {
					x = Equalizer.maxHz - Lightning.GetWidth();
				} else {
					Equalizer.minHz = x;
				}
			}
		}

		private void ChekBoxVolumeSimple_Checked(object sender, RoutedEventArgs e) {
			Volume.simpleVolume();
		}

		private void ChekBoxRandomTurn_Checked(object sender, RoutedEventArgs e) {
			Effects.Random.LEDturn();
		}

		private void ChekBoxTimerSimple_Checked(object sender, RoutedEventArgs e) {
			Animations.Timer.simpleNumbers();
		}
		private void ChekBoxTimerInvert_Checked(object sender, RoutedEventArgs e) {
			Animations.Timer.invertColor();
		}

		private void ChekBoxWaterfallTail_Checked(object sender, RoutedEventArgs e) {
			Waterfall.turnTail();
		}
		private void TextBoxWaterfallTailLength_TextChanged(object sender, RoutedEventArgs e) {
			int x = 0;
			if (int.TryParse(this.TextBoxWaterfallTailLength.Text, out x)) {
				if (x < 1) {
					Waterfall.tailLength = 1;
				} else {
					Waterfall.tailLength = x;
				}
			}
		}
		private void ChekBoxWaterfallInvert_Checked(object sender, RoutedEventArgs e) {
			Waterfall.turnInvert();
		}
		private void ChekBoxWaterfallHorizontal_Checked(object sender, RoutedEventArgs e) {
			Waterfall.turnHorizontal();
		}
		private void ChekBoxWaterfallDiagonal_Checked(object sender, RoutedEventArgs e) {
			Waterfall.turnDiagonal();
		}

		private void hideAllModsOptions() {
			this.ComboBoxEffects.Visibility = Visibility.Hidden;
			this.ComboBoxAnimations.Visibility = Visibility.Hidden;
			this.GroupBoxDelaySetting.Visibility = Visibility.Hidden;
			this.GroupBoxSpeedSetting.Visibility = Visibility.Hidden;
		}
		private void showAllModsOptions() {
			if (Lightning.effects) {
				this.ComboBoxEffects.Visibility = Visibility.Visible;
				this.GroupBoxDelaySetting.Visibility = Visibility.Visible;
			} else if (Lightning.animations) {
				this.ComboBoxAnimations.Visibility = Visibility.Visible;
				this.GroupBoxSpeedSetting.Visibility = Visibility.Visible;
			}
		}
		private void hideAllAlgsSettings() {
			this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Hidden;
			this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Hidden;
			this.GroupBoxEffectRandomSettings.Visibility = Visibility.Hidden;
			this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Hidden;
			this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Hidden;
		}
		private void showAllAlgsSettings() {
			if (Lightning.effects) {
				if (this.ComboBoxEffects.SelectedIndex == 0)
					this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Visible;
				else if (this.ComboBoxEffects.SelectedIndex == 1)
					this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Visible;
				else if (this.ComboBoxEffects.SelectedIndex == 2)
					this.GroupBoxEffectRandomSettings.Visibility = Visibility.Visible;
			} else if (Lightning.animations) {
				if (this.ComboBoxEffects.SelectedIndex == 0)
					this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
				else if (this.ComboBoxEffects.SelectedIndex == 2)
					this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Visible;
			}
		}
	}
}
*/
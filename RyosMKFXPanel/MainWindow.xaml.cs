using RyosMKFXPanel.Animations;
using RyosMKFXPanel.Effects;
using System;
using System.Windows;
using System.Windows.Media;

namespace RyosMKFXPanel {
    public partial class MainWindow :Window {
        public MainWindow() {
            InitializeComponent();
        }
        bool effects = true;
        bool animations = false;
        bool statics = false;
        bool games = false;

        private void ButtonEffects_Click(object sender, RoutedEventArgs e) {
            if (!(this.LabelMode.Content.ToString() == "Effect:")) {
                this.LabelMode.Content = "Effect:";
                this.ComboBoxEffects.Visibility = Visibility.Visible;
                this.GroupBoxDelaySetting.Visibility = Visibility.Visible;
                this.ComboBoxAnimations.Visibility = Visibility.Hidden;
                this.GroupBoxSpeedSetting.Visibility = Visibility.Hidden;
                if (Lightning.getStatus()) {
                    offLightAlgs();
                    effects = true;
                    animations = false;
                    statics = false;
                    games = false;
                    onLightAlgs();
                } else {
                    effects = true;
                    animations = false;
                    statics = false;
                    games = false;
                }
                hideAlgsSettings();
                if (this.ComboBoxEffects.SelectedIndex == 0) {
                    //Random
                    this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Visible;
                } else if (this.ComboBoxEffects.SelectedIndex == 1) {
                    //Random
                    this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Visible;
                } else if (this.ComboBoxEffects.SelectedIndex == 2) {
                    //Random
                    this.GroupBoxEffectRandomSettings.Visibility = Visibility.Visible;
                }
            }
        }
        private void ButtonAnimations_Click(object sender, RoutedEventArgs e) {
            if (!(this.LabelMode.Content.ToString() == "Animation:")) {
                this.LabelMode.Content = "Animation:";
                this.ComboBoxAnimations.Visibility = Visibility.Visible;
                this.GroupBoxSpeedSetting.Visibility = Visibility.Visible;
                this.ComboBoxEffects.Visibility = Visibility.Hidden;
                this.GroupBoxDelaySetting.Visibility = Visibility.Hidden;
                if (Lightning.getStatus()) {
                    offLightAlgs();
                    effects = false;
                    animations = true;
                    statics = false;
                    games = false;
                    onLightAlgs();
                } else {
                    effects = false;
                    animations = true;
                    statics = false;
                    games = false;
                }
                hideAlgsSettings();
                if (this.ComboBoxAnimations.SelectedIndex == 0) {
                    //Timer
                    this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
                } else if (this.ComboBoxAnimations.SelectedIndex == 2) {
                    //Waterfall
                    this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Visible;
                }
            }
        }

        private void ButtonLever_Click(object sender, RoutedEventArgs e) {
            if (!Lightning.getStatus()) {
                if (Lightning.connect()) {
                    if (Lightning.changeStatus()) {
                        this.LabelStatus.Content = "ON";
                        BrushConverter converter = new System.Windows.Media.BrushConverter();
                        this.LabelStatus.Foreground = (Brush)converter.ConvertFromString("#FF008800");
                        this.ButtonLever.Content = "Stop";
                        onLightAlgs();
                    }
                } else {
                    MessageBox.Show("Can't start a work with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else {
                if (Lightning.changeStatus()) {
                    this.LabelStatus.Content = "OFF";
                    BrushConverter converter = new System.Windows.Media.BrushConverter();
                    this.LabelStatus.Foreground = (Brush)converter.ConvertFromString("#FFFF0000");
                    this.ButtonLever.Content = "Start";
                    offLightAlgs();
                }
                Lightning.disconnect();
            }
        }

        private void SliderDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            this.LabelDelay.Content = "Delay: " + this.SliderDelay.Value.ToString() + "ms";
            Lightning.delay = Convert.ToInt32(this.SliderDelay.Value);
        }
        private void SliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            this.LabelSpeed.Content = "Speed: " + this.SliderSpeed.Value.ToString() + "x";
            Lightning.speed = (float)this.SliderSpeed.Value;
        }
        private void SliderBright_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            this.LabelBright.Content = "Bright: " + this.SliderBright.Value.ToString() + "%";
            Lightning.maxBright = (float)this.SliderBright.Value / 100f;
        }

        private void TextBoxRed_TextChanged(object sender, RoutedEventArgs e) {
            int x = 0;
            if (int.TryParse(this.TextBoxRed.Text, out x)) {
                if (x >= 0 && x <= 255) {
                    Lightning.red = (byte)x;
                }
            }
        }
        private void TextBoxGreen_TextChanged(object sender, RoutedEventArgs e) {
            int x = 0;
            if (int.TryParse(this.TextBoxGreen.Text, out x)) {
                if (x >= 0 && x <= 255) {
                    Lightning.green = (byte)x;
                }
            }
        }
        private void TextBoxBlue_TextChanged(object sender, RoutedEventArgs e) {
            int x = 0;
            if (int.TryParse(this.TextBoxBlue.Text, out x)) {
                if (x >= 0 && x <= 255) {
                    Lightning.blue = (byte)x;
                }
            }
        }

        private void Window_Close(object sender, System.ComponentModel.CancelEventArgs e) {
            offLightAlgs();
            Lightning.disconnect();
        }

        bool SliderBugFixer = false;
        private void ComboBoxEffects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            if (SliderBugFixer) {
                if (effects) {
                    if (this.ComboBoxEffects.SelectedIndex == 0) {
                        //Equalizer
                        this.SliderDelay.Value = 10;
                        this.SliderDelay.Minimum = 5;
                        this.SliderDelay.Maximum = 20;
                        hideAlgsSettings();
                        this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Visible;
                        if (Lightning.getStatus()) {
                            offLightAlgs();
                            onLightAlgs();
                        }
                    } else if (this.ComboBoxEffects.SelectedIndex == 1) {
                        //Volume
                        this.SliderDelay.Value = 20;
                        this.SliderDelay.Minimum = 15;
                        this.SliderDelay.Maximum = 30;
                        hideAlgsSettings();
                        this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Visible;
                        if (Lightning.getStatus()) {
                            offLightAlgs();
                            onLightAlgs();
                        }
                    } else if (this.ComboBoxEffects.SelectedIndex == 2) {
                        //Random
                        this.SliderDelay.Value = 20;
                        this.SliderDelay.Minimum = 15;
                        this.SliderDelay.Maximum = 30;
                        hideAlgsSettings();
                        this.GroupBoxEffectRandomSettings.Visibility = Visibility.Visible;
                        if (Lightning.getStatus()) {
                            offLightAlgs();
                            onLightAlgs();
                        }
                    }
                }
            } else {
                SliderBugFixer = true;
            }
        }

        private void ComboBoxAnimations_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            if (animations) {
                if (this.ComboBoxAnimations.SelectedIndex == 0) {
                    //Timer
                    this.SliderSpeed.Value = 1;
                    this.SliderSpeed.Minimum = 0.5;
                    this.SliderSpeed.Maximum = 5;
                    hideAlgsSettings();
                    this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
                    if (Lightning.getStatus()) {
                        offLightAlgs();
                        onLightAlgs();
                    }
                } else if (this.ComboBoxAnimations.SelectedIndex == 1) {
                    //Smiles
                    this.SliderSpeed.Value = 1;
                    this.SliderSpeed.Minimum = 0.5;
                    this.SliderSpeed.Maximum = 5;
                    hideAlgsSettings();
                    if (Lightning.getStatus()) {
                        offLightAlgs();
                        onLightAlgs();
                    }
                } else if (this.ComboBoxAnimations.SelectedIndex == 2) {
                    //Waterfall
                    this.SliderSpeed.Value = 10;
                    this.SliderSpeed.Minimum = 10;
                    this.SliderSpeed.Maximum = 20;
                    hideAlgsSettings();
                    this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Visible;
                    if (Lightning.getStatus()) {
                        offLightAlgs();
                        onLightAlgs();
                    }
                }
            }
        }
        private void ChekBoxStaticVolumeTurn_Checked(object sender, RoutedEventArgs e) {
            Equalizer.turnVolume();
        }
        private void TextBoxVolumeBorderSize_TextChanged(object sender, RoutedEventArgs e) {
            Equalizer.staticVolumeSize = Convert.ToInt32(this.TextBoxVolumeBorderSize.Text);
        }
        private void TextBoxStartColumn_TextChanged(object sender, RoutedEventArgs e) {
            int x = 1;
            if (int.TryParse(this.TextBoxStartColumn.Text, out x)) {
                if (x < 1) {
                    Equalizer.startColumn = 1;
                } else if (x > 23) {
                    Equalizer.startColumn = 23;
                } else {
                    Equalizer.startColumn = x;
                }
            }
        }
        private void TextBoxEndColumn_TextChanged(object sender, RoutedEventArgs e) {
            int x = 2;
            if (int.TryParse(this.TextBoxEndColumn.Text, out x)) {
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
                if (x < 0) {
                    Equalizer.minHz = 0;
                } else if (x > 23) {
                    Equalizer.minHz = 23;
                } else if (x > Equalizer.maxHz) {
                    x = Equalizer.maxHz - 1;
                } else {
                    Equalizer.minHz = x;
                }
            }
        }
        private void TextBoxMinFreq_TextChanged(object sender, RoutedEventArgs e) {
            int x = 0;
            if (int.TryParse(this.TextBoxMinFreq.Text, out x)) {
                if (x < 0) {
                    Equalizer.minHz = 0;
                } else if (x > 23) {
                    Equalizer.minHz = 23;
                } else if (x < Equalizer.minHz) {
                    x = Equalizer.minHz + 1;
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
            Timer.simpleNumbers();
        }
        private void ChekBoxTimerInvert_Checked(object sender, RoutedEventArgs e) {
            Timer.invertColor();
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

        private void offLightAlgs() {
            if (effects) {
                if (Equalizer.getState()) {
                    Equalizer.stop();
                } else if (Volume.getState()) {
                    Volume.stop();
                } else if (Effects.Random.getState()) {
                    Effects.Random.stop();
                }
            } 
            else if (animations) {
                if (Timer.getState()) {
                    Timer.stop();
                } else if (Smiles.getState()) {
                    Smiles.stop();
                } else if (Waterfall.getState()) {
                    Waterfall.stop();
                } else if (NyanCat.getState()) {
                    NyanCat.stop();
                }
            }
        }
        private void onLightAlgs() {
            if (effects) {
                if (this.ComboBoxEffects.SelectedIndex == 0) {
                    Equalizer.start();
                } else if (this.ComboBoxEffects.SelectedIndex == 1) {
                    Volume.start();
                } else if (this.ComboBoxEffects.SelectedIndex == 2) {
                    Effects.Random.start();
                }
            } else if (animations) {
                if (this.ComboBoxAnimations.SelectedIndex == 0) {
                    Timer.start();
                } else if (this.ComboBoxAnimations.SelectedIndex == 1) {
                    Smiles.start();
                } else if (this.ComboBoxAnimations.SelectedIndex == 2) {
                    Waterfall.start();
                } else if (this.ComboBoxAnimations.SelectedIndex == 3) {
                    NyanCat.start();
                }
            }
        }
        private void hideAlgsSettings() {
            this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Hidden;
            this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Hidden;
            this.GroupBoxEffectRandomSettings.Visibility = Visibility.Hidden;
            this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Hidden;
            this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Hidden;
        }

    }
}

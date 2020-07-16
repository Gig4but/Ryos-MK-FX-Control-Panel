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
                this.ComboBoxAnimations.Visibility = Visibility.Hidden;
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
                if (ComboBoxEffects.SelectedIndex == 2) {
                    //Random
                    this.GroupBoxEffectRandomSettings.Visibility = Visibility.Visible;
                }
            }
        }
        private void ButtonAnimations_Click(object sender, RoutedEventArgs e) {
            if (!(this.LabelMode.Content.ToString() == "Animation:")) {
                this.LabelMode.Content = "Animation:";
                this.ComboBoxAnimations.Visibility = Visibility.Visible;
                this.ComboBoxEffects.Visibility = Visibility.Hidden;
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
                if (ComboBoxAnimations.SelectedIndex == 0) {
                    //Timer
                    this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
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
                    if (ComboBoxEffects.SelectedIndex == 0) {
                        //Equalizer
                        this.SliderDelay.Value = 10;
                        this.SliderDelay.Minimum = 5;
                        this.SliderDelay.Maximum = 20;
                        hideAlgsSettings();

                        if (Lightning.getStatus()) {
                            offLightAlgs();
                            onLightAlgs();
                        }
                    } else if (ComboBoxEffects.SelectedIndex == 1) {
                        //Volume
                        this.SliderDelay.Value = 20;
                        this.SliderDelay.Minimum = 15;
                        this.SliderDelay.Maximum = 30;
                        hideAlgsSettings();

                        if (Lightning.getStatus()) {
                            offLightAlgs();
                            onLightAlgs();
                        }
                    } else if (ComboBoxEffects.SelectedIndex == 2) {
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
                if (ComboBoxAnimations.SelectedIndex == 0) {
                    //Timer
                    hideAlgsSettings();
                    this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
                    if (Lightning.getStatus()) {
                        offLightAlgs();
                        onLightAlgs();
                    }
                }
            }
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
                }
            }
        }
        private void onLightAlgs() {
            if (effects) {
                if (ComboBoxEffects.SelectedIndex == 0) {
                    Equalizer.start();
                } else if (ComboBoxEffects.SelectedIndex == 1) {
                    Volume.start();
                } else if (ComboBoxEffects.SelectedIndex == 2) {
                    Effects.Random.start();
                }
            } else if (animations) {
                if (ComboBoxAnimations.SelectedIndex == 0) {
                    Timer.start();
                }
            }
        }
        private void hideAlgsSettings() {
            this.GroupBoxEffectRandomSettings.Visibility = Visibility.Hidden;
            this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Hidden;
        }

    }
}

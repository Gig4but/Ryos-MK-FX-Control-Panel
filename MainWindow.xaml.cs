using RyosMKFXPanel.Effects;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RyosMKFXPanel {
    public partial class MainWindow :Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void ButtonLever_Click(object sender, RoutedEventArgs e) {
            if (!Lightning.getStatus()) {
                if (Lightning.connect()) {
                    if (Lightning.changeStatus()) {
                        this.LabelStatus.Content = "ON";
                        BrushConverter converter = new System.Windows.Media.BrushConverter();
                        this.LabelStatus.Foreground = (Brush)converter.ConvertFromString("#FF008800");
                        this.ButtonLever.Content = "Stop";
                        if (ComboBoxEffects.SelectedIndex == 0) {
                            Equalizer.start();
                        } else if (ComboBoxEffects.SelectedIndex == 1) {
                            Volume.start();
                        } else if (ComboBoxEffects.SelectedIndex == 2) {
                            Effects.Random.start();
                        }
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
                    if (ComboBoxEffects.SelectedIndex == 0) {
                        Equalizer.stop();
                    } else if (ComboBoxEffects.SelectedIndex == 1) {
                        Volume.stop();
                    } else if (ComboBoxEffects.SelectedIndex == 2) {
                        Effects.Random.stop();
                    }
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
            Lightning.disconnect();
        }

        bool SliderBugFixer = false;
        int selectedIndex = 0;
        private void ComboBoxEffects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            if (SliderBugFixer) {
                if (ComboBoxEffects.SelectedIndex == 0) {
                    this.SliderDelay.Value = 10;
                    this.SliderDelay.Minimum = 5;
                    this.SliderDelay.Maximum = 20;

                    if (Lightning.getStatus()) {
                        if (selectedIndex == 1) {
                            Volume.stop();
                        } else if (selectedIndex == 2) {
                            Effects.Random.stop();
                        }
                        Equalizer.start();
                    }
                    selectedIndex = 0;
                } else if (ComboBoxEffects.SelectedIndex == 1) {
                    this.SliderDelay.Value = 20;
                    this.SliderDelay.Minimum = 15;
                    this.SliderDelay.Maximum = 30;

                    if (Lightning.getStatus()) {
                        if (selectedIndex == 0) {
                            Equalizer.stop();
                        } else if (selectedIndex == 2) {
                            Effects.Random.stop();
                        }
                        Volume.start();
                    }
                    selectedIndex = 1;
                } else if (ComboBoxEffects.SelectedIndex == 2) {
                    this.SliderDelay.Value = 20;
                    this.SliderDelay.Minimum = 15;
                    this.SliderDelay.Maximum = 30;

                    if (Lightning.getStatus()) {
                        if (selectedIndex == 0) {
                            Equalizer.stop();
                        } else if (selectedIndex == 1) {
                            Volume.stop();
                        }
                        Effects.Random.start();
                    }
                    selectedIndex = 2;
                }
            } else {
                SliderBugFixer = true;
            }
        }
    }
}

using RyosMKFXPanel.Animations;
using RyosMKFXPanel.Effects;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RyosMKFXPanel.windowStyles {
    public partial class styleNew :Window {
        public styleNew() {
            InitializeComponent();
            //Lightning.runNewAlg(Lightning.prei);
        }
        private void Window_Close(object sender, System.ComponentModel.CancelEventArgs e) {
            Lightning.stopWork();
            Lightning.offLightAlgs();
        }


        //Colors and Brushes
        BrushConverter converterBrush = new BrushConverter();
        SolidColorBrush brush = null;
        Color c = new Color();
        private Color updateColor() {
            c = Color.FromArgb((byte)(0xAA * Lightning.maxBright), Lightning.red, Lightning.green, Lightning.blue);
            this.Resources["ActualColorAA"] = c;
            brush = null;
            brush = new SolidColorBrush(c);
            this.Resources["ActualColorBrushAA"] = brush;

            c = Color.FromArgb((byte)(0xFF * Lightning.maxBright), Lightning.red, Lightning.green, Lightning.blue);
            this.Resources["ActualColorFF"] = c;
            brush = null;
            brush = new SolidColorBrush(c);
            this.Resources["ActualColorBrushFF"] = brush;

            brush = null;
            brush = new SolidColorBrush(c);
            return c;
        }
        private Color resetColor() {
            c = Color.FromArgb(0xAA, 0xFF, 0xFF, 0xFF);
            brush = null;
            brush = new SolidColorBrush(c);
            c = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            return c;
        }


        //Buttons for mode choose
        private void ButtonEffects_Click(object sender, RoutedEventArgs e) {
            if (!Lightning.effects) {
                this.LabelMode.Content = "Effect:";
                hideAllModsOptions();
                Lightning.changeMode(0);
                showAllModsOptions();
                hideAllAlgsSettings();
                showAllAlgsSettings();

                resetModsButtonsColors();
                updateColor();
                this.ButtonEffectsImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                this.ButtonEffectsText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
            }
        }
        private void ButtonAnimations_Click(object sender, RoutedEventArgs e) {
            if (!Lightning.animations) {
                this.LabelMode.Content = "Animation:";
                hideAllModsOptions();
                Lightning.changeMode(1);
                showAllModsOptions();
                hideAllAlgsSettings();
                showAllAlgsSettings();

                resetModsButtonsColors();
                updateColor();
                this.ButtonAnimationsImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                this.ButtonAnimationsText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
            }
        }

        private void resetModsButtonsColors() {
            resetColor();
            this.ButtonEffectsImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
            this.ButtonEffectsText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");

            this.ButtonAnimationsImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
            this.ButtonAnimationsText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");
        }


        //Button for start work with device
        private void ButtonLever_Click(object sender, RoutedEventArgs e) {
            if (Lightning.switchWork()) {
                this.ButtonLeverText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
                this.RectangleStatusColor.Color = (Color)ColorConverter.ConvertFromString("#FF008800");
            } else {
                this.ButtonLeverText.SetResourceReference(Label.ForegroundProperty, "BlackColorBrushAA");
                this.RectangleStatusColor.Color = (Color)ColorConverter.ConvertFromString("#FF880000");
            }
        }


        //Buttons for effects choose
        private void ButtonEffectsEqualizer_Click(object sender, RoutedEventArgs e) {
            if (Lightning.effects) {
                if (Lightning.ei != 0) {
                    deselectAllButtons();
                    this.ButtonEffectsEqualizer.SetResourceReference(Button.StyleProperty, "EffectButtonSelected");
                    this.ButtonEffectsEqualizerImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                    this.ButtonEffectsEqualizerText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
                    this.ButtonEffectsEqualizerBorder.SetResourceReference(Border.BorderBrushProperty, "ActualColorBrushAA");

                    Lightning.runNewAlg(0);

                    this.SliderDelay.Maximum = 3;
                    this.SliderDelay.Minimum = 1;
                    this.SliderDelay.Value = 3;
                    hideAllAlgsSettings();
                    this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Visible;
                }
            }
        }
        private void ButtonEffectsVolume_Click(object sender, RoutedEventArgs e) {
            if (Lightning.effects) {
                if (Lightning.ei != 1) {
                    deselectAllButtons();
                    this.ButtonEffectsVolume.SetResourceReference(Button.StyleProperty, "EffectButtonSelected");
                    this.ButtonEffectsVolumeImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                    this.ButtonEffectsVolumeText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
                    this.ButtonEffectsVolumeBorder.SetResourceReference(Border.BorderBrushProperty, "ActualColorBrushAA");

                    Lightning.runNewAlg(1);

                    this.SliderDelay.Maximum = 30;
                    this.SliderDelay.Minimum = 15;
                    this.SliderDelay.Value = 20;
                    hideAllAlgsSettings();
                    this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Visible;
                }
            }
        }
        private void ButtonEffectsRandom_Click(object sender, RoutedEventArgs e) {
            if (Lightning.effects) {
                if (Lightning.ei != 2) {
                    deselectAllButtons();
                    this.ButtonEffectsRandom.SetResourceReference(Button.StyleProperty, "EffectButtonSelected");
                    this.ButtonEffectsRandomImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                    this.ButtonEffectsRandomText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
                    this.ButtonEffectsRandomBorder.SetResourceReference(Border.BorderBrushProperty, "ActualColorBrushAA");

                    Lightning.runNewAlg(2);

                    this.SliderDelay.Maximum = 30;
                    this.SliderDelay.Minimum = 15;
                    this.SliderDelay.Value = 20;
                    hideAllAlgsSettings();
                    this.GroupBoxEffectRandomSettings.Visibility = Visibility.Visible;
                }
            }
        }

        private void ButtonAnimationsTimer_Click(object sender, RoutedEventArgs e) {
            if (Lightning.animations) {
                if (Lightning.ai != 0) {
                    deselectAllButtons();
                    this.ButtonAnimationsTimer.SetResourceReference(Button.StyleProperty, "EffectButtonSelected");
                    this.ButtonAnimationsTimerImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                    this.ButtonAnimationsTimerText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
                    this.ButtonAnimationsTimerBorder.SetResourceReference(Border.BorderBrushProperty, "ActualColorBrushAA");

                    Lightning.runNewAlg(0);

                    this.SliderSpeed.Value = 1;
                    this.SliderSpeed.Minimum = 0.5;
                    this.SliderSpeed.Maximum = 5;
                    hideAllAlgsSettings();
                    this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
                }
            }
        }
        private void ButtonAnimationsSmiles_Click(object sender, RoutedEventArgs e) {
            if (Lightning.animations) {
                if (Lightning.ai != 1) {
                    deselectAllButtons();
                    this.ButtonAnimationsSmiles.SetResourceReference(Button.StyleProperty, "EffectButtonSelected");
                    this.ButtonAnimationsSmilesImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                    this.ButtonAnimationsSmilesText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
                    this.ButtonAnimationsSmilesBorder.SetResourceReference(Border.BorderBrushProperty, "ActualColorBrushAA");

                    Lightning.runNewAlg(1);

                    this.SliderSpeed.Value = 1;
                    this.SliderSpeed.Minimum = 0.5;
                    this.SliderSpeed.Maximum = 5;
                    hideAllAlgsSettings();
                }
            }
        }
        private void ButtonAnimationsWaterfall_Click(object sender, RoutedEventArgs e) {
            if (Lightning.animations) {
                if (Lightning.ai != 2) {
                    deselectAllButtons();
                    this.ButtonAnimationsWaterfall.SetResourceReference(Button.StyleProperty, "EffectButtonSelected");
                    this.ButtonAnimationsWaterfallImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                    this.ButtonAnimationsWaterfallText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
                    this.ButtonAnimationsWaterfallBorder.SetResourceReference(Border.BorderBrushProperty, "ActualColorBrushAA");

                    Lightning.runNewAlg(2);

                    this.SliderSpeed.Value = 10;
                    this.SliderSpeed.Minimum = 10;
                    this.SliderSpeed.Maximum = 20;
                    hideAllAlgsSettings();
                    this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Visible;
                }
            }
        }
        private void ButtonAnimationsNyanCat_Click(object sender, RoutedEventArgs e) {
            if (Lightning.animations) {
                if (Lightning.ai != 3) {
                    deselectAllButtons();
                    this.ButtonAnimationsNyanCat.SetResourceReference(Button.StyleProperty, "EffectButtonSelected");
                    this.ButtonAnimationsNyanCatImageBrush.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
                    this.ButtonAnimationsNyanCatText.SetResourceReference(Label.ForegroundProperty, "ActualColorBrushAA");
                    this.ButtonAnimationsNyanCatBorder.SetResourceReference(Border.BorderBrushProperty, "ActualColorBrushAA");

                    Lightning.runNewAlg(3);

                    this.SliderSpeed.Value = 1;
                    this.SliderSpeed.Minimum = 0.5;
                    this.SliderSpeed.Maximum = 5;
                    hideAllAlgsSettings();
                }
            }
        }

        private void deselectAllButtons() {
            if (Lightning.effects) {
                if (Lightning.ei == 0) {
                    this.ButtonEffectsEqualizer.SetResourceReference(Button.StyleProperty, "EffectButton");
                    this.ButtonEffectsEqualizerImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
                    this.ButtonEffectsEqualizerText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");
                    this.ButtonEffectsEqualizerBorder.SetResourceReference(Border.BorderBrushProperty, "DefaultColorBrushAA");
                } else if (Lightning.ei == 1) {
                    this.ButtonEffectsVolume.SetResourceReference(Button.StyleProperty, "EffectButton");
                    this.ButtonEffectsVolumeImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
                    this.ButtonEffectsVolumeText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");
                    this.ButtonEffectsVolumeBorder.SetResourceReference(Border.BorderBrushProperty, "DefaultColorBrushAA");
                } else if (Lightning.ei == 2) {
                    this.ButtonEffectsRandom.SetResourceReference(Button.StyleProperty, "EffectButton");
                    this.ButtonEffectsRandomImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
                    this.ButtonEffectsRandomText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");
                    this.ButtonEffectsRandomBorder.SetResourceReference(Border.BorderBrushProperty, "DefaultColorBrushAA");
                }
            } else if (Lightning.animations) {
                if (Lightning.ai == 0) {
                    this.ButtonAnimationsTimer.SetResourceReference(Button.StyleProperty, "EffectButton");
                    this.ButtonAnimationsTimerImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
                    this.ButtonAnimationsTimerText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");
                    this.ButtonAnimationsTimerBorder.SetResourceReference(Border.BorderBrushProperty, "DefaultColorBrushAA");
                } else if (Lightning.ai == 1) {
                    this.ButtonAnimationsSmiles.SetResourceReference(Button.StyleProperty, "EffectButton");
                    this.ButtonAnimationsSmilesImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
                    this.ButtonAnimationsSmilesText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");
                    this.ButtonAnimationsSmilesBorder.SetResourceReference(Border.BorderBrushProperty, "DefaultColorBrushAA");
                } else if (Lightning.ai == 2) {
                    this.ButtonAnimationsWaterfall.SetResourceReference(Button.StyleProperty, "EffectButton");
                    this.ButtonAnimationsWaterfallImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
                    this.ButtonAnimationsWaterfallText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");
                    this.ButtonAnimationsWaterfallBorder.SetResourceReference(Border.BorderBrushProperty, "DefaultColorBrushAA");
                } else if (Lightning.ai == 3) {
                    this.ButtonAnimationsNyanCat.SetResourceReference(Button.StyleProperty, "EffectButton");
                    this.ButtonAnimationsNyanCatImageBrush.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
                    this.ButtonAnimationsNyanCatText.SetResourceReference(Label.ForegroundProperty, "DefaultColorBrushAA");
                    this.ButtonAnimationsNyanCatBorder.SetResourceReference(Border.BorderBrushProperty, "DefaultColorBrushAA");
                }
            }
        }
        private void hideAllModsOptions() {
            this.GroupBoxDelaySetting.Visibility = Visibility.Hidden;
            this.GroupBoxSpeedSetting.Visibility = Visibility.Hidden;
            this.ScrollEffects.Visibility = Visibility.Hidden;
            this.ScrollAnimations.Visibility = Visibility.Hidden;
        }
        private void showAllModsOptions() {
            if (Lightning.effects) {
                this.ScrollEffects.Visibility = Visibility.Visible;
                this.GroupBoxDelaySetting.Visibility = Visibility.Visible;
            } else if (Lightning.animations) {
                this.ScrollAnimations.Visibility = Visibility.Visible;
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
                if (Lightning.ei == 0)
                    this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Visible;
                else if (Lightning.ei == 1)
                    this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Visible;
                else if (Lightning.ei == 2)
                    this.GroupBoxEffectRandomSettings.Visibility = Visibility.Visible;
            } else if (Lightning.animations) {
                if (Lightning.ai == 0)
                    this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
                else if (Lightning.ai == 2)
                    this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Visible;
            }
        }


        //Sliders for delay, speed and brightness
        private void SliderDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (Lightning.ei == 0) {
                this.LabelDelay.Content = "Speed: " + this.SliderDelay.Value.ToString() + "x";
                Lightning.changeDelay(Convert.ToInt32(this.SliderDelay.Value));
                Lightning.restartAlg();
            } else {
                this.LabelDelay.Content = "Delay: " + this.SliderDelay.Value.ToString() + "ms";
                Lightning.changeDelay(Convert.ToInt32(this.SliderDelay.Value));
            }
        }
        private void SliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (Math.Round(this.SliderSpeed.Value) == this.SliderSpeed.Value) {
                this.LabelSpeed.Content = "Speed: " + this.SliderSpeed.Value.ToString() + ",0x";
            } else {
                this.LabelSpeed.Content = "Speed: " + this.SliderSpeed.Value.ToString() + "x";
            }
            Lightning.changeSpeed((float)this.SliderSpeed.Value);
        }

        private void SliderBright_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            double x = this.SliderBright.Value;
            if (this.SliderBright.Value < 10) {
                this.LabelBright.Content = "Brightness:     " + this.SliderBright.Value.ToString() + "%";
            } else if (this.SliderBright.Value < 100) {
                this.LabelBright.Content = "Brightness:   " + this.SliderBright.Value.ToString() + "%";
            } else {
                this.LabelBright.Content = "Brightness: " + this.SliderBright.Value.ToString() + "%";
            }
            Lightning.changeBrightness((float)this.SliderBright.Value / 100f);
            updateColor();
        }



        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            int x = 0;
            if (int.TryParse(e.Text, out x)) {
                e.Handled = false;
            } else {
                e.Handled = true;
            }
        }
        int tbred = Lightning.red;
        int tbgreen = Lightning.green;
        int tbblue = Lightning.blue;
        private void TextBoxRed_TextChanged(object sender, RoutedEventArgs e) {
            if (TextBoxRed != null && TextBoxGreen != null && TextBoxBlue != null && TextBoxHex != null) {
                int x = 0;
                if (int.TryParse(this.TextBoxRed.Text, out x)) {
                    if (Regex.IsMatch(this.TextBoxRed.Text, "0/d")) {
                        this.TextBoxRed.Text = this.TextBoxRed.Text.Substring(1);
                        this.TextBoxRed.CaretIndex = this.TextBoxRed.Text.Length;
                    }
                    if (x >= 0 && x <= 255) {
                        Lightning.changeColorRed((byte)x);
                        tbred = x;
                        updateColor();

                        string s = Convert.ToString(x, 16).ToUpper();
                        if (!Regex.IsMatch(s, ".{2}")) {
                            s = "0" + s;
                        }
                        s = s + TextBoxHex.Text.Substring(2, 4);
                        if (TextBoxHex.Text != s)
                            TextBoxHex.Text = s;
                    } else {
                        this.TextBoxRed.Text = tbred.ToString();
                        this.TextBoxRed.CaretIndex = this.TextBoxRed.Text.Length;
                    }
                }
            }
        }
        private void TextBoxGreen_TextChanged(object sender, RoutedEventArgs e) {
            if (TextBoxRed != null && TextBoxGreen != null && TextBoxBlue != null && TextBoxHex != null) {
                int x = 0;
                if (int.TryParse(this.TextBoxGreen.Text, out x)) {
                    if (Regex.IsMatch(this.TextBoxGreen.Text, "0/d")) {
                        this.TextBoxGreen.Text = this.TextBoxGreen.Text.Substring(1);
                        this.TextBoxGreen.CaretIndex = this.TextBoxGreen.Text.Length;
                    }
                    if (x >= 0 && x <= 255) {
                        Lightning.changeColorGreen((byte)x);
                        tbgreen = x;
                        updateColor();

                        string s = Convert.ToString(x, 16).ToUpper();
                        if (!Regex.IsMatch(s, ".{2}")) {
                            s = "0" + s;
                        }
                        s = TextBoxHex.Text.Substring(0, 2) + s + TextBoxHex.Text.Substring(4, 2);
                        if (TextBoxHex.Text != s)
                            TextBoxHex.Text = s;
                    } else {
                        this.TextBoxGreen.Text = tbgreen.ToString();
                        this.TextBoxGreen.CaretIndex = this.TextBoxGreen.Text.Length;
                    }
                }
            }
        }
        private void TextBoxBlue_TextChanged(object sender, RoutedEventArgs e) {
            if (TextBoxRed != null && TextBoxGreen != null && TextBoxBlue != null && TextBoxHex != null) {
                int x = 0;
                if (int.TryParse(this.TextBoxBlue.Text, out x)) {
                    if (Regex.IsMatch(this.TextBoxBlue.Text, "0/d")) {
                        this.TextBoxBlue.Text = this.TextBoxBlue.Text.Substring(1);
                        this.TextBoxBlue.CaretIndex = this.TextBoxBlue.Text.Length;
                    }
                    if (x >= 0 && x <= 255) {
                        Lightning.changeColorBlue((byte)x);
                        tbblue = x;
                        updateColor();

                        string s = Convert.ToString(x, 16).ToUpper();
                        if (!Regex.IsMatch(s, ".{2}")) {
                            s = "0" + s;
                        }
                        s = TextBoxHex.Text.Substring(0, 4) + s;
                        if (TextBoxHex.Text != s)
                            TextBoxHex.Text = s;
                    } else {
                        this.TextBoxBlue.Text = tbblue.ToString();
                        this.TextBoxBlue.CaretIndex = this.TextBoxBlue.Text.Length;
                    }
                }
            }
        }

        private void HexValidationTextBox(object sender, TextCompositionEventArgs e) {
            if (Regex.IsMatch(e.Text, "[0-9a-fA-F]*")) {
                e.Handled = false;
            } else {
                e.Handled = true;
            }
        }
        private void TextBoxHex_TextChanged(object sender, RoutedEventArgs e) {
            if (TextBoxRed != null && TextBoxGreen != null && TextBoxBlue != null && TextBoxHex != null) {
                if (Regex.IsMatch(this.TextBoxHex.Text, "[0-9a-fA-F]{6}")) {
                    int r = Convert.ToInt32(this.TextBoxHex.Text.Substring(0, 2), 16);
                    int g = Convert.ToInt32(this.TextBoxHex.Text.Substring(2, 2), 16);
                    int b = Convert.ToInt32(this.TextBoxHex.Text.Substring(4, 2), 16);

                    int x;
                    if (int.TryParse(this.TextBoxRed.Text, out x)) {
                        if (x != r) {
                            this.TextBoxRed.Text = r.ToString();
                            Lightning.changeColorRed((byte)x);
                        }
                    }

                    if (int.TryParse(this.TextBoxGreen.Text, out x)) {
                        if (x != g) {
                            this.TextBoxGreen.Text = g.ToString();
                            Lightning.changeColorGreen((byte)x);
                        }
                    }

                    if (int.TryParse(this.TextBoxBlue.Text, out x)) {
                        if (x != b) {
                            this.TextBoxBlue.Text = b.ToString();
                            Lightning.changeColorBlue((byte)x);
                        }
                    }

                    updateColor();
                }
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
                if (x < Equalizer.minHz + Lightning.kbw) {
                    Equalizer.maxHz = Equalizer.minHz + Lightning.kbw;
                } else {
                    Equalizer.maxHz = x;
                }
            }
            TextBoxMinFreq_TextChanged();
        }
        private void TextBoxMaxFreq_TextChanged() {
            int x = 0;
            if (int.TryParse(this.TextBoxMaxFreq?.Text, out x)) {
                if (x < Equalizer.minHz + Lightning.kbw) {
                    Equalizer.maxHz = Equalizer.minHz + Lightning.kbw;
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
                } else if (x > Equalizer.maxHz - Lightning.kbw) {
                    x = Equalizer.maxHz - Lightning.kbw;
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
                } else if (x > Equalizer.maxHz - Lightning.kbw) {
                    x = Equalizer.maxHz - Lightning.kbw;
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

        
    }
}

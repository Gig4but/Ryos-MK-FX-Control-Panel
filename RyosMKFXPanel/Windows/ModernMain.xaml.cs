using RyosMKFXPanel.Animations;
using RyosMKFXPanel.Effects;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
namespace RyosMKFXPanel.Windows {
    public partial class ModernMain :Window {
        private static bool initialized = false;
        public ModernMain() {
            InitializeComponent();
            Properties.Settings.Default.reset = false;
            if (Properties.Settings.Default.DeviceID != null || Properties.Settings.Default.DeviceID != "") {
                Volume.deviceAutoChange(false);
                Volume.changeListenDeviceById(Properties.Settings.Default.DeviceID);
            }
            switch (Properties.Settings.Default.mode) {
                case 1:
                    ButtonAnimations_Click(null, null);
                    switch (Properties.Settings.Default.ai) {
                        case 1:
                            ButtonAnimationsSmiles_Click(null, null);
                            break;
                        case 2:
                            ButtonAnimationsWaterfall_Click(null, null);
                            break;
                        case 3:
                            ButtonAnimationsNyanCat_Click(null, null);
                            break;
                        default:
                            ButtonAnimationsTimer_Click(null, null);
                            break;
                    }
                    break;
                default:
                    ButtonEffects_Click(null, null);
                    switch (Properties.Settings.Default.ei) {
                        case 1:
                            ButtonEffectsVolume_Click(null, null);
                            break;
                        case 2:
                            ButtonEffectsRandom_Click(null, null);
                            break;
                        default:
                            ButtonEffectsEqualizer_Click(null, null);
                            break;
                    }
                    break;
            }
            ButtonEffectsVolume_Click(null, null);
            ButtonEffectsEqualizer_Click(null, null);
            //Lightning.runNewAlg(Lightning.prei);
            initialized = true;
        }
        private void Window_Close(object sender, System.ComponentModel.CancelEventArgs e) {
            Lightning.stopWork();
            Lightning.offLightAlgs();
            updateSettings();
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
            MouseMove += new System.Windows.Input.MouseEventHandler(MoveWindow);
        }
        private void MoveWindowLock(object sender, RoutedEventArgs e) {
            _windowMoveLock = false;
            MouseMove -= new System.Windows.Input.MouseEventHandler(MoveWindow);
        }

        //Colors and Brushes
        //BrushConverter converterBrush = new BrushConverter();
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

        private void elementsHide(UIElement[] elements) {
            foreach (UIElement i in elements) {
                i.Visibility = Visibility.Hidden;
            }
        }
        private void elementsUnhide(UIElement[] elements) {
            foreach (UIElement i in elements) {
                i.Visibility = Visibility.Visible;
            }
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
                this.ModeLighter.SetValue(Grid.ColumnProperty, 0);
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
                this.ModeLighter.SetValue(Grid.ColumnProperty, 1);
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

        private void ButtonSelect(Button button, Rectangle rectangle, TextBlock textBlock, Border border) {
            button.SetResourceReference(Button.StyleProperty, "EffectButtonSelected");
            rectangle.SetResourceReference(Rectangle.FillProperty, "ActualColorBrushFF");
            textBlock.SetResourceReference(TextBlock.ForegroundProperty, "ActualColorBrushAA");
            border.SetResourceReference(Border.BorderBrushProperty, "ActualColorBrushAA");
        }
        private void ButtonDeselect(Button button, Rectangle rectangle, TextBlock textBlock, Border border) {
            button.SetResourceReference(Button.StyleProperty, "EffectButton");
            rectangle.SetResourceReference(Rectangle.FillProperty, "DefaultColorBrushFF");
            textBlock.SetResourceReference(TextBlock.ForegroundProperty, "DefaultColorBrushAA");
            border.SetResourceReference(Border.BorderBrushProperty, "DefaultColorBrushAA");
        }
        
        private void ButtonEffectsEqualizer_Click(object sender, RoutedEventArgs e) {
            if (Lightning.effects) {
                if (Lightning.ei != 0) {
                    updateSettings();
                    deselectAllButtons();
                    ButtonSelect(this.ButtonEffectsEqualizer, this.ButtonEffectsEqualizerImageBrush, this.ButtonEffectsEqualizerText, this.ButtonEffectsEqualizerBorder);

                    this.SliderDelay.Maximum = 3;
                    this.SliderDelay.Minimum = 1;
                    this.SliderDelay.Value = Properties.Settings.Default.EqualizerDelay;
                    this.SliderBright.Value = Properties.Settings.Default.EqualizerBrightness;
                    this.TextBoxRed.Text = Properties.Settings.Default.EqualizerRed.ToString();
                    this.TextBoxGreen.Text = Properties.Settings.Default.EqualizerGreen.ToString();
                    this.TextBoxBlue.Text = Properties.Settings.Default.EqualizerBlue.ToString();

                    hideAllAlgsSettings();
                    this.GroupBoxEffectEqualizerSettings.Visibility = Visibility.Visible;
                    this.TextBoxMinFreq.Text = Properties.Settings.Default.EqualizerFrequenciesMin.ToString();
                    this.TextBoxMaxFreq.Text = Properties.Settings.Default.EqualizerFrequenciesMax.ToString();
                    this.CheckBoxEffectsEqualizerAntialiasingTurn.IsChecked = Properties.Settings.Default.EqualizerAntialiasing;
                    this.TextBoxEffectsEqualizerStartColumn.Text = Properties.Settings.Default.EqualizerAverageIntensityMin.ToString();
                    this.TextBoxEffectsEqualizerEndColumn.Text = Properties.Settings.Default.EqualizerAverageIntensityMax.ToString();
                    this.CheckBoxEffectsEqualizerStaticVolumeTurn.IsChecked = Properties.Settings.Default.EqualizerStaticVolumeBorder;
                    this.TextBoxEffectsEqualizerVolumeBorderSize.Text = Properties.Settings.Default.EqualizerMaxVolume.ToString();

                    Lightning.runNewAlg(0);
                }
            }
        }
        private void ButtonEffectsVolume_Click(object sender, RoutedEventArgs e) {
            if (Lightning.effects) {
                if (Lightning.ei != 1) {
                    updateSettings();
                    deselectAllButtons();
                    ButtonSelect(this.ButtonEffectsVolume, this.ButtonEffectsVolumeImageBrush, this.ButtonEffectsVolumeText, this.ButtonEffectsVolumeBorder);

                    this.SliderDelay.Maximum = 30;
                    this.SliderDelay.Minimum = 15;
                    this.SliderDelay.Value = Properties.Settings.Default.VolumeDelay;
                    this.SliderBright.Value = Properties.Settings.Default.VolumeBrightness;
                    this.TextBoxRed.Text = Properties.Settings.Default.VolumeRed.ToString();
                    this.TextBoxGreen.Text = Properties.Settings.Default.VolumeGreen.ToString();
                    this.TextBoxBlue.Text = Properties.Settings.Default.VolumeBlue.ToString();

                    hideAllAlgsSettings();
                    this.GroupBoxEffectVolumeSettings.Visibility = Visibility.Visible;
                    this.CheckBoxVolumeSimple.IsChecked = Properties.Settings.Default.VolumeSimpleVolume;

                    Lightning.runNewAlg(1);
                }
            }
        }
        private void ButtonEffectsRandom_Click(object sender, RoutedEventArgs e) {
            if (Lightning.effects) {
                if (Lightning.ei != 2) {
                    updateSettings();
                    deselectAllButtons();
                    ButtonSelect(this.ButtonEffectsRandom, this.ButtonEffectsRandomImageBrush, this.ButtonEffectsRandomText, this.ButtonEffectsRandomBorder);

                    this.SliderDelay.Maximum = 30;
                    this.SliderDelay.Minimum = 15;
                    this.SliderDelay.Value = Properties.Settings.Default.RandomDelay;
                    this.SliderBright.Value = Properties.Settings.Default.RandomBrightness;
                    this.TextBoxRed.Text = Properties.Settings.Default.RandomRed.ToString();
                    this.TextBoxGreen.Text = Properties.Settings.Default.RandomGreen.ToString();
                    this.TextBoxBlue.Text = Properties.Settings.Default.RandomBlue.ToString();
                    
                    hideAllAlgsSettings();
                    this.GroupBoxEffectRandomSettings.Visibility = Visibility.Visible;
                    this.CheckBoxRandomTurn.IsChecked = Properties.Settings.Default.RandomOnOffLeds;

                    Lightning.runNewAlg(2);
                }
            }
        }

        private void ButtonAnimationsTimer_Click(object sender, RoutedEventArgs e) {
            if (Lightning.animations) {
                if (Lightning.ai != 0) {
                    updateSettings();
                    deselectAllButtons();
                    ButtonSelect(this.ButtonAnimationsTimer, this.ButtonAnimationsTimerImageBrush, this.ButtonAnimationsTimerText, this.ButtonAnimationsTimerBorder);

                    this.SliderSpeed.Maximum = 5;
                    this.SliderSpeed.Minimum = 0.5;
                    this.SliderSpeed.Value = Properties.Settings.Default.TimerSpeed;
                    this.SliderBright.Value = Properties.Settings.Default.TimerBrightness;
                    this.TextBoxRed.Text = Properties.Settings.Default.TimerRed.ToString();
                    this.TextBoxGreen.Text = Properties.Settings.Default.TimerGreen.ToString();
                    this.TextBoxBlue.Text = Properties.Settings.Default.TimerBlue.ToString();

                    hideAllAlgsSettings();
                    this.GroupBoxAnimationTimerSettings.Visibility = Visibility.Visible;
                    this.CheckBoxTimerSimple.IsChecked = Properties.Settings.Default.TimerSimpleNumbers;
                    this.CheckBoxTimerInvert.IsChecked = Properties.Settings.Default.TimerInvertColors;

                    Lightning.runNewAlg(0);
                }
            }
        }
        private void ButtonAnimationsSmiles_Click(object sender, RoutedEventArgs e) {
            if (Lightning.animations) {
                if (Lightning.ai != 1) {
                    updateSettings();
                    deselectAllButtons();
                    ButtonSelect(this.ButtonAnimationsSmiles, this.ButtonAnimationsSmilesImageBrush, this.ButtonAnimationsSmilesText, this.ButtonAnimationsSmilesBorder);

                    this.SliderSpeed.Maximum = 5;
                    this.SliderSpeed.Minimum = 0.5;
                    this.SliderSpeed.Value = Properties.Settings.Default.SmilesSpeed;
                    this.SliderBright.Value = Properties.Settings.Default.SmilesBrightness;
                    this.TextBoxRed.Text = Properties.Settings.Default.SmilesRed.ToString();
                    this.TextBoxGreen.Text = Properties.Settings.Default.SmilesGreen.ToString();
                    this.TextBoxBlue.Text = Properties.Settings.Default.SmilesBlue.ToString();

                    hideAllAlgsSettings();

                    Lightning.runNewAlg(1);
                }
            }
        }
        private void ButtonAnimationsWaterfall_Click(object sender, RoutedEventArgs e) {
            if (Lightning.animations) {
                if (Lightning.ai != 2) {
                    updateSettings();
                    deselectAllButtons();
                    ButtonSelect(this.ButtonAnimationsWaterfall, this.ButtonAnimationsWaterfallImageBrush, this.ButtonAnimationsWaterfallText, this.ButtonAnimationsWaterfallBorder);

                    this.SliderSpeed.Maximum = 20;
                    this.SliderSpeed.Minimum = 10;
                    this.SliderSpeed.Value = Properties.Settings.Default.WaterfallSpeed;
                    this.SliderBright.Value = Properties.Settings.Default.WaterfallBrightness;
                    this.TextBoxRed.Text = Properties.Settings.Default.WaterfallRed.ToString();
                    this.TextBoxGreen.Text = Properties.Settings.Default.WaterfallGreen.ToString();
                    this.TextBoxBlue.Text = Properties.Settings.Default.WaterfallBlue.ToString();

                    hideAllAlgsSettings();
                    this.GroupBoxAnimationWaterfallSettings.Visibility = Visibility.Visible;
                    this.CheckBoxWaterfallTail.IsChecked = Properties.Settings.Default.WaterfallTail;
                    this.CheckBoxWaterfallInvert.IsChecked = Properties.Settings.Default.WaterfallReverse;

                    Lightning.runNewAlg(2);
                }
            }
        }
        private void ButtonAnimationsNyanCat_Click(object sender, RoutedEventArgs e) {
            if (Lightning.animations) {
                if (Lightning.ai != 3) {
                    updateSettings();
                    deselectAllButtons();
                    ButtonSelect(this.ButtonAnimationsNyanCat, this.ButtonAnimationsNyanCatImageBrush, this.ButtonAnimationsNyanCatText, this.ButtonAnimationsNyanCatBorder);

                    this.SliderSpeed.Maximum = 5;
                    this.SliderSpeed.Minimum = 0.5;
                    this.SliderSpeed.Value = Properties.Settings.Default.NyanSpeed;
                    this.SliderBright.Value = Properties.Settings.Default.NyanBrightness;

                    hideAllAlgsSettings();

                    Lightning.runNewAlg(3);
                }
            }
        }

        private void deselectAllButtons() {
            if (Lightning.effects) {
                if (Lightning.ei == 0) {
                    ButtonDeselect(this.ButtonEffectsEqualizer, this.ButtonEffectsEqualizerImageBrush, this.ButtonEffectsEqualizerText, this.ButtonEffectsEqualizerBorder);
                } else if (Lightning.ei == 1) {
                    ButtonDeselect(this.ButtonEffectsVolume, this.ButtonEffectsVolumeImageBrush, this.ButtonEffectsVolumeText, this.ButtonEffectsVolumeBorder);
                } else if (Lightning.ei == 2) {
                    ButtonDeselect(this.ButtonEffectsRandom, this.ButtonEffectsRandomImageBrush, this.ButtonEffectsRandomText, this.ButtonEffectsRandomBorder);
                }
            } else if (Lightning.animations) {
                if (Lightning.ai == 0) {
                    ButtonDeselect(this.ButtonAnimationsTimer, this.ButtonAnimationsTimerImageBrush, this.ButtonAnimationsTimerText, this.ButtonAnimationsTimerBorder);
                } else if (Lightning.ai == 1) {
                    ButtonDeselect(this.ButtonAnimationsSmiles, this.ButtonAnimationsSmilesImageBrush, this.ButtonAnimationsSmilesText, this.ButtonAnimationsSmilesBorder);
                } else if (Lightning.ai == 2) {
                    ButtonDeselect(this.ButtonAnimationsWaterfall, this.ButtonAnimationsWaterfallImageBrush, this.ButtonAnimationsWaterfallText, this.ButtonAnimationsWaterfallBorder);
                } else if (Lightning.ai == 3) {
                    ButtonDeselect(this.ButtonAnimationsNyanCat, this.ButtonAnimationsNyanCatImageBrush, this.ButtonAnimationsNyanCatText, this.ButtonAnimationsNyanCatBorder);
                }
            }
        }
        private void hideAllModsOptions() {
            elementsHide(new UIElement[] { this.GroupBoxDelaySetting, this.GroupBoxSpeedSetting, this.ScrollEffects, this.ScrollAnimations });
        }
        private void showAllModsOptions() {
            if (Lightning.effects) {
                elementsUnhide(new UIElement[] { this.ScrollEffects, this.GroupBoxDelaySetting });
            } else if (Lightning.animations) {
                elementsUnhide(new UIElement[] { this.ScrollAnimations, this.GroupBoxSpeedSetting });
            }
        }
        private void hideAllAlgsSettings() {
            elementsHide(new UIElement[] { this.GroupBoxEffectEqualizerSettings, this.GroupBoxEffectVolumeSettings, this.GroupBoxEffectRandomSettings, this.GroupBoxAnimationTimerSettings, this.GroupBoxAnimationWaterfallSettings });
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

                    if (Lightning.red != r) {
                        this.TextBoxRed.Text = r.ToString();
                        Lightning.changeColorRed((byte)r);
                    }

                    if (Lightning.green != g) {
                        this.TextBoxGreen.Text = g.ToString();
                        Lightning.changeColorGreen((byte)g);
                    }

                    if (Lightning.blue != b) {
                        this.TextBoxBlue.Text = b.ToString();
                        Lightning.changeColorBlue((byte)b);
                    }

                    updateColor();
                }
            }
        }


        private void CheckBoxEffectsEqualizerStaticVolumeTurn_Checked(object sender, RoutedEventArgs e) {
            Equalizer.turnVolume();
            this.GroupBoxEffectsEqualizerColumnsSettings.Visibility = Visibility.Hidden;
            this.GroupBoxEffectsEqualizerStaticVolumeSettings.Visibility = Visibility.Visible;
        }
        private void CheckBoxEffectsEqualizerStaticVolumeTurn_UnChecked(object sender, RoutedEventArgs e) {
            Equalizer.turnVolume();
            this.GroupBoxEffectsEqualizerColumnsSettings.Visibility = Visibility.Visible;
            this.GroupBoxEffectsEqualizerStaticVolumeSettings.Visibility = Visibility.Hidden;
        }
        private void CheckBoxEffectsEqualizerAntialiasingTurn_Checked(object sender, RoutedEventArgs e) {
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

        private void CheckBoxVolumeSimple_Checked(object sender, RoutedEventArgs e) {
            Volume.simpleVolume();
        }

        private void CheckBoxRandomTurn_Checked(object sender, RoutedEventArgs e) {
            Effects.Random.LEDturn();
        }

        private void CheckBoxTimerSimple_Checked(object sender, RoutedEventArgs e) {
            Animations.Timer.simpleNumbers();
        }
        private void CheckBoxTimerInvert_Checked(object sender, RoutedEventArgs e) {
            Animations.Timer.invertColor();
        }

        private void CheckBoxWaterfallTail_Checked(object sender, RoutedEventArgs e) {
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
        private void CheckBoxWaterfallInvert_Checked(object sender, RoutedEventArgs e) {
            Waterfall.turnInvert();
        }
        private void CheckBoxWaterfallHorizontal_Checked(object sender, RoutedEventArgs e) {
            Waterfall.turnHorizontal();
        }
        private void CheckBoxWaterfallDiagonal_Checked(object sender, RoutedEventArgs e) {
            Waterfall.turnDiagonal();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e) {
            ModernSettings WindowSettings = new ModernSettings();
            //WindowSettings.comboBoxDevices.SelectionChanged += new SelectionChangedEventHandler(change);
            WindowSettings.Left = this.Left + this.Width / 2 - WindowSettings.Width / 2;
            WindowSettings.Top = this.Top + this.Height / 2 - WindowSettings.Height / 2;
            WindowSettings.Show();
        }

        void updateSettings() {
            if (initialized && !Properties.Settings.Default.reset) {
                Properties.Settings.Default.DeviceID = Volume.getListenDevice().ID;
                Properties.Settings.Default.mode = (byte)(
                    (Lightning.effects ? 0 :
                    (Lightning.animations ? 1 :
                    (Lightning.custom ? 2 :
                    (Lightning.games ? 3 : 0
                    )))));
                Properties.Settings.Default.ei = Lightning.ei;
                Properties.Settings.Default.ai = Lightning.ai;
                Properties.Settings.Default.ci = Lightning.ci;
                Properties.Settings.Default.gi = Lightning.gi;

                if (Lightning.animations) {
                    switch (Lightning.ai) {
                        case 1:
                            Properties.Settings.Default.SmilesSpeed = Convert.ToSingle(this.SliderSpeed.Value);
                            Properties.Settings.Default.SmilesBrightness = Convert.ToSingle(this.SliderBright.Value);
                            Properties.Settings.Default.SmilesRed = Lightning.red;
                            Properties.Settings.Default.SmilesGreen = Lightning.green;
                            Properties.Settings.Default.SmilesBlue = Lightning.blue;
                            break;
                        case 2:
                            Properties.Settings.Default.WaterfallSpeed = Convert.ToSingle(this.SliderSpeed.Value);
                            Properties.Settings.Default.WaterfallBrightness = Convert.ToSingle(this.SliderBright.Value);
                            Properties.Settings.Default.WaterfallRed = Lightning.red;
                            Properties.Settings.Default.WaterfallGreen = Lightning.green;
                            Properties.Settings.Default.WaterfallBlue = Lightning.blue;
                            Properties.Settings.Default.WaterfallTail = this.CheckBoxWaterfallTail.IsChecked.Value;
                            Properties.Settings.Default.WaterfallReverse = this.CheckBoxWaterfallInvert.IsChecked.Value;
                            break;
                        case 3:
                            Properties.Settings.Default.NyanSpeed = Convert.ToSingle(this.SliderSpeed.Value);
                            Properties.Settings.Default.NyanBrightness = Convert.ToSingle(this.SliderBright.Value);
                            break;
                        default:
                            Properties.Settings.Default.TimerSpeed = Convert.ToSingle(this.SliderSpeed.Value);
                            Properties.Settings.Default.TimerBrightness = Convert.ToSingle(this.SliderBright.Value);
                            Properties.Settings.Default.TimerRed = Lightning.red;
                            Properties.Settings.Default.TimerGreen = Lightning.green;
                            Properties.Settings.Default.TimerBlue = Lightning.blue;
                            Properties.Settings.Default.TimerSimpleNumbers = this.CheckBoxTimerSimple.IsChecked.Value;
                            Properties.Settings.Default.TimerInvertColors = this.CheckBoxTimerInvert.IsChecked.Value;
                            break;
                    }
                } else {
                    switch (Lightning.ei) {
                        case 1:
                            Properties.Settings.Default.VolumeDelay = Convert.ToInt32(this.SliderDelay.Value);
                            Properties.Settings.Default.VolumeBrightness = Convert.ToSingle(this.SliderBright.Value);
                            Properties.Settings.Default.VolumeRed = Lightning.red;
                            Properties.Settings.Default.VolumeGreen = Lightning.green;
                            Properties.Settings.Default.VolumeBlue = Lightning.blue;
                            Properties.Settings.Default.VolumeSimpleVolume = this.CheckBoxVolumeSimple.IsChecked.Value;
                            break;
                        case 2:
                            Properties.Settings.Default.RandomDelay = Convert.ToInt32(this.SliderDelay.Value);
                            Properties.Settings.Default.RandomBrightness = Convert.ToSingle(this.SliderBright.Value);
                            Properties.Settings.Default.RandomRed = Lightning.red;
                            Properties.Settings.Default.RandomGreen = Lightning.green;
                            Properties.Settings.Default.RandomBlue = Lightning.blue;
                            Properties.Settings.Default.RandomOnOffLeds = this.CheckBoxRandomTurn.IsChecked.Value;
                            break;
                        default:
                            Properties.Settings.Default.EqualizerDelay = Convert.ToInt32(this.SliderDelay.Value);
                            Properties.Settings.Default.EqualizerBrightness = Convert.ToSingle(this.SliderBright.Value);
                            Properties.Settings.Default.EqualizerRed = Lightning.red;
                            Properties.Settings.Default.EqualizerGreen = Lightning.green;
                            Properties.Settings.Default.EqualizerBlue = Lightning.blue;
                            Properties.Settings.Default.EqualizerFrequenciesMin = Equalizer.minHz;
                            Properties.Settings.Default.EqualizerFrequenciesMax = Equalizer.maxHz;
                            Properties.Settings.Default.EqualizerAntialiasing = this.CheckBoxEffectsEqualizerAntialiasingTurn.IsChecked.Value;
                            Properties.Settings.Default.EqualizerAverageIntensityMin = Equalizer.startColumn;
                            Properties.Settings.Default.EqualizerAverageIntensityMax = Equalizer.endColumn;
                            Properties.Settings.Default.EqualizerStaticVolumeBorder = this.CheckBoxEffectsEqualizerStaticVolumeTurn.IsChecked.Value;
                            Properties.Settings.Default.EqualizerMaxVolume = Equalizer.staticVolumeSize;
                            break;
                    }
                }

                Properties.Settings.Default.Save();
            }
        }
    }
}

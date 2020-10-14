using System.Threading;
using RyosMKFXPanel.Effects;
using RyosMKFXPanel.Animations;
using System.Windows;
using System.Windows.Media;

namespace RyosMKFXPanel {
    class RyosMKFXPanel {
        public static bool effects = true;
        public static bool animations = false;
        public static bool custom = false;
        public static bool games = false;

        /// <summary>
        /// Turn off all effects from selected mode
        /// </summary>
        public static void offLightAlgs() {
            if (effects) {
                if (Equalizer.getState())
                    Equalizer.stop();
                else if (Volume.getState())
                    Volume.stop();
                else if (Effects.Random.getState())
                    Effects.Random.stop();

            } else if (animations) {
                if (Animations.Timer.getState())
                    Animations.Timer.stop();
                else if (Smiles.getState())
                    Smiles.stop();
                else if (Waterfall.getState())
                    Waterfall.stop();
                else if (NyanCat.getState())
                    NyanCat.stop();

            }
            Thread.Sleep(100);
        }

        /// <summary>
        /// Turn on specified effect from selected mode
        /// </summary>
        /// <param name="ai">index of effect (0 - Equalizer/Timer, 1 - Volume/Smiles, 2 - Random/Waterfall, 3 - null/NyanCat)</param>
        public static void onLightAlg(int ai) {
            if (effects) {
                if (ai == 0)
                    Equalizer.start();
                else if (ai == 1)
                    Volume.start();
                else if (ai == 2)
                    Effects.Random.start();

            } else if (animations) {
                if (ai == 0)
                    Animations.Timer.start();
                else if (ai == 1)
                    Smiles.start();
                else if (ai == 2)
                    Waterfall.start();
                else if (ai == 3)
                    NyanCat.start();

            }
        }




        /// <summary>
        /// Change status of only one mode to true
        /// </summary>
        /// <param name="mi">index of mode (0 - Effects, 1 - Animations, 2 - Custom, 3 - Games)</param>
        public static void changeMode(int mi) {
            effects = false;
            animations = false;
            custom = false;
            games = false;
            if (mi == 0)
                effects = true;
            else if (mi == 1)
                animations = true;
            else if (mi == 2)
                custom = true;
            else if (mi == 3)
                games = true;
        }

        /// <summary>
        /// Turn off actual effects and turn on new one
        /// </summary>
        /// <param name="mi">index of mode (0 - Effects, 1 - Animations, 2 - Custom, 3 - Games)</param>
        /// <param name="ai">index of effect (0 - Equalizer/Timer, 1 - Volume/Smiles, 2 - Random/Waterfall, 3 - null/NyanCat)</param>
        public static void runNewAlg(int mi, int ai) {
            offLightAlgs();
            changeMode(mi);
            onLightAlg(ai);
        }




        /// <summary>
        /// Initiate connection to device
        /// </summary>
        /// <returns>False if was started or can not connect too device, true if connected</returns>
        public static bool startWork() {
            if (!Lightning.getStatus()) {
                if (Lightning.connect()) {
                    if (Lightning.changeStatus()) {
                        return true;
                    }
                }
                else {
                    MessageBox.Show("Can't start a work\n with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        /// <summary>
        /// Release connection to device
        /// </summary>
        /// <returns>False if wasn't started or can not release device, true if disconnected</returns>
        public static bool stopWork() {
            if (Lightning.getStatus()) {
                if (Lightning.disconnect()) {
                    if (Lightning.changeStatus()) {
                        offLightAlgs();
                        Thread.Sleep(100);
                        return true;
                    }
                } else {
                    MessageBox.Show("Can't stop a work\n with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        /// <summary>
        /// Switch combination of 
        /// </summary>
        /// <returns>False if wasn't started or can not release device, true if disconnected</returns>
        public static bool switchWork() {
            if (!Lightning.getStatus()) {
                if (Lightning.connect()) {
                    if (Lightning.changeStatus()) {
                        return true;
                    }
                } else {
                    MessageBox.Show("Can't start a work\n with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else {
                if (Lightning.disconnect()) {
                    if (Lightning.changeStatus()) {
                        return false;
                    }
                } else {
                    MessageBox.Show("Can't stop a work\n with the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }



        /// <summary>
        /// Change delay (delay applies before send new packet to keyboard)
        /// </summary>
        /// <param name="delay">Delay in miliseconds</param>
        public static void changeDelay(int delay) {
            Lightning.delay = delay;
        }

        /// <summary>
        /// Change speed (division of 1 second)
        /// </summary>
        /// <param name="speed">Speed multiplier</param>
        public static void changeSpeed(float speed) {
            Lightning.speed = speed;
        }

        /// <summary>
        /// Change brightness of keys
        /// </summary>
        /// <param name="brightness">Brightness multiplier to color</param>
        public static void changeBrightness(float brightness) {
            Lightning.maxBright = brightness;
        }

        /// <summary>
        /// Change red color intensity
        /// </summary>
        /// <param name="red">1 byte number</param>
        public static void changeColorRed(byte red) {
            Lightning.red = red;
        }

        /// <summary>
        /// Change green color intensity
        /// </summary>
        /// <param name="green">1 byte number</param>
        public static void changeColorGreen(byte green) {
            Lightning.green = green;
        }

        /// <summary>
        /// Change blue color intensity
        /// </summary>
        /// <param name="blue">1 byte number</param>
        public static void changeColorBlue(byte blue) {
            Lightning.blue = blue;
        }

        /// <summary>
        /// Applies RGB code
        /// </summary>
        /// <param name="rgb">string of rgb code (0, 0, 255)/(#0000FF)</param>
        public static void changeColorRGB(string rgb) {
            Color color = (Color)ColorConverter.ConvertFromString(rgb);
            Lightning.red = color.R;
            Lightning.green = color.G;
            Lightning.blue = color.B;
        }
    }
}

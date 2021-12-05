using System.Windows;

namespace RyosMKFXPanel.Animations {
    class Timer :LightningModule {
        public override string GetUid() {
            return "DefaultTimer";
        }
        public override string GetName() {
            return "Timer";
        }
        public override string GetCategory() {
            return "Animations";
        }
        public override string GetIcon() {
            return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAGdAAABnQEox4btAAACBUlEQVRYhb1XMXaDMAz95XWv107hBuUG8RHYunIEjsAROAJrp3IEjkBuQKaumbqqg7+DcAkxSYje0zO2JX1ZyLYMEUEkVxJPVazdFxFBJBkAWTBWsy2D8R7AKcboGgfmqGNrbzXwulI+hYuCj4RvK7Y9eYg1GBMBA6Agf6jxA8Ywm5m5hrz8K1RCDCJSqr5hMp2YWB3ns4WkyijTUedEG0bJlMSCiEwc6KnUiIilkO+nK3aL55S6QltW9fs5B0CBXyW0tNpYztTifolxng+FCwq2QdjuZUObQoxZB3IV8kcBz0VYiDVxwIhLmP7BK5+LRE8sox3wnj3in8fkxDnSEJetW4f+0q9IIW5fity21W5lv+gyAZDz5BquH5wT+gQgbNfSQMw8AbAH0N5g5D1o11ILYJ8oj55NAzDehvc4sFff9UWpKw7cQzkZAH4AfK1R9g5YjMVFLH0H/bXgFnD1gC8g8iXpDagFkCZwK7dwRcWzyBCzS+jJG54bgZyYjT+ZOnFFw5YXkb6QBmKeLyPLo7F+ggM1sax2QE8UG4IX4UJDAV86beGEB+/1+Nz/8U5UDwSvFPgkzy4lSaMU7B3AVqbV9j+ZJeVcxtK8YwhjdomhbEfdQVQNGHLMy6iAe3rt2D/AHV7hi8cfLv6FdKRes2R8zeM0gztA/NtwF8wfMb4NW7ZX6Q+KkvrI/qV3qgAAAABJRU5ErkJggg==";
        }
        public override SliderType GetSliderType() {
            return SliderType.speed;
        }
        public override double GetSliderMax() {
            return 5.0d;
        }
        public override double GetSliderMin() {
            return 0.5d;
        }
        public override double GetSliderTick() {
            return 0.5d;
        }
        public override double GetSliderTickFrequency() {
            return 0.5d;
        }
        public override LightningModuleControls[] GetControls() {
            return new LightningModuleControls[] {
                new CCheckBox("Simple numbers", new RoutedEventHandler(OnSimple), new RoutedEventHandler(OffSimple)),
                new CCheckBox("Invert colors", new RoutedEventHandler(OnInvert), new RoutedEventHandler(OffInvert))
            };
        }


        public bool invert = false;
        public bool simple = false;

       // public override Param[] GetParameters() {
            //return new Param[] {
                //new Param(invert.GetType(), nameof(invert), "Invert"),
                //new Param(simple.GetType(), nameof(simple), "Simple")
            //};
        //}

        public Timer() { }



        public override void Work() {
            int s = 0;
            int ss = 0;
            while (true) {
                Lightning.devices[0].KeysColorUpdate();
                Lightning.devices[0].KeysLightAllOff();
                if (simple) {
                    switch (s) {
                        case 0:
                            break;
                        case 1:
                            Lightning.devices[0].KeyLightOn(92);
                            break;
                        case 2:
                            Lightning.devices[0].KeyLightOn(93);
                            break;
                        case 3:
                            Lightning.devices[0].KeyLightOn(94);
                            break;
                        case 4:
                            Lightning.devices[0].KeyLightOn(74);
                            break;
                        case 5:
                            Lightning.devices[0].KeyLightOn(75);
                            break;
                        case 6:
                            Lightning.devices[0].KeyLightOn(76);
                            break;
                        case 7:
                            Lightning.devices[0].KeyLightOn(56);
                            break;
                        case 8:
                            Lightning.devices[0].KeyLightOn(57);
                            break;
                        case 9:
                            Lightning.devices[0].KeyLightOn(58);
                            break;
                    }
                } else {
                    switch (s) {
                        case 0:
                            break;
                        case 1:
                            Lightning.devices[0].KeysLightOn(new int[] { 74, 57, 36, 58, 76, 94 });
                            break;
                        case 2:
                            Lightning.devices[0].KeysLightOn(new int[] { 34, 35, 36, 58, 76, 75, 74, 92, 108, 109 });
                            break;
                        case 3:
                            Lightning.devices[0].KeysLightOn(new int[] { 34, 35, 36, 58, 76, 75, 74, 94, 109, 108 });
                            break;
                        case 4:
                            Lightning.devices[0].KeysLightOn(new int[] { 34, 56, 75, 75, 75, 36, 58, 76, 94, 109 });
                            break;
                        case 5:
                            Lightning.devices[0].KeysLightOn(new int[] { 36, 35, 34, 56, 74, 75, 76, 94, 109, 108 });
                            break;
                        case 6:
                            Lightning.devices[0].KeysLightOn(new int[] { 36, 35, 34, 56, 74, 92, 108, 109, 94, 76, 75 });
                            break;
                        case 7:
                            Lightning.devices[0].KeysLightOn(new int[] { 34, 35, 36, 58, 76, 94, 109 });
                            break;
                        case 8:
                            Lightning.devices[0].KeysLightOn(new int[] { 58, 36, 35, 34, 56, 74, 75, 76, 94, 109, 108, 92 });
                            break;
                        case 9:
                            Lightning.devices[0].KeysLightOn(new int[] { 58, 36, 35, 34, 56, 74, 75, 76, 94, 109, 108 });
                            break;
                    }
                }
                switch (ss) {
                    case 0:
                        break;
                    case 1:
                        Lightning.devices[0].KeyLightOn(18);
                        break;
                    case 2:
                        Lightning.devices[0].KeyLightOn(19);
                        break;
                    case 3:
                        Lightning.devices[0].KeyLightOn(20);
                        break;
                    case 4:
                        Lightning.devices[0].KeyLightOn(21);
                        break;
                    case 5:
                        Lightning.devices[0].KeyLightOn(22);
                        break;
                    case 6:
                        Lightning.devices[0].KeyLightOn(23);
                        break;
                    case 7:
                        Lightning.devices[0].KeyLightOn(24);
                        break;
                    case 8:
                        Lightning.devices[0].KeyLightOn(25);
                        break;
                    case 9:
                        Lightning.devices[0].KeyLightOn(26);
                        break;
                }
                if (invert) {
                    if (simple) {
                        Lightning.devices[0].KeysColorInvertIfOff(new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26, 56, 57, 58, 74, 75, 76, 92, 93, 94 });
                    } else {
                        Lightning.devices[0].KeysColorInvertIfOff(new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26, 34, 35, 36, 56, 57, 58, 74, 75, 76, 92, 93, 94, 108, 109 });
                    }
                }
                s++;
                if (s >= 9) {
                    s = 0;
                    ss++;
                }
                if (ss >= 9) {
                    ss = 0;
                }
                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();
            }
        }


        private void OnSimple(object sender, RoutedEventArgs e) {
            simple = true;
        }
        private void OffSimple(object sender, RoutedEventArgs e) {
            simple = false;
        }
        private void OnInvert(object sender, RoutedEventArgs e) {
            invert = true;
        }
        private void OffInvert(object sender, RoutedEventArgs e) {
            invert = false;
        }


    }
}
/*  xx  :  00  xx  01  02  03  04 :  05  06  07  08 :  09  10  11  12  :  13  14  15  :  xx xx  xx xx
    ....:.........................:.................:..................:..............:................
    16  :  17  18  19  20  21  22  23  24  25  26  27  28  29  3____0  :  31  32  33  :  34  35  36  37
    38  :  3___9 40  41  42  43  44  45  46  47  48  49  50  51 5___2  :  53  54  55  :  56  57  58  5x
    60  :  6____1 62  63  64  65  66  67  68  69  70  71  72  7_____3  :  nm  sh  sc  :  74  75  76  x9
    77  :  7_____8  80  81  82  83  84  85  86  87  88  89  9_______0  :  xx  91  xx  :  92  93  94  9y
    96  :  9___7 98  99  1______________________00  101  102 103 1_04  :  105 106 107 :  1___08 109  y5
*/
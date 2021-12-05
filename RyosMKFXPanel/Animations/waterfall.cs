using System.Windows;
using System.Windows.Controls;

namespace RyosMKFXPanel.Animations {
    class Waterfall :LightningModule {
        public override string GetUid() {
            return "DefaultWaterfall";
        }
        public override string GetName() {
            return "Waterfall";
        }
        public override string GetCategory() {
            return "Animations";
        }
        public override string GetIcon() {
            return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAC63pUWHRSYXcgcHJvZmlsZSB0eXBlIGV4aWYAAHja7ZdtkuQmDIb/c4ocAUkIieNgMFW5QY6fF0x7umcmyW7t/klVm+LDQpZkPTI9E86//hzhD1xUMoek5rnkHHGlkgpXLDxeV1kjxbTGdfHewv2LPNwbDJFgluvW6tavkOvHAw8fdLzKg+8d9m1obzwMyvQ8vfXnICHnS05pGyrntcjF7TnUY4fatuIKZfeDbmOXLu7DsyAZstQVjoT5FJKIkWU/JFev6L7GBD2CrKJ7wCQSdyRIyMvrPeYYnxP0kuTHKnzO/r36lHyuWy6fcpl3jrD4doP0++SvFD85ljsift0weZj6muQxuo9xXm9XU0ZG866olWx6mIHigZTLeiyjGbpibasVNI81NiDvscUDrVEhBpURKFGnSoPONTdqCDHxyYaZubEsmYtx4SaTU5qNBpsU6SDI0vgMYJaE71ho+S3LXyOH505QZYKxifofW/i3zZ9pYYw2U0TR71whLp4lijAmuTlCC0BobG66EvxoG398qh+UKgjqSrPjBWs8LhOH0kdtyeIs0FPM1ydEwfo2gBTBtyIYEhCImUQpUzRmI0IeHYAqImd8GwcIkCp3BMlJBOeRsfP0jWeMli4rZ55inE0AoZLFwKZIBayUFPVjyVFDVUWTqmY19aBFa5acsuacLc9DrppYMrVsZm7FqosnV89u7l68Fi6CM1BLLla8lFIrhwpHFbYq9CskBx9ypEOPfNjhRzlqQ/m01LTlZs1babVzl45joudu3Xvp9aRw4qQ406lnPu30s5x1oNaGjDR05GHDRxn1prapfmk/QY02NV6kpp7d1CANZg8TNI8TncxAjBOBuE0CKGiezKJTSjzJTWaxMD4KZQSpk03oNIkBYTqJddDN7oPcD3EL6j/Ejf+LXJjofge5AHRfuX1Drc/fubaIXV/hzGkUfH3QqewBPUYMvzq/Db0NvQ29Db0NvQ29Df3/DcnAHw/4Jzb8DVVjnZBJ7blFAAABhGlDQ1BJQ0MgcHJvZmlsZQAAeJx9kT1Iw0AcxV8/pKVUHOyg4pChOlmQKuIoVSyChdJWaNXB5NIvaNKQpLg4Cq4FBz8Wqw4uzro6uAqC4AeIk6OToouU+L+k0CLGg+N+vLv3uHsHeFs1phj+SUBRTT2TTAj5wqoQeEUQfoQwjLjIDC2VXczBdXzdw8PXuxjPcj/35+iXiwYDPALxHNN0k3iDeGbT1DjvE0dYRZSJz4kndLog8SPXJYffOJdt9vLMiJ7LzBNHiIVyD0s9zCq6QjxNHJUVlfK9eYdlzluclVqDde7JXxguqitZrtMcRRJLSCENARIaqKIGEzFaVVIMZGg/4eIfsf1pcknkqoKRYwF1KBBtP/gf/O7WKE3FnaRwAuh7sayPMSCwC7SblvV9bFntE8D3DFypXX+9Bcx+kt7satEjYGAbuLjuatIecLkDDD1poi7ako+mt1QC3s/omwrA4C0QWnN66+zj9AHIUVfLN8DBITBepux1l3cHe3v790ynvx98P3Kr1f0CigAAAAZiS0dEAP8A/wD/oL2nkwAAAAlwSFlzAAABYgAAAWIBXyfQUwAAAAd0SU1FB+QIBAocH5j7+twAAAJJSURBVFjDxZcxj9NAEIW/OZ0QZX5CKKhocjRQITdIdBdRIDr8EywqOlxSofwEX3mdRYVEQUR1FXJDh0RaOktQIJpHkXG02bMT+84JI1mKdnZm3755O7sxSSUwYW0PgKdmVnEAkzQDPgHffKg2H2wAvAaeAMnYIHydJfAFeL8B0DKxAF4AF8DPgevcBf60jN8HngOXZpaGDutA+xX4C3wcCOAlsAKuovFnwB0zexgHnHYk+gBgZvlAmhPgKo6T1Blz0iNpLmnOgeykx5wEmPXMV/nX205voOa5l6eMfWaWHYKB2GYDGBmFgRSod2kEqNoYGYUBM1uZWT2SRkYpwcK/XTqZeosfX4R72GhsCpyPyoC36C6NLI5Rglc31MjoGtjbMc1sCZwdFUB8Gvpe50MA3It2XEqaHuMu2NQ6Gjp3tR+8E/a1TceUNG0BfFgNRKeh8LfB7QFImni/j+3M1X47DUia7Gqpruy3LTuu9rwNe4swk1REip54TX8Ab+L3gKRKaysiX6r1++tRC+jHklYel249l91R++9E0i9Jn725fPekzdzaAWf+u/TLJ/XkWTBvERxZedziGnivc+VBklRFN9vvwL+MwNdBXBEx1YxL0ruIqVpSFoNI29TrIPIt6rZ9SUfcxBmZdvgS/rvtaqf+d2pwvl07u7aeU7sMF3NKG8XGvjSoaxUeY8/V5VsEvk4BzoMFCgeybJK56Brf3EFWPt7EpT63DnJk7suDeeUGoAcUAcIyArgKGClawDeWtxxvNZuLTshKUv4PmnW8ErtYaUwAAAAASUVORK5CYII=";
        }
        public override SliderType GetSliderType() {
            return SliderType.speed;
        }
        public override double GetSliderMax() {
            return 100.0d;
        }
        public override double GetSliderMin() {
            return 1.0d;
        }
        public override double GetSliderTick() {
            return 5.0d;
        }
        public override double GetSliderTickFrequency() {
            return 5.0d;
        }
        public override LightningModuleControls[] GetControls() {
            return new LightningModuleControls[] {
                new CSpoiler(
                        new CCheckBox("Tail", new RoutedEventHandler(OnTail), new RoutedEventHandler(OffTail), true), null,
                        new CInputBox("Tail length", new TextChangedEventHandler(ChangeTailLength), 2, "5")
                    ),
                new CCheckBox("Reverse", new RoutedEventHandler(OnInvert), new RoutedEventHandler(OffInvert), false)
            };
        }


        public bool tail = false;
        public float tailLength = 5;
        public bool invert = false;
        public bool horizontal = false;
        public bool diagonal = false;

        //public override Param[] GetParameters() {
        //    return new Param[] {
        //        new Param(tail.GetType(), nameof(tail), "Tail"),
        //        new Param(tailLength.GetType(), nameof(tailLength), "TailLength"),
        //        new Param(invert.GetType(), nameof(invert), "Invert"),
        //        new Param(horizontal.GetType(), nameof(horizontal), "HorizontalMode"),
        //        new Param(diagonal.GetType(), nameof(diagonal), "DiagonalMode")
        //    };
        //}
        public Waterfall() { }

        

        public override void Work() {
            Lightning.devices[0].KeysLightAllOn();
            float[][] Matrix = new float[Lightning.devices[0].GetHeight()][];
            for (int i = 0; i < Lightning.devices[0].GetHeight(); i++) {
                Matrix[i] = new float[Lightning.devices[0].GetWidth()];
            }
            float x = 1f;
            int line = 0;
            while (true) {
                float p = (1 / tailLength);
                for (int i = 0; i < Lightning.devices[0].GetHeight(); i++) {
                    for (int ii = 0; ii < Lightning.devices[0].GetWidth(); ii++) {
                        Matrix[i][ii] = 0;
                    }
                }
                if (diagonal) {
                    if (horizontal) {

                    } else {
                        if (invert) {

                        } else {
                            for (int i = line; i < Lightning.devices[0].GetHeight(); i++) {
                                for (int ii = 0; ii < Lightning.devices[0].GetWidth(); ii++) {
                                    Matrix[i][ii] = x;
                                }
                                if (tail) {
                                    x = (x >= 1f) ? 1 / tailLength : x + 1 / tailLength;
                                    line = 0;
                                } else {
                                    line = (line >= 1) ? line - 1 : Lightning.devices[0].GetHeight() - 1;
                                    i = Lightning.devices[0].GetHeight();
                                }
                            }
                        }
                    }
                } else {
                    if (horizontal) {

                    } else {
                        if (invert) {
                            for (int i = line; i >= 0; i--) {
                                for (int ii = 0; ii < Lightning.devices[0].GetWidth(); ii++) {
                                    Matrix[i][ii] = x;
                                }
                                if (tail) {
                                    x = (x - p < p) ? 1f : x - p;
                                    line = Lightning.devices[0].GetHeight() -1;
                                } else {
                                    line = (line < Lightning.devices[0].GetHeight() -1) ? line + 1 : 0;
                                    i = -1;
                                    x = 1f;
                                }
                            }
                        } else {
                            for (int i = line; i < Lightning.devices[0].GetHeight(); i++) {
                                for (int ii = 0; ii < Lightning.devices[0].GetWidth(); ii++) {
                                    Matrix[i][ii] = x;
                                }
                                if (tail) {
                                    x = (x-p < p) ? 1f : x - p;
                                    line = 0;
                                } else {
                                    line = (line >= 1) ? line - 1 : Lightning.devices[0].GetHeight() - 1;
                                    i = Lightning.devices[0].GetHeight();
                                    x = 1f;
                                }
                            }
                        }
                    }
                }
                Lightning.devices[0].KeysColorChangeByMatrix(Matrix);
                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();
            }
        }



        private void OnInvert(object sender, RoutedEventArgs e) {
            invert = true;
        }
        private void OffInvert(object sender, RoutedEventArgs e) {
            invert = false;
        }
        private void OnTail(object sender, RoutedEventArgs e) {
            tail = true;
        }
        private void OffTail(object sender, RoutedEventArgs e) {
            tail = false;
        }
        private void ChangeTailLength(object sender, TextChangedEventArgs e) {
            tailLength = TextBoxLibrary.InputIntParse((TextBox)sender, 2);
        }


    }
}
/*
    005  :  015  025  035  045  055  065  075  085  095  105  115  125  135  145  155  :  165  175  185  :  195  205  215  225
    .....:..............................:...................:..........................:.................:....................
    004  :  014  024  034  044  054  064  074  084  094  104  114  124  134  144  154  :  164  174  184  :  194  204  214  224
    003  :  013  023  033  043  053  063  073  083  093  103  113  123  133  143  153  :  163  173  183  :  193  203  213  223
    002  :  012  022  032  042  052  062  072  082  092  102  112  122  132  142  152  :  162  172  182  :  192  202  212  222
    001  :  011  021  031  041  051  061  071  081  091  101  111  121  131  141  151  :  161  171  181  :  191  201  211  221
    000  :  010  020  030  040  050  060  070  080  090  100  110  120  130  140  150  :  160  170  180  :  190  200  210  220
*/

namespace RyosMKFXPanel.Animations {
    class NyanCat :LightningModule {
        public override string GetUid() {
            return "DefaultNyanCat";
        }
        public override string GetName() {
            return "NyanCat";
        }
        public override string GetCategory() {
            return "Animations";
        }
        public override string GetIcon() {
            return "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bG5zOkdJTVA9Imh0dHA6Ly93d3cuZ2ltcC5vcmcveG1wLyIKICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgIHhtbG5zOnBob3Rvc2hvcD0iaHR0cDovL25zLmFkb2JlLmNvbS9waG90b3Nob3AvMS4wLyIKICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgeG1wTU06RG9jdW1lbnRJRD0iYWRvYmU6ZG9jaWQ6cGhvdG9zaG9wOmViZjQ1YjNmLTkyZDYtY2E0OC04OTk2LTVmNzQzOWU2ODQyYyIKICAgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDoyYzNkZmYxZi02MWI3LTRiZGMtOTc0ZS1lYjkxZGNhZjMzOGIiCiAgIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDpiYTEyYTQ1YS05ZTZlLWEzNDAtODA5MC02M2NhZDQ5OGE3YzEiCiAgIEdJTVA6QVBJPSIyLjAiCiAgIEdJTVA6UGxhdGZvcm09IldpbmRvd3MiCiAgIEdJTVA6VGltZVN0YW1wPSIxNTk2NTQyOTk1ODE4MDM3IgogICBHSU1QOlZlcnNpb249IjIuMTAuMjAiCiAgIGRjOkZvcm1hdD0iaW1hZ2UvcG5nIgogICBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIgogICBwaG90b3Nob3A6SUNDUHJvZmlsZT0ic1JHQiBJRUM2MTk2Ni0yLjEiCiAgIHhtcDpDcmVhdGVEYXRlPSIyMDIwLTA4LTA0VDE0OjA1OjQyKzAyOjAwIgogICB4bXA6Q3JlYXRvclRvb2w9IkdJTVAgMi4xMCIKICAgeG1wOk1ldGFkYXRhRGF0ZT0iMjAyMC0wOC0wNFQxNDowODoxNSswMjowMCIKICAgeG1wOk1vZGlmeURhdGU9IjIwMjAtMDgtMDRUMTQ6MDg6MTUrMDI6MDAiPgogICA8aXB0Y0V4dDpMb2NhdGlvbkNyZWF0ZWQ+CiAgICA8cmRmOkJhZy8+CiAgIDwvaXB0Y0V4dDpMb2NhdGlvbkNyZWF0ZWQ+CiAgIDxpcHRjRXh0OkxvY2F0aW9uU2hvd24+CiAgICA8cmRmOkJhZy8+CiAgIDwvaXB0Y0V4dDpMb2NhdGlvblNob3duPgogICA8aXB0Y0V4dDpBcnR3b3JrT3JPYmplY3Q+CiAgICA8cmRmOkJhZy8+CiAgIDwvaXB0Y0V4dDpBcnR3b3JrT3JPYmplY3Q+CiAgIDxpcHRjRXh0OlJlZ2lzdHJ5SWQ+CiAgICA8cmRmOkJhZy8+CiAgIDwvaXB0Y0V4dDpSZWdpc3RyeUlkPgogICA8eG1wTU06SGlzdG9yeT4KICAgIDxyZGY6U2VxPgogICAgIDxyZGY6bGkKICAgICAgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIgogICAgICBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOmJhMTJhNDVhLTllNmUtYTM0MC04MDkwLTYzY2FkNDk4YTdjMSIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIxLjEgKFdpbmRvd3MpIgogICAgICBzdEV2dDp3aGVuPSIyMDIwLTA4LTA0VDE0OjA1OjQyKzAyOjAwIi8+CiAgICAgPHJkZjpsaQogICAgICBzdEV2dDphY3Rpb249InNhdmVkIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIgogICAgICBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOmMzNjYwNjU3LTIzYTMtYWE0Yy1hMmI2LWJmMGYyYmQzZjA3NCIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIxLjEgKFdpbmRvd3MpIgogICAgICBzdEV2dDp3aGVuPSIyMDIwLTA4LTA0VDE0OjA3OjQ1KzAyOjAwIi8+CiAgICAgPHJkZjpsaQogICAgICBzdEV2dDphY3Rpb249InNhdmVkIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIgogICAgICBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOmU0YmE2OWI0LTQ2NGYtNTU0OS05ZWUxLWEzOTBlODM3NjE5ZiIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIxLjEgKFdpbmRvd3MpIgogICAgICBzdEV2dDp3aGVuPSIyMDIwLTA4LTA0VDE0OjA4OjE1KzAyOjAwIi8+CiAgICAgPHJkZjpsaQogICAgICBzdEV2dDphY3Rpb249InNhdmVkIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIgogICAgICBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjFiOTkzMDJkLTNmNjQtNDQ2Yy04MDkwLTFhNzQ1YTlkYTI4NCIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iR2ltcCAyLjEwIChXaW5kb3dzKSIKICAgICAgc3RFdnQ6d2hlbj0iMjAyMC0wOC0wNFQxNDowOTo1NSIvPgogICAgPC9yZGY6U2VxPgogICA8L3htcE1NOkhpc3Rvcnk+CiAgIDxwbHVzOkltYWdlU3VwcGxpZXI+CiAgICA8cmRmOlNlcS8+CiAgIDwvcGx1czpJbWFnZVN1cHBsaWVyPgogICA8cGx1czpJbWFnZUNyZWF0b3I+CiAgICA8cmRmOlNlcS8+CiAgIDwvcGx1czpJbWFnZUNyZWF0b3I+CiAgIDxwbHVzOkNvcHlyaWdodE93bmVyPgogICAgPHJkZjpTZXEvPgogICA8L3BsdXM6Q29weXJpZ2h0T3duZXI+CiAgIDxwbHVzOkxpY2Vuc29yPgogICAgPHJkZjpTZXEvPgogICA8L3BsdXM6TGljZW5zb3I+CiAgPC9yZGY6RGVzY3JpcHRpb24+CiA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz7Tj7hAAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAB3RJTUUH5AgEDAk3nnbIgAAAAJNJREFUWMPtVssOwCAIE7P//2V2kYMmG/UVa7RHAoi1AiFcnA4pDaqqmYOI9Bzg5Ys0VGgCah+VbzkDjyuS9GZWtacJ1G8fBr6Y8PxQ7MPAqL5Ax4DMUrcXZ3Z+DaBdsLwpGrePBmbMHgoGYk3FNdMSnaLrNYD+85ad4O//m51nI+rdgFrz8mhg1JvXTlE+DVwchxfx0Y/kStkdzQAAAABJRU5ErkJggg==";
        }
        public override SliderType GetSliderType() {
            return SliderType.speed;
        }
        public override double GetSliderMax() {
            return 20.0d;
        }
        public override double GetSliderMin() {
            return 1.0d;
        }
        public override double GetSliderTick() {
            return 1.0d;
        }
        public override double GetSliderTickFrequency() {
            return 1.0d;
        }
        public override LightningModuleControls[] GetControls() {
            return new LightningModuleControls[] { };
        }


        //public override Param[] GetParameters() {
            //return new Param[] { };
        //}

        public NyanCat() { }



        public override void Work() {
            Lightning.devices[0].KeysLightAllOff();
            Lightning.devices[0].KeysColorReset();
            while (true) {
                int[] keys = {20,21,22,23,24,25,41,42,43,44,45,46,47,63,64,65,66,67,68,69,80,81,82,83,84,85,86};
                for (int i = 0; i < keys.Length; i++) {
                    Lightning.devices[0].keysLight[keys[i]] = 1;
                }
                //frame 1
                //____tail
                Lightning.devices[0].KeysColorChangeR(20, 22, 200);
                Lightning.devices[0].KeysColorChangeG(41, 43, 200);
                Lightning.devices[0].KeysColorChangeB(63, 65, 200);

                //____body
                Lightning.devices[0].KeysColorChange(23, 25, new float[] {219, 112, 147});
                Lightning.devices[0].KeyColorChange(44, new float[] { 219, 112, 147, 255, 20, 147 });
                Lightning.devices[0].KeysColorChange(66, 67, new float[] { 219, 112, 147 });

                //____head
                Lightning.devices[0].KeysColorChange(46, 47, 192);
                Lightning.devices[0].KeysColorChange(68, 69, 192);


                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();
                Lightning.devices[0].KeysColorReset();



                //frame 2
                //____tail
                Lightning.devices[0].KeysColorChangeR(20, 22, 200);
                Lightning.devices[0].KeysColorChangeG(41, 43, 200);
                Lightning.devices[0].KeysColorChangeB(63, 65, 200);

                //____body
                Lightning.devices[0].KeysColorChange(44, 46, new float[] { 219, 112, 147 });
                Lightning.devices[0].KeyColorChange(66, new float[] { 219, 112, 147, 255, 20, 147 });
                Lightning.devices[0].KeysColorChange(83, 84, new float[] { 219, 112, 147 });

                //____head
                Lightning.devices[0].KeysColorChange(68, 69, 192);
                Lightning.devices[0].KeysColorChange(85, 86, 192);


                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();
                Lightning.devices[0].KeysColorReset();



                //frame3
                //____tail
                Lightning.devices[0].KeysColorChangeR(41, 43, 200);
                Lightning.devices[0].KeysColorChangeG(63, 65, 200);
                Lightning.devices[0].KeysColorChangeB(80, 82, 200);

                //____body
                Lightning.devices[0].KeysColorChange(44, 46, new float[] { 219, 112, 147 });
                Lightning.devices[0].KeyColorChange(66, new float[] { 219, 112, 147, 255, 20, 147 });
                Lightning.devices[0].KeysColorChange(83, 84, new float[] { 219, 112, 147 });

                //____head
                Lightning.devices[0].KeysColorChange(68, 69, 192);
                Lightning.devices[0].KeysColorChange(85, 86, 192);


                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();
                Lightning.devices[0].KeysColorReset();



                //frame 4
                //____tail
                Lightning.devices[0].KeysColorChangeR(41, 43, 200);
                Lightning.devices[0].KeysColorChangeG(63, 65, 200);
                Lightning.devices[0].KeysColorChangeB(80, 82, 200);

                //____body
                Lightning.devices[0].KeysColorChange(23, 25, new float[] { 219, 112, 147 });
                Lightning.devices[0].KeyColorChange(44, new float[] { 219, 112, 147, 255, 20, 147 });
                Lightning.devices[0].KeysColorChange(66, 67, new float[] { 219, 112, 147 });

                //____head
                Lightning.devices[0].KeysColorChange(46, 47, 192);
                Lightning.devices[0].KeysColorChange(68, 69, 192);


                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();
                Lightning.devices[0].KeysColorReset();
            }
        }
    }
}

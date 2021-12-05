namespace RyosMKFXPanel.Animations {
    class Smiles : LightningModule {
        public override string GetUid() {
            return "DefaultSmiles";
        }
        public override string GetName() {
            return "Smiles";
        }
        public override string GetCategory() {
            return "Animations";
        }
        public override string GetIcon() {
            return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAFiAAABYgFfJ9BTAAABk0lEQVRYhe2XUXXDMAxFb3MKYAwWBguDGUIYLGMwCINQBiuEQEgZZAxWBmHw9hG7c924cdP65GfvHP0ksvQsy7K0kUQiKqAGDPAa0TkAHdACfYrRTQKBBvgAXoCjNd4BQ6BXeiSfgW9gB+yvWpcUk1JSpxF7SdUV3VAqu0bWRhnTjRmoJQ2S+hsdh2KsjcHaTCLQeLte6jgUF41mjkCdwXlI4iwS4ZkPmZz7JAZ5OeHfgg54spmcEz3jDTL+LXDnbjLu3k/MUz64j33m0E8dRe8IVJbRPdftVjn5LBgr15G/0jkAikhY/aaQsr63PuutTYbWM2AYkzFmfA6p61vAYEMxWaUySyNJxQ07ezR+AIoZpez4J+AIlCv4Lh2BA/nr/xQq4FAwPkL1CgRqoFu9FK/+GG1tOHbAF2MH2wWhaomX1jkMXB6vAd6AdyCpIanuJBDOB5MNCVqpJQsVVm1Kz16pB5NwzpvwX2yBP5iYOxwbLRhM/JzovGhkGc2WDqc99j33UHI5nH5y3m1dIIWAQ5bx/Bfug4mq9ETHlQAAAABJRU5ErkJggg==";
        }
        public override SliderType GetSliderType() {
            return SliderType.speed;
        }
        public override double GetSliderMax() {
            return 10.0d;
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
            return new LightningModuleControls[] { };
        }


        //public override Param[] GetParameters() {
            //return new Param[] { };
        //}

        public Smiles() { }



        public override void Work() {
            while (true) {
                Lightning.devices[0].KeysLightAllOff();
                Lightning.devices[0].KeysColorUpdate();

                //Fun
                Lightning.devices[0].KeysLightOn(new int[] { 42, 43, 47, 48, 81, 100, 88 });
                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();


                Lightning.devices[0].KeysLightAllOff();
                Lightning.devices[0].KeysColorUpdate();

                //Calm
                Lightning.devices[0].KeysLightOn(new int[] { 42, 43, 47, 48, 99, 100, 101 });
                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();


                Lightning.devices[0].KeysLightAllOff();
                Lightning.devices[0].KeysColorUpdate();

                //Sad
                Lightning.devices[0].KeysLightOn(new int[] { 42, 43, 47, 48, 99, 81, 82, 83, 84, 85, 86, 87, 101 });
                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();


                Lightning.devices[0].KeysLightAllOff();
                Lightning.devices[0].KeysColorUpdate();

                //Angry
                Lightning.devices[0].KeysLightOn(new int[] { 21, 43, 47, 26, 99, 100, 101 });
                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();


                Lightning.devices[0].KeysLightAllOff();
                Lightning.devices[0].KeysColorUpdate();

                //Kind
                Lightning.devices[0].KeysLightOn(new int[] { 42, 21, 43, 47, 26, 48, 99, 100, 101 });
                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();


                Lightning.devices[0].KeysLightAllOff();
                Lightning.devices[0].KeysColorUpdate();

                //Wow
                Lightning.devices[0].KeysLightOn(new int[] { 21, 20, 42, 43, 64, 65, 82, 81, 25, 26, 47, 48, 69, 70, 87, 88, 100 });
                Lightning.devices[0].SendPacket();
                Lightning.devices[0].SpeedSleep();


                Lightning.devices[0].KeysLightAllOff();
                Lightning.devices[0].KeysColorUpdate();
            }
        }
    }
}
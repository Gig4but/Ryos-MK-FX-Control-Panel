using System;
using System.Windows;

namespace RyosMKFXPanel.Effects {
	class Random :LightningModule {
		public override string GetUid() {
			return "DefaultRandom";
		}
		public override string GetName() {
			return "Random";
		}
		public override string GetCategory() {
			return "Effects";
		}
		public override string GetIcon() {
			return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAFgAAABYAEg2RPaAAADEUlEQVRYhbWXP2jTQRTHP5FOUjBQEBwkEbqIIBEEBztkU4JDQREnydBNh1QXx+BURDFDB0WHn4NDQaGLUrcIEXEyYMGl0mTRgg7RDur0dbh35pL+Lr/0Tx4cybv73su7e+++7wVJTGDkJXUkJfY9ij3EZCSxkQfaQCkGnJQDeaAJzAMN4CNQS0VOKARtSeVAL1lIVodDkpO0l9PV6V9rx0a4XgLKKfsSW5vHhYac0j34DnwFNoArQ2vJ0GfJjIfSAHqRA9SAh8Ai0PAOPIuALwOvgGumF4FN4ASDp96tlIBVIEFOYrFMJG0FesPmDiJPSpJ6WQ4UJG1LWrLk6UkqHhBPrEpKpjKuqgu8AS4CW7jE8VdfBKqRfaNywF9/G6h5HmgPjRYwFxh7hkueevDjbXYmH/Q5IE1qOE5o4F5CbwpYAf4MAc8Ca8Apc2bW5r3hup0inVwcrhzgwyd4xpx3MkbsPbFUg/hl5UIzIKIoCUkayYSJpE0z1Avm62Zs1N6mpHlJNUvyWgw7ykhB0lszVg/mexqk2egTs5OXRmGznkvRDPmrq1o4xn3nmbgsQKJB4ulIuj2G4QXbt7AfB/IWv6LpZfWbjKUMw21JP2x/Mgo7ykh9aHNTLpnWxgiDDJvY9w1Jc7txwD81n2w+qfJmSBnX6x3w4diW9EvS4jA2J6lqzBaKr9dV0xMcBddNbwN/gXMRIhKugq6YXgCeA+eBlwQlPiepyc6mokO/3hdxJfgR8BO4Ayzgavp0xIHfwOGU+WXgBvDFDtfyDtSJ83cDR6UzwEn6tFwBrkb2PMVReJpUbH0auOeTqxyJZUi748R+3FGQ9EFSO6srrtEvwS3gHfESvBvpAuvArO8HquxsIv38fKAnwBNcqY5dcZYUgMfABWAl9gq8+MYhlHXgKK5pTZMjuD7yZspaBZe8x4D7wN29tOUF4AFwHPicsj4DXMIdqhvMLwPXgW+4jvg1MLE/JiERFSS1bG7N9P/YSf0181IB3gOngVu43rI7gJjQDWxJ+mSnjtYBZRSj/Ywl+/EXWdh/p2/PpbzGq8cAAAAASUVORK5CYII=";
		}
		public override SliderType GetSliderType() {
			return SliderType.delay;
		}
		public override double GetSliderMax() {
			return 30.0d;
		}
		public override double GetSliderMin() {
			return 15.0d;
		}
		public override double GetSliderTick() {
			return 1.0d;
		}
		public override double GetSliderTickFrequency() {
			return 1.0d;
		}
		public override LightningModuleControls[] GetControls() {
			return new LightningModuleControls[] {
				new CCheckBox("On/Off LEDs", new RoutedEventHandler(OnTurn), new RoutedEventHandler(OffTurn))
			};
		}
		

		private readonly double _Contrast = 4; //TODO Control Number Up/Down
		public bool turn = false;

		//public override Param[] GetParameters() {
		//	return new Param[] {
		//		new Param(_turn.GetType(), nameof(_turn), "Turn")
		//	};
		//}

		public Random() { }



		private System.Random rnd = new System.Random();
		public override void Work() {
			while (true) {
				if (turn) {
					for (int i = 0; i < Lightning.devices[0].GetKeysCount(); i++) {
						Lightning.devices[0].KeyLightSetState(i, Lightning.ToByte(rnd.Next(2)));
					}
				} else {
					Lightning.devices[0].KeysLightAllOn();
				}
				for (int i = 0; i < Lightning.devices[0].GetKeysCount(); i++) {
					Lightning.devices[0].KeyColorChange(i, new float[] {
						RandWithContrast(Lightning.colorGain, Lightning.devices[0].red),
						RandWithContrast(Lightning.colorGain, Lightning.devices[0].green),
						RandWithContrast(Lightning.colorGain, Lightning.devices[0].blue)
					});
				}
				Lightning.devices[0].SendPacket();
			}
		}

		private float RandWithContrast(float min, float max) {
			if (max > 0) {
				double rand = rnd.NextDouble() * (max - min) + min;
				return (float)(rand * Math.Pow(rand / max, _Contrast));
			}
			return min;
		}



		private void OnTurn(object sender, RoutedEventArgs e) {
			turn = true;
		}
		private void OffTurn(object sender, RoutedEventArgs e) {
			turn = false;
		}
	}
}

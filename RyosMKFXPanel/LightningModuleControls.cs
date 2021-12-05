using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RyosMKFXPanel {

	public class LightningModuleControls {
		public StackPanel stack;
		//bool uniqueRow = false;

		public delegate void VisiblityChanged();
		private VisiblityChanged ToVisible;
		private VisiblityChanged ToHidden;

		public Thickness margin = new Thickness(0, 0, 32, 16);

		public LightningModuleControls() {
			stack = new StackPanel();
			stack.Orientation = Orientation.Horizontal;
			stack.VerticalAlignment = VerticalAlignment.Center;
			stack.Margin = margin;
			//stack = new StackPanel();
			//Border border = new Border();
			//border.BorderThickness = new Thickness(2);
			//border.BorderBrush = Brushes.Pink;
			//border.Width = border.Height = 8;
			//stack.Children.Add(border);
		}



		public void AddColumn(Grid grid, double width = 1, bool inStars = true) {
			ColumnDefinition column = new ColumnDefinition();
			column.Width = new GridLength(width, inStars ? GridUnitType.Star : GridUnitType.Pixel);
			grid.ColumnDefinitions.Add(column);
		}

		public void AddRow(Grid grid, double height = 1, bool inStars = true) {
			RowDefinition row = new RowDefinition();
			row.Height = new GridLength(height, inStars ? GridUnitType.Star : GridUnitType.Pixel);
			grid.RowDefinitions.Add(row);
		}


		public Grid CreateGridHorizontal(UIElement[] elements) {
			Grid grid = new Grid();
			for (int i = 0; i < elements.Length; i++) {
				AddColumn(grid);
				elements[i].SetValue(Grid.ColumnProperty, i);
				grid.Children.Add(elements[i]);
			}
			grid.VerticalAlignment = VerticalAlignment.Center;
			//grid.Margin = margin;
			return grid;
		}

		public void CreateGridVertical(UIElement[] elements) {
			Grid grid = new Grid();
			for (int i = 0; i < elements.Length; i++) {
				AddRow(grid);
				elements[i].SetValue(Grid.RowProperty, i);
				grid.Children.Add(elements[i]);
			}
			grid.VerticalAlignment = VerticalAlignment.Center;
			//grid.Margin = margin;
		}



		public static Label CreateLabel(string content, Thickness margin) {
			Label label = new Label();
			label.Content = content;
			label.Margin = margin;
			label.HorizontalAlignment = HorizontalAlignment.Left;
			return label;
		}
		public static CheckBox CreateCheckBox(RoutedEventHandler check, RoutedEventHandler uncheck, bool value) {
			CheckBox checkBox = new CheckBox();
			if (check != null)
				checkBox.Checked += check;
			if (uncheck != null)
				checkBox.Unchecked += uncheck;
			else if (check != null)
				checkBox.Unchecked += check;
			checkBox.IsChecked = value;
			checkBox.HorizontalAlignment = HorizontalAlignment.Left;
			return checkBox;
		}
		public static TextBox CreateTextBox(TextChangedEventHandler change, int length, string value, TextBoxLibrary.TextBoxValidationType type, TextAlignment alignment) {
			TextBox textBox = new TextBox();
			if (change != null)
				textBox.TextChanged += change;
			textBox.MaxLength = length;
			textBox.Width = TextBoxLibrary.TextBoxWidthFromLength(length);
			textBox.PreviewTextInput += TextBoxLibrary.GetInputValidator(type);
			textBox.TextAlignment = alignment;
			textBox.HorizontalAlignment = HorizontalAlignment.Left;
			textBox.Text = value;
			return textBox;
		}


		public void Show(object sender, RoutedEventArgs e) {
			stack.Visibility = Visibility.Visible;
			if (ToVisible != null)
				ToVisible.Invoke();
		}
		public void Hide(object sender, RoutedEventArgs e) {
			stack.Visibility = Visibility.Hidden;
			if (ToHidden != null)
				ToHidden.Invoke();
		}
		public void BindVisibility(VisiblityChanged visible, VisiblityChanged hidden) {
			ToVisible += visible;
			ToHidden += hidden;
		}

			
	}


	public class CCheckBox :LightningModuleControls {
		public CCheckBox(string text = "CChekBox", RoutedEventHandler check = null, RoutedEventHandler uncheck = null, bool value = false) {
			stack.Children.Add(
				CreateCheckBox(check, uncheck, value)
				);
			stack.Children.Add(
				CreateLabel(text, new Thickness(0, -8, 0, -8))
				);
		}
	}

	public class CSpoiler :LightningModuleControls {
		public CSpoiler(CCheckBox control = null, LightningModuleControls closed = null, LightningModuleControls opened = null) {
			if (control == null)
				control = new CCheckBox();
			control.stack.SetValue(Grid.ColumnProperty, 0);

			Grid grid = new Grid();
			if (closed != null) {
				grid.Children.Add(closed.stack);
				closed.stack.SetValue(Grid.ColumnProperty, 1);
				((CheckBox)control.stack.Children[0]).Checked += new RoutedEventHandler(closed.Hide);
				((CheckBox)control.stack.Children[0]).Unchecked += new RoutedEventHandler(closed.Show);
				if (!((CheckBox)control.stack.Children[0]).IsChecked.Value) {
					closed.Show(null, null);
					if (opened != null)
						opened.Hide(null, null);
				}
			}
			if (opened != null) {
				grid.Children.Add(opened.stack);
				opened.stack.SetValue(Grid.ColumnProperty, 1);
				((CheckBox)control.stack.Children[0]).Checked += new RoutedEventHandler(opened.Show);
				((CheckBox)control.stack.Children[0]).Unchecked += new RoutedEventHandler(opened.Hide);
				if (((CheckBox)control.stack.Children[0]).IsChecked.Value) {
					opened.Show(null, null);
					if (closed != null)
						closed.Hide(null, null);
				}
			}
			grid.HorizontalAlignment = HorizontalAlignment.Stretch;
			grid.Margin = new Thickness(-16,0,-16,0);

			WrapPanel wrap = new WrapPanel();
			wrap.Children.Add(control.stack);
			wrap.Children.Add(grid);
			wrap.Margin = new Thickness(-margin.Left, -margin.Top, -margin.Right, -margin.Bottom);

			stack.Children.Add(wrap);


		}
	}


	public static class TextBoxLibrary {
		public static double TextBoxWidthFromLength(int length) {
			return 10*length+4;
		}


		public enum TextBoxValidationType :uint { 
			none = 0,
			numbers = 1,
			hex = 2
		}
		public static void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
			int x = 0;
			if (int.TryParse(e.Text, out x)) {
				e.Handled = false;
			} else {
				e.Handled = true;
			}
		}
		public static void HexValidationTextBox(object sender, TextCompositionEventArgs e) {
			if (Regex.IsMatch(e.Text, "[0-9a-fA-F]*")) {
				e.Handled = false;
			} else {
				e.Handled = true;
			}
		}

		public static TextCompositionEventHandler GetInputValidator(TextBoxValidationType type) {
			switch (type) {
				case TextBoxValidationType.numbers:
					return new TextCompositionEventHandler(NumberValidationTextBox);
				case TextBoxValidationType.hex:
					return new TextCompositionEventHandler(HexValidationTextBox);
				default:
					break;
			}
			return null;
		}


		public static int InputIntParse(TextBox sender, int min = 0) {
			int x = min;
			if (int.TryParse(sender.Text, out x)) {
				if (x < min) {
					sender.Text = min.ToString();
					return min;
				} else {
					return x;
				}
			} else {
				sender.Text = min.ToString();
			}
			return min;
		}



	}

	public class CInputBox :LightningModuleControls {
		public CInputBox(string text = "CTextBox", TextChangedEventHandler change = null, int length = 2, string value = "", TextBoxLibrary.TextBoxValidationType type = TextBoxLibrary.TextBoxValidationType.numbers, TextAlignment alignment = TextAlignment.Right) {
			stack.Children.Add(
				CreateLabel(text, new Thickness(-4, -4, 0, -8))
				);
			stack.Children.Add(
				CreateTextBox(change, length, value, type, alignment)
				);
		}
	}

	public class CInterval :LightningModuleControls {
		public CInterval(string text = "CInterval", TextChangedEventHandler changeMin = null, TextChangedEventHandler changeMax = null, int lengthMin = 2, int lengthMax = 2, string valueMin = "", string valueMax = "", TextBoxLibrary.TextBoxValidationType type = TextBoxLibrary.TextBoxValidationType.numbers, TextAlignment alignment = TextAlignment.Right) {
			stack.Children.Add(
				CreateLabel(text, new Thickness(-4, -4, 0, -8))
				);
			stack.Children.Add(
				CreateTextBox(changeMin, lengthMin, valueMin, type, alignment)
				);
			stack.Children.Add(
				CreateLabel("-", new Thickness(0, -4, 0, -8))
			);
			stack.Children.Add(
				CreateTextBox(changeMax, lengthMax, valueMax, type, alignment)
			);
		}
	}

	public static class IntervalLibrary {
		public static int ChangeMaxValue(int minValue, int maxValue, TextBox input) {
			if (int.TryParse(input.Text, out maxValue)) {
				if (maxValue <= minValue) {
					return minValue + 1;
				} else if (maxValue > Lightning.devices[0].GetWidth() + 1) {
					return Lightning.devices[0].GetWidth() + 1;
				} else {
					return maxValue;
				}
			} else {
				input.Text = maxValue.ToString();
			}
			return maxValue;
		}
		public static int ChangeMinValue(int minValue, int maxValue, TextBox input) {
			if (int.TryParse(input.Text, out minValue)) {
				if (minValue < 1) {
					return 1;
				} else if (minValue >= maxValue) {
					return maxValue - 1;
				} else {
					return minValue;
				}
			} else {
				input.Text = minValue.ToString();
			}
			return minValue;
		}
		public static unsafe void ChangeMaxValueUnsafe(int minValue, int* maxValue, TextBox input) {
			int x = *maxValue;
			if (int.TryParse(input.Text, out x)) {
				if (x <= minValue) {
					*maxValue = minValue + 1;
				} else if (x > Lightning.devices[0].GetWidth() + 1) {
					*maxValue = Lightning.devices[0].GetWidth() + 1;
				} else {
					*maxValue = x;
				}
			} else {
				input.Text = x.ToString();
			}
		}
		public static unsafe void ChangeMinValueUnsafe(int* minValue, int maxValue, TextBox input) {
			int x = *minValue;
			if (int.TryParse(input.Text, out x)) {
				if (x < 1) {
					*minValue = 1;
				} else if (x >= maxValue) {
					*minValue = maxValue - 1;
				} else {
					*minValue = x;
				}
			} else {
				input.Text = x.ToString();
			}
		}
	}


}

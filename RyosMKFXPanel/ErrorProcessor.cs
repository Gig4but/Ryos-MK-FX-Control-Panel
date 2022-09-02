using System.Windows;

namespace RyosMKFXPanel {
	/// <summary>
	/// Represents simple error processing.
	/// </summary>
	static class ErrorProcessor {
		/// <summary>
		/// Shows message error box with text.
		/// </summary>
		/// <param name="message">the message to show.</param>
		public static void MakeErrorWindow(string message) {
			MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		/// <summary>
		/// Processes error codes.
		/// </summary>
		/// <param name="code">the error code.</param>
		/// <param name="customMessage">the addition to error message.</param>
		public static void Error(byte code, string customMessage = "") {
			switch (code) {
				case 0x00:
					MakeErrorWindow("Can't start a work with the device.\n" + customMessage);
					break;
				case 0x01:
					MakeErrorWindow("Can't stop a work with the device.\n" + customMessage);
					break;
				case 0x02:
					MakeErrorWindow("Uncorrect module class.\n" + customMessage);
					break;
				case 0x03:
					MakeErrorWindow("Unknown module indentificator.\n" + customMessage);
					break;
				case 0x04:
					MakeErrorWindow("Module UID duplicate.\n" + customMessage);
					break;
				case 0x05:
					MakeErrorWindow("A UID must contain only letters, digits, or underscores.\n" + customMessage);
					break;
				case 0x06:
					MakeErrorWindow("Wrong type of slider.\n" + customMessage);
					break;
				case 0x07:
					MakeErrorWindow("Wrong category.\n" + customMessage);
					break;
				case 0x0A: {
					MakeErrorWindow("No audio device.\n" + customMessage);
					Lightning.ModuleOff();
					break;
				}
				case 0x80:
					MakeErrorWindow("Unconvertable param type ->" + customMessage);
					break;
				case 0x90:
					MakeErrorWindow("Can't " + customMessage);
					break;
				default: {
					MakeErrorWindow("Unknown error.\n" + customMessage);
					/*foreach (Lightning i in Lightning.devices) {
						i.ModuleOff();
						i.BreakConnection();
					}*/
					Lightning.ModuleOff();
					break;
				}
			}
		}
	}
}

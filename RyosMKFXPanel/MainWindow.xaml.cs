using RyosMKFXPanel.Windows;
using System.Windows;

namespace RyosMKFXPanel {
    public partial class MainWindow :Window {
        public MainWindow() {
            //InitializeComponent();
            ModernMain modernMain = new ModernMain();
            modernMain.Show();
            this.Close();
        }
    }
}

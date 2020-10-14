using RyosMKFXPanel.windowStyles;
using System.Windows;

namespace RyosMKFXPanel {
    public partial class MainWindow :Window {
        public MainWindow() {
            //InitializeComponent();
            styleNew styleNew = new styleNew();
            styleNew.Show();
            this.Close();
        }
    }
}

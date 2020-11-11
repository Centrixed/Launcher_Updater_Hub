//  Matt Underation
//  Created: 2-21-19
//  Modified: 2-22-19
//  This script is supposed to be the front end script for the Update Prompt Window. It handles the front end events
//  and sends them to the backend scripts
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace moVR_Launcher {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UpdatePrompt : Window {
        public UpdatePrompt() {
            InitializeComponent();

            // Center the window on the screen
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.Topmost = true;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e) {
            Launcher.YesUpdateClick();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e) {
            Launcher.NoUpdateClick();
        }
    }
}

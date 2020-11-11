//  Matt Underation
//  Created: 2-14-19
//  Modified: 2-22-19
//  This script is supposed to be the front end script for the Main Window. It handles the front end events
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace moVR_Launcher {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private bool downloading = false;   // used to prevent launching the game mid-download.
        private bool unzipping = false;
        private bool versionChecking = false;   // used to prevent rapid switching between games while the current selection is still version checking

        public MainWindow() {
            InitializeComponent();

            // Center the window on the screen
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Begin the launcher setup
            Launcher.SetupLauncher(this);
        }

        // If the start button is clicked, launch the game (if not downloading)
        private void StartButton_Click(object sender, RoutedEventArgs e) {
            if(!downloading)
                Launcher.LaunchGame();
        }

        // If the update button is clicked, update the build (if there is an update)
        private void UpdateButton_Click(object sender, RoutedEventArgs e) {
            if (!downloading)
                Launcher.UpdateButtonClicked();
        }

        // If the repair button is clicked, redownload the build (if not downloading)
        private void RepairButton_Click(object sender, RoutedEventArgs e) {
            if (!downloading)
                Launcher.RepairButtonClicked();
        }

        // If the moVR game button is clicked, call the "change game" function
        private void moVR_Game_Button_Click(object sender, RoutedEventArgs e) {
            SwitchGameScreen(Launcher.Game.moVR);
        }

        // If the BaB game button is clicked, call the "change game" function
        private void BaB_Game_Button_Click(object sender, RoutedEventArgs e) {
            SwitchGameScreen(Launcher.Game.BaB);
        }

        // If the Concentration game button is clicked, call the "change game" function
        private void Concentration_Game_Button_Click(object sender, RoutedEventArgs e) {
            SwitchGameScreen(Launcher.Game.Concentration);
        }

        // If the UF game button is clicked, call the "change game" function
        private void UF_Game_Button_Click(object sender, RoutedEventArgs e) {
            SwitchGameScreen(Launcher.Game.UF);
        }

        // Takes in a game type, and switches the screen accordingly
        private void SwitchGameScreen(Launcher.Game gameType) {
            // Shows the download bar and buttons
            ToggleDownloadComponents(true);
            // Turns off the ready to play display (to prevent overlay with downloading status)
            ToggleReadyWindow(false);

            // Generates the colors used in highlighting the buttons
            var bc = new BrushConverter();
            var selectColor = (Brush)bc.ConvertFrom("#7ba6d1");
            var defaultColor = (Brush)bc.ConvertFrom("#343A40");

            if (!downloading && !versionChecking && !unzipping) {
                // Toggles back on the start button (if a download was paused, then switched to a new game, fixes the disabled start button)
                ToggleStartButton(true);

                if (gameType == Launcher.Game.moVR) {
                    // Change button colors
                    moVR_Game_Button.Background = selectColor;
                    BaB_Game_Button.Background = defaultColor;
                    Concentration_Game_Button.Background = defaultColor;
                    UF_Game_Button.Background = defaultColor;

                    // Change the splash image
                    BitmapImage bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.UriSource = new Uri("/images/dodgeball_splash.png", UriKind.Relative);
                    bImg.EndInit();
                    GameImage.Source = bImg;

                    // Call the function from launcher to begin the version checking
                    Launcher.ChangeGame(Launcher.Game.moVR);
                }
                else if (gameType == Launcher.Game.BaB) {
                    // Change button colors
                    moVR_Game_Button.Background = defaultColor;
                    BaB_Game_Button.Background = selectColor;
                    Concentration_Game_Button.Background = defaultColor;
                    UF_Game_Button.Background = defaultColor;

                    // Change the splash image
                    BitmapImage bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.UriSource = new Uri("/images/bab_splash.png", UriKind.Relative);
                    bImg.EndInit();
                    GameImage.Source = bImg;

                    // Call the function from launcher to begin the version checking
                    Launcher.ChangeGame(Launcher.Game.BaB);

                }
                else if (gameType == Launcher.Game.Concentration) {
                    // Change button colors
                    moVR_Game_Button.Background = defaultColor;
                    BaB_Game_Button.Background = defaultColor;
                    Concentration_Game_Button.Background = selectColor;
                    UF_Game_Button.Background = defaultColor;

                    // Change the splash image
                    BitmapImage bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.UriSource = new Uri("/images/concentration_splash.png", UriKind.Relative);
                    bImg.EndInit();
                    GameImage.Source = bImg;

                    // Call the function from launcher to begin the version checking
                    Launcher.ChangeGame(Launcher.Game.Concentration);
                }
                else if (gameType == Launcher.Game.UF) {
                    // Change button colors
                    moVR_Game_Button.Background = defaultColor;
                    BaB_Game_Button.Background = defaultColor;
                    Concentration_Game_Button.Background = defaultColor;
                    UF_Game_Button.Background = selectColor;

                    // Change the splash image
                    BitmapImage bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.UriSource = new Uri("/images/uf_splash.png", UriKind.Relative);
                    bImg.EndInit();
                    GameImage.Source = bImg;

                    // Call the function from launcher to begin the version checking
                    Launcher.ChangeGame(Launcher.Game.UF);
                }
            }
        }

        // Toggles everything involving downloading. Used when going from home screen to selecting a game
        public void ToggleDownloadComponents(bool toggle) {
            if (toggle) {
                HomeScreenText.Visibility = Visibility.Hidden;

                GameProgressBar.Visibility = Visibility.Visible;
                DownloadPlayButton.Visibility = Visibility.Visible;
                DownloadStopButton.Visibility = Visibility.Visible;
                GameImage.Visibility = Visibility.Visible;
                GameVersionNumber.Visibility = Visibility.Visible;
                StartButton.Visibility = Visibility.Visible;
                RepairButton.Visibility = Visibility.Visible;
            }
            else {
                GameProgressBar.Visibility = Visibility.Hidden;
                DownloadPlayButton.Visibility = Visibility.Hidden;
                DownloadStopButton.Visibility = Visibility.Hidden;
                GameImage.Visibility = Visibility.Hidden;
                GameVersionNumber.Visibility = Visibility.Hidden;
                StartButton.Visibility = Visibility.Hidden;
                RepairButton.Visibility = Visibility.Hidden;

                HomeScreenText.Visibility = Visibility.Visible;
            }
        }


        // Toggles the visuals for the download status
        public void ToggleDownloadStatus(bool toggle) {
            if (toggle) {
                DownloadStatus.Visibility = Visibility.Visible;

                DownloadStatus.Content = "Downloading...";
                downloading = true;
            }
            else {
                DownloadStatus.Visibility = Visibility.Hidden;
                DownloadStatus.Content = "Downloading...";
                downloading = false;
            }
        }

        // Toggles the visuals for the software being ready or not
        public void ToggleReadyWindow(bool toggle) {
            if (toggle) {
                ReadyPanel.Visibility = Visibility.Visible;
                UpdateDownloadProgress(100);
            }
            else {
                ReadyPanel.Visibility = Visibility.Hidden;
            }
        }

        // Updates the visual version number on the main content screen
        public void UpdateVersionNumber() {
            if (File.Exists(Launcher.clientVersionFile)) {
                GameVersionNumber.Content = "Version " + Launcher.clientVersion;
            }
        }

        // Toggles the "No game file available" window
        public void ToggleNoGameFile(bool toggle) {
            if (toggle)
                NoFile.Visibility = Visibility.Visible;
            else
                NoFile.Visibility = Visibility.Hidden;
        }

        // Toggles the visuals of the start button during & after downloading the file.
        public void ToggleStartButton(bool toggle) {
            if (toggle) {
                // Changes the font color to white
                StartButton.Foreground = Brushes.White;

                // Re-enables the start button
                StartButton.IsEnabled = true;

                // Changes the text back to "Start"
                StartButton.Content = "START";

                // Changes color back to green
                //var bc = new BrushConverter();
                //StartButton.Background = (Brush)bc.ConvertFrom("#1C8734");
            }
            else {
                // Changes the font color to black
                StartButton.Foreground = Brushes.DarkGray;

                // Disables the start button
                StartButton.IsEnabled = false;

                // Changes the button label to "WAIT"
                StartButton.Content = "WAIT";

                // Changes it's color to grey
                //var bc = new BrushConverter();
                //StartButton.Background = (Brush)bc.ConvertFrom("#36393d");
            }
        }

        // Updates the progress bar with whatever value is passed to it
        public void UpdateDownloadProgress(int progress) {
            // If progress is -1, then the download is unzipping
            if(progress == -1) {
                DownloadStatus.Content = "Unzipping...";
                GameProgressBar.IsIndeterminate = true;

                unzipping = true;
            }
            // Otherwise, progress represents the real download percentage number
            else {
                GameProgressBar.IsIndeterminate = false;
                GameProgressBar.Value = progress;

                unzipping = false;
            }
        }

        private void DownloadPlayButton_Click(object sender, RoutedEventArgs e) {
            if (!downloading) {
                Launcher.UpdateButtonClicked();
            }
        }

        private void DownloadStopButton_Click(object sender, RoutedEventArgs e) {
            if (!unzipping && downloading) {
                DownloadStatus.Content = "Download Stopped.";
                downloading = false;

                Launcher.StopDownload();
            }
        }

        // Prevents rapid switching of game modes while the version of another is still being checked
        public void ToggleVersionChecking(bool toggle) {
            versionChecking = toggle;
        }
    }
}

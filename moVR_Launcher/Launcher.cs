//  Matt Underation
//  Created: 2-14-19
//  Modified: 2-14-19
//  Launcher is a backend class that handles the file run, read/write, and update processes in the launcher.
//  It is basically the main control and backend script for the entire launcher program

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.IO.Compression;
using Dropbox.Api;

namespace moVR_Launcher {
    class Launcher {

        // Dropbox API Token
        public static string token = "removed for privacy";


        public static string clientVersion;   // Contains the client's game version
        public static string serverVersion;   // Contains the server's game version

        // A list of the game types
        public enum Game {
            moVR = 0,
            BaB = 1,
            Concentration = 2,
            UF = 3
        };

        // The currently selected game. Used during runtime to determine what files to load/check.
        public static Game currentGameSelection;

        // These strings are assigned during runtime based on the current game selection. They contain path locations and file name strings.
        public static string clientVersionFile = "";
        public static string serverVersionFile = "";
        public static string clientBuildFile = "";
        public static string clientDirectory = "";

        private static MainWindow mainwindowInstance;
        private static UpdatePrompt updateWindow;   // Contains the update prompt when it is instantiated
        private static NewDownloadPrompt newFileWindow;
        private static Task downloadTask;

        private static bool existingBuild;
        private static bool forceStopDownload;

        // TODO: Add encryption and an activation license to the software platform.

        // When the start button is clicked, launch the specified game
        public static void LaunchGame() {
            if (currentGameSelection == Game.moVR) {
                if (System.IO.File.Exists("movr_build/moVR.exe")) {
                    Process.Start(@"movr_build\moVR.exe");
                }
            }
            else if (currentGameSelection == Game.BaB) {
                if (System.IO.File.Exists("BaB_build/Build-A-Bot.exe")) {
                    Process.Start(@"BaB_build\Build-A-Bot.exe");
                }
            }
            else if (currentGameSelection == Game.Concentration) {
                if (System.IO.File.Exists("Concentration_build/Concentration.exe")) {
                    Process.Start(@"Concentration_build\Concentration.exe");
                }
            }
            else if (currentGameSelection == Game.UF) {
                if (System.IO.File.Exists("UF_build/MCL_VR_Florida.exe")) {
                    Process.Start(@"UF_build\MCL_VR_Florida.exe");
                }
            }

            // Close the launcher
            Environment.Exit(0);
        }

        // If the update button is clicked, check if an update is required and do the necessary steps.
        public static void UpdateButtonClicked() {
            // If the "no game file" message is showing, hide it when you click the update button
            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.ToggleNoGameFile(false);
            });

            var task = Task.Run(Launcher.DownloadVersionFile);
        }

        // If the repair button is clicked, delete the current version file and redownload
        public static void RepairButtonClicked() {
            // Reset the client version
            clientVersion = "0.0.0";

            var task = Task.Run(Launcher.DownloadVersionFile);
        }

        // Begins the tasks for reading the version files
        public static void SetupLauncher(MainWindow window) {
            // Grab the instance of the main window (used for some ownership configs with the subwindows)
            mainwindowInstance = window;

            // Begins the process of checking versions, and downloading the game (if needed)
            // CheckForExistingVersion();
        }

        // Called by the mainwindow function when a game button is clicked. Assigns a current game selection and 
        public static void ChangeGame(Game selection) {
            currentGameSelection = selection;

            CheckForExistingVersion();
        }

        // Checks whether or not the game file exists, then responds accordingly
        private static void CheckForExistingVersion() {
            // Sets the version file string to null
            clientVersionFile = "";

            // Sets the file string based on the selected game.
            if (currentGameSelection == Game.moVR)
                clientVersionFile = "version_moVR_client.txt";
            else if (currentGameSelection == Game.BaB)
                clientVersionFile = "version_BaB_client.txt";
            else if (currentGameSelection == Game.Concentration)
                clientVersionFile = "version_Concentration_client.txt";
            else if (currentGameSelection == Game.UF)
                clientVersionFile = "version_UF_client.txt";
            else
                Console.WriteLine("uwu I made a fucky");

            // Prevents the user from switching games during version checking
            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.ToggleVersionChecking(true);
            });

            // If there is already a version of the game that exists in the directory
            if (System.IO.File.Exists(clientVersionFile)) {
                // Set the existing build bool to true
                existingBuild = true;

                // Reads in the value of the client version
                var clientFile = new StreamReader(clientVersionFile);
                clientVersion = clientFile.ReadLine();
                Console.WriteLine($"Client Version: {clientVersion}");
                clientFile.Close();

                // Sets the version number on the main window
                mainwindowInstance.Dispatcher.Invoke(() => {
                    mainwindowInstance.UpdateVersionNumber();
                });

                // Run the task for downloading the version file
                var task = Task.Run(Launcher.DownloadVersionFile);
            }
            // If there is no version of the game currently in the directory
            else {
                // Set the existing build bool to false
                existingBuild = false;

                // Displays UI that tells the user there is no game file.
                mainwindowInstance.Dispatcher.Invoke(() => {
                    mainwindowInstance.ToggleNoGameFile(true);
                });

                // Run the task for downloading the version file
                var task = Task.Run(Launcher.DownloadVersionFile);
            }
        }

        // Connects to dropbox, downloads the text file, and reads in the value.
        private static async Task DownloadVersionFile() {
            // Sets the file string based on the selected game.
            if (currentGameSelection == Game.moVR)
                serverVersionFile = "version_moVR_server.txt";
            else if (currentGameSelection == Game.BaB)
                serverVersionFile = "version_BaB_server.txt";
            else if (currentGameSelection == Game.Concentration)
                serverVersionFile = "version_Concentration_server.txt";
            else if (currentGameSelection == Game.UF)
                serverVersionFile = "version_UF_server.txt";

            using (var dbx = new DropboxClient(token)) {

                using (var response = await dbx.Files.DownloadAsync("/" + serverVersionFile)) {
                    var s = response.GetContentAsByteArrayAsync();
                    s.Wait();   // Wait for download to finish.
                    var d = s.Result;
                    System.IO.File.WriteAllBytes(serverVersionFile, d);
                }

                // Read Text File
                var serverFile = new StreamReader(serverVersionFile);
                serverVersion = serverFile.ReadLine();
                Console.WriteLine($"Server Version: {serverVersion}");
                serverFile.Close();

                // Delete Text File
                try {
                    System.IO.File.Delete(serverVersionFile);
                }
                catch (Exception ex) {
                    Console.WriteLine("Could not delete " + serverVersionFile + ":" + ex.Source);
                }

                CompareVersions();
            }
        }

        // Compares the version numbers. If they do not match, then prompt the user to download it.
        private static void CompareVersions() {
            // If there is an existing build in the program directory, prompt the user for an update
            if (existingBuild) {
                if (clientVersion != serverVersion) {
                    Console.WriteLine("Versions Do Not Match. Creating update prompt");

                    // REPLACED: Toggling update prompt. Instead just going straight to the update without a choice.
                    YesUpdateClick();
                }
                else {
                    Console.WriteLine("Versions Match. You are up to date!");

                    ToggleReadyWindow(true);
                }
            }
            // If there is no existing build in the program directory, just begin downloading it
            else {
                Console.WriteLine("No Existing Build. Creating new download prompt.");

                mainwindowInstance.Dispatcher.Invoke(() => {
                    mainwindowInstance.ToggleNoGameFile(false);
                });

                // Immediately begins the download without prompting the user
                YesNewFileClick();
            }

            // Sets the version checking bool back to false. 
            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.ToggleVersionChecking(false);
            });
        }

        // Connects to dropbox and downloads the game file
        private static async Task DownloadGameFile() {
            Console.WriteLine("Downloading Game File");

            if (currentGameSelection == Game.moVR)
                clientBuildFile = "./moVR_build.zip";
            else if (currentGameSelection == Game.BaB)
                clientBuildFile = "./BaB_build.zip";
            else if (currentGameSelection == Game.Concentration)
                clientBuildFile = "./Concentration_build.zip";
            else if (currentGameSelection == Game.UF)
                clientBuildFile = "./UF_build.zip";

            // Delete the previous game build zip file
            if (System.IO.File.Exists(clientBuildFile)) {
                System.IO.File.Delete(clientBuildFile);
            }

            // Sets the force stop download to false
            forceStopDownload = false;

            // Create new timeout config to allow for larger f ile downloads
            var aHttpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 }) { Timeout = TimeSpan.FromMinutes(20) };
            var aDropboxConfig = new DropboxClientConfig("moVR") { HttpClient = aHttpClient };

            using (var dbx = new DropboxClient(token, aDropboxConfig)) {
                // Remove first two directory characters from the path string before using it to create a file stream.
                clientBuildFile = clientBuildFile.Remove(0, 2);

                string file = clientBuildFile;

                var response = await dbx.Files.DownloadAsync("/" + file);
                ulong fileSize = response.Response.Size;
                const int bufferSize = 1024 * 1024;

                var buffer = new byte[bufferSize];

                using (var stream = await response.GetContentAsStreamAsync()) {
                    using (var outputFile = new FileStream(clientBuildFile, FileMode.OpenOrCreate)) {
                        var length = stream.Read(buffer, 0, bufferSize);

                        while (length > 0) {
                            outputFile.Write(buffer, 0, length);
                            var percentage = 100 * (ulong)outputFile.Length / fileSize;

                            // Update progress bar with the percentage.
                            var progress = (int)percentage;
                            UpdateDownloadProgress(progress);

                            length = stream.Read(buffer, 0, bufferSize);

                            if (forceStopDownload == true) {
                                outputFile.Close();
                                stream.Close();
                                ToggleDownloadStatus(false);
                                return;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Game File Download Completed");
            UnpackBuild();
        }

        // Unpacks the .zip file into the movr_build folder, updates the version number, and updates the front end accordingly
        private static void UnpackBuild() {
            Console.WriteLine("Unzipping Build");

            // Inserts the './' back into the file string for the game (temporarily removed in the DownloadGameFiles function)
            clientBuildFile = "./" + clientBuildFile;

            Console.WriteLine("ClientBuildFile post insert: " + clientBuildFile);

            // Saves the client build file name under zipFile.
            string zipFile = clientBuildFile;

            // Removes the '.zip' from the file name in order to properly create a directory folder.
            clientBuildFile = clientBuildFile.Substring(0, clientBuildFile.Length - 4);

            Console.WriteLine("ClientBuildFile post directory: " + clientBuildFile);

            // Delete the previous game build directory
            if (Directory.Exists(clientBuildFile)) {
                DirectoryInfo di = new DirectoryInfo(clientBuildFile);
                foreach (FileInfo file in di.GetFiles()) {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories()) {
                    dir.Delete(true);
                }
            }

            // Creates a new directory
            Directory.CreateDirectory(clientBuildFile);

            // Updates the progress screen
            UpdateDownloadProgress(-1);

            // Extracts the zip file to that new directory
            ZipFile.ExtractToDirectory(zipFile, clientBuildFile + "/");

            Console.WriteLine("Unzip Complete");

            // Deletes the old zip file
            System.IO.File.Delete(zipFile);

            Console.WriteLine("Deleted zip");

            // Delete the client version txt file (if it exists)
            if (System.IO.File.Exists(clientVersionFile))
                System.IO.File.Delete("./" + clientVersionFile);

            // Modifies the version_client.txt file to the new version number
            var newFile = new StreamWriter(clientVersionFile);
            newFile.Write(serverVersion);
            newFile.Close();
            clientVersion = serverVersion;

            // Updates the version number on the main window
            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.UpdateVersionNumber();
            });

            // Toggle the proper UI.
            ToggleStartButton(true);
            ToggleDownloadStatus(false);
            ToggleReadyWindow(true);

            // Sets the existing build to true
            existingBuild = true;
        }

        // Creates a prompt for an update
        private static void OpenUpdatePrompt() {
            updateWindow = new UpdatePrompt();
            updateWindow.ShowDialog();
            updateWindow.Owner = mainwindowInstance;
        }

        // Destroys the prompt for an update
        private static void CloseUpdatePrompt() {
            updateWindow.Close();
            //updatePromptThread.Abort();
        }

        // Creates a prompt for a new file download
        private static void OpenNewFilePrompt() {
            newFileWindow = new NewDownloadPrompt();
            newFileWindow.ShowDialog();
            newFileWindow.Owner = mainwindowInstance;
        }

        // Destroys the prompt for a new file download
        private static void CloseNewFilePrompt() {
            newFileWindow.Close();
        }

        // If the "Yes" button is clicked on the update prompt
        public static void YesUpdateClick() {
            //CloseUpdatePrompt();

            // Toggle the UI properly
            ToggleStartButton(false);
            ToggleReadyWindow(false);
            ToggleDownloadStatus(true);

            // Begin the download process
            downloadTask = Task.Run(DownloadGameFile);
        }

        // If the "No" button is clicked on the update prompt
        public static void NoUpdateClick() {
            ToggleReadyWindow(true);
            CloseUpdatePrompt();
        }

        // If the "Yes" button is clicked on the new file download prompt
        public static void YesNewFileClick() {
            //CloseNewFilePrompt();

            // Hide the "no file for download" prompt
            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.ToggleNoGameFile(false);
            });

            // Toggle on the download visuals, and off the ready visuals
            ToggleStartButton(false);
            ToggleDownloadStatus(true);
            ToggleReadyWindow(false);

            // Begin the download process
            downloadTask = Task.Run(DownloadGameFile);
        }

        // If the "No" button is clicked on the new file download prompt
        public static void NoNewFileClick() {
            CloseNewFilePrompt();

            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.ToggleNoGameFile(true);
            });
        }

        // Required to enable/disable the download UI on the existing mainwindow (due to permissions or something)
        private static void ToggleDownloadStatus(bool toggle) {
            mainwindowInstance.Dispatcher.Invoke(() =>
            {
                mainwindowInstance.ToggleDownloadStatus(toggle);
            });
        }

        // Required to enable/disable the ready UI on the existing mainwindow (due to permissions or something)
        private static void ToggleReadyWindow(bool toggle) {
            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.ToggleReadyWindow(toggle);
            });
        }

        // Sends the string to the update download progress function in main window
        private static void UpdateDownloadProgress(int progress) {
            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.UpdateDownloadProgress(progress);
            });
        }

        // Toggles the start button visuals based on the download status
        private static void ToggleStartButton(bool toggle) {
            mainwindowInstance.Dispatcher.Invoke(() => {
                mainwindowInstance.ToggleStartButton(toggle);
            });
        }

        // Stops the game from downloading and resets all panels
        public static void StopDownload() {
            forceStopDownload = true;
        }
    }
}

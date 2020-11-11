# VR Software Launcher & Updater

The current launcher I built in early 2019 using a .NET WPF application and Dropbox API to update/install our VR software experiences. 

This was before I had a better understanding of the Single Responsibility Principle and the Open/Close principle, so the code structure could absolutely be improved.
 
Most of the functionality is encapsulated in the Launcher.cs file, with the other .xaml.cs files managing UI input. 

In hindsight I should have built the application as a ASP.NET Web App, using the Azure cloud to store the data.

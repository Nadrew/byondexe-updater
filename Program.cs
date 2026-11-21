
using byondexe_updater.Helpers.Download;
using byondexe_updater.Helpers.Output;
using byondexe_updater.Helpers.Version;


internal class Program {
    private static void Main(string[] args) {
        Output.Write("\n\nBYONDexe Updater 1.0 by Nadrew\n", Output.MESSAGE_LEVEL);
        Output.WriteLong("This program will update any existing BYONDexe installations in the same directory as it, if available.\nIf no installation is found, it will download the latest version and install it.\n\n", Output.MESSAGE_LEVEL);
        if (args.Length > 0 && args[0] == "--verbose") {
            Output.CURRENT_LEVEL = Output.VERBOSE_LEVEL;
            Output.Write("Verbose mode enabled.\n", Output.VERBOSE_LEVEL);
        }
     
        Output.Write("Checking current version...", Output.MESSAGE_LEVEL);
        string extractPath = Path.Combine(Directory.GetCurrentDirectory(), "temp_extract");
        string setupPath = Directory.GetCurrentDirectory();
        try {
            // We check byondcore.dll for the version number because it will always exist and has the BYOND build in it.
            // byondexe.exe unforunately doesn't have a version number.
            float currentVersion = VersionHelper.GetBYONDExeVersion();
            Output.WriteStatus($"[{currentVersion}]", Output.MESSAGE_LEVEL, ConsoleColor.Green);
            Output.Write("Checking for an updated version on the BYOND website...", Output.MESSAGE_LEVEL);
            float latestVersion = VersionHelper.GetLatestBYONDExeVersion();
            // We have to grab the latest version from the build index HTML using some regex.
            // BYOND doesn't provide a simple way to grab the latest version number, maybe we can get one.
            string downloadPath = Path.Combine(Directory.GetCurrentDirectory(), "downloads", $"{latestVersion}_byondexe.zip");
            if(latestVersion <= currentVersion) {
                Output.WriteStatus("[CURRENT]", Output.MESSAGE_LEVEL, ConsoleColor.Yellow);
            }
            else {
                // If the current version is out of date, download the latest version if it's not already cached.
                Output.WriteStatus($"[{latestVersion}]", Output.MESSAGE_LEVEL, ConsoleColor.Green);
                if(DownloadHelper.Download(latestVersion)) {
                    if(DownloadHelper.Extract(downloadPath, extractPath)){
                        // Then we extract the zip file and copy the temporary files to the current directory
                        // It will skip files that are normally edited by the user, if they exist.
                        // But certain files will always be overwritten, like byondexe.exe and the README files.
                        Output.Write("Zip file extracted successfully", Output.VERBOSE_LEVEL);
                        DownloadHelper.CopyFiles(extractPath, setupPath);
                    }
                    else{
                        Output.Write("Failed to extract the downloaded zip file.", Output.VERBOSE_LEVEL);
                    }
                }
            }
        }
        catch (Exception) {
            // If it doesn't find a BYONDexe installtion, it will download the latest version and install it.
            // This works pretty much like the above, except it treats it like a fresh install.
            // This lets people grab the latest version quickly, since there's not an easy link on the BYOND website.
            Output.Write("\nNo current BYONDexe installation found, downloading latest version...", Output.MESSAGE_LEVEL);
            float latestVersion = VersionHelper.GetLatestBYONDExeVersion();
            string downloadPath = Path.Combine(Directory.GetCurrentDirectory(), "downloads", $"{latestVersion}_byondexe.zip");
            if(DownloadHelper.Download(latestVersion)) {
                Output.WriteStatus("[DONE]", Output.MESSAGE_LEVEL, ConsoleColor.Green);
                Output.Write("Extracting BYONDexe to the current directory...", Output.MESSAGE_LEVEL);
                if(DownloadHelper.Extract(downloadPath, extractPath)) {
                    DownloadHelper.CopyFiles(extractPath, setupPath);
                    Directory.Delete(extractPath, recursive: true);
                    Output.WriteStatus("[DONE]", Output.MESSAGE_LEVEL, ConsoleColor.Green);
                }
                else {
                    Output.WriteStatus("[FAIL]", Output.MESSAGE_LEVEL, ConsoleColor.Red);
                }
            }
            else {
                Output.WriteStatus("[FAIL]", Output.MESSAGE_LEVEL, ConsoleColor.Red);
            }
        }
        Output.Write("\n\nAll done! Press any key to exit...", Output.MESSAGE_LEVEL);
        Console.ReadKey();
    }
}
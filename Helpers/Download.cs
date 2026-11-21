using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;


namespace byondexe_updater.Helpers.Download {
    using byondexe_updater.Helpers.Output;
    public static class DownloadHelper {
        private static readonly HashSet<string> alwaysOverwrite =
        [
            with(StringComparer.OrdinalIgnoreCase),
            "byondexe.exe",
            "README.txt",
            "README-CONFIG.txt",
            "verpatch.exe",
            "setup/livetest.html"
        ];

        public static bool Download(float latestVersion) {
            string downloadUrl = $"https://www.byond.com/download/build/LATEST/{latestVersion}_byondexe.zip";
            string downloadFolder = Path.Combine(Directory.GetCurrentDirectory(), "downloads");
            string downloadPath = Path.Combine(downloadFolder, $"{latestVersion}_byondexe.zip");
            if(File.Exists(downloadPath)) {
                Output.Write($"byondexe.zip version {latestVersion} already exists at {downloadPath}. Skipping download.", Output.VERBOSE_LEVEL);
                return true;
            }
            Directory.CreateDirectory(downloadFolder);
            using HttpClient client = new();
            HttpResponseMessage response = client.GetAsync(downloadUrl).Result;
            if (response.IsSuccessStatusCode) {
                using (FileStream fs = new(downloadPath, FileMode.Create))
                {
                    response.Content.CopyToAsync(fs).Wait();
                }
                Output.Write($"Downloaded BYONDexe version {latestVersion} to {downloadPath}\n", Output.VERBOSE_LEVEL);
                return true;

            }
            else{
                Output.Write($"Failed to download BYONDexe from BYOND, status code: {response.StatusCode}\n", Output.VERBOSE_LEVEL);
                return false;
            }
        }
        public static bool Extract(string zipFilePath, string extractPath) {
            if (!File.Exists(zipFilePath)) {
                Console.WriteLine($"Zip file {zipFilePath} does not exist. Cannot extract.\n");
                return false;
            }
            System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath, true);
            Output.Write($"Extracted {zipFilePath} to {extractPath}\n", Output.VERBOSE_LEVEL);
            return true;
        }
        public static void CopyFiles(string sourceDir, string targetDir) {
            sourceDir += "/byondexe";
            if (!Directory.Exists(sourceDir)) {
                Output.Write($"Source directory {sourceDir} does not exist. Cannot copy files.\n", Output.VERBOSE_LEVEL);
                return;
            }
            Output.Write($"Copying files from {sourceDir} to {targetDir}\n ", Output.VERBOSE_LEVEL);
            Directory.CreateDirectory(targetDir);
            foreach (string file in Directory.GetFiles(sourceDir,"*", SearchOption.AllDirectories)) {
                string fileName = Path.GetFileName(file);
                string relativePath = Path.GetRelativePath(sourceDir, file);
                string destFile = Path.Combine(targetDir, relativePath);
                bool isWhitelisted = alwaysOverwrite.Contains(fileName) ||
                         alwaysOverwrite.Contains(relativePath) ||
                         relativePath.StartsWith("setup" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
                if (!isWhitelisted && File.Exists(destFile)) {
                    Output.Write($"{fileName} skipped\n", Output.VERBOSE_LEVEL);
                    continue;
                }
                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
                File.Copy(file, destFile, true);
                Output.Write($"{fileName} copied\n", Output.VERBOSE_LEVEL);
            }
            
        }
    }
}
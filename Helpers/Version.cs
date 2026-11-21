using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace byondexe_updater.Helpers.Version {
    using byondexe_updater.Helpers.Output;
    public static partial class VersionHelper {
    private const string VERSION_URL = "https://www.byond.com/download/build/LATEST/?C=M;O=D";
    public static float GetBYONDExeVersion() {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "setup/bin/byondcore.dll");
        if (!File.Exists(filePath)) {
            Output.Write("setup/bin/byondcore.dll not found, unable to determine current version.\nPlease ensure that the BYONDexe Updater is located in the same directory as the BYONDexe files.", Output.VERBOSE_LEVEL);
            throw new FileNotFoundException("setup/bin/byondcore.dll not found, unable to determine current version.\nPlease ensure that the BYONDexe Updater is located in the same directory as the BYONDexe files.");
        }
        string? currentVersion = FileVersionInfo.GetVersionInfo(filePath).FileVersion?[4..];
        if (float.TryParse(currentVersion, out float currentVersionFloat)) {
            return currentVersionFloat;
        }
        return currentVersionFloat;
    }

    public static float GetLatestBYONDExeVersion() {
        string url = VERSION_URL;
        using HttpClient client = new();
        HttpResponseMessage response = client.GetAsync(url).Result;
        if (response.IsSuccessStatusCode) {
            string content = response.Content.ReadAsStringAsync().Result;
            Match match = LinkFind.Match(content);
            if (match.Success) {
            string latestVersion = match.Groups[1].Value[0..8];
            if (float.TryParse(latestVersion, out float latestVersionFloat)) {
                return latestVersionFloat;
                }
            }
        }
        else {
            Output.Write($"Failed to fetch the latest BYONDexe version from BYOND, status code: {response.StatusCode}", Output.VERBOSE_LEVEL);
            return 0.0f;
        }
        return 0.0f; 
    }

    [GeneratedRegex(@"href=""(\d+\.\d+_[^""]+)""")]
    private static partial Regex LinkFind { get; }
    }
}
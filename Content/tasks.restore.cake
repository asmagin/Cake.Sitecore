#addin "Cake.Npm&version=0.17.0"

Action<string, string, string, IEnumerable<string>> restoreNuGetPackages = (string solutionPath, string nuGetConfigPath, string libsNuGetDir, IEnumerable<string> additionalFeeds) => {
    NuGetRestoreSettings _settings = null;

    if (FileExists(nuGetConfigPath))
    {
        Warning("NuGet configuration file found and will be used.");

        _settings = new NuGetRestoreSettings 
        {
            ConfigFile = nuGetConfigPath
        };
    }
    else {
        Warning("NuGet configuration file not found and defaults and local settings will be used.");

        var nuGetFeedList = new List<string> {
            "https://api.nuget.org/v3/index.json",
            "https://sitecore.myget.org/F/sc-packages/api/v3/index.json"
        };

        nuGetFeedList.AddRange(additionalFeeds ?? Enumerable.Empty<string>());

        if (!string.IsNullOrEmpty(libsNuGetDir))
            nuGetFeedList.Add(libsNuGetDir);
        
        _settings = new NuGetRestoreSettings {
            Source = nuGetFeedList
        };
    }

    NuGetRestore(solutionPath, _settings);
};

Sitecore.Tasks.RestoreNuGetPackagesTask = Task("Restore :: Restore NuGet Packages")
    .Description("Restore NuGet packages for a solution")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionFilePath, "SolutionFilePath", "SOLUTION_FILE_PATH");

        restoreNuGetPackages(
            Sitecore.Parameters.SolutionFilePath,
            Sitecore.Parameters.NuGetConfigPath,
            Sitecore.Parameters.LibsNuGetDir,
            null
        );
    });

Sitecore.Tasks.RestoreNpmPackagesTask = Task("Restore :: Restore NPM Packages")
   .Description("Restore Npm packages for a solution")
   .Does(() => {
       Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");

       var settings = new NpmInstallSettings();
       settings.LogLevel = NpmLogLevel.Error;
       settings.FromPath(Sitecore.Parameters.SrcDir);

       NpmInstall(settings);
   });

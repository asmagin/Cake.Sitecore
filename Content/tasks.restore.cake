#addin "Cake.Npm&version=0.17.0"

Sitecore.Tasks.RestoreNuGetPackagesTask = Task("Restore :: Restore NuGet Packages")
    .Description("Restore NuGet packages for a solution")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionFilePath, "SolutionFilePath", "SOLUTION_FILE_PATH");

        // default NuGet settings
        NuGetRestoreSettings _settings = null;

        if (FileExists(Sitecore.Parameters.NuGetConfigPath))
        {
            Warning("NuGet configuration file found and will be used.");

            _settings =new NuGetRestoreSettings 
            {
                ConfigFile = Sitecore.Parameters.NuGetConfigPath
            };
        }
        else {
            Warning("NuGet configuration file not found and defaults and local settings will be used.");
            Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.LibsNuGetDir, "LibsNuGetDir", "LIBS_NUGET_DIR");
            
            _settings =new NuGetRestoreSettings
            {
                Source = new List<string>
                {
                    "https://api.nuget.org/v3/index.json;",
                    "https://sitecore.myget.org/F/sc-packages/api/v3/index.json",
                    Sitecore.Parameters.LibsNuGetDir
                }
            };
        }

        NuGetRestore(Sitecore.Parameters.SolutionFilePath, _settings);
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

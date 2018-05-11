#addin "Cake.Npm"

Sitecore.Tasks.RestoreNuGetPackagesTask = Task("Restore :: Restore NuGet Packages")
    .Description("Restore NuGet packages for solution")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionFilePath, "SolutionFilePath", "SOLUTION_FILE_PATH");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.LibsNuGetDir, "LibsNuGetDir", "LIBS_NU_GET_DIR");

        NuGetRestore(
            Sitecore.Parameters.SolutionFilePath,
            new NuGetRestoreSettings
            {
                Source = new List<string>
                {
                    "https://api.nuget.org/v3/index.json;",
                    "https://sitecore.myget.org/F/sc-packages/api/v3/index.json",
                    Sitecore.Parameters.LibsNuGetDir
                }
            });
    });

Sitecore.Tasks.RestoreNpmPackagesTask = Task("Restore :: Restore NPM Packages")
    .Description("Restore Npm packages for solution")
    .Does(() => {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");

        var settings = new NpmInstallSettings();
        settings.LogLevel = NpmLogLevel.Error;
        settings.FromPath(Sitecore.Parameters.SrcDir);

        NpmInstall(settings);
    });

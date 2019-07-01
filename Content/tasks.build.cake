#addin "Cake.FileHelpers&version=3.2.0"

Sitecore.Tasks.GenerateVersionFileTask = Task("Build :: Generate Version.txt file")
    .Description("Creates a file with detailed information about the build in publishing target directory (`PUBLISHING_TARGET_DIR`)")
    .Does(() => {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.PublishingTargetDir, "PublishingTargetDir", "PUBLISHING_TARGET_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ReleaseVersion, "ReleaseVersion", "RELEASE_VERSION");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BranchName, "BranchName", "BRANCH_NAME");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commit, "Commit", "COMMIT");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BuildNumber, "BuildNumber", "BUILD_NUMBER");

        EnsureDirectoryExists(Sitecore.Parameters.PublishingTargetDir);

        var file = $"{Sitecore.Parameters.PublishingTargetDir}/version.txt";
        string[] lines = {
            $"Version:    {Sitecore.Parameters.ReleaseVersion}",
            $"Branch:     {Sitecore.Parameters.BranchName}",
            $"Commit:     {Sitecore.Parameters.Commit}",
            $"Build:      {Sitecore.Parameters.BuildNumber}",
            "",
            $"TimeStamp:  {System.DateTime.UtcNow.ToString()}"
        };

        System.IO.File.WriteAllLines(file, lines);
    });

Sitecore.Tasks.SetPackageJsonVersionTask = Task("Build :: Set Version in package.json")
    .Description("Updates version in `packages.json` with current build version (`VERSION`)")
    .Does(() => {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ReleaseVersion, "Version", "VERSION");

        var file = $"{Sitecore.Parameters.SrcDir}/package.json";
        ReplaceRegexInFiles(file, "(?<=\"version\": \")(.+?)(?=\",)", Sitecore.Parameters.ReleaseVersion);
    });

Sitecore.Tasks.SetAssemblyVersionTask = Task("Build :: Set Version in Assembly.cs files")
    .Description("Updates `Assembly.cs` version(`ASSEMBLY_VERSION`)before the build in each project in source directory (`SRC_DIR`)")
    .Does(() => {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.AssemblyVersion, "AssemblyVersion", "ASSEMBLY_VERSION");

        var files = GetFiles($"{Sitecore.Parameters.SrcDir}/**/AssemblyInfo.cs");
        foreach(var file in files)
        {
            ReplaceRegexInFiles(file.ToString(), "(?<=Assembly(File)?Version\\(\")(.+?)(?=\"\\))", Sitecore.Parameters.AssemblyVersion);
        }
    });

// As of now it is expected that task will be excuted on the build server
Sitecore.Tasks.DownloadLicenseFileTask = Task("Build :: Download License File")
    .Description("Download license file from remote address (`SC_LICENSE_URI`) to the (`ROOT_DIR`)")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BuildConfiguration, "BuildConfiguration", "BUILD_CONFIGURATION");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.RootDir, "RootDir", "ROOT_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ScLicenseUri, "ScLicenseUri", "SC_LICENSE_URI");

        var license_file_token = ArgumentOrEnvironmentVariable("SC_LICENSE_TOKEN", "", "");
        var license_file_uri = ArgumentOrEnvironmentVariable("SC_LICENSE_URI", "", "");
        
        var url = $"{license_file_uri}" + $"{license_file_token}";

        DownloadFile(url, $"{Sitecore.Parameters.RootDir}/license.xml");
    });

// As of now it is expected that your packages.json file will have tasks "sc:codegen" to use code generation
Sitecore.Tasks.GenerateCodeTask = Task("Build :: Generate Code")
    .Description("Executes JS plugin to parse Unicorn files via `npm run` and generate code.")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");

        var settings = new NpmRunScriptSettings();

        settings.ScriptName = "sc:codegen"; // TODO: move to configurations
        settings.LogLevel = NpmLogLevel.Error;
        settings.FromPath(Sitecore.Parameters.SrcDir);

        NpmRunScript(settings);
    });

// As of now it is expected that your packages.json file will have tasks "build:Debug" or "build:Release"
Sitecore.Tasks.BuildClientCodeTask = Task("Build :: Build Client Code")
    .Description("Executes front-end code build via calling `npm run build:<your build configuration>`.")
    .Does(() => {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BuildConfiguration, "BuildConfiguration", "BUILD_CONFIGURATION");

        var settings = new NpmRunScriptSettings();

        settings.ScriptName = $"build:{Sitecore.Parameters.BuildConfiguration}"; // TODO: move to configurations
        settings.LogLevel = NpmLogLevel.Error;
        settings.FromPath(Sitecore.Parameters.SrcDir);

        NpmRunScript(settings);
    });

Sitecore.Tasks.BuildServerCodeTask = Task("Build :: Build Server Code")
    .Description("Runs MsBuild for a solution (`SOLUTION_FILE_PATH`) with a specific build configuration (`BUILD_CONFIGURATION`)")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BuildConfiguration, "BuildConfiguration", "BUILD_CONFIGURATION");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionFilePath, "SolutionFilePath", "SOLUTION_FILE_PATH");

        var msBuildConfig = new MSBuildSettings()
            .SetConfiguration(Sitecore.Parameters.BuildConfiguration)
            .SetVerbosity(Verbosity.Minimal) // TODO: figure out how to get access to -Verbosity flag
            .UseToolVersion(Sitecore.Parameters.MsBuildToolVersion)
            .SetMaxCpuCount(new int?()) // TODO: make configurable
            .WithTarget("Rebuild"); // TODO: move to configuration

        MSBuild(Sitecore.Parameters.SolutionFilePath, msBuildConfig);
    });

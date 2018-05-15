// //////////////////////////////////////////////////
// Dependencies
// //////////////////////////////////////////////////
#tool nuget:?package=Cake.Sitecore&prerelease
#load nuget:?package=Cake.Sitecore&prerelease

using System;
using System.Text.RegularExpressions

// //////////////////////////////////////////////////
// Arguments
// //////////////////////////////////////////////////
var Target = ArgumentOrEnvironmentVariable("target", "", "Default");

// //////////////////////////////////////////////////
// Prepare
// //////////////////////////////////////////////////

Sitecore.Constants.SetNames();
Sitecore.Parameters.InitParams(
    context: Context,
    msBuildToolVersion: MSBuildToolVersion.Default,
    solutionName: "Habitat",
    scSiteUrl: "https://sc9.local",
    rootDir: "./",
    libsDir: "./lib",
    srcDir: "./src",
    solutionFilePath: "./Habitat.sln",
    unicornConfigPath: "./src/Foundation/Serialization/code/App_Config/Include/Unicorn.SharedSecret.config"
);

// //////////////////////////////////////////////////
// Tasks
// //////////////////////////////////////////////////

Task("Helix")
    .Does(() => {
        var line1 = "The Habitat source code, tools and processes are examples of Sitecore Helix.";
        var line2 = "Habitat is not supported by Sitecore and should be used at your own risk.";

        Warning("O---o   _____ _ _                            _   _      _ _      ");
        Warning(" O-o   /  ___(_) |                          | | | |    | (_)     "); 
        Warning("  O    \\ `--. _| |_ ___  ___ ___  _ __ ___  | |_| | ___| |___  __");
        Warning(" o-O    `--. \\ | __/ _ \\/ __/ _ \\| '__/ _ \\ |  _  |/ _ \\ | \\ \\/ /");
        Warning("o---O  /\\__/ / | ||  __/ (_| (_) | | |  __/ | | | |  __/ | |>  < ");
        Warning("O---o  \\____/|_|\\__\\___|\\___\\___/|_|  \\___| \\_| |_/\\___|_|_/_/\\_\\");
        Warning(" O-o                                                             ");
        Warning("  O    -------------------- helix.sitecore.net ------------------");
        Warning(" o-O   "); 
        Warning("o---O  " + line1);
        Warning("O---o  " + line2);
        Warning(" O-o   "); 
        Warning("  O    ----------------------------------------------------------");
        Warning(" o-O   ");
        Warning("o---O  ");
    })
    .IsDependentOn(Sitecore.Tasks.CleanWildcardFoldersTaskName)
    .IsDependentOn(Sitecore.Tasks.ConfigureToolsTaskName)
    ;

Task("Copy-Sitecore-License")
    .Does(() => {
        Warning("Please, put Sitecore license in ./lib manually");
    })
    ;

Task("Copy-Sitecore-Lib")
    .Does(() => {
        Warning("Please, put Sitecore dlls in ./lib/Sitecore manually");
    })
    ;

Task("Restore")
    .IsDependentOn(Sitecore.Tasks.RestoreNuGetPackagesTask)
    .IsDependentOn(Sitecore.Tasks.RestoreNpmPackagesTaskName)
    ;

Task("Publish-All-Projects")
    .IsDependentOn(Sitecore.Tasks.PublishFoundationTaskName)
    .IsDependentOn(Sitecore.Tasks.PublishFeatureTaskName)
    .IsDependentOn(Sitecore.Tasks.PublishProjectTaskName)
    ;

Task("Apply-Xml-Transform")
    .Does(() => {
        // Equivalent of gulp "Apply-Xml-Transform"
        Func<IFileSystemInfo, bool> exclude_bin_obj = 
            fileSystemInfo => !fileSystemInfo.Path.FullPath.Contains("/bin/") 
                && !fileSystemInfo.Path.FullPath.Contains("/obj/");

        var _files = new List<FilePath>();
        
        _files.AddRange(GetFiles($"{Sitecore.Parameters.SrcDir}/Foundation/**/*.xdt", exclude_bin_obj));
        _files.AddRange(GetFiles($"{Sitecore.Parameters.SrcDir}/Feature/**/*.xdt", exclude_bin_obj));
        _files.AddRange(GetFiles($"{Sitecore.Parameters.SrcDir}/Project/**/*.xdt", exclude_bin_obj));

        foreach(var _file in _files){
            Verbose($"Found file: {_file.ToString()}");
            
            var rgx = new Regex(".+code/(.+)\\.xdt");
            var _fileToTransform = rgx.Replace(_file.ToString(), "$1");

            Verbose($"Applying configuration transform: {_fileToTransform}");

            var _settings = new MSBuildSettings()
                .SetConfiguration(Sitecore.Parameters.BuildConfiguration)
                .SetVerbosity(Verbosity.Minimal)
                .UseToolVersion(Sitecore.Parameters.MsBuildToolVersion)
                .WithTarget("ApplyTransform")
                .WithProperty("WebConfigToTransform", Sitecore.Parameters.PublishingTargetDir)
                .WithProperty("TransformFile", _file.ToString())
                .WithProperty("FileToTransform", _fileToTransform);

            MSBuild($"{Sitecore.Parameters.RootDir}/scripts/applytransform.targets", _settings);
        }
    })
    ;

Task("Sync-Unicorn")
    .IsDependentOn(Sitecore.Tasks.SyncAllUnicornItemsName)
    ;

Task("Publish-Transforms")
    .Does(() => {
        // Equivalent of gulp "Publish-Transforms"
        EnsureDirectoryExists($"{Sitecore.Parameters.PublishingTargetDir}/temp/transforms");
        CopyFiles($"{Sitecore.Parameters.SrcDir}/**/code/**/*.xdt", $"{Sitecore.Parameters.PublishingTargetDir}/temp/transforms");
    })
    ;

// //////////////////////////////////////////////////
// Targets
// //////////////////////////////////////////////////

Task("Default") // LocalDev
    .IsDependentOn("Helix")
    .IsDependentOn("Copy-Sitecore-License")
    .IsDependentOn("Copy-Sitecore-Lib")
    .IsDependentOn("Restore")
    .IsDependentOn("Publish-All-Projects")
    .IsDependentOn("Apply-Xml-Transform")
    .IsDependentOn("Sync-Unicorn")
    .IsDependentOn("Publish-Transforms")
    ;

// //////////////////////////////////////////////////
// Execution
// //////////////////////////////////////////////////

RunTarget(Target);
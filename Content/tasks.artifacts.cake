
Sitecore.Tasks.OptimizeBuildArtifactsTask = Task("Artifacts :: Optimize")
    .Description("Exclude unnecessary files from target directory (`ARTIFACTS_BUILD_DIR`).")
    .Does(() =>
    {
        EnsureDirectoryExists(Sitecore.Parameters.ArtifactsBuildDir);
        
        Action<string, List<FilePath>> includeFiles = (pattern, collection) =>
        {
            collection.AddRange(GetFiles(pattern));
        };

        // TODO: rewrite with glob
        var excludeList = new List<FilePath>();

        // DLLs
        includeFiles($"{Sitecore.Parameters.ArtifactsBuildDir}/bin/(Sitecore|Lucene|Newtonsoft|System)*.dll", excludeList);
        includeFiles($"{Sitecore.Parameters.ArtifactsBuildDir}/bin/Microsoft.Web.Infrastructure*.dll", excludeList);
        // Files
        includeFiles($"{Sitecore.Parameters.ArtifactsBuildDir}/bin/*.pdb", excludeList);
        includeFiles($"{Sitecore.Parameters.ArtifactsBuildDir}/compilerconfig.json.defaults", excludeList);
        includeFiles($"{Sitecore.Parameters.ArtifactsBuildDir}/packages.config", excludeList);

        foreach(var filePath in excludeList)
        {
            if (!FileExists(filePath))
            {
                Debug($"Not Found: {filePath}");
                continue;
            }

            Debug($"Excluding: {filePath}");
            DeleteFile(filePath);
        }
    });


Sitecore.Tasks.GatherBuildConfigsTask = Task("Artifacts :: Copy configuration files")
    .Description("Copy configuration files from source config directory (`SRC_CONFIGS_DIR`) to artifact directory (`ARTIFACTS_SRC_CONFIGS_DIR`).")
    .Does(() =>
    {
        EnsureDirectoryExists(Sitecore.Parameters.ArtifactsSrcConfigsDir);
        
        Debug($"Copy configuration files from '{Sitecore.Parameters.SrcConfigsDir}' to '{Sitecore.Parameters.ArtifactsSrcConfigsDir}'");
        CopyDirectory(Sitecore.Parameters.SrcConfigsDir, Sitecore.Parameters.ArtifactsSrcConfigsDir);
    });

Sitecore.Tasks.GatherBuildScriptsTask = Task("Artifacts :: Copy build scripts")
    .Description("Copy build scripts from source directory (`SRC_SCRIPTS_DIR`) to artifact directory (`ARTIFACTS_SRC_DIR`).")
    .Does(() =>
    {
        EnsureDirectoryExists(Sitecore.Parameters.ArtifactsSrcDir);

        // Copy mandatory scripts
        Debug($"Copy cake scripts to '{Sitecore.Parameters.ArtifactsSrcDir}'");
        var files = new [] {
                "./build.cake",
                "./build.ps1"
            };
        CopyFiles(files, Directory(Sitecore.Parameters.ArtifactsSrcDir));

        // Copy optional configuration files
        var configfilelist = (Sitecore.Parameters.SrcConfigFiles);
        char[] delimiterChars = {',', ';'};
        Debug($"Copy optional configuration files {configfilelist} to '{Sitecore.Parameters.ArtifactsSrcDir}'");      
        var configfiles = (configfilelist.Replace(" ","")).Split(delimiterChars);
        foreach(var configfile in configfiles)
        {
            CopyFiles(configfile, Directory(Sitecore.Parameters.ArtifactsSrcDir));
        }
        
        // Copy mandatory configuration files
        var targetDir = Directory(Sitecore.Parameters.ArtifactsSrcScriptsDir);
        EnsureDirectoryExists(targetDir);

        Debug($"Copy scripts '{Sitecore.Parameters.SrcScriptsDir}' to '{targetDir}'");
        CopyDirectory(Directory(Sitecore.Parameters.SrcScriptsDir), targetDir);
    });

Sitecore.Tasks.GatherSitecorePackagesTask = Task("Artifacts :: Copy Sitecore packages")
    .Description("Copy Sitecore packages from source directory (`LIBS_PACKAGES_DIR`) to artifact directory (`ARTIFACTS_LIBS_PACKAGES_DIR`).")
    .Does(() =>
    {
        var sourceFolder = $"{Sitecore.Parameters.LibsPackagesDir}";
        var targetFolder = $"{Sitecore.Parameters.ArtifactsLibsPackagesDir}";
        EnsureDirectoryExists(targetFolder);

        Verbose($"Copying packages files from {sourceFolder} to {targetFolder}.");

        CopyDirectory(Directory(sourceFolder), targetFolder);
    });
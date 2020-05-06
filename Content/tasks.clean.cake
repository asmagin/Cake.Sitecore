Sitecore.Tasks.CleanWildcardFoldersTask = Task("Clean-up :: Clean Wildcard Folders")
    .Description("Remove items from project ./dist and ./App_data folders")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");

        var _layerDirectories =
            GetDirectories($"{Sitecore.Parameters.SrcDir}/Foundation") +
            GetDirectories($"{Sitecore.Parameters.SrcDir}/Feature") +
            GetDirectories($"{Sitecore.Parameters.SrcDir}/Project");

        var _projectDirectories = Sitecore.Parameters.SupportHelix20 
            ? _layerDirectories.SelectMany(_dir => GetDirectories($"{_dir}/*/website")) 
            : _layerDirectories.SelectMany(_dir => GetDirectories($"{_dir}/*/code"));

        var _wildcardDirectories = _projectDirectories
            .SelectMany(_dir => GetDirectories($"{_dir}/dist") + GetDirectories($"{_dir}/App_Data/{Sitecore.Parameters.UnicornSerializationRoot}"))
            .ToArray();

        foreach (var directory in _wildcardDirectories)
        {
            CleanDirectory(directory);
        }
    });

Sitecore.Tasks.CleanArtifactsTask = Task("Clean-up :: Clean Artifacts")
    .Description("Clean artifacts (ARTIFACTS_DIR) and output (OUTPUT_DIR) directories")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ArtifactsDir, "ArtifactsDir", "ARTIFACTS_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.OutputDir, "OutputDir", "OUTPUT_DIR");
        
        EnsureDirectoryExists(Sitecore.Parameters.ArtifactsDir);
        Verbose($"Cleaning '{Sitecore.Parameters.ArtifactsDir}' directory");
        CleanDirectory(Sitecore.Parameters.ArtifactsDir);

        EnsureDirectoryExists(Sitecore.Parameters.OutputDir);
        Verbose($"Cleaning '{Sitecore.Parameters.OutputDir}' directory");
        CleanDirectory(Sitecore.Parameters.OutputDir);
    });
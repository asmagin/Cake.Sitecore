Sitecore.Tasks.CleanWildcardFoldersTask = Task("Clean-up :: Clean Wildcard Folders")
    .Description("Remove items from project ./dist and ./App_data folders")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");

        foreach(var directory in GetDirectories($"{Sitecore.Parameters.SrcDir}/**/code/dist"))
        {
            CleanDirectory(directory);
        }

        foreach(var directory in GetDirectories($"{Sitecore.Parameters.SrcDir}/**/code/App_Data/unicorn"))
        {
            CleanDirectory(directory);
        }
    });


Sitecore.Tasks.CleanArtifactsTask = Task("Clean-up :: Clean Artifacts")
    .Description("Clean artifacts folder")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ArtifactsDir, "ArtifactsDir", "ARTIFACTS_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.OutputDir, "OutputDir", "OUTPUT_DIR");
        
        EnsureDirectoryExists(Sitecore.Parameters.ArtifactsDir);
        Debug($"Cleaning '{Sitecore.Parameters.ArtifactsDir}' directory");
        CleanDirectory(Sitecore.Parameters.ArtifactsDir);

        EnsureDirectoryExists(Sitecore.Parameters.OutputDir);
        Debug($"Cleaning '{Sitecore.Parameters.OutputDir}' directory");
        CleanDirectory(Sitecore.Parameters.OutputDir);
    });
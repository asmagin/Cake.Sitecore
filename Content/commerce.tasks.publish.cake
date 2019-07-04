Action<DirectoryPath> cleanCommerceSiteDirectoriesTask = (targetDir) => {
    EnsureDirectoryExists(targetDir);
    Information($"Cleaning '{targetDir}' site root directory");

    // ToDo: IIS stop & start for the site
    FileWriteText(
        targetDir.CombineWithFilePath(FilePath.FromString($"App_Offline.htm")),
        string.Empty);

    var _excludedExtensions = new [] { ".log", ".txt", ".md" };
    RetryAccessDenied((retryCount) => {
            Verbose($"Cleaning directory '{targetDir}' at the {retryCount} attempt");
            CleanDirectory(
                targetDir,
                fileSystemInfo => !_excludedExtensions.Contains(FilePath.FromString(fileSystemInfo.Path.FullPath).GetExtension()));
        }, 10, 500);
};

Action<DirectoryPath> publishCommerceEngineProject = (publishingTargetDir) => {
    Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.BuildConfiguration, "BuildConfiguration", "COMMERCE_BUILD_CONFIGURATION");
    Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.EngineProjectPath, "EngineProjectPath", "COMMERCE_ENGINE_PROJECT_PATH");

    EnsureDirectoryExists(publishingTargetDir);

    // ToDo: does it make sense to reuse task.publish.cake?
    var _msBuildSettings = new MSBuildSettings()
        .SetConfiguration(Sitecore.Parameters.Commerce.BuildConfiguration)
        .SetVerbosity(Verbosity.Minimal)
        .UseToolVersion(Sitecore.Parameters.MsBuildToolVersion)
        .WithTarget("Rebuild")
        .WithProperty("DeployOnBuild", "true")
        .WithProperty("DeployDefaultTarget", "WebPublish")
        .WithProperty("WebPublishMethod", "FileSystem")
        .WithProperty("DeleteExistingFiles", "false")
        .WithProperty("PublishUrl", publishingTargetDir.ToString());

    var _projectFilePath = FilePath.FromString(Sitecore.Parameters.Commerce.EngineProjectPath);
    MSBuild(_projectFilePath, _msBuildSettings);
};

Action<DirectoryPath, string> applyAndDeleteCommerceEngineJsonTransformation = (commerceEngineDir, commerceRoleName) => {
    EnsureDirectoryExists(commerceEngineDir);

    // ToDo: patterns should be configured?
    transformJsonFile(
        Context,
        commerceEngineDir.CombineWithFilePath(FilePath.FromString($"wwwroot\\config.json")),
        commerceEngineDir.CombineWithFilePath(FilePath.FromString($"wwwroot\\config.transform.{commerceRoleName}.json")));

    DeleteFiles($"{commerceEngineDir}/wwwroot/config.transform.*.json");
};

Sitecore.Commerce.Tasks.PublishEngineAuthoringTask = Task("Commerce :: Publish :: Publish Engine to Authoring")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_AUTHORING_LOCAL_WEBSITE_ROOT_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.AuthoringLocalWebsiteRootDir, "AuthoringLocalWebsiteRootDir", "COMMERCE_AUTHORING_LOCAL_WEBSITE_ROOT_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.AuthoringLocalWebsiteRootDir);
        cleanCommerceSiteDirectoriesTask(_commerceSiteRootDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineJsonTransformation(_commerceSiteRootDir, "Authoring");
    });

Sitecore.Commerce.Tasks.PublishEngineMinionsTask = Task("Commerce :: Publish :: Publish Engine to Minions")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_MINIONS_LOCAL_WEBSITE_ROOT_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.MinionsLocalWebsiteRootDir, "MinionsLocalWebsiteRootDir", "COMMERCE_MINIONS_LOCAL_WEBSITE_ROOT_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.MinionsLocalWebsiteRootDir);
        cleanCommerceSiteDirectoriesTask(_commerceSiteRootDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineJsonTransformation(_commerceSiteRootDir, "Minions");
    });

Sitecore.Commerce.Tasks.PublishEngineOpsTask = Task("Commerce :: Publish :: Publish Engine to Ops")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_OPS_LOCAL_WEBSITE_ROOT_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.OpsLocalWebsiteRootDir, "OpsLocalWebsiteRootDir", "COMMERCE_OPS_LOCAL_WEBSITE_ROOT_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.OpsLocalWebsiteRootDir);
        cleanCommerceSiteDirectoriesTask(_commerceSiteRootDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineJsonTransformation(_commerceSiteRootDir, "Ops");
    });

Sitecore.Commerce.Tasks.PublishEngineShopsTask = Task("Commerce :: Publish :: Publish Engine to Shops")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_SHOPS_LOCAL_WEBSITE_ROOT_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.ShopsLocalWebsiteRootDir, "ShopsLocalWebsiteRootDir", "COMMERCE_SHOPS_LOCAL_WEBSITE_ROOT_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.ShopsLocalWebsiteRootDir);
        cleanCommerceSiteDirectoriesTask(_commerceSiteRootDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineJsonTransformation(_commerceSiteRootDir, "Shops");
    });

Sitecore.Commerce.Tasks.PublishArtifactsTask = Task("Commerce :: Publish :: Publish Artifacts")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_ARTIFACTS_BUILD_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.ArtifactsBuildDir, "ArtifactsBuildDir", "COMMERCE_ARTIFACTS_BUILD_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.ArtifactsBuildDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
    });

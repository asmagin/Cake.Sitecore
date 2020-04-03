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

    var _dotNetCorePublishSettings = new DotNetCorePublishSettings
    {
        Configuration = Sitecore.Parameters.Commerce.BuildConfiguration,
        OutputDirectory = publishingTargetDir,
        NoRestore = true,
        NoBuild = true,
        Verbosity = DotNetCoreVerbosity.Minimal
    };

    var _projectFilePath = FilePath.FromString(Sitecore.Parameters.Commerce.EngineProjectPath);
    DotNetCorePublish(_projectFilePath.ToString(), _dotNetCorePublishSettings);
};

Action<DirectoryPath, string> applyAndDeleteCommerceEngineRoleTransformation = (commerceEngineDir, commerceRoleName) => {
    Verbose($"Checking directory '{commerceEngineDir}'");
    EnsureDirectoryExists(commerceEngineDir);

    // ToDo: patterns should be configured?
    var baseFile = commerceEngineDir.CombineWithFilePath(FilePath.FromString($"wwwroot\\config.json"));
    var transformFile = commerceEngineDir.CombineWithFilePath(FilePath.FromString($"wwwroot\\config.transform.{commerceRoleName}.json"));

    Verbose($"Transforming '{baseFile}' file using '{transformFile}'");
    transformJsonFile(Context, baseFile, transformFile);

    Verbose($"Clearing transformation files");
    DeleteFiles($"{commerceEngineDir}/wwwroot/config.transform.*.json");
};

Action<DirectoryPath, string> applyAndDeleteCommerceEngineEnvironmentTransformation = (commerceEngineDir, environmentName) => {
    var rootConfigFolder = commerceEngineDir.Combine(DirectoryPath.FromString($"wwwroot"));
    EnsureDirectoryExists(rootConfigFolder);
    Debug($"Checking files under '{rootConfigFolder} directory'");

    var configFilePaths = Context.GetFiles($"{rootConfigFolder}/*/**/*.json");
    foreach (var configFilePath in configFilePaths)
    {
        Verbose($"Processing '{configFilePath} file'");
        var configDirectoryPath = configFilePath.GetDirectory();
        var configFileName = configFilePath.GetFilename();

        var transformFilePath = configDirectoryPath.CombineWithFilePath(FilePath.FromString($"{configFileName}.{environmentName}.transform"));
        if (Context.FileExists(transformFilePath)) {
            Verbose($"Transforming '{configFilePath}' file using '{transformFilePath}'");
            transformJsonFile(Context, configFilePath, transformFilePath);
        } else {
            Debug($"The config file '{configFilePath}' does not have transformation file '{transformFilePath}'");
        }

        var transformFilePaths = Context.GetFiles($"{configDirectoryPath}/{configFileName}.*.transform");
        if (transformFilePaths.Count() > 0) {
            DeleteFiles(transformFilePaths);
        }
    }
};

Sitecore.Commerce.Tasks.PublishEngineAuthoringTask = Task("Commerce :: Publish :: Publish Engine to Authoring")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_AUTHORING_LOCAL_WEBSITE_ROOT_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.AuthoringLocalWebsiteRootDir, "AuthoringLocalWebsiteRootDir", "COMMERCE_AUTHORING_LOCAL_WEBSITE_ROOT_DIR");

        // TODO: move the following block to separate action
        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.AuthoringLocalWebsiteRootDir);
        cleanCommerceSiteDirectoriesTask(_commerceSiteRootDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineEnvironmentTransformation(_commerceSiteRootDir, "Development");
        applyAndDeleteCommerceEngineRoleTransformation(_commerceSiteRootDir, "Authoring");
    });

Sitecore.Commerce.Tasks.PublishEngineMinionsTask = Task("Commerce :: Publish :: Publish Engine to Minions")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_MINIONS_LOCAL_WEBSITE_ROOT_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.MinionsLocalWebsiteRootDir, "MinionsLocalWebsiteRootDir", "COMMERCE_MINIONS_LOCAL_WEBSITE_ROOT_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.MinionsLocalWebsiteRootDir);
        cleanCommerceSiteDirectoriesTask(_commerceSiteRootDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineEnvironmentTransformation(_commerceSiteRootDir, "Development");
        applyAndDeleteCommerceEngineRoleTransformation(_commerceSiteRootDir, "Minions");
    });

Sitecore.Commerce.Tasks.PublishEngineOpsTask = Task("Commerce :: Publish :: Publish Engine to Ops")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_OPS_LOCAL_WEBSITE_ROOT_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.OpsLocalWebsiteRootDir, "OpsLocalWebsiteRootDir", "COMMERCE_OPS_LOCAL_WEBSITE_ROOT_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.OpsLocalWebsiteRootDir);
        cleanCommerceSiteDirectoriesTask(_commerceSiteRootDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineEnvironmentTransformation(_commerceSiteRootDir, "Development");
        applyAndDeleteCommerceEngineRoleTransformation(_commerceSiteRootDir, "Ops");
    });

Sitecore.Commerce.Tasks.PublishEngineShopsTask = Task("Commerce :: Publish :: Publish Engine to Shops")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_SHOPS_LOCAL_WEBSITE_ROOT_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.ShopsLocalWebsiteRootDir, "ShopsLocalWebsiteRootDir", "COMMERCE_SHOPS_LOCAL_WEBSITE_ROOT_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.ShopsLocalWebsiteRootDir);
        cleanCommerceSiteDirectoriesTask(_commerceSiteRootDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineEnvironmentTransformation(_commerceSiteRootDir, "Development");
        applyAndDeleteCommerceEngineRoleTransformation(_commerceSiteRootDir, "Shops");
    });

Sitecore.Commerce.Tasks.PublishArtifactsTask = Task("Commerce :: Publish :: Publish Artifacts")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) to (`COMMERCE_ARTIFACTS_BUILD_DIR`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.ArtifactsBuildDir, "ArtifactsBuildDir", "COMMERCE_ARTIFACTS_BUILD_DIR");

        var _commerceSiteRootDir = DirectoryPath.FromString(Sitecore.Parameters.Commerce.ArtifactsBuildDir);
        publishCommerceEngineProject(_commerceSiteRootDir);
        applyAndDeleteCommerceEngineEnvironmentTransformation(_commerceSiteRootDir, "Environment");
    });

// ToDo: transformation apply for CI: how to do it?

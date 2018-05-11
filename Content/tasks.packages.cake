Sitecore.Tasks.CopySpeRemotingFilesTask = Task("Packages :: Copy SPE remoting files")
    .Description("Copy folders with Spe to a target publishing location.")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.PublishingTargetDir, "PublishingTargetDir", "PUBLISHING_TARGET_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.LibsSpeDir, "LibsSpeDir", "LIBS_SPE_DIR");

        Debug($"Copying files from {Sitecore.Parameters.LibsSpeDir} to {Sitecore.Parameters.PublishingTargetDir}.");
        CopyDirectory(Sitecore.Parameters.LibsSpeDir, Sitecore.Parameters.PublishingTargetDir);
    });

Sitecore.Tasks.CopyShipFilesTask = Task("Packages :: Copy Ship files")
    .Description("Copy folders with Spe to a target publishing location.")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.PublishingTargetDir, "PublishingTargetDir", "PUBLISHING_TARGET_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.LibsShipDir, "LibsShipDir", "LIBS_SHIP_DIR");

        Debug($"Copying files from {Sitecore.Parameters.LibsShipDir} to {Sitecore.Parameters.PublishingTargetDir}.");
        CopyDirectory(Sitecore.Parameters.LibsShipDir, Sitecore.Parameters.PublishingTargetDir);
    });


Sitecore.Tasks.PrepareWebConfigTask = Task("Packages :: Prepare web.config")
    .Description("Transfor and copy web.config to target folder, to make packages working")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.PublishingTargetDir, "PublishingTargetDir", "PUBLISHING_TARGET_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcConfigsDir, "SrcConfigsDir", "SRC_CONFIGS_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ScNodeEnv, "ScNodeEnv", "SC_NODE_ENV");

        var _sourceFilePath = $"{Sitecore.Parameters.SrcConfigsDir}/Sitecore/web.config";
        var _targetFilePath = $"{Sitecore.Parameters.PublishingTargetDir}/web.config";
        var _transforms = Sitecore.Parameters.ScNodeEnv.Split('|').ToList();

        Debug($"Tranforming {_sourceFilePath} to {_targetFilePath}.");
        transform(_sourceFilePath, _targetFilePath, _transforms);
    });

// Installation will be skipped, if SC_SITE_URL is not set 
Sitecore.Tasks.RunPackagesInstallationTask = Task("Packages :: Install")
    .Description("Run installation of Sitecore packages using PowerShell Remoting Tools.")
    .Does(() =>
    {
        if (string.IsNullOrEmpty(Sitecore.Parameters.ScSiteUrl))
        {
            Warning($"Variable 'ScSiteUrl' is not set. Skipping step...");
        }

        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.LibsPackagesDir, "LibsPackagesDir", "LIBS_PACKAGES_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ScNodeRole, "ScNodeRole", "SC_NODE_ROLE");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ScSiteUrl, "ScSiteUrl", "SC_SITE_URL");

        var _transforms = Sitecore.Parameters.ScNodeRole.Split('|').ToList();
        var _files = new List<FilePath>();
        foreach(var _role in _transforms){
            Debug($"Installing packages from '{Sitecore.Parameters.LibsPackagesDir}/{_role}/*.zip' into '{Sitecore.Parameters.ScSiteUrl}'");

            _files.AddRange(GetFiles($"{Sitecore.Parameters.LibsPackagesDir}/{_role}/*.zip"));
        }

        DeploySitecorePackages(Sitecore.Parameters.ScSiteUrl, _files);
    })    
    .OnError(exception =>
    {
        Warning("Installation of packages failed: " + exception.Message);
    });
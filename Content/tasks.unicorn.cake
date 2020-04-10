Sitecore.Tasks.SyncAllUnicornItems = Task("Sync :: Unicorn")
    .Description("Run Unicorn sync process")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ScSiteUrl, "ScSiteUrl", "SC_SITE_URL");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.UnicornConfigPath, "UnicornConfigPath", "UNICORN_CONFIG_PATH");

        Information($"Unicorn config path: {Sitecore.Parameters.UnicornConfigPath}");
        
        var _unicornSecret = getUnicornSecret(Sitecore.Parameters.UnicornConfigPath, Sitecore.Parameters.UnicornSecret);
        var _scriptsDir = Context.Tools.Resolve("unicorn/*.psm1").GetDirectory().ToString(); // TODO: resolve dir from tools
        
        runUnicornSync(Sitecore.Parameters.ScSiteUrl, _unicornSecret, _scriptsDir, Sitecore.Parameters.UnicornConfigurations, Sitecore.Parameters.ScBasicAuth);
    });

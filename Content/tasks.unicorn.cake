#load "./scripts/unicorn/unicorn.cake"

Sitecore.Tasks.SyncAllUnicornItems = Task("Sync :: Unicorn")
    .Description("Run Unicorn sync process")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.ScSiteUrl, "ScSiteUrl", "SC_SITE_URL");

        var _unicornSecret = getUnicornSecret(unicornConfigPath);
        var _scriptsDir = ""; // TODO: resolve dir from tools

        runUnicornSync(ScSiteUrl, _unicornSecret, _scriptsDir);
    });
 
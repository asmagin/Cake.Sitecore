#addin "Cake.Powershell"

Sitecore.Tasks.ConfigureToolsTask = Task("Prepare :: Configure Tools")
    .Description("Provide configurations for tools")
    .Does(() =>
    {
        StartPowershellScript("$ProgressPreference='SilentlyContinue';");
        StartPowershellScript("npm set progress=false;");
    });
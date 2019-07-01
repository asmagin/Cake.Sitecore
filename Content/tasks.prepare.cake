#addin "Cake.Powershell&version=0.4.8"

Sitecore.Tasks.ConfigureToolsTask = Task("Prepare :: Configure Tools")
    .Description("Executes basic configuration to optimize build performance (e.g. hide progress bars)")
    .Does(() =>
    {
        StartPowershellScript("$ProgressPreference='SilentlyContinue';");
        StartPowershellScript("npm set progress=false;");
    });
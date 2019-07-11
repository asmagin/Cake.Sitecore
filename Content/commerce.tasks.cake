public class CommerceTasks {
    // Tasks
    public CakeTaskBuilder RestoreNuGetPackagesTask { get; set; }

    public CakeTaskBuilder BuildCommerceEngineCodeTask { get; set; }
    public CakeTaskBuilder RunServerUnitTestsTask { get; set; }

    public CakeTaskBuilder PublishEngineAuthoringTask { get; set; }
    public CakeTaskBuilder PublishEngineMinionsTask { get; set; }
    public CakeTaskBuilder PublishEngineOpsTask { get; set; }
    public CakeTaskBuilder PublishEngineShopsTask { get; set; }
    public CakeTaskBuilder PublishArtifactsTask { get; set; }

    public CakeTaskBuilder BootstrapCommerceConfigurationTask { get; set; }

    // Task names
    public string RestoreNuGetPackagesTaskName =>           GetTaskName(this.RestoreNuGetPackagesTask);
    public string BuildCommerceEngineCodeTaskName =>        GetTaskName(this.BuildCommerceEngineCodeTask);
    public string RunServerUnitTestsTaskName =>             GetTaskName(this.RunServerUnitTestsTask);
    public string PublishEngineAuthoringTaskName =>         GetTaskName(this.PublishEngineAuthoringTask);
    public string PublishEngineMinionsTaskName =>           GetTaskName(this.PublishEngineMinionsTask);
    public string PublishEngineOpsTaskName =>               GetTaskName(this.PublishEngineOpsTask);
    public string PublishEngineShopsTaskName =>             GetTaskName(this.PublishEngineShopsTask);
    public string PublishArtifactsTaskName =>               GetTaskName(this.PublishArtifactsTask);
    public string BootstrapCommerceConfigurationTaskName => GetTaskName(this.BootstrapCommerceConfigurationTask);

    // private helpers
    private static string GetTaskName(CakeTaskBuilder taskBuilder) => TaskExtensions.GetTaskName(taskBuilder);
}

public static partial class Sitecore
{
    public static class Commerce
    {
        public static CommerceTasks Tasks = new CommerceTasks();
    }
}

Sitecore.Commerce.Tasks.RestoreNuGetPackagesTask = Task("Commerce :: Restore :: Restore NuGet Packages")
    .Description("Commerce: Restore NuGet packages for a solution")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.SolutionFilePath, "SolutionFilePath", "COMMERCE_SOLUTION_FILE_PATH");

        restoreNuGetPackages(
            Sitecore.Parameters.Commerce.SolutionFilePath,
            Sitecore.Parameters.Commerce.NuGetConfigPath,
            Sitecore.Parameters.LibsNuGetDir,
            null
        );
    });

Sitecore.Commerce.Tasks.BuildCommerceEngineCodeTask = Task("Commerce :: Build :: Build Server Code")
    .Description("Runs MsBuild for a solution (`COMMERCE_SOLUTION_FILE_PATH`) with a specific build configuration (`COMMERCE_BUILD_CONFIGURATION`)")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.BuildConfiguration, "BuildConfiguration", "COMMERCE_BUILD_CONFIGURATION");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.SolutionFilePath, "SolutionFilePath", "COMMERCE_SOLUTION_FILE_PATH");

        var _msBuildSettings = new MSBuildSettings()
            .SetConfiguration(Sitecore.Parameters.Commerce.BuildConfiguration)
            .SetVerbosity(Verbosity.Minimal) // TODO: figure out how to get access to -Verbosity flag
            .UseToolVersion(Sitecore.Parameters.MsBuildToolVersion)
            .WithTarget("Rebuild"); // TODO: move to configuration

        MSBuild(Sitecore.Parameters.Commerce.SolutionFilePath, _msBuildSettings);
    });

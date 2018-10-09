public class Tasks {
    // Tasks
    public CakeTaskBuilder BuildClientCodeTask { get; set; }
    public CakeTaskBuilder BuildServerCodeTask { get; set; }
    public CakeTaskBuilder CleanArtifactsTask { get; set; }
    public CakeTaskBuilder CleanWildcardFoldersTask { get; set; }
    public CakeTaskBuilder ConfigureToolsTask { get; set; }
    public CakeTaskBuilder CopyShipFilesTask { get; set; }
    public CakeTaskBuilder CopySpeRemotingFilesTask { get; set; }
    public CakeTaskBuilder DownloadLicenseFileTask { get; set; }
    public CakeTaskBuilder GatherBuildConfigsTask { get; set; }
    public CakeTaskBuilder GatherBuildScriptsTask { get; set; }
    public CakeTaskBuilder GatherSitecorePackagesTask { get; set; }
    public CakeTaskBuilder GenerateCodeTask { get; set; }
    public CakeTaskBuilder GenerateVersionFileTask { get; set; }
    public CakeTaskBuilder MergeCoverageReportsTask { get; set; }
    public CakeTaskBuilder OptimizeBuildArtifactsTask { get; set; }
    public CakeTaskBuilder PrepareWebConfigTask { get; set; }
    public CakeTaskBuilder PublishFeatureTask { get; set; }
    public CakeTaskBuilder PublishFoundationTask { get; set; }
    public CakeTaskBuilder PublishProjectTask { get; set; }
    public CakeTaskBuilder RestoreNpmPackagesTask { get; set; }
    public CakeTaskBuilder RestoreNuGetPackagesTask { get; set; }
    public CakeTaskBuilder RunClientUnitTestsTask { get; set; }
    public CakeTaskBuilder RunPackagesInstallationTask { get; set; }
    public CakeTaskBuilder RunServerUnitTestsTask { get; set; }
    public CakeTaskBuilder SetAssemblyVersionTask { get; set; }
    public CakeTaskBuilder SetPackageJsonVersionTask { get; set; }
    public CakeTaskBuilder SyncAllUnicornItems { get; set; }

    // Task Names
    public string BuildClientCodeTaskName => GetTaskName(this.BuildClientCodeTask);
    public string BuildServerCodeTaskName => GetTaskName(this.BuildServerCodeTask);
    public string CleanArtifactsTaskName => GetTaskName(this.CleanArtifactsTask);
    public string CleanWildcardFoldersTaskName => GetTaskName(this.CleanWildcardFoldersTask);
    public string ConfigureToolsTaskName => GetTaskName(this.ConfigureToolsTask);
    public string CopyShipFilesTaskName => GetTaskName(this.CopyShipFilesTask);
    public string CopySpeRemotingFilesTaskName => GetTaskName(this.CopySpeRemotingFilesTask);
    public string DownloadLicenseFileTaskName => GetTaskName(this.DownloadLicenseFileTask);
    public string GatherBuildConfigsTaskName => GetTaskName(this.GatherBuildConfigsTask);
    public string GatherBuildScriptsTaskName => GetTaskName(this.GatherBuildScriptsTask);
    public string GatherSitecorePackagesTaskName => GetTaskName(this.GatherSitecorePackagesTask);
    public string GenerateCodeTaskName => GetTaskName(this.GenerateCodeTask);
    public string GenerateVersionFileTaskName => GetTaskName(this.GenerateVersionFileTask);
    public string MergeCoverageReportsTaskName => GetTaskName(this.MergeCoverageReportsTask);
    public string OptimizeBuildArtifactsTaskName => GetTaskName(this.OptimizeBuildArtifactsTask);
    public string PrepareWebConfigTaskName => GetTaskName(this.PrepareWebConfigTask);
    public string PublishFeatureTaskName => GetTaskName(this.PublishFeatureTask);
    public string PublishFoundationTaskName => GetTaskName(this.PublishFoundationTask);
    public string PublishProjectTaskName => GetTaskName(this.PublishProjectTask);
    public string RestoreNpmPackagesTaskName => GetTaskName(this.RestoreNpmPackagesTask);
    public string RestoreNuGetPackagesTaskName => GetTaskName(this.RestoreNuGetPackagesTask);
    public string RunClientUnitTestsTaskName => GetTaskName(this.RunClientUnitTestsTask);
    public string RunPackagesInstallationTaskName => GetTaskName(this.RunPackagesInstallationTask);
    public string RunServerUnitTestsTaskName => GetTaskName(this.RunServerUnitTestsTask);
    public string SetAssemblyVersionTaskName => GetTaskName(this.SetAssemblyVersionTask);
    public string SetPackageJsonVersionTaskName => GetTaskName(this.SetPackageJsonVersionTask);
    public string SyncAllUnicornItemsName => GetTaskName(this.SyncAllUnicornItems);

    // private helpers
    private static string GetTaskName(CakeTaskBuilder taskBuilder) {
        if (taskBuilder != null)
        {
            return taskBuilder.Task.Name;
        }

        throw new Exception("Cannot retrieve a name of a task as the task is undefined (null).");
    }
}

public static partial class Sitecore 
{ 
    public static Tasks Tasks = new Tasks();
}
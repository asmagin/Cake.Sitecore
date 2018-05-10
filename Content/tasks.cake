#load "./tasks.clean.cake"
#load "./tasks.prepare.cake"
#load "./tasks.packages.cake"
#load "./tasks.restore.cake"
#load "./tasks.tests.unit.cake"

public class Tasks {
    // Tasks
    public CakeTaskBuilder<ActionTask> BuildClientCodeTask { get; set; }
    public CakeTaskBuilder<ActionTask> BuildServerCodeTask { get; set; }
    public CakeTaskBuilder<ActionTask> CleanArtifactsTask { get; set; }
    public CakeTaskBuilder<ActionTask> CleanWildcardFoldersTask { get; set; }
    public CakeTaskBuilder<ActionTask> ConfigureToolsTask { get; set; }
    public CakeTaskBuilder<ActionTask> CopyShipFilesTask { get; set; }
    public CakeTaskBuilder<ActionTask> CopySpeRemotingFilesTask { get; set; }
    public CakeTaskBuilder<ActionTask> GenerateCodeTask { get; set; }
    public CakeTaskBuilder<ActionTask> GenerateVersionFileTask { get; set; }
    public CakeTaskBuilder<ActionTask> MergeCoverageReportsTask { get; set; }
    public CakeTaskBuilder<ActionTask> PrepareWebConfigTask { get; set; }
    public CakeTaskBuilder<ActionTask> RestoreNpmPackagesTask { get; set; }
    public CakeTaskBuilder<ActionTask> RestoreNuGetPackagesTask { get; set; }
    public CakeTaskBuilder<ActionTask> RunClientUnitTestsTask { get; set; }
    public CakeTaskBuilder<ActionTask> RunPackagesInstallationTask { get; set; }
    public CakeTaskBuilder<ActionTask> RunServerUnitTestsTask { get; set; }
    public CakeTaskBuilder<ActionTask> SetAssemblyVersionTask { get; set; }
    public CakeTaskBuilder<ActionTask> SetPackageJsonVersionTask { get; set; }

    // Task Names
    public string BuildClientCodeTaskName => GetTaskName(this.BuildClientCodeTask);
    public string BuildServerCodeTaskName => GetTaskName(this.BuildServerCodeTask);
    public string CleanArtifactsTaskName => GetTaskName(this.CleanArtifactsTask);
    public string CleanWildcardFoldersTaskName => GetTaskName(this.CleanWildcardFoldersTask);
    public string ConfigureToolsTaskName => GetTaskName(this.ConfigureToolsTask);
    public string CopyShipFilesTaskName => GetTaskName(this.CopyShipFilesTask);
    public string CopySpeRemotingFilesTaskName => GetTaskName(this.CopySpeRemotingFilesTask);
    public string GenerateCodeTaskName => GetTaskName(this.GenerateCodeTask);
    public string GenerateVersionFileTaskName => GetTaskName(this.GenerateVersionFileTask);
    public string MergeCoverageReportsTaskName => GetTaskName(this.MergeCoverageReportsTask);
    public string PrepareWebConfigTaskName => GetTaskName(this.PrepareWebConfigTask);
    public string RestoreNpmPackagesTaskName => GetTaskName(this.RestoreNpmPackagesTask);
    public string RestoreNuGetPackagesTaskName => GetTaskName(this.RestoreNuGetPackagesTask);
    public string RunClientUnitTestsTaskName => GetTaskName(this.RunClientUnitTestsTask);
    public string RunPackagesInstallationTaskName => GetTaskName(this.RunPackagesInstallationTask);
    public string RunServerUnitTestsTaskName => GetTaskName(this.RunServerUnitTestsTask);
    public string SetAssemblyVersionTaskName => GetTaskName(this.SetAssemblyVersionTask);
    public string SetPackageJsonVersionTaskName => GetTaskName(this.SetPackageJsonVersionTask);

    // private helpers
    private static string GetTaskName(CakeTaskBuilder<ActionTask> taskBuilder) {
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
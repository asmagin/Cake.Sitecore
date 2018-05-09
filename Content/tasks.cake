public class Tasks {
    // Tasks
    // 010 prepare
    public CakeTaskBuilder<ActionTask> ConfigureToolsTask { get; set; }
    public CakeTaskBuilder<ActionTask> CleanWildcardFoldersTask { get; set; }
    public CakeTaskBuilder<ActionTask> CleanArtifactsTask { get; set; }
    // 020 restore
    public CakeTaskBuilder<ActionTask> RestoreNuGetPackagesTask { get; set; }
    public CakeTaskBuilder<ActionTask> RestoreNpmPackagesTask { get; set; }

    // Task Names
    public string ConfigureToolsTaskName => GetTaskName(this.ConfigureToolsTask);
    public string CleanWildcardFoldersTaskName => GetTaskName(this.CleanWildcardFoldersTask);
    public string CleanArtifactsTaskName => GetTaskName(this.CleanArtifactsTask);
    public string RestoreNuGetPackagesTaskName => GetTaskName(this.RestoreNuGetPackagesTask);
    public string RestoreNpmPackagesTaskName => GetTaskName(this.RestoreNpmPackagesTask);
    
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
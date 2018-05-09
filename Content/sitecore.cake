#load "./constants.cake"
#load "./git.cake"
#load "./parameters.cake"
#load "./tasks.cake"
#load "./tasks.clean.cake"
#load "./tasks.prepare.cake"
#load "./tasks.restore.cake"
#load "./utils.cake"

Sitecore.Constants.SetNames();
Sitecore.Parameters.InitParams(
    context: Context,
    msBuildToolVersion: MSBuildToolVersion.Default
);

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

// Generic parameters
var BuildConfiguration =            Sitecore.Parameters.BuildConfiguration;
var SolutionName =                  Sitecore.Parameters.SolutionName;

// Sitecore parameters
var ScAdminUser =                   Sitecore.Parameters.ScAdminUser;
var ScAdminPassword =               Sitecore.Parameters.ScAdminPassword; 
var ScNodeEnv =                     Sitecore.Parameters.ScNodeEnv;
var ScNodeRole =                    Sitecore.Parameters.ScNodeRole;
var ScSiteUrl =                     Sitecore.Parameters.ScSiteUrl;

// Versioning
var Version =                       Sitecore.Parameters.Version;
var AssemblyVersion =               Sitecore.Parameters.AssemblyVersion;

// Source Control
var BranchName =                    Sitecore.Parameters.BranchName;
var Commit =                        Sitecore.Parameters.Commit;

var MsBuildToolVersion =            Sitecore.Parameters.MsBuildToolVersion;

var semVer =                        Sitecore.Parameters.Version;
var assemblyVersion =               Sitecore.Parameters.AssemblyVersion;

// Locations
// root should always be one level above the location of build.ps1 or build.cake files
var rootDir =                       Sitecore.Parameters.RootDir;

// Generated
var libsDir =                       Sitecore.Parameters.LibsDir;
var libsPackagesDir =               Sitecore.Parameters.LibsPackagesDir;
var libsNuGetDir =                  Sitecore.Parameters.LibsNuGetDir;
var libsSpeDir =                    Sitecore.Parameters.LibsSpeDir;
var libsShipDir =                   Sitecore.Parameters.LibsShipDir;

var buildDir =                      Sitecore.Parameters.BuildDir;

var srcDir =                        Sitecore.Parameters.SrcDir;
var srcConfigsDir =                 Sitecore.Parameters.SrcConfigsDir;
var srcScriptsDir =                 Sitecore.Parameters.SrcScriptsDir;
var srcScriptsGitDir =              Sitecore.Parameters.SrcScriptsGitDir;

var artifactsDir =                  Sitecore.Parameters.ArtifactsDir;
var artifactsBuildDir =             Sitecore.Parameters.ArtifactsBuildDir;
var artifactsLibsPackagesDir =      Sitecore.Parameters.ArtifactsLibsPackagesDir;
var artifactsSrcDir =               Sitecore.Parameters.ArtifactsSrcDir;
var artifactsSrcConfigsDir =        Sitecore.Parameters.ArtifactsSrcConfigsDir;
var artifactsSrcScriptsDir =        Sitecore.Parameters.ArtifactsSrcScriptsDir;
var artifactsSrcScriptsUnicornDir = Sitecore.Parameters.ArtifactsSrcScriptsUnicornDir;

var outputDir =                     Sitecore.Parameters.OutputDir;
var testsOutputDir =                Sitecore.Parameters.TestsOutputDir;
var testsCoverageOutputDir =        Sitecore.Parameters.TestsCoverageOutputDir;
var xUnitTestsCoverageOutputDir =   Sitecore.Parameters.XUnitTestsCoverageOutputDir;
var jestTestsCoverageOutputDir =    Sitecore.Parameters.JestTestsCoverageOutputDir;

// Pathes
var solutionFilePath =              Sitecore.Parameters.SolutionFilePath;
var unicornConfigPath =             Sitecore.Parameters.UnicornConfigPath;


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var publishingTargetDir =           Sitecore.Parameters.PublishingTargetDir;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Sitecore-Validate")
    .Does(() =>
{
    Verbose("Validating parameters required for Sitecore build");

    // Validating parameters
    Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionName, "SolutionName", "SOLUTION_NAME");
});

// Task("Restore-NuGet-Packages")
//     .IsDependentOn("Clean")
//     .Does(() =>
// {
//     NuGetRestore("./src/Example.sln");
// });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

// Task("Default")
//     .IsDependentOn("Run-Unit-Tests");

// //////////////////////////////////////////////////
// Execution
// //////////////////////////////////////////////////
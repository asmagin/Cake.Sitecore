#load "./constants.cake"
#load "./git.cake"
#load "./utils.cake"
#load "./variables.cake"

Constants.SetNames();
Variables.InitVariables(
    context: Context,
    msBuildToolVersion: MSBuildToolVersion.Default
);

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

// Generic parameters
var BuildConfiguration =            Variables.BuildConfiguration;
var SolutionName =                  Variables.SolutionName;

// Sitecore parameters
var ScAdminUser =                   Variables.ScAdminUser;
var ScAdminPassword =               Variables.ScAdminPassword;
var ScNodeEnv =                     Variables.ScNodeEnv;
var ScNodeRole =                    Variables.ScNodeRole;
var ScSiteUrl =                     Variables.ScSiteUrl;

// Versioning
var Version =                       Variables.Version;
var AssemblyVersion =               Variables.AssemblyVersion;

// Source Control
var BranchName =                    Variables.BranchName;
var Commit =                        Variables.Commit;

var MsBuildToolVersion =            Variables.MsBuildToolVersion;

var semVer =                        Variables.Version;
var assemblyVersion =               Variables.AssemblyVersion;

// Locations
// root should always be one level above the location of build.ps1 or build.cake files
var rootDir =                       Variables.RootDir;

// Generated
var libsDir =                       Variables.LibsDir;
var libsPackagesDir =               Variables.LibsPackagesDir;
var libsNuGetDir =                  Variables.LibsNuGetDir;
var libsSpeDir =                    Variables.LibsSpeDir;
var libsShipDir =                   Variables.LibsShipDir;

var buildDir =                      Variables.BuildDir;

var srcDir =                        Variables.SrcDir;
var srcConfigsDir =                 Variables.SrcConfigsDir;
var srcScriptsDir =                 Variables.SrcScriptsDir;
var srcScriptsGitDir =              Variables.SrcScriptsGitDir;

var artifactsDir =                  Variables.ArtifactsDir;
var artifactsBuildDir =             Variables.ArtifactsBuildDir;
var artifactsLibsPackagesDir =      Variables.ArtifactsLibsPackagesDir;
var artifactsSrcDir =               Variables.ArtifactsSrcDir;
var artifactsSrcConfigsDir =        Variables.ArtifactsSrcConfigsDir;
var artifactsSrcScriptsDir =        Variables.ArtifactsSrcScriptsDir;
var artifactsSrcScriptsUnicornDir = Variables.ArtifactsSrcScriptsUnicornDir;

var outputDir =                     Variables.OutputDir;
var testsOutputDir =                Variables.TestsOutputDir;
var testsCoverageOutputDir =        Variables.TestsCoverageOutputDir;
var xUnitTestsCoverageOutputDir =   Variables.XUnitTestsCoverageOutputDir;
var jestTestsCoverageOutputDir =    Variables.JestTestsCoverageOutputDir;

// Pathes
var solutionFilePath =              Variables.SolutionFilePath;
var unicornConfigPath =             Variables.UnicornConfigPath;


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
// Task("Sitecore-Validate")
//     .Does(() =>
// {
//     Verbose("Validating parameters required for Sitecore build");

//     // Validating parameters
//     Utils.ThrowIfNullOrEmpty("SolutionName", "SOLUTION_NAME");
// });

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
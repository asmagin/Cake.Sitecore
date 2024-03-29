# Cake.Sitecore

[![NuGet version](https://img.shields.io/nuget/v/Cake.Sitecore.svg)](https://www.nuget.org/packages/Cake.Sitecore/)
[![Build status](https://github.com/asmagin/Cake.Sitecore/actions/workflows/release.yaml/badge.svg)](https://github.com/asmagin/Cake.Sitecore/actions/workflows/release.yaml)

## Overview
The library provides a set of pre-build [CAKE build] tasks. Those tasks could be used to simplify a configuration of CI/CD for Helix-based Sitecore projects.

### Rationale 
Cake (C# Make) is a cross-platform build automation system with a C# DSL for tasks such as compiling code, copying files and folders, running unit tests, compressing files and building NuGet packages. 
It should be familiar for a Sitecore developers as script are written in C# and it is possible to use .NET dll and NuGet packages in them.
The library also provides a set of ready tasks that could be configured with environment variables, arguments and inside script, which makes it highly suitable for DevOps. As those tasks are decoupled from your build server - it makes your process highly portable (TeamCity, Jenkins, VSTS or even local machine - doesn't really matter).

## Setup
In order to use the library you need to follow the standard [initial setup from CAKE](https://cakebuild.net/docs/tutorials/getting-started). Once you have 3 required files `build.ps1`, `build.cake` and `tools/packages.json`, open `build.cake` and add following sections there:

``` cs
// //////////////////////////////////////////////////
// Dependencies
// //////////////////////////////////////////////////
#tool nuget:?package=Cake.Sitecore&version=<current>
#load nuget:?package=Cake.Sitecore&version=<current>

// ... other includes 

// //////////////////////////////////////////////////
// Arguments
// //////////////////////////////////////////////////
var Target = ArgumentOrEnvironmentVariable("target", "", "Default");

// //////////////////////////////////////////////////
// Prepare
// //////////////////////////////////////////////////

Sitecore.Constants.SetNames();
Sitecore.Parameters.InitParams(
    context: Context, // Pass CAKE context to a library
    msBuildToolVersion: MSBuildToolVersion.Default, // Select required version
    solutionName: "Habitat", // Name of your solution
    scSiteUrl: "https://sc9.local", // URL of a site
    // ... other parameters
);

// //////////////////////////////////////////////////
// Tasks
// //////////////////////////////////////////////////

Task("Restore")
    // predefined tasks could be used as a dependency for your build step
    .IsDependentOn(Sitecore.Tasks.RestoreNuGetPackagesTask) 
    .IsDependentOn(Sitecore.Tasks.RestoreNpmPackagesTaskName)
    ;

// ... other tasks

// //////////////////////////////////////////////////
// Targets
// //////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Restore")
    // ... other tasks
    ;

// //////////////////////////////////////////////////
// Execution
// //////////////////////////////////////////////////

RunTarget(Target);
```
Full version of this file can be found in `samples` directory.

## Tasks
### Prepare
#### Prepare :: Configure Tools (`Sitecore.Tasks.ConfigureToolsTaskName`)
Executes basic configuration to optimize build performance (e.g. hide progress bars)

### Clean up
#### Clean-up :: Clean Wildcard Folders (`Sitecore.Tasks.CleanWildcardFoldersTaskName`)
Removes items from project `./dist` and `./App_Data` directories in each project.
`App_Data` contains unicorn serialization files that are copied from serialization folder before publish.
`dist` holds client side artifacts, that are generated by webpack and also copied before publish.

#### Clean-up :: Clean Artifacts (`Sitecore.Tasks.CleanArtifactsTaskName`)
Cleans artifacts (`ARTIFACTS_DIR`) and output (`OUTPUT_DIR`) directories

### Restore
#### Restore :: Restore NuGet Packages (`Sitecore.Tasks.RestoreNuGetPackagesTaskName`)
Restore NuGet packages for a solution

#### Restore :: Restore NPM Packages (`Sitecore.Tasks.RestoreNpmPackagesTaskName`)
Restore Npm packages for a solution

### Build
#### Build :: Generate Version.txt file (`Sitecore.Tasks.GenerateVersionFileTaskName`)
Creates a file with detailed information about the build in publishing target directory (`PUBLISHING_TARGET_DIR`). The file includes:
 - current build version (`VERSION`)
 - branch name (`BRANCH_NAME`) that was used to generate it
 - commit SHA (`COMMIT`)
 - build number (`BUILD_NUMBER`)

and a time of its creation in UTC.

#### Build :: Set Version in package.json (`Sitecore.Tasks.SetPackageJsonVersionTaskName`)
Updates version in `packages.json` with current build version (`VERSION`)

#### Build :: Set Version in Assembly.cs files (`Sitecore.Tasks.SetAssemblyVersionTaskName`)
Updates `Assembly.cs` version(`ASSEMBLY_VERSION`)before the build in each project in source directory (`SRC_DIR`).

#### Build :: Download License File (`Sitecore.Tasks.DownloadLicenseFileTaskName`)
Download license file from remote address (`SC_LICENSE_URI`) to the (`ROOT_DIR`).

#### Build :: Generate Code (`Sitecore.Tasks.GenerateCodeTaskName`)
Executes JS plugin to parse Unicorn files via `npm run` and generate code. Script should be called `sc:codegen`.

#### Build :: Build Client Code (`Sitecore.Tasks.BuildClientCodeTaskName`)
Executes front-end code build via calling `npm run build:<your build configuration>`. Build configurations in taken from corresponding parameter (`BUILD_CONFIGURATION`).

#### Build :: Build Server Code (`Sitecore.Tasks.BuildServerCodeTaskName`)
Runs MsBuild for a solution (`SOLUTION_FILE_PATH`) with a specific build configuration (`BUILD_CONFIGURATION`).

### Unit Tests
#### Unit Tests :: Run Server Tests (`Sitecore.Tasks.RunServerUnitTestsTaskName`)
Executes all available tests for server-side code using xUnit. Result will be placed into (`TESTS_OUTPUT_DIR`), also code coverage reports will be created in `cobertura` format in (`XUNIT_TESTS_COVERAGE_OUTPUT_DIR`) directory. 

#### Unit Tests :: Run Client Tests (`Sitecore.Tasks.RunClientUnitTestsTaskName`)
Executes all available tests for client-side code via `npm run test-cover`. Result **should** be placed into (`TESTS_OUTPUT_DIR`) including code coverage reports in `cobertura`. 

#### Unit Tests :: Merge Coverage Reports (`Sitecore.Tasks.MergeCoverageReportsTaskName`)
Merges available code coverage reports produces by previous steps. Generates an `index` file in a coverage directory (`TESTS_COVERAGE_OUTPUT_DIR`).

### Packages
#### Packages :: Copy SPE remoting files (`Sitecore.Tasks.CopySpeRemotingFilesTaskName`)
Copies Sitecore PowerShell Remoting (SPE) plugin assets from (`LIBS_SPE_DIR`) to the publishing target directory (`PUBLISHING_TARGET_DIR`).

#### Packages :: Copy Ship files (`Sitecore.Tasks.CopyShipFilesTaskName`)
Copies Sitecore Ship plugin assets from (`LIBS_SHIP_DIR`) to the publishing target directory (`PUBLISHING_TARGET_DIR`).

#### Packages :: Prepare web.config (`Sitecore.Tasks.PrepareWebConfigTaskName`)
Transforms `web.config` file located in config directory (`SRC_CONFIGS_DIR`) and copy it the publishing target directory (`PUBLISHING_TARGET_DIR`). This is required to make plugins work. (`SC_NODE_ENV`) will define `env:node` for Sitecore layers configurations.

#### Packages :: Install (`Sitecore.Tasks.RunPackagesInstallationTaskName`)
Installs packages from a `libs/packages` directory (`LIBS_PACKAGES_DIR`) according to a node role (`SC_NODE_ROLE`). packages will be delivered via Sitecore Ship (`SC_SITE_URL`).

### Publish
#### Publish :: Foundation (`Sitecore.Tasks.PublishFoundationTaskName`)
Publishes all Foundation-layer projects to the publishing target directory (`PUBLISHING_TARGET_DIR`) using MsBuild.

#### Publish :: Features (`Sitecore.Tasks.PublishFeatureTaskName`)
Publishes all Feature-layer projects to the publishing target directory (`PUBLISHING_TARGET_DIR`) using MsBuild.

#### Publish :: Projects (`Sitecore.Tasks.PublishProjectTaskName`)
Publishes all Project-layer projects to the publishing target directory (`PUBLISHING_TARGET_DIR`) using MsBuild.

### Artifacts
#### Artifacts :: Optimize (`Sitecore.Tasks.OptimizeBuildArtifactsTaskName`)
Exclude unnecessary files from target directory (`ARTIFACTS_BUILD_DIR`).

#### Artifacts :: Copy configuration files (`Sitecore.Tasks.GatherBuildConfigsTaskName`)
Copy configuration files from source config directory (`SRC_CONFIGS_DIR`) to artifact directory (`ARTIFACTS_SRC_CONFIGS_DIR`).

#### Artifacts :: Copy build scripts (`Sitecore.Tasks.GatherBuildScriptsTaskName`)
Copy build scripts from source directory (`SRC_SCRIPTS_DIR`) to artifact directory (`ARTIFACTS_SRC_DIR`).

#### Artifacts :: Copy Sitecore packages (`Sitecore.Tasks.GatherSitecorePackagesTaskName`)
Copy Sitecore packages from source directory (`LIBS_PACKAGES_DIR`) to artifact directory (`ARTIFACTS_LIBS_PACKAGES_DIR`).

### Sync
#### Sync :: Unicorn (`Sitecore.Tasks.SyncAllUnicornItemsName`)
Executes Unicorn content synchronization using (`SC_SITE_URL`). Secret required to authenticate services should be located in a config file (`UNICORN_CONFIG_PATH`). List of configurations can be passed via parameter (`UNICORN_CONFIGURATIONS`)

## Contributing
Any feedback, [issues](https://github.com/asmagin/Cake.Sitecore/issues) or pull requests [pull requests](https://github.com/asmagin/Cake.Sitecore/pulls) are welcome and greatly appreciated.
## Samples
Sample usage of a CAKE-build with Habitat could be found in [here](https://github.com/asmagin/Habitat/tree/cake)  


## Release notes
# v.1.0.15
Sitecore Powershell Extensions updated to version 5.0
"ScAdminUser" parameter default fixed - "admin"
.nuspec version bumped 

# v.1.0.14
Fixed issue with wrong assembly version generation. Removed build number from assemsbly version and make it 0 by default. Otherwise it breaks reference for the nuget packages generated for the same {Major}.{Minor}.{Patch} version

# v.1.0.8
Added new parameter Sitecore.Parameters.TestsFailImmediately with default boolean value = true. 
In case of failed unit tests parameter Sitecore.Parameters.TestsFailImmediately controls if tasks Sitecore.Tasks.RunServerUnitTestsTaskName and Sitecore.Tasks.RunClientUnitTestsTaskName should fail immediately or not. Otherwise if Sitecore.Parameters.TestsFailImmediately = false unit test execution will throw an exception in the task Sitecore.Tasks.MergeCoverageReportsTaskName.
You can pass value to the parameter with argument:
-TESTS_FAIL_IMMEDIATELY=false

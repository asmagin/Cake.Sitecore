# Cake.Sitecore

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
#tool nuget:?package=Cake.Sitecore&prerelease
#load nuget:?package=Cake.Sitecore&prerelease

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

## Samples
Sample usage of a CAKE-build with Habitat could be found in [here](https://github.com/asmagin/Habitat/tree/cake)  

## Contributing
Any feedback, [issues](https://github.com/asmagin/Cake.Sitecore/issues) or pull requests [pull requests](https://github.com/asmagin/Cake.Sitecore/pulls) are welcome and greatly appreciated.

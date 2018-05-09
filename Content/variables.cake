using Cake.Common.Tools.MSBuild;
using System;
using System.Text.RegularExpressions;

public static class Variables
{
    private const string SEM_VER_REGEX = "\\d+\\.\\d+\\.\\d+";

    private static ICakeContext _context;

    public static MSBuildToolVersion MsBuildToolVersion { get; private set; }

    public static string BuildConfiguration { get; private set; }
    public static string SolutionName { get; private set; }

    public static string ScAdminUser { get; private set; }
    public static string ScAdminPassword { get; private set; }
    public static string ScNodeEnv { get; private set; }
    public static string ScNodeRole { get; private set; }
    public static string ScSiteUrl { get; private set; }

    public static string Version { get; private set; }
    public static string AssemblyVersion { get; private set; }

    public static string BranchName { get; private set; }
    public static string Commit { get; private set; }

    public static string BuildId { get; private set; }
    public static string BuildName { get; private set; }

    public static DirectoryPath RootDir { get; private set; }

    public static DirectoryPath LibsDir { get; private set; }
    public static DirectoryPath LibsPackagesDir { get; private set; }
    public static DirectoryPath LibsNuGetDir { get; private set; }
    public static DirectoryPath LibsSpeDir { get; private set; }
    public static DirectoryPath LibsShipDir { get; private set; }
    public static DirectoryPath BuildDir { get; private set; }
    public static DirectoryPath SrcDir { get; private set; }
    public static DirectoryPath SrcConfigsDir { get; private set; }
    public static DirectoryPath SrcScriptsDir { get; private set; }
    public static DirectoryPath SrcScriptsGitDir { get; private set; }
    public static DirectoryPath ArtifactsDir { get; private set; }
    public static DirectoryPath ArtifactsBuildDir { get; private set; }
    public static DirectoryPath ArtifactsLibsPackagesDir { get; private set; }
    public static DirectoryPath ArtifactsSrcDir { get; private set; }
    public static DirectoryPath ArtifactsSrcConfigsDir { get; private set; }
    public static DirectoryPath ArtifactsSrcScriptsDir { get; private set; }
    public static DirectoryPath ArtifactsSrcScriptsUnicornDir { get; private set; }
    public static DirectoryPath OutputDir { get; private set; }
    public static DirectoryPath TestsOutputDir { get; private set; }
    public static DirectoryPath TestsCoverageOutputDir { get; private set; }
    public static DirectoryPath XUnitTestsCoverageOutputDir { get; private set; }
    public static DirectoryPath JestTestsCoverageOutputDir { get; private set; }
    
    public static FilePath SolutionFilePath { get; private set; }
    public static FilePath UnicornConfigPath { get; private set; }

    public static void InitVariables(
        ICakeContext context,
        MSBuildToolVersion msBuildToolVersion)
    {
        _context = context;
        MsBuildToolVersion =    msBuildToolVersion;

        // Generic parameters
        BuildConfiguration =            GetVariableValue(Constants.BUILD_CONFIGURATION, "Debug");
        SolutionName =                  GetVariableValue(Constants.SOLUTION_NAME, "");

        // Sitecore parameters
        ScAdminUser =                   GetVariableValue(Constants.SC_ADMIN_USER, "b");
        ScAdminPassword =               GetVariableValue(Constants.SC_ADMIN_PASSWORD, "b");
        ScNodeEnv =                     GetVariableValue(Constants.SC_NODE_ENV, "local|standalone");
        ScNodeRole =                    GetVariableValue(Constants.SC_NODE_ROLE, "cm");
        ScSiteUrl =                     GetVariableValue(Constants.SC_SITE_URL, "");

        // Source Control
        BranchName =                    GetVariableValue(Constants.BRANCH_NAME, "develop");
        Commit =                        GetVariableValue(Constants.COMMIT, "");

        // Versioning
        Version =                       GetVersion();
        AssemblyVersion =               GetAssemblyVersion();

        //Build Server
        BuildId =                       GetVariableValue(Constants.BUILD_ID, "");
        BuildName =                     GetVariableValue(Constants.BUILD_NAME, "");

        RootDir =                       GetAbsoluteDirPath(GetVariableValue(Constants.ROOT_DIR, "./.."));

        LibsDir =                       GetAbsoluteDirPath(GetVariableValue(Constants.LIBS_DIR,                          $"{RootDir}/libs"));
        LibsPackagesDir =               GetAbsoluteDirPath(GetVariableValue(Constants.LIBS_PACKAGES_DIR,                 $"{LibsDir}/packages"));
        LibsNuGetDir =                  GetAbsoluteDirPath(GetVariableValue(Constants.LIBS_NU_GET_DIR,                   $"{LibsDir}/nuget"));
        LibsSpeDir =                    GetAbsoluteDirPath(GetVariableValue(Constants.LIBS_SPE_DIR,                      $"{LibsDir}/spe"));
        LibsShipDir =                   GetAbsoluteDirPath(GetVariableValue(Constants.LIBS_SHIP_DIR,                     $"{LibsDir}/ship"));
        BuildDir =                      GetAbsoluteDirPath(GetVariableValue(Constants.BUILD_DIR,                         $"{RootDir}/build"));
        SrcDir =                        GetAbsoluteDirPath(GetVariableValue(Constants.SRC_DIR,                           $"{RootDir}/src"));
        SrcConfigsDir =                 GetAbsoluteDirPath(GetVariableValue(Constants.SRC_CONFIGS_DIR,                   $"{SrcDir}/configs"));
        SrcScriptsDir =                 GetAbsoluteDirPath(GetVariableValue(Constants.SRC_SCRIPTS_DIR,                   $"{SrcDir}/scripts"));
        SrcScriptsGitDir =              GetAbsoluteDirPath(GetVariableValue(Constants.SRC_SCRIPTS_GIT_DIR,               $"{SrcScriptsDir}/git"));
        ArtifactsDir =                  GetAbsoluteDirPath(GetVariableValue(Constants.ARTIFACTS_DIR,                     $"{RootDir}/artifacts"));
        ArtifactsBuildDir =             GetAbsoluteDirPath(GetVariableValue(Constants.ARTIFACTS_BUILD_DIR,               $"{ArtifactsDir}/build"));
        ArtifactsLibsPackagesDir =      GetAbsoluteDirPath(GetVariableValue(Constants.ARTIFACTS_LIBS_PACKAGES_DIR,       $"{ArtifactsDir}/libs/packages"));
        ArtifactsSrcDir =               GetAbsoluteDirPath(GetVariableValue(Constants.ARTIFACTS_SRC_DIR,                 $"{ArtifactsDir}/src"));
        ArtifactsSrcConfigsDir =        GetAbsoluteDirPath(GetVariableValue(Constants.ARTIFACTS_SRC_CONFIGS_DIR,         $"{ArtifactsSrcDir}/configs"));
        ArtifactsSrcScriptsDir =        GetAbsoluteDirPath(GetVariableValue(Constants.ARTIFACTS_SRC_SCRIPTS_DIR,         $"{ArtifactsSrcDir}/scripts"));
        ArtifactsSrcScriptsUnicornDir = GetAbsoluteDirPath(GetVariableValue(Constants.ARTIFACTS_SRC_SCRIPTS_UNICORN_DIR, $"{ArtifactsSrcScriptsDir}/unicorn"));
        OutputDir =                     GetAbsoluteDirPath(GetVariableValue(Constants.OUTPUT_DIR,                        $"{RootDir}/output"));
        TestsOutputDir =                GetAbsoluteDirPath(GetVariableValue(Constants.TESTS_OUTPUT_DIR,                  $"{OutputDir}/tests"));
        TestsCoverageOutputDir =        GetAbsoluteDirPath(GetVariableValue(Constants.TESTS_COVERAGE_OUTPUT_DIR,         $"{TestsOutputDir}/coverage"));
        XUnitTestsCoverageOutputDir =   GetAbsoluteDirPath(GetVariableValue(Constants.XUNIT_TESTS_COVERAGE_OUTPUT_DIR,   $"{TestsCoverageOutputDir}/xUnit"));
        JestTestsCoverageOutputDir =    GetAbsoluteDirPath(GetVariableValue(Constants.JEST_TESTS_COVERAGE_OUTPUT_DIR,    $"{TestsCoverageOutputDir}/jest"));

        // Pathes
        SolutionFilePath =              GetAbsoluteFilePath(GetVariableValue(Constants.SOLUTION_FILE_PATH,               $"{SrcDir}/{SolutionName}.sln"));
        UnicornConfigPath =             GetAbsoluteFilePath(GetVariableValue(Constants.UNICORN_CONFIG_PATH,              $"{SrcDir}/Foundation/Serialization/code/App_Config/Include/Unicorn/Unicorn.UI.config"));

    }

    private static string GetVariableValue(string argumentName, string defaultValue, string environmentNamePrefix = null) {
        return Utils.ArgumentOrEnvironmentVariable(_context, argumentName, defaultValue, environmentNamePrefix);
    }

    private static string GetVersion() {
        var version = Utils.ArgumentOrEnvironmentVariable(_context, Constants.VERSION, "0.0.0");

        var regex = new Regex(SEM_VER_REGEX);
        if (!regex.IsMatch(version)){
            throw new Exception($"Environmental variable or argument {Constants.VERSION} = {version} should follow SemVer format (0.0.0).");
        }

        if (!BranchName.IsRelease()) {
            return $"{version}-{Git.GetTagFromBranchName(BranchName)}.{BuildId}";
        }
        else{
            return version;
        }
    }

    private static string GetAssemblyVersion() {
        var version = Utils.ArgumentOrEnvironmentVariable(_context, Constants.VERSION, "0.0.0");

        var regex = new Regex(SEM_VER_REGEX);
        if (!regex.IsMatch(version)){
            throw new Exception($"Environmental variable or argument {Constants.VERSION} = {version} should follow SemVer format (0.0.0).");
        }

        return $"{version}.0";
    }

    private static DirectoryPath GetAbsoluteDirPath(string path){
        return _context.MakeAbsolute(_context.Directory(path)).Collapse().FullPath;
    }

    private static FilePath GetAbsoluteFilePath(string path){
        return _context.MakeAbsolute(_context.File(path)).Collapse().FullPath;
    }
}
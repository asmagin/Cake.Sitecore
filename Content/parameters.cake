using Cake.Common.Tools.MSBuild;
using System;
using System.Text.RegularExpressions;

public static partial class Sitecore 
{
    public static class Parameters
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
        public static string BuildNumber { get; private set; }

        public static string RootDir { get; private set; }

        public static string LibsDir { get; private set; }
        public static string LibsPackagesDir { get; private set; }
        public static string LibsNuGetDir { get; private set; }
        public static string LibsSpeDir { get; private set; }
        public static string LibsShipDir { get; private set; }
        public static string BuildDir { get; private set; }
        public static string SrcDir { get; private set; }
        public static string SrcConfigsDir { get; private set; }
        public static string SrcScriptsDir { get; private set; }
        public static string SrcScriptsGitDir { get; private set; }
        public static string ArtifactsDir { get; private set; }
        public static string ArtifactsBuildDir { get; private set; }
        public static string ArtifactsLibsPackagesDir { get; private set; }
        public static string ArtifactsSrcDir { get; private set; }
        public static string ArtifactsSrcConfigsDir { get; private set; }
        public static string ArtifactsSrcScriptsDir { get; private set; }
        public static string ArtifactsSrcScriptsUnicornDir { get; private set; }
        public static string OutputDir { get; private set; }
        public static string TestsOutputDir { get; private set; }
        public static string TestsCoverageOutputDir { get; private set; }
        public static string XUnitTestsCoverageOutputDir { get; private set; }
        public static string JestTestsCoverageOutputDir { get; private set; }
        public static string PublishingTargetDir { get; private set; }
        public static string ScLocalWebsiteRootDir { get; private set; }

        public static string SolutionFilePath { get; private set; }
        public static string UnicornConfigPath { get; private set; }

        public static void InitParams(
            ICakeContext context,
            MSBuildToolVersion msBuildToolVersion)
        {
            _context = context;
            MsBuildToolVersion =    msBuildToolVersion;

            // Generic parameters
            BuildConfiguration =            GetParameterValue(Constants.BUILD_CONFIGURATION, "Debug");
            SolutionName =                  GetParameterValue(Constants.SOLUTION_NAME, "");
            
            // Sitecore parameters
            ScAdminUser =                   GetParameterValue(Constants.SC_ADMIN_USER, "b");
            ScAdminPassword =               GetParameterValue(Constants.SC_ADMIN_PASSWORD, "b");
            ScNodeEnv =                     GetParameterValue(Constants.SC_NODE_ENV, "local|standalone");
            ScNodeRole =                    GetParameterValue(Constants.SC_NODE_ROLE, "cm");
            ScSiteUrl =                     GetParameterValue(Constants.SC_SITE_URL, "");

            // Source Control
            BranchName =                    GetParameterValue(Constants.BRANCH_NAME, "develop");
            Commit =                        GetParameterValue(Constants.COMMIT, "");

            // Versioning
            Version =                       GetVersion();
            AssemblyVersion =               GetAssemblyVersion();

            //Build Server
            BuildId =                       GetParameterValue(Constants.BUILD_ID, "");
            BuildName =                     GetParameterValue(Constants.BUILD_NAME, "");
            BuildNumber =                   GetParameterValue(Constants.BUILD_NUMBER, "n/a");

            RootDir =                       GetAbsoluteDirPath(GetParameterValue(Constants.ROOT_DIR, "./.."));

            LibsDir =                       GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_DIR,                          $"{RootDir}/libs"));
            LibsPackagesDir =               GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_PACKAGES_DIR,                 $"{LibsDir}/packages"));
            LibsNuGetDir =                  GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_NU_GET_DIR,                   $"{LibsDir}/nuget"));
            LibsSpeDir =                    GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_SPE_DIR,                      $"{LibsDir}/spe"));
            LibsShipDir =                   GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_SHIP_DIR,                     $"{LibsDir}/ship"));
            BuildDir =                      GetAbsoluteDirPath(GetParameterValue(Constants.BUILD_DIR,                         $"{RootDir}/build"));
            SrcDir =                        GetAbsoluteDirPath(GetParameterValue(Constants.SRC_DIR,                           $"{RootDir}/src"));
            SrcConfigsDir =                 GetAbsoluteDirPath(GetParameterValue(Constants.SRC_CONFIGS_DIR,                   $"{SrcDir}/configs"));
            SrcScriptsDir =                 GetAbsoluteDirPath(GetParameterValue(Constants.SRC_SCRIPTS_DIR,                   $"{SrcDir}/scripts"));
            SrcScriptsGitDir =              GetAbsoluteDirPath(GetParameterValue(Constants.SRC_SCRIPTS_GIT_DIR,               $"{SrcScriptsDir}/git"));
            ArtifactsDir =                  GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_DIR,                     $"{RootDir}/artifacts"));
            ArtifactsBuildDir =             GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_BUILD_DIR,               $"{ArtifactsDir}/build"));
            ArtifactsLibsPackagesDir =      GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_LIBS_PACKAGES_DIR,       $"{ArtifactsDir}/libs/packages"));
            ArtifactsSrcDir =               GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_SRC_DIR,                 $"{ArtifactsDir}/src"));
            ArtifactsSrcConfigsDir =        GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_SRC_CONFIGS_DIR,         $"{ArtifactsSrcDir}/configs"));
            ArtifactsSrcScriptsDir =        GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_SRC_SCRIPTS_DIR,         $"{ArtifactsSrcDir}/scripts"));
            ArtifactsSrcScriptsUnicornDir = GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_SRC_SCRIPTS_UNICORN_DIR, $"{ArtifactsSrcScriptsDir}/unicorn"));
            OutputDir =                     GetAbsoluteDirPath(GetParameterValue(Constants.OUTPUT_DIR,                        $"{RootDir}/output"));
            TestsOutputDir =                GetAbsoluteDirPath(GetParameterValue(Constants.TESTS_OUTPUT_DIR,                  $"{OutputDir}/tests"));
            TestsCoverageOutputDir =        GetAbsoluteDirPath(GetParameterValue(Constants.TESTS_COVERAGE_OUTPUT_DIR,         $"{TestsOutputDir}/coverage"));
            XUnitTestsCoverageOutputDir =   GetAbsoluteDirPath(GetParameterValue(Constants.XUNIT_TESTS_COVERAGE_OUTPUT_DIR,   $"{TestsCoverageOutputDir}/xUnit"));
            JestTestsCoverageOutputDir =    GetAbsoluteDirPath(GetParameterValue(Constants.JEST_TESTS_COVERAGE_OUTPUT_DIR,    $"{TestsCoverageOutputDir}/jest"));
            PublishingTargetDir =           GetPublishingTargetDir();

            // Pathes
            SolutionFilePath =              GetAbsoluteFilePath(GetParameterValue(Constants.SOLUTION_FILE_PATH, $"{SrcDir}/{SolutionName}.sln"));
            UnicornConfigPath =             GetUnicornConfigPath();

            // Those parameters absolutely needed 
            Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionName, "SolutionName", "SOLUTION_NAME");
        }

        private static string GetParameterValue(string argumentName, string defaultValue, string environmentNamePrefix = null) {
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

        private static string GetAbsoluteDirPath(string path){
            return _context
                .MakeAbsolute(_context.Directory(path)).Collapse().FullPath.ToString();
        }

        private static string GetAbsoluteFilePath(string path){
            return _context.MakeAbsolute(_context.File(path)).Collapse().FullPath.ToString();
        }

        // if parameter not passed via env or args default values would be provided
        // "Debug" is assumed as a build configuration for local dev installation. 
        // In this case, unicorn configuration could be found in src
        // Otherwise - in artifacts folder.   
        private static string GetUnicornConfigPath(){
            var path = GetParameterValue(Constants.UNICORN_CONFIG_PATH, null);

            if (string.IsNullOrEmpty(path)) {
                path =  BuildConfiguration == "Debug"
                    ? $"{BuildDir}/App_Config/Include/Unicorn/Unicorn.UI.config" // TODO: check if this is correct location
                    : $"{SrcDir}/Foundation/Serialization/code/App_Config/Include/Unicorn/Unicorn.UI.config";
            }

            return GetAbsoluteFilePath(path);
        }

        private static string GetPublishingTargetDir(){
            var path = GetParameterValue(Constants.PUBLISHING_TARGET_DIR, null);
            var localWebRoot = GetParameterValue(Constants.SC_LOCAL_WEBSITE_ROOT_DIR, "\\\\192.168.50.4\\c$\\inetpub\\wwwroot\\sc9.local");

            if (string.IsNullOrEmpty(path)) {
                path = BuildConfiguration == "Debug"
                    ? localWebRoot
                    : ArtifactsBuildDir;
            }
            
            return GetAbsoluteFilePath(path);
        }   
    }
}
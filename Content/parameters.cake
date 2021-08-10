using Cake.Common.Tools.MSBuild;
using System;
using System.Text.RegularExpressions;

public static partial class Sitecore 
{
    public static partial class Parameters
    {
        private const string SEM_VER_REGEX = "\\d+\\.\\d+\\.\\d+";

        private static ICakeContext _context;

        public static MSBuildToolVersion MsBuildToolVersion { get; private set; }

        public static string BuildConfiguration { get; private set; }
        public static string SolutionName { get; private set; }
        public static string XUnitTestsRunInParallel { get; private set; }
        public static bool   SupportHelix20 { get; private set; }

        public static string ScAdminUser { get; private set; }
        public static string ScAdminPassword { get; private set; }
        public static string ScBasicAuth { get; private set; }
        public static string ScNodeEnv { get; private set; }
        public static string ScNodeRole { get; private set; }
        public static string ScSiteUrl { get; private set; }
        public static string ScLicenseUri { get; private set; }
        public static string ScLicenseToken { get; private set; }

        public static string ReleaseVersion { get; private set; }
        public static string AssemblyVersion { get; private set; }

        public static string Branch { get; private set; }
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
        public static string SrcConfigFiles { get; private set; }
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
        public static bool   TestsFailImmediately { get; private set; }
        public static string XUnitTestsCoverageOutputDir { get; private set; }
        public static string XUnitTestsCoverageRegister { get; private set; }
        public static string XUnitTestsCoverageExcludeAttributeFilters { get; private set; }
        public static string XUnitTestsCoverageExcludeFileFilters { get; private set; }
        public static string XUnitTestsCoverageExcludeDirectories { get; private set; }
        public static bool   XUnitShadowCopy { get; private set; }
        public static string JestTestsCoverageOutputDir { get; private set; }
        public static bool   PublishSerializationItems { get; private set; }
        public static string PublishingTargetDir { get; private set; }
        public static string ScLocalWebsiteRootDir { get; private set; }

        public static string NuGetConfigPath { get; private set; }
        public static string SolutionFilePath { get; private set; }
        public static string UnicornConfigPath { get; private set; }
        public static string UnicornConfigurations { get; private set; }
        public static string UnicornSecret { get; private set; }
        public static string UnicornSerializationRoot { get; private set; }

        public static void InitParams(
            ICakeContext context,
            MSBuildToolVersion msBuildToolVersion,
            
            // Add support of defining parameters in code
            string buildConfiguration =            null,
            string solutionName =                  null,
            string supportHelix20 =                null,
            string xUnitTestsRunInParallel =       null,

            string scAdminUser =                   null,
            string scAdminPassword =               null,
            string scBasicAuth =                   null,
            string scNodeEnv =                     null,
            string scNodeRole =                    null,
            string scSiteUrl =                     null,
            string scLicenseUri =                  null,
            string scLicenseToken =                null,

            string branch =                        null,
            string branchName =                    null,
            string commit =                        null,

            string releaseVersion =                null,
            string assemblyVersion =               null,

            string buildId =                       null,
            string buildName =                     null,
            string buildNumber =                   null,

            string rootDir =                       null,

            string libsDir =                       null,
            string libsPackagesDir =               null,
            string libsNuGetDir =                  null,
            string libsSpeDir =                    null,
            string libsShipDir =                   null,
            string buildDir =                      null,
            string srcDir =                        null,
            string srcConfigFiles =                 null,
            string srcConfigsDir =                 null,
            string srcScriptsDir =                 null,
            string srcScriptsGitDir =              null,
            string artifactsDir =                  null,
            string artifactsBuildDir =             null,
            string artifactsLibsPackagesDir =      null,
            string artifactsSrcDir =               null,
            string artifactsSrcConfigsDir =        null,
            string artifactsSrcScriptsDir =        null,
            string artifactsSrcScriptsUnicornDir = null,
            string outputDir =                     null,
            string testsOutputDir =                null,
            string testsCoverageOutputDir =        null,
            string testsFailImmediately =          null,
            string xUnitTestsCoverageOutputDir =   null,
            string xUnitTestsCoverageRegister =    null,
            string xUnitTestsCoverageExcludeAttributeFilters = null,
            string xUnitTestsCoverageExcludeFileFilters      = null,
            string xUnitTestsCoverageExcludeDirectories      = null,
            string xUnitShadowCopy =               null,
            string jestTestsCoverageOutputDir =    null,
            string publishSerializationItems =     null,
            string publishingTargetDir =           null,

            string nuGetConfigPath =               null,
            string solutionFilePath =              null,
            string unicornConfigPath =             null,
            string unicornConfigurations =         null,
            string unicornSecret =                 null,
            string unicornSerializationRoot =      null,
            string scLocalWebsiteRootDir =         null
            )
        {
            _context =                      context;
            MsBuildToolVersion =            msBuildToolVersion;

            // Generic parameters
            BuildConfiguration =            GetParameterValue(Constants.BUILD_CONFIGURATION,                                  buildConfiguration ??            "Debug");
            SolutionName =                  GetParameterValue(Constants.SOLUTION_NAME,                                        solutionName ??                  "");
            SupportHelix20 =                ToBoolean(GetParameterValue(Constants.SUPPORT_HELIX_20,                           supportHelix20 ??                "false"));
            XUnitTestsRunInParallel =       GetParameterValue(Constants.XUNIT_TESTS_RUN_IN_PARALLEL,                          xUnitTestsRunInParallel ??       "");
            
            // Sitecore parameters
            ScAdminUser =                   GetParameterValue(Constants.SC_ADMIN_USER,                                        scAdminUser ??                   "admin");
            ScAdminPassword =               GetParameterValue(Constants.SC_ADMIN_PASSWORD,                                    scAdminPassword ??               "b");
            ScBasicAuth =                   GetParameterValue(Constants.SC_BASICAUTH,                                         scBasicAuth ??                   "");
            ScNodeEnv =                     GetParameterValue(Constants.SC_NODE_ENV,                                          scNodeEnv ??                     "local|standalone");
            ScNodeRole =                    GetParameterValue(Constants.SC_NODE_ROLE,                                         scNodeRole ??                    "cm");
            ScSiteUrl =                     GetParameterValue(Constants.SC_SITE_URL,                                          scSiteUrl ??                     "");
            ScLicenseUri =                  GetParameterValue(Constants.SC_LICENSE_URI,                                       scLicenseUri ??                  "https://{sc_license_uri}");
            ScLicenseToken =                GetParameterValue(Constants.SC_LICENSE_TOKEN,                                     scLicenseToken ??                "");

            // Source Control
            Branch =                        GetParameterValue(Constants.BRANCH,                                               branch ??                        "develop");
            BranchName =                    GetParameterValue(Constants.BRANCH_NAME,                                          branchName ??                    "develop");
            Commit =                        GetParameterValue(Constants.COMMIT,                                               commit ??                        "");

            //Build info
            BuildId =                       GetParameterValue(Constants.BUILD_ID,                                             buildId ??                       "0");
            BuildName =                     GetParameterValue(Constants.BUILD_NAME,                                           buildName ??                     "");
            BuildNumber =                   GetParameterValue(Constants.BUILD_NUMBER,                                         buildNumber ??                   "n/a");

            // Versioning
            ReleaseVersion =                GetVersion(                                                                       releaseVersion ??                "0.0.0");
            AssemblyVersion =               GetAssemblyVersion(                                                               assemblyVersion ??               "0.0.0");

            //Build server
            RootDir =                       GetAbsoluteDirPath(GetParameterValue(Constants.ROOT_DIR,                          rootDir ??                       "./.."));

            LibsDir =                       GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_DIR,                          libsDir ??                       $"{RootDir}/libs"));
            LibsPackagesDir =               GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_PACKAGES_DIR,                 libsPackagesDir ??               $"{LibsDir}/packages"));
            LibsNuGetDir =                  GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_NUGET_DIR,                    libsNuGetDir ??                  $"{LibsDir}/nuget"));
            LibsSpeDir =                    GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_SPE_DIR,                      libsSpeDir ??                    $"{LibsDir}/spe"));
            LibsShipDir =                   GetAbsoluteDirPath(GetParameterValue(Constants.LIBS_SHIP_DIR,                     libsShipDir ??                   $"{LibsDir}/ship"));
            BuildDir =                      GetAbsoluteDirPath(GetParameterValue(Constants.BUILD_DIR,                         buildDir ??                      $"{RootDir}/build"));
            SrcDir =                        GetAbsoluteDirPath(GetParameterValue(Constants.SRC_DIR,                           srcDir ??                        $"{RootDir}/src"));
            SrcConfigFiles =                GetParameterValue(Constants.SRC_CONFIG_FILES,                                     srcConfigFiles ??                $"cake.config, Nuget.config");
            SrcConfigsDir =                 GetAbsoluteDirPath(GetParameterValue(Constants.SRC_CONFIGS_DIR,                   srcConfigsDir ??                 $"{SrcDir}/configs"));
            SrcScriptsDir =                 GetAbsoluteDirPath(GetParameterValue(Constants.SRC_SCRIPTS_DIR,                   srcScriptsDir ??                 $"{SrcDir}/scripts"));
            SrcScriptsGitDir =              GetAbsoluteDirPath(GetParameterValue(Constants.SRC_SCRIPTS_GIT_DIR,               srcScriptsGitDir ??              $"{SrcScriptsDir}/git"));
            ArtifactsDir =                  GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_DIR,                     artifactsDir ??                  $"{RootDir}/artifacts"));
            ArtifactsBuildDir =             GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_BUILD_DIR,               artifactsBuildDir ??             $"{ArtifactsDir}/build"));
            ArtifactsLibsPackagesDir =      GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_LIBS_PACKAGES_DIR,       artifactsLibsPackagesDir ??      $"{ArtifactsDir}/libs/packages"));
            ArtifactsSrcDir =               GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_SRC_DIR,                 artifactsSrcDir ??               $"{ArtifactsDir}/src"));
            ArtifactsSrcConfigsDir =        GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_SRC_CONFIGS_DIR,         artifactsSrcConfigsDir ??        $"{ArtifactsSrcDir}/configs"));
            ArtifactsSrcScriptsDir =        GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_SRC_SCRIPTS_DIR,         artifactsSrcScriptsDir ??        $"{ArtifactsSrcDir}/scripts"));
            ArtifactsSrcScriptsUnicornDir = GetAbsoluteDirPath(GetParameterValue(Constants.ARTIFACTS_SRC_SCRIPTS_UNICORN_DIR, artifactsSrcScriptsUnicornDir ?? $"{ArtifactsSrcScriptsDir}/unicorn"));
            OutputDir =                     GetAbsoluteDirPath(GetParameterValue(Constants.OUTPUT_DIR,                        outputDir ??                     $"{RootDir}/output"));
            TestsOutputDir =                GetAbsoluteDirPath(GetParameterValue(Constants.TESTS_OUTPUT_DIR,                  testsOutputDir ??                $"{OutputDir}/tests"));
            TestsCoverageOutputDir =        GetAbsoluteDirPath(GetParameterValue(Constants.TESTS_COVERAGE_OUTPUT_DIR,         testsCoverageOutputDir ??        $"{TestsOutputDir}/coverage"));
            TestsFailImmediately =          ToBoolean(GetParameterValue(Constants.TESTS_FAIL_IMMEDIATELY,                     testsFailImmediately ??          "true"));
            XUnitTestsCoverageOutputDir =   GetAbsoluteDirPath(GetParameterValue(Constants.XUNIT_TESTS_COVERAGE_OUTPUT_DIR,   xUnitTestsCoverageOutputDir ??   $"{TestsCoverageOutputDir}/xUnit"));
            XUnitTestsCoverageRegister =    GetParameterValue(Constants.XUNIT_TESTS_COVERAGE_REGISTER,                        xUnitTestsCoverageRegister ??    $"user");
            XUnitTestsCoverageExcludeAttributeFilters  = GetParameterValue(Constants.XUNIT_TESTS_COVERAGE_EXCLUDE_ATTRIBUTE_FILTERS,  xUnitTestsCoverageExcludeAttributeFilters  ?? "");
            XUnitTestsCoverageExcludeFileFilters       = GetParameterValue(Constants.XUNIT_TESTS_COVERAGE_EXCLUDE_FILE_FILTERS,       xUnitTestsCoverageExcludeFileFilters       ?? "");
            XUnitTestsCoverageExcludeDirectories       = GetParameterValue(Constants.XUNIT_TESTS_COVERAGE_EXCLUDE_DIRECTORIES,        xUnitTestsCoverageExcludeDirectories       ?? "");
            XUnitShadowCopy =               ToBoolean(GetParameterValue(Constants.XUNIT_SHADOW_COPY,                          xUnitShadowCopy ??               "true"));
            JestTestsCoverageOutputDir =    GetAbsoluteDirPath(GetParameterValue(Constants.JEST_TESTS_COVERAGE_OUTPUT_DIR,    jestTestsCoverageOutputDir ??    $"{TestsCoverageOutputDir}/jest"));

            // Pathes
            NuGetConfigPath =               GetAbsoluteFilePath(GetParameterValue(Constants.NUGET_CONFIG_PATH,                nuGetConfigPath ??               $"{SrcDir}/nuget.config"));
            SolutionFilePath =              GetAbsoluteFilePath(GetParameterValue(Constants.SOLUTION_FILE_PATH,               solutionFilePath ??              $"{SrcDir}/{SolutionName}.sln"));
            UnicornConfigPath =             GetUnicornConfigPath(GetParameterValue(Constants.UNICORN_CONFIG_PATH,             unicornConfigPath ??             ""));
            UnicornConfigurations =         GetParameterValue(Constants.UNICORN_CONFIGURATIONS,                               unicornConfigurations ??         "");
            UnicornSecret =                 GetParameterValue(Constants.UNICORN_SECRET,                                       unicornSecret ??                 "");
            UnicornSerializationRoot =      GetParameterValue(Constants.UNICORN_SERIALIZATION_ROOT,                           unicornSerializationRoot ??      "unicorn");
            PublishSerializationItems =     ToBoolean(GetParameterValue(Constants.PUBLISH_SERIALIZATION_ITEMS,                publishSerializationItems ??     (BuildConfiguration != "Debug").ToString()));
            ScLocalWebsiteRootDir =         GetParameterValue(Constants.SC_LOCAL_WEBSITE_ROOT_DIR,                            scLocalWebsiteRootDir ??         "\\\\192.168.50.4\\c$\\inetpub\\wwwroot\\sc9.local");
            PublishingTargetDir =           GetPublishingTargetDir(                                                           publishingTargetDir);

            // Those parameters absolutely needed 
            Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionName, "SolutionName", "SOLUTION_NAME");
        }

        private static string GetParameterValue(string argumentName, string defaultValue, string environmentNamePrefix = null) {
            return Utils.ArgumentOrEnvironmentVariable(_context, argumentName, defaultValue, environmentNamePrefix);
        }

        private static string GetVersion(string defaultValue) {
            var version = Utils.ArgumentOrEnvironmentVariable(_context, Constants.RELEASE_VERSION, defaultValue);
  
            var regex = new Regex(SEM_VER_REGEX);
            if (!regex.IsMatch(version)){
                throw new Exception($"Environmental variable or argument {Constants.RELEASE_VERSION} = {version} should follow SemVer format (0.0.0).");
            }

            if (!BranchName.IsRelease()) {
                return $"{version}-{Git.GetTagFromBranchName(BranchName)}.{BuildId}";
            }
            else{
                return version;
            }
        }

        private static string GetAssemblyVersion(string defaultValue) {
            var version = Utils.ArgumentOrEnvironmentVariable(_context, Constants.RELEASE_VERSION, defaultValue);

            var regex = new Regex(SEM_VER_REGEX);
            if (!regex.IsMatch(version)){
                throw new Exception($"Environmental variable or argument {Constants.RELEASE_VERSION} = {version} should follow SemVer format (0.0.0).");
            }

            return $"{version}.0"; //made BuildNumber = 0. Otherwise assembly version contains reference on the buildNumber and this could break reference
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
        // In case of "Release" unicorn configuration can be found in artifacts build folder.
        private static string GetUnicornConfigPath(string path) {
            if (string.IsNullOrEmpty(path)) {
                var _basePath = BuildConfiguration == "Debug"
                    ? (SupportHelix20 
                        ? $"{SrcDir}/Foundation/Serialization/website/App_Config/Include/Unicorn"
                        : $"{SrcDir}/Foundation/Serialization/code/App_Config/Include/Unicorn" )
                    : $"{BuildDir}/App_Config/Include/Unicorn";

                path = $"{_basePath}/Unicorn.zSharedSecret.config";

                if (!_context.FileExists(path)) {
                    path = $"{_basePath}/Unicorn.UI.config";
                }
            }

            return GetAbsoluteFilePath(path);
        }

        private static string GetPublishingTargetDir(string defaultValue){
            var path = GetParameterValue(Constants.PUBLISHING_TARGET_DIR, defaultValue);

            if (string.IsNullOrEmpty(path)) {
                path = BuildConfiguration == "Debug"
                    ? ScLocalWebsiteRootDir
                    : ArtifactsBuildDir;
            }

            _context.Information($"Publishing target dir: {path}");
            return GetAbsoluteFilePath(path);
        }

        private static bool ToBoolean(string defaultValue) {
            bool result = false;

            if (bool.TryParse(defaultValue, out result))
            {
                return result;
            }
            
            return false;
        }

    }
}
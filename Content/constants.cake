public static partial class Sitecore
{
    public static partial class Constants
    {
        // Generic parameters
        public static string BUILD_CONFIGURATION { get; private set; }
        public static string SOLUTION_NAME       { get; private set; }
        public static string SUPPORT_HELIX_20    { get; private set; }

        public static string XUNIT_TESTS_RUN_IN_PARALLEL    { get; private set; }
        // Sitecore parameters
        public static string SC_ADMIN_USER       { get; private set; }
        public static string SC_ADMIN_PASSWORD   { get; private set; }
        public static string SC_BASICAUTH        { get; private set; }
        public static string SC_NODE_ENV         { get; private set; }
        public static string SC_NODE_ROLE        { get; private set; }
        public static string SC_SITE_URL         { get; private set; }
        public static string SC_LICENSE_URI      { get; private set; }
        public static string SC_LICENSE_TOKEN    { get; private set; }
        // Versioning
        public static string RELEASE_VERSION     { get; private set; }
        public static string ASSEMBLY_VERSION    { get; private set; }
        // Source Control
        public static string BRANCH              { get; private set; }
        public static string BRANCH_NAME         { get; private set; }
        public static string COMMIT              { get; private set; }
        // Build Server
        public static string BUILD_ID            { get; private set; }
        public static string BUILD_NAME          { get; private set; }
        public static string BUILD_NUMBER        { get; private set; }

        public static string ROOT_DIR            { get; private set; }

        public static string LIBS_DIR                          { get; private set; }
        public static string LIBS_PACKAGES_DIR                 { get; private set; }
        public static string LIBS_NUGET_DIR                    { get; private set; }
        public static string LIBS_SPE_DIR                      { get; private set; }
        public static string LIBS_SHIP_DIR                     { get; private set; }
        public static string BUILD_DIR                         { get; private set; }
        public static string SRC_DIR                           { get; private set; }
        public static string SRC_CONFIG_FILES                  { get; private set; }
        public static string SRC_CONFIGS_DIR                   { get; private set; }
        public static string SRC_SCRIPTS_DIR                   { get; private set; }
        public static string SRC_SCRIPTS_GIT_DIR               { get; private set; }
        public static string ARTIFACTS_DIR                     { get; private set; }
        public static string ARTIFACTS_BUILD_DIR               { get; private set; }
        public static string ARTIFACTS_LIBS_PACKAGES_DIR       { get; private set; }
        public static string ARTIFACTS_SRC_DIR                 { get; private set; }
        public static string ARTIFACTS_SRC_CONFIGS_DIR         { get; private set; }
        public static string ARTIFACTS_SRC_SCRIPTS_DIR         { get; private set; }
        public static string ARTIFACTS_SRC_SCRIPTS_UNICORN_DIR { get; private set; }
        public static string OUTPUT_DIR                        { get; private set; }
        public static string TESTS_OUTPUT_DIR                  { get; private set; }
        public static string TESTS_COVERAGE_OUTPUT_DIR         { get; private set; }
        public static string TESTS_FAIL_IMMEDIATELY            { get; private set; }
        public static string XUNIT_TESTS_COVERAGE_OUTPUT_DIR   { get; private set; }
        public static string XUNIT_TESTS_COVERAGE_REGISTER     { get; private set; }
        public static string XUNIT_TESTS_COVERAGE_EXCLUDE_ATTRIBUTE_FILTERS { get; private set; }
        public static string XUNIT_TESTS_COVERAGE_EXCLUDE_FILE_FILTERS      { get; private set; }
        public static string XUNIT_TESTS_COVERAGE_EXCLUDE_DIRECTORIES       { get; private set; }
        public static string XUNIT_SHADOW_COPY         { get; private set; }
        public static string JEST_TESTS_COVERAGE_OUTPUT_DIR    { get; private set; }
        public static string PUBLISH_SERIALIZATION_ITEMS       { get; private set; }
        public static string PUBLISHING_TARGET_DIR             { get; private set; }
        public static string SC_LOCAL_WEBSITE_ROOT_DIR         { get; private set; }

        public static string NUGET_CONFIG_PATH                 { get; private set; }
        public static string SOLUTION_FILE_PATH                { get; private set; }
        public static string UNICORN_CONFIG_PATH               { get; private set; }
        public static string UNICORN_CONFIGURATIONS            { get; private set; }
        public static string UNICORN_SECRET                    { get; private set; }
        public static string UNICORN_SERIALIZATION_ROOT        { get; private set; }

        public static void SetNames(
            string BuildConfiguration            = null,
            string SolutionName                  = null,
            string SupportHelix20                = null,
            string xUnitTestsRunInParallel       = null,
            string ScAdminUser                   = null,
            string ScAdminPassword               = null,
            string ScBasicAuth                   = null,
            string ScNodeEnv                     = null,
            string ScNodeRole                    = null,
            string ScSiteUrl                     = null,
            string ScLicenseUri                  = null,
            string ScLicenseToken                = null,
            string ReleaseVersion                = null,
            string AssemblyVersion               = null,
            string Branch                        = null,
            string BranchName                    = null,
            string Commit                        = null,
            string BuildId                       = null,
            string BuildName                     = null,
            string BuildNumber                   = null,
            string RootDir                       = null,
            string LibsDir                       = null,
            string LibsPackagesDir               = null,
            string LibsNuGetDir                  = null,
            string LibsSpeDir                    = null,
            string LibsShipDir                   = null,
            string BuildDir                      = null,
            string SrcDir                        = null,
            string SrcConfigFiles                = null,
            string SrcConfigsDir                 = null,
            string SrcScriptsDir                 = null,
            string SrcScriptsGitDir              = null,
            string ArtifactsDir                  = null,
            string ArtifactsBuildDir             = null,
            string ArtifactsLibsPackagesDir      = null,
            string ArtifactsSrcDir               = null,
            string ArtifactsSrcConfigsDir        = null,
            string ArtifactsSrcScriptsDir        = null,
            string ArtifactsSrcScriptsUnicornDir = null,
            string OutputDir                     = null,
            string TestsOutputDir                = null,
            string TestsCoverageOutputDir        = null,
            string TestsFailImmediately          = null,
            string XunitTestsCoverageOutputDir   = null,
            string XunitTestsCoverageRegister    = null,
            string XunitTestsCoverageExcludeAttributeFilters = null,
            string XunitTestsCoverageExcludeFileFilters      = null,
            string XunitTestsCoverageExcludeDirectories      = null,
            string XunitShadowCopy               = null,
            string JestTestsCoverageOutputDir    = null,
            string PublishSerializationItems     = null,
            string PublishingTargetDir           = null,
            string ScLocalWebsiteRootDir         = null,

            string NuGetConfigPath               = null,
            string SolutionFilePath              = null,
            string UnicornConfigPath             = null,
            string UnicornConfigurations         = null,
            string UnicornSecret                 = null,
            string UnicornSerializationRoot      = null

            )
        {
            BUILD_CONFIGURATION               = BuildConfiguration            ?? "BUILD_CONFIGURATION";
            SOLUTION_NAME                     = SolutionName                  ?? "SOLUTION_NAME";
            SUPPORT_HELIX_20                  = SupportHelix20                ?? "SUPPORT_HELIX_20";
            XUNIT_TESTS_RUN_IN_PARALLEL       = xUnitTestsRunInParallel       ?? "XUNIT_TESTS_RUN_IN_PARALLEL";
            SC_ADMIN_USER                     = ScAdminUser                   ?? "SC_ADMIN_USER";
            SC_ADMIN_PASSWORD                 = ScAdminPassword               ?? "SC_ADMIN_PASSWORD";
            SC_BASICAUTH                      = ScBasicAuth                   ?? "SC_BASICAUTH";
            SC_NODE_ENV                       = ScNodeEnv                     ?? "SC_NODE_ENV";
            SC_NODE_ROLE                      = ScNodeRole                    ?? "SC_NODE_ROLE";
            SC_SITE_URL                       = ScSiteUrl                     ?? "SC_SITE_URL";
            SC_LICENSE_URI                    = ScLicenseUri                  ?? "SC_LICENSE_URI";
            SC_LICENSE_TOKEN                  = ScLicenseToken                ?? "SC_LICENSE_TOKEN";
            RELEASE_VERSION                   = ReleaseVersion                ?? "RELEASE_VERSION";
            ASSEMBLY_VERSION                  = AssemblyVersion               ?? "ASSEMBLY_VERSION";
            BRANCH                            = Branch                        ?? "BRANCH";
            BRANCH_NAME                       = BranchName                    ?? "BRANCH_NAME";
            COMMIT                            = Commit                        ?? "COMMIT";
            BUILD_ID                          = BuildId                       ?? "BUILD_ID";
            BUILD_NAME                        = BuildName                     ?? "BUILD_NAME";
            BUILD_NUMBER                      = BuildNumber                   ?? "BUILD_NUMBER";

            ROOT_DIR                          = RootDir                       ?? "ROOT_DIR";

            LIBS_DIR                          = LibsDir                       ?? "LIBS_DIR"; 
            LIBS_PACKAGES_DIR                 = LibsPackagesDir               ?? "LIBS_PACKAGES_DIR"; 
            LIBS_NUGET_DIR                    = LibsNuGetDir                  ?? "LIBS_NUGET_DIR";
            LIBS_SPE_DIR                      = LibsSpeDir                    ?? "LIBS_SPE_DIR"; 
            LIBS_SHIP_DIR                     = LibsShipDir                   ?? "LIBS_SHIP_DIR"; 
            BUILD_DIR                         = BuildDir                      ?? "BUILD_DIR"; 
            SRC_DIR                           = SrcDir                        ?? "SRC_DIR"; 
            SRC_CONFIG_FILES                  = SrcConfigFiles                ?? "SRC_CONFIG_FILES"; 
            SRC_CONFIGS_DIR                   = SrcConfigsDir                 ?? "SRC_CONFIGS_DIR"; 
            SRC_SCRIPTS_DIR                   = SrcScriptsDir                 ?? "SRC_SCRIPTS_DIR"; 
            SRC_SCRIPTS_GIT_DIR               = SrcScriptsGitDir              ?? "SRC_SCRIPTS_GIT_DIR"; 
            ARTIFACTS_DIR                     = ArtifactsDir                  ?? "ARTIFACTS_DIR"; 
            ARTIFACTS_BUILD_DIR               = ArtifactsBuildDir             ?? "ARTIFACTS_BUILD_DIR"; 
            ARTIFACTS_LIBS_PACKAGES_DIR       = ArtifactsLibsPackagesDir      ?? "ARTIFACTS_LIBS_PACKAGES_DIR"; 
            ARTIFACTS_SRC_DIR                 = ArtifactsSrcDir               ?? "ARTIFACTS_SRC_DIR"; 
            ARTIFACTS_SRC_CONFIGS_DIR         = ArtifactsSrcConfigsDir        ?? "ARTIFACTS_SRC_CONFIGS_DIR"; 
            ARTIFACTS_SRC_SCRIPTS_DIR         = ArtifactsSrcScriptsDir        ?? "ARTIFACTS_SRC_SCRIPTS_DIR"; 
            ARTIFACTS_SRC_SCRIPTS_UNICORN_DIR = ArtifactsSrcScriptsUnicornDir ?? "ARTIFACTS_SRC_SCRIPTS_UNICORN_DIR"; 
            OUTPUT_DIR                        = OutputDir                     ?? "OUTPUT_DIR"; 
            TESTS_OUTPUT_DIR                  = TestsOutputDir                ?? "TESTS_OUTPUT_DIR"; 
            TESTS_COVERAGE_OUTPUT_DIR         = TestsCoverageOutputDir        ?? "TESTS_COVERAGE_OUTPUT_DIR"; 
            TESTS_FAIL_IMMEDIATELY            = TestsFailImmediately          ?? "TESTS_FAIL_IMMEDIATELY"; 
            XUNIT_TESTS_COVERAGE_OUTPUT_DIR   = XunitTestsCoverageOutputDir   ?? "XUNIT_TESTS_COVERAGE_OUTPUT_DIR";
            XUNIT_TESTS_COVERAGE_REGISTER     = XunitTestsCoverageRegister    ?? "XUNIT_TESTS_COVERAGE_REGISTER";
            XUNIT_TESTS_COVERAGE_EXCLUDE_ATTRIBUTE_FILTERS = XunitTestsCoverageExcludeAttributeFilters ?? "XUNIT_TESTS_COVERAGE_EXCLUDE_ATTRIBUTE_FILTERS";
            XUNIT_TESTS_COVERAGE_EXCLUDE_FILE_FILTERS      = XunitTestsCoverageExcludeFileFilters      ?? "XUNIT_TESTS_COVERAGE_EXCLUDE_FILE_FILTERS";
            XUNIT_TESTS_COVERAGE_EXCLUDE_DIRECTORIES       = XunitTestsCoverageExcludeDirectories      ?? "XUNIT_TESTS_COVERAGE_EXCLUDE_DIRECTORIES";
            XUNIT_SHADOW_COPY                 = XunitShadowCopy               ?? "XUNIT_SHADOW_COPY";
            JEST_TESTS_COVERAGE_OUTPUT_DIR    = JestTestsCoverageOutputDir    ?? "JEST_TESTS_COVERAGE_OUTPUT_DIR";
            PUBLISH_SERIALIZATION_ITEMS       = PublishSerializationItems     ?? "PUBLISH_SERIALIZATION_ITEMS";
            PUBLISHING_TARGET_DIR             = PublishingTargetDir           ?? "PUBLISHING_TARGET_DIR";
            SC_LOCAL_WEBSITE_ROOT_DIR         = ScLocalWebsiteRootDir         ?? "SC_LOCAL_WEBSITE_ROOT_DIR";

            NUGET_CONFIG_PATH                 = NuGetConfigPath               ?? "NUGET_CONFIG_PATH";
            SOLUTION_FILE_PATH                = SolutionFilePath              ?? "SOLUTION_FILE_PATH"; 
            UNICORN_CONFIG_PATH               = UnicornConfigPath             ?? "UNICORN_CONFIG_PATH";
            UNICORN_CONFIGURATIONS            = UnicornConfigurations         ?? "UNICORN_CONFIGURATIONS";
            UNICORN_SECRET                    = UnicornSecret                 ?? "UNICORN_SECRET";
            UNICORN_SERIALIZATION_ROOT        = UnicornSerializationRoot      ?? "UNICORN_SERIALIZATION_ROOT";
        }
    }
}
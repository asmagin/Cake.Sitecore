public static partial class Sitecore
{
    public static partial class Constants
    {
        public static partial class Commerce
        {
            public static string BUILD_CONFIGURATION                { get; private set; }
            public static string NUGET_CONFIG_PATH                  { get; private set; }
            public static string LIBS_NUGET_DIR                     { get; private set; }

            public static string ENGINE_PROJECT_PATH                { get; private set; }
            public static string SOLUTION_NAME                      { get; private set; }
            public static string SOLUTION_FILE_PATH                 { get; private set; }

            public static string TESTS_OUTPUT_DIR                               { get; private set; }
            public static string TESTS_COVERAGE_OUTPUT_DIR                      { get; private set; }
            public static string XUNIT_TESTS_COVERAGE_OUTPUT_DIR                { get; private set; }
            public static string XUNIT_TESTS_COVERAGE_REGISTER                  { get; private set; }
            public static string XUNIT_TESTS_COVERAGE_EXCLUDE_ATTRIBUTE_FILTERS { get; private set; }
            public static string XUNIT_TESTS_COVERAGE_EXCLUDE_FILE_FILTERS      { get; private set; }
            public static string XUNIT_TESTS_COVERAGE_EXCLUDE_DIRECTORIES       { get; private set; }

            public static string ARTIFACTS_BUILD_DIR                { get; private set; }
            public static string BASE_LOCAL_WEBSITE_ROOT_DIR        { get; private set; }
            public static string AUTHORING_LOCAL_WEBSITE_ROOT_DIR   { get; private set; }
            public static string MINIONS_LOCAL_WEBSITE_ROOT_DIR     { get; private set; }
            public static string OPS_LOCAL_WEBSITE_ROOT_DIR         { get; private set; }
            public static string SHOPS_LOCAL_WEBSITE_ROOT_DIR       { get; private set; }

            public static string OPS_SERVER_URL                     { get; private set; }

            public static string IDENTITY_SERVER_URL                { get; private set; }
            public static string SC_ADMIN_USER                      { get; private set; }
            public static string SC_ADMIN_PASSWORD                  { get; private set; }
 
            public static void SetNames(
                string BuildConfiguration               = null,
                string NuGetConfigPath                  = null,
                string LibsNuGetDir                     = null,
                string EngineProjectPath                = null,
                string SolutionName                     = null,
                string SolutionFilePath                 = null,
                string TestsOutputDir                               = null,
                string TestsCoverageOutputDir                       = null,
                string XUnitTestsCoverageOutputDir                  = null,
                string XUnitTestsCoverageRegister                   = null,
                string XUnitTestsCoverageExcludeAttributeFilters    = null,
                string XUnitTestsCoverageExcludeFileFilters         = null,
                string XUnitTestsCoverageExcludeDirectories         = null,
                string ArtifactsBuildDir                = null,
                string BaseLocalWebsiteRootDir          = null,
                string AuthoringLocalWebsiteRootDir     = null,
                string MinionsLocalWebsiteRootDir       = null,
                string OpsLocalWebsiteRootDir           = null,
                string ShopsLocalWebsiteRootDir         = null,
                string OpsServerUrl                     = null,
                string IdentityServerUrl                = null,
                string ScAdminUser                      = null,
                string ScAdminPassword                  = null
            ) {
                BUILD_CONFIGURATION                 = BuildConfiguration                ?? "COMMERCE_BUILD_CONFIGURATION";
                NUGET_CONFIG_PATH                   = NuGetConfigPath                   ?? "COMMERCE_NUGET_CONFIG_PATH";
                LIBS_NUGET_DIR                      = LibsNuGetDir                      ?? "COMMERCE_LIBS_NUGET_DIR";
                ENGINE_PROJECT_PATH                 = EngineProjectPath                 ?? "COMMERCE_ENGINE_PROJECT_PATH";
                SOLUTION_NAME                       = SolutionName                      ?? "COMMERCE_SOLUTION_NAME";
                SOLUTION_FILE_PATH                  = SolutionFilePath                  ?? "COMMERCE_SOLUTION_FILE_PATH";

                TESTS_OUTPUT_DIR                                = TestsOutputDir                            ?? "COMMERCE_TESTS_OUTPUT_DIR";        
                TESTS_COVERAGE_OUTPUT_DIR                       = TestsCoverageOutputDir                    ?? "COMMERCE_TESTS_COVERAGE_OUTPUT_DIR";
                XUNIT_TESTS_COVERAGE_OUTPUT_DIR                 = XUnitTestsCoverageOutputDir               ?? "COMMERCE_XUNIT_TESTS_COVERAGE_OUTPUT_DIR";
                XUNIT_TESTS_COVERAGE_REGISTER                   = XUnitTestsCoverageRegister                ?? "COMMERCE_XUNIT_TESTS_COVERAGE_REGISTER";
                XUNIT_TESTS_COVERAGE_EXCLUDE_ATTRIBUTE_FILTERS  = XUnitTestsCoverageExcludeAttributeFilters ?? "COMMERCE_XUNIT_TESTS_COVERAGE_EXCLUDE_ATTRIBUTE_FILTERS";
                XUNIT_TESTS_COVERAGE_EXCLUDE_FILE_FILTERS       = XUnitTestsCoverageExcludeFileFilters      ?? "COMMERCE_XUNIT_TESTS_COVERAGE_EXCLUDE_FILE_FILTERS";
                XUNIT_TESTS_COVERAGE_EXCLUDE_DIRECTORIES        = XUnitTestsCoverageExcludeDirectories      ?? "COMMERCE_XUNIT_TESTS_COVERAGE_EXCLUDE_DIRECTORIES";

                ARTIFACTS_BUILD_DIR                 = ArtifactsBuildDir                 ?? "COMMERCE_ARTIFACTS_BUILD_DIR";
                BASE_LOCAL_WEBSITE_ROOT_DIR         = BaseLocalWebsiteRootDir           ?? "COMMERCE_BASE_LOCAL_WEBSITE_ROOT_DIR";
                AUTHORING_LOCAL_WEBSITE_ROOT_DIR    = AuthoringLocalWebsiteRootDir      ?? "COMMERCE_AUTHORING_LOCAL_WEBSITE_ROOT_DIR";
                MINIONS_LOCAL_WEBSITE_ROOT_DIR      = MinionsLocalWebsiteRootDir        ?? "COMMERCE_MINIONS_LOCAL_WEBSITE_ROOT_DIR";
                OPS_LOCAL_WEBSITE_ROOT_DIR          = OpsLocalWebsiteRootDir            ?? "COMMERCE_OPS_LOCAL_WEBSITE_ROOT_DIR";
                SHOPS_LOCAL_WEBSITE_ROOT_DIR        = ShopsLocalWebsiteRootDir          ?? "COMMERCE_SHOPS_LOCAL_WEBSITE_ROOT_DIR";
                OPS_SERVER_URL                      = OpsServerUrl                      ?? "COMMERCE_OPS_SERVER_URL";
                IDENTITY_SERVER_URL                 = IdentityServerUrl                 ?? "COMMERCE_IDENTITY_SERVER_URL";
                SC_ADMIN_USER                       = ScAdminUser                       ?? "COMMERCE_SC_ADMIN_USER";
                SC_ADMIN_PASSWORD                   = ScAdminPassword                   ?? "COMMERCE_SC_ADMIN_PASSWORD";
            }
        }
    }
}
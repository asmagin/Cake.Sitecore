public static partial class Sitecore
{
    public static class Constants
    {
        // Generic parameters
        public static string BUILD_CONFIGURATION { get; private set; }  
        public static string SOLUTION_NAME       { get; private set; }        
        // Sitecore parameters
        public static string SC_ADMIN_USER       { get; private set; }         
        public static string SC_ADMIN_PASSWORD   { get; private set; }     
        public static string SC_NODE_ENV         { get; private set; }           
        public static string SC_NODE_ROLE        { get; private set; }          
        public static string SC_SITE_URL         { get; private set; }           
        // Versioning
        public static string VERSION             { get; private set; }             
        public static string ASSEMBLY_VERSION    { get; private set; }     
        // Source Control
        public static string BRANCH_NAME         { get; private set; }          
        public static string COMMIT              { get; private set; }
        // Build Server
        public static string BUILD_ID            { get; private set; }          
        public static string BUILD_NAME          { get; private set; }

        public static string ROOT_DIR            { get; private set; }        

        public static string LIBS_DIR                          { get; private set; }
        public static string LIBS_PACKAGES_DIR                 { get; private set; }
        public static string LIBS_NU_GET_DIR                   { get; private set; }
        public static string LIBS_SPE_DIR                      { get; private set; }
        public static string LIBS_SHIP_DIR                     { get; private set; }
        public static string BUILD_DIR                         { get; private set; }
        public static string SRC_DIR                           { get; private set; }
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
        public static string XUNIT_TESTS_COVERAGE_OUTPUT_DIR   { get; private set; }
        public static string JEST_TESTS_COVERAGE_OUTPUT_DIR    { get; private set; }
        public static string PUBLISHING_TARGET_DIR             { get; private set; }
        public static string SC_LOCAL_WEBSITE_ROOT_DIR         { get; private set; }

        public static string SOLUTION_FILE_PATH                { get; private set; }
        public static string UNICORN_CONFIG_PATH               { get; private set; }

        public static void SetNames(
            string BuildConfiguration            = null,
            string SolutionName                  = null,      
            string ScAdminUser                   = null,       
            string ScAdminPassword               = null,   
            string ScNodeEnv                     = null,         
            string ScNodeRole                    = null,        
            string ScSiteUrl                     = null,         
            string Version                       = null,           
            string AssemblyVersion               = null,   
            string BranchName                    = null,        
            string Commit                        = null,
            string BuildId                       = null,
            string BuildName                     = null,
            string RootDir                       = null,
            string LibsDir                       = null,
            string LibsPackagesDir               = null,
            string LibsNuGetDir                  = null,
            string LibsSpeDir                    = null,
            string LibsShipDir                   = null,
            string BuildDir                      = null,
            string SrcDir                        = null,
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
            string XunitTestsCoverageOutputDir   = null,
            string JestTestsCoverageOutputDir    = null,        
            string PublishingTargetDir          = null,
            string ScLocalWebsiteRootDir        = null,
            
            string SolutionFilePath              = null,
            string UnicornConfigPath             = null     
            )
        {
            BUILD_CONFIGURATION               = BuildConfiguration            ?? "BUILD_CONFIGURATION";
            SOLUTION_NAME                     = SolutionName                  ?? "SOLUTION_NAME";
            SC_ADMIN_USER                     = ScAdminUser                   ?? "SC_ADMIN_USER";
            SC_ADMIN_PASSWORD                 = ScAdminPassword               ?? "SC_ADMIN_PASSWORD";
            SC_NODE_ENV                       = ScNodeEnv                     ?? "SC_NODE_ENV";
            SC_NODE_ROLE                      = ScNodeRole                    ?? "SC_NODE_ROLE";
            SC_SITE_URL                       = ScSiteUrl                     ?? "SC_SITE_URL";
            VERSION                           = Version                       ?? "VERSION";
            ASSEMBLY_VERSION                  = AssemblyVersion               ?? "ASSEMBLY_VERSION";
            BRANCH_NAME                       = BranchName                    ?? "BRANCH_NAME";
            COMMIT                            = Commit                        ?? "COMMIT";
            BUILD_ID                          = BuildId                       ?? "BUILD_ID";
            BUILD_NAME                        = BuildName                     ?? "BUILD_NAME";

            ROOT_DIR                          = RootDir                       ?? "ROOT_DIR";

            LIBS_DIR                          = LibsDir                       ?? "LIBS_DIR"; 
            LIBS_PACKAGES_DIR                 = LibsPackagesDir               ?? "LIBS_PACKAGES_DIR"; 
            LIBS_NU_GET_DIR                   = LibsNuGetDir                  ?? "LIBS_NU_GET_DIR"; 
            LIBS_SPE_DIR                      = LibsSpeDir                    ?? "LIBS_SPE_DIR"; 
            LIBS_SHIP_DIR                     = LibsShipDir                   ?? "LIBS_SHIP_DIR"; 
            BUILD_DIR                         = BuildDir                      ?? "BUILD_DIR"; 
            SRC_DIR                           = SrcDir                        ?? "SRC_DIR"; 
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
            XUNIT_TESTS_COVERAGE_OUTPUT_DIR   = XunitTestsCoverageOutputDir   ?? "XUNIT_TESTS_COVERAGE_OUTPUT_DIR"; 
            JEST_TESTS_COVERAGE_OUTPUT_DIR    = JestTestsCoverageOutputDir    ?? "JEST_TESTS_COVERAGE_OUTPUT_DIR"; 
            PUBLISHING_TARGET_DIR             = PublishingTargetDir           ?? "PUBLISHING_TARGET_DIR"; 
            SC_LOCAL_WEBSITE_ROOT_DIR         = ScLocalWebsiteRootDir         ?? "SC_LOCAL_WEBSITE_ROOT_DIR"; 
            
            SOLUTION_FILE_PATH                = SolutionFilePath              ?? "SOLUTION_FILE_PATH"; 
            UNICORN_CONFIG_PATH               = UnicornConfigPath             ?? "UNICORN_CONFIG_PATH";
        }
    }
}
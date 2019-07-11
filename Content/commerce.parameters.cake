using Cake.Common.Tools.MSBuild;

public static partial class Sitecore
{
    public static partial class Parameters
    {
        public static partial class Commerce
        {
            public static string BuildConfiguration             { get; private set; }
            public static string NuGetConfigPath                { get; private set; }
            public static string LibsNuGetDir                   { get; private set; }

            public static string EngineProjectPath              { get; private set; }
            public static string SolutionName                   { get; private set; }
            public static string SolutionFilePath               { get; private set; }

            public static string TestsOutputDir                             { get; private set; }
            public static string TestsCoverageOutputDir                     { get; private set; }
            public static string XUnitTestsCoverageOutputDir                { get; private set; }
            public static string XUnitTestsCoverageRegister                 { get; private set; }
            public static string XUnitTestsCoverageExcludeAttributeFilters  { get; private set; }
            public static string XUnitTestsCoverageExcludeFileFilters       { get; private set; }
            public static string XUnitTestsCoverageExcludeDirectories       { get; private set; }

            public static string ArtifactsBuildDir              { get; private set; }
            public static string BaseLocalWebsiteRootDir        { get; private set; }
            public static string AuthoringLocalWebsiteRootDir   { get; private set; }
            public static string MinionsLocalWebsiteRootDir     { get; private set; }
            public static string OpsLocalWebsiteRootDir         { get; private set; }
            public static string ShopsLocalWebsiteRootDir       { get; private set; }

            public static string OpsServerUrl                   { get; private set; }

            public static string IdentityServerUrl              { get; private set; }
            public static string ScAdminUser                    { get; private set; }
            public static string ScAdminPassword                { get; private set; }

            public static void InitParams(
                ICakeContext context,

                // Add support of defining parameters in code
                string buildConfiguration =                         null,
                string nuGetConfigPath =                            null,
                string libsNuGetDir =                               null,

                string engineProjectPath =                          null,
                string solutionName =                               null,
                string solutionFilePath =                           null,

                string testsOutputDir =                             null,
                string testsCoverageOutputDir =                     null,
                string xUnitTestsCoverageOutputDir =                null,
                string xUnitTestsCoverageRegister =                 null,
                string xUnitTestsCoverageExcludeAttributeFilters =  null,
                string xUnitTestsCoverageExcludeFileFilters =       null,
                string xUnitTestsCoverageExcludeDirectories =       null,

                string artifactsBuildDir =                          null,
                string baseLocalWebsiteRootDir =                    null,
                string authoringLocalWebsiteRootDir =               null,
                string minionsLocalWebsiteRootDir =                 null,
                string opsLocalWebsiteRootDir =                     null,
                string shopsLocalWebsiteRootDir =                   null,

                string opsServerUrl =                               null,
                string identityServerUrl =                          null,
                string scAdminUser =                                null,
                string scAdminPassword =                            null
            )
            {
                _context =                      context;

                BuildConfiguration =            GetParameterValue(Constants.Commerce.BUILD_CONFIGURATION,               buildConfiguration      ?? Sitecore.Parameters.BuildConfiguration);
                NuGetConfigPath =               GetParameterValue(Constants.Commerce.NUGET_CONFIG_PATH,                 nuGetConfigPath         ?? Sitecore.Parameters.NuGetConfigPath);
                LibsNuGetDir =              GetAbsoluteDirPath(
                                                GetParameterValue(Constants.Commerce.LIBS_NUGET_DIR,                    libsNuGetDir            ?? Sitecore.Parameters.LibsNuGetDir));

                EngineProjectPath =             GetParameterValue(Constants.Commerce.ENGINE_PROJECT_PATH,               engineProjectPath       ?? $"{Sitecore.Parameters.SrcDir}\\Commerce\\Sitecore.Commerce.Engine\\Sitecore.Commerce.Engine.csproj");
                SolutionName =                  GetParameterValue(Constants.Commerce.SOLUTION_NAME,                     solutionName            ?? Sitecore.Parameters.SolutionName);
                SolutionFilePath =              GetParameterValue(Constants.Commerce.SOLUTION_FILE_PATH,                solutionFilePath        ?? $"{Sitecore.Parameters.SrcDir}/{Sitecore.Parameters.Commerce.SolutionName}.Commerce.sln");

                TestsOutputDir =                            GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.TESTS_OUTPUT_DIR,                   testsOutputDir                              ?? $"{Sitecore.Parameters.OutputDir}/tests"));
                TestsCoverageOutputDir =                    GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.TESTS_COVERAGE_OUTPUT_DIR,          testsCoverageOutputDir                      ?? $"{TestsOutputDir}/coverage"));
                XUnitTestsCoverageOutputDir =               GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.XUNIT_TESTS_COVERAGE_OUTPUT_DIR,    xUnitTestsCoverageOutputDir                 ?? $"{TestsCoverageOutputDir}/commerce-xUnit"));
                XUnitTestsCoverageRegister =                GetParameterValue(Constants.Commerce.XUNIT_TESTS_COVERAGE_REGISTER,                         xUnitTestsCoverageRegister                  ?? Sitecore.Parameters.XUnitTestsCoverageRegister);
                XUnitTestsCoverageExcludeAttributeFilters = GetParameterValue(Constants.Commerce.XUNIT_TESTS_COVERAGE_EXCLUDE_ATTRIBUTE_FILTERS,        xUnitTestsCoverageExcludeAttributeFilters   ?? Sitecore.Parameters.XUnitTestsCoverageExcludeAttributeFilters);
                XUnitTestsCoverageExcludeFileFilters =      GetParameterValue(Constants.Commerce.XUNIT_TESTS_COVERAGE_EXCLUDE_FILE_FILTERS,             xUnitTestsCoverageExcludeFileFilters        ?? Sitecore.Parameters.XUnitTestsCoverageExcludeFileFilters);
                XUnitTestsCoverageExcludeDirectories =      GetParameterValue(Constants.Commerce.XUNIT_TESTS_COVERAGE_EXCLUDE_DIRECTORIES,              xUnitTestsCoverageExcludeDirectories        ?? Sitecore.Parameters.XUnitTestsCoverageExcludeDirectories);

                ResolveTargetDirs(
                    baseLocalWebsiteRootDir,
                    authoringLocalWebsiteRootDir,
                    minionsLocalWebsiteRootDir,
                    opsLocalWebsiteRootDir,
                    shopsLocalWebsiteRootDir,
                    artifactsBuildDir
                );

                OpsServerUrl =                  GetParameterValue(Constants.Commerce.OPS_SERVER_URL,                    opsServerUrl            ?? "https://commerceops.sc9.local");
                IdentityServerUrl =             GetParameterValue(Constants.Commerce.IDENTITY_SERVER_URL,               identityServerUrl       ?? "https://sc9.identityserver");
                ScAdminUser =               EnsureSitecoreUserWithDomain(
                                                GetParameterValue(Constants.Commerce.SC_ADMIN_USER,                     scAdminUser             ?? Sitecore.Parameters.ScAdminUser));
                ScAdminPassword =               GetParameterValue(Constants.Commerce.SC_ADMIN_PASSWORD,                 scAdminPassword         ?? Sitecore.Parameters.ScAdminPassword);
            }

            private static string GetParameterValue(string argumentName, string defaultValue, string environmentNamePrefix = null) {
                return Utils.ArgumentOrEnvironmentVariable(_context, argumentName, defaultValue, environmentNamePrefix);
            }

            private static string EnsureSitecoreUserWithDomain(string userName) => 
                string.IsNullOrEmpty(userName) || userName.Contains("\\") ? userName : $"sitecore\\{userName}";

            private static void ResolveTargetDirs(
                string baseLocalWebsiteRootDir,
                string authoringLocalWebsiteRootDir,
                string minionsLocalWebsiteRootDir,
                string opsLocalWebsiteRootDir,
                string shopsLocalWebsiteRootDir,
                string artifactsBuildDir) 
            {
                if (BuildConfiguration == "Debug") {

                    Sitecore.Parameters.Commerce.BaseLocalWebsiteRootDir = 
                        GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.BASE_LOCAL_WEBSITE_ROOT_DIR,       baseLocalWebsiteRootDir      ?? $"{Sitecore.Parameters.ScLocalWebsiteRootDir}/.." ?? "\\\\192.168.50.4\\c$\\inetpub\\wwwroot"));

                    Sitecore.Parameters.Commerce.AuthoringLocalWebsiteRootDir = 
                        GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.AUTHORING_LOCAL_WEBSITE_ROOT_DIR,  authoringLocalWebsiteRootDir ?? $"{Sitecore.Parameters.Commerce.BaseLocalWebsiteRootDir}/CommerceAuthoring_sc9"));
                    Sitecore.Parameters.Commerce.MinionsLocalWebsiteRootDir = 
                        GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.MINIONS_LOCAL_WEBSITE_ROOT_DIR,    minionsLocalWebsiteRootDir   ?? $"{Sitecore.Parameters.Commerce.BaseLocalWebsiteRootDir}/CommerceMinions_sc9"));
                    Sitecore.Parameters.Commerce.OpsLocalWebsiteRootDir =
                        GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.OPS_LOCAL_WEBSITE_ROOT_DIR,        opsLocalWebsiteRootDir       ?? $"{Sitecore.Parameters.Commerce.BaseLocalWebsiteRootDir}/CommerceOps_sc9"));
                    Sitecore.Parameters.Commerce.ShopsLocalWebsiteRootDir =
                        GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.SHOPS_LOCAL_WEBSITE_ROOT_DIR,      shopsLocalWebsiteRootDir     ?? $"{Sitecore.Parameters.Commerce.BaseLocalWebsiteRootDir}/CommerceShops_sc9"));

                } else {
                    Sitecore.Parameters.Commerce.ArtifactsBuildDir = 
                        GetAbsoluteDirPath(GetParameterValue(Constants.Commerce.ARTIFACTS_BUILD_DIR,               artifactsBuildDir            ?? $"{Sitecore.Parameters.ArtifactsDir}/commerce-build"));
                }
            }
        }
    }
}
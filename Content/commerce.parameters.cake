using Cake.Common.Tools.MSBuild;

public static partial class Sitecore
{
    public static partial class Parameters
    {
        public static partial class Commerce
        {
            public static string BuildConfiguration { get; private set; }

            public static string ArtifactsBuildDir { get; private set; }
            public static string BaseLocalWebsiteRootDir { get; private set; }
            public static IDictionary<string, string> RolesConfiguration { get; private set; }

            public static string EngineProjectPath { get; private set; }
            public static string SolutionName { get; private set; }
            public static string SolutionPath { get; private set; }

            public static string OpsServerUrl { get; private set; }

            public static string IdentityServerUrl { get; private set; }
            public static string ScAdminUser { get; private set; }
            public static string ScAdminPassword { get; private set; }

            public static void InitParams(
                ICakeContext context,

                // Add support of defining parameters in code
                string buildConfiguration =                         null,

                string artifactsBuildDir =                          null,
                string baseLocalWebsiteRootDir =                       null,
                IDictionary<string, string> rolesConfiguration =    null,

                string engineProjectPath =                          null,
                string solutionName =                               null,
                string solutionPath =                               null,

                string opsServerUrl =                               null,
                string identityServerUrl =                          null,
                string scAdminUser =                                null,
                string scAdminPassword =                            null
            )
            {
                _context =                      context;

                BuildConfiguration =            GetParameterValue(Constants.Commerce.BUILD_CONFIGURATION,               buildConfiguration      ?? Sitecore.Parameters.BuildConfiguration);

                ArtifactsBuildDir =             GetAbsoluteDirPath(
                                                    GetParameterValue(Constants.Commerce.ARTIFACTS_BUILD_DIR,           artifactsBuildDir       ?? $"{Sitecore.Parameters.ArtifactsDir}/commerce"));
                BaseLocalWebsiteRootDir =       GetAbsoluteDirPath(
                                                    GetParameterValue(Constants.Commerce.BASE_LOCAL_WEBSITE_ROOT_DIR,   baseLocalWebsiteRootDir ?? Sitecore.Parameters.BaseLocalWebsiteRootDir));
                RolesConfiguration =            GetRolesConfiguration(rolesConfiguration);

                EngineProjectPath =             GetParameterValue(Constants.Commerce.ENGINE_PROJECT_PATH,               engineProjectPath       ?? $"{Sitecore.Parameters.SrcDir}\\Commerce\\Sitecore.Commerce.Engine\\Sitecore.Commerce.Engine.csproj");
                SolutionName =                  GetParameterValue(Constants.Commerce.SOLUTION_NAME,                     solutionName            ?? $"{Sitecore.Parameters.SolutionName}.Commerce");
                SolutionPath =                  GetParameterValue(Constants.Commerce.SOLUTION_PATH,                     solutionPath            ?? $"{Sitecore.Parameters.SrcDir}/{Sitecore.Parameters.Commerce.SolutionName}.sln");

                OpsServerUrl =                  GetParameterValue(Constants.Commerce.OPS_SERVER_URL,                    opsServerUrl            ?? "https://commerceops.sc9.local");
                IdentityServerUrl =             GetParameterValue(Constants.Commerce.IDENTITY_SERVER_URL,               identityServerUrl       ?? "https://sc9.identityserver");
                ScAdminUser =                   GetParameterValue(Constants.Commerce.SC_ADMIN_USER,                     scAdminUser             ?? "sitecore\\admin");
                ScAdminPassword =               GetParameterValue(Constants.Commerce.SC_ADMIN_PASSWORD,                 scAdminPassword         ?? "b");

                // Those parameters absolutely needed
                Utils.AssertIfNull(RolesConfiguration, "RolesConfiguration", "COMMERCE_ROLES_CONFIGURATION");
            }

            private static string GetParameterValue(string argumentName, string defaultValue, string environmentNamePrefix = null) {
                return Utils.ArgumentOrEnvironmentVariable(_context, argumentName, defaultValue, environmentNamePrefix);
            }

            private static IDictionary<string, string> GetRolesConfiguration(IDictionary<string, string> defaultValue) {
                IDictionary<string, string> rolesConfiguration = null;
                var parameterValue = GetParameterValue(Constants.Commerce.ROLES_CONFIGURATION, null);
                if (parameterValue != null) {
                    rolesConfiguration = Utils.ParseStringAsNameValueCollection(parameterValue);
                }

                rolesConfiguration = rolesConfiguration ?? defaultValue;

                rolesConfiguration = rolesConfiguration
                    ?.ToDictionary(x => x.Key, x => {
                        var _dirPath = DirectoryPath.FromString(x.Value);
                        if (!_dirPath.IsRelative)
                            return _dirPath.ToString();

                        return _dirPath.MakeAbsolute(DirectoryPath.FromString(BaseLocalWebsiteRootDir)).ToString();
                    });

                // ToDo: implement for artifacts folder
                return rolesConfiguration;
            }

        }
    }
}
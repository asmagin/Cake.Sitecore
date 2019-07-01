using Cake.Common.Tools.MSBuild;

public static partial class Sitecore
{
    public static partial class Parameters
    {
        public static partial class Commerce
        {
            public static string BuildConfiguration { get; private set; }
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

                BuildConfiguration =            GetParameterValue(Constants.Commerce.BUILD_CONFIGURATION,           buildConfiguration  ?? "Debug");
                RolesConfiguration =            GetRolesConfiguration(Constants.Commerce.ROLES_CONFIGURATION,       rolesConfiguration  ?? null);

                EngineProjectPath =             GetParameterValue(Constants.Commerce.ENGINE_PROJECT_PATH,           engineProjectPath   ?? $"{Sitecore.Parameters.SrcDir}\\Commerce\\Sitecore.Commerce.Engine\\Sitecore.Commerce.Engine.csproj");
                SolutionName =                  GetParameterValue(Constants.Commerce.SOLUTION_NAME,                 solutionName        ?? $"{Sitecore.Parameters.SolutionName}.Commerce");
                SolutionPath =                  GetParameterValue(Constants.Commerce.SOLUTION_PATH,                 solutionPath        ?? $"{Sitecore.Parameters.SrcDir}/{Sitecore.Parameters.Commerce.SolutionName}.sln");

                OpsServerUrl =                  GetParameterValue(Constants.Commerce.OPS_SERVER_URL,                opsServerUrl        ?? "https://sc9.identityserver");
                IdentityServerUrl =             GetParameterValue(Constants.Commerce.IDENTITY_SERVER_URL,           identityServerUrl   ?? "https://commerceops.sc9.local");
                ScAdminUser =                   GetParameterValue(Constants.Commerce.SC_ADMIN_USER,                 scAdminUser         ?? Sitecore.Parameters.ScAdminUser);
                ScAdminPassword =               GetParameterValue(Constants.Commerce.SC_ADMIN_PASSWORD,             scAdminPassword     ?? Sitecore.Parameters.ScAdminPassword);

                // Those parameters absolutely needed
                Utils.AssertIfNull(Sitecore.Parameters.Commerce.RolesConfiguration, "RolesConfiguration", "COMMERCE_ROLES_CONFIGURATION");
            }

            private static string GetParameterValue(string argumentName, string defaultValue, string environmentNamePrefix = null) {
                return Utils.ArgumentOrEnvironmentVariable(_context, argumentName, defaultValue, environmentNamePrefix);
            }

            private static IDictionary<string, string> GetRolesConfiguration(string argumentName, IDictionary<string, string> defaultValue) {
                if (defaultValue != null)
                    return defaultValue;

                var parameterValue = GetParameterValue(argumentName, null);

                var localWebsitesRoot = GetParameterValue(Constants.LOCAL_WEBSITES_ROOT_DIR, "\\\\192.168.50.4\\c$\\inetpub\\wwwroot");
                //var localWebRoot = GetParameterValue(Constants.SC_LOCAL_WEBSITE_ROOT_DIR, $"{localWebsitesRoot}\\sc9.local");

                var configuration = Utils.ParseStringAsNameValueCollection(parameterValue);

                var result = configuration
                    .ToDictionary(x => x.Key, x => GetAbsoluteFilePath($"{localWebsitesRoot}/{x.Value}"));

                // ToDo: implement for artifacts folder
                return result;
            }

            // ToDo: implement for artifacts folder
            private static string GetPublishingTargetDir(string defaultValue) {
                var path = GetParameterValue(Constants.PUBLISHING_TARGET_DIR, defaultValue);
                var localWebsitesRoot = GetParameterValue(Constants.LOCAL_WEBSITES_ROOT_DIR, "\\\\192.168.50.4\\c$\\inetpub\\wwwroot");
                var localWebRoot = GetParameterValue(Constants.SC_LOCAL_WEBSITE_ROOT_DIR, $"{localWebsitesRoot}\\sc9.local");

                if (string.IsNullOrEmpty(path)) {
                    path = BuildConfiguration == "Debug"
                        ? localWebRoot
                        : ArtifactsBuildDir;
                }

                return GetAbsoluteFilePath(path);
            }
        }
    }
}
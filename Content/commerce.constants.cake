public static partial class Sitecore
{
    public static partial class Constants
    {
        public static partial class Commerce
        {
            public static string BUILD_CONFIGURATION            { get; private set; }
            public static string ROLES_CONFIGURATION            { get; private set; }

            public static string ENGINE_PROJECT_PATH            { get; private set; }
            public static string SOLUTION_NAME                  { get; private set; }
            public static string SOLUTION_PATH                  { get; private set; }

            public static string OPS_SERVER_URL                 { get; private set; }

            public static string IDENTITY_SERVER_URL            { get; private set; }
            public static string SC_ADMIN_USER                  { get; private set; }
            public static string SC_ADMIN_PASSWORD              { get; private set; }

            public static void SetNames(
                string BuildConfiguration           = null,
                string RolesConfiguration           = null,
                string EngineProjectPath            = null,
                string SolutionName                 = null,
                string SolutionPath                 = null,
                string OpsServerUrl                 = null,
                string IdentityServerUrl            = null,
                string ScAdminUser                  = null,
                string ScAdminPassword              = null
            ) {
                BUILD_CONFIGURATION             = BuildConfiguration            ?? "COMMERCE_BUILD_CONFIGURATION";
                ROLES_CONFIGURATION             = RolesConfiguration            ?? "COMMERCE_ROLES_CONFIGURATION";
                ENGINE_PROJECT_PATH             = EngineProjectPath             ?? "COMMERCE_ENGINE_PROJECT_PATH";
                SOLUTION_NAME                   = SolutionName                  ?? "COMMERCE_SOLUTION_NAME";
                SOLUTION_PATH                   = SolutionPath                  ?? "COMMERCE_SOLUTION_PATH";
                OPS_SERVER_URL                  = OpsServerUrl                  ?? "COMMERCE_OPS_SERVER_URL";
                IDENTITY_SERVER_URL             = IdentityServerUrl             ?? "COMMERCE_IDENTITY_SERVER_URL";
                SC_ADMIN_USER                   = ScAdminUser                   ?? "COMMERCE_SC_ADMIN_USER";
                SC_ADMIN_PASSWORD               = ScAdminPassword               ?? "COMMERCE_SC_ADMIN_PASSWORD";
            }
        }
    }
}
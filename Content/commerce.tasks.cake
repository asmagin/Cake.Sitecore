#addin "Cake.FileHelpers&version=3.2.0"
#addin "Cake.Http&version=0.6.0"

using System.Net.Http;

public class CommerceTasks {
    // Tasks
    public CakeTaskBuilder RestoreNuGetPackagesTask { get; set; }
    public CakeTaskBuilder CleanCommerceSiteDirectoriesTask { get; set; }
    public CakeTaskBuilder PublishEngineTask { get; set; }
    public CakeTaskBuilder BootstrapCommerceConfigurationTask { get; set; }

    // Task names
    public string RestoreNuGetPackagesTaskName => GetTaskName(this.RestoreNuGetPackagesTask);
    public string CleanCommerceSiteDirectoriesTaskName => GetTaskName(this.CleanCommerceSiteDirectoriesTask);
    public string PublishEngineTaskName => GetTaskName(this.PublishEngineTask);
    public string BootstrapCommerceConfigurationTaskName => GetTaskName(this.BootstrapCommerceConfigurationTask);

    // private helpers
    private static string GetTaskName(CakeTaskBuilder taskBuilder) => TaskExtensions.GetTaskName(taskBuilder);
}

public static partial class Sitecore
{
    public static class Commerce
    {
        public static CommerceTasks Tasks = new CommerceTasks();
    }
}

Sitecore.Commerce.Tasks.RestoreNuGetPackagesTask = Task("Commerce :: Restore :: Restore NuGet Packages")
    .Description("Commerce: Restore NuGet packages for a solution")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.SolutionPath, "SolutionPath", "COMMERCE_SOLUTION_PATH");

        // Default NuGet settings
        NuGetRestoreSettings _settings = null;

        if (FileExists(Sitecore.Parameters.NuGetConfigPath))
        {
            Warning("NuGet configuration file found and will be used.");
            _settings = new NuGetRestoreSettings {
                ConfigFile = Sitecore.Parameters.NuGetConfigPath
            };
        }
        else {
            Warning("NuGet configuration file not found and defaults and local settings will be used.");
            Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.LibsNuGetDir, "LibsNuGetDir", "LIBS_NUGET_DIR");

            _settings = new NuGetRestoreSettings {
                Source = new List<string> {
                    "https://api.nuget.org/v3/index.json;",
                    "https://sitecore.myget.org/F/sc-packages/api/v3/index.json",
                    "https://sitecore.myget.org/F/sc-commerce-packages/api/v3/index.json",
                    Sitecore.Parameters.LibsNuGetDir
                }
            };
        }

        NuGetRestore(Sitecore.Parameters.Commerce.SolutionPath, _settings);
    });

Sitecore.Commerce.Tasks.CleanCommerceSiteDirectoriesTask = Task("Commerce :: Clean :: Clean site directories")
    .Description("Commerce: Clean site directories")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.RolesConfiguration, "RolesConfiguration", "COMMERCE_ROLES_CONFIGURATION");

        var excludedExtensions = new [] { ".log", ".txt", ".md" };
        foreach (var _commerceRoleConfiguration in Sitecore.Parameters.Commerce.RolesConfiguration) {
            var _commerceSiteRootDir = DirectoryPath.FromString(_commerceRoleConfiguration.Value);

            EnsureDirectoryExists(_commerceSiteRootDir);
            Information($"Cleaning '{_commerceSiteRootDir}' site root directory");

            // ToDo: IIS stop & start for the site
            FileWriteText(
                _commerceSiteRootDir.CombineWithFilePath(FilePath.FromString($"App_Offline.htm")),
                string.Empty);

            RetryAccessDenied((retryCount) => {
                    Verbose($"Cleaning directory '{_commerceSiteRootDir}' at the {retryCount} attempt");
                    CleanDirectory(
                        _commerceSiteRootDir,
                        fileSystemInfo => !excludedExtensions.Contains(FilePath.FromString(fileSystemInfo.Path.FullPath).GetExtension()));
                }, 10, 500);
        }
    });

Sitecore.Commerce.Tasks.PublishEngineTask = Task("Commerce :: Publish :: Publish Engine")
    .Description("Commerce: Publishes Commerce Engine project (`COMMERCE_ENGINE_PROJECT_PATH`) according to (`COMMERCE_ROLES_CONFIGURATION`) using MsBuild.")
    .Does(() => {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BuildConfiguration, "BuildConfiguration", "BUILD_CONFIGURATION");

        Sitecore.Utils.AssertIfNull(Sitecore.Parameters.Commerce.RolesConfiguration, "RolesConfiguration", "COMMERCE_ROLES_CONFIGURATION");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.EngineProjectPath, "EngineProjectPath", "COMMERCE_ENGINE_PROJECT_PATH");

        foreach (var _commerceRoleConfiguration in Sitecore.Parameters.Commerce.RolesConfiguration)
        {
            var _commerceSiteRootDir = DirectoryPath.FromString(_commerceRoleConfiguration.Value);
            var _commerceRoleName = _commerceRoleConfiguration.Key;

            EnsureDirectoryExists(_commerceSiteRootDir);

            // ToDo: does it make sense to reuse task.publish.cake?
            var _msBuildSettings = new MSBuildSettings()
                .SetConfiguration(Sitecore.Parameters.BuildConfiguration)
                .SetVerbosity(Verbosity.Minimal)
                .UseToolVersion(Sitecore.Parameters.MsBuildToolVersion)
                .WithTarget("Rebuild")
                .WithProperty("DeployOnBuild", "true")
                .WithProperty("DeployDefaultTarget", "WebPublish")
                .WithProperty("WebPublishMethod", "FileSystem")
                .WithProperty("DeleteExistingFiles", "false")
                .WithProperty("PublishUrl", _commerceSiteRootDir.ToString());

            var _projectFilePath = FilePath.FromString(Sitecore.Parameters.Commerce.EngineProjectPath);
            MSBuild(_projectFilePath, _msBuildSettings);

            // ToDo: patterns should be configured?
            transformJsonFile(
                Context,
                _commerceSiteRootDir.CombineWithFilePath(FilePath.FromString($"wwwroot\\config.json")),
                _commerceSiteRootDir.CombineWithFilePath(FilePath.FromString($"wwwroot\\config.transform.{_commerceRoleName}.json")));

            DeleteFiles($"{_commerceSiteRootDir}/wwwroot/config.transform.*.json");
        }
    });


Sitecore.Commerce.Tasks.BootstrapCommerceConfigurationTask = Task("Commerce :: Bootstrap Commerce configuration using Identity Server")
    .Description("Commerce: Bootstraps the commerce environment using Identity Server")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.OpsServerUrl, "OpsServerUrl", "COMMERCE_OPS_SERVER_URL");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.IdentityServerUrl, "IdentityServerUrl", "COMMERCE_IDENTITY_SERVER_URL");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.ScAdminUser, "ScAdminUser", "COMMERCE_SC_ADMIN_USER");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.ScAdminPassword, "ScAdminPassword", "COMMERCE_SC_ADMIN_PASSWORD");

        // 1 - Authorization
        // ToDo: PostMan configuration is used - should identity server be configured for separate client?
        var _identityServerUrl = new Uri(Sitecore.Parameters.Commerce.IdentityServerUrl);
        Information($"Authenticating to '{_identityServerUrl}' identity server");
        string _authResponseBody = HttpPost($"{new Uri(_identityServerUrl, "/connect/token")}", _settings => _settings
                .SetNoCache()
                .EnsureSuccessStatusCode()
                .SetFormUrlEncodedRequestBody(new Dictionary<string, string> {
                    { "username", Sitecore.Parameters.Commerce.ScAdminUser },
                    { "password", Sitecore.Parameters.Commerce.ScAdminPassword },
                    { "grant_type", "password" },
                    { "client_id", "postman-api" },
                    { "scope", "openid EngineAPI postman_api" },
                })
        );

        var _authResponseJson = JsonEncoder.DeserializeObject<JsonObject>(_authResponseBody);
        var _authToken = $"{_authResponseJson["token_type"]} {_authResponseJson["access_token"]}";
        Debug($"AuthToken: {_authToken}");

        // 2 & 3 - Bootstrap Options & Bootstrap
        using (var httpClientHandler = new HttpClientHandler())
        {
            using (var httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(Sitecore.Parameters.Commerce.OpsServerUrl) })
            {
                Information($"Requesting OPTIONS for '{httpClient.BaseAddress}' commerce Bootstrap operation");
                var optionsResponse = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Options, "/commerceops/Bootstrap()") {
                    Headers = { { "Authorization", _authToken } }
                }).Result;
                optionsResponse.EnsureSuccessStatusCode();

                Information($"Calling '{httpClient.BaseAddress}' commerce Bootstrap operation");
                var xsrfToken = httpClientHandler.CookieContainer.GetCookies(httpClient.BaseAddress)?["XSRF-TOKEN"]?.Value;
                var response = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/commerceops/Bootstrap()") {
                    Headers = { { "X-XSRF-TOKEN", xsrfToken }, { "Authorization", _authToken } }
                }).Result;

                response.EnsureSuccessStatusCode();
            }
        }
    });

#addin "Cake.Http&version=0.6.0"

using System.Net.Http;

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
            using (var httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(Sitecore.Parameters.Commerce.OpsServerUrl), Timeout = TimeSpan.FromMinutes(5) })
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

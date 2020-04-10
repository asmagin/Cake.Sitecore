#addin "Cake.Powershell&version=0.4.8"

Func<string, string, string> getUnicornSecret = (unicornConfigPath, unicornSecret) => {
    if (string.IsNullOrEmpty(unicornSecret))
    {
        if (FileExists(unicornConfigPath))
        {
            return XmlPeek(
                unicornConfigPath,
                "//configuration/sitecore/unicorn/authenticationProvider/SharedSecret"
            );
        }
        return null;
    }
    else
    {
        return unicornSecret;
    }
};

Func<string, string> getSiteUrlFromPublishSettings = (srcRoot) => {
    var file = $"{srcRoot}/publishsettings.targets";

    if (FileExists(file))
    {
        return XmlPeek(
            $"{srcRoot}/publishsettings.targets",
            "//*[local-name() = 'publishUrl']"
        );
    }

    return null;
};

Action<string, string, string, string, string> runUnicornSync = (siteUrl, secret, scriptDir, configurations, basicAuth) =>
{
    var url = $"{siteUrl.Trim('/')}/unicorn.aspx";
    
    StartPowershellFile(
        $"{scriptDir}/Sync.ps1",
        new PowershellSettings()
            .WithArguments(args => {
                args.Append("url", url).AppendSecret("secret", secret);

                if (!string.IsNullOrEmpty(scriptDir))
                {
                    args.Append("scriptDir", scriptDir);
                }
                if (!string.IsNullOrEmpty(configurations))
                {
                    args.Append("Configurations", configurations);
                }
                if (!string.IsNullOrEmpty(basicAuth))
                {
                    args.AppendSecret("basicAuth", basicAuth);
                }
            })
    );
};

#addin "Cake.Powershell&version=0.4.8"

Action<string, List<FilePath>> DeploySitecorePackages = (sitecoreInstanceUri, files) =>
{
    foreach (var _file in files)
    {
        var _fileName = _file.FullPath;

        Information($"Starting processing: {_fileName}.");
        StartPowershellFile(
            Context.Tools.Resolve("ship/deploySitecorePackage.ps1"),
            new PowershellSettings()
                .WithArguments(args => {
                    args.Append("Url", sitecoreInstanceUri)
                        .Append("FilePath", $"\"{_fileName}\"");
                })
        );

        Information($"Processing completed: {_fileName}.");
    }

    Information($"Processing of all files completed.");
};
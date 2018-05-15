#addin "Microsoft.Web.Xdt"

using Microsoft.Web.XmlTransform;

Action<string, string, List<string>> transform = (sourceFile, targetFile, transforms) =>
{
    var _appliedTransforms = new List<String>();
    using(var _document  = new XmlTransformableDocument { PreserveWhitespace = true })
    {
        _document.Load(sourceFile);

        foreach(var _transform in transforms)
        {
            var _transformFile = File($"{sourceFile}.{_transform}.transform");
            if (!FileExists(_transformFile))
            {
                continue;
            }
            using(var _transformation = new Microsoft.Web.XmlTransform.XmlTransformation(_transformFile))
            {
                if(!_transformation.Apply(_document))
                {
                    throw new Exception($"Failed to transform \"{sourceFile}\" using \"{_transformFile}\" to \"{targetFile}\"");
                }
                _appliedTransforms.Add(_transform);
            }
        }

        var _targetFilePath = new FilePath(targetFile);
        var _targetDirectory = _targetFilePath.GetDirectory();

        if (!DirectoryExists(_targetDirectory))
        {
            CreateDirectory(_targetDirectory);
        }

        _document.Save(targetFile);

        var _transformationsMessage = _appliedTransforms.Any()
            ? string.Join(">", _appliedTransforms)
            : "transforms not found!";

        Information($" + updated '{targetFile}': {_transformationsMessage}");
    }
};

Action<string, string> copyClientAssets = (srcRootDir, layer) =>
{
    Verbose($"Executing [copyClientAssets] with params ({srcRootDir}, {layer})");

    var _template = $"{srcRootDir}/{layer}/*/client/build";
    var _directoryList = GetDirectories(_template);

    // iterate over every client build folder
    foreach(var _directory in _directoryList)
    {
        Verbose($"Copy client assets from: {_directory.ToString()}");

        var _pathSegments =  _directory.ToString()
            .Replace("/client/build", "")
            .Split('/');
        var _project = _pathSegments[_pathSegments.Length -1];

        // copy client build artifacts into a project wildcard folder
        var _targetDir = $"{_directory}/../../code/dist/TMNA/{layer}/{_project}";
        CopyDirectory(_directory, _targetDir);
    }
};

Action<string, string> copySerializationFiles = (srcRootDir, layer) =>
{
    Verbose($"Executing [copySerializationFiles] with params ({srcRootDir}, {layer})");

    var _directoryList = GetDirectories($"{srcRootDir}/{layer}/*/serialization");

    foreach(var _directory in _directoryList)
    {
        Verbose($"Copy Unicorn files from: {_directory.ToString()}");

        var _pathSegments =  _directory.ToString().Split('/');
        var _project = _pathSegments[_pathSegments.Length - 2];

        var _targetDir = $"{_directory}/../code/App_Data/unicorn/{layer}/{_project}/serialization";
        CopyDirectory(_directory, _targetDir);
    }

    // another pass to collect content yml files
    _directoryList = GetDirectories($"{srcRootDir}/{layer}/*/serialization.content");

    foreach(var _directory in _directoryList)
    {
        var _pathSegments = _directory.ToString().Split('/');
        var _project = _pathSegments[_pathSegments.Length - 2];

        var _targetDir = $"{_directory}/../code/App_Data/unicorn/{layer}/{_project}/serialization.content";
        CopyDirectory(_directory, _targetDir);
    }
};

Action<string, string, string, MSBuildToolVersion> publishProject = (projectFilePath, buildConfiguration, dest, msBuildToolVersion) =>
{
    Verbose($"Executing [publishProject] with params ({projectFilePath}, {buildConfiguration}, {dest}, {msBuildToolVersion.ToString()})");

    var _settings = new MSBuildSettings()
        .SetConfiguration(buildConfiguration)
        .SetVerbosity(Verbosity.Minimal)
        .UseToolVersion(msBuildToolVersion)
        .WithTarget("Rebuild")
        .WithProperty("DeployOnBuild", "true")
        .WithProperty("DeployDefaultTarget", "WebPublish")
        .WithProperty("WebPublishMethod", "FileSystem")
        .WithProperty("DeleteExistingFiles", "false")
        .WithProperty("publishUrl", dest)
        .WithProperty("_FindDependencies", "false");

    MSBuild(projectFilePath, _settings);
};

Action<string, string, string, string, MSBuildToolVersion> publishLayer = (srcRootDir, layer, buildConfiguration, dest, msBuildToolVersion) =>
{
    Verbose($"Executing [publishLayer] with params ({srcRootDir}, {layer}, {buildConfiguration}, {dest}, {msBuildToolVersion.ToString()})");

    // perform cleanup layer configs directory operation for "Debug" configuration
    if (buildConfiguration == "Debug")
    {
        var _configsDirPath = $"{dest}/App_Config/Include/{layer}";
        if (DirectoryExists(_configsDirPath)) {
            Information($"Cleaning configs directory: {_configsDirPath}");
            CleanDirectory(_configsDirPath);
        }
    }

    var _projectFilePathList = GetFiles($"{srcRootDir}/{layer}/**/code/*.csproj");

    foreach(var _projectFilePath in _projectFilePathList)
    {
        publishProject(_projectFilePath.ToString(), buildConfiguration, dest, msBuildToolVersion);
    }
};

Sitecore.Tasks.PublishFoundationTask = Task("Publish :: Foundation")
    .Description("Publishes all Foundation-layer projects to the publishing target directory (`PUBLISHING_TARGET_DIR`) using MsBuild.")
    .Does(() =>
    {
        var _layer = "Foundation";

        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.PublishingTargetDir, "PublishingTargetDir", "PUBLISHING_TARGET_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BuildConfiguration, "SrcConfigsDir", "SRC_CONFIGS_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");

        if(Sitecore.Parameters.BuildConfiguration != "Debug") {
            copySerializationFiles(Sitecore.Parameters.SrcDir, _layer);
        }
        copyClientAssets(Sitecore.Parameters.SrcDir, _layer);
        publishLayer(
            Sitecore.Parameters.SrcDir,
            _layer,
            Sitecore.Parameters.BuildConfiguration,
            Sitecore.Parameters.PublishingTargetDir,
            Sitecore.Parameters.MsBuildToolVersion);
    });

Sitecore.Tasks.PublishFeatureTask = Task("Publish :: Features")
    .Description("Publishes all Feature-layer projects to the publishing target directory (`PUBLISHING_TARGET_DIR`) using MsBuild.")
    .Does(() =>
    {
        var _layer = "Feature";

        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.PublishingTargetDir, "PublishingTargetDir", "PUBLISHING_TARGET_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BuildConfiguration, "SrcConfigsDir", "SRC_CONFIGS_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");

        if(Sitecore.Parameters.BuildConfiguration != "Debug") {
            copySerializationFiles(Sitecore.Parameters.SrcDir, _layer);
        }
        copyClientAssets(Sitecore.Parameters.SrcDir, _layer);
        publishLayer(
            Sitecore.Parameters.SrcDir,
            _layer,
            Sitecore.Parameters.BuildConfiguration,
            Sitecore.Parameters.PublishingTargetDir,
            Sitecore.Parameters.MsBuildToolVersion);
    });

Sitecore.Tasks.PublishProjectTask = Task("Publish :: Projects")
    .Description("Publishes all Project-layer projects to the publishing target directory (`PUBLISHING_TARGET_DIR`) using MsBuild.")
    .Does(() =>
    {
        var _layer = "Project";

        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.PublishingTargetDir, "PublishingTargetDir", "PUBLISHING_TARGET_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.BuildConfiguration, "SrcConfigsDir", "SRC_CONFIGS_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");

        if(Sitecore.Parameters.BuildConfiguration != "Debug") {
            copySerializationFiles(Sitecore.Parameters.SrcDir, _layer);
        }
        copyClientAssets(Sitecore.Parameters.SrcDir, _layer);
        publishLayer(
            Sitecore.Parameters.SrcDir,
            _layer,
            Sitecore.Parameters.BuildConfiguration,
            Sitecore.Parameters.PublishingTargetDir,
            Sitecore.Parameters.MsBuildToolVersion);
    });

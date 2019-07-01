#addin "Cake.FileHelpers&version=3.2.0"

#addin nuget:?package=Microsoft.VisualStudio.Jdt&version=0.9.23
using Microsoft.VisualStudio.Jdt;

#load "json-transform-logger.cake"

Action<ICakeContext, FilePath, FilePath> transformJsonFile = (ICakeContext context, FilePath filePath, FilePath transformFilePath) =>
{
    context.Information($"Transforming '{filePath}' file with '{transformFilePath}' transformation file");
    IJsonTransformationLogger logger = new CakeJsonTransformationLogger(context);

    var transform = new JsonTransformation(transformFilePath.FullPath, logger);
    var outputStream = transform.Apply(filePath.FullPath);

    using (var outputStreamReader = new StreamReader(outputStream))
    {
        string outputString = outputStreamReader.ReadToEnd();
        FileWriteText(filePath, outputString);
    }
};

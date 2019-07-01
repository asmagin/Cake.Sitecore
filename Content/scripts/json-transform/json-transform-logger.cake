#addin nuget:?package=Microsoft.VisualStudio.Jdt&version=0.9.23
using Microsoft.VisualStudio.Jdt;

public class CakeJsonTransformationLogger : IJsonTransformationLogger
{
    private readonly ICakeContext cakeContext;
    private readonly string contextMessage;

    public CakeJsonTransformationLogger(ICakeContext cakeContext, string contextMessage = null) {
        this.cakeContext = cakeContext;
        this.contextMessage = contextMessage;
    }

    public void LogMessage(string message)
    {
        this.cakeContext.Information(this.PostProcessMessage($"{message}"));
    }

    public void LogMessage(string message, string fileName, int lineNumber, int linePosition)
    {
        this.cakeContext.Information(this.PostProcessMessage($"{message} in {fileName}:{lineNumber}:{linePosition}"));
    }

    public void LogWarning(string message)
    {
        this.cakeContext.Warning(this.PostProcessMessage($"{message}"));
    }

    public void LogWarning(string message, string fileName)
    {
        this.cakeContext.Warning(this.PostProcessMessage($"{message} in {fileName}"));
    }

    public void LogWarning(string message, string fileName, int lineNumber, int linePosition)
    {
        this.cakeContext.Warning(this.PostProcessMessage($"{message} in {fileName}:{lineNumber}:{linePosition}"));
    }

    public void LogError(string message)
    {
        this.cakeContext.Error(this.PostProcessMessage($"{message}"));
    }

    public void LogError(string message, string fileName, int lineNumber, int linePosition)
    {
        this.cakeContext.Error(this.PostProcessMessage($"{message} in {fileName}:{lineNumber}:{linePosition}"));
    }

    public void LogErrorFromException(Exception ex)
    {
        this.cakeContext.Error(this.PostProcessMessage($"{ex.Message}\n{ex}"));
    }

    public void LogErrorFromException(Exception ex, string fileName, int lineNumber, int linePosition)
    {
        this.cakeContext.Error(this.PostProcessMessage($"{ex.Message} in {fileName}:{lineNumber}:{linePosition}\n{ex}"));
    }

    private string PostProcessMessage(string message) {
        return !string.IsNullOrEmpty(this.contextMessage) ? $"{this.contextMessage}\n{message}": message;
    }
}

// extentions
public static class Utils{
    public static string ArgumentOrEnvironmentVariable(ICakeContext context, string argumentName, string defaultValue, string environmentNamePrefix = null) {
        return context.Argument<string>(argumentName, context.EnvironmentVariable(environmentNamePrefix + argumentName)) ?? defaultValue;
    }

    public static void ThrowIfNullOrEmpty(string varName, string envName) {
        throw new Exception($"Variable '{varName}' cannot be empty. Please, set it in code or provide argument on environment variable '{envName}'");
    }
}

// Aliases
Func<string, string, string, string> ArgumentOrEnvironmentVariable = (argumentName, defaultValue, environmentNamePrefix) => {
    return Utils.ArgumentOrEnvironmentVariable(Context, argumentName, environmentNamePrefix, defaultValue ); // TODO: refactor !!! swap places of defaults and prefix
};
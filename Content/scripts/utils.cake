// extentions
public static partial class Sitecore 
{ 
    public static partial class Utils {
        public static string ArgumentOrEnvironmentVariable(ICakeContext context, string argumentName, string defaultValue, string environmentNamePrefix = null) {
            return context.Argument<string>(argumentName, context.EnvironmentVariable(environmentNamePrefix + argumentName)) ?? defaultValue;
        }

        public static void AssertIfNullOrEmpty(string value, string varName, string envName = null) {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"Variable '{varName}' cannot be empty. Please, set it in code or provide argument on environment variable '{envName ?? varName}'");
            }
        }
    }
}

// Aliases
Func<string, string, string, string> ArgumentOrEnvironmentVariable = (argumentName, defaultValue, environmentNamePrefix) => {
    return Sitecore.Utils.ArgumentOrEnvironmentVariable(Context, argumentName, environmentNamePrefix, defaultValue ); // TODO: refactor !!! swap places of defaults and prefix
};
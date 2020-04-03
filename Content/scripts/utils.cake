// extentions
public static partial class Sitecore 
{ 
    public static partial class Utils 
    {
        public static string ArgumentOrEnvironmentVariable(ICakeContext context, string argumentName, string defaultValue, string environmentNamePrefix = null) {
            return context.Argument<string>(argumentName, context.EnvironmentVariable(environmentNamePrefix + argumentName)) ?? defaultValue;
        }

        public static void AssertIfNullOrEmpty(string value, string varName, string envName = null) {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"Variable '{varName}' cannot be empty. Please, set it in code or provide argument on environment variable '{envName ?? varName}'");
            }
        }

        public static void AssertIfNull(object value, string varName, string envName = null) {
            if (value == null)
            {
                throw new Exception($"Variable '{varName}' cannot be null. Please, set it in code or provide argument on environment variable '{envName ?? varName}'");
            }
        }
    }
}

// Aliases
Func<string, string, string, string> ArgumentOrEnvironmentVariable = (argumentName, defaultValue, environmentNamePrefix) => {
    return Sitecore.Utils.ArgumentOrEnvironmentVariable(Context, argumentName, environmentNamePrefix, defaultValue ); // TODO: refactor !!! swap places of defaults and prefix
};

// File access
Action<Action<int>, int, int> RetryAccessDenied = (Action<int> action, int retryLimit, int millisecondsTimeout) => {
    // ToDo: test this method
    for (var retryCount = 1; retryCount <= retryLimit; retryCount ++) {
        try {
            action(retryCount);
            return;
        }
        catch (System.Exception ex) {
            if (retryCount == retryLimit)
                throw;

            var hResult = (int?)ex.GetType()
                ?.GetProperty("HResult", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.GetValue(ex, null);

            Debug($"Exception: {ex.GetType()}; Message: {ex.Message}; HResult: {hResult}\n{ex}");
            if (ex is System.UnauthorizedAccessException || (ex is System.IO.IOException && hResult == -2147024864))
            {
                System.Threading.Thread.Sleep(millisecondsTimeout);
                continue;
            }

            // ToDo: investigate other possible exceptions
            Warning($"Exception: {ex.GetType()}; Message: {ex.Message}; HResult: {hResult}\n{ex}");
            throw;
        }
    }
};

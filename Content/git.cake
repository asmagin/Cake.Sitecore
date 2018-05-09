using System.Text.RegularExpressions;

public static partial class Sitecore{
    public static class Git {
        private const int MAX_TAG_LENGTH = 20;

        public static string GetTagFromBranchName(string branchName) {
            var result = (branchName ?? "alpha")
                .ToLower()
                .Replace(" ", "-");

            var regex = new Regex("($0|[^0-9A-Za-z-])");
            result = regex.Replace(result, String.Empty);

            regex = new Regex("--+");
            result = regex.Replace(result, "-").Substring(0, result.Length > MAX_TAG_LENGTH ? MAX_TAG_LENGTH : result.Length);

            return result;
        }
    }
}

public static bool IsRelease(this string branch) {
    return (branch ?? "").ToLower().StartsWith("release");
}

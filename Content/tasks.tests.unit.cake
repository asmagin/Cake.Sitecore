
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=OpenCoverToCoberturaConverter"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=xunit.runner.console"

#load "./scripts/coverage/coverage.cake"

// extentions
public static partial class Sitecore 
{ 
    public static partial class Variables{
        public static bool UnitTestsFailed = false;
    }
}

Sitecore.Tasks.RunServerUnitTestsTask = Task("Unit Tests :: Run Server Tests")
    .Description("Execute all available tests available for server-side code (xUnit)")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionName, "SolutionName", "SOLUTION_NAME");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.XUnitTestsCoverageOutputDir, "XUnitTestsCoverageOutputDir", "XUNIT_TESTS_COVERAGE_OUTPUT_DIR");

        var _coverSettings = new OpenCoverSettings()
            .WithFilter($"+[{Sitecore.Parameters.SolutionName}.*]*")
            .WithFilter($"-[{Sitecore.Parameters.SolutionName}.*.Tests*]*");
        _coverSettings.SkipAutoProps = true;
        _coverSettings.Register = "user";
        _coverSettings.MergeByHash = true;
        _coverSettings.NoDefaultFilters = true;
        _coverSettings.ReturnTargetCodeOffset = 0;

        var _directories = GetDirectories(
                $"{Sitecore.Parameters.SrcDir}/**/bin", 
                fileSystemInfo => fileSystemInfo.Path.FullPath.IndexOf("node_modules", StringComparison.OrdinalIgnoreCase) < 0
            );
        foreach (var directory in _directories)
        {
            _coverSettings.SearchDirectories.Add(directory);
        }

        EnsureDirectoryExists(Sitecore.Parameters.XUnitTestsCoverageOutputDir);

        var _openCoverResultsFilePath = new FilePath($"{Sitecore.Parameters.XUnitTestsCoverageOutputDir}/coverage.xml");

        var _xUnit2Settings = new XUnit2Settings {
                XmlReport = true,
                Parallelism = ParallelismOption.None,
                NoAppDomain = false,
                OutputDirectory = testsOutputDir,
                ReportName = "xUnitTestResults"
            };

        OpenCover(
            tool => { tool.XUnit2($"{Sitecore.Parameters.SrcDir}/**/tests/bin/*.Tests.dll", _xUnit2Settings); }, 
            _openCoverResultsFilePath, 
            _coverSettings
        );

        ReportGenerator(_openCoverResultsFilePath, Sitecore.Parameters.XUnitTestsCoverageOutputDir);

        var _converterExecutablePath = Context.Tools.Resolve("OpenCoverToCoberturaConverter.exe");
        StartProcess(_converterExecutablePath, new ProcessSettings {
            Arguments = new ProcessArgumentBuilder()
                .Append($"-input:\"{_openCoverResultsFilePath}\"")
                .Append($"-output:\"{Sitecore.Parameters.XUnitTestsCoverageOutputDir}/cobertura-coverage.xml\"")
        });
    })
    .OnError(exception =>
    {
        Sitecore.Variables.UnitTestsFailed = true;
    });

Sitecore.Tasks.RunClientUnitTestsTask = Task("Unit Tests :: Run Client Tests")
    .Description("Run Jest tests")
    .Does(() => {
        var settings = new NpmRunScriptSettings();

        settings.ScriptName = "test-cover"; // TODO: make configurable
        settings.LogLevel = NpmLogLevel.Error;

        NpmRunScript(settings);
    })
    .OnError(exception =>
    {
        Sitecore.Variables.UnitTestsFailed = true;
    });


Sitecore.Tasks.MergeCoverageReportsTask = Task("Unit Tests :: Merge Coverage Reports")
    .Description("Merge test coverage reports")
    .Does(() => {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.TestsCoverageOutputDir, "TestsCoverageOutputDir", "TESTS_COVERAGE_OUTPUT_DIR");

        if (Sitecore.Variables.UnitTestsFailed) {
            throw new Exception("One or more tests are failing");
        }

        var _mergedReport = $"{Sitecore.Parameters.TestsCoverageOutputDir}/cobertura-coverage.xml";

        var _sourceFilePaths = GetFiles($"{testsCoverageOutputDir}/*/cobertura-coverage.xml");

        mergeCoberturaReports(Context.Tools.Resolve("cobertura.tpl.xml").ToString(), _sourceFilePaths, _mergedReport);

        var htmlReportFilePath = $"{testsCoverageOutputDir}/index.html";
        mergeHtmlReports(Context.Tools.Resolve("coberturaReport.tpl.html").ToString(), htmlReportFilePath);
    });    
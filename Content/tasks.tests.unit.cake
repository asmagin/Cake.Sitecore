#tool "nuget:?package=OpenCover&version=4.7.922"
#tool "nuget:?package=OpenCoverToCoberturaConverter&version=0.3.4"
#tool "nuget:?package=ReportGenerator&version=4.0.13.1"
#tool "nuget:?package=xunit.runner.console&version=2.4.1"

// extentions
public static partial class Sitecore 
{ 
    public static partial class Variables{
        public static bool UnitTestsFailed = false;
    }
}

Sitecore.Tasks.RunServerUnitTestsTask = Task("Unit Tests :: Run Server Tests")
    .Description("Executes all available tests for server-side code using xUnit. Result will be placed into (`TESTS_OUTPUT_DIR`), also code coverage reports will be created in `cobertura` format in (`XUNIT_TESTS_COVERAGE_OUTPUT_DIR`) directory.")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SolutionName, "SolutionName", "SOLUTION_NAME");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.XUnitTestsCoverageOutputDir, "XUnitTestsCoverageOutputDir", "XUNIT_TESTS_COVERAGE_OUTPUT_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.XUnitTestsCoverageRegister, "XUnitTestsCoverageRegister", "XUNIT_TESTS_COVERAGE_REGISTER");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.TestsOutputDir, "TestsOutputDir", "TESTS_OUTPUT_DIR");

        var _coverSettings = new OpenCoverSettings()
            .WithFilter($"+[{Sitecore.Parameters.SolutionName}.*]*")
            .WithFilter($"-[{Sitecore.Parameters.SolutionName}.*.Tests*]*");
        _coverSettings.SkipAutoProps = true;
        _coverSettings.Register = Sitecore.Parameters.XUnitTestsCoverageRegister;
        _coverSettings.MergeByHash = true;
        _coverSettings.NoDefaultFilters = true;
        _coverSettings.ReturnTargetCodeOffset = 0;

        void applyExclude<T>(ISet<T> filtersSet, string paramValue, Func<string, T> mapper)
        {
            if (!string.IsNullOrEmpty(paramValue))
            {
                var excludes = paramValue.Split(',').Select(mapper);
                filtersSet.UnionWith(excludes);
            }
        }

        applyExclude(_coverSettings.ExcludedAttributeFilters, Sitecore.Parameters.XUnitTestsCoverageExcludeAttributeFilters, x => x);
        applyExclude(_coverSettings.ExcludedFileFilters,      Sitecore.Parameters.XUnitTestsCoverageExcludeFileFilters, x => x);
        applyExclude(_coverSettings.ExcludeDirectories,       Sitecore.Parameters.XUnitTestsCoverageExcludeDirectories, x => Directory($"{Sitecore.Parameters.SrcDir}/{x}"));

        var _directories = GetDirectories(
            $"{Sitecore.Parameters.SrcDir}/**/bin",
            new GlobberSettings {
                Predicate = fileSystemInfo => fileSystemInfo.Path.FullPath.IndexOf("node_modules", StringComparison.OrdinalIgnoreCase) < 0
            });
        foreach (var directory in _directories)
        {
            _coverSettings.SearchDirectories.Add(directory);
        }

        EnsureDirectoryExists(Sitecore.Parameters.XUnitTestsCoverageOutputDir);

        var _openCoverResultsFilePath = new FilePath($"{Sitecore.Parameters.XUnitTestsCoverageOutputDir}/coverage.xml");

        var parallelismOptionArgument = ParallelismOption.None;
        switch (Sitecore.Parameters.XUnitTestsRunInParallel)
        {
            case "All":
                    parallelismOptionArgument = ParallelismOption.All;
                    break;
            case "Assemblies":
                    parallelismOptionArgument = ParallelismOption.Assemblies;
                    break;
            case "Collections":
                    parallelismOptionArgument = ParallelismOption.Collections;
                    break;
            default:
                    parallelismOptionArgument = ParallelismOption.None;
                    break;
        }

        var _xUnit2Settings = new XUnit2Settings {
                XmlReport = true,
                Parallelism = parallelismOptionArgument,
                NoAppDomain = false,
                OutputDirectory = Sitecore.Parameters.TestsOutputDir,
                ReportName = "xUnitTestResults",
                ShadowCopy = Sitecore.Parameters.XUnitShadowCopy
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

        if (Sitecore.Parameters.TestsFailImmediately) {
            throw new Exception("One or more tests are failing");
        }
    });

Sitecore.Tasks.RunClientUnitTestsTask = Task("Unit Tests :: Run Client Tests")
    .Description("Executes all available tests for client-side code via `npm run test-cover`. Result **should** be placed into (`TESTS_OUTPUT_DIR`) including code coverage reports in `cobertura`. ")
    .Does(() => {
        var settings = new NpmRunScriptSettings();

        settings.ScriptName = "test-cover"; // TODO: make configurable
        settings.LogLevel = NpmLogLevel.Error;

        NpmRunScript(settings);
    })
    .OnError(exception =>
    {
        Sitecore.Variables.UnitTestsFailed = true;

        if (Sitecore.Parameters.TestsFailImmediately) {
            throw new Exception("One or more tests are failing");
        }
    });


Sitecore.Tasks.MergeCoverageReportsTask = Task("Unit Tests :: Merge Coverage Reports")
    .Description("Merges available code coverage reports produces by previous steps. Generates an `index` file in a coverage directory (`TESTS_COVERAGE_OUTPUT_DIR`).")
    .Does(() => {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.TestsCoverageOutputDir, "TestsCoverageOutputDir", "TESTS_COVERAGE_OUTPUT_DIR");

        if (Sitecore.Variables.UnitTestsFailed) {
            throw new Exception("One or more tests are failing");
        }

        var _mergedReport = $"{Sitecore.Parameters.TestsCoverageOutputDir}/cobertura-coverage.xml";

        var _sourceFilePaths = GetFiles($"{Sitecore.Parameters.TestsCoverageOutputDir}/*/cobertura-coverage.xml");

        mergeCoberturaReports(Context.Tools.Resolve("coverage/cobertura.tpl.xml").ToString(), _sourceFilePaths, _mergedReport);

        var htmlReportFilePath = $"{Sitecore.Parameters.TestsCoverageOutputDir}/index.html";
        mergeHtmlReports(Context.Tools.Resolve("coverage/coberturaReport.tpl.html").ToString(), htmlReportFilePath);
    });    
#tool "nuget:?package=OpenCover&version=4.7.922"
#tool "nuget:?package=OpenCoverToCoberturaConverter&version=0.3.4"
#tool "nuget:?package=ReportGenerator&version=4.0.13.1"
#tool "nuget:?package=xunit.runner.console&version=2.4.1"

Sitecore.Commerce.Tasks.RunServerUnitTestsTask = Task("Commerce :: Unit Tests :: Run Server Tests")
    .Description("Executes all available tests for server-side code using xUnit. Result will be placed into (`TESTS_OUTPUT_DIR`), also code coverage reports will be created in `cobertura` format in (`XUNIT_TESTS_COVERAGE_OUTPUT_DIR`) directory.")
    .Does(() =>
    {
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.SrcDir, "SrcDir", "SRC_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.SolutionFilePath, "SolutionFilePath", "COMMERCE_SOLUTION_FILE_PATH");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.XUnitTestsCoverageOutputDir, "XUnitTestsCoverageOutputDir", "COMMERCE_XUNIT_TESTS_COVERAGE_OUTPUT_DIR");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.XUnitTestsCoverageRegister, "XUnitTestsCoverageRegister", "COMMERCE_XUNIT_TESTS_COVERAGE_REGISTER");
        Sitecore.Utils.AssertIfNullOrEmpty(Sitecore.Parameters.Commerce.TestsOutputDir, "TestsOutputDir", "COMMERCE_TESTS_OUTPUT_DIR");

        var _coverSettings = createOpenCoverSettings(
                $"{Sitecore.Parameters.SrcDir}/**/bin/**",
                Sitecore.Parameters.Commerce.XUnitTestsCoverageRegister,
                Sitecore.Parameters.Commerce.XUnitTestsCoverageExcludeAttributeFilters,
                Sitecore.Parameters.Commerce.XUnitTestsCoverageExcludeFileFilters,
                Sitecore.Parameters.Commerce.XUnitTestsCoverageExcludeDirectories)
            .WithFilter($"+[{Sitecore.Parameters.Commerce.SolutionName}.*]*")
            .WithFilter($"-[{Sitecore.Parameters.Commerce.SolutionName}.*.Tests*]*");

        var _dotNetCoreTestSettings = new DotNetCoreTestSettings {
            NoBuild = true,
            NoRestore = true,
            VSTestReportPath = $"{Sitecore.Parameters.Commerce.TestsOutputDir}/commerce-xUnitTestResults.xml"
        };

        runOpenCoverWithReporting(
            tool => { tool.DotNetCoreTest(Sitecore.Parameters.Commerce.SolutionFilePath, _dotNetCoreTestSettings); },
            Sitecore.Parameters.Commerce.XUnitTestsCoverageOutputDir,
            _coverSettings
        );
    })
    .OnError(exception =>
    {
        Sitecore.Variables.UnitTestsFailed = true;

        if (Sitecore.Parameters.TestsFailImmediately) {
            throw new Exception("One or more tests are failing");
        }
    });

#tool "nuget:?package=OpenCover&version=4.7.922"

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
                $"{Sitecore.Parameters.SrcDir}/**/bin/{Sitecore.Parameters.Commerce.BuildConfiguration}/*",
                Sitecore.Parameters.Commerce.XUnitTestsCoverageRegister,
                Sitecore.Parameters.Commerce.XUnitTestsCoverageExcludeAttributeFilters,
                Sitecore.Parameters.Commerce.XUnitTestsCoverageExcludeFileFilters,
                Sitecore.Parameters.Commerce.XUnitTestsCoverageExcludeDirectories)
            .WithFilter($"+[{Sitecore.Parameters.Commerce.SolutionName}.*]*")
            .WithFilter($"-[{Sitecore.Parameters.Commerce.SolutionName}.*.Tests*]*")
            .WithFilter($"-[{Sitecore.Parameters.Commerce.SolutionName}.*.Testing*]*");

        var _uniqueGuid = Guid.NewGuid();
        var _dotNetCoreTestSettings = new DotNetCoreTestSettings {
            NoBuild = true,
            NoRestore = true,
            Logger = $"trx;LogFileName=TestResults-{_uniqueGuid:N}.trx"
        };

        runOpenCoverWithReporting(
            tool => { tool.DotNetCoreTest(Sitecore.Parameters.Commerce.SolutionFilePath, _dotNetCoreTestSettings); },
            Sitecore.Parameters.Commerce.XUnitTestsCoverageOutputDir,
            _coverSettings
        );

        DeleteFiles($"{Sitecore.Parameters.Commerce.TestsOutputDir}/commerce-xUnitTestResults-*.xml");
        var _reportFiles = GetFiles($"{Sitecore.Parameters.SrcDir}/**/TestResults-{_uniqueGuid:N}.trx");
        foreach (var _reportFile in _reportFiles) {
            var _projectName = _reportFile.GetDirectory().Segments
                .LastOrDefault(x => x.StartsWith(Sitecore.Parameters.Commerce.SolutionName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(_projectName)) {
                MoveFile(_reportFile, $"{Sitecore.Parameters.Commerce.TestsOutputDir}/commerce-xUnitTestResults-{_projectName}.xml");
            }
        }
    })
    .OnError(exception =>
    {
        Sitecore.Variables.UnitTestsFailed = true;

        if (Sitecore.Parameters.TestsFailImmediately) {
            throw new Exception("One or more tests are failing");
        }
    });

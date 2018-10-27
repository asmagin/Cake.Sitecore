// base settings
#reference "System.Xml.Linq"

using System.Xml.Linq;
using System.Xml.XPath;

Func<XElement, string, string> getAttributeStringValue = (element, attributeName) => {
    return element.Attribute(attributeName).Value;
};

Func<XElement, string, double> getAttributeDoubleValue = (element, attributeName) => {
    var attributeValueStr = getAttributeStringValue(element, attributeName);

    double d = 0;
    if (double.TryParse(attributeValueStr, out d)){
        return d;
    }

    return 0;
};

Func<XElement, string, int> getAttributeIntegerValue = (element, attributeName) => {
    var attributeValueStr = getAttributeStringValue(element, attributeName);
    return int.Parse(attributeValueStr);
};

Action<string, Cake.Core.IO.FilePathCollection, string> mergeCoberturaReports = (coberturaTemplateFilePath, sourceFilePaths, destFilePath) => {
    var packages = new List<XElement>();
    var sources = new List<XElement>();
    
    const string lineRateAttributeName = "line-rate";

    const string branchRateAttributeName = "branch-rate";
    
    const string linesCoveredAttributeName = "lines-covered";
    var linesCoveredList = new List<int>();

    const string linesValidAttributeName = "lines-valid";
    var linesValidList = new List<int>();

    const string branchesCoveredAttributeName = "branches-covered";
    var branchesCoveredList = new List<int>();

    const string branchesValidAttributeName = "branches-valid";
    var branchesValidList = new List<int>();

    const string timestampAttributeName = "timestamp";
    var timestamp = 0d;

    const string complexityAttributeName = "complexity";
    var complexity = 0;

    foreach(var sourceFilePath in sourceFilePaths)
    {
        // loading source cobertura
        var sourceDoc = XDocument.Load(sourceFilePath.ToString());
        var rootNode = sourceDoc.Root;

        // select packages from the source document
        var selectedNodes = rootNode.XPathSelectElements("//coverage/packages/*");
        packages.AddRange(selectedNodes);

        // select sources
        var selectedSources = rootNode.XPathSelectElements("//coverage/sources/*");
        sources.AddRange(selectedSources);

        // get lines-covered
        linesCoveredList.Add(getAttributeIntegerValue(rootNode, linesCoveredAttributeName));

        // get lines-valid
        linesValidList.Add(getAttributeIntegerValue(rootNode, linesValidAttributeName));

        //get branches-covered
        branchesCoveredList.Add(getAttributeIntegerValue(rootNode, branchesCoveredAttributeName));

        // get branches-valid
        branchesValidList.Add(getAttributeIntegerValue(rootNode, branchesValidAttributeName));

        // get timestamp, always reset to latest
        var currentTimestamp = getAttributeDoubleValue(rootNode, timestampAttributeName);
        if (currentTimestamp > timestamp)
        {
            timestamp = currentTimestamp;
        }

        // get complexity, always reset to the highest
        var currentComplexity = getAttributeIntegerValue(rootNode, complexityAttributeName);
        if (currentComplexity > complexity)
        {
            complexity = currentComplexity;
        }
    }

    Information($"Merged packages count: {packages.Count}");
    Information($"Merged sources count: {sources.Count}");
    
    var mergedlineRate = (linesCoveredList.Sum() / (float)(linesValidList.Sum())).ToString();
    
    var mergedBranchRate = (branchesCoveredList.Sum() / (float)(branchesValidList.Sum())).ToString();
    
    var mergedLinesCovered = linesCoveredList.Sum().ToString();
    Information($"Merged {linesCoveredAttributeName}: {mergedLinesCovered}; {string.Join(", ", linesCoveredList.ToArray())}");

    var mergedLinesValid = linesValidList.Sum().ToString();
    Information($"Merged {linesValidAttributeName}: {mergedLinesValid}; {string.Join(", ", linesValidList.ToArray())}");

    var mergedBranchesCovered = branchesCoveredList.Sum().ToString();
    Information($"Merged {branchesCoveredAttributeName}: {mergedBranchesCovered}; {string.Join(", ", branchesCoveredList.ToArray())}");

    var mergedBranchesValid = branchesValidList.Sum().ToString();
    Information($"Merged {branchesValidAttributeName}: {mergedBranchesValid}; {string.Join(", ", branchesValidList.ToArray())}");

    var destDocument = XDocument.Load(coberturaTemplateFilePath);
    
    // set merged data
    destDocument.Root.Element("packages").Add(packages);
    destDocument.Root.Element("sources").Add(sources);

    destDocument.Root.Attribute(lineRateAttributeName).Value = mergedlineRate;
    destDocument.Root.Attribute(branchRateAttributeName).Value = mergedBranchRate;
    destDocument.Root.Attribute(linesCoveredAttributeName).Value = mergedLinesCovered;
    destDocument.Root.Attribute(linesValidAttributeName).Value = mergedLinesValid;
    destDocument.Root.Attribute(branchesCoveredAttributeName).Value = mergedBranchesCovered;
    destDocument.Root.Attribute(branchesValidAttributeName).Value = mergedBranchesValid;
    destDocument.Root.Attribute(timestampAttributeName).Value = timestamp.ToString();
    destDocument.Root.Attribute(complexityAttributeName).Value = complexity.ToString();
    // for right now hardcode to 0 
    destDocument.Root.Attribute("version").Value = "0";
    
    destDocument.Save(destFilePath);
};

Action<string, string> mergeHtmlReports = (reportTemplateFilePath, destFilePath) => {
    // TODO: remove hardcode to Jest
    CopyFile(reportTemplateFilePath, destFilePath);
};
using System.Collections.Generic;
using TestReport.Components;

namespace TestReportItemReader.Interface
{
    public class TestreportItem : ITestReportComponent
    {

        public TestreportItem()
        {
            TestReportItems = new List<TestreportItem>();
            TableTitles = new List<string> { };
            FurtherInfo = new List<string> { };
        }

        public List<ITestReportComponent> ListOfComponents { get; set; } = new List<ITestReportComponent>();
        public ReportItemType reportItemType { get; set; } = ReportItemType.Null;
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public bool HasTable { get; set; } = false;
        public string Reference { get; set; } = string.Empty;
        public bool HasAdditionalInformation { get; set; } = false;

        public List<TestreportItem> TestReportItems;
        public List<string> TableTitles { get; set; }
        public int TableColumnCount { get; set; } = 1;
        public List<string> FurtherInfo { get; set; }
        public TestreportComponentType TypeOfComponent { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }

    public enum ReportItemType
    {
        Null,
        ProductStandard,
        TestStandard
    }
}

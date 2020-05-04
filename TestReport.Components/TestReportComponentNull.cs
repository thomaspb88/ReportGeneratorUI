using TestreportComponent.Settings;

namespace TestReport.Components
{
    public class TestReportComponentNull : ITestReportComponent
    {
        public TestreportComponentType TypeOfComponent { get; set; } = TestreportComponentType.Null;

        public string Text { get; set; } = "Error - Unable to read this component";
        public ITestReportComponentSettings Settings { get; set; }
    }
}

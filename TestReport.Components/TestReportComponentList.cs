using System.Collections.Generic;
using TestreportComponent.Settings;


namespace TestReport.Components
{
    public class TestReportComponentList : ITestReportComponent
    {

        public TestreportComponentType TypeOfComponent { get; set; } = TestreportComponentType.List;

        public List<string> Text { get; set; } = new List<string>();
        public ITestReportComponentSettings Settings { get; set; }
    }

}
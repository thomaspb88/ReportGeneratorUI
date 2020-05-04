using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestreportComponent.Settings;
using TestReportDocument;

namespace TestReport.Components
{
    public class TestReportComponentList : ITestReportComponent
    {

        public TestreportComponentType TypeOfComponent { get; set; } = TestreportComponentType.List;

        public List<string> Text { get; set; } = new List<string>();
        public ITestReportComponentSettings Settings { get; set; }
    }

}
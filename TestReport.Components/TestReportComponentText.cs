using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestreportComponent.Settings;

namespace TestReport.Components
{
    public class TestReportComponentText : ITestReportComponent
    {
        public TestreportComponentType TypeOfComponent { get; set; } = TestreportComponentType.Text;
        public string Text { get; set; } = string.Empty;
        public ITestReportComponentSettings Settings { get; set; }
    }

}

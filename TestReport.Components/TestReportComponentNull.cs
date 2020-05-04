using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestReport.Components
{
    public class TestReportComponentNull : ITestReportComponent
    {
        public TestreportComponentType TypeOfComponent { get; set; } = TestreportComponentType.Null;

        public string Text { get; set; } = "Error - Unable to read this component";
    }
}

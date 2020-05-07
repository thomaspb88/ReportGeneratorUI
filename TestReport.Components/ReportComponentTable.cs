using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestreportComponent.Settings;

namespace TestReport.Components
{
    public class TestReportComponentTable : ITestReportComponent
    {

        public TestreportComponentType TypeOfComponent { get; set; } = TestreportComponentType.Table;

        public List<string> Titles { get; set; } = new List<string>();

        public int ColumnCount 
        { 
            get 
            {
                return Titles.Count;
            }
            private set 
            { 
            } 
        }

        public ITestReportComponentSettings Settings { get; set; }
    }
}
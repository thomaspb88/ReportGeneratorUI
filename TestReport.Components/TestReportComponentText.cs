using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestReport.Components
{
    public class TestReportComponentText : ITestReportComponent
    {
        public TestreportComponentType TypeOfComponent { get; set; } = TestreportComponentType.Paragrapgh;

        public string Text { get; set; } = string.Empty;

        public void ParseXmlNode(XmlNode node)
        {
            string TypeOfComponent = node.Name;

            this.TypeOfComponent = Enum.TryParse(TypeOfComponent, out TestreportComponentType reportTypComponent) ? reportTypComponent : TestreportComponentType.Null;
        }
    }
}

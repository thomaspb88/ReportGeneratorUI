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
            string TypeOfComponent = node.Attributes["type"].Value;

            this.TypeOfComponent = Enum.TryParse(TypeOfComponent, out TestreportComponentType reportTypComponent) ? reportTypComponent : TestreportComponentType.Null;

            this.Text = node.InnerText;
        }

        public void ParseInformation(XmlNode node, Action<XmlNode> action)
        {
            action(node);
        }

    }
}

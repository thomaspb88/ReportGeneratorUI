using System.Collections.Generic;
using System.Linq;
using System.Xml;

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

        public void ParseXmlNode(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                var listOfText = node.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
                this.Titles = listOfText.ToList();
            }
        }
    }
}
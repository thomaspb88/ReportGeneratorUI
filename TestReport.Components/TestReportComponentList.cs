using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TestReport.Components
{
    public class TestReportComponentList : ITestReportComponent
    {

        public TestreportComponentType TypeOfComponent { get; private set; } = TestreportComponentType.BulletList;

        public List<string> Text { get; set; } = new List<string>();
        TestreportComponentType ITestReportComponent.TypeOfComponent { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void ParseXmlNode(XmlNode node)
        {
            string TypeOfComponent = node.Name;

            this.TypeOfComponent = Enum.TryParse(TypeOfComponent, out TestreportComponentType reportItemType) ? reportItemType : TestreportComponentType.Null;

            if (node.HasChildNodes)
            {
                var listOfText = node.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
                this.Text = listOfText.ToList();
            }
            else
            {
                this.Text.Add(node.InnerText);
            }
        }
    }
}
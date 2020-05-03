using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestReport.Components;

namespace TestReportItemRepository.XML
{
    public static class PopulateTestreportComponents
    {
        internal static ITestReportComponent ParseXmlNode(XmlNode node, ref ITestReportComponent component)
        {
            string TypeOfComponent = node.Attributes["type"].Value;

            component.TypeOfComponent = Enum.TryParse(TypeOfComponent, out TestreportComponentType reportTypComponent) ? reportTypComponent : TestreportComponentType.Null;

            if (component.TypeOfComponent != TestreportComponentType.List || component.TypeOfComponent != TestreportComponentType.Table)
            {
                var reportComponentText = (TestReportComponentText)component;
                reportComponentText.Text = "";

                return reportComponentText;
            }
            else if (component.TypeOfComponent == TestreportComponentType.List)
            {
                var reportComponentList = (TestReportComponentList)component;
                if (node.HasChildNodes)
                {
                    var listOfText = node.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
                    reportComponentList.Text = listOfText.ToList();
                }
                else
                {
                    reportComponentList.Text.Add(node.InnerText);
                }
                return reportComponentList;
            }
            else if (component.TypeOfComponent != TestreportComponentType.Table)
            {
                var reportComponentTable = (TestReportComponentTable)component;
                if (node.HasChildNodes)
                {
                    var listOfText = node.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
                    reportComponentTable.Titles = listOfText.ToList();
                    return reportComponentTable;
                }
            }

            return (TestReportComponentNull)component;
        }
    }
}

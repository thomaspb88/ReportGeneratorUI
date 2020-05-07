using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestReport.Components;

namespace TestReportItemRepository.XML
{
    public static class PopulateTestreportComponent
    {

        private static Func<ITestReportComponent, XmlNode, ITestReportComponent> ParseText = (t, x) =>
        {
            var tr = (TestReportComponentText)t;
            tr.Text = x.InnerText;
            return tr;
        };

        private static Func<ITestReportComponent, XmlNode, ITestReportComponent> ParseList = (t, x) =>
        {
            var c = (TestReportComponentList)t;
            if (x.HasChildNodes)
            {
                var listOfText = x.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
                c.Text = listOfText.ToList();
            }
            else
            {
                c.Text.Add(x.InnerText);
            }
            return c;
        };

        private static Func<ITestReportComponent, XmlNode, ITestReportComponent> ParseTable = (t, x) =>
        {
            var c = (TestReportComponentTable)t;
            if (x.HasChildNodes)
            {
                var listOfText = x.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
                c.Titles = listOfText.ToList();
                return c;
            }
            return new TestReportComponentNull();
        };

        private static Func<ITestReportComponent, XmlNode, ITestReportComponent> ParseNull = (t, x) =>
        {
            return new TestReportComponentNull();
        };

        private static Dictionary<TestreportComponentType, Func<ITestReportComponent, XmlNode, ITestReportComponent>> parsingFunctions
            = new Dictionary<TestreportComponentType, Func<ITestReportComponent, XmlNode, ITestReportComponent>>()
            {
                { TestreportComponentType.Header, ParseText },
                { TestreportComponentType.List, ParseList },
                { TestreportComponentType.Text, ParseText },
                { TestreportComponentType.Reference, ParseText },
                { TestreportComponentType.Subtitle, ParseText },
                { TestreportComponentType.Table, ParseTable },
                { TestreportComponentType.Null, ParseNull }
            };

        internal static ITestReportComponent ParseXmlNode(XmlNode node, ref ITestReportComponent component)
        {
            string TypeOfComponent = node.Attributes["type"].Value;

            component.TypeOfComponent = Enum.TryParse(TypeOfComponent, out TestreportComponentType reportTypComponent) ? reportTypComponent : TestreportComponentType.Null;

            var func = parsingFunctions[component.TypeOfComponent];

            return func(component, node);
        }
    }
}

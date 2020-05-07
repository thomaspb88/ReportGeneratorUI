using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Report.Components;

namespace ReportItemReader.XML
{
    public static class PopulateReportComponent
    {

        private static Func<IReportComponent, XmlNode, IReportComponent> ParseText = (t, x) =>
        {
            var tr = (ReportComponentText)t;
            tr.Text = x.InnerText;
            return tr;
        };

        private static Func<IReportComponent, XmlNode, IReportComponent> ParseList = (t, x) =>
        {
            var c = (ReportComponentList)t;
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

        private static Func<IReportComponent, XmlNode, IReportComponent> ParseTable = (t, x) =>
        {
            var c = (ReportComponentTable)t;
            if (x.HasChildNodes)
            {
                var listOfText = x.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
                c.Titles = listOfText.ToList();
                return c;
            }
            return new ReportComponentNull();
        };

        private static Func<IReportComponent, XmlNode, IReportComponent> ParseNull = (t, x) =>
        {
            return new ReportComponentNull();
        };

        private static Dictionary<ReportComponentType, Func<IReportComponent, XmlNode, IReportComponent>> parsingFunctions
            = new Dictionary<ReportComponentType, Func<IReportComponent, XmlNode, IReportComponent>>()
            {
                { ReportComponentType.Header, ParseText },
                { ReportComponentType.List, ParseList },
                { ReportComponentType.Text, ParseText },
                { ReportComponentType.Reference, ParseText },
                { ReportComponentType.Subtitle, ParseText },
                { ReportComponentType.Table, ParseTable },
                { ReportComponentType.Null, ParseNull }
            };

        internal static IReportComponent ParseXmlNode(XmlNode node, ref IReportComponent component)
        {
            string TypeOfComponent = node.Attributes["type"].Value;

            component.TypeOfComponent = Enum.TryParse(TypeOfComponent, out ReportComponentType reportTypComponent) ? reportTypComponent : ReportComponentType.Null;

            var func = parsingFunctions[component.TypeOfComponent];

            return func(component, node);
        }
    }
}

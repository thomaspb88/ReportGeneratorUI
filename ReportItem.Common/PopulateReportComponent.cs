using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Report.Components;
using XmlNodeExtensionsMethods;

namespace ReportItem.Common
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

        private static Func<IReportComponent, XmlNode, IReportComponent> ParseDefault = (t, x) =>
        {
            return new ReportComponentNull();
        };

        private static Dictionary<ReportComponentType, Func<IReportComponent, XmlNode, IReportComponent>> parsingFunctions
            = new Dictionary<ReportComponentType, Func<IReportComponent, XmlNode, IReportComponent>>()
            {
                { ReportComponentType.Title, ParseText },
                { ReportComponentType.List, ParseList },
                { ReportComponentType.Text, ParseText },
                { ReportComponentType.Reference, ParseText },
                { ReportComponentType.Subtitle, ParseText },
                { ReportComponentType.Table, ParseTable },
                { ReportComponentType.Default, ParseDefault }
            };

        public static IReportComponent ParseXmlNode(XmlNode node, ref IReportComponent component)
        {
            var func = parsingFunctions[component.TypeOfComponent];

            return func(component, node);
        }
    }
}

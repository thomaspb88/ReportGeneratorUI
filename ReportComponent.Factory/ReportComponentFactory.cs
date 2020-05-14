using Report.Components;
using System;
using System.Xml;
using XmlNodeExtensionsMethods;

namespace ReportComponent.Factory
{
    public static class ReportComponentFactory
    {
        public static IReportComponent GetComponentFromXmlNode(XmlNode node)
        {
            string nameOfComponents = String.IsNullOrWhiteSpace(node.Name) ? "Null" : node.Name;
            string component = $"Report.Components.ReportComponent{ nameOfComponents }, Report.Components, Version = 1.0.0.0, Culture = neutral";
            Type componentType = Type.GetType(component);
            object type = Activator.CreateInstance(componentType);
            IReportComponent testreportComponent = type as IReportComponent;

            testreportComponent.TypeOfComponent = GetReportComponentType(node);

            return testreportComponent;
        }

        private static ReportComponentType GetReportComponentType(XmlNode node)
        {
            var nodeType = node.HasAttributes() ? node.GetAttributeValue("type") : node.Name;
            var typeComp = Enum.TryParse(nodeType, out ReportComponentType reportType) ? reportType : ReportComponentType.Default;
            return typeComp;
        }

        //TODO: Move GetSettings class into this library. Encapsulate this functionality into this class.
    }
}

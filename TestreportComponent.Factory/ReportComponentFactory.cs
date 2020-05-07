
using Report.Components;
using System;
using System.Xml;

namespace ReportComponent.Factory
{
    public static class ReportComponentFactory
    {
        public static IReportComponent GetComponentFromXmlNode(XmlNode node)
        {
            string nameOfComponents = String.IsNullOrWhiteSpace(node.Name) ? "Null" : node.Name;
            string component = $"TestReport.Components.TestReportComponent{ nameOfComponents }, TestReportComponent{ nameOfComponents }, Version = 1.0.0.0, Culture = neutral";
            Type componentType = Type.GetType(component);
            object type = Activator.CreateInstance(componentType);
            IReportComponent testreportComponent = type as IReportComponent;
            return testreportComponent;
        }
    }
}

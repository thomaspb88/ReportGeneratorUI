using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestReport.Components;

namespace TestreportComponent.Factory
{
    public static class TestReportComponentFactory
    {
        public static ITestReportComponent GetComponentFromXmlNode(XmlNode node)
        {
            string nameOfComponents = String.IsNullOrWhiteSpace(node.Name) ? "Null" : node.Name;
            string component = $"TestReport.Components.TestReportComponent{ nameOfComponents }, TestReportComponent{ nameOfComponents }, Version = 1.0.0.0, Culture = neutral";
            Type componentType = Type.GetType(component);
            object type = Activator.CreateInstance(componentType);
            ITestReportComponent testreportComponent = type as ITestReportComponent;
            return testreportComponent;
        }
    }
}

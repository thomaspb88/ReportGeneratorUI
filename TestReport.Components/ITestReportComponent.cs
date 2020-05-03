using System.Xml;

namespace TestReport.Components
{
    public interface ITestReportComponent
    {
        TestreportComponentType TypeOfComponent { get; set; }

        void ParseXmlNode(XmlNode testReportItemNode);
    }
}
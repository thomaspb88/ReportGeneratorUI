using System.Xml;
using TestreportComponent.Settings;

namespace TestReport.Components
{
    public interface ITestReportComponent
    {
        TestreportComponentType TypeOfComponent { get; set; }

        ITestReportComponentSettings Settings { get; set; }
    }
}
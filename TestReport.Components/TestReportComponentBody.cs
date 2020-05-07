using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TestreportComponent.Settings;

namespace TestReport.Components
{
    public class TestReportComponentBody : ITestReportComponent
    {
        public TestReportComponentBody()
        {
        }
        public List<ITestReportComponent> ListOfComponents { get; set; } = new List<ITestReportComponent>();
        public ReportItemType ReportItemType { get; set; } = ReportItemType.Null;
        public TestreportComponentType TypeOfComponent { get; set; }
        public ITestReportComponentSettings Settings { get; set; }
        public string Reference { get; set; }
        public string Title 
        {
            get
            {
                if(ListOfComponents != null)
                {

                    return ListOfComponents
                        .Where(x => x.TypeOfComponent == TestreportComponentType.Header)
                        .Cast<TestReportComponentText>()
                        .Select(z => z.Text)
                        .First();
                }

                return "Error - No Title Found";

            }

            set { }
        
        }
    }

    public enum ReportItemType
    {
        Null,
        ProductStandard,
        TestStandard
    }
}

using ReportComponent.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Report.Components
{
    public class ReportComponentBody : IReportComponent
    {
        public List<IReportComponent> ListOfComponents { get; set; } = new List<IReportComponent>();
        public ReportItemType ReportItemType { get; set; } = ReportItemType.Null;
        public ReportComponentType TypeOfComponent { get; set; }
        public ComponentSettings Settings { get; set; }
        public string Reference { get; set; }
        public string Title 
        {
            get
            {
                if(ListOfComponents != null)
                {

                    return ListOfComponents
                        .Where(x => x.TypeOfComponent == ReportComponentType.Header)
                        .Cast<ReportComponentText>()
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

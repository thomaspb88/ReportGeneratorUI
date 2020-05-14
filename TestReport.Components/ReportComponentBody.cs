using Component.Settings;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Report.Components
{
    public class ReportComponentBody : ReportComponentBase
    {
        public List<IReportComponent> ListOfComponents { get; set; } = new List<IReportComponent>();
        public ReportItemType ReportItemType { get; set; } = ReportItemType.Null;
        public IEnumerable<string> Reference 
        { 
            get
            {
                if (ListOfComponents.Count != 0)
                {

                    var reference = ListOfComponents
                        .Where(x => x.TypeOfComponent == ReportComponentType.Reference)
                        .Cast<ReportComponentText>()
                        .Select(z => z.Text);

                    if (reference.Count() < 1 ) { return new List<string>() { "Error - No Reference Found" }; };

                    return reference.ToList();
                }
                return new List<string>() { "Error - No Reference Found" };
            }

            set 
            { 

            }
        
        }
        public string Title 
        {
            get
            {
                if(ListOfComponents.Count != 0 )
                {

                    return ListOfComponents
                        .Where(x => x.TypeOfComponent == ReportComponentType.Title)
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

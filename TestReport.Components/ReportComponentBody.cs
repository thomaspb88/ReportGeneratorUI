﻿using Component.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Report.Components
{
    public class ReportComponentBody : IReportComponent
    {
        public List<IReportComponent> ListOfComponents { get; set; } = new List<IReportComponent>();
        public ReportItemType ReportItemType { get; set; } = ReportItemType.Null;
        public ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.Body;
        public ComponentSetting Settings { get; set; }
        public string Reference { get; set; }
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

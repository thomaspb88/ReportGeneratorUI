using System.Collections.Generic;
using ReportComponent.Settings;


namespace Report.Components
{
    public class ReportComponentList : IReportComponent
    {
        public ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.List;
        public List<string> Text { get; set; } = new List<string>();
        public ComponentSettings Settings { get; set; } = new TextSettings();
    }

}
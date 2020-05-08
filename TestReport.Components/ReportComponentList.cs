using System.Collections.Generic;
using Component.Settings;


namespace Report.Components
{
    public class ReportComponentList : IReportComponent
    {
        public ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.List;
        public List<string> Text { get; set; } = new List<string>();
        public ComponentSetting Settings { get; set; }
    }

}
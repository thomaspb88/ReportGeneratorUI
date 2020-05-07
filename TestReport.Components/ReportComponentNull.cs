using Report.Components;
using Component.Settings;

namespace Report.Components
{
    public class ReportComponentNull : IReportComponent
    {
        public ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.Null;

        public string Text { get; set; } = "Error - Unable to read this component";
        public ComponentSettings Settings { get; set; }
    }
}

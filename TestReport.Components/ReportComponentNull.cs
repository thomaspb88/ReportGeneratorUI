using Report.Components;
using ReportComponent.Settings;

namespace Report.Components
{
    public class ReportComponentNull : IReportComponent
    {
        public ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.Null;

        public string Text { get; set; } = "Error - Unable to read this component";
        public ComponentSettings Settings { get; set; } = new TextSettings();
    }
}

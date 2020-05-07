using Report.Components;
using ReportComponent.Settings;

namespace Report.Components
{
    public class ReportComponentText : IReportComponent
    {
        public ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.Text;
        public string Text { get; set; } = string.Empty;
        public ComponentSettings Settings { get; set; } = new TextSettings() { SpaceAfter = 10, StyleName = "Normal" };
    }

}

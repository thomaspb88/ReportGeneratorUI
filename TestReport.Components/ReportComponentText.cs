using Report.Components;
using Component.Settings;

namespace Report.Components
{
    public class ReportComponentText : IReportComponent
    {
        public ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.Text;
        public string Text { get; set; } = string.Empty;
        public ComponentSettings Settings { get; set; }
    }

}

using Component.Settings;

namespace Report.Components
{
    public interface IReportComponent
    {
        ReportComponentType TypeOfComponent { get; set; }

        ComponentSetting Settings { get; set; }
    }
}
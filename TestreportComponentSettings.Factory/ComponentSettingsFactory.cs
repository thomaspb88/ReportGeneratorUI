using Report.Components;
using Component.Settings;

namespace ComponentSettings.Factory
{
    public static class ComponentSettingsFactory
    {
        public static ComponentSetting GetSetting(IReportComponent component)
        {
            switch (component.TypeOfComponent)
            {
                case ReportComponentType.Default:
                    return new ComponentSetting();
                case ReportComponentType.Title:
                    return new ComponentSetting() { Bold = 1, SpaceAfter = 10, StyleName = "Heading 1" };
                case ReportComponentType.Subtitle:
                    return new ComponentSetting() { Bold = 1, StyleName = "Normal", SpaceAfter = 10 };
                case ReportComponentType.Text:
                    return new ComponentSetting() { SpaceAfter = 10, StyleName = "Normal" };
                case ReportComponentType.List:
                    return new ComponentSetting();
                case ReportComponentType.Table:
                    return new ComponentSetting() { SpaceAfter = 6, SpaceBefore = 6 };
                case ReportComponentType.Reference:
                    return new ComponentSetting();
                case ReportComponentType.Body:
                    return new ComponentSetting();
                default:
                    return new ComponentSetting();
            }
        }
    }
}

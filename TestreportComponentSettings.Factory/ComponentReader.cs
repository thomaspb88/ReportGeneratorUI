using Report.Components;
using Component.Settings;

namespace ReportComponentSettings.Factory
{
    public static class ComponentReader
    {
        public static ComponentSettings GetSetting(IReportComponent component)
        {
            switch (component.TypeOfComponent)
            {
                case ReportComponentType.Null:
                    return new ComponentSettings();
                case ReportComponentType.Header:
                    return new ComponentSettings() { Bold = 1, SpaceAfter = 10, StyleName = "Heading 1" };
                case ReportComponentType.Subtitle:
                    return new ComponentSettings() { Bold = 1, StyleName = "Normal", SpaceAfter = 10 };
                case ReportComponentType.Text:
                    return new ComponentSettings() { SpaceAfter = 10, StyleName = "Normal" };
                case ReportComponentType.List:
                    return new ComponentSettings();
                case ReportComponentType.Table:
                    return new ComponentSettings() { SpaceAfter = 6, SpaceBefore = 6 };
                case ReportComponentType.Reference:
                    return new ComponentSettings();
                case ReportComponentType.Body:
                    return new ComponentSettings();
                default:
                    return new ComponentSettings();
            }
        }
    }
}

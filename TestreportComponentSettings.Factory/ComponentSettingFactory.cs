using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestReport.Components;
using TestreportComponent.Settings;
using TestReportDocument;

namespace TestreportComponentSettings.Factory
{
    public static class ComponentSettingFactory
    {
        public static ITestReportComponentSettings GetComponentSetting(TestreportComponentType type)
        {
            switch (type)
            {
                case TestreportComponentType.Null:
                    return new TextSettings();
                case TestreportComponentType.Header:
                    return new TextSettings() { Bold = 1, SpaceAfter = 10, StyleName = "Heading 1" };
                case TestreportComponentType.Subtitle:
                    return new TextSettings() { Bold = 1, StyleName = "Normal", SpaceAfter = 10 }; ;
                case TestreportComponentType.Text:
                    return new TextSettings() { SpaceAfter = 10, StyleName = "Normal" };
                case TestreportComponentType.List:
                    return new TextSettings();
                case TestreportComponentType.Table:
                    return new TableSettings();
                case TestreportComponentType.Reference:
                    return new TextSettings();
                default:
                    return new TextSettings();
            }
        }
    }
}

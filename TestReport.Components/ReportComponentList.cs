using System.Collections.Generic;
using Component.Settings;


namespace Report.Components
{
    public class ReportComponentList : ReportComponentBase
    {
        public override ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.List;

        public override ComponentSetting Settings { get; set; } = new ComponentSetting();

        public List<string> Text { get; set; } = new List<string>();
    }

}
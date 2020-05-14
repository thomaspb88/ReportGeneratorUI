using Component.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Components
{
    public class ReportComponentBase : IReportComponent
    {
        public virtual ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.Default;
        public virtual ComponentSetting Settings { get; set; } = new ComponentSetting();
    }
}

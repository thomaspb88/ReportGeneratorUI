using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestreportComponent.Settings
{
    public interface ITestReportComponentSettings
    {
        int Bold { get; set; } 

        int Italic { get; set; }

        int SpaceAfter { get; set; }

        int SpaceBefore { get; set; }

        string StyleName { get; set; }
    }
}

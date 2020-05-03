using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReportDocument
{
    public class TableSettings
    {
        public int Bold { get; set; } = 0;

        public int Italic { get; set; } = 0;

        public int SpaceAfter { get; set; } = 6;

        public int SpaceBefore { get; set; } = 6;

        public string StyleName { get; set; } = "Normal";
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using TestreportComponent.Settings;
using TestReportDocument;

namespace TestReportDocument
{
    public class TextSettings : ITestReportComponentSettings
    {

        public int Bold { get; set; } = 0;

        public int Italic { get; set; } = 0;

        public int SpaceAfter { get; set; } = 10;

        public string StyleName { get; set; } = "Normal";
        public int SpaceBefore { get; set; } = 10; 
    }
}

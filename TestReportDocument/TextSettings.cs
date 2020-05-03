using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using TestReportDocument;

namespace TestReportDocument
{
    public class TextSettings
    {

        public int Bold { get; set; } = 0;

        public int Italic { get; set; } = 0;

        public int SpaceAfter { get; set; } = 10;

        public string StyleName { get; set; } = "Normal";


    }
}

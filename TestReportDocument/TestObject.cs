using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReportDocument
{
    public class TestObject
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public List<string> TableTitles { get; set; }
        public int TableColumnCount { get; set; }
        public List<string> FurtherInfo { get; set; }
        public string Reference { get; set; }
    }
}

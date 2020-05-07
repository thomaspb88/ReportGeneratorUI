using System.Collections.Generic;
using ReportComponent.Settings;

namespace Report.Components
{
    public class ReportComponentTable : IReportComponent
    {

        public ReportComponentType TypeOfComponent { get; set; } = ReportComponentType.Table;

        public List<string> Titles { get; set; } = new List<string>();

        public int ColumnCount 
        { 
            get 
            {
                return Titles.Count;
            }
            private set 
            { 
            } 
        }

        public ComponentSettings Settings { get; set; } = new TextSettings();
    }
}
using Report.Components;
using System.Collections.Generic;

namespace ReportItemReader.Interface
{
    public interface IReportItemReader
    {
        List<ReportComponentBody> GetAllTestreportItems();
        ReportComponentBody GetByName(string testName);
        void Load(string directoryPath);
        ReportItemReaderState Status { get; set; }
        string Directory { get; }
    }

    public enum ReportItemReaderState
    {
        Unknown,
        Intialised,
        Loaded
    }
}

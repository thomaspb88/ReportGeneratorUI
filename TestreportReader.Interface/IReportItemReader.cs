using System.Collections.Generic;
using Report.Components;

namespace ReportItemReader.Interface
{
    public interface IReportItemReader
    {
        List<ReportComponentBody> GetAllTestreportItems();
        ReportComponentBody GetByName(string testName);
        void LoadFromDirectory(string directoryPath);
        TestreportItemReaderState Status { get; set; }
        string Directory { get; }
    }

    public enum TestreportItemReaderState
    {
        Unknown,
        Intialised,
        Loaded
    }
}

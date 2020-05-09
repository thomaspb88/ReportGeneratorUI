using Report.Components;
using System.Collections.Generic;

namespace ReportItemReader.Interface
{
    public interface IReportItemReader
    {
        List<ReportComponentBody> GetAllReportItems();
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

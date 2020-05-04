using System.Collections.Generic;
using TestReport.Components;

namespace TestReportItemReader.Interface
{
    public interface ITestreportItemReader
    {
        List<TestReportComponentBody> GetAllTestreportItems();
        TestReportComponentBody GetByName(string testName);
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

using System.Collections.Generic;

namespace TestReportItemReader.Interface
{
    public interface ITestreportItemReader
    {
        List<TestreportItem> GetAllTestReportItems();
        TestreportItem GetByName(string testName);
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

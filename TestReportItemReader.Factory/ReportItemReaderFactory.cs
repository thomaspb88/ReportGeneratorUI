using System;
using System.Configuration;
using ReportItemReader.Interface;

namespace ReportItemReader.Factory
{
    public static class ReportItemReaderFactory
    {
        public static IReportItemReader GetRepository()
        {
            string repoType = ConfigurationManager.AppSettings["repsoitoryType"];
            Type repositoryType = Type.GetType(repoType);
            object repository = Activator.CreateInstance(repositoryType);
            IReportItemReader testreportItemRepository = repository as IReportItemReader;
            return testreportItemRepository;
        }
    }
}

using System;
using System.Configuration;
using TestReportItemReader.Interface;

namespace TestReportItemReader.Factory
{
    public static class TestreportItemReaderFactory
    {
        public static ITestreportItemReader GetRepository()
        {
            string repoType = ConfigurationManager.AppSettings["RepositoryType"];
            Type repositoryType = Type.GetType(repoType);
            object repository = Activator.CreateInstance(repositoryType);
            ITestreportItemReader testreportItemRepository = repository as ITestreportItemReader;
            return testreportItemRepository;
        }
    }
}

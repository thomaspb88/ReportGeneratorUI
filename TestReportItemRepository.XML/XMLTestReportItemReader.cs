using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using TestReport.Components;
using TestreportComponent.Factory;
using TestReportItemReader.Interface;
using TestReportItemRepository.XML;

namespace TestReportItemReader.XML
{

    public class XMLTestReportItemReader : ITestreportItemReader
    {
        private readonly XmlDocument xmlDocument = new XmlDocument();

        private TestreportItemReaderState status;

        public TestreportItemReaderState Status
        {
            get { return status; }
            set { status = value; }
        }

        private string _directory;

        public string Directory
        {
            get { return _directory; }
            private set 
            { 
                _directory = value; 
                if (String.IsNullOrWhiteSpace(value) || !File.Exists(value))
                {
                    this.status = TestreportItemReaderState.Unknown;
                }
                else
                {
                    this.status = TestreportItemReaderState.Intialised;
                }
            }
        }
        
        public void LoadFromDirectory(string directoryPath)
        {
            Directory = directoryPath;

            if (this.Status != TestreportItemReaderState.Intialised)
            {
                throw new Exception($"Unable to load file from {directoryPath}");
            }

            this.Status = TestreportItemReaderState.Loaded;

            xmlDocument.Load(Directory);
        }


        public TestreportItem GetByName(string testName)
        {
            if (string.IsNullOrEmpty(testName)) { throw new ArgumentNullException("testName"); }

            var testReportItem = new TestreportItem();

            try
            {
                xmlDocument.Load(Directory);

                testReportItem = (from node in xmlDocument.DocumentElement.ChildNodes.Cast<XmlNode>()
                                  where node.Name == "TestReportItem" && node.SelectSingleNode("Title").InnerText == testName
                                  select GetTesteportItem(node)).First();
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                testReportItem = new TestreportItem() { Title = $"Error - { testName } something went wrong trying to read this object in the test report" };
            }

            return testReportItem;
        }

        /// <summary>
        /// Gets all TestReportItems from repository
        /// </summary>
        /// <returns>List of TestReportItem objects</returns>

        public List<TestreportItem> GetAllTestreportItems()
        {
            List<TestreportItem> testReportItemList = new List<TestreportItem>();

            try
            {
                xmlDocument.Load(Directory);
                foreach(XmlNode node in xmlDocument.DocumentElement.ChildNodes)
                {
                    var testReportItem = GetTesteportItem(node);

                    testReportItemList.Add(testReportItem);
                }

                return testReportItemList;
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                throw ex;
            }
        }

        public TestreportItem GetTesteportItem(XmlNode node)
        {
            try
            {
                var testReportItem = new TestreportItem();

                var testReportItemType = node.Attributes["type"].Value;

                testReportItem.reportItemType = Enum.TryParse(testReportItemType, out ReportItemType reportItemType) ? reportItemType : ReportItemType.Null;


                foreach ( XmlNode childNode in node.ChildNodes)
                {
                    ITestReportComponent reportComponent = TestReportComponentFactory.GetComponentFromXmlNode(childNode);

                    ITestReportComponent intailisedTestReportComponent = PopulateTestreportComponent.ParseXmlNode(childNode, ref reportComponent);

                    testReportItem.ListOfComponents.Add(intailisedTestReportComponent);
                }

                return testReportItem;
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                throw ex;
            }
        }

    }

}

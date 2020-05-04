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


        public TestReportComponentBody GetByName(string testName)
        {
            if (string.IsNullOrEmpty(testName)) { throw new ArgumentNullException("testName"); }

            var testReportItem = new TestReportComponentBody();

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

                testReportItem = new TestReportComponentBody() { Title = $"Error - { testName } something went wrong trying to read this object in the test report" };
            }

            return testReportItem;
        }

        /// <summary>
        /// Gets all TestReportItems from repository
        /// </summary>
        /// <returns>List of TestReportItem objects</returns>

        public List<TestReportComponentBody> GetAllTestreportItems()
        {
            List<TestReportComponentBody> testReportItemList = new List<TestReportComponentBody>();

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

        public TestReportComponentBody GetTesteportItem(XmlNode node)
        {
            try
            {
                var nodeAttribute = node.Attributes["type"].Value;

                var componentType = Enum.TryParse(nodeAttribute, out TestreportComponentType reportItemType) ? reportItemType : TestreportComponentType.Null;

                if (componentType == TestreportComponentType.Body)
                {
                    var testReportBody = new TestReportComponentBody();

                    var nodeType = node.Attributes["type"].Value;

                    var reportType = Enum.TryParse(nodeType, out TestreportComponentType bodyType) ? bodyType : TestreportComponentType.Null;

                    foreach ( XmlNode childNode in node.ChildNodes)
                    {

                        if (componentType == TestreportComponentType.Body) 
                        {
                            return GetTesteportItem(childNode);
                        };

                        ITestReportComponent reportComponent = TestReportComponentFactory.GetComponentFromXmlNode(childNode);

                        ITestReportComponent intailisedTestReportComponent = PopulateTestreportComponent.ParseXmlNode(childNode, ref reportComponent);

                        testReportBody.ListOfComponents.Add(intailisedTestReportComponent);
                    }
                }

                

                return testReportBody;
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                throw ex;
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using TestReportItemRepository.Interface;

namespace TestReportItemRepository.XML
{

    public class XMLTestReportItemRepository : ITestReportItemRepository
    {
        private TestReportItemRepositoryState status;

        public TestReportItemRepositoryState Status
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
                    this.Status = TestReportItemRepositoryState.Unknown;
                }
                else
                {
                    this.Status = TestReportItemRepositoryState.Intialised;
                }
            }
        }

        /// <summary>
        /// Gets a TestReportItem from the repository by its name
        /// </summary>
        /// <param name="testName"></param>
        /// <returns></returns>
        
        public void LoadFromDirectory(string directoryPath)
        {
            Directory = directoryPath;

            if (this.Status != TestReportItemRepositoryState.Intialised)
            {
                throw new Exception($"Unable to load file from {directoryPath}");
            }

            this.Status = TestReportItemRepositoryState.Loaded;
        }


        public TestReportItem GetByName(string testName)
        {
            if (string.IsNullOrEmpty(testName)) { throw new ArgumentNullException("testName"); }

            var testReportItem = new TestReportItem();

            int depth = 0;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Directory);

                testReportItem = (from node in xmlDoc.DocumentElement.ChildNodes.Cast<XmlNode>()
                                  where node.Name == "TestReportItem" && node.SelectSingleNode("Title").InnerText == testName
                                  select ParseXmlNodeToTestReportItem(node, ref depth)).First();
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                testReportItem = new TestReportItem() { Title = $"Error - { testName } something went wrong trying to read this object in the test report" };
            }

            return testReportItem;
        }

        /// <summary>
        /// Gets all TestReportItems from repository
        /// </summary>
        /// <returns>List of TestReportItem objects</returns>
        public List<TestReportItem> GetAllTestReportItems()
        {
            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                int depth = 0;
                xmlDocument.Load(Directory);
                return xmlDocument.DocumentElement.ChildNodes.Cast<XmlNode>().Select(nodes => ParseXmlNodeToTestReportItem(nodes, ref depth)).ToList();
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                throw ex;
            }
        }

        /// <summary>
        /// Parses an XmlNode to TestReportItem object
        /// </summary>
        /// <param name="node">An XmlNode object</param>
        /// <returns>TestReportItem object</returns>
        private TestReportItem ParseXmlNodeToTestReportItem(XmlNode node, ref int recursiveDepth)
        {
            List<string> childNodes;

            TestReportItem testReportItem;

            int depth = recursiveDepth++;
            Debug.WriteLine(depth);
            
            try
            {
                if (node.ChildNodes.Count < 1) { return null; }
                childNodes = node.ChildNodes.Cast<XmlNode>().Select(n => n.Name).ToList();
                bool titleNodeExists = childNodes.Contains("Title");
                bool subtitleNodeExists = childNodes.Contains("SubTitle");
                bool tableTitleNodeExists = childNodes.Contains("TableTitles");
                bool furtherInfoExists = childNodes.Contains("FurtherInfo");
                bool referenceExists = childNodes.Contains("Reference");
                bool testStandardsExist = childNodes.Contains("TestStandards");
                TestReportItemType typeOfTestReportItem = node.Name == "TestReportTestStandard" ? TestReportItemType.TestStandard : node.Name == "TestReportProductStandard" ? TestReportItemType.ProductStandard : TestReportItemType.Null;

                testReportItem = new TestReportItem()
                {
                    Title = titleNodeExists ? node.SelectSingleNode("Title").InnerText : string.Empty,
                    SubTitle = subtitleNodeExists ? node.SelectSingleNode("SubTitle").InnerText : string.Empty,
                    TableTitles = tableTitleNodeExists ? node.SelectSingleNode("TableTitles").Cast<XmlNode>().Select(n => n.InnerText).ToList() : new List<string>(),
                    TableColumnCount = node.ChildNodes.Cast<XmlNode>().Where( n => n.Name == "TableTitles").Count(),
                    FurtherInfo = furtherInfoExists ? node.SelectSingleNode("FurtherInfo").Cast<XmlNode>().Select(n => n.InnerText).ToList() : null,
                    Reference = referenceExists ? node.SelectSingleNode("Reference").InnerText : "Missing Reference",
                    HasAdditionalInformation = furtherInfoExists,
                    HasTable = tableTitleNodeExists,
                    ItemType = typeOfTestReportItem
                };

                testReportItem.TestReportItems = testStandardsExist ? node.SelectSingleNode("TestStandards").Cast<XmlNode>().Select(n => ParseXmlNodeToTestReportItem(n, ref depth)).ToList() : null;
                depth--;
                Debug.WriteLine(depth);
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                testReportItem = new TestReportItem() {
                    Title = $"Error Occured - { node.Name } something went wrong trying to read this object in the directory.",
                };
            }

            return testReportItem;
        }

    }

}

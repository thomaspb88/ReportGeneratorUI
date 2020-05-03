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

        public Func<ITestReportComponent, XmlNode, ITestReportComponent> parseText = (t, x) =>
        {
            var tr = (TestReportComponentText)t;
            tr.Text = x.InnerText;
            return tr;
        };

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
                    this.Status = TestreportItemReaderState.Unknown;
                }
                else
                {
                    this.Status = TestreportItemReaderState.Intialised;
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

                testReportItem = new TestreportItem() { Title = $"Error - { testName } something went wrong trying to read this object in the test report" };
            }

            return testReportItem;
        }

        /// <summary>
        /// Gets all TestReportItems from repository
        /// </summary>
        /// <returns>List of TestReportItem objects</returns>
        public List<TestreportItem> GetAllTestReportItems()
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

        public List<TestreportItem> GetAllTestReportItemsExp()
        {
            List<TestreportItem> testReportItemList = new List<TestreportItem>();

            try
            {
                xmlDocument.Load(Directory);
                foreach(XmlNode node in xmlDocument.DocumentElement.ChildNodes)
                {
                    var testReportItem = GetTestReportItem(node);

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

        public TestreportItem GetTestReportItem(XmlNode node)
        {
            try
            {
                var testReportItem = new TestreportItem();

                var testReportItemType = node.Attributes["type"].Value;

                testReportItem.reportItemType = Enum.TryParse(testReportItemType, out ReportItemType reportItemType) ? reportItemType : ReportItemType.Null;


                foreach ( XmlNode testReportItemNode in node.ChildNodes)
                {
                    ITestReportComponent reportComponent = TestReportComponentFactory.GetComponent(testReportItemNode);

                    testReportItem.ListOfComponents.Add(PopulateTestreportComponent.ParseXmlNode(testReportItemNode, ref reportComponent));
                }

                return testReportItem;
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
        private TestreportItem ParseXmlNodeToTestReportItem(XmlNode node, ref int recursiveDepth)
        {
            List<string> childNodes;

            TestreportItem testReportItem;

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

                testReportItem = new TestreportItem()
                {
                    Title = titleNodeExists ? node.SelectSingleNode("Title").InnerText : string.Empty,
                    SubTitle = subtitleNodeExists ? node.SelectSingleNode("SubTitle").InnerText : string.Empty,
                    TableTitles = tableTitleNodeExists ? node.SelectSingleNode("TableTitles").Cast<XmlNode>().Select(n => n.InnerText).ToList() : new List<string>(),
                    TableColumnCount = node.ChildNodes.Cast<XmlNode>().Where( n => n.Name == "TableTitles").Count(),
                    FurtherInfo = furtherInfoExists ? node.SelectSingleNode("FurtherInfo").Cast<XmlNode>().Select(n => n.InnerText).ToList() : null,
                    Reference = referenceExists ? node.SelectSingleNode("Reference").InnerText : "Missing Reference",
                    HasAdditionalInformation = furtherInfoExists,
                    HasTable = tableTitleNodeExists,
                };

                testReportItem.TestReportItems = testStandardsExist ? node.SelectSingleNode("TestStandards").Cast<XmlNode>().Select(n => ParseXmlNodeToTestReportItem(n, ref depth)).ToList() : null;
                depth--;
                Debug.WriteLine(depth);
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                testReportItem = new TestreportItem() {
                    Title = $"Error Occured - { node.Name } something went wrong trying to read this object in the directory.",
                };
            }

            return testReportItem;
        }

    }

}

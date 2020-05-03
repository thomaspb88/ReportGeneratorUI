using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using TestReport.Components;
using TestreportComponent.Factory;
using TestReportItemReader.Interface;

namespace TestReportItemReader.XML
{

    public class XMLTestReportItemReader : ITestreportItemReader
    {
        private XmlDocument xmlDocument = new XmlDocument();

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

        /// <summary>
        /// Gets a TestReportItem from the repository by its name
        /// </summary>
        /// <param name="testName"></param>
        /// <returns></returns>
        
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
            XmlDocument xmlDocument = new XmlDocument();

            List<TestreportItem> testReportItemList = new List<TestreportItem>();

            try
            {
                xmlDocument.Load(Directory);
                foreach(XmlNode node in xmlDocument.DocumentElement.ChildNodes)
                {
                    testReportItemList.Add(WalkXmlTree(node));
                }

                return testReportItemList;
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                throw ex;
            }
        }

        public TestreportItem WalkXmlTree(XmlNode node)
        {
            try
            {
                var testReportItem = new TestreportItem();

                var testReportItemType = node.Attributes["type"].Value;

                testReportItem.reportItemType = Enum.TryParse(testReportItemType, out ReportItemType reportItemType) ? reportItemType : ReportItemType.Null;


                foreach ( XmlNode testReportItemNode in node.ChildNodes)
                {
                    ITestReportComponent reportComponent = TestReportComponentFactory.GetComponent(testReportItemNode);

                    reportComponent.ParseXmlNode(testReportItemNode);

                    testReportItem.ListOfComponents.Add(reportComponent);
                }

                return testReportItem;
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                throw ex;
            }
        }

        private object ParseTestReportItem(XmlNode node)
        {

            //TODO Extract this function to seperate file. Rename it a factory

            //TODO Convert to a dynamic factory

            //TODO In Xml File, use type attribute to dynamically load instance

            //TODO Think about how the responsibility of parsing XML nodes to objects.

            //TODO Add application setting with list of Types. This will allow extensibility.

            //if (node.NodeType == XmlNodeType.Element)
            //{
            //    switch (node.Name)
            //    {
            //        case "Title":
            //            var title = new TestReportComponentText() { TypeOfComponent = TestreportComponentType.Header };
            //            title.Text = node.InnerText;
            //            return title;
            //        case "SubTitle":
            //            var subtitle = new TestReportComponentText() { TypeOfComponent = TestreportComponentType.Subtitle };
            //            subtitle.Text = node.InnerText;
            //            return subtitle;
            //        case "Paragrapgh":
            //            var paragraph = new TestReportComponentText() { TypeOfComponent = TestreportComponentType.Paragrapgh };
            //            paragraph.Text = node.InnerText;
            //            return paragraph;
            //        case "BulletList":
            //            var list = new TestReportComponentList();
            //            if (node.HasChildNodes)
            //            {
            //                var listOfText = node.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
            //                list.Text = listOfText.ToList();
            //            }
            //            return list;
            //        case "Table":
            //            var table = new TestReportComponentTable();
            //            if (node.HasChildNodes)
            //            {
            //                var listOfText = node.ChildNodes.Cast<XmlNode>().Select(n => n.InnerText);
            //                table.Titles = listOfText.ToList();
            //            }
            //            return table;
            //        case "Reference":
            //            var reference = new TestReportComponentText() { TypeOfComponent = TestreportComponentType.Reference };
            //            reference.Text = node.InnerText;
            //            return reference;
            //        case "Body":
            //            return WalkXmlTree(node);
            //        default:
            //            return null;
            //    }
            //}

            return null;

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

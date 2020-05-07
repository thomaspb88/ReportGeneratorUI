using Report.Components;
using ReportItemReader.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using ReportComponent.Factory;
using ReportComponent.Settings;
using ReportComponentSettings.Factory;

namespace ReportItemReader.XML
{

    public class XMLReportItemReader : IReportItemReader
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


        public ReportComponentBody GetByName(string testName)
        {
            if (string.IsNullOrEmpty(testName)) { throw new ArgumentNullException("testName"); }

            var testReportItem = new ReportComponentBody();

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

                testReportItem = new ReportComponentBody() { Title = $"Error - { testName } something went wrong trying to read this object in the test report" };
            }

            return testReportItem;
        }

        /// <summary>
        /// Gets all TestReportItems from repository
        /// </summary>
        /// <returns>List of TestReportItem objects</returns>

        public List<ReportComponentBody> GetAllTestreportItems()
        {
            List<ReportComponentBody> testReportItemList = new List<ReportComponentBody>();

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

        public ReportComponentBody GetTesteportItem(XmlNode node)
        {
            try
            {
                var nodeAttribute = node.Attributes["type"].Value;

                var componentType = Enum.TryParse(nodeAttribute, out ReportComponentType reportItemType) ? reportItemType : ReportComponentType.Null;

                if (componentType == ReportComponentType.Body)
                {
                    var testReportBody = new ReportComponentBody();

                    var nodeType = node.Attributes["type"].Value;

                    var reportType = Enum.TryParse(nodeType, out ReportComponentType bodyType) ? bodyType : ReportComponentType.Null;

                    foreach ( XmlNode childNode in node.ChildNodes)
                    {

                        if (componentType == ReportComponentType.Body) 
                        {
                            return GetTesteportItem(childNode);
                        };

                        IReportComponent reportComponent = ReportComponentFactory.GetComponentFromXmlNode(childNode);

                        //IReportComponent intailisedTestReportComponent = PopulateReportComponent.ParseXmlNode(childNode, ref reportComponent);

                        reportComponent.Settings = ComponentReader.GetSetting(reportComponent);

                        testReportBody.ListOfComponents.Add(reportComponent);
                    }
                }

                return null;
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(string.Format("XmlException for test name: {0}", ex.Message));

                throw ex;
            }
        }

    }

}

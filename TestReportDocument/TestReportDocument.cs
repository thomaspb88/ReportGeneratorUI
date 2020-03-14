using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TestReportDocument
{
    public class TestReportDocument
    {
        /* The stypical structure of a test report:
        Front Page
        Contents
        Intro
        Specimens
        Tests
        References             
        */

        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public IEnumerable<string> TestTitles { get; set; }
        public List<TestObject> ListOfTestsStandards { get; set; }

        private Application wordApp = new Application { Visible = false };
        private Document _doc;

        #region Constructor
        /// <summary>
        /// A basic constructor that requires a document path on instantiating
        /// </summary>
        /// <param name="filePath"></param>

        public TestReportDocument(string filePath, List<TestObject> testObjects)

        {
            _doc = wordApp.Documents.Open(filePath, ReadOnly: false, Visible: false);
            ListOfTestsStandards.Add(new TestObject()
            {
                Reference = "test",
                FurtherInfo = new List<string>() { "Further Info", "More info" },
                SubTitle = "A Test",
                Title = "Test Test",
                TableColumnCount = 4,
                TableTitles = new List<string>() { "Hello", "World"}
            });

        }

        #endregion

        public void ReplaceWord(string oldWord, string newWord)
        {
            object Unknown = Type.Missing;
            this._doc.Activate();
            FindAndReplace(oldWord, newWord);
        }

        #region Save As
        /// <summary>
        /// Saves the document as a copy avoiding overriding the original
        /// </summary>
        /// <param name="saveDirectory"></param>
        /// <param name="fileName"></param>
        public void SaveAs(string saveDirectory, string fileName)
        {
            object FileFormat = WdSaveFormat.wdFormatDocumentDefault;
            this._doc.SaveAs2(Path.Combine(saveDirectory, fileName), ref FileFormat);
        }

        #endregion

        public void AddTestsToDocument()
        {
            foreach (var test in ListOfTestsStandards)
            {
    
                //Loop through columns and add titles.
                for(var i = 0; i< test.TableColumnCount; i++)
                {
                   
                }
            }



        }

        #region Close
        public void Close()
        {
            object SaveChanges = false;
            this._doc.Close(SaveChanges);
        }
        #endregion

        #region Find and Replace
        /// <summary>
        /// Finds a string in the documents header, body and footer and replaces it
        /// </summary>
        /// <param name="targetString"></param>
        /// <param name="replacementText"></param>
        private void FindAndReplace(object targetString, object replacementText)
        {
            //options
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            //execute find and replace

            foreach(Section section in _doc.Sections)
            {
                wordApp.Selection.Find.Execute(ref targetString, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replacementText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
                
                HeadersFooters headers = section.Headers;

                foreach(HeaderFooter header in headers)
                {
                    Range headerRange = header.Range;
                    headerRange.Find.Execute(ref targetString, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replacementText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
                }

                HeadersFooters footers = section.Footers;

                foreach(HeaderFooter footer in footers)
                {
                    Range footerRange = footer.Range;
                    footerRange.Find.Execute(ref targetString, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replacementText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
                }
            }
        }
        #endregion

    }
}

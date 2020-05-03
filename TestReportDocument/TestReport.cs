using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestReportItemReader.Interface;
using System.Configuration;

namespace TestReportDocument
{
    public class TestReport
    {
        /* The stypical structure of a test report template:
        Front Page
        Contents
        Intro
        Specimens
        Tests*
        References*
        
        * Sections produced by software
        
        */

        TableSettings tableSettings = new TableSettings();
        TextSettings headerSettings = new TextSettings() { Bold = 1, SpaceAfter = 10, StyleName = "Heading 1" };
        TextSettings subtitleSettings = new TextSettings() { Bold = 1, StyleName = "Normal", SpaceAfter = 10 };
        TextSettings textSettings = new TextSettings() { SpaceAfter = 10, StyleName = "Normal" };
        TextSettings bulletpointSettings = new TextSettings();

        public IEnumerable<TestreportItem> ListOfTestsReportItems { get; set; }
        private Application wordApp;
        private Document _wordDoc;

        object missing = System.Reflection.Missing.Value;
        object endOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

        private bool _isDocumentOpen { get; set; } = false;

        public void OpenDocument(string docTemplateFilePath)
        {
            if (this._isDocumentOpen)
            {
                this._wordDoc = wordApp.Documents.Open(docTemplateFilePath, ReadOnly: false, Visible: true);
            }
            else
            {
                this.wordApp = new Application { Visible = true };
                this._wordDoc = wordApp.Documents.Open(docTemplateFilePath, ReadOnly: false, Visible: true);
                this._isDocumentOpen = true;
            }
        }

        public void LoadReportItems(IEnumerable<TestreportItem> testStandardObjects)
        {
            this.ListOfTestsReportItems = testStandardObjects;
        }

        /// <summary>
        /// Appends test report items to the test report template
        /// </summary>
        /// <param name="testReportItem"></param>
        public void AppendTestReportItem(TestreportItem testReportItem)
        {
            if (_isDocumentOpen)
            {
                AppendHeading(testReportItem.Title);
                AppendSubTitle(testReportItem.SubTitle);
                if (testReportItem.HasTable)
                {
                    AppendTable(testReportItem.TableTitles);
                }

                if (testReportItem.HasAdditionalInformation)
                {
                    AppendBulletPointList(testReportItem.FurtherInfo);
                }
                if (testReportItem.TestReportItems != null) 
                {
                    testReportItem.TestReportItems.ForEach(testItem => AppendTestReportItem(testItem));
                    
                }
            }
        }

        public void AppendAllTests()
        {
            ListOfTestsReportItems.ToList().ForEach(testItem => AppendTestReportItem(testItem));
        }

        private void AppendParagraph(string text, TextSettings settings)
        {
            bool isInputValid = !string.IsNullOrWhiteSpace(text);

            if (isInputValid)
            {
                Paragraph para;
                object rng = _wordDoc.Bookmarks.get_Item(ref endOfDoc).Range;
                para = _wordDoc.Content.Paragraphs.Add(ref rng);
                para.Range.Text = text;
                para.Range.Font.Bold = settings.Bold;
                para.Range.Font.Italic = settings.Italic;
                para.Format.SpaceAfter = settings.SpaceAfter;
                para.set_Style(_wordDoc.Styles[settings.StyleName]);
                para.Range.InsertParagraphAfter();
            }
        }

        /// <summary>
        /// Appends a Heading to a test report word document template
        /// </summary>
        /// <param name="text"></param>
        private void AppendHeading(string text)
        {
            bool isInputValid = !string.IsNullOrWhiteSpace(text);

            if (isInputValid)
            {
                AppendParagraph(text, headerSettings);
            }
        }

        /// <summary>
        /// Appends a SubTitle to a test report word document template
        /// </summary>
        /// <param name="text"></param>
        private void AppendSubTitle(string text)
        {
            bool isInputValid = !string.IsNullOrWhiteSpace(text);
            if (isInputValid) 
            {
                AppendParagraph(text, subtitleSettings);
            }

        }

        /// <summary>
        /// Appends a Paragraph to a test report word document template
        /// </summary>
        /// <param name="text"></param>
        private void AppendText(string text)
        {
            bool isInputValid = !string.IsNullOrWhiteSpace(text);
            if (isInputValid)
            {
                AppendParagraph(text, textSettings);
            }
        }

        /// <summary>
        /// Appends a table to a test report word document template
        /// </summary>
        /// <param name="numberOfColumns">An positive and non-zero integer</param>
        /// <param name="tableTitles">An array of strings</param>
        private void AppendTable(List<string> coloumnTitles)
        {
            Range endOfDocRange = _wordDoc.Bookmarks.get_Item(ref endOfDoc).Range;
            Table table = _wordDoc.Tables.Add(endOfDocRange, 2, coloumnTitles.Count, ref missing, ref missing);
            table.Range.ParagraphFormat.SpaceAfter = tableSettings.SpaceAfter;

            //Add titles to top row of table
            for (int c = 1; c <= coloumnTitles.Count; c++)
            {
                table.Cell(1, c).Range.Text = coloumnTitles[c - 1];
                table.Cell(1, c).Range.ParagraphFormat.SpaceAfter = tableSettings.SpaceAfter;
                table.Cell(1, c).Range.ParagraphFormat.SpaceBefore = tableSettings.SpaceBefore;
                table.Cell(1, c).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                table.Cell(1, c).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            }
            table.Rows[1].Range.Font.Bold = tableSettings.Bold;
            table.Rows[1].Range.Font.Italic = tableSettings.Italic;
        }

        /// <summary>
        /// Appends a bullet point list to a word document template
        /// </summary>
        /// <param name="testInformation">An array of strings</param>
        private void AppendBulletPointList(List<string> testInformation)
        {
            bool isInputValid = testInformation.Count > 0;

            if (isInputValid)
            {
                object endOfDocRange = _wordDoc.Bookmarks.get_Item(ref endOfDoc).Range;
                Paragraph para = _wordDoc.Content.Paragraphs.Add(ref endOfDocRange);

                para.Range.ListFormat.ApplyBulletDefault();
                para.Format.SpaceAfter = bulletpointSettings.SpaceAfter; para.Range.Font.Italic = bulletpointSettings.Italic;
                para.Range.Font.Bold = bulletpointSettings.Bold;
                para.set_Style(_wordDoc.Styles[bulletpointSettings.StyleName]);

                testInformation.ForEach(t => para.Range.InsertBefore(t));

            }
            
        }

        /// <summary>
        /// Appends references to the last page of a word document template
        /// </summary>
        public void CreateReferencesPage()
        {
            //Move to new Page
            _wordDoc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);
            //Title Reference
            AppendHeading("References");

            //List of references
            object endOfDocRange = _wordDoc.Bookmarks.get_Item(ref endOfDoc).Range;
            Paragraph para = _wordDoc.Content.Paragraphs.Add(ref endOfDocRange);
            para.Range.ListFormat.ApplyNumberDefault();
            para.Range.Font.Bold = textSettings.Bold;

            //Test object reference property/s here
            var listOfReferences = ListOfTestsReportItems.Where(test => test.Reference != string.Empty).Select(test => test.Reference);
            var referenceArray = listOfReferences.ToList();

            for (var i = 0; i < listOfReferences.Count(); i++)
            {
                if(i != listOfReferences.Count()-1)
                {
                    para.Range.InsertBefore(referenceArray[i] + "\n");
                }
                else
                {
                    para.Range.InsertBefore(referenceArray[i]);
                }
            }
        }

        public void ReplaceWord(string oldWord, string newWord)
        {
            this._wordDoc.Activate();
            FindAndReplace(oldWord, newWord);
        }

        /// <summary>
        /// Saves a copy of the word document template
        /// </summary>
        /// <param name="wordTemplateFilePath"></param>
        /// <param name="wordTemplateFileName"></param>
        public void SaveAs(string wordTemplateFilePath, string wordTemplateFileName)
        {
            object FileFormat = WdSaveFormat.wdFormatDocumentDefault;
            this._wordDoc.SaveAs2(Path.Combine(wordTemplateFilePath, wordTemplateFileName), ref FileFormat);
        }

        /// <summary>
        /// Closes this word document template
        /// </summary>
        public void CloseApplication()
        {
            object SaveChanges = false;

            if (_isDocumentOpen)
            {
                this._wordDoc.Close(SaveChanges);
                this._isDocumentOpen = false;
            }
        }

        /// <summary>
        /// A helper method that finds a specified string in the document (header, body and footer) and replaces it
        /// </summary>
        /// <param name="oldString">The exisitng text</param>
        /// <param name="newString">The replacement text</param>
        private void FindAndReplace(object oldString, object newString)
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

            foreach(Section section in _wordDoc.Sections)
            {
                wordApp.Selection.Find.Execute(ref oldString, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref newString, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
                
                HeadersFooters headers = section.Headers;

                foreach(HeaderFooter header in headers)
                {
                    Range headerRange = header.Range;
                    headerRange.Find.Execute(ref oldString, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref newString, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
                }

                HeadersFooters footers = section.Footers;

                foreach(HeaderFooter footer in footers)
                {
                    Range footerRange = footer.Range;
                    footerRange.Find.Execute(ref oldString, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref newString, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
                }
            }
        }
    
    }
}

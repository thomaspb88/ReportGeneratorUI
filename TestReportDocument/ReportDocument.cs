using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.IO;
using System;
using Report.Components;

namespace Report.Document
{
    public class ReportDocument
    {
        public IEnumerable<ReportComponentBody> ListOfTestsReportItems { get; set; }
        private Application wordApp;
        private Document _wordDoc;
        object missing = System.Reflection.Missing.Value;
        object endOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

        private bool IsDocumentOpen { get; set; } = false;

        public void OpenDocument(string docTemplateFilePath)
        {
            if (this.IsDocumentOpen)
            {
                this._wordDoc = wordApp.Documents.Open(docTemplateFilePath, ReadOnly: false, Visible: true);
            }
            else
            {
                this.wordApp = new Application { Visible = true };
                this._wordDoc = wordApp.Documents.Open(docTemplateFilePath, ReadOnly: false, Visible: true);
                this.IsDocumentOpen = true;
            }
        }

        private Dictionary<ReportComponentType, Action<IReportComponent>> writeOperations
        = new Dictionary<ReportComponentType, Action<IReportComponent>>()
        { 
            { ReportComponentType.Header, WriteText },
            { ReportComponentType.Text, WriteText },
            { ReportComponentType.Reference, WriteText },
            { ReportComponentType.Subtitle, WriteText },
            { ReportComponentType.Table, WriteTable },
            { ReportComponentType.List, WriteList }
        };


        public void LoadReportItems(IEnumerable<ReportComponentBody> testreportItems)
        {
            this.ListOfTestsReportItems = testreportItems;
        }

        public void WriteReport()
        {
            foreach (var item in this.ListOfTestsReportItems)
            {
                WriteReportItemToDocument(item);            
            }
            //CreateReferencesPage()
        }

        internal void WriteReportItemToDocument(ReportComponentBody testreportItem)
        {
            foreach (var item in testreportItem.ListOfComponents)
            {
                var operation = writeOperations[item.TypeOfComponent];

                operation(item);
            };
        }

        internal static Action<IReportComponent> WriteList = (t) =>
        {
            var testreportComponentList = (ReportComponentList)t;
            var listSettings = testreportComponentList.Settings;

            object endOfDocRange = _wordDoc.Bookmarks.get_Item(ref endOfDoc).Range;
            Paragraph para = _wordDoc.Content.Paragraphs.Add(ref endOfDocRange);

            para.Range.ListFormat.ApplyBulletDefault();
            para.Format.SpaceAfter = listSettings.SpaceAfter; para.Range.Font.Italic = listSettings.Italic;
            para.Range.Font.Bold = listSettings.Bold;
            para.set_Style(_wordDoc.Styles[listSettings.StyleName]);

            testreportComponentList.Text.ForEach(item => para.Range.InsertBefore(t));
        };

        internal static Action<IReportComponent> WriteText = (t) =>
        {
            {
                var testreportComponentText = (ReportComponentText)t;
                var testSettings = testreportComponentText.Settings;

                bool isInputValid = !string.IsNullOrWhiteSpace(testreportComponentText.Text);

                if (isInputValid)
                {
                    Paragraph para;
                    object rng = _wordDoc.Bookmarks.get_Item(ref endOfDoc).Range;
                    para = _wordDoc.Content.Paragraphs.Add(ref rng);
                    para.Range.Text = testreportComponentText.Text;
                    para.Range.Font.Bold = testSettings.Bold;
                    para.Range.Font.Italic = testSettings.Italic;
                    para.Format.SpaceAfter = testSettings.SpaceAfter;
                    para.set_Style(_wordDoc.Styles[testSettings.StyleName]);
                    para.Range.InsertParagraphAfter();
                }
            }
        };

        internal static Action<IReportComponent> WriteTable = (t) =>
        {
            var testreportComponentTable = (ReportComponentTable)t;
            var tableSettings = testreportComponentTable.Settings;

            Range endOfDocRange = _wordDoc.Bookmarks.get_Item(ref endOfDoc).Range;
            Table table = _wordDoc.Tables.Add(endOfDocRange, 2, testreportComponentTable.ColumnCount, ref missing, ref missing);
            table.Range.ParagraphFormat.SpaceAfter = tableSettings.SpaceAfter;

            //Add titles to top row of table
            for (int c = 1; c <= testreportComponentTable.ColumnCount; c++)
            {
                table.Cell(1, c).Range.Text = testreportComponentTable.Titles[c - 1];
                table.Cell(1, c).Range.ParagraphFormat.SpaceAfter = tableSettings.SpaceAfter;
                table.Cell(1, c).Range.ParagraphFormat.SpaceBefore = tableSettings.SpaceBefore;
                table.Cell(1, c).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                table.Cell(1, c).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            }
            table.Rows[1].Range.Font.Bold = tableSettings.Bold;
            table.Rows[1].Range.Font.Italic = tableSettings.Italic;
        };

        //public void CreateReferencesPage()
        //{
        //    //Move to new Page
        //    _wordDoc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);
        //    //Title Reference
        //    AppendHeading("References");

        //    //List of references
        //    object endOfDocRange = _wordDoc.Bookmarks.get_Item(ref endOfDoc).Range;
        //    Paragraph para = _wordDoc.Content.Paragraphs.Add(ref endOfDocRange);
        //    para.Range.ListFormat.ApplyNumberDefault();
        //    para.Range.Font.Bold = textSettings.Bold;

        //    //Test object reference property/s here
        //    var listOfReferences = ListOfTestsReportItems.Where(test => test.Reference != string.Empty).Select(test => test.Reference);
        //    var referenceArray = listOfReferences.ToList();

        //    for (var i = 0; i < listOfReferences.Count(); i++)
        //    {
        //        if(i != listOfReferences.Count()-1)
        //        {
        //            para.Range.InsertBefore(referenceArray[i] + "\n");
        //        }
        //        else
        //        {
        //            para.Range.InsertBefore(referenceArray[i]);
        //        }
        //    }
        //}

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

            if (IsDocumentOpen)
            {
                this._wordDoc.Close(SaveChanges);
                this.IsDocumentOpen = false;
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

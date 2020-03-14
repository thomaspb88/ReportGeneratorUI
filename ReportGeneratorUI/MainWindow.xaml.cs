using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace ReportGeneratorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            object oTemplate = "C:\\Users\\Tom\\Documents\\Custom Office Templates\\TestTemplate.dotx";
            object oMissing = System.Reflection.Missing.Value;
            object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
            object oTestBookmark = "Test";
            object oTestBookmark1 = "Test1";

            //Start Word and create a new document.
            Word._Application oWord;
            Word._Document oDoc;
            oWord = new Word.Application();
            oWord.Visible = true;
            oDoc = oWord.Documents.Add(ref oTemplate, ref oMissing, ref oMissing, ref oMissing);

            object oRng = oDoc.Bookmarks.get_Item(oTestBookmark).Range;

            //Insert a paragraph at the beginning of the document.
            Word.Paragraph oPara1;
            oPara1 = oDoc.Content.Paragraphs.Add(ref oRng);
            oPara1.Range.Text = "Heading 1";
            oPara1.Range.Font.Bold = 1;
            oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter();

            for(var i = 0; i < 3; i++)
            {
                //Insert Paragraph
                Word.Paragraph oPara;
                oRng = oDoc.Content.Paragraphs[oDoc.Content.Paragraphs.Count - 1].Range;
                oPara = oDoc.Content.Paragraphs.Add(ref oRng);
                oPara.Range.Text = "Paragraph " + i;
                oPara.Range.set_Style(oDoc.Styles["emphasis"]);
                oPara.Range.Font.Bold = 1;
                oPara.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
                oPara.Range.InsertParagraphAfter();

                //Insert a 3 x 5 table, fill it with data, and make the first row
                //bold and italic.
                Word.Table oTable;
                Word.Range wrdRng = oDoc.Content.Paragraphs[oDoc.Content.Paragraphs.Count - 1].Range;
                oTable = oDoc.Tables.Add(wrdRng, 3, 5, ref oMissing, ref oMissing);
                oTable.Range.ParagraphFormat.SpaceAfter = 6;
                int r, c;
                string strText;
                for (r = 1; r <= 3; r++)
                    for (c = 1; c <= 5; c++)
                    {
                        strText = "r" + r + "c" + c;
                        oTable.Cell(r, c).Range.Text = strText;
                    }
                oTable.Rows[1].Range.Font.Bold = 1;
                oTable.Rows[1].Range.Font.Italic = 1;

                //Add some text after the table.
                Word.Paragraph oPara4;
                oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                oPara4 = oDoc.Content.Paragraphs.Add(ref oRng);
                oPara4.Range.InsertParagraphAfter();
                oPara4.Range.Text = "And here's another table:";
                oPara4.Range.ListFormat.ApplyBulletDefault();
                oPara4.Format.SpaceAfter = 24;
                oPara4.Range.InsertParagraphAfter();
            }

            //Add references here

            oDoc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);

            Word.Paragraph oPara5;
            oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oPara5 = oDoc.Content.Paragraphs.Add(ref oRng);
            oPara5.Range.ListFormat.ApplyNumberDefault();
            string[] bulletItems = new string[] { "One", "Two", "Three" };

            for (int i = 0; i < bulletItems.Length; i++)
            {
                string bulletItem = bulletItems[i];
                if (i < bulletItems.Length - 1)
                    bulletItem = bulletItem + "\n";
                oPara5.Range.InsertBefore(bulletItem);
            }

            this.Close();
        }
    
    }
}

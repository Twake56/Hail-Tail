using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLogs
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
         InitializeComponent();
         //TabControlParent.DrawItem += new DrawItemEventHandler(TabControlParent_DrawItem);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
            
        }

      

      private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
           // textBox1.
        }

      private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
      {

      }

      public string[] WriteSafeReadAllLines(String path)
      {
         using (var csv = new FileStream(openFileDialog3.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         using (var sr = new StreamReader(csv))
         {
            List<string> file = new List<string>();
            while (!sr.EndOfStream)
            {
               file.Add(sr.ReadLine());
            }

            return file.ToArray();
         }
      }
      private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var watch = new FileSystemWatcher();
                    watch.Path = Path.GetDirectoryName(openFileDialog3.FileName);
                    watch.Filter = Path.GetFileName(openFileDialog3.FileName);
                    watch.Changed += new FileSystemEventHandler(OnChanged);
                    watch.EnableRaisingEvents = true;
                    StreamReader reader = new StreamReader(openFileDialog3.FileName);
               //richTextBox1.Text = reader.ReadToEnd();
               //LogRichTextBox1.Text = reader.ReadToEnd();
                    reader.Close();
                    followTailCheckBox.Checked = true;
                    LogRichTextBox1.SelectionStart = LogRichTextBox1.Text.Length;
                    LogRichTextBox1.ScrollToCaret();

               //Creates a new tab for a new log
               TabPage tab = new TabPage() { Text = System.IO.Path.GetFileName(openFileDialog3.FileName) };
               TabControlParent.TabPages.Add(tab);
               TabControlParent.SelectedTab = tab;

               //ListBox TabBoxView = new ListBox { Parent = tab, Dock = DockStyle.Fill };
               //TabBoxView.DataSource = File.ReadAllLines(openFileDialog3.FileName);

               ListView TabViewText = new ListView { Parent = tab, Dock = DockStyle.Fill, View = View.Details };
               TabViewText.Columns.Add("Line", 50, HorizontalAlignment.Left);
               TabViewText.Columns.Add("Text", 1000, HorizontalAlignment.Left);
               TabViewText.FullRowSelect = true;

               string[] lines = WriteSafeReadAllLines(openFileDialog3.FileName);
               var ItemsCount = TabViewText.Items.Count;
               if (ItemsCount == 0 || lines.Length < ItemsCount)
               {
                  TabViewText.Items.Clear();
                  for (var linenum = 0; linenum < lines.Length; linenum++)
                  {
                     TabViewText.Items.Add((linenum + 1).ToString()).SubItems.Add(lines[linenum]);
                  }
               }

               /*RichTextBox TabTextBox = new RichTextBox { Parent = tab, Dock = DockStyle.Fill };
               TabTextBox.ReadOnly = true;
               TabTextBox.LoadFile(openFileDialog3.FileName, RichTextBoxStreamType.PlainText);*/

               //FileLengthTB.Text = TabBoxView.Lines.Length.ToString() + " lines"; //Grabs the Number of lines in a file
               long FileSizeValue = new FileInfo(openFileDialog3.FileName).Length; //Create the long for the file size value
               FileSizeTB.Text = (FileSizeValue / 1024) + " KB"; //Convert File size from bytes to KB
            }
                catch (IOException ioe)
                {
                    MessageBox.Show(ioe.Message);
                }

            }
        }
      private void OnChanged(object source, FileSystemEventArgs e)
      {
         if (TabViewText.InvokeRequired)
         {
            TabViewText.Invoke((MethodInvoker)delegate { OnChanged(source, e); });
         }
         else
         {
            //textBox1.Text = File.ReadAllText(openFileDialog3.FileName);
            try
            {
               //StreamReader reader = new StreamReader(openFileDialog3.FileName);
               string[] lines = WriteSafeReadAllLines(openFileDialog3.FileName);
               var ItemsCount = TabViewText.Items.Count;
               if (ItemsCount == 0 || lines.Length < ItemsCount)
               {
                  TabViewText.Items.Clear();
                  for (var linenum = 0; linenum < lines.Length; linenum++)
                  {
                     TabViewText.Items.Add((linenum + 1).ToString()).SubItems.Add(lines[linenum]);
                  }
               }
               else
               {
                  //var diff = lines.Length - ItemsCount;
                  for (var start = ItemsCount; start < lines.Length; start++)
                  {
                     TabViewText.Items.Add((start + 1).ToString()).SubItems.Add(lines[start]);
                  }
               }

               //LogListView.Text = reader.ReadToEnd();
               //FileLengthTB.Text = LogListView.Lines.Length.ToString() + " lines"; //Grabs the Number of lines in a file
               long FileSizeValue = new FileInfo(openFileDialog3.FileName).Length; //Create the long for the file size value
               FileSizeTB.Text = (FileSizeValue / 1024) + " KB"; //Convert File size from bytes to KB

               if (followTailCheckBox.Checked)
               {
                  // LogListView.TopItemIndex = 
               }

            }
            catch (IOException IOex)
            {
               Console.WriteLine(IOex.Message);
            }
         }
      }
   }
}

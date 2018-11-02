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
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                try {
                    var watch = new FileSystemWatcher();
                    watch.Path = Path.GetDirectoryName(openFileDialog3.FileName);
                    watch.Filter = Path.GetFileName(openFileDialog3.FileName);
                    watch.Changed += new FileSystemEventHandler(OnChanged);
                    watch.EnableRaisingEvents = true;
                    StreamReader reader = new StreamReader(openFileDialog3.FileName);
                    logRichTextBox1.Text = reader.ReadToEnd();
                    reader.Close();
                    FollowTailCheckBox.Checked = true;
                    logRichTextBox1.SelectionStart = logRichTextBox1.Text.Length;
                    logRichTextBox1.ScrollToCaret();
                }
                catch (IOException ioe)
                {
                    MessageBox.Show(ioe.Message);
                }
                
            }
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (logRichTextBox1.InvokeRequired)
            {
                logRichTextBox1.Invoke((MethodInvoker)delegate { OnChanged(source, e); });
            }
            else
            {
                //textBox1.Text = File.ReadAllText(openFileDialog3.FileName);
                try
                {
                    StreamReader reader = new StreamReader(openFileDialog3.FileName);
                    //textBox1.Text = "";
                    logRichTextBox1.Text = reader.ReadToEnd();
               FileLengthTB.Text = logRichTextBox1.Lines.Length.ToString() + " lines"; //Grabs the Number of lines in a file
               long FileSizeValue = new FileInfo(openFileDialog3.FileName).Length; //Create the long for the file size value
               FileSizeTB.Text = (FileSizeValue/1024) + " KB"; //Convert File size from bytes to KB

               if (FollowTailCheckBox.Checked)
                    {
                        var pos = this.logRichTextBox1.GetLineFromCharIndex(logRichTextBox1.SelectionStart);

                        logRichTextBox1.SelectionStart = logRichTextBox1.Text.Length;
                        logRichTextBox1.ScrollToCaret();
                    }
                    else
                    {
                        var pos = this.logRichTextBox1.GetLineFromCharIndex(logRichTextBox1.SelectionStart);
                        Console.WriteLine(pos);
                        //textBox1
                    }
                    reader.Close();
                }
                catch(IOException IOex)
                {
                    Console.WriteLine(IOex.Message);
                }
            }
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
   }
}

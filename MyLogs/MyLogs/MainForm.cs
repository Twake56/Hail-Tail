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
        public Dictionary<String, FileSystemWatcher> FileWatchers = new Dictionary<string, FileSystemWatcher>();
        public Dictionary<String, int> IndexKeeper = new Dictionary<String, int>();
        public static TabPage SelectedTabPage = new TabPage();


        public MainForm()
        {
            InitializeComponent();
        }

        private void SearchBox_Enter(object sender, EventArgs e)
        {
            if (SearchBox.Text == "Search")
            {
                SearchBox.Text = "";
                SearchBox.ForeColor = Color.Black;
            }
        }

        private void SearchBox_Leave(object sender, EventArgs e)
        {
            if (SearchBox.Text == "")
            {
                SearchBox.Text = "Search";
                SearchBox.ForeColor = Color.Gray;
            }
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        public string[] WriteSafeReadAllLines(String path)
        {
            using (var csv = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                if (FileWatchers.ContainsKey(openFileDialog3.FileName))//If file selected is already open, switch to tab and do no more
                {
                    foreach (TabPage tab in TabControlParent.TabPages)
                    {
                        if (openFileDialog3.FileName == tab.Name)
                        {
                            TabControlParent.SelectedTab = tab;
                            return;
                        }
                    }
                }
                try
                {
                    //Add a filesystem watcher to public dictionary
                    var watch = new FileSystemWatcher();
                    watch.Path = Path.GetDirectoryName(openFileDialog3.FileName);
                    watch.Filter = Path.GetFileName(openFileDialog3.FileName);
                    watch.Changed += new FileSystemEventHandler(OnChanged);
                    watch.EnableRaisingEvents = true;
                    FileWatchers.Add(openFileDialog3.FileName, watch);
                    followTailCheckBox.Checked = true;

                    //Creates a new tab for a new log
                    TabPage tab = new TabPage() { Text = System.IO.Path.GetFileName(openFileDialog3.FileName), Name = openFileDialog3.FileName };
                    TabControlParent.TabPages.Add(tab);
                    TabControlParent.SelectedTab = tab;

                    Classes.ListViewNF ListViewText = new Classes.ListViewNF { Parent = tab, Dock = DockStyle.Fill, View = View.Details };
                    ListViewText.Columns.Add("Line", 50, HorizontalAlignment.Left);
                    ListViewText.Columns.Add("Text", 1000, HorizontalAlignment.Left);
                    ListViewText.FullRowSelect = true;
                    ListViewText.ContextMenuStrip = contextMenuStrip1;
                    ListViewText.MultiSelect = true;
                    ListViewText.Name = openFileDialog3.FileName + "-ListView";//used to find listview later
                    

                    

                    //Write all lines to list view for tab
                    string[] lines = WriteSafeReadAllLines(openFileDialog3.FileName);
                    var ItemsCount = ListViewText.Items.Count;
                    if (ItemsCount == 0 || lines.Length < ItemsCount)
                    {
                        ListViewText.Items.Clear();
                        for (var linenum = 0; linenum < lines.Length; linenum++)
                        {
                            ListViewText.Items.Add((linenum + 1).ToString()).SubItems.Add(lines[linenum]);
                        }
                    }
                    try
                    {
                        ListViewText.Items[ListViewText.Items.Count - 1].EnsureVisible();
                    }
                    catch (ArgumentOutOfRangeException IndErr)
                    {
                        Console.WriteLine(IndErr.Message);
                    }

                    FileLengthTB.Text = ListViewText.Items.Count.ToString() + " lines"; //Grabs the Number of lines in a file
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
            
            TabPage EventPage = null;
            foreach (TabPage tab in TabControlParent.TabPages)
            {
                if (e.FullPath == tab.Name)
                {
                    EventPage = tab;
                }
            }
            if (EventPage == null)
            {
                FileWatchers.Remove(e.FullPath);
                return;
            }

            if (EventPage.InvokeRequired)
            {
                EventPage.Invoke((MethodInvoker)delegate { OnChanged(source, e); });
            }
            else
            {
                try
                {
                    string[] lines = WriteSafeReadAllLines(e.FullPath);
                    var tmp = Controls.Find(e.FullPath + "-ListView", true);
                    ListView ListViewText = tmp[0] as ListView;
                    int itemIndex = ListViewText.TopItem.Index;
                    int prevIndex = itemIndex;
                    if (IndexKeeper.ContainsKey(e.FullPath))
                    {
                        prevIndex = IndexKeeper[e.FullPath];
                    }

                    if(itemIndex < prevIndex)
                    {

                    }
                    var ItemsCount = ListViewText.Items.Count;
                    if (ItemsCount == 0 || lines.Length < ItemsCount)
                    {
                        ListViewText.Items.Clear();
                        for (var linenum = 0; linenum < lines.Length; linenum++)
                        {
                            ListViewText.Items.Add((linenum + 1).ToString()).SubItems.Add(lines[linenum]);
                        }
                    }
                    else
                    {
                        for (var start = ItemsCount; start < lines.Length; start++)
                        {
                            ListViewText.Items.Add((start + 1).ToString()).SubItems.Add(lines[start]);
                        }
                    }

                    //LogListView.Text = reader.ReadToEnd();
                    FileLengthTB.Text = ListViewText.Items.Count.ToString() + " lines"; //Grabs the Number of lines in a file
                    long FileSizeValue = new FileInfo(e.FullPath).Length; //Create the long for the file size value
                    FileSizeTB.Text = (FileSizeValue / 1024) + " KB"; //Convert File size from bytes to KB

                    if (followTailCheckBox.Checked)
                    {
                        try
                        {
                            ListViewText.Items[ListViewText.Items.Count - 1].EnsureVisible();
                        }
                        catch (ArgumentOutOfRangeException IndErr)
                        {
                            Console.WriteLine(IndErr.Message);
                        }
                    }

                }
                catch (IOException IOex)
                {
                    Console.WriteLine(IOex.Message);
                }
            }
        }
        private ListView GetListViewByTab(TabPage tab)
        {
            ListView ChildListView = Controls.Find(tab.Name + "-ListView", true).FirstOrDefault() as ListView;
            if(ChildListView != null)
            {
                return ChildListView;
            }
            else
                return null;
            
            
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Copy")
            {
                copySelectedItemsToClipboard();
            }

        }

      public void copySelectedItemsToClipboard()
      {
         ListView CurrentListView = GetListViewByTab(TabControlParent.SelectedTab);
         ListView.SelectedListViewItemCollection selectedItems = CurrentListView.SelectedItems;
         StringBuilder sb = new StringBuilder();
         foreach (ListViewItem item in selectedItems)
         {
            ListViewItem l = item as ListViewItem;
            if (l != null)
               foreach (ListViewItem.ListViewSubItem sub in l.SubItems)
                  sb.Append(sub.Text + "\t");
            sb.AppendLine();
         }
         Clipboard.SetDataObject(sb.ToString());
      }

      private void TabControlParent_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                copySelectedItemsToClipboard();
        }

        private void TabControlParent_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                for (int ix = 0; ix < TabControlParent.TabCount; ++ix)
                {
                    if (TabControlParent.GetTabRect(ix).Contains(e.Location))
                    {
                        TabContextMenuStrip.Show(this, e.Location);
                        SelectedTabPage = TabControlParent.TabPages[ix];
                        /*RenameForm rename = new RenameForm();
                        rename.ShowDialog();
                        break;*/
                    }
                }
            }
        }
        private void TabContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "renameTabToolStripMenuItem")
            {
                RenameForm rename = new RenameForm();
                rename.ShowDialog();
                if (rename.DialogResult == DialogResult.OK)
                {
                    //TabPage tab = MainForm.SelectedTabPage;
                    SelectedTabPage.Text = rename.RenameTextBox.Text;
                    SelectedTabPage.Update();
                }
                else
                {
                    rename.Hide();
                }

            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            ListView CurrentListView = GetListViewByTab(TabControlParent.SelectedTab);
            if (SearchBox.Text.Length > 0)
            {
                //ListView CurrentListView = GetListViewByTab(TabControlParent.SelectedTab);
                // Call FindItemWithText with the contents of the textbox.
                ListViewItem foundItem = CurrentListView.FindItemWithText(SearchBox.Text, false, 0, true);
                FindItemWithText(foundItem);
            }
        }

        private void FindItemWithText(ListViewItem foundItem)
        {
            ListView CurrentListView = GetListViewByTab(TabControlParent.SelectedTab);
            if (foundItem != null && SearchBox != null)
            {
                CurrentListView.TopItem = foundItem;
                foundItem.BackColor = Color.SteelBlue;
            }
        }

        private Point DragStartPosition = Point.Empty;

        private void TabControlParent_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DragStartPosition = new Point(e.X, e.Y);
        }


        private void TabControlParent_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Rectangle r = new Rectangle(DragStartPosition, Size.Empty);
            r.Inflate(SystemInformation.DragSize);

            TabPage tp = HoverTab();

            if (tp != null)
            {
                if (!r.Contains(e.X, e.Y))
                    TabControlParent.DoDragDrop(tp, DragDropEffects.All);
            }
            DragStartPosition = Point.Empty;
        }


        private void TabControlParent_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TabPage hover_Tab = HoverTab();
            if (hover_Tab == null)
                e.Effect = DragDropEffects.None;
            else
            {
                if (e.Data.GetDataPresent(typeof(TabPage)))
                {
                    e.Effect = DragDropEffects.Move;
                    TabPage drag_tab = (TabPage)e.Data.GetData(typeof(TabPage));

                    if (hover_Tab == drag_tab) return;

                    Rectangle TabRect = TabControlParent.GetTabRect(TabControlParent.TabPages.IndexOf(hover_Tab));
                    TabRect.Inflate(-3, -3);
                    if (TabRect.Contains(TabControlParent.PointToClient(new Point(e.X, e.Y))))
                    {
                        SwapTabPages(drag_tab, hover_Tab);
                        TabControlParent.SelectedTab = drag_tab;
                    }
                }
            }
        }
        private TabPage HoverTab()
        {
            for (int index = 0; index <= TabControlParent.TabCount - 1; index++)
            {
                if (TabControlParent.GetTabRect(index).Contains(TabControlParent.PointToClient(Cursor.Position)))
                    return TabControlParent.TabPages[index];
            }
            return null;
        }


        private void SwapTabPages(TabPage tp1, TabPage tp2)
        {
            int Index1 = TabControlParent.TabPages.IndexOf(tp1);
            int Index2 = TabControlParent.TabPages.IndexOf(tp2);
            TabControlParent.TabPages[Index1] = tp2;
            TabControlParent.TabPages[Index2] = tp1;
        }



    }
}

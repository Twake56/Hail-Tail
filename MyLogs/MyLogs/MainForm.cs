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
        public Dictionary<String, int> IndexKeeper = new Dictionary<String, int>();


        public MainForm()
        {
            InitializeComponent();
            ImageList iconList = new ImageList();
            Console.WriteLine(Path.GetFullPath(@"../"));
            iconList.Images.Add("doc", Image.FromFile(@"./Assets/Icons/document-icon.png"));
            iconList.Images.Add("folder", Image.FromFile(@"./Assets/Icons/folder-icon.png"));
            iconList.Images.Add("x", Image.FromFile(@"./Assets/Icons/x-icon.png"));
            TabControlParent.ImageList = iconList;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewFile();
        }

        private void OpenNewFile()
        {
            Classes.LogTabPage SelectedTabPage = GetSelectedTabPage();
            if (SelectedTabPage == null || SelectedTabPage.Name == "")
            {
                openFileDialog.InitialDirectory = @"C:\";
            }
            else
            {
                if (Directory.Exists(Path.GetDirectoryName(Path.GetFullPath(SelectedTabPage.Name))))
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(SelectedTabPage.Name));
                }
                else
                {
                    openFileDialog.InitialDirectory = @"C:\";
                }
            }

            //openFileDialog.InitialDirectory = SelectedTabPage.Name;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (TabPage tab in TabControlParent.TabPages)
                {
                    if (openFileDialog.FileName == tab.Name)
                    {
                        TabControlParent.SelectedTab = tab;
                        return;
                    }
                }
                CreateParentTabPageAtIndex(path: openFileDialog.FileName, index: null);
            }
        }

        public void ScrolledToBottom(object sender)
        {
            StartFollowingTail();
        }

        public void ScrolledFromBottom(object sender)
        {
            StopFollowingTail();
        }

        private void CreateParentTabPageAtIndex(string path, int? index, string parentName = null, string text = null)
        {
            try
            {
                int tabPosition = index ?? CountParentTabs() + 1;
                //Creates a new tab for a new log
                Classes.LogTabPage tab = new Classes.LogTabPage() { Text = text ?? Path.GetFileName(path), Name = path, Tag = "File", TailFollowed = true, PositionIndex = tabPosition, ImageIndex = 0 };
                
                tab.SetWatcher(path);
                TabControlParent.TabPages.Add(tab);
                TabControlParent.SelectedTab = tab;
                
                Classes.ListViewNF ListViewText = new Classes.ListViewNF { Parent = tab, Dock = DockStyle.Fill, View = View.Details, };

                ListViewText.Columns.Add("Line", 50, HorizontalAlignment.Left);
                ListViewText.Columns.Add("Text", 1000, HorizontalAlignment.Left);
                ListViewText.FullRowSelect = true;
                ListViewText.ContextMenuStrip = contextMenuStrip1;
                ListViewText.MultiSelect = true;
                ListViewText.Name = "ListViewText";//used to find listview later

                //SelectedTabPage = tab;
                TabViewChange(tab);
                tab.InitialLoad();

                FileLengthTB.Text = ListViewText.Items.Count.ToString() + " lines"; //Grabs the Number of lines in a file
               // long FileSizeValue = new FileInfo(path).Length; //Create the long for the file size value
               // FileSizeTB.Text = (FileSizeValue / 1024) + " KB"; //Convert File size from bytes to KB

                if (!String.IsNullOrWhiteSpace(parentName))
                {
                    TabPage folder = GetFolderByName(parentName);
                    TabControl subTabPageControl = folder.Controls.Find("SubTabControl", true).FirstOrDefault() as TabControl;
                    subTabPageControl.TabPages.Add(tab);
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
            }


        }

        //Copy lines from Logs
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Copy")
            {
                copySelectedItemsToClipboard();
            }

        }

        //Copy lines from Logs
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
                        sb.Append(sub.Text);
                sb.AppendLine();
            }
            Clipboard.SetDataObject(sb.ToString());
        }

        /**********************************
         * Handles the search functions
         * ********************************/
        private void SearchBox_EnterDown(object sender, KeyEventArgs e)
        {
            ListView CurrentListView = GetListViewByTab(TabControlParent.SelectedTab);

            if (e.KeyCode == Keys.Enter)
            {

                if (SearchBox.Text == string.Empty)
                {
                    CurrentListView.BackColor = Color.White;
                    foreach (ListViewItem items in CurrentListView.Items)
                    {
                        items.BackColor = SystemColors.Window;
                        items.ForeColor = SystemColors.WindowText;
                    }
                }

                if (CurrentListView.Items.Count > 0)
                {
                    // Focus the list view
                    //CurrentListView.Focus();
                    // Clear currently selected items
                    CurrentListView.SelectedItems.Clear();
                    int i = 0;
                    ListViewItem found;
                    do
                    {
                        // Recursively find all instances of the given text, starting from zero
                        found = CurrentListView.FindItemWithText(SearchBox.Text.ToString().ToLower(), true, i, true);
                        if (found != null)
                        {
                            // Select found item
                            found.Selected = true;
                            found.BackColor = Color.SteelBlue;
                            // Next search starts from the next element in the list view
                            i = found.Index + 1;
                        }
                        else
                        {
                            // Otherwise, stop
                            i = CurrentListView.Items.Count;
                        }
                    } while (i < CurrentListView.Items.Count);
                }
                // If nothing found, show message
                if (CurrentListView.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Value could not be found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                /*if (SearchBox.Text != string.Empty)
                {
                   if (CurrentListView != null)
                   {
                      foreach (ListViewItem item in CurrentListView.Items)
                      {
                         if (SearchBox.Text.Length > 0)
                         {
                            if (item.ToString().Contains(SearchBox.Text.ToString().ToLower()))
                            {
                               item.BackColor = Color.LightSteelBlue;
                            }
                            else if (item.SubItems[1].ToString().Contains(SearchBox.Text.ToString().ToLower()))
                            {
                               item.BackColor = Color.DodgerBlue;
                            }
                            else
                            {
                               CurrentListView.BackColor = Color.White;
                               item.BackColor = Color.White;
                            }
                         }
                      }
                   }
                }*/
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        /************************
         * Handle Search box Ghost Text
         **************************/
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
                ListView CurrentListView = GetListViewByTab(TabControlParent.SelectedTab);
                CurrentListView.BackColor = Color.White;
                foreach (ListViewItem items in CurrentListView.Items)
                {
                    items.BackColor = SystemColors.Window;
                    items.ForeColor = SystemColors.WindowText;
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentSession(null);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void LoadLastSessionMenuItem_Click(object sender, EventArgs e)
        {
            LoadSession(null);
        }

        private void followTailCheckBox_Click(object sender, EventArgs e)
        {
            if (followTailCheckBox.Checked)
            {
                StartFollowingTail();
            }
            else
            {
                StopFollowingTail();
            }
        }

        private void StartFollowingTail()
        {
            if ((TabControlParent.SelectedTab as Classes.LogTabPage).IsFolder)
            {
                TabControl subTabControl = (TabControlParent.SelectedTab as Classes.LogTabPage).Controls.Find("SubTabControl", true)[0] as TabControl;
                (subTabControl.SelectedTab as Classes.LogTabPage).TailFollowed = true;
                (subTabControl.SelectedTab as Classes.LogTabPage).ScrollToBottom();
            }
            else
            {
                (TabControlParent.SelectedTab as Classes.LogTabPage).TailFollowed = true;
                (TabControlParent.SelectedTab as Classes.LogTabPage).ScrollToBottom();
            }
            followTailCheckBox.Checked = true;
        }

        private void StopFollowingTail()
        {
            if ((TabControlParent.SelectedTab as Classes.LogTabPage).IsFolder)
            {
                TabControl subTabControl = (TabControlParent.SelectedTab as Classes.LogTabPage).Controls.Find("SubTabControl", true)[0] as TabControl;
                (subTabControl.SelectedTab as Classes.LogTabPage).TailFollowed = false;
                (subTabControl.SelectedTab as Classes.LogTabPage).SetLastVisibleItem();
            }
            else
            {
                (TabControlParent.SelectedTab as Classes.LogTabPage).TailFollowed = false;
                (TabControlParent.SelectedTab as Classes.LogTabPage).SetLastVisibleItem();
            }
            followTailCheckBox.Checked = false;
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedTab = (sender as TabControl).SelectedTab as Classes.LogTabPage;
                if (selectedTab.IsFolder && (selectedTab.Controls.Find("SubTabControl", true)[0] as TabControl).TabCount > 0)
                {
                    //SelectedFolder = selectedTab;
                    selectedTab = (selectedTab.Controls.Find("SubTabControl", true)[0] as TabControl).SelectedTab as Classes.LogTabPage;
                }
                SetTitle(selectedTab);
               // SelectedTabPage = selectedTab;
                TabViewChange(selectedTab);
            }
            catch (NullReferenceException err)
            {
                Console.WriteLine(err.Message);
            }
        }

        private void SetTitle(Classes.LogTabPage tab)
        {
            this.Text = "Hail Tail - " + tab.Name;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCurrentSession(null);
            if (TabControlParent.TabPages.Count > 0)
            {
                for (var i = 0; i < TabControlParent.TabPages.Count; i++)
                {
                    Classes.LogTabPage logTabPage = TabControlParent.TabPages[i] as Classes.LogTabPage;
                    if (logTabPage.IsFolder)
                    {
                        TabControl subTabControl = (logTabPage.Controls.Find("SubTabControl", true).FirstOrDefault()) as TabControl;
                        for (var ik = 0; ik < subTabControl.TabPages.Count; ik++)
                        {
                            try
                            {
                                (subTabControl.TabPages[ik] as Classes.LogTabPage).Deconstruct();
                            }
                            catch (NullReferenceException err)
                            {
                                Console.WriteLine(err.Message);
                            }
                        }
                    }
                    try
                    {
                        logTabPage.Deconstruct();
                    }
                    catch (NullReferenceException err)
                    {
                        Console.WriteLine(err.Message);
                    }
                }
                TabControlParent.TabPages.Clear();
            }
            DirectoryInfo tempDir = new DirectoryInfo("./Assets/TempFiles/");
            if (tempDir.GetFiles().Any())
            {
                foreach (FileInfo file in tempDir.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch(IOException err)
                    {
                        Console.WriteLine(err.Message);
                    }
                }
            }
            
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveSessionDialog = new SaveFileDialog();
            saveSessionDialog.Filter = "XML files | *.xml";
            saveSessionDialog.Title = "Save Session File";
            saveSessionDialog.ShowDialog();

            if(saveSessionDialog.FileName != "")
            {
                SaveCurrentSession(saveSessionDialog.FileName);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openSessionDialog = new OpenFileDialog();
            openSessionDialog.Filter = "XML files | *.xml";
            openSessionDialog.Title = "Open Session From File";
            openSessionDialog.ShowDialog();

            if (openSessionDialog.FileName != "")
            {
                LoadSession(openSessionDialog.FileName);
            }
        }

        private void highlightingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool handled = false;
            switch (keyData)
            {
                case Keys.F5:
                    GetSelectedTabPage().RefreshTab();
                    break;
                case Keys.Control | Keys.O:
                    OpenNewFile();
                    break;
                case Keys.Control | Keys.C:
                    copySelectedItemsToClipboard();
                    break;
            }
            return handled;
        }

    }
}

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
    public partial class MainForm
    {
        private Point DragStartPosition = Point.Empty;
        public TabPage SelectedTabPage = new TabPage();
        public TabPage SelectedFolder = new TabPage();


        public TabPage GetFolderByName(string FolderName)
        {

            for (var tabnum = 0; tabnum < TabControlParent.TabPages.Count; tabnum++)
            {
                if (TabControlParent.TabPages[tabnum].Name == FolderName)
                {
                    return TabControlParent.TabPages[tabnum];
                }
            }
            return null;
        }

        private bool CheckForExistingTabName(string tabName)
        {
            for (int x = 0; x < TabControlParent.TabCount; ++x)
            {
                if (TabControlParent.TabPages[x].Name == tabName)
                {
                    return true;
                }
            }
            return false;
        }


        private void createFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NewFolderName = "NewFolder-0";
            while (CheckForExistingTabName(NewFolderName) == true)
            {
                var StringNum = NewFolderName.Split('-')[1];
                int Counter = Convert.ToInt32(StringNum);
                Counter++;
                string NewName = "NewFolder-" + Counter.ToString() + "";
                NewFolderName = NewName;
            }

         /************************
          Attach subtabcontrol to folder tabs
          ************************/
         TabControl subTabControl = new TabControl();
            subTabControl.Name = "SubTabControl";
            subTabControl.Dock = DockStyle.Fill;
            subTabControl.MouseClick += new MouseEventHandler(SubTab_Click);


            TabPage tab = new TabPage() { Text = NewFolderName, Name = NewFolderName, Tag = "Folder" };
            tab.Controls.Add(subTabControl);
            TabControlParent.TabPages.Add(tab);
            TabControlParent.SelectedTab = tab;
        }



        private void SubTab_Click(object source, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //TabControl SubTabControl = GetTabControlByFolder();
                try
                { 
                    TabControl subTabControl = (SelectedFolder.Controls.Find("SubTabControl", true).FirstOrDefault()) as TabControl;
                    for (int ix = 0; ix < subTabControl.TabCount; ++ix)
                    {
                        if (subTabControl.GetTabRect(ix).Contains(e.Location))
                        {
                            TabContextMenuStrip.Show(this, e.Location);
                            SelectedTabPage = subTabControl.TabPages[ix];
                        }
                    }
                }
                catch(NullReferenceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }



        private void renameTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameForm rename = new RenameForm();
            rename.ShowDialog();
            if (rename.DialogResult == DialogResult.OK)
            {
                //TabPage tab = MainForm.SelectedTabPage;
                if(SelectedTabPage.Tag.ToString() == "Folder" && !CheckForExistingTabName(rename.RenameTextBox.Text))
                {
                    SelectedTabPage.Name = rename.RenameTextBox.Text;
                }
                else if(SelectedTabPage.Tag.ToString() == "Folder" && CheckForExistingTabName(rename.RenameTextBox.Text))
                {
                    MessageBox.Show("Folder Name Already Exists.");
                    rename.Hide();
                    return;
                }

                SelectedTabPage.Text = rename.RenameTextBox.Text;
                SelectedTabPage.Update();
            }
            else
            {
                rename.Hide();
            }
        }

        private void TabControlParent_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int ix = 0; ix < TabControlParent.TabCount; ++ix)
                {
                    if (TabControlParent.GetTabRect(ix).Contains(e.Location))
                    {
                        TabContextMenuStrip.Show(this, e.Location);
                        SelectedTabPage = TabControlParent.TabPages[ix];
                        
                    }
                    else if (TabControlParent.TabPages[ix].Tag.ToString() == "Folder")
                    {
                        SelectedFolder = TabControlParent.TabPages[ix];
                        
                    }
                }
            }
            else if(e.Button == MouseButtons.Left)
            {
                for (int ix = 0; ix < TabControlParent.TabCount; ++ix)
                {
                    if (TabControlParent.GetTabRect(ix).Contains(e.Location))
                    {
                        if (TabControlParent.TabPages[ix].Tag.ToString() == "Folder")
                        {
                            SelectedFolder = TabControlParent.TabPages[ix];
                        }
                        
                    }
                }
            }
        }



        private void TabContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;


            if (e.ClickedItem.Name == "TabToolStripMenuItem")
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
            else if (e.ClickedItem.Name == "moveToFolderToolStripMenuItem")
            {
                MoveToFolderForm MoveToForm = new MoveToFolderForm();
                string parentFolderName = SelectedTabPage.Parent.Name;
                for (int ix = 0; ix < TabControlParent.TabCount; ++ix)
                {                    
                    if (TabControlParent.TabPages[ix].Tag.ToString() == "Folder")
                    {
                        MoveToForm.FolderListBox.Items.Add(Text = TabControlParent.TabPages[ix].Text);
                    }

                }
                MoveToForm.ShowDialog();
                if (MoveToForm.DialogResult == DialogResult.OK)
                {
                    try
                    {
                        string selection = MoveToForm.FolderListBox.SelectedItem.ToString();
                        if (selection.ToString() == "No Folder")
                        {
                            TabControlParent.TabPages.Add(SelectedTabPage);
                        }
                        TabPage folder = GetFolderByName(selection);
                        TabControl subTabPageControl = folder.Controls.Find("SubTabControl", true).FirstOrDefault() as TabControl;
                        subTabPageControl.TabPages.Add(SelectedTabPage);
                    }
                    catch (NullReferenceException) { } //No folder selected do nothing
                }
            }
            else if (e.ClickedItem.Name == "deleteToolStripMenuItem")
            {
              
                if (SelectedTabPage.Tag.ToString() == "Folder")
                {
                    
                    TabControl subTabPageControl = SelectedTabPage.Controls.Find("SubTabControl", true).FirstOrDefault() as TabControl;
                    foreach(TabPage tab in subTabPageControl.TabPages)
                    {
                        FileWatchers.Remove(tab.Name);
                    }
                    
                }
                FileWatchers.Remove(SelectedTabPage.Name);
                TabControl tabControl = SelectedTabPage.Parent as TabControl;
                tabControl.TabPages.Remove(SelectedTabPage);
                
            }
        }


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


        private void TabControlParent_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                copySelectedItemsToClipboard();
        }


        private ListView GetListViewByTab(TabPage tab)
        {
            ListView ChildListView = Controls.Find(tab.Name + "-ListView", true).FirstOrDefault() as ListView;
            if (ChildListView != null)
            {
                return ChildListView;
            }
            else
                return null;
        }
    }
}

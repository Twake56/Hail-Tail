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
        private Classes.LogTabPage SelectedTabPage = new Classes.LogTabPage();
        private Classes.LogTabPage SelectedFolder = new Classes.LogTabPage();


        public TabPage GetFolderByName(string FolderName)
        {

            for (var tabnum = 0; tabnum < TabControlParent.TabPages.Count; tabnum++)
            {
                if (TabControlParent.TabPages[tabnum].Name == FolderName)
                {
                    return TabControlParent.TabPages[tabnum] as Classes.LogTabPage;
                }
            }
            return null;
        }

        private void TabViewChange(Classes.LogTabPage logTabPage)
        {
            try
            {
                if (logTabPage.TailFollowed)
                {
                    followTailCheckBox.Checked = true;
                }
                else
                {
                    followTailCheckBox.Checked = false;
                    logTabPage.ScrollToIndex();

                }
            }
            catch(NullReferenceException E)
            {
                Console.WriteLine(E.Message);
            }
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
            CreateFolder(name: NewFolderName, index: null);
        }

        private void CreateFolder(string name, int? index)
        {
            /************************
             Attach subtabcontrol to folder tabs
             ************************/
            TabControl subTabControl = new TabControl();
            subTabControl.Name = "SubTabControl";
            subTabControl.Dock = DockStyle.Fill;
            subTabControl.ShowToolTips = true;
            subTabControl.AllowDrop = true;
            subTabControl.MouseClick += new MouseEventHandler(SubTab_Click);
            subTabControl.SelectedIndexChanged += new EventHandler(TabControl_SelectedIndexChanged);
            subTabControl.DragOver += new DragEventHandler(TabControl_DragOver);
            subTabControl.MouseDown += new MouseEventHandler(TabControl_MouseDown);
            subTabControl.MouseMove += new MouseEventHandler(TabControl_MouseMove);


            int tabPosition = index ?? CountParentTabs() + 1;
            Classes.LogTabPage tab = new Classes.LogTabPage() { Text = name, Name = name, Tag = "Folder", PositionIndex = tabPosition, IsFolder = true };
            tab.Controls.Add(subTabControl);
            TabControlParent.TabPages.Add(tab);
            TabControlParent.SelectedTab = tab;
            tab.ToolTipText = "TabIndex = " + (subTabControl.TabPages.IndexOf(tab).ToString());
            SelectedTabPage = tab;
            TabViewChange(tab);
        }

        private int CountParentTabs ()
        {
            int TabCount = TabControlParent.TabPages.Count;
            return TabCount;
            
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
                            SelectedTabPage = subTabControl.TabPages[ix] as Classes.LogTabPage;
                            //TabViewChange(subTabControl.TabPages[ix] as Classes.LogTabPage);
                        }
                    }
                }
                catch(NullReferenceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }

      /******************************
       * Section that allows for Tab renaming
       ******************************/ 
        private void renameTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameForm rename = new RenameForm();
            rename.ShowDialog();
            if (rename.DialogResult == DialogResult.OK)
            {
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
                        SelectedTabPage = TabControlParent.TabPages[ix] as Classes.LogTabPage;
                   //     TabViewChange(TabControlParent.TabPages[ix] as Classes.LogTabPage);
                    }
                    else if (TabControlParent.TabPages[ix].Tag.ToString() == "Folder")
                    {
                        SelectedFolder = TabControlParent.TabPages[ix] as Classes.LogTabPage;
                        //TabViewChange(TabControlParent.TabPages[ix] as Classes.LogTabPage);
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
                            SelectedFolder = TabControlParent.TabPages[ix] as Classes.LogTabPage;
                           // TabViewChange(TabControlParent.TabPages[ix] as Classes.LogTabPage);
                        }
                        
                    }
                }
            }
        }

      /*************************
       * Handles the Tab menu item selections
       ***************************/ 
        private void TabContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;

            if (e.ClickedItem.Name == "TabToolStripMenuItem")
            {
                RenameForm rename = new RenameForm();
                rename.ShowDialog();

            if (rename.DialogResult == DialogResult.OK)
                {

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
                    
                }
                Console.WriteLine(SelectedTabPage.Name);
                TabControl tabControl = SelectedTabPage.Parent as TabControl;
                tabControl.TabPages.Remove(SelectedTabPage);
                
            }
        }

      /*
       * Tab Drag and order change controls
       */
        private void TabControl_MouseDown(object sender, MouseEventArgs e)
        {
            DragStartPosition = new Point(e.X, e.Y);
        }

        private void TabControl_MouseMove(object sender, MouseEventArgs e)
        {
            TabControl tabControl = (sender as TabControl);
            if (e.Button != MouseButtons.Left) return;

            Rectangle r = new Rectangle(DragStartPosition, Size.Empty);
            r.Inflate(SystemInformation.DragSize);

            TabPage tp = HoverTab(tabControl);

            if (tp != null)
            {
                if (!r.Contains(e.X, e.Y))
                    tabControl.DoDragDrop(tp, DragDropEffects.All);
            }
            DragStartPosition = Point.Empty;
        }

        private void TabControl_DragOver(object sender, DragEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            Classes.LogTabPage hover_Tab = HoverTab(tabControl);
            if (hover_Tab == null)
                e.Effect = DragDropEffects.None;
            else
            {
                if (e.Data.GetDataPresent(typeof(Classes.LogTabPage)))
                {
                    e.Effect = DragDropEffects.Move;
                    Classes.LogTabPage drag_tab = (Classes.LogTabPage)e.Data.GetData(typeof(Classes.LogTabPage));

                    if (hover_Tab == drag_tab) return;
                    //TabControl subTabControl = TabControlParent.SelectedTab.Controls.Find("SubTabControl", true)[0] as TabControl;
                    Rectangle TabRect = tabControl.GetTabRect(tabControl.TabPages.IndexOf(hover_Tab));
                    TabRect.Inflate(-3, -3);
                    if (TabRect.Contains(tabControl.PointToClient(new Point(e.X, e.Y))))
                    {
                        SwapTabPages(tabControl, drag_tab, hover_Tab);
                        tabControl.SelectedTab = drag_tab;
                    }
                }
            }
        }

        private Classes.LogTabPage HoverTab(TabControl tabControl)
        {
            for (int index = 0; index <= tabControl.TabCount - 1; index++)
            {
                if (tabControl.GetTabRect(index).Contains(tabControl.PointToClient(Cursor.Position)))
                    return tabControl.TabPages[index] as Classes.LogTabPage;
            }
            return null;
        }

        private void SwapTabPages(TabControl tabControl, Classes.LogTabPage tp1, Classes.LogTabPage tp2)
        {
            int Index1 = tabControl.TabPages.IndexOf(tp1);
            int Index2 = tabControl.TabPages.IndexOf(tp2);
            tabControl.TabPages[Index1] = tp2;
            tabControl.TabPages[Index2] = tp1;
            int tp1Temp = tp1.PositionIndex;
            tp1.PositionIndex = tp2.PositionIndex;
            tp2.PositionIndex = tp1Temp;
        }

      /**********************************
       * Controls for Copy and Paste
       **********************************/
        private void TabControlParent_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                copySelectedItemsToClipboard();
        }

        /****
         * TODO: NEEDS UPDATING
         *****/
      private ListView GetListViewByTab(TabPage tab)
      {
         if (tab != null)
         {
            ListView ChildListView = Controls.Find(tab.Name + "TabListView", true).FirstOrDefault() as ListView;
            if (ChildListView != null)
            {
               return ChildListView;
            }
            else
               return null;
         }
         else
            return null;
      }
    }
}

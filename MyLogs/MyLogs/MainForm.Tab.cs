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

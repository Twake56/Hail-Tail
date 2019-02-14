using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLogs.Classes
{
    class LogTabPage : TabPage
    {
        public bool TailFollowed { get; set; } = false;
        public bool IsFolder { get; set; } = false;
        public bool IsChild { get; set; } = false;
        public int PositionIndex { get; set; } = 0;
        public string ParentFolderName { get; set; } = null;
        public string FilePath { get; set; } = null;
        public FileSystemWatcher watcher = null;
        public ListViewNF ListView { get; set; } = null;
        public int TopVisibleIndex { get; set; } = 0;

        public void SetWatcher(string path)
        {
            var watch = new FileSystemWatcher();
            watch.Path = Path.GetDirectoryName(path);
            watch.Filter = Path.GetFileName(path);
            watch.Changed += new FileSystemEventHandler(OnChanged);
            watch.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Size;
            watch.EnableRaisingEvents = true;
            this.watcher = watch;
            
        }

        public void TabClicked(object sender, EventArgs e)
        {
 
        }
        public void ScrollToIndex ()
        {
            ListView listView = this.Controls.Find("ListViewText", true)[0] as ListView;
            listView.EnsureVisible(TopVisibleIndex);
        }

        public void SetLastVisibleItem()
        {
            ListView listView = this.Controls.Find("ListViewText", true)[0] as ListView;
            ListViewItem FirstVisible = listView.TopItem;
            this.TopVisibleIndex = FirstVisible.Index;
        }

        private string[] WriteSafeReadAllLines(String path)
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
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            TabControl TabControlParent = this.Parent as TabControl;

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { OnChanged(source, e); });
            }
            else
            {
                try
                {
                    SetLastVisibleItem();
                    string[] lines = WriteSafeReadAllLines(e.FullPath);
                    var ListViewControl = this.Controls.Find("ListViewText", true);
                    ListView ListViewText = ListViewControl[0] as ListView;
                    int itemIndex = ListViewText.TopItem.Index;
                    int prevIndex = itemIndex;
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

                    //Grabs the Number of lines in a file
                    //FileLengthTB.Text = ListViewText.Items.Count.ToString() + " lines";
                    //Create the long for the file size value
                    //long FileSizeValue = new FileInfo(e.FullPath).Length;
                    //Convert File size from bytes to KB
                    //FileSizeTB.Text = (FileSizeValue / 1024) + " KB";

                    if (TailFollowed)
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
    }
}

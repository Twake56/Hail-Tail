﻿using System;
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

        public void SetWatcher(string path)
        {
            Console.WriteLine("setting Watcher");
            var watch = new FileSystemWatcher();
            watch.Path = Path.GetDirectoryName(path);
            watch.Filter = Path.GetFileName(path);
            watch.Changed += new FileSystemEventHandler(OnChanged);
            watch.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Size;
            watch.EnableRaisingEvents = true;
            this.watcher = watch;
            //followTailCheckBox.Checked = true;
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
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //TabPage EventPage = null;
            TabControl TabControlParent = this.Parent as TabControl;
            /*foreach (TabPage tab in TabControlParent.TabPages)
            {
                if (tab.Tag.ToString() == "Folder")
                {
                    TabControl subTabControl = (.Controls.Find("SubTabControl", true).FirstOrDefault()) as TabControl;
                    if (subTabControl != null)
                    {
                        foreach (TabPage subTab in subTabControl.TabPages)
                        {
                            if (e.FullPath == subTab.Name)
                            {
                                EventPage = tab;
                            }
                        }
                    }
                }
                if (e.FullPath == tab.Name)
                {
                    EventPage = tab;
                }
            }*/

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { OnChanged(source, e); });
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
                    /*if (IndexKeeper.ContainsKey(e.FullPath))
                    {
                        prevIndex = IndexKeeper[e.FullPath];
                    }

                    if (itemIndex < prevIndex)
                    {

                    }*/
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

                    /*if (followTailCheckBox.Checked)
                    {
                        try
                        {
                            ListViewText.Items[ListViewText.Items.Count - 1].EnsureVisible();
                        }
                        catch (ArgumentOutOfRangeException IndErr)
                        {
                            Console.WriteLine(IndErr.Message);
                        }
                    }*/
                }
                catch (IOException IOex)
                {
                    Console.WriteLine(IOex.Message);
                }
            }
        }
    }
}

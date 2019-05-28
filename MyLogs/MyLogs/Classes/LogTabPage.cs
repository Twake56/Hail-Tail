using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;

namespace MyLogs.Classes
{
    class LogTabPage : TabPage
    {
        public bool TailFollowed { get; set; } = false;
        public bool IsFolder { get; set; } = false;
        public bool IsChild { get; set; } = false;
        public int PositionIndex { get; set; } = 0;
        public string FilePath { get; set; } = null;
        public FileSystemWatcher watcher = null;
        public ListViewNF ListView { get; set; } = null;
        public int TopVisibleIndex { get; set; } = 0;
        public Thread thread;
        public bool shouldStopThread = false;
        private bool isLoaded { get; set; } = false;
        BackgroundWorker worker = new BackgroundWorker();
        private string tempFileName { get; set; } = null;
        private DateTime fileRefreshedAt { get; set; } = DateTime.Now;

        public LogTabPage()
        {
            this.tempFileName = Path.GetRandomFileName() + ".tmp";
            this.thread = new Thread(new ThreadStart(this.ThreadProc));
            this.thread.IsBackground = true;
            this.thread.Start();
            InitializeWorker();
        }
        public void ThreadProc()
        {
            while (!this.shouldStopThread)
            {
                try
                {
                    Thread.Sleep(200);
                }
                catch (Exception)
                {
                    return;
                }
            }
            this.thread.Abort();
        }

        private void InitializeWorker()
        {
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_Complete);
        }

        public void MouseScroll(object sender, MouseEventArgs e)
        {
            Console.WriteLine(this.VerticalScroll.Value);
        }

        public void SetWatcher(string path)
        {

            if (!File.Exists(path))
            {
                return;
            }
            var watch = new FileSystemWatcher();
            watch.Path = Path.GetDirectoryName(path);
            watch.Filter = Path.GetFileName(path);
            watch.Changed += new FileSystemEventHandler(OnChanged);
            watch.NotifyFilter = NotifyFilters.Size;
            watch.EnableRaisingEvents = true;
            this.watcher = watch;
            Console.WriteLine(this.thread.Name);
        }

        public void ScrollToIndex ()
        {
            try
            {
                ListView listView = this.Controls.Find("ListViewText", true)[0] as ListView;
                listView.EnsureVisible(TopVisibleIndex);
                
            }
            catch(IndexOutOfRangeException)
            {}//Caught folder ignore
        }

        public void ScrollToBottom()
        {
            var ListViewControl = this.Controls.Find("ListViewText", true);
            ListView ListViewText = ListViewControl[0] as ListView;
            try
            {
                ListViewText.Items[ListViewText.Items.Count - 1].EnsureVisible();
            }
            catch (ArgumentOutOfRangeException IndErr)
            {
                Console.WriteLine(IndErr.Message);
            }
        }

        public void SetLastVisibleItem()
        {
            try
            {
                ListView listView = this.Controls.Find("ListViewText", true)[0] as ListView;
                ListViewItem FirstVisible = listView.TopItem;
                this.TopVisibleIndex = FirstVisible.Index;
            }
            catch(Exception err)
            {
                if (err is NullReferenceException || err is IndexOutOfRangeException)
                {
                    Console.WriteLine(err.Message);
                }
                else
                {
                    throw;
                }
            }
        }

        private string[] ReadLines(String path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            if(DateTime.Now < this.fileRefreshedAt.AddSeconds(3))
            {
                Thread.Sleep(3000);
            }

                string tempPath = "./Assets/TempFiles/" + tempFileName;
                File.Copy(path, tempPath, true);
                using (var fs = new FileStream(tempPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                {
                    List<string> file = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        file.Add(sr.ReadLine());
                    }

                    return file.ToArray();
                }
            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            TabControl TabControlParent = this.Parent as TabControl;

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { worker_DoWork(sender, e); });
            }
            else
            {
                try
                {
                    string path = this.Name;
                    string[] lines = ReadLines(path);
                    if (lines == null)
                    {
                        this.ImageIndex= 2;
                        return;  
                    }
                    var ListViewControl = this.Controls.Find("ListViewText", true);
                    ListViewNF ListViewText = ListViewControl[0] as ListViewNF;
                    
                    var ItemsCount = ListViewText.Items.Count;
                    ListViewItem[] itemArray = new ListViewItem[lines.Length];
                    for(var i = 0; i < lines.Length; i++)
                    {
                        ListViewItem item = new ListViewItem((i + 1).ToString());
                        item.SubItems.Add(lines[i]);
                        itemArray[i] = item;
                    }
                    if (ItemsCount == 0 || lines.Length < ItemsCount)
                    {
                        ListViewText.Items.Clear();
                        ListViewText.Items.AddRange(itemArray);
                    }
                    else
                    {
                        ListViewText.Items.AddRange(itemArray);
                    }
                }
                catch (IOException IOex)
                {
                    Console.WriteLine(IOex.Message);
                }
            }
        }

        private void worker_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            this.isLoaded = true;
        }

        public void InitialLoad()
        {
            worker.RunWorkerAsync();
            
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            
            while (!isLoaded) // Wait till worker completes
            {
                Thread.Sleep(200);
            }
            TabControl TabControlParent = this.Parent as TabControl;
           
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate { OnChanged(source, e); });
                }
                catch(Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
            else
            {
               try
                {
                    SetLastVisibleItem();

                    if (!File.Exists(e.FullPath))
                    {
                        return;
                    }
                    string[] lines = ReadLines(e.FullPath);
                    var ListViewControl = this.Controls.Find("ListViewText", true);
                    ListView ListViewText = ListViewControl[0] as ListView;
                    


                    var ItemsCount = ListViewText.Items.Count;

                    if (ItemsCount == 0 || lines.Length < ItemsCount)
                    {
                        // If logs reset or issue occurs, clear listview and reload async
                        ListViewText.Items.Clear();
                        this.isLoaded = false;
                        this.InitialLoad();
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
                        ScrollToBottom();
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

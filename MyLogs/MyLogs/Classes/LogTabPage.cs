using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Timers;

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
        BackgroundWorker initialWorker = new BackgroundWorker();
        BackgroundWorker upkeepWorker = new BackgroundWorker();
        private string tempFileName { get; set; } = null;
        private DateTime fileRefreshedAt { get; set; } = DateTime.Now;
        System.Timers.Timer timer = new System.Timers.Timer();


        public LogTabPage()
        {
            this.tempFileName = Path.GetRandomFileName() + ".tmp";
            this.thread = new Thread(new ThreadStart(this.ThreadProc));
            this.thread.IsBackground = true;
            this.thread.Start();
            InitializeWorkers();
            timer.Interval = 1500;
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
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

        public void Deconstruct()
        {
            if(upkeepWorker.IsBusy)
            {
                upkeepWorker.CancelAsync();
            }
            this.watcher.EnableRaisingEvents = false;
            ListView listView = this.Controls.Find("ListViewText", true)[0] as ListView;
            listView.Clear();
            this.thread.Abort();
        }
        private void InitializeWorkers()
        {
            initialWorker.DoWork += new DoWorkEventHandler(initialWorker_DoWork);
            initialWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(initialWorker_Complete);
            upkeepWorker.DoWork += new DoWorkEventHandler(upkeepWorker_DoWork);
            upkeepWorker.WorkerSupportsCancellation = true;
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
            watch.Error += new ErrorEventHandler(WatcherError);
            this.watcher = watch;
            Console.WriteLine(this.thread.Name);
        }

        public void ScrollToIndex()
        {
            try
            {
                ListView listView = this.Controls.Find("ListViewText", true)[0] as ListView;
                listView.EnsureVisible(TopVisibleIndex);

            }
            catch (Exception)
            { }//Caught folder ignore
        }

        private void WatcherError(object sender, ErrorEventArgs e)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.ImageIndex = 2;
                    var path = this.Name;
                    while (!File.Exists(path))
                    {
                        Thread.Sleep(4000);
                    }
                    this.SetWatcher(this.Name);
                    this.ImageIndex = 0;
                }));
            }
            else
            {
                this.ImageIndex = 2;
                var path = this.Name;
                while (!File.Exists(path))
                {
                    Thread.Sleep(4000);
                }
                this.SetWatcher(this.Name);
                this.ImageIndex = 0;
            }
            
        }

        public void ScrollToBottom()
        {
            var ListViewControl = this.Controls.Find("ListViewText", true);
            ListView ListViewText = ListViewControl[0] as ListView;

            try
            {
                if (ListViewText.InvokeRequired)
                {
                    ListViewText.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            ListViewText.Items[ListViewText.Items.Count - 1].EnsureVisible();
                        }
                        catch (ArgumentOutOfRangeException IndErr)
                        {
                            Console.WriteLine(IndErr.Message);
                        }
                    }));
                }
                else
                {
                    ListViewText.Items[ListViewText.Items.Count - 1].EnsureVisible();
                }
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
                if (listView.InvokeRequired)
                {
                    listView.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            ListViewItem FirstVisible = listView.TopItem;
                            this.TopVisibleIndex = FirstVisible.Index;
                        }
                        catch (Exception err)
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
                    }));
                }

                else
                {
                    ListViewItem FirstVisible = listView.TopItem;
                    this.TopVisibleIndex = FirstVisible.Index;
                }
            }
            catch (Exception err)
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

        private void initialWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TabControl TabControlParent = this.Parent as TabControl;
            try
            {
                string path = this.Name;
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

                    string[] lines = file.ToArray();
                    if (lines.Length < 1 || lines == null)
                    {
                        this.ImageIndex = 2;
                        return;
                    }
                    var ListViewControl = this.Controls.Find("ListViewText", true);
                    ListViewNF ListViewText = ListViewControl[0] as ListViewNF;

                    var ItemsCount = ListViewText.Items.Count;
                    ListViewItem[] itemArray = new ListViewItem[lines.Length];
                    for (var i = 0; i < lines.Length; i++)
                    {
                        ListViewItem item = new ListViewItem((i + 1).ToString());
                        item.SubItems.Add(lines[i]);
                        itemArray[i] = item;
                    }
                    if (ListViewText.InvokeRequired)
                    {
                        ListViewText.Invoke(new MethodInvoker(delegate
                        {
                            if (ItemsCount == 0 || lines.Length < ItemsCount)
                            {
                                ListViewText.Items.Clear();
                                ListViewText.Items.AddRange(itemArray);
                            }
                            else
                            {
                                ListViewText.Items.AddRange(itemArray);
                            }
                        }));
                    }
                    else
                    {
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
                }
            }
            catch (IOException IOex)
            {
                Console.WriteLine(IOex.Message);
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        this.ImageIndex = 2;
                    }));
                }
                else
                {
                    this.ImageIndex = 2;
                }

            }


        }

        private void upkeepWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!isLoaded || timer.Enabled) // Wait till initialWorker completes
            {
                return;
            }

            if (DateTime.Now < this.fileRefreshedAt.AddSeconds(2))
            {
                timer.Start();
            }

                TabControl TabControlParent = this.Parent as TabControl;
                try
                {
                    SetLastVisibleItem();
                    string path = this.Name;
                    if (!File.Exists(path))
                    {
                        return;
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
                        string[] lines = file.ToArray();
                        var ListViewControl = this.Controls.Find("ListViewText", true);
                        ListView ListViewText = ListViewControl[0] as ListView;



                        var ItemsCount = ListViewText.Items.Count;
                    if (ListViewText.InvokeRequired)
                    {
                        ListViewText.Invoke(new MethodInvoker(delegate
                        {
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
                        }));
                    }
                    else
                    {
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
                    }
                        this.fileRefreshedAt = DateTime.Now;

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
                }

                catch (IOException IOex)
                {
                    Console.WriteLine(IOex.Message);
                }

            //}
            
        }


        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!upkeepWorker.IsBusy)
                upkeepWorker.RunWorkerAsync();
        }

        /*private void upkeepWorker_Complete(object sender, RunWorkerCompletedEventArgs e)
        {

        }*/

        private void initialWorker_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            this.isLoaded = true;
        }

        public void InitialLoad()
        {
            initialWorker.RunWorkerAsync();

        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if(!upkeepWorker.IsBusy)
                upkeepWorker.RunWorkerAsync();
        }
    }
}

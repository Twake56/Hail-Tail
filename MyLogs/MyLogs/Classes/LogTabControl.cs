using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MyLogs.Classes
{
    class LogTabControl : TabControl
    {
        private bool shouldStopThread = false;
        private Thread thread;

        public LogTabControl()
        {
            this.thread = new Thread(new ThreadStart(this.ThreadProc));
            this.thread.IsBackground = true;
            this.thread.Start();
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
    }
}

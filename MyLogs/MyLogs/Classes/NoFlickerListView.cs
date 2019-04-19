using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MyLogs.Classes
{
    class ListViewNF : System.Windows.Forms.ListView
    {
        private int prevPostion { get; set; } = -1;

        public ListViewNF()
        {
            //Activate double bufferin
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            this.Scroll += new ScrollEventHandler(Scrolled);
        }


        public event ScrollEventHandler Scroll;
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler = this.Scroll;
            if (handler != null) handler(this, e);
        }

        public void Scrolled(object sender, ScrollEventArgs e)
        {

            ListViewItem lastVisible = this.TopItem;
            var newBounds = this.Items[this.Items.Count - 1].Bounds; // Dont include length of item
            newBounds.Width = 0;
            if ((this.Parent as LogTabPage).ClientRectangle.Contains(newBounds))
            {
                MainForm main = null;
                if (this.Parent.Parent.Parent.Parent is MainForm)
                {
                    main = this.Parent.Parent.Parent.Parent as MainForm;
                }
                else
                {
                    main = this.Parent.Parent.Parent.Parent.Parent.Parent as MainForm;
                }

                main.ScrolledToBottom(this);
            }
            else
            {
                MainForm main = null;
                if (this.Parent.Parent.Parent.Parent is MainForm)
                {
                    main = this.Parent.Parent.Parent.Parent as MainForm;
                }
                else
                {
                    main = this.Parent.Parent.Parent.Parent.Parent.Parent as MainForm;
                }

                main.ScrolledFromBottom(this);
            }
        
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetScrollPos(IntPtr hWnd, int nBar);
        protected override void WndProc(ref Message m)

        {
            base.WndProc(ref m);
            if (m.Msg == 0x115)//WM_VSCROLL
            {
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), GetScrollPos(this.Handle, 1)));
            }
            else if(m.Msg == 0x020A)//WM_MOUSEWHEEL
            {
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), GetScrollPos(this.Handle, 1)));
            }
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);

            }
        }

    }
}

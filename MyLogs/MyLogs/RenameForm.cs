﻿using System;
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
    public partial class RenameForm : Form
    {
        public RenameForm()
        {
            InitializeComponent();
        }

        private void Cancel_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        
    }
}
namespace MyLogs
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
         this.FollowTailCheckBox = new System.Windows.Forms.CheckBox();
         this.OpenFileBtn = new System.Windows.Forms.Button();
         this.logRichTextBox1 = new System.Windows.Forms.RichTextBox();
         this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
         this.tableLayoutPanel3.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel3
         // 
         this.tableLayoutPanel3.ColumnCount = 2;
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.30769F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.69231F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel3.Controls.Add(this.logRichTextBox1, 1, 2);
         this.tableLayoutPanel3.Controls.Add(this.FollowTailCheckBox, 1, 1);
         this.tableLayoutPanel3.Controls.Add(this.OpenFileBtn, 1, 0);
         this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.tableLayoutPanel3.Location = new System.Drawing.Point(0, -1);
         this.tableLayoutPanel3.Name = "tableLayoutPanel3";
         this.tableLayoutPanel3.RowCount = 4;
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
         this.tableLayoutPanel3.Size = new System.Drawing.Size(799, 363);
         this.tableLayoutPanel3.TabIndex = 0;
         this.tableLayoutPanel3.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel3_Paint);
         // 
         // FollowTailCheckBox
         // 
         this.FollowTailCheckBox.AutoSize = true;
         this.FollowTailCheckBox.Location = new System.Drawing.Point(101, 33);
         this.FollowTailCheckBox.Name = "FollowTailCheckBox";
         this.FollowTailCheckBox.Size = new System.Drawing.Size(76, 17);
         this.FollowTailCheckBox.TabIndex = 2;
         this.FollowTailCheckBox.Text = "Follow Tail";
         this.FollowTailCheckBox.UseVisualStyleBackColor = true;
         // 
         // OpenFileBtn
         // 
         this.OpenFileBtn.Location = new System.Drawing.Point(101, 3);
         this.OpenFileBtn.Name = "OpenFileBtn";
         this.OpenFileBtn.Size = new System.Drawing.Size(75, 23);
         this.OpenFileBtn.TabIndex = 0;
         this.OpenFileBtn.Text = "Open File";
         this.OpenFileBtn.UseVisualStyleBackColor = true;
         this.OpenFileBtn.Click += new System.EventHandler(this.OpenFileBtn_Click);
         // 
         // logRichTextBox1
         // 
         this.logRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.logRichTextBox1.Location = new System.Drawing.Point(101, 58);
         this.logRichTextBox1.Name = "logRichTextBox1";
         this.logRichTextBox1.ReadOnly = true;
         this.logRichTextBox1.Size = new System.Drawing.Size(695, 271);
         this.logRichTextBox1.TabIndex = 4;
         this.logRichTextBox1.Text = "";
         // 
         // openFileDialog3
         // 
         this.openFileDialog3.FileName = "openFileDialog3";
         // 
         // MainForm
         // 
         this.ClientSize = new System.Drawing.Size(799, 362);
         this.Controls.Add(this.tableLayoutPanel3);
         this.Name = "MainForm";
         this.Text = "Timber Tail";
         this.tableLayoutPanel3.ResumeLayout(false);
         this.tableLayoutPanel3.PerformLayout();
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button OpenFileBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.CheckBox FollowTailCheckBox;
        private System.Windows.Forms.RichTextBox logRichTextBox1;
    }
}


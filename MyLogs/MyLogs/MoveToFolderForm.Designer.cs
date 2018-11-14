namespace MyLogs
{
    partial class MoveToFolderForm
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
            this.fileSelectTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FolderListBox = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.fileSelectTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileSelectTableLayoutPanel
            // 
            this.fileSelectTableLayoutPanel.ColumnCount = 1;
            this.fileSelectTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fileSelectTableLayoutPanel.Controls.Add(this.FolderListBox, 0, 0);
            this.fileSelectTableLayoutPanel.Controls.Add(this.button1, 0, 1);
            this.fileSelectTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileSelectTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.fileSelectTableLayoutPanel.Name = "fileSelectTableLayoutPanel";
            this.fileSelectTableLayoutPanel.RowCount = 2;
            this.fileSelectTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fileSelectTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.fileSelectTableLayoutPanel.Size = new System.Drawing.Size(272, 371);
            this.fileSelectTableLayoutPanel.TabIndex = 0;
            // 
            // FolderListBox
            // 
            this.FolderListBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FolderListBox.FormattingEnabled = true;
            this.FolderListBox.Items.AddRange(new object[] {
            "No Folder"});
            this.FolderListBox.Location = new System.Drawing.Point(3, 17);
            this.FolderListBox.Name = "FolderListBox";
            this.FolderListBox.Size = new System.Drawing.Size(266, 316);
            this.FolderListBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(3, 339);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Move To Folder";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MoveToFolderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 371);
            this.Controls.Add(this.fileSelectTableLayoutPanel);
            this.Name = "MoveToFolderForm";
            this.Text = "Folder Selection";
            this.fileSelectTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel fileSelectTableLayoutPanel;
        public System.Windows.Forms.ListBox FolderListBox;
        private System.Windows.Forms.Button button1;
    }
}
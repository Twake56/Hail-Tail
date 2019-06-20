namespace MyLogs
{
    partial class RenameForm
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
            this.RenameMainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RenameDoneButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.RenameTextBox = new System.Windows.Forms.TextBox();
            this.RenameMainLayoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RenameMainLayoutPanel
            // 
            this.RenameMainLayoutPanel.ColumnCount = 1;
            this.RenameMainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RenameMainLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.RenameMainLayoutPanel.Controls.Add(this.RenameTextBox, 0, 1);
            this.RenameMainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameMainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.RenameMainLayoutPanel.Name = "RenameMainLayoutPanel";
            this.RenameMainLayoutPanel.RowCount = 3;
            this.RenameMainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.RenameMainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RenameMainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RenameMainLayoutPanel.Size = new System.Drawing.Size(321, 134);
            this.RenameMainLayoutPanel.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.RenameDoneButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 80);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(315, 30);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // RenameDoneButton
            // 
            this.RenameDoneButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.RenameDoneButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.RenameDoneButton.Location = new System.Drawing.Point(69, 3);
            this.RenameDoneButton.Name = "RenameDoneButton";
            this.RenameDoneButton.Size = new System.Drawing.Size(75, 24);
            this.RenameDoneButton.TabIndex = 0;
            this.RenameDoneButton.Text = "Done";
            this.RenameDoneButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Left;
            this.button1.Location = new System.Drawing.Point(170, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Cancel_MouseClick);
            // 
            // RenameTextBox
            // 
            this.RenameTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.RenameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.RenameTextBox.Location = new System.Drawing.Point(3, 23);
            this.RenameTextBox.Name = "RenameTextBox";
            this.RenameTextBox.Size = new System.Drawing.Size(315, 20);
            this.RenameTextBox.TabIndex = 1;
            // 
            // RenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 134);
            this.Controls.Add(this.RenameMainLayoutPanel);
            this.Name = "RenameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename Tab";
            this.RenameMainLayoutPanel.ResumeLayout(false);
            this.RenameMainLayoutPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RenameMainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button RenameDoneButton;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox RenameTextBox;
    }
}
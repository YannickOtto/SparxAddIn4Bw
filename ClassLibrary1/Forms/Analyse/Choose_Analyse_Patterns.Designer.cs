namespace Forms
{
    partial class Choose_Analyse_Patterns
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
            this.tree_Pattern = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Check = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox_SysArch = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree_Pattern
            // 
            this.tree_Pattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree_Pattern.Location = new System.Drawing.Point(3, 3);
            this.tree_Pattern.Name = "tree_Pattern";
            this.tableLayoutPanel1.SetRowSpan(this.tree_Pattern, 2);
            this.tree_Pattern.Size = new System.Drawing.Size(506, 376);
            this.tree_Pattern.TabIndex = 0;
            this.tree_Pattern.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_Pattern_BeforeCheck);
            this.tree_Pattern.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_Pattern_AfterSelect);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(3, 430);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(506, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "© Yannick Otto";
            // 
            // button_Check
            // 
            this.button_Check.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Check.Location = new System.Drawing.Point(515, 385);
            this.button_Check.Name = "button_Check";
            this.button_Check.Size = new System.Drawing.Size(90, 62);
            this.button_Check.TabIndex = 0;
            this.button_Check.Text = "Check All";
            this.button_Check.UseVisualStyleBackColor = true;
            this.button_Check.Click += new System.EventHandler(this.button_Check_Click);
            // 
            // textBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 3);
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.textBox1.Location = new System.Drawing.Point(515, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(282, 152);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Bitte wählen Sie die Pattern aus, \r\nwelche analysiert werden sollen.";
            // 
            // button_OK
            // 
            this.button_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_OK.Location = new System.Drawing.Point(611, 385);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(90, 62);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Cancel.Location = new System.Drawing.Point(707, 385);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(90, 62);
            this.button_Cancel.TabIndex = 0;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel1.Controls.Add(this.button_Cancel, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_OK, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_Check, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tree_Pattern, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_SysArch, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.77778F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // checkBox_SysArch
            // 
            this.checkBox_SysArch.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBox_SysArch, 3);
            this.checkBox_SysArch.Location = new System.Drawing.Point(515, 161);
            this.checkBox_SysArch.Name = "checkBox_SysArch";
            this.checkBox_SysArch.Size = new System.Drawing.Size(174, 24);
            this.checkBox_SysArch.TabIndex = 3;
            this.checkBox_SysArch.Text = "Systemaufbruch P2";
            this.checkBox_SysArch.UseVisualStyleBackColor = true;
           // 
            // Choose_Analyse_Patterns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Choose_Analyse_Patterns";
            this.Text = "Auswahl Patterns";
            this.Load += new System.EventHandler(this.Choose_Analyse_Patterns_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public void Set_Text_TextBox_1(string text)
        {
            this.textBox1.Text = text;
            this.textBox1.Update();
        }

        public void Set_Uberschrift(string text)
        {
            this.Text = text;
            this.Update();
        }

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView tree_Pattern;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Check;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox_SysArch;
    }
}
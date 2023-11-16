
namespace Requirement_Plugin.Forms
{
    partial class Choose_Import_Elements_To_Delete
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
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Konnektoren");
            this.Main_Groupbox = new System.Windows.Forms.GroupBox();
            this.Main_Panel = new System.Windows.Forms.Panel();
            this.tableLayout_Main = new System.Windows.Forms.TableLayoutPanel();
            this.treeView_Elements = new System.Windows.Forms.TreeView();
            this.label_Text_Abfrage = new System.Windows.Forms.Label();
            this.treeView_Connectoren = new System.Windows.Forms.TreeView();
            this.button_OK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.richTextBox_Search = new System.Windows.Forms.RichTextBox();
            this.label_Search = new System.Windows.Forms.Label();
            this.Main_Groupbox.SuspendLayout();
            this.Main_Panel.SuspendLayout();
            this.tableLayout_Main.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Main_Groupbox
            // 
            this.Main_Groupbox.Controls.Add(this.Main_Panel);
            this.Main_Groupbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_Groupbox.Location = new System.Drawing.Point(0, 0);
            this.Main_Groupbox.Name = "Main_Groupbox";
            this.Main_Groupbox.Size = new System.Drawing.Size(1051, 622);
            this.Main_Groupbox.TabIndex = 0;
            this.Main_Groupbox.TabStop = false;
            this.Main_Groupbox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // Main_Panel
            // 
            this.Main_Panel.Controls.Add(this.tableLayout_Main);
            this.Main_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_Panel.Location = new System.Drawing.Point(3, 22);
            this.Main_Panel.Name = "Main_Panel";
            this.Main_Panel.Size = new System.Drawing.Size(1045, 597);
            this.Main_Panel.TabIndex = 0;
            // 
            // tableLayout_Main
            // 
            this.tableLayout_Main.ColumnCount = 3;
            this.tableLayout_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.8714F));
            this.tableLayout_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.1286F));
            this.tableLayout_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayout_Main.Controls.Add(this.treeView_Elements, 0, 1);
            this.tableLayout_Main.Controls.Add(this.label_Text_Abfrage, 0, 0);
            this.tableLayout_Main.Controls.Add(this.treeView_Connectoren, 0, 2);
            this.tableLayout_Main.Controls.Add(this.button_OK, 2, 3);
            this.tableLayout_Main.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.tableLayout_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_Main.Location = new System.Drawing.Point(0, 0);
            this.tableLayout_Main.Name = "tableLayout_Main";
            this.tableLayout_Main.RowCount = 4;
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.91489F));
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.08511F));
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 201F));
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayout_Main.Size = new System.Drawing.Size(1045, 597);
            this.tableLayout_Main.TabIndex = 0;
            // 
            // treeView_Elements
            // 
            this.treeView_Elements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Elements.Location = new System.Drawing.Point(3, 102);
            this.treeView_Elements.Name = "treeView_Elements";
            this.treeView_Elements.Size = new System.Drawing.Size(513, 207);
            this.treeView_Elements.TabIndex = 1;
            // 
            // label_Text_Abfrage
            // 
            this.label_Text_Abfrage.AutoSize = true;
            this.label_Text_Abfrage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Text_Abfrage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Text_Abfrage.Location = new System.Drawing.Point(3, 0);
            this.label_Text_Abfrage.Name = "label_Text_Abfrage";
            this.label_Text_Abfrage.Size = new System.Drawing.Size(513, 99);
            this.label_Text_Abfrage.TabIndex = 0;
            this.label_Text_Abfrage.Text = "Es wurden Elemente bzw. Konnektoren identifiziert, welche nicht importiert wurden" +
    ". Wählen Sie die zu löschenden Elemente bzw. Konnektoren aus.";
            // 
            // treeView_Connectoren
            // 
            this.treeView_Connectoren.CheckBoxes = true;
            this.treeView_Connectoren.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Connectoren.Location = new System.Drawing.Point(3, 315);
            this.treeView_Connectoren.Name = "treeView_Connectoren";
            treeNode3.Name = "Konnektoren";
            treeNode3.Text = "Konnektoren";
            this.treeView_Connectoren.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.treeView_Connectoren.Size = new System.Drawing.Size(513, 195);
            this.treeView_Connectoren.TabIndex = 2;
            this.treeView_Connectoren.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_Connectoren_BeforeCheck);
            this.treeView_Connectoren.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Connectoren_AfterCheck);
            this.treeView_Connectoren.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Connectoren_AfterSelect);
            // 
            // button_OK
            // 
            this.button_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_OK.Location = new System.Drawing.Point(899, 516);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(143, 78);
            this.button_OK.TabIndex = 3;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.richTextBox_Search, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_Search, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(522, 102);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.3299F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.6701F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(371, 207);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // richTextBox_Search
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBox_Search, 2);
            this.richTextBox_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Search.Location = new System.Drawing.Point(3, 35);
            this.richTextBox_Search.Multiline = false;
            this.richTextBox_Search.Name = "richTextBox_Search";
            this.richTextBox_Search.Size = new System.Drawing.Size(365, 34);
            this.richTextBox_Search.TabIndex = 0;
            this.richTextBox_Search.Text = "";
            this.richTextBox_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyUp);
            // 
            // label_Search
            // 
            this.label_Search.AutoSize = true;
            this.label_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Search.Location = new System.Drawing.Point(3, 0);
            this.label_Search.Name = "label_Search";
            this.label_Search.Size = new System.Drawing.Size(74, 25);
            this.label_Search.TabIndex = 1;
            this.label_Search.Text = "Suche";
            // 
            // Choose_Import_Elements_To_Delete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1051, 622);
            this.Controls.Add(this.Main_Groupbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Choose_Import_Elements_To_Delete";
            this.Text = "Auswahl zu löschende Elemente";
            this.Load += new System.EventHandler(this.Choose_Import_Elements_To_Delete_Load);
            this.Main_Groupbox.ResumeLayout(false);
            this.Main_Panel.ResumeLayout(false);
            this.tableLayout_Main.ResumeLayout(false);
            this.tableLayout_Main.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Main_Groupbox;
        private System.Windows.Forms.Panel Main_Panel;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Main;
        private System.Windows.Forms.Label label_Text_Abfrage;
        private System.Windows.Forms.TreeView treeView_Elements;
        private System.Windows.Forms.TreeView treeView_Connectoren;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox richTextBox_Search;
        private System.Windows.Forms.Label label_Search;
    }
}
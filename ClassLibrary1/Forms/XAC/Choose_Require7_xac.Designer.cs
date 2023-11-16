namespace Forms
{
    public partial class Choose_Require7_xac
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Technisches System");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Szenarbaum");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Funktionsbaum");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Stakeholder");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Systemelemente", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Schnittstellen");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Funktional");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Nutzer");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Design");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Process");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Umwelt");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Typvertreter");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Anforderungen", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Systemelemente --> Systemelemente");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Anforderungen -- > Anforderungen");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Systemelemente --> Anforderungen");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Funktionsbaum --> Anforderungen");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Szenarbaum --> Anforderungen");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Stakeholder --> Anforderungen");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Konnektoren", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19});
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Nachweisart");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Glossar");
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.OK_EXPORT = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tree_Choose_Export = new System.Windows.Forms.TreeView();
            this.checkBox_Ueberpreufung = new System.Windows.Forms.CheckBox();
            this.checkBox_Szenar_Aufloesung = new System.Windows.Forms.CheckBox();
            this.checkBox_Sysele = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.875F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tree_Choose_Export, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_Ueberpreufung, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_Szenar_Aufloesung, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_Sysele, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.67037F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.23872F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 562);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 498);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "© Yannick Otto";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(679, 502);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.OK_EXPORT);
            this.splitContainer1.Size = new System.Drawing.Size(217, 55);
            this.splitContainer1.SplitterDistance = 97;
            this.splitContainer1.TabIndex = 3;
            // 
            // OK_EXPORT
            // 
            this.OK_EXPORT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OK_EXPORT.Location = new System.Drawing.Point(0, 0);
            this.OK_EXPORT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OK_EXPORT.Name = "OK_EXPORT";
            this.OK_EXPORT.Size = new System.Drawing.Size(116, 55);
            this.OK_EXPORT.TabIndex = 2;
            this.OK_EXPORT.Text = "OK";
            this.OK_EXPORT.UseVisualStyleBackColor = true;
            this.OK_EXPORT.Click += new System.EventHandler(this.OK_EXPORT_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(645, 29);
            this.label1.TabIndex = 4;
            this.label1.Text = "Wählen Sie die Elemente, welche exportiert werden sollen.";
            // 
            // tree_Choose_Export
            // 
            this.tree_Choose_Export.CheckBoxes = true;
            this.tree_Choose_Export.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree_Choose_Export.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tree_Choose_Export.Location = new System.Drawing.Point(3, 119);
            this.tree_Choose_Export.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tree_Choose_Export.Name = "tree_Choose_Export";
            treeNode1.Name = "Technisches System";
            treeNode1.Text = "Technisches System";
            treeNode2.Name = "Szenarbaum";
            treeNode2.Text = "Szenarbaum";
            treeNode3.Name = "Funktionsbaum";
            treeNode3.Text = "Funktionsbaum";
            treeNode4.Name = "Stakeholder";
            treeNode4.Tag = "";
            treeNode4.Text = "Stakeholder";
            treeNode5.Name = "Systemelemente";
            treeNode5.Tag = "this.Data.sys_Export";
            treeNode5.Text = "Systemelemente";
            treeNode6.Name = "Schnittstellen";
            treeNode6.Tag = "this.Data.afo_interface_Export";
            treeNode6.Text = "Schnittstellen";
            treeNode7.Name = "Funktional";
            treeNode7.Tag = "this.Data.afo_funktional_Export";
            treeNode7.Text = "Funktional";
            treeNode8.Name = "Nutzer";
            treeNode8.Tag = "this.Data.afo_user_Export";
            treeNode8.Text = "Nutzer";
            treeNode9.Name = "Design";
            treeNode9.Tag = "this.Data.afo_design_Export";
            treeNode9.Text = "Design";
            treeNode10.Name = "Process";
            treeNode10.Text = "Process";
            treeNode11.Name = "Umwelt";
            treeNode11.Text = "Umwelt";
            treeNode12.Name = "Typvertreter";
            treeNode12.Text = "Typvertreter";
            treeNode13.Name = "Anforderungen";
            treeNode13.Text = "Anforderungen";
            treeNode14.Name = "SystemelementeSystemelemente";
            treeNode14.Tag = "this.Data.link_decomposition";
            treeNode14.Text = "Systemelemente --> Systemelemente";
            treeNode15.Name = "AnforderungenAnforderungen";
            treeNode15.Tag = "this.Data.link_afo_afo";
            treeNode15.Text = "Anforderungen -- > Anforderungen";
            treeNode16.Name = "SystemelementeAnforderungen";
            treeNode16.Tag = "this.Data.link_afo_sys";
            treeNode16.Text = "Systemelemente --> Anforderungen";
            treeNode17.Name = "FunktionsbaumAnforderungen";
            treeNode17.Text = "Funktionsbaum --> Anforderungen";
            treeNode18.Name = "SzenarbaumAnforderungen";
            treeNode18.Text = "Szenarbaum --> Anforderungen";
            treeNode19.Name = "StakeholderAnforderungen";
            treeNode19.Text = "Stakeholder --> Anforderungen";
            treeNode20.Name = "Konnektoren";
            treeNode20.Text = "Konnektoren";
            treeNode21.Name = "Nachweisart";
            treeNode21.Text = "Nachweisart";
            treeNode22.Name = "Glossar";
            treeNode22.Text = "Glossar";
            this.tree_Choose_Export.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode13,
            treeNode20,
            treeNode21,
            treeNode22});
            this.tableLayoutPanel1.SetRowSpan(this.tree_Choose_Export, 2);
            this.tree_Choose_Export.Size = new System.Drawing.Size(670, 375);
            this.tree_Choose_Export.TabIndex = 0;
            this.tree_Choose_Export.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_Choose_Export_BeforeCheck);
            this.tree_Choose_Export.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tree_Choose_Export_AfterCheck);
            // 
            // checkBox_Ueberpreufung
            // 
            this.checkBox_Ueberpreufung.AutoSize = true;
            this.checkBox_Ueberpreufung.Checked = true;
            this.checkBox_Ueberpreufung.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Ueberpreufung.Location = new System.Drawing.Point(679, 159);
            this.checkBox_Ueberpreufung.Name = "checkBox_Ueberpreufung";
            this.checkBox_Ueberpreufung.Size = new System.Drawing.Size(175, 24);
            this.checkBox_Ueberpreufung.TabIndex = 6;
            this.checkBox_Ueberpreufung.Text = "Überprüfung Import";
            this.checkBox_Ueberpreufung.UseVisualStyleBackColor = true;
            this.checkBox_Ueberpreufung.Visible = false;
            // 
            // checkBox_Szenar_Aufloesung
            // 
            this.checkBox_Szenar_Aufloesung.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_Szenar_Aufloesung.AutoSize = true;
            this.checkBox_Szenar_Aufloesung.Location = new System.Drawing.Point(679, 123);
            this.checkBox_Szenar_Aufloesung.Name = "checkBox_Szenar_Aufloesung";
            this.checkBox_Szenar_Aufloesung.Size = new System.Drawing.Size(161, 24);
            this.checkBox_Szenar_Aufloesung.TabIndex = 5;
            this.checkBox_Szenar_Aufloesung.Text = "Auflösung Logical";
            this.checkBox_Szenar_Aufloesung.UseVisualStyleBackColor = true;
            this.checkBox_Szenar_Aufloesung.CheckedChanged += new System.EventHandler(this.checkBox_Szenar_Aufloesung_CheckedChanged);
            // 
            // checkBox_Sysele
            // 
            this.checkBox_Sysele.AutoSize = true;
            this.checkBox_Sysele.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_Sysele.Location = new System.Drawing.Point(679, 79);
            this.checkBox_Sysele.Name = "checkBox_Sysele";
            this.checkBox_Sysele.Size = new System.Drawing.Size(218, 33);
            this.checkBox_Sysele.TabIndex = 7;
            this.checkBox_Sysele.Text = "Systemaufbruch P2";
            this.checkBox_Sysele.UseVisualStyleBackColor = true;
            // 
            // Choose_Require7_xac
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(900, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Choose_Require7_xac";
            this.Text = "Auswahl Require7";
            this.Load += new System.EventHandler(this.Choose_Require7_xac_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView tree_Choose_Export;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OK_EXPORT;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox checkBox_Szenar_Aufloesung;
        public System.Windows.Forms.CheckBox checkBox_Ueberpreufung;
        public System.Windows.Forms.CheckBox checkBox_Sysele;
    }
}
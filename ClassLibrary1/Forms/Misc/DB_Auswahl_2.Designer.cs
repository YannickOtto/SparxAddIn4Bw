
namespace Requirement_Plugin.Forms
{
    public partial class DB_Auswahl_2
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip_DB_Auswahl = new System.Windows.Forms.MenuStrip();
            this.auswahlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manuellEingabeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_parent = new System.Windows.Forms.Panel();
            this.panel_auto = new System.Windows.Forms.Panel();
            this.tableLayoutPanel_auto = new System.Windows.Forms.TableLayoutPanel();
            this.label_Auto_Dropdown = new System.Windows.Forms.Label();
            this.comboBox_repository = new System.Windows.Forms.ComboBox();
            this.label_auto_Constring = new System.Windows.Forms.Label();
            this.richTextBox_auto = new System.Windows.Forms.RichTextBox();
            this.panel_manuell = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel_manuell = new System.Windows.Forms.TableLayoutPanel();
            this.label_manuell_connectionstring = new System.Windows.Forms.Label();
            this.richTextBox_manuell = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuStrip_DB_Auswahl.SuspendLayout();
            this.panel_parent.SuspendLayout();
            this.panel_auto.SuspendLayout();
            this.tableLayoutPanel_auto.SuspendLayout();
            this.panel_manuell.SuspendLayout();
            this.tableLayoutPanel_manuell.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.56767F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.43233F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel1.Controls.Add(this.menuStrip_DB_Auswahl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel_parent, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonOK, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_cancel, 3, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1171, 681);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // menuStrip_DB_Auswahl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.menuStrip_DB_Auswahl, 4);
            this.menuStrip_DB_Auswahl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip_DB_Auswahl.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip_DB_Auswahl.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip_DB_Auswahl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.auswahlToolStripMenuItem,
            this.manuellEingabeToolStripMenuItem});
            this.menuStrip_DB_Auswahl.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_DB_Auswahl.Name = "menuStrip_DB_Auswahl";
            this.menuStrip_DB_Auswahl.Size = new System.Drawing.Size(1171, 102);
            this.menuStrip_DB_Auswahl.TabIndex = 0;
            this.menuStrip_DB_Auswahl.Text = "menuStrip1";
            // 
            // auswahlToolStripMenuItem
            // 
            this.auswahlToolStripMenuItem.Name = "auswahlToolStripMenuItem";
            this.auswahlToolStripMenuItem.Size = new System.Drawing.Size(94, 98);
            this.auswahlToolStripMenuItem.Text = "Auswahl";
            this.auswahlToolStripMenuItem.Click += new System.EventHandler(this.auswahlToolStripMenuItem_Click);
            // 
            // manuellEingabeToolStripMenuItem
            // 
            this.manuellEingabeToolStripMenuItem.Name = "manuellEingabeToolStripMenuItem";
            this.manuellEingabeToolStripMenuItem.Size = new System.Drawing.Size(158, 98);
            this.manuellEingabeToolStripMenuItem.Text = "Manuell Eingabe";
            this.manuellEingabeToolStripMenuItem.Click += new System.EventHandler(this.manuellEingabeToolStripMenuItem_Click);
            // 
            // panel_parent
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel_parent, 4);
            this.panel_parent.Controls.Add(this.panel_manuell);
            this.panel_parent.Controls.Add(this.panel_auto);
            this.panel_parent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_parent.Location = new System.Drawing.Point(3, 105);
            this.panel_parent.Name = "panel_parent";
            this.panel_parent.Size = new System.Drawing.Size(1165, 470);
            this.panel_parent.TabIndex = 2;
            // 
            // panel_auto
            // 
            this.panel_auto.Controls.Add(this.tableLayoutPanel_auto);
            this.panel_auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_auto.Location = new System.Drawing.Point(0, 0);
            this.panel_auto.Name = "panel_auto";
            this.panel_auto.Size = new System.Drawing.Size(1165, 470);
            this.panel_auto.TabIndex = 3;
            // 
            // tableLayoutPanel_auto
            // 
            this.tableLayoutPanel_auto.ColumnCount = 2;
            this.tableLayoutPanel_auto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.40343F));
            this.tableLayoutPanel_auto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.59657F));
            this.tableLayoutPanel_auto.Controls.Add(this.label_Auto_Dropdown, 0, 0);
            this.tableLayoutPanel_auto.Controls.Add(this.comboBox_repository, 0, 1);
            this.tableLayoutPanel_auto.Controls.Add(this.label_auto_Constring, 1, 0);
            this.tableLayoutPanel_auto.Controls.Add(this.richTextBox_auto, 1, 1);
            this.tableLayoutPanel_auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_auto.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_auto.Name = "tableLayoutPanel_auto";
            this.tableLayoutPanel_auto.RowCount = 2;
            this.tableLayoutPanel_auto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.6383F));
            this.tableLayoutPanel_auto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.3617F));
            this.tableLayoutPanel_auto.Size = new System.Drawing.Size(1165, 470);
            this.tableLayoutPanel_auto.TabIndex = 0;
            // 
            // label_Auto_Dropdown
            // 
            this.label_Auto_Dropdown.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_Auto_Dropdown.AutoSize = true;
            this.label_Auto_Dropdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Auto_Dropdown.Location = new System.Drawing.Point(3, 10);
            this.label_Auto_Dropdown.Name = "label_Auto_Dropdown";
            this.label_Auto_Dropdown.Size = new System.Drawing.Size(139, 29);
            this.label_Auto_Dropdown.TabIndex = 0;
            this.label_Auto_Dropdown.Text = "Repository";
            // 
            // comboBox_repository
            // 
            this.comboBox_repository.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_repository.FormattingEnabled = true;
            this.comboBox_repository.Location = new System.Drawing.Point(3, 53);
            this.comboBox_repository.Name = "comboBox_repository";
            this.comboBox_repository.Size = new System.Drawing.Size(487, 28);
            this.comboBox_repository.TabIndex = 1;
            this.comboBox_repository.SelectedIndexChanged += new System.EventHandler(this.comboBox_repository_SelectedIndexChanged);
            // 
            // label_auto_Constring
            // 
            this.label_auto_Constring.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_auto_Constring.AutoSize = true;
            this.label_auto_Constring.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_auto_Constring.Location = new System.Drawing.Point(496, 10);
            this.label_auto_Constring.Name = "label_auto_Constring";
            this.label_auto_Constring.Size = new System.Drawing.Size(221, 29);
            this.label_auto_Constring.TabIndex = 2;
            this.label_auto_Constring.Text = "Connection String";
            // 
            // richTextBox_auto
            // 
            this.richTextBox_auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_auto.Location = new System.Drawing.Point(496, 53);
            this.richTextBox_auto.Name = "richTextBox_auto";
            this.richTextBox_auto.Size = new System.Drawing.Size(666, 414);
            this.richTextBox_auto.TabIndex = 3;
            this.richTextBox_auto.Text = "";
            // 
            // panel_manuell
            // 
            this.panel_manuell.Controls.Add(this.tableLayoutPanel_manuell);
            this.panel_manuell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_manuell.Location = new System.Drawing.Point(0, 0);
            this.panel_manuell.Name = "panel_manuell";
            this.panel_manuell.Size = new System.Drawing.Size(1165, 470);
            this.panel_manuell.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Location = new System.Drawing.Point(954, 581);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(104, 97);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_cancel.Location = new System.Drawing.Point(1064, 581);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(104, 97);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // tableLayoutPanel_manuell
            // 
            this.tableLayoutPanel_manuell.ColumnCount = 2;
            this.tableLayoutPanel_manuell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.95279F));
            this.tableLayoutPanel_manuell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.04721F));
            this.tableLayoutPanel_manuell.Controls.Add(this.label_manuell_connectionstring, 1, 0);
            this.tableLayoutPanel_manuell.Controls.Add(this.richTextBox_manuell, 1, 1);
            this.tableLayoutPanel_manuell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_manuell.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_manuell.Name = "tableLayoutPanel_manuell";
            this.tableLayoutPanel_manuell.RowCount = 2;
            this.tableLayoutPanel_manuell.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.97872F));
            this.tableLayoutPanel_manuell.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.02128F));
            this.tableLayoutPanel_manuell.Size = new System.Drawing.Size(1165, 470);
            this.tableLayoutPanel_manuell.TabIndex = 0;
            // 
            // label_manuell_connectionstring
            // 
            this.label_manuell_connectionstring.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_manuell_connectionstring.AutoSize = true;
            this.label_manuell_connectionstring.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_manuell_connectionstring.Location = new System.Drawing.Point(549, 16);
            this.label_manuell_connectionstring.Name = "label_manuell_connectionstring";
            this.label_manuell_connectionstring.Size = new System.Drawing.Size(221, 29);
            this.label_manuell_connectionstring.TabIndex = 0;
            this.label_manuell_connectionstring.Text = "Connection String";
            // 
            // richTextBox_manuell
            // 
            this.richTextBox_manuell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_manuell.Location = new System.Drawing.Point(549, 64);
            this.richTextBox_manuell.Name = "richTextBox_manuell";
            this.richTextBox_manuell.Size = new System.Drawing.Size(613, 403);
            this.richTextBox_manuell.TabIndex = 1;
            this.richTextBox_manuell.Text = "";
            // 
            // DB_Auswahl_2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1171, 681);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MainMenuStrip = this.menuStrip_DB_Auswahl;
            this.Name = "DB_Auswahl_2";
            this.Text = "Auswahgl Repository";
            this.Load += new System.EventHandler(this.DB_Auswahl_2_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuStrip_DB_Auswahl.ResumeLayout(false);
            this.menuStrip_DB_Auswahl.PerformLayout();
            this.panel_parent.ResumeLayout(false);
            this.panel_auto.ResumeLayout(false);
            this.tableLayoutPanel_auto.ResumeLayout(false);
            this.tableLayoutPanel_auto.PerformLayout();
            this.panel_manuell.ResumeLayout(false);
            this.tableLayoutPanel_manuell.ResumeLayout(false);
            this.tableLayoutPanel_manuell.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip_DB_Auswahl;
        private System.Windows.Forms.ToolStripMenuItem auswahlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manuellEingabeToolStripMenuItem;
        private System.Windows.Forms.Panel panel_manuell;
        private System.Windows.Forms.Panel panel_parent;
        private System.Windows.Forms.Panel panel_auto;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_auto;
        private System.Windows.Forms.Label label_Auto_Dropdown;
        private System.Windows.Forms.ComboBox comboBox_repository;
        private System.Windows.Forms.Label label_auto_Constring;
        private System.Windows.Forms.RichTextBox richTextBox_auto;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_manuell;
        private System.Windows.Forms.Label label_manuell_connectionstring;
        private System.Windows.Forms.RichTextBox richTextBox_manuell;
    }
}
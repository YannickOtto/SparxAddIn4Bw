
namespace Requirement_Plugin.Forms.AFO_Mgmt
{
    partial class Auswahl_AFO_Mgmt_Connectoren
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
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Überprüfung Verfeinerungs-Schleifen");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Überprüfung mehrere, ausgehende Verfeinerungs-Konnektoren");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Überprüfung Auslösung Dopplungen");
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView_check = new System.Windows.Forms.TreeView();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label_Check = new System.Windows.Forms.Label();
            this.button_check = new System.Windows.Forms.Button();
            this.checkBox_Modus = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.78001F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.17141F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_OK, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_cancel, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_Check, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_check, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_Modus, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.81374F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.12682F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1441, 757);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.treeView_check, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 100);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.2963F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.703704F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(881, 540);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // treeView_check
            // 
            this.treeView_check.CheckBoxes = true;
            this.treeView_check.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_check.Location = new System.Drawing.Point(3, 3);
            this.treeView_check.Name = "treeView_check";
            treeNode4.Name = "Knoten_Loops";
            treeNode4.Text = "Überprüfung Verfeinerungs-Schleifen";
            treeNode5.Name = "Knoten_Mult_Feines";
            treeNode5.Text = "Überprüfung mehrere, ausgehende Verfeinerungs-Konnektoren";
            treeNode6.Name = "Knoten_Dopplung";
            treeNode6.Text = "Überprüfung Auslösung Dopplungen";
            this.treeView_check.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6});
            this.treeView_check.Size = new System.Drawing.Size(875, 513);
            this.treeView_check.TabIndex = 4;
            // 
            // button_OK
            // 
            this.button_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_OK.Location = new System.Drawing.Point(1062, 646);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(184, 108);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_cancel.Location = new System.Drawing.Point(1252, 646);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(186, 108);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label_Check
            // 
            this.label_Check.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_Check.AutoSize = true;
            this.label_Check.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Check.Location = new System.Drawing.Point(3, 19);
            this.label_Check.Name = "label_Check";
            this.label_Check.Size = new System.Drawing.Size(276, 58);
            this.label_Check.TabIndex = 3;
            this.label_Check.Text = "Bitte wählen Sie die Überprüfungsfälle aus.";
            // 
            // button_check
            // 
            this.button_check.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_check.Location = new System.Drawing.Point(890, 646);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(166, 108);
            this.button_check.TabIndex = 4;
            this.button_check.Text = "Check All";
            this.button_check.UseVisualStyleBackColor = true;
            this.button_check.Click += new System.EventHandler(this.button_check_Click);
            // 
            // checkBox_Modus
            // 
            this.checkBox_Modus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBox_Modus, 2);
            this.checkBox_Modus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_Modus.Location = new System.Drawing.Point(890, 100);
            this.checkBox_Modus.Name = "checkBox_Modus";
            this.checkBox_Modus.Size = new System.Drawing.Size(356, 540);
            this.checkBox_Modus.TabIndex = 5;
            this.checkBox_Modus.Text = "Nur Export Anforderungen berücksichtigen";
            this.checkBox_Modus.UseVisualStyleBackColor = true;
            // 
            // Auswahl_AFO_Mgmt_Connectoren
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1441, 757);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Auswahl_AFO_Mgmt_Connectoren";
            this.Text = "Auswahl Überprüfung Connectoren";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_Check;
        private System.Windows.Forms.TreeView treeView_check;
        private System.Windows.Forms.Button button_check;
        private System.Windows.Forms.CheckBox checkBox_Modus;
    }
}
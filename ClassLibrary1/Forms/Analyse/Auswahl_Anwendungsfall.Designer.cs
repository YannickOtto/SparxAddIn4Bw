
namespace Requirement_Plugin.Forms
{
    partial class Auswahl_Anwendungsfall
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
            this.tableLayoutPanel_Anbwendungsfdall = new System.Windows.Forms.TableLayoutPanel();
            this.label_Anwendungsfall = new System.Windows.Forms.Label();
            this.treeView_Anwendungsfall = new System.Windows.Forms.TreeView();
            this.button_Check = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox_Hinweis = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel_Anbwendungsfdall.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_Anbwendungsfdall
            // 
            this.tableLayoutPanel_Anbwendungsfdall.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel_Anbwendungsfdall.ColumnCount = 6;
            this.tableLayoutPanel_Anbwendungsfdall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_Anbwendungsfdall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9F));
            this.tableLayoutPanel_Anbwendungsfdall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel_Anbwendungsfdall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_Anbwendungsfdall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9F));
            this.tableLayoutPanel_Anbwendungsfdall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9F));
            this.tableLayoutPanel_Anbwendungsfdall.Controls.Add(this.label_Anwendungsfall, 0, 0);
            this.tableLayoutPanel_Anbwendungsfdall.Controls.Add(this.treeView_Anwendungsfall, 0, 1);
            this.tableLayoutPanel_Anbwendungsfdall.Controls.Add(this.button_Check, 1, 2);
            this.tableLayoutPanel_Anbwendungsfdall.Controls.Add(this.buttonOK, 4, 3);
            this.tableLayoutPanel_Anbwendungsfdall.Controls.Add(this.button_cancel, 5, 3);
            this.tableLayoutPanel_Anbwendungsfdall.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel_Anbwendungsfdall.Controls.Add(this.richTextBox_Hinweis, 3, 1);
            this.tableLayoutPanel_Anbwendungsfdall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Anbwendungsfdall.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_Anbwendungsfdall.Name = "tableLayoutPanel_Anbwendungsfdall";
            this.tableLayoutPanel_Anbwendungsfdall.RowCount = 4;
            this.tableLayoutPanel_Anbwendungsfdall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel_Anbwendungsfdall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72F));
            this.tableLayoutPanel_Anbwendungsfdall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9F));
            this.tableLayoutPanel_Anbwendungsfdall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9F));
            this.tableLayoutPanel_Anbwendungsfdall.Size = new System.Drawing.Size(1347, 795);
            this.tableLayoutPanel_Anbwendungsfdall.TabIndex = 0;
            // 
            // label_Anwendungsfall
            // 
            this.label_Anwendungsfall.AutoSize = true;
            this.tableLayoutPanel_Anbwendungsfdall.SetColumnSpan(this.label_Anwendungsfall, 2);
            this.label_Anwendungsfall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Anwendungsfall.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Anwendungsfall.Location = new System.Drawing.Point(3, 0);
            this.label_Anwendungsfall.Name = "label_Anwendungsfall";
            this.label_Anwendungsfall.Size = new System.Drawing.Size(384, 79);
            this.label_Anwendungsfall.TabIndex = 0;
            this.label_Anwendungsfall.Text = "Anwendungsfälle";
            this.label_Anwendungsfall.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // treeView_Anwendungsfall
            // 
            this.tableLayoutPanel_Anbwendungsfdall.SetColumnSpan(this.treeView_Anwendungsfall, 3);
            this.treeView_Anwendungsfall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Anwendungsfall.Location = new System.Drawing.Point(3, 82);
            this.treeView_Anwendungsfall.Name = "treeView_Anwendungsfall";
            this.treeView_Anwendungsfall.Size = new System.Drawing.Size(828, 566);
            this.treeView_Anwendungsfall.TabIndex = 1;
            this.treeView_Anwendungsfall.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Anwendungsfall_AfterCheck);
            this.treeView_Anwendungsfall.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Anwendungsfall_AfterSelect);
            // 
            // button_Check
            // 
            this.button_Check.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Check.Location = new System.Drawing.Point(272, 654);
            this.button_Check.Name = "button_Check";
            this.button_Check.Size = new System.Drawing.Size(115, 65);
            this.button_Check.TabIndex = 2;
            this.button_Check.Text = "Check All";
            this.button_Check.UseVisualStyleBackColor = true;
            this.button_Check.Click += new System.EventHandler(this.button_Check_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Location = new System.Drawing.Point(1106, 725);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(115, 67);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_cancel.Location = new System.Drawing.Point(1227, 725);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(117, 67);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(837, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 79);
            this.label1.TabIndex = 5;
            this.label1.Text = "Hinweise";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBox_Hinweis
            // 
            this.richTextBox_Hinweis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Hinweis.Location = new System.Drawing.Point(837, 82);
            this.richTextBox_Hinweis.Name = "richTextBox_Hinweis";
            this.richTextBox_Hinweis.ReadOnly = true;
            this.richTextBox_Hinweis.Size = new System.Drawing.Size(263, 566);
            this.richTextBox_Hinweis.TabIndex = 6;
            this.richTextBox_Hinweis.Text = "";
            // 
            // Auswahl_Anwendungsfall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1347, 795);
            this.Controls.Add(this.tableLayoutPanel_Anbwendungsfdall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Auswahl_Anwendungsfall";
            this.Text = "Auswahl Anwendungsfälle";
            this.Load += new System.EventHandler(this.Auswahl_Anwendungsfall_Load);
            this.tableLayoutPanel_Anbwendungsfdall.ResumeLayout(false);
            this.tableLayoutPanel_Anbwendungsfdall.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Anbwendungsfdall;
        private System.Windows.Forms.Label label_Anwendungsfall;
        private System.Windows.Forms.TreeView treeView_Anwendungsfall;
        private System.Windows.Forms.Button button_Check;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_Hinweis;
    }
}

namespace Requirement_Plugin.Forms.Bewertung_Afo
{
    partial class Bewertung_UI
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.richTextBox_Hinweis = new System.Windows.Forms.RichTextBox();
            this.button_Reset = new System.Windows.Forms.Button();
            this.button_Gewichtung = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 521);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.90938F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.09062F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 189F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel1.Controls.Add(this.richTextBox_Hinweis, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_Reset, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_Gewichtung, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_OK, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.button_Cancel, 4, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.12346F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.87654F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 199F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1028, 521);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // richTextBox_Hinweis
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBox_Hinweis, 2);
            this.richTextBox_Hinweis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Hinweis.Location = new System.Drawing.Point(3, 65);
            this.richTextBox_Hinweis.Name = "richTextBox_Hinweis";
            this.richTextBox_Hinweis.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.richTextBox_Hinweis, 2);
            this.richTextBox_Hinweis.Size = new System.Drawing.Size(583, 171);
            this.richTextBox_Hinweis.TabIndex = 1;
            this.richTextBox_Hinweis.Text = "";
            // 
            // button_Reset
            // 
            this.button_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Reset.Location = new System.Drawing.Point(592, 65);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(183, 87);
            this.button_Reset.TabIndex = 2;
            this.button_Reset.Text = "Reset Gewichtung";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // button_Gewichtung
            // 
            this.button_Gewichtung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Gewichtung.Location = new System.Drawing.Point(592, 158);
            this.button_Gewichtung.Name = "button_Gewichtung";
            this.button_Gewichtung.Size = new System.Drawing.Size(183, 78);
            this.button_Gewichtung.TabIndex = 3;
            this.button_Gewichtung.Text = "Automatische Gewichtung";
            this.button_Gewichtung.UseVisualStyleBackColor = true;
            this.button_Gewichtung.Click += new System.EventHandler(this.button_Gewichtung_Click);
            // 
            // button_OK
            // 
            this.button_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_OK.Location = new System.Drawing.Point(781, 441);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(118, 77);
            this.button_OK.TabIndex = 4;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Cancel.Location = new System.Drawing.Point(905, 441);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(120, 77);
            this.button_Cancel.TabIndex = 5;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // Bewertung_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1028, 521);
            this.Controls.Add(this.panel1);
            this.Name = "Bewertung_UI";
            this.Text = "Gewichtung Anforderungen";
            this.Load += new System.EventHandler(this.Bewertung_UI_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox richTextBox_Hinweis;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.Button button_Gewichtung;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
    }
}
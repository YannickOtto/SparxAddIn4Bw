namespace Forms
{
    partial class DB_Auswahl
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
            this.label_User_ID = new System.Windows.Forms.Label();
            this.richText_User_ID = new System.Windows.Forms.RichTextBox();
            this.label_Password = new System.Windows.Forms.Label();
            this.label_Connection_String = new System.Windows.Forms.Label();
            this.richText_Password = new System.Windows.Forms.RichTextBox();
            this.richText_Connection_String = new System.Windows.Forms.RichTextBox();
            this.button_Save = new System.Windows.Forms.Button();
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
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.875F));
            this.tableLayoutPanel1.Controls.Add(this.label_User_ID, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.richText_User_ID, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_Password, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_Connection_String, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.richText_Password, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.richText_Connection_String, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.button_Save, 1, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label_User_ID
            // 
            this.label_User_ID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_User_ID.AutoSize = true;
            this.label_User_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_User_ID.Location = new System.Drawing.Point(246, 13);
            this.label_User_ID.Name = "label_User_ID";
            this.label_User_ID.Size = new System.Drawing.Size(100, 29);
            this.label_User_ID.TabIndex = 0;
            this.label_User_ID.Text = "User ID";
            this.label_User_ID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richText_User_ID
            // 
            this.richText_User_ID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richText_User_ID.Location = new System.Drawing.Point(3, 59);
            this.richText_User_ID.Name = "richText_User_ID";
            this.richText_User_ID.Size = new System.Drawing.Size(587, 50);
            this.richText_User_ID.TabIndex = 1;
            this.richText_User_ID.Text = "";
            // 
            // label_Password
            // 
            this.label_Password.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_Password.AutoSize = true;
            this.label_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Password.Location = new System.Drawing.Point(232, 125);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(128, 29);
            this.label_Password.TabIndex = 2;
            this.label_Password.Text = "Password";
            this.label_Password.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Connection_String
            // 
            this.label_Connection_String.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_Connection_String.AutoSize = true;
            this.label_Connection_String.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Connection_String.Location = new System.Drawing.Point(182, 237);
            this.label_Connection_String.Name = "label_Connection_String";
            this.label_Connection_String.Size = new System.Drawing.Size(228, 29);
            this.label_Connection_String.TabIndex = 3;
            this.label_Connection_String.Text = "Connection_String";
            this.label_Connection_String.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richText_Password
            // 
            this.richText_Password.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richText_Password.Location = new System.Drawing.Point(3, 171);
            this.richText_Password.Name = "richText_Password";
            this.richText_Password.Size = new System.Drawing.Size(587, 50);
            this.richText_Password.TabIndex = 4;
            this.richText_Password.Text = "";
            // 
            // richText_Connection_String
            // 
            this.richText_Connection_String.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richText_Connection_String.Location = new System.Drawing.Point(3, 283);
            this.richText_Connection_String.Name = "richText_Connection_String";
            this.tableLayoutPanel1.SetRowSpan(this.richText_Connection_String, 2);
            this.richText_Connection_String.Size = new System.Drawing.Size(587, 106);
            this.richText_Connection_String.TabIndex = 5;
            this.richText_Connection_String.Text = "";
            // 
            // button_Save
            // 
            this.button_Save.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Save.Location = new System.Drawing.Point(596, 395);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(201, 52);
            this.button_Save.TabIndex = 6;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // DB_Auswahl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DB_Auswahl";
            this.Text = "Auswahl Datenbank";
            this.Load += new System.EventHandler(this.DB_Auswahl_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_User_ID;
        private System.Windows.Forms.RichTextBox richText_User_ID;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.Label label_Connection_String;
        private System.Windows.Forms.RichTextBox richText_Password;
        private System.Windows.Forms.RichTextBox richText_Connection_String;
        private System.Windows.Forms.Button button_Save;
    }
}
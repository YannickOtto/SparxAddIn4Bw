namespace Forms
{
    public partial class Synch_MM
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
            this.button_Synch_All = new System.Windows.Forms.Button();
            this.button_Synch_Con = new System.Windows.Forms.Button();
            this.button_Synch_Elem = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.button_Synch_All, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.button_Synch_Con, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.button_Synch_Elem, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(322, 364);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button_Synch_All
            // 
            this.button_Synch_All.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Synch_All.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Synch_All.Location = new System.Drawing.Point(164, 291);
            this.button_Synch_All.Name = "button_Synch_All";
            this.button_Synch_All.Size = new System.Drawing.Size(155, 70);
            this.button_Synch_All.TabIndex = 0;
            this.button_Synch_All.Text = "Synchronize All";
            this.button_Synch_All.UseVisualStyleBackColor = true;
            this.button_Synch_All.Click += new System.EventHandler(this.button_Synch_All_Click);
            // 
            // button_Synch_Con
            // 
            this.button_Synch_Con.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Synch_Con.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Synch_Con.Location = new System.Drawing.Point(3, 219);
            this.button_Synch_Con.Name = "button_Synch_Con";
            this.button_Synch_Con.Size = new System.Drawing.Size(155, 66);
            this.button_Synch_Con.TabIndex = 1;
            this.button_Synch_Con.Text = "Synchronize Connector";
            this.button_Synch_Con.UseVisualStyleBackColor = true;
            this.button_Synch_Con.Click += new System.EventHandler(this.button_Synch_Con_Click);
            // 
            // button_Synch_Elem
            // 
            this.button_Synch_Elem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Synch_Elem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Synch_Elem.Location = new System.Drawing.Point(3, 75);
            this.button_Synch_Elem.Name = "button_Synch_Elem";
            this.button_Synch_Elem.Size = new System.Drawing.Size(155, 66);
            this.button_Synch_Elem.TabIndex = 2;
            this.button_Synch_Elem.Text = "Synchronize Elements";
            this.button_Synch_Elem.UseVisualStyleBackColor = true;
            this.button_Synch_Elem.Click += new System.EventHandler(this.button_Synch_Elem_Click);
            // 
            // Synch_MM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(322, 364);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Synch_MM";
            this.Text = "Synchronisierung Metamodel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button_Synch_All;
        private System.Windows.Forms.Button button_Synch_Con;
        private System.Windows.Forms.Button button_Synch_Elem;
    }
}
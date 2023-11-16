namespace MyAddin
{
    partial class Requirement_Interface
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
            this.components = new System.ComponentModel.Container();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.databaseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Bidirektional = new System.Windows.Forms.CheckBox();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.Text_Empfangen = new System.Windows.Forms.TextBox();
            this.AFo_Titel_Empfangen = new System.Windows.Forms.Label();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.Text_Senden = new System.Windows.Forms.TextBox();
            this.AFo_Titel_Senden = new System.Windows.Forms.Label();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.InfoElemBox = new System.Windows.Forms.CheckedListBox();
            this.label_InformationElement = new System.Windows.Forms.Label();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.Target_NodeType_Box = new System.Windows.Forms.ListBox();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.Target_Artikel = new System.Windows.Forms.Button();
            this.label_Supplier = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.Szenar_Box = new System.Windows.Forms.CheckedListBox();
            this.label_Szenar = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.NodeType_Box = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.NodeType_Artikel = new System.Windows.Forms.Button();
            this.label_Client = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.databaseBindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(756, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(8, 4);
            this.listBox1.TabIndex = 1;
            // 
            // databaseBindingSource
            // 
            this.databaseBindingSource.DataSource = typeof(MyAddin.Database);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.09225F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.84625F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer6, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer7, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer8, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.Bidirektional, 0, 2);
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.5042F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.4958F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(821, 494);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // Bidirektional
            // 
            this.Bidirektional.AutoSize = true;
            this.Bidirektional.Location = new System.Drawing.Point(3, 422);
            this.Bidirektional.Name = "Bidirektional";
            this.Bidirektional.Size = new System.Drawing.Size(107, 21);
            this.Bidirektional.TabIndex = 18;
            this.Bidirektional.Text = "Bidirektional";
            this.Bidirektional.UseVisualStyleBackColor = true;
            this.Bidirektional.CheckedChanged += new System.EventHandler(this.Bidirektional_CheckedChanged);
            // 
            // splitContainer8
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.splitContainer8, 2);
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.Location = new System.Drawing.Point(414, 257);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.AFo_Titel_Empfangen);
            this.splitContainer8.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer8_Panel1_Paint);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.Text_Empfangen);
            this.splitContainer8.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer8_Panel2_Paint);
            this.tableLayoutPanel1.SetRowSpan(this.splitContainer8, 2);
            this.splitContainer8.Size = new System.Drawing.Size(404, 199);
            this.splitContainer8.SplitterDistance = 36;
            this.splitContainer8.TabIndex = 17;
            // 
            // Text_Empfangen
            // 
            this.Text_Empfangen.Location = new System.Drawing.Point(-733, 378);
            this.Text_Empfangen.Multiline = true;
            this.Text_Empfangen.Name = "Text_Empfangen";
            this.Text_Empfangen.Size = new System.Drawing.Size(393, 142);
            this.Text_Empfangen.TabIndex = 0;
            // 
            // AFo_Titel_Empfangen
            // 
            this.AFo_Titel_Empfangen.AutoSize = true;
            this.AFo_Titel_Empfangen.Location = new System.Drawing.Point(3, 9);
            this.AFo_Titel_Empfangen.Name = "AFo_Titel_Empfangen";
            this.AFo_Titel_Empfangen.Size = new System.Drawing.Size(36, 17);
            this.AFo_Titel_Empfangen.TabIndex = 0;
            this.AFo_Titel_Empfangen.Text = "Test";
            // 
            // splitContainer7
            // 
            this.splitContainer7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.splitContainer7, 2);
            this.splitContainer7.Location = new System.Drawing.Point(414, 3);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.AFo_Titel_Senden);
            this.splitContainer7.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer7_Panel1_Paint);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.textBox1);
            this.splitContainer7.Panel2.Controls.Add(this.Text_Senden);
            this.splitContainer7.Size = new System.Drawing.Size(404, 248);
            this.splitContainer7.SplitterDistance = 43;
            this.splitContainer7.TabIndex = 16;
            // 
            // Text_Senden
            // 
            this.Text_Senden.Location = new System.Drawing.Point(0, 3);
            this.Text_Senden.Multiline = true;
            this.Text_Senden.Name = "Text_Senden";
            this.Text_Senden.Size = new System.Drawing.Size(394, 164);
            this.Text_Senden.TabIndex = 15;
            this.Text_Senden.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // AFo_Titel_Senden
            // 
            this.AFo_Titel_Senden.AutoSize = true;
            this.AFo_Titel_Senden.Location = new System.Drawing.Point(3, 11);
            this.AFo_Titel_Senden.Name = "AFo_Titel_Senden";
            this.AFo_Titel_Senden.Size = new System.Drawing.Size(36, 17);
            this.AFo_Titel_Senden.TabIndex = 0;
            this.AFo_Titel_Senden.Text = "Test";
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(208, 257);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.label_InformationElement);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.InfoElemBox);
            this.splitContainer6.Size = new System.Drawing.Size(200, 159);
            this.splitContainer6.SplitterDistance = 26;
            this.splitContainer6.TabIndex = 14;
            // 
            // InfoElemBox
            // 
            this.InfoElemBox.BackColor = System.Drawing.SystemColors.Window;
            this.InfoElemBox.FormattingEnabled = true;
            this.InfoElemBox.Location = new System.Drawing.Point(3, 3);
            this.InfoElemBox.Name = "InfoElemBox";
            this.InfoElemBox.Size = new System.Drawing.Size(184, 106);
            this.InfoElemBox.TabIndex = 3;
            this.InfoElemBox.SelectedIndexChanged += new System.EventHandler(this.InfoElemBox_SelectedIndexChanged);
            // 
            // label_InformationElement
            // 
            this.label_InformationElement.AutoSize = true;
            this.label_InformationElement.Location = new System.Drawing.Point(3, 9);
            this.label_InformationElement.Name = "label_InformationElement";
            this.label_InformationElement.Size = new System.Drawing.Size(129, 17);
            this.label_InformationElement.TabIndex = 9;
            this.label_InformationElement.Text = "InformationElement";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(208, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.Target_NodeType_Box);
            this.splitContainer4.Size = new System.Drawing.Size(200, 248);
            this.splitContainer4.SplitterDistance = 45;
            this.splitContainer4.TabIndex = 13;
            // 
            // Target_NodeType_Box
            // 
            this.Target_NodeType_Box.ItemHeight = 16;
            this.Target_NodeType_Box.Location = new System.Drawing.Point(0, 1);
            this.Target_NodeType_Box.Name = "Target_NodeType_Box";
            this.Target_NodeType_Box.Size = new System.Drawing.Size(198, 164);
            this.Target_NodeType_Box.TabIndex = 1;
            this.Target_NodeType_Box.SelectedIndexChanged += new System.EventHandler(this.Target_NodeType_Box_SelectedIndexChanged);
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.label_Supplier);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.Target_Artikel);
            this.splitContainer5.Size = new System.Drawing.Size(200, 45);
            this.splitContainer5.SplitterDistance = 86;
            this.splitContainer5.TabIndex = 0;
            // 
            // Target_Artikel
            // 
            this.Target_Artikel.Location = new System.Drawing.Point(12, 8);
            this.Target_Artikel.Name = "Target_Artikel";
            this.Target_Artikel.Size = new System.Drawing.Size(75, 23);
            this.Target_Artikel.TabIndex = 5;
            this.Target_Artikel.Text = "der";
            this.Target_Artikel.UseVisualStyleBackColor = true;
            this.Target_Artikel.Click += new System.EventHandler(this.Target_Artikel_Click);
            // 
            // label_Supplier
            // 
            this.label_Supplier.AutoSize = true;
            this.label_Supplier.Location = new System.Drawing.Point(15, 8);
            this.label_Supplier.Name = "label_Supplier";
            this.label_Supplier.Size = new System.Drawing.Size(60, 17);
            this.label_Supplier.TabIndex = 8;
            this.label_Supplier.Text = "Supplier";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 257);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.label_Szenar);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.Szenar_Box);
            this.splitContainer3.Size = new System.Drawing.Size(199, 159);
            this.splitContainer3.SplitterDistance = 26;
            this.splitContainer3.TabIndex = 12;
            // 
            // Szenar_Box
            // 
            this.Szenar_Box.FormattingEnabled = true;
            this.Szenar_Box.Location = new System.Drawing.Point(3, 3);
            this.Szenar_Box.Name = "Szenar_Box";
            this.Szenar_Box.Size = new System.Drawing.Size(184, 106);
            this.Szenar_Box.TabIndex = 2;
            this.Szenar_Box.SelectedIndexChanged += new System.EventHandler(this.Szenar_Box_SelectedIndexChanged);
            // 
            // label_Szenar
            // 
            this.label_Szenar.AutoSize = true;
            this.label_Szenar.Location = new System.Drawing.Point(0, 9);
            this.label_Szenar.Name = "label_Szenar";
            this.label_Szenar.Size = new System.Drawing.Size(53, 17);
            this.label_Szenar.TabIndex = 10;
            this.label_Szenar.Text = "Szenar";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.NodeType_Box);
            this.splitContainer1.Size = new System.Drawing.Size(199, 248);
            this.splitContainer1.SplitterDistance = 45;
            this.splitContainer1.TabIndex = 11;
            // 
            // NodeType_Box
            // 
            this.NodeType_Box.FormattingEnabled = true;
            this.NodeType_Box.ItemHeight = 16;
            this.NodeType_Box.Location = new System.Drawing.Point(6, 3);
            this.NodeType_Box.Name = "NodeType_Box";
            this.NodeType_Box.Size = new System.Drawing.Size(188, 164);
            this.NodeType_Box.TabIndex = 0;
            this.NodeType_Box.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label_Client);
            this.splitContainer2.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel1_Paint);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.NodeType_Artikel);
            this.splitContainer2.Size = new System.Drawing.Size(199, 45);
            this.splitContainer2.SplitterDistance = 86;
            this.splitContainer2.TabIndex = 0;
            // 
            // NodeType_Artikel
            // 
            this.NodeType_Artikel.Location = new System.Drawing.Point(13, 8);
            this.NodeType_Artikel.Name = "NodeType_Artikel";
            this.NodeType_Artikel.Size = new System.Drawing.Size(74, 23);
            this.NodeType_Artikel.TabIndex = 4;
            this.NodeType_Artikel.Text = "der";
            this.NodeType_Artikel.UseVisualStyleBackColor = true;
            this.NodeType_Artikel.Click += new System.EventHandler(this.NodeType_Artikel_Click);
            // 
            // label_Client
            // 
            this.label_Client.AutoSize = true;
            this.label_Client.Location = new System.Drawing.Point(24, 8);
            this.label_Client.Name = "label_Client";
            this.label_Client.Size = new System.Drawing.Size(43, 17);
            this.label_Client.TabIndex = 7;
            this.label_Client.Text = "Client";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1066, 509);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(-2, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(394, 164);
            this.textBox1.TabIndex = 15;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Requirement_Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 509);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Requirement_Interface";
            this.Text = "Layout_Interface";
            this.Load += new System.EventHandler(this.Layout_Interface_Load);
            ((System.ComponentModel.ISupportInitialize)(this.databaseBindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel1.PerformLayout();
            this.splitContainer8.Panel2.ResumeLayout(false);
            this.splitContainer8.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel1.PerformLayout();
            this.splitContainer7.Panel2.ResumeLayout(false);
            this.splitContainer7.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource databaseBindingSource;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label_Client;
        private System.Windows.Forms.Button NodeType_Artikel;
        private System.Windows.Forms.ListBox NodeType_Box;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label label_Szenar;
        private System.Windows.Forms.CheckedListBox Szenar_Box;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Label label_Supplier;
        private System.Windows.Forms.Button Target_Artikel;
        private System.Windows.Forms.ListBox Target_NodeType_Box;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.Label label_InformationElement;
        private System.Windows.Forms.CheckedListBox InfoElemBox;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.Label AFo_Titel_Senden;
        private System.Windows.Forms.TextBox Text_Senden;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.Label AFo_Titel_Empfangen;
        private System.Windows.Forms.TextBox Text_Empfangen;
        private System.Windows.Forms.CheckBox Bidirektional;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox textBox1;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class Choose_Export_Flat : Form
    {
        List<string> m_Stereotype;
        bool check;
        bool auto_check = false;

        public Choose_Export_Flat(List<string> m_Stereotype)
        {
            this.m_Stereotype = m_Stereotype;
            InitializeComponent();

          
        }

        private void Choose_Export_Flat_Load(object sender, EventArgs e)
        {
            check = false;
            /////////////////////////////7
            ///Check List aufbauen
            if (this.m_Stereotype != null)
            {

                this.tree_Stereotype.CheckBoxes = true;

                int i1 = 0;
                do
                {
                    if(this.m_Stereotype[i1] != "")
                    {
                        TreeNode Child = new TreeNode(this.m_Stereotype[i1]) { Tag = this.m_Stereotype[i1] };
                        Child.Name = Child.Text;
                        this.tree_Stereotype.Nodes.Add(Child);
                    }
                    i1++;
                } while (i1 < this.m_Stereotype.Count);

                this.tree_Stereotype.Sort();
                this.tree_Stereotype.Update();

                this.m_Stereotype.Clear();
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            //Schleife über alle Nodes
            if(this.tree_Stereotype.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.tree_Stereotype.Nodes[i1].Checked == true)
                    {
                        if(this.tree_Stereotype.Nodes[i1].Text != "none" && this.tree_Stereotype.Nodes[i1].Text != "")
                        {
                            this.m_Stereotype.Add(this.tree_Stereotype.Nodes[i1].Text);
                        }
                    }

                    i1++;
                } while (i1 < this.tree_Stereotype.Nodes.Count);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button_Check_Click(object sender, EventArgs e)
        {
            this.auto_check = true;

            if(tree_Stereotype.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.check == false)
                    {
                        tree_Stereotype.Nodes[i1].Checked = true;
                    }
                    else
                    {
                        tree_Stereotype.Nodes[i1].Checked = false;
                    }

                    i1++;
                } while (i1 < tree_Stereotype.Nodes.Count);
            }

            if(check == false)
            {
                check = true;
                button_Check.Text = "Uncheck All";
                
            }
            else
            {
                check = false;
                button_Check.Text = "Check All";
            }

            this.Refresh();
            this.auto_check = false;
        }

        private void tree_Stereotype_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if(this.auto_check == false)
            {
                if (e.Node.Checked == true)
                {
                    check = false;
                    button_Check.Text = "Check All";
                }
                else
                {
                    check = true;
                    button_Check.Text = "Uncheck All";
                }

                button_Check.Refresh();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metamodels;

namespace Forms
{
    public partial class Choose_Analyse_Patterns : Form
    {
        List<string> m_Pattern = new List<string>();
        Metamodel metamodel;
        bool auto_check = false;
        bool check = false;

        public Choose_Analyse_Patterns(Metamodel metamodel)
        {
            this.m_Pattern = metamodel.m_Pattern;
            this.metamodel = metamodel;

            InitializeComponent();

            #region Split Container
           //this.splitContainer1.IsSplitterFixed = true;
           // this.splitContainer2.IsSplitterFixed = true;
           // this.splitContainer3.IsSplitterFixed = true;
           // this.splitContainer4.IsSplitterFixed = true;
           // this.splitContainer5.IsSplitterFixed = true;
            #endregion SplitContainer
        }

        private void Choose_Analyse_Patterns_Load(object sender, EventArgs e)
        {
            this.tree_Pattern.CheckBoxes = true;

            if(this.m_Pattern.Count > 0)
            {
                int i1 = 0;
                do
                {
                    TreeNode Child = new TreeNode(this.m_Pattern[i1]) { Tag = this.m_Pattern[i1] };
                    Child.Name = Child.Text;
                    this.tree_Pattern.Nodes.Add(Child);

                    i1++;
                } while (i1 < this.m_Pattern.Count);

                this.tree_Pattern.Sort();
                this.tree_Pattern.Update();
                //this.m_Pattern.Clear();
             }

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            //Schleife über alle Nodes
            if (this.tree_Pattern.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.tree_Pattern.Nodes[i1].Checked == true)
                    {
                        if (this.tree_Pattern.Nodes[i1].Text != "none" && this.tree_Pattern.Nodes[i1].Text != "")
                        {
                            string recent = this.tree_Pattern.Nodes[i1].Tag as string;

                            int index = this.m_Pattern.FindIndex(x => x == recent);

                            if(index != -1)
                            {
                                this.metamodel.m_Pattern_flag[index] = true;
                            }
                            else
                            {
                                this.metamodel.m_Pattern_flag[index] = false;
                            }
                        }
                    }

                    i1++;
                } while (i1 < this.tree_Pattern.Nodes.Count);
            }

            if(this.checkBox_SysArch.Checked == true)
            {
                this.metamodel.flag_sysarch = true;
            }
            else
            {
                this.metamodel.flag_sysarch = false;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button_Check_Click(object sender, EventArgs e)
        {
            this.auto_check = true;

            if (tree_Pattern.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.check == false)
                    {
                        tree_Pattern.Nodes[i1].Checked = true;
                    }
                    else
                    {
                        tree_Pattern.Nodes[i1].Checked = false;
                    }

                    i1++;
                } while (i1 < tree_Pattern.Nodes.Count);
            }

            if (check == false)
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

        private void tree_Pattern_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (this.auto_check == false)
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

        private void tree_Pattern_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ennumerationen;
using Metamodels;

namespace Forms
{
    public partial class Choose_Metamodel_Form : Form
    {
        private bool tree_checked = false;
        private int index_checked = -1;
        Metamodel metamodel;
       

        public Choose_Metamodel_Form(Metamodel metamodel)
        {
            this.metamodel = metamodel;

            InitializeComponent();

            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add("Accdb");
            this.comboBox1.Items.Add("MSDASQL");
        }

        private void tree_Metamodel_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if(tree_checked == false && e.Node.ForeColor != Color.Gray)
            {
                tree_checked = true;
                button_OK.Enabled = true;

                //Der geänderte Index
                TreeNode Changed_Node = e.Node;
                index_checked = Changed_Node.Index;

                //Alle Deaktivieren bis auf den aktuellen
                int i1 = 0;
                do
                {
                    if(tree_Metamodel.Nodes[i1].Index != index_checked)
                    {
                        tree_Metamodel.Nodes[i1].ForeColor = Color.Gray;
                    }

                    i1++;
                } while (i1 < tree_Metamodel.Nodes.Count);

            }
            else
            {
                if(e.Node.ForeColor == Color.Gray)
                {
                    e.Node.Checked = false;
                }
                else
                {
                    tree_checked = false;
                    index_checked = -1;
                    button_OK.Enabled = false;

                    int i1 = 0;
                    do
                    {

                        tree_Metamodel.Nodes[i1].ForeColor = Color.Black;

                        i1++;
                    } while (i1 < tree_Metamodel.Nodes.Count);
                }
               
            }

            tree_Metamodel.Enabled = true;

        }

        private void tree_Metamodel_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            tree_Metamodel.Enabled = false;

            if (Color.Gray == e.Node.ForeColor)
            {
             //   tree_Metamodel.SelectedNode = tree_Metamodel.Nodes[index_checked];
                e.Cancel = true;                  
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if(button_OK.Enabled == true)
            { 

                this.metamodel.Metamodel_name = tree_Metamodel.Nodes[index_checked].Text;

                this.DialogResult = DialogResult.OK;

                this.Close();
            }
        }

        private void Choose_Metamodel_Form_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                metamodel.flag_Analyse_Diagram = true;
            }
            else
            {
                metamodel.flag_Analyse_Diagram = false;
            }
        }

        private void button_Load_Click(object sender, EventArgs e)
        {
            XML_Handler_Profil import = new XML_Handler_Profil(this.metamodel);
            bool ret = import.Import_Profil();

            if(ret == true)
            {
                this.metamodel.Metamodel_name = "Custom";
                this.DialogResult = DialogResult.OK;

                if (checkBox1.Checked == true)
                {
                    this.metamodel.flag_Analyse_Diagram = true;
                }
                else
                {
                    this.metamodel.flag_Analyse_Diagram = false;
                }
                if (checkBox2.Checked == true)
                {
                    this.metamodel.flag_slow_mode = true;
                }
                else
                {
                    this.metamodel.flag_slow_mode = false;
                }

                this.Close();
            }
           
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                metamodel.flag_slow_mode = true;
            }
            else
            {
                metamodel.flag_slow_mode = false;
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(this.comboBox1.SelectedItem.ToString())
            {
                case "Accdb":
                    metamodel.dB_Type = DB_Type.ACCDB;
                    break;
                case "MSDASQL":
                    metamodel.dB_Type = DB_Type.MSDASQL;
                    break;
                default:
                    metamodel.dB_Type = DB_Type.ACCDB;
                    break;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                metamodel.flag_bpmn = true;
            }
            else
            {
                metamodel.flag_bpmn = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                metamodel.flag_issue_aenderungen_export = true;
            }
            else
            {
                metamodel.flag_issue_aenderungen_export = false;
            }
        }
    }
}

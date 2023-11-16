using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Requirement_Plugin.Forms.AFO_Erstellung
{
    public partial class Abfrage_Annahme_Aenderung : Form
    {
        public List<bool> m_ret = new List<bool>();
        bool flag_check;

        public Abfrage_Annahme_Aenderung(List<bool> m_change)
        {
            this.m_ret.Clear();
            flag_check = true;

            InitializeComponent();

            if (this.treeView1.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_ret.Add(false);

                    if(m_change[i1] == false)
                    {
                        this.treeView1.Nodes[i1].Checked = false;
                        this.treeView1.Nodes[i1].ForeColor = Color.Gray;
                    }

                    i1++;
                } while (i1 < this.treeView1.Nodes.Count);
            }

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if(this.treeView1.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.treeView1.Nodes[i1].Checked == true && this.treeView1.Nodes[i1].ForeColor != Color.Gray)
                    {
                        this.m_ret[i1] = true;
                    }
                    else
                    {
                        this.m_ret[i1] = false;
                    }

                    i1++;
                } while (i1 < this.treeView1.Nodes.Count);

            }

            this.DialogResult = DialogResult.OK;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            if (this.treeView1.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_ret[i1] = false;

                    i1++;
                } while (i1 < this.treeView1.Nodes.Count);

            }

            this.DialogResult = DialogResult.Cancel;
        }

        private void Abfrage_Annahme_Aenderung_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (Color.Gray == e.Node.ForeColor)
            {
                this.treeView1.SelectedNode = e.Node.Parent;

                e.Cancel = true;
            }
        }

        private void treeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (Color.Gray == e.Node.ForeColor)
            {
                this.treeView1.SelectedNode = e.Node.Parent;
                flag_check = false;
                e.Cancel = true;


            }
            else
            {
                flag_check = true;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node.ForeColor == Color.Gray && flag_check == false)
            {
                e.Node.Checked = false;
                flag_check = true;
            }
        }
    }
}

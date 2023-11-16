using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Requirement_Plugin;
using Repsoitory_Elements;

namespace Requirement_Plugin.Forms.Analyse
{
    public partial class Auswahl_Systemelemente : Form
    {
        List<SysElement> m_Logical = new List<SysElement>();
        public List<SysElement> m_ret = new List<SysElement>();
        bool check_bottom = false; //False keiner ist angewählt
        bool flag_bottom_check = false;

        string Text_hinweis_Logical = "Es wurde mind. ein Systemelement ausgewählt, welches einen nicht ausgewählten Systemelement spezifiziert. Die betroffenen, nicht ausgewählten Systemelemente wurden gelb markiert. Diese gelb markieten Systemelemente werden nicht analysiert.";


        #region Initialisierung
        public Auswahl_Systemelemente(List<SysElement> m_logical)
        {
            this.m_Logical = m_logical;
            this.check_bottom = false;

            InitializeComponent();
        }
        private void Auswahl_Systemelemente_Load(object sender, EventArgs e)
        {
            this.Build_Anwendunsfall_Tree();
        }

        #endregion

        #region Treeview
        private void Build_Anwendunsfall_Tree()
        {
            //Tree löschen
            this.treeView_Anwendungsfall.Nodes.Clear();
            this.treeView_Anwendungsfall.CheckBoxes = true;

            List<SysElement> m_help = this.m_Logical.Where(x => x.m_Parent.Count == 0).ToList();

            //Baum befüllen mit Anwendungsfällen
            if (m_help.Count > 0)
            {
                int i1 = 0;
                do
                {
                    TreeNode recent_treenode = new TreeNode(m_help[i1].Name) { Tag = m_help[i1] };
                    recent_treenode.Checked = false;

                    this.treeView_Anwendungsfall.Nodes.Add(recent_treenode);

                    Build_tree_rekursiv(recent_treenode, m_help[i1]);

                    i1++;
                } while (i1 < m_help.Count);
            }

            //Nach Alphabet ordnen
            this.treeView_Anwendungsfall.Sort();
            this.treeView_Anwendungsfall.Update();

        }

        private void Build_tree_rekursiv(TreeNode node, SysElement sys)
        {
            if(sys.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    TreeNode recent_treenode = new TreeNode(sys.m_Child[i1].Name) { Tag = sys.m_Child[i1] };
                    recent_treenode.Checked = false;

                    node.Nodes.Add(recent_treenode);

                    Build_tree_rekursiv(recent_treenode, sys.m_Child[i1]);

                    i1++;
                } while (i1 < sys.m_Child.Count);
            }
        }

        private void Set_Status_TreeView(bool status)
        {
            if (this.treeView_Anwendungsfall.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.treeView_Anwendungsfall.Nodes[i1].Checked = status;
                    this.treeView_Anwendungsfall.Nodes[i1].BackColor = Color.White;

                    Set_Status_rekursiv(this.treeView_Anwendungsfall.Nodes[i1], status);

                    i1++;
                } while (i1 < this.treeView_Anwendungsfall.Nodes.Count);

                this.treeView_Anwendungsfall.Update();
            }
        }

        private void Set_Status_rekursiv(TreeNode node, bool status)
        {
            if(node.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    node.Nodes[i1].Checked = status;
                    node.Nodes[i1].BackColor = Color.White;

                    Set_Status_rekursiv(node.Nodes[i1], status);

                    i1++;
                } while (i1 < node.Nodes.Count);
            }
        }
        #endregion

        #region Bottom
        private void button_Check_Click(object sender, EventArgs e)
        {
            flag_bottom_check = true;

            if (this.check_bottom == true)
            {
                this.button_Check.Text = "Check All";
                this.button_Check.Update();

                Set_Status_TreeView(false);

                m_ret = new List<SysElement>();

                this.richTextBox_Hinweis.Text = "";
                this.richTextBox_Hinweis.Update();

                this.check_bottom = false;
            }
            else
            {
                this.button_Check.Text = "Uncheck All";
                this.button_Check.Update();

                Set_Status_TreeView(true);

                m_ret = this.m_Logical;

                this.richTextBox_Hinweis.Text = "";
                this.richTextBox_Hinweis.Update();

                this.check_bottom = true;
            }

            flag_bottom_check = false;
        }
        #endregion

        #region After Select
        private void treeView_Systemelemente_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Wird aktuell nicht genutzt
        }
        #endregion

      
        #region After Click
        private void treeView_Systemelemente_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var flag_Hinweis_Test_Logical = false;

            if (flag_bottom_check == false)
            {
                //Kinderelemente setzene
                if (e.Node.Checked == true)
                {
                    Set_Status_rekursiv(e.Node, true);
                }
                else
                {
                    Set_Status_rekursiv(e.Node, false);
                }
                this.treeView_Anwendungsfall.Update();

                List<SysElement> m_checked = new List<SysElement>();
                List<SysElement> m_To_Check = new List<SysElement>();

                if (this.treeView_Anwendungsfall.Nodes.Count > 0)
                {
                    //Checked Elements finden
                    int i1 = 0;
                    do
                    {
                        if (this.treeView_Anwendungsfall.Nodes[i1].Checked == true)
                        {
                            if (m_checked.Contains(this.treeView_Anwendungsfall.Nodes[i1].Tag as SysElement) == false)
                            {
                                m_checked.Add(this.treeView_Anwendungsfall.Nodes[i1].Tag as SysElement);
                            }
                        }

                        //Rekursiv erhalten
                        List<SysElement> m_help =  Get_rekrusiv(this.treeView_Anwendungsfall.Nodes[i1]);

                        if (m_help.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_checked.Contains(m_help[i2]) == false)
                                {
                                    m_checked.Add(m_help[i2]);
                                }

                                i2++;
                            } while (i2 < m_help.Count);
                        }

                        i1++;
                    } while (i1 < this.treeView_Anwendungsfall.Nodes.Count);


                    this.m_ret = m_checked;
                    //Zu checkende Elements finden
                    if (m_checked.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (m_To_Check.Contains(m_checked[i2]) == false)
                            {
                                m_To_Check.Add(m_checked[i2]);
                            }
                            List<SysElement> m_Speciefied = m_checked[i2].Get_All_SpecifiedBy(this.m_Logical);

                            if (m_Speciefied != null)
                            {
                                int i3 = 0;
                                do
                                {
                                    if (m_checked.Contains(m_Speciefied[i3]) == false)
                                    {
                                        m_To_Check.Add(m_Speciefied[i3]);
                                    }

                                    i3++;
                                } while (i3 < m_Speciefied.Count);
                            }

                            i2++;
                        } while (i2 < m_checked.Count);


                    }


                    //Elemente einfärben
                    int i4 = 0;
                    do
                    {
                        if (m_To_Check.Contains(this.treeView_Anwendungsfall.Nodes[i4].Tag as SysElement) == true)
                        {
                            if (this.treeView_Anwendungsfall.Nodes[i4].Checked == false)
                            {
                                this.treeView_Anwendungsfall.Nodes[i4].BackColor = Color.Yellow;
                                flag_Hinweis_Test_Logical = true;
                            }
                            else
                            {
                                this.treeView_Anwendungsfall.Nodes[i4].BackColor = Color.White;
                            }
                        }
                        else
                        {
                            this.treeView_Anwendungsfall.Nodes[i4].BackColor = Color.White;
                        }

                        bool help = Einfaerben_rekursiv(this.treeView_Anwendungsfall.Nodes[i4], m_To_Check);

                        if (help == true)
                        {
                            flag_Hinweis_Test_Logical = true;
                            this.treeView_Anwendungsfall.Nodes[i4].Expand();
                        }

                        i4++;
                    } while (i4 < this.treeView_Anwendungsfall.Nodes.Count);


                    this.treeView_Anwendungsfall.SelectedNode = null;
                    this.treeView_Anwendungsfall.Update();

                }
                else
                {
                    m_ret = null;
                }


                if (flag_Hinweis_Test_Logical == true)
                {
                    this.richTextBox_Hinweis.Text = this.Text_hinweis_Logical;
                }
                else
                {
                    this.richTextBox_Hinweis.Text = "";
                }

                this.richTextBox_Hinweis.Update();
            }
            else
            {
                if(e.Node.Checked == true)
                {
                    Set_Status_rekursiv(e.Node, true);
                }
                else
                {
                    Set_Status_rekursiv(e.Node, false);
                }

               

                this.treeView_Anwendungsfall.Update();
            }
        }

        private bool Einfaerben_rekursiv(TreeNode node, List<SysElement> m_To_Check)
        {
            bool flag_Hinweis_Test_Logical = false;

            if (node.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_To_Check.Contains(node.Nodes[i1].Tag as SysElement) == true)
                    {
                        if (node.Nodes[i1].Checked == false)
                        {
                            node.Nodes[i1].BackColor = Color.Yellow;
                            flag_Hinweis_Test_Logical = true;
                            node.Nodes[i1].Expand();
                            node.Expand();
                        }
                        else
                        {
                            node.Nodes[i1].BackColor = Color.White;
                        }
                    }
                    else
                    {
                        node.Nodes[i1].BackColor = Color.White;
                    }

                    bool help = Einfaerben_rekursiv(node.Nodes[i1], m_To_Check);

                    if(help == true)
                    {
                        flag_Hinweis_Test_Logical = true;
                        node.Expand();
                    }

                    i1++;
                } while (i1 < node.Nodes.Count);
            }

            return (flag_Hinweis_Test_Logical);
        }


        private List<SysElement> Get_rekrusiv(TreeNode node)
        {
            List<SysElement> m_ret2 = new List<SysElement>();

            if (node.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (node.Nodes[i1].Checked == true)
                    {
                        if (m_ret2.Contains(node.Nodes[i1].Tag as SysElement) == false)
                        {
                            m_ret2.Add(node.Nodes[i1].Tag as SysElement);
                        }
                    }

                    List<SysElement> m_help = Get_rekrusiv(node.Nodes[i1]);

                    if(m_help.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if(m_ret2.Contains(m_help[i2]) == false)
                            {
                                m_ret2.Add(m_help[i2]);
                            }

                            i2++;
                        } while (i2 < m_help.Count);
                    }

                    i1++;
                } while (i1 < node.Nodes.Count);
            }

            return (m_ret2);
        }
        #endregion

        #region DialogResults

        private void buttonOK_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button_cancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.m_ret = new List<SysElement>();
        }


        #endregion


    }
}

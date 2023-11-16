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

namespace Requirement_Plugin.Forms
{
    public partial class Auswahl_Anwendungsfall : Form
    {
        List<Logical> m_Logical = new List<Logical>();
        public List<Logical> m_ret = new List<Logical>();
        bool check_bottom = false; //False keiner ist angewählt
        bool flag_bottom_check = false;

        string Text_hinweis_Logical = "Es wurde mind. ein Anwendungsfall ausgewählt, welcher einen nicht ausgewählten Anwendungsfall spezifiziert. Die betroffenen, nicht ausgewählten Anwendungsfälle wurden gelb markiert. Diese gelb markieten Anwendungsfälle werden nicht analysiert.";


        #region Initialisierung
        public Auswahl_Anwendungsfall(List<Logical> m_logical)
        {
            this.m_Logical = m_logical;
            this.check_bottom = false;

            InitializeComponent();
        }

        private void Auswahl_Anwendungsfall_Load(object sender, EventArgs e)
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

            //Baum befüllen mit Anwendungsfällen
            if(this.m_Logical.Count > 0)
            {
                int i1 = 0;
                do
                {
                    TreeNode recent_treenode = new TreeNode(this.m_Logical[i1].Name) {Tag = this.m_Logical[i1] };
                    recent_treenode.Checked = false;
                    
                    this.treeView_Anwendungsfall.Nodes.Add(recent_treenode);


                    i1++;
                } while (i1 < this.m_Logical.Count);
            }

            //Nach Alphabet ordnen
            this.treeView_Anwendungsfall.Sort();
            this.treeView_Anwendungsfall.Update();

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

                    i1++;
                } while (i1 < this.treeView_Anwendungsfall.Nodes.Count);

                this.treeView_Anwendungsfall.Update();
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

                m_ret = new List<Logical>();

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
        private void treeView_Anwendungsfall_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Wird aktuell nicht genutzt
        }
        #endregion

        #region After Click
        private void treeView_Anwendungsfall_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var flag_Hinweis_Test_Logical = false;

            if(flag_bottom_check == false)
            {
                List<Logical> m_checked = new List<Logical>();
                List<Logical> m_To_Check = new List<Logical>();

                if (this.treeView_Anwendungsfall.Nodes.Count > 0)
                {
                    //Checked Elements finden
                    int i1 = 0;
                    do
                    {
                        if (this.treeView_Anwendungsfall.Nodes[i1].Checked == true)
                        {
                            if (m_checked.Contains(this.treeView_Anwendungsfall.Nodes[i1].Tag as Logical) == false)
                            {
                                m_checked.Add(this.treeView_Anwendungsfall.Nodes[i1].Tag as Logical);
                            }
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
                            List<Logical> m_Speciefied = m_checked[i2].Get_All_SpecifiedBy(this.m_Logical);

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
                            if (m_To_Check.Contains(this.treeView_Anwendungsfall.Nodes[i4].Tag as Logical) == true)
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

                            i4++;
                        } while (i4 < this.treeView_Anwendungsfall.Nodes.Count);


                    this.treeView_Anwendungsfall.SelectedNode = null;
                        this.treeView_Anwendungsfall.Update();
                    
                }
                else
                {
                    m_ret = null;
                }


                if(flag_Hinweis_Test_Logical == true)
                {
                    this.richTextBox_Hinweis.Text = this.Text_hinweis_Logical;
                }
                else
                {
                    this.richTextBox_Hinweis.Text = "";
                }

                this.richTextBox_Hinweis.Update();
            }
        }

        #endregion

        #region DialogResults
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;


        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.m_ret = new List<Logical>();
        }
        #endregion
    }
}

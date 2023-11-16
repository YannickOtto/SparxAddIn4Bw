using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Requirement_Plugin.Forms
{
    public partial class Choose_Import_Elements_To_Delete : Form
    {
        List<List<string>> m_No_Import_Connectoren = new List<List<string>>();
        Database database;
        EA.Repository repository;

        bool flag_bearbeitung = false;


        public Choose_Import_Elements_To_Delete(Database Data, List<List<string>> m_No_Import_Con, EA.Repository repository)
        {
            InitializeComponent();
            this.m_No_Import_Connectoren = m_No_Import_Con;
            this.database = Data;
            this.repository = repository;

            #region Tooltipps

            ToolTip toolTip_search = new ToolTip();
            toolTip_search.SetToolTip(richTextBox_Search, "Geben Sie eine Zeichenfolge ein, welche in den Bäumen gesucht wird. Zum Start der Suche bitte Enter drücken.");

            #endregion
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void Choose_Import_Elements_To_Delete_Load(object sender, EventArgs e)
        {

            //Tree aufbauen
            TreeNode Elements = new TreeNode("Elemente") { Tag = null };
            Elements.Name = Elements.Text;
            this.treeView_Elements.Nodes.Add(Elements);

            if (this.m_No_Import_Connectoren.Count > 0)
            {
                int i1 = 0;
                do
                {
                    switch (i1)
                    {
                        case 0:
                            if(this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode AFO_Funk = new TreeNode("Anforderung --> Funktionsbaum");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Derived_Capability[0].Stereotype , AFO_Funk, this.repository);

                                AFO_Funk.Name = "AFO_Funktionsbaum";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(AFO_Funk);

                            }
                            break;
                        case 1:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode AFO_Sys = new TreeNode("Anforderung --> Systemelement");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Derived_Element[0].Stereotype, AFO_Sys, this.repository);

                                AFO_Sys.Name = "AFO_Systemelement";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(AFO_Sys);
                            }
                            break;
                        case 2:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode AFO_Szenar = new TreeNode("Anforderung --> Szenar");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Derived_Logical[0].Stereotype, AFO_Szenar, this.repository);

                                AFO_Szenar.Name = "AFO_Szenar";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(AFO_Szenar);
                            }
                            break;
                        case 3:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode AFO_Refines = new TreeNode("Anforderung: Refines");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Afo_Refines[0].Stereotype, AFO_Refines, this.repository);

                                AFO_Refines.Name = "AFO_Refines";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(AFO_Refines);
                            }
                            break;
                        case 4:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode AFO_Requires = new TreeNode("Anforderung: Requires");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Afo_Requires[0].Stereotype, AFO_Requires, this.repository);

                                AFO_Requires.Name = "AFO_Requires";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(AFO_Requires);
                            }
                            break;
                        case 5:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode AFO_Replaces = new TreeNode("Anforderung: Replaces");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Afo_Replaces[0].Stereotype, AFO_Replaces, this.repository);

                                AFO_Replaces.Name = "AFO_Replaces";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(AFO_Replaces);
                            }
                            break;
                        case 6:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode AFO_Conflicts = new TreeNode("Anforderung: Conflicts");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Afo_Konflikt[0].Stereotype, AFO_Conflicts, this.repository);

                                AFO_Conflicts.Name = "AFO_Conflicts";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(AFO_Conflicts);
                            }
                            break;
                        case 7:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode AFO_Dublette = new TreeNode("Anforderung: Dublette");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Afo_Dublette[0].Stereotype, AFO_Dublette, this.repository);

                                AFO_Dublette.Name = "AFO_Dublete";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(AFO_Dublette);
                            }
                            break;
                        case 8:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode Funktionsbaum = new TreeNode("Funktionsbaum: Dekomposition");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Taxonomy_Capability[0].Stereotype, Funktionsbaum, this.repository);

                                Funktionsbaum.Name = "Funktionsbaum";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(Funktionsbaum);
                            }
                            break;
                        case 9:
                            if (this.m_No_Import_Connectoren[i1].Count > 0)
                            {
                                TreeNode SysElem = new TreeNode("Systemelement: Dekomposition");// { Tag = this.m_No_Import_Connectoren[i1] };

                                Build_SubTree(this.m_No_Import_Connectoren[i1], this.database.metamodel.m_Decomposition_Element[0].Stereotype, SysElem, this.repository);

                                SysElem.Name = "SysElem";
                                this.treeView_Connectoren.Nodes[0].Nodes.Add(SysElem);
                            }
                            break;
                    }

                    i1++;
                } while (i1 < this.m_No_Import_Connectoren.Count);

            }
        }

        private void Build_SubTree(List<string>m_guid_Con, string Stereotype, TreeNode parent, EA.Repository repository)
        {
            if(m_guid_Con.Count > 0)
            {
                EA.Element client;
                EA.Element supplier;

                int i1 = 0;
                do
                {
                    client = repository.GetElementByID(repository.GetConnectorByGuid(m_guid_Con[i1]).ClientID);
                    supplier = repository.GetElementByID(repository.GetConnectorByGuid(m_guid_Con[i1]).SupplierID);

                    TreeNode child = new TreeNode("<<"+ client.Stereotype+ ">> "+client.Name+ " --> << "+ supplier.Stereotype+ " >> " + supplier.Name) {Tag = m_guid_Con [i1]};
                    

                    parent.Nodes.Add(child);


                    i1++;
                } while (i1 < m_guid_Con.Count);

                
            }
        }

        private void treeView_Connectoren_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void Set_Check_Child(TreeNode treeNode, bool checked_status)
        {
            if(treeNode.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    treeNode.Nodes[i1].Checked = checked_status;
                    Set_Check_Child(treeNode.Nodes[i1], checked_status);

                    i1++;
                } while (i1 < treeNode.Nodes.Count);

            }
        }

        private void treeView_Connectoren_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if(this.flag_bearbeitung == true && this.treeView_Connectoren.Enabled == true)
            {
                this.treeView_Connectoren.Enabled = false;

                if (e.Node.Checked == true)
                {
                   
                    //e.Node.Checked = false;
                    Set_Check_Child(e.Node, true);
                    
                }
                else
                {

                    
                   // e.Node.Checked = true;
                    Set_Check_Child(e.Node, false);
                   


                }
                this.flag_bearbeitung = false;
                this.treeView_Connectoren.Enabled = true;
                this.treeView_Connectoren.Update();
            }
           
        }

        private void treeView_Connectoren_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
           

            if(this.flag_bearbeitung == false)
            {
               

                this.flag_bearbeitung = true;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            List<string> m_Delete_Con = new List<string>();

            //Feststellen löschende Konnekoren
            if (this.treeView_Connectoren.Nodes.Count > 0)
            {
                m_Delete_Con = Get_Tags(this.treeView_Connectoren.Nodes[0]);
            }
            //Konnekoren löschen
            if(m_Delete_Con.Count > 0)
            {
                Interfaces.Interface_Connectors interface_Connectors = new Interfaces.Interface_Connectors();
                interface_Connectors.Delete_Connectors(this.database, m_Delete_Con, this.repository);

                //Erzeugte Issues in Archiv verschieben und Status ändern


            }
            //Fenster schließen
            this.Close();
            repository.RefreshModelView(0);

        }

        private List<string> Get_Tags(TreeNode node)
        {
            List<string> m_ret = new List<string>();

            if(node.Tag != null && node.Checked == true)
            {
                //List<string> m_help = (List<string>)node.Tag;
                string m_help = (string) node.Tag;

                if (m_help.Count() > 0)
                {
                    m_ret.Add(m_help);

                    m_ret = m_ret.Distinct().ToList();
                }
            }
            if(node.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    //List<string> m_help2 = Get_Tags(node.Nodes[i1]);
                    List<string> m_help2 = Get_Tags(node.Nodes[i1]);

                    if (m_help2.Count() > 0)
                    {
                        m_ret.AddRange(m_help2);

                        m_ret = m_ret.Distinct().ToList();
                    }

                    i1++;
                }while( i1 < node.Nodes.Count);
            }


            return (m_ret);
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(richTextBox_Search.Text.Trim() != "")
                {
                    //Elemente durchsuchen
                    if(treeView_Elements.Nodes.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            Search_rekrusiv(treeView_Elements.Nodes[i1], richTextBox_Search.Text.Trim());

                            i1++;
                        } while (i1 < treeView_Elements.Nodes.Count);

                        treeView_Elements.Update();
                    }

                    //Connectoren durchsuchen
                    if (treeView_Connectoren.Nodes.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            Search_rekrusiv(treeView_Connectoren.Nodes[i1], richTextBox_Search.Text.Trim());

                            i1++;
                        } while (i1 < treeView_Connectoren.Nodes.Count);

                        treeView_Connectoren.Update();
                    }

                }
            }
        }


        private bool Search_rekrusiv(TreeNode treeNode, string search)
        {
            bool expand = false;

            if(treeNode.Text.ToUpper().Contains(search.ToUpper()))
            {
                treeNode.Expand();
                treeNode.BackColor = Color.Yellow;

                expand = true;
            }
            else
            {
                treeNode.BackColor = Color.Empty;
                treeNode.Collapse();
            }

            if(treeNode.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    bool help =  Search_rekrusiv(treeNode.Nodes[i1], search);

                    if(help == true)
                    {
                        expand = true;
                    }

                    i1++;
                } while (i1 < treeNode.Nodes.Count);
            }

            if(expand == true)
            {
                treeNode.Expand();
            }

            return (expand);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Globalization;
using System.Data.OleDb;
using System.Data.Odbc;

using Database_Connection;
using Repsoitory_Elements;
using Elements;
using Requirements;

namespace Forms
{
    public partial class Interface_Decomposition : Form
    {

        Requirement_Plugin.Database Database;
        EA.Repository Repository;
        string Package_InfoÜbertragung_ID;
        ////////////////
        ///Artikel Client und Supplier
       // public List<string> Artikel = new List<string>();
        public int NodeType_Artikel_Index = 0;
        public int Target_Artikel_Index = 0;
        ////////////////
        ///Text des Senders und des Empfängers
        public List<string> recent_Text_Senden = new List<string>();
        public List<string> recent_Text_Empfangen = new List<string>();
        ////////////////
        ///Merker für aktuellen Client Node und Supplier Node
        private TreeNode recentTreeNode_Client;
        private TreeNode recentTreeNode_Supplier;
        ///////////////
        //aktuelle Prozesswörter
        // string[] m_Prozesswort = new string[2];
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        Repository_Element repository_Element = new Repository_Element();
        /// <summary>
        /// Initialisierung Interface_Decomposition
        /// </summary>
        /// <param name="Database"></param>
        /// <param name="Repository"></param>
        public Interface_Decomposition(Requirement_Plugin.Database Database, EA.Repository Repository)
        {
            this.Database = Database;
            this.Repository = Repository;
            this.recentTreeNode_Client = null;
            this.recentTreeNode_Supplier = null;
            ////////////////////
            //Text_Senden initialisieren
            recent_Text_Senden.Add("..."); //Artikel
            recent_Text_Senden.Add(" "); //Leer
            recent_Text_Senden.Add("..."); //NodeType
            recent_Text_Senden.Add(" muss fähig sein, " + Database.metamodel.string_Interface[0] + " ("); //Füll
            recent_Text_Senden.Add("..."); //Information Element
            recent_Text_Senden.Add(") an "); //Füll2
            recent_Text_Senden.Add("..."); //Artikel
            recent_Text_Senden.Add(" "); //Leer
            recent_Text_Senden.Add("..."); //Target
            recent_Text_Senden.Add(" zu " + this.Database.metamodel.m_Prozesswort_Interface[0] + "."); //Füll3
            //////////////////////
            //Text empfangen initialisieren
            recent_Text_Empfangen.Add("..."); //Artikel
            recent_Text_Empfangen.Add(" "); //Leer
            recent_Text_Empfangen.Add("..."); //Target
            recent_Text_Empfangen.Add(" muss fähig sein, " + Database.metamodel.string_Interface[0] + " ("); //Füll
            recent_Text_Empfangen.Add("..."); //Information Element
            recent_Text_Empfangen.Add(") von "); //Füll2
            recent_Text_Empfangen.Add("..."); //Artikel
            recent_Text_Empfangen.Add(" "); //Leer
            recent_Text_Empfangen.Add("..."); //Nodetype
            recent_Text_Empfangen.Add(" zu " + this.Database.metamodel.m_Prozesswort_Interface[1] + "."); //Füll3

            this.Package_InfoÜbertragung_ID = repository_Element.Create_Package_Model(Database.metamodel.m_Package_Name[1], Repository, this.Database);


            InitializeComponent();

            Client_Tree.Sort();
            Supplier_Tree.Sort();
            InfoElemBox_Tree.Sort();
        }
        /// <summary>
        /// Beim Initialisieren wird dies ausgeführt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Interface_Decomposition_Load(object sender, EventArgs e)
        {
            if (Database.Decomposition != null)
            {
                TreeNode Ebene0 = new TreeNode("Client") { Tag = Database.Decomposition.Classifier_ID };
                Ebene0.Name = Ebene0.Text;
                TreeNode Ebene0_Supplier = new TreeNode("Supplier") { Tag = Database.Decomposition.Classifier_ID };
                Ebene0_Supplier.Name = Ebene0_Supplier.Text;
                Ebene0_Supplier.ForeColor = Color.Gray;

                Client_Tree.Nodes.Add(Ebene0);
                Supplier_Tree.Nodes.Add(Ebene0_Supplier);

                recentTreeNode_Client = Ebene0;

                if (Database.metamodel.flag_sysarch == false)
                {
                    if (this.Database.Decomposition.m_NodeType.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            bool flag_Grey = false;

                            TreeNode Child = new TreeNode(Database.Decomposition.m_NodeType[i1].Get_Name(this.Database)) { Tag = Database.Decomposition.m_NodeType[i1] };
                            Child.Name = Child.Text;
                            Ebene0.Nodes.Add(Child);

                            if (checkBox_Bidirektional.Checked == false)
                            {
                                if (Database.Decomposition.m_NodeType[i1].m_Element_Interface.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                            }
                            else
                            {
                                if (Database.Decomposition.m_NodeType[i1].m_Element_Interface_Bidirectional.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                            }

                            if (flag_Grey == true)
                            {
                                Child.ForeColor = Color.Gray;
                            }


                            TreeNode Child_Supplier = new TreeNode(Database.Decomposition.m_NodeType[i1].Get_Name(this.Database)) { Tag = Database.Decomposition.m_NodeType[i1] };
                            Child.Name = Child.Text;
                            Child_Supplier.ForeColor = Color.Gray;
                            Ebene0_Supplier.Nodes.Add(Child_Supplier);


                            Show_Treeview_NodeType_rekursiv(Database.Decomposition.m_NodeType[i1], Child, true);
                            Show_Treeview_NodeType_rekursiv(Database.Decomposition.m_NodeType[i1], Child_Supplier, false);

                            i1++;
                        } while (i1 < this.Database.Decomposition.m_NodeType.Count);
                    }
                }
                else
                {
                    List<SysElement> m_dec = new List<SysElement>();
                    m_dec = Database.m_SysElemente.Where(x => x.m_Parent.Count == 0).ToList();

                    if (m_dec.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            bool flag_Grey = false;

                            TreeNode Child = new TreeNode(m_dec[i1].Get_Name(this.Database)) { Tag = m_dec[i1] };
                            Child.Name = Child.Text;
                            Ebene0.Nodes.Add(Child);

                            if (checkBox_Bidirektional.Checked == false)
                            {
                                if (m_dec[i1].m_Element_Interface.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                            }
                            else
                            {
                                if (m_dec[i1].m_Element_Interface_Bidirectional.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                            }

                            if (flag_Grey == true)
                            {
                                Child.ForeColor = Color.Gray;
                            }


                            TreeNode Child_Supplier = new TreeNode(m_dec[i1].Get_Name(this.Database)) { Tag = m_dec[i1] };
                            Child.Name = Child.Text;
                            Child_Supplier.ForeColor = Color.Gray;
                            Ebene0_Supplier.Nodes.Add(Child_Supplier);


                            Show_Treeview_SysElement_rekursiv(m_dec[i1], Child, true);
                            Show_Treeview_SysElement_rekursiv(m_dec[i1], Child_Supplier, false);

                            i1++;
                        } while (i1 < m_dec.Count);
                    }
                }


                /////////////////
                //Farbe Reuirements anzeigen
                splitContainer10.Panel1.BackColor = Color.White;
                splitContainer11.Panel1.BackColor = Color.White;
            }


        }
        /// <summary>
        /// Befüllen eines Baumes
        /// </summary>
        /// <param name="NodeType"></param>
        /// <param name="Parent"></param>
        /// <param name="selectable"></param>
        private void Show_Treeview_NodeType_rekursiv(NodeType NodeType, TreeNode Parent, bool selectable)
        {
            if (NodeType.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    bool flag_Grey = false;

                    TreeNode Child = new TreeNode(NodeType.m_Child[i1].Get_Name(this.Database)) { Tag = NodeType.m_Child[i1] };
                    Child.Name = Child.Text;

                    if (checkBox_Bidirektional.Checked == false)
                    {
                        if (NodeType.m_Child[i1].m_Element_Interface.Count == 0)
                        {
                            flag_Grey = true;
                        }
                    }
                    else
                    {
                        if (NodeType.m_Child[i1].m_Element_Interface_Bidirectional.Count == 0)
                        {
                            flag_Grey = true;
                        }
                    }

                    if (flag_Grey == true)
                    {
                        Child.ForeColor = Color.Gray;
                    }

                    if (selectable == false)
                    {
                        Child.ForeColor = Color.Gray;
                    }

                    Parent.Nodes.Add(Child);

                    Show_Treeview_NodeType_rekursiv(NodeType.m_Child[i1], Child, selectable);

                    i1++;
                } while (i1 < NodeType.m_Child.Count);
            }

        }

        private void Show_Treeview_SysElement_rekursiv(SysElement NodeType, TreeNode Parent, bool selectable)
        {
            if (NodeType.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    bool flag_Grey = false;

                    TreeNode Child = new TreeNode(NodeType.m_Child[i1].Get_Name(this.Database)) { Tag = NodeType.m_Child[i1] };
                    Child.Name = Child.Text;

                    if (checkBox_Bidirektional.Checked == false)
                    {
                        if (NodeType.m_Child[i1].m_Element_Interface.Count == 0)
                        {
                            flag_Grey = true;
                        }
                    }
                    else
                    {
                        if (NodeType.m_Child[i1].m_Element_Interface_Bidirectional.Count == 0)
                        {
                            flag_Grey = true;
                        }
                    }

                    if (flag_Grey == true)
                    {
                        Child.ForeColor = Color.Gray;
                    }

                    if (selectable == false)
                    {
                        Child.ForeColor = Color.Gray;
                    }

                    Parent.Nodes.Add(Child);

                    Show_Treeview_SysElement_rekursiv(NodeType.m_Child[i1], Child, selectable);

                    i1++;
                } while (i1 < NodeType.m_Child.Count);
            }

        }
        /// <summary>
        /// Aufgerugfen wenn im Cleint Tree ein Node angewählt wird 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Supplier_Tree zurücksetzen
            //  MessageBox.Show("Reset");
            Reset_Tree_Decomposition(Supplier_Tree);
            //InfoElemBoxClearen
            InfoElemBox_Tree.Nodes.Clear();
            //AFO Text Clearen
            Text_Senden.Clear();
            Text_Empfangen.Clear();
            recent_Text_Empfangen[4] = "...";
            recent_Text_Senden[4] = "...";
            NodeType_Artikel.Text = "...";
            Target_Artikel.Text = "...";
            ////////
            //Farbe Reuirements reseten
            splitContainer10.Panel1.BackColor = Color.White;
            splitContainer11.Panel1.BackColor = Color.White;
            /////////
            //AFO Titel reseten
            if (checkBox_Bidirektional.Checked == true)
            {
                AFo_Titel_Senden.Text = this.Database.metamodel.m_Prozesswort_Interface[2] + ": ";

                AFo_Titel_Empfangen.Text = this.Database.metamodel.m_Prozesswort_Interface[2] + ": ";
            }
            else
            {
                AFo_Titel_Senden.Text = this.Database.metamodel.m_Prozesswort_Interface[0] + ": ";

                AFo_Titel_Empfangen.Text = this.Database.metamodel.m_Prozesswort_Interface[1] + ": ";
            }
            AFo_Titel_Senden.Text = ti.ToTitleCase(AFo_Titel_Senden.Text);
            AFo_Titel_Empfangen.Text = ti.ToTitleCase(AFo_Titel_Empfangen.Text);
            //

            // MessageBox.Show("selectedNode");
            TreeNode selectedNode = Client_Tree.SelectedNode;

            //    MessageBox.Show("Type of selectedNode.Tag");
            if (selectedNode.Tag != null)
            {
                if (selectedNode.Tag.GetType() == (typeof(NodeType)))
                {
                    //    MessageBox.Show("Tag");
                    NodeType selectedNodeType = selectedNode.Tag as NodeType;

                    this.recentTreeNode_Client = selectedNode;

                    //Aktuellen Node hervorheben
                    selectedNode.ForeColor = Color.White;
                    selectedNode.BackColor = Color.Green;

                    int length = 0;

                    if (checkBox_Bidirektional.Checked == true)
                    {
                        length = selectedNodeType.m_Element_Interface_Bidirectional.Count;
                    }
                    else
                    {
                        length = selectedNodeType.m_Element_Interface.Count;
                    }

                    ///Element Interface anzeigen --> hervorhebn
                    if (length > 0)
                    {
                        //  MessageBox.Show("Activate Suppliers: "+selectedNodeType.m_Element_Interface.Count);
                        int i1 = 0;
                        do
                        {
                            //  MessageBox.Show(selectedNodeType.m_Element_Interface[i1].Client.Instantiate_GUID);
                            if (checkBox_Bidirektional.Checked == true)
                            {
                                selectedNodeType.m_Element_Interface_Bidirectional[i1].Supplier.Activate_TreeView(Supplier_Tree, this.Database, this.Repository);
                            }
                            else
                            {
                                selectedNodeType.m_Element_Interface[i1].Supplier.Activate_TreeView(Supplier_Tree, this.Database, this.Repository);
                            }

                            i1++;
                        } while (i1 < length);
                    }
                    ////////////////////////
                    //Artikel zuordnen und anzeigen
                    NodeType_Artikel_Index = this.Database.metamodel.Artikel.FindIndex(x => x == selectedNodeType.W_Artikel);
                    NodeType_Artikel.Text = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
                    ///////////////////////
                    //AFO Überschrift
                    NodeType Client = this.recentTreeNode_Client.Tag as NodeType;

                    AFo_Titel_Senden.Text = AFo_Titel_Senden.Text + Client.Get_Name(this.Database) + " - ";
                    AFo_Titel_Empfangen.Text = AFo_Titel_Empfangen.Text + "... - " + Client.Get_Name(this.Database);

                    AFo_Titel_Senden.Text = ti.ToTitleCase(AFo_Titel_Senden.Text);
                    AFo_Titel_Empfangen.Text = ti.ToTitleCase(AFo_Titel_Empfangen.Text);
                    /////////////////////
                    //AFO Text
                    //Artikel als Hilfe
                    string help = Client.W_Artikel;
                    //AFo Text Senden befüllen
                    recent_Text_Senden[0] = help.ToString();
                    recent_Text_Senden[2] = Client.Get_Name(this.Database);
                    //AFo Text Empfangen befüllen
                    recent_Text_Empfangen[6] = this.Database.metamodel.Artikel[NodeType_Artikel_Index + 6];
                    recent_Text_Empfangen[8] = Client.Get_Name(this.Database);
                    //Beide Text Box schreiben
                    this.Database.Write_List(Text_Senden, recent_Text_Senden);
                    this.Database.Write_List(Text_Empfangen, recent_Text_Empfangen);
                    /////////////////////////////////////////
                }

                if (selectedNode.Tag.GetType() == (typeof(SysElement)))
                {
                    //    MessageBox.Show("Tag");
                    NodeType selectedNodeType = selectedNode.Tag as NodeType;

                    this.recentTreeNode_Client = selectedNode;

                    //Aktuellen Node hervorheben
                    selectedNode.ForeColor = Color.White;
                    selectedNode.BackColor = Color.Green;

                    int length = 0;

                    if (checkBox_Bidirektional.Checked == true)
                    {
                        length = selectedNodeType.m_Element_Interface_Bidirectional.Count;
                    }
                    else
                    {
                        length = selectedNodeType.m_Element_Interface.Count;
                    }

                    ///Element Interface anzeigen --> hervorhebn
                    if (length > 0)
                    {
                        //  MessageBox.Show("Activate Suppliers: "+selectedNodeType.m_Element_Interface.Count);
                        int i1 = 0;
                        do
                        {
                            //  MessageBox.Show(selectedNodeType.m_Element_Interface[i1].Client.Instantiate_GUID);
                            if (checkBox_Bidirektional.Checked == true)
                            {
                                selectedNodeType.m_Element_Interface_Bidirectional[i1].Supplier.Activate_TreeView(Supplier_Tree, this.Database, this.Repository);
                            }
                            else
                            {
                                selectedNodeType.m_Element_Interface[i1].Supplier.Activate_TreeView(Supplier_Tree, this.Database, this.Repository);
                            }

                            i1++;
                        } while (i1 < length);
                    }
                    ////////////////////////
                    //Artikel zuordnen und anzeigen
                    NodeType_Artikel_Index = this.Database.metamodel.Artikel.FindIndex(x => x == selectedNodeType.W_Artikel);
                    NodeType_Artikel.Text = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
                    ///////////////////////
                    //AFO Überschrift
                    NodeType Client = this.recentTreeNode_Client.Tag as NodeType;

                    AFo_Titel_Senden.Text = AFo_Titel_Senden.Text + Client.Get_Name(this.Database) + " - ";
                    AFo_Titel_Empfangen.Text = AFo_Titel_Empfangen.Text + "... - " + Client.Get_Name(this.Database);

                    AFo_Titel_Senden.Text = ti.ToTitleCase(AFo_Titel_Senden.Text);
                    AFo_Titel_Empfangen.Text = ti.ToTitleCase(AFo_Titel_Empfangen.Text);
                    /////////////////////
                    //AFO Text
                    //Artikel als Hilfe
                    string help = Client.W_Artikel;
                    //AFo Text Senden befüllen
                    recent_Text_Senden[0] = help.ToString();
                    recent_Text_Senden[2] = Client.Get_Name(this.Database);
                    //AFo Text Empfangen befüllen
                    recent_Text_Empfangen[6] = this.Database.metamodel.Artikel[NodeType_Artikel_Index + 6];
                    recent_Text_Empfangen[8] = Client.Get_Name(this.Database);
                    //Beide Text Box schreiben
                    this.Database.Write_List(Text_Senden, recent_Text_Senden);
                    this.Database.Write_List(Text_Empfangen, recent_Text_Empfangen);
                    /////////////////////////////////////////
                }
            }
        }

        /// <summary>
        /// Aufgerufen, wenn im Supplier Tree einer angewählt wird, wird aber vor setzen des Index ausgeführt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Supplier_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if(recentTreeNode_Supplier != null)
            {
                recentTreeNode_Supplier.ForeColor = Color.Black;
                recentTreeNode_Supplier.BackColor = Color.White;
            }

            if(Color.Gray == e.Node.ForeColor)
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// Tree wird resetet und mit der Decomposition befüllt
        /// </summary>
        /// <param name="Tree"></param>
        private void Reset_Tree_Decomposition(TreeView Tree)
        {
            Tree.Nodes[0].Nodes.Clear();
            Tree.CollapseAll();

            recentTreeNode_Supplier = null;

            if(this.Database.Decomposition.m_NodeType.Count > 0)
            {
                int i1 = 0;
                do
                {
                    TreeNode Child_Supplier = new TreeNode(Database.Decomposition.m_NodeType[i1].Get_Name(this.Database)) { Tag = Database.Decomposition.m_NodeType[i1] };
                    Child_Supplier.Name = Child_Supplier.Text;
                    Child_Supplier.ForeColor = Color.Gray;
                    Tree.Nodes[0].Nodes.Add(Child_Supplier);

                    Show_Treeview_NodeType_rekursiv(Database.Decomposition.m_NodeType[i1], Child_Supplier, false);

                    i1++;
                } while (i1 < this.Database.Decomposition.m_NodeType.Count);
            }

        }
        /// <summary>
        /// Aufgerufgen, wenn im Client Tree einer angewählt wird, ausgeführt vor Änderung des Index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //Supplier_Tree.CollapseAll();

            this.recentTreeNode_Client.ForeColor = Color.Black;
            this.recentTreeNode_Client.BackColor = Color.White;




            if (Color.Gray == e.Node.ForeColor)
            {
                Client_Tree.SelectedNode = e.Node.Parent;

                e.Cancel = true;
            }
        }
        /// <summary>
        /// Wenn Box NodeType Artikel geclickt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeType_Artikel_Click(object sender, EventArgs e)
        {
            bool flag = true;

            if(Client_Tree.SelectedNode == null || Client_Tree.SelectedNode.Level == 0)
            {
                flag = false;
                NodeType_Artikel.Text = "...";
            }

           

            switch (NodeType_Artikel_Index)
            {
                case 0:
                    NodeType_Artikel.Text = this.Database.metamodel.Artikel[1];
                    NodeType_Artikel_Index++;
                    break;
                case 1:
                    NodeType_Artikel.Text = this.Database.metamodel.Artikel[2];
                    NodeType_Artikel_Index++;
                    break;
                case 2:
                    NodeType_Artikel.Text = this.Database.metamodel.Artikel[0];
                    NodeType_Artikel_Index = 0;
                    break;
            }
            if (flag  == true)
            {
                NodeType recent = Client_Tree.SelectedNode.Tag as NodeType;
                ////
                //Artikel abspeichern für alle mit diesem NodeType
                if(this.Database.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(recent.Classifier_ID == this.Database.m_NodeType[i1].Classifier_ID)
                        {
                            this.Database.m_NodeType[i1].W_Artikel = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
                        }

                        i1++;
                    } while (i1 < this.Database.m_NodeType.Count);

                }
                /////////////////////////////////////////
                //Artikel in AFO Text neu schreiben
                recent_Text_Empfangen[6] = this.Database.metamodel.Artikel[NodeType_Artikel_Index + 6];
                recent_Text_Senden[0] = this.Database.metamodel.Artikel[NodeType_Artikel_Index];

                this.Database.Write_List(Text_Senden, recent_Text_Senden);
                this.Database.Write_List(Text_Empfangen, recent_Text_Empfangen);
                ////////////////////////////////////////
                //Artikel im Classifier hinterlegen
                TaggedValue Tagged = new TaggedValue(this.Database.metamodel, this.Database);

                List<DB_Insert> m_Insert = new List<DB_Insert>();
                m_Insert.Add(new DB_Insert("SYS_Artikel", OleDbType.VarChar, OdbcType.VarChar, this.Database.metamodel.Artikel[NodeType_Artikel_Index], -1));
                recent.Update_TV(m_Insert, Database, Repository);


                //Tagged.Update_Tagged_Value(recent.Classifier_ID, "SYS_Artikel", this.Database.metamodel.Artikel[NodeType_Artikel_Index], "Values: der, die, das", Repository);
            }
        }
        /// <summary>
        /// Wenn Supplier Artikel geclickt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Target_Artikel_Click(object sender, EventArgs e)
        {
            bool flag = true;

            if (Supplier_Tree.SelectedNode == null || Supplier_Tree.SelectedNode.Level == 0)
            {
                flag = false;
                Target_Artikel.Text = "...";
            }

            switch (Target_Artikel_Index)
            {
                case 0:
                    Target_Artikel.Text = this.Database.metamodel.Artikel[1];
                    Target_Artikel_Index++;
                    break;
                case 1:
                    Target_Artikel.Text = this.Database.metamodel.Artikel[2];
                    Target_Artikel_Index++;
                    break;
                case 2:
                    Target_Artikel.Text = this.Database.metamodel.Artikel[0];
                    Target_Artikel_Index = 0;
                    break;
            }
            if (flag == true)
            {
                NodeType recent = recentTreeNode_Supplier.Tag as NodeType;
                NodeType Client = recentTreeNode_Client.Tag as NodeType;
                ////
                if(recent.Classifier_ID == Client.Classifier_ID)
                {
                    NodeType_Artikel.Text = Target_Artikel.Text;
                }
                //Artikel abspeichern für alle mit diesem NodeType
                if (this.Database.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (recent.Classifier_ID == this.Database.m_NodeType[i1].Classifier_ID)
                        {
                            this.Database.m_NodeType[i1].W_Artikel = this.Database.metamodel.Artikel[Target_Artikel_Index];
                        }

                        i1++;
                    } while (i1 < this.Database.m_NodeType.Count);
                }

                /////////////////////////////////////////
                //Artikel in AFO Text neu schreiben
                int Artikel_ind = 3;

                if (checkBox_Bidirektional.Checked == true)
                {
                    Artikel_ind = 6;
                }

                recent_Text_Senden[6] = this.Database.metamodel.Artikel[Target_Artikel_Index + Artikel_ind];

                recent_Text_Empfangen[0] = this.Database.metamodel.Artikel[Target_Artikel_Index];
                //recent_Text_Senden[6] = Artikel[Target_Artikel_Index + 3];

                this.Database.Write_List(Text_Senden, recent_Text_Senden);
                this.Database.Write_List(Text_Empfangen, recent_Text_Empfangen);

                ////////////////////////////////////////
                //Artikel im Classifier hinterlegen
                TaggedValue Tagged = new TaggedValue(this.Database.metamodel, this.Database);
                List<DB_Insert> m_Insert = new List<DB_Insert>();
                m_Insert.Add(new DB_Insert("SYS_Artikel", OleDbType.VarChar, OdbcType.VarChar, this.Database.metamodel.Artikel[NodeType_Artikel_Index], -1));
                recent.Update_TV(m_Insert, Database, Repository);

               // Tagged.Update_Tagged_Value(recent.Classifier_ID, "SYS_Artikel", this.Database.metamodel.Artikel[NodeType_Artikel_Index], "Values: der, die, das", Repository);

            }
        }
        /// <summary>
        /// Nach Auswahl eines Nodes im Supplier Tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Supplier_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool flag = true;
            //InfoElemBoxClearen
            InfoElemBox_Tree.Nodes.Clear();


            if (Supplier_Tree.SelectedNode == null || Supplier_Tree.SelectedNode.Level == 0)
            {
                flag = false;
                Target_Artikel.Text = "...";
            }

            if(flag == true)
            {
                string Info_Elem_AFO = "";

                NodeType Client = Client_Tree.SelectedNode.Tag as NodeType;
                NodeType Supplier = Supplier_Tree.SelectedNode.Tag as NodeType;

                recentTreeNode_Supplier = Supplier_Tree.SelectedNode;

                recentTreeNode_Supplier.ForeColor = Color.White;
                recentTreeNode_Supplier.BackColor = Color.Green;

                List<InformationElement> m_InfoElem = new List<InformationElement>();

                int length = 0;

                if(checkBox_Bidirektional.Checked == true)
                {
                    length = Client.m_Element_Interface_Bidirectional.Count;
                }
                else
                {
                    length = Client.m_Element_Interface.Count;
                }

                if (length > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(checkBox_Bidirektional.Checked == true)
                        {
                            if(Client.m_Element_Interface_Bidirectional[i1].Supplier == Supplier)
                            {
                                m_InfoElem = Client.m_Element_Interface_Bidirectional[i1].Get_All_Information_Element();
                                i1 = Client.m_Element_Interface_Bidirectional.Count;
                            }
                        }
                        else
                        {
                            if (Client.m_Element_Interface[i1].Supplier == Supplier)
                            {
                                m_InfoElem = Client.m_Element_Interface[i1].Get_All_Information_Element();
                                i1 = Client.m_Element_Interface.Count;
                            }
                        }

                     

                        i1++;
                    } while (i1 < length);

                    //MessageBox.Show("Anzahl InfoElem: "+m_InfoElem.Count.ToString());

                    //Darstellen InfoElem
                    if(m_InfoElem.Count > 0)
                    {
                        //MessageBox.Show(m_InfoElem.Count.ToString());

                        InfoElemBox_Tree.CheckBoxes = true;

                        i1 = 0;
                        do
                        {

                            TreeNode recent_Node = new TreeNode(m_InfoElem[i1].Get_InformationItem_Name(this.Repository, this.Database)) { Tag = m_InfoElem[i1] };
                            recent_Node.Name = recent_Node.Text;
                            recent_Node.Checked = true;
                            InfoElemBox_Tree.Nodes.Add(recent_Node);
                          //  InfoElemBox_Tree.CheckBoxes = true;

                            //String für AFo Info Elem Text generieren
                            if(i1 != (m_InfoElem.Count -1))
                            {
                                Info_Elem_AFO = Info_Elem_AFO + m_InfoElem[i1].Get_InformationItem_Name(this.Repository, this.Database) + ", ";
                            }
                            else
                            {
                                Info_Elem_AFO = Info_Elem_AFO + m_InfoElem[i1].Get_InformationItem_Name(this.Repository, this.Database);
                            }

                            i1++;
                        } while (i1 < m_InfoElem.Count);

                    }
                    else
                    {
                        Info_Elem_AFO = "nicht spezifiziert";
                    }

                }

                ////////////////////////
                //Artikel zuordnen und anzeigen
                Target_Artikel_Index = this.Database.metamodel.Artikel.FindIndex(x => x == Supplier.W_Artikel);
                Target_Artikel.Text = this.Database.metamodel.Artikel[Target_Artikel_Index];
                //////////////////////////
                //AFO Überschrift
                if(checkBox_Bidirektional.Checked == true)
                {
                    AFo_Titel_Senden.Text = this.Database.metamodel.m_Prozesswort_Interface[2] + " : " + Client.Get_Name( this.Database) + " - " + Supplier.Get_Name(this.Database); ;
                    AFo_Titel_Empfangen.Text = this.Database.metamodel.m_Prozesswort_Interface[2] + " : " + Supplier.Get_Name(this.Database) + " - " + Client.Get_Name( this.Database);

                }
                else
                {

                    AFo_Titel_Senden.Text = this.Database.metamodel.m_Prozesswort_Interface[0] + ": " + Client.Get_Name( this.Database) + " - " + Supplier.Get_Name( this.Database);
                    AFo_Titel_Empfangen.Text = this.Database.metamodel.m_Prozesswort_Interface[1] + ": " + Supplier.Get_Name( this.Database) + " - " + Client.Get_Name( this.Database);
                }

                AFo_Titel_Senden.Text = ti.ToTitleCase(AFo_Titel_Senden.Text);
                AFo_Titel_Empfangen.Text = ti.ToTitleCase(AFo_Titel_Empfangen.Text);

                ////////////////////////
                //AFO Text befüllen
                string help = "";

                help = Supplier.W_Artikel;

                recent_Text_Empfangen[0] = help.ToString();
                recent_Text_Empfangen[2] = Supplier.Get_Name( this.Database);

                int Artikel_ind = 3;

                if(checkBox_Bidirektional.Checked == true)
                {
                    Artikel_ind = 6;
                }

                recent_Text_Senden[6] = this.Database.metamodel.Artikel[Target_Artikel_Index + Artikel_ind];
                recent_Text_Senden[8] = Supplier.Get_Name(this.Database);

                //Info Elem noch hinzufügen
                recent_Text_Senden[4] = Info_Elem_AFO;
                recent_Text_Empfangen[4] = Info_Elem_AFO;

                this.Database.Write_List(Text_Senden, recent_Text_Senden);
                this.Database.Write_List(Text_Empfangen, recent_Text_Empfangen);

              
            }


            //RequirementsFarben
            Set_Requiremnts_Color(false, 0);

        }
        /// <summary>
        /// Ausgeführt, wenn CheckStatus von Bidirektional sich ändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_Bidirektional_CheckedChanged(object sender, EventArgs e)
        {    ////////////////
            //Zurücksetzen
            Client_Tree.Nodes.Clear();
            Supplier_Tree.Nodes.Clear();
            Text_Senden.Clear();
            Text_Empfangen.Clear();

            ///////////
            //Ändern der Texte
            if (checkBox_Bidirektional.Checked == true)
            {
                //label_Client.Text = "Client, bidirektional";
                //label_Supplier.Text = "Supplier, bidirektional";
                AFo_Titel_Senden.Text = this.Database.metamodel.m_Prozesswort_Interface[2] + " Client: ";
                AFo_Titel_Empfangen.Text = this.Database.metamodel.m_Prozesswort_Interface[2] + " Supplier: ";

                AFo_Titel_Senden.Text = ti.ToTitleCase(AFo_Titel_Senden.Text);
                AFo_Titel_Empfangen.Text = ti.ToTitleCase(AFo_Titel_Empfangen.Text);

                recent_Text_Senden[9] = " "+this.Database.metamodel.m_Prozesswort_zu_Interface[2]+".";
                recent_Text_Empfangen[9] = " " + this.Database.metamodel.m_Prozesswort_zu_Interface[2] + ".";
                recent_Text_Senden[5] = ") mit ";
                recent_Text_Empfangen[5] = ") mit ";

                //Prozesswort
               // this.Database.metamodel.m_Prozesswort_Interface[0] = "austauschen";
               // this.Database.metamodel.m_Prozesswort_Interface[1] = "austauschen";

                //   InfoElemBox.Enabled = false;
            }
            else
            {
                //label_Client.Text = "Client";
                //label_Supplier.Text = "Supplier";
                AFo_Titel_Senden.Text = this.Database.metamodel.m_Prozesswort_Interface[0]+": ";
                AFo_Titel_Empfangen.Text = this.Database.metamodel.m_Prozesswort_Interface[1]+": ";

                AFo_Titel_Senden.Text = ti.ToTitleCase(AFo_Titel_Senden.Text);
                AFo_Titel_Empfangen.Text = ti.ToTitleCase(AFo_Titel_Empfangen.Text);

                recent_Text_Senden[9] = " " + this.Database.metamodel.m_Prozesswort_zu_Interface[0] + ".";
                recent_Text_Empfangen[9] = " " + this.Database.metamodel.m_Prozesswort_zu_Interface[1] + ".";
                recent_Text_Senden[5] = ") an ";
                recent_Text_Empfangen[5] = ") von ";
                //Prozesswort
              //  this.Database.metamodel.m_Prozesswort_Interface[0] = "senden";
                //this.m_Prozesswort[1] = "empfangen";
                //   InfoElemBox.Enabled = true;
            }

            /////////////////////


            //Neu laden
            Interface_Decomposition_Load(sender, e);

        }
        /// <summary>
        /// Ausgeführt, wenn Save geklickt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_AFO_Click(object sender, EventArgs e)
        {
            Repository_Class repository_Element = new Repository_Class();
            Repository_Connector repository_Connector = new Repository_Connector();
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Requirement_Plugin.Interfaces.Interface_Collection();

            if(recentTreeNode_Client != null && recentTreeNode_Supplier != null) //wurde ausgewählt
            {
                /////////////////////////////////
                //PAckage allg Requirement anlegen bzw. erhalten
                string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this.Database);
                EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
                //Package Infoübertragung anlegen bzw erhalten
                string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Infoübertragung - Requirement_Plugin", Repository, this.Database);
                EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                //Infoübertragung Child von Requirement
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Update();
                //////////////////////////////////
                //AFO abspeichern
                NodeType Client = recentTreeNode_Client.Tag as NodeType;
                NodeType Supplier = recentTreeNode_Supplier.Tag as NodeType;
                //Element_Interface suchen --> Check muss != Null sein
                Element_Interface Check;
                Element_Interface Check_Supplier = null;
                if (checkBox_Bidirektional.Checked == false)
                {
                    Check = Client.Check_Element_Interface(Supplier);
                }
                else
                {
                    //MessageBox.Show("nope");
                    
                    Check = Client.Check_Element_Interface_Bidirectional(Supplier);
                    Check_Supplier = Supplier.Check_Element_Interface_Bidirectional(Client);
                }
                

                EA.Element Requirement_help;

                    //Client
                    string help = recent_Text_Senden[5];
                    help = help.Remove(0, 2);

                    string W_OBJECT = Database.metamodel.string_Interface[0]+" ("+ recent_Text_Senden[4] + recent_Text_Senden[5] + recent_Text_Senden[6]+ recent_Text_Senden[7] + recent_Text_Senden[8];
                    string W_SUBJECT = Client.W_Artikel + " " + Client.Get_Name( this.Database);
                string Prozesswort_senden = "";
                if (checkBox_Bidirektional.Checked == false)
                {
                    Prozesswort_senden = this.Database.metamodel.m_Prozesswort_Interface[0];
                }
                else
                {
                    Prozesswort_senden = this.Database.metamodel.m_Prozesswort_Interface[2];
                }

                if (Check.m_Requirement_Interface_Send.Count == 0 && Check.m_Requirement_Interface_Receive.Count == 0)
                {
                    // MessageBox.Show("Senden Requirement erzeugen");
                    //Requiremnt_Interface in Datenbank einfügen
                   
                    Requirement_Interface Client_AFO = new Requirement_Interface(AFo_Titel_Senden.Text, Text_Senden.Text, W_OBJECT, Prozesswort_senden, "", "", true, W_SUBJECT, true, null, this.Database.metamodel);
                    Check.m_Requirement_Interface_Send.Add(Client_AFO);
                    if(checkBox_Bidirektional.Checked == true)
                    {
                        Check_Supplier.m_Requirement_Interface_Receive.Add(Client_AFO);
                    }
                    //AFO_anlegen
                    //MessageBox.Show(Client_AFO.W_ZU.ToString());
                    Client_AFO.Create_Requirement_Interface(Repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Interface[0].Stereotype, Database);
                }
                else
                {
                   // MessageBox.Show("Senden Requirement updaten");
                  /*  Check.m_Requirement_Interface_Send[0].AFO_TEXT = Text_Senden.Text;
                    Check.m_Requirement_Interface_Send[0].Name = AFo_Titel_Senden.Text;
                    Check.m_Requirement_Interface_Send[0].AFO_TITEL = AFo_Titel_Senden.Text;
                    Check.m_Requirement_Interface_Send[0].W_OBJEKT = W_OBJECT;
                    Check.m_Requirement_Interface_Send[0].W_SUBJEKT = W_SUBJECT;*/
                    Requirement_help = Repository.GetElementByGuid(Check.m_Requirement_Interface_Send[0].Classifier_ID);
                    Requirement_help.Notes = Text_Senden.Text;
                    Requirement_help.Name = AFo_Titel_Senden.Text;

                    Requirement_help.Update();

                    Requirement copy = new Requirement(Check.m_Requirement_Interface_Send[0].Classifier_ID, this.Database.metamodel);
                    copy.Get_Tagged_Values_From_Requirement(copy.Classifier_ID, Repository, Database);

                    Check.m_Requirement_Interface_Send[0].Update_Requirement(Repository, Database, AFo_Titel_Senden.Text, Text_Senden.Text, W_OBJECT, Prozesswort_senden, "", "", true, W_SUBJECT, true, null);

                    Check.m_Requirement_Interface_Send[0].Compare_Requirement(Database, Repository, copy);

                }

                    //Supplier
                    string help2 = recent_Text_Empfangen[5];
                    help2 = help2.Remove(0, 2);

                    
                string W_OBJECT2 = Database.metamodel.string_Interface[0] + " (" + recent_Text_Empfangen[4] + recent_Text_Empfangen[5] + recent_Text_Empfangen[6] + recent_Text_Empfangen[7] + recent_Text_Empfangen[8];

                string W_SUBJECT2 = Supplier.W_Artikel + " " + Supplier.Get_Name(this.Database);
                string Prozesswort_empfangen = "";
                if (checkBox_Bidirektional.Checked == false)
                {
                    Prozesswort_empfangen = this.Database.metamodel.m_Prozesswort_Interface[1];
                }
                else
                {
                    Prozesswort_empfangen = this.Database.metamodel.m_Prozesswort_Interface[2];
                }
                if (Check.m_Requirement_Interface_Receive.Count == 0)
                {
                    // MessageBox.Show("Empfangen Requirement erzeugen");
                    //Requiremnt_Interface in Datenbank einfügen
                   
                    Requirement_Interface Supplier_AFO = new Requirement_Interface(AFo_Titel_Empfangen.Text, Text_Empfangen.Text, W_OBJECT2, Prozesswort_empfangen, "", "", true, W_SUBJECT2, true, null, this.Database.metamodel);
                    Check.m_Requirement_Interface_Receive.Add(Supplier_AFO);
                    if (checkBox_Bidirektional.Checked == true)
                    {
                        Check_Supplier.m_Requirement_Interface_Send.Add(Supplier_AFO);
                    }
                    //AFO_anlegen
                    Supplier_AFO.Create_Requirement_Interface(Repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Interface[0].Stereotype, Database);
                }
                else
                {
                  //  MessageBox.Show("Empfangen Requirement updaten");
                  //  Check.m_Requirement_Interface_Receive[0].AFO_TEXT = Text_Empfangen.Text;
                    Requirement_help = Repository.GetElementByGuid(Check.m_Requirement_Interface_Receive[0].Classifier_ID);
                    Requirement_help.Notes = Text_Empfangen.Text;
                    Requirement_help.Name = AFo_Titel_Empfangen.Text;
                    Requirement_help.Update();
                    Requirement copy2 = new Requirement(Check.m_Requirement_Interface_Receive[0].Classifier_ID, this.Database.metamodel);
                    copy2.Get_Tagged_Values_From_Requirement(copy2.Classifier_ID, Repository, Database);
                    Check.m_Requirement_Interface_Receive[0].Update_Requirement(Repository, Database, AFo_Titel_Empfangen.Text, Text_Empfangen.Text, W_OBJECT2, Prozesswort_empfangen, "", "", true, W_SUBJECT2, true, null);

                    Check.m_Requirement_Interface_Receive[0].Compare_Requirement(Database, Repository, copy2);
                }

                //////
                ///Connectoren der Anforderungen löschen
                //DerivedFrom --> InformationItem löschen
        //        Check.m_Requirement_Interface_Send[0].Delete_All_Connector(Repository, Database, Database.metamodel.m_InformationItem.Select(x => x.Type).ToList(), Database.metamodel.m_InformationItem.Select(x => x.Stereotype).ToList());
        //        Check.m_Requirement_Interface_Receive[0].Delete_All_Connector(Repository, Database, Database.metamodel.m_InformationItem.Select(x => x.Type).ToList(), Database.metamodel.m_InformationItem.Select(x => x.Stereotype).ToList());
                //DerivedFrom --> Node etc löschen
         //       Check.m_Requirement_Interface_Send[0].Delete_All_Connector(Repository, Database, Database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList());
         //       Check.m_Requirement_Interface_Send[0].Delete_All_Connector(Repository, Database, Database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
         //       Check.m_Requirement_Interface_Receive[0].Delete_All_Connector(Repository, Database, Database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList());
         //       Check.m_Requirement_Interface_Receive[0].Delete_All_Connector(Repository, Database, Database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                
                #region Create Dependency
                //////////////////////////////////////////
                /////////////////////////////////////////////////
                //Zum m_Requirements des Sender hiinzufügen
                Check.m_Requirement_Interface_Send[0].m_Requirement_Requires.Add(Check.m_Requirement_Interface_Receive[0]);
                //Connectoren anlegen zu dem Sender und Empfänger --> Targets und InfoElem
                if (Check.m_Target.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        //Client_AFo zu Client bei Target
                        //Check.m_Requirement_Interface_Send[0].Create_Dependency(Check.m_Target[i1].CLient_ID, Database.metamodel.StereoType_Derived[0], Database.metamodel.StereoType_Derived[1], Repository);
                        Check.m_Target[i1].Create_Dependency(Check.m_Requirement_Interface_Send[0].Classifier_ID, Check.m_Target[i1].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                        //Supplier_AFO zu Supplier bei Target
                        //Check.m_Requirement_Interface_Receive[0].Create_Dependency(Check.m_Target[i1].Supplier_ID, Database.metamodel.StereoType_Derived[0], Database.metamodel.StereoType_Derived[1], Repository);
                        Check.m_Target[i1].Create_Dependency(Check.m_Requirement_Interface_Receive[0].Classifier_ID, Check.m_Target[i1].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                        //ERkennne, dass die AFO zusammengehören
                        //Check.m_Requirement_Interface_Send[0].Create_Dependency(Check.m_Requirement_Interface_Receive[0].Classifier_ID, Database.metamodel.StereoType_Send[0], Database.metamodel.StereoType_Send[1], Repository); //StereoType anpasssen an MetaModel
                        Check.m_Target[i1].Create_Dependency(Check.m_Requirement_Interface_Send[0].Classifier_ID, Check.m_Requirement_Interface_Receive[0].Classifier_ID, Database.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Requires.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Requires.Select(x => x.SubType).ToList()[0], Repository, Database, Database.metamodel.m_Afo_Requires.Select(x => x.Toolbox).ToList()[0], Database.metamodel.m_Afo_Requires[0].direction); //StereoType anpasssen an MetaModel

                        i1++;
                    } while (i1 < Check.m_Target.Count);

                }


                //Nodes welche ausgewählt wurden Weiß setzen und nicht ausgewählte Connectoren löschen
                if (InfoElemBox_Tree.Nodes.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        InformationElement elem = InfoElemBox_Tree.Nodes[i2].Tag as InformationElement;

                        if (InfoElemBox_Tree.Nodes[i2].Checked == true)
                        {
                            InfoElemBox_Tree.BackColor = Color.White;
                            //Check.m_Requirement_Interface_Send[0].Create_Dependency(elem.Classifier_ID, Database.metamodel.StereoType_Derived[0], Database.metamodel.StereoType_Derived[1], Repository);
                            repository_Connector.Create_Dependency(Check.m_Requirement_Interface_Send[0].Classifier_ID, elem.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                            //Check.m_Requirement_Interface_Receive[0].Create_Dependency(elem.Classifier_ID, Database.metamodel.StereoType_Derived[0], Database.metamodel.StereoType_Derived[1], Repository);
                            repository_Connector.Create_Dependency(Check.m_Requirement_Interface_Receive[0].Classifier_ID, elem.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                            if (checkBox_Bidirektional.Checked == true)
                            {
                                //Check_Supplier.m_Requirement_Interface_Send[0].Create_Dependency(elem.Classifier_ID, Database.metamodel.StereoType_Derived[0], Database.metamodel.StereoType_Derived[1], Repository);
                                repository_Connector.Create_Dependency(Check_Supplier.m_Requirement_Interface_Send[0].Classifier_ID, elem.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                //Check_Supplier.m_Requirement_Interface_Receive[0].Create_Dependency(elem.Classifier_ID, Database.metamodel.StereoType_Derived[0], Database.metamodel.StereoType_Derived[1], Repository);
                                repository_Connector.Create_Dependency(Check_Supplier.m_Requirement_Interface_Receive[0].Classifier_ID, elem.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                            }

                        }
                        else
                        {

                           // InfoElemBox_Tree.Nodes[i2].Remove();
                            //Check.m_Requirement_Interface_Send[0].Delete_Connector_Supplier(elem.Classifier_ID, Repository, Database.metamodel);
                            repository_Connector.Delete_Connector_Supplier(Check.m_Requirement_Interface_Send[0].Classifier_ID, elem.Classifier_ID, Repository, Database.metamodel, Database);

                            //Check.m_Requirement_Interface_Receive[0].Delete_Connector_Supplier(elem.Classifier_ID, Repository, Database.metamodel);
                            repository_Connector.Delete_Connector_Supplier(Check.m_Requirement_Interface_Receive[0].Classifier_ID, elem.Classifier_ID, Repository, Database.metamodel, Database);


                            if (checkBox_Bidirektional.Checked == true)
                            {
                                //Check_Supplier.m_Requirement_Interface_Send[0].Delete_Connector_Supplier(elem.Classifier_ID, Repository, Database.metamodel);
                                repository_Connector.Delete_Connector_Supplier(Check_Supplier.m_Requirement_Interface_Send[0].Classifier_ID, elem.Classifier_ID, Repository, Database.metamodel, Database);

                                //Check_Supplier.m_Requirement_Interface_Receive[0].Delete_Connector_Supplier(elem.Classifier_ID, Repository, Database.metamodel);
                                repository_Connector.Delete_Connector_Supplier(Check_Supplier.m_Requirement_Interface_Receive[0].Classifier_ID, elem.Classifier_ID, Repository, Database.metamodel, Database);

                            }
                        }

                        i2++;
                    } while (i2 < InfoElemBox_Tree.Nodes.Count);
                }

                //////////////////////
                //AFo zu Capability zuordnen
               Requirement_Plugin.xml.XML xML = new Requirement_Plugin.xml.XML();
                Requirement_Plugin.Interfaces.Interface_XML interface_XML = new Requirement_Plugin.Interfaces.Interface_XML();


                List<string> m_Cap_GUID = interface_XML.SQL_Query_Select( "Name", Database.metamodel.m_Capability_Interface[0].DefaultName, "ea_guid", "t_object", Database);

                string Cap_GUID = "";

                if(m_Cap_GUID != null)
                {
                    Cap_GUID = m_Cap_GUID[0];
                }
                else
                {
                    /* if (Database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                     {
                         Database.oLEDB_Interface.dbConnection.Close();
                     }*/
                    interface_Collection_OleDB.Close_Connection(Database);
                    Cap_GUID = repository_Element.Create_Element_Class(Database.metamodel.m_Capability_Interface[0].DefaultName, Database.metamodel.m_Capability[0].Type, Database.metamodel.m_Capability[0].Stereotype, Database.metamodel.m_Capability[0].Toolbox, -1, this.Package_InfoÜbertragung_ID, Repository, Database.metamodel.m_Capability_Interface[0].DefaultName, Database);
                    interface_Collection_OleDB.Open_Connection(Database);
                    /* if (Database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                    {
                        Database.oLEDB_Interface.dbConnection.Open();
                    }*/
                }

                //Check.m_Requirement_Interface_Send[0].Create_Dependency(Cap_GUID, Database.metamodel.StereoType_Derived[2], Database.metamodel.StereoType_Derived[3], Repository);
                repository_Connector.Create_Dependency(Check.m_Requirement_Interface_Send[0].Classifier_ID, Cap_GUID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);
                //Check.m_Requirement_Interface_Receive[0].Create_Dependency(Cap_GUID, Database.metamodel.StereoType_Derived[2], Database.metamodel.StereoType_Derived[3], Repository);
                repository_Connector.Create_Dependency(Check.m_Requirement_Interface_Receive[0].Classifier_ID, Cap_GUID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);

                Capability cap_interface = Database.Check_Capability_Database(Cap_GUID);

                if(cap_interface == null)
                {
                    cap_interface = new Capability(Cap_GUID, this.Repository, this.Database);
                    this.Database.m_Capability.Add(cap_interface);
                }

                if(Check.m_Requirement_Interface_Send[0].m_Capability.Contains(cap_interface) == false)
                {
                    Check.m_Requirement_Interface_Send[0].m_Capability.Add(cap_interface);
                }
                if (Check.m_Requirement_Interface_Receive[0].m_Capability.Contains(cap_interface) == false)
                {
                    Check.m_Requirement_Interface_Receive[0].m_Capability.Add(cap_interface);
                }

                

                if (checkBox_Bidirektional.Checked == true)
                {
                    //Check_Supplier.m_Requirement_Interface_Send[0].Create_Dependency(Cap_GUID, Database.metamodel.StereoType_Derived[2], Database.metamodel.StereoType_Derived[3], Repository);
                    repository_Connector.Create_Dependency(Check_Supplier.m_Requirement_Interface_Send[0].Classifier_ID, Cap_GUID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);
                    //Check_Supplier.m_Requirement_Interface_Receive[0].Create_Dependency(Cap_GUID, Database.metamodel.StereoType_Derived[2], Database.metamodel.StereoType_Derived[3], Repository);
                    repository_Connector.Create_Dependency(Check_Supplier.m_Requirement_Interface_Receive[0].Classifier_ID, Cap_GUID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);

                    if (Check_Supplier.m_Requirement_Interface_Send[0].m_Capability.Contains(cap_interface) == false)
                    {
                        Check_Supplier.m_Requirement_Interface_Send[0].m_Capability.Add(cap_interface);
                    }
                    if (Check_Supplier.m_Requirement_Interface_Receive[0].m_Capability.Contains(cap_interface) == false)
                    {
                        Check_Supplier.m_Requirement_Interface_Receive[0].m_Capability.Add(cap_interface);
                    }

                    
                }

                //Zuorndung System
                if(Database.metamodel.flag_sysarch == true)
                {
                    repository_Connector.Create_Dependency(Check.m_Requirement_Interface_Send[0].Classifier_ID, Check.Client.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, Repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);

                    repository_Connector.Create_Dependency(Check.m_Requirement_Interface_Receive[0].Classifier_ID, Check.Client.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, Repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);

                }

                #endregion Create_Dependency

                Check.m_Requirement_Interface_Send[0].Get_InformationElement(Repository, Database);
                Check.m_Requirement_Interface_Receive[0].Get_InformationElement(Repository, Database);

                if(checkBox_Bidirektional.Checked == true)
                {
                    Check_Supplier.m_Requirement_Interface_Send[0].Get_InformationElement(Repository, Database);
                    Check_Supplier.m_Requirement_Interface_Receive[0].Get_InformationElement(Repository, Database);
                }

                //RequirementsFarben
                Set_Requiremnts_Color(false, 0);

            }
        }
        /// <summary>
        /// InformationElement wird farbig hinterlegt, auch Status der REquirements
        /// </summary>
        /// <param name="Check_Index"></param>
        /// <param name="Checked_Index"></param>
        private void Set_Requiremnts_Color(bool Check_Index, int Checked_Index)
        {
            /////////////////////
            //Farbe Requirments setzen
            List<InformationElement> Requirement_List = new List<InformationElement>(); ;

            NodeType Client = Client_Tree.SelectedNode.Tag as NodeType;
            NodeType Supplier = Supplier_Tree.SelectedNode.Tag as NodeType;

            Element_Interface Check = null;

            if (checkBox_Bidirektional.Checked == true)
            {
                Check = Client.Check_Element_Interface_Bidirectional(Supplier);
            }
            else
            {
                Check = Client.Check_Element_Interface(Supplier);
            }
            if(true)
            { 
                if (Check.m_Requirement_Interface_Send.Count == 0 && Check.m_Requirement_Interface_Receive.Count == 0)
                {
                    //keine AFO vorhanden bisher
                    splitContainer10.Panel1.BackColor = Color.Red;
                    splitContainer11.Panel1.BackColor = Color.Red;

                    //                        Requirement_List = null;
                }
                else
                {
                    //Überprüfen InfoElem
                    Requirement_List = Check.m_Requirement_Interface_Send[0].m_InformationElement;
                   // MessageBox.Show("REquireemnt Anzahl InfoElem: " + Requirement_List.Count.ToString());
                    splitContainer10.Panel1.BackColor = Color.Green;
                    splitContainer11.Panel1.BackColor = Color.Green;

                    bool flag_Color = true;

                    if(InfoElemBox_Tree.Nodes.Count > 0)
                    {
                        int i1 = 0;

                        do
                        {
                            if(InfoElemBox_Tree.Nodes[i1].Checked == true && InfoElemBox_Tree.Nodes[i1].BackColor == Color.Red)
                            {
                                flag_Color = false;
                            }
                            else
                            {
                                if (Check.m_Requirement_Interface_Send[0].m_InformationElement.Contains(InfoElemBox_Tree.Nodes[i1].Tag as InformationElement) == false)
                                {
                                    if (InfoElemBox_Tree.Nodes[i1].Checked == true)
                                    {
                                        //InfoElem was in aktuelen Requirement nicht angewählt
                                        flag_Color = false;
                                        InfoElemBox_Tree.Nodes[i1].BackColor = Color.Yellow;
                                    }
                                    else
                                    {
                                        if(InfoElemBox_Tree.Nodes[i1].BackColor != Color.Red)
                                        {
                                            InfoElemBox_Tree.Nodes[i1].BackColor = Color.White;
                                        }
                                        
                                    }

                                }
                                else
                                {
                                    //Requirement besitzt zu viele InfoElem
                                    //hinzufügen, nicht wählbar und rot machen
                                    if (InfoElemBox_Tree.Nodes[i1].Checked == false && InfoElemBox_Tree.Nodes[i1].BackColor != Color.Red)
                                    {
                                        flag_Color = false;
                                        InfoElemBox_Tree.Nodes[i1].BackColor = Color.Yellow;
                                    }
                                    else
                                    {
                                        if(InfoElemBox_Tree.Nodes[i1].BackColor != Color.Red)
                                        {
                                            InfoElemBox_Tree.Nodes[i1].BackColor = Color.White;
                                        }
                                        //InfoElemBox_Tree.Nodes[i1].BackColor = Color.White;


                                    }
                                }
                            }

                           

                            i1++;
                        } while (i1 < InfoElemBox_Tree.Nodes.Count);

                    }
                   

                    //Hinvon Info Elem aus dem REquirementzufügen 
                    if(Check.m_Requirement_Interface_Send[0].m_InformationElement.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            bool flag_InfoElem = false;

                            if (InfoElemBox_Tree.Nodes.Count > 0)
                            {
                                

                                int i3 = 0;
                                do
                                {
                                    if(Check.m_Requirement_Interface_Send[0].m_InformationElement[i2] == (InfoElemBox_Tree.Nodes[i3].Tag as InformationElement))
                                    {
                                        flag_InfoElem = true;
                                    }

                                    i3++;
                                } while (i3 < InfoElemBox_Tree.Nodes.Count);
                            }

                            if (flag_InfoElem == false)
                            {
                                TreeNode recent_Node = new TreeNode(Check.m_Requirement_Interface_Send[0].m_InformationElement[i2].Get_InformationItem_Name(this.Repository, this.Database)) { Tag = Check.m_Requirement_Interface_Send[0].m_InformationElement[i2] };
                                recent_Node.Name = recent_Node.Text;
                                recent_Node.BackColor = Color.Red;
                                recent_Node.Checked = true;

                               // flag_Color = false;

                                InfoElemBox_Tree.Nodes.Add(recent_Node);
                            }

                            i2++;
                        } while (i2 < Check.m_Requirement_Interface_Send[0].m_InformationElement.Count);
                    }

                    if (flag_Color == false)
                    {
                        splitContainer10.Panel1.BackColor = Color.Yellow;
                        splitContainer11.Panel1.BackColor = Color.Yellow;
                    }

                   
                }
            }
        }
        /// <summary>
        /// Ausgeführt wenn in InformationeElemnt gechecked opder unchecked wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoElemBox_Tree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //Der geänderte Index
            TreeNode Changed_Node = e.Node;

            //AFo Texte reseten bezüglich InfoElem
            recent_Text_Senden[4] = "...";
            recent_Text_Empfangen[4] = "...";

            string Info_Elem = "";

            if (InfoElemBox_Tree.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (InfoElemBox_Tree.Nodes[i1].Checked == true)
                    {
                        if (Info_Elem == "")
                        {
                            Info_Elem = Info_Elem + InfoElemBox_Tree.Nodes[i1].Text;
                        }
                        else
                        {
                            Info_Elem = Info_Elem + ", " + InfoElemBox_Tree.Nodes[i1].Text;
                        }
                    }

                    i1++;
                } while (i1 < InfoElemBox_Tree.Nodes.Count);

            }

            if (Info_Elem == "")
            {
                Info_Elem = "keine";
            }

            recent_Text_Senden[4] = Info_Elem;
            recent_Text_Empfangen[4] = Info_Elem;

            this.Database.Write_List(Text_Senden, recent_Text_Senden);
            this.Database.Write_List(Text_Empfangen, recent_Text_Empfangen);

            //RequirementsFarben
            Set_Requiremnts_Color(false, 0);

            InfoElemBox_Tree.Enabled = true;
        }
        /// <summary>
        /// Aktuelle sinnlos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoElemBox_Tree_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            InfoElemBox_Tree.Enabled = false;
            //TreeNode recent_Node = e.Node;

            if (Color.Red == e.Node.BackColor)
            {
              //  e.Cancel = true;
            }
        }
        /// <summary>
        /// Interface Decomposition wird geschlossen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            this.Close();
            //this.Repository.RefreshModelView(0);
        }

        private void splitContainer10_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

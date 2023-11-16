using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.Odbc;

using Database_Connection;
using Repsoitory_Elements;
using Requirements;
using Requirement_Plugin;
using Microsoft.Office.Interop.Excel;


namespace Forms
{
    public partial class NonFunctional_Decomposition : Form
    {
        Requirement_Plugin.Database Database;
        EA.Repository repository;

        ////////////////
        //Aktuelle Elemente
        private TreeNode recentTreeNode_Client;
        private TreeNode recentTreeNode_Supplier;
        private TreeNode recentTreeNode_Activity;
        private TreeNode recentTreeNode_Measurement;
        private TreeView recent_Supplier_View;
        private TreeView recent_treeView;
        private TreeView recent_Measurement;
        private OperationalConstraint recent_opcon;
        private NodeType receent_typ;
        private TreeNode Ebene0;

        private TreeNode recent_node;
        ///Artikel Client und Supplier
        public int NodeType_Artikel_Index = 0;
        private int Supplier_index = 0;
        private int Activity_index = 0;
        private string requirement_Text = null;
        private string requirement_Titel = null;

        private Requirement_Non_Functional recent_req = new Requirement_Non_Functional(null, null);
        ///Welcher Type
        int type; //-1 nichts
                  //0 Qualitätsbedingung
                  //1 Designanforderung
                  //2 Prozessanforderung
                  //3 Umweltbedingung
        #region Tooltips
        List<string> m_Tooltipp_Title = new List<string>();
        List<string> m_Tooltipp_Text = new List<string>();


        #endregion Tooltips
        #region Ini
        public NonFunctional_Decomposition(Requirement_Plugin.Database Database, EA.Repository Repository)
        {
            this.Database = Database;
            this.repository = Repository;
            this.recentTreeNode_Client = null;
            this.recentTreeNode_Supplier = null;
            this.recent_treeView = null;
            this.recent_Supplier_View = null;
            this.recent_opcon = null;
            this.type = -1;
            this.requirement_Text = "";
            this.requirement_Titel = "";

            InitializeComponent();

            this.Visibility_Panel();

            m_Tooltipp_Title.Add("Keine Anforderung gewählt");
            m_Tooltipp_Text.Add("Wählen Sie zunächst eine Kombination aus.");

            m_Tooltipp_Title.Add("Anforderung im Model vorhanden");
            m_Tooltipp_Text.Add("Die gewählte Kombination ist im Model vorhanden.");

            m_Tooltipp_Title.Add("Anforderung nicht vollständig im Model vorhanden");
            m_Tooltipp_Text.Add("Die gewählte Kombination ist im Modell nicht vollständig vorhanden. Der Text weicht ab oder es sind nicht alle Konnektoren vorhanden.");

            m_Tooltipp_Title.Add("Anforderung möglicherweise doppelt vorhanden");
            m_Tooltipp_Text.Add("Die gewählte Kombination ist im Model möglicher weise doppelt vorhanden. Dies kommt z.B. vor wenn diesselbe Kombination für eine Funktionale und Nutzer Anforderung vorhanden ist.");

            m_Tooltipp_Title.Add("Anforderung nicht vorhanden");
            m_Tooltipp_Text.Add("Die gewählte Kombination ist im Model nicht vorhanden.");


            this.ToolTipp_Farbe.ToolTipTitle = m_Tooltipp_Title[0];
            this.ToolTipp_Farbe.SetToolTip(this.Requirement_Color, m_Tooltipp_Text[0]);

            if(this.Database.metamodel.flag_sysarch == true)
            {
                this.checkBox_logical_elements.Checked = false;
            }
            else
            {
                this.checkBox_logical_elements.Checked = true;
            }

            this.checkBox_logical_elements.Update();

        }

        private void NonFunctional_Decomposition_Load(object sender, EventArgs e)
        {
            if (Database.Decomposition != null)
            {
                //Client
                //Ersten Knoten schaffen des Cleint Tree schaffen
                TreeNode help = new TreeNode("Client") { Tag = Database.Decomposition.Classifier_ID };
                Ebene0 = help;
                Ebene0.Name = Ebene0.Text;
                recentTreeNode_Client = Ebene0;
                //Ebene 0 hinzufügen
                Client_Tree.Nodes.Add(Ebene0);
                //Komplette Decomposition anlegen
                this.Create_Client_Tree();
            }

        }

        private void Visibility_Panel()
        {
            switch(this.type)
            {
                case -1:
                    this.panel_Qualitätsbedingung.Visible = false;
                    this.panel_Designanforderung.Visible = false;
                    this.panel_Prozessanforderung.Visible = false;
                    this.panel_Umweltbedingung.Visible = false;
                    this.panel_Description.Visible = false;
                    break;
                case 0:
                    this.panel_Qualitätsbedingung.Visible = true;
                    this.panel_Designanforderung.Visible = false;
                    this.panel_Prozessanforderung.Visible = false;
                    this.panel_Umweltbedingung.Visible = false;
                    this.panel_Description.Visible = true ;
                    break;
                case 1:
                    this.panel_Qualitätsbedingung.Visible = false;
                    this.panel_Designanforderung.Visible = true;
                    this.panel_Prozessanforderung.Visible = false;
                    this.panel_Umweltbedingung.Visible = false;
                    this.panel_Description.Visible = true;
                    break;
                case 2:
                    this.panel_Qualitätsbedingung.Visible = false;
                    this.panel_Designanforderung.Visible = false;
                    this.panel_Prozessanforderung.Visible = true;
                    this.panel_Umweltbedingung.Visible = false;
                    this.panel_Description.Visible = true;
                    break;
                case 3:
                    this.panel_Qualitätsbedingung.Visible = false;
                    this.panel_Designanforderung.Visible = false;
                    this.panel_Prozessanforderung.Visible = false;
                    this.panel_Umweltbedingung.Visible = true;
                    this.panel_Description.Visible = true;
                    break;
                case 4:
                    this.panel_Qualitätsbedingung.Visible = true;
                    this.panel_Designanforderung.Visible = false;
                    this.panel_Prozessanforderung.Visible = false;
                    this.panel_Umweltbedingung.Visible = false;
                    this.panel_Description.Visible = true;
                    break;
                case 5:
                    this.panel_Qualitätsbedingung.Visible = true;
                    this.panel_Designanforderung.Visible = false;
                    this.panel_Prozessanforderung.Visible = false;
                    this.panel_Umweltbedingung.Visible = false;
                    this.panel_Description.Visible = true;
                    break;
                case 6:
                    this.panel_Qualitätsbedingung.Visible = false;
                    this.panel_Designanforderung.Visible = false;
                    this.panel_Prozessanforderung.Visible = true;
                    this.panel_Umweltbedingung.Visible = false;
                    this.panel_Description.Visible = true;
                    break;
            }

            panel_Type.Refresh();
        }
        #endregion Ini
        #region Function Baum Erstellnúng
        private void Create_Client_Tree()
        {
            this.Client_Tree.Nodes.Clear();

            TreeNode help = new TreeNode("Client") { Tag = Database.Decomposition.Classifier_ID };
            Ebene0 = help;
            Ebene0.Name = Ebene0.Text;

            #region NodeType
            if (this.Database.metamodel.flag_sysarch == false)
            {
                if (this.Database.Decomposition.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        bool flag_Grey = false;



                        TreeNode Child = new TreeNode(Database.Decomposition.m_NodeType[i1].Name) { Tag = Database.Decomposition.m_NodeType[i1] };
                        Child.Name = Child.Text;
                        this.Ebene0.Nodes.Add(Child);

                        switch (this.type)
                        {
                            case -1:
                                flag_Grey = true;
                                break;
                            case 0:
                                flag_Grey = true;
                                break;
                            case 1:
                                if (Database.Decomposition.m_NodeType[i1].m_Design.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 2:
                                if (Database.Decomposition.m_NodeType[i1].m_Element_Functional.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 3:
                                if (Database.Decomposition.m_NodeType[i1].m_Enviromental.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 4:
                                if (Database.Decomposition.m_NodeType[i1].m_Typvertreter.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 5:
                                if (Database.Decomposition.m_NodeType[i1].m_Element_Measurement.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 6:
                                if (Database.Decomposition.m_NodeType[i1].m_Element_Functional.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                        }



                        if (flag_Grey == true)
                        {
                            Child.ForeColor = Color.Gray;
                        }

                        Show_Treeview_NodeType_Functional_rekursiv(Database.Decomposition.m_NodeType[i1], Child, true);


                        i1++;
                    } while (i1 < this.Database.Decomposition.m_NodeType.Count);
                }
            }
            #endregion NodeType
            #region SysElement 
            else
            {
                List<SysElement> m_treesys = new List<SysElement>();
                m_treesys = Database.m_SysElemente.Where(x => x.m_Parent.Count == 0).ToList();

                if (m_treesys.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        bool flag_Grey = false;



                        TreeNode Child = new TreeNode(m_treesys[i1].Name) { Tag = m_treesys[i1] };
                        Child.Name = Child.Text;
                        this.Ebene0.Nodes.Add(Child);

                        switch (this.type)
                        {
                            case -1:
                                flag_Grey = true;
                                break;
                            case 0:
                                flag_Grey = true;
                                break;
                            case 1:
                                if (m_treesys[i1].Get_m_Element_Design().Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 2:
                                if (m_treesys[i1].Get_m_Element_Functional().Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 3:
                                if (m_treesys[i1].Get_m_Element_Environment().Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 4:
                                if (m_treesys[i1].Get_m_Element_Typvertreter().Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 5:
                                if (m_treesys[i1].m_Element_Measurement.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                            case 6:
                                if (m_treesys[i1].m_Element_Functional.Count == 0)
                                {
                                    flag_Grey = true;
                                }
                                break;
                        }



                        if (flag_Grey == true)
                        {
                            Child.ForeColor = Color.Gray;
                        }

                        Show_Treeview_SysElement_Functional_rekursiv(m_treesys[i1], Child, true);


                        i1++;
                    } while (i1 < m_treesys.Count);
                }
            }
            #endregion SysElement

            //Ebene 0 hinzufügen
            Client_Tree.Nodes.Add(Ebene0);
        }

        /// <summary>
        /// Befüllen eines Baumes
        /// </summary>
        /// <param name="NodeType"></param>
        /// <param name="Parent"></param>
        /// <param name="selectable"></param>
        private void Show_Treeview_NodeType_Functional_rekursiv(NodeType NodeType, TreeNode Parent, bool selectable)
        {
            if (NodeType.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    bool flag_Grey = false;

                    TreeNode Child = new TreeNode(NodeType.m_Child[i1].Name) { Tag = NodeType.m_Child[i1] };
                    Child.Name = Child.Text;

                    switch (this.type)
                    {
                        case -1:
                            flag_Grey = true;
                            break;
                        case 0:
                            flag_Grey = true;
                            break;
                        case 1:
                            if (NodeType.m_Child[i1].m_Design.Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
                        case 2:
                            if (NodeType.m_Child[i1].m_Element_Functional.Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
                        case 3:
                            if (NodeType.m_Child[i1].m_Enviromental.Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
                        case 4:
                            if (NodeType.m_Child[i1].m_Typvertreter.Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
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

                    Show_Treeview_NodeType_Functional_rekursiv(NodeType.m_Child[i1], Child, selectable);

                    i1++;
                } while (i1 < NodeType.m_Child.Count);
            }

        }

        private void Show_Treeview_SysElement_Functional_rekursiv(SysElement NodeType, TreeNode Parent, bool selectable)
        {
            if (NodeType.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    bool flag_Grey = false;

                    TreeNode Child = new TreeNode(NodeType.m_Child[i1].Name) { Tag = NodeType.m_Child[i1] };
                    Child.Name = Child.Text;

                    switch (this.type)
                    {
                        case -1:
                            flag_Grey = true;
                            break;
                        case 0:
                            flag_Grey = true;
                            break;
                        case 1:
                            if (NodeType.m_Child[i1].Get_m_Element_Design().Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
                        case 2:
                            if (NodeType.m_Child[i1].Get_m_Element_Functional().Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
                        case 3:
                            if (NodeType.m_Child[i1].Get_m_Element_Environment().Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
                        case 4:
                            if (NodeType.m_Child[i1].Get_m_Element_Typvertreter().Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
                        case 5:
                            if (NodeType.m_Child[i1].m_Element_Measurement.Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
                        case 6:
                            if (NodeType.m_Child[i1].m_Element_Functional.Count == 0)
                            {
                                flag_Grey = true;
                            }
                            break;
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

                    Show_Treeview_SysElement_Functional_rekursiv(NodeType.m_Child[i1], Child, selectable);

                    i1++;
                } while (i1 < NodeType.m_Child.Count);
            }

        }
        #endregion Function Baum Erstellnúng

        #region Client Tree
        private void Client_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if(this.recentTreeNode_Client != null)
            {
                this.recentTreeNode_Client.ForeColor = Color.Black;
                this.recentTreeNode_Client.BackColor = Color.White;

                if (Color.Gray == e.Node.ForeColor)
                {
                    Client_Tree.SelectedNode = e.Node.Parent;

                    e.Cancel = true;
                }
            }  
        }

        private void Client_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Festlegen gewähltes Element
            this.Reset_Supplier_Tree();
            TreeNode selectedNode = Client_Tree.SelectedNode;
            this.recentTreeNode_Client = selectedNode;

            if(selectedNode.Tag != null)
            {
                if (selectedNode.Tag.GetType() == (typeof(NodeType)))
                {
                    NodeType selectedNodeType = selectedNode.Tag as NodeType;

                    //Aktuellen Node hervorheben
                    selectedNode.ForeColor = Color.White;
                    selectedNode.BackColor = Color.Green;
                    ////////////////////////
                    //Artikel zuordnen und anzeigen
                    NodeType_Artikel_Index = this.Database.metamodel.Artikel.FindIndex(x => x == selectedNodeType.W_Artikel);
                    System_Artikel.Text = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
                    ////////////////////////
                    //Reseten Supplier Tree

                    //Befüllung des Supplier Tree
                    switch (this.type)
                    {
                        case 0:
                            break;
                        case 1:
                            this.recent_treeView = Supplier_Tree_Design;
                            this.Build_Design_Tree(selectedNodeType.m_Design.Select(x => x.OpConstraint).ToList());
                            break;
                        case 2:
                            this.recent_treeView = this.treeView_Activity;
                        //    this.Build_Activity_Tree(this.Database.m_Activity, this.recent_treeView, selectedNodeType.m_Element_Functional.Select(x => x.Supplier).ToList());
                            this.Build_Activity_Tree(this.Database.m_Activity, this.recent_treeView, selectedNodeType.Get_Process_Activity(), 0);

                            break;
                        case 3:
                            this.recent_treeView = treeView_Umwelt;
                            this.Build_Design_Tree(selectedNodeType.m_Enviromental.Select(x => x.OpConstraint).ToList());
                            break;
                        case 4:
                            this.recent_treeView = treeView_Typvertreter;
                            this.Build_Typvertreter_Tree(selectedNodeType.m_Typvertreter.Select(x => x.Typvertreter).ToList());
                            break;
                        case 5:
                            this.recent_treeView = treeView_Typvertreter;
                            this.Build_MeasurementType_Tree(selectedNodeType.m_Element_Measurement.Select(x => x.Measurement).ToList());
                            break;
                        case 6:
                            this.recent_treeView = this.treeView_Activity;
                            this.Build_Activity_Tree(this.Database.m_Activity, this.recent_treeView, selectedNodeType.Get_Process_Activity(),1 );
                            break;
                    }



                }
                if (selectedNode.Tag.GetType() == (typeof(SysElement)))
                {
                    SysElement selectedNodeType = selectedNode.Tag as SysElement;

                    //Aktuellen Node hervorheben
                    selectedNode.ForeColor = Color.White;
                    selectedNode.BackColor = Color.Green;
                    ////////////////////////
                    //Artikel zuordnen und anzeigen
                    NodeType_Artikel_Index = this.Database.metamodel.Artikel.FindIndex(x => x == selectedNodeType.W_Artikel);
                    System_Artikel.Text = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
                    ////////////////////////
                    //Reseten Supplier Tree

                    //Befüllung des Supplier Tree
                    switch (this.type)
                    {
                        case 0:
                            break;
                        case 1:
                            this.recent_treeView = Supplier_Tree_Design;
                            this.Build_Design_Tree(selectedNodeType.Get_m_Element_Design().Select(x => x.OpConstraint).ToList());
                            break;
                        case 2:
                            this.recent_treeView = this.treeView_Activity;
                           // this.Build_Activity_Tree(this.Database.m_Activity, this.recent_treeView, selectedNodeType.Get_m_Element_Functional().Select(x => x.Supplier).ToList());
                            this.Build_Activity_Tree(this.Database.m_Activity, this.recent_treeView, selectedNodeType.Get_Process_Activity(), 0);

                            break;
                        case 3:
                            this.recent_treeView = treeView_Umwelt;
                            this.Build_Design_Tree(selectedNodeType.Get_m_Element_Environment().Select(x => x.OpConstraint).ToList());
                            break;
                        case 4:
                            this.recent_treeView = treeView_Typvertreter;
                            this.Build_Typvertreter_Tree(selectedNodeType.Get_m_Element_Typvertreter().Select(x => x.Typvertreter).ToList());
                            break;
                        case 5:
                            this.recent_treeView = treeView_Typvertreter;
                            this.Build_MeasurementType_Tree(selectedNodeType.m_Element_Measurement.Select(x => x.Measurement).ToList());
                            break;
                        case 6:
                            this.recent_treeView = treeView_Activity;
                            this.Build_Activity_Tree(this.Database.m_Activity, this.recent_treeView, selectedNodeType.Get_Process_Activity(), 1);
                            break;
                    }



                }
            }

         

        }

        private void System_Artikel_Click(object sender, EventArgs e)
        {
            this.NodeType_Artikel_Index = this.NodeType_Artikel_Index+1;

            if (this.NodeType_Artikel_Index == 3)
            {
                this.NodeType_Artikel_Index = 0;
            }

            System_Artikel.Text = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
            //Refresh
            this.System_Artikel.Update();

            if(this.recentTreeNode_Supplier != null)
            {
                Get_Requirement_Text();
            }

            if(this.recentTreeNode_Client != null)
            {
                NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                recent.W_Artikel = this.System_Artikel.Text;
                ////////////////////////////////////////
                //Artikel im Classifier hinterlegen
                TaggedValue Tagged = new TaggedValue(this.Database.metamodel, this.Database);

                List<DB_Insert> m_Insert = new List<DB_Insert>();
                m_Insert.Add(new DB_Insert("SYS_Artikel", OleDbType.VarChar, OdbcType.VarChar, this.Database.metamodel.Artikel[NodeType_Artikel_Index], -1));
                recent.Update_TV(m_Insert, Database, repository);
            }
        }
        #endregion Client Tree

        #region Supplier Tree

        #region Build
        private void Build_Design_Tree(List<OperationalConstraint> opcon)
        {
            recent_treeView.Nodes.Clear();

            if (this.recentTreeNode_Client.Tag.GetType() == (typeof(NodeType)) || this.recentTreeNode_Client.Tag.GetType() == (typeof(SysElement)))
            {
                if(opcon.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        TreeNode Child = new TreeNode(opcon[i1].Name) { Tag = opcon[i1] };
                        recent_treeView.Nodes.Add(Child);

                        i1++;
                    } while (i1 < opcon.Count);
                }
                

            }
        }

        private void Build_Typvertreter_Tree(List<NodeType> opcon)
        {
            recent_treeView.Nodes.Clear();

            if (this.recentTreeNode_Client.Tag.GetType() == (typeof(NodeType)) || this.recentTreeNode_Client.Tag.GetType() == (typeof(SysElement)))
            {
                if (opcon.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        TreeNode Child = new TreeNode(opcon[i1].Name) { Tag = opcon[i1] };
                        recent_treeView.Nodes.Add(Child);

                        i1++;
                    } while (i1 < opcon.Count);
                }


            }
        }

        private void Build_MeasurementType_Tree(List<Requirement_Plugin.Repository_Elements.Measurement> opcon)
        {
            recent_treeView.Nodes.Clear();

            if (this.recentTreeNode_Client.Tag.GetType() == (typeof(NodeType)) || this.recentTreeNode_Client.Tag.GetType() == (typeof(SysElement)))
            {
                if (opcon.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        TreeNode Child = new TreeNode(opcon[i1].measurementType.Name) { Tag = opcon[i1] };
                        recent_treeView.Nodes.Add(Child);

                        i1++;
                    } while (i1 < opcon.Count);
                }

                recent_treeView.Sort();
                recent_treeView.Refresh();

            }
        }

        #endregion Build
        private void Reset_Supplier_Tree()
        {
            if(this.recent_treeView != null)
            {
                this.recent_treeView.Nodes.Clear();
                this.recent_treeView.Refresh();
            }

            ///////////////////////
            //Textfelder leeren
            this.Requirement_Name.Text = "";
            this.Requirement_Name.Refresh();
            this.Requirement_Text.Text = "";
            this.Requirement_Text.Refresh();
            this.Design_Name.Text = "";
            this.Design_Name.Refresh();
            this.Design_Text.Text = "";
            this.Design_Text.Refresh();
        }

        #region After Select
        private void Supplier_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = recent_treeView.SelectedNode;
            this.recentTreeNode_Supplier = selectedNode;
            this.Supplier_index = recent_treeView.SelectedNode.Index;

            if (selectedNode.Tag.GetType() == (typeof(OperationalConstraint)))
            {
                //Aktuellen Node hervorheben
                selectedNode.ForeColor = Color.White;
                selectedNode.BackColor = Color.Green;
                this.recent_opcon = (OperationalConstraint)recentTreeNode_Supplier.Tag;

                //OperationalConstraint Werte anzeigen
                this.Design_Name.Text = recent_opcon.Name;
                this.Design_Text.Update();
                this.Design_Text.Text = recent_opcon.Notes;
                this.Design_Text.Update();

                if(this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                    //Requirement anzeigen
                    switch (this.type)
                    {
                        case 1:
                            #region Design

                            if (recent.m_Design[Supplier_index].requirement != null)
                            {
                                this.Requirement_Name.Text = recent.m_Design[Supplier_index].requirement.Name;
                                this.Requirement_Text.Text = recent.m_Design[Supplier_index].requirement.Notes;
                                //Farbe klären
                                //Check ob alle m_Guid abgedeck 
                                this.recent_req = recent.m_Design[Supplier_index].requirement;
                                //
                                //ÄNDERN!!!!!
                                this.Check_Requirement();

                                this.Requirement_Name.Update();

                                this.Requirement_Text.Update();

                            }
                            else
                            {
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = recent.Name + " - " + this.Design_Name.Text;
                                this.Requirement_Name.Update();



                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Design(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Design[0].Stereotype);
                                recent_req = new_req;
                            }
                            #endregion Design
                            break;
                        /*    case 2:
                                recent_req.Add_Requirement_Process(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);
                                break;
                            case 3:
                                recent_req.Add_Requirement_Umwelt(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);
                                break;*/
                        case 4:
                            #region Typvertreter
                            //  NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                            if (recent.m_Typvertreter[Supplier_index].requirement != null)
                            {
                                this.Requirement_Name.Text = recent.m_Design[Supplier_index].requirement.Name;
                                this.Requirement_Text.Text = recent.m_Design[Supplier_index].requirement.Notes;
                                //Farbe klären
                                //Check ob alle m_Guid abgedeck 
                                this.recent_req = recent.m_Design[Supplier_index].requirement;
                                //
                                //ÄNDERN!!!!!
                                this.Check_Requirement();

                                this.Requirement_Name.Update();

                                this.Requirement_Text.Update();

                            }
                            else
                            {
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = recent.Name + " - " + this.Design_Name.Text;
                                this.Requirement_Name.Update();



                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Typvertreter[0].Stereotype);
                                recent_req = new_req;
                            }
                            #endregion Typvertreter
                            break;
                        case 5:
                            #region Measurement Class
                            //Finden Measurement
                            Requirement_Plugin.Repository_Elements.MeasurementType measurementType = (Requirement_Plugin.Repository_Elements.MeasurementType) recentTreeNode_Supplier.Tag;

                            List<Elements.Element_Measurement> m_help_measure =  recent.m_Element_Measurement.Where(x => x.Measurement.measurementType == measurementType).ToList();
                            #endregion 
                            break;

                    }
                }
                else
                {
                    SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;
                    //Requirement anzeigen
                    switch (this.type)
                    {
                        case 1:
                            #region Design
                            List<Elements.Element_Design> m_ret = recent.Get_m_Element_Design();

                            if (m_ret.ElementAt(Supplier_index).requirement != null)
                            {
                                this.Requirement_Name.Text = m_ret.ElementAt(Supplier_index).requirement.Name;
                                this.Requirement_Text.Text = m_ret.ElementAt(Supplier_index).requirement.Notes;
                                //Farbe klären
                                //Check ob alle m_Guid abgedeck 
                                this.recent_req = m_ret.ElementAt(Supplier_index).requirement;
                                //
                                //ÄNDERN!!!!!
                                this.Check_Requirement();

                                this.Requirement_Name.Update();

                                this.Requirement_Text.Update();

                            }
                            else
                            {
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = recent.Name + " - " + this.Design_Name.Text;
                                this.Requirement_Name.Update();



                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Design(this.requirement_Titel, this.requirement_Text, "", "", "", "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Design[0].Stereotype);
                                recent_req = new_req;
                            }
                            #endregion Design
                            break;
                        /*    case 2:
                                recent_req.Add_Requirement_Process(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);
                                break;
                            case 3:
                                recent_req.Add_Requirement_Umwelt(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);
                                break;*/
                        case 4:
                            #region Typvertreter
                            List<Elements.Element_Typvertreter> m_ret_type = recent.Get_m_Element_Typvertreter();
                            //  NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                            if (m_ret_type[Supplier_index].requirement != null)
                            {
                                this.Requirement_Name.Text = m_ret_type[Supplier_index].requirement.Name;
                                this.Requirement_Text.Text = m_ret_type[Supplier_index].requirement.Notes;
                                //Farbe klären
                                //Check ob alle m_Guid abgedeck 
                                this.recent_req = m_ret_type[Supplier_index].requirement;
                                //
                                //ÄNDERN!!!!!
                                this.Check_Requirement();

                                this.Requirement_Name.Update();

                                this.Requirement_Text.Update();

                            }
                            else
                            {
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = recent.Name + " - " + this.Design_Name.Text;
                                this.Requirement_Name.Update();



                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Typvertreter[0].Stereotype);
                                recent_req = new_req;
                            }
                            #endregion Typvertreter
                            break;
                        case 5:
                            #region Measurement Class
                            //Finden Measurement
                            Requirement_Plugin.Repository_Elements.MeasurementType measurementType = (Requirement_Plugin.Repository_Elements.MeasurementType)recentTreeNode_Supplier.Tag;

                            List<Elements.Element_Measurement> m_help_measure = recent.m_Element_Measurement.Where(x => x.Measurement.measurementType == measurementType).ToList();
                            #endregion 
                            break;



                    }
                }

                
            }
        }

        private void treeView_Typvertreter_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = recent_treeView.SelectedNode;
            this.recentTreeNode_Supplier = selectedNode;
            this.Supplier_index = recent_treeView.SelectedNode.Index;

            switch (this.type)
            {
                case 4:
                    #region Typvetreter

                    

                    if (true)
                    {
                        if (selectedNode.Tag.GetType() == (typeof(NodeType)))
                        {
                            //Aktuellen Node hervorheben
                            selectedNode.ForeColor = Color.White;
                            selectedNode.BackColor = Color.Green;
                            this.receent_typ = (NodeType)recentTreeNode_Supplier.Tag;

                            //OperationalConstraint Werte anzeigen
                            this.Design_Name.Text = receent_typ.Name;
                            this.Design_Text.Update();
                            this.Design_Text.Text = receent_typ.Notes;
                            this.Design_Text.Update();
                            //Requirement anzeigen

                            if (this.Database.metamodel.flag_sysarch == false)
                            {
                                NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                                if (recent.m_Typvertreter[Supplier_index].requirement != null)
                                {
                                    this.Requirement_Name.Text = recent.m_Typvertreter[Supplier_index].requirement.Name;
                                    this.Requirement_Text.Text = recent.m_Typvertreter[Supplier_index].requirement.Notes;
                                    //Farbe klären
                                    //Check ob alle m_Guid abgedeck 

                                    //
                                    //ÄNDERN!!!!!
                                    this.Check_Requirement_Typvertreter();

                                    this.Requirement_Name.Update();

                                    this.Requirement_Text.Update();

                                    this.recent_req = recent.m_Typvertreter[Supplier_index].requirement;

                                }
                                else
                                {
                                    this.Requirement_Color.BackColor = Color.Red;
                                    this.Requirement_Color.Update();

                                    this.Requirement_Name.Text = recent.Name + " - " + this.Design_Name.Text;
                                    this.Requirement_Name.Update();

                                    //////////
                                    //Requirement Text
                                    Get_Requirement_Text();
                                    Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                    new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Typvertreter[0].Stereotype);
                                    recent_req = new_req;
                                }
                            }
                            else
                            {
                                SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;

                                List<Elements.Element_Typvertreter> m_typevertreter = recent.Get_m_Element_Typvertreter();

                                if (m_typevertreter[Supplier_index].requirement != null)
                                {
                                    this.Requirement_Name.Text = m_typevertreter[Supplier_index].requirement.Name;
                                    this.Requirement_Text.Text = m_typevertreter[Supplier_index].requirement.Notes;
                                    //Farbe klären
                                    //Check ob alle m_Guid abgedeck 

                                    //
                                    //ÄNDERN!!!!!
                                    this.Check_Requirement_Typvertreter();

                                    this.Requirement_Name.Update();

                                    this.Requirement_Text.Update();

                                    this.recent_req = m_typevertreter[Supplier_index].requirement;

                                }
                                else
                                {
                                    this.Requirement_Color.BackColor = Color.Red;
                                    this.Requirement_Color.Update();

                                    this.Requirement_Name.Text = recent.Name + " - " + this.Design_Name.Text;
                                    this.Requirement_Name.Update();

                                    //////////
                                    //Requirement Text
                                    Get_Requirement_Text();
                                    Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                    new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Typvertreter[0].Stereotype);
                                    recent_req = new_req;
                                }
                            }

                        }
                    }

                    #endregion

                    break;
                case 5:
                    #region Measurement Class
                    //Finden Measurement
                    if (this.Database.metamodel.flag_sysarch == false)
                    {
                      //  TreeNode selectedNode = recent_treeView.SelectedNode;
                        //selectedNode = recent_treeView.SelectedNode;
                      //  this.recentTreeNode_Supplier = selectedNode;
                      //  this.Supplier_index = recent_treeView.SelectedNode.Index;

                        Requirement_Plugin.Repository_Elements.Measurement measurement = (Requirement_Plugin.Repository_Elements.Measurement)recentTreeNode_Supplier.Tag;
                        NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                        List<Elements.Element_Measurement> m_help_measure = recent.m_Element_Measurement.Where(x => x.Measurement.Classifier_ID == measurement.Classifier_ID).ToList();

                        this.recentTreeNode_Measurement = selectedNode;

                        if(m_help_measure.Count > 0)
                        {
                            this.Design_Name.Text = m_help_measure[0].Measurement.Name;
                            this.Design_Text.Text = m_help_measure[0].Measurement.Notes;

                            this.Design_Name.Refresh();
                            this.Design_Text.Refresh();


                            if(m_help_measure[0].m_requirement.Count > 0)
                            {
                                this.Requirement_Name.Text = m_help_measure[0].m_requirement[0].Name;
                                this.Requirement_Text.Text = m_help_measure[0].m_requirement[0].Notes;

                                this.Check_Requirement_Qualitaet_Class();

                                this.Requirement_Name.Update();

                                this.Requirement_Text.Update();

                                this.recent_req = m_help_measure[0].m_requirement[0];
                            }
                            else
                            {
                                //AFO Text einfügen
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = recent.Name + " - " + measurement.measurementType.Name + " von " + this.Design_Name.Text;
                                this.Requirement_Name.Update();

                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Qualitaet_Class(this.requirement_Titel, this.requirement_Text, measurement.measurementType.Name, "aufweisen", "von " + this.Design_Name.Text, "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Quality_Class[0].Stereotype);
                                recent_req = new_req;
                            }

                          
                        }
                        else
                        {
                            this.Design_Name.Text = "";
                            this.Design_Text.Text = "";

                            this.Design_Name.Refresh();
                            this.Design_Text.Refresh();
                        }

                    }
                    else
                    {
                        Requirement_Plugin.Repository_Elements.Measurement measurement = (Requirement_Plugin.Repository_Elements.Measurement)recentTreeNode_Supplier.Tag;
                        SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;
                        List<Elements.Element_Measurement> m_help_measure = recent.m_Element_Measurement.Where(x => x.Measurement.Classifier_ID == measurement.Classifier_ID).ToList();

                        this.recentTreeNode_Measurement = selectedNode;

                        if (m_help_measure.Count > 0)
                        {
                            this.Design_Name.Text = m_help_measure[0].Measurement.Name;
                            this.Design_Text.Text = m_help_measure[0].Measurement.Notes;

                            this.Design_Name.Refresh();
                            this.Design_Text.Refresh();


                            if (m_help_measure[0].m_requirement.Count > 0)
                            {
                                this.Requirement_Name.Text = m_help_measure[0].m_requirement[0].Name;
                                this.Requirement_Text.Text = m_help_measure[0].m_requirement[0].Notes;

                                this.Check_Requirement_Qualitaet_Class();

                                this.Requirement_Name.Update();

                                this.Requirement_Text.Update();

                                this.recent_req = m_help_measure[0].m_requirement[0];
                            }
                            else
                            {
                                //AFO Text einfügen
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = recent.Name + " - " + measurement.measurementType.Name + " von " + this.Design_Name.Text;
                                this.Requirement_Name.Update();

                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Qualitaet_Class(this.requirement_Titel, this.requirement_Text, measurement.measurementType.Name, "aufweisen", "von " + this.Design_Name.Text, "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Quality_Class[0].Stereotype);
                                recent_req = new_req;
                            }


                        }
                        else
                        {
                            this.Design_Name.Text = "";
                            this.Design_Text.Text = "";

                            this.Design_Name.Refresh();
                            this.Design_Text.Refresh();
                        }
                    }
                    #endregion
                    break;
            }
            
        }
        #endregion After Select

        #region BeforeSelect
        private void Supplier_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.recentTreeNode_Supplier != null)
            {
                this.recentTreeNode_Supplier.ForeColor = Color.Black;
                this.recentTreeNode_Supplier.BackColor = Color.White;

                if (Color.Gray == e.Node.ForeColor)
                {
                    treeview_Constraint_Activity. SelectedNode = e.Node.Parent;

                    e.Cancel = true;
                }
            }
        }
        #endregion Before Select
        #region Check_Requirement
        private void Check_Requirement()
        {
            Repository_Connector con = new Repository_Connector();

           
            int flag = 0; //0 --> grün; 1 --> gelb; 2 --> rot
            int count = 0;

            List<string> m_Stereotype = Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
            List<string> m_Type = Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();

            if(this.Database.metamodel.flag_sysarch == false)
            {
                NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                if (recent.m_Design[Supplier_index].requirement != null && recent.m_Design[Supplier_index].m_GUID.Count > 0)
                {
                    List<string> m_Type2 = Database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList();
                    List<string> m_Stereotype2 = Database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList();

                    int i1 = 0;
                    do
                    {
                        if (con.Check_Dependency(recent.m_Design[Supplier_index].requirement.Classifier_ID, recent.m_Design[Supplier_index].m_GUID[i1], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < recent.m_Design[Supplier_index].m_GUID.Count);

                    if (count == recent.m_Design[Supplier_index].m_GUID.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }

                }

                else
                {
                    flag = 0;
                }
            }
            else
            {
                SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;

                List<Elements.Element_Design> m_design = recent.Get_m_Element_Design();

                if (m_design[Supplier_index].requirement != null && m_design[Supplier_index].m_GUID.Count > 0)
                {
                    List<string> m_Type2 = Database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList();
                    List<string> m_Stereotype2 = Database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList();

                    int i1 = 0;
                    do
                    {
                        if (con.Check_Dependency(m_design[Supplier_index].requirement.Classifier_ID, m_design[Supplier_index].m_GUID[i1], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < m_design[Supplier_index].m_GUID.Count);

                    if (count == m_design[Supplier_index].m_GUID.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }

                }

                else
                {
                    flag = 0;
                }
            }
          


            /////////////////Einfärben
            switch(flag)
            {
                case 0:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;
                case 1:
                    this.Requirement_Color.BackColor = Color.Yellow;
                    break;
                case 2:
                    this.Requirement_Color.BackColor = Color.Green;
                    break;
                default:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;

            }
        }

        private void Check_Requirement_Typvertreter()
        {
            Repository_Connector con = new Repository_Connector();
          
            int flag = 0; //0 --> rot; 1 --> gelb; 2 --> grün
            int count = 0;

            List<string> m_Stereotype = Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
            List<string> m_Type = Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();

            if(this.Database.metamodel.flag_sysarch == false)
            {

                NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;

                if (recent.m_Typvertreter[Supplier_index].requirement != null && recent.m_Typvertreter[Supplier_index].m_GUID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (con.Check_Dependency(recent.m_Typvertreter[Supplier_index].requirement.Classifier_ID, recent.m_Typvertreter[Supplier_index].m_GUID[i1], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < recent.m_Typvertreter[Supplier_index].m_GUID.Count);

                    if (count == recent.m_Typvertreter[Supplier_index].m_GUID.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }

                }
                else
                {
                    flag = 0;
                }
            }
            else
            {

                SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;

                List<Elements.Element_Typvertreter> m_typevertreter = recent.Get_m_Element_Typvertreter();

                if (m_typevertreter[Supplier_index].requirement != null && m_typevertreter[Supplier_index].m_GUID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (con.Check_Dependency(m_typevertreter[Supplier_index].requirement.Classifier_ID, m_typevertreter[Supplier_index].m_GUID[i1], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < m_typevertreter[Supplier_index].m_GUID.Count);

                    if (count == m_typevertreter[Supplier_index].m_GUID.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }

                }
                else
                {
                    flag = 0;
                }
            }



            /////////////////Einfärben
            switch (flag)
            {
                case 0:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;
                case 1:
                    this.Requirement_Color.BackColor = Color.Yellow;
                    break;
                case 2:
                    this.Requirement_Color.BackColor = Color.Green;
                    break;
                default:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;

            }
        }


        private void Check_Requirement_Qualitaet_Class()
        {
            Repository_Connector con = new Repository_Connector();

            int flag = 0; //0 --> rot; 1 --> gelb; 2 --> grün
            int count = 0;

            List<string> m_Stereotype = Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
            List<string> m_Type = Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();

            if (this.Database.metamodel.flag_sysarch == false)
            {

                NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;

                Requirement_Plugin.Repository_Elements.Measurement recent_measure = (Requirement_Plugin.Repository_Elements.Measurement)this.recentTreeNode_Supplier.Tag;

                List<Elements.Element_Measurement> m_elem = recent.m_Element_Measurement.Where(x => x.Measurement == recent_measure).ToList();

                if(m_elem.Count > 0)
                {
                    Elements.Element_Measurement recent_elem_measure = m_elem[0];

                if (recent_elem_measure.m_requirement.Count > 0 && recent_elem_measure.m_guid_Instanzen.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            if (con.Check_Dependency(recent_elem_measure.m_requirement[0].Classifier_ID, recent_elem_measure.m_guid_Instanzen[i1], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                            {
                                count++;
                            }

                            i1++;
                        } while (i1 < recent_elem_measure.m_guid_Instanzen.Count);

                        if (count == recent_elem_measure.m_guid_Instanzen.Count)
                        {
                            flag = 0;
                        }
                        else
                        {
                            if (count == 0)
                            {
                                flag = 2;
                            }
                            else
                            {
                                flag = 1;
                            }
                        }

                    }
                    else
                    {
                        flag = 0;
                    }
                }

               
            }
            else
            {

                SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;

                Requirement_Plugin.Repository_Elements.Measurement recent_measure = (Requirement_Plugin.Repository_Elements.Measurement)this.recentTreeNode_Supplier.Tag;

                List<Elements.Element_Measurement> m_elem = recent.m_Element_Measurement.Where(x => x.Measurement == recent_measure).ToList();

                if (m_elem.Count > 0)
                {
                    Elements.Element_Measurement recent_elem_measure = m_elem[0];

                    if (recent_elem_measure.m_requirement.Count >0 && recent_elem_measure.m_guid_Instanzen.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            if (con.Check_Dependency(recent_elem_measure.m_requirement[0].Classifier_ID, recent_elem_measure.m_guid_Instanzen[i1], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                            {
                                count++;
                            }

                            i1++;
                        } while (i1 < recent_elem_measure.m_guid_Instanzen.Count);

                        if (count == recent_elem_measure.m_guid_Instanzen.Count)
                        {
                            flag = 0;
                        }
                        else
                        {
                            if (count == 0)
                            {
                                flag = 2;
                            }
                            else
                            {
                                flag = 1;
                            }
                        }

                    }
                    else
                    {
                        flag = 0;
                    }
                }


            }

            switch (flag)
            {
                case 0:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;
                case 1:
                    this.Requirement_Color.BackColor = Color.Yellow;
                    break;
                case 2:
                    this.Requirement_Color.BackColor = Color.Green;
                    break;
                default:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;

            }
        }

       
        #endregion Check_Requirment
        #endregion Supplier Tree

        #region Activity tree
        private void Activity_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.recentTreeNode_Activity != null)
            {
                this.recentTreeNode_Activity.ForeColor = Color.Black;
                this.recentTreeNode_Activity.BackColor = Color.White;

                if (Color.Gray == e.Node.ForeColor)
                {
                    treeView_Activity.SelectedNode = e.Node.Parent;

                    e.Cancel = true;
                }
            }
        }

        private void treeView_Activity_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Aktuellen Node hervorheben
            this.recentTreeNode_Activity = treeView_Activity.SelectedNode;
            treeView_Activity.SelectedNode.ForeColor = Color.White;
            treeView_Activity.SelectedNode.BackColor = Color.Green;
            //this.recent_opcon = (Activity)recentTreeNode_Activity.Tag;
            //   this.Activity_index = treeView_Activity.SelectedNode.Index;

            if(this.Database.metamodel.flag_sysarch == false)
            {
                if(this.type == 2)
                {
                    NodeType help = (NodeType)this.recentTreeNode_Client.Tag;

                    help.Get_Process_Activity();

                    //this.Activity_index = help.m_Element_Functional.FindIndex(x => x.Supplier == (Activity)this.recentTreeNode_Activity.Tag);
                    this.Activity_index = help.Get_Process_Activity().FindIndex(x => x == (Activity)this.recentTreeNode_Activity.Tag);
                }
              
            }
            else
            {
                if (this.type == 2)
                {
                    SysElement help = (SysElement)this.recentTreeNode_Client.Tag;
                    help.Get_Process_Activity();
                    //this.Activity_index = help.Get_m_Element_Functional().FindIndex(x => x.Supplier == (Activity)this.recentTreeNode_Activity.Tag);
                    this.Activity_index = help.Get_Sys_ProcessActivity().FindIndex(x => x == (Activity)this.recentTreeNode_Activity.Tag);

                }
            }
         


            if (recentTreeNode_Activity.Tag.GetType() == (typeof(Activity)))
            {
                Activity selectedActivity = recentTreeNode_Activity.Tag as Activity;

                this.recent_treeView = this.treeview_Constraint_Activity;

                if(this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType node = (NodeType)this.recentTreeNode_Client.Tag;
                    switch (this.type)
                    {
                        case 2:
                            

                            this.Build_Design_Tree(selectedActivity.Get_Process_Constraint(node));
                            break;
                        case 6:
                            
                            this.Build_MeasurementType_Tree(selectedActivity.Get_MeasurementActivity());
                            break;
                    }
                   
                }
                else
                {
                    SysElement sys = (SysElement)this.recentTreeNode_Client.Tag;
                    switch (this.type)
                    {
                        case 2:
                            this.Build_Design_Tree(selectedActivity.Get_Process_Constraint_SysElem(sys));
                            break;
                        case 6:

                            this.Build_MeasurementType_Tree(selectedActivity.Get_MeasurementActivity());
                            break;
                    }

                    

                   
                }

               // this.Build_Design_Tree(selectedActivity.m_Process.Select(x => x.OpConstraint).ToList());
            }
        }

        /*  private void Build_Activity_Tree(List<Activity> m_Activity)
          {
              recent_treeView.Nodes.Clear();

              if (this.recentTreeNode_Client.Tag.GetType() == (typeof(NodeType)))
              {
                  if (m_Activity.Count > 0)
                  {
                      int i1 = 0;
                      do
                      {
                          TreeNode Child = new TreeNode(m_Activity[i1].Name) { Tag = m_Activity[i1] };
                          recent_treeView.Nodes.Add(Child);

                          if(m_Activity[i1].m_Process.Count > 0)
                          {
                              Child.ForeColor = Color.Black;
                          }
                          else
                          {
                              Child.ForeColor = Color.Gray;
                          }

                          i1++;
                      } while (i1 < m_Activity.Count);
                  }


              }
          }*/
        private void Build_Activity_Tree(List<Activity> m_Activity, TreeView tree, List<Activity> m_Supplier, int type)
        {
            //Tree Reset
            tree.Nodes.Clear();
            //Obere Ebene nur Aktivitäten ohne Parent
            if (m_Activity.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_Activity[i1].m_Parent.Count == 0)
                    {
                        TreeNode Child = new TreeNode(m_Activity[i1].Get_Name(this.Database)) { Tag = m_Activity[i1] };
                        Child.Name = Child.Text;

                        int index = m_Supplier.FindIndex(x => x == m_Activity[i1]);

                        if (index == -1)
                        {
                            Child.ForeColor = Color.Gray;
                        }

                        switch(type)
                        {
                            case 0:
                                if (m_Activity[i1].m_Process.Count == 0)
                                {
                                    Child.ForeColor = Color.Gray;
                                }
                                break;
                            case 1:
                                if (m_Activity[i1].m_Element_Measurement_Activity.Count == 0)
                                {
                                    Child.ForeColor = Color.Gray;
                                }
                                break;
                        }

                       

                        bool flag_extend = Build_Activity_Node(m_Activity[i1].m_Child, Child, m_Supplier, type);

                        if (flag_extend == true)
                        {
                            Child.Expand();
                        }

                        tree.Nodes.Add(Child);
                    }

                    i1++;
                } while (i1 < m_Activity.Count);
            }
        }
        private bool Build_Activity_Node(List<Activity> m_Activity, TreeNode node, List<Activity> m_Supplier, int type)
        {
            bool flag_extend = false;
            //Es sind Aktivitöten vorhanden
            if (m_Activity.Count > 0)
            {
                int i1 = 0;
                do
                {

                    TreeNode Child = new TreeNode(m_Activity[i1].Get_Name(this.Database)) { Tag = m_Activity[i1] };
                    Child.Name = Child.Text;

                    int index = m_Supplier.FindIndex(x => x == m_Activity[i1]);

                    if (index == -1)
                    {
                        Child.ForeColor = Color.Gray;
                    }

                    switch(type)
                    {
                        case 0:
                            if (m_Activity[i1].m_Process.Count == 0)
                            {
                                Child.ForeColor = Color.Gray;
                            }
                            break;
                        case 1:
                            if (m_Activity[i1].m_Element_Measurement_Activity.Count == 0)
                            {
                                Child.ForeColor = Color.Gray;
                            }
                            break;
                    }
                    

                    if(Child.ForeColor != Color.Gray)
                    {
                        flag_extend = true;
                    }

                    bool flag_help = Build_Activity_Node(m_Activity[i1].m_Child, Child, m_Supplier, type);

                    if (flag_help == true)
                    {
                        flag_extend = true;
                    }

                    node.Nodes.Add(Child);

                    i1++;
                } while (i1 < m_Activity.Count);
            }

            if (flag_extend == true)
            {
                node.Expand();
            }

            return (flag_extend);
        }

        private void Reset_Activity_Tree()
        {
            if (this.treeView_Activity != null)
            {
                this.treeView_Activity.Nodes.Clear();
                this.treeView_Activity.Refresh();
            }

            ///////////////////////
            //Textfelder leeren
            this.Requirement_Name.Text = "";
            this.Requirement_Name.Refresh();
            this.Requirement_Text.Text = "";
            this.Requirement_Text.Refresh();
            this.Design_Name.Text = "";
            this.Design_Name.Refresh();
            this.Design_Text.Text = "";
            this.Design_Text.Refresh();
        }
        #endregion Activity tree

        #region Activity_Constraint
        private void Activity_Constraint_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = recent_treeView.SelectedNode;
            this.recentTreeNode_Supplier = selectedNode;
            this.Supplier_index = recent_treeView.SelectedNode.Index;

            this.recent_node = selectedNode;
            this.recentTreeNode_Measurement = selectedNode;

            switch(this.type)
            {
                case 2:
                    #region Process
                    if (selectedNode.Tag.GetType() == (typeof(OperationalConstraint)))
                    {
                        //Aktuellen Node hervorheben
                        selectedNode.ForeColor = Color.White;
                        selectedNode.BackColor = Color.Green;
                        this.recent_opcon = (OperationalConstraint)recentTreeNode_Supplier.Tag;

                        //OperationalConstraint Werte anzeigen
                        this.Design_Name.Text = recent_opcon.Name;
                        this.Design_Text.Update();
                        this.Design_Text.Text = recent_opcon.Notes;
                        this.Design_Text.Update();
                        //Requirement anzeigen

                        #region NodeType
                        if (this.Database.metamodel.flag_sysarch == false)
                        {
                            NodeType recent_node = (NodeType)this.recentTreeNode_Client.Tag;
                            Activity recent = (Activity)this.recentTreeNode_Activity.Tag;

                            List<Requirement_Non_Functional> m_req_process = recent.Element_Process_Get_Requirement(recent_node, this.recent_opcon);

                            //if (recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Count > 0)
                            if (m_req_process.Count > 0)
                            {
                                //  List<Requirement_Non_Functional> m_req = recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                                if (m_req_process.Count > 0)
                                {
                                    this.Requirement_Name.Text = m_req_process[0].AFO_TITEL;
                                    this.Requirement_Text.Text = m_req_process[0].AFO_TEXT;
                                    //Farbe klären
                                    //Check ob alle m_Guid abgedeck 
                                    this.Check_Requirement_Process();

                                    this.Requirement_Name.Refresh();
                                    this.Requirement_Text.Refresh();

                                    this.recent_req = m_req_process[0];
                                }
                                else
                                {
                                    this.Requirement_Color.BackColor = Color.Red;
                                    this.Requirement_Color.Update();

                                    this.Requirement_Name.Text = this.recentTreeNode_Client.Name + " " + recent.Name + " - " + this.Design_Name.Text;
                                    this.Requirement_Name.Refresh();


                                    //////////
                                    //Requirement Text
                                    Get_Requirement_Text();
                                    Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                    new_req.Add_Requirement_Process(this.requirement_Titel, this.requirement_Text, recent.W_Prozesswort, recent.W_Object, "", "", new_req.W_SINGULAR, recent.Name, this.Database.metamodel.m_Requirement_Process[0].Stereotype);
                                    recent_req = new_req;
                                }
                            }
                            else
                            {
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = this.recentTreeNode_Client.Name + " " + recent.Name + " - " + this.Design_Name.Text;
                                this.Requirement_Name.Refresh();

                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Process(this.requirement_Titel, this.requirement_Text, recent.W_Prozesswort, recent.W_Object, "", "", new_req.W_SINGULAR, recent.Name, this.Database.metamodel.m_Requirement_Process[0].Stereotype);
                                recent_req = new_req;
                            }
                        }
                        #endregion NodeType
                        #region SysElemente
                        else
                        {
                            SysElement recent_node = (SysElement)this.recentTreeNode_Client.Tag;
                            Activity recent = (Activity)this.recentTreeNode_Activity.Tag;
                            List<Elements.Element_Process> m_help_proc =  recent_node.m_Element_Functional.Where(x => x.Supplier.Classifier_ID == recent.Classifier_ID).ToList()[0].m_element_Processes.Where(y => y.OpConstraint.Classifier_ID == this.recent_opcon.Classifier_ID).ToList();

                            List<Requirement_Non_Functional> m_req_process = m_help_proc.Select(x => x.Requirement_Process).ToList().Where(y => y != null).ToList();
                            //List<Requirement_Non_Functional> m_req_process = recent.Element_Process_Get_Requirement(recent_node, this.recent_opcon);

                            // List<Elements.Element_Functional> m_elemfunc = recent_node.Get_m_Element_Functional();

                            //if (m_elemfunc[Activity_index].m_Requirement_Process.Count > 0)
                            if (m_req_process.Count > 0)
                            {
                                // List<Requirement_Non_Functional> m_req = m_elemfunc[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                                if (m_req_process.Count > 0)
                                {
                                    this.Requirement_Name.Text = m_req_process[0].AFO_TITEL;
                                    this.Requirement_Text.Text = m_req_process[0].AFO_TEXT;
                                    //Farbe klären
                                    //Check ob alle m_Guid abgedeck 
                                    this.Check_Requirement_Process();

                                    this.Requirement_Name.Refresh();
                                    this.Requirement_Text.Refresh();

                                    this.recent_req = m_req_process[0];
                                }
                                else
                                {
                                    this.Requirement_Color.BackColor = Color.Red;
                                    this.Requirement_Color.Update();

                                    this.Requirement_Name.Text = this.recentTreeNode_Client.Name + " " + recent.Name + " - " + this.Design_Name.Text;
                                    this.Requirement_Name.Refresh();


                                    //////////
                                    //Requirement Text
                                    Get_Requirement_Text();
                                    Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                    new_req.Add_Requirement_Process(this.requirement_Titel, this.requirement_Text, recent.W_Prozesswort, recent.W_Object, "", "", new_req.W_SINGULAR, recent.Name, this.Database.metamodel.m_Requirement_Process[0].Stereotype);
                                    recent_req = new_req;
                                }
                            }
                            else
                            {
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = this.recentTreeNode_Client.Name + " " + recent.Name + " - " + this.Design_Name.Text;
                                this.Requirement_Name.Refresh();

                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Process(this.requirement_Titel, this.requirement_Text, recent.W_Prozesswort, recent.W_Object, "", "", new_req.W_SINGULAR, recent.Name, this.Database.metamodel.m_Requirement_Process[0].Stereotype);
                                recent_req = new_req;
                            }
                        }
                        #endregion SysElement
                    }
                    #endregion
                    break;
                case 6:
                    #region Measurement
                    if (selectedNode.Tag.GetType() == (typeof(Requirement_Plugin.Repository_Elements.Measurement)))
                    {
                        
                        //Aktuellen Node hervorheben
                        selectedNode.ForeColor = Color.White;
                        selectedNode.BackColor = Color.Green;
                    //    this.recent_opcon = (OperationalConstraint)recentTreeNode_Supplier.Tag;

                        //OperationalConstraint Werte anzeigen
                       
                        //Requirement anzeigen

                        #region NodeType
                        if (this.Database.metamodel.flag_sysarch == false)
                        {
                            NodeType recent_node = (NodeType)this.recentTreeNode_Client.Tag;
                            Activity recent = (Activity)this.recentTreeNode_Activity.Tag;
                            Requirement_Plugin.Repository_Elements.Measurement recent_type = (Requirement_Plugin.Repository_Elements.Measurement)this.recent_node.Tag;

                            this.Design_Name.Text = recent_type.Name;
                            this.Design_Text.Update();
                            this.Design_Text.Text = recent_type.Notes;
                            this.Design_Text.Update();

                            List<Requirement_Non_Functional> m_help_measure = recent.Element_Measurenent_Get_Requirement(recent_node, recent_type, this.Database);

                            if (m_help_measure.Count > 0)
                            {
                                //  List<Requirement_Non_Functional> m_req = recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                                if (m_help_measure.Count > 0)
                                {
                                    this.Requirement_Name.Text = m_help_measure[0].AFO_TITEL;
                                    this.Requirement_Text.Text = m_help_measure[0].AFO_TEXT;
                                    //Farbe klären
                                    //Check ob alle m_Guid abgedeck 
                                    this.Check_Requirement_Measurement_Activity();

                                    this.Requirement_Name.Refresh();
                                    this.Requirement_Text.Refresh();

                                    this.recent_req = m_help_measure[0];
                                }
                                else
                                {
                                    this.Requirement_Color.BackColor = Color.Red;
                                    this.Requirement_Color.Update();

                                    this.Requirement_Name.Text = this.recentTreeNode_Client.Name + " " + recent.Name + " - " + this.Design_Name.Text;
                                    this.Requirement_Name.Refresh();


                                    //////////
                                    //Requirement Text
                                    Get_Requirement_Text();
                                    Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                    new_req.Add_Requirement_Qualitaet_Activity(this.requirement_Titel, this.requirement_Text, recent.W_Object, recent.W_Prozesswort, " mit einer " + recent_type.measurementType.Name + " von " + recent_type.Name, "", new_req.W_SINGULAR, recent_node.Name, this.Database.metamodel.m_Requirement_Quality_Activity[0].Stereotype);
                                    recent_req = new_req;
                                }
                            }
                            else
                            {
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = this.recentTreeNode_Client.Name + " " + recent.W_Object +" "+ recent.W_Prozesswort+ " - "+recent_type.measurementType.Name +" von " + recent_type.Name;
                                this.Requirement_Name.Refresh();

                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Qualitaet_Activity(this.requirement_Titel, this.requirement_Text, recent.W_Object,recent.W_Prozesswort, " mit einer "+ recent_type.measurementType.Name+ " von "+ recent_type.Name, "", new_req.W_SINGULAR, recent_node.Name, this.Database.metamodel.m_Requirement_Quality_Activity[0].Stereotype);
                                recent_req = new_req;
                            }
                        }
                        else
                        {
                            SysElement recent_node = (SysElement)this.recentTreeNode_Client.Tag;
                            Activity recent = (Activity)this.recentTreeNode_Activity.Tag;
                            Requirement_Plugin.Repository_Elements.Measurement recent_type = (Requirement_Plugin.Repository_Elements.Measurement)this.recent_node.Tag;

                            this.Design_Name.Text = recent_type.Name;
                            this.Design_Text.Update();
                            this.Design_Text.Text = recent_type.Notes;
                            this.Design_Text.Update();

                            List<Requirement_Non_Functional> m_help_measure = recent.Element_Measurenent_Get_Requirement(recent_node, recent_type, this.Database);

                            if (m_help_measure.Count > 0)
                            {
                                //  List<Requirement_Non_Functional> m_req = recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                                if (m_help_measure.Count > 0)
                                {
                                    this.Requirement_Name.Text = m_help_measure[0].AFO_TITEL;
                                    this.Requirement_Text.Text = m_help_measure[0].AFO_TEXT;
                                    //Farbe klären
                                    //Check ob alle m_Guid abgedeck 
                                    this.Check_Requirement_Measurement_Activity();

                                    this.Requirement_Name.Refresh();
                                    this.Requirement_Text.Refresh();

                                    this.recent_req = m_help_measure[0];
                                }
                                else
                                {
                                    this.Requirement_Color.BackColor = Color.Red;
                                    this.Requirement_Color.Update();

                                    this.Requirement_Name.Text = this.recentTreeNode_Client.Name + " " + recent.Name + " - " + this.Design_Name.Text;
                                    this.Requirement_Name.Refresh();


                                    //////////
                                    //Requirement Text
                                    Get_Requirement_Text();
                                    Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                    new_req.Add_Requirement_Qualitaet_Activity(this.requirement_Titel, this.requirement_Text, recent.W_Object, recent.W_Prozesswort, " mit einer " + recent_type.measurementType.Name + " von " + recent_type.Name, "", new_req.W_SINGULAR, recent_node.Name, this.Database.metamodel.m_Requirement_Quality_Activity[0].Stereotype);
                                    recent_req = new_req;
                                }
                            }
                            else
                            {
                                this.Requirement_Color.BackColor = Color.Red;
                                this.Requirement_Color.Update();

                                this.Requirement_Name.Text = this.recentTreeNode_Client.Name + " " + recent.W_Object + " " + recent.W_Prozesswort + " - " + recent_type.measurementType.Name + " von " + recent_type.Name;
                                this.Requirement_Name.Refresh();

                                //////////
                                //Requirement Text
                                Get_Requirement_Text();
                                Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                                new_req.Add_Requirement_Qualitaet_Activity(this.requirement_Titel, this.requirement_Text, recent.W_Object, recent.W_Prozesswort, " mit einer " + recent_type.measurementType.Name + " von " + recent_type.Name, "", new_req.W_SINGULAR, recent_node.Name, this.Database.metamodel.m_Requirement_Quality_Activity[0].Stereotype);
                                recent_req = new_req;
                            }
                        }
                        #endregion

                    }

                    #endregion
                    break;
            }

           
        }

        private void Activity_Constraint_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.recentTreeNode_Supplier != null)
            {
                this.recentTreeNode_Supplier.ForeColor = Color.Black;
                this.recentTreeNode_Supplier.BackColor = Color.White;

                if (Color.Gray == e.Node.ForeColor)
                {
                    Supplier_Tree_Design.SelectedNode = e.Node.Parent;

                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region Umwelt Tree
        private void Umwelt_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.recentTreeNode_Supplier != null)
            {
                this.recentTreeNode_Supplier.ForeColor = Color.Black;
                this.recentTreeNode_Supplier.BackColor = Color.White;

                if (Color.Gray == e.Node.ForeColor)
                {
                    treeView_Umwelt.SelectedNode = e.Node.Parent;

                    e.Cancel = true;
                }
            }
        }
        private void Umwelt_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = recent_treeView.SelectedNode;
            this.recentTreeNode_Supplier = selectedNode;
            this.Supplier_index = recent_treeView.SelectedNode.Index;

            if (selectedNode.Tag.GetType() == (typeof(OperationalConstraint)))
            {
                //Aktuellen Node hervorheben
                selectedNode.ForeColor = Color.White;
                selectedNode.BackColor = Color.Green;
                this.recent_opcon = (OperationalConstraint)recentTreeNode_Supplier.Tag;

                //OperationalConstraint Werte anzeigen
                this.Design_Name.Text = recent_opcon.Name;
                this.Design_Text.Update();
                this.Design_Text.Text = recent_opcon.Notes;
                this.Design_Text.Update();
                //Requirement anzeigen
                if(this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                    if (recent.m_Enviromental[Supplier_index].requirement != null)
                    {
                        this.Requirement_Name.Text = recent.m_Enviromental[Supplier_index].requirement.Name;
                        this.Requirement_Text.Text = recent.m_Enviromental[Supplier_index].requirement.Notes;
                        //Farbe klären
                        //Check ob alle m_Guid abgedeck 

                        //
                        //ÄNDERN!!!!!
                        this.Check_Requirement_Umwelt();

                        this.Requirement_Name.Refresh();

                        this.Requirement_Text.Refresh();

                        recent_req = recent.m_Enviromental[Supplier_index].requirement;

                    }
                    else
                    {
                        this.Requirement_Color.BackColor = Color.Red;
                        this.Requirement_Color.Update();

                        this.Requirement_Name.Text = recent.Name + " - " + this.Design_Name.Text;
                        this.Requirement_Name.Refresh();

                        //////////
                        //Requirement Text
                        Get_Requirement_Text();
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        new_req.Add_Requirement_Umwelt(this.requirement_Titel, this.requirement_Text, "", "", "", "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Environment[0].Stereotype);
                        recent_req = new_req;
                    }
                }
                else
                {
                    SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;

                    List<Elements.Element_Environmental> m_environment = recent.Get_m_Element_Environment();

                    if (m_environment.ElementAt(Supplier_index).requirement != null)
                    {
                        this.Requirement_Name.Text = m_environment[Supplier_index].requirement.Name;
                        this.Requirement_Text.Text = m_environment[Supplier_index].requirement.Notes;
                        //Farbe klären
                        //Check ob alle m_Guid abgedeck 

                        //
                        //ÄNDERN!!!!!
                        this.Check_Requirement_Umwelt();

                        this.Requirement_Name.Refresh();

                        this.Requirement_Text.Refresh();

                        recent_req = m_environment[Supplier_index].requirement;

                    }
                    else
                    {
                        this.Requirement_Color.BackColor = Color.Red;
                        this.Requirement_Color.Update();

                        this.Requirement_Name.Text = recent.Name + " - " + this.Design_Name.Text;
                        this.Requirement_Name.Refresh();

                        //////////
                        //Requirement Text
                        Get_Requirement_Text();
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        new_req.Add_Requirement_Umwelt(this.requirement_Titel, this.requirement_Text, "", "", "", "", new_req.W_SINGULAR, recent.W_Artikel + " " + recent.Name, this.Database.metamodel.m_Requirement_Environment[0].Stereotype);
                        recent_req = new_req;
                    }
                }
               
            }
        }
        #endregion Umwelt Tree
        #region menu_strip
        private void designanforderungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.type = 1;
           // this.treeView_Activity.Enabled = false;
            this.Create_Client_Tree();
            this.Reset_Supplier_Tree();
            this.Visibility_Panel();

           // recent_req.Add_Requirement_Design(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, receent_typ.W_Artikel + " " + receent_typ.Name);

            //new_req.Add_Requirement_Process();

        }

        private void qualitätsbedingungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.type = 0;
           // this.treeView_Activity.Enabled = false;
            this.Create_Client_Tree();
            this.Reset_Supplier_Tree();
            this.Visibility_Panel();
        }

        private void prozessanforderungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.type = 2;
           // this.treeView_Activity.Enabled = true;
            this.Create_Client_Tree();
            this.recent_treeView = this.treeview_Constraint_Activity;
            this.Reset_Supplier_Tree();
            this.Reset_Activity_Tree();
            this.Visibility_Panel();

            this.label3.Text = "Constraint";
            this.label3.Refresh();
        }

        private void umweltbedingungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.type = 3;
            //  this.treeView_Activity.Enabled = false;
            this.recent_treeView = treeView_Umwelt;
            this.Create_Client_Tree();
            this.Reset_Supplier_Tree();
            this.Visibility_Panel();
        }

        private void typvertreterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.type = 4;
            //  this.treeView_Activity.Enabled = false;
            this.recent_treeView = treeView_Typvertreter;
            this.Create_Client_Tree();
            this.Reset_Supplier_Tree();
            this.Visibility_Panel();

            this.label5.Text = "Typvertreter";
            this.label5.Refresh();
        }

        private void classToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.type = 5;
            //  this.treeView_Activity.Enabled = false;
            this.recent_treeView = treeView_Typvertreter;
            this.Create_Client_Tree();
            this.Reset_Supplier_Tree();
            this.Visibility_Panel();

            this.label5.Text = "MeasurementType";
            this.label5.Refresh();
        }


        private void aktivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.type = 6;
            //  this.treeView_Activity.Enabled = false;
            this.recent_treeView = this.treeview_Constraint_Activity;
            this.Create_Client_Tree();
            this.Reset_Supplier_Tree();
            this.Reset_Activity_Tree();
            this.Visibility_Panel();

            this.label3.Text = "MeasurementType";
            this.label3.Refresh();
        }

        #endregion menu_strip

        #region Requirement_Text
        private void Get_Requirement_Text()
        {
            NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
          
            string gg = this.System_Artikel.Text.Substring(1);
            string g = this.System_Artikel.Text.ElementAt(0).ToString().ToUpper();

            if(recent.W_SINGULAR == true)
            {
                this.requirement_Text = g + gg + " " + recent.Name + " muss ";
            }
            else
            {
                this.requirement_Text = g + gg + " " + recent.Name + " müssen ";
            }

           

            switch (this.type)
            {
                case 1:
                    this.requirement_Text = this.requirement_Text + "...";
                    break;
                case 2:
                    Activity recent_activity  = (Activity)this.recentTreeNode_Activity.Tag;
                    this.requirement_Text = this.requirement_Text + recent_activity.W_Object + " " + recent_activity.W_Prozesswort + " ... ";
                    break;
                case 4:
                    this.requirement_Text = this.requirement_Text+"mittels "+this.Design_Name.Text+" realisiert werden.";
                    break;
                case 5:
                    Requirement_Plugin.Repository_Elements.Measurement measuremen = (Requirement_Plugin.Repository_Elements.Measurement)this.recentTreeNode_Supplier.Tag;
                    
                    this.requirement_Text = this.requirement_Text + " eine " + measuremen.measurementType.Name +" von " +this.Design_Name.Text +" aufweisen.";
                    break;
                case 6:
                    Activity recent_activity2 = (Activity)this.recentTreeNode_Activity.Tag;
                    Requirement_Plugin.Repository_Elements.Measurement measurement = (Requirement_Plugin.Repository_Elements.Measurement)this.recentTreeNode_Supplier.Tag;

                    this.requirement_Text = this.requirement_Text + recent_activity2.W_Object +  " mit einer " + measurement.measurementType.Name + " von " + this.Design_Name.Text + " "+ recent_activity2.W_Prozesswort + ".";
                    break;
                default:
                    this.requirement_Text = this.requirement_Text + "...";
                    break;
            }


            this.Requirement_Text.Text = requirement_Text;
            this.Requirement_Text.Update();
        }
        #endregion Requirement_Text

        #region Button
        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
            repository.RefreshModelView(0);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {

           // Requirement_Non_Functional req = new Requirement_Non_Functional(null);

          /*  req.AFO_FUNKTIONAL = Ennumerationen.AFO_FUNKTIONAL.nicht_funktional;
            req.AFO_TEXT = this.requirement_Text;
            req.AFO_TITEL = this.requirement_Titel;
*/
            Form_Edit_Requirement form_Edit = new Form_Edit_Requirement(this.Database, recent_req, repository, false);

            if(this.checkBox_logical_elements.Checked == true)
            {
                this.Database.metamodel.flag_sysarch = false;
            }
            else
            {
                this.Database.metamodel.flag_sysarch = true;
            }

            if(recent_req.W_AFO_MANUAL == true)
            {
                form_Edit.Set_Satzschablone_Status(false);
            }

          

            DialogResult dialog_res = form_Edit.ShowDialog();

            if(dialog_res == DialogResult.OK)
            {
                //Abspeichern
                  switch (this.type)
                  {
                      case 1:
                          Save_Design_Requirement(recent_req);
                          break;
                      case 2:
                          Save_Process_Requirement(recent_req);
                          break;
                      case 3:
                          Save_Umwelt_Requirement(recent_req);
                          break;
                      case 4:
                          Save_Typvertreter_Requirement(recent_req);
                          break;
                    case 5:
                        this.Save_Qualitaet_Class_Requirement(recent_req);
                        break;
                    case 6:
                        this.Save_Qualitaet_Activity_Requirement(recent_req);
                        break;
                  }
                //Refreshen
                this.Requirement_Text.Text = recent_req.AFO_TEXT;
                this.Requirement_Text.Refresh();
                this.Requirement_Name.Text = recent_req.AFO_TITEL;
                this.Requirement_Name.Refresh();
            }


            //MessageBox.Show(dialog_res.ToString());






        }
        #endregion Button

        #region Eingaben
        private void Requirement_Name_TextChanged(object sender, EventArgs e)
        {
            this.requirement_Titel = this.Requirement_Name.Text;
            this.Requirement_Name.Update();
        }

        private void Requirement_Text_TextChanged(object sender, EventArgs e)
        {
            this.requirement_Text = this.Requirement_Text.Text;
            this.Requirement_Text.Update();
        }

        #endregion Eingaben

        #region Check_Requirement
        private void Check_Requirement_Process()
        {
            Repository_Connector con = new Repository_Connector();
           
            Activity recent_activity = (Activity)this.recentTreeNode_Activity.Tag;
            int flag = 0; //0 --> rot; 1 --> gelb; 2 --> grün
            int count = 0;

            List<string> m_Stereotype = Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
            List<string> m_Type = Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();

            if(Database.metamodel.flag_sysarch == false)
            {
                NodeType recent_node = (NodeType)this.recentTreeNode_Client.Tag;

                //List<Requirement_Non_Functional> m_req = recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                List<Requirement_Non_Functional> m_req = recent_activity.Element_Process_Get_Requirement(recent_node, this.recent_opcon);

                if (m_req.Count > 0) // eigentlich Länge 1 da nur eine GUID immer vorhanden
                {
                    List<string> m_Stereotype2 = Database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList();
                    List<string> m_Type2 = Database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList();
                    //Überprüfen ob alle Elemente verbunden
                    int i1 = 0;
                    do
                    {
                        if (con.Check_Dependency(m_req[0].Classifier_ID, recent_node.m_Element_Functional[Activity_index].m_Target_Functional[i1].CLient_ID, m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < recent_node.m_Element_Functional[Activity_index].m_Target_Functional.Count);
                 
                    if (count == recent_node.m_Element_Functional[Activity_index].m_Target_Functional.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }
                    //Überprüfen ob alle Activity verbunden
                    int i2 = 0;
                    if (flag == 2)
                    {
                        count = 0;
                        do
                        {
                            if (con.Check_Dependency(m_req[0].Classifier_ID, recent_node.m_Element_Functional[Activity_index].m_Target_Functional[i2].Supplier_ID, m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                            {
                                count++;
                            }

                            i2++;
                        } while (i2 < recent_node.m_Element_Functional[Activity_index].m_Target_Functional.Count);

                        if (count == recent_node.m_Element_Functional[Activity_index].m_Target_Functional.Count)
                        {
                            flag = 0;
                        }
                        else
                        {
                            if (count == 0)
                            {
                                flag = 2;
                            }
                            else
                            {
                                flag = 1;
                            }
                        }

                    }
                }
                else
                {
                    flag = 0;
                }
            }
            else
            {
                SysElement recent_node = (SysElement)this.recentTreeNode_Client.Tag;

                List<Elements.Element_Functional> m_elemfunc = recent_node.Get_m_Element_Functional();

                //List<Requirement_Non_Functional> m_req = m_elemfunc[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                List<Requirement_Non_Functional> m_req = recent_activity.Element_Process_Get_Requirement_SysElem(recent_node, this.recent_opcon);

                if (m_req.Count > 0) // eigentlich Länge 1 da nur eine GUID immer vorhanden
                {
                    List<string> m_Stereotype2 = Database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList();
                    List<string> m_Type2 = Database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList();
                    //Überprüfen ob alle Elemente verbunden
                    int i1 = 0;
                    do
                    {
                        if (con.Check_Dependency(m_req[0].Classifier_ID, m_elemfunc[Activity_index].m_Target_Functional[i1].CLient_ID, m_Stereotype2, m_Type2, Database, Database.metamodel.m_Derived_Constraint[0].direction) == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < m_elemfunc[Activity_index].m_Target_Functional.Count);
                    if (count == m_elemfunc[Activity_index].m_Target_Functional.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }
                    //Überprüfen ob alle Activity verbunden
                    int i2 = 0;

                    List<string> m_Action_ID = recent_activity.Element_Process_Get_ActionID_SysElem(recent_node, this.recent_opcon);

                    if (flag == 2)
                    {
                        count = 0;
                        do
                        {
                            if (con.Check_Dependency(m_req[0].Classifier_ID, m_Action_ID[i2], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                            {


                                count++;
                            }

                            i2++;
                        } while (i2 < m_Action_ID.Count);

                        /* do
                         {
                             if (con.Check_Dependency(m_req[0].Classifier_ID, m_elemfunc[Activity_index].m_Target_Functional[i2].Supplier_ID, m_Stereotype, m_Type, Database) == null)
                             {


                                 count++;
                             }

                             i2++;
                         } while (i2 < m_elemfunc[Activity_index].m_Target_Functional.Count);
                        */

                        if (count == m_elemfunc[Activity_index].m_Target_Functional.Count)
                        {
                            flag = 0;
                        }
                        else
                        {
                            if (count == 0)
                            {
                                flag = 2;
                            }
                            else
                            {
                                flag = 1;
                            }
                        }

                    }
                }
                else
                {
                    flag = 0;
                }
            }

          



            /////////////////Einfärben
            switch (flag)
            {
                case 0:
                    this.Requirement_Color.BackColor = Color.Red;

                    this.ToolTipp_Farbe.ToolTipTitle = m_Tooltipp_Title[4];
                    this.ToolTipp_Farbe.SetToolTip(this.Requirement_Color, m_Tooltipp_Text[4]);
                    break;
                case 1:
                    this.Requirement_Color.BackColor = Color.Yellow;
                    this.ToolTipp_Farbe.ToolTipTitle = m_Tooltipp_Title[2];
                    this.ToolTipp_Farbe.SetToolTip(this.Requirement_Color, m_Tooltipp_Text[2]);
                    break;
                case 2:
                    this.Requirement_Color.BackColor = Color.Green;
                    this.ToolTipp_Farbe.ToolTipTitle = m_Tooltipp_Title[1];
                    this.ToolTipp_Farbe.SetToolTip(this.Requirement_Color, m_Tooltipp_Text[1]);
                    break;
                default:
                    this.Requirement_Color.BackColor = Color.Red;
                    this.ToolTipp_Farbe.ToolTipTitle = m_Tooltipp_Title[4];
                    this.ToolTipp_Farbe.SetToolTip(this.Requirement_Color, m_Tooltipp_Text[4]);
                    break;

            }
        }

        private void Check_Requirement_Umwelt()
        {
            Repository_Connector con = new Repository_Connector();
           
            int flag = 0; //0 --> rot; 1 --> gelb; 2 --> grün
            int count = 0;

            List<string> m_Stereotype = Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
            List<string> m_Type = Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();

            if(this.Database.metamodel.flag_sysarch == false)
            {

                NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;

                if (recent.m_Enviromental[Supplier_index].requirement != null && recent.m_Enviromental[Supplier_index].m_GUID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (con.Check_Dependency(recent.m_Enviromental[Supplier_index].requirement.Classifier_ID, recent.m_Enviromental[Supplier_index].m_GUID[i1], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < recent.m_Enviromental[Supplier_index].m_GUID.Count);

                    if (count == recent.m_Enviromental[Supplier_index].m_GUID.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }

                }
                else
                {
                    flag = 0;
                }
            }
            else
            {

                SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;

                List<Elements.Element_Environmental> m_environment = recent.Get_m_Element_Environment();

                if( m_environment[Supplier_index].requirement != null && m_environment[Supplier_index].m_GUID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (con.Check_Dependency(m_environment[Supplier_index].requirement.Classifier_ID, m_environment[Supplier_index].m_GUID[i1], m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < m_environment[Supplier_index].m_GUID.Count);

                    if (count == m_environment[Supplier_index].m_GUID.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }

                }
                else
                {
                    flag = 0;
                }
            }


            /////////////////Einfärben
            switch (flag)
            {
                case 0:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;
                case 1:
                    this.Requirement_Color.BackColor = Color.Yellow;
                    break;
                case 2:
                    this.Requirement_Color.BackColor = Color.Green;
                    break;
                default:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;

            }
        }


        private void Check_Requirement_Measurement_Activity()
        {
            Repository_Connector con = new Repository_Connector();

            Activity recent_activity = (Activity)this.recentTreeNode_Activity.Tag;
            int flag = 0; //0 --> rot; 1 --> gelb; 2 --> grün
            int count = 0;

            List<string> m_Stereotype = Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
            List<string> m_Type = Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();

            if (Database.metamodel.flag_sysarch == false)
            {
                NodeType recent_node = (NodeType)this.recentTreeNode_Client.Tag;
                Requirement_Plugin.Repository_Elements.Measurement measurement = (Requirement_Plugin.Repository_Elements.Measurement)this.recentTreeNode_Supplier.Tag;

                //List<Requirement_Non_Functional> m_req = recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                List<Requirement_Non_Functional> m_req = recent_activity.Element_Measurenent_Get_Requirement(recent_node, measurement, this.Database);

                if (m_req.Count > 0) // eigentlich Länge 1 da nur eine GUID immer vorhanden
                {
                    List<Elements.Element_Functional> m_func = recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList();
                    //Überprüfen ob alle Elemente verbunden
                    int i1 = 0;
                    do
                    {
                        List<string> m_ret = con.Check_Dependency(m_req[0].Classifier_ID, m_func[0].m_Target_Functional[i1].CLient_ID, m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction);

                        if (m_ret == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < m_func[0].m_Target_Functional.Count);

                    if (count == m_func[0].m_Target_Functional.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }
                    //Überprüfen ob alle Activity verbunden
                    int i2 = 0;
                    if (flag == 2)
                    {
                        count = 0;
                        do
                        {
                            if (con.Check_Dependency(m_req[0].Classifier_ID, recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList()[0].m_Target_Functional[i2].Supplier_ID, m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                            {
                                count++;
                            }

                            i2++;
                        } while (i2 < recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList()[0].m_Target_Functional.Count);

                        if (count == recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList()[0].m_Target_Functional.Count)
                        {
                            flag = 0;
                        }
                        else
                        {
                            if (count == 0)
                            {
                                flag = 2;
                            }
                            else
                            {
                                flag = 1;
                            }
                        }

                    }
                }
                else
                {
                    flag = 0;
                }
            }
            else
            {
                SysElement recent_node = (SysElement)this.recentTreeNode_Client.Tag;
                Requirement_Plugin.Repository_Elements.Measurement measurement = (Requirement_Plugin.Repository_Elements.Measurement)this.recentTreeNode_Supplier.Tag;

                //List<Requirement_Non_Functional> m_req = recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                List<Requirement_Non_Functional> m_req = recent_activity.Element_Measurenent_Get_Requirement(recent_node, measurement, this.Database);

                if (m_req.Count > 0) // eigentlich Länge 1 da nur eine GUID immer vorhanden
                {
                    List<Elements.Element_Functional> m_func = recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList();
                    //recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList();
                    //Überprüfen ob alle Elemente verbunden
                    int i1 = 0;
                    do
                    {
                        List<string> m_ret = con.Check_Dependency(m_req[0].Classifier_ID, m_func[0].m_Target_Functional[i1].CLient_ID, m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction);

                        if (m_ret == null)
                        {
                            count++;
                        }

                        i1++;
                    } while (i1 < m_func[0].m_Target_Functional.Count);

                    if (count == m_func[0].m_Target_Functional.Count)
                    {
                        flag = 0;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                    }
                    //Überprüfen ob alle Activity verbunden
                    int i2 = 0;
                    if (flag == 2)
                    {


                        var help = this.recentTreeNode_Measurement;

                        count = 0;
                        do
                        {
                            if (con.Check_Dependency(m_req[0].Classifier_ID, recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList()[0].m_Target_Functional[i2].Supplier_ID, m_Stereotype, m_Type, Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                            {
                                count++;
                            }

                            i2++;
                        } while (i2 < recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList()[0].m_Target_Functional.Count);

                        if (count == recent_node.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList()[0].m_Target_Functional.Count)
                        {
                            flag = 0;
                        }
                        else
                        {
                            if (count == 0)
                            {
                                flag = 2;
                            }
                            else
                            {
                                flag = 1;
                            }
                        }

                    }
                }
                else
                {
                    flag = 0;
                }
            }


            switch (flag)
            {
                case 0:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;
                case 1:
                    this.Requirement_Color.BackColor = Color.Yellow;
                    break;
                case 2:
                    this.Requirement_Color.BackColor = Color.Green;
                    break;
                default:
                    this.Requirement_Color.BackColor = Color.Red;
                    break;

            }
        }
        #endregion Check_Requirement

        #region Save_Requirement
        private void Save_Design_Requirement(Requirement_Non_Functional requirement)
        {
            if (recentTreeNode_Client != null && recentTreeNode_Supplier != null)
            {
                Repository_Class repository_Element = new Repository_Class();
                Repository_Connector repository_Connector = new Repository_Connector();
                /////////////////////////////////
                //PAckage allg Requirement anlegen bzw. erhalten
                string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                //Package Infoübertragung anlegen bzw erhalten
                string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("DesignRequirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                //Infoübertragung Child von Requirement
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Update();

                if(this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;

                    if (recent.m_Design[Supplier_index].requirement == null)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //   new_req.Add_Requirement_Design(requirement.AFO_TITEL, requirement.AFO_TEXT, requirement.W_OBJEKT, requirement.W_PROZESSWORT, requirement.W_QUALITAET, requirement.W_RANDBEDINGUNG, true, recent.W_Artikel + " " + recent.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Design[0].Stereotype, Database);
                        recent.m_Design[Supplier_index].requirement = new_req;

                    }
                    else
                    {
                        /* recent.m_Design[Supplier_index].requirement.AFO_TITEL = requirement.AFO_TITEL;
                         recent.m_Design[Supplier_index].requirement.Name = requirement.AFO_TITEL;
                         recent.m_Design[Supplier_index].requirement.AFO_TEXT = requirement.AFO_TEXT;
                         recent.m_Design[Supplier_index].requirement.Notes = requirement.AFO_TEXT;
                         recent.m_Design[Supplier_index].requirement.W_SUBJEKT = recent.W_Artikel + " " + recent.Name;
                        */
                        recent.m_Design[Supplier_index].requirement = requirement;
                        recent.m_Design[Supplier_index].requirement.Stereotype = Database.metamodel.m_Requirement_Design[0].Stereotype;
                        recent.m_Design[Supplier_index].requirement.Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(recent.m_Design[Supplier_index].requirement.Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement --> OperationalConstraint
                    if (recent.m_Design[Supplier_index].OpConstraint != null)
                    {
                        repository_Connector.Create_Dependency(recent.m_Design[Supplier_index].requirement.Classifier_ID, recent.m_Design[Supplier_index].OpConstraint.Classifier_ID, Database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Constraint[0].SubType, repository, Database, Database.metamodel.m_Derived_Constraint[0].Toolbox, Database.metamodel.m_Derived_Constraint[0].direction);
                    }
                    //Requirment --> Class || Part
                    if (recent.m_Design[Supplier_index].m_GUID.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(recent.m_Design[Supplier_index].requirement.Classifier_ID, recent.m_Design[Supplier_index].m_GUID[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);


                            i1++;
                        } while (i1 < recent.m_Design[Supplier_index].m_GUID.Count);
                    }

                    //////////////////////////
                    //Dubletten
                    recent.m_Design[Supplier_index].Create_Connectoren_Generalisierung(repository, Database);
                }
                else
                {
                    SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;

                    List<Elements.Element_Design> m_design = recent.Get_m_Element_Design();


                    if (m_design[Supplier_index].requirement == null)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //   new_req.Add_Requirement_Design(requirement.AFO_TITEL, requirement.AFO_TEXT, requirement.W_OBJEKT, requirement.W_PROZESSWORT, requirement.W_QUALITAET, requirement.W_RANDBEDINGUNG, true, recent.W_Artikel + " " + recent.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Design[0].Stereotype, Database);
                        m_design[Supplier_index].requirement = new_req;

                    }
                    else
                    {
                        /* recent.m_Design[Supplier_index].requirement.AFO_TITEL = requirement.AFO_TITEL;
                         recent.m_Design[Supplier_index].requirement.Name = requirement.AFO_TITEL;
                         recent.m_Design[Supplier_index].requirement.AFO_TEXT = requirement.AFO_TEXT;
                         recent.m_Design[Supplier_index].requirement.Notes = requirement.AFO_TEXT;
                         recent.m_Design[Supplier_index].requirement.W_SUBJEKT = recent.W_Artikel + " " + recent.Name;
                        */
                        m_design[Supplier_index].requirement = requirement;
                        m_design[Supplier_index].requirement.Stereotype = Database.metamodel.m_Requirement_Design[0].Stereotype;
                        m_design[Supplier_index].requirement.Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(m_design[Supplier_index].requirement.Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement --> OperationalConstraint
                    if (m_design[Supplier_index].OpConstraint != null)
                    {
                        repository_Connector.Create_Dependency(m_design[Supplier_index].requirement.Classifier_ID, m_design[Supplier_index].OpConstraint.Classifier_ID, Database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Constraint[0].SubType, repository, Database, Database.metamodel.m_Derived_Constraint[0].Toolbox, Database.metamodel.m_Derived_Constraint[0].direction);
                    }
                    //Requirment --> Class || Part
                    if (m_design[Supplier_index].m_GUID.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_design[Supplier_index].requirement.Classifier_ID, m_design[Supplier_index].m_GUID[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);


                            i1++;
                        } while (i1 < m_design[Supplier_index].m_GUID.Count);
                    }
                    //Requirement --> SysElement
                    repository_Connector.Create_Dependency(m_design[Supplier_index].requirement.Classifier_ID, recent.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);

                }



                /////////////////////////
                ///Farbe festlegen
                this.Requirement_Color.BackColor = Color.Green;
                this.Requirement_Color.Update();
            }
        }

        private void Save_Process_Requirement(Requirement_Non_Functional requirement)
        {
            if (recentTreeNode_Client != null && recentTreeNode_Supplier != null && recentTreeNode_Activity != null)
            {
                Repository_Class repository_Element = new Repository_Class();
                Repository_Connector repository_Connector = new Repository_Connector();
                /////////////////////////////////
                //PAckage allg Requirement anlegen bzw. erhalten
                string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                //Package Infoübertragung anlegen bzw erhalten
                string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("ProcessRequirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                //Infoübertragung Child von Requirement
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Update();


                if(this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent_node = (NodeType)this.recentTreeNode_Client.Tag;
                    Activity recent = (Activity)this.recentTreeNode_Activity.Tag;

                    List<OperationalConstraint> m_help_constraint = recent.Get_Process_Constraint(recent_node);

                    //List<Requirement_Non_Functional> m_req = recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();
                    //Aktuelle Afo auffinden
                    List<Requirement_Non_Functional> m_req = recent.Element_Process_Get_Requirement(recent_node, m_help_constraint[Supplier_index]);

                    if (m_req.Count < 1)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //new_req.Add_Requirement_Process(this.requirement_Titel, this.requirement_Text, recent.W_Prozesswort, "", "", "", true, recent_node.W_Artikel + " " + recent_node.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Process[0].Stereotype, Database);
                        //recent.m_Process[Supplier_index].Requirement_Process = new_req;
                        new_req.m_GUID_Act.Add(recent.Classifier_ID);
                        new_req.m_GUID_OpCon.Add(recent_opcon.Classifier_ID);
                        new_req.m_GUID_Sys.Add(recent_node.Classifier_ID);

                        //recent_node.m_Element_Functional[Activity_index].m_Requirement_Process.Add(new_req);
                         recent.Element_Process_Set_Requirement(recent_node, m_help_constraint[Supplier_index], new_req);
                     //   recent.m_Process[this.Supplier_index].Requirement_Process = new_req;

                        m_req.Add(new_req);
                    }
                    else
                    {
                        int i1 = 0;
                        do
                        {
                            m_req[i1] = requirement;

                            /*   m_req[0].AFO_TITEL = requirement_Titel;
                               m_req[0].Name = requirement_Titel;
                               m_req[0].AFO_TEXT = requirement_Text;
                               m_req[0].Notes = requirement_Text;
                               m_req[0].W_SUBJEKT = recent_node.W_Artikel + " " + recent_node.Name;
                            */
                            m_req[i1].Stereotype = Database.metamodel.m_Requirement_Process[0].Stereotype;
                            m_req[i1].Update_Requirement_All(repository, Database);
                            EA.Element req_help = repository.GetElementByGuid(m_req[i1].Classifier_ID);
                            req_help.Name = m_req[i1].AFO_TITEL;
                            req_help.Notes = m_req[i1].AFO_TEXT;
                            req_help.Update();
                            i1++;
                        } while (i1 < m_req.Count);


                      
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement --> OperationalConstraint
                    //Coinstraints erhalten

                   

                    if (m_help_constraint[Supplier_index] != null)
                    {
                        repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_help_constraint[Supplier_index].Classifier_ID, Database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Constraint[0].SubType, repository, Database, Database.metamodel.m_Derived_Constraint[0].Toolbox, Database.metamodel.m_Derived_Constraint[0].direction);
                    }
                    //Requirment --> Class || Part
                    //Alle Node_Id der passenden Element_Process der Element Functional und Eleemnt User erhalten
                    List<string> m_NodeID = recent.Get_NodeID_ProcessConstraint(recent_node, m_help_constraint[Supplier_index]);
                    if(m_NodeID.Count > 0)
                    {
                        int s1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_NodeID[s1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                            s1++;
                        } while (s1 < m_NodeID.Count);
                    }
                    //Requirment --> Activity || Action
                    List<string> m_ActionID = recent.Get_ActionID_ProcessConstraint(recent_node, m_help_constraint[Supplier_index]);
                    if (m_ActionID.Count > 0)
                    {
                        int s1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_ActionID[s1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                            s1++;
                        } while (s1 < m_ActionID.Count);
                    }

                    #region Archiv

                    /*      if (recent.m_Process[Supplier_index].m_GUID_Client.Count > 0)
                          {
                              //Um das System zu erhalten muss mittels der m_Guid über die Element Functional iteriert werden
                              int s1 = 0;
                              do
                              {

                                  List<Elements.Target_Functional> help = recent_node.m_Element_Functional[Activity_index].m_Target_Functional.Where(x => x.Supplier_ID == recent.m_Process[Supplier_index].m_GUID_Client[s1]).ToList();

                                  if (help.Count > 0)
                                  {
                                      int h1 = 0;
                                      do
                                      {

                                          repository_Connector.Create_Dependency(m_req[0].Classifier_ID, help[h1].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database);

                                          h1++;
                                      } while (h1 < help.Count);
                                  }

                                  s1++;
                              } while (s1 < recent.m_Process[Supplier_index].m_GUID_Client.Count);

                          }
                    */

                    //Requirment --> Activity || Action
                    /*     if (recent.m_Process[Supplier_index].m_GUID_Client.Count > 0)
                         {
                             int i1 = 0;
                             do
                             {
                                 repository_Connector.Create_Dependency(m_req[0].Classifier_ID, recent.m_Process[Supplier_index].m_GUID_Client[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database);


                                 i1++;
                             } while (i1 < recent.m_Process[Supplier_index].m_GUID_Client.Count);

                         }
                    */
                    #endregion Archiv
                    //Requirement --> Requirement
                    if (recent_node.m_Element_Functional[Activity_index].m_Requirement_Functional.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_req[0].Classifier_ID, recent_node.m_Element_Functional[Activity_index].m_Requirement_Functional[i1].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);


                            i1++;
                        } while (i1 < recent_node.m_Element_Functional[Activity_index].m_Requirement_Functional.Count);


                    }

                    if (recent_node.m_Element_User.Count > 0)
                    {
                        List<Elements.Element_User> m_USer_all = recent_node.m_Element_User.ToList();

                        List<Elements.Element_User> m_User = m_USer_all.Where(x => x.Supplier == recent_node.m_Element_Functional[Activity_index].Supplier).ToList();


                        if (m_User.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_User[i2].m_Requirement_User.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_User[i2].m_Requirement_User[i3].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);

                                        i3++;
                                    } while (i3 < m_User[i2].m_Requirement_User.Count);
                                }

                                i2++;
                            } while (i2 < m_User.Count);


                        }
                    }

                    #region Dopplung Generalisierung

                    List<Elements.Element_Process> m_proc = recent_node.m_Element_Functional.Where(x => x.Supplier == recent).ToList().SelectMany(x => x.m_element_Processes).Where(x => x.OpConstraint == m_help_constraint[Supplier_index]).ToList();

                    if(m_proc.Count > 0)
                    {
                        m_proc[0].Create_Connectoren_Duplicate_Generalisierung(repository, Database, recent, recent_node);
                    }

                    #endregion
                }
                else
                {
                    SysElement recent_node = (SysElement)this.recentTreeNode_Client.Tag;
                    Activity recent = (Activity)this.recentTreeNode_Activity.Tag;
                   // List<Requirement_Non_Functional> m_req = recent_node.Get_m_Element_Functional()[Activity_index].m_Requirement_Process.Where(x => x.m_GUID_OpCon.Contains(recent_opcon.Classifier_ID)).ToList();

                   
                    List<OperationalConstraint> m_help_constraint = recent.Get_Process_Constraint_SysElem(recent_node);

                    List<Elements.Element_Process> m_proc = recent_node.m_Element_Functional.Where(x => x.Supplier.Classifier_ID == recent.Classifier_ID).SelectMany(y => y.m_element_Processes).Where(z => z.OpConstraint.Classifier_ID == this.recent_opcon.Classifier_ID).ToList();
                    List<Requirement_Non_Functional> m_req = m_proc.Select(x => x.Requirement_Process).Where( y => y != null).ToList();


                    //List<Requirement_Non_Functional> m_req = recent.Element_Process_Get_Requirement(recent_node, m_help_constraint[Supplier_index]);

                    if (m_req.Count < 1)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                       
                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Process[0].Stereotype, Database);
                        //recent.m_Process[Supplier_index].Requirement_Process = new_req;
                        new_req.m_GUID_Act.Add(recent.Classifier_ID);
                        new_req.m_GUID_OpCon.Add(recent_opcon.Classifier_ID);
                        new_req.m_GUID_Sys.Add(recent_node.Classifier_ID);

                        //recent_node.Get_m_Element_Functional()[Activity_index].m_Requirement_Process.Add(new_req);

                        //recent.Element_Process_Set_Requirement_SysElem(recent_node, m_help_constraint[Supplier_index], new_req);
                        //    recent.m_Process[this.Supplier_index].Requirement_Process = new_req;
                        m_proc[0].Requirement_Process = new_req;

                        m_req.Add(new_req);
                    }
                    else
                    {
                        m_req[0] = requirement;

                        m_req[0].Stereotype = Database.metamodel.m_Requirement_Process[0].Stereotype;
                        m_req[0].Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(m_req[0].Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement --> OperationalConstraint
                   // List<OperationalConstraint> m_help_constraint = recent.Get_Process_Constraint_SysElem(recent_node);

                    if (m_help_constraint[Supplier_index] != null)
                    {
                        repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_help_constraint[Supplier_index].Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                    }
                    //Requirment --> Class || Part
                    //Alle Node_Id der passenden Element_Process der Element Functional und Eleemnt User erhalten
                    List<string> m_NodeID = recent.Get_NodeID_ProcessConstraint_SysElem(recent_node, m_help_constraint[Supplier_index]);
                    if (m_NodeID.Count > 0)
                    {
                        int s1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_NodeID[s1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                            s1++;
                        } while (s1 < m_NodeID.Count);
                    }
                    //Requirment --> Activity || Action
                    List<string> m_ActionID = recent.Get_ActionID_ProcessConstraint_SysElem(recent_node, m_help_constraint[Supplier_index]);
                    if (m_ActionID.Count > 0)
                    {
                        int s1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_ActionID[s1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                            s1++;
                        } while (s1 < m_ActionID.Count);
                    }
                    #region Archiv
                    /*   if (recent.m_Process[Supplier_index].OpConstraint != null)
                       {
                           repository_Connector.Create_Dependency(m_req[0].Classifier_ID, recent.m_Process[Supplier_index].OpConstraint.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database);
                       }
                       //Requirment --> Class || Part
                       if (recent.m_Process[Supplier_index].m_GUID_Client.Count > 0)
                       {
                           //Um das System zu erhalten muss mittels der m_Guid über die Element Functional iteriert werden
                           int s1 = 0;
                           do
                           {

                               List<Elements.Target_Functional> help = recent_node.Get_m_Element_Functional()[Activity_index].m_Target_Functional.Where(x => x.Supplier_ID == recent.m_Process[Supplier_index].m_GUID_Client[s1]).ToList();

                               if (help.Count > 0)
                               {
                                   int h1 = 0;
                                   do
                                   {

                                       repository_Connector.Create_Dependency(m_req[0].Classifier_ID, help[h1].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database);

                                       h1++;
                                   } while (h1 < help.Count);
                               }

                               s1++;
                           } while (s1 < recent.m_Process[Supplier_index].m_GUID_Client.Count);

                       }


                       //Requirment --> Activity || Action
                       if (recent.m_Process[Supplier_index].m_GUID_Client.Count > 0)
                       {
                           int i1 = 0;
                           do
                           {
                               repository_Connector.Create_Dependency(m_req[0].Classifier_ID, recent.m_Process[Supplier_index].m_GUID_Client[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database);


                               i1++;
                           } while (i1 < recent.m_Process[Supplier_index].m_GUID_Client.Count);

                       }
                    */
                    #endregion Archiv
                    //Requirement --> Requirement
                    if (recent_node.Get_m_Element_Functional()[Activity_index].m_Requirement_Functional.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_req[0].Classifier_ID, recent_node.Get_m_Element_Functional()[Activity_index].m_Requirement_Functional[i1].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);


                            i1++;
                        } while (i1 < recent_node.Get_m_Element_Functional()[Activity_index].m_Requirement_Functional.Count);


                    }

                    if (recent_node.Get_m_Element_User().Count > 0)
                    {
                        List<Elements.Element_User> m_USer_all = recent_node.Get_m_Element_User().ToList();

                        List<Elements.Element_User> m_User = m_USer_all.Where(x => x.Supplier == recent_node.Get_m_Element_Functional()[Activity_index].Supplier).ToList();


                        if (m_User.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_User[i2].m_Requirement_User.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_User[i2].m_Requirement_User[i3].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);

                                        i3++;
                                    } while (i3 < m_User[i2].m_Requirement_User.Count);
                                }

                                i2++;
                            } while (i2 < m_User.Count);


                        }
                    }

                    //Requirement Syselement
                    repository_Connector.Create_Dependency(m_req[0].Classifier_ID, recent_node.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);

                }


                /////////////////////////
                ///Farbe festlegen
                this.Requirement_Color.BackColor = Color.Green;
                this.Requirement_Color.Update();
            }
        }

        private void Save_Umwelt_Requirement(Requirement_Non_Functional requirement)
        {
            if (recentTreeNode_Client != null && recentTreeNode_Supplier != null)
            {
                Repository_Class repository_Element = new Repository_Class();
                Repository_Connector repository_Connector = new Repository_Connector();
                /////////////////////////////////
                //PAckage allg Requirement anlegen bzw. erhalten
                string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                //Package Infoübertragung anlegen bzw erhalten
                string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("EnvironmentRequirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                //Infoübertragung Child von Requirement
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Update();

                if(this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;

                    if (recent.m_Enviromental[Supplier_index].requirement == null)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        // new_req.Add_Requirement_Umwelt(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Environment[0].Stereotype, Database);
                        recent.m_Enviromental[Supplier_index].requirement = new_req;

                    }
                    else
                    {
                        recent.m_Enviromental[Supplier_index].requirement = requirement;
                        recent.m_Enviromental[Supplier_index].requirement.Stereotype = Database.metamodel.m_Requirement_Environment[0].Stereotype;
                        recent.m_Enviromental[Supplier_index].requirement.Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(recent.m_Enviromental[Supplier_index].requirement.Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement --> OperationalConstraint
                    if (recent.m_Enviromental[Supplier_index].OpConstraint != null)
                    {
                        repository_Connector.Create_Dependency(recent.m_Enviromental[Supplier_index].requirement.Classifier_ID, recent.m_Enviromental[Supplier_index].OpConstraint.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                    }
                    //Requirment --> Class || Part
                    if (recent.m_Enviromental[Supplier_index].m_GUID.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(recent.m_Enviromental[Supplier_index].requirement.Classifier_ID, recent.m_Enviromental[Supplier_index].m_GUID[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);


                            i1++;
                        } while (i1 < recent.m_Enviromental[Supplier_index].m_GUID.Count);
                    }

                    //////////////////////////
                    //Dubletten
                    recent.m_Enviromental[Supplier_index].Create_Connectoren_Generalisierung(repository, Database);
                }
                else
                {
                    SysElement recent_sys = (SysElement)this.recentTreeNode_Client.Tag;

                    List<Elements.Element_Environmental> m_environment = recent_sys.Get_m_Element_Environment();


                    if (m_environment[Supplier_index].requirement == null)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        // new_req.Add_Requirement_Umwelt(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Environment[0].Stereotype, Database);
                        m_environment[Supplier_index].requirement = new_req;

                    }
                    else
                    {
                        m_environment[Supplier_index].requirement = requirement;
                        m_environment[Supplier_index].requirement.Stereotype = Database.metamodel.m_Requirement_Environment[0].Stereotype;
                        m_environment[Supplier_index].requirement.Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(m_environment[Supplier_index].requirement.Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement --> OperationalConstraint
                    if (m_environment[Supplier_index].OpConstraint != null)
                    {
                        repository_Connector.Create_Dependency(m_environment[Supplier_index].requirement.Classifier_ID, m_environment[Supplier_index].OpConstraint.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                    }
                    //Requirment --> Class || Part
                    if (m_environment[Supplier_index].m_GUID.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_environment[Supplier_index].requirement.Classifier_ID, m_environment[Supplier_index].m_GUID[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);


                            i1++;
                        } while (i1 < m_environment[Supplier_index].m_GUID.Count);
                    }
                    //Requirement Syselement
                    repository_Connector.Create_Dependency(m_environment[Supplier_index].requirement.Classifier_ID, recent_sys.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);

                }



                /////////////////////////
                ///Farbe festlegen
                this.Requirement_Color.BackColor = Color.Green;
                this.Requirement_Color.Update();
            }
        }

        private void Save_Typvertreter_Requirement(Requirement_Non_Functional requirement)
        {
            if (recentTreeNode_Client != null && recentTreeNode_Supplier != null)
            {
                Repository_Class repository_Element = new Repository_Class();
                Repository_Connector repository_Connector = new Repository_Connector();
                /////////////////////////////////
                //PAckage allg Requirement anlegen bzw. erhalten
                string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                //Package Infoübertragung anlegen bzw erhalten
                string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("TypvertreterRequirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                //Infoübertragung Child von Requirement
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Update();

                if(this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;

                    if (recent.m_Typvertreter[Supplier_index].requirement == null)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Typvertreter[0].Stereotype, Database);
                        recent.m_Typvertreter[Supplier_index].requirement = new_req;

                    }
                    else
                    {

                        recent.m_Typvertreter[Supplier_index].requirement = requirement;
                        recent.m_Typvertreter[Supplier_index].requirement.Stereotype = Database.metamodel.m_Requirement_Typvertreter[0].Stereotype;
                        recent.m_Typvertreter[Supplier_index].requirement.Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(recent.m_Typvertreter[Supplier_index].requirement.Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement --> OperationalConstraint
                    if (recent.m_Typvertreter[Supplier_index].Typvertreter != null)
                    {
                        repository_Connector.Create_Dependency(recent.m_Typvertreter[Supplier_index].requirement.Classifier_ID, recent.m_Typvertreter[Supplier_index].Typvertreter.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                    }
                    //Requirment --> Class || Part
                    if (recent.m_Typvertreter[Supplier_index].m_GUID.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(recent.m_Typvertreter[Supplier_index].requirement.Classifier_ID, recent.m_Typvertreter[Supplier_index].m_GUID[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);


                            i1++;
                        } while (i1 < recent.m_Typvertreter[Supplier_index].m_GUID.Count);
                    }

                    //////////////////////////
                    //Dubletten
                    recent.m_Typvertreter[Supplier_index].Create_Connectoren_Generalisierung(repository, Database);
                }
                else
                {
                    SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;

                    List<Elements.Element_Typvertreter> m_typevertreter = recent.Get_m_Element_Typvertreter();

                    if (m_typevertreter[Supplier_index].requirement == null)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Typvertreter[0].Stereotype, Database);
                        m_typevertreter[Supplier_index].requirement = new_req;

                    }
                    else
                    {

                        m_typevertreter[Supplier_index].requirement = requirement;
                        m_typevertreter[Supplier_index].requirement.Stereotype = Database.metamodel.m_Requirement_Typvertreter[0].Stereotype;
                        m_typevertreter[Supplier_index].requirement.Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(m_typevertreter[Supplier_index].requirement.Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement --> OperationalConstraint
                    if (m_typevertreter[Supplier_index].Typvertreter != null)
                    {
                        repository_Connector.Create_Dependency(m_typevertreter[Supplier_index].requirement.Classifier_ID, m_typevertreter[Supplier_index].Typvertreter.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                    }
                    //Requirment --> Class || Part
                    if (m_typevertreter[Supplier_index].m_GUID.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_typevertreter[Supplier_index].requirement.Classifier_ID, m_typevertreter[Supplier_index].m_GUID[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);


                            i1++;
                        } while (i1 < m_typevertreter[Supplier_index].m_GUID.Count);
                    }
                    //Requirement SysEleemt
                    repository_Connector.Create_Dependency(m_typevertreter[Supplier_index].requirement.Classifier_ID,recent.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);

                }



                /////////////////////////
                ///Farbe festlegen
                this.Requirement_Color.BackColor = Color.Green;
                this.Requirement_Color.Update();
            }
        }


        private void Save_Qualitaet_Class_Requirement(Requirement_Non_Functional requirement)
        {
            if (recentTreeNode_Client != null && recentTreeNode_Supplier != null)
            {
                Repository_Class repository_Element = new Repository_Class();
                Repository_Connector repository_Connector = new Repository_Connector();

                /////////////////////////////////
                //PAckage allg Requirement anlegen bzw. erhalten
                string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                //Package Infoübertragung anlegen bzw erhalten
                string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("QualityRequirement Class - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                //Infoübertragung Child von Requirement
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Update();

                Requirement_Plugin.Repository_Elements.Measurement measurement = (Requirement_Plugin.Repository_Elements.Measurement) recentTreeNode_Supplier.Tag;


                if (this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                    List<Elements.Element_Measurement> m_element_Measurement = recent.m_Element_Measurement.Where(x => x.Measurement == measurement).ToList();


                    if (m_element_Measurement[0].m_requirement.Count == 0)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Quality_Class[0].Stereotype, Database);
                        m_element_Measurement[0].m_requirement.Add(new_req);

                    }
                    else
                    {

                        m_element_Measurement[0].m_requirement[0] = requirement;
                        m_element_Measurement[0].m_requirement[0].Stereotype = Database.metamodel.m_Requirement_Quality_Class[0].Stereotype;
                        m_element_Measurement[0].m_requirement[0].Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(m_element_Measurement[0].m_requirement[0].Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement -->  Measurement
                    if (m_element_Measurement[0].Measurement != null)
                    {
                        repository_Connector.Create_Dependency(m_element_Measurement[0].m_requirement[0].Classifier_ID, m_element_Measurement[0].Measurement.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                    }
                    //Requirment --> Class || Part
                    if (m_element_Measurement[0].m_guid_Instanzen.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_element_Measurement[0].m_requirement[0].Classifier_ID, m_element_Measurement[0].m_guid_Instanzen[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);


                            i1++;
                        } while (i1 < m_element_Measurement[0].m_guid_Instanzen.Count);
                    }

                    //////////////////////////
                    //Dubletten
                    //Dubletten auf der selben Ebene
                    m_element_Measurement[0].Create_Connectoren_Dubletten(repository, Database, 0, recent, null);

                    //Dubletten bei Generalisierungen
                    m_element_Measurement[0].Create_Connectoren_Generalisierung(repository, Database, 0 , recent, null);


                    this.Check_Requirement_Qualitaet_Class();
                }
                else
                {
                    SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;
                    List<Elements.Element_Measurement> m_element_Measurement = recent.m_Element_Measurement.Where(x => x.Measurement == measurement).ToList();


                    if (m_element_Measurement[0].m_requirement.Count == 0)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);

                        new_req = requirement;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Quality_Class[0].Stereotype, Database);
                        m_element_Measurement[0].m_requirement.Add(new_req);

                    }
                    else
                    {

                        m_element_Measurement[0].m_requirement[0] = requirement;
                        m_element_Measurement[0].m_requirement[0].Stereotype = Database.metamodel.m_Requirement_Quality_Class[0].Stereotype;
                        m_element_Measurement[0].m_requirement[0].Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(m_element_Measurement[0].m_requirement[0].Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement -->  Measurement
                    if (m_element_Measurement[0].Measurement != null && m_element_Measurement[0].m_requirement.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_element_Measurement[0].m_requirement[i1].Classifier_ID, m_element_Measurement[0].Measurement.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                            i1++;
                        } while (i1 < m_element_Measurement[0].m_requirement.Count);

                    }
                    //Requirment --> Class || Part
                    if (m_element_Measurement[0].m_guid_Instanzen.Count > 0 && m_element_Measurement[0].m_requirement.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            int i2 = 0;
                            do
                            {
                                repository_Connector.Create_Dependency(m_element_Measurement[0].m_requirement[i2].Classifier_ID, m_element_Measurement[0].m_guid_Instanzen[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                i2++;
                            } while (i2 < m_element_Measurement[0].m_requirement.Count);

                            i1++;
                        } while (i1 < m_element_Measurement[0].m_guid_Instanzen.Count);
                    }

                    //Requirment --> SysElem
                    if (m_element_Measurement[0].m_requirement.Count > 0)
                    {
                         int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_element_Measurement[0].m_requirement[i2].Classifier_ID, recent.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);

                            i2++;
                        } while (i2 < m_element_Measurement[0].m_requirement.Count);
                    }
                    //////////////////////////
                    //Dubletten
                    //Dubletten auf der selben Ebene
                    m_element_Measurement[0].Create_Connectoren_Dubletten(repository, Database, 0, recent, null);

                    //Dubletten bei Generalisierungen
                    m_element_Measurement[0].Create_Connectoren_Generalisierung(repository, Database, 0, recent, null);


                    this.Check_Requirement_Qualitaet_Class();
                }
            }
        }

        private void Save_Qualitaet_Activity_Requirement(Requirement_Non_Functional requirement)
        {
            if (recentTreeNode_Client != null && recentTreeNode_Supplier != null)
            {
                Repository_Class repository_Element = new Repository_Class();
                Repository_Connector repository_Connector = new Repository_Connector();

                /////////////////////////////////
                //PAckage allg Requirement anlegen bzw. erhalten
                string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                //Package Infoübertragung anlegen bzw erhalten
                string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("QualityRequirement Activity - Requirement_Plugin", repository, this.Database);
                EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                //Infoübertragung Child von Requirement
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Update();

                Requirement_Plugin.Repository_Elements.Measurement measurement = (Requirement_Plugin.Repository_Elements.Measurement)recentTreeNode_Supplier.Tag;

                if (this.Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent = (NodeType)this.recentTreeNode_Client.Tag;
                    Activity recent_activity = (Activity)this.recentTreeNode_Activity.Tag;

                    //Aktuelle ElementFunctional bzw. Element_User finden
                    List<Elements.Element_Functional> m_func = recent_activity.m_Element_Functional.Where(x => x.Client == recent).ToList();

                    List<Elements.Element_Measurement> m_element_Measurement = m_func.SelectMany( y => y.m_element_measurement.Where(x => x.Measurement == measurement).ToList()).ToList();


                    if (m_element_Measurement[0].m_requirement.Count ==  0)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);
                       // new_req.Add_Requirement_Qualitaet_Activity(this.requirement_Titel, this.requirement_Text, requirement.W_OBJEKT, );


                        new_req = requirement;
                        new_req.AFO_FUNKTIONAL = Ennumerationen.AFO_FUNKTIONAL.funktional;
                       // new_req.AFO_FUNKTIONAL = Ennumerationen.AFO_FUNKTIONAL.funktional;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Quality_Activity[0].Stereotype, Database);
                        m_element_Measurement[0].m_requirement.Add(new_req);

                    }
                    else
                    {

                        m_element_Measurement[0].m_requirement[0] = requirement;
                        m_element_Measurement[0].m_requirement[0].Stereotype = Database.metamodel.m_Requirement_Quality_Activity[0].Stereotype;
                        m_element_Measurement[0].m_requirement[0].Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(m_element_Measurement[0].m_requirement[0].Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement -->  Measurement
                    if (m_element_Measurement[0].Measurement != null)
                    {
                        m_element_Measurement[0].Create_Connectoren_Measurement(Database, repository);
                    }
                    //Requirment --> Acivity || Action
                    if (m_element_Measurement[0].m_guid_Instanzen.Count > 0)
                    {
                        m_element_Measurement[0].Create_Connectoren_Instanzen(Database, repository);
                    }
                    //Requirement --> Class || Part
                    if(m_element_Measurement[0].m_guid_Instanzen.Count > 0)
                    {
                        m_element_Measurement[0].Create_Connectoren_element(Database, repository, m_func[0]);


                    }
                    ///////////////////////////////////////////////////////
                    //Refines Beziehungen
                    m_element_Measurement[0].Create_Connectoren_Refines(Database, repository, recent_activity, recent);
                    /////////////////////////////
                    //Dubletten
                    //Dubletten auf der selben Ebene
                     m_element_Measurement[0].Create_Connectoren_Dubletten(repository, Database, 1, recent, recent_activity);

                    //Dubletten bei Generalisierungen
                     m_element_Measurement[0].Create_Connectoren_Generalisierung(repository, Database, 1, recent, recent_activity);


                    this.Check_Requirement_Measurement_Activity();
                }
                else
                {
                    SysElement recent = (SysElement)this.recentTreeNode_Client.Tag;
                    Activity recent_activity = (Activity)this.recentTreeNode_Activity.Tag;

                    //Aktuelle ElementFunctional bzw. Element_User finden
                    List<Elements.Element_Functional> m_func = recent.m_Element_Functional.Where(x => x.Supplier == recent_activity).ToList();
                     //   recent_activity.m_Element_Functional.Where(x => x.Client == recent).ToList();

                    List<Elements.Element_Measurement> m_element_Measurement = m_func.SelectMany(y => y.m_element_measurement.Where(x => x.Measurement == measurement).ToList()).ToList();


                    if (m_element_Measurement[0].m_requirement.Count == 0)
                    {
                        Requirement_Non_Functional new_req = new Requirement_Non_Functional(null, this.Database.metamodel);
                        //new_req.Add_Requirement_Typvertreter(this.requirement_Titel, this.requirement_Text, "", "", "", "", true, recent.W_Artikel + " " + recent.Name);
                        // new_req.Add_Requirement_Qualitaet_Activity(this.requirement_Titel, this.requirement_Text, requirement.W_OBJEKT, );


                        new_req = requirement;
                        new_req.AFO_FUNKTIONAL = Ennumerationen.AFO_FUNKTIONAL.funktional;
                        // new_req.AFO_FUNKTIONAL = Ennumerationen.AFO_FUNKTIONAL.funktional;

                        new_req.Create_Requirement(repository, Package_Infoübertragung.PackageGUID, Database.metamodel.m_Requirement_Quality_Activity[0].Stereotype, Database);
                        m_element_Measurement[0].m_requirement.Add(new_req);

                    }
                    else
                    {

                        m_element_Measurement[0].m_requirement[0] = requirement;
                        m_element_Measurement[0].m_requirement[0].Stereotype = Database.metamodel.m_Requirement_Quality_Activity[0].Stereotype;
                        m_element_Measurement[0].m_requirement[0].Update_Requirement_All(repository, Database);
                        EA.Element req_help = repository.GetElementByGuid(m_element_Measurement[0].m_requirement[0].Classifier_ID);
                        req_help.Name = this.Requirement_Name.Text;
                        req_help.Notes = this.Requirement_Text.Text;
                        req_help.Update();
                    }
                    ////////////////
                    /////Konnektoren anlegen
                    //Requirement -->  Measurement
                    if (m_element_Measurement[0].Measurement != null)
                    {
                        m_element_Measurement[0].Create_Connectoren_Measurement(Database, repository);
                    }
                    //Requirment --> Acivity || Action
                    if (m_element_Measurement[0].m_guid_Instanzen.Count > 0)
                    {
                        m_element_Measurement[0].Create_Connectoren_Instanzen(Database, repository);
                    }
                    //Requirement --> Class || Part
                    if (m_element_Measurement[0].m_guid_Instanzen.Count > 0)
                    {
                        m_element_Measurement[0].Create_Connectoren_element(Database, repository, m_func[0]);
                    }
                    //Requirement --> SysElement
                    if (m_element_Measurement[0].m_guid_Instanzen.Count > 0 && m_element_Measurement[0].m_requirement.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(m_element_Measurement[0].m_requirement[i2].Classifier_ID, recent.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);


                            i2++;
                        } while (i2 < m_element_Measurement[0].m_requirement.Count);

                       
                    }
                    ///////////////////////////////////////////////////////
                    //Refines Beziehungen
                    m_element_Measurement[0].Create_Connectoren_Refines(Database, repository, recent_activity, recent);
                    /////////////////////////////
                    //Dubletten
                    //Dubletten auf der selben Ebene
                    m_element_Measurement[0].Create_Connectoren_Dubletten(repository, Database, 1, recent, recent_activity);

                    //Dubletten bei Generalisierungen
                    m_element_Measurement[0].Create_Connectoren_Generalisierung(repository, Database, 1, recent, recent_activity);


                    this.Check_Requirement_Measurement_Activity();
                }
            }
        }
        #endregion Save Requirement

        private void Requirement_Color_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox_logical_elements_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBox_logical_elements.Checked == true)
            {
                this.Database.metamodel.flag_sysarch = false;

                //Supplier etc löschen
                this.Client_Tree.Nodes.Clear();
                if(recent_treeView != null)
                {
                    this.recent_treeView.Nodes.Clear();
                }
               
                //Client neu aufbauen
                this.Create_Client_Tree();
            }
            else
            {
                this.Database.metamodel.flag_sysarch = true;

                //Supplier etc löschen
                this.Client_Tree.Nodes.Clear();
                if(recent_treeView != null)
                {
                    this.recent_treeView.Nodes.Clear();

                }

         
                //Client neu aufbauen
                this.Create_Client_Tree();
            }
        }

        private void qualitätsanforderungToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Data.OleDb;
using System.Data.Odbc;

using Database_Connection;
using Repsoitory_Elements;
using Elements;
using Requirements;

namespace Forms
{

    public partial class Functional_Decomposition : Form
    {


        #region Attribute From
        Requirement_Plugin.Database Database;
        EA.Repository Repository;
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        //////////////////
        int flag_functional = -1; //-1 keine Auswahl, 0 muss, 1 muss dem Nutzer die Möglichkeit bieten
        bool flag_Prozesswort = false;
        bool flag_Objcet = false;
        bool flag_Qualitaet = false;
        bool flag_Randbedingung = false;
        bool flag_reset = false;
        /////////////////////////
        //string für die Textboxen
        string Text_Prozesswort_ini = "<Prozesswort>";
        string Text_Prozesswort_sel = "!Bitte Prozesswort einfügen!";
        string Text_Object_ini = "<Objekt>";
        string Text_Object_sel = "!Bitte Objekt einfügen!";
        string Text_Qualitaet_ini = "<Qualität>";
        string Text_Qualitaet_sel = "!Bei Bedarf Qualität einfügen!";
        string Text_Randbedingung_ini = "<Randbedingung>";
        string Text_Randbedingung_sel = "!Bei Bedarf Randbedingung einfügen!";
        ///////////////////////
        //AFO
        string AFO_Text = "";
        int Pos_Qualitaet = -1;
        ////////////////
        ///Merker für aktuellen Client Node und Supplier Node
        private TreeNode recentTreeNode_Client;
        private TreeNode recentTreeNode_Activity;
        private Element_User recent_Element_User;
        private Stakeholder recent_Stakeholder;
        private string recent_Stakeholder_String;
        private string recent_Stakeholder_Artikel_string;
        private int Stakeholder_Artikel_Index;
        ////////////////
        ///Artikel Client und Supplier
        public int NodeType_Artikel_Index = 0;
        public int Target_Artikel_Index = 0;

        bool new_req = false;

        private Requirement_Functional recent_req_functional = new Requirement_Functional("", "", "", "", "", "", false, "", false, null, null);
        private Requirement_User recent_req_user = new Requirement_User("", "", "", "", "", "", false, "", false, "", null, null);


        #region Tooltips
        List<string> m_Tooltipp_Title = new List<string>();
        List<string> m_Tooltipp_Text= new List<string>();
        

        #endregion Tooltips
        #endregion Attribute From
        #region Initialisierung & Laden Form
        public Functional_Decomposition(Requirement_Plugin.Database Database, EA.Repository Repository)
        {
            this.Database = Database;
            this.Repository = Repository;
            this.recentTreeNode_Client = null;
            this.recentTreeNode_Activity = null;
            this.recent_Stakeholder = null;
            this.recent_Stakeholder_String = this.Database.metamodel.Stakeholder_Default;
            this.Stakeholder_Artikel_Index = 0;
            this.recent_Stakeholder_Artikel_string = this.Database.metamodel.Artikel[Stakeholder_Artikel_Index + 6];
            
            InitializeComponent();

            //Sortieren der Ebenen nach Alphabet
            Client_Tree.Sort();
            Supplier_Tree.Sort();

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


            this.Tooltippp_Farbe.ToolTipTitle = m_Tooltipp_Title[0];
            this.Tooltippp_Farbe.SetToolTip(this.pictureBox_Farbe, m_Tooltipp_Text[0]);

            if(this.Database.metamodel.flag_sysarch == true)
            {
                this.checkBox_logical_elements.Checked = false;
            }
            else
            {
                this.checkBox_logical_elements.Checked = true;
            }

        }

        //Beim LAden des Forms 
        private void Functional_Decomposition_Load(object sender, EventArgs e)
        {
            if (Database.Decomposition != null)
            {
                /////////////////////////////////////////////////////////////////
                //Client
                this.Create_Client_Tree();

            
                ///////////
                ///Zuordnung Fähigkeitsbaum
                ///
                if (this.Database.m_Capability.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        ComboBoxItem_Capability recent_item = new ComboBoxItem_Capability(this.Database.m_Capability[i2].Get_Name(this.Database), this.Database.m_Capability[i2]);
                        this.Auswahl_Capability.Items.Add(recent_item);

                        i2++;
                    } while (i2 < this.Database.m_Capability.Count);

                }
                Stakeholder_Artikel.Enabled = false;
                Stakeholder_Artikel.Refresh();
            }
           
        }
        #endregion Initialisierung & Laden Form

        #region Function Baum Erstellnúng
        private void Create_Client_Tree()
        {

            this.Client_Tree.Nodes.Clear();
            //Ersten Knoten schaffen des Cleint Tree schaffen
            TreeNode Ebene0 = new TreeNode("Client");// { Tag = Database.Decomposition.Classifier_ID };
            Ebene0.Name = Ebene0.Text;
            recentTreeNode_Client = Ebene0;
            //Ebene 0 hinzufügen
            Client_Tree.Nodes.Add(Ebene0);
            //Komplette Decomposition anlegen

            if (this.Database.metamodel.flag_sysarch == false)
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

                        if (Database.Decomposition.m_NodeType[i1].m_Element_Functional.Count == 0 && Database.Decomposition.m_NodeType[i1].m_Element_User.Count == 0)
                        {
                            flag_Grey = true;
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
            else
            {
                List<SysElement> Decompositoin = new List<SysElement>();
                Decompositoin = this.Database.m_SysElemente.Where(x => x.m_Parent.Count == 0).ToList();


                if (Decompositoin.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        bool flag_Grey = false;

                        TreeNode Child = new TreeNode(Decompositoin[i1].Get_Name(this.Database)) { Tag = Decompositoin[i1] };
                        Child.Name = Child.Text;
                        Ebene0.Nodes.Add(Child);

                        List<Element_Functional> m_elemfunc = Decompositoin[i1].Get_m_Element_Functional();
                        List<Element_User> m_elemuser = Decompositoin[i1].Get_m_Element_User();

                        if (m_elemfunc.Count == 0 && m_elemuser.Count == 0)
                        {
                            flag_Grey = true;
                        }

                        if (flag_Grey == true)
                        {
                            Child.ForeColor = Color.Gray;
                        }

                        Show_Treeview_NodeType_Functional_rekursiv(Decompositoin[i1], Child, true);


                        i1++;
                    } while (i1 < Decompositoin.Count);
                }
            }
        }


        /// <summary>
        /// Befüllen eines Baumes
        /// </summary>
        /// <param name="NodeType"></param>
        /// <param name="Parent"></param>
        /// <param name="selectable"></param>
        private void Show_Treeview_NodeType_Functional_rekursiv(Repository_Class classType, TreeNode Parent, bool selectable)
        {
            if(this.Database.metamodel.flag_sysarch == false)
            {
                NodeType NodeType = (NodeType)classType;

                if (NodeType.m_Child.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        bool flag_Grey = false;

                        TreeNode Child = new TreeNode(NodeType.m_Child[i1].Get_Name(this.Database)) { Tag = NodeType.m_Child[i1] };
                        Child.Name = Child.Text;

                        if (NodeType.m_Child[i1].m_Element_Functional.Count == 0 && NodeType.m_Child[i1].m_Element_User.Count == 0)
                        {
                            flag_Grey = true;
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
            else
            {
                SysElement sys_elem = (SysElement)classType;

                if (sys_elem.m_Child.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        bool flag_Grey = false;

                        TreeNode Child = new TreeNode(sys_elem.m_Child[i1].Get_Name(this.Database)) { Tag = sys_elem.m_Child[i1] };
                        Child.Name = Child.Text;

                        List<Element_Functional> m_elemfunc = sys_elem.m_Child[i1].Get_m_Element_Functional();
                        List<Element_User> m_elemuser = sys_elem.m_Child[i1].Get_m_Element_User();

                        if (m_elemfunc.Count == 0 && m_elemuser.Count == 0)
                        {
                            flag_Grey = true;
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

                        Show_Treeview_NodeType_Functional_rekursiv(sys_elem.m_Child[i1], Child, selectable);

                        i1++;
                    } while (i1 < sys_elem.m_Child.Count);
                }
            }

          

        }
        #endregion Function Baum Erstellnúng

        #region Afo_Erstellung
        private void Create_Text_Afo_ini()
        {
            TaggedValue taggedValue = new TaggedValue(this.Database.metamodel, this.Database);
            Pos_Qualitaet = -1;

            flag_reset = true;
            Text_Afo.Text = "";
            Text_Afo.Refresh();

            if (recentTreeNode_Client != null && recentTreeNode_Activity != null)
            {

                if (recentTreeNode_Client.Tag.GetType() == (typeof(NodeType)))
                {
                    Activity recent_Activity = null;

                    if (recentTreeNode_Activity.Tag.GetType() == (typeof(Activity)))
                    {
                        recent_Activity = recentTreeNode_Activity.Tag as Activity;
                    }
                    if (recentTreeNode_Activity.Tag.GetType() == (typeof(Element_User)))
                    {
                        var help = recentTreeNode_Activity.Tag as Element_User;
                        recent_Activity = help.Supplier;
                    }
                    NodeType recent_NodeType = recentTreeNode_Client.Tag as NodeType;

                    ///////////////////
                    //Aktivity
                    //  var w_prozesswort = taggedValue.Get_Tagged_Value(recent_Activity.Classifier_ID, "W_PROZESSWORT", this.Repository);
                    //  var w_object = taggedValue.Get_Tagged_Value(recent_Activity.Classifier_ID, "W_OBJEKT", this.Repository);
                    var w_prozesswort = recent_Activity.W_Prozesswort;
                    var w_object = recent_Activity.W_Object;

                    ///////////////
                    //W_Qualitaet und W_Randebedingung prüfen
                   

                    string w_qualitaet = "";
                    string w_randbedingung = "";

                    if(flag_functional == 0)
                    {
                        Element_Functional Check = recent_NodeType.Check_Element_Functional(recent_NodeType, recent_Activity);
                        if (Check != null)
                        {
                            if (Check.m_Requirement_Functional.Count > 0)
                            {
                                /*  w_prozesswort = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_PROZESSWORT", this.Repository);
                                  w_object = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_OBJEKT", this.Repository);
                                  w_qualitaet = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_QUALITAET", this.Repository);
                                  w_randbedingung = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_RANDBEDINGUNG", this.Repository);*/
                                w_prozesswort = Check.m_Requirement_Functional[0].W_PROZESSWORT;
                                w_object = Check.m_Requirement_Functional[0].W_OBJEKT;
                                w_qualitaet = Check.m_Requirement_Functional[0].W_QUALITAET;
                                w_randbedingung = Check.m_Requirement_Functional[0].W_RANDBEDINGUNG;

                                if (w_prozesswort == null)
                                {
                                    w_prozesswort = "";
                                }
                                if (w_object == null)
                                {
                                    w_object = "";
                                }
                                if (w_qualitaet == null)
                                {
                                    w_qualitaet = "";
                                }
                                if (w_randbedingung == null)
                                {
                                    w_randbedingung = "";
                                }
                            }
                        }
                    }
                    else
                    {
                        List<Element_User> m_all = recent_NodeType.m_Element_User;

                        List<Element_User> m_element_user = m_all.Where(x => x.Client == recent_NodeType && x.Supplier == recent_Activity).ToList();

                        if (m_element_user[0].m_Requirement_User.Count > 0)
                        {
                            /*  w_prozesswort = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_PROZESSWORT", this.Repository);
                              w_object = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_OBJEKT", this.Repository);
                              w_qualitaet = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_QUALITAET", this.Repository);
                              w_randbedingung = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_RANDBEDINGUNG", this.Repository);*/
                            w_prozesswort = m_element_user[0].m_Requirement_User[0].W_PROZESSWORT;
                            w_object = m_element_user[0].m_Requirement_User[0].W_OBJEKT;
                            w_qualitaet = m_element_user[0].m_Requirement_User[0].W_QUALITAET;
                            w_randbedingung = m_element_user[0].m_Requirement_User[0].W_RANDBEDINGUNG;

                            recent_Stakeholder_Artikel_string = "dem";
                            recent_Stakeholder_String = "Nutzer";

                            if (w_prozesswort == null)
                            {
                                w_prozesswort = "";
                            }
                            if (w_object == null)
                            {
                                w_object = "";
                            }
                            if (w_qualitaet == null)
                            {
                                w_qualitaet = "";
                            }
                            if (w_randbedingung == null)
                            {
                                w_randbedingung = "";
                            }
                        }
                    }

                    ////////////
                    //Label
                    //label_Afo.Text = recent_NodeType.Get_Name_t_object_GUID(this.Repository, this.Database) + " - ";
                    //label_Afo.Text = label_Afo.Text + recent_Activity.Get_Name_t_object_GUID(this.Repository, this.Database);
                    label_Afo.Text = recent_NodeType.Name + " - ";
                    label_Afo.Text = label_Afo.Text + recent_Activity.Name;
                    ///////////////////////////////////
                    //AFO_Text
                    AFO_Text = "";

                    if (w_randbedingung != "")
                    {
                        AFO_Text = w_randbedingung + " ";
                        switch (flag_functional)
                        {
                            case -1:
                                AFO_Text = AFO_Text + " ...";
                                break;
                            case 0:
                                // AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Get_Name_t_object_GUID(this.Repository, this.Database) + " ";
                                AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Name + " ";
                                break;
                            case 1:
                             //   AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Get_Name_t_object_GUID(this.Repository, this.Database) + " " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                                AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Name+ " " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                                break;
                        }
                    }
                    else
                    {
                        //AFO_Text = ti.ToTitleCase(System_Artikel.Text) + " " + recent_NodeType.Get_Name_t_object_GUID(this.Repository, this.Database);
                        AFO_Text = ti.ToTitleCase(System_Artikel.Text) + " " + recent_NodeType.Name;

                        switch (flag_functional)
                        {
                            case -1:
                                AFO_Text = AFO_Text + " ...";
                                break;
                            case 0:
                                AFO_Text = AFO_Text + " muss ";
                                break;
                            case 1:
                                AFO_Text = AFO_Text + " muss " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                                break;
                        }
                    }


                    switch (flag_functional)
                    {
                        case -1:
                            AFO_Text = AFO_Text + " ...";
                            break;
                        case 0:
                            if (w_prozesswort != null && w_object != null)
                            {
                                if (w_qualitaet != "")
                                {
                                    AFO_Text = AFO_Text + w_object + " " + w_qualitaet + " " + w_prozesswort + ".";
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + w_object + " " + w_prozesswort + ".";
                                }
                            }
                            break;
                        case 1:
                            if (w_prozesswort != null && w_object != null)
                            {
                                if (w_qualitaet != "")
                                {
                                    AFO_Text = AFO_Text + w_object + " " + w_qualitaet + " zu " + w_prozesswort + ".";
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + w_object + " zu " + w_prozesswort + ".";
                                }
                            }
                            break;
                    }

                    if (w_qualitaet != Text_Qualitaet_ini && w_qualitaet != Text_Qualitaet_sel)
                    {
                        Change_Pos_Qualitaet(w_qualitaet);
                    }

                }
                if (recentTreeNode_Client.Tag.GetType() == (typeof(SysElement)))
                {
                    Activity recent_Activity = null;

                    if (recentTreeNode_Activity.Tag.GetType() == (typeof(Activity)))
                    {
                        recent_Activity = recentTreeNode_Activity.Tag as Activity;
                    }
                    if (recentTreeNode_Activity.Tag.GetType() == (typeof(Element_User)))
                    {
                        var help = recentTreeNode_Activity.Tag as Element_User;
                        recent_Activity = help.Supplier;
                    }
                    SysElement recent_NodeType = recentTreeNode_Client.Tag as SysElement;

                    ///////////////////
                    //Aktivity
                    //  var w_prozesswort = taggedValue.Get_Tagged_Value(recent_Activity.Classifier_ID, "W_PROZESSWORT", this.Repository);
                    //  var w_object = taggedValue.Get_Tagged_Value(recent_Activity.Classifier_ID, "W_OBJEKT", this.Repository);
                    var w_prozesswort = recent_Activity.W_Prozesswort;
                    var w_object = recent_Activity.W_Object;

                    ///////////////
                    //W_Qualitaet und W_Randebedingung prüfen


                    string w_qualitaet = "";
                    string w_randbedingung = "";

                    if (flag_functional == 0)
                    {
                        List<NodeType> m_recent_Client = new List<NodeType>();
                        m_recent_Client = recent_NodeType.m_Implements;

                        int c1 = 0;
                        do
                        {
                            Element_Functional Check = m_recent_Client[c1].Check_Element_Functional(m_recent_Client[c1], recent_Activity);
                            if (Check != null)
                            {
                                if (Check.m_Requirement_Functional.Count > 0)
                                {
                                    /*  w_prozesswort = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_PROZESSWORT", this.Repository);
                                      w_object = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_OBJEKT", this.Repository);
                                      w_qualitaet = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_QUALITAET", this.Repository);
                                      w_randbedingung = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_RANDBEDINGUNG", this.Repository);*/
                                    w_prozesswort = Check.m_Requirement_Functional[0].W_PROZESSWORT;
                                    w_object = Check.m_Requirement_Functional[0].W_OBJEKT;
                                    w_qualitaet = Check.m_Requirement_Functional[0].W_QUALITAET;
                                    w_randbedingung = Check.m_Requirement_Functional[0].W_RANDBEDINGUNG;

                                    if (w_prozesswort == null)
                                    {
                                        w_prozesswort = "";
                                    }
                                    if (w_object == null)
                                    {
                                        w_object = "";
                                    }
                                    if (w_qualitaet == null)
                                    {
                                        w_qualitaet = "";
                                    }
                                    if (w_randbedingung == null)
                                    {
                                        w_randbedingung = "";
                                    }
                                }
                            }
                            c1++;
                        } while (c1 < m_recent_Client.Count);

                       
                    }
                    else
                    {
                        List<Element_User> m_all = recent_NodeType.Get_m_Element_User();

                        List<Element_User> m_element_user = m_all.Where(x => x.Client.Classifier_ID == recent_NodeType.Classifier_ID && x.Supplier == recent_Activity).ToList();

                        if(m_element_user.Count > 0)
                        {
                            if (m_element_user[0].m_Requirement_User.Count > 0)
                            {
                                /*  w_prozesswort = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_PROZESSWORT", this.Repository);
                                  w_object = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_OBJEKT", this.Repository);
                                  w_qualitaet = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_QUALITAET", this.Repository);
                                  w_randbedingung = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_RANDBEDINGUNG", this.Repository);*/
                                w_prozesswort = m_element_user[0].m_Requirement_User[0].W_PROZESSWORT;
                                w_object = m_element_user[0].m_Requirement_User[0].W_OBJEKT;
                                w_qualitaet = m_element_user[0].m_Requirement_User[0].W_QUALITAET;
                                w_randbedingung = m_element_user[0].m_Requirement_User[0].W_RANDBEDINGUNG;

                                recent_Stakeholder_Artikel_string = "dem";
                                recent_Stakeholder_String = "Nutzer";

                                if (w_prozesswort == null)
                                {
                                    w_prozesswort = "";
                                }
                                if (w_object == null)
                                {
                                    w_object = "";
                                }
                                if (w_qualitaet == null)
                                {
                                    w_qualitaet = "";
                                }
                                if (w_randbedingung == null)
                                {
                                    w_randbedingung = "";
                                }
                            }
                        }

                      
                    }

                    ////////////
                    //Label
                    //label_Afo.Text = recent_NodeType.Get_Name_t_object_GUID(this.Repository, this.Database) + " - ";
                    //label_Afo.Text = label_Afo.Text + recent_Activity.Get_Name_t_object_GUID(this.Repository, this.Database);
                    label_Afo.Text = recent_NodeType.Name + " - ";
                    label_Afo.Text = label_Afo.Text + recent_Activity.Name;
                    ///////////////////////////////////
                    //AFO_Text
                    AFO_Text = "";

                    if (w_randbedingung != "")
                    {
                        AFO_Text = w_randbedingung + " ";
                        switch (flag_functional)
                        {
                            case -1:
                                AFO_Text = AFO_Text + " ...";
                                break;
                            case 0:
                                // AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Get_Name_t_object_GUID(this.Repository, this.Database) + " ";
                                AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Name + " ";
                                break;
                            case 1:
                                //   AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Get_Name_t_object_GUID(this.Repository, this.Database) + " " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                                AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Name + " " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                                break;
                        }
                    }
                    else
                    {
                        //AFO_Text = ti.ToTitleCase(System_Artikel.Text) + " " + recent_NodeType.Get_Name_t_object_GUID(this.Repository, this.Database);
                        AFO_Text = ti.ToTitleCase(System_Artikel.Text) + " " + recent_NodeType.Name;

                        switch (flag_functional)
                        {
                            case -1:
                                AFO_Text = AFO_Text + " ...";
                                break;
                            case 0:
                                AFO_Text = AFO_Text + " muss ";
                                break;
                            case 1:
                                AFO_Text = AFO_Text + " muss " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                                break;
                        }
                    }


                    switch (flag_functional)
                    {
                        case -1:
                            AFO_Text = AFO_Text + " ...";
                            break;
                        case 0:
                            if (w_prozesswort != null && w_object != null)
                            {
                                if (w_qualitaet != "")
                                {
                                    AFO_Text = AFO_Text + w_object + " " + w_qualitaet + " " + w_prozesswort + ".";
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + w_object + " " + w_prozesswort + ".";
                                }
                            }
                            break;
                        case 1:
                            if (w_prozesswort != null && w_object != null)
                            {
                                if (w_qualitaet != "")
                                {
                                    AFO_Text = AFO_Text + w_object + " " + w_qualitaet + " zu " + w_prozesswort + ".";
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + w_object + " zu " + w_prozesswort + ".";
                                }
                            }
                            break;
                    }

                    if (w_qualitaet != Text_Qualitaet_ini && w_qualitaet != Text_Qualitaet_sel)
                    {
                        Change_Pos_Qualitaet(w_qualitaet);
                    }

                }


                Text_Afo.Text = AFO_Text;
                Text_Afo.Refresh();
                label_Afo.Refresh();

                //Check_For_Requirement();

            }

            flag_reset = false;
        }

        private void Create_Text_Afo_change()
        {
            TaggedValue taggedValue = new TaggedValue(this.Database.metamodel, this.Database);

            flag_reset = true;
            Text_Afo.Text = "";
            Text_Afo.Refresh();

            if (recentTreeNode_Client != null && recentTreeNode_Activity != null)
            {
                if(recentTreeNode_Client.Tag != null)
                {
                    if (recentTreeNode_Client.Tag.GetType() == (typeof(NodeType)))
                    {
                        Activity recent_Activity = null;

                        if (recentTreeNode_Activity.Tag.GetType() == (typeof(Activity)))
                        {
                            recent_Activity = recentTreeNode_Activity.Tag as Activity;
                        }
                        if (recentTreeNode_Activity.Tag.GetType() == (typeof(Element_User)))
                        {
                            var help = recentTreeNode_Activity.Tag as Element_User;
                            recent_Activity = help.Supplier;
                        }
                        NodeType recent_NodeType = recentTreeNode_Client.Tag as NodeType;

                        ///////////////////
                        //Aktivity
                        // var w_prozesswort = taggedValue.Get_Tagged_Value(recent_Activity.Classifier_ID, "W_PROZESSWORT", this.Repository);
                        // var w_object = taggedValue.Get_Tagged_Value(recent_Activity.Classifier_ID, "W_OBJEKT", this.Repository);

                        ///////////////
                        //W_Qualitaet und W_Randebedingung prüfen
                        //Element_Functional Check = recent_NodeType.Check_Element_Functional(recent_NodeType, recent_Activity);

                        string w_prozesswort = "";
                        string w_object = "";
                        string w_qualitaet = "";
                        string w_randbedingung = "";


                        if (Text_Prozesswort.Text != Text_Prozesswort_ini && Text_Prozesswort.Text != Text_Prozesswort_sel)
                        {
                            w_prozesswort = Text_Prozesswort.Text;
                        }
                        if (Text_Object.Text != Text_Object_ini && Text_Object.Text != Text_Object_sel)
                        {
                            w_object = Text_Object.Text;
                        }
                        if (Text_Qualitaet.Text != Text_Qualitaet_ini && Text_Qualitaet.Text != Text_Qualitaet_sel)
                        {
                            w_qualitaet = Text_Qualitaet.Text;
                        }
                        if (Text_Randbedingung.Text != Text_Randbedingung_ini && Text_Randbedingung.Text != Text_Randbedingung_sel)
                        {
                            w_randbedingung = Text_Randbedingung.Text;
                        }
                        /*
                        if (Check != null)
                        {
                            if (Check.m_Requirement_Functional.Count > 0)
                            {
                                w_qualitaet = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_QUALITAET", this.Repository);
                                w_randbedingung = taggedValue.Get_Tagged_Value(Check.m_Requirement_Functional[0].Classifier_ID, "W_RANDBEDINGUNG", this.Repository);

                                if (w_qualitaet == null)
                                {
                                    w_qualitaet = "";
                                }
                                if (w_randbedingung == null)
                                {
                                    w_randbedingung = "";
                                }

                            }
                        }
                        */
                        ///////////////////////////////////
                        if (flag_Prozesswort == true)
                        {
                            w_prozesswort = Text_Prozesswort.Text;
                            flag_Prozesswort = false;
                        }
                        if (flag_Objcet == true)
                        {
                            w_object = Text_Object.Text;
                            flag_Objcet = false;
                        }
                        if (flag_Qualitaet == true)
                        {
                            if (Text_Qualitaet.Text != Text_Qualitaet_sel && Text_Qualitaet.Text != Text_Qualitaet_ini)
                            {
                                w_qualitaet = Text_Qualitaet.Text;
                            }
                            else
                            {
                                w_qualitaet = "";
                            }

                            flag_Qualitaet = false;
                        }
                        if (flag_Randbedingung == true)
                        {
                            w_randbedingung = Text_Randbedingung.Text;
                            flag_Randbedingung = false;
                        }

                        ////////////
                        //Label
                        label_Afo.Text = recent_NodeType.Get_Name(this.Database) + " - ";
                        label_Afo.Text = label_Afo.Text + recent_Activity.Get_Name(this.Database);
                        ///////////////////////////////////
                        //AFO_Text
                        AFO_Text = "";

                        if (w_randbedingung != "")
                        {
                            AFO_Text = w_randbedingung + " ";
                            switch (flag_functional)
                            {
                                case -1:
                                    AFO_Text = AFO_Text + " ...";
                                    break;
                                case 0:
                                    AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Get_Name(this.Database) + " ";
                                    break;
                                case 1:
                                    AFO_Text = AFO_Text + "muss " + System_Artikel.Text + " " + recent_NodeType.Get_Name(this.Database) + " " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                                    break;
                            }
                        }
                        else
                        {
                            AFO_Text = ti.ToTitleCase(System_Artikel.Text) + " " + recent_NodeType.Get_Name(this.Database);
                            switch (flag_functional)
                            {
                                case -1:
                                    AFO_Text = AFO_Text + " ...";
                                    break;
                                case 0:
                                    AFO_Text = AFO_Text + " muss ";
                                    break;
                                case 1:
                                    AFO_Text = AFO_Text + " muss " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                                    break;
                            }
                        }

                        switch (flag_functional)
                        {
                            case -1:
                                AFO_Text = AFO_Text + " ...";
                                break;
                            case 0:
                                if (w_prozesswort != null && w_object != null)
                                {
                                    if (w_qualitaet != "")
                                    {
                                        AFO_Text = AFO_Text + w_object + " " + w_qualitaet + " " + w_prozesswort + ".";
                                    }
                                    else
                                    {
                                        AFO_Text = AFO_Text + w_object + " " + w_prozesswort + ".";
                                    }
                                }
                                break;
                            case 1:
                                if (w_prozesswort != null && w_object != null)
                                {
                                    if (w_qualitaet != "")
                                    {
                                        AFO_Text = AFO_Text + w_object + " " + w_qualitaet + " zu " + w_prozesswort + ".";
                                    }
                                    else
                                    {
                                        AFO_Text = AFO_Text + w_object + " zu " + w_prozesswort + ".";
                                    }
                                }
                                break;
                        }

                        if (w_qualitaet != Text_Qualitaet_ini && w_qualitaet != Text_Qualitaet_sel)
                        {
                            Change_Pos_Qualitaet(w_qualitaet);
                        }

                    }
                    Text_Afo.Text = AFO_Text;
                    Text_Afo.Refresh();
                    label_Afo.Refresh();
                }

               

               // Check_For_Requirement();

            }

            flag_reset = false;

            /*
            if (recentTreeNode_Client.Tag.GetType() == (typeof(NodeType)))
            {
                NodeType selectedNodeType = this.recentTreeNode_Client.Tag as NodeType;
                label_Afo.Text = selectedNodeType.Get_Name_t_object_GUID(this.Repository) + " - ";
                ///////////////////////
                //Afo Text
                ////////////////////////////////
                ///Wenn Randebedingung ist dies der erste Schritt
                if (flag_Randbedingung == true)
                {
                    AFO_Text = Text_Randbedingung.Text + ", ";
                    switch (flag_functional)
                    {
                        case -1:
                            AFO_Text = AFO_Text + " ...";
                            break;
                        case 0:
                            AFO_Text = AFO_Text + " muss " + System_Artikel.Text + " " + selectedNodeType.Get_Name_t_object_GUID(this.Repository);
                            break;
                        case 1:
                            AFO_Text = AFO_Text + " muss "+System_Artikel.Text+" " + selectedNodeType.Get_Name_t_object_GUID(this.Repository)+" " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                            break;
                    }
                }
                else
                {
                    ///////////////////////
                    ///System
                    AFO_Text = ti.ToTitleCase(System_Artikel.Text) + " " + selectedNodeType.Get_Name_t_object_GUID(this.Repository);
                    switch (flag_functional)
                    {
                        case -1:
                            AFO_Text = AFO_Text + " ...";
                            break;
                        case 0:
                            AFO_Text = AFO_Text + " muss ";
                            break;
                        case 1:
                            AFO_Text = AFO_Text + " muss " + recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String + " die Möglichkeit bieten, ";
                            break;
                    }
                }

                //////////////////////
                ///Activity
                if (recentTreeNode_Activity != null)
                {
                    if (recentTreeNode_Activity.Tag.GetType() == (typeof(Activity)))
                    {
                        Activity selectedActivity = this.recentTreeNode_Activity.Tag as Activity;
                        label_Afo.Text = label_Afo.Text + selectedActivity.Get_Name_t_object_GUID(this.Repository);
                        TaggedValue taggedValue = new TaggedValue();
                        /////////////////
                        //Prozesswort
                        var w_prozesswort = taggedValue.Get_Tagged_Value(selectedActivity.Classifier_ID, "W_PROZESSWORT", this.Repository);
                        if(flag_Prozesswort == true)
                        {
                            w_prozesswort = Text_Prozesswort.Text;
                        }
                        //Object
                        var w_object = taggedValue.Get_Tagged_Value(selectedActivity.Classifier_ID, "W_OBJECT", this.Repository);
                        if (flag_Objcet == true)
                        {
                            w_object = Text_Object.Text;
                        }
                        switch (flag_functional)
                        {
                            case -1:
                                AFO_Text = AFO_Text + " ...";
                                break;
                            case 0:
                                if (w_object != null)
                                {
                                    AFO_Text = AFO_Text + w_object +" ";
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + "... ";
                                }
                                if(flag_Qualitaet == true && Pos_Qualitaet == -1)
                                {
                                    AFO_Text = AFO_Text + Text_Qualitaet.Text + " ";
                                }
                                if(w_prozesswort != null)
                                {
                                    AFO_Text = AFO_Text + w_prozesswort + ".";
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + "... .";
                                }
                                break;
                            case 1:
                                if (w_object != null)
                                {
                                    AFO_Text = AFO_Text + w_object + " ";
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + "... ";
                                }
                                if (flag_Qualitaet == true && Pos_Qualitaet == -1)
                                {
                                    AFO_Text = AFO_Text + Text_Qualitaet.Text + " ";
                                }
                                if (w_prozesswort != null)
                                {
                                    AFO_Text = AFO_Text +"zu "+ w_prozesswort + ".";
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + "zu ... .";
                                }
                                break;
                        }
                    }
                }
                else
                {
                    AFO_Text = AFO_Text + " ...";
                }

                if (Pos_Qualitaet != -1)
                {
                    var AFO_Text_split = AFO_Text.Split(' ');

                    AFO_Text = "";
                    if (AFO_Text_split.Length > 0)
                    {
                        bool flag_lauf = false;
                        int i1 = 0;
                        do
                        {
                            if (i1 != Pos_Qualitaet || flag_lauf == true)
                            {
                                if(i1 == 0)
                                {
                                    AFO_Text = AFO_Text_split[i1];
                                }
                                else
                                {
                                    AFO_Text = AFO_Text+" " +AFO_Text_split[i1];
                                }
                            }
                            if(i1 == Pos_Qualitaet && flag_lauf == false)
                            {

                                if (i1 == 0)
                                {
                                    AFO_Text = Text_Qualitaet.Text;
                                }
                                else
                                {
                                    AFO_Text = AFO_Text + " " + Text_Qualitaet.Text;
                                }
                                flag_lauf = true;

                                i1--;
                            }

                            i1++;
                        } while (i1 < AFO_Text_split.Length);
                    }
                }
                //Text in TExtbox updaten
                Text_Afo.Text = AFO_Text;
                Text_Afo.Refresh();
                label_Afo.Refresh();

                //Check_For_Requirement();
                
            }
            */
        }

        private void Check_For_Requirement()
        {
            bool flag_both = false;

            if (recentTreeNode_Client != null && recentTreeNode_Activity != null)
            {
                NodeType recent_NodeType = null;
                Activity recent_Activity = null;

                int color = 0; //

                if (recentTreeNode_Client.Tag.GetType() == (typeof(NodeType)))
                {
                    recent_NodeType = recentTreeNode_Client.Tag as NodeType;
                }
                if (recentTreeNode_Client.Tag.GetType() == (typeof(SysElement)))
                {
                    recent_NodeType = recentTreeNode_Client.Tag as SysElement;
                }
                if (flag_functional == 0)
                {
                    if (recentTreeNode_Activity.Tag.GetType() == (typeof(Activity)))
                    {
                        recent_Activity = recentTreeNode_Activity.Tag as Activity;
                    }
                }
                if(flag_functional == 1)
                {
                    if (recentTreeNode_Activity.Tag.GetType() == (typeof(Activity)))
                    {
                        recent_Activity = recentTreeNode_Activity.Tag as Activity;
                      //  recent_Actiity = recent_Element_User.Supplier;
                      //  recent_Element_User
                    }
                }

                if (recent_NodeType != null && recent_Activity != null)
                {
                    #region Element Functional
                    if(flag_functional == 0)
                    {
                        Element_Functional Check = recent_NodeType.Check_Element_Functional(recent_NodeType, recent_Activity);

                        List<Element_User> m_User = recent_NodeType.m_Element_User;

                        List<Element_User> m_element_user = m_User.Where(x => x.Client == recent_NodeType && x.Supplier == recent_Activity).ToList();

                        if(m_element_user.Count > 0)
                        {
                            if(m_element_user[0].m_Requirement_User.Count > 0)
                            {
                                flag_both = true;
                            }
                        }

                        if (Check != null)
                        {
                            if (Check.m_Requirement_Functional.Count > 0)
                            {
                                //vergleichen
                                if (Check.m_Requirement_Functional[0].AFO_TEXT == AFO_Text)
                                {
                                    //Wenn jetzt auch noch die Dependencies stimmen
                                    Repository_Connector repository_Connector = new Repository_Connector();

                                    if (Check.m_Target_Functional.Count > 0)
                                    {
                                        bool flag_Targets = true;

                                        int i1 = 0;
                                        do
                                        {
                                            if (repository_Connector.Check_Dependency(Check.m_Requirement_Functional[0].Classifier_ID, Check.m_Target_Functional[i1].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) == null || repository_Connector.Check_Dependency(Check.m_Requirement_Functional[0].Classifier_ID, Check.m_Target_Functional[i1].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                                            {
                                                flag_Targets = false;
                                            }

                                            i1++;
                                        } while (i1 < Check.m_Target_Functional.Count && flag_Targets == true);

                                        if (flag_Targets == true)
                                        {
                                            color = 2;
                                            //splitContainer6.Panel1.BackColor = Color.Green;

                                        }
                                        else
                                        {
                                            color = 1;
                                            //splitContainer6.Panel1.BackColor = Color.Yellow;

                                        }

                                    }
                                    else
                                    {
                                        if (repository_Connector.Check_Dependency(Check.m_Requirement_Functional[0].Classifier_ID, recent_NodeType.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) != null && repository_Connector.Check_Dependency(Check.m_Requirement_Functional[0].Classifier_ID, recent_NodeType.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) != null)
                                        {
                                            // splitContainer6.Panel1.BackColor = Color.Green;
                                            color = 2;
                                        }
                                        else
                                        {
                                            //splitContainer6.Panel1.BackColor = Color.Yellow;
                                            color = 1;
                                        }
                                    }
                                }
                                else
                                {
                                    //splitContainer6.Panel1.BackColor = Color.Yellow;
                                    color = 1;
                                }
                            }
                            else
                            {
                                //nicht vorhanden --> einfärben
                                //splitContainer6.Panel1.BackColor = Color.Red;
                                color = 0;
                            }
                        }
                    }
                    #endregion Element Functional

                    #region Element User
                    if (flag_functional == 1)
                    {
                        Element_User Check = recent_Element_User;

                        List<Element_Functional> m_Functional = recent_NodeType.m_Element_Functional;

                        List<Element_Functional> m_element_functional = m_Functional.Where(x => x.Client == recent_NodeType && x.Supplier == recent_Activity).ToList();

                        if (m_element_functional.Count > 0)
                        {
                            if (m_element_functional[0].m_Requirement_Functional.Count > 0)
                            {
                                flag_both = true;
                            }
                        }

                        color = 0;

                        if (Check != null)
                        {
                            if (Check.m_Requirement_User.Count > 0)
                            {
                                int i2 = 0;
                                do
                                {

                                    if (Check.m_Requirement_User[i2].AFO_TEXT == AFO_Text)
                                    {
                                        //Wenn jetzt auch noch die Dependencies stimmen
                                        Repository_Connector repository_Connector = new Repository_Connector();

                                        if (Check.m_Target_User.Count > 0)
                                        {
                                            bool flag_Targets = true;

                                            int i1 = 0;
                                            do
                                            {
                                                if (repository_Connector.Check_Dependency(Check.m_Requirement_User[i2].Classifier_ID, Check.m_Target_User[i1].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) == null || repository_Connector.Check_Dependency(Check.m_Requirement_User[i2].Classifier_ID, Check.m_Target_User[i1].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) == null || repository_Connector.Check_Dependency(Check.m_Requirement_User[i2].Classifier_ID, Check.m_Target_User[i1].Client_ST, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                                                {
                                                    flag_Targets = false;
                                                }

                                                i1++;
                                            } while (i1 < Check.m_Target_User.Count && flag_Targets == true);

                                            if (flag_Targets == true) // Text und Connectoren vohanden
                                            {
                                                color = 2;
                                                //splitContainer6.Panel1.BackColor = Color.Green;

                                            }
                                            else
                                            {
                                                color = 1; // Text aber Connectoren nicht alle vorhanden
                                                //splitContainer6.Panel1.BackColor = Color.Yellow;

                                            }

                                        }
                                        else
                                        {
                                            if (repository_Connector.Check_Dependency(Check.m_Requirement_User[i2].Classifier_ID, recent_NodeType.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) != null && repository_Connector.Check_Dependency(Check.m_Requirement_User[i2].Classifier_ID, recent_NodeType.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) != null)
                                            {
                                                if(Check.m_Client_ST.Count > 0)
                                                {
                                                    bool flag_ST = true;
                                                    int i3 = 0;
                                                    do
                                                    {
                                                        if(repository_Connector.Check_Dependency(Check.m_Requirement_User[i2].Classifier_ID, Check.m_Client_ST[i3].Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database, Database.metamodel.m_Derived_Element[0].direction) == null)
                                                        {
                                                            flag_ST = false;
                                                        }
                                                        i3++;
                                                    } while (i3 < Check.m_Client_ST.Count && flag_ST == true);

                                                    if (flag_ST == true)
                                                    {
                                                        color = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    color = 1;
                                                }
                                                // splitContainer6.Panel1.BackColor = Color.Green;
                                              
                                            }
                                            else
                                            {
                                                //splitContainer6.Panel1.BackColor = Color.Yellow;
                                                color = 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //splitContainer6.Panel1.BackColor = Color.Yellow;
                                        color = 1;
                                    }

                                    i2++;
                                } while (i2 < Check.m_Requirement_User.Count && color == 0);
                                //vergleichen
                               
                            }
                            else
                            {
                                //nicht vorhanden --> einfärben
                                //splitContainer6.Panel1.BackColor = Color.Red;
                                color = 0;
                            }
                        }
                    }
                    #endregion Element User
                }

                switch (color)
                {
                    case 0:
                        if (flag_both == true)
                        {
                            pictureBox_Farbe.BackColor = Color.Blue;

                            this.Tooltippp_Farbe.ToolTipTitle = m_Tooltipp_Title[3];
                            this.Tooltippp_Farbe.SetToolTip(this.pictureBox_Farbe, m_Tooltipp_Text[3]);
                        }
                        else
                        {
                            pictureBox_Farbe.BackColor = Color.Red;
                            this.Tooltippp_Farbe.ToolTipTitle = m_Tooltipp_Title[4];
                            this.Tooltippp_Farbe.SetToolTip(this.pictureBox_Farbe, m_Tooltipp_Text[4]);
                        }
                       
                        break;
                    case 1:

                        if(flag_both == true)
                        {
                            pictureBox_Farbe.BackColor = Color.Blue;

                            this.Tooltippp_Farbe.ToolTipTitle = m_Tooltipp_Title[3];
                            this.Tooltippp_Farbe.SetToolTip(this.pictureBox_Farbe, m_Tooltipp_Text[3]);
                        }
                        else
                        {
                            pictureBox_Farbe.BackColor = Color.Yellow;

                            this.Tooltippp_Farbe.ToolTipTitle = m_Tooltipp_Title[2];
                            this.Tooltippp_Farbe.SetToolTip(this.pictureBox_Farbe, m_Tooltipp_Text[2]);
                        }
                       
                        break;
                    case 2:
                        pictureBox_Farbe.BackColor = Color.Green;
                        this.Tooltippp_Farbe.ToolTipTitle = m_Tooltipp_Title[1];
                        this.Tooltippp_Farbe.SetToolTip(this.pictureBox_Farbe, m_Tooltipp_Text[1]);
                        break;
                }

                pictureBox_Farbe.Refresh();
            }


        }
        #endregion Afo_Erstellung
        #region Client Tree Auswahl

        private void Client_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            this.recentTreeNode_Client.ForeColor = Color.Black;
            this.recentTreeNode_Client.BackColor = Color.White;

            if (Color.Gray == e.Node.ForeColor)
            {
                Client_Tree.SelectedNode = e.Node.Parent;

                e.Cancel = true;
            }
        }

        private void Client_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            flag_reset = true;
            //Festlegen gewähltes Element
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
                    /////////////////////////////////
                    //Supplier_Tree neu anzeigen
                    Reset_Supplier();
                    Show_Supplier();
                    ////////////////////////
                    //Artikel zuordnen und anzeigen
                    NodeType_Artikel_Index = this.Database.metamodel.Artikel.FindIndex(x => x == selectedNodeType.W_Artikel);
                    System_Artikel.Text = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
                    ////////////////////////
                }
                if(selectedNode.Tag.GetType() == (typeof(SysElement)))
                {
                    SysElement selectedNodeType = selectedNode.Tag as SysElement;

                    //Aktuellen Node hervorheben
                    selectedNode.ForeColor = Color.White;
                    selectedNode.BackColor = Color.Green;
                    /////////////////////////////////
                    //Supplier_Tree neu anzeigen
                    Reset_Supplier();
                    Show_Supplier();
                    ////////////////////////
                    //Artikel zuordnen und anzeigen
                    NodeType_Artikel_Index = this.Database.metamodel.Artikel.FindIndex(x => x == selectedNodeType.W_Artikel);
                    System_Artikel.Text = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
                }
            }

            
            flag_reset = false;
            //AFO Text
            //Create_Text_Afo_ini();
            Create_Text_Afo_change();
        }

        #endregion Client Tree Auswahl
     
        #region Client Artikel
        private void System_Artikel_Click(object sender, EventArgs e)
        {
            bool flag = true;
            List<DB_Insert> m_Insert = new List<DB_Insert>();

            if (Client_Tree.SelectedNode == null || Client_Tree.SelectedNode.Level == 0)
            {
                flag = false;
                System_Artikel.Text = "...";
            }



            switch (NodeType_Artikel_Index)
            {
                case 0:
                    System_Artikel.Text = this.Database.metamodel.Artikel[1];
                    NodeType_Artikel_Index++;
                    break;
                case 1:
                    System_Artikel.Text = this.Database.metamodel.Artikel[2];
                    NodeType_Artikel_Index++;
                    break;
                case 2:
                    System_Artikel.Text = this.Database.metamodel.Artikel[0];
                    NodeType_Artikel_Index = 0;
                    break;
            }
            if (flag == true)
            {
                NodeType recent = Client_Tree.SelectedNode.Tag as NodeType;
                ////
                //Artikel abspeichern für alle mit diesem NodeType
                if (this.Database.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (recent.Classifier_ID == this.Database.m_NodeType[i1].Classifier_ID)
                        {
                            this.Database.m_NodeType[i1].W_Artikel = this.Database.metamodel.Artikel[NodeType_Artikel_Index];
                        }

                        i1++;
                    } while (i1 < this.Database.m_NodeType.Count);

                }
                /////////////////////////////////////////
                //Artikel in AFO Text neu schreiben
                /*   recent_Text_Empfangen[6] = this.Database.metamodel.Artikel[NodeType_Artikel_Index + 6];
                   recent_Text_Senden[0] = this.Database.metamodel.Artikel[NodeType_Artikel_Index];

                   this.Database.Write_List(Text_Senden, recent_Text_Senden);
                   this.Database.Write_List(Text_Empfangen, recent_Text_Empfangen);
                   */
                Create_Text_Afo_change();
                ////////////////////////////////////////
                //Artikel im Classifier hinterlegen
                TaggedValue Tagged = new TaggedValue(this.Database.metamodel, this.Database);

                m_Insert.Clear();
                m_Insert.Add(new DB_Insert(  "SYS_Artikel", OleDbType.VarChar, OdbcType.VarChar, this.Database.metamodel.Artikel[NodeType_Artikel_Index], -1));

                recent.Update_TV(m_Insert, Database, Repository);

               // Tagged.Update_Tagged_Value(recent.Classifier_ID, "SYS_Artikel", this.Database.metamodel.Artikel[NodeType_Artikel_Index], "Values: der, die, das", Repository);
            }
        }
        #endregion Client Artikel

        #region Auswahl Functional
        private void Auswahl_Functional_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Auswahl_Functional.SelectedIndex == 0)
            {
                flag_functional = 0;
                Auswahl_Stakeholder.Enabled = false;

                Auswahl_Stakeholder.Items.Clear();
                Auswahl_Stakeholder.Refresh();
            }
            else
            {
                flag_functional = 1;
                Auswahl_Stakeholder.Enabled = true;
            }
            //Supplier_Tree neu anzeigen
            Show_Supplier();
            Reset_Supplier();
            //AFo Text neu generieren
            Create_Text_Afo_ini();
        }
        #endregion Auswahl Functional

        #region Supplier Tree
        private void Show_Supplier()
        {
            //Alle Nodes entfernen
            Supplier_Tree.Nodes.Clear();

            if (recentTreeNode_Client != null && flag_functional != -1&& recentTreeNode_Client.Level != 0) //Client und Auswahl Functional eine Wahl getroffen
            {
                if (Database.metamodel.flag_sysarch == false)
                {
                    NodeType recent_Client = Client_Tree.SelectedNode.Tag as NodeType;

                    if (recent_Client != null)
                    {
                        List<Activity> m_Supplier = new List<Activity>();

                        if (flag_functional == 0)
                        {
                            m_Supplier = recent_Client.m_Element_Functional.Select(x => x.Supplier).ToList();
                        }
                        else
                        {
                            m_Supplier = recent_Client.m_Element_User.Select(x => x.Supplier).ToList();
                        }

                        //Activity aufbauen
                        Build_Activity_Tree(this.Database.m_Activity, Supplier_Tree, m_Supplier);
                    }
                }
                else
                {
                    SysElement recent_Client = Client_Tree.SelectedNode.Tag as SysElement;

                    if (recent_Client != null)
                    {
                        List<Activity> m_Supplier = new List<Activity>();

                        if (flag_functional == 0)
                        {
                            List<Element_Functional> m_elemfunc = recent_Client.Get_m_Element_Functional();
                            m_Supplier = m_elemfunc.Select(x => x.Supplier).ToList();
                        }
                        else
                        {
                            List<Element_User> m_elemfunc = recent_Client.Get_m_Element_User();
                            m_Supplier = m_elemfunc.Select(x => x.Supplier).ToList();
                        }

                        //Activity aufbauen
                        Build_Activity_Tree(this.Database.m_Activity, Supplier_Tree, m_Supplier);
                    }
                }

            }

            //NAch Alphabet sortieren
            Supplier_Tree.Sort();
        }

        private void Build_Activity_Tree(List<Activity> m_Activity, TreeView tree, List<Activity> m_Supplier)
        {
            //Tree Reset
            tree.Nodes.Clear();
            //Obere Ebene nur Aktivitäten ohne Parent
            if(m_Activity.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_Activity[i1].m_Parent.Count == 0)
                    {
                        TreeNode Child = new TreeNode(m_Activity[i1].Get_Name(this.Database)) { Tag = m_Activity[i1] };
                        Child.Name = Child.Text;

                        int index = m_Supplier.FindIndex( x => x == m_Activity[i1]);

                        if(index == -1)
                        {
                            Child.ForeColor = Color.Gray;
                        }

                        bool flag_extend = Build_Activity_Node(m_Activity[i1].m_Child, Child, m_Supplier);

                        if(flag_extend == true)
                        {
                            Child.Expand();
                        }

                        tree.Nodes.Add(Child);
                    }

                    i1++;
                } while (i1 < m_Activity.Count);
            }
        }
        private bool Build_Activity_Node(List<Activity> m_Activity, TreeNode node, List<Activity> m_Supplier)
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
                    else
                    {
                        flag_extend = true;
                    }

                    bool flag_help = Build_Activity_Node(m_Activity[i1].m_Child, Child, m_Supplier);

                    if(flag_help == true)
                    {
                        flag_extend = true;
                    }

                    node.Nodes.Add(Child);

                    i1++;
                } while (i1 < m_Activity.Count);
            }

            if(flag_extend == true)
            {
                node.Expand();
            }

            return (flag_extend);
        }
        private void Reset_Supplier()
        {
            flag_reset = true;
            Pos_Qualitaet = -1;
            //////////////////////////////////////////////
            ///Textboxen reseten
            Text_Randbedingung.Text = Text_Randbedingung_ini;
            Text_Randbedingung.Enabled = false;
            Text_Qualitaet.Text = Text_Qualitaet_ini;
            Text_Qualitaet.Enabled = false;
            Text_Prozesswort.Text = Text_Prozesswort_ini;
            Text_Prozesswort.Enabled = false;
            Text_Object.Text = Text_Object_ini;
            Text_Object.Enabled = false;
            splitContainer4.Refresh();
            splitContainer3.Refresh();
            //Flags der Textboxen reseten
            flag_Prozesswort = false;
            flag_Objcet = false;
            flag_Qualitaet = false;
            flag_Randbedingung = false;
            //redcent_Activity reseten
            if (recentTreeNode_Activity != null)
            {
                recentTreeNode_Activity.ForeColor = Color.Black;
                recentTreeNode_Activity.BackColor = Color.White;
            }
            recentTreeNode_Activity = null;

            flag_reset = false;

            Auswahl_Capability.SelectedIndex = -1;
            Auswahl_Capability.Refresh();

            Auswahl_Stakeholder.Items.Clear();
            Auswahl_Stakeholder.Text = "";
            Auswahl_Stakeholder.Refresh();

            this.recent_Stakeholder_String = this.Database.metamodel.Stakeholder_Default;
            this.Stakeholder_Artikel_Index = 0;
            this.recent_Stakeholder_Artikel_string = this.Database.metamodel.Artikel[Stakeholder_Artikel_Index + 6];
            this.recent_Stakeholder = null;

            Stakeholder_Artikel.Text = "...";
            Stakeholder_Artikel.Enabled = false;
            Stakeholder_Artikel.Refresh();
        }
        /// <summary>
        /// Aufgerufen, wenn im Supplier Tree einer angewählt wird, wird aber vor setzen des Index ausgeführt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Supplier_Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //Textboxen reseten
            //Reset_Supplier();

            if (recentTreeNode_Activity != null)
            {
                recentTreeNode_Activity.ForeColor = Color.Black;
                recentTreeNode_Activity.BackColor = Color.White;
            }

            if (Color.Gray == e.Node.ForeColor)
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// Aufgerufen, wenn im Supplier Tree einer angewählt wird, wird aber nach setzen des Index ausgeführt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Supplier_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.new_req = false;
            Pos_Qualitaet = -1;
            flag_reset = true;
            Auswahl_Stakeholder.Items.Clear();
            Auswahl_Stakeholder.Text = "";
            Auswahl_Stakeholder.Refresh();
            Stakeholder_Artikel.Text = "...";
            Stakeholder_Artikel.Enabled = false;
            Stakeholder_Artikel.Refresh();
            this.recent_Stakeholder_String = this.Database.metamodel.Stakeholder_Default;
            this.Stakeholder_Artikel_Index = 0;
            this.recent_Stakeholder_Artikel_string = this.Database.metamodel.Artikel[Stakeholder_Artikel_Index + 6];
            this.recent_Stakeholder = null;
            /////////////////////////////////
            NodeType recent_Client = new NodeType(null ,null, null);
            SysElement recent_SysElem = new SysElement(null ,null, null);
            Activity recent_Supplier;

            #region Client, Supplier festlegen
            /////////////////////////////////
            if (this.Database.metamodel.flag_sysarch == false)
            {
                recent_Client = recentTreeNode_Client.Tag as NodeType;
               
                ///Stakeholder setzen
                if (flag_functional == 1)
                {
                    //Element_User finden
                    Activity recent_Activity = Supplier_Tree.SelectedNode.Tag as Activity;

                    if (recent_Activity.m_Element_User.Count > 0)
                    {
                        List<Element_User> m_User = recent_Activity.m_Element_User.Where(x => x.Client == recent_Client).ToList();

                        if (m_User.Count > 0)
                        {
                            this.recent_Element_User = m_User[0];
                            int combo_index_ST = -1;

                            int i1 = 0;
                            do
                            {
                                ComboBoxItem_Stakeholder recent_item = new ComboBoxItem_Stakeholder(recent_Element_User.m_Client_ST[i1].w_NUTZENDER, recent_Element_User.m_Client_ST[i1]);
                                this.Auswahl_Stakeholder.Items.Add(recent_item);

                                i1++;
                            } while (i1 < recent_Element_User.m_Client_ST.Count);

                            Auswahl_Stakeholder.Refresh();
                        }
                    }


                    //Aktuellen Supplier setzen

                    recent_Supplier = Supplier_Tree.SelectedNode.Tag as Activity;
                    recentTreeNode_Activity = Supplier_Tree.SelectedNode;
                    recentTreeNode_Activity.ForeColor = Color.White;
                    recentTreeNode_Activity.BackColor = Color.Green;
                }
                else
                {
                    //Aktuellen Supplier setzen
                    recent_Client = recentTreeNode_Client.Tag as NodeType;
                    recent_Supplier = Supplier_Tree.SelectedNode.Tag as Activity;
                    recentTreeNode_Activity = Supplier_Tree.SelectedNode;
                    recentTreeNode_Activity.ForeColor = Color.White;
                    recentTreeNode_Activity.BackColor = Color.Green;
                }
            }
            else
            {
                recent_SysElem = (SysElement) recentTreeNode_Client.Tag as SysElement;
              
                ///Stakeholder setzen
                if (flag_functional == 1)
                {
                    //Element_User finden
                    Activity recent_Activity = Supplier_Tree.SelectedNode.Tag as Activity;

                    if (recent_Activity.m_Element_User.Count > 0)
                    {
                        // List<string> m_id = recent_SysElem.m_Implements.Select(x => x.Classifier_ID).ToList();

                        //   List <Element_User> m_User = recent_Activity.m_Element_User.Where(x => recent_SysElem.m_Implements.Exists(x.Client) == true ).ToList();
                        List<Element_User> m_User = recent_SysElem.Get_m_Element_User().Where(x => x.Supplier == recent_Activity).ToList();

                        if (m_User.Count > 0)
                        {
                            this.recent_Element_User = m_User[0];
                            int combo_index_ST = -1;

                            int i1 = 0;
                            do
                            {
                                ComboBoxItem_Stakeholder recent_item = new ComboBoxItem_Stakeholder(recent_Element_User.m_Client_ST[i1].w_NUTZENDER, recent_Element_User.m_Client_ST[i1]);
                                this.Auswahl_Stakeholder.Items.Add(recent_item);

                                i1++;
                            } while (i1 < recent_Element_User.m_Client_ST.Count);

                            Auswahl_Stakeholder.Refresh();
                        }
                    }


                    //Aktuellen Supplier setzen

                    recent_Supplier = Supplier_Tree.SelectedNode.Tag as Activity;
                    recentTreeNode_Activity = Supplier_Tree.SelectedNode;
                    recentTreeNode_Activity.ForeColor = Color.White;
                    recentTreeNode_Activity.BackColor = Color.Green;
                }
                else
                {
                    //Aktuellen Supplier setzen
                    recent_SysElem = recentTreeNode_Client.Tag as SysElement;
                    recent_Supplier = Supplier_Tree.SelectedNode.Tag as Activity;
                    recentTreeNode_Activity = Supplier_Tree.SelectedNode;
                    recentTreeNode_Activity.ForeColor = Color.White;
                    recentTreeNode_Activity.BackColor = Color.Green;
                }
            }
            #endregion
            ////////////////////////////////
            //W_OBJECT, W_PROZESSWORT, W_QUALITAET und W_RANDBEDINGUNG befüllen
            TaggedValue taggedValue = new TaggedValue(this.Database.metamodel, this.Database);

            ///////////////////////////////////////
            //W_Qualitaet ist einer Anforderung zugeordnet
            string w_qualitaet = "";
            string w_randbedingung = "";
            //  string w_prozesswort = taggedValue.Get_Tagged_Value(recent_Supplier.Classifier_ID, "W_PROZESSWORT", this.Repository);
            //  string w_object = taggedValue.Get_Tagged_Value(recent_Supplier.Classifier_ID, "W_OBJEKT", this.Repository);
            string w_prozesswort = recent_Supplier.W_Prozesswort;
            string w_object = recent_Supplier.W_Object;



            #region Element_functional
            Element_Functional Check_functional = null;
            if (flag_functional == 0)
            {
                if(this.Database.metamodel.flag_sysarch == false)
                {
                    Check_functional = recent_Client.Check_Element_Functional(recent_Client, recent_Supplier);

                    if (Check_functional != null)
                    {
                        if (Check_functional.m_Requirement_Functional != null)
                        {
                            if (Check_functional.m_Requirement_Functional.Count > 0)
                            {
                                recent_req_functional = Check_functional.m_Requirement_Functional[0];
                                // w_qualitaet = taggedValue.Get_Tagged_Value(Check_functional.m_Requirement_Functional[0].Classifier_ID, "W_QUALITAET", this.Repository);
                                w_qualitaet = Check_functional.m_Requirement_Functional[0].W_QUALITAET;

                                if (w_qualitaet == null)
                                {
                                    w_qualitaet = "";
                                }
                                //   w_randbedingung = taggedValue.Get_Tagged_Value(Check_functional.m_Requirement_Functional[0].Classifier_ID, "W_RANDBEDINGUNG", this.Repository);
                                w_randbedingung = Check_functional.m_Requirement_Functional[0].W_RANDBEDINGUNG;

                                if (w_randbedingung == null)
                                {
                                    w_randbedingung = "";
                                }
                                // w_prozesswort = taggedValue.Get_Tagged_Value(Check_functional.m_Requirement_Functional[0].Classifier_ID, "W_PROZESSWORT", this.Repository);
                                w_prozesswort = Check_functional.m_Requirement_Functional[0].W_PROZESSWORT;

                                if (w_prozesswort == null)
                                {
                                    w_prozesswort = "";
                                }
                                // w_object = taggedValue.Get_Tagged_Value(Check_functional.m_Requirement_Functional[0].Classifier_ID, "W_OBJEKT", this.Repository);
                                w_object = Check_functional.m_Requirement_Functional[0].W_OBJEKT;

                                if (w_object == null)
                                {
                                    w_object = "";
                                }
                            }
                            else
                            {
                                new_req = true;
                                // Requirement_Functional recent_req = new Requirement_Functional(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_Client.W_Artikel+" "+recent_Client.Name, false, null);
                                // recent_req_functional = recent_req;
                            }
                        }
                        else
                        {
                            new_req = true;
                            //  Requirement_Functional recent_req_functional = new Requirement_Functional(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_Client.W_Artikel + " " + recent_Client.Name, false, null);
                            //  recent_req_functional = recent_req_functional;
                        }
                    }
                }

                else
                {
                    List<NodeType> m_recent_Client = recent_SysElem.m_Implements;

                    int s1 = 0;
                    do
                    {
                        Check_functional = m_recent_Client[s1].Check_Element_Functional(m_recent_Client[s1], recent_Supplier);

                        if (Check_functional != null)
                        {
                            if (Check_functional.m_Requirement_Functional != null)
                            {
                                if (Check_functional.m_Requirement_Functional.Count > 0)
                                {
                                    recent_req_functional = Check_functional.m_Requirement_Functional[0];
                                    // w_qualitaet = taggedValue.Get_Tagged_Value(Check_functional.m_Requirement_Functional[0].Classifier_ID, "W_QUALITAET", this.Repository);
                                    w_qualitaet = Check_functional.m_Requirement_Functional[0].W_QUALITAET;

                                    if (w_qualitaet == null)
                                    {
                                        w_qualitaet = "";
                                    }
                                    //   w_randbedingung = taggedValue.Get_Tagged_Value(Check_functional.m_Requirement_Functional[0].Classifier_ID, "W_RANDBEDINGUNG", this.Repository);
                                    w_randbedingung = Check_functional.m_Requirement_Functional[0].W_RANDBEDINGUNG;

                                    if (w_randbedingung == null)
                                    {
                                        w_randbedingung = "";
                                    }
                                    // w_prozesswort = taggedValue.Get_Tagged_Value(Check_functional.m_Requirement_Functional[0].Classifier_ID, "W_PROZESSWORT", this.Repository);
                                    w_prozesswort = Check_functional.m_Requirement_Functional[0].W_PROZESSWORT;

                                    if (w_prozesswort == null)
                                    {
                                        w_prozesswort = "";
                                    }
                                    // w_object = taggedValue.Get_Tagged_Value(Check_functional.m_Requirement_Functional[0].Classifier_ID, "W_OBJEKT", this.Repository);
                                    w_object = Check_functional.m_Requirement_Functional[0].W_OBJEKT;

                                    if (w_object == null)
                                    {
                                        w_object = "";
                                    }
                                }
                                else
                                {
                                    new_req = true;
                                    // Requirement_Functional recent_req = new Requirement_Functional(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_Client.W_Artikel+" "+recent_Client.Name, false, null);
                                    // recent_req_functional = recent_req;
                                }
                            }
                            else
                            {
                                new_req = true;
                                //  Requirement_Functional recent_req_functional = new Requirement_Functional(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_Client.W_Artikel + " " + recent_Client.Name, false, null);
                                //  recent_req_functional = recent_req_functional;
                            }
                        }

                        s1++;
                    } while (s1 < m_recent_Client.Count);

                  
                }
             
            }
            #endregion
     
            #region Element User
            Element_User Check_User = null;

            if(flag_functional == 1)
            {
                Check_User = recent_Element_User;
                if (Check_User != null)
                {
                    if (Check_User.m_Requirement_User != null)
                    {
                        if (Check_User.m_Requirement_User.Count > 0)
                        {
                            recent_req_user = Check_User.m_Requirement_User[0];
                            //   w_qualitaet = taggedValue.Get_Tagged_Value(Check_User.m_Requirement_User[0].Classifier_ID, "W_QUALITAET", this.Repository);
                            w_qualitaet = Check_User.m_Requirement_User[0].W_QUALITAET;
                            if (w_qualitaet == null)
                            {
                                w_qualitaet = "";
                            }
                            // w_randbedingung = taggedValue.Get_Tagged_Value(Check_User.m_Requirement_User[0].Classifier_ID, "W_RANDBEDINGUNG", this.Repository);
                            w_randbedingung = Check_User.m_Requirement_User[0].W_RANDBEDINGUNG;
                            if (w_randbedingung == null)
                            {
                                w_randbedingung = "";
                            }
                            // w_prozesswort = taggedValue.Get_Tagged_Value(Check_User.m_Requirement_User[0].Classifier_ID, "W_PROZESSWORT", this.Repository);
                            w_prozesswort = Check_User.m_Requirement_User[0].W_PROZESSWORT;
                            if (w_prozesswort == null)
                            {
                                w_prozesswort = "";
                            }
                            //  w_object = taggedValue.Get_Tagged_Value(Check_User.m_Requirement_User[0].Classifier_ID, "W_OBJEKT", this.Repository);
                            w_object = Check_User.m_Requirement_User[0].W_OBJEKT;
                            if (w_object == null)
                            {
                                w_object = "";
                            }
                        }
                        else
                        {
                            new_req = true;
                          //  Requirement_User recent_req = new Requirement_User(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_Client.W_Artikel + " " + recent_Client.Name, true, recent_Stakeholder_Artikel_string + " "+this.Auswahl_Stakeholder.Text, null);
                          //  recent_req_user = recent_req;
                        }
                    }
                    else
                    {
                        new_req = true;
                      //  Requirement_User recent_req = new Requirement_User(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_Client.W_Artikel + " " + recent_Client.Name, true, recent_Stakeholder_Artikel_string + " " + this.Auswahl_Stakeholder.Text, null);
                      //  recent_req_user = recent_req;
                    }
                }
            }
            #endregion Element_User

            if (w_qualitaet == "")
            {
                Text_Qualitaet.Text = Text_Qualitaet_sel;
                Text_Qualitaet.Refresh();
            }
            else
            {
                Text_Qualitaet.Text = w_qualitaet;
                Text_Qualitaet.Refresh();
            }
            Text_Qualitaet.Enabled = true;
            ///////////////////////////////////////
            //W_RANDBEDINGUNG
            if (w_randbedingung == "")
            {
                Text_Randbedingung.Text = Text_Randbedingung_sel;
                Text_Randbedingung.Refresh();
            }
            else
            {
                Text_Randbedingung.Text = w_randbedingung;
                Text_Randbedingung.Refresh();
            }
            Text_Randbedingung.Enabled = true;
            flag_reset = false;
            //W_PROZESSWORT
            if (w_prozesswort == "")
            {
                Text_Prozesswort.Text = Text_Prozesswort_sel;
                Text_Prozesswort.Refresh();
            }
            else
            {
                Text_Prozesswort.Text = w_prozesswort;
                Text_Prozesswort.Refresh();
            }
            Text_Prozesswort.Enabled = true;
            //////////////////////////////////////
            //W_OBJECT          
            if (w_object == "")
            {
                Text_Object.Text = Text_Object_sel;
                Text_Object.Refresh();
            }
            else
            {
                Text_Object.Text = w_object;
                Text_Object.Refresh();
            }
            Text_Object.Enabled = true;
            /////////////////////////////////////////////
            //AFO Text
            Create_Text_Afo_ini();
            //Create_Text_Afo_change();
            //Einfärben
            Check_For_Requirement();


            if(new_req == true)
            {
                if(flag_functional == 0)
                {
                    if(this.Database.metamodel.flag_sysarch == false)
                    {
                        Requirement_Functional recent_req = new Requirement_Functional(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_Client.W_Artikel + " " + recent_Client.Name, false, null, this.Database.metamodel);
                        recent_req_functional = recent_req;
                    }
                    else
                    {
                        Requirement_Functional recent_req = new Requirement_Functional(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_SysElem.W_Artikel + " " + recent_SysElem.Name, false, null, this.Database.metamodel);
                        recent_req_functional = recent_req;
                    }
                  
                }
                else
                {
                    if (this.Database.metamodel.flag_sysarch == false)
                    {
                        Requirement_User recent_req = new Requirement_User(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_Client.W_Artikel + " " + recent_Client.Name, true, recent_Stakeholder_Artikel_string + " " + this.Auswahl_Stakeholder.Text, null, this.Database.metamodel);
                        recent_req_user = recent_req;
                    }
                    else
                    {
                        Requirement_User recent_req = new Requirement_User(this.label_Afo.Text, this.Text_Afo.Text, this.Text_Object.Text, this.Text_Prozesswort.Text, this.Text_Qualitaet.Text, this.Text_Randbedingung.Text, true, recent_SysElem.W_Artikel + " " + recent_SysElem.Name, true, recent_Stakeholder_Artikel_string + " " + this.Auswahl_Stakeholder.Text, null, this.Database.metamodel);
                        recent_req_user = recent_req;
                    }
                }

            }
            /////////////////////////////////////////////
            /////////////////////////////////////////////
            //Zuordnung Fähigkeitsbaum
            #region Zuordnung Fähigkeitsbaum
            int combo_index = -1;
            
            if(flag_functional == 0)
            {
                Element_Functional Check = Check_functional;

                if (Check != null && Auswahl_Capability.Items.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        ComboBoxItem_Capability recent = Auswahl_Capability.Items[i1] as ComboBoxItem_Capability;

                        if (recent.Value == Check.Capability)
                        {
                            combo_index = i1;
                        }

                        i1++;
                    } while (i1 < Auswahl_Capability.Items.Count);
                }
            }
            if(flag_functional == 1)
            {
                Element_User Check = Check_User;

                if (Check != null && Auswahl_Capability.Items.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        ComboBoxItem_Capability recent = Auswahl_Capability.Items[i1] as ComboBoxItem_Capability;

                        if (recent.Value == Check.Capability)
                        {
                            combo_index = i1;
                        }

                        i1++;
                    } while (i1 < Auswahl_Capability.Items.Count);
                }
            }
           

            Auswahl_Capability.SelectedIndex = combo_index;
            Auswahl_Capability.Refresh();
            #endregion Zuordnung Fägihkeitsbaum

        }

        #endregion Supplier Tree

        #region Save Button
        private void Save_Click(object sender, EventArgs e)
        {
            Form_Edit_Requirement new_edit;

            if(checkBox_logical_elements.Checked == true)
            {
                this.Database.metamodel.flag_sysarch = false;
            }
            else
            {
                this.Database.metamodel.flag_sysarch = true;
            }


            if(flag_functional == 0)
            {
                new_edit = new Form_Edit_Requirement(this.Database, recent_req_functional, Repository, false);
            }
            else
            {
                new_edit = new Form_Edit_Requirement(this.Database, recent_req_user, Repository, false);
            }

            DialogResult dialog_result = new_edit.ShowDialog();


            List<DB_Insert> m_Insert = new List<DB_Insert>();


            if(dialog_result == DialogResult.OK)
            {

                #region AFO anlegen
                if (recentTreeNode_Client != null && recentTreeNode_Activity != null)
                {
                    #region PAckages
                    Repository_Element repository_Element = new Repository_Element();
                    //PAckage allg Requirement anlegen bzw. erhalten
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this.Database);
                    EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package Infoübertragung anlegen bzw erhalten
                    string Package_GUID = repository_Element.Create_Package_Model("Funktionale Anforderung - Requirement_Plugin", Repository, this.Database);
                    EA.Package Package_Funktionale = Repository.GetPackageByGuid(Package_GUID);
                    Package_Funktionale.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Funktionale.Update();
                    #endregion PAckages
                    ///////////////
                    //Anforderung erstellen
                    //Requirement_Functional requirement_Functional = new Requirement_Functional();
                    NodeType recent_NodeType = new NodeType(null ,null, null);
                    SysElement recent_SysElem = new SysElement(null, null, null);
                    if (this.Database.metamodel.flag_sysarch == false)
                    {
                        recent_NodeType = recentTreeNode_Client.Tag as NodeType;
                    }
                    else
                    {
                        recent_SysElem = recentTreeNode_Client.Tag as SysElement;
                    }
                    Activity recent_Activity = null;

                    if (flag_functional == 0)
                    {
                        recent_Activity = recentTreeNode_Activity.Tag as Activity;
                    }
                    if (flag_functional == 1)
                    {
                        recent_Activity = recentTreeNode_Activity.Tag as Activity;

                    }

                    #region Afo in Database anlegen
                    Requirement_Functional requirement_Functional = null;
                    Requirement_User requirement_User = null;

                    string w_prozesswort = "";
                    string w_Object = "";
                    string w_Qualitaet = "";
                    string w_Randebedingung = "";

                    bool W_ZU = false;
                    string W_NUTZENDER = null;

                    if (flag_functional == 0)
                    {
                        requirement_Functional = recent_req_functional;

                    }
                    else
                    {
                        requirement_User = recent_req_user;
                    }
                        if (flag_functional == 1)
                       {
                            W_ZU = true;
                            W_NUTZENDER = requirement_User.W_NUTZENDER; //recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String;
                            if (requirement_User.W_PROZESSWORT != this.Text_Prozesswort_ini && requirement_User.W_PROZESSWORT != this.Text_Prozesswort_sel)
                            {
                                w_prozesswort = requirement_User.W_PROZESSWORT; //Text_Prozesswort.Text;
                                this.Text_Prozesswort.Text = w_prozesswort;
                            }
                            if (requirement_User.W_OBJEKT != Text_Object_ini && requirement_User.W_OBJEKT != Text_Object_sel)
                            {
                                w_Object = requirement_User.W_OBJEKT;
                                this.Text_Object.Text = w_Object;
                            }
                            if (requirement_User.W_QUALITAET != Text_Qualitaet_ini & requirement_User.W_QUALITAET != Text_Qualitaet_sel)
                            {
                              w_Qualitaet = requirement_User.W_QUALITAET;
                            this.Text_Qualitaet.Text = w_Qualitaet;
                            }

                            if (requirement_User.W_RANDBEDINGUNG != Text_Randbedingung_ini && requirement_User.W_RANDBEDINGUNG != Text_Randbedingung_sel)
                            {
                                w_Randebedingung = requirement_User.W_RANDBEDINGUNG;
                            this.Text_Randbedingung.Text = w_Randebedingung;
                            }
                    }
                        else
                    {
                        W_ZU = false;
                    //    W_NUTZENDER = requirement_User.W_NUTZENDER; //recent_Stakeholder_Artikel_string + " " + recent_Stakeholder_String;
                        if (requirement_Functional.W_PROZESSWORT != this.Text_Prozesswort_ini && requirement_Functional.W_PROZESSWORT != this.Text_Prozesswort_sel)
                        {
                            w_prozesswort = requirement_Functional.W_PROZESSWORT; //Text_Prozesswort.Text;
                            this.Text_Prozesswort.Text = w_prozesswort;
                        }
                        if (requirement_Functional.W_OBJEKT != Text_Object_ini && requirement_Functional.W_OBJEKT != Text_Object_sel)
                        {
                            w_Object = requirement_Functional.W_OBJEKT;
                            this.Text_Object.Text = w_Object;
                        }
                        if (requirement_Functional.W_QUALITAET != Text_Qualitaet_ini & requirement_Functional.W_QUALITAET != Text_Qualitaet_sel)
                        {
                            w_Qualitaet = requirement_Functional.W_QUALITAET;
                            this.Text_Qualitaet.Text = w_Qualitaet;
                        }

                        if (requirement_Functional.W_RANDBEDINGUNG != Text_Randbedingung_ini && requirement_Functional.W_RANDBEDINGUNG != Text_Randbedingung_sel)
                        {
                            w_Randebedingung = requirement_Functional.W_RANDBEDINGUNG;
                            this.Text_Randbedingung.Text = w_Randebedingung;
                        }
                    }

                    this.Text_Prozesswort.Refresh();
                    this.Text_Object.Refresh();
                    this.Text_Qualitaet.Refresh();
                    this.Text_Randbedingung.Refresh();

                  
                    //Parnet Afo zuornden


                    #endregion Afo in Database anlegen
                    string GUID_Requirement = "";
                    
                    if(this.Database.metamodel.flag_sysarch == false)
                    {

                        if (recent_NodeType.m_Element_Functional.Count > 0 || recent_NodeType.m_Element_User.Count > 0)
                        {
                            #region AFo im Repository anlegen
                            int i1 = 0;
                            #region Element_Functional
                            if (flag_functional == 0)
                            {
                                GUID_Requirement = "";
                                do
                                {

                                    if (recent_NodeType.m_Element_Functional[i1].Supplier == recent_Activity)
                                    {

                                        if (recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Count > 0)
                                        {
                                            //Requirement_Functional requirement_Functional = new Requirement_Functional(label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name_t_object_GUID(Repository), W_ZU);
                                            Requirement copy = new Requirement(recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Classifier_ID, this.Database.metamodel);
                                            copy.Get_Tagged_Values_From_Requirement(copy.Classifier_ID, Repository, Database);
                                            //  GUID_Requirement = recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Update_Requirement(Repository, Database, label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name(this.Database), W_ZU, null);
                                            GUID_Requirement = recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Classifier_ID;
                                            requirement_Functional.Classifier_ID = GUID_Requirement;
                                            //  GUID_Requirement = recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Update_Requirement(Repository, Database, requirement_Functional.AFO_TITEL, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name(this.Database), W_ZU, null);
                                            recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0] = requirement_Functional;
                                            recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Stereotype = this.Database.metamodel.m_Requirement_Functional[0].Stereotype;
                                            recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Update_Requirement_All(Repository, this.Database);

                                            recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Compare_Requirement(Database, Repository, copy);

                                        }
                                        else
                                        {
                                            GUID_Requirement = requirement_Functional.Create_Requirement_funktional(Repository, Package_Funktionale.PackageGUID, Database.metamodel.m_Requirement_Functional[0].Stereotype, Database);


                                            recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Add(requirement_Functional);
                                        }

                                        Repository_Connector repository_Connector = new Repository_Connector();

                                        requirement_Functional.Classifier_ID = GUID_Requirement;

                                        //Alle Connectoren löschen
                                        //Node
                                        /*       requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList());
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                                               //User
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList());
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList());
                                               //Activity
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList());
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList());
                                               */

                                        //Connecotr Capability anlegen
                                        if (Auswahl_Capability.SelectedIndex != -1)
                                        {
                                            ComboBoxItem_Capability recent_comboitem = Auswahl_Capability.Items[Auswahl_Capability.SelectedIndex] as ComboBoxItem_Capability;

                                            recent_NodeType.m_Element_Functional[i1].Capability = null;
                                            recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].m_Capability = new List<Capability>();

                                            //Connecotor einfügen
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_comboitem.Value.Classifier_ID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);
                                            recent_NodeType.m_Element_Functional[i1].Capability = recent_comboitem.Value;

                                            if (recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].m_Capability.Contains(recent_comboitem.Value) == false)
                                            {
                                                recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].m_Capability.Add(recent_comboitem.Value);
                                            }
                                        }

                                        //  recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Add(requirement_Functional);

                                        if (recent_NodeType.m_Element_Functional[i1].m_Target_Functional.Count > 0) //DerivedFrom anlegen 
                                        {
                                            int i2 = 0;
                                            do
                                            {

                                                repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_Functional[i1].m_Target_Functional[i2].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                                repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_Functional[i1].m_Target_Functional[i2].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                                i2++;
                                            } while (i2 < recent_NodeType.m_Element_Functional[i1].m_Target_Functional.Count);
                                        }
                                        else
                                        {
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_Functional[i1].Client.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_Functional[i1].Supplier.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                        }

                                        

                                        //  List<Requirement_Non_Functional> m_req_process = recent_NodeType.m_Element_Functional[i1].Supplier.Element_Process_Get_All_Requirement();
                                        List<Requirement_Non_Functional> m_req_process = recent_NodeType.m_Element_Functional[i1].Supplier.Element_Process_Get_NodeType_Requirement(recent_NodeType);

                                        if (m_req_process.Count > 0)
                                        {
                                            //this.Database

                                            int i4 = 0;
                                            do
                                            {
                                                repository_Connector.Create_Dependency(m_req_process[i4].Classifier_ID, GUID_Requirement, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);

                                                i4++;
                                            } while (i4 < m_req_process.Count);
                                        }

                                    }
                                    i1++;
                                } while (i1 < recent_NodeType.m_Element_Functional.Count);
                            }
                            #endregion Element_Functional

                            #region Element User
                            if (flag_functional == 1)
                            {
                                GUID_Requirement = "";
                                do
                                {
                                    if (recent_NodeType.m_Element_User[i1] == recent_Element_User)
                                    {

                                        if (recent_NodeType.m_Element_User[i1].m_Requirement_User.Count > 0)
                                        {
                                            //Requirement_Functional requirement_Functional = new Requirement_Functional(label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name_t_object_GUID(Repository), W_ZU);
                                            Requirement copy = new Requirement(recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Classifier_ID, this.Database.metamodel);
                                            copy.Get_Tagged_Values_From_Requirement(copy.Classifier_ID, Repository, Database);
                                            GUID_Requirement = recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Classifier_ID;

                                            requirement_User.Classifier_ID = GUID_Requirement;
                                            recent_NodeType.m_Element_User[i1].m_Requirement_User[0] = requirement_User;
                                            recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Stereotype = Database.metamodel.m_Requirement_User[0].Stereotype;
                                            recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Update_Requirement_All(Repository, Database);
                                            //GUID_Requirement = recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Update_Requirement(Repository, Database, label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name(this.Database), W_ZU, W_NUTZENDER);

                                            recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Compare_Requirement(Database, Repository, copy);
                                        }
                                        else
                                        {
                                            GUID_Requirement = requirement_User.Create_Requirement_User(Repository, Package_Funktionale.PackageGUID, Database.metamodel.m_Requirement_User[0].Stereotype, Database);
                                            recent_NodeType.m_Element_User[i1].m_Requirement_User.Add(requirement_User);
                                        }

                                        Repository_Connector repository_Connector = new Repository_Connector();

                                        requirement_User.Classifier_ID = GUID_Requirement;

                                        //Alle Connectoren löschen
                                        //Node
                                        /*    requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList());
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                                            //User
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList());
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList());
                                            //Activity
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList());
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList());
            */

                                        //Connecotr Capability anlegen
                                        if (Auswahl_Capability.SelectedIndex != -1)
                                        {
                                            ComboBoxItem_Capability recent_comboitem = Auswahl_Capability.Items[Auswahl_Capability.SelectedIndex] as ComboBoxItem_Capability;

                                            recent_NodeType.m_Element_User[i1].Capability = null;
                                            recent_NodeType.m_Element_User[i1].m_Requirement_User[0].m_Capability = new List<Capability>();

                                            //Connecotor einfügen
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_comboitem.Value.Classifier_ID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);
                                            recent_NodeType.m_Element_User[i1].Capability = recent_comboitem.Value;

                                            if (recent_NodeType.m_Element_User[i1].m_Requirement_User[0].m_Capability.Contains(recent_comboitem.Value) == false)
                                            {
                                                recent_NodeType.m_Element_User[i1].m_Requirement_User[0].m_Capability.Add(recent_comboitem.Value);
                                            }
                                        }

                                        //  recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Add(requirement_Functional);

                                        if (recent_NodeType.m_Element_User[i1].m_Target_User.Count > 0)
                                        {
                                            int i2 = 0;
                                            do
                                            {
                                                bool flag_cd = false;

                                                if (recent_Stakeholder != null)
                                                {
                                                    if (recent_NodeType.m_Element_User[i1].m_Target_User[i2].Check_Stakeholder(recent_Stakeholder.Classifier_ID) == true)
                                                    {
                                                        flag_cd = true;
                                                    }
                                                }
                                                else
                                                {
                                                    flag_cd = true;
                                                }

                                                if (flag_cd == true)
                                                {
                                                    repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].m_Target_User[i2].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                                    repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].m_Target_User[i2].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                                    repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].m_Target_User[i2].Client_ST, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                                }

                                                i2++;
                                            } while (i2 < recent_NodeType.m_Element_User[i1].m_Target_User.Count);
                                        }
                                        else
                                        {
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].Client.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].Supplier.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                            if (recent_Stakeholder != null)
                                            {
                                                repository_Connector.Create_Dependency(GUID_Requirement, recent_Stakeholder.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                            }
                                            else
                                            {
                                                if (recent_NodeType.m_Element_User[i1].m_Client_ST.Count > 0)
                                                {
                                                    int i3 = 0;
                                                    do
                                                    {
                                                        repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].m_Client_ST[i3].Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                                        i3++;
                                                    } while (i3 < recent_NodeType.m_Element_User[i1].m_Client_ST.Count);
                                                }

                                            }


                                        }

                                       //List<Requirement_Non_Functional> m_req_process = recent_NodeType.m_Element_User[i1].Supplier.Element_Process_Get_All_Requirement();
                                       List < Requirement_Non_Functional > m_req_process = recent_NodeType.m_Element_User[i1].Supplier.Element_Process_Get_NodeType_Requirement(recent_NodeType);

                                        if (m_req_process.Count > 0)
                                        {
                                            int i4 = 0;
                                            do
                                            {
                                                repository_Connector.Create_Dependency(m_req_process[i4].Classifier_ID, GUID_Requirement, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);

                                                i4++;
                                            } while (i4 < m_req_process.Count);
                                        }

                                    }
                                    i1++;
                                } while (i1 < recent_NodeType.m_Element_User.Count);
                            }
                            #endregion Element User

                            #region Parent AFO zuweisen
                            if (recent_Activity.m_Parent.Count != 0 && GUID_Requirement != "")
                            {
                                Repository_Connector repository_Connector = new Repository_Connector();

                                int p1 = 0;
                                do
                                {
                                    Activity Parent = recent_Activity.m_Parent[p1];

                                    if (Parent.m_Element_Functional.Count > 0)
                                    {
                                        int p2 = 0;
                                        do
                                        {
                                            if (Parent.m_Element_Functional[p2].m_Requirement_Functional.Count > 0)
                                            {
                                                repository_Connector.Create_Dependency(GUID_Requirement, Parent.m_Element_Functional[p2].m_Requirement_Functional[0].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                            }

                                            p2++;
                                        } while (p2 < Parent.m_Element_Functional.Count);
                                    }

                                    if (Parent.m_Element_User.Count > 0)
                                    {
                                        int p3 = 0;
                                        do
                                        {
                                            if (Parent.m_Element_User[p3].m_Requirement_User.Count > 0)
                                            {
                                                repository_Connector.Create_Dependency(GUID_Requirement, Parent.m_Element_User[p3].m_Requirement_User[0].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                            }


                                            p3++;
                                        } while (p3 < Parent.m_Element_User.Count);
                                    }

                                    p1++;
                                } while (p1 < recent_Activity.m_Parent.Count);
                            }

                            #endregion Parent AFO zuweisen

                            #region Child AFO zuweisen
                            if(recent_Activity.m_Child.Count != 0 && GUID_Requirement != "")
                            {
                                Repository_Connector repository_Connector = new Repository_Connector();

                                int p1 = 0;
                                do
                                {
                                    Activity Child = recent_Activity.m_Child[p1];

                                    if (Child.m_Element_Functional.Count > 0)
                                    {
                                        int p2 = 0;
                                        do
                                        {
                                            if (Child.m_Element_Functional[p2].m_Requirement_Functional.Count > 0)
                                            {
                                                repository_Connector.Create_Dependency(Child.m_Element_Functional[p2].m_Requirement_Functional[0].Classifier_ID, GUID_Requirement, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                            }

                                            p2++;
                                        } while (p2 < Child.m_Element_Functional.Count);
                                    }

                                    if (Child.m_Element_User.Count > 0)
                                    {
                                        int p3 = 0;
                                        do
                                        {
                                            if (Child.m_Element_User[p3].m_Requirement_User.Count > 0)
                                            {
                                                repository_Connector.Create_Dependency(Child.m_Element_User[p3].m_Requirement_User[0].Classifier_ID, GUID_Requirement, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                            }


                                            p3++;
                                        } while (p3 < Child.m_Element_User.Count);
                                    }

                                    p1++;
                                } while (p1 < recent_Activity.m_Child.Count);
                            }
                            #endregion

                            #region Processanforderung zuweisen
                            /*  if(recent_Activity.m_Process.Count != 0 && GUID_Requirement != "")
                              {
                                  Repository_Connector repository_Connector = new Repository_Connector();
                                  int p1 = 0;
                                  do
                                  {
                                      if(recent_Activity.m_Process[p1].Requirement_Process != null)
                                      {
                                          if (recent_Activity.m_Process[p1].Requirement_Process.Classifier_ID != null)
                                          {
                                              repository_Connector.Create_Dependency(recent_Activity.m_Process[p1].Requirement_Process.Classifier_ID, GUID_Requirement, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database);
                                          }
                                      }


                                      p1++;
                                  } while (p1 < recent_Activity.m_Process.Count);
                              }*/
                            #endregion

                            #region Dopplungen Zuweisen
                            List<List<Repository_Element>> m_help = recent_NodeType.Check_Dopplung_Functional(this.Database, Repository);

                            if(m_help.Count >0)
                            {
                                recent_NodeType.Create_Issue_Dopplung_Functional(this.Database, Repository, m_help, null, false);

                                
                                    List<List<Repository_Element>> m_Dopplung_Process = new List<List<Repository_Element>>();


                                    Parallel.ForEach(this.Database.m_NodeType, nodetype =>
                                    {
                                        List<List<Repository_Element>> m_help_Issue = m_help.Where(x => (NodeType)x[0] == nodetype).ToList();

                                        if (m_help_Issue.Count > 0)
                                        {
                                            List<List<Repository_Element>> m_help2 = new List<List<Repository_Element>>();
                                            m_help2 = nodetype.Check_Dopplung_Process(this.Database, Repository, m_help_Issue);

                                            if (m_help2.Count > 0)
                                            {
                                                m_Dopplung_Process.AddRange(m_help2);
                                            }
                                        }
                                    });

                                    if (m_Dopplung_Process.Count > 0)
                                    {
                                        

                                        int p1 = 0;
                                        do
                                        {
                                            List<List<Repository_Element>> m_help_Issue = m_Dopplung_Process.Where(x => (NodeType)x[0] == this.Database.m_NodeType[p1]).ToList();

                                            if (m_help_Issue.Count > 0)
                                            {
                                                this.Database.m_NodeType[p1].Create_Issue_Dopplung_Process(this.Database, Repository, m_help_Issue, null, false);
                                            }

                                            i1++;
                                        } while (i1 < this.Database.m_NodeType.Count);
                                    }
                                
                                
                            }

                            List<List<Repository_Element>> m_help3 = recent_NodeType.Check_Dopplung_User(this.Database, Repository);

                            if (m_help3.Count > 0)
                            {
                                recent_NodeType.Create_Issue_Dopplung_User(this.Database, Repository, m_help3, null, false);

                            }
                            #endregion

                            #region QualityRequirement zuweisen
                            if(flag_functional == 0)
                            {
                                List<Element_Functional> m_func_help = recent_NodeType.m_Element_Functional.Where(x => x.Supplier == recent_Activity).ToList();

                                if(m_func_help.Count > 0)
                                {
                                    m_func_help[0].Create_Refines_Measurement(this.Database, Repository);
                                }
                            }
                            else
                            {
                                List<Element_User> m_func_help = recent_NodeType.m_Element_User.Where(x => x.Supplier == recent_Activity).ToList();

                                if (m_func_help.Count > 0)
                                {
                                    m_func_help[0].Create_Refines_Measurement_User(this.Database, Repository);
                                }
                            }

                            #endregion

                            Create_Text_Afo_change();

                            //Einfärben
                            Check_For_Requirement();

                            #endregion Afo im Repository anlegen
                        }
                    }
                    else
                    {
                        List<Element_Functional> m_elemfunc = recent_SysElem.Get_m_Element_Functional();
                        List<Element_User> m_elemuser = recent_SysElem.Get_m_Element_User();

                        if (m_elemfunc.Count > 0 || m_elemuser.Count > 0)
                        {

                            int i1 = 0;
                            #region Element_Functional
                            if (flag_functional == 0)
                            {
                                Repository_Connector repository_Connector = new Repository_Connector();
                                GUID_Requirement = "";
                                do
                                {

                                    if (m_elemfunc[i1].Supplier == recent_Activity)
                                    {

                                        if (m_elemfunc[i1].m_Requirement_Functional.Count > 0)
                                        {
                                            //Requirement_Functional requirement_Functional = new Requirement_Functional(label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name_t_object_GUID(Repository), W_ZU);
                                            Requirement copy = new Requirement(m_elemfunc[i1].m_Requirement_Functional[0].Classifier_ID, this.Database.metamodel);
                                            copy.Get_Tagged_Values_From_Requirement(copy.Classifier_ID, Repository, Database);
                                            //  GUID_Requirement = recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Update_Requirement(Repository, Database, label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name(this.Database), W_ZU, null);
                                            GUID_Requirement = m_elemfunc[i1].m_Requirement_Functional[0].Classifier_ID;
                                            requirement_Functional.Classifier_ID = GUID_Requirement;
                                            //  GUID_Requirement = recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Update_Requirement(Repository, Database, requirement_Functional.AFO_TITEL, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name(this.Database), W_ZU, null);
                                            m_elemfunc[i1].m_Requirement_Functional[0] = requirement_Functional;
                                            m_elemfunc[i1].m_Requirement_Functional[0].Stereotype = this.Database.metamodel.m_Requirement_Functional[0].Stereotype;
                                            m_elemfunc[i1].m_Requirement_Functional[0].Update_Requirement_All(Repository, this.Database);

                                            m_elemfunc[i1].m_Requirement_Functional[0].Compare_Requirement(Database, Repository, copy);

                                        }
                                        else
                                        {
                                            GUID_Requirement = requirement_Functional.Create_Requirement_funktional(Repository, Package_Funktionale.PackageGUID, Database.metamodel.m_Requirement_Functional[0].Stereotype, Database);


                                            m_elemfunc[i1].m_Requirement_Functional.Add(requirement_Functional);
                                        }

                                       

                                        requirement_Functional.Classifier_ID = GUID_Requirement;

                                        //Alle Connectoren löschen
                                        //Node
                                        /*       requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList());
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                                               //User
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList());
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList());
                                               //Activity
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList());
                                               requirement_Functional.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList());
                                               */

                                        //Connecotr Capability anlegen
                                        if (Auswahl_Capability.SelectedIndex != -1)
                                        {
                                            ComboBoxItem_Capability recent_comboitem = Auswahl_Capability.Items[Auswahl_Capability.SelectedIndex] as ComboBoxItem_Capability;

                                            m_elemfunc[i1].Capability = null;
                                            m_elemfunc[i1].m_Requirement_Functional[0].m_Capability = new List<Capability>();

                                            //Connecotor einfügen
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_comboitem.Value.Classifier_ID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);
                                            recent_NodeType.m_Element_Functional[i1].Capability = recent_comboitem.Value;

                                            if (m_elemfunc[i1].m_Requirement_Functional[0].m_Capability.Contains(recent_comboitem.Value) == false)
                                            {
                                                m_elemfunc[i1].m_Requirement_Functional[0].m_Capability.Add(recent_comboitem.Value);
                                            }
                                        }

                                        //  recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Add(requirement_Functional);

                                        if (m_elemfunc[i1].m_Target_Functional.Count > 0) //DerivedFrom anlegen 
                                        {
                                            int i2 = 0;
                                            do
                                            {

                                                repository_Connector.Create_Dependency(GUID_Requirement, m_elemfunc[i1].m_Target_Functional[i2].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                                repository_Connector.Create_Dependency(GUID_Requirement, m_elemfunc[i1].m_Target_Functional[i2].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                                i2++;
                                            } while (i2 < m_elemfunc[i1].m_Target_Functional.Count);
                                        }
                                        else
                                        {
                                            repository_Connector.Create_Dependency(GUID_Requirement, m_elemfunc[i1].Client.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                            repository_Connector.Create_Dependency(GUID_Requirement, m_elemfunc[i1].Supplier.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                        }
                                    }
                                    i1++;
                                } while (i1 < m_elemfunc.Count);

                                //Konnektor System anlegen
                                repository_Connector.Create_Dependency(GUID_Requirement, recent_SysElem.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, Repository, Database, Database.metamodel.m_Derived_SysElement[0].SubType, Database.metamodel.m_Derived_SysElement[0].direction);

                            }
                            #endregion Element_Functional

                            #region Element User
                            if (flag_functional == 1)
                            {
                                Repository_Connector repository_Connector = new Repository_Connector();
                                GUID_Requirement = "";
                                do
                                {
                                    if (m_elemuser[i1] == recent_Element_User)
                                    {

                                        if (m_elemuser[i1].m_Requirement_User.Count > 0)
                                        {
                                            //Requirement_Functional requirement_Functional = new Requirement_Functional(label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name_t_object_GUID(Repository), W_ZU);
                                            Requirement copy = new Requirement(m_elemuser[i1].m_Requirement_User[0].Classifier_ID, this.Database.metamodel);
                                            copy.Get_Tagged_Values_From_Requirement(copy.Classifier_ID, Repository, Database);
                                            GUID_Requirement = m_elemuser[i1].m_Requirement_User[0].Classifier_ID;

                                            requirement_User.Classifier_ID = GUID_Requirement;
                                            m_elemuser[i1].m_Requirement_User[0] = requirement_User;
                                            m_elemuser[i1].m_Requirement_User[0].Stereotype = Database.metamodel.m_Requirement_User[0].Stereotype;
                                            m_elemuser[i1].m_Requirement_User[0].Update_Requirement_All(Repository, Database);
                                            //GUID_Requirement = recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Update_Requirement(Repository, Database, label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name(this.Database), W_ZU, W_NUTZENDER);

                                            m_elemuser[i1].m_Requirement_User[0].Compare_Requirement(Database, Repository, copy);
                                        }
                                        else
                                        {
                                            GUID_Requirement = requirement_User.Create_Requirement_User(Repository, Package_Funktionale.PackageGUID, Database.metamodel.m_Requirement_User[0].Stereotype, Database);
                                            m_elemuser[i1].m_Requirement_User.Add(requirement_User);
                                        }

                                       

                                        requirement_User.Classifier_ID = GUID_Requirement;

                                        //Alle Connectoren löschen
                                        //Node
                                        /*    requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList());
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                                            //User
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList());
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList());
                                            //Activity
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList(), Database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList());
                                            requirement_User.Delete_All_Connector(this.Repository, this.Database, Database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList(), Database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList());
            */

                                        //Connecotr Capability anlegen
                                        if (Auswahl_Capability.SelectedIndex != -1)
                                        {
                                            ComboBoxItem_Capability recent_comboitem = Auswahl_Capability.Items[Auswahl_Capability.SelectedIndex] as ComboBoxItem_Capability;

                                            m_elemuser[i1].Capability = null;
                                            m_elemuser[i1].m_Requirement_User[0].m_Capability = new List<Capability>();

                                            //Connecotor einfügen
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_comboitem.Value.Classifier_ID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);
                                            m_elemuser[i1].Capability = recent_comboitem.Value;

                                            if (m_elemuser[i1].m_Requirement_User[0].m_Capability.Contains(recent_comboitem.Value) == false)
                                            {
                                                m_elemuser[i1].m_Requirement_User[0].m_Capability.Add(recent_comboitem.Value);
                                            }
                                        }

                                        //  recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Add(requirement_Functional);

                                        if (m_elemuser[i1].m_Target_User.Count > 0)
                                        {
                                            int i2 = 0;
                                            do
                                            {
                                                bool flag_cd = false;

                                                if (recent_Stakeholder != null)
                                                {
                                                    if (m_elemuser[i1].m_Target_User[i2].Check_Stakeholder(recent_Stakeholder.Classifier_ID) == true)
                                                    {
                                                        flag_cd = true;
                                                    }
                                                }
                                                else
                                                {
                                                    flag_cd = true;
                                                }

                                                if (flag_cd == true)
                                                {
                                                    repository_Connector.Create_Dependency(GUID_Requirement, m_elemuser[i1].m_Target_User[i2].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                                    repository_Connector.Create_Dependency(GUID_Requirement, m_elemuser[i1].m_Target_User[i2].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                                    repository_Connector.Create_Dependency(GUID_Requirement, m_elemuser[i1].m_Target_User[i2].Client_ST, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                                }

                                                i2++;
                                            } while (i2 < m_elemuser[i1].m_Target_User.Count);
                                        }
                                        else
                                        {
                                            repository_Connector.Create_Dependency(GUID_Requirement, m_elemuser[i1].Client.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                            repository_Connector.Create_Dependency(GUID_Requirement, m_elemuser[i1].Supplier.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                            if (recent_Stakeholder != null)
                                            {
                                                repository_Connector.Create_Dependency(GUID_Requirement, recent_Stakeholder.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                            }
                                            else
                                            {
                                                if (m_elemuser[i1].m_Client_ST.Count > 0)
                                                {
                                                    int i3 = 0;
                                                    do
                                                    {
                                                        repository_Connector.Create_Dependency(GUID_Requirement, m_elemuser[i1].m_Client_ST[i3].Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                                        i3++;
                                                    } while (i3 < m_elemuser[i1].m_Client_ST.Count);
                                                }

                                            }


                                        }
                                    }
                                    i1++;
                                } while (i1 < m_elemuser.Count);
                                //Konnektor System
                                repository_Connector.Create_Dependency(GUID_Requirement, recent_SysElem.Classifier_ID, Database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_SysElement[0].SubType, Repository, Database, Database.metamodel.m_Derived_SysElement[0].Toolbox, Database.metamodel.m_Derived_SysElement[0].direction);


                            }
                            #endregion Element User

                            #region Parent AFO zuweisen
                            if (recent_Activity.m_Parent.Count != 0 && GUID_Requirement != "")
                            {
                                Repository_Connector repository_Connector = new Repository_Connector();

                                int p1 = 0;
                                do
                                {
                                    Activity Parent = recent_Activity.m_Parent[p1];

                                    if (Parent.m_Element_Functional.Count > 0)
                                    {
                                        int p2 = 0;
                                        do
                                        {
                                            if (Parent.m_Element_Functional[p2].m_Requirement_Functional.Count > 0)
                                            {
                                                repository_Connector.Create_Dependency(GUID_Requirement, Parent.m_Element_Functional[p2].m_Requirement_Functional[0].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                            }

                                            p2++;
                                        } while (p2 < Parent.m_Element_Functional.Count);
                                    }

                                    if (Parent.m_Element_User.Count > 0)
                                    {
                                        int p3 = 0;
                                        do
                                        {
                                            if (Parent.m_Element_User[p3].m_Requirement_User.Count > 0)
                                            {
                                                repository_Connector.Create_Dependency(GUID_Requirement, Parent.m_Element_User[p3].m_Requirement_User[0].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                            }


                                            p3++;
                                        } while (p3 < Parent.m_Element_User.Count);
                                    }

                                    p1++;
                                } while (p1 < recent_Activity.m_Parent.Count);
                            }

                            #endregion Parent AFO zuweisen

                            #region Child AFO zuweisen
                            if (recent_Activity.m_Child.Count != 0 && GUID_Requirement != "")
                            {
                                Repository_Connector repository_Connector = new Repository_Connector();

                                int p1 = 0;
                                do
                                {
                                    Activity Child = recent_Activity.m_Child[p1];

                                    if (Child.m_Element_Functional.Count > 0)
                                    {
                                        int p2 = 0;
                                        do
                                        {
                                            if (Child.m_Element_Functional[p2].m_Requirement_Functional.Count > 0)
                                            {
                                                repository_Connector.Create_Dependency(Child.m_Element_Functional[p2].m_Requirement_Functional[0].Classifier_ID, GUID_Requirement, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                            }

                                            p2++;
                                        } while (p2 < Child.m_Element_Functional.Count);
                                    }

                                    if (Child.m_Element_User.Count > 0)
                                    {
                                        int p3 = 0;
                                        do
                                        {
                                            if (Child.m_Element_User[p3].m_Requirement_User.Count > 0)
                                            {
                                                repository_Connector.Create_Dependency(Child.m_Element_User[p3].m_Requirement_User[0].Classifier_ID, GUID_Requirement, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                            }


                                            p3++;
                                        } while (p3 < Child.m_Element_User.Count);
                                    }

                                    p1++;
                                } while (p1 < recent_Activity.m_Child.Count);
                            }
                            #endregion

                            #region Processanforderung zuweisen
                            if (recent_Activity.m_Process.Count != 0 && GUID_Requirement != "")
                            {
                                Repository_Connector repository_Connector = new Repository_Connector();
                                int p1 = 0;
                                do
                                {
                                    if (recent_Activity.m_Process[p1].Requirement_Process != null)
                                    { 
                                        if (recent_Activity.m_Process[p1].Requirement_Process.Classifier_ID != null)
                                        {
                                            repository_Connector.Create_Dependency(recent_Activity.m_Process[p1].Requirement_Process.Classifier_ID, GUID_Requirement, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, Repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                        }
                                    }
                                    p1++;
                                } while (p1 < recent_Activity.m_Process.Count);
                            }
                            #endregion

                            #region QualityRequirement zuweisen


                            #endregion

                            Create_Text_Afo_change();

                            //Einfärben
                            Check_For_Requirement();

                          
                        }
                    }
                    #endregion Afo im Repository anlegen


                    #region Activity updaten 
                    //Aktuell entfernt
              /*      TaggedValue tagged = new TaggedValue(this.Database.metamodel, this.Database);
                    //   tagged.Update_Tagged_Value(recent_Activity.Classifier_ID, "W_PROZESSWORT", w_prozesswort, null, Repository);

                    string w_object_parsed = w_Object.Replace(Database.metamodel.Pos_Qualitaet_Operator, "");
                    w_object_parsed = w_object_parsed.Replace("  ", " ");

                    //   tagged.Update_Tagged_Value(recent_Activity.Classifier_ID, "W_OBJEKT", w_object_parsed, null, Repository);

                    m_Insert.Clear();

                    m_Insert.Add(new DB_Insert("W_PROZESSWORT", OleDbType.VarChar, OdbcType.VarChar, w_prozesswort, -1));
                    m_Insert.Add(new DB_Insert("W_OBJEKT", OleDbType.VarChar, OdbcType.VarChar, w_object_parsed, -1));

                    recent_Activity.Update_TV(m_Insert, Database, Repository);


                    EA.Element element = Repository.GetElementByGuid(recent_Activity.Classifier_ID);
                    element.Name = w_prozesswort + " " + w_object_parsed;
                    element.Update();
              */
                    #endregion Activity updaten 

                    #region Stakeholder Updaten
                    if (flag_functional == 1 && recent_Stakeholder != null)
                    {
                        m_Insert.Clear();

                        m_Insert.Add(new DB_Insert("W_NUTZENDER", OleDbType.VarChar, OdbcType.VarChar, recent_Stakeholder_String, -1));
                        recent_Stakeholder.Update_TV(m_Insert, Database, Repository);

                        //tagged.Update_Tagged_Value(recent_Stakeholder.Classifier_ID, "W_NUTZENDER", recent_Stakeholder_String, null, Repository);
                    }
                    #endregion Stakeholder Updaten

                    //Repository.RefreshModelView(0);
                }
                
            }


        }

        #endregion Save_Button

        #region Änderungen Textbox Activities
        private void Text_Prozesswort_TextChanged(object sender, EventArgs e)
        {
            if (flag_reset == false)
            {
                flag_Prozesswort = true;

                if (Text_Prozesswort.Text == "")
                {
                    flag_Prozesswort = false;
                    Text_Prozesswort.Text = Text_Prozesswort_sel;
                }

                Create_Text_Afo_change();
            }

        }

        private void Text_Object_TextChanged(object sender, EventArgs e)
        {
            if (flag_reset == false)
            {
                flag_Objcet = true;

                Create_Text_Afo_change();
            }

        }

        private void Text_Qualitaet_TextChanged(object sender, EventArgs e)
        {
            if (flag_reset == false)
            {
                flag_Qualitaet = true;


                if (Text_Qualitaet.Text == "")
                {
                    Pos_Qualitaet = -1;
                    flag_Qualitaet = true;
                    //   Text_Qualitaet.Text = Text_Qualitaet_sel;
                    //   Text_Qualitaet.Refresh();
                }

                Create_Text_Afo_change();
            }
        }

        private void Text_Randbedingung_TextChanged(object sender, EventArgs e)
        {
            if (flag_reset == false)
            {
                flag_Randbedingung = true;

                //Parse_For_Qualitaet(this.Database.metamodel.Pos_Qualitaet_Operator, Pos_Qualitaet);
                Pos_Qualitaet = -1;

                Create_Text_Afo_change();
            }

        }
        #endregion Änderungen Textbox Activities

        #region String_Manipilation

        /// <summary>
        /// -1 ist die anzeige dass Box_Split ein #q enthält. andererseits wird mit pos die Position der Qulaität angegeben
        /// </summary>
        /// <param name="Operator"></param>
        /// <param name="Pos"></param>
        private void Parse_For_Qualitaet(string Operator, int Pos)
        {
            /*
            var help = Text_Afo.Text.Replace(Text_Qualitaet.Text, "");
            var Box_split = help.Split(' ');
            //var AFO_split = AFO_Text.Split(' ');

            AFO_Text = "";

            int i1 = 0;
            do
            {

                if (Box_split[i1] != Operator )
                {
                        if (i1 != 0)
                        {
                            AFO_Text = AFO_Text + " " + Box_split[i1];
                        }
                        else
                        {
                            AFO_Text = Box_split[i1];
                        }
                    
                  
                }
                if (Box_split[i1] == Operator)
                {
                    //Pos_Qualitaet = i1;
                    Pos_Qualitaet = AFO_Text.Length+1;

                    if (i1 != 0)
                    {
                        AFO_Text = AFO_Text + " " + Text_Qualitaet.Text;
                    }
                    else
                    {
                        AFO_Text = Text_Qualitaet.Text;
                    }
                }
               

                i1++;
            } while (i1 < Box_split.Length);
            */

            var help = Text_Afo.Text.Replace(Text_Qualitaet.Text + " ", "");
            var Text_split = help.Split(' ');
            var help2 = Text_split.ToList();
            string help3 = String.Join(" ", help2);

            int help4 = help3.IndexOf(Operator);

            if (help4 != -1)
            {
                Pos_Qualitaet = help4;

                AFO_Text = help3.Replace(Operator, Text_Qualitaet.Text);
            }
        }

        public void Change_Pos_Qualitaet(string w_qualitaet)
        {
            /*  if(Pos_Qualitaet != -1)
              {
                  var help = AFO_Text.Replace(Text_Qualitaet.Text+" ", "");
                  var Text_split = help.Split(' ');
                  var help2 = Text_split.ToList();
                  var help3 = String.Join(" ", help2);

                  AFO_Text = help3.Insert(Pos_Qualitaet, Text_Qualitaet.Text+" ");

              }
              */

            if (AFO_Text.Contains(Database.metamodel.Pos_Qualitaet_Operator) == true)
            {
                if (w_qualitaet != Text_Qualitaet_ini && w_qualitaet != Text_Qualitaet_sel && w_qualitaet.Length > 0)
                {
                    AFO_Text = AFO_Text.Replace(w_qualitaet, "");
                    AFO_Text = AFO_Text.Replace(Database.metamodel.Pos_Qualitaet_Operator, w_qualitaet);
                    AFO_Text = AFO_Text.Replace("  ", " ");
                }
                else
                {
                    AFO_Text = AFO_Text.Replace(Database.metamodel.Pos_Qualitaet_Operator, "");
                }

            }

        }


        #endregion String_Manipilation

        #region Capability

        private void Auswahl_Capability_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Content item for the combo box Capbility
        private class ComboBoxItem_Capability
        {
            public string Name;
            public Capability Value;
            public ComboBoxItem_Capability(string name, Capability value)
            {
                Name = name;
                Value = value;
            }
            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Name;
            }
        }
        // Content item for the combo box Stakeholder
        #endregion Capability


        #region Stakeholder
        private class ComboBoxItem_Stakeholder
        {
            public string Name;
            public Stakeholder Value;
            public ComboBoxItem_Stakeholder(string name, Stakeholder value)
            {
                Name = name;
                Value = value;
            }
            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Name;
            }
        }
        /// <summary>
        /// Auswahl des Stakeholders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auswahl_Stakeholder_SelectedIndexChanged(object sender, EventArgs e)
        {
        //   XML xML = new XML();
            TaggedValue tagged = new TaggedValue(this.Database.metamodel, this.Database);

            if(Auswahl_Stakeholder.SelectedIndex != null)
            {

                ComboBoxItem_Stakeholder recent = Auswahl_Stakeholder.Items[Auswahl_Stakeholder.SelectedIndex] as ComboBoxItem_Stakeholder;
                this.recent_Stakeholder = recent.Value;

                Stakeholder_Artikel.Enabled = true;
                Stakeholder_Artikel.Text = recent_Stakeholder.st_ARTIKEL;
                Stakeholder_Artikel.Refresh();

                /* string SQL = "SELECT NAME FROM t_object WHERE ea_guid = '"+recent.Value.Classifier_ID+"'";
                 string SQL_Dat = this.Repository.SQLQuery(SQL);
                 List<string> m_Name = xML.Xml_Read_Attribut("Name", SQL_Dat);
                 */
                //string m_Name = tagged.Get_Tagged_Value(recent.Value.Classifier_ID, "W_NUTZENDER", Repository); 
                string m_Name = recent.Value.w_NUTZENDER;

                if (m_Name != null)
                {
                    // recent_Stakeholder_String = m_Name[0];
                    recent_Stakeholder_String = m_Name;
                    Stakeholder_Artikel_Index = this.Database.metamodel.Artikel.FindIndex(x => x == recent_Stakeholder.st_ARTIKEL);

                    recent_Stakeholder_Artikel_string = this.Database.metamodel.Artikel[Stakeholder_Artikel_Index + 6];
                }
                else
                {
                    recent_Stakeholder_String = this.Database.metamodel.Stakeholder_Default;
                    Stakeholder_Artikel_Index = 0;
                    recent_Stakeholder_Artikel_string = this.Database.metamodel.Artikel[Stakeholder_Artikel_Index + 6];
                    this.recent_Stakeholder = null;
                }
                
            }
            else
            {
                recent_Stakeholder_String = this.Database.metamodel.Stakeholder_Default;
                Stakeholder_Artikel_Index = 0;
                recent_Stakeholder_Artikel_string = this.Database.metamodel.Artikel[Stakeholder_Artikel_Index + 6];
                this.recent_Stakeholder = null;
            }

           // Create_Text_Afo_change();
            
           // Check_For_Requirement();

        }

        private void Stakeholder_Artikel_Click(object sender, EventArgs e)
        {
            bool flag = true;
            List<DB_Insert> m_Insert = new List<DB_Insert>();

            if (recent_Stakeholder == null)
            {
                flag = false;
                Stakeholder_Artikel.Text = "...";
            }

            switch (Stakeholder_Artikel_Index)
            {
                case 0:
                    Stakeholder_Artikel.Text = this.Database.metamodel.Artikel[1];
                    Stakeholder_Artikel_Index++;
                    
                    break;
                case 1:
                    Stakeholder_Artikel.Text = this.Database.metamodel.Artikel[2];
                    Stakeholder_Artikel_Index++;
                    break;
                case 2:
                    Stakeholder_Artikel.Text = this.Database.metamodel.Artikel[0];
                    Stakeholder_Artikel_Index = 0;
                    break;
            }

            Stakeholder_Artikel.Refresh();

            if (flag == true)
            {
                recent_Stakeholder_Artikel_string = this.Database.metamodel.Artikel[Stakeholder_Artikel_Index + 6];
                //Artikel abspeichern für alle mit diesem NodeType
                TaggedValue Tagged = new TaggedValue(this.Database.metamodel, this.Database);

                m_Insert.Clear();
                m_Insert.Add(new DB_Insert("ST_ARTIKEL", OleDbType.VarChar, OdbcType.VarChar, this.Database.metamodel.Artikel[Stakeholder_Artikel_Index], -1));

                recent_Stakeholder.Update_TV(m_Insert, Database, Repository);
                //Tagged.Update_Tagged_Value(recent_Stakeholder.Classifier_ID, "ST_ARTIKEL", this.Database.metamodel.Artikel[Stakeholder_Artikel_Index], "Values: der, die, das", this.Repository);
                recent_Stakeholder.st_ARTIKEL = this.Database.metamodel.Artikel[Stakeholder_Artikel_Index];
                /////////////////////////////////////////
                //Artikel in AFO Text neu schreiben

                Create_Text_Afo_change();

                Check_For_Requirement();

            }
        }

        private void Auswahl_Stakeholder_TextChanged(object sender, EventArgs e)
        {
            recent_Stakeholder_String = this.Auswahl_Stakeholder.Text;

            Create_Text_Afo_change();

            Check_For_Requirement();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  List<DB_Insert> m_Insert = new List<DB_Insert>();

            if (recentTreeNode_Client != null && recentTreeNode_Activity != null)
            {
                ///////////////
                //Anforderung erstellen
                //Requirement_Functional requirement_Functional = new Requirement_Functional();
                NodeType recent_NodeType = recentTreeNode_Client.Tag as NodeType;
                Activity recent_Activity = null;

                if (flag_functional == 0)
                {
                    recent_Activity = recentTreeNode_Activity.Tag as Activity;
                }
                if (flag_functional == 1)
                {
                    recent_Activity = recentTreeNode_Activity.Tag as Activity;

                    recent_Element_User = recent_NodeType.m_Element_User.Where(x => x.Supplier == recent_Activity).ToList()[0];

                   // this.recent_Element_User = recentTreeNode_Activity.Tag as Element_User;
                   // recent_Activity = recent_Element_User.Supplier;
                }

                #region AFo im Repository anlegen
                if (recent_NodeType.m_Element_Functional.Count > 0 || recent_NodeType.m_Element_User.Count > 0)
                {

                    int i1 = 0;
                    #region Element_Functional
                    if (flag_functional == 0)
                    {
                        do
                        {
                            if (recent_NodeType.m_Element_Functional[i1].Supplier == recent_Activity)
                            {
                                string GUID_Requirement = "";
                                if (recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Count > 0)
                                {
                                    //Requirement_Functional requirement_Functional = new Requirement_Functional(label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name_t_object_GUID(Repository), W_ZU);
                                    Requirement copy = new Requirement(recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Classifier_ID, this.Database.metamodel);
                                    GUID_Requirement = recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Classifier_ID;

                                  //  copy.Get_Tagged_Values_From_Requirement(copy.Classifier_ID, Repository, Database);
                                  //  GUID_Requirement = recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Update_Requirement(Repository, Database, label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name(this.Database), W_ZU, null);

                                    // recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Compare_Requirement(Database, Repository, copy);

                                }
                              /*  else
                                {
                                    GUID_Requirement = requirement_Functional.Create_Requirement_funktional(Repository, Package_Funktionale.PackageGUID, Database.metamodel.m_Requirement_Functional[0].Stereotype, Database);
                                    recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Add(requirement_Functional);
                                }*/

                                Repository_Connector repository_Connector = new Repository_Connector();

                                if(GUID_Requirement != "")
                                {
                                    if (Auswahl_Capability.SelectedIndex != -1)
                                    {
                                        ComboBoxItem_Capability recent_comboitem = Auswahl_Capability.Items[Auswahl_Capability.SelectedIndex] as ComboBoxItem_Capability;

                                        recent_NodeType.m_Element_Functional[i1].Capability = null;
                                        recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].m_Capability = new List<Capability>();

                                        //Connecotor einfügen
                                        repository_Connector.Create_Dependency(GUID_Requirement, recent_comboitem.Value.Classifier_ID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);
                                        recent_NodeType.m_Element_Functional[i1].Capability = recent_comboitem.Value;

                                        if (recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].m_Capability.Contains(recent_comboitem.Value) == false)
                                        {
                                            recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].m_Capability.Add(recent_comboitem.Value);
                                        }
                                    }

                                    //  recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Add(requirement_Functional);

                                    if (recent_NodeType.m_Element_Functional[i1].m_Target_Functional.Count > 0)
                                    {
                                        int i2 = 0;
                                        do
                                        {

                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_Functional[i1].m_Target_Functional[i2].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_Functional[i1].m_Target_Functional[i2].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                            i2++;
                                        } while (i2 < recent_NodeType.m_Element_Functional[i1].m_Target_Functional.Count);
                                    }
                                    else
                                    {
                                        repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_Functional[i1].Client.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                        repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_Functional[i1].Supplier.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                    }
                                }

                              //  requirement_Functional.Classifier_ID = GUID_Requirement;

                                //Connecotr Capability anlegen
                               
                            }
                            i1++;
                        } while (i1 < recent_NodeType.m_Element_Functional.Count);
                    }
                    #endregion Element_Functional

                    #region Element User
                    if (flag_functional == 1)
                    {
                        do
                        {
                            if (recent_NodeType.m_Element_User[i1] == recent_Element_User)
                            {
                                string GUID_Requirement = "";
                                if (recent_NodeType.m_Element_User[i1].m_Requirement_User.Count > 0)
                                {
                                    //Requirement_Functional requirement_Functional = new Requirement_Functional(label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name_t_object_GUID(Repository), W_ZU);
                                    Requirement copy = new Requirement(recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Classifier_ID, this.Database.metamodel);

                                    GUID_Requirement = recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Classifier_ID;
                                 //   copy.Get_Tagged_Values_From_Requirement(copy.Classifier_ID, Repository, Database);

                                    //   GUID_Requirement = recent_NodeType.m_Element_User[i1].m_Requirement_User[0].Update_Requirement(Repository, Database, label_Afo.Text, Text_Afo.Text, w_Object, w_prozesswort, w_Qualitaet, w_Randebedingung, true, recent_NodeType.W_Artikel + " " + recent_NodeType.Get_Name(this.Database), W_ZU, W_NUTZENDER);

                                    //  recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional[0].Compare_Requirement(Database, Repository, copy);
                                }
                             /*   else
                                {
                                    GUID_Requirement = requirement_User.Create_Requirement_User(Repository, Package_Funktionale.PackageGUID, Database.metamodel.m_Requirement_User[0].Stereotype, Database);
                                    recent_NodeType.m_Element_User[i1].m_Requirement_User.Add(requirement_User);
                                }*/

                                Repository_Connector repository_Connector = new Repository_Connector();

                              //  requirement_User.Classifier_ID = GUID_Requirement;
                             
                                //Connecotr Capability anlegen
                                if (Auswahl_Capability.SelectedIndex != -1)
                                {
                                    ComboBoxItem_Capability recent_comboitem = Auswahl_Capability.Items[Auswahl_Capability.SelectedIndex] as ComboBoxItem_Capability;

                                    recent_NodeType.m_Element_User[i1].Capability = null;
                                    recent_NodeType.m_Element_User[i1].m_Requirement_User[0].m_Capability = new List<Capability>();

                                    //Connecotor einfügen
                                    repository_Connector.Create_Dependency(GUID_Requirement, recent_comboitem.Value.Classifier_ID, Database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Capability[0].SubType, Repository, Database, Database.metamodel.m_Derived_Capability[0].Toolbox, Database.metamodel.m_Derived_Capability[0].direction);
                                    recent_NodeType.m_Element_User[i1].Capability = recent_comboitem.Value;

                                    if (recent_NodeType.m_Element_User[i1].m_Requirement_User[0].m_Capability.Contains(recent_comboitem.Value) == false)
                                    {
                                        recent_NodeType.m_Element_User[i1].m_Requirement_User[0].m_Capability.Add(recent_comboitem.Value);
                                    }
                                }

                                //  recent_NodeType.m_Element_Functional[i1].m_Requirement_Functional.Add(requirement_Functional);

                                if (recent_NodeType.m_Element_User[i1].m_Target_User.Count > 0)
                                {
                                    int i2 = 0;
                                    do
                                    {
                                        bool flag_cd = false;

                                        if (recent_Stakeholder != null)
                                        {
                                            if (recent_NodeType.m_Element_User[i1].m_Target_User[i2].Check_Stakeholder(recent_Stakeholder.Classifier_ID) == true)
                                            {
                                                flag_cd = true;
                                            }
                                        }
                                        else
                                        {
                                            flag_cd = true;
                                        }

                                        if (flag_cd == true)
                                        {
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].m_Target_User[i2].CLient_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].m_Target_User[i2].Supplier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                            repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].m_Target_User[i2].Client_ST, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                        }

                                        i2++;
                                    } while (i2 < recent_NodeType.m_Element_User[i1].m_Target_User.Count);
                                }
                                else
                                {
                                    repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].Client.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);
                                    repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].Supplier.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                    if (recent_Stakeholder != null)
                                    {
                                        repository_Connector.Create_Dependency(GUID_Requirement, recent_Stakeholder.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                    }
                                    else
                                    {
                                        if (recent_NodeType.m_Element_User[i1].m_Client_ST.Count > 0)
                                        {
                                            int i3 = 0;
                                            do
                                            {
                                                repository_Connector.Create_Dependency(GUID_Requirement, recent_NodeType.m_Element_User[i1].m_Client_ST[i3].Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, Repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                                                i3++;
                                            } while (i3 < recent_NodeType.m_Element_User[i1].m_Client_ST.Count);
                                        }

                                    }


                                }
                            }
                            i1++;
                        } while (i1 < recent_NodeType.m_Element_User.Count);
                    }
                    #endregion Element User
                    Create_Text_Afo_change();

                    //Einfärben
                    Check_For_Requirement();

                    #endregion Afo im Repository anlegen
                }
            }
        }

        #endregion Stakeholder

        private void label_Afo_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void checkBox_logical_elements_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBox_logical_elements.Checked == true)
            {
                Database.metamodel.flag_sysarch = false;

                //Alle Trees löschen
                this.Client_Tree.Nodes.Clear();
                this.Supplier_Tree.Nodes.Clear();
                //Client Tree aufbauen
                this.Create_Client_Tree();

            }
            else
            {
                Database.metamodel.flag_sysarch = true;

                //Alle Trees löschen
                this.Client_Tree.Nodes.Clear();
                this.Supplier_Tree.Nodes.Clear();
                //Client Tree aufbauen
                this.Create_Client_Tree();
            }
        }

        private void Text_Afo_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
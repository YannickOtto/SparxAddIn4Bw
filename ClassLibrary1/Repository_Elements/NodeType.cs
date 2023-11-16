using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;
using System.Data.Odbc;

using Database_Connection;
using Metamodels;
using Elements;
using Requirement_Plugin;
using Ennumerationen;
using Requirements;

namespace Repsoitory_Elements
{
    public class NodeType : Repository_Class 
    {
        
        public List<Element_Interface> m_Element_Interface;
        public List<Element_Interface_Bidirectional> m_Element_Interface_Bidirectional;
        
        
        public List<NodeType> m_Parent;
        public List<NodeType> m_Child;
        public List<Pool> m_Pools;
        public List<Pool> m_Lanes;
        public List<SysElement> m_ImplementedBy;
       // public List<string> m_GUID_Inst;
       
       
		public List<Element_Functional> m_Element_Functional;
		public List<Element_User> m_Element_User;
        public List<Stakeholder> m_Stakeholder;
        public List<Element_Design> m_Design;
        public List<Element_Environmental> m_Enviromental;
        public List<Element_Typvertreter> m_Typvertreter;
        public List<NodeType> m_Specialize = new List<NodeType>();

        public List<Requirement> m_requirements_all = new List<Requirement>();


        public List<Element_Measurement> m_Element_Measurement;// = new List<Element_Measurement>();
        public List<Element_Measurement> m_Element_Measurement_Activity;
        //    public List<Element_Process> m_element_Processes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Classifier_ID"></param>
        /// <param name="Repository"></param>
        /// <param name="Data"></param>
        public NodeType(string Classifier_ID, EA.Repository Repository, Database Data) 
        {

            // this.m_Element_Interface = GetElement_Interfaces(Classifier_ID, Repository, Database);
            this.m_Element_Interface = new List<Element_Interface>();
            this.m_Element_Interface_Bidirectional = new List<Element_Interface_Bidirectional>();
            //this.W_Artikel = "der";
            this.m_Parent = new List<NodeType>();
            this.m_Child = new List<NodeType>();
            this.m_ImplementedBy = new List<SysElement>();
            this.Instantiate_GUID = null;
           // this.Parent = null;
            this.m_Element_Functional = new List<Element_Functional>();
            this.m_Element_User = new List<Element_User>();
            this.m_Stakeholder = new List<Stakeholder>();
            this.m_Instantiate = new List<string>();
            this.m_Design = new List<Element_Design>();
            this.m_Enviromental = new List<Element_Environmental>();
            this.m_Typvertreter = new List<Element_Typvertreter>();
        //    this.m_element_Processes = new List<Element_Process>();
            this.m_Pools = new List<Pool>();
            this.m_Lanes = new List<Pool>();
            // this.m_GUID_Inst = new List<string>();
             this.m_Element_Measurement = new List<Element_Measurement>();
            this.m_requirements_all = new List<Requirement>();

            if (Classifier_ID != null && Classifier_ID != "")
            {
                this.Classifier_ID = Classifier_ID;
                this.ID = this.Get_Object_ID(Data);
                this.Author = this.Get_Author(Data);
                this.Name = this.Get_Name(Data);
                this.Notes = this.Get_Notes( Data);
                this.StereoType = null;
                this.Type = null;
                this.m_Instantiate = new List<string>();
                /* if(this.m_Instantiate.Contains(Classifier_ID) == false)
                 {
                     this.m_Instantiate.Add(Classifier_ID);
                 }*/



                if (Classifier_ID != null)
                {
                    EA.Element recent = Repository.GetElementByGuid(Classifier_ID);

                    if(recent != null)
                    {
                       
                        //Tagged Values erhalten
                        this.Get_TV(Data, Repository);

                    }

                }
            }

           

        }

        ~NodeType()
        {

        }

        #region TV
        public void Get_TV(Database Data, EA.Repository Repository)
        {
            TV_Map help;

            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            List<DB_Insert> m_Insert = new List<DB_Insert>();

            // string SQL = "SELECT Value, Notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
            //  string[] output = { "Value", "Notes" };

            //   OleDbCommand oleDbCommand = new OleDbCommand(SQL, Data.oLEDB_Interface.dbConnection);
            //  List<DB_Return> m_TV = Data.oLEDB_Interface.oleDB_SELECT_One_Table_Multiple_Property(oleDbCommand, output, Data.metamodel.SYS_Tagged_Values, this.ID);

            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();

            List<DB_Return> m_TV = interface_TaggedValue.Get_Tagged_Value(Data.metamodel.SYS_Tagged_Values, this.ID, Data);

            if (m_TV != null)
            {
                string Value = "";
                string Note = "";
                string insert = "";
                bool flag_insert = false;
                int i1 = 0;
                do
                {
                    if (m_TV[i1].Ret.Count > 1)
                    {
                        string recent = "";
                        if (m_TV[i1].Ret[1] == null && m_TV[i1].Ret[2] == null)
                        {
                            flag_insert = true;
                        }
                        /*    if (m_TV[i1].Ret[1] == "" && m_TV[i1].Ret[2] == "")
                            {
                                flag_insert = true;
                            }*/
                        else
                        {
                            if (m_TV[i1].Ret[1].ToString() != "<memo>")
                            {
                                recent = (string)m_TV[i1].Ret[1];
                            }
                            else
                            {
                                recent = (string)m_TV[i1].Ret[2];
                            }

                            switch (m_TV[i1].Ret[0])
                            {
                                case "SYS_ANSPRECHPARTNER":
                                    this.SYS_ANSPRECHPARTNER = recent;
                                    this.Author = this.SYS_ANSPRECHPARTNER;
                                    break;
                                case "UUID":
                                    this.UUID = recent;
                                    break;
                                case "OBJECT_ID":
                                    this.OBJECT_ID = recent;
                                    break;
                                case "SYS_AG_ID":
                                    this.SYS_AG_ID = recent;
                                    break;
                                case "SYS_AN_ID":
                                    this.SYS_AN_ID = recent;
                                    break;
                                case "SYS_KUERZEL":
                                    this.SYS_KUERZEL = recent;
                                    this.Name = recent;
                                    break;
                                case "SYS_BEZEICHNUNG":
                                    this.SYS_BEZEICHNUNG = recent;
                                    this.Notes = recent;
                                    break;
                                case "SYS_ARTIKEL":
                                    help = Data.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_ARTIKEL");
                                    if (recent != help.Default_Value)
                                    {
                                        this.SYS_ARTIKEL = (SYS_ARTIKEL)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_ARTIKEL, recent);
                                        this.W_Artikel = recent;
                                    }
                                    else
                                    {
                                        this.SYS_ARTIKEL = (SYS_ARTIKEL)0;
                                        this.W_Artikel = Data.SYS_ENUM.SYS_ARTIKEL[0];
                                    }                         
                                    break;
                                case "SYS_DETAILSTUFE":
                                    help = Data.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_DETAILSTUFE");
                                    if (recent != help.Default_Value)
                                    {
                                        this.SYS_DETAILSTUFE = (SYS_DETAILSTUFE)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_DETAILSTUFE, recent);
                                    }
                                    else
                                    {
                                        this.SYS_DETAILSTUFE = (SYS_DETAILSTUFE)0;
                                    }
                                    break;
                                case "SYS_TYP":
                                    help = Data.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_TYP");
                                    if (recent != help.Default_Value)
                                    {
                                        this.SYS_TYP = (SYS_TYP)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_TYP, recent);
                                    }
                                    else
                                    {
                                        this.SYS_TYP = (SYS_TYP)0;
                                    }
                                   break;
                                case "SYS_ERFUELLT_AFO":
                                    help = Data.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_ERFUELLT_AFO");
                                    if (recent != help.Default_Value)
                                    {
                                        this.SYS_ERFUELLT_AFO = (SYS_ERFUELLT_AFO)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_ERFUELLT_AFO, recent);
                                    }
                                    else
                                    {
                                        this.SYS_ERFUELLT_AFO = (SYS_ERFUELLT_AFO)0;
                                    }
                                    break;
                                case "SYS_SUBORDINATES_AFO":
                                    help = Data.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_SUBORDINATES_AFO");
                                    if (recent != help.Default_Value)
                                    {
                                        this.SYS_SUBORDINATES_AFO = (SYS_SUBORDINATES_AFO)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_SUBORDINATES_AFO, recent);
                                    }
                                    else
                                    {
                                        this.SYS_SUBORDINATES_AFO = (SYS_SUBORDINATES_AFO)0;
                                    }
                                    break;
                                case "SYS_KOMPONENTENTYP":
                                    help = Data.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_KOMPONENTENTYP");
                                    if (recent != help.Default_Value)
                                    {
                                        this.SYS_KOMPONENTENTYP = (SYS_KOMPONENTENTYP)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_KOMPONENTENTYP, recent);
                                    }
                                    else
                                    {
                                        this.SYS_KOMPONENTENTYP = (SYS_KOMPONENTENTYP)0;
                                    }
                                     break;
                                case "SYS_STATUS":
                                    help = Data.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_STATUS");
                                    if (recent != help.Default_Value)
                                    {
                                        this.SYS_STATUS = (SYS_STATUS)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_STATUS, recent);
                                    }
                                    else
                                    {
                                        this.SYS_STATUS = (SYS_STATUS)0;
                                    }
                                    break;
                                case "IN_CATEGORY":
                                    help = Data.metamodel.AFO_Tagged_Values.Find(x => x.Name == "IN_CATEGORY");
                                    if (recent != help.Default_Value)
                                    {
                                        this.IN_CATEGORY = (IN_CATEGORY)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.IN_CATEGORY, recent);
                                    }
                                    else
                                    {
                                        this.IN_CATEGORY = (IN_CATEGORY)0;
                                    }
                                    break;
                                case "SYS_PRODUKT_STATUS":
                                    help = Data.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_PRODUKT_STATUS");
                                    if (recent != help.Default_Value)
                                    {
                                        this.SYS_PRODUKT_STATUS = (SYS_PRODUKT_STATUS)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_PRODUKT_STATUS, recent);
                                    }
                                    else
                                    {
                                        this.SYS_PRODUKT_STATUS = (SYS_PRODUKT_STATUS)0;
                                    }
                                    break;
                                case "SYS_PRODUKT":
                                    this.SYS_PRODUKT = recent;
                                    break;
                                case "B_KENNUNG":
                                    this.B_KENNUNG = recent;
                                    break;
                                case "SYS_REL_GEWICHT":
                                    this.SYS_REL_GEWICHT = recent;
                                    break;
                                case "AFO_KLAERUNGSPUNKTE":
                                    this.AFO_KLAERUNGSPUNKTE = recent;
                                    break;
                                case "TagMask":
                                    this.TagMask = recent;
                                    break;
                                case "RPI_Export":
                                    if(recent == "False" || recent == "false")
                                    {
                                        this.RPI_Export = false;
                                    }
                                    else
                                    {
                                        this.RPI_Export = true;
                                    }
                                    break;
                                case "W_SINGULAR":
                                    if (recent == "False" || recent == "false")
                                    {
                                        this.W_SINGULAR = false;
                                    }
                                    else
                                    {
                                        this.W_SINGULAR = true;
                                    }
                                    break;

                            }



                        }

                    }
                    else
                    {
                        flag_insert = true;
                    }


                    if (flag_insert == true)
                    {
                        switch (m_TV[i1].Ret[0])
                        {
                            case "SYS_ANSPRECHPARTNER":
                                this.SYS_ANSPRECHPARTNER = this.Author;
                                m_Insert.Add(new DB_Insert("SYS_ANSPRECHPARTNER", OleDbType.VarChar, OdbcType.VarChar, this.Author, -1));
                                break;
                            case "UUID":
                                string UUID = Classifier_ID;
                                UUID = UUID.Trim('{', '}');
                                this.UUID = UUID;
                                m_Insert.Add(new DB_Insert("UUID", OleDbType.VarChar, OdbcType.VarChar, UUID, -1));
                                break;
                            case "OBJECT_ID":
                                this.OBJECT_ID = this.ID.ToString();
                                m_Insert.Add(new DB_Insert("OBJECT_ID", OleDbType.VarChar, OdbcType.VarChar, this.ID.ToString(), -1));
                                break;
                            case "SYS_AG_ID":
                                this.SYS_AG_ID = "kein";
                                m_Insert.Add(new DB_Insert("SYS_AG_ID", OleDbType.VarChar, OdbcType.VarChar, "kein", -1));
                                break;
                            case "SYS_AN_ID":
                                this.SYS_AN_ID = "kein";
                                m_Insert.Add(new DB_Insert("SYS_AN_ID", OleDbType.VarChar, OdbcType.VarChar, "kein", -1));
                                break;
                            case "SYS_KUERZEL":
                                this.SYS_KUERZEL = this.Name;
                                m_Insert.Add(new DB_Insert("SYS_KUERZEL", OleDbType.VarChar, OdbcType.VarChar, this.Name, -1));
                                break;
                            case "SYS_BEZEICHNUNG":
                                this.SYS_BEZEICHNUNG = this.Name;
                                m_Insert.Add(new DB_Insert("SYS_BEZEICHNUNG", OleDbType.VarChar, OdbcType.VarChar, this.Notes, -1));
                                break;
                            case "SYS_ARTIKEL":
                                this.SYS_ARTIKEL = SYS_ARTIKEL.der;
                                this.W_Artikel = Data.SYS_ENUM.SYS_ARTIKEL[(int)SYS_ARTIKEL.der];
                                m_Insert.Add(new DB_Insert("SYS_ARTIKEL", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_ARTIKEL[(int)SYS_ARTIKEL.der], -1));
                                break;
                            case "SYS_DETAILSTUFE":
                                this.SYS_DETAILSTUFE = SYS_DETAILSTUFE.unbestimmt;
                                m_Insert.Add(new DB_Insert("SYS_DETAILSTUFE", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_DETAILSTUFE[(int)SYS_DETAILSTUFE.unbestimmt], -1));
                                break;
                            case "SYS_TYP":
                                this.SYS_TYP = SYS_TYP.TechnischesSystem;
                                m_Insert.Add(new DB_Insert("SYS_TYP", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_TYP[(int)SYS_TYP.TechnischesSystem], -1));
                                break;
                            case "SYS_ERFUELLT_AFO":
                                this.SYS_ERFUELLT_AFO = SYS_ERFUELLT_AFO.True;
                                m_Insert.Add(new DB_Insert("SYS_ERFUELLT_AFO", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_ERFUELLT_AFO[(int)SYS_ERFUELLT_AFO.True], -1));
                                break;
                            case "SYS_SUBORDINATES_AFO":
                                this.SYS_SUBORDINATES_AFO = SYS_SUBORDINATES_AFO.Sys_subordinates_Afo_true;
                                m_Insert.Add(new DB_Insert("SYS_SUBORDINATES_AFO", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_SUBORDINATES_AFO[(int)SYS_SUBORDINATES_AFO.Sys_subordinates_Afo_true], -1));
                                break;
                            case "SYS_KOMPONENTENTYP":
                                string Typ = Data.metamodel.Find_Sys_KomponentenTyp(Data, this, Repository);
                                this.SYS_KOMPONENTENTYP = (SYS_KOMPONENTENTYP)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_KOMPONENTENTYP, Typ);
                                m_Insert.Add(new DB_Insert("SYS_KOMPONENTENTYP", OleDbType.VarChar, OdbcType.VarChar, Typ, 0));
                                break;
                            case "SYS_STATUS":
                                this.SYS_STATUS = SYS_STATUS.geplant;
                                m_Insert.Add(new DB_Insert("SYS_STATUS", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_STATUS[(int)SYS_STATUS.geplant], -1));
                                break;
                            case "IN_CATEGORY":
                                this.IN_CATEGORY = IN_CATEGORY.Null;
                                m_Insert.Add(new DB_Insert("IN_CATEGORY", OleDbType.VarChar, OdbcType.VarChar, Data.AFO_ENUM.IN_CATEGORY[(int)IN_CATEGORY.Null], -1));
                                break;
                            case "SYS_PRODUKT_STATUS":
                                this.SYS_PRODUKT_STATUS = SYS_PRODUKT_STATUS.unbestimmt;
                                m_Insert.Add(new DB_Insert("SYS_PRODUKT_STATUS", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_PRODUKT_STATUS[(int)SYS_PRODUKT_STATUS.unbestimmt], -1));
                                break;
                            case "SYS_PRODUKT":
                                this.SYS_PRODUKT = "kein";
                                m_Insert.Add(new DB_Insert("SYS_PRODUKT", OleDbType.VarChar, OdbcType.VarChar, "kein", -1));
                                break;
                            case "B_KENNUNG":
                                this.B_KENNUNG = "kein";
                                m_Insert.Add(new DB_Insert("B_KENNUNG", OleDbType.VarChar, OdbcType.VarChar, "kein", -1));
                                break;
                            case "SYS_REL_GEWICHT":
                                this.SYS_REL_GEWICHT = "kein";
                                m_Insert.Add(new DB_Insert("SYS_REL_GEWICHT", OleDbType.VarChar, OdbcType.VarChar, "kein", -1));
                                break;
                            case "RPI_Export":
                                this.RPI_Export = true;
                                m_Insert.Add(new DB_Insert("RPI_Export", OleDbType.VarChar, OdbcType.VarChar, "True", -1));
                                break;
                            case "AFO_KLAERUNGSPUNKTE":
                                this.AFO_KLAERUNGSPUNKTE = "";
                                m_Insert.Add(new DB_Insert("AFO_KLAERUNGSPUNKTE", OleDbType.VarChar, OdbcType.VarChar, "", -1));
                                break;
                            case "TagMask":
                                this.TagMask = "0";
                                m_Insert.Add(new DB_Insert("TagMask", OleDbType.VarChar, OdbcType.VarChar, "0", -1));
                                break;
                            case "W_SINGULAR":
                                this.RPI_Export = true;
                                m_Insert.Add(new DB_Insert("W_SINGULAR", OleDbType.VarChar, OdbcType.VarChar, "True", -1));
                                break;
                        }

                    }

                    i1++;
                } while (i1 < m_TV.Count);

                if (flag_insert == true)
                {
                    //Insert Befehl TV
                    string[] m_Input_Property = { "Object_ID", "Property", "Value", "Notes", "ea_guid" };
                    interface_TaggedValue.Insert_Tagged_Value(Data, m_Insert, tagged, this.ID, m_Input_Property);

                    //Data.oLEDB_Interface.OLEDB_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, "t_objectproperties", this.ID, m_Input_Property);
                }
            }
        }

       
        #endregion TV



        #region Get
        public void Check_Generalization(Database database)
        {

            List<string> m_Type = new List<string>();
            m_Type = database.metamodel.m_Elements_OpArch_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype = new List<string>();
            m_Stereotype = database.metamodel.m_Elements_OpArch_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_con = new List<string>();
            m_Type_con = database.metamodel.m_General_Class.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = new List<string>();
            m_Stereotype_con = database.metamodel.m_General_Class.Select(x => x.Stereotype).ToList();

            List<string> m_guid = this.Get_Generalization(database, m_Type, m_Stereotype, m_Type_con, m_Stereotype_con);

            if (m_guid != null)
            {
                int i1 = 0;
                do
                {
                    List<NodeType> m_act = database.m_NodeType.Where(x => x.Classifier_ID == m_guid[i1]).ToList();

                    if (m_act.Count > 0)
                    {
                        if (this.m_Specialize.Contains(m_act[0]) == false)
                        {
                            this.m_Specialize.Add(m_act[0]);
                        }
                    }

                    i1++;
                } while (i1 < m_guid.Count);

            }
        }
        public void Get_Children_Part(string GUID_Part, Database database, EA.Repository repository)
        {
            Repository_Element repository_element = new Repository_Element();

            List<string> m_Type = database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype = database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            repository_element.Classifier_ID = GUID_Part;
            List<string> m_Children_GUID = repository_element.Get_Children_Guid(database, m_Type, m_Stereotype);

            if (m_Children_GUID != null)
            {
                int i1 = 0;
                do
                {
                    repository_element.Classifier_ID = m_Children_GUID[i1];
                    string Classifier = repository_element.Get_Classifier(database);

                    if (Classifier != null)
                    {
                        NodeType recent_nt = new NodeType(Classifier, repository, database);
                        recent_nt.ID = repository_element.Get_Object_ID(database);
                        recent_nt.Instantiate_GUID = m_Children_GUID[i1];
                        recent_nt.Get_TV_Instantiate(database, repository);

                        recent_nt.Get_Children_Part(m_Children_GUID[i1], database, repository);

                        this.m_Child.Add(recent_nt);
                    }



                    i1++;
                } while (i1 < m_Children_GUID.Count);
            }
            //   return (Logical_all);


        }
        public void Get_Children_Class(Database database, EA.Repository repository)
        {
            Interface_Connectors interface_Connectors = new Interface_Connectors();
            List<string> m_Child_GUID = new List<string>();
            //Alle Client erhalten, welche aktuelles Element als Zeil haben
            List<string> m_Supplier_GUID = new List<string>();
            m_Supplier_GUID.Add(this.Classifier_ID);
            List<string> m_Type_Client = database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Client = database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_con = database.metamodel.m_Decomposition_Element.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = database.metamodel.m_Decomposition_Element.Select(x => x.Stereotype).ToList();

           

            m_Child_GUID = interface_Connectors.Get_Client_Element_By_Connector(database, m_Supplier_GUID, m_Type_Client, m_Stereotype_Client, m_Type_con, m_Stereotype_con);

            if(m_Child_GUID != null)
            {
              // MessageBox.Show("Kinderelemente: " + m_Child_GUID.Count);
                if(m_Child_GUID.Count > 0) //Kinderelemente vorhanden
                {
                    int i1 = 0;
                    do
                    {
                        //Classifier des Kinderelements suchen
                        List<NodeType> m_NodeType_Child = database.m_NodeType.Where(x => x.Classifier_ID == m_Child_GUID[i1]).ToList();

                        if(m_NodeType_Child.Count > 0) //Classifier vorhanden, wenn nicht ist es ein ungültiger Classifier
                        {

                            if(this.m_Child.Contains(m_NodeType_Child[0]) == false)
                            {
                                this.m_Child.Add(m_NodeType_Child[0]);
                                m_NodeType_Child[0].m_Parent.Add(this);
                            }                     
                        }



                        i1++;
                    } while (i1 < m_Child_GUID.Count);

                }

            }
           


        }

        public List<NodeType> Get_All_Children()
        {
            List<NodeType> m_ret = new List<NodeType>();

            if(this.m_Child.Count > 0)
            {
                m_ret = this.m_Child;

                int i1 = 0;
                do
                {
                    List<NodeType> m_help = new List<NodeType>();
                    m_help = this.m_Child[i1].Get_All_Children();

                    if(m_help != null)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < this.m_Child.Count);

                return (m_ret);

            }
            else
            {
                return (null);
            }

        }

        public List<NodeType> Get_All_Specifies()
        {
            List<NodeType> m_ret = new List<NodeType>();

            if (this.m_Specialize.Count > 0)
            {
                m_ret = this.m_Specialize;

                int i1 = 0;
                do
                {
                    List<NodeType> m_help = new List<NodeType>();
                    m_help = this.m_Specialize[i1].Get_All_Specifies();

                    if (m_help != null)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < this.m_Specialize.Count);

                return (m_ret);

            }
            else
            {
                return (null);
            }

        }

        public List<NodeType> Get_All_SpecifiedBy(List<NodeType> m_NodeType)
        {
            List<NodeType> m_ret = new List<NodeType>();

            List<NodeType> m_help_General = m_NodeType.Where(x => x.m_Specialize.Contains(this) == true).ToList();


            if (m_help_General.Count > 0)
            {
                m_ret = m_help_General;

                int i1 = 0;
                do
                {
                    List<NodeType> m_help = new List<NodeType>();
                    m_help = m_help_General[i1].Get_All_SpecifiedBy(m_NodeType);

                    if (m_help != null)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < m_help_General.Count);

                return (m_ret);

            }
            else
            {
                return (null);
            }

        }

        public List<Activity> Get_Process_Activity()
        {
            List<Activity> m_ret = new List<Activity>();

            m_ret = this.m_Element_Functional.Select(x => x.Supplier).ToList();
            m_ret.AddRange(this.m_Element_User.Select(x => x.Supplier).ToList());
            m_ret.Distinct().ToList();

            return (m_ret);
        }

        public List<OperationalConstraint> Get_OpCon_Deisgn()
        {
            return (this.m_Design.Select(x => x.OpConstraint).ToList());
        }


        public List<NodeType> Get_Typvertreter()
        {
            return (this.m_Typvertreter.Select(x => x.Typvertreter).ToList());
        }

        public List<Requirement_Plugin.Repository_Elements.Measurement> Get_Measurement()
        {
            return (this.m_Element_Measurement.Select(x => x.Measurement).ToList());
        }

        public List<OperationalConstraint> Get_Umwelt()
        {
            return (this.m_Enviromental.Select(x => x.OpConstraint).ToList());
        }
       
        public List<Requirement> Get_Requirements(Database database, EA.Repository repository)
        {
            List<Requirement> requirements = new List<Requirement>();

            Interface_Connectors_Requirement interface_Connectors = new Interface_Connectors_Requirement();
            //Client mit DerivedFrom erhalten
            List<string> m_Type = database.metamodel.m_Requirement.Select(x => x.Type).ToList();
            List<string> m_Stereotype = database.metamodel.m_Requirement.Select(x => x.Stereotype).ToList();
            List<string> m_Type_con = database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

            if(this.m_Instantiate.Contains(this.Classifier_ID) == false)
            {
                this.m_Instantiate.Add(this.Classifier_ID);
            }

            List<string> m_GUID = interface_Connectors.Get_Client_Element_By_Connector(database, this.m_Instantiate, m_Type , m_Stereotype, m_Type_con, m_Stereotype_con, database.metamodel.m_Derived_Element[0].direction);

            if(m_GUID.Count > 0)
            {
                List<string> m_req_guid = database.m_Requirement.Select(x => x.Classifier_ID).ToList();

                int i1 = 0;
                do
                {
                    if(m_req_guid.Contains(m_GUID[i1]) == false)
                    {
                        Requirement recent_req = new Requirement(m_GUID[i1], database.metamodel);
                        recent_req.Get_RPI_Export(database);
                        if(recent_req.RPI_Export == true)
                        {
                            recent_req.Get_Tagged_Values_From_Requirement(m_GUID[i1], repository, database);
                            database.m_Requirement.Add(recent_req);
                            recent_req.Get_Logical(database);

                            if (this.m_requirements_all.Contains(recent_req) == false)
                            {
                              
                                this.m_requirements_all.Add(recent_req);
                            }   
                        }
                        
                    }
                    else
                    {
                        if (this.m_requirements_all.Select(x => x.Classifier_ID).ToList().Contains(m_GUID[i1]) ==false)
                        {
                            Requirement recent_req = new Requirement(m_GUID[i1], database.metamodel);
                            recent_req.Get_RPI_Export(database);
                            if (recent_req.RPI_Export == true)
                            {
                                recent_req.Get_Tagged_Values_From_Requirement(m_GUID[i1], repository, database);
                                recent_req.Get_Logical(database);
                                //database.m_Requirement.Add(recent_req);
                                this.m_requirements_all.Add(recent_req);

                            }
                        }
                    }

                    i1++;
                } while (i1 < m_GUID.Count);
            }


            return (requirements);
        }
        
        public void Get_Measurements_Class(Database database)
        {
            List<Requirement_Plugin.Repository_Elements.Measurement> m_measurement_class = this.Get_Measurement(this.Classifier_ID, database);
            //Prüfen vorhanden
            if (m_measurement_class != null)
            {
                int i2 = 0;
                do
                {
                    List<Element_Measurement> m_test = this.m_Element_Measurement.Where(x => x.Measurement.Classifier_ID == m_measurement_class[i2].Classifier_ID).ToList();

                    if (m_test.Count == 0) //Neu anlegen
                    {
                        Element_Measurement elem_mes = new Element_Measurement(m_measurement_class[i2], database);
                        elem_mes.m_guid_Instanzen.Add(this.Classifier_ID);
                        this.m_Element_Measurement.Add(elem_mes);
                    }
                    else //erweitern
                    {
                        if(m_test[0].m_guid_Instanzen.Contains(this.Classifier_ID) == false)
                        {
                            m_test[0].m_guid_Instanzen.Add(this.Classifier_ID);
                        }
                    }

                        i2++;
                } while (i2 < m_measurement_class.Count);
            }

           // List<Element_Measurement> m_eleme_measure = this.m_NodeType[i1].m_element_measure.Where(x => m_measurement_class.Contains(x.Measurement) == true).ToList();


        }


        public void Get_Measurements_Instanzen(Database database)
        {
            if(this.m_Instantiate.Count > 0)
            {
                int i2 = 0;
                do
                {
                    List<Requirement_Plugin.Repository_Elements.Measurement> m_measurement_class = this.Get_Measurement(this.m_Instantiate[i2], database);

                    //Prüfen vorhanden
                    if (m_measurement_class != null)
                    {
                        int i3 = 0;
                        do
                        {
                            List<Element_Measurement> m_test = this.m_Element_Measurement.Where(x => x.Measurement.Classifier_ID == m_measurement_class[i3].Classifier_ID).ToList();

                            if (m_test.Count == 0) //Neu anlegen
                            {
                                Element_Measurement elem_mes = new Element_Measurement(m_measurement_class[i3], database);
                                elem_mes.m_guid_Instanzen.Add(this.m_Instantiate[i2]);
                                this.m_Element_Measurement.Add(elem_mes);
                            }
                            else //erweitern
                            {
                                if (m_test[0].m_guid_Instanzen.Contains(this.m_Instantiate[i2]) == false)
                                {
                                    m_test[0].m_guid_Instanzen.Add(this.m_Instantiate[i2]);
                                }
                            }

                            i3++;
                        } while (i3 < m_measurement_class.Count);
                    }

                    i2++;
                } while (i2 < this.m_Instantiate.Count);
            }

         

            // List<Element_Measurement> m_eleme_measure = this.m_NodeType[i1].m_element_measure.Where(x => m_measurement_class.Contains(x.Measurement) == true).ToList();


        }
        #endregion Get

        #region Get Element_Interface
        /// <summary>
        /// Connectoren erhalten
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Element_GUID"></param>
        /// <param name="Logical"></param>
        /// <param name="node"></param>
        /// <param name="flag_NT"></param>
        /// <param name="LA_Logical"></param>
        public void Get_Connectors_InfoEx(bool Logical, Database Data, EA.Repository repository)
        {
            #region UML
            List<string> m_Type_Con = new List<string>();
            List<string> m_Stereotype_Con = new List<string>();

            m_Type_Con = Data.metamodel.m_Infoaus.Select(x => x.Type).ToList();
            m_Stereotype_Con = Data.metamodel.m_Infoaus.Select(x => x.Stereotype).ToList();

            Repository_Connectors repository_Connectors = new Repository_Connectors();
            List<string> Connector_GUID = repository_Connectors.Get_Connectors_Element(this.m_Instantiate, Data, m_Type_Con, m_Stereotype_Con);
           
            //aktuelles Element besitzt ausgehende InfoExConnectoren
            if (Connector_GUID != null)
            {
                int i1 = 0;
                do
                {
                    NodeType Supplier = Data.Check_Supplier_NodeType_2(repository, Connector_GUID[i1], this);

                    if (Supplier != null)
                    {
                        //Check ob Element_Interface vorhanden
                        Element_Interface elem_interface = this.Check_Element_Interface(Supplier);
                        if (elem_interface == null) //nicht vorhanden
                        {
                            //  MessageBox.Show("Neues Element Interface anlegen");
                            Element_Interface elem_interface2 = new Element_Interface(this.Classifier_ID, Supplier.Classifier_ID);
                            elem_interface2.Client = this;
                            elem_interface2.Supplier = Supplier;

                            this.m_Element_Interface.Add(elem_interface2);

                            elem_interface = elem_interface2;
                        }
                        EA.Connector Connector = repository.GetConnectorByGuid(Connector_GUID[i1]);

                        Target target = elem_interface.Check_Target(repository.GetElementByID(Connector.ClientID).ElementGUID, repository.GetElementByID(Connector.SupplierID).ElementGUID);

                        if (target == null) // Target nicht vorhanden
                        {
                            Target target2 = new Target(repository.GetElementByID(Connector.ClientID).ElementGUID, repository.GetElementByID(Connector.SupplierID).ElementGUID, Data);
                            List<InformationElement> InfoElm1 = target2.Get_Information_Element_Logical(repository, Connector.ConnectorGUID, target2.Logical, Data);

                            if (InfoElm1 != null)
                            {
                                target2.m_Information_Element.AddRange(InfoElm1);
                            }

                            List<List<Requirement_Interface>> m_Requ_List = target2.Check_Requirement_Interface(repository, Data, true, Data.metamodel.m_Prozesswort_Interface[0]);

                            if (m_Requ_List != null)
                            {
                                int j1 = 0;
                                do
                                {
                                    if (elem_interface.m_Requirement_Interface_Send.Contains(m_Requ_List[0][j1]) == false)
                                    {
                                        elem_interface.m_Requirement_Interface_Send.Add(m_Requ_List[0][j1]);
                                    }

                                    j1++;
                                } while (j1 < m_Requ_List[0].Count);
                                int j2 = 0;
                                do
                                {
                                    if (elem_interface.m_Requirement_Interface_Receive.Contains(m_Requ_List[1][j2]) == false)
                                    {
                                        elem_interface.m_Requirement_Interface_Receive.Add(m_Requ_List[1][j2]);
                                    }

                                    j2++;
                                } while (j2 < m_Requ_List[1].Count);
                            }
                            /*    if (Requ_List != null)
                                {
                                    //Hier die Listen richtig machen, da Sie mehr als 2 EintrÃ¤ge haben kÃ¶nnen
                                    if (elem_interface.m_Requirement_Interface_Send.Contains(Requ_List[0]) == false)
                                    {
                                        elem_interface.m_Requirement_Interface_Send.Add(Requ_List[0]);

                                    }
                                    if (elem_interface.m_Requirement_Interface_Receive.Contains(Requ_List[1]) == false)
                                    {
                                        elem_interface.m_Requirement_Interface_Receive.Add(Requ_List[1]);
                                    }

                                }
                            */
                            elem_interface.m_Target.Add(target2);
                        }
                        else //Target vorhanden
                        {
                            List<List<Requirement_Interface>> m_Requ_List2 = target.Check_Requirement_Interface(repository, Data, true, Data.metamodel.m_Prozesswort_Interface[0]);

                            if (m_Requ_List2 != null)
                            {
                                int j1 = 0;
                                do
                                {
                                    if (elem_interface.m_Requirement_Interface_Send.Contains(m_Requ_List2[0][j1]) == false)
                                    {
                                        elem_interface.m_Requirement_Interface_Send.Add(m_Requ_List2[0][j1]);
                                    }

                                    j1++;
                                } while (j1 < m_Requ_List2[0].Count);
                                int j2 = 0;
                                do
                                {
                                    if (elem_interface.m_Requirement_Interface_Receive.Contains(m_Requ_List2[1][j2]) == false)
                                    {
                                        elem_interface.m_Requirement_Interface_Receive.Add(m_Requ_List2[1][j2]);
                                    }

                                    j2++;
                                } while (j2 < m_Requ_List2[1].Count);
                            }
                            /*
                            if (Requ_List2 != null)
                            {//Hier die Listen richtig machen
                                if (elem_interface.m_Requirement_Interface_Send.Contains(Requ_List2[0]) == false)
                                {
                                    elem_interface.m_Requirement_Interface_Send.Add(Requ_List2[0]);

                                }
                                if (elem_interface.m_Requirement_Interface_Receive.Contains(Requ_List2[1]) == false)
                                {
                                    elem_interface.m_Requirement_Interface_Receive.Add(Requ_List2[1]);
                                }
                            }
                            */
                            List<InformationElement> InfoElm = target.Get_Information_Element_Logical(repository, Connector.ConnectorGUID, target.Logical, Data);

                            if (InfoElm.Count > 0)
                            {
                                int d4 = 0;
                                do
                                {
                                    target.Add_InformationElement(InfoElm[d4]);

                                    d4++;
                                } while (d4 < InfoElm.Count);
                            }
                        }
                    }
                    i1++;
                } while (i1 < Connector_GUID.Count);
            }
            //Kinderelemente betrachten
            if (this.m_Child.Count > 0)
            {
                short i2 = 0;
                do
                {
                    if(this.m_Child[i2].m_Instantiate.Count > 0)
                    {
                        this.m_Child[i2].Get_Connectors_InfoEx(Logical, Data, repository);
                    }

                    i2++;
                } while (i2 < this.m_Child.Count);
            }
            #endregion UML

            #region BPMN
            //Pool erhalten
           // this.Get_Pools(Data);
            //StartEvents in den Pools
            if(Data.metamodel.flag_bpmn == true)
            {
                if (this.m_Pools.Count > 0)
                {
                    int p1 = 0;
                    do
                    {
                        this.m_Pools[p1].Get_Element_Interface(Data, this, repository);
                        p1++;
                    } while (p1 < this.m_Pools.Count);
                }
            }
          
            #endregion BPMN
        }
        /// <summary>
        /// NodeType.Get_All_InformationElement: Alle Informationselemente zum zugehÃ¶rigen Target --< unidirektional
        /// </summary>
        /// <returns>List<InformationElement></returns>
        public List<InformationElement> Get_Target_InformationElement(string Target_GUID)
        {
            List<InformationElement> m_InformationElement = new List<InformationElement>();

            if (this.m_Element_Interface.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Element_Interface[i1].Target_Classifier_ID == Target_GUID)
                    {
                        var help = this.m_Element_Interface[i1].Get_All_Information_Element();

                        if (help.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_InformationElement.Contains(help[i2]) == false)
                                {
                                    m_InformationElement.Add(help[i2]);
                                }

                                i2++;
                            } while (i2 < help.Count);
                        }
                    }
                    i1++;
                } while (i1 < this.m_Element_Interface.Count);
            }

            return (m_InformationElement);

        }
        /// <summary>
                /// aktueller NodeType wird Ã¼berprÃ¼ft, ob ein Interface zum Supplier besteht
                /// </summary>
                /// <param name="Supplier"></param>
                /// <returns></returns>
        public Element_Interface Check_Element_Interface(NodeType Supplier)
        {
            //MessageBox.Show(this.Instantiate_GUID + " " + Supplier.Instantiate_GUID);0
            List<Element_Interface> obj_inter = this.m_Element_Interface.Where(x => x.Classifier_ID == this.Classifier_ID && Supplier.Classifier_ID == x.Target_Classifier_ID).ToList();

            if(obj_inter.Count > 0)
            {
                return (obj_inter[0]);
            }
            else
            {
                return (null);
            }
          
        }
        /// <summary>
        /// Aktueller NodeType wird Ã¼berprÃ¼ft, ob ein Element_Interface_Bidirektional vorliegt
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns></returns>
        public Element_Interface_Bidirectional Check_Element_Interface_Bidirectional(NodeType Supplier)
        {
            //MessageBox.Show(this.Instantiate_GUID + " " + Supplier.Instantiate_GUID);

            if (this.m_Element_Interface_Bidirectional.Count > 0)
            {
                int i1 = 0;
                do
                {
                    //    MessageBox.Show(this.Classifier_ID + " = " + this.m_Element_Interface[i1].Classifier_ID + "\r\n" + Supplier.Classifier_ID + " = " + this.m_Element_Interface[i1].Target_Classifier_ID);

                    if (this.Classifier_ID == this.m_Element_Interface_Bidirectional[i1].Classifier_ID && Supplier.Classifier_ID == this.m_Element_Interface_Bidirectional[i1].Target_Classifier_ID)
                    {

                        //  return (this.m_Element_Interface[i1]);
                        //   MessageBox.Show(this.m_Element_Interface[i1].Client.Instantiate_GUID + " = " + this.Instantiate_GUID + "\r\n" + this.m_Element_Interface[i1].Supplier.Instantiate_GUID + " = " + Supplier.Instantiate_GUID);

                        //Wenn dies nicht da ist kann es auch ein anderer Supplier sein
                        if (this.m_Element_Interface_Bidirectional[i1].Client.Instantiate_GUID == this.Instantiate_GUID && this.m_Element_Interface_Bidirectional[i1].Supplier.Instantiate_GUID == Supplier.Instantiate_GUID)
                        {
                            //   MessageBox.Show("Check");
                            return (this.m_Element_Interface_Bidirectional[i1]);
                        }

                    }
                    else
                    {
                        if(Supplier.GetType() == (typeof(SysElement)))
                        {
                            SysElement help = (SysElement)Supplier;

                           if(this.Classifier_ID == this.m_Element_Interface_Bidirectional[i1].Classifier_ID && help.m_Implements.Select(x => x.Classifier_ID).Contains(this.m_Element_Interface_Bidirectional[i1].Target_Classifier_ID))
                            {
                                return (this.m_Element_Interface_Bidirectional[i1]);
                            }
                        }

                   //     (this.Classifier_ID == this.m_Element_Interface_Bidirectional[i1].Classifier_ID && Supplier.m_I this.m_Element_Interface_Bidirectional[i1].Target_Classifier_ID
                    }

                    i1++;
                } while (i1 < this.m_Element_Interface_Bidirectional.Count);
            }


            return null;
        }
        /// <summary>
        /// NodeType wird Ã¼berprÃ¼ft, ob unter ihm ein NodeType mit der GUID ist
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public NodeType Check_Decomposition_For_NodeType(string GUID)
        {
            //  MessageBox.Show(this.m_Child.Count.ToString());
            List<NodeType> obj_check = this.m_Child.Where(x => x.Classifier_ID == GUID).Select(x => x).ToList();

            if(obj_check.Count > 0)
            {
                return (obj_check[0]);
            }
            else
            {
                return (null);
            }


       /*     if (this.m_Child.Count > 0)
            {
                List<NodeType> m_NodeType = this.m_Child;
                int i1 = 0;
                do
                {
                    if (m_NodeType[i1].Classifier_ID == GUID)
                    {
                        return (m_NodeType[i1]);
                    }

                    i1++;
                } while (i1 < this.m_Child.Count);
            }

            return (null);*/
        }
        /// <summary>
        /// Alle Supplier Nodes werden ausgeklappt und anwÃ¤hlbare angezeigt
        /// </summary>
        /// <param name="Tree"></param>
        /// <param name="Data"></param>
        /// <param name="Repository"></param>
        public void Activate_TreeView(TreeView Tree, Database Data, EA.Repository Repository)
        {
            // MessageBox.Show("Start Activate");

            if (Tree.Nodes.Count > 0) //Kinder im Baum vorhanden
            {
                //  MessageBox.Show("FindNodes with Name: "+ Data.Get_Name_t_object_GUID(this.Classifier_ID, Repository));
                TreeNode[] Nodes = Tree.Nodes.Find(this.Get_Name( Data), true);

                if (Nodes.Length > 0)
                {
                    //   MessageBox.Show("Anzahl Nodes: " + Nodes.Length);

                    int i1 = 0;
                    do
                    {
                        if (Nodes[i1].Tag == this)
                        {
                            Nodes[i1].ForeColor = Color.Black;

                            int level = Nodes[i1].Level;
                            if (level != 0)
                            {
                                TreeNode Parent = Nodes[i1].Parent;

                                int i2 = 0;
                                do
                                {
                                    Parent.Expand();

                                    Parent = Parent.Parent;

                                    i2++;
                                } while (i2 < level);
                            }

                        }

                        i1++;
                    } while (i1 < Nodes.Length);
                }

            }
        }
        /// <summary>
        /// NodeType wird auf Bidirektionale SChnittstellen Ã¼berprÃ¼ft und diese angelegt
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Repository"></param>
        public void Check_Element_Interface_Bidirectional(Database Data, EA.Repository Repository)
        {
            if (this.m_Element_Interface.Count > 0)
            {
                bool flag_lauf = true;

                int i1 = 0;
                do
                {
                    List<InformationElement> m_InfoElem = this.m_Element_Interface[i1].Get_All_Information_Element();

                    if (m_InfoElem.Count > 0) //Bidirektionale Beziehung nur wenn InfoElem vorhanden
                    {
                        NodeType Supplier = this.m_Element_Interface[i1].Supplier;
                        Element_Interface ElemInter_Supplier = Supplier.Check_Element_Interface(this.m_Element_Interface[i1].Client);

                        if (ElemInter_Supplier != null && Supplier != this)
                        {
                            List<InformationElement> m_InfoElem_Supplier = ElemInter_Supplier.Get_All_Information_Element();

                            if (m_InfoElem_Supplier.Count > 0)
                            {
                                var result = from List1 in m_InfoElem join List2 in m_InfoElem_Supplier on List1 equals List2 select List1;

                                var help = result.ToList();

                                if (help.Count > 0)
                                {
                                    string help2 = "";
                                    int i2 = 0;
                                    do
                                    {
                                        help2 = help2 + help[i2].Get_Name(Data) + ", ";

                                        i2++;
                                    } while (i2 < help.Count);

                                    //         MessageBox.Show("Bidirektional: " + Data.Get_Name_t_object_GUID(ElemInter_Supplier.Supplier.Classifier_ID, Repository) + " --> " + Data.Get_Name_t_object_GUID(ElemInter_Supplier.Client.Classifier_ID, Repository) + "\r\r\nBetroffene InformationElement: " + help2);

                                    //die doppelten Information Element aus den Target vom Client und Supplier entfernen & Targets zurÃ¼ckgeben mit dem entsprechenden InfoElem
                                    //--> womÃ¶glich Target lÃ¶schen und sogar Element_Interface, wenn keine Targets Ã¼brig bleiben
                                    //"neuen" Tragets in Element_Interface_Bidirektional Ã¼berfÃ¼hren
                                    List<Target> m_Target_Client = this.m_Element_Interface[i1].Create_Target_Bidirectional(help);
                                    List<Target> m_Target_Supplier = ElemInter_Supplier.Create_Target_Bidirectional(help);

                                    //ÃœberprÃ¼fen, ob Element_Interface lÃ¶schen --> wenn kein Target mehr vorhanden
                                    if (this.m_Element_Interface[i1].m_Target.Count == 0)
                                    {
                                        this.m_Element_Interface.Remove(this.m_Element_Interface[i1]);
                                        i1--;
                                    }
                                    if (ElemInter_Supplier.m_Target.Count == 0)
                                    {
                                        Supplier.m_Element_Interface.Remove(ElemInter_Supplier);
                                    }

                                    //////////////
                                    //Bidirektionala anlegen
                                    if (m_Target_Client != null && m_Target_Supplier != null)
                                    {
                                        //     MessageBox.Show("Anzahl neuer Client Target: " + m_Target_Client.Count.ToString());

                                        ////Client Seite anlegen
                                        Element_Interface_Bidirectional recent = new Element_Interface_Bidirectional(this.Classifier_ID, Supplier.Classifier_ID, Repository, Data);
                                        recent.Client = this;
                                        recent.Supplier = Supplier;
                                        recent.m_Target_Client = m_Target_Client;
                                        recent.m_Target_Supplier = m_Target_Supplier;
                                        recent.m_Target = m_Target_Client;
                                        recent.m_Target.AddRange(m_Target_Supplier);

                                        this.m_Element_Interface_Bidirectional.Add(recent);

                                        ////////Supplier Seite anlegen, da alles andere ja denn nun gelÃ¶scht und kann spÃ¤ter nicht nocheinmal durclaufen werden
                                        Element_Interface_Bidirectional recent_Supplier = new Element_Interface_Bidirectional(Supplier.Classifier_ID, this.Classifier_ID, Repository, Data);
                                        recent_Supplier.Supplier = this;
                                        recent_Supplier.Client = Supplier;
                                        recent_Supplier.m_Target_Client = m_Target_Supplier;
                                        recent_Supplier.m_Target_Supplier = m_Target_Client;

                                        recent_Supplier.m_Target = m_Target_Supplier;
                                        recent_Supplier.m_Target.AddRange(m_Target_Client);

                                        Supplier.m_Element_Interface_Bidirectional.Add(recent_Supplier);

                                        ////////////
                                        //ÃœberprÃ¼fen ob bidirektionale Afo vorliegen
                                        //Schleife Ã¼ber alle Target_Client, dann alle Target_Supplier
                                        //Suche nach Requirement mit Prozesswort austauschen --> anlaog zur unidirektionalen Suche(Copy?)
                                        //Wenn eine gefunden allen Targets zuordnen 
                                        if (m_Target_Client.Count > 0)
                                        {
                                            i2 = 0;
                                            do
                                            {
                                                List<List<Requirement_Interface>> m_Check = m_Target_Client[i2].Check_Requirement_Interface(Repository, Data, true, Data.metamodel.m_Prozesswort_Interface[2]);

                                                if (m_Check != null)
                                                {
                                                    int j1 = 0;
                                                    do
                                                    {
                                                        if (recent.m_Requirement_Interface_Send.Contains(m_Check[0][j1]) == false)
                                                        {
                                                            recent.m_Requirement_Interface_Send.Add(m_Check[0][j1]);
                                                        }
                                                        if (recent_Supplier.m_Requirement_Interface_Receive.Contains(m_Check[0][j1]) == false)
                                                        {
                                                            recent_Supplier.m_Requirement_Interface_Receive.Add(m_Check[0][j1]);
                                                        }

                                                        j1++;
                                                    } while (j1 < m_Check[0].Count);
                                                    int j2 = 0;
                                                    do
                                                    {
                                                        if (recent.m_Requirement_Interface_Receive.Contains(m_Check[1][j2]) == false)
                                                        {
                                                            recent.m_Requirement_Interface_Receive.Add(m_Check[1][j2]);
                                                        }
                                                        if (recent_Supplier.m_Requirement_Interface_Send.Contains(m_Check[1][j2]) == false)
                                                        {
                                                            recent_Supplier.m_Requirement_Interface_Send.Add(m_Check[1][j2]);
                                                        }

                                                        j2++;
                                                    } while (j2 < m_Check[1].Count);
                                                }
                                            /*    if (Check != null)
                                                {
                                                    recent.m_Requirement_Interface_Send.Add(Check[0]);
                                                    recent.m_Requirement_Interface_Receive.Add(Check[1]);
                                                    recent_Supplier.m_Requirement_Interface_Send.Add(Check[1]);
                                                    recent_Supplier.m_Requirement_Interface_Receive.Add(Check[0]);
                                                }
                                            */

                                                i2++;
                                            } while (i2 < m_Target_Client.Count);
                                        }


                                    }

                                }

                            }

                        }

                    }

                    i1++;

                    if (i1 >= this.m_Element_Interface.Count)
                    {
                        flag_lauf = false;
                    }

                } while (flag_lauf == true); //while (i1 < this.m_Element_Interface.Count);
            }
        }
        #endregion Element_Interface
       
        #region Get_Functional
        /// <summary>
        /// Der NodeType wird nach Element_Interface hin untersucht
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        public void Get_Functional(EA.Repository repository, Database database, Forms.Loading_OpArch Load)
        {

            if(this.m_Instantiate.Contains(this.Classifier_ID) == false)
            {
                this.m_Instantiate.Add(this.Classifier_ID);
            }

            //Waren sie überhaupt vorhanden
            if(this.m_Instantiate.Count > 0)
            {

                #region UML
               // if (database.metamodel.flag_bpmn == false)
                //{
                    /////////////////////////////////////////////////////////
                    //Variablen
                    List<string> m_GUID_ClassActivity = new List<string>();
                    List<string> m_GUID_ClassAction = new List<string>();
                    List<string> m_GUID_PartActivity = new List<string>();
                    List<string> m_GUID_PartAction = new List<string>();
                    Repository_Connector repository_Connector = new Repository_Connector();
                    //////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////
                    //Alle Aktion & Aktivity erhalten
                    m_GUID_ClassActivity = Get_Functional_ClassActivity(repository, database);
                    m_GUID_ClassAction = Get_Functional_ClassAction(repository, database);
                    m_GUID_PartActivity = Get_Functional_PartActivity(repository, database);
                    m_GUID_PartAction = Get_Functional_PartAction(repository, database);
                    //////////////////////////////////////////////////////////
                    //Anlegen Element_Functional & Element_User
                    #region Check and Add ClassActivity
                    if (m_GUID_ClassActivity != null)
                    {
                        int i1 = 0;
                        do
                        {

                            //Supplier_GUID erhalten
                            string GUID_Supplier = repository_Connector.Get_Connector_Supplier_GUID(repository, m_GUID_ClassActivity[i1], database);
                            if (GUID_Supplier != null)
                            {
                                //   var theElement = repository.GetElementByGuid(GUID_Supplier);
                                //   var theElement2 = repository.GetElementByGuid(this.Classifier_ID);
                                //   MessageBox.Show(theElement2.Name + " --> " + theElement.Name);

                                //Activity mit GUID erhalten
                                List<Activity> recent_activity = database.Check_Activity(GUID_Supplier);

                                if (recent_activity == null)
                                {
                                    Activity recent = new Activity(GUID_Supplier, database, repository);
                                    database.m_Activity.Add(recent);
                                    List<Activity> recent_activity2 = new List<Activity>();
                                    recent_activity2.Add(recent);
                                    recent_activity = recent_activity2;
                                }


                                if (recent_activity != null)
                                {
                                    int i2 = 0;
                                    do
                                    {

                                        //Überprüfung, ob Element_Functional vorhanden
                                        Element_Functional check_elem_func = Check_Element_Functional(this, recent_activity[i2]);
                                        if (check_elem_func == null)
                                        {
                                            //Element_Functional anlegen
                                            Element_Functional Add_elem = new Element_Functional();
                                            Add_elem.Client = this;
                                            Add_elem.Supplier = recent_activity[i2];

                                            //Neues Element zum NodeType hinzufügen
                                            this.m_Element_Functional.Add(Add_elem);
                                            //Target_Functional anlegen
                                            Add_elem.Create_Target_Functional(this.Classifier_ID, recent_activity[i2].Classifier_ID, repository, database);

                                            if (recent_activity[i2].m_Element_Functional.Contains(Add_elem) == false)
                                            {
                                                recent_activity[i2].m_Element_Functional.Add(Add_elem);
                                            }
                                        }
                                        else
                                        {
                                            //Target_Functional überprüfen
                                            Target_Functional check_target = check_elem_func.Check_Target(this.Classifier_ID, recent_activity[i2].Classifier_ID);
                                            if (check_target == null)
                                            {
                                                check_elem_func.Create_Target_Functional(this.Classifier_ID, recent_activity[i2].Classifier_ID, repository, database);
                                            }

                                        }

                                        i2++;
                                    } while (i2 < recent_activity.Count);
                                }
                            }

                            i1++;
                        } while (i1 < m_GUID_ClassActivity.Count);
                    }
                    #endregion Check and Add ClassActivity

                    #region Check and Add ClassAction
                    if (m_GUID_ClassAction != null)
                    {
                        int i1 = 0;
                        do
                        {
                            //Supplier_GUID erhalten
                            string GUID_Supplier = repository_Connector.Get_Connector_Supplier_Classifier_GUID(repository, m_GUID_ClassAction[i1], "Classifier", database);
                            //Activity mit GUID erhalten
                            if (GUID_Supplier != null)
                            {
                                //  var theElement = repository.GetElementByGuid(GUID_Supplier);
                                //  var theElement2 = repository.GetElementByGuid(this.Classifier_ID);
                                //  MessageBox.Show(theElement2.Name + " --> " + theElement.Name);

                                string GUID_Supplier_Action = repository_Connector.Get_Connector_Supplier_GUID(repository, m_GUID_ClassAction[i1], database);
                                List<Activity> recent_activity = database.Check_Activity(GUID_Supplier);

                                if (recent_activity == null)
                                {
                                    Activity recent = new Activity(GUID_Supplier, database, repository);
                                    database.m_Activity.Add(recent);
                                    List<Activity> recent_activity2 = new List<Activity>();
                                    recent_activity2.Add(recent);
                                    recent_activity = recent_activity2;
                                }

                                if (recent_activity != null)
                                {
                                    int i2 = 0;
                                    do
                                    {
                                        //Überprüfung, ob Element_Functional vorhanden
                                        Element_Functional check_elem_func = Check_Element_Functional(this, recent_activity[i2]);
                                        if (check_elem_func == null)
                                        {
                                            //Element_Functional anlegen
                                            Element_Functional Add_elem = new Element_Functional();
                                            Add_elem.Client = this;
                                            Add_elem.Supplier = recent_activity[i2];
                                            //Neues Element zum NodeType hinzufügen
                                            this.m_Element_Functional.Add(Add_elem);
                                            //Target_Functional anlegen
                                            Add_elem.Create_Target_Functional(this.Classifier_ID, GUID_Supplier_Action, repository, database);

                                            if (recent_activity[i2].m_Element_Functional.Contains(Add_elem) == false)
                                            {
                                                recent_activity[i2].m_Element_Functional.Add(Add_elem);
                                            }

                                        }
                                        else
                                        {
                                            //Target_Functional überprüfen
                                            Target_Functional check_target = check_elem_func.Check_Target(this.Classifier_ID, GUID_Supplier_Action);
                                            if (check_target == null)
                                            {
                                                check_elem_func.Create_Target_Functional(this.Classifier_ID, GUID_Supplier_Action, repository, database);
                                            }

                                        }


                                        i2++;
                                    } while (i2 < recent_activity.Count);
                                }
                            }
                            i1++;
                        } while (i1 < m_GUID_ClassAction.Count);
                    }
                    #endregion

                    #region Check and Add PartActivity
                    if (m_GUID_PartActivity != null)
                    {
                        int i1 = 0;
                        do
                        {
                            //Supplier_GUID erhalten
                            string GUID_Supplier = repository_Connector.Get_Connector_Supplier_GUID(repository, m_GUID_PartActivity[i1], database);
                            //Activity mit GUID erhalten
                            if (GUID_Supplier != null)
                            {
                                //   var theElement = repository.GetElementByGuid(GUID_Supplier);
                                //   var theElement2 = repository.GetElementByGuid(this.Classifier_ID);
                                //  MessageBox.Show(theElement2.Name + " --> " + theElement.Name);

                                string GUID_Client = repository_Connector.Get_Connector_Client_GUID(repository, m_GUID_PartActivity[i1], database, null, null);
                                List<Activity> recent_activity = database.Check_Activity(GUID_Supplier);

                                if (recent_activity == null)
                                {
                                    Activity recent = new Activity(GUID_Supplier, database, repository);
                                    database.m_Activity.Add(recent);
                                    List<Activity> recent_activity2 = new List<Activity>();
                                    recent_activity2.Add(recent);
                                    recent_activity = recent_activity2;
                                }

                                if (recent_activity != null)
                                {
                                    int i2 = 0;
                                    do
                                    {
                                        //Überprüfung, ob Element_Functional vorhanden
                                        Element_Functional check_elem_func = Check_Element_Functional(this, recent_activity[i2]);
                                        if (check_elem_func == null)
                                        {
                                            //Element_Functional anlegen
                                            Element_Functional Add_elem = new Element_Functional();
                                            Add_elem.Client = this;
                                            Add_elem.Supplier = recent_activity[i2];
                                            //Neues Element zum NodeType hinzufügen
                                            this.m_Element_Functional.Add(Add_elem);
                                            //Target_Functional anlegen
                                            Add_elem.Create_Target_Functional(GUID_Client, recent_activity[i2].Classifier_ID, repository, database);

                                            if (recent_activity[i2].m_Element_Functional.Contains(Add_elem) == false)
                                            {
                                                recent_activity[i2].m_Element_Functional.Add(Add_elem);
                                            }
                                        }
                                        else
                                        {
                                            //Target_Functional überprüfen
                                            Target_Functional check_target = check_elem_func.Check_Target(GUID_Client, recent_activity[i2].Classifier_ID);
                                            if (check_target == null)
                                            {
                                                check_elem_func.Create_Target_Functional(GUID_Client, recent_activity[i2].Classifier_ID, repository, database);
                                            }

                                        }


                                        i2++;
                                    } while (i2 < recent_activity.Count);
                                }
                            }
                            i1++;
                        } while (i1 < m_GUID_PartActivity.Count);
                    }
                    #endregion

                    #region Check and Add PartAction
                    if (m_GUID_PartAction != null)
                    {
                        int i1 = 0;
                        do
                        {
                            //Supplier_GUID erhalten
                            string GUID_Supplier = repository_Connector.Get_Connector_Supplier_Classifier_GUID(repository, m_GUID_PartAction[i1], "Classifier", database);
                            //Activity mit GUID erhalten
                            if (GUID_Supplier != null)
                            {
                                // var theElement = repository.GetElementByGuid(GUID_Supplier);
                                // var theElement2 = repository.GetElementByGuid(this.Classifier_ID);
                                // MessageBox.Show(theElement2.Name + " --> " + theElement.Name);

                                string GUID_Supplier_Action = repository_Connector.Get_Connector_Supplier_GUID(repository, m_GUID_PartAction[i1], database);
                                //Supplier_GUID erhalten
                                string GUID_Client = repository_Connector.Get_Connector_Supplier_Classifier_GUID(repository, m_GUID_PartAction[i1], "PDATA1", database);
                                string GUID_Client_Part = repository_Connector.Get_Connector_Client_GUID(repository, m_GUID_PartAction[i1], database, null, null);



                                List<Activity> recent_activity = database.Check_Activity(GUID_Supplier);

                                if (recent_activity == null)
                                {
                                    Activity recent = new Activity(GUID_Supplier, database, repository);
                                    database.m_Activity.Add(recent);
                                    List<Activity> recent_activity2 = new List<Activity>();
                                    recent_activity2.Add(recent);
                                    recent_activity = recent_activity2;
                                }

                                if (recent_activity != null)
                                {
                                    int i2 = 0;
                                    do
                                    {
                                        //Überprüfung, ob Element_Functional vorhanden
                                        Element_Functional check_elem_func = Check_Element_Functional(this, recent_activity[i2]);
                                        if (check_elem_func == null)
                                        {
                                            //Element_Functional anlegen
                                            Element_Functional Add_elem = new Element_Functional();
                                            Add_elem.Client = this;
                                            Add_elem.Supplier = recent_activity[i2];
                                            //Neues Element zum NodeType hinzufügen
                                            this.m_Element_Functional.Add(Add_elem);
                                            Add_elem.Create_Target_Functional(GUID_Client_Part, GUID_Supplier_Action, repository, database);

                                            if (recent_activity[i2].m_Element_Functional.Contains(Add_elem) == false)
                                            {
                                                recent_activity[i2].m_Element_Functional.Add(Add_elem);
                                            }
                                        }
                                        else
                                        {
                                            //Target_Functional überprüfen
                                            Target_Functional check_target = check_elem_func.Check_Target(GUID_Client, GUID_Supplier_Action);
                                            if (check_target == null)
                                            {
                                                check_elem_func.Create_Target_Functional(GUID_Client_Part, GUID_Supplier_Action, repository, database);
                                            }

                                        }

                                        i2++;
                                    } while (i2 < recent_activity.Count);
                                }
                            }
                            i1++;
                        } while (i1 < m_GUID_PartAction.Count);
                    }
                    #endregion
             //   }

                #endregion UML

                #region BPMN
                if (database.metamodel.flag_bpmn == true)
                {
                    #region Get Pools
                    //this.Get_Pools(database);
                    #endregion Get Pool

                    #region Get Activity
                    if (this.m_Pools.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            this.m_Pools[i2].Get_Activity(database, repository);

                            i2++;
                        } while (i2 < this.m_Pools.Count);
                    }
                    if (this.m_Lanes.Count > 0)
                    {
                        int l1 = 0;
                        do
                        {
                            this.m_Lanes[l1].Get_Activity(database, repository);

                            l1++;
                        } while (l1 < this.m_Lanes.Count);
                    }
                    #endregion GetActivity

                    #region Get_Element_Functional
                    if (this.m_Pools.Count > 0) //Element Functional anlegen
                    {
                        int i3 = 0;
                        do
                        {
                            //Jede Activität des Pools überprüfen
                            this.m_Pools[i3].Get_Element_Functional(database, repository);

                            i3++;
                        } while (i3 < this.m_Pools.Count);
                    }
                    if (this.m_Lanes.Count > 0) //Element Functional anlegen
                    {
                        int l2 = 0;
                        do
                        {
                            //Jede Activität des Pools überprüfen
                            this.m_Lanes[l2].Get_Element_Functional(database, repository);

                            l2++;
                        } while (l2 < this.m_Lanes.Count);
                    }
                    #endregion Get_Element_Functional

                }
                #endregion BPMN


                #region Check for Requirements
                if (this.m_Element_Functional.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        this.m_Element_Functional[i1].Check_For_Requirement(repository, database);

                        i1++;
                    } while (i1 < this.m_Element_Functional.Count);
                }
                #endregion

            }






            //MessageBox.Show("Anzahl Element_Interface: "+this.m_Element_Functional.Count.ToString());
        }
        

        #region Get_Functional Faelle
        /// <summary>
        /// Erhalten der Connector_GUID zwischen einem Class und Action welchem dem Stereotype des Connectors Element_Functional entsprechen
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        private List<string> Get_Functional_ClassAction(EA.Repository repository, Database database)
        {
           // DB_Command command = new DB_Command();
           // XML xML = new XML();
           // List<string> m_Connector_GUID = new List<string>();

            List<string> m_Type_Action = database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Action = database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Definition = database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Definition = database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Behavior = database.metamodel.m_Behavior.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Behavior = database.metamodel.m_Behavior.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();

            return (interface_Connectors.Get_Connector_From_Client_Property(database, "ea_guid" , m_Type_Definition, m_Stereotype_Definition, this.Classifier_ID, m_Type_Action, m_Stereotype_Action, m_Type_Behavior, m_Stereotype_Behavior));

           /* string SQL = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN " + xML.SQL_IN_Array(m_Type_Behavior.ToArray()) + "  AND Stereotype IN " + xML.SQL_IN_Array(m_Stereotype_Behavior.ToArray()) + " AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(m_Type_Definition.ToArray())+" AND ea_guid = '" + this.Classifier_ID + "') AND END_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN "+xML.SQL_IN_Array(m_Type_Action.ToArray())+" AND Stereotype IN "+xML.SQL_IN_Array(m_Stereotype_Action.ToArray())+");";
            string SQL_Dat = repository.SQLQuery(SQL);
            m_Connector_GUID = xML.Xml_Read_Attribut("ea_guid", SQL_Dat);
            */
          /*  string SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Behavior.ToArray()) + ")  AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Behavior.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Definition.ToArray()) + ") AND ea_guid = ?) AND END_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Type_Action.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Action.ToArray()) + "));";

            OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(this.Classifier_ID);

            ee.Add(m_Type_Definition.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type_Action.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_Action.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type_Behavior.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_Behavior.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
            string[] m_output = { "ea_guid" };

            List<DB_Return> m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }

            return (m_Connector_GUID);*/
		}
        /// <summary>
        /// Erhalten der Connector_GUID zwischen einem Class und Activity welchem dem Stereotype des Connectors Element_Functional entsprechen
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        /// <returns></returns>
		private List<string> Get_Functional_ClassActivity(EA.Repository repository, Database database)
        {
          //  DB_Command command = new DB_Command();
          //  XML xML = new XML();
          //  List<string> m_Connector_GUID = new List<string>();

            List<string> m_Type_Activity = database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Activity = database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Definition = database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Definition = database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Behavior = database.metamodel.m_Behavior.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Behavior = database.metamodel.m_Behavior.Select(x => x.Stereotype).ToList();


            Interface_Connectors interface_Connectors = new Interface_Connectors();

            return (interface_Connectors.Get_Connector_From_Client_Property(database, "ea_guid", m_Type_Definition, m_Stereotype_Definition, this.Classifier_ID, m_Type_Activity, m_Stereotype_Activity, m_Type_Behavior, m_Stereotype_Behavior));


            /*  string SQL = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN " + xML.SQL_IN_Array(m_Type_Behavior.ToArray()) + "  AND Stereotype IN" + xML.SQL_IN_Array(m_Stereotype_Behavior.ToArray()) + " AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(m_Type_Definition.ToArray())+" AND ea_guid = '" + this.Classifier_ID + "') AND END_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN " + xML.SQL_IN_Array(m_Type_Activity.ToArray()) + " AND Stereotype IN " + xML.SQL_IN_Array(m_Stereotype_Activity.ToArray()) + ");";
              string SQL_Dat = repository.SQLQuery(SQL);
              m_Connector_GUID = xML.Xml_Read_Attribut("ea_guid", SQL_Dat);
              */
            /*  string SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Behavior.ToArray()) + ")  AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Behavior.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Definition.ToArray()) + ") AND ea_guid = ?) AND END_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Type_Activity.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Activity.ToArray()) + "));";

               OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);

               List<DB_Input[]> ee = new List<DB_Input[]>();
               List<string> help_guid = new List<string>();
               help_guid.Add(this.Classifier_ID);

               ee.Add(m_Type_Definition.Select(x => new DB_Input(-1, x)).ToArray());
               ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
               ee.Add(m_Type_Activity.Select(x => new DB_Input(-1, x)).ToArray());
               ee.Add(m_Stereotype_Activity.Select(x => new DB_Input(-1, x)).ToArray());
               ee.Add(m_Type_Behavior.Select(x => new DB_Input(-1, x)).ToArray());
               ee.Add(m_Stereotype_Behavior.Select(x => new DB_Input(-1, x)).ToArray());

               OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
               database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
               string[] m_output = { "ea_guid" };

               List<DB_Return> m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);

               if (m_ret3[0].Ret.Count > 1)
               {
                   return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
               }
               else
               {
                   return (null);
               }
               */


        }
        /// <summary>
        /// Erhalten der Connector_GUID zwischen einem Part und Action welchem dem Stereotype des Connectors Element_Functional entsprechen
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        private List<string> Get_Functional_PartAction(EA.Repository repository, Database database)
        {
           // DB_Command command = new DB_Command();
           // XML xML = new XML();
           // List<string> m_Connector_GUID = new List<string>();

            List<string> m_Type_Action = database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Action = database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Usage = database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Usage = database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Behavior = database.metamodel.m_Behavior.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Behavior = database.metamodel.m_Behavior.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();

            //Hier muss eine SChleife über m_Instantiate durchgeführt werden, ansonsten werden immer alle Elemente analyisiert

            List<string> m_ret = new List<string>();

            if (this.m_Instantiate.Count > 0)
            {
                int i1 = 0;
                do
                {
                    List<string> m_help = interface_Connectors.Get_Connector_From_Client_Property(database, "ea_guid", m_Type_Usage, m_Stereotype_Usage, this.m_Instantiate[i1], m_Type_Action, m_Stereotype_Action, m_Type_Behavior, m_Stereotype_Behavior);

                    if (m_help != null)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < this.m_Instantiate.Count);
            }

            if (m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);
            //  return (interface_Connectors.Get_Connector_From_Client_Property(database, "PDATA1", m_Type_Usage, m_Stereotype_Usage, this.Classifier_ID, m_Type_Action, m_Stereotype_Action, m_Type_Behavior, m_Stereotype_Behavior));

        }

        /// <summary>
        /// Erhalten der Connector_GUID zwischen einem Part und Activity welchem dem Stereotype des Connectors Element_Functional entsprechen
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        /// <returns></returns>
		private List<string> Get_Functional_PartActivity(EA.Repository repository, Database database)
        {
           // DB_Command command = new DB_Command();
           // XML xML = new XML(); 
           // List<string> m_Connector_GUID = new List<string>();

            List<string> m_Type_Activity = database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Activity = database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Usage = database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Usage = database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Behavior = database.metamodel.m_Behavior.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Behavior = database.metamodel.m_Behavior.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();

            //Hier muss eine SChleife über m_Instantiate durchgeführt werden, ansonsten werden immer alle Elemente analyisiert

            List<string> m_ret = new List<string>();

            if(this.m_Instantiate.Count > 0)
            {
                int i1 = 0;
                do
                {
                    List<string> m_help = interface_Connectors.Get_Connector_From_Client_Property(database, "ea_guid", m_Type_Usage, m_Stereotype_Usage, this.m_Instantiate[i1], m_Type_Activity, m_Stereotype_Activity, m_Type_Behavior, m_Stereotype_Behavior);

                    if(m_help != null)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < this.m_Instantiate.Count);
            }

            if(m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);

            //return (interface_Connectors.Get_Connector_From_Client_Property(database, "PDATA1", m_Type_Usage, m_Stereotype_Usage, this.Classifier_ID, m_Type_Activity, m_Stereotype_Activity, m_Type_Behavior, m_Stereotype_Behavior));
        }

        /*	private Element_Functional Check_Element_Functional(){

                return null;
            }
            */
        #endregion Get_Functional
        #endregion Get_Functional

        #region Check_Elements
        /// <summary>
        /// Es wird der NodeType überprüft, ob ein Element_Functional mit den entsprechenden Elementen existiert.
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public Element_Functional Check_Element_Functional(NodeType nodeType, Activity activity)
        {
            if (this.m_Element_Functional.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Element_Functional[i1].Client == nodeType && this.m_Element_Functional[i1].Supplier == activity)
                    {
                        return (this.m_Element_Functional[i1]);
                    }
                    i1++;
                } while (i1 < this.m_Element_Functional.Count);
            }


            return (null);
        }
        /// <summary>
        /// Es wird der NodeType überprüft, ob ein Element_User mit den entsprechenden Elementen existiert.
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public Element_User Check_Element_User(List<Stakeholder> m_stakeholder, Activity activity)
        {
            if (this.m_Element_User.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Element_User[i1].Supplier == activity)
                    {
                        int i2 = 0;
                        do
                        {
                            if(this.m_Element_User[i1].m_Client_ST.Contains(m_stakeholder[i2]) == true)
                            {
                                return (this.m_Element_User[i1]);
                            }

                            i2++;
                        } while (i2 < m_stakeholder.Count);

                     
                    }
                    i1++;
                } while (i1 < this.m_Element_User.Count);
            }

            return (null);

        }

        private Element_Design Check_Element_Design(OperationalConstraint opcon)
        {
            List<Element_Design> m_help =  this.m_Design.Where(x => x.OpConstraint == opcon).ToList();

            if (m_help.Count > 0)
            {
                return (m_help[0]);
            }
            else
            {
                return (null);
            }
        }


        private Element_Environmental Check_Element_Umwelt(OperationalConstraint opcon)
        {
            List<Element_Environmental> m_help = this.m_Enviromental.Where(x => x.OpConstraint == opcon).ToList();

            if (m_help.Count > 0)
            {
                return (m_help[0]);
            }
            else
            {
                return (null);
            }
        }

        private Element_Typvertreter Check_Element_Typvertreter(NodeType Typvertreter)
        {
            List<Element_Typvertreter> m_help = this.m_Typvertreter.Where(x => x.Typvertreter == Typvertreter).ToList();

            if (m_help.Count > 0)
            {
                return (m_help[0]);
            }
            else
            {
                return (null);
            }
        }

        #endregion Check_Elements

        #region Get_Stakeholder
        /// <summary>
        /// Der NodeType wird nach Element_Interface hin untersucht
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        public void Get_Stakeholder(EA.Repository repository, Database database)
        {
            Repository_Connector repository_Connector = new Repository_Connector();
            //Variablen
            List<string> m_GUID_ClassClass = new List<string>();
            List<string> m_GUID_ClassPart = new List<string>();
            List<string> m_GUID_PartClass = new List<string>();
            List<string> m_GUID_PartPart = new List<string>();

            List<string> m_Type_ST = database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_ST = database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList();

            List<string> m_Type_ST_Usage = database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_ST_Usage = database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList();

            //Debughelfer
            var tt = (repository.GetElementByGuid(this.Classifier_ID).Name);

            m_GUID_ClassClass = Get_Stakeholder_ClassClass(repository, database);
            m_GUID_ClassPart = Get_Stakeholder_ClassPart(repository, database);
            m_GUID_PartClass = Get_Stakeholder_PartClass(repository, database);
            m_GUID_PartPart = Get_Stakeholder_PartPart(repository, database);

            #region Check and Add ClassClass
            if (m_GUID_ClassClass != null)
            {
                int i1 = 0;
                do
                {

                    //Supplier_GUID erhalten
                    string GUID_Client = repository_Connector.Get_Connector_Client_GUID(repository, m_GUID_ClassClass[i1], database, null, null);
                    if (GUID_Client != null)
                    {
                        ////////////////////////////////////
                        //Stakeholder erhalten
                        List<Stakeholder> recent_Stakeholder = database.Check_Stakeholder(GUID_Client);

                        if (recent_Stakeholder != null)
                        {
                            int i2 = 0;
                            do
                            {
                                ////////////////////////////////////////////////////
                                ///Überprüfen, ob NodeType im Stakeholder enthalten
                                if(recent_Stakeholder[i2].m_Realisation.Contains(this) == false)
                                {
                                    recent_Stakeholder[i2].m_Realisation.Add(this);
                                }
                                if(this.m_Stakeholder.Contains(recent_Stakeholder[i2]) == false)
                                {
                                    this.m_Stakeholder.Add(recent_Stakeholder[i2]);
                                }
                              /*  ////////////////////////////////////////////////////
                                //Überprüfung, ob Element_User vorhanden
                                Element_User check_elem_user = this.Check_Element_User(recent_Stakeholder[i2]);
                                if (check_elem_user == null)
                                {
                                    //Element_User anlegen
                                    Element_User Add_elem = new Element_User();
                                    Add_elem.Client = this;
                                    Add_elem.m_Client_ST.Add(recent_Stakeholder[i2]);
                                    Add_elem.Supplier = null;
                                    //Neues Element zum NodeType hinzufügen
                                    this.m_Element_User.Add(Add_elem);
                                    //Target_User anlegen
                                    Add_elem.Create_Target_User(this.Classifier_ID, recent_Stakeholder[i2].Classifier_ID, repository, database);
                                }
                                else
                                {
                                    //Target_User überprüfen
                                    Target_Stakeholder check_target = check_elem_user.Check_Target_User(this.Classifier_ID, recent_Stakeholder[i2].Classifier_ID);
                                    if (check_target == null)
                                    {
                                        check_elem_user.Create_Target_User(this.Classifier_ID, recent_Stakeholder[i2].Classifier_ID, repository, database);
                                    }

                                }
                                */
                                i2++;
                            } while (i2 < recent_Stakeholder.Count);
                        }
                    }

                    i1++;
                } while (i1 < m_GUID_ClassClass.Count);
            }
            #endregion Check and Add ClassClass
            #region Check and Add ClassPart
            if (m_GUID_ClassPart != null)
            {
                int i1 = 0;
                do
                {
                    //Supplier_GUID erhalten
                    string GUID_Client = repository_Connector.Get_Connector_Client_GUID(repository, m_GUID_ClassPart[i1], database, null, null);
                    //Activity mit GUID erhalten
                    if (GUID_Client != null)
                    {
                        string GUID_Supplier_Part = repository_Connector.Get_Connector_Supplier_Classifier_GUID(repository, m_GUID_ClassPart[i1], "PDATA1", database);
                        List<Stakeholder> recent_stakeholder = database.Check_Stakeholder(GUID_Client);
                        if (recent_stakeholder != null)
                        {
                            int i2 = 0;
                            do
                            {
                                ////////////////////////////////////////////////////
                                ///Überprüfen, ob NodeType im Stakeholder enthalten
                                if (recent_stakeholder[i2].m_Realisation.Contains(this) == false)
                                {
                                    recent_stakeholder[i2].m_Realisation.Add(this);
                                }
                                if (this.m_Stakeholder.Contains(recent_stakeholder[i2]) == false)
                                {
                                    this.m_Stakeholder.Add(recent_stakeholder[i2]);
                                }
                             /*   ///////////////////////////////////////////////////
                                //Überprüfung, ob Element_User vorhanden
                                Element_User check_elem_user = this.Check_Element_User(recent_stakeholder[i2]);
                                if (check_elem_user == null)
                                {
                                    //Element_User anlegen
                                    Element_User Add_elem = new Element_User();
                                    Add_elem.Client = this;
                                    Add_elem.Client_ST= recent_stakeholder[i2];
                                    Add_elem.Supplier = null;
                                    //Neues Element zum NodeType hinzufügen
                                    this.m_Element_User.Add(Add_elem);
                                    //Target_User anlegen
                                    Add_elem.Create_Target_User(this.Classifier_ID, GUID_Supplier_Part, repository, database);
                                }
                                else
                                {
                                    //Target_User überprüfen
                                    Target_Stakeholder check_target = check_elem_user.Check_Target_User(this.Classifier_ID, GUID_Supplier_Part);
                                    if (check_target == null)
                                    {
                                        check_elem_user.Create_Target_User(this.Classifier_ID, GUID_Supplier_Part, repository, database);
                                    }

                                }

                            */
                                i2++;
                            } while (i2 < recent_stakeholder.Count);
                        }
                    }
                    i1++;
                } while (i1 < m_GUID_ClassPart.Count);
            }
            #endregion
            #region Check and Add PartClass
            if (m_GUID_PartClass != null)
            {
                int i1 = 0;
                do
                {
                    //Supplier_GUID erhalten
                    string GUID_Client = repository_Connector.Get_Connector_Client_Classifier_GUID(repository, m_GUID_PartClass[i1], "PDATA1", database, null, null);
                    //Activity mit GUID erhalten
                    if (GUID_Client != null)
                    {
                        List<Stakeholder> recent_stakeholder = database.Check_Stakeholder(GUID_Client);
                        if (recent_stakeholder != null)
                        {
                            int i2 = 0;
                            do
                            {
                                ////////////////////////////////////////////////////
                                ///Überprüfen, ob NodeType im Stakeholder enthalten
                                if (recent_stakeholder[i2].m_Realisation.Contains(this) == false)
                                {
                                    recent_stakeholder[i2].m_Realisation.Add(this);
                                }
                                if (this.m_Stakeholder.Contains(recent_stakeholder[i2]) == false)
                                {
                                    this.m_Stakeholder.Add(recent_stakeholder[i2]);
                                }
                            /*    ///////////////////////////////////////////////////
                                //Classifier des Zieles erhalten
                                string GUID_Client_Part = repository_Connector.Get_Connector_Client_Classifier_GUID(repository, m_GUID_PartClass[i1], "PDATA1");
                                //Überprüfung, ob Element_User vorhanden
                                Element_User check_elem_user = this.Check_Element_User(recent_stakeholder[i2]);
                                if (check_elem_user == null)
                                {
                                    //Element_User anlegen
                                    Element_User Add_elem = new Element_User();
                                    Add_elem.Client = this;
                                    Add_elem.Client_ST = recent_stakeholder[i2];
                                    Add_elem.Supplier = null;
                                    //Neues Element zum NodeType hinzufügen
                                    this.m_Element_User.Add(Add_elem);
                                    //Target_User anlegen
                                    Add_elem.Create_Target_User(GUID_Client_Part, recent_stakeholder[i2].Classifier_ID, repository, database);
                                }
                                else
                                {
                                    //Target_User überprüfen
                                    Target_Stakeholder check_target = check_elem_user.Check_Target_User(GUID_Client_Part, recent_stakeholder[i2].Classifier_ID);
                                    if (check_target == null)
                                    {
                                        check_elem_user.Create_Target_User(GUID_Client_Part, recent_stakeholder[i2].Classifier_ID, repository, database);
                                    }

                                }
*/

                                i2++;
                            } while (i2 < recent_stakeholder.Count);
                        }
                    }
                    i1++;
                } while (i1 < m_GUID_PartClass.Count);
            }
            #endregion
            #region Check and Add PartPart
            if (m_GUID_PartPart != null)
            {
                int i1 = 0;
                do
                {
                    //Supplier_GUID erhalten
                    string GUID_Client = repository_Connector.Get_Connector_Client_Classifier_GUID(repository, m_GUID_PartPart[i1], "PDATA1", database, null, null);
                    //Activity mit GUID erhalten
                    if (GUID_Client != null)
                    {
                        //GUID Client erhalten
                        string GUID_Supplier_Part = repository_Connector.Get_Connector_Supplier_GUID(repository, m_GUID_PartPart[i1], database);
                        string GUID_Supplier = repository_Connector.Get_Connector_Supplier_Classifier_GUID(repository, m_GUID_PartPart[i1], "PDATA1", database);
                        //Supplier_GUID erhalten
                        // string GUID_Client = repository_Connector.Get_Connector_Supplier_Classifier_GUID(repository, m_GUID_PartAction[i1], "PDATA1");
                        string GUID_Client_Part = repository_Connector.Get_Connector_Client_GUID(repository, m_GUID_PartPart[i1], database, null, null);

                        List<Stakeholder> recent_stakeholder = database.Check_Stakeholder(GUID_Client);
                        if (recent_stakeholder != null)
                        {
                            int i2 = 0;
                            do
                            {
                                ////////////////////////////////////////////////////
                                ///Überprüfen, ob NodeType im Stakeholder enthalten
                                if (recent_stakeholder[i2].m_Realisation.Contains(this) == false)
                                {
                                    recent_stakeholder[i2].m_Realisation.Add(this);
                                }
                                if (this.m_Stakeholder.Contains(recent_stakeholder[i2]) == false)
                                {
                                    this.m_Stakeholder.Add(recent_stakeholder[i2]);
                                }
                           /*     ////////////////////////////////////////////////////
                                //Überprüfung, ob Element_User vorhanden
                                Element_User check_elem_user = this.Check_Element_User(recent_stakeholder[i2]);
                                if (check_elem_user == null)
                                {
                                    //Element_User anlegen
                                    Element_User Add_elem = new Element_User();
                                    Add_elem.Client = this;
                                    Add_elem.Client_ST = recent_stakeholder[i2];
                                    Add_elem.Supplier = null;
                                    //Neues Element zum NodeType hinzufügen
                                    this.m_Element_User.Add(Add_elem);
                                    Add_elem.Create_Target_User(GUID_Client_Part, GUID_Supplier_Part, repository, database);
                                }
                                else
                                {
                                    //Target_User überprüfen
                                    Target_Stakeholder check_target = check_elem_user.Check_Target_User(GUID_Client, GUID_Supplier_Part);
                                    if (check_target == null)
                                    {
                                        check_elem_user.Create_Target_User(GUID_Client_Part, GUID_Supplier_Part, repository, database);
                                    }

                                }
                                */
                                i2++;
                            } while (i2 < recent_stakeholder.Count);
                        }
                    }
                    i1++;
                } while (i1 < m_GUID_PartPart.Count);
            }
            #endregion
       /*     #region Check gor Requirements
            if (this.m_Element_Functional.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Element_Functional[i1].Check_For_Requirement(repository, database);

                    i1++;
                } while (i1 < this.m_Element_Functional.Count);
            }
            #endregion

    */
           /* MessageBox.Show("Anzahl Element_Interface: "+this.m_Element_User.Count.ToString());
            if(this.m_Element_User.Count > 0)
            {
                MessageBox.Show("Anzahl Element_Interface Targets: " + this.m_Element_User[0].m_Target_User.Count.ToString());
            }*/

        }

        

        #region Get_Stakeholder Faelle
        /// <summary>
        /// Erhalten der Connector_GUID zwischen einem Class und Action welchem dem Stereotype des Connectors Element_Functional entsprechen
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        private List<string> Get_Stakeholder_ClassPart(EA.Repository repository, Database database)
        {
           // DB_Command command = new DB_Command();
           // XML xML = new XML();
           // List<string> m_Connector_GUID = new List<string>();

            List<string> m_Type_Stakeholder = database.metamodel.m_Stakeholder.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Stakeholder = database.metamodel.m_Stakeholder.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Element = database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Element = database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();
            List<string> m_Stakeholder_Type = database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList();
            List<string> m_Stakeholder_Stereotype = database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();

            return (interface_Connectors.Get_Connector_From_Supplier_Property(database, "PDATA1", m_Type_Element, m_Stereotype_Element, this.Classifier_ID, m_Stakeholder_Type, m_Stakeholder_Stereotype, m_Type_Stakeholder, m_Stereotype_Stakeholder));


            /*   string SQL = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN " + xML.SQL_IN_Array(m_Type_Stakeholder.ToArray()) + "  AND Stereotype IN " + xML.SQL_IN_Array(m_Stereotype_Stakeholder.ToArray()) + " AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList().ToArray())+" AND PDATA1 = '" + this.Classifier_ID + "') AND START_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList().ToArray())+" AND Stereotype IN " + xML.SQL_IN_Array(database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList().ToArray()) + ");";
               string SQL_Dat = repository.SQLQuery(SQL);
               m_Connector_GUID = xML.Xml_Read_Attribut("ea_guid", SQL_Dat);
               */
        /*    string SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Stakeholder.ToArray()) + ")  AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Stakeholder.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Element.ToArray()) + ") AND PDATA1 = ?) AND START_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Stakeholder_Type.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stakeholder_Stereotype.ToArray()) + "));";

            OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(this.Classifier_ID);

            ee.Add(m_Type_Element.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stakeholder_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stakeholder_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type_Stakeholder.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_Stakeholder.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
            string[] m_output = { "ea_guid" };

            List<DB_Return> m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }

            */
            
        }
        /// <summary>
        /// Erhalten der Connector_GUID zwischen einem Class und Activity welchem dem Stereotype des Connectors Element_Functional entsprechen
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        /// <returns></returns>
		private List<string> Get_Stakeholder_ClassClass(EA.Repository repository, Database database)
        {
          //  DB_Command command = new DB_Command();
          //  XML xML = new XML();
            List<string> m_Connector_GUID = new List<string>();


            List<string> m_Type_Stakeholder = database.metamodel.m_Stakeholder.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Stakeholder = database.metamodel.m_Stakeholder.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Element = database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Element = database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Stakeholder_Type = database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList();
            List<string> m_Stakeholder_Stereotype = database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();

            return (interface_Connectors.Get_Connector_From_Supplier_Property(database, "ea_guid", m_Type_Element, m_Stereotype_Element, this.Classifier_ID, m_Stakeholder_Type, m_Stakeholder_Stereotype, m_Type_Stakeholder, m_Stereotype_Stakeholder));


            /*   string SQL = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN " + xML.SQL_IN_Array(m_Type_Stakeholder.ToArray()) + "  AND Stereotype IN" + xML.SQL_IN_Array(m_Stereotype_Stakeholder.ToArray()) + " AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList().ToArray())+" AND ea_guid = '" + this.Classifier_ID + "') AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList().ToArray())+" AND Stereotype IN " + xML.SQL_IN_Array(database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList().ToArray()) + ");";
               string SQL_Dat = repository.SQLQuery(SQL);
               m_Connector_GUID = xML.Xml_Read_Attribut("ea_guid", SQL_Dat);
               */
         /*   string SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Stakeholder.ToArray()) + ")  AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Stakeholder.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Element.ToArray()) + ") AND ea_guid = ?) AND START_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Stakeholder_Type.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stakeholder_Stereotype.ToArray()) + "));";

            OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(this.Classifier_ID);

            ee.Add(m_Type_Element.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stakeholder_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stakeholder_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type_Stakeholder.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_Stakeholder.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
            string[] m_output = { "ea_guid" };

            List<DB_Return> m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }
            */
        }
        /// <summary>
        /// Erhalten der Connector_GUID zwischen einem Part und Action welchem dem Stereotype des Connectors Element_Functional entsprechen
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        private List<string> Get_Stakeholder_PartPart(EA.Repository repository, Database database)
        {
         //   DB_Command command = new DB_Command();
         //   XML xML = new XML();
            List<string> m_Connector_GUID = new List<string>();

            List<string> m_Type_Stakeholder = database.metamodel.m_Stakeholder.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Stakeholder = database.metamodel.m_Stakeholder.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Element = database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Element = database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();
            List<string> m_Stakeholder_Type = database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList();
            List<string> m_Stakeholder_Stereotype = database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();

            return (interface_Connectors.Get_Connector_From_Supplier_Property(database, "PDATA1", m_Type_Element, m_Stereotype_Element, this.Classifier_ID, m_Stakeholder_Type, m_Stakeholder_Stereotype, m_Type_Stakeholder, m_Stereotype_Stakeholder));


            /*   string SQL = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN" + xML.SQL_IN_Array(m_Type_Stakeholder.ToArray()) + "  AND Stereotype IN" + xML.SQL_IN_Array(m_Stereotype_Stakeholder.ToArray()) + " AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array( database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList().ToArray())+" AND PDATA1 = '" + this.Classifier_ID + "') AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList().ToArray())+" AND Stereotype IN " + xML.SQL_IN_Array(database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList().ToArray()) + ");";
               string SQL_Dat = repository.SQLQuery(SQL);
               m_Connector_GUID = xML.Xml_Read_Attribut("ea_guid", SQL_Dat);
               */
         /*   string SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Stakeholder.ToArray()) + ")  AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Stakeholder.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Element.ToArray()) + ") AND PDATA1 = ?) AND START_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Stakeholder_Type.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stakeholder_Stereotype.ToArray()) + "));";

            OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(this.Classifier_ID);

            ee.Add(m_Type_Element.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stakeholder_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stakeholder_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type_Stakeholder.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_Stakeholder.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
            string[] m_output = { "ea_guid" };

            List<DB_Return> m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }
            */
        }

        /// <summary>
        /// Erhalten der Connector_GUID zwischen einem Part und Activity welchem dem Stereotype des Connectors Element_Functional entsprechen
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        /// <returns></returns>
		private List<string> Get_Stakeholder_PartClass(EA.Repository repository, Database database)
        {
          //  DB_Command command = new DB_Command();
         //   XML xML = new XML();
            List<string> m_Connector_GUID = new List<string>();

            List<string> m_Type_Stakeholder = database.metamodel.m_Stakeholder.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Stakeholder = database.metamodel.m_Stakeholder.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Element = database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Element = database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Stakeholder_Type = database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList();
            List<string> m_Stakeholder_Stereotype = database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();

            return (interface_Connectors.Get_Connector_From_Supplier_Property(database, "ea_guid", m_Type_Element, m_Stereotype_Element, this.Classifier_ID, m_Stakeholder_Type, m_Stakeholder_Stereotype, m_Type_Stakeholder, m_Stereotype_Stakeholder));


            /*    string SQL = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN" + xML.SQL_IN_Array(m_Type_Stakeholder.ToArray()) + " AND Stereotype IN" + xML.SQL_IN_Array(m_Stereotype_Stakeholder.ToArray()) + " AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList().ToArray())+" AND ea_guid = '" + this.Classifier_ID + "') AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList().ToArray())+" AND Stereotype IN " + xML.SQL_IN_Array(database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList().ToArray()) + ");";
                string SQL_Dat = repository.SQLQuery(SQL);
                m_Connector_GUID = xML.Xml_Read_Attribut("ea_guid", SQL_Dat);
                */
         /*   string SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Stakeholder.ToArray()) + ")  AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Stakeholder.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Element.ToArray()) + ") AND ea_guid = ?) AND START_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Stakeholder_Type.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stakeholder_Stereotype.ToArray()) + "));";

            OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(this.Classifier_ID);

            ee.Add(m_Type_Element.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stakeholder_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stakeholder_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type_Stakeholder.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_Stakeholder.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
            string[] m_output = { "ea_guid" };

            List<DB_Return> m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }
            */
        }
        #endregion Get_Stakeholder

        #endregion Get_Stakeholder

        #region Check_Dopplung
        #region Functional & User
        public List<List<Repository_Element>> Check_Dopplung_User(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            #region Dopplung Functional und User

            if (this.m_Element_Functional.Count > 0 && this.m_Element_User.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Element_Functional[i1].m_Requirement_Functional.Count > 0)
                    {
                        //Check auf Gleiche Activity
                        List<Element_User> m_check = this.m_Element_User.Where(x => x.Supplier == this.m_Element_Functional[i1].Supplier).ToList();

                        if(m_check.Count > 0)
                        { 
                            if(m_check[0].m_Requirement_User.Count > 0)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(this);
                                m_help.Add(this.m_Element_Functional[i1].m_Requirement_Functional[0]);
                                m_help.Add(m_check[0].m_Requirement_User[0]);
                                

                                m_Dopplung.Add(m_help);
                            }
                           
                        }
                    }


                    i1++;
                } while (i1 < this.m_Element_Functional.Count);
            }

            #endregion

            return (m_Dopplung);
        }

        public void Create_Issue_Dopplung_User(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID, bool create_issue)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                Requirement Parent = (Requirement)m_Tupel[i1][1];
                Requirement Child = (Requirement)m_Tupel[i1][2];
                //Activity activity = (Activity)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                if (Parent.Classifier_ID != null && Child.Classifier_ID != null)
                {
                    repository_Connector.Create_Dependency(Child.Classifier_ID, Parent.Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                }

                i1++;
            } while (i1 < m_Tupel.Count);
        }

        public List<List<Repository_Element>> Check_Dopplung_Functional(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            if(database.metamodel.flag_sysarch == false)
            {
                #region Kinderelemente
                List<NodeType> m_Child = this.Get_All_Children();

                if (m_Child != null)
                {
                    int i1 = 0;
                    do
                    {
                        List<Activity> m_same = new List<Activity>();
                        List<Activity> m_same1 = this.Get_Process_Activity();
                        List<Activity> m_same2 = m_Child[i1].Get_Process_Activity();

                        if (m_same1.Count > 0 && m_same2.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_same2.Select(x => x.Classifier_ID).Contains(m_same1.Select(x => x.Classifier_ID).ToList()[i2]) == true)
                                {
                                    m_same.Add(m_same1[i2]);
                                }

                                i2++;
                            } while (i2 < m_same1.Count);
                        }
                        else
                        {
                            m_same = new List<Activity>();
                        }

                        //  List<Activity> m_same = this.Get_Process_Activity().Intersect(m_Child[i1].Get_Process_Activity()).ToList();

                        if (m_same.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(this);
                                m_help.Add(m_Child[i1]);
                                m_help.Add(m_same[i2]);

                                m_Dopplung.Add(m_help);

                                i2++;
                            } while (i2 < m_same.Count);
                        }

                        i1++;
                    } while (i1 < m_Child.Count);
                }
                #endregion

                #region Genralisierungen

                List<NodeType> m_Specifiy = this.Get_All_Specifies();

                if (m_Specifiy != null)
                {
                    int i1 = 0;
                    do
                    {
                        List<Activity> m_same = new List<Activity>();
                        List<Activity> m_same1 = this.Get_Process_Activity();
                        List<Activity> m_same2 = m_Specifiy[i1].Get_Process_Activity();

                        if (m_same1.Count > 0 && m_same2.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_same2.Select(x => x.Classifier_ID).Contains(m_same1.Select(x => x.Classifier_ID).ToList()[i2]) == true)
                                {
                                    m_same.Add(m_same1[i2]);
                                }

                                i2++;
                            } while (i2 < m_same1.Count);
                        }
                        else
                        {
                            m_same = new List<Activity>();
                        }




                        //  List<Activity> m_same = this.Get_Process_Activity().Intersect(m_Specifiy[i1].Get_Process_Activity()).ToList();

                        if (m_same.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(this);
                                m_help.Add(m_Specifiy[i1]);
                                m_help.Add(m_same[i2]);

                                m_Dopplung.Add(m_help);

                                i2++;
                            } while (i2 < m_same.Count);
                        }

                        i1++;
                    } while (i1 < m_Specifiy.Count);
                }
                #endregion

                #region Genralisierungen hoch
                List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

                if (m_Specified != null)
                {
                    int i1 = 0;
                    do
                    {
                        List<Activity> m_same = new List<Activity>();
                        List<Activity> m_same1 = this.Get_Process_Activity();
                        List<Activity> m_same2 = m_Specified[i1].Get_Process_Activity();

                        if (m_same1.Count > 0 && m_same2.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_same2.Select(x => x.Classifier_ID).Contains(m_same1.Select(x => x.Classifier_ID).ToList()[i2]) == true)
                                {
                                    m_same.Add(m_same1[i2]);
                                }

                                i2++;
                            } while (i2 < m_same1.Count);
                        }
                        else
                        {
                            m_same = new List<Activity>();
                        }

                        // List<Activity> m_same = this.Get_Process_Activity().Intersect(m_Specified[i1].Get_Process_Activity()).ToList();

                        if (m_same.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();

                                m_help.Add(m_Specified[i1]);
                                m_help.Add(this);
                                m_help.Add(m_same[i2]);

                                m_Dopplung.Add(m_help);

                                i2++;
                            } while (i2 < m_same.Count);
                        }

                        i1++;
                    } while (i1 < m_Specified.Count);
                }
                #endregion

                #region Dopplung Functional und User

                if (this.m_Element_Functional.Count > 0 && this.m_Element_User.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (this.m_Element_Functional[i1].m_Requirement_Functional.Count > 0)
                        {
                            //Check auf Gleiche 
                        }


                        i1++;
                    } while (i1 < this.m_Element_Functional.Count);
                }

                #endregion
            }
          


            return (m_Dopplung);
        }
       
        public void Create_Issue_Dopplung_Functional(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID, bool create_issue)
        {
            Repository_Connector repository_Connector = new Repository_Connector();
         

            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Requirement_Functional> m_Parent_func_req = m_Parent_func.SelectMany(x => x.m_Requirement_Functional).ToList();
                List<Requirement_User> m_Parent_user_req = m_Parent_user.SelectMany(x => x.m_Requirement_User).ToList();
                List<Requirement_Functional> m_Child_func_req = m_Child_func.SelectMany(x => x.m_Requirement_Functional).ToList();
                List<Requirement_User> m_Child_user_req = m_Child_user.SelectMany(x => x.m_Requirement_User).ToList();

                #region Req_Functional Dopplung
                if (m_Parent_func_req.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if(m_Child_func_req.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if(m_Child_func_req[i3] != null && m_Parent_func_req[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_req[i3].Classifier_ID, m_Parent_func_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_req.Count);
                        }
                        if(m_Child_user_req.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_req[i3] != null && m_Parent_func_req[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_req[i3].Classifier_ID, m_Parent_func_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_user_req.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_func_req.Count);
                }
                #endregion
                #region Req User Dopplung
                if (m_Parent_user_req.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_req.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_req[i3] != null && m_Parent_user_req[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_req[i3].Classifier_ID, m_Parent_user_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_func_req.Count);
                        }
                        if (m_Child_user_req.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_req[i3] != null && m_Parent_user_req[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_req[i3].Classifier_ID, m_Parent_user_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_user_req.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_user_req.Count);
                }
                #endregion
                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                if(create_issue == true)
                {
                    #region Issue
                    string name = Parent.Name + " und " + Child.Name + " sind mit derselben Activity " + activity.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                    #endregion


                    //Instanzen mit Issue verküpfen
                    List<string> m_Parent_func_client_guid = m_Parent_func.SelectMany(x => x.Get_Client_GUID_Target()).ToList();
                    List<string> m_Parent_user_client_guid = m_Parent_user.SelectMany(x => x.Get_Client_GUID_Target_User()).ToList();
                    List<string> m_Parent_func_target_guid = m_Parent_func.SelectMany(x => x.Get_Supplier_GUID_Target()).ToList();
                    List<string> m_Parent_user_target_guid = m_Parent_user.SelectMany(x => x.Get_Supplier_GUID_Target_User()).ToList();
                    List<string> m_Child_func_client_guid = m_Child_func.SelectMany(x => x.Get_Client_GUID_Target()).ToList();
                    List<string> m_Childt_user_client_guid = m_Child_user.SelectMany(x => x.Get_Client_GUID_Target_User()).ToList();
                    List<string> m_Child_func_target_guid = m_Child_func.SelectMany(x => x.Get_Supplier_GUID_Target()).ToList();
                    List<string> m_Child_user_target_guid = m_Child_user.SelectMany(x => x.Get_Supplier_GUID_Target_User()).ToList();

                    if (m_Parent_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_client_guid.Count);
                    }
                    if (m_Parent_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_client_guid.Count);
                    }
                    if (m_Parent_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_target_guid.Count);
                    }
                    if (m_Parent_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_target_guid.Count);
                    }

                    if (m_Child_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_client_guid.Count);
                    }
                    if (m_Childt_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Childt_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Childt_user_client_guid.Count);
                    }
                    if (m_Child_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_target_guid.Count);
                    }
                    if (m_Child_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_user_target_guid.Count);
                    }
                }

               


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #region Process
        public List<List<Repository_Element>> Check_Dopplung_Process(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            int i1 = 0;
            do
            {
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];

                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();

                List<Element_Process> m_Parent_func_process = m_Parent_func.SelectMany(x => x.m_element_Processes).ToList();
                List<Element_Process> m_Parent_user_process = m_Parent_user.SelectMany(x => x.m_element_Processes).ToList();
                List<Element_Process> m_Child_func_process = m_Child_func.SelectMany(x => x.m_element_Processes).ToList();
                List<Element_Process> m_Child_user_process = m_Child_user.SelectMany(x => x.m_element_Processes).ToList();

                List<OperationalConstraint> m_Parent_func_process_constraint = m_Parent_func_process.Select(x => x.OpConstraint).ToList();
                List<OperationalConstraint> m_Parent_user_process_constraint = m_Parent_user_process.Select(x => x.OpConstraint).ToList();
                List<OperationalConstraint> m_Child_func_process_constraint = m_Child_func_process.Select(x => x.OpConstraint).ToList();
                List<OperationalConstraint> m_Child_user_process_constraint = m_Child_user_process.Select(x => x.OpConstraint).ToList();

                #region Functional
                if (m_Parent_func_process_constraint.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_process_constraint.Count> 0)
                        {
                           if(m_Child_func_process_constraint.Contains(m_Parent_func_process_constraint[i2]) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }
                        if(m_Child_user_process_constraint.Count > 0)
                        {
                            if (m_Child_user_process_constraint.Contains(m_Parent_func_process_constraint[i2]) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }

                        i2++;
                    } while (i2 < m_Parent_func_process_constraint.Count);
                }
                #endregion
                #region User
                if (m_Parent_user_process_constraint.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_process_constraint.Count > 0)
                        {
                            if (m_Child_func_process_constraint.Contains(m_Parent_user_process_constraint[i2]) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_user_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }
                        if (m_Child_user_process_constraint.Count > 0)
                        {
                            if (m_Child_user_process_constraint.Contains(m_Parent_user_process_constraint[i2]) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }

                        i2++;
                    } while (i2 < m_Parent_user_process_constraint.Count);
                }
                #endregion

                i1++;
            } while (i1 < m_Tupel.Count);

           

            return (m_Dopplung);
        }

        public void Create_Issue_Dopplung_Process(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID, bool create_issue)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];
                OperationalConstraint opcon = (OperationalConstraint)m_Tupel[i1][3];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();

                List<Element_Process> m_Parent_func_Elemnte_Process = m_Parent_func.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == opcon).ToList();
                List<Element_Process> m_Parent_user_Elemnte_Process = m_Parent_user.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == opcon).ToList();
                List<Element_Process> m_Child_func_Elemnte_Process = m_Child_func.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == opcon).ToList();
                List<Element_Process> m_Child_user_Elemnte_Process = m_Child_user.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == opcon).ToList();

                List<Requirement_Non_Functional> m_Parent_func_Req_Process = m_Parent_func_Elemnte_Process.Select(x => x.Requirement_Process).ToList();
                List<Requirement_Non_Functional> m_Parent_user_Req_Process = m_Parent_user_Elemnte_Process.Select(x => x.Requirement_Process).ToList();
                List<Requirement_Non_Functional> m_Child_func_Req_Process = m_Child_func_Elemnte_Process.Select(x => x.Requirement_Process).ToList();
                List<Requirement_Non_Functional> m_Child_user_Req_Process = m_Child_user_Elemnte_Process.Select(x => x.Requirement_Process).ToList();


                #region Req_Functional Dopplung
                if (m_Parent_func_Req_Process.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if(m_Child_func_Req_Process[i3] != null && m_Parent_func_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_Req_Process[i3].Classifier_ID, m_Parent_func_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_Req_Process.Count);
                        }
                        if (m_Child_user_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_Req_Process[i3] != null && m_Parent_func_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_Req_Process[i3].Classifier_ID, m_Parent_func_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_user_Req_Process.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_func_Req_Process.Count);
                }
                #endregion
                #region Req User Dopplung
                if (m_Parent_user_Req_Process.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if(m_Child_func_Req_Process[i3] != null && m_Parent_user_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_Req_Process[i3].Classifier_ID, m_Parent_user_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_Req_Process.Count);
                        }
                        if (m_Child_user_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_Req_Process[i3] != null && m_Parent_user_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_Req_Process[i3].Classifier_ID, m_Parent_user_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_user_Req_Process.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_user_Req_Process.Count);
                }
                #endregion
                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                if(create_issue == true)
                {
                    #region Issue
                    string name = Parent.Name + " und " + Child.Name + " sind mit derselben Activity " + activity.Name + " und OperationalConstraint " + opcon.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                    #endregion
                    //Instanzen mit Issue verküpfen
                    List<string> m_Parent_func_client_guid = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_Node_ID).ToList();
                    List<string> m_Parent_user_client_guid = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_Node_ID).ToList();
                    List<string> m_Parent_func_target_guid = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_Action_ID).ToList();
                    List<string> m_Parent_user_target_guid = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_Action_ID).ToList();
                    List<string> m_Child_func_client_guid = m_Child_func_Elemnte_Process.SelectMany(x => x.m_Node_ID).ToList();
                    List<string> m_Childt_user_client_guid = m_Child_user_Elemnte_Process.SelectMany(x => x.m_Node_ID).ToList();
                    List<string> m_Child_func_target_guid = m_Child_func_Elemnte_Process.SelectMany(x => x.m_Action_ID).ToList();
                    List<string> m_Child_user_target_guid = m_Child_user_Elemnte_Process.SelectMany(x => x.m_Action_ID).ToList();

                    if (m_Parent_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_client_guid.Count);
                    }
                    if (m_Parent_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_client_guid.Count);
                    }
                    if (m_Parent_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_target_guid.Count);
                    }
                    if (m_Parent_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_target_guid.Count);
                    }

                    if (m_Child_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_client_guid.Count);
                    }
                    if (m_Childt_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Childt_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Childt_user_client_guid.Count);
                    }
                    if (m_Child_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_target_guid.Count);
                    }
                    if (m_Child_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_user_target_guid.Count);
                    }
                    repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);



                }


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #region Design
        public List<List<Repository_Element>> Check_Dopplung_Design(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            #region Kinerelemente
            List<NodeType> m_Child = this.Get_All_Children();

            if(m_Child != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList().Intersect(m_Child[i1].Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();

                   

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            OperationalConstraint opcon = database.m_DesignConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Specifies();

            if (m_Specify != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList().Intersect(m_Specify[i1].Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Specify[i1]);

                            OperationalConstraint opcon = database.m_DesignConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Genralisierungen hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList().Intersect(m_Specified[i1].Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                           
                            m_help.Add(m_Specified[i1]);
                            m_help.Add(this);

                            OperationalConstraint opcon = database.m_DesignConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_Dopplung);
        }
        public void Create_Issue_Dopplung_Design(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                OperationalConstraint opcon = (OperationalConstraint)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Design> m_Parent_design = Parent.m_Design.Where(x => x.OpConstraint.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Element_Design> m_Child_design = Child.m_Design.Where(x => x.OpConstraint.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Requirement_Non_Functional> m_Parent_req_design = m_Parent_design.Select(x => x.requirement).ToList();
                List<Requirement_Non_Functional> m_Child_req_design = m_Child_design.Select(x => x.requirement).ToList();

                #region Req_Design Dopplung
                if (m_Parent_req_design.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_design.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if(m_Child_req_design[i3] != null && m_Parent_req_design[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_design[i3].Classifier_ID, m_Parent_req_design[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_design.Count);
                        }
                       

                        i2++;
                    } while (i2 < m_Parent_req_design.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind mit demselben Constraint" + opcon.Name + " verknüpft";
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_desing_client_guid = m_Parent_design.SelectMany(x => x.m_GUID).ToList();
                List<string> m_Child_design_client_guid = m_Child_design.SelectMany(x => x.m_GUID).ToList();

                if (m_Parent_desing_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_desing_client_guid.Count);
                }
                if (m_Child_design_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_design_client_guid.Count);
                }

                repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #region Typvertreter
        public List<List<Repository_Element>> Check_Dopplung_Typverteter(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Typvertreter = new List<List<Repository_Element>>();

            #region Kinderelemente
            List<NodeType> m_Child = this.Get_All_Children();

            if (m_Child != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Typvertreter().Select(x => x.Classifier_ID).ToList().Intersect(m_Child[i1].Get_Typvertreter().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            SysElement opcon = database.m_SysElemente.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Specifies();

            if (m_Specify != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Typvertreter().Select(x => x.Classifier_ID).ToList().Intersect(m_Specify[i1].Get_Typvertreter().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Specify[i1]);

                            SysElement opcon = database.m_SysElemente.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified= this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Typvertreter().Select(x => x.Classifier_ID).ToList().Intersect(m_Specified[i1].Get_Typvertreter().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            
                            m_help.Add(m_Specified[i1]);
                            m_help.Add(this);

                            SysElement opcon = database.m_SysElemente.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_Typvertreter);
        }

        public void Create_Issue_Dopplung_Typvertreter(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                SysElement opcon = (SysElement)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Typvertreter> m_Parent_design = Parent.m_Typvertreter.Where(x => x.Typvertreter.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Element_Typvertreter> m_Child_design = Child.m_Typvertreter.Where(x => x.Typvertreter.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Requirement_Non_Functional> m_Parent_req_design = m_Parent_design.Select(x => x.requirement).ToList();
                List<Requirement_Non_Functional> m_Child_req_design = m_Child_design.Select(x => x.requirement).ToList();

                #region Req_Design Dopplung
                if (m_Parent_req_design.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_design.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if(m_Child_req_design[i3]  != null && m_Parent_req_design[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_design[i3].Classifier_ID, m_Parent_req_design[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_design.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_design.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind durch denselben Typvertreter" + opcon.Name + " verknüpft";
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_desing_client_guid = m_Parent_design.SelectMany(x => x.m_GUID).ToList();
                List<string> m_Child_design_client_guid = m_Child_design.SelectMany(x => x.m_GUID).ToList();

                if (m_Parent_desing_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_desing_client_guid.Count);
                }
                if (m_Child_design_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_design_client_guid.Count);
                }

                repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #region Umwelt
        public List<List<Repository_Element>> Check_Dopplung_Umwelt(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Typvertreter = new List<List<Repository_Element>>();

            #region Kinderelemente
            List<NodeType> m_Child = this.Get_All_Children();

            if (m_Child != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Umwelt().Select(x => x.Classifier_ID).ToList().Intersect(m_Child[i1].Get_Umwelt().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            OperationalConstraint opcon = database.m_UmweltConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Genralisierung
            List<NodeType> m_Specifiy = this.Get_All_Specifies();

            if (m_Specifiy != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Umwelt().Select(x => x.Classifier_ID).ToList().Intersect(m_Specifiy[i1].Get_Umwelt().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Specifiy[i1]);

                            OperationalConstraint opcon = database.m_UmweltConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specifiy.Count);
            }
            #endregion

            #region Genralisierung hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Umwelt().Select(x => x.Classifier_ID).ToList().Intersect(m_Specified[i1].Get_Umwelt().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            
                            m_help.Add(m_Specified[i1]);
                            m_help.Add(this);

                            OperationalConstraint opcon = database.m_UmweltConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_Typvertreter);
        }
        public void Create_Issue_Dopplung_Umwelt(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                OperationalConstraint opcon = (OperationalConstraint)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Environmental> m_Parent_design = Parent.m_Enviromental.Where(x => x.OpConstraint.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Element_Environmental> m_Child_design = Child.m_Enviromental.Where(x => x.OpConstraint.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Requirement_Non_Functional> m_Parent_req_design = m_Parent_design.Select(x => x.requirement).ToList();
                List<Requirement_Non_Functional> m_Child_req_design = m_Child_design.Select(x => x.requirement).ToList();

                #region Req_Design Dopplung
                if (m_Parent_req_design.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_design.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if(m_Child_req_design[i3] != null && m_Parent_req_design[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_design[i3].Classifier_ID, m_Parent_req_design[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_design.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_design.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind durch mit demselbem Umweltelement " + opcon.Name + " verknüpft";
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_desing_client_guid = m_Parent_design.SelectMany(x => x.m_GUID).ToList();
                List<string> m_Child_design_client_guid = m_Child_design.SelectMany(x => x.m_GUID).ToList();

                if (m_Parent_desing_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_desing_client_guid.Count);
                }
                if (m_Child_design_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_design_client_guid.Count);
                }

                repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #region Interface
        public List<List<Repository_Element>> Check_Dopplung_Interface_Unidirektional(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            #region Kinderelemente
            List<NodeType> m_Child = this.Get_All_Children();

            if (m_Child != null)
            {
                List<NodeType> m_Parent_Uni_Supplier = this.m_Element_Interface.Select(x => x.Supplier).ToList();
              

                int i1 = 0;
                do
                {
                    List<NodeType> m_Child_Uni_Supplier = m_Child[i1].m_Element_Interface.Select(x => x.Supplier).ToList();



                    List<NodeType> m_same = m_Parent_Uni_Supplier.Intersect(m_Child_Uni_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Specifies();

            if (m_Specify != null)
            {
                List<NodeType> m_Parent_Uni_Supplier = this.m_Element_Interface.Select(x => x.Supplier).ToList();


                int i1 = 0;
                do
                {
                    List<NodeType> m_Child_Uni_Supplier = m_Specify[i1].m_Element_Interface.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Uni_Supplier.Intersect(m_Child_Uni_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Specify[i1]);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                List<NodeType> m_Parent_Uni_Supplier = this.m_Element_Interface.Select(x => x.Supplier).ToList();


                int i1 = 0;
                do
                {
                    List<NodeType> m_Child_Uni_Supplier = m_Specified[i1].m_Element_Interface.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Uni_Supplier.Intersect(m_Child_Uni_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                           
                            m_help.Add(m_Specified[i1]);
                            m_help.Add(this);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_Dopplung);
        }
        public List<List<Repository_Element>> Check_Dopplung_Interface_Bidirektional(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            #region Kinderelemente
            List<NodeType> m_Child = this.Get_All_Children();

            if (m_Child != null)
            {
                List<NodeType> m_Parent_Bi_Supplier = this.m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();

                int i1 = 0;
                do
                {
                  
                    List<NodeType> m_Child_Bi_Supplier = m_Child[i1].m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Bi_Supplier.Intersect(m_Child_Bi_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }
                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Children();

            if (m_Specify != null)
            {
                List<NodeType> m_Parent_Bi_Supplier = this.m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();

                int i1 = 0;
                do
                {

                    List<NodeType> m_Child_Bi_Supplier = m_Specify[i1].m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Bi_Supplier.Intersect(m_Child_Bi_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Specify[i1]);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }
                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified= this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                List<NodeType> m_Parent_Bi_Supplier = this.m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();

                int i1 = 0;
                do
                {

                    List<NodeType> m_Child_Bi_Supplier = m_Specified[i1].m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Bi_Supplier.Intersect(m_Child_Bi_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                           
                            m_help.Add(m_Specified[i1]);
                            m_help.Add(this);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }
                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion
            return (m_Dopplung);
        }
        public void Create_Issue_Dopplung_Interface_Unidirektional(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                NodeType Supplier = (NodeType)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Interface> m_Parent_interface = Parent.m_Element_Interface.Where(x => x.Supplier.Classifier_ID == Supplier.Classifier_ID).ToList();
                List<Element_Interface> m_Child_interface = Child.m_Element_Interface.Where(x => x.Supplier.Classifier_ID == Supplier.Classifier_ID).ToList();
                List<Requirement_Interface> m_Parent_req_send = m_Parent_interface.SelectMany(x => x.m_Requirement_Interface_Send).ToList();
                List<Requirement_Interface> m_Parent_req_receive = m_Parent_interface.SelectMany(x => x.m_Requirement_Interface_Receive).ToList();
                List<Requirement_Interface> m_Child_req_send = m_Child_interface.SelectMany(x => x.m_Requirement_Interface_Send).ToList();
                List<Requirement_Interface> m_Child_req_receive = m_Child_interface.SelectMany(x => x.m_Requirement_Interface_Receive).ToList();

                #region Req send Dopplung
                if (m_Parent_req_send.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_send.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_send[i3] != null && m_Parent_req_send[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_send[i3].Classifier_ID, m_Parent_req_send[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_send.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_send.Count);
                }
                #endregion
                #region Req receive Dopplung
                if (m_Parent_req_receive.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_receive.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_receive[i3] != null && m_Parent_req_receive[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_receive[i3].Classifier_ID, m_Parent_req_receive[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_receive.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_receive.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind haben eine unidirektionale Schnittstelle mit " + Supplier.Name;
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_client_guid = m_Parent_interface.SelectMany(x => x.m_Target.Select(y => y.CLient_ID).ToList()).ToList();
                List<string> m_Parent_supplier_guid = m_Parent_interface.SelectMany(x => x.m_Target.Select(y => y.Supplier_ID).ToList()).ToList();
                List<string> m_Child_client_guid = m_Child_interface.SelectMany(x => x.m_Target.Select(y => y.CLient_ID).ToList()).ToList();
                List<string> m_Child_supplier_guid = m_Child_interface.SelectMany(x => x.m_Target.Select(y => y.Supplier_ID).ToList()).ToList();

               

                if (m_Parent_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_client_guid.Count);
                }
                if (m_Parent_supplier_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_supplier_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_supplier_guid.Count);
                }
                if (m_Child_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {

                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_client_guid.Count);
                }
                if (m_Child_supplier_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_supplier_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_supplier_guid.Count);
                }

                // repository_Connector.Create_Dependency(issue.Classifier_ID, Supplier.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database);


                i1++;
            } while (i1 < m_Tupel.Count);
        }

        public void Create_Issue_Dopplung_Interface_Bidirektional(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                NodeType Supplier = (NodeType)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Interface_Bidirectional> m_Parent_interface = Parent.m_Element_Interface_Bidirectional.Where(x => x.Supplier.Classifier_ID == Supplier.Classifier_ID).ToList();
                List<Element_Interface_Bidirectional> m_Child_interface = Child.m_Element_Interface_Bidirectional.Where(x => x.Supplier.Classifier_ID == Supplier.Classifier_ID).ToList();
                List<Requirement_Interface> m_Parent_req_send = m_Parent_interface.SelectMany(x => x.m_Requirement_Interface_Send).ToList();
                List<Requirement_Interface> m_Parent_req_receive = m_Parent_interface.SelectMany(x => x.m_Requirement_Interface_Receive).ToList();
                List<Requirement_Interface> m_Child_req_send = m_Child_interface.SelectMany(x => x.m_Requirement_Interface_Send).ToList();
                List<Requirement_Interface> m_Child_req_receive = m_Child_interface.SelectMany(x => x.m_Requirement_Interface_Receive).ToList();

                #region Req send Dopplung
                if (m_Parent_req_send.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_send.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_send[i3] != null && m_Parent_req_send[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_send[i3].Classifier_ID, m_Parent_req_send[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_send.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_send.Count);
                }
                #endregion
                #region Req receive Dopplung
                if (m_Parent_req_receive.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_receive.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_receive[i3] != null && m_Parent_req_receive[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_receive[i3].Classifier_ID, m_Parent_req_receive[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_receive.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_receive.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind haben eine bidirektionale Schnittstelle mit " + Supplier.Name;
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_client_guid = m_Parent_interface.SelectMany(x => x.m_Target.Select(y => y.CLient_ID).ToList()).ToList();
                List<string> m_Parent_supplier_guid = m_Parent_interface.SelectMany(x => x.m_Target.Select(y => y.Supplier_ID).ToList()).ToList();
                List<string> m_Child_client_guid = m_Child_interface.SelectMany(x => x.m_Target.Select(y => y.CLient_ID).ToList()).ToList();
                List<string> m_Child_supplier_guid = m_Child_interface.SelectMany(x => x.m_Target.Select(y => y.Supplier_ID).ToList()).ToList();



                if (m_Parent_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {

                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_client_guid.Count);
                }
                if (m_Parent_supplier_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_supplier_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_supplier_guid.Count);
                }
                if (m_Child_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {

                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_client_guid.Count);
                }
                if (m_Child_supplier_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_supplier_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_supplier_guid.Count);
                }

                // repository_Connector.Create_Dependency(issue.Classifier_ID, Supplier.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database);


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #region QualityClass
        public List<List<Repository_Element>> Check_Dopplung_QualityClass(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_QualityClass = new List<List<Repository_Element>>();

            #region Eigene Ebene
            if(this.m_Element_Measurement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    List<Requirement_Plugin.Repository_Elements.Measurement> m_same = this.Get_Measurement().Where(x => x.measurementType == this.m_Element_Measurement[i1].Measurement.measurementType).ToList();

                    if(m_same.Count > 1)
                    {
                        List<Repository_Element> m_help = new List<Repository_Element>();

                        m_help.Add(this);
                        m_help.Add(null);
                        m_help.Add(this.m_Element_Measurement[i1].Measurement.measurementType);

                        m_QualityClass.Add(m_help);
                    }


                    i1++;
                } while (i1 < this.m_Element_Measurement.Count);
            }

            #endregion 

            #region Kinderelemente
            List<NodeType> m_Child = this.Get_All_Children();

            if (m_Child != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList().Intersect(m_Child[i1].Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList()).ToList();

                
                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            /* List<Elements.Element_Measurement> m_recent_measure = new List<Elements.Element_Measurement>();
                             m_recent_measure = this.m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == m_same[i2]).ToList();
                             List<Elements.Element_Measurement> m_child_measure = new List<Elements.Element_Measurement>();
                             m_child_measure = m_Child[i1].m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == m_same[i2]).ToList();*/
                            Requirement_Plugin.Repository_Elements.MeasurementType mearue_type = database.m_MeasurementType.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];


                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);
                            m_help.Add(mearue_type);
                          //  m_help.AddRange(m_recent_measure.Select(x => x.Measurement).ToList());
                          //  m_help.AddRange(m_child_measure.Select(x => x.Measurement).ToList());

                            //SysElement opcon = database.m_SysElemente.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            //m_help.Add(opcon);

                            m_QualityClass.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Specifies();

            if (m_Specify != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList().Intersect(m_Specify[i1].Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Specify[i1]);

                            Requirement_Plugin.Repository_Elements.MeasurementType mearue_type = database.m_MeasurementType.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                             m_help.Add(mearue_type);

                            m_QualityClass.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList().Intersect(m_Specified[i1].Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList()).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();

                            m_help.Add(m_Specified[i1]);
                            m_help.Add(this);

                            Requirement_Plugin.Repository_Elements.MeasurementType mearue_type = database.m_MeasurementType.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(mearue_type);

                            m_QualityClass.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_QualityClass);
        }

        public void Create_Issue_Dopplung_QualityClass(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                #region Selbst
                if(m_Tupel[i1][1] == null)
                {
                    NodeType Parent = (NodeType)m_Tupel[i1][0];
                    Requirement_Plugin.Repository_Elements.MeasurementType measure_Type = (Requirement_Plugin.Repository_Elements.MeasurementType)m_Tupel[i1][2];

                    List<Elements.Element_Measurement> m_Parent_measure = Parent.m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == measure_Type.Classifier_ID).ToList();
                    List<Requirements.Requirement_Non_Functional> m_req = m_Parent_measure.SelectMany(x => x.m_requirement).Distinct().ToList();


                    #region Req_QualityClass Selbst Dopplung
                    if (m_req.Count > 1)
                    {
                        int i2 = 1;
                        do
                        {

                            repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);


                            i2++;
                        } while (i2 < m_req.Count);
                    }
                    #endregion

                    #region Issue
                    string name = Parent.Name + " ist mit mehreren, gleichen " + measure_Type.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);

                    //Instanzen mit Issue verküpfen
                    List<string> m_Parent_desing_client_guid = m_Parent_measure.SelectMany(x => x.m_guid_Instanzen).ToList();
                    List<string> m_Parent_measurement = m_Parent_measure.Select(x => x.Measurement.Classifier_ID).ToList();

                    if (m_Parent_desing_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_desing_client_guid.Count);
                    }
                    if(m_Parent_measurement.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_measurement[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_measurement.Count);
                    }
                       #endregion

                }
                #endregion
                #region Kinder, Generalsisierung
                else
                {
                    //Elemente
                    NodeType Parent = (NodeType)m_Tupel[i1][0];
                    NodeType Child = (NodeType)m_Tupel[i1][1];
                    Requirement_Plugin.Repository_Elements.MeasurementType measure_Type = (Requirement_Plugin.Repository_Elements.MeasurementType)m_Tupel[i1][2];
                    //Wenn Anforderungen vorhanden mit Dopplung versehen
                    List<Elements.Element_Measurement> m_Parent_measure = Parent.m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == measure_Type.Classifier_ID).ToList();
                    List<Elements.Element_Measurement> m_Child_measure = Child.m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == measure_Type.Classifier_ID).ToList();
                    List<Requirement_Non_Functional> m_Parent_req_measure = m_Parent_measure.SelectMany(x => x.m_requirement).Distinct().ToList();
                    List<Requirement_Non_Functional> m_Child_req_measure = m_Child_measure.SelectMany(x => x.m_requirement).Distinct().ToList();

                    #region Req_Design Dopplung
                    if (m_Parent_req_measure.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (m_Child_req_measure.Count > 0)
                            {
                                int i3 = 0;
                                do
                                {
                                    if (m_Child_req_measure[i3] != null && m_Parent_req_measure[i2] != null)
                                    {
                                        repository_Connector.Create_Dependency(m_Child_req_measure[i3].Classifier_ID, m_Parent_req_measure[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                    }

                                    i3++;
                                } while (i3 < m_Child_req_measure.Count);
                            }


                            i2++;
                        } while (i2 < m_Parent_req_measure.Count);
                    }
                    #endregion

                    //Issue zwischen Elementen und Activity anlegen
                    //Issue erzeugen
                    #region Issue
                    string name = Parent.Name + " und " + Child.Name + " sind mit dem selben MeasurmentType " + measure_Type.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                   
                    //Instanzen mit Issue verküpfen
                    List<string> m_Parent_desing_client_guid = m_Parent_measure.SelectMany(x => x.m_guid_Instanzen).ToList();
                    List<string> m_Child_design_client_guid = m_Child_measure.SelectMany(x => x.m_guid_Instanzen).ToList();

                    List<string> m_Parent_desing_measure_guid = m_Parent_measure.Select(x => x.Measurement.Classifier_ID).ToList();
                    List<string> m_Child_design_measure_guid = m_Child_measure.Select(x => x.Measurement.Classifier_ID).ToList();

                    if (m_Parent_desing_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_desing_client_guid.Count);
                    }
                    if (m_Child_design_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_design_client_guid.Count);
                    }

                    if(m_Parent_desing_measure_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_measure_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_desing_measure_guid.Count);
                    }
                    if(m_Child_design_measure_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_measure_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_design_measure_guid.Count);
                    }

                    #endregion
                }
                #endregion

                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #region QualityActivity
        public List<List<Repository_Element>> Check_Dopplung_QualityActivity(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            int i1 = 0;
            do
            {
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];

                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();

                List<Elements.Element_Measurement> m_Parent_func_process = m_Parent_func.SelectMany(x => x.m_element_measurement).ToList();
                List<Elements.Element_Measurement> m_Parent_user_process = m_Parent_user.SelectMany(x => x.m_element_measurement).ToList();
                List<Elements.Element_Measurement> m_Child_func_process = m_Child_func.SelectMany(x => x.m_element_measurement).ToList();
                List<Elements.Element_Measurement> m_Child_user_process = m_Child_user.SelectMany(x => x.m_element_measurement).ToList();

                List<Requirement_Plugin.Repository_Elements.Measurement> m_Parent_func_process_constraint = m_Parent_func_process.Select(x => x.Measurement).ToList();
                List<Requirement_Plugin.Repository_Elements.Measurement> m_Parent_user_process_constraint = m_Parent_user_process.Select(x => x.Measurement).ToList();
                List<Requirement_Plugin.Repository_Elements.Measurement> m_Child_func_process_constraint = m_Child_func_process.Select(x => x.Measurement).ToList();
                List<Requirement_Plugin.Repository_Elements.Measurement> m_Child_user_process_constraint = m_Child_user_process.Select(x => x.Measurement).ToList();

                #region Functional
                if (m_Parent_func_process_constraint.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_process_constraint.Count > 0)
                        {
                            if (m_Child_func_process_constraint.Select(x => x.Classifier_ID).ToList().Contains(m_Parent_func_process_constraint[i2].Classifier_ID) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }
                        if (m_Child_user_process_constraint.Count > 0)
                        {
                            if (m_Child_user_process_constraint.Select(x => x.Classifier_ID).ToList().Contains(m_Parent_func_process_constraint[i2].Classifier_ID) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }

                        i2++;
                    } while (i2 < m_Parent_func_process_constraint.Count);
                }
                #endregion
                #region User
                if (m_Parent_user_process_constraint.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_process_constraint.Count > 0)
                        {
                            if (m_Child_func_process_constraint.Select(x => x.Classifier_ID).ToList().Contains(m_Parent_user_process_constraint[i2].Classifier_ID) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_user_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }
                        if (m_Child_user_process_constraint.Count > 0)
                        {
                            if (m_Child_user_process_constraint.Select(x => x.Classifier_ID).ToList().Contains(m_Parent_user_process_constraint[i2].Classifier_ID) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }

                        i2++;
                    } while (i2 < m_Parent_user_process_constraint.Count);
                }
                #endregion

                i1++;
            } while (i1 < m_Tupel.Count);



            return (m_Dopplung);
            //  List < List < Repository_Element >>  m_Dopplung_:Functionalthis.Check_Dopplung_Functional(database, repository);

            /*  List<List<Repository_Element>> m_QualityActivity = new List<List<Repository_Element>>();

              List<Activity> m_activity = this.Get_Process_Activity();
              List<Activity> m_activity_measure = new List<Activity>();

              if (m_activity.Count > 0)
              {
                  int i1 = 0;
                  do
                  {

                      List<List<Element_Measurement>> m_ret = m_activity[i1].Check_Measurements_Dopplung(this);

                      if(m_ret.Count > 0)
                      {
                          m_activity_measure.Add(m_activity[i1]);
                      }

                      i1++;
                  } while (i1 < m_activity.Count);
              }

              #region Eigene Ebene
              if (m_activity_measure.Count > 0)
              {
                  int i1 = 0;
                  do
                  {
                      List<Repository_Element> m_help = new List<Repository_Element>();

                      m_help.Add(this);
                      m_help.Add(m_activity_measure[i1]);
                      m_help.Add(null);

                      m_QualityActivity.Add(m_help);

                      i1++;
                  } while (i1 < m_activity_measure.Count);
              }
              #endregion

              #region Kinderelemente
              List<NodeType> m_Child = this.Get_All_Children();
              if (m_Child != null)
              {
                  int i1 = 0;
                  do
                  {
                      List<Activity> m_activity_child = m_Child[i1].Get_Process_Activity();

                      if(m_activity_child.Count > 0)
                      {
                          int i2 = 0;
                          do
                          {
                              if (m_activity_measure.Contains(m_activity_child[i2]))
                              {
                                  List<Repository_Element> m_help = new List<Repository_Element>();

                                  m_help.Add(this);
                                  m_help.Add(m_activity_child[i2]);
                                  m_help.Add(m_Child[i1]);

                                  m_QualityActivity.Add(m_help);

                              }

                              i2++;
                          } while (i2 < m_activity_child.Count);
                      }



                      i1++;
                  } while (i1 < m_Child.Count);
              }

              #endregion
              #region Generalsiiserung
              List<NodeType> m_Specify = this.Get_All_Specifies();

              if (m_Specify != null)
              {
                  int i1 = 0;
                  do
                  {
                      List<Activity> m_activity_spec = m_Specify[i1].Get_Process_Activity();

                      if (m_activity_spec.Count > 0)
                      {
                          int i2 = 0;
                          do
                          {
                              if (m_activity_measure.Contains(m_activity_spec[i2]))
                              {
                                  List<Repository_Element> m_help = new List<Repository_Element>();

                                  m_help.Add(this);
                                  m_help.Add(m_activity_spec[i2]);
                                  m_help.Add(m_Specify[i1]);

                                  m_QualityActivity.Add(m_help);
                              }

                              i2++;
                          } while (i2 < m_activity_spec.Count);
                      }


                      i1++;
                  }while(i1 < m_Specify.Count);
              }
              #endregion

              #region Gneralsisierung hoch
              List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

              if (m_Specified != null)
              {
                  int i1 = 0;
                  do
                  {
                      List<Activity> m_activity_spec = m_Specified[i1].Get_Process_Activity();

                      if (m_activity_spec.Count > 0)
                      {
                          int i2 = 0;
                          do
                          {
                              if (m_activity_measure.Contains(m_activity_spec[i2]))
                              {
                                  List<Repository_Element> m_help = new List<Repository_Element>();

                                  m_help.Add(this);
                                  m_help.Add(m_activity_spec[i2]);
                                  m_help.Add(m_Specified[i1]);

                                  m_QualityActivity.Add(m_help);
                              }

                              i2++;
                          } while (i2 < m_activity_spec.Count);
                      }


                      i1++;
                  } while (i1 < m_Specified.Count);
              }
                  #endregion

              */
           // return (m_QualityActivity);
        }

        public void Create_Issue_Dopplung_QualityActivity(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();
            bool create_issue = true;

            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];
                Requirement_Plugin.Repository_Elements.Measurement opcon = (Requirement_Plugin.Repository_Elements.Measurement)m_Tupel[i1][3];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();

                List<Element_Measurement> m_Parent_func_Elemnte_Process = m_Parent_func.SelectMany(x => x.m_element_measurement).ToList().Where(x => x.Measurement == opcon).ToList();
                List<Element_Measurement> m_Parent_user_Elemnte_Process = m_Parent_user.SelectMany(x => x.m_element_measurement).ToList().Where(x => x.Measurement == opcon).ToList();
                List<Element_Measurement> m_Child_func_Elemnte_Process = m_Child_func.SelectMany(x => x.m_element_measurement).ToList().Where(x => x.Measurement == opcon).ToList();
                List<Element_Measurement> m_Child_user_Elemnte_Process = m_Child_user.SelectMany(x => x.m_element_measurement).ToList().Where(x => x.Measurement == opcon).ToList();

                List<Requirement_Non_Functional> m_Parent_func_Req_Process = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_requirement).Distinct().ToList();
                List<Requirement_Non_Functional> m_Parent_user_Req_Process = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_requirement).Distinct().ToList();
                List<Requirement_Non_Functional> m_Child_func_Req_Process = m_Child_func_Elemnte_Process.SelectMany(x => x.m_requirement).Distinct().ToList();
                List<Requirement_Non_Functional> m_Child_user_Req_Process = m_Child_user_Elemnte_Process.SelectMany(x => x.m_requirement).Distinct().ToList();

                List<string> m_Parent_func_client_guid = m_Parent_func.SelectMany(x => x.m_Target_Functional).ToList().Select(x => x.CLient_ID).ToList(); 
                List<string> m_Parent_user_client_guid = m_Parent_user.SelectMany(x => x.m_Target_User).ToList().Select(x => x.CLient_ID).ToList();
                List<string> m_Child_func_client_guid = m_Child_func.SelectMany(x => x.m_Target_Functional).ToList().Select(x => x.CLient_ID).ToList();
                List<string> m_Childt_user_client_guid = m_Child_user.SelectMany(x => x.m_Target_User).ToList().Select(x => x.CLient_ID).ToList();


                #region Req_Functional Dopplung
                if (m_Parent_func_Req_Process.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_Req_Process[i3] != null && m_Parent_func_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_Req_Process[i3].Classifier_ID, m_Parent_func_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_Req_Process.Count);
                        }
                        if (m_Child_user_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_Req_Process[i3] != null && m_Parent_func_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_Req_Process[i3].Classifier_ID, m_Parent_func_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_user_Req_Process.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_func_Req_Process.Count);
                }
                #endregion
                #region Req User Dopplung
                if (m_Parent_user_Req_Process.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_Req_Process[i3] != null && m_Parent_user_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_Req_Process[i3].Classifier_ID, m_Parent_user_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_Req_Process.Count);
                        }
                        if (m_Child_user_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_Req_Process[i3] != null && m_Parent_user_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_Req_Process[i3].Classifier_ID, m_Parent_user_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_user_Req_Process.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_user_Req_Process.Count);
                }
                #endregion
                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                if (create_issue == true)
                {
                    #region Issue
                    string name = Parent.Name + " und " + Child.Name + " sind mit derselben Activity " + activity.Name + " und Measurement " + opcon.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                    #endregion
                    //Instanzen mit Issue verküpfen
                    //List<string> m_Parent_func_client_guid = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();
                    //List<string> m_Parent_user_client_guid = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();

                    List<string> m_Parent_func_target_guid = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();


                    List<string> m_Parent_user_target_guid = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();

                    //List<string> m_Child_func_client_guid = m_Child_func_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();
                    //List<string> m_Childt_user_client_guid = m_Child_user_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();

                    List<string> m_Child_func_target_guid = m_Child_func_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();
                    List<string> m_Child_user_target_guid = m_Child_user_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();

                    //Client über Element functional und target m_guid_Instnazen finden
                    if (m_Parent_func_Elemnte_Process.Count > 0)
                    {
                        int i3 = 0;
                        do
                        {


                            i3++;
                        } while (i3 < m_Parent_func_Elemnte_Process.Count);
                    }

                    if (m_Parent_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_client_guid.Count);
                    }
                    if (m_Parent_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_client_guid.Count);
                    }
                    if (m_Parent_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_target_guid.Count);
                    }
                    if (m_Parent_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_target_guid.Count);
                    }

                    if (m_Child_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_client_guid.Count);
                    }
                    if (m_Childt_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Childt_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Childt_user_client_guid.Count);
                    }
                    if (m_Child_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_target_guid.Count);
                    }
                    if (m_Child_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_user_target_guid.Count);
                    }
                    repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0] , database.metamodel.m_Con_Trace[0].direction);



                }


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #endregion


        #region DesignRequirement
        public void Get_Element_Design(Database Data, EA.Repository repository)
        {
            List<string> m_Type_elem = Data.metamodel.m_Design_Constraint.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem = Data.metamodel.m_Design_Constraint.Select(x => x.Stereotype).ToList();
            List<string> m_Type_con = Data.metamodel.m_Satisfy_Design.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = Data.metamodel.m_Satisfy_Design.Select(x => x.Stereotype).ToList();

            List<string> m_Type_elem_def = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_def = Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_elem_usage = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_usage = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            Interface_Constraint interface_Constraint = new Interface_Constraint();

            //Schleife über m_Instantiate --> Genaue Zuordnung sonst schwer möglich
            if(this.m_Instantiate.Count > 0) 
            {
                int s1 = 0;
                do
                {
                    List<string> m_Instantiate2 = new List<string>();
                    m_Instantiate2.Clear();
                    m_Instantiate2.Add(this.m_Instantiate[s1]);

                    List<string> m_GUID = interface_Constraint.Get_Constraint(W_Constraint_Type.Design, Data, -1, this.m_Instantiate[s1], m_Type_elem, m_Stereotype_elem, m_Type_con, m_Stereotype_con, m_Stereotype_elem_def, m_Stereotype_elem_usage);

                    if (m_GUID.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            bool flag_check = true;
                            //OperationConstraint anlegen
                            OperationalConstraint opcon = new OperationalConstraint(m_GUID[i1], Data, repository);
                            opcon.m_NodeType.Add(this);

                            if (Data.Check_Design_Constraint(m_GUID[i1]) == null)
                            {
                                Data.m_DesignConstraint.Add(opcon);
                            }

                            //Prüfen ob ein Element Design schon vorliegt mit dem OpConstraint
                            if (this.m_Design.Count > 0)
                            {
                                List<Element_Design> m_DesignCheck = this.m_Design.Where(x => x.OpConstraint.Classifier_ID == m_GUID[i1]).ToList();

                                if (m_DesignCheck.Count == 0)
                                {
                                    flag_check = false;
                                }
                                else
                                {
                                    if (m_DesignCheck[0].m_GUID.Contains(this.m_Instantiate[s1]) == false)
                                    {
                                        m_DesignCheck[0].m_GUID.Add(this.m_Instantiate[s1]);
                                    }

                                }
                            }
                            else
                            {
                                flag_check = false;
                            }

                            //Anlegen neues Element Design mit dem OpConstraint
                            if (flag_check == false)
                            {
                                Element_Design element_Design = new Element_Design(m_Instantiate2, opcon, this);
                            }
                            i1++;
                        } while (i1 < m_GUID.Count);
                    }

                    s1++;
                } while (s1 < this.m_Instantiate.Count);

            }

           



        }

        public void Create_Requirement_Design(Database Data, EA.Repository repository, string Package_GUID)
        {
            if (this.m_Design.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Design[i1].Create_Requirement_Design(repository, Data, Package_GUID);

                    i1++;
                } while (i1 < this.m_Design.Count);
            }
        }

        public void Update_Connectoren_Requirement_Design(Database Data, EA.Repository repository)
        {
            if (this.m_Design.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Design[i1].Update_Connectoren_Requirements_Design(repository, Data);

                    i1++;
                } while (i1 < this.m_Design.Count);
            }
        }
        #endregion DesignRequirement

        #region Umweltbedingung
        public void Get_Element_Umwelt(Database Data, EA.Repository repository)
        {
            List<string> m_Type_elem = Data.metamodel.m_Constraint_Umwelt.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem = Data.metamodel.m_Constraint_Umwelt.Select(x => x.Stereotype).ToList();
            List<string> m_Type_con = Data.metamodel.m_Satisfy_Umwelt.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = Data.metamodel.m_Satisfy_Umwelt.Select(x => x.Stereotype).ToList();

            List<string> m_Type_elem_def = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_def = Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_elem_usage = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_usage = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            Interface_Constraint interface_Constraint = new Interface_Constraint();

            //Schleife über m_Instantiate --> Genaue Zuordnung sonst schwer möglich
            if(this.m_Instantiate.Count > 0)
            {
                int s1 = 0;
                do
                {
                    List<string> m_Instantiate2 = new List<string>();
                    m_Instantiate2.Clear();
                    m_Instantiate2.Add(this.m_Instantiate[s1]);

                    List<string> m_GUID = interface_Constraint.Get_Constraint(W_Constraint_Type.Umwelt, Data, -1, this.m_Instantiate[s1], m_Type_elem, m_Stereotype_elem, m_Type_con, m_Stereotype_con, m_Stereotype_elem_def, m_Stereotype_elem_usage);

                    if (m_GUID.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            bool flag_check = true;
                            //OperationConstraint anlegen
                            OperationalConstraint opcon = new OperationalConstraint(m_GUID[i1], Data, repository);
                            opcon.m_NodeType.Add(this);

                            if (Data.Check_Umwelt_Constraint(m_GUID[i1]) == null)
                            {
                                Data.m_UmweltConstraint.Add(opcon);
                            }

                            //Prüfen ob ein Element Design schon vorliegt mit dem OpConstraint
                            if (this.m_Enviromental.Count > 0)
                            {
                                List<Element_Environmental> m_UmweltCheck = this.m_Enviromental.Where(x => x.OpConstraint.Classifier_ID == m_GUID[i1]).ToList();

                                if (m_UmweltCheck.Count == 0)
                                {
                                    flag_check = false;
                                }
                                else
                                {
                                    if (m_UmweltCheck[0].m_GUID.Contains(this.m_Instantiate[s1]) == false)
                                    {
                                        m_UmweltCheck[0].m_GUID.Add(this.m_Instantiate[s1]);
                                    }

                                }
                            }
                            else
                            {
                                flag_check = false;
                            }

                            //Anlegen neues Element Design mit dem OpConstraint
                            if (flag_check == false)
                            {
                                Element_Environmental element_Design = new Element_Environmental(m_Instantiate2, opcon, this);
                            }
                            i1++;
                        } while (i1 < m_GUID.Count);
                    }

                    s1++;
                } while (s1 < this.m_Instantiate.Count);
            }
           



        }

        public void Create_Requirement_Umwelt(Database Data, EA.Repository repository, string Package_GUID)
        {
            if (this.m_Enviromental.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Enviromental[i1].Create_Requirement_Umwelt(repository, Data, Package_GUID);

                    i1++;
                } while (i1 < this.m_Enviromental.Count);
            }
        }
        public void Update_Connectoren_Requirement_Umwelt(Database Data, EA.Repository repository)
        {
            if (this.m_Enviromental.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Enviromental[i1].Update_Connectoren_Requiremnts_Umwelt(repository, Data);

                    i1++;
                } while (i1 < this.m_Enviromental.Count);
            }
        }

        #endregion Umweltbedingung

        #region Typvertreter
        public void Get_Typvertreter(Database Data, EA.Repository repository)
        {
            List<string> m_Type_def = Data.metamodel.m_Typvertreter_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_def = Data.metamodel.m_Typvertreter_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_usage = Data.metamodel.m_Typvertreter_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_usage = Data.metamodel.m_Typvertreter_Usage.Select(x => x.Stereotype).ToList();

            List<string> m_Type_elem = new List<string>();
            m_Type_elem.AddRange(m_Type_def);
            m_Type_elem.AddRange(m_Type_usage);

            List<string> m_Stereotype_elem = new List<string>();
            m_Stereotype_elem.AddRange(m_Stereotype_def);
            m_Stereotype_elem.AddRange(m_Stereotype_usage);

            List<string> m_Type_con = Data.metamodel.m_Con_Typvertreter.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = Data.metamodel.m_Con_Typvertreter.Select(x => x.Stereotype).ToList();

            List<string> m_Type_elem_def = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_def = Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_elem_usage = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_usage = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            Interface_Constraint interface_Constraint = new Interface_Constraint();

            //Schleife über m_Instantiate --> Genaue Zuordnung sonst schwer möglich
            if(this.m_Instantiate.Count > 0)
            {
                int s1 = 0;
                do
                {
                    List<string> m_Instantiate2 = new List<string>();
                    m_Instantiate2.Clear();
                    m_Instantiate2.Add(this.m_Instantiate[s1]);

                    List<string> m_GUID = interface_Constraint.Get_Typvertreter(Data, -1, this.m_Instantiate[s1], m_Type_elem, m_Stereotype_elem, m_Type_con, m_Stereotype_con);

                    if (m_GUID != null)
                    {
                        int i1 = 0;
                        do
                        {
                            bool flag_check = true;
                            //OperationConstraint anlegen
                            NodeType typvertreter = new NodeType(m_GUID[i1], repository, Data);
                            //  Data.m_Typvertreter.Add(typvertreter);

                            if (Data.Check_Typvertreter(m_GUID[i1]) == null)
                            {
                                Data.m_Typvertreter.Add(typvertreter);
                            }

                            //Prüfen ob ein Element Design schon vorliegt mit dem OpConstraint
                            if (this.m_Typvertreter.Count > 0)
                            {
                                List<Element_Typvertreter> m_Element_Typvertreter = this.m_Typvertreter.Where(x => x.Typvertreter.Classifier_ID == m_GUID[i1]).ToList();

                                if (m_Element_Typvertreter.Count == 0)
                                {
                                    flag_check = false;
                                }
                                else
                                {
                                    if (m_Element_Typvertreter[0].m_GUID.Contains(this.m_Instantiate[s1]) == false)
                                    {
                                        m_Element_Typvertreter[0].m_GUID.Add(this.m_Instantiate[s1]);
                                    }

                                }
                            }
                            else
                            {
                                flag_check = false;
                            }

                            //Anlegen neues Element Design mit dem OpConstraint
                            if (flag_check == false)
                            {
                                Element_Typvertreter element_Typvertreter = new Element_Typvertreter(m_Instantiate2, typvertreter, this);
                            }
                            i1++;
                        } while (i1 < m_GUID.Count);
                    }

                    s1++;
                } while (s1 < this.m_Instantiate.Count);                                                                
            }
           



        }

        public void Create_Requirement_Typvertreter(Database Data, EA.Repository repository, string Package_GUID)
        {
            if (this.m_Typvertreter.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Typvertreter[i1].Create_Requirement_Typvertreter(repository, Data, Package_GUID);

                    i1++;
                } while (i1 < this.m_Typvertreter.Count);
            }
        }

        public void Update_Connectoren_Requirement_Typvertreter(Database Data, EA.Repository repository)
        {
            if (this.m_Typvertreter.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Typvertreter[i1].Update_Connectoren_Requirement_Typvertreter(repository, Data);

                    i1++;
                } while (i1 < this.m_Typvertreter.Count);
            }
        }
        #endregion Typvertreter

        #region QualityClass
        public void Create_Requirement_QualityClass(Database Data, EA.Repository repository, string Package_GUID)
        {
            if (this.m_Element_Measurement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Element_Measurement[i1].Create_Requirement_Class(repository, Data, Package_GUID, this, Package_GUID);

                    i1++;
                } while (i1 < this.m_Element_Measurement.Count);
            }
        }

        public void Update_Connectoren_Requirement_QualityClass(Database Data, EA.Repository repository)
        {
            if (this.m_Element_Measurement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Element_Measurement[i1].Update_Connectoren_QualityClass(repository, Data, this);

                    i1++;
                } while (i1 < this.m_Element_Measurement.Count);
            }
        }
        #endregion

        #region QualityActivity
        public void Create_Requirement_QualityActivity(Database Data, EA.Repository repository, string Package_GUID)
        {
            if (this.m_Element_Functional.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Element_Functional[i1].Create_Requirement_Quality(repository, Data, Package_GUID);

                    i1++;
                } while (i1 < this.m_Element_Functional.Count);
            }
        }

        public void Update_Connectoren_Requirement_QualityActivity(Database Data, EA.Repository repository)
        {
            if (this.m_Element_Functional.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Element_Functional[i1].Update_Connectoren_QualityAcitvity(repository, Data);

                    i1++;
                } while (i1 < this.m_Element_Functional.Count);
            }
        }
        #endregion

        #region  BPMN
        #region Pools
        public void Get_Pools(Database database)
        {
            Interface_Connectors interface_Connectors = new Interface_Connectors();

            List<string> m_Type_Client = database.metamodel.m_Pools_BPMN.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Client = database.metamodel.m_Pools_BPMN.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Con = database.metamodel.m_Con_Pools_BPMN.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Con = database.metamodel.m_Con_Pools_BPMN.Select(x => x.Stereotype).ToList();

            if(this.m_Instantiate.Count > 0)
            {
                int i2 = 0;
                do
                {
                  /*  List<string> m_GUID_Supplier = new List<string>();
                    m_GUID_Supplier.Add(this.m_Instantiate[i2]);

                    List<string> m_GUID_Client = interface_Connectors.Get_Client_Element_By_Connector(database, m_GUID_Supplier, m_Type_Client, m_Stereotype_Client, m_Type_Con, m_Stereotype_Con);
                  */
                    List<string> m_GUID_Client = new List<string>();
                    m_GUID_Client.Add(this.m_Instantiate[i2]);

                    List<string> m_GUID_Supplier = interface_Connectors.Get_Supplier_Element_By_Connector(database, m_GUID_Client, m_Type_Client, m_Stereotype_Client, m_Type_Con, m_Stereotype_Con);

                    if (m_GUID_Supplier != null)
                    {
                        int i1 = 0;
                        do
                        {
                            //Check Pool
                            if(this.m_Pools.Select(x => x.Classifier_ID).ToList().Contains(m_GUID_Supplier[i1]) == false)
                            {
                                //neu anlegen
                                Pool recent = new Pool();
                                recent.Classifier_ID = m_GUID_Supplier[i1];
                                recent.m_Represent.Add(this.m_Instantiate[i2]);
                                this.m_Pools.Add(recent);
                                recent.m_Owner.Add(this);
                                recent.ID = recent.Get_Object_ID(database);

                                recent.Get_Lanes(database);

                                database.m_Pools.Add(recent);
                            }
                            else
                            {
                                // verknüpfen m_Instiate
                                Pool recent = new Pool();
                                recent = this.m_Pools[this.m_Pools.Select(x => x.Classifier_ID).ToList().IndexOf(m_GUID_Supplier[i1])];
                                if(recent.m_Represent.Contains(this.m_Instantiate[i2]) == false)
                                {
                                    recent.m_Represent.Add(this.m_Instantiate[i2]);
                                }
                             
                            }

                            i1++;
                        } while (i1 < m_GUID_Supplier.Count);

                    }

                    i2++;
                } while (i2 < this.m_Instantiate.Count);
            }

            
        }
        #endregion Pools
        #region Lanes
        public void Get_Lanes(Database database)
        {
            Interface_Connectors interface_Connectors = new Interface_Connectors();

            List<string> m_Type_Client = database.metamodel.m_Lanes_BPMN.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Client = database.metamodel.m_Lanes_BPMN.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Con = database.metamodel.m_Con_Pools_BPMN.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Con = database.metamodel.m_Con_Pools_BPMN.Select(x => x.Stereotype).ToList();

            if (this.m_Instantiate.Count > 0)
            {
                int i2 = 0;
                do
                {
                    /*  List<string> m_GUID_Supplier = new List<string>();
                      m_GUID_Supplier.Add(this.m_Instantiate[i2]);

                      List<string> m_GUID_Client = interface_Connectors.Get_Client_Element_By_Connector(database, m_GUID_Supplier, m_Type_Client, m_Stereotype_Client, m_Type_Con, m_Stereotype_Con);
                    */
                    List<string> m_GUID_Client = new List<string>();
                    m_GUID_Client.Add(this.m_Instantiate[i2]);

                    List<string> m_GUID_Supplier = interface_Connectors.Get_Supplier_Element_By_Connector(database, m_GUID_Client, m_Type_Client, m_Stereotype_Client, m_Type_Con, m_Stereotype_Con);

                    if (m_GUID_Supplier != null)
                    {
                        int i1 = 0;
                        do
                        {
                            //Check Pool
                            if (this.m_Lanes.Select(x => x.Classifier_ID).ToList().Contains(m_GUID_Supplier[i1]) == false)
                            {
                                //neu anlegen
                                Pool recent = new Pool();
                                recent.Classifier_ID = m_GUID_Supplier[i1];
                                recent.m_Represent.Add(this.m_Instantiate[i2]);
                                this.m_Lanes.Add(recent);
                                recent.m_Owner.Add(this);
                                recent.ID = recent.Get_Object_ID(database);

                                recent.Get_Lanes(database);

                                database.m_Pools.Add(recent);
                            }
                            else
                            {
                                // verknüpfen m_Instiate
                                Pool recent = new Pool();
                                recent = this.m_Lanes[this.m_Lanes.Select(x => x.Classifier_ID).ToList().IndexOf(m_GUID_Supplier[i1])];
                                if (recent.m_Represent.Contains(this.m_Instantiate[i2]) == false)
                                {
                                    recent.m_Represent.Add(this.m_Instantiate[i2]);
                                }

                            }

                            i1++;
                        } while (i1 < m_GUID_Supplier.Count);

                    }

                    i2++;
                } while (i2 < this.m_Instantiate.Count);
            }


        }
        #endregion Lanes
        #endregion BPMN

        #region Generalisierung

        public void Copy_Generalize(EA.Repository repository, Database database)
        {
            if(this.m_Specialize.Count > 0)
            {
               
                int i1 = 0;
                do
                {
                    #region Functional, User, Process
                    this.Copy_Functional(m_Specialize[i1], repository, database);
                    #endregion

                    #region Design
                    this.Copy_Design(m_Specialize[i1], repository, database);
                    #endregion

                    #region Umwelt
                    this.Copy_Umwelt(m_Specialize[i1], repository, database);
                    #endregion

                    #region Typvertreter
                    this.Copy_Typvertreter(m_Specialize[i1], repository, database);
                    #endregion

                    #region Interfaces
                    this.Copy_Interfaces(m_Specialize[i1], repository, database);
                    #endregion

                    #region Copy_Measurement
                    this.Copy_Measurement(m_Specialize[i1], repository, database);
                    #endregion

                    i1++;
                } while (i1 < this.m_Specialize.Count);

            }
        }

        #region Copy_functional
        private void Copy_Functional(NodeType Target, EA.Repository repository, Database database)
        {
            #region Functional, User, Process
            if (this.m_Element_Functional.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Element_Functional elem_func =  Target.Check_Element_Functional(Target, this.m_Element_Functional[i1].Supplier);

                    if(elem_func != null)
                    {
                        //Ergänzen
                        //elem_func.m_Target_Functional.AddRange(this.m_Element_Functional[i1].m_Target_Functional);
                    }
                    else
                    {
                        //Übertragen
                        Element_Functional copy =  this.m_Element_Functional[i1].Copy_element_Functional(Target, repository, database );

                        Target.m_Element_Functional.Add(copy);
                    }

                    i1++;
                } while (i1 < this.m_Element_Functional.Count);
            }
            if(this.m_Element_User.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Element_User eleme_user = Target.Check_Element_User(this.m_Element_User[i1].m_Client_ST, this.m_Element_User[i1].Supplier);

                    if (eleme_user != null)
                    {
                        //Ergänzen
                        //elem_func.m_Target_Functional.AddRange(this.m_Element_Functional[i1].m_Target_Functional);
                    }
                    else
                    {
                        //Übertragen
                        Element_User copy = this.m_Element_User[i1].Copy_element_User(Target, repository, database);

                        Target.m_Element_User.Add(copy);
                    }

                    i1++;
                } while (i1 < this.m_Element_User.Count);
            }
            #endregion
        }
        #endregion

        #region Copy Design
        private void Copy_Design(NodeType Target, EA.Repository repository, Database database)
        {
            if(this.m_Design.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Element_Design elem_design = Target.Check_Element_Design(this.m_Design[i1].OpConstraint);

                    if(elem_design == null)
                    {
                        Element_Design copy =  this.m_Design[i1].Copy_Element_Design(Target, repository, database);

                       // Target.m_Design.Add(copy);
                    }

                    i1++;
                } while (i1 < this.m_Design.Count);
            }
        }
        #endregion

        #region Copy Umwelt
        private void Copy_Umwelt(NodeType Target, EA.Repository repository, Database database)
        {
            if (this.m_Enviromental.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Element_Environmental elem_umwelt = Target.Check_Element_Umwelt(this.m_Enviromental[i1].OpConstraint);

                    if (elem_umwelt == null)
                    {
                        Element_Environmental copy = this.m_Enviromental[i1].Copy_Element_Umwelt(Target, repository, database);

                        // Target.m_Design.Add(copy);
                    }

                    i1++;
                } while (i1 < this.m_Enviromental.Count);
            }
        }
        #endregion

        #region Typvertreter
        private void Copy_Typvertreter(NodeType Target, EA.Repository repository, Database database)
        {
            if (this.m_Typvertreter.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Element_Typvertreter elem_typvertreter = Target.Check_Element_Typvertreter(this.m_Typvertreter[i1].Typvertreter);

                    if (elem_typvertreter == null)
                    {
                        Element_Typvertreter copy = this.m_Typvertreter[i1].Copy_Element_Typvertreter(Target, repository, database);

                        // Target.m_Design.Add(copy);
                    }

                    i1++;
                } while (i1 < this.m_Typvertreter.Count);
            }
        }
        #endregion

        #region Copy_Interfaces
        private void Copy_Interfaces(NodeType Target, EA.Repository repository, Database database)
        {
            //Client
            #region Client
            #region Unidirektional
            if (this.m_Element_Interface.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Element_Interface elem_inter = Target.Check_Element_Interface(this.m_Element_Interface[i1].Supplier);

                    if(elem_inter == null)
                    {
                        //Kopieren
                        Element_Interface copy = this.m_Element_Interface[i1].Copy_Interface_Unidirektional_Client(Target, repository, database);

                        Target.m_Element_Interface.Add(copy);
                    }
                    else
                    {
                        //InfoElem ergänzen
                        this.m_Element_Interface[i1].Copy_InformationElement_Client(elem_inter, database);
                    }

                    i1++;
                } while (i1 < this.m_Element_Interface.Count);
            }
            #endregion

            #region Bidirektional
            if (this.m_Element_Interface_Bidirectional.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Element_Interface_Bidirectional elem_inter = Target.Check_Element_Interface_Bidirectional(this.m_Element_Interface_Bidirectional[i1].Supplier);

                    if (elem_inter == null)
                    {
                        //Kopieren
                        Element_Interface_Bidirectional copy = this.m_Element_Interface_Bidirectional[i1].Copy_Interface_Bidirektional_Client(Target, repository, database);

                        Target.m_Element_Interface_Bidirectional.Add(copy);
                    }
                    else
                    {
                        //InfoElem ergänzen
                      //  this.m_Element_Interface[i1].Copy_InformationElement_Client(elem_inter, database);
                    }

                    

                    i1++;
                } while (i1 < this.m_Element_Interface_Bidirectional.Count);
            }
            #endregion
            #endregion
            //Supplier
            #region Supplier
            //Unidirektional Client erhalten
            #region  Unidirektional
            List<NodeType> m_Client = database.m_NodeType.Where(x => x.m_Element_Interface.Where(y => y.Supplier == this).ToList().Count > 0).ToList();

            if(m_Client.Count > 0)
            {
                int i1 = 0;
                do
                {

                    Element_Interface elem_alt = m_Client[i1].Check_Element_Interface(this);

                    Element_Interface elem_inter = m_Client[i1].Check_Element_Interface(Target);

                    if (elem_inter == null)
                    {
                        //Kopieren
                        Element_Interface copy = elem_alt.Copy_Interface_Unidirektional_Supplier(Target, repository, database);

                        m_Client[i1].m_Element_Interface.Add(copy);
                    }
                    else
                    {
                        //InfoElem ergänzen
                        elem_alt.Copy_InformationElement_Client(elem_inter, database);
                    }

                    i1++;
                } while (i1 < m_Client.Count);
            }
            #endregion

            #region Bidirektional
            List<NodeType> m_Client_Bi = database.m_NodeType.Where(x => x.m_Element_Interface_Bidirectional.Where(y => y.Supplier == this).ToList().Count > 0).ToList();

            if(m_Client_Bi.Count > 0)
            {
                int i1 = 0;
                do
                {

                    Element_Interface_Bidirectional elem_alt = m_Client_Bi[i1].Check_Element_Interface_Bidirectional(this);

                    Element_Interface_Bidirectional elem_inter = m_Client_Bi[i1].Check_Element_Interface_Bidirectional(Target);

                    if (elem_inter == null)
                    {
                        //Kopieren
                        Element_Interface_Bidirectional copy = elem_alt.Copy_Interface_Bidirektional_Supplier(Target, repository, database);

                        m_Client_Bi[i1].m_Element_Interface_Bidirectional.Add(copy);
                    }
                    else
                    {
                        //InfoElem ergänzen
                        //elem_alt.Copy_InformationElement_Client(elem_inter, database);
                    }

                    i1++;
                } while (i1 < m_Client_Bi.Count);
            }
            #endregion
            #endregion


        }
        #endregion

        #region Copy Measurement
        private void Copy_Measurement(NodeType Target, EA.Repository repository, Database database)
        {

            if(this.m_Element_Measurement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    List<Element_Measurement> m_help = Target.m_Element_Measurement.Where(x => x.Measurement.measurementType == this.m_Element_Measurement[i1].Measurement.measurementType).ToList();

                    if (m_help.Count == 0)
                    {
                        Element_Measurement help = new Element_Measurement(this.m_Element_Measurement[i1].Measurement, database);
                        help.m_guid_Instanzen.Add(Target.Classifier_ID);

                        help.Check_For_Requirement(0, repository, database, null, this);

                        Target.m_Element_Measurement.Add(help);
                    }
                    else
                    {
                        if (m_help[0].m_guid_Instanzen.Contains(Target.Classifier_ID) == false)
                        {
                            m_help[0].m_guid_Instanzen.Add(Target.Classifier_ID);
                        }
                    }

                    i1++;
                } while (i1 < this.m_Element_Measurement.Count);
            }

          
        }
        #endregion
        #endregion


    }
}
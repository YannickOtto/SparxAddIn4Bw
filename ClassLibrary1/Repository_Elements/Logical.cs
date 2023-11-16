using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;
using System.Data.Odbc;

using Database_Connection;
using Elements;
using Requirement_Plugin;
using Ennumerationen;

namespace Repsoitory_Elements
{
    public class Logical : Repository_Class
    {
       
        public List<Target> m_Target;
        public List<Target> m_Target_Bidirectional;
        public List<InformationElement> m_InformationElement;
        public List<Logical> m_Specialize = new List<Logical>();
        public List<string> m_allchildren_guid = new List<string>();
        public List<string> m_allchildreninstance_guid = new List<string>();


        public bool Addin;

        public List<NodeType> m_NodeType;

        public string SYS_ANSPRECHPARTNER;
      

        public Logical(string Logical_ID, Database Data)
        {
            this.Classifier_ID = Logical_ID;
            this.ID = this.Get_Object_ID(Data);
            this.Author = this.Get_Author(Data);
            this.Name = this.Get_Name(Data);
            this.Notes = this.Get_Notes(Data);
            this.m_Target = new List<Target>();
            this.m_Target_Bidirectional = new List<Target>();
            this.m_InformationElement = new List<InformationElement>();
            this.Addin = false;
            this.W_Artikel = "der";
            this.m_NodeType = new List<NodeType>();

            if (Logical_ID != null)
            {
                Get_TV(Data);

            }


        }

        public void Check_Generalization(Database database)
        {

            List<string> m_Type = new List<string>();
            m_Type = database.metamodel.m_Szenar.Select(x => x.Type).ToList();
            List<string> m_Stereotype = new List<string>();
            m_Stereotype = database.metamodel.m_Szenar.Select(x => x.Stereotype).ToList();
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
                    List<Logical> m_act = database.m_Logical.Where(x => x.Classifier_ID == m_guid[i1]).ToList();

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


        public List<Logical> Get_All_SpecifiedBy(List<Logical> m_Logical)
        {
            List<Logical> m_ret = new List<Logical>();

            List<Logical> m_help_General = m_Logical.Where(x => x.m_Specialize.Contains(this) == true).ToList();


            if (m_help_General.Count > 0)
            {
                m_ret = m_help_General;

                int i1 = 0;
                do
                {
                    List<Logical> m_help = new List<Logical>();
                    m_help = m_help_General[i1].Get_All_SpecifiedBy(m_Logical);

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

        private void Get_TV(Database Data)
        {
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            List<DB_Insert> m_Insert = new List<DB_Insert>();
            string SQL = "SELECT Value, Notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
            string[] output = { "Value", "Notes" };

            //  OleDbCommand oleDbCommand = new OleDbCommand(SQL, Data.oLEDB_Interface.dbConnection);
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
                string recent = "";
                do
                {
                    if (m_TV[i1].Ret.Count > 1)
                    {

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
                            flag_insert = false;

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
                                    this.SYS_ARTIKEL = (SYS_ARTIKEL)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_ARTIKEL, recent);
                                    this.W_Artikel = recent;
                                    break;
                                case "SYS_DETAILSTUFE":
                                    this.SYS_DETAILSTUFE = (SYS_DETAILSTUFE)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_DETAILSTUFE, recent);
                                    break;
                                case "SYS_TYP":
                                    this.SYS_TYP = (SYS_TYP)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_TYP, recent);
                                    break;
                                case "SYS_ERFUELLT_AFO":
                                    this.SYS_ERFUELLT_AFO = (SYS_ERFUELLT_AFO)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_ERFUELLT_AFO, recent);
                                    break;
                                case "SYS_SUBORDINATES_AFO":
                                    this.SYS_SUBORDINATES_AFO = (SYS_SUBORDINATES_AFO)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_SUBORDINATES_AFO, recent);
                                    break;
                                case "SYS_KOMPONENTENTYP":
                                    this.SYS_KOMPONENTENTYP = (SYS_KOMPONENTENTYP)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_KOMPONENTENTYP, recent);
                                    break;
                                case "SYS_STATUS":
                                    this.SYS_STATUS = (SYS_STATUS)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_STATUS, recent);
                                    break;
                                case "IN_CATEGORY":
                                    this.IN_CATEGORY = (IN_CATEGORY)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.IN_CATEGORY, recent);
                                    break;
                                case "SYS_PRODUKT_STATUS":
                                    this.SYS_PRODUKT_STATUS = (SYS_PRODUKT_STATUS)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_PRODUKT_STATUS, recent);
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
                                case "TagMask":
                                    this.TagMask = recent;
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
                                this.SYS_TYP = SYS_TYP.Szenarbaum;
                                m_Insert.Add(new DB_Insert("SYS_TYP", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_TYP[(int)SYS_TYP.Szenarbaum], -1));
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
                                this.SYS_KOMPONENTENTYP = SYS_KOMPONENTENTYP.unbestimmt;
                                m_Insert.Add(new DB_Insert("SYS_KOMPONENTENTYP", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_KOMPONENTENTYP[(int)SYS_KOMPONENTENTYP.unbestimmt], -1));
                                break;
                            case "SYS_STATUS":
                                this.SYS_STATUS = SYS_STATUS.realisiert;
                                m_Insert.Add(new DB_Insert("SYS_STATUS", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_STATUS[(int)SYS_STATUS.realisiert], -1));
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
                            case "TagMask":
                                this.TagMask = "0";
                                m_Insert.Add(new DB_Insert("TagMask", OleDbType.VarChar, OdbcType.VarChar, "0", -1));
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

        public List<string> Get_GUID_Children(Database database, List<string> m_Type_Children, List<string> m_Stereotype_Children)
        {
            List<string> m_ret = new List<string>();

            List<string> m_help =  this.Get_Children_Guid(database, m_Type_Children, m_Stereotype_Children);

            if(m_help !=  null)
            {
                int i1 = 0;
                do
                {
                    Logical logical = new Logical(null, null);
                    logical.Classifier_ID = m_help[i1];

                    List<string> m_help2 = logical.Get_GUID_Children(database, m_Type_Children, m_Stereotype_Children);

                    logical.ID = logical.Get_Object_ID(database);
                    string help = logical.Get_Classifier(database);

                    if (help != null)
                    {
                        if (help != m_help[i1])
                        {
                            m_help[i1] = help;
                        }
                    }

                    m_ret.Add(m_help[i1]);

                    if (m_help2 != null)
                    {
                        m_ret.AddRange(m_help2);
                    }

                    i1++;
                } while (i1 < m_help.Count);
            }


            if(m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);

        }

        public List<string> Get_GUID_Instance_Children(Database database, List<string> m_Type_Children, List<string> m_Stereotype_Children)
        {
            List<string> m_ret = new List<string>();

            List<string> m_help = this.Get_Children_Guid(database, m_Type_Children, m_Stereotype_Children);

            if (m_help != null)
            {
                int i1 = 0;
                do
                {
                    Logical logical = new Logical(null, null);
                    logical.Classifier_ID = m_help[i1];

                    List<string> m_help2 = logical.Get_GUID_Instance_Children(database, m_Type_Children, m_Stereotype_Children);

                    //logical.ID = logical.Get_Object_ID(database);
                    //string help = logical.Get_Classifier(database);

                  /*  if (help != null)
                    {
                        if (help != m_help[i1])
                        {
                            m_help[i1] = help;
                        }
                    }*/

                    m_ret.Add(m_help[i1]);

                    if (m_help2 != null)
                    {
                        m_ret.AddRange(m_help2);
                    }

                    i1++;
                } while (i1 < m_help.Count);
            }


            if (m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);

        }

    }
}

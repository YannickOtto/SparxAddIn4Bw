using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;
using System.Data.Odbc;

using Database_Connection;
using Requirements;
using Ennumerationen;

namespace Repsoitory_Elements
{
    public class Capability : Repository_Class
    {


        public List<Capability> m_Parent;
        public List<Capability> m_Child;
        public List<Requirement> m_Requirement;

        public int rang;



        public Capability(string Classifier_ID, EA.Repository Repository, Requirement_Plugin.Database Data)
        {
            //Sys_Komponententyp

            this.Classifier_ID = Classifier_ID;
            this.ID = this.Get_Object_ID(Data);
            this.Author = this.Get_Author(Data);
            this.Name = this.Get_Name(Data);
            this.Notes = this.Get_Notes(Data);
            this.m_Parent = new List<Capability>();
            this.m_Child = new List<Capability>();
            this.m_Requirement = new List<Requirement>();
            this.W_Artikel = "der";
            this.SYS_ARTIKEL = SYS_ARTIKEL.der;

            if (Classifier_ID != null)
            {
                Get_TV(Data, Repository);
            }


        }

        /// <summary>
        /// Es handelt sich hier aktuell um eine Taxonie mit Konnektoren
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="repository"></param>
        public void Get_Parent(Requirement_Plugin.Database Data, EA.Repository repository)
        {

            //    XML xML = new XML();
            List<string> m_Capability_Type = Data.metamodel.m_Capability.Select(x => x.Type).ToList();
            List<string> m_Capability_Stereotype = Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList();

            List<string> m_Taxonomy_Type = Data.metamodel.m_Taxonomy_Capability.Select(x => x.Type).ToList();
            List<string> m_Taxonomy_Stereotype = Data.metamodel.m_Taxonomy_Capability.Select(x => x.Stereotype).ToList();

            ///Alle Endungen einer Capability Taxomie erhalten
          /*  string SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN" + xML.SQL_IN_Array(m_Taxonomy_Type.ToArray()) + " AND Stereotype IN" + xML.SQL_IN_Array(m_Taxonomy_Stereotype.ToArray()) + " AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN" + xML.SQL_IN_Array(m_Capability_Type.ToArray()) + " AND Stereotype IN" + xML.SQL_IN_Array(m_Capability_Stereotype.ToArray()) + " AND ea_guid = '" + this.Classifier_ID + "'));";
            string xml_String = repository.SQLQuery(SQL);
            List<string> m_Parent_GUID = (xML.Xml_Read_Attribut("ea_guid", xml_String));
            */
            List<string> m_Parent_GUID = this.Get_Parent_Taxonomy(Data, this.Classifier_ID, m_Taxonomy_Type, m_Taxonomy_Stereotype, m_Capability_Type, m_Capability_Stereotype);

            if (m_Parent_GUID != null)
            {
                //Suche nach vorhandenem Capability
                int i1 = 0;
                do
                {


                    if (m_Parent_GUID.Contains(Data.m_Capability[i1].Classifier_ID) == true)
                    {
                        this.m_Parent.Add(Data.m_Capability[i1]);
                        i1 = Data.m_Capability.Count;
                    }

                    i1++;
                } while (i1 < Data.m_Capability.Count);
            }


        }
        /// <summary>
        /// Es handelt sich hier aktuell um eine Taxonomy mit Konnektoren
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="repository"></param>
        public void Get_Child(Requirement_Plugin.Database Data, EA.Repository repository)
        {
            //      XML xML = new XML();
            List<string> m_Capability_Type = Data.metamodel.m_Capability.Select(x => x.Type).ToList();
            List<string> m_Capability_Stereotype = Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList();

            List<string> m_Taxonomy_Type = Data.metamodel.m_Taxonomy_Capability.Select(x => x.Type).ToList();
            List<string> m_Taxonomy_Stereotype = Data.metamodel.m_Taxonomy_Capability.Select(x => x.Stereotype).ToList();
            ///Alle Endungen einer Capability Taxomie erhalten
        /*    string SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE Connector_Type IN" + xML.SQL_IN_Array(m_Taxonomy_Type.ToArray()) + " AND Stereotype IN" + xML.SQL_IN_Array(m_Taxonomy_Type.ToArray()) + " AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(m_Capability_Type.ToArray())+" AND Stereotype IN"+xML.SQL_IN_Array(m_Capability_Stereotype.ToArray())+" AND ea_guid = '" + this.Classifier_ID + "'));";

            string xml_String = repository.SQLQuery(SQL);

            List<string> m_Parent_GUID = (xML.Xml_Read_Attribut("ea_guid", xml_String));*/

            List<string> m_Parent_GUID = this.Get_Child_Taxonomy(Data, this.Classifier_ID, m_Taxonomy_Type, m_Taxonomy_Stereotype, m_Capability_Type, m_Capability_Stereotype);

            if (m_Parent_GUID != null)
            {
                //Suche nach vorhandenem Capability
                int i1 = 0;
                do
                {
                    if (m_Parent_GUID.Contains(Data.m_Capability[i1].Classifier_ID) == true)
                    {
                        this.m_Child.Add(Data.m_Capability[i1]);
                    }

                    i1++;
                } while (i1 < Data.m_Capability.Count);
            }
        }

        public void Get_Child_Catalogue(Requirement_Plugin.Database Data, EA.Repository repository)
        {
            //      XML xML = new XML();
            List<string> m_Capability_Type = Data.metamodel.m_Capability.Select(x => x.Type).ToList();
            m_Capability_Type.AddRange(Data.metamodel.m_Capability_Catalogue.Select(x => x.Type).ToList());
            List<string> m_Capability_Stereotype = Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList();
            m_Capability_Stereotype.AddRange(Data.metamodel.m_Capability_Catalogue.Select(x => x.Stereotype).ToList());
            List<string> m_Taxonomy_Type = Data.metamodel.m_Taxonomy_Capability.Select(x => x.Type).ToList();
            List<string> m_Taxonomy_Stereotype = Data.metamodel.m_Taxonomy_Capability.Select(x => x.Stereotype).ToList();
            ///Alle Endungen einer Capability Taxomie erhalten
        /*    string SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE Connector_Type IN" + xML.SQL_IN_Array(m_Taxonomy_Type.ToArray()) + " AND Stereotype IN" + xML.SQL_IN_Array(m_Taxonomy_Type.ToArray()) + " AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(m_Capability_Type.ToArray())+" AND Stereotype IN"+xML.SQL_IN_Array(m_Capability_Stereotype.ToArray())+" AND ea_guid = '" + this.Classifier_ID + "'));";

            string xml_String = repository.SQLQuery(SQL);

            List<string> m_Parent_GUID = (xML.Xml_Read_Attribut("ea_guid", xml_String));*/

            List<string> m_Parent_GUID = this.Get_Child_Taxonomy(Data, this.Classifier_ID, m_Taxonomy_Type, m_Taxonomy_Stereotype, m_Capability_Type, m_Capability_Stereotype);

            if (m_Parent_GUID != null)
            {
                //Suche nach vorhandenem Capability
                int i1 = 0;
                do
                {
                    if (m_Parent_GUID.Contains(Data.m_Capability[i1].Classifier_ID) == true)
                    {
                        this.m_Child.Add(Data.m_Capability[i1]);
                    }

                    i1++;
                } while (i1 < Data.m_Capability.Count);
            }
        }
        private void Get_TV(Requirement_Plugin.Database Data, EA.Repository Repository)
        {
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            List<DB_Insert> m_Insert = new List<DB_Insert>();
            //   string SQL = "SELECT Value, Notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
            //   string[] output = { "Value", "Notes" };


            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();

            List<DB_Return> m_TV = interface_TaggedValue.Get_Tagged_Value(Data.metamodel.SYS_Tagged_Values, this.ID, Data);

            if (m_TV != null)
            {
                string Value = "";
                string Note = "";
                string insert = "";
                bool flag_insert = false;
                string recent = "";
                int i1 = 0;
                do
                {
                    flag_insert = false;

                    if (m_TV[i1].Ret.Count > 1)
                    {
                        //m_TV[i1].Ret[1] m_TV[i1].Ret[1].ToString().Replace(" ", "");

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
                                case "SYS_FARBCODE":
                                    if(recent.Contains("#"))
                                    {
                                        this.SYS_FARBCODE = recent;
                                    }
                                    else
                                    {
                                        this.SYS_FARBCODE = null;
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
                                this.SYS_TYP = SYS_TYP.Funktionsbaum;
                                m_Insert.Add(new DB_Insert("SYS_TYP", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_TYP[(int)SYS_TYP.Funktionsbaum], -1));
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
                                this.SYS_REL_GEWICHT = "0";
                                m_Insert.Add(new DB_Insert("TagMask", OleDbType.VarChar, OdbcType.VarChar, "0", -1));
                                break;
                        }

                    }

                    i1++;
                } while (i1 < m_TV.Count);

                if (m_Insert.Count > 0)
                {
                    //Insert Befehl TV
                    string[] m_Input_Property = { "Object_ID", "Property", "Value", "Notes", "ea_guid" };
                    interface_TaggedValue.Insert_Tagged_Value(Data, m_Insert, tagged, this.ID, m_Input_Property);
                    //  Data.oLEDB_Interface.OLEDB_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, "t_objectproperties", this.ID, m_Input_Property);
                }
            }
        }

        public void Update_TV_Gewichtung(Requirement_Plugin.Database database, EA.Repository repository)
        {
            List<Database_Connection.DB_Insert> m_Insert = new List<Database_Connection.DB_Insert>();
            m_Insert.Add(new Database_Connection.DB_Insert("SYS_REL_GEWICHT", System.Data.OleDb.OleDbType.VarChar, System.Data.Odbc.OdbcType.VarChar, this.SYS_REL_GEWICHT, -1));

            this.Update_TV(m_Insert, database, repository);
        }

        public void Get_Requirements_Bewertung(Requirement_Plugin.Database database, List<Capability> m_cap)
        {
            Repository_Connectors repository_Connectors = new Repository_Connectors();
            List<string> m_help = repository_Connectors.m_Get_Client(database, this.ID, database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList());

            if (m_help != null)
            {
                if (m_help.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(m_cap.Select(x => x.Classifier_ID).ToList().Contains(m_help[i1]) ==false)
                        {
                            if (database.m_Requirement.Select(x => x.Classifier_ID).ToList().Contains(m_help[i1]) == false)
                            {
                                if (this.m_Requirement.Select(x => x.Classifier_ID).ToList().Contains(m_help[i1]) == false)
                                {
                                    Requirement requirement = new Requirement(null, database.metamodel);
                                    requirement.Classifier_ID = m_help[i1];

                                    requirement.ID = requirement.Get_Object_ID(database);

                                    requirement.Get_Anforderungsart(database);
                                    requirement.Get_RPI_Export(database);
                                    requirement.Get_Phase(database);
                                    requirement.Get_Titel_AGID(database);

                                    this.m_Requirement.Add(requirement);

                                    if (requirement.m_Capability.Contains(this) == false)
                                    {
                                        requirement.m_Capability.Add(this);
                                    }

                                    database.m_Requirement.Add(requirement);

                                }

                            }
                            else
                            {
                                if (this.m_Requirement.Select(x => x.Classifier_ID).ToList().Contains(m_help[i1]) == false)
                                {
                                    Requirement requirement = new Requirement(null, database.metamodel);
                                    requirement.Classifier_ID = m_help[i1];

                                    requirement.ID = requirement.Get_Object_ID(database);

                                    requirement.Get_Anforderungsart(database);
                                    requirement.Get_RPI_Export(database);
                                    requirement.Get_Phase(database);

                                    this.m_Requirement.Add(requirement);

                                    if (requirement.m_Capability.Contains(this) == false)
                                    {
                                        requirement.m_Capability.Add(this);
                                    }
                                }
                            }
                        }

                        
                        i1++;
                    } while (i1 < m_help.Count);

                }

                //Nur gültige zur Bewertung anstehende Anforerungen betrachten
                this.m_Requirement = this.m_Requirement.Where(x => x.RPI_Export == true && x.AFO_WV_ART == AFO_WV_ART.Anforderung && x.AFO_CPM_PHASE == AFO_CPM_PHASE.Eins).ToList();
            }
        }


        public void Set_Rang(int add_rang)
        {
            if(this.m_Child.Count == 0)
            {
                //      this.rang = this.m_Requirement.Where(y => y.AFO_OPERATIVEBEWERTUNG != AFO_OPERATIVEBEWERTUNG.kritisch).ToList().Select(x => x.rang).ToList().Take(this.m_Requirement.Where(y => y.AFO_OPERATIVEBEWERTUNG != AFO_OPERATIVEBEWERTUNG.kritisch).ToList().Count).Sum();

                List<Requirement> m_req = this.m_Requirement.Where(x => x.AFO_WV_ART == AFO_WV_ART.Anforderung && x.AFO_OPERATIVEBEWERTUNG != AFO_OPERATIVEBEWERTUNG.kritisch).ToList();

                this.rang = m_req.Select(x => x.rang).ToList().Sum();

                
                //this.rang = this.m_Requirement.Where(y => y.AFO_OPERATIVEBEWERTUNG != AFO_OPERATIVEBEWERTUNG.kritisch).ToList().Select(x => x.rang).ToList().Sum();
              

                if (this.m_Parent.Count > 0)
               {
                    this.m_Parent[0].Set_Rang(this.rang);
               }

            }
            else
            {
                this.rang = this.rang + add_rang;

                if (this.m_Parent.Count > 0)
                {
                    //this.m_Parent[0].Set_Rang(this.rang);
                    this.m_Parent[0].Set_Rang(add_rang);
                }
            }
            
        }

        public void Set_Rel_Gewicht(int rang_ges)
        {
            //Aktuelles Rel Gewicht 
            float help = (((float)this.rang / (float)rang_ges) * 100);

            int help2 = (int)Math.Floor(help);

            this.SYS_REL_GEWICHT = help2.ToString();
            //Neuer rang_ges_child
            List<Repsoitory_Elements.Capability> m_root_child = new List<Repsoitory_Elements.Capability>();
            m_root_child = this.m_Child;

            int rang_ges_child = m_root_child.Select(x => x.rang).Take(m_root_child.Count).ToList().Sum();

            if(this.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    m_root_child[i1].Set_Rel_Gewicht(rang_ges_child);

                    i1++;
                } while (i1 < this.m_Child.Count);
            }
        }

        public List<Capability> GetCapabilities_rek()
        {
            List<Capability> m_ret = new List<Capability>();

            if (this.m_Child.Count > 0)
            {
                m_ret.AddRange(this.m_Child);

                int i1 = 0;
                do
                {
                    List<Capability> m_help = this.m_Child[i1].GetCapabilities_rek();

                    if (m_help.Count > 0)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < this.m_Child.Count);
            }

            return (m_ret);
        }

    }
}

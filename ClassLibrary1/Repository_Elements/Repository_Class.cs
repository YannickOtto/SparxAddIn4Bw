using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;
using System.Data.Odbc;

using Database_Connection;
using Ennumerationen;
using Requirement_Plugin;

using System;

namespace Repsoitory_Elements
{
    public class Repository_Class : Repository_Element, IEquatable<Repository_Class>, IComparable<Repository_Class>
    {
        public string StereoType;
        public string Type; //wird zur Festlegung von Port benötigt
        public string SYS_ANSPRECHPARTNER;
        public string UUID;
        public string OBJECT_ID;
        public string SYS_AG_ID;
        public string SYS_AN_ID;
        public string SYS_KUERZEL{ get; set; }
        public string SYS_BEZEICHNUNG;
        public SYS_ARTIKEL SYS_ARTIKEL;
        public SYS_DETAILSTUFE SYS_DETAILSTUFE;
        public SYS_TYP SYS_TYP;
        public SYS_ERFUELLT_AFO SYS_ERFUELLT_AFO;
        public SYS_SUBORDINATES_AFO SYS_SUBORDINATES_AFO;
        public SYS_KOMPONENTENTYP SYS_KOMPONENTENTYP;
        public SYS_STATUS SYS_STATUS;
        public IN_CATEGORY IN_CATEGORY;
        public SYS_PRODUKT_STATUS SYS_PRODUKT_STATUS;
        public string SYS_PRODUKT;
        public string B_KENNUNG;
        public string SYS_REL_GEWICHT;
        public string W_Artikel;
        public bool RPI_Export;
        public string AFO_KLAERUNGSPUNKTE;
        public string TagMask;
        public bool W_SINGULAR;
        public string SYS_FARBCODE;

        public List<string> m_Instantiate;
        public string Instantiate_GUID;

        #region IEquatable
        public override bool Equals(object obj)
        {
            if (obj == null) return (false);
            Repository_Class recent = obj as Repository_Class;
            if (recent == null) return (false);
            else return (Equals(recent));
        }

        public bool Equals(Repository_Class other)
        {
            

            if (other == null) return (false);
            return (this.Name.Equals(other.Name));
        }
        #endregion
        #region IComparable
        public int CompareTo(Repository_Class compareClass)
        {
            if (compareClass == null) return (1);
            else
            {
                if (compareClass.Name == null || this.Name == null) return (1);
                else
                {
                    return (this.Name.CompareTo(compareClass.Name));
                }
             
            }
        }
        #endregion

        #region Update
      
        public void Update_Repository_Class(Database database, EA.Repository repository)
        {
            List<DB_Insert> m_Insert2 = new List<DB_Insert>();

            this.Get_TV_reduced(database, repository);
            string name = this.Get_Name(database);
            string notes = this.Get_Notes(database);

            if (this.Name != name)
            {
                this.Name = name;
                this.SYS_KUERZEL = name;
                m_Insert2.Add(new DB_Insert("SYS_KUERZEL", OleDbType.VarChar, OdbcType.VarChar, name, -1));
            }
            if (this.Notes != notes)
            {
                this.Notes = notes;
                this.SYS_BEZEICHNUNG = notes;
                m_Insert2.Add(new DB_Insert("SYS_BEZEICHNUNG", OleDbType.VarChar, OdbcType.VarChar, notes, -1));

            }

            if (m_Insert2.Count > 0)
            {
                this.Update_TV(m_Insert2, database, repository);
            }

        }

        #endregion

        #region TV
        public void Get_TV_reduced(Database Data, EA.Repository Repository)
        {
            Metamodels.TV_Map help;

            if (this.Classifier_ID != null)
            {
                this.ID = this.Get_Object_ID(Data);
                this.Author = this.Get_Author(Data);
                this.Name = this.Get_Name(Data);
                this.Notes = this.Get_Notes(Data);
                this.StereoType = null;
                this.Type = null;

                List<Metamodels.TV_Map> m_help = new List<Metamodels.TV_Map>();
                m_help.Add(new Metamodels.TV_Map("SYS_KUERZEL", OleDbType.VarChar, Data.metamodel.Get_XAC_Import("SYS_KUERZEL"), "not set"));
                m_help.Add(new Metamodels.TV_Map("SYS_BEZEICHNUNG", OleDbType.VarChar, Data.metamodel.Get_XAC_Import("SYS_BEZEICHNUNG"), "not set"));
                m_help.Add(new Metamodels.TV_Map("SYS_ARTIKEL", OleDbType.VarChar, Data.metamodel.Get_XAC_Import("SYS_ARTIKEL"), "not set"));
                m_help.Add(new Metamodels.TV_Map("AFO_KLAERUNGSPUNKTE", OleDbType.VarChar, Data.metamodel.Get_XAC_Import("AFO_KLAERUNGSPUNKTE"), "not set"));
                m_help.Add(new Metamodels.TV_Map("RPI_Export", OleDbType.VarChar, Data.metamodel.Get_XAC_Import("RPI_Export"), "True"));
                m_help.Add(new Metamodels.TV_Map("SYS_AG_ID", OleDbType.VarChar, Data.metamodel.Get_XAC_Import("SYS_AG_ID"), "not set"));

                TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
                List<DB_Insert> m_Insert = new List<DB_Insert>();
                //     string SQL = "SELECT Value, Notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
                //     string[] output = { "Value", "Notes" };

                //     OleDbCommand oleDbCommand = new OleDbCommand(SQL, Data.oLEDB_Interface.dbConnection);
                //      List<DB_Return> m_TV = Data.oLEDB_Interface.oleDB_SELECT_One_Table_Multiple_Property(oleDbCommand, output, m_help, this.ID);
                Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
                List<DB_Return> m_TV = interface_TaggedValue.Get_Tagged_Value(m_help, this.ID, Data);

                if (m_TV != null)
                {
                    string Value = "";
                    string Note = "";
                    string insert = "";
                    bool flag_insert = false;
                    int i1 = 0;
                    do
                    {

                        flag_insert = false;

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
                                    case "SYS_KUERZEL":
                                        this.SYS_KUERZEL = recent;
                                        this.Name = recent;
                                        break;
                                    case "SYS_BEZEICHNUNG":
                                        this.SYS_BEZEICHNUNG = recent;
                                        this.Notes = recent;
                                        break;
                                    case "AFO_KLAERUNGSPUNKTE":
                                        this.AFO_KLAERUNGSPUNKTE = recent;
                                        break;
                                    case "RPI_Export":
                                        if (recent == "True")
                                        {
                                            this.RPI_Export = true;
                                        }
                                        else
                                        {
                                            this.RPI_Export = false;
                                        }

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
                                    case "SYS_AG_ID":
                                       
                                        this.SYS_AG_ID = recent;
                                       
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

                                case "SYS_KUERZEL":
                                    this.SYS_KUERZEL = this.Name;
                                    m_Insert.Add(new DB_Insert("SYS_KUERZEL", OleDbType.VarChar, OdbcType.VarChar, this.Name, -1));
                                    break;
                                case "SYS_BEZEICHNUNG":
                                    this.SYS_BEZEICHNUNG = this.Name;
                                    m_Insert.Add(new DB_Insert("SYS_BEZEICHNUNG", OleDbType.VarChar, OdbcType.VarChar, this.Notes, -1));
                                    break;
                                case "AFO_KLAERUNGSPUNKTE":
                                    this.AFO_KLAERUNGSPUNKTE = "";
                                    m_Insert.Add(new DB_Insert("AFO_KLAERUNGSPUNKTE", OleDbType.VarChar, OdbcType.VarChar, this.AFO_KLAERUNGSPUNKTE, -1));
                                    break;
                                case "SYS_ARTIKEL":
                                    this.SYS_ARTIKEL = SYS_ARTIKEL.der;
                                    this.W_Artikel = Data.SYS_ENUM.SYS_ARTIKEL[(int)SYS_ARTIKEL.der];
                                    m_Insert.Add(new DB_Insert("SYS_ARTIKEL", OleDbType.VarChar, OdbcType.VarChar, Data.SYS_ENUM.SYS_ARTIKEL[(int)SYS_ARTIKEL.der], -1));
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

                        // Data.oLEDB_Interface.OLEDB_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, Repository, "t_objectproperties", this.ID, m_Input_Property);
                    }
                }
            }


        }
        public void Get_TV_Instantiate(Database Data, EA.Repository repository)
        {
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            List<DB_Insert> m_Insert = new List<DB_Insert>();
            //   string SQL = "SELECT Value, Notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
            //   string[] output = { "Value", "Notes" };

            //   OleDbCommand oleDbCommand = new OleDbCommand(SQL, Data.oLEDB_Interface.dbConnection);
            //   List<DB_Return> m_TV = Data.oLEDB_Interface.oleDB_SELECT_One_Table_Multiple_Property(oleDbCommand, output, Data.metamodel.SYS_Inst_Tagged_Values, this.ID);
            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
            List<DB_Return> m_TV = interface_TaggedValue.Get_Tagged_Value(Data.metamodel.SYS_Inst_Tagged_Values, this.ID, Data);

            if (m_TV != null)
            {
                string Value = "";
                string Note = "";
                string insert = "";
                bool flag_insert = false;
                bool flag = false;
                int i1 = 0;
                do
                {
                    flag = false;
                    if (m_TV[i1].Ret.Count > 1)
                    {
                        string recent = "";
                        if (m_TV[i1].Ret[1] == null && m_TV[i1].Ret[2] == null)
                        {
                            flag_insert = true;
                            flag = true;
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
                                case "RPI_Export":
                                    if(recent == "True")
                                    {
                                        this.RPI_Export = true;
                                    }
                                    else
                                    {
                                        this.RPI_Export = false;
                                    }
                                    break;
                                
                            }



                        }

                    }
                    else
                    {
                        flag_insert = true;
                        flag = true;
                    }


                    if (flag == true)
                    {
                        switch (m_TV[i1].Ret[0])
                        {
                         
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
                            case "RPI_Export":
                                this.RPI_Export = true;
                                m_Insert.Add(new DB_Insert("RPI_Export", OleDbType.VarChar, OdbcType.VarChar, true.ToString(), -1));
                                break;

                        }

                    }

                    i1++;
                } while (i1 < m_TV.Count);

                if (flag_insert == true)
                {
                    //Insert Befehl TV
                    string[] m_Input_Property = { "Object_ID", "Property", "Value", "Notes", "ea_guid" };

                    Repository_Element rep = new Repository_Element();
                    rep.Classifier_ID = this.Instantiate_GUID;
                    rep.ID = rep.Get_Object_ID(Data);

                    interface_TaggedValue.Insert_Tagged_Value(Data, m_Insert, tagged, rep.ID, m_Input_Property);

                 //   Data.oLEDB_Interface.OLEDB_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, "t_objectproperties", rep.ID, m_Input_Property);
                }
            }
        }

        public void Update_reduced(Database database, EA.Repository repository)
        {
            List<Database_Connection.DB_Insert> m_Insert = new List<Database_Connection.DB_Insert>();
            m_Insert.Add(new Database_Connection.DB_Insert("SYS_ARTIKEL", System.Data.OleDb.OleDbType.VarChar, System.Data.Odbc.OdbcType.VarChar, this.SYS_ARTIKEL, -1));
            m_Insert.Add(new Database_Connection.DB_Insert("SYS_BEZEICHNUNG", System.Data.OleDb.OleDbType.VarChar, System.Data.Odbc.OdbcType.VarChar, this.SYS_BEZEICHNUNG, -1));
            m_Insert.Add(new Database_Connection.DB_Insert("W_SINGULAR", System.Data.OleDb.OleDbType.VarChar, System.Data.Odbc.OdbcType.VarChar, this.W_SINGULAR.ToString(), -1));

            this.Update_TV(m_Insert, database, repository);
        }

        #endregion TV

        



        #region Create
        /// <summary>
        /// Es wird ein Element vom Type Class mit Stereotype, in einem PAckage und einen Elment untergeordnet angelegt
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Type"></param>
        /// <param name="StereoType"></param>
        /// <param name="ParentID"></param>
        /// <param name="Package_GUID"></param>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public string Create_Element_Class(string Name2, string Type, string StereoType, string Toolbox, int ParentID, string Package_GUID, EA.Repository Repository, string Notes, Database database)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();
            switch(database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    /*if (database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                    {
                        database.oLEDB_Interface.dbConnection.Close();
                    }*/
                    interface_Collection_OleDB.Close_Connection(database);
                    break;
                case DB_Type.MSDASQL:
                    /* if (database.oDBC_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                     {
                         database.oDBC_Interface.dbConnection.Close();
                     }
                     */
                    interface_Collection_OleDB.Close_Connection(database);
                    break;
            }


           // XML Instantiate_GUID = new XML();
           // TaggedValue tagged = new TaggedValue(database.metamodel, database);
          
            EA.Package Packages = Repository.GetPackageByGuid(Package_GUID);

         //   XML xml = new XML();

            List<string> help2 = new List<string>();

           
            Interface_Element interface_Element = new Interface_Element();
           string help = interface_Element.Check_Database_Element_Class(database, Type, StereoType, Name2, ParentID);





            if (help == null)//Object muss angelegt werden
            {
                EA.Element Element = Packages.Elements.AddNew(Name2, Type);
                Element.Stereotype = StereoType;
                Element.Notes = Notes;

               // Element.Update();
              
                Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
                interface_TaggedValue.Set_Stereotype(Element.ElementGUID, Toolbox, StereoType, database);


                if (ParentID != -1)
                {

                    interface_Element.Update_BigInt(Element.ElementGUID, ParentID, "ParentID", database);

                   
                }
                
                interface_Collection_OleDB.Open_Connection(database);

                Element.Update();
                //Repository.GetPackageByID(Element.PackageID).Update();

                return (Element.ElementGUID);

            }
            else
            {
               
                interface_Collection_OleDB.Open_Connection(database);
                //TaggedValues überprüfen
                return (help);
            }


        }
        /// <summary>
        /// Es wird ein Element vom Type Part mit Stereotype, in einem PAckage und einen Elment untergeordnet angelegt
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Type"></param>
        /// <param name="StereoType"></param>
        /// <param name="Classifier"></param>
        /// <param name="ParentID"></param>
        /// <param name="Package_GUID"></param>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public string Create_Element_Instantiate(string Name, string Type, string StereoType, string Toolbox, string Classifier, int ParentID, string Package_GUID, EA.Repository Repository, bool create, Database database)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();

            /* if (database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
             {
                 database.oLEDB_Interface.dbConnection.Close();
             }*/
            interface_Collection_OleDB.Close_Connection(database);

            if (Classifier != null && Classifier != "")
            {
             //   XML Instantiate_GUID = new XML();

                EA.Package Packages = Repository.GetPackageByGuid(Package_GUID);
                /* //Überprüfung, ob Node schon vorhanden	
                 string SQL = @"SELECT ea_guid FROM t_object 
             WHERE Object_Type = '" + Type + "'" +
                 " AND Stereotype = '" + StereoType +
                 "' AND PDATA1 = '" + Classifier + "'" +
                 " AND ParentID = " + ParentID +
                 " AND Package_ID = " + Packages.PackageID + ";";

                 string xml_String = Repository.SQLQuery(SQL);

                 List<string> help = Instantiate_GUID.Xml_Read_Attribut("ea_guid", xml_String);
                 */
                Interface_Element interface_Element = new Interface_Element();
                string help = interface_Element.Check_Database_Element_Instantiate(database, Type, StereoType, ParentID, Classifier);

                if (help == null || create == true)//Object muss angelegt werden
                {
                    DB_Command sQL_Command = new DB_Command();

                    string Type2 = "Part";

                    EA.Element Element = Packages.Elements.AddNew(Name, Type2);
                    Element.Stereotype = StereoType;
                    //     Element.ParentID = ParentID;
                    //     Element.PackageID = Packages.PackageID;
                    //Element.ClassifierType = Classifier;
                    Element.Update();
                    //Packages.Elements.Refresh();
                   // Packages.Update();


                    /*   string SQL2 = @"UPDATE t_object SET Object_Type = '"+Type+"', PDATA1 = '" + Classifier + "' , ParentID = " + ParentID + ", Stereotype = '" + StereoType +
                       "' WHERE ea_guid = '" + Element.ElementGUID + "';";

                       Repository.Execute(SQL2);*/
                    #region Update t_object

                    /*    string[] m_input_property = { "Object_Type", "PDATA1", "ParentID", "Stereotype" };
                        object[] m_input_value = { Type, Classifier, ParentID, StereoType };
                        OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar };
                        string[] m_select_property = { "ea_guid" };
                        object[] m_v_11 = { Element.ElementGUID };
                        List<object[]> m_select_value = new List<object[]>();
                        OleDbType[] m_select_Type = { OleDbType.VarChar };
                        m_select_value.Add(m_v_11);

                        //   database.oLEDB_Interface.dbConnection.Close();

                        OleDbCommand Update = sQL_Command.Get_Update_Command("t_object", m_input_property, m_input_value, m_select_property, m_select_value, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Update(Update, m_input_value, m_input_Type, m_select_value, m_select_Type);
                        database.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update);*/

                    interface_Element.Update_VarChar(Element.ElementGUID, Type, "Object_Type", database);
                    interface_Element.Update_VarChar(Element.ElementGUID, Classifier, "PDATA1", database);
                    interface_Element.Update_VarChar(Element.ElementGUID, StereoType, "Stereotype", database);
                    interface_Element.Update_BigInt(Element.ElementGUID, ParentID, "ParentID", database);

                    #endregion t_object

                    #region Update t_xref

                    /*string des = "@STEREO;Name=" + StereoType + ";FQName=" + Toolbox + "::" + StereoType + ";@ENDSTEREO;";

                    string[] m_input_property4 = { "Description", };
                    object[] m_input_value4 = { des };
                    OleDbType[] m_input_Type4 = { OleDbType.VarChar };
                    string[] m_select_property4 = { "Name", "Type", "Client" };
                    object[] m_v_1 = { "Stereotypes" };
                    object[] m_v_2 = { "element property" };
                    object[] m_v_3 = { Element.ElementGUID };
                    List<object[]> m_select_value2 = new List<object[]>();
                    OleDbType[] m_select_Type4 = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                    m_select_value2.Add(m_v_1);
                    m_select_value2.Add(m_v_2);
                    m_select_value2.Add(m_v_3);

                    OleDbCommand Update2 = sQL_Command.Get_Update_Command("t_xref", m_input_property4, m_input_value4, m_select_property4, m_select_value2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    database.oLEDB_Interface.Add_Parameters_Update(Update2, m_input_value4, m_input_Type4, m_select_value2, m_select_Type4);
                    database.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update2);
                    */
                    Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
                    interface_TaggedValue.Set_Stereotype(Element.ElementGUID, Toolbox, StereoType, database);
                    #endregion t_xref

                    // Element.Update();
                    // Packages.Elements.Refresh();
                    // Packages.Update();
                    //   database.oLEDB_Interface.dbConnection.Close();

                    Element.Update();
                   // Element.Refresh();
                   // Packages.Elements.Refresh();
                    
                   // Packages.Update();


                    // Repository.GetPackageByID(Element.PackageID).Elements.Refresh();
                    // Repository.GetPackageByID(Element.PackageID).Update();
                    /*  if (database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                      {
                          database.oLEDB_Interface.dbConnection.Open();
                      }*/
                    interface_Collection_OleDB.Open_Connection(database);

                    return (Element.ElementGUID);
                }
                else
                {
                    /*if (database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                    {
                        database.oLEDB_Interface.dbConnection.Open();
                    }*/
                    interface_Collection_OleDB.Open_Connection(database);

                    return (help);
                }
            }
            else
            {
                /*  if (database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                  {
                      database.oLEDB_Interface.dbConnection.Open();
                  }*/
                interface_Collection_OleDB.Open_Connection(database);
                return (null);
            }


        }

       

        #endregion Create

        #region Get
        /// <summary>
        /// Es wird der Classifier der Instanz als GUID zurückgeben.
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public string Get_Classifier_Part(Database database)
        {
            if (this.Classifier_ID != null)
            {
                return(this.Get_Classifier(database));


               
            }
            else
            {
                return (null);
            }

        }

        #endregion Get


    }
}

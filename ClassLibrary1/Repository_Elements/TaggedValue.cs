using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;

using Database_Connection;
using Metamodels;
using Requirement_Plugin;

namespace Repsoitory_Elements
{
    public class TaggedValue
    {
        Metamodel metamodel;
        Database Data;

        public TaggedValue(Metamodel metamodel, Database Data)
        {
            this.metamodel = metamodel;
            this.Data = Data;
        }

        #region Select Elements
        /// <summary>
        /// Überprüft ob Tagged Valie vorhanden und wird zurückgegeb nals string
        /// </summary>
        /// <param name="Element_GUID"></param>
        /// <param name="Property"></param>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public string Get_Tagged_Value(string Element_GUID, string Property, EA.Repository Repository)
        {

            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
            return (interface_TaggedValue.Get_Tagged_Value_By_Property(this.Data, Property, Element_GUID));
         //   XML xML = new XML();

            //  MessageBox.Show("GetTagged1");

          /*  if (Element_GUID != null && Element_GUID != "")
            {
                int index = metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                if (index != -1)
                {
                    Property = metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                }

                XML xml = new XML();
                //SQL_Command command = new SQL_Command();
                OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar };
                string table = "t_object";
                string[] m_output = { "Value" };

                string ret = "SELECT Value FROM t_objectproperties WHERE Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?) AND Property = ?";
                OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
                oleDbCommand.Parameters.Add("?", m_input_Type[0]).Value = Element_GUID;
                oleDbCommand.Parameters.Add("?", m_input_Type[1]).Value = Property;

                List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);

                if (m_ret3[0].Ret.Count > 1)
                {
                    if (m_ret3[0].Ret[1] == "<memo>")
                    {
                        return (this.Get_Tagged_Value_Notes(Element_GUID, Property, Repository));
                    }

                    return (m_ret3[0].Ret[1].ToString());

                    //  MessageBox.Show("GetTagged3");
                }

            }

            return (null);
            */
        }
        /// <summary>
        /// Überprüft ob Tagged Valie vorhanden und Notes wird zurückgegeben als string
        /// </summary>
        /// <param name="Element_GUID"></param>
        /// <param name="Property"></param>
        /// <param name="Repository"></param>
        /// <returns></returns>
     /*   public string Get_Tagged_Value_Notes(string Element_GUID, string Property, EA.Repository Repository)
        {
            XML xML = new XML();

            //  MessageBox.Show("GetTagged1");

            if (Element_GUID != null)
            {
                int index = metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                if (index != -1)
                {
                    Property = metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                }

                XML xml = new XML();
                //SQL_Command command = new SQL_Command();
                OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar };
                string table = "t_object";
                string[] m_output = { "Notes" };

                string ret = "SELECT Notes FROM t_objectproperties WHERE Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?) AND Property = ?";
                OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                oleDbCommand.Parameters.Add("?", m_input_Type[0]).Value = Element_GUID;
                oleDbCommand.Parameters.Add("?", m_input_Type[1]).Value = Property;

                List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);

                if (m_ret3[0].Ret.Count > 1)
                {

                    return (m_ret3[0].Ret[1].ToString());

                    //  MessageBox.Show("GetTagged3");
                }
            
            }

            return (null);
        }*/

        public int Get_ID_From_Value(string Value, string Property, EA.Repository repository)
        {
            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
            return (interface_TaggedValue.Get_ID(this.Data, Property, Value));
            /*   XML xML = new XML();

               if (Value != null)
               {
                   int index = metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                   if (index != -1)
                   {
                       Property = metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                   }

                   XML xml = new XML();
                   //SQL_Command command = new SQL_Command();
                   OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar };
                   string table = "t_object";
                   string[] m_output = { "Object_ID" };

                   string ret = "SELECT Object_ID FROM t_objectproperties WHERE Property  = ? AND Value = ?";
                   OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                   oleDbCommand.Parameters.Add("?", m_input_Type[0]).Value = Property;
                   oleDbCommand.Parameters.Add("?", m_input_Type[1]).Value = Value;

                   List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);

                   if (m_ret3[0].Ret.Count > 1)
                   {

                       return (m_ret3[0].Ret[1].ToString());

                       //  MessageBox.Show("GetTagged3");
                   }

               }

               return (null);
               */
        }

        public string Get_GUID_From_ID(int Value, EA.Repository repository)
        {
            Repository_Element repository_Element = new Repository_Element();
            return(repository_Element.Get_GUID_By_ID(Value, this.Data));

        /*    XML xML = new XML();

            if (Value != null)
            {

                XML xml = new XML();
                //SQL_Command command = new SQL_Command();
                OleDbType[] m_input_Type = { OleDbType.VarChar };
                string table = "t_object";
                string[] m_output = { "ea_guid" };

                string ret = "SELECT ea_guid FROM t_object WHERE Object_ID = ?";
                OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                oleDbCommand.Parameters.Add("?", m_input_Type[0]).Value = Value;

                List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);

                if (m_ret3[0].Ret.Count > 1)
                {

                    return (m_ret3[0].Ret[1].ToString());

                    //  MessageBox.Show("GetTagged3");
                }

            }

            return (null);
            */
        }

        public string Get_Datetime()
        {
            DateTime recent_Date = new DateTime();
            recent_Date = DateTime.Now;

            string actualdate_Element = recent_Date.ToString("o");
            if (actualdate_Element.Split('+').Length == 1)
            {
                actualdate_Element = actualdate_Element + "+02:00";
            }
            //WErtkorrigieren auf das zu nutzend Format
            if (actualdate_Element.Split('+')[0].Length != 27)
            {
                string help = actualdate_Element.Split('+')[0];

                if (actualdate_Element.Split('+')[0].Length < 27)
                {
                    //verlängern
                    do
                    {
                        help = help + "0";

                    } while (help.Length < 27);
                }
                else
                {
                    //verkürzen
                    do
                    {
                        help = help.Remove(help.Length - 1, 1);

                    } while (help.Length > 27);

                }

                // MessageBox.Show(help + "+" + import_modified.Split('+')[1]);

                actualdate_Element = help;
            }
            // MessageBox.Show(import_modified.Split('+')[0].Length.ToString());

            return (actualdate_Element);

        }

        public List<string> Get_Distinct_Property(string Property, int Value)
        {
            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
            return (interface_TaggedValue.Get_Distinct_Property(this.Data, Property, Value));
            /*    List<string> ret_all = new List<string>();
                XML xML = new XML();

                string str_Value = "Value";

                if(Value != 0)
                {
                    str_Value = "Notes";
                }
                //  MessageBox.Show("GetTagged1");

                if (Property != "")
                {
                    int index = metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                    if (index != -1)
                    {
                        Property = metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                    }

                    XML xml = new XML();
                    //SQL_Command command = new SQL_Command();
                    OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar };
                    string table = "t_object";

                    string[] m_output = { str_Value};

                    string ret = "SELECT DISTINCT Value FROM t_objectproperties WHERE Property = ?";
                    OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
                    oleDbCommand.Parameters.Add("?", m_input_Type[1]).Value = Property;

                    string[] m_output2 = { "Notes" };

                    List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);

                    string ret2 = "SELECT DISTINCT Notes FROM t_objectproperties WHERE Property = ? AND Value = ?";
                    OleDbCommand oleDbCommand2 = new OleDbCommand(ret2, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
                    oleDbCommand2.Parameters.Add("?", m_input_Type[1]).Value = Property;
                    oleDbCommand2.Parameters.Add("?", m_input_Type[1]).Value = "<memo>";

                    List<DB_Return> m_ret4 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand2, m_output2);


                    if (m_ret3[0].Ret.Count > 1)
                    {
                        m_ret3[0].Ret.ForEach(x => x.ToString());
                        ret_all.AddRange(m_ret3[0].Ret.Select(x => x.ToString()).ToList().Where(x => x.ToString() != str_Value));
                    }
                    if(m_ret4[0].Ret.Count > 1)
                    {
                        ret_all.AddRange(m_ret4[0].Ret.Select(x => x.ToString()).ToList().Where(x => x.ToString() != "Notes"));
                    }

                    return (ret_all);

                }

                return(null);
                */
        }

       public List<string> Get_Distinct_Property_multiple(List<string> m_Property, int ID, Database Data)
        {
            List<NodeType> ret_all = new List<NodeType>();
            // XML xML = new XML();
            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
            Repository_Element repository_Element = new Repository_Element();
            List<string> m_ret = new List<string>(); 

            if (m_Property.Count > 0)
            {
                int i1 = 0;
                do
                {

                    int index = metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(m_Property[i1]);

                    if (index != -1)
                    {
                        m_Property[i1] = metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                    }

                    repository_Element.ID = ID;
                    m_ret.Add(interface_TaggedValue.Get_Tagged_Value_By_Property(Data, m_Property[i1], repository_Element.Get_GUID_By_ID(ID, Data)));

                    i1++;
                } while (i1 < m_Property.Count);


                return (m_ret);
          /*      DB_Command command = new DB_Command();


                OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.BigInt };
                string table = "t_objectproperties";
                string[] m_output = { "Object_ID", "Value", "Notes" };
                string[] m_input = { "Property", "Object_ID" };
                List<DB_Input[]> ee = new List<DB_Input[]>();
                DB_Input[] tt = { new DB_Input(-1, m_Property[0]) };
                ee.Add(tt);
                ee.Add(m_ID.Select(x => new DB_Input(x, null)).ToArray());
                List<DB_Input[]> ee2 = new List<DB_Input[]>();
                DB_Input[] tt2 = { new DB_Input(-1, m_Property[1]) };
                ee2.Add(tt2);
                ee2.Add(m_ID.Select(x => new DB_Input(x, null)).ToArray());

                string select = command.Get_Select_Command(table, m_output, m_input, ee);
                OleDbCommand SELECT = new OleDbCommand(select, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                Data.oLEDB_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type);
                List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output);

                string select2 = command.Get_Select_Command(table, m_output, m_input, ee2);
                OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee2, m_input_Type);
                List<DB_Return> m_ret4 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);

                if (m_ret3[1].Ret.Count > 1)
                {
                    i1 = 1;
                    do
                    {
                        NodeType recent = new NodeType(null, null, null);

                        recent.ID = (int)m_ret3[0].Ret[i1];
                        recent.SYS_KUERZEL = m_ret3[1].Ret[i1].ToString();

                        if(ret_all.Contains( recent) == false)
                        {
                            ret_all.Add(recent);
                        }


                        i1++;
                    } while (i1 < m_ret3[0].Ret.Count);
                }
                if (m_ret4[1].Ret.Count > 1)
                {
                    i1 = 1;
                    do
                    {
                        List<NodeType> m_recent = new List<NodeType>();
                        m_recent = ret_all.Where(x => x.ID == (int)m_ret4[0].Ret[i1]).ToList();

                        m_recent.ForEach(x => x.SYS_ARTIKEL = (SYS_ARTIKEL)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_ARTIKEL, m_ret4[1].Ret[i1].ToString()));
                        m_recent.ForEach(x => x.W_Artikel = Data.SYS_ENUM.SYS_ARTIKEL[(int)x.SYS_ARTIKEL]);

                        i1++;
                    } while (i1 < m_ret4[0].Ret.Count);
                }

                return (ret_all);
                */
            }
            

            return (null);
        }
        
        #endregion Select Elements

        #region Update Elements
        /// <summary>
        /// Tagged Value wird geupdatet. WEnn nicht vorfhanden wir dieser angelegt.
        /// </summary>
        /// <param name="Element_GUID"></param>
        /// <param name="Property"></param>
        /// <param name="Value"></param>
        /// <param name="Notes"></param>
        /// <param name="Repository"></param>
        public void Update_Tagged_Value(string Element_GUID, string Property, string Value, string Notes, EA.Repository Repository)
        {
            if (Element_GUID != null)
            {
                if (this.Get_Tagged_Value(Element_GUID, Property, Repository) == null)
                {
                    //Tagged Value in Table einfügen
                    Insert_Tagged_Value(Element_GUID, Property, Value, Notes, Repository);
                }

                int index = metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                if (index != -1)
                {
                    Property = metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                }

                if (Value == null)
                {
                    Value = " ";
                }
                if (Value.Length == 0)
                {
                    Value = " ";
                }
            /*    if (Value.Length > 255)
                {
                    Notes = Value;
                    Value = "<memo>";
                }
                */
                if (Notes == null || Notes.Length == 0)
                {
                    Notes = " ";
                }

                DB_Command sQL_Command = new DB_Command();

                //Object ID Erhalten
                #region Object_ID
                /*      List<DB_Input[]> ee = new List<DB_Input[]>();

                      string[] m_output = { "Object_ID" };
                      string[] m_input_Property = { "ea_guid" };
                      DB_Input[] m_input_Value1 = { new DB_Input(-1, Element_GUID) };
                      OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar };

                      ee.Add(m_input_Value1);



                      string select2 = sQL_Command.Get_Select_Command("t_object", m_output, m_input_Property, ee);
                      OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
                      this.Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type);
                      List<DB_Return> m_object_ID = this.Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                      */
                Repository_Element repository_Element = new Repository_Element();
                repository_Element.Classifier_ID = Element_GUID;
                repository_Element.ID = repository_Element.Get_Object_ID(this.Data);
                #endregion Object_ID

                #region Update
                if (repository_Element.ID != null)
                {
                    Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
                    List<DB_Insert> dB_Inserts = new List<DB_Insert>();
                    dB_Inserts.Add(new DB_Insert(Property, OleDbType.VarChar, System.Data.Odbc.OdbcType.VarChar, Value, -1));
                    interface_TaggedValue.Update_Tagged_Value(dB_Inserts, this.Data, repository_Element.ID);


                  /*  string[] m_input_property4 = { "Value", "Notes" };
                    string[] m_input_value4 = { Value, Notes };
                    OleDbType[] m_input_Type4 = { OleDbType.VarChar, OleDbType.VarChar };
                    string[] m_select_property4 = { "Object_ID", "Property" };
                    object[] m_v_1 = { repository_Element.ID };
                    object[] m_v_2 = { Property };
                    List<object[]> m_select_value = new List<object[]>();
                    OleDbType[] m_select_Type4 = { OleDbType.BigInt, OleDbType.VarChar };
                    m_select_value.Add(m_v_1);
                    m_select_value.Add(m_v_2);

                    OleDbCommand Update = sQL_Command.Get_Update_Command("t_objectproperties", m_input_property4, m_input_value4, m_select_property4, m_select_value, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
                    this.Data.oLEDB_Interface.Add_Parameters_Update(Update, m_input_value4, m_input_Type4, m_select_value, m_select_Type4);
                    this.Data.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update);*/
                }
                #endregion Updated
            }


          
        }

        /// <summary>
        /// Beachten, dass einige Table ID und andere GUID brauchen
        /// </summary>
        /// <param name="table"></param>
        /// <param name="Value"></param>
        /// <param name="Property"></param>
        /// <param name="GUID"></param>
  /*      public void UPDATE_SQL_t_object(string Value, string Property, string GUID, EA.Repository repository)
        {
            DB_Command sQL_Command = new DB_Command();

            string[] m_input_property4 = { Property };
            string[] m_input_value4 = { Value };
            OleDbType[] m_input_Type4 = { OleDbType.VarChar };
            string[] m_select_property4 = { "ea_guid" };
            object[] m_v_1 = { GUID };
            List<object[]> m_select_value = new List<object[]>();
            OleDbType[] m_select_Type4 = { OleDbType.VarChar };
            m_select_value.Add(m_v_1);

            OleDbCommand Update = sQL_Command.Get_Update_Command("t_object", m_input_property4, m_input_value4, m_select_property4, m_select_value, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
            this.Data.oLEDB_Interface.Add_Parameters_Update(Update, m_input_value4, m_input_Type4, m_select_value, m_select_Type4);
            this.Data.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update);

        }
        */
        public void UPDATE_SQL_t_xref(string Value, string Property, string GUID, EA.Repository repository)
        {
            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();

            interface_TaggedValue.Update_Tagged_Value_Reference(this.Data, GUID, Property, Value);

        /*    DB_Command sQL_Command = new DB_Command();

            string[] m_input_property4 = { Property };
            string[] m_input_value4 = { Value };
            OleDbType[] m_input_Type4 = { OleDbType.VarChar };
            string[] m_select_property4 = { "Client" };
            object[] m_v_1 = { GUID };
            List<object[]> m_select_value = new List<object[]>();
            OleDbType[] m_select_Type4 = { OleDbType.VarChar };
            m_select_value.Add(m_v_1);

            OleDbCommand Update = sQL_Command.Get_Update_Command("t_xref", m_input_property4, m_input_value4, m_select_property4, m_select_value, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
            this.Data.oLEDB_Interface.Add_Parameters_Update(Update, m_input_value4, m_input_Type4, m_select_value, m_select_Type4);
            this.Data.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update);
            */
            /*
            Repository_Elements repository_Elements = new Repository_Elements();

            Value = repository_Elements.Correct_SQL_Strings(Value);

            string SQL = "UPDATE t_xref SET [" + Property + "] = '" + Value + "' WHERE Client = '" + GUID + "';";

            // MessageBox.Show(SQL);

            repository.Execute(SQL);*/
        }

        #endregion Update Elements

        #region Insert Elements

        /// <summary>
        /// Tagged Value wird angelegt. Wenn vorhanden, wird dieser nur geupdatet
        /// </summary>
        /// <param name="Element_GUID"></param>
        /// <param name="Property"></param>
        /// <param name="Value"></param>
        /// <param name="Notes"></param>
        /// <param name="Repository"></param>
        public void Insert_Tagged_Value(string Element_GUID, string Property, string Value, string Notes, EA.Repository Repository)
        {
            if (Element_GUID != null)
            {
                if (Get_Tagged_Value(Element_GUID, Property, Repository) == null)
                {
                    Repository_Elements repository_Elements = new Repository_Elements();
                    //    MessageBox.Show("InserTagged1");

                    if (Value == null)
                    {
                        Value = "";
                    }


                    if (Element_GUID != null && Value.Length > 0)
                    {
                        int index = metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                        if (index != -1)
                        {
                            Property = metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                        }

/*if (Value == null)
                        {
                            Value = " ";
                        }
                        if (Value.Length == 0)
                        {
                            Value = " ";
                        }
                        if (Value.Length > 255)
                        {
                            Notes = Value;
                            Value = "<memo>";
                        }

                        if (Notes == null || Notes.Length == 0)
                        {
                            Notes = " ";
                        }*/

                        /*     DB_Command sQL_Command = new DB_Command();

                             //Object ID Erhalten
                             #region Object_ID
                             List<DB_Input[]> ee = new List<DB_Input[]>();

                             string[] m_output = { "Object_ID" };
                             string[] m_input_Property = { "ea_guid" };
                             DB_Input[] m_input_Value1 = { new DB_Input(-1, Element_GUID) };
                             OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar };

                             ee.Add(m_input_Value1);

                             string select2 = sQL_Command.Get_Select_Command("t_object", m_output, m_input_Property, ee);
                             OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
                             this.Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type);
                             List<DB_Return> m_object_ID = this.Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                             */
                        Repository_Element repository_Element = new Repository_Element();
                        repository_Element.Classifier_ID = Element_GUID;
                        repository_Element.ID = repository_Element.Get_Object_ID(this.Data);
                     //   #endregion Object_ID

                        #region Insert
                        if (repository_Element.ID != null)
                        {
                            List<DB_Insert> dB_Inserts = new List<DB_Insert>();
                            dB_Inserts.Add(new DB_Insert(Property, OleDbType.VarChar, System.Data.Odbc.OdbcType.VarChar, Value, -1));
                            repository_Element.Insert_TV(dB_Inserts, this.Data, Repository);
                         /*   string[] m_input_Property3 = { "Object_ID", "Property", "Value", "Notes", "ea_guid" };
                            object[] m_input_Value3 = { m_object_ID[0].Ret[1], Property, Value, Notes, this.Generate_GUID("t_objectproperties") };
                            OleDbType[] m_input_Type3 = { OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };

                            string insert = sQL_Command.Get_Insert_Command("t_objectproperties", m_input_Property3, m_input_Value3);

                            OleDbCommand Insert =  new OleDbCommand(insert, (OleDbConnection)this.Data.oLEDB_Interface.dbConnection);
                                this.Data.oLEDB_Interface.Add_Parameters_Insert(Insert, m_input_Value3, m_input_Type3);
                            this.Data.oLEDB_Interface.OLEDB_INSERT_One_Table(Insert);
                            */
                        }

                        #endregion Insert
                    }

                }
                else
                {
                    //vorhanden --> Updaten
                    Update_Tagged_Value(Element_GUID, Property, Value, Notes, Repository);
                }




                /*

                //    SQL = "INSERT INTO t_objectproperties (Object_ID, Property, [Value], Notes, ea_guid) VALUES (" + element.ElementID + ", '" + Property + "', '" + Value + "', '" + Notes + "', '" + GUID + "' );";


                EA.Element element = Repository.GetElementByGuid(Element_GUID);

                //   MessageBox.Show("InserTagged2");
                //Vorhanden?
                if (Get_Tagged_Value(Element_GUID, Property, Repository) == null)
                {
                    //nicht vorhanden 
                    string GUID = this.Generate_GUID("t_objectproperties", Repository);

                    //Korrektur der Strings
                    Value = repository_Elements.Correct_SQL_Strings(Value);
                    Notes = repository_Elements.Correct_SQL_Strings(Notes);
                    // MessageBox.Show("Value: " + Value);
                    int index = metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                    if (index != -1)
                    {
                        Property = metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                    }


                    string SQL = "";
                    if (Value.Length > 255)
                    {
                        SQL = @"INSERT INTO t_objectproperties (Object_ID, Property, [Value], [Notes], ea_guid) VALUES (" + element.ElementID + ", '" + Property + "', '<memo>', '" + Value + "', '" + GUID + "' );";
                    }
                    else
                    {
                        if (Notes != null)
                        {
                            SQL = @"INSERT INTO t_objectproperties (Object_ID, Property, [Value], Notes, ea_guid) VALUES (" + element.ElementID + ", '" + Property + "', '" + Value + "', '" + Notes + "', '" + GUID + "' );";

                        }
                        else
                        {
                            SQL = @"INSERT INTO t_objectproperties (Object_ID, Property, [Value], ea_guid) VALUES (" + element.ElementID + ", '" + Property + "', '" + Value + "', '" + GUID + "' );";

                        }
                    }
                    // string SQL = @"INSERT INTO t_objectproperties (Object_ID, Property, Value, Notes, ea_guid) VALUES (" + element.ElementID + ", " + Property + ", " + Value + ", " + Notes + ", " + GUID + " );";
                    //  string SQL = @"INSERT INTO t_objectproperties (Object_ID, Property, [Value], ea_guid) VALUES (" + element.ElementID + ", '" + Property + "', '" + Value + "', '" + GUID + "' );";
                    //  MessageBox.Show("InserTagged3");
                    //MessageBox.Show(SQL);

                    Repository.Execute(SQL);

                }
                else
                {
                    //vorhanden --> Updaten
                    Update_Tagged_Value(Element_GUID, Property, Value, Notes, Repository);
                }*/
                
            }
        }

        #endregion Insert Elements

        #region UUID/GUID
        /// <summary>
        /// Genererierung einer GUID
        /// </summary>
        /// <returns></returns>
        public string Generate_GUID(string table)
        {
            string GUID;
           // XML xmL = new XML();
            System.Guid guid;
            guid = System.Guid.NewGuid();

            GUID = "{" + guid.ToString() + "}";

            if (table != null)
            {
                //ÜBerprüfung, ob GUID schon vorhanden
           /*     string SQL = "SELECT ea_guid FROM " + table + " WHERE ea_guid = '" + GUID + "'";
                string xml_String = repository.SQLQuery(SQL);
                List<string> m_GUID = (xmL.Xml_Read_Attribut("ea_guid", xml_String));
*/
                List<string> m_GUID = new List<string>();

                if (table == "t_xref")
                {
                    m_GUID = this.Check_GUID(table, "XrefID", GUID, Data);
                }
                else
                {
                    m_GUID = this.Check_GUID(table, "ea_guid", GUID, Data);
                }


                if (m_GUID != null)
                {
                    GUID = Generate_GUID(table);
                }
            }

            return (GUID);

        }
        /// <summary>
        /// Genererierung einer UUID
        /// </summary>
        /// <returns></returns>
        public string Generate_UUID()
        {
            string GUID;
            System.Guid guid;
            guid = System.Guid.NewGuid();

            GUID = guid.ToString();

            return (GUID);

        }

        private List<string> Check_GUID(string _table, string _return, string GUID, Database Data)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();
            return (interface_Collection_OleDB.Check_GUID(Data, GUID, _table, _return));
          /*  List<string> _ret = new List<string>();
            XML xml = new XML();
            //SQL_Command command = new SQL_Command();
            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar };
            string table = _table;
            string[] m_output = { _return };

            string ret = "SELECT " + _return + " FROM " + table + " WHERE " + _return + " = ?";
            OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
            oleDbCommand.Parameters.Add("?", OleDbType.VarChar).Value = GUID;

            List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);

            if (m_ret3[0].Ret.Count > 1)
            {
                _ret.Add(m_ret3[0].Ret[1].ToString());
                return (_ret);
            }
            else
            {
                return (null);


            }*/
        }
        #endregion UUID/GUID
    }
}
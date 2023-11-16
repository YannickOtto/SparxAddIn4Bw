using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;
using Microsoft.Data.Sqlite;

using Database_Connection;
using Ennumerationen;
using Metamodels;
using Repsoitory_Elements;
using Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using EA;

namespace Requirement_Plugin.Interfaces
{
    public class Interface_TaggedValue
    {
        public string Get_Tagged_Value_By_Property(Database Data, string Property, string Element_GUID )
        {
            List<DB_Return> m_ret3 = new List<DB_Return>();
            if (Element_GUID != null && Element_GUID != "")
            {
                int index = Data.metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                if (index != -1)
                {
                    Property = Data.metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                }

           //     XML xml = new XML();
                //SQL_Command command = new SQL_Command();
                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };


                string[] m_output = { "Value" };
                string ret = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    ret = "SELECT Value FROM t_objectproperties WHERE Property = @Property_1_0 AND Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0)";
                }
                else
                {
                    ret = "SELECT Value FROM t_objectproperties WHERE Property = ? AND Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?)";
                }

               
               

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        oleDbCommand.Parameters.Add("?", m_input_Type_oledb[0]).Value = Element_GUID;
                        oleDbCommand.Parameters.Add("?", m_input_Type_oledb[1]).Value = Property;
                        m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand odbcCommand = new OdbcCommand(ret, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        odbcCommand.Parameters.Add("?", m_input_Type_odbc[1]).Value = Property;
                        odbcCommand.Parameters.Add("?", m_input_Type_odbc[0]).Value = Element_GUID;
                        m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(odbcCommand, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Property");
                        m_str.Add("ea_guid");

                        sqliteTypes.Add(SqliteType.Text);
                   

                        List<DB_Input[]> ee = new List<DB_Input[]>();
                        List<string> help_guid = new List<string>();
                        help_guid.Add(Property);
                        List<string> help_guid2 = new List<string>();
                        help_guid2.Add(Element_GUID);
                        ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_guid2.Select(x => new DB_Input(-1, x)).ToArray());

                        string[] m_output2 = { "Value" };

                        SqliteCommand SELECT4 = new SqliteCommand(ret, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret3 = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output2, sqliteTypes);
                        break;
                }

                if (m_ret3[0].Ret.Count > 1)
                {
                    if (m_ret3[0].Ret[1].ToString() == "<memo>" || m_ret3[0].Ret[1] == "<memo>")
                    {
                        return (this.Get_Tagged_Value_Notes_By_Property(Data,  Property, Element_GUID));

                    }

                    return (m_ret3[0].Ret[1].ToString());

                    //  MessageBox.Show("GetTagged3");
                }

            }

            return (null);
        }

        public string Get_Tagged_Value_Notes_By_Property(Database Data, string Property, string Element_GUID)
        {
            List<DB_Return> m_ret3 = new List<DB_Return>();

            if (Element_GUID != null && Element_GUID != "")
            {
                int index = Data.metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                if (index != -1)
                {
                    Property = Data.metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                }

              //  XML xml = new XML();
                //SQL_Command command = new SQL_Command();
                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };
                //  string table = "t_object";
                string[] m_output = { "Notes" };


                string ret = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    ret = "SELECT Notes FROM t_objectproperties WHERE Property = @Property_1_0 AND Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0)";
                }
                else
                {
                    ret = "SELECT Notes FROM t_objectproperties WHERE Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?) AND Property = ?";
                }

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        oleDbCommand.Parameters.Add("?", m_input_Type_oledb[0]).Value = Element_GUID;
                        oleDbCommand.Parameters.Add("?", m_input_Type_oledb[1]).Value = Property;
                        m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand odbcCommand = new OdbcCommand(ret, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        odbcCommand.Parameters.Add("?", m_input_Type_odbc[0]).Value = Element_GUID;
                        odbcCommand.Parameters.Add("?", m_input_Type_odbc[1]).Value = Property;
                        m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(odbcCommand, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Property");
                        m_str.Add("ea_guid");

                        sqliteTypes.Add(SqliteType.Text);

                        List<DB_Input[]> ee = new List<DB_Input[]>();
                        List<string> help_guid = new List<string>();
                        help_guid.Add(Property);
                        List<string> help_guid2 = new List<string>();
                        help_guid2.Add(Element_GUID);
                        ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_guid2.Select(x => new DB_Input(-1, x)).ToArray());

                        SqliteCommand SELECT4 = new SqliteCommand(ret, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret3 = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret3[0].Ret.Count > 1)
                {

                    return (m_ret3[0].Ret[1].ToString());

                    //  MessageBox.Show("GetTagged3");
                }

            }

            return (null);
        }

        public int Get_ID(Database Data, string Property, string Value)
        {
            List<DB_Return> m_ret3 = new List<DB_Return>();

            if (Value != null)
            {
                int index = Data.metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                if (index != -1)
                {
                    Property = Data.metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                }

              //  XML xml = new XML();
                //SQL_Command command = new SQL_Command();
                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };

                string[] m_output = { "Object_ID" };

                string ret = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    ret = "SELECT Object_ID FROM t_objectproperties WHERE Property  = @Property_1_0 AND Value = @Value_1_0";
                }
                else
                {
                    ret = "SELECT Object_ID FROM t_objectproperties WHERE Property  = ? AND Value = ?";
                }

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        oleDbCommand.Parameters.Add("?", m_input_Type_oledb[0]).Value = Property;
                        oleDbCommand.Parameters.Add("?", m_input_Type_oledb[1]).Value = Value;
                        m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand odbcCommand = new OdbcCommand(ret, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        odbcCommand.Parameters.Add("?", m_input_Type_odbc[0]).Value = Property;
                        odbcCommand.Parameters.Add("?", m_input_Type_odbc[1]).Value = Value;
                        m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(odbcCommand, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Property");
                        m_str.Add("Value");

                        sqliteTypes.Add(SqliteType.Integer);


                        List<DB_Input[]> ee = new List<DB_Input[]>();
                        List<string> help_guid = new List<string>();
                        help_guid.Add(Property);
                        List<string> help_guid2 = new List<string>();
                        help_guid2.Add(Value);
                        ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_guid2.Select(x => new DB_Input(-1, x)).ToArray());

                        string[] m_output2 = { "Object_ID" };

                        SqliteCommand SELECT4 = new SqliteCommand(ret, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret3 = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output2, sqliteTypes);
                        break;
                }

                if (m_ret3[0].Ret.Count > 1)
                {

                    return ((int) m_ret3[0].Ret[1]);

                    //  MessageBox.Show("GetTagged3");
                }

            }

            return (-1);
        }

        public List<string> Get_Distinct_Property(Database Data, string Property, int Value)
        {
            List<string> ret_all = new List<string>();
        //    XML xML = new XML();

            string str_Value = "Value";

            if (Value != 0)
            {
                str_Value = "Notes";
            }
            //  MessageBox.Show("GetTagged1");

            if (Property != "")
            {
                int index = Data.metamodel.AFO_TAGGED_VALUES_EXPORT_XAC.IndexOf(Property);

                if (index != -1)
                {
                    Property = Data.metamodel.AFO_TAGGED_VALUES_IMPORT_XAC[index];
                }

             //   XML xml = new XML();
                //SQL_Command command = new SQL_Command();
                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite1 = { SqliteType.Text};
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };
                // string table = "t_object";

                string[] m_output = { str_Value };


                string ret = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    ret = "SELECT DISTINCT Value FROM t_objectproperties WHERE Property = @Property_1_0";
                }
                else
                {
                    ret = "SELECT DISTINCT Value FROM t_objectproperties WHERE Property = ?";
                }

                string[] m_output2 = { "Notes" };

                List<DB_Return> m_ret3 = new List<DB_Return>();

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        oleDbCommand.Parameters.Add("?", m_input_Type_oledb[1]).Value = Property;
                        m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand odbcCommand = new OdbcCommand(ret, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        odbcCommand.Parameters.Add("?", m_input_Type_odbc[1]).Value = Property;
                        m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(odbcCommand, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Property");
                        
                        sqliteTypes.Add(SqliteType.Text);


                        List<DB_Input[]> ee = new List<DB_Input[]>();
                        List<string> help_guid = new List<string>();
                        help_guid.Add(Property);
                        ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                        string[] m_output3 = { "Value" };

                        SqliteCommand SELECT4 = new SqliteCommand(ret, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite1, m_str);
                        m_ret3 = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output3, sqliteTypes);
                        break;
                }

                string ret2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    ret2 = "SELECT DISTINCT Notes FROM t_objectproperties WHERE Property = @Property_1_0 AND Value = @Value_1_0";
                }
                else
                {
                    ret2 = "SELECT DISTINCT Notes FROM t_objectproperties WHERE Property = ? AND Value = ?";
                }

                List<DB_Return> m_ret4 = new List<DB_Return>();

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand oleDbCommand2 = new OleDbCommand(ret2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        oleDbCommand2.Parameters.Add("?", m_input_Type_oledb[0]).Value = Property;
                        oleDbCommand2.Parameters.Add("?", m_input_Type_oledb[1]).Value = "<memo>";
                        m_ret4 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand2, m_output2);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand odbcCommand2 = new OdbcCommand(ret, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        odbcCommand2.Parameters.Add("?", m_input_Type_odbc[0]).Value = Property;
                        odbcCommand2.Parameters.Add("?", m_input_Type_odbc[1]).Value = "<memo>";
                        m_ret4 = Data.oDBC_Interface.DB_SELECT_One_Table(odbcCommand2, m_output2);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Property");
                        m_str.Add("Value");

                        sqliteTypes.Add(SqliteType.Text);


                        List<DB_Input[]> ee = new List<DB_Input[]>();
                        List<string> help_guid = new List<string>();
                        help_guid.Add(Property);
                        List<string> help_guid2 = new List<string>();
                        help_guid2.Add("<memo>");
                        ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_guid2.Select(x => new DB_Input(-1, x)).ToArray());

                        string[] m_output4 = { "Notes" };

                        SqliteCommand SELECT4 = new SqliteCommand(ret2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret4 = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output4, sqliteTypes);
                        break;
                }

                if (m_ret3[0].Ret.Count > 1)
                {
                    m_ret3[0].Ret.ForEach(x => x.ToString());
                    ret_all.AddRange(m_ret3[0].Ret.Select(x => x.ToString()).ToList().Where(x => x.ToString() != str_Value));
                }
                if (m_ret4[0].Ret.Count > 1)
                {
                    ret_all.AddRange(m_ret4[0].Ret.Select(x => x.ToString()).ToList().Where(x => x.ToString() != "Notes"));
                }

                return (ret_all);

            }

            return (null);
        }

        public List<DB_Return> Get_Tagged_Value(List<TV_Map> tV_Maps, int id, Database Data)
        { 
           
            List<DB_Return> m_TV = new List<DB_Return>();

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    string SQL = "SELECT Value, Notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
                    string[] output = { "Value", "Notes" };
                    OleDbCommand oleDbCommand = new OleDbCommand(SQL, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    m_TV = Data.oLEDB_Interface.oleDB_SELECT_One_Table_Multiple_Property(oleDbCommand, output, tV_Maps, id);
                    break;
                case DB_Type.MSDASQL:
                    string SQL2 = "SELECT value, notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
                    string[] output2 = { "value", "notes" };
                    OdbcCommand odbcCommand = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    m_TV = Data.oDBC_Interface.DB_SELECT_One_Table_Multiple_Property(odbcCommand, output2, tV_Maps, id);
                    break;
                case DB_Type.SQLITE:
                    
                    string[] output3 = { "Value", "Notes" };
                    //SqliteCommand sqliteCommand = new SqliteCommand(SQL3, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);
                    sqliteTypes.Add(SqliteType.Text);

                    SqliteType[] m_input_Type_sqlite = { SqliteType.Text , SqliteType.Integer};
                    List<string> m_Name = new List<string>();
                    m_Name.Add("Property");
                    m_Name.Add("Object_ID");

                    if (tV_Maps.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            List<DB_Input[]> ee = new List<DB_Input[]>();
                            List<string> help_guid = new List<string>();
                            help_guid.Add(tV_Maps[i1].Map_Name);
                            List<int> help_guid2 = new List<int>();
                            help_guid2.Add(id);
                            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(help_guid2.Select(x => new DB_Input(x, null)).ToArray());

                            string SQL3 = "SELECT Value, Notes FROM t_objectproperties WHERE Property = @Property_1_0 AND Object_ID = @Object_ID_1_0";
                            SqliteCommand sqliteCommand = new SqliteCommand(SQL3, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                            Data.SQLITE_Interface.Add_Parameters_Select(sqliteCommand, ee, m_input_Type_sqlite, m_Name);


                            List<DB_Return> m_help = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(sqliteCommand, output3, sqliteTypes);

                            DB_Return n = new DB_Return();

                            n.Ret.Add(tV_Maps[i1].Name);

                            if(m_help[0].Ret.Count > 1)
                            {
                                n.Ret.Add(m_help[0].Ret[1]);
                            }
                            else
                            {
                               // n.Ret.Add(null);
                            }
                            if (m_help[1].Ret.Count > 1)
                            {
                                n.Ret.Add(m_help[1].Ret[1]);
                            }
                            else
                            {
                                //n.Ret.Add(null);
                            }

                            //m_help[0].Ret.Add(m_help[1].Ret[1]);

                            //                            n.Ret.Add(m_help[0].Ret);
                            //  n.Ret.Add(m_help[1].Ret);

                            m_TV.Add(n);

                            i1++;
                        } while (i1 < tV_Maps.Count);
                    }

                    //m_TV = Data.SQLITE_Interface.SQLITE_SELECT_One_Table_Multiple_Property(SQL3, Data, output3, tV_Maps, id);
                    break;
            }

           

            return (m_TV);
        }

        public void Insert_Tagged_Value(Database Data, List<DB_Insert> m_Insert, Repsoitory_Elements.TaggedValue tagged, int ID, string[] m_Input_Property)
        {
            
            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    Data.oLEDB_Interface.OLEDB_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, "t_objectproperties", ID, m_Input_Property);
                    break;
                case DB_Type.MSDASQL:
                    Data.oDBC_Interface.ODBC_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, "t_objectproperties", ID, m_Input_Property);
                    break;
                case DB_Type.SQLITE:
                    Data.SQLITE_Interface.SQLITE_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, "t_objectproperties", ID, m_Input_Property);
                    break;
            }
        }

        public void Update_Tagged_Value(List<DB_Insert> m_Update_new, Database Data, int ID)
        {

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    string SQL_Update_ACCDB = "UPDATE t_objectproperties SET [Value] = ?, [Notes] = ? WHERE Object_ID = ? AND Property = ?";
                    Data.oLEDB_Interface.OLEDB_Update_One_Table_Multiple_TV(SQL_Update_ACCDB, m_Update_new, Data, ID);
                    break;
                case DB_Type.MSDASQL:
                    string SQL_Update_MSDASQL = "UPDATE t_objectproperties SET Value = ?, Notes = ? WHERE Object_ID = ? AND Property = ?";
                    Data.oDBC_Interface.ODBC_Update_One_Table_Multiple_TV(SQL_Update_MSDASQL, m_Update_new, Data, ID);
                    break;
                case DB_Type.SQLITE:
                    string SQL_Update_Sqlite = "UPDATE t_objectproperties SET Value = @Value_1_0, Notes = @Notes_1_0 WHERE Object_ID = @Object_ID_1_0 AND Property = @Property_1_0";
                    Data.SQLITE_Interface.SQLITE_Update_One_Table_Multiple_TV(SQL_Update_Sqlite, m_Update_new, Data, ID);
                    break;
            }
        }

        public void Update_Tagged_Value_Reference(Database Data, string Client_GUID, string Property, string Value)
        {
            DB_Command sQL_Command = new DB_Command();

            string[] m_input_property4 = { Property };
            string[] m_input_value4 = { Value };
           
            string[] m_select_property4 = { "Client" };
            object[] m_v_1 = { Client_GUID };
            List<object[]> m_select_value = new List<object[]>();

            OleDbType[] m_select_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_select_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_select_Type_sqlite = { SqliteType.Text };

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            m_select_value.Add(m_v_1);

            string update = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                if(Property == "Client")
                {

                    update = "UPDATE t_xref SET Client = @Client_1_0 WHERE Client IN(@Client1_1_0)";
                    m_select_property4[0] = "Client1";
                }
                else
                {
                    update = sQL_Command.Get_Update_Command_SQLITE("t_xref", m_input_property4, m_input_value4, m_select_property4, m_select_value);
                }

               
            }
            else
            {
                update = sQL_Command.Get_Update_Command("t_xref", m_input_property4, m_input_value4, m_select_property4, m_select_value, Data.metamodel.dB_Type);
            }

         

          

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand Update = new OleDbCommand(update, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    Data.oLEDB_Interface.Add_Parameters_Update(Update, m_input_value4, m_input_Type_oledb, m_select_value, m_select_Type_oledb);
                    Data.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand Update2 = new OdbcCommand(update, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    Data.oDBC_Interface.Add_Parameters_Update(Update2, m_input_value4, m_input_Type_odbc, m_select_value, m_select_Type_odbc);
                    Data.oDBC_Interface.DB_UPDATE_One_Table(Update2);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand Update3 = new SqliteCommand(update, (SqliteConnection)Data.SQLITE_Interface.dbConnection);


                    List<DB_Input> m_input_value5 = new List<DB_Input>();
                    m_input_value5.Add(new DB_Input(-1, Value));

                    Data.SQLITE_Interface.Add_Parameters_Update(Update3, m_input_value5.ToArray(), m_input_property4, m_input_Type_sqlite, m_select_property4, m_select_value, m_select_Type_sqlite);

                    Data.SQLITE_Interface.DB_UPDATE_One_Table(Update3);
                    break;
            }

        }

        public void Set_Stereotype(string GUID, string Toolbox, string StereoType, Database database)
        {
            DB_Command sQL_Command = new DB_Command();

            string des = "@STEREO;Name=" + StereoType + ";FQName=" + Toolbox + "::" + StereoType + ";@ENDSTEREO;";

            string[] m_input_property4 = { "Description", };
            object[] m_input_value4 = { des };
           
            string[] m_select_property4 = { "Name", "Type", "Client" };
            object[] m_v_1 = { "Stereotypes" };
            object[] m_v_2 = { "element property" };
            object[] m_v_3 = { GUID };
            List<object[]> m_select_value = new List<object[]>();
           
            m_select_value.Add(m_v_1);
            m_select_value.Add(m_v_2);
            m_select_value.Add(m_v_3);

            string update = "";

            if(database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                update = sQL_Command.Get_Update_Command_SQLITE("t_xref", m_input_property4, m_input_value4, m_select_property4, m_select_value);
            }
            else
            {
                update = sQL_Command.Get_Update_Command("t_xref", m_input_property4, m_input_value4, m_select_property4, m_select_value, database.metamodel.dB_Type);
            }

           

            OleDbType[] m_select_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_select_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_select_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text };

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand Update = new OleDbCommand(update, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    database.oLEDB_Interface.Add_Parameters_Update(Update, m_input_value4, m_input_Type_oledb, m_select_value, m_select_Type_oledb);
                    database.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand Update2 = new OdbcCommand(update, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    database.oDBC_Interface.Add_Parameters_Update(Update2, m_input_value4, m_input_Type_odbc, m_select_value, m_select_Type_odbc);
                    database.oDBC_Interface.DB_UPDATE_One_Table(Update2);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand Update3 = new SqliteCommand(update, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<DB_Input> m_input_value5 = new List<DB_Input>();
                    DB_Input input = new DB_Input(-1, des);

                    m_input_value5.Add(input);

                    database.SQLITE_Interface.Add_Parameters_Update(Update3, m_input_value5.ToArray(), m_input_property4, m_input_Type_sqlite, m_select_property4, m_select_value, m_select_Type_sqlite);
                    database.SQLITE_Interface.DB_UPDATE_One_Table(Update3);
                    break;
            }

        }

    }
}

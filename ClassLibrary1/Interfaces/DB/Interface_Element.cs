using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;
using Microsoft.Data.Sqlite;

using Ennumerationen;
using Database_Connection;
using Repsoitory_Elements;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using EA;
using Microsoft.Office.Interop.Excel;

namespace Requirement_Plugin.Interfaces
{
    public class Interface_Element
    {
        #region Get
        public List<int> Get_ID_By_Name(Database Data, List<string> m_Type, List<string> m_Stereotype, List<string> m_Name)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret7 = new List<DB_Return>();

            List<int> m_Object_ID_Logical = new List<int>();
            string SQL_Logical2 = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL_Logical2 = "SELECT Object_ID FROM t_object WHERE Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Stereotype") + ") AND Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Type") + ") AND Name IN(" + command.Add_SQLiteParameters_Pre(m_Name.ToArray(), "Name") + ");";

            }
            else
            {
                 SQL_Logical2 = "SELECT Object_ID FROM t_object WHERE Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ") AND Object_Type IN(" + command.Add_Parameters_Pre(m_Type.ToArray()) + ") AND Name IN(" + command.Add_Parameters_Pre(m_Name.ToArray()) + ");";
            }
            List<DB_Input[]> ee1 = new List<DB_Input[]>();
            ee1.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
            ee1.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee1.Add(m_Name.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text };


            string[] m_output = { "Object_ID"};



            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT = new OleDbCommand(SQL_Logical2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT, ee1, m_input_Type_oledb);
                    m_ret7 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL_Logical2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee1, m_input_Type_odbc);
                    m_ret7 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    List<string> m_str = new List<string>();
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    m_str.Add("Stereotype");
                    m_str.Add("Type");
                    m_str.Add("Name");

                    sqliteTypes.Add(SqliteType.Integer);

                    SqliteCommand SELECT3 = new SqliteCommand(SQL_Logical2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee1, m_input_Type_sqlite, m_str);
                    m_ret7 = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT3, m_output, sqliteTypes);
                    break;
            }


            if (m_ret7[0].Ret.Count > 1)
            {
                List<object> help = m_ret7[0].Ret.GetRange(1, m_ret7[0].Ret.Count - 1).ToList();

                List<int> help2 = help.Select(x => (int)x).ToList();

                m_Object_ID_Logical = m_ret7[0].Ret.GetRange(1, m_ret7[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList();
            }
            else
            {
                m_Object_ID_Logical = null;
            }

            return (m_Object_ID_Logical);
        }

        public string Get_One_Attribut_String(string GUID, Database database, string Attribut)
        {
            if(GUID != null)
            {
                
                List<DB_Return> m_ret = new List<DB_Return>();
                List<string> help = new List<string>();
                help.Add(GUID);

                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                string[] m_output = { Attribut };
                string[] m_input_Property = { "ea_guid" };
                string table = "t_object";

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text};

                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);
                DB_Command command = new DB_Command();


                string select2 = "";

                if(database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }
               
              

                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        int i1 = 0;
                        do
                        {
                            m_str.Add(m_input_Property[i1]);

                            i1++;
                        } while (i1 < m_input_Property.Length);

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }


                if (m_ret[0].Ret.Count > 1)
                {
                    if (m_ret[0].Ret[1] != null)
                    {
                        return ((string)m_ret[0].Ret[1]);
                    }
                    else
                    {
                        return (null);
                    }

                }
                else
                {
                    return (null);
                }
            }
            else
            {
                return (null);
            }
           
        }

        public string Get_One_Attribut_String_by_ID(int ID, Database database, string Attribut)
        {
            if (ID != null)
            {
               
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<int> help_ID = new List<int>();
                help_ID.Add(ID);
                ee.Add(help_ID.Select(x => new DB_Input(x, null)).ToArray());

                string[] m_output = { Attribut };
                string table = "t_object";
                string[] m_input_Property = { "Object_ID" };
                OleDbType[] m_input_Type_oledb = { OleDbType.BigInt };
                OdbcType[] m_input_Type_odbc = { OdbcType.Int };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Integer };

                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);

                DB_Command command = new DB_Command();

                string select2 = "";

                if(database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }

                

                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        int i1 = 0;
                        do
                        {
                            m_str.Add(m_input_Property[i1]);

                            i1++;
                        } while (i1 < m_input_Property.Length);

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            else
            {
                return (null);
            }

        }

        public string Get_One_Attribut_String_by_Classifier(string Classifier, Database database, string Attribut)
        {
            if (Classifier != null)
            {
               
                List<DB_Return> m_ret = new List<DB_Return>();
                List<string> help = new List<string>();
                help.Add(Classifier);

                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                string[] m_output = { Attribut };
                string[] m_input_Property = { "PDATA1" };
                string table = "t_object";
                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text };


                //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);

                DB_Command command = new DB_Command();

                string select2 = "";

                if(database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }



                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        int i1 = 0;
                        do
                        {
                            m_str.Add(m_input_Property[i1]);

                            i1++;
                        } while (i1 < m_input_Property.Length);

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    if (m_ret[0].Ret[1] != null)
                    {
                        return ((string)m_ret[0].Ret[1]);
                    }
                    else
                    {
                        return (null);
                    }

                }
                else
                {
                    return (null);
                }
            }
            else
            {
                return (null);
            }

        }

        public int Get_One_Attribut_Integer(string GUID, Database database, string Attribut, string TABLE)
        {
            if (GUID != null)
            {
               
                List<DB_Return> m_ret = new List<DB_Return>();
                List<string> help = new List<string>();
                help.Add(GUID);

                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                string[] m_output = { Attribut };
                string[] m_input_Property = { "ea_guid" };
                string table = TABLE;

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text };
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);

                DB_Command command = new DB_Command();

                string select2 = "";

                if(database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }

                


                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        int i1 = 0;
                        do
                        {
                            m_str.Add(m_input_Property[i1]);

                            i1++;
                        } while (i1 < m_input_Property.Length);

                        sqliteTypes.Add(SqliteType.Integer);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    if (m_ret[0].Ret[1] != null)
                    {
                        return ((int)m_ret[0].Ret[1]);
                    }
                    else
                    {
                        return (-1);
                    }

                }
                else
                {
                    return (-1);
                }
            }
            else
            {
                return (-1);
            }

        }

        public string Get_Parent_Package_GUID(string GUID, Database database)
        {
            if (GUID != null && GUID != "")
            {


                int ret = 0;
                List<DB_Return> m_ret = new List<DB_Return>();

                string SQL = "";

                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_package WHERE Package_ID IN(SELECT Parent_ID FROM t_package WHERE ea_guid = @ea_guid_1_0);";
                }
                else
                {
                   SQL  = "SELECT ea_guid FROM t_package WHERE Package_ID IN(SELECT Parent_ID FROM t_package WHERE ea_guid = ?);";
                }
               



                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<string> help_guid = new List<string>();
                help_guid.Add(GUID);
                ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

                string[] m_output = { "ea_guid" };



                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(SQL, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("ea_guid");


                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    //List<string> help = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1);
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            else
            {
                return (null);
            }
        }
       
        public string Get_Parent_GUID(string GUID, Database database)
        {
            if (GUID != null && GUID != "")
            {

          
                int ret = 0;
                List<DB_Return> m_ret = new List<DB_Return>();

                string SQL = "";

                if(database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN( SELECT ParentID FROM t_object WHERE ea_guid = @ea_guid_1_0);";
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN( SELECT ParentID FROM t_object WHERE ea_guid = ?);";
                }
                

               

                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<string> help_guid = new List<string>();
                help_guid.Add(GUID);
                ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

                string[] m_output = { "ea_guid" };

               

                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(SQL, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("ea_guid");


                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    //List<string> help = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1);
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            else
            {
                return (null);
            }
        }

        public List<string> Get_Children(string GUID, Database database, List<string> m_Type_child, List<string> m_Stereotype_child)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();

           
            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(GUID);
          

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text , SqliteType.Text };

            string[] m_output = { "ea_guid" };

          

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:

                    string SQL2 = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Classifier FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Type_child.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_child.ToArray()) + ") AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?));";
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_child.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    string SQL3 = "SELECT ea_guid FROM t_object WHERE ea_guid IN (SELECT classifier_guid FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Type_child.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_child.ToArray()) + ") AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?));";
                    OdbcCommand SELECT3 = new OdbcCommand(SQL3, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    ee.Add(m_Type_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_child.Select(x => new DB_Input(-1, x)).ToArray());
                    
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                    break;
                case DB_Type.SQLITE:
                    string SQL4 = "SELECT ea_guid FROM t_object WHERE ea_guid IN (SELECT classifier_guid FROM t_object WHERE Object_Type IN( " + command.Add_SQLiteParameters_Pre(m_Type_child.ToArray(), "Object_Type") + ") AND Stereotype IN( " + command.Add_SQLiteParameters_Pre(m_Stereotype_child.ToArray(), "Stereotype") + ") AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0));";

                    List<string> m_str = new List<string>();
                    List<SqliteType> sqliteTypes = new List<SqliteType>();

                    m_str.Add("Object_Type");
                    m_str.Add("Stereotype");
                    m_str.Add("ea_guid");

                    ee.Add(m_Type_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                    sqliteTypes.Add(SqliteType.Text);

                    SqliteCommand SELECT4 = new SqliteCommand(SQL4, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                    m_ret3 = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }
        }

        public List<string> Get_Children_Element(string Parent_GUID, Database database, List<string> m_Type_child, List<string> m_Stereotype_child)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL2 = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN( " + command.Add_SQLiteParameters_Pre(m_Type_child.ToArray(), "Object_Type") + ") AND Stereotype IN( " + command.Add_SQLiteParameters_Pre(m_Stereotype_child.ToArray(), "Stereotype") + ") AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0);";

            }
            else
            {
                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Type_child.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_child.ToArray()) + ") AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?);";

            }


            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(Parent_GUID);

           
        

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };


            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_child.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT3 = new OdbcCommand(SQL2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    ee.Add(m_Type_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                    break;
                case DB_Type.SQLITE:
                    List<string> m_str = new List<string>();
                    List<SqliteType> sqliteTypes = new List<SqliteType>();

                    m_str.Add("Object_Type");
                    m_str.Add("Stereotype");
                    m_str.Add("ea_guid");

                    ee.Add(m_Type_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                    sqliteTypes.Add(SqliteType.Text);

                    SqliteCommand SELECT4 = new SqliteCommand(SQL2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                    m_ret3 = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }
        }

        public List<string> Get_Children_Package(string Parent_GUID, Database database)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();


            string SQL2 = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid FROM t_package WHERE Parent_ID IN (SELECT Package_ID FROM t_package WHERE ea_guid = @ea_guid_1_0);";
            }
            else
            {
                SQL2 = SQL2 = "SELECT ea_guid FROM t_package WHERE Parent_ID IN (SELECT Package_ID FROM t_package WHERE ea_guid = ?);";
            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(Parent_GUID);




           // OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
           // OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar};
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar};
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text};

            string[] m_output = { "ea_guid" };


            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                  //  ee.Add(m_Type_child.Select(x => new DB_Input(-1, x)).ToArray());
                  //  ee.Add(m_Stereotype_child.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT3 = new OdbcCommand(SQL2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                //   ee.Add(m_Type_child.Select(x => new DB_Input(-1, x)).ToArray());
                //    ee.Add(m_Stereotype_child.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                    break;
                case DB_Type.SQLITE:
                    List<string> m_str = new List<string>();
                    List<SqliteType> sqliteTypes = new List<SqliteType>();

                    m_str.Add("ea_guid");

                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                    sqliteTypes.Add(SqliteType.Text);

                    SqliteCommand SELECT4 = new SqliteCommand(SQL2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                    m_ret3 = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }
        }
        public List<string> Get_Parent_Taxonomy(Database database, string GUID, List<string> m_Type, List<string> m_Stereotype, List<string> m_Type_elem, List<string> m_Stereotype_elem)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();
           
            List<string> m_Supplier_GUID = new List<string>();


            string SQL = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Stereotype_Con") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_elem.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_elem.ToArray(),"Stereotype") + ") AND ea_guid = @ea_guid_1_0));";

            }
            else
            {
                SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_Parameters_Pre(m_Type.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_elem.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_elem.ToArray()) + ") AND ea_guid = ?));";

            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(GUID);

            

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };
            

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT3 = new OdbcCommand(SQL, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                    break;
                case DB_Type.SQLITE:
                    List<string> m_str = new List<string>();
                    List<SqliteType> sqliteTypes = new List<SqliteType>();

                    m_str.Add("Connector_Type");
                    m_str.Add("Stereotype_Con");
                    m_str.Add("Object_Type");
                    m_str.Add("Stereotype");
                    m_str.Add("ea_guid");
                   

                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                    sqliteTypes.Add(SqliteType.Text);

                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                    m_ret3 = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }
        }

        public List<string> Get_Children_Taxonomy(Database database, string GUID, List<string> m_Type, List<string> m_Stereotype, List<string> m_Type_elem, List<string> m_Stereotype_elem)
        {
            DB_Command command = new DB_Command();
         
            List<DB_Return> m_ret3 = new List<DB_Return>();
            List<string> m_Supplier_GUID = new List<string>();


            string SQL = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Stereotype_Con") + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_elem.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_elem.ToArray(), "Stereotype") + ") AND ea_guid = @ea_guid_1_0));";

            }
            else
            {
                SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_Parameters_Pre(m_Type.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_elem.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_elem.ToArray()) + ") AND ea_guid = ?));";

            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(GUID);

         

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };

           

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT3 = new OdbcCommand(SQL, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                    break;
                case DB_Type.SQLITE:
                    List<string> m_str = new List<string>();
                    List<SqliteType> sqliteTypes = new List<SqliteType>();

                    m_str.Add("Connector_Type");
                    m_str.Add("Stereotype_Con");
                    m_str.Add("Object_Type");
                    m_str.Add("Stereotype");
                    m_str.Add("ea_guid");


                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                    sqliteTypes.Add(SqliteType.Text);

                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                    m_ret3 = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                return (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return (null);
            }
        }

        public List<string> Get_Children_By_Classifier(Database Data, string Classifier_GUID, string Parent_GUID)
        {
            List<string> m_ea_guid_Child = new List<string>();
            List<DB_Return> m_ret4 = new List<DB_Return>();
            string SQL_Child2 = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL_Child2 = "SELECT ea_guid FROM t_object WHERE PDATA1 = @PDATA1_1_0 AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0)";
            }
            else
            {
                SQL_Child2 = "SELECT ea_guid FROM t_object WHERE PDATA1 = ? AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?)";
            }

           
            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help = new List<string>();
            List<string> help2 = new List<string>();
            help.Add(Parent_GUID);
            help2.Add(Classifier_GUID);
            

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text};

            string[] m_output = { "ea_guid" };
            

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT = new OleDbCommand(SQL_Child2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help2.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_oledb);
                    m_ret4 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT3 = new OdbcCommand(SQL_Child2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    ee.Add(help2.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                    m_ret4 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                    break;
                case DB_Type.SQLITE:
                    List<string> m_str = new List<string>();
                    List<SqliteType> sqliteTypes = new List<SqliteType>();

                 
                    m_str.Add("PDATA1");
                    m_str.Add("ea_guid");

                    ee.Add(help2.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());

                    sqliteTypes.Add(SqliteType.Text);

                    SqliteCommand SELECT4 = new SqliteCommand(SQL_Child2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                    m_ret4 = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                    break;
            }

            if (m_ret4[0].Ret.Count > 1)
            {
                m_ea_guid_Child = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_ea_guid_Child = null;
            }

            return (m_ea_guid_Child);
        }

        public string Get_Classifier_Activity(Database database, string GUID)
        {
            if (GUID != null)
            {

                List<DB_Return> m_ret = new List<DB_Return>();
                List<string> help = new List<string>();
                help.Add(GUID);

                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                string[] m_output = { "Classifier" };
                string[] m_output2 = { "ea_guid" };
                string[] m_input_Property = { "ea_guid" };
                string table = "t_object";

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text};

                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);
                DB_Command command = new DB_Command();

                string select3 = "";

                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    string select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                    select3 = "SELECT ea_guid FROM t_object WHERE Object_ID IN (" + select2 + ");";
                }
                else
                {
                    string select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);

                    select3 = "SELECT ea_guid FROM t_object WHERE Object_ID IN (" + select2 + ");";
                }

                

                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select3, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output2);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select3, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output2);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("ea_guid");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select3, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output2, sqliteTypes);
                        break;
                }


                if (m_ret[0].Ret.Count > 1)
                {
                    if (m_ret[0].Ret[1] != null)
                    {
                        return ((string)m_ret[0].Ret[1]);
                    }
                    else
                    {
                        return (null);
                    }

                }
                else
                {
                    return (null);
                }
            }
            else
            {
                return (null);
            }
        }


        public List<string> Get_Instanzen_Activity(Database database, string GUID)
        {
            if (GUID != null)
            {

                List<DB_Return> m_ret = new List<DB_Return>();
                List<string> help = new List<string>();
                help.Add(GUID);

                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                string[] m_output = { "ea_guid" };
                string[] m_input_Property = { "Classifier_guid" };
                string table = "t_object";

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text };
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);
                DB_Command command = new DB_Command();

                

                string select2 = "";

                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                     select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }
                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Classifier_guid");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }


                if (m_ret[0].Ret.Count > 1)
                {
                    if (m_ret[0].Ret[1] != null)
                    {
                        List<string> m_return = new List<string>();

                        int i1 = 1;
                        do
                        {
                            m_return.Add((string)m_ret[0].Ret[i1]);

                            i1++;
                        } while (i1 < (m_ret[0].Ret.Count));

                        return (m_return);
                    }
                    else
                    {
                        return (null);
                    }

                }
                else
                {
                    return (null);
                }
            }
            else
            {
                return (null);
            }
        }
        #endregion Get

        #region Check
        public string Check_Database_Element_Class(Database Data, string Type, string StereoType, string Name, int ParentID)
        {
       

            List<string> help_Type = new List<string>();
            help_Type.Add(Type);
            List<string> help_StereoType = new List<string>();
            help_StereoType.Add(StereoType);
            List<string> help_Name = new List<string>();
            help_Name.Add(Name);
            List<int> help_Parent = new List<int>();
            help_Parent.Add(ParentID);

            if (ParentID == -1 && StereoType != null)
            {
                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<DB_Return> m_ret = new List<DB_Return>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Stereotype", "Name" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc= { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text };
                //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);

                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }

               
               

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("Stereotype");
                        m_str.Add("Name");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID == -1 && StereoType == null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Name" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };
                //List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);

                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }


                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("Name");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID != -1 && StereoType != null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Parent.Select(x => new DB_Input(x, null)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Stereotype", "Name", "ParentID" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Integer  };
                //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);

                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }


                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("Stereotype");
                        m_str.Add("Name");
                        m_str.Add("ParentID");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID != -1 && StereoType == null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
               // ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Parent.Select(x => new DB_Input(x, -1)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Name", "ParentID" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Integer };
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);

                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }


                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("Name");
                        m_str.Add("ParentID");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }

            return (null);


        }

        public string Check_Database_Element_Instantiate(Database Data, string Type, string StereoType, int ParentID, string Classisfier_GUID)
        {
          

            List<string> help_Type = new List<string>();
            help_Type.Add(Type);
            List<string> help_StereoType = new List<string>();
            help_StereoType.Add(StereoType);
            List<string> help_Name = new List<string>();
            help_Name.Add(Classisfier_GUID);
            List<int> help_Parent = new List<int>();
            help_Parent.Add(ParentID);

            if (ParentID == -1 && StereoType != null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Stereotype", "PDATA1" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text};
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);
                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("Stereotype");
                        m_str.Add("PDATA1");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID == -1 && StereoType == null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "PDATA1" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text};
                //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);
                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("PDATA1");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID != -1 && StereoType != null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Parent.Select(x => new DB_Input(x, null)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Stereotype", "PDATA1", "ParentID" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Integer };
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);

                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("Stereotype");
                        m_str.Add("PDATA1");
                        m_str.Add("ParentID");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID != -1 && StereoType == null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                // ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Parent.Select(x => new DB_Input(x, null)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "PDATA1", "ParentID" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Integer };
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);

                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }


                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("PDATA1");
                        m_str.Add("ParentID");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    return (null);
                }
            }

            return (null);


        }

        public List<string> Check_Database_Element_Classifier(Database Data, List<string> m_Type, List<string> m_StereoType, int ParentID, string Classisfier_GUID)
        {


            List<string> help_Type = new List<string>();
            //   help_Type.Add(Type);
            help_Type = m_Type;
            List<string> help_StereoType = new List<string>();
            //help_StereoType.Add(StereoType);
            help_StereoType = m_StereoType;
            List<string> help_Name = new List<string>();
            help_Name.Add(Classisfier_GUID);
            List<int> help_Parent = new List<int>();
            help_Parent.Add(ParentID);

            if (ParentID == -1 && m_StereoType != null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Stereotype", "Classifier_guid" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text };
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);
                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("Stereotype");
                        m_str.Add("Classifier_guid");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID == -1 && m_StereoType == null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Classifier_guid" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text};
                //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);
                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }


                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Object_Type");
                        m_str.Add("Classifier_guid");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID != -1 && m_StereoType != null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<DB_Input[]> ee1 = new List<DB_Input[]>();
                ee1.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee1.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee1.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                ee1.Add(help_Parent.Select(x => new DB_Input(x, null)).ToArray());
        
                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Stereotype", "Classifier_guid", "ParentID" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text,  SqliteType.Text, SqliteType.Text, SqliteType.Integer };
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);

                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee1);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee1);
                }


                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_Parent.Select(x => new DB_Input(x, null)).ToArray());


                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_Parent.Select(x => new DB_Input(x, null)).ToArray());
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help_Parent.Select(x => new DB_Input(x, null)).ToArray());
                        m_str.Add("Object_Type");
                        m_str.Add("Stereotype");
                        m_str.Add("Classifier_guid");
                        m_str.Add("ParentID");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    return (null);
                }
            }
            if (ParentID != -1 && m_StereoType == null)
            {
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
                // ee.Add(help_StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(help_Parent.Select(x => new DB_Input(x, null)).ToArray());

                string[] m_output = { "ea_guid" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Classifier_guid", "ParentID" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Integer };
                // List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, Data);

                DB_Command command = new DB_Command();

                string select2 = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }


                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                      
                        m_str.Add("Object_Type");
                        m_str.Add("Classifier_guid");
                        m_str.Add("ParentID");

                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    return (null);
                }
            }

            return (null);


        }
        #endregion Check

        #region Create
        public string Create_Class_Check_DB(Database database, string Name, string Package_guid, string Note, EA.Repository repository, string Type, string Stereotype)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();

            /* if (database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
             {
                 database.oLEDB_Interface.dbConnection.Close();
             }*/
            interface_Collection_OleDB.Close_Connection(database);
            Repository_Class repository_Element = new Repository_Class();
            string Classifier_ID = repository_Element.Create_Element_Class(Name, Type, Stereotype, "", -1, Package_guid, repository, Note, database);
            interface_Collection_OleDB.Open_Connection(database);
           /*if (database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
            {
                database.oLEDB_Interface.dbConnection.Open();
            }*/

            return (Classifier_ID);
        }
        #endregion Create

        #region Update

        public void Update_BigInt(string GUID, int BigInt, string Property, Database database)
        {
            DB_Command sQL_Command = new DB_Command();

            string[] m_input_property = { Property };
            object[] m_input_value = { BigInt };
           
            string[] m_select_property = { "ea_guid" };
            object[] m_v_4 = { GUID };
            List<object[]> m_select_value2 = new List<object[]>();

            OleDbType[] m_select_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_select_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_select_Type_sqlite = { SqliteType.Text };

            OleDbType[] m_input_Type_oledb = { OleDbType.BigInt };
            OdbcType[] m_input_Type_odbc = { OdbcType.Int };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer };

            m_select_value2.Add(m_v_4);

            string update2 = "";

            if(database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                update2 = sQL_Command.Get_Update_Command_SQLITE("t_object", m_input_property, m_input_value, m_select_property, m_select_value2);
            }
            else
            {
                update2 = sQL_Command.Get_Update_Command("t_object", m_input_property, m_input_value, m_select_property, m_select_value2, database.metamodel.dB_Type);
            }

           

           

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand Update2 = new OleDbCommand(update2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    database.oLEDB_Interface.Add_Parameters_Update(Update2, m_input_value, m_input_Type_oledb, m_select_value2, m_select_Type_oledb);
                    database.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update2);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand Update3 = new OdbcCommand(update2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    database.oDBC_Interface.Add_Parameters_Update(Update3, m_input_value, m_input_Type_odbc, m_select_value2, m_select_Type_odbc);
                    database.oDBC_Interface.DB_UPDATE_One_Table(Update3);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand Update4 = new SqliteCommand(update2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<DB_Input> m_db = new List<DB_Input>();
                    m_db.Add(new DB_Input(BigInt, null));
                    

                    database.SQLITE_Interface.Add_Parameters_Update(Update4, m_db.ToArray(), m_input_property, m_input_Type_sqlite, m_select_property, m_select_value2, m_select_Type_sqlite);
                    database.SQLITE_Interface.DB_UPDATE_One_Table(Update4);
                    break;
            }

        }

        public void Update_VarChar(string GUID, string VarChar, string Property, Database database)
        {
            DB_Command sQL_Command = new DB_Command();

            string[] m_input_property = { Property };
            object[] m_input_value = { VarChar };
         
            string[] m_select_property = { "ea_guid" };
            object[] m_v_4 = { GUID };
            List<object[]> m_select_value2 = new List<object[]>();

            OleDbType[] m_select_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_select_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_select_Type_sqlite = { SqliteType.Text };

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            m_select_value2.Add(m_v_4);

           
            string update2 = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                if(Property == "ea_guid")
                {
                    update2 = "UPDATE t_object SET ea_guid = @ea_guid_1_0 WHERE ea_guid IN(@ea_guid1_1_0)";
                    m_select_property[0] = "ea_guid1";
                }
                else
                {
                    update2 = sQL_Command.Get_Update_Command_SQLITE("t_object", m_input_property, m_input_value, m_select_property, m_select_value2);
                }

            }
            else
            {
               update2 = sQL_Command.Get_Update_Command("t_object", m_input_property, m_input_value, m_select_property, m_select_value2, database.metamodel.dB_Type);
            }

            /*   OleDbCommand Update2 = new OleDbCommand(update2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
               database.oLEDB_Interface.Add_Parameters_Update(Update2, m_input_value, m_input_Type, m_select_value2, m_select_Type);
               database.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update2);*/

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand Update2 = new OleDbCommand(update2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    database.oLEDB_Interface.Add_Parameters_Update(Update2, m_input_value, m_input_Type_oledb, m_select_value2, m_select_Type_oledb);
                    database.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update2);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand Update3 = new OdbcCommand(update2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    database.oDBC_Interface.Add_Parameters_Update(Update3, m_input_value, m_input_Type_odbc, m_select_value2, m_select_Type_odbc);
                    database.oDBC_Interface.DB_UPDATE_One_Table(Update3);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand Update4 = new SqliteCommand(update2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<DB_Input> m_db = new List<DB_Input>();
                    m_db.Add(new DB_Input(-1, VarChar));


                    database.SQLITE_Interface.Add_Parameters_Update(Update4, m_db.ToArray(), m_input_property, m_input_Type_sqlite, m_select_property, m_select_value2, m_select_Type_sqlite);
                    database.SQLITE_Interface.DB_UPDATE_One_Table(Update4);
                    break;
            }


        }


        public void Update_Stereotype_Xref(string GUID, string element_stereo, string element_toolbox , Database database)
        {
            DB_Command sQL_Command = new DB_Command();

            string VarChar = " @STEREO; Name = "+ element_stereo + "; FQName = "+ element_toolbox + "::"+ element_stereo + "; @ENDSTEREO;";

            string[] m_input_property = { "Description" };
            object[] m_input_value = { VarChar };

            string[] m_select_property = { "Client", "Name" };
            object[] m_v_4 = { GUID};
            object[] m_v_5= { "Stereotypes" };
            List<object[]> m_select_value2 = new List<object[]>();

            OleDbType[] m_select_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_select_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_select_Type_sqlite = { SqliteType.Text, SqliteType.Text };

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar};
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            m_select_value2.Add(m_v_4);
            m_select_value2.Add(m_v_5);

            string update2 = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                update2 = sQL_Command.Get_Update_Command_SQLITE("t_xref", m_input_property, m_input_value, m_select_property, m_select_value2);
            }
            else
            {
                update2 = sQL_Command.Get_Update_Command("t_xref", m_input_property, m_input_value, m_select_property, m_select_value2, database.metamodel.dB_Type);
            }
            

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand Update2 = new OleDbCommand(update2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    database.oLEDB_Interface.Add_Parameters_Update(Update2, m_input_value, m_input_Type_oledb, m_select_value2, m_select_Type_oledb);
                    database.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update2);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand Update3 = new OdbcCommand(update2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    database.oDBC_Interface.Add_Parameters_Update(Update3, m_input_value, m_input_Type_odbc, m_select_value2, m_select_Type_odbc);
                    database.oDBC_Interface.DB_UPDATE_One_Table(Update3);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand Update4 = new SqliteCommand(update2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<DB_Input> m_db = new List<DB_Input>();
                    m_db.Add(new DB_Input(-1, VarChar));


                    database.SQLITE_Interface.Add_Parameters_Update(Update4, m_db.ToArray(), m_input_property, m_input_Type_sqlite, m_select_property, m_select_value2, m_select_Type_sqlite);
                    database.SQLITE_Interface.DB_UPDATE_One_Table(Update4);
                    break;
            }

        }
        #endregion Update
    }
}

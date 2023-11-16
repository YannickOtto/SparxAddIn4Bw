using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;
using Database_Connection;
using Ennumerationen;
using Repsoitory_Elements;
using Microsoft.Data.Sqlite;
using EA;
using Microsoft.Office.Interop.Excel;

namespace Requirement_Plugin.Interfaces
{
    public class Interface_Connectors
    {
        #region Get
        public List<string> Get_Connector_By_Type_and_Stereotype(Database Data, List<string> m_Type, List<string> m_Stereotype, List<string> m_Type_Client, List<string> m_Stereotype_Client, List<string> m_Type_Supplier, List<string> m_Stereotype_Supplier)
        {
            List<string> m_ea_guid = new List<string>();
            DB_Command command = new DB_Command();
            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar , OleDbType.VarChar , OleDbType.VarChar , OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar , OdbcType.VarChar , OdbcType.VarChar , OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };

            string SQL = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Connector_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Stereotype") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type1") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_Client.ToArray(), "Stereotype1") + ")) AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type2") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype2") + "));";

            }
            else
            {
                SQL = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN (" + command.Add_Parameters_Pre(m_Type.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN (" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_Client.ToArray()) + ")) AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN (" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + "));";

            }

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Data.oLEDB_Interface.dbConnection);


                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());

                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                 
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());

                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());

                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());

                    List<string> m_Name = new List<string>();
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype");
                    m_Name.Add("Object_Type1");
                    m_Name.Add("Stereotype1");
                    m_Name.Add("Object_Type2");
                    m_Name.Add("Stereotype2");

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                m_ea_guid = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_ea_guid = null;
            }

            return (m_ea_guid);
        }


        public List<string> Get_Connetor_By_Elements(Database Data, List<string> m_Type_Client, List<string> m_Stereotype_Client, List<string> m_Type_Supplier, List<string> m_Stereotype_Supplier, List<string> m_Type_Con, List<string> m_Stereotype_Con)
        {
            List<string> m_ea_guid = new List<string>();

            DB_Command command = new DB_Command();

            List<DB_Input[]> ee = new List<DB_Input[]>();
           

            string[] m_output = { "ea_guid" };

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };


            List<DB_Return> m_ret3 = new List<DB_Return>();
            string SQL2 = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "Stereotype") + ") AND End_Object_ID IN(SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type1") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype1") + ")) AND Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type2") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Client.ToArray(), "Stereotype2") + ")); ";

            }
            else
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Con.ToArray()) + ") AND End_Object_ID IN(SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + ")) AND Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Client.ToArray()) + ")); ";

            }

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());

                    List<string> m_Name = new List<string>();
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype");
                    m_Name.Add("Object_Type1");
                    m_Name.Add("Stereotype1");
                    m_Name.Add("Object_Type2");
                    m_Name.Add("Stereotype2");

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                m_ea_guid = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_ea_guid = null;
            }


        
            return (m_ea_guid);
        }

        public List<string> Get_Connector_By_m_Client_GUID(Database Data, List<string> m_Client_GUID, List<string> m_Type_Con, List<string> m_Stereotype_Con)
        {
            
            if(m_Client_GUID.Count > 0)
            {
                DB_Command command = new DB_Command();
                List<string> ret = new List<string>();
                List<DB_Return> m_ret3 = new List<DB_Return>();

                string SQL = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid IN (" + command.Add_SQLiteParameters_Pre(m_Client_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "Stereotype") + ")";
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid IN (" + command.Add_Parameters_Pre(m_Client_GUID.ToArray()) + ")) AND Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_Con.ToArray()) + ")";
                }

                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(m_Client_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text };

                string[] m_output = { "ea_guid" };

                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Data.oLEDB_Interface.dbConnection);

                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                        m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                        m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();

                        m_Name.Add("ea_guid");
                        m_Name.Add("Connector_Type");
                        m_Name.Add("Stereotype");
                       
                     
                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                        m_ret3 = Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                        break;
                }


                List<string> m_GUID_ret = new List<string>();

                if (m_ret3[0].Ret.Count > 1)
                {
                    m_GUID_ret = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
                }
                else
                {
                    m_GUID_ret = null;
                }


                return m_GUID_ret;
            }
            else
            {
                return (null);
            }
        }

        public List<string> Get_Connector_From_Client_Property(Database database, string Property, List<string> m_Type_Client, List<string> m_Stereotype_Client, string GUID_Client, List<string> m_Type_Supplier, List<string> m_Stereotype_Supplier, List<string> m_Type_Connector, List<string> m_Stereotype_Connector)
        {
            DB_Command command = new DB_Command();
           // XML xML = new XML();
            List<string> m_Connector_GUID = new List<string>();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL2 = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_SQLiteParameters_Pre(m_Type_Connector.ToArray(), "Connector_Type") + ")  AND Stereotype IN( " + command.Add_SQLiteParameters_Pre(m_Stereotype_Connector.ToArray(), "Stereotype") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type1") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Client.ToArray(), "Stereotype1") + ") AND " + Property + " = @" + Property + "1_1_0) AND END_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type2") + ") AND Stereotype IN( " + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype2") + "));";

            }
            else
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Connector.ToArray()) + ")  AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Connector.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Client.ToArray()) + ") AND " + Property + " = ?) AND END_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + "));";
            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(GUID_Client);

          

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    ee.Add(m_Type_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                  
                    database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    ee.Add(m_Type_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                   

                    List<string> m_Name = new List<string>();
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype");
                    m_Name.Add("Object_Type1");
                    m_Name.Add("Stereotype1");
                    m_Name.Add(Property + "1");
                    m_Name.Add("Object_Type2");
                    m_Name.Add("Stereotype2");

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
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

            return (m_Connector_GUID);
        }

        public List<string> Get_Connector_From_Supplier_Property(Database database, string Property, List<string> m_Type_Supplier, List<string> m_Stereotype_Supplier, string Property_Supplier, List<string> m_Type_Client, List<string> m_Stereotype_Client, List<string> m_Type_Connector, List<string> m_Stereotype_Connector)
        {
            DB_Command command = new DB_Command();
         //   XML xML = new XML();
            List<string> m_Connector_GUID = new List<string>();
            List<DB_Return> m_ret3 = new List<DB_Return>();

         
            string SQL2 = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_SQLiteParameters_Pre(m_Type_Connector.ToArray(), "Connector_Type") + ")  AND Stereotype IN( " + command.Add_SQLiteParameters_Pre(m_Stereotype_Connector.ToArray(), "Stereotype") + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type1") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype1") + ") AND " + Property + " = @"+Property+"1_1_0) AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type2") + ") AND Stereotype IN( " + command.Add_SQLiteParameters_Pre(m_Stereotype_Client.ToArray(), "Stereotype2") + "));";
            }
            else
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Connector.ToArray()) + ")  AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Connector.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + ") AND " + Property + " = ?) AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN( " + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Client.ToArray()) + "));";
            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(Property_Supplier);

          

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    ee.Add(m_Type_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    ee.Add(m_Type_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Connector.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());

                    List<string> m_Name = new List<string>();

                    
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype");
                    m_Name.Add("Object_Type1");
                    m_Name.Add("Stereotype1");
                    m_Name.Add(Property+"1");
                    m_Name.Add("Object_Type2");
                    m_Name.Add("Stereotype2");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
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

            return (m_Connector_GUID);
        }

        public List<string> Get_Connector_By_PropertyType(Database Database, string Client_Classifier_GUID, string Supplier_Classifier_GUID, List<string> m_Type_Con, List<string> m_Stereotype_Con)
        {
           DB_Command  command = new DB_Command();

            List<string> Targets_GUID = new List<string>();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL2 = "";

            if (Database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID IN( SELECT Object_ID FROM t_object WHERE PDATA1 = @PDATA1_1_1_0 ) AND End_Object_ID IN( SELECT Object_ID FROM t_object WHERE PDATA1 = @PDATA1_2_1_0 ) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "Stereotype") + ")";
            }
            else
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE t_connector.[Start_Object_ID] IN( SELECT t_object.Object_ID FROM t_object WHERE t_object.PDATA1 = ? ) AND t_connector.[End_Object_ID] IN( SELECT t_object.Object_ID FROM t_object WHERE t_object.PDATA1 = ? ) AND t_connector.Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Con.ToArray()) + ")";
            }

            
            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            List<string> help_guid2 = new List<string>();
            help_guid.Add(Client_Classifier_GUID);
            help_guid2.Add(Supplier_Classifier_GUID);
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_guid2.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text , SqliteType.Text };

            string[] m_output = { "ea_guid" };

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL2, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("PDATA1_1");
                    m_Name.Add("PDATA1_2");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }

           

            if (m_ret3[0].Ret.Count > 1)
            {
                Targets_GUID = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                Targets_GUID = null;
            }

            return (Targets_GUID);
        }

        public List<DB_Return> Get_m_Client_From_Supplier(Database Data, int ID_Client, List<string> m_Type_con, List<string> m_Stereotype_con)
        {
            DB_Command command = new DB_Command();

            string SQL2 = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid, Object_Type, Stereotype FROM t_object WHERE Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID = @End_Object_ID_1_0 AND Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype") + ")); ";
            }
            else
            {
                SQL2 = "SELECT ea_guid, Object_Type, Stereotype FROM t_object WHERE Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID = ? AND Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ")); ";
            }

           

            List<DB_Input[]> ee = new List<DB_Input[]>();

            List<int> help_guid = new List<int>();
           
            help_guid.Add(ID_Client);

            ee.Add(help_guid.Select(x => new DB_Input(x, null)).ToArray());
            ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer, SqliteType.Text, SqliteType.Text };


            string[] m_output = { "ea_guid", "Object_Type", "Stereotype" };

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    return (Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output));
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    return (Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output));
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("End_Object_ID");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);
                    sqliteTypes.Add(SqliteType.Text);
                    sqliteTypes.Add(SqliteType.Text);

                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    return (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                    break;
            }

            return (null);
        }

        public List<DB_Return> Get_m_Supplier_From_Client(Database Data, int ID_Supplier, List<string> m_Type_con, List<string> m_Stereotype_con, bool direction)
        {
            DB_Command command = new DB_Command();


            string SQL2 = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                if(direction == true)
                {
                    SQL2 = "SELECT ea_guid, Object_Type, Stereotype FROM t_object WHERE Object_ID IN(SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID = @Start_Object_ID_1_0 AND Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype") + ")); ";

                }
                else
                {
                    SQL2 = "SELECT ea_guid, Object_Type, Stereotype FROM t_object WHERE Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID = @Start_Object_ID_1_0 AND Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype") + ")); ";

                }

            }
            else
            {
                if (direction == true)
                {
                    SQL2 = "SELECT ea_guid, Object_Type, Stereotype FROM t_object WHERE Object_ID IN(SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID = ? AND Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ")); ";

                }
                else
                {
                    SQL2 = "SELECT ea_guid, Object_Type, Stereotype FROM t_object WHERE Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID = ? AND Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ")); ";

                }
            }

            

            List<DB_Input[]> ee = new List<DB_Input[]>();

            List<int> help_guid = new List<int>();

            help_guid.Add(ID_Supplier);

            ee.Add(help_guid.Select(x => new DB_Input(x, null)).ToArray());
            ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer, SqliteType.Text, SqliteType.Text };


            string[] m_output = { "ea_guid", "Object_Type", "Stereotype" };

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    return (Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output));
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    return (Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output));
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Start_Object_ID");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);
                    sqliteTypes.Add(SqliteType.Text);
                    sqliteTypes.Add(SqliteType.Text);

                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    return (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                    break;
            }

            return (null);
        }

        public List<string> GetInformationElements(Database Database, string Connector_GUID)
        {
            List<string> Info_Elem_GUID = new List<string>();
            

            //Hier muss noch für den ControlFlow eine Unterscheidung getroffen werden!!! Dieser verweist auf einen InformationFlow, welcher die Datenelemente in der Description besitztz.
            //Abfrage ob vom Type control Flow
            string type = this.Get_Connector_Type(Connector_GUID, Database);
            if(type == "ControlFlow" || type == "Connector")
            {
               List<string> m_GUID =  Get_Abstract_Connector(Connector_GUID, Database);

                if(m_GUID != null)
                {
                    int i1 = 0;
                    do
                    {
                        List<string> m_GUID_abstraction = GetInformationElements(Database, m_GUID[i1]);

                        if(m_GUID_abstraction != null)
                        {
                            Info_Elem_GUID.AddRange(m_GUID_abstraction);
                        }

                        i1++;
                    } while (i1 < m_GUID.Count);
                }

                if(Info_Elem_GUID.Count == 0)
                {
                    Info_Elem_GUID = null;
                }

                return (Info_Elem_GUID);
            }

          
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL_InfoElem = "";

            if (Database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL_InfoElem = " SELECT Description FROM t_xref WHERE Client = @Client_1_0 AND Behavior = 'conveyed';";
            }
            else
            {
                SQL_InfoElem = " SELECT Description FROM t_xref WHERE t_xref.Client = ? AND t_xref.Behavior = 'conveyed';";
            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(Connector_GUID);
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            string[] m_output = { "Description" };

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL_InfoElem, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL_InfoElem, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL_InfoElem, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Client");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                Info_Elem_GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                Info_Elem_GUID = (null);
            }

            return (Info_Elem_GUID);
        }

        public List<string> Get_Connector_By_Supplier_GUID(Database Database, string Connector_GUID, string Supplier_GUID, List<string> m_Type, List<string> m_Stereotype)
        {

            DB_Command command = new DB_Command();
         
            List<string> m_ea_guid = new List<string>();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL_Check = "";

            if(Database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL_Check = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0) AND End_Object_ID = @End_Object_ID_1_0 AND Connector_Type IN("+command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Connector_Type")+") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Stereotype") + ")";

            }
            else
            {
                SQL_Check = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE ea_guid = ?) AND End_Object_ID = ? AND Connector_Type IN(" + m_Type.ToArray() + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ")";
            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_id = new List<string>();
            List<int> help_id2 = new List<int>();
            help_id.Add(Connector_GUID);
            Repository_Element repository_Element = new Repository_Element();
            repository_Element.Classifier_ID = Supplier_GUID;
            help_id2.Add(repository_Element.Get_Object_ID(Database));
            ee.Add(help_id.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_id2.Select(x => new DB_Input(x, null)).ToArray());
            ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Integer, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };


            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL_Check, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL_Check, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL_Check, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("ea_guid");
                    m_Name.Add("End_Object_ID");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                    break;
            }

           

            if (m_ret3[0].Ret.Count > 1)
            {
                m_ea_guid = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                m_ea_guid = (null);
            }

            return (m_ea_guid);
        }

        public List<string> Get_Connector_By_Client_ID(Database Data, int Client_ID)
        {
            List<string> ret = new List<string>();
            List<DB_Return> m_ret4 = new List<DB_Return>();

            string SQL = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = @Start_Object_ID_1_0";
            }
            else
            {
                SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = ?";
            }

             
           
            List<DB_Input[]> ee2 = new List<DB_Input[]>();
            List<int> help = new List<int>();
            help.Add(Client_ID);
            ee2.Add(help.Select(x => new DB_Input(x, null)).ToArray());

            OleDbType[] m_input_Type2_oledb = { OleDbType.BigInt };
            OdbcType[] m_input_Type2_odbc = { OdbcType.Int };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer };

            string[] m_output2 = { "ea_guid" };

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT2 = new OleDbCommand(SQL, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee2, m_input_Type2_oledb);
                    m_ret4 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output2);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT3 = new OdbcCommand(SQL, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    Data.oDBC_Interface.Add_Parameters_Select(SELECT3, ee2, m_input_Type2_odbc);
                    m_ret4 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output2);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Start_Object_ID");
                    
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee2, m_input_Type_sqlite, m_Name);
                    m_ret4 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output2, sqliteTypes));
                    break;
            }


            if (m_ret4[0].Ret.Count > 1)
            {
                ret = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                ret = null;
            }

            return (ret);
        }

        public List<string> Get_Supplier_Element_By_Connector(Database Data, List<string> m_Client_GUID, List<string> m_Type_Supplier, List<string> m_Stereotype_Supplier, List<string> m_Type_con, List<string> m_Stereotype_con)
        {
     //       XML xml = new XML();
            DB_Command command = new DB_Command();
            List<string> GUIDS = new List<string>();
            List<DB_Return> m_ret3 = new List<DB_Return>();



            string SQL2 = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Client_GUID.ToArray(),"ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "));";
            }
            else
            {
                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Client_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "));";

            }

            List<DB_Input[]> ee = new List<DB_Input[]>();

            

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };
           
            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    ee.Add(m_Client_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Client_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Object_Type");
                    m_Name.Add("Stereotype");
                    m_Name.Add("ea_guid");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype2");

                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Client_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                GUIDS = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                GUIDS = (null);
            }

            return (GUIDS);
        }

        public List<string> Get_Client_Element_By_Connector(Database Data, List<string> m_Supplier_GUID, List<string> m_Type_Client, List<string> m_Stereotype_Client, List<string> m_Type_con, List<string> m_Stereotype_con)
        {
            List<string> GUIDS = new List<string>();

            if (m_Supplier_GUID.Count > 0)
            {
                DB_Command command = new DB_Command();
              
                List<DB_Return> m_ret3 = new List<DB_Return>();
                string SQL2 = "";


                var t = m_Stereotype_Client.Where(x => x != "" && x != null).ToList();
                var tt = m_Stereotype_con.Where(x => x != "" && x != null).ToList();



                if (t.Count > 0)
                {
                    m_Stereotype_Client = t;
                    if (tt.Count > 0)
                    {
                        if(Data.metamodel.dB_Type == DB_Type.SQLITE)
                        {
                            SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(t.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "));";

                        }
                        else
                        {
                            SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(t.ToArray()) + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "));";

                        }

                    }
                    else
                    {
                        if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                        {
                            SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(t.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + "));";

                        }
                        else
                        {
                            SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(t.ToArray()) + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + "));";

                        }

                    }


                }
                else
                {
                    if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "));";

                    }
                    else
                    {
                        SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "));";

                    }


                }
                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<int> help_guid = new List<int>();



                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };

                string[] m_output = { "ea_guid" };



                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        ee.Add(m_Supplier_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                        if (tt.Count > 0)
                        {
                            ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                        }
                        ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                        m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                        if (m_Stereotype_Client.Select(x => x != "").ToList().Count > 0)
                        {
                            ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                        }
                        ee.Add(m_Supplier_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                        if (tt.Count > 0)
                        {
                            ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                        }

                        Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                        m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT4 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();

                        List<SqliteType> m_input_Type_sqlite = new List<SqliteType>(); ;// { SqliteType.Text };
                        ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());

                        if (t.Count>0)
                        {
                            if (tt.Count > 0)
                            {
                                m_Name.Add("Object_Type");
                                m_Name.Add("Stereotype");
                                m_Name.Add("ea_guid");
                                m_Name.Add("Connector_Type");
                                m_Name.Add("Stereotype2");

                                ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                                ee.Add(m_Supplier_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                                ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                                ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());

                                m_input_Type_sqlite.Add(SqliteType.Text);
                                m_input_Type_sqlite.Add(SqliteType.Text);
                                m_input_Type_sqlite.Add(SqliteType.Text);
                                m_input_Type_sqlite.Add(SqliteType.Text);
                                m_input_Type_sqlite.Add(SqliteType.Text);

                            }
                            else
                            {
                                m_Name.Add("Object_Type");
                                m_Name.Add("Stereotype");
                                m_Name.Add("ea_guid");
                                m_Name.Add("Connector_Type");

                                m_input_Type_sqlite.Add(SqliteType.Text);
                                m_input_Type_sqlite.Add(SqliteType.Text);
                                m_input_Type_sqlite.Add(SqliteType.Text);
                                m_input_Type_sqlite.Add(SqliteType.Text);


                                ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                                ee.Add(m_Supplier_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                                ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                            }
                        }
                        else
                        {
                            m_Name.Add("Object_Type");
                            m_Name.Add("ea_guid");
                            m_Name.Add("Connector_Type");
                            m_Name.Add("Stereotype2");

                            m_input_Type_sqlite.Add(SqliteType.Text);
                            m_input_Type_sqlite.Add(SqliteType.Text);
                            m_input_Type_sqlite.Add(SqliteType.Text);
                            m_input_Type_sqlite.Add(SqliteType.Text);


                            ee.Add(m_Supplier_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                        }

                        SqliteType[] m_input_Type_sqlite2 = m_input_Type_sqlite.ToArray();

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);


                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite2, m_Name);
                        m_ret3 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                        break;
                }

                if (m_ret3[0].Ret.Count > 1)
                {
                    GUIDS = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    GUIDS = (null);
                }
            }
            else
            {
                GUIDS = null;
            }
         //   XML xml = new XML();
           

            return (GUIDS);
        }


      
        public List<string> Get_Supplier_By_Connecror_And_Supplier(Database database, string Connector_GUID)
        {
            List<string> m_Type = database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            m_Type.AddRange(database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList());

            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0) AND Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Object_Type") + ");";

            }
            else
            {
                SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = ?) AND Object_Type IN(" + command.Add_Parameters_Pre(m_Type.ToArray()) + ");";
            }

         
            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_id = new List<string>();
            help_id.Add(Connector_GUID);
            ee.Add(help_id.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
          
            OleDbType[] m_input_Type_oledb = {OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };
        

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("ea_guid");
                    m_Name.Add("Object_Type");

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                return(m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                return(null);
            }


        
        }

        public string Get_Supplier_GUID(Database Database, string Connector_GUID, List<string> m_Type_Supplier, List<string>  m_Stereotype_Supplier)
        {
            DB_Command command = new DB_Command();
            List<string> m_Supplier_GUID = new List<string>();
            List<DB_Return> m_ret3 = new List<DB_Return>();
            string SQL = "";

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_id = new List<string>();
            help_id.Add(Connector_GUID);
            ee.Add(help_id.Select(x => new DB_Input(-1, x)).ToArray());


            List<SqliteType> sqliteType2 = new List<SqliteType>();

            if (m_Type_Supplier == null && m_Stereotype_Supplier == null)
            {
                if(Database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0);";
                    sqliteType2.Add(SqliteType.Text);
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = ?);";

                }

            }
            else
            {
                if (Database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0) AND Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype") + ");";
                    sqliteType2.Add(SqliteType.Text);
                    sqliteType2.Add(SqliteType.Text);
                    sqliteType2.Add(SqliteType.Text);
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = ?) AND Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + ");";

                }
                ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
            }
          
            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar , OleDbType.VarChar , OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = sqliteType2.ToArray();
            string[] m_output = { "ea_guid" };
          

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    if (m_Type_Supplier == null && m_Stereotype_Supplier == null)
                    {
                        m_Name.Add("ea_guid");
                    }
                    else
                    {
                        m_Name.Add("ea_guid");
                        m_Name.Add("Object_Type");
                        m_Name.Add("Stereotype");
                    }



                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                m_Supplier_GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                m_Supplier_GUID = (null);
            }


            if (m_Supplier_GUID != null)
            {
                return (m_Supplier_GUID[0]);
            }
            else
            {
                return (null);
            }
        }

        public string Get_Supplier_Classifier(Database Database, string t_object_row, string Connector_GUID)
        {
            //XML xML = new XML();
            List<string> m_Supplier_GUID = new List<string>();
            string[] m_output = new string[1];
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL = "";

            if (t_object_row != "Classifier")
            {   
                if(Database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT " + t_object_row + " FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0);";
                }
                else
                {
                    SQL = "SELECT " + t_object_row + " FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = ?);";
                }

             
                m_output[0] = t_object_row;
            }
            else
            {
                if (Database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN(SELECT " + t_object_row + " FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0));";
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN(SELECT " + t_object_row + " FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE ea_guid = ?));";
                }

                
                m_output[0] = "ea_guid";
            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_id = new List<string>();
            help_id.Add(Connector_GUID);
            ee.Add(help_id.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("ea_guid");
                    
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                var m_help = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList();

                m_Supplier_GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());



            }
            else
            {
                m_Supplier_GUID = (null);
            }

            if (m_Supplier_GUID != null)
            {
                if (m_Supplier_GUID[0].Length > 5)
                {
                    return (m_Supplier_GUID[0]);
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

        public List<string> Get_m_Supplier_By_ClientGUID_And_Connector(Database database, string Client_GUID, List<string> m_Type_Con, List<string> m_Stereotype_Con )
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret5 = new List<DB_Return>();
            string SQL = "";

            List<SqliteType> sqliteTypes1 = new List<SqliteType>();

            if (m_Stereotype_Con != null)
            {
                if(database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "Stereotype") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0));";

                    sqliteTypes1.Add(SqliteType.Text);
                    sqliteTypes1.Add(SqliteType.Text);
                    sqliteTypes1.Add(SqliteType.Text);
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Con.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?));";

                }
            }
            else
            {
                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0));";
                    sqliteTypes1.Add(SqliteType.Text);
                    sqliteTypes1.Add(SqliteType.Text);
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?));";
                }
               }

            //  string xml_Dat = repository.SQLQuery(SQL);
            //  List<string> m_GUID = xML.Xml_Read_Attribut("ea_guid", xml_Dat);
            List<string> m_GUID = new List<string>();
           

            List<DB_Input[]> ee3 = new List<DB_Input[]>();
            List<string> help_guid3 = new List<string>();
            help_guid3.Add(Client_GUID);
            string[] m_output3 = { "ea_guid" };
            /*   else
               {
                   OleDbType[] m_input_Type3 = { OleDbType.VarChar, OleDbType.VarChar };

               }*/

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = sqliteTypes1.ToArray();


            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT3 = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee3.Add(help_guid3.Select(x => new DB_Input(-1, x)).ToArray());
                    ee3.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    if (m_Stereotype_Con != null)
                    {
                        ee3.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    }
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT3, ee3, m_input_Type_oledb);
                    m_ret5 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT3, m_output3);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    ee3.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    if (m_Stereotype_Con != null)
                    {
                        ee3.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    }
                    ee3.Add(help_guid3.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee3, m_input_Type_odbc);
                    m_ret5 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output3);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Connector_Type");
                    ee3.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    if (m_Stereotype_Con != null)
                    {
                        ee3.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                        m_Name.Add("Stereotype");
                    }
                    ee3.Add(help_guid3.Select(x => new DB_Input(-1, x)).ToArray());
                    m_Name.Add("ea_guid");
                  

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);
                    
                   

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee3, m_input_Type_sqlite, m_Name);
                    m_ret5 = (database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output3, sqliteTypes));
                    break;
            }

            if (m_ret5[0].Ret.Count > 1)
            {
                m_GUID = (m_ret5[0].Ret.GetRange(1, m_ret5[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                m_GUID = (null);
            }

            return (m_GUID);
        }

        public string Get_Client_GUID(Database Database, string Connector_GUID, List<string> mType_Client, List<string> m_Stereotype_Client)
        {
            OleDbType[] m_input_Type_oledb = new OleDbType[1];
            OdbcType[] m_input_Type_odbc = new OdbcType[1];
            List<SqliteType> sqliteTypes1 = new List<SqliteType>();
            List<string> m_Client_GUID = new List<string>();
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL = "";
            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_id = new List<string>();
            help_id.Add(Connector_GUID);
            ee.Add(help_id.Select(x => new DB_Input(-1, x)).ToArray());
            if (mType_Client == null || m_Stereotype_Client == null)
            {
                if(Database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0);";
                    sqliteTypes1.Add(SqliteType.Text);
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE ea_guid = ?);";

                    m_input_Type_oledb[0] = OleDbType.VarChar;
                    m_input_Type_odbc[0] = OdbcType.VarChar;
                }

                
            }
            else
            {
                if (Database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0) AND Object_Type IN(" + command.Add_SQLiteParameters_Pre(mType_Client.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Client.ToArray(), "Stereotype") + ");";
                    ee.Add(mType_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    sqliteTypes1.Add(SqliteType.Text);
                    sqliteTypes1.Add(SqliteType.Text);
                    sqliteTypes1.Add(SqliteType.Text);
                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE ea_guid = ?) AND Object_Type IN(" + command.Add_Parameters_Pre(mType_Client.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Client.ToArray()) + ");";
                    ee.Add(mType_Client.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());

                    m_input_Type_oledb = new OleDbType[3];
                    m_input_Type_odbc = new OdbcType[3];
                    m_input_Type_oledb[0] = OleDbType.VarChar;
                    m_input_Type_oledb[1] = OleDbType.VarChar;
                    m_input_Type_oledb[2] = OleDbType.VarChar;

                    m_input_Type_odbc[0] = OdbcType.VarChar;
                    m_input_Type_odbc[1] = OdbcType.VarChar;
                    m_input_Type_odbc[2] = OdbcType.VarChar;
                }
                  
             //   m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            }

            SqliteType[] m_input_Type_sqlite = sqliteTypes1.ToArray();
            string[] m_output = { "ea_guid" };

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    if (mType_Client == null || m_Stereotype_Client == null)
                    {
                        m_Name.Add("ea_guid");

                    }
                    else
                    {
                        m_Name.Add("ea_guid");
                        m_Name.Add("Object_Type");
                        m_Name.Add("Stereotype");
                    }
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                m_Client_GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                m_Client_GUID = (null);
            }


            if (m_Client_GUID != null)
            {
                return (m_Client_GUID[0]);
            }
            else
            {
                return (null);
            }
        }

       
        public List<int> Get_Supplier_ID_By_Client_ID(Database database, int Client_ID, List<string> m_Stereotype)
        {
            if (Client_ID != null && m_Stereotype != null)
            {
             //   XML xml = new XML();
                DB_Command command = new DB_Command();
                List<DB_Return> m_ret = new List<DB_Return>();
                List<int> help = new List<int>();
                help.Add(Client_ID);

                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help.Select(x => new DB_Input(x, null)).ToArray());
                ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                string[] m_output = { "End_Object_ID" };
                string table = "t_connector";
                string[] m_input_Property = { "Start_Object_ID", "Stereotype" };

                OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.Int, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Integer, SqliteType.Text };

                string select2 = "";

                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);

                }

                

                //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);

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
                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();

                        m_Name.Add("Start_Object_ID");
                        m_Name.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Integer);


                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                        m_ret = (database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                        break;
                }

                if (m_ret[0].Ret.Count > 1)
                {
                    List<int> help2 = m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList();

                    return(help2.Distinct().ToList());


                }

                return (null);

            }
            else
            {
                return (null);
            }
        }

        public List<string> Get_Connector_By_Client_GUID(Database Data, string Client_GUID, List<string> m_Type_Supplier, List<string> m_StereoType_Supplier, List<string> m_Type_Con, List<string> m_Stereotype_Con)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            List<string> m_Connector_GUID = new List<string>();

            string SQL2 = "";

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL2 = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = @Start_Object_ID_1_0 AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "Stereotype") + ") AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_StereoType_Supplier.ToArray(), "Stereotype2") + ") AND Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type") + "));";

            }
            else
            {
               SQL2 = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = ? AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Con.ToArray()) + ") AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Stereotype IN(" + command.Add_Parameters_Pre(m_StereoType_Supplier.ToArray()) + ") AND Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + "));";

            }




            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<int> help_guid = new List<int>();
            Repository_Element rep = new Repository_Element();
            rep.Classifier_ID = Client_GUID;
            help_guid.Add(rep.Get_Object_ID(Data));

          

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer, SqliteType.Text, SqliteType.Text , SqliteType.Text , SqliteType.Text };

            string[] m_output = { "ea_guid" };
           

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    ee.Add(m_StereoType_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    ee.Add(help_guid.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_StereoType_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Start_Object_ID");
                    m_Name.Add("Stereotype");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype2");
                    m_Name.Add("Object_Type");
                    ee.Add(help_guid.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_StereoType_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;

            }

            if (m_ret3[0].Ret.Count > 1)
            {
                m_Connector_GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                m_Connector_GUID = (null);
            }

            return (m_Connector_GUID);
        }

        public List<string> Get_Connector_By_Client_GUID_And_Supplier_Type(Database database, string Client_GUID, List<string> m_Type_Supplier, List<string> m_Stereotype_Supplier, List<string> m_Type_Con, List<string> m_Stereotype_Con)
        {
           
            DB_Command command = new DB_Command();
            List<string> GUID = new List<string>();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL = "";

            if(database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT ea_guid FROM t_connector WHERE Stereotype IN( " + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "Stereotype") + ") AND Connector_Type IN( " + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0) AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN  (" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type") + ") AND Stereotype IN ( " + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype2") + "));";

            }
            else
            {
                SQL = "SELECT ea_guid FROM t_connector WHERE Stereotype IN( " + command.Add_Parameters_Pre(m_Stereotype_Con.ToArray()) + ") AND Connector_Type IN( " + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?) AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN  (" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN ( " + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + "));";

            }
            

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(Client_GUID);

        

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

            string[] m_output = { "ea_guid" };
           

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Stereotype");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("ea_guid");
                    m_Name.Add("Object_Type");
                    m_Name.Add("Stereotype2");
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                GUID = (null);
            }

            return (GUID);
        }
     
        public string Get_Connector_Type(string Connector_GUID, Database Database)
        {
            string type = "";
            List<DB_Return> m_ret3 = new List<DB_Return>();


            string SQL_InfoElem = "";

            if (Database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL_InfoElem = "SELECT Connector_Type FROM t_connector WHERE ea_guid = @ea_guid_1_0;";
            }
            else
            {
                SQL_InfoElem = "SELECT Connector_Type FROM t_connector WHERE ea_guid = ?;";
            }


            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(Connector_GUID);
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar};
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar};
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            string[] m_output = { "Connector_Type" };

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL_InfoElem, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL_InfoElem, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL_InfoElem, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("ea_guid");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                type = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList())[0];
            }
            else
            {
                type = (null);
            }

            return (type);
        }

        public List<string> Get_Abstract_Connector(string Connector_GUID, Database Database)
        {
            List<string> m_Info_Elem_GUID = new List<string>();
            string help = "";
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL_InfoElem = "";

            if (Database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL_InfoElem = " SELECT Description FROM t_xref WHERE Client = @Client_1_0 AND Behavior = 'abstraction';";
            }
            else
            {
                SQL_InfoElem = " SELECT Description FROM t_xref WHERE t_xref.Client = ? AND t_xref.Behavior = 'abstraction';";
            }

            


            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            help_guid.Add(Connector_GUID);
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            string[] m_output = { "Description" };

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL_InfoElem, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL_InfoElem, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL_InfoElem, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Client");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3= (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                help = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList())[0];
            }
            else
            {
                help = (null);
            }

            if (help != null)
            {
                var GUID_splitted = help.Split(',');

                if (GUID_splitted.Length > 0)
                {
                    int i2 = 0;
                    do
                    {

                        m_Info_Elem_GUID.Add(GUID_splitted[i2]);

                         i2++;
                    } while (i2 < GUID_splitted.Length);
                 }
            }

            return (m_Info_Elem_GUID);
        }
        #endregion Get

        #region Check
        public List<string> Check_Connector(string Client_GUID, string Supplier_GUID, List<string> Type, List<string> StereoType, Database Database, bool direction)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();
            bool flag = false;
       //     XML xml = new XML();
            //  EA.Element Element = Repository.GetElementByGuid(Element_GUID);
            //  EA.Element Requirement = Repository.GetElementByGuid(Classifier_ID);
            Repository_Element Requirement = new Repository_Element();
            Requirement.Classifier_ID = Client_GUID;
            Repository_Element Element = new Repository_Element();
            Element.Classifier_ID = Supplier_GUID;

            //Wenn Stereotype nict vorhanden

            List<string> m_Stereotype = StereoType.Where(x => x != "").ToList();
            string SQL = "";
            if (m_Stereotype.Count > 0)
            {
                if(Database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    if(direction == true)
                    {
                        SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = @Start_Object_ID_1_0 AND End_Object_ID = @End_Object_ID_1_0 AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(StereoType.ToArray(), "Stereotype") + ") AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(Type.ToArray(), "Connector_Type") + ");";

                    }
                    else
                    {
                        SQL = "SELECT ea_guid FROM t_connector WHERE End_Object_ID = @Start_Object_ID_1_0 AND Start_Object_ID = @End_Object_ID_1_0 AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(StereoType.ToArray(), "Stereotype") + ") AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(Type.ToArray(), "Connector_Type") + ");";

                    }

                    }
                else
                {
                    if(direction == true)
                    {
                        SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = ? AND End_Object_ID = ? AND Stereotype IN(" + command.Add_Parameters_Pre(StereoType.ToArray()) + ") AND Connector_Type IN(" + command.Add_Parameters_Pre(Type.ToArray()) + ");";

                    }
                    else
                    {
                        SQL = "SELECT ea_guid FROM t_connector WHERE End_Object_ID = ? AND Start_Object_ID = ? AND Stereotype IN(" + command.Add_Parameters_Pre(StereoType.ToArray()) + ") AND Connector_Type IN(" + command.Add_Parameters_Pre(Type.ToArray()) + ");";

                    }
                   }

                //Hier schauen, ob zwischen dem Requirement und dem Element ein Connector mit dem Type und StereoType besteht. Wenn ja dann true zurück geben
                List<string> Check = new List<string>();


                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<int> help_id = new List<int>();
                List<int> help_id2 = new List<int>();
                help_id.Add(Requirement.Get_Object_ID(Database));
                help_id2.Add(Element.Get_Object_ID(Database));
                ee.Add(help_id.Select(x => new DB_Input(x, null)).ToArray());
                ee.Add(help_id2.Select(x => new DB_Input(x, null)).ToArray());
                ee.Add(StereoType.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(Type.Select(x => new DB_Input(-1, x)).ToArray());

                OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.Int, OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = {SqliteType.Integer, SqliteType.Integer , SqliteType.Text, SqliteType.Text};

                string[] m_output = { "ea_guid" };

                switch (Database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                        Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                        m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                        Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                        m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();

                        m_Name.Add("Start_Object_ID");
                        m_Name.Add("End_Object_ID");
                        m_Name.Add("Stereotype");
                        m_Name.Add("Connector_Type");


                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);


                        Database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                        m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                        break;
                }

                if (m_ret3[0].Ret.Count > 1)
                {
                    Check = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    Check = (null);
                }

                if (Check != null) //Existiert
                {
                    flag = true;
                }

                return (Check);
            }
            else
            {
                if (Database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    if(direction == true)
                    {
                        SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = @Start_Object_ID_1_0 AND End_Object_ID = @End_Object_ID_1_0 AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(Type.ToArray(), "Connector_Type") + ");";


                    }
                    else
                    {
                        SQL = "SELECT ea_guid FROM t_connector WHERE End_Object_ID = @Start_Object_ID_1_0 AND Start_Object_ID = @End_Object_ID_1_0 AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(Type.ToArray(), "Connector_Type") + ");";

                    }
                }
                else
                {
                    if (direction == true)
                    {
                        SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = ? AND End_Object_ID = ? AND Connector_Type IN(" + command.Add_Parameters_Pre(Type.ToArray()) + ");";

                    }
                    else
                    {
                        SQL = "SELECT ea_guid FROM t_connector WHERE End_Object_ID = ? AND Start_Object_ID = ? AND Connector_Type IN(" + command.Add_Parameters_Pre(Type.ToArray()) + ");";

                    }
                }
                    
                //Hier schauen, ob zwischen dem Requirement und dem Element ein Connector mit dem Type und StereoType besteht. Wenn ja dann true zurück geben
                List<string> Check = new List<string>();


                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<int> help_id = new List<int>();
                List<int> help_id2 = new List<int>();
                help_id.Add(Requirement.Get_Object_ID(Database));
                help_id2.Add(Element.Get_Object_ID(Database));
                ee.Add(help_id.Select(x => new DB_Input(x, null)).ToArray());
                ee.Add(help_id2.Select(x => new DB_Input(x, null)).ToArray());
                ee.Add(Type.Select(x => new DB_Input(-1, x)).ToArray());

                OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.BigInt,  OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.Int, OdbcType.Int,  OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Integer, SqliteType.Integer, SqliteType.Text};

                string[] m_output = { "ea_guid" };

                switch (Database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                        Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                        m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                        Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                        m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();

                        m_Name.Add("Start_Object_ID");
                        m_Name.Add("End_Object_ID");
                        m_Name.Add("Connector_Type");


                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);


                        Database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                        m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                        break;
                }

                if (m_ret3[0].Ret.Count > 1)
                {
                    Check = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    Check = (null);
                }

                if (Check != null) //Existiert
                {
                    flag = true;
                }

                return (Check);
            }



           
        }

        public List<string> Check_ProxyConnector_Supplier(string Connector_GUID, string Element_GUID, List<string> Type, List<string> StereoType, Database Database)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();
            bool flag = false;
            //     XML xml = new XML();
            //  EA.Element Element = Repository.GetElementByGuid(Element_GUID);
            //  EA.Element Requirement = Repository.GetElementByGuid(Classifier_ID);
            //Repository_Element Requirement = new Repository_Element();
            //Requirement.Classifier_ID = Client_GUID;
            Repository_Element Element = new Repository_Element();
            Element.Classifier_ID = Element_GUID;
            //Hier schauen, ob zwischen dem Requirement und dem Element ein Connector mit dem Type und StereoType besteht. Wenn ja dann true zurück geben
            List<string> Check = new List<string>();

            string SQL = "";

            if(Database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = @Start_Object_ID_1_0 AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type = @Object_Type_1_0 AND Classifier_guid = @Classifier_guid_1_0) AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(StereoType.ToArray(), "Stereotype") + ") AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(Type.ToArray(), "Connector_Type") + ");";
            }
            else
            {
                SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = ? AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type = ? AND Classifier_guid = ?) AND Stereotype IN(" + command.Add_Parameters_Pre(StereoType.ToArray()) + ") AND Connector_Type IN(" + command.Add_Parameters_Pre(Type.ToArray()) + ");";
            }

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_Type = new List<string>();
            help_Type.Add("ProxyConnector");
            List<string> help_guid = new List<string>();
            help_guid.Add(Connector_GUID);
            List<int> help_id2 = new List<int>();
            //help_id.Add(Requirement.Get_Object_ID(Database));
            help_id2.Add(Element.Get_Object_ID(Database));

            ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_id2.Select(x => new DB_Input(x, null)).ToArray());
            ee.Add(StereoType.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(Type.Select(x => new DB_Input(-1, x)).ToArray());


            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Integer, SqliteType.Text, SqliteType.Text };
            //OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
            //OdbcType[] m_input_Type_odbc = { OdbcType.Int, OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar };

            string[] m_output = { "ea_guid" };

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();

                    m_Name.Add("Object_Type");
                    m_Name.Add("Classifier_guid");
                    m_Name.Add("Start_Object_ID");
                    m_Name.Add("Stereotype");
                    m_Name.Add("Connector_Type");


                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;
            }

            if (m_ret3[0].Ret.Count > 1)
            {
                Check = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                Check = (null);
            }

            if (Check != null) //Existiert
            {
                flag = true;
            }

            return (Check);
        }
         #endregion Check

        #region Create
        public string Create_Connector(string Classifier_ID, string Element_GUID, List<string> m_StereoType, List<string> m_Type, string SubType, EA.Repository Repository, Database Data, string m_Toolbox, bool direction)
        {
            if(Data.metamodel.dB_Type == DB_Type.ACCDB)
            {
                Data.oLEDB_Interface.db_Close();
            }
            if (Data.metamodel.dB_Type == DB_Type.MSDASQL)
            {
                Data.oDBC_Interface.db_Close();
            }
            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                Data.SQLITE_Interface.db_Close();
            }


            // EA.Element Element = Repository.GetElementByGuid(Element_GUID);
            Repository_Element rep_Elem = new Repository_Element();
            rep_Elem.Classifier_ID = Element_GUID;
            rep_Elem.ID = rep_Elem.Get_Object_ID(Data);
            int elem_ID = rep_Elem.ID;
            EA.Element Requirement = Repository.GetElementByGuid(Classifier_ID);

            Repository_Plugin help = new Repository_Plugin();

            string Connector_GUID = help.Generate_GUID("t_connector", Repository, Data);
            string GUID2 = help.Generate_GUID("t_xref", Repository, Data);
            string SQL = "";

            string StereoType = m_StereoType[0];
            string Type = m_Type[0];

            if (SubType == "")
            {
                SubType = null;
            }

            if (StereoType != "")
            {
                if(Data.metamodel.dB_Type == DB_Type.SQLITE)
                {

                    if(direction == true)
                    {
                        SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '0', " + Requirement.ElementID + ", " + elem_ID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                        if (SubType == "Strong")
                        {
                            SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '2', " + Requirement.ElementID + ", " + elem_ID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                        }
                    }
                    else
                    {
                        SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '0', " + elem_ID + ", " + Requirement.ElementID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                        if (SubType == "Strong")
                        {
                            SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '2', " + elem_ID + ", " + Requirement.ElementID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                        }
                    }
 
                }
                else
                {
                    if (direction == true)
                    {
                        SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '0', " + Requirement.ElementID + ", " + elem_ID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                        if (SubType == "Strong")
                        {
                            SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '2', " + Requirement.ElementID + ", " + elem_ID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                        }
                    }
                    else
                    {
                        SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '0', " + elem_ID + ", " + Requirement.ElementID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                        if (SubType == "Strong")
                        {
                            SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '2', " + elem_ID + ", " + Requirement.ElementID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                        }
                    }
                       
                }


                Repository.Execute(SQL);
            }
            else
            {
                if (direction == true)
                {
                    SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, Start_Object_ID, End_Object_ID, Btm_Mid_Label, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', " + Requirement.ElementID + ", " + elem_ID + ", '" + "<<" + StereoType + ">>" + "' , " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";

                }
                else
                {
                    SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, Start_Object_ID, End_Object_ID, Btm_Mid_Label, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', " + elem_ID + ", " + Requirement.ElementID + ", '" + "<<" + StereoType + ">>" + "' , " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";

                }

                Repository.Execute(SQL);
            }

            //Wenn MDG TechnologY File übernommen wird, muss hier in t_xref noch eingefügt werden; Es folgt ein Beispiel
            var SQL2 = "Insert INTO t_xref(XrefID, Name, Type, Visibility, Partition, Description, Client, Supplier) VALUES ('" + GUID2 + "', 'Stereotypes', 'connector property', 'Public', 0, '@STEREO;Name=" + StereoType + ";FQName="+m_Toolbox+"::" + StereoType + ";@ENDSTEREO;', '" + Connector_GUID + "','<none>');";

            Repository.Execute(SQL2);

            //    Element.Connectors.Refresh();
            //    Requirement.Connectors.Refresh();
        //    Data.oLEDB_Interface.db_Open();
            return (Connector_GUID);
        }

        public string Create_ProxyConnector_Supplier(string Connector_guid, string Element_GUID, List<string> m_StereoType, List<string> m_Type, string SubType, EA.Repository Repository, Database Data, string Toolbox, bool direction)
        {
            if (Data.metamodel.dB_Type == DB_Type.ACCDB)
            {
                Data.oLEDB_Interface.db_Close();
            }
            if (Data.metamodel.dB_Type == DB_Type.MSDASQL)
            {
                Data.oDBC_Interface.db_Close();
            }
            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                Data.SQLITE_Interface.db_Close();
            }


            // EA.Element Element = Repository.GetElementByGuid(Element_GUID);
            Repository_Element rep_Elem = new Repository_Element();
            rep_Elem.Classifier_ID = Element_GUID;
            rep_Elem.ID = rep_Elem.Get_Object_ID(Data);
            int package_id = rep_Elem.Get_Package_ID(Data);
            int elem_ID = rep_Elem.ID;
            //ProxyConnector erzeugen
            EA.Package package = Repository.GetPackageByID(package_id);
            
            //EA.Element element_parnet = Repository.GetElementByID(elem_ID);
            EA.Collection elem_col = package.Elements;
            EA.Element Element = elem_col.AddNew("ProxyConnector", "ProxyConnector");
            //Element.ParentID = elem_ID;
            Element.Update();
           // element_parnet.Update();
            package.Elements.Refresh();

            var t = "UPDATE t_object SET Classifier_guid = '" + Connector_guid + "' WHERE Object_ID = " + Element.ElementID + ";";
            Repository.Execute(t);
            //EA.Element Requirement = Repository.GetElementByGuid(Classifier_ID);

            Repository_Plugin help = new Repository_Plugin();

            string Connector_GUID = help.Generate_GUID("t_connector", Repository, Data);
            string GUID2 = help.Generate_GUID("t_xref", Repository, Data);
            string SQL = "";

            string StereoType = m_StereoType[0];
            string Type = m_Type[0];

            if (SubType == "")
            {
                SubType = null;
            }

            if (StereoType != "")
            {

                if(direction == true)
                {
                    SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '0', " + elem_ID + ", " + Element.ElementID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                    if (SubType == "Strong")
                    {
                        SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '2', " + elem_ID + ", " + Element.ElementID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                    }
                }
                else
                {
                    SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '0', " + Element.ElementID + ", " + elem_ID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                    if (SubType == "Strong")
                    {
                        SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, DestIsAggregate, Start_Object_ID, End_Object_ID, Btm_Mid_Label, Stereotype, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', '2', " + Element.ElementID + ", " + elem_ID + ", '" + "<<" + StereoType + ">>" + "' , '" + StereoType + "', " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";
                    }
                }

                Repository.Execute(SQL);
            }
            else
            {
                if (direction == true)
                {
                    SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, Start_Object_ID, End_Object_ID, Btm_Mid_Label, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', " + elem_ID + ", " + Element.ElementID + ", '" + "<<" + StereoType + ">>" + "' , " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";

                }
                else
                {
                    SQL = "INSERT INTO t_connector (Direction, Connector_Type, SubType, SourceAccess, DestAccess, SourceContainment, DestContainment, Start_Object_ID, End_Object_ID, Btm_Mid_Label, VirtualInheritance, ea_guid, SourceChangeable, DestChangeable, SourceTS, DestTS) VALUES ('Source -> Destination', '" + Type + "', '" + SubType + "', 'Public', 'Public', 'Unspecified', 'Unspecified', " + Element.ElementID + ", " + elem_ID + ", '" + "<<" + StereoType + ">>" + "' , " + 0 + ", '" + Connector_GUID + "', 'none', 'none', 'instance', 'instance');";

                }

                Repository.Execute(SQL);
            }

            //Wenn MDG TechnologY File übernommen wird, muss hier in t_xref noch eingefügt werden; Es folgt ein Beispiel
            var SQL2 = "Insert INTO t_xref(XrefID, Name, Type, Visibility, Partition, Description, Client, Supplier) VALUES ('" + GUID2 + "', 'Stereotypes', 'connector property', 'Public', 0, '@STEREO;Name=" + StereoType + ";FQName="+Toolbox+"::" + StereoType + ";@ENDSTEREO;', '" + Connector_GUID + "','<none>');";

            Repository.Execute(SQL2);

            //    Element.Connectors.Refresh();
            //    Requirement.Connectors.Refresh();
            //    Data.oLEDB_Interface.db_Open();
            return (Connector_GUID);
        }

        #endregion Create

        #region Delete
        public void Delete_Connectors(Database Database, List<string> m_Connector_GUID, EA.Repository repository)
        {
            if(m_Connector_GUID.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.Delete_Connector(Database, m_Connector_GUID[i1], repository);

                    i1++;
                } while (i1 < m_Connector_GUID.Count);
            }
        }

        public void Delete_Connector(Database Database, string Connector_GUID, EA.Repository repository)
        {
            DB_Command command = new DB_Command();
         //   XML xML = new XML();
            //Connector_ID erhalten
            /*  string SQL = "SELECT Connector_ID FROM t_connector WHERE ea_guid = '" + Connector_GUID + "';";
              string xml_String = repository.SQLQuery(SQL);
              List<string> m_Connector_ID = xML.Xml_Read_Attribut("Connector_ID", xml_String);
              */
            List<int> m_Connector_ID = new List<int>();

            string SQL = "";

            List<DB_Return> m_ret3 = new List<DB_Return>();

            if (Database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT Connector_ID FROM t_connector WHERE ea_guid = @ea_guid_1_0;";


                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<string> help_id = new List<string>();
                help_id.Add(Connector_GUID);
                ee.Add(help_id.Select(x => new DB_Input(-1, x)).ToArray());

                string[] m_output = { "Connector_ID" };

                SqliteCommand SELECT4 = new SqliteCommand(SQL, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                List<string> m_Name = new List<string>();

                m_Name.Add("ea_guid");


                SqliteType[] m_input_Type_sqlite = { SqliteType.Text};

                List<SqliteType> sqliteTypes = new List<SqliteType>();
                sqliteTypes.Add(SqliteType.Integer);


                Database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                
            }
            else
            {
                SQL = "SELECT Connector_ID FROM t_connector WHERE ea_guid = ?;";

                OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Database.oLEDB_Interface.dbConnection);

                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<string> help_id = new List<string>();
                help_id.Add(Connector_GUID);
                ee.Add(help_id.Select(x => new DB_Input(-1, x)).ToArray());


                OleDbType[] m_input_Type = { OleDbType.VarChar };
                Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
                string[] m_output = { "Connector_ID" };

                m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
            }


            

            if (m_ret3[0].Ret.Count > 1)
            {
                m_Connector_ID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList());
            }
            else
            {
                m_Connector_ID = (null);
            }


            if (m_Connector_ID != null)
            {
                var SQL1 = "DELETE FROM t_connectortag WHERE t_connectortag.ElementID = " + m_Connector_ID[0] + ";"; // 	t_connectortag

                var SQL2 = "DELETE FROM t_xref WHERE t_xref.Client = '" + Connector_GUID + "';"; // t_xref

                var SQL3 = "DELETE FROM t_connector WHERE t_connector.Connector_ID = " + m_Connector_ID[0] + ";"; // t_connector

                var SQL4 = "DELETE FROM t_diagramlinks WHERE t_diagramlinks.ConnectorID = " + m_Connector_ID[0] + ";"; // t_connector

                var SQL5 = "DELETE FROM t_object WHERE Object_Type = 'ProxyConnector' AND Classifier_guid = '" + Connector_GUID + "';";

                repository.Execute(SQL5);
                repository.Execute(SQL4);
                repository.Execute(SQL1);
                repository.Execute(SQL2);
                repository.Execute(SQL3);

            }
        }

        public void Delete_Connector_by_IDs(Database Database, EA.Repository Repository, string Client_GUID, string Supplier_GUID, List<string> m_Stereotype, List<string> m_Type)
        {
          //  XML xml = new XML();
            DB_Command command = new DB_Command();

         //   List<string> m_Stereotype = Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
         //   List<string> m_Type = Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
            Repository_Element Requirement = new Repository_Element();
            Requirement.Classifier_ID = Client_GUID;
            Repository_Element Element = new Repository_Element();
            Element.Classifier_ID = Supplier_GUID;


            /*  string SQL = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = " + Repository.GetElementByGuid(Classifier_ID).ElementID + " AND End_Object_ID = " + Repository.GetElementByGuid(Supplier_GUID).ElementID + " AND Stereotype IN" +xml.SQL_IN_Array(metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList().ToArray()) + " AND Connector_Type IN" + xml.SQL_IN_Array(metamodel.m_Derived_Element.Select(x => x.Type).ToList().ToArray()) + ";";
              string xml_Dat = Repository.SQLQuery(SQL);
              List<string> GUID = xml.Xml_Read_Attribut("ea_guid", xml_Dat);
              */
            List<string> GUID = new List<string>();

            string SQL_2 = "";

            List<DB_Return> m_ret3 = new List<DB_Return>();

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<int> help_id = new List<int>();
            List<int> help_id2 = new List<int>();

            string[] m_output = { "ea_guid" };
            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    SQL_2 = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = ? AND End_Object_ID = ? AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ") AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type.ToArray()) + ");";
                    OleDbCommand SELECT1 = new OleDbCommand(SQL_2, (OleDbConnection)Database.oLEDB_Interface.dbConnection);

                    help_id.Add(Requirement.Get_Object_ID(Database));
                    help_id2.Add(Element.Get_Object_ID(Database));
                    ee.Add(help_id.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(help_id2.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());

                    OleDbType[] m_input_Type = { OleDbType.BigInt, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
                    Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
                   
                    m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.SQLITE:
                    SQL_2 = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = @Start_Object_ID_1_0 AND End_Object_ID = @End_Object_ID_1_0 AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Stereotype") + ") AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Connector_Type") + ");";
                    SqliteCommand SELECT4 = new SqliteCommand(SQL_2, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                 //   Database.SQLITE_Interface.db_Close();

                    List<string> m_Name = new List<string>();

                    SqliteType[] m_input_Type_sqlite = {SqliteType.Integer, SqliteType.Integer , SqliteType.Text, SqliteType.Text};

                    m_Name.Add("Start_Object_ID");
                    m_Name.Add("End_Object_ID");
                    m_Name.Add("Stereotype");
                    m_Name.Add("Connector_Type");
                    help_id.Add(Requirement.Get_Object_ID(Database));
                    help_id2.Add(Element.Get_Object_ID(Database));
                    ee.Add(help_id.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(help_id2.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes));
                    break;
            }
            
           

            

            if (m_ret3[0].Ret.Count > 1)
            {
                GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                GUID = (null);
            }


            if (GUID != null)
            {
                int i1 = 0;
                do
                {
                    Delete_Connector(Database, GUID[i1], Repository);


                    /*EA.Connector Connector = Repository.GetConnectorByGuid(GUID[i1]);

                    string SQL1 = "DELETE FROM t_connectortag WHERE t_connectortag.ElementID = " + Connector.ConnectorID + ";"; // 	t_connectortag

                    string SQL2 = "DELETE FROM t_xref WHERE t_xref.Client = '" + Connector.ConnectorGUID + "';"; // t_xref

                    string SQL3 = "DELETE FROM t_connector WHERE t_connector.Connector_ID = " + Connector.ConnectorID + ";"; // t_connector

                    string SQL4 = "DELETE FROM t_diagramlinks WHERE t_diagramlinks.ConnectorID = " + Connector.ConnectorID + ";"; // t_connector

                    Repository.Execute(SQL4);
                    Repository.Execute(SQL1);
                    Repository.Execute(SQL2);
                    Repository.Execute(SQL3);*/

                    i1++;
                } while (i1 < GUID.Count);

            }

        }
        #endregion Delete

        #region Update
        public void Update_Connector(Database Database, List<string> m_Type, List<string> m_Stereoype, string Supplier_GUID, string Connector_GUID)
        {
            //Updaten
            DB_Command sQL_Command = new DB_Command();
            /*  string SQL = "UPDATE t_connector SET Connector_Type IN" + xML.SQL_IN_Array(Database.metamodel.m_Derived_Logical.Select(x => x.Type).ToList().ToArray()) + ", Stereotype IN" +xML.SQL_IN_Array(Database.metamodel.m_Derived_Logical.Select(x => x.Stereotype).ToList().ToArray()) + ", End_Object_ID = " + repository.GetElementByGuid(Sys_GUID).ElementID + " WHERE ea_guid = '" + Connector_GUID + "';";

              repository.Execute(SQL);*/

           // List<string> m_Type_1 = Database.metamodel.m_Derived_Logical.Select(x => x.Type).ToList();
           // List<string> m_Stereoype_1 = Database.metamodel.m_Derived_Logical.Select(x => x.Type).ToList();
            Repository_Element rep = new Repository_Element();
            rep.Classifier_ID = Supplier_GUID;

            string[] m_input_property4 = { "Connector_Type", "Stereotype", "End_Object_ID" };
            object[] m_input_value4 = { m_Type[0], m_Stereoype[0], rep.Get_Object_ID(Database) };
            
            string[] m_select_property4 = { "ea_guid" };
            object[] m_v_1 = { Connector_GUID };
            List<object[]> m_select_value = new List<object[]>();
           
            m_select_value.Add(m_v_1);

            OleDbType[] m_select_Type_oledb = { OleDbType.BigInt, OleDbType.VarChar };
            OdbcType[] m_select_Type_odbc = { OdbcType.Int, OdbcType.VarChar };
            SqliteType[] m_select_Type_sqlite = { SqliteType.Text };

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Integer };

            string update = "";

            if (Database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                update = sQL_Command.Get_Update_Command_SQLITE("t_connector", m_input_property4, m_input_value4, m_select_property4, m_select_value);
            }
            else
            {
                update = sQL_Command.Get_Update_Command("t_connector", m_input_property4, m_input_value4, m_select_property4, m_select_value, Database.metamodel.dB_Type);
            }
            

          
            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand Update = new OleDbCommand(update, (OleDbConnection)Database.oLEDB_Interface.dbConnection);
                    Database.oLEDB_Interface.Add_Parameters_Update(Update, m_input_value4, m_input_Type_oledb, m_select_value, m_select_Type_oledb);
                    Database.oLEDB_Interface.OLEDB_UPDATE_One_Table(Update);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand Update2 = new OdbcCommand(update, (OdbcConnection)Database.oDBC_Interface.dbConnection);
                    Database.oDBC_Interface.Add_Parameters_Update(Update2, m_input_value4, m_input_Type_odbc, m_select_value, m_select_Type_odbc);
                    Database.oDBC_Interface.DB_UPDATE_One_Table(Update2);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand Update3 = new SqliteCommand(update, (SqliteConnection)Database.SQLITE_Interface.dbConnection);

                    List<DB_Input> m_db = new List<DB_Input>();
                    m_db.Add(new DB_Input(-1, m_Type[0]));
                    m_db.Add(new DB_Input(-1, m_Stereoype[0]));
                    m_db.Add(new DB_Input(rep.Get_Object_ID(Database), null));

                    Database.SQLITE_Interface.Add_Parameters_Update(Update3, m_db.ToArray(), m_input_property4, m_input_Type_sqlite, m_select_property4, m_select_value, m_select_Type_sqlite);
                    Database.SQLITE_Interface.DB_UPDATE_One_Table(Update3);
                    break;
            }

        }
        #endregion Update
    }
}

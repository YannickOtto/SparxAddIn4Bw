using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

using Database_Connection;
using Ennumerationen;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Requirement_Plugin.Interfaces
{
  public  class Interface_Collection_Requirements
    {
       
        public List<string> Get_Elements_GUID(Database database, List<string> m_Type, List<string> m_Stereotype)
        {
            //Allgemeine Variablen
            DB_Command command = new DB_Command();
            List<string> m_GUID = new List<string>();
            List<string> m_GUID2 = new List<string>();
            List<string> m_GUID3 = new List<string>();

            List<string> m_Stereotype_ADMBw = database.metamodel.m_Requirement_ADMBw.Select(x => x.Stereotype).ToList();

            List<DB_Input[]> ee = new List<DB_Input[]>();
            DB_Input[] help = new DB_Input[m_Type.Count];

            List<string> help_Property = new List<string>();
            help_Property.Add(database.metamodel.Afo_Stereotype[0]);
            //ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_Property.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());
           
            string[] m_output = { "Object_ID" };
            string[] m_output2 = { "ea_guid" };
            string[] m_output3 = { "PDATA1" };
            string table = "t_object";
            string[] m_input_Property = { "Object_Type", "Stereotype" };

            
            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text};

            if (m_Type.Count > 0)
            {
                if (database.metamodel.flag_Analyse_Diagram == true)
                {
                    //Im Diagramm
                    #region Diagramelemente
                    //Klassen direkt
                    #region Klasse
                    List<DB_Return> m_ret = new List<DB_Return>();

                    string select = "";

                    if (database.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        select = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_ADMBw.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property = @Property_1_0 AND Value IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Value") + ") );";

                    }
                    else
                    {
                        select = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_ADMBw.ToArray()) + ") AND Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property = ? AND Value IN (" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ") );";
                    }

                     
                   // string select = command.Get_Select_Command(table, m_output, m_input_Property, ee);

                    switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT = new OleDbCommand(select, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_oledb);
                            SELECT.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT.CommandText + "));";
                            m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output2);
                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT2 = new OdbcCommand(select, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                            SELECT2.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT2.CommandText + "));";
                            m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output2);
                            break;
                        case DB_Type.SQLITE:
                            SqliteCommand SELECT3 = new SqliteCommand(select, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                            List<string> m_str = new List<string>();
                            m_str.Add("Property");
                            m_str.Add("Value");
                            m_str.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_str);
                            SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT3.CommandText + "));";
                            m_ret = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output2, sqliteTypes);
                            break;
                    }

                    if (m_ret[0].Ret.Count > 1)
                    {
                        m_GUID = (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                    }
                    else
                    {
                        m_GUID = (null);
                    }
                    #endregion Klasse
                    //Instanzen 
                    #region Instanz
                    List<DB_Return> m_ret_Instanz = new List<DB_Return>();

                    

                    string select_instanz = "";

                    if (database.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        select_instanz =  "SELECT ea_guid FROM t_object WHERE Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_ADMBw.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property = @Property_1_0 AND Value IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Value") + ") );";

                    }
                    else
                    {
                        select_instanz = "SELECT ea_guid FROM t_object WHERE Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_ADMBw.ToArray()) + ") AND Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property = ? AND Value IN (" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ") );";
                    }
                    //string select_instanz = command.Get_Select_Command(table, m_output2, m_input_Property, ee);

                    switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT_Instanz = new OleDbCommand(select_instanz, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT_Instanz, ee, m_input_Type_oledb);
                            SELECT_Instanz.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz.CommandText + ");";
                            m_ret_Instanz = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_Instanz, m_output3);

                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT_Instanz2 = new OdbcCommand(select_instanz, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT_Instanz2, ee, m_input_Type_odbc);
                            SELECT_Instanz2.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz2.CommandText + ");";
                            m_ret_Instanz = database.oDBC_Interface.DB_SELECT_One_Table(SELECT_Instanz2, m_output3);
                            break;
                        case DB_Type.SQLITE:
                            SqliteCommand SELECT_Instanz3 = new SqliteCommand(select, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                            List<string> m_str = new List<string>();
                            m_str.Add("Property");
                            m_str.Add("Value");
                            m_str.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT_Instanz3, ee, m_input_Type_sqlite, m_str);
                            SELECT_Instanz3.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz3.CommandText + "));";
                            m_ret_Instanz = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT_Instanz3, m_output3, sqliteTypes);
                            break;
                    }

                    if (m_ret_Instanz[0].Ret.Count > 1)
                    {
                        m_GUID2 = (m_ret_Instanz[0].Ret.GetRange(1, m_ret_Instanz[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                    }
                    else
                    {
                        m_GUID2 = (null);
                    }


                    if (m_GUID != null && m_GUID2 != null)
                    {
                        m_GUID.AddRange(m_GUID2);
                        m_GUID = m_GUID.Distinct().ToList();

                    }

                    if (m_GUID == null)
                    {
                        m_GUID = m_GUID2;
                    }
                    #endregion Instanz
                    #endregion Diagramelemente
                }
                else
                {
                    #region Alle Elemente
                    List<DB_Return> m_ret2 = new List<DB_Return>();

                    string select2 = "";

                    if (database.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        select2 = "SELECT ea_guid FROM t_object WHERE Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_ADMBw.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property = @Property_1_0 AND Value IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Value") + ") );";

                    }
                    else
                    {
                        select2 = "SELECT ea_guid FROM t_object WHERE Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_ADMBw.ToArray()) + ") AND Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property = ? AND Value IN (" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ") );";
                    }


                    // string select2 = command.Get_Select_Command(table, m_output2, m_input_Property, ee);

                    switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                            m_ret2 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output2);
                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                            m_ret2 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output2);
                            break;
                        case DB_Type.SQLITE:
                            SqliteCommand SELECT4= new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                            List<string> m_str = new List<string>();
                            m_str.Add("Property");
                            m_str.Add("Value");
                            m_str.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                            //SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT3.CommandText + "));";
                            m_ret2 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output2, sqliteTypes);
                            break;
                    }



                    if (m_ret2[0].Ret.Count > 1)
                    {
                        m_GUID = (m_ret2[0].Ret.GetRange(1, m_ret2[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                    }
                    else
                    {
                        m_GUID = (null);
                    }
                    #endregion Alle Elemente
                }

                if (m_GUID != null)
                {
                    if (m_GUID.Count > 0)
                    {

                        return (m_GUID);
                    }

                }
            }

           

            return (null);
        }

        public List<string> Get_DiagramElements_GUID(Database database, List<string> m_Type, List<string> m_Stereotype, int Diagram_ID)
        {
            //Allgemeine Variablen
            DB_Command command = new DB_Command();
            List<string> m_GUID = new List<string>();
            List<string> m_GUID2 = new List<string>();
            List<string> m_GUID3 = new List<string>();

            List<DB_Input[]> ee = new List<DB_Input[]>();
            DB_Input[] help = new DB_Input[m_Type.Count];
            ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
            //ee.Add(m_Stereotype.Select(x => new DB_Input(Diagram_ID, null)).ToArray());

            string[] m_output = { "Object_ID" };
            string[] m_output2 = { "ea_guid" };
            string[] m_output3 = { "PDATA1" };
            string table = "t_object";
            string[] m_input_Property = { "Object_Type", "Stereotype" };


            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text};


            if (m_Type.Count > 0)
            {
               
                    //Im Diagramm
                    #region Diagramelemente
                    //Klassen direkt
                    #region Klasse
                    List<DB_Return> m_ret = new List<DB_Return>();
                   

                string select = "";

                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                }
                else
                {
                     select = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                }

                switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT = new OleDbCommand(select, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_oledb);
                            SELECT.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Diagram_ID = "+Diagram_ID+" AND Object_ID IN (" + SELECT.CommandText + "));";
                            m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output2);
                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT2 = new OdbcCommand(select, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                            SELECT2.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Diagram_ID = " + Diagram_ID + " AND Object_ID IN (" + SELECT2.CommandText + "));";
                            m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output2);
                            break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT3 = new SqliteCommand(select, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                        List<string> m_str = new List<string>();
                        m_str.Add("Object_Type");
                        m_str.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_str);
                        SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Diagram_ID = @Diagram_ID_1_0 AND Object_ID IN (" + SELECT3.CommandText + "));";
                        SELECT3.Parameters.AddWithValue("@Diagram_ID_1_0", Diagram_ID).SqliteType = SqliteType.Integer;
                        m_ret = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output2, sqliteTypes);
                        break;
                }

                    if (m_ret[0].Ret.Count > 1)
                    {
                        m_GUID = (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                    }
                    else
                    {
                        m_GUID = (null);
                    }
                    #endregion Klasse
                    //Instanzen 
                    #region Instanz
                    List<DB_Return> m_ret_Instanz = new List<DB_Return>();

                    string select_instanz = "";

                    if (database.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                    select_instanz = command.Get_Select_Command_SQLITE(table, m_output2, m_input_Property, ee);
                    }
                    else
                    {
                    select_instanz = command.Get_Select_Command(table, m_output2, m_input_Property, ee);
                }

                switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT_Instanz = new OleDbCommand(select_instanz, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT_Instanz, ee, m_input_Type_oledb);
                            SELECT_Instanz.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz.CommandText + ");";
                            m_ret_Instanz = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_Instanz, m_output3);

                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT_Instanz2 = new OdbcCommand(select_instanz, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT_Instanz2, ee, m_input_Type_odbc);
                            SELECT_Instanz2.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz2.CommandText + ");";
                            m_ret_Instanz = database.oDBC_Interface.DB_SELECT_One_Table(SELECT_Instanz2, m_output3);
                            break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT_Instanz3 = new SqliteCommand(select_instanz, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        List<string> m_str = new List<string>();
                        m_str.Add("Object_Type");
                        m_str.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT_Instanz3, ee, m_input_Type_sqlite, m_str);
                        SELECT_Instanz3.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz3.CommandText + ");";
                       // SELECT_Instanz3.Parameters.AddWithValue("@Diagram_ID_1_0", Diagram_ID).SqliteType = SqliteType.Integer;
                        m_ret_Instanz = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT_Instanz3, m_output2, sqliteTypes);
                        break;
                }

                    if (m_ret_Instanz[0].Ret.Count > 1)
                    {
                        m_GUID2 = (m_ret_Instanz[0].Ret.GetRange(1, m_ret_Instanz[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                    }
                    else
                    {
                        m_GUID2 = (null);
                    }


                    if (m_GUID != null && m_GUID2 != null)
                    {
                        m_GUID.AddRange(m_GUID2);
                        m_GUID = m_GUID.Distinct().ToList();

                    }

                    if (m_GUID == null)
                    {
                        m_GUID = m_GUID2;
                    }
                    #endregion Instanz
                    #endregion Diagramelemente
                
             
                if (m_GUID != null)
                {
                    if (m_GUID.Count > 0)
                    {

                        return (m_GUID);
                    }

                }
            }



            return (null);
        }

        public List<string> Get_Elements_By_Package(Database database, int Package_ID, List<string> m_Type, List<string> m_Stereotype)
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret4 = new List<DB_Return>();

            List<string> m_NodeType_Logical_GUID = new List<string>();
            //List<string> m_Type = database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

             string SQL12 = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL12 = "SELECT ea_guid FROM t_object WHERE Package_ID =  @Package_ID_1_0 AND Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Stereotype") + ");";
            }
            else
            {
                SQL12 = "SELECT ea_guid FROM t_object WHERE Package_ID = ? AND Object_Type IN(" + command.Add_Parameters_Pre(m_Type.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ");";
            }


            List<DB_Input[]> ee2 = new List<DB_Input[]>();
            List<int> help_ee = new List<int>();
            help_ee.Add(Package_ID);
            ee2.Add(help_ee.Select(x => new DB_Input(x, null)).ToArray());
            ee2.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee2.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());

            string[] m_output2 = { "ea_guid" };

            OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type2_odbc = { OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer, SqliteType.Text, SqliteType.Text};

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT = new OleDbCommand(SQL12, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT, ee2, m_input_Type_oledb);
                    m_ret4 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output2);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL12, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee2, m_input_Type2_odbc);
                    m_ret4 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output2);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL12, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    m_Name.Add("Package_ID");
                    m_Name.Add("Object_Type");
                    m_Name.Add("Stereotype");

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee2, m_input_Type_sqlite, m_Name);
                    m_ret4 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output2, sqliteTypes);
                    break;
            }

           

            if (m_ret4[0].Ret.Count > 1)
            {
                m_NodeType_Logical_GUID = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_NodeType_Logical_GUID = null;
            }

            return (m_NodeType_Logical_GUID);
        }

        public List<int> Get_Elements_ID(Database database, List<string> m_Type, List<string> m_Stereotype)
        {
            
            if(m_Type.Count >0)
            {
                List<int> m_GUID = new List<int>();
                List<int> m_GUID2 = new List<int>();
                List<int> m_GUID3 = new List<int>();

                List<DB_Input[]> ee = new List<DB_Input[]>();
                DB_Input[] help = new DB_Input[m_Type.Count];
                ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());

                string[] m_output = { "Object_ID" };
                string[] m_output2 = { "Object_ID" };
                string[] m_output3 = { "Object_ID" };
                string table = "t_object";
                string[] m_input_Property = { "Object_Type", "Stereotype" };

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = {SqliteType.Text, SqliteType.Text };

                DB_Command command = new DB_Command();

                if (database.metamodel.flag_Analyse_Diagram == true)
                {
                    //Im Diagramm
                    #region Diagram
                    //Klassen direkt
                    #region Klasse
                    List<DB_Return> m_ret = new List<DB_Return>();

                    string select = "";

                    if (database.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        select = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                    }
                    else
                    {
                        select = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                    }

                     

                    switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT = new OleDbCommand(select, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_oledb);
                            SELECT.CommandText = "SELECT Object_ID FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT.CommandText + "));";
                            m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output2);
                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT2 = new OdbcCommand(select, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                            SELECT2.CommandText = "SELECT Object_ID FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT2.CommandText + "));";
                            m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output2);
                            break;
                        case DB_Type.SQLITE:
                            SqliteCommand SELECT3 = new SqliteCommand(select, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_Type");
                            m_Name.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Integer);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                            SELECT3.CommandText = "SELECT Object_ID FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT3.CommandText + "));";
                            m_ret = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output2, sqliteTypes);

                            break;
                    }

                    if (m_ret[0].Ret.Count > 1)
                    {
                        m_GUID = (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList());
                    }
                    else
                    {
                        m_GUID = (null);
                    }
                    #endregion Klasse
                    //Instanzen 
                    #region Instanzen
                    List<DB_Return> m_ret_Instanz = new List<DB_Return>();

                    string select_instanz = "";

                    if (database.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        select_instanz = command.Get_Select_Command_SQLITE(table, m_output2, m_input_Property, ee);
                    }
                    else
                    {
                        select_instanz =  command.Get_Select_Command(table, m_output2, m_input_Property, ee);
                    }
                     

                    switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT_Instanz = new OleDbCommand(select_instanz, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT_Instanz, ee, m_input_Type_oledb);
                            SELECT_Instanz.CommandText = "SELECT Object_ID FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz.CommandText + ");";
                            m_ret_Instanz = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_Instanz, m_output3);
                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT_Instanz2 = new OdbcCommand(select_instanz, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT_Instanz2, ee, m_input_Type_odbc);
                            SELECT_Instanz2.CommandText = "SELECT Object_ID FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz2.CommandText + ");";
                            m_ret_Instanz = database.oDBC_Interface.DB_SELECT_One_Table(SELECT_Instanz2, m_output3);
                            break;
                        case DB_Type.SQLITE:
                            SqliteCommand SELECT_Instanz3 = new SqliteCommand(select_instanz, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_Type");
                            m_Name.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Integer);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT_Instanz3, ee, m_input_Type_sqlite, m_Name);
                            SELECT_Instanz3.CommandText = "SELECT Object_ID FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz3.CommandText + ");";
                            m_ret_Instanz = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT_Instanz3, m_output3, sqliteTypes);

                            break;
                    }


                    if (m_ret_Instanz[0].Ret.Count > 1)
                    {
                        m_GUID2 = (m_ret_Instanz[0].Ret.GetRange(1, m_ret_Instanz[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList());
                    }
                    else
                    {
                        m_GUID2 = (null);
                    }


                    if (m_GUID != null && m_GUID2 != null)
                    {
                        m_GUID.AddRange(m_GUID2);
                        m_GUID = m_GUID.Distinct().ToList();

                    }

                    if (m_GUID == null)
                    {
                        m_GUID = m_GUID2;
                    }
                    #endregion Instanzen
                    #endregion Diagram
                }
                else
                {
                    #region Alle Elemente
                    List<DB_Return> m_ret2 = new List<DB_Return>();

                    string select2 = "";

                    if (database.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        select2 = command.Get_Select_Command_SQLITE(table, m_output2, m_input_Property, ee);
                    }
                    else
                    {
                        select2 = command.Get_Select_Command(table, m_output2, m_input_Property, ee);
                    }

                   

                    switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                            m_ret2 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output2);
                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                            m_ret2 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output2);
                            break;
                        case DB_Type.SQLITE:
                            SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_Type");
                            m_Name.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Integer);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                            m_ret2 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output2, sqliteTypes);

                            break;
                    }



                    if (m_ret2[0].Ret.Count > 1)
                    {
                        m_GUID = (m_ret2[0].Ret.GetRange(1, m_ret2[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList());
                    }
                    else
                    {
                        m_GUID = (null);
                    }
                    #endregion Alle Elemente
                }

                if (m_GUID != null)
                {
                    if (m_GUID.Count > 0)
                    {

                        return (m_GUID);
                    }

                }
            }
           

            return (null);
        }

    }
}

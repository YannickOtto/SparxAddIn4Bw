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
using EA;

namespace Requirement_Plugin.Interfaces
{
  public  class Interface_Collection
    {
        public void Test(Database database)
        {
            List<string> m_ID = this.Get_Elements_GUID(database, database.metamodel.m_Szenar.Select(x => x.Type).ToList(), database.metamodel.m_Szenar.Select(x => x.Stereotype).ToList());

           // MessageBox.Show(m_ID.Count.ToString());

            Interface_Element interface_Element = new Interface_Element();

            List<string> m_child = interface_Element.Get_Children_Element(m_ID[0], database, database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList(), database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList());

            MessageBox.Show(m_child.Count.ToString());
        }

        public List<string> Get_Elements_GUID(Database database, List<string> m_Type, List<string> m_Stereotype)
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

            string[] m_output = { "Object_ID" };
            string[] m_output2 = { "ea_guid" };
            string[] m_output3 = { "PDATA1" };
            string table = "t_object";
            string[] m_input_Property = { "Object_Type", "Stereotype" };

            
            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };

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

                    if(database.metamodel.dB_Type == DB_Type.SQLITE)
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

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_Type");
                            m_Name.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
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

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_Type");
                            m_Name.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT_Instanz3, ee, m_input_Type_sqlite, m_Name);
                            SELECT_Instanz3.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz3.CommandText + ");";
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
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);

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

        public List<string> Get_Elements_By_Type_GUID(Database database, List<string> m_Type)
        {
            //Allgemeine Variablen
            DB_Command command = new DB_Command();
            List<string> m_GUID = new List<string>();
            List<string> m_GUID2 = new List<string>();
            List<string> m_GUID3 = new List<string>();

            List<DB_Input[]> ee = new List<DB_Input[]>();
            DB_Input[] help = new DB_Input[m_Type.Count];
            ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
         

            string[] m_output = { "Object_ID" };
            string[] m_output2 = { "ea_guid" };
            string[] m_output3 = { "PDATA1" };
            string table = "t_object";
            string[] m_input_Property = { "Object_Type"};


            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar};
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar};
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text};

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
                        select = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                    }
                    else
                    {
                        select = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                    }

                    //string select = command.Get_Select_Command(table, m_output, m_input_Property, ee);

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

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_Type");
                           
                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
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

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_Type");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT_Instanz3, ee, m_input_Type_sqlite, m_Name);
                            SELECT_Instanz3.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN (" + SELECT_Instanz3.CommandText + ");";
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

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_Name);
                            //SELECT4.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT4.CommandText + "));";
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
        public List<string> Get_Stereotype_By_Type_AND_GUID(Database database, List<string> m_guid)
        {
            string Output_Name = "Stereotype";
            string TABLE = "t_object";
            DB_Command command = new DB_Command();

            List<string> m_Output = new List<string>();
            List<DB_Input[]> ee = new List<DB_Input[]>();
            ee.Add(m_guid.Select(x => new DB_Input(-1, x)).ToArray());
            string[] m_output = { Output_Name };


            string SQL = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE Object_Type IN ('Class', 'Acitivity', 'Object', 'InformationItem') AND ea_guid IN (" + command.Add_SQLiteParameters_Pre(m_guid.ToArray(), "ea_guid") + ")";
            }
            else
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE Object_Type IN ('Class', 'Acitivity', 'Object', 'InformationItem') AND ea_guid IN (" + command.Add_Parameters_Pre(m_guid.ToArray()) + ")";

            }


            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            List<DB_Return> m_ret3 = new List<DB_Return>();

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
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    m_Name.Add("ea_guid");
                  
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }


            if (m_ret3[0].Ret.Count > 1)
            {
                m_Output = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_Output = null;
            }


            if (m_Output != null)
            {
                return (m_Output);
            }
            else
            {
                return (null);
            }


        }
        public List<string> Get_GUID_By_Stereotype_AND_GUID(Database database, List<string> m_guid, string stereotype)
        {
            string Output_Name = "ea_guid";
            string TABLE = "t_object";
            DB_Command command = new DB_Command();

            List<string> m_Output = new List<string>();
            List<DB_Input[]> ee = new List<DB_Input[]>();
            ee.Add(m_guid.Select(x => new DB_Input(-1, x)).ToArray());
            string[] m_output = { Output_Name };

             

            string SQL = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE Stereotype = '" + stereotype + "' AND ea_guid IN (" + command.Add_SQLiteParameters_Pre(m_guid.ToArray(), "ea_guid") + ")";
            }
            else
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE Stereotype = '" + stereotype + "' AND ea_guid IN (" + command.Add_Parameters_Pre(m_guid.ToArray()) + ")";
            }

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text};


            List<DB_Return> m_ret3 = new List<DB_Return>();

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
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    m_Name.Add("ea_guid");
                    
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    //SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT3.CommandText + "));";
                    m_ret3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }


            if (m_ret3[0].Ret.Count > 1)
            {
                m_Output = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_Output = null;
            }


            if (m_Output != null)
            {
                return (m_Output);
            }
            else
            {
                return (null);
            }


        }
        public List<string> Get_By_NOT_PackageID(Database database, List<string> m_guid, List<int> m_package_id)
        {
            string Output_Name = "ea_guid";
            string TABLE = "t_object";
            DB_Command command = new DB_Command();

            List<string> m_Output = new List<string>();
            List<DB_Input[]> ee = new List<DB_Input[]>();
            ee.Add(m_package_id.Select(x => new DB_Input(x, -1)).ToArray());
            ee.Add(m_guid.Select(x => new DB_Input(-1, x)).ToArray());
            string[] m_output = { Output_Name };

            //string SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE NOT Package_ID IN (" + command.Add_Parameters_Pre_Integer(m_package_id.ToArray()) + ")";



            string  SQL = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE ea_guid IN (" + command.Add_SQLiteParameters_Pre(m_guid.ToArray(), "ea_guid") + ") AND ea_guid IN (SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE NOT Package_ID IN (" + command.Add_SQLiteParameters_Pre_Integer(m_package_id.ToArray(), "Package_ID") + "))";
            }
            else
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE ea_guid IN (" + command.Add_Parameters_Pre(m_guid.ToArray()) + ") AND ea_guid IN (SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE NOT Package_ID IN (" + command.Add_Parameters_Pre_Integer(m_package_id.ToArray()) + "))";

            }

            OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.BigInt, OdbcType .VarChar};
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer, SqliteType.Text };


            List<DB_Return> m_ret3 = new List<DB_Return>();

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
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    
                    m_Name.Add("Package_ID");
                    m_Name.Add("ea_guid");

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    //SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT3.CommandText + "));";
                    m_ret3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }


            if (m_ret3[0].Ret.Count > 1)
            {
                m_Output = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_Output = null;
            }


            if (m_Output != null)
            {
                return (m_Output);
            }
            else
            {
                return (null);
            }


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
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };

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

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_Type");
                            m_Name.Add("Stereotype");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                            SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Diagram_ID = @Diagram_ID_1_0 AND Object_ID IN (" + SELECT3.CommandText + "));";
                            SELECT3.Parameters.AddWithValue ("@Diagram_ID_1_0", Diagram_ID).SqliteType = SqliteType.Integer;
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

                        List<string> m_Name = new List<string>();
                        m_Name.Add("Object_Type");
                        m_Name.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT_Instanz3, ee, m_input_Type_sqlite, m_Name);
                        SELECT_Instanz3.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN (" + SELECT_Instanz3.CommandText + ");";
                        //   SELECT3.Parameters.AddWithValue("@Diagram_ID_1_0", Diagram_ID).SqliteType = SqliteType.Integer;
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

        public List<string> Get_DiagramElements_GUID_wo_Stereotype(Database database, List<string> m_Type, int Diagram_ID)
        {
            //Allgemeine Variablen
            DB_Command command = new DB_Command();
            List<string> m_GUID = new List<string>();
            List<string> m_GUID2 = new List<string>();
            List<string> m_GUID3 = new List<string>();

            List<DB_Input[]> ee = new List<DB_Input[]>();
            DB_Input[] help = new DB_Input[m_Type.Count];
            ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
            //ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
            //ee.Add(m_Stereotype.Select(x => new DB_Input(Diagram_ID, null)).ToArray());

            string[] m_output = { "Object_ID" };
            string[] m_output2 = { "ea_guid" };
            string[] m_output3 = { "PDATA1" };
            string[] m_output4 = { "Classifier_guid" };
            string table = "t_object";
            string[] m_input_Property = { "Object_Type" };


            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar};
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar};
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text};

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
                        SELECT.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Diagram_ID = " + Diagram_ID + " AND Object_ID IN (" + SELECT.CommandText + "));";
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

                        List<string> m_Name = new List<string>();
                        m_Name.Add("Object_Type");
                       // m_Name.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
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
                // string select_instanz = command.Get_Select_Command(table, m_output2, m_input_Property, ee);
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

                        List<string> m_Name = new List<string>();
                        m_Name.Add("Object_Type");
                       // m_Name.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT_Instanz3, ee, m_input_Type_sqlite, m_Name);
                        SELECT_Instanz3.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz3.CommandText + ");";
                        //   SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Diagram_ID = @Diagram_ID_1_0 AND Object_ID IN (" + SELECT3.CommandText + "));";
                        //   SELECT3.Parameters.AddWithValue("@Diagram_ID_1_0", Diagram_ID).SqliteType = SqliteType.Integer;
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

                #region Instanz 2
                List<DB_Return> m_ret_Instanz2 = new List<DB_Return>();
               // string select_instanz2 = command.Get_Select_Command(table, m_output2, m_input_Property, ee);

                string select_instanz2 = "";

                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select_instanz2 = command.Get_Select_Command_SQLITE(table, m_output2, m_input_Property, ee);
                }
                else
                {
                    select_instanz2 = command.Get_Select_Command(table, m_output2, m_input_Property, ee);
                }

                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT_Instanz3 = new OleDbCommand(select_instanz2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT_Instanz3, ee, m_input_Type_oledb);
                        SELECT_Instanz3.CommandText = "SELECT Classifier_guid FROM t_object WHERE Classifier_guid IN(" + SELECT_Instanz3.CommandText + ");";
                        m_ret_Instanz2 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_Instanz3, m_output4);

                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT_Instanz4 = new OdbcCommand(select_instanz2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT_Instanz4, ee, m_input_Type_odbc);
                        SELECT_Instanz4.CommandText = "SELECT Classifier_guid FROM t_object WHERE Classifier_guid IN(" + SELECT_Instanz4.CommandText + ");";
                        m_ret_Instanz2 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT_Instanz4, m_output4);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT_Instanz5 = new SqliteCommand(select_instanz2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();
                        m_Name.Add("Object_Type");
                        // m_Name.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT_Instanz5, ee, m_input_Type_sqlite, m_Name);
                        SELECT_Instanz5.CommandText = "SELECT Classifier_guid FROM t_object WHERE Classifier_guid IN(" + SELECT_Instanz5.CommandText + ");";
                        //   SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Diagram_ID = @Diagram_ID_1_0 AND Object_ID IN (" + SELECT3.CommandText + "));";
                        //   SELECT3.Parameters.AddWithValue("@Diagram_ID_1_0", Diagram_ID).SqliteType = SqliteType.Integer;
                        m_ret_Instanz = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT_Instanz5, m_output4, sqliteTypes);
                        break;
                }

                if (m_ret_Instanz[0].Ret.Count > 1)
                {
                    m_GUID2 = (m_ret_Instanz2[0].Ret.GetRange(1, m_ret_Instanz2[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
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

                        m_GUID = m_GUID.Distinct().ToList();

                        return (m_GUID);
                    }

                }
            }



            return (null);
        }

        public List<string> Get_DiagramLinks_ConveyedItems(Database database, List<int> m_Diagram_ID)
        {
            string Output_Name = "Description";
            string TABLE = "t_xref";
            DB_Command command = new DB_Command();

            List<string> m_Output = new List<string>();
            List<DB_Input[]> ee = new List<DB_Input[]>();
            ee.Add(m_Diagram_ID.Select(x => new DB_Input(x, -1)).ToArray());
            string[] m_output = { Output_Name };

            string SQL = "";

            if(database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE Name = 'MOFProps' AND Type = 'connector property' AND Behavior = 'conveyed' AND Client IN (SELECT ea_guid FROM t_connector WHERE Connector_ID IN (SELECT ConnectorID FROM t_diagramlinks WHERE DiagramID IN(" + command.Add_SQLiteParameters_Pre_Integer(m_Diagram_ID.ToArray(), "DiagramID") + ")))";
            }
            else
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE Name = 'MOFProps' AND Type = 'connector property' AND Behavior = 'conveyed' AND Client IN (SELECT ea_guid FROM t_connector WHERE Connector_ID IN (SELECT ConnectorID FROM t_diagramlinks WHERE DiagramID IN(" + command.Add_Parameters_Pre_Integer(m_Diagram_ID.ToArray()) + ")))";
            }


            OleDbType[] m_input_Type_oledb = { OleDbType.BigInt };
            OdbcType[] m_input_Type_odbc = { OdbcType.BigInt };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer};


            List<DB_Return> m_ret3 = new List<DB_Return>();

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
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    m_Name.Add("DiagramID");
                   
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }


            if (m_ret3[0].Ret.Count > 1)
            {
                m_Output = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_Output = null;
            }


            if (m_Output != null)
            {
                return (m_Output);
            }
            else
            {
                return (null);
            }
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
                SQL12 = "SELECT ea_guid FROM t_object WHERE Package_ID = @Package_ID_1_0 AND Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype.ToArray(), "Stereotype") + ");";

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
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer, SqliteType.Text, SqliteType.Text };

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
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };

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
                        select_instanz = command.Get_Select_Command(table, m_output2, m_input_Property, ee);
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
                    //string select2 = command.Get_Select_Command(table, m_output2, m_input_Property, ee);

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
                           // SELECT4.CommandText = "SELECT Object_ID FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT3.CommandText + "));";
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

        public List<string> Get_Activity_GUID(Database database, List<string> m_Type_Activity, List<string> m_Stereotype_Activity, List<string> m_Type_Action, List<string> m_Stereotype_Action)
        {
       
            List<string> m_GUID = new List<string>();
            List<string> m_GUID2 = new List<string>();
            List<string> m_GUID3 = new List<string>();
            string SQL = "";
            string SQL2 = "";
            string SQL3 = "";
            string SQL_Dat = "";
            string SQL_Dat2 = "";
            string SQL_Dat3 = "";

            List<DB_Input[]> ee = new List<DB_Input[]>();
            ee.Add(m_Type_Activity.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype_Activity.Select(x => new DB_Input(-1, x)).ToArray());
            string[] m_output = { "Object_ID" };
            string[] m_output2 = { "ea_guid" };
            string table = "t_object";
            string[] m_input_Property = { "Object_Type", "Stereotype" };

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text };

            DB_Command command = new DB_Command();

            if (database.metamodel.flag_Analyse_Diagram == true) // muss in einem Diagram lieg
            {
                #region Diagram

                #region Activity
                //Activity direkt  
                List<DB_Return> m_ret = new List<DB_Return>();
                m_output2[0] = "ea_guid";

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
                        SELECT.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (" + SELECT.CommandText + ")";
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output2);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT2 = new OdbcCommand(select, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                        SELECT2.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (" + SELECT2.CommandText + ")";
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output2);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT3 = new SqliteCommand(select, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();
                        m_Name.Add("Object_Type");
                        m_Name.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                        SELECT3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (" + SELECT3.CommandText + "));";
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
                #endregion Activity

                //Action 
                #region Action
                List<DB_Input[]> ee2 = new List<DB_Input[]>();
                ee2.Add(m_Type_Action.Select(x => new DB_Input(-1, x)).ToArray());
                ee2.Add(m_Stereotype_Action.Select(x => new DB_Input(-1, x)).ToArray());

                List<DB_Return> m_ret_2 = new List<DB_Return>();

                m_output2[0] = "Classifier";

                string select_action = "";

                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select_action = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee2);
                }
                else
                {
                    select_action = command.Get_Select_Command(table, m_output, m_input_Property, ee2);
                }

                 

                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT_Action = new OleDbCommand(select_action, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT_Action, ee2, m_input_Type_oledb);
                        SELECT_Action.CommandText = "SELECT Classifier FROM t_object WHERE Classifier IN (" + SELECT_Action.CommandText + ")";
                         m_ret_2 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_Action, m_output2);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT_Action2 = new OdbcCommand(select_action, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT_Action2, ee2, m_input_Type_odbc);
                        SELECT_Action2.CommandText = "SELECT Classifier FROM t_object WHERE Classifier IN (" + SELECT_Action2.CommandText + ")";
                        m_ret_2 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT_Action2, m_output2);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT_Action3 = new SqliteCommand(select, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();
                        m_Name.Add("Object_Type");
                        m_Name.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Integer);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT_Action3, ee, m_input_Type_sqlite, m_Name);
                        SELECT_Action3.CommandText = "SELECT Classifier FROM t_object WHERE Classifier IN (" + SELECT_Action3.CommandText + "));";
                        m_ret_2 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT_Action3, m_output2, sqliteTypes);
                        break;
                }
               

                List<object> m_GUID4 = new List<object>();

                if (m_ret_2[0].Ret.Count > 1)
                {
                    m_GUID2 = (m_ret_2[0].Ret.GetRange(1, m_ret_2[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                    m_GUID4 = (m_ret_2[0].Ret.GetRange(1, m_ret_2[0].Ret.Count - 1).ToList().Select(x => x).ToList());
                }
                else
                {
                    m_GUID2 = (null);
                }

              
                if (m_GUID2 != null)
                {
                    ee.Clear();
                    ee.Add(m_GUID4.Select(x => new DB_Input(-1, x)).ToArray());
                    m_output[0] = "ea_guid";
                    table = "t_object";
                    string[] m_input_Property2 = { "Object_ID" };

                    OleDbType[] m_input_Type2_oledb = { OleDbType.BigInt };
                    OdbcType[] m_input_Type2_odbc = { OdbcType.Int };
                    SqliteType[] m_input_Type2_sqlite = {SqliteType.Integer};

                    List<DB_Return> m_ret_3 = new List<DB_Return>();

                    string select_action2 = "";

                    if(database.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        select_action2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property2, ee);
                    }
                    else
                    {
                        select_action2 = command.Get_Select_Command(table, m_output, m_input_Property2, ee);
                    }

                  

                    switch (database.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT_Action2 = new OleDbCommand(select_action2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                            database.oLEDB_Interface.Add_Parameters_Select(SELECT_Action2, ee, m_input_Type2_oledb);
                            SELECT_Action2.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (" + SELECT_Action2.CommandText + ")";
                             m_ret_3 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_Action2, m_output2);
                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT_Action3 = new OdbcCommand(select_action2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                            database.oDBC_Interface.Add_Parameters_Select(SELECT_Action3, ee, m_input_Type2_odbc);
                            SELECT_Action3.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (" + SELECT_Action3.CommandText + ")";
                            m_ret_3 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT_Action3, m_output2);
                            break;
                        case DB_Type.SQLITE:
                            SqliteCommand SELECT_Action4 = new SqliteCommand(select_action2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                            List<string> m_Name = new List<string>();
                            m_Name.Add("Object_ID");

                            List<SqliteType> sqliteTypes = new List<SqliteType>();
                            sqliteTypes.Add(SqliteType.Text);

                            database.SQLITE_Interface.Add_Parameters_Select(SELECT_Action4, ee, m_input_Type2_sqlite, m_Name);
                            SELECT_Action4.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (" + SELECT_Action4.CommandText + ")";
                            m_ret_3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT_Action4, m_output2, sqliteTypes);
                            break;
                    }
                  

                    if(m_ret_3 != null)
                    {
                        if (m_ret_3[0].Ret.Count > 1)
                        {
                            m_GUID3 = (m_ret_3[0].Ret.GetRange(1, m_ret_3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                        }
                        else
                        {
                            m_GUID3 = (null);
                        }

                        if (m_GUID != null && m_GUID3 != null)
                        {
                            m_GUID.AddRange(m_GUID3);
                            m_GUID = m_GUID.Distinct().ToList();

                        }

                        if (m_GUID == null)
                        {
                            m_GUID = m_GUID3;
                        }
                    }

                    

                }
                #endregion Action
                #endregion Diagramelemente
            }
            else // muss nicht in einem Diagram liegen --> gesamte Datenbank wird analysiert
            {
                #region Alle Elemente
                List<DB_Return> m_ret = new List<DB_Return>();
                m_output[0] = "ea_guid";

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
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT2 = new OdbcCommand(select, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT3 = new SqliteCommand(select, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();
                        m_Name.Add("Object_Type");
                        m_Name.Add("Stereotype");

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);

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
                #endregion Alle Elemente
            }

            return (m_GUID);
        }

        public List<string> Get_m_Attribut_By_m_Attribut(Database database, List<string> m_Input, string Input_Name, string Output_Name, string TABLE)
        {
            DB_Command command = new DB_Command();
            
            List<string> m_Output = new List<string>();
            List<DB_Input[]> ee = new List<DB_Input[]>();
            ee.Add(m_Input.Select(x => new DB_Input(-1, x)).ToArray());
            string[] m_output = { Output_Name };

            string SQL = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE " + Input_Name + " IN(" + command.Add_SQLiteParameters_Pre(m_Input.ToArray(), Input_Name) + ")";
            }
            else
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE " + Input_Name + " IN(" + command.Add_Parameters_Pre(m_Input.ToArray()) + ")";
            }



            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text};


            List<DB_Return> m_ret3 = new List<DB_Return>();

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
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    m_Name.Add(Input_Name);
                   
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                      m_ret3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }

           
            if (m_ret3[0].Ret.Count > 1)
            {
                m_Output = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_Output = null;
            }


            if (m_Output != null)
            {
                return (m_Output);
            }
            else
            {
                return (null);
            }
        }

        public List<string> Get_m_Attribut_By_m_Attribut_Integer(Database database, List<int> m_Input, string Input_Name, string Output_Name, string TABLE)
        {
            DB_Command command = new DB_Command();

            List<string> m_Output = new List<string>();
            List<DB_Input[]> ee = new List<DB_Input[]>();
            ee.Add(m_Input.Select(x => new DB_Input(x, -1)).ToArray());
            string[] m_output = { Output_Name };

            string SQL = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE " + Input_Name + " IN(" + command.Add_SQLiteParameters_Pre_Integer(m_Input.ToArray(), Input_Name) + ")";
            }
            else
            {
                SQL = "SELECT Distinct " + Output_Name + " FROM " + TABLE + " WHERE " + Input_Name + " IN(" + command.Add_Parameters_Pre_Integer(m_Input.ToArray()) + ")";
            }



            OleDbType[] m_input_Type_oledb = { OleDbType.BigInt };
            OdbcType[] m_input_Type_odbc = { OdbcType.BigInt };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Integer };


            List<DB_Return> m_ret3 = new List<DB_Return>();

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
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    m_Name.Add(Input_Name);

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    database.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes);
                    break;
            }


            if (m_ret3[0].Ret.Count > 1)
            {
                m_Output = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                m_Output = null;
            }


            if (m_Output != null)
            {
                return (m_Output);
            }
            else
            {
                return (null);
            }
        }
        public List<string> Check_Element(Database database, List<string> m_GUID, List<string> m_Type, List<string> m_Stereotype)
        {
          

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> m_input_Property_List = new List<string>();
            List<OleDbType> m_input_Type_List_oledb = new List<OleDbType>();
            List<OdbcType> m_input_Type_List_odbc = new List<OdbcType>();
            List<SqliteType> m_input_Type_List_sqlite = new List<SqliteType>();

            string[] m_output = { "ea_guid" };
            string table = "t_object";

            if (m_GUID != null)
            {
                m_input_Property_List.Add("ea_guid");
                ee.Add(m_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                m_input_Type_List_oledb.Add(OleDbType.VarChar);
                m_input_Type_List_odbc.Add(OdbcType.VarChar);
                m_input_Type_List_sqlite.Add(SqliteType.Text);
            }
            if (m_Type != null)
            {
                m_input_Property_List.Add("Object_Type");
                ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
                m_input_Type_List_oledb.Add(OleDbType.VarChar);
                m_input_Type_List_odbc.Add(OdbcType.VarChar);
                m_input_Type_List_sqlite.Add(SqliteType.Text);
            }
            if (m_Stereotype != null)
            {
                m_input_Property_List.Add("Stereotype");
                ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                m_input_Type_List_oledb.Add(OleDbType.VarChar);
                m_input_Type_List_odbc.Add(OdbcType.VarChar);
                m_input_Type_List_sqlite.Add(SqliteType.Text);
            }

            string[] m_input_Property = m_input_Property_List.ToArray();

            if (ee.Count > 0)
            {
                List<DB_Return> m_ret = new List<DB_Return>();

              //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);

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
                        OleDbType[] m_input_Type = m_input_Type_List_oledb.ToArray();
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcType[] m_input_Type_odbc = m_input_Type_List_odbc.ToArray();
                        OdbcCommand SELECT3 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT3, m_output);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                        List<string> m_Name = new List<string>();
                        m_Name = m_input_Property_List;


                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_List_sqlite.ToArray(), m_Name);
                       
                        m_ret = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT4, m_output, sqliteTypes);
                        break;
                }

              

                if (m_ret[0].Ret.Count > 1)
                {
                    return (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
            }

            return (null);
        }

        public List<string> Check_GUID(Database Data, string GUID, string _table, string _return)
        {
            List<string> _ret = new List<string>();
        //    XML xml = new XML();
            //SQL_Command command = new SQL_Command();
            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = {SqliteType.Text, SqliteType.Text};

            string table = _table;
            string[] m_output = { _return };

            List<DB_Return> m_ret3 = new List<DB_Return>();

            string ret = "";

            if(Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                ret = "SELECT " + _return + " FROM " + table + " WHERE " + _return + " = @"+_return+"_1_0";
            }
            else
            {
                ret = "SELECT " + _return + " FROM " + table + " WHERE " + _return + " = ?";
            }

           

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    oleDbCommand.Parameters.Add("?", OleDbType.VarChar).Value = GUID;
                    m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand oleDbCommand2 = new OdbcCommand(ret, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    oleDbCommand2.Parameters.Add("?", OdbcType.VarChar).Value = GUID;
                    m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(oleDbCommand2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand oleDbCommand3 = new SqliteCommand(ret, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                    oleDbCommand3.Parameters.AddWithValue("@" + _return + "_1_0", GUID).SqliteType = SqliteType.Text;
                    //oleDbCommand2.Parameters.Add("?", OdbcType.VarChar).Value = GUID;
                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);
                    m_ret3 = Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(oleDbCommand3, m_output, sqliteTypes);
                    break;
            }
          

            if (m_ret3[0].Ret.Count > 1)
            {
                _ret.Add(m_ret3[0].Ret[1].ToString());
                return (_ret);
            }
            else
            {
                return (null);
            }
        }


        #region Connection
        private bool Switch_Connection_State(Database database)
        {

            DB_Interface dB_Interface;


            switch(database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    dB_Interface = database.oLEDB_Interface;
                    break;
                case DB_Type.MSDASQL:
                    dB_Interface = database.oDBC_Interface;
                    break;
                default:
                    return (false);
            }

            switch (dB_Interface.dbConnection.State)
            {
                case System.Data.ConnectionState.Open:
                    dB_Interface.dbConnection.Close();
                    break;
                case System.Data.ConnectionState.Closed:
                    dB_Interface.dbConnection.Open();
                    break;
                default:
                    return (false);
            }

            return (true);
        }

        public bool Open_Connection(Database database)
        {
           DB_Interface dB_Interface;
            

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    dB_Interface = database.oLEDB_Interface;
                    break;
                case DB_Type.MSDASQL:
                    dB_Interface = database.oDBC_Interface;
                    break;
                case DB_Type.SQLITE:
                    dB_Interface = database.SQLITE_Interface;
                    break;
                default:
                    return (false);
            }

            if(dB_Interface.dbConnection == null)
            {
                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        database.oLEDB_Interface.OLEDB_Open();
                        break;
                    case DB_Type.MSDASQL:
                        database.oDBC_Interface.odbc_Open();
                        break;
                    case DB_Type.SQLITE:
                        database.SQLITE_Interface.SQLITE_Open();
                        break;
                    default:
                        return (false);
                }
            }

            switch (dB_Interface.dbConnection.State)
            {
                case System.Data.ConnectionState.Open:
                    return (true);
                    break;
                case System.Data.ConnectionState.Closed:
                    dB_Interface.dbConnection.Open();
                    break;
                default:
                    return (false);
            }

            return (true);
        }

        public bool Close_Connection(Database database)
        {
            DB_Interface dB_Interface;


            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    dB_Interface = database.oLEDB_Interface;
                    break;
                case DB_Type.MSDASQL:
                    dB_Interface = database.oDBC_Interface;
                    break;
                case DB_Type.SQLITE:
                    dB_Interface = database.SQLITE_Interface;
                    break;
                default:
                    return (false);
            }

            switch (dB_Interface.dbConnection.State)
            {
                case System.Data.ConnectionState.Open:

                    System.Threading.Thread.Sleep(1);

                   if( dB_Interface.dbConnection.State == System.Data.ConnectionState.Broken)
                    {
                        dB_Interface.dbConnection.Open();
                        dB_Interface.dbConnection.Dispose();
                        dB_Interface.dbConnection.Close();
                    }
                   else
                    {
                        
                            if (database.metamodel.dB_Type == DB_Type.SQLITE)
                            {
                            //System.Threading.Thread.Sleep(5);
                            //string test = database.SQLITE_Interface.dbConnection.DataSource;
                            database.SQLITE_Interface.SQLITE_Close();

                            //database.SQLITE_Interface.db_Close();
                            }
                            else
                            {

                             //   dB_Interface.dbConnection.Dispose();
                                dB_Interface.dbConnection.Close();
                            }
                       
  

                    }
                    break;
                case System.Data.ConnectionState.Closed:
                    return (true);
                    break;
                default:
                    return (false);
            }

            return (true);
        }
        #endregion Connection
    }
}

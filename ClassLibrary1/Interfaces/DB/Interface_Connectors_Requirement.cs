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

namespace Requirement_Plugin.Interfaces
{
    public class Interface_Connectors_Requirement
    {
        #region Get

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

       
        public List<string> Get_Supplier_Element_By_Connector(Database Data, List<string> m_Client_GUID, List<string> m_Type_Supplier, List<string> m_Stereotype_Supplier, List<string> m_Type_con, List<string> m_Stereotype_con, bool direction)
        {
     //       XML xml = new XML();
            DB_Command command = new DB_Command();
            List<string> GUIDS = new List<string>();
            List<DB_Return> m_ret3 = new List<DB_Return>();

            string SQL2 = "";

            if(Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                if(direction == true)
                {
                    SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Client_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "));";
                }
                else
                {
                    // SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Client_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "));";

                    SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Supplier.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Supplier.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Client_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "));";


                }

            }
            else
            {
                if(direction == true)
                {
                    SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Client_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "));";
                }
                else
                {
                    SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Supplier.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Supplier.ToArray()) + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Client_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "));";
                }

            }

        
            List<DB_Input[]> ee = new List<DB_Input[]>();

            

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = {SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

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
                    SqliteCommand SELECT3 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    ee.Add(m_Type_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Supplier.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Client_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());

                    m_Name.Add("Object_Type");
                    m_Name.Add("Stereotype");
                    m_Name.Add("ea_guid");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype2");

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
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

        public List<string> Get_Client_Element_By_Connector(Database Data, List<string> m_Supplier_GUID, List<string> m_Type_Client, List<string> m_Stereotype_Client, List<string> m_Type_con, List<string> m_Stereotype_con, bool direction)
        {
            List<string> GUIDS = new List<string>();

            if (m_Supplier_GUID.Count > 0)
            {
                List<string> m_Type_ADMBw = Data.metamodel.m_Requirement_ADMBw.Select(x => x.Type).ToList();
                List<string> m_Stereotype_ADMBw = Data.metamodel.m_Requirement_ADMBw.Select(x => x.Stereotype).ToList();

                DB_Command command = new DB_Command();
              
                List<DB_Return> m_ret3 = new List<DB_Return>();
                string SQL2 = "";


                var t = m_Stereotype_ADMBw.Where(x => x != "" && x != null).ToList();
                var tt = m_Stereotype_con.Where(x => x != "" && x != null).ToList();
                var ttt = m_Stereotype_Client.Where(x => x != "" && x != null).ToList();

                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<int> help_guid = new List<int>();

                List<string> m_Name = new List<string>();

                List<SqliteType> sqliteTypes1 = new List<SqliteType>();

                if (t.Count > 0)
                {
                    m_Stereotype_ADMBw = t;
                    if (tt.Count > 0)
                    {
                        if(Data.metamodel.dB_Type == DB_Type.SQLITE)
                        {
                            if(direction == true)
                            {
                                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property IN (" + command.Add_SQLiteParameters_Pre(Data.metamodel.Afo_Stereotype.ToArray(), "Property") + ") AND Value IN (" + command.Add_SQLiteParameters_Pre(ttt.ToArray(), "Value") + ") AND Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_ADMBw.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(t.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "))));";
                            }
                            else
                            {
                                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property IN (" + command.Add_SQLiteParameters_Pre(Data.metamodel.Afo_Stereotype.ToArray(), "Property") + ") AND Value IN (" + command.Add_SQLiteParameters_Pre(ttt.ToArray(), "Value") + ") AND Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_ADMBw.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(t.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "))));";
                            }


                            m_Name.Add("Property");
                            m_Name.Add("Value");
                            m_Name.Add("Object_Type");
                            m_Name.Add("Stereotype");
                            m_Name.Add("ea_guid");
                            m_Name.Add("Connector_Type");
                            m_Name.Add("Stereotype2");

                            ee.Add(Data.metamodel.Afo_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(ttt.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(m_Type_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(t.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(m_Supplier_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());

                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                        }
                        else
                        {
                            if(direction == true)
                            {
                                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property IN (" + command.Add_Parameters_Pre(Data.metamodel.Afo_Stereotype.ToArray()) + ") AND Value IN (" + command.Add_Parameters_Pre(ttt.ToArray()) + ") AND Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_ADMBw.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(t.ToArray()) + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "))));";
                            }
                            else
                            {
                                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_objectproperties WHERE Property IN (" + command.Add_Parameters_Pre(Data.metamodel.Afo_Stereotype.ToArray()) + ") AND Value IN (" + command.Add_Parameters_Pre(ttt.ToArray()) + ") AND Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_ADMBw.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(t.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "))));";
                            }


                        }

                    }
                    else
                    {
                        if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                        {
                            if(direction == true)
                            {
                                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(t.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + "));";

                            }
                            else
                            {
                                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(t.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + "));";

                            }

                            
                            m_Name.Add("Object_Type");
                            m_Name.Add("Stereotype");
                            m_Name.Add("ea_guid");
                            m_Name.Add("Connector_Type");

                            ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(t.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(m_Supplier_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                            ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());

                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                            sqliteTypes1.Add(SqliteType.Text);
                        }
                        else
                        {

                            if(direction == true)
                            {
                                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(t.ToArray()) + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + "));";

                            }
                            else
                            {
                                SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(t.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN(SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + "));";

                            }
                           
                         
                        }

                    }


                }
                else
                {
                    if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        if(direction == true)
                        {
                            SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "));";

                        }
                        else
                        {
                            SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Client.ToArray(), "Object_Type") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_SQLiteParameters_Pre(m_Supplier_GUID.ToArray(), "ea_guid") + ")) AND Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + "));";

                        }

                        m_Name.Add("Object_Type");
                        m_Name.Add("ea_guid");
                        m_Name.Add("Connector_Type");
                        m_Name.Add("Stereotype2");

                        ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Supplier_GUID.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());

                        sqliteTypes1.Add(SqliteType.Text);
                        sqliteTypes1.Add(SqliteType.Text);
                        sqliteTypes1.Add(SqliteType.Text);
                        sqliteTypes1.Add(SqliteType.Text);

                    }
                    else
                    {
                        if (direction == true)
                        {
                            SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "));";
                        }
                        else 
                        {
                            SQL2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Client.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid IN(" + command.Add_Parameters_Pre(m_Supplier_GUID.ToArray()) + ")) AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + "));";

                        }


                    }

                }
               



                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = sqliteTypes1.ToArray();

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
                        ee.Add(m_Type_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(t.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(Data.metamodel.Afo_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                        m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        ee.Add(Data.metamodel.Afo_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_Client.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Type_Client.Select(x => new DB_Input(-1, x)).ToArray());
                        if (m_Stereotype_ADMBw.Select(x => x != "").ToList().Count > 0)
                        {
                            ee.Add(m_Stereotype_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());
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
                        SqliteCommand SELECT3 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);


                       
                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);


                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                        m_ret3 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
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


      
        
        public List<string> Get_m_Supplier_By_ClientGUID_And_Connector(Database database, string Client_GUID, List<string> m_Type_Con, List<string> m_Stereotype_Con )
        {
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret5 = new List<DB_Return>();
            string SQL = "";

            if (m_Stereotype_Con != null)
            {
                if(database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "Stereotype") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0));";

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
                    SqliteCommand SELECT5 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<string> m_Name = new List<string>();
                    List<SqliteType> sqliteTypes1 = new List<SqliteType>();
                    m_Name.Add("Connector_Type");
                    sqliteTypes1.Add(SqliteType.Text);
                    ee3.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    if (m_Stereotype_Con != null)
                    {
                        m_Name.Add("Stereotype");
                        sqliteTypes1.Add(SqliteType.Text);
                        ee3.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    }
                    m_Name.Add("ea_guid");
                    sqliteTypes1.Add(SqliteType.Text);
                    ee3.Add(help_guid3.Select(x => new DB_Input(-1, x)).ToArray());

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    database.SQLITE_Interface.Add_Parameters_Select(SELECT5, ee3, sqliteTypes1.ToArray(), m_Name);
                    m_ret5 = (database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT5, m_output3, sqliteTypes));
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
                    m_ret3 = (Database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
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
       
        public List<string> Check_Connector_Requirement_Interface(Database Data, string Client_GUID, string Supplier_GUID ,List<string> m_Type_reqint, List<string> m_Stereotype_reqint, List<string> m_Type_DerivedElem, List<string> m_Stereotype_DerivedElem, List<string> m_Type_Con, List<string> m_Stereotype_Con)
        {
            List<string> m_Type_ADMBw = Data.metamodel.m_Requirement_ADMBw.Select(x => x.Type).ToList();
            List<string> m_Stereotype_ADMBw = Data.metamodel.m_Requirement_ADMBw.Select(x => x.Stereotype).ToList();

            DB_Command command = new DB_Command();

            List<DB_Return> m_ret3 = new List<DB_Return>();
            string SQL2 = "";


            var t = m_Stereotype_ADMBw.Where(x => x != "" && x != null).ToList();
            var tt = m_Stereotype_Con.Where(x => x != "" && x != null).ToList();
            var ttt = m_Stereotype_reqint.Where(x => x != "" && x != null).ToList();
            Repository_Element repository_Element = new Repository_Element();
            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<int> help_id = new List<int>();
            List<int> help_id2 = new List<int>();
            List<string> help_Property = new List<string>();
            repository_Element.Classifier_ID = Client_GUID;
            help_id.Add(repository_Element.Get_Object_ID(Data));
            repository_Element.Classifier_ID = Supplier_GUID;
            help_id2.Add(repository_Element.Get_Object_ID(Data));
            help_Property.Add(Data.metamodel.Afo_Stereotype[0]);


            List<string> m_Name = new List<string>();
            List<string> Connector_GUID = new List<string>();
            //Alle  Requirement_Interface vom Client
            string SQL_ges = "";

            if(Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                if (Data.metamodel.m_Derived_Element[0].direction == true)
                {
                    string SQL23 = "SELECT Object_ID FROM t_objectproperties WHERE Property = @Property1_1_0 AND Value IN (" + command.Add_SQLiteParameters_Pre(ttt.ToArray(), "Value1") + ")";
                    string SQL24 = "SELECT Object_ID FROM t_objectproperties WHERE Property = @Property2_1_0 AND Value IN (" + command.Add_SQLiteParameters_Pre(ttt.ToArray(), "Value2") + ")";

                    string SQL11 = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_ADMBw.ToArray(), "Stereotype11") + ") AND Object_ID IN (" + SQL23 + ") AND Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID = @End_Object_ID1_1_0 AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_DerivedElem.ToArray(), "Stereotype12") + "))";
                    //Alle Requirement_Interface vomn Supplier
                    string SQL21 = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_ADMBw.ToArray(), "Stereotype21") + ") AND Object_ID IN (" + SQL24 + ") AND Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID = @End_Object_ID2_1_0 AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_DerivedElem.ToArray(), "Stereotype22") + "))";

                    SQL_ges = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "StereotypeCon") + ") AND Start_Object_ID IN (" + SQL11 + ") AND End_Object_ID IN (" + SQL21 + ");";
                }
                else
                {
                    string SQL23 = "SELECT Object_ID FROM t_objectproperties WHERE Property = @Property1_1_0 AND Value IN (" + command.Add_SQLiteParameters_Pre(ttt.ToArray(), "Value1") + ")";
                    string SQL24 = "SELECT Object_ID FROM t_objectproperties WHERE Property = @Property2_1_0 AND Value IN (" + command.Add_SQLiteParameters_Pre(ttt.ToArray(), "Value2") + ")";

                    string SQL11 = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_ADMBw.ToArray(), "Stereotype11") + ") AND Object_ID IN (" + SQL23 + ") AND Object_ID IN(SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID = @End_Object_ID1_1_0 AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_DerivedElem.ToArray(), "Stereotype12") + "))";
                    //Alle Requirement_Interface vomn Supplier
                    string SQL21 = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_ADMBw.ToArray(), "Stereotype21") + ") AND Object_ID IN (" + SQL24 + ") AND Object_ID IN(SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID = @End_Object_ID2_1_0 AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_DerivedElem.ToArray(), "Stereotype22") + "))";

                    SQL_ges = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN(" + command.Add_SQLiteParameters_Pre(m_Type_Con.ToArray(), "Connector_Type") + ") AND Stereotype IN(" + command.Add_SQLiteParameters_Pre(m_Stereotype_Con.ToArray(), "StereotypeCon") + ") AND Start_Object_ID IN (" + SQL11 + ") AND End_Object_ID IN (" + SQL21 + ");";

                }


                m_Name.Add("Connector_Type");
                m_Name.Add("StereotypeCon");
                m_Name.Add("Stereotype11");
                m_Name.Add("Property1");
                m_Name.Add("Value1");
                m_Name.Add("End_Object_ID1");
                m_Name.Add("Stereotype12");
                m_Name.Add("Stereotype21");
                m_Name.Add("Property2");
                m_Name.Add("Value2");
                m_Name.Add("End_Object_ID2");
                m_Name.Add("Stereotype22");


                //SQL Ges
                ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                //SQL11
                ee.Add(m_Stereotype_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());
                //SQL23
                ee.Add(help_Property.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(ttt.Select(x => new DB_Input(-1, x)).ToArray());
                //SQL11
                ee.Add(help_id.Select(x => new DB_Input(x, null)).ToArray());
                ee.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());
                //SQL21
                ee.Add(m_Stereotype_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());
                //SQL24
                ee.Add(help_Property.Select(x => new DB_Input(-1, x)).ToArray());
                ee.Add(ttt.Select(x => new DB_Input(-1, x)).ToArray());
                //SQL21
                ee.Add(help_id2.Select(x => new DB_Input(x, null)).ToArray());
                ee.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());



             
            }
            else
            {
                if (Data.metamodel.m_Derived_Element[0].direction == true)
                {
                    string SQL23 = "SELECT Object_ID FROM t_objectproperties WHERE Property = ? AND Value IN (" + command.Add_Parameters_Pre(ttt.ToArray()) + ")";
                    string SQL24 = "SELECT Object_ID FROM t_objectproperties WHERE Property = ? AND Value IN (" + command.Add_Parameters_Pre(ttt.ToArray()) + ")";

                    string SQL11 = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_ADMBw.ToArray()) + ") AND Object_ID IN (" + SQL23 + ") AND Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID = ? AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_DerivedElem.ToArray()) + "))";
                    //Alle Requirement_Interface vomn Supplier
                    string SQL21 = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_ADMBw.ToArray()) + ") AND Object_ID IN (" + SQL24 + ") AND Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID = ? AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_DerivedElem.ToArray()) + "))";

                    SQL_ges = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Con.ToArray()) + ") AND Start_Object_ID IN (" + SQL11 + ") AND End_Object_ID IN (" + SQL21 + ");";

                }
                else
                {
                    string SQL23 = "SELECT Object_ID FROM t_objectproperties WHERE Property = ? AND Value IN (" + command.Add_Parameters_Pre(ttt.ToArray()) + ")";
                    string SQL24 = "SELECT Object_ID FROM t_objectproperties WHERE Property = ? AND Value IN (" + command.Add_Parameters_Pre(ttt.ToArray()) + ")";

                    string SQL11 = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_ADMBw.ToArray()) + ") AND Object_ID IN (" + SQL23 + ") AND Object_ID IN(SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID = ? AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_DerivedElem.ToArray()) + "))";
                    //Alle Requirement_Interface vomn Supplier
                    string SQL21 = "SELECT Object_ID FROM t_object WHERE Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_ADMBw.ToArray()) + ") AND Object_ID IN (" + SQL24 + ") AND Object_ID IN(SELECT End_Object_ID FROM t_connector WHERE Start_Object_ID = ? AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_DerivedElem.ToArray()) + "))";

                    SQL_ges = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN(" + command.Add_Parameters_Pre(m_Type_Con.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_Con.ToArray()) + ") AND Start_Object_ID IN (" + SQL11 + ") AND End_Object_ID IN (" + SQL21 + ");";

                }


            }


            //string SQL_ges = "SELECT Object_ID FROM t_object WHERE Object_ID IN ("+SQL11+") AND Object_ID IN ("+ SQL11+");";

           

            // OleDbType[] m_input_Type_oledb = { OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };

            //OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };

            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int, OdbcType.VarChar };

            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Integer, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Integer, SqliteType.Text };


            string[] m_output = { "ea_guid" };
            //string[] m_output = { "Object_ID" };

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL_ges, (OleDbConnection)Data.oLEDB_Interface.dbConnection);

                    ee.Add(help_Property.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_reqint.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_id.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());

                    ee.Add(help_Property.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_reqint.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_id2.Select(x => new DB_Input(x, null)).ToArray());
                    ee.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());

                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                  
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                    m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL_ges, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    ee.Add(m_Type_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_Con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_ADMBw.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_Property.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_reqint.Select(x => new DB_Input(-1, x)).ToArray());   
                    help_id.Add(repository_Element.Get_Object_ID(Data));
                    ee.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help_Property.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_reqint.Select(x => new DB_Input(-1, x)).ToArray());
                    help_id2.Add(repository_Element.Get_Object_ID(Data));
                    ee.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());

                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL_ges, (SqliteConnection)Data.SQLITE_Interface.dbConnection);



                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);


                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                    break;
            }
            //Requires Connector
            if (m_ret3[0].Ret.Count > 1)
            {
                Connector_GUID = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                Connector_GUID = null;
            }

            return (Connector_GUID);
        }
        #endregion Check

        #region Create
       
        #endregion Create

        #region Delete
        
       
        #endregion Delete

        #region Update

         #endregion Update
    }
}

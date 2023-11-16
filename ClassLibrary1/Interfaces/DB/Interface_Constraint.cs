using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;

using Ennumerationen;
using Database_Connection;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

namespace Requirement_Plugin.Interfaces
{
    public class Interface_Constraint
    {
        /// <summary>
        /// Zum erhalten von 
        ///     DesignConstraint
        ///     ProcessConstraint
        ///     EnvironmentConstraint
        /// </summary>
        /// <param name="w_Constraint_Type"></param>
        /// <param name="Data"></param>
        /// <param name="ID"></param>
        /// <param name="Classifier_ID"></param>
        /// <param name="m_Type_elem"></param>
        /// <param name="m_Stereotype_elem"></param>
        /// <param name="m_Type_con"></param>
        /// <param name="m_Stereotype_con"></param>
        /// <param name="m_Stereotype_act_def"></param>
        /// <param name="m_Stereotype_act_usage"></param>
        /// <returns></returns>
        public List<string> Get_Constraint(W_Constraint_Type w_Constraint_Type, Database Data, int ID, string Classifier_ID, List<string> m_Type_elem, List<string> m_Stereotype_elem, List<string> m_Type_con, List<string> m_Stereotype_con, List<string> m_Stereotype_act_def, List<string> m_Stereotype_act_usage)
        {

            List<string> m_GUID_ret = new List<string>();

            //Entscheidung oleDB

            DB_Command command = new DB_Command();
            List<DB_Return> m_ret3 = new List<DB_Return>();
            //////////////////////////////////////////7
            //OperationConstraint erhalten
            string SQL = "";

            List<string> m_Name = new List<string>();

            if (Data.metamodel.dB_Type == DB_Type.SQLITE)
            {
                switch (w_Constraint_Type)
                {
                    case W_Constraint_Type.Process:
                        SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_elem.ToArray(), "Object_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_elem.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype1") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0));"; // OR ea_guid IN (SELECT ea_guid FROM t_object WHERE Classifier = ?)))";
                        break;
                    case W_Constraint_Type.Design:
                        SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_elem.ToArray(), "Object_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_elem.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype1") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0));";
                        break;
                    case W_Constraint_Type.Umwelt:
                        SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_elem.ToArray(), "Object_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_elem.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype1") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0));";
                        break;
                    default:
                        SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_elem.ToArray(), "Object_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_elem.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype1") + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0));";
                        break;
                }
                m_Name.Add("Object_Type");
                m_Name.Add("Stereotype");
                m_Name.Add("Connector_Type");
                m_Name.Add("Stereotype1");
                m_Name.Add("ea_guid");


            }
            else
            {
                switch (w_Constraint_Type)
                {
                    case W_Constraint_Type.Process:
                        SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_Parameters_Pre(m_Type_elem.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_elem.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?));"; // OR ea_guid IN (SELECT ea_guid FROM t_object WHERE Classifier = ?)))";
                        break;
                    case W_Constraint_Type.Design:
                        SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_Parameters_Pre(m_Type_elem.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_elem.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?));";
                        break;
                    case W_Constraint_Type.Umwelt:
                        SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_Parameters_Pre(m_Type_elem.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_elem.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?));";
                        break;
                    default:
                        SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_Parameters_Pre(m_Type_elem.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_elem.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?));";
                        break;
                }
            }

              


          

            List<DB_Input[]> ee = new List<DB_Input[]>();

             Classifier_ID = Classifier_ID;
            DB_Input[] help2 = { new DB_Input(-1, Classifier_ID) };

            

           
            OleDbType[] m_input_Type_oledb = new OleDbType[6];
            m_input_Type_oledb[0] = OleDbType.VarChar;
            m_input_Type_oledb[1] = OleDbType.VarChar;
            m_input_Type_oledb[2] = OleDbType.VarChar;
            m_input_Type_oledb[3] = OleDbType.VarChar;
            m_input_Type_oledb[4] = OleDbType.VarChar;
            m_input_Type_oledb[5] = OleDbType.VarChar;

            OdbcType[] m_input_Type_odbc = new OdbcType[6];
            m_input_Type_odbc[0] = OdbcType.VarChar;
            m_input_Type_odbc[1] = OdbcType.VarChar;
            m_input_Type_odbc[2] = OdbcType.VarChar;
            m_input_Type_odbc[3] = OdbcType.VarChar;
            m_input_Type_odbc[4] = OdbcType.VarChar;
            m_input_Type_odbc[5] = OdbcType.VarChar;

            SqliteType[] m_input_Type_sqlite = {SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text, SqliteType.Text };

       /*     if (W_Constraint_Type.Process == w_Constraint_Type)
            {
                m_input_Type_oledb[0] = OleDbType.BigInt;
                m_input_Type_odbc[5] = OdbcType.Int;
            }
            else
            {
                m_input_Type_oledb[0] = OleDbType.VarChar;
                m_input_Type_odbc[5] = OdbcType.VarChar;
            }*/


            string[] m_output = { "ea_guid" };
         

            switch (Data.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                   /* if (w_Constraint_Type == W_Constraint_Type.Process && Data.metamodel.dB_Type == DB_Type.ACCDB)
                    {
                        DB_Input[] help = { new DB_Input(ID, null) };
                        ee.Add(help);
                    }*/

                    ee.Add(help2);
                    ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                     m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help2);
                 /*   if (w_Constraint_Type == W_Constraint_Type.Process && Data.metamodel.dB_Type == DB_Type.MSDASQL)
                    {
                        DB_Input[] help = { new DB_Input(ID, null) };
                        ee.Add(help);
                    }*/

                    Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                    m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)Data.SQLITE_Interface.dbConnection);



                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Text);

                    ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                    ee.Add(help2);

                    Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                    m_ret3 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                    break;
            }

            List<string> m_GUID = new List<string>();
            m_GUID = null;

            if (m_ret3[0].Ret.Count > 1)
            {
                m_GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                m_GUID = (null);
            }
            ////////////////////////////////////////////////
            //Prüfen, ob OperationalConstraint Vorgaben Prozessanforderung entsprSicht --> nur an Element_Aktivity bzw Element_Action
            if (m_GUID != null)
            {
                int i1 = 0;
                do
                {
                    //Alle Start Stereotypen erhalten welche mit der OpConstraint mit Hilfe Stisfy verbunden sind

                    string SQL2 = "";

                    if(Data.metamodel.dB_Type == DB_Type.SQLITE)
                    {
                        SQL2 = "SELECT Stereotype FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype") + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0))";


                    }
                    else
                    {
                        SQL2 = "SELECT Stereotype FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?))";


                    }

                   

                    List<DB_Input[]> ee2 = new List<DB_Input[]>();
                    List<string> help_guid = new List<string>();
                    List<DB_Return> m_ret4 = new List<DB_Return>();
                    help_guid.Add(m_GUID[i1]);

                  

                    OleDbType[] m_input_Type2_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                    OdbcType[] m_input_Type2_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
                    SqliteType[] m_input_Type_sqlite2 = {SqliteType.Text, SqliteType.Text, SqliteType.Text};

                    string[] m_output2 = { "Stereotype" };

                    

                    switch (Data.metamodel.dB_Type)
                    {
                        case DB_Type.ACCDB:
                            OleDbCommand SELECT3 = new OleDbCommand(SQL2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                            ee2.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                            ee2.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                            ee2.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                            Data.oLEDB_Interface.Add_Parameters_Select(SELECT3, ee2, m_input_Type2_oledb);
                            m_ret4 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT3, m_output2);
                            break;
                        case DB_Type.MSDASQL:
                            OdbcCommand SELECT4 = new OdbcCommand(SQL2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                            ee2.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                            ee2.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                            ee2.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
                            Data.oDBC_Interface.Add_Parameters_Select(SELECT4, ee2, m_input_Type2_odbc);
                            m_ret4 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT4, m_output2);
                            break;
                       case DB_Type.SQLITE:
                            SqliteCommand SELECT5 = new SqliteCommand(SQL2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);



                            List<SqliteType> sqliteTypes2 = new List<SqliteType>();
                            sqliteTypes2.Add(SqliteType.Text);
                            ee2.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                            ee2.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                            ee2.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());

                            List<string> m_Name2 = new List<string>();
                            m_Name2.Add("Connector_Type");
                            m_Name2.Add("Stereotype");
                            m_Name2.Add("ea_guid");


                            Data.SQLITE_Interface.Add_Parameters_Select(SELECT5, ee2, m_input_Type_sqlite2, m_Name2);
                            m_ret4 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT5, m_output2, sqliteTypes2));
                            break;
                    }

                    List<string> m_Stereotype = new List<string>();

                    if (m_ret4[0].Ret.Count > 1)
                    {
                        m_Stereotype = (m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                    }
                    else
                    {
                        m_Stereotype = (null);
                    }

                    if (m_Stereotype != null)
                    {
                        int i2 = 0;
                        do
                        {
                            if (m_Stereotype_act_def.Contains(m_Stereotype[0]) || m_Stereotype_act_usage.Contains(m_Stereotype[0]))
                            {
                               if(m_GUID_ret.Contains(m_GUID[i1]) == false)
                                {
                                    m_GUID_ret.Add(m_GUID[i1]);
                                }

                            }

                            i2++;
                        } while (i2 < m_Stereotype.Count);
                    }

                    i1++;
                } while (i1 < m_GUID.Count);
            }

            return (m_GUID_ret);
        }
        /// <summary>
        /// Zum erhalten von 
        ///     DesignConstraint
        ///     ProcessConstraint
        ///     EnvironmentConstraint
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ID"></param>
        /// <param name="Classifier_ID"></param>
        /// <param name="m_Type_elem"></param>
        /// <param name="m_Stereotype_elem"></param>
        /// <param name="m_Type_con"></param>
        /// <param name="m_Stereotype_con"></param>
        /// <param name="m_Stereotype_act_def"></param>
        /// <param name="m_Stereotype_act_usage"></param>
        /// <returns></returns>
        public List<string> Get_Typvertreter( Database Data, int ID, string Classifier_ID, List<string> m_Type_elem, List<string> m_Stereotype_elem, List<string> m_Type_con, List<string> m_Stereotype_con)
        {

            if(m_Type_elem.Count > 0 )
            {
                List<string> m_GUID_ret = new List<string>();

                //Entscheidung oleDB

                DB_Command command = new DB_Command();
                List<DB_Return> m_ret3 = new List<DB_Return>();
                //////////////////////////////////////////7
                //Typvertreter erhalten
                List<string> m_Name = new List<string>();

                string SQL = "";

                if (Data.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_elem.ToArray(), "Object_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_elem.ToArray(), "Stereotype") + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_SQLiteParameters_Pre(m_Type_con.ToArray(), "Connector_Type") + ") AND Stereotype IN (" + command.Add_SQLiteParameters_Pre(m_Stereotype_con.ToArray(), "Stereotype2") + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = @ea_guid_1_0))";

                    m_Name.Add("Object_Type");
                    m_Name.Add("Stereotype");
                    m_Name.Add("Connector_Type");
                    m_Name.Add("Stereotype2");
                    m_Name.Add("ea_guid");

                }
                else
                {
                    SQL = "SELECT ea_guid FROM t_object WHERE Object_Type IN (" + command.Add_Parameters_Pre(m_Type_elem.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_elem.ToArray()) + ") AND Object_ID IN (SELECT Start_Object_ID FROM t_connector WHERE Connector_Type IN (" + command.Add_Parameters_Pre(m_Type_con.ToArray()) + ") AND Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_con.ToArray()) + ") AND End_Object_ID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?))";

                }




                List<DB_Input[]> ee = new List<DB_Input[]>();


                DB_Input[] help2 = { new DB_Input(-1, Classifier_ID) };




                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar, OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = {SqliteType.Text, SqliteType.Text , SqliteType.Text , SqliteType.Text , SqliteType.Text };


                string[] m_output = { "ea_guid" };


                switch (Data.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT1 = new OleDbCommand(SQL, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        ee.Add(help2);
                        ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type_oledb);
                        m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                        ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help2);
                        Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                        m_ret3 = Data.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.SQLITE:
                        SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)Data.SQLITE_Interface.dbConnection);



                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        ee.Add(m_Type_elem.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_elem.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Type_con.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(m_Stereotype_con.Select(x => new DB_Input(-1, x)).ToArray());
                        ee.Add(help2);

                        Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_Name);
                        m_ret3 = (Data.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output, sqliteTypes));
                        break;
                }


                List<string> m_GUID = new List<string>();
                m_GUID = null;

                if (m_ret3[0].Ret.Count > 1)
                {
                    m_GUID = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    m_GUID = (null);
                }
                ////////////////////////////////////////////////



                return (m_GUID);
            }
            else
            {
                return (null);
            }
           
        }
    }
}
   

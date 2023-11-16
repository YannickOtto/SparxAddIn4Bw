using Database_Connection;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Requirement_Plugin;

namespace Requirement_Plugin.Interfaces
{
    public class Interface_XML
    {
        #region SQL-Query
        /// <summary>
        /// Es wird ein SELECT SQL Befehl ausgeführt
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Set_Attribut"></param>
        /// <param name="Set_Attribut_Value"></param>
        /// <param name="Get_Attribut"></param>
        /// <param name="Database"></param>
        /// <returns></returns>
        public List<string> SQL_Query_Select(string Set_Attribut, string Set_Attribut_Value, string Get_Attribut, string Database, Database database)
        {
            DB_Interface help_db = new DB_Interface();
            Requirement_Plugin.xml.XML xml = new Requirement_Plugin.xml.XML();

            List<string> help = new List<string>();
            help.Add(Set_Attribut_Value);

            List<DB_Input[]> ee = new List<DB_Input[]>();

            ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());

            string[] m_output = { Get_Attribut };
            string table = Database;
            string[] m_input_Property = { Set_Attribut };
            OleDbType[] m_input_Type = { OleDbType.VarChar };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

            List<DB_Return> m_ret = new List<DB_Return>();

            Interfaces.Interface_XML interface_XML = new Interfaces.Interface_XML();

            m_ret = interface_XML.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, m_input_Type_odbc, m_input_Type_sqlite, database);




            if (m_ret[0].Ret.Count > 1)
            {
                return (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }

            return (null);


        }

        #endregion SQL Query

        #region OleDB Command
        public List<DB_Return> Read_Attribut(string[] m_output, string table, string[] m_input_Property, List<DB_Input[]> m_input_Value, OleDbType[] m_input_Type_oledb, OdbcType[] m_input_Type_odbc, SqliteType[] m_input_Type_sqlite, Database Data)
        {
            DB_Command command = new DB_Command();
            //SELECT
            List<DB_Input[]> ee = m_input_Value;


            if (Data.metamodel.dB_Type == Ennumerationen.DB_Type.ACCDB)
            {
                string select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);

                return (m_ret3);
            }
            if (Data.metamodel.dB_Type == Ennumerationen.DB_Type.MSDASQL)
            {
                m_output = Data.oDBC_Interface.change_output(m_output);
                string select2 = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                OdbcCommand SELECT2 = new OdbcCommand(select2, (OdbcConnection)Data.oDBC_Interface.dbConnection);
                Data.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_odbc);
                List<DB_Return> m_ret3 = Data.oDBC_Interface.odbc_SELECT_One_Table(SELECT2, m_output);

                return (m_ret3);
            }
            if (Data.metamodel.dB_Type == Ennumerationen.DB_Type.SQLITE)
            {
                string select2 = command.Get_Select_Command_SQLITE(table, m_output, m_input_Property, ee);
                SqliteCommand SELECT3 = new SqliteCommand(select2, (SqliteConnection)Data.SQLITE_Interface.dbConnection);
                Data.SQLITE_Interface.Add_Parameters_Select(SELECT3, ee, m_input_Type_sqlite, m_input_Property.ToList());

                List<SqliteType> sqliteTypes = new List<SqliteType>();
                sqliteTypes.Add(SqliteType.Text);

                List<DB_Return> m_ret3 = Data.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT3, m_output, sqliteTypes);

                return (m_ret3);
            }

            return (null);


        }

        #endregion OleDB Command

    }
}

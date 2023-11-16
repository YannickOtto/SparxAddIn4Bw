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

namespace Requirement_Plugin.Interfaces
{
    public class Interface_Glossar
    {
        public List<string> Get_Glosar_ID(Database database)
        {
          //  XML xML = new XML();
            List<string> IDS = new List<string>();
            List<DB_Return> m_ret4 = new List<DB_Return>();
            string SQL = "SELECT Distinct GlossaryID FROM t_glossary";
        
           
       
            string[] m_output2 = { "GlossaryID" };

           

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    m_ret4 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output2);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT2 = new OdbcCommand(SQL, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    m_ret4 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output2);
                    break;
                case DB_Type.SQLITE:
                    SqliteCommand SELECT3 = new SqliteCommand(SQL, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                    List<SqliteType> sqliteTypes = new List<SqliteType>();
                    sqliteTypes.Add(SqliteType.Integer);

                    m_ret4 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output2, sqliteTypes);
                    break;
            }

            if (m_ret4[0].Ret.Count > 1)
            {
                IDS = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                IDS = null;
            }

            return (IDS);
        }

        public string Get_Glossar_Property(string Property, string ID, Database database)
        {
            //XML xML = new XML();
            // string ret = "";
            DB_Command command = new DB_Command();
            List<DB_Return> m_ret4 = new List<DB_Return>();

            if (Property == "Term" || Property == "Type" || Property == "Meaning")
            {
               
               
             
               
                string[] m_output2 = { Property };
              

                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        string SQL = "SELECT " + Property + " FROM t_glossary WHERE GlossaryID = " + ID + ";";
                        OleDbCommand SELECT = new OleDbCommand(SQL, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        m_ret4 = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output2);
                        break;
                    case DB_Type.MSDASQL:
                        string SQL2 = "SELECT " + Property + " FROM t_glossary WHERE GlossaryID = " + ID + ";";
                        OdbcCommand SELECT2 = new OdbcCommand(SQL2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        m_ret4 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output2);
                        break;
                    case DB_Type.SQLITE:
                        string SQL3 = "SELECT " + Property + " FROM t_glossary WHERE GlossaryID = @ID;";
                        SqliteCommand SELECT3 = new SqliteCommand(SQL3, (SqliteConnection)database.SQLITE_Interface.dbConnection);

                        SELECT3.Parameters.AddWithValue("@ID", ID).SqliteType = SqliteType.Integer;

                        List<SqliteType> sqliteTypes = new List<SqliteType>();
                        sqliteTypes.Add(SqliteType.Text);

                        m_ret4 = database.SQLITE_Interface.DB_SELECT_One_Table_Sqlite(SELECT3, m_output2, sqliteTypes);
                        break;
                }

                if (m_ret4[0].Ret.Count > 1)
                {
                    return ( m_ret4[0].Ret[1].ToString());
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
        

    }
}

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
    public class Interface_Package
    {
        public int Get_One_Attribut_Integer(string GUID, Database database, string Attribut)
        {
            if (GUID != null)
            {
              //  XML xml = new XML();
                List<DB_Return> m_ret = new List<DB_Return>();
                List<string> help = new List<string>();
                help.Add(GUID);

                List<DB_Input[]> ee = new List<DB_Input[]>();
                ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                string[] m_output = { Attribut };
                string[] m_input_Property = { "ea_guid" };
                string table = "t_Package";

                OleDbType[] m_input_Type_oledb = { OleDbType.VarChar };
                OdbcType[] m_input_Type_odbc = { OdbcType.VarChar };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Text };

                //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);

                DB_Command command = new DB_Command();
                //SELECT

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
                        OdbcCommand SELECT = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("ea_guid");


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

        public string Get_One_Attribut_String_by_ID(int ID, Database database, string Attribut)
        {
            if (ID != null)
            {
          //      XML xml = new XML();
                List<DB_Return> m_ret = new List<DB_Return>();
                List<DB_Input[]> ee = new List<DB_Input[]>();
                List<int> help_ID = new List<int>();
                help_ID.Add(ID);
                ee.Add(help_ID.Select(x => new DB_Input(x, null)).ToArray());

                string[] m_output = { Attribut };
                string table = "t_Package";
                string[] m_input_Property = { "Package_ID" };
                OleDbType[] m_input_Type_oledb = { OleDbType.BigInt };
                OdbcType[] m_input_Type_odbc = { OdbcType.Int };
                SqliteType[] m_input_Type_sqlite = { SqliteType.Integer };

                //  List<DB_Return> m_ret = xml.Read_Attribut(m_output, table, m_input_Property, ee, m_input_Type, database);
                DB_Command command = new DB_Command();
                //SELECT
                // List<DB_Input[]> ee = m_input_Value;
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
                        OdbcCommand SELECT = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Package_ID");


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

        public string Create_Package_Model(Database database, EA.Repository Repository, string Package_Name)
        {
       //     XML Package_GUID = new XML();
            List<DB_Return> m_ret = new List<DB_Return>();

            string[] m_output = { "ea_guid" };
            string[] m_input_Property = { "Name", "Object_Type", "ParentID" };
           
            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_Name = new List<string>();
            help_Name.Add(Package_Name);
            ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
            List<string> help_Type = new List<string>();
            help_Type.Add("Package");
            ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
            List<int> help_ID = new List<int>();
            help_ID.Add(0);
            ee.Add(help_ID.Select(x => new DB_Input(x, null)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Integer };
           
            DB_Command command = new DB_Command();
            //SELECT
            string select2 = "";

            if (database.metamodel.dB_Type == DB_Type.SQLITE)
            {
                select2 = command.Get_Select_Command_SQLITE("t_object", m_output, m_input_Property, ee);
            }
            else
            {
                select2 = command.Get_Select_Command("t_object", m_output, m_input_Property, ee);
            }
            
          
            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                    database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                    m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                    break;
                case DB_Type.MSDASQL:
                    OdbcCommand SELECT = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                    database.oDBC_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_odbc);
                    m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT, m_output);
                    break;
                case DB_Type.SQLITE:
                    List<string> m_str = new List<string>();
                    List<SqliteType> sqliteTypes = new List<SqliteType>();

                    m_str.Add("Name");
                    m_str.Add("Object_Type");
                    m_str.Add("ParentID");


                    sqliteTypes.Add(SqliteType.Text);

                    SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                    database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                    m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                    break;
            }

            string help = "";
            if (m_ret[0].Ret.Count > 1)
            {
                help = (m_ret[0].Ret[1].ToString());
            }
            else
            {
                help = null;
            }

         
            if (help == null)
            {

                EA.Package Model = Repository.Models.GetAt(0);

                EA.Package Package = Model.Packages.AddNew(Package_Name, "Package");
                Model.Packages.Refresh();
                Package.Update();
                Model.Update();
                return (Package.PackageGUID);
            }
            else
            {

                EA.Package Package = Repository.GetPackageByGuid(help);
                return (Package.PackageGUID);
            }
        }

        public string Create_Package(Database database, EA.Repository Repository, EA.Package Parent_Package, string Package_Name, bool create)
        {
            List<DB_Return> m_ret = new List<DB_Return>();

            string[] m_output = { "ea_guid" };
            string[] m_input_Property = { "Name", "Object_Type", "Package_ID", "ParentID" };

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_Name = new List<string>();
            help_Name.Add(Package_Name);
            ee.Add(help_Name.Select(x => new DB_Input(-1, x)).ToArray());
            List<string> help_Type = new List<string>();
            help_Type.Add("Package");
            ee.Add(help_Type.Select(x => new DB_Input(-1, x)).ToArray());
            List<int> help_ID1 = new List<int>();
            help_ID1.Add(Parent_Package.PackageID);
            ee.Add(help_ID1.Select(x => new DB_Input(x, null)).ToArray());
            List<int> help_ID = new List<int>();
            help_ID.Add(0);
            ee.Add(help_ID.Select(x => new DB_Input(x, null)).ToArray());

            OleDbType[] m_input_Type_oledb = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.BigInt, OleDbType.BigInt };
            OdbcType[] m_input_Type_odbc = { OdbcType.VarChar, OdbcType.VarChar, OdbcType.Int, OdbcType.Int };
            SqliteType[] m_input_Type_sqlite = { SqliteType.Text, SqliteType.Text, SqliteType.Integer, SqliteType.Integer };

            DB_Command command = new DB_Command();
            string help = "";
            if (create == false)
            {
                //SELECT


                string select2 = "";

                if (database.metamodel.dB_Type == DB_Type.SQLITE)
                {
                    select2 = command.Get_Select_Command_SQLITE("t_object", m_output, m_input_Property, ee);
                }
                else
                {
                    select2 = command.Get_Select_Command("t_object", m_output, m_input_Property, ee);
                }

                switch (database.metamodel.dB_Type)
                {
                    case DB_Type.ACCDB:
                        OleDbCommand SELECT2 = new OleDbCommand(select2, (OleDbConnection)database.oLEDB_Interface.dbConnection);
                        database.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type_oledb);
                        m_ret = database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output);
                        break;
                    case DB_Type.MSDASQL:
                        OdbcCommand SELECT = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                        database.oDBC_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type_odbc);
                        m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT, m_output);
                        break;
                    case DB_Type.SQLITE:
                        List<string> m_str = new List<string>();
                        List<SqliteType> sqliteTypes = new List<SqliteType>();

                        m_str.Add("Name");
                        m_str.Add("Object_Type");
                        m_str.Add("Package_ID");
                        m_str.Add("ParentID");


                        sqliteTypes.Add(SqliteType.Text);

                        SqliteCommand SELECT4 = new SqliteCommand(select2, (SqliteConnection)database.SQLITE_Interface.dbConnection);
                        database.SQLITE_Interface.Add_Parameters_Select(SELECT4, ee, m_input_Type_sqlite, m_str);
                        m_ret = database.SQLITE_Interface.SQLITE_SELECT_One_Table(SELECT4, m_output, sqliteTypes);
                        break;
                }

               
                if (m_ret[0].Ret.Count > 1)
                {
                    help = (m_ret[0].Ret[1].ToString());
                }
                else
                {
                    help = null;
                }
            }
            else
            {
                help = null;
            }
            


            if (help == null)
            {

               // EA.Package Model = Repository.Models.GetAt(0);

                EA.Package Package = Parent_Package.Packages.AddNew(Package_Name, "Package");

                Parent_Package.Packages.Refresh();
                Package.Update();
                Parent_Package.Update();
               // Package.ParentID = Parent_ID;
                return (Package.PackageGUID);
            }
            else
            {

                EA.Package Package = Repository.GetPackageByGuid(help);
                return (Package.PackageGUID);
            }
        }
    }
}

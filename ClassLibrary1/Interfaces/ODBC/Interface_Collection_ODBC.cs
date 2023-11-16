using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using Database_Connection;


namespace Requirement_Plugin.Interfaces
{
    public class Interface_Collection_ODBC
    {
        public List<string> Get_Elements_GUID(Database database, List<string> m_Type, List<string> m_Stereotype)
        {
            // XML xML = new XML();

            //List<string> m_Type = database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            //List<string> m_Stereotype = database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

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
            OdbcType[] m_input_Type = { OdbcType.VarChar, OdbcType.VarChar };
            DB_Command command = new DB_Command();

            if (database.metamodel.flag_Analyse_Diagram == true)
            {
                //Im Diagramm
                //Klassen direkt
                string select = command.Get_Select_Command(table, m_output, m_input_Property, ee);
                OdbcCommand SELECT = new OdbcCommand(select, (OdbcConnection)database.oDBC_Interface.dbConnection);
                database.oDBC_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type);
                SELECT.CommandText = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Object_ID IN (" + SELECT.CommandText + "));";
                List<DB_Return> m_ret = database.oDBC_Interface.DB_SELECT_One_Table(SELECT, m_output2);

                if (m_ret[0].Ret.Count > 1)
                {
                    m_GUID = (m_ret[0].Ret.GetRange(1, m_ret[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    m_GUID = (null);
                }
                //Instanzen 
                string select_instanz = command.Get_Select_Command(table, m_output2, m_input_Property, ee);
                OdbcCommand SELECT_Instanz = new OdbcCommand(select_instanz, (OdbcConnection)database.oDBC_Interface.dbConnection);
                database.oDBC_Interface.Add_Parameters_Select(SELECT_Instanz, ee, m_input_Type);
                SELECT_Instanz.CommandText = "SELECT PDATA1 FROM t_object WHERE PDATA1 IN(" + SELECT_Instanz.CommandText + ");";
                List<DB_Return> m_ret_Instanz = database.oDBC_Interface.DB_SELECT_One_Table(SELECT_Instanz, m_output3);

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
            }
            else
            {
                string select2 = command.Get_Select_Command(table, m_output2, m_input_Property, ee);
                OdbcCommand SELECT2 = new OdbcCommand(select2, (OdbcConnection)database.oDBC_Interface.dbConnection);
                database.oDBC_Interface.Add_Parameters_Select(SELECT2, ee, m_input_Type);
                List<DB_Return> m_ret2 = database.oDBC_Interface.DB_SELECT_One_Table(SELECT2, m_output2);

                if (m_ret2[0].Ret.Count > 1)
                {
                    m_GUID = (m_ret2[0].Ret.GetRange(1, m_ret2[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
                }
                else
                {
                    m_GUID = (null);
                }
            }

            if (m_GUID != null)
            {
                if (m_GUID.Count > 0)
                {

                    return (m_GUID);
                }

            }

            return (null);
        }
    }
}

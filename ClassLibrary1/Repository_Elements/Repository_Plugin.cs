///////////////////////////////////////////////////////////
//  Repository_Plugin.cs
//  Implementation of the Class Repository_Plugin
//  Generated by Enterprise Architect
//  Created on:      14-Jan-2019 19:40:37
//  Original author: Yannick
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.OleDb;


namespace Repsoitory_Elements
{
	public class Repository_Plugin {

		public Repository_Plugin(){

		}

		~Repository_Plugin(){

		}

        /// <summary>
        /// GUID erzeugen
        /// </summary>
        /// <returns></returns>
        public string Generate_GUID(string table, EA.Repository repository, Requirement_Plugin.Database Data)
        {
            string GUID;
          //  XML.XML xmL = new XML.XML();
            System.Guid guid;
            guid = System.Guid.NewGuid();

            GUID = "{" + guid.ToString() + "}";

            if (repository != null)
            {
                //ÜBerprüfung, ob GUID schon vorhanden
                //   string SQL = "SELECT ea_guid FROM " + table + " WHERE ea_guid = '" + GUID + "'";
                /*    if(table == "t_xref")
                    {
                        SQL = "SELECT XrefID FROM t_xref WHERE XrefID = '" + GUID + "'";
                    }
                    string xml_String = repository.SQLQuery(SQL);
                    List<string> m_GUID = (xmL.Xml_Read_Attribut("ea_guid", xml_String));
                    */
                List<string> m_GUID = new List<string>();

                if (table == "t_xref")
                {
                    m_GUID = this.Check_GUID(table, "XrefID", GUID, Data);
                }
                else
                {
                    m_GUID = this.Check_GUID(table, "ea_guid", GUID, Data);
                }
                if (m_GUID != null)
                {
                    GUID = Generate_GUID(table, repository, Data);
                }
            }

            return (GUID);

        }


        public List<string> Check_GUID(string _table, string _return, string GUID, Requirement_Plugin.Database Data)
        {
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Requirement_Plugin.Interfaces.Interface_Collection();

            return (interface_Collection_OleDB.Check_GUID(Data, GUID, _table, _return));

           /* List<string> _ret = new List<string>();
            XML xml = new XML();
            //SQL_Command command = new SQL_Command();
            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar };
            string table = _table;
            string[] m_output = { _return };

            string ret = "SELECT " + _return + " FROM " + table + " WHERE " + _return + " = ?";
            OleDbCommand oleDbCommand = new OleDbCommand(ret, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
            oleDbCommand.Parameters.Add("?", OleDbType.VarChar).Value = GUID;

            List<DB_Return> m_ret3 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(oleDbCommand, m_output);

            if (m_ret3[0].Ret.Count > 1)
            {
                _ret.Add(m_ret3[0].Ret[1].ToString());
                return (_ret);
            }
            else
            {
                return (null);
            }*/
        }
    }//end Repository_Plugin

}//end namespace Requirement_Plugin
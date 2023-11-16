using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data.Common;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using Requirement_Plugin;

using Ennumerationen;

namespace Database_Connection
{
    public class ODBC_Interface : DB_Interface
    {
       // public OdbcConnection dbConnection;

        #region Konstruktor & Destruktor
        public ODBC_Interface(EA.Repository repository, Requirement_PluginClass Base)
        {
            this.dB_Type = DB_Type.MSDASQL;
            bool flag_Connection_string = Get_Connection_String_Local(repository, Base);

            //   MessageBox.Show(this.Connection_string);
            //  bool flag_Open = OLEDB_Open();
           // DbConnection
        }

        ~ODBC_Interface()
        {
            // this.OLEDB_Close();
        }


        /// <summary>
        /// Es wird der Connection String für die OLDB Connection aus der lokalen EAP-File erstellt
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private bool Get_Connection_String_Local(EA.Repository repository, Requirement_PluginClass Base)
        {
            string ret_str = "";

            if (Base.IsProjectOpen(repository))
            {
               
                ret_str = repository.ConnectionString;
             //   MessageBox.Show(ret_str);
                ret_str = ret_str.Replace(@"\", @"\\");
             //   MessageBox.Show(ret_str);
                //Klappt dies auch mit eap-File?
                //  ret_str = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ret_str + ";Persist Security Info=False";
                ret_str = "Driver={PostgreSQL UNICODE};Server=pan3.bue.mukdo.mar;Port=5432;Database=afm_naf;Uid=afm_naf;Pwd=B0hne1!;";
               // ret_str = "Provider=MSDASQL.1,DSN=" + ret_str + "";

                this.Set_Connection_String(ret_str);

                return (true);
            }

            return (false);
        }
        /// <summary>
        /// Open einer OLDB Verbindung
        /// </summary>
        /// <param name="Connection_string"></param>
        /// <returns></returns>
        public bool  odbc_Open()
        {
            bool ret = false;

            OdbcConnection oledb_Connect = new OdbcConnection(this.Get_Connection_String());

            try
            {

                ret = true;
                this.dbConnection = oledb_Connect;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            return (true);

        }
        /// <summary>
        /// Schließen der OLEDB Verbindung
        /// </summary>
        /// <returns></returns>
        public bool odbc_Close()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Close();
                return (true);
            }

            return (false);

        }
        #endregion  Konstruktor & Destruktor

        #region helpers
        public string[] change_output(string[] m_output)
        {
            //string[] m_ret = { };

            if (m_output.Length > 0)
            {
                int i1 = 0;
                do
                {
                    string recent = m_output[i1];

                    recent = recent.ToLower();

                    m_output[i1] = recent;

                    i1++;
                } while (i1 < m_output.Length);
            }

            return (m_output);
        }

        #endregion

        #region SELECT
        public List<DB_Return> odbc_SELECT_One_Table(OdbcCommand command, string[] m_output)
        {
            //output bearbeiten
            string[] new_output = this.change_output(m_output);

            return (this.DB_SELECT_One_Table(command, m_output));

        }
            /// <summary>
            /// Es werden die Parameter zum oleDbCommand fpr einen Select Befehl hinzugefügt.
            /// </summary>
            /// <param name="command"></param>
            /// <param name="m_input_Value"></param>
            /// <param name="m_input_Type"></param>
            public void Add_Parameters_Select(OdbcCommand command, List<DB_Input[]> m_input_Value, OdbcType[] m_input_Type)
        {
           if (m_input_Value.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_input_Value[i1].Length > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (m_input_Type[i1] == OdbcType.Int || m_input_Type[i1] == OdbcType.Int || m_input_Type[i1] == OdbcType.Double)
                            {
                                command.Parameters.Add("?", m_input_Type[i1]).Value = m_input_Value[i1][i2]._int;
                            }
                            else
                            {
                                command.Parameters.Add("?", m_input_Type[i1]).Value = m_input_Value[i1][i2]._str;
                            }

                            i2++;
                        } while (i2 < m_input_Value[i1].Length);
                    }



                    i1++;
                } while (i1 < m_input_Value.Count);
            }
        }
        #endregion SELECT

        #region Insert
        public void ODBC_INSERT_One_Table(OdbcCommand odbc_command)
        {
            this.DB_INSERT_One_Table(odbc_command);
        }

        public void ODBC_INSERT_One_Table_Multiple_TV(Database Data, List<DB_Insert> m_Insert, Repsoitory_Elements.TaggedValue tagged, string table, int ID, object[] m_Input_Property)
        {
            DB_Command command = new DB_Command();
            string Insert = command.Get_Insert_Command(table, m_Input_Property, m_Input_Property, Ennumerationen.DB_Type.MSDASQL);
            OdbcCommand odbc_command = new OdbcCommand(Insert, (OdbcConnection)this.dbConnection);
            bool flag_open = true;
            try
            {
               

                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }

                if (m_Insert.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        string guid = tagged.Generate_GUID(table);

                        odbc_command.Parameters.Clear();

                        odbc_command.Parameters.Add("?", OdbcType.Int).Value = ID;
                        odbc_command.Parameters.Add("?", m_Insert[i1].odbc_Type).Value = Data.metamodel.Get_XAC_Import(m_Insert[i1].Property);

                        if (m_Insert[i1].odbc_Type == OdbcType.VarChar)
                        {
                            if (m_Insert[i1].Value_str == "" || m_Insert[i1].Value_str == null)
                            {
                                m_Insert[i1].Value_str = " ";
                            }

                            if (m_Insert[i1].Value_str.ToString().Length > 255)
                            {
                                odbc_command.Parameters.Add("?", m_Insert[i1].odbc_Type).Value = "<memo>";
                                odbc_command.Parameters.Add("?", m_Insert[i1].odbc_Type).Value = m_Insert[i1].Value_str;
                            }
                            else
                            {
                                odbc_command.Parameters.Add("?", m_Insert[i1].odbc_Type).Value = m_Insert[i1].Value_str;
                                odbc_command.Parameters.Add("?", m_Insert[i1].odbc_Type).Value = " ";
                            }
                        }


                        odbc_command.Parameters.Add("?", OdbcType.VarChar).Value = guid;

                        var numberOfRowsInserted = odbc_command.ExecuteNonQuery();
                        i1++;
                    } while (i1 < m_Insert.Count);
                }



                if (flag_open != true)
                {
                    this.dbConnection.Close();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                MessageBox.Show(odbc_command.CommandText);
                this.dbConnection.Close();
            }


        }

        /// <summary>
        /// Es werden die Parameter zum oleDbCommand fpr einen Select Befehl hinzugefügt.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="m_input_Value"></param>
        /// <param name="m_input_Type"></param>
        public void Add_Parameters_Insert(OdbcCommand command, object[] m_input_Value, OdbcType[] m_input_Type)
        {
            if (m_input_Value.Length > 0 && m_input_Value.Length == m_input_Type.Length)
            {
                int i2 = 0;
                do
                {
                    command.Parameters.Add("?", m_input_Type[i2]).Value = m_input_Value[i2];

                    i2++;
                } while (i2 < m_input_Value.Length);
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler beim Einfügen der Parameter des Insert Befehls: " + command.CommandText + " mit den Value " + m_input_Value.ToString() + " und den ODBCTypen " + m_input_Type.ToString() + " gekommen.");
            }
        }
        #endregion Insert
        #region UPDATE
        /// <summary>
        /// Update Befehl
        /// </summary>
        /// <param name="Command"></param>
        public void ODBCB_UPDATE_One_Table(OdbcCommand odbc_Command)
        {
            this.DB_UPDATE_One_Table(odbc_Command);
        }

        public void ODBC_Update_One_Table_Multiple_TV(string SQL, List<DB_Insert> m_Update, Database Data, int ID)
        {
            OdbcCommand odbc_command = new OdbcCommand(SQL, (OdbcConnection)this.dbConnection);
            try
            {

                // SQL_Command command = new SQL_Command();
               

                bool flag_open = true;

                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }

                if (m_Update.Count > 0)
                {
                    int i1 = 0;
                    do
                    {

                        odbc_command.Parameters.Clear();
                        if (m_Update[i1].odbc_Type == OdbcType.VarChar)
                        {
                            if (m_Update[i1].Value_str == "" || m_Update[i1].Value_str == null)
                            {
                                m_Update[i1].Value_str = " ";
                            }

                            if (m_Update[i1].Value_str.ToString().Length > 255)
                            {
                                odbc_command.Parameters.Add("?", m_Update[i1].odbc_Type).Value = "<memo>";
                                odbc_command.Parameters.Add("?", m_Update[i1].odbc_Type).Value = m_Update[i1].Value_str;
                            }
                            else
                            {
                                odbc_command.Parameters.Add("?", m_Update[i1].odbc_Type).Value = m_Update[i1].Value_str;
                                odbc_command.Parameters.Add("?", m_Update[i1].odbc_Type).Value = " ";
                            }
                        }

                        odbc_command.Parameters.Add("?", OdbcType.Int).Value = ID;
                        odbc_command.Parameters.Add("?", OdbcType.VarChar).Value = Data.metamodel.Get_XAC_Import(m_Update[i1].Property);

                        // oledb_Command.Parameters.Add("?", OleDbType.VarChar).Value = guid;

                        var numberOfRowsInserted = odbc_command.ExecuteNonQuery();
                        i1++;
                    } while (i1 < m_Update.Count);
                }



                if (flag_open != true)
                {
                    this.dbConnection.Close();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                MessageBox.Show(odbc_command.CommandText);
                this.dbConnection.Close();
            }

        }


        public void Add_Parameters_Update(OdbcCommand command, object[] m_input_Value, OdbcType[] m_input_Type, List<object[]> m_select_Value, OdbcType[] m_select_Type)
        {
            if (m_input_Value.Length > 0 && m_input_Value.Length == m_input_Type.Length)
            {
                int i2 = 0;
                do
                {
                    command.Parameters.Add("?", m_input_Type[i2]).Value = m_input_Value[i2];

                    i2++;
                } while (i2 < m_input_Value.Length);
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler beim Einfügen der Parameter des Update Befehls: " + command.CommandText + " mit den Value " + m_input_Value.ToString() + " und den odbcTypen " + m_input_Type.ToString() + " gekommen.");
            }
            if (m_select_Value.Count > 0 && m_select_Value.Count == m_select_Type.Length)
            {
                int i1 = 0;
                do
                {
                    if (m_select_Value[i1].Length > 0)
                    {
                        int i3 = 0;
                        do
                        {
                            command.Parameters.Add("?", m_select_Type[i1]).Value = m_select_Value[i1][i3];

                            i3++;
                        } while (i3 < m_select_Value[i1].Length);
                    }

                    i1++;
                } while (i1 < m_select_Value.Count);
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler beim Einfügen der Parameter des Update Befehls: " + command.CommandText + " mit den Value " + m_input_Value.ToString() + " und den odbcTypen " + m_input_Type.ToString() + " gekommen.");
            }



        }
        #endregion UPDATE

        #region DELETE
        /// <summary>dbConnection.Close()
        /// Delete Befehl
        /// </summary>
        /// <param name="Command"></param>
        public void ODBC_DELETE_One_Table(OdbcCommand odbc_Command)
        {
            this.DB_DELETE_One_Table(odbc_Command);
          
        }
        public void Add_Parameters_Delete(OdbcCommand command, List<object[]> m_input_Value, OdbcType[] m_input_Type)
        {
            if (m_input_Value.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_input_Value[i1].Length > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            command.Parameters.Add("?", m_input_Type[i1]).Value = m_input_Value[i1][i2];

                            i2++;
                        } while (i2 < m_input_Value[i1].Length);
                    }



                    i1++;
                } while (i1 < m_input_Value.Count);
            }
        }
        #endregion DELETE
    }
}

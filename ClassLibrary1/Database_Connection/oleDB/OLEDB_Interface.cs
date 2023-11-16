using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

using Requirement_Plugin;
using Ennumerationen;
//using Metamodels;

namespace Database_Connection
{
    public class OLEDB_Interface : DB_Interface
    {
       
      //  public OleDbConnection dbConnection;

        #region Konstruktor & Destruktor
        public OLEDB_Interface(EA.Repository repository, Requirement_PluginClass Base)
        {
            this.dB_Type = DB_Type.ACCDB;
            bool flag_Connection_string = Get_Connection_String_Local(repository, Base);

         //   MessageBox.Show(this.Connection_string);
          //  bool flag_Open = OLEDB_Open();

        }

        ~OLEDB_Interface()
        {
           // this.OLEDB_Close();
        }

  /*      public void Set_Connection_String(string con_string_new)
        {
            this.Connection_string = con_string_new;
        }
        public string Get_Connection_String()
        {
            return (this.Connection_string);
        }*/

        /// <summary>
        /// Open einer OLDB Verbindung
        /// </summary>
        /// <param name="Connection_string"></param>
        /// <returns></returns>
        public bool OLEDB_Open()
        {
            bool ret = false;

           OleDbConnection oledb_Connect = new OleDbConnection(this.Get_Connection_String());

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
        public bool OLEDB_Close()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Close();
                return (true);
            }

            return (false);

        }

        /// <summary>
        /// Es wird der Connection String für die OLDB Connection aus der lokalen EAP-File erstellt
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private bool Get_Connection_String_Local(EA.Repository repository, Requirement_PluginClass Base)
        {
            string ret_str = "";

            if(Base.IsProjectOpen(repository))
            {
                ret_str = repository.ConnectionString;
                ret_str = ret_str.Replace(@"\", @"\\");

                //Klappt dies auch mit eap-File?
                ret_str = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ret_str + ";Persist Security Info=False";

                this.Set_Connection_String (ret_str);

                return (true);
            }

            return (false);
        }
        #endregion Konstruktor & Destruktor
        #region SELECT
        /// <summary
        /// Select Befehl
        /// Rückgabe im Format [outputname, value[0], value[1],..., value[n]]
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="m_output"></param>
        /// <returns></returns>
       public List<DB_Return> oleDB_SELECT_One_Table(OleDbCommand oledb_Command, string[] m_output)
        {
            return (this.DB_SELECT_One_Table( oledb_Command,  m_output));
          /*  List<DB_Return> m_ret = new List<DB_Return>();
            bool flag_open = true;

            try
            {
                if(this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }
               
                

                OleDbDataAdapter oledb_Adapter = new OleDbDataAdapter(oledb_Command);

                DataSet dset = new DataSet();

                oledb_Adapter.Fill(dset);

                if (m_output.Length > 0)
                {
                    int o1 = 0;
                    do
                    {
                        // object[] m_help = new object[dset.Tables[0].Rows.Count + 1];
                        DB_Return m_help = new DB_Return();

                        if (dset.Tables[0].Rows.Count > 0)
                        {
                            int i1 = 1;

                            m_help.Ret.Add((dset.Tables[0].Columns[o1].ColumnName));
                            
                            do
                            {

                                if(dset.Tables[0].Rows[i1 - 1].ItemArray[o1].GetType().FullName != "System.DBNull")
                                {
                                    
                                    m_help.Ret.Add(dset.Tables[0].Rows[i1 - 1].ItemArray[o1]);
                                }
                                else
                                {
                                    m_help.Ret.Add("null");
                                }
                               
                                i1++;
                            } while (i1 <= dset.Tables[0].Rows.Count);



                        }

                        m_ret.Add(m_help);

                        o1++;
                    } while (o1 < m_output.Length);
                }
                else
                {
                    m_ret = null;
                }

                if(flag_open != true)
                {
                    this.OLEDB_Close();
                }
                    
               
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                m_ret = null;
                this.OLEDB_Close();
            }
            


            return (m_ret);
            */
        }

        public List<DB_Return> oleDB_SELECT_One_Table_Multiple_Property(OleDbCommand oledb_Command, string[] m_output, List<Metamodels.TV_Map> tV_Maps, int id)
        {
            return (this.DB_SELECT_One_Table_Multiple_Property(oledb_Command, m_output, tV_Maps, id));

            /*   List<DB_Return> m_ret = new List<DB_Return>();

             try
               {
                   bool flag_open = true;

                   if (this.dbConnection.State == ConnectionState.Closed)
                   {
                       this.dbConnection.Open();
                       flag_open = false;
                   }

                   DataSet dset = new DataSet();

                   if(tV_Maps.Count > 0)
                   {
                       int i1 = 0;
                       do
                       {
                           oledb_Command.Parameters.Clear();
                           oledb_Command.Parameters.Add("?", tV_Maps[i1].oleDB_Type).Value = tV_Maps[i1].Map_Name;
                           oledb_Command.Parameters.Add("?", OleDbType.BigInt).Value = id;

                           OleDbDataAdapter oledb_Adapter = new OleDbDataAdapter(oledb_Command);
                           oledb_Adapter.Fill(dset, tV_Maps[i1].Name);

                           i1++;
                       } while (i1 < tV_Maps.Count);
                   }

                   if(dset.Tables.Count > 0)
                   {
                       int i2 = 0;
                       do
                       {
                           DB_Return m_help = new DB_Return();

                           m_help.Ret.Add((dset.Tables[i2].TableName));

                           if (dset.Tables[i2].Rows.Count > 0)
                           {
                               int i1 = 0;

                               if(dset.Tables[i2].Rows[0].ItemArray.Length > 0)
                               {
                                   do
                                   {
                                           if (dset.Tables[i2].Rows[0].ItemArray[i1].GetType().FullName != "System.DBNull")
                                           {
                                               m_help.Ret.Add(dset.Tables[i2].Rows[0].ItemArray[i1]);
                                           }
                                           else
                                           {
                                               m_help.Ret.Add(null);
                                           }


                                       i1++;
                                   } while (i1 < dset.Tables[i2].Rows[0].ItemArray.Length);
                               }                          
                           }

                           m_ret.Add(m_help);

                           i2++;
                       } while (i2 < dset.Tables.Count);

                   }
                   else
                   {
                       m_ret = null;
                   }

                   if (flag_open != true)
                   {
                       this.OLEDB_Close();
                   }
               }
              catch (Exception err)
               {
                   MessageBox.Show(err.Message);
                   m_ret = null;
                   this.OLEDB_Close();
               }



               return (m_ret);
               */
        }
       /// <summary>
        /// Es werden die Parameter zum oleDbCommand fpr einen Select Befehl hinzugefügt.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="m_input_Value"></param>
        /// <param name="m_input_Type"></param>
        public void Add_Parameters_Select(OleDbCommand command, List<DB_Input[]> m_input_Value, OleDbType[] m_input_Type)
        {
            if(m_input_Type.Length > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_input_Value[i1].Length > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (m_input_Value[i1][i2] != null)
                            {
                                if (m_input_Type[i1] == OleDbType.BigInt || m_input_Type[i1] == OleDbType.Integer || m_input_Type[i1] == OleDbType.Double)
                                {
                                    command.Parameters.Add("?", m_input_Type[i1]).Value = m_input_Value[i1][i2]._int;
                                }
                                else
                                {
                                    command.Parameters.Add("?", m_input_Type[i1]).Value = m_input_Value[i1][i2]._str;
                                }
                            }

                            i2++;
                        } while (i2 < m_input_Value[i1].Length);
                    }

                  

                    i1++;
                } while (i1 < m_input_Type.Length && i1 < m_input_Value.Count);
            }
        }
        
        #endregion SELECT
        #region INSERT
        /// <summary>
        /// Insert Befehl
        /// </summary>
        /// <param name="Command"></param>
        public void OLEDB_INSERT_One_Table(OleDbCommand oledb_Command)
        {
            this.DB_INSERT_One_Table(oledb_Command);
           /* try
            {
                bool flag_open = true;

                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }

                var numberOfRowsInserted = oledb_Command.ExecuteNonQuery();

                if (flag_open != true)
                {
                    this.OLEDB_Close();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                this.dbConnection.Close();
            }
            */
        }

        public void OLEDB_INSERT_One_Table_Multiple_TV(Database Data, List<DB_Insert> m_Insert, Repsoitory_Elements.TaggedValue tagged, string table, int ID, object[] m_Input_Property)
        {
            try
            {
                DB_Command command = new DB_Command();
                string Insert = command.Get_Insert_Command(table, m_Input_Property, m_Input_Property, Ennumerationen.DB_Type.ACCDB);
                OleDbCommand oledb_Command = new OleDbCommand(Insert, (OleDbConnection)this.dbConnection);
                bool flag_open = true;

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
                        
                        oledb_Command.Parameters.Clear();

                        oledb_Command.Parameters.Add("?", OleDbType.BigInt).Value = ID;
                        oledb_Command.Parameters.Add("?", m_Insert[i1].oleDB_Type).Value = Data.metamodel.Get_XAC_Import(m_Insert[i1].Property);

                        if(m_Insert[i1].oleDB_Type == OleDbType.VarChar)
                        {
                            if (m_Insert[i1].Value_str == "" || m_Insert[i1].Value_str == null)
                            {
                                m_Insert[i1].Value_str = " ";
                            }

                            if(m_Insert[i1].Value_str.ToString().Length > 255)
                            {
                                oledb_Command.Parameters.Add("?", m_Insert[i1].oleDB_Type).Value = "<memo>";
                                oledb_Command.Parameters.Add("?", m_Insert[i1].oleDB_Type).Value = m_Insert[i1].Value_str;
                            }
                            else
                            {
                                oledb_Command.Parameters.Add("?", m_Insert[i1].oleDB_Type).Value = m_Insert[i1].Value_str;
                                oledb_Command.Parameters.Add("?", m_Insert[i1].oleDB_Type).Value = " ";
                            }
                        }


                        oledb_Command.Parameters.Add("?", OleDbType.VarChar).Value = guid;

                        var numberOfRowsInserted = oledb_Command.ExecuteNonQuery();
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
                this.dbConnection.Close();
           }
           

        }

        /// <summary>
        /// Es werden die Parameter zum oleDbCommand fpr einen Select Befehl hinzugefügt.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="m_input_Value"></param>
        /// <param name="m_input_Type"></param>
        public void Add_Parameters_Insert(OleDbCommand command, object[] m_input_Value, OleDbType[] m_input_Type)
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
                MessageBox.Show("Es ist ein Fehler beim Einfügen der Parameter des Insert Befehls: " + command.CommandText + " mit den Value " + m_input_Value.ToString() + " und den oleDBTypen " + m_input_Type.ToString() + " gekommen.");
            }
        }
        #endregion INSERT
        #region UPDATE
        /// <summary>
        /// Update Befehl
        /// </summary>
        /// <param name="Command"></param>
        public void OLEDB_UPDATE_One_Table(OleDbCommand oledb_Command)
        {
            this.DB_UPDATE_One_Table(oledb_Command);
        /*  try
           {
                bool flag_open = true;

                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }

                var numberOfRowsInserted = oledb_Command.ExecuteNonQuery();

            if (flag_open != true)
            {
                this.dbConnection.Close();
            }

              }
              catch (Exception err)
              {
                  MessageBox.Show(err.Message);
                  this.dbConnection.Close();
              }
            */  
        }

        public void OLEDB_Update_One_Table_Multiple_TV(string SQL, List<DB_Insert> m_Update, Database Data, int ID)
        {
            try
            {
               
                // SQL_Command command = new SQL_Command();
                OleDbCommand oledb_Command = new OleDbCommand(SQL, (OleDbConnection)this.dbConnection);

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
                     //   string guid = tagged.Generate_GUID(table, repository);

                        oledb_Command.Parameters.Clear();

                       
                     //   oledb_Command.Parameters.Add("?", m_Update[i1].oleDB_Type).Value = Data.metamodel.Get_XAC_Import(m_Update[i1].Property);

                        if (m_Update[i1].oleDB_Type == OleDbType.VarChar)
                        {
                            if (m_Update[i1].Value_str == "" || m_Update[i1].Value_str == null)
                            {
                                m_Update[i1].Value_str = " ";
                            }

                            if (m_Update[i1].Value_str.ToString().Length > 255)
                            {
                                oledb_Command.Parameters.Add("?", m_Update[i1].oleDB_Type).Value = "<memo>";
                                oledb_Command.Parameters.Add("?", m_Update[i1].oleDB_Type).Value = m_Update[i1].Value_str;
                            }
                            else
                            {
                                oledb_Command.Parameters.Add("?", m_Update[i1].oleDB_Type).Value = m_Update[i1].Value_str;
                                oledb_Command.Parameters.Add("?", m_Update[i1].oleDB_Type).Value = " ";
                            }
                        }

                        oledb_Command.Parameters.Add("?", OleDbType.BigInt).Value = ID;
                        oledb_Command.Parameters.Add("?", OleDbType.VarChar).Value = Data.metamodel.Get_XAC_Import(m_Update[i1].Property);

                        // oledb_Command.Parameters.Add("?", OleDbType.VarChar).Value = guid;

                        var numberOfRowsInserted = oledb_Command.ExecuteNonQuery();
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
                this.dbConnection.Close();
            }
            
        }


        public void Add_Parameters_Update(OleDbCommand command, object[] m_input_Value, OleDbType[] m_input_Type, List<object[]> m_select_Value, OleDbType[] m_select_Type)
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
                MessageBox.Show("Es ist ein Fehler beim Einfügen der Parameter des Update Befehls: " + command.CommandText + " mit den Value " + m_input_Value.ToString() + " und den oleDBTypen " + m_input_Type.ToString() + " gekommen.");
            }
            if(m_select_Value.Count > 0 && m_select_Value.Count == m_select_Type.Length)
            {
                int i1 = 0;
                do
                {
                    if(m_select_Value[i1].Length > 0)
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
                MessageBox.Show("Es ist ein Fehler beim Einfügen der Parameter des Update Befehls: " + command.CommandText + " mit den Value " + m_input_Value.ToString() + " und den oleDBTypen " + m_input_Type.ToString() + " gekommen.");
            }



        }
        #endregion UPDATE
        #region DELETE
        /// <summary>dbConnection.Close()
        /// Delete Befehl
        /// </summary>
        /// <param name="Command"></param>
        public void OLEDB_DELETE_One_Table(OleDbCommand oledb_Command)
        {
            this.DB_DELETE_One_Table(oledb_Command);
           /* try
            {
                bool flag_open = true;

                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }

                var numberOfRowsInserted = oledb_Command.ExecuteNonQuery();

                if (flag_open != true)
                {
                    this.dbConnection.Close();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
               this.dbConnection.Close();
            }
            */
        }
        public void Add_Parameters_Delete(OleDbCommand command, List<object[]> m_input_Value, OleDbType[] m_input_Type)
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

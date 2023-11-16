using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;


//using Metamodels;
using Ennumerationen;

namespace Database_Connection
{
    public class DB_Interface
    {
        private string Connection_string;
        public DbConnection dbConnection;
        public DB_Type dB_Type;

        public void Set_Connection_String(string con_string_new)
        {
            this.Connection_string = con_string_new;
        }
        public string Get_Connection_String()
        {
            return (this.Connection_string);
        }

        #region DB_Connection
        /// <summary>
        /// Open einer OLDB Verbindung
        /// </summary>
        /// <param name="Connection_string"></param>
        /// <returns></returns>
        public bool db_Open()
        {
            bool ret = false;

            if(dB_Type == DB_Type.SQLITE)
            {
                SqliteConnection oledb_Connect = new SqliteConnection(this.Get_Connection_String());

                try
                {

                    ret = true;
                    this.dbConnection = oledb_Connect;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
            else
            {
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
            }

           

            return (true);

        }
        /// <summary>
        /// Schließen der OLEDB Verbindung
        /// </summary>
        /// <returns></returns>
        public bool db_Close()
        {
            if (this.dbConnection != null)
            {
                try
                {
                    if (this.dbConnection.State == ConnectionState.Open)
                    {
                        this.dbConnection.Close();
                    }
                }
                catch
                {

                }

               
                return (true);
            }

            return (false);

        }
        #endregion DB_Connection

        #region helpers
        public string[] change_output(string[] m_output)
        {
            //string[] m_ret = { };

            if (m_output.Length > 0)
            {
                int i1 = 0;
                do
                {
                    string recent =  m_output[i1] ;

                    recent =  recent.ToLower();

                    m_output[i1] = recent;

                    i1++;
                } while (i1 < m_output.Length);
            }

            return (m_output);
        }

        #endregion

        #region SELECT
        public List<DB_Return> DB_SELECT_One_Table(DbCommand Command, string[] m_output)
        {
            List<DB_Return> m_ret = new List<DB_Return>();
           
            bool flag_open = true;

            try
            {
                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }


                DataAdapter dataAdapter = null;

                switch (dB_Type)
                {
                    case DB_Type.ACCDB:
                        dataAdapter = new OleDbDataAdapter((OleDbCommand)Command);
                        break;
                    case DB_Type.MSDASQL:
                        m_output = this.change_output(m_output);
                        dataAdapter = new OdbcDataAdapter((OdbcCommand)Command);
                        break;
                    
                }


                DataSet dset = new DataSet();

                dataAdapter.Fill(dset);

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

                                if (dset.Tables[0].Rows[i1 - 1].ItemArray[o1].GetType().FullName != "System.DBNull")
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
                        else
                        {
                            if(dset.Tables.Count > 1)
                            {
                                if (dset.Tables[1].Rows.Count > 0)
                                {
                                    if(dset.Tables[1].TableName == dset.Tables[0].TableName+"1")
                                    {
                                        int i1 = 1;

                                        m_help.Ret.Add((dset.Tables[1].Columns[o1].ColumnName));

                                        do
                                        {

                                            if (dset.Tables[1].Rows[i1 - 1].ItemArray[o1].GetType().FullName != "System.DBNull")
                                            {

                                                m_help.Ret.Add(dset.Tables[1].Rows[i1 - 1].ItemArray[o1]);
                                            }
                                            else
                                            {
                                                m_help.Ret.Add("null");
                                            }

                                            i1++;
                                        } while (i1 <= dset.Tables[1].Rows.Count);
                                    }
                                    
                                }
                            }
                        }

                        m_ret.Add(m_help);

                        o1++;
                    } while (o1 < m_output.Length);
                }
                else
                {
                    m_ret = null;
                }

                if (flag_open != true)
                {
                    this.dbConnection.Close();
                }


            }
            catch (Exception err)
            {
                
                
                MessageBox.Show(err.Message);
                MessageBox.Show(Command.CommandText);
                m_ret = null;
                this.dbConnection.Close();
            }



            return (m_ret);

        }

        public List<DB_Return> DB_SELECT_One_Table_Multiple_Property(DbCommand db_Command, string[] m_output, List<Metamodels.TV_Map> tV_Maps, int id)
        {
            List<DB_Return> m_ret = new List<DB_Return>();

            try
            {
                bool flag_open = true;

                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }

                DataSet dset = new DataSet();

                if (tV_Maps.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        db_Command.Parameters.Clear();
                       

                       // OleDbDataAdapter oledb_Adapter = new OleDbDataAdapter(oledb_Command);
                        DataAdapter dataAdapter = null;

                        switch (dB_Type)
                        {
                            case DB_Type.ACCDB:  
                                OleDbCommand db_Command2 = (OleDbCommand)db_Command;
                                db_Command2.Parameters.Add("?", tV_Maps[i1].oleDB_Type).Value = tV_Maps[i1].Map_Name;
                                db_Command2.Parameters.Add("?", OleDbType.BigInt).Value = id;
                                OleDbDataAdapter dataAdapter2 = new OleDbDataAdapter(db_Command2);
                                dataAdapter2.Fill(dset, tV_Maps[i1].Name);
                                break;
                            case DB_Type.MSDASQL:
                                OdbcCommand db_Command3 = (OdbcCommand)db_Command;
                                db_Command3.Parameters.Add("?", tV_Maps[i1].odbc_Type).Value = tV_Maps[i1].Map_Name;
                                db_Command3.Parameters.Add("?", OdbcType.Int).Value = id;
                                OdbcDataAdapter dataAdapter3 = new OdbcDataAdapter(db_Command3);
                                dataAdapter3.Fill(dset, tV_Maps[i1].Name);
                                break;

                        }
                        //dataAdapter.Fill(dset, tV_Maps[i1].Name);

                        i1++;
                    } while (i1 < tV_Maps.Count);
                }

                if (dset.Tables.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        DB_Return m_help = new DB_Return();

                        m_help.Ret.Add((dset.Tables[i2].TableName));

                        if (dset.Tables[i2].Rows.Count > 0)
                        {
                            int i1 = 0;

                            if (dset.Tables[i2].Rows[0].ItemArray.Length > 0)
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
                        else
                        {
                            if(i2 < dset.Tables.Count-1)
                            {
                                if ((dset.Tables[i2 + 1].TableName) == m_help.Ret[0]+"1")
                                {
                                    i2++;
                                    if (dset.Tables[i2].Rows.Count > 0)
                                    {
                                        int i1 = 0;

                                        if (dset.Tables[i2].Rows[0].ItemArray.Length > 0)
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
                                }
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
                    this.dbConnection.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                MessageBox.Show(db_Command.CommandText);
                m_ret = null;
                this.dbConnection.Close();
            }
           


            return (m_ret);

        }

        #region SQLITE
        public List<DB_Return> DB_SELECT_One_Table_Sqlite(SqliteCommand Command, string[] m_output, List<SqliteType> m_sqliteType)
        {
            List<DB_Return> m_ret = new List<DB_Return>();

            bool flag_open = true;

            try
            {
                

                int i1 = 0;

                do
                {
                    if (this.dbConnection.State == ConnectionState.Closed)
                    {
                        this.dbConnection.Open();
                        flag_open = false;
                    }


                    SqliteDataReader sqliteDataReader = Command.ExecuteReader();

                    List<object> m_help = new List<object>();

                    m_help.Add(m_output[i1]);

                    while (sqliteDataReader.Read())
                     {
                        if(sqliteDataReader.IsDBNull(sqliteDataReader.GetOrdinal(m_output[i1])) == false)
                        {
                            if (m_sqliteType[i1] == SqliteType.Text)
                            {

                                m_help.Add(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal(m_output[i1])));
                            }
                            else
                            {
                                m_help.Add(sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal(m_output[i1])));
                            }
                        }
                        else
                        {
                            //m_help.Add(null);
                        }
                       
                       
                     }

                    if (m_help.Count == 1)
                    {
                        if (m_sqliteType[i1] == SqliteType.Text)
                        {

                       //     m_help.Add(null);
                        }
                        else
                        {
                       //     m_help.Add(null);
                        }
                    }


                    DB_Return dB_Return = new DB_Return();
                    dB_Return.Ret = m_help;

                    m_ret.Add(dB_Return);

                    sqliteDataReader.Close();

                   // this.dbConnection.Close();

                    i1++;
                 } while (i1 < m_output.Length);


              


            }
            catch (Exception err)
            {


                MessageBox.Show(err.Message);
                MessageBox.Show(Command.CommandText);
                m_ret = null;
                this.dbConnection.Close();
            }



            return (m_ret);

        }

       
        #endregion

        #endregion SELECT

        #region INSERT
        /// <summary>
        /// Insert Befehl
        /// </summary>
        /// <param name="Command"></param>
        public void DB_INSERT_One_Table(DbCommand db_Command)
        {
            try
            {
                bool flag_open = true;

                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }

                var numberOfRowsInserted = db_Command.ExecuteNonQuery();

                if (flag_open != true)
                {
                    this.dbConnection.Close();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                MessageBox.Show(db_Command.CommandText);
                this.dbConnection.Close();
            }

        }


        #endregion Insert
        #region UPDATE
        /// <summary>
        /// Update Befehl
        /// </summary>
        /// <param name="Command"></param>
        public void DB_UPDATE_One_Table(DbCommand db_Command)
        {
            var flag = false;
            var counter = 0;
            var counter_limit = 5;

            do
            {
                try
                {
                    bool flag_open = true;

                    if (this.dbConnection.State == ConnectionState.Closed)
                    {
                        this.dbConnection.Open();
                        flag_open = false;
                    }

                    var numberOfRowsInserted = db_Command.ExecuteNonQuery();

                    if (flag_open != true)
                    {
                        this.dbConnection.Close();
                    }
                    flag = true;
                }
                catch (Exception err)
                {
                    if(err.HResult != -2147467259 || counter > counter_limit-1)
                    {
                        MessageBox.Show(err.Message);
                        MessageBox.Show(db_Command.CommandText);
                        this.dbConnection.Close();

                        flag = true;
                    }
                    
                }


                counter++;
            } while (flag == false && counter < counter_limit);

           

        }
        #endregion Update

        #region DELETE
        /// <summary>dbConnection.Close()
        /// Delete Befehl
        /// </summary>
        /// <param name="Command"></param>
        public void DB_DELETE_One_Table(DbCommand db_Command)
        {
            try
            {
                bool flag_open = true;

                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                    flag_open = false;
                }

                var numberOfRowsInserted = db_Command.ExecuteNonQuery();

                if (flag_open != true)
                {
                    this.dbConnection.Close();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                MessageBox.Show(db_Command.CommandText);
                this.dbConnection.Close();
            }

        }

        #endregion DELETE
    }
}

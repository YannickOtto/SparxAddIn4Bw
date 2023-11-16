using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Common;
using System.Windows.Forms;
using Ennumerationen;
using Microsoft.Office.Interop.Excel;
using System.Xml.Linq;

namespace Database_Connection
{
    public class DB_Command
    {
        #region Konstruktor und Destruktor
        public DB_Command()
        {
            //Hier soll nichts passieren
        }

        ~DB_Command()
        {

        }
        #endregion Konstruktor und Destruktor

        #region SELECT
        /// <summary>
        /// Nur für einfache Select Befehle mit AND Verknüpfung. Jeder Eintrag in der Liste von m_input_Value ist eine Array mit zugehörigen 
        /// Werten zu seinem Property.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="m_output"></param>
        /// <param name="m_input_Property"></param>
        /// <param name="m_input_Value"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public string Get_Select_Command(string table, string[] m_output, string[] m_input_Property, List<DB_Input[]> m_input_Value)
        {
            string ret = "";
            DbCommand oleDbCommand;

            if (table != null)
            {
                //Output Parameter  und Table Festlegen
                if (m_output.Length > 0)
                {
                    ret = "SELECT ";
                    int i1 = 0;
                    do
                    {
                        ret = ret + m_output[i1] + " ";

                        if(i1 != m_output.Length-1)
                        {
                            ret = ret + ", ";
                        }

                        i1++;
                    } while (i1 < m_output.Length);

                    ret = ret + "FROM " + table + " ";

                }
                else
                {
                    ret = "SELECT * FROM " + table + " ";
                }

                //Input Parameter einfügen
                if(m_input_Property.Length > 0 && m_input_Value.Count > 0 && m_input_Value.Count == m_input_Property.Length)
                {
                    ret = ret + "WHERE ";
                    int i2 = 0;
                    do
                    {
                        //ret = ret + m_input_Property[i2] + " IN (" + m_input_Value[i2] + ") ";

                        ret = ret + m_input_Property[i2] + " IN (";

                        if(m_input_Value[i2].Length > 1)
                        {
                            int i3 = 0;
                            do
                            {
                                ret = ret + "?";

                                if(i3 < m_input_Value[i2].Length-1)
                                {
                                    ret = ret + ",";
                                }

                                i3++;
                            } while (i3 < m_input_Value[i2].Length);

                            ret = ret + ") ";
                        }
                        else
                        {
                            ret = ret + "?) ";
                        }


                        if (i2 != m_input_Property.Length-1)
                        {
                            ret = ret + "AND ";
                        }

                        i2++;
                    } while (i2 < m_input_Property.Length);
                }

            //   oleDbCommand = new DbCommand(ret, connection);
            }
            else
            {
                return (null);
            }
          

            return ret;
        }

        public string Get_Select_Command_SQLITE(string table, string[] m_output, string[] m_input_Property, List<DB_Input[]> m_input_Value)
        {
            string ret = "";
            DbCommand oleDbCommand;

            if (table != null)
            {
                //Output Parameter  und Table Festlegen
                if (m_output.Length > 0)
                {
                    ret = "SELECT ";
                    int i1 = 0;
                    do
                    {
                        ret = ret + m_output[i1] + " ";

                        if (i1 != m_output.Length - 1)
                        {
                            ret = ret + ", ";
                        }

                        i1++;
                    } while (i1 < m_output.Length);

                    ret = ret + "FROM " + table + " ";

                }
                else
                {
                    ret = "SELECT * FROM " + table + " ";
                }

                //Input Parameter einfügen
                if (m_input_Property.Length > 0 && m_input_Value.Count > 0 && m_input_Value.Count == m_input_Property.Length)
                {
                    ret = ret + "WHERE ";
                    int i2 = 0;
                    do
                    {
                        //ret = ret + m_input_Property[i2] + " IN (" + m_input_Value[i2] + ") ";

                        ret = ret + m_input_Property[i2] + " IN (";

                        if (m_input_Value[i2].Length > 1)
                        {
                            int i3 = 0;
                            do
                            {
                                ret = ret + "@"+ m_input_Property[i2] + "_1_"+i3;

                                if (i3 < m_input_Value[i2].Length - 1)
                                {
                                    ret = ret + ",";
                                }

                                i3++;
                            } while (i3 < m_input_Value[i2].Length);

                            ret = ret + ") ";
                        }
                        else
                        {
                            ret = ret + "@"+ m_input_Property[i2] + "_1_0)";
                        }


                        if (i2 != m_input_Property.Length - 1)
                        {
                            ret = ret + "AND ";
                        }

                        i2++;
                    } while (i2 < m_input_Property.Length);
                }

                //   oleDbCommand = new DbCommand(ret, connection);
            }
            else
            {
                return (null);
            }


            return ret;
        }
        #endregion SELECT

        #region INSERT
        /// <summary>
        /// Erzeugung eines Insert Befehles für eine einiziege Tabelle
        /// </summary>
        /// <param name="table"></param>
        /// <param name="m_input_Property"></param>
        /// <param name="m_input_Value"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public string Get_Insert_Command(string table, object[] m_input_Property, object[] m_input_Value, Ennumerationen.DB_Type type)
        { 
            string ret = "";
           // OleDbCommand oleDbCommand;

            if (table != null)
            {
               ret = "INSERT INTO "+table+" (";
              
               //Input Parameter einfügen
                if (m_input_Property.Length > 0 && m_input_Value.Length > 0 && m_input_Value.Length == m_input_Property.Length)
                {
                    int i2 = 0;
                    //Property
                    do
                    {
                        if(type == Ennumerationen.DB_Type.ACCDB)
                        {
                            ret = ret + "[" + m_input_Property[i2] + "]";
                        }
                        if (type == Ennumerationen.DB_Type.MSDASQL)
                        {
                            ret = ret + "" + m_input_Property[i2] + "";
                        }

                        // ret = ret + m_input_Property[i2];

                        if (i2 != m_input_Property.Length - 1)
                        {
                            ret = ret + ", ";
                        }

                        i2++;
                    } while (i2 < m_input_Property.Length);

                    ret = ret + ") VALUES (";
                    i2 = 0;
                    //Values einfügen
                    do
                    {
                        // ret = ret + m_input_Value[i2];
                        ret = ret + "?";

                        if (i2 != m_input_Value.Length - 1)
                        {
                            ret = ret + ", ";
                        }

                        i2++;
                    } while (i2 < m_input_Value.Length);

                    ret = ret + ")";
                   // oleDbCommand = new OleDbCommand(ret, connection);
                    return (ret);
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

        public string Get_Insert_Command_SQLITE(string table, object[] m_input_Property, object[] m_input_Value)
        {
            string ret = "";
            // OleDbCommand oleDbCommand;

            if (table != null)
            {
                ret = "INSERT INTO " + table + " (";

                //Input Parameter einfügen
                if (m_input_Property.Length > 0 && m_input_Value.Length > 0 && m_input_Value.Length == m_input_Property.Length)
                {
                    int i2 = 0;
                    //Property
                    do
                    {
                       
                        ret = ret + "" + m_input_Property[i2] + "";
                        

                        // ret = ret + m_input_Property[i2];

                        if (i2 != m_input_Property.Length - 1)
                        {
                            ret = ret + ", ";
                        }

                        i2++;
                    } while (i2 < m_input_Property.Length);

                    ret = ret + ") VALUES (";
                    i2 = 0;
                    //Values einfügen
                    do
                    {
                        // ret = ret + m_input_Value[i2];
                        ret = ret + "@" + m_input_Property[i2] +"_1_0";

                        if (i2 != m_input_Value.Length - 1)
                        {
                            ret = ret + ", ";
                        }

                        i2++;
                    } while (i2 < m_input_Value.Length);

                    ret = ret + ")";
                    // oleDbCommand = new OleDbCommand(ret, connection);
                    return (ret);
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
        #endregion

        #region UPDATE
        /// <summary>
        /// Erzeugung eines Update Befehls für eine Tabelle
        /// </summary>
        /// <param name="table"></param>
        /// <param name="m_input_Property"></param>
        /// <param name="m_input_Value"></param>
        /// <param name="m_select_property"></param>
        /// <param name="m_select_value"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public string Get_Update_Command(string table, object[] m_input_Property, object[] m_input_Value, object[] m_select_property, List<object[]> m_select_value, Ennumerationen.DB_Type type)
        {
            string ret = "";

            //OleDbCommand oleDbCommand;

            if (table != null)
            {
                ret = "UPDATE "+ table + " SET ";

                //Input Parameter einfügen
                if (m_input_Property.Length > 0 && m_input_Value.Length > 0 && m_input_Value.Length == m_input_Property.Length)
                {
                    int i2 = 0;
                    //Property
                    do
                    {
                        if(type == Ennumerationen.DB_Type.ACCDB)
                        {
                            ret = ret + "[" + m_input_Property[i2] + "] = ?";
                        }
                        if (type == Ennumerationen.DB_Type.MSDASQL)
                        {
                            ret = ret + "" + m_input_Property[i2] + " = ?";
                        }


                        if (i2 != m_input_Property.Length - 1)
                        {
                            ret = ret + ", ";
                        }

                        i2++;
                    } while (i2 < m_input_Property.Length);

                    ret = ret + " WHERE ";
                    i2 = 0;
                    //Select Parameter und Values einfügen
                    do
                    {
                        ret = ret + m_select_property[i2]+" IN (";

                        if(m_select_value[i2].Length > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                ret = ret + "@"+ m_select_property[i2]+"_"+i2+"_"+i3;
                                if(i3 < m_select_value[i2].Length-1)
                                {
                                    ret = ret + ",";
                                }

                                i3++;
                            } while (i3 < m_select_value[i2].Length);
                        }

                        ret = ret + ") ";

                        if(i2 < m_select_property.Length-1)
                        {
                            ret = ret + "AND ";
                        }

                        i2++;
                    } while (i2 < m_select_property.Length);

                    
                  //  oleDbCommand = new OleDbCommand(ret, connection);
                    return (ret);
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

        public string Get_Update_Command_SQLITE(string table, object[] m_input_Property, object[] m_input_Value, object[] m_select_property, List<object[]> m_select_value)
        {
            string ret = "";

            //OleDbCommand oleDbCommand;

            if (table != null)
            {
                ret = "UPDATE " + table + " SET ";

                //Input Parameter einfügen
                if (m_input_Property.Length > 0 && m_input_Value.Length > 0 && m_input_Value.Length == m_input_Property.Length)
                {
                    int i2 = 0;
                    //Property
                    do
                    {
                       
                         ret = ret + "" + m_input_Property[i2] + " = @" + m_input_Property[i2] +"_1_"+i2;
                        


                        if (i2 != m_input_Property.Length - 1)
                        {
                            ret = ret + ", ";
                        }

                        i2++;
                    } while (i2 < m_input_Property.Length);

                    ret = ret + " WHERE ";
                    i2 = 0;
                    //Select Parameter und Values einfügen
                    do
                    {
                        ret = ret + m_select_property[i2] + " IN (";

                        if (m_select_value[i2].Length > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                ret = ret + "@" + m_select_property[i2]+"_1_"+i3;
                                if (i3 < m_select_value[i2].Length - 1)
                                {
                                    ret = ret + ",";
                                }

                                i3++;
                            } while (i3 < m_select_value[i2].Length);
                        }

                        ret = ret + ") ";

                        if (i2 < m_select_property.Length - 1)
                        {
                            ret = ret + "AND ";
                        }

                        i2++;
                    } while (i2 < m_select_property.Length);


                    //  oleDbCommand = new OleDbCommand(ret, connection);
                    return (ret);
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
        #endregion UPDATE

        #region DELETE
        public string Get_Delete_Command(string table, object[] m_input_Property, List<object[]> m_input_Value)
        {
            string ret = "";
           // OleDbCommand oleDbCommand;

            if (table != null)
            {
                ret = "DELETE FROM " + table + " WHERE ";

                //Input Parameter einfügen
                if (m_input_Property.Length > 0 && m_input_Value.Count > 0 && m_input_Value.Count == m_input_Property.Length)
                {  
                    int i2 = 0;
                    do
                    {
                        //ret = ret + m_input_Property[i2] + " IN (" + m_input_Value[i2] + ") ";

                        ret = ret + table+".["+m_input_Property[i2]+"]" + " IN (";

                        if (m_input_Value[i2].Length > 1)
                        {
                            int i3 = 0;
                            do
                            {
                                ret = ret + "?";

                                if (i3 < m_input_Value[i2].Length - 1)
                                {
                                    ret = ret + ",";
                                }

                                i3++;
                            } while (i3 < m_input_Value[i2].Length);

                            ret = ret + ") ";
                        }
                        else
                        {
                            ret = ret + "?) ";
                        }


                        if (i2 != m_input_Property.Length - 1)
                        {
                            ret = ret + "AND ";
                        }

                        i2++;
                    } while (i2 < m_input_Property.Length);
                }

                //  oleDbCommand = new OleDbCommand(ret, connection);
                return ret;
            }
            else
            {
                return (null);
            }

            return (null);
           
        }
        #endregion DELETE

        #region Custom
        public string Add_Parameters_Pre(object[] m_value)
        {
            string ret = "";
            int lauf = 0;
            if(m_value.Length > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_value[i1] != null)
                    {
                        if (lauf == 0)
                        {
                            lauf++;

                           
                                ret = ret + "?";
                            
                           
                        }
                        else
                        {
                           
                                ret = ret + ",?";
                            
                        }
                    }
                   
                    i1++;
                } while (i1 < m_value.Length);

                return ret;
            }

            return null;
            
        }

        public string Add_Parameters_Pre_Integer(int[] m_value)
        {
            string ret = "";
            int lauf = 0;
            if (m_value.Length > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_value[i1] != null)
                    {
                        if (lauf == 0)
                        {
                            lauf++;

                            ret = ret + "?";
                        }
                        else
                        {
                            ret = ret + ",?";
                        }
                    }

                    i1++;
                } while (i1 < m_value.Length);

                return ret;
            }

            return null;

        }

        public string Add_SQLiteParameters_Pre(object[] m_value, string name)
        {
            string ret = "";
            int lauf = 0;
            if (m_value.Length > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_value[i1] != null)
                    {
                        if (lauf == 0)
                        {
                            lauf++;

                            
                                ret = ret + "@" + name + "_" + lauf + "_" + i1;
                          

                        }
                        else
                        {
                            
                                ret = ret + ",@" + name + "_" + lauf + "_" + i1;
                           
                        }
                    }

                    i1++;
                } while (i1 < m_value.Length);

                return ret;
            }

            return null;

        }

        public string Add_SQLiteParameters_Pre_Integer(int[] m_value, string name)
        {
            /* string ret = "";
             int lauf = 0;
             if (m_value.Length > 0)
             {
                 int i1 = 0;
                 do
                 {
                     if (m_value[i1] != null)
                     {
                         if (lauf == 0)
                         {
                             lauf++;

                             ret = ret + "?";
                         }
                         else
                         {
                             ret = ret + ",?";
                         }
                     }

                     i1++;
                 } while (i1 < m_value.Length);

                 return ret;
             }

             return null;*/
            string ret = "";
            int lauf = 0;
            if (m_value.Length > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_value[i1] != null)
                    {
                        if (lauf == 0)
                        {
                            lauf++;


                            ret = ret + "@" + name + "_" + lauf + "_" + i1;


                        }
                        else
                        {

                            ret = ret + ",@" + name + "_" + lauf + "_" + i1;

                        }
                    }

                    i1++;
                } while (i1 < m_value.Length);

                return ret;
            }
            return null;
        }
        #endregion
    }


}

using System;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.Odbc;
using Database_Connection;

using Requirement_Plugin;

namespace Requirement_Plugin.xml
{

    public class XML
    {

        public XML()
        {

        }

       

        #region Read XML
        /// <summary>
        /// Es wird aus einem xml-String ein Attribut ausgelesen
        /// </summary>
        /// <param name="Get_Attribut"></param>
        /// <param name="xml_String"></param>
        /// <returns></returns>
   /*     public List<string> Xml_Read_Attribut(string Get_Attribut, string xml_String)
        {
            //  MessageBox.Show("xml_Read1");
            //  MessageBox.Show(xml_String);

            XmlDocument xml_Dat = new XmlDocument();
            List<string> List_Attribut = new List<string>();

            if (xml_String != null)
            {

                xml_Dat.LoadXml(xml_String);


                var Node_List = xml_Dat.GetElementsByTagName("Row");
                //    MessageBox.Show(Node_List.Count.ToString());
                if (Node_List.Count > 0)
                {
                    var i = 0;
                    var Node = Node_List.Item(i);



                    if (Node != null)
                    {
                        bool flag = true;

                        do
                        {

                            i++;
                            var Attribut = Node.FirstChild;

                            string show = Attribut.InnerText;
                            List_Attribut.Add(show);

                            //  MessageBox.Show(show);

                            Node = Node_List.Item(i);

                            if (Node == null)
                            {
                                flag = false;
                            }

                        } while (flag == true);

                        return (List_Attribut);
                    }
                    else
                    {
                        return (null);
                    }
                }
            }
            return (null);
        }
        */
        #endregion Read XML

        #region Built SQL 
        /// <summary>
        /// Es wird die Liste so aufbereitet, dass der SQL .. IN(..) ausgeführt werden kann
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public string SQL_IN_Array(string[] array)
        {
            string ret = "";
            if (array != null)
            {
                if (array.Length > 0)
                {
                    ret = "(";

                    int i1 = 0;
                    do
                    {
                        if (array[i1] != null)
                        {
                            if (array[i1] != "")
                            {
                                ret = ret + "'" + array[i1] + "'";

                                if (i1 + 1 < array.Length)
                                {
                                    if (array[i1 + 1] != "")
                                    {
                                        ret = ret + ",";
                                    }

                                }
                            }

                        }



                        i1++;
                    } while (i1 < array.Length);

                    ret = ret + ")";
                }
            }

            return (ret);
        }
        /// <summary>
        /// Es wird die Liste so aufbereitet, dass der SQL .. IN(..) ausgeführt werden kann
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public string SQL_IN_Array_Num(string[] array)
        {
            string ret = "";
            if (array != null)
            {
                if (array.Length > 0)
                {
                    ret = "(";

                    int i1 = 0;
                    do
                    {
                        ret = ret + array[i1];

                        if (i1 + 1 < array.Length)
                        {
                            ret = ret + ",";
                        }

                        i1++;
                    } while (i1 < array.Length);

                    ret = ret + ")";
                }
            }

            return (ret);
        }
        #endregion Built SQL

       

        #region Sonderzeichen
        public string Change_sonderzeichen(string input, List<Requirement_Plugin.XML.Sonderzeichen> m_Sonder)
        {
            string ret = "";

            int i1 = 0;
            do
            {
                var flag = false;

                do
                {
                    int test = input.IndexOf(m_Sonder[i1].out_);

                    if (test == -1 || test == 0)
                    {

                        flag = true;
                    }
                    else
                    {
                        string one = "";
                        one = input.Substring(0, test);

                        string t = one[test - 1].ToString();

                        string two = input.Substring(test + 10);
                        two = m_Sonder[i1].in_ + two;
                        input = one + two;
                    }

                } while (flag == false);

                i1++;
            } while (i1 < m_Sonder.Count);

            ret = input;

            return (ret);

      
        }
        #endregion

        /* Artefakt welches später entfernt werden kann
        public List<string> Get_Target_Classifier(string NodeType_GUID, EA.Repository Repository, Database Data)
        {
            XML xml = new XML();

            string SQL = "SELECT t_object.ea_guid FROM t_object WHERE t_object.[ea_guid] IN " +
                "(SELECT t_object.PDATA1 FROM t_object WHERE t_object.[Object_ID] IN" +
                "   (SELECT t_connector.End_Object_ID FROM t_connector WHERE t_connector.[Start_Object_ID] IN" +
                "       (SELECT A.Object_ID FROM t_object A, t_object B WHERE A.PDATA1 = '" + NodeType_GUID + "') AND t_connector.Connector_Type IN" + xml.SQL_IN_Array(Data.metamodel.OpArch_InfoEx_Type) + " AND Stereotype IN" + xml.SQL_IN_Array(Data.metamodel.OpArch_InfoEx_Stereotype) + "));";

            string xml_String = Repository.SQLQuery(SQL);

            return (Xml_Read_Attribut("ea_guid", xml_String));
        }
        */

    }

}

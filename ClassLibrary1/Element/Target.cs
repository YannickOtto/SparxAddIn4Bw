using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;

using Repsoitory_Elements;
using Requirements;
using Requirement_Plugin;

namespace Elements
{
    public class Target : Repository_Connector
    {

        public Logical Logical;
        public List<InformationElement> m_Information_Element;
        //public List<InformationElement> m_Information_Element;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Client_ID"></param>
        /// <param name="Supplier_ID"></param>
        public Target(string Client_ID, string Supplier_ID, Database Data)
        {
            //MessageBox.Show("Target anlegen");

            this.CLient_ID = Client_ID;
            this.Supplier_ID = Supplier_ID;
            this.m_Information_Element = new List<InformationElement>();

            if(Client_ID != null)
            {

                this.Logical = this.Get_LA(this.CLient_ID, Data);
            }


        }

        ~Target()
        {

        }

        #region Get Elements


        //Prüft, ob für das aktuelle Target Requirement_Interface vorlegen Es wird vom Client und vom Supplier zurückgegeben
        /// <summary>
        /// Prüfung ob Requirement vorhanden
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Data"></param>
        /// <param name="flag_NT"></param>
        /// <param name="W_PROZESSWORT"></param>
        /// <returns></returns>
        public List<List<Requirement_Interface>> Check_Requirement_Interface(EA.Repository Repository, Database Data, bool flag_NT, string W_PROZESSWORT)
        {
            bool flag_Check = true;
            List<List<Requirement_Interface>> m_m_ret = new List<List<Requirement_Interface>>();


            Interface_Connectors_Requirement interface_Connectors = new Interface_Connectors_Requirement();

            #region uml Abfrage 
            List<string> Connector_GUID = new List<string>();
            List<string> m_Type_reqint = Data.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList();
            List<string> m_Stereotype_reqint = Data.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList();

            List<string> m_Type_DerivedElem = Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
            List<string> m_Stereotype_DerivedElem = Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Con = Data.metamodel.m_Afo_Requires.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Con = Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList();

            Connector_GUID = interface_Connectors.Check_Connector_Requirement_Interface(Data, this.CLient_ID, this.Supplier_ID, m_Type_reqint, m_Stereotype_reqint, m_Type_DerivedElem, m_Stereotype_DerivedElem, m_Type_Con, m_Stereotype_Con);
            
            #endregion SQL Abfrage
            #region Direkter Connector zwischen Client und Supplier ist vorhanden mit entsprehchendem _Stereotype
            if (Connector_GUID != null)
            {
                // MessageBox.Show("vorhanden Requirements");
                int d1 = 0;
                TaggedValue Tagged_1 = new TaggedValue(Data.metamodel, Data);

                //Schleife über alle Konnektoren welche aus der SQL Abfrage stammen
                List<Requirement_Interface> List_1 = new List<Requirement_Interface>();
                List<Requirement_Interface> List_2 = new List<Requirement_Interface>();
                do
                {
                    EA.Connector Connector = Repository.GetConnectorByGuid(Connector_GUID[d1]);
                    //Hier nur Unidirektionale Requirements zu Beginn
                    if (Tagged_1.Get_Tagged_Value(Repository.GetElementByID(Connector.ClientID).ElementGUID, "W_PROZESSWORT", Repository) == W_PROZESSWORT)
                    {
                        #region Client Requirement
                        EA.Element Requirement = Repository.GetElementByID(Connector.ClientID);
                        Requirement_Interface Requirement_Client = new Requirement_Interface("", "", "", "", "", "", true, "", true, null, Data.metamodel);
                        Requirement_Client.Classifier_ID = Requirement.ElementGUID;
                        Requirement_Client.Get_InformationElement(Repository, Data);
                        Requirement_Client.Get_Tagged_Values_From_Requirement(Requirement.ElementGUID, Repository, Data);
                        //Nach Capability suchen
                        Requirement_Client.Check_Capability(Repository, Data);
                        //Check Issue
                        Requirement_Client.Get_Issues(Data);
                        //Check Klärungspunkte
                        Requirement_Client.Check_Klärungspunkte(Requirement_Client.m_Issues, Data, Repository);


                        Requirement_Client.Add_to_Database(Data);


                        #endregion Client Requirement
                        #region Supplier Requirement
                        // MessageBox.Show( Requirement_Client.W_AKTIVITAET.ToString());
                        Requirement = Repository.GetElementByID(Connector.SupplierID);
                        Requirement_Interface Requirement_Supplier = new Requirement_Interface("", "", "", "", "", "", true, "", true, null, Data.metamodel);
                        Requirement_Supplier.Classifier_ID = Requirement.ElementGUID;
                        Requirement_Supplier.Get_InformationElement(Repository, Data);
                        Requirement_Supplier.Get_Tagged_Values_From_Requirement(Requirement.ElementGUID, Repository, Data);
                        //Nach Capability suchen
                        Requirement_Supplier.Check_Capability(Repository, Data);
                        //Check Issue
                        Requirement_Supplier.Get_Issues(Data);
                        //Check Klärungspunkte
                        Requirement_Supplier.Check_Klärungspunkte(Requirement_Client.m_Issues, Data, Repository);

                        Requirement_Supplier.Add_to_Database(Data);
                        #endregion Supplier Requirement
                        //Check NT
                        if (flag_NT == true)
                        {
                            //Requirement darf nur zu einem Part führen
                            #region Check
                            /*    string SQL_NT = "SELECT ea_guid FROM t_object WHERE Object_Type IN "+xml.SQL_IN_Array(m_Type_Usage.ToArray())+" AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Stereotype IN"+xml.SQL_IN_Array(m_Stereotype_DerivedElem.ToArray())+" AND Start_Object_ID = " + Requirement.ElementID + ");";
                                string xml_dat = Repository.SQLQuery(SQL_NT);

                                List<string> Check_guid = xml.Xml_Read_Attribut("ea_guid", xml_dat);
                                */
                            List<string> Check_guid = new List<string>();

                            /*   string SQL_NT2 = "SELECT ea_guid FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Type_Usage.ToArray()) + ") AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Stereotype IN (" + command.Add_Parameters_Pre(m_Stereotype_DerivedElem.ToArray()) + ") AND Start_Object_ID = ?);";

                               OleDbCommand SELECT_Check = new OleDbCommand(SQL_NT2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);

                               List<DB_Input[]> ee2 = new List<DB_Input[]>();
                               List<int> help_id3 = new List<int>();
                               help_id3.Add(Requirement.ElementID);
                               ee2.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());
                               ee2.Add(help_id3.Select(x => new DB_Input(x, null)).ToArray());
                               ee2.Add(m_Type_Usage.Select(x => new DB_Input(-1, x)).ToArray());

                               OleDbType[] m_input_Type2 = { OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar };
                               Data.oLEDB_Interface.Add_Parameters_Select(SELECT_Check, ee2, m_input_Type2);
                               string[] m_output2 = { "ea_guid" };

                               List<DB_Return> m_ret4 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_Check, m_output2);

                               if (m_ret4[0].Ret.Count > 1)
                               {
                                   Check_guid = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
                               }
                               else
                               {
                                   Check_guid = null;
                               }
                               */
                            List<string> help_id3 = new List<string>();
                            List<string> m_Type_Usage = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
                            List<string> m_Stereotype_Usage = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();
                            help_id3.Add(Requirement.ElementGUID);

                            Check_guid = interface_Connectors.Get_Supplier_Element_By_Connector(Data, help_id3, m_Type_Usage, m_Stereotype_Usage, m_Type_DerivedElem, m_Stereotype_DerivedElem, Data.metamodel.m_Derived_Element[0].direction);

                            #endregion Check

                            if(Check_guid == null)
                            {
                                flag_Check = false;
                            }
                            else if (Check_guid.Count == 0) //Es können hier mehr als 1 auftreten --> 2 Instanzen mit dem selben Konnektor
                            {
                                flag_Check = false;
                            }
                            
                        }
                        else
                        {
                            //Element ist Klasse
                            List<string> Check_guid = new List<string>();
                            List<string> help_id3 = new List<string>();
                            List<string> m_Type_Usage = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
                            List<string> m_Stereotype_Usage = Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
                            help_id3.Add(Requirement.ElementGUID);
                            Check_guid = interface_Connectors.Get_Supplier_Element_By_Connector(Data, help_id3, m_Type_Usage, m_Stereotype_Usage, m_Type_DerivedElem, m_Stereotype_DerivedElem, Data.metamodel.m_Derived_Element[0].direction);

                            

                            if (Check_guid == null)
                            {
                                flag_Check = false;
                            }
                            else if (Check_guid.Count == 0) //Es können hier mehr als 1 auftreten --> 2 Instanzen mit dem selben Konnektor
                            {
                                flag_Check = false;
                            }
                        }

                        if (flag_Check == true)
                        {
                            List_1.Add(Requirement_Client);
                            List_2.Add(Requirement_Supplier);
                        }

                    }

                    d1++;
                } while (d1 < Connector_GUID.Count);


                // MessageBox.Show(List.Count.ToString());
                if (List_1.Count > 0)
                {
                    //List_1.AddRange(List_2);
                    m_m_ret.Add(List_1);
                    m_m_ret.Add(List_2);


                    return (m_m_ret);
                }

            }
            #endregion Direkter Connector vorhanden
         //   #region Indirekter Connecor vorhanden?
            
            return (null);
        }
        #region Archiv
        //  else
        //  {
        /*
        #region SQL Abfrage
        bool flag_Existence = false;
        //Suche nach Requiremnts im Repository, welche wie das aktuelle vom Target wären
        //Alle  Requirement_Interface vom Client
        EA.Element Client = Repository.GetElementByGuid(this.CLient_ID);

        string GUIDS_Client = this.Get_Classifiers_To_LA(this.CLient_ID, Data);


        repository_Element.Classifier_ID = this.CLient_ID;

        List<string> PDATA1_Client = new List<string>();
        PDATA1_Client.Add(repository_Element.Get_Classifier_Part(Data));

        //Classifier von dem Supplier
        string GUIDS_Supplier = this.Get_Classifiers_To_LA(this.Supplier_ID, Data);


        repository_Element.Classifier_ID = this.Supplier_ID;

        List<string> PDATA1_Supplier = new List<string>();
        PDATA1_Client.Add(repository_Element.Get_Classifier_Part(Data));
        //Requirments, welche mit dem Supplier verbunden sind
        #region Requirement Supplier

        List<int> GUIDS_Requirement_Client = new List<int>();

        string SQL_obj = "SELECT Object_ID FROM t_object WHERE Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_reqint.ToArray()) + ") AND Object_ID IN(SELECT Start_Object_ID FROM t_connector WHERE End_Object_ID IN (SELECT Object_ID FROM t_object WHERE PDATA1 = ?) AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_DerivedElem.ToArray()) + "))";

        OleDbCommand SELECT_Check = new OleDbCommand(SQL_obj, Data.oLEDB_Interface.dbConnection);

        List<DB_Input[]> ee2 = new List<DB_Input[]>();
        List<string> help_id3 = new List<string>();
        help_id3.Add(PDATA1_Client[0]);
        ee2.Add(help_id3.Select(x => new DB_Input(-1, x)).ToArray());
        ee2.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());
        ee2.Add(m_Stereotype_reqint.Select(x => new DB_Input(-1, x)).ToArray());

        OleDbType[] m_input_Type2 = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
        Data.oLEDB_Interface.Add_Parameters_Select(SELECT_Check, ee2, m_input_Type2);
        string[] m_output2 = { "Object_ID" };

        List<DB_Return> m_ret4 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_Check, m_output2);

        if (m_ret4[0].Ret.Count > 1)
        {
            //List<string> help = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1);
            GUIDS_Requirement_Client = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList();
        }
        else
        {
            GUIDS_Requirement_Client = null;
        }


        #endregion Requirement Supplier

        #endregion SQL Abfrage
        */
        //Schleife über die Requirements des Client
        /*
        if (GUIDS_Requirement_Client != null)
        {
            //      MessageBox.Show(Repository.GetElementByGuid(this.CLient_ID).Name + ": " + GUIDS_Requirement_Client.Count.ToString());
            TaggedValue Tagged = new TaggedValue(Data.metamodel, Data);
            List<Requirement_Interface> List = new List<Requirement_Interface>();
            int i1 = 0;
            do
            {
                //MessageBox.Show("test");

                 string recent_GUID = repository_Element.Get_GUID_By_ID(GUIDS_Requirement_Client[i1], Data);

             //   MessageBox.Show(Tagged.Get_Tagged_Value(recent_GUID[0], "W_PROZESSWORT", Repository));
                // if(GUIDS_Client == GUIDS_Client_Requirement) //Es handelt sich um einen Node, der dem aktuellen ähnelt
                if (Tagged.Get_Tagged_Value(recent_GUID, "W_PROZESSWORT", Repository) == W_PROZESSWORT) 
                {
                    //Schauen ob Connector mit StereoType 'send' welcher Requirement hat welches auf einen Node mit dem Classifier von Supllier_ID zeigt

                    List<int> ID_Requirement_Supplier = repository_Connectors.Get_Connector_Supplier_ID_By_Start_ID(Data, Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList(), GUIDS_Requirement_Client[i1]);


                    if (ID_Requirement_Supplier != null)
                    {
                        //    MessageBox.Show(Repository.GetElementByGuid(this.CLient_ID).Name + " Anzahl 'send': " + ID_Requirement_Supplier.Count.ToString());

                        //Schleife über alle Supplier_Requirements
                        int i2 = 0;

                        do
                        {
                            #region SQL

                            List<string> GUID_Supplier_Req = new List<string>();
                            string SQL31 = "SELECT ea_guid FROM t_object WHERE PDATA1 = ? AND Object_ID IN (SELECT End_Object_ID FROM t_connector WHERE Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype_DerivedElem.ToArray()) + "= AND Start_Object_ID = ?);";

                            OleDbCommand SELECT_31 = new OleDbCommand(SQL31, Data.oLEDB_Interface.dbConnection);

                            List<DB_Input[]> ee31 = new List<DB_Input[]>();
                            List<string> help_id31 = new List<string>();
                            List<int> help_id32 = new List<int>();
                            help_id31.Add(PDATA1_Supplier[0]);
                            help_id32.Add(ID_Requirement_Supplier[i2]);
                            ee31.Add(m_Stereotype_DerivedElem.Select(x => new DB_Input(-1, x)).ToArray());
                            ee31.Add(help_id32.Select(x => new DB_Input(x, null)).ToArray());
                            ee31.Add(help_id31.Select(x => new DB_Input(-1, x)).ToArray());

                            OleDbType[] m_input_Type31 = { OleDbType.VarChar, OleDbType.BigInt, OleDbType.VarChar };
                            Data.oLEDB_Interface.Add_Parameters_Select(SELECT_31, ee31, m_input_Type31);
                            string[] m_output31 = { "ea_guid" };

                            List<DB_Return> m_ret31 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT_31, m_output31);

                            if (m_ret4[0].Ret.Count > 1)
                            {
                                //List<string> help = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1);
                                GUID_Supplier_Req = m_ret4[0].Ret.GetRange(1, m_ret4[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
                            }
                            else
                            {
                                GUID_Supplier_Req = null;
                            }

                            #endregion SQL



                            if (GUID_Supplier_Req != null)
                            {
                                //    MessageBox.Show(" Anzahl Supplier: " + GUID_Supplier_Req.Count.ToString());

                                int i3 = 0;
                                do
                                {
                                    string GUIDS_Supplier_Req = this.Get_Classifiers_To_LA(GUID_Supplier_Req[i3], Data);

                                    if (GUIDS_Supplier_Req == GUIDS_Supplier)
                                    {
                                        flag_Existence = true;
                                    }

                                    i3++;
                                } while (i3 < GUID_Supplier_Req.Count);

                                if (flag_Existence == true)
                                {


                                    string guid_ci = repository_Element.Get_GUID_By_ID(GUIDS_Requirement_Client[i1], Data);


                                    EA.Element Requirement = Repository.GetElementByGuid(guid_ci);
                                    Requirement_Interface Requirement_Client = new Requirement_Interface("", "", "", "", "", "", true, "", true, null);
                                    Requirement_Client.Classifier_ID = Requirement.ElementGUID;
                                    Requirement_Client.Get_InformationElement(Repository, Data);
                                    Requirement_Client.Get_Tagged_Values_From_Requirement(Requirement.ElementGUID, Repository, Data);
                                    //Nach Capability suchen
                                    Requirement_Client.Check_Capability(Repository, Data);

                                    Requirement_Client.Add_to_Database(Data);
                                    //Requirement_Client.Create_Dependency(this.CLient_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                                    this.Create_Dependency(Requirement_Client.Classifier_ID, this.CLient_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType,  Repository, Data);

                                    // MessageBox.Show( Requirement_Client.W_AKTIVITAET.ToString());
                                    //Requirments, welche mit dem Supplier verbunden sind";
                                    //GUID vom Requirement_Supplier

                                    string guid_sup = repository_Element.Get_GUID_By_ID(ID_Requirement_Supplier[i2], Data);

                                    Requirement = Repository.GetElementByGuid(guid_sup);
                                    Requirement_Interface Requirement_Supplier = new Requirement_Interface("", "", "", "", "", "", true, "", true, null);
                                    Requirement_Supplier.Classifier_ID = Requirement.ElementGUID;
                                    Requirement_Supplier.Get_InformationElement(Repository, Data);
                                    Requirement_Supplier.Get_Tagged_Values_From_Requirement(Requirement.ElementGUID, Repository, Data);
                                    //Nach Capability suchen
                                    Requirement_Supplier.Check_Capability(Repository, Data);
                                    Requirement_Supplier.Add_to_Database(Data);

                                    //Requirement_Supplier.Create_Dependency(this.Supplier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                                    this.Create_Dependency(Requirement_Supplier.Classifier_ID, this.Supplier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data);


                                    List.Add(Requirement_Client);
                                    List.Add(Requirement_Supplier);

                                    // MessageBox.Show(List.Count.ToString());


                                }
                            }

                            i2++;
                        } while (i2 < ID_Requirement_Supplier.Count);


                    }
                }



                i1++;
            } while (i1 < GUIDS_Requirement_Client.Count);
            #endregion Indirekter Connector

            if (List.Count > 0)
            {
                return (List);
            }


        }


    }



    return (null);
}
*/
        #endregion Archiv
        /// <summary>
        /// Es werden alle Classifier identifiziert, bis vom aktuellen Element eine Logical erreicht wird.
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="Repository"></param>
        /// <returns></returns>
        private string Get_Classifiers_To_LA(string GUID, Database Data)
        {
            Repository_Class repository_Element = new Repository_Class();
            string guid = "";

            bool flag = true;

            // EA.Element recent = Repository.GetElementByGuid(GUID);
            repository_Element.Classifier_ID = GUID;

            do
            {
                /*  string SQL = "SELECT PDATA1 FROM t_object WHERE ea_guid = '"+repository_Element.Classifier_ID+"';";
                  XML xml = new XML();
                  string xml_dat = Repository.SQLQuery(SQL);
                  List<string> GUIDS = xml.Xml_Read_Attribut("PDATA1", xml_dat); //KAnn sowohl Senden als Auch empfangen sein
                  */
                string n_GUID = repository_Element.Get_Classifier_Part(Data);

                guid = guid + " " + n_GUID;

                if (repository_Element.Get_Parent_ID(Data) == 0)
                {
                    flag = false;
                }
                else
                {
                    repository_Element.Classifier_ID = repository_Element.Get_Parent_GUID(Data);
                }


            } while (flag == true);


            return (guid);
        }

        private Logical Get_LA(string GUID, Database Data)
        {
            Repository_Class repository_Element = new Repository_Class();
            string guid = "";

            bool flag = true;

            // EA.Element recent = Repository.GetElementByGuid(GUID);
            repository_Element.Classifier_ID = GUID;

            do
            {
                /*  string SQL = "SELECT PDATA1 FROM t_object WHERE ea_guid = '"+repository_Element.Classifier_ID+"';";
                  XML xml = new XML();
                  string xml_dat = Repository.SQLQuery(SQL);
                  List<string> GUIDS = xml.Xml_Read_Attribut("PDATA1", xml_dat); //KAnn sowohl Senden als Auch empfangen sein
                  */
                string n_GUID = repository_Element.Get_Classifier_Part(Data);

               

                if (repository_Element.Get_Parent_ID(Data) == 0)
                {
                    flag = false;
                    guid = repository_Element.Classifier_ID;
                }
                else
                {
                    repository_Element.Classifier_ID = repository_Element.Get_Parent_GUID(Data);
                }


            } while (flag == true);

            if(guid != "" && guid != null)
            {
                List<Logical> m_Logical = Data.m_Logical.Where(x => x.Classifier_ID == guid).ToList();

                if(m_Logical != null)
                {
                    if(m_Logical.Count > 0)
                    {
                        return (m_Logical[0]);
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

            return (null);
        }

        #endregion Get Elements
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Data.OleDb;
using System.Data.Odbc;
using Requirement_Plugin.Interfaces;

using Database_Connection;
using Requirements;
using Repsoitory_Elements;
using Requirement_Plugin;

namespace Elements
{
    public class Element_Interface
    {
      //  public bool Bidirectional;
        public string Classifier_ID;
        public string Target_Classifier_ID;
        public List<Requirement_Interface> m_Requirement_Interface_Send;
        public List<Requirement_Interface> m_Requirement_Interface_Receive;
        public List<Target> m_Target;
        // public System.InformationElement m_InformationElement;


        public NodeType Client;
        public NodeType Supplier;
	//	public List<InformationElement> m_InformationElement;
		public List<Logical> m_Logical_Supplier;
        
        /// <summary>
        /// Constructor Element_Intercafe
        /// </summary>
        /// <param name="Source_Classifier_ID"></param>
        /// <param name="Target_ID"></param>
        public Element_Interface(string Source_Classifier_ID, string Target_ID)
        {
            this.Classifier_ID = Source_Classifier_ID;
            this.Target_Classifier_ID = Target_ID;
            this.m_Target = new List<Target>();
            this.m_Requirement_Interface_Send = new List<Requirement_Interface>();
            this.m_Requirement_Interface_Receive = new List<Requirement_Interface>();
        }

        ~Element_Interface()
        {

        }



        #region Check Elements
        /// <summary>
        /// Alle Targets des Element_Interface erhalten
        /// </summary>
        /// <param name="Source_Classifier_ID"></param>
        /// <param name="Target_ID"></param>
        /// <param name="Repository"></param>
        /// <param name="Database"></param>
        /// <returns></returns>
        public List<Target> Get_Target(string Source_Classifier_ID, string Target_ID, EA.Repository Repository, Database Database)
        {
            //  MessageBox.Show("Target");
         //   XML xml = new XML();
         //   DB_Command command = new DB_Command();
            List<Target> Targets = new List<Target>();
            EA.Connector Connector;
            Repository_Element repository_Element = new Repository_Element();
            //Alle Targets (Nodes), welche mit den Classifiern Instanzen einen InformationFlow haben
        /*    string SQL = "SELECT t_connector.ea_guid FROM t_connector WHERE t_connector.[Start_Object_ID] IN( SELECT t_object.Object_ID FROM t_object WHERE t_object.PDATA1 = '" + Source_Classifier_ID + "' ) AND t_connector.[End_Object_ID] IN( SELECT t_object.Object_ID FROM t_object WHERE t_object.PDATA1 = '" + Target_ID + "' ) AND t_connector.Connector_Type IN" + xml.SQL_IN_Array(Database.metamodel.m_Infoaus.Select(x => x.Type).ToList().ToArray()) + " AND Stereotype IN" + xml.SQL_IN_Array(Database.metamodel.m_Infoaus.Select(x => x.Stereotype).ToList().ToArray()) + ";";

            string xml_String = Repository.SQLQuery(SQL);

            List<string> Targets_GUID = xml.Xml_Read_Attribut("ea_guid", xml_String);
            */
            List<string> Targets_GUID = new List<string>();
            List<string> m_Type = Database.metamodel.m_Infoaus.Select(x => x.Type).ToList();
            List<string> m_Stereotype = Database.metamodel.m_Infoaus.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();
            Targets_GUID = interface_Connectors.Get_Connector_By_PropertyType(Database, Source_Classifier_ID, Target_ID, m_Type, m_Stereotype);
        /*    string SQL2 = "SELECT ea_guid FROM t_connector WHERE t_connector.[Start_Object_ID] IN( SELECT t_object.Object_ID FROM t_object WHERE t_object.PDATA1 = ? ) AND t_connector.[End_Object_ID] IN( SELECT t_object.Object_ID FROM t_object WHERE t_object.PDATA1 = ? ) AND t_connector.Connector_Type IN(" + command.Add_Parameters_Pre(m_Type.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereotype.ToArray()) + ")";

            OleDbCommand SELECT1 = new OleDbCommand(SQL2, (OleDbConnection)Database.oLEDB_Interface.dbConnection);

            List<DB_Input[]> ee = new List<DB_Input[]>();
            List<string> help_guid = new List<string>();
            List<string> help_guid2 = new List<string>();
            help_guid.Add(Source_Classifier_ID);
            help_guid2.Add(Target_ID);
            ee.Add(help_guid.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(help_guid2.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee.Add(m_Stereotype.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            Database.oLEDB_Interface.Add_Parameters_Select(SELECT1, ee, m_input_Type);
            string[] m_output = { "ea_guid" };

            List<DB_Return> m_ret3 = Database.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT1, m_output);

            if (m_ret3[0].Ret.Count > 1)
            {
                Targets_GUID = m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
            }
            else
            {
                Targets_GUID = null;
            }
            */
            //    MessageBox.Show("Targets : " + Targets_GUID.Count.ToString());

            if (Targets_GUID.Count > 0)
            {

                int i2 = 0;
                do
                {
                    // MessageBox.Show("Connector_Guid: " + Targets_GUID[i2]);
                    Connector = Repository.GetConnectorByGuid(Targets_GUID[i2]);

                    ////////////
                    //GUID des Clients finden
                 /*   string SQL_Client = "SELECT t_object.ea_guid FROM t_object WHERE t_object.Object_ID = " + Connector.ClientID + ";";
                    string xml_Client = Repository.SQLQuery(SQL_Client);
                    XML xml_Client2 = new XML();
                    List<string> Client_GUID = xml_Client2.Xml_Read_Attribut("ea_guid", xml_Client);
                    */
                    string Client_GUID = repository_Element.Get_GUID_By_ID(Connector.ClientID, Database);
                    ////////////
                    //GUID des Suppliers finden
                    /*string SQL_Supplier = "SELECT t_object.ea_guid FROM t_object WHERE t_object.Object_ID = " + Connector.SupplierID + ";";
                                        string xml_Supplier = Repository.SQLQuery(SQL_Supplier);
                                        XML xml_Supplier2 = new XML();

                                        List<string> Supplier_GUID = xml_Supplier2.Xml_Read_Attribut("ea_guid", xml_Supplier);
                                        */
                    string Supplier_GUID = repository_Element.Get_GUID_By_ID(Connector.SupplierID, Database);
                    //////////////////



                    ///////////
                    //Target anlegen
                    Target help = new Target(Client_GUID, Supplier_GUID, Database);

                    var test = help.Get_Logical(Client_GUID, Supplier_GUID, Repository, Database);

                    if (test != null)
                    {
                        help.Logical = test;


                        help.m_Information_Element = help.Get_Information_Element_Logical(Repository, Targets_GUID[i2], help.Logical, Database);

                        help.Logical.m_Target.Add(help);

                        Targets.Add(help);
                    }


                    i2++;

                } while (i2 < Targets_GUID.Count);

            }


            return (Targets);


        }
        /// <summary>
        /// Alle InformationElement der Targets des ElementInterface
        /// </summary>
        /// <returns></returns>
        public List<InformationElement> Get_All_Information_Element()
        {
            List<InformationElement> InfoElems = new List<InformationElement>();

            if (this.m_Target.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Target[i1].m_Information_Element.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {


                            if (InfoElems.Contains(this.m_Target[i1].m_Information_Element[i2]) == true)
                            {
                                // MessageBox.Show("Ist enthalten");
                            }
                            else
                            {
                                InfoElems.Add(this.m_Target[i1].m_Information_Element[i2]);
                            }


                            i2++;
                        } while (i2 < this.m_Target[i1].m_Information_Element.Count);

                    }

                    i1++;
                } while (i1 < this.m_Target.Count);
            }

            return (InfoElems);
        }
        /// <summary>
        /// Es wird Element_Interface auf das Target überprüft
        /// </summary>
        /// <param name="Client_ID"></param>
        /// <param name="Supplier_ID"></param>
        /// <returns></returns>
        public Target Check_Target(string Client_ID, string Supplier_ID)
        {
            List<Target> obj_Target = this.m_Target.Where(x => x.CLient_ID == Client_ID && x.Supplier_ID == Supplier_ID).ToList();

            if(obj_Target.Count > 0)
            {
                return (obj_Target[0]);
            }
            else
            {
                return (null);
            }

            if (this.m_Target.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Target[i1].CLient_ID == Client_ID && this.m_Target[i1].Supplier_ID == Supplier_ID)
                    {
                        return (this.m_Target[i1]);
                    }

                    i1++;
                } while (i1 < this.m_Target.Count);
            }

            return null;
        }

        public List<Logical> Get_Logicals_Unidirektional(EA.Repository repository, Database Data)
        {
            List<Logical> m_Logical = new List<Logical>();

            if (this.m_Target.Count > 0)
            {
                int i1 = 0;
                do
                {
                    var recent_logical = this.m_Target[i1].Get_Logical(this.m_Target[i1].CLient_ID, this.m_Target[i1].Supplier_ID, repository, Data);

                    if (recent_logical != null)
                    {
                        m_Logical.Add(recent_logical);
                    }

                    i1++;
                } while (i1 < this.m_Target.Count);
            }

            if (m_Logical.Count > 0)
            {
                return (m_Logical);
            }
            else
            {
                return (null);
            }
        }
        #endregion Check Elements

        #region Create ELements
        /// <summary>
        /// ERhalten Targets für Bidirektionale Connectoren
        /// </summary>
        /// <param name="m_Remove"></param>
        /// <returns></returns>
        public List<Target> Create_Target_Bidirectional(List<InformationElement> m_Remove)
        {
            List<Target> m_Target = new List<Target>();

            if (this.m_Target.Count > 0 && m_Remove.Count > 0)
            {

                //   MessageBox.Show("Activate Create_Target_Bidirectional");
                int i1 = 0;
                do
                {
                    //    MessageBox.Show("Client_ID: " + this.m_Target[i1].CLient_ID + "\r\nSupplier_ID: " + this.m_Target[i1].Supplier_ID);

                    int save = this.m_Target[i1].m_Information_Element.Count;
                    bool flag = false;
                    Target help_Target = new Target(null, null, null);

                    int i2 = 0;
                    do
                    {
                        if (this.m_Target[i1].m_Information_Element.Contains(m_Remove[i2]) == true)
                        {


                            //   MessageBox.Show("ISt hier vorhanden");
                            if (flag == false)
                            {
                                help_Target.CLient_ID = this.m_Target[i1].CLient_ID;
                                help_Target.Supplier_ID = this.m_Target[i1].Supplier_ID;
                                help_Target.Logical = this.m_Target[i1].Logical;
                                flag = true;
                            }

                            //InfoElmem hinzufügen
                            help_Target.m_Information_Element.Add(m_Remove[i2]);
                            //InfoElem aus altem wegnehmen
                            this.m_Target[i1].m_Information_Element.Remove(m_Remove[i2]);


                            //  this.m_Target[i1].m_Information_Element.Remove()

                        }

                        i2++;
                    } while (i2 < m_Remove.Count);

                    if (flag == true)
                    {
                        //  MessageBox.Show("Hier adden");
                        //  MessageBox.Show("Client_ID: " + help_Target.CLient_ID + "\r\nSupplier_ID: " + help_Target.Supplier_ID);
                        m_Target.Add(help_Target);
                        // MessageBox.Show(help_Target.m_Information_Element.Count.ToString());
                    }

                    //Altes Target löschen --> keine unidirektionale mehr vorhanden
                    // MessageBox.Show(save.ToString());

                    if (this.m_Target[i1].m_Information_Element.Count == 0)
                    {
                        this.m_Target.Remove(this.m_Target[i1]);
                        i1--;
                    }


                    i1++;
                } while (i1 < this.m_Target.Count);

                if (m_Target.Count > 0)
                {
                    //  MessageBox.Show("Hier export");
                    return (m_Target);
                }

            }

            return (null);
        }
        /// <summary>
        /// Es werden im Automatischen Modus die Requiremnt des Element_Interface angelegt bzw. erweitert
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Data"></param>
        public void Create_Requirement(EA.Repository Repository, Database Data, string Package_Guid, bool Bidirectional)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();
            Repository_Connector repository_Connector = new Repository_Connector();
            Repository_Class repository_Element = new Repository_Class();
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            List<DB_Insert> m_Insert = new List<DB_Insert>();

            string Package_InfoÜbertragung_ID = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[1], Repository, Data);
            //////////////////////
            //Alle InfoElem erhalten
            List<InformationElement> InfoElem = this.Get_All_Information_Element();
            //String für InfoELem erzeugen
            string InfoElem_string = "nicht spezifiziert";
            if (InfoElem.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (i1 == 0)
                    {
                        InfoElem_string = InfoElem[i1].Get_InformationItem_Name(Repository, Data);
                    }
                    else
                    {
                        InfoElem_string = InfoElem_string + ", " + InfoElem[i1].Get_InformationItem_Name(Repository, Data);
                    }

                    i1++;
                } while (i1 < InfoElem.Count);
            }

            ////////////////////////////////////////////
            //Titel und Text für Sende_AFo festlegen
            //Titel
            string first_Sender = "";
            if (Bidirectional == false)
            {
                first_Sender = Data.metamodel.m_Prozesswort_Interface[0];
            }
            else
            {
                first_Sender = Data.metamodel.m_Prozesswort_Interface[2];
            }
            string Titel_Senden = first_Sender + ": " + this.Client.Get_Name( Data) + " - " + this.Supplier.Get_Name( Data); ;
            Titel_Senden = ti.ToTitleCase(Titel_Senden);
            //Text
            List<string> Senden_AFo = this.Generate_AFo_Text_Send(Repository, Data, InfoElem_string, ti, Bidirectional);
            // MessageBox.Show(Senden_AFo[0]);
            ///////////////////////////////
            //Titel und Text Empfangen_AFo festlegen
            //Titel
            string first_Empfänger = "";
            if (Bidirectional == false)
            {
                first_Empfänger = Data.metamodel.m_Prozesswort_Interface[1];
            }
            else
            {
                first_Empfänger = Data.metamodel.m_Prozesswort_Interface[2];
            }
            string Titel_Empfangen = first_Empfänger + ": " + this.Supplier.Get_Name( Data) + " - " + this.Client.Get_Name(Data);
            Titel_Empfangen = ti.ToTitleCase(Titel_Empfangen);
            //Text
            List<string> Empfangen_AFo = this.Generate_AFo_Text_Receive(Repository, Data, InfoElem_string, ti, Bidirectional);
            //////////////////////////////
            //Betrachtung der Sende_AFo
            if (this.m_Requirement_Interface_Send.Count == 0)
            {
                //neue AFo erstellen
                string Prozesswort_Senden = "";
                if (Bidirectional == false)
                {
                    Prozesswort_Senden = Data.metamodel.m_Prozesswort_Interface[0];
                }
                else
                {
                    Prozesswort_Senden = Data.metamodel.m_Prozesswort_Interface[2];
                }
                Requirement_Interface AFo_Senden = new Requirement_Interface(Titel_Senden, Senden_AFo[0], Senden_AFo[2], Prozesswort_Senden, "", "", true, Senden_AFo[1], true, null, Data.metamodel);
                //in EA_Database erzeugen
                string AFo_Senden_GUID = AFo_Senden.Create_Requirement_Interface(Repository, Package_Guid, Data.metamodel.m_Requirement_Interface[0].Stereotype, Data);
                AFo_Senden.Classifier_ID = AFo_Senden_GUID;
                AFo_Senden.Add_to_Database(Data);
                //  Repository.GetElementByGuid(AFo_Senden_GUID).Notes = Senden_AFo[0];
                this.m_Requirement_Interface_Send.Clear();
                this.m_Requirement_Interface_Send.Add(AFo_Senden);

                if (Bidirectional == true)
                {
                    Element_Interface_Bidirectional help = this.Supplier.Check_Element_Interface_Bidirectional(this.Client);

                    help.m_Requirement_Interface_Receive.Clear();
                    help.m_Requirement_Interface_Receive.Add(AFo_Senden);
                }
            }
            else
            {
                //Updaten Tagged Values Titel, Text, W_object
                TaggedValue Tagged = new TaggedValue(Data.metamodel, Data);

                //     Tagged.Update_Tagged_Value(this.m_Requirement_Interface_Send[0].Classifier_ID, "AFO_TITEL", Titel_Senden, null, Repository);
                //     Tagged.Update_Tagged_Value(this.m_Requirement_Interface_Send[0].Classifier_ID, "AFO_Text", Senden_AFo[0], null, Repository);
                //     Tagged.Update_Tagged_Value(this.m_Requirement_Interface_Send[0].Classifier_ID, "W_OBJEKT", Senden_AFo[2], null, Repository);


                m_Insert.Clear();
                m_Insert.Add(new DB_Insert("AFO_TITEL", OleDbType.VarChar, OdbcType.VarChar, Titel_Senden, -1));
                m_Insert.Add(new DB_Insert("AFO_Text", OleDbType.VarChar, OdbcType.VarChar, Senden_AFo[0], -1));
                m_Insert.Add(new DB_Insert("W_OBJEKT", OleDbType.VarChar, OdbcType.VarChar, Senden_AFo[2], -1));

                this.m_Requirement_Interface_Send[0].Update_TV(m_Insert, Data, Repository);

                //Updaten Attribute Titel, Text
                EA.Element recent_Afo = Repository.GetElementByGuid(this.m_Requirement_Interface_Send[0].Classifier_ID);

                recent_Afo.Name = Titel_Senden;
                recent_Afo.Notes = Senden_AFo[0];
                recent_Afo.Update();
                Repository.GetPackageByGuid(Package_Guid).Update();
            }
            //////////////////////////////
            if (this.m_Requirement_Interface_Receive.Count == 0)
            {
                //neue AFo erstellen
                string Prozesswort_Empfangen = "";
                if (Bidirectional == false)
                {
                    Prozesswort_Empfangen = Data.metamodel.m_Prozesswort_Interface[1];
                }
                else
                {
                    Prozesswort_Empfangen = Data.metamodel.m_Prozesswort_Interface[2];
                }
                Requirement_Interface AFo_Empfangen = new Requirement_Interface(Titel_Empfangen, Empfangen_AFo[0], Empfangen_AFo[2], Prozesswort_Empfangen, "", "", true, Empfangen_AFo[1], true, null, Data.metamodel);
                //in EA_Database erzeugen
                string AFo_Senden_GUID = AFo_Empfangen.Create_Requirement_Interface(Repository, Package_Guid, Data.metamodel.m_Requirement_Interface[0].Stereotype, Data);
                AFo_Empfangen.Classifier_ID = AFo_Senden_GUID;
                AFo_Empfangen.Add_to_Database(Data);
                // Repository.GetElementByGuid(AFo_Senden_GUID).Notes = Empfangen_AFo[0];
                this.m_Requirement_Interface_Receive.Clear();
                this.m_Requirement_Interface_Receive.Add(AFo_Empfangen);

                if (Bidirectional == true)
                {
                    Element_Interface_Bidirectional help = this.Supplier.Check_Element_Interface_Bidirectional(this.Client);

                    help.m_Requirement_Interface_Send.Clear();
                    help.m_Requirement_Interface_Send.Add(AFo_Empfangen);
                }
            }
            else
            {
                //Updaten Tagged Values Titel, Text, W_object
                TaggedValue Tagged = new TaggedValue(Data.metamodel, Data);

          //      Tagged.Update_Tagged_Value(this.m_Requirement_Interface_Receive[0].Classifier_ID, "AFO_TITEL", Titel_Empfangen, null, Repository);
          //      Tagged.Update_Tagged_Value(this.m_Requirement_Interface_Receive[0].Classifier_ID, "AFO_Text", Empfangen_AFo[0], null, Repository);
          //      Tagged.Update_Tagged_Value(this.m_Requirement_Interface_Receive[0].Classifier_ID, "W_OBJEKT", Empfangen_AFo[2], null, Repository);

                m_Insert.Clear();
                m_Insert.Add(new DB_Insert("AFO_TITEL", OleDbType.VarChar, OdbcType.VarChar, Titel_Empfangen, -1));
                m_Insert.Add(new DB_Insert("AFO_Text", OleDbType.VarChar, OdbcType.VarChar, Empfangen_AFo[0], -1));
                m_Insert.Add(new DB_Insert("W_OBJEKT", OleDbType.VarChar, OdbcType.VarChar, Empfangen_AFo[2], -1));

                this.m_Requirement_Interface_Receive[0].Update_TV(m_Insert, Data, Repository);
                //Updaten Attribute Titel, Text
                EA.Element recent_Afo = Repository.GetElementByGuid(this.m_Requirement_Interface_Receive[0].Classifier_ID);

                recent_Afo.Name = Titel_Empfangen;
                recent_Afo.Notes = Empfangen_AFo[0];
                recent_Afo.Update();
                Repository.GetPackageByGuid(Package_Guid).Update();
            }
            /////////////////////////////////
            //Connecktoren hinzufügen
            //'Send' zwischen Sender und Empfänger AFo
            //this.m_Requirement_Interface_Send[0].Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, Data.metamodel.StereoType_Send[0], Data.metamodel.StereoType_Send[1], Repository);
            repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, this.m_Requirement_Interface_Receive[0].Classifier_ID, Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.SubType).ToList()[0], Repository, Data, Data.metamodel.m_Afo_Requires.Select(x => x.Toolbox).ToList()[0], Data.metamodel.m_Afo_Requires[0].direction);

            if (Bidirectional == true) //Wenn Birectional ist dies beidseitig
            {
                //this.m_Requirement_Interface_Receive[0].Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, Data.metamodel.StereoType_Send[0], Data.metamodel.StereoType_Send[1], Repository);
                repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, this.m_Requirement_Interface_Send[0].Classifier_ID, Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.SubType).ToList()[0], Repository, Data, Data.metamodel.m_Afo_Requires.Select(x => x.Toolbox).ToList()[0], Data.metamodel.m_Afo_Requires[0].direction);

            }
            //Derived für alle Nodes
            int i2 = 0;
            //Schelife über alle Targets
            if (this.m_Target.Count > 0)
            {
                i2 = 0;
                do
                {
                    //this.m_Requirement_Interface_Send[0].Create_Dependency(this.m_Target[i2].CLient_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, this.m_Target[i2].CLient_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                    //this.m_Requirement_Interface_Receive[0].Create_Dependency(this.m_Target[i2].Supplier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, this.m_Target[i2].Supplier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                    if (Bidirectional == true)
                    {
                        // this.m_Requirement_Interface_Send[0].Create_Dependency(this.m_Target[i2].Supplier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                        repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, this.m_Target[i2].Supplier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                        //this.m_Requirement_Interface_Receive[0].Create_Dependency(this.m_Target[i2].CLient_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                        repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, this.m_Target[i2].CLient_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                    }

                    i2++;
                } while (i2 < this.m_Target.Count);
            }
            //Connectoren zu den Info Elem

            this.m_Requirement_Interface_Send[0].Get_InformationElement(Repository, Data);
            this.m_Requirement_Interface_Receive[0].Get_InformationElement(Repository, Data);

            if (InfoElem.Count > 0)
            {
                i2 = 0;
                do
                {
                    //this.m_Requirement_Interface_Send[0].Create_Dependency(InfoElem[i2].Classifier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, InfoElem[i2].Classifier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                    //this.m_Requirement_Interface_Receive[0].Create_Dependency(InfoElem[i2].Classifier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, InfoElem[i2].Classifier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                    i2++;
                } while (i2 < InfoElem.Count);
            }



            //Connector zu Capability
            Requirement_Plugin.xml.XML xML = new Requirement_Plugin.xml.XML();
            Interface_XML interface_XML = new Interface_XML();

            List<string> m_Cap_GUID = interface_XML.SQL_Query_Select( "Name", Data.metamodel.m_Capability_Interface[0].DefaultName, "ea_guid", "t_object", Data);

            string Cap_GUID = "";

            if (m_Cap_GUID != null)
            {
                Cap_GUID = m_Cap_GUID[0];
            }
            else
            {
                /*if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                {
                    Data.oLEDB_Interface.dbConnection.Close();
                }*/
                interface_Collection_OleDB.Close_Connection(Data);
                Cap_GUID = repository_Element.Create_Element_Class(Data.metamodel.m_Capability_Interface[0].DefaultName, Data.metamodel.m_Capability[0].Type, Data.metamodel.m_Capability[0].Stereotype, Data.metamodel.m_Capability[0].Toolbox, -1, Package_InfoÜbertragung_ID, Repository, Data.metamodel.m_Capability_Interface[0].DefaultName, Data);
                interface_Collection_OleDB.Open_Connection(Data);
                /*  if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    Data.oLEDB_Interface.dbConnection.Open();
                }*/
            }

            Capability cap_interface = Data.Check_Capability_Database(Cap_GUID);


            if (cap_interface == null)
            {
                cap_interface = new Capability(Cap_GUID, Repository, Data);
                Data.m_Capability.Add(cap_interface);
            }

            //this.m_Requirement_Interface_Send[0].Create_Dependency(Cap_GUID, Data.metamodel.StereoType_Derived[2], Data.metamodel.StereoType_Derived[3], Repository);
            repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, Cap_GUID, Data.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Capability[0].SubType, Repository, Data, Data.metamodel.m_Derived_Capability[0].Toolbox, Data.metamodel.m_Derived_Capability[0].direction);

            //this.m_Requirement_Interface_Receive[0].Create_Dependency(Cap_GUID, Data.metamodel.StereoType_Derived[2], Data.metamodel.StereoType_Derived[3], Repository);
            repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, Cap_GUID, Data.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Capability[0].SubType, Repository, Data, Data.metamodel.m_Derived_Capability[0].Toolbox, Data.metamodel.m_Derived_Capability[0].direction);

            if (this.m_Requirement_Interface_Send[0].m_Capability.Contains(cap_interface) == false)
            {
                this.m_Requirement_Interface_Send[0].m_Capability.Add(cap_interface);
            }
            if (this.m_Requirement_Interface_Receive[0].m_Capability.Contains(cap_interface) == false)
            {
                this.m_Requirement_Interface_Receive[0].m_Capability.Add(cap_interface);
            }
            //this.m_Requirement_Interface_Send[0].m_Capability.Add(cap_interface);
            // this.m_Requirement_Interface_Receive[0].m_Capability.Add(cap_interface);

           // if(Bidirectional == false)
           // {
                Create_Unidirektional_Generalisierung(Data, Repository, Bidirectional);
           // }
          

        }

        public void Create_Unidirektional_Generalisierung(Database database, EA.Repository repository, bool bidirectional)
        {
            Repository_Connector repository_Connector = new Repository_Connector();

          

            if (bidirectional == false)
            {
                #region Generalisierung
                List<NodeType> m_Specify = this.Client.Get_All_Specifies();
                if (m_Specify != null)
                {


                    List<Element_Interface> m_Child_Uni_ElementInterface = m_Specify.SelectMany(x => x.m_Element_Interface.Where(y => y.Supplier == this.Supplier)).ToList();


                    int i1 = 0;
                    do
                    {
                        if (this.m_Requirement_Interface_Send.Count > 0 && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send.Count > 0)
                        {
                            if (this.m_Requirement_Interface_Send[0].Classifier_ID != null && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send[0].Classifier_ID != null)
                            {
                                repository_Connector.Create_Dependency(m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send[0].Classifier_ID, this.m_Requirement_Interface_Send[0].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                            }
                        }

                        if (this.m_Requirement_Interface_Receive.Count > 0 && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive.Count > 0)
                        {
                            if (this.m_Requirement_Interface_Receive[0].Classifier_ID != null && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive[0].Classifier_ID != null)
                            {
                                repository_Connector.Create_Dependency(m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive[0].Classifier_ID, this.m_Requirement_Interface_Receive[0].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                            }
                        }

                        i1++;
                    } while (i1 < m_Child_Uni_ElementInterface.Count);
                }
                #endregion

                #region Generalisierung hoch
                List<NodeType> m_Specified = this.Client.Get_All_SpecifiedBy(database.m_NodeType);

                if (m_Specified != null)
                {
                    List<Element_Interface> m_Child_Uni_ElementInterface = m_Specified.SelectMany(x => x.m_Element_Interface.Where(y => y.Supplier == this.Supplier)).ToList();


                    if(m_Child_Uni_ElementInterface.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            if (this.m_Requirement_Interface_Send.Count > 0 && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send.Count > 0)
                            {
                                if (this.m_Requirement_Interface_Send[0].Classifier_ID != null && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send[0].Classifier_ID != null)
                                {
                                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send[0].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                                }
                            }

                            if (this.m_Requirement_Interface_Receive.Count > 0 && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive.Count > 0)
                            {
                                if (this.m_Requirement_Interface_Receive[0].Classifier_ID != null && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive[0].Classifier_ID != null)
                                {
                                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive[0].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                                }
                            }


                            i1++;
                        } while (i1 < m_Child_Uni_ElementInterface.Count);
                    }

                   
                }
                #endregion
            }
            else
            {
                #region Generalisierung
                List<NodeType> m_Specify = this.Client.Get_All_Specifies();
                if (m_Specify != null)
                {


                    List<Element_Interface_Bidirectional> m_Child_Uni_ElementInterface = m_Specify.SelectMany(x => x.m_Element_Interface_Bidirectional.Where(y => y.Supplier == this.Supplier)).ToList();

                    if(m_Child_Uni_ElementInterface.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            if (this.m_Requirement_Interface_Send.Count > 0 && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send.Count > 0)
                            {
                                if (this.m_Requirement_Interface_Send[0].Classifier_ID != null && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send[0].Classifier_ID != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send[0].Classifier_ID, this.m_Requirement_Interface_Send[0].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                                }
                            }

                            if (this.m_Requirement_Interface_Receive.Count > 0 && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive.Count > 0)
                            {
                                if (this.m_Requirement_Interface_Receive[0].Classifier_ID != null && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive[0].Classifier_ID != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive[0].Classifier_ID, this.m_Requirement_Interface_Receive[0].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                                }
                            }

                            i1++;
                        } while (i1 < m_Child_Uni_ElementInterface.Count);
                    }

                   
                }
                #endregion

                #region Generalisierung hoch
                List<NodeType> m_Specified = this.Client.Get_All_SpecifiedBy(database.m_NodeType);

                if (m_Specified != null)
                {
                    List<Element_Interface_Bidirectional> m_Child_Uni_ElementInterface = m_Specified.SelectMany(x => x.m_Element_Interface_Bidirectional.Where(y => y.Supplier == this.Supplier)).ToList();

                    if(m_Child_Uni_ElementInterface.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            if (this.m_Requirement_Interface_Send.Count > 0 && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send.Count > 0)
                            {
                                if (this.m_Requirement_Interface_Send[0].Classifier_ID != null && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send[0].Classifier_ID != null)
                                {
                                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Send[0].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                                }
                            }

                            if (this.m_Requirement_Interface_Receive.Count > 0 && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive.Count > 0)
                            {
                                if (this.m_Requirement_Interface_Receive[0].Classifier_ID != null && m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive[0].Classifier_ID != null)
                                {
                                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, m_Child_Uni_ElementInterface[i1].m_Requirement_Interface_Receive[0].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                                }
                            }


                            i1++;
                        } while (i1 < m_Child_Uni_ElementInterface.Count);
                    }

                   
                }
                #endregion
            }

        }


        public void Create_Targets(List<string> m_Client_GUID, List<string> m_Supplier_GUID, Database database, EA.Repository repository, string ConnectorGUID)
        {
            if(m_Client_GUID.Count > 0 && m_Supplier_GUID.Count > 0)
            {

                int j1 = 0;
                do
                {
                    int j2 = 0;
                    do
                    {
                        Target target = this.Check_Target(m_Client_GUID[j1], m_Supplier_GUID[j2]);

                        if(target == null)
                        {
                            Target target2 = new Target(m_Client_GUID[j1], m_Supplier_GUID[j2], database);
                            //  target2.m_Information_Element = m_Info_Elem;

                            List<InformationElement> InfoElm1 = target2.Get_Information_Element_Logical(repository, ConnectorGUID, target2.Logical, database);

                            if (InfoElm1 != null)
                            {
                                target2.m_Information_Element.AddRange(InfoElm1);
                            }

                            List<List<Requirement_Interface>> m_Requ_List = target2.Check_Requirement_Interface(repository, database, true, database.metamodel.m_Prozesswort_Interface[0]);

                            if (m_Requ_List != null)
                            {
                                if(m_Requ_List[0].Count > 0)
                                {
                                    int j3 = 0;
                                    do
                                    {
                                        if (this.m_Requirement_Interface_Send.Contains(m_Requ_List[0][j3]) == false)
                                        {
                                            this.m_Requirement_Interface_Send.Add(m_Requ_List[0][j3]);

                                        }

                                        j3++;
                                    } while (j3 < m_Requ_List[0].Count);
                                }

                                if (m_Requ_List[1].Count > 0)
                                {
                                    int j3 = 0;
                                    do
                                    {
                                        if (this.m_Requirement_Interface_Receive.Contains(m_Requ_List[1][j3]) == false)
                                        {
                                            this.m_Requirement_Interface_Receive.Add(m_Requ_List[1][j3]);

                                        }

                                        j3++;
                                    } while (j3 < m_Requ_List[1].Count);
                                }

                              /*  if (this.m_Requirement_Interface_Receive.Contains(Requ_List[1]) == false)
                                {
                                    this.m_Requirement_Interface_Receive.Add(Requ_List[1]);
                                }
                              */

                            }

                            //List<Requirement_Interface> Requ_List = target2.Check_Requirement_Interface(repository, database, true, database.metamodel.m_Prozesswort_Interface[0]);


                            /* if (Requ_List != null)
                             {
                                 //Hier die Listen richtig machen, da Sie mehr als 2 EintrÃ¤ge haben kÃ¶nnen
                                 if (this.m_Requirement_Interface_Send.Contains(Requ_List[0]) == false)
                                 {
                                     this.m_Requirement_Interface_Send.Add(Requ_List[0]);

                                 }
                                 if (this.m_Requirement_Interface_Receive.Contains(Requ_List[1]) == false)
                                 {
                                     this.m_Requirement_Interface_Receive.Add(Requ_List[1]);
                                 }

                             }
                            */

                            this.m_Target.Add(target2);
                        }
                        else
                        {
                            List<InformationElement> InfoElm = target.Get_Information_Element_Logical(repository, ConnectorGUID, target.Logical, database);

                            if (InfoElm != null)
                            {
                                int d4 = 0;
                                do
                                {
                                    if(target.m_Information_Element.Contains(InfoElm[d4]) == false)
                                    {
                                        target.m_Information_Element.Add(InfoElm[d4]);
                                    }
                               //          List<Requirement_Interface> Requ_List = target2.Check_Requirement_Interface(repository, Data, true, Data.metamodel.m_Prozesswort_Interface[0]);
                                 //   target.Add_InformationElement(InfoElm[d4]);
                                    d4++;
                                } while (d4 < InfoElm.Count);
                            }
                        }

                        j2++;
                    } while (j2 < m_Supplier_GUID.Count);

                    j1++;
                } while (j1 < m_Client_GUID.Count);
            }

           


        }
        #endregion Create Elements

        #region Requirements
        /// <summary>
        /// Es wird für das Element_Interface nach dem Maximalprinzip der Anforderungstext des Senders erstellt.
        /// </summary>
        /// <param name="Repository"></param>
        /// <returns></returns>
        private List<string> Generate_AFo_Text_Send(EA.Repository Repository, Database Data, string InfoElem_string, TextInfo ti, bool Bidirectional)
        {
            //Text
            List<string> recent_Text_Senden = new List<string>();

            recent_Text_Senden.Add(ti.ToTitleCase(Client.W_Artikel)); //Artikel
            recent_Text_Senden.Add(" "); //Leer
            recent_Text_Senden.Add(Client.Get_Name( Data)); //NodeType
            if(Client.W_SINGULAR == true)
            {
                recent_Text_Senden.Add(" muss fähig sein, " + Data.metamodel.string_Interface[0] + " ("); //Füll
            }
            else
            {
                recent_Text_Senden.Add(" müssen fähig sein, " + Data.metamodel.string_Interface[0] + " ("); //Füll
            }
           
            recent_Text_Senden.Add(InfoElem_string); //Information Element; werden als nächster Schritt eingefügt
            if (Bidirectional == false)
            {
                recent_Text_Senden.Add(") an "); //Füll2
            }
            else
            {
                recent_Text_Senden.Add(") mit ");
            }
            //Artikel
            int index_Supplier_Artikel = 0;
            int Artikel_ind = 3;
            if (Bidirectional == true)
            {
                Artikel_ind = 6;
            }
            switch (Supplier.W_Artikel)
            {
                case "der":
                    index_Supplier_Artikel = 0;
                    break;
                case "die":
                    index_Supplier_Artikel = 1;
                    break;
                case "das":
                    index_Supplier_Artikel = 2;
                    break;
            }
            recent_Text_Senden.Add(Data.metamodel.Artikel[Artikel_ind + index_Supplier_Artikel]); //Artikel
            recent_Text_Senden.Add(" "); //Leer
            recent_Text_Senden.Add(Supplier.Get_Name(Data)); //Target
            if (Bidirectional == false)
            {
                recent_Text_Senden.Add(" " + Data.metamodel.m_Prozesswort_zu_Interface[0] + "."); //Füll3
            }
            else
            {
                recent_Text_Senden.Add(" " + Data.metamodel.m_Prozesswort_zu_Interface[2] + "."); //Füll3
            }
            //Aus List einen string erzeugen
            string Afo_Text = "";
            if (recent_Text_Senden.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Afo_Text = Afo_Text + recent_Text_Senden[i1];
                    i1++;
                } while (i1 < recent_Text_Senden.Count);
            }
            ///////////////////////
            //W_Object
            string W_Object = Data.metamodel.string_Interface[0] + " (" + recent_Text_Senden[4] + recent_Text_Senden[5] + recent_Text_Senden[6] + recent_Text_Senden[7] + recent_Text_Senden[8];
            ///////////////////////
            //W_Subject
            string W_Subject = recent_Text_Senden[0].ToLower() + recent_Text_Senden[1] + recent_Text_Senden[2];

            List<string> Return = new List<string>();

            Return.Add(Afo_Text);
            Return.Add(W_Subject);
            Return.Add(W_Object);

            return (Return);
        }
        /// <summary>
        /// Es wird für das Element_Interface nach dem Maximalprinzip der Anforderungstext des Empfängers erstellt.
        /// </summary>
        /// <param name="Repository"></param>
        /// <returns></returns>
        private List<string> Generate_AFo_Text_Receive(EA.Repository Repository, Database Data, string InfoElem_string, TextInfo ti, bool Bidirectional)
        {
            //Text
            List<string> recent_Text_Empfangen = new List<string>();

            recent_Text_Empfangen.Add(ti.ToTitleCase(Supplier.W_Artikel)); //Artikel
            recent_Text_Empfangen.Add(" "); //Leer
            recent_Text_Empfangen.Add(Supplier.Get_Name(Data)); //NodeType
            if(Supplier.W_SINGULAR == true)
            {
                recent_Text_Empfangen.Add(" muss fähig sein, " + Data.metamodel.string_Interface[0] + " ("); //Füll
            }
            else
            {
                recent_Text_Empfangen.Add(" müssen fähig sein, " + Data.metamodel.string_Interface[0] + " ("); //Füll
            }
           
            recent_Text_Empfangen.Add(InfoElem_string); //Information Element; werden als nächster Schritt eingefügt
            if (Bidirectional == false)
            {
                recent_Text_Empfangen.Add(") von "); //Füll2
            }
            else
            {
                recent_Text_Empfangen.Add(") mit ");
            }
            //Artikel
            int index_Client_Artikel = 0;
            int Artikel_ind_2 = 6;
            switch (Client.W_Artikel)
            {
                case "der":
                    index_Client_Artikel = 0;
                    break;
                case "die":
                    index_Client_Artikel = 1;
                    break;
                case "das":
                    index_Client_Artikel = 2;
                    break;
            }
            recent_Text_Empfangen.Add(Data.metamodel.Artikel[Artikel_ind_2 + index_Client_Artikel]); //Artikel
            recent_Text_Empfangen.Add(" "); //Leer
            recent_Text_Empfangen.Add(Client.Get_Name( Data)); //Target
            if (Bidirectional == false)
            {
                recent_Text_Empfangen.Add(" " + Data.metamodel.m_Prozesswort_zu_Interface[1] + "."); //Füll3
            }
            else
            {
                recent_Text_Empfangen.Add(" " + Data.metamodel.m_Prozesswort_zu_Interface[2] + "."); //Füll3
            }
            //Aus List einen string erzeugen
            string Afo_Text = "";
            if (recent_Text_Empfangen.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Afo_Text = Afo_Text + recent_Text_Empfangen[i1];
                    i1++;
                } while (i1 < recent_Text_Empfangen.Count);
            }
            ///////////////////////
            //W_Object
            string W_Object = Data.metamodel.string_Interface[0] + " (" + recent_Text_Empfangen[4] + recent_Text_Empfangen[5] + recent_Text_Empfangen[6] + recent_Text_Empfangen[7] + recent_Text_Empfangen[8];
            ///////////////////////
            //W_Subject
            string W_Subject = recent_Text_Empfangen[0].ToLower() + recent_Text_Empfangen[1] + recent_Text_Empfangen[2];

            List<string> Return = new List<string>();

            Return.Add(Afo_Text);
            Return.Add(W_Subject);
            Return.Add(W_Object);

            return (Return);
        }

        public void Update_Connectoren_Requirement_Interface(EA.Repository Repository, Database Data, bool Bidirectional)
        {
            if(this.m_Requirement_Interface_Receive.Count > 0 && this.m_Requirement_Interface_Send.Count > 0)
            {
                Repository_Connector repository_Connector = new Repository_Connector();
                /////////////////////////////////
                //Connecktoren hinzufügen
                //'Send' zwischen Sender und Empfänger AFo
                //this.m_Requirement_Interface_Send[0].Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, Data.metamodel.StereoType_Send[0], Data.metamodel.StereoType_Send[1], Repository);
                repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, this.m_Requirement_Interface_Receive[0].Classifier_ID, Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.SubType).ToList()[0], Repository, Data, Data.metamodel.m_Afo_Requires.Select(x => x.Toolbox).ToList()[0], Data.metamodel.m_Afo_Requires[0].direction);

                if (Bidirectional == true) //Wenn Birectional ist dies beidseitig
                {
                    //this.m_Requirement_Interface_Receive[0].Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, Data.metamodel.StereoType_Send[0], Data.metamodel.StereoType_Send[1], Repository);
                    repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, this.m_Requirement_Interface_Send[0].Classifier_ID, Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.SubType).ToList()[0], Repository, Data, Data.metamodel.m_Afo_Requires.Select(x => x.Toolbox).ToList()[0], Data.metamodel.m_Afo_Requires[0].direction);

                }
                //Derived für alle Nodes
                int i2 = 0;
                //Schelife über alle Targets
                if (this.m_Target.Count > 0)
                {
                    i2 = 0;
                    do
                    {
                        //this.m_Requirement_Interface_Send[0].Create_Dependency(this.m_Target[i2].CLient_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                        repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, this.m_Target[i2].CLient_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                        //this.m_Requirement_Interface_Receive[0].Create_Dependency(this.m_Target[i2].Supplier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                        repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, this.m_Target[i2].Supplier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                        if (Bidirectional == true)
                        {
                            // this.m_Requirement_Interface_Send[0].Create_Dependency(this.m_Target[i2].Supplier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                            repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, this.m_Target[i2].Supplier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                            //this.m_Requirement_Interface_Receive[0].Create_Dependency(this.m_Target[i2].CLient_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                            repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, this.m_Target[i2].CLient_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                        }

                        i2++;
                    } while (i2 < this.m_Target.Count);
                }
                //Connectoren zu den Info Elem

                this.m_Requirement_Interface_Send[0].Get_InformationElement(Repository, Data);
                this.m_Requirement_Interface_Receive[0].Get_InformationElement(Repository, Data);


                List<InformationElement> InfoElem = this.Get_All_Information_Element();
                if (InfoElem.Count > 0)
                {
                    i2 = 0;
                    do
                    {
                        //this.m_Requirement_Interface_Send[0].Create_Dependency(InfoElem[i2].Classifier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                        repository_Connector.Create_Dependency(this.m_Requirement_Interface_Send[0].Classifier_ID, InfoElem[i2].Classifier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                        //this.m_Requirement_Interface_Receive[0].Create_Dependency(InfoElem[i2].Classifier_ID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], Repository);
                        repository_Connector.Create_Dependency(this.m_Requirement_Interface_Receive[0].Classifier_ID, InfoElem[i2].Classifier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, Repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                        i2++;
                    } while (i2 < InfoElem.Count);
                }
            }
            
        }
        #endregion Requirments

        #region Copy
        public Element_Interface Copy_Interface_Unidirektional_Client(NodeType Target, EA.Repository repository, Requirement_Plugin.Database database)
        {
            Element_Interface element_Interface = new Element_Interface(Target.Classifier_ID, this.Target_Classifier_ID);

            element_Interface.Supplier = this.Supplier;
            element_Interface.Client = Target;
            element_Interface.m_Logical_Supplier = this.m_Logical_Supplier;

            if(this.m_Target.Count > 0)
            {
                Target neu = new Target(Target.Classifier_ID, this.Supplier.Classifier_ID, database);

                element_Interface.m_Target.Add(neu);

                int i1 = 0;
                do
                {
                    if(this.m_Target[i1].m_Information_Element.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if(neu.m_Information_Element.Contains(this.m_Target[i1].m_Information_Element[i2])== false)
                            {
                                neu.m_Information_Element.Add(this.m_Target[i1].m_Information_Element[i2]);
                            }

                            i2++;
                        } while (i2 < this.m_Target[i1].m_Information_Element.Count);
                    }

                    i1++;
                } while (i1 < this.m_Target.Count);

                List<List<Requirement_Interface>> m_Requ_List = neu.Check_Requirement_Interface(repository, database, false, database.metamodel.m_Prozesswort_Interface[0]);

                if (m_Requ_List != null)
                {
                    int j1 = 0;
                    do
                    {
                        if (element_Interface.m_Requirement_Interface_Send.Contains(m_Requ_List[0][j1]) == false)
                        {
                            element_Interface.m_Requirement_Interface_Send.Add(m_Requ_List[0][j1]);
                        }

                        j1++;
                    } while (j1 < m_Requ_List[0].Count);
                    int j2 = 0;
                    do
                    {
                        if (element_Interface.m_Requirement_Interface_Receive.Contains(m_Requ_List[1][j2]) == false)
                        {
                            element_Interface.m_Requirement_Interface_Receive.Add(m_Requ_List[1][j2]);
                        }

                        j2++;
                    } while (j2 < m_Requ_List[1].Count);
                }

             /*       if (Requ_List != null)
                {
                    if (element_Interface.m_Requirement_Interface_Send.Contains(Requ_List[0]) == false)
                    {
                        element_Interface.m_Requirement_Interface_Send.Add(Requ_List[0]);
                    }
                    if (element_Interface.m_Requirement_Interface_Receive.Contains(Requ_List[1]) == false)
                    {
                        element_Interface.m_Requirement_Interface_Receive.Add(Requ_List[1]);
                    }
                }*/

               
            }

            

            return (element_Interface);

        }
        
        public void Copy_InformationElement_Client(Element_Interface element_target, Database database)
        {
            List<InformationElement> m_Info = this.Get_All_Information_Element();
            List<InformationElement> m_Info_target = element_target.Get_All_Information_Element();

            List<InformationElement> m_neu = new List<InformationElement>();

            if (m_Info.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_Info_target.Contains(m_Info[i1]) == false && m_neu.Contains(m_Info[i1]) == false)
                    {
                        m_neu.Add(m_Info[i1]);
                    }

                    i1++;
                } while (i1 < m_Info.Count);


                if(m_neu.Count > 0)
                {
                    Target target = new Target(element_target.Client.Classifier_ID, element_target.Supplier.Classifier_ID, database);
                    target.m_Information_Element = m_neu;

                    element_target.m_Target.Add(target);
                }
               
            }

        }

        public Element_Interface Copy_Interface_Unidirektional_Supplier(NodeType Target, EA.Repository repository, Requirement_Plugin.Database database)
        {
            Element_Interface element_Interface = new Element_Interface(this.Classifier_ID, Target.Classifier_ID);
            element_Interface.Client = this.Client;
            element_Interface.Supplier = Target;
            element_Interface.m_Logical_Supplier = this.m_Logical_Supplier;

            if (this.m_Target.Count > 0)
            {
                Target neu = new Target( this.Client.Classifier_ID, Target.Classifier_ID, database);

                element_Interface.m_Target.Add(neu);

                int i1 = 0;
                do
                {
                    if (this.m_Target[i1].m_Information_Element.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (neu.m_Information_Element.Contains(this.m_Target[i1].m_Information_Element[i2]) == false)
                            {
                                neu.m_Information_Element.Add(this.m_Target[i1].m_Information_Element[i2]);
                            }

                            i2++;
                        } while (i2 < this.m_Target[i1].m_Information_Element.Count);
                    }

                    i1++;
                } while (i1 < this.m_Target.Count);

                List<List<Requirement_Interface>> m_Requ_List = neu.Check_Requirement_Interface(repository, database, false, database.metamodel.m_Prozesswort_Interface[0]);

                if (m_Requ_List != null)
                {
                    int j1 = 0;
                    do
                    {
                        if (element_Interface.m_Requirement_Interface_Send.Contains(m_Requ_List[0][j1]) == false)
                        {
                            element_Interface.m_Requirement_Interface_Send.Add(m_Requ_List[0][j1]);
                        }

                        j1++;
                    } while (j1 < m_Requ_List[0].Count);
                    int j2 = 0;
                    do
                    {
                        if (element_Interface.m_Requirement_Interface_Receive.Contains(m_Requ_List[1][j2]) == false)
                        {
                            element_Interface.m_Requirement_Interface_Receive.Add(m_Requ_List[1][j2]);
                        }

                        j2++;
                    } while (j2 < m_Requ_List[1].Count);
                }
                /*  if (Requ_List != null)
                  {
                      if (element_Interface.m_Requirement_Interface_Send.Contains(Requ_List[0]) == false)
                      {
                          element_Interface.m_Requirement_Interface_Send.Add(Requ_List[0]);
                      }
                      if (element_Interface.m_Requirement_Interface_Receive.Contains(Requ_List[1]) == false)
                      {
                          element_Interface.m_Requirement_Interface_Receive.Add(Requ_List[1]);
                      }
                  }*/


            }

            return (element_Interface);

        }
        #endregion

    }


}

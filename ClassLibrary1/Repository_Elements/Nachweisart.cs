using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;
using System.Data.Odbc;

using Database_Connection;
using Metamodels;
using Elements;
using Requirement_Plugin;
using Ennumerationen;
using Requirements;



namespace Repsoitory_Elements
{

    public class Nachweisart  : Repository_Class
    {
        public AFO_WV_NACHWEISART nachweisart;
        public string abnahmekriterium;
        public List<Abnahmekriterium> m_abnahmekriteria;

        public Nachweisart (string id, Database database, EA.Repository repository)
        {
            if(id != null)
            {
                this.m_abnahmekriteria = new List<Abnahmekriterium>();
                this.Classifier_ID = id;
                this.ID = this.Get_Object_ID(database);
                this.Name = this.Get_Name(database);
                this.Notes = this.Get_Notes(database);
                this.Get_TV_Nachweisart(database, repository);
            }
            else
            {
                this.m_abnahmekriteria = new List<Abnahmekriterium>();
            }
            
        }

        #region TV
        public void Get_TV_Nachweisart(Database Data, EA.Repository repository)
        {
            
                TV_Map help;

                TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
                List<DB_Insert> m_Insert = new List<DB_Insert>();

                // string SQL = "SELECT Value, Notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
                //  string[] output = { "Value", "Notes" };

                //   OleDbCommand oleDbCommand = new OleDbCommand(SQL, Data.oLEDB_Interface.dbConnection);
                //  List<DB_Return> m_TV = Data.oLEDB_Interface.oleDB_SELECT_One_Table_Multiple_Property(oleDbCommand, output, Data.metamodel.SYS_Tagged_Values, this.ID);

                Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();

                List<DB_Return> m_TV = interface_TaggedValue.Get_Tagged_Value(Data.metamodel.Nachweisart_Tagged_Values, this.ID, Data);

                if (m_TV != null)
                {
                    string Value = "";
                    string Note = "";
                    string insert = "";
                    bool flag_insert = false;
                    int i1 = 0;
                    do
                    {
                        if (m_TV[i1].Ret.Count > 1)
                        {
                            string recent = "";
                        if (m_TV[i1].Ret[1] == null && m_TV[i1].Ret[2] == null)
                        {
                            flag_insert = true;
                        }
                        /*    if (m_TV[i1].Ret[1] == "" && m_TV[i1].Ret[2] == "")
                            {
                                flag_insert = true;
                            }*/
                        else
                        {
                            if (m_TV[i1].Ret[1].ToString() != "<memo>")
                            {
                                recent = (string)m_TV[i1].Ret[1];
                            }
                            else
                            {
                                recent = (string)m_TV[i1].Ret[2];
                            }

                            switch (m_TV[i1].Ret[0])
                            {
                                case "AFO_ABNAHMEKRITERIUM":
                                    this.abnahmekriterium = recent;
                                    break;
                                case "AFO_WV_NACHWEISART":
                                    help = Data.metamodel.AFO_Tagged_Values.Find(x => x.Name == "AFO_WV_NACHWEISART");
                                    if (recent != help.Default_Value)
                                    {
                                        this.nachweisart = (AFO_WV_NACHWEISART)Data.nACHWEIS_ENUM.Get_Index(Data.AFO_ENUM.AFO_WV_NACHWEISART, recent);

                                    }
                                    else
                                    {
                                        this.nachweisart = (AFO_WV_NACHWEISART)0;

                                    }
                                    break;
                            }
                        }
                        }
                        else
                        {
                            flag_insert = true;
                        }


                        if (flag_insert == true)
                        {
                            switch (m_TV[i1].Ret[0])
                            {
                                case "AFO_ABNAHMEKRITERIUM":
                                    this.abnahmekriterium = this.Name+";"+this.Notes;
                                    m_Insert.Add(new DB_Insert("AFO_ABNAHMEKRITERIUM", OleDbType.VarChar, OdbcType.VarChar, this.abnahmekriterium, -1));
                                    break;
                                case "AFO_WV_NACHWEISART":
                                    this.nachweisart = (AFO_WV_NACHWEISART)0;
                                    m_Insert.Add(new DB_Insert("AFO_WV_NACHWEISART", OleDbType.VarChar, OdbcType.VarChar, Data.AFO_ENUM.AFO_WV_NACHWEISART[0], -1));
                                    break;
                                
                            }

                        }

                        i1++;
                    } while (i1 < m_TV.Count);

                    if (flag_insert == true)
                    {
                        //Insert Befehl TV
                        string[] m_Input_Property = { "Object_ID", "Property", "Value", "Notes", "ea_guid" };
                        interface_TaggedValue.Insert_Tagged_Value(Data, m_Insert, tagged, this.ID, m_Input_Property);

                        //Data.oLEDB_Interface.OLEDB_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, "t_objectproperties", this.ID, m_Input_Property);
                    }
                }
            }
        #endregion

        #region Get
        public void Get_Abnahmekriterien(Database database, EA.Repository repository)
        {
            //Alle Nachweisarten erhalten, welche mit der Anforderung verknüpft sind
            List<string> m_Abnahmekriterium_Type = database.metamodel.m_Abnahmekriterium.Select(x => x.Type).ToList();
            List<string> m_Abnahmekriterium_Stereotype = database.metamodel.m_Abnahmekriterium.Select(x => x.Stereotype).ToList();
            List<string> m_Stereotype_Con = database.metamodel.m_Con_Abnahmekriterium.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Con = database.metamodel.m_Con_Abnahmekriterium.Select(x => x.Type).ToList();
            //List<string> m_Connector_GUID = new List<string>();
            List<string> m_GUID = new List<string>();
            Interface_Connectors interface_Connectors = new Interface_Connectors();
            //m_Connector_GUID = interface_Connectors.Get_Connector_By_Client_GUID(database, this.Classifier_ID, m_Nachweisart_Type, m_Nachweisart_Stereotype, m_Type_Con, m_Stereotype_Con);
            List<string> m_GUID_Supplier = new List<string>();
            m_GUID_Supplier.Add(this.Classifier_ID);

            // m_GUID = interface_Connectors.Get_m_Supplier_By_ClientGUID_And_Connector(database, this.Classifier_ID, m_Type_Con, m_Stereotype_Con);
            m_GUID = interface_Connectors.Get_Client_Element_By_Connector(database, m_GUID_Supplier, m_Abnahmekriterium_Type, m_Abnahmekriterium_Stereotype, m_Type_Con, m_Stereotype_Con);
            if (m_GUID != null)
            {
                if (m_GUID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        // EA.Connector recent_Con = repository.GetConnectorByGuid(m_Connector_GUID[i1]);
                        // EA.Element recent_Ele = repository.GetElementByID(recent_Con.SupplierID);

                        List<Abnahmekriterium> m_help = database.m_Abnahmekriterium.Where(x => x.Classifier_ID == m_GUID[i1]).ToList();

                        if (m_help.Count > 0)
                        {
                            if (this.m_abnahmekriteria.Contains(m_help[0]) == false)
                            {
                                this.m_abnahmekriteria.Add(m_help[0]);

                                this.abnahmekriterium = this.abnahmekriterium + ";" + m_help[0].Name + ";" + m_help[0].Notes;

                            }

                        }
                        else
                        {
                            Abnahmekriterium neu_ = new Abnahmekriterium(m_GUID[i1], database, repository);
                            database.m_Abnahmekriterium.Add(neu_);
                            if (this.m_abnahmekriteria.Contains(neu_) == false)
                            {
                                this.m_abnahmekriteria.Add(neu_);

                                this.abnahmekriterium = this.abnahmekriterium + ";" + neu_.Name + ";" + neu_.Notes;
                            }
                        }



                        i1++;
                    } while (i1 < m_GUID.Count);
                }


            }
        }
        #endregion

        #region Create
        public void Create_Nachweisart(int Parent_ID,string Package_GUID, Database database, EA.Repository repository)
        {
            this.Classifier_ID =  this.Create_Element_Class(this.Name, database.metamodel.m_Nachweisart.Select(x => x.Type).ToList()[0], database.metamodel.m_Nachweisart.Select(x => x.Stereotype).ToList()[0], database.metamodel.m_Nachweisart.Select(x => x.Toolbox).ToList()[0], Parent_ID, Package_GUID, repository, this.abnahmekriterium, database);
            this.ID = this.Get_Object_ID(database);
            this.Update_Abnahmekriterium(repository, database);

        }
        #endregion

        #region Update
        public void Update_Abnahmekriterium(EA.Repository repository, Database database)
        {
            if (this.abnahmekriterium != "")
            {
                List<DB_Insert> m_Insert = new List<DB_Insert>();
                EA.Element element = repository.GetElementByGuid(this.Classifier_ID);
                TaggedValue taggedValue = new TaggedValue(database.metamodel, database);

                m_Insert.Add(new DB_Insert("AFO_ABNAHMEKRITERIUM", OleDbType.VarChar, OdbcType.VarChar, this.abnahmekriterium, -1));
                m_Insert.Add(new DB_Insert("AFO_WV_NACHWEISART", OleDbType.VarChar, OdbcType.VarChar, database.AFO_ENUM.AFO_WV_NACHWEISART[(int)this.nachweisart], -1));

                this.Update_TV(m_Insert, database, repository);


               // element.Notes = abnahmekriterium;
                element.Update();
            }



        }
        #endregion

    }

}

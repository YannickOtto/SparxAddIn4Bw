using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requirement_Plugin.Interfaces;

using System.Data.OleDb;
using System.Data.Odbc;

namespace Repsoitory_Elements
{
    public class Issue : Repository_Class
    {
        //public string Name;
        //public string Note;
        private string Type;
        private string Stereotype;
        public string Con_guid;
        public string Client_GUID;
        public string Supplier_GUID;


        public Issue(Requirement_Plugin.Database database, string Name, string Note, string Package_guid, EA.Repository repository, bool create, string Type_help)
        {


            switch (Type_help)
            {
                case null:
                    Type = database.metamodel.m_Issue[0].Type;
                    Stereotype = database.metamodel.m_Issue[0].Stereotype;
                    break;
                case "Zuordnung":
                    Type = database.metamodel.m_Issue[1].Type;
                    Stereotype = database.metamodel.m_Issue[1].Stereotype;
                    break;
                case "Taxonomy":
                    Type = database.metamodel.m_Issue[2].Type;
                    Stereotype = database.metamodel.m_Issue[2].Stereotype;
                    break;
                case "Beziehung":
                    Type = database.metamodel.m_Issue[3].Type;
                    Stereotype = database.metamodel.m_Issue[3].Stereotype;
                    break;
                case "Klaerungspunkt":
                    Type = database.metamodel.m_Issue[4].Type;
                    Stereotype = database.metamodel.m_Issue[4].Stereotype;
                    break;
                case "AFO_Subject":
                    Type = database.metamodel.m_Issue[5].Type;
                    Stereotype = database.metamodel.m_Issue[5].Stereotype;
                    break;
                case "AFO_Satzschablone":
                    Type = database.metamodel.m_Issue[6].Type;
                    Stereotype = database.metamodel.m_Issue[6].Stereotype;
                    break;
                case "Dopplung":
                    Type = database.metamodel.m_Issue[7].Type;
                    Stereotype = database.metamodel.m_Issue[7].Stereotype;
                    break;
                case "Bewertung":
                    Type = database.metamodel.m_Issue[9].Type;
                    Stereotype = database.metamodel.m_Issue[9].Stereotype;
                    break;
                default:
                    Type = database.metamodel.m_Issue[0].Type;
                    Stereotype = database.metamodel.m_Issue[0].Stereotype;
                    break;
            }

            this.Name = Name;
            this.Notes = Note;
            if (create == true)
            {
                Interface_Element interface_Element = new Interface_Element();

                this.Classifier_ID = interface_Element.Create_Class_Check_DB(database, Name, Package_guid, Note, repository, Type, Stereotype);

                this.ID = this.Get_Object_ID(database);

            }
        }

        public void Insert_TV_Connector(string Value, Requirement_Plugin.Database Data, EA.Repository repository)
        {
            List<Database_Connection.DB_Insert> m_Insert = new List<Database_Connection.DB_Insert>();
            m_Insert.Add(new Database_Connection.DB_Insert("Connector_GUID", OleDbType.VarChar, OdbcType.VarChar, Value, -1));
            this.Update_TV(m_Insert, Data, repository);
        }


        public void Get_TV_Connector(Requirement_Plugin.Database Data)
        {
            Metamodels.TV_Map help;
            this.Con_guid = "";

            if (this.Classifier_ID != null)
            {
                this.ID = this.Get_Object_ID(Data);


                List<Metamodels.TV_Map> m_help = new List<Metamodels.TV_Map>();
                m_help.Add(new Metamodels.TV_Map("Connector_GUID", OleDbType.VarChar, Data.metamodel.Get_XAC_Import("Connector_GUID"), "not set"));


                TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
                List<Database_Connection.DB_Insert> m_Insert = new List<Database_Connection.DB_Insert>();

                Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();
                List<Database_Connection.DB_Return> m_TV = interface_TaggedValue.Get_Tagged_Value(m_help, this.ID, Data);

                if (m_TV != null)
                {
                    string Value = "";
                    string Note = "";
                    string insert = "";
                    bool flag_insert = false;
                    int i1 = 0;
                    do
                    {

                        flag_insert = false;

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
                                    case "Connector_GUID":
                                        this.Con_guid = recent;

                                        break;
                                }
                            }

                        }


                        i1++;
                    } while (i1 < m_TV.Count);
                }
            }
        }

        public bool Check_Connector(EA.Repository repository, string guid)
        {
            bool ret = false;

            EA.Connector con = repository.GetConnectorByGuid(guid);

            if(con != null)
            {
                ret = true;
            }

            return (ret);
        }
    }
}

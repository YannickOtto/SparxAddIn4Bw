using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;

using Database_Connection;
using Requirement_Plugin;


namespace Repsoitory_Elements
{
    public class OperationalConstraint : Repository_Class
    {
        public List<NodeType> m_NodeType;
        public List<Activity> m_Activity;
        public string W_QUALITAET_ADD = "";

        public OperationalConstraint(string Guid, Database database, EA.Repository repository)
        {
            this.Classifier_ID = Guid;
            this.m_NodeType = new List<NodeType>();
            this.m_Activity = new List<Activity>();

            if(Guid != null && database != null)
            {
                this.ID = this.Get_Object_ID(database);
                this.Name = this.Get_Name(database);
                this.Notes = this.Get_Notes(database);
                W_QUALITAET_ADD = Notes;

                List<DB_Insert> m_Insert = new List<DB_Insert>();

                m_Insert.Add(new DB_Insert("W_QUALITAET_ADD", OleDbType.VarChar, OdbcType.VarChar, this.Notes, -1));

                List<DB_Return> m_Get = this.Get_TV_ret(m_Insert, database);

                if(m_Get == null)
                {
                    this.Insert_TV(m_Insert, database, repository);
                }
                else
                {
                    if (m_Get[0].Ret.Count > 1)
                    {
                        if (m_Insert[0].oleDB_Type == OleDbType.VarChar)
                        {
                            W_QUALITAET_ADD = m_Get[0].Ret[0].ToString();
                        }
                    }
                    else
                    {
                        this.Insert_TV(m_Insert, database, repository);
                    }
                }

            } 
        }

        public List<string> Check_DesignConstraint(Database Data)
        {
            //Alle Startpunkte mit Satisfy_Design erhalten
          //  XML xml = new XML();
            DB_Command command = new DB_Command();
            List<string> GUIDS = new List<string>();
            List<string> TYPES = new List<string>();
            List<string> STEREOTYPES = new List<string>();
            List<string> m_ret = new List<string>();

            List<string> m_Type_Def = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Def = Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Usage = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Usage = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            List<string> m_Type = m_Type_Def.Concat(m_Type_Usage).ToList();
            List<string> m_Stereotype = m_Stereotype_Def.Concat(m_Stereotype_Usage).ToList();

            List<string> m_Type_con = Data.metamodel.m_Satisfy_Design.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = Data.metamodel.m_Satisfy_Design.Select(x => x.Stereotype).ToList();

          
            Requirement_Plugin.Interfaces.Interface_Connectors interface_Connectors = new Requirement_Plugin.Interfaces.Interface_Connectors();

            List<DB_Return> m_ret3 = interface_Connectors.Get_m_Client_From_Supplier(Data, this.Get_Object_ID(Data), m_Type_con, m_Stereotype_con);

            if (m_ret3[0].Ret.Count > 1)
            {
                GUIDS = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                GUIDS = (null);
            }
            if (m_ret3[1].Ret.Count > 1)
            {
                TYPES = (m_ret3[1].Ret.GetRange(1, m_ret3[1].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                TYPES = (null);
            }
            if (m_ret3[2].Ret.Count > 1)
            {
                STEREOTYPES = (m_ret3[2].Ret.GetRange(1, m_ret3[2].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                STEREOTYPES = (null);
            }

            if (GUIDS != null && TYPES != null && STEREOTYPES != null)
            {
                //Schelife über alle Elemente
                int i1 = 0;
                do
                {
                    if(m_Type.Contains(TYPES[i1]) == false || m_Stereotype.Contains(STEREOTYPES[i1]) == false)
                    {
                        m_ret.Add(GUIDS[i1]);
                    }

                    i1++;
                } while (i1 < GUIDS.Count);
            }

            if (m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);
        }

        public List<string> Check_UmweltConstraint(Database Data)
        {
            //Alle Startpunkte mit Satisfy_Design erhalten
           // XML xml = new XML();
            DB_Command command = new DB_Command();
            List<string> GUIDS = new List<string>();
            List<string> TYPES = new List<string>();
            List<string> STEREOTYPES = new List<string>();
            List<string> m_ret = new List<string>();

            List<string> m_Type_Def = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Def = Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

            List<string> m_Type_Usage = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Usage = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            List<string> m_Type = m_Type_Def.Concat(m_Type_Usage).ToList();
            List<string> m_Stereotype = m_Stereotype_Def.Concat(m_Stereotype_Usage).ToList();

            List<string> m_Type_con = Data.metamodel.m_Satisfy_Umwelt.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = Data.metamodel.m_Satisfy_Umwelt.Select(x => x.Stereotype).ToList();


            Requirement_Plugin.Interfaces.Interface_Connectors interface_Connectors = new Requirement_Plugin.Interfaces.Interface_Connectors();

            List<DB_Return> m_ret3 = interface_Connectors.Get_m_Client_From_Supplier(Data, this.Get_Object_ID(Data), m_Type_con, m_Stereotype_con);

            if (m_ret3[0].Ret.Count > 1)
            {
                GUIDS = (m_ret3[0].Ret.GetRange(1, m_ret3[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                GUIDS = (null);
            }
            if (m_ret3[1].Ret.Count > 1)
            {
                TYPES = (m_ret3[1].Ret.GetRange(1, m_ret3[1].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                TYPES = (null);
            }
            if (m_ret3[2].Ret.Count > 1)
            {
                STEREOTYPES = (m_ret3[2].Ret.GetRange(1, m_ret3[2].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList());
            }
            else
            {
                STEREOTYPES = (null);
            }

            if (GUIDS != null && TYPES != null && STEREOTYPES != null)
            {
                //Schelife über alle Elemente
                int i1 = 0;
                do
                {
                    if (m_Type.Contains(TYPES[i1]) == false || m_Stereotype.Contains(STEREOTYPES[i1]) == false)
                    {
                        m_ret.Add(GUIDS[i1]);
                    }

                    i1++;
                } while (i1 < GUIDS.Count);
            }

            if (m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);
        }

        public void Get_Begruendung_Variante2(Activity activity)
        {
            string Titel = "Prozessbegründung '" + activity.W_Prozesswort + " " + activity.W_Object + "'";
            string Text = "Um den Prozess durchzuführen, werden folgende Prozessschritte durch das Projekt durchgeführt: ";
            
            if(activity.m_Child.Count > 0)
            {
                Text = Text + "<ul>";
                int i1 = 0;
                do
                {
                    Text = Text+ "<li>" + activity.m_Child[i1].W_Prozesswort+ " " + activity.m_Child[i1].W_Object + "</li>";
                 
                    i1++;
                } while (i1 < activity.m_Child.Count);
                Text = Text + "</ul>";
            }

            this.Name = Titel;
            this.Notes = Text;

        }
        public void Get_Begruendung_Variante1(List<Activity> m_activity, Requirements.Requirement requirement)
        {
         

            string Titel = "Begründung <<" + requirement.Name + ">>";
            string Text = "";

            bool flag = false;

            if(m_activity.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_activity[i1].m_Parent.Count > 0)
                    {
                        string text_help = (i1+1).ToString()+".) Der Teilprozess "+ m_activity[i1].Name+ " wird für folgende Prozesse benötigt:";

                        text_help = text_help + "<ul>";

                        int i2 = 0;
                        do
                        {
                            text_help = text_help + "<li>" + m_activity[i1].m_Parent[i2].W_Prozesswort + " " + m_activity[i1].m_Parent[i2].W_Object + "</li>";

                            i2++;
                        } while (i2 < m_activity[i1].m_Parent.Count);

                        text_help = text_help + "</ul>";

                        Text = Text + text_help;
                    }
                     i1++;
                } while (i1 < m_activity.Count);
            }

            this.Name = Titel;
            this.Notes = Text;

        }


    }
}

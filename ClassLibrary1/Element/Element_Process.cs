using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requirement_Plugin.Interfaces;

using Database_Connection;
using Repsoitory_Elements;
using Requirements;
using System.Globalization;

namespace Elements
{
    public class Element_Process
    {
        public OperationalConstraint OpConstraint;
        public Activity activity;
        public Requirement_Non_Functional Requirement_Process;
        public Capability capability;
        public List<string> m_GUID_Client;

        public List<string> m_Action_ID;
        public List<string> m_Node_ID;

        public bool flag_Activity;

        public Element_Process(Activity recent, OperationalConstraint opcon, bool flag_activity)
        {
            this.activity = recent;
            activity.m_Process.Add(this);
            this.OpConstraint = opcon;
            m_GUID_Client = new List<string>();

            m_Action_ID = new List<string>();
            m_Node_ID = new List<string>();
             flag_Activity = flag_activity;
            //  this.OpConstraint.m_NodeType.Add(recent);

        }

        #region Check
        public void Check_Requirement_Process(Requirement_Plugin.Database database, EA.Repository repository)
        {
            Interface_Connectors_Requirement interface_Connectors = new Interface_Connectors_Requirement();

            List<string> m_Type_req_process = database.metamodel.m_Requirement_Process.Select(x => x.Type).ToList();
            List<string> m_Stereotype_req_rocess = database.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList();
            List<string> m_Type_DerivedElem = database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
            List<string> m_Stereotype_DerivedElem = database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Derivedconstraint = database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Derivedconstraint = database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList();
            List<string> Requ = new List<string>();
            List<string> OpCon_Requ = new List<string>();
            List<string> m_help = new List<string>();
            List<string> Activity_Requ = new List<string>();
            List<string> NT_Requ = new List<string>();
             

            if (this.m_Action_ID.Count > 0 && this.m_Node_ID.Count > 0) //Wenn m_GUID.Count == 0, dann wurde kein Element_Functional zu dem Element angelegt, entsprechend sind nicht alle Bedingungen erfüllt
            {
               

                Activity_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, this.m_Action_ID, m_Type_req_process, m_Stereotype_req_rocess, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);
                //Requirement --> Constraint

                m_help.Add(this.OpConstraint.Classifier_ID);
                OpCon_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, m_help, m_Type_req_process, m_Stereotype_req_rocess, m_Type_Derivedconstraint, m_Stereotype_Derivedconstraint, database.metamodel.m_Derived_Constraint[0].direction);

                //Requirement --> NodeType
                NT_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, this.m_Node_ID, m_Type_req_process, m_Stereotype_req_rocess, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);
            }
            else
            {
                OpCon_Requ = null;
                Activity_Requ = null;
                NT_Requ = null;
            }

            if (OpCon_Requ != null && Activity_Requ != null && NT_Requ != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_act = Activity_Requ.Where(x => x == OpCon_Requ[i1]).ToList();

                    if (m_act.Count > 0) //Kombination vorhanden
                    {


                        int i2 = 0;
                        do
                        {
                            List<string> m_nt = m_act.Where(x => x == NT_Requ[i2]).ToList();

                            if (m_nt.Count > 0)
                            {
                                Requirement_Non_Functional requirement_process = new Requirement_Non_Functional(OpCon_Requ[i1], database.metamodel);
                                requirement_process.Classifier_ID = OpCon_Requ[i1];
                                requirement_process.m_GUID_Sys.AddRange(this.m_Node_ID);
                                requirement_process.m_GUID_OpCon.Add(this.OpConstraint.Classifier_ID);
                                requirement_process.m_GUID_Sys.AddRange(this.m_Action_ID);
                                //this.Requirement_Process = requirement_process;
                                requirement_process.Get_Tagged_Values_From_Requirement(OpCon_Requ[i1], repository, database);

                                bool flag_zu = false;

                                if(this.Requirement_Process == null)
                                {

                                    flag_zu = true;
                                }
                                else if (this.Requirement_Process.Classifier_ID != requirement_process.Classifier_ID)
                                {

                                    flag_zu = true;
                                }

                                if(flag_zu == true)
                                {
                                    //this.Requirement_Process = (requirement_process);
                                    //this.activity.m_Element_Functional[i2].m_Requirement_Process.Add(requirement_process);
                                    this.Requirement_Process = requirement_process;

                                    if (database.m_Requirement.Contains(requirement_process) == false)
                                    {
                                        database.m_Requirement.Add(requirement_process);
                                    }

                                    //Zuordnung Element_Functional zu einer Capability
                                    Check_For_Capability(repository, database, requirement_process);
                                    //Check Issue
                                    requirement_process.Get_Issues(database);
                                    //Check Klärungspunkte
                                    requirement_process.Check_Klärungspunkte(requirement_process.m_Issues, database, repository);
                                }
                                {
                                    //Sollte nicht vorkommen
                                }
                            }

                            i2++;
                        } while (i2 < NT_Requ.Count);
                    }
                    i1++;
                } while (i1 < OpCon_Requ.Count);
            }
        }

        public void Check_For_Capability(EA.Repository repository, Requirement_Plugin.Database database, Requirement_Non_Functional requirement)
        {
            Capability capability = null;
         //   DB_Command command = new DB_Command();
         //   XML xML = new XML();
            List<string> m_Type_DerivedCap = database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList();
            List<string> m_Stereotype_DerivedCap = database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList();
            List<string> m_Capability_Type = database.metamodel.m_Capability.Select(x => x.Type).ToList();
            List<string> m_Capability_Stereotype = database.metamodel.m_Capability.Select(x => x.Stereotype).ToList();

            if (requirement != null)
            {
                List<string> m_GUID = new List<string>();
                Interface_Connectors interface_Connectors = new Interface_Connectors();
                List<string> m_help = new List<string>();
                m_help.Add(requirement.Classifier_ID);
                m_GUID = interface_Connectors.Get_Supplier_Element_By_Connector(database, m_help, m_Capability_Type, m_Capability_Stereotype, m_Type_DerivedCap, m_Stereotype_DerivedCap);

                if (m_GUID != null)
                {
                    Capability cap = database.Check_Capability_Database(m_GUID[0]);

                    if (cap != null)
                    {

                        this.capability = cap;
                        if (requirement.m_Capability.Contains(cap) == false)
                        {
                        //    this.Requirement_Process.m_Capability.Add(cap);
                            requirement.m_Capability.Add(cap);
                        }

                    }
                }


            }
        }
    
        public bool Check_For_NodeType(NodeType node)
        {
            bool ret = false;

            List<string> m_guid = node.m_Instantiate;
            if(m_guid.Contains(node.Classifier_ID) == false)
            {
                m_guid.Add(node.Classifier_ID);
            }

            if(m_guid.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.m_Node_ID.Contains(m_guid[i1]))
                    {
                        ret = true;
                        i1 = m_guid.Count;
                    }

                    i1++;
                } while (i1 < m_guid.Count);
            }
          


            return (ret);
        }

        #endregion
        #region Create
        public void Create_Requirement_Process(EA.Repository repository, Requirement_Plugin.Database database, string Package_GUID, Activity activity, List<NodeType> m_create)
        {
            //NodeType erhalten
            List<NodeType> m_nt = new List<NodeType>();
            if(this.m_Node_ID.Count > 0)
            {
                int i1 = 0;
                do
                {
                    NodeType help = new NodeType(null, repository, database);
                    help.Classifier_ID = this.m_Node_ID[i1];
                    string classifier = help.Get_Classifier(database);

                    if(classifier == null) //Ist NodeType
                    {
                        List<NodeType> m_nt2 = database.m_NodeType.Where(x => x.Classifier_ID == this.m_Node_ID[i1]).ToList();

                        if(m_nt2.Count == 1)
                        {
                            if(m_nt.Contains(m_nt2[0]) == false)
                            {
                                m_nt.Add(m_nt2[0]);
                            }
                           
                        }
                    }
                    else
                    {
                        if(classifier == "null")
                        {
                            List<NodeType> m_nt2 = database.m_NodeType.Where(x => x.Classifier_ID == this.m_Node_ID[i1]).ToList();

                            if (m_nt2.Count == 1)
                            {
                                if (m_nt.Contains(m_nt2[0]) == false)
                                {
                                    m_nt.Add(m_nt2[0]);
                                }

                            }
                        }
                        else
                        {
                            List<NodeType> m_nt2 = database.m_NodeType.Where(x => x.Classifier_ID == classifier).ToList();
                            if (m_nt2.Count == 1)
                            {
                                if (m_nt.Contains(m_nt2[0]) == false)
                                {
                                    m_nt.Add(m_nt2[0]);
                                }
                            }
                        }
                    
                    }

                    i1++;
                } while (i1 < this.m_Node_ID.Count);
            }

            if (this.Requirement_Process == null && m_nt.Count > 0)
            {
                if(m_create.Contains(m_nt[0]) == true)
                {
                    //m_nt muss Länge 1 haben
                    NodeType recent = new NodeType(null, repository, database);
                    recent = m_nt[0];
                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

                    #region AFO_Parameter
                    //W_Object
                    string W_Object = activity.W_Object;
                    //W_Prozesswort
                    string W_Prozesswort = activity.W_Prozesswort;
                    //W_Qualitaet
                    string W_Qualitaet = this.OpConstraint.Name;
                    //W_Randbedingung
                    string W_Randbedingung = "";
                    //W_Singular
                    bool W_Singular = recent.W_SINGULAR;
                    //W_Subject
                    string W_Subject = recent.W_Artikel + " " + recent.Name;
                    //W_Zu
                    bool W_zu = false;
                    //recent_guid
                    string recent_guid = "";
                    //Titel
                    string Titel = recent.Name + " - " + W_Prozesswort + " " + W_Object + " - " + this.OpConstraint.Name;
                    //Text
                    string AFO_Text = "";

                    if (W_Singular == true)
                    {
                        AFO_Text = ti.ToTitleCase(recent.W_Artikel) + " " + recent.Name + " " + database.metamodel.m_Verbindlichkeit[0] + " " + W_Object + " " + W_Qualitaet + " " + W_Prozesswort + ".";
                    }
                    else
                    {
                        AFO_Text = ti.ToTitleCase(recent.W_Artikel) + " " + recent.Name + " " + database.metamodel.m_Verbindlichkeit[1] + " " + W_Object + " " + W_Qualitaet + " " + W_Prozesswort + ".";
                    }
                    #endregion

                    Requirement_Non_Functional requirement2 = new Requirement_Non_Functional(null, database.metamodel);
                    requirement2.Add_Requirement_Process(Titel, AFO_Text, W_Object, W_Prozesswort, W_Qualitaet, W_Randbedingung, W_Singular, W_Subject, database.metamodel.m_Requirement_Process[0].Stereotype);
                    requirement2.W_AFO_MANUAL = true;
                    requirement2.AFO_KLAERUNGSPUNKTE = "<<Constraint>> "+this.OpConstraint.Name+": "+ this.OpConstraint.Notes+"<</Constraint>>";

                    requirement2.Classifier_ID = requirement2.Create_Requirement(repository, Package_GUID, database.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList()[0], database);

                    this.Requirement_Process = requirement2;

                    #region An andere Elemnt_Prcess AFO hinzufügen
                    activity.Element_Process_Set_Requirement(recent, this.OpConstraint, requirement2);

                    #endregion
                    #region Connectoren
                    //NodeType
                    Create_Connectoren_NodeType(repository, database);
                    //OperationalConstraint
                    Create_Connectoren_OperationalConstraint(repository, database);
                    //Activity
                    Create_Connectoren_Activity(repository, database);
                    //Zuorndung ElementFunctional und ElementUser
                    Create_Connectoren_Parent(repository, database, activity, recent);

                    Create_Connectoren_Duplicate_Generalisierung(repository, database, activity, recent);

                    #endregion
                }


            }
            else if(m_nt.Count > 0)
            {
                //Konnektoren updaten
                #region Connectoren
                //NodeType
                Create_Connectoren_NodeType(repository, database);
                //OperationalConstraint
                Create_Connectoren_OperationalConstraint(repository, database);
                //Activity
                Create_Connectoren_Activity(repository, database);
                //Zuorndung ElementFunctional und ElementUser
                Create_Connectoren_Parent(repository, database, activity, m_nt[0]);

                Create_Connectoren_Duplicate_Generalisierung(repository, database, activity, m_nt[0]);

                #endregion
            }


        }

        #endregion

        public void Update_Connectoren_Requirement_Process(EA.Repository repository, Requirement_Plugin.Database database, Activity activity)
        {
            //NodeType erhalten
            List<NodeType> m_nt = new List<NodeType>();
            if (this.m_Node_ID.Count > 0)
            {
                int i1 = 0;
                do
                {
                    NodeType help = new NodeType(null, repository, database);
                    help.Classifier_ID = this.m_Node_ID[i1];
                    string classifier = help.Get_Classifier(database);

                    if (classifier == null) //Ist NodeType
                    {
                        List<NodeType> m_nt2 = database.m_NodeType.Where(x => x.Classifier_ID == this.m_Node_ID[i1]).ToList();

                        if (m_nt2.Count == 1)
                        {
                            if (m_nt.Contains(m_nt2[0]) == false)
                            {
                                m_nt.Add(m_nt2[0]);
                            }

                        }
                    }
                    else
                    {
                        List<NodeType> m_nt2 = database.m_NodeType.Where(x => x.Classifier_ID == classifier).ToList();
                        if (m_nt2.Count == 1)
                        {
                            if (m_nt.Contains(m_nt2[0]) == false)
                            {
                                m_nt.Add(m_nt2[0]);
                            }
                        }
                    }

                    i1++;
                } while (i1 < this.m_Node_ID.Count);
            }

            if (m_nt.Count> 0)
            {
                NodeType recent = new NodeType(null, repository, database);
                recent = m_nt[0];
                #region Connectoren
                //NodeType
                Create_Connectoren_NodeType(repository, database);
                //OperationalConstraint
                Create_Connectoren_OperationalConstraint(repository, database);
                //Activity
                Create_Connectoren_Activity(repository, database);
                //Zuorndung ElementFunctional und ElementUser
                Create_Connectoren_Parent(repository, database, activity, recent);

                Create_Connectoren_Duplicate_Generalisierung(repository, database, activity, recent);
            }
           

            #endregion
        }

        #region Konnektoren
        private void Create_Connectoren_NodeType(EA.Repository repository, Requirement_Plugin.Database database)
        {
            if (this.Requirement_Process != null)
            {
                Repository_Connector repository_Connector = new Repository_Connector();
                if (this.m_Node_ID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(this.Requirement_Process.Classifier_ID, this.m_Node_ID[i1], database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), database.metamodel.m_Derived_Element[0].SubType, repository, database, database.metamodel.m_Derived_Element[0].Toolbox, database.metamodel.m_Derived_Element[0].direction);

                        i1++;
                    } while (i1 < this.m_Node_ID.Count);
                }
            }
         
        }
        private void Create_Connectoren_OperationalConstraint(EA.Repository repository, Requirement_Plugin.Database database)
        {
            if (this.Requirement_Process != null)
            {
                Repository_Connector repository_Connector = new Repository_Connector();
                if (this.m_Node_ID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(this.Requirement_Process.Classifier_ID, this.m_Node_ID[i1], database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), database.metamodel.m_Derived_Element[0].SubType, repository, database, database.metamodel.m_Derived_Element[0].Toolbox, database.metamodel.m_Derived_Element[0].direction);

                        i1++;
                    } while (i1 < this.m_Node_ID.Count);
                }
               // Repository_Connector repository_Connector = new Repository_Connector();
                    if (this.OpConstraint != null)
                    {
                        if (this.OpConstraint.Classifier_ID != null)
                        {
                            repository_Connector.Create_Dependency(this.Requirement_Process.Classifier_ID, this.OpConstraint.Classifier_ID, database.metamodel.m_Derived_Constraint.Select(x => x.Stereotype).ToList(), database.metamodel.m_Derived_Constraint.Select(x => x.Type).ToList(), database.metamodel.m_Derived_Constraint[0].SubType, repository, database, database.metamodel.m_Derived_Constraint[0].Toolbox, database.metamodel.m_Derived_Constraint[0].direction);
                        }

                    }
            }

        }
        private void Create_Connectoren_Activity(EA.Repository repository, Requirement_Plugin.Database database)
        {
            if (this.Requirement_Process != null)
            {
                Repository_Connector repository_Connector = new Repository_Connector();
                if (this.m_Action_ID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(this.Requirement_Process.Classifier_ID, this.m_Action_ID[i1], database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), database.metamodel.m_Derived_Element[0].SubType, repository, database, database.metamodel.m_Derived_Element[0].Toolbox, database.metamodel.m_Derived_Element[0].direction);

                        i1++;
                    } while (i1 < this.m_Action_ID.Count);
                }
            }
        }
        private void Create_Connectoren_Parent(EA.Repository repository, Requirement_Plugin.Database database, Activity activity, NodeType node)
        {
            if (this.Requirement_Process != null)
            {
                Repository_Connector repository_Connector = new Repository_Connector();
                if (activity.m_Element_Functional.Count > 0)
                {

                    List<Requirement_Functional> m_req_func = activity.m_Element_Functional.Where(x => x.Client == node).ToList().SelectMany(x => x.m_Requirement_Functional).ToList();

                    if (m_req_func.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(this.Requirement_Process.Classifier_ID, m_req_func[i1].Classifier_ID, database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Refines[0].SubType, repository, database, database.metamodel.m_Afo_Refines[0].Toolbox, database.metamodel.m_Afo_Refines[0].direction);

                            i1++;
                        } while (i1 < m_req_func.Count);
                    }
                    List<Requirement_User> m_req_user = activity.m_Element_User.Where(x => x.Client == node).ToList().SelectMany(x => x.m_Requirement_User).ToList();
                    if (m_req_user.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(this.Requirement_Process.Classifier_ID, m_req_user[i1].Classifier_ID, database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Refines[0].SubType, repository, database, database.metamodel.m_Afo_Refines[0].Toolbox, database.metamodel.m_Afo_Refines[0].direction);

                            i1++;
                        } while (i1 < m_req_user.Count);
                    }
                }
            }
        }

        public void Create_Connectoren_Duplicate_Generalisierung(EA.Repository repository, Requirement_Plugin.Database database, Activity activity, NodeType node)
        {
            Repository_Connector repository_Connector = new Repository_Connector();

            #region Generalsiierung runter
            List<NodeType> m_Specilize = new List<NodeType>();
            m_Specilize = node.m_Specialize;

            if (m_Specilize.Count > 0)
            {
                
                List<Element_Functional> m_func = activity.m_Element_Functional.Where(x => m_Specilize.Contains(x.Client) == true).ToList();
                List<Element_Process> m_proc = m_func.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == this.OpConstraint).ToList();

                if(m_proc.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(m_proc[i1].Requirement_Process != null)
                        {
                            if(m_proc[i1].Requirement_Process.Classifier_ID != null && this.Requirement_Process.Classifier_ID != null)
                            {
                                repository_Connector.Create_Dependency( m_proc[i1].Requirement_Process.Classifier_ID, this.Requirement_Process.Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                            }
                        }

                        i1++;
                    } while (i1 < m_proc.Count);
                }

                List<Element_User> m_user = activity.m_Element_User.Where(x => m_Specilize.Contains(x.Client) == true).ToList();
                List<Element_Process> m_proc_user = m_user.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == this.OpConstraint).ToList();

                if (m_proc_user.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_proc_user[i1].Requirement_Process != null)
                        {
                            if (m_proc_user[i1].Requirement_Process.Classifier_ID != null && this.Requirement_Process.Classifier_ID != null)
                            {
                                repository_Connector.Create_Dependency( m_proc_user[i1].Requirement_Process.Classifier_ID, this.Requirement_Process.Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                            }
                        }

                        i1++;
                    } while (i1 < m_proc_user.Count);
                }

            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified =node.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {

                List<Element_Functional> m_func = activity.m_Element_Functional.Where(x => m_Specified.Contains(x.Client) == true).ToList();
                List<Element_Process> m_proc = m_func.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == this.OpConstraint).ToList();

                if (m_proc.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_proc[i1].Requirement_Process != null)
                        {
                            if (m_proc[i1].Requirement_Process.Classifier_ID != null && this.Requirement_Process.Classifier_ID != null)
                            {
                                repository_Connector.Create_Dependency( this.Requirement_Process.Classifier_ID, m_proc[i1].Requirement_Process.Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                            }
                        }

                        i1++;
                    } while (i1 < m_proc.Count);
                }

                List<Element_User> m_user = activity.m_Element_User.Where(x => m_Specified.Contains(x.Client) == true).ToList();
                List<Element_Process> m_proc_user = m_user.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == this.OpConstraint).ToList();

                if (m_proc_user.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_proc_user[i1].Requirement_Process != null)
                        {
                            if (m_proc_user[i1].Requirement_Process.Classifier_ID != null && this.Requirement_Process.Classifier_ID != null)
                            {
                                repository_Connector.Create_Dependency( this.Requirement_Process.Classifier_ID, m_proc_user[i1].Requirement_Process.Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                            }
                        }

                        i1++;
                    } while (i1 < m_proc_user.Count);
                }

            }
            #endregion
        }
        #endregion
    }
}

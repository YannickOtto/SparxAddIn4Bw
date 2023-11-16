using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Requirement_Plugin;
using Elements;
using Requirement_Plugin.Interfaces;

namespace Repsoitory_Elements
{
    public class Pool : Repository_Element
    {
        public List<Activity> m_Activity;
        public List<NodeType> m_Owner;
        public List<string> m_Represent;
        public List<Pool> m_Lanes;
        public List<string> m_GUID_sendEvent;
        public List<string> m_GUID_receiveEvent;
        public Pool Owning_Partition;

        public Pool()
        {
            this.m_Activity = new List<Activity>();
            this.m_Owner = new List<NodeType>();
            this.m_Lanes = new List<Pool>();
            this.Owning_Partition = null;
            this.m_Represent = new List<string>();
            this.m_GUID_sendEvent = new List<string>();
            this.m_GUID_receiveEvent = new List<string>();

        }

        public void Get_Lanes(Database database)
        {
            Requirement_Plugin.Interfaces.Interface_Element interface_Element = new Requirement_Plugin.Interfaces.Interface_Element();
            //Erhalten alle embedded Lanes
            List<string> m_Type_Child = database.metamodel.m_Lanes_BPMN.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Child = database.metamodel.m_Lanes_BPMN.Select(x => x.Stereotype).ToList();

            List<string> m_GUID_Lanes = interface_Element.Get_Children_Element(this.Classifier_ID, database, m_Type_Child, m_Stereotype_Child);

            if (m_GUID_Lanes != null)
            {
                int i1 = 0;
                do
                {
                    Pool recent_Child = new Pool();
                    recent_Child.Classifier_ID = m_GUID_Lanes[i1];
                    this.m_Lanes.Add(recent_Child);
                    recent_Child.Owning_Partition = this;

                    recent_Child.ID = recent_Child.Get_Object_ID(database);

                    recent_Child.Get_Lanes(database);



                    i1++;
                } while (i1 < m_GUID_Lanes.Count);
            }
        }

        public void Get_Activity(Database database, EA.Repository repository)
        {
            Requirement_Plugin.Interfaces.Interface_Element interface_Element = new Requirement_Plugin.Interfaces.Interface_Element();
            /*//Erhalten alle embedded Lanes
            List<string> m_Type_Child = database.metamodel.m_Lanes_BPMN.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Child = database.metamodel.m_Lanes_BPMN.Select(x => x.Stereotype).ToList();

            List<string> m_GUID_Lanes = interface_Element.Get_Children_Element(this.Classifier_ID, database, m_Type_Child, m_Stereotype_Child);*/

            if(this.m_Lanes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    /* Pool recent_Child = new Pool();
                     recent_Child.Classifier_ID = m_GUID_Lanes[i1];
                     this.m_Lanes.Add(recent_Child);
                     recent_Child.Owning_Partition = this;
                     */
                    this.m_Lanes[i1].Get_Activity(database, repository);

                    i1++;
                } while (i1 < this.m_Lanes.Count);
            }
            //Erhalten alle Activity welche im Pool bzw untergeordneten Lanes
            List<string> m_Type_Activity = database.metamodel.m_Aktivity_Definition_BPMN.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Activity = database.metamodel.m_Aktivity_Definition_BPMN.Select(x => x.Stereotype).ToList();
            
            List<string> m_GUID_Activity = interface_Element.Get_Children_Element(this.Classifier_ID, database, m_Type_Activity, m_Stereotype_Activity);

            if(m_GUID_Activity != null)
            {
                Requirement_Plugin.Interfaces.Interface_TaggedValue interface_TaggedValue = new Requirement_Plugin.Interfaces.Interface_TaggedValue();
                string GUID_Classifier = "";

                int i2 = 0;
                do
                {
                    
                    bool flag_called = false;
                    bool flag_Classifier = false;
                    //Überprüfen, ob dieses Element ein Behaviour Verweis auf eine andere Activity besitzt TV calledActivityRef
                    //Dies wurde durch Classifier ersetzt
                    string called = interface_TaggedValue.Get_Tagged_Value_By_Property(database, database.metamodel.Act_BPMN_Tagged_Values[0].Map_Name, m_GUID_Activity[i2]);




                    if(called != null)
                    {
                        if (called.Length == 38) //Länge einer Sparx GUID
                        {
                            flag_called = true;
                            //Ist eine Activity ref
                            GUID_Classifier = called;
                          //  m_GUID_Activity[i2] = called;
                        }
                        else
                        {
                            Activity help = new Activity(null, database, repository);
                            help.Classifier_ID = m_GUID_Activity[i2];

                            string classified = help.Get_Classifier_Activity(database);

                            if(classified != null)
                            {
                                GUID_Classifier = classified;
                            }
                            else
                            {
                                GUID_Classifier = m_GUID_Activity[i2];
                            }

                           
                        }
                    }
                    else
                    {
                        Activity help = new Activity(null, database, repository);
                        help.Classifier_ID = m_GUID_Activity[i2];

                        string classified = help.Get_Classifier_Activity(database);

                        if (classified != null)
                        {
                            GUID_Classifier = classified;
                        }
                        else
                        {
                            GUID_Classifier = m_GUID_Activity[i2];
                        }
                    }
                    //Überprüfen Activity vorhanden
                    List<Activity> m_check_act = database.Check_Activity(GUID_Classifier);

                    


                    if(m_check_act == null) //neu anlegen
                    {
                        Activity recent_act = new Activity(GUID_Classifier, database, repository);
                        recent_act.m_GUID.Add(m_GUID_Activity[i2]);
                        this.m_Activity.Add(recent_act);
                        
                    }
                    else //vorhanden --> nur neu verknüpfen
                    {
                        //Überprüfen, ob Activities schon verknüpft
                        int i3 = 0;
                        do
                        {
                            if(this.m_Activity.Select(x => x.Classifier_ID).ToList().Contains(m_check_act[i3].Classifier_ID) == false)
                            {
                                this.m_Activity.Add(m_check_act[i3]);

                                if(m_check_act[i3].m_GUID.FindIndex(x => x == m_GUID_Activity[i2]) == -1)
                                {
                                    m_check_act[i3].m_GUID.Add(m_GUID_Activity[i2]);
                                }
                            }

                            i3++;
                        } while (i3 < m_check_act.Count);
                    }

                    i2++;
                } while (i2 < m_GUID_Activity.Count);
                //
            }

            if(this.Owning_Partition != null)
            {
                this.Owning_Partition.m_Activity.AddRange(this.m_Activity);
            }
        }

        public List<string> Check_Action(Database database, string GUID)
        {
            Interface_Element interface_Element = new Interface_Element();

            List<string> check_action = new List<string>();

           
            //Lanes prüfen
            if(this.m_Lanes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    List<string> help_child =  this.m_Lanes[i1].Check_Action(database, GUID);

                    if(help_child != null)
                    {
                        check_action.AddRange(help_child);
                    }

                    i1++;
                } while (i1 < this.m_Lanes.Count);
            }

            List<string> check_recent = interface_Element.Check_Database_Element_Classifier(database, database.metamodel.m_Aktivity_Definition_BPMN.Select(x => x.Type).ToList(), database.metamodel.m_Aktivity_Definition_BPMN.Select(x => x.Stereotype).ToList(), this.ID, GUID);

            if(check_recent != null)
            {
                check_action.AddRange(check_recent);
            }

            if(check_action.Count == 0)
            {
                check_action = null;
            }

            return (check_action);
        }

        public void Get_Element_Functional(Database database, EA.Repository repository)
        {
           

            if(this.m_Activity.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.m_Owner != null)
                    {
                        int i2 = 0;
                        do
                        {
                            Element_Functional check = this.m_Owner[i2].Check_Element_Functional(this.m_Owner[i2], this.m_Activity[i1]);
                            //GUID erhalten von Activity oder Action
                            List<string> check_action = this.Check_Action(database, this.m_Activity[i1].Classifier_ID);

                            if(check_action == null)
                            {
                                check_action = new List<string>();
                                check_action.Add(this.m_Activity[i1].Classifier_ID);
                            }

                            if (check == null) //nicht vorhanden in DB
                            {
                                Element_Functional recent = new Element_Functional();
                                recent.Client = this.m_Owner[i2];
                                recent.Supplier = this.m_Activity[i1];

                                if (this.m_Represent != null && check_action != null)
                                {
                                    //Targets zuordnen
                                    List<string> join = this.m_Owner[i2].m_Instantiate.Intersect(this.m_Represent).ToList();

                                    int a1 = 0;
                                    do
                                    {
                                        int i3 = 0;
                                        do
                                        {
                                            Target_Functional check_target = recent.Check_Target(join[i3], check_action[a1]);

                                            if (check_target == null)
                                            {
                                                //anlegen
                                                recent.Create_Target_Functional(join[i3], check_action[a1], repository, database);
                                            }

                                            i3++;
                                        } while (i3 < join.Count);

                                        a1++;
                                    } while (a1 < check_action.Count );

                                }

                                this.m_Owner[i2].m_Element_Functional.Add(recent);
                                this.m_Activity[i1].m_Element_Functional.Add(recent);
                            }
                            else //ElementFunctional in DB vorhanden
                            {
                                //Target überprüfen für zugehörige m_Represent
                                List<string> join = this.m_Owner[i2].m_Instantiate.Intersect(this.m_Represent).ToList();

                                int a1 = 0;
                                do
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        Target_Functional check_target = check.Check_Target(join[i3], check_action[a1]);

                                        if (check_target == null)
                                        {
                                            //anlegen
                                            check.Create_Target_Functional(join[i3], check_action[a1], repository, database);
                                        }

                                        i3++;
                                    } while (i3 < join.Count);

                                    a1++;
                                } while (a1 < check_action.Count);
                               
                            }

                                i2++;
                        } while (i2 < this.m_Owner.Count);
                    }
                    i1++;
                } while (i1 < this.m_Activity.Count);


                #region Archiv
                /*   int i1 = 0;
                   do
                   {
                       //Jeden Owner überprüfen, ob Element Functional vorhanden
                       if(this.m_Owner != null)
                       {
                           int i2 = 0;
                           do
                           {
                               Element_Functional check = this.m_Owner[i2].Check_Element_Functional(this.m_Owner[i2], this.m_Activity[i1]);

                               if(check == null) //nicht vorhanden in DB
                               {
                                   //neu anlegen
                                   Element_Functional recent = new Element_Functional();
                                   recent.Client = this.m_Owner[i2];
                                   recent.Supplier = this.m_Activity[i1];

                                   this.m_Owner[i2].m_Element_Functional.Add(recent);
                                   this.m_Activity[i1].m_Element_Functional.Add(recent);

                                   if(this.m_Represent != null)
                                   {
                                       //Targets zuordnen
                                       List<string> join = this.m_Owner[i2].m_Instantiate.Intersect(this.m_Represent).ToList();

                                       //Targets prüfen
                                       int i3 = 0;
                                       do
                                       {
                                           Target_Functional check_target =  recent.Check_Target(join[i3], this.m_Activity[i1].Classifier_ID);

                                           if(check_target == null)
                                           {
                                               //anlegen
                                               recent.Create_Target_Functional(join[i3], this.m_Activity[i1].Classifier_ID, repository, database);
                                           }

                                           i3++;
                                       } while (i3 < join.Count);


                                   }
                               }
                               else //Vorhanden in DB
                               {
                                   //Target überprüfen für zugehörige m_Represent
                                   List<string> join =  this.m_Owner[i2].m_Instantiate.Intersect(this.m_Represent).ToList();
                                   //Targets prüfen
                                   int i3 = 0;
                                   do
                                   {
                                       Target_Functional check_target = check.Check_Target(join[i3], this.m_Activity[i1].Classifier_ID);

                                       if (check_target == null)
                                       {
                                           //anlegen
                                           check.Create_Target_Functional(join[i3], this.m_Activity[i1].Classifier_ID, repository, database);
                                       }

                                       i3++;
                                   } while (i3 < join.Count);
                               }

                               i2++;
                           } while (i2 < this.m_Owner.Count);
                       }

                       i1++;
                   } while (i1 < this.m_Activity.Count);
                */
                #endregion Archiv
            }
        }

        public void Get_Event(Database database)
        {
            Requirement_Plugin.Interfaces.Interface_Element interface_Element = new Requirement_Plugin.Interfaces.Interface_Element();
          /*  //Erhalten alle embedded Lanes
            List<string> m_Type_Child = database.metamodel.m_Lanes_BPMN.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Child = database.metamodel.m_Lanes_BPMN.Select(x => x.Stereotype).ToList();

            List<string> m_GUID_Lanes = interface_Element.Get_Children_Element(this.Classifier_ID, database, m_Type_Child, m_Stereotype_Child);
*/
            if (this.m_Lanes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    /* Pool recent_Child = new Pool();
                     recent_Child.Classifier_ID = m_GUID_Lanes[i1];
                     this.m_Lanes.Add(recent_Child);
                     recent_Child.Owning_Partition = this;
                     */
                    this.m_Lanes[i1].Get_Event(database);

                    i1++;
                } while (i1 < this.m_Lanes.Count);
            }
            //Erhalten alle Activity welche im Pool bzw untergeordneten Lanes
            /*   List<string> m_Type_EndEvent = database.metamodel.m_EndEvent_BPMN.Select(x => x.Type).ToList();
               List<string> m_Stereotype_EndEvent = database.metamodel.m_EndEvent_BPMN.Select(x => x.Stereotype).ToList();

               List<string> m_Type_StartEvent = database.metamodel.m_StartEvent_BPMN.Select(x => x.Type).ToList();
               List<string> m_Stereotype_StartEvent = database.metamodel.m_StartEvent_BPMN.Select(x => x.Stereotype).ToList();*/

            //Alle Möglichen Elemente, wo ein MessageFlow starten/Ednen kann
            List<string> m_Type_EndEvent = database.metamodel.m_EndEvent_BPMN.Select(x => x.Type).ToList().Concat(database.metamodel.m_Aktivity_Definition_BPMN.Select(x => x.Type).ToList()).ToList();
            List<string> m_Stereotype_EndEvent = database.metamodel.m_EndEvent_BPMN.Select(x => x.Stereotype).ToList().Concat(database.metamodel.m_Aktivity_Definition_BPMN.Select(x => x.Stereotype).ToList()).ToList();

            List<string> m_Type_StartEvent = database.metamodel.m_StartEvent_BPMN.Select(x => x.Type).ToList().Concat(database.metamodel.m_Aktivity_Definition_BPMN.Select(x => x.Type).ToList()).ToList(); ;
            List<string> m_Stereotype_StartEvent = database.metamodel.m_StartEvent_BPMN.Select(x => x.Stereotype).ToList().Concat(database.metamodel.m_Aktivity_Definition_BPMN.Select(x => x.Stereotype).ToList()).ToList(); ;

            //SendEvent
            List<string> m_GUID_EndEvent = interface_Element.Get_Children_Element(this.Classifier_ID, database, m_Type_EndEvent, m_Stereotype_EndEvent);

            if (m_GUID_EndEvent != null)
            {
                Requirement_Plugin.Interfaces.Interface_TaggedValue interface_TaggedValue = new Requirement_Plugin.Interfaces.Interface_TaggedValue();
                string GUID_Classifier = "";
                int i2 = 0;
                do
                {
                    GUID_Classifier = m_GUID_EndEvent[i2];

                    if(this.m_GUID_sendEvent.Contains(GUID_Classifier) == false)
                    {
                        this.m_GUID_sendEvent.Add(GUID_Classifier);
                    }

                    i2++;
                } while (i2 < m_GUID_EndEvent.Count);
            }
            if (this.Owning_Partition != null)
            {
                this.Owning_Partition.m_GUID_sendEvent.AddRange(this.m_GUID_sendEvent);
            }
            //ReceiveEvent
            List<string> m_GUID_StartEvent = interface_Element.Get_Children_Element(this.Classifier_ID, database, m_Type_StartEvent, m_Stereotype_StartEvent);

            if (m_GUID_StartEvent != null)
            {
                Requirement_Plugin.Interfaces.Interface_TaggedValue interface_TaggedValue = new Requirement_Plugin.Interfaces.Interface_TaggedValue();
                string GUID_Classifier = "";
                int i2 = 0;
                do
                {
                    GUID_Classifier = m_GUID_StartEvent[i2];

                    if (this.m_GUID_receiveEvent.Contains(GUID_Classifier) == false)
                    {
                        this.m_GUID_receiveEvent.Add(GUID_Classifier);
                    }

                    i2++;
                } while (i2 < m_GUID_StartEvent.Count);
            }
            if (this.Owning_Partition != null)
            {
                this.Owning_Partition.m_GUID_receiveEvent.AddRange(this.m_GUID_receiveEvent);
            }

        }

        public void Get_Element_Interface(Database Data, NodeType Client, EA.Repository repository)
        {
            Repository_Connector repository_Connector = new Repository_Connector();
            Repository_Connectors repository_Connectors = new Repository_Connectors();
            Requirement_Plugin.Interfaces.Interface_Connectors interface_Connectors = new Requirement_Plugin.Interfaces.Interface_Connectors();
            List<string> m_Con_Type = Data.metamodel.m_Con_Message_BPMN.Select(x => x.Type).ToList();
            List<string> m_Con_Stereotype = Data.metamodel.m_Con_Message_BPMN.Select(x => x.Stereotype).ToList();
          
             if (this.m_GUID_sendEvent.Count > 0)
                {
                    //Alle SendeEvents betrachten und daraus jeweils ein Element Interface erstellen mit dem jeweiligen Empfänger
                    int p2 = 0;
                    do
                    {
                        //Zugehörigen Connector erhalten
                        List<string> m_GUID_Client = new List<string>();
                        m_GUID_Client.Add(this.m_GUID_sendEvent[p2]);

                        List<string> m_GUID_Con = repository_Connectors.Get_Connectors_Element(m_GUID_Client, Data, m_Con_Type, m_Con_Stereotype);

                        //Supplier erhalten
                        if (m_GUID_Con != null)
                        {
                            int p3 = 0;
                            do
                            {
                               #region Info Elem
                         /*   //InfoElem erhalten
                            List<string> m_GUID_Info_Elem = interface_Connectors.GetInformationElements(Data, m_GUID_Con[p3]);
                                List<InformationElement> m_Info_Elem = new List<InformationElement>();
                                //InfoElem erstellen
                                if (m_GUID_Info_Elem != null)
                                {
                                    int e1 = 0;
                                    do
                                    {
                                        InformationElement check_info = Data.Check_InformationElement(m_GUID_Info_Elem[e1]);

                                        if (check_info == null)
                                        {
                                            InformationElement new_Info_Elem = new InformationElement(m_GUID_Info_Elem[e1]);
                                            m_Info_Elem.Add(new_Info_Elem);
                                        }
                                        else
                                        {
                                            if (m_Info_Elem.Contains(check_info) == false)
                                            {
                                                m_Info_Elem.Add(check_info);
                                            }
                                        }

                                        e1++;
                                    } while (e1 < m_GUID_Info_Elem.Count);
                                }*/
                            #endregion Infoelem
                                 //Supplier
                                string receiveEvent_GUID = repository_Connector.Get_Connector_Supplier_GUID(repository, m_GUID_Con[p3], Data);
                                Pool receivePool = Data.Check_ReceiveEvent(receiveEvent_GUID);

                                //Check Element Interface der Pool Owner
                                if (receivePool != null)
                                {
                                    if (receivePool.m_Owner.Count > 0)
                                    {
                                        int p4 = 0;
                                        do
                                        {
                                            Element_Interface check_interface = Client.Check_Element_Interface(receivePool.m_Owner[p4]);

                                            if (check_interface == null)
                                            {
                                                //anlegen Element Interface
                                                Element_Interface elem_interface2 = new Element_Interface(Client.Classifier_ID, receivePool.m_Owner[p4].Classifier_ID);
                                                elem_interface2.Client = Client;
                                                elem_interface2.Supplier = receivePool.m_Owner[p4];

                                                Client.m_Element_Interface.Add(elem_interface2);

                                                check_interface = elem_interface2;

                                                //Targets anlegen

                                                //Guids des aktuellen NodeType
                                                List<string> join_Client = Client.m_Instantiate.Intersect(this.m_Represent).ToList();
                                                //Guides des aktuellen receivePool Owners
                                                List<string> join_Supplier = receivePool.m_Owner[p4].m_Instantiate.Intersect(receivePool.m_Represent).ToList();

                                                elem_interface2.Create_Targets(join_Client, join_Supplier, Data, repository, m_GUID_Con[p3]);
                                            }
                                            else
                                            {
                                                //überpürfen, ob Targets alle vorhanden
                                               //Guids des aktuellen NodeType
                                                 List<string> join_Client = Client.m_Instantiate.Intersect(this.m_Represent).ToList();
                                                //Guides des aktuellen receivePool Owners
                                                List<string> join_Supplier = receivePool.m_Owner[p4].m_Instantiate.Intersect(receivePool.m_Represent).ToList();

                                                 check_interface.Create_Targets(join_Client, join_Supplier, Data, repository, m_GUID_Con[p3]);
                                            }

                                            p4++;
                                        } while (p4 < receivePool.m_Owner.Count);
                                    }
                                }

                                p3++;
                            } while (p3 < m_GUID_Con.Count);
                        }


                        p2++;
                    } while (p2 < this.m_GUID_sendEvent.Count);
                }

        
        }
    }
}

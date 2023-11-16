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
    public class SysElement : NodeType
    {
        public List<NodeType> m_Implements = new List<NodeType>();
        public List<SysElement> m_Child = new List<SysElement>();
        public List<SysElement> m_Paretn = new List<SysElement>();
        public List<SysElement> m_Specialize_SysElem = new List<SysElement>();

        public List<List<string>> m_m_Implements_guid = new List<List<string>>();

        public List<string> m_Implements_guid = new List<string>();
        public SysElement(string Classifier_ID, EA.Repository Repository, Database Data): base( Classifier_ID, Repository, Data)
        {

        }

        #region Struktur

        public void Check_Generalization_SysElem(Database database)
        {

            List<string> m_Type = new List<string>();
            m_Type = database.metamodel.m_Elements_SysArch_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype = new List<string>();
            m_Stereotype = database.metamodel.m_Elements_SysArch_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_con = new List<string>();
            m_Type_con = database.metamodel.m_General_Class.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = new List<string>();
            m_Stereotype_con = database.metamodel.m_General_Class.Select(x => x.Stereotype).ToList();

            List<string> m_guid = this.Get_Generalization(database, m_Type, m_Stereotype, m_Type_con, m_Stereotype_con);

            if (m_guid != null)
            {
                int i1 = 0;
                do
                {
                    List<SysElement> m_act = database.m_SysElemente.Where(x => x.Classifier_ID == m_guid[i1]).ToList();

                    if (m_act.Count > 0)
                    {
                        if (this.m_Specialize.Contains(m_act[0]) == false)
                        {
                            this.m_Specialize.Add(m_act[0]);
                        }
                    }

                    i1++;
                } while (i1 < m_guid.Count);

            }
        }

        public void Get_Children_Class(Database database, EA.Repository repository)
        {
            Interface_Connectors interface_Connectors = new Interface_Connectors();
            List<string> m_Child_GUID = new List<string>();
            //Alle Client erhalten, welche aktuelles Element als Zeil haben
            List<string> m_Supplier_GUID = new List<string>();
            m_Supplier_GUID.Add(this.Classifier_ID);
            List<string> m_Type_Client = database.metamodel.m_Elements_SysArch_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_Client = database.metamodel.m_Elements_SysArch_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_con = database.metamodel.m_Decomposition_Element.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = database.metamodel.m_Decomposition_Element.Select(x => x.Stereotype).ToList();



            m_Child_GUID = interface_Connectors.Get_Client_Element_By_Connector(database, m_Supplier_GUID, m_Type_Client, m_Stereotype_Client, m_Type_con, m_Stereotype_con);

            if (m_Child_GUID != null)
            {
                // MessageBox.Show("Kinderelemente: " + m_Child_GUID.Count);
                if (m_Child_GUID.Count > 0) //Kinderelemente vorhanden
                {
                    int i1 = 0;
                    do
                    {
                        //Classifier des Kinderelements suchen
                        List<SysElement> m_NodeType_Child = database.m_SysElemente.Where(x => x.Classifier_ID == m_Child_GUID[i1]).ToList();

                        if (m_NodeType_Child.Count > 0) //Classifier vorhanden, wenn nicht ist es ein ungültiger Classifier
                        {
                            this.m_Child.Add(m_NodeType_Child[0]);
                            m_NodeType_Child[0].m_Parent.Add(this);
                        }



                        i1++;
                    } while (i1 < m_Child_GUID.Count);

                }

            }



        }

        public void Get_Children_Part_Sys(string GUID_Part, Database database, EA.Repository repository)
        {
            Repository_Element repository_element = new Repository_Element();

            List<string> m_Type = database.metamodel.m_Elements_SysArch_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype = database.metamodel.m_Elements_SysArch_Usage.Select(x => x.Stereotype).ToList();

            repository_element.Classifier_ID = GUID_Part;
            List<string> m_Children_GUID = repository_element.Get_Children_Guid(database, m_Type, m_Stereotype);

            if (m_Children_GUID != null)
            {
                int i1 = 0;
                do
                {
                    repository_element.Classifier_ID = m_Children_GUID[i1];
                    string Classifier = repository_element.Get_Classifier(database);

                    if (Classifier != null)
                    {
                        List<SysElement> m_help = database.m_SysElemente.Where(x => x.Classifier_ID == Classifier).ToList();

                        if (m_help.Count == 0)
                        {
                            SysElement recent_nt = new SysElement(Classifier, repository, database);
                            recent_nt.ID = repository_element.Get_Object_ID(database);
                            recent_nt.m_Instantiate.Add(m_Children_GUID[i1]);
                            recent_nt.Get_TV_Instantiate(database, repository);

                            recent_nt.Get_Children_Part(m_Children_GUID[i1], database, repository);

                            this.m_Child.Add(recent_nt);
                        }
                        else
                        {
                            if (m_help[0].m_Instantiate.Contains(m_Children_GUID[i1]) == false)
                            {
                                m_help[0].m_Instantiate.Add(m_Children_GUID[i1]);
                            }

                            if(this.m_Child.Where(x => x.Classifier_ID == Classifier).ToList().Count == 0)
                            {

                                this.m_Child.Add(m_help[0]);

                                if(m_help[0].m_Parent.Where(x => x.Classifier_ID == this.Classifier_ID).ToList().Count == 0)
                                {
                                    m_help[0].m_Parent.Add(this);
                                }

                            }
                        }

                       
                    }



                    i1++;
                } while (i1 < m_Children_GUID.Count);
            }
            //   return (Logical_all);


        }
        public void Transform_Implements()
        {
            if(this.m_m_Implements_guid.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_m_Implements_guid[i1].Count > 0)
                    {
                        this.m_Implements_guid.AddRange(m_m_Implements_guid[i1]);
                    }

                    i1++;
                } while (i1 < this.m_m_Implements_guid.Count);
            }
        }

        #endregion


        #region Start SysArch
        public void Get_Implements(Database database, EA.Repository repository)
        {
            Repository_Connectors rep_cons = new Repository_Connectors();

            List<string> m_Implentns_help = rep_cons.m_Get_Supplier(database, this.ID, database.metamodel.m_Implements.Select(x => x.Type).ToList(), database.metamodel.m_Implements.Select(x => x.Stereotype).ToList(), database.metamodel.m_Implements[0].direction); ;

            if(m_Implentns_help != null)
            {
                bool flag = false;

                int i1 = 0;
                do
                {
                    Repository_Element recent = new Repository_Element();

                    #region NodedType
                    //NodeType erhalten/anlegen/erweitern
                    NodeType nodeType = null;
                    flag = false;
                    recent.Classifier_ID = m_Implentns_help[i1];

                    if(recent.Get_Type(database) == "Class")
                    {
                        NodeType neu = new NodeType(null, null, null);
                        neu.Classifier_ID = recent.Classifier_ID;
                        nodeType = neu;
                        nodeType.Get_TV_reduced(database, repository);

                        List<NodeType> m_nodes = database.m_NodeType.Where(x => x.Classifier_ID == nodeType.Classifier_ID).ToList();
                        flag = true;
                        if (m_nodes.Count > 0)
                        {
                           
                            nodeType = m_nodes[0];
                            if (nodeType.m_Instantiate.Contains(m_Implentns_help[i1]) == false)
                            {
                                nodeType.m_Instantiate.Add(m_Implentns_help[i1]);
                            }
                          
                        }
                        else
                        {
                            nodeType.m_Instantiate.Add(m_Implentns_help[i1]);
                            database.m_NodeType.Add(nodeType);
                        }

                    }
                    else
                    {
                        string guid =  recent.Get_Classifier(database);

                        if(guid != null)
                        {
                            flag = true;
                            // recent.Classifier_ID = guid;
                            // nodeType = (NodeType)recent;

                            NodeType neu = new NodeType(null, null, null);
                            neu.Classifier_ID = guid;
                            nodeType = neu;

                            nodeType.Get_TV_reduced(database, repository);

                            List<NodeType> m_nodes = database.m_NodeType.Where(x => x.Classifier_ID == nodeType.Classifier_ID).ToList();

                            if (m_nodes.Count > 0)
                            {
                                
                                nodeType = m_nodes[0];
                                if (nodeType.m_Instantiate.Contains(m_Implentns_help[i1]) == false)
                                {
                                    nodeType.m_Instantiate.Add(m_Implentns_help[i1]);
                                }
                            }
                            else
                            {
                                nodeType.m_Instantiate.Add(m_Implentns_help[i1]);
                                database.m_NodeType.Add(nodeType);
                            }
                        }
                    }
                    #endregion

                    #region Verknüpfen
                    //Verknüpfen
                    if(flag == true)
                    {
                        if (this.m_Implements.Contains(nodeType) == false)
                        {
                            this.m_Implements.Add(nodeType);
                            nodeType.m_ImplementedBy.Add(this);

                        }
                    }
                    #endregion
                 
                    i1++;
                } while (i1 < m_Implentns_help.Count);
                //Sind die Implements in m_Instantiate der NodeType vorhanden?
              

            }

        }
    
        public void Get_Requirements_Sys(Database database, EA.Repository repository)
        {
            //Anforderungen erhalten der Implentierten NodeType
            if(this.m_Implements.Count > 0)
            {
                int i1 = 0;
                do
                {
                    //Alle zu exportierenden Anforderungen erhalten
                    this.m_Implements[i1].Get_Requirements(database, repository);

                    i1++;
                } while (i1 < this.m_Implements.Count);

                

            }
        }

        public void Copy_Requirements_Sys(EA.Repository repository, Database database)
        {
            if(this.m_Implements.Count > 0)
            {
                this.ID = this.Get_Object_ID(database);
                this.Name = this.Get_Name(database);
                this.Notes = this.Get_Notes(database);
                this.Get_TV_reduced(database, repository);
                Repository_Element repository_Element = new Repository_Element();
                Repository_Connector repository_Connector = new Repository_Connector();

                bool flag_req = false;

                int i1 = 0;
                do
                {
                    if(this.m_Implements[i1].m_requirements_all.Count > 0)
                    {
                        //this.m_Implements[i1].Get_TV_reduced(database, repository);
                        //Packages erzeugen
                        #region PAckages

                        //Packages anlegen
                        string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement_Plugin - Requirements", repository, database);
                        EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                        //Package InfoÃ¼bertragung anlegen bzw erhalten
                        string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model(this.Name+" - Kopierte Anforderung", repository, database);
                        EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                        Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                        Package_Requirement.Packages.Refresh();
                        Package_Infoübertragung.Update();
                        #endregion

                        int i2 = 0;
                        do
                        {
                            //Stereotypen erhalten
                            string stereo = this.m_Implements[i1].m_requirements_all[i2].Get_Stereotype(database);
                            //IntefaceRequireements müssen überprüft werden, ob diese eine Client Anforderung sind, wenn nicht ist dies hier die falsche AFO
                            //Neue AFO erzeugen
                            if (this.m_Implements[i1].m_requirements_all[i2].W_SUBJEKT.Contains(this.SYS_KUERZEL) == false)
                            {


                                if (this.m_Implements[i1].m_requirements_all[i2].W_SUBJEKT.Contains(this.m_Implements[i1].SYS_KUERZEL) == false)
                                {
                                    flag_req = false;
                                }
                                else
                                {
                                    flag_req = true;
                                }
                            }
                            else
                            {
                                flag_req = true;
                            }

                            if(flag_req == true)
                            {
                                Requirement new_req = new Requirement(null, database.metamodel);
                                new_req.Copy_Requirement(this.m_Implements[i1].m_requirements_all[i2], this, database);
                                //Überprüfung, ob copyfrom wirklich Syelement zugeorndet


                                if (database.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList().Contains(stereo) == true)
                                {
                                    new_req.Change_AFO_TExt_Interface(database, repository);

                                }
                                new_req.Create_Requirement(repository, Package_Infoübertragung_GUID, stereo, database);
                                this.m_requirements_all.Add(new_req);
                                //Neue AFO verknüpfen
                                //SysElement verknüpfen DerivedSysElem
                                if(new_req.Classifier_ID != null)
                                {
                                    new_req.Copy_Requirement_Connectoren(this.m_Implements[i1].m_requirements_all[i2], database, repository);
                                    //Refines auf alte AFO und alte AFO als Überschrift setzen
                                    repository_Connector.Create_Dependency(new_req.Classifier_ID, this.m_Implements[i1].m_requirements_all[i2].Classifier_ID, database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Refines.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Refines.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Refines[0].direction);
                                    repository_Connector.Create_Dependency(new_req.Classifier_ID, this.Classifier_ID, database.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), database.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), database.metamodel.m_Derived_SysElement.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Derived_SysElement.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Derived_SysElement[0].direction);


                                }
                                //Konektoren verknüpfen
                                this.m_Implements[i1].m_requirements_all[i2].AFO_WV_ART = AFO_WV_ART.Überschrift_Einleitung;
                                this.m_Implements[i1].m_requirements_all[i2].Update_TV_WV_Art(database, repository);
                            }
                           


                            i2++;
                        } while (i2 < this.m_Implements[i1].m_requirements_all.Count);
                    }

                    i1++;
                } while (i1 < this.m_Implements.Count);
            }


            //Neue Afo erstellen
           

            
        }

        #endregion
        public List<Activity> Get_Sys_ProcessActivity()
        {
            List<Activity> m_ret = new List<Activity>();

           m_ret =  this.m_Implements.SelectMany(x => x.Get_Process_Activity()).ToList().Distinct().ToList();


            return (m_ret);
        }

        #region Get

        public void Get_Implements_Logical(Database database, EA.Repository repository)
        {
            Repository_Connectors rep_cons = new Repository_Connectors();

            List<string> m_Implentns_help = rep_cons.m_Get_Supplier(database, this.ID, database.metamodel.m_Implements.Select(x => x.Type).ToList(), database.metamodel.m_Implements.Select(x => x.Stereotype).ToList(), database.metamodel.m_Implements[0].direction); ;

            if(m_Implentns_help != null)
            {
                Repsoitory_Elements.Repository_Element repository_Element = new Repository_Element();

                int i1 = 0;
                do
                {
                    repository_Element.Classifier_ID = m_Implentns_help[i1];
                    repository_Element.ID = repository_Element.Get_Object_ID(database);

                    string type = repository_Element.Get_Type(database);

                    switch(type)
                    {
                       case "Class":
                       {
                                List<NodeType> m_help = database.m_NodeType.Where(x => x.Classifier_ID == m_Implentns_help[i1]).ToList();

                                if (this.m_Implements.Select(x => x.Classifier_ID).Contains(m_Implentns_help[i1]) == false)
                                {
                                    this.m_Implements.Add(m_help[0]);
                                    this.m_m_Implements_guid[this.m_Implements.Count - 1] = m_help[0].m_Instantiate;
                                }
                                else
                                {
                                    if(m_help[0].m_Instantiate.Count > 0)
                                    {
                                        int index = this.m_Implements.Select(x => x.Classifier_ID).ToList().IndexOf(m_Implentns_help[i1]);

                                        List<string> m_help_str = this.m_m_Implements_guid[index];

                                        int i2 = 0;
                                        do
                                        {
                                            if(m_help_str.Contains(m_help[0].m_Instantiate[i2]) == false)
                                            {
                                                m_help_str.Add(m_help[0].m_Instantiate[i2]);
                                            }

                                            i2++;
                                        } while (i2 < m_help[0].m_Instantiate.Count);

                                        this.m_m_Implements_guid[index] = m_help_str;
                                    }
                                }
                           break;
                       }
                        case "Part":
                       {
                               string classifier =  repository_Element.Get_Classifier(database);

                                if(classifier != null)
                                {
                                    List<NodeType> m_help = database.m_NodeType.Where(x => x.Classifier_ID == classifier).ToList();

                                    if (this.m_Implements.Select(x => x.Classifier_ID).Contains(classifier) == false)
                                    {
                                        this.m_Implements.Add(m_help[0]);
                                        List<string> m_help2 = new List<string>();
                                        m_help2.Add(m_Implentns_help[i1]);
                                        this.m_m_Implements_guid.Add(m_help2);
                                    }
                                    else
                                    {
                                        if (m_help[0].m_Instantiate.Count > 0)
                                        {
                                            int index = this.m_Implements.Select(x => x.Classifier_ID).ToList().IndexOf(classifier);

                                            if(this.m_m_Implements_guid[index].Contains(m_Implentns_help[i1]) == false)
                                            {
                                                this.m_m_Implements_guid[index].Add(m_Implentns_help[i1]);
                                            }

                                           /* List<string> m_help_str = this.m_m_Implements_guid[index];

                                            int i2 = 0;
                                            do
                                            {
                                                if (m_help_str.Contains(m_help[0].m_Instantiate[i2]) == false)
                                                {
                                                    m_help_str.Add(m_help[0].m_Instantiate[i2]);
                                                }

                                                i2++;
                                            } while (i2 < m_help[0].m_Instantiate.Count);

                                            this.m_m_Implements_guid[index] = m_help_str;*/
                                        }
                                    }

                                }

                                

                                break;
                       }
                        default:
                            {
                                break;
                            }
                            
                    }

                     i1++;
                } while (i1 < m_Implentns_help.Count);
            }

        }

    public List<Element_Functional> Get_m_Element_Functional()
        {
            List<Element_Functional> m_ret = new List<Element_Functional>();

          
          //  m_ret = this.m_Implements.Select(x => x.m_Element_Functional).ToList().SelectMany(x => x).ToList();
            m_ret = this.m_Element_Functional;

            return (m_ret);
        }

        public List<Element_User> Get_m_Element_User()
        {
            List<Element_User> m_ret = new List<Element_User>();

            //m_ret = this.m_Implements.Select(x => x.m_Element_User).ToList().SelectMany(x => x).ToList();

            m_ret = this.m_Element_User;

            return (m_ret);
        }

        public List<Element_Design> Get_m_Element_Design()
        {
            List<Element_Design> m_ret = new List<Element_Design>();

            // m_ret = this.m_Implements.Select(x => x.m_Design).ToList().SelectMany(x => x).ToList();

            m_ret = this.m_Design;

            return (m_ret);
        }

        public List<Element_Typvertreter> Get_m_Element_Typvertreter()
        {
            List<Element_Typvertreter> m_ret = new List<Element_Typvertreter>();

            //  m_ret = this.m_Implements.Select(x => x.m_Typvertreter).ToList().SelectMany(x => x).ToList();

            m_ret = this.m_Typvertreter;

            return (m_ret);
        }

        public List<Element_Environmental> Get_m_Element_Environment()
        {
            List<Element_Environmental> m_ret = new List<Element_Environmental>();

            // m_ret = this.m_Implements.Select(x => x.m_Enviromental).ToList().SelectMany(x => x).ToList();

            m_ret = this.m_Enviromental;

            return (m_ret);
        }

        public List<SysElement> Get_All_SpecifiedBy(List<SysElement> m_NodeType)
        {
            List<SysElement> m_ret = new List<SysElement>();

            List<SysElement> m_help_General = m_NodeType.Where(x => x.m_Specialize.Contains(this) == true).ToList();


            if (m_help_General.Count > 0)
            {
                m_ret = m_help_General;

                int i1 = 0;
                do
                {
                    List<SysElement> m_help = new List<SysElement>();
                    m_help = m_help_General[i1].Get_All_SpecifiedBy(m_NodeType);

                    if (m_help != null)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < m_help_General.Count);

                return (m_ret);

            }
            else
            {
                return (null);
            }

        }

        public void Get_Sys_StereoType_Import( Database Data)
        {
            string stereo = this.Get_Stereotype(Data);
            List<Element_Metamodel> m_elem = Data.metamodel.m_Elements_SysArch_Definition.Where(x => x.Stereotype == stereo).ToList();

            this.SYS_KOMPONENTENTYP =  (SYS_KOMPONENTENTYP)Data.SYS_ENUM.Get_Index(Data.SYS_ENUM.SYS_KOMPONENTENTYP ,m_elem[0].XAC_Attribut);
               

        }

        public List<List<Repository_Element>> Check_Dopplung_Functional(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

                #region Kinderelemente
                List<SysElement> m_Child = this.Get_All_Children_Sys();

                if (m_Child != null)
                {
                    int i1 = 0;
                    do
                    {
                        List<Activity> m_same = new List<Activity>();
                        List<Activity> m_same1 = this.Get_Process_Activity();
                        List<Activity> m_same2 = m_Child[i1].Get_Process_Activity();

                        if (m_same1.Count > 0 && m_same2.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_same2.Select(x => x.Classifier_ID).Contains(m_same1.Select(x => x.Classifier_ID).ToList()[i2]) == true)
                                {
                                    m_same.Add(m_same1[i2]);
                                }

                                i2++;
                            } while (i2 < m_same1.Count);
                        }
                        else
                        {
                            m_same = new List<Activity>();
                        }

                        //  List<Activity> m_same = this.Get_Process_Activity().Intersect(m_Child[i1].Get_Process_Activity()).ToList();

                        if (m_same.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(this);
                                m_help.Add(m_Child[i1]);
                                m_help.Add(m_same[i2]);

                                m_Dopplung.Add(m_help);

                                i2++;
                            } while (i2 < m_same.Count);
                        }

                        i1++;
                    } while (i1 < m_Child.Count);
                }
                #endregion

                #region Genralisierungen

                List<NodeType> m_Specifiy = this.Get_All_Specifies();

                if (m_Specifiy != null)
                {
                    int i1 = 0;
                    do
                    {
                        List<Activity> m_same = new List<Activity>();
                        List<Activity> m_same1 = this.Get_Process_Activity();
                        List<Activity> m_same2 = m_Specifiy[i1].Get_Process_Activity();

                        if (m_same1.Count > 0 && m_same2.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_same2.Select(x => x.Classifier_ID).Contains(m_same1.Select(x => x.Classifier_ID).ToList()[i2]) == true)
                                {
                                    m_same.Add(m_same1[i2]);
                                }

                                i2++;
                            } while (i2 < m_same1.Count);
                        }
                        else
                        {
                            m_same = new List<Activity>();
                        }




                        //  List<Activity> m_same = this.Get_Process_Activity().Intersect(m_Specifiy[i1].Get_Process_Activity()).ToList();

                        if (m_same.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(this);
                                m_help.Add(m_Specifiy[i1]);
                                m_help.Add(m_same[i2]);

                                m_Dopplung.Add(m_help);

                                i2++;
                            } while (i2 < m_same.Count);
                        }

                        i1++;
                    } while (i1 < m_Specifiy.Count);
                }
                #endregion

                #region Genralisierungen hoch
                List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

                if (m_Specified != null)
                {
                    int i1 = 0;
                    do
                    {
                        List<Activity> m_same = new List<Activity>();
                        List<Activity> m_same1 = this.Get_Process_Activity();
                        List<Activity> m_same2 = m_Specified[i1].Get_Process_Activity();

                        if (m_same1.Count > 0 && m_same2.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (m_same2.Select(x => x.Classifier_ID).Contains(m_same1.Select(x => x.Classifier_ID).ToList()[i2]) == true)
                                {
                                    m_same.Add(m_same1[i2]);
                                }

                                i2++;
                            } while (i2 < m_same1.Count);
                        }
                        else
                        {
                            m_same = new List<Activity>();
                        }

                        // List<Activity> m_same = this.Get_Process_Activity().Intersect(m_Specified[i1].Get_Process_Activity()).ToList();

                        if (m_same.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();

                                m_help.Add(m_Specified[i1]);
                                m_help.Add(this);
                                m_help.Add(m_same[i2]);

                                m_Dopplung.Add(m_help);

                                i2++;
                            } while (i2 < m_same.Count);
                        }

                        i1++;
                    } while (i1 < m_Specified.Count);
                }
                #endregion

                #region Dopplung Functional und User

                if (this.m_Element_Functional.Count > 0 && this.m_Element_User.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (this.m_Element_Functional[i1].m_Requirement_Functional.Count > 0)
                        {
                            //Check auf Gleiche 
                        }


                        i1++;
                    } while (i1 < this.m_Element_Functional.Count);
                }

                #endregion
            


            return (m_Dopplung);
        }

        /*    public List<List<Repository_Element>> Check_Dopplung_User(Database database, EA.Repository repository)
            {
                List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

                #region Dopplung Functional und User

                if (this.m_Element_Functional.Count > 0 && this.m_Element_User.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (this.m_Element_Functional[i1].m_Requirement_Functional.Count > 0)
                        {
                            //Check auf Gleiche Activity
                            List<Element_User> m_check = this.m_Element_User.Where(x => x.Supplier == this.m_Element_Functional[i1].Supplier).ToList();

                            if (m_check.Count > 0)
                            {
                                if (m_check[0].m_Requirement_User.Count > 0)
                                {
                                    List<Repository_Element> m_help = new List<Repository_Element>();
                                    m_help.Add(this);
                                    m_help.Add(this.m_Element_Functional[i1].m_Requirement_Functional[0]);
                                    m_help.Add(m_check[0].m_Requirement_User[0]);


                                    m_Dopplung.Add(m_help);
                                }

                            }
                        }


                        i1++;
                    } while (i1 < this.m_Element_Functional.Count);
                }

                #endregion

                return (m_Dopplung);
            }*/
        #region Issues Dopplung
        public void Create_Issue_Dopplung_Functional(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID, bool create_issue)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Requirement_Functional> m_Parent_func_req = m_Parent_func.SelectMany(x => x.m_Requirement_Functional).ToList();
                List<Requirement_User> m_Parent_user_req = m_Parent_user.SelectMany(x => x.m_Requirement_User).ToList();
                List<Requirement_Functional> m_Child_func_req = m_Child_func.SelectMany(x => x.m_Requirement_Functional).ToList();
                List<Requirement_User> m_Child_user_req = m_Child_user.SelectMany(x => x.m_Requirement_User).ToList();

                #region Req_Functional Dopplung
                if (m_Parent_func_req.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_req.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_req[i3] != null && m_Parent_func_req[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_req[i3].Classifier_ID, m_Parent_func_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_req.Count);
                        }
                        if (m_Child_user_req.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_req[i3] != null && m_Parent_func_req[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_req[i3].Classifier_ID, m_Parent_func_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_user_req.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_func_req.Count);
                }
                #endregion
                #region Req User Dopplung
                if (m_Parent_user_req.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_req.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_req[i3] != null && m_Parent_user_req[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_req[i3].Classifier_ID, m_Parent_user_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_func_req.Count);
                        }
                        if (m_Child_user_req.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_req[i3] != null && m_Parent_user_req[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_req[i3].Classifier_ID, m_Parent_user_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_user_req.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_user_req.Count);
                }
                #endregion
                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                if (create_issue == true)
                {
                    #region Issue
                    string name = Parent.Name + " und " + Child.Name + " sind mit derselben Activity " + activity.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                    #endregion


                    //Instanzen mit Issue verküpfen
                    List<string> m_Parent_func_client_guid = m_Parent_func.SelectMany(x => x.Get_Client_GUID_Target()).ToList();
                    List<string> m_Parent_user_client_guid = m_Parent_user.SelectMany(x => x.Get_Client_GUID_Target_User()).ToList();
                    List<string> m_Parent_func_target_guid = m_Parent_func.SelectMany(x => x.Get_Supplier_GUID_Target()).ToList();
                    List<string> m_Parent_user_target_guid = m_Parent_user.SelectMany(x => x.Get_Supplier_GUID_Target_User()).ToList();
                    List<string> m_Child_func_client_guid = m_Child_func.SelectMany(x => x.Get_Client_GUID_Target()).ToList();
                    List<string> m_Childt_user_client_guid = m_Child_user.SelectMany(x => x.Get_Client_GUID_Target_User()).ToList();
                    List<string> m_Child_func_target_guid = m_Child_func.SelectMany(x => x.Get_Supplier_GUID_Target()).ToList();
                    List<string> m_Child_user_target_guid = m_Child_user.SelectMany(x => x.Get_Supplier_GUID_Target_User()).ToList();

                    if (m_Parent_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_client_guid.Count);
                    }
                    if (m_Parent_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_client_guid.Count);
                    }
                    if (m_Parent_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_target_guid.Count);
                    }
                    if (m_Parent_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_target_guid.Count);
                    }

                    if (m_Child_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_client_guid.Count);
                    }
                    if (m_Childt_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Childt_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Childt_user_client_guid.Count);
                    }
                    if (m_Child_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_target_guid.Count);
                    }
                    if (m_Child_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_user_target_guid.Count);
                    }
                }




                i1++;
            } while (i1 < m_Tupel.Count);
        }

        #region Process
        public List<List<Repository_Element>> Check_Dopplung_Process(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            int i1 = 0;
            do
            {
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];

                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();

                List<Element_Process> m_Parent_func_process = m_Parent_func.SelectMany(x => x.m_element_Processes).ToList();
                List<Element_Process> m_Parent_user_process = m_Parent_user.SelectMany(x => x.m_element_Processes).ToList();
                List<Element_Process> m_Child_func_process = m_Child_func.SelectMany(x => x.m_element_Processes).ToList();
                List<Element_Process> m_Child_user_process = m_Child_user.SelectMany(x => x.m_element_Processes).ToList();

                List<OperationalConstraint> m_Parent_func_process_constraint = m_Parent_func_process.Select(x => x.OpConstraint).ToList();
                List<OperationalConstraint> m_Parent_user_process_constraint = m_Parent_user_process.Select(x => x.OpConstraint).ToList();
                List<OperationalConstraint> m_Child_func_process_constraint = m_Child_func_process.Select(x => x.OpConstraint).ToList();
                List<OperationalConstraint> m_Child_user_process_constraint = m_Child_user_process.Select(x => x.OpConstraint).ToList();

                #region Functional
                if (m_Parent_func_process_constraint.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_process_constraint.Count > 0)
                        {
                            if (m_Child_func_process_constraint.Contains(m_Parent_func_process_constraint[i2]) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }
                        if (m_Child_user_process_constraint.Count > 0)
                        {
                            if (m_Child_user_process_constraint.Contains(m_Parent_func_process_constraint[i2]) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }

                        i2++;
                    } while (i2 < m_Parent_func_process_constraint.Count);
                }
                #endregion
                #region User
                if (m_Parent_user_process_constraint.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_process_constraint.Count > 0)
                        {
                            if (m_Child_func_process_constraint.Contains(m_Parent_user_process_constraint[i2]) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_user_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }
                        if (m_Child_user_process_constraint.Count > 0)
                        {
                            if (m_Child_user_process_constraint.Contains(m_Parent_user_process_constraint[i2]) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }

                        i2++;
                    } while (i2 < m_Parent_user_process_constraint.Count);
                }
                #endregion

                i1++;
            } while (i1 < m_Tupel.Count);



            return (m_Dopplung);
        }

        public void Create_Issue_Dopplung_Process(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID, bool create_issue)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];
                OperationalConstraint opcon = (OperationalConstraint)m_Tupel[i1][3];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();

                List<Element_Process> m_Parent_func_Elemnte_Process = m_Parent_func.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == opcon).ToList();
                List<Element_Process> m_Parent_user_Elemnte_Process = m_Parent_user.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == opcon).ToList();
                List<Element_Process> m_Child_func_Elemnte_Process = m_Child_func.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == opcon).ToList();
                List<Element_Process> m_Child_user_Elemnte_Process = m_Child_user.SelectMany(x => x.m_element_Processes).ToList().Where(x => x.OpConstraint == opcon).ToList();

                List<Requirement_Non_Functional> m_Parent_func_Req_Process = m_Parent_func_Elemnte_Process.Select(x => x.Requirement_Process).ToList();
                List<Requirement_Non_Functional> m_Parent_user_Req_Process = m_Parent_user_Elemnte_Process.Select(x => x.Requirement_Process).ToList();
                List<Requirement_Non_Functional> m_Child_func_Req_Process = m_Child_func_Elemnte_Process.Select(x => x.Requirement_Process).ToList();
                List<Requirement_Non_Functional> m_Child_user_Req_Process = m_Child_user_Elemnte_Process.Select(x => x.Requirement_Process).ToList();


                #region Req_Functional Dopplung
                if (m_Parent_func_Req_Process.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_Req_Process[i3] != null && m_Parent_func_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_Req_Process[i3].Classifier_ID, m_Parent_func_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_Req_Process.Count);
                        }
                        if (m_Child_user_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_Req_Process[i3] != null && m_Parent_func_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_Req_Process[i3].Classifier_ID, m_Parent_func_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_user_Req_Process.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_func_Req_Process.Count);
                }
                #endregion
                #region Req User Dopplung
                if (m_Parent_user_Req_Process.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_Req_Process[i3] != null && m_Parent_user_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_Req_Process[i3].Classifier_ID, m_Parent_user_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_Req_Process.Count);
                        }
                        if (m_Child_user_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_Req_Process[i3] != null && m_Parent_user_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_Req_Process[i3].Classifier_ID, m_Parent_user_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_user_Req_Process.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_user_Req_Process.Count);
                }
                #endregion
                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                if (create_issue == true)
                {
                    #region Issue
                    string name = Parent.Name + " und " + Child.Name + " sind mit derselben Activity " + activity.Name + " und OperationalConstraint " + opcon.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                    #endregion
                    //Instanzen mit Issue verküpfen
                    List<string> m_Parent_func_client_guid = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_Node_ID).ToList();
                    List<string> m_Parent_user_client_guid = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_Node_ID).ToList();
                    List<string> m_Parent_func_target_guid = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_Action_ID).ToList();
                    List<string> m_Parent_user_target_guid = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_Action_ID).ToList();
                    List<string> m_Child_func_client_guid = m_Child_func_Elemnte_Process.SelectMany(x => x.m_Node_ID).ToList();
                    List<string> m_Childt_user_client_guid = m_Child_user_Elemnte_Process.SelectMany(x => x.m_Node_ID).ToList();
                    List<string> m_Child_func_target_guid = m_Child_func_Elemnte_Process.SelectMany(x => x.m_Action_ID).ToList();
                    List<string> m_Child_user_target_guid = m_Child_user_Elemnte_Process.SelectMany(x => x.m_Action_ID).ToList();

                    if (m_Parent_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_client_guid.Count);
                    }
                    if (m_Parent_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_client_guid.Count);
                    }
                    if (m_Parent_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_target_guid.Count);
                    }
                    if (m_Parent_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_target_guid.Count);
                    }

                    if (m_Child_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_client_guid.Count);
                    }
                    if (m_Childt_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Childt_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Childt_user_client_guid.Count);
                    }
                    if (m_Child_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_target_guid.Count);
                    }
                    if (m_Child_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_user_target_guid.Count);
                    }
                    repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);



                }


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion

        #region Design
        public List<List<Repository_Element>> Check_Dopplung_Design(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            #region Kinerelemente
            List<SysElement> m_Child = this.Get_All_Children_Sys();

            if (m_Child != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList().Intersect(m_Child[i1].Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            OperationalConstraint opcon = database.m_DesignConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Specifies();

            if (m_Specify != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList().Intersect(m_Specify[i1].Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add((SysElement)m_Specify[i1]);

                            OperationalConstraint opcon = database.m_DesignConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Genralisierungen hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList().Intersect(m_Specified[i1].Get_OpCon_Deisgn().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();

                            m_help.Add((SysElement)m_Specified[i1]);
                            m_help.Add(this);

                            OperationalConstraint opcon = database.m_DesignConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_Dopplung);
        }
        public void Create_Issue_Dopplung_Design(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                OperationalConstraint opcon = (OperationalConstraint)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Design> m_Parent_design = Parent.m_Design.Where(x => x.OpConstraint.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Element_Design> m_Child_design = Child.m_Design.Where(x => x.OpConstraint.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Requirement_Non_Functional> m_Parent_req_design = m_Parent_design.Select(x => x.requirement).ToList();
                List<Requirement_Non_Functional> m_Child_req_design = m_Child_design.Select(x => x.requirement).ToList();

                #region Req_Design Dopplung
                if (m_Parent_req_design.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_design.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_design[i3] != null && m_Parent_req_design[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_design[i3].Classifier_ID, m_Parent_req_design[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_design.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_design.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind mit demselben Constraint" + opcon.Name + " verknüpft";
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_desing_client_guid = m_Parent_design.SelectMany(x => x.m_GUID).ToList();
                List<string> m_Child_design_client_guid = m_Child_design.SelectMany(x => x.m_GUID).ToList();

                if (m_Parent_desing_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_desing_client_guid.Count);
                }
                if (m_Child_design_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_design_client_guid.Count);
                }

                repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion

        #region Typvertreter
        public List<List<Repository_Element>> Check_Dopplung_Typverteter(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Typvertreter = new List<List<Repository_Element>>();

            #region Kinderelemente
            List<SysElement> m_Child = this.Get_All_Children_Sys();

            if (m_Child != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Typvertreter().Select(x => x.Classifier_ID).ToList().Intersect(m_Child[i1].Get_Typvertreter().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            SysElement opcon = database.m_SysElemente.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Specifies();

            if (m_Specify != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Typvertreter().Select(x => x.Classifier_ID).ToList().Intersect(m_Specify[i1].Get_Typvertreter().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add((SysElement)m_Specify[i1]);

                            SysElement opcon = database.m_SysElemente.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Typvertreter().Select(x => x.Classifier_ID).ToList().Intersect(m_Specified[i1].Get_Typvertreter().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();

                            m_help.Add((SysElement)m_Specified[i1]);
                            m_help.Add(this);

                            SysElement opcon = database.m_SysElemente.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_Typvertreter);
        }

        public void Create_Issue_Dopplung_Typvertreter(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                SysElement opcon = (SysElement)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Typvertreter> m_Parent_design = Parent.m_Typvertreter.Where(x => x.Typvertreter.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Element_Typvertreter> m_Child_design = Child.m_Typvertreter.Where(x => x.Typvertreter.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Requirement_Non_Functional> m_Parent_req_design = m_Parent_design.Select(x => x.requirement).ToList();
                List<Requirement_Non_Functional> m_Child_req_design = m_Child_design.Select(x => x.requirement).ToList();

                #region Req_Design Dopplung
                if (m_Parent_req_design.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_design.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_design[i3] != null && m_Parent_req_design[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_design[i3].Classifier_ID, m_Parent_req_design[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_design.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_design.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind durch denselben Typvertreter" + opcon.Name + " verknüpft";
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_desing_client_guid = m_Parent_design.SelectMany(x => x.m_GUID).ToList();
                List<string> m_Child_design_client_guid = m_Child_design.SelectMany(x => x.m_GUID).ToList();

                if (m_Parent_desing_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_desing_client_guid.Count);
                }
                if (m_Child_design_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_design_client_guid.Count);
                }

                repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion

        #region Umwelt
        public List<List<Repository_Element>> Check_Dopplung_Umwelt(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Typvertreter = new List<List<Repository_Element>>();

            #region Kinderelemente
            List<SysElement> m_Child = this.Get_All_Children_Sys();

            if (m_Child != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Umwelt().Select(x => x.Classifier_ID).ToList().Intersect(m_Child[i1].Get_Umwelt().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            OperationalConstraint opcon = database.m_UmweltConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Genralisierung
            List<NodeType> m_Specifiy = this.Get_All_Specifies();

            if (m_Specifiy != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Umwelt().Select(x => x.Classifier_ID).ToList().Intersect(m_Specifiy[i1].Get_Umwelt().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add((SysElement)m_Specifiy[i1]);

                            OperationalConstraint opcon = database.m_UmweltConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specifiy.Count);
            }
            #endregion

            #region Genralisierung hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Umwelt().Select(x => x.Classifier_ID).ToList().Intersect(m_Specified[i1].Get_Umwelt().Select(x => x.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();

                            m_help.Add((SysElement)m_Specified[i1]);
                            m_help.Add(this);

                            OperationalConstraint opcon = database.m_UmweltConstraint.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(opcon);

                            m_Typvertreter.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_Typvertreter);
        }
        public void Create_Issue_Dopplung_Umwelt(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                OperationalConstraint opcon = (OperationalConstraint)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Environmental> m_Parent_design = Parent.m_Enviromental.Where(x => x.OpConstraint.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Element_Environmental> m_Child_design = Child.m_Enviromental.Where(x => x.OpConstraint.Classifier_ID == opcon.Classifier_ID).ToList();
                List<Requirement_Non_Functional> m_Parent_req_design = m_Parent_design.Select(x => x.requirement).ToList();
                List<Requirement_Non_Functional> m_Child_req_design = m_Child_design.Select(x => x.requirement).ToList();

                #region Req_Design Dopplung
                if (m_Parent_req_design.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_design.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_design[i3] != null && m_Parent_req_design[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_design[i3].Classifier_ID, m_Parent_req_design[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_design.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_design.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind durch mit demselbem Umweltelement " + opcon.Name + " verknüpft";
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_desing_client_guid = m_Parent_design.SelectMany(x => x.m_GUID).ToList();
                List<string> m_Child_design_client_guid = m_Child_design.SelectMany(x => x.m_GUID).ToList();

                if (m_Parent_desing_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_desing_client_guid.Count);
                }
                if (m_Child_design_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_design_client_guid.Count);
                }

                repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion

        #region Interface
        public List<List<Repository_Element>> Check_Dopplung_Interface_Unidirektional(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            #region Kinderelemente
            List<SysElement> m_Child = this.Get_All_Children_Sys();

            if (m_Child != null)
            {
                List<NodeType> m_Parent_Uni_Supplier = this.m_Element_Interface.Select(x => x.Supplier).ToList();


                int i1 = 0;
                do
                {
                    List<NodeType> m_Child_Uni_Supplier = m_Child[i1].m_Element_Interface.Select(x => x.Supplier).ToList();



                    List<NodeType> m_same = m_Parent_Uni_Supplier.Intersect(m_Child_Uni_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Specifies();

            if (m_Specify != null)
            {
                List<NodeType> m_Parent_Uni_Supplier = this.m_Element_Interface.Select(x => x.Supplier).ToList();


                int i1 = 0;
                do
                {
                    List<NodeType> m_Child_Uni_Supplier = m_Specify[i1].m_Element_Interface.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Uni_Supplier.Intersect(m_Child_Uni_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add((SysElement)m_Specify[i1]);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                List<NodeType> m_Parent_Uni_Supplier = this.m_Element_Interface.Select(x => x.Supplier).ToList();


                int i1 = 0;
                do
                {
                    List<NodeType> m_Child_Uni_Supplier = m_Specified[i1].m_Element_Interface.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Uni_Supplier.Intersect(m_Child_Uni_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();

                            m_help.Add((SysElement)m_Specified[i1]);
                            m_help.Add(this);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_Dopplung);
        }
        public List<List<Repository_Element>> Check_Dopplung_Interface_Bidirektional(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            #region Kinderelemente
            List<SysElement> m_Child = this.Get_All_Children_Sys();

            if (m_Child != null)
            {
                List<NodeType> m_Parent_Bi_Supplier = this.m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();

                int i1 = 0;
                do
                {

                    List<NodeType> m_Child_Bi_Supplier = m_Child[i1].m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Bi_Supplier.Intersect(m_Child_Bi_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }
                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Children();

            if (m_Specify != null)
            {
                List<NodeType> m_Parent_Bi_Supplier = this.m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();

                int i1 = 0;
                do
                {

                    List<NodeType> m_Child_Bi_Supplier = m_Specify[i1].m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Bi_Supplier.Intersect(m_Child_Bi_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add((SysElement)m_Specify[i1]);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }
                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                List<NodeType> m_Parent_Bi_Supplier = this.m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();

                int i1 = 0;
                do
                {

                    List<NodeType> m_Child_Bi_Supplier = m_Specified[i1].m_Element_Interface_Bidirectional.Select(x => x.Supplier).ToList();
                    List<NodeType> m_same = m_Parent_Bi_Supplier.Intersect(m_Child_Bi_Supplier).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();

                            m_help.Add((SysElement)m_Specified[i1]);
                            m_help.Add(this);

                            m_help.Add(m_same[i2]);

                            m_Dopplung.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }
                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion
            return (m_Dopplung);
        }
        public void Create_Issue_Dopplung_Interface_Unidirektional(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                NodeType Supplier = (NodeType)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Interface> m_Parent_interface = Parent.m_Element_Interface.Where(x => x.Supplier.Classifier_ID == Supplier.Classifier_ID).ToList();
                List<Element_Interface> m_Child_interface = Child.m_Element_Interface.Where(x => x.Supplier.Classifier_ID == Supplier.Classifier_ID).ToList();
                List<Requirement_Interface> m_Parent_req_send = m_Parent_interface.SelectMany(x => x.m_Requirement_Interface_Send).ToList();
                List<Requirement_Interface> m_Parent_req_receive = m_Parent_interface.SelectMany(x => x.m_Requirement_Interface_Receive).ToList();
                List<Requirement_Interface> m_Child_req_send = m_Child_interface.SelectMany(x => x.m_Requirement_Interface_Send).ToList();
                List<Requirement_Interface> m_Child_req_receive = m_Child_interface.SelectMany(x => x.m_Requirement_Interface_Receive).ToList();

                #region Req send Dopplung
                if (m_Parent_req_send.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_send.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_send[i3] != null && m_Parent_req_send[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_send[i3].Classifier_ID, m_Parent_req_send[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_send.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_send.Count);
                }
                #endregion
                #region Req receive Dopplung
                if (m_Parent_req_receive.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_receive.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_receive[i3] != null && m_Parent_req_receive[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_receive[i3].Classifier_ID, m_Parent_req_receive[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_receive.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_receive.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind haben eine unidirektionale Schnittstelle mit " + Supplier.Name;
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_client_guid = m_Parent_interface.SelectMany(x => x.m_Target.Select(y => y.CLient_ID).ToList()).ToList();
                List<string> m_Parent_supplier_guid = m_Parent_interface.SelectMany(x => x.m_Target.Select(y => y.Supplier_ID).ToList()).ToList();
                List<string> m_Child_client_guid = m_Child_interface.SelectMany(x => x.m_Target.Select(y => y.CLient_ID).ToList()).ToList();
                List<string> m_Child_supplier_guid = m_Child_interface.SelectMany(x => x.m_Target.Select(y => y.Supplier_ID).ToList()).ToList();



                if (m_Parent_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {

                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_client_guid.Count);
                }
                if (m_Parent_supplier_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_supplier_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_supplier_guid.Count);
                }
                if (m_Child_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {

                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_client_guid.Count);
                }
                if (m_Child_supplier_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_supplier_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_supplier_guid.Count);
                }

                // repository_Connector.Create_Dependency(issue.Classifier_ID, Supplier.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database);


                i1++;
            } while (i1 < m_Tupel.Count);
        }

        public void Create_Issue_Dopplung_Interface_Bidirektional(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                //Elemente
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                NodeType Supplier = (NodeType)m_Tupel[i1][2];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Interface_Bidirectional> m_Parent_interface = Parent.m_Element_Interface_Bidirectional.Where(x => x.Supplier.Classifier_ID == Supplier.Classifier_ID).ToList();
                List<Element_Interface_Bidirectional> m_Child_interface = Child.m_Element_Interface_Bidirectional.Where(x => x.Supplier.Classifier_ID == Supplier.Classifier_ID).ToList();
                List<Requirement_Interface> m_Parent_req_send = m_Parent_interface.SelectMany(x => x.m_Requirement_Interface_Send).ToList();
                List<Requirement_Interface> m_Parent_req_receive = m_Parent_interface.SelectMany(x => x.m_Requirement_Interface_Receive).ToList();
                List<Requirement_Interface> m_Child_req_send = m_Child_interface.SelectMany(x => x.m_Requirement_Interface_Send).ToList();
                List<Requirement_Interface> m_Child_req_receive = m_Child_interface.SelectMany(x => x.m_Requirement_Interface_Receive).ToList();

                #region Req send Dopplung
                if (m_Parent_req_send.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_send.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_send[i3] != null && m_Parent_req_send[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_send[i3].Classifier_ID, m_Parent_req_send[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_send.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_send.Count);
                }
                #endregion
                #region Req receive Dopplung
                if (m_Parent_req_receive.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_req_receive.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_req_receive[i3] != null && m_Parent_req_receive[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_req_receive[i3].Classifier_ID, m_Parent_req_receive[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_req_receive.Count);
                        }


                        i2++;
                    } while (i2 < m_Parent_req_receive.Count);
                }
                #endregion

                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                #region Issue
                string name = Parent.Name + " und " + Child.Name + " sind haben eine bidirektionale Schnittstelle mit " + Supplier.Name;
                string notes = "Diese Dopplung muss aufgelöst werden.";
                string type = database.metamodel.m_Issue[7].Stereotype;
                Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                #endregion 
                //Instanzen mit Issue verküpfen
                List<string> m_Parent_client_guid = m_Parent_interface.SelectMany(x => x.m_Target.Select(y => y.CLient_ID).ToList()).ToList();
                List<string> m_Parent_supplier_guid = m_Parent_interface.SelectMany(x => x.m_Target.Select(y => y.Supplier_ID).ToList()).ToList();
                List<string> m_Child_client_guid = m_Child_interface.SelectMany(x => x.m_Target.Select(y => y.CLient_ID).ToList()).ToList();
                List<string> m_Child_supplier_guid = m_Child_interface.SelectMany(x => x.m_Target.Select(y => y.Supplier_ID).ToList()).ToList();



                if (m_Parent_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {

                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_client_guid.Count);
                }
                if (m_Parent_supplier_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_supplier_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Parent_supplier_guid.Count);
                }
                if (m_Child_client_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {

                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_client_guid.Count);
                }
                if (m_Child_supplier_guid.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_supplier_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                        i2++;
                    } while (i2 < m_Child_supplier_guid.Count);
                }

                // repository_Connector.Create_Dependency(issue.Classifier_ID, Supplier.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database);


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion

        #region QualityClass
        public List<List<Repository_Element>> Check_Dopplung_QualityClass(Database database, EA.Repository repository)
        {
            List<List<Repository_Element>> m_QualityClass = new List<List<Repository_Element>>();

            #region Eigene Ebene
            if (this.m_Element_Measurement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    List<Requirement_Plugin.Repository_Elements.Measurement> m_same = this.Get_Measurement().Where(x => x.measurementType == this.m_Element_Measurement[i1].Measurement.measurementType).ToList();

                    if (m_same.Count > 1)
                    {
                        List<Repository_Element> m_help = new List<Repository_Element>();

                        m_help.Add(this);
                        m_help.Add(null);
                        m_help.Add(this.m_Element_Measurement[i1].Measurement.measurementType);

                        m_QualityClass.Add(m_help);
                    }


                    i1++;
                } while (i1 < this.m_Element_Measurement.Count);
            }

            #endregion 

            #region Kinderelemente
            List<SysElement> m_Child = this.Get_All_Children_Sys();

            if (m_Child != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList().Intersect(m_Child[i1].Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList()).ToList();


                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            /* List<Elements.Element_Measurement> m_recent_measure = new List<Elements.Element_Measurement>();
                             m_recent_measure = this.m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == m_same[i2]).ToList();
                             List<Elements.Element_Measurement> m_child_measure = new List<Elements.Element_Measurement>();
                             m_child_measure = m_Child[i1].m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == m_same[i2]).ToList();*/
                            Requirement_Plugin.Repository_Elements.MeasurementType mearue_type = database.m_MeasurementType.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];


                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add(m_Child[i1]);
                            m_help.Add(mearue_type);
                            //  m_help.AddRange(m_recent_measure.Select(x => x.Measurement).ToList());
                            //  m_help.AddRange(m_child_measure.Select(x => x.Measurement).ToList());

                            //SysElement opcon = database.m_SysElemente.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            //m_help.Add(opcon);

                            m_QualityClass.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Child.Count);
            }
            #endregion

            #region Generalisierung
            List<NodeType> m_Specify = this.Get_All_Specifies();

            if (m_Specify != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList().Intersect(m_Specify[i1].Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList()).ToList();

                    //List<OperationalConstraint> m_same = this.Get_OpCon_Deisgn().Intersect(m_Child[i1].Get_OpCon_Deisgn()).ToList();



                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();
                            m_help.Add(this);
                            m_help.Add((SysElement)m_Specify[i1]);

                            Requirement_Plugin.Repository_Elements.MeasurementType mearue_type = database.m_MeasurementType.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(mearue_type);

                            m_QualityClass.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specify.Count);
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                int i1 = 0;
                do
                {
                    List<string> m_same = this.Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList().Intersect(m_Specified[i1].Get_Measurement().Select(x => x.measurementType.Classifier_ID).ToList()).ToList();

                    if (m_same.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Repository_Element> m_help = new List<Repository_Element>();

                            m_help.Add((SysElement)m_Specified[i1]);
                            m_help.Add(this);

                            Requirement_Plugin.Repository_Elements.MeasurementType mearue_type = database.m_MeasurementType.Where(x => x.Classifier_ID == m_same[i2]).ToList()[0];

                            m_help.Add(mearue_type);

                            m_QualityClass.Add(m_help);

                            i2++;
                        } while (i2 < m_same.Count);
                    }

                    i1++;
                } while (i1 < m_Specified.Count);
            }
            #endregion

            return (m_QualityClass);
        }

        public void Create_Issue_Dopplung_QualityClass(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();


            int i1 = 0;
            do
            {
                #region Selbst
                if (m_Tupel[i1][1] == null)
                {
                    SysElement Parent = (SysElement)m_Tupel[i1][0];
                    Requirement_Plugin.Repository_Elements.MeasurementType measure_Type = (Requirement_Plugin.Repository_Elements.MeasurementType)m_Tupel[i1][2];

                    List<Elements.Element_Measurement> m_Parent_measure = Parent.m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == measure_Type.Classifier_ID).ToList();
                    List<Requirements.Requirement_Non_Functional> m_req = m_Parent_measure.SelectMany(x => x.m_requirement).Distinct().ToList();


                    #region Req_QualityClass Selbst Dopplung
                    if (m_req.Count > 1)
                    {
                        int i2 = 1;
                        do
                        {

                            repository_Connector.Create_Dependency(m_req[0].Classifier_ID, m_req[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);


                            i2++;
                        } while (i2 < m_req.Count);
                    }
                    #endregion

                    #region Issue
                    string name = Parent.Name + " ist mit mehreren, gleichen " + measure_Type.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);

                    //Instanzen mit Issue verküpfen
                    List<string> m_Parent_desing_client_guid = m_Parent_measure.SelectMany(x => x.m_guid_Instanzen).ToList();
                    List<string> m_Parent_measurement = m_Parent_measure.Select(x => x.Measurement.Classifier_ID).ToList();

                    if (m_Parent_desing_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_desing_client_guid.Count);
                    }
                    if (m_Parent_measurement.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_measurement[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_measurement.Count);
                    }
                    #endregion

                }
                #endregion
                #region Kinder, Generalsisierung
                else
                {
                    //Elemente
                    SysElement Parent = (SysElement)m_Tupel[i1][0];
                    SysElement Child = (SysElement)m_Tupel[i1][1];
                    Requirement_Plugin.Repository_Elements.MeasurementType measure_Type = (Requirement_Plugin.Repository_Elements.MeasurementType)m_Tupel[i1][2];
                    //Wenn Anforderungen vorhanden mit Dopplung versehen
                    List<Elements.Element_Measurement> m_Parent_measure = Parent.m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == measure_Type.Classifier_ID).ToList();
                    List<Elements.Element_Measurement> m_Child_measure = Child.m_Element_Measurement.Where(x => x.Measurement.measurementType.Classifier_ID == measure_Type.Classifier_ID).ToList();
                    List<Requirement_Non_Functional> m_Parent_req_measure = m_Parent_measure.SelectMany(x => x.m_requirement).Distinct().ToList();
                    List<Requirement_Non_Functional> m_Child_req_measure = m_Child_measure.SelectMany(x => x.m_requirement).Distinct().ToList();

                    #region Req_Design Dopplung
                    if (m_Parent_req_measure.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (m_Child_req_measure.Count > 0)
                            {
                                int i3 = 0;
                                do
                                {
                                    if (m_Child_req_measure[i3] != null && m_Parent_req_measure[i2] != null)
                                    {
                                        repository_Connector.Create_Dependency(m_Child_req_measure[i3].Classifier_ID, m_Parent_req_measure[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                    }

                                    i3++;
                                } while (i3 < m_Child_req_measure.Count);
                            }


                            i2++;
                        } while (i2 < m_Parent_req_measure.Count);
                    }
                    #endregion

                    //Issue zwischen Elementen und Activity anlegen
                    //Issue erzeugen
                    #region Issue
                    string name = Parent.Name + " und " + Child.Name + " sind mit dem selben MeasurmentType " + measure_Type.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);

                    //Instanzen mit Issue verküpfen
                    List<string> m_Parent_desing_client_guid = m_Parent_measure.SelectMany(x => x.m_guid_Instanzen).ToList();
                    List<string> m_Child_design_client_guid = m_Child_measure.SelectMany(x => x.m_guid_Instanzen).ToList();

                    List<string> m_Parent_desing_measure_guid = m_Parent_measure.Select(x => x.Measurement.Classifier_ID).ToList();
                    List<string> m_Child_design_measure_guid = m_Child_measure.Select(x => x.Measurement.Classifier_ID).ToList();

                    if (m_Parent_desing_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_desing_client_guid.Count);
                    }
                    if (m_Child_design_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_design_client_guid.Count);
                    }

                    if (m_Parent_desing_measure_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_desing_measure_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_desing_measure_guid.Count);
                    }
                    if (m_Child_design_measure_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_design_measure_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_design_measure_guid.Count);
                    }

                    #endregion
                }
                #endregion

                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion

        #region QualityActivity
        public List<List<Repository_Element>> Check_Dopplung_QualityActivity(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel)
        {
            List<List<Repository_Element>> m_Dopplung = new List<List<Repository_Element>>();

            int i1 = 0;
            do
            {
                SysElement Parent = (SysElement)m_Tupel[i1][0];
                SysElement Child = (SysElement)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];

                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();

                List<Elements.Element_Measurement> m_Parent_func_process = m_Parent_func.SelectMany(x => x.m_element_measurement).ToList();
                List<Elements.Element_Measurement> m_Parent_user_process = m_Parent_user.SelectMany(x => x.m_element_measurement).ToList();
                List<Elements.Element_Measurement> m_Child_func_process = m_Child_func.SelectMany(x => x.m_element_measurement).ToList();
                List<Elements.Element_Measurement> m_Child_user_process = m_Child_user.SelectMany(x => x.m_element_measurement).ToList();

                List<Requirement_Plugin.Repository_Elements.Measurement> m_Parent_func_process_constraint = m_Parent_func_process.Select(x => x.Measurement).ToList();
                List<Requirement_Plugin.Repository_Elements.Measurement> m_Parent_user_process_constraint = m_Parent_user_process.Select(x => x.Measurement).ToList();
                List<Requirement_Plugin.Repository_Elements.Measurement> m_Child_func_process_constraint = m_Child_func_process.Select(x => x.Measurement).ToList();
                List<Requirement_Plugin.Repository_Elements.Measurement> m_Child_user_process_constraint = m_Child_user_process.Select(x => x.Measurement).ToList();

                #region Functional
                if (m_Parent_func_process_constraint.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_process_constraint.Count > 0)
                        {
                            if (m_Child_func_process_constraint.Select(x => x.Classifier_ID).ToList().Contains(m_Parent_func_process_constraint[i2].Classifier_ID) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }
                        if (m_Child_user_process_constraint.Count > 0)
                        {
                            if (m_Child_user_process_constraint.Select(x => x.Classifier_ID).ToList().Contains(m_Parent_func_process_constraint[i2].Classifier_ID) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }

                        i2++;
                    } while (i2 < m_Parent_func_process_constraint.Count);
                }
                #endregion
                #region User
                if (m_Parent_user_process_constraint.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_process_constraint.Count > 0)
                        {
                            if (m_Child_func_process_constraint.Select(x => x.Classifier_ID).ToList().Contains(m_Parent_user_process_constraint[i2].Classifier_ID) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_user_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }
                        if (m_Child_user_process_constraint.Count > 0)
                        {
                            if (m_Child_user_process_constraint.Select(x => x.Classifier_ID).ToList().Contains(m_Parent_user_process_constraint[i2].Classifier_ID) == true)
                            {
                                List<Repository_Element> m_help = new List<Repository_Element>();
                                m_help.Add(Parent);
                                m_help.Add(Child);
                                m_help.Add(activity);
                                m_help.Add(m_Parent_func_process_constraint[i2]);

                                m_Dopplung.Add(m_help);
                            }
                        }

                        i2++;
                    } while (i2 < m_Parent_user_process_constraint.Count);
                }
                #endregion

                i1++;
            } while (i1 < m_Tupel.Count);



            return (m_Dopplung);
            //  List < List < Repository_Element >>  m_Dopplung_:Functionalthis.Check_Dopplung_Functional(database, repository);

            /*  List<List<Repository_Element>> m_QualityActivity = new List<List<Repository_Element>>();

              List<Activity> m_activity = this.Get_Process_Activity();
              List<Activity> m_activity_measure = new List<Activity>();

              if (m_activity.Count > 0)
              {
                  int i1 = 0;
                  do
                  {

                      List<List<Element_Measurement>> m_ret = m_activity[i1].Check_Measurements_Dopplung(this);

                      if(m_ret.Count > 0)
                      {
                          m_activity_measure.Add(m_activity[i1]);
                      }

                      i1++;
                  } while (i1 < m_activity.Count);
              }

              #region Eigene Ebene
              if (m_activity_measure.Count > 0)
              {
                  int i1 = 0;
                  do
                  {
                      List<Repository_Element> m_help = new List<Repository_Element>();

                      m_help.Add(this);
                      m_help.Add(m_activity_measure[i1]);
                      m_help.Add(null);

                      m_QualityActivity.Add(m_help);

                      i1++;
                  } while (i1 < m_activity_measure.Count);
              }
              #endregion

              #region Kinderelemente
              List<NodeType> m_Child = this.Get_All_Children();
              if (m_Child != null)
              {
                  int i1 = 0;
                  do
                  {
                      List<Activity> m_activity_child = m_Child[i1].Get_Process_Activity();

                      if(m_activity_child.Count > 0)
                      {
                          int i2 = 0;
                          do
                          {
                              if (m_activity_measure.Contains(m_activity_child[i2]))
                              {
                                  List<Repository_Element> m_help = new List<Repository_Element>();

                                  m_help.Add(this);
                                  m_help.Add(m_activity_child[i2]);
                                  m_help.Add(m_Child[i1]);

                                  m_QualityActivity.Add(m_help);

                              }

                              i2++;
                          } while (i2 < m_activity_child.Count);
                      }



                      i1++;
                  } while (i1 < m_Child.Count);
              }

              #endregion
              #region Generalsiiserung
              List<NodeType> m_Specify = this.Get_All_Specifies();

              if (m_Specify != null)
              {
                  int i1 = 0;
                  do
                  {
                      List<Activity> m_activity_spec = m_Specify[i1].Get_Process_Activity();

                      if (m_activity_spec.Count > 0)
                      {
                          int i2 = 0;
                          do
                          {
                              if (m_activity_measure.Contains(m_activity_spec[i2]))
                              {
                                  List<Repository_Element> m_help = new List<Repository_Element>();

                                  m_help.Add(this);
                                  m_help.Add(m_activity_spec[i2]);
                                  m_help.Add(m_Specify[i1]);

                                  m_QualityActivity.Add(m_help);
                              }

                              i2++;
                          } while (i2 < m_activity_spec.Count);
                      }


                      i1++;
                  }while(i1 < m_Specify.Count);
              }
              #endregion

              #region Gneralsisierung hoch
              List<NodeType> m_Specified = this.Get_All_SpecifiedBy(database.m_NodeType);

              if (m_Specified != null)
              {
                  int i1 = 0;
                  do
                  {
                      List<Activity> m_activity_spec = m_Specified[i1].Get_Process_Activity();

                      if (m_activity_spec.Count > 0)
                      {
                          int i2 = 0;
                          do
                          {
                              if (m_activity_measure.Contains(m_activity_spec[i2]))
                              {
                                  List<Repository_Element> m_help = new List<Repository_Element>();

                                  m_help.Add(this);
                                  m_help.Add(m_activity_spec[i2]);
                                  m_help.Add(m_Specified[i1]);

                                  m_QualityActivity.Add(m_help);
                              }

                              i2++;
                          } while (i2 < m_activity_spec.Count);
                      }


                      i1++;
                  } while (i1 < m_Specified.Count);
              }
                  #endregion

              */
            // return (m_QualityActivity);
        }

        public void Create_Issue_Dopplung_QualityActivity(Database database, EA.Repository repository, List<List<Repository_Element>> m_Tupel, List<string> m_Package_GUID)
        {
            Repository_Connector repository_Connector = new Repository_Connector();
            bool create_issue = true;

            int i1 = 0;
            do
            {
                //Elemente
                NodeType Parent = (NodeType)m_Tupel[i1][0];
                NodeType Child = (NodeType)m_Tupel[i1][1];
                Activity activity = (Activity)m_Tupel[i1][2];
                Requirement_Plugin.Repository_Elements.Measurement opcon = (Requirement_Plugin.Repository_Elements.Measurement)m_Tupel[i1][3];
                //Wenn Anforderungen vorhanden mit Dopplung versehen
                List<Element_Functional> m_Parent_func = Parent.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Parent_user = Parent.m_Element_User.Where(x => x.Supplier == activity).ToList();
                List<Element_Functional> m_Child_func = Child.m_Element_Functional.Where(x => x.Supplier == activity).ToList();
                List<Element_User> m_Child_user = Child.m_Element_User.Where(x => x.Supplier == activity).ToList();

                List<Element_Measurement> m_Parent_func_Elemnte_Process = m_Parent_func.SelectMany(x => x.m_element_measurement).ToList().Where(x => x.Measurement == opcon).ToList();
                List<Element_Measurement> m_Parent_user_Elemnte_Process = m_Parent_user.SelectMany(x => x.m_element_measurement).ToList().Where(x => x.Measurement == opcon).ToList();
                List<Element_Measurement> m_Child_func_Elemnte_Process = m_Child_func.SelectMany(x => x.m_element_measurement).ToList().Where(x => x.Measurement == opcon).ToList();
                List<Element_Measurement> m_Child_user_Elemnte_Process = m_Child_user.SelectMany(x => x.m_element_measurement).ToList().Where(x => x.Measurement == opcon).ToList();

                List<Requirement_Non_Functional> m_Parent_func_Req_Process = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_requirement).Distinct().ToList();
                List<Requirement_Non_Functional> m_Parent_user_Req_Process = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_requirement).Distinct().ToList();
                List<Requirement_Non_Functional> m_Child_func_Req_Process = m_Child_func_Elemnte_Process.SelectMany(x => x.m_requirement).Distinct().ToList();
                List<Requirement_Non_Functional> m_Child_user_Req_Process = m_Child_user_Elemnte_Process.SelectMany(x => x.m_requirement).Distinct().ToList();

                List<string> m_Parent_func_client_guid = m_Parent_func.SelectMany(x => x.m_Target_Functional).ToList().Select(x => x.CLient_ID).ToList();
                List<string> m_Parent_user_client_guid = m_Parent_user.SelectMany(x => x.m_Target_User).ToList().Select(x => x.CLient_ID).ToList();
                List<string> m_Child_func_client_guid = m_Child_func.SelectMany(x => x.m_Target_Functional).ToList().Select(x => x.CLient_ID).ToList();
                List<string> m_Childt_user_client_guid = m_Child_user.SelectMany(x => x.m_Target_User).ToList().Select(x => x.CLient_ID).ToList();


                #region Req_Functional Dopplung
                if (m_Parent_func_Req_Process.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_Req_Process[i3] != null && m_Parent_func_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_Req_Process[i3].Classifier_ID, m_Parent_func_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_Req_Process.Count);
                        }
                        if (m_Child_user_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_Req_Process[i3] != null && m_Parent_func_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_Req_Process[i3].Classifier_ID, m_Parent_func_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_user_Req_Process.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_func_Req_Process.Count);
                }
                #endregion
                #region Req User Dopplung
                if (m_Parent_user_Req_Process.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (m_Child_func_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_func_Req_Process[i3] != null && m_Parent_user_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_func_Req_Process[i3].Classifier_ID, m_Parent_user_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }

                                i3++;
                            } while (i3 < m_Child_func_Req_Process.Count);
                        }
                        if (m_Child_user_Req_Process.Count > 0)
                        {
                            int i3 = 0;
                            do
                            {
                                if (m_Child_user_Req_Process[i3] != null && m_Parent_user_Req_Process[i2] != null)
                                {
                                    repository_Connector.Create_Dependency(m_Child_user_Req_Process[i3].Classifier_ID, m_Parent_user_Req_Process[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Afo_Dublette.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Afo_Dublette[0].direction);
                                }
                                i3++;
                            } while (i3 < m_Child_user_Req_Process.Count);
                        }

                        i2++;
                    } while (i2 < m_Parent_user_Req_Process.Count);
                }
                #endregion
                //Issue zwischen Elementen und Activity anlegen
                //Issue erzeugen
                if (create_issue == true)
                {
                    #region Issue
                    string name = Parent.Name + " und " + Child.Name + " sind mit derselben Activity " + activity.Name + " und Measurement " + opcon.Name + " verknüpft";
                    string notes = "Diese Dopplung muss aufgelöst werden.";
                    string type = database.metamodel.m_Issue[7].Stereotype;
                    Issue issue = new Issue(database, name, notes, m_Package_GUID[0], repository, true, type);
                    #endregion
                    //Instanzen mit Issue verküpfen
                    //List<string> m_Parent_func_client_guid = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();
                    //List<string> m_Parent_user_client_guid = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();

                    List<string> m_Parent_func_target_guid = m_Parent_func_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();


                    List<string> m_Parent_user_target_guid = m_Parent_user_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();

                    //List<string> m_Child_func_client_guid = m_Child_func_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();
                    //List<string> m_Childt_user_client_guid = m_Child_user_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();

                    List<string> m_Child_func_target_guid = m_Child_func_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();
                    List<string> m_Child_user_target_guid = m_Child_user_Elemnte_Process.SelectMany(x => x.m_guid_Instanzen).ToList();

                    //Client über Element functional und target m_guid_Instnazen finden
                    if (m_Parent_func_Elemnte_Process.Count > 0)
                    {
                        int i3 = 0;
                        do
                        {


                            i3++;
                        } while (i3 < m_Parent_func_Elemnte_Process.Count);
                    }

                    if (m_Parent_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_client_guid.Count);
                    }
                    if (m_Parent_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_client_guid.Count);
                    }
                    if (m_Parent_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_func_target_guid.Count);
                    }
                    if (m_Parent_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Parent_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Parent_user_target_guid.Count);
                    }

                    if (m_Child_func_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_client_guid.Count);
                    }
                    if (m_Childt_user_client_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Childt_user_client_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Childt_user_client_guid.Count);
                    }
                    if (m_Child_func_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_func_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_func_target_guid.Count);
                    }
                    if (m_Child_user_target_guid.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_Child_user_target_guid[i2], database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                            i2++;
                        } while (i2 < m_Child_user_target_guid.Count);
                    }
                    repository_Connector.Create_Dependency(issue.Classifier_ID, opcon.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);



                }


                i1++;
            } while (i1 < m_Tupel.Count);
        }
        #endregion
        #endregion
        public List<SysElement> Get_All_Children_Sys()
        {
            List<SysElement> m_ret = new List<SysElement>();

            if (this.m_Child.Count > 0)
            {
                m_ret = this.m_Child;

                int i1 = 0;
                do
                {
                    List<SysElement> m_help = new List<SysElement>();
                    m_help = this.m_Child[i1].Get_All_Children_Sys();

                    if (m_help != null)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < this.m_Child.Count);

                return (m_ret);

            }
            else
            {
                return (null);
            }

        }


        #endregion

        #region Create

        public void Create_Sys_Requirement_Functional(EA.Repository Repository, Database Data, string Package_GUID)
        {

        }

        #endregion

        #region Transform Logical

        public void Transform_Functional(Database database, EA.Repository repository)
        {
            if(this.m_Implements.Count > 0)
            {
                List<string> m_inst_guid = new List<string>();

                int i1 = 0;
                do
                {
                    m_inst_guid = this.m_m_Implements_guid[i1];

                    if(m_inst_guid.Count > 0 && this.m_Implements[i1].m_Element_Functional.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            Element_Functional element_Functional = new Element_Functional();
                            element_Functional.m_Target_Functional = this.m_Implements[i1].m_Element_Functional[i2].Check_Target_ByClient(m_inst_guid);
                            element_Functional.Client = this;
                            element_Functional.Supplier = this.m_Implements[i1].m_Element_Functional[i2].Supplier;
                            element_Functional.Capability = this.m_Implements[i1].m_Element_Functional[i2].Capability;

                            if(this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement.Count > 0)
                            {
                                int i3 = 0;
                                do
                                {
                                    Element_Measurement element_Measurement = new Element_Measurement(this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement[i3].Measurement, database);
                                    element_Measurement.m_guid_Instanzen = this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement[i3].m_guid_Instanzen;
                                    if (this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement[i3].m_requirement.Count > 0)
                                    {
                                        int i4 = 0;
                                        do
                                        {
                                            this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement[i3].m_requirement[i4].Get_SysElement(database, repository);
                                            if (this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement[i3].m_requirement[i4].sysElement.Classifier_ID == this.Classifier_ID)
                                            {
                                                element_Measurement.m_requirement.Add(this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement[i3].m_requirement[i4]);
                                            }

                                            i4++;
                                        } while (i4 < this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement[i3].m_requirement.Count);

                                      
                                    }

                                    element_Functional.m_element_measurement.Add(element_Measurement);

                                    i3++;
                                } while (i3 < this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement.Count);
                            }

                            //element_Functional.m_element_measurement = this.m_Implements[i1].m_Element_Functional[i2].m_element_measurement;



                            element_Functional.m_element_Processes = this.m_Implements[i1].m_Element_Functional[i2].m_element_Processes;
                           
                            //Überprüfung Measurement --> Node_ID noch vorhanden

                            element_Functional.m_Logical = this.m_Implements[i1].m_Element_Functional[i2].m_Logical;
                            List<Requirement_Functional> m_help = this.m_Implements[i1].m_Element_Functional[i2].m_Requirement_Functional;

                            //Überprüfen, ob Requirement mit SysElem verknüpft
                            if(m_help.Count > 0)
                            {
                                List<Requirement_Functional> m_req = new List<Requirement_Functional>();

                                int i3 = 0;
                                do
                                {
                                    if(m_help[i3] != null)
                                    {
                                        m_help[i3].Get_SysElement(database, repository);

                                        if (m_help[i3].sysElement.Classifier_ID == null)
                                        {
                                            //Issue erzeugen
                                            Issue issue = new Issue(database, "Keine Zuorndung Systemelement", "Forderung muss einen Systemelement zugeorndet werden.", database.m_package[1], repository, true, null);
                                        }
                                        else
                                        {
                                            if (m_help[i3].sysElement.Classifier_ID != this.Classifier_ID)
                                            {
                                                //element_Functional.m_Requirement_Functional[i3] = null;
                                            }
                                            else
                                            {
                                                m_req.Add(m_help[i3]);
                                            }
                                        }
                                    }
                                   

                                    i3++;
                                } while (i3 < m_help.Count);


                                element_Functional.m_Requirement_Functional = m_req;
                            }

                            this.m_Element_Functional.Add(element_Functional);

                            i2++;
                        } while (i2 < this.m_Implements[i1].m_Element_Functional.Count);
                    }

                    i1++;
                } while (i1 < this.m_Implements.Count);
                //Transfom m_Process
                if(this.m_Element_Functional.Count > 0)
                {
                    this.Transform_Implements();
                    int i2 = 0;
                    do
                    {
                        if(this.m_Element_Functional[i2].m_element_Processes.Count > 0)
                        {
                           
                            this.m_Element_Functional[i2].m_element_Processes = this.m_Element_Functional[i2].Transform_Node_Element_Proces(this.m_Implements_guid, this);
                        }

                        i2++;
                    } while (i2 < this.m_Element_Functional.Count);
                }
                //Transform m_Measurement
                if(this.m_Element_Functional.Count > 0)
                {
                    int i3 = 0;
                    do
                    {
                        if(this.m_Element_Functional[i3].m_element_measurement.Count > 0)
                        {
                            this.m_Element_Functional[i3].m_element_measurement = this.m_Element_Functional[i3].Transform_Node_Element_Measurement(this.m_Implements_guid, this, database);
                        }

                        i3++;
                    } while (i3 < this.m_Element_Functional.Count);

                }

            }
        }


        public void Transform_User(Database database, EA.Repository repository)
        {
            Repository_Connector rep_con = new Repository_Connector();

            if (this.m_Implements.Count > 0)
            {
                List<string> m_inst_guid = new List<string>();

                int i1 = 0;
                do
                {
                    m_inst_guid = this.m_m_Implements_guid[i1];

                    if (m_inst_guid.Count > 0 && this.m_Implements[i1].m_Element_User.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            Element_User element_Functional = new Element_User();
                            element_Functional.m_Target_User = this.m_Implements[i1].m_Element_User[i2].Check_Target_ByClient(m_inst_guid);
                            element_Functional.Client = this;
                            element_Functional.Supplier = this.m_Implements[i1].m_Element_User[i2].Supplier;
                            element_Functional.Capability = this.m_Implements[i1].m_Element_User[i2].Capability;
                            element_Functional.m_element_measurement = this.m_Implements[i1].m_Element_User[i2].m_element_measurement;
                            element_Functional.m_element_Processes = this.m_Implements[i1].m_Element_User[i2].m_element_Processes;

                            element_Functional.m_Client_ST = this.m_Implements[i1].m_Element_User[i2].m_Client_ST;
                            element_Functional.m_Requirement_Functional = this.m_Implements[i1].m_Element_User[i2].m_Requirement_Functional;
                            element_Functional.m_Target_Functional = this.m_Implements[i1].m_Element_User[i2].m_Target_Functional;



                            element_Functional.m_Logical = this.m_Implements[i1].m_Element_User[i2].m_Logical;
                            //element_Functional.m_Requirement_User = this.m_Implements[i1].m_Element_User[i2].m_Requirement_User;

                            List<Requirement_User> m_help = this.m_Implements[i1].m_Element_User[i2].m_Requirement_User;

                            //Überprüfen, ob Requirement mit SysElem verknüpft
                            if(m_help.Count > 0)
                            {
                                List<Requirement_User> m_req = new List<Requirement_User>();

                                int i3 = 0;
                                do
                                {
                                    m_help[i3].Get_SysElement(database, repository);

                                    if (m_help[i3].sysElement.Classifier_ID == null)
                                    {
                                        //Issue erzeugen
                                        Issue issue = new Issue(database, "Keine Zuorndung Systemelement", "Forderung muss einen Systemelement zugeorndet werden.", database.m_package[1], repository, true, null);
                                        rep_con.Create_Dependency(issue.Classifier_ID, m_help[i3].Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);
                                    }
                                    else
                                    {
                                        if (m_help[i3].sysElement.Classifier_ID != this.Classifier_ID)
                                        {
                                            //element_Functional.m_Requirement_Functional[i3] = null;
                                        }
                                        else
                                        {
                                            m_req.Add(m_help[i3]);
                                        }
                                    }

                                    i3++;
                                } while (i3 < m_help.Count);

                                element_Functional.m_Requirement_User = m_req;
                            }

                            this.m_Element_User.Add(element_Functional);

                            i2++;
                        } while (i2 < this.m_Implements[i1].m_Element_User.Count);

                       
                    }

                    i1++;
                } while (i1 < this.m_Implements.Count);

                //Transfom m_Process
                if (this.m_Element_User.Count > 0)
                {
                    this.Transform_Implements();
                    int i2 = 0;
                    do
                    {
                        if (this.m_Element_User[i2].m_element_Processes.Count > 0)
                        {
                           
                            this.m_Element_User[i2].m_element_Processes = this.m_Element_User[i2].Transform_Node_Element_Proces(this.m_Implements_guid, this);
                        }

                        i2++;
                    } while (i2 < this.m_Element_User.Count);
                }
                //Transform m_Measurement
                if (this.m_Element_User.Count > 0)
                {
                    int i3 = 0;
                    do
                    {
                        if (this.m_Element_User[i3].m_element_measurement.Count > 0)
                        {
                            this.m_Element_User[i3].m_element_measurement = this.m_Element_User[i3].Transform_Node_User_Element_Measurement(this.m_Implements_guid, this, database);
                        }

                        i3++;
                    } while (i3 < this.m_Element_User.Count);

                }
            }

        }

        public void Transform_Interface(Database database, EA.Repository repository)
        {
            if (this.m_Implements.Count > 0)
            {
                int i1 = 0;
                do
                {
                    //Unidirektoinale SChnittstelle
                    #region Unidirektionale Schnittstelle
                    if (this.m_Implements[i1].m_Element_Interface.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Element_Interface> m_chek_uni = this.m_Element_Interface.Where(x => x.Client == this).Where(x => x.Supplier == this.m_Implements[i1].m_Element_Interface[i2].Supplier).ToList();
                            //Prüfung, ob bereits bei SysElem vorhanden
                            if (m_chek_uni.Count == 0)
                            {
                                //Nicht vorhanden
                                Element_Interface elem_inter_new = new Element_Interface(this.Classifier_ID, this.m_Implements[i1].m_Element_Interface[i2].Supplier.Classifier_ID);
                                elem_inter_new.Client = this;
                                elem_inter_new.Supplier = this.m_Implements[i1].m_Element_Interface[i2].Supplier;
                                elem_inter_new.m_Logical_Supplier = this.m_Implements[i1].m_Element_Interface[i2].m_Logical_Supplier;

                                List<Requirement_Interface> m_help_1 = this.m_Implements[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive;
                                List<Requirement_Interface> m_help_2 = this.m_Implements[i1].m_Element_Interface[i2].m_Requirement_Interface_Send;
                                //elem_inter_new.m_Requirement_Interface_Receive = this.m_Implements[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive;
                                //elem_inter_new.m_Requirement_Interface_Send = this.m_Implements[i1].m_Element_Interface[i2].m_Requirement_Interface_Send;


                                elem_inter_new.m_Target = this.m_Implements[i1].m_Element_Interface[i2].m_Target;

                                //Überprüfen, ob Requirement mit SysElem verknüpft
                                if (m_help_1.Count > 0)
                                {
                                    List<Requirement_Interface> m_req_1 = new List<Requirement_Interface>();

                                    int i3 = 0;
                                    do
                                    {
                                        m_help_1[i3].Get_SysElement(database, repository);

                                        if (m_help_1[i3].sysElement.Classifier_ID == null)
                                        {
                                            //Issue erzeugen
                                            Issue issue = new Issue(database, "Keine Zuorndung Systemelement", "Forderung muss einen Systemelement zugeorndet werden.", database.m_package[1], repository, true, null);
                                        }
                                        else
                                        {
                                            if (m_help_1[i3].sysElement.Classifier_ID != this.Classifier_ID)
                                            {
                                                //element_Functional.m_Requirement_Functional[i3] = null;
                                            }
                                            else
                                            {
                                                m_req_1.Add(m_help_1[i3]);
                                            }
                                        }

                                        i3++;
                                    } while (i3 < m_help_1.Count);

                                    elem_inter_new.m_Requirement_Interface_Receive = m_req_1;
                                }

                                if (m_help_2.Count > 0)
                                {
                                    List<Requirement_Interface> m_req_2 = new List<Requirement_Interface>();

                                    int i3 = 0;
                                    do
                                    {
                                        m_help_2[i3].Get_SysElement(database, repository);

                                        if (m_help_2[i3].sysElement.Classifier_ID == null)
                                        {
                                            //Issue erzeugen
                                            Issue issue = new Issue(database, "Keine Zuorndung Systemelement", "Forderung muss einen Systemelement zugeorndet werden.", database.m_package[1], repository, true, null);
                                        }
                                        else
                                        {
                                            if (m_help_2[i3].sysElement.Classifier_ID != this.Classifier_ID)
                                            {
                                                //element_Functional.m_Requirement_Functional[i3] = null;
                                            }
                                            else
                                            {
                                                m_req_2.Add(m_help_2[i3]);
                                            }
                                        }

                                        i3++;
                                    } while (i3 < m_help_2.Count);

                                    elem_inter_new.m_Requirement_Interface_Send = m_req_2;
                                }

                                this.m_Element_Interface.Add(elem_inter_new);
                            }
                            else
                            {
                                //Target aufdatieren
                                if(this.m_Implements[i1].m_Element_Interface[i2].m_Target.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        Target help = m_chek_uni[0].Check_Target(this.m_Implements[i1].m_Element_Interface[i2].m_Target[i3].CLient_ID, this.m_Implements[i1].m_Element_Interface[i2].m_Target[i3].Supplier_ID);

                                        if(help == null)
                                        {
                                            m_chek_uni[0].m_Target.Add(this.m_Implements[i1].m_Element_Interface[i2].m_Target[i3]);
                                        }

                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Element_Interface[i2].m_Target.Count);
                                }

                                
                            }

                                i2++;
                        } while (i2 < this.m_Implements[i1].m_Element_Interface.Count);
                        

                    }
                    #endregion
                    //Bidirektional Schnittstelle
                    #region Bidirektionale Schnittstelle
                    if (this.m_Implements[i1].m_Element_Interface_Bidirectional.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Element_Interface_Bidirectional> m_chek_bi = this.m_Element_Interface_Bidirectional.Where(x => x.Client == this).Where(x => x.Supplier == this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].Supplier).ToList();

                            if(m_chek_bi.Count == 0)
                            {
                                //Nicht vorhanden
                                Element_Interface_Bidirectional element_Interface_Bi = new Element_Interface_Bidirectional(this.Classifier_ID, this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].Supplier.Classifier_ID, repository, database);
                                element_Interface_Bi.Bidirectional = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].Bidirectional;
                                element_Interface_Bi.Client = this;
                                element_Interface_Bi.m_InformationElement = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_InformationElement;
                                element_Interface_Bi.m_Logical_Client = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Logical_Client;
                                element_Interface_Bi.m_Logical_Supplier = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Logical_Supplier;

                                List<Requirement_Interface> m_help_1 = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Requirement_Interface_Receive;
                                List<Requirement_Interface> m_help_2 = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Requirement_Interface_Send;
                                // element_Interface_Bi.m_Requirement_Interface_Receive = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Requirement_Interface_Receive;
                                // element_Interface_Bi.m_Requirement_Interface_Send = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Requirement_Interface_Send;


                                element_Interface_Bi.m_Target = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target;
                                element_Interface_Bi.m_Target_Client = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Client;
                                element_Interface_Bi.m_Target_Supplier = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Supplier;
                                element_Interface_Bi.Supplier = this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].Supplier;

                                //Überprüfen, ob Requirement mit SysElem verknüpft
                                if (m_help_1.Count > 0)
                                {
                                    List<Requirement_Interface> m_req_1 = new List<Requirement_Interface>();

                                    int i3 = 0;
                                    do
                                    {
                                        m_help_1[i3].Get_SysElement(database, repository);

                                        if (m_help_1[i3].sysElement.Classifier_ID == null)
                                        {
                                            //Issue erzeugen
                                            Issue issue = new Issue(database, "Keine Zuorndung Systemelement", "Forderung muss einen Systemelement zugeorndet werden.", database.m_package[1], repository, true, null);
                                        }
                                        else
                                        {
                                            if (m_help_1[i3].sysElement.Classifier_ID != this.Classifier_ID)
                                            {
                                                //element_Functional.m_Requirement_Functional[i3] = null;
                                            }
                                            else
                                            {
                                                m_req_1.Add(m_help_1[i3]);
                                            }
                                        }

                                        

                                        i3++;
                                    } while (i3 < m_help_1.Count);

                                    element_Interface_Bi.m_Requirement_Interface_Receive = m_req_1;

                                }
                                if (m_help_2.Count > 0)
                                {
                                    List<Requirement_Interface> m_req_2 = new List<Requirement_Interface>();

                                    int i3 = 0;
                                    do
                                    {
                                        m_help_2[i3].Get_SysElement(database, repository);

                                        if (m_help_2[i3].sysElement.Classifier_ID == null)
                                        {
                                            //Issue erzeugen
                                            Issue issue = new Issue(database, "Keine Zuorndung Systemelement", "Forderung muss einen Systemelement zugeorndet werden.", database.m_package[1], repository, true, null);
                                        }
                                        else
                                        {
                                            if (m_help_2[i3].sysElement.Classifier_ID != this.Classifier_ID)
                                            {
                                                //element_Functional.m_Requirement_Functional[i3] = null;
                                            }
                                            else
                                            {
                                                m_req_2.Add(m_help_2[i3]);
                                            }
                                        }

                                      

                                        i3++;
                                    } while (i3 < m_help_2.Count);

                                    element_Interface_Bi.m_Requirement_Interface_Send = m_req_2;
                                }

                                this.m_Element_Interface_Bidirectional.Add(element_Interface_Bi);
                            }
                            else
                            {
                                //Vorhanden
                                //Target aufdatieren
                                if (this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        Target help = m_chek_bi[0].Check_Target(this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target[i3].CLient_ID, this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target[i3].Supplier_ID);

                                        if (help == null)
                                        {
                                            m_chek_bi[0].m_Target.Add(this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target[i3]);
                                        }

                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target.Count);
                                }
                                //Target Client aufdatieren
                                if (this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Client.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        List<Target> m_help = m_chek_bi[0].m_Target_Client.Where(x => x.CLient_ID == this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Client[i3].CLient_ID).Where(x => x.Supplier_ID == this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Client[i3].Supplier_ID).ToList();

                                        if (m_help.Count == 0)
                                        {
                                            m_chek_bi[0].m_Target_Client.Add(this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Client[i3]);
                                        }

                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Client.Count);
                                }
                                //Target Supplier aufdatieren
                                if (this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Supplier.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        List<Target> m_help = m_chek_bi[0].m_Target_Client.Where(x => x.CLient_ID == this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Supplier[i3].CLient_ID).Where(x => x.Supplier_ID == this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Supplier[i3].Supplier_ID).ToList();

                                        if (m_help.Count == 0)
                                        {
                                            m_chek_bi[0].m_Target_Client.Add(this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Supplier[i3]);
                                        }

                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Element_Interface_Bidirectional[i2].m_Target_Supplier.Count);
                                }

                            }

                            i2++;
                        } while (i2 < this.m_Implements[i1].m_Element_Interface_Bidirectional.Count);

                    }
                        #endregion

                        i1++;
                } while (i1 < this.m_Implements.Count);

            }
        }

        public void Transform_Quality_Class(Database database, EA.Repository repository)
        {
            if(this.m_Implements.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.m_Implements[i1].m_Element_Measurement.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Element_Measurement> m_help = this.m_Element_Measurement.Where(x => x.Measurement.Classifier_ID == this.m_Implements[i1].m_Element_Measurement[i2].Measurement.Classifier_ID).ToList();

                            if(m_help.Count == 0)
                            {
                                Element_Measurement element_Measurement = new Element_Measurement(this.m_Implements[i1].m_Element_Measurement[i2].Measurement, database);
                                element_Measurement.m_guid_Instanzen = this.m_Implements[i1].m_Element_Measurement[i2].m_guid_Instanzen;
                                element_Measurement.m_requirement = this.m_Implements[i1].m_Element_Measurement[i2].m_requirement;

                                this.m_Element_Measurement.Add(element_Measurement);
                            }
                            else
                            {
                                if(this.m_Implements[i1].m_Element_Measurement[i2].m_guid_Instanzen.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        if(m_help[0].m_guid_Instanzen.Contains(this.m_Implements[i1].m_Element_Measurement[i2].m_guid_Instanzen[i3]) == false)
                                        {
                                            m_help[0].m_guid_Instanzen.Add(this.m_Implements[i1].m_Element_Measurement[i2].m_guid_Instanzen[i3]);
                                        }

                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Element_Measurement[i2].m_guid_Instanzen.Count);
                                }
                            }

                                i2++;
                        } while (i2 < this.m_Implements[i1].m_Element_Measurement.Count);

                        
                    }

                    i1++;
                } while (i1 < this.m_Implements.Count);
            }
        }

        public void Transform_Environment(Database database, EA.Repository repository)
        {
            if (this.m_Implements.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.m_Implements[i1].m_Enviromental.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Element_Environmental> m_help = this.m_Enviromental.Where(x => x.OpConstraint.Classifier_ID == this.m_Implements[i1].m_Enviromental[i2].OpConstraint.Classifier_ID).ToList();

                            if (m_help.Count == 0)
                            {
                                Element_Environmental element_Design = new Element_Environmental(this.m_Implements[i1].m_Enviromental[i2].m_GUID, this.m_Implements[i1].m_Enviromental[i2].OpConstraint, this);
                                element_Design.m_Logical = this.m_Implements[i1].m_Enviromental[i2].m_Logical;
                                element_Design.requirement = this.m_Implements[i1].m_Enviromental[i2].requirement;
                                element_Design.capability = this.m_Implements[i1].m_Enviromental[i2].capability;

                               // this.m_Enviromental.Add(element_Design);
                            }
                            else
                            {
                                if(this.m_Implements[i1].m_Enviromental[i2].m_GUID.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        if(m_help[0].m_GUID.Contains(this.m_Implements[i1].m_Enviromental[i2].m_GUID[i3]) == false)
                                        {
                                            m_help[0].m_GUID.Add(this.m_Implements[i1].m_Enviromental[i2].m_GUID[i3]);
                                        }
                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Enviromental[i2].m_GUID.Count);
                                }

                                if(this.m_Implements[i1].m_Enviromental[i2].m_Logical.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        if (m_help[0].m_Logical.Select(x => x.Classifier_ID).Contains(this.m_Implements[i1].m_Enviromental[i2].m_Logical[i3].Classifier_ID) == false)
                                        {
                                            m_help[0].m_Logical.Add(this.m_Implements[i1].m_Enviromental[i2].m_Logical[i3]);
                                        }
                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Enviromental[i2].m_Logical.Count);
                                }
                            }

                            i2++;
                        } while (i2 < this.m_Implements[i1].m_Enviromental.Count);
                    }


                    i1++;
                } while (i1 < this.m_Implements.Count);
            }
        }

        public void Transform_Design(Database database, EA.Repository repository)
        {
            if (this.m_Implements.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Implements[i1].m_Design.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<Element_Design> m_help = this.m_Design.Where(x => x.OpConstraint.Classifier_ID == this.m_Implements[i1].m_Design[i2].OpConstraint.Classifier_ID).ToList();

                            if (m_help.Count == 0)
                            {
                                Element_Design element_Design = new Element_Design(this.m_Implements[i1].m_Design[i2].m_GUID, this.m_Implements[i1].m_Design[i2].OpConstraint, this);
                                element_Design.m_Logical = this.m_Implements[i1].m_Design[i2].m_Logical;
                                element_Design.requirement = this.m_Implements[i1].m_Design[i2].requirement;
                                element_Design.capability = this.m_Implements[i1].m_Design[i2].capability;

                                //this.m_Design.Add(element_Design);
                            }
                            else
                            {
                                if (this.m_Implements[i1].m_Design[i2].m_GUID.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        if (m_help[0].m_GUID.Contains(this.m_Implements[i1].m_Design[i2].m_GUID[i3]) == false)
                                        {
                                            m_help[0].m_GUID.Add(this.m_Implements[i1].m_Design[i2].m_GUID[i3]);
                                        }
                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Design[i2].m_GUID.Count);
                                }

                                if (this.m_Implements[i1].m_Design[i2].m_Logical.Count > 0)
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        if (m_help[0].m_Logical.Select(x => x.Classifier_ID).Contains(this.m_Implements[i1].m_Design[i2].m_Logical[i3].Classifier_ID) == false)
                                        {
                                            m_help[0].m_Logical.Add(this.m_Implements[i1].m_Design[i2].m_Logical[i3]);
                                        }
                                        i3++;
                                    } while (i3 < this.m_Implements[i1].m_Design[i2].m_Logical.Count);
                                }
                            }
                            i2++;
                        } while (i2 < this.m_Implements[i1].m_Design.Count);
                    }

                    i1++;
                } while (i1 < this.m_Implements.Count);
            }
        }
        #endregion

        #region TV
        public void Update_SYSKomponententyp(Database database, EA.Repository Repository)
        {
            List<DB_Insert> m_Insert = new List<DB_Insert>();
            
            m_Insert.Add(new DB_Insert("SYS_KOMPONENTENTYP", OleDbType.VarChar, OdbcType.VarChar, this.SYS_KOMPONENTENTYP, -1));

            this.Update_TV(m_Insert, database, Repository);
        }
        #endregion
    }
}

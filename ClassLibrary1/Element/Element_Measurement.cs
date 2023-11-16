using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requirement_Plugin.Repository_Elements;

using System.Globalization;
using Requirement_Plugin;

namespace Elements
{
    public class Element_Measurement
    {
        //List<MeasurementType> Measurement_Type;
        public Measurement Measurement;

        public List<string> m_guid_Instanzen = new List<string>();

        public List<Requirements.Requirement_Non_Functional> m_requirement = new List<Requirements.Requirement_Non_Functional>();

        //public Requirements.Requirement_Non_Functional requirement = new Requirements.Requirement_Non_Functional(null, null);

        public Element_Measurement(Measurement measurement, Requirement_Plugin.Database database)
        {
            this.Measurement = measurement;
            this.m_requirement = new List<Requirements.Requirement_Non_Functional>();
        }

            #region Check_Requirement
            public void Check_For_Requirement(int type, EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.Activity activity, Repsoitory_Elements.NodeType nodeType) //type 0: Class, type 1:Activity, 
        {
            if(type == 0)
            {
                this.Check_Requirement_Class(repository, database);
            }
            else
            {
                this.Check_Requirement_Activity(repository, database, nodeType, activity);
            }
        }

        private void Check_Requirement_Class(EA.Repository repository, Requirement_Plugin.Database database)
        {
            List<string> m_Type_req_func = database.metamodel.m_Requirement_Quality_Class.Select(x => x.Type).ToList();
            List<string> m_Stereotype_req_func = database.metamodel.m_Requirement_Quality_Class.Select(x => x.Stereotype).ToList();
            List<string> m_Type_DerivedElem = database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
            List<string> m_Stereotype_DerivedElem = database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

            //AFO mit Measurment verknüpft
            List<string> Measurment_Requ = new List<string>();
            Requirement_Plugin.Interfaces.Interface_Connectors_Requirement interface_Connectors = new Requirement_Plugin.Interfaces.Interface_Connectors_Requirement();
            List<string> help_guid = new List<string>();
            help_guid.Add(this.Measurement.Classifier_ID);
            Measurment_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, help_guid, m_Type_req_func, m_Stereotype_req_func, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);
            //AFO nur mit Elements_Definiton bzw. Element_Usage verknüpft
            List<string> Element_Requ = new List<string>();
            List<string> help_guid2 = new List<string>();
            help_guid2.AddRange(this.m_guid_Instanzen);
            Element_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, help_guid2, m_Type_req_func, m_Stereotype_req_func, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);

            if(Measurment_Requ != null && Element_Requ != null)
            {
                //Überschneidung finden
                List<string> m_doppel = Measurment_Requ.Intersect(Element_Requ).ToList();

                if(m_doppel.Count > 0)
                {
                    //Überprüfung ob Anforderung wirklich richtig verknüpft ist
                    int i1 = 0;
                    do
                    {
                        Requirements.Requirement_Non_Functional requirement_Non_Functional = new Requirements.Requirement_Non_Functional(m_doppel[i1], database.metamodel);
                        requirement_Non_Functional.ID = requirement_Non_Functional.Get_Object_ID(database);

                        List<string> m_help3 = requirement_Non_Functional.Get_Supplier_Connector(database, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);

                        if (m_help3 != null)
                        {
                            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
                            List<string> m_stereo = repository_Elements.Get_Stereotypes(m_help3, database);

                            List<string> m1 = m_stereo.Except(database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList()).ToList();
                            List<string> m2 = m1.Except(database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList()).ToList();
                            List<string> m3 = m2.Except(database.metamodel.m_Measurement_Usage.Select(x => x.Stereotype).ToList()).ToList();
                            List<string> m4 = m3.Except(database.metamodel.m_Measurement_Type.Select(x => x.Stereotype).ToList()).ToList();

                            if (m4.Count > 0)
                            {
                                //ungültige Elemente
                            }
                            else
                            {
                                requirement_Non_Functional.Add_Requirement_NonFunctional("", "", "", "", "", "", true, "");
                                requirement_Non_Functional.Get_Tagged_Values_From_Requirement(requirement_Non_Functional.Classifier_ID, repository, database);
                                this.m_requirement.Add(requirement_Non_Functional);
                            }

                        }
                        

                        i1++;
                    } while (i1 < m_doppel.Count);
                }
            }

        }

        private void Check_Requirement_Activity(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity)
        {
            List<string> m_Type_req_func = database.metamodel.m_Requirement_Quality_Activity.Select(x => x.Type).ToList();
            List<string> m_Stereotype_req_func = database.metamodel.m_Requirement_Quality_Activity.Select(x => x.Stereotype).ToList();
            List<string> m_Type_DerivedElem = database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
            List<string> m_Stereotype_DerivedElem = database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

            //Measurment verknüpft
            List<string> Measurment_Requ = new List<string>();
            Requirement_Plugin.Interfaces.Interface_Connectors_Requirement interface_Connectors = new Requirement_Plugin.Interfaces.Interface_Connectors_Requirement();
            List<string> help_guid = new List<string>();
            help_guid.Add(this.Measurement.Classifier_ID);
            Measurment_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, help_guid, m_Type_req_func, m_Stereotype_req_func, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);
            //Activity verknüpft
            List<string> Activity_Requ = new List<string>();
            List<string> help_guid2 = new List<string>();

            //Alle Instnazen der Activity finden
            List<string> m_guid_instanzen2 = activity.Get_Instanzen_Repository(database);

            help_guid2.AddRange(this.m_guid_Instanzen);

            if(m_guid_instanzen2 != null)
            {
                help_guid2.AddRange(m_guid_instanzen2);
            }
            Activity_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, help_guid2, m_Type_req_func, m_Stereotype_req_func, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);
            //Class verknüpft
            List<string> Element_Requ = new List<string>();
            List<string> help_guid3 = new List<string>();
            help_guid3.AddRange(nodeType.m_Instantiate);
            Element_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, help_guid3, m_Type_req_func, m_Stereotype_req_func, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);

            if (Measurment_Requ != null && Element_Requ != null && Activity_Requ != null)
            {
                List<string> m_doppel = Measurment_Requ.Intersect(Element_Requ).ToList();
                List<string> m_doppel2 = m_doppel.Intersect(Activity_Requ).ToList();

                if (m_doppel2.Count > 0)
                {
                    //Überprüfung ob Anforderung wirklich richtig verknüpft ist
                    int i1 = 0;
                    do
                    {
                        Requirements.Requirement_Non_Functional requirement_Non_Functional = new Requirements.Requirement_Non_Functional(m_doppel2[i1], database.metamodel);
                        requirement_Non_Functional.ID = requirement_Non_Functional.Get_Object_ID(database);

                        List<string> m_help3 = requirement_Non_Functional.Get_Supplier_Connector(database, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);

                        if (m_help3 != null)
                        {
                            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
                            List<string> m_stereo = repository_Elements.Get_Stereotypes(m_help3, database);

                            List<string> m1 = m_stereo.Except(database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList()).ToList();
                            List<string> m2 = m1.Except(database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList()).ToList();
                            List<string> m3 = m2.Except(database.metamodel.m_Measurement_Usage.Select(x => x.Stereotype).ToList()).ToList();
                            List<string> m4 = m3.Except(database.metamodel.m_Measurement_Type.Select(x => x.Stereotype).ToList()).ToList();
                            List<string> m5 = m4.Except(database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList()).ToList();
                            List<string> m6 = m5.Except(database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList()).ToList();

                            if (m6.Count > 0)
                            {
                                //ungültige Elemente
                             //   this.requirement = null;
                            }
                            else
                            {
                                requirement_Non_Functional.Add_Requirement_NonFunctional("", "", "", "", "", "", true, "");
                                requirement_Non_Functional.Get_Tagged_Values_From_Requirement(requirement_Non_Functional.Classifier_ID, repository, database);
                                this.m_requirement.Add(requirement_Non_Functional);
                            }

                        }
                        else
                        {
                           // this.requirement = null;
                        }

                        i1++;
                    } while (i1 < m_doppel2.Count);


                   

                }
                else
                {
                    //this.requirement = null;
                }


            }
            else
            {
                //this.requirement = null;
            }
        }

        #endregion

        #region Dubletten
        public void Create_Connectoren_Generalisierung(EA.Repository repository, Requirement_Plugin.Database database, int type, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity)
        {
            if (type == 0)
            {
                this.Create_Connectoren_Generalisierung_Class(repository, database, nodeType);
            }
            else
            {
                this.Create_Connectoren_Generalisierung_Activity(repository, database, nodeType, activity);
            }
        }

        public void Create_Connectoren_Dubletten(EA.Repository repository, Requirement_Plugin.Database database, int type, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity) //type == 0 class, type == 1 activity
        {
            if(type == 0)
            {
                this.Create_Connectoren_Dubletten_Class(repository, database, nodeType);
            }
            else
            {
                this.Create_Connectoren_Dublette_Activity(repository, database, nodeType, activity);
            }
        }

        private void Create_Connectoren_Dubletten_Class(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType)
        {
             Repsoitory_Elements.Repository_Connector repository_Connector = new Repsoitory_Elements.Repository_Connector();

            List<Elements.Element_Measurement> m_measurement = new List<Element_Measurement>();
            m_measurement = nodeType.m_Element_Measurement.Where(x =>  x.Measurement.measurementType == this.Measurement.measurementType).ToList();

            if(m_measurement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_measurement[i1] != this)
                    {
                        if(this.m_requirement.Count > 0 != null && m_measurement[i1].m_requirement.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                int i3 = 0;
                                do
                                {
                                    repository_Connector.Create_Dependency(this.m_requirement[i3].Classifier_ID, m_measurement[i1].m_requirement[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);


                                    i3++;
                                } while (i3 < this.m_requirement.Count);

                                i2++;
                            } while (i2 < m_measurement[i1].m_requirement.Count);

                          
                        }

                    }

                    i1++;
                } while (i1 < m_measurement.Count);
            }
        }

        private void Create_Connectoren_Dublette_Activity(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity)
        {
            Repsoitory_Elements.Repository_Connector repository_Connector = new Repsoitory_Elements.Repository_Connector();

            List<Elements.Element_Measurement> m_measurement = new List<Element_Measurement>();

            List<Element_Functional> m_func = activity.m_Element_Functional.Where(x => x.Client == nodeType).ToList();
            List<Element_User> m_user = activity.m_Element_User.Where(x => x.Client == nodeType).ToList();

            if(m_func.Count > 0)
            {
                m_measurement = m_func[0].m_element_measurement.Where(x => x.Measurement.measurementType == this.Measurement.measurementType).ToList();

                if (m_measurement.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_measurement[i1] != this)
                        {
                            if (this.m_requirement.Count > 0 != null && m_measurement[i1].m_requirement.Count > 0)
                            {
                                // repository_Connector.Create_Dependency(this.requirement.Classifier_ID, m_measurement[i1].requirement.Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox);
                                int i2 = 0;
                                do
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        repository_Connector.Create_Dependency(this.m_requirement[i3].Classifier_ID, m_measurement[i1].m_requirement[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);


                                        i3++;
                                    } while (i3 < this.m_requirement.Count);

                                    i2++;
                                } while (i2 < m_measurement[i1].m_requirement.Count);
                            }

                        }

                        i1++;
                    } while (i1 < m_measurement.Count);
                }
            }

          if(m_user.Count > 0)
            {
                m_measurement = m_user[0].m_element_measurement.Where(x => x.Measurement.measurementType == this.Measurement.measurementType).ToList();

                if (m_measurement.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_measurement[i1] != this)
                        {
                            if (this.m_requirement.Count > 0 != null && m_measurement[i1].m_requirement.Count > 0)
                            {
                                  int i2 = 0;
                                do
                                {
                                    int i3 = 0;
                                    do
                                    {
                                        repository_Connector.Create_Dependency(this.m_requirement[i3].Classifier_ID, m_measurement[i1].m_requirement[i2].Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);


                                        i3++;
                                    } while (i3 < this.m_requirement.Count);

                                    i2++;
                                } while (i2 < m_measurement[i1].m_requirement.Count);
                            }

                        }

                        i1++;
                    } while (i1 < m_measurement.Count);
                }
            }


           

        }

        private void Create_Connectoren_Generalisierung_Class(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType)
        {
            //Hoch
            this.Gnerealisierung_Class_hoch_rekrusiv(repository, database, nodeType);

            //runter
            this.Gnerealisierung_Class_runter_rekrusiv(repository, database, nodeType);
        }

        private void Gnerealisierung_Class_hoch_rekrusiv(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType)
        {
            List<Repsoitory_Elements.NodeType> m_nodetype = database.m_NodeType.Where(x => x.m_Specialize.Contains(nodeType)).ToList();

            if(m_nodetype.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.Create_Connectoren_Dubletten_Class(repository, database, m_nodetype[i1]);

                    this.Gnerealisierung_Class_hoch_rekrusiv(repository, database, m_nodetype[i1]);

                    i1++;
                } while (i1 < m_nodetype.Count);
            }

        }

        private void Gnerealisierung_Class_runter_rekrusiv(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType)
        {
            if (nodeType.m_Specialize.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.Create_Connectoren_Dubletten_Class(repository, database, nodeType.m_Specialize[i1]);

                    this.Gnerealisierung_Class_runter_rekrusiv(repository, database, nodeType.m_Specialize[i1]);

                    i1++;
                } while (i1 < nodeType.m_Specialize.Count);
            }

        }

        private void Create_Connectoren_Generalisierung_Activity(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity)
        {
            //Hoch
            this.Gnerealisierung_Activity_hoch_rekrusiv(repository, database, nodeType, activity);

            //runter
            this.Gnerealisierung_Activity_runter_rekrusiv(repository, database, nodeType, activity);
        }

        private void Gnerealisierung_Activity_hoch_rekrusiv(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity)
        {
            List<Repsoitory_Elements.NodeType> m_nodetype = database.m_NodeType.Where(x => x.m_Specialize.Contains(nodeType)).ToList();

            if (m_nodetype.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.Create_Connectoren_Dublette_Activity(repository, database, m_nodetype[i1], activity);

                    this.Gnerealisierung_Activity_hoch_rekrusiv(repository, database, m_nodetype[i1], activity);

                    i1++;
                } while (i1 < m_nodetype.Count);
            }

        }

        private void Gnerealisierung_Activity_runter_rekrusiv(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity)
        {
            if (nodeType.m_Specialize.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.Create_Connectoren_Dublette_Activity(repository, database, nodeType.m_Specialize[i1], activity);

                    this.Gnerealisierung_Activity_runter_rekrusiv(repository, database, nodeType.m_Specialize[i1], activity);

                    i1++;
                } while (i1 < nodeType.m_Specialize.Count);
            }

        }
        #endregion

        #region Konnektoren

        public void Update_Connectoren_QualityClass(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType)
        {
            //Connectoren ziehen
            //Konnector auf NodeType
            this.Create_Connectoren_Instanzen(database, repository);
            //Konnector auf Measurement
            this.Create_Connectoren_Measurement(database, repository);
            //Generalisierung
            this.Create_Connectoren_Generalisierung_Class(repository, database, nodeType);
            //Dubletten
            this.Create_Connectoren_Dubletten_Class(repository, database, nodeType);
        }

        public void Update_Connectoren_QualityActivity(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity)
        {
            //Konnector auf Action/Activity
            this.Create_Connectoren_Instanzen(database, repository);
            //Konnektoren auf NodeType
            List<Element_Functional> m_func = activity.m_Element_Functional.Where(x => x.Client == nodeType).ToList();
            this.Create_Connectoren_element(database, repository, m_func[0]);
            //Konnector auf Measurement
            this.Create_Connectoren_Measurement(database, repository);
            //Generalisierung
            this.Create_Connectoren_Generalisierung_Activity(repository, database, nodeType, activity);
            //Dubletten
            this.Create_Connectoren_Dublette_Activity(repository, database, nodeType, activity);
            //Refines
            this.Create_Connectoren_Refines(database, repository, activity, nodeType);
        }

        public void Create_Connectoren_Measurement(Requirement_Plugin.Database Database, EA.Repository repository)
        {
            Repsoitory_Elements.Repository_Connector repository_Connector = new Repsoitory_Elements.Repository_Connector();

            if(this.m_requirement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    repository_Connector.Create_Dependency(this.m_requirement[i1].Classifier_ID, this.Measurement.Classifier_ID, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                    i1++;
                } while (i1 < this.m_requirement.Count);
            }
        }

        public void Create_Connectoren_Instanzen(Requirement_Plugin.Database Database, EA.Repository repository)
        {
            Repsoitory_Elements.Repository_Connector repository_Connector = new Repsoitory_Elements.Repository_Connector();

            int i1 = 0;
            do
            {
                var guid = this.Measurement.Get_Parent_GUID(Database);
                int i2 = 0;
                do
                {
                    if (this.m_requirement[i2].Classifier_ID != null)
                    {
                        //repository_Connector.Create_Dependency(this.requirement.Classifier_ID, this.m_guid_Instanzen[i1], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database);
                        repository_Connector.Create_Dependency(this.m_requirement[i2].Classifier_ID, guid, Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);

                    }
                    i2++;
                } while (i2 < this.m_requirement.Count);
               



                i1++;
            } while (i1 < this.m_guid_Instanzen.Count);
        }


        public void Create_Connectoren_element(Requirement_Plugin.Database Database, EA.Repository repository, Elements.Element_Functional func)
        {
            Repsoitory_Elements.Repository_Connector repository_Connector = new Repsoitory_Elements.Repository_Connector();

            if(this.m_guid_Instanzen.Count > 0)
            {
                int i1 = 0;
                do
                {
                    string guid = this.Measurement.Get_Parent_GUID(Database);
                    List<string> m_help = func.Get_Client_GUID_Target_By_SupplierID(guid);
                    //List<string> m_help = func.Get_Client_GUID_Target_By_SupplierID(this.m_guid_Instanzen[i1]);

                    //////////////////////////////
                    //Abfrage Pool/Lane
                 /*   Repsoitory_Elements.Activity activity = new Repsoitory_Elements.Activity(null, null, null);
                    Repsoitory_Elements.Repository_Element repository_Element = new Repsoitory_Elements.Repository_Element();
                    repository_Element.Classifier_ID = this.m_guid_Instanzen[i1];
                    activity.Classifier_ID = repository_Element.Get_Classifier(Database);

                    int lane_id = activity.Get_Parent_ID(Database);

                    if(lane_id != null)
                    {
                        if(lane_id != -1)
                        {

                        }
                    }*/


                    if (m_help.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            int i3 = 0;
                            do
                            {
                                repository_Connector.Create_Dependency(this.m_requirement[i3].Classifier_ID, m_help[i2], Database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Database.metamodel.m_Derived_Element[0].SubType, repository, Database, Database.metamodel.m_Derived_Element[0].Toolbox, Database.metamodel.m_Derived_Element[0].direction);


                                i3++;
                            } while (i3 < this.m_requirement.Count);

                           

                            i2++;
                        } while (i2 < m_help.Count);


                    }


                    i1++;
                } while (i1 < this.m_guid_Instanzen.Count);
            }

            
        }

        public void Create_Connectoren_Refines(Requirement_Plugin.Database Database, EA.Repository repository, Repsoitory_Elements.Activity activity, Repsoitory_Elements.NodeType nodeType)
        {

            Repsoitory_Elements.Repository_Connector repository_Connector = new Repsoitory_Elements.Repository_Connector();

            #region Element_functional
            List<Elements.Element_Functional> m_func = activity.m_Element_Functional.Where(x => x.Client == nodeType).ToList();

            if(m_func.Count > 0)
            {
                if(m_func[0].m_Requirement_Functional.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(this.m_requirement.Count > 0  && m_func[0].m_Requirement_Functional[i1] != null)
                        {
                            int i2 = 0;
                            do
                            {
                                repository_Connector.Create_Dependency(this.m_requirement[i2].Classifier_ID, m_func[0].m_Requirement_Functional[i1].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);


                                i2++;
                            } while (i2 < this.m_requirement.Count);

                        }
                        i1++;
                    } while (i1 < m_func[0].m_Requirement_Functional.Count);
                }
            }
            #endregion

            #region Element_User
            List<Elements.Element_User> m_user = activity.m_Element_User.Where(x => x.Client == nodeType).ToList();

            if (m_user.Count > 0)
            {
                if (m_user[0].m_Requirement_User.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (this.m_requirement.Count > 0  && m_user[0].m_Requirement_User[i1] != null)
                        {
                            int i2 = 0;
                            do
                            {
                                repository_Connector.Create_Dependency(this.m_requirement[i2].Classifier_ID, m_user[0].m_Requirement_User[i1].Classifier_ID, Database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), Database.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Database.metamodel.m_Afo_Refines[0].SubType, repository, Database, Database.metamodel.m_Afo_Refines[0].Toolbox, Database.metamodel.m_Afo_Refines[0].direction);
                                i2++;
                            } while (i2 < this.m_requirement.Count);

                        }
                        i1++;
                    } while (i1 < m_user[0].m_Requirement_User.Count);
                }
            }
            #endregion
        }
        #endregion

        #region Create_Requirement_Class
        public void Create_Requirement_Class(EA.Repository repository,Requirement_Plugin.Database database, string Package_GUID, Repsoitory_Elements.NodeType nodeType, string PAckage_GUID)
        {
            //AFO erzeugen
            if (this.m_requirement.Count == 0)
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

                #region AFO Parameter
                //W_Object
                string W_Object = "";
                //W_Prozesswort
                string W_Prozesswort = "aufweisen";
                //W_Qualitaet
                string W_Qualitaet = "eine " + this.Measurement.measurementType.Name + " von " + this.Measurement.Name;
                //W_Randbedingung
                string W_Randbedingung = "";
                //W_Singular
                bool W_Singular = nodeType.W_SINGULAR;
                //W_Subject
                string W_Subject = nodeType.W_Artikel + " " + nodeType.Name;
                //W_Zu
                bool W_zu = false;
                //recent_guid
                string recent_guid = "";
                //Titel
                string Titel = nodeType.Name + " - "+ this.Measurement.measurementType.Name+ " von " + this.Measurement.Name;
                //Text
                string AFO_Text = "";
                #endregion

                #region AFO Text
                if (W_Singular == true)
                {
                    AFO_Text = ti.ToTitleCase(nodeType.W_Artikel) + " " + nodeType.Name + " " + database.metamodel.m_Verbindlichkeit[0] + " eine " + this.Measurement.measurementType.Name + " von "+ this.Measurement.Name + " aufweisen.";
                }
                else
                {
                    AFO_Text = ti.ToTitleCase(nodeType.W_Artikel) + " " + nodeType.Name + " " + database.metamodel.m_Verbindlichkeit[1] + " eine " + this.Measurement.measurementType.Name + " von " + this.Measurement.Name + " aufweisen.";
                }
                #endregion

                #region AFO ERstellung
                Requirements.Requirement_Non_Functional requirement2 = new Requirements.Requirement_Non_Functional(null, database.metamodel);
                requirement2.Add_Requirement_Qualitaet_Class(Titel, AFO_Text, W_Object, W_Prozesswort, W_Qualitaet, W_Randbedingung, W_Singular, W_Subject, database.metamodel.m_Requirement_Quality_Class[0].Stereotype);
                requirement2.W_AFO_MANUAL = false;

                requirement2.Classifier_ID = requirement2.Create_Requirement(repository, PAckage_GUID, database.metamodel.m_Requirement_Quality_Class.Select(x => x.Stereotype).ToList()[0], database);

                this.m_requirement.Add(requirement2);
                #endregion

                #region Konnektoren
                //Connectoren ziehen
                //Konnector auf NodeType
                /* this.Create_Connectoren_Instanzen(database, repository);
                 //Konnector auf Measurement
                 this.Create_Connectoren_Measurement(database, repository);
                 //Generalisierung
                 this.Create_Connectoren_Generalisierung_Class(repository, database, nodeType);
                 //Dubletten
                 this.Create_Connectoren_Dubletten_Class(repository, database, nodeType);*/
                this.Update_Connectoren_QualityClass(repository, database, nodeType);
                #endregion
            }


           


        }
        #endregion

        #region Create_Requirement_Activity

        public void Create_Requirement_Activity(EA.Repository repository, Requirement_Plugin.Database database, Repsoitory_Elements.NodeType nodeType, Repsoitory_Elements.Activity activity, string PAckage_GUID)
        {
            //AFO erzeugen
            if (this.m_requirement.Count == 0)
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

                #region AFO Parameter
                //W_Object
                string W_Object = activity.W_Object;
                //W_Prozesswort
                string W_Prozesswort = activity.W_Prozesswort;
                //W_Qualitaet
                string W_Qualitaet = "mit einer " + this.Measurement.measurementType.Name + " von " + this.Measurement.Name;
                //W_Randbedingung
                string W_Randbedingung = "";
                //W_Singular
                bool W_Singular = nodeType.W_SINGULAR;
                //W_Subject
                string W_Subject = nodeType.W_Artikel + " " + nodeType.Name;
                //W_Zu
                bool W_zu = false;
                //recent_guid
                string recent_guid = "";
                //Titel
                string Titel = nodeType.Name + " - "+W_Object+" "+W_Prozesswort+" mit einer " + this.Measurement.measurementType.Name + " von " + this.Measurement.Name;
                //Text
                string AFO_Text = "";
                #endregion

                #region AFO Text
                if (W_Singular == true)
                {
                    AFO_Text = ti.ToTitleCase(nodeType.W_Artikel) + " " + nodeType.Name + " " + database.metamodel.m_Verbindlichkeit[0] +" "+W_Object +" mit einer " + this.Measurement.measurementType.Name + " von " + this.Measurement.Name + " "+W_Prozesswort+".";
                }
                else
                {
                    AFO_Text = ti.ToTitleCase(nodeType.W_Artikel) + " " + nodeType.Name + " " + database.metamodel.m_Verbindlichkeit[1] + " " + W_Object + " mit einer " + this.Measurement.measurementType.Name + " von " + this.Measurement.Name + " " + W_Prozesswort + ".";
                }
                #endregion

                #region AFO ERstellung
                Requirements.Requirement_Non_Functional requirement2 = new Requirements.Requirement_Non_Functional(null, database.metamodel);
                requirement2.Add_Requirement_Qualitaet_Activity(Titel, AFO_Text, W_Object, W_Prozesswort, W_Qualitaet, W_Randbedingung, W_Singular, W_Subject, database.metamodel.m_Requirement_Quality_Activity[0].Stereotype);
                requirement2.W_AFO_MANUAL = false;

                requirement2.Classifier_ID = requirement2.Create_Requirement(repository, PAckage_GUID, database.metamodel.m_Requirement_Quality_Activity.Select(x => x.Stereotype).ToList()[0], database);

                this.m_requirement.Add(requirement2);
                #endregion

                #region Konnektoren
                //Connectoren ziehen
                this.Update_Connectoren_QualityActivity(repository, database, nodeType, activity);
                #endregion
            }





        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requirement_Plugin.Interfaces;
using Database_Connection;
using System.Globalization;
using Repsoitory_Elements;
using Requirements;

namespace Elements
{
    public class Element_Environmental
    {
        public OperationalConstraint OpConstraint;
        public List<string> m_GUID;
        public NodeType NodeType;
        public Requirement_Non_Functional requirement;
        public Capability capability;
        public List<Logical> m_Logical;

        public Element_Environmental(List<string> m_Guid, OperationalConstraint opcon, NodeType recent)
        {
            this.m_Logical = new List<Logical>();
            this.m_GUID = m_Guid;
            this.OpConstraint = opcon;
            this.requirement = null;
            this.capability = null;
            this.NodeType = recent;

            if (recent.m_Enviromental.Contains(this) == false)
            {
                recent.m_Enviromental.Add(this);
            }
        }

        #region Check
        public void Check_Requirement_Umwelt(Requirement_Plugin.Database database, EA.Repository repository)
        {
            if (this.m_GUID.Count > 0 && OpConstraint.Classifier_ID != null)
            {
                List<string> m_Type_req_umwelt = database.metamodel.m_Requirement_Environment.Select(x => x.Type).ToList();
                List<string> m_Stereotype_req_umwelt = database.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList();
                List<string> m_Type_DerivedElem = database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                List<string> m_Stereotype_DerivedElem = database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
                List<string> Requ = new List<string>();

                #region NodeType
                List<string> NodeType_Requ = new List<string>();
                Interface_Connectors_Requirement interface_Connectors = new Interface_Connectors_Requirement();
                NodeType_Requ = interface_Connectors.Get_Client_Element_By_Connector(database, this.m_GUID, m_Type_req_umwelt, m_Stereotype_req_umwelt, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);
                #endregion NodeType
                #region OperationalConstraint
                List<string> Activity_Requ = new List<string>();
                Interface_Connectors_Requirement interface_Connectors2 = new Interface_Connectors_Requirement();
                List<string> m_help = new List<string>();
                m_help.Add(this.OpConstraint.Classifier_ID);
                Activity_Requ = interface_Connectors2.Get_Client_Element_By_Connector(database, m_help, m_Type_req_umwelt, m_Stereotype_req_umwelt, m_Type_DerivedElem, m_Stereotype_DerivedElem, database.metamodel.m_Derived_Element[0].direction);
                #endregion OperationalConstraint
                if (NodeType_Requ != null && Activity_Requ != null)
                {
                    int i1 = 0;
                    do
                    {
                        if (Activity_Requ.Contains(NodeType_Requ[i1]))
                        {
                            Requirement_Non_Functional requirement_design = new Requirement_Non_Functional(NodeType_Requ[i1], database.metamodel);
                            requirement_design.Classifier_ID = NodeType_Requ[i1];
                            requirement_design.Get_Tagged_Values_From_Requirement(NodeType_Requ[i1], repository, database);

                            if (this.requirement != (requirement_design))
                            {
                                this.requirement = (requirement_design);
                                database.m_Requirement.Add(this.requirement);
                                //Zuordnung Element_Functional zu einer Capability
                                Check_For_Capability(repository, database);
                                //Check Issue
                                this.requirement.Get_Issues(database);
                                //Check Klärungspunkte
                                this.requirement.Check_Klärungspunkte(this.requirement.m_Issues, database, repository);
                            }
                        }

                        i1++;
                    } while (i1 < NodeType_Requ.Count);
                }
            }




        }

        public void Check_For_Capability(EA.Repository repository, Requirement_Plugin.Database database)
        {
            Capability capability = null;
         //  DB_Command command = new DB_Command();
         //   XML xML = new XML();
            List<string> m_Type_DerivedCap = database.metamodel.m_Derived_Capability.Select(x => x.Type).ToList();
            List<string> m_Stereotype_DerivedCap = database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList();
            List<string> m_Capability_Type = database.metamodel.m_Capability.Select(x => x.Type).ToList();
            List<string> m_Capability_Stereotype = database.metamodel.m_Capability.Select(x => x.Stereotype).ToList();

            if (this.requirement != null)
            {

                List<string> GUID = new List<string>();
                Interface_Connectors interface_Connectors = new Interface_Connectors();
                List<string> m_help = new List<string>();
                m_help.Add(this.requirement.Classifier_ID);
                GUID = interface_Connectors.Get_Supplier_Element_By_Connector(database, m_help, m_Capability_Type, m_Capability_Stereotype, m_Type_DerivedCap, m_Stereotype_DerivedCap);

                if (GUID != null)
                {
                    Capability cap = database.Check_Capability_Database(GUID[0]);

                    if (cap != null)
                    {

                        this.capability = cap;
                        if (this.requirement.m_Capability.Contains(cap) == false)
                        {
                            this.requirement.m_Capability.Add(cap);
                        }

                    }
                }


            }
        }
        #endregion

        #region Create
        public void Create_Requirement_Umwelt(EA.Repository repository, Requirement_Plugin.Database database, string Package_GUID)
        {
            if (this.requirement == null)
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

                #region AFO Parameter
                //W_Object
                string W_Object = "";
                //W_Prozesswort
                string W_Prozesswort = "";
                //W_Qualitaet
                string W_Qualitaet = this.OpConstraint.Name;
                //W_Randbedingung
                string W_Randbedingung = "";
                //W_Singular
                bool W_Singular = this.NodeType.W_SINGULAR;
                //W_Subject
                string W_Subject = this.NodeType.W_Artikel + " " + this.NodeType.Name;
                //W_Zu
                bool W_zu = false;
                //recent_guid
                string recent_guid = "";
                //Titel
                string Titel = this.NodeType.Name + " - Umwelt - " + this.OpConstraint.Name;
                //Text
                string AFO_Text = "";
                #endregion

                #region AFO Text
                if (W_Singular == true)
                {
                    AFO_Text = ti.ToTitleCase(this.NodeType.W_Artikel) + " " + this.NodeType.Name + " " + database.metamodel.m_Verbindlichkeit[0] + " " + W_Qualitaet + " ....";
                }
                else
                {
                    AFO_Text = ti.ToTitleCase(this.NodeType.W_Artikel) + " " + this.NodeType.Name + " " + database.metamodel.m_Verbindlichkeit[1] + " " + W_Qualitaet + " ....";
                }
                #endregion

                #region AFO ERstellung
                Requirement_Non_Functional requirement2 = new Requirement_Non_Functional(null, database.metamodel);
                requirement2.Add_Requirement_Umwelt(Titel, AFO_Text, W_Object, W_Prozesswort, W_Qualitaet, W_Randbedingung, W_Singular, W_Subject, database.metamodel.m_Requirement_Environment[0].Stereotype);
                requirement2.W_AFO_MANUAL = true;
                requirement2.AFO_KLAERUNGSPUNKTE = this.OpConstraint.Notes;

                requirement2.Classifier_ID = requirement2.Create_Requirement(repository, Package_GUID, database.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList()[0], database);

                this.requirement = requirement2;
                #endregion

                #region Konnektoren
                //Konnector auf NodeType
                Create_Connectoren_NodeType(repository, database);
                //Konnector auf Typvertreter
                Create_Connectoren_OperationaConstraint(repository, database);

                Create_Connectoren_Generalisierung(repository, database);

                #endregion
            }
        }

       
        private void Create_Connectoren_NodeType(EA.Repository repository, Requirement_Plugin.Database database)
        {
            if(this.requirement != null)
            {
                Repository_Connector repository_Connector = new Repository_Connector();
                if (this.m_GUID.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        repository_Connector.Create_Dependency(this.requirement.Classifier_ID, this.m_GUID[i1], database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), database.metamodel.m_Derived_Element[0].SubType, repository, database, database.metamodel.m_Derived_Element[0].Toolbox, database.metamodel.m_Derived_Element[0].direction);

                        i1++;
                    } while (i1 < this.m_GUID.Count);
                }
                else
                {
                    repository_Connector.Create_Dependency(this.requirement.Classifier_ID, this.NodeType.Classifier_ID, database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), database.metamodel.m_Derived_Element[0].SubType, repository, database, database.metamodel.m_Derived_Element[0].Toolbox, database.metamodel.m_Derived_Element[0].direction);
                }
            }

        }
        private void Create_Connectoren_OperationaConstraint(EA.Repository repository, Requirement_Plugin.Database database)
        {
            if (this.requirement != null)
            {
                Repository_Connector repository_Connector = new Repository_Connector();
                if (this.OpConstraint != null)
                {
                    repository_Connector.Create_Dependency(this.requirement.Classifier_ID, this.OpConstraint.Classifier_ID, database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), database.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), database.metamodel.m_Derived_Element[0].SubType, repository, database, database.metamodel.m_Derived_Element[0].Toolbox, database.metamodel.m_Derived_Element[0].direction);
                }
            }

        }

        public void Create_Connectoren_Generalisierung(EA.Repository repository, Requirement_Plugin.Database database)
        {
            Repository_Connector repository_Connector = new Repository_Connector();

            #region Generalsiierung runter
            List<NodeType> m_Specilize = new List<NodeType>();
            m_Specilize = this.NodeType.m_Specialize;

            if (m_Specilize.Count > 0)
            {
                List<Element_Environmental> m_op = m_Specilize.SelectMany(x => x.m_Enviromental).ToList().Where(x => x.OpConstraint == this.OpConstraint).ToList();

                if (m_op.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_op[i1].requirement != null)
                        {
                            if (m_op[i1].requirement.Classifier_ID != null && this.requirement.Classifier_ID != null)
                            {
                                repository_Connector.Create_Dependency(m_op[i1].requirement.Classifier_ID, this.requirement.Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                            }
                        }

                        i1++;
                    } while (i1 < m_op.Count);
                }
            }
            #endregion

            #region Generalisierung hoch
            List<NodeType> m_Specified = this.NodeType.Get_All_SpecifiedBy(database.m_NodeType);

            if (m_Specified != null)
            {
                List<Element_Environmental> m_op = m_Specified.SelectMany(x => x.m_Enviromental).ToList().Where(x => x.OpConstraint == this.OpConstraint).ToList();

                if (m_op.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_op[i1].requirement != null)
                        {
                            if (m_op[i1].requirement.Classifier_ID != null && this.requirement.Classifier_ID != null)
                            {
                                repository_Connector.Create_Dependency(this.requirement.Classifier_ID, m_op[i1].requirement.Classifier_ID, database.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), database.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), database.metamodel.m_Afo_Dublette[0].SubType, repository, database, database.metamodel.m_Afo_Dublette[0].Toolbox, database.metamodel.m_Afo_Dublette[0].direction);

                            }
                        }

                        i1++;
                    } while (i1 < m_op.Count);
                }
            }
            #endregion

        }
        #endregion

        #region Update
        public void Update_Connectoren_Requiremnts_Umwelt(EA.Repository repository, Requirement_Plugin.Database database)
        {
            #region Konnektoren
            //Konnector auf NodeType
            Create_Connectoren_NodeType(repository, database);
            //Konnector auf Typvertreter
            Create_Connectoren_OperationaConstraint(repository, database);
            //
            Create_Connectoren_Generalisierung(repository, database);
            #endregion
        }

        #endregion

        #region Copy
        public Element_Environmental Copy_Element_Umwelt(NodeType Target, EA.Repository repository, Requirement_Plugin.Database database)
        {
            List<string> m_guid = new List<string>();
            m_guid.Add(Target.Classifier_ID);

            Element_Environmental element_umwelt = new Element_Environmental(m_guid, this.OpConstraint, Target);
            element_umwelt.capability = this.capability;
            element_umwelt.m_Logical = this.m_Logical;

            element_umwelt.Check_Requirement_Umwelt(database, repository);

            return (element_umwelt);
        }
        #endregion
    }
}

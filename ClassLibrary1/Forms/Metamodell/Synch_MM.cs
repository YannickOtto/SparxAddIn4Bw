using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Metamodels;

namespace Forms
{
    public partial class Synch_MM : Form
    {
        Metamodel metamodel;
        EA.Repository repository;

        public Synch_MM(Metamodel metamodel, EA.Repository repository)
        {
            InitializeComponent();

            this.metamodel = metamodel;
            this.repository = repository;

        }

        private void button_Synch_All_Click(object sender, EventArgs e)
        {
            #region Synch Elemente
            Synch_Szenar();
            Synch_CapConf();
            Synch_Elements_Definition();
            Synch_Elements_Usage();
            Synch_Requirements();
            Synch_InformationItem();
            Synch_Activity_Def();
            Synch_Activity_Usage();
            Synch_Stakeholder_Def();
            Synch_Stakeholder_Usage();
            Synch_Fähigkeitsbaum();
            #endregion Synch Elemente

            #region Synch Konnecktoren
            Synch_Derived();
            Synch_Req_Con();
            Synch_Infoaus();
            Synch_Verhalten();
            Synch_Verhalten_St();
            Synch_Taxonomy();
            #endregion Synch Konnektoren


        }

        #region Synch Elemente
        private void Synch_Szenar()
        {
            if(this.metamodel.m_Szenar.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Szenar[i1].Toolbox, this.metamodel.m_Szenar[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Szenar.Count);
            } 
        }
        private void Synch_CapConf()
        {
            if (this.metamodel.m_CapConf.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_CapConf[i1].Toolbox, this.metamodel.m_CapConf[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_CapConf.Count);
            }
        }
        private void Synch_Elements_Definition()
        {
            if (this.metamodel.m_Elements_Definition.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Elements_Definition[i1].Toolbox, this.metamodel.m_Elements_Definition[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Elements_Definition.Count);
            }
        }
        private void Synch_Elements_Usage()
        {
            if (this.metamodel.m_Elements_Usage.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Elements_Usage[i1].Toolbox, this.metamodel.m_Elements_Usage[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Elements_Usage.Count);
            }
        }
        private void Synch_Requirements()
        {
            if (this.metamodel.m_Requirement.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Requirement[i1].Toolbox, this.metamodel.m_Requirement[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Requirement.Count);
            }
        }
        private void Synch_InformationItem()
        {
            if (this.metamodel.m_InformationItem.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_InformationItem[i1].Toolbox, this.metamodel.m_InformationItem[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_InformationItem.Count);
            }
        }
        private void Synch_Activity_Def()
        {
            if (this.metamodel.m_Aktivity_Definition.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Aktivity_Definition[i1].Toolbox, this.metamodel.m_Aktivity_Definition[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Aktivity_Definition.Count);
            }
        }
        private void Synch_Activity_Usage()
        {
            if (this.metamodel.m_Aktivity_Usage.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Aktivity_Usage[i1].Toolbox, this.metamodel.m_Aktivity_Usage[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Aktivity_Usage.Count);
            }
        }
        private void Synch_Stakeholder_Def()
        {
            if (this.metamodel.m_Stakeholder_Definition.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Stakeholder_Definition[i1].Toolbox, this.metamodel.m_Stakeholder_Definition[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Stakeholder_Definition.Count);
            }
        }
        private void Synch_Stakeholder_Usage()
        {
            if (this.metamodel.m_Stakeholder_Usage.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Stakeholder_Usage[i1].Toolbox, this.metamodel.m_Stakeholder_Usage[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Stakeholder_Usage.Count);
            }
        }
        private void Synch_Fähigkeitsbaum()
        {
            if (this.metamodel.m_Capability.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Capability[i1].Toolbox, this.metamodel.m_Capability[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Capability.Count);
            }
        }
        #endregion Synch Elemente
        private void Synch_Derived()
        {
            if (this.metamodel.m_Derived_Capability.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Derived_Capability[i1].Toolbox, this.metamodel.m_Derived_Capability[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Derived_Capability.Count);
            }
            if (this.metamodel.m_Derived_Element.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Derived_Element[i1].Toolbox, this.metamodel.m_Derived_Element[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Derived_Element.Count);
            }
            if (this.metamodel.m_Derived_Logical.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Derived_Logical[i1].Toolbox, this.metamodel.m_Derived_Logical[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Derived_Logical.Count);
            }
        }
        private void Synch_Req_Con()
        {
            if (this.metamodel.m_Afo_Dublette.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Afo_Dublette[i1].Toolbox, this.metamodel.m_Afo_Dublette[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Afo_Dublette.Count);
            }
            if (this.metamodel.m_Afo_Konflikt.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Afo_Konflikt[i1].Toolbox, this.metamodel.m_Afo_Konflikt[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Afo_Konflikt.Count);
            }
            if (this.metamodel.m_Afo_Refines.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Afo_Refines[i1].Toolbox, this.metamodel.m_Afo_Refines[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Afo_Refines.Count);
            }
            if (this.metamodel.m_Afo_Replaces.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Afo_Replaces[i1].Toolbox, this.metamodel.m_Afo_Replaces[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Afo_Replaces.Count);
            }
            if (this.metamodel.m_Afo_Requires.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Afo_Requires[i1].Toolbox, this.metamodel.m_Afo_Requires[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Afo_Requires.Count);
            }
        }
        private void Synch_Infoaus()
        {
            if (this.metamodel.m_Infoaus.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Infoaus[i1].Toolbox, this.metamodel.m_Infoaus[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Infoaus.Count);
            }
         
        }
        private void Synch_Verhalten()
        {
            if (this.metamodel.m_Behavior.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Behavior[i1].Toolbox, this.metamodel.m_Behavior[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Behavior.Count);
            }

        }
        private void Synch_Verhalten_St()
        {
            if (this.metamodel.m_Stakeholder.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Stakeholder[i1].Toolbox, this.metamodel.m_Stakeholder[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Stakeholder.Count);
            }

        }
        private void Synch_Taxonomy()
        {
            if (this.metamodel.m_Taxonomy_Capability.Count > 0)
            {
                int i1 = 0;
                do
                {

                    repository.SynchProfile(this.metamodel.m_Taxonomy_Capability[i1].Toolbox, this.metamodel.m_Taxonomy_Capability[i1].Stereotype);

                    i1++;
                } while (i1 < this.metamodel.m_Taxonomy_Capability.Count);
            }

        }
        #region Synch Konnektoren


        #endregion Synch Konnektoren

        private void button_Synch_Elem_Click(object sender, EventArgs e)
        {
            Synch_Szenar();
            Synch_CapConf();
            Synch_Elements_Definition();
            Synch_Elements_Usage();
            Synch_Requirements();
            Synch_InformationItem();
            Synch_Activity_Def();
            Synch_Activity_Usage();
            Synch_Stakeholder_Def();
            Synch_Stakeholder_Usage();
            Synch_Fähigkeitsbaum();
        }

        private void button_Synch_Con_Click(object sender, EventArgs e)
        {
            Synch_Derived();
            Synch_Req_Con();
            Synch_Infoaus();
            Synch_Verhalten();
            Synch_Verhalten_St();
            Synch_Taxonomy();
        }
    }
}

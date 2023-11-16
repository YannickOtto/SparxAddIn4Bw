using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Forms;

namespace Requirement_Plugin.Domain.Bewertung_Afo
{
    public class Bewertung
    {
        public List<Repsoitory_Elements.Capability> m_capability;
        public List<Requirements.Requirement> m_req_gewicht;

        public Bewertung(List<Repsoitory_Elements.Capability> m_cap)
        {
            this.m_capability = m_cap;
            this.m_req_gewicht = new List<Requirements.Requirement>();

            List<Requirements.Requirement> m_req_cap = this.m_capability.SelectMany(x => x.m_Requirement).ToList();

            if (m_req_cap.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_req_cap[i1].AFO_ABS_GEWICHT != "" && m_req_cap[i1].AFO_ABS_GEWICHT != "not set" && m_req_cap[i1].AFO_ABS_GEWICHT != null)
                    {
                        m_req_gewicht.Add(m_req_cap[i1]);
                    }


                    i1++;
                } while (m_req_cap.Count > i1);
            }
        }


        public void Reset_Gewicht(Database database, EA.Repository repository)
        {
            if(m_req_gewicht.Count > 0)
            {
                //Package_Issue
               // EA.Element element;
                List<string> m_Type_con = new List<string>();
                m_Type_con.Add("Dependency");
                List<string> m_Stereotype_con = new List<string>();
                m_Stereotype_con.Add("trace");
                List<string> m_Toolbox_con = new List<string>();
                m_Toolbox_con.Add("");
                Repsoitory_Elements.Repository_Element repository_Element = new Repsoitory_Elements.Repository_Element();
                Repsoitory_Elements.Repository_Connector con = new Repsoitory_Elements.Repository_Connector();
                string Package_guid = repository_Element.Create_Package_Model(database.metamodel.m_Package_Name[2], repository, database);
                string Package_guid_child =  repository_Element.Create_Package("RPI - Gewichtung", repository.GetPackageByGuid(Package_guid), repository, database);

                int i1 = 0;
                do
                {
                    //Issue mit Rang und Gewicht erzeugen und eintragen
                    #region Issue
                    string Name = DateTime.Now.ToString() + " Gewichtung " + repository.GetElementByGuid(m_req_gewicht[i1].Classifier_ID).Name;
                    string Note = "Rang: " + m_req_gewicht[i1].AFO_RANG + " Gewicht (absolut)" + m_req_gewicht[i1].AFO_ABS_GEWICHT;

                    Repsoitory_Elements.Issue issue = new Repsoitory_Elements.Issue(database, Name, Note, Package_guid_child, repository, true, "Bewertung");
                    con.Create_Dependency(issue.Classifier_ID, m_req_gewicht[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, database, m_Toolbox_con[0], true);
                    #endregion
                    #region TaggedValues
                    m_req_gewicht[i1].AFO_ABS_GEWICHT = "";
                    m_req_gewicht[i1].AFO_RANG = "";

                    m_req_gewicht[i1].Update_TV_Gewichtung(database, repository);
                    #endregion


                    i1++;
                } while (i1 < m_req_gewicht.Count);
            }
            m_req_gewicht = new List<Requirements.Requirement>();
        }


        public void Start_Gewichtung(Database database, EA.Repository repository)
        {
            #region Loading Balken
            Loading_OpArch load = new Loading_OpArch();
            load.label2.Text = "Gewichtung Anforderungen";
            load.label_Progress.Text = "Erhalten Funktionsbaum";

            load.progressBar1.Minimum = 0;
            load.progressBar1.Maximum = 4;
            load.progressBar1.Value = 0;
            load.progressBar1.Step = 1;
            load.Refresh();
            load.Show();
            #endregion
            #region Capability Baum
            //Baum der aktuellen Capability aufbauen
            if (this.m_capability.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_capability[i1].Get_Child(database, repository);
                    this.m_capability[i1].Get_Parent(database, repository);

                    this.m_capability[i1].rang = 0;

                    i1++;
                } while (i1 < this.m_capability.Count);
            }

            #endregion

            #region Rang setzen
            load.progressBar1.PerformStep();
            load.label_Progress.Text = "Setzen Rang Anforderungen";
            load.Refresh();
            //Nur Anforderungen vom Funktionsbaum der untersten Ebene betrachten
            //Rang setzen
            List<Repsoitory_Elements.Capability> m_low_cap = new List<Repsoitory_Elements.Capability>();
            m_low_cap = m_capability.Where(x => x.m_Child.Count == 0).ToList();
            List<Requirements.Requirement> m_low_req = new List<Requirements.Requirement>();
            m_low_req = m_low_cap.SelectMany(x => x.m_Requirement).ToList();

            m_low_req = m_low_req.Where(x => x.AFO_WV_ART == Ennumerationen.AFO_WV_ART.Anforderung).ToList();

            if (m_low_req.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Set_Rang(m_low_req[i1]);

                    i1++;
                } while (i1 < m_low_req.Count);

            }
            #endregion

            #region Rang den Capability zuordnen
            load.progressBar1.PerformStep();
            load.label_Progress.Text = "Setzen Rang Funktionsbaum";
            load.Refresh();
            if (m_low_cap.Count > 0)
            {
                List<Repsoitory_Elements.Capability> m_parent = new List<Repsoitory_Elements.Capability>();


                int i1 = 0;
                do
                {
                    m_low_cap[i1].Set_Rang(0);

                    i1++;
                } while (i1 < m_low_cap.Count);


              /*  if(m_parent.Count > 0)
                {
                    Get_Rang(m_parent);
                }
              */
            }

            #endregion

            #region Capability relatives Gewicht verteilen
            load.progressBar1.PerformStep();
            load.label_Progress.Text = "Setzen relatives Gewicht Funktionsbaum";
            load.Refresh();
            //Gesamt Rang erhalten

            List<Repsoitory_Elements.Capability> m_root = new List<Repsoitory_Elements.Capability>();
            m_root = this.m_capability.Where(y => y.m_Parent.Count == 0).ToList();

            int rang_ges = m_root.Select(x => x.rang).Take(m_root.Count).ToList().Sum();

            if(m_root.Count > 0)
            {
                int i1 = 0;
                do
                {
                    m_root[i1].Set_Rel_Gewicht(rang_ges);

                    i1++;
                } while (i1 < m_root.Count);
            }

            #endregion


            #region Update Capability und Requirements m_low_req
            load.progressBar1.PerformStep();
            load.label_Progress.Text = "Update Funktionsbaum";
            load.Refresh();
            //Funktionsbaum
            if (this.m_capability.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_capability[i1].Update_TV_Gewichtung(database, repository);

                    i1++;
                } while (i1 < this.m_capability.Count);
            }
            //Anforderung
            load.progressBar1.PerformStep();
            load.label_Progress.Text = "Update Anforderungen";
            load.Refresh();
            if (m_low_req.Count > 0)
            {
                int i1 = 0;
                do
                {
                    m_low_req[i1].Update_TV_Gewichtung(database, repository);

                    i1++;
                } while (i1 < m_low_req.Count);
            }
            #endregion

            load.Close();

            //
        }

        private void Set_Rang(Requirements.Requirement requirement)
        {
            requirement.AFO_ABS_GEWICHT = "";

            switch(requirement.AFO_OPERATIVEBEWERTUNG)
            {
                case Ennumerationen.AFO_OPERATIVEBEWERTUNG.unerheblich:
                    requirement.AFO_RANG = "10";
                    requirement.rang = 10;
                    break;
                case Ennumerationen.AFO_OPERATIVEBEWERTUNG.notwendig:
                    requirement.AFO_RANG = "20";
                    requirement.rang = 20;
                    break;
                case Ennumerationen.AFO_OPERATIVEBEWERTUNG.essentiell:
                    requirement.AFO_RANG = "30";
                    requirement.rang = 30;
                    break;
                case Ennumerationen.AFO_OPERATIVEBEWERTUNG.kritisch:
                    requirement.AFO_RANG = "";
                    requirement.rang = -1;
                    break;
            }
        }


     /*   private void Get_Rang(List<Repsoitory_Elements.Capability> m_parent)
        {
            if(m_parent.Count > 0)
            {
                List<Repsoitory_Elements.Capability> m_parent_new = new List<Repsoitory_Elements.Capability>();

                int i1 = 0;
                do
                {
                    List<Repsoitory_Elements.Capability> m_ret = m_parent[i1].Set_Rang();

                    if (m_ret.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (m_parent_new.Contains(m_ret[i2]) == false)
                            {
                                m_parent_new.Add(m_ret[i2]);
                            }

                            i2++;
                        } while (i2 < m_ret.Count);
                    }

                    i1++;
                } while (i1 < m_parent.Count);

                Get_Rang(m_parent_new);
            }
        }
     */
    }

}



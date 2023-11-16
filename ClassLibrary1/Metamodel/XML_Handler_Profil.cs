using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;

namespace Metamodels
{
    public class XML_Handler_Profil
    {
        Metamodel metamodel;

        public XML_Handler_Profil(Metamodel metamodel)
        {
            this.metamodel = metamodel;
        }

        #region Export
        public void Export_Profil()
        {
            try
            {
                //Filename
                string filename = this.Choose_Savespace();

                //Erstellung XMLDatei
                #region Add_Header
                var xml_dat = new XDocument(
                     new XDeclaration("1.0", "UTF-8", "true"),
                     new XComment("Dies ist ein Profil für das RPI")
                    );
                var Root = new XElement("Profil_RPI");
                var Att_revision = new XAttribute("revision", "1");
                var Att_itemspec = new XAttribute("itemspec", "");
                var Att_description = new XAttribute("description", "");
                var Att_lastmodiefiedby = new XAttribute("lastmodifiedby", this.metamodel.createdby);//Autor eintragen?
                var Att_Name = new XAttribute("Custom", this.metamodel.createdby);//Autor eintragen?
                                                                                                     //Hinzufügen der Attribute
                Root.Add(Att_revision);
                Root.Add(Att_itemspec);
                Root.Add(Att_description);
                Root.Add(Att_lastmodiefiedby);
                Root.Add(Att_Name);
                xml_dat.Add(Root);
                #endregion

                #region Elemente hinzufügen
                XElement elem = new XElement("Elements");
                Root.Add(elem);
                Add_Szenar(elem);
                Add_Decomposition(elem);
                Add_Anforderungen(elem);
                Add_InformationItem(elem);
                Add_Aktivity(elem);
                Add_Stakeholder(elem);
                Add_Fähigkeitsbaum(elem);
                Add_Constraints(elem);
                Add_Typvertreter(elem);
                Add_BPMN(elem);
                #endregion Elemente hinzufügen

                #region Konnektoren hinzufügen
                XElement con = new XElement("Connectors");
                Root.Add(con);
                Add_Derived(con);
                Add_Relation_Requirement(con);
                Add_InformationExchange(con);
                Add_Behavior(con);
                Add_Behavior_Stakeholder(con);
                Add_Taxonomy(con);
                Add_Satisfy(con);
                Add_Typvertreter_Con(con);
                Add_BPMN_Con(con);
                #endregion Konnektoren hinzufügen

                #region In File schreiben
                //string xac_Header = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"true\"?>";
                string xml_string = xml_dat.ToString();
                //xml_dat.Save(filename);
                System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
                //file.WriteLine(xac_Header);
                file.WriteLine(xml_string);
                file.Close();
                #endregion


                MessageBox.Show("Profil_RPI erfolgreich exportiert.");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        #region Add Elemente
        private void Add_Szenar(XElement header)
        {
            XElement szenar = new XElement("Element_Logical");

            Add_Database_Element(this.metamodel.m_Szenar, szenar);

            header.Add(szenar);
        }
        private void Add_Decomposition(XElement header)
        {
            XElement dec = new XElement("Element_Decomposition");

            if (this.metamodel.m_Elements_Definition.Count > 0)
            {
                int i1 = 0;
                do
                {
                    XElement recent_pair = new XElement("pair");
                    XElement recent_def = new XElement("Definition");
                    XElement recent_usage = new XElement("Usage");
                    dec.Add(recent_pair);
                    recent_pair.Add(recent_def);
                    recent_pair.Add(recent_usage);

                    Add_xElement(this.metamodel.m_Elements_Definition[i1], recent_def);
                    Add_xElement(this.metamodel.m_Elements_Usage[i1], recent_usage);

                    i1++;
                } while (i1 < this.metamodel.m_Elements_Definition.Count);
            }

            header.Add(dec);
        }
        private void Add_Anforderungen(XElement header)
        {
            XElement req = new XElement("Element_Requirement");
            XElement req_func = new XElement("Requirement_Functional");
            XElement req_if = new XElement("Requirement_Interface");
            XElement req_us = new XElement("Requirement_User");
            XElement req_des = new XElement("Requirement_Design");
            XElement req_pro = new XElement("Requirement_Process");
            XElement req_env = new XElement("Requirement_Umwelt");
            XElement req_typ = new XElement("Requirement_Typvertreter");
            XElement req_qual = new XElement("Requirement_Quality");
            XElement req_nonf = new XElement("Requirement_NonFunctional");

            Add_Database_Element(this.metamodel.m_Requirement_Functional, req_func);
            Add_Database_Element(this.metamodel.m_Requirement_Interface, req_if);
            Add_Database_Element(this.metamodel.m_Requirement_Design, req_us );
            Add_Database_Element(this.metamodel.m_Requirement_Design, req_des);
            Add_Database_Element(this.metamodel.m_Requirement_Process, req_pro);
            Add_Database_Element(this.metamodel.m_Requirement_Environment, req_env);
            Add_Database_Element(this.metamodel.m_Requirement_Typvertreter, req_typ);
            Add_Database_Element(this.metamodel.m_Requirement_Quality_Class, req_qual);
            Add_Database_Element(this.metamodel.m_Requirement_Quality_Activity, req_qual);
            Add_Database_Element(this.metamodel.m_Requirement_NonFunctional, req_nonf);

            req.Add(req_func);
            req.Add(req_if);
            req.Add(req_us);
            req.Add(req_des);
            req.Add(req_pro);
            req.Add(req_env);
            req.Add(req_typ);
            req.Add(req_qual);
            req.Add(req_nonf);

            header.Add(req);
        }
        private void Add_InformationItem(XElement header)
        {
            XElement infoitem = new XElement("Element_InformationItem");

            Add_Database_Element(this.metamodel.m_InformationItem, infoitem);

            header.Add(infoitem);
        }
        private void Add_Aktivity(XElement header)
        {
            XElement dec = new XElement("Element_Aktivity");

            if (this.metamodel.m_Aktivity_Definition.Count > 0)
            {
                int i1 = 0;
                do
                {
                    XElement recent_pair = new XElement("pair");
                    XElement recent_def = new XElement("Definition");
                    XElement recent_usage = new XElement("Usage");
                    dec.Add(recent_pair);
                    recent_pair.Add(recent_def);
                    recent_pair.Add(recent_usage);

                    Add_xElement(this.metamodel.m_Aktivity_Definition[i1], recent_def);
                    Add_xElement(this.metamodel.m_Aktivity_Usage[i1], recent_usage);

                    i1++;
                } while (i1 < this.metamodel.m_Aktivity_Definition.Count);
            }

            header.Add(dec);
        }
        private void Add_Stakeholder(XElement header)
        {
            XElement dec = new XElement("Element_Stakeholder");

            if (this.metamodel.m_Stakeholder_Definition.Count > 0)
            {
                int i1 = 0;
                do
                {
                    XElement recent_pair = new XElement("pair");
                    XElement recent_def = new XElement("Definition");
                    XElement recent_usage = new XElement("Usage");
                    dec.Add(recent_pair);
                    recent_pair.Add(recent_def);
                    recent_pair.Add(recent_usage);

                    Add_xElement(this.metamodel.m_Stakeholder_Definition[i1], recent_def);
                    Add_xElement(this.metamodel.m_Stakeholder_Usage[i1], recent_usage);

                    i1++;
                } while (i1 < this.metamodel.m_Stakeholder_Definition.Count);
            }

            header.Add(dec);
        }
        private void Add_Fähigkeitsbaum(XElement header)
        {
            XElement szenar = new XElement("Element_Fähigkeitsbaum");

            Add_Database_Element(this.metamodel.m_Capability, szenar);

            header.Add(szenar);
        }
        private void Add_Constraints(XElement header)
        {
            XElement constraint = new XElement("Element_Constraint");

            XElement Design = new XElement("Element_DesignConstraint");
            Add_Database_Element(this.metamodel.m_Design_Constraint, Design);
            constraint.Add(Design);

            XElement Process = new XElement("Element_ProcessConstraint");
            Add_Database_Element(this.metamodel.m_Process_Constraint, Process);
            constraint.Add(Process);

            XElement Umwelt = new XElement("Element_UmweltConstraint");
            Add_Database_Element(this.metamodel.m_Constraint_Umwelt, Umwelt);
            constraint.Add(Umwelt);



            header.Add(constraint);
        }
        private void Add_Typvertreter(XElement header)
        {
            XElement dec = new XElement("Element_Typvertreter");

            if (this.metamodel.m_Typvertreter_Definition.Count > 0)
            {
                int i1 = 0;
                do
                {
                    XElement recent_pair = new XElement("pair");
                    XElement recent_def = new XElement("Definition");
                    XElement recent_usage = new XElement("Usage");
                    dec.Add(recent_pair);
                    recent_pair.Add(recent_def);
                    recent_pair.Add(recent_usage);

                    Add_xElement(this.metamodel.m_Typvertreter_Definition[i1], recent_def);
                    Add_xElement(this.metamodel.m_Typvertreter_Usage[i1], recent_usage);

                    i1++;
                } while (i1 < this.metamodel.m_Typvertreter_Definition.Count);
            }

            header.Add(dec);
        }

        private void Add_BPMN(XElement header)
        {
            XElement szenar = new XElement("Element_BPMN");

            XElement szenar1 = new XElement("Element_Pool_BPMN");

             Add_Database_Element(this.metamodel.m_Pools_BPMN, szenar);

            szenar.Add(szenar1);

            XElement szenar2 = new XElement("Element_Lane_BPMN");

            Add_Database_Element(this.metamodel.m_Pools_BPMN, szenar2);

            szenar.Add(szenar2);

            XElement szenar3 = new XElement("Element_Activity_BPMN");

            Add_Database_Element(this.metamodel.m_Aktivity_Definition_BPMN, szenar3);

            szenar.Add(szenar3);

            header.Add(szenar);
        }
        #endregion Add Elemente

        #region Add Konnektoren
        private void Add_Derived(XElement header)
        {
            XElement derived = new XElement("Relation_Derived");
            XElement derived_elem = new XElement("Element");
            XElement derived_log = new XElement("Logical");
            XElement derived_cap = new XElement("Capability");

            Add_Database_Connector(this.metamodel.m_Derived_Capability, derived_cap);
            Add_Database_Connector(this.metamodel.m_Derived_Element, derived_elem);
            Add_Database_Connector(this.metamodel.m_Derived_Logical, derived_log);

            derived.Add(derived_elem);
            derived.Add(derived_log);
            derived.Add(derived_cap);
            header.Add(derived);
        }
        private void Add_Relation_Requirement(XElement header)
        {
            XElement re_req = new XElement("Relation_Requirement");
            XElement re_req_refines = new XElement("Refines");
            XElement re_req_conflict = new XElement("Conflicts");
            XElement re_req_dublette = new XElement("Doublette");
            XElement re_req_requires = new XElement("Requires");
            XElement re_req_replaces = new XElement("Replaces");

            Add_Database_Connector(this.metamodel.m_Afo_Refines, re_req_refines);
            Add_Database_Connector(this.metamodel.m_Afo_Dublette, re_req_dublette);
            Add_Database_Connector(this.metamodel.m_Afo_Konflikt, re_req_conflict);
            Add_Database_Connector(this.metamodel.m_Afo_Replaces, re_req_replaces);
            Add_Database_Connector(this.metamodel.m_Afo_Requires, re_req_requires);

            re_req.Add(re_req_refines);
            re_req.Add(re_req_conflict);
            re_req.Add(re_req_dublette);
            re_req.Add(re_req_requires);
            re_req.Add(re_req_replaces);
            header.Add(re_req);
        }
        private void Add_InformationExchange(XElement header)
        {
            XElement derived = new XElement("Relation_InformationExchange");

            Add_Database_Connector(this.metamodel.m_Infoaus, derived);

            header.Add(derived);
        }
        private void Add_Behavior(XElement header)
        {
            XElement derived = new XElement("Relation_Behavior");

            Add_Database_Connector(this.metamodel.m_Behavior, derived);

            header.Add(derived);
        }
        private void Add_Behavior_Stakeholder(XElement header)
        {
            XElement derived = new XElement("Relation_Stakeholder");

            Add_Database_Connector(this.metamodel.m_Stakeholder, derived);

            header.Add(derived);
        }
        private void Add_Taxonomy(XElement header)
        {
            XElement derived = new XElement("Relation_Taxonomy");
            XElement taxo_cap = new XElement("Fähigkeitsbaum");

            Add_Database_Connector(this.metamodel.m_Taxonomy_Capability, taxo_cap);

            derived.Add(taxo_cap);

            XElement taxo_comp = new XElement("Komposition");

            Add_Database_Connector(this.metamodel.m_Decomposition_Element, taxo_comp);

            derived.Add(taxo_comp);

            header.Add(derived);
        }
        private void Add_Satisfy(XElement header)
        {
            XElement satisfy = new XElement("Relation_Satisfy");

            XElement satisfy_Design = new XElement("Satisfy_DesignConstraint");
            Add_Database_Connector(this.metamodel.m_Satisfy_Design, satisfy_Design);
            satisfy.Add(satisfy_Design);

            XElement satisfy_Process = new XElement("Satisfy_ProcessConstraint");
            Add_Database_Connector(this.metamodel.m_Satisfy_Process, satisfy_Process);
            satisfy.Add(satisfy_Process);

            XElement satisfy_Umwelt = new XElement("Satisfy_UmweltConstraint");
            Add_Database_Connector(this.metamodel.m_Satisfy_Umwelt, satisfy_Umwelt);
            satisfy.Add(satisfy_Umwelt);

            header.Add(satisfy);
        }
        private void Add_Typvertreter_Con(XElement header)
        {
            XElement derived = new XElement("Relation_Typvertreter");

            Add_Database_Connector(this.metamodel.m_Con_Typvertreter, derived);

            header.Add(derived);
        }

        private void Add_BPMN_Con(XElement header)
        {
            XElement derived = new XElement("Relation_BPMN");

            Add_BPMN__Pool_Con(derived);

            header.Add(derived);
        }

        private void Add_BPMN__Pool_Con(XElement header)
        {
            XElement derived = new XElement("Relation_Pool_Representation");

            Add_Database_Connector(this.metamodel.m_Con_Pools_BPMN, derived);

            header.Add(derived);
        }
        #endregion Add Konnektoren

        #region Write Data in xml
        private void Add_xElement(object obj, XElement header)
        {
            XElement recent = new XElement(obj.GetType().Name);

            if (obj.GetType().Name == "Element_Metamodel")
            {
                Element_Metamodel recent_ele = obj as Element_Metamodel;

                XElement Type = new XElement("Type", recent_ele.Type);
                XElement Stereotype = new XElement("Stereotype", recent_ele.Stereotype);
                XElement Toolbox = new XElement("Toolbox", recent_ele.Toolbox);
                XElement DefaultName = new XElement("DefaultName", recent_ele.DefaultName);
                XElement XAC_Attribut = new XElement("XAC_Attribut", recent_ele.XAC_Attribut);

                recent.Add(Type);
                recent.Add(Stereotype);
                recent.Add(Toolbox);
                recent.Add(DefaultName);
                recent.Add(XAC_Attribut);
            }

            if (obj.GetType().Name == "Connector_Metamodel")
            {
                Connector_Metamodel recent_con = obj as Connector_Metamodel;

                switch(recent_con.Type)
                {
                    case "Composition":
                        recent_con.Type = "Aggregation";
                        recent_con.SubType = null;
                        break;
                    case "Aggregation":
                        recent_con.Type = "Aggregation";
                        recent_con.SubType = "Strong";
                        break;
                    default:
                        recent_con.SubType = null;
                        break;
                }

                XElement Type = new XElement("Type", recent_con.Type);
                XElement Stereotype = new XElement("Stereotype", recent_con.Stereotype);
                XElement Toolbox = new XElement("Toolbox", recent_con.Toolbox);
                XElement SubType = new XElement("SubType", recent_con.SubType);
                XElement XAC_Attribut = new XElement("XAC_Attribut", recent_con.XAC);

                recent.Add(Type);
                recent.Add(Stereotype);
                recent.Add(Toolbox);
                recent.Add(SubType);
                recent.Add(XAC_Attribut);
            }

            header.Add(recent);

        }
        private void Add_Database_Element(List<Element_Metamodel> m_elem, XElement header)
        {
            if (m_elem.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Add_xElement(m_elem[i1], header);

                    i1++;
                } while (i1 < m_elem.Count);
            }
        }
        private void Add_Database_Connector(List<Connector_Metamodel> m_elem, XElement header)
        {
            if (m_elem.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Add_xElement(m_elem[i1], header);

                    i1++;
                } while (i1 < m_elem.Count);
            }
        }
        #endregion Write Data in xml


        #region Savespace
        /// <summary>
        /// Abfrage des Speicherortes der xml Datei mit Hilfe eines Savefildialoges
        /// </summary>
        /// <returns></returns>
        private string Choose_Savespace()
        {
            string filename = "test";
            bool save = false;

            do
            {
                SaveFileDialog saveFileDialog_Save = new SaveFileDialog();
                saveFileDialog_Save.Filter = "Profil_RPI|*.xml";
                saveFileDialog_Save.Title = "Save an Profil_RPI File";
                saveFileDialog_Save.ShowDialog();

                if (saveFileDialog_Save.FileName != "")
                {
                    filename = saveFileDialog_Save.FileName;
                    save = true;
                }

            } while (save == false);

            return (filename);
        }
        #endregion
        #endregion Export
        #region Import
        public bool Import_Profil()
        {
            this.metamodel.Create_NAFv31();
            bool flag_import = false;
            //File wählen
            string filename = this.Choose_OpenFile();
            //File öffnen
            #region Einlesen File
            XElement xac_xml;
            if(filename != null && filename != "")
            {
                string xac_string = System.IO.File.ReadAllText(@filename);
                //MessageBox.Show(xac_string.Length.ToString());
                //String zu XElement konvertieren
                xac_xml = XElement.Parse(xac_string);
                #endregion Einlesen File
                //Prüfen, ob Profil_RPI File
                if (xac_xml.Name.ToString() == "Profil_RPI")
                {
                    flag_import = true;
                }
            }
            //Importieren
            if(flag_import == true)
            {
                string xac_string = System.IO.File.ReadAllText(@filename);
                xac_xml = XElement.Parse(xac_string);
                #region Import Elemente
                //Elements erhalten
                var m_Elements = from c in xac_xml.Descendants("Elements")
                                        where (string)c.Name.ToString() == "Elements"
                                        select c;

                XElement elements = m_Elements.ToList()[0] as XElement;
                //Import der Elemente
                Import_Szenar(elements);
                Import_Decomposition(elements);
                Import_Requirements(elements);
                Import_InformationItems(elements);
                Import_Aktivity(elements);
                Import_Stakeholder(elements);
                Import_Fähigkeitsbaum(elements);
                Import_Constraints(elements);
                Import_Typvertreter(elements);
                #endregion Import Elemente

                #region Import Connectoren
                //Connectors erhalten
                var m_Connectors = from c in xac_xml.Descendants("Connectors")
                                 where (string)c.Name.ToString() == "Connectors"
                                 select c;

                XElement connectors = m_Connectors.ToList()[0] as XElement;
                //Import der Konnektoren
                Import_Relation_Derived(connectors);
                Import_Relation_Requiremnt(connectors);
                Import_Informationsaustausch(connectors);
                Import_Behavior(connectors);
                Import_Behavior_Stakeholder(connectors);
                Import_Relation_Taxonomy(connectors);
                Import_Relation_Satisfy(connectors);
                Import_Relation_Typvertreter(connectors);
                #endregion Import Connectoren

                MessageBox.Show("Import Profil_RPI abgeschlossen");

                return (true);
            }

            return (false);

        }




        #region Choose File
        private string Choose_OpenFile()
        {
            string filename = "";
            bool save = false;

            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "c:\\";
            open.Filter = "Profil_RPI|*.xml";
            open.Title = "Open an Profil_RPI File";
            open.ShowDialog();

            if (open.FileName != "")
            {
                filename = open.FileName;
                save = true;
            }

            return (filename);
        }
        #endregion Choose File

        #region Import Elemente
        private void Import_Szenar(XElement elements)
        {
            List<Element_Metamodel> m_Szenar = new List<Element_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Element_Logical = from d in elements.Descendants("Element_Logical")
                                      where (string)d.Name.ToString() == "Element_Logical"
                                    select d;

            XElement element_Logical = m_Element_Logical.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Element_Metamodel = from d in element_Logical.Descendants("Element_Metamodel")
                                    where (string)d.Name.ToString() == "Element_Metamodel"
                                      select d;

            foreach(var i1 in m_Element_Metamodel)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Szenar.Add(recent);
            }

            if(m_Szenar.Count > 0)
            {
                this.metamodel.m_Szenar = null;
                this.metamodel.m_Szenar = m_Szenar;
            }
            else
            {
                MessageBox.Show("Es sind keine Szenare im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Decomposition(XElement elements)
        {
            List<Element_Metamodel> m_Definition = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Usage = new List<Element_Metamodel>();
            //Element_Decomposition erhalten --> ist einzigartig
            var m_Elemens_Decomposition = from d in elements.Descendants("Element_Decomposition")
                                    where (string)d.Name.ToString() == "Element_Decomposition"
                                    select d;
            //Alle pair erhalten
            XElement element_decomposition = m_Elemens_Decomposition.ToList()[0] as XElement;
            var m_pair = from d in element_decomposition.Descendants("pair")
                         where (string)d.Name.ToString() == "pair"
                         select d;
            foreach(var i1 in m_pair)
            {
                //Definition erhalten
                XElement def_x = i1.Descendants("Definition").ToList()[0] as XElement; //einzigartig
                XElement def_elem = def_x.Descendants("Element_Metamodel").ToList()[0] as XElement;
                Element_Metamodel recent_def = Read_Element_Metamodel(def_elem); //einzigartig
                m_Definition.Add(recent_def);
                //Usage erhalten
                XElement usage_x = i1.Descendants("Usage").ToList()[0] as XElement; //einzigartig
                XElement usage_elem = usage_x.Descendants("Element_Metamodel").ToList()[0] as XElement;
                Element_Metamodel recent_usage = Read_Element_Metamodel(usage_elem); //einzigartig
                m_Usage.Add(recent_usage);
            }
              
            if (m_Definition.Count > 0 && m_Usage.Count > 0 && m_Definition.Count == m_Usage.Count)
            {
                this.metamodel.m_Elements_Definition = null;
                this.metamodel.m_Elements_Definition = m_Definition;
                this.metamodel.m_Elements_Usage = null;
                this.metamodel.m_Elements_Usage = m_Usage;
            }
            else
            {
                MessageBox.Show("Die Anzahl der Element_Definition und Element_Usage Element_Decomposition ist nicht korrekt.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Requirements(XElement elements)
        {
            List<Element_Metamodel> m_Functional = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Ínterface = new List<Element_Metamodel>();
            List<Element_Metamodel> m_User = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Design = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Process = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Environment = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Typvertreter = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Quality = new List<Element_Metamodel>();
            List<Element_Metamodel> m_NonFunctional = new List<Element_Metamodel>();

            //Element_Requirement erhalten --> ist einzigartig
            var m_element_req = from d in elements.Descendants("Element_Requirement")
                                    where (string)d.Name.ToString() == "Element_Requirement"
                                    select d;

            XElement element_req = m_element_req.ToList()[0] as XElement;
            //Requirement_Functional erhalten 
            #region Requirement Functional
            XElement req_func = element_req.Descendants("Requirement_Functional").ToList()[0] as XElement;
            var m_func_elem= from d in req_func.Descendants("Element_Metamodel")
                             where (string)d.Name.ToString() == "Element_Metamodel"
                             select d;

            foreach(var i1 in m_func_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Functional.Add(recent);
            }
            if(m_Functional.Count > 0)
            {
                this.metamodel.m_Requirement_Functional = null;
                this.metamodel.m_Requirement_Functional = m_Functional;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_Functional im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Requirement Functional
            //Requirement_Interface
            #region Interface
            XElement req_inter = element_req.Descendants("Requirement_Interface").ToList()[0] as XElement;
            var m_inter_elem = from d in req_inter.Descendants("Element_Metamodel")
                              where (string)d.Name.ToString() == "Element_Metamodel"
                              select d;

            foreach (var i1 in m_inter_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Ínterface.Add(recent);
            }
            if (m_Ínterface.Count > 0)
            {
                this.metamodel.m_Requirement_Interface = null;
                this.metamodel.m_Requirement_Interface = m_Ínterface;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_Interface im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Interface
            //Requirement_User
            #region User
            XElement req_user = element_req.Descendants("Requirement_User").ToList()[0] as XElement;
            var m_user_elem = from d in req_user.Descendants("Element_Metamodel")
                               where (string)d.Name.ToString() == "Element_Metamodel"
                               select d;

            foreach (var i1 in m_user_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_User.Add(recent);
            }
            if (m_User.Count > 0)
            {
                this.metamodel.m_Requirement_User = null;
                this.metamodel.m_Requirement_User = m_User;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_User im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion User
            //Requirement_Design
            #region Design
            XElement req_des = element_req.Descendants("Requirement_Design").ToList()[0] as XElement;
            var m_des_elem = from d in req_des.Descendants("Element_Metamodel")
                              where (string)d.Name.ToString() == "Element_Metamodel"
                              select d;

            foreach (var i1 in m_des_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Design.Add(recent);
            }
            if (m_Design.Count > 0)
            {
                this.metamodel.m_Requirement_Design = null;
                this.metamodel.m_Requirement_Design = m_Design;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_Design im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Design
            // Requirement Process
            #region Process
            XElement req_pro = element_req.Descendants("Requirement_Process").ToList()[0] as XElement;
            var m_pro_elem = from d in req_pro.Descendants("Element_Metamodel")
                             where (string)d.Name.ToString() == "Element_Metamodel"
                             select d;

            foreach (var i1 in m_pro_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Process.Add(recent);
            }
            if (m_Process.Count > 0)
            {
                this.metamodel.m_Requirement_Process = null;
                this.metamodel.m_Requirement_Process = m_Process;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_Process im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Process
            //Requirement Umwelt
            #region Umwelt
            XElement req_env = element_req.Descendants("Requirement_Umwelt").ToList()[0] as XElement;
            var m_env_elem = from d in req_env.Descendants("Element_Metamodel")
                             where (string)d.Name.ToString() == "Element_Metamodel"
                             select d;

            foreach (var i1 in m_env_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Environment.Add(recent);
            }
            if (m_Process.Count > 0)
            {
                this.metamodel.m_Requirement_Environment = null;
                this.metamodel.m_Requirement_Environment = m_Environment;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_Umwelt im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Umwelt
            //REquirement Typvertreter
            #region Typvertreter
            XElement req_typ = element_req.Descendants("Requirement_Typvertreter").ToList()[0] as XElement;
            var m_tpy_elem = from d in req_typ.Descendants("Element_Metamodel")
                             where (string)d.Name.ToString() == "Element_Metamodel"
                             select d;

            foreach (var i1 in m_tpy_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Typvertreter.Add(recent);
            }
            if (m_Process.Count > 0)
            {
                this.metamodel.m_Requirement_Typvertreter = null;
                this.metamodel.m_Requirement_Typvertreter = m_Typvertreter;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_Typvertreter im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Typvertreter
            //Requirement Quality
            #region Quality
            XElement req_qual = element_req.Descendants("Requirement_Quality").ToList()[0] as XElement;
            var m_qual_elem = from d in req_qual.Descendants("Element_Metamodel")
                             where (string)d.Name.ToString() == "Element_Metamodel"
                             select d;

            foreach (var i1 in m_qual_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Quality.Add(recent);
            }
            if (m_Process.Count > 0)
            {
                this.metamodel.m_Requirement_Quality_Class = null;
                this.metamodel.m_Requirement_Quality_Class = m_Quality;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_Quality im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Quality
            //NonFunctional
            XElement req_nonf = element_req.Descendants("Requirement_NonFunctional").ToList()[0] as XElement;
            var m_nonf_elem = from d in req_nonf.Descendants("Element_Metamodel")
                              where (string)d.Name.ToString() == "Element_Metamodel"
                              select d;

            foreach (var i1 in m_nonf_elem)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_NonFunctional.Add(recent);
            }
            if (m_NonFunctional.Count > 0)
            {
                this.metamodel.m_Requirement_NonFunctional = null;
                this.metamodel.m_Requirement_NonFunctional = m_NonFunctional;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Requirement_NonFunctional im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }


            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_Functional);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_Interface);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_User);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_Design);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_Process);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_Environment);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_Typvertreter);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_Quality_Class);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_Quality_Activity);
            this.metamodel.m_Requirement.AddRange(this.metamodel.m_Requirement_NonFunctional);

        }
        private void Import_InformationItems(XElement elements)
        {
            List<Element_Metamodel> m_InformationItem = new List<Element_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Element_Info = from d in elements.Descendants("Element_InformationItem")
                                    where (string)d.Name.ToString() == "Element_InformationItem"
                                    select d;

            XElement element_info = m_Element_Info.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Element_Metamodel = from d in element_info.Descendants("Element_Metamodel")
                                      where (string)d.Name.ToString() == "Element_Metamodel"
                                      select d;

            foreach (var i1 in m_Element_Metamodel)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_InformationItem.Add(recent);
            }

            if (m_InformationItem.Count > 0)
            {
                this.metamodel.m_InformationItem = null;
                this.metamodel.m_InformationItem = m_InformationItem;
            }
            else
            {
                MessageBox.Show("Es ist kein InformationItem im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Aktivity(XElement elements)
        {
            List<Element_Metamodel> m_Definition = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Usage = new List<Element_Metamodel>();
            //Element_Decomposition erhalten --> ist einzigartig
            var m_Elemens_Decomposition = from d in elements.Descendants("Element_Aktivity")
                                          where (string)d.Name.ToString() == "Element_Aktivity"
                                          select d;
            //Alle pair erhalten
            XElement element_decomposition = m_Elemens_Decomposition.ToList()[0] as XElement;
            var m_pair = from d in element_decomposition.Descendants("pair")
                         where (string)d.Name.ToString() == "pair"
                         select d;
            foreach (var i1 in m_pair)
            {
                //Definition erhalten
                XElement def_x = i1.Descendants("Definition").ToList()[0] as XElement; //einzigartig
                XElement def_elem = def_x.Descendants("Element_Metamodel").ToList()[0] as XElement;
                Element_Metamodel recent_def = Read_Element_Metamodel(def_elem); //einzigartig
                m_Definition.Add(recent_def);
                //Usage erhalten
                XElement usage_x = i1.Descendants("Usage").ToList()[0] as XElement; //einzigartig
                XElement usage_elem = usage_x.Descendants("Element_Metamodel").ToList()[0] as XElement;
                Element_Metamodel recent_usage = Read_Element_Metamodel(usage_elem); //einzigartig
                m_Usage.Add(recent_usage);
            }

            if (m_Definition.Count > 0 && m_Usage.Count > 0 && m_Definition.Count == m_Usage.Count)
            {
                this.metamodel.m_Aktivity_Definition = null;
                this.metamodel.m_Aktivity_Definition = m_Definition;
                this.metamodel.m_Aktivity_Usage = null;
                this.metamodel.m_Aktivity_Usage = m_Usage;
            }
            else
            {
                MessageBox.Show("Die Anzahl der Element_Definition und Element_Usage bei Element_Aktivity ist nicht korrekt.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Stakeholder(XElement elements)
        {
            List<Element_Metamodel> m_Definition = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Usage = new List<Element_Metamodel>();
            //Element_Decomposition erhalten --> ist einzigartig
            var m_Elemens_Decomposition = from d in elements.Descendants("Element_Stakeholder")
                                          where (string)d.Name.ToString() == "Element_Stakeholder"
                                          select d;
            //Alle pair erhalten
            XElement element_decomposition = m_Elemens_Decomposition.ToList()[0] as XElement;
            var m_pair = from d in element_decomposition.Descendants("pair")
                         where (string)d.Name.ToString() == "pair"
                         select d;
            foreach (var i1 in m_pair)
            {
                //Definition erhalten
                XElement def_x = i1.Descendants("Definition").ToList()[0] as XElement; //einzigartig
                XElement def_elem = def_x.Descendants("Element_Metamodel").ToList()[0] as XElement;
                Element_Metamodel recent_def = Read_Element_Metamodel(def_elem); //einzigartig
                m_Definition.Add(recent_def);
                //Usage erhalten
                XElement usage_x = i1.Descendants("Usage").ToList()[0] as XElement; //einzigartig
                XElement usage_elem = usage_x.Descendants("Element_Metamodel").ToList()[0] as XElement;
                Element_Metamodel recent_usage = Read_Element_Metamodel(usage_elem); //einzigartig
                m_Usage.Add(recent_usage);
            }

            if (m_Definition.Count > 0 && m_Usage.Count > 0 && m_Definition.Count == m_Usage.Count)
            {
                this.metamodel.m_Stakeholder_Definition = null;
                this.metamodel.m_Stakeholder_Definition = m_Definition;
                this.metamodel.m_Stakeholder_Usage = null;
                this.metamodel.m_Stakeholder_Usage = m_Usage;
            }
            else
            {
                MessageBox.Show("Die Anzahl der Element_Definition und Element_Usage Element_Stakeholder ist nicht korrekt.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Fähigkeitsbaum(XElement elements)
        {
            List<Element_Metamodel> m_Fähigkeitsbaum = new List<Element_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Element_Info = from d in elements.Descendants("Element_Fähigkeitsbaum")
                                 where (string)d.Name.ToString() == "Element_Fähigkeitsbaum"
                                 select d;

            XElement element_info = m_Element_Info.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Element_Metamodel = from d in element_info.Descendants("Element_Metamodel")
                                      where (string)d.Name.ToString() == "Element_Metamodel"
                                      select d;

            foreach (var i1 in m_Element_Metamodel)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Fähigkeitsbaum.Add(recent);
            }

            if (m_Fähigkeitsbaum.Count > 0)
            {
                this.metamodel.m_Capability = null;
                this.metamodel.m_Capability = m_Fähigkeitsbaum;
            }
            else
            {
                MessageBox.Show("Es ist kein Element für den Fähigkeitsbuam im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Constraints(XElement elements)
        {
            List<Element_Metamodel> m_Design = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Process = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Umwelt = new List<Element_Metamodel>();
            //Element_Constraint erhalten --> ist einzigartig
            var m_element_con = from d in elements.Descendants("Element_Constraint")
                                where (string)d.Name.ToString() == "Element_Constraint"
                                select d;

            XElement element_con = m_element_con.ToList()[0] as XElement;
            //DesignConstraint erhalten 
            #region DesignConstraint
            XElement con_des = element_con.Descendants("Element_DesignConstraint").ToList()[0] as XElement;
            var m_con_des = from d in con_des.Descendants("Element_Metamodel")
                              where (string)d.Name.ToString() == "Element_Metamodel"
                              select d;

            foreach (var i1 in m_con_des)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Design.Add(recent);
            }
            if (m_Design.Count > 0)
            {
                this.metamodel.m_Design_Constraint = null;
                this.metamodel.m_Design_Constraint = m_Design;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für DesignConstraint im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion DesignConstraint
            //ProcessConstraint
            #region ProcessConstraint
            XElement con_pro = m_element_con.Descendants("Element_ProcessConstraint").ToList()[0] as XElement;
            var m_con_pro = from d in con_pro.Descendants("Element_Metamodel")
                               where (string)d.Name.ToString() == "Element_Metamodel"
                               select d;

            foreach (var i1 in m_con_pro)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Process.Add(recent);
            }
            if (m_Process.Count > 0)
            {
                this.metamodel.m_Process_Constraint = null;
                this.metamodel.m_Process_Constraint = m_Process;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für ProcessConstraint im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion ProcessConstraint
            //ProcessConstraint
            #region UmweltConstraint
            XElement con_umw = m_element_con.Descendants("Element_UmweltConstraint").ToList()[0] as XElement;
            var m_con_umw = from d in con_umw.Descendants("Element_Metamodel")
                            where (string)d.Name.ToString() == "Element_Metamodel"
                            select d;

            foreach (var i1 in m_con_umw)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Umwelt.Add(recent);
            }
            if (m_Umwelt.Count > 0)
            {
                this.metamodel.m_Constraint_Umwelt = null;
                this.metamodel.m_Constraint_Umwelt = m_Umwelt;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für UmweltConstraint im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion UmweltConstraint
        }
        private void Import_Typvertreter(XElement elements)
        {
            List<Element_Metamodel> m_Definition = new List<Element_Metamodel>();
            List<Element_Metamodel> m_Usage = new List<Element_Metamodel>();
            //Element_Decomposition erhalten --> ist einzigartig
            var m_Elemens_Decomposition = from d in elements.Descendants("Element_Typvertreter")
                                          where (string)d.Name.ToString() == "Element_Typvertreter"
                                          select d;
            //Alle pair erhalten
            XElement element_decomposition = m_Elemens_Decomposition.ToList()[0] as XElement;
            var m_pair = from d in element_decomposition.Descendants("pair")
                         where (string)d.Name.ToString() == "pair"
                         select d;
            foreach (var i1 in m_pair)
            {
                //Definition erhalten
                XElement def_x = i1.Descendants("Definition").ToList()[0] as XElement; //einzigartig
                XElement def_elem = def_x.Descendants("Element_Metamodel").ToList()[0] as XElement;
                Element_Metamodel recent_def = Read_Element_Metamodel(def_elem); //einzigartig
                m_Definition.Add(recent_def);
                //Usage erhalten
                XElement usage_x = i1.Descendants("Usage").ToList()[0] as XElement; //einzigartig
                XElement usage_elem = usage_x.Descendants("Element_Metamodel").ToList()[0] as XElement;
                Element_Metamodel recent_usage = Read_Element_Metamodel(usage_elem); //einzigartig
                m_Usage.Add(recent_usage);
            }

            if (m_Definition.Count > 0 && m_Usage.Count > 0 && m_Definition.Count == m_Usage.Count)
            {
                this.metamodel.m_Typvertreter_Definition = null;
                this.metamodel.m_Typvertreter_Definition = m_Definition;
                this.metamodel.m_Typvertreter_Usage = null;
                this.metamodel.m_Typvertreter_Usage = m_Usage;
            }
            else
            {
                MessageBox.Show("Die Anzahl der Element_Definition und Element_Usage Element_Typvertreter ist nicht korrekt.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_BPMN(XElement elements)
        {
            Import_Pool_BPMN(elements);
            Import_Lane_BPMN(elements);
        }

        private void Import_Pool_BPMN(XElement elements)
        {
            //Pool
            List<Element_Metamodel> m_Pool = new List<Element_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Element_Info = from d in elements.Descendants("Element_Pool_BPMN")
                                 where (string)d.Name.ToString() == "Element_Pool_BPMN"
                                 select d;

            XElement element_info = m_Element_Info.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Element_Metamodel = from d in element_info.Descendants("Element_Metamodel")
                                      where (string)d.Name.ToString() == "Element_Metamodel"
                                      select d;

            foreach (var i1 in m_Element_Metamodel)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Pool.Add(recent);
            }

            if (m_Pool.Count > 0)
            {
                this.metamodel.m_Pools_BPMN = null;
                this.metamodel.m_Pools_BPMN = m_Pool;
            }
            else
            {
                MessageBox.Show("Es ist kein Element für die Pools BPMN im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }

        private void Import_Lane_BPMN(XElement elements)
        {
            //Pool
            List<Element_Metamodel> m_Lane = new List<Element_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Element_Info = from d in elements.Descendants("Element_Lane_BPMN")
                                 where (string)d.Name.ToString() == "Element_Lane_BPMN"
                                 select d;

            XElement element_info = m_Element_Info.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Element_Metamodel = from d in element_info.Descendants("Element_Metamodel")
                                      where (string)d.Name.ToString() == "Element_Metamodel"
                                      select d;

            foreach (var i1 in m_Element_Metamodel)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Lane.Add(recent);
            }

            if (m_Lane.Count > 0)
            {
                this.metamodel.m_Lanes_BPMN = null;
                this.metamodel.m_Lanes_BPMN = m_Lane;
            }
            else
            {
                MessageBox.Show("Es ist kein Element für die Lanes BPMN im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }

        private void Import_Activity_BPMN(XElement elements)
        {
            //Pool
            List<Element_Metamodel> m_Activity = new List<Element_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Element_Info = from d in elements.Descendants("Element_Activity_BPMN")
                                 where (string)d.Name.ToString() == "Element_Activity_BPMN"
                                 select d;

            XElement element_info = m_Element_Info.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Element_Metamodel = from d in element_info.Descendants("Element_Metamodel")
                                      where (string)d.Name.ToString() == "Element_Metamodel"
                                      select d;

            foreach (var i1 in m_Element_Metamodel)
            {
                Element_Metamodel recent = Read_Element_Metamodel(i1);
                m_Activity.Add(recent);
            }

            if (m_Activity.Count > 0)
            {
                this.metamodel.m_Aktivity_Definition_BPMN = null;
                this.metamodel.m_Aktivity_Definition_BPMN = m_Activity;
            }
            else
            {
                MessageBox.Show("Es ist kein Element für die Activity BPMN im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }

        private Element_Metamodel Read_Element_Metamodel(XElement recent)
        {
            Element_Metamodel ret = new Element_Metamodel(null, null, null, null, null);

            if(recent.Name.ToString() == "Element_Metamodel")
            {
                var Type = recent.Descendants("Type"); //einzigartig
                ret.Type = Type.ToList()[0].Value.ToString();
                var Stereotype = recent.Descendants("Stereotype"); //einzigartig
                ret.Stereotype = Stereotype.ToList()[0].Value.ToString();
                var Toolbox = recent.Descendants("Toolbox"); //einzigartig
                ret.Toolbox = Toolbox.ToList()[0].Value.ToString();
                var DefaultName = recent.Descendants("DefaultName"); //einzigartig
                ret.DefaultName = DefaultName.ToList()[0].Value.ToString();
                var XAC_Attribut = recent.Descendants("XAC_Attribut"); //einzigartig
                ret.XAC_Attribut = XAC_Attribut.ToList()[0].Value.ToString();
            }
            else
            {
                ret = null;
            }

            return (ret);

        }

        #endregion Import Elemente

        #region Import Connectoren
        private void Import_Relation_Derived(XElement connectors)
        {
            List<Connector_Metamodel> m_Element = new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Logical = new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Capability = new List<Connector_Metamodel>();
            //Relation_Derived erhalten --> ist einzigartig
            var m_rel_der = from d in connectors.Descendants("Relation_Derived")
                                where (string)d.Name.ToString() == "Relation_Derived"
                            select d;

            XElement rel_der = m_rel_der.ToList()[0] as XElement;
            //Element erhalten 
            XElement element = rel_der.Descendants("Element").ToList()[0] as XElement;
            var m_elem_con = from d in element.Descendants("Connector_Metamodel")
                              where (string)d.Name.ToString() == "Connector_Metamodel"
                              select d;

            foreach (var i1 in m_elem_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Element.Add(recent);
            }
            if (m_Element.Count > 0)
            {
                this.metamodel.m_Derived_Element = null;
                this.metamodel.m_Derived_Element = m_Element;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Derived_Element im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            //Logicalerhalten
            XElement logical = rel_der.Descendants("Logical").ToList()[0] as XElement;
            var m_log_con = from d in logical.Descendants("Connector_Metamodel")
                               where (string)d.Name.ToString() == "Connector_Metamodel"
                               select d;

            foreach (var i1 in m_log_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Logical.Add(recent);
            }
            if (m_Logical.Count > 0)
            {
                this.metamodel.m_Derived_Logical = null;
                this.metamodel.m_Derived_Logical = m_Logical;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Derived_Logical im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            //Requirement_User
            XElement capability = rel_der.Descendants("Capability").ToList()[0] as XElement;
            var m_cap_con = from d in capability.Descendants("Connector_Metamodel")
                              where (string)d.Name.ToString() == "Connector_Metamodel"
                              select d;

            foreach (var i1 in m_cap_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Capability.Add(recent);
            }
            if (m_Capability.Count > 0)
            {
                this.metamodel.m_Derived_Capability = null;
                this.metamodel.m_Derived_Capability = m_Capability;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Derived_Capability im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Relation_Requiremnt(XElement connectors)
        {
            List<Connector_Metamodel> m_Conflict = new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Dublette = new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Refines = new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Replaces = new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Requires = new List<Connector_Metamodel>();
            //Relation_Derived erhalten --> ist einzigartig
            var m_rel_der = from d in connectors.Descendants("Relation_Requirement")
                            where (string)d.Name.ToString() == "Relation_Requirement"
                            select d;

            XElement rel_der = m_rel_der.ToList()[0] as XElement;
            //Refines erhalten 
            XElement refines = rel_der.Descendants("Refines").ToList()[0] as XElement;
            var m_refines_con = from d in refines.Descendants("Connector_Metamodel")
                             where (string)d.Name.ToString() == "Connector_Metamodel"
                             select d;

            foreach (var i1 in m_refines_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Refines.Add(recent);
            }
            if (m_Refines.Count > 0)
            {
                this.metamodel.m_Afo_Refines = null;
                this.metamodel.m_Afo_Refines = m_Refines;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation_Requirement Refines im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            //Conflict erhalten
            XElement conflict = rel_der.Descendants("Conflicts").ToList()[0] as XElement;
            var m_conflicts_con = from d in conflict.Descendants("Connector_Metamodel")
                            where (string)d.Name.ToString() == "Connector_Metamodel"
                            select d;

            foreach (var i1 in m_conflicts_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Conflict.Add(recent);
            }
            if (m_Conflict.Count > 0)
            {
                this.metamodel.m_Afo_Konflikt = null;
                this.metamodel.m_Afo_Konflikt = m_Conflict;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation_Requirement Conflicts im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            //Dublette
            XElement doublette = rel_der.Descendants("Doublette").ToList()[0] as XElement;
            var m_dob_con = from d in doublette.Descendants("Connector_Metamodel")
                            where (string)d.Name.ToString() == "Connector_Metamodel"
                            select d;

            foreach (var i1 in m_dob_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Dublette.Add(recent);
            }
            if (m_Dublette.Count > 0)
            {
                this.metamodel.m_Afo_Dublette = null;
                this.metamodel.m_Afo_Dublette = m_Dublette;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation_Requirement Doublette im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            //Requires
            XElement Requires = rel_der.Descendants("Requires").ToList()[0] as XElement;
            var m_requires_con = from d in doublette.Descendants("Connector_Metamodel")
                            where (string)d.Name.ToString() == "Connector_Metamodel"
                            select d;

            foreach (var i1 in m_requires_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Requires.Add(recent);
            }
            if (m_Requires.Count > 0)
            {
                this.metamodel.m_Afo_Requires = null;
                this.metamodel.m_Afo_Requires = m_Dublette;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation_Requirement Requires im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            //Replaces
            XElement Replaces = rel_der.Descendants("Replaces").ToList()[0] as XElement;
            var m_replaces_con = from d in doublette.Descendants("Connector_Metamodel")
                                 where (string)d.Name.ToString() == "Connector_Metamodel"
                                 select d;

            foreach (var i1 in m_replaces_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Replaces.Add(recent);
            }
            if (m_Replaces.Count > 0)
            {
                this.metamodel.m_Afo_Replaces = null;
                this.metamodel.m_Afo_Replaces = m_Replaces;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation_Requirement Replaces im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Informationsaustausch(XElement connectors)
        {
            List<Connector_Metamodel> m_Info = new List<Connector_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Relation = from d in connectors.Descendants("Relation_InformationExchange")
                                    where (string)d.Name.ToString() == "Relation_InformationExchange"
                                    select d;

            XElement relation = m_Relation.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Connector_Metamodel = from d in relation.Descendants("Connector_Metamodel")
                                      where (string)d.Name.ToString() == "Connector_Metamodel"
                                        select d;

            foreach (var i1 in m_Connector_Metamodel)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Info.Add(recent);
            }

            if (m_Info.Count > 0)
            {
                this.metamodel.m_Infoaus = null;
                this.metamodel.m_Infoaus = m_Info;
            }
            else
            {
                MessageBox.Show("Es sind keine Relation_InformationExchange im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Behavior(XElement connectors)
        {
            List<Connector_Metamodel> m_Behavior = new List<Connector_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Relation = from d in connectors.Descendants("Relation_Behavior")
                             where (string)d.Name.ToString() == "Relation_Behavior"
                             select d;

            XElement relation = m_Relation.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Connector_Metamodel = from d in relation.Descendants("Connector_Metamodel")
                                        where (string)d.Name.ToString() == "Connector_Metamodel"
                                        select d;

            foreach (var i1 in m_Connector_Metamodel)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Behavior.Add(recent);
            }

            if (m_Behavior.Count > 0)
            {
                this.metamodel.m_Behavior = null;
                this.metamodel.m_Behavior = m_Behavior;
            }
            else
            {
                MessageBox.Show("Es sind keine Relation_Behavior im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Behavior_Stakeholder(XElement connectors)
        {
            List<Connector_Metamodel> m_Behavior = new List<Connector_Metamodel>();
            //Element_Logical erhalten --> ist einzigartig
            var m_Relation = from d in connectors.Descendants("Relation_Stakeholder")
                             where (string)d.Name.ToString() == "Relation_Stakeholder"
                             select d;

            XElement relation = m_Relation.ToList()[0] as XElement;
            //Element_Metamodel erhalten 
            var m_Connector_Metamodel = from d in relation.Descendants("Connector_Metamodel")
                                        where (string)d.Name.ToString() == "Connector_Metamodel"
                                        select d;

            foreach (var i1 in m_Connector_Metamodel)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Behavior.Add(recent);
            }

            if (m_Behavior.Count > 0)
            {
                this.metamodel.m_Stakeholder = null;
                this.metamodel.m_Stakeholder = m_Behavior;
            }
            else
            {
                MessageBox.Show("Es sind keine Relation_Stakeholder im Profil_RPI definiert.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Relation_Taxonomy(XElement connectors)
        {
            List<Connector_Metamodel> m_Fähigkeitsbaum= new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Dekomposition = new List<Connector_Metamodel>();
            //Relation_Derived erhalten --> ist einzigartig
            var m_rel_der = from d in connectors.Descendants("Relation_Taxonomy")
                            where (string)d.Name.ToString() == "Relation_Taxonomy"
                            select d;
            //Fähigkeitsbaum
            XElement rel_der = m_rel_der.ToList()[0] as XElement;
            //Refines erhalten 
            XElement refines = rel_der.Descendants("Fähigkeitsbaum").ToList()[0] as XElement;
            var m_refines_con = from d in refines.Descendants("Connector_Metamodel")
                                where (string)d.Name.ToString() == "Connector_Metamodel"
                                select d;

            foreach (var i1 in m_refines_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Fähigkeitsbaum.Add(recent);
            }
            if (m_Fähigkeitsbaum.Count > 0)
            {
                this.metamodel.m_Taxonomy_Capability = null;
                this.metamodel.m_Taxonomy_Capability = m_Fähigkeitsbaum;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Taxonomy Fähigkeitsbaum im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            //Dekomposition
            XElement refines_dec = rel_der.Descendants("Komposition").ToList()[0] as XElement;
            var m_refines_con_dec = from d in refines_dec.Descendants("Connector_Metamodel")
                                where (string)d.Name.ToString() == "Connector_Metamodel"
                                select d;

            foreach (var i1 in m_refines_con_dec)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Dekomposition.Add(recent);
            }
            if (m_Dekomposition.Count > 0)
            {
                this.metamodel.m_Decomposition_Element = null;
                this.metamodel.m_Decomposition_Element = m_Dekomposition;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Taxonomy Komposition im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }
        private void Import_Relation_Satisfy(XElement connectors)
        {
            List<Connector_Metamodel> m_Design = new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Process = new List<Connector_Metamodel>();
            List<Connector_Metamodel> m_Umwelt = new List<Connector_Metamodel>();

            //Relation_Satisfy erhalten --> ist einzigartig
            var m_rel_sat = from d in connectors.Descendants("Relation_Satisfy")
                            where (string)d.Name.ToString() == "Relation_Satisfy"
                            select d;

            XElement rel_sat = m_rel_sat.ToList()[0] as XElement;
            //Satisfy_DesignConstraint erhalten 
            #region Design
            XElement sat_des = rel_sat.Descendants("Satisfy_DesignConstraint").ToList()[0] as XElement;
            var m_sat_des = from d in sat_des.Descendants("Connector_Metamodel")
                                where (string)d.Name.ToString() == "Connector_Metamodel"
                                select d;

            foreach (var i1 in m_sat_des)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Design.Add(recent);
            }
            if (m_Design.Count > 0)
            {
                this.metamodel.m_Satisfy_Design = null;
                this.metamodel.m_Satisfy_Design = m_Design;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation_Satisfy Satisfy_DesignConstraint im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Design
            //Satisfy_ProcessConstraint erhalten
            #region Process
            XElement sat_pro = m_rel_sat.Descendants("Satisfy_ProcessConstraint").ToList()[0] as XElement;
            var m_sat_pro = from d in sat_pro.Descendants("Connector_Metamodel")
                                  where (string)d.Name.ToString() == "Connector_Metamodel"
                                  select d;

            foreach (var i1 in m_sat_pro)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Process.Add(recent);
            }
            if (m_Process.Count > 0)
            {
                this.metamodel.m_Satisfy_Process = null;
                this.metamodel.m_Satisfy_Process = m_Process;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation_Requirement Satisfy_ProcessConstraint im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Process
            //Umwelt Satisfy
            #region Umwelt
            XElement sat_umw = m_rel_sat.Descendants("Satisfy_UmweltConstraint").ToList()[0] as XElement;
            var m_sat_umw = from d in sat_umw.Descendants("Connector_Metamodel")
                            where (string)d.Name.ToString() == "Connector_Metamodel"
                            select d;

            foreach (var i1 in m_sat_umw)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Umwelt.Add(recent);
            }
            if (m_Umwelt.Count > 0)
            {
                this.metamodel.m_Satisfy_Umwelt = null;
                this.metamodel.m_Satisfy_Umwelt = m_Umwelt;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation_Requirement Satisfy_UmweltConstraint im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
            #endregion Umwelt
        }
        private void Import_Relation_Typvertreter(XElement connectors)
        {
            List<Connector_Metamodel> m_Fähigkeitsbaum = new List<Connector_Metamodel>();
            //Relation_Derived erhalten --> ist einzigartig
            var m_rel_der = from d in connectors.Descendants("Relation_Typvertreter")
                            where (string)d.Name.ToString() == "Relation_Typvertreter"
                            select d;

            XElement rel_der = m_rel_der.ToList()[0] as XElement;
            //Refines erhalten 
            //XElement refines = rel_der.Descendants("Relation_Typvertreter").ToList()[0] as XElement;
            var m_refines_con = from d in rel_der.Descendants("Connector_Metamodel")
                                where (string)d.Name.ToString() == "Connector_Metamodel"
                                select d;

            foreach (var i1 in m_refines_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Fähigkeitsbaum.Add(recent);
            }
            if (m_Fähigkeitsbaum.Count > 0)
            {
                this.metamodel.m_Con_Typvertreter = null;
                this.metamodel.m_Con_Typvertreter = m_Fähigkeitsbaum;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation Typvertreter im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }

        private void Import_Relation_BPMN(XElement connectors)
        {
            List<Connector_Metamodel> m_Fähigkeitsbaum = new List<Connector_Metamodel>();
            //Relation_Derived erhalten --> ist einzigartig
            var m_rel_der2 = from d in connectors.Descendants("Relation_BPMN")
                            where (string)d.Name.ToString() == "Relation_BPMN"
                            select d;

            XElement rel_der2 = m_rel_der2.ToList()[0] as XElement;
            //Pool Represent<tion erhalten 
            //Relation_Derived erhalten --> ist einzigartig
            var m_rel_der = from d in rel_der2.Descendants("Relation_Pool_Representation")
                            where (string)d.Name.ToString() == "Relation_Pool_Representation"
                            select d;

            XElement rel_der = m_rel_der.ToList()[0] as XElement;


            //XElement refines = rel_der.Descendants("Relation_Typvertreter").ToList()[0] as XElement;
            var m_refines_con = from d in rel_der.Descendants("Connector_Metamodel")
                                where (string)d.Name.ToString() == "Connector_Metamodel"
                                select d;

            foreach (var i1 in m_refines_con)
            {
                Connector_Metamodel recent = Read_Connector_Metamodel(i1);
                m_Fähigkeitsbaum.Add(recent);
            }
            if (m_Fähigkeitsbaum.Count > 0)
            {
                this.metamodel.m_Con_Pools_BPMN = null;
                this.metamodel.m_Con_Pools_BPMN = m_Fähigkeitsbaum;
            }
            else
            {
                MessageBox.Show("Es ist keine Definition für Relation Pool Representation im Profil_RPI vorhanden.\nDas gewählte Metamodell wird vorausichtlich Fehler erzeugen.");
            }
        }

        private Connector_Metamodel Read_Connector_Metamodel(XElement recent)
        {
            Connector_Metamodel ret = new Connector_Metamodel(null, null, null, null, null, true);

            if (recent.Name.ToString() == "Connector_Metamodel")
            {
                var Type = recent.Descendants("Type"); //einzigartig
                ret.Type = Type.ToList()[0].Value.ToString();
                var Stereotype = recent.Descendants("Stereotype"); //einzigartig
                ret.Stereotype = Stereotype.ToList()[0].Value.ToString();
                var Toolbox = recent.Descendants("Toolbox"); //einzigartig
                ret.Toolbox = Toolbox.ToList()[0].Value.ToString();
                var SubType = recent.Descendants("SubType"); //einzigartig
                ret.SubType = SubType.ToList()[0].Value.ToString();
                var XAC = recent.Descendants("XAC_Attribut"); //einzigartig
                ret.XAC = XAC.ToList()[0].Value.ToString();
                ret.direction = true;
            }
            else
            {
                ret = null;
            }

            return (ret);

        }


        #endregion Import Connectoren
        #endregion Import


        #region Import_Repositoryies
        public List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> Import_Repository(string filename)
        {
            List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> m_ret = new List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC>();

          /* OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "c:\\";
            open.Filter = "Profil_RPI|*.xml";
            open.Title = "Open an Profil_RPI File";
            open.ShowDialog();

            if (open.FileName != "")
            {
                filename = open.FileName;
                //save = true;
            }*/

            if (File.Exists(filename))
            {
               

                //string xac_string = System.IO.File.ReadAllText(filename);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(@filename);


                 XmlNodeList m_nodes =  xmlDocument.ChildNodes;

                XmlNode xmlNode;

                if(m_nodes.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        xmlNode = m_nodes[i1];

                        if(xmlNode.Name == "repos")
                        {
                            List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> m_repo =  Get_Repos(m_nodes[i1]);

                            if(m_repo != null)
                            {
                                return (m_repo);
                            }
                        }

                        i1++;
                    } while (i1 < m_nodes.Count);
                }

            }

            if(m_ret.Count == 0)
            {
                m_ret = null;
            }




            return (m_ret);
        }

        private List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> Get_Repos(XmlNode Parent)
        {
            List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> Repos = new List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC>();

            if(Parent.ChildNodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    XmlNode xmlNode = Parent.ChildNodes[i1];

                    if(xmlNode.Name == "repo")
                    {
                        Requirement_Plugin.Database_Connection.Repository.Repository_ODBC help =  Get_Repo(Parent.ChildNodes[i1]);

                        if(help != null)
                        {
                            if(Repos.Contains(help) == false)
                            {
                                Repos.Add(help);
                            }
                        }
                    }

                    i1++;
                } while (i1 < Parent.ChildNodes.Count);
            }

            if(Repos.Count == 0)
            {
                Repos = null;
            }

            return (Repos);
        }


        private Requirement_Plugin.Database_Connection.Repository.Repository_ODBC Get_Repo(XmlNode xmlNode)
        {
            if(xmlNode.ChildNodes.Count > 0)
            {
                string db_name = "";
                string root_node = "";
                string con_string = "";
                string user = "";
                string database = "";
                string server = "";

                int i1 = 0;
                do
                {
                    if(xmlNode.ChildNodes[i1].Name == "db_name")
                    {
                        db_name = xmlNode.ChildNodes[i1].InnerText;
                    }
                    if (xmlNode.ChildNodes[i1].Name == "rootnode")
                    {
                        root_node = xmlNode.ChildNodes[i1].InnerText;
                    }
                    if (xmlNode.ChildNodes[i1].Name == "connection_string")
                    {
                        con_string = xmlNode.ChildNodes[i1].InnerText;
                    }
                    if (xmlNode.ChildNodes[i1].Name == "user")
                    {
                        user = xmlNode.ChildNodes[i1].InnerText;
                    }
                    if (xmlNode.ChildNodes[i1].Name == "database")
                    {
                        database = xmlNode.ChildNodes[i1].InnerText;
                    }
                    if (xmlNode.ChildNodes[i1].Name == "server")
                    {
                        server = xmlNode.ChildNodes[i1].InnerText;
                    }

                    i1++;
                } while (i1 < xmlNode.ChildNodes.Count);

                if(db_name != "" && root_node != "" && con_string != "" && user != "" && database != "" && server != "")
                {
                    Requirement_Plugin.Database_Connection.Repository.Repository_ODBC new_con = new Requirement_Plugin.Database_Connection.Repository.Repository_ODBC(db_name, root_node, con_string, user, database, server);

                    return (new_con);
                }

            }

            return (null);

        }

        #endregion
    }


}

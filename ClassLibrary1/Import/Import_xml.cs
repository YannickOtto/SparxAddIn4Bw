using Database_Connection;
using Ennumerationen;
using Forms;
using Metamodels;
using Repsoitory_Elements;
using Requirement_Plugin.Interfaces;
using Requirements;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Requirement_Plugin
{
    public class Import_xml
    {
        private List<string> m_Type_Requirement = new List<string>();
        private List<string> m_StereoType_Requirement = new List<string>();
        private List<string> m_GUID_Requirement = new List<string>();
        private List<string> m_GUID_Requirement_Update = new List<string>();
        private List<string> m_GUID_Requirement_NonUpdate = new List<string>();
        private List<string> m_GUID_Requirement_New = new List<string>();

        private List<string> m_GUID_Systemelement = new List<string>();
        private List<string> m_GUID_Systemelement_Update = new List<string>();
        private List<string> m_GUID_Systemelement_NonUpdate = new List<string>();
        private List<string> m_GUID_Systemelement_New = new List<string>();

        private List<string> m_GUID_Funktionsbaum = new List<string>();

        private List<string> m_GUID_FunktionsbaumAfo_Connector_Import = new List<string>();
       
        private List<string> m_GUID_SysAfo_Connector_Import = new List<string>();
       
        private List<string> m_GUID_LogicalAfo_Connector_Import = new List<string>();
        

        private List<string> m_GUID_FunktionsbaumAfo_Connector_Import_new = new List<string>();
        private List<string> m_GUID_SysAfo_Connector_Import_new = new List<string>();
        private List<string> m_GUID_LogicalAfo_Connector_Import_new = new List<string>();

        private List<string> m_GUID_Refines_Connector_Import = new List<string>();
        private List<string> m_GUID_Requires_Connector_Import = new List<string>();
        private List<string> m_GUID_Dublette_Connector_Import = new List<string>();
        private List<string> m_GUID_Conflicts_Connector_Import = new List<string>();
        private List<string> m_GUID_Replaces_Connector_Import = new List<string>();

        private List<string> m_GUID_Refines_Connector_Import_new = new List<string>();
        private List<string> m_GUID_Requires_Connector_Import_new = new List<string>();
        private List<string> m_GUID_Dublette_Connector_Import_new = new List<string>();
        private List<string> m_GUID_Conflicts_Connector_Import_new = new List<string>();
        private List<string> m_GUID_Replaces_Connector_Import_new = new List<string>();
        private List<string> m_GUID_InheritsFrom_Connector_Import_new = new List<string>();

        private List<string> m_GUID_System_Connector_Import = new List<string>();
        private List<string> m_GUID_System_Connector_Import_new = new List<string>();

        private List<string> m_GUID_Funktionsbaum_Connector_Import = new List<string>();
        private List<string> m_GUID_Funktionsbaum_Connector_Import_new = new List<string>();

        List<string> m_Afo_Capability_NoImport = new List<string>();
        List<string> m_Afo_Sys_NoImport = new List<string>();
        List<string> m_Afo_Logical_NoImport = new List<string>();
        List<string> m_Afo_Refines_NoImport = new List<string>();
        List<string> m_Afo_Requires_NoImport = new List<string>();
        List<string> m_Afo_Replaces_NoImport = new List<string>();
        List<string> m_Afo_Conflicts_NoImport = new List<string>();
        List<string> m_Afo_Dublette_NoImport = new List<string>();
        List<string> m_Taxonomy_Capability_NoImport = new List<string>();
        List<string> m_Taxonomy_System_NoImport = new List<string>();
        List<List<string>> m_No_Import = new List<List<string>>();

        


        string GUID_Issue; 
        string GUID_Klaerungspunkte;
        string GUID_Guid;
        string GUID_Konnektoren;
        string GUID_Konnektoren_Archiv;
        string GUID_Elemente;

        bool flag_delete = false;

        private Create_Metamodel_Class Metamodel_Base = new Create_Metamodel_Class();
        private List<string> m_GUID = new List<string>();

        public Import_xml(EA.Repository repository, Database Data)
        {
            flag_delete = false;
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();
            ////Variablen
            #region Var
            List<string> m_Type1 = Data.metamodel.m_Requirement_Functional.Select(x => x.Type).ToList();
            List<string> m_Type2 = Data.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList();
            List<string> m_Type3 = Data.metamodel.m_Requirement_User.Select(x => x.Type).ToList();
            List<string> m_Type4 = Data.metamodel.m_Requirement_Design.Select(x => x.Type).ToList();
            List<string> m_Type5 = Data.metamodel.m_Requirement_Process.Select(x => x.Type).ToList();
            List<string> m_StereoType1 = Data.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList();
            List<string> m_StereoType2 = Data.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList();
            List<string> m_StereoType3 = Data.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList();
            List<string> m_StereoType4 = Data.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList();
            List<string> m_StereoType5 = Data.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList();

            if (m_Type2.Count > 0)
            {
                m_Type1.AddRange(m_Type2);
            }
            if (m_Type3.Count > 0)
            {
                m_Type1.AddRange(m_Type3);
            }
            if (m_Type4.Count > 0)
            {
                m_Type1.AddRange(m_Type4);
            }
            if (m_Type5.Count > 0)
            {
                m_Type1.AddRange(m_Type5);
            }

            if (m_StereoType2.Count > 0)
            {
                m_StereoType1.AddRange(m_StereoType2);
            }
            if (m_StereoType3.Count > 0)
            {
                m_StereoType1.AddRange(m_StereoType3);
            }
            if (m_StereoType4.Count > 0)
            {
                m_StereoType1.AddRange(m_StereoType4);
            }
            if (m_StereoType5.Count > 0)
            {
                m_StereoType1.AddRange(m_StereoType5);
            }

            this.m_Type_Requirement = m_Type1;
            this.m_StereoType_Requirement = m_StereoType1;
            #endregion Var

            Repository_Element repository_Element = new Repository_Element();
            #region FileDialog
            ///Welche Datei soll importiert werden?
            string filename = Choose_OpenFile();
            #endregion FileDialog
            //MessageBox.Show("Name: " +filename);
            //Es wurde eine Datei ausgewählt
            if(filename != "")
            {
                #region Import Package anlegen
                ///PAckages anlegen
                string Package_Import_GUID = repository_Element.Create_Package_Model("Import - XAC", repository, Data);
                EA.Package Package_Import = repository.GetPackageByGuid(Package_Import_GUID);

                this.GUID_Issue  = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, Data);
                EA.Package help = repository.GetPackageByGuid(GUID_Issue);
                this.GUID_Klaerungspunkte = repository_Element.Create_Package_Model("Issue - Klaerungspunkte", repository, Data);
                this.GUID_Elemente = repository_Element.Create_Package("Neue Parts Szenare", help, repository, Data);
                this.GUID_Guid = repository_Element.Create_Package("Issue - Guid Dopplung", help, repository, Data);
                EA.Package help2 = repository.GetPackageByGuid(GUID_Klaerungspunkte);
                help2.ParentID = help.PackageID;
                help.Packages.Refresh();
                help2.Update();
                EA.Package help3 = repository.GetPackageByGuid(GUID_Guid);
                help3.ParentID = help.PackageID;
                help.Packages.Refresh();
                help3.Update();
                #endregion Import Package anlegen

                //Was soll importiert werden
                #region Abfrage Import
                Choose_Require7_xac Choose = new Choose_Require7_xac(Data);
                Choose.label1.Text = "Wählen Sie die Elemente, welche importiert werden sollen.";
                Choose.checkBox_Szenar_Aufloesung.Checked = false;
                Choose.checkBox_Szenar_Aufloesung.Enabled = true;
                Choose.checkBox_Szenar_Aufloesung.Visible = true;
                Choose.checkBox_Ueberpreufung.Checked = true;
                Choose.checkBox_Ueberpreufung.Enabled = true;
                Choose.checkBox_Ueberpreufung.Visible = true;
                Choose.checkBox_Sysele.Visible = false;
                Choose.checkBox_Sysele.Enabled = false;
                Choose.checkBox_Szenar_Aufloesung.Update();
                Choose.checkBox_Ueberpreufung.Update();
                Choose.checkBox_Sysele.Update();
                Choose.TopMost = true;
              //  Choose.ActiveControl = Choose;
                Choose.BringToFront();
                Choose.ShowDialog();

                
                #endregion Abfrage Import
                ///////////////////////////////////
                ///Ladebalken
                #region Ladebalken initialisieren
                Loading_OpArch loading = new Loading_OpArch();
                loading.label2.Text = "Import Elements from xac";
                loading.label_Progress.Text = "Read xml File";
                loading.progressBar1.Step = 1;
                loading.progressBar1.Minimum = 0;
                loading.progressBar1.Maximum = 1;

                loading.Show();
                #endregion LAdebalken initialisieren

                //MessageBox.Show(Data.sys_xac.ToString());
                //Einlesen der Datei als string
                #region Einlesen File
                string xac_string = System.IO.File.ReadAllText(@filename);

                //MessageBox.Show(xac_string.Length.ToString());
                //String zu XElement konvertieren
                XElement xac_xml = XElement.Parse(xac_string);

                loading.progressBar1.PerformStep();
                loading.progressBar1.Refresh();
                #endregion Einlesen File
                //Anforderungen importieren
                #region Setzen ID auf not set
                //Alle Elemente mit TV OBJECT_ID erhalten
                loading.label2.Text = "Repository";
                loading.label_Progress.Text = "Korrektur ID's für Import";
                loading.progressBar1.Step = 1;
                loading.progressBar1.Minimum = 0;
                loading.progressBar1.Maximum = 1;
                loading.progressBar1.Value = 0;
                loading.Refresh();
                List<string> m_Type_all2 = new List<string>();
                m_Type_all2.AddRange(Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList());
                // m_Type_all.AddRange(Data.metamodel.m_Requirement.Select(x => x.Type).ToList());
                m_Type_all2.AddRange(Data.metamodel.m_Szenar.Select(x => x.Type).ToList());
                m_Type_all2.AddRange(Data.metamodel.m_Capability.Select(x => x.Type).ToList());
                List<string> m_Stereotype_all2 = new List<string>();
                m_Stereotype_all2.AddRange(Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                //m_Stereotype_all.AddRange(Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                m_Stereotype_all2.AddRange(Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList());
                m_Stereotype_all2.AddRange(Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList());

                List<Repository_Element> repository_element1 = repository_Elements.Get_All_Elements(Data, m_Type_all2, m_Stereotype_all2);
                List<Repository_Element> repository_element12 = repository_Elements.Get_All_Elements_Requirement(Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());

                repository_element1.AddRange(repository_element12);

                if (repository_element1.Count > 0)
                {
                    loading.progressBar1.Maximum = repository_element1.Count;
                    loading.Refresh();
                    int r1 = 0;
                    do
                    {
                        List<DB_Insert> m_TV = new List<DB_Insert>();
                        DB_Insert TV = new DB_Insert("OBJECT_ID", OleDbType.VarChar, OdbcType.VarChar, "not set", -1);
                        m_TV.Add(TV);
                        //TV Object_Id updaten
                        repository_element1[r1].Update_TV(m_TV, Data, repository);
                        r1++;
                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Refresh();
                    } while (r1 < repository_element1.Count);
                }

                #endregion Korrektur ID

                #region Anforderungen importieren
                if (Data.afo_funktional_xac == true || Data.afo_interface_xac == true || Data.afo_design_xac == true || Data.afo_user_xac == true ||Data.afo_process_xac == true || Data.afo_umwelt_xac == true ||Data.afo_typevertreter_xac == true)
                {
                  
                    interface_Collection_OleDB.Open_Connection(Data);


                    loading.label2.Text = "Requirements";
                    loading.label_Progress.Text = "Search xml for Requirements";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = 1;
                    loading.progressBar1.Value = 0;
                    loading.Refresh();

                    //PAckage Afo
                    string Package_Import_AFo_GUID = repository_Element.Create_Package_Model("Import - XAC - Anforderungen", repository, Data);
                    EA.Package Package_Import_AFo = repository.GetPackageByGuid(Package_Import_AFo_GUID);
                    Package_Import_AFo.ParentID = Package_Import.PackageID;
                    Package_Import.Packages.Refresh();
                    Package_Import_AFo.Update();

                    string Package_Import_Nachweisart_GUID = repository_Element.Create_Package_Model("Import - XAC - Nachweisart", repository, Data);
                    EA.Package Package_Nachweisart_AFo = repository.GetPackageByGuid(Package_Import_Nachweisart_GUID);
                    Package_Nachweisart_AFo.ParentID = Package_Import.PackageID;
                    Package_Import.Packages.Refresh();
                    Package_Nachweisart_AFo.Update();



                    var item_afo = from c in xac_xml.Descendants("item")
                                  where (string)c.Attribute("Klasse") == "Anforderung"
                                  select c;

                    loading.progressBar1.Maximum = item_afo.Count();
                    loading.progressBar1.Refresh();
                    //MessageBox.Show(item_afo.Count().ToString());
                    int i2 = 0;
                    foreach (var i1 in item_afo)
                    {
                        if(i1.Parent.Name == "items")
                        {
                           // MessageBox.Show("Index: " + i2.ToString());
                            Import_AFo(i1, repository, Data, Package_Import_AFo_GUID, loading);
                        }

                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Refresh();
                        i2++;
                    }

                    ////////////////////////////////////////
                    Check_Requirement(Data, repository);
                    ////////////////////////////////////////
                    //Erzeugung Nachweisart
                    if(Data.Nachweisart_xac == true)
                    {
                        Create_Nachweisart(Data, repository, Package_Import_Nachweisart_GUID);
                    }

                    //MDG der Afo aufdatieren

                    if(Data.metamodel.m_Requirement_ADMBw.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {

                            repository_Elements.Update_Anforderungen_MDG(Data, repository, Data.metamodel.m_Requirement_ADMBw[i1].Type, Data.metamodel.m_Requirement_ADMBw[i1].Stereotype, Data.metamodel.m_Requirement_ADMBw[i1].Toolbox);

                            i1++;
                        } while (i1 < Data.metamodel.m_Requirement_ADMBw.Count);
                    }

                    interface_Collection_OleDB.Close_Connection(Data);
                }
                #endregion Anforderungen importieren
                ////////////////////////
                #region Systemelemente importieren
                ///Systemelemente importieren
                if (Data.sys_xac == true ||Data.logical_xac == true || Data.capability_xac == true)
                {
                   
                    interface_Collection_OleDB.Open_Connection(Data);
                    //Ladebalken
                    loading.label2.Text = "Systemelemente";
                    loading.label_Progress.Text = "Search xml for SystemElements";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = 1;
                    loading.progressBar1.Value = 0;
                    loading.Refresh();
                    //PAckage Afo
                    string Package_Import_Sys_GUID = repository_Element.Create_Package_Model("Import - XAC - Technisches System", repository, Data);
                    EA.Package Package_Import_Sys = repository.GetPackageByGuid(Package_Import_Sys_GUID);
                    Package_Import_Sys.ParentID = Package_Import.PackageID;
                    Package_Import.Packages.Refresh();
                    Package_Import_Sys.Update();

                    string Package_Import_Logical_GUID = repository_Element.Create_Package_Model("Import - XAC - Szenarbaum", repository, Data);
                    EA.Package Package_Import_Logical_ = repository.GetPackageByGuid(Package_Import_Logical_GUID);
                    Package_Import_Logical_.ParentID = Package_Import.PackageID;
                    Package_Import.Packages.Refresh();
                    Package_Import_Logical_.Update();

                    string Package_Import_Capability_GUID = repository_Element.Create_Package_Model("Import - XAC - Funktionsbaum", repository, Data);
                    EA.Package Package_Import_Capability_ = repository.GetPackageByGuid(Package_Import_Capability_GUID);
                    Package_Import_Capability_.ParentID = Package_Import.PackageID;
                    Package_Import.Packages.Refresh();
                    Package_Import_Capability_.Update();

                    var item_sys = from c in xac_xml.Descendants("item")
                                   where (string)c.Attribute("Klasse") == "Systemelement"
                                   select c;

                    loading.progressBar1.Maximum = item_sys.Count();
                    loading.progressBar1.Refresh();

                    foreach (var i1 in item_sys)
                    {
                        
                        if(i1.Parent.Name == "items")
                        {
                            Import_Sys(i1, repository, Data, Package_Import_Sys_GUID, Package_Import_Logical_GUID, Package_Import_Capability_GUID, loading);
                        }

                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Refresh();

                    }
                    /*  if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                      {
                          Data.oLEDB_Interface.OLEDB_Close();
                      }
                      */
                    // Interface_Collection interface_Collection_OleDB = new Interface_Collection();

                    Check_SysElem(Data, repository);

                    interface_Collection_OleDB.Close_Connection(Data);
                }
                #endregion Systemelemente importieren
                /////////////////////////////
                #region Stakeholder importieren
                if(Data.stakeholder_xac == true)
                {
                    /*  if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                      {
                          Data.oLEDB_Interface.OLEDB_Open();
                      }*/
                //    Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                    interface_Collection_OleDB.Open_Connection(Data);
                    //Ladebalken
                    loading.label2.Text = "Stakeholder";
                    loading.label_Progress.Text = "Search xml for Stakeholders";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = 1;
                    loading.progressBar1.Value = 0;
                    loading.Refresh();
                    //PAckage Stakeholder
                    string Package_Import_St_GUID = repository_Element.Create_Package_Model("Import - XAC -  Stakeholder", repository, Data);
                    EA.Package Package_Import_St = repository.GetPackageByGuid(Package_Import_St_GUID);
                    Package_Import_St.ParentID = Package_Import.PackageID;
                    Package_Import.Packages.Refresh();
                    Package_Import_St.Update();


                    var item_st = from c in xac_xml.Descendants("item")
                                   where (string)c.Attribute("Klasse") == "Stakeholder"
                                   select c;

                    loading.progressBar1.Maximum = item_st.Count();
                    loading.progressBar1.Refresh();

                    foreach (var i1 in item_st)
                    {

                        if (i1.Parent.Name == "items")
                        {
                            Import_St(i1, repository, Data, Package_Import_St_GUID, loading);
                        }

                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Refresh();

                    }
                    /*  if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                      {
                          Data.oLEDB_Interface.OLEDB_Close();
                      }*/
                  //  Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                    interface_Collection_OleDB.Close_Connection(Data);

                }
                #endregion Stakeholder importieren
                /////////////////////////////
                ///Konnektoren importieren
                #region Konnektoren importieren
                #region Direkter Import
                if (Data.link_decomposition == true || Data.link_afo_sys == true || Data.link_afo_afo == true || Data.capability_xac == true|| Data.link_afo_logical == true || Data.link_afo_st == true)
                {
                   
                 //   Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                    interface_Collection_OleDB.Open_Connection(Data);

                    var links = from e in xac_xml.Descendants("link")
                                   select e;

                    //Ladebalken
                    loading.label2.Text = "Konnektoren";
                    loading.label_Progress.Text = "Search xml for Links";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = links.Count();
                    loading.progressBar1.Value = 0;
                    loading.Refresh();

                    foreach (var i4 in links)
                    {
                        if(i4.Parent.Name == "links")
                        {
                            Import_Link(i4, Data, repository, loading);
                        }
                       

                        loading.progressBar1.PerformStep();
                        loading.Refresh();
                    }
                 
                    interface_Collection_OleDB.Close_Connection(Data);
                }
                #endregion Direkter Import

                #region Auflösung Logical --> AFo
                //Logcial --> Afo Konnektoren auflösen
                if (Data.link_afo_sys == true && Data.link_afo_logical == true && Data.Logical_aufloesung_xac == true)
                {
                    //  Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                    interface_Collection_OleDB.Open_Connection(Data);
                    //Ladebalken
                    loading.label2.Text = "Anlegen Elemente Szenare und Konnektoren";
                    loading.label_Progress.Text = " ";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = 1;
                    loading.progressBar1.Value = 0;
                    loading.Refresh();

                    Import_Link_Afo_Logical(repository, Data, loading);

                    //Löschen aktuell  aussetzen
                    //Delete_Link_Afo_Sys_Decomposition(Data, repository);

                    interface_Collection_OleDB.Close_Connection(Data);
                }
                #endregion Auflösung Logical --> AFo

                #region Neue Konnektoren Issues anlegen

                //Package Neue Konnektoren
                this.GUID_Konnektoren = repository_Element.Create_Package("Neue Konnektoren", help, repository, Data);
               
                #region FunktionsbaumAfo
                if (m_GUID_FunktionsbaumAfo_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Afo --> Funktionsbaum: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_FunktionsbaumAfo_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region SysAfo
                if (m_GUID_SysAfo_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Afo --> System: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_SysAfo_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region LogicalAfo
                if (m_GUID_LogicalAfo_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Afo --> Szenar: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_LogicalAfo_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region Refines
                if (m_GUID_Refines_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Refines: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_Refines_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region Requires
                if (m_GUID_Requires_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Requires: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_Requires_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region Dublette
                if (m_GUID_Dublette_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Dubletten: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_Dublette_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region Conflicts
                if (m_GUID_Conflicts_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Conflicts: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_Conflicts_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region Replaces
                if (m_GUID_Replaces_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Replaces: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_Replaces_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region InheritsFrom
                if (m_GUID_InheritsFrom_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "InheritsFrom: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_InheritsFrom_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region SysSys
                if (m_GUID_System_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Sys --> Sys: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_System_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

                #region FunkFunk
                if (m_GUID_Funktionsbaum_Connector_Import_new.Count > 0)
                {
                    //Issue Name und Note
                    string name = "Funktionsbaum --> Funktionsbaum: Neue Zuordnungen Import " + DateTime.Now.ToString();
                    string note = "Die hier verknüpften Konnektoren wurden neu erzeugt";
                    //Issue erzeugen und verknüpfen
                    this.Create_Issue_Connectoren(name, note, m_GUID_Funktionsbaum_Connector_Import_new, this.GUID_Konnektoren, Data, repository);
                }
                #endregion

           
        #endregion

                #region Korrektur ID
        //Alle Elemente mit TV OBJECT_ID erhalten
            /*    loading.label2.Text = "Repository";
                loading.label_Progress.Text = "Korrektur ID's";
                loading.progressBar1.Step = 1;
                loading.progressBar1.Minimum = 0;
                loading.progressBar1.Maximum = 1;
                loading.progressBar1.Value = 0;
                loading.Refresh();
                List<string> m_Type_all = new List<string>();
                m_Type_all.AddRange(Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList());
                // m_Type_all.AddRange(Data.metamodel.m_Requirement.Select(x => x.Type).ToList());
                m_Type_all.AddRange(Data.metamodel.m_Szenar.Select(x => x.Type).ToList());
                m_Type_all.AddRange(Data.metamodel.m_Capability.Select(x => x.Type).ToList());
                List<string> m_Stereotype_all = new List<string>();
                m_Stereotype_all.AddRange(Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                //m_Stereotype_all.AddRange(Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                m_Stereotype_all.AddRange(Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList());
                m_Stereotype_all.AddRange(Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList());

                List<Repository_Element> repository_element = repository_Elements.Get_All_Elements(Data, m_Type_all, m_Stereotype_all);
                List<Repository_Element> repository_element2 = repository_Elements.Get_All_Elements_Requirement(Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());

                repository_element.AddRange(repository_element2);

                if (repository_element.Count > 0)
                {
                    loading.progressBar1.Maximum = repository_element.Count;
                    loading.Refresh();
                    int r1 = 0;
                    do
                    {
                        List<DB_Insert> m_TV = new List<DB_Insert>();
                        DB_Insert TV = new DB_Insert("OBJECT_ID", OleDbType.VarChar, OdbcType.VarChar, repository_element[r1].ID, -1);
                        m_TV.Add(TV);
                        //TV Object_Id updaten
                        repository_element[r1].Update_TV(m_TV, Data, repository);
                        r1++;
                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Refresh();
                    } while (r1 < repository_element.Count);
                }

                */
                #endregion Korrektur ID

                #region Check

                if(Data.Check_xac == true)
                {
                    #region Check Konnektoren Anforderungen
                    loading.label2.Text = "Check Import Konnektoren Anforderungen";
                    loading.label_Progress.Text = " ";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = 9;
                    loading.progressBar1.Value = 0;
                    loading.Update();


                    #region Check Import Funktionsbaum Afo Konnektoren
                    loading.label_Progress.Text = "Anforderung Zuordnung Funktionsbaum";
                    loading.label_Progress.Refresh();

                    m_No_Import.Add(m_Afo_Capability_NoImport);

                    if (Data.link_afo_sys == true)
                    {


                        m_Afo_Capability_NoImport = this.Check_Konnektoren(Data.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), this.m_GUID_FunktionsbaumAfo_Connector_Import, Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList());
                       


                        

                        if (m_Afo_Capability_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[5], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Afo_Capability_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Afo_Capability_NoImport[c1], Data.metamodel.m_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Requirement.Contains(Client_GUID) == true)
                                    {
                                       // m_Afo_Capability_NoImport2.Add(m_Afo_Capability_NoImport[c1]);

                                        m_help.Add(m_Afo_Capability_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);
                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Zuordnung Funktionsbaum - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Zuordnung");
                                        issue.Insert_TV_Connector(m_Afo_Capability_NoImport[c1], Data, repository);


                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0],true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Afo_Capability_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                                        flag_delete = true;
                                    }





                                }


                                c1++;
                            } while (c1 < m_Afo_Capability_NoImport.Count);

                            m_Afo_Capability_NoImport = m_help;
                            m_No_Import[0] = m_Afo_Capability_NoImport;
                        }


                    }

                    #endregion Check Import Funktionsbaum Afo Konnektoren

                    #region Check Imprt Sys Afo Konnektoren
                    loading.progressBar1.PerformStep();
                    loading.progressBar1.Update();
                    loading.label_Progress.Text = "Anforderung Zuordnung Technisches System";
                    loading.Update();

                    m_No_Import.Add(m_Afo_Sys_NoImport);
                    if (Data.link_afo_sys == true)
                    {

                        m_Afo_Sys_NoImport = this.Check_Konnektoren(Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), this.m_GUID_SysAfo_Connector_Import, Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                        

                        if (m_Afo_Sys_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("trace");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[6], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Afo_Sys_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Afo_Sys_NoImport[c1], Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Requirement.Contains(Client_GUID) == true) //Nur imporierte Anforderungen betrachten
                                    {
                                        m_help.Add(m_Afo_Sys_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);

                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Zuordnung Technisches System - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Zuordnung");
                                        issue.Insert_TV_Connector(m_Afo_Sys_NoImport[c1], Data, repository);
                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Afo_Sys_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);


                                        flag_delete = true;
                                    }
                                }


                                c1++;
                            } while (c1 < m_Afo_Sys_NoImport.Count);

                            m_Afo_Sys_NoImport = m_help;
                            m_No_Import[1] = m_Afo_Sys_NoImport;
                        }


                    }
                    #endregion Check Imprt Sys Afo Konnektoren

                    #region Check Imprt Logical Afo Konnektoren
                    loading.progressBar1.PerformStep();
                    loading.progressBar1.Update();
                    loading.label_Progress.Text = "Anforderung Zuordnung Szenar";
                    loading.Update();

                    m_No_Import.Add(m_Afo_Logical_NoImport);

                    if (Data.link_afo_logical == true)
                    {

                        m_Afo_Logical_NoImport = this.Check_Konnektoren(Data.metamodel.m_Derived_Logical.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Logical.Select(x => x.Stereotype).ToList(), this.m_GUID_LogicalAfo_Connector_Import, Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Szenar.Select(x => x.Type).ToList(), Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList());
                       
                        if (m_Afo_Logical_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[7], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Afo_Logical_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Afo_Logical_NoImport[c1], Data.metamodel.m_Szenar.Select(x => x.Type).ToList(), Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Requirement.Contains(Client_GUID) == true)
                                    {
                                        m_help.Add(m_Afo_Logical_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);

                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Zuordnung Szenar - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Zuordnung");
                                        issue.Insert_TV_Connector(m_Afo_Logical_NoImport[c1], Data, repository);
                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Afo_Logical_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);


                                        flag_delete = true;
                                    }
                                }


                                c1++;
                            } while (c1 < m_Afo_Logical_NoImport.Count);

                            m_Afo_Logical_NoImport = m_help;
                            m_No_Import[2] = m_Afo_Logical_NoImport;
                        }
                    }
                    #endregion Check Imprt Logical Afo Konnektoren

                    #region Check Anforderungne Beziehungen

                    m_No_Import.Add(m_Afo_Refines_NoImport);
                    m_No_Import.Add(m_Afo_Requires_NoImport);
                    m_No_Import.Add(m_Afo_Replaces_NoImport);
                    m_No_Import.Add(m_Afo_Conflicts_NoImport);
                    m_No_Import.Add(m_Afo_Dublette_NoImport);

                    if (Data.link_afo_afo == true)
                    {

                        #region Refines
                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Update();
                        loading.label_Progress.Text = "Anforderung Beziehung Refines";
                        loading.Update();
                        m_Afo_Refines_NoImport = this.Check_Konnektoren(Data.metamodel.m_Afo_Refines.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList(), this.m_GUID_Refines_Connector_Import, Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                       
                        if (m_Afo_Refines_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[9], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Afo_Refines_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Afo_Refines_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Requirement.Contains(Client_GUID) == true && this.m_GUID_Requirement.Contains(Supplier_GUID))
                                    {
                                        m_help.Add(m_Afo_Refines_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);
                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Beziehung Anforderung - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Beziehung");
                                        issue.Insert_TV_Connector(m_Afo_Refines_NoImport[c1], Data, repository);
                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Afo_Refines_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                                        flag_delete = true;
                                    }
                                }


                                c1++;
                            } while (c1 < m_Afo_Refines_NoImport.Count);

                            m_Afo_Refines_NoImport = m_help;
                            m_No_Import[3] = m_Afo_Refines_NoImport;
                        }
                        #endregion Refines

                        #region Requires
                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Update();
                        loading.label_Progress.Text = "Anforderung Beziehung Requires";
                        loading.Update();
                        m_Afo_Requires_NoImport = this.Check_Konnektoren(Data.metamodel.m_Afo_Requires.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList(), this.m_GUID_Requires_Connector_Import, Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                       
                        if (m_Afo_Requires_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[10], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Afo_Requires_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Afo_Requires_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Requirement.Contains(Client_GUID) == true && this.m_GUID_Requirement.Contains(Supplier_GUID))
                                    {
                                        m_help.Add(m_Afo_Requires_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);
                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Beziehung Anforderung - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Beziehung");
                                        issue.Insert_TV_Connector(m_Afo_Requires_NoImport[c1], Data, repository);
                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Afo_Requires_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                                        flag_delete = true;
                                    }
                                }


                                c1++;
                            } while (c1 < m_Afo_Requires_NoImport.Count);

                            m_Afo_Requires_NoImport = m_help;
                            m_No_Import[4] = m_Afo_Requires_NoImport;
                        }
                        #endregion  Requires

                        #region Replaces
                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Update();
                        loading.label_Progress.Text = "Anforderung Beziehung Replaces";
                        loading.Update();
                        m_Afo_Replaces_NoImport = this.Check_Konnektoren(Data.metamodel.m_Afo_Replaces.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Replaces.Select(x => x.Stereotype).ToList(), this.m_GUID_Requires_Connector_Import, Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                       
                        if (m_Afo_Replaces_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[11], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Afo_Replaces_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Afo_Replaces_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Requirement.Contains(Client_GUID) == true && this.m_GUID_Requirement.Contains(Supplier_GUID))
                                    {
                                        m_help.Add(m_Afo_Replaces_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);
                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Beziehung Anforderung - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Beziehung");
                                        issue.Insert_TV_Connector(m_Afo_Replaces_NoImport[c1], Data, repository);
                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Afo_Replaces_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                                        flag_delete = true;
                                    }
                                }


                                c1++;
                            } while (c1 < m_Afo_Replaces_NoImport.Count);

                            m_Afo_Replaces_NoImport = m_help;
                            m_No_Import[5] = m_Afo_Replaces_NoImport;
                        }
                        #endregion  Replaces

                        #region Conflicts
                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Update();
                        loading.label_Progress.Text = "Anforderung Beziehung Conflicts";
                        loading.Update();
                        m_Afo_Conflicts_NoImport = this.Check_Konnektoren(Data.metamodel.m_Afo_Konflikt.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Konflikt.Select(x => x.Stereotype).ToList(), this.m_GUID_Conflicts_Connector_Import, Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                        
                        if (m_Afo_Conflicts_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[12], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Afo_Conflicts_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Afo_Conflicts_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Requirement.Contains(Client_GUID) == true && this.m_GUID_Requirement.Contains(Supplier_GUID))
                                    {
                                        m_help.Add(m_Afo_Conflicts_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);
                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Beziehung Anforderung - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Beziehung");
                                        issue.Insert_TV_Connector(m_Afo_Conflicts_NoImport[c1], Data, repository);
                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Afo_Conflicts_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                                        flag_delete = true;
                                    }
                                }


                                c1++;
                            } while (c1 < m_Afo_Conflicts_NoImport.Count);

                            m_Afo_Conflicts_NoImport = m_help;
                            m_No_Import[6] = m_Afo_Conflicts_NoImport;
                        }
                        #endregion  Conflicts

                        #region Dublette
                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Update();
                        loading.label_Progress.Text = "Anforderung Beziehung Dublette";
                        loading.Update();
                        m_Afo_Dublette_NoImport = this.Check_Konnektoren(Data.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList(), Data.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList(), this.m_GUID_Dublette_Connector_Import, Data, Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                       
                        if (m_Afo_Dublette_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[13], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Afo_Dublette_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Afo_Dublette_NoImport[c1], Data.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Requirement.Contains(Client_GUID) == true && this.m_GUID_Requirement.Contains(Supplier_GUID))
                                    {
                                        m_help.Add(m_Afo_Dublette_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);
                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Beziehung Anforderung - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Beziehung");

                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Afo_Dublette_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                                        flag_delete = true;
                                    }
                                }


                                c1++;
                            } while (c1 < m_Afo_Dublette_NoImport.Count);

                            m_Afo_Dublette_NoImport = m_help;
                            m_No_Import[7] = m_Afo_Dublette_NoImport;
                        }
                        #endregion  Dublette
                    }


                    #endregion Check Anforderungen Beziehungen

                    #endregion Check Konnektoren Anforderungen

                    #region Check Dekomposition
                    loading.label2.Text = "Check Import Konnektoren Taxonomy";
                    loading.label_Progress.Text = " ";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = 4;
                    loading.progressBar1.Value = 0;
                    loading.Update();
                    #region Check Import Funktionsbaum Konnektoren


                    //Alle Konnektoren Funktionsbaum erhalten
                    loading.label_Progress.Text = "Taxonomy Funktionsbaum";
                    loading.label_Progress.Refresh();

                    m_No_Import.Add(m_Taxonomy_Capability_NoImport);

                    if (Data.link_decomposition == true)
                    {
                        m_Taxonomy_Capability_NoImport = this.Check_Konnektoren(Data.metamodel.m_Taxonomy_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Taxonomy_Capability.Select(x => x.Stereotype).ToList(), this.m_GUID_Funktionsbaum_Connector_Import, Data, Data.metamodel.m_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList());
                       
                        if (m_Taxonomy_Capability_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[4], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Taxonomy_Capability_NoImport[c1], Data.metamodel.m_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Taxonomy_Capability_NoImport[c1], Data.metamodel.m_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Systemelement.Contains(Client_GUID) && this.m_GUID_Systemelement.Contains(Supplier_GUID))
                                    {
                                        m_help.Add(m_Taxonomy_Capability_NoImport[c1]);

                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);

                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Taxonomy Funktionsbaum - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Taxonomy");
                                        issue.Insert_TV_Connector(m_Taxonomy_Capability_NoImport[c1], Data, repository);
                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Taxonomy_Capability_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        flag_delete = true;
                                    }


                                }


                                c1++;
                            } while (c1 < m_Taxonomy_Capability_NoImport.Count);

                            m_Taxonomy_Capability_NoImport = m_help;
                            m_No_Import[8] = m_Taxonomy_Capability_NoImport;
                        }
                    }


                    #endregion Check Import Funktionsbaum Konnektoren

                    #region Check Import Technisches System Konnektoren
                    loading.progressBar1.PerformStep();
                    loading.progressBar1.Update();
                    loading.label_Progress.Text = "Taxonomy Technisches System";
                    loading.label_Progress.Refresh();
                    loading.Update();

                    m_No_Import.Add(m_Taxonomy_System_NoImport);


                    if (Data.link_decomposition == true)
                    {
                        m_Taxonomy_System_NoImport = this.Check_Konnektoren(Data.metamodel.m_Decomposition_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Decomposition_Element.Select(x => x.Stereotype).ToList(), this.m_GUID_System_Connector_Import, Data, Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                       
                        if (m_Taxonomy_System_NoImport.Count > 0)
                        {
                            Interface_Connectors interface_Connectors = new Interface_Connectors();
                            Repository_Connector con = new Repository_Connector();
                            Repository_Class client = new Repository_Class();
                            Repository_Class supplier = new Repository_Class();
                            DateTime time = new DateTime();
                            List<string> m_Type_con = new List<string>();
                            m_Type_con.Add("Dependency");
                            List<string> m_Stereotype_con = new List<string>();
                            m_Stereotype_con.Add("trace");
                            List<string> m_Toolbox_con = new List<string>();
                            m_Toolbox_con.Add("");
                            //Packages anlegen
                            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);
                            string Package_guid2 = repository_Element.Create_Package(Data.metamodel.m_Package_Name[8], repository.GetPackageByGuid(Package_guid), repository, Data);

                            List<string> m_help = new List<string>();

                            int c1 = 0;
                            do
                            {
                                //Client erhalten
                                string Client_GUID = interface_Connectors.Get_Client_GUID(Data, m_Taxonomy_System_NoImport[c1], Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());
                                string Supplier_GUID = interface_Connectors.Get_Supplier_GUID(Data, m_Taxonomy_System_NoImport[c1], Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList());

                                if (Client_GUID != null && Supplier_GUID != null)
                                {
                                    if (this.m_GUID_Systemelement.Contains(Client_GUID) && this.m_GUID_Systemelement.Contains(Supplier_GUID))
                                    {

                                        m_help.Add(m_Taxonomy_System_NoImport[c1]);
                                        client.Classifier_ID = Client_GUID;
                                        client.Name = client.Get_Name(Data);
                                        //Supplier erhalten
                                        supplier.Classifier_ID = Supplier_GUID;
                                        supplier.Name = supplier.Get_Name(Data);

                                        //Issue anlegen


                                        string Name8 = DateTime.Now.ToString() + " ";
                                        string name = Name8 + "Taxonomy Technisches System - Konnektor nicht importiert : " + client.Name + " --> " + supplier.Name;

                                        Issue issue = new Issue(Data, name, Data.metamodel.issues.m_Issue_Note[19], Package_guid2, repository, true, "Taxonomy");
                                        issue.Insert_TV_Connector(m_Taxonomy_System_NoImport[c1], Data, repository);
                                        //Issue mit Client und Supplier des Konnektors verknüpfen
                                        con.Create_Dependency(issue.Classifier_ID, Client_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        con.Create_Dependency(issue.Classifier_ID, Supplier_GUID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        //Issue mit ProxyConnector verknüpfen
                                        con.Create_ConnectorProxy_Dependency_Supplier(m_Taxonomy_System_NoImport[c1], issue.Classifier_ID, m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                                        flag_delete = true;
                                    }


                                }


                                c1++;
                            } while (c1 < m_Taxonomy_System_NoImport.Count);

                            m_Taxonomy_System_NoImport = m_help;
                            m_No_Import[9] = m_Taxonomy_System_NoImport;
                        }
                    }


                    #endregion Check Import Funktionsbaum Konnektoren

                    #endregion Check Dekomposition
                }

                #endregion Check
                #endregion Konnektoren importieren



                #region Abfrage Löschen Elemente
                if (flag_delete == true && Data.Check_xac == true)
                {
                    Forms.Choose_Import_Elements_To_Delete Form_delete = new Forms.Choose_Import_Elements_To_Delete(Data, m_No_Import, repository);

                    List<string> m_check_con = new List<string>();

                   if(m_No_Import.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if(m_No_Import[i2] != null)
                            {
                                m_check_con.AddRange(m_No_Import[i2]);
                            }

                            i2++;
                        } while (i2 < m_No_Import.Count);
                    }

                    Form_delete.ShowDialog();

                    Repsoitory_Elements.Repository_Elements repository_Elements1 = new Repsoitory_Elements.Repository_Elements();
                    List<Issue> m_con_issue = repository_Elements1.Get_Issues_Connetoren(m_check_con, Data, repository);

                    if(m_con_issue.Count > 0)
                    {
                        this.GUID_Konnektoren_Archiv = repository_Element.Create_Package("Konnektoren - Archiv", help, repository, Data);
                        EA.Element issue;

                        int i2 = 0;
                        do
                        {
                            if(m_con_issue[i2].Check_Connector(repository, m_con_issue[i2].Con_guid) == false)
                            {
                                //Archiv verschieben
                                issue = repository.GetElementByGuid(m_con_issue[i2].Classifier_ID);
                                issue.PackageID = repository.GetPackageByGuid(this.GUID_Konnektoren_Archiv).PackageID;
                                issue.Update();
                            }

                            i2++;
                        } while (i2 < m_con_issue.Count);

                    }

                   
                }

                #endregion Abfrage Löschen Elemente
                ////////////////////////////
                ///Korrektur ID --> mit einer ID mit -xxx kann SPARX EA nicht umgehen

                ///Erzeugung InformationFlow
                ///
                ///Dies ist leider aktuell nicht sinnvoll, da die InformationsElement nicht direkt importiert werden können.
                /*
                if(Data.link_afo_sys == true && Data.link_afo_afo == true && Data.sys_xac == true && Data.afo_interface_xac == true)
                {
                    Import_Create_InformationFlow(repository, Data);
                }
                */
                loading.Close();
                repository.RefreshModelView(0);

            }
        }

        #region Choose File
        private string Choose_OpenFile()
        {
            string filename = "";
            bool save = false;

            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "c:\\";
            open.Filter = "Require7|*.xac";
            open.Title = "Open an Require7 File";
            open.ShowDialog();
               

            if (open.FileName != "")
            {
                filename = open.FileName;
                save = true;
            }

            return (filename);
        }
        #endregion Choose File

        #region Import Element
        private void Import_AFo(XElement Afo, EA.Repository repository, Database Data, string Package_Import_AFo_GUID, Loading_OpArch loading)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();
            interface_Collection_OleDB.Close_Connection(Data);

            bool created = false;
            //Variablen
            Repository_Element repository_Element = new Repository_Element();
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            //StereoType überprüfen
            List<string> m_AFo_StereoType = Get_Afo_StereoType_Import(Afo, Data);

            //    XML xml = new XML();
            //GUID erhalten
            XAttribute att = Afo.Attribute("uuid");

            string AFO_GUID = "{" + att.Value + "}";




            if (created == false)
            {

                List<string> GUIDS2 = repository_Elements.Check_GUID_t_object(Data, AFO_GUID);

                List<string> m_GUID_help = new List<string>();
                m_GUID_help.Add(AFO_GUID);

                //      List<string> GUIDS = repository_Elements.Check_Element_t_object(Data, this.m_Type_Requirement, m_AFo_StereoType, m_GUID_help);
                List<string> GUIDS = repository_Elements.Check_Element_t_object(Data, this.m_Type_Requirement, null, m_GUID_help);
                //MessageBox.Show(GUIDS.Count.ToString());
                //vorhanden
                if (GUIDS != null)
                {

                    bool flag_update = false;

                    //Interface
                    if (Data.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_interface_xac == true)
                    {
                        flag_update = true;
                    }
                    //Functional
                    if (Data.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_funktional_xac == true)
                    {
                        flag_update = true;
                    }
                    //Stakeholder
                    if (Data.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_user_xac == true)
                    {
                        flag_update = true;
                    }
                    //Design
                    if (Data.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_design_xac == true)
                    {
                        flag_update = true;
                    }
                    //Process
                    if (Data.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_process_xac == true)
                    {
                        flag_update = true;
                    }
                    //Umwelt
                    if (Data.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_umwelt_xac == true)
                    {
                        flag_update = true;
                    }
                    //Typevertreter
                    if (Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_typevertreter_xac == true)
                    {
                        flag_update = true;
                    }
                    //Nichtfunctionale Anforderungen
                    if (Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true)
                    {
                        flag_update = true;
                    }

                    //Update aller Tagged Values
                    if (flag_update == true)
                    {


                        repository_Element.Classifier_ID = AFO_GUID;

                        loading.label_Progress.Text = repository_Element.Get_Name(Data) + " - Update AFo";
                        loading.label_Progress.Refresh();

                        Update_Import(Afo, repository, AFO_GUID, true, false, false, Data, null, Package_Import_AFo_GUID);
                    }
                }
                else
                {
                    bool flag_update = false;

                    //Interfaces
                    if (Data.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_interface_xac == true)
                    {
                        flag_update = true;
                    }
                    //Functional
                    if (Data.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_funktional_xac == true)
                    {
                        flag_update = true;
                    }
                    //Stakeholder
                    if (Data.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_user_xac == true)
                    {
                        flag_update = true;
                    }
                    //Design
                    if (Data.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_design_xac == true)
                    {
                        flag_update = true;
                    }
                    //Process
                    if (Data.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_process_xac == true)
                    {
                        flag_update = true;
                    }
                    //Umwelt
                    if (Data.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_umwelt_xac == true)
                    {
                        flag_update = true;
                    }
                    //Typevertreter
                    if (Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true && Data.afo_typevertreter_xac == true)
                    {
                        flag_update = true;
                    }
                    //Nichtfunctionale Anforderungen
                    if (Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Stereotype).ToList().Contains(m_AFo_StereoType[0]) == true)
                    {
                        flag_update = true;
                    }


                    //Update aller Tagged Values
                    if (GUIDS2 != null)
                    {
                        //MessageBox.Show("Element mit GUID: " + AFO_GUID + " ist im Repository schon vorhanden. Das aktuelle Element wird mit einer neuen GUID angelegt.");
                        TaggedValue tagged2 = new TaggedValue(Data.metamodel, Data);

                        AFO_GUID = tagged2.Generate_GUID("t_object");

                        Metamodels.Issues issues = new Issues();
                        Repsoitory_Elements.Issue issue = new Issue(Data, issues.m_Issue_Name[28], issues.m_Issue_Note[28], this.GUID_Guid, repository, true, "Dopplung");

                        Repsoitory_Elements.Repository_Connector repository_Connector = new Repository_Connector();
                        repository_Connector.Create_Dependency(issue.Classifier_ID, AFO_GUID, Data.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), Data.metamodel.m_Con_Trace[0].SubType, repository, Data, Data.metamodel.m_Con_Trace[0].Toolbox, Data.metamodel.m_Con_Trace[0].direction);

                        if (this.m_GUID_Requirement.Contains(AFO_GUID))
                        {
                            created = true; //Anforderung mit der uuid wurde schon betrachtet
                        }
                        else
                        {
                            this.m_GUID_Requirement.Add(AFO_GUID);
                        }

                        if (flag_update == true)
                        {

                            loading.label_Progress.Text = "Generierung AFo";
                            loading.label_Progress.Refresh();
                            //Anlegen eines Requirements und der Tagged Values
                            Requirement new_Add = new Requirement(null, Data.metamodel);

                            string GUID = new_Add.Create_Requirement(repository, Package_Import_AFo_GUID, m_AFo_StereoType[0], Data);


                            //Es müssen hier noch nun die ea_guid in allen Tables auf die aus dem Import synchronisiert werden
                            //t_object, t_objectproperties, t_xref....
                            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);

                            Interface_Element interface_Element = new Interface_Element();
                            interface_Element.Update_VarChar(GUID, AFO_GUID, "ea_guid", Data);

                            //  tagged.UPDATE_SQL_t_object(AFO_GUID, "ea_guid", GUID, repository);
                            tagged.UPDATE_SQL_t_xref(AFO_GUID, "Client", GUID, repository);

                            this.m_GUID_Requirement_New.Add(AFO_GUID);



                            Update_Import(Afo, repository, AFO_GUID, true, false, true, Data, null, Package_Import_AFo_GUID);
                        }


                    }
                    else
                    {
                        if (flag_update == true)
                        {

                            loading.label_Progress.Text = "Generierung AFo";
                            loading.label_Progress.Refresh();
                            //Anlegen eines Requirements und der Tagged Values
                            Requirement new_Add = new Requirement(null, Data.metamodel);

                            string GUID = new_Add.Create_Requirement(repository, Package_Import_AFo_GUID, m_AFo_StereoType[0], Data);


                            //Es müssen hier noch nun die ea_guid in allen Tables auf die aus dem Import synchronisiert werden
                            //t_object, t_objectproperties, t_xref....
                            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);

                            Interface_Element interface_Element = new Interface_Element();
                            interface_Element.Update_VarChar(GUID, AFO_GUID, "ea_guid", Data);

                            //  tagged.UPDATE_SQL_t_object(AFO_GUID, "ea_guid", GUID, repository);
                            tagged.UPDATE_SQL_t_xref(AFO_GUID, "Client", GUID, repository);

                            this.m_GUID_Requirement_New.Add(AFO_GUID);



                            Update_Import(Afo, repository, AFO_GUID, true, false, true, Data, null, Package_Import_AFo_GUID);
                        }
                    }
                }
                
                interface_Collection_OleDB.Open_Connection(Data);


            }
        }
        private void Import_Sys(XElement Sys, EA.Repository repository, Database Data, string Package_Import_Sys_GUID, string Package_Import_Logical_GUID, string Package_Import_Capability_GUID, Loading_OpArch loading)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();
            interface_Collection_OleDB.Close_Connection(Data);
            //Hilfsvariablen
            bool flag_header = false;

            //Variablen
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Repository_Class repository_Element = new Repository_Class();
            ///Überprüfen ob Element Header Element ist (Technisches System, Funktionsbaum, ...)
            XAttribute att_Header = Sys.Attribute("agid");

            //kein Header Element
            if (att_Header != null)
            {
                if (Data.metamodel.m_Header_agid.Contains(att_Header.Value) == false)
                {
                    flag_header = true;
                }
            }
            else
            {
                flag_header = true;
            }

            if (flag_header == true)
            {
                TaggedValue tagged = new TaggedValue(Data.metamodel, Data);

                //StereoType überprüfen
                List<Element_Metamodel> Sys_StereoType = Get_Sys_StereoType_Import(Sys, Data);
             //   XML xml = new XML();
                if (Sys_StereoType != null)
                {
                    //GUID erhalten
                    XAttribute att = Sys.Attribute("uuid");

                    string SYS_GUID = "{" + att.Value + "}";

                    //MessageBox.Show(AFO_GUID);
                    /////////////////////////////////////////////////
                    //Überprüfen ob Element vom Type Class vorhanden
                    string SQL1 = "";

                    //Es wird zunächst überprüft ob Klasse vorhanden
                    List<string> GUIDS = new List<string>();
                    List<string> m_GUID_help = new List<string>();
                    List<string> m_Stereotype_help = new List<string>();
                    m_GUID_help.Add(SYS_GUID);
                    m_Stereotype_help.Add(Sys_StereoType[0].Stereotype);
                    // if (SyStereoType == Data.metamodel.Szenar[0])
                    List<string> m_Type1 = Sys_StereoType.Select(x => x.Type).ToList();


                    GUIDS = repository_Elements.Check_Element_t_object(Data, m_Type1, m_Stereotype_help, m_GUID_help);
                  
                    /*if (Sys_StereoType.Count > 1)
                    {
                        if (Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList().Contains(Sys_StereoType[1].Stereotype) == true)
                        {
                          
                            //GUIDS = repository_Elements.Check_Element_t_object(Data, Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList(), null, m_GUID_help);
                            GUIDS = repository_Elements.Check_Element_t_object(Data, Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList(), null, m_GUID_help);
                        }
                    }
                    else //Szenar oder Capability
                    {
                        List<string> m_Type1 = Data.metamodel.m_Szenar.Select(x => x.Type).ToList();
                        List<string> m_Type2 = Data.metamodel.m_CapConf.Select(x => x.Type).ToList();

                        m_Type1.AddRange(m_Type2);

                        GUIDS = repository_Elements.Check_Element_t_object(Data, m_Type1, m_Stereotype_help, m_GUID_help);

                    }*/

                    List<string> GUIDS2 = repository_Elements.Check_GUID_t_object(Data, SYS_GUID);
                 
                    //vorhanden
                    if (GUIDS != null)
                    {

                        bool flag_update = false;

                        if (Data.sys_xac == true || Data.logical_xac == true || Data.capability_xac == true)
                        {
                            flag_update = true;
                        }
                        else
                        {
                            this.m_GUID_Systemelement_NonUpdate.Add(SYS_GUID);
                        }

                        //Update aller Tagged Values
                        if (flag_update == true)
                        {
                            repository_Element.Classifier_ID = SYS_GUID;

                            loading.label_Progress.Text = repository_Element.Get_Name(Data) + " - Update Sys";
                            loading.label_Progress.Refresh();

                            this.m_GUID_Systemelement_Update.Add(SYS_GUID);

                            Update_Import(Sys, repository, SYS_GUID, false, false, false, Data, Sys_StereoType, Package_Import_Capability_GUID);
                        }
                    }
                    else // Instanz mit der GUID bzw LA ist nicht vorhanden
                    {
                        bool flag_update = false;

                        if (Data.sys_xac == true || Data.logical_xac == true)
                        {
                            flag_update = true;
                        }

                        if (GUIDS2 != null)
                        {
                            MessageBox.Show("Element mit GUID: " + SYS_GUID + " ist im Repository schon vorhanden. Das aktuelle Element wird mit einer neuen GUID angelegt.");
                            TaggedValue tagged2 = new TaggedValue(Data.metamodel, Data);

                            SYS_GUID = tagged2.Generate_GUID("t_object");

                        }

                        //Update aller Tagged Values
                        if (flag_update == true)
                        {

                            loading.label_Progress.Text = "Generate Sys";
                            loading.label_Progress.Refresh();
                            //Aktuelles Package
                            string recent_Package_GUID = "";
                            //if(Sys_StereoType == Data.metamodel.Elements_OpArch[1])
                            if (Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList().Contains(Sys_StereoType[0].Stereotype))
                            {
                                recent_Package_GUID = Package_Import_Sys_GUID;
                            }

                            if (Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList().Contains(Sys_StereoType[0].Stereotype))
                            {
                                recent_Package_GUID = Package_Import_Logical_GUID;
                            }
                            if (Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList().Contains(Sys_StereoType[0].Stereotype))
                            {
                                recent_Package_GUID = Package_Import_Capability_GUID;
                            }
                            if (Data.metamodel.m_Elements_SysArch_Definition.Select(x => x.Stereotype).ToList().Contains(Sys_StereoType[0].Stereotype))
                            {
                                recent_Package_GUID = Package_Import_Sys_GUID;
                            }
                            ///////////////////
                            //Klasse erzeugen
                            //Zuerste den Name des Sys erfahren
                            string SYS_Name = Get_NameValue_Value_xac(Sys, "SYS_KUERZEL");
                            interface_Collection_OleDB.Close_Connection(Data);
                            //Klasse mit Namen anlegen
                            string GUID_Class = repository_Element.Create_Element_Class(SYS_Name, Sys_StereoType[0].Type, Sys_StereoType[0].Stereotype, Sys_StereoType[0].Toolbox, -1, recent_Package_GUID, repository, "", Data);


                            interface_Collection_OleDB.Open_Connection(Data);
                            this.m_GUID_Systemelement_New.Add(SYS_GUID);
                            //GUID abgleichen
                            Interface_Element interface_Element = new Interface_Element();
                            interface_Element.Update_VarChar(GUID_Class, SYS_GUID, "ea_guid", Data);
                            tagged.UPDATE_SQL_t_xref(SYS_GUID, "Client", GUID_Class, repository);
                            Update_Import(Sys, repository, SYS_GUID, false, false, true, Data, Sys_StereoType, Package_Import_Capability_GUID);

                         
                        }

                    }
                }


            }
            else
            {
                //Header Element vorhanden --> soll nicht importiert werden
                //MessageBox.Show("Header Element: "+ att_Header.Value);
            }
          
            interface_Collection_OleDB.Open_Connection(Data);

        }

        private void Import_St(XElement Stakeholder, EA.Repository repository, Database Data, string Package_Import_St_GUID, Loading_OpArch loading)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();
            interface_Collection_OleDB.Close_Connection(Data);
            //Variablen
            Repository_Class repository_Element = new Repository_Class();
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            //StereoType überprüfen
            Element_Metamodel St_ret = Get_St_StereoType_Import(Stakeholder, Data);

            string St_StereoType = St_ret.Stereotype;
            string ST_Type = St_ret.Type;

        
            //GUID erhalten
            XAttribute att = Stakeholder.Attribute("uuid");

            string St_GUID = "{" + att.Value + "}";

            List<string> m_Type_help = new List<string>();
            List<string> m_Stereoype_help = new List<string>();
            List<string> m_GUID_help = new List<string>();

            m_Type_help.Add(ST_Type);
            m_Stereoype_help.Add(St_StereoType);
            m_GUID_help.Add(St_GUID);

            //MessageBox.Show(AFO_GUID);
            //Überprüfen ob Element vom Type Stakeholder vorhanden
            //    string SQL1 = @"SELECT ea_guid FROM t_object WHERE Object_Type = '" + ST_Type + "' AND Stereotype = '" + St_StereoType + "' AND ea_guid = '" + St_GUID + "';";
            //    string xml_Dat = repository.SQLQuery(SQL1);
            //    List<string> GUIDS = xml.Xml_Read_Attribut("ea_guid", xml_Dat);
         //   List<string> GUIDS = repository_Elements.Check_Element_t_object(Data, m_Type_help, m_Stereoype_help, m_GUID_help);
            List<string> GUIDS = repository_Elements.Check_Element_t_object(Data, m_Type_help, null, m_GUID_help);
            List<string> GUIDS2 = repository_Elements.Check_GUID_t_object(Data, St_GUID);
            //string SQL2 = @"SELECT ea_guid FROM t_object WHERE ea_guid = '" + St_GUID + "';";
            //string xml_Dat2 = repository.SQLQuery(SQL2);
            // List<string> GUIDS2 = xml.Xml_Read_Attribut("ea_guid", xml_Dat2);
            //MessageBox.Show(GUIDS.Count.ToString());
            //vorhanden
            if (GUIDS != null)
            {

                bool flag_update = false;

                if (Data.metamodel.m_Stakeholder_Definition.Select(x =>x.Type).ToList().Contains(ST_Type) == true)// && Data.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList().Contains(St_StereoType) == true)
                {
                    flag_update = true;
                }

                //Update aller Tagged Values
                if (flag_update == true)
                {

                    repository_Element.Classifier_ID = St_GUID;

                    loading.label_Progress.Text = repository_Element.Get_Name( Data) + " - Update Stakeholder";
                    loading.label_Progress.Refresh();

                    //Update_Import für Stakeholder überprüfen
                    Update_Import(Stakeholder, repository, St_GUID, false, true, false, Data, null, Package_Import_St_GUID);
                }
            }
            else
            {
                bool flag_update = false;
                bool flag_created_update = false;

                //Stakeholder
                if (Data.stakeholder_xac == true)
                {
                    flag_update = true;
                }

                if (GUIDS2 != null)
                {
                    MessageBox.Show("Element mit GUID: " + St_GUID + " ist im Repository schon vorhanden. Das aktuelle Element wird mit einer neuen GUID angelegt.");
                    TaggedValue tagged2 = new TaggedValue(Data.metamodel, Data);

                    St_GUID = tagged2.Generate_GUID("t_object");

                }

                //Update aller Tagged Values
                if (flag_update == true)
                {
                    List<string> CC_GUIDS = repository_Elements.Get_CapConf_GUID(repository, Data);
                    string CC_GUID = "";
                    // MessageBox.Show("hier3");
                    bool flag_CC_Dec = false;
                    if (CC_GUIDS != null)
                    {
                        List<string> m_Name = Data.metamodel.m_CapConf.Select(x => x.DefaultName).ToList();

                        int i1 = 0;
                        do
                        {
                            repository_Element.Classifier_ID = CC_GUIDS[i1];

                            if (m_Name.Contains( repository_Element.Get_Name( Data)) == true)
                            {
                                flag_CC_Dec = true;
                                CC_GUID = CC_GUIDS[i1];
                                i1 = CC_GUIDS.Count;
                            }

                            i1++;
                        } while (i1 < CC_GUIDS.Count);
                    }
                    //  MessageBox.Show("vorLA");
                    //nicht vorhanden
                    if (flag_CC_Dec == false)
                    {
                        string CC_PAckage_GUID = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[0], repository, Data);
                        /* if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                         {
                             Data.oLEDB_Interface.dbConnection.Close();
                         }*/
                        Interface_Collection interface_Collection_OleDB2 = new Interface_Collection();
                        interface_Collection_OleDB2.Close_Connection(Data);
                        CC_GUID = repository_Element.Create_Element_Class(Data.metamodel.m_CapConf[0].DefaultName, Data.metamodel.m_CapConf[0].Type, Data.metamodel.m_CapConf[0].Stereotype, Data.metamodel.m_CapConf[0].Toolbox, -1, CC_PAckage_GUID, repository, "", Data);
                        /*   if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                           {
                               Data.oLEDB_Interface.dbConnection.Open();
                           }*/
                   //     Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                        interface_Collection_OleDB2.Open_Connection(Data);
                    }


                    loading.label_Progress.Text = "Generierung Stakeholder";
                    loading.label_Progress.Refresh();
                    //Anlegen eines Requirements und der Tagged Values
                    // Stakeholder new_Add = new Stakeholder(St_G);

                    //Zuerste den Name des Sys erfahren
                    string St_Name = Get_NameValue_Value_xac(Stakeholder, "ST_STAKEHOLDER");
                    /*  if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                      {
                          Data.oLEDB_Interface.dbConnection.Close();
                      }*/
                  //  Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                    interface_Collection_OleDB.Close_Connection(Data);
                    string GUID_Class = repository_Element.Create_Element_Class(St_Name, St_ret.Type, St_ret.Stereotype, St_ret.Toolbox, -1, Package_Import_St_GUID, repository, "", Data);
                    /*    if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                        {
                            Data.oLEDB_Interface.dbConnection.Open();
                        }*/
                  //  Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                    interface_Collection_OleDB.Open_Connection(Data);

                    if (Data.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList().Contains(ST_Type))
                    {
                        flag_created_update = true;
                        //Es müssen hier noch nun die ea_guid in allen Tables auf die aus dem Import synchronisiert werden
                        //t_object, t_objectproperties, t_xref....
                        Interface_Element interface_Element = new Interface_Element();
                        interface_Element.Update_VarChar(GUID_Class, St_GUID, "ea_guid", Data);
                       // tagged.UPDATE_SQL_t_object(St_GUID, "ea_guid", GUID_Class, repository);
                        tagged.UPDATE_SQL_t_xref(St_GUID, "Client", GUID_Class, repository);
                    }

                    if (Data.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList().Contains(ST_Type))
                    {
                        flag_created_update = true;
                        /*   if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                           {
                               Data.oLEDB_Interface.dbConnection.Close();
                           }*/
                     //  Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                        interface_Collection_OleDB.Close_Connection(Data);
                        string GUID_Part = repository_Element.Create_Element_Instantiate(St_Name, ST_Type, St_StereoType,St_ret.Toolbox,  GUID_Class, repository.GetElementByGuid(CC_GUID).ElementID, Package_Import_St_GUID, repository, true, Data);
                        /*   if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                           {
                               Data.oLEDB_Interface.dbConnection.Open();
                           }*/
                      //  Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                        interface_Collection_OleDB.Open_Connection(Data);
                        //Es müssen hier noch nun die ea_guid in allen Tables auf die aus dem Import synchronisiert werden
                        //t_object, t_objectproperties, t_xref....
                        Interface_Element interface_Element = new Interface_Element();
                        interface_Element.Update_VarChar(GUID_Part, St_GUID, "ea_guid", Data);
                     //   tagged.UPDATE_SQL_t_object(St_GUID, "ea_guid", GUID_Part, repository);
                        tagged.UPDATE_SQL_t_xref(St_GUID, "Client", GUID_Part, repository);

                    }

                    if (flag_created_update == true)
                    {
                        Update_Import(Stakeholder, repository, St_GUID, false, true, true, Data, null, Package_Import_St_GUID);
                    }

                }

            }
           
            interface_Collection_OleDB.Open_Connection(Data);

        }

        #endregion Import Element

        #region Get Elements
        private List<string> Get_Afo_StereoType_Import(XElement recent, Database Data)
        {


            string W_akt = Get_NameValue_Value_xac(recent, "W_AKTIVITAET");
            List<string> AFo_StereoType = new List<string>();
            string W_funktional = Get_NameValue_Value_xac(recent, "AFO_FUNKTIONAL");
            string W_art = Get_NameValue_Value_xac(recent, "AFO_WV_ART");
            string W_phase = Get_NameValue_Value_xac(recent, "AFO_WV_PHASE");

            if (W_akt != "" && W_funktional == Data.AFO_ENUM.AFO_FUNKTIONAL[(int) AFO_FUNKTIONAL.funktional] && W_art == Data.AFO_ENUM.AFO_WV_ART[(int)AFO_WV_ART.Anforderung])
            {
                switch (W_akt)
                {
                    case "fähig sein":
                        AFo_StereoType = Data.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList();
                        break;
                    case "-":
                        AFo_StereoType = Data.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList();
                        break;
                    case "die Möglichkeit bieten":
                        AFo_StereoType = Data.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList();
                        break;
                    default:
                        AFo_StereoType = Data.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList();
                        break;
                }
            }
            else
            {
                bool t = false;

               if(W_art == Data.AFO_ENUM.AFO_WV_ART[(int)AFO_WV_ART.Systemanforderung])
               {
                    if(W_phase != Data.AFO_ENUM.AFO_WV_PHASE[(int)AFO_WV_PHASE.RollOut])
                    {
                        AFo_StereoType = Data.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList();
                        t = true;
                    }
                    else
                    {
                        AFo_StereoType = Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Stereotype).ToList();
                        t = true;
                    }
                  
               }
               if(t == false && W_art == Data.AFO_ENUM.AFO_WV_ART[(int)AFO_WV_ART.Prozessanforderung])
               {
                    AFo_StereoType = Data.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList();
                    t = true;
               }
                if (t == false && W_art == Data.AFO_ENUM.AFO_WV_ART[(int)AFO_WV_ART.Randbedingung])
                {
                    AFo_StereoType = Data.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList();
                    t = true;
                }
                if (t == false)
                {
                    //!!!!!!!!!!!!!!!!!!// Ändern
                    AFo_StereoType = Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Stereotype).ToList();
                }
            }
           
            return (AFo_StereoType);

        }

        private List<Element_Metamodel> Get_Sys_StereoType_Import(XElement recent, Database Data)
        {


            string Sys_Typ = Get_NameValue_Value_xac(recent, "SYS_TYP");
            string Sys_StereoType = "";

            List<string> m_Sys_SteroType = new List<string>();
            List<Element_Metamodel> m_recent_Element = new List<Element_Metamodel>();
            m_Sys_SteroType.Clear();

            switch (Sys_Typ)
            {
                case "Technisches System":

                    string Sys_KomponentenTyp = Get_NameValue_Value_xac(recent, "SYS_KOMPONENTENTYP");

                    m_recent_Element = Data.metamodel.Find_StereoType_Sys_Komponententyp(Data, Sys_KomponentenTyp);

                    if(m_recent_Element.Count > 0)
                    {
                        Sys_StereoType = m_recent_Element[0].Stereotype;
                    }
                    else
                    {
                        MessageBox.Show("Dem SYS_KOMPONENTENTYP '" + Sys_KomponentenTyp + "' konnte kein Stereotyp aus dem aktuell gewählten Metamodell zugeordnet werden." +
                            " Bitte erweitern Sie Ihr Metamodel, um ein entsprechendes Element mit dem SYS_KOMPONENTENTYP.");
                        m_recent_Element.Add(Data.metamodel.m_Elements_Definition[0]);
                        m_recent_Element.Add(Data.metamodel.m_Elements_Usage[0]);
                    }
                    break;
                case "Szenarbaum":
                    m_recent_Element.Add(Data.metamodel.m_Szenar[0]);
                    break;
                case "Funktionsbaum":
                    m_recent_Element.Add(Data.metamodel.m_Capability[0]);
                    break;
                default:
                    // m_Sys_SteroType = null;
                    m_recent_Element = null;
                    break;


            }



            return (m_recent_Element);
        }

        private Element_Metamodel Get_St_StereoType_Import(XElement recent, Database Data)
        {
         /*   string[] ret = new string[2];

            string W_akt = Get_NameValue_Value_xac(recent, "ST_GRUPPE");
            string St_StereoType = "";
            string St_Type = "";


            //Hier wird noch ein Entscheidungskriterium benötigt
            St_StereoType = Data.metamodel.m_Stakeholder_Definition[0].Stereotype;
            St_Type = Data.metamodel.m_Stakeholder_Definition[0].Type;

            ret[0] = St_Type;
            ret[1] = St_StereoType;
            */

            return (Data.metamodel.m_Stakeholder_Definition[0]);

        }

        private string Get_Klaerungspunkte(XElement recent)
        {
            string ret = "";
           
            var tt = recent.Elements("Klaerungspunkte");

            ret = tt.ToList()[0].Value;



            return (ret);
        }

        private string Get_TagMask(XElement recent)
        {
            string ret = "";

            XAttribute tt = recent.Attribute("tagmask");

            ret = tt.Value;

            return (ret);
        }

        private string Get_NameValue_Value_xac(XElement recent, string Name)
        {
            string ret = "";

            var res = from c in recent.Descendants("Name")
                      where (string)c.Value == Name
                      select c.Parent;


            foreach (var i1 in res)
            {
                // MessageBox.Show(i1.Value);
                // MessageBox.Show(i1.Name.ToString());
                var res2 = from c2 in i1.Descendants("Value")
                           where (XName)c2.Name == "Value"
                           select c2.Value;

                foreach (var i2 in res2) //Gibt es nur einmal in einer XAC Datei
                {
                    //MessageBox.Show(i2);
                    ret = i2;
                }

            }

            return (ret);
        }

        private List<string> Get_NameValue_Value_And_Name(XElement NameValue)
        {
            List<string> ret = new List<string>();

            var res_Name = from c in NameValue.Descendants("Name")
                           where (XName)c.Name == "Name"
                           select c.Value;

            foreach (var i1 in res_Name)
            {
                if (ret.Count == 0)
                {
                    ret.Add(i1);
                }
            }

            var res_Value = from c in NameValue.Descendants("Value")
                            where (XName)c.Name == "Value"
                            select c.Value;

            foreach (var i2 in res_Value)
            {
                if (ret.Count == 1)
                {
                    if (i2 == "" || i2 == null)
                    {
                        ret.Add(" ");
                    }
                    else
                    {
                        ret.Add(i2);
                    }

                }
            }

            if (ret.Count == 1)
            {
                ret.Add(" ");
            }

            return (ret);
        }

        private List<List<string>> Get_Parents_Classifier_Logical(string GUID, EA.Repository repository, Database Data)
        {

            List<string> m_Type_Con = new List<string>();
            List<string> m_Stereotype_Con = new List<string>();

            m_Type_Con = Data.metamodel.m_Decomposition_Element.Select(x => x.Type).ToList();
            m_Stereotype_Con = Data.metamodel.m_Decomposition_Element.Select(x => x.Stereotype).ToList();

            Repository_Element repository_Element = new Repository_Element();
            repository_Element.Classifier_ID = GUID;
            repository_Element.ID = repository_Element.Get_Object_ID(Data);

            List<string> m_GUID = new List<string>();

            string ea_guid = "";
            string ea_guid_Classifier = "";
            List<string> m_ea_guid_Instance = new List<string>();

            bool flag = false;
            //Parent erhalten
           // List<string> m_Stereotype = Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList();

            do
            {
                //PArent der Decomposition erhalten
                Interface_Connectors interface_Connectors = new Interface_Connectors();

               List<DB_Return> m_ret =  interface_Connectors.Get_m_Supplier_From_Client(Data, repository_Element.ID, m_Type_Con, m_Stereotype_Con, Data.metamodel.m_Decomposition_Element[0].direction);

               if(m_ret[0].Ret.Count  == 0)
               {
                    flag = true;
                   // m_GUID.Add(null);
                   // m_ea_guid_Instance.Add(null);
                }
                else
               {
                    m_GUID.Add(m_ret[0].Ret[1].ToString());
                    m_ea_guid_Instance.Add(m_ret[0].Ret[1].ToString());

                    repository_Element.Classifier_ID = m_ret[0].Ret[1].ToString();
                    repository_Element.ID = repository_Element.Get_Object_ID(Data);
                }
                
              /*  repository_Element.Classifier_ID = GUID;
                ea_guid = repository_Element.Get_Parent_GUID(Data);
                string stereo = repository.GetElementByGuid(ea_guid).Stereotype;

                if (ea_guid != null && m_Stereotype.Contains(stereo) == false)
                {
                   
                    ea_guid_Classifier = repository_Element.Get_Classifier(Data);

                    if (ea_guid_Classifier != null)
                    {
                        m_GUID.Add(ea_guid_Classifier);
                        m_ea_guid_Instance.Add(ea_guid);
                    }

                    GUID = ea_guid;
                }
                else
                {
                    flag = true;
                }
              */


            } while (flag == false);

            List<List<string>> ret = new List<List<string>>();
            ret.Add(m_GUID);
            ret.Add(m_ea_guid_Instance);


            return (ret);
        }
     
        #endregion Get Elements

        #region Update Elements
        private void Update_Import(XElement recent, EA.Repository repository, string GUID, bool AFo, bool Stakeholder, bool created, Database Data, List<Element_Metamodel> Sys_StereoType, string GUID_Package)
        {
            Interface_Element interface_Element = new Interface_Element();

            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
          //  XML xml = new XML();

            // MessageBox.Show(Sys_StereoType);

            bool update = false;

            if (created == true)
            {
                update = true;
            }
            else
            {
                //Überprüfung welches Element am aktuellsten ist --> aktuellstes wird imporitiert
                //Element
                #region Prüfung Datum DAtenbank und Import
                EA.Element Element = repository.GetElementByGuid(GUID);

                if(Element != null)
                {
                    string date_Element = Element.Modified.ToString("o");

                    string actualdate_Element = date_Element;
                    if (date_Element.Split('+').Length == 1)
                    {
                        actualdate_Element = actualdate_Element + "+02:00";
                    }
                    DateTime modified_date = DateTime.ParseExact(actualdate_Element, "o", System.Globalization.CultureInfo.InvariantCulture);

                    //Import
                    string import_modified = recent.Attribute("modified").Value;

                    //WErt von Require 7 korrigieren
                    if (import_modified.Split('+')[0].Length != 27)
                    {
                        string help = import_modified.Split('+')[0];

                        if (import_modified.Split('+')[0].Length < 27)
                        {
                            //verlängern
                            do
                            {
                                help = help + "0";

                            } while (help.Length < 27);
                        }
                        else
                        {
                            //verkürzen
                            do
                            {
                                help = help.Remove(help.Length - 1, 1);

                            } while (help.Length > 27);

                        }

                        // MessageBox.Show(help + "+" + import_modified.Split('+')[1]);

                        import_modified = help;
                    }
                    // MessageBox.Show(import_modified.Split('+')[0].Length.ToString());

                    DateTime import_datetime = DateTime.ParseExact(import_modified, "o", System.Globalization.CultureInfo.InvariantCulture);

                    //MessageBox.Show("Import: " + import_datetime.ToString("o") + "\nDatabase: " + modified_date.ToString("o"));


                    int result = DateTime.Compare(import_datetime, modified_date);

                    if (result > 0)
                    {
                        update = true;
                        // MessageBox.Show("Import: " + import_datetime.ToString("o")+"\nDatabase: "+ modified_date.ToString("o"));
                    }
                    else
                    {
                        //UPDATE Afo_ID

                        this.m_GUID_Requirement_NonUpdate.Add(GUID);
                    }
                }
                else
                {
                    update = true;
                }

               

                //MessageBox.Show("Import: "+import_modified+"\nDatabase: "+ actualdate_Element);
                #endregion Prüfung Datum DAtenbank und Import
            }

            var res_NameValue = from c in recent.Descendants("NameValue")
                                where (XName)c.Name == "NameValue"
                                select c;

            /////////////////////////
            //Es muss immer  Object ID für die Konnektoren geupdatet werden
            #region Update OBJECT_ID
            var res_Object_ID = from d in recent.Descendants("Name")
                                where (XName)d.Name == "OBJECT_ID"
                                select d.Parent;

            //  MessageBox.Show("hier1");
            if(res_Object_ID.Count() == 0)
            {
                string objectid = recent.Attribute("objectid").Value;
                tagged.Update_Tagged_Value(GUID, "OBJECT_ID", objectid, null, repository);
            }

            foreach (var i3 in res_Object_ID)
            {
                List<string> NameValue_1 = Get_NameValue_Value_And_Name(i3);

                if (NameValue_1[0] == "OBJECT_ID")
                {
                    if(NameValue_1[1] != null && NameValue_1[1] != "")
                    {
                        tagged.Update_Tagged_Value(GUID, NameValue_1[0], NameValue_1[1], null, repository);
                    }
                    else
                    {
                        string objectid = recent.Attribute("objectid").Value;
                        tagged.Update_Tagged_Value(GUID, "OBJECT_ID", objectid, null, repository);
                    }
                   
                }

            }
            #endregion Update OBJECT_ID

            #region Update GUID
            string uuid = recent.Attribute("uuid").Value;
            if(uuid != null)
            {
                tagged.Update_Tagged_Value(GUID, "UUID", uuid, null, repository);
            }

            #endregion GUID
            //   MessageBox.Show("hier2");
            #region Klaerungspunkte

            #endregion 
            ///Afo
            #region AFo Updaten
            if (AFo == true && update == true)
            { 


                if(this.m_GUID_Requirement_New.Contains(GUID) == false)
                {
                    this.m_GUID_Requirement_Update.Add(GUID);
                }
                List<DB_Insert> m_Insert_TV = new List<DB_Insert>();
                Requirement requirement = new Requirement(GUID, Data.metamodel);
                requirement.ID = requirement.Get_Object_ID(Data);
                requirement.Author = requirement.Get_Author(Data);
                requirement.Notes = requirement.Get_Notes(Data);
                requirement.Name = requirement.Get_Name(Data);
                //Modified Datum einer Anforderungen ändern
                requirement.Update_ModifiedDate(DateTime.Now, repository);



                #region Klaerungspunkte
                //string klaerungspunkte = recent.Attribute("Klaerungspunkte").Value;
                requirement.AFO_KLAERUNGSPUNKTE = this.Get_Klaerungspunkte(recent);

                //Issue erzeugen
                if (requirement.AFO_KLAERUNGSPUNKTE != "")
                {
                    Issue issue = new Issue(Data, requirement.AFO_KLAERUNGSPUNKTE, requirement.AFO_KLAERUNGSPUNKTE, GUID_Klaerungspunkte, repository, true, Data.metamodel.m_Issue[4].Stereotype);
               //     issue.C
                    Interface_Connectors interface_Connectors = new Interface_Connectors();
                    interface_Connectors.Create_Connector(issue.Classifier_ID, GUID, Data.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, Data, Data.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], Data.metamodel.m_Con_Trace[0].direction);
                }

                m_Insert_TV.Add(new DB_Insert("AFO_KLAERUNGSPUNKTE", OleDbType.VarChar, OdbcType.VarChar, requirement.AFO_KLAERUNGSPUNKTE, -1));
                #endregion
                #region TagMask
                requirement.TagMask = this.Get_TagMask(recent);

                m_Insert_TV.Add(new DB_Insert("TagMask", OleDbType.VarChar, OdbcType.VarChar, requirement.TagMask, -1));

                #endregion 
                foreach (var i1 in res_NameValue)
                {
                    List<string> NameValue = Get_NameValue_Value_And_Name(i1);

                    //MessageBox.Show(NameValue[00] + " " + NameValue[1]);
                    // c
                    //PUID nicht updaten
                    if (NameValue[0] != "PUID")
                    {

                        //MessageBox.Show(NameValue[0] + " " + NameValue[1]);

                      //  tagged.Update_Tagged_Value(GUID, NameValue[0], NameValue[1], null, repository);

                        m_Insert_TV.Add(new DB_Insert(NameValue[0], OleDbType.VarChar, OdbcType.VarChar, NameValue[1], -1));
                      
                        string recent_str = NameValue[1];

                        //Bei einigen Tags muss dies noch an anderer Stelle im Object upgedatet werden
                        switch (NameValue[0])
                        {
                            case "AFO_TITEL":
                                requirement.AFO_TITEL = recent_str;
                                requirement.Name = recent_str;
                                requirement.Update_Name(repository, recent_str);
                                interface_Element.Update_VarChar(GUID, NameValue[1], "Name", Data);
                              //  tagged.UPDATE_SQL_t_object(NameValue[1], "Name", GUID, repository);
                                //  MessageBox.Show("Check");"
                                break;
                            case "AFO_TEXT":
                                requirement.AFO_TEXT = recent_str;
                                requirement.Update_Notes(repository, recent_str);
                                interface_Element.Update_VarChar(GUID, NameValue[1], "Note", Data);
                             //   tagged.UPDATE_SQL_t_object(NameValue[1], "Note", GUID, repository);
                                //tagged.Update_Tagged_Value(GUID, "AFO_TEXT", NameValue[1], null, repository);
                                break;
                            case "AFO_ABNAHMEKRITERIUM":
                                requirement.AFO_ABNAHMEKRITERIUM = recent_str;
                                break;
                            case "AFO_AG_ID":
                                requirement.AFO_AG_ID = recent_str;
                                break;
                            case "AFO_AN_ID":
                                requirement.AFO_AN_ID = recent_str;
                                break;
                            case "AFO_ANSPRECHPARTNER":
                                requirement.AFO_ANSPRECHPARTNER = recent_str;
                                break;
                            case "AFO_BEZUG":
                                requirement.AFO_BEZUG = (AFO_BEZUG)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_BEZUG, recent_str);
                                break;
                            case "AFO_CPM_PHASE":
                                requirement.AFO_CPM_PHASE = (AFO_CPM_PHASE)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_CPM_PHASE, recent_str);
                                break;
                            case "AFO_DETAILSTUFE":
                                requirement.AFO_DETAILSTUFE = (AFO_DETAILSTUFE)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_DETAILSTUFE, recent_str);
                                break;
                            case "AFO_FUNKTIONAL":
                                requirement.AFO_FUNKTIONAL = (AFO_FUNKTIONAL)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_FUNKTIONAL, recent_str);
                                break;
                            case "AFO_HINWEIS":
                                requirement.AFO_HINWEIS = recent_str;
                                break;
                            case "AFO_KLAERUNGSPUNKTE":
                                requirement.AFO_KLAERUNGSPUNKTE = recent_str;
                                break;
                            case "AFO_KRITIKALITAET":
                                requirement.AFO_KRITIKALITAET = (AFO_KRITIKALITAET)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_KRITIKALITAET, recent_str);
                                break;
                            case "AFO_OPERATIVEBEWERTUNG":
                                requirement.AFO_OPERATIVEBEWERTUNG = (AFO_OPERATIVEBEWERTUNG)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_OPERATIVEBEWERTUNG, recent_str);
                                break;
                            case "AFO_PRIORITAET_VERGABE":
                                requirement.AFO_PRIORITAET_VERGABE = (AFO_PRIORITAET_VERGABE)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_PRIORITAET_VERGABE, recent_str);
                                break;
                            case "AFO_PROJEKTROLLE":
                                requirement.AFO_PROJEKTROLLE = (AFO_PROJEKTROLLE)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_PROJEKTROLLE, recent_str);
                                break;
                            case "AFO_QS_STATUS":
                                requirement.AFO_QS_STATUS = (AFO_QS_STATUS)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_QS_STATUS, recent_str);
                                break;
                            case "AFO_QUELLTEXT":
                                requirement.AFO_QUELLTEXT = recent_str;
                                break;
                            case "AFO_REGELUNGEN":
                                requirement.AFO_REGELUNGEN = recent_str;
                                break;
                            case "AFO_STATUS":
                                requirement.AFO_STATUS = (AFO_STATUS)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_STATUS, recent_str);
                                break;
                            case "AFO_VERERBUNG":
                                requirement.AFO_VERERBUNG = (AFO_VERERBUNG)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_VERERBUNG, recent_str);
                                break;
                            case "AFO_WV_ART":
                                requirement.AFO_WV_ART = (AFO_WV_ART)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_WV_ART, recent_str);
                                break;
                            case "AFO_WV_NACHWEISART":
                                requirement.AFO_WV_NACHWEISART = (AFO_WV_NACHWEISART)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_WV_NACHWEISART, recent_str);
                                break;
                            case "AFO_WV_PHASE":
                                requirement.AFO_WV_PHASE = (AFO_WV_PHASE)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.AFO_WV_PHASE, recent_str);
                                break;
                            case "B_BEMERKUNG":
                                requirement.B_BEMERKUNG = recent_str;
                                break;
                            case "CLARIFICATION":
                                requirement.CLARIFICATION = recent_str;
                                break;
                            case "DB_Stand":
                                requirement.DB_Stand = recent_str;
                                break;
                            case "IN_CATEGORY":
                                requirement.IN_CATEGORY = (IN_CATEGORY)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.IN_CATEGORY, recent_str);
                                break;
                            case "OBJECT_ID":
                                requirement.OBJECT_ID = recent_str;
                                break;
                            case "UUID":
                                requirement.UUID = recent_str;
                                break;
                            case "W_AFO_MANUAL":
                                if (recent_str == "true")
                                {
                                    requirement.W_AFO_MANUAL = true;
                                }
                                else
                                {
                                    requirement.W_AFO_MANUAL = false;
                                }
                                break;
                            case "W_AKTIVITAET":
                                requirement.W_AKTIVITAET = (W_AKTIVITAET)Data.AFO_ENUM.Get_Index(Data.AFO_ENUM.W_AKTIVITAET, recent_str);
                                break;
                            case "W_FREEZE_TITLE":
                                if (recent_str == "true")
                                {
                                    requirement.W_FREEZE_TITLE = true;
                                }
                                else
                                {
                                    requirement.W_FREEZE_TITLE = false;
                                }
                                break;
                            case "W_NUTZENDER":
                                requirement.W_NUTZENDER = recent_str;
                                break;
                            case "W_OBJEKT":
                                requirement.W_OBJEKT = recent_str;
                                break;
                            case "W_QUALITAET":
                                requirement.W_QUALITAET = recent_str;
                                break;
                            case "W_RANDBEDINGUNG":
                                requirement.W_RANDBEDINGUNG = recent_str;
                                break;
                            case "W_SINGULAR":
                                if (recent_str == "true")
                                {
                                    requirement.W_SINGULAR = true;
                                }
                                else
                                {
                                    requirement.W_SINGULAR = false;
                                }
                                break;
                            case "W_SUBJEKT":
                                requirement.W_SUBJEKT = recent_str;
                                break;
                            case "W_ZU":
                                if (recent_str == "true")
                                {
                                    requirement.W_ZU = true;
                                }
                                else
                                {
                                    requirement.W_ZU = false;
                                }
                                break;
                            case "AFO_VERBINDLICHKEIT":
                                requirement.AFO_VERBINDLICHKEIT = recent_str;
                                break;

                        }
                    }
                }

                if(m_Insert_TV.Count > 0)
                {
                    requirement.Update_TV(m_Insert_TV, Data, repository);

                  
                }
            }
            #endregion AFo Updaten
            /////////////////////////////
            //Sys (Technisches Element, Szenarbaum, Funktionsbaum)
            #region Sys Update
            if (AFo == false && update == true && Stakeholder == false)
            {
                //string GUID_Class = "";

                // if(Sys_StereoType == Data.metamodel.Elements_OpArch[1])
                //if(Data.metamodel.Elements_Class.Contains(Sys_StereoType))
                //  {
                if (Data.Get_Element_Classifier(GUID, repository, Data) != null)
                {
                    string GUID_Class = Data.Get_Element_Classifier(GUID, repository, Data)[0];

                    //  }

                    if (Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList().Contains(Sys_StereoType[0].Stereotype) == true || Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList().Contains(Sys_StereoType[0].Stereotype))
                    {
                        GUID_Class = GUID;
                        //Update des Stereotype bei Szenarbaum
                        if (created == false)
                        {
                            interface_Element.Update_VarChar(GUID_Class, Sys_StereoType[0].Stereotype, "Stereotype", Data);
                          //  tagged.UPDATE_SQL_t_object(Sys_StereoType[0].Stereotype, "Stereotype", GUID_Class, repository);
                        }
                    }
                    else
                    {
                        //Update des Stereotype bei Technischem System
                        if (created == false)
                        {
                            interface_Element.Update_VarChar(GUID, Sys_StereoType[1].Stereotype, "Stereotype", Data);
                            interface_Element.Update_VarChar(GUID_Class, Sys_StereoType[0].Stereotype, "Stereotype", Data);

                         //   tagged.UPDATE_SQL_t_object(Sys_StereoType[1].Stereotype, "Stereotype", GUID, repository);
                         //   tagged.UPDATE_SQL_t_object(Sys_StereoType[0].Stereotype, "Stereotype", GUID_Class, repository);
                        }
                    }


                    //MessageBox.Show("GUID_Class: "+GUID_Class+"\nGUID: "+GUID);

                    List<DB_Insert> m_Insert_Sys = new List<DB_Insert>();
                    List<DB_Insert> m_Insert_Sys_Class = new List<DB_Insert>();


                    foreach (var i1 in res_NameValue)
                    {
                        List<string> NameValue = Get_NameValue_Value_And_Name(i1);

                        //                    MessageBox.Show(NameValue[00] + " " + NameValue[1]);
                        // c
                        //PUID nicht updaten
                        if (NameValue[0] != "PUID")
                        {

                            //MessageBox.Show(NameValue[0] + " " + NameValue[1]);
                               if (NameValue[0] != "UUID" && NameValue[0] != "SYS_AG_ID" && NameValue[0] != "OBJECT_ID" && NameValue[0] != "SYS_AN_ID")
                               {
                                   if (NameValue.Count > 1)
                                   {
                                    //   tagged.Update_Tagged_Value(GUID_Class, NameValue[0], NameValue[1], null, repository);
                                    m_Insert_Sys_Class.Add(new DB_Insert(NameValue[0], OleDbType.VarChar, OdbcType.VarChar, NameValue[1], -1));
                                   }
                                   else
                                   {
                                    //tagged.Update_Tagged_Value(GUID_Class, NameValue[0], " ", null, repository);
                                    m_Insert_Sys_Class.Add(new DB_Insert(NameValue[0], OleDbType.VarChar, OdbcType.VarChar, " ", -1));
                                   }

                               }
                               else
                                {
                                   m_Insert_Sys.Add(new DB_Insert(NameValue[0], OleDbType.VarChar, OdbcType.VarChar, NameValue[1], -1));
                                }
                          /*     else
                               {
                                   // if(created == true)
                                   // {
                                   //     tagged.Update_Tagged_Value(GUID_Class, NameValue[0], NameValue[1], null, repository);
                                   // }
                                   // else
                                   // {
                                  tagged.Update_Tagged_Value(GUID, NameValue[0], NameValue[1], null, repository);

                                   // }
                               }
                               */
                           

                            //    MessageBox.Show("nachupdate");
                            //Bei einigen Tags muss dies noch an anderer Stelle im Object und Klasse upgedatet werden
                            switch (NameValue[0])
                            {
                                case "SYS_KUERZEL":
                                    interface_Element.Update_VarChar(GUID, NameValue[1], "Name", Data);
                                //    tagged.UPDATE_SQL_t_object(NameValue[1], "Name", GUID, repository);
                                    if (GUID != GUID_Class)
                                    {
                                        interface_Element.Update_VarChar(GUID_Class, NameValue[1], "Name", Data);
                                      //  tagged.UPDATE_SQL_t_object(NameValue[1], "Name", GUID_Class, repository);
                                    }
                                    //  MessageBox.Show("Check");
                                    break;
                                case "SYS_BEZEICHNUNG":
                                    interface_Element.Update_VarChar(GUID, NameValue[1], "Note", Data);
                                   // tagged.UPDATE_SQL_t_object(NameValue[1], "Note", GUID, repository);
                                    if (GUID != GUID_Class)
                                    {
                                        interface_Element.Update_VarChar(GUID_Class, NameValue[1], "Note", Data);
                                      //  tagged.UPDATE_SQL_t_object(NameValue[1], "Note", GUID_Class, repository);
                                    }
                                    break;

                            }
                        }
                    }

                    NodeType Class = new NodeType(null, repository, Data);
                    Class.Classifier_ID = GUID_Class;


                    #region Klaerungspunkte
                    //string klaerungspunkte = recent.Attribute("Klaerungspunkte").Value;
                    Class.AFO_KLAERUNGSPUNKTE = this.Get_Klaerungspunkte(recent);
                    //Issue erzeugen
                    if (Class.AFO_KLAERUNGSPUNKTE != "")
                    {
                        Issue issue = new Issue(Data, Class.AFO_KLAERUNGSPUNKTE, Class.AFO_KLAERUNGSPUNKTE, GUID_Klaerungspunkte, repository, true, Data.metamodel.m_Issue[4].Stereotype);
                        Interface_Connectors interface_Connectors = new Interface_Connectors();
                        interface_Connectors.Create_Connector(issue.Classifier_ID, GUID_Class, Data.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, Data, Data.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], Data.metamodel.m_Con_Trace[0].direction);
                    }

                    m_Insert_Sys.Add(new DB_Insert("AFO_KLAERUNGSPUNKTE", OleDbType.VarChar, OdbcType.VarChar, Class.AFO_KLAERUNGSPUNKTE, -1));
                    #endregion
                    #region TagMask
                    Class.TagMask = this.Get_TagMask(recent);

                    m_Insert_Sys.Add(new DB_Insert("TagMask", OleDbType.VarChar, OdbcType.VarChar, Class.TagMask, -1));

                    #endregion
                    Class.ID = Class.Get_Object_ID(Data);
                  /*  Class.Author = Class.Get_Author(Data);
                    Class.Notes = Class.Get_Notes(repository, Class.Classifier_ID, Data);
                    Class.Name = Class.Get_Name(Data);*/

                    Class.Update_TV(m_Insert_Sys_Class, Data, repository);


                    NodeType obj = new NodeType(null, repository, Data);
                    obj.Classifier_ID = GUID;

                    if (m_Insert_Sys.Where(x => x.Property == "UUID").ToList().Count == 0)
                    {
                        string UUID = obj.Classifier_ID;
                        UUID = UUID.Trim('{', '}');
                        m_Insert_Sys.Add(new DB_Insert("UUID", OleDbType.VarChar, OdbcType.VarChar, UUID, -1));
                    }

                    if (GUID_Class != GUID)
                    {
                        obj.ID = obj.Get_Object_ID(Data);
                     /*   obj.Author = obj.Get_Author(Data);
                        obj.Notes = obj.Get_Notes(repository, obj.Classifier_ID, Data);
                        obj.Name = obj.Get_Name(Data);*/
                       

                        obj.Update_TV(m_Insert_Sys, Data, repository);
                    }
                    else
                    {
                        Class.Update_TV(m_Insert_Sys, Data, repository);
                    }


                }
            }
            #endregion Sys Update

            //Stakeholder updaten
            #region Stakeholder
            if (Stakeholder == true && update == true)
            {
                if (Data.Get_Element_Classifier(GUID, repository, Data) != null)
                {
                    List<DB_Insert> m_Insert_St = new List<DB_Insert>();
                    List<DB_Insert> m_Insert_St_Class = new List<DB_Insert>();

                    string GUID_Class = Data.Get_Element_Classifier(GUID, repository, Data)[0];

                    foreach (var i1 in res_NameValue)
                    {
                        List<string> NameValue = Get_NameValue_Value_And_Name(i1);

                        //                    MessageBox.Show(NameValue[00] + " " + NameValue[1]);
                        // c
                        //PUID nicht updaten
                        if (NameValue[0] != "PUID")
                        {

                            //MessageBox.Show(NameValue[0] + " " + NameValue[1]);
                            if (NameValue[0] != "UUID" && NameValue[0] != "ST_AG_ID" && NameValue[0] != "OBJECT_ID" && NameValue[0] != "ST_AN_ID")
                            {
                                if (NameValue.Count > 1)
                                {
                                  //  tagged.Update_Tagged_Value(GUID_Class, NameValue[0], NameValue[1], null, repository);
                                    m_Insert_St_Class.Add(new DB_Insert(NameValue[0], OleDbType.VarChar, OdbcType.VarChar, NameValue[1], -1));
                                }
                                else
                                {
                                   // tagged.Update_Tagged_Value(GUID_Class, NameValue[0], " ", null, repository);
                                    m_Insert_St_Class.Add(new DB_Insert(NameValue[0], OleDbType.VarChar, OdbcType.VarChar, " ", -1));
                                }
                            }
                            else
                            {
                                // if(created == true)
                                // {
                                //     tagged.Update_Tagged_Value(GUID_Class, NameValue[0], NameValue[1], null, repository);
                                // }
                                // else
                                // {
                                //tagged.Update_Tagged_Value(GUID, NameValue[0], NameValue[1], null, repository);
                                m_Insert_St.Add(new DB_Insert(NameValue[0], OleDbType.VarChar, OdbcType.VarChar, NameValue[1], -1));

                                // }
                            }

                            //    MessageBox.Show("nachupdate");
                            //Bei einigen Tags muss dies noch an anderer Stelle im Object und Klasse upgedatet werden
                            switch (NameValue[0])
                            {
                                case "ST_STAKEHOLDER":
                                    interface_Element.Update_VarChar(GUID, NameValue[1], "Name", Data);
                                  //  tagged.UPDATE_SQL_t_object(NameValue[1], "Name", GUID, repository);
                                    if (GUID != GUID_Class)
                                    {
                                        interface_Element.Update_VarChar(GUID_Class, NameValue[1], "Name", Data);
                                      //  tagged.UPDATE_SQL_t_object(NameValue[1], "Name", GUID_Class, repository);
                                    }
                                    //  MessageBox.Show("Check");
                                    break;
                                case "ST_BESCHREIBUNG":
                                    interface_Element.Update_VarChar(GUID, NameValue[1], "Note", Data);
                                 //   tagged.UPDATE_SQL_t_object(NameValue[1], "Note", GUID, repository);
                                    if (GUID != GUID_Class)
                                    {
                                        interface_Element.Update_VarChar(GUID_Class, NameValue[1], "Note", Data);
                                      //  tagged.UPDATE_SQL_t_object(NameValue[1], "Note", GUID_Class, repository);
                                    }
                                    break;

                            }
                        }
                    }

                  

                    Stakeholder Class = new Stakeholder(null, repository, Data);
                    Class.Classifier_ID = GUID_Class;

                    Class.ID = Class.Get_Object_ID(Data);
                     Class.Author = Class.Get_Author(Data);
                      Class.Notes = Class.Get_Notes(Data);
                      Class.Name = Class.Get_Name(Data);

                    #region Klaerungspunkte
                    //string klaerungspunkte = recent.Attribute("Klaerungspunkte").Value;
                    Class.AFO_KLAERUNGSPUNKTE = this.Get_Klaerungspunkte(recent);
                    //Issue erzeugen
                    if (Class.AFO_KLAERUNGSPUNKTE != "")
                    {
                        Issue issue = new Issue(Data, Class.AFO_KLAERUNGSPUNKTE, Class.AFO_KLAERUNGSPUNKTE, GUID_Klaerungspunkte, repository, true, Data.metamodel.m_Issue[4].Stereotype);
                        Interface_Connectors interface_Connectors = new Interface_Connectors();
                        interface_Connectors.Create_Connector(issue.Classifier_ID, GUID_Class, Data.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, Data, Data.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], Data.metamodel.m_Con_Trace[0].direction);
                    }

                    m_Insert_St_Class.Add(new DB_Insert("AFO_KLAERUNGSPUNKTE", OleDbType.VarChar, OdbcType.VarChar, Class.AFO_KLAERUNGSPUNKTE, -1));
                    #endregion
                    #region TagMask
                    Class.TagMask = this.Get_TagMask(recent);

                    m_Insert_St_Class.Add(new DB_Insert("TagMask", OleDbType.VarChar, OdbcType.VarChar, Class.TagMask, -1));

                    #endregion


                    Class.Update_TV(m_Insert_St_Class, Data, repository);

                    Stakeholder obj = new Stakeholder(null, repository, Data);
                    obj.Classifier_ID = GUID;
                    if (m_Insert_St.Where(x => x.Property == "UUID").ToList().Count == 0)
                    {

                        string UUID = obj.Classifier_ID;
                        UUID = UUID.Trim('{', '}');
                        m_Insert_St.Add(new DB_Insert("UUID", OleDbType.VarChar, OdbcType.VarChar, UUID, -1));
                    }

                    if (GUID_Class != GUID)
                    {

                        obj.ID = obj.Get_Object_ID(Data);
                        /*   obj.Author = obj.Get_Author(Data);
                           obj.Notes = obj.Get_Notes(repository, obj.Classifier_ID, Data);
                           obj.Name = obj.Get_Name(Data);*/
                  

                        obj.Update_TV(m_Insert_St, Data, repository);
                    }
                    else
                    {
                        Class.Update_TV(m_Insert_St, Data, repository);
                    }
                }
            }
            #endregion Stakeholder
            //
            //  MessageBox.Show("durch");
        }

        #endregion Update Elements

        #region Import Links

        private void Import_Link(XElement recent, Database Data, EA.Repository repository, Loading_OpArch loading)
        {

            /////////////
            ///Source erhalten
            var source = from e in recent.Descendants("source")
                         select e;

            XElement[] source_array = source.ToArray();

            /////////////
            ///Target erhalten
            var target = from f in recent.Descendants("target")
                         select f;

            XElement[] target_array = target.ToArray();
            ///////////
            //MessageBox.Show(recent.Attribute("name").Value);
            ///Welcher Fall liegt vor?
            ////Sys --> Sys
            if (recent.Attribute("name").Value == this.Metamodel_Base.m_Connectoren_Req7[5] && Data.link_decomposition == true)
            {
                Import_Link_Sys_Sys(source_array[0], target_array[0], repository, Data);
            }
            ////Afo --> Sys
            if (recent.Attribute("name").Value == this.Metamodel_Base.m_Connectoren_Req7[6] && Data.link_afo_sys == true)
            {
                Import_Link_Afo_Sys(source_array[0], target_array[0], repository, Data);
            }
            ////Afo --> St
            if (recent.Attribute("name").Value == this.Metamodel_Base.m_Connectoren_Req7[7] && Data.link_afo_st == true)
            {
                Import_Link_Afo_St(source_array[0], target_array[0], repository, Data);
            }
            ////Afo --> Afo --> send bei Schnittstellen
            

            if (this.Metamodel_Base.m_Connectoren_Req7_Afo.Contains(recent.Attribute("name").Value) == true && Data.link_afo_afo == true)
            {
                Import_Link_Afo_Afo(source_array[0], target_array[0], repository, Data, recent.Attribute("name").Value);
            }

        }

        private void Import_Link_Sys_Sys(XElement source, XElement target, EA.Repository repository, Database Data)
        {
            bool flag_check = false;
            //DB_Command sQL_Command = new DB_Command();
            Repository_Class repository_child = new Repository_Class();
            Repository_Class repository_parent = new Repository_Class();
            Repository_Connector repository_Connector = new Repository_Connector();
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
           // List<string> m_Type = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Type = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            // XML xML = new XML();
            ////////////////////////////
            //Object ID erhalten
            //source
            var source_Object_ID = from a in source.Descendants("objectid")
                                   select a.Value;

            string[] source_Object_ID_array = source_Object_ID.ToArray();
            //target
            var target_Object_ID = from b in target.Descendants("objectid")
                                   select b.Value;

            string[] target_Object_ID_array = target_Object_ID.ToArray();
            ///////////////////////////
            //Abfrage, ob Elemente exisitieren --> TaggedValues Value abfragen "OBJECT_ID"
            int source_Rep_ID = tagged.Get_ID_From_Value(source_Object_ID_array[0], "OBJECT_ID", repository);
            int target_Rep_ID = tagged.Get_ID_From_Value(target_Object_ID_array[0], "OBJECT_ID", repository);

            //MessageBox.Show("source_Rep_ID: " + source_Rep_ID + "\nsource_Object_ID: " + source_Object_ID_array[0] + "\ntarget_Rep_ID: " + target_Rep_ID + "\ntarget_Object_ID: " + target_Object_ID_array[0]);

            if (source_Rep_ID != -1 && target_Rep_ID != -1)
            {
                //Das gilt hier nur für Technische Systeme diese Hierachie für Szenarbaum und Funktionsbaum gelten andere Dinge --> Erzeugung Taxonomy 
                //ElternKindBeziehung festlegen; Beide Elemente müssen "Part" sein
                //ParentID von Target auf OBject_ID des Parent festlegen
                string source_GUID_EA = tagged.Get_GUID_From_ID(source_Rep_ID, repository);
                string target_GUID_EA = tagged.Get_GUID_From_ID(target_Rep_ID, repository);
                //Technisches System --> Part
                if (m_Type.Contains(repository.GetElementByGuid(source_GUID_EA).Type) && m_Type.Contains( repository.GetElementByGuid(target_GUID_EA).Type))
                {
                    //Funktionsbaum
                    if (Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList().Contains(repository.GetElementByGuid(source_GUID_EA).Stereotype) == true && Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList().Contains(repository.GetElementByGuid(target_GUID_EA).Stereotype) == true)
                    {
                        //Requirement help = new Requirement();
                        //help.Classifier_ID = target_GUID_EA;
                        List<string> m_Taxonomy_Type = Data.metamodel.m_Taxonomy_Capability.Select(x => x.Type).ToList();
                        List<string> m_Taxonomy_Stereotype = Data.metamodel.m_Taxonomy_Capability.Select(x => x.Stereotype).ToList();

                        var m_help_guid = repository_Connector.Check_Dependency(target_GUID_EA, source_GUID_EA, m_Taxonomy_Stereotype, m_Taxonomy_Type, Data, Data.metamodel.m_Taxonomy_Capability[0].direction);

                        if (m_help_guid == null)
                        {
                            flag_check = true;
                        }

                        //help.Create_Dependency(source_GUID_EA, Data.metamodel.StereoType_Capability_Taxonomy[1], Data.metamodel.StereoType_Capability_Taxonomy[0], repository);
                        var help_guid = repository_Connector.Create_Dependency(target_GUID_EA, source_GUID_EA, m_Taxonomy_Stereotype, m_Taxonomy_Type, Data.metamodel.m_Taxonomy_Capability[0].SubType, repository, Data, Data.metamodel.m_Taxonomy_Capability[0].Toolbox, Data.metamodel.m_Taxonomy_Capability[0].direction);


                       // if (help_guid != null)
                        //{
                            this.m_GUID_Funktionsbaum_Connector_Import.Add(help_guid);
                        //} 
                        if(flag_check == true)
                        {
                            this.m_GUID_Funktionsbaum_Connector_Import_new.Add(help_guid);
                        }

                    }
                    else
                    {
                        //Connector_Dekomposition anlegen
                        List<string> m_Decomposition_Type = Data.metamodel.m_Decomposition_Element.Select(x => x.Type).ToList();
                        List<string> m_Decomposition_Stereotype = Data.metamodel.m_Decomposition_Element.Select(x => x.Stereotype).ToList();
                        //help.Create_Dependency(source_GUID_EA, Data.metamodel.StereoType_Capability_Taxonomy[1], Data.metamodel.StereoType_Capability_Taxonomy[0], repository);
                        var m_help_guid = repository_Connector.Check_Dependency(target_GUID_EA, source_GUID_EA, m_Decomposition_Stereotype, m_Decomposition_Type, Data, Data.metamodel.m_Decomposition_Element[0].direction);

                        if (m_help_guid == null)
                        {
                            flag_check = true;
                        }

                        var help_guid =  repository_Connector.Create_Dependency(target_GUID_EA, source_GUID_EA, m_Decomposition_Stereotype, m_Decomposition_Type, Data.metamodel.m_Decomposition_Element[0].SubType, repository, Data, Data.metamodel.m_Decomposition_Element[0].Toolbox, Data.metamodel.m_Decomposition_Element[0].direction);
                        //Part anlegen unter ParentElement
                        //Benötigte Attribute für Erzeugung PArt erhalten
                        this.m_GUID_System_Connector_Import.Add(help_guid);
                        if (flag_check == true)
                        {
                            this.m_GUID_System_Connector_Import_new.Add(help_guid);
                        }

                        repository_child.Classifier_ID = target_GUID_EA;
                        repository_child.Name = repository_child.Get_Name(Data);
                        repository_parent.Classifier_ID = source_GUID_EA;
                        repository_parent.ID = repository_parent.Get_Object_ID(Data);
                        //Stereotype erhalten
                        int help = Data.metamodel.m_Elements_Definition.FindIndex(x => x.Stereotype == repository_child.Get_Stereotype(Data));
                        if (help != -1)
                        {
                            Element_Metamodel child_usage = Data.metamodel.m_Elements_Usage[help];
                            //Instanz anlegen
                            Interface_Element interface_Element = new Interface_Element();
                            string help_check = interface_Element.Check_Database_Element_Instantiate(Data, child_usage.Type, child_usage.Stereotype, repository_parent.ID, target_GUID_EA);

                            string instantiate_GUID = repository_child.Create_Element_Instantiate(repository_child.Name, child_usage.Type, child_usage.Stereotype, child_usage.Toolbox, target_GUID_EA, repository_parent.ID, repository.GetPackageByID(repository_parent.Get_Package_ID(Data)).PackageGUID, repository, false, Data);

                            //Issue neues Part erzeugen
                            if(help_check == null)
                            {
                                string name_issue = "Neuer Part <<" + repository_child.Name + ">> " + DateTime.Now.ToString();
                                string notes_issue = "Durch den Import erzeugter Part";
                                Issue issue = new Issue(Data, name_issue, notes_issue, this.GUID_Elemente, repository, true, null);
                                string guid2 = repository_Connector.Create_Dependency(issue.Classifier_ID, instantiate_GUID, Data.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), Data.metamodel.m_Con_Trace[0].SubType, repository, Data, Data.metamodel.m_Con_Trace[0].Toolbox, Data.metamodel.m_Con_Trace[0].direction);

                            }


                            //Konnektor anlegen zwischen Classifier
                            //List<string> m_Taxonomy_Type = Data.metamodel.m_Decomposition_Element.Select(x => x.Type).ToList();
                            //List<string> m_Taxonomy_Stereotype = Data.metamodel.m_Decomposition_Element.Select(x => x.Stereotype).ToList();




                            // var help_guid = repository_Connector.Create_Dependency(target_GUID_EA, source_GUID_EA, m_Taxonomy_Stereotype, m_Taxonomy_Type, Data.metamodel.m_Decomposition_Element[0].SubType, repository, Data);



                        }
                    }
                  

                }
                //Technisches System --> Port
                /* if (repository.GetElementByGuid(target_GUID_EA).Type == "Port")
                 {
                     string SQL = "UPDATE t_object SET [ParentID] = " + source_Rep_ID + " WHERE [Object_ID] = " + target_Rep_ID + " AND Object_Type IN ('Port');";

                     repository.Execute(SQL);
                 }*/
              



            }

        }

        private void Import_Link_Afo_Sys(XElement source, XElement target, EA.Repository repository, Database Data)
        {
            bool flag_check = false;
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            Repository_Connector repository_Connector = new Repository_Connector();
            ////////////////////////////
            //Object ID erhalten
            //source
            var source_Object_ID = from a in source.Descendants("objectid")
                                   select a.Value;

            string[] source_Object_ID_array = source_Object_ID.ToArray();
            //target
            var target_Object_ID = from b in target.Descendants("objectid")
                                   select b.Value;

            string[] target_Object_ID_array = target_Object_ID.ToArray();
            ///////////////////////////
            //Abfrage, ob Elemente exisitieren --> TaggedValues Value abfragen "OBJECT_ID"
            int source_Rep_ID = tagged.Get_ID_From_Value(source_Object_ID_array[0], "OBJECT_ID", repository);
            int target_Rep_ID = tagged.Get_ID_From_Value(target_Object_ID_array[0], "OBJECT_ID", repository);

            //MessageBox.Show("source_Rep_ID: " + source_Rep_ID + "\nsource_Object_ID: " + source_Object_ID_array[0] + "\ntarget_Rep_ID: " + target_Rep_ID + "\ntarget_Object_ID: " + target_Object_ID_array[0]);

            if (source_Rep_ID != -1 && target_Rep_ID != -1)
            {
                //Konnektor Afo --> Sys anlegen
                Requirement recent_req = new Requirement(null, Data.metamodel);
                string requirement_GUID_EA = tagged.Get_GUID_From_ID(source_Rep_ID, repository);
                recent_req.Add_to_Database(Data);
                string sys_GUID_EA = tagged.Get_GUID_From_ID(target_Rep_ID, repository);

                //Sollte vorhanden sein, object_id zu diesen Elementen exisitiert
                if (requirement_GUID_EA != null && sys_GUID_EA != null)
                {
                    recent_req.Classifier_ID = requirement_GUID_EA;
                    //Der Fall von AFo zu Sys Logical wird nicht direkt auf die Elemente verlinkt --> Doch soll zunächst gemacht werden
                    //    if(repository.GetElementByGuid(sys_GUID_EA).Stereotype != Data.metamodel.Szenar[0])
                    //    {
                    string Con_Type = "test";
                    string Con_Stereotype = "kein";
                    string stereo = repository.GetElementByGuid(sys_GUID_EA).Stereotype;
                    //Afo --> Sys unbestimmt
                   // if (Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList().Contains(stereo) == true && Data.link_afo_sys == true)
                    if (Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList().Contains(stereo) == true && Data.link_afo_sys == true)
                    {
                        // Con_Type = Data.metamodel.Derived_AfoElem_Type[0];
                        // Con_Stereotype = Data.metamodel.Derived_AfoElem_Stereotype[0];
                        List<string> m_Con_GUID = repository_Connector.Check_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data, Data.metamodel.m_Derived_Element[0].direction);

                        if (m_Con_GUID == null)
                        {
                            flag_check = true;
                        }
                        //recent_req.Create_Dependency(sys_GUID_EA, Con_Stereotype, Con_Type, repository);
                        string Con_GUID = "";

                        int index =  Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList().FindIndex(x => x == stereo);

                        if(Data.metamodel.m_Elements_Definition[index].XAC_Attribut == "unbestimmt")
                        {
                            Con_GUID = repository_Connector.Create_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                        }
                        else
                        {
                            Con_GUID = repository_Connector.Create_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_SysElement[0].SubType, repository, Data, Data.metamodel.m_Derived_SysElement[0].Toolbox, Data.metamodel.m_Derived_SysElement[0].direction);

                        }

                        if (flag_check == true)
                        {
                            m_GUID_SysAfo_Connector_Import_new.Add(Con_GUID);
                        }

                        m_GUID_SysAfo_Connector_Import.Add(Con_GUID);
                    }
                    //Afo --> Sys bestimmt
                    if (Data.metamodel.m_Elements_SysArch_Definition.Select(x => x.Stereotype).ToList().Contains(stereo) == true && Data.link_afo_sys == true)
                    {
                        // Con_Type = Data.metamodel.Derived_AfoElem_Type[0];
                        // Con_Stereotype = Data.metamodel.Derived_AfoElem_Stereotype[0];
                        List<string> m_Con_GUID = repository_Connector.Check_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Data, Data.metamodel.m_Derived_SysElement[0].direction);

                        if (m_Con_GUID == null)
                        {
                            flag_check = true;
                        }
                        //recent_req.Create_Dependency(sys_GUID_EA, Con_Stereotype, Con_Type, repository);
                        string Con_GUID = repository_Connector.Create_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_SysElement[0].SubType, repository, Data, Data.metamodel.m_Derived_SysElement[0].Toolbox, Data.metamodel.m_Derived_SysElement[0].direction);

                        if (flag_check == true)
                        {
                            m_GUID_SysAfo_Connector_Import_new.Add(Con_GUID);
                        }

                        m_GUID_SysAfo_Connector_Import.Add(Con_GUID);
                    }
                    //Afo --> Capability
                    if (Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList().Contains( repository.GetElementByGuid(sys_GUID_EA).Stereotype) == true && Data.link_afo_cap == true)
                    {

                        // Con_Type = Data.metamodel.StereoType_Derived[3];
                        // Con_Stereotype = Data.metamodel.StereoType_Derived[2];
                       
                        List<string> m_Con_GUID = repository_Connector.Check_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Data, Data.metamodel.m_Derived_Capability[0].direction);

                        if(m_Con_GUID == null)
                        {
                            flag_check = true;
                        }
                        //recent_req.Create_Dependency(sys_GUID_EA, Con_Stereotype, Con_Type, repository);
                        string Con_GUID =  repository_Connector.Create_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Capability.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Capability[0].SubType, repository, Data, Data.metamodel.m_Derived_Capability[0].Toolbox, Data.metamodel.m_Derived_Capability[0].direction);

                        if(flag_check == true)
                        {
                            m_GUID_FunktionsbaumAfo_Connector_Import_new.Add(Con_GUID);
                        }

                        m_GUID_FunktionsbaumAfo_Connector_Import.Add(Con_GUID);


                    }
                    //Afo --> Logical
                    if (Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList().Contains(repository.GetElementByGuid(sys_GUID_EA).Stereotype) == true && Data.link_afo_logical == true)
                    {
                        // Con_Type = Data.metamodel.StereoType_Derived[5];
                        // Con_Stereotype = Data.metamodel.StereoType_Derived[4];
                        List<string> m_Con_GUID = repository_Connector.Check_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_Logical.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Logical.Select(x => x.Type).ToList(), Data, Data.metamodel.m_Derived_Logical[0].direction);

                        if (m_Con_GUID == null)
                        {
                            flag_check = true;
                        }
                        //recent_req.Create_Dependency(sys_GUID_EA, Con_Stereotype, Con_Type, repository);
                        string Con_GUID  = repository_Connector.Create_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_Logical.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Logical.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Logical[0].SubType, repository, Data, Data.metamodel.m_Derived_Logical[0].Toolbox, Data.metamodel.m_Derived_Logical[0].direction);

                        if (flag_check == true)
                        {
                            m_GUID_LogicalAfo_Connector_Import_new.Add(Con_GUID);
                        }

                        m_GUID_LogicalAfo_Connector_Import.Add(Con_GUID);
                    }
                    // }
                    /*
                    else
                    {
                        //Import AFo Sys Verlinkung --> Dazu müssen die Systemelemente importiert werden und die Verlinkung von Afo zu diesen
                        if(Data.sys_xac == true && Data.link_afo_sys == true)
                        {
                            Import_Link_Afo_Logical(requirement_GUID_EA, sys_GUID_EA, repository, Data);
                        }
                    }
                    */
                }

            }
        }

        private void Import_Link_Afo_Afo(XElement source, XElement target, EA.Repository repository, Database Data, string Link_name)
        {
            bool flag_check = false;
            List<string> m_Stereotype = new List<string>();
            List<string> m_Type = new List<string>();

            int Con_Type = -1;

            string Stereotype = "";
            string Type = "Dependency";
            string Toolbox = "";
            string Subtype = null;
            bool direction = true;

            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            Repository_Connector repository_Connector = new Repository_Connector();
            ////////////////////////////
            //Object ID erhalten
            //source
            var source_Object_ID = from a in source.Descendants("objectid")
                                   select a.Value;

            string[] source_Object_ID_array = source_Object_ID.ToArray();
            //target
            var target_Object_ID = from b in target.Descendants("objectid")
                                   select b.Value;

            string[] target_Object_ID_array = target_Object_ID.ToArray();
            ///////////////////////////
            //Abfrage, ob Elemente exisitieren --> TaggedValues Value abfragen "OBJECT_ID"
            int source_Rep_ID = tagged.Get_ID_From_Value(source_Object_ID_array[0], "OBJECT_ID", repository);
            int target_Rep_ID = tagged.Get_ID_From_Value(target_Object_ID_array[0], "OBJECT_ID", repository);

            //MessageBox.Show("source_Rep_ID: " + source_Rep_ID + "\nsource_Object_ID: " + source_Object_ID_array[0] + "\ntarget_Rep_ID: " + target_Rep_ID + "\ntarget_Object_ID: " + target_Object_ID_array[0]);
            if (Link_name == this.Metamodel_Base.m_Connectoren_Req7_Afo[0])
            {
                m_Stereotype = Data.metamodel.m_Afo_Konflikt.Select(x => x.Stereotype).ToList();
                m_Type = Data.metamodel.m_Afo_Konflikt.Select(x => x.Type).ToList();
                Toolbox = Data.metamodel.m_Afo_Konflikt[0].Toolbox;
                Subtype = Data.metamodel.m_Afo_Konflikt[0].SubType;
                Con_Type = 0;
                direction = Data.metamodel.m_Afo_Konflikt[0].direction;
            }
            if (Link_name == this.Metamodel_Base.m_Connectoren_Req7_Afo[1])
            {
                m_Stereotype = Data.metamodel.m_Afo_Dublette.Select(x => x.Stereotype).ToList();
                m_Type = Data.metamodel.m_Afo_Dublette.Select(x => x.Type).ToList();
                Toolbox = Data.metamodel.m_Afo_Dublette[0].Toolbox;
                Subtype = Data.metamodel.m_Afo_Dublette[0].SubType;
                Con_Type = 1;
                direction = Data.metamodel.m_Afo_Dublette[0].direction;
            }
            if (Link_name == this.Metamodel_Base.m_Connectoren_Req7_Afo[2])
            {
                m_Stereotype = Data.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList();
                m_Type = Data.metamodel.m_Afo_Refines.Select(x => x.Type).ToList();
                Toolbox = Data.metamodel.m_Afo_Refines[0].Toolbox;
                Subtype = Data.metamodel.m_Afo_Refines[0].SubType;
                Con_Type = 2;
                direction = Data.metamodel.m_Afo_Refines[0].direction;
            }
            if (Link_name == this.Metamodel_Base.m_Connectoren_Req7_Afo[3])
            {
                m_Stereotype = Data.metamodel.m_Afo_Replaces.Select(x => x.Stereotype).ToList();
                m_Type = Data.metamodel.m_Afo_Replaces.Select(x => x.Type).ToList();
                Toolbox = Data.metamodel.m_Afo_Replaces[0].Toolbox;
                Subtype = Data.metamodel.m_Afo_Replaces[0].SubType;
                Con_Type = 3;
                direction = Data.metamodel.m_Afo_Replaces[0].direction;
            }
            if (Link_name == this.Metamodel_Base.m_Connectoren_Req7_Afo[4])
            {
                m_Stereotype = Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList();
                m_Type = Data.metamodel.m_Afo_Requires.Select(x => x.Type).ToList();
                Toolbox = Data.metamodel.m_Afo_Requires[0].Toolbox;
                Subtype = Data.metamodel.m_Afo_Requires[0].SubType;
                Con_Type = 4;
                direction = Data.metamodel.m_Afo_Requires[0].direction;
            }
            if (Link_name == this.Metamodel_Base.m_Connectoren_Req7_Afo[5])
            {
                m_Stereotype = Data.metamodel.m_Afo_InheritsFrom.Select(x => x.Stereotype).ToList();
                m_Type = Data.metamodel.m_Afo_InheritsFrom.Select(x => x.Type).ToList();
                Toolbox = Data.metamodel.m_Afo_InheritsFrom[0].Toolbox;
                Subtype = Data.metamodel.m_Afo_InheritsFrom[0].SubType;
                Con_Type = 5;
                direction = Data.metamodel.m_Afo_InheritsFrom[0].direction;
            }



            if (source_Rep_ID != -1 && target_Rep_ID != -1)
            {
                string source_GUID_EA = "";
                string target_GUID_EA = "";

               

                if (Link_name == this.Metamodel_Base.m_Connectoren_Req7_Afo[2])
                {
                    target_GUID_EA = tagged.Get_GUID_From_ID(source_Rep_ID, repository);
                    source_GUID_EA = tagged.Get_GUID_From_ID(target_Rep_ID, repository);
                }
                else
                {
                    source_GUID_EA = tagged.Get_GUID_From_ID(source_Rep_ID, repository);
                    target_GUID_EA = tagged.Get_GUID_From_ID(target_Rep_ID, repository);
                }


                // repository_Connector.Check_Dependency
                //Elemente existieren --> entsprechend Connector anlegen
                /////////////
                //Schnittstelle
                //  if(repository.GetElementByGuid(source_GUID_EA).Stereotype == Data.metamodel.Requirement_Interface_Stereotype && repository.GetElementByGuid(target_GUID_EA).Stereotype == Data.metamodel.Requirement_Interface_Stereotype)
                //  {
                //Requirement help = new Requirement(); //dies nur zur Hilfe, da hier die Connectoren ERzeugen Funktion hinterlegt
                //help.Classifier_ID = source_GUID_EA;
                //help.Create_Dependency(target_GUID_EA, Data.metamodel.StereoType_Send[0], Data.metamodel.StereoType_Send[1], repository);
                List<string> m_Con_GUID = repository_Connector.Check_Dependency(source_GUID_EA, target_GUID_EA, m_Stereotype, m_Type, Data, direction);

                if (m_Con_GUID == null)
                {
                    flag_check = true;
                }

                string con_guid = repository_Connector.Create_Dependency(source_GUID_EA, target_GUID_EA, m_Stereotype, m_Type, Subtype, repository, Data, Toolbox, direction);

                switch (Con_Type)
                {
                    case 0:
                        this.m_GUID_Conflicts_Connector_Import.Add(con_guid);
                        if (flag_check == true)
                        {
                            m_GUID_Conflicts_Connector_Import_new.Add(con_guid);
                        }
                        break;
                    case 1:
                        this.m_GUID_Dublette_Connector_Import.Add(con_guid);
                        if (flag_check == true)
                        {
                            m_GUID_Dublette_Connector_Import_new.Add(con_guid);
                        }
                        break;
                    case 2:
                        this.m_GUID_Refines_Connector_Import.Add(con_guid);
                        if (flag_check == true)
                        {
                            m_GUID_Refines_Connector_Import_new.Add(con_guid);
                        }
                        break;
                    case 3:
                        this.m_GUID_Replaces_Connector_Import.Add(con_guid);
                        if (flag_check == true)
                        {
                            m_GUID_Replaces_Connector_Import_new.Add(con_guid);
                        }
                        break;
                    case 4:
                        this.m_GUID_Requires_Connector_Import.Add(con_guid);
                        if (flag_check == true)
                        {
                            m_GUID_Requires_Connector_Import_new.Add(con_guid);
                        }
                        break;
                    case 5:
                        this.m_GUID_InheritsFrom_Connector_Import_new.Add(con_guid);
                        if (flag_check == true)
                        {
                            m_GUID_InheritsFrom_Connector_Import_new.Add(con_guid);
                        }
                        break;
                    default:
                        break;

                }
                //  }
            }
        }

        private void Import_Link_Afo_Logical(EA.Repository repository, Database Data, Loading_OpArch loading)
        {
            Repository_Connector repository_Connector = new Repository_Connector();
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
          //  DB_Command command = new DB_Command();
          //  XML xML = new XML();
            List<string> m_ea_guid = new List<string>();
            List<string> m_ea_guid_Part = new List<string>();
            List<string> m_ea_guid_Classifier = new List<string>();

            // List<string> m_Logical_GUID = Data.Get_Logicals_GUID(repository);

            //////////////////////////////////////////////////////////////////////////
            //Alle Derived From Logical erhalten * EndType Class & Szenar[0]
            List<string> m_Type = Data.metamodel.m_Szenar.Select(x => x.Type).ToList();
            List<string> m_Stereoype = Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList();
            List<string> m_Requirement_Type = Data.metamodel.m_Requirement.Select(x => x.Type).ToList();
            List<string> m_Requirement_Stereotype = Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList();
            List<string> m_Usage_Type = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Usage_Stereotype = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();
            List<string> m_Definition_Type = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Definition_Stereotype = Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

         
            List<string> con_type = Data.metamodel.m_Derived_Logical.Select(x => x.Type).ToList();
            List<string> con_stereotype = Data.metamodel.m_Derived_Logical.Select(x => x.Stereotype).ToList();

            Interface_Connectors interface_Connectors = new Interface_Connectors();
            m_ea_guid = interface_Connectors.Get_Connetor_By_Elements(Data, m_Requirement_Type, m_Requirement_Stereotype, m_Type, m_Stereoype, con_type, con_stereotype);

            if (m_ea_guid != null) //Konnektor Logical & Afo vorhanden
            {
                loading.label_Progress.Text = "Konnektor Szenarbaum Auflösung";
                loading.progressBar1.Step = 1;
                loading.progressBar1.Minimum = 0;
                loading.progressBar1.Maximum = m_ea_guid.Count;
                loading.progressBar1.Value = 0;
                loading.Refresh();

                List<string> m_Type2 = Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                List<string> m_Stereotype2 = Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                Repository_Class repository_ = new Repository_Class();

                List<Logical> m_Logical_szenar = new List<Logical>();
                //Jeder Connector DerivedFromLogical wird betrachtet
                int i1 = 0;
                do
                {
                    //GUID AFO erhalten
                    string help_Client = interface_Connectors.Get_Client_GUID(Data, m_ea_guid[i1], null, null);
                    List<string> help = new List<string>();
                    // GUID Logical erhalten
                    string help_Supplier = interface_Connectors.Get_Supplier_GUID(Data, m_ea_guid[i1], null, null);


                    Logical recent_logical = new Logical(null, null);
                    List<Logical> m_recent_logical = new List<Logical>();
                    m_recent_logical = m_Logical_szenar.Where(x => x.Classifier_ID == help_Supplier).ToList();

                    if (m_recent_logical.Count == 0)
                    {
                        Logical n_logical = new Logical(null, null);
                        n_logical.Classifier_ID = help_Supplier;

                        m_Logical_szenar.Add(n_logical);

                        List<string> m_children_guid = n_logical.Get_GUID_Children(Data, m_Usage_Type, m_Usage_Stereotype);
                        List<string> m_children_instnaceguid = n_logical.Get_GUID_Instance_Children(Data, m_Usage_Type, m_Usage_Stereotype);

                        if (m_children_guid ==  null)
                        {
                            m_children_guid = new List<string>();
                        }
                        if (m_children_instnaceguid == null)
                        {
                            m_children_instnaceguid = new List<string>();
                        }

                        n_logical.m_allchildren_guid = m_children_guid;
                        n_logical.m_allchildreninstance_guid = m_children_instnaceguid;


                        recent_logical = n_logical;
                    }
                    else
                    {
                        recent_logical = m_recent_logical[0];
                    }


                    help.Add(help_Client);
                    //GUID von Elemnt Definition erhalten, welche auch mit der AFO verbunden sind
                    m_ea_guid_Part = interface_Connectors.Get_Supplier_Element_By_Connector(Data, help, m_Definition_Type, m_Definition_Stereotype, m_Type2, m_Stereotype2);

                    if (m_ea_guid_Part != null)
                    {
                        int i2 = 0;
                        do
                        {
                            if (recent_logical.m_allchildren_guid.Contains(m_ea_guid_Part[i2]) == false)
                            {

                                Repository_Class recent_Element = new Repository_Class();
                                recent_Element.Classifier_ID = m_ea_guid_Part[i2];
                                //SYS_KOMPONETENTYP erhalten
                                string sys_kt = tagged.Get_Tagged_Value(m_ea_guid_Part[i2], "SYS_KOMPONENTENTYP", repository);
                                List<Element_Metamodel> m_Sterotype_Class = new List<Element_Metamodel>();
                                //Sterotype aus SYS_KOMPONETENTYP erhalten
                                if (sys_kt != null)
                                {
                                    m_Sterotype_Class = Data.metamodel.Find_StereoType_Sys_Komponententyp(Data, sys_kt);
                                }
                                else
                                {
                                    m_Sterotype_Class.Add(Data.metamodel.m_Elements_Definition[0]);
                                    m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0]);
                                }

                                // int index = Data.metamodel.m_Elements_Definition.FindIndex(x => x.Stereotype == recent_Class.Stereotype);
                                string Name = recent_Element.Get_Name(Data);

                                if (Name == null)
                                {
                                    Name = "nameless";
                                }

                                //Element anlegen
                                string instantiate_GUID = repository_.Create_Element_Instantiate(Name, m_Sterotype_Class[1].Type, m_Sterotype_Class[1].Stereotype, m_Sterotype_Class[1].Toolbox, m_ea_guid_Part[i2], repository.GetElementByGuid(recent_logical.Classifier_ID).ElementID, repository.GetPackageByID(repository.GetElementByGuid(recent_logical.Classifier_ID).PackageID).PackageGUID, repository, true, Data);

                                //Issue neues Part erzeugen
                                string name_issue = "Neuer Part <<"+ Name+">> " + DateTime.Now.ToString();
                                string notes_issue = "Durch den Import erzeugter Part";
                                Issue issue = new Issue(Data, name_issue, notes_issue, this.GUID_Elemente, repository, true, null);
                                string guid2 = repository_Connector.Create_Dependency(issue.Classifier_ID, instantiate_GUID, Data.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), Data.metamodel.m_Con_Trace[0].SubType, repository, Data, Data.metamodel.m_Con_Trace[0].Toolbox, Data.metamodel.m_Con_Trace[0].direction);



                                List<string> m_help_check = repository_Connector.Check_Dependency(help_Client, instantiate_GUID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data, Data.metamodel.m_Derived_Element[0].direction);

                                //Connector anlegen
                               string guid =  repository_Connector.Create_Dependency(help_Client, instantiate_GUID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                                if(m_help_check == null)
                                {
                                    this.m_GUID_SysAfo_Connector_Import_new.Add(guid);
                                }



                                //m_allchildren hinzufügen
                                recent_logical.m_allchildren_guid.Add(m_ea_guid_Part[i2]);
                                recent_logical.m_allchildreninstance_guid.Add(instantiate_GUID);
                            }
                            else
                            {
                                int ind = recent_logical.m_allchildren_guid.IndexOf(m_ea_guid_Part[i2]);

                                List<string> m_help_check = repository_Connector.Check_Dependency(help_Client, recent_logical.m_allchildreninstance_guid[ind], Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data, Data.metamodel.m_Derived_Element[0].direction);

                                //Connector anlegen
                                string guid2 = repository_Connector.Create_Dependency(help_Client, recent_logical.m_allchildreninstance_guid[ind], Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                                if (m_help_check == null)
                                {
                                    this.m_GUID_SysAfo_Connector_Import_new.Add(guid2);
                                }
                            }


                            i2++;
                        } while (i2 < m_ea_guid_Part.Count);

                      
                    }



                   


                    loading.progressBar1.PerformStep();
                    loading.progressBar1.Refresh();

                    i1++;
                } while (i1 < m_ea_guid.Count);



            }

        }

        private void Import_Link_Afo_St(XElement source, XElement target, EA.Repository repository, Database Data)
        {
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            Repository_Connector repository_Connector = new Repository_Connector();
            ////////////////////////////
            //Object ID erhalten
            //source
            var source_Object_ID = from a in source.Descendants("objectid")
                                   select a.Value;

            string[] source_Object_ID_array = source_Object_ID.ToArray();
            //target
            var target_Object_ID = from b in target.Descendants("objectid")
                                   select b.Value;

            string[] target_Object_ID_array = target_Object_ID.ToArray();
            ///////////////////////////
            //Abfrage, ob Elemente exisitieren --> TaggedValues Value abfragen "OBJECT_ID"
            int source_Rep_ID = tagged.Get_ID_From_Value(source_Object_ID_array[0], "OBJECT_ID", repository);
            int target_Rep_ID = tagged.Get_ID_From_Value(target_Object_ID_array[0], "OBJECT_ID", repository);

            //MessageBox.Show("source_Rep_ID: " + source_Rep_ID + "\nsource_Object_ID: " + source_Object_ID_array[0] + "\ntarget_Rep_ID: " + target_Rep_ID + "\ntarget_Object_ID: " + target_Object_ID_array[0]);

            if (source_Rep_ID != -1 && target_Rep_ID != -1)
            {
                //Konnektor Afo --> Sys anlegen
                Requirement recent_req = new Requirement(null, Data.metamodel);
                string requirement_GUID_EA = tagged.Get_GUID_From_ID(source_Rep_ID, repository);
                recent_req.Add_to_Database(Data);
                string sys_GUID_EA = tagged.Get_GUID_From_ID(target_Rep_ID, repository);

                //Sollte vorhanden sein, object_id zu diesen Elementen exisitiert
                if (requirement_GUID_EA != null && sys_GUID_EA != null)
                {
                    recent_req.Classifier_ID = requirement_GUID_EA;
                    //Der Fall von AFo zu Sys Logical wird nicht direkt auf die Elemente verlinkt --> Doch soll zunächst gemacht werden
                    //    if(repository.GetElementByGuid(sys_GUID_EA).Stereotype != Data.metamodel.Szenar[0])
                    //    {
                    string Con_Type = "test";
                    string Con_Stereotype = "kein";

                    repository_Connector.Create_Dependency(recent_req.Classifier_ID, sys_GUID_EA, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                }

            }
        }
        #endregion region Import Links

        #region Decomposition
        private void Create_Decomposition_Logical(string Logical_GUID, List<string> m_GUID, string Part_Classifier, EA.Repository repository, Database Data, string Logical_Connector_GUID)
        {
           // XML xML = new XML();
            Repository_Class repository_Element = new Repository_Class();
            Repository_Class recent_Element = new Repository_Class();
            Repository_Connector repository_Connector = new Repository_Connector();
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            Interface_Element interface_Element = new Interface_Element();

            string Parent_GUID = Logical_GUID;

            //Es wird zunächst in der Logical die Decomposition bis zum Elemente aufgebaut
            if (m_GUID.Count > 0)
            {
               
                Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                int i1 = 0;
                do
                {
                    List<string> m_ea_guid_Child = new List<string>();
                    m_ea_guid_Child = interface_Element.Get_Children_By_Classifier(Data, m_GUID[m_GUID.Count - 1 - i1], Parent_GUID);

                    //Wenn nicht vorhanden Part anlegen
                    if (m_ea_guid_Child == null)
                    {
                        // var recent_Class = repository.GetElementByGuid(m_GUID[m_GUID.Count - 1 - i1]);
                        recent_Element.Classifier_ID = m_GUID[m_GUID.Count - 1 - i1];
                        //SYS_KOMPONETENTYP erhalten
                        string sys_kt = tagged.Get_Tagged_Value(m_GUID[m_GUID.Count - 1 - i1], "SYS_KOMPONENTENTYP", repository);
                        List<Element_Metamodel> m_Sterotype_Class = new List<Element_Metamodel>();
                        //Sterotype aus SYS_KOMPONETENTYP erhalten
                        if(sys_kt != null)
                        {
                            m_Sterotype_Class = Data.metamodel.Find_StereoType_Sys_Komponententyp(Data, sys_kt);
                        }
                        else
                        {
                            /* m_Sterotype_Class.Add(Data.metamodel.m_Elements_Definition[0].Stereotype);
                             m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0].Stereotype);
                             m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0].Stereotype);
                             m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0].Type);
                             */
                            m_Sterotype_Class.Add(Data.metamodel.m_Elements_Definition[0]);
                            m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0]);
                        }

                        // int index = Data.metamodel.m_Elements_Definition.FindIndex(x => x.Stereotype == recent_Class.Stereotype);
                        string Name = recent_Element.Get_Name(Data);

                        if(Name == null)
                        {
                            Name = "nameless";
                        }

                        /*  if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                          {
                              Data.oLEDB_Interface.dbConnection.Close();
                          }
                          */
                        interface_Collection_OleDB.Close_Connection(Data);
                        Parent_GUID = repository_Element.Create_Element_Instantiate(Name, m_Sterotype_Class[1].Type, m_Sterotype_Class[1].Stereotype,m_Sterotype_Class[1].Toolbox, m_GUID[m_GUID.Count - 1 - i1], repository.GetElementByGuid(Parent_GUID).ElementID, repository.GetPackageByID(repository.GetElementByGuid(Logical_GUID).PackageID).PackageGUID, repository, true, Data);
                        /*   if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                           {
                               Data.oLEDB_Interface.dbConnection.Open();
                           }*/
                        interface_Collection_OleDB.Open_Connection(Data);

                    }
                    else
                    {
                        Parent_GUID = m_ea_guid_Child[0];
                    }

                    i1++;
                } while (i1 < m_GUID.Count);
            }

            //Es wird das Element angelegt
            if (Part_Classifier != null)
            {

                /*string SQL_Child = "SELECT ea_guid FROM t_object WHERE PDATA1 = '" + Part_Classifier + "' AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = '" + Parent_GUID + "')";
                string xml_String = repository.SQLQuery(SQL_Child);
                List<string> m_ea_guid_Child = xML.Xml_Read_Attribut("ea_guid", xml_String);*/

                List<string> m_ea_guid_Child = new List<string>();
                m_ea_guid_Child = interface_Element.Get_Children_By_Classifier(Data, Part_Classifier, Parent_GUID);
                /*  string SQL_Child2 = "SELECT ea_guid FROM t_object WHERE PDATA1 = ? AND ParentID IN (SELECT Object_ID FROM t_object WHERE ea_guid = ?)";
                  OleDbCommand SELECT = new OleDbCommand(SQL_Child2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                  List<DB_Input[]> ee = new List<DB_Input[]>();
                  List<string> help = new List<string>();
                  List<string> help2 = new List<string>();
                  help.Add(Parent_GUID);
                  help2.Add(Part_Classifier);
                  ee.Add(help.Select(x => new DB_Input(-1, x)).ToArray());
                  ee.Add(help2.Select(x => new DB_Input(-1, x)).ToArray());

                  OleDbType[] m_input_Type2 = { OleDbType.VarChar, OleDbType.VarChar };
                  Data.oLEDB_Interface.Add_Parameters_Select(SELECT, ee, m_input_Type2);
                  string[] m_output = { "ea_guid" };

                  List<DB_Return> m_ret5 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output);

                  if (m_ret5[0].Ret.Count > 1)
                  {
                      m_ea_guid_Child = m_ret5[0].Ret.GetRange(1, m_ret5[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
                  }
                  else
                  {
                      m_ea_guid_Child = null;
                  }
                  */


                //Wenn nicht vorhanden Part anlegen
                if (m_ea_guid_Child == null)
                {
                  //  var recent_Class = repository.GetElementByGuid(Part_Classifier);

                 /*   string SQL_Sterreotype = "SELECT Stereotype FROM t_object WHERE ea_guid = '" + Part_Classifier + "'";
                    string xml_String_Stereo = repository.SQLQuery(SQL_Sterreotype);
                    List<string> m_ea_guid_Stereo = xML.Xml_Read_Attribut("Stereotype", xml_String_Stereo);*/

                    List<string> m_ea_guid_Stereo = new List<string>();
                    string ea_guid_Stereo = interface_Element.Get_One_Attribut_String(Part_Classifier, Data, "Stereotype");
                    if(ea_guid_Stereo != null)
                    {
                        m_ea_guid_Stereo.Add(ea_guid_Stereo);
                    }
                   
                /*    string SQL_Sterreotype2 = "SELECT Stereotype FROM t_object WHERE ea_guid = ?";
                    OleDbCommand SELECT2 = new OleDbCommand(SQL_Sterreotype2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    List<DB_Input[]> ee2 = new List<DB_Input[]>();
                    List<string> help3 = new List<string>();
                    help3.Add(Part_Classifier);
                    ee2.Add(help3.Select(x => new DB_Input(-1, x)).ToArray());

                    OleDbType[] m_input_Type3 = { OleDbType.VarChar};
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee2, m_input_Type3);
                    string[] m_output2 = { "Stereotype" };

                    List<DB_Return> m_ret6 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output2);

                    if (m_ret6[0].Ret.Count > 1)
                    {
                        m_ea_guid_Stereo = m_ret6[0].Ret.GetRange(1, m_ret6[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
                    }
                    else
                    {
                        m_ea_guid_Stereo = null;
                    }
                    */

                    /*    int index = -1;

                        if (m_ea_guid_Stereo != null)
                        {
                            index = Data.metamodel.m_Elements_Definition.FindIndex(x => x.Stereotype == m_ea_guid_Stereo[0]);
                        }
                        else
                        {
                            index = -1;
                        }
                        */
                    //SYS_KOMPONETENTYP erhalten
                    string sys_kt = tagged.Get_Tagged_Value(Part_Classifier, "SYS_KOMPONENTENTYP", repository);
                    List<Element_Metamodel> m_Sterotype_Class = new List<Element_Metamodel>();
                    //Sterotype aus SYS_KOMPONETENTYP erhalten
                    if (sys_kt != null)
                    {
                        m_Sterotype_Class = Data.metamodel.Find_StereoType_Sys_Komponententyp(Data, sys_kt);
                    }
                    else
                    {
                        /*    m_Sterotype_Class.Add(Data.metamodel.m_Elements_Definition[0].Stereotype);
                            m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0].Stereotype);
                            m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0].Stereotype);
                            m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0].Type);*/
                        m_Sterotype_Class.Add(Data.metamodel.m_Elements_Definition[0]);
                        m_Sterotype_Class.Add(Data.metamodel.m_Elements_Usage[0]);
                    }


                    //   if (index != -1)
                    // {
                    recent_Element.Classifier_ID = Part_Classifier;
                    string Name = recent_Element.Get_Name(Data);
                    if(Name == null)
                    {
                        Name = "nameless";
                    }
                    /*  if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                      {
                          Data.oLEDB_Interface.dbConnection.Close();
                      }*/
                    Interface_Collection interface_Collection_OleDB = new Interface_Collection();
                    interface_Collection_OleDB.Close_Connection(Data);
                    Parent_GUID = repository_Element.Create_Element_Instantiate(Name, m_Sterotype_Class[1].Type, m_Sterotype_Class[1].Stereotype, m_Sterotype_Class[1].Toolbox, Part_Classifier, repository.GetElementByGuid(Parent_GUID).ElementID, repository.GetPackageByID(repository.GetElementByGuid(Logical_GUID).PackageID).PackageGUID, repository, true, Data);
                    /* if (Data.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Closed)
                      {
                          Data.oLEDB_Interface.dbConnection.Open();
                      }*/
                    interface_Collection_OleDB.Open_Connection(Data);
                    /*  }
                    else
                    {
                        Parent_GUID = repository_Element.Create_Element_Instantiate(recent_Class.Name, Data.metamodel.m_Elements_Usage., Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList()[0], Part_Classifier, repository.GetElementByGuid(Parent_GUID).ElementID, repository.GetPackageByID(repository.GetElementByGuid(Logical_GUID).PackageID).PackageGUID, repository, true);
                    }*/

                }
                else
                {
                    Parent_GUID = m_ea_guid_Child[0];
                }
                //ER muss kopoet werden dann nach Abschluss gelöscht werden

                /*     string SQL_help = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_Connector WHERE ea_guid = '" + Logical_Connector_GUID + "');";
                     string xml_String_help = repository.SQLQuery(SQL_help);
                     List<string> m_ea_guid_help = xML.Xml_Read_Attribut("ea_guid", xml_String_help);
     */
                Interface_Connectors interface_Connectors = new Interface_Connectors();
                List<string> m_ea_guid_help = new List<string>();
                string ea_guid_help = interface_Connectors.Get_Client_GUID(Data, Logical_Connector_GUID, null, null);
                if(ea_guid_help != null)
                {
                    m_ea_guid_help.Add(ea_guid_help);
                }
              /*  string SQL_help2 = "SELECT ea_guid FROM t_object WHERE Object_ID IN (SELECT Start_Object_ID FROM t_Connector WHERE ea_guid = ?);";
                OleDbCommand SELECT3 = new OleDbCommand(SQL_help2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                List<DB_Input[]> ee3 = new List<DB_Input[]>();
                List<string> help4 = new List<string>();
                help4.Add(Logical_Connector_GUID);
                ee3.Add(help4.Select(x => new DB_Input(-1, x)).ToArray());

                OleDbType[] m_input_Type4 = { OleDbType.VarChar };
                Data.oLEDB_Interface.Add_Parameters_Select(SELECT3, ee3, m_input_Type4);
                string[] m_output3 = { "ea_guid" };

                List<DB_Return> m_ret7 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT3, m_output3);

                if (m_ret7[0].Ret.Count > 1)
                {
                    m_ea_guid_help = m_ret7[0].Ret.GetRange(1, m_ret7[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
                }
                else
                {
                    m_ea_guid_help = null;
                }
                */


                if (m_ea_guid_help != null)
                {
                    //Requirement help = new Requirement();
                    //help.Classifier_ID = m_ea_guid_help[0];
                    //help.Create_Dependency(Parent_GUID, Data.metamodel.StereoType_Derived[0], Data.metamodel.StereoType_Derived[1], repository);
                    repository_Connector.Create_Dependency(m_ea_guid_help[0], Parent_GUID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data.metamodel.m_Derived_Element[0].SubType, repository, Data, Data.metamodel.m_Derived_Element[0].Toolbox, Data.metamodel.m_Derived_Element[0].direction);

                }
                //Konnektor vom Logical zum Element umlegen && StereoType ändern in Derived_From
                //tagged.Update_Connector_Logical(repository, Data, Logical_Connector_GUID, Parent_GUID);
            }

        }
        #endregion

        #region Delete 
        private void Delete_Link_Afo_Sys_Decomposition(Database Data, EA.Repository repository)
        {
         //   DB_Command command = new DB_Command();
            Repository_Connector repository_Connector = new Repository_Connector();
            Interface_Element interface_Element = new Interface_Element();
          //  XML xML = new XML();
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            //Logical Addinn Import erhalten
            List<string> m_Type = Data.metamodel.m_Szenar.Select(x => x.Type).ToList();
            List<string> m_Stereoype = Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList();
            List<string> m_Name = Data.metamodel.m_Szenar.Select(x => x.DefaultName).ToList();
            List<string> m_Requirement_Type = Data.metamodel.m_Requirement.Select(x => x.Type).ToList();
            List<string> m_Requirement_Stereotype = Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList();
            List<string> m_Usage_Type = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Usage_Stereotype = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            /*  string SQL_Logical = "SELECT Object_ID FROM t_object WHERE Stereotype IN" + xML.SQL_IN_Array(m_Stereoype.ToArray()) + " AND Object_Type IN" + xML.SQL_IN_Array(m_Type.ToArray())+" AND Name IN" + xML.SQL_IN_Array(m_Name.ToArray()) + ";";
              string xml_String = repository.SQLQuery(SQL_Logical);
              List<string> m_Object_ID_Logical = xML.Xml_Read_Attribut("Object_ID", xml_String);
              */
            List<int> m_Object_ID_Logical = new List<int>();
            m_Object_ID_Logical = interface_Element.Get_ID_By_Name(Data, m_Type, m_Stereoype, m_Name);
          /*  string SQL_Logical2 = "SELECT Object_ID FROM t_object WHERE Stereotype IN(" + command.Add_Parameters_Pre(m_Stereoype.ToArray()) + ") AND Object_Type IN(" + command.Add_Parameters_Pre(m_Type.ToArray()) + ") AND Name IN(" + command.Add_Parameters_Pre(m_Name.ToArray()) + ");";
            OleDbCommand SELECT = new OleDbCommand(SQL_Logical2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
            List<DB_Input[]> ee1 = new List<DB_Input[]>();
            ee1.Add(m_Stereoype.Select(x => new DB_Input(-1, x)).ToArray());
            ee1.Add(m_Type.Select(x => new DB_Input(-1, x)).ToArray());
            ee1.Add(m_Name.Select(x => new DB_Input(-1, x)).ToArray());

            OleDbType[] m_input_Type = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
            Data.oLEDB_Interface.Add_Parameters_Select(SELECT, ee1, m_input_Type);
            string[] m_output = { "Object_ID" };

            List<DB_Return> m_ret7 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT, m_output);

            if (m_ret7[0].Ret.Count > 1)
            {
                m_Object_ID_Logical = m_ret7[0].Ret.GetRange(1, m_ret7[0].Ret.Count - 1).ToList().Select(x => (int) x).ToList();
            }
            else
            {
               m_Object_ID_Logical = null;
            }*/


            if (m_Object_ID_Logical != null)
            {
                Repository_Element repository_Element = new Repository_Element();
                int i1 = 0;
                do
                {
                    //Kinderelement erhalten
                 /*   string SQL_Child = "SELECT Object_ID FROM t_object WHERE ParentID = " + m_Object_ID_Logical[i1] + " AND Object_Type IN"+xML.SQL_IN_Array(m_Usage_Type.ToArray())+";";
                    string xml_String_Child = repository.SQLQuery(SQL_Child);
                    List<string> m_Object_ID_Child = xML.Xml_Read_Attribut("Object_ID", xml_String_Child);
                    */
                    List<string> m_Object_GUID_Child = new List<string>();
                    repository_Element.ID = m_Object_ID_Logical[i1];
                    repository_Element.Classifier_ID = repository_Element.Get_GUID_By_ID(repository_Element.ID, Data);
                    m_Object_GUID_Child = interface_Element.Get_Children_Element(repository_Element.Classifier_ID, Data, m_Usage_Type, m_Usage_Stereotype);
                /*    string SQL_Child = "SELECT Object_ID FROM t_object WHERE ParentID = "+ m_Object_ID_Logical[i1]+" AND Object_Type IN(" + command.Add_Parameters_Pre(m_Usage_Type.ToArray()) + ");";
                    OleDbCommand SELECT2 = new OleDbCommand(SQL_Child, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                    List<DB_Input[]> ee2 = new List<DB_Input[]>();
                    ee2.Add(m_Usage_Type.Select(x => new DB_Input(-1, x)).ToArray());

                    OleDbType[] m_input_Type2 = {OleDbType.VarChar};
                    Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee2, m_input_Type2);
                    string[] m_output2 = { "Object_ID" };

                    List<DB_Return> m_ret8 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output2);

                    if (m_ret8[0].Ret.Count > 1)
                    {
                        m_Object_ID_Child = m_ret8[0].Ret.GetRange(1, m_ret8[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList();
                    }
                    else
                    {
                        m_Object_ID_Child = null;
                    }
                    */
                    if (m_Object_GUID_Child != null)
                    {
                        List<string> m_Type2 = Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                        List<string> m_Stereoype2 = Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();
                        List<int> m_Object_ID_Child = new List<int>();

                      //  List<string> m_Type_Usage = Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                      //  List<string> m_Stereoype_Usage = Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                        Interface_Connectors interface_Connectors = new Interface_Connectors();

                        int i2 = 0;
                        do
                        {
                            repository_Element.Classifier_ID = m_Object_GUID_Child[i2];
                            m_Object_ID_Child.Add(repository_Element.Get_Object_ID(Data));
                            //Alle Konektoren zwischen Kinderelement und Requirement erhalten --> Derived_From
                            /*  string SQL_Derived = "SELECT ea_guid FROM t_connector WHERE End_Object_ID = " + m_Object_ID_Child[i2] + " AND Connector_Type IN" + xML.SQL_IN_Array(Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList().ToArray()) + " AND Stereotype IN" + xML.SQL_IN_Array(Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList().ToArray()) + " AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(m_Requirement_Type.ToArray())+");";
                              string xml_String_Derived = repository.SQLQuery(SQL_Derived);
                              List<string> m_ea_guid_Derived = xML.Xml_Read_Attribut("ea_guid", xml_String_Derived);
  */
                            List<string> m_ea_guid_Derived = new List<string>();
                            m_ea_guid_Derived = interface_Connectors.Get_Connector_From_Supplier_Property(Data, "ea_guid", m_Usage_Type, m_Usage_Stereotype, m_Object_GUID_Child[i2], m_Requirement_Type, m_Requirement_Stereotype, m_Type2, m_Stereoype2);
                    /*        string SQL_Derived2 = "SELECT ea_guid FROM t_connector WHERE End_Object_ID = " + m_Object_ID_Child[i2] + " AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type2.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereoype2.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Requirement_Type.ToArray()) + "));";


                            OleDbCommand SELECT3 = new OleDbCommand(SQL_Derived2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                            List<DB_Input[]> ee3 = new List<DB_Input[]>();
                            ee3.Add(m_Requirement_Type.Select(x => new DB_Input(-1, x)).ToArray());
                            ee3.Add(m_Type2.Select(x => new DB_Input(-1, x)).ToArray());
                            ee3.Add(m_Stereoype2.Select(x => new DB_Input(-1, x)).ToArray());

                            OleDbType[] m_input_Type3 = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                            Data.oLEDB_Interface.Add_Parameters_Select(SELECT3, ee3, m_input_Type3);
                            string[] m_output3 = { "ea_guid" };

                            List<DB_Return> m_ret9 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT3, m_output3);

                            if (m_ret9[0].Ret.Count > 1)
                            {
                                m_ea_guid_Derived = m_ret9[0].Ret.GetRange(1, m_ret9[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
                            }
                            else
                            {
                                m_ea_guid_Derived = null;
                            }
                            */

                            if (m_ea_guid_Derived != null)
                            {
                                int i3 = 0;
                                do
                                {
                                    //Konnektor löschen
                                    //Vorher noch Abfrage ob dieser Konnektor der Einziege is
                                    repository_Connector.Delete_Connector(m_ea_guid_Derived[i3], repository, Data);

                                    i3++;
                                } while (i3 < m_ea_guid_Derived.Count);

                            }
                            //rekursiv für Kinderelemente 
                            Delete_Link_Afo_Sys_Decomposition_rekursiv(m_Object_ID_Child[i2], Data, repository);

                            i2++;
                        } while (i2 < m_Object_GUID_Child.Count);
                    }


                    i1++;
                } while (i1 < m_Object_ID_Logical.Count);
            }

        }

        private void Delete_Link_Afo_Sys_Decomposition_rekursiv(int Parent_ID, Database Data, EA.Repository repository)
        {
          //  DB_Command command = new DB_Command();
            Repository_Connector repository_Connector = new Repository_Connector();
          //  XML xML = new XML();
          //  TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            List<string> m_Requirement_Type = Data.metamodel.m_Requirement.Select(x => x.Type).ToList();
            List<string> m_Requirement_Stereotype = Data.metamodel.m_Requirement.Select(x => x.Stereotype).ToList();
            List<string> m_Usage_Type = Data.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Usage_Stereotype = Data.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();
            List<string> m_Type_Con = Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
            List<string> m_Stereoype_Con = Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();


            if (Parent_ID != null)
            {
                //Kinderelement erhalten
            /*    string SQL_Child = "SELECT Object_ID FROM t_object WHERE ParentID = " + Parent_ID + " AND Object_Type IN"+xML.SQL_IN_Array(m_Usage_Type.ToArray())+";";
                string xml_String_Child = repository.SQLQuery(SQL_Child);
                List<string> m_Object_ID_Child = xML.Xml_Read_Attribut("Object_ID", xml_String_Child);
*/
                List<string> m_Object_ID_Child = new List<string>();
                Repository_Element repository_Parent = new Repository_Element();
                repository_Parent.ID = Parent_ID;
                repository_Parent.Classifier_ID = repository_Parent.Get_GUID_By_ID(Parent_ID, Data);
                Interface_Element interface_Element = new Interface_Element();
                Interface_Connectors interface_Connectors = new Interface_Connectors();
                m_Object_ID_Child = interface_Element.Get_Children_Element(repository_Parent.Classifier_ID, Data, m_Usage_Type, m_Usage_Stereotype);
               /* string SQL_Child = "SELECT Object_ID FROM t_object WHERE ParentID = " + Parent_ID + " AND Object_Type IN(" + command.Add_Parameters_Pre(m_Usage_Type.ToArray()) + ");";
                OleDbCommand SELECT2 = new OleDbCommand(SQL_Child, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                List<DB_Input[]> ee2 = new List<DB_Input[]>();
                ee2.Add(m_Usage_Type.Select(x => new DB_Input(-1, x)).ToArray());

                OleDbType[] m_input_Type2 = { OleDbType.VarChar };
                Data.oLEDB_Interface.Add_Parameters_Select(SELECT2, ee2, m_input_Type2);
                string[] m_output2 = { "Object_ID" };

                List<DB_Return> m_ret8 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT2, m_output2);

                if (m_ret8[0].Ret.Count > 1)
                {
                    m_Object_ID_Child = m_ret8[0].Ret.GetRange(1, m_ret8[0].Ret.Count - 1).ToList().Select(x => (int)x).ToList();
                }
                else
                {
                    m_Object_ID_Child = null;
                }*/


                if (m_Object_ID_Child != null)
                {

                 
                    int i2 = 0;
                    do
                    {
                        //Alle Konektoren zwischen Kinderelement und Requirement erhalten --> Derived_From
                    /*    string SQL_Derived = "SELECT ea_guid FROM t_connector WHERE End_Object_ID = " + m_Object_ID_Child[i2] + " AND Connector_Type IN" + xML.SQL_IN_Array(Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList().ToArray()) + " AND Stereotype IN" + xML.SQL_IN_Array(Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList().ToArray()) + " AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN"+xML.SQL_IN_Array(m_Requirement_Type.ToArray())+");";
                        string xml_String_Derived = repository.SQLQuery(SQL_Derived);
                        List<string> m_ea_guid_Derived = xML.Xml_Read_Attribut("ea_guid", xml_String_Derived);
*/
                        List<string> m_ea_guid_Derived = new List<string>();

                        m_ea_guid_Derived = interface_Connectors.Get_Connector_From_Supplier_Property(Data, m_Object_ID_Child[i2], m_Usage_Type, m_Usage_Stereotype, "ea_guid", m_Requirement_Type, m_Requirement_Stereotype, m_Type_Con, m_Stereoype_Con);

                     /*   string SQL_Derived2 = "SELECT ea_guid FROM t_connector WHERE End_Object_ID = " + m_Object_ID_Child[i2] + " AND Connector_Type IN(" + command.Add_Parameters_Pre(m_Type2.ToArray()) + ") AND Stereotype IN(" + command.Add_Parameters_Pre(m_Stereoype2.ToArray()) + ") AND Start_Object_ID IN (SELECT Object_ID FROM t_object WHERE Object_Type IN(" + command.Add_Parameters_Pre(m_Requirement_Type.ToArray()) + "));";
                    
                        OleDbCommand SELECT3 = new OleDbCommand(SQL_Derived2, (OleDbConnection)Data.oLEDB_Interface.dbConnection);
                        List<DB_Input[]> ee3 = new List<DB_Input[]>();
                        ee3.Add(m_Requirement_Type.Select(x => new DB_Input(-1, x)).ToArray());
                        ee3.Add(m_Type2.Select(x => new DB_Input(-1, x)).ToArray());
                        ee3.Add(m_Stereoype2.Select(x => new DB_Input(-1, x)).ToArray());

                        OleDbType[] m_input_Type3 = { OleDbType.VarChar, OleDbType.VarChar, OleDbType.VarChar };
                        Data.oLEDB_Interface.Add_Parameters_Select(SELECT3, ee3, m_input_Type3);
                        string[] m_output3 = { "ea_guid" };

                        List<DB_Return> m_ret9 = Data.oLEDB_Interface.oleDB_SELECT_One_Table(SELECT3, m_output3);

                        if (m_ret9[0].Ret.Count > 1)
                        {
                            m_ea_guid_Derived = m_ret9[0].Ret.GetRange(1, m_ret9[0].Ret.Count - 1).ToList().Select(x => x.ToString()).ToList();
                        }
                        else
                        {
                            m_ea_guid_Derived = null;
                        }
                        */

                        if (m_ea_guid_Derived != null)
                        {
                            int i3 = 0;
                            do
                            {
                                //Konnektor löschen
                                repository_Connector.Delete_Connector(m_ea_guid_Derived[i3], repository, Data);

                                i3++;
                            } while (i3 < m_ea_guid_Derived.Count);

                        }
                        //rekursiv für Kinderelemente 
                        repository_Parent.ID = -1;
                        repository_Parent.Classifier_ID = m_Object_ID_Child[i2];
                        repository_Parent.ID = repository_Parent.Get_Object_ID(Data);
                        Delete_Link_Afo_Sys_Decomposition_rekursiv(repository_Parent.ID, Data, repository);


                        i2++;
                    } while (i2 < m_Object_ID_Child.Count);


                }
            }
        }

        #endregion Delete

        #region Check
/// <summary>
/// Es wird überprüft, welche Requirements imporitert wurden. Nicht importierte Requirement werden mit einem Issue versehen
/// </summary>
/// <param name="Data"></param>
/// <param name="repository"></param>
        private void Check_Requirement(Database Data, EA.Repository repository)
        { 
            List<string> m_Type_con = new List<string>();
            m_Type_con.Add("Dependency");
            List<string> m_Stereotype_con = new List<string>();
            m_Stereotype_con.Add("trace");
            List<string> m_Toolbox_con = new List<string>();
            m_Toolbox_con.Add("");
            Repository_Element repository_Element = new Repository_Element();
            Repository_Connector con = new Repository_Connector();
            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);

            #region Vorhandene Req nicht imporitert
            //Überprüfung nicht importierte, vorhanden AFO
            List<string> m_Stereotype = this.m_StereoType_Requirement;
            List<string> m_Type = this.m_Type_Requirement;

            List<string> m_Guid = new List<string>();
            Interface_Collection interface_collection = new Interface_Collection();
            m_Guid = interface_collection.Get_Elements_GUID(Data, m_Type, m_Stereotype);
      

            if (m_Guid != null)
            {
                DateTime time = new DateTime();

                string Name8 = DateTime.Now.ToString() +" "+ Data.metamodel.issues.m_Issue_Name[8];

                this.m_GUID_Requirement = m_GUID_Requirement_New.Concat(m_GUID_Requirement_NonUpdate).ToList().Concat(m_GUID_Requirement_Update).ToList();

                int i1 = 0;
                do
                {
                    if(m_GUID_Requirement.Contains(m_Guid[i1]) == false)
                    {
                        Requirement recent_req = new Requirement(m_Guid[i1], Data.metamodel);
                        recent_req.Get_Tagged_Values_From_Requirement(m_Guid[i1], repository, Data);

                        if(recent_req.RPI_Export == true)
                        {
                            Issue issue = new Issue(Data, Name8, Data.metamodel.issues.m_Issue_Note[8], Package_guid, repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Guid[i1], m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                        }

                       
                    }

                    i1++;
                } while (i1 < m_Guid.Count);
            }
            #endregion Überprüfung nicht importierte Req

            #region Neu erzeugte Req
            if(this.m_GUID_Requirement_New.Count > 0)
            {
                string Name10 = DateTime.Now.ToString() + " " + Data.metamodel.issues.m_Issue_Name[10];
                Issue issue2 = new Issue(Data, Name10, Data.metamodel.issues.m_Issue_Note[10], Package_guid, repository, true, null);

                int i2 = 0;
                do
                {
                    con.Create_Dependency(issue2.Classifier_ID, m_GUID_Requirement_New[i2], m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                    i2++;
                } while (i2 < this.m_GUID_Requirement_New.Count);
            }
            #endregion Neu erzeugte Req

            #region Update Req
            if (this.m_GUID_Requirement_Update.Count > 0)
            {
                string Name9 = DateTime.Now.ToString() + " " + Data.metamodel.issues.m_Issue_Name[9];
                Issue issue3 = new Issue(Data, Name9, Data.metamodel.issues.m_Issue_Note[9], Package_guid, repository, true, null);

                int i3 = 0;
                do
                {
                    con.Create_Dependency(issue3.Classifier_ID, m_GUID_Requirement_Update[i3], m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                    i3++;
                } while (i3 < this.m_GUID_Requirement_Update.Count);
            }
            #endregion Update Req

            #region Nicht Update Req
            if (this.m_GUID_Requirement_NonUpdate.Count > 0)
            {
                string Name11 = DateTime.Now.ToString() + " " + Data.metamodel.issues.m_Issue_Name[11];
                Issue issue4 = new Issue(Data, Name11, Data.metamodel.issues.m_Issue_Note[11], Package_guid, repository, true, null);

                int i4 = 0;
                do
                {
                    con.Create_Dependency(issue4.Classifier_ID, m_GUID_Requirement_NonUpdate[i4], m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                    i4++;
                } while (i4 < this.m_GUID_Requirement_NonUpdate.Count);
            }
            #endregion Nicht Update Req
        }
        /// <summary>
        /// Es wird überprüft, welche Systemelelente imporitert wurden. Nicht importierte Systemelelente werden mit einem Issue versehen
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="repository"></param>
        private void Check_SysElem(Database Data, EA.Repository repository)
        {
            List<string> m_Type_con = new List<string>();
            m_Type_con.Add("Dependency");
            List<string> m_Stereotype_con = new List<string>();
            m_Stereotype_con.Add("trace");
            List<string> m_Toolbox_con = new List<string>();
            m_Toolbox_con.Add("trace");
            Repository_Element repository_Element = new Repository_Element();
            Repository_Connector con = new Repository_Connector();
            string Package_guid = repository_Element.Create_Package_Model(Data.metamodel.m_Package_Name[2], repository, Data);

            #region Vorhandene Req nicht imporitert
            //Überprüfung nicht importierte, vorhanden Systemelemente

            List<string> m_Type_Funktionsbaum = Data.metamodel.m_Capability.Select(x => x.Type).ToList();
            List<string> m_Type_TechnischesSystem = Data.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Type_Szenar = Data.metamodel.m_Szenar.Select(x => x.Type).ToList();

            List<string> m_Stereotype_Funktionsbaum = Data.metamodel.m_Capability.Select(x => x.Stereotype).ToList();
            List<string> m_Stereotype_TechnischesSystem = Data.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Stereotype_Szenar = Data.metamodel.m_Szenar.Select(x => x.Stereotype).ToList();

            List<string> m_Type = m_Type_Funktionsbaum.Concat(m_Type_TechnischesSystem).ToList().Concat(m_Type_Szenar).ToList();
            List<string> m_Stereotype = m_Stereotype_Funktionsbaum.Concat(m_Stereotype_TechnischesSystem).ToList().Concat(m_Stereotype_Szenar).ToList();

            List<string> m_Guid = new List<string>();
            Interface_Collection interface_collection = new Interface_Collection();
            m_Guid = interface_collection.Get_Elements_GUID(Data, m_Type, m_Stereotype);


            if (m_Guid != null)
            {
                DateTime time = new DateTime();

                string Name8 = DateTime.Now.ToString() + " " + Data.metamodel.issues.m_Issue_Name[20];

                this.m_GUID_Systemelement = m_GUID_Systemelement_New.Concat(m_GUID_Systemelement_NonUpdate).ToList().Concat(m_GUID_Systemelement_Update).ToList();

                int i1 = 0;
                do
                {
                    if (m_GUID_Systemelement.Contains(m_Guid[i1]) == false)
                    {
                        Repository_Class recent_req = new Repository_Class();
                        recent_req.Classifier_ID = m_Guid[i1];
                        recent_req.Instantiate_GUID = m_Guid[i1];
                        recent_req.Get_TV_Instantiate(Data, repository);

                        if (recent_req.RPI_Export == true)
                        {
                            Issue issue = new Issue(Data, Name8, Data.metamodel.issues.m_Issue_Note[20], Package_guid, repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Guid[i1], m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);
                        }


                    }

                    i1++;
                } while (i1 < m_Guid.Count);
            }
            #endregion Überprüfung nicht importierte Req

            #region Neu erzeugte Req
            if (this.m_GUID_Systemelement_New.Count > 0)
            {
                string Name10 = DateTime.Now.ToString() + " " + Data.metamodel.issues.m_Issue_Name[22];
                Issue issue2 = new Issue(Data, Name10, Data.metamodel.issues.m_Issue_Note[22], Package_guid, repository, true, null);

                int i2 = 0;
                do
                {
                    con.Create_Dependency(issue2.Classifier_ID, m_GUID_Systemelement_New[i2], m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                    i2++;
                } while (i2 < this.m_GUID_Systemelement_New.Count);
            }
            #endregion Neu erzeugte Req

            #region Update Req
            if (this.m_GUID_Systemelement_Update.Count > 0)
            {
                string Name9 = DateTime.Now.ToString() + " " + Data.metamodel.issues.m_Issue_Name[21];
                Issue issue3 = new Issue(Data, Name9, Data.metamodel.issues.m_Issue_Note[21], Package_guid, repository, true, null);

                int i3 = 0;
                do
                {
                    con.Create_Dependency(issue3.Classifier_ID, m_GUID_Systemelement_Update[i3], m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                    i3++;
                } while (i3 < this.m_GUID_Systemelement_Update.Count);
            }
            #endregion Update Req

            #region Nicht Update Req
            if (this.m_GUID_Systemelement_NonUpdate.Count > 0)
            {
                string Name11 = DateTime.Now.ToString() + " " + Data.metamodel.issues.m_Issue_Name[23];
                Issue issue4 = new Issue(Data, Name11, Data.metamodel.issues.m_Issue_Note[23], Package_guid, repository, true, null);

                int i4 = 0;
                do
                {
                    con.Create_Dependency(issue4.Classifier_ID, m_GUID_Systemelement_NonUpdate[i4], m_Stereotype_con, m_Type_con, null, repository, Data, m_Toolbox_con[0], true);

                    i4++;
                } while (i4 < this.m_GUID_Systemelement_NonUpdate.Count);
            }
            #endregion Nicht Update Req
        }
        /// <summary>
        /// Es wird überprüft, ob alle Konnektoren der Database in der Compare List sind
        /// </summary>
        /// <param name="m_Type"></param>
        /// <param name="m_Stereotype"></param>
        /// <param name="m_GUID_Compare"></param>
        /// <param name="database"></param>
        private List<string> Check_Konnektoren(List<string> m_Type, List<string> m_Stereotype, List<string> m_GUID_Compare, Database database, List<string> m_Type_Client, List<string> m_Stereotype_Client, List<string> m_Type_Supplier, List<string> m_Stereotype_Supplier)
        {
            List<string> m_GUID_ret = new List<string>();
            Interface_Connectors interface_Connectors = new Interface_Connectors();

            //Alle Konnektoren des Type und Stereotype erhalten
            List<string> m_GUID_Database =  interface_Connectors.Get_Connector_By_Type_and_Stereotype(database, m_Type, m_Stereotype, m_Type_Client, m_Stereotype_Client, m_Type_Supplier, m_Stereotype_Supplier);

            if(m_GUID_Database != null)
            {

                if (m_GUID_Database.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        int help_index = m_GUID_Compare.FindIndex(x => x == m_GUID_Database[i1]);

                        // int help2_index = 

                        if (help_index == -1) //Konnektor wurde nicht importiert
                        {
                            m_GUID_ret.Add(m_GUID_Database[i1]);
                        }

                        i1++;
                    } while (i1 < m_GUID_Database.Count);
                }
            }


            return (m_GUID_ret);

        }

        #endregion Check

        #region Issue
        private void Create_Issue_Connectoren(string IssueName, string IssueNote, List<string> m_con_guid, string package_guid, Database database, EA.Repository repository )
        {
            EA.Connector con;
            //Issue erzeugen
            Issue issue = new Issue(database, IssueName, IssueNote, package_guid, repository, true, null);
            //Issue verknüpfen
            if(m_con_guid.Count > 0)
            {
                Repository_Connector repository_Connector = new Repository_Connector();
                int i1 = 0;
                do
                {
                    con = repository.GetConnectorByGuid(m_con_guid[i1]);
                    repository_Connector.Create_Dependency(issue.Classifier_ID, repository.GetElementByID(con.ClientID).ElementGUID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);
                    repository_Connector.Create_Dependency(issue.Classifier_ID, repository.GetElementByID(con.SupplierID).ElementGUID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);
                    issue.Client_GUID = repository.GetElementByID(con.ClientID).ElementGUID;
                    issue.Supplier_GUID = repository.GetElementByID(con.SupplierID).ElementGUID;

                    repository_Connector.Create_ConnectorProxy_Dependency_Supplier(m_con_guid[i1], issue.Classifier_ID, database.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), database.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), database.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, database, database.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], database.metamodel.m_Con_Trace[0].direction);

                    i1++;
                } while (i1 < m_con_guid.Count);
            }
        }
        #endregion

        #region Nachweisart
        private void Create_Nachweisart(Database database, EA.Repository repository, string Package_GUID)
        {
            if(this.m_GUID_Requirement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Requirement req = new Requirement(null, database.metamodel);
                    req.Classifier_ID = m_GUID_Requirement[i1];
                    req.ID = req.Get_Object_ID(database);
                    req.Name = req.Get_Name(database);
                    req.Get_TV_Nachweisart(database);

                    req.Get_Nachweisarten(database, repository, true);

                    if(req.m_Nachweisarten.Count == 0)
                    {
                        req.Create_Nachweisart(Package_GUID, database, repository);
                    }
                    else
                    {
                        req.Update_Nachweisart(database, repository);
                    }
                    

                    i1++;
                } while (i1 < this.m_GUID_Requirement.Count);
            }
        }
        #endregion

        #region in Bearbeitung
        //Aktuell nicht genutzt, da keine eindeutiger Import stattfinden kann
        /*    private void Import_Create_InformationFlow(EA.Repository repository, Database Data)
            {
                XML xML = new XML();
                //Alle "Send" Konnektoren erhalten
                string SQL_Send = "SELECT ea_guid FROM t_connector WHERE Connector_Type IN" +xML.SQL_IN_Array(Data.metamodel.m_Afo_Requires.Select(x => x.Type).ToList().ToArray()) + " AND Stereotype IN" +xML.SQL_IN_Array(Data.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList().ToArray()) + ";";
                string xml_String_Send = repository.SQLQuery(SQL_Send);
                List<string> m_ea_guid_Send = xML.Xml_Read_Attribut("ea_guid", xml_String_Send);

                if(m_ea_guid_Send != null)
                {
                    int i1 = 0;
                    do
                    {
                        //Derived_From von Source und Target erhalten

                        i1++;
                    } while (i1 < m_ea_guid_Send.Count);
                }


                //Source Knoten und Target Knoten müssen im selben Szenar liegen

                //InformationFlow anlegen
            }
            */
        #endregion In Bearbeitung
    }
}

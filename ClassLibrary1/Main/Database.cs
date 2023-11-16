using System;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;
using System.Data.Odbc;

using Database_Connection;
using Ennumerationen;
using Metamodels;
using Repsoitory_Elements;
using Requirements;
using Elements;
using Forms;
using System.Threading;
using System.Threading.Tasks;
using Requirement_Plugin.xml;

using Repsoitory_Elements;
using Microsoft.Office.Interop.Excel;

namespace Requirement_Plugin
{
    public class Database
    {

        public OLEDB_Interface oLEDB_Interface;
        public ODBC_Interface oDBC_Interface;
        public SQLITE_Interface SQLITE_Interface;
        public Requirement_PluginClass Base;
        //Allgemeine Attribute
        public string DB_Stand;
        public string Author;
        public AFO_ENUM AFO_ENUM;
        public SYS_ENUM SYS_ENUM;
        public ST_ENUM ST_ENUM;
        public Ennumeration.NACHWEIS_ENUM nACHWEIS_ENUM;

        public List<string> m_StereoType; // Auslagerung in Metamodelm_StereoType
        public Metamodel metamodel;

        public List<NodeType> m_NodeType;
        public List<SysElement> m_SysElemente;
        public List<NodeType> m_Typvertreter;
        public List<InformationElement> m_InformationElement;
        public List<OperationalConstraint> m_DesignConstraint;
        public List<OperationalConstraint> m_UmweltConstraint;
        public List<OperationalConstraint> m_ProcessConstraint;
        public List<Logical> m_Logical;
        public List<Logical> m_NodeType_Logical;
        public Logical Decomposition;
        public List<SysElement> m_Decomposition_Sys;
        public NodeType NT_help;
        public NodeType NT_help1;
        private List<NodeType> Add_NodeType;
        public List<Nachweisart> m_Nachweisarten;
        public List<Abnahmekriterium> m_Abnahmekriterium;

        public List<Capability> m_Capability;
        public List<Repository_Elements.Catalogue> m_Catalogue;
        public List<Activity> m_Activity;
        public List<Stakeholder> m_Stakeholder;
        public List<Requirement> m_Requirement;
        public List<Pool> m_Pools;
        public List<Package> m_Packages;
        public List<Repository_Elements.Diagram> m_Diagram;

        public List<Requirement_Plugin.Repository_Elements.MeasurementType> m_MeasurementType;

        public bool flag_modus;

        //Export Attribute
        public bool sys_xac;
       // public bool afo_Export;
        public bool afo_interface_xac;
        public bool afo_funktional_xac;
        public bool afo_user_xac;
        public bool afo_design_xac;
        public bool afo_process_xac;
        public bool afo_umwelt_xac;
        public bool afo_typevertreter_xac;
        public bool logical_xac;
        public bool capability_xac;
        public bool stakeholder_xac;
        public bool link_decomposition;
        public bool link_afo_sys;
        public bool link_afo_afo;
        public bool link_afo_cap;
        public bool link_afo_logical;
        public bool link_afo_st;
        public bool glossary_xac;
        public bool Nachweisart_xac;
        public bool Logical_aufloesung_xac;
        public bool Check_xac;

        

        public List<string> Synch;
        public List<string> m_package;


        public Loading_OpArch Load;

        #region Constructor

        public Database(Metamodel metamodel, EA.Repository repository, Requirement_PluginClass Base)
        {
         

            this.m_NodeType = new List<NodeType>();
            this.m_SysElemente = new List<SysElement>();
            this.m_Decomposition_Sys = new List<SysElement>();
         //   this.Decomposition = new Logical();
            this.m_InformationElement = new List<InformationElement>();
            this.m_DesignConstraint = new List<OperationalConstraint>();
            this.m_UmweltConstraint = new List<OperationalConstraint>();
            this.m_ProcessConstraint = new List<OperationalConstraint>();
            this.m_Logical = new List<Logical>();
            this.m_NodeType_Logical = new List<Logical>();
            this.Add_NodeType = new List<NodeType>();
            this.m_Capability = new List<Capability>();
            this.m_Catalogue = new List<Repository_Elements.Catalogue>();
            this.m_Activity = new List<Activity>();
            this.m_Stakeholder = new List<Stakeholder>();
            this.m_Requirement = new List<Requirement>();
            this.m_Typvertreter = new List<NodeType>();
            this.m_Pools = new List<Pool>();
            this.m_Nachweisarten = new List<Nachweisart>();
            this.m_Abnahmekriterium = new List<Abnahmekriterium>();
            this.m_MeasurementType = new List<Repository_Elements.MeasurementType>();
            this.m_Packages = new List<Package>();
            this.m_Diagram = new List<Repository_Elements.Diagram>();
            //Export Attribute
            this.sys_xac = false;
            this.afo_interface_xac = false;
            this.afo_funktional_xac = false;
            this.afo_user_xac = false;
            this.afo_design_xac = false;
            this.afo_process_xac = false;
            this.afo_umwelt_xac = false;
            this.afo_typevertreter_xac = false;
            this.logical_xac = false;
            this.capability_xac = false;
            this.stakeholder_xac = false;

            this.link_decomposition = false;
            this.link_afo_sys = false;
            this.link_afo_afo = false;
            this.link_afo_cap = false;
            this.link_afo_logical = false;

          

            this.glossary_xac = false;
            this.Nachweisart_xac = false;
            this.Logical_aufloesung_xac = false;

            this.m_package = new List<string>();
            ///////////////////////
            //StereoTypen, welche zum Aufbau der Database zugelassen werden
            //      this.m_StereoType = new List<string>();
            //      this.m_StereoType.Add("Node"); //Part
            //      this.m_StereoType.Add("NodeType"); //Class
            //      this.m_StereoType.Add("KnownResource"); // Part
            ///////////////////////
            this.metamodel = metamodel;

            this.AFO_ENUM = new AFO_ENUM();
            this.SYS_ENUM = new SYS_ENUM();
            this.ST_ENUM = new ST_ENUM();
            this.nACHWEIS_ENUM = new Ennumeration.NACHWEIS_ENUM();

            this.Base = Base;

            switch(this.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    this.oLEDB_Interface = new OLEDB_Interface(repository, this.Base);
                    break;
                case DB_Type.MSDASQL:
                    this.oDBC_Interface = new ODBC_Interface(repository, this.Base);
                    break;
                case DB_Type.SQLITE:
                    this.SQLITE_Interface = new SQLITE_Interface(repository, this.Base);
                    break;
                default:
                    this.oLEDB_Interface = new OLEDB_Interface(repository, this.Base);
                    break;
            }
          

            this.Synch = new List<string>();

        }

        #endregion Constructor

        /// <summary>
        /// Es wird eine Datenbank OpArch erzeugt mit Dekomposition der LogicalArchitectures
        /// Hierbei werden die Connectoren und Requirements entsprechnd der Hierachie zugeordnet
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Database_OpArch"></param>
        public void Create_OpArch_Decomposition(EA.Repository Repository, Database Database_OpArch, Requirement_PluginClass Base)
        {
            /////////////////////////////////////
            //Auswahl Datenbank
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();

          /*  if (metamodel.dB_Type == DB_Type.MSDASQL)
            {
                Repository_Elements repository_Elements = new Repository_Elements();
                List<string> m_GUID_Logical = repository_Elements.Get_Logicals_GUID2(this);
                repository_Elements.Test(this);

                MessageBox.Show(m_GUID_Logical.Count.ToString());
            }*/
            ////////////////////////////////////
            //Zurücksetzen der Vraiablen
            //   try
            //   {
            this.m_NodeType.Clear();
            this.m_Typvertreter.Clear();
            this.m_Activity.Clear();
            this.m_Capability.Clear();
            this.m_InformationElement.Clear();
            this.m_Logical.Clear();
            this.m_Requirement.Clear();
            this.m_Stakeholder.Clear();
            this.m_DesignConstraint.Clear();
            this.m_UmweltConstraint.Clear();
            this.m_ProcessConstraint.Clear();
            this.m_Nachweisarten.Clear();
            this.m_Abnahmekriterium.Clear();
            this.m_MeasurementType.Clear();
            //   this.Decomposition.Clear();
            this.m_Pools.Clear();
            this.m_SysElemente.Clear();



            /////////////////////////////////////            
            //Es wurde mind ein Pattern ausgewählt, welches analysiert werden soll
            if (Database_OpArch.metamodel.m_Pattern_flag.Contains(true) == true)
            {
                TaggedValue Tagged = new TaggedValue(Database_OpArch.metamodel, Database_OpArch);
                Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
                Repository_Class repository_Element = new Repository_Class();

                interface_Collection_OleDB.Open_Connection(this);
                //////////////////////////test
                ////////////////////////////////
                ///////////////
                //Ladebalken anlegen
                this.Load = new Loading_OpArch();
                this.Load.label2.Text = "...";
                Load.progressBar1.Minimum = 0;
                Load.progressBar1.Maximum = 10;
                Load.label_Progress.Text = "Initialisierung Database OpArch";
                Load.Show();
                ///////////////////////////////////////////
                //Alle Logicals anlegen
                #region Logical anlegen
                Load.label_Progress.Text = "Anlegen Logical";
                Load.progressBar1.PerformStep();
                Load.Refresh();

                repository_Elements.Get_Logicals_Parallel(this);

                if (this.m_Logical.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (Tagged.Get_Tagged_Value(m_Logical[i1].Classifier_ID, "SYS_AG_ID", Repository) == null || Tagged.Get_Tagged_Value(m_Logical[i1].Classifier_ID, "SYS_AG_ID", Repository) == "kein")
                        {
                            Tagged.Insert_Tagged_Value(m_Logical[i1].Classifier_ID, "SYS_AG_ID", this.metamodel.m_Header_agid[1] + " " + (i1 + 1).ToString(), null, Repository);
                        }
                        i1++;
                    } while (i1 < this.m_Logical.Count);
                }


                #endregion Logical anlegen

                #region Logical Decomposition anlegen

               
                interface_Collection_OleDB.Open_Connection(this);

                this.Decomposition = new Logical(null, Database_OpArch);
                this.Decomposition.m_NodeType = new List<NodeType>();
                this.Decomposition.Addin = true;
                #endregion

                #region Logical auswählen

                Requirement_Plugin.Forms.Auswahl_Anwendungsfall auswahl_Anwendungsfall = new Forms.Auswahl_Anwendungsfall(this.m_Logical);

                auswahl_Anwendungsfall.ShowDialog();

                this.m_Logical = auswahl_Anwendungsfall.m_ret;
                #endregion

                #region SysElemente erhalten

                if(this.metamodel.flag_sysarch == true)
                {
                    repository_Elements.Get_Systemelemente_Parallel(this, Repository, Load);

                    //Update SysKomponenetnetype
                    if(this.m_SysElemente.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            this.m_SysElemente[i1].Update_SYSKomponententyp(this, Repository);

                            i1++;
                        } while (i1 < this.m_SysElemente.Count);
                    }

                }
               

                #region Archiv
                /* List<SysElement> m_SysElemente2 = new List<SysElement>();
                 /////////////////////////
                 //if (this.metamodel.flag_slow_mode == true)

                     var SysElemente_GUID_List = repository_Elements.Get_SysElemente(this);

                     if (SysElemente_GUID_List != null)
                     {
                         int length = SysElemente_GUID_List.Count;

                         Load.label_Progress.Text = "Anlegen SysElemente";
                         Load.progressBar1.Maximum = length;
                         Load.progressBar1.Value = 0;
                         Load.Refresh();
                     }


                     if (SysElemente_GUID_List != null)
                     {
                         int length = SysElemente_GUID_List.Count;
                         // Schleife Ã¼ber alle NodeTypes
                         //NodeType anlegen und in Database sichern
                         int i = 0;
                         do
                         {
                             Load.progressBar1.PerformStep();
                             Load.Refresh();
                             EA.Element recent = Repository.GetElementByGuid(SysElemente_GUID_List[i]);
                             ////////////////////////
                             if (recent != null)
                             {
                             //NodeType anlegen
                                  SysElement NodeType_recent = new SysElement(SysElemente_GUID_List[i], Repository, this);
                                 NodeType_recent.Classifier_ID = SysElemente_GUID_List[i];
                                 NodeType_recent.Instantiate_GUID = SysElemente_GUID_List[i];
                                 NodeType_recent.Get_TV_Instantiate(this, Repository);
                             //TaggedValue RPI Export


                             //Allg. hinzufügen
                                  m_SysElemente2.Add(NodeType_recent);
                             }
                             i++;
                         } while (i < length);
                         this.m_SysElemente = m_SysElemente2;
                         //Dekompositon anlegen
                         i = 0;
                         do
                         {
                             this.m_SysElemente[i].Get_Children_Class(this, Repository);

                             i++;
                         } while (i < this.m_SysElemente.Count);
                     }
                */
                #endregion
                #endregion Syselemente erhalten

                #region Syselemente auswählen
                if(this.metamodel.flag_sysarch == true)
                {
                    Forms.Analyse.Auswahl_Systemelemente auswahl_Systemelemente = new Forms.Analyse.Auswahl_Systemelemente(this.m_SysElemente);

                    auswahl_Systemelemente.ShowDialog();

                    this.m_SysElemente = auswahl_Systemelemente.m_ret;
                }
                #endregion
                ////////////////////////////////////////////
                #region Anlegen InformationElement
                /////////////////
                string Package_GUID = "";
                /////////////////
                List<Logical> m_Logical2 = new List<Logical>();
                ///

                //Alle InformationElemente anlegen in Database anlegen
                Load.label_Progress.Text = "Anlegen InformationElement";
                Load.progressBar1.Maximum = 6;
                Load.progressBar1.Step = 1;
                Load.Refresh();

                repository_Elements.Get_InformationElement_Parallel(this, Repository);
                #region Archiv
                /* List<string> Information_Element_GUID = new List<string>();
                 List<InformationElement> m_InformationElements2 = new List<InformationElement>();
                 List<Logical> m_Logical2 = new List<Logical>();
                 Information_Element_GUID = repository_Elements.Get_Information_Element(this);
                 //Ladebalken aktualisieren
                 Load.label_Progress.Text = "Anlegen InformationElement";
                 // Load.label_Text.Text = "...";
                 if (Information_Element_GUID != null)
                 {
                     Load.progressBar1.Maximum = Information_Element_GUID.Count;
                     Load.progressBar1.Step = 1;
                     Load.Refresh();
                 }

                 //////////////////////////
                 //Schleife Ã¼ber alle InformationElement
                 if (Information_Element_GUID != null)
                 {
                     //Anlegen InfoELem und zuordnen zur Database
                     int t1 = 0;
                     do
                     {
                         Load.progressBar1.PerformStep();
                         Load.Refresh();

                         InformationElement recent = new InformationElement(Information_Element_GUID[t1]);

                         m_InformationElements2.Add(recent);

                         t1++;
                     } while (t1 < Information_Element_GUID.Count);
                 }
                 Database_OpArch.m_InformationElement = m_InformationElements2;
                */
                #endregion
                #endregion InformationElement
                ///////////////////////////////////////////
                //Alle Capability anlegen

                #region Capability
                //Ladebalken aktualisieren
                Load.label_Progress.Text = "Anlegen Capability";
                Load.progressBar1.PerformStep();
                Load.Refresh();

                repository_Elements.Get_Capability_Parallel(this, Repository);
                #region Archiv
                /*
                 List<string> m_Capability_GUID = new List<string>();
                 m_Capability_GUID = repository_Elements.Get_Capability_GUID(Repository, this);

                 repository_Elements.Get_Capability(m_Capability_GUID, this, Load, Repository, Tagged);
                */
                #endregion
                #endregion Capability

                ///////////////////////////////////////////
                //Alle Activity anlegen
                #region Activity anlegen
                Load.label_Progress.Text = "Anlegen Activity";
                Load.progressBar1.PerformStep();
                Load.Refresh();

                repository_Elements.Get_Activity_Parallel(this, Repository);

                #region Archiv
                /*
                 * Load.label_Progress.Text = "Anlegen Activity";
                 Load.Refresh();
                 this.m_Activity = repository_Elements.Get_Activity(Repository, this);
                 //Verschachtelung der Activity erhalten
                 if(this.m_Activity.Count  > 0)
                 {
                     int a1 = 0;
                     do
                     {
                         this.m_Activity[a1].Check_Children(Repository, this);

                         a1++;
                     } while (a1 < this.m_Activity.Count);

                 }
                */
                #endregion
                #endregion Activity anlegen

                ///////////////////////////////////////////////////////
               
                ////////////////////////////////////////////
                //Alle NodeType erhalten und Decompositon anlegen
                #region NodeType erhalten
                Load.label_Progress.Text = "Anlegen Logical";
                Load.progressBar1.PerformStep();
                Load.Refresh();

                repository_Elements.Get_NodeTypes_Parallel(this, Repository, Load);

                if(this.m_NodeType.Count > 0)
                {
                    int i = 0;
                    do
                    {
                        if (this.m_NodeType[i].m_Parent.Count == 0)
                        {
                            this.Decomposition.m_NodeType.Add(this.m_NodeType[i]);
                        }

                        i++;
                    } while (i < this.m_NodeType.Count);
                }

                #region Archiv

                /*List<NodeType> m_NodeType2 = new List<NodeType>();
                /////////////////////////
                //if (this.metamodel.flag_slow_mode == true)
                if (this.m_Logical.Count > 0)
                {
                    var NodeType_GUID_List = repository_Elements.Get_NodeTypes(this);

                    if (NodeType_GUID_List != null)
                    {
                        int length = NodeType_GUID_List.Count;

                        Load.label_Progress.Text = "Anlegen NodeType";
                        Load.progressBar1.Maximum = length;
                        Load.progressBar1.Value = 0;
                        Load.Refresh();
                    }


                    if (NodeType_GUID_List != null)
                    {
                        int length = NodeType_GUID_List.Count;
                        // Schleife Ã¼ber alle NodeTypes
                        //NodeType anlegen und in Database sichern
                        int i = 0;
                        do
                        {
                            Load.progressBar1.PerformStep();
                            Load.Refresh();
                            EA.Element recent = Repository.GetElementByGuid(NodeType_GUID_List[i]);
                            ////////////////////////
                            if (recent != null)
                            {
                                //NodeType anlegen
                                NodeType NodeType_recent = new NodeType(NodeType_GUID_List[i], Repository, this);
                                NodeType_recent.Classifier_ID = NodeType_GUID_List[i];
                                NodeType_recent.Instantiate_GUID = NodeType_GUID_List[i];
                                NodeType_recent.Get_TV_Instantiate(this, Repository);
                                //TaggedValue RPI Export


                                //Allg. hinzufügen
                                m_NodeType2.Add(NodeType_recent);
                            }
                            i++;
                        } while (i < length);
                        this.m_NodeType = m_NodeType2;
                        //Dekompositon anlegen
                        i = 0;
                        do
                        {
                            this.m_NodeType[i].Get_Children_Class(this, Repository);

                            i++;
                        } while (i < this.m_NodeType.Count);
                  

                        i = 0;
                        do
                        {
                            if (this.m_NodeType[i].m_Parent.Count == 0)
                            {
                                this.Decomposition.m_NodeType.Add(this.m_NodeType[i]);
                            }

                            i++;
                        } while (i < this.m_NodeType.Count);


                    }
                }
                */
                #endregion
                #endregion NodeType erhalten
                ////////////////////////////////////

                

                #region Nachweisarten erhalten

                repository_Elements.Get_Nachweisarten_Parallel(this, Repository, Load);
                #endregion

                #region Abnahmekritreien erhalten
                repository_Elements.Get_Abnahemkriterium_Parallel(this, Repository, Load);
                #endregion

                #region MeasurementType erhalten
                Load.label_Progress.Text = "Anlegen MeasurementType";
                Load.progressBar1.PerformStep();
                Load.Refresh();

                repository_Elements.Get_MeasurementType_Parallel(this, Repository, Load);


                #endregion

                #region Packages & Misc
                //Anlegen eines Packages zum Ablegen der Decomposition

                Interface_Package interface_Package = new Interface_Package();
                string pack_guid = interface_Package.Create_Package_Model(this, Repository, this.metamodel.m_Package_Name[2]);
                
                var recetn_pck = Repository.GetPackageByGuid(pack_guid);

                string pack_guid2 = interface_Package.Create_Package(this, Repository, recetn_pck, this.metamodel.m_Package_Name[4], false);

                m_package.Add(pack_guid);
                m_package.Add(pack_guid2);
                //Es wird geprÃ¼ft ob Sammel Logical Architecture schon vorhanden
                Logical Remove = this.Check_Logical(Decomposition.Classifier_ID);

                if (Remove != null)
                {
                    //Sammel Logical Architecture vorhanden
                    //Wird entfernt um nicht alle Informationen/Decomposition mehrfach zu betrachten
                    this.m_Logical.Remove(Remove);
                }
                //////////////////////////////

                List<string> m_Type = this.metamodel.m_Elements_OpArch_Definition.Select(x => x.Type).ToList();
                List<string> m_Stereotype = this.metamodel.m_Elements_OpArch_Definition.Select(x => x.Stereotype).ToList();

                /////////////////////////////////////
                //     EA.Element LA_help = Repository.GetElementByGuid(Logical_GUID);
                ///////////////////////////////////////
                //Auswahl Logical und NodeType zum Anlegen der Decomposition
                //Form zur Auswahl
                //ausgewÃ¤hlte NoddyYpes sind jetzt Logical


                //OpArch_Create1 Choose_Logical = new OpArch_Create1(Database_OpArch, Repository);
                //Choose_Logical.ShowDialog();
                //aktuelle NodeTyp lÃ¶schen, da nun neue aufgebaut werden
                //m_NodeType.Clear();

                #endregion Packages & Misc
                /////////////////////
                //Decomposition
                #region Aufbau Decomposition


                Load.label_Progress.Text = "Anlegen Decomposition - " + this.metamodel.m_Szenar[0].DefaultName + " Elemente ";
                Load.progressBar1.Maximum = this.m_Logical.Count;
                Load.progressBar1.Value = 0;
                Load.Refresh();
                ///////////////////////////////////////////////////
                ///Decomposition aufbauen
                #region Logical betrachten
                //Schleife Ã¼ber zunÃ¤chst alle Logical

                this.Create_Dekomposition(Repository, Package_GUID);
               
                #endregion Logical betrachten
                ///////////////////////////////////////////////////
                #region BPMN Lanes
                if (this.m_NodeType.Count > 0 && this.metamodel.flag_bpmn == true)
                {
                    int p1 = 0;
                    do
                    {
                        this.m_NodeType[p1].Get_Pools(this);
                        this.m_NodeType[p1].Get_Lanes(this);
                        if (this.m_NodeType[p1].m_Pools.Count > 0)
                        {
                            int p2 = 0;
                            do
                            {
                                this.m_NodeType[p1].m_Pools[p2].Get_Event(this);

                                p2++;
                            } while (p2 < this.m_NodeType[p1].m_Pools.Count);
                        }
                        if (this.m_NodeType[p1].m_Lanes.Count > 0)
                        {
                            int p3 = 0;
                            do
                            {
                                this.m_NodeType[p1].m_Lanes[p3].Get_Event(this);

                                p3++;
                            } while (p3 < this.m_NodeType[p1].m_Lanes.Count);
                        }


                        p1++;
                    } while (p1 < this.m_NodeType.Count);
                }
                #endregion BPMN LAnes
                #endregion Aufbau Decomposition
                ///////////////////////////////////////
                //Connectoren der Decomposition anlegen
                #region Konnektoren zwischen den Elementen anlegen
                if (Database_OpArch.metamodel.m_Pattern_flag[0] == true)
                {
                    Load.label_Progress.Text = "Anlegen Decomposition - " + this.metamodel.m_Szenar[0].DefaultName + " Connectoren ";
                    Load.progressBar1.Maximum = this.m_NodeType.Count;
                    Load.progressBar1.Step = 1;
                    Load.progressBar1.Value = 0;
                    Load.Refresh();
                    //Schleife Ã¼ber alle LogicalArchitecture


                    #region Über die Decomposition
                    if(this.m_NodeType.Count > 0)
                    {
                       /* Parallel.ForEach(this.Decomposition.m_NodeType, nodeType =>
                        {
                            if (nodeType.m_Instantiate.Count > 0)
                            {
                                nodeType.Get_Connectors_InfoEx(true, this, Repository);
                            }
                        });*/

                        #region Archiv
                        
                        var d1 = 0;
                        do
                        {
                            if (m_NodeType[d1].m_Instantiate.Count > 0)
                            {
                                this.Decomposition.m_NodeType[d1].Get_Connectors_InfoEx(true, this, Repository);
                            }

                            d1++;
                        } while (d1 < this.Decomposition.m_NodeType.Count);
                        
                        #endregion
                    }



                    #endregion Über die Decomposition
                }
                #endregion Konnektoren zwischen den Elementen anlegen
                //////////////////
                //Bidirektionale Connectoren anlegen
                //alle NodeType durchgehen
                #region Bidirektionale Konnektoren aus den Element_Interface heruasfinden
                //Load aktualisieren
                Load.label_Progress.Text = "Anlegen Decomposition - Bidirektionale Connectoren ";
                Load.progressBar1.Maximum = this.m_NodeType.Count;
                Load.progressBar1.Value = 0;
                Load.Refresh();

                if (this.m_NodeType.Count > 0 && Database_OpArch.metamodel.m_Pattern_flag[0] == true)
                {
                   /* Parallel.ForEach(this.m_NodeType, nodeType =>
                    {
                        nodeType.Check_Element_Interface_Bidirectional(this, Repository);
                    });*/
                    #region Archiv
                    //Schleife Ã¼ber alle NodeType
                    
                    int r1 = 0;
                    do
                    {
                        //Load.label_Progress.Text = "Anlegen Decomposition - " + this.m_NodeType[r1].Name + " - Bidirektionale Connectoren ";
                        //Load.progressBar1.PerformStep();
                        //Load.Refresh();
                        this.m_NodeType[r1].Check_Element_Interface_Bidirectional(this, Repository);


                        r1++;
                    } while (r1 < this.m_NodeType.Count);
                   
                    #endregion
                }
                #endregion Bidirektionale Konnektoren aus den Element_Interface heruasfinden

                //////////
                #region AG_ID vergeben
                //SYS_AG_ID festlegen der Systemelemente 
                this.Decomposition_Get_SYS_AGID_ID(Repository, this.metamodel);
                //SYS_AG_ID des Funktionsbaumes festlegen
                this.Database_Get_F_AGID_ID(Repository);
                #endregion AG_ID vergeben


                ///////////////////////////////////////////
                /// ///Alle Stakholder anlegen
                #region Stakeholder anlegen
                if (Database_OpArch.metamodel.m_Pattern_flag[1] == true)
                {
                    Load.label_Progress.Text = "Anlegen Stakeholder";
                    Load.Refresh();

                    repository_Elements.Get_Stakeholder_Parallel(Repository, this);

                    if(this.m_NodeType.Count > 0 && this.m_Stakeholder.Count > 0)
                    {
                        Parallel.ForEach(this.m_NodeType, nodeType =>
                        {
                            nodeType.Get_Stakeholder(Repository, this);
                        });
                    }

                  
                    #region Archiv
                    //Ladebalken aktualisieren
                    /*Load.label_Progress.Text = "Anlegen Stakeholder";

                    List<string> m_GUID_Stakeholder = repository_Elements.Get_Stakeholder_GUID(Repository, this);

                    // Load.label_Text.Text = "...";
                    if (m_GUID_Stakeholder != null)
                    {
                        Load.progressBar1.Maximum = 3 * m_GUID_Stakeholder.Count;
                        Load.progressBar1.Value = 0;
                        Load.progressBar1.Step = 1;
                        Load.Refresh();
                    }
                    if (m_GUID_Stakeholder != null)
                    {
                        int a2 = 0;
                        do
                        {
                            Load.progressBar1.PerformStep();
                            Load.Refresh();

                            Stakeholder recent_stakeholder = new Stakeholder(m_GUID_Stakeholder[a2], Repository, this);
                            this.m_Stakeholder.Add(recent_stakeholder);

                            a2++;
                        } while (a2 < m_GUID_Stakeholder.Count);


                        //Hier erfolgt nun die -zuordnung der Stakholder zu den Nodetype --> Abfrage nach dem Connektor "NodeRealisation"
                        if (this.m_NodeType.Count > 0)
                        {
                            int a3 = 0;
                            do
                            {
                                Load.progressBar1.PerformStep();
                                Load.Refresh();

                                this.m_NodeType[a3].Get_Stakeholder(Repository, this);

                                a3++;
                            } while (a3 < this.m_NodeType.Count);
                        }

                    }
                    */
                    #endregion
                }

                #endregion Stakeholder anlegen
                ///Element_Functional anlegen
                #region Element_Functional anlegen
                Load.label_Progress.Text = "Anlegen Element Functional";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                Load.progressBar1.Refresh();
                if (Database_OpArch.metamodel.m_Pattern_flag[1] == true)
                { 
                    this.Get_Functional_NodeType(Repository, Load);
                }
                #endregion Element_Functional anlegen
                ///////////////////////////////////////////////////////
                ///Element_User anlegen
                #region Element_User anlegen
                if (Database_OpArch.metamodel.m_Pattern_flag[1] == true)
                {
                    this.Get_Element_User(Repository, Load);
                }
                #endregion Element_User anlegen
                ////////////////////////////////////7
                #region DesignRequirement
                Load.label_Progress.Text = "Anlegen Element Design";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                if (Database_OpArch.metamodel.m_Pattern_flag[2] == true)
                {

                    //Schelife über alle NodeType
                    if (this.m_NodeType.Count > 0)
                    {

                        Parallel.ForEach(this.m_NodeType, nodeType =>
                        {
                            try
                            {
                                nodeType.Get_Element_Design(this, Repository);

                                ////Requirements erhalten
                                if (nodeType.m_Design.Count > 0)
                                {
                                    Repository_Connector rep_con = new Repository_Connector();

                                    int d2 = 0;
                                    do
                                    {
                                        nodeType.m_Design[d2].Check_Requirement_Design(this, Repository);

                                        ////////////////
                                        ///Logical erhalten
                                        if (nodeType.m_Design[d2].m_GUID.Count > 0)
                                        {
                                            int d3 = 0;
                                            do
                                            {
                                                Logical log = rep_con.Get_Logical(nodeType.m_Design[d2].m_GUID[d3], null, Repository, this);
                                                if (log != null)
                                                {
                                                    nodeType.m_Design[d2].m_Logical.Add(log);
                                                }
                                                d3++;
                                            } while (d3 < nodeType.m_Design[d2].m_GUID.Count);
                                        }


                                        d2++;
                                    } while (d2 < nodeType.m_Design.Count);
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Design AFO NodeType: " + nodeType.Name + ": " + err.Message);
                            }
                        });
                        #region Archiv
                      /*  
                       *  Load.progressBar1.Value = 0;
                        Load.progressBar1.Maximum = this.m_NodeType.Count - 1;
                        Load.label_Progress.Refresh();
                        int d1 = 0;
                        do
                        {
                            try
                            {
                                this.m_NodeType[d1].Get_Element_Design(this, Repository);

                                ////Requirements erhalten
                                if (this.m_NodeType[d1].m_Design.Count > 0)
                                {
                                    Repository_Connector rep_con = new Repository_Connector();

                                    int d2 = 0;
                                    do
                                    {
                                        this.m_NodeType[d1].m_Design[d2].Check_Requirement_Design(this, Repository);

                                        ////////////////
                                        ///Logical erhalten
                                        if (this.m_NodeType[d1].m_Design[d2].m_GUID.Count > 0)
                                        {
                                            int d3 = 0;
                                            do
                                            {
                                                Logical log = rep_con.Get_Logical(this.m_NodeType[d1].m_Design[d2].m_GUID[d3], null, Repository, this);
                                                if (log != null)
                                                {
                                                    this.m_NodeType[d1].m_Design[d2].m_Logical.Add(log);
                                                }
                                                d3++;
                                            } while (d3 < this.m_NodeType[d1].m_Design[d2].m_GUID.Count);
                                        }


                                        d2++;
                                    } while (d2 < this.m_NodeType[d1].m_Design.Count);
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Design AFO NodeType: " + this.m_NodeType[d1].Name + ": " + err.Message);
                            }
                            

                            Load.progressBar1.PerformStep();
                            Load.progressBar1.Refresh();

                            d1++;
                        } while (d1 < this.m_NodeType.Count);
                      */
                        #endregion
                    }

                }
                #endregion DesignRequirement
                /////////////////////////////////////////////
                #region ProcessRequirement
                Load.label_Progress.Text = "Anlegen Element Prozess";
                Load.progressBar1.Value = 0;
                Load.progressBar1.Refresh();
                if (Database_OpArch.metamodel.m_Pattern_flag[3] == true)
                {
                    //Schleife über alle Activity
                    if (this.m_Activity.Count > 0)
                    {
                        Load.progressBar1.Maximum = this.m_Activity.Count - 1;
                        Load.label_Progress.Refresh();

                        int p1 = 0;
                        do
                        {
                            //try
                            //{

                                if(this.m_Activity[p1].m_GUID.Count > 0) //Wenn m_guid.Count == 0 wurde Activity nicht mit einem System verbunden
                                {
                                    this.m_Activity[p1].Get_Element_Process(this, Repository);
                                }

                                Load.progressBar1.PerformStep();
                                Load.progressBar1.Refresh();

                         /*   }
                            catch (Exception err)
                            {
                                MessageBox.Show("Process AFO Activity: " + this.m_Activity[p1].Name + ": " + err.Message);
                            }*/
                            
                            p1++;
                        } while (p1 < this.m_Activity.Count);

                    }
                }
                #endregion ProcessRequirement
                /////////////////////////////////////////////////
                #region EnviromentalRequirement
                Load.label_Progress.Text = "Anlegen Element Umwelt";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                if (Database_OpArch.metamodel.m_Pattern_flag[4] == true)
                {
                    //Schelife über alle NodeType
                    if (this.m_NodeType.Count > 0)
                    {
                        Parallel.ForEach(this.m_NodeType, nodeType =>
                        {
                            try
                            {
                                nodeType.Get_Element_Umwelt(this, Repository);

                                ////Requirements erhalten
                                if (nodeType.m_Enviromental.Count > 0)
                                {
                                    Repository_Connector rep_con = new Repository_Connector();

                                    int d2 = 0;
                                    do
                                    {
                                        nodeType.m_Enviromental[d2].Check_Requirement_Umwelt(this, Repository);

                                        ////////////////
                                        ///Logical erhalten
                                        if (nodeType.m_Enviromental[d2].m_GUID.Count > 0)
                                        {
                                            int d3 = 0;
                                            do
                                            {
                                                Logical log = rep_con.Get_Logical(nodeType.m_Enviromental[d2].m_GUID[d3], null, Repository, this);
                                                if (log != null)
                                                {
                                                    nodeType.m_Enviromental[d2].m_Logical.Add(log);
                                                }
                                                d3++;
                                            } while (d3 < nodeType.m_Enviromental[d2].m_GUID.Count);
                                        }


                                        d2++;
                                    } while (d2 < nodeType.m_Enviromental.Count);
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Environment AFO NodeType: " + nodeType.Name + ": " + err.Message);
                            }
                        });

                        #region Archiv
                        /*
                        Load.progressBar1.Maximum = this.m_NodeType.Count - 1;
                        Load.label_Progress.Refresh();
                        int d1 = 0;
                        do
                        {
                            try
                            {
                                this.m_NodeType[d1].Get_Element_Umwelt(this, Repository);

                                ////Requirements erhalten
                                if (this.m_NodeType[d1].m_Enviromental.Count > 0)
                                {
                                    Repository_Connector rep_con = new Repository_Connector();

                                    int d2 = 0;
                                    do
                                    {
                                        this.m_NodeType[d1].m_Enviromental[d2].Check_Requirement_Umwelt(this, Repository);

                                        ////////////////
                                        ///Logical erhalten
                                        if (this.m_NodeType[d1].m_Enviromental[d2].m_GUID.Count > 0)
                                        {
                                            int d3 = 0;
                                            do
                                            {
                                                Logical log = rep_con.Get_Logical(this.m_NodeType[d1].m_Enviromental[d2].m_GUID[d3], null, Repository, this);
                                                if (log != null)
                                                {
                                                    this.m_NodeType[d1].m_Enviromental[d2].m_Logical.Add(log);
                                                }
                                                d3++;
                                            } while (d3 < this.m_NodeType[d1].m_Enviromental[d2].m_GUID.Count);
                                        }


                                        d2++;
                                    } while (d2 < this.m_NodeType[d1].m_Enviromental.Count);
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Environment AFO NodeType: " + this.m_NodeType[d1].Name + ": " + err.Message);
                            }
                           
                            Load.progressBar1.PerformStep();
                            Load.progressBar1.Refresh();
                            d1++;
                        } while (d1 < this.m_NodeType.Count);
                        */
                        #endregion
                    }

                }
                #endregion EnvironmenatlRequirement
                /////////////////////////////////////////////////
                #region Typvertreter
                Load.label_Progress.Text = "Anlegen Element Typvertreter";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                if (Database_OpArch.metamodel.m_Pattern_flag[5] == true)
                {
                    //Schelife über alle NodeType
                    if (this.m_NodeType.Count > 0)
                    {
                        Parallel.ForEach(this.m_NodeType, nodeType =>
                        {
                            try
                            {
                                nodeType.Get_Typvertreter(this, Repository);

                                ////Requirements erhalten
                                if (nodeType.m_Typvertreter.Count > 0)
                                {
                                    Repository_Connector rep_con = new Repository_Connector();

                                    int d2 = 0;
                                    do
                                    {
                                        nodeType.m_Typvertreter[d2].Check_Requirement_Typevertreter(this, Repository);

                                        ////////////////
                                        ///Logical erhalten
                                        if (nodeType.m_Typvertreter[d2].m_GUID.Count > 0)
                                        {
                                            int d3 = 0;
                                            do
                                            {
                                                Logical log = rep_con.Get_Logical(nodeType.m_Typvertreter[d2].m_GUID[d3], null, Repository, this);
                                                if (log != null)
                                                {
                                                    nodeType.m_Typvertreter[d2].m_Logical.Add(log);
                                                }
                                                d3++;
                                            } while (d3 < nodeType.m_Typvertreter[d2].m_GUID.Count);
                                        }


                                        d2++;
                                    } while (d2 < nodeType.m_Typvertreter.Count);
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Typvertreter AFO NodeType: " + nodeType.Name + ": " + err.Message);
                            }
                        });


                        #region Archiv
                       /* Load.progressBar1.Maximum = this.m_NodeType.Count - 1;
                        Load.label_Progress.Refresh();
                        int d1 = 0;
                        do
                        {
                            try
                            {
                                this.m_NodeType[d1].Get_Typvertreter(this, Repository);

                                ////Requirements erhalten
                                if (this.m_NodeType[d1].m_Typvertreter.Count > 0)
                                {
                                    Repository_Connector rep_con = new Repository_Connector();

                                    int d2 = 0;
                                    do
                                    {
                                        this.m_NodeType[d1].m_Typvertreter[d2].Check_Requirement_Typevertreter(this, Repository);

                                        ////////////////
                                        ///Logical erhalten
                                        if (this.m_NodeType[d1].m_Typvertreter[d2].m_GUID.Count > 0)
                                        {
                                            int d3 = 0;
                                            do
                                            {
                                                Logical log = rep_con.Get_Logical(this.m_NodeType[d1].m_Typvertreter[d2].m_GUID[d3], null, Repository, this);
                                                if (log != null)
                                                {
                                                    this.m_NodeType[d1].m_Typvertreter[d2].m_Logical.Add(log);
                                                }
                                                d3++;
                                            } while (d3 < this.m_NodeType[d1].m_Typvertreter[d2].m_GUID.Count);
                                        }


                                        d2++;
                                    } while (d2 < this.m_NodeType[d1].m_Typvertreter.Count);
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Typvertreter AFO NodeType: " + this.m_NodeType[d1].Name + ": " + err.Message);
                            }
                           
                            Load.progressBar1.PerformStep();
                            Load.progressBar1.Refresh();
                            d1++;
                        } while (d1 < this.m_NodeType.Count);
                       */
                        #endregion
                    }

                }
                #endregion Typvertreter
                //////////////////////////////////////////////////
                #region Measurement
                #region Class
                if(this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        //Measurement Class
                        this.m_NodeType[i1].Get_Measurements_Class(this);

                        //Measurement Instanzen
                        if (this.m_NodeType[i1].m_Instantiate.Count > 0)
                        {
                            this.m_NodeType[i1].Get_Measurements_Instanzen(this);
                        }

                        if(this.m_NodeType[i1].m_Element_Measurement.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                this.m_NodeType[i1].m_Element_Measurement[i2].Check_For_Requirement(0, Repository, this, null, this.m_NodeType[i1]);

                                i2++;
                            } while (i2 < this.m_NodeType[i1].m_Element_Measurement.Count);


                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
                #endregion

                #region Activity
                if(this.m_Activity.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        //Measurement Activity
                        this.m_Activity[i1].Get_Measurements_Activity(this);
                        //Measurement Action
                        if(this.m_Activity[i1].m_GUID.Count > 0)
                        {
                            this.m_Activity[i1].Get_Measurements_Instanzen(this);
                        }

                        //Für NodeType übernehmen
                        this.m_Activity[i1].Create_Functional_Measurement(Repository, this);
                        this.m_Activity[i1].Create_User_Measurement(Repository, this);


                        i1++;
                    } while (i1 < this.m_Activity.Count);
                }
                #endregion
                #endregion
                /////////////////////////////////////////////////////
                ///Requiremtns Verknüpfungen untereinander anlegen
                #region REquirement Relation
                Load.label_Progress.Text = "Anlegen Anforderungen Beziehungen";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                if (this.m_Requirement.Count > 0)
                {
                    Parallel.ForEach(this.m_Requirement, req =>
                    {
                        req.Get_Connector_Requirements(this, Repository);
                    });
                /*    int i1 = 0;
                    do
                    {
                        this.m_Requirement[i1].Get_Connector_Requirements(this, Repository);

                        i1++;
                    } while (i1 < this.m_Requirement.Count);
                */
                    #region Archiv
                    /*
                    int r1 = 0;
                    do
                    {
                        this.m_Requirement[r1].Get_Connector_Requirements(this, Repository);

                        r1++;
                    } while (r1 < this.m_Requirement.Count);
                    */
                    #endregion
                }
                #endregion Requirement Relation
               
                /////////////////////////////////////////////////////
                ///Nahweisart für Anforderugen erhalten
                #region Nachweidsart
                Load.label_Progress.Text = "Anlegen Anforderungen Nachweisarten";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                if (this.m_Requirement.Count > 0 && this.m_Nachweisarten.Count > 0)
                {
                    Parallel.ForEach(this.m_Requirement, req =>
                    {
                        req.Get_Nachweisarten(this, Repository, true);
                    });
                   
                }
                #endregion
                #region Abnahemkriterium
                Load.label_Progress.Text = "Anlegen Nachweisart Abnahemkriterium";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                if (this.m_Abnahmekriterium.Count > 0 && this.m_Nachweisarten.Count > 0)
                {
                    Parallel.ForEach(this.m_Nachweisarten, nachweis =>
                    {
                        nachweis.Get_Abnahmekriterien(this, Repository);
                    });

                /*    int i1 = 0;
                    do
                    {
                        this.m_Nachweisarten[i1].Update_Abnahmekriterium(Repository, this);

                        i1++;
                    } while (i1 < this.m_Nachweisarten.Count);
                */

                }
                #endregion

                #region Update Nachweisart und Abnahmekriterium der Anforderungen
                if(this.m_Requirement.Count > 0 && this.m_Nachweisarten.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        this.m_Requirement[i1].Update_Nachweisart(Repository, this);

                        i1++;
                    } while (i1 < this.m_Requirement.Count);
                }

                #endregion
                /////////////////////////////////////////////////////
                ///Verknüpfung OpArch und SysArch
                #region Verknüpfung OpArch & SysArch
            /*    if (this.m_SysElemente.Count > 0)
                {
                    int s1 = 0;
                    do
                    {
                        this.m_SysElemente[s1].Get_Implements(this);

                        s1++;
                    } while (s1 < this.m_SysElemente.Count);

                    m_Decomposition_Sys = this.m_SysElemente.Where(x => x.m_Parent.Count == 0).ToList();

                }*/

                #endregion

                #region Übergabe der Generalisierungen
                Load.label_Progress.Text = "Generalisierung NodeType";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                //NodeType
                if (this.m_NodeType.Count > 0)
                {
                    Parallel.ForEach(this.m_NodeType, nodetype =>
                    {
                        nodetype.Copy_Generalize(Repository, this);
                    });
                }
                //Activity
                Load.label_Progress.Text = "Generalisierung Activity";
                Load.progressBar1.Value = 0;
                Load.label_Progress.Refresh();
                if (this.m_Activity.Count > 0)
                {
                    Parallel.ForEach(this.m_Activity, activity =>
                    {
                        activity.Copy_Generalize(Repository, this);
                    });
                }
                #endregion

                #region Implements
                if(this.metamodel.flag_sysarch == true)
                {
                    if(this.m_SysElemente.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            this.m_SysElemente[i1].Get_Implements_Logical(this, Repository);

                            i1++;
                        } while (i1 < this.m_SysElemente.Count);
                    }


                }
                #endregion

                #region Transform Implements
                if (this.metamodel.flag_sysarch == true)
                {
                    if (this.m_SysElemente.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            //Transform Funktional AFO
                            this.m_SysElemente[i1].Transform_Functional(this, Repository);
                            //Transform User AFO
                            this.m_SysElemente[i1].Transform_User(this, Repository);
                            //Transform Schnittstellen
                            this.m_SysElemente[i1].Transform_Interface(this, Repository);
                            //Tansform Quality Class
                            this.m_SysElemente[i1].Transform_Quality_Class(this, Repository);
                            //Transform Environment
                            this.m_SysElemente[i1].Transform_Environment(this, Repository);
                            //Transform Design
                            this.m_SysElemente[i1].Transform_Design(this, Repository);

                            i1++;
                        } while (i1 < this.m_SysElemente.Count);
                    }
                }
                #endregion


                    #region Refresh Model
                    Load.label_Progress.Text = "Refresh ModelView";
                Load.label_Progress.Refresh();
                //PAttern_flag wieder auf false
                if (this.metamodel.m_Pattern_flag.Count > 0)
                {
                    int p1 = 0;
                    do
                    {
                        this.metamodel.m_Pattern_flag[p1] = false;
                        p1++;
                    } while (p1 < this.metamodel.m_Pattern_flag.Count);
                }


                #endregion
                Load.Close();
            }

            interface_Collection_OleDB.Close_Connection(this);

       //     Repository.RefreshModelView(0);
        }

        #region Decomposition

        private void Create_Dekomposition(EA.Repository Repository, string Package_GUID)
        {
            if (this.m_Logical.Count > 0)
            {
                Parallel.ForEach(this.m_Logical, logical =>
                {
                    //Kinderelemente der LogicalArchitecture erhalten
                    EA.Element LogicalArchitecture = Repository.GetElementByGuid(logical.Classifier_ID);
                    //Alle Elemente unter der LogicalArchitecture erhalten
                    EA.Collection Collection = LogicalArchitecture.Elements;
                    if (Collection.Count > 0)
                    {
                        #region Hierachie aktuelle LA
                        //Elemente unter aktueller LogicalArchitecture vorhanden
                        short d2 = 0;
                        do
                        {
                            int index_Element = -1;
                            EA.Element recentElement = Collection.GetAt(d2); //(Aktuelles Element erhalten)

                            if (this.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList().Contains(recentElement.Stereotype) == true)
                            {
                                string GUID = recentElement.ElementGUID;
                                //Prüfen ob gültiges Element nach Definition
                                if (this.metamodel.m_Elements_Usage.Select(x => x.Type).ToList().Contains(recentElement.Type) == true)
                                {
                                    index_Element = this.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList().FindIndex(x => x == recentElement.Stereotype);
                                    Repository_Class recent_repository_Element = new Repository_Class();
                                    recent_repository_Element.Classifier_ID = GUID;

                                    GUID = recent_repository_Element.Get_Classifier_Part(this);

                                    //verwendetes Element hat einen Classifier und ist gültiges Element
                                    if (GUID != null && GUID != "" && index_Element != -1)
                                    {
                                        //Prüfen, ob Classifier gültiges Element
                                        List<NodeType> m_NodeType_Classifier = this.m_NodeType.Where(x => x.Classifier_ID == GUID).ToList();

                                        if (m_NodeType_Classifier.Count > 0) //Muss einzigartig sein, wenn vorhanden und gültig
                                        {
                                            m_NodeType_Classifier[0].m_Instantiate.Add(recentElement.ElementGUID);

                                            //m_NodeType_Classifier[0].Get_TV_Instantiate(this, Repository);

                                            //Kinderelemente rekursiv betrachten
                                            Decomposition_rekursiv(m_NodeType_Classifier[0], recentElement, Package_GUID, Repository, true);
                                        }



                                    }
                                }
                            }
                            d2++;
                        } while (d2 < Collection.Count);
                    }
                    #endregion

                });
                        #region Archiv
                        /* int d1 = 0;
                         do
                         {
                             //Load aktualisieren
                             Load.progressBar1.PerformStep();
                             this.Load.label2.Text = this.m_Logical[d1].Get_Name(this);
                             Load.Refresh();
                             ////////////////////////////////
                             //Kinderelemente der LogicalArchitecture erhalten
                             EA.Element LogicalArchitecture = Repository.GetElementByGuid(this.m_Logical[d1].Classifier_ID);
                             //Alle Elemente unter der LogicalArchitecture erhalten
                             EA.Collection Collection = LogicalArchitecture.Elements;
                             if (Collection.Count > 0)
                             {
                                 #region Hierachie aktuelle LA
                                 //Elemente unter aktueller LogicalArchitecture vorhanden
                                 short d2 = 0;
                                 do
                                 {
                                     int index_Element = -1;
                                     EA.Element recentElement = Collection.GetAt(d2); //(Aktuelles Element erhalten)

                                     if (this.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList().Contains(recentElement.Stereotype) == true)
                                     {
                                         string GUID = recentElement.ElementGUID;
                                         //Prüfen ob gültiges Element nach Definition
                                         if (this.metamodel.m_Elements_Usage.Select(x => x.Type).ToList().Contains(recentElement.Type) == true)
                                         {
                                             index_Element = this.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList().FindIndex(x => x == recentElement.Stereotype);
                                             Repository_Class recent_repository_Element = new Repository_Class();
                                             recent_repository_Element.Classifier_ID = GUID;

                                             GUID = recent_repository_Element.Get_Classifier_Part(this);

                                             //verwendetes Element hat einen Classifier und ist gültiges Element
                                             if (GUID != null && GUID != "" && index_Element != -1)
                                             {
                                                 //Prüfen, ob Classifier gültiges Element
                                                 List<NodeType> m_NodeType_Classifier = this.m_NodeType.Where(x => x.Classifier_ID == GUID).ToList();

                                                 if (m_NodeType_Classifier.Count > 0) //Muss einzigartig sein, wenn vorhanden und gültig
                                                 {
                                                     m_NodeType_Classifier[0].m_Instantiate.Add(recentElement.ElementGUID);

                                                     //m_NodeType_Classifier[0].Get_TV_Instantiate(this, Repository);

                                                     //Kinderelemente rekursiv betrachten
                                                     Decomposition_rekursiv(m_NodeType_Classifier[0], recentElement, Package_GUID, Repository, true);
                                                 }



                                             }
                                         }
                                     }

                                     d2++;
                                 } while (d2 < Collection.Count);
                                 #endregion Hierachie aktuelel LA
                             }

                             d1++;
                         } while (d1 < this.m_Logical.Count);
                        */

                        #endregion
                    

            }
        }

        #endregion


        #region Ckeck_Elememnts
        /// <summary>
        /// Die DAtanbnk wird nach den InformationElement durchsucht
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public InformationElement Check_InformationElement(string GUID)
        {
            List<string> GUIDS = new List<string>();

            if (this.m_InformationElement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.m_InformationElement[i1] != null)
                    {
                        if (this.m_InformationElement[i1].Classifier_ID == GUID)
                        {
                            return (this.m_InformationElement[i1]);
                        }
                    }

                   

                    i1++;
                } while (i1 < this.m_InformationElement.Count);
            }


            return (null);
        }
        /// <summary>
        /// Es wird die Datenbank Ã¼berprÃ¼ft, ob der Logical enthalten ist.
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public Logical Check_Logical(string GUID)
        {
            List<string> GUIDS = new List<string>();

            if (this.m_Logical.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Logical[i1].Classifier_ID == GUID)
                    {
                        return (this.m_Logical[i1]);
                    }

                    i1++;
                } while (i1 < this.m_Logical.Count);
            }


            return (null);
        }
        /// <summary>
        /// Es wird Ã¼berprÃ¼ft, ob der NodeType in der Database vorhanden ist
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public NodeType Check_NodeType(string GUID)
        {
            List<string> GUIDS = new List<string>();

            if (this.m_NodeType.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_NodeType[i1].Classifier_ID == GUID)
                    {
                        return (this.m_NodeType[i1]);
                    }

                    i1++;
                } while (i1 < this.m_NodeType.Count);
            }


            return (null);
        }
        //Check Requirement
        public Requirement Check_Requirement(string GUID)
        {
            int index =  this.m_Requirement.FindIndex(x => x.Classifier_ID == GUID);
            
            if(index != -1)
            {
                return (this.m_Requirement[index]);
            }
            else
            {
                return (null);
            }

        }
        //Check Design_Constraint
        public OperationalConstraint Check_Design_Constraint(string GUID)
        {
            int index = this.m_DesignConstraint.FindIndex(x => x.Classifier_ID == GUID);

            if (index != -1)
            {
                return (this.m_DesignConstraint[index]);
            }
            else
            {
                return (null);
            }

        }
        //Check Umwelt_Constraint
        public OperationalConstraint Check_Umwelt_Constraint(string GUID)
        {
            int index = this.m_UmweltConstraint.FindIndex(x => x.Classifier_ID == GUID);

            if (index != -1)
            {
                return (this.m_UmweltConstraint[index]);
            }
            else
            {
                return (null);
            }

        }
        //Check Typvertreter
        public NodeType Check_Typvertreter(string GUID)
        {
            int index = this.m_Typvertreter.FindIndex(x => x.Classifier_ID == GUID);

            if (index != -1)
            {
                return (this.m_Typvertreter[index]);
            }
            else
            {
                return (null);
            }

        }
        /// <summary>
        /// Es wird die aktuelle Decomposition (hier oberste Stufe) auf den NodeType auf der Ebene geprÃ¼ft
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public NodeType Check_Decomposition_LA_For_NodeType(string GUID)
        {
            List<NodeType> obj_check = this.Decomposition.m_NodeType.Where(x => x.Classifier_ID == GUID).Select(x => x).ToList();

            if (obj_check.Count > 0)
            {
                return (obj_check[0]);
            }
            else
            {
                return (null);
            }

       
        }

        public Capability Check_Capability_Database(string GUID)
        {
            if (this.m_Capability.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Capability[i1].Classifier_ID == GUID)
                    {
                        return (this.m_Capability[i1]);
                    }

                    i1++;
                } while (i1 < this.m_Capability.Count);
            }

            return (null);
        }

        /// <summary>
        /// Es wird die Datenbank überprüft, ob eine Activity mit der GUID vorhanden sind
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public List<Activity> Check_Activity(string GUID)
        {

            List<Activity> m_ret = new List<Activity>();

            if (this.m_Activity.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Activity[i1].Classifier_ID == GUID)
                    {
                        m_ret.Add(this.m_Activity[i1]);
                    }
                    i1++;
                } while (i1 < this.m_Activity.Count);
            }

            if (m_ret.Count > 0)
            {
                return (m_ret);
            }
            else
            {
                return (null);
            }
        }
        /// <summary>
        /// Es wird die Datenbank überprüft, ob ein Stakeholder mit der GUID vorhanden sind
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
		public List<Stakeholder> Check_Stakeholder(string GUID)
        {

            List<Stakeholder> m_ret = new List<Stakeholder>();

            if (this.m_Stakeholder.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Stakeholder[i1].Classifier_ID == GUID)
                    {
                        m_ret.Add(this.m_Stakeholder[i1]);
                    }
                    i1++;
                } while (i1 < this.m_Stakeholder.Count);
            }

            if (m_ret.Count > 0)
            {
                return (m_ret);
            }
            else
            {
                return (null);
            }
        }

        public Pool Check_ReceiveEvent(string GUID)
        {
            if(this.m_Pools.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.m_Pools[i1].m_GUID_receiveEvent.Contains(GUID) == true)
                    {
                        return (this.m_Pools[i1]);
                    }

                    i1++;
                } while (i1 < this.m_Pools.Count);
            }

            return (null);
        }
        #endregion Check_Elements

        #region Forms
        /////////////////////////
        // Listbox Funktionen
        /// <summary>
        /// Listbox werden alle Elemente Unchecked
        /// </summary>
        /// <param name="Box"></param>
        public void Uncheck_CheckedListbox(System.Windows.Forms.CheckedListBox Box)
        {
            ///Alle Indices Szenar_Box unchecken
            ///
            if (Box.Items.Count > 0)
            {
                int i2 = 0;
                do
                {
                    Box.SetItemChecked(i2, false);

                    i2++;
                } while (i2 < Box.Items.Count);
            }
        }
        /// <summary>
        /// Es werdn in der Listbox alle Elemente Gechecked
        /// </summary>
        /// <param name="Box"></param>
        public void Check_All_CheckedListbox(System.Windows.Forms.CheckedListBox Box)
        {
            ///Alle Indices Szenar_Box unchecken
            ///
            if (Box.Items.Count > 0)
            {
                int i2 = 0;
                do
                {
                    Box.SetItemChecked(i2, true);

                    i2++;
                } while (i2 < Box.Items.Count);
            }
        }
        /// <summary>
        /// Es wird die Liste Ã¼berprÃ¼ft und in der Listbox dann gechecked
        /// </summary>
        /// <param name="Box"></param>
        /// <param name="List"></param>
        /// <param name="Repository"></param>
        public void Check_CheckedListbox(System.Windows.Forms.CheckedListBox Box, List<string> List, EA.Repository Repository)
        {
            Repository_Element repository_Element = new Repository_Element();
            //Setzen, wenn aktuelle GUID in Szenar enthalten
            int i2 = 0;
            do
            {
                repository_Element.Classifier_ID = List[i2];

                if (Box.Items.Contains(repository_Element.Get_Name(this)) == true)
                {
                    int recent_Index = Box.Items.IndexOf(repository_Element.Get_Name(this));

                    Box.SetItemChecked(recent_Index, true);
                }

                i2++;
            } while (i2 < List.Count);
        }
        /////////////////////////
        //Textbox Funktionen
        /// <summary>
        /// Es wird eine Textbox fÃ¼r die AFO beschrieben
        /// </summary>
        /// <param name="TextBox"></param>
        /// <param name="List"></param>
        public void Write_List(System.Windows.Forms.TextBox TextBox, List<string> List)
        {
            string Text = "";

            if (List.Count > 0)
            {
                int i = 0;
                do
                {
                    Text = Text + List[i];

                    i++;
                } while (i < List.Count);
            }

            string gg = Text.Substring(1);
            string g = Text.ElementAt(0).ToString().ToUpper();

            Text = g + gg;

            TextBox.Text = Text;

        }
        #endregion Forms

        #region Add_Elements to Database
        public void Add_NodeType_Logicals()
        {
            List<NodeType> NodeTypes = this.m_NodeType;

            int i1 = 0;

            if (this.m_NodeType.Count > 0)
            {
                do
                {
                    Logical help = new Logical(this.m_NodeType[i1].Classifier_ID, null);

                    this.m_Logical.Add(help);

                    i1++;
                } while (this.m_NodeType.Count > 0);
            }


        }
        ///public Liste erweitern
        ///
        public List<Logical> Add_Distinct_Logical(List<Logical> List1, List<Logical> List2)
        {
            if (List1 != null)
            {
                if (List2 != null)
                {
                    int i1 = 0;
                    do
                    {
                        if (List1.Contains(List2[i1]) == false)
                        {
                            List1.Add(List2[i1]);
                        }

                        i1++;
                    } while (i1 < List2.Count);
                }
            }

            return (List1);
        }

        public List<InformationElement> Add_Distinct_InformationElement(List<InformationElement> List1, List<InformationElement> List2)
        {
            if (List1 != null)
            {
                if (List2 != null)
                {
                    int i1 = 0;
                    do
                    {
                        if (List1.Contains(List2[i1]) == false)
                        {
                            List1.Add(List2[i1]);
                        }

                        i1++;
                    } while (i1 < List2.Count);
                }
            }

            return (List1);
        }

        /// <summary>
        /// HizufÃ¼gen der NodeType Definitionen
        /// </summary>
        /// <param name="NodeType"></param>
        /// <param name="Repository"></param>
        /// <param name="Package_GUID"></param>
        public void Add_to_all_Classifier(NodeType NodeType, EA.Repository Repository, string Package_GUID)
        {

            List<NodeType> Add_List = new List<NodeType>();
            //NodeType das zu Ã¼bernehemnde Object --> in diesemFall ein NodeType--> daher sollen die Kinder Ã¼bernommen werden
            if (this.m_NodeType.Count > 0)
            {
                int d1 = 0;
                do
                {
                    if (this.m_NodeType[d1].Classifier_ID == NodeType.Classifier_ID)
                    {
                        //Erst Decomposition
                        if (NodeType.m_Child.Count > 0)
                        {
                            // MessageBox.Show(NodeType.Classifier_ID);
                            // this.m_NodeType[d1].m_Child.AddRange(NodeType.m_Child);


                            int d2 = 0;
                            do
                            {
                                if (this.m_NodeType[d1].Instantiate_GUID != null)
                                {
                                    this.Load.label2.Text = this.m_NodeType[d1].Get_Name(this);
                                    Load.Refresh();

                                    Add_m_Child_rekursiv(this.m_NodeType[d1], NodeType.m_Child[d2], Repository, Package_GUID, Add_List);
                                }


                                d2++;
                            } while (d2 < NodeType.m_Child.Count);
                        }
                    }
                    d1++;
                } while (d1 < this.m_NodeType.Count);

                this.Add_NodeType.AddRange(Add_List);

            }


        }
        /// <summary>
        /// HinzufÃ¼gen Child rekursiv
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Child"></param>
        /// <param name="Repository"></param>
        /// <param name="Package_GUID"></param>
        /// <param name="Add_List"></param>
        public void Add_m_Child_rekursiv(NodeType Parent, NodeType Child, EA.Repository Repository, string Package_GUID, List<NodeType> Add_List)
        {
            //Parent das Object wohinChild Ã¼bernommen werden soll
            //   MessageBox.Show(Parent.Classifier_ID+" "+Child.Classifier_ID);

            //Child.Parent = Parent;
            EA.Element Parent1 = Repository.GetElementByGuid(Parent.Instantiate_GUID); // Instanz aus Addinn Ordner
            EA.Element Child1 = Repository.GetElementByGuid(Child.Classifier_ID); // Classifier des Child aus Projekt
            List<DB_Insert> m_Insert = new List<DB_Insert>();
            NodeType help = Parent.Check_Decomposition_For_NodeType(Child.Classifier_ID); // PrÃ¼fen ob Parent einen solchen NodeType besitzt
                                                                                          //  MessageBox.Show("Twa");

            //MessageBox.Show(help.Instantiate_GUID+" "+Parent.Instantiate_GUID);


            if (help == null && Child1 != null && Parent1 != null) // Element nicht als NodeType vorhanden
            {
                NodeType Child2 = new NodeType(Child.Classifier_ID, Repository, this); //neuen NodeType erzeugen

                Child2.m_Parent.Add(Parent); // PArent als Parent von Child2
                                        //  Child2.m_Element_Interface = Child.m_Element_Interface; // Element Interface Ã¼bernehemnn
                                        //Problem ist hier, dass die Element Interface auch alle Supplier neu erstellt werden
                Parent.m_Child.Add(Child2);

                Add_List.Add(Child2);

                int index_elem = this.metamodel.m_Elements_Definition.FindIndex(x => x.Stereotype == Child1.Stereotype);

                if(index_elem < 0)
                {
                    index_elem = 0;
                }


                Child2.Instantiate_GUID = Child2.Create_Element_Instantiate(Child1.Name, this.metamodel.m_Elements_Usage.Select(x => x.Type).ToList()[index_elem], this.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList()[index_elem], this.metamodel.m_Elements_Usage.Select(x => x.Toolbox).ToList()[index_elem], Child.Classifier_ID, Parent1.ElementID, Package_GUID, Repository, false, this);
                string UUID = Child2.Instantiate_GUID;
                UUID = UUID.Trim('{', '}');
            //    TaggedValue Tagged = new TaggedValue(this.metamodel, this);
           //     Tagged.Update_Tagged_Value(Child2.Instantiate_GUID, "UUID", UUID, null, Repository);
                var Element = Repository.GetElementByGuid(Child2.Instantiate_GUID);
           //     Tagged.Update_Tagged_Value(Child2.Instantiate_GUID, "OBJECT_ID", Element.ElementID.ToString(), null, Repository);

                m_Insert.Clear();
                m_Insert.Add(new DB_Insert("UUID", OleDbType.VarChar, OdbcType.VarChar, UUID, -1));
                m_Insert.Add(new DB_Insert("OBJECT_ID", OleDbType.VarChar, OdbcType.VarChar, Element.ElementID.ToString(), -1));

                Child2.Update_TV(m_Insert, this, Repository);


                help = Child2;
            }

            if (Child.m_Child.Count > 0)
            {
                int d1 = 0;
                do
                {
                    this.Add_m_Child_rekursiv(help, Child.m_Child[d1], Repository, Package_GUID, Add_List);

                    d1++;
                } while (d1 < Child.m_Child.Count);

            }

        }

        public void Add_m_Child_Connector_rekursiv(NodeType Parent, NodeType Child, EA.Repository Repository)
        {
            /*
            //MessageBox.Show("Activate Con rekursiv");
            //alle Elemente zu Child mÃ¼ssen exisiteren, da diese vorher angelegt wurden
            if(Child.m_Element_Interface.Count > 0) 
            {
                int i1 = 0;
                do
                {
                 //   MessageBox.Show(i1.ToString());
                    //Supplier unter Parent finden
                    NodeType Supplier = Search_Parent_Supplier(Child.m_Element_Interface[i1].Supplier, Parent);
                   // MessageBox.Show(Supplier.Classifier_ID);
                    //Client unter PArent finden --> direkt unter Parent zu finden
                    NodeType Client = Parent.Check_Decomposition_For_NodeType(Child.Classifier_ID);

                    Element_Interface Check = Client.Check_Element_Interface(Supplier);

                    if(Check == null) //anlegen
                    {
                        Element_Interface elem_Inter = new Element_Interface(Client.Classifier_ID, Supplier.Classifier_ID);
                        elem_Inter.Client = Client;
                        elem_Inter.Supplier = Supplier;

                        elem_Inter.m_Target.AddRange(Child.m_Element_Interface[i1].m_Target);

                        Client.m_Element_Interface.Add(elem_Inter);
                    }
                    else //Target hinzufÃ¼gen, da einzigartig
                    {
                        Check.m_Target.AddRange(Child.m_Element_Interface[i1].m_Target);
                    }

                    if (Child.m_Child.Count > 0)
                    {
                        int d1 = 0;
                        do
                        {
                            this.Add_m_Child_Connector_rekursiv(Client, Child.m_Child[d1], Repository);

                            d1++;
                        } while (d1 < Child.m_Child.Count);

                    }

                    i1++;
                } while (i1 < Child.m_Element_Interface.Count);
            }

          */

        }

        /// <summary>
        /// Connecoren der NodeType Definitionen hinzufÃ¼gen
        /// </summary>
        /// <param name="new_Parent"></param>
        /// <param name="recent_Parent"></param>
        /// <param name="Repository"></param>
        /// <param name="new_Overall"></param>
        /// <param name="recent_Overall"></param>
        public void Add_m_Child_Connector_rekursiv_2(NodeType new_Parent, NodeType recent_Parent, EA.Repository Repository, NodeType new_Overall, NodeType recent_Overall)
        {
            if (recent_Parent.m_Child.Count > 0)
            {
                //  MessageBox.Show(this.Get_Name_t_object_GUID(recent_Parent.Classifier_ID, Repository)+" Anzahl Child: " + recent_Parent.m_Child.Count.ToString());

                int i1 = 0;
                do
                {
                    NodeType Check = new_Parent.Check_Decomposition_For_NodeType(recent_Parent.m_Child[i1].Classifier_ID);

                    if (Check != null && recent_Parent.m_Child[i1].m_Element_Interface.Count > 0)
                    {
                        //    MessageBox.Show("Check: " + this.Get_Name_t_object_GUID(Check.Classifier_ID, Repository));
                        //  MessageBox.Show(this.Get_Name_t_object_GUID(recent_Parent.m_Child[i1].Classifier_ID, Repository) + " Anzahl m_Element_Interface: " + recent_Parent.m_Child[i1].m_Element_Interface.Count.ToString());


                        Copy_Connector_NodeType(Check, recent_Parent.m_Child[i1], new_Overall, recent_Overall, Repository);
                    }

                    if (recent_Parent.m_Child[i1].m_Child.Count > 0)
                    {
                        Add_m_Child_Connector_rekursiv_2(Check, recent_Parent.m_Child[i1], Repository, new_Overall, recent_Overall);
                    }

                    i1++;
                } while (i1 < recent_Parent.m_Child.Count);


            }


        }
        /// <summary>
        /// Kopieren der Konnektoren des aktuellen NodeType
        /// </summary>
        /// <param name="new_Element"></param>
        /// <param name="recent_Element"></param>
        /// <param name="new_Overall"></param>
        /// <param name="recent_Overall"></param>
        /// <param name="Repository"></param>
        public void Copy_Connector_NodeType(NodeType new_Element, NodeType recent_Element, NodeType new_Overall, NodeType recent_Overall, EA.Repository Repository)
        {
            if (recent_Element.m_Element_Interface.Count > 0)
            {
                int i1 = 0;
                do
                {   //Supplier finden des neuen Element
                    List<string> GUIDS = new List<string>();

                    //    MessageBox.Show("StereoType: " + recent_Element.m_Element_Interface[i1].Supplier.StereoType+"\r\nSupplier: "+this.Get_Name_t_object_GUID(recent_Element.m_Element_Interface[i1].Supplier.Classifier_ID, Repository));

                    GUIDS.Add(recent_Element.m_Element_Interface[i1].Supplier.Classifier_ID);
                    string recent_StereoType = recent_Element.m_Element_Interface[i1].Supplier.StereoType;

                    NodeType lauf = recent_Element.m_Element_Interface[i1].Supplier;

                    //  MessageBox.Show("Test");

                    int i2 = 1;

                    do
                    {
                        //     MessageBox.Show(lauf.Parent.Classifier_ID);
                        lauf = lauf.m_Parent[0];
                        //  recent_StereoType = lauf.StereoType;
                        GUIDS.Add(lauf.Classifier_ID);


                    } while (lauf != recent_Overall);

                    GUIDS.Reverse();

                    //    MessageBox.Show("Anzahl GUIDS: " + GUIDS.Count.ToString());

                    //nun new_Elment zurÃ¼cklaufen
                    i2 = 1;

                    lauf = new_Overall;
                    NodeType new_Supplier;

                    do
                    {
                        lauf = lauf.Check_Decomposition_For_NodeType(GUIDS[i2]);

                        if (lauf == null) //in diesem Umfeld nicht vorhanden
                        {
                            i2 = GUIDS.Count;
                            lauf = null;
                        }

                        i2++;
                    } while (i2 < GUIDS.Count);

                    new_Supplier = lauf;

                    if (new_Supplier != null)
                    {
                        //    MessageBox.Show(this.Get_Name_t_object_GUID(new_Supplier.Classifier_ID, Repository));

                        Element_Interface Check_2 = new_Element.Check_Element_Interface(new_Supplier);

                        if (Check_2 == null) //Element Interface nicht vorhanden
                        {
                            //neues Element_Interface hinzufÃ¼gen
                            Element_Interface new_help = new Element_Interface(new_Element.Classifier_ID, new_Supplier.Classifier_ID);
                            new_help.Client = new_Element;
                            new_help.Supplier = new_Supplier;
                            new_help.m_Requirement_Interface_Send = recent_Element.m_Element_Interface[i1].m_Requirement_Interface_Send;
                            new_help.m_Requirement_Interface_Receive = recent_Element.m_Element_Interface[i1].m_Requirement_Interface_Receive;


                            new_Element.m_Element_Interface.Add(new_help);

                            //Target kopieren
                            new_help.m_Target.AddRange(recent_Element.m_Element_Interface[i1].m_Target);
                        }
                        else
                        {
                            //nur Targets kopeieren, diese sind einziegarteig und mÃ¼ssen nicht Ã¼beroprÃ¼ft werden
                            Check_2.m_Target.AddRange(recent_Element.m_Element_Interface[i1].m_Target);
                            //  Check_2.m_Requirement_Interface_Send = recent_Element.m_Element_Interface[i1].m_Requirement_Interface_Send;
                            // Check_2.m_Requirement_Interface_Receive = recent_Element.m_Element_Interface[i1].m_Requirement_Interface_Receive;
                            //Hier Requirements mergen
                            //zu dem des Element_Interface hinzufÃ¼gen (Connector und anderen Connector lÃ¶schen)
                            if (recent_Element.m_Element_Interface[i1].m_Requirement_Interface_Send[0] != Check_2.m_Requirement_Interface_Send[0])
                            {

                            }
                        }

                    }


                    i1++;
                } while (i1 < recent_Element.m_Element_Interface.Count);
            }
        }
        #endregion Add Elements to Database

        #region Decomposition

        /// <summary>
        /// Anlegen der Decomposition. Dies geschieht rekursiv Ã¼ber die Kinderelemente des aktuelenen Elternknotens
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="recentParent"></param>
        /// <param name="Package_GUID"></param>
        /// <param name="Repository"></param>
        /// <param name="Logical_Decomposition"></param>
        public void Decomposition_rekursiv(NodeType Parent, EA.Element recentParent, string Package_GUID, EA.Repository Repository, bool Logical_Decomposition)
        {
            Interface_Collection interface_Collection_OleDB = new Interface_Collection();
            Repository_Class repository_Element = new Repository_Class();
            EA.Collection Collection = recentParent.Elements;
            EA.Element Parent_Class = Repository.GetElementByGuid(Parent.Instantiate_GUID);
            TaggedValue Tagged = new TaggedValue(this.metamodel, this);
            List<DB_Insert> m_Insert = new List<DB_Insert>();

            if (Collection.Count > 0)
            {
                //Elemente unter recentElement vorhanden
                short d2 = 0;
                do
                {
                    EA.Element recentElement = Collection.GetAt(d2);

                    //Gültige Kinderelemente heraussuchen
                    if (this.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList().Contains(recentElement.Stereotype) == true)
                    {
                        string GUID = recentElement.ElementGUID;
                        string GUID_copy = GUID;

                        if (this.metamodel.m_Elements_Usage.Select(x => x.Type).ToList().Contains(recentElement.Type) == true)
                        {
                            //Classifier des aktuellen Child
                            /*    XML help = new XML();
                                string SQL = "SELECT PDATA1 FROM t_object WHERE ea_guid = '" + GUID + "';";
                                string xml_String = Repository.SQLQuery(SQL);
                                List<string> help_Class = help.Xml_Read_Attribut("PDATA1", xml_String);
                            */
                            repository_Element.Classifier_ID = GUID;
                            GUID = repository_Element.Get_Classifier_Part(this);

                            //Stereotype bestätigen 
                            int index_elem = this.metamodel.m_Elements_Usage.FindIndex(x => x.Stereotype == recentElement.Stereotype);

                            //verwendetes Element hat einen Classifier und ist gültiges Element
                            if (GUID != null && GUID != "" && index_elem != -1)
                            {
                                //Prüfen, ob Classifier gültiges Element
                                List<NodeType> m_NodeType_Classifier = this.m_NodeType.Where(x => x.Classifier_ID == GUID).ToList();

                                if (m_NodeType_Classifier.Count > 0) //Muss einzigartig sein, wenn vorhanden und gültig
                                {
                                    m_NodeType_Classifier[0].m_Instantiate.Add(recentElement.ElementGUID);

                                    //Kinderelemente rekursiv betrachten
                                    Decomposition_rekursiv(m_NodeType_Classifier[0], recentElement, Package_GUID, Repository, true);
                                }
                            }
                        }
                    }
                    d2++;
                } while (d2 < Collection.Count);
            }
        }

        #endregion Decomposition

        #region Get_Elements

        public List<string> Check_Connectors_Element(string Element_GUID, Database Data)
        {
            Repository_Connectors repository_Connectors = new Repository_Connectors();
            Repository_Element repository_Element = new Repository_Element();
            repository_Element.Classifier_ID = Element_GUID;

        /*    List<string> Connector_GUID = new List<string>();
            EA.Element recentElement = Repository.GetElementByGuid(Element_GUID);
            XML xml = new XML();

            string SQL = @"SELECT ea_guid FROM t_connector WHERE Start_Object_ID = " + recentElement.ElementID + ";";
            var help = Repository.SQLQuery(SQL);

            Connector_GUID = xml.Xml_Read_Attribut("Connector_ID", help);
            */
            List<string> Connector_GUID2 = repository_Connectors.Get_Connector_By_StartID(Data, repository_Element.Get_Object_ID(Data));

            return Connector_GUID2;
        }

        public NodeType Check_Supplier_NodeType(EA.Repository Repository, string Connector_ID)
        {
            // XML xml = new XML();
            Interface_Connectors interface_Connectors = new Interface_Connectors();
            List<string> m_Type = this.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            m_Type.AddRange(this.metamodel.m_Elements_Usage.Select(x => x.Type).ToList());
            List<string> m_Stereotype = this.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            m_Stereotype.AddRange(this.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList());

            string Supplier_ID =  interface_Connectors.Get_Client_GUID(this, Connector_ID, m_Type, m_Stereotype);
          /*  string SQL = @"SELECT ea_guid FROM t_object WHERE Object_ID IN
            (SELECT End_Object_ID FROM t_connector WHERE ea_guid = '" + Connector_ID + "') AND Object_Type IN"+xml.SQL_IN_Array(m_Type.ToArray())+";";
            var help = Repository.SQLQuery(SQL);
            List<string> Supplier_ID = xml.Xml_Read_Attribut("ea_guid", help);*/


            if (Supplier_ID != null)
            {
                List<NodeType> obj_supplier = this.m_NodeType.Where(x => x.m_Instantiate.Contains(Supplier_ID) == true).ToList();

                if (obj_supplier.Count > 0)
                {
                    return (obj_supplier[0]);
                }
                else
                {
                    return (null);
                }
            

            }

            return (null);
        }

        public NodeType Check_Supplier_NodeType_2(EA.Repository Repository, string Connector_ID, NodeType Node)
        {
            /*   XML xml = new XML();
               List<string> m_Type = this.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
               m_Type.AddRange(this.metamodel.m_Elements_Usage.Select(x => x.Type).ToList());

               string SQL = @"SELECT ea_guid FROM t_object WHERE Object_ID IN
               (SELECT End_Object_ID FROM t_connector WHERE ea_guid = '" + Connector_ID + "') AND Object_Type IN"+xml.SQL_IN_Array(m_Type.ToArray())+"; ";
               var help = Repository.SQLQuery(SQL);
               List<string> Supplier_ID = xml.Xml_Read_Attribut("ea_guid", help);*/
            Interface_Connectors interface_Connectors = new Interface_Connectors();
            List<string> Supplier_ID = interface_Connectors.Get_Supplier_By_Connecror_And_Supplier(this, Connector_ID);

            if (Supplier_ID != null)
            {
                List<NodeType> obj_supplier = this.m_NodeType.Where(x => x.m_Instantiate.Contains(Supplier_ID[0]) == true).ToList();

                if(obj_supplier.Count > 0)
                {
                    return (obj_supplier[0]);
                }
                else
                {
                    return (null);
                }
              

            }

            return (null);
        }


        public NodeType Search_Parent_Supplier(NodeType Supplier, NodeType Parent)
        {
            List<string> GUIDS = new List<string>();

            //            MessageBox.Show("Activate Search_Parent_Supplier");


            string help = Supplier.Classifier_ID;

            do
            {
                //    MessageBox.Show(GUIDS.Count.ToString());

                GUIDS.Add(help);

                help = Supplier.m_Parent[0].Classifier_ID;

            } while (help != null);


            if (GUIDS.Count > 0)
            {
                GUIDS.Reverse();
                NodeType Parent2 = Parent;
                NodeType Parent3 = Parent;
                //Index 0 muss Element unter Parent sein
                int i1 = 0;
                do
                {
                    Parent2 = Parent2.Check_Decomposition_For_NodeType(GUIDS[i1]);

                    if (i1 == 0 && Parent2 == null)
                    {
                        i1--;
                        if (Parent3.m_Parent != null)
                        {
                            Parent2 = Parent3.m_Parent[0];
                            Parent = Parent3.m_Parent[0];
                        }
                        else
                        {
                            return null;
                        }
                    }

                    i1++;
                } while (i1 < GUIDS.Count);

                NodeType Supplier_Parent = Parent2;



                return (Supplier_Parent);
            }



            return null;
        }

        /// <summary>
        /// Connectoren erhalten
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Element_GUID"></param>
        /// <param name="Logical"></param>
        /// <param name="node"></param>
        /// <param name="flag_NT"></param>
        /// <param name="LA_Logical"></param>
        public void Create_Connectors(EA.Repository Repository, string Element_GUID, bool Logical, NodeType node, bool flag_NT, Logical LA_Logical)
        {
            EA.Element El = Repository.GetElementByGuid(Element_GUID);
            string Name = El.Name;
            //MessageBox.Show(El.Name);

            List<string> Connector_ID = Check_Connectors_Element(Element_GUID,this);
            //aktuelles Element besitzt ausgehende Connectoren
            if (Connector_ID != null)
            {
                //MessageBox.Show("Anzahl Con: "+Connector_ID.Count.ToString());

            //    XML xml = new XML();
                List<string> GUID = new List<string>();
                //Classifier Element
                GUID = this.Get_Element_Classifier(Element_GUID, Repository, this);

                if (GUID != null)
                {
                    NodeType Client;

                    //   MessageBox.Show("VorLogical");

                    if (Logical == true)
                    {
                        Client = this.Check_Decomposition_LA_For_NodeType(GUID[0]);
                    }
                    else
                    {
                        Client = node;
                    }


                    if (Client.Classifier_ID == GUID[0])
                    {
                        //   MessageBox.Show("Client Name: "+Repository.GetElementByGuid(Client.Classifier_ID).Name+" "+Client.Classifier_ID);
                        //vorhanden --> Connectoren Ã¼bernehmen, wenn Supplier da
                        int d3 = 0;
                        do
                        {
                            NodeType Supplier;

                            if (flag_NT == true)
                            {
                                // MessageBox.Show("Client: " + this.Get_Name_t_object_GUID(Client.Classifier_ID, Repository));

                                Supplier = this.Check_Supplier_NodeType_2(Repository, Connector_ID[d3], this.NT_help1);

                                // MessageBox.Show("Client: "+this.Get_Name_t_object_GUID(Client.Classifier_ID, Repository)+" Supplier: " + this.Get_Name_t_object_GUID(Supplier.Classifier_ID, Repository));
                            }
                            else
                            {
                                Supplier = this.Check_Supplier_NodeType(Repository, Connector_ID[d3]);
                            }


                            EA.Connector Connector = Repository.GetConnectorByGuid(Connector_ID[d3]);

                            if (Supplier != null)
                            {
                                //  MessageBox.Show("Connector: " + this.Get_Name_t_object_GUID(Client.Classifier_ID, Repository) + " --> " + this.Get_Name_t_object_GUID(Supplier.Classifier_ID, Repository));
                                // Connector und Supplier vorhathis.Get_Name_t_object_GUID(Supplier.Classifier_ID, Repository)nden
                                //Check Element Interface vorhanden
                                Element_Interface elem_interface = Client.Check_Element_Interface(Supplier);

                                //  MessageBox.Show(elem_interface.)

                                if (elem_interface == null) //nicht vorhanden
                                {
                                    //  MessageBox.Show("Neues Element Interface anlegen");
                                    Element_Interface elem_interface2 = new Element_Interface(Client.Classifier_ID, Supplier.Classifier_ID);
                                    elem_interface2.Client = Client;
                                    elem_interface2.Supplier = Supplier;

                                    Client.m_Element_Interface.Add(elem_interface2);

                                    elem_interface = elem_interface2;

                                }

                                Target target = elem_interface.Check_Target(Repository.GetElementByID(Connector.ClientID).ElementGUID, Repository.GetElementByID(Connector.SupplierID).ElementGUID);

                                if (target == null) // Target nicht vorhanden
                                {
                                    //  MessageBox.Show("Neues Target anlegen");
                                    Target target2 = new Target(Repository.GetElementByID(Connector.ClientID).ElementGUID, Repository.GetElementByID(Connector.SupplierID).ElementGUID, this);
                                    List<InformationElement> InfoElm1 = target2.Get_Information_Element_Logical(Repository, Connector.ConnectorGUID, LA_Logical, this);

                                    //  MessageBox.Show("Neu --> Client_ID: " + target2.CLient_ID + "\r\nSupplier_ID: " + target2.Supplier_ID);

                                    if (InfoElm1 != null)
                                    {
                                        //  MessageBox.Show("Info Elem zu Target hinzufÃ¼gen");
                                        target2.m_Information_Element.AddRange(InfoElm1);
                                    }

                                    // MessageBox.Show("Target zu Element_Interface hinzufÃ¼gen");
                                    List<List<Requirement_Interface>> m_Requ_List = target2.Check_Requirement_Interface(Repository, this, flag_NT, this.metamodel.m_Prozesswort_Interface[0]);

                                    if (m_Requ_List != null)
                                    {
                                        int j1 = 0;
                                        do
                                        {
                                            if (elem_interface.m_Requirement_Interface_Send.Contains(m_Requ_List[0][j1]) == false)
                                            {
                                                elem_interface.m_Requirement_Interface_Send.Add(m_Requ_List[0][j1]);
                                            }

                                            j1++;
                                        } while (j1 < m_Requ_List[0].Count);
                                        int j2 = 0;
                                        do
                                        {
                                            if (elem_interface.m_Requirement_Interface_Receive.Contains(m_Requ_List[1][j2]) == false)
                                            {
                                                elem_interface.m_Requirement_Interface_Receive.Add(m_Requ_List[1][j2]);
                                            }

                                            j2++;
                                        } while (j2 < m_Requ_List[1].Count);
                                    }
                                    /*  if (Requ_List != null)
                                      //  if(Requ_List != null && flag_NT == false)
                                      {

                                          //Hier die Listen richtig machen, da Sie mehr als 2 EintrÃ¤ge haben kÃ¶nnen
                                          if (elem_interface.m_Requirement_Interface_Send.Contains(Requ_List[0]) == false)
                                          {
                                              elem_interface.m_Requirement_Interface_Send.Add(Requ_List[0]);

                                          }
                                          if (elem_interface.m_Requirement_Interface_Receive.Contains(Requ_List[1]) == false)
                                          {
                                              elem_interface.m_Requirement_Interface_Receive.Add(Requ_List[1]);
                                          }

                                      }
                                    */
                                    // MessageBox.Show(elem_interface.m_Requirement_Interface_Send.Count.ToString());

                                    elem_interface.m_Target.Add(target2);
                                }
                                else //Target vorhanden
                                {
                                    //   MessageBox.Show("Client_ID: " + target.CLient_ID + "\r\nSupplier_ID: " + target.Supplier_ID);

                                    List<List<Requirement_Interface>> m_Requ_List2 = target.Check_Requirement_Interface(Repository, this, flag_NT, this.metamodel.m_Prozesswort_Interface[0]);

                                    if (m_Requ_List2 != null)
                                    {
                                        int j1 = 0;
                                        do
                                        {
                                            if (elem_interface.m_Requirement_Interface_Send.Contains(m_Requ_List2[0][j1]) == false)
                                            {
                                                elem_interface.m_Requirement_Interface_Send.Add(m_Requ_List2[0][j1]);
                                            }

                                            j1++;
                                        } while (j1 < m_Requ_List2[0].Count);
                                        int j2 = 0;
                                        do
                                        {
                                            if (elem_interface.m_Requirement_Interface_Receive.Contains(m_Requ_List2[1][j2]) == false)
                                            {
                                                elem_interface.m_Requirement_Interface_Receive.Add(m_Requ_List2[1][j2]);
                                            }

                                            j2++;
                                        } while (j2 < m_Requ_List2[1].Count);
                                    }
                                    /*  if (Requ_List2 != null)
                                      //   if (Requ_List2 != null && flag_NT == false )
                                      {//Hier die Listen richtig machen
                                          if (elem_interface.m_Requirement_Interface_Send.Contains(Requ_List2[0]) == false)
                                          {
                                              elem_interface.m_Requirement_Interface_Send.Add(Requ_List2[0]);

                                          }
                                          if (elem_interface.m_Requirement_Interface_Receive.Contains(Requ_List2[1]) == false)
                                          {
                                              elem_interface.m_Requirement_Interface_Receive.Add(Requ_List2[1]);
                                          }
                                      }
                                    */

                                    //MessageBox.Show(elem_interface.m_Requirement_Interface_Send.Count.ToString());

                                    List<InformationElement> InfoElm = target.Get_Information_Element_Logical(Repository, Connector.ConnectorGUID, LA_Logical, this);

                                    if (InfoElm.Count > 0)
                                    {
                                        int d4 = 0;
                                        do
                                        {
                                            target.Add_InformationElement(InfoElm[d4]);

                                            d4++;
                                        } while (d4 < InfoElm.Count);
                                    }
                                }


                            }
                            d3++;
                        } while (d3 < Connector_ID.Count);
                    }
                    else
                    {
                        //nicht vorhanden --> keine Aktion
                    }

                }


            }
            //  MessageBox.Show("Test0");
            //Kinderelemente rekursiv Ã¼berlaufen
            EA.Collection Childs = El.Elements;

            //  MessageBox.Show("Test1");


            if (Childs.Count > 0)
            {
                //  MessageBox.Show("Anzahl Child: "+Childs.Count.ToString());

                short i2 = 0;
                do
                {
                    List<string> GUID = new List<string>();
                    //Classifier Element
                    GUID = this.Get_Element_Classifier(Element_GUID, Repository, this);

                    if (GUID != null)
                    {
                        NodeType Client;

                        if (Logical == true)
                        {
                            Client = this.Check_Decomposition_LA_For_NodeType(GUID[0]);
                        }
                        else
                        {
                            Client = node;
                        }

                        // MessageBox.Show(Client.m_Element_Interface.Count.ToString());

                        NodeType Child = Client.Check_Decomposition_For_NodeType(this.Get_Element_Classifier(Childs.GetAt(i2).ElementGUID, Repository, this)[0]);

                        if (Child != null)
                        {
                            Create_Connectors(Repository, Childs.GetAt(i2).ElementGUID, false, Child, flag_NT, LA_Logical);
                        }

                    }



                    /*
                    var Guid = this.Get_Element_Classifier(Childs.GetAt(i2).ElementGUID, Repository)[0];

                    MessageBox.Show(Guid);

                    if (Logical == true)
                    {
                        NodeType Child = this.Check_Decomposition_LA_For_NodeType(Guid);

                        MessageBox.Show(Child.Classifier_ID);

                        if(Child.Classifier_ID == Guid)
                        {
                            Create_Connectors(Repository, Childs.GetAt(i2).ElementGUID, false, Child);
                        }
                        
                    }
                    else
                    {
                        NodeType Child = node.Check_Decomposition_For_NodeType(Guid);
                        if (Child != null)
                        {
                            Create_Connectors(Repository, Childs.GetAt(i2).ElementGUID, false, Child);
                        }
                        
                    }
                    */
                    i2++;
                } while (i2 < Childs.Count);
            }
        }



        /// <summary>
        /// Es wird der Calsssifiers des aktuellen Elemntes ausgegeben
        /// </summary>
        /// <param name="ElementGUID"></param>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public List<string> Get_Element_Classifier(string ElementGUID, EA.Repository Repository, Database database)
        {
            Repository_Element rep_elem = new Repository_Element();
            rep_elem.Classifier_ID = ElementGUID;

            //   EA.Element recentElement = Repository.GetElementByGuid(ElementGUID);
            Interfaces.Interface_XML interface_XML = new Interface_XML();

            Requirement_Plugin.xml.XML xml = new Requirement_Plugin.xml.XML();
           
            List<string> help2 = new List<string>();

            switch (rep_elem.Get_Type(this))
            {
                case "Class":
                    help2 = interface_XML.SQL_Query_Select("ea_guid", ElementGUID, "ea_guid", "t_object", database);
                    break;
                case "Part":
                    help2 = interface_XML.SQL_Query_Select( "ea_guid", ElementGUID, "PDATA1", "t_object", database);
                    break;
                case "Port":
                    help2 = interface_XML.SQL_Query_Select( "ea_guid", ElementGUID, "PDATA1", "t_object", database);
                    break;
                case "Activity":
                    help2 = null;
                    break;
                case "Action":
                    help2 = null;
                    break;
                default:
                    help2 = interface_XML.SQL_Query_Select( "ea_guid", ElementGUID, "ea_guid", "t_object", database);
                    break;
            }

            // MessageBox.Show(Repository.GetElementByGuid(help2[0]).Name);

            return (help2);
        }

        private void Decomposition_Get_SYS_AGID_ID(EA.Repository repository, Metamodel metamodel)
        {
            if (this.Decomposition.m_NodeType.Count > 0)
            {
                int i1 = 1;
                string Logical = this.metamodel.m_Header_agid[0];
                int lauf = 1;
                TaggedValue tagged = new TaggedValue(metamodel, this);
                List<DB_Insert> m_Insert = new List<DB_Insert>();

                do
                {
                    m_Insert.Clear();
                    m_Insert.Add(new DB_Insert("SYS_AG_ID", OleDbType.VarChar, OdbcType.VarChar, Logical + i1.ToString(), -1));
                    NodeType inst = new NodeType(null, repository, this);
                    inst.Classifier_ID = this.Decomposition.m_NodeType[i1 - 1].Instantiate_GUID;
                    inst.ID = inst.Get_Object_ID(this);
                    inst.Update_TV(m_Insert, this, repository);

                   // tagged.Update_Tagged_Value(this.Decomposition.m_NodeType[i1 - 1].Instantiate_GUID, "SYS_AG_ID", Logical + i1.ToString(), null, repository);
                    Get_Sys_AG_ID_rekursiv(this.Decomposition.m_NodeType[i1 - 1], repository, Logical + i1.ToString() + ".", metamodel);

                    i1++;
                } while (i1 <= this.Decomposition.m_NodeType.Count);
            }
        }

        private void Get_Sys_AG_ID_rekursiv(NodeType nodetype, EA.Repository repository, string Header, Metamodel metamodel)
        {
            TaggedValue tagged = new TaggedValue(metamodel, this);
            List<DB_Insert> m_Insert = new List<DB_Insert>();

            if (nodetype.m_Child.Count > 0)
            {
                int i1 = 1;
                do
                {
                    // tagged.Update_Tagged_Value(nodetype.m_Child[i1 - 1].Instantiate_GUID, "SYS_AG_ID", Header + i1.ToString(), null, repository);

                    m_Insert.Clear();
                    m_Insert.Add(new DB_Insert("SYS_AG_ID", OleDbType.VarChar, OdbcType.VarChar, Header + i1.ToString(), -1));
                 //   nodetype.m_Child[i1 - 1].Update_TV(m_Insert, this, repository);

                    NodeType inst = new NodeType(null, repository, this);
                    inst.Classifier_ID = nodetype.m_Child[i1 - 1].Instantiate_GUID;
                    inst.ID = inst.Get_Object_ID(this);
                    inst.Update_TV(m_Insert, this, repository);

                    Get_Sys_AG_ID_rekursiv(nodetype.m_Child[i1 - 1], repository, Header + i1.ToString() + ".", metamodel);

                    i1++;
                } while (i1 <= nodetype.m_Child.Count);
            }
        }



        private void Database_Get_F_AGID_ID(EA.Repository repository)
        {
            if (this.m_Capability.Count > 0)
            {
                int i1 = 1;
                string Funktionsbaum = this.metamodel.m_Header_agid[2];
                int lauf = 1;
                TaggedValue tagged = new TaggedValue(this.metamodel, this);
                List<DB_Insert> m_Insert = new List<DB_Insert>();

                do
                {
                    if (this.m_Capability[i1 - 1].m_Parent.Count == 0)
                    {
                        m_Insert.Clear();
                        m_Insert.Add(new DB_Insert("SYS_AG_ID", OleDbType.VarChar, OdbcType.VarChar, Funktionsbaum + lauf.ToString(), -1));
                        this.m_Capability[i1 - 1].Update_TV(m_Insert, this, repository);

                       // tagged.Update_Tagged_Value(this.m_Capability[i1 - 1].Classifier_ID, "SYS_AG_ID", Funktionsbaum + lauf.ToString(), null, repository);
                        Get_F_AG_ID_rekursiv(this.m_Capability[i1 - 1], repository, Funktionsbaum + lauf.ToString() + ".");
                        lauf++;
                    }

                    i1++;
                } while (i1 <= this.m_Capability.Count);
            }
        }

        private void Get_F_AG_ID_rekursiv(Capability recent, EA.Repository repository, string Header)
        {
            TaggedValue tagged = new TaggedValue(this.metamodel, this);
            List<DB_Insert> m_Insert = new List<DB_Insert>();

            if (recent.m_Child.Count > 0)
            {
                int i1 = 1;
                do
                {
                    //  tagged.Update_Tagged_Value(recent.m_Child[i1 - 1].Classifier_ID, "SYS_AG_ID", Header + i1.ToString(), null, repository);
                    m_Insert.Clear();
                    m_Insert.Add(new DB_Insert("SYS_AG_ID", OleDbType.VarChar, OdbcType.VarChar, Header + i1.ToString(), -1));
                    recent.m_Child[i1 - 1].Update_TV(m_Insert, this, repository);
                    Get_F_AG_ID_rekursiv(recent.m_Child[i1 - 1], repository, Header + i1.ToString() + ".");

                    i1++;
                } while (i1 <= recent.m_Child.Count);
            }
        }


        /// <summary>
        /// Hiermit werden die NodeType der Database auf die Element_Functional hin analysiert
        /// </summary>
		private void Get_Functional_NodeType(EA.Repository repository, Loading_OpArch Load)
        {
            if (this.m_NodeType.Count > 0)
            {
                Load.progressBar1.Maximum = this.m_NodeType.Count;
                Load.progressBar1.Value = 0;
                Load.progressBar1.Step = 1;
                Load.Refresh();

            /*    Parallel.ForEach(this.m_NodeType, nodeType =>
                {
                    try
                    {
                        nodeType.Get_Functional(repository, this, Load);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Functional AFO NodeType: " + nodeType.Name + ": " + err.Message);
                    }
                });*/
                #region Archiv

               int i1 = 0;
                do
                {
                    try
                    {
                        Load.progressBar1.PerformStep();
                        Load.Refresh();
                        this.m_NodeType[i1].Get_Functional(repository, this, Load);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Functional AFO NodeType: " + this.m_NodeType[i1].Name + ": " + err.Message);
                    }


                    i1++;
                } while (i1 < this.m_NodeType.Count);
              
                #endregion
            }

        }
        /// <summary>
        /// Hier werden alle Element User angelegt
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="Load"></param>
        private void Get_Element_User(EA.Repository repository, Loading_OpArch Load)
        {
            ///Schleife über alle NodeType
            if (this.m_NodeType.Count > 0)
            {
                Load.progressBar1.Maximum = this.m_NodeType.Count;
                Load.progressBar1.Value = 0;
                Load.progressBar1.Step = 1;

               

              
                int i1 = 0;
                do
                {
                    try
                    {
                        Load.progressBar1.PerformStep();
                        Load.label2.Text = this.m_NodeType[i1].Get_Name(this);
                        Load.Refresh();
                        if (this.m_NodeType[i1].m_Stakeholder.Count > 0)
                        {
                            if (this.m_NodeType[i1].m_Element_Functional.Count > 0)
                            {
                                int length = this.m_NodeType[i1].m_Element_Functional.Count;

                                int i2 = 0;
                                do
                                {
                                    this.m_NodeType[i1].m_Element_Functional[i2].Supplier.Transform_Element_Functional_To_Element_User(repository, this, this.m_NodeType[i1]);

                                    this.m_NodeType[i1].m_Element_Functional.RemoveAt(i2);
                                    length--;

                                } while (i2 < length);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("User AFO NodeType: " + this.m_NodeType[i1].Name + ": " + err.Message);
                    }

                    i1++;
                } while (i1 < this.m_NodeType.Count);
               
            }
          
        }
        #endregion Get_Elements

        #region Requirements
        #region Check_Requirements
        public void Check_Requirements(EA.Repository repository)
        {
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Loading_OpArch load = new Loading_OpArch();
            load.Show();
            //Alle NodeType erhalten
            repository_Elements.Get_NodeTypes_Parallel(this, repository, load);
            //Alle Logical erhalten
            repository_Elements.Get_Logicals_Parallel(this);
            //Alle REquirementCategory erhalten
            repository_Elements.Get_Capability_Parallel(this, repository);
            //Alle Requirements erhalten
            repository_Elements.Get_Requirements_Parallel(this, repository, this.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), this.metamodel.m_Requirement.Select(x => x.Type).ToList(), load);
            //Alle REquirements, welche RPI_Export == true und Satzschablone == true
            if (this.m_Requirement.Count > 0)
            {
                #region PAckages
                List<string> m_package_guid = new List<string>();
                Repository_Element repository_Element = new Repository_Element();
                //Packages anlegen
                string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                //Package InfoÃ¼bertragung anlegen bzw erhalten
                string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Plausibilitätscheck", repository, this);
                EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Update();
                // string package_GUID = 
                string package_GUID = repository_Element.Create_Package_Model("AFO_Subject", repository, this);
                EA.Package Package_2 = repository.GetPackageByGuid(package_GUID);
                Package_2.ParentID = Package_Infoübertragung.PackageID;
                m_package_guid.Add(package_GUID);
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Packages.Refresh();
                Package_2.Update();
                //Satzschablone 
                string package_GUID2 = repository_Element.Create_Package_Model("Satzschablone", repository, this);
                EA.Package Package_3 = repository.GetPackageByGuid(package_GUID);
                Package_3.ParentID = Package_Infoübertragung.PackageID;
                m_package_guid.Add(package_GUID);
                Package_Requirement.Packages.Refresh();
                Package_Infoübertragung.Packages.Refresh();
                Package_3.Update();
                #endregion Packages
                #region Subject Prüfen
                //Subject überprüfen
                List<List<string>> m_guid_Issue = new List<List<string>>();

                if(this.m_Requirement.Count > 0)
                {
                     int i1 = 0;
                      do
                      {
                          List<string> m_help = new List<string>();

                          List<string> m_ret = this.m_Requirement[i1].Check_Subject(this, repository, m_package_guid);


                          m_help.Add(m_ret[0]);
                          m_help.Add(m_ret[1]);
                          m_help.Add(m_ret[2]);
                          m_help.Add(this.m_Requirement[i1].Classifier_ID);

                          m_guid_Issue.Add(m_help);
                          i1++;
                      } while (i1 < this.m_Requirement.Count);
                    /*
                    Parallel.ForEach(this.m_Requirement, requirement =>
                    {
                        List<string> m_help = new List<string>();
                        List<string> m_ret = requirement.Check_Subject(this, repository, m_package_guid);
                        m_help.Add(m_ret[0]);
                        m_help.Add(m_ret[1]);
                        m_help.Add(m_ret[2]);
                        m_help.Add(requirement.Classifier_ID);

                        m_guid_Issue.Add(m_help);
                    });
                    */
                }
               

                /*Parallel.ForEach(this.m_Requirement, requirement =>
                {
                    List<string> m_help = new List<string>();

                    List<string> m_ret = requirement.Check_Subject(this, repository, m_package_guid);


                    m_help.Add(m_ret[0]);
                    m_help.Add(m_ret[1]);
                    m_help.Add(m_ret[2]);
                    m_help.Add(requirement.Classifier_ID);

                    m_guid_Issue.Add(m_help);

             
                });*/

                Repository_Connector repository_Connector = new Repository_Connector();

                //Konnektoren anlegen
                if(m_guid_Issue.Count > 0)
                {
                    int i2 = 0;
                    do
                    {

                        if (m_guid_Issue[i2][0] != null)
                        {
                            repository_Connector.Create_Dependency(m_guid_Issue[i2][0], m_guid_Issue[i2][3], this.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), this.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), null, repository, this, this.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], this.metamodel.m_Con_Trace[0].direction);
                        }
                        if (m_guid_Issue[i2][1] != null)
                        {
                            repository_Connector.Create_Dependency(m_guid_Issue[i2][1], m_guid_Issue[i2][3], this.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), this.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), null, repository, this, this.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], this.metamodel.m_Con_Trace[0].direction);
                        }
                        if (m_guid_Issue[i2][2] != null)
                        {
                            repository_Connector.Create_Dependency(m_guid_Issue[i2][2], m_guid_Issue[i2][3], this.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), this.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), null, repository, this, this.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], this.metamodel.m_Con_Trace[0].direction);
                        }

                        i2++;
                         
                    } while (i2 < m_guid_Issue.Count);
                }
                

            }
            #endregion
                
                

            load.Close();

            
        }

        public void Check_Nachweisart(EA.Repository repository)
        {
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Loading_OpArch load = new Loading_OpArch();
            load.Show();

            repository_Elements.Get_Nachweisarten_Parallel(this, repository, load);

            #region Type
            List<string> m_Type = new List<string>();
            m_Type.AddRange(this.metamodel.m_Requirement_Design.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_Environment.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_Functional.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_NonFunctional.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_Process.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_Quality_Class.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_Quality_Activity.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_Typvertreter.Select(x => x.Type).ToList());
            m_Type.AddRange(this.metamodel.m_Requirement_User.Select(x => x.Type).ToList());
            #endregion
            #region Stereotype
            List<string> m_Stereotype = new List<string>();
            m_Stereotype.AddRange(this.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_NonFunctional.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_Quality_Class.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_Quality_Activity.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_Typvertreter.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(this.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList());
            #endregion

            List<string> m_GUID = repository_Elements.Get_Requirements_GUID(this, m_Type, m_Stereotype);

            if(m_GUID.Count > 0)
            {
                load.label2.Text = "Überprüfung  Nachweisarten";
                load.label_Progress.Text = "Anforderung mehrere Nachweisarten";

                load.progressBar1.Minimum = 0;
                load.progressBar1.Maximum = 2;
                load.progressBar1.Value = 0;
                load.progressBar1.Step = 1;
                load.Refresh();

                #region Fälle prüfen
                #region Anforderung mit mehreren Nachwiesarten
                List<List<string>> m_Dopplung_Nachweisart = new List<List<string>>();
                List<List<string>> m_Abgleich_Nachweisart = new List<List<string>>();
                Parallel.ForEach(m_GUID, guid =>
                {
                    List<string> m_help = new List<string>();
                    Requirement req = new Requirement(null, this.metamodel);
                    req.Classifier_ID = guid;
                    req.ID = req.Get_Object_ID(this);
                    m_help = req.Check_Nachweisarten_Mehrere(this, repository);
                    
                    if(m_help != null)
                    {
                        if (m_help.Count > 0)
                        {
                            m_Dopplung_Nachweisart.Add(m_help);
                        }
                    }

                    m_help = req.Check_Nachweisarten_Abgleich(this, repository);

                    if (m_help != null)
                    {
                        if (m_help.Count > 0)
                        {
                            m_Abgleich_Nachweisart.Add(m_help);
                        }
                    }

                });
                #endregion
               
                #endregion
                #region Issue anlegen
                if (m_Dopplung_Nachweisart.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Mehrfache Nachweisarten", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        if(m_Dopplung_Nachweisart[i1] != null)
                        {
                            repository_Elements.Create_Issue_Nachweisart_Mehrfach(m_Dopplung_Nachweisart[i1], this, repository, m_package_guid);
                        }


                        i1++;
                    } while (i1 < m_Dopplung_Nachweisart.Count);

                }
                if(m_Abgleich_Nachweisart.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Abgleich Nachweisart", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        if (m_Abgleich_Nachweisart[i1] != null)
                        {
                            repository_Elements.Create_Issue_Nachweisart_Abgleich(m_Abgleich_Nachweisart[i1], this, repository, m_package_guid);
                        }


                        i1++;
                    } while (i1 < m_Abgleich_Nachweisart.Count);
                }
                #endregion

            }


            load.Close();


        }


        public void Check_Requirements_Dopplung(EA.Repository repository)
        {
            //Schleife über NodeType haben Kinderelemente die selbe Activity Kombination?
            if(this.m_NodeType.Count > 0 && this.metamodel.flag_sysarch == false)
            {
                Loading_OpArch load = new Loading_OpArch();
                load.label2.Text = "Überprüfung Dopplungen";
                load.label_Progress.Text = "Requirement Functional";

                load.progressBar1.Minimum = 0;
                load.progressBar1.Maximum = 7;
                load.progressBar1.Value = 0;
                load.progressBar1.Step = 1;
                load.Refresh();
                load.Show();
                #region Dopplung Functional
                List<List<Repository_Element>> m_Dopplung_Functional = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_NodeType, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Functional(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Functional.AddRange(m_help);
                    }
                });

               if(m_Dopplung_Functional.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Functional", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Functional.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if(m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_Functional(this, repository, m_help_Issue, m_package_guid, true);
                        }

                        i1++;
                    } while( i1 < this.m_NodeType.Count);
                }


                #endregion

                #region Dopplung User
                List<List<Repository_Element>> m_Dopplung_User = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_NodeType, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_User(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_User.AddRange(m_help);
                    }
                });

                if (m_Dopplung_User.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung User", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_User.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_User(this, repository, m_help_Issue, m_package_guid, true);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
                #endregion

                #region Dopplung Process
                //Ausgangsbasis Dopplung Functional
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Process";
                load.Refresh();
                if (m_Dopplung_Functional.Count > 0)
                {
                    List<List<Repository_Element>> m_Dopplung_Process = new List<List<Repository_Element>>();


                    Parallel.ForEach(this.m_NodeType, nodetype =>
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Functional.Where(x => (NodeType)x[0] == nodetype).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                            m_help = nodetype.Check_Dopplung_Process(this, repository, m_help_Issue);

                            if (m_help.Count > 0)
                            {
                                m_Dopplung_Process.AddRange(m_help);
                            }
                        }
                    });

                    if (m_Dopplung_Process.Count > 0)
                    {
                        #region PAckages
                        List<string> m_package_guid = new List<string>();
                        Repository_Element repository_Element = new Repository_Element();
                        //Packages anlegen
                        string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                        EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                        //Package InfoÃ¼bertragung anlegen bzw erhalten
                        string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Process", repository, this);
                        EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                        Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                        Package_Requirement.Packages.Refresh();
                        Package_Infoübertragung.Update();
                        m_package_guid.Add(Package_Infoübertragung_GUID);
                        #endregion

                        int i1 = 0;
                        do
                        {
                            List<List<Repository_Element>> m_help_Issue = m_Dopplung_Process.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                            if (m_help_Issue.Count > 0)
                            {
                                this.m_NodeType[i1].Create_Issue_Dopplung_Process(this, repository, m_help_Issue, m_package_guid, true);
                            }

                            i1++;
                        } while (i1 < this.m_NodeType.Count);
                    }
                }
                #endregion

                #region Dopplung Design
                //Schleife über NodeType und Abgleich Element Design
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Design";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_Design = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_NodeType, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Design(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Design.AddRange(m_help);
                    }
                });

                if (m_Dopplung_Design.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Design", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Design.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_Design(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
                #endregion

                #region Dopplung Typvertreter
                //Schleife über NodeType und Abgleich Element Typvertreter
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Typvertreter";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_Typvertreter = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_NodeType, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Typverteter(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Typvertreter.AddRange(m_help);
                    }
                });

                if (m_Dopplung_Typvertreter.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Typvertreter", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Typvertreter.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_Typvertreter(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
                #endregion

                #region Dopplung Umwelt
                //Schleife über NodeType und Abgleich Element Umwelt
                //Schleife über NodeType und Abgleich Element Typvertreter
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Umwelt";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_Umwelt = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_NodeType, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Umwelt(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Umwelt.AddRange(m_help);
                    }
                });

                if (m_Dopplung_Umwelt.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Umwelt", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Umwelt.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_Umwelt(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
                #endregion

                #region Dopplung Interfaces
                //Schleife über NodeType und Abgleich Element Interface und Element Bidirektional
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Interface";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_Interface_Uni = new List<List<Repository_Element>>();
                List<List<Repository_Element>> m_Dopplung_Interface_Bi = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_NodeType, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Interface_Unidirektional(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Interface_Uni.AddRange(m_help);
                    }
                    m_help = nodeType.Check_Dopplung_Interface_Bidirektional(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Interface_Bi.AddRange(m_help);
                    }

                });
                if (m_Dopplung_Interface_Uni.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Interface Unidirektional", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Interface_Uni.Where(x => x != null).Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_Interface_Unidirektional(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
                if (m_Dopplung_Interface_Bi.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Interface Bidirektional", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Interface_Bi.Where(x => x != null).Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_Interface_Bidirektional(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
                #endregion

                #region Dopplung QualityClass
                //Schleife über NodeType und Abgleich Element Typvertreter
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement QualityClass";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_QualityClass = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_NodeType, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_QualityClass(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_QualityClass.AddRange(m_help);
                    }
                });

                if (m_Dopplung_QualityClass.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung QualityClass", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_QualityClass.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_QualityClass(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
                #endregion

                #region Dopplung QualityActivity
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement QualityActivity";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_QualityActivity = new List<List<Repository_Element>>();
               

                Parallel.ForEach(this.m_NodeType, nodetype =>
                {
                    List<List<Repository_Element>> m_help_Issue = m_Dopplung_Functional.Where(x => (NodeType)x[0] == nodetype).ToList();

                    if (m_help_Issue.Count > 0)
                    {
                        List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                        m_help = nodetype.Check_Dopplung_QualityActivity(this, repository, m_help_Issue);

                        if (m_help.Count > 0)
                        {
                            m_Dopplung_QualityActivity.AddRange(m_help);
                        }
                    }
                });

                if (m_Dopplung_QualityActivity.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung QualityActivity", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_QualityActivity.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_QualityActivity(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }

           /*     if (m_Dopplung_QualityActivity.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung QualityClass", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_QualityClass.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_NodeType[i1].Create_Issue_Dopplung_QualityActivity(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }*/
                #endregion

                load.Close();

                repository.RefreshModelView(0);
            }

            if(this.m_SysElemente.Count > 0 && this.metamodel.flag_sysarch == true)
            {
                Loading_OpArch load = new Loading_OpArch();
                load.label2.Text = "Überprüfung Dopplungen";
                load.label_Progress.Text = "Requirement Functional";

                load.progressBar1.Minimum = 0;
                load.progressBar1.Maximum = 7;
                load.progressBar1.Value = 0;
                load.progressBar1.Step = 1;
                load.Refresh();
                load.Show();

                #region Dopplung Functional
                List<List<Repository_Element>> m_Dopplung_Functional = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_SysElemente, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Functional(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Functional.AddRange(m_help);
                    }
                });

                if (m_Dopplung_Functional.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Functional", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Functional.Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_Functional(this, repository, m_help_Issue, m_package_guid, true);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }


                #endregion

                #region Dopplung User
                List<List<Repository_Element>> m_Dopplung_User = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_SysElemente, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_User(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_User.AddRange(m_help);
                    }
                });

                if (m_Dopplung_User.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung User", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_User.Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_User(this, repository, m_help_Issue, m_package_guid, true);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }
                #endregion

                #region Dopplung Process
                //Ausgangsbasis Dopplung Functional
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Process";
                load.Refresh();
                if (m_Dopplung_Functional.Count > 0)
                {
                    List<List<Repository_Element>> m_Dopplung_Process = new List<List<Repository_Element>>();


                    Parallel.ForEach(this.m_SysElemente, nodetype =>
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Functional.Where(x => (SysElement)x[0] == nodetype).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                            m_help = nodetype.Check_Dopplung_Process(this, repository, m_help_Issue);

                            if (m_help.Count > 0)
                            {
                                m_Dopplung_Process.AddRange(m_help);
                            }
                        }
                    });

                    if (m_Dopplung_Process.Count > 0)
                    {
                        #region PAckages
                        List<string> m_package_guid = new List<string>();
                        Repository_Element repository_Element = new Repository_Element();
                        //Packages anlegen
                        string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                        EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                        //Package InfoÃ¼bertragung anlegen bzw erhalten
                        string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Process", repository, this);
                        EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                        Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                        Package_Requirement.Packages.Refresh();
                        Package_Infoübertragung.Update();
                        m_package_guid.Add(Package_Infoübertragung_GUID);
                        #endregion

                        int i1 = 0;
                        do
                        {
                            List<List<Repository_Element>> m_help_Issue = m_Dopplung_Process.Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                            if (m_help_Issue.Count > 0)
                            {
                                this.m_SysElemente[i1].Create_Issue_Dopplung_Process(this, repository, m_help_Issue, m_package_guid, true);
                            }

                            i1++;
                        } while (i1 < this.m_SysElemente.Count);
                    }
                }
                #endregion

                #region Dopplung Design
                //Schleife über NodeType und Abgleich Element Design
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Design";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_Design = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_SysElemente, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Design(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Design.AddRange(m_help);
                    }
                });

                if (m_Dopplung_Design.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Design", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Design.Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_Design(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }
                #endregion

                #region Dopplung Typvertreter
                //Schleife über NodeType und Abgleich Element Typvertreter
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Typvertreter";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_Typvertreter = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_SysElemente, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Typverteter(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Typvertreter.AddRange(m_help);
                    }
                });

                if (m_Dopplung_Typvertreter.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Typvertreter", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Typvertreter.Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_Typvertreter(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }
                #endregion

                #region Dopplung Umwelt
                //Schleife über NodeType und Abgleich Element Umwelt
                //Schleife über NodeType und Abgleich Element Typvertreter
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Umwelt";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_Umwelt = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_SysElemente, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Umwelt(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Umwelt.AddRange(m_help);
                    }
                });

                if (m_Dopplung_Umwelt.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Umwelt", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Umwelt.Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_Umwelt(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }
                #endregion

                #region Dopplung Interfaces
                //Schleife über NodeType und Abgleich Element Interface und Element Bidirektional
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement Interface";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_Interface_Uni = new List<List<Repository_Element>>();
                List<List<Repository_Element>> m_Dopplung_Interface_Bi = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_SysElemente, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_Interface_Unidirektional(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Interface_Uni.AddRange(m_help);
                    }
                    m_help = nodeType.Check_Dopplung_Interface_Bidirektional(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_Interface_Bi.AddRange(m_help);
                    }

                });
                if (m_Dopplung_Interface_Uni.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Interface Unidirektional", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Interface_Uni.Where(x => x != null).Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_Interface_Unidirektional(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }
                if (m_Dopplung_Interface_Bi.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung Interface Bidirektional", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_Interface_Bi.Where(x => x != null).Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_Interface_Bidirektional(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }
                #endregion

                #region Dopplung QualityClass
                //Schleife über NodeType und Abgleich Element Typvertreter
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement QualityClass";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_QualityClass = new List<List<Repository_Element>>();

                Parallel.ForEach(this.m_SysElemente, nodeType =>
                {
                    List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                    m_help = nodeType.Check_Dopplung_QualityClass(this, repository);

                    if (m_help.Count > 0)
                    {
                        m_Dopplung_QualityClass.AddRange(m_help);
                    }
                });

                if (m_Dopplung_QualityClass.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung QualityClass", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_QualityClass.Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_QualityClass(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }
                #endregion

                #region Dopplung QualityActivity
                load.progressBar1.PerformStep();
                load.label_Progress.Text = "Requirement QualityActivity";
                load.Refresh();
                List<List<Repository_Element>> m_Dopplung_QualityActivity = new List<List<Repository_Element>>();


                Parallel.ForEach(this.m_SysElemente, nodetype =>
                {
                    List<List<Repository_Element>> m_help_Issue = m_Dopplung_Functional.Where(x => (SysElement)x[0] == nodetype).ToList();

                    if (m_help_Issue.Count > 0)
                    {
                        List<List<Repository_Element>> m_help = new List<List<Repository_Element>>();
                        m_help = nodetype.Check_Dopplung_QualityActivity(this, repository, m_help_Issue);

                        if (m_help.Count > 0)
                        {
                            m_Dopplung_QualityActivity.AddRange(m_help);
                        }
                    }
                });

                if (m_Dopplung_QualityActivity.Count > 0)
                {
                    #region PAckages
                    List<string> m_package_guid = new List<string>();
                    Repository_Element repository_Element = new Repository_Element();
                    //Packages anlegen
                    string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                    EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                    //Package InfoÃ¼bertragung anlegen bzw erhalten
                    string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung QualityActivity", repository, this);
                    EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                    Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                    Package_Requirement.Packages.Refresh();
                    Package_Infoübertragung.Update();
                    m_package_guid.Add(Package_Infoübertragung_GUID);
                    #endregion

                    int i1 = 0;
                    do
                    {
                        List<List<Repository_Element>> m_help_Issue = m_Dopplung_QualityActivity.Where(x => (SysElement)x[0] == this.m_SysElemente[i1]).ToList();

                        if (m_help_Issue.Count > 0)
                        {
                            this.m_SysElemente[i1].Create_Issue_Dopplung_QualityActivity(this, repository, m_help_Issue, m_package_guid);
                        }

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }

                /*     if (m_Dopplung_QualityActivity.Count > 0)
                     {
                         #region PAckages
                         List<string> m_package_guid = new List<string>();
                         Repository_Element repository_Element = new Repository_Element();
                         //Packages anlegen
                         string Package_Requirement_GUID = repository_Element.Create_Package_Model("Issue - Requirement_Plugin", repository, this);
                         EA.Package Package_Requirement = repository.GetPackageByGuid(Package_Requirement_GUID);
                         //Package InfoÃ¼bertragung anlegen bzw erhalten
                         string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Dopplung QualityClass", repository, this);
                         EA.Package Package_Infoübertragung = repository.GetPackageByGuid(Package_Infoübertragung_GUID);
                         Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
                         Package_Requirement.Packages.Refresh();
                         Package_Infoübertragung.Update();
                         m_package_guid.Add(Package_Infoübertragung_GUID);
                         #endregion

                         int i1 = 0;
                         do
                         {
                             List<List<Repository_Element>> m_help_Issue = m_Dopplung_QualityClass.Where(x => (NodeType)x[0] == this.m_NodeType[i1]).ToList();

                             if (m_help_Issue.Count > 0)
                             {
                                 this.m_NodeType[i1].Create_Issue_Dopplung_QualityActivity(this, repository, m_help_Issue, m_package_guid);
                             }

                             i1++;
                         } while (i1 < this.m_NodeType.Count);
                     }*/
                #endregion

                load.Close();

                repository.RefreshModelView(0);
            }
            //Kinderelemente mit denselben Constraints verknüpft?
        }
        #endregion Get_Requirements

        #region Update
        public void Update_Systemelemente(EA.Repository repository)
        {
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Loading_OpArch load = new Loading_OpArch();
            load.Show();
            //Alle NodeType Updaten
            repository_Elements.Update_NodeTypes_Parallel(this, repository, load);
            //Alle Logical Updaten
             repository_Elements.Update_Logical_Parallel(this, repository, load);
            //Alle ReqCategory Updaten
            repository_Elements.Update_ReqCategory_Parallel(this, repository, load);
            //Alle Systemelemente Updaten
            repository_Elements.Update_SysElemente_Parallel(this, repository, load);


            load.Close();

        }

        public void Update_Nachweisarten(EA.Repository repository)
        {
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Loading_OpArch load = new Loading_OpArch();
            load.Show();
            //Alle Nachweisarten updaten
            repository_Elements.Update_Nachweisarten_Parallel(this, repository, load);
            //Anforderungen Nachweisarten Updaten
            repository_Elements.Update_Anforderungen_Nachweisarten_Parallel(this, repository, load);

            load.Close();
        }

        #endregion Update

        #region Anforderungen auto
        #region Interface Auto
        /// <summary>
        /// Es werden automatisch im Maximalprinzip alle Schnittstellen AFO erzeugt
        /// </summary>
        public void Create_Requirements_Interface_AFO_Auto(EA.Repository Repository, Loading_OpArch loading, List<NodeType> m_create)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
           // Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Erstelle Packages...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

            //PAckage allg Requirement anlegen bzw. erhalten
            string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this);
            EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
            //Package InfoÃ¼bertragung anlegen bzw erhalten
            string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("Infoübertragung - Requirement_Plugin", Repository, this);
            EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
            Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
            //InfoÃ¼bertragung Child von Requirement
            Package_Requirement.Packages.Refresh();
            Package_Infoübertragung.Update();

            //Rekursiv alle Elemente bearbeiten im Baum
            if(this.Decomposition != null)
            {
                if (this.Decomposition.m_NodeType.Count > 0)
                {
                    //Ladebalken
                    loading.label_Progress.Text = "Anforderungserstellung";
                    loading.Refresh();

                    int i1 = 0;
                    do
                    {

                        if(m_create.Contains(this.Decomposition.m_NodeType[i1]) == true)
                        {
                            loading.progressBar1.Step = 1;
                            loading.progressBar1.PerformStep();
                            loading.label2.Text = this.Decomposition.m_NodeType[i1].Get_Name(this) + ": Unidirektionale Schnittstellen";
                            loading.Refresh();
                            //Alle aktuellen Element_Interface betrachten
                            int i2 = 0;
                            if (this.Decomposition.m_NodeType[i1].m_Element_Interface.Count > 0)
                            {
                                do
                                {
                                    //MessageBox.Show("Vorher: " + i2);
                                    this.Decomposition.m_NodeType[i1].m_Element_Interface[i2].Create_Requirement(Repository, this, Package_Infoübertragung_GUID, false);

                                    //MessageBox.Show("Danach: " + i2);

                                    i2++;
                                } while (i2 < this.Decomposition.m_NodeType[i1].m_Element_Interface.Count);
                            }
                            //Alle Element_Interface_Bidirectional
                            i2 = 0;
                            if (this.Decomposition.m_NodeType[i1].m_Element_Interface_Bidirectional.Count > 0)
                            {
                                loading.label2.Text = this.Decomposition.m_NodeType[i1].Get_Name(this) + ": Bidirektionale Schnittstellen";
                                loading.Refresh();

                                do
                                {

                                    this.Decomposition.m_NodeType[i1].m_Element_Interface_Bidirectional[i2].Create_Requirement(Repository, this, Package_Infoübertragung_GUID, true);

                                    i2++;
                                } while (i2 < this.Decomposition.m_NodeType[i1].m_Element_Interface_Bidirectional.Count);
                            }

                        }

                        //Rekursiver Aufruf
                        Create_Requirements_Interface_AFO_Auto_rekursiv(Repository, Decomposition.m_NodeType[i1], Package_Infoübertragung_GUID, loading, m_create);

                        i1++;
                    } while (i1 < this.Decomposition.m_NodeType.Count);
                }
            }
          

          //  loading.Close();
        }
        /// <summary>
        /// Rekursiver Baumdurchlauf der Decomposition und dabei automatisches Anlegen von den Interface Requirements
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Element"></param>
        private void Create_Requirements_Interface_AFO_Auto_rekursiv(EA.Repository Repository, NodeType Element, string Package_Guid, Loading_OpArch loading, List<NodeType> m_create)
        {
            //Schelife Ã¼ber die Kinderelemente
            if (Element.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_create.Contains(Element.m_Child[i1]) == true)
                    {
                        loading.progressBar1.PerformStep();
                        loading.Refresh();
                        //Schleife Ã¼ber ElementInterface
                        int i2 = 0;
                        if (Element.m_Child[i1].m_Element_Interface.Count > 0)
                        {
                            loading.label2.Text = Element.m_Child[i2].Get_Name(this) + ": Unidirektionale Schnittstellen";
                            loading.Refresh();

                            i2 = 0;
                            do
                            {
                                Element.m_Child[i1].m_Element_Interface[i2].Create_Requirement(Repository, this, Package_Guid, false);

                                i2++;
                            } while (i2 < Element.m_Child[i1].m_Element_Interface.Count);
                        }
                        //Schelife Ã¼ber Element_Interface_Bidirektional
                        if (Element.m_Child[i1].m_Element_Interface_Bidirectional.Count > 0)
                        {
                            loading.label2.Text = Element.m_Child[i1].Get_Name(this) + ": Bidirektionale Schnittstellen";
                            loading.Refresh();

                            i2 = 0;
                            do
                            {

                                Element.m_Child[i1].m_Element_Interface_Bidirectional[i2].Create_Requirement(Repository, this, Package_Guid, true);

                                i2++;
                            } while (i2 < Element.m_Child[i1].m_Element_Interface_Bidirectional.Count);
                        }
                    }
                  

                    Create_Requirements_Interface_AFO_Auto_rekursiv(Repository, Element.m_Child[i1], Package_Guid, loading, m_create);

                    i1++;
                } while (i1 < Element.m_Child.Count);

            }
            else
            {
                return;
            }
        }

        private void Update_Connectoren_Requirements_Interface_AFO_Auto_rekursiv(EA.Repository Repository, NodeType Element, Loading_OpArch loading)
        {
            //Schelife Ã¼ber die Kinderelemente
            if (Element.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    loading.progressBar1.PerformStep();
                    loading.Refresh();
                    //Schleife Ã¼ber ElementInterface
                    int i2 = 0;
                    if (Element.m_Child[i1].m_Element_Interface.Count > 0)
                    {
                        loading.label2.Text = Element.m_Child[i2].Get_Name(this) + ": Unidirektionale Schnittstellen";
                        loading.Refresh();

                        i2 = 0;
                        do
                        {
                            Element.m_Child[i1].m_Element_Interface[i2].Update_Connectoren_Requirement_Interface(Repository, this, false);

                            i2++;
                        } while (i2 < Element.m_Child[i1].m_Element_Interface.Count);
                    }
                    //Schelife Ã¼ber Element_Interface_Bidirektional
                    if (Element.m_Child[i1].m_Element_Interface_Bidirectional.Count > 0)
                    {
                        loading.label2.Text = Element.m_Child[i1].Get_Name(this) + ": Bidirektionale Schnittstellen";
                        loading.Refresh();

                        i2 = 0;
                        do
                        {

                            Element.m_Child[i1].m_Element_Interface_Bidirectional[i2].Update_Connectoren_Requirement_Interface(Repository, this, true);

                            i2++;
                        } while (i2 < Element.m_Child[i1].m_Element_Interface_Bidirectional.Count);
                    }

                    Update_Connectoren_Requirements_Interface_AFO_Auto_rekursiv(Repository, Element.m_Child[i1], loading);

                    i1++;
                } while (i1 < Element.m_Child.Count);

            }
            else
            {
                return;
            }
        }
        #endregion Interface Auto

        #region Functional Auto
        public void Create_Requirements_Functional_AFO_Auto(EA.Repository Repository, Loading_OpArch loading, List<NodeType> m_create)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
         //   Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Erstelle Packages...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

            //PAckage allg Requirement anlegen bzw. erhalten
            string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this);
            EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
            //Package InfoÃ¼bertragung anlegen bzw erhalten
            string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("RequirementFunctional Automatik - Requirement_Plugin", Repository, this);
            EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
            Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
            string Package_User_GUID = repository_Element.Create_Package_Model("RequirementUser Automatik - Requirement_Plugin", Repository, this);
            EA.Package Package_User = Repository.GetPackageByGuid(Package_User_GUID);
            Package_User.ParentID = Package_Requirement.PackageID;
            //InfoÃ¼bertragung Child von Requirement
            Package_Requirement.Packages.Refresh();
            Package_User.Update();
            Package_Infoübertragung.Update();

            if(this.metamodel.flag_Systemelemente == false)
            {

                //Schleife über NodeTypes
                if (this.m_NodeType.Count > 0)
                {
                    int  i1 = 0;
                    do
                    {

                        if(m_create.Contains(this.m_NodeType[i1]) == true)
                        {
                            loading.label_Progress.Text = "NodeType: " + this.m_NodeType[i1].Name;
                            loading.progressBar1.PerformStep();
                            loading.Update();

                            //Schleife über Element Functional
                            if (this.m_NodeType[i1].m_Element_Functional.Count > 0)
                            {
                                int i2 = 0;
                                do
                                {
                                    this.m_NodeType[i1].m_Element_Functional[i2].Create_Requirement_Functional(Repository, this, Package_Infoübertragung.PackageGUID);

                                    i2++;
                                } while (i2 < this.m_NodeType[i1].m_Element_Functional.Count);
                            }
                            //Schleife über Element_User
                            if (this.m_NodeType[i1].m_Element_User.Count > 0)
                            {
                                int i2 = 0;
                                do
                                {
                                    this.m_NodeType[i1].m_Element_User[i2].Create_Requirement_User(Repository, this, Package_User.PackageGUID);


                                    i2++;
                                } while (i2 < this.m_NodeType[i1].m_Element_User.Count);
                            }
                        }

                       

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }


               // loading.Close();

            }
            else
            {
                if(this.m_SysElemente.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        loading.label_Progress.Text = "SysElement: " + this.m_SysElemente[i1].Name;
                        loading.progressBar1.PerformStep();
                        loading.Update();

                        this.m_SysElemente[i1].Create_Sys_Requirement_Functional(Repository, this, Package_Infoübertragung.PackageGUID);

                        i1++;
                    } while (i1 < this.m_SysElemente.Count);
                }

              //  loading.Close();
            }
        }
        #endregion Functional Auto

        #region Design Auto
        public void Create_Requirements_Design_AFO_Auto(EA.Repository Repository, Loading_OpArch loading, List<NodeType> m_create)
        {

            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
           // Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Erstelle Packages...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

            //PAckage allg Requirement anlegen bzw. erhalten
            string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this);
            EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
            //Package InfoÃ¼bertragung anlegen bzw erhalten
            string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("RequirementDesign Automatik - Requirement_Plugin", Repository, this);
            EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
            Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
            //InfoÃ¼bertragung Child von Requirement
            Package_Requirement.Packages.Refresh();
            Package_Infoübertragung.Update();


            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(m_create.Contains(this.m_NodeType[i1]) == true)
                        {
                            loading.label_Progress.Text = this.m_NodeType[i1].Name;
                            loading.progressBar1.PerformStep();

                            loading.Update();

                            this.m_NodeType[i1].Create_Requirement_Design(this, Repository, Package_Infoübertragung.PackageGUID);
                        }
                     

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }




          //  loading.Close();
        

        }
        #endregion Design Auto

        #region Process Auto
        public void Create_Requirements_Process_AFO_Auto(EA.Repository Repository, Loading_OpArch loading, List<NodeType> m_create)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
          //  Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Erstelle Packages...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

            //PAckage allg Requirement anlegen bzw. erhalten
            string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this);
            EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
            //Package InfoÃ¼bertragung anlegen bzw erhalten
            string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("RequirementProcess Automatik - Requirement_Plugin", Repository, this);
            EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
            Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
            //InfoÃ¼bertragung Child von Requirement
            Package_Requirement.Packages.Refresh();
            Package_Infoübertragung.Update();

            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_Activity.Count > 0)
                {
                    int i1 = 0;
                    do
                    {

                        loading.label_Progress.Text = this.m_Activity[i1].W_Object + " " + this.m_Activity[i1].W_Prozesswort;
                        loading.progressBar1.PerformStep();

                        loading.Update();


                        this.m_Activity[i1].Create_Requirement_Process(this, Repository, Package_Infoübertragung.PackageGUID, m_create);


                        i1++;
                    } while (i1 < this.m_Activity.Count);
                }
            }
            else
            {

            }

            // loading.Close();
        }
        #endregion Process Auto

        #region Umwelt Auto
        public void Create_Requirements_Umwelt_AFO_Auto(EA.Repository Repository, Loading_OpArch loading, List<NodeType> m_create)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
            //Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Erstelle Packages...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

            //PAckage allg Requirement anlegen bzw. erhalten
            string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this);
            EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
            //Package InfoÃ¼bertragung anlegen bzw erhalten
            string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("RequirementUmwelt Automatik - Requirement_Plugin", Repository, this);
            EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
            Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
            //InfoÃ¼bertragung Child von Requirement
            Package_Requirement.Packages.Refresh();
            Package_Infoübertragung.Update();

            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(m_create.Contains(this.m_NodeType[i1]) == true)
                        {
                            loading.label_Progress.Text = this.m_NodeType[i1].Name;
                            loading.progressBar1.PerformStep();

                            loading.Update();

                            this.m_NodeType[i1].Create_Requirement_Umwelt(this, Repository, Package_Infoübertragung.PackageGUID);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }



           // loading.Close();
        }
        #endregion Umwelt Auto

        #region Typvertreter Auto
        public void Create_Requirements_Typvertreter_AFO_Auto(EA.Repository Repository, Loading_OpArch loading, List<NodeType> m_create)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
           // Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Erstelle Packages...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

            //PAckage allg Requirement anlegen bzw. erhalten
            string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this);
            EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
            //Package InfoÃ¼bertragung anlegen bzw erhalten
            string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("RequirementTypvertreter Automatik - Requirement_Plugin", Repository, this);
            EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
            Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
            //InfoÃ¼bertragung Child von Requirement
            Package_Requirement.Packages.Refresh();
            Package_Infoübertragung.Update();

            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(m_create.Contains(this.m_NodeType[i1]) == true)
                        {
                            loading.label_Progress.Text = this.m_NodeType[i1].Name;
                            loading.progressBar1.PerformStep();

                            loading.Update();

                            this.m_NodeType[i1].Create_Requirement_Typvertreter(this, Repository, Package_Infoübertragung.PackageGUID);
                        }

                        

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }



//            loading.Close();
        }
        #endregion Typvertreter Auto

        #region Quality_Class
        public void Create_Requirements_QualityClass_AFO_Auto(EA.Repository Repository, Loading_OpArch loading, List<NodeType> m_create)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
           // Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Erstelle Packages...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

            //PAckage allg Requirement anlegen bzw. erhalten
            string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this);
            EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
            //Package InfoÃ¼bertragung anlegen bzw erhalten
            string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("RequirementQualityClass Automatik - Requirement_Plugin", Repository, this);
            EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
            Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
            //InfoÃ¼bertragung Child von Requirement
            Package_Requirement.Packages.Refresh();
            Package_Infoübertragung.Update();


            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_create.Contains(this.m_NodeType[i1]) == true)
                        {
                            loading.label_Progress.Text = this.m_NodeType[i1].Name;
                            loading.progressBar1.PerformStep();

                            loading.Update();

                            this.m_NodeType[i1].Create_Requirement_QualityClass(this, Repository, Package_Infoübertragung.PackageGUID);
                        }



                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }

        }
        #endregion

        #region Quality Activity
        public void Create_Requirements_QualityActivity_AFO_Auto(EA.Repository Repository, Loading_OpArch loading, List<NodeType> m_create)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
           // Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Erstelle Packages...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

            //PAckage allg Requirement anlegen bzw. erhalten
            string Package_Requirement_GUID = repository_Element.Create_Package_Model("Requirement - Requirement_Plugin", Repository, this);
            EA.Package Package_Requirement = Repository.GetPackageByGuid(Package_Requirement_GUID);
            //Package InfoÃ¼bertragung anlegen bzw erhalten
            string Package_Infoübertragung_GUID = repository_Element.Create_Package_Model("RequirementQualityActivity Automatik - Requirement_Plugin", Repository, this);
            EA.Package Package_Infoübertragung = Repository.GetPackageByGuid(Package_Infoübertragung_GUID);
            Package_Infoübertragung.ParentID = Package_Requirement.PackageID;
            //InfoÃ¼bertragung Child von Requirement
            Package_Requirement.Packages.Refresh();
            Package_Infoübertragung.Update();

            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_create.Contains(this.m_NodeType[i1]) == true)
                        {
                            loading.label_Progress.Text = this.m_NodeType[i1].Name;
                            loading.progressBar1.PerformStep();

                            loading.Update();

                            this.m_NodeType[i1].Create_Requirement_QualityActivity(this, Repository, Package_Infoübertragung.PackageGUID);
                        }



                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }
        }
        #endregion
        #endregion
        #endregion Requirements

        #region Update Conncectoren

        #region Interfaces
        public void Update_Connectoren_Requirments_Interfaces(EA.Repository Repository, Loading_OpArch loading)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
          //  Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Update Konnektoren Requirement Interface...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

          

            //Rekursiv alle Elemente bearbeiten im Baum
            if (this.Decomposition != null)
            {
                if (this.Decomposition.m_NodeType.Count > 0)
                {
                    //Ladebalken
                    loading.label_Progress.Text = "Anforderungserstellung";
                    loading.Refresh();

                    int i1 = 0;
                    do
                    {
                        loading.progressBar1.Step = 1;
                        loading.progressBar1.PerformStep();
                        loading.label2.Text = this.Decomposition.m_NodeType[i1].Get_Name(this) + ": Unidirektionale Schnittstellen";
                        loading.Refresh();
                        //Alle aktuellen Element_Interface betrachten
                        int i2 = 0;
                        if (this.Decomposition.m_NodeType[i1].m_Element_Interface.Count > 0)
                        {
                            do
                            {
                                //MessageBox.Show("Vorher: " + i2);
                                this.Decomposition.m_NodeType[i1].m_Element_Interface[i2].Update_Connectoren_Requirement_Interface(Repository, this, false);

                                //MessageBox.Show("Danach: " + i2);

                                i2++;
                            } while (i2 < this.Decomposition.m_NodeType[i1].m_Element_Interface.Count);
                        }
                        //Alle Element_Interface_Bidirectional
                        i2 = 0;
                        if (this.Decomposition.m_NodeType[i1].m_Element_Interface_Bidirectional.Count > 0)
                        {
                            loading.label2.Text = this.Decomposition.m_NodeType[i1].Get_Name(this) + ": Bidirektionale Schnittstellen";
                            loading.Refresh();

                            do
                            {

                                this.Decomposition.m_NodeType[i1].m_Element_Interface_Bidirectional[i2].Update_Connectoren_Requirement_Interface(Repository, this, true);

                                i2++;
                            } while (i2 < this.Decomposition.m_NodeType[i1].m_Element_Interface_Bidirectional.Count);
                        }
                        //Rekursiver Aufruf
                        Update_Connectoren_Requirements_Interface_AFO_Auto_rekursiv(Repository, Decomposition.m_NodeType[i1], loading);

                        i1++;
                    } while (i1 < this.Decomposition.m_NodeType.Count);
                }
            }


           
        }
        #endregion

        #region Functional
        public void Update_Connectoren_Requirments_Functional(EA.Repository Repository, Loading_OpArch loading)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
         //  Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Update Konnektoren Requirement Functional...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();


            if (this.metamodel.flag_Systemelemente == false)
            {

                //Schleife über NodeTypes
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        loading.label_Progress.Text = "NodeType: " + this.m_NodeType[i1].Name;
                        loading.progressBar1.PerformStep();
                        loading.Update();

                        //Schleife über Element Functional
                        if (this.m_NodeType[i1].m_Element_Functional.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if(this.m_NodeType[i1].m_Element_Functional[i2].m_Requirement_Functional.Count > 0)
                                {
                                    this.m_NodeType[i1].m_Element_Functional[i2].Update_Connectoren_Requirement_Functional(Repository, this);
                                }
                               

                                i2++;
                            } while (i2 < this.m_NodeType[i1].m_Element_Functional.Count);
                        }
                        //Schleife über Element_User
                        if (this.m_NodeType[i1].m_Element_User.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if(this.m_NodeType[i1].m_Element_User[i2].m_Requirement_User.Count > 0)
                                {
                                    this.m_NodeType[i1].m_Element_User[i2].Update_Connetoren_Requirement_User(Repository, this);
                                }
                               


                                i2++;
                            } while (i2 < this.m_NodeType[i1].m_Element_User.Count);
                        }

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }


            }
            else
            {
               


            }



          //  loading.Close();
        }
        #endregion

        #region Design
        public void Update_Connectoren_Requirments_Design(EA.Repository Repository, Loading_OpArch loading)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
          //  Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Update Konnektoren Requirement Design...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

   
            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        loading.label_Progress.Text = this.m_NodeType[i1].Name;
                        loading.progressBar1.PerformStep();

                        loading.Update();

                        this.m_NodeType[i1].Update_Connectoren_Requirement_Design(this, Repository);

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }

       //     loading.Close();
        }
        #endregion

        #region Process
        public void Update_Connectoren_Requirments_Process(EA.Repository Repository, Loading_OpArch loading)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
        //    Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Update Konnektoren Requirement Process...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();

          

            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_Activity.Count > 0)
                {
                    int i1 = 0;
                    do
                    {

                        loading.label_Progress.Text = this.m_Activity[i1].W_Object + " " + this.m_Activity[i1].W_Prozesswort;
                        loading.progressBar1.PerformStep();

                        loading.Update();


                        this.m_Activity[i1].Update_Connectoren_Requirement_Process(this, Repository);


                        i1++;
                    } while (i1 < this.m_Activity.Count);
                }
            }
            else
            {

            }

        //    loading.Close();
        }
        #endregion

        #region Umwelt
        public void Update_Connectoren_Requirments_Umwelt(EA.Repository Repository, Loading_OpArch loading)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
        //    Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Update Konnektoren Requirement Umwelt...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();


            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        loading.label_Progress.Text = this.m_NodeType[i1].Name;
                        loading.progressBar1.PerformStep();

                        loading.Update();

                        this.m_NodeType[i1].Update_Connectoren_Requirement_Umwelt(this, Repository);

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }

         //   loading.Close();
        }
        #endregion

        #region Typverteter
        public void Update_Connectoren_Requirments_Typverteter(EA.Repository Repository, Loading_OpArch loading)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
          //  Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Update Konnektoren Requirement Typvertreter...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();


            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        loading.label_Progress.Text = this.m_NodeType[i1].Name;
                        loading.progressBar1.PerformStep();

                        loading.Update();

                        this.m_NodeType[i1].Update_Connectoren_Requirement_Typvertreter(this, Repository);

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }

         //  loading.Close();
        }
        #endregion

        #region qualityClass
        public void Update_Connectoren_Requirments_QualityClass(EA.Repository Repository, Loading_OpArch loading)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
          //  Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Update Konnektoren Requirement QualityClass...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();


            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        loading.label_Progress.Text = this.m_NodeType[i1].Name;
                        loading.progressBar1.PerformStep();

                        loading.Update();

                        this.m_NodeType[i1].Update_Connectoren_Requirement_QualityClass(this, Repository);

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }

            //  loading.Close();
        }
        #endregion

        #region QualityActivity
        public void Update_Connectoren_Requirments_QualityActivity(EA.Repository Repository, Loading_OpArch loading)
        {
            Repository_Element repository_Element = new Repository_Element();
            ///Ladebalken
          //  Loading_OpArch loading = new Loading_OpArch();
            loading.label_Progress.Text = "Update Konnektoren Requirement QualityActivity...";
            loading.label2.Text = " ";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = this.m_NodeType.Count;
            loading.Show();


            if (this.metamodel.flag_Systemelemente == false)
            {
                if (this.m_NodeType.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        loading.label_Progress.Text = this.m_NodeType[i1].Name;
                        loading.progressBar1.PerformStep();

                        loading.Update();

                        this.m_NodeType[i1].Update_Connectoren_Requirement_QualityActivity(this, Repository);

                        i1++;
                    } while (i1 < this.m_NodeType.Count);
                }
            }
            else
            {

            }

            //  loading.Close();
        }
        #endregion

        #endregion

        #region Anforderungsmanagement

        public void Considerate_AFO_Connectoren(EA.Repository repository)
        {
            //Form Abfrage, was überprüft werden soll
            Forms.AFO_Mgmt.Auswahl_AFO_Mgmt_Connectoren auswahl_AFO_Mgmt = new Forms.AFO_Mgmt.Auswahl_AFO_Mgmt_Connectoren();

            auswahl_AFO_Mgmt.ShowDialog();

            if (auswahl_AFO_Mgmt.DialogResult == DialogResult.OK)
            {
                #region Ladebalken
                //Ladebalken initialisieren
                Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
                Checks.Check check = new Checks.Check(this, repository);
                Loading_OpArch load = new Loading_OpArch();
                load.label2.Text = "Überprüfung Konnektoren";
                load.label_Progress.Text = "Erhalten Anforderungen";

                load.progressBar1.Minimum = 0;
                load.progressBar1.Maximum = 2;
                load.progressBar1.Value = 0;
                load.progressBar1.Step = 1;
                load.Update();
                load.Show();
                #endregion

                #region Anforderungen vorbereiten
                if (auswahl_AFO_Mgmt.m_ret.Contains(true) == true)
                {
                    repository_Elements.Get_Requirements_WithConnector_Parallel(this, repository, -1);

                    //Nur zu exportierende Anforderungen betrachten

                    if (auswahl_AFO_Mgmt.modus == true)
                    {
                        this.m_Requirement = this.m_Requirement.Where(x => x.RPI_Export == true).ToList();
                    }


                    //Refines Konnektoren erhalten
                    if (auswahl_AFO_Mgmt.m_ret[0] == true || auswahl_AFO_Mgmt.m_ret[1] == true)
                    {
                        load.label_Progress.Text = "Erhalten Anforderungen Refines";
                        load.Update();

                        Parallel.ForEach(this.m_Requirement, requirement =>
                        {
                            requirement.Get_Connector_Refines(this, repository);

                        });
                    }
                    if (auswahl_AFO_Mgmt.m_ret[2] == true)
                    {
                        load.label_Progress.Text = "Erhalten Anforderungsart";
                        load.Update();

                        Parallel.ForEach(this.m_Requirement, requirement =>
                        {
                            requirement.Get_Anforderungsart(this);

                        });

                        load.label_Progress.Text = "Erhalten Anforderungen IsDuplicate";
                        load.Update();

                        Parallel.ForEach(this.m_Requirement, requirement =>
                        {
                            requirement.Get_Connector_Duplicate(this, repository);

                        });
                        
                        load.label_Progress.Text = "Erhalten Anforderungen Replaces";
                        load.Update();

                        Parallel.ForEach(this.m_Requirement, requirement =>
                        {
                            requirement.Get_Connector_Replaces(this, repository);

                        });
                        
                    }
                }

                #endregion


                #region Loops
                if (auswahl_AFO_Mgmt.m_ret[0] == true)
                {
                    load.label2.Text = "Überprüfung Konnektoren";
                    load.label_Progress.Text = "Verfeinerungs-Schleifen";

                    load.Update();

                    check.Check_Refines_Loops(this.m_Requirement);


                }
                #endregion

                #region Multiple Refines
                if (auswahl_AFO_Mgmt.m_ret[1] == true)
                {
                    load.label2.Text = "Überprüfung Konnektoren";
                    load.label_Progress.Text = "Mehrere, ausgehende Verfeinerungs-Konnektoren";

                    load.Update();

                    check.Check_Multiple_Refines(this.m_Requirement);
                }
                #endregion

                 #region Auflösung Dublette
                if (auswahl_AFO_Mgmt.m_ret[2] == true)
                {
                    load.label2.Text = "Überprüfung Konnektoren";
                    load.label_Progress.Text = "Auflösung Dubletten";

                    load.Update();

                    check.Check_Dubletten(this.m_Requirement);
                }
                #endregion

                load.Close();
            }


         

        }

        public void Start_SysArch(EA.Repository repository)
        {
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Loading_OpArch load = new Loading_OpArch();
            load.label2.Text = "Erhalten Systemelemente";
            load.label_Progress.Text = "Mapping Anforderungen Start AP 2";

            load.progressBar1.Minimum = 0;
            load.progressBar1.Maximum = 3;
            load.progressBar1.Value = 0;
            load.progressBar1.Step = 1;
            load.Refresh();
            load.Show();

            //Systemelemente erhalten
            repository_Elements.Get_Systemelemente_Parallel(this, repository, load);
            //Instnazen der Systemelement erhalten
            //Logical erhalten
            repository_Elements.Get_Logicals_Parallel(this);

            //Implements erhalten
            if (this.m_SysElemente.Count > 0)
            {
                int s1 = 0;
                do
                {
                    this.m_SysElemente[s1].Get_Implements(this, repository);

                    s1++;
                } while (s1 < this.m_SysElemente.Count);

                m_Decomposition_Sys = this.m_SysElemente.Where(x => x.m_Parent.Count == 0).ToList();

            }
            //Anforderungen erhalten der zu implenetierenden Anforderungen
            if (this.m_SysElemente.Count > 0)
            {
                int s1 = 0;
                do
                {
                    this.m_SysElemente[s1].Get_Requirements_Sys(this, repository);

                    s1++;
                } while (s1 < this.m_SysElemente.Count);

                m_Decomposition_Sys = this.m_SysElemente.Where(x => x.m_Parent.Count == 0).ToList();

            }
            //Neue Anforderungen erstellen, mappen und mit Klärungspunkt/Issue versehen
            if (this.m_SysElemente.Count > 0)
            {
                int s1 = 0;
                do
                {
                    if(this.m_SysElemente[s1].m_Implements.Count > 0)
                    {
                        this.m_SysElemente[s1].Copy_Requirements_Sys(repository, this);
                    }

                    s1++;
                } while (s1 < this.m_SysElemente.Count);

            }

            //Metamodell ändern
            this.metamodel.Create_NAFv4_Physical();


            load.Close();

        }

        public void Considerate_Replaces_Con(EA.Repository repository)
        {
            Requirement_Plugin.Forms.Auswahl_AFO_Mgmt_Replaces auswahl = new Requirement_Plugin.Forms.Auswahl_AFO_Mgmt_Replaces(this);

            auswahl.ShowDialog();

            if(auswahl.DialogResult == DialogResult.OK)
            {
                Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
                Loading_OpArch load = new Loading_OpArch();
                load.label2.Text = "Betrachtung Replaces";
                load.label_Progress.Text = "Erhalten Anforderungen";

                load.progressBar1.Minimum = 0;
                load.progressBar1.Maximum = 3;
                load.progressBar1.Value = 0;
                load.progressBar1.Step = 1;
                load.Refresh();
                load.Show();
                ///////////////////////////////////////////////////////////////////////////////
                //Anforderungen erhalten
                repository_Elements.Get_Requirements_WithConnector_Parallel(this, repository, 0);
                ///////////////////////////////////////////////////////////////////////////////
                //RPI == False bei Anforderungen setzten, welche Replaced werden
                load.label_Progress.Text = "Setzen RPI Export";
                load.progressBar1.PerformStep();
                load.progressBar1.Refresh();
                load.Refresh();
                load.Update();

                List<Requirement> m_req_replac = this.m_Requirement.Where(x => x.m_Requirement_Replace.Count > 0).ToList();

                if (m_req_replac.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        m_req_replac[i1].Update_TV_RPI_Export(this, repository);
                        m_req_replac[i1].m_Requirement_Replace.ForEach(x => x.RPI_Export = false);
                        m_req_replac[i1].m_Requirement_Replace.ForEach(x => x.Update_TV_RPI_Export(this, repository));

                        i1++;
                    } while (i1 < m_req_replac.Count);
                }
                ///////////////////////////////////////////////////////////////////////////////
                //Übernahme der Konnektoren DerivedFrom der ersetzten Anforderungen
                load.label_Progress.Text = "Übernahme Ableitung";
                load.progressBar1.Minimum = 0;
                load.progressBar1.Maximum = m_req_replac.Count;
                load.progressBar1.Value = 0;
                load.progressBar1.Step = 1;
                load.progressBar1.Refresh();
                load.Refresh();
                load.Update();


                    if (m_req_replac.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            m_req_replac[i1].Copy_Elements_Replace(this, repository, auswahl.m_bool);

                        load.progressBar1.PerformStep();
                        load.progressBar1.Refresh();

                        i1++;
                        } while (i1 < m_req_replac.Count);
                    }

                ////////////////////////
                load.Close();
            }

           
        }

        public void Start_Bewertung(EA.Repository repository)
        {
            #region GültigeElemente auf Diagram erhalten

            //Überprüfung offenes Diagram 
            EA.Diagram current_dia = repository.GetCurrentDiagram();
            List<Capability> m_cap = new List<Capability>();
            //Überprüfung RequirementCategory auf Diagram
            #region Category erhalten
            if (current_dia != null)
            {
                Diagram_Elements.Diagram_Elements diagram_Elements = new Diagram_Elements.Diagram_Elements(current_dia.DiagramID);
                List<string> m_help = diagram_Elements.Get_DiagramElements_guid(this, this.metamodel.m_Capability.Select(x => x.Type).ToList(), this.metamodel.m_Capability.Select(x => x.Stereotype).ToList());

                if(m_help.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if (m_cap.Select(x => x.Classifier_ID).ToList().Contains(m_help[i1]) == false)
                        {
                            Capability capability = new Capability(m_help[i1], repository, this);

                            m_cap.Add(capability);
                        }

                            i1++;
                    } while (i1 < m_help.Count);
                }
            }
            else
            {
                MessageBox.Show("Es ist kein Diagram geöffnet.");
            }
            #endregion
            #region Anforderungen erhalten
            //Anforderungen zu den RequirementCategory erhalten (RPI==True und AFO_WV_ANFORDERUNGSART == Anforderung, CPM_Phase == AP 1) 
            if (m_cap.Count > 0)
            {
                int i2 = 0;
                do
                {
                    //Jede Anforderung zu m_cap erhalten
                    m_cap[i2].Get_Requirements_Bewertung(this, m_cap);

                    i2++;
                } while (i2 < m_cap.Count);

  //              m_cap = m_cap.Where(x => x.m_Requirement.Count > 0).ToList();
            }
            else
            {
                MessageBox.Show("Im geöffneten Diagram befinden sich keine Elemente vom Stereotype "+this.metamodel.m_Capability[0].Type+".");
            }
            #endregion
            #endregion
            //Öffen Abfrage, weiteres vorgehen
            if(m_cap.Count > 0)
            {
                this.m_Capability = m_cap;
                //Prüfung Anforderungen, ob Gewicht vorhanden
                Requirement_Plugin.Domain.Bewertung_Afo.Bewertung bewertung = new Requirement_Plugin.Domain.Bewertung_Afo.Bewertung(m_cap);

                //Öffnen Form
                Requirement_Plugin.Forms.Bewertung_Afo.Bewertung_UI bewertung_UI = new Forms.Bewertung_Afo.Bewertung_UI(bewertung, this,repository);
                bewertung_UI.ShowDialog();

            }

        }

        public void Start_Begruendung(EA.Repository repository)
        {
            List<Activity> m_save_activity = this.m_Activity;

            Loading_OpArch loading_Op = new Loading_OpArch();
            loading_Op.label_Progress.Text = "Get Requirement...";
            loading_Op.Refresh();

            loading_Op.Show();

            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();

            //Alle Anforderungen erhalten, welche exportiert werden sollen --> TV RPI_Export == true
            List<Requirement> m_req =  repository_Elements.Get_Requirments_To_Export(this, repository, loading_Op);
            
            if(m_req.Count > 0 )
            {
                loading_Op.label_Progress.Text = "Check Requirements...";
                loading_Op.progressBar1.Maximum = m_req.Count;
                loading_Op.progressBar1.Minimum = 0;
                loading_Op.progressBar1.Step = 1;
                loading_Op.progressBar1.Value = 0;
                loading_Op.progressBar1.Update();
                loading_Op.Refresh();

                #region Varinate ein Constraint pro Forderung

                
                //Alle Actions zu einem Requirement erhalten mit zugehörigen ElternActivity
                this.m_Activity = new List<Activity>();
                List<List<Activity>> m_req_act = new List<List<Activity>>();
                int i1 = 0;
                do
                {
                    

                    List<Activity> m_ret = m_req[i1].Check_Begruendung(this, repository);

                    m_req_act.Add(m_ret);


                    loading_Op.progressBar1.PerformStep();
                    loading_Op.progressBar1.Refresh();
                    loading_Op.progressBar1.Update();
                    i1++;
                } while (i1 < m_req.Count);

                loading_Op.label_Progress.Text = "Create Constraints...";
                loading_Op.progressBar1.Maximum = m_req_act.Count;
                loading_Op.progressBar1.Minimum = 0;
                loading_Op.progressBar1.Step = 1;
                loading_Op.progressBar1.Value = 0;
                loading_Op.progressBar1.Update();

                loading_Op.Refresh();

                if (m_req_act.Count > 0)
                {

                    Repository_Connector repository_Connector = new Repository_Connector();
                    #region Packages
                    //Packages erzeugen
                    Repository_Element package = new Repository_Element();
                    //RPI
                    string guid = package.Create_Package_Model(this.metamodel.m_Package_Name[14], repository, this);
                    // EA.Package Parent_package = new EA.Package();
                    //var Parent_package = ;
                    //Katalog
                    guid = package.Create_Package(this.metamodel.m_Package_Name[20], repository.GetPackageByGuid(guid), repository, this);
                    // Parent_package = repository.GetPackageByGuid(guid);
                    //Constraint
                    string guid1 = package.Create_Package(this.metamodel.m_Package_Name[21], repository.GetPackageByGuid(guid), repository, this);
                    string guid2 = package.Create_Package(this.metamodel.m_Package_Name[22], repository.GetPackageByGuid(guid), repository, this);
                    string guid3 = package.Create_Package(this.metamodel.m_Package_Name[23], repository.GetPackageByGuid(guid), repository, this);
                    //Referenz
                    //Reference SPARX EA Model <<Modelname>> erzeugen
                    Repository_Class reference_model = new Repository_Class();
                    reference_model.Name = "EA Model <<Projektname>>";
                    reference_model.Notes = "Dies beschreibt die Architektur des Projekts <<Projektname>>";
                    string Refernce_type = this.metamodel.m_Reference[0].Type;
                    string Refernce_Stereotype = this.metamodel.m_Reference[0].Stereotype;
                    string Refernce_Toolbox = this.metamodel.m_Reference[0].Toolbox;
                    string ref_guid = reference_model.Create_Element_Class(reference_model.Name, Refernce_type, Refernce_Stereotype, Refernce_Toolbox, 0, guid2, repository, reference_model.Notes, this);
                    reference_model.Classifier_ID = ref_guid;
                    reference_model.ID = reference_model.Get_Object_ID(this);
                    #endregion
                    //constraint
                    string const_type = this.metamodel.m_Design_Constraint[0].Type;
                    string const_Stereotype = this.metamodel.m_Design_Constraint[0].Stereotype;
                    string const_Toolbox = this.metamodel.m_Design_Constraint[0].Toolbox;

                    var i2 = 0;
                    do
                    {
                        if(m_req_act[i2] == null)
                        {
                            //Keine Begründung und nicht aus Action ableitbar --> Issue erzeugen
                            Issue issue = new Issue(this, "Kein Constraint ableitbar", "Es ist automatisierbar kein Constraint ableitbar.", guid3, repository, true, "Klaerungspunkt");
                            repository_Connector.Create_Dependency(issue.Classifier_ID, m_req[i2].Classifier_ID, this.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), this.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), this.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, this, this.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0], this.metamodel.m_Con_Trace[0].direction);


                        }
                        else
                        {
                            if(m_req_act[i2].Count > 0)
                            {
                                //Erzeugung Constraint
                                m_req[i2].Name = m_req[i2].Get_Name(this);
                                OperationalConstraint operationalConstraint = new OperationalConstraint(null, null, null);
                                operationalConstraint.Get_Begruendung_Variante1(m_req_act[i2], m_req[i2]);
                                string const_guid = operationalConstraint.Create_Element_Class(operationalConstraint.Name, const_type, const_Stereotype, const_Toolbox, 0, guid1, repository, operationalConstraint.Notes, this);
                                operationalConstraint.Classifier_ID = const_guid;
                                //Verknüpfung constraint und Anforderung
                                repository_Connector.Create_Dependency(m_req[i2].Classifier_ID, operationalConstraint.Classifier_ID, this.metamodel.m_Derived_Logical.Select(x => x.Stereotype).ToList(), this.metamodel.m_Derived_Logical.Select(x => x.Type).ToList(), this.metamodel.m_Derived_Logical.Select(x => x.SubType).ToList()[0], repository, this, this.metamodel.m_Derived_Logical.Select(x => x.Toolbox).ToList()[0], this.metamodel.m_Derived_Logical[0].direction);
                                //Actions mit Constraint verknüpfen
                                m_req[i2].Create_SatisfyDesign_Begruendung_Variante1(operationalConstraint, this, repository);
                            }
                               

                            
                        }


                        i2++;
                    } while (i2 < m_req_act.Count);
                }


                #endregion

                #region Variante Mehrere constraints pro Forderung

                /*  this.m_Activity = new List<Activity>();
                  List<List<Activity>> m_req_act = new List<List<Activity>>();

                  int i1 = 0;
                  do
                  {
                     List<Activity> m_ret = m_req[i1].Check_Begruendung(this, repository);

                      m_req_act.Add(m_ret);

                      loading_Op.progressBar1.PerformStep();
                      loading_Op.progressBar1.Refresh();
                      loading_Op.progressBar1.Update();
                      i1++;
                  } while (i1 < m_req.Count);

                  List<Activity> m_Parent = this.m_Activity.Where(x => x.m_Child.Count > 0).ToList();

                  //Constraint anlegen mit Titel und Text für jede Activity mit Kinderelementen
                  if(m_Parent.Count> 0)
                  {
                      loading_Op.label_Progress.Text = "Create Constraints...";
                      loading_Op.progressBar1.Maximum = m_Parent.Count;
                      loading_Op.progressBar1.Minimum = 0;
                      loading_Op.progressBar1.Step = 1;
                      loading_Op.progressBar1.Value = 0;
                      loading_Op.progressBar1.Update();

                      loading_Op.Refresh();

                      #region Packages
                      //Packages erzeugen
                      Repository_Element package = new Repository_Element();
                      //RPI
                      string guid = package.Create_Package_Model(this.metamodel.m_Package_Name[14], repository, this);
                     // EA.Package Parent_package = new EA.Package();
                      //var Parent_package = ;
                      //Katalog
                      guid = package.Create_Package(this.metamodel.m_Package_Name[20], repository.GetPackageByGuid(guid), repository, this);
                     // Parent_package = repository.GetPackageByGuid(guid);
                      //Constraint
                      string guid1 = package.Create_Package(this.metamodel.m_Package_Name[21], repository.GetPackageByGuid(guid), repository, this);
                      string guid2 = package.Create_Package(this.metamodel.m_Package_Name[22], repository.GetPackageByGuid(guid), repository, this);
                      //Referenz
                      //Reference SPARX EA Model <<Modelname>> erzeugen
                      Repository_Class reference_model = new Repository_Class();
                      reference_model.Name = "EA Model <<Projektname>>";
                      reference_model.Notes = "Dies beschreibt die Architektur des Projekts <<Projektname>>";
                      string Refernce_type = this.metamodel.m_Reference[0].Type;
                      string Refernce_Stereotype = this.metamodel.m_Reference[0].Stereotype;
                      string Refernce_Toolbox = this.metamodel.m_Reference[0].Toolbox;
                      string ref_guid = reference_model.Create_Element_Class(reference_model.Name, Refernce_type, Refernce_Stereotype, Refernce_Toolbox, 0, guid2, repository, reference_model.Notes, this);
                      reference_model.Classifier_ID = ref_guid;
                      reference_model.ID = reference_model.Get_Object_ID(this);
                      #endregion
                      //constraint
                      string const_type = this.metamodel.m_Design_Constraint[0].Type;
                      string const_Stereotype = this.metamodel.m_Design_Constraint[0].Stereotype;
                      string const_Toolbox = this.metamodel.m_Design_Constraint[0].Toolbox;

                      int i2 = 0;
                      do
                      {
                          //Constraint erzeugen
                          OperationalConstraint operationalConstraint = new OperationalConstraint(null, null, null);
                          operationalConstraint.Get_Begruendung(m_Parent[i2]);
                          string const_guid = operationalConstraint.Create_Element_Class(operationalConstraint.Name, const_type, const_Stereotype, const_Toolbox, 0, guid1, repository, operationalConstraint.Notes, this);
                          operationalConstraint.Classifier_ID = const_guid;
                          operationalConstraint.ID = operationalConstraint.Get_Object_ID(this);
                          //Constraint mit Requirement verknüpfen
                          Repository_Connector repository_Connector = new Repository_Connector();
                          List<Requirement> m_req_recent = new List<Requirement>();
                          //Anforderungen erhalten, welche mit dem Constraint verknüpft werden
                          int i3 = 0;
                          do
                          {
                              if(m_req_act[i3].Where(x => x.m_Parent.Select(y => y.Classifier_ID).ToList().Contains(m_Parent[i2].Classifier_ID)).ToList().Count > 0)
                              {
                                  m_req_recent.Add(m_req[i3]);
                              }

                               i3++;
                          } while (i3 < m_req.Count);

                          if (m_req_recent.Count > 0)
                          {


                              int i4 = 0;
                              do
                              {
                                  //Anforderung und Constraint verküpfen
                                  repository_Connector.Create_Dependency(m_req_recent[i4].Classifier_ID, operationalConstraint.Classifier_ID, this.metamodel.m_Derived_Logical.Select(x => x.Stereotype).ToList(), this.metamodel.m_Derived_Logical.Select(x => x.Type).ToList(), this.metamodel.m_Derived_Logical.Select(x => x.SubType).ToList()[0], repository, this, this.metamodel.m_Derived_Logical.Select(x => x.Toolbox).ToList()[0]);

                                  //Actions mit Constraint verknüpfen
                                  m_req_recent[i4].Create_SatisfyDesign_Begruendung(operationalConstraint, m_Parent[i2], this, repository);
                                  //Taktisch Planerische Begruendung übernehmen
                                  //m_req_recent[i4].

                                  i4++;
                              } while (i4 < m_req_recent.Count);
                          }


                          //Als Reference EA Model <<Modelname>> verknüpfen
                          repository_Connector.Create_Dependency(operationalConstraint.Classifier_ID, reference_model.Classifier_ID, this.metamodel.m_JustifiedBy.Select(x => x.Stereotype).ToList(), this.metamodel.m_JustifiedBy.Select(x => x.Type).ToList(), this.metamodel.m_JustifiedBy.Select(x => x.SubType).ToList()[0], repository, this, this.metamodel.m_JustifiedBy.Select(x => x.Toolbox).ToList()[0]);

                          loading_Op.progressBar1.PerformStep();
                          loading_Op.progressBar1.Refresh();
                          loading_Op.progressBar1.Update();

                          i2++;
                      } while (i2 < m_Parent.Count);
                  }

                 */
                #endregion
            }

            this.m_Activity = m_save_activity;
              

            loading_Op.Close();
        }


        public void Transform_Requirements(EA.Repository repository, List<string> m_req_guid)
        {
            Loading_OpArch loading_Op = new Loading_OpArch();
            loading_Op.label_Progress.Text = "Get Requirement...";
            loading_Op.Refresh();

            loading_Op.Show();

            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
          
            this.m_Requirement = repository_Elements.Get_Requirements(this, repository, this.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), loading_Op, this.metamodel.m_Requirement.Select(x => x.Type).ToList());

            List<Requirement> m_req = new List<Requirement>();
            int i2 = 0;
            do
            {
                Requirement requirement = new Requirement(m_req_guid[i2], this.metamodel);
                requirement.ID = requirement.Get_Object_ID(this);
                //TV Technisch Funktionaler Leistungswert festlegen
                requirement.Get_Tagged_Values_From_Requirement(m_req_guid[i2], repository, this);

                m_req.Add(requirement);

                i2++;
            } while (i2 < m_req_guid.Count);
        }

        public void Export_TFL_Requirments(EA.Repository repository, string Catalogue_guid)
        {
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            //Funktionsbaum erhalten mit Konnektoren, ausgehend vom Katalogue
            repository_Elements.Get_Capability_Parallel(this, repository);
            Capability Catalogue = repository_Elements.Get_Capability_Catalogue(this, repository, Catalogue_guid);
            //PFK Anforderungen des Funktionsbaum erhalten
            List<Capability> m_Capability_Leaf = this.m_Capability.Where(x => x.m_Child.Count == 0).ToList();

            if(m_Capability_Leaf.Count > 0)
            {
                int i1 = 0;
                do
                {
                    m_Capability_Leaf[i1].Get_Requirements_Bewertung(this, this.m_Capability);

                    i1++;
                } while (i1 < m_Capability_Leaf.Count);
            }
            //Technisch Funktionale Leistungswerte der PFK Anforderungen erhalten
            if (m_Capability_Leaf.Count > 0)
            {
                int i1 = 0;
                do
                {
                   if(m_Capability_Leaf[i1].m_Requirement.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            m_Capability_Leaf[i1].m_Requirement[i2].Get_TF_Leistungswerte(this, repository);


                            i2++;
                        } while (i2 < m_Capability_Leaf[i1].m_Requirement.Count);
                    }

                    i1++;
                } while (i1 < m_Capability_Leaf.Count);
            }
            //Export nach Excel

            Requirement_Plugin.Export.Export_technisch_Funktionale_Leitungswerte export_tfl = new Export.Export_technisch_Funktionale_Leitungswerte();

            export_tfl.Export_TFL(Catalogue);
        }

        public void Export_Afo_Mapping(EA.Repository repository, string Catalogue_guid)
        {

            Loading_OpArch loading_OpArch = new Loading_OpArch();
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            ////////////////////////////////////////////////////////
            ///Systemelemente erhalten
            repository_Elements.Get_Systemelemente_reduced_Parallel(this, repository, loading_OpArch);
            ///Dekomposition erhalten
            if(this.m_SysElemente.Count > 0)
            {
                int i1 = 0;
                do
                {
                  //  this.m_SysElemente[i1].

                    this.m_SysElemente[i1].Get_Children_Part_Sys(this.m_SysElemente[i1].Classifier_ID, this, repository);

                    i1++;
                } while (i1 < (this.m_SysElemente.Count));
            }

            // MessageBox.Show("Count: " + this.m_SysElemente.Count);
            ////////////////////////////////////////////////////////
            //Funktionsbaum erhalten mit Konnektoren, ausgehend vom Katalogue
            
            repository_Elements.Get_Capability_Parallel(this, repository);
            List<Capability> m_Capability_Leaf = this.m_Capability.Where(x => x.m_Child.Count == 0).ToList();

            Capability Catalogue = repository_Elements.Get_Capability_Catalogue(this, repository, Catalogue_guid);

            if (m_Capability_Leaf.Count > 0)
            {
                int i1 = 0;
                do
                {
                    m_Capability_Leaf[i1].Get_Requirements_Bewertung(this, this.m_Capability);

                    i1++;
                } while (i1 < m_Capability_Leaf.Count);
            }
            //Zuorndung Afo und System erhalten
            if(this.m_Requirement.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.m_Requirement[i1].Get_SysElement_Instances(this, repository);

                    i1++;
                } while (i1 < this.m_Requirement.Count);
            }

            //Export nach Excel

            Requirement_Plugin.Export.Export_Afo_Mapping export_afomap = new Export.Export_Afo_Mapping();

            export_afomap.Export(Catalogue, this) ;

        }
        public void Archiv_Anforderungen(EA.Repository repository, Database database)
        {
            Repository_Element repository_Element = new Repository_Element();
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Loading_OpArch load = new Loading_OpArch();
            load.label2.Text = "Archivierung Anforderungen";
            load.label_Progress.Text = "Erhalten Anforderungen";

            load.progressBar1.Minimum = 0;
            load.progressBar1.Maximum = 3;
            load.progressBar1.Value = 0;
            load.progressBar1.Step = 1;
            load.Refresh();
            load.Show();
            ///////////////////////////////////////////////////////////////////////////////
            //Anforderungen erhalten
            repository_Elements.Get_Requirements_WithConnector_Parallel(this, repository, -1);
            ///////////////////////////////////////////////////////////////////////////////
            //Anforderungen archivieren
            load.label_Progress.Text = "Archivieren Anforderungen";
            load.progressBar1.PerformStep();
            load.progressBar1.Refresh();
            load.Refresh();
            load.Update();
            //Packages erzeugen
            //  string Package_guid = repository_Element.Create_Package_Model(database.metamodel.m_Package_Name[14],repository, database);
           
            string Package_guid = repository_Element.Create_Package(database.metamodel.m_Package_Name[14], repository.Models.GetAt(0), repository, database);
            string Package_guid_Archiv = repository_Element.Create_Package(database.metamodel.m_Package_Name[15],repository.GetPackageByGuid(Package_guid),  repository, database);
            repository.GetPackageByGuid(Package_guid).Packages.Refresh();
            repository.GetPackageByGuid(Package_guid).Update();
            repository.GetPackageByGuid(Package_guid_Archiv).Packages.Refresh();
            repository.GetPackageByGuid(Package_guid_Archiv).Update();

            


            #region Archivieren
            List < Requirement > m_req = database.m_Requirement.Where(x => x.RPI_Export == false).ToList();

            List<string> m_packages_name = new List<string>();
            List<string> m_packages_guid = new List<string>();

            if (m_req.Count > 0)
            {
                EA.Element recent_req;
                EA.Package recenet_package;

                int i1 = 0;
                do
                {
                    string stereo = m_req[i1].Get_Stereotype(database);
                    //Package anlegen
                    string Package_guid_recent = "";
                    int index_name = m_packages_name.IndexOf(stereo + " - Archiv");
                    if (index_name == -1)
                    {
                        Package_guid_recent = repository_Element.Create_Package(stereo + " - Archiv", repository.GetPackageByGuid(Package_guid_Archiv), repository, database);

                        m_packages_name.Add(stereo + " - Archiv");
                        m_packages_guid.Add(Package_guid_recent);
                    }
                    else
                    {
                        Package_guid_recent = m_packages_guid[index_name];
                    }

                    

                    recent_req = repository.GetElementByGuid(m_req[i1].Classifier_ID);
                    recenet_package = repository.GetPackageByGuid(Package_guid_recent);
                    recent_req.PackageID = recenet_package.PackageID;
                    recent_req.Update();
                    recenet_package.Update();

                    i1++;
                } while (i1 < m_req.Count);
            }

            repository.GetPackageByGuid(Package_guid_Archiv).Packages.Refresh();
            repository.GetPackageByGuid(Package_guid_Archiv).Update();
            #endregion Archivieren

            #region Überprüfung obsolete Überschriften
            List<Requirement> m_req_ueber = database.m_Requirement.Where(x => x.RPI_Export == true).Where(y => y.AFO_WV_ART == AFO_WV_ART.Überschrift_Einleitung).ToList();

            if (m_req_ueber.Count > 0)
            {
                //Obsolete Überschriften erhalten
                List<Requirement> m_ueber_archiv = new List<Requirement>();

                int i1 = 0;
                do
                {
                    bool help = m_req_ueber[i1].Check_Export_Child_rekursiv();

                    if (help == false)
                    {
                        m_ueber_archiv.Add(m_req_ueber[i1]);
                    }

                    i1++;
                } while (i1 < m_req_ueber.Count);

                //Obsolete Überschriften verschieben
                if (m_ueber_archiv.Count > 0)
                {
                    EA.Package recenet_package;
                    EA.Element recent_req;
                    string Package_guid_ueber = repository_Element.Create_Package("Überschriften - Archiv", repository.GetPackageByGuid(Package_guid_Archiv), repository, database);
                    recenet_package = repository.GetPackageByGuid(Package_guid_ueber);

                    i1 = 0;

                    do
                    {
                        recent_req = repository.GetElementByGuid(m_ueber_archiv[i1].Classifier_ID);

                        m_ueber_archiv[i1].RPI_Export = false;
                        m_ueber_archiv[i1].Update_TV_RPI_Export(database, repository);

                        recent_req.PackageID = recenet_package.PackageID;
                        recent_req.Update();
                        recenet_package.Update();

                        i1++;
                    } while (i1 < m_ueber_archiv.Count);
                }
            }
            #endregion

            #region Export anforderungen aus Archiv holen
            List<Requirement> m_req2 = database.m_Requirement.Where(x => x.RPI_Export == true).ToList();
            List<string> m_guid = m_req2.Select(x => x.Classifier_ID).ToList();
            //Anfoderungen Live im Archiv Ordner finden
            List<string> m_req_guid_live =  repository_Elements.Get_Requirements_Package(repository.GetPackageByGuid(Package_guid_Archiv).PackageID, repository, m_guid);

            if(m_req_guid_live.Count > 0)
            {
                string Package_guid_Live = repository_Element.Create_Package(database.metamodel.m_Package_Name[16], repository.GetPackageByGuid(Package_guid), repository, database);
                repository.GetPackageByGuid(Package_guid).Packages.Refresh();
                repository.GetPackageByGuid(Package_guid).Update();
                repository.GetPackageByGuid(Package_guid_Live).Packages.Refresh();
                repository.GetPackageByGuid(Package_guid_Live).Update();

                List<string> m_packages_name_live = new List<string>();
                List<string> m_packages_guid_live = new List<string>();

                EA.Element recent_req;
                EA.Package recenet_package;

                int i1 = 0;
                do
                {
                    List<Requirement> m_requirement = m_req2.Where(x => x.Classifier_ID == m_req_guid_live[i1]).ToList();

                    if(m_requirement.Count > 0)
                    {
                        string stereo = m_requirement[0].Get_Stereotype(database);
                        //Package anlegen
                        string Package_guid_recent = "";
                        int index_name = m_packages_name_live.IndexOf(stereo + " - Live");
                        if (index_name == -1)
                        {
                            Package_guid_recent = repository_Element.Create_Package(stereo + " - Live", repository.GetPackageByGuid(Package_guid_Live), repository, database);

                            m_packages_name_live.Add(stereo + " - Live");
                            m_packages_guid_live.Add(Package_guid_recent);
                        }
                        else
                        {
                            Package_guid_recent = m_packages_guid_live[index_name];
                        }



                        recent_req = repository.GetElementByGuid(m_requirement[0].Classifier_ID);
                        recenet_package = repository.GetPackageByGuid(Package_guid_recent);
                        recent_req.PackageID = recenet_package.PackageID;
                        recent_req.Update();
                        recenet_package.Update();
                    }

                   

                    i1++;
                } while (i1 < m_req_guid_live.Count);

                repository.GetPackageByGuid(Package_guid_Live).Packages.Refresh();
                repository.GetPackageByGuid(Package_guid_Live).Update();

            }

          
            #endregion



            load.Close();
        }
        #endregion

        #region Model Mgmt
        public void Model_Reduce(EA.Repository repository)
        {
            //Check: Package ausgewählt
            EA.ObjectType item_type = repository.GetTreeSelectedItemType();

            if(item_type == EA.ObjectType.otPackage)
            {
                Loading_OpArch load = new Loading_OpArch();
                load.label2.Text = "Reduzierung Root Nodes";
                load.label_Progress.Text = "Erhalten Packages";
                load.progressBar1.Minimum = 0;
                load.progressBar1.Maximum = 4;
                load.progressBar1.Value = 0;
                load.progressBar1.Step = 1;
                load.Refresh();
                load.Show();

                EA.Package pack = repository.GetTreeSelectedPackage();
                #region Packages
                //Get: Packages im Package erhalten
                Package model = new Package(this, pack.PackageGUID);

                this.m_Packages.Add(model);
                model.Get_Parent(true, this);
                model.Get_Children(this);
                #endregion

                #region Diagramm Packages
                //Get: Diagramme in den Packages erhalten
                List<int> m_package_id = this.m_Packages.Select(x => x.Package_ID).ToList();

                if(m_package_id.Count > 0)
                {
                    Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
                    List<string> m_dia_guid = repository_Elements.Get_Diagram_By_Package(this, m_package_id);

                    if(m_dia_guid.Count > 0)
                    {
                        m_dia_guid = m_dia_guid.Distinct().ToList();

                        List<string> m_dia = this.m_Diagram.Select(x => x.Classifier).ToList();

                        int i1 = 0;
                        do
                        {
                            if (m_dia.Where(x => x == m_dia_guid[i1]).ToList().Count == 0)
                            {
                                Requirement_Plugin.Repository_Elements.Diagram diagram = new Repository_Elements.Diagram(m_dia_guid[i1], this);
                                this.m_Diagram.Add(diagram);
                                m_dia.Add(m_dia_guid[i1]);
                            }
                            i1++;
                        } while (i1 < m_dia_guid.Count);
                    }

                }
                #endregion

                List<string> m_guid_ges = new List<string>();

                #region Diagrammelemente 
                //Get DiagramElemente erhalten
                load.label_Progress.Text = "Erhalten Diagramelemente";
                load.progressBar1.PerformStep();
                load.Refresh();
                load.Show();
                if (this.m_Diagram.Count > 0)
                {

                    List<string> m_guid_all = new List<string>();

                    /*   Parallel.ForEach(this.m_Diagram, diagram =>
                       {
                           Requirement_Plugin.Diagram_Elements.Diagram_Elements diagram_Elements = new Requirement_Plugin.Diagram_Elements.Diagram_Elements(diagram.Diagram_ID);

                           List<string> help = diagram_Elements.Get_DiagramElements_All(this);

                           if(help != null)
                           {
                               m_guid_all.AddRange(help);
                               m_guid_all = m_guid_all.Distinct().ToList();
                           }

                       });
                    */
                    int i1 = 0;
                    do
                    {
                        Requirement_Plugin.Diagram_Elements.Diagram_Elements diagram_Elements = new Requirement_Plugin.Diagram_Elements.Diagram_Elements(this.m_Diagram[i1].Diagram_ID);

                        List<string> help = diagram_Elements.Get_DiagramElements_All(this);

                        if (help != null)
                        {
                            m_guid_all.AddRange(help);
                            m_guid_all = m_guid_all.Distinct().ToList();
                        }

                        i1++;
                    } while (i1 < this.m_Diagram.Count);

                    //Kinderelemente Classifier erhalten

                    m_guid_all = m_guid_all.Distinct().ToList();
                    m_guid_ges = m_guid_all;
                }
                #endregion


                #region Diagramlinks 
                //Get: Diagramlinks, davon die ConveyedItems erhalten
                load.label_Progress.Text = "Erhalten Diagramlinks";
                load.progressBar1.PerformStep();
                load.Refresh();
                load.Show();
                if (this.m_Diagram.Count > 0)
                {
                    List<string> m_guid_conveyed_all = new List<string>();

                    Parallel.ForEach(this.m_Diagram, diagram =>
                    {
                        Requirement_Plugin.Diagram_Elements.Diagram_Links diagram_links = new Requirement_Plugin.Diagram_Elements.Diagram_Links(diagram.Diagram_ID);

                        List<string> help = diagram_links.Get_DiagramLinks_All(this);

                        if (help != null)
                        {
                            m_guid_conveyed_all.AddRange(help);
                        }
                    });

                    m_guid_conveyed_all = m_guid_conveyed_all.Distinct().ToList();

                    m_guid_ges.AddRange(m_guid_conveyed_all);
                }

                #endregion

                #region Transfer: Elements

                if (this.m_Diagram.Count > 0)
                {
                    load.label_Progress.Text = "Transfer Elemente";
                    load.progressBar1.PerformStep();
                    load.Refresh();
                    load.Show();
                    if (m_guid_ges.Count > 0)
                    {
                        //Alle Stereotypen erhalten, welche Type Class, Object, Activity, InformationItem haben
                        Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
                        Repository_Element repository_Element = new Repository_Element();
                        //Alle Element erhalten, welche nicht in den Packages liegen

                        //Hier muss m_guid_ges und Elemente, welche nicht im Package sind veglichen werden


                        List<string> guid_no_pack = repository_Elements.Get_Elements_NotPackage(m_guid_ges, m_package_id, this);

                        List<string> m_stereo = repository_Elements.Get_Stereotypes2(guid_no_pack, this);

                        string mk_extern_guid = repository_Element.Create_Package_Create("extern - Modelelementekatalog", pack, repository, this);
                        EA.Package mk_extern = repository.GetPackageByGuid(mk_extern_guid);

                        if (m_stereo.Count > 0)
                        {

                            List<string> m_pack_guid = new List<string>();
                            string mk_extern_guid2 = "";
                            //Packages anlegen
                            int i1 = 0;
                            do
                            {
                                mk_extern_guid2 = repository_Element.Create_Package_Create(m_stereo[i1], mk_extern, repository, this);
                                m_pack_guid.Add(mk_extern_guid2);
                                i1++;
                            } while (i1 < m_stereo.Count);

                            mk_extern.Update();
                            //Elemente pro Stereotype verschieben
                            int i2 = 0;
                            do
                            {
                                //Elemente mit Stereotype aus guid_no_pack erhalten
                                List<string> guid_stereo = repository_Elements.Get_Elements_GUID_Stereotype(guid_no_pack, m_stereo[i2], this);
                                //Einzelne Elemente verschieben
                                if(guid_stereo.Count > 0)
                                {
                                    Parallel.ForEach(guid_stereo, guid =>
                                    {
                                        EA.Element element = repository.GetElementByGuid(guid);
                                        element.PackageID = repository.GetPackageByGuid(m_pack_guid[i2]).PackageID;
                                        element.Update();
                                    });
                                        /*  int i3 = 0;
                                          do
                                          {
                                              EA.Element element;
                                              element = repository.GetElementByGuid(guid_stereo[i3]);
                                              element.PackageID = repository.GetPackageByGuid(m_pack_guid[i2]).PackageID;
                                              element.Update();

                                              i3++;
                                          } while (guid_stereo.Count > i3);
                                        */
                                    }

                                repository.GetPackageByGuid(m_pack_guid[i2]).Update();

                                i2++;
                            } while (i2 < m_stereo.Count);

                            repository.RefreshModelView(pack.PackageID);
                        }

                    }
                }

                    #endregion
                }
            else
            {
                MessageBox.Show("Es wurde keine Root Node ausgewählt."); 
            }

            

            //
        }
        #endregion



    }
}
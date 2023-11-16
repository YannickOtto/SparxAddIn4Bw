using System;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

using EA_Framework;
using Ennumerationen;
using Database_Connection;
using Metamodels;
using Checks;
using Requirements;
using Repsoitory_Elements;
using Forms;
using Requirement_Plugin.Interfaces;
using EA;
using System.ComponentModel;

namespace Requirement_Plugin

{
    public class Requirement_PluginClass : EAAddinBase
    {
        //Gültigkeit des Plugin --> wird unten definiert
        string valid_date_string = "";

        // define menu constants
        const string menuHeader = "-&Requirement_Plugin";
        //const string menu_SysArch = "-&SysArch";
        const string menu_leer = "&leer";
        const string menu_Optionen = "&Optionen";
        const string menu_OpArch = "-&Analyse";
        const string menu_Update = "-&Update";
        const string menu_Test = "&Test";
        const string menu_AFOManagement = "-&Anforderungsmanagement";
        const string menu_ModelManagement = "-&Modellmanagement";
        const string menu_ModelReduction= "&Reduzierung Root Nodes";
        const string menu_AFO_Schleifen_Refines = "&Überprüfung Schleifen";
        const string menu_AFO_Tranform_Leistungswert = "&Transfomation Technisch-Funktionale Leistungswerte";
        const string menu_AFO_Export_Leistungswert = "&Export Technisch-Funktionale Leistungswerte";
        const string menu_AFO_Export_AfoMapping= "&Export Mapping Anforderung";
        const string menu_AFO_Multiple_Refines = "&Überprüfung Konnektoren Anforderungen";
        const string menu_AFO_Replaces = "&Bearbeitung Konnektor 'Replaces'";
        const string menu_AFO_SysArch = "&Übergang Systemarchitektur";
        const string menu_AFO_Bewertung = "&Bewertung Anforderungen";
        const string menu_AFO_Begruendung = "&Begründung Anforderungen";
        const string menu_AFO_Archiv = "&Archivieren Anforderungen";
        const string menu_Update_Sys = "&Update SystemElemente";
        const string menu_Update_Nachweisart = "&Update Nachweisarten";
        const string menu_Create_DB = "&Analyse Database";
        const string menu_Refresh_DB = "&Refresh Database";
        const string menu_AFO_IF = "&Create Requirements Interfaces";
        const string menu_AFO_Func = "&Create Requirements Functional";
        const string menu_AFO_NonFunc = "&Create Requirements NonFunctional";
        const string menu_AFO_Auto = "&Create Requirements Automatik";
        const string menu_AFO_Update = "&Update Requirements Connectoren";
        const string menu_AFO_Dopplung = "&Check Requirements Dopplung";
        const string menu_Export_Req = "-&Export Analysed Elements";
        const string menu_Export_Req_xac = "&As xac-File (analysed)";
        const string menu_Export_Req_reqif = "&As reqif-File (analysed)";
        const string menu_Import_Req = "-&Import Elements";
        const string menu_Import_Req_xac = "&From xac-File";
        const string menu_Import_Req_reqif = "&From reqif-File";
        const string menu_Export_Req2 = "-&Export Elements";
        const string menu_Export_Req_xac2 = "&As xac-File";
        const string menu_Export_Req_reqif2 = "&As reqif-File";
        const string menu_Export_Req_PFK = "&As PFK xac-File";
        const string menu_Export_Req_LV = "&As LV xac-File";
        const string menu_Export_Req_LB = "&As LB xac-File";
        const string menu_Metamodel_sup = "-&Metamodel";
        const string menu_Metamodel = "&Choose Metamodel";
        const string menu__createMetamodel = "&Create Metamodel";
        const string menu__synchMetamodel = "&Synchronize Metamodel";
        const string menu_Requ = "&Edit Requirement";
        const string menu_Requ_Ex = "-&Considerate Elements";
        const string menu_Requ_Ex1 = "&Activate Export Elements";
        const string menu_Requ_Ex2 = "&Deactivate Export Elements";
        const string menu_Check = "-&Prüfung";
        const string menu_Check_Pattern = "&Check Pattern";
        const string menu_Check_Rquirements = "&Check Requirements";
        const string menu_Check_Nachweisart = "&Check Nachweisart";
        const string menu_Check_DBConnection = "&Chose Database-Connection";

        // remember if we have to say hello or goodbye
        private bool shouldWeSayHello = true;
        private bool created_DB = false;
        private bool choosed_mm = true;
        private bool flag_activated = true;
        private bool created_DB_Edit = false;
        private AFO_CPM_PHASE CPM_Phase = AFO_CPM_PHASE.Eins;
        private bool import_repo = false;

        Database Database_OpArch;
        Database Data_Edit;
        Metamodel metamodel = null;


        #region Connection Strings

        //= @"c:\Program Files(x86)\EA_Addins\Connection_strings.xml";
        //string repo_string = "C:/Program Files (x86)/EA_Addins/Connection_strings.xml";
        string repo_string = "C:/Users/11431085/AppData/Local/EA_Addins";
        #endregion

        private List<Database_Connection.Repository.Repository_ODBC> repository_ODBCs = new List<Database_Connection.Repository.Repository_ODBC>();

        private Database_Connection.Repository.Repository_ODBC recent_repo = null;

        public override bool EA_OnContextItemDoubleClicked(EA.Repository Repository, string GUID, EA.ObjectType ot)
        {
           // MessageBox.Show(ot.ToString()+" "+ choosed_mm.ToString());

            if (ot.ToString() == "otElement" && choosed_mm == true && flag_activated == true)
           {
                Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
              
                // EA.Element elem = new EA.Element();
                // elem = Repository.GetElementByGuid(GUID);
                List<string> m_GUID = new List<string>();
                m_GUID.Add(GUID);

                if (this.Database_OpArch == null)
                {
                    if(this.metamodel == null)
                    {
                        this.metamodel = new Metamodel();
                        this.metamodel.Create_NAFv4_Logical();
                    }

                    this.Data_Edit = new Database(this.metamodel, Repository, this);

                    this.Database_OpArch = this.Data_Edit;

                    
                }

                Requirement_Plugin.Interfaces.Interface_Collection interface_Collection = new Interfaces.Interface_Collection();
                interface_Collection.Open_Connection(this.Data_Edit);

                List<string> m_help = repository_Elements.Check_Element_t_object(this.Data_Edit, Data_Edit.metamodel.m_Requirement.Select(x => x.Type).ToList(), Data_Edit.metamodel.m_Requirement.Select(x => x.Stereotype).ToList(), m_GUID);

                if(m_help != null)
                {
                    this.Edit_Requirement(GUID, Repository);

                    interface_Collection.Close_Connection(this.Data_Edit);
                    return (true);
                }
                else
                {
                    interface_Collection.Close_Connection(this.Data_Edit);
                    return (false);
                }
                
           }
           else
           {
                return (false);
            }

           
        }
        ///
        /// Called Before EA starts to check Add-In Exists
        /// Nothing is done here.
        /// This operation needs to exists for the addin to work
        ///
        /// <param name="Repository" />the EA repository
        /// a string
        public override String  EA_Connect(EA.Repository Repository)
        {
            //Kann zum Debuggen genutzt werden wenn man den genauen Ablauf vorgibt
            
             //Öffnen Test Datei -- > Create Database --> Export xax
             /*
             EA.Repository repository = new EA.Repository();
             repository.OpenFile("C:\\Users\\Yannick\\Desktop\\test.eapx");
             Metamodel metamodel = new Metamodel();
             metamodel.Create_NAFv31();
             Database Data = new Database(metamodel);
             Data.Create_OpArch_Decomposition(repository, Data);
             this.Export_XAC(Data, repository);
             */

            //No special processing required.

            return "a string";
        }

        ///
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu

        public override object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {

            switch (MenuName)
            {
                // defines the top level menu option
                case "":
                    return menuHeader;
                // defines the submenu options
                case menuHeader:
                    string[] subMenus = { menu_Requ, menu_Optionen, menu_OpArch, menu_Update, menu_AFOManagement, menu_ModelManagement, menu_Import_Req, menu_Export_Req2, menu_Metamodel_sup, menu_Check, menu_Requ_Ex, menu_Check_DBConnection };
                    return subMenus;
                case menu_OpArch:
                    string[] subMenus_OpArch = {menu_Create_DB, menu_Refresh_DB, menu_AFO_IF, menu_AFO_Func, menu_AFO_NonFunc, menu_AFO_Auto, menu_AFO_Update, menu_AFO_Dopplung, menu_Export_Req };
                    return subMenus_OpArch;
                case menu_Update:
                    string[] subMenus_Update = { menu_Update_Sys, menu_Update_Nachweisart };
                    return subMenus_Update;
                case menu_AFOManagement:
                    string[] subMenus_AFOManagement = { menu_AFO_Multiple_Refines, menu_AFO_Replaces, menu_AFO_Bewertung, menu_AFO_Begruendung, menu_AFO_Tranform_Leistungswert, menu_AFO_SysArch, menu_AFO_Archiv };
                    return subMenus_AFOManagement;
                case menu_ModelManagement:
                    string[] subMenus_ModellManagement = { menu_ModelReduction, menu_Test };
                    return subMenus_ModellManagement;
                case menu_Export_Req:
                    string[] subMenus_Export = { menu_Export_Req_xac, menu_Export_Req_reqif};
                    return (subMenus_Export);
                case menu_Metamodel_sup:
                    string[] subMenus_SysArch = {  menu_Metamodel, menu__createMetamodel, menu__synchMetamodel };
                    return subMenus_SysArch;
                case menu_Import_Req:
                    string[] subMenus_Import = { menu_Import_Req_xac, menu_Import_Req_reqif };
                    return (subMenus_Import);
                case menu_Export_Req2:
                    string[] subMenus_Export2 = { menu_Export_Req_xac2, menu_Export_Req_reqif2, menu_AFO_Export_Leistungswert, menu_AFO_Export_AfoMapping, menu_Export_Req_PFK, menu_Export_Req_LV, menu_Export_Req_LB };
                    return (subMenus_Export2);
                case menu_Check:
                    string[] subMenus_Export3 = { menu_Check_Rquirements, menu_Check_Nachweisart, menu_Check_Pattern };
                    return (subMenus_Export3);
                case menu_Requ_Ex:
                   string[] subMenus_Export4 = { menu_Requ_Ex1, menu_Requ_Ex2};
                    return (subMenus_Export4);

            }

            return "";
        }

        ///
        /// returns true if a project is currently opened
        ///
        /// <param name="Repository" />the repository
        /// true if a project is opened in EA
        public override bool IsProjectOpen(EA.Repository Repository)
        {
            try
            {
                EA.Collection c = Repository.Models;

               

                //    this.Database_OpArch = new Database(this.metamodel, Repository, this);

                //   this.Data_Edit = new Database(this.metamodel, Repository, this);
                //  this.Database_OpArch.metamodel = this.metamodel;
                //this.Data_Edit.metamodel = this.metamodel;

              //  MessageBox.Show(Repository.ConnectionString);

                return true;
            }
            catch
            {
                return false;
            }
        }

        ///
        /// Called once Menu has been opened to see what menu items should active.
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the menu item
        /// <param name="IsEnabled" />boolean indicating whethe the menu item is enabled
        /// <param name="IsChecked" />boolean indicating whether the menu is checked
        public override void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {
            if (IsProjectOpen(Repository))
            {
                #region Metamodell
                if (this.metamodel == null)
                {
                    //StandardMetamodell festgelegt
                    Metamodel metamodel = new Metamodel();

                    //metamodel.Choose_Metamodel();
                    //metamodel.Create_NAFv31();
                    metamodel.Create_NAFv4_Logical();
                    metamodel.flag_Systemelemente = false;
                    this.choosed_mm = true;
                    this.metamodel = metamodel;
                    this.metamodel.Metamodel_name = "NAFv4 - Logical";
                    this.created_DB_Edit = false;

                    if (this.metamodel.Metamodel_name != "none")
                    {


                        this.choosed_mm = true;
                        this.Database_OpArch = new Database(this.metamodel, Repository, this);
                        this.Database_OpArch.metamodel = this.metamodel;
                        this.Data_Edit = new Database(this.metamodel, Repository, this);
                        this.Data_Edit.metamodel = this.metamodel;


                    }
                    else
                    {
                        this.choosed_mm = false;
                        this.metamodel = null;
                    }
                }
                #endregion

                #region Import Repo Daten

                if (this.recent_repo == null && import_repo == false)
                {
                    import_repo = true;

                    XML_Handler_Profil xML_Handler_Profil = new XML_Handler_Profil(null);

                    List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> m_help_rep = xML_Handler_Profil.Import_Repository(repo_string);

                    if(m_help_rep != null)
                    {
                        if (m_help_rep.Count > 0)
                        {
                            List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> m_help_recent = m_help_rep.Where(x => x.rootnode == Repository.Models.GetAt(0).Name).ToList();

                            repository_ODBCs = m_help_rep;

                            if (m_help_recent.Count == 1)
                            {
                                this.recent_repo = m_help_recent[0];

                                this.Database_OpArch.metamodel.dB_Type = DB_Type.MSDASQL;
                                this.Data_Edit.metamodel.dB_Type = DB_Type.MSDASQL;
                                if (this.Database_OpArch.oDBC_Interface == null)
                                {
                                    this.Database_OpArch.oDBC_Interface = new ODBC_Interface(Repository, this.Database_OpArch.Base);
                                }
                                if (this.Data_Edit.oDBC_Interface == null)
                                {
                                    this.Data_Edit.oDBC_Interface = new ODBC_Interface(Repository, this.Data_Edit.Base);
                                }
                                this.Data_Edit.oDBC_Interface.Set_Connection_String(m_help_recent[0].connection_string);
                                // this.Data_Edit.oDBC_Interface
                                // this.Data_Edit.oDBC_Interface
                                this.Data_Edit.oDBC_Interface.odbc_Open();
                                //this.Data_Edit.oDBC_Interface.dbConnection.Database = 
                                this.Database_OpArch.oDBC_Interface.Set_Connection_String(m_help_recent[0].connection_string);
                                this.Database_OpArch.oDBC_Interface.odbc_Open();

                            }
                            if (m_help_recent.Count > 1)
                            {
                                //Mehr als eine vorhanden, bitte händisch auswählen
                                MessageBox.Show("Es wurden in der Konfiguratonsdatei mehrere passende Repository's für die aktuelle Umgebung gefundenb. Bitte wählen Sie das richtige Repository manuell aus.");
                            }

                        }
                    }
                    

                }

                #endregion




                switch (ItemName)
                {
                    ///Erste Ebene immer aktiv
                    case menu_OpArch:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Test:
                        IsEnabled = true;
                        break;
                    case menu_Optionen:
                        IsEnabled = true;
                        break;
                    case menu_Update:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFOManagement:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFO_Replaces:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFO_Multiple_Refines:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFO_Bewertung:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFO_Begruendung:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFO_SysArch:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFO_Tranform_Leistungswert:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFO_Export_Leistungswert:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_AFO_Export_AfoMapping:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_ModelReduction:
                        IsEnabled = true;
                        break;
                    case menu_AFO_Archiv:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Update_Sys:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Update_Nachweisart:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Metamodel_sup:
                        IsEnabled = true;
                        break;
                    //Flat Export
                    case menu_Export_Req2:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Export_Req_xac2:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Export_Req_reqif2:
                        IsEnabled = false;
                        break;
                    case menu_Export_Req_PFK:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Export_Req_LV:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Export_Req_LB:
                        IsEnabled = choosed_mm;
                        break;
                    //Flat Import
                    case menu_Import_Req:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Import_Req_xac:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Import_Req_reqif:
                        IsEnabled = false;
                        break;
                    ///Ebene der OpArch entsprechend aktiv
                    case menu_Create_DB:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Refresh_DB:
                        IsEnabled = created_DB;
                        break;
                    case menu_AFO_IF:
                        IsEnabled = created_DB;
                        break;
                    case menu_AFO_Func:
                        IsEnabled = created_DB;
                        break;
                    case menu_AFO_NonFunc:
                        IsEnabled = created_DB;
                        break;
                    case menu_Export_Req:
                        // IsEnabled = created_DB;
                        IsEnabled = false;
                        break;
                    case menu_AFO_Auto:
                        IsEnabled = created_DB;
                        break;
                    case menu_AFO_Update:
                        IsEnabled = created_DB;
                        break;
                    case menu_AFO_Dopplung:
                        IsEnabled = created_DB;
                        break;
                    case menu_Export_Req_xac:
                        //IsEnabled = created_DB;
                        IsEnabled = false;
                        break;
                    case menu_Export_Req_reqif:
                        IsEnabled = false;
                        break;
                    case menu_Metamodel:
                        IsEnabled = true;
                        break;
                    case menu__createMetamodel:
                        IsEnabled = true;
                        break;
                    case menu__synchMetamodel:
                        IsEnabled = choosed_mm;
                        break;
                    /// SysArch aktuell als Platzhalter
                    case menu_leer:
                        IsEnabled = false;
                        break;
                    case menu_Requ:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Check:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Check_Rquirements:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Check_Nachweisart:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Check_Pattern:
                        IsEnabled = choosed_mm;
                        break;
                    case menu_Requ_Ex:
                         IsEnabled = choosed_mm;
                        break;
                    case menu_Requ_Ex1:
                        IsEnabled = choosed_mm;
                        /*EA.Collection m_col2 = Repository.GetTreeSelectedElements();
                        if (m_col2.Count > 0)
                        {
                            IsEnabled = choosed_mm;
                        }
                        else
                        {
                            IsEnabled = false;
                        }*/

                      

                        break;
                    case menu_Requ_Ex2:
                        IsEnabled = choosed_mm;
                        /*EA.Collection m_col3 = Repository.GetTreeSelectedElements();
                        if (m_col3.Count > 0)
                        {
                            IsEnabled = choosed_mm;
                        }
                        else
                        {
                            IsEnabled = false;
                        }*/
                        break;
                    case menu_Check_DBConnection:
                        IsEnabled = choosed_mm;
                        break;
                    // there shouldn't be any other, but just in case disable it.
                    default:
                        IsEnabled = false;
                        break;
                }
            }
            else
            {
                // If no open project, disable all menu options
                IsEnabled = false;
            }
        }

        ///
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the selected menu item
        public override void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            //Festgelegte Zeit 
            CultureInfo provider = CultureInfo.InvariantCulture;
            valid_date_string = "12/24/2099";
            DateTime valid_date = DateTime.ParseExact(valid_date_string, "d", provider);
            //Aktuelle Zeit
            DateTime recent_Time = DateTime.Now;

            if(recent_Time.CompareTo(valid_date) == -1)
            {
                switch (ItemName)
                {
                    case menu_Create_DB:

                        //MessageBox.Show("Test_Installer");

                       // Metamodel metamodel = new Metamodel();
                       // metamodel.Choose_Metamodel();

                        if (this.metamodel != null && flag_activated == true)
                        {

                            this.Create_Database(Repository, Database_OpArch);
                        }
                        break;
                    case menu_Update_Sys:
                        if (flag_activated == true)
                        {
                            this.Update_Sys(Repository);
                        }
    
                        break;
                    case menu_AFO_Replaces:
                        if (flag_activated == true)
                        {
                            this.Considerate_Replaces(Repository);
                        }
                        break;
                    case menu_AFO_Multiple_Refines:
                        if (flag_activated == true)
                        {
                            this.Considerate_Connectoren(Repository);
                        }
                        break;
                    case menu_AFO_SysArch:
                        if (flag_activated == true)
                        {
                            this.Start_SysArch(Repository);
                        }
                        break;
                    case menu_AFO_Tranform_Leistungswert:
                        if (flag_activated == true)
                        {
                            this.Transform_Leistungswerte(Repository);
                        }
                            break;
                    case menu_AFO_Export_Leistungswert:
                        if (flag_activated == true)
                        {
                            this.Export_Leistungswerte(Repository);
                        }
                            break;
                    case menu_AFO_Export_AfoMapping:
                        if (flag_activated == true)
                        {
                            this.Export_Afo_Mapping(Repository);
                        }
                        break;
                    case menu_AFO_Bewertung:
                        if (flag_activated == true)
                        {
                            this.Start_Bewertung(Repository);
                        }
                        break;
                    case menu_AFO_Begruendung:
                        if (flag_activated == true)
                        {
                            this.Start_Begruendung(Repository);
                        }
                        break;
                    case menu_AFO_Archiv:
                        if (flag_activated == true)
                        {
                            this.Archiv_Requirement(Repository);
                        }
                        break;
                    case menu_Update_Nachweisart:
                        if (flag_activated == true)
                        {
                            this.Update_Nachweisart(Repository);
                        }
                        break;
                    case menu_Test:
                        this.Test(Repository);
                        break;
                        
                    case menu_Refresh_DB:
                      //  this.Database_OpArch.test(Repository, this);
                        break;
                    case menu_AFO_IF:
                        if (flag_activated == true)
                        {
                            this.Create_AFO_IF(Repository, Database_OpArch);
                        }
                        break;
                    case menu_AFO_Func:
                        if (flag_activated == true)
                        {
                            this.Create_AFO_Func(Repository, Database_OpArch);
                        }
                        break;
                    case menu_AFO_NonFunc:
                        if (flag_activated == true)
                        {
                            this.Create_AFO_NonFunc(Repository, Database_OpArch);
                        }
                        break;
                    case menu_AFO_Auto:
                        if (flag_activated == true)
                        {
                            this.Create_AFO_Auto(Repository, Database_OpArch);
                        }
                        break;
                    case menu_AFO_Update:
                        if (flag_activated == true)
                        {
                            this.Update_Requirement_Connectoren(Database_OpArch, Repository);
                        }
                            break;
                    case menu_AFO_Dopplung:
                        if (flag_activated == true)
                        {
                            this.Check_Requirements_Dopplung(Repository);
                        }
                            break;
                    case menu_Export_Req_xac:
                        if (flag_activated == true)
                        {
                            this.Export_XAC(this.Database_OpArch, Repository);
                        }
                        break;
                    case menu_Import_Req_xac:
                        if (flag_activated == true)
                        {
                            this.Import_XAC(Repository);
                        }
                        break;
                    case menu_Export_Req_xac2:
                        if (flag_activated == true)
                        {
                            this.Export_Flat_XAC(Repository);
                        }
                        break;
                    case menu_Export_Req_PFK:
                        if (flag_activated == true)
                        {
                            this.Export_PFK(Repository);
                        }
                        break;
                    case menu_Export_Req_LV:
                        if (flag_activated == true)
                        {
                            this.Export_LV(Repository);
                        }
                        break;
                    case menu_Export_Req_LB: 
                     if (flag_activated == true)
                        {
                            this.Export_LB(Repository);
                        }
                        break;
                    case menu_Metamodel:
                        if (flag_activated == true)
                        {
                            this.Choose_Metamodel(Repository);
                        }
                        break;
                    case menu__createMetamodel:
                        if (flag_activated == true)
                        {
                            Create_Metamodel_Class create = new Create_Metamodel_Class();
                            if (this.Database_OpArch != null)
                            {
                                this.metamodel = create.Get_Metamodel(this.Database_OpArch.metamodel);
                            }
                            else
                            {
                                Metamodel metamodel = new Metamodel();
                                metamodel.Create_NAFv31();
                                this.metamodel = create.Get_Metamodel(metamodel);
                            }

                            this.Database_OpArch = new Database(this.metamodel, Repository, this);
                            this.Database_OpArch.metamodel = this.metamodel;
                            this.Data_Edit = new Database(this.metamodel, Repository, this);
                            this.Data_Edit.metamodel = this.metamodel;
                            this.choosed_mm = true;
                        }
                        break;
                    case menu__synchMetamodel:
                        if (flag_activated == true)
                        {
                            //Aktuelles Metamodell synchronisieren
                            Synch_MM rec = new Synch_MM(this.metamodel, Repository);
                            rec.ShowDialog();
                        }
                        break;
                    case menu_Requ:
                        if (flag_activated == true)
                        {
                            EA.Diagram current_dia = Repository.GetCurrentDiagram();


                            if (current_dia != null)
                            {
                                if (this.Data_Edit == null)
                                {
                                    if (this.Database_OpArch == null)
                                    {
                                        this.Data_Edit = new Database(this.metamodel, Repository, this);

                                        this.Database_OpArch = this.Data_Edit;
                                    }
                                    else
                                    {
                                        this.Data_Edit = this.Database_OpArch;
                                    }

                                }
                                else
                                {
                                    if (this.Database_OpArch == null)
                                    {
                                        this.Data_Edit = new Database(this.metamodel, Repository, this);

                                        this.Database_OpArch = this.Data_Edit;
                                    }
                                    else
                                    {
                                        this.Data_Edit = this.Database_OpArch;
                                    }
                                }

                                Requirement_Plugin.Interfaces.Interface_Collection interface_Collection = new Interfaces.Interface_Collection();
                                interface_Collection.Open_Connection(this.Data_Edit);
                                //  this.Data_Edit.oLEDB_Interface.OLEDB_Open();
                                EA.Collection selectedObjects = current_dia.SelectedObjects;
                                //Ein Object vom Type der definierten Requirements wurde gewählt
                                if (selectedObjects.Count == 1)
                                {
                                    Diagrams.Diagram_Element recent_dia_elem = new Diagrams.Diagram_Element(selectedObjects.GetAt(0).ElementID, this.Data_Edit);
                                    Element_Metamodel recent_elem = recent_dia_elem.GetElement_Metamodel(this.Data_Edit);

                                    if (this.metamodel.m_Requirement.Select(x => x.Type).ToList().Contains(recent_elem.Type) == true && this.metamodel.m_Requirement.Select(x => x.Stereotype).ToList().Contains(recent_elem.Stereotype) == true)
                                    {
                                        if (recent_dia_elem.Element_GUID != null)
                                        {
                                            //Hier nun Form zur Bearbeitung öffnen
                                            this.Edit_Requirement(recent_dia_elem.Element_GUID, Repository);
                                        }
                                    }
                                }

                            }
                        }
                        break;
                    case menu_Check_Rquirements:
                        if (flag_activated == true)
                        {
                            this.Check_Requirements(Repository);
                        }
                        break;
                    case menu_Check_Nachweisart:
                        if (flag_activated == true)
                        {
                            this.Check_Nachweisart(Repository);
                        }
                        break;
                    case menu_Check_Pattern:
                        if (flag_activated == true)
                        {
                            this.Check_Pattern(Repository);
                        }
                        break;
                    case menu_Optionen:
                        this.Choose_Optionen(Repository);
                        break;
                    case menu_Requ_Ex1:
                        if (flag_activated == true)
                        {
                            if (this.Database_OpArch == null)
                            {
                                this.Database_OpArch = new Database(this.metamodel, Repository, this);
                            }
                            this.Database_OpArch.oLEDB_Interface.OLEDB_Open();

                            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
                            var treeSelectedType = Repository.GetTreeSelectedItemType();
                            switch (treeSelectedType)
                            {
                                case EA.ObjectType.otElement:
                                    {
                                        EA.Collection m_col = Repository.GetTreeSelectedElements();

                                        repository_Elements.Change_Export_Status(m_col, true, this.Database_OpArch, Repository);

                                        break;
                                    }
                                case EA.ObjectType.otPackage:
                                    {
                                        // Code for when a package is selected
                                        EA.Package package = Repository.GetTreeSelectedPackage();
                                        repository_Elements.Change_Package_Export_Status(package, true, this.Database_OpArch, Repository);


                                        break;
                                    }
                            }





                            this.Database_OpArch.oLEDB_Interface.OLEDB_Close();
                        }
                        break;
                    case menu_Requ_Ex2:
                        if (flag_activated == true)
                        {
                            if (this.Database_OpArch == null)
                            {
                                this.Database_OpArch = new Database(this.metamodel, Repository, this);
                            }
                            this.Database_OpArch.oLEDB_Interface.OLEDB_Open();
                            EA.Collection m_col2 = Repository.GetTreeSelectedElements();
                            Repsoitory_Elements.Repository_Elements repository_Elements2 = new Repsoitory_Elements.Repository_Elements();

                            var treeSelectedType2 = Repository.GetTreeSelectedItemType();
                            switch (treeSelectedType2)
                            {
                                case EA.ObjectType.otElement:
                                    {
                                        EA.Collection m_col = Repository.GetTreeSelectedElements();

                                        repository_Elements2.Change_Export_Status(m_col, false, this.Database_OpArch, Repository);

                                        break;
                                    }
                                case EA.ObjectType.otPackage:
                                    {
                                        // Code for when a package is selected
                                        EA.Package package = Repository.GetTreeSelectedPackage();
                                        repository_Elements2.Change_Package_Export_Status(package, false, this.Database_OpArch, Repository);


                                        break;
                                    }
                            }
                            this.Database_OpArch.oLEDB_Interface.OLEDB_Close();
                        }
                        break;
                    case menu_Check_DBConnection:
                        if (flag_activated == true)
                        {
                            int index_repo = repository_ODBCs.FindIndex(x => x == recent_repo);

                            Requirement_Plugin.Forms.DB_Auswahl_2 dB_Auswahl = new Requirement_Plugin.Forms.DB_Auswahl_2(repository_ODBCs, index_repo, this.Database_OpArch, Repository);



                            if (this.Data_Edit != null)
                            {
                                Data_Edit = Database_OpArch;
                            }
                            else
                            {
                                this.Data_Edit = Database_OpArch;
                            }
                            dB_Auswahl.ShowDialog();

                            recent_repo = this.repository_ODBCs[dB_Auswahl.index_repositroy];
                            /*DB_Auswahl dB_Auswahl = new DB_Auswahl(Database_OpArch, Repository);
                            if(this.Data_Edit != null)
                            {
                                Data_Edit = Database_OpArch;
                            }
                            else
                            {
                                this.Data_Edit = Database_OpArch;
                            }
                            dB_Auswahl.ShowDialog();*/
                        }
                        
                        break;
                    case menu_ModelReduction:
                        this.Reduce_Model(Repository);
                        break;


                }
            }
            else
            {
                MessageBox.Show("Die Verwendungsdauer des Plugin ist abgelaufen. Wenden Sie sich für eine aktuelle Version an den Ersteller.");
            }


        }



        public override void EA_FileClose(EA.Repository Repository)
        {
            created_DB = false;
            choosed_mm = false;
            this.Database_OpArch = null;
            this.metamodel = null;

        }
        ///
        /// Say Hello to the world
        ///
        private void Reduce_Model(EA.Repository repository)
        {

            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();

            interface_Collection_OleDB.Open_Connection(this.Database_OpArch);
            //Start Reduzierung
            this.Database_OpArch.Model_Reduce(repository);

        }

        private void Choose_Optionen(EA.Repository repository)
        {
            

            Requirement_Plugin.Forms.Misc.Optionen_RPI optionen_RPI = new Forms.Misc.Optionen_RPI(this.flag_activated, this.metamodel.CPM_PHASE);

            optionen_RPI.ShowDialog();

            this.flag_activated = optionen_RPI.status;
            this.CPM_Phase = optionen_RPI.AFO_CPM;

            this.metamodel.CPM_PHASE = optionen_RPI.AFO_CPM;
            
        }

        private void Choose_Metamodel(EA.Repository repository)
        {
            Metamodel metamodel = new Metamodel();
           
            metamodel.Choose_Metamodel();

            this.metamodel = metamodel;
            this.created_DB_Edit = false;

            if (this.metamodel.Metamodel_name != "none")
            {
                this.choosed_mm = true;
                this.Database_OpArch = new Database(this.metamodel, repository, this);
                this.Database_OpArch.metamodel = this.metamodel;
                this.Data_Edit = new Database(this.metamodel, repository, this);
                this.Data_Edit.metamodel = this.metamodel;
            }
            else
            {
                this.choosed_mm = false;
                this.metamodel = null;
            }

            
        }

        private void Test(EA.Repository repository)
        {
           
            this.Database_OpArch.SQLITE_Interface.SQLITE_Open();


            #region Interface_element
            /*
            Interfaces.Interface_Element test = new Interfaces.Interface_Element();

           
            List<string> m_Type = new List<string>();
            m_Type.Add("Class");
            m_Type.Add("Class");
            List<string> m_Name = new List<string>();
            m_Name.Add("OperationalArchitecture1");
            m_Name.Add("OperationalPerformer1");
            m_Name.Add("OperationalPerformer2");
            List<string> m_Stereotype = new List<string>();
            m_Stereotype.Add("OperationalArchitecture");
            m_Stereotype.Add("OperationalPerformer");


            List<string> m_Type_Child = new List<string>();
            m_Type_Child.Add("Part");
            List<string> m_Stereotype_Child = new List<string>();
            m_Stereotype_Child.Add("OperationalRole");

            List<string> m_Type_Child_action = new List<string>();
            m_Type_Child_action.Add("Action");
            m_Type_Child_action.Add("Activity");
            List<string> m_Stereotype_Child_action = new List<string>();
            m_Stereotype_Child_action.Add("OperationalActivityAction");
            m_Stereotype_Child_action.Add("OperationalActivityAction");

            List<string> m_Type_Gen = new List<string>();
            m_Type_Gen.Add("Generalization");

            List<string> m_Stereotype_Gen = new List<string>();
            m_Stereotype_Gen.Add("PropertySetGeneralisation");
            //OA und OP erhalten
            #region SELECT

            List<int> m_test = test.Get_ID_By_Name(this.Database_OpArch, m_Type, m_Stereotype, m_Name);
            //OR erhalten
           // List<int> m_test2 = test.Get_ID_By_Name(this.Database_OpArch, m_Type, m_Stereotype, m_Name);

            string t2 = test.Get_One_Attribut_String("{FB4EB850-D54D-42da-A04A-9E0D6936E36C}", this.Database_OpArch, "Name");

            string t3 = test.Get_One_Attribut_String_by_ID(m_test[1], this.Database_OpArch, "Name");

            string t4 = test.Get_One_Attribut_String_by_Classifier("{14409494-C542-4c61-8871-C90546BE118F}", this.Database_OpArch, "Name");

            int t5 = test.Get_One_Attribut_Integer("{14409494-C542-4c61-8871-C90546BE118F}", this.Database_OpArch, "Package_ID", "t_object");

            string t6 = test.Get_Parent_Package_GUID("{BBB6C947-39F5-43ee-B4AA-A6D8EFE1686D}", this.Database_OpArch);

            string t7 = test.Get_Parent_GUID("{A63D939E-1925-4334-9C12-1D35A92C5584}", this.Database_OpArch);

            List<string> t8 = test.Get_Children("{FC7CA6D6-FF7F-46c9-8741-39B5FB0C80D9}", this.Database_OpArch, m_Type_Child_action, m_Stereotype_Child_action);

            List<string> t9 = test.Get_Children_Element("{FB4EB850-D54D-42da-A04A-9E0D6936E36C}", this.Database_OpArch, m_Type_Child, m_Stereotype_Child);

            List<string> t10 = test.Get_Children_Package("{33FCF192-B2AC-42fe-AB6A-B4D67067C567}", this.Database_OpArch);

            List<string> t11 = test.Get_Parent_Taxonomy(this.Database_OpArch, "{14409494-C542-4c61-8871-C90546BE118F}", m_Type_Gen, m_Stereotype_Gen, m_Type, m_Stereotype);

            List<string> t_12 = test.Get_Children_Taxonomy(this.Database_OpArch, "{4835566B-A1D4-49c6-A426-400E170C3475}", m_Type_Gen, m_Stereotype_Gen, m_Type, m_Stereotype);

            List<string> t_13 = test.Get_Children_By_Classifier(this.Database_OpArch, "{14409494-C542-4c61-8871-C90546BE118F}", "{FB4EB850-D54D-42da-A04A-9E0D6936E36C}");

            string t_14 = test.Get_Classifier_Activity(this.Database_OpArch, "{6B412F6A-BBA7-4b1c-AD90-AD706C32D068}");

            List<string> t_15 = test.Get_Instanzen_Activity(this.Database_OpArch, "{9C2B3E27-7FAF-40e6-BEE2-B67B889A2C42}");

            #endregion SELECT

            #region Check

            string t_161 = test.Check_Database_Element_Class(this.Database_OpArch, "Part", "OperationalRole", "test", -1);
            string t_162 = test.Check_Database_Element_Class(this.Database_OpArch, "Part", null, "test", -1);
            string t_163 = test.Check_Database_Element_Class(this.Database_OpArch, "Part", "OperationalRole", "test", test.Get_One_Attribut_Integer("{FB4EB850-D54D-42da-A04A-9E0D6936E36C}", this.Database_OpArch, "Object_ID", "t_object"));
            string t_164 = test.Check_Database_Element_Class(this.Database_OpArch, "Part", null, "test", test.Get_One_Attribut_Integer("{FB4EB850-D54D-42da-A04A-9E0D6936E36C}", this.Database_OpArch, "Object_ID", "t_object"));

            string t_171 = test.Check_Database_Element_Instantiate(this.Database_OpArch, "Part", "OperationalRole", -1, "{14409494-C542-4c61-8871-C90546BE118F}");
            string t_172 = test.Check_Database_Element_Instantiate(this.Database_OpArch, "Part", null, -1, "{14409494-C542-4c61-8871-C90546BE118F}");
            string t_173 = test.Check_Database_Element_Instantiate(this.Database_OpArch, "Part", "OperationalRole", test.Get_One_Attribut_Integer("{FB4EB850-D54D-42da-A04A-9E0D6936E36C}", this.Database_OpArch, "Object_ID", "t_object"), "{14409494-C542-4c61-8871-C90546BE118F}");
            string t_174 = test.Check_Database_Element_Instantiate(this.Database_OpArch, "Part", null, test.Get_One_Attribut_Integer("{FB4EB850-D54D-42da-A04A-9E0D6936E36C}", this.Database_OpArch, "Object_ID", "t_object"), "{14409494-C542-4c61-8871-C90546BE118F}");

            List<string> t_181 = test.Check_Database_Element_Classifier(this.Database_OpArch, m_Type_Child_action, m_Stereotype_Child_action, -1, "{8F32B4F1-A34A-4746-888B-59DA1865C4FF}");
            List<string> t_182 = test.Check_Database_Element_Classifier(this.Database_OpArch, m_Type_Child_action, null, -1, "{8F32B4F1-A34A-4746-888B-59DA1865C4FF}");
            List<string> t_183 = test.Check_Database_Element_Classifier(this.Database_OpArch, m_Type_Child_action, m_Stereotype_Child_action, test.Get_One_Attribut_Integer("{FC7CA6D6-FF7F-46c9-8741-39B5FB0C80D9}", this.Database_OpArch, "Object_ID", "t_object"), "{8F32B4F1-A34A-4746-888B-59DA1865C4FF}");
            List<string> t_184 = test.Check_Database_Element_Classifier(this.Database_OpArch, m_Type_Child_action, null, test.Get_One_Attribut_Integer("{FC7CA6D6-FF7F-46c9-8741-39B5FB0C80D9}", this.Database_OpArch, "Object_ID", "t_object"), "{8F32B4F1-A34A-4746-888B-59DA1865C4FF}");

            #endregion

            #region Update

            //t_19 //
            test.Update_BigInt("{9C2B3E27-7FAF-40e6-BEE2-B67B889A2C42}", test.Get_One_Attribut_Integer("{FB4EB850-D54D-42da-A04A-9E0D6936E36C}", this.Database_OpArch, "Package_ID", "t_object"), "Package_ID", this.Database_OpArch);
            //t20//
            test.Update_VarChar("{9C2B3E27-7FAF-40e6-BEE2-B67B889A2C42}", "test5", "Name", this.Database_OpArch);
            //t21//
            
            test.Update_Stereotype_Xref("{9C2B3E27-7FAF-40e6-BEE2-B67B889A2C42}", "OperationalActivity", "NAFv4-ADMBw", this.Database_OpArch);
           
            EA.Element Elem = repository.GetElementByGuid("{9C2B3E27-7FAF-40e6-BEE2-B67B889A2C42}");
            Elem.Stereotype = "OperationalActivity";
            Elem.Update();
            Elem.Refresh();
            repository.RefreshModelView(0);
            #endregion
            */
            #endregion Interface_element

            #region Interface_Glossar
            /* Interfaces.Interface_Glossar interface_Glossar = new Interfaces.Interface_Glossar();

             List<string> t22 = interface_Glossar.Get_Glosar_ID(this.Database_OpArch);

             string t23 = interface_Glossar.Get_Glossar_Property("Term", "1", this.Database_OpArch);
            */
            #endregion Interface_Glossar

            #region Interface_Package

            /* 
             Interfaces.Interface_Package interface_Package = new Interfaces.Interface_Package();

             int t23 = interface_Package.Get_One_Attribut_Integer("{1CD7CB06-9186-46d8-A20F-38EA9900AD3A}", this.Database_OpArch, "Package_ID");

             string t24 = interface_Package.Get_One_Attribut_String_by_ID(4, this.Database_OpArch, "Name");

             string t25 = interface_Package.Create_Package_Model(this.Database_OpArch, repository, "Test66");

             string t26 = interface_Package.Create_Package(this.Database_OpArch, repository, repository.GetPackageByGuid(t25), "test77", false);
            */
            #endregion Interface_Package

            #region Interface_TaggedValues
            /*  Interfaces.Interface_TaggedValue interface_TaggedValue = new Interfaces.Interface_TaggedValue();

              string t27 = interface_TaggedValue.Get_Tagged_Value_By_Property(this.Database_OpArch, "test" , "{9C2B3E27-7FAF-40e6-BEE2-B67B889A2C42}");
              string t28 = interface_TaggedValue.Get_Tagged_Value_By_Property(this.Database_OpArch, "test2", "{9C2B3E27-7FAF-40e6-BEE2-B67B889A2C42}");
              int t29 = interface_TaggedValue.Get_ID(this.Database_OpArch, "test", "test2");

              List<string> t30 = interface_TaggedValue.Get_Distinct_Property(this.Database_OpArch, "test", 0);
              List<string> t301 = interface_TaggedValue.Get_Distinct_Property(this.Database_OpArch, "test2", 0);

              List<TV_Map> m_TV = new List<TV_Map>();
              TV_Map n1 = new TV_Map("test", System.Data.OleDb.OleDbType.VarChar, "test", "test");
              TV_Map n2 = new TV_Map("test2", System.Data.OleDb.OleDbType.VarChar, "test2", "test2");

              m_TV.Add(n1);
              m_TV.Add(n2);



              Interfaces.Interface_Element test = new Interfaces.Interface_Element();
              int tt = test.Get_One_Attribut_Integer("{C3D55127-3646-4015-A508-A560C86DFB62}", this.Database_OpArch, "Object_ID", "t_object");
              List<DB_Return> t31 = interface_TaggedValue.Get_Tagged_Value(this.Database_OpArch.metamodel.AFO_Tagged_Values, tt, this.Database_OpArch);

              List<DB_Insert> m_Insert = new List<DB_Insert>();
              DB_Insert ins = new DB_Insert("test3", System.Data.OleDb.OleDbType.VarChar, System.Data.Odbc.OdbcType.VarChar, "test4", -1);

              m_Insert.Add(ins);
              TaggedValue taggedValue = new TaggedValue(this.Database_OpArch.metamodel, this.Database_OpArch);
              int ttt = test.Get_One_Attribut_Integer("{78194A89-991E-4a5b-9E34-05B558805C3B}", this.Database_OpArch, "Object_ID", "t_object");
              string[] m_Input_Property = { "Object_ID", "Property", "Value", "Notes", "ea_guid" };
              interface_TaggedValue.Insert_Tagged_Value(this.Database_OpArch, m_Insert, taggedValue, ttt, m_Input_Property);

              List<DB_Insert> m_Insert2 = new List<DB_Insert>();
              DB_Insert ins2 = new DB_Insert("test3", System.Data.OleDb.OleDbType.VarChar, System.Data.Odbc.OdbcType.VarChar, "test5", -1);
              m_Insert.Add(ins2);

              interface_TaggedValue.Update_Tagged_Value(m_Insert, this.Database_OpArch, ttt);

              EA.Element Elem = repository.GetElementByGuid("{78194A89-991E-4a5b-9E34-05B558805C3B}");
              Elem.Stereotype = "ResourceArtifact";
              Elem.Update();
              Elem.Refresh();
              interface_TaggedValue.Set_Stereotype("{78194A89-991E-4a5b-9E34-05B558805C3B}", "NAFv4-ADMBw", "ResourceArtifact", this.Database_OpArch);
              repository.RefreshModelView(0);
            */

            #endregion Interface_TaggedValues

            #region Interface_Collection

            /* Interfaces.Interface_Collection interface_Collection = new Interfaces.Interface_Collection();

             List<string> m_Type = new List<string>();
             m_Type.Add("Class");
             m_Type.Add("Class");
             List<string> m_Name = new List<string>();
             m_Name.Add("OperationalArchitecture1");
             m_Name.Add("OperationalPerformer1");
             m_Name.Add("OperationalPerformer2");
             List<string> m_Stereotype = new List<string>();
             m_Stereotype.Add("OperationalArchitecture");
             m_Stereotype.Add("OperationalPerformer");

             this.Database_OpArch.metamodel.flag_Analyse_Diagram = true;
             List<string> t34 = interface_Collection.Get_Elements_GUID(this.Database_OpArch, m_Type, m_Stereotype);
             this.Database_OpArch.metamodel.flag_Analyse_Diagram = false;
             List<string> t341 = interface_Collection.Get_Elements_GUID(this.Database_OpArch, m_Type, m_Stereotype);
             this.Database_OpArch.metamodel.flag_Analyse_Diagram = true;
             List<string> t35 = interface_Collection.Get_Elements_By_Type_GUID(this.Database_OpArch, m_Type);
             this.Database_OpArch.metamodel.flag_Analyse_Diagram = false;
             List<string> t351 = interface_Collection.Get_Elements_By_Type_GUID(this.Database_OpArch, m_Type);

             List<string> m_guid = new List<string>();
             m_guid.Add("{14409494-C542-4c61-8871-C90546BE118F}");
             m_guid.Add("{FB4EB850-D54D-42da-A04A-9E0D6936E36C}");
             List<string> t36 = interface_Collection.Get_Stereotype_By_Type_AND_GUID(this.Database_OpArch, m_guid);
             List<string> t37 = interface_Collection.Get_GUID_By_Stereotype_AND_GUID(this.Database_OpArch, m_guid, m_Stereotype[1]);
             List<int> m_id = new List<int>();
             m_id.Add(1);
             List<string> t38 = interface_Collection.Get_By_NOT_PackageID(this.Database_OpArch, m_guid, m_id);
             List<int> m_dia_id = new List<int>();
             m_dia_id.Add(1);
             List<string> t39 = interface_Collection.Get_DiagramElements_GUID(this.Database_OpArch, m_Type, m_Stereotype, m_dia_id[0]);
             List<string> t40 = interface_Collection.Get_DiagramElements_GUID_wo_Stereotype(this.Database_OpArch, m_Type, m_dia_id[0]);
             List<string> t41 = interface_Collection.Get_DiagramLinks_ConveyedItems(this.Database_OpArch, m_dia_id);

             Interfaces.Interface_Package interface_Package = new Interfaces.Interface_Package();

             int t23 = interface_Package.Get_One_Attribut_Integer("{BBB6C947-39F5-43ee-B4AA-A6D8EFE1686D}", this.Database_OpArch, "Package_ID");

             List<string> t42 = interface_Collection.Get_Elements_By_Package(this.Database_OpArch, 3, m_Type, m_Stereotype);
             List<string> t422= interface_Collection.Get_Elements_By_Package(this.Database_OpArch, 4, m_Type, m_Stereotype);
             List<string> t423 = interface_Collection.Get_Elements_By_Package(this.Database_OpArch, 5, m_Type, m_Stereotype);
             List<string> t424 = interface_Collection.Get_Elements_By_Package(this.Database_OpArch, 6, m_Type, m_Stereotype);
             List<string> t425 = interface_Collection.Get_Elements_By_Package(this.Database_OpArch, t23, m_Type, m_Stereotype);

             this.Database_OpArch.metamodel.flag_Analyse_Diagram = true;
             List<int> t43 = interface_Collection.Get_Elements_ID(this.Database_OpArch, m_Type, m_Stereotype);
             this.Database_OpArch.metamodel.flag_Analyse_Diagram = false;
             List<int> t431 = interface_Collection.Get_Elements_ID(this.Database_OpArch, m_Type, m_Stereotype);
             List<string> t44 = interface_Collection.Get_m_Attribut_By_m_Attribut(this.Database_OpArch, m_Type, "Object_Type", "Name", "t_object");
             List<string> t45 = interface_Collection.Get_m_Attribut_By_m_Attribut_Integer(this.Database_OpArch, m_id, "Object_ID", "Name", "t_object");
             List<string> t46 = interface_Collection.Check_Element(this.Database_OpArch, m_guid, m_Type, m_Stereotype);
            */
            #endregion Interface_Collection

            #region Interface_Req
            /*
            Interfaces.Interface_Collection_Requirements interface_Collection_Requirements = new Interfaces.Interface_Collection_Requirements();

            List<string> m_Type_req = new List<string>();
            m_Type_req.Add("Requirement");
            List<string> m_Stereotype_req = new List<string>();
            m_Stereotype_req = this.Database_OpArch.metamodel.m_Requirement.Select(x => x.Stereotype).ToList();
            List<int> m_dia_id = new List<int>();
            m_dia_id.Add(1);



            List<string> t47 = interface_Collection_Requirements.Get_Elements_GUID(this.Database_OpArch, m_Type_req, m_Stereotype_req);
            List<string> t481 = interface_Collection_Requirements.Get_DiagramElements_GUID(this.Database_OpArch, m_Type_req, m_Stereotype_req, 2);
            List<string> t482 = interface_Collection_Requirements.Get_DiagramElements_GUID(this.Database_OpArch, m_Type_req, m_Stereotype_req, 3);
            List<string> t483 = interface_Collection_Requirements.Get_DiagramElements_GUID(this.Database_OpArch, m_Type_req, m_Stereotype_req, 4);
            List<string> t484 = interface_Collection_Requirements.Get_DiagramElements_GUID(this.Database_OpArch, m_Type_req, m_Stereotype_req, 5);
            List<string> t485 = interface_Collection_Requirements.Get_DiagramElements_GUID(this.Database_OpArch, m_Type_req, m_Stereotype_req, 6);
            List<string> t486 = interface_Collection_Requirements.Get_DiagramElements_GUID(this.Database_OpArch, m_Type_req, m_Stereotype_req, 7);

            Interfaces.Interface_Package interface_Package = new Interfaces.Interface_Package();
            int package_id = interface_Package.Get_One_Attribut_Integer("{3992436B-F02C-4c59-A1CA-91FE5C96C340}", this.Database_OpArch, "Package_ID");

            List<string> t49 = interface_Collection_Requirements.Get_Elements_By_Package(this.Database_OpArch, package_id, m_Type_req, m_Stereotype_req) ;
            this.Database_OpArch.metamodel.flag_Analyse_Diagram = true;
            List<int> t50 = interface_Collection_Requirements.Get_Elements_ID(this.Database_OpArch, m_Type_req, m_Stereotype_req);
            this.Database_OpArch.metamodel.flag_Analyse_Diagram = false;
            List<int> t501 = interface_Collection_Requirements.Get_Elements_ID(this.Database_OpArch, m_Type_req, m_Stereotype_req);
            */
            #endregion Interface_Req

            #region Interface_Connector

            Interfaces.Interface_Connectors interface_Connectors = new Interface_Connectors();

            /*   List<string> m_Type_Con = this.Database_OpArch.metamodel.m_Infoaus.Select(x => x.Type).ToList();
               List<string> m_Stereotype_Con = this.Database_OpArch.metamodel.m_Infoaus.Select(x => x.Stereotype).ToList();

               List<string> m_Type_Client = this.Database_OpArch.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
               List<string> m_Stereotype_Client = this.Database_OpArch.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

               List<string> m_Type_Suplier = this.Database_OpArch.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
               List<string> m_Stereotype_Supplier = this.Database_OpArch.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();


               List<string> t51 = interface_Connectors.Get_Connector_By_Type_and_Stereotype(this.Database_OpArch, m_Type_Con, m_Stereotype_Con, m_Type_Client, m_Stereotype_Client, m_Type_Suplier, m_Stereotype_Supplier);
               List<string> t52 = interface_Connectors.Get_Connetor_By_Elements(this.Database_OpArch, m_Type_Client, m_Stereotype_Client, m_Type_Suplier, m_Stereotype_Supplier, m_Type_Con, m_Stereotype_Con);

               List<string> m_Client_GUID = new List<string>();
               m_Client_GUID.Add("{14409494-C542-4c61-8871-C90546BE118F}");

               List<string> t53 = interface_Connectors.Get_Connector_By_m_Client_GUID(this.Database_OpArch, m_Client_GUID, m_Type_Con, m_Stereotype_Con);

               List<string> t54 = interface_Connectors.Get_Connector_From_Client_Property(this.Database_OpArch, "ea_guid", m_Type_Client, m_Stereotype_Client, m_Client_GUID[0], m_Type_Suplier, m_Stereotype_Supplier, m_Type_Con, m_Stereotype_Con);

               List<string> m_Supplier_GUID = new List<string>();
               m_Supplier_GUID.Add("{DB0B0F1B-1C28-4c28-B17B-731DE3F93504}");

               List<string> t55 = interface_Connectors.Get_Connector_From_Supplier_Property(this.Database_OpArch, "ea_guid", m_Type_Suplier, m_Stereotype_Supplier, m_Supplier_GUID[0], m_Type_Client, m_Stereotype_Client, m_Type_Con, m_Stereotype_Con);

               List<string> t56 = interface_Connectors.Get_Connector_By_PropertyType(this.Database_OpArch, m_Client_GUID[0], m_Supplier_GUID[0], m_Type_Con, m_Stereotype_Con);

               Interfaces.Interface_Element test = new Interface_Element();
               int tt5 = test.Get_One_Attribut_Integer("{DB0B0F1B-1C28-4c28-B17B-731DE3F93504}", this.Database_OpArch, "Object_ID", "t_object");

               List<DB_Return> t57 = interface_Connectors.Get_m_Client_From_Supplier(this.Database_OpArch, tt5, m_Type_Con, m_Stereotype_Con);

               List<DB_Return> t58 = interface_Connectors.Get_m_Supplier_From_Client(this.Database_OpArch, tt5, m_Type_Con, m_Stereotype_Con);

               List<string> t59 = interface_Connectors.GetInformationElements(this.Database_OpArch, "{64131C73-5839-488c-AD97-FF1C57CB4EB6}");

               List<string> t60 = interface_Connectors.Get_Connector_By_Supplier_GUID(this.Database_OpArch, "{64131C73-5839-488c-AD97-FF1C57CB4EB6}", "{DB0B0F1B-1C28-4c28-B17B-731DE3F93504}", m_Type_Con, m_Stereotype_Con);

               Repository_Element repository_Element = new Repository_Element();
               repository_Element.Classifier_ID = "{B543A743-F4BF-44ae-8B14-0E5799F6ABB5}";
               repository_Element.ID = repository_Element.Get_Object_ID(this.Database_OpArch);

               List<string> t61 = interface_Connectors.Get_Connector_By_Client_ID(this.Database_OpArch, repository_Element.ID);



               List<string> t62 = interface_Connectors.Get_Supplier_Element_By_Connector(this.Database_OpArch, m_Client_GUID, m_Type_Suplier, m_Stereotype_Supplier, m_Type_Con, m_Stereotype_Con);

               List<string> m_Client_GUID2 = new List<string>();
               m_Client_GUID2.Add("{B543A743-F4BF-44ae-8B14-0E5799F6ABB5}");

               List<string> t63 = interface_Connectors.Get_Client_Element_By_Connector(this.Database_OpArch, m_Client_GUID2, m_Type_Client, m_Stereotype_Client, m_Type_Con, m_Stereotype_Con);

               List<string> t64 = interface_Connectors.Get_Supplier_By_Connecror_And_Supplier(this.Database_OpArch, "{C18445D5-78D9-4db6-9BE0-F62D7E8438C0}");

               string t65 = interface_Connectors.Get_Supplier_GUID(this.Database_OpArch, "{64131C73-5839-488c-AD97-FF1C57CB4EB6}", m_Type_Suplier, m_Stereotype_Supplier);

               string t66 = interface_Connectors.Get_Supplier_Classifier(this.Database_OpArch, "PDATA1", "{E4742C58-3B87-4904-B57A-5EAAA6846235}");

               List<string> t67 = interface_Connectors.Get_m_Supplier_By_ClientGUID_And_Connector(this.Database_OpArch, "{14409494-C542-4c61-8871-C90546BE118F}", m_Type_Con, m_Stereotype_Con);

               string t68 = interface_Connectors.Get_Client_GUID(this.Database_OpArch, "{64131C73-5839-488c-AD97-FF1C57CB4EB6}", m_Type_Client, m_Stereotype_Client);

               List<int> t69 = interface_Connectors.Get_Supplier_ID_By_Client_ID(this.Database_OpArch, repository_Element.ID, m_Stereotype_Con);

               List<string> t70 = interface_Connectors.Get_Connector_By_Client_GUID(this.Database_OpArch, "{14409494-C542-4c61-8871-C90546BE118F}", m_Type_Suplier, m_Stereotype_Supplier, m_Type_Con, m_Stereotype_Con);

               List<string> t71 = interface_Connectors.Get_Connector_By_Client_GUID_And_Supplier_Type(this.Database_OpArch, t68, m_Type_Suplier, m_Stereotype_Supplier, m_Type_Con, m_Stereotype_Con);

               List<string> m_con_type = this.Database_OpArch.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
               List<string> m_con_stereotype = this.Database_OpArch.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

               List<string> t72 = interface_Connectors.Check_Connector("{597C01CD-29D0-4c97-A434-E78758C4810F}", "{14409494-C542-4c61-8871-C90546BE118F}", m_con_type, m_con_stereotype, this.Database_OpArch);


               List<string> m_Type_con = new List<string>();
               m_Type_con.Add("Dependency");
               List<string> m_Stereotype_con = new List<string>();
               m_Stereotype_con.Add("trace");
               List<string> m_Toolbox_con = new List<string>();
               List<string> t73 = interface_Connectors.Check_ProxyConnector_Supplier("{64131C73-5839-488c-AD97-FF1C57CB4EB6}", "{72DC0C95-38BC-4701-9677-910687C15C98}", m_Type_con, m_Stereotype_con, this.Database_OpArch );


               string t74 = interface_Connectors.Create_Connector("{72DC0C95-38BC-4701-9677-910687C15C98}", "{B543A743-F4BF-44ae-8B14-0E5799F6ABB5}", this.Database_OpArch.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), this.Database_OpArch.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), this.Database_OpArch.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, this.Database_OpArch, this.Database_OpArch.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0]);

               string t75 = interface_Connectors.Create_ProxyConnector_Supplier("{64131C73-5839-488c-AD97-FF1C57CB4EB6}", "{B543A743-F4BF-44ae-8B14-0E5799F6ABB5}", this.Database_OpArch.metamodel.m_Con_Trace.Select(x => x.Stereotype).ToList(), this.Database_OpArch.metamodel.m_Con_Trace.Select(x => x.Type).ToList(), this.Database_OpArch.metamodel.m_Con_Trace.Select(x => x.SubType).ToList()[0], repository, this.Database_OpArch, this.Database_OpArch.metamodel.m_Con_Trace.Select(x => x.Toolbox).ToList()[0]);

               interface_Connectors.Delete_Connector(this.Database_OpArch, t74, repository);

               interface_Connectors.Delete_Connector_by_IDs(this.Database_OpArch, repository, "{1A2B9019-38DE-4441-B6FB-D491FF7D63B7}", "{DB0B0F1B-1C28-4c28-B17B-731DE3F93504}", m_Stereotype_Con, m_Type_Con);

               interface_Connectors.Update_Connector(this.Database_OpArch, m_Type_Con, m_Stereotype_Con, "{1A2B9019-38DE-4441-B6FB-D491FF7D63B7}", "{B0AFBB01-C159-4281-820B-DA668BC857DB}");
            */
            #endregion Interface_Connector

            #region Interface Connectors Requirement

            Interfaces.Interface_Connectors_Requirement interface_Connectors_Requirement = new Interface_Connectors_Requirement();

            /*   List<string> m_client = new List<string>();
               m_client.Add("{14409494-C542-4c61-8871-C90546BE118F}");

               List<string> m_Type_supp = this.Database_OpArch.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
               List<string> m_Stereotype_supp = this.Database_OpArch.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

               List<string> m_Type_con = this.Database_OpArch.metamodel.m_Infoaus.Select(x => x.Type).ToList();
               List<string> m_Stereotype_con = this.Database_OpArch.metamodel.m_Infoaus.Select(x => x.Stereotype).ToList();

               List<string> t79 = interface_Connectors_Requirement.Get_Supplier_Element_By_Connector(this.Database_OpArch, m_client, m_Type_supp, m_Stereotype_supp.ToList(), m_Type_con, m_Stereotype_con);


               List<string> m_supplier = new List<string>();
               m_supplier.Add("{14409494-C542-4c61-8871-C90546BE118F}");
               List<string> m_Type_ADMBw = this.Database_OpArch.metamodel.m_Requirement_ADMBw.Select(x => x.Type).ToList();
               List<string> m_Stereotype_ADMBw = this.Database_OpArch.metamodel.m_Requirement_ADMBw.Select(x => x.Stereotype).ToList();

               List<string> m_Type_Derived = this.Database_OpArch.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
               List<string> m_Stereotype_Derived = this.Database_OpArch.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

               List<string> t80 = interface_Connectors_Requirement.Get_Client_Element_By_Connector(this.Database_OpArch, m_supplier, m_Type_ADMBw, m_Stereotype_ADMBw, m_Type_Derived, m_Stereotype_Derived);

               List<string> t81 = interface_Connectors_Requirement.Get_m_Supplier_By_ClientGUID_And_Connector(this.Database_OpArch, "{597C01CD-29D0-4c97-A434-E78758C4810F}", m_Type_Derived, m_Stereotype_Derived);

               List<string> m_Type_reqint2 = this.Database_OpArch.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList();
               List<string> m_Stereotype_reqint2 = this.Database_OpArch.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList();

               List<string> m_Type_DerivedElem2 = this.Database_OpArch.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
               List<string> m_Stereotype_DerivedElem2 = this.Database_OpArch.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

               List<string> m_Type_Con2 = this.Database_OpArch.metamodel.m_Afo_Requires.Select(x => x.Type).ToList();
               List<string> m_Stereotype_Con2 = this.Database_OpArch.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList();

               List<string> t82 = interface_Connectors_Requirement.Check_Connector_Requirement_Interface(this.Database_OpArch, "{A63D939E-1925-4334-9C12-1D35A92C5584}", "{29D3F5E4-1522-486c-A5BA-B98925EC342E}", m_Type_reqint2, m_Stereotype_reqint2, m_Type_DerivedElem2, m_Stereotype_DerivedElem2, m_Type_Con2, m_Stereotype_Con2);
              */
            #endregion Interface Connectors Requirement

            #region Interface Constraint

         /*   Interfaces.Interface_Constraint interface_Constraint = new Interface_Constraint();

            List<string> m_Type_elem = this.Database_OpArch.metamodel.m_Design_Constraint.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem = this.Database_OpArch.metamodel.m_Design_Constraint.Select(x => x.Stereotype).ToList();
            List<string> m_Type_con = this.Database_OpArch.metamodel.m_Satisfy_Design.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con = this.Database_OpArch.metamodel.m_Satisfy_Design.Select(x => x.Stereotype).ToList();

            List<string> m_Type_elem_def = this.Database_OpArch.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_def = this.Database_OpArch.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();
            List<string> m_Type_elem_usage = this.Database_OpArch.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_usage = this.Database_OpArch.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

            List<string> m_Type_elem_res = this.Database_OpArch.metamodel.m_Elements_SysArch_Definition.Select(x => x.Type).ToList();
            List<string> m_Stereotype_elem_res = this.Database_OpArch.metamodel.m_Elements_SysArch_Definition.Select(x => x.Stereotype).ToList();

            List<string> m_Type_con2 = this.Database_OpArch.metamodel.m_Implements.Select(x => x.Type).ToList();
            List<string> m_Stereotype_con2 = this.Database_OpArch.metamodel.m_Implements.Select(x => x.Stereotype).ToList();

            List<string> t83 = interface_Constraint.Get_Constraint(W_Constraint_Type.Design, this.Database_OpArch, -1, "{A63D939E-1925-4334-9C12-1D35A92C5584}", m_Type_elem, m_Stereotype_elem, m_Type_con, m_Stereotype_con, m_Stereotype_elem_def, m_Stereotype_elem_usage);


            List<string> t84 = interface_Constraint.Get_Typvertreter(this.Database_OpArch, -1, "{A63D939E-1925-4334-9C12-1D35A92C5584}", m_Type_elem_res, m_Stereotype_elem_res, m_Type_con2, m_Stereotype_con2);
         */
            #endregion Interface constraint


            this.Database_OpArch.SQLITE_Interface.SQLITE_Close();

        }
        /// <summary>
        /// Es wird eine Datenbank für das aktuelle Projekt erzeugt
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Database"></param>
        //
        private void Create_Database(EA.Repository Repository, Database Database)
        {
         //   Database.m_NodeType = null;

            Choose_Analyse_Patterns Patterns_Form = new Choose_Analyse_Patterns(Database.metamodel);
            Patterns_Form.ShowDialog();

            bool open = true;

            switch(Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    Database.oLEDB_Interface.OLEDB_Open();
                    break;
                case DB_Type.MSDASQL:
                    Database.oDBC_Interface.odbc_Open();
                    break;
                case DB_Type.SQLITE:
                    Database.SQLITE_Interface.SQLITE_Open();
                    break;
                default:
                    open = false;
                    break;
            }
           
         
            if(open == true)
            {
                this.Database_OpArch.Create_OpArch_Decomposition(Repository, Database_OpArch, this);
                this.created_DB = true;
            }
            else
            {
                MessageBox.Show("Es wurde kein Datenbankformat festgelgt. Es kann keine fehlerfrei Datenverbindung hergestellt werden.");
            }
           
        }

        private void Create_AFO_IF(EA.Repository Repository, Database Database_OpArch)
        {
            //   Requirement_Interface form = new Requirement_Interface(Database_OpArch, Repository);
            //  form.Show();
            Database_OpArch.flag_modus = false;

            //Auswauhl ob Manuell oder Automatisch
           // Choose_Interface_Modus Choose_Modus = new Choose_Interface_Modus(Database_OpArch);
           // Choose_Modus.ShowDialog();

           // MessageBox.Show(Database_OpArch.flag_modus.ToString());

            if(Database_OpArch.flag_modus == false)
            {
                ///Anzeigen
                Interface_Decomposition form_interface = new Interface_Decomposition(Database_OpArch, Repository);

                form_interface.ShowDialog();
            }

            //Automatik wurde in einen anderen Bereich verschoben
        /*   else
            {
                //Automatisch
                Loading_OpArch loading = new Loading_OpArch();
                loading.label_Progress.Text = "Anforderungserstellung Automatik";
                loading.label2.Text = " ";
                loading.progressBar1.Minimum = 0;
                loading.progressBar1.Maximum = Database_OpArch.m_NodeType.Count;
                loading.Show();

                Database_OpArch.Create_Requirements_Interface_AFO_Auto(Repository,  loading);

                
            }*/
            
            Repository.RefreshModelView(0);

        }

        private void Create_AFO_Func(EA.Repository Repository, Database Database_OpArch)
        {
            Functional_Decomposition form_functional = new Functional_Decomposition(Database_OpArch, Repository);

            form_functional.ShowDialog();

            Repository.RefreshModelView(0);
        }

        private void Create_AFO_NonFunc(EA.Repository Repository, Database Database_OpArch)
        {
            NonFunctional_Decomposition form_functional = new NonFunctional_Decomposition(Database_OpArch, Repository);

            form_functional.ShowDialog();

            Repository.RefreshModelView(0);
        }
    
        private void Create_AFO_Auto(EA.Repository Repository, Database Database_OpArch)
        {
            //Abfrage welche Typen Auto erstellt werden sollen
            Choose_Analyse_Patterns choose_Analyse_Patterns = new Choose_Analyse_Patterns(Database_OpArch.metamodel);

            choose_Analyse_Patterns.Set_Uberschrift("Anforderungstypen Automatik Modus");
            choose_Analyse_Patterns.Set_Text_TextBox_1("Wählen Sie die Anforderungtypen aus, welche automatisch mit Default Anforderungen befüllt werden sollen.");


            choose_Analyse_Patterns.ShowDialog();

            if (choose_Analyse_Patterns.DialogResult == DialogResult.OK)
            {

                Forms.Choose_Systemelement  choose_SysElem = new Forms.Choose_Systemelement(Database_OpArch);
                choose_SysElem.ShowDialog();

                if(choose_SysElem.DialogResult == DialogResult.OK)
                {
                    Loading_OpArch loading = new Loading_OpArch();
                    loading.label_Progress.Text = "Anforderungserstellung Automatik";
                    loading.label2.Text = " ";
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = Database_OpArch.m_NodeType.Count;
                    loading.Show();

                    List<NodeType> m_Nodetype_create = new List<NodeType>();
                    m_Nodetype_create = choose_SysElem.m_nodeTypes;

                    if(m_Nodetype_create != null)
                    {

                        //Interface
                        if (this.Database_OpArch.metamodel.m_Pattern_flag[0] == true)
                        {
                            this.Database_OpArch.Create_Requirements_Interface_AFO_Auto(Repository, loading, m_Nodetype_create);
                        }
                        //Functional
                        if (this.Database_OpArch.metamodel.m_Pattern_flag[1] == true)
                        {
                            this.Database_OpArch.Create_Requirements_Functional_AFO_Auto(Repository, loading, m_Nodetype_create);
                        }
                        //Design
                        if (this.Database_OpArch.metamodel.m_Pattern_flag[2] == true)
                        {
                            this.Database_OpArch.Create_Requirements_Design_AFO_Auto(Repository, loading, m_Nodetype_create);
                        }
                        //Process
                        if (this.Database_OpArch.metamodel.m_Pattern_flag[3] == true)
                        {
                            this.Database_OpArch.Create_Requirements_Process_AFO_Auto(Repository, loading, m_Nodetype_create);
                        }
                        //Umwelt
                        if (this.Database_OpArch.metamodel.m_Pattern_flag[4] == true)
                        {
                            this.Database_OpArch.Create_Requirements_Umwelt_AFO_Auto(Repository, loading, m_Nodetype_create);
                        }
                        //Typvertreter
                        if (this.Database_OpArch.metamodel.m_Pattern_flag[5] == true)
                        {
                            this.Database_OpArch.Create_Requirements_Typvertreter_AFO_Auto(Repository, loading, m_Nodetype_create);
                        }
                        //Qualität Class
                        if (this.Database_OpArch.metamodel.m_Pattern_flag[6] == true)
                        {
                            this.Database_OpArch.Create_Requirements_QualityClass_AFO_Auto(Repository, loading, m_Nodetype_create);
                        }
                        //Qualität Actvivity
                        if (this.Database_OpArch.metamodel.m_Pattern_flag[7] == true)
                        {
                            this.Database_OpArch.Create_Requirements_QualityActivity_AFO_Auto(Repository, loading, m_Nodetype_create);
                        }
                    }



                    loading.Close();
                }


               

            }

           
           

        }

        private void Update_Requirement_Connectoren(Database Data, EA.Repository repository)
        {
            //Abfrage welche Typen Auto erstellt werden sollen
            Choose_Analyse_Patterns choose_Analyse_Patterns = new Choose_Analyse_Patterns(Database_OpArch.metamodel);

            choose_Analyse_Patterns.Set_Uberschrift("Anforderungstypen Connectoren Update");
            choose_Analyse_Patterns.Set_Text_TextBox_1("Wählen Sie die Anforderungtypen aus, deren Connectoren geupdatet werden sollen.");


            choose_Analyse_Patterns.ShowDialog();

            if (choose_Analyse_Patterns.DialogResult == DialogResult.OK)
            {
                Loading_OpArch loading = new Loading_OpArch();
                loading.label_Progress.Text = "Update Konnektoren";
                loading.label2.Text = " ";
                loading.progressBar1.Minimum = 0;
                loading.progressBar1.Maximum = Data.m_NodeType.Count;
                loading.Show();

                //Interface
                if (this.Database_OpArch.metamodel.m_Pattern_flag[0] == true)
                {
                    Data.Update_Connectoren_Requirments_Interfaces(repository, loading);
                }
                //Functional
                if (this.Database_OpArch.metamodel.m_Pattern_flag[1] == true)
                {
                    Data.Update_Connectoren_Requirments_Functional(repository, loading);
                }
                //Design
                if (this.Database_OpArch.metamodel.m_Pattern_flag[2] == true)
                {
                    Data.Update_Connectoren_Requirments_Design(repository, loading);
                }
                //Process
                if (this.Database_OpArch.metamodel.m_Pattern_flag[3] == true)
                {
                    Data.Update_Connectoren_Requirments_Process(repository, loading);
                }
                //Umwelt
                if (this.Database_OpArch.metamodel.m_Pattern_flag[4] == true)
                {
                    Data.Update_Connectoren_Requirments_Umwelt(repository, loading);
                }
                //Typvertreter
                if (this.Database_OpArch.metamodel.m_Pattern_flag[5] == true)
                {
                    Data.Update_Connectoren_Requirments_Typverteter(repository, loading);
                }
                //QualityClass
                if (this.Database_OpArch.metamodel.m_Pattern_flag[6] == true)
                {
                    Data.Update_Connectoren_Requirments_QualityClass(repository, loading);
                }
                //QualityActivity
                if (this.Database_OpArch.metamodel.m_Pattern_flag[7] == true)
                {
                    Data.Update_Connectoren_Requirments_Typverteter(repository, loading);
                }

                loading.Close();

            }
        }
        private void Export_XAC(Database Data, EA.Repository repository)
        {
            Export_xml xml_Export = new Export_xml();
            //Dialog: Wo soll exportiert werden?
            string filename = Choose_Savespace();
            xml_Export.Export_xml_xac(Data, repository, filename);
        }

        private void Export_Flat_XAC(EA.Repository repository)
        {

           

            ////////////////////////////////////
            //Dialog: Wo soll exportiert werden?
            string filename = Choose_Savespace();
            /////////////////////////////////
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_oleDB = new Requirement_Plugin.Interfaces.Interface_Collection();
            //Metamodel festlegen
           // Metamodel metamodel = new Metamodel();
           // metamodel.Choose_Metamodel();
            Database Data = new Database(this.metamodel, repository, this);
            interface_Collection_oleDB.Open_Connection(Data);

            Choose_Require7_xac export_form = new Choose_Require7_xac(Data);
            export_form.checkBox_Szenar_Aufloesung.Checked = false;
            export_form.checkBox_Szenar_Aufloesung.Enabled = false;
            export_form.checkBox_Szenar_Aufloesung.Visible = false;
            export_form.checkBox_Sysele.Visible = true;
            export_form.checkBox_Sysele.Enabled = true;
            export_form.checkBox_Szenar_Aufloesung.Update();

            export_form.Refresh();
            export_form.ShowDialog();
            //  try
            // {
            Loading_OpArch Load = new Loading_OpArch();
            Load.Show();

            
            

            Load.progressBar1.Maximum = 4;// * m_Capability_GUID.Count;
            Load.progressBar1.Value = 0;
            Load.progressBar1.Step = 1;
            Load.Refresh();
            ////////////////////////////////////////////////////
            ///RequiremetnsCategory
            #region RequiremntsCategory
            if(Data.capability_xac == true)
            {
                Load.label_Progress.Text = "Anlegen Capability";
                Load.progressBar1.Update();
                //  List<string> m_Capability_GUID = new List<string>();
                //  m_Capability_GUID = repository_Elements.Get_Capability_GUID(repository, Data);
                repository_Elements.Get_Capability_Parallel(Data, repository);
                Load.progressBar1.PerformStep();
                Load.progressBar1.Update();
                Load.Refresh();
            }
        
            #endregion RequirementsCategory
            //Szenar
            if(Data.logical_xac == true)
            {
                #region Logical
                Load.label_Progress.Text = "Anlegen Logical";
                Load.progressBar1.Update();
                repository_Elements.Get_Logicals_Parallel(Data);
                Load.progressBar1.PerformStep();
                Load.progressBar1.Update();
                Load.Refresh();
                /* List<Logical> m_Logical_1 =  repository_Elements.Get_Logicals(Data);
                 if(m_Logical_1 != null)
                 {
                     Data.m_Logical = m_Logical_1;
                 }*/
                #endregion Logical
            }

            //NodeType
            #region NodeType 
            List<NodeType> m_choosed_NodeType = new List<NodeType>();
            List<SysElement> m_choosed_SysElem = new List<SysElement>();

            if (Data.sys_xac == true)
            {
                Load.label_Progress.Text = "Anlegen NodeType";
                Load.progressBar1.Update();
                if(Data.metamodel.modus == 0)
                {
                    repository_Elements.Get_NodeTypes_Parallel(Data, repository, Load);
                }
                if (Data.metamodel.modus == 1)
                {
                    repository_Elements.Get_Systemelemente_Parallel(Data, repository, Load);
                }

               
                Load.progressBar1.PerformStep();
                Load.progressBar1.Update();
                Load.Refresh();

                Forms.Choose_Systemelement choose = new Forms.Choose_Systemelement(Data);

                choose.ShowDialog();

                if(choose.DialogResult == DialogResult.OK)
                {
                    m_choosed_NodeType = choose.m_nodeTypes;
                    m_choosed_SysElem = choose.m_syselem;
                }

               
            }
            else
            {
                m_choosed_NodeType = Data.m_NodeType;
                m_choosed_SysElem = Data.m_SysElemente;
            }

            Data.m_NodeType = m_choosed_NodeType;
            Data.m_SysElemente = m_choosed_SysElem;

            #endregion
            ///////////////////////////////////////////////////
            ///Stakeholder
            if(Data.stakeholder_xac == true)
            {
                #region Stakeholder
                Load.label_Progress.Text = "Anlegen Stakeholder";
                Load.progressBar1.Update();
                //List<string> m_st_guid = repository_Elements.Get_Stakeholder_GUID(repository, Data);
                repository_Elements.Get_Stakeholder_Parallel(repository, Data);
                Load.progressBar1.PerformStep();
                Load.progressBar1.Update();
                Load.Refresh();
                #endregion
            }

            

            //  Data.oLEDB_Interface.OLEDB_Open();
            //  Data.oLEDB_Interface.dbConnection.Open();

            #region Stereotype
            //Erhalten Stereotypen
            List<string> m_Type1 = Data.metamodel.m_Requirement_Functional.Select(x => x.Type).ToList();
                List<string> m_Type2 = Data.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList();
                List<string> m_Type3 = Data.metamodel.m_Requirement_User.Select(x => x.Type).ToList();
                List<string> m_Type4 = Data.metamodel.m_Requirement_Design.Select(x => x.Type).ToList();
                List<string> m_Type5 = Data.metamodel.m_Requirement_Process.Select(x => x.Type).ToList();
                List<string> m_Type6 = Data.metamodel.m_Requirement_Environment.Select(x => x.Type).ToList();
                List<string> m_Type7 = Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Type).ToList();
                List<string> m_Type8 = Data.metamodel.m_Requirement_Quality_Class.Select(x => x.Type).ToList();
            List<string> m_Type81 = Data.metamodel.m_Requirement_Quality_Activity.Select(x => x.Type).ToList();
            List<string> m_Type9 = Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Type).ToList();

            if (m_Type2.Count > 0)
                {
                    m_Type1.AddRange(m_Type2);
                }
                if (m_Type3.Count > 0)
                {
                    m_Type1.AddRange(m_Type3);
                }
                if(m_Type4.Count > 0)
                {
                    m_Type1.AddRange(m_Type4);
                }
                if (m_Type5.Count > 0)
                {
                    m_Type1.AddRange(m_Type5);
                }
                if (m_Type6.Count > 0)
                {
                    m_Type1.AddRange(m_Type6);
                }
                if (m_Type7.Count > 0)
                {
                    m_Type1.AddRange(m_Type7);
                }
            if (m_Type8.Count > 0)
            {
                m_Type1.AddRange(m_Type8);
            }
                if (m_Type81.Count > 0)
                {
                    m_Type1.AddRange(m_Type81);
                }
                if (m_Type9.Count > 0)
            {
                m_Type1.AddRange(m_Type9);
            }


            // List<string> m_Stereotype = repository_Elements.Get_Stereotpye_Requirements(m_Type1, Data);
            //Stereotypen der AFo erhalten
            #region Add Stereptype Requirement
            List<string> m_Stereotype = new List<string>();
            if(Data.afo_funktional_xac == true)
            {
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList());
            }
            if(Data.afo_design_xac == true)
            {
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList());
            }
            if (Data.afo_interface_xac == true)
            {
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList());
            }
            if (Data.afo_process_xac == true)
            {
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList());
            }
            if (Data.afo_typevertreter_xac == true)
            {
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Stereotype).ToList());
            }
            if (Data.afo_umwelt_xac == true)
            {
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList());
            }
            if (Data.afo_user_xac == true)
            {
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList());
            }
           
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(Data.metamodel.m_Requirement_Quality_Class.Select(x => x.Stereotype).ToList());
            m_Stereotype.AddRange(Data.metamodel.m_Requirement_Quality_Activity.Select(x => x.Stereotype).ToList());


            m_Stereotype.Add("none");
            #endregion
            #endregion

            Load.Close();
            //Auswählen Stereotypen
            //  Choose_Export_Flat choose_Export = new Choose_Export_Flat(m_Stereotype);
            //     choose_Export.ShowDialog();
            //Erhalten Requirements und Exportieren
            //   if (choose_Export.DialogResult == DialogResult.OK)
            //   {
            //REquirements
            Loading_OpArch loading = new Loading_OpArch();

            if (Data.afo_funktional_xac == true || Data.afo_design_xac == true || Data.afo_interface_xac == true || Data.afo_process_xac == true || Data.afo_typevertreter_xac == true || Data.afo_umwelt_xac == true || Data.afo_user_xac == true)
            {
                #region Requirements
                ///////////////////////////////////
                ///Ladebalken
             
                loading.label2.Text = "Export Elements as xac";
                loading.label_Progress.Text = "Get Requirements";
                loading.progressBar1.Step = 1;
                loading.progressBar1.Minimum = 0;
                loading.progressBar1.Maximum = 4;

                loading.Show();
                repository_Elements.Get_Requirements_Parallel(Data, repository, m_Stereotype, m_Type1, loading);

                //Filtern Anforderungen
                //Anforderungen ohne System
                if(Data.sys_xac == true)
                {
                    switch(Data.metamodel.modus)
                    {
                        case 0:
                            List<Requirements.Requirement> m_req_1 = Data.m_Requirement.Where(x => x.nodeType.Classifier_ID == null).ToList();
                            List<Requirements.Requirement> m_req_2 = Data.m_Requirement.Where(x => m_choosed_NodeType.Contains(x.nodeType) == true).ToList();
                            List<Requirements.Requirement> m_req = new List<Requirements.Requirement>();
                            m_req.AddRange(m_req_1);
                            m_req.AddRange(m_req_2);
                            Data.m_Requirement = m_req;
                            break;
                        case 1:
                            //List<Requirement> m_req_12 = Data.m_Requirement.Where(x => x.sysElement.Classifier_ID == null).ToList();
                            List<Requirements.Requirement> m_req_22 = Data.m_Requirement.Where(x => m_choosed_SysElem.Contains(x.sysElement) == true).ToList();
                            List<Requirements.Requirement> m_req2 = new List<Requirements.Requirement>();
                            //m_req2.AddRange(m_req_12);
                            m_req2.AddRange(m_req_22);
                            Data.m_Requirement = m_req2;
                            break;
                        default:
                            List<Requirements.Requirement> m_req_11 = Data.m_Requirement.Where(x => x.nodeType.Classifier_ID == null).ToList();
                            List<Requirements.Requirement> m_req_21 = Data.m_Requirement.Where(x => m_choosed_NodeType.Contains(x.nodeType) == true).ToList();
                            List<Requirements.Requirement> m_req1 = new List<Requirements.Requirement>();
                            m_req1.AddRange(m_req_11);
                            m_req1.AddRange(m_req_21);
                            Data.m_Requirement = m_req1;
                            break;
                    }
                    
                }
               
                #endregion
            }


          

            Export_xml export_Xml = new Export_xml();
                    export_Xml.Data = Data;
                    export_Xml.Export_flat_xac(Data, repository, loading, filename);

          
            // }

            interface_Collection_oleDB.Close_Connection(Data);
            //Data.oLEDB_Interface.OLEDB_Close();

            //}
          /*  catch (Exception err)
            {
                MessageBox.Show(err.Message);
              
                Data.oLEDB_Interface.OLEDB_Close();
            }*/

        }

        private void Export_PFK(EA.Repository repository)
        {
            ////////////////////////////////////
            //Dialog: Wo soll exportiert werden?
            string filename = Choose_Savespace();
            /////////////////////////////////
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_oleDB = new Requirement_Plugin.Interfaces.Interface_Collection();
            //Metamodel festlegen
            // Metamodel metamodel = new Metamodel();
            // metamodel.Choose_Metamodel();
            this.metamodel.Create_NAFv4_OpArch();
            Database Data = new Database(this.metamodel, repository, this);
            interface_Collection_oleDB.Open_Connection(Data);

            #region Setzen Flags
            Data.capability_xac = true;
            Data.logical_xac = true;
            Data.stakeholder_xac = true;
            Data.sys_xac = true;
            Data.metamodel.modus = 1;
            Data.afo_funktional_xac = true;
            Data.afo_design_xac = true;
            Data.afo_interface_xac = true;
            Data.afo_process_xac = true;
            Data.afo_typevertreter_xac = true;
            Data.afo_umwelt_xac = true;
            Data.afo_user_xac = true;

            Data.link_decomposition = true;
            Data.link_afo_afo = true;
            Data.link_afo_cap = true;
            Data.link_afo_logical = true;
            Data.link_afo_sys = true;

            #endregion
            Loading_OpArch Load = new Loading_OpArch();
            Load.Show();


            ////////////////////////////////////////////////////
            ///RequiremetnsCategory
            #region RequiremntsCategory
            if (Data.capability_xac == true)
            {
                Load.label_Progress.Text = "Anlegen RequirementCategory";
                Load.progressBar1.Update();
                //  List<string> m_Capability_GUID = new List<string>();
                //  m_Capability_GUID = repository_Elements.Get_Capability_GUID(repository, Data);
                repository_Elements.Get_Capability_Parallel(Data, repository);
                Load.progressBar1.PerformStep();
                Load.progressBar1.Update();
                Load.Refresh();
            }
            #endregion RequirementsCategory

            #region RequirementCatalogue
            if (Data.capability_xac == true)
            {
                Load.label_Progress.Text = "Anlegen RequirementCatalogue";
                Load.progressBar1.Update();
                //  List<string> m_Capability_GUID = new List<string>();
                //  m_Capability_GUID = repository_Elements.Get_Capability_GUID(repository, Data);
                repository_Elements.Get_CatalogueParallel(Data, repository);
                Load.progressBar1.PerformStep();
                Load.progressBar1.Update();
                Load.Refresh();
            }
            #endregion

            #region Auswahl RequirementsCatalogue

            Requirement_Plugin.Forms.Export.Auswahl_Catalogue auswahl_Catalogue = new Forms.Export.Auswahl_Catalogue(Data.m_Catalogue);

            auswahl_Catalogue.ShowDialog();

            #endregion


            if(auswahl_Catalogue.m_ret.Count == 1)
            {
                #region Capability rauswerfen

                Data.m_Capability = auswahl_Catalogue.m_ret[0].GetCapabilities();

                #endregion


                #region Szenar
                //Szenar
                if (Data.logical_xac == true)
                {
                    #region Logical
                    Load.label_Progress.Text = "Anlegen Logical";
                    Load.progressBar1.Update();
                    repository_Elements.Get_Logicals_Parallel(Data);
                    Load.progressBar1.PerformStep();
                    Load.progressBar1.Update();
                    Load.Refresh();
                    #endregion Logical
                }
                #endregion Szenar

                #region NodeType 
                List<NodeType> m_choosed_NodeType = new List<NodeType>();
                List<SysElement> m_choosed_SysElem = new List<SysElement>();

                if (Data.sys_xac == true)
                {
                    Load.label_Progress.Text = "Anlegen NodeType";
                    Load.progressBar1.Update();
                    if (Data.metamodel.modus == 0)
                    {
                        repository_Elements.Get_NodeTypes_Parallel(Data, repository, Load);
                    }
                    if (Data.metamodel.modus == 1)
                    {
                        repository_Elements.Get_Systemelemente_Parallel(Data, repository, Load);
                    }


                    Load.progressBar1.PerformStep();
                    Load.progressBar1.Update();
                    Load.Refresh();

                    Forms.Choose_Systemelement choose = new Forms.Choose_Systemelement(Data);

                    choose.ShowDialog();

                    if (choose.DialogResult == DialogResult.OK)
                    {
                        m_choosed_NodeType = choose.m_nodeTypes;
                        m_choosed_SysElem = choose.m_syselem;
                    }


                }
                else
                {
                    m_choosed_NodeType = Data.m_NodeType;
                    m_choosed_SysElem = Data.m_SysElemente;
                }

                Data.m_NodeType = m_choosed_NodeType;
                Data.m_SysElemente = m_choosed_SysElem;

                #endregion
                ///////////////////////////////////////////////////
                ///Stakeholder
                if (Data.stakeholder_xac == true)
                {
                    #region Stakeholder
                    Load.label_Progress.Text = "Anlegen Stakeholder";
                    Load.progressBar1.Update();
                    //List<string> m_st_guid = repository_Elements.Get_Stakeholder_GUID(repository, Data);
                    repository_Elements.Get_Stakeholder_Parallel(repository, Data);
                    Load.progressBar1.PerformStep();
                    Load.progressBar1.Update();
                    Load.Refresh();
                    #endregion
                }


                #region Stereotype
                //Erhalten Stereotypen
                List<string> m_Type1 = Data.metamodel.m_Requirement_Functional.Select(x => x.Type).ToList();
                List<string> m_Type2 = Data.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList();
                List<string> m_Type3 = Data.metamodel.m_Requirement_User.Select(x => x.Type).ToList();
                List<string> m_Type4 = Data.metamodel.m_Requirement_Design.Select(x => x.Type).ToList();
                List<string> m_Type5 = Data.metamodel.m_Requirement_Process.Select(x => x.Type).ToList();
                List<string> m_Type6 = Data.metamodel.m_Requirement_Environment.Select(x => x.Type).ToList();
                List<string> m_Type7 = Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Type).ToList();
                List<string> m_Type8 = Data.metamodel.m_Requirement_Quality_Class.Select(x => x.Type).ToList();
                List<string> m_Type81 = Data.metamodel.m_Requirement_Quality_Activity.Select(x => x.Type).ToList();
                List<string> m_Type9 = Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Type).ToList();

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
                if (m_Type6.Count > 0)
                {
                    m_Type1.AddRange(m_Type6);
                }
                if (m_Type7.Count > 0)
                {
                    m_Type1.AddRange(m_Type7);
                }
                if (m_Type8.Count > 0)
                {
                    m_Type1.AddRange(m_Type8);
                }
                if (m_Type81.Count > 0)
                {
                    m_Type1.AddRange(m_Type81);
                }
                if (m_Type9.Count > 0)
                {
                    m_Type1.AddRange(m_Type9);
                }


                // List<string> m_Stereotype = repository_Elements.Get_Stereotpye_Requirements(m_Type1, Data);
                //Stereotypen der AFo erhalten
                #region Add Stereptype Requirement
                List<string> m_Stereotype = new List<string>();
                if (Data.afo_funktional_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_design_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_interface_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_process_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_typevertreter_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_umwelt_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_user_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList());
                }

                m_Stereotype.AddRange(Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Stereotype).ToList());
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Quality_Class.Select(x => x.Stereotype).ToList());
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Quality_Activity.Select(x => x.Stereotype).ToList());


                m_Stereotype.Add("none");
                #endregion
                #endregion

                Load.Close();

                Loading_OpArch loading = new Loading_OpArch();

                if (Data.afo_funktional_xac == true || Data.afo_design_xac == true || Data.afo_interface_xac == true || Data.afo_process_xac == true || Data.afo_typevertreter_xac == true || Data.afo_umwelt_xac == true || Data.afo_user_xac == true)
                {
                    #region Requirements
                    ///////////////////////////////////
                    ///Ladebalken

                    loading.label2.Text = "Export Elements as xac";
                    loading.label_Progress.Text = "Get Requirements";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = 4;

                    loading.Show();
                    repository_Elements.Get_Requirements_Parallel_PFK(Data, repository, m_Stereotype, m_Type1, loading);

                    //Filtern Anforderungen
                    //Anforderungen ohne System
                    if (Data.sys_xac == true)
                    {
                        switch (Data.metamodel.modus)
                        {
                            case 0:
                                List<Requirements.Requirement> m_req_1 = Data.m_Requirement.Where(x => x.nodeType.Classifier_ID == null).ToList();
                                List<Requirements.Requirement> m_req_2 = Data.m_Requirement.Where(x => m_choosed_NodeType.Contains(x.nodeType) == true).ToList();
                                List<Requirements.Requirement> m_req = new List<Requirements.Requirement>();
                                m_req.AddRange(m_req_1);
                                m_req.AddRange(m_req_2);
                                Data.m_Requirement = m_req;
                                break;
                            case 1:
                                //List<Requirement> m_req_12 = Data.m_Requirement.Where(x => x.sysElement.Classifier_ID == null).ToList();
                                List<Requirements.Requirement> m_req_22 = Data.m_Requirement.Where(x => m_choosed_SysElem.Contains(x.sysElement) == true).ToList();
                                List<Requirements.Requirement> m_req2 = new List<Requirements.Requirement>();
                                //m_req2.AddRange(m_req_12);
                                m_req2.AddRange(m_req_22);
                                Data.m_Requirement = m_req2;
                                break;
                            default:
                                List<Requirements.Requirement> m_req_11 = Data.m_Requirement.Where(x => x.nodeType.Classifier_ID == null).ToList();
                                List<Requirements.Requirement> m_req_21 = Data.m_Requirement.Where(x => m_choosed_NodeType.Contains(x.nodeType) == true).ToList();
                                List<Requirements.Requirement> m_req1 = new List<Requirements.Requirement>();
                                m_req1.AddRange(m_req_11);
                                m_req1.AddRange(m_req_21);
                                Data.m_Requirement = m_req1;
                                break;
                        }

                    }

                    //Nur AP1 Anforderungen
                    Data.m_Requirement = Data.m_Requirement.Where(x => x.AFO_CPM_PHASE == AFO_CPM_PHASE.Eins).ToList();

                    #endregion
                }

                Export_xml export_Xml = new Export_xml();
                export_Xml.Data = Data;
                export_Xml.Export_flat_xac(Data, repository, loading, filename);

                Load.Close();

                interface_Collection_oleDB.Close_Connection(Data);

            }
            else
            {
                //Mehrfachauswahl
                MessageBox.Show("Es wurde mehr als eine RequirementsCategory ausgewählt. Der Export wird abgebrochen.");

                Load.Close();
            }
           


            
        }

        private void Export_LV(EA.Repository repository)
        {

        }

        private void Export_LB(EA.Repository repository)
        {
            ////////////////////////////////////
            //Dialog: Wo soll exportiert werden?
            string filename = Choose_Savespace();
            /////////////////////////////////
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_oleDB = new Requirement_Plugin.Interfaces.Interface_Collection();
            //Metamodel festlegen
            // Metamodel metamodel = new Metamodel();
            // metamodel.Choose_Metamodel();
            this.metamodel.Create_NAFv4_OpArch();
            Database Data = new Database(this.metamodel, repository, this);
            interface_Collection_oleDB.Open_Connection(Data);

            #region Setzen Flags
            Data.capability_xac = true;
            Data.logical_xac = true;
            Data.stakeholder_xac = true;
            Data.sys_xac = true;
            Data.metamodel.modus = 1;
            Data.afo_funktional_xac = true;
            Data.afo_design_xac = true;
            Data.afo_interface_xac = true;
            Data.afo_process_xac = true;
            Data.afo_typevertreter_xac = true;
            Data.afo_umwelt_xac = true;
            Data.afo_user_xac = true;

            Data.link_decomposition = true;
            Data.link_afo_afo = true;
            Data.link_afo_cap = true;
            Data.link_afo_logical = true;
            Data.link_afo_sys = true;

            #endregion
            Loading_OpArch Load = new Loading_OpArch();
            Load.Show();


            ////////////////////////////////////////////////////
            ///RequiremetnsCategory
            #region RequiremntsCategory
            if (Data.capability_xac == true)
            {
                Load.label_Progress.Text = "Anlegen RequirementCategory";
                Load.progressBar1.Update();
                //  List<string> m_Capability_GUID = new List<string>();
                //  m_Capability_GUID = repository_Elements.Get_Capability_GUID(repository, Data);
                repository_Elements.Get_Capability_Parallel(Data, repository);
                Load.progressBar1.PerformStep();
                Load.progressBar1.Update();
                Load.Refresh();
            }
            #endregion RequirementsCategory

            #region RequirementCatalogue
            if (Data.capability_xac == true)
            {
                Load.label_Progress.Text = "Anlegen RequirementCatalogue";
                Load.progressBar1.Update();
                //  List<string> m_Capability_GUID = new List<string>();
                //  m_Capability_GUID = repository_Elements.Get_Capability_GUID(repository, Data);
                repository_Elements.Get_CatalogueParallel(Data, repository);
                Load.progressBar1.PerformStep();
                Load.progressBar1.Update();
                Load.Refresh();
            }
            #endregion

            #region Auswahl RequirementsCatalogue

            Requirement_Plugin.Forms.Export.Auswahl_Catalogue auswahl_Catalogue = new Forms.Export.Auswahl_Catalogue(Data.m_Catalogue);

            auswahl_Catalogue.ShowDialog();

            #endregion


            if (auswahl_Catalogue.m_ret.Count == 1)
            {
                #region Capability rauswerfen

                Data.m_Capability = auswahl_Catalogue.m_ret[0].GetCapabilities();

                #endregion


                #region Szenar
                //Szenar
                if (Data.logical_xac == true)
                {
                    #region Logical
                    Load.label_Progress.Text = "Anlegen Logical";
                    Load.progressBar1.Update();
                    repository_Elements.Get_Logicals_Parallel(Data);
                    Load.progressBar1.PerformStep();
                    Load.progressBar1.Update();
                    Load.Refresh();
                    #endregion Logical
                }
                #endregion Szenar

                #region NodeType 
                List<NodeType> m_choosed_NodeType = new List<NodeType>();
                List<SysElement> m_choosed_SysElem = new List<SysElement>();

                if (Data.sys_xac == true)
                {
                    Load.label_Progress.Text = "Anlegen NodeType";
                    Load.progressBar1.Update();
                    if (Data.metamodel.modus == 0)
                    {
                        repository_Elements.Get_NodeTypes_Parallel(Data, repository, Load);
                    }
                    if (Data.metamodel.modus == 1)
                    {
                        repository_Elements.Get_Systemelemente_Parallel(Data, repository, Load);
                    }


                    Load.progressBar1.PerformStep();
                    Load.progressBar1.Update();
                    Load.Refresh();

                    Forms.Choose_Systemelement choose = new Forms.Choose_Systemelement(Data);

                    choose.ShowDialog();

                    if (choose.DialogResult == DialogResult.OK)
                    {
                        m_choosed_NodeType = choose.m_nodeTypes;
                        m_choosed_SysElem = choose.m_syselem;
                    }


                }
                else
                {
                    m_choosed_NodeType = Data.m_NodeType;
                    m_choosed_SysElem = Data.m_SysElemente;
                }

                Data.m_NodeType = m_choosed_NodeType;
                Data.m_SysElemente = m_choosed_SysElem;

                #endregion
                ///////////////////////////////////////////////////
                ///Stakeholder
                if (Data.stakeholder_xac == true)
                {
                    #region Stakeholder
                    Load.label_Progress.Text = "Anlegen Stakeholder";
                    Load.progressBar1.Update();
                    //List<string> m_st_guid = repository_Elements.Get_Stakeholder_GUID(repository, Data);
                    repository_Elements.Get_Stakeholder_Parallel(repository, Data);
                    Load.progressBar1.PerformStep();
                    Load.progressBar1.Update();
                    Load.Refresh();
                    #endregion
                }


                #region Stereotype
                //Erhalten Stereotypen
                List<string> m_Type1 = Data.metamodel.m_Requirement_Functional.Select(x => x.Type).ToList();
                List<string> m_Type2 = Data.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList();
                List<string> m_Type3 = Data.metamodel.m_Requirement_User.Select(x => x.Type).ToList();
                List<string> m_Type4 = Data.metamodel.m_Requirement_Design.Select(x => x.Type).ToList();
                List<string> m_Type5 = Data.metamodel.m_Requirement_Process.Select(x => x.Type).ToList();
                List<string> m_Type6 = Data.metamodel.m_Requirement_Environment.Select(x => x.Type).ToList();
                List<string> m_Type7 = Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Type).ToList();
                List<string> m_Type8 = Data.metamodel.m_Requirement_Quality_Class.Select(x => x.Type).ToList();
                List<string> m_Type81 = Data.metamodel.m_Requirement_Quality_Activity.Select(x => x.Type).ToList();
                List<string> m_Type9 = Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Type).ToList();

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
                if (m_Type6.Count > 0)
                {
                    m_Type1.AddRange(m_Type6);
                }
                if (m_Type7.Count > 0)
                {
                    m_Type1.AddRange(m_Type7);
                }
                if (m_Type8.Count > 0)
                {
                    m_Type1.AddRange(m_Type8);
                }
                if (m_Type81.Count > 0)
                {
                    m_Type1.AddRange(m_Type81);
                }
                if (m_Type9.Count > 0)
                {
                    m_Type1.AddRange(m_Type9);
                }


                // List<string> m_Stereotype = repository_Elements.Get_Stereotpye_Requirements(m_Type1, Data);
                //Stereotypen der AFo erhalten
                #region Add Stereptype Requirement
                List<string> m_Stereotype = new List<string>();
                if (Data.afo_funktional_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_design_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_interface_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_process_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_typevertreter_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Typvertreter.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_umwelt_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList());
                }
                if (Data.afo_user_xac == true)
                {
                    m_Stereotype.AddRange(Data.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList());
                }

                m_Stereotype.AddRange(Data.metamodel.m_Requirement_NonFunctional.Select(x => x.Stereotype).ToList());
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Quality_Class.Select(x => x.Stereotype).ToList());
                m_Stereotype.AddRange(Data.metamodel.m_Requirement_Quality_Activity.Select(x => x.Stereotype).ToList());


                m_Stereotype.Add("none");
                #endregion
                #endregion

                Load.Close();

                Loading_OpArch loading = new Loading_OpArch();

                if (Data.afo_funktional_xac == true || Data.afo_design_xac == true || Data.afo_interface_xac == true || Data.afo_process_xac == true || Data.afo_typevertreter_xac == true || Data.afo_umwelt_xac == true || Data.afo_user_xac == true)
                {
                    #region Requirements
                    ///////////////////////////////////
                    ///Ladebalken

                    loading.label2.Text = "Export Elements as xac";
                    loading.label_Progress.Text = "Get Requirements";
                    loading.progressBar1.Step = 1;
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = 4;

                    loading.Show();
                    repository_Elements.Get_Requirements_Parallel(Data, repository, m_Stereotype, m_Type1, loading);

                    //Filtern Anforderungen
                    //Anforderungen ohne System
                    if (Data.sys_xac == true)
                    {
                        switch (Data.metamodel.modus)
                        {
                            case 0:
                                List<Requirements.Requirement> m_req_1 = Data.m_Requirement.Where(x => x.nodeType.Classifier_ID == null).ToList();
                                List<Requirements.Requirement> m_req_2 = Data.m_Requirement.Where(x => m_choosed_NodeType.Contains(x.nodeType) == true).ToList();
                                List<Requirements.Requirement> m_req = new List<Requirements.Requirement>();
                                m_req.AddRange(m_req_1);
                                m_req.AddRange(m_req_2);
                                Data.m_Requirement = m_req;
                                break;
                            case 1:
                                //List<Requirement> m_req_12 = Data.m_Requirement.Where(x => x.sysElement.Classifier_ID == null).ToList();
                                List<Requirements.Requirement> m_req_22 = Data.m_Requirement.Where(x => m_choosed_SysElem.Contains(x.sysElement) == true || m_choosed_SysElem.Contains(x.sysElement_Real)).ToList();
                                List<Requirements.Requirement> m_req2 = new List<Requirements.Requirement>();
                                //m_req2.AddRange(m_req_12);
                                m_req2.AddRange(m_req_22);
                                Data.m_Requirement = m_req2;
                                break;
                            default:
                                List<Requirements.Requirement> m_req_11 = Data.m_Requirement.Where(x => x.nodeType.Classifier_ID == null).ToList();
                                List<Requirements.Requirement> m_req_21 = Data.m_Requirement.Where(x => m_choosed_NodeType.Contains(x.nodeType) == true).ToList();
                                List<Requirements.Requirement> m_req1 = new List<Requirements.Requirement>();
                                m_req1.AddRange(m_req_11);
                                m_req1.AddRange(m_req_21);
                                Data.m_Requirement = m_req1;
                                break;
                        }

                    }

                    //Nur AP1, AP2, Realisierungsphase Anforderungen
                    Data.m_Requirement = Data.m_Requirement.Where(x => x.AFO_CPM_PHASE == AFO_CPM_PHASE.Eins || x.AFO_CPM_PHASE == AFO_CPM_PHASE.Zwei || x.AFO_CPM_PHASE == AFO_CPM_PHASE.Drei).ToList();


                   

                    #endregion
                }

                Export_xml export_Xml = new Export_xml();
                export_Xml.Data = Data;
                export_Xml.Export_flat_xac(Data, repository, loading, filename);

                Load.Close();

                interface_Collection_oleDB.Close_Connection(Data);

            }
            else
            {
                //Mehrfachauswahl
                MessageBox.Show("Es wurde mehr als eine RequirementsCategory ausgewählt. Der Export wird abgebrochen.");

                Load.Close();
            }


        }

        private void Import_XAC(EA.Repository repository)
        {
            // Metamodel metamodel = new Metamodel();
            // metamodel.Choose_Metamodel();
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();

            if(this.metamodel != null)
            {
               Database help = new Database(metamodel, repository, this);
                
              // switch()

                // try
                // {

                //help.oLEDB_Interface.OLEDB_Open();
                // help.oLEDB_Interface.dbConnection.Open();
                interface_Collection_OleDB.Open_Connection(help);

                    Import_xml xml_Import = new Import_xml(repository, help);

                //    help.oLEDB_Interface.OLEDB_Close();
                /*     }
                    catch(Exception err)
                     {
                         MessageBox.Show(err.Message);
                         help.oLEDB_Interface.OLEDB_Close();
                     }
                     */
            }

           
        }

        private void Edit_Requirement(string GUID, EA.Repository repository)
        {
            #region DataEdit aufbauen

            List<NodeType> m_NodeType = new List<NodeType>();
            List<SysElement> m_Systemelelement = new List<SysElement>();
            List<Activity> m_Activity = new List<Activity>();
            List<Stakeholder> m_Stakeholder = new List<Stakeholder>();
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();

            if (this.created_DB_Edit == false)
            {
                if(this.Data_Edit.metamodel.flag_slow_mode == true)
                {
                    try
                    {
                        //Ladebalken anlegen
                        Loading_OpArch Load = new Loading_OpArch();
                        Load.label2.Text = "...";
                        Load.progressBar1.Minimum = 0;
                        Load.progressBar1.Maximum = 100;
                        Load.label_Progress.Text = "Initialisierung Database Edit";
                        Load.Show();

                        Repsoitory_Elements.Repository_Elements rep_Elems = new Repsoitory_Elements.Repository_Elements();

                        // this.Data_Edit.oLEDB_Interface.OLEDB_Open();
                        //  this.Data_Edit.oLEDB_Interface.dbConnection.Open();
                        interface_Collection_OleDB.Open_Connection(this.Data_Edit);
                        //  this.Data_Edit.metamodel = this.metamodel;
                        this.created_DB_Edit = true;

                        //Alle NodeType erhalten
                        this.Data_Edit.m_NodeType = new List<NodeType>();
                        Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();

                        repository_Elements.Get_NodeTypes_reduced_Parallel(this.Data_Edit, repository, Load);
                        #region Archiv

                        /* List<string> m_NodeType_GUID = rep_Elems.Get_NodeTypes(this.Data_Edit);

                         if (m_NodeType_GUID != null)
                         {
                             Load.progressBar1.Value = 0;
                             Load.progressBar1.Maximum = m_NodeType_GUID.Count;
                             Load.progressBar1.Step = 1;
                             Load.label_Progress.Text = "Anlegen NodeType";
                             Load.Refresh();

                             int i1 = 0;
                             do
                             {
                                 NodeType recent = new NodeType(null, repository, this.Data_Edit);
                                 recent.Classifier_ID = m_NodeType_GUID[i1];
                                 recent.Get_TV_reduced(this.Data_Edit, repository);

                                 this.Data_Edit.m_NodeType.Add(recent);

                                 Load.progressBar1.PerformStep();
                                 Load.Refresh();

                                 i1++;
                             } while (i1 < m_NodeType_GUID.Count);
                         }
                        */
                        #endregion
                        //Alle Systemelemletne erhalten
                        this.Data_Edit.m_SysElemente = new List<SysElement>();

                        repository_Elements.Get_Systemelemente_reduced_Parallel(this.Data_Edit, repository, Load);
                        #region Arcviv

                       /* List<string> m_SysElement_GUID = rep_Elems.Get_SysElemente(this.Data_Edit);

                        if (m_SysElement_GUID != null)
                        {
                            Load.progressBar1.Value = 0;
                            Load.progressBar1.Maximum = m_SysElement_GUID.Count;
                            Load.progressBar1.Step = 1;
                            Load.label_Progress.Text = "Anlegen Systemelemente";
                            Load.Refresh();

                            int i1 = 0;
                            do
                            {
                                SysElement recent = new SysElement(null, repository, this.Data_Edit);
                                recent.Classifier_ID = m_SysElement_GUID[i1];
                                recent.Get_TV_reduced(this.Data_Edit, repository);

                                this.Data_Edit.m_SysElemente.Add(recent);

                                Load.progressBar1.PerformStep();
                                Load.Refresh();

                                i1++;
                            } while (i1 < m_SysElement_GUID.Count);
                        }
                       */
                        #endregion
                        //Alle Aktivitaeten erhalten
                        this.Data_Edit.m_Activity = new List<Activity>();
                        repository_Elements.Get_Activity_Parallel(this.Data_Edit, repository);
                        #region Archiv
                        /*
                        List<string> m_Activity_GUID = rep_Elems.Get_Activity_GUID(repository, this.Data_Edit);

                        if (m_Activity_GUID != null)
                        {
                            Load.progressBar1.Value = 0;
                            Load.progressBar1.Maximum = m_Activity_GUID.Count;
                            Load.progressBar1.Step = 1;
                            Load.label_Progress.Text = "Anlegen Aktivity";
                            Load.Refresh();

                            int i1 = 0;
                            do
                            {
                                Activity recent_Act = new Activity(m_Activity_GUID[i1], this.Data_Edit, repository);
                                this.Data_Edit.m_Activity.Add(recent_Act);

                                Load.progressBar1.PerformStep();
                                Load.Refresh();

                                i1++;
                            } while (i1 < m_Activity_GUID.Count);
                        }
                        */
                        #endregion
                        //Alle Stakeholder erhalten
                        this.Data_Edit.m_Stakeholder = new List<Stakeholder>();

                        repository_Elements.Get_Stakeholder_Parallel(repository, this.Data_Edit);
                        #region Archiv
                        /*
                        List<string> m_Stakeholder_GUID = rep_Elems.Get_Stakeholder_GUID(repository, this.Data_Edit);

                        if (m_Stakeholder_GUID != null)
                        {
                            Load.progressBar1.Value = 0;
                            Load.progressBar1.Maximum = m_Stakeholder_GUID.Count;
                            Load.progressBar1.Step = 1;
                            Load.label_Progress.Text = "Anlegen Stakeholder";
                            Load.Refresh();

                            int i1 = 0;
                            do
                            {
                                Stakeholder recent_St = new Stakeholder(m_Stakeholder_GUID[i1], repository, this.Data_Edit);
                                this.Data_Edit.m_Stakeholder.Add(recent_St);

                                Load.progressBar1.PerformStep();
                                Load.Refresh();

                                i1++;
                            } while (i1 < m_Stakeholder_GUID.Count);
                        }
                        */
                        #endregion
                        Load.Close();
                    }
                    catch (Exception err)
                    {
                        //Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
                        interface_Collection_OleDB.Close_Connection(this.Database_OpArch);
                     //  this.Database_OpArch.oLEDB_Interface.dbConnection.c
                        MessageBox.Show(err.Message);
                    }
                }
                else
                {
                    Loading_OpArch Load = new Loading_OpArch();
                    Load.label2.Text = "...";
                    Load.progressBar1.Minimum = 0;
                    Load.progressBar1.Maximum = 100;
                    Load.label_Progress.Text = "Initialisierung Database Edit";
                    Load.Show();

                    Repsoitory_Elements.TaggedValue tagged = new Repsoitory_Elements.TaggedValue(this.Data_Edit.metamodel, this.Data_Edit);
                    Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();

                    //Alle W_PROZESSWORT erhalten
                   
                    List<string> m_prozesswort = new List<string>();
                    m_prozesswort = tagged.Get_Distinct_Property("W_PROZESSWORT", 0);
                    m_prozesswort = m_prozesswort.Where(X => X != "null" && X != "value").ToList();
                    Load.progressBar1.Value = 0;
                    Load.progressBar1.Maximum = m_prozesswort.Count;
                    Load.progressBar1.Step = 1;
                    Load.label_Progress.Text = "Erhalten Prozesswörter";
                    Load.Refresh();
                    if (m_prozesswort.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            Activity recent = new Activity(null, null, null);
                            recent.W_Prozesswort = m_prozesswort[i1];

                            if (m_Activity.Contains(recent) == false)
                            {
                                m_Activity.Add(recent);
                            }

                            Load.progressBar1.PerformStep();
                            Load.progressBar1.Refresh();
                            i1++;
                        } while (i1 < m_prozesswort.Count);
                    }

                    this.Data_Edit.m_Activity = m_Activity;
                    //Alle SYS_KUERZEL und SYS_ARTIKEL
                 //   List<string> m_Property = new List<string>();
                 //   m_Property.Add("SYS_KUERZEL");
                 //   m_Property.Add("SYS_ARTIKEL");
                //    Repository_Element repository_Element = new Repository_Element();
                //NodeTypes
                    List<int> m_ID = new List<int>();
                    m_ID = repository_Elements.Get_NodeTypes_ID(repository, this.Data_Edit);
                    Load.progressBar1.Value = 0;
                    Load.Refresh();
                    if (m_ID != null)
                    {
                        Load.progressBar1.Maximum = m_ID.Count;
                        Load.progressBar1.Step = 1;
                        Load.label_Progress.Text = "Erhalten NodeType reduced";
                        Load.Refresh();
                        int i2 = 0;
                        do
                        {
                            if(this.Data_Edit.m_NodeType.Select(x => x.ID).Contains(m_ID[i2]) == false)
                            {
                               
                                NodeType recent = new NodeType(null, null, null);
                                recent.ID = m_ID[i2];
                                recent.Classifier_ID = recent.Get_GUID_By_ID(m_ID[i2], Data_Edit);
                                recent.Get_TV_reduced(this.Data_Edit, repository);

                                this.Data_Edit.m_NodeType.Add(recent);
                            }
                            Load.progressBar1.PerformStep();
                            Load.progressBar1.Refresh();
                            i2++;
                        } while (i2 < m_ID.Count);
                    }
                    //Systemelemente
                    List<int> m_ID2 = new List<int>();
                    m_ID2 = repository_Elements.Get_SysElemente_ID(repository, this.Data_Edit);
                    Load.progressBar1.Value = 0;
                    Load.Refresh();
                    if (m_ID2 != null)
                    {
                        Load.progressBar1.Maximum = m_ID2.Count;
                        Load.progressBar1.Step = 1;
                        Load.label_Progress.Text = "Erhalten Systelemente reduced";
                        Load.Refresh();
                        int i2 = 0;
                        do
                        {
                            if (this.Data_Edit.m_SysElemente.Select(x => x.ID).Contains(m_ID2[i2]) == false)
                            {

                                SysElement recent = new SysElement(null, null, null);
                                recent.ID = m_ID2[i2];
                                recent.Classifier_ID = recent.Get_GUID_By_ID(m_ID2[i2], Data_Edit);
                                recent.Get_TV_reduced(this.Data_Edit, repository);

                                this.Data_Edit.m_SysElemente.Add(recent);
                            }
                            Load.progressBar1.PerformStep();
                            Load.progressBar1.Refresh();
                            i2++;
                        } while (i2 < m_ID2.Count);
                    }
                    /*
                    m_NodeType = tagged.Get_Distinct_Property_multiple(m_Property, m_ID, this.Data_Edit);
                    */
                    // this.Data_Edit.m_NodeType = m_NodeType;

                    //////////////////////////////////////////
                    //Stakholder
                    Repository_Element repository_Element = new Repository_Element();
                    List<string> m_GUID = new List<string>();
                    m_GUID = repository_Elements.Get_Stakeholder_GUID(repository, this.Data_Edit);
                    Load.progressBar1.Value = 0;
                    Load.Refresh();
                    if (m_GUID != null)
                    {
                        Load.progressBar1.Maximum = m_ID.Count;
                        Load.progressBar1.Step = 1;
                        Load.label_Progress.Text = "Erhalten Stakeholder reduced";
                        Load.Refresh();
                        int i2 = 0;
                        do
                        {
                            if (this.Data_Edit.m_Stakeholder.Select(x => x.Classifier_ID).Contains(m_GUID[i2]) == false)
                            {

                                Stakeholder recent = new Stakeholder(m_GUID[i2], repository, this.Data_Edit);

                                this.Data_Edit.m_Stakeholder.Add(recent);
                            }
                            Load.progressBar1.PerformStep();
                            Load.progressBar1.Refresh();
                            i2++;
                        } while (i2 < m_GUID.Count);
                    }


                    if(this.Data_Edit.m_Stakeholder.Where(x => x.st_STAKEHOLDER == this.Data_Edit.metamodel.Stakeholder_Default).ToList().Count == 0)
                    {
                        Stakeholder recent = new Stakeholder(null, null, null);
                        recent.st_STAKEHOLDER = this.Data_Edit.metamodel.Stakeholder_Default;
                        recent.st_ARTIKEL = "der";
                        recent.w_NUTZENDER = this.Data_Edit.metamodel.Stakeholder_Default;
                        recent.Name = this.Data_Edit.metamodel.Stakeholder_Default;

                        this.Data_Edit.m_Stakeholder.Add(recent);
                    }


                  /*  Stakeholder stakeholder = new Stakeholder(null, null, null);
                    stakeholder.st_ARTIKEL = "der";
                    stakeholder.st_STAKEHOLDER = "Nutzer";

                    m_Stakeholder.Add(stakeholder);

                    this.Data_Edit.m_Stakeholder = m_Stakeholder;*/

                    this.created_DB_Edit = true;

                    Load.Close();

                }

            }
            
            #endregion Data Edit aufbauen

            #region Requirement erhalten
            Requirements.Requirement requirement = new Requirements.Requirement(GUID, this.Data_Edit.metamodel);
            requirement.Get_Tagged_Values_From_Requirement(GUID, repository, this.Data_Edit);
            requirement.Get_System(this.Data_Edit, repository);
            #endregion Requirement erhalten

            // this.Data_Edit.oLEDB_Interface.dbConnection.Close();
            interface_Collection_OleDB.Close_Connection(this.Data_Edit);

            Form_Edit_Requirement form_Edit = new Form_Edit_Requirement(this.Data_Edit, requirement, repository, true);
            form_Edit.ShowDialog();
        }
      
        private void Check_Pattern(EA.Repository repository)
        {
            Choose_Analyse_Patterns form = new Choose_Analyse_Patterns(this.metamodel);
            Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            form.ShowDialog();

            try
            {
                //  this.Database_OpArch.oLEDB_Interface.OLEDB_Open();
                //  this.Database_OpArch.oLEDB_Interface.dbConnection.Open();
                interface_Collection_OleDB.Open_Connection(this.Database_OpArch);

                Check check = new Check(this.Database_OpArch, repository);
                check.Check_Pattern();

                interface_Collection_OleDB.Close_Connection(this.Database_OpArch);
               // this.Database_OpArch.oLEDB_Interface.OLEDB_Close();

            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
                // this.Database_OpArch.oLEDB_Interface.OLEDB_Close();
                interface_Collection_OleDB.Close_Connection(this.Database_OpArch);
            }

        
        }


        private void Check_Requirements(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Check_Requirements(repository);

            interface_Collection_OleDB.Close_Connection(database_check_req);

            repository.RefreshModelView(0);
        }

        private void Check_Nachweisart(EA.Repository repository)
        {

            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Check_Nachweisart(repository);

            interface_Collection_OleDB.Close_Connection(database_check_req);

            repository.RefreshModelView(0);
        }




        private void Check_Requirements_Dopplung(EA.Repository repository)
        {
            this.Database_OpArch.Check_Requirements_Dopplung(repository);
        }
        
        
        private void Update_Sys(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Update_Systemelemente(repository);

            interface_Collection_OleDB.Close_Connection(database_check_req);

            repository.RefreshModelView(0);
        }

        private void Update_Nachweisart(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Update_Nachweisarten(repository);

            interface_Collection_OleDB.Close_Connection(database_check_req);

            repository.RefreshModelView(0);
        }

        public override void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #region Afo Managemenet
        private void Considerate_Connectoren(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Considerate_AFO_Connectoren(repository);

            interface_Collection_OleDB.Close_Connection(database_check_req);

            repository.RefreshModelView(0);
        }

        private void Transform_Leistungswerte(EA.Repository repository)
        {


            //AFO im ProjektBrowser erhalten
            EA.Collection collection = repository.GetTreeSelectedElements();
            if(collection.Count > 0)
            {
                List<string> m_req_guid = new List<string>();
                EA.Element element;
                short i1 = 0;
                do
                {
                    element = collection.GetAt(i1);
                    if(element.Type == "Requirement")
                    {
                        m_req_guid.Add(element.ElementGUID);
                    }

                    i1++;
                } while (i1 < collection.Count);

                if(m_req_guid.Count > 0)
                {
                    Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
                    database_check_req.Transform_Requirements(repository, m_req_guid);
                }
                else
                {
                    MessageBox.Show("Es wurden keine Anforderungen im ProjektBrowser ausgewählt.");
                }
            }
            else
            {
                MessageBox.Show("Es wurden keine Anforderungen im ProjektBrowser ausgewählt.");
            }
            

        }

        private void Export_Leistungswerte(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            bool element_tag = false;

            var recent_type = repository.GetTreeSelectedItemType();

            if(recent_type == EA.ObjectType.otElement)
            {
                EA.Element recent = repository.GetTreeSelectedObject();

                if(database_check_req.metamodel.m_Capability_Catalogue.Select(x => x.Stereotype).ToList().Where(y => y == recent.Stereotype).ToList().Count > 0)
                {
                    Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
                    interface_Collection_OleDB.Open_Connection(database_check_req);

                    database_check_req.Export_TFL_Requirments(repository, recent.ElementGUID);

                    interface_Collection_OleDB.Close_Connection(database_check_req);


                    element_tag = true;
                }
                else
                {
                    element_tag = false;
                }

            }
            else
            {
                element_tag = false;
            }

            if (element_tag == false)
            {
                MessageBox.Show("Es wurde keine " + database_check_req.metamodel.m_Capability_Catalogue.Select(x => x.Stereotype).ToList()[0] + " im ProjektBrwoser ausgewählt. Es wird kein Export erstellt");
            }

        }
        private void Export_Afo_Mapping (EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            bool element_tag = false;

            var recent_type = repository.GetTreeSelectedItemType();

            if (recent_type == EA.ObjectType.otElement)
            {
                EA.Element recent = repository.GetTreeSelectedObject();

                if (database_check_req.metamodel.m_Capability_Catalogue.Select(x => x.Stereotype).ToList().Where(y => y == recent.Stereotype).ToList().Count > 0)
                {
                    database_check_req.Export_Afo_Mapping(repository, recent.ElementGUID);
                }

                element_tag = true;
            }
            else
            {
                element_tag = false;
            }

            if (element_tag == false)
            {
                MessageBox.Show("Es wurde keine " + database_check_req.metamodel.m_Capability_Catalogue.Select(x => x.Stereotype).ToList()[0] + " im ProjektBrwoser ausgewählt. Es wird kein Export erstellt");
            }
        }
        private void Start_SysArch(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Start_SysArch(repository);

            interface_Collection_OleDB.Close_Connection(database_check_req);

            this.Data_Edit = null;
            this.Database_OpArch = null;
            this.metamodel = database_check_req.metamodel;

            repository.RefreshModelView(0);
        }

        private void Start_Bewertung(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Start_Bewertung(repository);


            interface_Collection_OleDB.Close_Connection(database_check_req);

            //this.Data_Edit = null;
            //this.Database_OpArch = null;
            this.metamodel = database_check_req.metamodel;

            repository.RefreshModelView(0);
        }

        private void Start_Begruendung(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Start_Begruendung(repository);

            interface_Collection_OleDB.Close_Connection(database_check_req);

            repository.RefreshModelView(0);
        }
        private void Considerate_Replaces(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Considerate_Replaces_Con(repository);

            interface_Collection_OleDB.Close_Connection(database_check_req);

            repository.RefreshModelView(0);
        }

        private void Archiv_Requirement(EA.Repository repository)
        {
            Database database_check_req = new Database(this.Database_OpArch.metamodel, repository, this.Database_OpArch.Base);
            Requirement_Plugin.Interfaces.Interface_Collection interface_Collection_OleDB = new Interfaces.Interface_Collection();
            interface_Collection_OleDB.Open_Connection(database_check_req);

            database_check_req.Archiv_Anforderungen(repository, database_check_req);

            interface_Collection_OleDB.Close_Connection(database_check_req);

          //  repository.Models.GetAt(0).Refresh();
            repository.RefreshModelView(0);
        }
        #endregion

        #region Savespace
        /// <summary>
        /// Abfrage des Speicherortes der xac Datei mit Hilfe eines Savefildialoges
        /// </summary>
        /// <returns></returns>
        private string Choose_Savespace()
        {
            string filename = "test";
            bool save = false;

            do
            {
                SaveFileDialog saveFileDialog_Save = new SaveFileDialog();
                saveFileDialog_Save.Filter = "Require7|*.xac";
                saveFileDialog_Save.Title = "Save an Require7 File";
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
    }
}
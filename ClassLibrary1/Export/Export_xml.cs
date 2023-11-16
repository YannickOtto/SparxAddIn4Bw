using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using Metamodels;
using Repsoitory_Elements;
using Elements;
using Requirements;
using Forms;

namespace Requirement_Plugin
{
    public class Export_xml
    {
        List<Requirement> m_Requirement;
        List<Capability> m_Capability;
        List<NodeType> m_NodeType;
        List<SysElement> m_SysElem;
        List<SysElement> m_SysElem_Real;
        public Database Data;
        private Create_Metamodel_Class Metamodel_Base;
        private List<string> m_GUID_Requirement = new List<string>();

        #region Konstruktor und Destrukto
        public Export_xml()
        {
            this.m_Requirement = new List<Requirement>();
            this.m_Capability = new List<Capability>();
            this.Metamodel_Base = new Create_Metamodel_Class();
        }

        ~Export_xml()
        {

        }
        #endregion Konstruktor und Destrukto

        #region Exportmöglichkeiten
        public void Export_xml_xac(Database Data, EA.Repository repository,string  filename)
        {
            this.Data = Data;
            this.m_Requirement = new List<Requirement>();

         
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            Repsoitory_Elements.Repository_Elements repository_Elements = new Repsoitory_Elements.Repository_Elements();
            Repository_Connector repository_Connector = new Repository_Connector();
            ////////////////////////////////////
           
            /////////////////////////////////////
            //Dialog: Was soll exportiert werden?

            Choose_Require7_xac export_form = new Choose_Require7_xac(Data);
            export_form.ShowDialog();

            ///////////////////////////////////
            ///Ladebalken
            Loading_OpArch loading = new Loading_OpArch();
            loading.label2.Text = "Export Elements as xac";
            loading.label_Progress.Text = "Generate xml-Header";
            loading.progressBar1.Step = 1;
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = 1;

            loading.Show();
            ////////////////////////////////////
            //Erstellung XMLDatei
            #region Add_Header
            var xml_dat = new XDocument(
                 new XDeclaration("1.0", "UTF-8", "yes")
                // new XComment("Kommentar")
                );
            var Root = new XElement("xac");
            var Att_revision = new XAttribute("revision", "3");
            var Att_itemspec = new XAttribute("itemspec", "");
            var Att_description = new XAttribute("description", "");
            var Att_lastmodiefiedby = new XAttribute("lastmodifiedby", Data.metamodel.createdby);//Autor eintragen?
            //Uhrziet und Datum
            string date = DateTime.Now.ToString("o");
            var Att_lastmodifieddate = new XAttribute("lastmodifieddate", date);
            var Att_CreatedwithWv = new XAttribute("createdWithWv", "Analysephase 06.11.2019 16:12");
            var Att_createdwith = new XAttribute("createdWith", "Require.7 V2.4.2435");
            var Att_source = new XAttribute("source", "");
            //Hinzufügen der Attribute
            Root.Add(Att_revision);
            Root.Add(Att_itemspec);
            Root.Add(Att_description);
            Root.Add(Att_lastmodiefiedby);
            Root.Add(Att_lastmodifieddate);
            Root.Add(Att_CreatedwithWv);
            Root.Add(Att_createdwith);
            Root.Add(Att_source);

            //Root.Add(Att_source);
            //Root.Add(Att_createdwith);
            //Root.Add(Att_CreatedwithWv);
            //Root.Add(Att_lastmodifieddate);
            //Root.Add(Att_lastmodiefiedby);
            //Root.Add(Att_description);
            //Root.Add(Att_itemspec);
            //Root.Add(Att_revision);

            xml_dat.Add(Root);
            #endregion
            ///////////////////////////////////
            //items Knoten hinzufügen ..> Anforderungen, Systemelemente
            XElement items = new XElement("items");
            Root.Add(items);
            //Links Knoten hinzufügen
            XElement links = new XElement("links");
            Root.Add(links);

            ///
            /// Ladebalken refresehn
            loading.progressBar1.PerformStep();
            loading.Refresh();
            ///////////////////////////////////
            //Sys und Afo hinzufügen 
            #region Add_Sys_And_Afo
            if (Data.sys_xac == true || Data.link_afo_sys == true || Data.afo_interface_xac == true || Data.afo_funktional_xac == true || Data.afo_design_xac == true || Data.afo_process_xac == true || Data.afo_umwelt_xac == true|| Data.afo_typevertreter_xac == true)
            {
                if(Data.Decomposition != null)
                {
                    if (Data.Decomposition.m_NodeType.Count > 0)
                    {
                        ////////////////////
                        ///Festlegen Werte Ladebalken
                        loading.label_Progress.Text = "Dekomposition";
                        loading.label2.Text = "Systemelemente";
                        loading.progressBar1.Value = 0;
                        loading.progressBar1.Maximum = Data.m_NodeType.Count;

                        loading.Refresh();

                        #region Header_Sys
                        if (Data.sys_xac == true)
                        {
                            //L <Logisches Decomposition erstellen>
                            XElement Log_Dec = this.Add_Sys_Root(Data, Data.metamodel.m_Header_agid[0], Data.SYS_ENUM.SYS_TYP[2], "Systemelement", "<LogischeDecomposition>", "die", Data.metamodel.m_Header[0], Data.metamodel.m_object_id[0]);

                            items.Add(Log_Dec);
                        }
                        #endregion Header_Sys
                        //Schleife über die erstellte Decomposition
                        int i1 = 0;
                        do
                        {
                            string sys = Data.Decomposition.m_NodeType[i1].Get_Name(this.Data);

                            loading.label2.Text = sys;
                            loading.label2.Refresh();

                            //Hier werden die Logischen Systemelemente der xml Datei hinzugefügt, ink. Decomposition
                            #region Add_Sys_To_Items
                            if (Data.sys_xac == true && Data.Decomposition.m_NodeType[i1].RPI_Export == true)
                            {
                                //Item des aktuelen Sys anlegen
                                XElement recent_Sys = this.Add_Sys(Data.Decomposition.m_NodeType[i1], repository, Data.metamodel);
                                items.Add(recent_Sys);
                                //Link zu Sys_Header
                                //Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], tagged.Get_Tagged_Value(Data.Decomposition.m_NodeType[i1].Instantiate_GUID, "OBJECT_ID", repository));
                                Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], tagged.Get_Tagged_Value(Data.Decomposition.m_NodeType[i1].Instantiate_GUID, "OBJECT_ID", repository));

                            }
                            #endregion Add_Sys_To_Items

                            //////////////////////////////
                            //Kinderlemete des aktuellen Elemntes in der Dekomposition werden nun betrachtet
                            //Hier müssen noch die Links mit übergeben werden
                          //  if(Data.Decomposition.m_NodeType[i1].RPI_Export == true)
                            {
                                Decomposition_rekursiv(items, links, Data.Decomposition.m_NodeType[i1], repository, Data.metamodel, Data, loading);
                            }


                            //Hier werden die Afo zu der xml Datei hinzugefügt, ink. Konnektoren zu Afo und zu Sys
                            //Sollen die AFO des Systems exportiert werden?
                            //  if(Data.Decomposition.m_NodeType[i1].RPI_Export == true)
                            {
                                //Afo und Link zu den Afo hinzufügen
                                if (Data.link_afo_sys == true || Data.afo_interface_xac == true || Data.afo_funktional_xac == true || Data.afo_design_xac == true || Data.afo_user_xac == true || Data.afo_process_xac == true || Data.afo_umwelt_xac == true||Data.afo_typevertreter_xac == true)
                                {


                                    NodeType recent = Data.Decomposition.m_NodeType[i1];
                                    //Link vom recent_Child System zu seinen Afo ziehen
                                    //link unidirektionale Schnittstellen
                                    if (recent.m_Element_Interface.Count > 0 && Data.afo_interface_xac == true)
                                    {
                                        #region Unidriektionale Schnittstellen betrachten
                                        loading.label2.Text = sys + " - Unidirektionale Schnittstellen";
                                        loading.label2.Refresh();
                                        int i2 = 0;
                                        do
                                        {

                                            #region Add_AFo_Send
                                            if (recent.m_Element_Interface[i2].m_Requirement_Interface_Send.Count > 0 && recent.RPI_Export == true)
                                            {
                                                if (recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].RPI_Export == true)
                                                {
                                                    #region Afo_Send zur xml DAtei ink. Konnektoren (send) hinzufügen
                                                    //Afo_send hinzufügen
                                                    if (this.m_GUID_Requirement.Contains(recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID) == false && Data.afo_interface_xac == true && this.m_Requirement.Contains(recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0]) == false)
                                                    {
                                                        #region Add_AFo
                                                        this.m_GUID_Requirement.Add(recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID);
                                                        this.m_Requirement.Add(recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0]);
                                                        XElement recent_Afo_send = this.Add_AFo(recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0], repository, Data.metamodel);
                                                        items.Add(recent_Afo_send);
                                                        #endregion Add_AFo
                                                        #region Add_Send_Connector
                                                        //Send Konnektor --> Ist imAllgemeien Link Export nun drin
                                                        /*      if (Data.link_afo_afo == true)
                                                               {
                                                                   string SQL_send = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = " + repository.GetElementByGuid(recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID).ElementID + " AND Stereotype IN" +xML.SQL_IN_Array(Data.metamodel.Send_Stereotype) + " AND Connector_Type IN" +xML.SQL_IN_Array( Data.metamodel.Send_Type) + ";";
                                                                   string xml_String_send = repository.SQLQuery(SQL_send);
                                                                   List<string> m_End_ID = xML.Xml_Read_Attribut("ea_guid", xml_String_send);

                                                                   if (m_End_ID != null)
                                                                   {
                                                                       //MessageBox.Show(m_End_ID.Count.ToString());

                                                                       int i5 = 0;
                                                                       do
                                                                       {
                                                                           EA.Element recent2 = repository.GetElementByID(repository.GetConnectorByGuid(m_End_ID[i5]).SupplierID);

                                                                           //Für die SChnittstellen aktuell
                                                                           if (Data.metamodel.Requirement_Interface_Stereotype == recent2.Stereotype)
                                                                           {
                                                                               this.Create_Link_Afo_Afo(Data.metamodel, links, repository, recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID, recent2.ElementGUID, Data.metamodel.m_Connectren_Require7[2]);
                                                                           }


                                                                           i5++;
                                                                       } while (i5 < m_End_ID.Count);
                                                                   }
                                                               }
                                                               */
                                                        #endregion Add_Send_Connector
                                                    }

                                                    #region Link AFo_Sys
                                                    //Link Afo und Sys hinzufügen
                                                    if (Data.link_afo_sys == true && Data.sys_xac == true && recent.RPI_Export == true)
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID, recent, null);
                                                    }
                                                    //Link Afo_Capability
                                                    if (Data.link_afo_cap == true && Data.capability_xac == true)
                                                    {
                                                        //Schleife über alle Capability einer AFo
                                                        if (recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].m_Capability.Count > 0)
                                                        {
                                                            int c1 = 0;
                                                            do
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID, null, recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].m_Capability[c1].Classifier_ID);
                                                                c1++;
                                                            } while (c1 < recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].m_Capability.Count);
                                                        }
                                                    }
                                                    //Link Afo_Logical
                                                    if (Data.link_afo_logical == true && Data.logical_xac == true)
                                                    {
                                                        List<Logical> m_Logical = recent.m_Element_Interface[i2].Get_Logicals_Unidirektional(repository, Data);
                                                        if (m_Logical != null)
                                                        {
                                                            int l1 = 0;
                                                            do
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                                l1++;
                                                            } while (l1 < m_Logical.Count);
                                                        }
                                                    }
                                                    #endregion Link AFo_Sys
                                                    #endregion Afo_Send zur xml DAtei ink. Konnektoren (send) hinzufügen
                                                }
                                            }
                                            #endregion Add_AFo_Send
                                            #region Add_AFo_Receive
                                             if (recent.m_Element_Interface[i2].m_Requirement_Interface_Receive.Count > 0)
                                             {
                                                if(recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].RPI_Export == true && recent.m_Element_Interface[i2].Supplier.RPI_Export == true)
                                                {
                                                    #region Afo_receive zur xml DAtei ink. Konnektoren (send) hinzufügen
                                                    //Afo_receive hinzufügen
                                                    if (this.m_GUID_Requirement.Contains(recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Classifier_ID) == false && Data.afo_interface_xac == true && this.m_Requirement.Contains(recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0]) == false)
                                                    {
                                                        this.m_GUID_Requirement.Add(recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Classifier_ID);
                                                        this.m_Requirement.Add(recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0]);
                                                        XElement recent_Afo_receive = this.Add_AFo(recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0], repository, Data.metamodel);
                                                        items.Add(recent_Afo_receive);
                                                        #region send bei receive nicht nöötig --> sogar flasch, da Beziehung verdreht wird
                                                        //Send Konnektor
                                                        //Bei receive ist das hier flasch, da er ja durch das sendende Element gesetzt wird
                                                        /*
                                                        if (Data.link_afo_afo == true)
                                                        {
                                                            string SQL_send = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = " + repository.GetElementByGuid(recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Instantiate_GUID).ElementID + " AND Stereotype = '" + Data.metamodel.StereoType_Send[0] + "' AND Connector_Type = '" + Data.metamodel.StereoType_Send[1] + "';";
                                                            string xml_String_send = repository.SQLQuery(SQL_send);
                                                            List<string> m_End_ID = xML.Xml_Read_Attribut("ea_guid", xml_String_send);

                                                            if (m_End_ID != null)
                                                            {
                                                                //MessageBox.Show(m_End_ID.Count.ToString());

                                                                int i5 = 0;
                                                                do
                                                                {
                                                                    EA.Element recent2 = repository.GetElementByID(repository.GetConnectorByGuid(m_End_ID[i5]).SupplierID);

                                                                    //Für die Schnittstellen aktuell
                                                                    if (Data.metamodel.StereoType_Requirement_Interface == recent2.Stereotype)
                                                                    {
                                                                        this.Create_Link_Afo_Afo(Data.metamodel, links, repository, recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Instantiate_GUID, recent2.ElementGUID, Data.metamodel.m_Connectren_Require7[2]);
                                                                    }


                                                                    i5++;
                                                                } while (i5 < m_End_ID.Count);
                                                            }
                                                        }
                                                        */
                                                        #endregion
                                                    }
                                                    if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Element_Interface[i2].Supplier.RPI_Export == true)
                                                    {
                                                        //Link Afo und Sys hinzufügen
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Classifier_ID, recent.m_Element_Interface[i2].Supplier, null);
                                                    }
                                                    //Link Afo_Capability
                                                    if (Data.link_afo_cap == true && Data.capability_xac == true)
                                                    {
                                                        //Schleife über alle Capability einer AFo
                                                        if (recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].m_Capability.Count > 0)
                                                        {
                                                            int c1 = 0;
                                                            do
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Classifier_ID, null, recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].m_Capability[c1].Classifier_ID);
                                                                c1++;
                                                            } while (c1 < recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].m_Capability.Count);
                                                        }
                                                    }
                                                    //Link Afo_Logical
                                                    if (Data.link_afo_logical == true && Data.logical_xac == true)
                                                    {
                                                        List<Logical> m_Logical = recent.m_Element_Interface[i2].Get_Logicals_Unidirektional(repository, Data);
                                                        if (m_Logical != null)
                                                        {
                                                            int l1 = 0;
                                                            do
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                                l1++;
                                                            } while (l1 < m_Logical.Count);
                                                        }
                                                    }
                                                    #endregion Afo_receive zur xml DAtei ink. Konnektoren (send) hinzufügen
                                                }

                                            }



                                                i2++;
                                            } while (i2 < recent.m_Element_Interface.Count) ;
                                            #endregion Add_AFo_Receive
                                        }


                                    
                                    #endregion Unidriektionale Schnittstellen betrachten
                                    //link bidirektionale SChnittstellen
                                    #region Bidriektonale Schnittstelle
                                    if (recent.m_Element_Interface_Bidirectional.Count > 0 && Data.afo_interface_xac == true && recent.RPI_Export == true)
                                    {
                                        #region Bidirektionale Schnittstellen betrachten
                                        loading.label2.Text = sys + " - Bidirektionale Schnittstellen";
                                        loading.label2.Refresh();

                                        int i3 = 0;
                                        do
                                        {

                                            if (recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send.Count > 0)
                                            {
                                                if(recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].RPI_Export == true)
                                                {
                                                    #region Afo_Send zur xml DAtei ink. Konnektoren (send) hinzufügen

                                                    //Afo_send hinzufügen
                                                    if (this.m_GUID_Requirement.Contains(recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID) == false && Data.afo_interface_xac == true && this.m_Requirement.Contains(recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0]) == false)
                                                    {
                                                        this.m_GUID_Requirement.Add(recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID);
                                                        this.m_Requirement.Add(recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0]);
                                                        XElement recent_Afo_bidi = this.Add_AFo(recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0], repository, Data.metamodel);
                                                        items.Add(recent_Afo_bidi);
                                                        //Send Konnektor
                                                        /*
                                                        if (Data.link_afo_afo == true)
                                                        {
                                                            string SQL_send = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = " + repository.GetElementByGuid(recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID).ElementID + " AND Stereotype IN" +xML.SQL_IN_Array( Data.metamodel.Send_Stereotype) + " AND Connector_Type IN" +xML.SQL_IN_Array( Data.metamodel.Send_Type) + ";";
                                                            string xml_String_send = repository.SQLQuery(SQL_send);
                                                            List<string> m_End_ID = xML.Xml_Read_Attribut("ea_guid", xml_String_send);

                                                            if (m_End_ID != null)
                                                            {
                                                                //MessageBox.Show(m_End_ID.Count.ToString());

                                                                int i5 = 0;
                                                                do
                                                                {
                                                                    EA.Element recent2 = repository.GetElementByID(repository.GetConnectorByGuid(m_End_ID[i5]).SupplierID);

                                                                    //Für die SChnittstellen aktuell
                                                                    if (Data.metamodel.Requirement_Interface_Stereotype == recent2.Stereotype)
                                                                    {
                                                                        this.Create_Link_Afo_Afo(Data.metamodel, links, repository, recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID, recent2.ElementGUID, Data.metamodel.m_Connectren_Require7[2]);
                                                                    }


                                                                    i5++;
                                                                } while (i5 < m_End_ID.Count);

                                                            }

                                                        }
                                                        */
                                                    }

                                                    if (Data.link_afo_sys == true && Data.sys_xac == true)
                                                    {
                                                        //Link Afo und Sys hinzufügen
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID, recent, null);
                                                    }
                                                    //Link Afo_Capability
                                                    if (Data.link_afo_cap == true && Data.capability_xac == true)
                                                    {
                                                        //Schleife über alle Capability einer AFo
                                                        if (recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].m_Capability.Count > 0)
                                                        {
                                                            int c1 = 0;
                                                            do
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID, null, recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].m_Capability[c1].Classifier_ID);
                                                                c1++;
                                                            } while (c1 < recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].m_Capability.Count);
                                                        }
                                                    }
                                                    //Link Afo_Logical
                                                    if (Data.link_afo_logical == true && Data.logical_xac == true)
                                                    {
                                                        List<Logical> m_Logical = recent.m_Element_Interface_Bidirectional[i3].Get_Logicals_Bidirektional(repository, Data);


                                                        if (m_Logical != null)
                                                        {
                                                            int l1 = 0;
                                                            do
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                                l1++;
                                                            } while (l1 < m_Logical.Count);
                                                        }

                                                    }
                                                    #endregion
                                                }

                                            }

                                            i3++;
                                        } while (i3 < recent.m_Element_Interface_Bidirectional.Count);

                                        #endregion
                                    }
                                    #endregion Bidriektonale Schnittstelle

                                    #region Funktionale Afo
                                    if (recent.m_Element_Functional.Count > 0 && Data.afo_funktional_xac == true && recent.RPI_Export == true)
                                    {
                                        int i4 = 0;
                                        do
                                        {
                                            if (recent.m_Element_Functional[i4].m_Requirement_Functional.Count > 0)
                                            {
                                                if(recent.m_Element_Functional[i4].m_Requirement_Functional[0].RPI_Export == true)
                                                {
                                                    if (this.m_GUID_Requirement.Contains(recent.m_Element_Functional[i4].m_Requirement_Functional[0].Classifier_ID) == false && this.m_Requirement.Contains(recent.m_Element_Functional[i4].m_Requirement_Functional[0]) == false)
                                                    {
                                                        this.m_GUID_Requirement.Add(recent.m_Element_Functional[i4].m_Requirement_Functional[0].Classifier_ID);
                                                        this.m_Requirement.Add(recent.m_Element_Functional[i4].m_Requirement_Functional[0]);
                                                        XElement recent_Afo_func = this.Add_AFo(recent.m_Element_Functional[i4].m_Requirement_Functional[0], repository, Data.metamodel);
                                                        items.Add(recent_Afo_func);
                                                    }


                                                    if (Data.link_afo_sys == true && Data.sys_xac == true && recent.RPI_Export == true)
                                                    {
                                                        //Link Afo und Sys hinzufügen
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Element_Functional[i4].m_Requirement_Functional[0].Classifier_ID, recent, null);
                                                    }
                                                    //Link Afo_Logical
                                                    if (Data.link_afo_logical == true && Data.logical_xac == true)
                                                    {
                                                        List<Logical> m_Logical = recent.m_Element_Functional[i4].m_Logical;


                                                        if (m_Logical != null)
                                                        {
                                                            if (m_Logical.Count > 0)
                                                            {
                                                                int l1 = 0;
                                                                do
                                                                {
                                                                    if (recent.m_Element_Functional[i4].m_Requirement_Functional.Count > 0)
                                                                    {
                                                                        Create_Link_Afo_Sys(links, repository, recent.m_Element_Functional[i4].m_Requirement_Functional[0].Classifier_ID, null, m_Logical[l1].Classifier_ID);
                                                                    }

                                                                    l1++;
                                                                } while (l1 < m_Logical.Count);
                                                            }

                                                        }

                                                    }
                                                    //Link Afo_Capability
                                                    if (Data.link_afo_cap == true && Data.capability_xac == true)
                                                    {
                                                        //Schleife über alle Capability einer AFo
                                                        if (recent.m_Element_Functional[i4].m_Requirement_Functional.Count > 0)
                                                        {
                                                            if (recent.m_Element_Functional[i4].m_Requirement_Functional[0].m_Capability.Count > 0)
                                                            {
                                                                int c1 = 0;
                                                                do
                                                                {
                                                                    Create_Link_Afo_Sys(links, repository, recent.m_Element_Functional[i4].m_Requirement_Functional[0].Classifier_ID, null, recent.m_Element_Functional[i4].m_Requirement_Functional[0].m_Capability[c1].Classifier_ID);
                                                                    c1++;
                                                                } while (c1 < recent.m_Element_Functional[i4].m_Requirement_Functional[0].m_Capability.Count);
                                                            }
                                                        }

                                                    }
                                                }
                                             
                                            }
                                       

                                            i4++;
                                        } while (i4 < recent.m_Element_Functional.Count);



                                    }
                                    #endregion Funktionale Afo

                                    #region  User Afo
                                    if (recent.m_Element_User.Count > 0 && Data.afo_user_xac == true && recent.RPI_Export == true)
                                    {
                                        int i4 = 0;
                                        do
                                        {
                                            if (recent.m_Element_User[i4].m_Requirement_User.Count > 0)
                                            {
                                                int i5 = 0;
                                                do
                                                {
                                                    if(recent.m_Element_User[i4].m_Requirement_User[i5].RPI_Export == true)
                                                    {
                                                        if (this.m_GUID_Requirement.Contains(recent.m_Element_User[i4].m_Requirement_User[i5].Classifier_ID) == false && this.m_Requirement.Contains(recent.m_Element_User[i4].m_Requirement_User[i5]) == false)
                                                        {
                                                            this.m_GUID_Requirement.Add(recent.m_Element_User[i4].m_Requirement_User[i5].Classifier_ID);
                                                            this.m_Requirement.Add(recent.m_Element_User[i4].m_Requirement_User[i5]);
                                                            XElement recent_Afo_func = this.Add_AFo(recent.m_Element_User[i4].m_Requirement_User[i5], repository, Data.metamodel);
                                                            items.Add(recent_Afo_func);
                                                        }


                                                        if (Data.link_afo_sys == true && Data.sys_xac == true && recent.RPI_Export == true)
                                                        {
                                                            //Link Afo und Sys hinzufügen
                                                            Create_Link_Afo_Sys(links, repository, recent.m_Element_User[i4].m_Requirement_User[i5].Classifier_ID, recent, null);
                                                        }
                                                    }
                                                  

                                                    i5++;
                                                } while (i5 < recent.m_Element_User[i4].m_Requirement_User.Count);
                                            }
                                            //Link Afo_Logical
                                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                                            {
                                                List<Logical> m_Logical = recent.m_Element_User[i4].m_Logical;


                                                if (m_Logical != null)
                                                {
                                                    if (m_Logical.Count > 0)
                                                    {
                                                        int l1 = 0;
                                                        do
                                                        {
                                                            if (recent.m_Element_User[i4].m_Requirement_User.Count > 0)
                                                            {
                                                                int i6 = 0;
                                                                do
                                                                {
                                                                    if(recent.m_Element_User[i4].m_Requirement_User[i6].RPI_Export == true)
                                                                    {
                                                                        Create_Link_Afo_Sys(links, repository, recent.m_Element_User[i4].m_Requirement_User[i6].Classifier_ID, null, m_Logical[l1].Classifier_ID);
                                                                    }

                                                                    i6++;
                                                                } while (i6 < recent.m_Element_User[i4].m_Requirement_User.Count);
                                                            }

                                                            l1++;
                                                        } while (l1 < m_Logical.Count);
                                                    }

                                                }

                                            }
                                            //Link Afo_Capability
                                            if (Data.link_afo_cap == true && Data.capability_xac == true)
                                            {
                                                //Schleife über alle Capability einer AFo
                                                if (recent.m_Element_User[i4].m_Requirement_User.Count > 0)
                                                {
                                                    int i7 = 0;
                                                    do
                                                    {
                                                        if (recent.m_Element_User[i4].m_Requirement_User[i7].m_Capability.Count > 0)
                                                        {
                                                            int c1 = 0;
                                                            do
                                                            {
                                                                if(recent.m_Element_User[i4].m_Requirement_User[i7].RPI_Export == true)
                                                                {
                                                                    Create_Link_Afo_Sys(links, repository, recent.m_Element_User[i4].m_Requirement_User[i7].Classifier_ID, null, recent.m_Element_User[i4].m_Requirement_User[i7].m_Capability[c1].Classifier_ID);

                                                                }
                                                                c1++;
                                                            } while (c1 < recent.m_Element_User[i4].m_Requirement_User[i7].m_Capability.Count);
                                                        }

                                                        i7++;
                                                    } while (i7 < recent.m_Element_User[i4].m_Requirement_User.Count);
                                                }

                                            }


                                            //Link Afo --> Stakeholder
                                            if (Data.link_afo_st == true && Data.stakeholder_xac == true)
                                            {
                                                //Schleife über alle Stakeholder einer AFo
                                                if (recent.m_Element_User[i4].m_Requirement_User.Count > 0)
                                                {
                                                    int i7 = 0;
                                                    do
                                                    {
                                                        if (recent.m_Element_User[i4].m_Client_ST.Count > 0)
                                                        {
                                                            int c1 = 0;
                                                            do
                                                            {
                                                                if (repository_Connector.Check_Dependency(recent.m_Element_User[i4].m_Requirement_User[i7].Classifier_ID, recent.m_Element_User[i4].m_Client_ST[c1].Classifier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data, Data.metamodel.m_Derived_Element[0].direction) != null)
                                                                {
                                                                    if(recent.m_Element_User[i4].m_Requirement_User[i7].RPI_Export == true)
                                                                    {
                                                                        Create_Link_Afo_St(links, repository, recent.m_Element_User[i4].m_Requirement_User[i7].Classifier_ID, null, recent.m_Element_User[i4].m_Client_ST[c1].Classifier_ID, Data.metamodel);
                                                                    }
                                                                }
                                                                c1++;
                                                            } while (c1 < recent.m_Element_User[i4].m_Client_ST.Count);
                                                        }

                                                        i7++;
                                                    } while (i7 < recent.m_Element_User[i4].m_Requirement_User.Count);
                                                }
                                            }

                                            i4++;
                                        } while (i4 < recent.m_Element_User.Count);



                                    }
                                    #endregion User AFo

                                    #region Design AFO
                                    if (recent.m_Design.Count > 0 && Data.afo_design_xac == true && recent.RPI_Export == true)
                                    {
                                        int d1 = 0;
                                        do
                                        {
                                            if(recent.m_Design[d1].requirement != null)
                                            {
                                                #region Add Afo
                                                if (recent.m_Design[d1].requirement.Classifier_ID != null && recent.m_Design[d1].requirement.RPI_Export == true)
                                                {
                                                    this.m_GUID_Requirement.Add(recent.m_Design[d1].requirement.Classifier_ID);
                                                    this.m_Requirement.Add(recent.m_Design[d1].requirement);
                                                    XElement recent_Design = this.Add_AFo(recent.m_Design[d1].requirement, repository, Data.metamodel);
                                                    items.Add(recent_Design);
                                                }
                                                #endregion Add AFO
                                                #region Add Link afo_sys
                                                if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Design[d1].requirement.RPI_Export == true && recent.RPI_Export == true)
                                                {
                                                    //Link Afo und Sys hinzufügen
                                                    Create_Link_Afo_Sys(links, repository, recent.m_Design[d1].requirement.Classifier_ID, recent.m_Design[d1].NodeType, null);
                                                }
                                                #endregion Add Link afo_sys
                                                //Link Afo_Capability
                                                #region Add Link afo_cap
                                                if (Data.link_afo_cap == true && Data.capability_xac == true && recent.m_Design[d1].requirement.RPI_Export == true)
                                                {
                                                    //Schleife über alle Capability einer AFo
                                                    if (recent.m_Design[d1].capability != null)
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Design[d1].requirement.Classifier_ID, null, recent.m_Design[d1].capability.Classifier_ID);

                                                    }
                                                }
                                                #endregion Add Link afo_cap
                                                //Link Afo_Logical
                                                #region Add Link afo_log
                                                if (Data.link_afo_logical == true && Data.logical_xac == true && recent.m_Design[d1].requirement.RPI_Export == true)
                                                {
                                                    List<Logical> m_Logical = recent.m_Design[d1].m_Logical;
                                                    if (m_Logical.Count > 0)
                                                    {
                                                        int l1 = 0;
                                                        do
                                                        {
                                                            Create_Link_Afo_Sys(links, repository, recent.m_Design[d1].requirement.Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                            l1++;
                                                        } while (l1 < m_Logical.Count);
                                                    }
                                                }
                                                #endregion Add Link afo_sys
                                            }


                                            d1++;
                                        } while (d1 < recent.m_Design.Count);

                                    }
                                    #endregion Design AFO

                                    #region Process AFO
                                    if (recent.m_Element_Functional.Count > 0 && Data.afo_process_xac == true && recent.RPI_Export == true)
                                    {
                                        int d1 = 0;
                                        do
                                        {
                                            List<OperationalConstraint> m_opcon = recent.m_Element_Functional[d1].Supplier.Get_Process_Constraint(recent);

                                            if(m_opcon.Count> 0)
                                            {
                                                int o1 = 0;
                                                do
                                                {

                                                    List<Requirement_Non_Functional> m_req = recent.m_Element_Functional[d1].Supplier.Element_Process_Get_Requirement(recent, m_opcon[o1] );

                                                    if (m_req.Count > 0)
                                                    {
                                                        int d2 = 0;
                                                        do
                                                        {
                                                            #region Add Afo
                                                            if (m_req[d2].Classifier_ID != null && m_req[d2].RPI_Export == true)
                                                            {
                                                                this.m_GUID_Requirement.Add(m_req[d2].Classifier_ID);
                                                                this.m_Requirement.Add(m_req[d2]);
                                                                XElement recent_Process = this.Add_AFo(m_req[d2], repository, Data.metamodel);
                                                                items.Add(recent_Process);

                                                                #region Add Link afo_sys
                                                                if (Data.link_afo_sys == true && Data.sys_xac == true && m_req[d2].RPI_Export == true && recent.RPI_Export == true)
                                                                {
                                                                    //Link Afo und Sys hinzufügen
                                                                    Create_Link_Afo_Sys(links, repository, m_req[d2].Classifier_ID, recent, null);
                                                                }
                                                                #endregion Add Link afo_sys

                                                                //Link Afo_Capability
                                                                #region Add Link afo_cap
                                                                if (Data.link_afo_cap == true && Data.capability_xac == true && m_req[d2].RPI_Export == true)
                                                                {
                                                                    //Schleife über alle Capability einer AFo
                                                                    if (m_req[d2].m_Capability.Count > 0)
                                                                    {
                                                                        Create_Link_Afo_Sys(links, repository, m_req[d2].Classifier_ID, null, m_req[d2].m_Capability[0].Classifier_ID);

                                                                    }
                                                                }
                                                                #endregion Add Link afo_cap

                                                                //Link Afo_Logical
                                                                #region Add Link afo_log
                                                                if (Data.link_afo_logical == true && Data.logical_xac == true && m_req[d2].RPI_Export == true)
                                                                {
                                                                    List<Logical> m_Logical = recent.m_Element_Functional[d1].m_Logical;
                                                                    if (m_Logical.Count > 0)
                                                                    {
                                                                        int l1 = 0;
                                                                        do
                                                                        {
                                                                            Create_Link_Afo_Sys(links, repository, m_req[d2].Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                                            l1++;
                                                                        } while (l1 < m_Logical.Count);
                                                                    }
                                                                }
                                                                #endregion Add Link afo_sys
                                                            }
                                                            #endregion Add AFO

                                                            d2++;
                                                        } while (d2 < m_req.Count);
                                                    }

                                                    o1++;
                                                } while (o1 < m_opcon.Count);
                                            }

                                           

                                         

                                          
                                            d1++;
                                        } while (d1 < recent.m_Element_Functional.Count);

                                    }
                                    #endregion Process AFO

                                    #region Umwelt Afo
                                    if (recent.m_Enviromental.Count > 0 && Data.afo_umwelt_xac == true && recent.RPI_Export == true)
                                    {
                                        int d1 = 0;
                                        do
                                        {
                                            #region Add Afo
                                            if(recent.m_Enviromental[d1].requirement != null)
                                            {
                                                if (recent.m_Enviromental[d1].requirement.RPI_Export == true)
                                                {
                                                    this.m_GUID_Requirement.Add(recent.m_Enviromental[d1].requirement.Classifier_ID);
                                                    this.m_Requirement.Add(recent.m_Enviromental[d1].requirement);
                                                    XElement recent_Umwelt = this.Add_AFo(recent.m_Enviromental[d1].requirement, repository, Data.metamodel);
                                                    items.Add(recent_Umwelt);
                                                    #region Add Link afo_sys
                                                    if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Enviromental[d1].requirement.RPI_Export == true && recent.RPI_Export == true)
                                                    {
                                                        //Link Afo und Sys hinzufügen
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Enviromental[d1].requirement.Classifier_ID, recent.m_Enviromental[d1].NodeType, null);
                                                    }
                                                    #endregion Add Link afo_sys
                                                    //Link Afo_Capability
                                                    #region Add Link afo_cap
                                                    if (Data.link_afo_cap == true && Data.capability_xac == true && recent.m_Enviromental[d1].requirement.RPI_Export == true)
                                                    {
                                                        //Schleife über alle Capability einer AFo
                                                        if (recent.m_Enviromental[d1].capability != null)
                                                        {
                                                            Create_Link_Afo_Sys(links, repository, recent.m_Enviromental[d1].requirement.Classifier_ID, null, recent.m_Enviromental[d1].capability.Classifier_ID);

                                                        }
                                                    }
                                                    #endregion Add Link afo_cap
                                                    //Link Afo_Logical
                                                    #region Add Link afo_log
                                                    if (Data.link_afo_logical == true && Data.logical_xac == true && recent.m_Enviromental[d1].requirement.RPI_Export == true)
                                                    {
                                                        List<Logical> m_Logical = recent.m_Enviromental[d1].m_Logical;
                                                        if (m_Logical.Count > 0)
                                                        {
                                                            int l1 = 0;
                                                            do
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Enviromental[d1].requirement.Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                                l1++;
                                                            } while (l1 < m_Logical.Count);
                                                        }
                                                    }
                                                    #endregion Add Link afo_sys
                                                }
                                            }

                                           
                                            #endregion Add AFO


                                            d1++;
                                        } while (d1 < recent.m_Enviromental.Count);

                                    }
                                    #endregion Umwelt Afo

                                    #region Typevertreter_AFO
                                    if (recent.m_Typvertreter.Count > 0 && Data.afo_typevertreter_xac == true && recent.RPI_Export == true)
                                    {
                                        int d1 = 0;
                                        do
                                        {
                                            if(recent.m_Typvertreter[d1].requirement != null)
                                            {
                                                #region Add Afo
                                                if (recent.m_Typvertreter[d1].requirement.Classifier_ID != null && recent.m_Typvertreter[d1].requirement.RPI_Export == true)
                                                {
                                                    this.m_GUID_Requirement.Add(recent.m_Typvertreter[d1].requirement.Classifier_ID);
                                                    this.m_Requirement.Add(recent.m_Typvertreter[d1].requirement);
                                                    XElement recent_Type = this.Add_AFo(recent.m_Typvertreter[d1].requirement, repository, Data.metamodel);
                                                    items.Add(recent_Type);
                                                }
                                                #endregion Add AFO
                                                #region Add Link afo_sys
                                                if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Typvertreter[d1].requirement.RPI_Export == true && recent.RPI_Export == true)
                                                {
                                                    //Link Afo und Sys hinzufügen
                                                    Create_Link_Afo_Sys(links, repository, recent.m_Typvertreter[d1].requirement.Classifier_ID, recent.m_Typvertreter[d1].NodeType, null);
                                                }
                                                #endregion Add Link afo_sys
                                                //Link Afo_Capability
                                                #region Add Link afo_cap
                                                if (Data.link_afo_cap == true && Data.capability_xac == true && recent.m_Typvertreter[d1].requirement.RPI_Export == true)
                                                {
                                                    //Schleife über alle Capability einer AFo
                                                    if (recent.m_Typvertreter[d1].capability != null)
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Typvertreter[d1].requirement.Classifier_ID, null, recent.m_Typvertreter[d1].capability.Classifier_ID);

                                                    }
                                                }
                                                #endregion Add Link afo_cap
                                                //Link Afo_Logical
                                                #region Add Link afo_log
                                                if (Data.link_afo_logical == true && Data.logical_xac == true && recent.m_Typvertreter[d1].requirement.RPI_Export == true)
                                                {
                                                    List<Logical> m_Logical = recent.m_Typvertreter[d1].m_Logical;
                                                    if (m_Logical.Count > 0)
                                                    {
                                                        int l1 = 0;
                                                        do
                                                        {
                                                            Create_Link_Afo_Sys(links, repository, recent.m_Typvertreter[d1].requirement.Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                            l1++;
                                                        } while (l1 < m_Logical.Count);
                                                    }
                                                }
                                                #endregion Add Link afo_sys
                                            }


                                            d1++;
                                        } while (d1 < recent.m_Typvertreter.Count);

                                    }
                                    #endregion Typevertret_ AFO
                                }


                            }

                            loading.progressBar1.PerformStep();
                            loading.progressBar1.Refresh();

                            i1++;
                        } while (i1 < Data.Decomposition.m_NodeType.Count);
                    }
                }
              

                loading.progressBar1.Value = Data.m_NodeType.Count;
                loading.progressBar1.Refresh();

            }

            #endregion
            ///////////////////////////////////
            //Wurde in die Decomposition mit verschoben, da hier Alle Afo exportiet werden 
            //--> nicht der Gedanke dieses Exportes, da "Szenar weise xporitert werden soll
            #region Afo zu item hinzufügen
           
            #endregion Afo zu item hinzufügen
            ///////////////////////////////////
            //Szenarbaum hinzufügen
            #region Add_Szenar
            if (Data.logical_xac == true)
            {
                //Root Szenar einfügen
                XElement Root_Szenar = this.Add_Sys_Root(Data, Data.metamodel.m_Header_agid[1], Data.SYS_ENUM.SYS_TYP[1],"Systemelement", "Szenarbaum", "der", Data.metamodel.m_Header[1], Data.metamodel.m_object_id[1]);

                items.Add(Root_Szenar);

                if(Data.m_Logical.Count > 0)
                {
                    ////////////////////
                    ///Festlegen Werte Ladebalken
                    loading.label_Progress.Text = "Szenarbaum";
                    loading.label2.Text = "Szenarbaum";
                    loading.progressBar1.Value = 0;
                    loading.progressBar1.Maximum = Data.m_Logical.Count;
                    loading.progressBar1.Refresh();

                    int i2 = 0;
                    do
                    {

                        loading.label2.Text = Data.m_Logical[i2].Get_Name(this.Data);
                        loading.label2.Refresh();

                        XElement recent_Logical = Add_Logical(Data.m_Logical[i2], repository, Data.metamodel);

                        items.Add(recent_Logical);

                        //Link zu Szenar_Header
                        Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[1], repository.GetElementByGuid(Data.m_Logical[i2].Classifier_ID).ElementID.ToString());


                        loading.progressBar1.PerformStep();
                        loading.Refresh();

                        i2++;
                    } while (i2 < Data.m_Logical.Count);
                }
            }
            #endregion
            ///////////////////////////////////
            //Funktionsbaum hinzufügen
            #region Add_Funktionsbaum
            if(Data.capability_xac == true)
            {
                //Root Szenar einfügen
                XElement Root_Capability = this.Add_Sys_Root(Data, Data.metamodel.m_Header_agid[2], Data.SYS_ENUM.SYS_TYP[0], "Systemelement", Data.metamodel.m_Header[2], "der", Data.metamodel.m_Header[3], Data.metamodel.m_object_id[2]);

                items.Add(Root_Capability);

                if(Data.m_Capability.Count > 0)
                {
                    ////////////////////
                    ///Festlegen Werte Ladebalken
                    loading.label_Progress.Text = "Funktionsbaum";
                    loading.label2.Text = "Funktionsbaum";
                    loading.progressBar1.Value = 0;
                    loading.progressBar1.Maximum = Data.m_Capability.Count;
                    loading.progressBar1.Refresh();

                    int i3 = 0;
                    do
                    {
                        loading.label2.Text = Data.m_Capability[i3].Get_Name( this.Data);
                        loading.label2.Refresh();

                        XElement recent_Capability = Add_Funktionsbaum(Data.m_Capability[i3], repository, Data.metamodel);

                        items.Add(recent_Capability);

                        //Connecotr zum Header erzeugen
                        if(Data.m_Capability[i3].m_Parent.Count == 0)
                        {
                            //Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[2], repository.GetElementByGuid(Data.m_Capability[i3].Classifier_ID).ElementID.ToString());
                            Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[2], Data.m_Capability[i3].OBJECT_ID.ToString());
                        }
                        else
                        {
                            //Taxonomy der Capability aufbauen
                            //reate_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], repository.GetElementByGuid(Data.m_Capability[i3].m_Parent[0].Classifier_ID).ElementID.ToString(), repository.GetElementByGuid(Data.m_Capability[i3].Classifier_ID).ElementID.ToString());
                            Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.m_Capability[i3].m_Parent[0].OBJECT_ID.ToString(), Data.m_Capability[i3].OBJECT_ID.ToString());

                        }
                        /*
                        //Link zu Szenar_Header
                        Create_Link_Sys(Data, links, repository, null, null, Data.metamodel.m_Connectren_Require7[0], Data.metamodel.m_object_id[1], repository.GetElementByGuid(Data.m_Logical[i2].Logical_ID).ElementID.ToString());
                        */

                        loading.progressBar1.PerformStep();
                        loading.Refresh();

                        i3++;
                    } while (i3 < Data.m_Capability.Count);

                }

            }


            #endregion
            ///////////////////////////////////
            //Stakeholder hinzufügen
            #region Add Stakeholder
            //Stakeholder
            if (Data.stakeholder_xac == true)
            {
                if (Data.m_Stakeholder.Count > 0)
                {
                    ////////////////////
                    ///Festlegen Werte Ladebalken
                    loading.label_Progress.Text = "Stakeholder";
                    loading.label2.Text = "Stakeholder";
                    loading.progressBar1.Value = 0;
                    loading.progressBar1.Maximum = Data.m_Stakeholder.Count;
                    loading.progressBar1.Refresh();

                    int s1 = 0;
                    do
                    {
                        XElement recent_ST =  this.Add_Stakeholder(Data.m_Stakeholder[s1], repository, Data.metamodel);

                        items.Add(recent_ST);

                        loading.progressBar1.PerformStep();
                        loading.Refresh();

                        s1++;
                    } while (s1 < Data.m_Stakeholder.Count);
                }
            }
            #endregion Add Stakeholder
            ////////////////////////////////////
            ///Links zwischen AFo hinzufügen
            #region Export Afo_Afo_Links
            if (Data.link_afo_afo == true)
            {
                if(this.m_Requirement.Count > 0)
                {
                    int r1 = 0;
                    do
                    {
                        if(this.m_Requirement[r1].m_Requirement_Conflict.Count > 0)
                        {
                            Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[0], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Conflict, links, Data.metamodel, repository);
                        }
                        if(this.m_Requirement[r1].m_Requirement_Duplicate.Count > 0)
                        {
                            Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[1], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Duplicate, links, Data.metamodel, repository);
                        }
                        if (this.m_Requirement[r1].m_Requirement_Refines.Count > 0)
                        {
                            Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[2], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Refines, links, Data.metamodel, repository);
                        }
                        if (this.m_Requirement[r1].m_Requirement_Replace.Count > 0)
                        {
                            Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[3], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Replace, links, Data.metamodel, repository);
                        }
                        if (this.m_Requirement[r1].m_Requirement_Requires.Count > 0)
                        {
                            Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[4], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Requires, links, Data.metamodel, repository);
                        }
                        if (this.m_Requirement[r1].m_Requirement_InheritsFrom.Count > 0)
                        {
                            Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[5], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_InheritsFrom, links, Data.metamodel, repository);
                        }

                        r1++;
                    } while (r1 < this.m_Requirement.Count);
                }
            }
            #endregion Export Afo_Afo_Links
            ///////////////////////////////////
            //Glossar Element hinzufügen
            #region Add GlossarElement
            if (Data.glossary_xac == true)
            {
                List<string> m_Glossar_ID = repository_Elements.Get_Glossar_Element_IDS(Data);

                if(m_Glossar_ID != null)
                {
                    int g1 = 0;
                    do
                    {
                        Glossar_Element recent = new Glossar_Element(m_Glossar_ID[g1], Data);

                        XElement recent_gloe = this.Add_GlossarElement(recent, repository, Data.metamodel);

                        items.Add(recent_gloe);

                     
                        g1++;
                    } while (g1 < m_Glossar_ID.Count);
                }
            }
            #endregion AddGlosarElement
            //Abspeichern
            #region Save
            string xac_Header = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
            string xml_string = xml_dat.ToString();


            ////////////////////
            ///Festlegen Werte Ladebalken
            loading.label_Progress.Text = "Speichern xac-File";
            loading.label2.Text = "Speichern xac-File";
            loading.progressBar1.Value = 0;
            loading.progressBar1.Maximum = 1;
            loading.progressBar1.Refresh();


            //MessageBox.Show(xml_string);

            //xml_dat.Save(filename);
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.WriteLine(xac_Header);
            file.WriteLine(xml_string);
            file.Close();

            loading.progressBar1.PerformStep();
            loading.label2.Text = "Speichern abgeschlossen";

            loading.Close();
            #endregion
        }
       
        public void Export_flat_xac(Database Data, EA.Repository repository, Loading_OpArch loading, string filename)
        {
           

            ///////////////////////////////////
            ///Ladebalken
            loading.label2.Text = "Export Elements as xac";
            loading.label2.Refresh();
            loading.label_Progress.Text = "Generate xml-Header";
            loading.label_Progress.Refresh();
            loading.progressBar1.Step = 1;
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = 1;
            loading.progressBar1.Refresh();
            ////////////////////////////////////
            //Erstellung XMLDatei
            #region Add_Header
            var xml_dat = new XDocument(
                 new XDeclaration("1.0", "UTF-8", "yes")
                // new XComment("Kommentar")
                );
            var Root = new XElement("xac");
            var Att_revision = new XAttribute("revision", "3");
            var Att_itemspec = new XAttribute("itemspec", "");
            var Att_description = new XAttribute("description", "");
            var Att_lastmodiefiedby = new XAttribute("lastmodifiedby", Data.metamodel.createdby);//Autor eintragen?
            //Uhrziet und Datum
            string date = DateTime.Now.ToString("o");
            var Att_lastmodifieddate = new XAttribute("lastmodifieddate", date);
            var Att_CreatedwithWv = new XAttribute("createdWithWv", "Analysephase 09.05.2017 16:34");
            var Att_createdwith = new XAttribute("createdWith", "Require.7:V2.3.2031");
            var Att_source = new XAttribute("source", "");
            //Hinzufügen der Attribute
            Root.Add(Att_revision);
            Root.Add(Att_itemspec);
            Root.Add(Att_description);
            Root.Add(Att_lastmodiefiedby);
            Root.Add(Att_lastmodifieddate);
            Root.Add(Att_CreatedwithWv);
            Root.Add(Att_createdwith);
            Root.Add(Att_source);

            xml_dat.Add(Root);

            loading.progressBar1.PerformStep();
            loading.progressBar1.Refresh();
            #endregion
            ///////////////////////////////////
            //items Knoten hinzufügen ..> Anforderungen, Systemelemente
            loading.label_Progress.Text = "Generate xml-item & link";
            loading.progressBar1.Minimum = 0;
            loading.progressBar1.Maximum = 1;
            loading.progressBar1.Value = 0;
            loading.Refresh();

            XElement items = new XElement("items");
            Root.Add(items);
            //Links Knoten hinzufügen
            XElement links = new XElement("links");
            Root.Add(links);
            ////////////////////////////////////////////////////////////
            #region Add_Capability
            if(Data.m_Capability != null)
            {
                if (Data.m_Capability.Count > 0 && Data.capability_xac == true)
                {
                    loading.label_Progress.Text = "Add Capability";
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = Data.m_Capability.Count;
                    loading.progressBar1.Value = 0;
                    loading.Refresh();

                    //Root Szenar einfügen
                    XElement Root_Capability = this.Add_Sys_Root(Data, Data.metamodel.m_Header_agid[2], Data.SYS_ENUM.SYS_TYP[0], "Systemelement", Data.metamodel.m_Header[2], "der", Data.metamodel.m_Header[3], Data.metamodel.m_object_id[2]);

                    items.Add(Root_Capability);


                    Parallel.ForEach(Data.m_Capability, capability =>
                    {

                        try
                        {
                            this.m_Capability.Add(capability);
                            XElement recent_Cap = this.Add_Funktionsbaum(capability, repository, Data.metamodel);
                            items.Add(recent_Cap);
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("Capability: " + loading.label2.Text + ": " + err.Message);
                        }
                    });
        
                  /*  int i1 = 0;
                    do
                    {
                        //if (Data.m_Capability[i1].RPI_Export == true)
                        //   {
                        try
                        {
                            this.m_Capability.Add(Data.m_Capability[i1]);
                            XElement recent_Cap = this.Add_Funktionsbaum(Data.m_Capability[i1], repository, Data.metamodel);
                            items.Add(recent_Cap);
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("Capability: "+ loading.label2.Text + ": " + err.Message);
                        }
                       
                        //    }

                        loading.progressBar1.PerformStep();
                        loading.progressBar1.Refresh();

                        i1++;
                    } while (i1 < Data.m_Capability.Count);
                  */
                }
            }
         
            #endregion Add Capability

            #region Add Cap_Cap_Links
            if(Data.m_Capability != null)
            {
                if (this.m_Capability.Count > 0 && Data.link_decomposition == true)
                {
                    loading.label_Progress.Text = "Add Capability Links";
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = Data.m_Capability.Count;
                    loading.progressBar1.Value = 0;
                    loading.Refresh();

                    Parallel.ForEach(this.m_Capability, capability =>
                    {
                        try
                        {
                            if(capability != null)
                            {
                                //Connecotr zum Header erzeugen
                                if (capability.m_Parent.Count == 0)
                                {
                                    //Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[2], repository.GetElementByGuid(capability.Classifier_ID).ElementID.ToString());
                                    Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[2], capability.OBJECT_ID.ToString());
                                }
                                else
                                {
                                    //Taxonomy der Capability aufbauen
                                    //Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], repository.GetElementByGuid(capability.m_Parent[0].Classifier_ID).ElementID.ToString(), repository.GetElementByGuid(capability.Classifier_ID).ElementID.ToString());
                                    Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], capability.m_Parent[0].OBJECT_ID.ToString(), capability.OBJECT_ID.ToString());
                                }
                            }
                            
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("Capability Link: " + loading.label2.Text + ": " + err.Message);
                        }
                    });

                    /*      int i1 = 0;

                      do
                        {
                            try
                            {
                                //Connecotr zum Header erzeugen
                                if (this.m_Capability[i1].m_Parent.Count == 0)
                                {
                                    Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[2], repository.GetElementByGuid(this.m_Capability[i1].Classifier_ID).ElementID.ToString());
                                }
                                else
                                {
                                    //Taxonomy der Capability aufbauen
                                    Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], repository.GetElementByGuid(this.m_Capability[i1].m_Parent[0].Classifier_ID).ElementID.ToString(), repository.GetElementByGuid(this.m_Capability[i1].Classifier_ID).ElementID.ToString());
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Capability Link: " + loading.label2.Text + ": " + err.Message);
                            }

                            i1++;
                        } while (i1 < this.m_Capability.Count);
                */
                }
            }
           
            #endregion
            /////////////////////////////////////////////////////////
            ///////////////////////////////////
            //Szenarbaum hinzufügen
            #region Add_Szenar
            if (Data.logical_xac == true)
            {
                //Root Szenar einfügen
                XElement Root_Szenar = this.Add_Sys_Root(Data, Data.metamodel.m_Header_agid[1], Data.SYS_ENUM.SYS_TYP[1], "Systemelement", "Szenarbaum", "der", Data.metamodel.m_Header[1], Data.metamodel.m_object_id[1]);

                items.Add(Root_Szenar);

                if(Data.m_Logical != null)
                {
                    if (Data.m_Logical.Count > 0)
                    {
                        ////////////////////
                        ///Festlegen Werte Ladebalken
                        loading.label_Progress.Text = "Szenarbaum";
                        loading.label2.Text = "Szenarbaum";
                        loading.progressBar1.Value = 0;
                        loading.progressBar1.Maximum = Data.m_Logical.Count;
                        loading.progressBar1.Refresh();

                        Parallel.ForEach(Data.m_Logical, logical =>
                        {
                            try
                            {
                                XElement recent_Logical = Add_Logical(logical, repository, Data.metamodel);

                                items.Add(recent_Logical);

                                //Link zu Szenar_Header
                                // Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[1], repository.GetElementByGuid(logical.Classifier_ID).ElementID.ToString());
                                Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[1], logical.OBJECT_ID.ToString());

                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Szenar: " + loading.label2.Text + ": " + err.Message);
                            }
                        });
                        /*
                        int i2 = 0;
                        do
                        {

                            loading.label2.Text = Data.m_Logical[i2].Get_Name(this.Data);
                            loading.label2.Refresh();

                            try
                            {
                                XElement recent_Logical = Add_Logical(Data.m_Logical[i2], repository, Data.metamodel);

                                items.Add(recent_Logical);

                                //Link zu Szenar_Header
                                Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[1], repository.GetElementByGuid(Data.m_Logical[i2].Classifier_ID).ElementID.ToString());


                                loading.progressBar1.PerformStep();
                                loading.Refresh();
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Szenar: "+loading.label2.Text + ": " + err.Message);
                            }

                            i2++;
                        } while (i2 < Data.m_Logical.Count);
                        */
                    }
                }

               
            }
            #endregion
            //////////////////////////////////
            //Technisches System hinzufügen
            #region Add System
            if(Data.sys_xac == true)
            {
                if (Data.m_NodeType != null)
                {
                    ///Festlegen Werte Ladebalken
                    loading.label_Progress.Text = "Technisches System";
                    loading.label2.Text = "Technisches System";
                    loading.progressBar1.Value = 0;

                    switch (Data.metamodel.modus)
                    {
                        case 0:
                            #region NodeType
                            loading.progressBar1.Maximum = Data.m_NodeType.Count;
                            loading.progressBar1.Refresh();
                            if (Data.m_NodeType.Count > 0)
                            {
                                //L <Logisches Decomposition erstellen>
                                XElement Log_Dec = this.Add_Sys_Root(Data, Data.metamodel.m_Header_agid[0], Data.SYS_ENUM.SYS_TYP[2], "Systemelement", "<LogischeDecomposition>", "die", Data.metamodel.m_Header[0], Data.metamodel.m_object_id[0]);
                                items.Add(Log_Dec);
                                this.m_NodeType = new List<NodeType>();

                                Parallel.ForEach(Data.m_NodeType, nodetype =>
                                {
                                    try
                                    {
                                        if (nodetype.RPI_Export == true)
                                        {
                                            this.m_NodeType.Add(nodetype);
                                            XElement recent_Cap = this.Add_Sys(nodetype, repository, Data.metamodel);
                                            items.Add(recent_Cap);
                                        }
                                    }
                                    catch (Exception err)
                                    {
                                        MessageBox.Show("Technisches System: " + loading.label2.Text + ": " + err.Message);
                                    }
                                });
                            }
                            #endregion
                            break;
                        case 1:
                            #region SysElement
                            loading.progressBar1.Maximum = Data.m_SysElemente.Count;
                            loading.progressBar1.Refresh();
                            if (Data.m_SysElemente.Count > 0)
                            {
                                //L <Logisches Decomposition erstellen>
                                XElement Log_Dec = this.Add_Sys_Root(Data, Data.metamodel.m_Header_agid[0], Data.SYS_ENUM.SYS_TYP[2], "Systemelement", "<TechnischeDecomposition>", "die", Data.metamodel.m_Header[0], Data.metamodel.m_object_id[0]);
                                items.Add(Log_Dec);
                                this.m_SysElem = new List<SysElement>();
                                this.m_SysElem_Real = new List<SysElement>();

                                Parallel.ForEach(Data.m_SysElemente, nodetype =>
                                {
                                    try
                                    {
                                        if (nodetype.RPI_Export == true)
                                        {
                                            this.m_SysElem.Add(nodetype);
                                            XElement recent_Cap = this.Add_Sys(nodetype, repository, Data.metamodel);
                                            items.Add(recent_Cap);
                                        }
                                    }
                                    catch (Exception err)
                                    {
                                        MessageBox.Show("Technisches System: " + loading.label2.Text + ": " + err.Message);
                                    }
                                });
                            }
                            #endregion
                            break;
                    }
                }

            }
            #endregion
            /////////////////////////////////
            //Dekomposition anlegen
            #region Add Link Sys
            if(Data.link_decomposition == true)
            {
                switch(Data.metamodel.modus)
                {
                    case 0:
                        if(this.m_NodeType != null)
                        {
                            if (this.m_NodeType.Count > 0)
                            {
                                Parallel.ForEach(this.m_NodeType, nodetype =>
                                {
                                    try
                                    {
                                        if (nodetype != null)
                                        {
                                            //Connecotr zum Header erzeugen
                                            if (nodetype.m_Parent.Count == 0)
                                            {
                                                //  Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], repository.GetElementByGuid(nodetype.Classifier_ID).ElementID.ToString());
                                                Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], nodetype.OBJECT_ID.ToString());

                                            }
                                            else
                                            {
                                                //Taxonomy der Systeme aufbauen
                                                if (this.m_NodeType.Where(x => x == nodetype.m_Parent[0]).ToList().Count > 0)
                                                {
                                                    //Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], repository.GetElementByGuid(nodetype.m_Parent[0].Classifier_ID).ElementID.ToString(), repository.GetElementByGuid(nodetype.Classifier_ID).ElementID.ToString());
                                                    Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], nodetype.m_Parent[0].OBJECT_ID.ToString(), nodetype.OBJECT_ID.ToString());

                                                }
                                                else
                                                {
                                                    //Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], repository.GetElementByGuid(nodetype.Classifier_ID).ElementID.ToString());
                                                    Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], nodetype.OBJECT_ID.ToString());

                                                }
                                            }
                                        }

                                    }
                                    catch (Exception err)
                                    {
                                        MessageBox.Show("System Link: " + loading.label2.Text + ": " + err.Message);
                                    }
                                });
                            }
                        }
                        break;
                    case 1:
                        if (this.m_SysElem != null)
                        {
                            if (this.m_SysElem.Count > 0)
                            {
                                Parallel.ForEach(this.m_SysElem, nodetype =>
                                {
                                    try
                                    {
                                        if (nodetype != null)
                                        {
                                            //Connecotr zum Header erzeugen
                                            if (nodetype.m_Parent.Count == 0)
                                            {
                                                //   Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], repository.GetElementByGuid(nodetype.Classifier_ID).ElementID.ToString());
                                                Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], nodetype.OBJECT_ID.ToString());

                                            }
                                            else
                                            {
                                                //Taxonomy der Systeme aufbauen
                                                if (this.m_SysElem.Where(x => x == nodetype.m_Parent[0]).ToList().Count > 0)
                                                {
                                                    //Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], repository.GetElementByGuid(nodetype.m_Parent[0].Classifier_ID).ElementID.ToString(), repository.GetElementByGuid(nodetype.Classifier_ID).ElementID.ToString());
                                                    Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], nodetype.m_Parent[0].OBJECT_ID.ToString(), nodetype.OBJECT_ID.ToString());

                                                }
                                                else
                                                {
                                                    //Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], repository.GetElementByGuid(nodetype.Classifier_ID).ElementID.ToString());
                                                    Create_Link_Sys(Data, links, repository, null, null, this.Metamodel_Base.m_Connectoren_Req7[5], Data.metamodel.m_object_id[0], nodetype.OBJECT_ID.ToString());

                                                }
                                            }
                                        }

                                    }
                                    catch (Exception err)
                                    {
                                        MessageBox.Show("System Link: " + loading.label2.Text + ": " + err.Message);
                                    }
                                });
                            }
                        }
                        break;
                }

               
            }
            #endregion
            //////////////////////////////////////////////////////////////
            ///Add Stakeholder
            #region Stakeholder
            //Stakeholder
            if (Data.stakeholder_xac == true && Data.m_Stakeholder != null)
            {
                if (Data.m_Stakeholder.Count > 0)
                {
                    ////////////////////
                    ///Festlegen Werte Ladebalken
                    loading.label_Progress.Text = "Stakeholder";
                    loading.label2.Text = "Stakeholder";
                    loading.progressBar1.Value = 0;
                    loading.progressBar1.Maximum = Data.m_Stakeholder.Count;
                    loading.progressBar1.Refresh();


                    Parallel.ForEach(Data.m_Stakeholder, stakeholder =>
                    {
                        XElement recent_ST = this.Add_Stakeholder(stakeholder, repository, Data.metamodel);

                        items.Add(recent_ST);
                    });
                        /*   int s1 = 0;
                    do
                    {
                        XElement recent_ST = this.Add_Stakeholder(Data.m_Stakeholder[s1], repository, Data.metamodel);

                        items.Add(recent_ST);

                        loading.progressBar1.PerformStep();
                        loading.Refresh();

                        s1++;
                    } while (s1 < Data.m_Stakeholder.Count);

                    */
                }
            }
            #endregion Add Stakeholder
            /////////////////////////////W////////////////////////////
            #region Add_AFo
            if (Data.m_Requirement != null)
            {
                if (Data.m_Requirement.Count > 0)
                {
                    loading.label_Progress.Text = "Add Requirements";
                    loading.progressBar1.Minimum = 0;
                    loading.progressBar1.Maximum = Data.m_Requirement.Count;
                    loading.progressBar1.Value = 0;
                    loading.Refresh();


                  /*  Parallel.ForEach(Data.m_Requirement, requirement =>
                    {
                        if (requirement.RPI_Export == true)
                        {
                            this.m_Requirement.Add(requirement);
                            XElement recent_Afo = this.Add_AFo(requirement, repository, Data.metamodel);
                            items.Add(recent_Afo);
                        }

                    });*/
                    int i1 = 0;
                    do
                    {
                     //   try
                       // {
                            if (Data.m_Requirement[i1].RPI_Export == true)
                            {
                                this.m_Requirement.Add(Data.m_Requirement[i1]);
                                XElement recent_Afo = this.Add_AFo(Data.m_Requirement[i1], repository, Data.metamodel);
                                items.Add(recent_Afo);

                            if(Data.m_Requirement[i1].sysElement_Real.Classifier_ID != null)
                            {
                                if(this.m_SysElem_Real.Contains(Data.m_Requirement[i1].sysElement_Real) == false)
                                {
                                    this.m_SysElem_Real.Add(Data.m_Requirement[i1].sysElement_Real);
                                }
                                
                            }
                            }

                            loading.progressBar1.PerformStep();
                            loading.progressBar1.Refresh();
                    

                        i1++;
                    } while (i1 < Data.m_Requirement.Count);

                    
                }
            }

          
            #endregion Add_AFo
            if(Data.m_Requirement != null)
            {
                #region Export Afo_Afo_Links
                if (Data.link_afo_afo == true)
                {
                    if (this.m_Requirement.Count > 0)
                    {

                        loading.label_Progress.Text = "Add Requirements Links";
                        loading.Refresh();

                        Parallel.ForEach(this.m_Requirement, requirement =>
                        {
                            try
                            {
                                if(requirement != null)
                                {
                                    if (requirement.m_Requirement_Conflict.Count > 0)
                                    {
                                        Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[0], requirement.Classifier_ID, requirement.m_Requirement_Conflict, links, Data.metamodel, repository);
                                    }
                                    if (requirement.m_Requirement_Duplicate.Count > 0)
                                    {
                                        Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[1], requirement.Classifier_ID, requirement.m_Requirement_Duplicate, links, Data.metamodel, repository);
                                    }
                                    if (requirement.m_Requirement_Refines.Count > 0)
                                    {
                                        Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[2], requirement.Classifier_ID, requirement.m_Requirement_Refines, links, Data.metamodel, repository);
                                    }
                                    if (requirement.m_Requirement_Replace.Count > 0)
                                    {
                                        Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[3], requirement.Classifier_ID, requirement.m_Requirement_Replace, links, Data.metamodel, repository);
                                    }
                                    if (requirement.m_Requirement_Requires.Count > 0)
                                    {
                                        Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[4], requirement.Classifier_ID, requirement.m_Requirement_Requires, links, Data.metamodel, repository);
                                    }
                                    if (requirement.m_Requirement_InheritsFrom.Count > 0)
                                    {
                                        Export_Links_Requirement(Data, this.Metamodel_Base.m_Connectoren_Req7_Afo[5], requirement.Classifier_ID, requirement.m_Requirement_InheritsFrom, links, Data.metamodel, repository);
                                    }
                                }

                              
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show(requirement.Name + " Afo-->Afo: " + err.Message);
                            }
                        });
                        /*
                        int r1 = 0;
                        do
                        {
                            try
                            {

                                if (this.m_Requirement[r1].m_Requirement_Conflict.Count > 0)
                                {
                                    Export_Links_Requirement(this.Metamodel_Base.m_Connectoren_Req7_Afo[0], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Conflict, links, Data.metamodel, repository);
                                }
                                if (this.m_Requirement[r1].m_Requirement_Duplicate.Count > 0)
                                {
                                    Export_Links_Requirement(this.Metamodel_Base.m_Connectoren_Req7_Afo[1], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Duplicate, links, Data.metamodel, repository);
                                }
                                if (this.m_Requirement[r1].m_Requirement_Refines.Count > 0)
                                {
                                    Export_Links_Requirement(this.Metamodel_Base.m_Connectoren_Req7_Afo[2], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Refines, links, Data.metamodel, repository);
                                }
                                if (this.m_Requirement[r1].m_Requirement_Replace.Count > 0)
                                {
                                    Export_Links_Requirement(this.Metamodel_Base.m_Connectoren_Req7_Afo[3], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Replace, links, Data.metamodel, repository);
                                }
                                if (this.m_Requirement[r1].m_Requirement_Requires.Count > 0)
                                {
                                    Export_Links_Requirement(this.Metamodel_Base.m_Connectoren_Req7_Afo[4], this.m_Requirement[r1].Classifier_ID, this.m_Requirement[r1].m_Requirement_Requires, links, Data.metamodel, repository);
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show(Data.m_Requirement[r1].Name + " Afo-->Afo: " + err.Message);
                            }


                            r1++;
                        } while (r1 < this.m_Requirement.Count);

                        */
                    }
                }
                #endregion Export Afo_Afo_Links
                #region Export_AFo_Cap Links
                if (Data.link_afo_cap == true)
                {
                    loading.label_Progress.Text = "Add Requirements Links Capability";
                    loading.Refresh();

                    if (this.m_Requirement.Count > 0)
                    {

                        Parallel.ForEach(this.m_Requirement, requirement =>
                        {
                            if (requirement.m_Capability.Count > 0)
                            {
                                int c1 = 0;
                                do
                                {
                                  //  try
                                  //  {
                                        Create_Link_Afo_Sys(links, repository, requirement.Classifier_ID, null, requirement.m_Capability[c1].Classifier_ID);
                                  //  }
                                  /*  catch (Exception err)
                                    {
                                        MessageBox.Show(requirement.Name + " Afo-->Capability: " + err.Message);
                                    }
                                  */

                                    c1++;
                                } while (c1 < requirement.m_Capability.Count);
                            }

                        });

                    /*    var r1 = 0;
                        do
                        {
                            if (this.m_Requirement[r1].m_Capability.Count > 0)
                            {
                                int c1 = 0;
                                do
                                {
                                    try
                                    {
                                        Create_Link_Afo_Sys(links, repository, this.m_Requirement[r1].Classifier_ID, null, this.m_Requirement[r1].m_Capability[c1].Classifier_ID);
                                    }
                                    catch (Exception err)
                                    {
                                        MessageBox.Show(Data.m_Requirement[r1].Name + " Afo-->Capability: " + err.Message);
                                    }
                                   
                                    c1++;
                                } while (c1 < this.m_Requirement[r1].m_Capability.Count);
                            }
                            r1++;
                        } while (r1 < this.m_Requirement.Count);
                    */
                    }
                }
                #endregion
                #region Export AFO_Szenar Links
                if (Data.link_afo_logical == true)
                {
                    loading.label_Progress.Text = "Add Requirements Links Logical";
                    loading.Refresh();

                    if (this.m_Requirement.Count > 0)
                    {

                        //  Parallel.ForEach(this.m_Requirement, requirement =>
                        //  {
                        int i1 = 0;
                        do
                        {
                            //Link Afo_Logical
                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                            {
                                List<Logical> m_Logical = this.m_Requirement[i1].m_Logical;

                                if (m_Logical != null)
                                {
                                    if (m_Logical.Count > 0)
                                    {
                                        int l1 = 0;
                                        do
                                        {
                                            // try
                                            // {
                                            Create_Link_Afo_Sys(links, repository, this.m_Requirement[i1].Classifier_ID, null, m_Logical[l1].Classifier_ID);
                                            // }
                                            /* catch (Exception err)
                                             {
                                                 MessageBox.Show(requirement.Name + " Afo-->Logical: " + err.Message);
                                             }*/

                                            l1++;
                                        } while (l1 < m_Logical.Count);
                                    }

                                }

                            }

                            i1++;
                        } while (i1 < this.m_Requirement.Count);

                           
                     //  });
          
         }
     }
     #endregion
                #region Export AFO_Sys_Links
                 if(Data.link_afo_sys == true)
                 {
                                loading.label_Progress.Text = "Add Requirements Links System";
                                loading.Refresh();

                                if (this.m_Requirement.Count > 0)
                     {
                        switch(Data.metamodel.modus)
                        {
                            case 0:
                                #region NodeType
                                if (this.m_NodeType != null)
                                {
                                    int as1 = 0;
                                    do
                                    {
                                        if (this.m_Requirement[as1].nodeType != null)
                                        {
                                            
                                                if (this.m_NodeType.Contains(this.m_Requirement[as1].nodeType) == true)
                                                {
                                                    try
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, this.m_Requirement[as1].Classifier_ID, null, this.m_Requirement[as1].nodeType.Classifier_ID);
                                                    }
                                                    catch (Exception err)
                                                    {
                                                        MessageBox.Show(Data.m_Requirement[as1].Name + " Afo-->Sys: " + err.Message);
                                                    }
                                                }


                                        }
                                        as1++;
                                    } while (this.m_Requirement.Count > as1);
                                 }
                                break;
                            #endregion
                            case 1:
                                #region SysEleme
                                if(this.m_SysElem != null)
                                {
                                    int as1 = 0;
                                    do
                                    {
                                        if (this.m_Requirement[as1].sysElements.Count > 0)
                                        {
                                            int as2 = 0;
                                            do
                                            {
                                                if (this.m_SysElem.Contains(this.m_Requirement[as1].sysElements[as2]) == true)
                                                {
                                                    try
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, this.m_Requirement[as1].Classifier_ID, null, this.m_Requirement[as1].sysElements[as2].Classifier_ID);
                                                    }
                                                    catch (Exception err)
                                                    {
                                                        MessageBox.Show(Data.m_Requirement[as1].Name + " Afo-->Sys: " + err.Message);
                                                    }
                                                }


                                                as2++;
                                            } while (as2 < this.m_Requirement[as1].sysElements.Count);


                                        }


                                        as1++;
                                    } while (as1 < this.m_Requirement.Count);
                                }

                                if (this.m_SysElem_Real != null)
                                {
                                    int as1 = 0;
                                    do
                                    {
                                        if (this.m_Requirement[as1].sysElements_Real.Count > 0)
                                        {
                                            int as2 = 0;
                                            do
                                            {
                                                if (this.m_SysElem_Real.Contains(this.m_Requirement[as1].sysElements_Real[as2]) == true)
                                                {
                                                    try
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, this.m_Requirement[as1].Classifier_ID, null, this.m_Requirement[as1].sysElements_Real[as2].Classifier_ID);
                                                    }
                                                    catch (Exception err)
                                                    {
                                                        MessageBox.Show(Data.m_Requirement[as1].Name + " Afo-->Sys: " + err.Message);
                                                    }
                                                }


                                                as2++;
                                            } while (as2 < this.m_Requirement[as1].sysElements_Real.Count);


                                        }


                                        as1++;
                                    } while (as1 < this.m_Requirement.Count);
                                }
                                #endregion
                                break;
                        }


                                    //Parallel.ForEach(this.m_Requirement, requirement =>
                                    //{
                         
                                    /*if (this.m_NodeType.Contains(requirement.nodeType) == true)
                                        {
                                           // try
                                           // {
                                                Create_Link_Afo_Sys(links, repository, requirement.Classifier_ID, null, requirement.nodeType.Classifier_ID);
                                           // }
                                            //catch (Exception err)
                                           // {
                                           //     MessageBox.Show(requirement.Name + " Afo-->Sys: " + err.Message);
                                           // }
                                        }
                                    */
                                  //  });
                                       
              
                     }


                 }
                 #endregion
            }






     //////////////////////////////////////////////////////////
     //Abspeichern
     #region Save
     string xac_Header = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
     string xml_string = xml_dat.ToString();
            ////////////////////////////////////////////////////////////
            //Sonderzeichen ersetzen
            #region sonderzeichen
            if (Data.metamodel.m_Sonder.Count > 0)
            {
                Requirement_Plugin.xml.XML xml = new Requirement_Plugin.xml.XML();
                xml_string = xml.Change_sonderzeichen(xml_string, Data.metamodel.m_Sonder);
            }
            #endregion

            ////////////////////
            ///Festlegen Werte Ladebalken
            loading.label_Progress.Text = "Speichern xac-File";
     loading.label2.Text = "Speichern xac-File";
     loading.progressBar1.Value = 0;
     loading.progressBar1.Maximum = 1;
     loading.progressBar1.Refresh();
     //xml_dat.Save(filename);
     System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
     file.WriteLine(xac_Header);
     file.WriteLine(xml_string);
     file.Close();

     loading.progressBar1.PerformStep();
     loading.label2.Text = "Speichern abgeschlossen";

     loading.Close();
     #endregion
}

#endregion Exportmöglichkeiten


#region Export_Flat
//public Export_Flat()
#endregion

#region Add_Elements
/// <summary>
/// Es wird ein Systemelement für die Require7 Datei erzeugt
/// </summary>
/// <param name="recent"></param>
/// <param name="repository"></param>
/// <returns></returns>
private XElement Add_Sys(NodeType recent, EA.Repository repository, Metamodel metamodel)
{
 TaggedValue tagged = new TaggedValue(metamodel, this.Data);

 string agid = tagged.Get_Tagged_Value(recent.Instantiate_GUID, "SYS_AG_ID", repository); //Fest? --> nein
 string anid = "";

 var Element_Decomposition = repository.GetElementByGuid(recent.Instantiate_GUID);
 var Element = repository.GetElementByGuid(recent.Classifier_ID);


 XElement xml_Element = new XElement("item");

 ///Attribte von item

 XAttribute Att_puid = new XAttribute("puid", "");
            //XAttribute Att_object_ID = new XAttribute("objectid", Element_Decomposition.ElementID);
            XAttribute Att_object_ID = new XAttribute("objectid", recent.OBJECT_ID);
            XAttribute Att_Klasse = new XAttribute("Klasse", "Systemelement");
 string UUID = recent.Instantiate_GUID;
 UUID = UUID.Trim('{', '}');
 XAttribute Att_uuid = new XAttribute("uuid", UUID);
 XAttribute Att_submittedBy = new XAttribute("submittedBy", "");
 string date = Element.Created.ToString("o");
 //   MessageBox.Show(date);
 string actualdate = date;
 if (date.Split('+').Length == 1)
 {
     actualdate = actualdate + "+02:00";
 }
 XAttribute Att_created = new XAttribute("created", actualdate);
 //XAttribute Att_createdBy = new XAttribute("createdBy", tagged.Get_Tagged_Value(recent.Classifier_ID, "SYS_ANSPRECHPARTNER", repository));
 //XAttribute Att_createdBy = new XAttribute("createdBy", tagged.Get_Tagged_Value(recent.Classifier_ID, "SYS_ANSPRECHPARTNER", repository));
 XAttribute Att_createdBy = new XAttribute("createdBy", recent.Author);

 string modified_date = Element.Modified.ToString("o");
 string actual_modified_date = modified_date;
 if (modified_date.Split('+').Length == 1)
 {
     actual_modified_date = actual_modified_date + "+02:00";
 }
 XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
 // XAttribute Att_modifiedBy = new XAttribute("modifiedBy", tagged.Get_Tagged_Value(recent.Classifier_ID, "SYS_ANSPRECHPARTNER", repository));
 XAttribute Att_modifiedBy = new XAttribute("modifiedBy", recent.Author);

// if (tagged.Get_Tagged_Value(recent.Classifier_ID, "SYS_AG_ID", repository) != "kein")
 if (recent.SYS_AG_ID != "kein")
 {
     agid = recent.SYS_AG_ID;
 }
 XAttribute Att_agid = new XAttribute("agid", agid);
// if (tagged.Get_Tagged_Value(recent.Classifier_ID, "SYS_AN_ID", repository) != "kein")
 if (recent.SYS_AN_ID != "kein")
 {
     anid = recent.SYS_AN_ID;
 }
 XAttribute Att_anid = new XAttribute("anid", anid);

 XAttribute Att_tagmask = new XAttribute("tagmask", recent.TagMask);

    xml_Element.Add(Att_tagmask);
            if(recent.TagMask != "0")
            {
                XAttribute Att_tagged = new XAttribute("tagged", "true");
                xml_Element.Add(Att_tagged);
            }
            else
            {
                XAttribute Att_tagged = new XAttribute("tagged", "false");
                xml_Element.Add(Att_tagged);
            }


         


 xml_Element.Add(Att_anid);
 xml_Element.Add(Att_agid);
 xml_Element.Add(Att_modifiedBy);
 xml_Element.Add(Att_modified);
 xml_Element.Add(Att_createdBy);
 xml_Element.Add(Att_created);
 xml_Element.Add(Att_submittedBy);
 xml_Element.Add(Att_uuid);
 xml_Element.Add(Att_Klasse);
 xml_Element.Add(Att_object_ID);
 xml_Element.Add(Att_puid);

 /*
 xml_Element.Add(Att_puid);
 xml_Element.Add(Att_object_ID);
 xml_Element.Add(Att_Klasse);
 xml_Element.Add(Att_uuid);
 xml_Element.Add(Att_submittedBy);
 xml_Element.Add(Att_created);
 xml_Element.Add(Att_createdBy);
 xml_Element.Add(Att_modified);
 xml_Element.Add(Att_modifiedBy);
 xml_Element.Add(Att_agid);
 xml_Element.Add(Att_anid);
 xml_Element.Add(Att_tagged);
 xml_Element.Add(Att_tagmask);
 */

            //  XElement Titel = new XElement("Titel", tagged.Get_Tagged_Value(recent.Classifier_ID, "SYS_KUERZEL", repository));
            XElement Titel = new XElement("Titel", recent.SYS_KUERZEL);
            xml_Element.Add(Titel);
            XElement Beschreibung = new XElement("Beschreibung", Element.Notes);
            xml_Element.Add(Beschreibung);
            XElement Klaerungspunkte = new XElement("Klaerungspunkte", recent.AFO_KLAERUNGSPUNKTE);
            xml_Element.Add(Klaerungspunkte);

            XElement NameValues = Add_NameValues( null, null, recent, null, null, repository, true, false, false, false, false, false, metamodel);

            xml_Element.Add(NameValues);

            return (xml_Element);

            //  XAttribute Att_created = new XAttribute("createdby")
        }
        //////////////
        /// <summary>
        /// Es wird ein Stakeholder für die Require 7 Datei erzeugt
        /// </summary>
        /// <param name="recent"></param>
        /// <param name="repository"></param>
        /// <param name="metamodel"></param>
        /// <returns></returns>
        private XElement Add_Stakeholder(Stakeholder recent, EA.Repository repository, Metamodel metamodel)
        {
            TaggedValue tagged = new TaggedValue(metamodel, this.Data);

            string agid = ""; //Fest? --> nein
            string anid = "";

            var Element_Decomposition = repository.GetElementByGuid(recent.Classifier_ID);
            var Element = repository.GetElementByGuid(recent.Classifier_ID);


            XElement xml_Element = new XElement("item");

            ///Attribte von item

            XAttribute Att_puid = new XAttribute("puid", "");
            //  XAttribute Att_object_ID = new XAttribute("objectid", Element_Decomposition.ElementID);
            XAttribute Att_object_ID = new XAttribute("objectid", recent.OBJECT_ID);
            XAttribute Att_Klasse = new XAttribute("Klasse", "Stakeholder");
            XAttribute Att_uuid = new XAttribute("uuid", recent.UUID);
            XAttribute Att_submittedBy = new XAttribute("submittedBy", "");
            string date = Element.Created.ToString("o");
            //   MessageBox.Show(date);
            string actualdate = date;
            if (date.Split('+').Length == 1)
            {
                actualdate = actualdate + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actualdate);
            XAttribute Att_createdBy = new XAttribute("createdBy", recent.st_ANSPRECHPARTNER);
            string modified_date = Element.Modified.ToString("o");
            string actual_modified_date = modified_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_modified_date = actual_modified_date + "+02:00";
            }
            XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
            XAttribute Att_modifiedBy = new XAttribute("modifiedBy", recent.st_ANSPRECHPARTNER);

            if (recent.st_AG_ID != "kein")
            {
                agid = recent.st_AG_ID;
            }
            XAttribute Att_agid = new XAttribute("agid", agid);
            if (recent.st_AN_ID != "kein")
            {
                anid = recent.st_AN_ID;
            }
            XAttribute Att_anid = new XAttribute("anid", anid);

           
            XAttribute Att_tagmask = new XAttribute("tagmask", recent.TagMask);

            xml_Element.Add(Att_tagmask);

            if(recent.TagMask != "0")
            {
                XAttribute Att_tagged = new XAttribute("tagged", "true");
                   xml_Element.Add(Att_tagged);
            }
            else
            {
                XAttribute Att_tagged = new XAttribute("tagged", "false");
                xml_Element.Add(Att_tagged);
            }


         
            xml_Element.Add(Att_anid);
            xml_Element.Add(Att_agid);
            xml_Element.Add(Att_modifiedBy);
            xml_Element.Add(Att_modified);
            xml_Element.Add(Att_createdBy);
            xml_Element.Add(Att_created);
            xml_Element.Add(Att_submittedBy);
            xml_Element.Add(Att_uuid);
            xml_Element.Add(Att_Klasse);
            xml_Element.Add(Att_object_ID);
            xml_Element.Add(Att_puid);

            XElement Titel = new XElement("Titel", recent.st_STAKEHOLDER);
            xml_Element.Add(Titel);
            XElement Beschreibung = new XElement("Beschreibung", recent.st_BESCHREIBUNG);
            xml_Element.Add(Beschreibung);
            XElement Klaerungspunkte = new XElement("Klaerungspunkte");
            xml_Element.Add(Klaerungspunkte);

            XElement NameValues = Add_NameValues(recent.Classifier_ID,null, null, recent, null, repository, false, false, false, false, true, false, metamodel);

            xml_Element.Add(NameValues);

            return (xml_Element);
        }

        /// Es wird eine Anforderung für die Require 7 Datei erzeugt
        /// </summary>
        /// <param name="AFo_GUID"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        private XElement Add_AFo(Requirement AFO, EA.Repository repository, Metamodel metamodel)
        {
            string AFo_GUID = AFO.Classifier_ID;

            TaggedValue tagged = new TaggedValue(metamodel, this.Data);
            var AFo_DB = repository.GetElementByGuid(AFo_GUID);
            XElement AFo = new XElement("item");

            //Attribute des AFo Headers
            XAttribute Att_Tagmask = new XAttribute("tagmask", AFO.TagMask);
            AFo.Add(Att_Tagmask);
            if(AFO.TagMask != "0")
            {
                XAttribute Att_tagged = new XAttribute("tagged", "true");
                AFo.Add(Att_tagged);
            }
            else
            {
                XAttribute Att_tagged = new XAttribute("tagged", "false");
                AFo.Add(Att_tagged);
            }
          
            string anid = "";
            //  if (tagged.Get_Tagged_Value(AFo_GUID, "AFO_AN_ID", repository) != "kein")
            if (AFO.AFO_AN_ID != "kein")
            {
                anid = AFO.AFO_AN_ID;
                if (anid == null)
                {
                    anid = "";
                }
            }
            XAttribute Att_anid = new XAttribute("anid", anid);
            AFo.Add(Att_anid);
            string agid = "";
            //  if (tagged.Get_Tagged_Value(AFo_GUID, "AFO_AG_ID", repository) != "kein")
            if (AFO.AFO_AG_ID != "kein")
            {
                agid = AFO.AFO_AG_ID;
                if (agid == null)
                {
                    agid = "";
                }
            }
            XAttribute Att_agid = new XAttribute("agid", agid);
            AFo.Add(Att_agid);
            XAttribute Att_modifiedby = new XAttribute("modifiedBy", AFo_DB.Author);
            AFo.Add(Att_modifiedby);
            string date = AFo_DB.Modified.ToString("o");
            // MessageBox.Show(date);
            string actualdate = date;
            if (date.Split('+').Length == 1)
            {
                actualdate = actualdate + "+02:00";
            }
            //    MessageBox.Show(actualdate);
            XAttribute Att_modified = new XAttribute("modified", actualdate);
            AFo.Add(Att_modified);
            XAttribute Att_createdby = new XAttribute("createdBy", AFo_DB.Author);
            AFo.Add(Att_createdby);
            string date2 = AFo_DB.Created.ToString("o");
            string actualdate2 = date2;
            if (date2.Split('+').Length == 1)
            {
                actualdate2 = actualdate2 + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actualdate2);
            AFo.Add(Att_created);
            XAttribute Att_submittedBy = new XAttribute("submittedBy", "");
            AFo.Add(Att_submittedBy);
            //     string uuid = tagged.Get_Tagged_Value(AFo_GUID, "UUID", repository);
            string uuid = AFO.UUID;
            if (uuid == null)
            {
                uuid = AFo_GUID;
                uuid = uuid.Trim('{', '}');
            }
            XAttribute Att_uuid = new XAttribute("uuid", uuid);
            AFo.Add(Att_uuid);
            XAttribute Att_Klasse = new XAttribute("Klasse", "Anforderung");
            AFo.Add(Att_Klasse);
            //  XAttribute Att_objectid = new XAttribute("objectid", tagged.Get_Tagged_Value(AFo_GUID, "OBJECT_ID", repository));
            XAttribute Att_objectid = new XAttribute("objectid", AFO.OBJECT_ID);
            AFo.Add(Att_objectid);
            XAttribute Att_puid = new XAttribute("puid", "");
            AFo.Add(Att_puid);

            //Titel
            string Titel = "";
            //   if (tagged.Get_Tagged_Value(AFo_GUID, "AFO_TITEL", repository) != null && tagged.Get_Tagged_Value(AFo_GUID, "AFO_TITEL", repository) != "kein")
            if (AFO.AFO_TITEL != null && AFO.AFO_TITEL != "kein")
            {
                Titel = AFO.AFO_TITEL;
            }
            XElement XTitel = new XElement("Titel", Titel);
            AFo.Add(XTitel);
            //Beschreibung
            string Beschreibung = "";
            //  if (tagged.Get_Tagged_Value(AFo_GUID, "AFO_TEXT", repository) != null && tagged.Get_Tagged_Value(AFo_GUID, "AFO_TEXT", repository) != "kein")
            if (AFO.AFO_TEXT != null && AFO.AFO_TEXT != "kein")
            {
                Beschreibung = AFO.AFO_TEXT;
            }
            XElement XBeschreibung = new XElement("Beschreibung", Beschreibung);
            AFo.Add(XBeschreibung);
            //Klaerungspunkte
            string Klaerungspunkte = "";
            //     if (tagged.Get_Tagged_Value(AFo_GUID, "AFO_KLAERUNGSPUNKTE", repository) != null && tagged.Get_Tagged_Value(AFo_GUID, "AFO_KLAERUNGSPUNKTE", repository) != "kein")
            if (AFO.AFO_KLAERUNGSPUNKTE != null && AFO.AFO_KLAERUNGSPUNKTE != "kein" && AFO.AFO_KLAERUNGSPUNKTE != "not set")
            {
                Klaerungspunkte = AFO.AFO_KLAERUNGSPUNKTE;
            }
            XElement XKlaerungspunkte = new XElement("Klaerungspunkte", Klaerungspunkte);
            AFo.Add(XKlaerungspunkte);
            //NameValues etc aufbauen
            AFo.Add(Add_NameValues(AFo_GUID, AFO, null, null, null, repository, false, true, false, false, false, false, metamodel));

            return (AFo);
        }

        private XElement Add_Logical(Logical recent, EA.Repository repository, Metamodel metamodel)
        {
            TaggedValue tagged = new TaggedValue(metamodel, this.Data);

            // string agid = tagged.Get_Tagged_Value(recent.Classifier_ID, "SYS_AG_ID", repository); //Fest? --> nein
            string agid = recent.SYS_AG_ID;
            string anid = "";

            var Element = repository.GetElementByGuid(recent.Classifier_ID);

            XElement xml_Element = new XElement("item");

            ///Attribte von item

            XAttribute Att_puid = new XAttribute("puid", "");
            //XAttribute Att_object_ID = new XAttribute("objectid", Element.ElementID);
            XAttribute Att_object_ID = new XAttribute("objectid", recent.OBJECT_ID);
            XAttribute Att_Klasse = new XAttribute("Klasse", "Systemelement");
            //  XAttribute Att_uuid = new XAttribute("uuid", tagged.Get_Tagged_Value(recent.Classifier_ID, "UUID", repository));
            XAttribute Att_uuid = new XAttribute("uuid",recent.UUID);
            XAttribute Att_submittedBy = new XAttribute("submittedBy", "");
            string date = Element.Created.ToString("o");
            //   MessageBox.Show(date);
            string actualdate = date;
            if (date.Split('+').Length == 1)
            {
                actualdate = actualdate + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actualdate);
            //   XAttribute Att_createdBy = new XAttribute("createdBy", tagged.Get_Tagged_Value(recent.Classifier_ID, "SYS_ANSPRECHPARTNER", repository));
            XAttribute Att_createdBy = new XAttribute("createdBy",recent.SYS_ANSPRECHPARTNER);
            string modified_date = Element.Modified.ToString("o");
            string actual_modified_date = modified_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_modified_date = actual_modified_date + "+02:00";
            }
            XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
            XAttribute Att_modifiedBy = new XAttribute("modifiedBy", recent.SYS_ANSPRECHPARTNER);

            if (recent.SYS_AG_ID != "kein")
            {
                agid = recent.SYS_AG_ID;
            }
            XAttribute Att_agid = new XAttribute("agid", agid);
            if (recent.SYS_AN_ID != "kein")
            {
                anid = recent.SYS_AN_ID;
            }
            XAttribute Att_anid = new XAttribute("anid", anid);


            XAttribute Att_tagmask = new XAttribute("tagmask", recent.TagMask);

            xml_Element.Add(Att_tagmask);
            if (recent.TagMask != "0")
            {
                XAttribute Att_tagged = new XAttribute("tagged", "true");
                xml_Element.Add(Att_tagged);
            }
            else
            {
                XAttribute Att_tagged = new XAttribute("tagged", "false");
                xml_Element.Add(Att_tagged);
            }

          
           
            xml_Element.Add(Att_anid);
            xml_Element.Add(Att_agid);
            xml_Element.Add(Att_modifiedBy);
            xml_Element.Add(Att_modified);
            xml_Element.Add(Att_createdBy);
            xml_Element.Add(Att_created);
            xml_Element.Add(Att_submittedBy);
            xml_Element.Add(Att_uuid);
            xml_Element.Add(Att_Klasse);
            xml_Element.Add(Att_object_ID);
            xml_Element.Add(Att_puid);

            XElement Titel = new XElement("Titel", recent.SYS_KUERZEL);
            xml_Element.Add(Titel);
            XElement Beschreibung = new XElement("Beschreibung", Element.Notes);
            xml_Element.Add(Beschreibung);
            XElement Klaerungspunkte = new XElement("Klaerungspunkte");
            xml_Element.Add(Klaerungspunkte);

            XElement NameValues = Add_NameValues(recent.Classifier_ID, null, recent, null, null, repository, false, false, true, false, false, false, metamodel);

            xml_Element.Add(NameValues);

            return (xml_Element);
        }

        private XElement Add_Sys_Root(Database Data, string Sys, string Sys_Type, string Klasse, string Name, string Artikel_Name, string Text, string OBJECT_ID)
        {
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            XElement Root = new XElement("item");

            ///Attribte von item

            XAttribute Att_puid = new XAttribute("puid", "");
            XAttribute Att_object_ID = new XAttribute("objectid", OBJECT_ID);
            XAttribute Att_Klasse = new XAttribute("Klasse", Klasse);

            string UUID = tagged.Generate_GUID("t_object");
            UUID = UUID.Trim('{', '}');

            XAttribute Att_uuid = new XAttribute("uuid", UUID);
            XAttribute Att_submittedBy = new XAttribute("submittedBy", "");

            DateTime dateTime = DateTime.Now;
            string date = dateTime.ToString("o");
            //   MessageBox.Show(date);
            string actualdate = date;
            if (date.Split('+').Length == 1)
            {
                actualdate = actualdate + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actualdate);
            XAttribute Att_createdBy = new XAttribute("createdBy", Data.metamodel.createdby);
            string modified_date = actualdate;
            string actual_modified_date = actualdate;
            if (modified_date.Split('+').Length == 1)
            {
                actual_modified_date = actual_modified_date + "+02:00";
            }
            XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
            XAttribute Att_modifiedBy = new XAttribute("modifiedBy", Data.metamodel.createdby);

            XAttribute Att_agid = new XAttribute("agid", Sys);

            XAttribute Att_anid = new XAttribute("anid", " ");
            XAttribute Att_tagged = new XAttribute("tagged", "false");
            XAttribute Att_tagmask = new XAttribute("tagmask", 0);

            Root.Add(Att_tagmask);
            Root.Add(Att_tagged);
            Root.Add(Att_anid);
            Root.Add(Att_agid);
            Root.Add(Att_modifiedBy);
            Root.Add(Att_modified);
            Root.Add(Att_createdBy);
            Root.Add(Att_created);
            Root.Add(Att_submittedBy);
            Root.Add(Att_uuid);
            Root.Add(Att_Klasse);
            Root.Add(Att_object_ID);
            Root.Add(Att_puid);
            ///////////////////////////
            ///Kinder
            XElement Titel = new XElement("Titel", Name);
            Root.Add(Titel);
            XElement Beschreibung = new XElement("Beschreibung", Text);
            Root.Add(Beschreibung);
            XElement Klaerungspunkte = new XElement("Klaerungspunkte");
            Root.Add(Klaerungspunkte);

            ///NameValues
            ///
            XElement NameValues = new XElement("NameValues");
            //PUID
            NameValues.Add(Add_NameValue("PUID", ""));
            //OBJECT_ID
            string Object_ID = "";
            NameValues.Add(Add_NameValue("OBJECT_ID", OBJECT_ID));
            //SYS_AG_ID
            NameValues.Add(Add_NameValue("SYS_AG_ID", Sys));
            //SYS_AN_ID
            NameValues.Add(Add_NameValue("SYS_AN_ID", " "));
            //SYS_KUERZEL
            NameValues.Add(Add_NameValue("SYS_KUERZEL", Name));
            //SYS_BEZEICHNUNG
            NameValues.Add(Add_NameValue("SYS_BEZEICHNUNG", Text));
            //SYS_ARTIKEL
            NameValues.Add(Add_NameValue("SYS_ARTIKEL", Artikel_Name));
            //SYS_DETAILSTUFE
            NameValues.Add(Add_NameValue("SYS_DETAILSTUFE", Data.SYS_ENUM.SYS_DETAILSTUFE[0]));
            //SYS_TYP
            NameValues.Add(Add_NameValue("SYS_TYP", Sys_Type));
            //SYS_ERFUELLT_AFO
            NameValues.Add(Add_NameValue("SYS_ERFUELLT_AFO", "true"));
            //SYS_SUBORDINATES_AFO
            NameValues.Add(Add_NameValue("SYS_SUBORDINATES_AFO", Data.SYS_ENUM.SYS_SUBORDINATES_AFO[0]));
            //SYS_KOMPONENTENTYP
            NameValues.Add(Add_NameValue("SYS_KOMPONENTENTYP", Data.SYS_ENUM.SYS_KOMPONENTENTYP[0]));
            //SYS_STATUS
            string SYS_STATUS = "";
            NameValues.Add(Add_NameValue("SYS_STATUS", Data.SYS_ENUM.SYS_STATUS[1]));
            //IN_CATEGORY
            NameValues.Add(Add_NameValue("IN_CATEGORY", Data.AFO_ENUM.IN_CATEGORY[0]));
            //SYS_PRODUKT_STATUS
            string SYS_PRODUKT_STATUS = "";
            NameValues.Add(Add_NameValue("SYS_PRODUKT_STATUS", ""));
            //SYS_PRODUKT
            NameValues.Add(Add_NameValue("SYS_PRODUKT", ""));
            //B_KENNUNG
            string B_KENNUNG = "";
            NameValues.Add(Add_NameValue("B_KENNUNG", ""));
            //SYS_REL_GEWICHT
            if(Sys_Type == Data.SYS_ENUM.SYS_TYP[0])
            {
                NameValues.Add(Add_NameValue("SYS_REL_GEWICHT", "100"));
            }
            else
            {
                NameValues.Add(Add_NameValue("SYS_REL_GEWICHT", ""));
            }
            


            Root.Add(NameValues);

            return (Root);
        }

        private XElement Add_Funktionsbaum(Capability recent, EA.Repository repository, Metamodel metamodel)
        {
            TaggedValue tagged = new TaggedValue(metamodel, this.Data);

            string agid = recent.SYS_AG_ID;
            string anid = "";

            var Element = repository.GetElementByGuid(recent.Classifier_ID);

            XElement xml_Element = new XElement("item");

            ///Attribte von item

            XAttribute Att_puid = new XAttribute("puid", "");
            //XAttribute Att_object_ID = new XAttribute("objectid", Element.ElementID);
            XAttribute Att_object_ID = new XAttribute("objectid", recent.OBJECT_ID);
            XAttribute Att_Klasse = new XAttribute("Klasse", "Systemelement");
            XAttribute Att_uuid = new XAttribute("uuid",recent.UUID);
            XAttribute Att_submittedBy = new XAttribute("submittedBy", "");
            string date = Element.Created.ToString("o");
            //   MessageBox.Show(date);
            string actualdate = date;
            if (date.Split('+').Length == 1)
            {
                actualdate = actualdate + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actualdate);
            XAttribute Att_createdBy = new XAttribute("createdBy", recent.SYS_ANSPRECHPARTNER);
            string modified_date = Element.Modified.ToString("o");
            string actual_modified_date = modified_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_modified_date = actual_modified_date + "+02:00";
            }
            XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
            XAttribute Att_modifiedBy = new XAttribute("modifiedBy", recent.SYS_ANSPRECHPARTNER);

            if (recent.SYS_AG_ID != "kein")
            {
                agid = recent.SYS_AG_ID;
            }
            XAttribute Att_agid = new XAttribute("agid", agid);
            if (recent.SYS_AN_ID != "kein")
            {
                anid = recent.SYS_AN_ID;
            }
            XAttribute Att_anid = new XAttribute("anid", anid);

            XAttribute Att_tagmask = new XAttribute("tagmask", recent.TagMask);

            xml_Element.Add(Att_tagmask);

            if (recent.TagMask != "0")
            {
                XAttribute Att_tagged = new XAttribute("tagged", "true");
                xml_Element.Add(Att_tagged);
            }
            else
            {
                XAttribute Att_tagged = new XAttribute("tagged", "false");
                xml_Element.Add(Att_tagged);
            }

          
           
           
            xml_Element.Add(Att_anid);
            xml_Element.Add(Att_agid);
            xml_Element.Add(Att_modifiedBy);
            xml_Element.Add(Att_modified);
            xml_Element.Add(Att_createdBy);
            xml_Element.Add(Att_created);
            xml_Element.Add(Att_submittedBy);
            xml_Element.Add(Att_uuid);
            xml_Element.Add(Att_Klasse);
            xml_Element.Add(Att_object_ID);
            xml_Element.Add(Att_puid);

            XElement Titel = new XElement("Titel", recent.SYS_KUERZEL);
            xml_Element.Add(Titel);
            XElement Beschreibung = new XElement("Beschreibung", recent.SYS_BEZEICHNUNG);
            xml_Element.Add(Beschreibung);
            XElement Klaerungspunkte = new XElement("Klaerungspunkte");
            xml_Element.Add(Klaerungspunkte);

            XElement NameValues = Add_NameValues(recent.Classifier_ID, null, recent, null, null, repository, false, false, false, true, false, false, metamodel);

            xml_Element.Add(NameValues);

            return (xml_Element);
        }

        private XElement Add_GlossarElement(Glossar_Element recent, EA.Repository repository, Metamodel metamodel)
        {
            XElement xml_Element = new XElement("item");
            TaggedValue tagged = new TaggedValue(metamodel, this.Data);

            ///Attribte von item

            XAttribute Att_puid = new XAttribute("puid", "");
            XAttribute Att_object_ID = new XAttribute("objectid", recent.object_ID);
            XAttribute Att_Klasse = new XAttribute("Klasse", "Glossarelement");

            string UUID = tagged.Generate_UUID();
            string date = tagged.Get_Datetime();

            XAttribute Att_uuid = new XAttribute("uuid", UUID);
            XAttribute Att_submittedBy = new XAttribute("submittedBy", "");

            XAttribute Att_created = new XAttribute("created", date);
            XAttribute Att_createdBy = new XAttribute("createdBy", recent.gloe_AUTOR);
            XAttribute Att_modified = new XAttribute("modified", date);
            XAttribute Att_modifiedBy = new XAttribute("modifiedBy", recent.gloe_AUTOR);
            XAttribute Att_agid = new XAttribute("agid", recent.gloe_AG_ID);
            XAttribute Att_anid = new XAttribute("anid", recent.gloe_AN_ID);
            XAttribute Att_tagged = new XAttribute("tagged", "false");
            XAttribute Att_tagmask = new XAttribute("tagmask", 0);

            xml_Element.Add(Att_tagmask);
            xml_Element.Add(Att_tagged);
            xml_Element.Add(Att_anid);
            xml_Element.Add(Att_agid);
            xml_Element.Add(Att_modifiedBy);
            xml_Element.Add(Att_modified);
            xml_Element.Add(Att_createdBy);
            xml_Element.Add(Att_created);
            xml_Element.Add(Att_submittedBy);
            xml_Element.Add(Att_uuid);
            xml_Element.Add(Att_Klasse);
            xml_Element.Add(Att_object_ID);
            xml_Element.Add(Att_puid);

            XElement Titel = new XElement("Titel", recent.gloe_BEGRIFF);
            xml_Element.Add(Titel);
            XElement Beschreibung = new XElement("Beschreibung", recent.gloe_GLOSSARTEXT);
            xml_Element.Add(Beschreibung);
            XElement Klaerungspunkte = new XElement("Klaerungspunkte");
            xml_Element.Add(Klaerungspunkte);

            XElement NameValues = Add_NameValues(null, null, null, null, recent, repository, false, false, false, false, false, true, metamodel);

            xml_Element.Add(NameValues);

            return (xml_Element);
        }
        #endregion Add_Elements

        #region Add_links

        /// <summary>
        /// Anlegen eines Links zwiscchen zwei Systemen
        /// </summary>
        /// <param name="links"></param>
        /// <param name="repository"></param>
        /// <param name="Client"></param>
        /// <param name="Supplier"></param>
        /// <param name="link_name"></param>
        private void Create_Link_Sys(Database Data, XElement links, EA.Repository repository, NodeType Client, NodeType Supplier, string link_name, string Client_object_id, string Supplier_object_id)
        {
            //Hilfsvariablen
            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);

            XElement link_Hierachie = new XElement("link");
            links.Add(link_Hierachie);
            //Atribute von link
            string modified_date = "";
            string modified_by = "";

            if (Client_object_id == null && Supplier_object_id == null)
            {
                modified_by = Supplier.SYS_ANSPRECHPARTNER;
                modified_date = repository.GetElementByGuid(Supplier.Classifier_ID).Modified.ToString("o");
            }
            else
            {
                modified_by = Data.metamodel.createdby;
                modified_date = DateTime.Now.ToString("o");
            }
            XAttribute modifiedBy = new XAttribute("modifiedBy", modified_by);
            string actual_modified_date = modified_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_modified_date = actual_modified_date + "+02:00";
            }
            XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
            string created_by = "";
            string created_date = "";
            if (Client_object_id == null && Supplier_object_id == null)
            {
                created_by = Supplier.SYS_ANSPRECHPARTNER; ;
                created_date = repository.GetElementByGuid(Supplier.Classifier_ID).Modified.ToString("o");
            }
            else
            {
                created_by = Data.metamodel.createdby;
                created_date = DateTime.Now.ToString("o");
            }
            XAttribute createdBy = new XAttribute("createdBy", created_by);

            string actual_created_date = created_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_created_date = actual_created_date + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actual_created_date);
            XAttribute Att_submitted = new XAttribute("submittedBy", "");
            XAttribute Att_name = new XAttribute("name", link_name);

            link_Hierachie.Add(modifiedBy);
            link_Hierachie.Add(Att_modified);
            link_Hierachie.Add(createdBy);
            link_Hierachie.Add(Att_created);
            link_Hierachie.Add(Att_submitted);
            link_Hierachie.Add(Att_name);

            //////////////
            //Source erzeugen
            XElement source = new XElement("source");
            link_Hierachie.Add(source);
            XElement source_Klasse = new XElement("Klasse", "Systemelement");
            source.Add(source_Klasse);
            XElement source_puid = new XElement("puid", "");
            source.Add(source_puid);
            string source_ID_string = "";
            if (Client_object_id == null && Supplier_object_id == null)
            {
                source_ID_string = tagged.Get_Tagged_Value(Client.Instantiate_GUID, "OBJECT_ID", repository);
            }
            else
            {
                source_ID_string = Client_object_id;
            }
            XElement source_ID = new XElement("objectid", source_ID_string);
            source.Add(source_ID);
            //Target erzeugen
            XElement target = new XElement("target");
            link_Hierachie.Add(target);
            XElement target_Klasse = new XElement("Klasse", "Systemelement");
            target.Add(target_Klasse);
            XElement target_puid = new XElement("puid", "");
            target.Add(source_puid);
            string target_id_String = "";
            if (Client_object_id == null && Supplier_object_id == null)
            {
                target_id_String = tagged.Get_Tagged_Value(Supplier.Instantiate_GUID, "OBJECT_ID", repository);
            }
            else
            {
                target_id_String = Supplier_object_id;
            }
            XElement target_ID = new XElement("objectid", target_id_String);
            target.Add(target_ID);
        }
        /// <summary>
        /// Es wird ein Link zwischen AFo und System erzeugt
        /// </summary>
        /// <param name="links"></param>
        /// <param name="repository"></param>
        /// <param name="Client_GUID"></param>
        /// <param name="Supplier"></param>
        /// <param name="link_name"></param>
        private void Create_Link_Afo_Sys(XElement links, EA.Repository repository, string Client_GUID, NodeType Supplier, string Supplier_GUID)
        {
            string link_name = this.Metamodel_Base.m_Connectoren_Req7[6];
            //Hilfsvariablen
            TaggedValue tagged = new TaggedValue(this.Data.metamodel, this.Data);
            string Classifier_ID = "";
            string Instantiate_ID = "";

            if (Supplier == null) //Zwischen Afo und Instanz aus der Decomposition
            {
                Classifier_ID = Supplier_GUID;
                Instantiate_ID = Supplier_GUID;
            }
            else //AFo --> Klasse wie Logical oder Capaility
            {
                Classifier_ID = Supplier.Classifier_ID;
                Instantiate_ID = Supplier.Instantiate_GUID;
            }

            XElement link_Hierachie = new XElement("link");
            links.Add(link_Hierachie);
            //Atribute von link
            string ansprechpartner = tagged.Get_Tagged_Value(Classifier_ID, "SYS_ANSPRECHPARTNER", repository);
            if(ansprechpartner == null)
            {
                Repsoitory_Elements.Repository_Element repository_Element = new Repository_Element();
                repository_Element.Classifier_ID = Classifier_ID;
                ansprechpartner = repository_Element.Get_Author(this.Data);

            }
            XAttribute modifiedBy = new XAttribute("modifiedBy", ansprechpartner);
            string modified_date = repository.GetElementByGuid(Classifier_ID).Modified.ToString("o");
            string actual_modified_date = modified_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_modified_date = actual_modified_date + "+02:00";
            }
            XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
            XAttribute createdBy = new XAttribute("createdBy", ansprechpartner);
            string created_date = repository.GetElementByGuid(Classifier_ID).Modified.ToString("o");
            string actual_created_date = created_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_created_date = actual_created_date + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actual_created_date);
            XAttribute Att_submitted = new XAttribute("submittedBy", "");
            XAttribute Att_name = new XAttribute("name", link_name);

            link_Hierachie.Add(modifiedBy);
            link_Hierachie.Add(Att_modified);
            link_Hierachie.Add(createdBy);
            link_Hierachie.Add(Att_created);
            link_Hierachie.Add(Att_submitted);
            link_Hierachie.Add(Att_name);

            //////////////
            //Source erzeugen
            XElement source = new XElement("source");
            link_Hierachie.Add(source);
            XElement source_Klasse = new XElement("Klasse", "Anforderung");
            source.Add(source_Klasse);
            XElement source_puid = new XElement("puid", "");
            source.Add(source_puid);
            XElement source_ID = new XElement("objectid", tagged.Get_Tagged_Value(Client_GUID, "OBJECT_ID", repository));
            source.Add(source_ID);
            //Target erzeugen
            XElement target = new XElement("target");
            link_Hierachie.Add(target);
            XElement target_Klasse = new XElement("Klasse", "Systemelement");
            target.Add(target_Klasse);
            XElement target_puid = new XElement("puid", "");
            target.Add(source_puid);
            XElement target_ID = new XElement("objectid", tagged.Get_Tagged_Value(Instantiate_ID, "OBJECT_ID", repository));
            target.Add(target_ID);
        }

        private void Create_Link_Afo_St(XElement links, EA.Repository repository, string Client_GUID, NodeType Supplier, string Supplier_GUID, Metamodel metamodel)
        {
            string link_name = this.Metamodel_Base.m_Connectoren_Req7[7];
            //Hilfsvariablen
            TaggedValue tagged = new TaggedValue(metamodel, this.Data);
            string Classifier_ID = "";
            string Instantiate_ID = "";

            if (Supplier == null) //Zwischen Afo und Instanz aus der Decomposition
            {
                Classifier_ID = Supplier_GUID;
                Instantiate_ID = Supplier_GUID;
            }
            else //AFo --> Klasse wie Logical oder Capaility
            {
                Classifier_ID = Supplier.Classifier_ID;
                Instantiate_ID = Supplier.Instantiate_GUID;
            }

            XElement link_Hierachie = new XElement("link");
            links.Add(link_Hierachie);
            //Atribute von link
            XAttribute modifiedBy = new XAttribute("modifiedBy", tagged.Get_Tagged_Value(Classifier_ID, "ST_ANSPRECHPARTNER", repository));
            string modified_date = repository.GetElementByGuid(Classifier_ID).Modified.ToString("o");
            string actual_modified_date = modified_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_modified_date = actual_modified_date + "+02:00";
            }
            XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
            XAttribute createdBy = new XAttribute("createdBy", tagged.Get_Tagged_Value(Classifier_ID, "ST_ANSPRECHPARTNER", repository));
            string created_date = repository.GetElementByGuid(Classifier_ID).Modified.ToString("o");
            string actual_created_date = created_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_created_date = actual_created_date + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actual_created_date);
            XAttribute Att_submitted = new XAttribute("submittedBy", "");
            XAttribute Att_name = new XAttribute("name", link_name);

            link_Hierachie.Add(modifiedBy);
            link_Hierachie.Add(Att_modified);
            link_Hierachie.Add(createdBy);
            link_Hierachie.Add(Att_created);
            link_Hierachie.Add(Att_submitted);
            link_Hierachie.Add(Att_name);

            //////////////
            //Source erzeugen
            XElement source = new XElement("source");
            link_Hierachie.Add(source);
            XElement source_Klasse = new XElement("Klasse", "Anforderung");
            source.Add(source_Klasse);
            XElement source_puid = new XElement("puid", "");
            source.Add(source_puid);
            XElement source_ID = new XElement("objectid", tagged.Get_Tagged_Value(Client_GUID, "OBJECT_ID", repository));
            source.Add(source_ID);
            //Target erzeugen
            XElement target = new XElement("target");
            link_Hierachie.Add(target);
            XElement target_Klasse = new XElement("Klasse", "Stakeholder");
            target.Add(target_Klasse);
            XElement target_puid = new XElement("puid", "");
            target.Add(source_puid);
            XElement target_ID = new XElement("objectid", tagged.Get_Tagged_Value(Instantiate_ID, "OBJECT_ID", repository));
            target.Add(target_ID);
        }

        private void Create_Link_Afo_Afo(Database database, Metamodel metamodel, XElement links, EA.Repository repository, string Client_GUID, string Supplier_GUID, string link_name)
        {
            // string source_ID_string = repository.GetElementByGuid(Client_GUID).ElementID.ToString();
            // string target_ID_string = repository.GetElementByGuid(Supplier_GUID).ElementID.ToString();

            Repository_Element source_req = new Repository_Element();
            source_req.Classifier_ID = Client_GUID;
            source_req.ID = source_req.Get_Object_ID(database);

            Repository_Element target_req = new Repository_Element();
            target_req.Classifier_ID = Supplier_GUID;
            target_req.ID = target_req.Get_Object_ID(database);

            string source_ID_string = source_req.Get_TV_Object_ID(database).ToString(); 
            string target_ID_string = target_req.Get_TV_Object_ID(database).ToString();
            //Hilfsvariablen
            TaggedValue tagged = new TaggedValue(metamodel, this.Data);

            XElement link_Hierachie = new XElement("link");
            links.Add(link_Hierachie);
            //Atribute von link
            XAttribute modifiedBy = new XAttribute("modifiedBy", metamodel.createdby);
            string modified_date = DateTime.Now.ToString("o");
            string actual_modified_date = modified_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_modified_date = actual_modified_date + "+02:00";
            }
            XAttribute Att_modified = new XAttribute("modified", actual_modified_date);
            XAttribute createdBy = new XAttribute("createdBy", metamodel.createdby);
            string created_date = DateTime.Now.ToString("o");
            string actual_created_date = created_date;
            if (modified_date.Split('+').Length == 1)
            {
                actual_created_date = actual_created_date + "+02:00";
            }
            XAttribute Att_created = new XAttribute("created", actual_created_date);
            XAttribute Att_submitted = new XAttribute("submittedBy", "");
            XAttribute Att_name = new XAttribute("name", link_name);

            link_Hierachie.Add(modifiedBy);
            link_Hierachie.Add(Att_modified);
            link_Hierachie.Add(createdBy);
            link_Hierachie.Add(Att_created);
            link_Hierachie.Add(Att_submitted);
            link_Hierachie.Add(Att_name);

            //////////////
            //Source erzeugen
            XElement source = new XElement("source");
            link_Hierachie.Add(source);
            XElement source_Klasse = new XElement("Klasse", "Anforderung");
            source.Add(source_Klasse);
            XElement source_puid = new XElement("puid", "");
            source.Add(source_puid);
            XElement source_ID = new XElement("objectid", source_ID_string);
            source.Add(source_ID);
            //Target erzeugen
            XElement target = new XElement("target");
            link_Hierachie.Add(target);
            XElement target_Klasse = new XElement("Klasse", "Anforderung");
            target.Add(target_Klasse);
            XElement target_puid = new XElement("puid", "");
            target.Add(source_puid);
            XElement target_ID = new XElement("objectid", target_ID_string);
            target.Add(target_ID);
        }

        private void Export_Links_Requirement(Database database, string Link_Name, string Client_ID, List<Requirement> m_Requirements, XElement Links, Metamodel metamodel, EA.Repository repository)
        {
            if (m_Requirements.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.m_Requirement.Contains(m_Requirements[i1]) == true)
                    {
                        Create_Link_Afo_Afo(database,metamodel, Links, repository, Client_ID, m_Requirements[i1].Classifier_ID, Link_Name);
                    }

                    i1++;
                } while (i1 < m_Requirements.Count);
            }
            //Create_Link_Afo_Afo
        }
        #endregion Add_links

        #region Decomposition
        /// <summary>
        /// Rekursiver Ablauf der Baumstruktur der Decomposition. Es werden dazu die Knoten unter items erzeugt und die passenden Knoten und links abegelegt.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="links"></param>
        /// <param name="recent"></param>
        /// <param name="repository"></param>
        private void Decomposition_rekursiv(XElement items, XElement links, NodeType recent, EA.Repository repository, Metamodel metamodel, Database Data, Loading_OpArch loading)
        {
            Repository_Connector repository_Connector = new Repository_Connector();
         //   XML xML = new XML();

            if (recent.m_Child.Count > 0)
            {
                int i1 = 0;
                do
                {
                    string sys = recent.m_Child[i1].Get_Name( this.Data);

                    loading.label2.Text = sys;
                    loading.label2.Refresh();

                    if (Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                    {
                        //Sys-Element zu items hinzufügen
                        XElement recent_Sys = this.Add_Sys(recent.m_Child[i1], repository, metamodel);
                        items.Add(recent_Sys);
                    }


                    //Link Decomposition erzeugen
                    if (Data.link_decomposition == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                    {
                        Create_Link_Sys(Data, links, repository, recent, recent.m_Child[i1], this.Metamodel_Base.m_Connectoren_Req7[5], null, null);
                    }

                    ////////////////
                    ///Kinderelemente des aktuellen Elementes betrachten
                   // if(recent.m_Child[i1].RPI_Export == true)
                    {
                        Decomposition_rekursiv(items, links, recent.m_Child[i1], repository, metamodel, Data, loading);
                    }
               

                 //   if(recent.m_Child[i1].RPI_Export == true)
                    {
                        //Link vom recent_Child System zu seinen Afo ziehen
                        if (Data.link_afo_sys == true || Data.afo_interface_xac == true || Data.afo_funktional_xac == true || Data.afo_design_xac == true || Data.afo_user_xac == true || Data.afo_process_xac == true ||Data.afo_umwelt_xac == true)
                        {
                            //link unidirektionale Schnittstellen
                            if (recent.m_Child[i1].m_Element_Interface.Count > 0)
                            {
                                #region Unidirektionalle Schnittstellen
                                loading.label2.Text = sys + " - Unidirektionale Schnittstellen";
                                loading.label2.Refresh();

                                int i2 = 0;
                                do
                                {
                                    if (recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send.Count > 0 && recent.m_Child[i1].RPI_Export == true)
                                    {
                                        if(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].RPI_Export == true)
                                        {
                                            #region Afo send zur xml Datei inl. Konnektor (send)
                                            //Afo_send hinzufügen
                                            if (Data.afo_interface_xac == true && this.m_Requirement.Contains(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0]) == false)
                                            {
                                                this.m_Requirement.Add(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0]);
                                                XElement recent_Afo_send = this.Add_AFo(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0], repository, Data.metamodel);
                                                items.Add(recent_Afo_send);
                                                //Im Algemeinen Link Export
                                                /*
                                                if (Data.link_afo_afo == true)
                                                {
                                                    string SQL_send = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = " + repository.GetElementByGuid(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID).ElementID + " AND Stereotype IN" +xML.SQL_IN_Array( Data.metamodel.Send_Stereotype) + " AND Connector_Type IN" +xML.SQL_IN_Array( Data.metamodel.Send_Type) + ";";
                                                    string xml_String_send = repository.SQLQuery(SQL_send);
                                                    List<string> m_End_ID = xML.Xml_Read_Attribut("ea_guid", xml_String_send);

                                                    if (m_End_ID != null)
                                                    {
                                                        //MessageBox.Show(m_End_ID.Count.ToString());

                                                        int i5 = 0;
                                                        do
                                                        {
                                                            EA.Element recent2 = repository.GetElementByID(repository.GetConnectorByGuid(m_End_ID[i5]).SupplierID);

                                                            //Für die SChnittstellen aktuell
                                                            if (Data.metamodel.Requirement_Interface_Stereotype == recent2.Stereotype)
                                                            {
                                                                this.Create_Link_Afo_Afo(Data.metamodel, links, repository, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID, recent2.ElementGUID, Data.metamodel.m_Connectren_Require7[2]);
                                                            }


                                                            i5++;
                                                        } while (i5 < m_End_ID.Count);
                                                    }
                                                }
                                                */
                                            }
                                            //Link Afo und Sys hinzufügen
                                            if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                                            {
                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID, recent.m_Child[i1], null);
                                            }
                                            //Link Afo_Capability
                                            if (Data.link_afo_cap == true && Data.capability_xac == true)
                                            {
                                                //Schleife über alle Capability einer AFo
                                                if (recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].m_Capability.Count > 0)
                                                {
                                                    int c1 = 0;
                                                    do
                                                    {
                                                        var test = "test";
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID, null, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].m_Capability[c1].Classifier_ID);
                                                        c1++;
                                                    } while (c1 < recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].m_Capability.Count);
                                                }
                                            }
                                            //Link Afo_Logical
                                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                                            {
                                                List<Logical> m_Logical = recent.m_Child[i1].m_Element_Interface[i2].Get_Logicals_Unidirektional(repository, Data);
                                                if (m_Logical != null)
                                                {
                                                    int l1 = 0;
                                                    do
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Send[0].Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                        l1++;
                                                    } while (l1 < m_Logical.Count);
                                                }
                                            }
                                            #endregion
                                        }

                                    }
                                    if (recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive.Count > 0 && recent.m_Child[i1].m_Element_Interface[i2].Supplier.RPI_Export == true)
                                    {
                                        if(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0].RPI_Export == true )
                                        {
                                            #region Afo_receive ink. Konnektor (send) hinzufügen
                                            //Afo_receive hinzufügen
                                            if (Data.afo_interface_xac == true && this.m_Requirement.Contains(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0]) == false)
                                            {
                                                this.m_Requirement.Add(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0]);
                                                XElement recent_Afo_receive = this.Add_AFo(recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0], repository, Data.metamodel);
                                                items.Add(recent_Afo_receive);
                                            }
                                            if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].m_Element_Interface[i2].Supplier.RPI_Export)
                                            {
                                                //Link Afo und Sys hinzufügen
                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Classifier_ID, recent.m_Child[i1].m_Element_Interface[i2].Supplier, null);
                                            }
                                            //Link Afo_Capability
                                            if (Data.link_afo_cap == true && Data.capability_xac == true)
                                            {
                                                //Schleife über alle Capability einer AFo
                                                if (recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0].m_Capability.Count > 0)
                                                {
                                                    int c1 = 0;
                                                    do
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Classifier_ID, null, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0].m_Capability[c1].Classifier_ID);
                                                        c1++;
                                                    } while (c1 < recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0].m_Capability.Count);
                                                }
                                            }
                                            //Link Afo_Logical
                                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                                            {
                                                List<Logical> m_Logical = recent.m_Child[i1].m_Element_Interface[i2].Get_Logicals_Unidirektional(repository, Data);
                                                if (m_Logical != null)
                                                {
                                                    int l1 = 0;
                                                    do
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface[i2].m_Requirement_Interface_Receive[0].Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                        l1++;
                                                    } while (l1 < m_Logical.Count);
                                                }
                                            }
                                            #endregion
                                        }

                                    }

                                    i2++;
                                } while (i2 < recent.m_Child[i1].m_Element_Interface.Count);
                                #endregion
                            }
                            //link bidirektionale SChnittstellen
                            if (recent.m_Child[i1].m_Element_Interface_Bidirectional.Count > 0 && recent.m_Child[i1].RPI_Export == true)
                            {
                                #region Bidirektionale Schnittstellen
                                loading.label2.Text = sys + " - Bidirektionale Schnittstellen";
                                loading.label2.Refresh();

                                int i3 = 0;
                                do
                                {
                                    if (recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send.Count > 0)
                                    {
                                        if(recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].RPI_Export == true)
                                        {
                                            //Afo_send hinzufügen
                                            if (Data.afo_interface_xac == true && this.m_Requirement.Contains(recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0]) == false)
                                            {
                                                this.m_Requirement.Add(recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0]);
                                                XElement recent_Afo_bidi = this.Add_AFo(recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0], repository, Data.metamodel);
                                                items.Add(recent_Afo_bidi);
                                                //Send Konnektor
                                                //im Algemiene Link Export
                                                /*
                                                if (Data.link_afo_afo == true)
                                                {
                                                    string SQL_send = "SELECT ea_guid FROM t_connector WHERE Start_Object_ID = " + repository.GetElementByGuid(recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID).ElementID + " AND Stereotype IN" +xML.SQL_IN_Array( Data.metamodel.Send_Stereotype) + " AND Connector_Type IN" +xML.SQL_IN_Array( Data.metamodel.Send_Type) + ";";
                                                    string xml_String_send = repository.SQLQuery(SQL_send);
                                                    List<string> m_End_ID = xML.Xml_Read_Attribut("ea_guid", xml_String_send);

                                                    if (m_End_ID != null)
                                                    {
                                                        //MessageBox.Show(m_End_ID.Count.ToString());

                                                        int i5 = 0;
                                                        do
                                                        {
                                                            EA.Element recent2 = repository.GetElementByID(repository.GetConnectorByGuid(m_End_ID[i5]).SupplierID);

                                                            //Für die SChnittstellen aktuell
                                                            if (Data.metamodel.Requirement_Interface_Stereotype == recent2.Stereotype)
                                                            {
                                                                this.Create_Link_Afo_Afo(Data.metamodel, links, repository, recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID, recent2.ElementGUID, Data.metamodel.m_Connectren_Require7[2]);
                                                            }


                                                            i5++;
                                                        } while (i5 < m_End_ID.Count);
                                                    }
                                                }
                                                */
                                            }
                                            if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                                            {
                                                //Link Afo und Sys hinzufügen
                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID, recent.m_Child[i1], null);
                                            }
                                            //Link Afo_Capability
                                            if (Data.link_afo_cap == true && Data.capability_xac == true)
                                            {
                                                //Schleife über alle Capability einer AFo
                                                if (recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].m_Capability.Count > 0)
                                                {
                                                    int c1 = 0;
                                                    do
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID, null, recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].m_Capability[c1].Classifier_ID);
                                                        c1++;
                                                    } while (c1 < recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].m_Capability.Count);
                                                }
                                            }
                                            //Link Afo_Logical
                                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                                            {
                                                List<Logical> m_Logical = recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].Get_Logicals_Bidirektional(repository, Data);


                                                if (m_Logical != null)
                                                {
                                                    int l1 = 0;
                                                    do
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Interface_Bidirectional[i3].m_Requirement_Interface_Send[0].Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                        l1++;
                                                    } while (l1 < m_Logical.Count);
                                                }

                                            }
                                        }                                    
                                    }

                                    i3++;
                                } while (i3 < recent.m_Child[i1].m_Element_Interface_Bidirectional.Count);
                                #endregion
                            }

                            #region Funktionale Afo
                            if (recent.m_Child[i1].m_Element_Functional.Count > 0 && Data.afo_funktional_xac == true && recent.m_Child[i1].RPI_Export == true)
                            {
                                int i4 = 0;
                                do
                                {
                                    if (recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional.Count > 0)
                                    {
                                        if(recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0].RPI_Export == true)
                                        {
                                            this.m_Requirement.Add(recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0]);
                                            XElement recent_Afo_func = this.Add_AFo(recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0], repository, metamodel);
                                            items.Add(recent_Afo_func);

                                            if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                                            {
                                                //Link Afo und Sys hinzufügen
                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0].Classifier_ID, recent.m_Child[i1], null);
                                            }
                                            //Link Afo_Logical
                                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                                            {
                                                List<Logical> m_Logical = recent.m_Child[i1].m_Element_Functional[i4].m_Logical;


                                                if (m_Logical != null)
                                                {
                                                    if (m_Logical.Count > 0)
                                                    {
                                                        int l1 = 0;
                                                        do
                                                        {
                                                            if (recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional.Count > 0)
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0].Classifier_ID, null, m_Logical[l1].Classifier_ID);
                                                            }

                                                            l1++;
                                                        } while (l1 < m_Logical.Count);
                                                    }

                                                }

                                            }
                                            //Link Afo_Capability
                                            if (Data.link_afo_cap == true && Data.capability_xac == true)
                                            {
                                                //Schleife über alle Capability einer AFo
                                                if (recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional.Count > 0)
                                                {
                                                    if (recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0].m_Capability.Count > 0)
                                                    {
                                                        int c1 = 0;
                                                        do
                                                        {
                                                            Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0].Classifier_ID, null, recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0].m_Capability[c1].Classifier_ID);
                                                            c1++;
                                                        } while (c1 < recent.m_Child[i1].m_Element_Functional[i4].m_Requirement_Functional[0].m_Capability.Count);
                                                    }
                                                }

                                            }
                                        }
                                      
                                    }
                                   

                                    i4++;
                                } while (i4 < recent.m_Child[i1].m_Element_Functional.Count);



                            }
                            #endregion Funktionale Afo

                            #region  User Afo
                            if (recent.m_Child[i1].m_Element_User.Count > 0 && Data.afo_user_xac == true && recent.m_Child[i1].RPI_Export == true)
                            {
                                int i4 = 0;
                                do
                                {
                                    if (recent.m_Child[i1].m_Element_User[i4].m_Requirement_User.Count > 0)
                                    {
                                        int i5 = 0;
                                        do
                                        {
                                            if(recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i5].RPI_Export == true)
                                            {
                                                this.m_Requirement.Add(recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i5]);
                                                XElement recent_Afo_func = this.Add_AFo(recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i5], repository, Data.metamodel);
                                                items.Add(recent_Afo_func);

                                                if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                                                {
                                                    //Link Afo und Sys hinzufügen
                                                    Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i5].Classifier_ID, recent.m_Child[i1], null);
                                                }
                                            }
                                            i5++;
                                        } while (i5 < recent.m_Child[i1].m_Element_User[i4].m_Requirement_User.Count);
                                    }
                                    //Link Afo_Logical
                                    if (Data.link_afo_logical == true && Data.logical_xac == true)
                                    {
                                        List<Logical> m_Logical = recent.m_Child[i1].m_Element_User[i4].m_Logical;


                                        if (m_Logical != null)
                                        {
                                            if (m_Logical.Count > 0)
                                            {
                                                int l1 = 0;
                                                do
                                                {
                                                    if (recent.m_Child[i1].m_Element_User[i4].m_Requirement_User.Count > 0)
                                                    {
                                                        int i6 = 0;
                                                        do
                                                        {
                                                            if(recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i6].RPI_Export == true)
                                                            {
                                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i6].Classifier_ID, null, m_Logical[l1].Classifier_ID);
                                                            }

                                                            i6++;
                                                        } while (i6 < recent.m_Child[i1].m_Element_User[i4].m_Requirement_User.Count);
                                                    }

                                                    l1++;
                                                } while (l1 < m_Logical.Count);
                                            }

                                        }

                                    }
                                    //Link Afo_Capability
                                    if (Data.link_afo_cap == true && Data.capability_xac == true)
                                    {
                                        //Schleife über alle Capability einer AFo
                                        if (recent.m_Child[i1].m_Element_User[i4].m_Requirement_User.Count > 0)
                                        {
                                            int i7 = 0;
                                            do
                                            {
                                                if (recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i7].m_Capability.Count > 0)
                                                {
                                                    int c1 = 0;
                                                    do
                                                    {
                                                        if(recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i7].RPI_Export == true)
                                                        {
                                                            Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i7].Classifier_ID, null, recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i7].m_Capability[c1].Classifier_ID);

                                                        }
                                                        c1++;
                                                    } while (c1 < recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i7].m_Capability.Count);
                                                }

                                                i7++;
                                            } while (i7 < recent.m_Child[i1].m_Element_User[i4].m_Requirement_User.Count);
                                        }

                                    }
                                    //Link Afo --> Stakeholder
                                    if (Data.link_afo_st == true && Data.stakeholder_xac == true)
                                    {
                                        //Schleife über alle Stakeholder einer AFo
                                        if (recent.m_Child[i1].m_Element_User[i4].m_Requirement_User.Count > 0)
                                        {
                                            int i7 = 0;
                                            do
                                            {
                                                if (recent.m_Child[i1].m_Element_User[i4].m_Client_ST.Count > 0)
                                                {
                                                    if(recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i7].RPI_Export == true)
                                                    {
                                                        int c1 = 0;
                                                        do
                                                        {
                                                            if (repository_Connector.Check_Dependency(recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i7].Classifier_ID, recent.m_Child[i1].m_Element_User[i4].m_Client_ST[c1].Classifier_ID, Data.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), Data.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), Data, Data.metamodel.m_Derived_Element[0].direction) != null)
                                                            {
                                                                Create_Link_Afo_St(links, repository, recent.m_Child[i1].m_Element_User[i4].m_Requirement_User[i7].Classifier_ID, null, recent.m_Child[i1].m_Element_User[i4].m_Client_ST[c1].Classifier_ID, Data.metamodel);
                                                            }
                                                            c1++;
                                                        } while (c1 < recent.m_Child[i1].m_Element_User[i4].m_Client_ST.Count);
                                                    }
                                                  
                                                }

                                                i7++;
                                            } while (i7 < recent.m_Child[i1].m_Element_User[i4].m_Requirement_User.Count);
                                        }
                                    }

                                    i4++;
                                } while (i4 < recent.m_Child[i1].m_Element_User.Count);



                            }
                            #endregion User AFo

                            #region Design AFO
                            if (recent.m_Child[i1].m_Design.Count > 0 && Data.afo_design_xac == true && recent.m_Child[i1].RPI_Export == true)
                            {
                                int d1 = 0;
                                do
                                {
                                    #region Add Afo
                                    if (recent.m_Child[i1].m_Design[d1].requirement != null)
                                    {
                                        if(recent.m_Child[i1].m_Design[d1].requirement.RPI_Export == true)
                                        {
                                            this.m_GUID_Requirement.Add(recent.m_Child[i1].m_Design[d1].requirement.Classifier_ID);
                                            this.m_Requirement.Add(recent.m_Child[i1].m_Design[d1].requirement);
                                            XElement recent_Design = this.Add_AFo(recent.m_Child[i1].m_Design[d1].requirement, repository, Data.metamodel);
                                            items.Add(recent_Design);
                                            #region Add Link afo_sys
                                            if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                                            {
                                                //Link Afo und Sys hinzufügen
                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Design[d1].requirement.Classifier_ID, recent.m_Child[i1].m_Design[d1].NodeType, null);
                                            }
                                            #endregion Add Link afo_sys
                                            //Link Afo_Capability
                                            #region Add Link afo_cap
                                            if (Data.link_afo_cap == true && Data.capability_xac == true)
                                            {
                                                //Schleife über alle Capability einer AFo
                                                if (recent.m_Child[i1].m_Design[d1].capability != null)
                                                {
                                                    Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Design[d1].requirement.Classifier_ID, null, recent.m_Child[i1].m_Design[d1].capability.Classifier_ID);

                                                }
                                            }
                                            #endregion Add Link afo_cap
                                            //Link Afo_Logical
                                            #region Add Link afo_log
                                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                                            {
                                                List<Logical> m_Logical = recent.m_Child[i1].m_Design[d1].m_Logical;
                                                if (m_Logical != null)
                                                {
                                                    int l1 = 0;
                                                    do
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Design[d1].requirement.Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                        l1++;
                                                    } while (l1 < m_Logical.Count);
                                                }
                                            }
                                            #endregion Add Link afo_sys
                                        }

                                    }
                                    #endregion Add AFO


                                    d1++;
                                } while (d1 < recent.m_Child[i1].m_Design.Count);

                            }
                            #endregion Design AFO

                            #region Process AFO
                            if (recent.m_Child[i1].m_Element_Functional.Count > 0 && Data.afo_process_xac == true && recent.m_Child[i1].RPI_Export == true)
                            {
                                int d1 = 0;
                                do
                                {
                                    List<OperationalConstraint> m_opcon = recent.m_Element_Functional[d1].Supplier.Get_Process_Constraint(recent);

                                    if(m_opcon.Count > 0)
                                    {
                                        int o1 = 0;
                                        do
                                        {
                                            List<Requirement_Non_Functional> m_req = recent.m_Element_Functional[d1].Supplier.Element_Process_Get_Requirement(recent, m_opcon[o1]);

                                            if (m_req != null)
                                            {
                                                if (m_req.Count > 0)
                                                {
                                                    int d2 = 0;
                                                    do
                                                    {
                                                        #region Add Afo
                                                        if (m_req[d2] != null)
                                                        {
                                                            if (m_req[d2].RPI_Export == true)
                                                            {
                                                                this.m_GUID_Requirement.Add(m_req[d2].Classifier_ID);
                                                                this.m_Requirement.Add(m_req[d2]);
                                                                XElement recent_Design = this.Add_AFo(m_req[d2], repository, Data.metamodel);
                                                                items.Add(recent_Design);
                                                                #region Add Link afo_sys
                                                                if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                                                                {
                                                                    //Link Afo und Sys hinzufügen
                                                                    Create_Link_Afo_Sys(links, repository, m_req[d2].Classifier_ID, recent, null);
                                                                }
                                                                #endregion Add Link afo_sys
                                                                //Link Afo_Capability
                                                                #region Add Link afo_cap
                                                                if (Data.link_afo_cap == true && Data.capability_xac == true)
                                                                {
                                                                    //Schleife über alle Capability einer AFo
                                                                    if (m_req[d2].m_Capability.Count > 0)
                                                                    {
                                                                        Create_Link_Afo_Sys(links, repository, m_req[d2].Classifier_ID, null, m_req[d2].m_Capability[0].Classifier_ID);

                                                                    }
                                                                }
                                                                #endregion Add Link afo_cap
                                                                //Link Afo_Logical
                                                                #region Add Link afo_log
                                                                if (Data.link_afo_logical == true && Data.logical_xac == true)
                                                                {
                                                                    List<Logical> m_Logical = recent.m_Child[i1].m_Element_Functional[d1].m_Logical;
                                                                    if (m_Logical != null)
                                                                    {
                                                                        int l1 = 0;
                                                                        do
                                                                        {
                                                                            Create_Link_Afo_Sys(links, repository, m_req[d2].Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                                            l1++;
                                                                        } while (l1 < m_Logical.Count);
                                                                    }
                                                                }
                                                                #endregion Add Link afo_sys
                                                            }

                                                        }
                                                        #endregion Add AFO


                                                        d2++;
                                                    } while (d2 < m_req.Count);
                                                }


                                            }
                                            o1++;
                                        } while (o1 < m_opcon.Count);

                                    
                                    }

                                   
                                 

                                   
                                    d1++;
                                } while (d1 < recent.m_Child[i1].m_Element_Functional.Count);

                            }
                            #endregion Process AFO

                            #region Umwelt AFO
                            if (recent.m_Child[i1].m_Enviromental.Count > 0 && Data.afo_umwelt_xac == true && recent.m_Child[i1].RPI_Export == true)
                            {
                                int d1 = 0;
                                do
                                {
                                    #region Add Afo
                                    if (recent.m_Child[i1].m_Enviromental[d1].requirement != null)
                                    {
                                        if (recent.m_Child[i1].m_Enviromental[d1].requirement.RPI_Export == true)
                                        {
                                            this.m_GUID_Requirement.Add(recent.m_Child[i1].m_Enviromental[d1].requirement.Classifier_ID);
                                            this.m_Requirement.Add(recent.m_Child[i1].m_Enviromental[d1].requirement);
                                            XElement recent_Design = this.Add_AFo(recent.m_Child[i1].m_Enviromental[d1].requirement, repository, Data.metamodel);
                                            items.Add(recent_Design);
                                            #region Add Link afo_sys
                                            if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                                            {
                                                //Link Afo und Sys hinzufügen
                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Enviromental[d1].requirement.Classifier_ID, recent.m_Child[i1].m_Enviromental[d1].NodeType, null);
                                            }
                                            #endregion Add Link afo_sys
                                            //Link Afo_Capability
                                            #region Add Link afo_cap
                                            if (Data.link_afo_cap == true && Data.capability_xac == true)
                                            {
                                                //Schleife über alle Capability einer AFo
                                                if (recent.m_Child[i1].m_Design[d1].capability != null)
                                                {
                                                    Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Enviromental[d1].requirement.Classifier_ID, null, recent.m_Child[i1].m_Enviromental[d1].capability.Classifier_ID);

                                                }
                                            }
                                            #endregion Add Link afo_cap
                                            //Link Afo_Logical
                                            #region Add Link afo_log
                                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                                            {
                                                List<Logical> m_Logical = recent.m_Child[i1].m_Enviromental[d1].m_Logical;
                                                if (m_Logical != null)
                                                {
                                                    int l1 = 0;
                                                    do
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Enviromental[d1].requirement.Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                        l1++;
                                                    } while (l1 < m_Logical.Count);
                                                }
                                            }
                                            #endregion Add Link afo_sys
                                        }

                                    }
                                    #endregion Add AFO


                                    d1++;
                                } while (d1 < recent.m_Child[i1].m_Enviromental.Count);

                            }
                            #endregion Umwelt AFO

                            #region Typvertreter AFO
                            if (recent.m_Child[i1].m_Typvertreter.Count > 0 && Data.afo_typevertreter_xac == true && recent.m_Child[i1].RPI_Export == true)
                            {
                                int d1 = 0;
                                do
                                {
                                    #region Add Afo
                                    if (recent.m_Child[i1].m_Typvertreter[d1].requirement != null)
                                    {
                                        if (recent.m_Child[i1].m_Typvertreter[d1].requirement.RPI_Export == true)
                                        {
                                            this.m_GUID_Requirement.Add(recent.m_Child[i1].m_Typvertreter[d1].requirement.Classifier_ID);
                                            this.m_Requirement.Add(recent.m_Child[i1].m_Typvertreter[d1].requirement);
                                            XElement recent_Type = this.Add_AFo(recent.m_Child[i1].m_Typvertreter[d1].requirement, repository, Data.metamodel);
                                            items.Add(recent_Type);
                                            #region Add Link afo_sys
                                            if (Data.link_afo_sys == true && Data.sys_xac == true && recent.m_Child[i1].RPI_Export == true)
                                            {
                                                //Link Afo und Sys hinzufügen
                                                Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Typvertreter[d1].requirement.Classifier_ID, recent.m_Child[i1].m_Typvertreter[d1].NodeType, null);
                                            }
                                            #endregion Add Link afo_sys
                                            //Link Afo_Capability
                                            #region Add Link afo_cap
                                            if (Data.link_afo_cap == true && Data.capability_xac == true)
                                            {
                                                //Schleife über alle Capability einer AFo
                                                if (recent.m_Child[i1].m_Typvertreter[d1].capability != null)
                                                {
                                                    Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Typvertreter[d1].requirement.Classifier_ID, null, recent.m_Child[i1].m_Typvertreter[d1].capability.Classifier_ID);

                                                }
                                            }
                                            #endregion Add Link afo_cap
                                            //Link Afo_Logical
                                            #region Add Link afo_log
                                            if (Data.link_afo_logical == true && Data.logical_xac == true)
                                            {
                                                List<Logical> m_Logical = recent.m_Child[i1].m_Typvertreter[d1].m_Logical;
                                                if (m_Logical != null)
                                                {
                                                    int l1 = 0;
                                                    do
                                                    {
                                                        Create_Link_Afo_Sys(links, repository, recent.m_Child[i1].m_Typvertreter[d1].requirement.Classifier_ID, null, m_Logical[l1].Classifier_ID);

                                                        l1++;
                                                    } while (l1 < m_Logical.Count);
                                                }
                                            }
                                            #endregion Add Link afo_sys
                                        }

                                    }
                                    #endregion Add AFO


                                    d1++;
                                } while (d1 < recent.m_Child[i1].m_Typvertreter.Count);

                            }
                            #endregion Typevertreter AFO
                        }

                    }


                    loading.progressBar1.PerformStep();
                    loading.progressBar1.Refresh();

                    i1++;
                } while (i1 < recent.m_Child.Count);
            }
        }
        #endregion Decomposition

        #region Add xml spezifisch
        /// <summary>
        /// Hinzufügen der XML Werte für ein item
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        private XElement Add_NameValues(string GUID ,Requirement recent_req, Repository_Class recent,  Stakeholder recent_ST, Glossar_Element glossar, EA.Repository repository, bool sys, bool afo, bool logical, bool capability, bool stakeholder, bool glossar_xac, Metamodel metamodel)
        {
            //Rueckgabewert
            XElement NameValues = new XElement("NameValues");
            //Hilfsvarable
            TaggedValue Tagged = new TaggedValue(metamodel, this.Data);
            //Aktuelles Element
            //var Element = repository.GetElementByGuid(GUID);

            //Betrachtung eines Systemelements
            #region Systemelement
            if (sys == true || logical == true || capability == true)
            {
                string sys_GUID_Class = "";
                string sys_GUID_Instantiate = "";

                if (sys == true)
                {
                    sys_GUID_Class = recent.Classifier_ID;
                    sys_GUID_Instantiate = recent.Instantiate_GUID;
                }

                if (logical == true || capability == true)
                {
                    sys_GUID_Class = GUID;
                    sys_GUID_Instantiate = GUID;
                }

                Repository_Class rec_Class = new Repository_Class();
                rec_Class = recent;


                Repository_Class rec_Ins = new Repository_Class();
                if (sys_GUID_Class != sys_GUID_Instantiate)
                {
                    rec_Ins.Classifier_ID = sys_GUID_Instantiate;
                    rec_Ins.ID = rec_Ins.Get_Object_ID(Data);
                    rec_Ins.Get_TV_Instantiate(Data, repository);
                }
                else
                {
                    rec_Ins = rec_Class;
                }
               
                //SYS_FARBCODE
                if(rec_Ins.SYS_FARBCODE != null)
                {
                    if(rec_Ins.SYS_FARBCODE != "" && rec_Ins.SYS_FARBCODE != "" && rec_Ins.SYS_FARBCODE != "not set")
                    {
                        string SYS_FARBCODE = "";
                        SYS_FARBCODE = rec_Ins.SYS_FARBCODE;

                        NameValues.Add(Add_NameValue("SYS_FARBCODE", SYS_FARBCODE));
                    }
                   
                }

                //PUID
                NameValues.Add(Add_NameValue("PUID", ""));
                //OBJECT_ID
                string Object_ID = "";
                // if (Tagged.Get_Tagged_Value(sys_GUID_Instantiate, "OBJECT_ID", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Instantiate, "OBJECT_ID", repository) != "kein")
                if (rec_Ins.OBJECT_ID != null && rec_Ins.OBJECT_ID != "kein")
                {
                    Object_ID = rec_Ins.OBJECT_ID;
                }
                NameValues.Add(Add_NameValue("OBJECT_ID", Object_ID));
                //SYS_AG_ID
                string SYS_AG_ID = "";
                // if (Tagged.Get_Tagged_Value(sys_GUID_Instantiate, "SYS_AG_ID", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Instantiate, "SYS_AG_ID", repository) != "kein")
                if (rec_Ins.SYS_AG_ID != null && rec_Ins.SYS_AG_ID != "kein")
                {
                    SYS_AG_ID = rec_Ins.SYS_AG_ID;

                }
                NameValues.Add(Add_NameValue("SYS_AG_ID", SYS_AG_ID));
                //SYS_AN_ID
                string SYS_AN_ID = "";
                //  if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_AN_ID", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_AN_ID", repository) != "kein")
                if (rec_Ins.SYS_AN_ID != null && rec_Ins.SYS_AN_ID != "kein")
                {
                    SYS_AN_ID = rec_Ins.SYS_AN_ID;
                    if (SYS_AN_ID == "kein")
                    {
                        SYS_AN_ID = "";
                    }
                }
                NameValues.Add(Add_NameValue("SYS_AN_ID", SYS_AN_ID));
                //SYS_KUERZEL
                string SYS_KUERZEL = "";
                //if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_KUERZEL", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_KUERZEL", repository) != "kein")
                if (rec_Class.SYS_KUERZEL != null && rec_Class.SYS_KUERZEL != "kein")
                {
                    SYS_KUERZEL = rec_Class.SYS_KUERZEL;
                }
                NameValues.Add(Add_NameValue("SYS_KUERZEL", SYS_KUERZEL));
                //SYS_BEZEICHNUNG
                string SYS_BEZEICHNUNG = "";
                //if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_BEZEICHNUNG", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_BEZEICHNUNG", repository) != "kein")
                if (rec_Class.SYS_BEZEICHNUNG != null && rec_Class.SYS_BEZEICHNUNG != "kein")
                {
                    //SYS_BEZEICHNUNG = Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_BEZEICHNUNG", repository);
                    SYS_BEZEICHNUNG = rec_Class.SYS_BEZEICHNUNG;
                }
                NameValues.Add(Add_NameValue("SYS_BEZEICHNUNG", SYS_BEZEICHNUNG));
                //SYS_ARTIKEL
                string SYS_ARTIKEL = "";
                // if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_ARTIKEL", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_ARTIKEL", repository) != "kein")
                if (Data.SYS_ENUM.SYS_ARTIKEL[(int)rec_Class.SYS_ARTIKEL] != null && Data.SYS_ENUM.SYS_ARTIKEL[(int)rec_Class.SYS_ARTIKEL] != "kein")
                {
                    SYS_ARTIKEL = Data.SYS_ENUM.SYS_ARTIKEL[(int)rec_Class.SYS_ARTIKEL];
                }
                NameValues.Add(Add_NameValue("SYS_ARTIKEL", SYS_ARTIKEL));
                //SYS_DETAILSTUFE
                string SYS_DETAILSTUFE = "";
                // if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_DETAILSTUFE", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_DETAILSTUFE", repository) != "kein")
                if (Data.SYS_ENUM.SYS_DETAILSTUFE[(int)rec_Class.SYS_DETAILSTUFE] != null && Data.SYS_ENUM.SYS_DETAILSTUFE[(int)rec_Class.SYS_DETAILSTUFE] != "kein")
                {
                    SYS_DETAILSTUFE = Data.SYS_ENUM.SYS_DETAILSTUFE[(int)rec_Class.SYS_DETAILSTUFE];
                }
                NameValues.Add(Add_NameValue("SYS_DETAILSTUFE", SYS_DETAILSTUFE));
                //SYS_TYP
                string SYS_TYP = "";
                //if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_TYP", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_TYP", repository) != "kein")
                if (Data.SYS_ENUM.SYS_TYP[(int)rec_Class.SYS_TYP] != null && Data.SYS_ENUM.SYS_TYP[(int)rec_Class.SYS_TYP] != "kein")
                {
                    SYS_TYP = Data.SYS_ENUM.SYS_TYP[(int)rec_Class.SYS_TYP];
                }
                NameValues.Add(Add_NameValue("SYS_TYP", SYS_TYP));
                //SYS_ERFUELLT_AFO
                string SYS_ERFUELLT_AFO = "";
                // if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_ERFUELLT_AFO", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_ERFUELLT_AFO", repository) != "kein")
                if (Data.SYS_ENUM.SYS_ERFUELLT_AFO[(int)rec_Class.SYS_ERFUELLT_AFO] != null && Data.SYS_ENUM.SYS_ERFUELLT_AFO[(int)rec_Class.SYS_ERFUELLT_AFO] != "kein")
                {
                    SYS_ERFUELLT_AFO = Data.SYS_ENUM.SYS_ERFUELLT_AFO[(int)rec_Class.SYS_ERFUELLT_AFO];
                }
                NameValues.Add(Add_NameValue("SYS_ERFUELLT_AFO", SYS_ERFUELLT_AFO));
                //SYS_SUBORDINATES_AFO
                string SYS_SUBORDINATES_AFO = "";
                //if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_SUBORDINATES_AFO", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_SUBORDINATES_AFO", repository) != "kein")
                if (Data.SYS_ENUM.SYS_SUBORDINATES_AFO[(int)rec_Class.SYS_SUBORDINATES_AFO] != null && Data.SYS_ENUM.SYS_SUBORDINATES_AFO[(int)rec_Class.SYS_SUBORDINATES_AFO] != "kein")
                {
                    SYS_SUBORDINATES_AFO = Data.SYS_ENUM.SYS_SUBORDINATES_AFO[(int)rec_Class.SYS_SUBORDINATES_AFO];
                }
                NameValues.Add(Add_NameValue("SYS_SUBORDINATES_AFO", SYS_SUBORDINATES_AFO));
                //SYS_KOMPONENTENTYP
                string SYS_KOMPONENTENTYP = "";
                //  if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_KOMPONENTENTYP", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_KOMPONENTENTYP", repository) != "kein")
                if (Data.SYS_ENUM.SYS_KOMPONENTENTYP[(int)rec_Class.SYS_KOMPONENTENTYP] != null && Data.SYS_ENUM.SYS_KOMPONENTENTYP[(int)rec_Class.SYS_KOMPONENTENTYP] != "kein")
                {
                    SYS_KOMPONENTENTYP = Data.SYS_ENUM.SYS_KOMPONENTENTYP[(int)rec_Class.SYS_KOMPONENTENTYP];
                }
                NameValues.Add(Add_NameValue("SYS_KOMPONENTENTYP", SYS_KOMPONENTENTYP));
                //SYS_STATUS
                string SYS_STATUS = "";
                // if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_STATUS", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_STATUS", repository) != "kein")
                if (Data.SYS_ENUM.SYS_STATUS[(int)rec_Class.SYS_STATUS] != null && Data.SYS_ENUM.SYS_STATUS[(int)rec_Class.SYS_STATUS] != "kein")
                {
                    SYS_STATUS = Data.SYS_ENUM.SYS_STATUS[(int)rec_Class.SYS_STATUS];
                }
                NameValues.Add(Add_NameValue("SYS_STATUS", SYS_STATUS));
                //IN_CATEGORY
                string IN_CATEGORY = "";
                // if (Tagged.Get_Tagged_Value(sys_GUID_Class, "IN_CATEGORY", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "IN_CATEGORY", repository) != "kein")
                if (Data.AFO_ENUM.IN_CATEGORY[(int)rec_Class.IN_CATEGORY] != null && Data.AFO_ENUM.IN_CATEGORY[(int)rec_Class.IN_CATEGORY] != "kein")
                {
                    IN_CATEGORY = Data.AFO_ENUM.IN_CATEGORY[(int)rec_Class.IN_CATEGORY];
                }
                NameValues.Add(Add_NameValue("IN_CATEGORY", IN_CATEGORY));
                //SYS_PRODUKT_STATUS
                string SYS_PRODUKT_STATUS = "";
                //  if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_PRODUKT_STATUS", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_PRODUKT_STATUS", repository) != "kein")
                if (Data.SYS_ENUM.SYS_PRODUKT_STATUS[(int)rec_Class.SYS_PRODUKT_STATUS] != null && Data.SYS_ENUM.SYS_PRODUKT_STATUS[(int)rec_Class.SYS_PRODUKT_STATUS] != "kein")
                {
                    SYS_PRODUKT_STATUS = Data.SYS_ENUM.SYS_PRODUKT_STATUS[(int)rec_Class.SYS_PRODUKT_STATUS];
                }
                NameValues.Add(Add_NameValue("SYS_PRODUKT_STATUS", SYS_PRODUKT_STATUS));
                //SYS_PRODUKT
                string SYS_PRODUKT = "";
                //  if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_PRODUKT", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_PRODUKT", repository) != "kein")
                if (rec_Class.SYS_PRODUKT != null && rec_Class.SYS_PRODUKT != "kein")
                {
                    SYS_PRODUKT = rec_Class.SYS_PRODUKT;
                }
                NameValues.Add(Add_NameValue("SYS_PRODUKT", SYS_PRODUKT));
                //B_KENNUNG
                string B_KENNUNG = "";
                //  if (Tagged.Get_Tagged_Value(sys_GUID_Class, "B_KENNUNG", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "B_KENNUNG", repository) != "kein")
                if (rec_Class.B_KENNUNG != null && rec_Class.B_KENNUNG != "kein")
                {
                    B_KENNUNG = rec_Class.B_KENNUNG;
                }
                NameValues.Add(Add_NameValue("B_KENNUNG", B_KENNUNG));
                //SYS_REL_GEWICHT
                string SYS_REL_GEWICHT = "";
                // if (Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_REL_GEWICHT", repository) != null && Tagged.Get_Tagged_Value(sys_GUID_Class, "SYS_REL_GEWICHT", repository) != "kein")
                if (rec_Class.SYS_REL_GEWICHT != null && rec_Class.SYS_REL_GEWICHT != "kein")
                {
                    SYS_REL_GEWICHT = rec_Class.SYS_REL_GEWICHT;
                }
                NameValues.Add(Add_NameValue("SYS_REL_GEWICHT", SYS_REL_GEWICHT));
            }
            #endregion Systemelement
            //Betrachtung einer Anforderung
            #region Anforderung
            if (afo == true)
            {
                string AFo_GUID = GUID;

                var Afo_Element = repository.GetElementByGuid(AFo_GUID);
                //////////////////////
                //NameValues
                //PUID
                string PUID = "";
                NameValues.Add(Add_NameValue("PUID", PUID));
                //OBJECT_ID
                string OBJECT_ID = "";
                //   if (Tagged.Get_Tagged_Value(AFo_GUID, "OBJECT_ID", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "OBJECT_ID", repository) != "kein")
                if (recent_req.OBJECT_ID != null && recent_req.OBJECT_ID != "kein")
                {
                    OBJECT_ID = recent_req.OBJECT_ID;
                }
                NameValues.Add(Add_NameValue("OBJECT_ID", OBJECT_ID));
                //AFO_AG_ID
                string AFO_AG_ID = "";
                //   if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_AG_ID", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_AG_ID", repository) != "kein")
                if (recent_req.AFO_AG_ID != null && recent_req.AFO_AG_ID != "kein")
                {
                    AFO_AG_ID = recent_req.AFO_AG_ID;
                }
                NameValues.Add(Add_NameValue("AFO_AG_ID", AFO_AG_ID));
                //AFO_AN_ID
                string AFO_AN_ID = "";
                //    if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_AN_ID", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_AN_ID", repository) != "kein")
                if (recent_req.AFO_AN_ID != null && recent_req.AFO_AN_ID != "kein")
                {
                    AFO_AN_ID = recent_req.AFO_AN_ID;
                }
                NameValues.Add(Add_NameValue("AFO_AN_ID", AFO_AN_ID));
                //AFO_TITEL
                string AFO_TITEL = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_TITEL", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_TITEL", repository) != "kein")
                if (recent_req.AFO_TITEL != null && recent_req.AFO_TITEL != "kein")
                {
                    AFO_TITEL = recent_req.AFO_TITEL;
                }
                NameValues.Add(Add_NameValue("AFO_TITEL", AFO_TITEL));
                //AFO_TEXT
                string AFO_TEXT = "";
                if (Afo_Element.Notes != null && Afo_Element.Notes != "kein")
                {
                    AFO_TEXT = Afo_Element.Notes;
                }
                NameValues.Add(Add_NameValue("AFO_TEXT", AFO_TEXT));
                //AFO_WV_PHASE
                string AFO_WV_PHASE = "";
                //   if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_WV_PHASE", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_WV_PHASE", repository) != "kein")
                if (recent_req.AFO_WV_PHASE != null && Data.AFO_ENUM.AFO_WV_PHASE[(int)recent_req.AFO_WV_PHASE] != "kein")
                {
                    AFO_WV_PHASE = Data.AFO_ENUM.AFO_WV_PHASE[(int)recent_req.AFO_WV_PHASE];
                }
                NameValues.Add(Add_NameValue("AFO_WV_PHASE", AFO_WV_PHASE));
                //W_AKTIVITAET
                string W_AKTIVITAET = "";
                //   if (Tagged.Get_Tagged_Value(AFo_GUID, "W_AKTIVITAET", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_AKTIVITAET", repository) != "kein")
                if (recent_req.W_AKTIVITAET != null && Data.AFO_ENUM.W_AKTIVITAET[(int)recent_req.W_AKTIVITAET] != "kein")
                {
                    W_AKTIVITAET = Data.AFO_ENUM.W_AKTIVITAET[(int)recent_req.W_AKTIVITAET];
                }
                NameValues.Add(Add_NameValue("W_AKTIVITAET", W_AKTIVITAET));
                //AFO_WV_ART
                string AFO_WV_ART = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_WV_ART", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_WV_ART", repository) != "kein")
                if (recent_req.AFO_WV_ART != null && Data.AFO_ENUM.AFO_WV_ART[(int)recent_req.AFO_WV_ART] != "kein")
                {
                    AFO_WV_ART = Data.AFO_ENUM.AFO_WV_ART[(int)recent_req.AFO_WV_ART];
                }
                NameValues.Add(Add_NameValue("AFO_WV_ART", AFO_WV_ART));
                //AFO_BEZUG
                string AFO_BEZUG = "";
                //    if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_BEZUG", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_BEZUG", repository) != "kein")
                if (recent_req.AFO_BEZUG != null && Data.AFO_ENUM.AFO_BEZUG[(int)recent_req.AFO_BEZUG] != "kein")
                {
                    AFO_BEZUG = Data.AFO_ENUM.AFO_BEZUG[(int)recent_req.AFO_BEZUG];
                }
                NameValues.Add(Add_NameValue("AFO_BEZUG", AFO_BEZUG));
                //AFO_DETAILSTUFE
                string AFO_DETAILSTUFE = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_DETAILSTUFE", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_DETAILSTUFE", repository) != "kein")
                if (recent_req.AFO_DETAILSTUFE != null && Data.AFO_ENUM.AFO_DETAILSTUFE[(int)recent_req.AFO_DETAILSTUFE] != "kein")
                {
                    AFO_DETAILSTUFE = Data.AFO_ENUM.AFO_DETAILSTUFE[(int)recent_req.AFO_DETAILSTUFE];
                }
                NameValues.Add(Add_NameValue("AFO_DETAILSTUFE", AFO_DETAILSTUFE));
                //AFO_FUNKTIONAL
                string AFO_FUNKTIONAL = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_FUNKTIONAL", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_FUNKTIONAL", repository) != "kein")
                if (recent_req.AFO_FUNKTIONAL != null && Data.AFO_ENUM.AFO_FUNKTIONAL[(int)recent_req.AFO_FUNKTIONAL] != "kein")
                {
                    AFO_FUNKTIONAL = Data.AFO_ENUM.AFO_FUNKTIONAL[(int)recent_req.AFO_FUNKTIONAL];
                }
                NameValues.Add(Add_NameValue("AFO_FUNKTIONAL", AFO_FUNKTIONAL));
                //AFO_KRITIKALITAET
                string AFO_KRITIKALITAET = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_KRITIKALITAET", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_KRITIKALITAET", repository) != "kein")
                if (recent_req.AFO_KRITIKALITAET != null && Data.AFO_ENUM.AFO_KRITIKALITAET[(int)recent_req.AFO_KRITIKALITAET] != "kein")
                {
                    AFO_KRITIKALITAET = Data.AFO_ENUM.AFO_KRITIKALITAET[(int)recent_req.AFO_KRITIKALITAET];
                }
                NameValues.Add(Add_NameValue("AFO_KRITIKALITAET", AFO_KRITIKALITAET));
                //AFO_WV_NACHWEISART

                if(Data.Nachweisart_xac == true)
                {
                    string AFO_WV_NACHWEISART = "";
                    if ((int)recent_req.AFO_WV_NACHWEISART != -1)
                    {
                        if (recent_req.AFO_WV_NACHWEISART != null && Data.AFO_ENUM.AFO_WV_NACHWEISART[(int)recent_req.AFO_WV_NACHWEISART] != "kein")
                        {
                            AFO_WV_NACHWEISART = Data.AFO_ENUM.AFO_WV_NACHWEISART[(int)recent_req.AFO_WV_NACHWEISART];
                        }
                    }
                    NameValues.Add(Add_NameValue("AFO_WV_NACHWEISART", AFO_WV_NACHWEISART));
                }
                //AFO_OPERATIVEBEWERTUNG
                string AFO_OPERATIVEBEWERTUNG = "";
                //   if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_OPERATIVEBEWERTUNG", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_OPERATIVEBEWERTUNG", repository) != "kein")
                if (recent_req.AFO_OPERATIVEBEWERTUNG != null && Data.AFO_ENUM.AFO_OPERATIVEBEWERTUNG[(int)recent_req.AFO_OPERATIVEBEWERTUNG] != "kein")
                {
                    AFO_OPERATIVEBEWERTUNG = Data.AFO_ENUM.AFO_OPERATIVEBEWERTUNG[(int)recent_req.AFO_OPERATIVEBEWERTUNG];
                }
                NameValues.Add(Add_NameValue("AFO_OPERATIVEBEWERTUNG", AFO_OPERATIVEBEWERTUNG));
                //AFO_PRIORITAET_VERGABE
                string AFO_PRIORITAET_VERGABE = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_PRIORITAET_VERGABE", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_PRIORITAET_VERGABE", repository) != "kein")
                if (recent_req.AFO_PRIORITAET_VERGABE != null && Data.AFO_ENUM.AFO_PRIORITAET_VERGABE[(int)recent_req.AFO_PRIORITAET_VERGABE] != "kein")
                {
                    AFO_PRIORITAET_VERGABE = Data.AFO_ENUM.AFO_PRIORITAET_VERGABE[(int)recent_req.AFO_PRIORITAET_VERGABE];
                }
                NameValues.Add(Add_NameValue("AFO_PRIORITAET_VERGABE", AFO_PRIORITAET_VERGABE));
                //AFO_PROJEKTROLLE
                string AFO_PROJEKTROLLE = "";
                //    if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_PROJEKTROLLE", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_PROJEKTROLLE", repository) != "kein")
                if (recent_req.AFO_PROJEKTROLLE != null && Data.AFO_ENUM.AFO_PROJEKTROLLE[(int)recent_req.AFO_PROJEKTROLLE] != "kein")
                {
                    AFO_PROJEKTROLLE = Data.AFO_ENUM.AFO_PROJEKTROLLE[(int)recent_req.AFO_PROJEKTROLLE];
                }
                NameValues.Add(Add_NameValue("AFO_PROJEKTROLLE", AFO_PROJEKTROLLE));
                //AFO_QS_STATUS
                string AFO_QS_STATUS = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_QS_STATUS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_QS_STATUS", repository) != "kein")
                if (recent_req.AFO_QS_STATUS != null && Data.AFO_ENUM.AFO_QS_STATUS[(int)recent_req.AFO_QS_STATUS] != "kein")
                {
                    AFO_QS_STATUS = Data.AFO_ENUM.AFO_QS_STATUS[(int)recent_req.AFO_QS_STATUS];
                }
                NameValues.Add(Add_NameValue("AFO_QS_STATUS", AFO_QS_STATUS));
                //AFO_STATUS
                string AFO_STATUS = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_STATUS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_STATUS", repository) != "kein")
                if (recent_req.AFO_STATUS != null && Data.AFO_ENUM.AFO_STATUS[(int)recent_req.AFO_STATUS] != "kein")
                {
                    AFO_STATUS = Data.AFO_ENUM.AFO_STATUS[(int)recent_req.AFO_STATUS];
                }
                NameValues.Add(Add_NameValue("AFO_STATUS", AFO_STATUS));
                //W_SUBJEKT
                string W_SUBJEKT = "";
                //     if (Tagged.Get_Tagged_Value(AFo_GUID, "W_SUBJEKT", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_SUBJEKT", repository) != "kein")
                if (recent_req.W_SUBJEKT != null && recent_req.W_SUBJEKT != "kein" && recent_req.W_SUBJEKT != "not set")
                {
                    W_SUBJEKT = recent_req.W_SUBJEKT;
                }
                NameValues.Add(Add_NameValue("W_SUBJEKT", W_SUBJEKT));
                //AFO_VERBINDLICHKEIT
                string AFO_VERBINDLICHKEIT = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_VERBINDLICHKEIT", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_VERBINDLICHKEIT", repository) != "kein")
                if (recent_req.AFO_VERBINDLICHKEIT != null && recent_req.AFO_VERBINDLICHKEIT != "kein" && recent_req.AFO_VERBINDLICHKEIT != "not set")
                {
                    AFO_VERBINDLICHKEIT = recent_req.AFO_VERBINDLICHKEIT;
                }
                NameValues.Add(Add_NameValue("AFO_VERBINDLICHKEIT", AFO_VERBINDLICHKEIT));
                //AFO_VERERBUNG
                string AFO_VERERBUNG = "";
                //   if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_VERERBUNG", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_VERERBUNG", repository) != "kein")
                if (recent_req.AFO_VERERBUNG != null && Data.AFO_ENUM.AFO_VERERBUNG[(int)recent_req.AFO_VERERBUNG] != "kein")
                {
                    AFO_VERERBUNG = Data.AFO_ENUM.AFO_VERERBUNG[(int)recent_req.AFO_VERERBUNG];
                }
                NameValues.Add(Add_NameValue("AFO_VERERBUNG", AFO_VERERBUNG));
                //IN_CATEGORY
                string IN_CATEGORY = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "IN_CATEGORY", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "IN_CATEGORY", repository) != "kein")
                if (recent_req.IN_CATEGORY != null && Data.AFO_ENUM.IN_CATEGORY[(int)recent_req.IN_CATEGORY] != "kein")
                {
                    IN_CATEGORY = Data.AFO_ENUM.IN_CATEGORY[(int)recent_req.IN_CATEGORY];
                }
                NameValues.Add(Add_NameValue("IN_CATEGORY", IN_CATEGORY));
                //AFO_CPM_PHASE
                string AFO_CPM_PHASE = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_CPM_PHASE", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_CPM_PHASE", repository) != "kein")
                if (recent_req.AFO_CPM_PHASE != null && Data.AFO_ENUM.AFO_CPM_PHASE[(int)recent_req.AFO_CPM_PHASE] != "kein")
                {
                    AFO_CPM_PHASE = Data.AFO_ENUM.AFO_CPM_PHASE[(int)recent_req.AFO_CPM_PHASE];
                }
                NameValues.Add(Add_NameValue("AFO_CPM_PHASE", AFO_CPM_PHASE));
                //W_RANDBEDINGUNG
                string W_RANDBEDINGUNG = "";
                //    if (Tagged.Get_Tagged_Value(AFo_GUID, "W_RANDBEDINGUNG", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_RANDBEDINGUNG", repository) != "kein")
                if (recent_req.W_RANDBEDINGUNG != null && recent_req.W_RANDBEDINGUNG != "kein" && recent_req.W_RANDBEDINGUNG != "not set")
                {
                    W_RANDBEDINGUNG = recent_req.W_RANDBEDINGUNG;
                }
                NameValues.Add(Add_NameValue("W_RANDBEDINGUNG", W_RANDBEDINGUNG));
                //W_SINGULAR
                string W_SINGULAR = "";
                //      if (Tagged.Get_Tagged_Value(AFo_GUID, "W_SINGULAR", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_SINGULAR", repository) != "kein")
                if (recent_req.W_SINGULAR != null)
                {
                    W_SINGULAR = recent_req.W_SINGULAR.ToString() ;
                }
                NameValues.Add(Add_NameValue("W_SINGULAR", W_SINGULAR));
                //W_NUTZENDER
                string W_NUTZENDER = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "W_NUTZENDER", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_NUTZENDER", repository) != "kein")
                if (recent_req.W_NUTZENDER != null && recent_req.W_NUTZENDER != "kein" && recent_req.W_NUTZENDER != "not set")
                {
                    W_NUTZENDER = recent_req.W_NUTZENDER;
                }
                NameValues.Add(Add_NameValue("W_NUTZENDER", W_NUTZENDER));
                //W_OBJEKT
                string W_OBJEKT = "";
                //   if (Tagged.Get_Tagged_Value(AFo_GUID, "W_OBJEKT", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_OBJEKT", repository) != "kein")
                if (recent_req.W_OBJEKT != null && recent_req.W_OBJEKT != "kein" && recent_req.W_OBJEKT != "not set")
                {
                    W_OBJEKT = recent_req.W_OBJEKT;
                }
                NameValues.Add(Add_NameValue("W_OBJEKT", W_OBJEKT));
                //W_ZU
                string W_ZU = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "W_ZU", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_ZU", repository) != "kein")
                if (recent_req.W_ZU != null)
                {
                    W_ZU = recent_req.W_ZU.ToString();
                }
                NameValues.Add(Add_NameValue("W_ZU", W_ZU));
                //W_PROZESSWORT
                string W_PROZESSWORT = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "W_PROZESSWORT", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_PROZESSWORT", repository) != "kein")
                if (recent_req.W_PROZESSWORT != null && recent_req.W_PROZESSWORT != "kein" && recent_req.W_PROZESSWORT != "not set")
                {
                    W_PROZESSWORT = recent_req.W_PROZESSWORT;
                }
                NameValues.Add(Add_NameValue("W_PROZESSWORT", W_PROZESSWORT));
                //W_QUALITAET
                string W_QUALITAET = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "W_QUALITAET", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_QUALITAET", repository) != "kein")
                if (recent_req.W_QUALITAET != null && recent_req.W_QUALITAET != "kein" && recent_req.W_QUALITAET != "not set")
                {
                    W_QUALITAET = recent_req.W_QUALITAET;
                }
                NameValues.Add(Add_NameValue("W_QUALITAET", W_QUALITAET));
                //W_AFO_MANUAL
                string W_AFO_MANUAL = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "W_AFO_MANUAL", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_AFO_MANUAL", repository) != "kein")
                if (recent_req.W_AFO_MANUAL != null )
                {
                    W_AFO_MANUAL = recent_req.W_AFO_MANUAL.ToString();
                }
                NameValues.Add(Add_NameValue("W_AFO_MANUAL", W_AFO_MANUAL));
                //W_FREEZE_TITLE
                string W_FREEZE_TITLE = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "W_FREEZE_TITLE", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "W_FREEZE_TITLE", repository) != "kein")
                if (recent_req.W_FREEZE_TITLE != null)
                {
                    W_FREEZE_TITLE = recent_req.W_FREEZE_TITLE.ToString();
                }
                NameValues.Add(Add_NameValue("W_FREEZE_TITLE", W_FREEZE_TITLE));
                //AFO_ABNAHMEKRITERIUM
                string AFO_ABNAHMEKRITERIUM = "";
                //    if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_ABNAHMEKRITERIUM", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_ABNAHMEKRITERIUM", repository) != "kein")
                if (recent_req.AFO_ABNAHMEKRITERIUM != null && recent_req.AFO_ABNAHMEKRITERIUM != "kein" && recent_req.AFO_ABNAHMEKRITERIUM != "not set")
                {
                    AFO_ABNAHMEKRITERIUM = recent_req.AFO_ABNAHMEKRITERIUM;
                }
                NameValues.Add(Add_NameValue("AFO_ABNAHMEKRITERIUM", AFO_ABNAHMEKRITERIUM));
                //B_BEMERKUNG
                string B_BEMERKUNG = "";
                //     if (Tagged.Get_Tagged_Value(AFo_GUID, "B_BEMERKUNG", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "B_BEMERKUNG", repository) != "kein")
                if (recent_req.B_BEMERKUNG != null && recent_req.B_BEMERKUNG != "kein" && recent_req.B_BEMERKUNG != "not set")
                {
                    B_BEMERKUNG = recent_req.B_BEMERKUNG;
                }
                NameValues.Add(Add_NameValue("B_BEMERKUNG", B_BEMERKUNG));
                //AFO_REGELUNGEN
                string AFO_REGELUNGEN = "";
                //   if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_REGELUNGEN", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_REGELUNGEN", repository) != "kein")
                if (recent_req.AFO_REGELUNGEN != null && recent_req.AFO_REGELUNGEN != "kein" && recent_req.AFO_REGELUNGEN != "not set")
                {
                    AFO_REGELUNGEN = recent_req.AFO_REGELUNGEN;
                }
                NameValues.Add(Add_NameValue("AFO_REGELUNGEN", AFO_REGELUNGEN));
                //AFO_QUELLTEXT
                string AFO_QUELLTEXT = "";
                // if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_QUELLTEXT", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_QUELLTEXT", repository) != "kein")
                if (recent_req.AFO_QUELLTEXT != null && recent_req.AFO_QUELLTEXT != "kein" && recent_req.AFO_QUELLTEXT != "not set")
                {
                    AFO_QUELLTEXT = recent_req.AFO_QUELLTEXT;
                }
                NameValues.Add(Add_NameValue("AFO_QUELLTEXT", AFO_QUELLTEXT));
                //AFO_ANSPRECHPARTNER
                string AFO_ANSPRECHPARTNER = "";
                //if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_ANSPRECHPARTNER", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_ANSPRECHPARTNER", repository) != "kein")
                if (recent_req.AFO_ANSPRECHPARTNER != null && recent_req.AFO_ANSPRECHPARTNER != "kein" && recent_req.AFO_ANSPRECHPARTNER != "not set")
                {
                    AFO_ANSPRECHPARTNER = recent_req.AFO_ANSPRECHPARTNER;
                }
                NameValues.Add(Add_NameValue("AFO_ANSPRECHPARTNER", AFO_ANSPRECHPARTNER));
                //AFO_HINWEIS
                string AFO_HINWEIS = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_req.AFO_HINWEIS != null && recent_req.AFO_HINWEIS != "kein" && recent_req.AFO_HINWEIS != "not set")
                {
                    AFO_HINWEIS = recent_req.AFO_HINWEIS;
                }
                NameValues.Add(Add_NameValue("AFO_HINWEIS", AFO_HINWEIS));
                //ID
                string ID = "";
                NameValues.Add(Add_NameValue("ID", ID));
                //AFO_RANG
                string AFO_RANG = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_req.AFO_RANG != null && recent_req.AFO_RANG != "kein" && recent_req.AFO_RANG != null && recent_req.AFO_RANG != "not set")
                {
                    AFO_RANG = recent_req.AFO_RANG;
                }
                NameValues.Add(Add_NameValue("AFO_RANG", AFO_RANG));
                //AFO_ABS_GEWICHT
                string AFO_ABS_GEWICHT = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_req.AFO_ABS_GEWICHT != null && recent_req.AFO_ABS_GEWICHT != "kein" && recent_req.AFO_ABS_GEWICHT != null && recent_req.AFO_ABS_GEWICHT != "not set")
                {
                    AFO_ABS_GEWICHT = recent_req.AFO_ABS_GEWICHT;
                }
                NameValues.Add(Add_NameValue("AFO_ABS_GEWICHT", AFO_ABS_GEWICHT));

            }
            #endregion Anforderung
            //Betrachtung eines Stakeholders
            if(stakeholder == true)
            {
                //PUID
                string PUID = "";
                NameValues.Add(Add_NameValue("PUID", PUID));
                //UUID
                string UUID = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.UUID != null && recent_ST.UUID != "kein")
                {
                    UUID = recent_ST.UUID;
                }
                NameValues.Add(Add_NameValue("UUID", UUID));
                //OBJECT_ID
                string OBJECT_ID = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.OBJECT_ID != null && recent_ST.OBJECT_ID != "kein")
                {
                    OBJECT_ID = recent_ST.OBJECT_ID;
                }
                NameValues.Add(Add_NameValue("OBJECT_ID", OBJECT_ID));
                //ST_AG_ID
                string ST_AG_ID = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_AG_ID != null && recent_ST.st_AG_ID != "kein")
                {
                    ST_AG_ID = recent_ST.st_AG_ID;
                }
                NameValues.Add(Add_NameValue("ST_AG_ID", ST_AG_ID));
                //ST_AN_ID
                string ST_AN_ID = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_AN_ID != null && recent_ST.st_AN_ID != "kein")
                {
                    ST_AN_ID = recent_ST.st_AG_ID;
                }
                NameValues.Add(Add_NameValue("ST_AN_ID", ST_AN_ID));
                //ST_STAKEHOLDER
                string ST_STAKEHOLDER = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_STAKEHOLDER != null && recent_ST.st_STAKEHOLDER != "kein")
                {
                    ST_STAKEHOLDER = recent_ST.st_STAKEHOLDER;
                }
                NameValues.Add(Add_NameValue("ST_STAKEHOLDER", ST_STAKEHOLDER));
                //W_NUTZENDER
                string W_NUTZENDER = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.w_NUTZENDER != null && recent_ST.w_NUTZENDER != "kein")
                {
                    W_NUTZENDER = recent_ST.w_NUTZENDER;
                }
                NameValues.Add(Add_NameValue("W_NUTZENDER", W_NUTZENDER));
                //ST_BESCHREIBUNG
                string ST_BESCHREIBUNG = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_BESCHREIBUNG != null && recent_ST.st_BESCHREIBUNG != "kein")
                {
                    ST_BESCHREIBUNG = recent_ST.st_BESCHREIBUNG;
                }
                NameValues.Add(Add_NameValue("ST_BESCHREIBUNG", ST_BESCHREIBUNG));
                //ST_ARTIKEL
                string ST_ARTIKEL = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_ARTIKEL != null && recent_ST.st_ARTIKEL != "kein")
                {
                    ST_ARTIKEL = recent_ST.st_ARTIKEL;
                }
                NameValues.Add(Add_NameValue("ST_ARTIKEL", ST_ARTIKEL));
                //ST_WV_PHASE
                string ST_WV_PHASE = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_WV_PHASE != null && recent_ST.st_WV_PHASE != "kein")
                {
                    ST_WV_PHASE = recent_ST.st_WV_PHASE;
                }
                NameValues.Add(Add_NameValue("ST_WV_PHASE", ST_WV_PHASE));
                //ST_GRUPPE
                string ST_GRUPPE = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_GRUPPE != null && recent_ST.st_GRUPPE != "kein")
                {
                    ST_GRUPPE = recent_ST.st_GRUPPE;
                }
                NameValues.Add(Add_NameValue("ST_GRUPPE", ST_GRUPPE));
                //IN_CATEGORY
                string IN_CATEGORY = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.in_CATEGORY != null && recent_ST.in_CATEGORY != "kein")
                {
                    IN_CATEGORY = recent_ST.in_CATEGORY;
                }
                NameValues.Add(Add_NameValue("IN_CATEGORY", IN_CATEGORY));
                //ST_AKTEUR_TYP
                string ST_AKTEUR_TYP = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_AKTEUR_TYP != null && recent_ST.st_AKTEUR_TYP != "kein")
                {
                    ST_AKTEUR_TYP = recent_ST.st_AKTEUR_TYP;
                }
                NameValues.Add(Add_NameValue("ST_AKTEUR_TYP", ST_AKTEUR_TYP));
                //ST_ANSPRECHPARTNER
                string ST_ANSPRECHPARTNER = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_ANSPRECHPARTNER != null && recent_ST.st_ANSPRECHPARTNER != "kein")
                {
                    ST_ANSPRECHPARTNER = recent_ST.st_ANSPRECHPARTNER;
                }
                NameValues.Add(Add_NameValue("ST_ANSPRECHPARTNER", ST_ANSPRECHPARTNER));
                //ST_WISSENSGEBIET
                string ST_WISSENSGEBIET = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_WISSENSGEBIET != null && recent_ST.st_WISSENSGEBIET != "kein")
                {
                    ST_WISSENSGEBIET = recent_ST.st_WISSENSGEBIET;
                }
                NameValues.Add(Add_NameValue("ST_WISSENSGEBIET", ST_WISSENSGEBIET));
                //ST_INTERESSE
                string ST_INTERESSE = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_INTERESSE != null && recent_ST.st_INTERESSE != "kein")
                {
                    ST_INTERESSE = recent_ST.st_INTERESSE;
                }
                NameValues.Add(Add_NameValue("ST_INTERESSE", ST_INTERESSE));
                //ST_INTERESSE
                string ST_EINFLUSS = "";
                //  if (Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != null && Tagged.Get_Tagged_Value(AFo_GUID, "AFO_HINWEIS", repository) != "kein")
                if (recent_ST.st_EINFLUSS != null && recent_ST.st_EINFLUSS != "kein")
                {
                    ST_EINFLUSS = recent_ST.st_EINFLUSS;
                }
                NameValues.Add(Add_NameValue("ST_EINFLUSS", ST_EINFLUSS)); 
                    
            }


          //GlossarElement
            #region GlossarElement
            if (glossar_xac == true)
            {
                NameValues.Add(Add_NameValue("PUID", ""));

                //OBJECT_ID
                string Object_ID = "";
                if (glossar.object_ID != null && glossar.object_ID != "kein")
                {
                    Object_ID = glossar.object_ID;
                }
                NameValues.Add(Add_NameValue("OBJECT_ID", Object_ID));
                //GLOE_AG_ID
                string Gloe_AG_ID = "";
                if (glossar.gloe_AG_ID != null && glossar.gloe_AG_ID != "kein")
                {
                    Gloe_AG_ID = glossar.gloe_AG_ID;
                }
                NameValues.Add(Add_NameValue("GLOE_AG_ID", Gloe_AG_ID));
                //GLOE_AN_ID
                string Gloe_AN_ID = "";
                if (glossar.gloe_AN_ID != null && glossar.gloe_AN_ID != "kein")
                {
                    Gloe_AN_ID = glossar.gloe_AN_ID;
                }
                NameValues.Add(Add_NameValue("GLOE_AN_ID", Gloe_AN_ID));
                //GLOE_BEGRIFF
                string Gloe_Begriff = "";
                if (glossar.gloe_BEGRIFF != null && glossar.gloe_BEGRIFF != "kein")
                {
                    Gloe_Begriff = glossar.gloe_BEGRIFF;
                }
                NameValues.Add(Add_NameValue("GLOE_BEGRIFF", Gloe_Begriff));
                //GLOE_GLOSSARTEXT
                string Gloe_Glossartext = "";
                if (glossar.gloe_GLOSSARTEXT != null && glossar.gloe_GLOSSARTEXT != "kein")
                {
                    Gloe_Glossartext = glossar.gloe_GLOSSARTEXT;
                }
                NameValues.Add(Add_NameValue("GLOE_GLOSSARTEXT", Gloe_Glossartext));
                //GLOE_SPRACHE
                string Gloe_Sprache = "";
                if (glossar.gloe_SPRACHE != null && glossar.gloe_SPRACHE != "kein")
                {
                    Gloe_Sprache = glossar.gloe_SPRACHE;
                }
                NameValues.Add(Add_NameValue("GLOE_SPRACHE", Gloe_Sprache));
                NameValues.Add(Add_NameValue("GLOE_GLOSSARTEXT", Gloe_Glossartext));
                //GLOE_PUBLICSTATUS
                string Gloe_Publicstatus = "";
                if (glossar.gloe_PUBLICSTATUS != null && glossar.gloe_PUBLICSTATUS != "kein")
                {
                    Gloe_Publicstatus = glossar.gloe_PUBLICSTATUS;
                }
                NameValues.Add(Add_NameValue("GLOE_PUBLICSTATUS", Gloe_Publicstatus));
                //GLOE_STATUS
                string Gloe_Status = "";
                if (glossar.gloe_STATUS != null && glossar.gloe_STATUS != "kein")
                {
                    Gloe_Status = glossar.gloe_STATUS;
                }
                NameValues.Add(Add_NameValue("GLOE_STATUS", Gloe_Status));
                //GLOE_TYP
                string Gloe_Typ = "";
                if (glossar.gloe_TYP != null && glossar.gloe_TYP != "kein")
                {
                    Gloe_Typ = glossar.gloe_TYP;
                }
                NameValues.Add(Add_NameValue("GLOE_TYP", Gloe_Typ));
                //IN_CATEGORY
                string in_category = "";
                if (glossar.in_CATEGORY != null && glossar.in_CATEGORY != "kein")
                {
                    in_category = glossar.in_CATEGORY;
                }
                NameValues.Add(Add_NameValue("IN_CATEGORY", in_category));
                //GLOE_KUERZEL
                string gloe_kuerzel = "";
                if (glossar.gloe_KUERZEL != null && glossar.gloe_KUERZEL != "kein")
                {
                    gloe_kuerzel = glossar.gloe_KUERZEL;
                }
                NameValues.Add(Add_NameValue("GLOE_KUERZEL", gloe_kuerzel));
                //GLOE_KONTEXT
                string gloe_kontext = "";
                if (glossar.gloe_KONTEXT != null && glossar.gloe_KONTEXT != "kein")
                {
                    gloe_kontext = glossar.gloe_KONTEXT;
                }
                NameValues.Add(Add_NameValue("GLOE_KONTEXT", gloe_kontext));
                //GLOE_HYPERLINK
                string gloe_hyperlink = "";
                if (glossar.gloe_HYPERLINK != null && glossar.gloe_HYPERLINK != "kein")
                {
                    gloe_hyperlink = glossar.gloe_HYPERLINK;
                }
                NameValues.Add(Add_NameValue("GLOE_HYPERLINK", gloe_hyperlink));
                //GLOE_INT_FACHBEGRIFF
                string gloe_fachbegriff = "";
                if (glossar.gloe_INT_FACHBEGRIFF != null && glossar.gloe_INT_FACHBEGRIFF != "kein")
                {
                    gloe_fachbegriff = glossar.gloe_INT_FACHBEGRIFF;
                }
                NameValues.Add(Add_NameValue("GLOE_INT_FACHBEGRIFF", gloe_fachbegriff));
                //GLOE_AUTOR
                string gloe_autor = "";
                if (glossar.gloe_AUTOR != null && glossar.gloe_AUTOR != "kein")
                {
                    gloe_autor = glossar.gloe_AUTOR;
                }
                NameValues.Add(Add_NameValue("GLOE_AUTOR", gloe_autor));
                //GLOE_ZUSATZINFORMATIONEN
                string gloe_zusatzinformationen = "";
                if (glossar.gloe_ZUSATZINFORMATIONEN != null && glossar.gloe_ZUSATZINFORMATIONEN != "kein")
                {
                    gloe_zusatzinformationen = glossar.gloe_ZUSATZINFORMATIONEN;
                }
                NameValues.Add(Add_NameValue("GLOE_ZUSATZINFORMATIONEN", gloe_zusatzinformationen));
                //GLOE_ERSTELLUNGSDATUM
                string gloe_erstellungsdatun = "";
                if (glossar.gloe_ERSTELLUNGSDATUM != null && glossar.gloe_ERSTELLUNGSDATUM != "kein")
                {
                    gloe_erstellungsdatun = glossar.gloe_ERSTELLUNGSDATUM;
                }
                NameValues.Add(Add_NameValue("GLOE_ERSTELLUNGSDATUM", gloe_erstellungsdatun));
            }
            #endregion GlossarElement
            return (NameValues);
        }
        /// <summary>
        /// Es wird die xml Struktur eines NameValue ingefügt
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        private XElement Add_NameValue(string Name, string Value)
        {
            XElement NameValue = new XElement("NameValue");
            XElement Names = new XElement("Name", Name);
            NameValue.Add(Names);
            XElement Values = new XElement("Values");
            NameValue.Add(Values);
            XElement Value_in = new XElement("Value", Value);
            Values.Add(Value_in);

            return (NameValue);

        }
        #endregion Add xml spezifisch

    }
}
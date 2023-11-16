using Ennumerationen;
using Metamodels;
using Repsoitory_Elements;
using Requirements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Forms
{
    public partial class Form_Edit_Requirement : Form
    {
        Requirement_Plugin.Database Data_Edit;
        Requirement Requirement_Edit;
        Requirement Requirement_Copy;
        Create_Metamodel_Class Metamodel_Base = new Create_Metamodel_Class();
        TaggedValue tagged;
        EA.Repository repository;

        private bool afo_ini = true;
        private bool combo_system_index = false;
        private bool w_zu = false;

        //Index ComboBox Satzschablone
        private int index_sys = 0;
        private int index_aktiviaet = 0;
        private int index_prozesswort = 0;
        private int index_stakeholder = 0;

        bool create_con = false;
        DialogResult dialog_result = DialogResult.Cancel;

        //index Combobox Metadaten
      //  private int index_Art_AFO = 0;

        //Index Artiekl
        private int index_Artikel_sys = 0;
        private int index_Artikel_act = 0;

        public Form_Edit_Requirement(Requirement_Plugin.Database data, Requirement requirement, EA.Repository repository, bool create_Connector)
        {
            this.create_con = create_Connector;
            this.Data_Edit = data;
            this.Requirement_Edit = requirement;
            this.Requirement_Copy = new Requirement(Requirement_Edit.Classifier_ID, data.metamodel);
            Requirement_Copy.Get_Tagged_Values_From_Requirement(Requirement_Edit.Classifier_ID, repository, data);
            this.tagged = new TaggedValue(this.Data_Edit.metamodel, data);
            this.repository = repository;

            this.Data_Edit.m_NodeType.Sort();
            this.Data_Edit.m_SysElemente.Sort();
            this.Data_Edit.m_Stakeholder.Sort();
            this.Data_Edit.m_Activity.Sort();
            dialog_result = DialogResult.Cancel;
            InitializeComponent();

            #region Visisble Panel
            //Panel
            //reserve
           // this.panel_reserve1.Visible = false;
           // this.panel_reserve1.Enabled = false;
            this.panel_Bewertung.Visible = false;
            this.panel_Bewertung.Enabled = false;
            this.panel_MetadatenII.Visible = false;
            this.panel_MetadatenII.Enabled = false;
            //Satzschalone default
            this.panel_Satzschablone.Visible = true;
            this.panel_Satzschablone.Enabled = true;
            //Klaerungspunkte
            this.panel_Klaerungspunkte.Visible = false;
            this.panel_Klaerungspunkte.Enabled = false;
            //Metadaten
            this.panel_Metadaten.Visible = false;
            this.panel_Metadaten.Enabled = false;
            //Prüfung
            this.panel_Prüfung.Visible = false;
            this.panel_Prüfung.Enabled = false;
            #endregion Visiblie Panel

            #region Satzschablone Werte zuordnen


            #region Combobox Wert festlegen & Databinding

            #region Systemelelemente
            Set_Systemelemente();
            this.combo_System.Update();
            #endregion

            #region combobox Aktivitaet
            if ((int)this.Requirement_Edit.W_AKTIVITAET != -1)
            {
                string recent_aktivitaet = Data_Edit.AFO_ENUM.W_AKTIVITAET[(int)this.Requirement_Edit.W_AKTIVITAET];

                if (recent_aktivitaet != null)
                {
                    index_aktiviaet = (int)this.Requirement_Edit.W_AKTIVITAET;
                    if (index_aktiviaet == 3)
                    {
                        index_aktiviaet = 0;
                    }
                }
                else
                {
                    index_aktiviaet = 0;
                }
            }
            else
            {
                index_aktiviaet = 0;
            }

          

            BindingSource bindingSource_Aktivitaet = new BindingSource();
            bindingSource_Aktivitaet.DataSource = this.Metamodel_Base.m_W_Aktiviaet;
            this.comboBox_Aktivitaet.DataSource = (bindingSource_Aktivitaet);
            this.comboBox_Aktivitaet.SelectedIndex = index_aktiviaet;
            this.comboBox_Aktivitaet.Refresh();
            #endregion  combobox Aktivitaet

            #region combobox Prozesswort
            string recent_prozesswort = this.Requirement_Edit.W_PROZESSWORT;

            if (recent_prozesswort != null)
            {

                index_prozesswort = this.Data_Edit.m_Activity.Select(x => x.W_Prozesswort).ToList().Distinct().ToList().FindIndex(x => x == this.Requirement_Edit.W_PROZESSWORT);

                if (index_prozesswort == -1) //nicht vorhanden
                {
                    index_prozesswort = this.Data_Edit.m_Activity.Select(x => x.W_Prozesswort).ToList().Distinct().ToList().Count;
                    Activity act = new Activity(null, null, null);

                    act.W_Prozesswort = recent_prozesswort;
                    act.W_Object = this.Requirement_Edit.W_OBJEKT;

                    this.Data_Edit.m_Activity.Add(act);
                }
            }
            else
            {
                index_prozesswort = 0;
            }

            if(this.Data_Edit.m_Activity.Count > 0)
            {
                BindingSource bindingSource_Prozesswort = new BindingSource();


                this.Data_Edit.m_Activity.Distinct().ToList();

                List<Activity> m_help = new List<Activity>();

                int a1 = 0;
                do
                {
                    List<Activity> m_string = m_help.Where(x => x.W_Prozesswort == this.Data_Edit.m_Activity[a1].W_Prozesswort).ToList();

                    if (m_string.Count == 0)
                    {
                        m_help.Add(this.Data_Edit.m_Activity[a1]);
                    }

                    a1++;
                } while (a1 < this.Data_Edit.m_Activity.Count);


                //      bindingSource_Prozesswort.DataSource = this.Data_Edit.m_Activity.Select(x => x.W_Prozesswort).ToList().Distinct();
                bindingSource_Prozesswort.DataSource = m_help;
                this.comboBox_Prozesswort.DataSource = (bindingSource_Prozesswort);
                this.comboBox_Prozesswort.DisplayMember = "W_PROZESSWORT";
                this.comboBox_Prozesswort.SelectedIndex = index_prozesswort;
                this.comboBox_Prozesswort.Refresh();
                //this.Co
            }


            #endregion combobox Prozesswort

            #region combobox Stakeholder & Artikel
            string recent_st = this.Requirement_Edit.W_NUTZENDER;

            if (recent_st != null)
            {
                if(recent_st != "")
                {

                    index_stakeholder = this.Data_Edit.m_Stakeholder.FindIndex(x => this.Metamodel_Base.m_Artikel_Akt.FindIndex(y => y + " " + x.w_NUTZENDER == recent_st) != -1);



                        //index_stakeholder = this.Data_Edit.m_Stakeholder.FindIndex(x => this.Metamodel_Base.m_Artikel[this.Metamodel_Base.m_Artikel_Akt.FindIndex(y => y == x.st_ARTIKEL)] + " " + x.w_NUTZENDER == recent_st);

                        if (index_stakeholder == -1)
                        {
                            index_stakeholder = this.Data_Edit.m_Stakeholder.Count;
                            Stakeholder st = new Stakeholder(null, null, null);
                            var str_split = recent_st.Split(' ');

                            if (str_split.Length > 1 && this.Metamodel_Base.m_Artikel_Akt.Contains(str_split[0]) == true)
                            {
                                st.st_ARTIKEL = str_split[0];
                                st.w_NUTZENDER = str_split[1];

                                if (str_split.Length > 2)
                                {
                                    int i1 = 2;
                                    do
                                    {
                                        st.w_NUTZENDER = st.w_NUTZENDER + " " + str_split[i1];

                                        i1++;
                                    } while (i1 < str_split.Length - 1);
                                }

                            }
                            else
                            {
                                st.st_ARTIKEL = this.Metamodel_Base.m_Artikel[0];
                                st.w_NUTZENDER = recent_st;
                            }

                            if (this.Data_Edit.m_Stakeholder.Select(x => x.w_NUTZENDER).ToList().Contains(st.w_NUTZENDER) == false)
                            {
                                this.Data_Edit.m_Stakeholder.Add(st);
                            }
                        

                 
                    
                       
                    }
                }
             


            }
            else
            {
                index_stakeholder = 0;
            }

            if(index_stakeholder >= this.Data_Edit.m_Stakeholder.Count)
            {
                index_stakeholder = 0;
            }

            if(this.Data_Edit.m_Stakeholder.Count > 0)
            {
                this.index_Artikel_act = this.Metamodel_Base.m_Artikel.FindIndex(x => x == this.Metamodel_Base.m_Artikel[(int)this.Data_Edit.m_Stakeholder[index_stakeholder].SYS_ARTIKEL]);

                if(this.index_Artikel_act != -1)
                {
                    BindingSource bindingSource_Stakeholder = new BindingSource();
                    bindingSource_Stakeholder.DataSource = this.Data_Edit.m_Stakeholder;
                    this.combo_Akteur.DataSource = (bindingSource_Stakeholder);
                    this.combo_Akteur.DisplayMember = "w_Nutzender";
                    this.combo_Akteur.SelectedIndex = index_stakeholder;
                    this.combo_Akteur.Refresh();
                    //Aktueller Artikel
                    this.button_Art_Aktuer.Text = this.Metamodel_Base.m_Artikel_Akt[this.index_Artikel_act];
                    this.button_Art_Aktuer.Refresh();
                }
                else
                {
                    this.index_Artikel_act = 0;
                    this.button_Art_Aktuer.Text = this.Metamodel_Base.m_Artikel_Akt[this.index_Artikel_act];
                    this.button_Art_Aktuer.Refresh();
                }


            }

            if ((int)this.Requirement_Edit.W_AKTIVITAET == 1)
            {
                this.combo_Akteur.Enabled = true;
                this.button_Art_Aktuer.Enabled = true;
            }
            else
            {
                this.combo_Akteur.Enabled = false;
                this.button_Art_Aktuer.Enabled = false;
            }

          /*  if (this.comboBox_Aktivitaet.Text != "dem <Akteur> die Möglichkeit bieten")
            {
                this.combo_Akteur.Enabled = false;
                this.button_Art_Aktuer.Enabled = false;
            }
            else
            {
                this.combo_Akteur.Enabled = true;
                this.button_Art_Aktuer.Enabled = true;
            }*/

            #endregion combobox Stakeholder & Artikel

            #endregion Combobox Wert festlegen & Databinding

            #endregion Satzschablone Werte zuordnen

            #region Klärungspunkte Werte zuordnen
            this.Text_Klaerungspunkte.Text = this.Requirement_Edit.AFO_KLAERUNGSPUNKTE;
            #endregion Klärungspunkte Werte zuordnen

            #region Metadaten Werte zuordnen
            ///////////////////////////////
            #region Textfeld
            //Textfelder
            this.richText_Autor.Text = this.Requirement_Edit.AFO_ANSPRECHPARTNER;
            this.richText_Begründung.Text = this.Requirement_Edit.B_BEMERKUNG;
            this.richText_Dokument.Text = this.Requirement_Edit.AFO_REGELUNGEN;
            this.richText_Hinweis.Text = this.Requirement_Edit.AFO_HINWEIS;
            this.richText_Zitat.Text = this.Requirement_Edit.AFO_QUELLTEXT;
            //this.richText_Abnahmekriterium.Text = this.Requirement_Edit.AFO_ABNAHMEKRITERIUM;
            #endregion Textfeld
            /////////////////////////////////
            #region Auswahl
            //DropDown
            //AFO_ART
            BindingSource bindingSource_Art_AFO = new BindingSource();
            bindingSource_Art_AFO.DataSource = this.Data_Edit.AFO_ENUM.AFO_WV_ART;
            this.combo_Art_AFO.DataSource = (bindingSource_Art_AFO);
            this.combo_Art_AFO.SelectedIndex = (int) this.Requirement_Edit.AFO_WV_ART;
            //funktional
            /*  BindingSource bindingSource_func = new BindingSource();
              bindingSource_func.DataSource = this.Data_Edit.AFO_ENUM.AFO_FUNKTIONAL;
              this.combo_funktional.DataSource = (bindingSource_func);
              this.combo_funktional.SelectedIndex = (int)this.Requirement_Edit.AFO_FUNKTIONAL;*/

            if(this.Data_Edit.AFO_ENUM.AFO_FUNKTIONAL[(int)this.Requirement_Edit.AFO_FUNKTIONAL] == this.Data_Edit.AFO_ENUM.AFO_FUNKTIONAL[0])
            {
                this.checkBox_funktional.Checked = true;
                this.checkBox_funktional.Update();
            }
            else
            {
                this.checkBox_funktional.Checked = false;
                this.checkBox_funktional.Update();
            }

            //Status
            BindingSource bindingSource_status = new BindingSource();
            bindingSource_status.DataSource = this.Data_Edit.AFO_ENUM.AFO_STATUS;
            this.combo_status.DataSource = (bindingSource_status);
            this.combo_status.SelectedIndex = (int)this.Requirement_Edit.AFO_STATUS;
            //QS-Status
            BindingSource bindingSource_qsstatus = new BindingSource();
            bindingSource_qsstatus.DataSource = this.Data_Edit.AFO_ENUM.AFO_QS_STATUS;
            this.combo_qsstatus.DataSource = (bindingSource_qsstatus);
            this.combo_qsstatus.SelectedIndex = (int)this.Requirement_Edit.AFO_QS_STATUS;
            //Phase
          //  BindingSource bindingSource_phase = new BindingSource();
          //  bindingSource_phase.DataSource = this.Data_Edit.AFO_ENUM.AFO_CPM_PHASE;
          //  this.combo_Phase.DataSource = (bindingSource_phase);
          //  this.combo_Phase.SelectedIndex = (int)this.Requirement_Edit.AFO_CPM_PHASE;

            BindingSource bindingSource_phase2 = new BindingSource();
            bindingSource_phase2.DataSource = this.Data_Edit.AFO_ENUM.AFO_CPM_PHASE;
            this.comboBox_Phasen.DataSource = (bindingSource_phase2);
            this.comboBox_Phasen.SelectedIndex = (int)this.Requirement_Edit.AFO_CPM_PHASE;
            //Bezug
            BindingSource bindingSource_bezug = new BindingSource();
            bindingSource_bezug.DataSource = this.Data_Edit.AFO_ENUM.AFO_BEZUG;
            this.combo_Bezug.DataSource = (bindingSource_bezug);
            this.combo_Bezug.SelectedIndex = (int)this.Requirement_Edit.AFO_BEZUG;
            //Nachweisart
           /* BindingSource bindingSource_nachweisart = new BindingSource();
            bindingSource_nachweisart.DataSource = this.Data_Edit.AFO_ENUM.AFO_WV_NACHWEISART;
            this.combo_Nachweisart.DataSource = (bindingSource_nachweisart);
            this.combo_Nachweisart.SelectedIndex = (int)this.Requirement_Edit.AFO_WV_NACHWEISART;*/
            //Projektrolle
            BindingSource bindingSource_projektrolle = new BindingSource();
            bindingSource_projektrolle.DataSource = this.Data_Edit.AFO_ENUM.AFO_PROJEKTROLLE;
            this.combo_Projektrolle.DataSource = (bindingSource_projektrolle);
            this.combo_Projektrolle.SelectedIndex = (int)this.Requirement_Edit.AFO_PROJEKTROLLE;
            //W_ZU
            if(this.Requirement_Edit.W_ZU == true)
            {
                this.checkBox_W_ZU.Checked = true;
            }
            else
            {
                this.checkBox_W_ZU.Checked = false;
            }
            this.checkBox_W_ZU.Update();

            #endregion Auswahl
            ///////////////////////////////////
            #region Bewertung
            //Detailstufe
            BindingSource bindingSource_detail = new BindingSource();
            bindingSource_detail.DataSource = this.Data_Edit.AFO_ENUM.AFO_DETAILSTUFE;
            this.combo_Detailstufe.DataSource = (bindingSource_detail);
            this.combo_Detailstufe.SelectedIndex = (int)this.Requirement_Edit.AFO_DETAILSTUFE;
            //Kategorie
            BindingSource bindingSource_kategorie = new BindingSource();
            bindingSource_kategorie.DataSource = this.Data_Edit.AFO_ENUM.IN_CATEGORY;
            this.combo_Kategorie.DataSource = (bindingSource_kategorie);
            this.combo_Kategorie.SelectedIndex = (int)this.Requirement_Edit.IN_CATEGORY;
            //Kritikalitaet
            BindingSource bindingSource_krit = new BindingSource();
            bindingSource_krit.DataSource = this.Data_Edit.AFO_ENUM.AFO_KRITIKALITAET;
            this.combo_Kritikalitaet.DataSource = (bindingSource_krit);
            this.combo_Kritikalitaet.SelectedIndex = (int)this.Requirement_Edit.AFO_KRITIKALITAET;
            //Prioritaet Vergabe
            BindingSource bindingSource_prio = new BindingSource();
            bindingSource_prio.DataSource = this.Data_Edit.AFO_ENUM.AFO_PRIORITAET_VERGABE;
            this.combo_PriotitaetVergabe.DataSource = (bindingSource_prio);
            this.combo_PriotitaetVergabe.SelectedIndex = (int)this.Requirement_Edit.AFO_PRIORITAET_VERGABE;
            //Operative Bewertung Vergabe
            BindingSource bindingSource_op_bewertung = new BindingSource();
            bindingSource_op_bewertung.DataSource = this.Data_Edit.AFO_ENUM.AFO_OPERATIVEBEWERTUNG;
            this.comboBox_OperativeBewertung.DataSource = (bindingSource_op_bewertung);
            this.comboBox_OperativeBewertung.SelectedIndex = (int)this.Requirement_Edit.AFO_OPERATIVEBEWERTUNG;
            //RPI_Stereotype
            BindingSource bindingSource_rpi_stereo = new BindingSource();
            this.Data_Edit.AFO_ENUM.AFO_Stereotype = this.Data_Edit.metamodel.m_Requirement.Select(x => x.Stereotype).ToList().ToArray();
            bindingSource_rpi_stereo.DataSource = this.Data_Edit.AFO_ENUM.AFO_Stereotype;
            this.comboBox_RPIStereotype.DataSource = (bindingSource_rpi_stereo);
            this.comboBox_RPIStereotype.SelectedIndex = this.Data_Edit.AFO_ENUM.Get_Index(this.Data_Edit.AFO_ENUM.AFO_Stereotype, this.Requirement_Edit.RPI_Stereotype);

            //AG_ID
            this.richText_AG_ID.Text = Requirement_Edit.AFO_AG_ID;
            //AN_ID
            this.richText_AN_ID.Text = Requirement_Edit.AFO_AN_ID;
            #endregion Bewertung
            #endregion Metadaten Werte zuordnen

            #region Prüfung Werte zuordnene
            this.richTextBoxpanelPrüfung.Text = this.Requirement_Edit.AFO_ABNAHMEKRITERIUM;

            //Nachweisart
            BindingSource bindingSource_nachweisart = new BindingSource();
            bindingSource_nachweisart.DataSource = this.Data_Edit.AFO_ENUM.AFO_WV_NACHWEISART;
            this.comboBoxpanelPrufungNachweisart.DataSource = (bindingSource_nachweisart);
            this.comboBoxpanelPrufungNachweisart.SelectedIndex = (int)this.Requirement_Edit.AFO_WV_NACHWEISART;
            #endregion
            ////////////////////////////////////
            #region Split Container
            //      this.splitContainer1.IsSplitterFixed = true;
            //    this.splitContainer2.IsSplitterFixed = true;     
            this.splitContainer18.IsSplitterFixed = true;

            if(this.Requirement_Edit.W_AFO_MANUAL == true)
            {
                this.checkBox_Satzschablone.Checked = false;
            }
            else
            {
                this.checkBox_Satzschablone.Checked = true;
            }
            this.checkBox_Satzschablone.Update();

            #endregion Split Container

            #region Strings
            this.Text_Randbedingung.Text = this.Requirement_Edit.W_RANDBEDINGUNG;
            this.Text_Qualitaet.Text = this.Requirement_Edit.W_QUALITAET;
            this.Text_Objekt.Text = this.Requirement_Edit.W_OBJEKT;
            this.Text_Header.Text = this.Requirement_Edit.AFO_TITEL;

            if(this.Requirement_Edit.AFO_TEXT != "null")
            {
                this.Text_AFO_Text.Text = this.Requirement_Edit.AFO_TEXT;
            }
            else
            {
                // this.Text_AFO_Text.Text = button_Sys_Artikel.Text+" "+this.combo_System.SelectedText+" muss ..."
                this.Get_AFO_Text();
            }


            #endregion Strings

            #region CheckBoxen

            if (this.Data_Edit.metamodel.flag_sysarch == true)
            {
                this.checkBox_logical.Checked = false;
            }
            else
            {
                this.checkBox_logical.Checked = true;
            }
            this.checkBox_logical.Update();

            this.afo_ini = false;

            if(this.Requirement_Edit.W_SINGULAR == true)
            {
                this.checkBox_Plural.Checked = false;
            }
            else
            {
                this.checkBox_Plural.Checked = true;
            }
            this.checkBox_Plural.Update();

            if(this.Requirement_Edit.RPI_Export == true)
            {
                this.checkBox_Export.Checked = true;
            }
            else
            {
                this.checkBox_Export.Checked = false;
            }
            this.checkBox_Export.Update();
            #endregion
        }

        private void Set_Systemelemente()
        {
            #region combobox System und Artikel
            string recent_sys_name = this.Requirement_Edit.W_SUBJEKT;

            if (recent_sys_name != null)
            {
                if (this.Data_Edit.metamodel.flag_sysarch == false)
                {
                    index_sys = this.Data_Edit.m_NodeType.FindIndex(x => x.W_Artikel + " " + x.SYS_KUERZEL == recent_sys_name);
                }
                else
                {
                    index_sys = this.Data_Edit.m_SysElemente.FindIndex(x => x.W_Artikel + " " + x.SYS_KUERZEL == recent_sys_name);
                }


                if (index_sys == -1) //nicht vorhanden
                {
                    if(this.Data_Edit.metamodel.flag_sysarch == false)
                    {
                        index_sys = this.Data_Edit.m_NodeType.Count-1;
                    }
                    else
                    {
                        index_sys = this.Data_Edit.m_SysElemente.Count-1;
                    }
                  

                    NodeType node = new NodeType(null, null, null);
                    SysElement sys = new SysElement(null, null, null);

                    var str_split = recent_sys_name.Split(' ');

                    if (str_split.Length > 1 && this.Metamodel_Base.m_Artikel.Contains(str_split[0]) == true)
                    {
                        node.W_Artikel = str_split[0];
                        node.Name = str_split[1];
                        sys.W_Artikel = node.W_Artikel;

                        if (str_split.Length > 2)
                        {
                            int i1 = 2;
                            do
                            {
                                node.Name = node.Name + " " + str_split[i1];

                                i1++;
                            } while (i1 < str_split.Length - 1);
                        }

                        node.SYS_KUERZEL = node.Name;

                        sys.W_Artikel = node.W_Artikel;
                        sys.SYS_PRODUKT = node.SYS_KUERZEL;
                        sys.Name = node.Name;

                    }
                    else
                    {
                        node.W_Artikel = this.Metamodel_Base.m_Artikel[0];
                        node.Name = recent_sys_name;
                        sys.W_Artikel = node.W_Artikel;
                        sys.Name = node.Name;
                    }

                    if (this.Data_Edit.metamodel.flag_sysarch == false)
                    {
                       // this.Data_Edit.m_NodeType.Add(node);
                    }
                    else
                    {
                        //SysElement node2 = (SysElement)node;
                       // this.Data_Edit.m_SysElemente.Add(sys);
                    }

                }
            }
            else
            {
                index_sys = 0;


            }

            if (this.Data_Edit.metamodel.flag_sysarch == false)
            {
                if (this.Data_Edit.m_NodeType.Count > 0)
                {

                    BindingSource bindingSource_System = new BindingSource();
                    bindingSource_System.DataSource = this.Data_Edit.m_NodeType.Where(x => x.Name != "");
                    this.combo_System.DataSource = (bindingSource_System);

                    if (index_sys != -1)
                    {
                        this.index_Artikel_sys = this.Metamodel_Base.m_Artikel.FindIndex(x => x == this.Data_Edit.m_NodeType[index_sys].W_Artikel);
                        this.combo_System.SelectedIndex = index_sys;
                        this.button_Sys_Artikel.Text = this.Data_Edit.m_NodeType[index_sys].W_Artikel;
                    }



                    //Zuweisung Daten und Aktueller Index comboBox
                 
                 
                  
                    this.combo_System.Refresh();
                    //Aktueller Artikel
                   
                    this.button_Sys_Artikel.Refresh();
                }


            }
            else
            {
                if (this.Data_Edit.m_SysElemente.Count > 0)
                {

                    //Zuweisung Daten und Aktueller Index comboBox
                    BindingSource bindingSource_System = new BindingSource();
                    bindingSource_System.DataSource = this.Data_Edit.m_SysElemente;
                    this.combo_System.DataSource = (bindingSource_System);

                    if (index_sys != -1)
                    {
                        this.index_Artikel_sys = this.Metamodel_Base.m_Artikel.FindIndex(x => x == this.Data_Edit.m_SysElemente[index_sys].W_Artikel);
                        this.combo_System.SelectedIndex = index_sys;
                        this.button_Sys_Artikel.Text = this.Data_Edit.m_SysElemente[index_sys].W_Artikel;
                    } 
                    this.combo_System.Refresh();
                    //Aktueller Artikel
                  
                    this.button_Sys_Artikel.Refresh();
                }
            }

            this.combo_System.DisplayMember = "SYS_KUERZEL";

            #endregion System und Artikel
        }

        private void satzschabloneToolStripMenuItem_Click(object sender, EventArgs e)
        {

            #region Visisble Panel
            //Panel
            //reserve
           // this.panel_reserve1.Visible = false;
           // this.panel_reserve1.Enabled = false;
            this.panel_Bewertung.Visible = false;
            this.panel_Bewertung.Enabled = false;
            this.panel_MetadatenII.Visible = false;
            this.panel_MetadatenII.Enabled = false;
            //Satzschalone default
            this.panel_Satzschablone.Visible = true;
            this.panel_Satzschablone.Enabled = true;
            //Klaerungspunkte
            this.panel_Klaerungspunkte.Visible = false;
            this.panel_Klaerungspunkte.Enabled = false;
            //Metadaten
            this.panel_Metadaten.Visible = false;
            this.panel_Metadaten.Enabled = false;
            //Prüfung
            this.panel_Prüfung.Visible = false;
            this.panel_Prüfung.Enabled = false;
            #endregion Visiblie Panel

            /*      #region Satzschablone Werte zuordnen
                  #region Strings
                  this.Text_Randbedingung.Text = this.Requirement_Edit.W_RANDBEDINGUNG;
                  this.Text_Qualitaet.Text = this.Requirement_Edit.W_QUALITAET;
                  this.Text_Objekt.Text = this.Requirement_Edit.W_OBJEKT;
                  this.Text_Header.Text = this.Requirement_Edit.AFO_TITEL;
                  this.Text_AFO_Text.Text = this.Requirement_Edit.AFO_TEXT;
                  #endregion Strings

                  #region Combobox Wert festlegen & Databinding
                  #region combobox System und Artikel
                  string recent_sys_name = this.Requirement_Edit.W_SUBJEKT;

                  if (recent_sys_name != null)
                  {
                      index_sys = this.Data_Edit.m_NodeType.FindIndex(x => x.W_Artikel + " " + x.Name == recent_sys_name);

                      if (index_sys == -1) //nicht vorhanden
                      {
                          index_sys = this.Data_Edit.m_NodeType.Count;
                          NodeType node = new NodeType(null, null, null);
                          var str_split = recent_sys_name.Split(' ');

                          if (str_split.Length > 1 && this.Metamodel_Base.m_Artikel.Contains(str_split[0]) == true)
                          {
                              node.W_Artikel = str_split[0];
                              node.Name = str_split[1];

                              if (str_split.Length > 2)
                              {
                                  int i1 = 2;
                                  do
                                  {
                                      node.Name = node.Name + " " + str_split[i1];

                                      i1++;
                                  } while (i1 < str_split.Length - 1);
                              }

                          }
                          else
                          {
                              node.W_Artikel = this.Metamodel_Base.m_Artikel[0];
                              node.Name = recent_sys_name;
                          }

                          this.Data_Edit.m_NodeType.Add(node);
                      }
                  }
                  else
                  {
                      index_sys = 0;
                  }

                  //Zuweisung Daten und Aktueller Index comboBox
                  BindingSource bindingSource_System = new BindingSource();
                  bindingSource_System.DataSource = this.Data_Edit.m_NodeType;
                  this.combo_System.DataSource = (bindingSource_System);
                  this.combo_System.DisplayMember = "Name";
                  this.combo_System.SelectedIndex = index_sys;
                  this.combo_System.Refresh();
                  //Aktueller Artikel
                  this.button_Sys_Artikel.Text = this.Data_Edit.m_NodeType[index_sys].W_Artikel;
                  this.button_Sys_Artikel.Refresh();

                  #endregion System und Artikel

                  #region combobox Aktivitaet
                  string recent_aktivitaet = Data_Edit.AFO_ENUM.W_AKTIVITAET[(int)this.Requirement_Edit.W_AKTIVITAET];

                  if (recent_aktivitaet != null)
                  {
                      index_aktiviaet = (int)this.Requirement_Edit.W_AKTIVITAET;
                      if (index_aktiviaet == 3)
                      {
                          index_aktiviaet = 0;
                      }
                  }
                  else
                  {
                      index_aktiviaet = 0;
                  }
                  BindingSource bindingSource_Aktivitaet = new BindingSource();
                  bindingSource_Aktivitaet.DataSource = this.Metamodel_Base.m_W_Aktiviaet;
                  this.comboBox_Aktivitaet.DataSource = (bindingSource_Aktivitaet);
                  this.comboBox_Aktivitaet.SelectedIndex = index_aktiviaet;
                  this.comboBox_Aktivitaet.Refresh();
                  #endregion  combobox Aktivitaet

                  #region combobox Prozesswort
                  string recent_prozesswort = this.Requirement_Edit.W_PROZESSWORT;

                  if (recent_prozesswort != null)
                  {

                      index_prozesswort = this.Data_Edit.m_Activity.Select(x => x.W_Prozesswort).ToList().Distinct().ToList().FindIndex(x => x == this.Requirement_Edit.W_PROZESSWORT);

                      if (index_prozesswort == -1) //nicht vorhanden
                      {
                          index_prozesswort = this.Data_Edit.m_Activity.Select(x => x.W_Prozesswort).ToList().Distinct().ToList().Count;
                          Activity act = new Activity(null, null, null);

                          act.W_Prozesswort = recent_prozesswort;
                          act.W_Object = this.Requirement_Edit.W_OBJEKT;

                          this.Data_Edit.m_Activity.Add(act);
                      }
                  }
                  else
                  {
                      index_prozesswort = 0;
                  }

                  BindingSource bindingSource_Prozesswort = new BindingSource();
                  bindingSource_Prozesswort.DataSource = this.Data_Edit.m_Activity.Select(x => x.W_Prozesswort).ToList().Distinct();
                  this.comboBox_Prozesswort.DataSource = (bindingSource_Prozesswort);
                  this.comboBox_Prozesswort.SelectedIndex = index_prozesswort;
                  this.comboBox_Prozesswort.Refresh();

                  #endregion combobox Prozesswort

                  #region combobox Stakeholder & Artikel
                  string recent_st = this.Requirement_Edit.W_NUTZENDER;

                  if (recent_st != null)
                  {
                      index_stakeholder = this.Data_Edit.m_Stakeholder.FindIndex(x => this.Metamodel_Base.m_Artikel_Akt[this.Metamodel_Base.m_Artikel.FindIndex(y => y == x.st_ARTIKEL)] + " " + x.w_NUTZENDER == recent_st);

                      if (index_stakeholder == -1)
                      {
                          index_stakeholder = this.Data_Edit.m_Stakeholder.Count;
                          Stakeholder st = new Stakeholder(null, null, null);
                          var str_split = recent_st.Split(' ');

                          if (str_split.Length > 1 && this.Metamodel_Base.m_Artikel.Contains(str_split[0]) == true)
                          {
                              st.st_ARTIKEL = str_split[0];
                              st.w_NUTZENDER = str_split[1];

                              if (str_split.Length > 2)
                              {
                                  int i1 = 2;
                                  do
                                  {
                                      st.w_NUTZENDER = st.w_NUTZENDER + " " + str_split[i1];

                                      i1++;
                                  } while (i1 < str_split.Length - 1);
                              }

                          }
                          else
                          {
                              st.st_ARTIKEL = this.Metamodel_Base.m_Artikel[0];
                              st.w_NUTZENDER = recent_st;
                          }



                          this.Data_Edit.m_Stakeholder.Add(st);
                      }


                  }
                  else
                  {
                      index_stakeholder = 0;
                  }

                  BindingSource bindingSource_Stakeholder = new BindingSource();
                  bindingSource_Stakeholder.DataSource = this.Data_Edit.m_Stakeholder;
                  this.combo_Akteur.DataSource = (bindingSource_Stakeholder);
                  this.combo_Akteur.DisplayMember = "w_Nutzender";
                  this.combo_Akteur.SelectedIndex = index_stakeholder;
                  this.combo_Akteur.Refresh();
                  //Aktueller Artikel
                  this.button_Art_Aktuer.Text = this.Metamodel_Base.m_Artikel_Akt[this.Metamodel_Base.m_Artikel.FindIndex(x => x == this.Data_Edit.m_Stakeholder[index_stakeholder].st_ARTIKEL)];
                  this.button_Art_Aktuer.Refresh();

                  if (this.comboBox_Aktivitaet.Text != "dem <Akteur> die Möglichkeit bieten")
                  {
                      this.combo_Akteur.Enabled = false;
                      this.button_Art_Aktuer.Enabled = false;
                  }
                  else
                  {
                      this.combo_Akteur.Enabled = true;
                      this.button_Art_Aktuer.Enabled = true;
                  }

                  #endregion combobox Stakeholder & Artikel

                  #endregion Combobox Wert festlegen & Databinding

                  #endregion Satzschablone Werte zuordnen

          */
        }

        private void klärungspunkteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Visisble Panel
            //Panel
            //reserve
           // this.panel_reserve1.Visible = false;
           // this.panel_reserve1.Enabled = false;
            this.panel_Bewertung.Visible = false;
            this.panel_Bewertung.Enabled = false;
            this.panel_MetadatenII.Visible = false;
            this.panel_MetadatenII.Enabled = false;
            //Satzschalone default
            this.panel_Satzschablone.Visible = false;
            this.panel_Satzschablone.Enabled = false;
            //Klaerungspunkte
            this.panel_Klaerungspunkte.Visible = true;
            this.panel_Klaerungspunkte.Enabled = true;
            //Metadaten
            this.panel_Metadaten.Visible = false;
            this.panel_Metadaten.Enabled = false;
            //Prüfung
            this.panel_Prüfung.Visible = false;
            this.panel_Prüfung.Enabled = false;
            #endregion Visiblie Panel

            #region Strings
            this.Text_Klaerungspunkte.Text = this.Requirement_Edit.AFO_KLAERUNGSPUNKTE;
            #endregion Strings
        }

        private void textfelderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Visisble Panel
            //Panel
            //reserve
           // this.panel_reserve1.Visible = false;
           // this.panel_reserve1.Enabled = false;
            this.panel_Bewertung.Visible = false;
            this.panel_Bewertung.Enabled = false;
            this.panel_MetadatenII.Visible = false;
            this.panel_MetadatenII.Enabled = false;
            //Satzschalone default
            this.panel_Satzschablone.Visible = false;
            this.panel_Satzschablone.Enabled = false;
            //Klaerungspunkte
            this.panel_Klaerungspunkte.Visible = false;
            this.panel_Klaerungspunkte.Enabled = false;
            //Metadaten
            this.panel_Metadaten.Visible = true;
            this.panel_Metadaten.Enabled = true;
            //Prüfung
            this.panel_Prüfung.Visible = false;
            this.panel_Prüfung.Enabled = false;
            #endregion Visiblie Panel
        }

        private void auswahlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Visisble Panel
            //Panel
            //reserve
            //this.panel_reserve1.Visible = false;
            //this.panel_reserve1.Enabled = false;
            this.panel_Bewertung.Visible = false;
            this.panel_Bewertung.Enabled = false;

            //Satzschalone default
            this.panel_Satzschablone.Visible = false;
            this.panel_Satzschablone.Enabled = false;
            //Klaerungspunkte
            this.panel_Klaerungspunkte.Visible = false;
            this.panel_Klaerungspunkte.Enabled = false;
            //Metadaten
            this.panel_Metadaten.Visible = false;
            this.panel_Metadaten.Enabled = false;
            //Auswahl
            this.panel_MetadatenII.Visible = true;
            this.panel_MetadatenII.Enabled = true;
            //Prüfung
            this.panel_Prüfung.Visible = false;
            this.panel_Prüfung.Enabled = false;
            #endregion Visiblie Panel
        }


        private void bewertungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Visisble Panel
            //Panel
            //reserve
          // this.panel_reserve1.Visible = false;
          //  this.panel_reserve1.Enabled = false;


            //Satzschalone default
            this.panel_Satzschablone.Visible = false;
            this.panel_Satzschablone.Enabled = false;
            //Klaerungspunkte
            this.panel_Klaerungspunkte.Visible = false;
            this.panel_Klaerungspunkte.Enabled = false;
            //Metadaten
            this.panel_Metadaten.Visible = false;
            this.panel_Metadaten.Enabled = false;
            //Auswahl
            this.panel_MetadatenII.Visible = false;
            this.panel_MetadatenII.Enabled = false;
            //Bewertung
            this.panel_Bewertung.Visible = true;
            this.panel_Bewertung.Enabled = true;
            //Prüfung
            this.panel_Prüfung.Visible = false;
            this.panel_Prüfung.Enabled = false;
            #endregion Visiblie Panel
        }

        private void prüfungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Visisble Panel
            //Panel
            //reserve
         //  this.panel_reserve1.Visible = false;
         //   this.panel_reserve1.Enabled = false;


            //Satzschalone default
            this.panel_Satzschablone.Visible = false;
            this.panel_Satzschablone.Enabled = false;
            //Klaerungspunkte
            this.panel_Klaerungspunkte.Visible = false;
            this.panel_Klaerungspunkte.Enabled = false;
            //Metadaten
            this.panel_Metadaten.Visible = false;
            this.panel_Metadaten.Enabled = false;
            //Auswahl
            this.panel_MetadatenII.Visible = false;
            this.panel_MetadatenII.Enabled = false;
            //Bewertung
            this.panel_Bewertung.Visible = false;
            this.panel_Bewertung.Enabled = false;
            //Prüfung
            this.panel_Prüfung.Visible = true;
            this.panel_Prüfung.Enabled = true;
            #endregion Visiblie Panel
        }

        #region TextBox Change

        #region Randbedingung
        private void Text_Randbedingung_TextChanged(object sender, EventArgs e)
        {
            if (this.afo_ini == false)
            {
                this.Get_AFO_Text();
            }
        }
        #endregion Randbedingung

        #region  Objekt
        private void Text_Objekt_TextChanged(object sender, EventArgs e)
        {
            if (afo_ini == false)
            {
                this.Get_AFO_Text();
            }
        }
        #endregion Objekt 

        #region Qualitaet
        private void Text_Qualitaet_TextChanged(object sender, EventArgs e)
        {
            if (this.afo_ini == false)
            {
                this.Get_AFO_Text();
            }
        }
        #endregion Qualitaet

        #region Combo System
        private void combo_System_TextChanged(object sender, EventArgs e)
        {
            if (this.afo_ini == false && combo_system_index == false)
            {
                this.Get_AFO_Text();
            }
        }
        #endregion Combo System
        #endregion TextBox Change

        #region ComboBox Index
        #region System
        private void combo_System_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.afo_ini == false)
            {
                this.index_sys = combo_System.SelectedIndex;

                this.button_Sys_Artikel.Text = (combo_System.SelectedItem as NodeType).W_Artikel;
                this.button_Sys_Artikel.Refresh();

                if((combo_System.SelectedItem as NodeType).W_SINGULAR == true)
                {
                    this.checkBox_Plural.Checked = false;
                }
                this.checkBox_Plural.Refresh();

                this.Get_AFO_Text();

                combo_system_index = false;
            }

        }

        private void combo_System_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (afo_ini == false)
            {
                combo_system_index = true;
            }

        }
        #endregion System

        #region Aktivitaet
        private void comboBox_Aktivitaet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.afo_ini == false)
            {
                this.index_aktiviaet = this.comboBox_Aktivitaet.SelectedIndex;
                //Schaltflächen de- bzw. aktivieren
                if(this.comboBox_Aktivitaet.Text == this.Metamodel_Base.m_W_Aktiviaet[1])
                {
                    this.combo_Akteur.Enabled = true;
                    this.button_Art_Aktuer.Enabled = true;
                }
                else
                {
                    this.combo_Akteur.Enabled = false;
                    this.button_Art_Aktuer.Enabled = false;
                }

                if(this.Requirement_Edit.W_ZU == true)
                {
                    this.checkBox_W_ZU.Checked = true;
                }
                else
                {
                    this.checkBox_W_ZU.Checked = false;
                }
                this.checkBox_W_ZU.Update();



                this.Get_AFO_Text();
            }
        }
        #endregion Aktivitaet

        #region Prozesswort
        private void comboBox_Prozesswort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(afo_ini == false)
            {
                this.Get_AFO_Text();
            }      
        }
        #endregion Prozesswort

        #endregion ComboBox Index

        #region Button

        #region  Sys_Artikel
        private void button_Sys_Artikel_Click(object sender, EventArgs e)
        {
            if (afo_ini == false)
            {
                index_Artikel_sys = index_Artikel_sys + 1;
                if (index_Artikel_sys == 3)
                {
                    index_Artikel_sys = 0;
                }

                
                this.button_Sys_Artikel.Text = this.Metamodel_Base.m_Artikel[index_Artikel_sys];
                this.button_Sys_Artikel.Refresh();

                this.Get_AFO_Text();
            }
        }
        #endregion  Sys_Artikel

        #region Aktuer Titel
        private void button_Art_Aktuer_Click(object sender, EventArgs e)
        {
            if (afo_ini == false)
            {
                index_Artikel_act = index_Artikel_act + 1;
                if (index_Artikel_act == 4)
                {
                    index_Artikel_act = 0;
                }

                this.button_Art_Aktuer.Text = this.Metamodel_Base.m_Artikel_Akt[index_Artikel_act];
                this.button_Art_Aktuer.Refresh();

                this.Get_AFO_Text();
            }
        }
        #endregion Aktuer Titel

        #region Check_Satzschablone
        private void checkBox_Satzschablone_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_Satzschablone.Checked == true)
            {
                //Alle Möglichkeiten aktivieren
                Text_Randbedingung.Enabled = true;
                combo_System.Enabled = true;
                button_Sys_Artikel.Enabled = true;
                comboBox_Aktivitaet.Enabled = true;
                if (this.comboBox_Aktivitaet.Text != "dem <Akteur> die Möglichkeit bieten")
                {
                    this.combo_Akteur.Enabled = false;
                    this.button_Art_Aktuer.Enabled = false;
                }
                else
                {
                    this.combo_Akteur.Enabled = true;
                    this.button_Art_Aktuer.Enabled = true;
                }
                Text_Objekt.Enabled = true;
                Text_Qualitaet.Enabled = true;
                comboBox_Prozesswort.Enabled = true;
                //Afo Text erhalten
                this.Get_AFO_Text();
            }
            else
            {
                //Alle Möglichkeiten deaktivieren
                Text_Randbedingung.Enabled = false;
                combo_System.Enabled = false;
                button_Sys_Artikel.Enabled = false;
                comboBox_Aktivitaet.Enabled = false;
                combo_Akteur.Enabled = false;
                button_Art_Aktuer.Enabled = false;
                Text_Objekt.Enabled = false;
                Text_Qualitaet.Enabled = false;
                comboBox_Prozesswort.Enabled = false;

                this.Text_AFO_Text.Text = this.Requirement_Edit.AFO_TEXT;
            }
        }

        public void Set_Satzschablone_Status(bool flag)
        {
            if (flag == true)
            {
                this.checkBox_Satzschablone.Checked = true;

                //Alle Möglichkeiten aktivieren
                Text_Randbedingung.Enabled = true;
                combo_System.Enabled = true;
                button_Sys_Artikel.Enabled = true;
                comboBox_Aktivitaet.Enabled = true;
                if (this.comboBox_Aktivitaet.Text != "dem <Akteur> die Möglichkeit bieten")
                {
                    this.combo_Akteur.Enabled = false;
                    this.button_Art_Aktuer.Enabled = false;
                }
                else
                {
                    this.combo_Akteur.Enabled = true;
                    this.button_Art_Aktuer.Enabled = true;
                }
                Text_Objekt.Enabled = true;
                Text_Qualitaet.Enabled = true;
                comboBox_Prozesswort.Enabled = true;
                //Afo Text erhalten
                this.Get_AFO_Text();
            }
            else
            {
                this.checkBox_Satzschablone.Checked = false;
                //Alle Möglichkeiten deaktivieren
                Text_Randbedingung.Enabled = false;
                combo_System.Enabled = false;
                button_Sys_Artikel.Enabled = false;
                comboBox_Aktivitaet.Enabled = false;
                combo_Akteur.Enabled = false;
                button_Art_Aktuer.Enabled = false;
                Text_Objekt.Enabled = false;
                Text_Qualitaet.Enabled = false;
                comboBox_Prozesswort.Enabled = false;

                this.Text_AFO_Text.Text = this.Requirement_Edit.AFO_TEXT;
            }

            this.checkBox_Satzschablone.Update();
        }

        #endregion Check_Satzschablone

        #region Save
        private void button_Save_Click(object sender, EventArgs e)
        {
            bool change_node = false;
            bool change_akteuer = false;

            List<bool> m_change = new List<bool>();

            m_change.Add(change_node);
            m_change.Add(change_akteuer);


            string help_akteur = null; 
            //W_NUTZENDER
            if (this.index_aktiviaet == 1)
            {
                help_akteur = this.button_Art_Aktuer.Text + " ";

                if (this.combo_Akteur.Text == "" || this.combo_Akteur.Text == null)
                {
                    help_akteur = help_akteur + Data_Edit.metamodel.Stakeholder_Default;
                }
                else
                {
                    help_akteur = help_akteur + this.combo_Akteur.Text;
                }
            }

            if(this.Requirement_Edit.W_SUBJEKT != this.button_Sys_Artikel.Text + " " + this.combo_System.Text)
            {
                m_change[0] = true;
            }
            if(this.Requirement_Edit.W_NUTZENDER != help_akteur)
            {
                m_change[1] = true;
            }

                //Abfrage Übernaheme Änderungen
            if (m_change[0] == true || m_change[1] == true)
            {
                Requirement_Plugin.Forms.AFO_Erstellung.Abfrage_Annahme_Aenderung abfrage_Annahme_Aenderung = new Requirement_Plugin.Forms.AFO_Erstellung.Abfrage_Annahme_Aenderung(m_change);

                abfrage_Annahme_Aenderung.ShowDialog();

                change_node = abfrage_Annahme_Aenderung.m_ret[0];
                change_akteuer = abfrage_Annahme_Aenderung.m_ret[1];
            }

            

            //Werte in Requirement Databaseabspeichern
            ///////////////////////////////////////////////////
            #region Satzschablone
            //Satzschablone Elemente aktualisieren
            if (checkBox_Satzschablone.Checked == true)
            {
                //AFO_Text
                this.Requirement_Edit.AFO_TEXT = this.Text_AFO_Text.Text;
                //AFO_Titel
                this.Requirement_Edit.AFO_TITEL = this.Text_Header.Text;
                this.Requirement_Edit.Name = this.Text_Header.Text;
                //W_RANDBEDINGUNG
                this.Requirement_Edit.W_RANDBEDINGUNG = this.Text_Randbedingung.Text;
                //AFO_VERBINDLICHKEIT
                this.Requirement_Edit.AFO_VERBINDLICHKEIT = "muss";
                //W_SINGULAR
                if(this.checkBox_Plural.Checked == true)
                {
                    this.Requirement_Edit.W_SINGULAR = false;
                }
                else
                {
                    this.Requirement_Edit.W_SINGULAR = true;
                }
                
                //W_SUBJEKT
                this.Requirement_Edit.W_SUBJEKT = this.button_Sys_Artikel.Text + " " + this.combo_System.Text;
                //W_NUTZENDER
                if(this.index_aktiviaet == 1)
                {
                    this.Requirement_Edit.W_NUTZENDER = this.button_Art_Aktuer.Text + " "; 

                    if (this.combo_Akteur.Text == "" || this.combo_Akteur.Text == null)
                    {
                        this.Requirement_Edit.W_NUTZENDER  = this.Requirement_Edit.W_NUTZENDER + Data_Edit.metamodel.Stakeholder_Default;
                    }
                    else
                    {
                        this.Requirement_Edit.W_NUTZENDER = this.Requirement_Edit.W_NUTZENDER + this.combo_Akteur.Text;
                    }
                }
                else
                {
                    this.Requirement_Edit.W_NUTZENDER = "";
                }
                //W_AKTIVITAET
                this.Requirement_Edit.W_AKTIVITAET = (W_AKTIVITAET)comboBox_Aktivitaet.SelectedIndex;
                //W_OBJECT
                this.Requirement_Edit.W_OBJEKT = this.Text_Objekt.Text;
                //W_QUALITAET
                this.Requirement_Edit.W_QUALITAET = this.Text_Qualitaet.Text;
                //W_PROZESSWORT
                this.Requirement_Edit.W_PROZESSWORT = this.comboBox_Prozesswort.Text;
                //W_ZU
                if(this.checkBox_W_ZU.Checked == false)
                {
                    this.Requirement_Edit.W_ZU = false;
                }
                else
                {
                    this.Requirement_Edit.W_ZU = true;
                }
                //W_AFO_MANUAL
                this.Requirement_Edit.W_AFO_MANUAL = false;

                
            }
            else
            {
                //AFO_Text
                this.Requirement_Edit.AFO_TEXT = this.Text_AFO_Text.Text;
                //AFO_Titel
                this.Requirement_Edit.AFO_TITEL = this.Text_Header.Text;
                this.Requirement_Edit.Name = this.Text_Header.Text;
                //W_RANDBEDINGUNG
                if (this.Requirement_Edit.W_RANDBEDINGUNG == null)
                {
                    this.Requirement_Edit.W_RANDBEDINGUNG = "";
                }
                //AFO_VERBINDLICHKEIT
                this.Requirement_Edit.AFO_VERBINDLICHKEIT = "muss";
                //W_SINGULAR
                this.Requirement_Edit.W_SINGULAR = true;
                //W_SUBJEKT
                if (this.Requirement_Edit.W_SUBJEKT == null )
                {
                    this.Requirement_Edit.W_SUBJEKT = "";
                }
                //W_NUTZENDER
                if (this.Requirement_Edit.W_NUTZENDER == null)
                {
                    this.Requirement_Edit.W_NUTZENDER = "";
                }
                //W_AKTIVITAET
                if(this.Requirement_Edit.W_AKTIVITAET == null)
                this.Requirement_Edit.W_AKTIVITAET = (W_AKTIVITAET)0;
                //W_OBJECT
                if (this.Requirement_Edit.W_OBJEKT == null)
                {
                    this.Requirement_Edit.W_OBJEKT = "";
                }
                //W_QUALITAET
                if(this.Requirement_Edit.W_QUALITAET == null)
                {
                    this.Requirement_Edit.W_QUALITAET = "";
                }
                //W_PROZESSWORT
                if(this.Requirement_Edit.W_PROZESSWORT == null)
                {
                    this.Requirement_Edit.W_PROZESSWORT = "";

                }
                //W_ZU
                if(this.Requirement_Edit.W_ZU == null)
                {
                    this.Requirement_Edit.W_ZU = false;
                }
                //W_AFO_MANUAL
                this.Requirement_Edit.W_AFO_MANUAL = true;

            }
            //Allgemeine Werte
            this.Requirement_Edit.W_FREEZE_TITLE = true;
            this.Requirement_Edit.AFO_VERERBUNG = (AFO_VERERBUNG)0;
            #endregion Satzschablone
            ////////////////////////////////////////////////
            #region Textfelder
            //Form Textfelder
            //Autor
            this.Requirement_Edit.AFO_ANSPRECHPARTNER = this.richText_Autor.Text;
            //Hinweis
            this.Requirement_Edit.AFO_HINWEIS = this.richText_Hinweis.Text;
            //Mitgeltendes Dokument
            this.Requirement_Edit.AFO_REGELUNGEN = this.richText_Dokument.Text;
            //Begründung
            this.Requirement_Edit.B_BEMERKUNG = this.richText_Begründung.Text;
            //Quelltextzitat
            this.Requirement_Edit.AFO_QUELLTEXT = this.richText_Zitat.Text;
            //Abnahmekriterium
            this.Requirement_Edit.AFO_ABNAHMEKRITERIUM = this.richTextBoxpanelPrüfung.Text;
            #endregion Textfelder
            //////////////////////////////////////////////////
            #region Klärungspunkte
            this.Requirement_Edit.AFO_KLAERUNGSPUNKTE = Text_Klaerungspunkte.Text;
            #endregion Klärungspunkte
            /////////////////////////////////////////////////
            #region Bewertung
            //Detailstufe
            this.Requirement_Edit.AFO_DETAILSTUFE = (AFO_DETAILSTUFE)this.combo_Detailstufe.SelectedIndex;
            //Kategorie
            this.Requirement_Edit.IN_CATEGORY = (IN_CATEGORY)this.combo_Kategorie.SelectedIndex;
            //Kritikalitaet
            this.Requirement_Edit.AFO_KRITIKALITAET = (AFO_KRITIKALITAET)this.combo_Kritikalitaet.SelectedIndex;
            //Prioritaet Vergabe
            this.Requirement_Edit.AFO_PRIORITAET_VERGABE = (AFO_PRIORITAET_VERGABE)this.combo_PriotitaetVergabe.SelectedIndex;
            //OperativeBewertung
            this.Requirement_Edit.AFO_OPERATIVEBEWERTUNG = (AFO_OPERATIVEBEWERTUNG)this.comboBox_OperativeBewertung.SelectedIndex;
            //AG_ID
            this.Requirement_Edit.AFO_AG_ID =  this.richText_AG_ID.Text;
            //AN_ID
            this.Requirement_Edit.AFO_AN_ID = this.richText_AN_ID.Text;
            #endregion Bewertung
            /////////////////////////////////////////////////
            #region Auswahl
            //AFO_WV_ART
            this.Requirement_Edit.AFO_WV_ART = (AFO_WV_ART)this.combo_Art_AFO.SelectedIndex;
            
           
            //Status
            this.Requirement_Edit.AFO_STATUS = (AFO_STATUS)this.combo_status.SelectedIndex;
            //QS-Status
            this.Requirement_Edit.AFO_QS_STATUS = (AFO_QS_STATUS)this.combo_qsstatus.SelectedIndex;
            //Phase
            //this.Requirement_Edit.AFO_CPM_PHASE = (AFO_CPM_PHASE)this.combo_Phase.SelectedIndex;
            this.Requirement_Edit.AFO_CPM_PHASE = (AFO_CPM_PHASE)this.comboBox_Phasen.SelectedIndex;
            //Bezug
            this.Requirement_Edit.AFO_BEZUG = (AFO_BEZUG)this.combo_Bezug.SelectedIndex;
            //Nachweisart
            this.Requirement_Edit.AFO_WV_NACHWEISART = (AFO_WV_NACHWEISART)this.comboBoxpanelPrufungNachweisart.SelectedIndex;
            //Projektrolle
            this.Requirement_Edit.AFO_PROJEKTROLLE = (AFO_PROJEKTROLLE)this.combo_Projektrolle.SelectedIndex;
            //RPI_Stereotype
            this.Requirement_Edit.RPI_Stereotype = this.comboBox_RPIStereotype.Text;
            #endregion Auswahl
            /////////////////////////////////////////////////
            #region Checkbox
            //RPI_Export
            if (this.checkBox_Export.Checked == true)
            {
                this.Requirement_Edit.RPI_Export = true;
            }
            else
            {
                this.Requirement_Edit.RPI_Export = false;
            }
            //funktional
            if (this.checkBox_funktional.Checked == true)
            {
                this.Requirement_Edit.AFO_FUNKTIONAL = (AFO_FUNKTIONAL)0;
                this.Requirement_Edit.ADMBw_Stereotype = this.Data_Edit.metamodel.m_Requirement_ADMBw[0].Stereotype;
            }
            else
            {
                this.Requirement_Edit.AFO_FUNKTIONAL = (AFO_FUNKTIONAL)1;
                this.Requirement_Edit.ADMBw_Stereotype = this.Data_Edit.metamodel.m_Requirement_ADMBw[1].Stereotype;
            }
            #endregion
            ////////////////////////////////////////////////
            //Werte des Requirement in das Repository schreiben
            this.Requirement_Edit.W_FREEZE_TITLE = false;
            if(this.create_con == true)
            {
                this.Requirement_Edit.Refresh_in_Diagram(this.repository, this.Data_Edit);
            }
           
            //Konnektor Systemelement
            Repository_Connector repository_Connector = new Repository_Connector();

            if(this.Data_Edit.metamodel.flag_sysarch == false)
            {
                List<NodeType> m_nt = this.Data_Edit.m_NodeType.Where(x => x.SYS_KUERZEL == this.combo_System.Text).ToList();

                if (this.index_sys >= 0 && m_nt.Count == 1 && this.create_con == true)
                {
                    repository_Connector.Create_Dependency(this.Requirement_Edit.Classifier_ID, m_nt[0].Classifier_ID, this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.SubType).ToList()[0], repository, Data_Edit, this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.Toolbox).ToList()[0], this.Data_Edit.metamodel.m_Derived_Element[0].direction);
                }
            }
            else
            {
                List<SysElement> m_nt = this.Data_Edit.m_SysElemente.Where(x => x.SYS_KUERZEL == this.combo_System.Text).ToList();

                if(m_nt.Count > 0)
                {

                    if (this.index_sys >= 0 && m_nt.Count == 1 && this.create_con == true)
                    {
                        repository_Connector.Create_Dependency(this.Requirement_Edit.Classifier_ID, m_nt[0].Classifier_ID, this.Data_Edit.metamodel.m_Derived_SysElement.Select(x => x.Stereotype).ToList(), this.Data_Edit.metamodel.m_Derived_SysElement.Select(x => x.Type).ToList(), this.Data_Edit.metamodel.m_Derived_SysElement.Select(x => x.SubType).ToList()[0], repository, Data_Edit, this.Data_Edit.metamodel.m_Derived_SysElement.Select(x => x.Toolbox).ToList()[0], this.Data_Edit.metamodel.m_Derived_SysElement[0].direction);
                    }

                /*    if (m_nt[0].m_Implements.Count > 0 && this.create_con == true)
                    {
                        int m1 = 0;
                        do
                        {
                            repository_Connector.Create_Dependency(this.Requirement_Edit.Classifier_ID, m_nt[0].m_Implements[m1].Classifier_ID, this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), null, repository, Data_Edit);

                            m1++;
                        } while (m1 < m_nt[0].m_Implements.Count);
                    }
                */
                }


            }
           
            //Konnektor Aktuer
            List<Stakeholder> m_st = this.Data_Edit.m_Stakeholder.Where(x => x.st_STAKEHOLDER == this.combo_Akteur.Text).ToList();

            if (this.index_stakeholder >= 0 && m_st.Count == 1 && this.create_con == true)
            {
                repository_Connector.Create_Dependency(this.Requirement_Edit.Classifier_ID, m_st[0].Classifier_ID, this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList(), this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.Type).ToList(), this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.SubType).ToList()[0], repository, Data_Edit, this.Data_Edit.metamodel.m_Derived_Element.Select(x => x.Toolbox).ToList()[0], this.Data_Edit.metamodel.m_Derived_Element[0].direction);
            }

            /////Vergleich 
            this.Requirement_Edit.Compare_Requirement(this.Data_Edit, this.repository, this.Requirement_Copy);

            //NodeType refresehen
            if(change_node == true)
            {
                TV_Map help = Data_Edit.metamodel.SYS_Tagged_Values.Find(x => x.Name == "SYS_ARTIKEL");

                if (this.Data_Edit.metamodel.flag_sysarch == false)
                {
                    if (this.Data_Edit.m_NodeType[index_sys].Classifier_ID != null)
                    {
                        this.Data_Edit.m_NodeType[index_sys].SYS_ARTIKEL = (SYS_ARTIKEL)Data_Edit.SYS_ENUM.Get_Index(Data_Edit.SYS_ENUM.SYS_ARTIKEL, this.button_Sys_Artikel.Text);
                        this.Data_Edit.m_NodeType[index_sys].W_Artikel = this.button_Sys_Artikel.Text;
                        this.Data_Edit.m_NodeType[index_sys].Name = this.combo_System.Text;
                        this.Data_Edit.m_NodeType[index_sys].SYS_BEZEICHNUNG = this.combo_System.Text;
                        if (this.checkBox_Plural.Checked == false)
                        {
                            this.Data_Edit.m_NodeType[index_sys].W_SINGULAR = true;
                        }
                        else
                        {
                            this.Data_Edit.m_NodeType[index_sys].W_SINGULAR = false;
                        }


                        this.Data_Edit.m_NodeType[index_sys].Update_reduced(this.Data_Edit, repository);
                    }
                }
                else
                {
                    if (this.Data_Edit.m_SysElemente[index_sys].Classifier_ID != null)
                    {
                        this.Data_Edit.m_SysElemente[index_sys].SYS_ARTIKEL = (SYS_ARTIKEL)Data_Edit.SYS_ENUM.Get_Index(Data_Edit.SYS_ENUM.SYS_ARTIKEL, this.button_Sys_Artikel.Text);
                        this.Data_Edit.m_SysElemente[index_sys].W_Artikel = this.button_Sys_Artikel.Text;
                        this.Data_Edit.m_SysElemente[index_sys].Name = this.combo_System.Text;
                        this.Data_Edit.m_SysElemente[index_sys].SYS_BEZEICHNUNG = this.combo_System.Text;

                        if (this.checkBox_Plural.Checked == false)
                        {
                            this.Data_Edit.m_SysElemente[index_sys].W_SINGULAR = true;
                        }
                        else
                        {
                            this.Data_Edit.m_SysElemente[index_sys].W_SINGULAR = false;
                        }

                        this.Data_Edit.m_SysElemente[index_sys].Update_reduced(this.Data_Edit, repository);
                    }
                }
            }
            //Stakeholder Refrehsen
            if (change_akteuer == true)
            {
                if (this.Data_Edit.m_Stakeholder.Count > 0)
                {
                    List<Stakeholder> m_recent_st = this.Data_Edit.m_Stakeholder.Where(x => x.Name == this.Data_Edit.m_Stakeholder[index_stakeholder].st_STAKEHOLDER).ToList().Where(x => x.Classifier_ID != null).ToList(); ;

                    if(m_recent_st.Count > 0)
                    {
                        m_recent_st[0].st_ARTIKEL = this.Metamodel_Base.m_Artikel[index_Artikel_act];
                        m_recent_st[0].Name = this.combo_Akteur.Text;
                        m_recent_st[0].st_STAKEHOLDER = this.combo_Akteur.Text;
                        m_recent_st[0].w_NUTZENDER = this.combo_Akteur.Text;

                        m_recent_st[0].Update_st_reduced(this.Data_Edit, repository);

                        //Binding Stakeholder aktualisieren
                    //    BindingSource bindingSource_Stakeholder = new BindingSource();
                    //    bindingSource_Stakeholder.DataSource = this.Data_Edit.m_Stakeholder;
                    //    this.combo_Akteur.DataSource = (bindingSource_Stakeholder);
                    //    this.combo_Akteur.DisplayMember = "w_Nutzender";
                    //    this.combo_Akteur.SelectedIndex = index_stakeholder;
                    //    this.combo_Akteur.Refresh();
                    }

                   /* if (this.Data_Edit.m_Stakeholder[index_stakeholder].Classifier_ID != null)
                    {
                        this.Data_Edit.m_Stakeholder[index_stakeholder].st_ARTIKEL = this.Metamodel_Base.m_Artikel[index_Artikel_act];
                        this.Data_Edit.m_Stakeholder[index_stakeholder].Name = this.combo_Akteur.Text;
                        this.Data_Edit.m_Stakeholder[index_stakeholder].st_STAKEHOLDER = this.combo_Akteur.Text;

                        this.Data_Edit.m_Stakeholder[index_stakeholder].Update_st_reduced(this.Data_Edit, repository);

                        //Element Name updaten3
                    }*/
                }
            }
               
            this.dialog_result = DialogResult.OK;

        }
        #endregion Save
        #endregion Button

        #region Hilfsfunctionen
        private void Get_AFO_Text()
        {
            string AFO_Text = "";
            bool pos_quali = false;

            List<string> AFO_Text_List = new List<string>();
            AFO_Text_List.Add(this.Text_Randbedingung.Text);        //0
            if (this.checkBox_Plural.Checked == true)               //1
            {
                AFO_Text_List.Add("müssen ");
            }
            else
            {
                AFO_Text_List.Add("muss ");
            }                             
            AFO_Text_List.Add(this.button_Sys_Artikel.Text);        //2

            if(this.Data_Edit.metamodel.flag_sysarch == false)
            {
                string test = "";
                if (this.combo_System.SelectedItem != null)
                {
                    test = (this.combo_System.SelectedItem as NodeType).Name;

                    if(test != this.combo_System.Text)
                    {
                        test = this.combo_System.Text;
                    }
                }
                else
                {
                    test = "Fähigkeitsträger";

                    if (test != this.combo_System.Text)
                    {
                        test = this.combo_System.Text;
                    }
                }
                AFO_Text_List.Add(test);

            }
            else
            {
                string test = "";
                if (this.combo_System.SelectedItem != null)
                {
                     test = (this.combo_System.SelectedItem as SysElement).Name;
                    if (test != this.combo_System.Text)
                    {
                        test = this.combo_System.Text;
                    }
                }
                else
                {
                     test = "Fähigkeitsträger";
                    if (test != this.combo_System.Text)
                    {
                        test = this.combo_System.Text;
                    }
                }
                
                AFO_Text_List.Add(test);
            }

           

                   //3
            //SysElement sysElement = this.combo_System.SelectedItem;
            // AFO_Text_List.Add(this.combo_System.SelectedText);
          //  test = (this.combo_System.SelectedItem.);
            AFO_Text_List.Add(this.button_Art_Aktuer.Text);         //4
            //   AFO_Text_List.Add(this.Metamodel_Base.m_Artikel_Akt[index_Artikel_act]);
            if (this.Data_Edit.m_Stakeholder.Count == 0)
            {
                Stakeholder help = new Stakeholder(null, null, null);
                help.Name = Data_Edit.metamodel.Stakeholder_Default;

                this.Data_Edit.m_Stakeholder.Add(help);
            }

            AFO_Text_List.Add(this.Data_Edit.m_Stakeholder[index_stakeholder].Name);             //5
            if (AFO_Text_List[5] == null)
            {
                AFO_Text_List[5] = Data_Edit.metamodel.Stakeholder_Default;
            }

            if (AFO_Text_List[5] != this.combo_Akteur.Text)
            {
                AFO_Text_List[5] = this.combo_Akteur.Text;
            }

            string act2 = "";
            if ((this.comboBox_Prozesswort.SelectedItem as Activity) !=  null)
            {
                act2 = (this.comboBox_Prozesswort.SelectedItem as Activity).W_Prozesswort;
                if(act2 == "")
                {
                    act2 = this.comboBox_Prozesswort.Text;
                }
            }
            else
            {
                act2 = this.comboBox_Prozesswort.Text;
            }
          

            AFO_Text_List.Add(this.comboBox_Aktivitaet.Text);       //6
          
            AFO_Text_List.Add(", ");                                //7
            AFO_Text_List.Add(" ");                                  //8
            AFO_Text_List.Add(this.Text_Objekt.Text);               //9
            AFO_Text_List.Add(this.Text_Qualitaet.Text);            //10
            AFO_Text_List.Add("zu ");                               //11
          //  AFO_Text_List.Add(this.comboBox_Prozesswort.Text);      //12
            AFO_Text_List.Add(act2);
            AFO_Text_List.Add(".");                                 //13

            //Mit Randbedingung
            if (AFO_Text_List[0] != "" && AFO_Text_List[0] != null)
            {
                //Randbedingung
                AFO_Text = AFO_Text_List[0];
                AFO_Text = AFO_Text[0].ToString().ToUpper() + AFO_Text.Substring(1);
                AFO_Text = AFO_Text + AFO_Text_List[8];
                //muss
                AFO_Text = AFO_Text + AFO_Text_List[1];
                //Sys Artikel
                AFO_Text = AFO_Text + AFO_Text_List[2];
                if (AFO_Text_List[2] != "")
                {
                    AFO_Text = AFO_Text + AFO_Text_List[8];
                }
                //Sys Name
                AFO_Text = AFO_Text + AFO_Text_List[3];
                if (AFO_Text_List[3] != "")
                {
                    AFO_Text = AFO_Text + AFO_Text_List[8];
                }

            }
            else //ohne Randbedingung
            {
                //Sys Artiekl
                AFO_Text = AFO_Text + AFO_Text_List[2];
                if (AFO_Text_List[2] != "")
                {
                    AFO_Text = AFO_Text + AFO_Text_List[8];
                }
                AFO_Text = AFO_Text[0].ToString().ToUpper() + AFO_Text.Substring(1);
                //Sys Name
                AFO_Text = AFO_Text + AFO_Text_List[3];
                if (AFO_Text_List[3] != "")
                {
                    AFO_Text = AFO_Text + AFO_Text_List[8];
                }
                //muss
                AFO_Text = AFO_Text + AFO_Text_List[1];
            }

            //Varante Akteur
            if (AFO_Text_List[6] == this.Metamodel_Base.m_W_Aktiviaet[1])
            {
                //Artiek Stakeholder
                AFO_Text = AFO_Text + AFO_Text_List[4];
                if (AFO_Text_List[4] != "")
                {
                    AFO_Text = AFO_Text + AFO_Text_List[8];
                }
                //Name Stakeholder
                AFO_Text = AFO_Text + AFO_Text_List[5];
                if (AFO_Text_List[5] != "")
                {
                    AFO_Text = AFO_Text + AFO_Text_List[8];
                }
            }

            if (AFO_Text_List[6] == this.Metamodel_Base.m_W_Aktiviaet[0])
            {
                AFO_Text_List[6] = "";
            }
            //Aktiviaet einfügen
            AFO_Text = AFO_Text + AFO_Text_List[6];

            if (AFO_Text_List[6] != "")
            {
                AFO_Text = AFO_Text + AFO_Text_List[7];
            }
            else
            {
                //  AFO_Text = AFO_Text + AFO_Text_List[8];
            }

            if(AFO_Text_List[9].Contains(this.Data_Edit.metamodel.Pos_Qualitaet_Operator) == true)
            {
                pos_quali = true;

                AFO_Text_List[9] = AFO_Text_List[9].Replace(this.Data_Edit.metamodel.Pos_Qualitaet_Operator, AFO_Text_List[10]);

            }

            //Objekt
            AFO_Text = AFO_Text + AFO_Text_List[9];
            if (AFO_Text_List[9] != "")
            {
                AFO_Text = AFO_Text + AFO_Text_List[8];
            }

            //Qualitaet
            if(pos_quali == false)
            {
                AFO_Text = AFO_Text + AFO_Text_List[10];
                if (AFO_Text_List[10] != "")
                {
                    AFO_Text = AFO_Text + AFO_Text_List[8];
                }
            }

            //Zu einfügen
            if (AFO_Text_List[6] == this.Metamodel_Base.m_W_Aktiviaet[1] || AFO_Text_List[6] == this.Metamodel_Base.m_W_Aktiviaet[2])
            {
                if(this.Requirement_Edit.W_ZU == true)
                {
                    AFO_Text = AFO_Text + AFO_Text_List[11];
                }
               
            }

            //Prozesswort
            AFO_Text = AFO_Text + AFO_Text_List[12];
            //Satzende
            AFO_Text = AFO_Text + AFO_Text_List[13];
            //TextBox beschreiebn
            this.Text_AFO_Text.Text = AFO_Text;
            this.Text_AFO_Text.Refresh();

        }

       
#endregion Hilfsfunktionen

        private void Form_Edit_Requirement_Load(object sender, EventArgs e)
        {

        }

        private void comboBox_Prozesswort_TextUpdate(object sender, EventArgs e)
        {
            if (afo_ini == false)
            {
                this.Get_AFO_Text();
            }
        }

        private void combo_Akteur_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.afo_ini == false)
            {
                this.index_stakeholder = this.combo_Akteur.SelectedIndex;
                //Schaltflächen de- bzw. aktivieren
               

                this.Get_AFO_Text();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.dialog_result == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

            
            this.Close();
        }

        private void checkBox_logical_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_logical.Checked == false)
            {
                this.Data_Edit.metamodel.flag_sysarch = true;

                //Systemelemetne neu aufbauen
                Set_Systemelemente();
            }
            else
            {
                this.Data_Edit.metamodel.flag_sysarch = false;
                Set_Systemelemente();
            }
           
        }

        private void checkBox_W_ZU_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBox_W_ZU.Checked == true)
            {
                this.Requirement_Edit.W_ZU = true;
            }
            else
            {
                this.Requirement_Edit.W_ZU = false;
            }

            //AFO Text updaten
            this.Get_AFO_Text();
        }

        private void panel_reserve1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label_Abnahmekriterium_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_Plural_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_Plural.Checked == true)
            {
                this.Requirement_Edit.W_SINGULAR = false;
            }
            else
            {
                this.Requirement_Edit.W_SINGULAR = true;
            }
            //AFO Text updaten
            this.Get_AFO_Text();
        }

        private void checkBox_funktional_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void combo_Akteur_TextChanged(object sender, EventArgs e)
        {
            if (this.afo_ini == false && combo_Akteur.SelectedIndex != null)
            {
                this.Get_AFO_Text();
            }



        }

        private void begründungUndKommentareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Visisble Panel
            //Panel
            //reserve
            // this.panel_reserve1.Visible = false;
            // this.panel_reserve1.Enabled = false;
            this.panel_Bewertung.Visible = false;
            this.panel_Bewertung.Enabled = false;
            this.panel_MetadatenII.Visible = false;
            this.panel_MetadatenII.Enabled = false;
            //Satzschalone default
            this.panel_Satzschablone.Visible = false;
            this.panel_Satzschablone.Enabled = false;
            //Klaerungspunkte
            this.panel_Klaerungspunkte.Visible = false;
            this.panel_Klaerungspunkte.Enabled = false;
            //Metadaten
            this.panel_Metadaten.Visible = true;
            this.panel_Metadaten.Enabled = true;
            //Prüfung
            this.panel_Prüfung.Visible = false;
            this.panel_Prüfung.Enabled = false;
            #endregion Visiblie Panel
        }

        private void label_Zitat_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_OperativeBewertung_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void combo_Bezug_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class Choose_Require7_xac : Form
    {
        public Requirement_Plugin.Database Data;
        private bool flag_Choose;

        public Choose_Require7_xac(Requirement_Plugin.Database Data)
        {
            this.Data = Data;
            this.flag_Choose = false;


            InitializeComponent();

            //Tooltipps
            #region Tooltipss
            ToolTip toolTip_aufloesung = new ToolTip();
            toolTip_aufloesung.SetToolTip(checkBox_Szenar_Aufloesung, "Wird diese CheckBox aktiviert, werden beim Import soweit wie möglich Parts in den Szenaren angelegt, welchen den unbestimmten technischen Systemen aus Require7 entsprechen.");
            ToolTip toolTip_ueberpruefung = new ToolTip();
            toolTip_ueberpruefung.SetToolTip(checkBox_Ueberpreufung, "Wird diese CheckBox aktiviert, werden beim Import die importierten und im Modell vorhandenen Elemente und Konnektoren abgeglichen. Im Anschluss können Elemente zum Löschen ausgewählt werden.");

            #endregion Tooltips
        }


        private void tree_Choose_Export_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            tree_Choose_Export.Enabled = false;

            if(this.flag_Choose == false)
            {
                this.flag_Choose = true;
            }
        }

        private void tree_Choose_Export_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if(flag_Choose == true)
            {

                TreeNode Node = e.Node;
                //if (Node.Level == 0)
                //{
                    //Alle drunter checken oder unchecken
                    //Und entsprechend die flags setzen
                    if(Node.Checked == true)
                    {
                        Set_Nodes_rekursiv(true, Node);
                        Node.Tag = true;
                    }
                    else
                    {
                        Set_Nodes_rekursiv(false, Node);
                        Node.Tag = false;
                }
                //}


                ////////////
                //Variablen zum auswählen zurücksetzen
                flag_Choose = false;
                tree_Choose_Export.Enabled = true;
            }

        }

        private void Set_Nodes_rekursiv(bool state, TreeNode recent)
        {
            if(recent.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    recent.Nodes[i1].Checked = state;
                    recent.Nodes[i1].Tag = state;
                    Set_Nodes_rekursiv(state, recent.Nodes[i1]);
                    
                    i1++;
                } while (i1 < recent.Nodes.Count);
            }
        }

        private void OK_EXPORT_Click(object sender, EventArgs e)
        {

            TreeNode help;
            //Abrage welche gesetzt sind.
            //Systemelemente
            help = tree_Choose_Export.Nodes.Find("Technisches System", true)[0];
            if(help.Checked == true)
            {
                this.Data.sys_xac = true;
            }
            else
            {
                this.Data.sys_xac = false;
            }
            ////////////////////
            //Szenarbaum
            help = tree_Choose_Export.Nodes.Find("Szenarbaum", true)[0];
            if (help.Checked == true)
            {
                this.Data.logical_xac = true;
            }
            else
            {
                this.Data.logical_xac = false;
            }
            //////////////////////
            //Funktionsbaum
            help = tree_Choose_Export.Nodes.Find("Funktionsbaum", true)[0];
            if (help.Checked == true)
            {
                this.Data.capability_xac = true;
            }
            else
            {
                this.Data.capability_xac = false;
            }
            //Stakeholder
            help = tree_Choose_Export.Nodes.Find("Stakeholder", true)[0];
            if (help.Checked == true)
            {
                this.Data.stakeholder_xac = true;
            }
            else
            {
                this.Data.stakeholder_xac = false;
            }
            //////////////////////
            //Anforderungen
            //Schnittstellen
            help = tree_Choose_Export.Nodes.Find("Schnittstellen", true)[0];
            if (help.Checked == true)
            {
                this.Data.afo_interface_xac = true;
            }
            else
            {
                this.Data.afo_interface_xac = false;
            }
            //Funktional
            help = tree_Choose_Export.Nodes.Find("Funktional", true)[0];
            if (help.Checked == true)
            {
                this.Data.afo_funktional_xac = true;
            }
            else
            {
                this.Data.afo_funktional_xac = false;
            }
            //User
            help = tree_Choose_Export.Nodes.Find("Nutzer", true)[0];
            if (help.Checked == true)
            {
                this.Data.afo_user_xac = true;
            }
            else
            {
                this.Data.afo_user_xac = false;
            }
            //Design
            help = tree_Choose_Export.Nodes.Find("Design", true)[0];
            if (help.Checked == true)
            {
                this.Data.afo_design_xac = true;
            }
            else
            {
                this.Data.afo_design_xac = false;
            }
            //Process
            help = tree_Choose_Export.Nodes.Find("Process", true)[0];
            if (help.Checked == true)
            {
                this.Data.afo_process_xac = true;
            }
            else
            {
                this.Data.afo_process_xac = false;
            }
            //Umwelt
            help = tree_Choose_Export.Nodes.Find("Umwelt", true)[0];
            if (help.Checked == true)
            {
                this.Data.afo_umwelt_xac = true;
            }
            else
            {
                this.Data.afo_umwelt_xac = false;
            }
            //Typvertreter
            //Umwelt
            help = tree_Choose_Export.Nodes.Find("Typvertreter", true)[0];
            if (help.Checked == true)
            {
                this.Data.afo_typevertreter_xac = true;
            }
            else
            {
                this.Data.afo_typevertreter_xac = false;
            }
            /////////////////////////////////////////
            //Links
            //Decomposition
            help = tree_Choose_Export.Nodes.Find("SystemelementeSystemelemente", true)[0];
            if (help.Checked == true)
            {
                this.Data.link_decomposition = true;
            }
            else
            {
                this.Data.link_decomposition = false;
            }
            //Afo Links
            help = tree_Choose_Export.Nodes.Find("AnforderungenAnforderungen", true)[0];
            if (help.Checked == true)
            {
                this.Data.link_afo_afo = true;
            }
            else
            {
                this.Data.link_afo_afo = false;
            }
            //Sys--> Afo
            help = tree_Choose_Export.Nodes.Find("SystemelementeAnforderungen", true)[0];
            if (help.Checked == true)
            {
                this.Data.link_afo_sys = true;
            }
            else
            {
                this.Data.link_afo_sys = false;
            }
            //Capability --> AFo
            help = tree_Choose_Export.Nodes.Find("FunktionsbaumAnforderungen", true)[0];
            if (help.Checked == true)
            {
                this.Data.link_afo_cap = true;
            }
            else
            {
                this.Data.link_afo_cap = false;
            }
            //Logical --> AFo
            help = tree_Choose_Export.Nodes.Find("SzenarbaumAnforderungen", true)[0];
            if (help.Checked == true)
            {
                this.Data.link_afo_logical = true;
            }
            else
            {
                this.Data.link_afo_logical = false;
            }
            //Stakeholder --> AFo
            help = tree_Choose_Export.Nodes.Find("StakeholderAnforderungen", true)[0];
            if (help.Checked == true)
            {
                this.Data.link_afo_st = true;
            }
            else
            {
                this.Data.link_afo_st = false;
            }
            //Glossar
            help = tree_Choose_Export.Nodes.Find("Glossar", true)[0];
            if (help.Checked == true)
            {
                this.Data.glossary_xac = true;
            }
            else
            {
                this.Data.glossary_xac = false;
            }
            //Nachweisart
            help = tree_Choose_Export.Nodes.Find("Nachweisart", true)[0];
            if (help.Checked == true)
            {
                this.Data.Nachweisart_xac = true;
            }
            else
            {
                this.Data.Nachweisart_xac = false;
            }


            //Checkbox
            if(this.checkBox_Szenar_Aufloesung.Checked == true)
            {
                this.Data.Logical_aufloesung_xac = true;
            }
            else
            {
                this.Data.Logical_aufloesung_xac = false;
            }
            if (this.checkBox_Ueberpreufung.Checked == true)
            {
                this.Data.Check_xac = true;
            }
            else
            {
                this.Data.Check_xac = false;
            }
            if (this.checkBox_Sysele.Checked == true)
            {
                this.Data.metamodel.modus = 1;
            }
            else
            {
                this.Data.metamodel.modus = 0;
            }

            this.Close();
        }

        private void Choose_Require7_xac_Load(object sender, EventArgs e)
        {

        }

        private void checkBox_Szenar_Aufloesung_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Requirement_Plugin.Forms.Misc
{
    public partial class Optionen_RPI : Form
    {
        //Rückgabewerte
        public bool status = true;
        public Ennumerationen.AFO_CPM_PHASE AFO_CPM;

        private bool flag_build = false;
        private Ennumerationen.AFO_ENUM aFO_ENUM = new Ennumerationen.AFO_ENUM();

        public Optionen_RPI(bool status_RPI, Ennumerationen.AFO_CPM_PHASE aFO_CPM_PHASE)
        {
            //Initialisieren
            InitializeComponent();
            flag_build = true;
            ////////////////
            ///Übernahme Werte
            status = status_RPI;
            AFO_CPM = aFO_CPM_PHASE;

            ////////////////
            ///Comboboxen
          
            #region AFO_CPM_PHASE
            BindingSource bindingSource_phase2 = new BindingSource();
            bindingSource_phase2.DataSource = aFO_ENUM.AFO_CPM_PHASE;
            this.comboBox_CPM.DataSource = (bindingSource_phase2);
            
            #endregion

            /////////////////
            ///Setzen Werte
           
            #region Status RPI
            if (status == true)
            {
                this.radioButton_activ.Checked = true;
                this.radioButton_deact.Checked = false;
            }
            else
            {
                this.radioButton_activ.Checked = false;
                this.radioButton_deact.Checked = true;
            }

            #endregion

            #region CPM
            this.comboBox_CPM.SelectedIndex = (int)AFO_CPM;
            #endregion


            flag_build = false;
            this.panel_Optionen.Refresh();


      
        }

        private void Optionen_RPI_Load(object sender, EventArgs e)
        {

        }

        private void tab_Project_Selected(object sender, TabControlEventArgs e)
        {
            //index
            //0 --> Project
            //1 --> RPI
          switch(this.tab_Project.SelectedIndex)
          {
                case 0:
                    this.panel_Project.Visible = true;
                    this.panel_Project.IsAccessible = true;
                    this.panel_RPI.Visible = false;
                    this.panel_RPI.IsAccessible = false;
                    break;
                case 1:
                    this.panel_Project.Visible = false;
                    this.panel_Project.IsAccessible = false;
                    this.panel_RPI.Visible = true;
                    this.panel_RPI.IsAccessible = true;
                    break;

                default:
                    this.panel_Project.Visible = true;
                    this.panel_Project.IsAccessible = true;
                    this.panel_RPI.Visible = false;
                    this.panel_RPI.IsAccessible = false;
                    break;
          }

            this.panel_Optionen_2.Refresh();

           
        }

        private void radioButton_activ_CheckedChanged(object sender, EventArgs e)
        {
            if(flag_build == false)
            {
                if (this.radioButton_activ.Checked)
                {
                    this.status = true;
                }
                else
                {
                    this.status = false;
                }
            }
        }

        private void comboBox_CPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flag_build == false)
            {
                this.AFO_CPM = (Ennumerationen.AFO_CPM_PHASE)aFO_ENUM.Get_Index(aFO_ENUM.AFO_CPM_PHASE, comboBox_CPM.Text);

            }
        }
    }
}

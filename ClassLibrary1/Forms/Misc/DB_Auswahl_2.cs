using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database_Connection;

namespace Requirement_Plugin.Forms
{
    public partial class DB_Auswahl_2 : Form
    {

        private bool modus_manuell = false; //False auto; true manuell
        private List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> m_Repos = new List<Database_Connection.Repository.Repository_ODBC>();
        public int index_repositroy;
        public  Database database;
        public EA.Repository repository1;

        public DB_Auswahl_2(List<Requirement_Plugin.Database_Connection.Repository.Repository_ODBC> m_Reposs, int index, Database database1, EA.Repository repository)
        {
            InitializeComponent();

            this.m_Repos = m_Reposs;
            this.database = database1;
            this.repository1 = repository;

            if(index == -1)
            {
                index = 0;
            }

          

            #region Combobox Auto
            BindingSource bindingSource_repository = new BindingSource();
            bindingSource_repository.DataSource = this.m_Repos.Select(x => x.name).ToList(); ;
            this.comboBox_repository.DataSource = (bindingSource_repository);
            this.comboBox_repository.DisplayMember = "name";
            this.index_repositroy = index;
            this.comboBox_repository.SelectedIndex = this.index_repositroy;
            this.comboBox_repository.Refresh();

            this.richTextBox_auto.Text = m_Repos[this.index_repositroy].connection_string;
            this.richTextBox_auto.ReadOnly = true;
            this.richTextBox_auto.Refresh(); ;

            #endregion

        }

        private void DB_Auswahl_2_Load(object sender, EventArgs e)
        {
            //Sichtbare Fenster
            #region Fenseter Visibility
            this.panel_auto.Visible = true;
            this.panel_auto.Enabled = true;


            this.panel_manuell.Visible = false;
            this.panel_manuell.Enabled = false;

            this.tableLayoutPanel1.Refresh();
            this.tableLayoutPanel1.Update();

            #endregion
        }


        #region Stripmenu
        private void auswahlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Fenseter Visibility
            modus_manuell = false;

            this.panel_auto.Visible = true;
            this.panel_auto.Enabled = true;


            this.panel_manuell.Visible = false;
            this.panel_manuell.Enabled = false;

            this.tableLayoutPanel1.Refresh();
            this.tableLayoutPanel1.Update();

            #endregion
        }

        private void manuellEingabeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Fenseter Visibility
            modus_manuell = true;

            this.panel_auto.Visible = false;
            this.panel_auto.Enabled = false;


            this.panel_manuell.Visible = true;
            this.panel_manuell.Enabled = true;

            this.tableLayoutPanel1.Refresh();
            this.tableLayoutPanel1.Update();

            #endregion
        }
        #endregion Stripmenu


        #region Combobox
        private void comboBox_repository_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.index_repositroy = this.comboBox_repository.SelectedIndex;

            this.richTextBox_auto.Text = this.m_Repos[index_repositroy].connection_string;
            this.richTextBox_auto.Refresh();
        }

        #endregion
        #region Botton
        private void buttonOK_Click(object sender, EventArgs e)
        {
           

            this.database.metamodel.dB_Type = Ennumerationen.DB_Type.MSDASQL;

            if(this.modus_manuell == false)
            {
                if(this.database.oDBC_Interface == null)
                {
                    this.database.oDBC_Interface = new ODBC_Interface(this.repository1, this.database.Base);
                }

                this.database.oDBC_Interface.Set_Connection_String(this.richTextBox_auto.Text);
                this.database.oDBC_Interface.odbc_Open();
            }
            else
            {
                if (this.database.oDBC_Interface == null)
                {
                    this.database.oDBC_Interface = new ODBC_Interface(this.repository1, this.database.Base);
                }
                this.database.oDBC_Interface.Set_Connection_String(this.richTextBox_manuell.Text);
                this.database.oDBC_Interface.odbc_Open();
            }
            this.DialogResult = DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion


    }
}

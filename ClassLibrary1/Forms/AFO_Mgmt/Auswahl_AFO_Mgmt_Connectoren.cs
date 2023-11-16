using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Requirement_Plugin.Forms.AFO_Mgmt
{
    public partial class Auswahl_AFO_Mgmt_Connectoren : Form
    {
        private bool Check_Loops;
        private bool Check_Mult_Refines;
        private bool Check_Dopplung;
        public bool modus;

        public List<bool> m_ret = new List<bool>();

        private bool Check;

        public Auswahl_AFO_Mgmt_Connectoren()
        {


            this.Check_Loops = false;
            this.Check_Mult_Refines = false;
            this.Check_Dopplung = false;
            this.modus = true;

          

            this.Check = false;

            InitializeComponent();

            this.checkBox_Modus.Checked = true;
            this.checkBox_Modus.Update();
        }


        #region Button
        private void button_OK_Click(object sender, EventArgs e)
        {
           if(this.treeView_check.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    bool state = this.treeView_check.Nodes[i1].Checked;

                    switch (this.treeView_check.Nodes[i1].Name)
                    {
                        case "Knoten_Loops":
                            this.Check_Loops = state;
                            break;
                        case "Knoten_Mult_Feines":
                            this.Check_Mult_Refines = state;
                            break;
                        case "Knoten_Dopplung":
                            this.Check_Dopplung = state;
                            break;
                    }

                    i1++;
                } while (i1 < this.treeView_check.Nodes.Count);
            }

            if(this.checkBox_Modus.Checked == true)
            {
                this.modus = true;
            }
            else
            {
                this.modus = false;
            }


            this.m_ret.Add(this.Check_Loops);
            this.m_ret.Add(this.Check_Mult_Refines);
            this.m_ret.Add(this.Check_Dopplung);


            

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }
        #endregion

        #region Check
        private void button_check_Click(object sender, EventArgs e)
        {
            if (this.Check == false)
            {
                this.Set_State(true);
                this.Check = true;
                this.button_check.Name = "Uncheck All";
            }
            else
            {
                this.Set_State(false);
                this.Check = false;
                this.button_check.Name = "Check All";
            }

            this.Update();
        }

        private void Set_State(bool state)
        {
            if(this.treeView_check.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(state == true)
                    {
                        this.treeView_check.Nodes[i1].Checked = true;
                    }
                    else
                    {
                        this.treeView_check.Nodes[i1].Checked = false;
                    }

                    i1++;
                } while (i1 < this.treeView_check.Nodes.Count);

                this.treeView_check.Update();
            }
        }

        #endregion

      
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Requirement_Plugin.Forms
{
    public partial class Auswahl_AFO_Mgmt_Replaces : Form
    {
       private bool element_derived = false;
        private bool logical_derived = false;
        private bool capability_derived = false;

        private bool parent_refines = false;
        private bool child_refines = false;

        public List<bool> m_bool = new List<bool>();


        public Auswahl_AFO_Mgmt_Replaces(Database database)
        {
            InitializeComponent();

            //Checkboxen Namen ändern nach Stereotype
            #region Namen
            this.checkBox_DerivedElement.Name = database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList()[0] + " Elemente";
            this.checkBox_DerivedElement.Checked = true;
            this.checkBox_DerivedLogical.Name = database.metamodel.m_Derived_Logical.Select(x => x.Stereotype).ToList()[0] + " Logical";
            this.checkBox_DerivedLogical.Checked = true;
            this.checkBox_DerivedCapability.Name = database.metamodel.m_Derived_Capability.Select(x => x.Stereotype).ToList()[0] + " "+database.metamodel.m_Capability.Select(x => x.Stereotype).ToList()[0];
            this.checkBox_DerivedCapability.Checked = true;
            this.checkBox_refinesEltern.Name = database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList()[0] + " Elteranforderungen";
            this.checkBox_refinesEltern.Checked =true;
            this.checkBox_RefinesChildren.Name = database.metamodel.m_Afo_Refines.Select(x => x.Stereotype).ToList()[0] + " Kinderanforderungen";
            this.checkBox_RefinesChildren.Checked = true;


            this.Refresh();
            this.Update();
            #endregion

        }

        private void Auswahl_AFO_Mgmt_Replaces_Load(object sender, EventArgs e)
        {

        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            if(this.checkBox_DerivedElement.Checked == true)
            {
                element_derived = true;
            }
            else
            {
                element_derived = false;
            }

            if (this.checkBox_DerivedLogical.Checked == true)
            {
                logical_derived = true;
            }
            else
            {
                logical_derived = false;
            }

            if (this.checkBox_DerivedCapability.Checked == true)
            {
                capability_derived = true;
            }
            else
            {
                capability_derived = false;
            }

            if (this.checkBox_refinesEltern.Checked == true)
            {
                parent_refines = true;
            }
            else
            {
                parent_refines = false;
            }

            if (this.checkBox_RefinesChildren.Checked == true)
            {
                child_refines = true;
            }
            else
            {
                child_refines = false;
            }

            this.m_bool = new List<bool>();

            this.m_bool.Add(element_derived);
            this.m_bool.Add(logical_derived);
            this.m_bool.Add(capability_derived);
            this.m_bool.Add(parent_refines);
            this.m_bool.Add(child_refines);


            this.DialogResult = DialogResult.OK;


            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.m_bool = new List<bool>();

            this.DialogResult = DialogResult.Cancel;

            this.Close();
            
        }
    }
}

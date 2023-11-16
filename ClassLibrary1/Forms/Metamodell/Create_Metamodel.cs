using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Linq;

using Metamodels;

namespace Forms
{

    public partial class Create_Metamodel : Form
    {
        #region Variablen
        Metamodel metamodel;
        Create_Metamodel_Class metamodel_base = new Create_Metamodel_Class();
        bool create = false;
        DataGridView recent;

        int flag_type = 0; // 0 Element Metamodel /Szenar, 1 Dekomposition, 2 InformatioItem, 3 Konnektoren 3 Grid
        int grid4_Type = 0;

        int recent_columns = -1;
        int recent_row = -1;
        #endregion Variablen
     
        #region Initialisierung Form
        public Create_Metamodel(Metamodel metamodel, Create_Metamodel_Class help)
        {
            this.metamodel = metamodel;
            this.metamodel_base = help;

            InitializeComponent();

            //Split Container fixed setzen
            #region Split Container
         
  
            #endregion Split Container
        }

        private void Create_Metamodel_Load(object sender, EventArgs e)
        {
            panel_Grid1.Visible = false;
            panel_Grid2.Visible = false;
            panel_Grid3.Visible = false;
            panel_Grid4.Visible = false;
        }
        #endregion Initialisierung Form

        #region Reiter Elemente

        #region Szenar
        private void tag_Szenar_Click(object sender, EventArgs e)
        {
            this.recent = data_Metamodel;
            this.flag_type = 0;
            //Visibile tauschen
            #region Label
            this.label_Szenar.Text = "Szenar";
            #endregion LAbel
            //Visibile tauschen
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = true;
            panel_Grid1.Enabled = true;
            TextBox_DefaultName.Enabled = true;
            TextBox_DefaultName.Visible = true;
            combo_XAC_Attribut.Visible = false;
            comboBox_Type.Visible = true;
            comboBox_Stereotype.Visible = true;
            comboBox_Toolbox.Visible = true;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = metamodel.m_Szenar;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            
            this.data_Metamodel.Columns["XAC_Attribut"].Visible = false;
            this.data_Metamodel.Columns["DefaultName"].Visible = true;
            this.data_Metamodel.AutoResizeColumns();
        }
        #endregion Szenar

        #region Decomposition
        private void tag_Decomposition_Click(object sender, EventArgs e)
        {
            #region Label
            this.label_DecDef.Text = "Element Definition";
            this.label_DecUsage.Text = "Element Usage";
            #endregion  Label
            this.flag_type = 1;
            this.create = true;
            #region Visibility
            //Visibile tauschen
            //Grid1
          
            //splitContainer_Main.Visible = false;
            // data_Metamodel.Visible = false;
            panel_Grid1.Visible = false;
            panel_Grid1.Enabled = false;
            //Grid2
            panel_Grid2.Visible = true;
            panel_Grid2.Enabled = true;
            TextBox_DefaultName.Enabled = false;
            TextBox_DefaultName.Visible = false;
            combo_XAC_Attribut.Visible = true;
            combo_XAC_Attribut.Enabled = true;
            combo_XAC_Attribut.Visible = false;
            comboBox_Type.Visible = true;
            comboBox_Stereotype.Visible = true;
            comboBox_Toolbox.Visible = true;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //Auswahl
            #region Databinding Comboboxen
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XAC_Attribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = this.metamodel_base.sys_Enum.SYS_KOMPONENTENTYP.ToList();
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            this.combo_XAC_Attribut.Visible = true;
            this.combo_XAC_Attribut.Enabled = true;
            #endregion Databinding Comboboxen
            //Tabellen
            #region Databinding Tabellen
            //DataGrid_left
            BindingSource bindingSource_Type_Grid_left = new BindingSource();
            bindingSource_Type_Grid_left.DataSource = metamodel.m_Elements_Definition;
            this.data_metamodel_left.DataSource = (bindingSource_Type_Grid_left);
            this.data_metamodel_left.Columns["DefaultName"].Visible = false;
            this.data_metamodel_left.Columns["XAC_Attribut"].Visible = true;
            this.data_metamodel_left.CurrentCell = null;
            this.data_metamodel_left.AutoResizeColumns();
            //DataGrid_rigth
            BindingSource bindingSource_Type_Grid_rigth = new BindingSource();
            bindingSource_Type_Grid_rigth.DataSource = metamodel.m_Elements_Usage;
            this.data_Metamodel_right.DataSource = (bindingSource_Type_Grid_rigth);
            this.data_Metamodel_right.Columns["DefaultName"].Visible = false;
            this.data_Metamodel_right.Columns["XAC_Attribut"].Visible = true;
            this.data_Metamodel_right.CurrentCell = null;
            this.data_Metamodel_right.AutoResizeColumns();
            #endregion Databinding Tabellen

            this.create = false;
        }

  
       
        #endregion Dekomposition

        #region Anforderung

        private void tag_Anforderungen_Click(object sender, EventArgs e)
        {
            this.flag_type = 1;
            this.create = true;
            /*
            #region label
            this.label_AfoInterface.Text = "Requirement Interface";
            this.label_AfoFunctional.Text = "Requirement Functional";
            this.label_AfoUser.Text = "Requirement User";
            #endregion label
            #region Visibility
            //Visibile tauschen
            //Grid1

            //splitContainer_Main.Visible = false;
            // data_Metamodel.Visible = false;
            panel_Grid1.Visible = false;
            panel_Grid1.Enabled = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            TextBox_DefaultName.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            //Grid3
            panel_Grid3.Visible = true;
            panel_Grid3.Enabled = true;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //Auswahl
            #region Databinding Comboboxen
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XAC_Attribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = this.metamodel_base.afo_Enum.AFO_WV_ART.ToList();
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            #endregion Databinding Comboboxen
            //Tabellen
            #region Databinding Tabellen
            //DataGrid_left
            BindingSource bindingSource_Type_Grid_left = new BindingSource();
            bindingSource_Type_Grid_left.DataSource = metamodel.m_Requirement_Interface;
            this.data_3_left.DataSource = (bindingSource_Type_Grid_left);
            this.data_3_left.Columns["DefaultName"].Visible = false;
            this.data_3_left.CurrentCell = null;
            this.data_3_left.AutoResizeColumns();
            //DataGrid_middle
            BindingSource bindingSource_Type_Grid_middle = new BindingSource();
            bindingSource_Type_Grid_middle.DataSource = metamodel.m_Requirement_Functional;
            this.data_3_middle.DataSource = (bindingSource_Type_Grid_middle);
            this.data_3_middle.Columns["DefaultName"].Visible = false;
            this.data_3_middle.CurrentCell = null;
            this.data_3_middle.AutoResizeColumns();
            //DataGrid_rigth
            BindingSource bindingSource_Type_Grid_rigth = new BindingSource();
            bindingSource_Type_Grid_rigth.DataSource = metamodel.m_Requirement_User;
            this.data_3_right.DataSource = (bindingSource_Type_Grid_rigth);
            this.data_3_right.Columns["DefaultName"].Visible = false;
            this.data_3_right.CurrentCell = null;
            this.data_3_right.AutoResizeColumns();
            #endregion Databinding Tabellen

            create = false;
            */
            this.grid4_Type = 3;
            this.recent = data_grid4;
            this.flag_type = 1;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel.m_Requirement_Name.Select(x => x.Stereotype).ToList();
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut;
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = metamodel_base.afo_Enum.AFO_WV_ART.ToList();
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = null;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.AutoResizeColumns();

            create = false;
        }
        
        #endregion Anforderung

        #region InformationItem
        private void tag_InfromationItem_Click(object sender, EventArgs e)
        {
            this.recent = data_Metamodel;
            this.flag_type = 2;
            //Visibile tauschen
            #region Label
            this.label_Szenar.Text = "InformationItem";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = true;
            panel_Grid1.Enabled = true;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = false;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            combo_XAC_Attribut.Visible = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_InformationItem;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = metamodel.m_InformationItem;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.AutoResizeColumns();
            this.data_Metamodel.Columns["XAC_Attribut"].Visible = false;
            this.data_Metamodel.Columns["DefaultName"].Visible = false;
        }
        #endregion InformationItem        

        #region Aktivity
        private void tag_Aktivity_Click(object sender, EventArgs e)
        {
            this.flag_type = 1;
            this.create = true;
            #region Label

            #endregion Label
            this.label_DecDef.Text = "Aktivity Definition";
            this.label_DecUsage.Text = "Aktivity Usage";
            #region Visibility
            //Visibile tauschen
            //Grid1
          
            //splitContainer_Main.Visible = false;
            // data_Metamodel.Visible = false;
            panel_Grid1.Visible = false;
            panel_Grid1.Enabled = false;
            //Grid2
            panel_Grid2.Visible = true;
            panel_Grid2.Enabled = true;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //Auswahl
            #region Databinding Comboboxen
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XAC_Attribut
           /* BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource.C;
            */
            this.combo_XAC_Attribut.DataSource = null;
            this.combo_XAC_Attribut.Visible = false;
            this.combo_XAC_Attribut.Enabled = false;
            this.combo_XAC_Attribut.Text = null;
            #endregion Databinding Comboboxen
            //Tabellen
            #region Databinding Tabellen
            //DataGrid_left
            BindingSource bindingSource_Type_Grid_left = new BindingSource();
            bindingSource_Type_Grid_left.DataSource = metamodel.m_Aktivity_Definition;
            this.data_metamodel_left.DataSource = (bindingSource_Type_Grid_left);
            this.data_metamodel_left.AutoResizeColumns();
            this.data_metamodel_left.Columns["DefaultName"].Visible = false;
            this.data_metamodel_left.Columns["XAC_Attribut"].Visible = false;
            this.data_metamodel_left.CurrentCell = null;
            //DataGrid_rigth
            BindingSource bindingSource_Type_Grid_rigth = new BindingSource();
            bindingSource_Type_Grid_rigth.DataSource = metamodel.m_Aktivity_Usage;
            this.data_Metamodel_right.DataSource = (bindingSource_Type_Grid_rigth);
            this.data_Metamodel_right.AutoResizeColumns();
            this.data_Metamodel_right.Columns["DefaultName"].Visible = false;
            this.data_Metamodel_right.Columns["XAC_Attribut"].Visible = false;
            this.data_Metamodel_right.CurrentCell = null;
            #endregion Databinding Tabellen

            this.create = false;
        }
        #endregion Aktivity

        #region Stakeholder
        private void stakeholderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Label
            this.label_DecDef.Text = "Stakeholder Definition";
            this.label_DecUsage.Text = "Stakeholder Usage";
            #endregion  Label
            this.flag_type = 1;
            this.create = true;
            #region Visibility
            //Visibile tauschen
            //Grid1
          
            //splitContainer_Main.Visible = false;
            // data_Metamodel.Visible = false;
            panel_Grid1.Visible = false;
            panel_Grid1.Enabled = false;
            //Grid2
            panel_Grid2.Visible = true;
            panel_Grid2.Enabled = true;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //Auswahl
            #region Databinding Comboboxen
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XAC_Attribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = this.metamodel_base.sT_ENUM.ST_GRUPPE.ToList();
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            this.combo_XAC_Attribut.Visible = true;
            this.combo_XAC_Attribut.Enabled = true;
            #endregion Databinding Comboboxen
            //Tabellen
            #region Databinding Tabellen
            //DataGrid_left
            BindingSource bindingSource_Type_Grid_left = new BindingSource();
            bindingSource_Type_Grid_left.DataSource = metamodel.m_Stakeholder_Definition;
            this.data_metamodel_left.DataSource = (bindingSource_Type_Grid_left);
            this.data_metamodel_left.Columns["DefaultName"].Visible = false;
            this.data_metamodel_left.Columns["XAC_Attribut"].Visible = true;
            this.data_metamodel_left.CurrentCell = null;
            this.data_metamodel_left.AutoResizeColumns();
            //DataGrid_rigth
            BindingSource bindingSource_Type_Grid_rigth = new BindingSource();
            bindingSource_Type_Grid_rigth.DataSource = metamodel.m_Stakeholder_Usage;
            this.data_Metamodel_right.DataSource = (bindingSource_Type_Grid_rigth);
            this.data_Metamodel_right.Columns["DefaultName"].Visible = false;
            this.data_Metamodel_right.Columns["XAC_Attribut"].Visible = true;
            this.data_Metamodel_right.CurrentCell = null;
            this.data_Metamodel_right.AutoResizeColumns();
            #endregion Databinding Tabellen

            this.create = false;
        }
        #endregion

        #region Fähigkeitsbaum
        private void fähigkeitsbaumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grid4_Type = 1;
            this.recent = data_grid4;
            this.flag_type = 1;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = false;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel_base.combo_Taxonomy;
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = null;
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = null;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.AutoResizeColumns();
        }
        #endregion Fähigkeitsbaum

        #region Constraints
        private void constraintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grid4_Type = 4;
            this.recent = data_grid4;
            this.flag_type = 1;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = false;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel.m_Op_Constraint_Name.Select(x => x.Stereotype).ToList();
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut;
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = metamodel_base.afo_Enum.AFO_WV_ART.ToList();
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = null;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.AutoResizeColumns();

            create = false;
        }
        #endregion Constrains

        #region BPMN
        private void activityBPMN20ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flag_type = 1;
            this.create = true;

            this.grid4_Type = 6;
            this.recent = data_grid4;
            this.flag_type = 1;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel_base.combo_BPMN_Elemenet;
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut;
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = metamodel_base.afo_Enum.AFO_WV_ART.ToList();
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = null;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.AutoResizeColumns();

            create = false;
        }
        #endregion BPMN
        #endregion Reiter Elemente

        #region Konnektoren

        #region Derived
        private void derivedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flag_type = 3;
            this.create = true;
            #region label
            this.label_AfoInterface.Text = "Ableitung Fähigkeitsbaum";
            this.label_AfoFunctional.Text = "Ableitung Szenare";
            this.label_AfoUser.Text = "Ableitung Elemente";
            #endregion label
            #region Visibility
            //Visibile tauschen
            //Grid1
        
            //splitContainer_Main.Visible = false;
            // data_Metamodel.Visible = false;
            panel_Grid1.Visible = false;
            panel_Grid1.Enabled = false;
            //Grid2
            panel_Grid2.Visible = false;
            TextBox_DefaultName.Enabled = false;
            TextBox_DefaultName.Visible = false;
            combo_XAC_Attribut.Visible = false;
            //Grid3
            panel_Grid3.Visible = true;
            panel_Grid3.Enabled = true;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //Auswahl
            #region Databinding Comboboxen
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XAC_Attribut
            this.combo_XAC_Attribut.DataSource = null;
            #endregion Databinding Comboboxen
            //Tabellen
            #region Databinding Tabellen
            //DataGrid_left
            BindingSource bindingSource_Type_Grid_left = new BindingSource();
            bindingSource_Type_Grid_left.DataSource = metamodel.m_Derived_Capability;
            this.data_3_left.DataSource = (bindingSource_Type_Grid_left);
            this.data_3_left.Columns["SubType"].Visible = true;
            this.data_3_left.Columns["XAC"].Visible = false;
            this.data_3_left.CurrentCell = null;
            this.data_3_left.AutoResizeColumns();
            //DataGrid_middle
            BindingSource bindingSource_Type_Grid_middle = new BindingSource();
            bindingSource_Type_Grid_middle.DataSource = metamodel.m_Derived_Logical;
            this.data_3_middle.DataSource = (bindingSource_Type_Grid_middle);
            this.data_3_middle.Columns["SubType"].Visible = true;
            this.data_3_middle.Columns["XAC"].Visible = false;
            this.data_3_middle.CurrentCell = null;
            this.data_3_middle.AutoResizeColumns();
            //DataGrid_rigth
            BindingSource bindingSource_Type_Grid_rigth = new BindingSource();
            bindingSource_Type_Grid_rigth.DataSource = metamodel.m_Derived_Element;
            this.data_3_right.DataSource = (bindingSource_Type_Grid_rigth);
            this.data_3_right.Columns["SubType"].Visible = true;
            this.data_3_right.Columns["XAC"].Visible = false;
            this.data_3_right.CurrentCell = null;
            this.data_3_right.AutoResizeColumns();
            #endregion Databinding Tabellen

            create = false;
        }
        #endregion Derived

        #region Infromatiosnaustausch
        private void informationsaustauschToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.recent = data_Metamodel;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_Szenar.Text = "Konnektor Informationsaustausch";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = true;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = false;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            combo_XAC_Attribut.Visible = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = metamodel.m_Infoaus;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.Columns["SubType"].Visible = true;
            this.data_Metamodel.Columns["XAC"].Visible = false;
            this.data_Metamodel.AutoResizeColumns();
            //this.data_Metamodel.Columns["DefaultName"].Visible = false;
        }
        #endregion Infromatiosnaustausch

        #region Anforderungen
        private void anforderungenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.grid4_Type = 0;
            this.recent = data_grid4;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel_base.combo_Konnektoren_Afo;
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = metamodel_base.m_Connectoren_Req7;
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = null;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
          //  this.data_Metamodel.Columns["SubType"].Visible = true;
          //  this.data_Metamodel.Columns["XAC"].Visible = false;
            this.data_Metamodel.AutoResizeColumns();

            //this.data_Metamodel.Columns["DefaultName"].Visible = false;
        }
        #endregion Anforderungen

        #region Taxonomy Connectoren
        private void taxonomyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grid4_Type = 2;
            this.recent = data_grid4;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel_base.combo_Taxonomy;
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = metamodel_base.m_Connectoren_Req7;
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = null;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.AutoResizeColumns();
           // this.data_Metamodel.Columns["SubType"].Visible = true;
           // this.data_Metamodel.Columns["XAC"].Visible = false;
        }
        #endregion Taxonomy Connectoren

        #region Verhalten
        private void verhaltenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.recent = data_Metamodel;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_Szenar.Text = "Konnektor Verhaltenszuweisung";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = true;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = false;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            combo_XAC_Attribut.Visible = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = metamodel.m_Behavior;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.Columns["SubType"].Visible = true;
            this.data_Metamodel.Columns["XAC"].Visible = false;
            this.data_Metamodel.AutoResizeColumns();
        }
        #endregion

        #region Stakeholderzuweisung
        private void stakholderzuweisungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.recent = data_Metamodel;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_Szenar.Text = "Konnektor Stakeholderzuweisung";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = true;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = false;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            combo_XAC_Attribut.Visible = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = metamodel.m_Stakeholder;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.Columns["SubType"].Visible = true;
            this.data_Metamodel.Columns["XAC"].Visible = false;
            this.data_Metamodel.AutoResizeColumns();
        }
        #endregion Stakeholderzuweisung

        #region Constraints

        private void constraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grid4_Type = 5;
            this.recent = data_grid4;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel_base.combo_Satisfy;
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = metamodel_base.m_Connectoren_Req7;
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = this.metamodel.m_Satisfy_Design;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.Columns["SubType"].Visible = true;
            this.data_Metamodel.Columns["XAC"].Visible = false;
            this.data_Metamodel.AutoResizeColumns();
        }

        #endregion Constraints

        #region BPMN
        private void bPMNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grid4_Type = 7;
            this.recent = data_grid4;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel_base.combo_BPMN_Connectoren;
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = metamodel_base.m_Connectoren_Req7;
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = null;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.AutoResizeColumns();
        //    this.data_Metamodel.Columns["SubType"].Visible = false;
        //    this.data_Metamodel.Columns["XAC"].Visible = false;
        }
        #endregion BPMN
        #endregion Konnektoren

        #region Buttons
        //Zeile löschen
        private void button_Delete_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection var = recent.SelectedRows;

            if(var.Count > 0)
            {
                int i1 = 0;
                do
                {
                    recent.Rows.RemoveAt(var[i1].Index);

                    i1++;
                } while (i1 < var.Count);

                recent.Refresh();
            }
        }
        //Zeile hinzufügen
        private void button_Add_Click(object sender, EventArgs e)
        {
            if (comboBox_Type.Text != null && comboBox_Toolbox.Text != null && recent != null)
            {
                //Element_Metamodel
                switch (flag_type)
                {
                    case 0:
                        if (TextBox_DefaultName.Text != "")
                        {
                            List<Element_Metamodel> m_element_metamodel = (this.data_Metamodel.DataSource as BindingSource).DataSource as List<Element_Metamodel>;
                            m_element_metamodel.Add(new Element_Metamodel(comboBox_Type.Text, comboBox_Stereotype.Text, comboBox_Toolbox.Text, TextBox_DefaultName.Text, null));
                            BindingSource bindingSource_Type_Grid = new BindingSource();
                            bindingSource_Type_Grid.DataSource = m_element_metamodel;
                            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
                            this.data_Metamodel.AutoResizeColumns();
                        }
                        else
                        {
                            MessageBox.Show("Es wird ein Default Name benötigt.");
                        }
                        break;
                    case 1:
                        List<Element_Metamodel> m_element_metamodel_2 = (recent.DataSource as BindingSource).DataSource as List<Element_Metamodel>;
                        m_element_metamodel_2.Add(new Element_Metamodel(comboBox_Type.Text, comboBox_Stereotype.Text, comboBox_Toolbox.Text, null, combo_XAC_Attribut.Text));
                        BindingSource bindingSource_Type_Grid2 = new BindingSource();
                        bindingSource_Type_Grid2.DataSource = m_element_metamodel_2;
                        recent.DataSource = (bindingSource_Type_Grid2);
                        recent.AutoResizeColumns();
                        break;
                    case 2:
                        List<Element_Metamodel> m_element_metamodel3 = (this.recent.DataSource as BindingSource).DataSource as List<Element_Metamodel>;
                        m_element_metamodel3.Add(new Element_Metamodel(comboBox_Type.Text, comboBox_Stereotype.Text, comboBox_Toolbox.Text, null, null));
                        BindingSource bindingSource_Type_Grid3 = new BindingSource();
                        bindingSource_Type_Grid3.DataSource = m_element_metamodel3;
                        this.recent.DataSource = (bindingSource_Type_Grid3);
                        this.recent.AutoResizeColumns();
                        break;
                    case 3:
                        List<Connector_Metamodel> m_element_metamodel4 = (this.recent.DataSource as BindingSource).DataSource as List<Connector_Metamodel>;
                        string SubType = "";
                        if (comboBox_Type.Text == "Aggregation")
                        {
                            SubType = "Strong";
                        }
                        else
                        {
                            SubType = null;
                        }

                        m_element_metamodel4.Add(new Connector_Metamodel(comboBox_Type.Text, comboBox_Stereotype.Text, comboBox_Toolbox.Text, SubType, combo_XAC_Attribut.Text, true));
                        BindingSource bindingSource_Type_Grid4 = new BindingSource();
                        bindingSource_Type_Grid4.DataSource = m_element_metamodel4;
                        this.recent.DataSource = (bindingSource_Type_Grid4);
                        this.recent.AutoResizeColumns();
                        break;
                }

                data_Metamodel.Refresh();
            }

            }
        //Aktuelles Metamodell speichern
        private void button_Save_Click(object sender, EventArgs e)
        {
            List<bool> m_flag_check = new List<bool>();
            //Überprüfung, ob alle Tabellen richtig befüllt
            #region Check Einträge
            m_flag_check.Add(Check_Szenar());
            m_flag_check.Add(Check_Decomposition());
            m_flag_check.Add(Check_Anforderungen());
            m_flag_check.Add(Check_InformationItem());
            m_flag_check.Add(Check_Aktivity());
            m_flag_check.Add(Check_Stakeholder());
            m_flag_check.Add(Check_Fähigkeitsbaum());
            m_flag_check.Add(Check_Constraint());
            m_flag_check.Add(Check_Typvertreter_Element());
            m_flag_check.Add(Check_BPMN_Element());



            m_flag_check.Add(Check_Derived());
            m_flag_check.Add(Check_Relation_Anforderungen());
            m_flag_check.Add(Check_Informationsaustausch());
            m_flag_check.Add(Check_Verhaltenszuweisung());
            m_flag_check.Add(Check_Taxonomy());
            m_flag_check.Add(Check_Stakeholderzuweisung());
            m_flag_check.Add(Check_Satisfy());
            m_flag_check.Add(Check_Typvertreter());
            m_flag_check.Add(Check_BPMN_Con());

            #endregion Check Einträge

            if (m_flag_check.Contains(false))
            {
                //Es wird nichts gemacht
            }
            else
            {
                //Speicherort wählen mit Namen
                //xml anlegen
                XML_Handler_Profil xml_hp = new XML_Handler_Profil(this.metamodel);
                xml_hp.Export_Profil();

               
            }
        }


        #endregion Buttons



        #region Grid 1 Hilfsfunc

        #endregion Grid1

        #region Grid 2Hilfsfunc
        private void data_metamodel_left_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (create == false)
            {
                data_Metamodel_right.CurrentCell = null;
                recent = this.data_metamodel_left;
            }

        }

        private void data_Metamodel_right_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (create == false)
            {
                data_metamodel_left.CurrentCell = null;
                recent = this.data_Metamodel_right;
            }
        }
        #endregion Hilfsfunc

        #region Grid 3Hilfsfunc
        private void data_3_left_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (create == false)
            {
                data_3_middle.CurrentCell = null;
                data_3_right.CurrentCell = null;
                recent = this.data_3_left;
            }
        }

        private void data_3_middle_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (create == false)
            {
                data_3_left.CurrentCell = null;
                data_3_right.CurrentCell = null;
                recent = this.data_3_middle;
            }
        }

        private void data_3_right_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (create == false)
            {
                data_3_middle.CurrentCell = null;
                data_3_left.CurrentCell = null;
                recent = this.data_3_right;
            }
        }




        #endregion Hilfsfunc

        #region Hilfsfunc Grid4
        private void combo_Grid4_Auswahl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Anforderungen Konnektoren
            if(grid4_Type == 0)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();

                switch (combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Konnektoren_Afo[0];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Afo_Refines;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 1:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Konnektoren_Afo[1];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Afo_Requires;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 2:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Konnektoren_Afo[2];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Afo_Replaces;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 3:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Konnektoren_Afo[3];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Afo_Dublette;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 4:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Konnektoren_Afo[4];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Afo_Konflikt;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                }

                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["SubType"].Visible = false;
                    this.recent.Columns["XAC"].Visible = true;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }
            //Fähigkeitsbaum Elemente
            if(grid4_Type == 1)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();
                switch(combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Taxonomy[0];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Capability;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                        break;
                }


                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["DefaultName"].Visible = false;
                    this.recent.Columns["XAC_Attribut"].Visible = false;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }
            //Fähigkeitsbaun Taxonomy
            if (grid4_Type == 2)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();
                switch (combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Taxonomy[0];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Taxonomy_Capability;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 1:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Taxonomy[1];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Decomposition_Element;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                        break;
                }


                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["SubType"].Visible = false;
                    this.recent.Columns["XAC"].Visible = true;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }
            //Anforderungs Auswahl
            if(grid4_Type == 3)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();
                switch (combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[0].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_Functional;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 1:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[1].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_Interface;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 2:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[2].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_User;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 3:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[3].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_Design;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 4:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[4].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_Process;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 5:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[5].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_Environment;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 6:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[6].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_Typvertreter;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 7:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[7].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_Quality_Class;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 8:
                        this.label_grid4_rigth.Text = metamodel.m_Requirement_Name[8].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Requirement_NonFunctional;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                }


                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["DefaultName"].Visible = false;
                    this.recent.Columns["XAC_Attribut"].Visible = true;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }
            //Constraints
            if (grid4_Type == 4)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();
                switch (combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = metamodel.m_Op_Constraint_Name[0].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Design_Constraint;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 1:
                        this.label_grid4_rigth.Text = metamodel.m_Op_Constraint_Name[1].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Process_Constraint;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 2:
                        this.label_grid4_rigth.Text = metamodel.m_Op_Constraint_Name[2].Stereotype;
                        bindingSource_Type_Grid.DataSource = metamodel.m_Constraint_Umwelt;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    /*  case 2:
                          this.label_grid4_rigth.Text = metamodel.m_Op_Constraint_Name[2].Stereotype;
                          bindingSource_Type_Grid.DataSource = metamodel.m_Requirement[2];
                          this.recent.DataSource = (bindingSource_Type_Grid);
                          break;
                      case 3:
                          this.label_grid4_rigth.Text = metamodel.m_Op_Constraint_Name[3].Stereotype;
                          bindingSource_Type_Grid.DataSource = metamodel.m_Requirement[3];
                          this.recent.DataSource = (bindingSource_Type_Grid);
                          break;
                     */
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                }


                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["DefaultName"].Visible = false;
                    this.recent.Columns["XAC_Attribut"].Visible = false;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }
            //Satisfy Constaints
            if (grid4_Type == 5)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();

                switch (combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Satisfy[0];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Satisfy_Design;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 1:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Satisfy[1];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Satisfy_Process;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 2:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Satisfy[2];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Satisfy_Umwelt;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    /*    case 2:
                            this.label_grid4_rigth.Text = this.metamodel_base.combo_Konnektoren_Afo[2];
                            bindingSource_Type_Grid.DataSource = metamodel.m_Afo_Replaces;
                            this.recent.DataSource = (bindingSource_Type_Grid);
                            break;
                        case 3:
                            this.label_grid4_rigth.Text = this.metamodel_base.combo_Konnektoren_Afo[3];
                            bindingSource_Type_Grid.DataSource = metamodel.m_Afo_Dublette;
                            this.recent.DataSource = (bindingSource_Type_Grid);
                            break;
                        case 4:
                            this.label_grid4_rigth.Text = this.metamodel_base.combo_Konnektoren_Afo[4];
                            bindingSource_Type_Grid.DataSource = metamodel.m_Afo_Konflikt;
                            this.recent.DataSource = (bindingSource_Type_Grid);
                            break;
                     */
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                }

                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["SubType"].Visible = false;
                    this.recent.Columns["XAC"].Visible = false;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }

            //BPMN-Elemente
            if (grid4_Type == 6)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();
                switch (combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = "Pool";
                        bindingSource_Type_Grid.DataSource = metamodel.m_Pools_BPMN;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 1:
                        this.label_grid4_rigth.Text = "Lane";
                        bindingSource_Type_Grid.DataSource = metamodel.m_Lanes_BPMN;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 2:
                        this.label_grid4_rigth.Text = "Activity";
                        bindingSource_Type_Grid.DataSource = metamodel.m_Aktivity_Definition_BPMN;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                }


                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["DefaultName"].Visible = false;
                    this.recent.Columns["XAC_Attribut"].Visible = false;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }

            //BPMN-Konnektoren
            if (grid4_Type == 7)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();
                switch (combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_BPMN_Connectoren[0];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Con_Pools_BPMN;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                  /*  case 1:
                        this.label_grid4_rigth.Text = "Lane";
                        bindingSource_Type_Grid.DataSource = metamodel.m_Lanes_BPMN;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                    case 2:
                        this.label_grid4_rigth.Text = "Activity";
                        bindingSource_Type_Grid.DataSource = metamodel.m_Aktivity_Definition_BPMN;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;*/
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                }


                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["SubType"].Visible = false;
                    this.recent.Columns["XAC"].Visible = true;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }
            //Komposition-Konnektoren
         /*   if (grid4_Type == 8)
            {
                BindingSource bindingSource_Type_Grid = new BindingSource();
                switch (combo_Grid4_Auswahl.SelectedIndex)
                {
                    case 0:
                        this.label_grid4_rigth.Text = this.metamodel_base.combo_Taxonomy[0];
                        bindingSource_Type_Grid.DataSource = metamodel.m_Decomposition_Element;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                   
                    default:
                        this.label_grid4_rigth.Text = "";
                        bindingSource_Type_Grid.DataSource = null;
                        this.recent.DataSource = (bindingSource_Type_Grid);
                        break;
                }


                if (this.recent.DataSource != null)
                {
                    this.recent.Columns["SubType"].Visible = false;
                    this.recent.Columns["XAC"].Visible = true;
                    this.recent.AutoResizeColumns();
                    this.recent.Refresh();
                }
            }
        */
        }



        #endregion Hilfsfunc Grid4

        #region Hilfsfunc Button

        #region Check korrekt ausgefüllt
        #region Reiter Elemente
        private bool Check_Szenar()
        {
            bool ret = true;

            if(this.metamodel.m_Szenar.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss mindestens ein Szenarelement angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            return (ret);
        }
        private bool Check_Decomposition()
        {
            bool ret = true;

            if (this.metamodel.m_Elements_Definition.Count < 1 || this.metamodel.m_Elements_Usage.Count < 1)
            {
                ret = false;
                MessageBox.Show("In der Decomposition muss mindestens ein Element für die Definition und Usage angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            if (this.metamodel.m_Elements_Definition.Count != this.metamodel.m_Elements_Usage.Count)
            {
                ret = false;
                MessageBox.Show("In der Decomposition muss die Anzahl der Definitionen genau der Anzahl der Usage entsprechen.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            return (ret);
        }
        private bool Check_Anforderungen()
        {
            bool ret = true;

            if(this.metamodel.m_Requirement_Functional.Count < 1 || this.metamodel.m_Requirement_Interface.Count < 1 || this.metamodel.m_Requirement_User.Count < 1 || this.metamodel.m_Requirement_Design.Count < 1 || this.metamodel.m_Requirement_Process.Count < 1 || this.metamodel.m_Requirement_Environment.Count < 1 || this.metamodel.m_Requirement_Typvertreter.Count < 1 || this.metamodel.m_Requirement_Quality_Class.Count < 1 || this.metamodel.m_Requirement_Quality_Activity.Count < 1 || this.metamodel.m_Requirement_NonFunctional.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für jeden Anforderungstyp mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }
        private bool Check_InformationItem()
        {
            bool ret = true;

            if (this.metamodel.m_InformationItem.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für die InformationItem mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }
        private bool Check_Aktivity()
        {
            bool ret = true;

            if (this.metamodel.m_Aktivity_Definition.Count < 1 || this.metamodel.m_Aktivity_Usage.Count < 1)
            {
                ret = false;
                MessageBox.Show("Bei dem Reiter Aktivity muss mindestens ein Eintrag für die Definition und Usage angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            if (this.metamodel.m_Elements_Definition.Count != this.metamodel.m_Elements_Usage.Count)
            {
                ret = false;
                MessageBox.Show("Bei dem Reiter Aktivity muss die Anzahl der Definitionen genau der Anzahl der Usage entsprechen.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            return (ret);
        }
        private bool Check_Stakeholder()
        {
            bool ret = true;

            if (this.metamodel.m_Szenar.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss mindestens ein Stakeholder angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            return (ret);
        }
        private bool Check_Fähigkeitsbaum()
        {
            bool ret = true;

            if (this.metamodel.m_Capability.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss mindestens ein Eintrag für den Fähigkeitsbaum angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            return (ret);
        }
        private bool Check_Constraint()
        {
            bool ret = true;

            if (this.metamodel.m_Design_Constraint.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für die DesignConstraint mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            if (this.metamodel.m_Process_Constraint.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für die ProcessConstraint mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            if (this.metamodel.m_Constraint_Umwelt.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für die UmweltConstraint mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }
        private bool Check_Typvertreter_Element()
        {
            bool ret = true;

            if (this.metamodel.m_Typvertreter_Definition.Count < 1 || this.metamodel.m_Typvertreter_Usage.Count < 1)
            {
                ret = false;
                MessageBox.Show("Bei den Typvertetern muss mindestens ein Element für die Definition und Usage angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            if (this.metamodel.m_Typvertreter_Definition.Count != this.metamodel.m_Typvertreter_Usage.Count)
            {
                ret = false;
                MessageBox.Show("Bei den Typvertetern muss die Anzahl der Definitionen genau der Anzahl der Usage entsprechen.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            return (ret);
        }

        private bool Check_BPMN_Element()
        {
            bool ret = true;

            if (this.metamodel.m_Aktivity_Definition_BPMN.Count < 1)
            {
                ret = false;
                MessageBox.Show("Bei den Activity der BPMN muss mindestens ein Element für die Definition angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            if (this.metamodel.m_Lanes_BPMN.Count < 1)
            {
                ret = false;
                MessageBox.Show("Bei den Lanes der BPMN muss mindestens ein Element für die Definition angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            if (this.metamodel.m_Pools_BPMN.Count < 1)
            {
                ret = false;
                MessageBox.Show("Bei den Pools der BPMN muss mindestens ein Element für die Definition angelegt werden.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            return (ret);
        }

        #endregion Reiter Elemente
        #region Reiter Konnektoren
        private bool Check_Derived()
        {
            bool ret = true;

            if (this.metamodel.m_Derived_Capability.Count < 1 || this.metamodel.m_Derived_Element.Count < 1 || this.metamodel.m_Derived_Logical.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für jeden Konnektor der Ableitung mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

           /* if(ret == true)
            {
                int i1 = 0;
                do
                {
                    if(this.metamodel.m_Derived_Capability[i1].Type

                    i1++;
                } while (i1 < this.metamodel.m_Derived_Capability.Count);
            }*/

            return (ret);
        }
        private bool Check_Relation_Anforderungen()
        {
            bool ret = true;

            if (this.metamodel.m_Afo_Dublette.Count < 1 || this.metamodel.m_Afo_Konflikt.Count < 1 || this.metamodel.m_Afo_Refines.Count < 1 || this.metamodel.m_Afo_Replaces.Count < 1 || this.metamodel.m_Afo_Requires.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für jede Relation der Anforderungen mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            } 

            return (ret);
        }
        private bool Check_Informationsaustausch()
        {
            bool ret = true;

            if (this.metamodel.m_Infoaus.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für den Konnektor des Informationsaustausch mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }
        private bool Check_Verhaltenszuweisung()
        {
            bool ret = true;

            if (this.metamodel.m_Behavior.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für den Konnektor der Verhaltenszuweisung mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }
        private bool Check_Taxonomy()
        {
            bool ret = true;

            if (this.metamodel.m_Taxonomy_Capability.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für den Taxonomy-Konnektor des Fähigkeitsbaums mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            if(this.metamodel.m_Decomposition_Element.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für den Taxonomy-Konnektor der Dekomposition mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");
            }

            return (ret);
        }
        private bool Check_Stakeholderzuweisung()
        {
            bool ret = true;

            if (this.metamodel.m_Stakeholder.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für den Konnektor der Stakeholderzuweisung mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }
        private bool Check_Satisfy()
        {
            bool ret = true;

            if (this.metamodel.m_Satisfy_Design.Count < 1 || this.metamodel.m_Satisfy_Process.Count < 1 || this.metamodel.m_Satisfy_Umwelt.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für jeden Konnektor der Satisfy Constraints mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }

        private bool Check_Typvertreter()
        {
            bool ret = true;

            if (this.metamodel.m_Con_Typvertreter.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für jeden Konnektor der Typvertreter mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }

        private bool Check_BPMN_Con()
        {
            bool ret = true;

            if (this.metamodel.m_Con_Pools_BPMN.Count < 1)
            {
                ret = false;
                MessageBox.Show("Es muss für jeden Konnektor der Pool Repräsentation mindestens ein Eintrag vorliegen.\nDas aktuelle Metamodell wird nicht gespeichert.");

            }

            return (ret);
        }
        #endregion Konnektoren

        #endregion korrekt ausgefüllt

        #endregion Hilfsfunc Button

        private void tag_Connectoren_Click(object sender, EventArgs e)
        {

        }

        private void typvertreterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.recent = data_Metamodel;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_Szenar.Text = "Konnektor Typvertreter";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = true;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = false;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            combo_XAC_Attribut.Visible = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = metamodel.m_Con_Typvertreter;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.Columns["SubType"].Visible = true;
            this.data_Metamodel.Columns["XAC"].Visible = false;
            this.data_Metamodel.AutoResizeColumns();
        }

        private void typverteterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Label
            this.label_DecDef.Text = "Typvertreter Definition";
            this.label_DecUsage.Text = "Typvertreter Usage";
            #endregion  Label
            this.flag_type = 1;
            this.create = true;
            #region Visibility
            //Visibile tauschen
            //Grid1

            //splitContainer_Main.Visible = false;
            // data_Metamodel.Visible = false;
            panel_Grid1.Visible = false;
            panel_Grid1.Enabled = false;
            //Grid2
            panel_Grid2.Visible = true;
            panel_Grid2.Enabled = true;
            TextBox_DefaultName.Enabled = false;
            TextBox_DefaultName.Visible = false;
            combo_XAC_Attribut.Visible = true;
            combo_XAC_Attribut.Enabled = true;
            combo_XAC_Attribut.Visible = false;
            comboBox_Type.Visible = true;
            comboBox_Stereotype.Visible = true;
            comboBox_Toolbox.Visible = true;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = false;
            panel_Grid4.Enabled = false;
            #endregion Visibility
            //Auswahl
            #region Databinding Comboboxen
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Elemente;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XAC_Attribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = this.metamodel_base.sys_Enum.SYS_KOMPONENTENTYP.ToList();
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            this.combo_XAC_Attribut.Visible = true;
            this.combo_XAC_Attribut.Enabled = true;
            #endregion Databinding Comboboxen
            //Tabellen
            #region Databinding Tabellen
            //DataGrid_left
            BindingSource bindingSource_Type_Grid_left = new BindingSource();
            bindingSource_Type_Grid_left.DataSource = metamodel.m_Typvertreter_Definition;
            this.data_metamodel_left.DataSource = (bindingSource_Type_Grid_left);
            this.data_metamodel_left.Columns["DefaultName"].Visible = false;
            this.data_metamodel_left.Columns["XAC_Attribut"].Visible = true;
            this.data_metamodel_left.CurrentCell = null;
            this.data_metamodel_left.AutoResizeColumns();
            //DataGrid_rigth
            BindingSource bindingSource_Type_Grid_rigth = new BindingSource();
            bindingSource_Type_Grid_rigth.DataSource = metamodel.m_Typvertreter_Usage;
            this.data_Metamodel_right.DataSource = (bindingSource_Type_Grid_rigth);
            this.data_Metamodel_right.Columns["DefaultName"].Visible = false;
            this.data_Metamodel_right.Columns["XAC_Attribut"].Visible = true;
            this.data_Metamodel_right.CurrentCell = null;
            this.data_Metamodel_right.AutoResizeColumns();
            #endregion Databinding Tabellen

            this.create = false;
        }
        #region Komppostion
      /*  private void kompositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grid4_Type = 8;
            this.recent = data_grid4;
            this.flag_type = 3;
            //Visibile tauschen
            #region Label
            this.label_grid4_left.Text = "Auswahl";
            this.label_grid4_rigth.Text = "";
            #endregion LAbel
            #region Visibility
            //Grid1
            //splitContainer_Main_Second.Visible = false;
            //data_Metamodel.Visible = true;
            //label_Szenar.Visible = true;
            panel_Grid1.Visible = false;
            TextBox_DefaultName.Enabled = false;
            combo_XAC_Attribut.Visible = true;
            TextBox_DefaultName.Visible = false;
            //Grid2
            panel_Grid2.Visible = false;
            panel_Grid2.Enabled = false;
            //Grid3
            panel_Grid3.Visible = false;
            panel_Grid3.Enabled = false;
            //Grid4
            panel_Grid4.Visible = true;
            panel_Grid4.Enabled = true;
            #endregion Visibility
            //////////////////////////////////////////////////////////////////
            //Auswahl
            BindingSource bindingSource_Auswahl = new BindingSource();
            bindingSource_Auswahl.DataSource = metamodel_base.combo_Taxonomy;
            this.combo_Grid4_Auswahl.DataSource = (bindingSource_Auswahl);
            //Type
            BindingSource bindingSource_Type = new BindingSource();
            bindingSource_Type.DataSource = metamodel_base.m_Type_Konnektoren;
            this.comboBox_Type.DataSource = (bindingSource_Type);
            //Toolbox
            BindingSource bindingSource_Toolbox = new BindingSource();
            bindingSource_Toolbox.DataSource = metamodel_base.m_Toolbox;
            this.comboBox_Toolbox.DataSource = (bindingSource_Toolbox);
            //XACAttribut
            BindingSource bindingSource_XAC = new BindingSource();
            bindingSource_XAC.DataSource = metamodel_base.m_Connectoren_Req7;
            this.combo_XAC_Attribut.DataSource = (bindingSource_XAC);
            ///////////////////////////////////////////////////////////////////
            //DataGrid
            BindingSource bindingSource_Type_Grid = new BindingSource();
            bindingSource_Type_Grid.DataSource = null;
            this.data_Metamodel.DataSource = (bindingSource_Type_Grid);
            this.data_Metamodel.AutoResizeColumns();
            // this.data_Metamodel.Columns["SubType"].Visible = true;
            // this.data_Metamodel.Columns["XAC"].Visible = false;
        }
      */
    }
      
    #endregion
}

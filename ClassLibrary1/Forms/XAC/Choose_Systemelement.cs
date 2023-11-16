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
    public partial class Choose_Systemelement : Form
    {
        public List<Repsoitory_Elements.NodeType> m_nodeTypes = new List<Repsoitory_Elements.NodeType>();
        public List<Repsoitory_Elements.SysElement> m_syselem = new List<Repsoitory_Elements.SysElement>();

        bool flag_ini = true;
        bool flag_checkall = true;
        int modus;

        Database Data;
        public Choose_Systemelement(Database database)
        {
            this.Data = database;
            InitializeComponent();

            flag_ini = true;

            //Tree befüllen
            this.treeView_Client.CheckBoxes = true;


            if(database.metamodel.modus == 0)
            {
                Create_Treeview(database.m_NodeType);
            }
            if(database.metamodel.modus == 1)
            {
                Create_Treeview_SysElem(database.m_SysElemente);
            }

            this.modus = database.metamodel.modus;
            flag_ini = false;
        }



        #region Treeview
        private void Create_Treeview(List<Repsoitory_Elements.NodeType> nodes)
        {
            this.treeView_Client.Nodes.Clear();

            if(nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(nodes[i1].m_Parent.Count == 0)
                    {
                        TreeNode recent = new TreeNode(nodes[i1].Name) { Tag = nodes[i1] };

                        this.treeView_Client.Nodes.Add(recent);

                        if(nodes[i1].m_Child.Count > 0)
                        {
                            Create_rekrusiv(recent, nodes[i1].m_Child);
                        }

                    }

                    i1++;
                } while (i1 < nodes.Count);

            }

            this.treeView_Client.Refresh();
        }
        private void Create_Treeview_SysElem(List<Repsoitory_Elements.SysElement> nodes)
        {
            this.treeView_Client.Nodes.Clear();

            if (nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (nodes[i1].m_Parent.Count == 0)
                    {
                        TreeNode recent = new TreeNode(nodes[i1].Name) { Tag = nodes[i1] };

                        this.treeView_Client.Nodes.Add(recent);

                        if (nodes[i1].m_Child.Count > 0)
                        {
                            Create_rekrusiv_SysElem(recent, nodes[i1].m_Child);
                        }

                    }

                    i1++;
                } while (i1 < nodes.Count);

            }

            this.treeView_Client.Refresh();
        }


        private void Create_rekrusiv(TreeNode recent, List<Repsoitory_Elements.NodeType> nodes)
        {
            if(nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    TreeNode child = new TreeNode(nodes[i1].Name) { Tag = nodes[i1] };

                    if (nodes[i1].m_Child.Count > 0)
                    {
                        Create_rekrusiv(child, nodes[i1].m_Child);
                    }

                    recent.Nodes.Add(child);

                    i1++;
                } while (i1 < nodes.Count);
            }

        }

        private void Create_rekrusiv_SysElem(TreeNode recent, List<Repsoitory_Elements.SysElement> nodes)
        {
            if (nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    TreeNode child = new TreeNode(nodes[i1].Name) { Tag = nodes[i1] };

                    if (nodes[i1].m_Child.Count > 0)
                    {
                        Create_rekrusiv_SysElem(child, nodes[i1].m_Child);
                    }

                    recent.Nodes.Add(child);

                    i1++;
                } while (i1 < nodes.Count);
            }

        }

        #endregion

        #region Botton
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button_check_Click(object sender, EventArgs e)
        {
            flag_ini = true;

            if (flag_checkall == false)
            {
                Set_Check(false);
                this.treeView_Client.Refresh();
                this.button_check.Text = "Check All";

                this.button_check.Refresh();

                flag_checkall = true;
            }
            else
            {
                Set_Check(true);
                this.treeView_Client.Refresh();
                this.button_check.Text = "UnCheck All";

                this.button_check.Refresh();

                flag_checkall = false;
            }

            flag_ini = false;


            switch(this.modus)
            {
                case 0:
                    List<Repsoitory_Elements.NodeType> nodes = this.Get_Checked();
                    m_nodeTypes = nodes;
                    break;
                case 1:
                    List<Repsoitory_Elements.SysElement> nodes1 = this.Get_Checked_SysElem();
                    m_syselem = nodes1;
                    break;
                default:
                    List<Repsoitory_Elements.NodeType> nodes2 = this.Get_Checked();
                    m_nodeTypes = nodes2;
                    break;
            }
            
        }
        #endregion



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #region Check
        private void treeView_Client_AfterCheck(object sender, TreeViewEventArgs e)
        {

           
            if(flag_ini == false)
            {
                if(e.Node.Nodes.Count> 0)
                {
                    flag_ini = true;
                    Set_checkes_rekrusiv(e.Node.Checked, e.Node);
                    flag_ini = false;
                }

                switch (this.modus)
                {
                    case 0:
                        List<Repsoitory_Elements.NodeType> nodes = this.Get_Checked();
                        m_nodeTypes = nodes;
                        break;
                    case 1:
                        List<Repsoitory_Elements.SysElement> nodes1 = this.Get_Checked_SysElem();
                        m_syselem = nodes1;
                        break;
                    default:
                        List<Repsoitory_Elements.NodeType> nodes2 = this.Get_Checked();
                        m_nodeTypes = nodes2;
                        break;
                }
            }
        }

        #region Get_Checked NodeType
        private List<Repsoitory_Elements.NodeType> Get_Checked()
        {
            List<Repsoitory_Elements.NodeType> m_ret = new List<Repsoitory_Elements.NodeType>();

            if(this.treeView_Client.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(this.treeView_Client.Nodes[i1].Checked == true)
                    {
                        Repsoitory_Elements.NodeType recent = (Repsoitory_Elements.NodeType) this.treeView_Client.Nodes[i1].Tag;

                        m_ret.Add(recent);
                    }

                    if (this.treeView_Client.Nodes[i1].Nodes.Count > 0)
                    {
                        List<Repsoitory_Elements.NodeType> m_help = Get_Checked_rekrusiv(this.treeView_Client.Nodes[i1]);

                        if (m_help != null)
                        {
                            m_ret.AddRange(m_help);
                        }
                    }


                    i1++;
                } while (i1 < this.treeView_Client.Nodes.Count);
            }


            if(m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);
        }

        private List<Repsoitory_Elements.NodeType> Get_Checked_rekrusiv(TreeNode treeNode)
        {
            List<Repsoitory_Elements.NodeType> m_ret = new List<Repsoitory_Elements.NodeType>();

            if (treeNode.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (treeNode.Nodes[i1].Checked == true)
                    {
                        Repsoitory_Elements.NodeType recent = (Repsoitory_Elements.NodeType) treeNode.Nodes[i1].Tag;

                        m_ret.Add(recent);
                    }

                    if (treeNode.Nodes[i1].Nodes.Count > 0)
                    {
                        List<Repsoitory_Elements.NodeType> m_help = Get_Checked_rekrusiv(treeNode.Nodes[i1]);

                        if(m_help != null)
                        {
                            m_ret.AddRange(m_help);
                        }
                    }


                    i1++;
                } while (i1 < treeNode.Nodes.Count);
            }



            if (m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);
        }
        #endregion

        #region Get_checked _SysEleme
        private List<Repsoitory_Elements.SysElement> Get_Checked_SysElem()
        {
            List<Repsoitory_Elements.SysElement> m_ret = new List<Repsoitory_Elements.SysElement>();

            if (this.treeView_Client.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (this.treeView_Client.Nodes[i1].Checked == true)
                    {
                        Repsoitory_Elements.SysElement recent = (Repsoitory_Elements.SysElement)this.treeView_Client.Nodes[i1].Tag;

                        m_ret.Add(recent);
                    }

                    if (this.treeView_Client.Nodes[i1].Nodes.Count > 0)
                    {
                        List<Repsoitory_Elements.SysElement> m_help = Get_Checked_rekrusiv_SysElem(this.treeView_Client.Nodes[i1]);

                        if (m_help != null)
                        {
                            m_ret.AddRange(m_help);
                        }
                    }


                    i1++;
                } while (i1 < this.treeView_Client.Nodes.Count);
            }


            if (m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);
        }

        private List<Repsoitory_Elements.SysElement> Get_Checked_rekrusiv_SysElem(TreeNode treeNode)
        {
            List<Repsoitory_Elements.SysElement> m_ret = new List<Repsoitory_Elements.SysElement>();

            if (treeNode.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (treeNode.Nodes[i1].Checked == true)
                    {
                        Repsoitory_Elements.SysElement recent = (Repsoitory_Elements.SysElement)treeNode.Nodes[i1].Tag;

                        m_ret.Add(recent);
                    }

                    if (treeNode.Nodes[i1].Nodes.Count > 0)
                    {
                        List<Repsoitory_Elements.SysElement> m_help = Get_Checked_rekrusiv_SysElem(treeNode.Nodes[i1]);

                        if (m_help != null)
                        {
                            m_ret.AddRange(m_help);
                        }
                    }


                    i1++;
                } while (i1 < treeNode.Nodes.Count);
            }



            if (m_ret.Count == 0)
            {
                m_ret = null;
            }

            return (m_ret);
        }
        #endregion

        private void Set_Check(bool flag)
        {
            if(this.treeView_Client.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    this.treeView_Client.Nodes[i1].Checked = flag;

                    if(this.treeView_Client.Nodes[i1].Nodes.Count > 0)
                    {
                        Set_checkes_rekrusiv(flag, this.treeView_Client.Nodes[i1]);
                    }


                    i1++;
                } while (i1 < this.treeView_Client.Nodes.Count);

            }
        }

        private void Set_checkes_rekrusiv(bool flag, TreeNode treeNode)
        {
            if (treeNode.Nodes.Count > 0)
            {
                int i1 = 0;
                do
                {
                    treeNode.Nodes[i1].Checked = flag;

                    if (treeNode.Nodes[i1].Nodes.Count > 0)
                    {
                        Set_checkes_rekrusiv(flag, treeNode.Nodes[i1]);
                    }


                    i1++;
                } while (i1 < treeNode.Nodes.Count);

            }
        }
            #endregion

       

        private void Choose_Systemelement_Load(object sender, EventArgs e)
        {

        }

        private void checkBox_modus_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}

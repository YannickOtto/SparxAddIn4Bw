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
    public partial class Choose_Interface_Modus : Form
    {
        Requirement_Plugin.Database Data;

        public Choose_Interface_Modus(Requirement_Plugin.Database Database)
        {
            this.Data = Database;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Data.flag_modus = false;

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Data.flag_modus = true;

            MessageBox.Show("Es werden nun die Interface Requirements nach dem Maximalprinzip angelegt bzw. erweitert.");

            this.Close();
        }

        private void Choose_Interface_Modus_Load(object sender, EventArgs e)
        {

        }
    }
}

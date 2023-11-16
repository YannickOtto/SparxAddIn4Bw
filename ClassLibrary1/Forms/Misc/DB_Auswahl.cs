using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Requirement_Plugin;

using Ennumerationen;

namespace Forms
{
    public partial class DB_Auswahl : Form
    {
        Database Database;
        EA.Repository repository;

        public DB_Auswahl(Database database, EA.Repository repository)
        {
            this.Database = database;
            this.repository = repository;

            InitializeComponent();
            Database.metamodel.dB_Type = DB_Type.MSDASQL;

            switch (database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    this.richText_Connection_String.Text = this.Database.oLEDB_Interface.Get_Connection_String();
                    break;
                case DB_Type.MSDASQL:
                    if (this.Database.oDBC_Interface == null)
                    {
                        this.Database.oDBC_Interface = new Database_Connection.ODBC_Interface(this.repository, this.Database.Base);
                    }
                    this.richText_Connection_String.Text = this.Database.oDBC_Interface.Get_Connection_String();
                    break;
            }
           
            this.richText_Connection_String.Refresh();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
           

            switch (Database.metamodel.dB_Type)
            {
                case DB_Type.ACCDB:
                    this.Database.oLEDB_Interface.Set_Connection_String(this.richText_Connection_String.Text);
                    break;
                case DB_Type.MSDASQL:


                    this.Database.oDBC_Interface.Set_Connection_String(this.richText_Connection_String.Text);
                    this.Database.oDBC_Interface.odbc_Open();

                    break;
            }

        /*    if(this.Database.oLEDB_Interface.dbConnection != null)
            {
                if (this.Database.oLEDB_Interface.dbConnection.State == System.Data.ConnectionState.Open)
                {
                    this.Database.oLEDB_Interface.dbConnection.Close();
                }
            }
         */

           

            //this.Database.oLEDB_Interface.


        }

        private void DB_Auswahl_Load(object sender, EventArgs e)
        {

        }
    }
}

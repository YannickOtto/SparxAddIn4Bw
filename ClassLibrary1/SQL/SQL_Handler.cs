using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Requirement_Plugin
{
    public class SQL_Handler
    {
        public SQL_Handler(string Connection_string)
        {
            string cn = Properties.Settings.Default.RPI_DatabaseConnectionString;

            SqlConnection sql_con = new SqlConnection(cn);

            string SQL_Text = "SELECT * FROM t_object";

            SqlDataAdapter sql_adp = new SqlDataAdapter(SQL_Text, sql_con);

            DataTable data = new DataTable();

            sql_adp.Fill(data);

            DataTableReader reader = data.CreateDataReader();
            

            MessageBox.Show(reader.GetString(0));
        }

    }
}

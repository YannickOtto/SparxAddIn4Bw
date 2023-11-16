using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data;

namespace Database_Connection
{
    public class DB_Insert
    {
        public string Property { get; set; }
        public OleDbType oleDB_Type { get; set; }
        public OdbcType odbc_Type { get; set; }
        public object Value_str { get; set; }
        public object Default_str { get; set; }
        public int Value_int { get; set; }
        public int Default_int { get; set; }

        public DB_Insert(string Property, OleDbType oleDB_Type, OdbcType odbc_Type, object Value_str, int Value_int)
        {
            this.Property = Property;
            this.oleDB_Type = oleDB_Type;
            this.odbc_Type = odbc_Type;


            if (oleDB_Type == OleDbType.BigInt || odbc_Type == OdbcType.Int)
            {
                this.Value_int = Value_int;
            }
            else
            {
            
                this.Value_str = Value_str;
            }
        }

    }
}

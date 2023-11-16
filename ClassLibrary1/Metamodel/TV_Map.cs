using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Drawing;

namespace Metamodels
{
    public class TV_Map
    {
        public string Name { get; set; }
        public OleDbType oleDB_Type { get; set; }
        public OdbcType odbc_Type { get; set; }
        public string Map_Name { get; set; }
        public string Default_Value { get; set; }

        public TV_Map(string Name, OleDbType Type, object Map_Name, string default_value)
        {
            this.Name = Name;
            this.oleDB_Type = Type;

            switch(this.oleDB_Type)
            {
                case OleDbType.VarChar:
                    this.odbc_Type = OdbcType.VarChar;
                    break;
                case OleDbType.BigInt:
                    this.odbc_Type = OdbcType.Int;
                    break;
                default:
                    this.odbc_Type = OdbcType.VarChar;
                    break;
            }

            if(Map_Name != null)
            {
                this.Map_Name = (string)Map_Name;
            }
            else
            {
                this.Map_Name = null;
            }

            this.Default_Value = default_value;
            
        }
    }
}

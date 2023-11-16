using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Connection
{
    public class DB_Input
    {
        public object _str;
        public int _int;

        public DB_Input(int _int, object _str)
        {
            this._int = _int;
            this._str = _str;
        }

    }
}

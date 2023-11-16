using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Connection
{
   

    public class DB_Return
    {
        public List<object> Ret { get; set; }

        public DB_Return()
        {
            this.Ret = new List<object>();
        }
    }
}

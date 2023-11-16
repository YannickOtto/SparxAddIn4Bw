using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requirement_Plugin.Database_Connection.Repository
{
    public class Repository_ODBC
    {

        public string name;
        public string rootnode;
        public string connection_string;
        public string user;
        public string database;
        public string server;

        public Repository_ODBC(string new_name, string new_rootnode, string new_con_string, string user1, string database1, string server1)
        {
            this.name = new_name;
            this.rootnode = new_rootnode;
            this.connection_string = new_con_string;
            this.user = user1;
            this.database = database1;
            this.server = server1;
        }


    }
}

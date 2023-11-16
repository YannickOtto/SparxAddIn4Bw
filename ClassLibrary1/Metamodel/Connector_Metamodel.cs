using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metamodels
{
    public class Connector_Metamodel
    {
        public string Type { get; set; }
        public string Stereotype { get; set; }
        public string Toolbox { get; set; }
        public string SubType { get; set; }
        public string XAC { get; set; }

        public bool direction { get; set; }

        //Bool Direction
        //true --> Client Supplier
        //false --> Supplier client

        public Connector_Metamodel(string Type, string Stereotype, string Toolbox, string SubType, string XAC, bool direction)
        {
            this.Type = Type;
            this.Stereotype = Stereotype;
            this.Toolbox = Toolbox;
            this.SubType = SubType; //SubTpye Strong für Komposition ansonsten null
            this.XAC = XAC;
            this.direction = direction;
        }
    }
}

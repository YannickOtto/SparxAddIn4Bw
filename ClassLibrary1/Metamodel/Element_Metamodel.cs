using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metamodels
{
    public class Element_Metamodel
    {
        public string Type { get; set; }
        public string Stereotype { get; set; }
        public string Toolbox { get; set; }
        public string DefaultName { get; set; }
        public string XAC_Attribut { get; set; }

        public Element_Metamodel(string Type, string StereroType, string Toolbox, string DefaultName, string XAC_Attribut)
        {
            this.Type = Type;
            this.Stereotype = StereroType;
            this.Toolbox = Toolbox;
            this.DefaultName = DefaultName;
            this.XAC_Attribut = XAC_Attribut;
        }
    


    }
}

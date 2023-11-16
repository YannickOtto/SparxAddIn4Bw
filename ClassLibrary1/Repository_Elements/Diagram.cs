using Requirement_Plugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requirement_Plugin.Repository_Elements
{
    public class Diagram
    {
        public int Diagram_ID;
        public string Classifier;

        public Diagram (string Classifier, Database database)
        {
            this.Classifier = Classifier;

            this.Diagram_ID = this.Get_Diagram_ID(database);
        }

        ~Diagram()
        {

        }


        #region Get
        private int Get_Diagram_ID(Database database)
        {
            if (this.Classifier != null)
            {
                Interface_Element interface_Element = new Interface_Element();
                return (interface_Element.Get_One_Attribut_Integer(this.Classifier, database, "Diagram_ID", "t_diagram"));
            }
            else
            {
                return (-1);
            }
        }

        #endregion
    }
}

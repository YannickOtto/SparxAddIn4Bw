using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repsoitory_Elements
{
    public class InformationElement : Repository_Element
    {
        
        public List<Logical> Logicals;

        public InformationElement( string InformationItem_ID, Requirement_Plugin.Database database)
        {
            //MessageBox.Show("Test");
            this.Classifier_ID = InformationItem_ID;
            this.Logicals = new List<Logical>();

            this.Name = this.Get_Name(database);

        }

        /// <summary>
        /// Name des aktuellen InformationELement
        /// </summary>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public string Get_InformationItem_Name(EA.Repository Repository, Requirement_Plugin.Database database)
        {
            string Logical_Name = "kein";

            if (this.Classifier_ID != null)
            {
                Logical_Name = this.Get_Name( database);
            }

            return (Logical_Name);
        }
    }
}

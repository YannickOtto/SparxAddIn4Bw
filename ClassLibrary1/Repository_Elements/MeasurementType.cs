using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requirement_Plugin.Repository_Elements
{
    public class MeasurementType : Repsoitory_Elements.Repository_Class
    {
        public List<Measurement> m_Measurement;

        public MeasurementType(string guid, Database database)
        {
            if(guid != null)
            {
                this.Classifier_ID = guid;
                this.ID = this.Get_Object_ID(database);
                this.Name = this.Get_Name(database);
               
                this.m_Measurement = new List<Measurement>();
            }
          
        }

    }
}

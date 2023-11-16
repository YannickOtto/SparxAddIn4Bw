using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requirement_Plugin.Repository_Elements
{
    public class Measurement : Repsoitory_Elements.Repository_Element
    {
        public MeasurementType measurementType;
        string element_guid;
        public Measurement(string guid, Database database)
        {
            if(guid != null)
            {
              
                this.Classifier_ID = guid;
                this.ID = this.Get_Object_ID(database);
                this.Name = this.Get_Name(database);
                this.Notes = this.Get_Notes(database);

                string guid_mtype = this.Get_Classifier(database);

                if(guid_mtype != null)
                {
                    List<MeasurementType> m_type = database.m_MeasurementType.Where(x => x.Classifier_ID == guid_mtype).ToList();

                    if(m_type.Count > 0)
                    {
                        this.measurementType = m_type[0];
                    }
                    
                }




            }
        }
    }
}

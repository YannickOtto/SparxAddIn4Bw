using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requirement_Plugin.Diagram_Elements
{
    public class Diagram_Elements
    {
        int Diagram_ID;
        public List<int> m_Diagram_ID;

        public Diagram_Elements(int DiagramID)
        {
            this.m_Diagram_ID = new List<int>();
            this.Diagram_ID = DiagramID;

            m_Diagram_ID.Add(this.Diagram_ID);
        }

        #region Get_DiagramElements
        public List<string> Get_DiagramElements_guid(Database database, List<string> m_Type, List<string> m_Stereotype)
        {
            List<string> m_ret = new List<string>();

            Interfaces.Interface_Collection interface_Collection = new Interfaces.Interface_Collection();

            m_ret = interface_Collection.Get_DiagramElements_GUID(database, m_Type, m_Stereotype, this.Diagram_ID);

            if(m_ret != null)
            {
                if (m_ret.Count == 0)
                {
                    m_ret = null;
                }
            }
          

            return (m_ret);
        }

        public List<string> Get_DiagramElements_All(Database database)
        {
            List<string> m_ret = new List<string>();

            Interfaces.Interface_Collection interface_Collection = new Interfaces.Interface_Collection();

            string[] m_help = { "Class", "Part", "Port", "Object", "InformationItem", "Activity", "Action"};



            m_ret = interface_Collection.Get_DiagramElements_GUID_wo_Stereotype(database, m_help.ToList(), this.Diagram_ID);

            if (m_ret != null)
            {
                if (m_ret.Count == 0)
                {
                    m_ret = null;
                }
            }


            return (m_ret);
        }
        #endregion
    }
}

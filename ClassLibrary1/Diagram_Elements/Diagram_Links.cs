using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requirement_Plugin.Diagram_Elements
{
    public class Diagram_Links
    {
        int Diagram_ID;
        public List<int> m_Diagram_ID;

        public Diagram_Links(int DiagramID)
        {
            this.m_Diagram_ID = new List<int>();
            this.Diagram_ID = DiagramID;

            m_Diagram_ID.Add(this.Diagram_ID);
        }

        ~Diagram_Links()
        { 
        }
        public List<string> Get_DiagramLinks_All(Database database)
        {
            List<string> m_ret = new List<string>();

            Interfaces.Interface_Collection interface_Collection = new Interfaces.Interface_Collection();

            
            List<int> m_help2 = new List<int>();
            m_help2.Add(this.Diagram_ID);
            
            m_ret = interface_Collection.Get_DiagramLinks_ConveyedItems(database, m_help2);

            if (m_ret != null)
            {
                List<string> m_ret2 = new List<string>();
             

                if (m_ret.Count == 0)
                {
                    m_ret = null;
                }
                else
                {
                    Parallel.ForEach(m_ret, ret => 
                    {
                      string[] help =  ret.Split(',');
                        m_ret2.AddRange(help.ToList());
                      //if(help.)
                    });
                }

                m_ret2 = m_ret2.Distinct().ToList();
                m_ret = m_ret2;
            }


            return (m_ret);
        }
    }
}

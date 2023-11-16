using EA;
using Repsoitory_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requirement_Plugin.Repository_Elements
{
    public class Catalogue : Repsoitory_Elements.Capability
    {
        public Catalogue(string Classifier_ID, Repository Repository, Database Data) : base(Classifier_ID, Repository, Data)
        {
        }

        public List<Capability> GetCapabilities()
        {
            List<Capability> m_ret = new List<Capability>();

            if(this.m_Child.Count > 0)
            {
                m_ret.AddRange(this.m_Child);

                int i1 = 0;
                do
                {
                    List<Capability> m_help = this.m_Child[i1].GetCapabilities_rek();

                    if (m_help.Count > 0)
                    {
                        m_ret.AddRange(m_help);
                    }

                    i1++;
                } while (i1 < this.m_Child.Count);
            }

            return (m_ret);

        }
    
    }
}

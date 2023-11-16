using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Metamodels;
using Requirement_Plugin;
using Repsoitory_Elements;

namespace Diagrams
{
    public class Diagram_Element
    {
        private int Element_ID;
        public string Element_GUID = null;

        public Diagram_Element(int ID, Database Data)
        {
            this.Element_ID = ID;
            this.Get_Element_GUID(Data);
        }

        public Element_Metamodel GetElement_Metamodel(Database Data)
        {
            return (new Element_Metamodel(this.Get_Type(Data), this.Get_StereoType(Data), null, null, null));
        }

        private string Get_Type(Database Data)
        {
            Repository_Element repository_Element = new Repository_Element();

            if(Element_ID != null && Element_ID > 0)
            {
                repository_Element.Classifier_ID = repository_Element.Get_GUID_By_ID(this.Element_ID, Data);

                return (repository_Element.Get_Type(Data));
            }

            return (null);

        }

        private string Get_StereoType(Database Data)
        {
            Repository_Element repository_Element = new Repository_Element();

            if (Element_ID != null && Element_ID > 0)
            {
                repository_Element.Classifier_ID = repository_Element.Get_GUID_By_ID(this.Element_ID, Data);

                return (repository_Element.Get_Stereotype(Data));
            }

            return (null);

        }

        private void Get_Element_GUID(Database Data)
        {
            Repository_Element repository_Element = new Repository_Element();

            if (Element_ID != null && Element_ID > 0)
            {
                this.Element_GUID = repository_Element.Get_GUID_By_ID(this.Element_ID, Data);

            }

        }
    }
}

using Repsoitory_Elements;
using Requirement_Plugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Requirement_Plugin
{
    public class Package : Repository_Element, IEquatable<Package>, IComparable<Package>
    {
        public int Package_ID;

        Package parent_package;
        List<Package> m_child_packages;

        public Package(Database database, string Classifier)
        {
           this.m_child_packages = new List<Package>();
           this.parent_package = null;
           this.Classifier_ID = Classifier;
           this.Package_ID = this.Get_Package_ID_package(database);
           
        }

        ~Package()
        {

        }

        #region IEquatable
        public override bool Equals(object obj)
        {
            if (obj == null) return (false);
            Package recent = obj as Package;
            if (recent == null) return (false);
            else return (Equals(recent));
        }

        public bool Equals(Package other)
        {


            if (other == null) return (false);
            return (this.Name.Equals(other.Name));
        }
        #endregion
        #region IComparable
        public int CompareTo(Package compareClass)
        {
            if (compareClass == null) return (1);
            else
            {
                if (compareClass.Name == null || this.Name == null) return (1);
                else
                {
                    return (this.Name.CompareTo(compareClass.Name));
                }

            }
        }
        #endregion

        public int Get_Package_ID_package(Database database)
        {
            if (this.Classifier_ID != null)
            {
                Interface_Element interface_Element = new Interface_Element();
                return (interface_Element.Get_One_Attribut_Integer(this.Classifier_ID, database, "Package_ID", "t_package"));
            }
            else
            {
                return (-1);
            }
        }

        #region Parent
        public void Get_Parent(bool model, Database database)
        {
            if(model == true)
            {
                parent_package = null;
            }
            else
            {
                if (this.Package_ID != -1)
                {
                    Interface_Element interface_Element = new Interface_Element();
                    // int Parent_ID = interface_Element.Get_One_Attribut_Integer(this.Classifier_ID, database, "Parent_ID", "t_package");
                    string Parent_guid = interface_Element.Get_Parent_Package_GUID(this.Classifier_ID, database);

                    List<Package> m_help = database.m_Packages.Where(y => y.Classifier_ID == Parent_guid).ToList();

                    if (m_help.Count == 0)
                    {
                        Package parent = new Package(database, Parent_guid);
                        database.m_Packages.Add(parent);
                        this.parent_package = parent;
                    }
                    else
                    {
                        this.parent_package = m_help[0];
                        if (m_help[0].m_child_packages.Where(x => x.Classifier_ID == this.Classifier_ID).ToList().Count == 0)
                        {
                            m_help[0].m_child_packages.Add(this);
                        }
                    }
                }
                else
                {
                    parent_package = null;
                }
            }
        }
        #endregion

        #region Children
        public void Get_Children(Database database)
        {
            Interface_Element interface_Element = new Interface_Element();

            List<string> m_guid = interface_Element.Get_Children_Package(this.Classifier_ID, database);

            if(m_guid != null)
            {
                int i1 = 0;
                do
                {
                    List<Package> m_help = database.m_Packages.Where(x => x.Classifier_ID == m_guid[i1]).ToList();

                    if(m_help.Count == 0)
                    {
                        Package child = new Package(database, m_guid[i1]);
                        database.m_Packages.Add(child);
                        this.m_child_packages.Add(child);

                        child.Get_Children(database);
                    }
                    else
                    {
                        //dieser Fall darf nicht eintreten, ansonsten ist die DB fehlerhaft
                    }

                    i1++;
                } while (i1 < m_guid.Count);
            }
        }
        #endregion


    }

}

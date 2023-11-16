using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Repsoitory_Elements;

namespace Elements
{
    public class Element_Interface_Bidirectional : Element_Interface
    {
        //Hier ohne Target arbeiten, da Szenar übergreifend

        public bool Bidirectional = true;
        public List<Logical> m_Logical_Client;
        public List<Logical> m_Logical_Supplier;
        public List<InformationElement> m_InformationElement;

        public List<Target> m_Target_Client;
        public List<Target> m_Target_Supplier;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source_Classifier_ID"></param>
        /// <param name="Target_ID"></param>
        /// <param name="repository"></param>
        /// <param name="database"></param>
        public Element_Interface_Bidirectional(string Source_Classifier_ID, string Target_ID, EA.Repository repository, Requirement_Plugin.Database database) 
            : base (Source_Classifier_ID, Target_ID)
        {
                  this.m_Target_Client = new List<Target>();
                  this.m_Target_Supplier= new List<Target>();
            this.m_Target = new List<Target>();
        }

        #region Check Elements
        public List<Logical> Get_Logicals_Bidirektional(EA.Repository repository, Requirement_Plugin.Database Data)
        {
            List<Logical> m_Logical = new List<Logical>();

            if (this.m_Target_Client.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Logical recent_logical = this.m_Target_Client[i1].Get_Logical(this.m_Target_Client[i1].CLient_ID, this.m_Target_Client[i1].Supplier_ID, repository, Data);

                    if (recent_logical != null)
                    {
                        if (m_Logical.Contains(recent_logical) == false)
                        {
                            m_Logical.Add(recent_logical);
                        }

                    }

                    i1++;
                } while (i1 < this.m_Target_Client.Count);
            }

            if (this.m_Target_Supplier.Count > 0)
            {
                int i1 = 0;
                do
                {
                    Logical recent_logical = this.m_Target_Supplier[i1].Get_Logical(this.m_Target_Supplier[i1].CLient_ID, this.m_Target_Supplier[i1].Supplier_ID, repository, Data);

                    if (m_Logical.Contains(recent_logical) == false)
                    {
                        m_Logical.Add(recent_logical);
                    }

                    i1++;
                } while (i1 < this.m_Target_Supplier.Count);
            }

            if (m_Logical.Count > 0)
            {
                return (m_Logical);
            }
            else
            {
                return (null);
            }
        }
        #endregion

        #region Copy
        public Element_Interface_Bidirectional Copy_Interface_Bidirektional_Client(NodeType Target, EA.Repository repository, Requirement_Plugin.Database database)
        {
            Element_Interface_Bidirectional element_Interface = new Element_Interface_Bidirectional(Target.Classifier_ID, this.Target_Classifier_ID, repository, database);

            element_Interface.Supplier = this.Supplier;
            element_Interface.Client = Target;
            element_Interface.m_Logical_Supplier = this.m_Logical_Supplier;
            element_Interface.m_Logical_Client = this.m_Logical_Client;

            element_Interface.m_InformationElement = this.m_InformationElement;

            if (this.m_Target_Client.Count > 0)
            {
                #region Target_Client
                Target neu = new Target(Target.Classifier_ID, this.Supplier.Classifier_ID, database);

                element_Interface.m_Target_Client.Add(neu);

                int i1 = 0;
                do
                {
                    if (this.m_Target_Client[i1].m_Information_Element.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (neu.m_Information_Element.Contains(this.m_Target_Client[i1].m_Information_Element[i2]) == false)
                            {
                                neu.m_Information_Element.Add(this.m_Target_Client[i1].m_Information_Element[i2]);
                            }

                            i2++;
                        } while (i2 < this.m_Target_Client[i1].m_Information_Element.Count);
                    }

                    i1++;
                } while (i1 < this.m_Target_Client.Count);

                element_Interface.m_Target = element_Interface.m_Target_Client;
                #endregion

                #region Target Supplier
                Target neu2 = new Target(this.Supplier.Classifier_ID, Target.Classifier_ID, database);

                element_Interface.m_Target_Supplier.Add(neu2);

                i1 = 0;
                do
                {
                    if (this.m_Target_Supplier[i1].m_Information_Element.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            if (neu2.m_Information_Element.Contains(this.m_Target_Supplier[i1].m_Information_Element[i2]) == false)
                            {
                                neu2.m_Information_Element.Add(this.m_Target_Supplier[i1].m_Information_Element[i2]);
                            }

                            i2++;
                        } while (i2 < this.m_Target_Supplier[i1].m_Information_Element.Count);
                    }

                    i1++;
                } while (i1 < this.m_Target_Supplier.Count);
                #endregion

                List<List<Requirements.Requirement_Interface>> m_Requ_List = neu.Check_Requirement_Interface(repository, database, false, database.metamodel.m_Prozesswort_Interface[2]);
                List<List<Requirements.Requirement_Interface>> m_Requ_List2 = neu2.Check_Requirement_Interface(repository, database, false, database.metamodel.m_Prozesswort_Interface[2]);

                if (m_Requ_List != null)
                {
                    int j1 = 0;
                    do
                    {
                        if (element_Interface.m_Requirement_Interface_Send.Contains(m_Requ_List[0][j1]) == false)
                        {
                            element_Interface.m_Requirement_Interface_Send.Add(m_Requ_List[0][j1]);
                        }

                            j1++;
                    } while (j1 < m_Requ_List[0].Count);
                    int j2 = 0;
                    do
                    {
                        if (element_Interface.m_Requirement_Interface_Receive.Contains(m_Requ_List[1][j2]) == false)
                        {
                            element_Interface.m_Requirement_Interface_Receive.Add(m_Requ_List[1][j2]);
                        }

                        j2++;
                    } while (j2 < m_Requ_List[1].Count);

                   
                }
                if (m_Requ_List2 != null && m_Requ_List == null)
                {
                    int j1 = 0;
                    do
                    {
                        if (element_Interface.m_Requirement_Interface_Send.Contains(m_Requ_List2[0][j1]) == false)
                        {
                            element_Interface.m_Requirement_Interface_Send.Add(m_Requ_List2[0][j1]);
                        }

                        j1++;
                    } while (j1 < m_Requ_List2[0].Count);
                    int j2 = 0;
                    do
                    {
                        if (element_Interface.m_Requirement_Interface_Receive.Contains(m_Requ_List2[1][j2]) == false)
                        {
                            element_Interface.m_Requirement_Interface_Receive.Add(m_Requ_List2[1][j2]);
                        }

                        j2++;
                    } while (j2 < m_Requ_List2[1].Count);

                }
                /* if (element_Interface.m_Requirement_Interface_Send.Contains(Requ_List[0]) == false)
                    {
                        element_Interface.m_Requirement_Interface_Send.Add(Requ_List[0]);
                    }
                   */
                /*if (element_Interface.m_Requirement_Interface_Receive.Contains(Requ_List[1]) == false)
                {
                    element_Interface.m_Requirement_Interface_Receive.Add(Requ_List[1]);
                }*/
                /*    if (Requ_List == null && Requ_List2 != null)
                {
                    if (element_Interface.m_Requirement_Interface_Send.Contains(Requ_List2[1]) == false)
                    {
                        element_Interface.m_Requirement_Interface_Send.Add(Requ_List2[1]);
                    }
                    if (element_Interface.m_Requirement_Interface_Receive.Contains(Requ_List2[0]) == false)
                    {
                        element_Interface.m_Requirement_Interface_Receive.Add(Requ_List2[0]);
                    }
                }*/


            }



            return (element_Interface);

        }


        public Element_Interface_Bidirectional Copy_Interface_Bidirektional_Supplier(NodeType Target, EA.Repository repository, Requirement_Plugin.Database database)
        {
            Element_Interface_Bidirectional element_Interface = new Element_Interface_Bidirectional(this.Classifier_ID, Target.Classifier_ID, repository, database);
            element_Interface.Client = this.Client;
            element_Interface.Supplier = Target;
            element_Interface.m_Logical_Supplier = this.m_Logical_Supplier;
            element_Interface.m_Logical_Client = this.m_Logical_Client;

            if (this.m_Target.Count > 0)
            {
                if (this.m_Target_Client.Count > 0)
                {
                    #region Target_Client
                    Target neu = new Target(this.Client.Classifier_ID, Target.Classifier_ID, database);

                    element_Interface.m_Target_Client.Add(neu);

                    int i1 = 0;
                    do
                    {
                        if (this.m_Target_Client[i1].m_Information_Element.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (neu.m_Information_Element.Contains(this.m_Target_Client[i1].m_Information_Element[i2]) == false)
                                {
                                    neu.m_Information_Element.Add(this.m_Target_Client[i1].m_Information_Element[i2]);
                                }

                                i2++;
                            } while (i2 < this.m_Target_Client[i1].m_Information_Element.Count);
                        }

                        i1++;
                    } while (i1 < this.m_Target_Client.Count);

                    element_Interface.m_Target = element_Interface.m_Target_Client;
                    #endregion

                    #region Target Supplier
                    Target neu2 = new Target(Target.Classifier_ID, this.Client.Classifier_ID, database);

                    element_Interface.m_Target_Supplier.Add(neu2);

                    i1 = 0;
                    do
                    {
                        if (this.m_Target_Supplier[i1].m_Information_Element.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if (neu2.m_Information_Element.Contains(this.m_Target_Supplier[i1].m_Information_Element[i2]) == false)
                                {
                                    neu2.m_Information_Element.Add(this.m_Target_Supplier[i1].m_Information_Element[i2]);
                                }

                                i2++;
                            } while (i2 < this.m_Target_Supplier[i1].m_Information_Element.Count);
                        }

                        i1++;
                    } while (i1 < this.m_Target_Supplier.Count);
                    #endregion

                    List<List<Requirements.Requirement_Interface>> m_Requ_List = neu.Check_Requirement_Interface(repository, database, false, database.metamodel.m_Prozesswort_Interface[2]);
                    List<List<Requirements.Requirement_Interface>> m_Requ_List2 = neu2.Check_Requirement_Interface(repository, database, false, database.metamodel.m_Prozesswort_Interface[2]);

                    if (m_Requ_List != null)
                    {
                        int j1 = 0;
                        do
                        {
                            if (element_Interface.m_Requirement_Interface_Send.Contains(m_Requ_List[0][j1]) == false)
                            {
                                element_Interface.m_Requirement_Interface_Send.Add(m_Requ_List[0][j1]);
                            }

                            j1++;
                        } while (j1 < m_Requ_List[0].Count);
                        int j2 = 0;
                        do
                        {
                            if (element_Interface.m_Requirement_Interface_Receive.Contains(m_Requ_List[1][j2]) == false)
                            {
                                element_Interface.m_Requirement_Interface_Receive.Add(m_Requ_List[1][j2]);
                            }

                            j2++;
                        } while (j2 < m_Requ_List[1].Count);


                    }
                    if (m_Requ_List2 != null && m_Requ_List == null)
                    {
                        int j1 = 0;
                        do
                        {
                            if (element_Interface.m_Requirement_Interface_Send.Contains(m_Requ_List2[0][j1]) == false)
                            {
                                element_Interface.m_Requirement_Interface_Send.Add(m_Requ_List2[0][j1]);
                            }

                            j1++;
                        } while (j1 < m_Requ_List2[0].Count);
                        int j2 = 0;
                        do
                        {
                            if (element_Interface.m_Requirement_Interface_Receive.Contains(m_Requ_List2[1][j2]) == false)
                            {
                                element_Interface.m_Requirement_Interface_Receive.Add(m_Requ_List2[1][j2]);
                            }

                            j2++;
                        } while (j2 < m_Requ_List2[1].Count);

                    }
                    /*  if (Requ_List != null)
                      {
                          if (element_Interface.m_Requirement_Interface_Send.Contains(Requ_List[0]) == false)
                          {
                              element_Interface.m_Requirement_Interface_Send.Add(Requ_List[0]);
                          }
                          if (element_Interface.m_Requirement_Interface_Receive.Contains(Requ_List[1]) == false)
                          {
                              element_Interface.m_Requirement_Interface_Receive.Add(Requ_List[1]);
                          }
                      }
                      if(Requ_List == null && Requ_List2 != null)
                      {
                          if (element_Interface.m_Requirement_Interface_Send.Contains(Requ_List2[1]) == false)
                          {
                              element_Interface.m_Requirement_Interface_Send.Add(Requ_List2[1]);
                          }
                          if (element_Interface.m_Requirement_Interface_Receive.Contains(Requ_List2[0]) == false)
                          {
                              element_Interface.m_Requirement_Interface_Receive.Add(Requ_List2[0]);
                          }
                      }
                    */


                }
            }
            return (element_Interface);

        }
        #endregion

    }
}

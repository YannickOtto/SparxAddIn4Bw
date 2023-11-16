using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

using Database_Connection;
using Requirement_Plugin;
using Repsoitory_Elements;
using Requirements;
using Forms;

namespace Checks
{
    public class Check
    {
        Database database;
        EA.Repository repository;
        string Package_guid;

        public Check(Database database, EA.Repository repository)
        {
            this.database = database;
            this.repository = repository;
        }

        #region Check Pattern
        public void Check_Pattern()
        {
            Repository_Element repository_Element = new Repository_Element();
            Repository_Elements repository_Elements = new Repository_Elements();
            //Anlegen eines Packages zum Ablegen der Decomposition
            this.Package_guid = repository_Element.Create_Package_Model(this.database.metamodel.m_Package_Name[2], this.repository, this.database);

            if(this.database.metamodel.m_Pattern_flag.Contains(true))
            {
                if(this.database.m_Capability.Count ==  0)
                {
                    List<string> m_Cap_Guid =  repository_Elements.Get_Capability_GUID(this.repository, this.database);

                    if(m_Cap_Guid != null)
                    {
                        int i1 = 0;
                        do
                        {
                            this.database.m_Capability.Add(new Capability(m_Cap_Guid[i1], this.repository, this.database));

                            i1++;
                        } while (i1 < m_Cap_Guid.Count);
                    }
                }
                if (this.database.m_InformationElement.Count == 0)
                {
                    List<string> m_Info_Guid = repository_Elements.Get_Information_Element(this.database);

                    if (m_Info_Guid != null)
                    {
                        int i1 = 0;
                        do
                        {
                            this.database.m_InformationElement.Add(new InformationElement(m_Info_Guid[i1], this.database));

                            i1++;
                        } while (i1 < m_Info_Guid.Count);
                    }
                }
            }


            if (this.database.metamodel.m_Pattern_flag[0] == true)
            {
                Check_Interface();
            }

            if (this.database.metamodel.m_Pattern_flag[1] == true)
            {
                Check_Functional();
                Check_Stakeholder();
            }

            if (this.database.metamodel.m_Pattern_flag[2] == true)
            {
                Check_Design();
            }

            if (this.database.metamodel.m_Pattern_flag[3] == true)
            {
                Check_Process();
            }

            if (this.database.metamodel.m_Pattern_flag[4] == true)
            {
                Check_Umwelt();
            }


            if (this.database.metamodel.m_Pattern_flag[5] == true)
            {
                Check_Typevertreter();
            }


        }

        #region Check Faelle
        private void Check_Interface()
        {
            Repository_Elements repository_Elements = new Repository_Elements();
            Loading_OpArch Loading = new Loading_OpArch();
            Loading.Show();
            Loading.progressBar1.Minimum = 0;
            Loading.progressBar1.Maximum = 1;
            Loading.progressBar1.Value = 0;
            Loading.progressBar1.Step = 1;
            Loading.label2.Text = "Get Requirements Interface";
            Loading.label_Progress.Text = "Get Requirements Interface";
            Loading.Refresh();
            List<Requirement> m_Req = repository_Elements.Get_Requirements(this.database, this.repository, this.database.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList(), Loading, this.database.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList());
            DB_Command command = new DB_Command();

            if(m_Req != null)
            {
                Repository_Connector con = new Repository_Connector();
                List<string> m_Type_con = new List<string>();
                List<string> m_Stereotype_con = new List<string>();
                List<string> m_Toolbox_con = new List<string>();
                m_Type_con.Add("Abstraction");
                m_Stereotype_con.Add("trace");
                m_Toolbox_con.Add("");

                Loading.progressBar1.Minimum = 0;
                Loading.progressBar1.Maximum = m_Req.Count;
                Loading.progressBar1.Value = 0;
                Loading.label2.Text = "0/"+ m_Req.Count + "";
                Loading.label_Progress.Text = "Get Requirements Interface";
                Loading.Refresh();

                List<string> m_Type_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Sup3 = this.database.metamodel.m_Requirement_Interface.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup3 = this.database.metamodel.m_Requirement_Interface.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                List<string> m_Stereoype_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Con2 = this.database.metamodel.m_Afo_Requires.Select(x => x.Type).ToList();
                List<string> m_Stereoype_Con2 = this.database.metamodel.m_Afo_Requires.Select(x => x.Stereotype).ToList();


                int i1 = 0;
                do
                {
                    #region Check Client
                    //Definition
                    List<string> m_GUID_Def =  m_Req[i1].Get_Supplier(this.database, m_Type_Sup, m_Stereotype_Sup, m_Type_Con, m_Stereoype_Con);
                    //Usage
                    List<string> m_GUID_Usage = m_Req[i1].Get_Supplier(this.database, m_Type_Sup2, m_Stereotype_Sup2, m_Type_Con, m_Stereoype_Con);

                    if(m_GUID_Def != null)
                    {
                        if(m_GUID_Usage != null)
                        {
                            m_GUID_Def.AddRange(m_GUID_Usage);
                        }
                       
                    }
                    else
                    {
                        m_GUID_Def = m_GUID_Usage;
                    }


                    if (m_GUID_Def == null)
                    {
                        Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[0], database.metamodel.issues.m_Issue_Note[0], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    
                    #endregion Check Client

                    #region Check InfromationItem
                    m_Req[i1].Get_InformationElement(this.repository, this.database);

                    if(m_Req[i1].m_InformationElement.Count == 0)
                    {
                        Issue issue2 = new Issue(database, database.metamodel.issues.m_Issue_Name[1], database.metamodel.issues.m_Issue_Note[1], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue2.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    #endregion Check InformationItem

                    #region Check Capability
                    m_Req[i1].Check_Capability(this.repository, this.database);

                    if (m_Req[i1].m_Capability.Count == 0)
                    {
                        Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[2], database.metamodel.issues.m_Issue_Note[2], this.Package_guid, this.repository, true,  null);
                        con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }

                    if (m_Req[i1].m_Capability.Count > 1)
                    {
                        Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[3], database.metamodel.issues.m_Issue_Note[3], this.Package_guid, this.repository, true,  null);
                        con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);


                    }
                    #endregion Check Capability

                    #region Check Afo Requires
                    List<string> m_GUID_Supplier = m_Req[i1].Get_Supplier(this.database, m_Type_Sup3, m_Stereotype_Sup3, m_Type_Con2, m_Stereoype_Con2);
                    List<string> m_GUID_Client = m_Req[i1].Get_Client(this.database, m_Type_Sup3, m_Stereotype_Sup3, m_Type_Con2, m_Stereoype_Con2);
                    if (m_GUID_Supplier != null)
                    {
                        if(m_GUID_Client != null)
                        {
                            m_GUID_Supplier.AddRange(m_GUID_Client);
                        }                      
                    }
                    else
                    {
                        m_GUID_Supplier = m_GUID_Client;
                    }


                    if (m_GUID_Supplier == null)
                    {
                        Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[4], database.metamodel.issues.m_Issue_Note[4], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    #endregion Check Afo Requires
                    i1++;
                    Loading.progressBar1.PerformStep();
                    Loading.label2.Text = i1+"/" + m_Req.Count + "";
                    Loading.Refresh();
                } while (i1 < m_Req.Count); 
            }




            Loading.Close();
        }

        private void Check_Functional()
        {
            Repository_Elements repository_Elements = new Repository_Elements();
            Loading_OpArch Loading = new Loading_OpArch();
            Loading.Show();
            Loading.progressBar1.Minimum = 0;
            Loading.progressBar1.Maximum = 1;
            Loading.progressBar1.Value = 0;
            Loading.label2.Text = "Get Requirements Functional";
            Loading.label_Progress.Text = "Get Requirements Functional";
            Loading.Refresh();
            List<Requirement> m_Req = repository_Elements.Get_Requirements(this.database, this.repository, this.database.metamodel.m_Requirement_Functional.Select(x => x.Stereotype).ToList(), Loading, this.database.metamodel.m_Requirement_Functional.Select(x => x.Type).ToList());
        //    DB_Command command = new DB_Command();

            if(m_Req != null)
            {
                if (m_Req.Count > 0)
                {
                    Repository_Connector con = new Repository_Connector();
                    List<string> m_Type_con = new List<string>();
                    List<string> m_Stereotype_con = new List<string>();
                    List<string> m_Toolbox_con = new List<string>();
                    m_Type_con.Add("Abstraction");
                    m_Stereotype_con.Add("trace");
                    m_Toolbox_con.Add("");

                    Loading.progressBar1.Minimum = 0;
                    Loading.progressBar1.Maximum = m_Req.Count;
                    Loading.progressBar1.Value = 0;
                    Loading.label2.Text = "0/" + m_Req.Count + "";
                    Loading.label_Progress.Text = "Get Requirements Functional";
                    Loading.Refresh();

                    List<string> m_Type_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup3 = this.database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup3 = this.database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup4 = this.database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup4 = this.database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                    List<string> m_Stereoype_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                    int i1 = 0;
                    do
                    {
                        #region Check System
                        //Definition
                        List<string> m_GUID_Def = m_Req[i1].Get_Supplier(this.database, m_Type_Sup, m_Stereotype_Sup, m_Type_Con, m_Stereoype_Con);
                        //Usage
                        List<string> m_GUID_Usage = m_Req[i1].Get_Supplier(this.database, m_Type_Sup2, m_Stereotype_Sup2, m_Type_Con, m_Stereoype_Con);

                        if (m_GUID_Def != null)
                        {
                            if (m_GUID_Usage != null)
                            {
                                m_GUID_Def.AddRange(m_GUID_Usage);
                            }

                        }
                        else
                        {
                            m_GUID_Def = m_GUID_Usage;
                        }


                        if (m_GUID_Def == null)
                        {
                            Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[5], database.metamodel.issues.m_Issue_Note[5], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }
                        #endregion Check System

                        #region Check Activity
                        //Definition
                        List<string> m_GUID_Def2 = m_Req[i1].Get_Supplier(this.database, m_Type_Sup3, m_Stereotype_Sup3, m_Type_Con, m_Stereoype_Con);
                        //Usage
                        List<string> m_GUID_Usage2 = m_Req[i1].Get_Supplier(this.database, m_Type_Sup4, m_Stereotype_Sup4, m_Type_Con, m_Stereoype_Con);

                        if (m_GUID_Def2 != null)
                        {
                            if (m_GUID_Usage2 != null)
                            {
                                m_GUID_Def2.AddRange(m_GUID_Usage2);
                            }

                        }
                        else
                        {
                            m_GUID_Def2 = m_GUID_Usage2;
                        }


                        if (m_GUID_Def2 == null)
                        {
                            Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[6], database.metamodel.issues.m_Issue_Note[6], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }
                        #endregion Check Activity

                        #region Check Capability
                        m_Req[i1].Check_Capability(this.repository, this.database);

                        if (m_Req[i1].m_Capability.Count == 0)
                        {
                            Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[2], database.metamodel.issues.m_Issue_Note[2], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }

                        if (m_Req[i1].m_Capability.Count > 1)
                        {
                            Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[3], database.metamodel.issues.m_Issue_Note[3], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);


                        }
                        #endregion Check Capability
                        i1++;
                        Loading.progressBar1.PerformStep();
                        Loading.label2.Text = i1 + "/" + m_Req.Count + "";
                        Loading.Refresh();
                    } while (i1 < m_Req.Count);
                }
            }
         


            Loading.Close();

        }

        private void Check_Stakeholder()
        {
            Repository_Elements repository_Elements = new Repository_Elements();
            Loading_OpArch Loading = new Loading_OpArch();
            Loading.Show();
            Loading.progressBar1.Minimum = 0;
            Loading.progressBar1.Maximum = 1;
            Loading.progressBar1.Value = 0;
            Loading.label2.Text = "Get Requirements Stakeholder";
            Loading.label_Progress.Text = "Get Requirements Stakeholder";
            Loading.Refresh();
            List<Requirement> m_Req = repository_Elements.Get_Requirements(this.database, this.repository, this.database.metamodel.m_Requirement_User.Select(x => x.Stereotype).ToList(), Loading, this.database.metamodel.m_Requirement_User.Select(x => x.Type).ToList());
            DB_Command command = new DB_Command();

            if(m_Req != null)
            {
                if (m_Req.Count > 0)
                {
                    Repository_Connector con = new Repository_Connector();
                    List<string> m_Type_con = new List<string>();
                    List<string> m_Stereotype_con = new List<string>();
                    List<string> m_Toolbox_con = new List<string>();
                    m_Type_con.Add("Abstraction");
                    m_Stereotype_con.Add("trace");
                    m_Toolbox_con.Add("");

                    Loading.progressBar1.Minimum = 0;
                    Loading.progressBar1.Maximum = m_Req.Count;
                    Loading.progressBar1.Value = 0;
                    Loading.label2.Text = "0/" + m_Req.Count + "";
                    Loading.label_Progress.Text = "Get Requirements Stakeholder";
                    Loading.Refresh();

                    List<string> m_Type_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup3 = this.database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup3 = this.database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup4 = this.database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup4 = this.database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup5 = this.database.metamodel.m_Stakeholder_Definition.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup5 = this.database.metamodel.m_Stakeholder_Definition.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup6 = this.database.metamodel.m_Stakeholder_Usage.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup6 = this.database.metamodel.m_Stakeholder_Usage.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                    List<string> m_Stereoype_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                    int i1 = 0;
                    do
                    {
                        #region Check System
                        //Definition
                        List<string> m_GUID_Def = m_Req[i1].Get_Supplier(this.database, m_Type_Sup, m_Stereotype_Sup, m_Type_Con, m_Stereoype_Con);
                        //Usage
                        List<string> m_GUID_Usage = m_Req[i1].Get_Supplier(this.database, m_Type_Sup2, m_Stereotype_Sup2, m_Type_Con, m_Stereoype_Con);

                        if (m_GUID_Def != null)
                        {
                            if (m_GUID_Usage != null)
                            {
                                m_GUID_Def.AddRange(m_GUID_Usage);
                            }

                        }
                        else
                        {
                            m_GUID_Def = m_GUID_Usage;
                        }


                        if (m_GUID_Def == null)
                        {
                            Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[5], database.metamodel.issues.m_Issue_Note[5], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }
                        #endregion Check System

                        #region Check Activity
                        //Definition
                        List<string> m_GUID_Def2 = m_Req[i1].Get_Supplier(this.database, m_Type_Sup3, m_Stereotype_Sup3, m_Type_Con, m_Stereoype_Con);
                        //Usage
                        List<string> m_GUID_Usage2 = m_Req[i1].Get_Supplier(this.database, m_Type_Sup4, m_Stereotype_Sup4, m_Type_Con, m_Stereoype_Con);

                        if (m_GUID_Def2 != null)
                        {
                            if (m_GUID_Usage2 != null)
                            {
                                m_GUID_Def2.AddRange(m_GUID_Usage2);
                            }

                        }
                        else
                        {
                            m_GUID_Def2 = m_GUID_Usage2;
                        }


                        if (m_GUID_Def2 == null)
                        {
                            Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[6], database.metamodel.issues.m_Issue_Note[6], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }
                        #endregion Check Activity

                        #region Check Stakeholder
                        //Definition
                        List<string> m_GUID_Def3 = m_Req[i1].Get_Supplier(this.database, m_Type_Sup5, m_Stereotype_Sup5, m_Type_Con, m_Stereoype_Con);
                        //Usage
                        List<string> m_GUID_Usage3 = m_Req[i1].Get_Supplier(this.database, m_Type_Sup6, m_Stereotype_Sup6, m_Type_Con, m_Stereoype_Con);

                        if (m_GUID_Def3 != null)
                        {
                            if (m_GUID_Usage3 != null)
                            {
                                m_GUID_Def3.AddRange(m_GUID_Usage3);
                            }

                        }
                        else
                        {
                            m_GUID_Def3 = m_GUID_Usage3;
                        }


                        if (m_GUID_Def3 == null)
                        {
                            Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[7], database.metamodel.issues.m_Issue_Note[7], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }
                        #endregion Check Stakeholder

                        #region Check Capability
                        m_Req[i1].Check_Capability(this.repository, this.database);

                        if (m_Req[i1].m_Capability.Count == 0)
                        {
                            Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[2], database.metamodel.issues.m_Issue_Note[2], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }

                        if (m_Req[i1].m_Capability.Count > 1)
                        {
                            Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[3], database.metamodel.issues.m_Issue_Note[3], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);


                        }
                        #endregion Check Capability

                        i1++;
                        Loading.progressBar1.PerformStep();
                        Loading.label2.Text = i1 + "/" + m_Req.Count + "";
                        Loading.Refresh();
                    } while (i1 < m_Req.Count);


                   
                }
               
            }
            Loading.Close();
        }

        private void Check_Design()
        {
            Repository_Elements repository_Elements = new Repository_Elements();
            Loading_OpArch Loading = new Loading_OpArch();
            Loading.Show();
            Loading.progressBar1.Minimum = 0;
            Loading.progressBar1.Maximum = 1;
            Loading.progressBar1.Value = 0;
            Loading.progressBar1.Step = 1;
            Loading.label2.Text = "Get Requirements Design";
            Loading.label_Progress.Text = "Get Requirements Design";
            Loading.Refresh();
            List<Requirement> m_Req = repository_Elements.Get_Requirements(this.database, this.repository, this.database.metamodel.m_Requirement_Design.Select(x => x.Stereotype).ToList(), Loading, this.database.metamodel.m_Requirement_Design.Select(x => x.Type).ToList());
            DB_Command command = new DB_Command();

            if (m_Req != null)
            {
                Repository_Connector con = new Repository_Connector();
                List<string> m_Type_con = new List<string>();
                List<string> m_Stereotype_con = new List<string>();
                List<string> m_Toolbox_con = new List<string>();
                m_Type_con.Add("Abstraction");
                m_Stereotype_con.Add("trace");
                m_Toolbox_con.Add("");

                Loading.progressBar1.Minimum = 0;
                Loading.progressBar1.Maximum = m_Req.Count;
                Loading.progressBar1.Value = 0;
                Loading.label2.Text = "0/" + m_Req.Count + "";
                Loading.label_Progress.Text = "Get Requirements Design";
                Loading.Refresh();

                List<string> m_Type_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                List<string> m_Stereoype_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                int i1 = 0;
                do
                {
                    #region Check System
                    //Definition
                    List<string> m_GUID_Def = m_Req[i1].Get_Supplier(this.database, m_Type_Sup, m_Stereotype_Sup, m_Type_Con, m_Stereoype_Con);
                    //Usage
                    List<string> m_GUID_Usage = m_Req[i1].Get_Supplier(this.database, m_Type_Sup2, m_Stereotype_Sup2, m_Type_Con, m_Stereoype_Con);

                    if (m_GUID_Def != null)
                    {
                        if (m_GUID_Usage != null)
                        {
                            m_GUID_Def.AddRange(m_GUID_Usage);
                        }

                    }
                    else
                    {
                        m_GUID_Def = m_GUID_Usage;
                    }


                    if (m_GUID_Def == null)
                    {
                        Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[5], database.metamodel.issues.m_Issue_Note[5], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    #endregion Check_System
                    #region Check Capability
                    m_Req[i1].Check_Capability(this.repository, this.database);

                    if (m_Req[i1].m_Capability.Count == 0)
                    {
                        Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[2], database.metamodel.issues.m_Issue_Note[2], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }

                    if (m_Req[i1].m_Capability.Count > 1)
                    {
                        Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[3], database.metamodel.issues.m_Issue_Note[3], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    #endregion Check Capability
                    #region DesignConstraint
                    m_Req[i1].Get_DesignConstraint(this.repository, this.database);

                    if (m_Req[i1].m_Design.Count == 0) //Mit keinem DesignConstraint verbunden
                    {
                        Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[12], database.metamodel.issues.m_Issue_Note[12], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    //Überprüfung der DesignConstraint ob nur an Definitionen und deren Instanzen
                    if (m_Req[i1].m_Design.Count> 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<string> m_GUID_false = m_Req[i1].m_Design[i2].Check_DesignConstraint(database);

                            if(m_GUID_false != null)
                            {
                                int i3 = 0;
                                do
                                {
                                    Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[13], database.metamodel.issues.m_Issue_Note[13], this.Package_guid, this.repository, true, null);
                                    con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].m_Design[i2].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                                    con.Create_Dependency(issue4.Classifier_ID, m_GUID_false[i3], m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);


                                    i3++;
                                } while (i3 < m_GUID_false.Count);
                            }

                            i2++;
                        } while (i2 < m_Req[i1].m_Design.Count);
                    }

                    #endregion DesignConstraint

                        i1++;
                } while (i1 < m_Req.Count);

                
            }

            Loading.Close();
        }

        private void Check_Process()
        {
            Repository_Elements repository_Elements = new Repository_Elements();
            Loading_OpArch Loading = new Loading_OpArch();
            Loading.Show();
            Loading.progressBar1.Minimum = 0;
            Loading.progressBar1.Maximum = 1;
            Loading.progressBar1.Value = 0;
            Loading.label2.Text = "Get Requirements Process";
            Loading.label_Progress.Text = "Get Requirements Process";
            Loading.Refresh();
            List<Requirement> m_Req = repository_Elements.Get_Requirements(this.database, this.repository, this.database.metamodel.m_Requirement_Process.Select(x => x.Stereotype).ToList(), Loading, this.database.metamodel.m_Requirement_Process.Select(x => x.Type).ToList());
          
            if(m_Req != null)
            {
                if (m_Req.Count > 0)
                {
                    Repository_Connector con = new Repository_Connector();
                    List<string> m_Type_con = new List<string>();
                    List<string> m_Stereotype_con = new List<string>();
                    List<string> m_Toolbox_con = new List<string>();
                    m_Type_con.Add("Abstraction");
                    m_Stereotype_con.Add("trace");
                    m_Toolbox_con.Add("");

                    Loading.progressBar1.Minimum = 0;
                    Loading.progressBar1.Maximum = m_Req.Count;
                    Loading.progressBar1.Value = 0;
                    Loading.label2.Text = "0/" + m_Req.Count + "";
                    Loading.label_Progress.Text = "Get Requirements Functional";
                    Loading.Refresh();

                    List<string> m_Type_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup3 = this.database.metamodel.m_Aktivity_Definition.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup3 = this.database.metamodel.m_Aktivity_Definition.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup4 = this.database.metamodel.m_Aktivity_Usage.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup4 = this.database.metamodel.m_Aktivity_Usage.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Sup5 = this.database.metamodel.m_Process_Constraint.Select(x => x.Type).ToList();
                    List<string> m_Stereotype_Sup5 = this.database.metamodel.m_Process_Constraint.Select(x => x.Stereotype).ToList();

                    List<string> m_Type_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                    List<string> m_Stereoype_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                    int i1 = 0;
                    do
                    {
                        #region Check System
                        //Definition
                        List<string> m_GUID_Def = m_Req[i1].Get_Supplier(this.database, m_Type_Sup, m_Stereotype_Sup, m_Type_Con, m_Stereoype_Con);
                        //Usage
                        List<string> m_GUID_Usage = m_Req[i1].Get_Supplier(this.database, m_Type_Sup2, m_Stereotype_Sup2, m_Type_Con, m_Stereoype_Con);

                        if (m_GUID_Def != null)
                        {
                            if (m_GUID_Usage != null)
                            {
                                m_GUID_Def.AddRange(m_GUID_Usage);
                            }

                        }
                        else
                        {
                            m_GUID_Def = m_GUID_Usage;
                        }


                        if (m_GUID_Def == null)
                        {
                            Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[5], database.metamodel.issues.m_Issue_Note[5], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }
                        #endregion Check System

                        #region Check Activity
                        //Definition
                        List<string> m_GUID_Def2 = m_Req[i1].Get_Supplier(this.database, m_Type_Sup3, m_Stereotype_Sup3, m_Type_Con, m_Stereoype_Con);
                        //Usage
                        List<string> m_GUID_Usage2 = m_Req[i1].Get_Supplier(this.database, m_Type_Sup4, m_Stereotype_Sup4, m_Type_Con, m_Stereoype_Con);

                        if (m_GUID_Def2 != null)
                        {
                            if (m_GUID_Usage2 != null)
                            {
                                m_GUID_Def2.AddRange(m_GUID_Usage2);
                            }

                        }
                        else
                        {
                            m_GUID_Def2 = m_GUID_Usage2;
                        }


                        if (m_GUID_Def2 == null)
                        {
                            Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[6], database.metamodel.issues.m_Issue_Note[6], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }
                        #endregion Check Activity

                        #region Check Capability
                        m_Req[i1].Check_Capability(this.repository, this.database);

                        if (m_Req[i1].m_Capability.Count == 0)
                        {
                            Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[2], database.metamodel.issues.m_Issue_Note[2], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }

                        if (m_Req[i1].m_Capability.Count > 1)
                        {
                            Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[3], database.metamodel.issues.m_Issue_Note[3], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);


                        }
                        #endregion Check Capability

                        #region Check ProcessConstraint
                        List<string> m_GUID_OpCon = m_Req[i1].Get_Supplier(this.database, m_Type_Sup5, m_Stereotype_Sup5, m_Type_Con, m_Stereoype_Con);

                        if (m_GUID_OpCon == null)
                        {
                            Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[14], database.metamodel.issues.m_Issue_Note[14], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                        }
                        #endregion Check ProcessConstraint

                        i1++;
                        Loading.progressBar1.PerformStep();
                        Loading.label2.Text = i1 + "/" + m_Req.Count + "";
                        Loading.Refresh();
                    } while (i1 < m_Req.Count);
                }
            }
            
          

            Loading.Close();


        }

        private void Check_Umwelt()
        {
            Repository_Elements repository_Elements = new Repository_Elements();
            Loading_OpArch Loading = new Loading_OpArch();
            Loading.Show();
            Loading.progressBar1.Minimum = 0;
            Loading.progressBar1.Maximum = 1;
            Loading.progressBar1.Value = 0;
            Loading.progressBar1.Step = 1;
            Loading.label2.Text = "Get Requirements Umwelt";
            Loading.label_Progress.Text = "Get Requirements Umwelt";
            Loading.Refresh();
            List<Requirement> m_Req = repository_Elements.Get_Requirements(this.database, this.repository, this.database.metamodel.m_Requirement_Environment.Select(x => x.Stereotype).ToList(), Loading, this.database.metamodel.m_Requirement_Environment.Select(x => x.Type).ToList());
          //  DB_Command command = new DB_Command();

            if (m_Req != null)
            {
                Repository_Connector con = new Repository_Connector();
                List<string> m_Type_con = new List<string>();
                List<string> m_Stereotype_con = new List<string>();
                List<string> m_Toolbox_con = new List<string>();
                m_Type_con.Add("Abstraction");
                m_Stereotype_con.Add("trace");
                m_Toolbox_con.Add("");

                Loading.progressBar1.Minimum = 0;
                Loading.progressBar1.Maximum = m_Req.Count;
                Loading.progressBar1.Value = 0;
                Loading.label2.Text = "0/" + m_Req.Count + "";
                Loading.label_Progress.Text = "Get Requirements Umwelt";
                Loading.Refresh();

                List<string> m_Type_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                List<string> m_Stereoype_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                int i1 = 0;
                do
                {
                    #region Check System
                    //Definition
                    List<string> m_GUID_Def = m_Req[i1].Get_Supplier(this.database, m_Type_Sup, m_Stereotype_Sup, m_Type_Con, m_Stereoype_Con);
                    //Usage
                    List<string> m_GUID_Usage = m_Req[i1].Get_Supplier(this.database, m_Type_Sup2, m_Stereotype_Sup2, m_Type_Con, m_Stereoype_Con);

                    if (m_GUID_Def != null)
                    {
                        if (m_GUID_Usage != null)
                        {
                            m_GUID_Def.AddRange(m_GUID_Usage);
                        }

                    }
                    else
                    {
                        m_GUID_Def = m_GUID_Usage;
                    }


                    if (m_GUID_Def == null)
                    {
                        Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[5], database.metamodel.issues.m_Issue_Note[5], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    #endregion Check_System
                    #region Check Capability
                    m_Req[i1].Check_Capability(this.repository, this.database);

                    if (m_Req[i1].m_Capability.Count == 0)
                    {
                        Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[2], database.metamodel.issues.m_Issue_Note[2], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }

                    if (m_Req[i1].m_Capability.Count > 1)
                    {
                        Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[3], database.metamodel.issues.m_Issue_Note[3], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    #endregion Check Capability
                    #region UmweltConstraint
                    m_Req[i1].Get_UmweltConstraint(this.repository, this.database);

                    if (m_Req[i1].m_Umwelt.Count == 0) //Mit keinem UmweltConstraint verbunden
                    {
                        Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[15], database.metamodel.issues.m_Issue_Note[15], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    //Überprüfung der DesignConstraint ob nur an Definitionen und deren Instanzen
                    if (m_Req[i1].m_Umwelt.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            List<string> m_GUID_false = m_Req[i1].m_Umwelt[i2].Check_UmweltConstraint(database);

                            if (m_GUID_false != null)
                            {
                                int i3 = 0;
                                do
                                {
                                    Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[15], database.metamodel.issues.m_Issue_Note[15], this.Package_guid, this.repository, true, null);
                                    con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].m_Design[i2].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                                    con.Create_Dependency(issue4.Classifier_ID, m_GUID_false[i3], m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);


                                    i3++;
                                } while (i3 < m_GUID_false.Count);
                            }

                            i2++;
                        } while (i2 < m_Req[i1].m_Umwelt.Count);
                    }

                    #endregion DesignConstraint

                    i1++;
                } while (i1 < m_Req.Count);


            }

            Loading.Close();
        }

        private void Check_Typevertreter()
        {
            //Requiremnt muss mit einem Element aus Decomposition und einem Element aus Typevertreter verbunden sein. Hierbei dürfen diese Elemente von der GUID nicht über einstimmen

            Repository_Elements repository_Elements = new Repository_Elements();
            Requirement_Plugin.Interfaces.Interface_Connectors interface_Connectors = new Requirement_Plugin.Interfaces.Interface_Connectors();
            Loading_OpArch Loading = new Loading_OpArch();
            Loading.Show();
            Loading.progressBar1.Minimum = 0;
            Loading.progressBar1.Maximum = 1;
            Loading.progressBar1.Value = 0;
            Loading.progressBar1.Step = 1;
            Loading.label2.Text = "Get Requirements Typevertreter";
            Loading.label_Progress.Text = "Get Requirements Typevertreter";
            Loading.Refresh();
            List<Requirement> m_Req = repository_Elements.Get_Requirements(this.database, this.repository, this.database.metamodel.m_Requirement_Typvertreter.Select(x => x.Stereotype).ToList(), Loading, this.database.metamodel.m_Requirement_Typvertreter.Select(x => x.Type).ToList());

            if (m_Req != null)
            {
                Repository_Connector con = new Repository_Connector();
                List<string> m_Type_con = new List<string>();
                List<string> m_Stereotype_con = new List<string>();
                List<string> m_Toolbox_con = new List<string>();
                m_Type_con.Add("Abstraction");
                m_Stereotype_con.Add("trace");
                m_Toolbox_con.Add("");

                Loading.progressBar1.Minimum = 0;
                Loading.progressBar1.Maximum = m_Req.Count;
                Loading.progressBar1.Value = 0;
                Loading.label2.Text = "0/" + m_Req.Count + "";
                Loading.label_Progress.Text = "Get Requirements Umwelt";
                Loading.Refresh();

                List<string> m_Type_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup = this.database.metamodel.m_Elements_Definition.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Type).ToList();
                List<string> m_Stereotype_Sup2 = this.database.metamodel.m_Elements_Usage.Select(x => x.Stereotype).ToList();

                List<string> m_Type_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Type).ToList();
                List<string> m_Stereoype_Con = this.database.metamodel.m_Derived_Element.Select(x => x.Stereotype).ToList();

                int i1 = 0;
                do
                {
                    #region Check System
                    //Definition
                    List<string> m_GUID_Def = m_Req[i1].Get_Supplier(this.database, m_Type_Sup, m_Stereotype_Sup, m_Type_Con, m_Stereoype_Con);
                    //Usage
                    List<string> m_GUID_Usage = m_Req[i1].Get_Supplier(this.database, m_Type_Sup2, m_Stereotype_Sup2, m_Type_Con, m_Stereoype_Con);

                    if (m_GUID_Def != null)
                    {
                        if (m_GUID_Usage != null)
                        {
                            m_GUID_Def.AddRange(m_GUID_Usage);
                        }

                    }
                    else
                    {
                        m_GUID_Def = m_GUID_Usage;
                    }


                    if (m_GUID_Def == null)
                    {
                        Issue issue = new Issue(database, database.metamodel.issues.m_Issue_Name[5], database.metamodel.issues.m_Issue_Note[5], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    #endregion Check_System
                    #region Check Capability
                    m_Req[i1].Check_Capability(this.repository, this.database);

                    if (m_Req[i1].m_Capability.Count == 0)
                    {
                        Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[2], database.metamodel.issues.m_Issue_Note[2], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }

                    if (m_Req[i1].m_Capability.Count > 1)
                    {
                        Issue issue4 = new Issue(database, database.metamodel.issues.m_Issue_Name[3], database.metamodel.issues.m_Issue_Note[3], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue4.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    #endregion Check Capability
                    #region Typevertreter
                    m_Req[i1].Get_Typevertreter(this.repository, this.database);

                    if (m_Req[i1].m_Typevertreter.Count == 0) //Mit keinem Typevertreter verbunden
                    {
                        Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[16], database.metamodel.issues.m_Issue_Note[16], this.Package_guid, this.repository, true, null);
                        con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);
                    }
                    //Überprüfung der Typevertreter nicht in den oben bestimmten Systeme ob nur an Definitionen und deren Instanzen
                    if (m_Req[i1].m_Typevertreter.Count > 0 && m_GUID_Def != null)
                    {
                        int i3 = 0;
                        do
                        {
                            m_Req[i1].m_Typevertreter[i3].StereoType =  m_Req[i1].m_Typevertreter[i3].Get_Stereotype(database);

                            i3++;
                        } while (i3 < m_Req[i1].m_Typevertreter.Count);

                        List<string> m_stereotype_type = m_Req[i1].m_Typevertreter.Select(x => x.StereoType).ToList();
                        bool flag_uebereinstimmung = false;

                        int i2 = 0;
                        do
                        {
                            NodeType help = new NodeType(null, null, null);
                            help.Classifier_ID = m_GUID_Def[i2];
                            help.StereoType = help.Get_Stereotype(database);

                            if (m_stereotype_type.Contains(help.StereoType) == false)
                            {
                                flag_uebereinstimmung = true;
                            }

                            i2++;
                        } while (i2 < m_GUID_Def.Count);

                        if(flag_uebereinstimmung == false)
                        {
                            Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[17], database.metamodel.issues.m_Issue_Note[17], this.Package_guid, this.repository, true, null);
                            con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);

                        }
                    }

                    #endregion Typevertreter
                    #region Check TypevertreterCon
                    //Der Typevertreter muss immer Client der REalisation sein
                    if (m_Req[i1].m_Typevertreter.Count >  0)
                    {
                        List<string> help2 = new List<string>();

                        int i4 = 0;
                        do
                        {
                            help2.Clear();
                            help2.Add(m_Req[i1].m_Typevertreter[i4].Classifier_ID);

                            List<string> m_GUID_start = interface_Connectors.Get_Connector_By_m_Client_GUID(database, help2, database.metamodel.m_Con_Typvertreter.Select(x => x.Type).ToList(), database.metamodel.m_Con_Typvertreter.Select(x => x.Stereotype).ToList());
                            if(m_GUID_start == null)
                            {
                                Issue issue3 = new Issue(database, database.metamodel.issues.m_Issue_Name[18], database.metamodel.issues.m_Issue_Note[18], this.Package_guid, this.repository, true, null);
                                con.Create_Dependency(issue3.Classifier_ID, m_Req[i1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);

                            }

                            i4++;
                        } while (i4 < m_Req[i1].m_Typevertreter.Count);
                       
                    }
                    #endregion Typevertreter Con
                        i1++;
                } while (i1 < m_Req.Count);


               
            }
            Loading.Close();
        }

        #endregion
        #endregion


        #region AFO_MGMT

        #region Loop
        public void Check_Refines_Loops(List<Requirement> m_req)
        {
            List<List<Requirement>> m_ret = new List<List<Requirement>>();
            List<Requirement> m_refines = m_req.Where(x => x.m_Requirement_Refines.Count > 0).ToList();

            List<Requirement> m_done = new List<Requirement>();

            #region Loops erhalten
            //
            if (m_refines.Count > 1)
            {
                int i1 = 0;
                do
                {
                    if (m_done.Contains(m_refines[i1]) == false)
                    {
                        List<Requirement> m_loop = new List<Requirement>();
                        m_loop.Add(m_refines[i1]);

                        List<List<Requirement>> m_help = this.Check_Loop_rekrusiv(m_loop);

                        if (m_help != null)
                        {
                            if (m_help.Count > 0)
                            {
                                List<List<Requirement>> m_help2 = m_help.Where(x => x != null).ToList();

                                if (m_help2.Count > 0)
                                {
                                    m_ret.AddRange(m_help2);
                                }
                            }
                        }

                    }

                    i1++;
                } while (i1 < m_refines.Count);
                #endregion

            #region Doppelte Loops entfernen

                if (m_ret.Count > 0)
                {
                    int i2 = 0;
                    do
                    {
                        if (i2 + 1 < m_ret.Count)
                        {
                            int i3 = i2 + 1;
                            do
                            {
                                List<string> test = m_ret[i2].Select(x => x.Classifier_ID).ToList().Except(m_ret[i3].Select(y => y.Classifier_ID).ToList()).ToList();

                                if (test.Count == 0)//selbe Liste
                                {
                                    m_ret.RemoveAt(i3);
                                    i3--;
                                }

                                i3++;
                            } while (i3 < m_ret.Count);
                        }


                        i2++;
                    } while (i2 < m_ret.Count);



                }
                #endregion

            #region Issue für die Loops erstellen
                if (m_ret.Count > 0)
                {
                    Repository_Element repository_Element = new Repository_Element();
                    Repository_Connector con = new Repository_Connector();

                    List<string> m_Type_con = new List<string>();
                    List<string> m_Stereotype_con = new List<string>();
                    m_Type_con.Add("Abstraction");
                    m_Stereotype_con.Add("trace");

                    List<string> m_Toolbox_con = new List<string>();
                    m_Toolbox_con.Add("");

                    this.Package_guid = repository_Element.Create_Package_Model(this.database.metamodel.m_Package_Name[2], this.repository, this.database);

                    string Package_guid_issue = repository_Element.Create_Package(this.database.metamodel.m_Package_Name[17], repository.GetPackageByGuid(this.Package_guid), repository, database);
                    repository.GetPackageByGuid(Package_guid).Packages.Refresh();
                    repository.GetPackageByGuid(Package_guid).Update();
                    repository.GetPackageByGuid(Package_guid_issue).Packages.Refresh();


                    int i3 = 0;
                    do
                    {
                        if (m_ret[i3].Count > 0)
                        {
                            m_ret[i3][0].Name = m_ret[i3][0].Get_Name(this.database);
                            m_ret[i3][m_ret[i3].Count-1].Name = m_ret[i3][m_ret[i3].Count - 1].Get_Name(this.database);
                            string name = this.database.metamodel.issues.m_Issue_Name[24] +" "+ m_ret[i3][0].Name+ " --> "+ m_ret[i3][m_ret[i3].Count - 1].Name;
                            //Issue erstellen
                            Issue issue = new Issue(this.database, name, database.metamodel.issues.m_Issue_Note[24], Package_guid_issue, this.repository, true, null);
                            //Issue mit Anforderungen der Loop verknüpfen
                            int i4 = 0;
                            do
                            {
                                con.Create_Dependency(issue.Classifier_ID, m_ret[i3][i4].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);

                                i4++;
                            } while (i4 < m_ret[i3].Count);
                        }

                        i3++;
                    } while (i3 < m_ret.Count);
                }

                #endregion
            }
        }


        private List<List<Requirement>> Check_Loop_rekrusiv(List<Requirement> m_loop)
        {
            List<List<Requirement>> m_ret = new List<List<Requirement>>();

           


            if (m_loop[m_loop.Count - 1].m_Requirement_Refines.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if (m_loop.Contains(m_loop[m_loop.Count - 1].m_Requirement_Refines[i1]) == false)
                    {
                        List<Requirement> m_child_loop = new List<Requirement>();
                        m_child_loop.AddRange(m_loop);
                        m_child_loop.Add(m_loop[m_loop.Count - 1].m_Requirement_Refines[i1]);

                        List<List<Requirement>> m_help = this.Check_Loop_rekrusiv(m_child_loop);

                        if (m_help != null)
                        {
                            if (m_help.Count > 0)
                            {
                                List<List<Requirement>> m_help2 = m_help.Where(x => x != null).ToList();

                                if (m_help2.Count > 0)
                                {
                                    m_ret.AddRange(m_help2);
                                }
                            }
                        }
                    }
                    else
                    {

                        //Zuschneiden Loop und Dopplung entfernen
                        int index = m_loop.IndexOf(m_loop[m_loop.Count - 1].m_Requirement_Refines[i1]);

                        List<Requirement> m_help = m_loop.Where(x => m_loop.IndexOf(x) >= index).ToList();

                        m_ret.Add(m_help);
                    }

                    i1++;
                } while (i1 < m_loop[m_loop.Count - 1].m_Requirement_Refines.Count);

                if (m_ret.Count == 0)
                {
                    m_ret = null;
                }

                return (m_ret);

            }
            else
            {
                return (null);
            }



        }

        #endregion

        #region Multiple Refines

        public void Check_Multiple_Refines(List<Requirement> m_req)
        {
            List<Requirement> m_ret = new List<Requirement>();

            #region Erhalten Multiple Refines
            if (m_req.Count > 0)
            {
                int i1 = 0;
                do
                {
                    bool help = this.Check_Multiple(m_req, m_req[i1]);

                    if(help == true && m_ret.Contains(m_req[i1]) == false)
                    {
                        m_ret.Add(m_req[i1]);
                    }

                    i1++;
                } while (i1 < m_req.Count);
            }
            #endregion

            #region Issue erzeugen
            if(m_ret.Count > 0)
            {
                Repository_Element repository_Element = new Repository_Element();
                Repository_Connector con = new Repository_Connector();

                List<string> m_Type_con = new List<string>();
                List<string> m_Stereotype_con = new List<string>();
                m_Type_con.Add("Abstraction");
                m_Stereotype_con.Add("trace");

                List<string> m_Toolbox_con = new List<string>();
                m_Toolbox_con.Add("");

                this.Package_guid = repository_Element.Create_Package_Model(this.database.metamodel.m_Package_Name[2], this.repository, this.database);

                string Package_guid_issue = repository_Element.Create_Package(this.database.metamodel.m_Package_Name[18], repository.GetPackageByGuid(this.Package_guid), repository, database);
                repository.GetPackageByGuid(Package_guid).Packages.Refresh();
                repository.GetPackageByGuid(Package_guid).Update();
                repository.GetPackageByGuid(Package_guid_issue).Packages.Refresh();

                int i2 = 0;
                do
                {
                    m_ret[i2].Name = m_ret[i2].Get_Name(this.database);
                    string name = this.database.metamodel.issues.m_Issue_Name[25] + m_ret[i2].Name;
                    //Issue erstellen
                    Issue issue = new Issue(this.database, name, database.metamodel.issues.m_Issue_Note[25], Package_guid_issue, this.repository, true, null);
                    //Issue mit Anforderungen der Loop verknüpfen
                    con.Create_Dependency(issue.Classifier_ID, m_ret[i2].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, m_Toolbox_con[0], true);

                    i2++;
                } while (i2 < m_ret.Count);
            }

            #endregion
        }

        private bool Check_Multiple(List<Requirement> m_req, Requirement req)
        {
            bool ret = false;

            List<Requirement> m_help = m_req.Where(x => x.m_Requirement_Refines.Contains(req) == true).ToList();

            if(m_help.Count > 1)
            {
                ret = true;
            }

            return (ret);

        }

        #endregion
        #endregion

        #region Dubletten

        public void Check_Dubletten(List<Requirement> m_req)
        {

            List<List<Requirement>> m_ret = new List<List<Requirement>>();
            List<List<Requirement>> m_ret_replace = new List<List<Requirement>>();

            List<Requirement> m_dublette = m_req.Where(x => x.m_Requirement_Duplicate.Count > 0).ToList();
            List<Requirement> m_replace = m_req.Where(x => x.m_Requirement_Replace.Count > 0).ToList();

            if (m_dublette.Count > 0)
            {
                int i1 = 0;
                do
                {
                    if(m_dublette[i1].m_Requirement_Duplicate.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            #region Check Export
                            //--> eine der Anforderungen wird nicht exportiert
                            if (m_dublette[i1].RPI_Export == true && m_dublette[i1].m_Requirement_Duplicate[i2].RPI_Export == true)
                            {
                                //--> Eine Anforderung ist als Überschrift deklariert
                                if (m_dublette[i1].AFO_WV_ART != Ennumerationen.AFO_WV_ART.Überschrift_Einleitung && m_dublette[i1].m_Requirement_Duplicate[i2].AFO_WV_ART != Ennumerationen.AFO_WV_ART.Überschrift_Einleitung)
                                {
                                    List<Requirement> m_help1 = m_replace.Where(x => x.m_Requirement_Replace.Contains(m_dublette[i1]) && x.m_Requirement_Replace.Contains(m_dublette[i1].m_Requirement_Duplicate[i2])).ToList();
                                   
                                    if (m_help1.Count > 0)
                                    {
                                        List<Requirement> m_help = new List<Requirement>();
                                        m_help.Add(m_dublette[i1]);
                                        m_help.Add(m_dublette[i1].m_Requirement_Duplicate[i2]);

                                        m_ret_replace.Add(m_help);
                                    }
                                    else
                                    {
                                        List<Requirement> m_help = new List<Requirement>();
                                        m_help.Add(m_dublette[i1]);
                                        m_help.Add(m_dublette[i1].m_Requirement_Duplicate[i2]);

                                        m_ret.Add(m_help);
                                    }

                                  
                                }
                            }
                            #endregion

                            i2++;
                        } while (i2 < m_dublette[i1].m_Requirement_Duplicate.Count);
                    }
                  
                   

                    i1++;
                } while (i1 < m_dublette.Count);
            }

            if(m_ret.Count > 0)
            {
                //Packages erzeugen
                Repository_Element repository_Element = new Repository_Element();
                Repository_Connector con = new Repository_Connector();

                List<string> m_Type_con = new List<string>();
                List<string> m_Stereotype_con = new List<string>();
                List<string> Toolbox_con = new List<string>();
                Toolbox_con.Add("");
                m_Type_con.Add("Abstraction");
                m_Stereotype_con.Add("trace");

                this.Package_guid = repository_Element.Create_Package_Model(this.database.metamodel.m_Package_Name[2], this.repository, this.database);

                string Package_guid_issue = repository_Element.Create_Package(this.database.metamodel.m_Package_Name[19], repository.GetPackageByGuid(this.Package_guid), repository, database);
                repository.GetPackageByGuid(Package_guid).Packages.Refresh();
                repository.GetPackageByGuid(Package_guid).Update();
                repository.GetPackageByGuid(Package_guid_issue).Packages.Refresh();

                int i3 = 0;
                do
                {
                    //Issue erzeugen
                    m_ret[i3][0].Name = m_ret[i3][0].Get_Name(this.database);
                    m_ret[i3][1].Name = m_ret[i3][1].Get_Name(this.database);
                    string name = this.database.metamodel.issues.m_Issue_Name[26] + m_ret[i3][0].Name+" --> " + m_ret[i3][1].Name;
                    //Issue verknüpfen
                    Issue issue = new Issue(this.database, name, database.metamodel.issues.m_Issue_Note[26], Package_guid_issue, this.repository, true, null);
                    //Issue mit Anforderungen verknüpfen
                    con.Create_Dependency(issue.Classifier_ID, m_ret[i3][0].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, Toolbox_con[0], true);
                    con.Create_Dependency(issue.Classifier_ID, m_ret[i3][1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, Toolbox_con[0], true);


                    i3++;
                } while (i3 < m_ret.Count);
            }

            if (m_ret_replace.Count > 0)
            {
                //Packages erzeugen
                Repository_Element repository_Element = new Repository_Element();
                Repository_Connector con = new Repository_Connector();

                List<string> m_Type_con = new List<string>();
                List<string> m_Stereotype_con = new List<string>();
                List<string> Toolbox_con = new List<string>();
                Toolbox_con.Add("");
                m_Type_con.Add("Abstraction");
                m_Stereotype_con.Add("trace");

                this.Package_guid = repository_Element.Create_Package_Model(this.database.metamodel.m_Package_Name[2], this.repository, this.database);

                string Package_guid_issue = repository_Element.Create_Package(this.database.metamodel.m_Package_Name[19], repository.GetPackageByGuid(this.Package_guid), repository, database);
                repository.GetPackageByGuid(Package_guid).Packages.Refresh();
                repository.GetPackageByGuid(Package_guid).Update();
                repository.GetPackageByGuid(Package_guid_issue).Packages.Refresh();

                int i3 = 0;
                do
                {
                    //Issue erzeugen
                    m_ret_replace[i3][0].Name = m_ret_replace[i3][0].Get_Name(this.database);
                    m_ret_replace[i3][1].Name = m_ret_replace[i3][1].Get_Name(this.database);
                    string name = this.database.metamodel.issues.m_Issue_Name[27] + m_ret_replace[i3][0].Name + " --> " + m_ret_replace[i3][1].Name;
                    //Issue verknüpfen
                    Issue issue = new Issue(this.database, name, database.metamodel.issues.m_Issue_Note[27], Package_guid_issue, this.repository, true, null);
                    //Issue mit Anforderungen verknüpfen
                    con.Create_Dependency(issue.Classifier_ID, m_ret_replace[i3][0].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, Toolbox_con[0], true);
                    con.Create_Dependency(issue.Classifier_ID, m_ret_replace[i3][1].Classifier_ID, m_Stereotype_con, m_Type_con, null, this.repository, this.database, Toolbox_con[0], true);


                    i3++;
                } while (i3 < m_ret_replace.Count);
            }

        }

        #endregion

    }
}

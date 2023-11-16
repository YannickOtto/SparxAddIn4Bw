using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data.OleDb;
using Requirement_Plugin.Interfaces;
using System.Data.Odbc;

using Database_Connection;
using Metamodels;
using Elements;
using Requirement_Plugin;
using Ennumerationen;
using Requirements;

namespace Repsoitory_Elements
{
    public class Abnahmekriterium : Repository_Class
    {
        public string abnahmekriterium;

        public Abnahmekriterium(string id, Database database, EA.Repository repository)
        {
            if (id != null)
            {
                this.Classifier_ID = id;
                this.ID = this.Get_Object_ID(database);
                this.Name = this.Get_Name(database);
                this.Notes = this.Get_Notes(database);
                this.Get_TV_Ábnahmekriteriuem(database, repository);
            }

        }

        #region TV
        private void Get_TV_Ábnahmekriteriuem(Database Data, EA.Repository repository)
        {

            TV_Map help;

            TaggedValue tagged = new TaggedValue(Data.metamodel, Data);
            List<DB_Insert> m_Insert = new List<DB_Insert>();

            // string SQL = "SELECT Value, Notes FROM t_objectproperties WHERE Property = ? AND Object_ID = ?";
            //  string[] output = { "Value", "Notes" };

            //   OleDbCommand oleDbCommand = new OleDbCommand(SQL, Data.oLEDB_Interface.dbConnection);
            //  List<DB_Return> m_TV = Data.oLEDB_Interface.oleDB_SELECT_One_Table_Multiple_Property(oleDbCommand, output, Data.metamodel.SYS_Tagged_Values, this.ID);

            Interface_TaggedValue interface_TaggedValue = new Interface_TaggedValue();

            List<DB_Return> m_TV = interface_TaggedValue.Get_Tagged_Value(Data.metamodel.Nachweisart_Tagged_Values, this.ID, Data);

            if (m_TV != null)
            {
                string Value = "";
                string Note = "";
                string insert = "";
                bool flag_insert = false;
                int i1 = 0;
                do
                {
                    if (m_TV[i1].Ret.Count > 1)
                    {
                        string recent = "";
                        if (m_TV[i1].Ret[1] == null && m_TV[i1].Ret[2] == null)
                        {
                            flag_insert = true;
                        }
                        /*    if (m_TV[i1].Ret[1] == "" && m_TV[i1].Ret[2] == "")
                            {
                                flag_insert = true;
                            }*/
                        else
                        {
                            if (m_TV[i1].Ret[1].ToString() != "<memo>")
                            {
                                recent = (string)m_TV[i1].Ret[1];
                            }
                            else
                            {
                                recent = (string)m_TV[i1].Ret[2];
                            }

                            switch (m_TV[i1].Ret[0])
                            {
                                case "AFO_ABNAHMEKRITERIUM":
                                    this.abnahmekriterium = recent;
                                    break;
                                
                            }
                        }
                    }
                    else
                    {
                        flag_insert = true;
                    }


                    if (flag_insert == true)
                    {
                        switch (m_TV[i1].Ret[0])
                        {
                            case "AFO_ABNAHMEKRITERIUM":
                                this.abnahmekriterium = this.Notes;
                                m_Insert.Add(new DB_Insert("AFO_ABNAHMEKRITERIUM", OleDbType.VarChar, OdbcType.VarChar, this.abnahmekriterium, -1));
                                break;
                        }

                    }

                    i1++;
                } while (i1 < m_TV.Count);

                if (flag_insert == true)
                {
                    //Insert Befehl TV
                    string[] m_Input_Property = { "Object_ID", "Property", "Value", "Notes", "ea_guid" };
                    interface_TaggedValue.Insert_Tagged_Value(Data, m_Insert, tagged, this.ID, m_Input_Property);

                    //Data.oLEDB_Interface.OLEDB_INSERT_One_Table_Multiple_TV(Data, m_Insert, tagged, "t_objectproperties", this.ID, m_Input_Property);
                }
            }
        }
        #endregion
    }
}

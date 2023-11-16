using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAddin
{
    public partial class Requirement_Interface : Form
    {
        public Database Data = new Database();
        public EA.Repository Repository = new EA.Repository();
        public int Index_NodeType_Box = -1;
        public int Index_Target_Box = -1;
        public List<string> Artikel = new List<string>();
        public int NodeType_Artikel_Index = 0;
        public int Target_Artikel_Index = 0;
        public List<Logical> recent_Logical = new List<Logical>();
        public List<InformationElement> recent_InformationElement = new List<InformationElement>();

        public List<string> recent_Text_Senden = new List<string>();
        public List<string> recent_Text_Empfangen = new List<string>();
        //   public List<string> InfoElem_GUID_all = new List<string>();
        // public int Index_NodeType_Box = 0;
        // public int Index_Target_Box = 0;

        private List<NodeType> Recent_m_NodeType = new List<NodeType>();
        private List<Element_Interface> recent_m_ElementInterface = new List<Element_Interface>();
        private List<Element_Interface_Bidirectional> recent_m_ElementInterfaceBidirectional = new List<Element_Interface_Bidirectional>();

        public Requirement_Interface(Database Data, EA.Repository repository)
        {
            this.Data = Data;
            this.Repository = repository;
            this.Artikel = new List<string>();
            Artikel.Add("der");
            Artikel.Add("die");
            Artikel.Add("das");
            Artikel.Add("den");
            Artikel.Add("die");
            Artikel.Add("das");
            Artikel.Add("dem");
            Artikel.Add("der");
            Artikel.Add("dem");
            ////////////////////
            //Text_Senden initialisieren
            recent_Text_Senden.Add("..."); //Artikel
            recent_Text_Senden.Add(" "); //Leer
            recent_Text_Senden.Add("..."); //NodeType
            recent_Text_Senden.Add(" muss fähig sein, Daten("); //Füll
            recent_Text_Senden.Add("..."); //Information Element
            recent_Text_Senden.Add(" ) an "); //Füll2
            recent_Text_Senden.Add("..."); //Artikel
            recent_Text_Senden.Add(" "); //Leer
            recent_Text_Senden.Add("..."); //Target
            recent_Text_Senden.Add(" zu senden."); //Füll3
            //////////////////////
            //Text empfangen initialisieren
            recent_Text_Empfangen.Add("..."); //Artikel
            recent_Text_Empfangen.Add(" "); //Leer
            recent_Text_Empfangen.Add("..."); //Target
            recent_Text_Empfangen.Add(" muss fähig sein, Daten("); //Füll
            recent_Text_Empfangen.Add("..."); //Information Element
            recent_Text_Empfangen.Add(" ) von "); //Füll2
            recent_Text_Empfangen.Add("..."); //Artikel
            recent_Text_Empfangen.Add(" "); //Leer
            recent_Text_Empfangen.Add("..."); //Nodetype
            recent_Text_Empfangen.Add(" zu empfangen."); //Füll3

            InitializeComponent();
        }

        private void Layout_Interface_Load(object sender, EventArgs e)
        {
            /////////////////////////////////////////
            //Anzeigen aller verfügbaren NodeTypes
            XML Element_Name = new XML(); 
            List<string> Element_Name_str = new List<string>();

            NodeType_Box.Items.Clear(); //Alle bisherigen löschen
            Target_NodeType_Box.Items.Clear();
            Szenar_Box.Items.Clear();
            InfoElemBox.Items.Clear();
            NodeType_Artikel.Text = "";
            Target_Artikel.Text = "";

            this.Recent_m_NodeType.Clear();

            if (Data.m_NodeType.Count > 0) //Schleife über alle NodeTypes in der Database
            {
   
                    int i1 = 0;
                    do
                    {
                        
                        //Bedingung, ob Bidirectional oder nicht
                        if (Bidirektional.Checked == true && Data.m_NodeType[i1].m_Element_Interface_Bidirectional.Count > 0)
                        {
                          if (Data.m_NodeType[i1].m_Element_Interface_Bidirectional.Count > 0)
                          {
                            this.Recent_m_NodeType.Add(Data.m_NodeType[i1]);
                            //Namen aus dem Repository suchen[]
                            Element_Name_str = Element_Name.SQL_Query_Select(Repository, "ea_guid", Data.m_NodeType[i1].Classifier_ID, "Name", "t_object");
                            //Hinzufügen des Nmens zu der Listbox
                            NodeType_Box.Items.Add(Element_Name_str[0]);
                          }
                        }
                        else
                        {
                            if(Data.m_NodeType[i1].m_Element_Interface.Count>0 && Bidirektional.Checked == false)
                            {
                                 if (Data.m_NodeType[i1].m_Element_Interface.Count > 0)
                                 {
                                    this.Recent_m_NodeType.Add(Data.m_NodeType[i1]);
                                    //Namen aus dem Repository suchen[]
                                     Element_Name_str = Element_Name.SQL_Query_Select(Repository, "ea_guid", Data.m_NodeType[i1].Classifier_ID, "Name", "t_object");
                                  //Hinzufügen des Nmens zu der Listbox
                                    NodeType_Box.Items.Add(Element_Name_str[0]);
                                 }
                              
                            }
                        }
                        i1++;
                    } while (i1 < Data.m_NodeType.Count);
            }

            NodeType_Box.Sorted = true;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NodeType_Box.Sorted = false;
            Target_NodeType_Box.Sorted = false;
            
            Target_NodeType_Box.Items.Clear();
            Szenar_Box.Items.Clear();
            InfoElemBox.Items.Clear();
            Text_Senden.Clear();
            Text_Empfangen.Clear();
            recent_Text_Empfangen[4] = "...";
            recent_Text_Senden[4] = "...";
            

            recent_m_ElementInterface.Clear();
            recent_m_ElementInterfaceBidirectional.Clear();
            ///////////////////////////////////
            //Initialisierung NodeTypeBox
            NodeType_Artikel.Text = "";
            Target_Artikel.Text = "";

            if(Bidirektional.Checked == false)
            {
                AFo_Titel_Senden.Text = "Senden: ";
                AFo_Titel_Empfangen.Text = "Empfangen: ";
            }
            else
            {
                AFo_Titel_Senden.Text = "Client Austauschen: ";
                AFo_Titel_Empfangen.Text = "Supplier Austauschen: ";
            }


            var length = this.Data.m_NodeType.Count;

          //  MessageBox.Show("Element Interface: " + length.ToString());

            this.Index_NodeType_Box = NodeType_Box.SelectedIndex;

            if (Index_NodeType_Box != -1)
            {
                ///////////////////////////////////////////
                //NodeType Box befüllen
                //AFo Titel Senden & Empfangen mit NodeType und Artikel befüllen
                NodeType_Artikel.Text = Recent_m_NodeType[Index_NodeType_Box].W_Artikel;
                NodeType_Artikel_Index = this.Artikel.FindIndex(x => x == Recent_m_NodeType[Index_NodeType_Box].W_Artikel);
                if (Bidirektional.Checked == false)
                {
                    AFo_Titel_Senden.Text = "Senden: " + Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository) + " - ";
                    AFo_Titel_Empfangen.Text = "Empfangen: ... - " + Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository);
                }
                else
                {
                    AFo_Titel_Senden.Text = "Client Austauschen: " + Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository) + " - ";
                    AFo_Titel_Empfangen.Text = "Supplier Austauschen: ... - " + Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository);
                }
                ///////////////////////////////////////////
                //Text Box befüllen 
                //Artikel als Hilfe
                string help = Recent_m_NodeType[Index_NodeType_Box].W_Artikel;
                //AFo Text Senden befüllen
                recent_Text_Senden[0]= help.ToString();
                recent_Text_Senden[2] = Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository);
                //AFo Text Empfangen befüllen
                recent_Text_Empfangen[6] = Artikel[NodeType_Artikel_Index + 6];
                recent_Text_Empfangen[8] = Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository);
                //Beide Text Box schreiben
                Data.Write_List(Text_Senden, recent_Text_Senden);
                Data.Write_List(Text_Empfangen, recent_Text_Empfangen);
                /////////////////////////////////////////
                XML Element_Name = new XML();
                List<string> Element_Name_str = new List<string>();
                //Alle Targets des NodeType finden über Element_Interface und in Target Box Schreiben


                if (Recent_m_NodeType[Index_NodeType_Box].m_Element_Interface.Count > 0 && Bidirektional.Checked == false) //Schleife über alle Element_Interface der NodeType
                {
                    int i2 = 0;
                    do
                    {   //Target Namen aus dem Repository suchen
                        Element_Name_str = Element_Name.SQL_Query_Select(Repository, "ea_guid", Recent_m_NodeType[Index_NodeType_Box].m_Element_Interface[i2].Target_Classifier_ID, "Name", "t_object");
                        //Hinzufügen des Nmens zu der Listbox
                        Target_NodeType_Box.Items.Add(Element_Name_str[0]);

                        recent_m_ElementInterface.Add(Recent_m_NodeType[Index_NodeType_Box].m_Element_Interface[i2]);

                        i2++;
                    } while (i2 < Recent_m_NodeType[Index_NodeType_Box].m_Element_Interface.Count);
                }
                if(Recent_m_NodeType[Index_NodeType_Box].m_Element_Interface_Bidirectional.Count > 0 && Bidirektional.Checked == true)
                {
                    int i2 = 0;
                    do
                    {   //Target Namen aus dem Repository suchen
                        Element_Name_str = Element_Name.SQL_Query_Select(Repository, "ea_guid", Recent_m_NodeType[Index_NodeType_Box].m_Element_Interface_Bidirectional[i2].Target_Classifier_ID, "Name", "t_object");
                        //Hinzufügen des Nmens zu der Listbox
                        Target_NodeType_Box.Items.Add(Element_Name_str[0]);

                        recent_m_ElementInterfaceBidirectional.Add(Recent_m_NodeType[Index_NodeType_Box].m_Element_Interface_Bidirectional[i2]);

                        i2++;
                    } while (i2 < Recent_m_NodeType[Index_NodeType_Box].m_Element_Interface_Bidirectional.Count);

                }
            }

            NodeType_Box.Sorted = true;
            Target_NodeType_Box.Sorted = true;


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            /* nicht genutzt*/
        }

        private void Target_NodeType_Box_SelectedIndexChanged(object sender, EventArgs e)
        {
           

            //Szenar und InfoElem löschen
            Szenar_Box.Items.Clear();
            InfoElemBox.Items.Clear();
            
           
            //aktuelle Indices erhalte
            this.Index_Target_Box =  Target_NodeType_Box.SelectedIndex; //Target NodeType
            this.Index_NodeType_Box= NodeType_Box.SelectedIndex; // NodeType

          //  MessageBox.Show(Index_Target_Box.ToString());
            ///////////////////////////////////////////////////////////
           //Überprüfung ob wirklich was angewählt wurde
            if (Index_Target_Box != -1 && Index_NodeType_Box != -1)
            {
                ////////////
                //Target Artikel 
               
               // NodeType Target_NodeType = Data.Check_NodeType(recent_m_ElementInterface[Index_Target_Box].Target_Classifier_ID);

             
                if (Bidirektional.Checked == false && recent_m_ElementInterface.Count > 0)
                {
                    AFo_Titel_Senden.Text = "Senden: " + Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository) + " - " + Data.Get_Name_t_object_GUID(recent_m_ElementInterface[Index_Target_Box].Target_Classifier_ID, Repository);
                    AFo_Titel_Empfangen.Text = "Empfangen: " + Data.Get_Name_t_object_GUID(recent_m_ElementInterface[Index_Target_Box].Target_Classifier_ID, Repository) + " - " + Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository);

                }
                if(Bidirektional.Checked == true && recent_m_ElementInterfaceBidirectional.Count > 0)
                {
                    AFo_Titel_Senden.Text = "Client Austauschen: " + Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository) + " - " + Data.Get_Name_t_object_GUID(recent_m_ElementInterfaceBidirectional[Index_Target_Box].Target_Classifier_ID, Repository);
                    AFo_Titel_Empfangen.Text = "Supplier Austauschen: " + Data.Get_Name_t_object_GUID(recent_m_ElementInterfaceBidirectional[Index_Target_Box].Target_Classifier_ID, Repository) + " - " + Data.Get_Name_t_object_GUID(Recent_m_NodeType[Index_NodeType_Box].Classifier_ID, Repository);
                }



                ////////
                //Text Box befüllen
                string help = "";
                NodeType Target_NodeType;

                if (Bidirektional.Checked == false)
                {
                    help = Data.Check_NodeType(recent_m_ElementInterface[Index_Target_Box].Target_Classifier_ID).W_Artikel;

                    recent_Text_Empfangen[0] = help.ToString();
                    recent_Text_Empfangen[2] = Data.Get_Name_t_object_GUID(recent_m_ElementInterface[Index_Target_Box].Target_Classifier_ID, Repository);

                    recent_Text_Senden[6] = Artikel[Target_Artikel_Index + 6];
                    recent_Text_Senden[8] = Data.Get_Name_t_object_GUID(recent_m_ElementInterface[Index_Target_Box].Target_Classifier_ID, Repository);


                    Target_NodeType = Data.Check_NodeType(recent_m_ElementInterface[Index_Target_Box].Target_Classifier_ID);
                }
                else
                {
                    help = Data.Check_NodeType(recent_m_ElementInterfaceBidirectional[Index_Target_Box].Target_Classifier_ID).W_Artikel;

                    recent_Text_Empfangen[0] = help.ToString();
                    recent_Text_Empfangen[2] = Data.Get_Name_t_object_GUID(recent_m_ElementInterfaceBidirectional[Index_Target_Box].Target_Classifier_ID, Repository);

                    recent_Text_Senden[6] = Artikel[Target_Artikel_Index + 6];
                    recent_Text_Senden[8] = Data.Get_Name_t_object_GUID(recent_m_ElementInterfaceBidirectional[Index_Target_Box].Target_Classifier_ID, Repository);

                    Target_NodeType = Data.Check_NodeType(recent_m_ElementInterfaceBidirectional[Index_Target_Box].Target_Classifier_ID);
                }
                


                ////////
                //label_Supplier Artikel anzeigen
                if (Target_NodeType != null)
                {
                    Target_Artikel.Text = Target_NodeType.W_Artikel;
                    Target_Artikel_Index =  this.Artikel.FindIndex(x => x == Target_NodeType.W_Artikel);
                }
                
                //////////////////////////////////////////////
                //Alle Info Elemente anzeigen
                //Alle "Szenare" erhalten und in Szenar Box anzeigen
                List<Target> Targets = new List<Target>();
                List<string> Logicals = new List<string>();
                List<string> InfoElems = new List<string>();

                //Alle Logical_ID zum Element_Interface erhalten
                if(Bidirektional.Checked == false)
                {
                    Logicals = recent_m_ElementInterface[Index_Target_Box].Get_All_Logical();
                    recent_Logical = recent_m_ElementInterface[Index_Target_Box].Get_All_Logicals();
                    //Alle InformationElemnt zum Element_Interface erhalten
                    InfoElems = recent_m_ElementInterface[Index_Target_Box].Get_All_Information_Element_GUID();
                }
                else
                {

                    //Alle ID der Logicals
                    var m_Client = recent_m_ElementInterfaceBidirectional[Index_Target_Box].m_Logical_Client;
                    var m_Supplier = recent_m_ElementInterfaceBidirectional[Index_Target_Box].m_Logical_Supplier;

                    var Joined_Logicals = from List1 in m_Client join List2 in m_Supplier on List1 equals List2 select List1;

                    var Joined_Logicals2 = Joined_Logicals.Distinct().ToList();

                    recent_Logical = Joined_Logicals2;

                 //   MessageBox.Show("Anzahl vorher: " + Joined_Logicals.Count().ToString()+"\r\nAnzahl danach: "+ Joined_Logicals2.Count+ " "+ Joined_Logicals2[0].Logical_ID);

                    if (Joined_Logicals2.Count > 0)
                    {
                        int i1 = 0;
                        do
                        {
                            Logicals.Add(Joined_Logicals2[i1].Logical_ID);

                            i1++;
                        } while (i1 < Joined_Logicals2.Count);
                    }
                    //Alle ID der Info ELems
                    InfoElems = recent_m_ElementInterfaceBidirectional[Index_Target_Box].Get_All_InfoElemGUID();

                }

                string Write_InfoElem = "";
                int i2 = 0;


                if (InfoElems.Count > 0)
                {

                    do
                    {
                        if (i2 == 0)
                        {
                            Write_InfoElem = Data.Get_Name_t_object_GUID(InfoElems[i2], this.Repository);
                        }
                        else
                        {
                            Write_InfoElem = Write_InfoElem + ", " + Data.Get_Name_t_object_GUID(InfoElems[i2], this.Repository);
                        }


                        i2++;
                    } while (i2 < InfoElems.Count);
                }
                else
                {
                    Write_InfoElem = "keine";
                }
               

                recent_Text_Senden[4] = Write_InfoElem;
                recent_Text_Empfangen[4] = Write_InfoElem;
                //Beide Text Box schreiben
                Data.Write_List(Text_Senden, recent_Text_Senden);
                Data.Write_List(Text_Empfangen, recent_Text_Empfangen);
                //////////////
                //Alle Logicals darstellen in Szenar_Box
                //   MessageBox.Show("Anzahl Szenare: " + Logicals.Count.ToString());
                if (Logicals.Count > 0)
                {
                    Szenar_Box.Sorted = false;
                    int i4 = 0;
                    do
                    {
                        //Namen der Logical erhalten
                        string print = "kein";

                        if (Logicals[i4] != "kein")
                        {
                            print = Data.Get_Name_t_object_GUID(Logicals[i4], Repository);
                        }
                        //Einfügen des Namens
                        Szenar_Box.Items.Add(print);


                   //     MessageBox.Show(i4.ToString());

                        Szenar_Box.SetItemChecked(i4, true);
                        

                        i4++;
                    } while (i4 < Logicals.Count);
                    //ListBox sortieren
                    Szenar_Box.Sorted = true;
                    //
                }
                
                ////////////
                //Alle Info Elem darstellen
             //   MessageBox.Show("Anzahl InfoElem: " + InfoElems.Count.ToString());
                if (InfoElems.Count > 0)
                {
                    InfoElemBox.Sorted = false;
                    
                    int i3 = 0;
                    do
                    {

                        //Namen erhalten
                        string print = Data.Get_Name_t_object_GUID(InfoElems[i3], Repository);
                        //Namen in Lsitbox einfügen
                        //   MessageBox.Show("Info Elem " + i2 + ": " + print);
                        InfoElemBox.Items.Add(print);
                        InfoElemBox.SetItemChecked(i3, true);

                    

                        i3++;
                    } while (i3 < InfoElems.Count);

                    InfoElemBox.Sorted = true;
                    
    
                }
                
            }
            
        }
        

        private void InfoElemBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            InfoElemBox.Sorted = false;
            
            int Changed_Index = InfoElemBox.SelectedIndex;

            List<string> Info_Elem_GUID = new List<string>();
            List<string> Checked_Info_Elem_GUID = new List<string>();


            if(Changed_Index != -1)
            {
                ///////////////
                //Checked Status setzen
                bool Checked_Index = InfoElemBox.GetItemChecked(Changed_Index);

                if (Checked_Index == true)
                {
                    InfoElemBox.SetItemChecked(Changed_Index, false);
                }
                else
                {
                    InfoElemBox.SetItemChecked(Changed_Index, true);
                }

                //Uncheck Szenar_Box
                Data.Uncheck_CheckedListbox(Szenar_Box);
                //Alle InformationElemnt in der DAtabase
                List<InformationElement> m_InformationElement = Data.m_InformationElement;
                //Aktuelle InfoElem
                List<InformationElement> recent_InformationElement = new List<InformationElement>(); //Object
                List<string> Info_Names = new List<string>(); //Name
                //Aktuelle Logical
                List<Logical> recent_Logical = new List<Logical>(); //Objekt
                List<string> recent_Logical_Names = new List<string>();//Name

                //Indices der geteickten Element in InfoBox
                var Checked_Indices = InfoElemBox.CheckedIndices;

                if (Checked_Indices.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        Info_Names.Add(InfoElemBox.Items[Checked_Indices[i1]].ToString());

                        i1++;
                    } while (i1 < Checked_Indices.Count);
                }
                ///
                //InfoElem zu Namen finden
                if (m_InformationElement.Count > 0)
                {
                    //Schleife über alle InfoElem --> InfoElem finden
                    int i1 = 0;
                    do
                    {
                        if(Info_Names.Contains(Data.Get_Name_t_object_GUID(m_InformationElement[i1].InformationItem_ID, Repository)) == true)
                        {   
                            //Zu aktuellen InfoElem hinzufügen
                            recent_InformationElement.Add(m_InformationElement[i1]);
                        }

                        i1++;
                    } while (i1 < m_InformationElement.Count);
                }


                /////////////////////////////////////
                //recent Logical suchen
                //var recent_ElementInterface = new Element_Interface();
                List<Target> m_Target = new List<Target>();

                if (Bidirektional.Checked == false)
                {
                    m_Target = recent_m_ElementInterface[Index_Target_Box].m_Target;
                }
                else
                {
                    m_Target = recent_m_ElementInterfaceBidirectional[Index_Target_Box].m_Target;
                }
                 

                if (m_Target.Count > 0)
                {
                    int i1 = 0;
                    //Schleife über alle Targets des Element_Interface
                    do
                    {
                        Target recent_Target = m_Target[i1];
                        
                        if(recent_Target.m_Information_Element.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                if(recent_InformationElement.Contains(recent_Target.m_Information_Element[i2]) == true)
                                {
                                    if(recent_Logical.Contains(recent_Target.Logical) == false)
                                    {
                                        recent_Logical.Add(recent_Target.Logical);
                                        
                                        if(recent_Target.Logical.Logical_ID != "kein")
                                        {
                                          //  MessageBox.Show(Data.Get_Name_t_object_GUID(recent_Target.Logical.Logical_ID, Repository));
                                            recent_Logical_Names.Add(Data.Get_Name_t_object_GUID(recent_Target.Logical.Logical_ID, Repository));
                                        }
                                        else
                                        {
                                          //  MessageBox.Show((recent_Target.Logical.Logical_ID));
                                            recent_Logical_Names.Add(recent_Target.Logical.Logical_ID);
                                        }
                                    }
                                }

                                i2++;
                            } while (i2 < recent_Target.m_Information_Element.Count);
                        }


                        i1++;
                    } while (i1 < m_Target.Count);
                    
                }
                //Namen derSzenar
                var Szenar_Items = Szenar_Box.Items;
                List<string> Szenar_Names = new List<string>();

                if (Szenar_Items.Count > 0)
                {
                    int i1 = 0;
                    do
                    {
                        if(recent_Logical_Names.Contains(Szenar_Items[i1].ToString()) == true)
                        {
                           Szenar_Box.SetItemChecked(i1, true);
                        }
                        i1++;
                    } while (i1 < Szenar_Items.Count);
                }

                InfoElemBox.ClearSelected();
                
            }
           
            InfoElemBox.Sorted = true;
        }

        private void Szenar_Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            Szenar_Box.Sorted = false;

            int Changed_Index = Szenar_Box.SelectedIndex;
            int Index_Target_Box = Target_NodeType_Box.SelectedIndex; //Target NodeType
            int Index_NodeType_Box = NodeType_Box.SelectedIndex; // NodeType


            List<string> Index_Szenar_Box = new List<string>();
            List<Target> Targets = new List<Target>();

            if (Changed_Index != -1)
            {

                ///////////////
                //Checked Status setzen
                bool Checked_Index = Szenar_Box.GetItemChecked(Changed_Index);

                if (Checked_Index == true)
                {
                    Szenar_Box.SetItemChecked(Changed_Index, false);
                }
                else
                {
                    Szenar_Box.SetItemChecked(Changed_Index, true);
                }
                //////////////////
                //Indices der markierten Szenar erhalten
                var Name_Szenar = Szenar_Box.CheckedIndices;

                if (Name_Szenar.Count > 0)
                {
                    var i1 = 0;
                    do
                    {
                        // MessageBox.Show(Name_Szenar[i1].ToString());

                        //Index_Szenar_Box.Add(Name_Szenar[i1].ToString());
                        Index_Szenar_Box.Add(Szenar_Box.Items[Name_Szenar[i1]].ToString());
                        i1++;
                    } while (i1 < Name_Szenar.Count);
                    //Alle Target erhalten, welche gewählt wurden
                    if(Bidirektional.Checked == false)
                    {
                        Targets = recent_m_ElementInterface[Index_Target_Box].Get_Target_Szenar_Name(Index_Szenar_Box, Repository);
                    }
                    else
                    {
                        Targets = recent_m_ElementInterfaceBidirectional[Index_Target_Box].Get_Target_Szenar_Name(Index_Szenar_Box, Repository);

                       // List<Logical> m_Logical 
                    }
                    
                    }

                    List<string> Info_Elem_GUID_Szenar = new List<string>();
                    List<string> Info_Elem_GUID = new List<string>();

                    if (Bidirektional.Checked == false)
                    {
                        //Alle Info Elem zu den gewählten Targets erhalten
                        Info_Elem_GUID_Szenar = recent_m_ElementInterface[Index_Target_Box].Szenar_Merge_All_Information_Elemen_GUID(Targets, Repository);
                        //Alle InfoElem des Element_Interface erhalten
                        Info_Elem_GUID = recent_m_ElementInterface[Index_Target_Box].Get_All_Information_Element_GUID();
                    }
                    else
                    {
                        //Alle Info Elem zu den gewählten Targets erhalten
                        Info_Elem_GUID_Szenar = recent_m_ElementInterfaceBidirectional[Index_Target_Box].Szenar_Merge_All_Information_Elemen_GUID(Targets, Repository);
                        //Alle InfoElem des Element_Interface erhalten
                        Info_Elem_GUID = recent_m_ElementInterfaceBidirectional[Index_Target_Box].Get_All_Information_Element_GUID();
                    }



                //Setzen, wenn aktuelle GUID in Szenar enthalten
                int i2 = 0;
                        do
                        {
                            int recent_Index = InfoElemBox.Items.IndexOf(Data.Get_Name_t_object_GUID(Info_Elem_GUID[i2],Repository));
                        //    MessageBox.Show(recent_Index.ToString());

                            if (Info_Elem_GUID_Szenar.Contains(Info_Elem_GUID[i2]) == true)
                            {
                                InfoElemBox.SetItemChecked(recent_Index, true);
                            }
                            else
                            {
                                InfoElemBox.SetItemChecked(recent_Index, false);
                            }

                            i2++;
                        } while (i2 < Info_Elem_GUID.Count);


                if(Bidirektional.Checked == true)
                {
                    NodeType help3 = this.Data.Check_NodeType(recent_m_ElementInterfaceBidirectional[Index_Target_Box].Target_Classifier_ID);

                    Check_Bidirektional_Szenar(help3, recent_m_ElementInterfaceBidirectional[Index_Target_Box], Info_Elem_GUID_Szenar, Targets);
                }


                string Write_InfoElem = "";
                int i5 = 0;

                if(Info_Elem_GUID_Szenar.Count > 0)
                {
                    do
                    {
                        if (i5 == 0)
                        {
                            Write_InfoElem = Data.Get_Name_t_object_GUID(Info_Elem_GUID_Szenar[i5], this.Repository);
                        }
                        else
                        {
                            Write_InfoElem = Write_InfoElem + ", " + Data.Get_Name_t_object_GUID(Info_Elem_GUID_Szenar[i5], this.Repository);
                        }


                        i5++;
                    } while (i5 < Info_Elem_GUID_Szenar.Count);
                }
                else
                {
                    Write_InfoElem = "keine";
                }



                recent_Text_Senden[4] = Write_InfoElem;
                recent_Text_Empfangen[4] = Write_InfoElem;

                //Beide Text Box schreiben
                Data.Write_List(Text_Senden, recent_Text_Senden);
                Data.Write_List(Text_Empfangen, recent_Text_Empfangen);

            }


          



            Szenar_Box.ClearSelected();

                Szenar_Box.Sorted = false;
        }

        private void NodeType_Artikel_Click(object sender, EventArgs e)
        {
            int NodeType_Index = NodeType_Box.SelectedIndex;

            switch( NodeType_Artikel_Index)
            {
                case 0:
                    NodeType_Artikel.Text = Artikel[1];
                    NodeType_Artikel_Index++;
                    break;
                case 1:
                    NodeType_Artikel.Text = Artikel[2];
                    NodeType_Artikel_Index++;
                    break;
                case 2:
                    NodeType_Artikel.Text = Artikel[0];
                    NodeType_Artikel_Index = 0;
                    break;
            }
            if (NodeType_Box.SelectedIndex != -1)
            {
                ////
                //Artikel abspeichern
                Recent_m_NodeType[NodeType_Index].W_Artikel = Artikel[NodeType_Artikel_Index];
                recent_Text_Senden[0] = Artikel[NodeType_Artikel_Index];
                recent_Text_Empfangen[6] = Artikel[NodeType_Artikel_Index+6];
                //Textbox refreshen
                Data.Write_List(Text_Senden, recent_Text_Senden);
                Data.Write_List(Text_Empfangen, recent_Text_Empfangen);
            }
        }

        private void Target_Artikel_Click(object sender, EventArgs e)
        {
            int NodeType_Index = NodeType_Box.SelectedIndex;
            int Target_Index = Target_NodeType_Box.SelectedIndex;


            switch (Target_Artikel_Index)
            {
                case 0:
                    Target_Artikel.Text = Artikel[1];
                    Target_Artikel_Index++;
                    break;
                case 1:
                    Target_Artikel.Text = Artikel[2];
                    Target_Artikel_Index++;
                    break;
                case 2:
                    Target_Artikel.Text = Artikel[0];
                    Target_Artikel_Index = 0;
                    break;
            }

            if (NodeType_Box.SelectedIndex != -1 && Target_NodeType_Box.SelectedIndex != -1)
            { 
                Data.Check_NodeType(Recent_m_NodeType[NodeType_Index].m_Element_Interface[Target_Index].Target_Classifier_ID).W_Artikel = Artikel[NodeType_Artikel_Index];
                recent_Text_Senden[6] = Artikel[Target_Artikel_Index+3];
                recent_Text_Empfangen[0] = Artikel[Target_Artikel_Index];
                //Textbox refreshen
                Data.Write_List(Text_Senden, recent_Text_Senden);
                Data.Write_List(Text_Empfangen, recent_Text_Empfangen);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
        private void splitContainer7_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer8_Panel1_Paint(object sender, PaintEventArgs e)
        {


        }

        private void Bidirektional_CheckedChanged(object sender, EventArgs e)
        {
            //Alles zurücksetzen
            NodeType_Box.Items.Clear();
            Target_NodeType_Box.Items.Clear();
            Text_Senden.Clear();
            Text_Empfangen.Clear();
            Target_NodeType_Box.Sorted = false;
            NodeType_Box.Sorted = false;

            //Neu laden
            Layout_Interface_Load(sender, e);

            if (Bidirektional.Checked == true)
            {
                label_Client.Text = "Client, bidirektional";
                label_Supplier.Text = "Supplier, bidirektional";
                AFo_Titel_Senden.Text = "Austauschen Client: ";
                AFo_Titel_Empfangen.Text = "Austauschen Supplier: ";

                recent_Text_Senden[9] = " auszutauschen.";
                recent_Text_Empfangen[9] = " auszutauschen.";
                recent_Text_Senden[5] = ") mit ";
                recent_Text_Empfangen[5] = ") mit ";

                InfoElemBox.Enabled = false;

            }
            else
            {
                label_Client.Text = "Client";
                label_Supplier.Text = "Supplier";
                AFo_Titel_Senden.Text = "Senden: ";
                AFo_Titel_Empfangen.Text = "Empfangen: ";

                recent_Text_Senden[9] = " zu senden.";
                recent_Text_Empfangen[9] = " zu empfangen.";
                recent_Text_Senden[5] = ") an ";
                recent_Text_Empfangen[5] = ") von ";

                InfoElemBox.Enabled = true;
            }
            
        }




        /////////////////////
        private void Check_Bidirektional_Szenar(NodeType NodeType, Element_Interface_Bidirectional elem, List<string> m_InfoElem, List<Target> m_target)
        {
            Element_Interface_Bidirectional Supplier;

            Supplier =  NodeType.Check_ElementInterfaceBidirectional(elem.Target_Classifier_ID, elem.Classifier_ID);

            

          //  MessageBox.Show(Supplier.m_Target.Count.ToString());

            if(Supplier.m_Target.Count > 0 && m_InfoElem.Count > 0)
            {
                List<Logical> m_Logical = new List<Logical>();
                List<InformationElement> m_InformationElement = new List<InformationElement>();

                int i1 = 0;
                do
                {
                    //MessageBox.Show(Supplier.Get_All_Logicals().Count.ToString());

                    if( Supplier.Get_All_Logicals().Contains(m_target[i1].Logical) == true)
                    {
                        m_Logical.Add(m_target[i1].Logical);
                    }

                    i1++;
                } while (i1 < m_target.Count);

                //MessageBox.Show("Logicals hinzugefügt");

                if(m_Logical.Count > 0)
                {

                    int i2 = 0;
                    do
                    {
                        if(m_Logical.Contains(Supplier.m_Target[i2].Logical) == true)
                        {
                            this.Data.Add_Distinct_InformationElement(m_InformationElement, Supplier.m_Target[i2].m_Information_Element);
                        }

                        i2++;
                    } while (i2 < Supplier.m_Target.Count);


                   // MessageBox.Show("InfoElem hinzugefügt");

                    if(m_InformationElement.Count > 0)
                    {
                        List<string> m_InfoGUID = new List<string>();

                        int i3 = 0;
                        do
                        {
                            if(m_InformationElement.Contains(this.Data.Check_InformationElement(m_InfoElem[i3])) == false)
                            {
                                m_InfoGUID.Add(m_InfoElem[i3]);
                            }

                            i3++;
                        } while (i3 < m_InfoElem.Count);

                        MessageBox.Show("Abgleich InfoElem durchgeführt");

                        if(m_InfoGUID.Count > 0)
                        {
                            string help = "";
                            int i4 = 0;
                            do
                            {
                                help = help + "\r\n- " + this.Data.Get_Name_t_object_GUID(m_InfoGUID[i4], this.Repository);

                                i4++;
                            } while (i4 < m_InfoGUID.Count);

                            MessageBox.Show("Die aktuelle Szenar Konstellation ist für folgende InformationsElement\r\nfür den bidirektionalen Austausch nicht korrekt:"+help);
                        }
                    }

                }
            }


        }

        private void splitContainer8_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    }

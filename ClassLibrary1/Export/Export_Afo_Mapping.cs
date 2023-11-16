using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data.Common;
using Microsoft.Office.Interop.Excel;

namespace Requirement_Plugin.Export
{
    public class Export_Afo_Mapping
    {
        private Export_excel export_excel;
        private int max_column_sys_aufbruch;
        private int max_row;
        private int max_column;
        private int max_column_offset;
        private int column_sysauf_sys_id;


        private List<Color> m_color = new List<Color>();
        private List<Color> m_color_font = new List<Color>();

        private List<Color> m_color_req = new List<Color>();
        private List<Color> m_color_font_req = new List<Color>();

        public Export_Afo_Mapping()
        {
            export_excel = new Export_excel();

            column_sysauf_sys_id = 2;
            max_column_sys_aufbruch = 2;

            m_color.Add(Color.DarkBlue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.Blue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.RoyalBlue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.DodgerBlue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.DeepSkyBlue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.SkyBlue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.LightSkyBlue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.LightSteelBlue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.PowderBlue);
            m_color_font.Add(Color.White);
            m_color.Add(Color.LightCyan);
            m_color_font.Add(Color.White);

            max_column_offset = 6;

            m_color_req.Add(Color.LightGray);
            m_color_font_req.Add(Color.Black);
            m_color_req.Add(Color.LightGray);
            m_color_font_req.Add(Color.Black);
        }

        ~Export_Afo_Mapping()
        {

        }

        public void Export(Repsoitory_Elements.Capability catalogue, Database database)
        {

            object missing = Type.Missing;
            this.export_excel.Create_File();

            Excel.Workbook oWB = this.export_excel.oWB;

            

            Excel.Worksheet oWS_Sys_Afo = oWB.Sheets.Add(missing, missing, 1, missing) as Excel.Worksheet;

            oWS_Sys_Afo.Name = "Mapping Sys Afo";

            /////////////////////////////////
            //Beschreiben Excel Tabelle
            #region Systemaufbruch
            if (database.m_SysElemente.Count > 0)
            {
                Excel.Worksheet oWS_Sytemaufbruch = oWB.ActiveSheet as Excel.Worksheet;

                oWS_Sytemaufbruch.Name = "Systemaufbruch";

                List<Repsoitory_Elements.SysElement> m_sort_sys = database.m_SysElemente.OrderBy(x => x.SYS_AG_ID).ToList().Where(x => x.m_Parent.Count == 0).ToList();

                int row = 2;
                int column = 2;
                int ebene = 1;

                if (m_sort_sys.Count > 0)
                {
                    List<string> m_save = new List<string>();

                    this.export_excel.Write_Cell_string(1, column_sysauf_sys_id, "SYS_AG_ID", oWS_Sytemaufbruch);

                    int i1 = 0;
                    do
                    {
                        if(m_save.Contains(m_sort_sys[i1].Classifier_ID) == false)
                        {

                            m_save.Add(m_sort_sys[i1].Classifier_ID);

                            this.export_excel.Write_Cell_string(row, column_sysauf_sys_id, m_sort_sys[i1].SYS_AG_ID, oWS_Sytemaufbruch);
                            this.export_excel.Write_Cell_string(row, column + 1, m_sort_sys[i1].Name, oWS_Sytemaufbruch);

                          

                            row++;

                            row = Insert_Sys_Child(row, column, m_sort_sys[i1], oWS_Sytemaufbruch, m_save);

                            if((column+1) > max_column_sys_aufbruch)
                            {
                                max_column_sys_aufbruch = column + 1;
                            }
                        }

                        i1++;
                    } while (i1 < m_sort_sys.Count);
                }

                oWS_Sytemaufbruch.UsedRange.Columns.AutoFit();
                oWS_Sytemaufbruch.UsedRange.Rows.AutoFit();

            }

            #endregion Systemaufbruch

            #region Sys Afo
            if (database.m_SysElemente.Count > 0)
            {
                Excel.Worksheet oWS_SysAfo = oWB.Sheets.Add(missing, missing,1,missing) as Excel.Worksheet;
                oWS_SysAfo.Name = "Mapping System Anforderung";

                List<Repsoitory_Elements.SysElement> m_sort_sys = database.m_SysElemente.OrderBy(x => x.SYS_AG_ID).ToList().Where(x => x.m_Parent.Count == 0).ToList();

                int row = 2;
                int column = 2;
                int ebene = 1;

                if (m_sort_sys.Count > 0)
                {
                    List<string> m_save2 = new List<string>();

                    this.export_excel.Write_Cell_string(1, column_sysauf_sys_id, "SYS_AG_ID", oWS_SysAfo);

                    this.export_excel.Write_Cell_string(1, max_column_sys_aufbruch + 2, "AFO_AG_ID", oWS_SysAfo);
                    this.export_excel.Write_Cell_string(1, max_column_sys_aufbruch + 3, "AFO_Titel", oWS_SysAfo);
                    this.export_excel.Write_Cell_string(1, max_column_sys_aufbruch + 4, "AFO_Text", oWS_SysAfo);

                    int i1 = 0;
                    do
                    {
                        if (m_save2.Contains(m_sort_sys[i1].Classifier_ID) == false)
                        {

                            m_save2.Add(m_sort_sys[i1].Classifier_ID);

                            this.export_excel.Write_Cell_string(row, column_sysauf_sys_id, m_sort_sys[i1].SYS_AG_ID, oWS_SysAfo);
                            this.export_excel.Write_Cell_string(row, column + 1, m_sort_sys[i1].Name, oWS_SysAfo);

                            row++;
                            //Afo hinzufügen
                            List<Requirements.Requirement> m_req = database.m_Requirement.Where(x => x.sysElements.Select(y => y.Classifier_ID).Contains(m_sort_sys[i1].Classifier_ID)).ToList();

                            m_req = m_req.OrderBy(x => x.AFO_AG_ID).ToList();

                            if (m_req.Count > 0)
                            {
                                int i2 = 0;
                                do
                                {
                                    this.export_excel.Write_Cell_string(row, max_column_sys_aufbruch+2, m_req[i2].AFO_AG_ID, oWS_SysAfo);
                                    this.export_excel.Write_Cell_string(row, max_column_sys_aufbruch + 3, m_req[i2].AFO_TITEL, oWS_SysAfo);
                                    this.export_excel.Write_Cell_string(row, max_column_sys_aufbruch + 4, m_req[i2].AFO_TEXT, oWS_SysAfo);

                                    row++;
                                    i2++;
                                } while (i2 < m_req.Count);
                            }
                            

                            row = Insert_SysAfo_Child(row, column, m_sort_sys[i1], oWS_SysAfo, m_save2, database);

                           
                        }

                        i1++;
                    } while (i1 < m_sort_sys.Count);
                }

                oWS_SysAfo.UsedRange.Columns.AutoFit();
                oWS_SysAfo.UsedRange.Rows.AutoFit();

            }
            #endregion Sys Afo

            #region Afo Sys Mapping
            if(database.m_Requirement.Count > 0)
            {
                Excel.Worksheet oWS_AfoSys = oWB.Sheets.Add(missing, missing, 1, missing) as Excel.Worksheet;
                oWS_AfoSys.Name = "Mapping Anforderung System";

                Export_TFL(catalogue, oWS_AfoSys);
            }

            #endregion


            this.export_excel.Save_File();

            MessageBox.Show("Export Anforderung Mapping abgeschlossen.");
        }

        #region Systemaufbruch
        private int Insert_Sys_Child(int row, int column, Repsoitory_Elements.SysElement parent, Excel.Worksheet oWS, List<string> m_save)
        {
            int column_new = column;

            if(parent.m_Child.Count > 0)
            {
                column_new++;

                List<Repsoitory_Elements.SysElement> m_sort_sys = parent.m_Child.OrderBy(x => x.SYS_AG_ID).ToList();

                int i1 = 0;
                do
                {
                    if (m_save.Contains(parent.m_Child[i1].Classifier_ID) == false)
                    {
                        m_save.Add(parent.m_Child[i1].Classifier_ID);

                        this.export_excel.Write_Cell_string(row, column_sysauf_sys_id, parent.m_Child[i1].SYS_AG_ID, oWS);
                        this.export_excel.Write_Cell_string(row, column_new + 1, parent.m_Child[i1].Name, oWS);

                        row++;

                        row = Insert_Sys_Child(row, column_new, parent.m_Child[i1], oWS, m_save);

                        if ((column_new + 1) > max_column_sys_aufbruch)
                        {
                            max_column_sys_aufbruch = column_new + 1;
                        }
                    }

                    i1++;
                } while (i1 < parent.m_Child.Count);
            }


            return (row);
        }
        #endregion

        #region Sys Afo Mapping
        private int Insert_SysAfo_Child(int row, int column, Repsoitory_Elements.SysElement parent, Excel.Worksheet oWS, List<string> m_save, Database database)
        {
            int column_new = column;

            if (parent.m_Child.Count > 0)
            {
                column_new++;

                List<Repsoitory_Elements.SysElement> m_sort_sys = parent.m_Child.OrderBy(x => x.SYS_AG_ID).ToList();

                int i1 = 0;
                do
                {
                    if (m_save.Contains(parent.m_Child[i1].Classifier_ID) == false)
                    {
                        m_save.Add(parent.m_Child[i1].Classifier_ID);

                        this.export_excel.Write_Cell_string(row, column_sysauf_sys_id, parent.m_Child[i1].SYS_AG_ID, oWS);
                        this.export_excel.Write_Cell_string(row, column_new + 1, parent.m_Child[i1].Name, oWS);

                        row++;

                        //Afo hinzufügen
                        List<Requirements.Requirement> m_req = database.m_Requirement.Where(x => x.sysElements.Select(y => y.Classifier_ID).Contains(m_sort_sys[i1].Classifier_ID)).ToList();

                        m_req = m_req.OrderBy(x => x.AFO_AG_ID).ToList();

                        if (m_req.Count > 0)
                        {
                            int i2 = 0;
                            do
                            {
                                this.export_excel.Write_Cell_string(row, max_column_sys_aufbruch + 2, m_req[i2].AFO_AG_ID, oWS);
                                this.export_excel.Write_Cell_string(row, max_column_sys_aufbruch + 3, m_req[i2].AFO_TITEL, oWS);
                                this.export_excel.Write_Cell_string(row, max_column_sys_aufbruch + 4, m_req[i2].AFO_TEXT, oWS);

                                row++;
                                i2++;
                            } while (i2 < m_req.Count);
                        }

                        row = Insert_SysAfo_Child(row, column_new, parent.m_Child[i1], oWS, m_save, database);

                       
                    }

                    i1++;
                } while (i1 < parent.m_Child.Count);
            }


            return (row);
        }
        #endregion
        #region Afo Sys Mapping
        private void Export_TFL(Repsoitory_Elements.Capability catalogue, Excel.Worksheet oWS)
        {
          

            //Beschreiben Excel Tabelle
            if (catalogue.m_Child.Count > 0)
            {
                int row = 2;
                int column = 2;
                int ebene = 1;

                max_column = 3;
                max_row = 1;

                List<Repsoitory_Elements.Capability> m_sort = catalogue.m_Child.OrderBy(x => x.SYS_AG_ID).ToList();

                #region Req Category, Anforderungen und TFL
                int i1 = 0;
                do
                {
                    //var test = oWS.Cells[1, 1].GetStyle();
                    //  test.ForegroundColor = Color.Gold;
                    // oWS.Cells[1, 1].Font.Color = Color.Black;
                    //oWS.Cells[1, 1].Interior.Color = Color.Blue;
                    this.export_excel.Write_Cell_string(row, column, m_sort[i1].SYS_AG_ID, oWS);
                    this.export_excel.Write_Cell_string(row, column + 1, m_sort[i1].Name, oWS);
                    this.export_excel.Write_Cell_integer(row, 1, ebene, oWS);

                    //Anforderung hinzufügen
                    if (m_sort[i1].m_Requirement.Count > 0)
                    {
                        int i2 = 0;
                        do
                        {
                            row++;
                            this.export_excel.Write_Cell_string(row, column + 1, m_sort[i1].m_Requirement[i2].AFO_AG_ID, oWS);
                            this.export_excel.Write_Cell_string(row, column + 2, m_sort[i1].m_Requirement[i2].AFO_TITEL, oWS);
                            this.export_excel.Write_Cell_string(row, column + 3, m_sort[i1].m_Requirement[i2].AFO_TEXT, oWS);
                            // this.export_excel.Write_Cell_string(row, column + 2, m_sort[i1].m_Requirement[i2].AFO_ABS_GEWICHT, oWS);
                            this.export_excel.Write_Cell_integer(row, 1, ebene + 1, oWS);

                            //Systeme Platzhalter hinzufügen
                            if (m_sort[i1].m_Requirement[i2].sysElements.Count > 0)
                            {
                                int i3 = 0;
                                do
                                {
                                    row++;

                                    i3++;
                                } while (i3 < m_sort[i1].m_Requirement[i2].sysElements.Count);
                            }

                            i2++;
                        } while (i2 < m_sort[i1].m_Requirement.Count);
                    }

                    List<int> m_help = this.Insert_rekrusiv(m_sort[i1], oWS, row, column + 1, ebene);

                    row = m_help[0];

                    if (m_help[0] > max_row)
                    {
                        max_row = m_help[0];
                    }

                    i1++;
                } while (i1 < m_sort.Count);

                #endregion

                #region ea_guid hinzufügen und einfärben
                row = 2;
                column = 2;
                int i4 = 0;

                max_row--;

               // max_column++;

                oWS.Range[oWS.Cells[1, 1], oWS.Cells[max_row, max_column + max_column_offset+1]].Interior.Color = m_color[0];
                oWS.Range[oWS.Cells[1, 1], oWS.Cells[max_row, max_column + max_column_offset+1]].Font.Color = m_color_font[0];

                bool flag_color = false;
                ColorConverter colorConverter = new ColorConverter();

                do
                {
                    flag_color = false;

                    //this.export_excel.Write_Cell_string(row, max_column + max_column_offset, m_sort[i4].Classifier_ID, oWS);



                    if (m_sort[i4].SYS_FARBCODE != "" && m_sort[i4].SYS_FARBCODE != null)
                    {
                        if (m_sort[i4].SYS_FARBCODE.Length == 7)
                        {
                            flag_color = true;


                            Color recent_color = (Color)colorConverter.ConvertFromString(m_sort[i4].SYS_FARBCODE);

                            oWS.Range[oWS.Cells[row, column], oWS.Cells[row, max_column + max_column_offset+1]].Interior.Color = recent_color;
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[max_row, column]].Interior.Color = recent_color;
                        }



                    }

                    if (flag_color == false)
                    {
                        if ((ebene) < m_color.Count)
                        {
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[row, max_column + max_column_offset+1]].Interior.Color = m_color[ebene];
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[row, max_column + max_column_offset+1]].Font.Color = m_color_font[ebene];
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[max_row, column ]].Interior.Color = m_color[ebene];
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[max_row, column ]].Font.Color = m_color_font[ebene];
                        }
                        else
                        {
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[row, max_column + max_column_offset + 1]].Interior.Color = m_color[m_color.Count - 1];
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[row, max_column + max_column_offset + 1]].Font.Color = m_color_font[m_color.Count - 1];
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[max_row, column]].Interior.Color = m_color[m_color.Count - 1];
                            oWS.Range[oWS.Cells[row, column], oWS.Cells[max_row, column]].Font.Color = m_color_font[m_color.Count - 1];
                        }
                    }

                    //Überschriften Werte
                    //this.export_excel.Write_Cell_string(1, max_column + max_column_offset - 1, "Absolutes Gewicht Forderung", oWS);
                    this.export_excel.Write_Cell_string(1, max_column + max_column_offset, "SYS_AG_ID", oWS);
                    this.export_excel.Write_Cell_string(1, max_column + max_column_offset+1, "SYS_Titel", oWS);
                    //this.export_excel.Write_Cell_string(1, max_column + max_column_offset+2, "SYS_AG_ID", oWS);
                    //this.export_excel.Write_Cell_string(1, max_column + max_column_offset - 2, "Operative Bewertung", oWS);
                    //this.export_excel.Write_Cell_string(1, max_column + max_column_offset - 3, "Relatives Gewicht TFL", oWS);


                    //Systeme hinzufügen
                    if (m_sort[i4].m_Requirement.Count > 0)
                    {
                        Ennumerationen.AFO_ENUM aFO_ENUM = new Ennumerationen.AFO_ENUM();

                        int row_save = row;

                        int i5 = 0;
                        do
                        {
                            row++;
                            // this.export_excel.Write_Cell_string(row, max_column + max_column_offset - 2, "100%", oWS);
                            // this.export_excel.Write_Cell_string(row, max_column + max_column_offset - 2, aFO_ENUM.AFO_OPERATIVEBEWERTUNG[(int)m_sort[i4].m_Requirement[i5].AFO_OPERATIVEBEWERTUNG], oWS);
                            // this.export_excel.Write_Cell_string(row, max_column + max_column_offset - 1, m_sort[i4].m_Requirement[i5].AFO_ABS_GEWICHT, oWS);
                            // this.export_excel.Write_Cell_string(row, max_column + max_column_offset, m_sort[i4].m_Requirement[i5].Classifier_ID, oWS);

                            if (m_sort[i4].m_Requirement[i5].sysElements.Count > 0)
                            {
                                int i6 = 0;
                                do
                                {
                                    this.export_excel.Write_Cell_string(row+1, max_column + max_column_offset, m_sort[i4].m_Requirement[i5].sysElements[i6].SYS_AG_ID, oWS);
                                    this.export_excel.Write_Cell_string(row+1, max_column + max_column_offset+1, m_sort[i4].m_Requirement[i5].sysElements[i6].Name, oWS);
                                    row++;
                                    i6++;
                                } while (i6 < m_sort[i4].m_Requirement[i5].sysElements.Count);

                             /*   oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[row, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Color = Color.Black;
                                oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[row, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                                oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[row, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Weight = XlBorderWeight.xlMedium;*/
                            }

                           

                            i5++;
                        } while (i5 < m_sort[i4].m_Requirement.Count);

                        /*
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row, max_column + max_column_offset]].Interior.Color = m_color_req[0];
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row, max_column + max_column_offset]].Font.Color = m_color_font_req[0];
                        */

                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Interior.Color = m_color_req[0];
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Font.Color = m_color_font_req[0];

                      /*  oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Color = Color.Black;
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Weight = XlBorderWeight.xlMedium;*/

                        oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[row, max_column + max_column_offset + 1]].Interior.Color = m_color_req[1];
                        oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[row, max_column + max_column_offset + 1]].Font.Color = m_color_font_req[1];

                        oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[row, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Color = Color.Black;
                        oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[row, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                        oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[row, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Weight = XlBorderWeight.xlMedium;
                        /*if ((ebene+1) < m_color.Count)
                        {
                            oWS.Range[oWS.Cells[row_save+1, column + 1], oWS.Cells[row, max_column + max_column_offset]].Interior.Color = m_color[ebene + 1];
                            oWS.Range[oWS.Cells[row_save+1, column + 1], oWS.Cells[row, max_column + max_column_offset]].Font.Color = m_color_font[ebene + 1];
                        }
                        else
                        {
                            oWS.Range[oWS.Cells[row_save+1, column + 1], oWS.Cells[row, max_column + max_column_offset]].Interior.Color = m_color[m_color.Count - 1];
                            oWS.Range[oWS.Cells[row_save+1, column + 1], oWS.Cells[row, max_column + max_column_offset]].Font.Color = m_color_font[m_color.Count - 1];
                        }*/

                    }


                    List<int> m_help = this.Insert_guid_rekrusiv(m_sort[i4], oWS, row, column + 1, ebene);

                    row = m_help[0];

                    i4++;
                } while (i4 < m_sort.Count);

                #endregion
            }


            oWS.UsedRange.Columns.AutoFit();
            oWS.UsedRange.Rows.AutoFit();

        }

        private List<int> Insert_rekrusiv(Repsoitory_Elements.Capability recent, Excel.Worksheet oWS, int row, int column, int ebene)
        {
            List<int> m_ret = new List<int>();
            int recent_row = row;
            int recent_column = column;
            int ebene_neu = ebene;
            ebene_neu++;

            if (recent.m_Child.Count > 0)
            {
                if (this.max_column < recent_column + 2)
                {
                    this.max_column = recent_column + 2;
                }

                List<Repsoitory_Elements.Capability> m_sort = recent.m_Child.OrderBy(x => x.SYS_AG_ID).ToList();

                recent_row++;
                //recent_column++;
                int i1 = 0;
                do
                {


                    this.export_excel.Write_Cell_string(recent_row, recent_column, m_sort[i1].SYS_AG_ID, oWS);
                    this.export_excel.Write_Cell_string(recent_row, recent_column + 1, m_sort[i1].Name, oWS);
                    this.export_excel.Write_Cell_integer(recent_row, 1, ebene_neu, oWS);


                    //Anforderung hinzufügen
                    if (m_sort[i1].m_Requirement.Count > 0)
                    {
                        int row_save = recent_row;

                        int i2 = 0;
                        do
                        {
                            recent_row++;
                            this.export_excel.Write_Cell_string(recent_row, recent_column + 1, m_sort[i1].m_Requirement[i2].AFO_AG_ID, oWS);
                            this.export_excel.Write_Cell_string(recent_row, recent_column + 2, m_sort[i1].m_Requirement[i2].AFO_TITEL, oWS);
                            this.export_excel.Write_Cell_string(recent_row, recent_column + 3, m_sort[i1].m_Requirement[i2].AFO_TEXT, oWS);
                            // this.export_excel.Write_Cell_string(recent_row, recent_column + 2, m_sort[i1].m_Requirement[i2].AFO_ABS_GEWICHT, oWS);
                            this.export_excel.Write_Cell_integer(recent_row, 1, ebene_neu + 1, oWS);

                            //Systeme Platzhalter hinzufügen
                            if (m_sort[i1].m_Requirement[i2].sysElements.Count > 0)
                            {
                                int i3 = 0;
                                do
                                {
                                    recent_row++;

                                    i3++;
                                } while (i3 < m_sort[i1].m_Requirement[i2].sysElements.Count);
                            }

                            i2++;
                        } while (i2 < m_sort[i1].m_Requirement.Count);


                    }

                    List<int> m_help = this.Insert_rekrusiv(m_sort[i1], oWS, recent_row, recent_column + 1, ebene_neu);

                    recent_row = m_help[0];

                    i1++;
                } while (i1 < m_sort.Count);
            }
            else
            {
                recent_row++;
            }
            m_ret.Add(recent_row);
            m_ret.Add(recent_column);

            return (m_ret);

        }


        private List<int> Insert_guid_rekrusiv(Repsoitory_Elements.Capability recent, Excel.Worksheet oWS, int row, int column, int ebene)
        {
            List<int> m_ret = new List<int>();
            int recent_row = row;
            int recent_column = column;
            int ebene_neu = ebene;
            ebene_neu++;

            if (recent.m_Child.Count > 0)
            {
                Ennumerationen.AFO_ENUM aFO_ENUM = new Ennumerationen.AFO_ENUM();

                List<Repsoitory_Elements.Capability> m_sort = recent.m_Child.OrderBy(x => x.SYS_AG_ID).ToList();
                bool flag_color = false;
                ColorConverter colorConverter = new ColorConverter();
                recent_row++;
                //recent_column++;
                int i1 = 0;
                do
                {
                    flag_color = false;

                    this.export_excel.Write_Cell_string(recent_row, max_column + max_column_offset, m_sort[i1].Classifier_ID, oWS);

                    if (m_sort[i1].SYS_FARBCODE != "" && m_sort[i1].SYS_FARBCODE != null)
                    {
                        if (m_sort[i1].SYS_FARBCODE.Length == 7)
                        {
                            flag_color = true;


                            Color recent_color = (Color)colorConverter.ConvertFromString(m_sort[i1].SYS_FARBCODE);

                            oWS.Range[oWS.Cells[recent_row, column-1], oWS.Cells[recent_row, max_column + max_column_offset+1]].Interior.Color = recent_color;
                            oWS.Range[oWS.Cells[recent_row, column - 1], oWS.Cells[max_row, column]].Interior.Color = recent_color;
                        }



                    }

                    if (flag_color == false)
                    {
                        if ((ebene_neu) < m_color.Count)
                        {
                            oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[max_row, column]].Interior.Color = m_color[ebene];
                            oWS.Range[oWS.Cells[max_row, column], oWS.Cells[max_row, column]].Font.Color = m_color_font[ebene];
                            oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[recent_row, max_column + max_column_offset+1]].Interior.Color = m_color[ebene_neu];
                            oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[recent_row, max_column + max_column_offset+1]].Font.Color = m_color_font[ebene_neu];
                            
                        }
                        else
                        {
                            oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[max_row, column]].Interior.Color = m_color[m_color.Count - 1];
                            oWS.Range[oWS.Cells[max_row, column], oWS.Cells[max_row, column]].Font.Color = m_color_font[m_color.Count - 1];
                            oWS.Range[oWS.Cells[recent_row, column-1], oWS.Cells[recent_row, max_column + max_column_offset + 1]].Interior.Color = m_color[m_color.Count - 1];
                            oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[recent_row, max_column + max_column_offset + 1]].Font.Color = m_color_font[m_color.Count - 1];
                           
                        }

                    }
                    /*   if ((ebene_neu) < m_color.Count)
                       {
                           oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[recent_row, max_column + max_column_offset]].Interior.Color = m_color[ebene_neu];
                           oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[recent_row, max_column + max_column_offset]].Font.Color = m_color_font[ebene_neu];
                           oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[max_row, column]].Interior.Color = m_color[ebene_neu];
                           oWS.Range[oWS.Cells[max_row, column], oWS.Cells[max_row, column]].Font.Color = m_color_font[ebene_neu];
                       }
                       else
                       {
                           oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[recent_row, max_column + max_column_offset]].Interior.Color = m_color[m_color.Count - 1];
                           oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[recent_row, max_column + max_column_offset]].Font.Color = m_color_font[m_color.Count - 1];
                           oWS.Range[oWS.Cells[recent_row, column], oWS.Cells[recent_row, column]].Interior.Color = m_color[m_color.Count - 1];
                           oWS.Range[oWS.Cells[max_row, column], oWS.Cells[max_row, column]].Font.Color = m_color_font[m_color.Count - 1];
                       }*/
                    //  }


                    //Anforderung hinzufügen
                    if (m_sort[i1].m_Requirement.Count > 0)
                    {
                        int row_save = recent_row;

                        int i2 = 0;
                        do
                        {
                            recent_row++;
                            //  this.export_excel.Write_Cell_string(recent_row, max_column + max_column_offset - 2, aFO_ENUM.AFO_OPERATIVEBEWERTUNG[(int)m_sort[i1].m_Requirement[i2].AFO_OPERATIVEBEWERTUNG], oWS);
                            //  this.export_excel.Write_Cell_string(recent_row, max_column + max_column_offset - 1, m_sort[i1].m_Requirement[i2].AFO_ABS_GEWICHT, oWS);
                            //  this.export_excel.Write_Cell_string(recent_row, max_column + max_column_offset, m_sort[i1].m_Requirement[i2].Classifier_ID, oWS);
                            if (m_sort[i1].m_Requirement[i2].sysElements.Count > 0)
                            {
                                int i6 = 0;
                                do
                                {
                                    this.export_excel.Write_Cell_string(recent_row + 1 , max_column + max_column_offset, m_sort[i1].m_Requirement[i2].sysElements[i6].SYS_AG_ID, oWS);
                                    this.export_excel.Write_Cell_string(recent_row + 1 , max_column + max_column_offset + 1, m_sort[i1].m_Requirement[i2].sysElements[i6].Name, oWS);
                                    recent_row++;
                                    i6++;
                                } while (i6 < m_sort[i1].m_Requirement[i2].sysElements.Count);
                            }
                            //TFL hinzufügen
                            /*   if (m_sort[i1].m_Requirement[i2].m_Requirement_TFL.Count > 0)
                               {
                                   int i3 = 0;
                                   do
                                   {
                                       recent_row++;
                                       // this.export_excel.Write_Cell_string(recent_row, max_column + max_column_offset, m_sort[i1].m_Requirement[i2].m_Requirement_TFL[i3].Classifier_ID, oWS);


                                       i3++;
                                   } while (i3 < m_sort[i1].m_Requirement[i2].m_Requirement_TFL.Count);
                               }*/

                           /* oWS.Range[recent_row, max_column + max_column_offset + 1].Borders.Item(XlBordersIndex.xlEdgeTop).Color = Color.Black;
                            oWS.Range[oWS.Cells[recent_row, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                            oWS.Range[oWS.Cells[recent_row, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Weight = XlBorderWeight.xlMedium;*/


                            i2++;
                        } while (i2 < m_sort[i1].m_Requirement.Count);

                        /*
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[recent_row, max_column + max_column_offset]].Interior.Color = m_color_req[0];
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[recent_row, max_column + max_column_offset]].Font.Color = m_color_font_req[0];
                        */
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Interior.Color = m_color_req[0];
                        oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Font.Color = m_color_font_req[0];
                        oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[recent_row, max_column + max_column_offset + 1]].Interior.Color = m_color_req[1];
                        oWS.Range[oWS.Cells[row_save + 2, column + 1], oWS.Cells[recent_row, max_column + max_column_offset + 1]].Font.Color = m_color_font_req[1];

                        /*  oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Color = Color.Black;
                          oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                          oWS.Range[oWS.Cells[row_save + 1, column + 1], oWS.Cells[row_save + 1, max_column + max_column_offset + 1]].Borders.Item(XlBordersIndex.xlEdgeTop).Weight = XlBorderWeight.xlMedium;*/


                        /*if ((ebene_neu + 1) < m_color.Count)
                        {
                            oWS.Range[oWS.Cells[row_save+1, column + 1], oWS.Cells[recent_row, max_column + max_column_offset]].Interior.Color = m_color[ebene_neu + 1];
                            oWS.Range[oWS.Cells[row_save+1, column + 1], oWS.Cells[recent_row, max_column + max_column_offset]].Font.Color = m_color_font[ebene_neu + 1];
                        }
                        else
                        {
                            oWS.Range[oWS.Cells[row_save+1, column + 1], oWS.Cells[recent_row, max_column + max_column_offset]].Interior.Color = m_color[m_color.Count - 1];
                            oWS.Range[oWS.Cells[row_save+1, column + 1], oWS.Cells[recent_row, max_column + max_column_offset]].Font.Color = m_color_font[m_color.Count - 1];
                        }*/
                    }

                    List<int> m_help = this.Insert_guid_rekrusiv(m_sort[i1], oWS, recent_row, recent_column + 1, ebene_neu);

                    recent_row = m_help[0];

                    i1++;
                } while (i1 < m_sort.Count);
            }
            else
            {
                recent_row++;
            }
            m_ret.Add(recent_row);
            m_ret.Add(recent_column);

            return (m_ret);

        }
        #endregion
    }


}

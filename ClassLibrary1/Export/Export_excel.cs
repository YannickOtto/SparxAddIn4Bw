using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;

namespace Requirement_Plugin.Export
{
    public class Export_excel
    {
        public Excel.Application oXL;
        public Excel.Workbook oWB;

        public string _filename = "";

        #region Konstruktor und Destruktor
        public Export_excel()
        {

        }

        ~Export_excel()
        {

        }
        #endregion

        #region Create_Excel
        public void Create_File()
        {
            string filename = Choose_Savespace();
            _filename = filename;

            oXL = new Excel.Application();
            oXL.Visible = false;

            //oWB = new Excel.Workbook();
            oWB = oXL.Workbooks.Add(Type.Missing);
            oWB = oXL.ActiveWorkbook;


            
        }

        public void Save_File()
        {
            this.oWB.SaveAs(this._filename, 52);

            this.oWB.Close();

        }
        #endregion

        #region Write
        public void Write_Cell_string(int row, int column, string _str, Excel.Worksheet oWS)
        {

             oWS.Cells[row, column].Value = _str;

        }

        public void Write_Cell_integer(int row, int column, int _int, Excel.Worksheet oWS)
        {

                oWS.Cells[row, column].Value = _int;
            
        }

        #endregion
        #region Savespace
        /// <summary>
        /// Abfrage des Speicherortes der xac Datei mit Hilfe eines Savefildialoges
        /// </summary>
        /// <returns></returns>
        private string Choose_Savespace()
        {
            string filename = "export";
            bool save = false;

            do
            {
                SaveFileDialog saveFileDialog_Save = new SaveFileDialog();
                saveFileDialog_Save.Filter = "Excel|*.xlsm";
                saveFileDialog_Save.Title = "Save an xlsm File";
                saveFileDialog_Save.ShowDialog();

                if (saveFileDialog_Save.FileName != "")
                {
                    filename = saveFileDialog_Save.FileName;
                    save = true;
                }

            } while (save == false);

            return (filename);
        }
        #endregion
    }


}

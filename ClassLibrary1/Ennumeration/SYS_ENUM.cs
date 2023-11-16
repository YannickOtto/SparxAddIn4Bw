using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ennumerationen
{
    public class SYS_ENUM
    {

        public string[] SYS_ARTIKEL;
        public string[] SYS_DETAILSTUFE;
        public string[] SYS_ERFUELLT_AFO;
        public string[] SYS_KOMPONENTENTYP;
        public string[] SYS_PRODUKT_STATUS;
        public string[] SYS_STATUS;
        public string[] SYS_SUBORDINATES_AFO;
        public string[] SYS_TYP;

        public SYS_ENUM()
        {
            this.SYS_ARTIKEL = new string[] { "der", "die", "das", "kein" };

            this.SYS_DETAILSTUFE = new string[] { "unbestimmt", "Uebergreifend", "0", "1", "2", "3", "4", "5", "kein" };

            this.SYS_ERFUELLT_AFO = new string[] { "true", "false", "" };

            this.SYS_KOMPONENTENTYP = new string[]  {"unbestimmt","System","Unterstützungssystem","Segment","HW-Einheit","SW-Einheit",
            "Externe Einheit","HW-Komponente","SW-Komponente","HW-Modul","Externes HW-Modul","SW-Modul","Externes SW-Modul","Datenspeicher",
            "Verteilerelement","Service","Service Level","Physical Architecture","Entwurfsentscheidung","Schnittstelle", "kein"};

            this.SYS_PRODUKT_STATUS = new string[] { "vom AG gesetzt", "unbestimmt", "kein" };
            
            this.SYS_STATUS = new string[] { "geplant", "realisiert", "obsolet", "kein" };

            this.SYS_SUBORDINATES_AFO = new string[] { "Sys_subordinates_Afo_true", "Sys_subordinates_Afo_false", "kein" };

            this.SYS_TYP = new string[] { "Funktionsbaum", "Szenarbaum", "Technisches System", "C3 Taxonomie", "Qualitaetsmerkmal",
            "Loesungen", "Ausschreibungskriterium", "Angebot", "kein" };

        }

        public int Get_Index(string[] Array, string Name)
        {
            int Index = 0;

            if (Array.Length > 0)
            {
                int i1 = 0;
                do
                {
                    if (Array[i1] == Name)
                    {
                        return (i1);
                    }

                    i1++;
                } while (i1 < Array.Length);
            }


            return (Index);
        }

    }
}

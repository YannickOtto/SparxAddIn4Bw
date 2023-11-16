using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ennumerationen
{
    public class ST_ENUM
    {
        public string[] ST_AKTEUR_TYP;
        public string[] ST_EINFLUSS;
        public string[] ST_GRUPPE;
        public string[] ST_INTERESSE;
        public string[] ST_WV_PHASE;

        public ST_ENUM()
        {
            this.ST_AKTEUR_TYP = new string[] { "Human", "Bewerter", "Stakeholder", "kein" };
            this.ST_EINFLUSS = new string[] { "0", "1", "2", "3", "kein" };
            this.ST_GRUPPE = new string[] { "Kernteam", "Operationell Beteiligte", "Geschäftsfeld", "Erweitertes Umfeld", "Sonstige", "kein" };
            this.ST_INTERESSE = new string[] { "-3", "-2", "-1", "0", "1", "2", "3", "kein" };
            this.ST_WV_PHASE = new string[] { "Analysephase", "Realisierungsphase", "Nutzungsphase", "phasenübergreifend", "kein" };

        }

    }
}

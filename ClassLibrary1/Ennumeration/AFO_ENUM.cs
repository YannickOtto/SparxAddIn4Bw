using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ennumerationen
{
    public class AFO_ENUM
    {
        public string[] AFO_BEZUG;
        public string[] AFO_CPM_PHASE;
        public string[] AFO_DETAILSTUFE;
        public string[] AFO_FUNKTIONAL;
        public string[] AFO_KRITIKALITAET;
        public string[] AFO_OPERATIVEBEWERTUNG;
        public string[] AFO_PRIORITAET_VERGABE;
        public string[] AFO_PROJEKTROLLE;
        public string[] AFO_QS_STATUS;
        public string[] AFO_STATUS;
        public string[] AFO_VERERBUNG;
        public string[] AFO_WV_ART;
        public string[] AFO_WV_NACHWEISART;
        public string[] AFO_WV_PHASE;
        public string[] IN_CATEGORY;
        public string[] W_AKTIVITAET;
        public string[] AFO_Stereotype;

        public AFO_ENUM()
        {
            this.AFO_BEZUG = new string[] { "System", "Unterstuetzungssystem", "Angebot", "Konzept", "Lieferumfang",
                "Projektdurchfuehrung", "Vertrag", "Organisation", "Beistellung", "kein"};

            this.AFO_CPM_PHASE = new string[] { "0", "1", "2", "3", "4", "99", "kein" };

           // this.AFO_CPM_PHASE = new string[] { "phasenübergreifend", "phasenübergreifend", "phasenübergreifend", "phasenübergreifend", "phasenübergreifend", "phasenübergreifend", "kein" };

            this.AFO_DETAILSTUFE = new string[] { "0", "1", "2", "3", "4", "kein" };

            this.AFO_FUNKTIONAL = new string[] { "funktional", "nicht_funktional", "kein" };

            this.AFO_KRITIKALITAET = new string[] { "A1", "A2", "A3", "B1", "B2", "B3", "C1", "C2", "C3", "unbestimmt", "kein" };

            this.AFO_OPERATIVEBEWERTUNG = new string[] { "kritisch", "essentiell", "notwendig", "unerheblich", "kein" };

            this.AFO_PRIORITAET_VERGABE = new string[] { "kritisch", "essentiell", "notwendig", "unerheblich", "kein" };

            this.AFO_PROJEKTROLLE = new string[] { "AG", "AN", "kein" };

            this.AFO_QS_STATUS = new string[] { "ungeprueft", "akzeptiert", "bemaengelt", "kein" };

            this.AFO_STATUS = new string[] { "neu", "in Abstimmung", "befuerwortet", "zurueckgestellt", "festgeschrieben", "realisiert", "obsolet", "kein" };

            this.AFO_VERERBUNG = new string[] { "Ja", "Nein", "kein" };

            this.AFO_WV_ART = new string[] { "Anforderung", "Nutzerforderung", "Systemanforderung", "Prozessanforderung",
                "Anwendungsfall", "Randbedingung", "Annahme", "Regel", "Fähigkeit", "Geschäftsprozess", "Feature", "Parameter",
                "Ziel", "Bedrohung", "Überschrift/Einleitung", "kein" };

            this.AFO_WV_NACHWEISART = new string[] { "unbestimmt","Prüffall","Review","Test Release Unit", "Test Release Package","Systemnachweis",
               "Konformitätsnachweis","Übergabenachweis", "Selbsterklärung","Annahmeprüfung", "Sichtprüfung",
                 "Installationstest","Service-Konformitätsprüfung","Test Funktionaler Anforderungen","Test nicht-funktionaler Qualitätskriterien (SLA Teil1)",
                 "Kompatibilitätstest","Koexistenztest","Interoperabilitätstest (Verifikation)", "Test nicht-funktionaler Qualitätskriterien (SLA Teil2)",
                  "Zulassung IT-Sicherheit","Test IT-Sicherheitsanforderungen","Prüfung Migrationsverfahren",
                "Test SW-Funktionalität der Migration", "Test Datenmigration","Test Rollen- und Rechtemigration",
                "Test Datenqualität Teil 1", "Test Datenqualität Teil 2", "Test Nutzeranforderungen (User Functionality)",
                "Interoperabilitätstest (Validierung)","Szenarbasierter Test","Einsatzprüfung","Test Quality in Use",
                "Betreibbarkeitsprüfung (nicht-technisch)","Deployment Quality Test","Betreibbarkeitstest",
                "Quality in Operations Monitoring / Dauertest","Sonstiges","kein Nachweis", "kein"};

            this.AFO_WV_PHASE = new string[] {"Konzept", "Prototyp", "Einsatzreife", "Roll-Out", "phasenübergreifend", "kein" };

            this.IN_CATEGORY = new string[] { "0", "43", "kein" };

            this.W_AKTIVITAET = new string[] { "-", "die Möglichkeit bieten", "fähig sein", "kein" };

            this.AFO_Stereotype = new string[] { };
        }
        /// <summary>
        /// Index aus einer Ennumeration
        /// </summary>
        /// <param name="Array"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Get_Index(string[] Array, string Name)
        {
            int Index = -1;

            if(Array.Length > 0)
            {
                int i1 = 0;
                do
                {
                    if(Array[i1] == Name)
                    {
                        return (i1);
                    }

                    i1++;
                } while (i1 < Array.Length);
            }

            if(Index == -1) //Es wird ein Standardwert gesetzt bzw angenommen
            {
                Index = 0;
            }

            return (Index);
        }

    }
}

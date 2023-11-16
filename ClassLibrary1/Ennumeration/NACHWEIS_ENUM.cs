using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ennumeration
{
	public class NACHWEIS_ENUM
    {
        public string[] Nachweisart;

        public  NACHWEIS_ENUM()
        {
            this.Nachweisart  = new string[]
			{
				"unbestimmt",
				"Prüffall" ,
				"Review",
				"Test Release Unit",
				"Test Release Package",
				"Systemnachweis",
				"Konformitätsnachweis",
				"Übergabenachweis",
				"Selbsterklärung",
				"Annahmeprüfung",
				"Sichtprüfung",
				"Installationstest",
				"Service-Konformitätsprüfung (nicht-technisch)",
				"Test Funktionaler Anforderungen",
				"Test nicht-funktionaler Anforderungen",
				"Test nicht-funktionaler Qualitätskriterien (SLA Teil1)",
				"Kompatibilitätstest",
				"Koexistenztest",
				"Interoperabilitätstest (Verifikation)",
				"Test nicht-funktionaler Qualitätskriterien (SLA Teil2)",
				"Zulassung ITS-icherheit",
				"Test IT-Sicherheitsanforderungen",
				"Prüfung Migrationsverfahren",
				"Test SW-Funktinalität der Migration",
				"Test Datenmigration",
				"Test Rollen- und Rechtemigration",
				"Test Datenqualität Teil 1",
				"Test Datenqualität Teil 2",
				"Test Nutzeranforderungen (UserFunctionality)",
				"Interoperabilitätstest (Validierung)",
				"Szenarbasierter Test",
				"Einsatzprüfung",
				"Test Quality in Use",
				"Betreibbarkeitsprüfung (nicht-technisch)",
				"Deployment Qualitiy Test",
				"Betreibbarkeitstest",
				"Quality in Operations Monitoring / Dauertest",
				"Sonstiges",
				"kein Nachweis"
			};
        }
		public int Get_Index(string[] Array, string Name)
		{
			int Index = -1;

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

			if (Index == -1) //Es wird ein Standardwert gesetzt bzw angenommen
			{
				Index = 0;
			}

			return (Index);
		}

	}
}

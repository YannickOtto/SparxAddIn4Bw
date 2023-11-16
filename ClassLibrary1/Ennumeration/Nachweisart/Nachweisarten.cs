using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace Ennumerationen
{
	public enum Nachweisarten : int
	{

		unbestimmt,
		Prüffall,
		Review,
		TestReleaseUnit,
		TestReleasePackage,
		Systemnachweis,
		Konformitätsnachweis,
		Übergabenachweis,
		Selbsterklärung,
		Annahmeprüfung,
		Sichtprüfung,
		Installationstest,
		ServiceKonformitätsprüfung,
		TestFunktionalerAnforderungen,
		TestNichtFunktionalerAnforderungen,
		TestNichtFunktionalerQualitätskriterienTeil1,
		Kompatibilitätstest,
		Koexistenztest,
		InteroperabilitätstestVerifikation,
		TestNichtFunktionalerQualitätskriterienTeil2,
		ZulassungITSicherheit,
		TestITSicherheitsanforderungen,
		PrüfungMigrationsverfahren,
		TestSWFunktinalitätderMigration,
		TestDatenmigration,
		TestRollenundRechtemigration,
		TestDatenqualität1,
		TestDatenqualität2,
		TestNutzerforderungenUserFunctionality,
		InteroperabilitätstestValidierung,
		SzenarbasierterTest,
		Einsatzprüfung,
		TestQualityinUse,
		Betreibbarkeitsprüfungnichttechnisch,
		DeploymentQualitiyTest,
		Betreibbarkeitstest,
		QualityinOperationsMonitoringDauertest,
		Sonstiges,
		keinNachweis

		//{"Funktionsbaum", "Szenarbaum", "Technisches System", "C3 Taxonomie", "Qualitaetsmerkmal", "Loesungen", "Ausschreibungskriterium", "Angebot"};
	}//end SYS_TYP

}//end namespace Systemelement
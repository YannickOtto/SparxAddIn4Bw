using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metamodels
{
    public class Issues
    {

        public string[] m_Issue_Name = new string[30];
        public string[] m_Issue_Note = new string[30];

        public Issues()
        {

            /////////////////////////////////////////////////////////////////////////////
            ///Issue
            this.m_Issue_Name[0] = "Requirement: Fehlendes System Schnittstellenanforderung";
            this.m_Issue_Note[0] = "Es fehlt der eindeutige Sender bzw. Empfänger der Schnittstellen-Anforderung.";
            this.m_Issue_Name[1] = "Requirement: Fehlendes InformationItem";
            this.m_Issue_Note[1] = "Es fehlt die Verknüpfung von mind. einem InformationItem zu der Schnittstellen-Anforderung.";
            this.m_Issue_Name[2] = "Requirement: Fehlende Fähigkeit";
            this.m_Issue_Note[2] = "Es fehlt die Verknüpfung von einer Fähigkeit zu der Anforderung.";
            this.m_Issue_Name[3] = "Requirement: Zu viele Fähigkeiten";
            this.m_Issue_Note[3] = "Es wurden zu viele Fähigkeiten der Schnittstellen-Anforderung zugeordnet (max 1).";
            this.m_Issue_Name[4] = "Requirement: Keine Abhängigkeit Schnittstellen-Anforderungen";
            this.m_Issue_Note[4] = "Es wurden keine Abhängigkeit zu einer anderen Schnittstellen-Anforderung zugeordnet.";
            this.m_Issue_Name[5] = "Requirement: Fehlendes System Anforderung";
            this.m_Issue_Note[5] = "Es fehlt die Zuordnung zu dem System der Anforderung.";
            this.m_Issue_Name[6] = "Requirement: Fehlende Aktivität Anforderung";
            this.m_Issue_Note[6] = "Es fehlt die Zuordnung zu der Aktivität der Anforderung.";
            this.m_Issue_Name[7] = "Requirement: Fehlender Zuweisung Stakeholders";
            this.m_Issue_Note[7] = "Es fehlt die Zuordnung von mind. einem Stakeholder zu der Anforderung.";

            this.m_Issue_Name[12] = "Requirement: Fehlende Zuweisung DesignConstraint";
            this.m_Issue_Note[12] = "Es fehlt die Zuordnung von mind. einem DesignConstraint zu der Anforderung. Dieses Issue kann auch durch eine Zuordnung des DesignConstraint zu falschen Modellelementen getriggert werden.";

            this.m_Issue_Name[13] = "DesignConstraint: Fehlerhafte Zuweisung DesignConstraint";
            this.m_Issue_Note[13] = "Das DesignConstraint wurde einem Modellelement ungleich vom Type eines Element_Definition oder eines Element_Usage zugewiesen..";

            this.m_Issue_Name[14] = "Requirement: Fehlende Zuweisung ProcessConstraint";
            this.m_Issue_Note[14] = "Es fehlt die Zuordnung von mind. einem ProcessConstraint zu der Anforderung. Dieses Issue kann auch durch eine Zuordnung des ProcessConstraint zu falschen Modellelementen getriggert werden.";

            this.m_Issue_Name[15] = "UmweltConstraint: Fehlerhafte Zuweisung UmweltConstraint";
            this.m_Issue_Note[15] = "Das UmweltConstraint wurde einem Modellelement ungleich vom Type eines Element_Definition oder eines Element_Usage zugewiesen.";

            this.m_Issue_Name[16] = "Requirement: Fehlende Zuweisung Typevertreter";
            this.m_Issue_Note[16] = "Es fehlt die Zuordnung von mind. einem Typevertreter zu der Anforderung. Dieses Issue kann auch durch eine Zuordnung des Typevertreter zu falschen Modellelementen getriggert werden.";

            this.m_Issue_Name[17] = "Requirement_Typevertreter: Keine Unterscheidung System und zugehöriger Typevertreter";
            this.m_Issue_Note[17] = "Die Stereotypen der zugeordneten Systeme und der Typvertreter unterscheiden sich nicht.";

            this.m_Issue_Name[18] = "Requirement_Typevertreter: Fehlerhafter Zuordnung Typevertreter";
            this.m_Issue_Note[18] = "Ein identifizierter Typevertreter ist kein Client einer Relation_Typevertreter Beziehung. Dies kann durch identische Stereotypen von System und Typevertreter entstehen.";



            this.m_Issue_Name[24] = "Requirement_Loop: Bildung einer Schleife der Anforderung ";
            this.m_Issue_Note[24] = "Die verknüpften Anforderungen bilden eine Schleife. Für den ordnungsgemäßen Gebrauch muss diese Schleife aufgelöst werden.";

            this.m_Issue_Name[25] = "Requirement_Multiple_Refines: Mehrere ausgehende Verfeinerungen der Anforderung ";
            this.m_Issue_Note[25] = "Die verknüpften Anforderungen besitzt mehrere, ausgehende Verfeinerungen zu anderen Anforderungen. Für einen übersichtlichen xac Export empfiehlt es sich dies aufzulösen.";

            this.m_Issue_Name[26] = "Requirement_Duplicate: Anforderungen haben eine ungelöste Dopplungsbeziehung ";
            this.m_Issue_Note[26] = "Die Dopplung zwischen den Anforderungen wurde noch nicht gelöst.";

            this.m_Issue_Name[27] = "Requirement_Duplicate: gelöste Dopplungsbeziehung aber trotzdem Export ";
            this.m_Issue_Note[27] = "Die Dopplung zwischen den Anforderungen wurde gelöst, jedoch werden die Anforderungen dennoch exportiert.";

            ///////////////////////////////////////////////////////////////////////////////
            //Import
            this.m_Issue_Name[8] = "Import: Nicht importierte Requirement";
            this.m_Issue_Note[8] = "Die mit diesem Issue verknüpften Requirement wurden nicht importiert. Dies betrifft neue Anforderungen bzw. im Modell vorhandene Anforderungen, welche für den Export vorgesehen sind.";
            this.m_Issue_Name[9] = "Import: Update Requirement";
            this.m_Issue_Note[9] = "Die mit diesem Issue verknüpften Requirement wurden geupdatet.";
            this.m_Issue_Name[10] = "Import: Neu importierte Requirement";
            this.m_Issue_Note[10] = "Die mit diesem Issue verknüpften Requirement wurden neu importiert.";
            this.m_Issue_Name[11] = "Import: Kein Update Requirement";
            this.m_Issue_Note[11] = "Die mit diesem Issue verknüpften Requirement wurden nicht geupdatet.";

            this.m_Issue_Note[19] = "Die mit diesem Issue verknüpften Elemente, besitzen einen Konnektor, welcher ncht importiert wurde.";

            this.m_Issue_Name[20] = "Import: Nicht importierte Systemelemente";
            this.m_Issue_Note[20] = "Die mit diesem Issue verknüpften Systemelemente wurden nicht importiert. Dies betrifft neue Systemelemente bzw. im Modell vorhandene Systemelemente, welche für den Export vorgesehen sind.";

            this.m_Issue_Name[21] = "Import: Update Systemelemente";
            this.m_Issue_Note[21] = "Die mit diesem Issue verknüpften Systemelemente wurden geupdatet.";

            this.m_Issue_Name[22] = "Import: Neu importierte Systemelemente";
            this.m_Issue_Note[22] = "Die mit diesem Issue verknüpften Systemelemente wurden neu importiert.";

            this.m_Issue_Name[23] = "Import: Kein Update Systemelemente";
            this.m_Issue_Note[23] = "Die mit diesem Issue verknüpften Systemelemente wurden nicht geupdatet.";

            this.m_Issue_Name[28] = "Import: guid bereits im Modell vorhanden";
            this.m_Issue_Note[28] = "Es wurde für das importierte Element eine neue Guid erstellt. Für zukünftige, reibungslose Importe muss dieses Issue gelöst werden.";


        }


     
    }
}

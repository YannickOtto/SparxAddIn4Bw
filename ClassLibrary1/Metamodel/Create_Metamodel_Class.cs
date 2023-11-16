using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Ennumerationen;
using Forms;

namespace Metamodels
{
    public class Create_Metamodel_Class
    {
        public List<string> m_Type_Konnektoren = new List<string>();
        public List<string> m_Stereoype_Konnektoren = new List<string>();
        public List<string> m_Type_Elemente = new List<string>();
        public List<string> m_Type_Requirement = new List<string>();
        public List<string> m_Type_InformationItem = new List<string>();
        public List<string> m_Stereotype_Elemente = new List<string>();
        public List<string> m_Toolbox = new List<string>();
        public SYS_ENUM sys_Enum;
        public AFO_ENUM afo_Enum;
        public ST_ENUM sT_ENUM;
        public List<string> combo_xac = new List<string>();
        public List<string> combo_Konnektoren_Afo = new List<string>();
        public List<string> combo_Taxonomy = new List<string>();
        public List<string> combo_Satisfy = new List<string>();
        public List<string> combo_BPMN_Elemenet = new List<string>();
        public List<string> combo_BPMN_Connectoren = new List<string>();
        public List<string> m_Connectoren_Req7 = new List<string>();
        public List<string> m_Connectoren_Req7_Afo = new List<string>();

        public List<string> m_W_Aktiviaet = new List<string>();
        public List<string> m_Artikel = new List<string>();
        public List<string> m_Artikel_Akt = new List<string>();




        public Create_Metamodel_Class()
        {
            #region Definitionen

            #region UML
            ///////////////////////////////////////////////
            ////////////////////////Konnektoren
            //Typen der Konnektoren
            this.m_Type_Konnektoren.Add("Dependency");
            this.m_Type_Konnektoren.Add("Realisation");
            this.m_Type_Konnektoren.Add("Aggregation");
            this.m_Type_Konnektoren.Add("Composition");
            this.m_Type_Konnektoren.Add("InformationFlow");
            this.m_Type_Konnektoren.Add("Abstraction");
            //Stereotypen
            //Standardmäßig leer
            //Toolbox
            this.m_Toolbox.Add("ADMBw");
            //Default Name
            //Standardmäßig leer
            //////////////////////////////////////////////
            //////////////////////Element
            /////Typen der Konnektoren
            this.m_Type_Elemente.Add("Class");
            this.m_Type_Elemente.Add("Requirement");
            this.m_Type_Elemente.Add("Part");
            this.m_Type_Elemente.Add("Activity");
            this.m_Type_Elemente.Add("Action");
            this.m_Type_Elemente.Add("ActivityPartition");
            // /////////////////////////Requirement
            //this.m_Type_Requirement;
            //Standardmäßig leer
            ////////////////////////////
            ///InformationItem
            this.m_Type_InformationItem.Add("InformationItem");
            /////////////////////////////////////////////
            ///XAC
            this.sys_Enum = new SYS_ENUM();
            this.afo_Enum = new AFO_ENUM();
            this.sT_ENUM = new ST_ENUM();
            /////////////////////////////////////////////
            //Combobox Anforderungen
            this.combo_Konnektoren_Afo.Add("Refines");
            this.combo_Konnektoren_Afo.Add("Requires");
            this.combo_Konnektoren_Afo.Add("Replaces");
            this.combo_Konnektoren_Afo.Add("Dublette");
            this.combo_Konnektoren_Afo.Add("Konflikt");
            //////////////////////////////////////////
            //Combobox Elemente
            this.combo_Taxonomy.Add("Fähigkeitsbaum");
            this.combo_Taxonomy.Add("Komposition");
            //////////////////////////////////////////
            //Combobox Satisfy
            this.combo_Satisfy.Add("Satisfy DesignConstraint");
            this.combo_Satisfy.Add("Satisfy ProcessConstraint");
            this.combo_Satisfy.Add("Satisfy UmweltConstraint");
            #endregion UML

            #region Require7
            this.m_Connectoren_Req7_Afo.Add("Afo_widerspricht_Afo");
            this.m_Connectoren_Req7_Afo.Add("Afo_ist_Dublette_von_Afo");
            this.m_Connectoren_Req7_Afo.Add("Afo_wird_verfeinert_durch_Afo");
            this.m_Connectoren_Req7_Afo.Add("Afo_wird_ersetzt_durch_Afo");
            this.m_Connectoren_Req7_Afo.Add("Afo_ist_Voraussetzung_fuer_Afo");
            this.m_Connectoren_Req7_Afo.Add("Afo_stammt_von_Afo");

            this.m_Connectoren_Req7.Add("Afo_widerspricht_Afo");
            this.m_Connectoren_Req7.Add("Afo_ist_Dublette_von_Afo");
            this.m_Connectoren_Req7.Add("Afo_wird_verfeinert_durch_Afo");
            this.m_Connectoren_Req7.Add("Afo_wird_ersetzt_durch_Afo");
            this.m_Connectoren_Req7.Add("Afo_ist_Voraussetzung_fuer_Afo");
            this.m_Connectoren_Req7.Add("Sys_wird_detailliert_durch_Sys");
            this.m_Connectoren_Req7.Add("Afo_wird_erfuellt_durch_Sys");
            this.m_Connectoren_Req7.Add("Afo_stammt_von_St");
            this.m_Connectoren_Req7.Add("Afo_ist_Voraussetzung_fuer_Afo");

            this.m_W_Aktiviaet.Add("-");
            this.m_W_Aktiviaet.Add("die Möglichkeit bieten");
            this.m_W_Aktiviaet.Add("fähig sein");

            this.m_Artikel.Add("der");
            this.m_Artikel.Add("die");
            this.m_Artikel.Add("das");
            this.m_Artikel.Add("die");
            this.m_Artikel.Add("die");
            this.m_Artikel.Add("die");


            this.m_Artikel_Akt.Add("dem");
            this.m_Artikel_Akt.Add("der");
            this.m_Artikel_Akt.Add("dem");
            this.m_Artikel_Akt.Add("den");
            this.m_Artikel_Akt.Add("den");
            this.m_Artikel_Akt.Add("den");
            #endregion

            #region BPMN
            this.combo_BPMN_Elemenet.Add("Pool");
            this.combo_BPMN_Elemenet.Add("Lane");
            this.combo_BPMN_Elemenet.Add("Activity");

            this.combo_BPMN_Connectoren.Add("Pool Repräsentierung");
            #endregion BPMN

            #endregion
        }

        public Metamodel Get_Metamodel(Metamodel metamodel)
        {
            Create_Metamodel create_Metamodel = new Create_Metamodel(metamodel, this);

            create_Metamodel.ShowDialog();

           // this.m_Type_Requirement = metamodel.m_Requirement;

            return (metamodel);
        }

    }
}

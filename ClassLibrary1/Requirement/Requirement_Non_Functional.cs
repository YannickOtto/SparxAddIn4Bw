using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ennumerationen;

namespace Requirements
{
     public class Requirement_Non_Functional : Requirement
    {
        public List<string> m_GUID_Sys;
        public List<string> m_GUID_Act;
        public List<string> m_GUID_OpCon;
        public List<string> m_GUID_Cap;


        public Requirement_Non_Functional(string recent_GUID, Metamodels.Metamodel metamodel) : base(recent_GUID, metamodel)
        {
            this.m_GUID_Act = new List<string>();
            this.m_GUID_OpCon = new List<string>();
            this.m_GUID_Sys = new List<string>();
            this.m_GUID_Cap = new List<string>();
            if(metamodel != null)
            {
                this.AFO_CPM_PHASE = metamodel.CPM_PHASE;
            }
           
        }

        public void Add_Requirement_NonFunctional(string Titel, string Text, string W_OBJECT, string W_PROZESSWORT, string QUALITÄT, string RANDBEDINGUNG, bool W_SINGULAR, string W_SUBJECT)

        {
            this.AFO_FUNKTIONAL = AFO_FUNKTIONAL.nicht_funktional;
            this.AFO_WV_ART = AFO_WV_ART.Anforderung;
            this.W_AFO_MANUAL = true;
            this.W_AKTIVITAET = W_AKTIVITAET.NULL;

            this.AFO_TITEL = Titel;
            this.AFO_TEXT = Text;
            this.Notes = Text;
            this.Name = Titel;
            this.W_OBJEKT = W_OBJECT;
            this.W_PROZESSWORT = W_PROZESSWORT;
            this.W_QUALITAET = W_QUALITAET;
            this.W_RANDBEDINGUNG = W_RANDBEDINGUNG;
            this.W_SINGULAR = W_SINGULAR;
            this.W_SUBJEKT = W_SUBJECT;
            this.W_ZU = false;
        }


        public void Add_Requirement_Design(string Titel, string Text, string W_OBJECT, string W_PROZESSWORT, string QUALITÄT, string RANDBEDINGUNG, bool W_SINGULAR, string W_SUBJECT, string RPI_stereo)

        {
            //this.ADMBw_Stereotype = 

            this.AFO_FUNKTIONAL = AFO_FUNKTIONAL.nicht_funktional;
            this.AFO_WV_ART = AFO_WV_ART.Anforderung;
            this.W_AFO_MANUAL = true;
            this.W_AKTIVITAET = W_AKTIVITAET.NULL;

            this.AFO_TITEL = Titel;
            this.AFO_TEXT = Text;
            this.Notes = Text;
            this.Name = Titel;
            this.W_OBJEKT = W_OBJECT;
            this.W_PROZESSWORT = W_PROZESSWORT;
            this.W_QUALITAET = W_QUALITAET;
            this.W_RANDBEDINGUNG = W_RANDBEDINGUNG;
            this.W_SINGULAR = W_SINGULAR;
            this.W_SUBJEKT = W_SUBJECT;
            this.W_ZU = false;

            this.RPI_Stereotype = RPI_stereo;
        }

        public void Add_Requirement_Process(string Titel, string Text, string W_OBJECT, string W_PROZESSWORT, string QUALITÄT, string RANDBEDINGUNG, bool W_SINGULAR, string W_SUBJECT, string RPI_stereo)
        {
            this.AFO_FUNKTIONAL = AFO_FUNKTIONAL.funktional;
            this.AFO_WV_ART = AFO_WV_ART.Anforderung;
            this.W_AFO_MANUAL = true;
            this.W_AKTIVITAET = W_AKTIVITAET.NULL;

            this.AFO_TITEL = Titel;
            this.AFO_TEXT = Text;
            this.Notes = Text;
            this.Name = Titel;
            this.W_OBJEKT = W_OBJECT;
            this.W_PROZESSWORT = W_PROZESSWORT;
            this.W_QUALITAET = W_QUALITAET;
            this.W_RANDBEDINGUNG = W_RANDBEDINGUNG;
            this.W_SINGULAR = W_SINGULAR;
            this.W_SUBJEKT = W_SUBJECT;
            this.W_ZU = false;

            this.RPI_Stereotype = RPI_stereo;
        }

        public void Add_Requirement_Umwelt(string Titel, string Text, string W_OBJECT, string W_PROZESSWORT, string QUALITÄT, string RANDBEDINGUNG, bool W_SINGULAR, string W_SUBJECT, string RPI_stereo)

        {
            this.AFO_FUNKTIONAL = AFO_FUNKTIONAL.nicht_funktional;
            this.AFO_WV_ART = AFO_WV_ART.Anforderung;
            this.W_AFO_MANUAL = true;
            this.W_AKTIVITAET = W_AKTIVITAET.NULL;

            this.AFO_TITEL = Titel;
            this.AFO_TEXT = Text;
            this.Notes = Text;
            this.Name = Titel;
            this.W_OBJEKT = W_OBJECT;
            this.W_PROZESSWORT = W_PROZESSWORT;
            this.W_QUALITAET = W_QUALITAET;
            this.W_RANDBEDINGUNG = W_RANDBEDINGUNG;
            this.W_SINGULAR = W_SINGULAR;
            this.W_SUBJEKT = W_SUBJECT;
            this.W_ZU = false;

            this.RPI_Stereotype = RPI_stereo;
        }


        public void Add_Requirement_Typvertreter(string Titel, string Text, string W_OBJECT, string W_PROZESSWORT, string QUALITÄT, string RANDBEDINGUNG, bool W_SINGULAR, string W_SUBJECT, string RPI_stereo)

        {
            this.AFO_FUNKTIONAL = AFO_FUNKTIONAL.nicht_funktional;
            this.AFO_WV_ART = AFO_WV_ART.Systemanforderung;
            this.W_AFO_MANUAL = true;
            this.W_AKTIVITAET = W_AKTIVITAET.NULL;
            this.AFO_WV_PHASE = AFO_WV_PHASE.RollOut;

            this.AFO_TITEL = Titel;
            this.AFO_TEXT = Text;
            this.Notes = Text;
            this.Name = Titel;
            this.W_OBJEKT = W_OBJECT;
            this.W_PROZESSWORT = W_PROZESSWORT;
            this.W_QUALITAET = W_QUALITAET;
            this.W_RANDBEDINGUNG = W_RANDBEDINGUNG;
            this.W_SINGULAR = W_SINGULAR;
            this.W_SUBJEKT = W_SUBJECT;
            this.W_ZU = false;

             this.RPI_Stereotype = RPI_stereo;
        }
   
        public void Add_Requirement_Qualitaet_Class(string Titel, string Text, string W_OBJECT, string W_PROZESSWORT, string QUALITÄT, string RANDBEDINGUNG, bool W_SINGULAR, string W_SUBJECT, string RPI_stereo)
        {
            this.AFO_FUNKTIONAL = AFO_FUNKTIONAL.nicht_funktional;
            this.AFO_WV_ART = AFO_WV_ART.Anforderung;
            this.W_AFO_MANUAL = false;
            this.W_AKTIVITAET = W_AKTIVITAET.Minus;
            this.AFO_WV_PHASE = AFO_WV_PHASE.phasenübergreifend;

            this.AFO_TITEL = Titel;
            this.AFO_TEXT = Text;
            this.Notes = Text;
            this.Name = Titel;
            this.W_OBJEKT = W_OBJECT;
            this.W_PROZESSWORT = W_PROZESSWORT;
            this.W_QUALITAET = QUALITÄT;
            this.W_RANDBEDINGUNG = RANDBEDINGUNG;
            this.W_SINGULAR = W_SINGULAR;
            this.W_SUBJEKT = W_SUBJECT;
            this.W_ZU = false;

            this.RPI_Stereotype = RPI_stereo;
        }

        public void Add_Requirement_Qualitaet_Activity(string Titel, string Text, string W_OBJECT, string W_PROZESSWORT, string QUALITÄT, string RANDBEDINGUNG, bool W_SINGULAR, string W_SUBJECT, string RPI_stereo)
        {
            this.AFO_FUNKTIONAL = AFO_FUNKTIONAL.funktional;
            this.AFO_WV_ART = AFO_WV_ART.Anforderung;
            this.W_AFO_MANUAL = false;
            this.W_AKTIVITAET = W_AKTIVITAET.Minus;
            this.AFO_WV_PHASE = AFO_WV_PHASE.phasenübergreifend;

            this.AFO_TITEL = Titel;
            this.AFO_TEXT = Text;
            this.Notes = Text;
            this.Name = Titel;
            this.W_OBJEKT = W_OBJECT;
            this.W_PROZESSWORT = W_PROZESSWORT;
            this.W_QUALITAET = QUALITÄT;
            this.W_RANDBEDINGUNG = RANDBEDINGUNG;
            this.W_SINGULAR = W_SINGULAR;
            this.W_SUBJEKT = W_SUBJECT;
            this.W_ZU = false;

            this.RPI_Stereotype = RPI_stereo;
        }

    }
}

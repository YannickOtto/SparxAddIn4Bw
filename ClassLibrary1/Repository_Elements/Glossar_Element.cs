using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repsoitory_Elements
{
    public class Glossar_Element
    {
        public string object_ID;
        public string gloe_AG_ID;
        public string gloe_AN_ID;
        public string gloe_BEGRIFF;
        public string gloe_GLOSSARTEXT;
        public string gloe_SPRACHE;
        public string gloe_PUBLICSTATUS;
        public string gloe_STATUS;
        public string gloe_TYP;
        public string in_CATEGORY;
        public string gloe_KUERZEL;
        public string gloe_KONTEXT;
        public string gloe_HYPERLINK;
        public string gloe_INT_FACHBEGRIFF;
        public string gloe_AUTOR;
        public string gloe_ZUSATZINFORMATIONEN;
        public string gloe_ERSTELLUNGSDATUM;


        public Glossar_Element( string id, Requirement_Plugin.Database database)
        {
            Repository_Elements repository_Elements = new Repository_Elements();

            string name = repository_Elements.Get_Glossar_Property(database, "Term", id);
            string type = repository_Elements.Get_Glossar_Property(database, "Type", id);
            string bezeichnung = repository_Elements.Get_Glossar_Property(database, "Meaning", id);

            id = "-" + id + "00";

            this.object_ID = id;
            this.gloe_AG_ID = "";
            this.gloe_AN_ID = "";
            this.gloe_BEGRIFF = name;
            this.gloe_GLOSSARTEXT = bezeichnung;
            this.gloe_SPRACHE = "Deutsch";
            this.gloe_PUBLICSTATUS = "public";
            this.gloe_STATUS = "neu";
            this.gloe_TYP = "Glossarelement";
            this.in_CATEGORY = "0";
            this.gloe_KUERZEL = name;
            this.gloe_KONTEXT = "";
            this.gloe_HYPERLINK = "";
            this.gloe_INT_FACHBEGRIFF = "";
            this.gloe_AUTOR = "Projektteam";
            this.gloe_ZUSATZINFORMATIONEN = "";
            this.gloe_ERSTELLUNGSDATUM = "";
        }

        ~Glossar_Element()
        {

        }

      

    }
}

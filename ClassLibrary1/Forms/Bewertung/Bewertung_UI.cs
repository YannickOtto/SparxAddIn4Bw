using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Requirement_Plugin.Forms.Bewertung_Afo
{
    public partial class Bewertung_UI : Form
    {
        public Requirement_Plugin.Domain.Bewertung_Afo.Bewertung bewertung;
        private Database Database;
        private EA.Repository repository;

        public Bewertung_UI(Requirement_Plugin.Domain.Bewertung_Afo.Bewertung bewertung1, Database database, EA.Repository rep)
        {
            InitializeComponent();

            this.bewertung = bewertung1;
            this.Database = database;
            this.repository = rep;

            if(this.bewertung.m_req_gewicht.Count > 0)
            {
                this.richTextBox_Hinweis.Text = "Es sind Anforderungen vorhanden, welche bereits ein absolutes Gewicht aufweisen. Dies muss zunächst über den Reset Knopf bereinigt werden.";
                this.richTextBox_Hinweis.Update();

                this.button_Gewichtung.Enabled = false;
                this.button_Gewichtung.Update();
                this.button_Reset.Enabled = true;
                this.button_Reset.Update();
            }
        }

        private void Bewertung_UI_Load(object sender, EventArgs e)
        {

        }

        #region Botton
        private void button_Reset_Click(object sender, EventArgs e)
        {
            //Zurücksetzen
            this.bewertung.Reset_Gewicht(this.Database, this.repository);
            //Text
            this.richTextBox_Hinweis.Text = "";
            this.richTextBox_Hinweis.Update();
            //Reset deaktivieren
            this.button_Reset.Enabled = false;
            this.button_Reset.Update();
            //Gewichtung aktivieren
            this.button_Gewichtung.Enabled = true;
            this.button_Gewichtung.Update();
        }

        private void button_Gewichtung_Click(object sender, EventArgs e)
        {
            //Gewichten
            this.bewertung.Start_Gewichtung(this.Database, this.repository);

            //Text
            this.richTextBox_Hinweis.Text = "Es sind Anforderungen vorhanden, welche bereits ein absolutes Gewicht aufweisen. Dies muss zunächst über den Reset Knopf bereinigt werden.";
            this.richTextBox_Hinweis.Update();
            //Reset aktivieren
            this.button_Reset.Enabled = true;
            this.button_Reset.Update();
            //Gewichtung deaktivieren
            this.button_Gewichtung.Enabled = false;
            this.button_Gewichtung.Update();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }
        #endregion
    }
}

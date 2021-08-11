using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Compass.CommomLibrary;

namespace Compass.DecompToolsShellX
{
    public partial class FrmOnsReCcee : Form
    {
        public CommomLibrary.Newave.Deck deckONS { get; set; }



        public CommomLibrary.Newave.Deck deckCCEEAnterior { get; set; }

        public CommomLibrary.Newave.Deck deckONSAnterior { get; set; }

        public FrmOnsReCcee()
        {
            InitializeComponent();
        }


        public FrmOnsReCcee(CommomLibrary.Newave.Deck deckONS) : this()
        {
            this.deckONS = deckONS;
            CarregaDecksOld();
        }


        public void Salvar(CommomLibrary.Newave.Deck deckCcee, CommomLibrary.Newave.Deck deckOns)
        {
            try
            {
                this.deckCCEEAnterior = deckCcee;
                this.deckONSAnterior = deckOns;

                if (deckONSAnterior == null || deckCCEEAnterior == null)
                    throw new Exception("Caminho do Deck está incorreto ou é invalido");


                Compass.Services.Deck.Ons2Ccee(deckONS, deckCCEEAnterior);


                var redatCCEEAnterior = ((Compass.CommomLibrary.Newave.Deck)deckCCEEAnterior)[CommomLibrary.Newave.Deck.DeckDocument.re].Document as CommomLibrary.ReDat.ReDat;
                var redatONSAnterior = ((Compass.CommomLibrary.Newave.Deck)deckONSAnterior)[CommomLibrary.Newave.Deck.DeckDocument.re].Document as CommomLibrary.ReDat.ReDat;
                var redat = ((Compass.CommomLibrary.Newave.Deck)deckONS)[CommomLibrary.Newave.Deck.DeckDocument.re].Document as CommomLibrary.ReDat.ReDat;

                Services.Deck.VerificarRestricaoEletrica(redatCCEEAnterior, redatONSAnterior, redat);

                this.Close();

            }
            catch (Exception i)
            {
                Program.AutoClosingMessageBox.Show(i.Message, "Caption", 3000);
                //MessageBox.Show(i.Message);
                this.Close();
            }
        }

        


        public void Salvar() {
            Salvar(DeckFactory.CreateDeck(TextBoxCCEE.Text) as CommomLibrary.Newave.Deck, DeckFactory.CreateDeck(TextBoxONS.Text) as CommomLibrary.Newave.Deck);
        }





        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void FrmOnsReCcee_Load(object sender, EventArgs e)
        {
            CarregaDecksOld();
        }

        private void CarregaDecksOld() {

            var data = deckONS.Dger.DataEstudo.AddMonths(-1);
            // var data = DateTime.Today.AddMonths(-1);
            var nomeMes = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(data.Month).ToLower();

            TextBoxCCEE.Text = System.IO.Path.Combine(ConfigurationManager.AppSettings["nvPath"], "CCEE_NW", data.ToString("yyyy"), data.ToString("MM") + "_" + nomeMes, "NW" + data.ToString("yyyyMM"));
            TextBoxONS.Text = System.IO.Path.Combine(ConfigurationManager.AppSettings["nvPath"], "ONS_NW", data.ToString("yyyy"), data.ToString("MM_yyyy"), "deck_newave_" + data.ToString("yyyy_MM"));
        }

    }
}

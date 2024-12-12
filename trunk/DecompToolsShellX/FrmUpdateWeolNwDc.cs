using Compass.CommomLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX
{
    public partial class FrmUpdateWeolNwDc : Form
    {
        public CommomLibrary.Newave.Deck deckNW { get; set; }
        public CommomLibrary.Decomp.Deck deckDC { get; set; }
        public string CSVFILE { get; set; }
        public FrmUpdateWeolNwDc()
        {
            InitializeComponent();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(TextBoxDeck.Text) && Directory.Exists(TextBoxWEOL.Text) )
            {
                var csvsFiles = Directory.GetFiles(TextBoxWEOL.Text).Where(x => Path.GetFileName(x).ToLower().EndsWith(".csv")).ToList();
                if (csvsFiles.Count() > 0)
                {
                    CSVFILE = csvsFiles.First();
                    var deck = DeckFactory.CreateDeck(TextBoxDeck.Text);

                    if (deck is Compass.CommomLibrary.Newave.Deck)
                    {
                        deckDC = null;
                        Compass.Services.Deck.AtualizaWeolNWDCProcess(deck as Compass.CommomLibrary.Newave.Deck, deckDC, CSVFILE);
                    }
                    else if (deck is Compass.CommomLibrary.Decomp.Deck)
                    {
                        deckNW = null;
                        Compass.Services.Deck.AtualizaWeolNWDCProcess(deckNW, deck as Compass.CommomLibrary.Decomp.Deck, CSVFILE);
                    }
                    else
                    {
                        MessageBox.Show("Diretorios invalidos ou vazios!", "Atualizar Weol decks NW DC ");
                    }
                }
                else
                {
                    MessageBox.Show("Diretorios invalidos ou vazios!", "Atualizar Weol decks NW DC ");
                }
            }
            else
            {
                MessageBox.Show("Processo Interrompido! \n Verifique entradas.", "Atualizar Weol decks NW DC ");
            }
        }

        private void FrmUpdateWeolNwDc_Load(object sender, EventArgs e)
        {
            CarregaDecks();
        }

        private void CarregaDecks()
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");

            //var data = deckNW.Dger.DataEstudo;
            // var data = DateTime.Today.AddMonths(-1);
            //var nomeMes = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(data.Month).ToLower();
            //var nomeMes = Culture.DateTimeFormat.GetMonthName(data.Month).ToLower();

            var dataWEOL = DateTime.Today;
            // DateTime dataRef = data;
            int contDc = 0;
            bool OkDc = false;



            while (OkDc == false && contDc < 30)
            {
                //DateTime dat = dataDC;
                //DateTime datVE = dataDC;
                //if (dat.DayOfWeek == DayOfWeek.Friday)
                //{
                //    datVE = dat.AddDays(-1);
                //}
                //var rev = GetCurrRev(datVE);
                //H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\2023\09_set\Relatorio_Sumario-202309-sem5
                //int semana = rev.rev + 1;
                var mes = GetMonthNumAbrev(dataWEOL.Month);//dataRef

                var cam = $@"H:\Middle - Preço\Resultados_Modelos\WEOL-SM\{dataWEOL:yyyy}\{mes}\Deck_PrevMes_{dataWEOL:yyyyMMdd}\Arquivos Saida\Previsoes Subsistemas Finais\Total";
                if (Directory.Exists(cam))
                {
                    var csvsFiles = Directory.GetFiles(cam).Where(x => Path.GetFileName(x).ToLower().EndsWith(".csv")).ToList();

                    if (csvsFiles.Count() > 0 )
                    {
                        CSVFILE = csvsFiles.First();
                        TextBoxWEOL.Text = cam;
                        OkDc = true;
                    }
                    
                }
                else
                {
                    contDc++;
                    dataWEOL = dataWEOL.AddDays(-1);
                }


            }
            if (deckDC != null)
            {
                TextBoxDeck.Text = deckDC.BaseFolder;
            }
            else if (deckNW != null)
            {
                TextBoxDeck.Text = deckNW.BaseFolder;
            }


            //TextBoxNW.Text = deckNW.BaseFolder;
            //TextBoxDC.Text = Path.Combine("H:\\Middle - Preço\\Resultados_Modelos\\DECOMP\\CCEE_DC", ano, mes + "_" + mesExtenso, "DC" + ano + mes + "-sem" + sem);
        }

        public static (DateTime revDate, int rev) GetCurrRev(DateTime date)
        {
            var currRevDate = date;

            do
            {
                currRevDate = currRevDate.AddDays(1);
            } while (currRevDate.DayOfWeek != DayOfWeek.Friday);
            var currRevNum = currRevDate.Day / 7 - (currRevDate.Day % 7 == 0 ? 1 : 0);

            return (currRevDate, currRevNum);
        }

        public static string GetMonthNumAbrev(int month)
        {

            switch (month)
            {
                case 1: return "01_jan";
                case 2: return "02_fev";
                case 3: return "03_mar";
                case 4: return "04_abr";
                case 5: return "05_mai";
                case 6: return "06_jun";
                case 7: return "07_jul";
                case 8: return "08_ago";
                case 9: return "09_set";
                case 10: return "10_out";
                case 11: return "11_nov";
                case 12: return "12_dez";

                default:
                    return null;
            }
        }
    }
}

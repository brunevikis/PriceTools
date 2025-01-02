using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
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
    public partial class FrmUpdateConfhd : Form
    {
        public CommomLibrary.Newave.Deck deckNW { get; set; }
        public CommomLibrary.Decomp.Deck deckDC { get; set; }

        public FrmUpdateConfhd()
        {
            InitializeComponent();
        }

        private void TextBoxDC_Load(object sender, EventArgs e)
        {

        }

        private void FrmUpdateConfhd_Load(object sender, EventArgs e)
        {
            CarregaDecks();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string NWdestino = TextBoxNW.Text;
                string decompRef = TextBoxDC.Text;
                var decompDeck = DeckFactory.CreateDeck(decompRef) as Compass.CommomLibrary.Decomp.Deck;
                var rv = decompDeck.Caso;
                var deckNW = DeckFactory.CreateDeck(NWdestino) as Compass.CommomLibrary.Newave.Deck;

                var confihdFile = Directory.GetFiles(NWdestino).Where(x => Path.GetFileName(x).ToLower().Equals("confhd.dat")).FirstOrDefault();
                var dadgerFile = Directory.GetFiles(decompRef).Where(x => Path.GetFileName(x).ToLower().Equals("dadger." + rv)).FirstOrDefault();
                var hidrFile = Directory.GetFiles(decompRef).Where(x => Path.GetFileName(x).ToLower().Equals("hidr.dat")).FirstOrDefault();

                if (confihdFile != null && dadgerFile != null && hidrFile != null)
                {
                    DateTime dataNw = new DateTime(deckNW.Dger.DataEstudo.Year, deckNW.Dger.DataEstudo.Month, 1);

                    var dadger = DocumentFactory.Create(dadgerFile) as Compass.CommomLibrary.Dadger.Dadger;
                    DateTime dataDC = new DateTime(dadger.DataEstudo.Year, dadger.DataEstudo.Month, 1);
                    var revisao = Tools.GetCurrRev(dadger.DataEstudo);
                    if (revisao.rev == 0)
                    {
                        dataDC = new DateTime(revisao.revDate.Year, revisao.revDate.Month, 1);
                    }
                    if (dataDC != dataNw)
                    {
                        if (System.Windows.Forms.MessageBox.Show($"Divergência de datas \r\nAs datas bases dos decks Newave e Decomp não são correspondentes\r\nDeseja continuar?"
                   , "Atualizar Confhd", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                    }

                    var hidr = DocumentFactory.Create(hidrFile) as Compass.CommomLibrary.HidrDat.HidrDat;

                    var configH = new Compass.CommomLibrary.Decomp.ConfigH(dadger, hidr);
                    var confihdNew = (Compass.CommomLibrary.ConfhdDat.ConfhdDat)DocumentFactory.Create(confihdFile);

                    var dadgerRef = configH.baseDoc as Dadger;
                    //var dadgertest = configHNew.baseDoc as Dadger;
                    foreach (var conf in confihdNew)
                    {

                        int codUH;
                        var Usi = configH.usinas[conf.Cod];
                        if (Usi.IsFict)
                        {
                            codUH = Usi.CodReal ?? 0;
                        }
                        else
                        {
                            codUH = Usi.Cod;
                        }
                        double VolInicial = dadgerRef.BlocoUh.Where(x => x.Usina == codUH).Select(x => x.VolIniPerc).FirstOrDefault();
                        if (conf.Cod == 291)//fict. serra da mesa
                        {
                            if (VolInicial < 55)
                            {
                                VolInicial = VolInicial / 0.55f;
                            }
                            else
                            {
                                VolInicial = 100;
                            }
                        }
                        conf.VolUtil = VolInicial;
                    }
                    confihdNew.SaveToFile(createBackup: true);
                    System.Windows.Forms.MessageBox.Show("Processo concuído com sucesso!!!");
                }
            }
            catch (Exception ehd)
            {
                var texto = ehd.ToString();
                if (ehd.ToString().Contains("reconhecido"))
                {
                    texto = "Deck não reconhecido para a execução!";
                }
                MessageBox.Show(texto, "Atenção");
            }
        }
        private void CarregaDecks()
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");

            var data = deckNW.Dger.DataEstudo;
            // var data = DateTime.Today.AddMonths(-1);
            //var nomeMes = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(data.Month).ToLower();
            var nomeMes = Culture.DateTimeFormat.GetMonthName(data.Month).ToLower();

            var dataDC = DateTime.Today;
            // DateTime dataRef = data;
            string xPublicacao = "";
            int contDc = 0;
            bool OkDc = false;



            while (OkDc == false && contDc < 30)
            {
                DateTime dat = dataDC;
                DateTime datVE = dataDC;
                if (dat.DayOfWeek == DayOfWeek.Friday)
                {
                    datVE = dat.AddDays(-1);
                }
                var rev = GetCurrRev(datVE);
                //H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\2023\09_set\Relatorio_Sumario-202309-sem5
                int semana = rev.rev + 1;
                var mes = GetMonthNumAbrev(rev.revDate.Month);//dataRef
                for (int p = 5; p >= 1; p--)//verfirifica se existe republicaçoes e prioriza na escolha do deck
                {
                    var camX = $@"H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\{rev.revDate:yyyy}\{mes}\DC{rev.revDate:yyyyMM}-sem" + semana.ToString() + $"_{p}aPublicacao";
                    if (Directory.Exists(camX))
                    {
                        xPublicacao = camX;
                        OkDc = true;
                        break;
                    }
                }
                if (xPublicacao != "")
                {
                    TextBoxDC.Text = xPublicacao;
                }
                else
                {
                    var cam = $@"H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\{rev.revDate:yyyy}\{mes}\DC{rev.revDate:yyyyMM}-sem" + semana.ToString();
                    if (Directory.Exists(cam))
                    {
                        TextBoxDC.Text = cam;
                        OkDc = true;
                    }
                    else
                    {
                        contDc++;
                        dataDC = dataDC.AddDays(-1);
                    }
                }

            }


            TextBoxNW.Text = deckNW.BaseFolder;
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

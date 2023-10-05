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
    public partial class FrmMapCorteDS : Form
    {
        string WorkDir;

        public FrmMapCorteDS()
        {
            InitializeComponent();
        }

        public FrmMapCorteDS(string deckDs) : this()
        {
            this.WorkDir = deckDs;
            CarregaDecksRef();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string texto = "Processo cancelado!";
            MessageBox.Show(texto, "PriceTools");
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        public void Salvar()
        {
            string decompRef = TextBoxDirRef.Text;


            string mapcut = "mapcut";
            string cortdeco = "cortdeco";
            //throw new NotImplementedException("Deck não reconhecido para a execução");
            //
            try
            {
                if (Directory.Exists(decompRef))
                {
                    var arqMapcut = Directory.GetFiles(decompRef).Where(x => Path.GetFileNameWithoutExtension(x).ToLower() == (mapcut)).FirstOrDefault();
                    var arqCortdeco = Directory.GetFiles(decompRef).Where(x => Path.GetFileNameWithoutExtension(x).ToLower() == (cortdeco)).FirstOrDefault();

                    if (File.Exists(arqMapcut) && File.Exists(arqCortdeco))// copia arquivos e altera dessem arq com os nome em minusculo (dessemarq é case sensitive)
                    {
                        mapcut = Path.GetFileName(arqMapcut);
                        cortdeco = Path.GetFileName(arqCortdeco);

                        File.Copy(arqMapcut, Path.Combine(WorkDir, mapcut.ToLower()), true);
                        File.Copy(arqCortdeco, Path.Combine(WorkDir, cortdeco.ToLower()), true);

                        var dessemArqFile = Directory.GetFiles(WorkDir).Where(x => Path.GetFileName(x).ToLower().Contains("dessem.arq")).First();
                        var dessemArq = DocumentFactory.Create(dessemArqFile) as Compass.CommomLibrary.DessemArq.DessemArq;

                        var mapline = dessemArq.BlocoArq.Where(x => x.Minemonico.ToUpper().Trim() == "MAPFCF").First();
                        var cortline = dessemArq.BlocoArq.Where(x => x.Minemonico.ToUpper().Trim() == "CORTFCF").First();
                        mapline.NomeArq = mapcut.ToLower();
                        cortline.NomeArq = cortdeco.ToLower();

                        foreach (var file in Directory.GetFiles(WorkDir).ToList())
                        {
                            var fileName = Path.GetFileName(file);
                            var minusculo = fileName.ToLower();
                            File.Move(Path.Combine(WorkDir, fileName), Path.Combine(WorkDir, minusculo));
                        }
                        foreach (var line in dessemArq.BlocoArq.ToList())
                        {
                            if (line.Minemonico.Trim() != "CASO" && line.Minemonico.Trim() != "TITULO")
                            {
                                string mini = line.NomeArq.ToLower();
                                line.NomeArq = mini;
                            }
                            
                        }
                        dessemArq.SaveToFile(createBackup: true);

                        string texto = "Processo Finalizado com sucesso!";
                        MessageBox.Show(texto, "PriceTools");
                    }
                    else
                    {
                        string texto = "Falha ao copiar arquivos, diretório ou arquivos decomp (Mapcut Cortdeco)inexistentes";
                        MessageBox.Show(texto, "PriceTools");
                    }

                }
                else
                {
                    string texto = "Falha ao copiar arquivos, diretório ou arquivos decomp (Mapcut Cortdeco)inexistentes";
                    MessageBox.Show(texto, "PriceTools");
                }

                this.Close();
            }
            catch (Exception e)
            {
                string texto = "Falha ao copiar arquivos," + e.Message.ToString();
                MessageBox.Show(texto, "PriceTools");
                this.Close();
            }


        }

        public void CarregaDecksRef()
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var data = DateTime.Today;
            var dataDC = DateTime.Today;
            DateTime dataRef = data;


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
                var cam = $@"H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\{rev.revDate:yyyy}\{mes}\Relatorio_Sumario-{rev.revDate:yyyyMM}-sem" + semana.ToString();
                if (Directory.Exists(cam))
                {
                    TextBoxDirRef.Text = cam;
                    OkDc = true;
                }
                else
                {
                    contDc++;
                    dataDC = dataDC.AddDays(-1);
                }
            }

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

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
    public partial class FrmDsOns2CCEE : Form
    {
        public FrmDsOns2CCEE()
        {
            InitializeComponent();
        }

        public FrmDsOns2CCEE(string deckONS) : this()
        {
            this.WorkDir = deckONS;
            CarregaDecksRef();
        }


        private void btnSalvar_Click(object sender, EventArgs e)
        {

            Salvar();
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
                var cam = $@"H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\{rev.revDate:yyyy}\{mes}\Relatorio_Sumario-202309-sem" + semana.ToString();
                if (Directory.Exists(cam))
                {
                    TextBoxDcRef.Text = cam;
                    OkDc = true;
                }
                else
                {
                    contDc++;
                    dataDC = dataDC.AddDays(-1);
                }
            }



            //DateTime Ve;
            //if (data.DayOfWeek == DayOfWeek.Friday)
            //{
            //    Ve = data.AddDays(-1);
            //}
            //else
            //{
            //    Ve = data;
            //}
            //var revDc = GetCurrRev(Ve);
            ////string mapcut = "mapcut.rv" + rev.rev.ToString();
            ////tring cortdeco = "cortdeco.rv" + rev.rev.ToString();

            //for (int i = 1; i <= 10; i++)
            //{
            //    //H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\2023\09_set\Relatorio_Sumario-202309-sem5
            //    string camDecomp = @"K:\4_curto_prazo\" + revDc.revDate.ToString("yyyy_MM") + "\\DEC_ONS_" + revDc.revDate.ToString("MMyyyy") + "_RV" + revDc.rev.ToString() + $"_VE_ccee ({i})";
            //    if (Directory.Exists(camDecomp))
            //    {
            //        TextBoxDcRef.Text = camDecomp;
            //    }
            //}

            int cont = 0;
            bool Ok = false;
            while (Ok == false && cont < 30)
            {
                DateTime dat = dataRef;
                DateTime datVE = dataRef;
                if (dat.DayOfWeek == DayOfWeek.Friday)
                {
                    datVE = dat.AddDays(-1);
                }
                var rev = GetCurrRev(datVE);
                //H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\2021\01_jan\RV3\DS_CCEE_012021_SEMREDE_RV3D19
                var mes = GetMonthNumAbrev(rev.revDate.Month);//dataRef
                var cam = $@"H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
                if (Directory.Exists(cam))
                {
                    TextBoxDsRef.Text = cam;
                    Ok = true;
                }
                else
                {
                    cont++;
                    dataRef = dataRef.AddDays(-1);
                }
            }

        }
        string WorkDir;

        public void Salvar()
        {
            string decompRef = TextBoxDcRef.Text;
            string dessemRef = TextBoxDsRef.Text;


            string mapcut = "mapcut.rv";
            string cortdeco = "cortdeco.rv";

            if (Directory.Exists(decompRef))
            {
                var arqs = Directory.GetFiles(decompRef).ToList();
                foreach (var arq in arqs)
                {
                    var filename = Path.GetFileName(arq);
                    if ((filename.ToLower().Contains(mapcut)) || (filename.ToLower().Contains(cortdeco)))
                    {
                        File.Copy(arq, Path.Combine(WorkDir, filename), true);
                    }
                }
            }
            if (Directory.Exists(dessemRef))
            {
                File.WriteAllText(Path.Combine(WorkDir, "dir.txt"), dessemRef);
            }
            else
            {
                File.WriteAllText(Path.Combine(WorkDir, "dir.txt"), "");
            }
            this.Close();
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

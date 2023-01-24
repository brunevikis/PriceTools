using Compass.CommomLibrary;
using Compass.ExcelTools.Templates;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace Compass.Services
{
    public class Previvaz
    {
        static List<Propagacao> Propagacoes = new List<Propagacao>();
        static List<Regressao> RegressoesA1 = new List<Regressao>();
        static List<Regressao> RegressoesA0 = new List<Regressao>();
        static List<PostoRegre> PostoRegredidos = new List<PostoRegre>();
        static string usr = System.Environment.UserName.Replace('.', '_');
        static string tempFolder = @"L:\shared\CHUVA-VAZAO\previvaz_" + usr;
        static Compass.CommomLibrary.Previvaz.Deck prevDeck;

        static Dictionary<int, Tuple<int[], int>> postosIncrementais = new Dictionary<int, Tuple<int[], int>>(){ // <num posto, { postos montantes ... }, num posto inc >
                    {34, new Tuple<int[], int>( new int[] {18, 33, 99, 241, 261}, 139)}, //i solteira//antigo 135 mudou pois colocaram um posto existente com esse numero 
                    {243,new Tuple<int[], int>( new int[] {242}, 138)}, // 3 irmaos
                    {245,new Tuple<int[], int>( new int[] {34, 243}, 136)}, // jupia
                    {246,new Tuple<int[], int>( new int[] {245, 154}, 137)}, // p primaveira
                    {266,new Tuple<int[], int>( new int[] {63, 246}, 166 )}, // itaipu
                    {253,new Tuple<int[], int>( new int[] {191}, 308 )},// sao salvador
                    {273,new Tuple<int[], int>( new int[] {257}, 309 )},// lajeado
                    {271,new Tuple<int[], int>( new int[] {273}, 310 )},// estreito
                    {275,new Tuple<int[], int>( new int[] {271}, 311 )},// tucurui
                    //{257,new Tuple<int[], int>( new int[] {253}, 308 )},// peixe angical
                    // { 169, new Tuple<int[], int>( new int[] {156, 158}, 168 )}, // sobradinho
                    // { 239, new int[] {237 } },
                    // { 242, new int[] {239 } },
                };

        static List<Acomph> acompH = null;

        public static void RunCenario(string caminhoWbCenario, bool useAcomph, bool encad = false)// bool encad)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = null;
            try
            {
                xlApp = ExcelTools.Helper.StartExcel();
                //copia excel para nome unico /temporario

                var sufixo = $"{DateTime.Now:HHmmss}";
                var nomeTemp = Path.Combine(
                    Path.GetDirectoryName(caminhoWbCenario),
                    Path.GetFileNameWithoutExtension(caminhoWbCenario) + $"_{sufixo}.xlsm"
                    );

                System.IO.File.Copy(caminhoWbCenario, nomeTemp);

                var wbxls = xlApp.Workbooks.Open(nomeTemp);
                var wb = new WorkbookPrevsCenariosMen(wbxls);
                RunCenario(wb, useAcomph, encad, sufixo);// encad);
                wbxls.Save();
                wbxls.Close(SaveChanges: false);

                System.IO.File.Delete(caminhoWbCenario);
                System.IO.File.Move(nomeTemp, caminhoWbCenario);

            }
            finally
            {
                if (xlApp != null)
                {
                    xlApp.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
                    xlApp.ScreenUpdating = true;

                    ExcelTools.Helper.Release(xlApp);
                }
            }
        }

        public static void RunCenarioPrevsM2SemExcel(string arqLog, string arqConfig, string caminhoWbCenario, bool useAcomph, Tuple<string, object[,], int> metas, string previvazFolder, string raiz, string cenario, bool encad = false)// bool encad)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var configs = System.IO.File.ReadAllLines(arqConfig).ToList();
            string arqEntradaPath = configs[0];
            DateTime dataPrevs = Convert.ToDateTime(configs[1], Culture.DateTimeFormat);
            DateTime dataSEmEstimada = Convert.ToDateTime(configs[2], Culture.DateTimeFormat);
            int cont = System.IO.File.ReadAllLines(arqLog).Skip(1).ToList().Count();

            //object[,] vazoes = System.IO.File.ReadAllLines(arqLog).Skip(1).ToList().ForEach(x => x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(y => Convert.ToDouble(y[0]))), ++i } };
            var vazoes = System.IO.File.ReadAllLines(arqLog).Skip(1).ToList().Select(x => new object[1, 12] {
                {

                   Convert.ToDouble( x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[3]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[4]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[5]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[6]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[7]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[8]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[9]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[10]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[11]),
                    Convert.ToDouble(x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[12])
                }
            });

            // object[,] teste = vazoes.SelectMany(x=> new object[1, 1] { {x[0,0] } }); 
            object[,] teste = new object[12 * 320, 12];
            int l = 0;
            foreach (var vaz in vazoes)
            {
                int c = 0;

                foreach (var v in vaz)
                {
                    teste[l, c] = v;
                    c++;
                }
                l++;
            }



            Microsoft.Office.Interop.Excel.Application xlsApp = null;
            xlsApp = new Microsoft.Office.Interop.Excel.Application();

            try
            {
                //var xlsApp = new Microsoft.Office.Interop.Excel.Application();
                while (!xlsApp.Ready)
                {
                    System.Threading.Thread.Sleep(200);
                }

                xlsApp.Visible = true;
                xlsApp.ScreenUpdating = true;
                xlsApp.DisplayAlerts = false;
                //xlApp = ExcelTools.Helper.StartExcel();
                //copia excel para nome unico /temporario

                var sufixo = $"_{metas.Item1}";
                var nomeTemp = Path.Combine(
                    Path.GetDirectoryName(arqLog),
                     Path.GetFileNameWithoutExtension(caminhoWbCenario).Replace("_Log", $"_{sufixo}.xlsm")
                    //Path.GetFileName(caminhoWbCenario).Replace("_Log", sufixo)
                    );
                if (System.IO.File.Exists(nomeTemp))
                {
                    System.IO.File.Delete(nomeTemp);
                }


                //System.IO.File.Copy(caminhoWbCenario, nomeTemp, true);

                var wbxls = xlsApp.Workbooks.Open(caminhoWbCenario);
                wbxls.Activate();
                //xlsApp.Workbooks.
                Worksheet wsEnt1 = wbxls.Worksheets["Entrada"];
                wsEnt1.Select();
                wsEnt1.Range["H3"].Value = arqEntradaPath;

                //string arqEntradaPath = wsEnt1.Range["_entradaPrevivaz"].Text;
                int Li = wsEnt1.Range["_entrada"].Row;
                int Ci = wsEnt1.Range["_entrada"].Column + 2;
                int Lf = 320 + Li - 1;
                int Cf = wsEnt1.Range["_entrada"].Columns.Count + Ci - 3;

                wsEnt1.Range[wsEnt1.Cells[Li, Ci], wsEnt1.Cells[Lf, Cf]].Value = teste;


                //foreach (var vaz in vazoes)
                //{
                //    wsEnt1.Range[wsEnt1.Cells[Li, 3], wsEnt1.Cells[Li, 14]].Value = vaz;
                //    Li++;
                //}

                Worksheet wsPrevs = wbxls.Worksheets["Prevs"];
                wsPrevs.Select();
                wsPrevs.Range["A2"].Value = dataPrevs;
                wsPrevs.Range["A4"].Value = dataSEmEstimada;
                xlsApp.Calculate();

                //DateTime dataPrevs = wsPrevs.Range["A2"].Value;
                //DateTime dataSEmEstimada = wsPrevs.Range["A4"].Value;

                ///

                //ws.Range[ws.Cells[1, 1], ws.Cells[res.GetLength(0), res.GetLength(1)]].Value = res;

                //ws = wb.Worksheets["Aux"] as Microsoft.Office.Interop.Excel.Worksheet;
                //(ws.Range[ws.Cells[3, 2], ws.Cells[3, 2]] as Excel.Range).Value2 = this.ArquivoPrevsBase;
                //(ws.Range[ws.Cells[4, 2], ws.Cells[4, 2]] as Excel.Range).Value2 = this.ArquivosDeEntradaPrevivaz;
                //(ws.Range[ws.Cells[5, 2], ws.Cells[5, 2]] as Excel.Range).Value2 = this.ArquivosDeSaida.Split('\\').LastOrDefault();
                //



                //var wb = new WorkbookPrevsCenariosMen(wbxls);
                //Workbook wbc = xlsApp.ActiveWorkbook;
                Worksheet wsPega = wbxls.Worksheets["Cen1"];
                wsPega.Select();
                wsPega.Range["_metaCen1"].NumberFormat = "General";
                wsPega.Range["_metaCen1"].Value = metas.Item2;

                var nome = Path.Combine(
                       Path.GetDirectoryName(arqLog),
                        Path.GetFileNameWithoutExtension(caminhoWbCenario) + ".xlsm"
                       //Path.GetFileName(caminhoWbCenario).Replace("_Log", sufixo)
                       );
                if (System.IO.File.Exists(nome))
                {
                    System.IO.File.Delete(nome);
                }
                wbxls.SaveAs(nome, wbxls.FileFormat);

                //wb.METAS = metas.Item2;
                //!AjustarCenarios
                //xlApp.Run($"'CHUVAVAZAO_{code}.xlsm'!CriarCenario");
                xlsApp.Run($"'{Path.GetFileName(nome)}'!AjustarCenarios");//arrumar a chamada do nome da planilha Log é template, ela esta abrindo como Log1

                //var result = wb.PrevsCen1;
                object[,] result = wsPega.Range["_cen1"].Value;
                int linhas = wsPega.Range["_cen1"].Rows.Count;
                int colunas = wsPega.Range["_cen1"].Columns.Count;

                for (int x = 1; x <= linhas; x++)
                {
                    for (int y = metas.Item3 + 3; y <= colunas; y++)//14
                    {
                        if (result[x, y] != null)
                        {
                            result[x, y] = null;
                        }
                    }
                }
                wbxls.Close(SaveChanges: false);

                string PlanModelo = $@"H:\TI - Sistemas\UAT\PricingExcelTools\files\Gera_e_Avalia_Cenarios_Men_Sem_Extendido.xltm";
                //wbxls = xlsApp.Workbooks.Open(nomeTemp);
                wbxls = xlsApp.Workbooks.Open(PlanModelo);
                wbxls.Activate();

                wsPrevs = wbxls.Worksheets["Prevs"];
                wsPrevs.Select();
                wsPrevs.Range["A2"].Value = dataPrevs;
                wsPrevs.Range["A4"].Value = dataSEmEstimada;

                wsPega = wbxls.Worksheets["Cen1"];
                wsPega.Select();
                wsPega.Range["_metaCen1"].NumberFormat = "General";
                wsPega.Range["_metaCen1"].Value = metas.Item2;

                Worksheet wsEnt = wbxls.Worksheets["Entrada"];
                wsEnt.Select();
                wsEnt.Range["_entrada"].Value = result;
                xlsApp.Calculate();
                wbxls.SaveAs(nomeTemp, wbxls.FileFormat);
                //wbxls.Close(SaveChanges: false);
                var wb = new WorkbookPrevsCenariosMen(wbxls);
                wb.ArquivosDeEntrada = arqEntradaPath;

                RunCenarioTestePrevM2(wb, useAcomph, encad, previvazFolder, true);// encad);
                ExportaPrevsM2(wb, raiz, cenario);


                wbxls.Save();
                wbxls.Close(SaveChanges: false);
                //System.IO.File.Delete(caminhoWbCenario);
                //System.IO.File.Move(nomeTemp, caminhoWbCenario);

            }
            finally
            {
                if (xlsApp != null)
                {
                    xlsApp.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
                    xlsApp.ScreenUpdating = true;
                    xlsApp.Quit();
                    ExcelTools.Helper.Release(xlsApp);

                }
            }
        }
        public static void RunCenarioPrevsM2(string caminhoWbCenario, bool useAcomph, Tuple<string, object[,], int> metas, string previvazFolder, string raiz, string cenario, bool encad = false)// bool encad)
        {
            Microsoft.Office.Interop.Excel.Application xlsApp = null;
            xlsApp = new Microsoft.Office.Interop.Excel.Application();

            try
            {
                //var xlsApp = new Microsoft.Office.Interop.Excel.Application();
                while (!xlsApp.Ready)
                {
                    System.Threading.Thread.Sleep(200);
                }

                xlsApp.Visible = true;
                xlsApp.ScreenUpdating = true;
                xlsApp.DisplayAlerts = false;
                //xlApp = ExcelTools.Helper.StartExcel();
                //copia excel para nome unico /temporario

                var sufixo = $"_{metas.Item1}";
                var nomeTemp = Path.Combine(
                    Path.GetDirectoryName(caminhoWbCenario),
                    // Path.GetFileNameWithoutExtension(caminhoWbCenario) + $"_{sufixo}.xlsm"
                    Path.GetFileName(caminhoWbCenario).Replace("_dummy", sufixo)
                    );
                if (System.IO.File.Exists(nomeTemp))
                {
                    System.IO.File.Delete(nomeTemp);
                }
                //System.IO.File.Copy(caminhoWbCenario, nomeTemp, true);

                var wbxls = xlsApp.Workbooks.Open(caminhoWbCenario);
                wbxls.Activate();
                //xlsApp.Workbooks.
                Worksheet wsEnt1 = wbxls.Worksheets["Entrada"];
                wsEnt1.Select();
                string arqEntradaPath = wsEnt1.Range["_entradaPrevivaz"].Text;

                Worksheet wsPrevs = wbxls.Worksheets["Prevs"];
                wsPrevs.Select();
                DateTime dataPrevs = wsPrevs.Range["A2"].Value;
                DateTime dataSEmEstimada = wsPrevs.Range["A4"].Value;

                //var wb = new WorkbookPrevsCenariosMen(wbxls);
                //Workbook wbc = xlsApp.ActiveWorkbook;
                Worksheet wsPega = wbxls.Worksheets["Cen1"];
                wsPega.Select();
                wsPega.Range["_metaCen1"].NumberFormat = "General";
                wsPega.Range["_metaCen1"].Value = metas.Item2;



                //wb.METAS = metas.Item2;
                //!AjustarCenarios
                //xlApp.Run($"'CHUVAVAZAO_{code}.xlsm'!CriarCenario");
                xlsApp.Run($"'{Path.GetFileName(caminhoWbCenario)}'!AjustarCenarios");

                //var result = wb.PrevsCen1;
                object[,] result = wsPega.Range["_cen1"].Value;
                int linhas = wsPega.Range["_cen1"].Rows.Count;
                int colunas = wsPega.Range["_cen1"].Columns.Count;

                for (int x = 1; x <= linhas; x++)
                {
                    for (int y = metas.Item3 + 3; y <= colunas; y++)//14
                    {
                        if (result[x, y] != null)
                        {
                            result[x, y] = null;
                        }
                    }
                }
                wbxls.Close(SaveChanges: false);

                string PlanModelo = $@"H:\TI - Sistemas\UAT\PricingExcelTools\files\Gera_e_Avalia_Cenarios_Men_Sem_Extendido.xltm";
                //wbxls = xlsApp.Workbooks.Open(nomeTemp);
                wbxls = xlsApp.Workbooks.Open(PlanModelo);
                wbxls.Activate();

                wsPrevs = wbxls.Worksheets["Prevs"];
                wsPrevs.Select();
                wsPrevs.Range["A2"].Value = dataPrevs;
                wsPrevs.Range["A4"].Value = dataSEmEstimada;

                wsPega = wbxls.Worksheets["Cen1"];
                wsPega.Select();
                wsPega.Range["_metaCen1"].NumberFormat = "General";
                wsPega.Range["_metaCen1"].Value = metas.Item2;

                Worksheet wsEnt = wbxls.Worksheets["Entrada"];
                wsEnt.Select();
                wsEnt.Range["_entrada"].Value = result;
                xlsApp.Calculate();
                wbxls.SaveAs(nomeTemp, wbxls.FileFormat);
                //wbxls.Close(SaveChanges: false);
                var wb = new WorkbookPrevsCenariosMen(wbxls);
                wb.ArquivosDeEntrada = arqEntradaPath;

                RunCenarioTestePrevM2(wb, useAcomph, encad, previvazFolder, true);// encad);
                ExportaPrevsM2(wb, raiz, cenario);


                wbxls.Save();
                wbxls.Close(SaveChanges: false);
                //System.IO.File.Delete(caminhoWbCenario);
                //System.IO.File.Move(nomeTemp, caminhoWbCenario);

            }
            finally
            {
                if (xlsApp != null)
                {
                    xlsApp.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
                    xlsApp.ScreenUpdating = true;
                    xlsApp.Quit();
                    ExcelTools.Helper.Release(xlsApp);

                }
            }
        }

        //------------------
        public void RunPrevsLocal(string baseFolder/*, bool saveFileToFolder = false*/)
        {

            try
            {
                List<string> folders = new List<string>();

                string dir;
                if (Directory.Exists(baseFolder))
                {
                    var dirs = Directory.GetDirectories(baseFolder).ToList();
                    if (dirs.Count() > 0)
                    {
                        foreach (var d in dirs)
                        {
                            folders.Add(d);
                        }
                    }
                    else
                    {
                        folders.Add(baseFolder);
                    }
                }
                else if (System.IO.File.Exists(baseFolder))
                {
                    dir = Path.GetDirectoryName(baseFolder);
                    folders.Add(dir);
                }
                else
                    return;
                foreach (var childFolder in folders)
                {
                    workFolder = childFolder;
                    CopyPrevivazFiles(workFolder);
                }

                Parallel.ForEach(folders, x =>
                 {
                     RunPrevivaz(x);
                 });

                List<string> removeArqs = new List<string>
                {
                    ".BCX",
                    ".exc",
                    ".rel",
                    "_MOD.DAT",
                    "FAIXAS.TXT",
                    "LIMITES.TXT",
                    "PREVISAO.DAT",
                    "PREVP.DAT",
                    "PREVT.DAT",
                    "RUNSTATE.DAT",
                    "RUNTRACE.DAT",
                };

                foreach (var childFolder in folders)
                {

                    var files = Directory.GetFiles(childFolder).ToList();
                    foreach (var f in files)
                    {
                        var filename = Path.GetFileName(f);
                        if (removeArqs.Any(x => f.Contains(x)))
                        {
                            System.IO.File.Delete(f);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        public static void RunCenarioLocal(WorkbookPrevsCenariosMen wb, bool useAcomph, bool encad, string sufixoDePasta = "")//bool encad)
        {
            var previvazBaseFolder = wb.ArquivosDeEntrada;


            ///log parcial 1 -- original
            wb.Saida1 = wb.PrevsCen1;//[a,1] = id posto, [a,3...] = vazões 

            List<Acomph> acompH = null;
            var semanaprevisao = 6; //dias necessários para considerar a média como semanal


            if (useAcomph)
            {
                acompH = Tools.GetAcomphData(DateTime.Today.AddDays(-21), DateTime.Today);
            }

            var prevsCen1 = wb.PrevsCen1;
            var anoPrev = wb.AnoAtual;
            var usr = System.Environment.UserName.Replace('.', '_');
            // var tempFolder = @"Z:\previsaopld\shared\CHUVA-VAZAO\previvaz_" + usr + sufixoDePasta;
            var localFolder = Path.GetTempPath();
            var tempFolder = Path.Combine(localFolder, "CHUVA-VAZAO\\previvaz_" + usr + sufixoDePasta);

            var teste = acompH.GroupBy(ac => new { ac.semana, ac.posto })
                   .Where(ac => ac.Count() >= semanaprevisao).ToList();

            if (acompH != null)
            {
                acompH.GroupBy(ac => new { ac.semana, ac.posto })
                    .Where(ac => ac.Count() >= semanaprevisao).ToList()
                    .ForEach(ac =>
                    {
                        for (int i = 1; i <= wb.SemanasPrevs.Length; i++)
                        {
                            if ((double)wb.SemanasPrevs[1, i] == (double)ac.Key.semana)
                            {
                                prevsCen1[ac.Key.posto, i + 2] = ac.Average(x => x.qNat);

                                if (postosIncrementais.ContainsKey(ac.Key.posto))
                                {
                                    if (ac.Key.posto == 253)//tocantins sao salvador + canabrava
                                    {
                                        var canaBrava = acompH.Where(x => x.posto == 191 && x.semana == ac.Key.semana).ToList();
                                        var dado = canaBrava.Average(x => x.qInc);
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc) + dado;

                                    }
                                    else if (ac.Key.posto == 273)//tocantins lajeado + peixe Angical
                                    {
                                        var peixeAngi = acompH.Where(x => x.posto == 257 && x.semana == ac.Key.semana).ToList();
                                        var dado = peixeAngi.Average(x => x.qInc);
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc) + dado;

                                    }
                                    else
                                    {
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc);
                                    }
                                }

                                if (ac.Key.posto == 169) prevsCen1[168, i + 2] = ac.Average(x => x.qInc);

                                // if (ac.Key.posto == 239) prevsCen1[239, i + 2] = ac.Average(x => x.qNat) - (double)prevsCen1[237, i + 2];
                                // if (ac.Key.posto == 242) prevsCen1[242, i + 2] = ac.Average(x => x.qNat) - (double)prevsCen1[239, i + 2];

                            }
                        }
                    });
            }



            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
            Directory.CreateDirectory(tempFolder);

            var postosPrevivaz = Directory.GetFiles(previvazBaseFolder).GroupBy(x =>
                System.Text.RegularExpressions.Regex.Match(
                Path.GetFileNameWithoutExtension(x),
                @"^\d+").Value
               );

            Dictionary<int, List<object>> results = new Dictionary<int, List<object>>();
            var prevDecks = new List<Compass.CommomLibrary.Previvaz.Deck>();
            //Globals.ThisAddIn.Application.StatusBar = "Executando : ";
            foreach (var p in postosPrevivaz)
            {//atualiza os dados do inp com o numero das semanas e o  arquivo str com as vazões semanais da planilha
                var prevDeck = new Compass.CommomLibrary.Previvaz.Deck(p.Key);
                prevDeck.GetFiles(previvazBaseFolder);


                if (prevDeck[CommomLibrary.Previvaz.DeckDocument.str] == null || prevDeck[CommomLibrary.Previvaz.DeckDocument.lim] == null)
                {
                    continue;
                }


                prevDecks.Add(prevDeck);

                var path = Path.Combine(tempFolder, p.Key);

                int posto = int.Parse(prevDeck.Posto);

                var inp = (Compass.CommomLibrary.Previvaz.Inp)prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document;
                var str = (Compass.CommomLibrary.Previvaz.Str)prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document;

                inp.SemanaPrevisao = Convert.ToInt32(wb.SemanasPrevs[1, 1]);
                inp.AnoPrevisao = anoPrev;


                for (int s = 1; s < 12; s++)
                {

                    if (postosIncrementais.ContainsKey(posto) && (prevsCen1[postosIncrementais[posto].Item2, s + 2] is double && (double)prevsCen1[postosIncrementais[posto].Item2, s + 2] != 0))
                    {
                        str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = (double)prevsCen1[postosIncrementais[posto].Item2, s + 2];//se o posto incremental desse posto conter dados da semana, ele atribui esse dado para o posto(esses processo só acontece da semana rv0 até a atual)
                        var dd = str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])];
                    }
                    else if (postosIncrementais.ContainsKey(posto) && (prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0))  //só entra nesse if para as semanas previstas,para esse posto que contem posto incremental,a sua vazão da semana prevista sera
                    {                                                                                                                               //igual à sua vazão menos a vazão de seus postos montantes ex: vazão do posto 34 = v34 - (v33-v18-v99-v241-v261)  //OBS: o valor minimo será sempre 12
                        if (posto == 253 || posto == 273 || posto == 271 || posto == 275)//postos tocantins que rodam com a incremental alocada nos postos auxiliares 308~311
                        {
                            str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = (double)prevsCen1[posto, s + 2];
                        }
                        else
                        {
                            var v = (double)prevsCen1[posto, s + 2]
                            - postosIncrementais[posto].Item1.Sum(pm => (double)prevsCen1[pm, s + 2]);
                            str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = Math.Max(v, 12);
                        }


                    }
                    else if (posto == 239 && prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        var v = (double)prevsCen1[239, s + 2] - (double)prevsCen1[237, s + 2];// para o posto 239(ibitinga) sera usado a vazão de ibitinga menoa a vazão de barra bonita(237)
                        if (v < 1) v = 5;                                                     //OBS: o valor minimo sempre sera 5
                        str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = v;
                    }
                    else if (posto == 242 && prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        var v = (double)prevsCen1[242, s + 2] - (double)prevsCen1[239, s + 2];// para o posto 242(N.Avanhadava) sera usado a vazão de N.Avanhadava menoa a vazão de ibitinga(239)
                        if (v < 1) v = 5;                                                     //OBS: o valor minimo sempre sera 5
                        str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = v;
                    }
                    else if (prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = (double)prevsCen1[posto, s + 2];
                    }
                    else
                    {
                        break;
                    }

                    var proxSem = Convert.ToInt32(wb.SemanasPrevs[1, s + 1]);

                    if (proxSem < inp.SemanaPrevisao)    //confere se a proxima semana ainda faz parte do mesmo ano se nao incrementa o ano
                    {
                        inp.AnoPrevisao = inp.AnoPrevisao + 1;
                    }
                    if (proxSem == 2)
                    {
                        str.AnoFinal = inp.AnoPrevisao;
                    }

                    inp.SemanaPrevisao = proxSem;
                }

                prevDeck.CopyFilesToFolder(path);
            }
            Previvaz previvaz = new Previvaz();
            previvaz.RunPrevsLocal(tempFolder);
            //                Services.Previvaz.RunOnLinux(tempFolder);

            if (encad)
            {

                //rodadas encadeadas previvaz inicio
                foreach (var prevDeck in prevDecks)
                {
                    var rs = prevDeck.GetFut();
                    if (rs.Count > 0)
                    {
                        //object[] r = (object[])rs.First().Value;
                        results.Add((int)rs[0], rs);


                        var inp = prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document as CommomLibrary.Previvaz.Inp;
                        var str = prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document as CommomLibrary.Previvaz.Str;

                        inp.SemanaPrevisao += 6;

                        // var numSemanas = 52 + inp.NumSemanasHist;
                        //tratar virada de ano
                        //var numSemanas = inp.NumSemanasHist - inp.SemanaPrevisao;

                        //if (numSemanas == -1)
                        //{
                        //    inp.AnoPrevisao = inp.AnoPrevisao + 1;
                        //}
                        if (inp.SemanaPrevisao > inp.NumSemanasHist)
                        {
                            inp.AnoPrevisao = inp.AnoPrevisao + 1;
                            str.AnoFinal = inp.AnoPrevisao;
                            inp.SemanaPrevisao = inp.SemanaPrevisao - inp.NumSemanasHist;
                        }
                        str.AnoFinal = inp.AnoPrevisao;



                        // var str = prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document as CommomLibrary.Previvaz.Str;
                        try
                        {
                            var anoInicial = Convert.ToInt32(rs[1]);
                            var semanaInicial = (int)rs[3];

                            for (int i = 0; i < 6; i++)
                            {
                                str[anoInicial, semanaInicial] = (double)rs[4 + i];

                                semanaInicial++;

                                if (semanaInicial > inp.NumSemanasHist)
                                {
                                    semanaInicial = 1;
                                    anoInicial++;
                                }


                            }


                            str.SaveToFile();
                            inp.SaveToFile();
                            ///renomar o *_fut;
                            var camFut = Path.Combine(prevDeck.Folder, prevDeck.Posto + "_fut.DAT");
                            if (System.IO.File.Exists(camFut))
                            {
                                System.IO.File.Move(camFut, Path.Combine(prevDeck.Folder, prevDeck.Posto + "_futbkp.DAT"));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }

                }


                previvaz.RunPrevsLocal(tempFolder);

                //rodadas encadeadas previvaz fimmmm
            }


            foreach (var prevDeck in prevDecks)
            {
                var rs = prevDeck.GetFut();
                if (rs.Count > 0)
                {
                    //   object[] r = (object[])rs.First().Value;

                    if (results.ContainsKey((int)rs[0]))
                    {
                        var rAnterior = results[(int)rs[0]];

                        rAnterior.Add(rs[4]);
                        rAnterior.Add(rs[5]);
                        rAnterior.Add(rs[6]);
                        rAnterior.Add(rs[7]);
                        rAnterior.Add(rs[8]);
                        rAnterior.Add(rs[9]);

                    }
                    else
                        results.Add((int)rs[0], rs);
                }
            }


            // coloca resultado do previvaz na entrada para calcular postos artificias;
            foreach (var r in results)
            {
                var c = 1;
                for (; c <= 12; c++)
                {
                    if ((int)(double)wb.SemanasPrevs[1, c] == (int)r.Value[3])
                    {
                        c += 2;
                        break;
                    }
                }

                // var c = (int)r[3] - (int)(double)wb.SemanasPrevs[1, 1] + 3;
                var posto = r.Key;
                for (int i = 0; i < 11 && (c + i < 15); i++)
                {
                    var vaz = 4 + i < r.Value.Count ? r.Value[4 + i] : r.Value.Last(); // completa todo o horizonte de 12 dias repetindo o ultimo resultado do previvaz para além da 6a semana de previsão.

                    if (postosIncrementais.ContainsKey(posto))
                        prevsCen1[postosIncrementais[posto].Item2, c + i] = vaz;

                    else if (posto == 239) prevsCen1[239, c + i] = (double)vaz + (double)prevsCen1[237, c + i];
                    else if (posto == 242) prevsCen1[242, c + i] = (double)vaz + (double)prevsCen1[239, c + i];
                    else prevsCen1[posto, c + i] = vaz;
                }
            }

            #region trata posto 169

            var str156 = (Compass.CommomLibrary.Previvaz.Str)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.str].Document;
            var str158 = (Compass.CommomLibrary.Previvaz.Str)prevDecks.First(x => x.Posto == "158")[CommomLibrary.Previvaz.DeckDocument.str].Document;
            var sem_2 = Convert.ToInt32(wb.SemanasPrevs[1, 1]) - 2;

            var sem_Atual = Convert.ToInt32(wb.SemanaAtualIndice) + 2; // Adiciona 2 para ajustar a celula da planilha 
            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            {
                sem_Atual = sem_Atual - 1;
            }
            /*
                        if (sem_2 < 1)
                        {
                            sem_2 = sem_2 +
                                ((Compass.CommomLibrary.Previvaz.Inp)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.inp].Document).NumSemanasHist;
                        }

                        prevsCen1[169, 3] = (double)prevsCen1[168, 3]
                            + str156[anoPrev, sem_2]
                            + str158[anoPrev, sem_2];

                        sem_2 = Convert.ToInt32(wb.SemanasPrevs[1, 1]) - 1;
                        if (sem_2 < 1)
                        {
                            sem_2 = sem_2 +
                                ((Compass.CommomLibrary.Previvaz.Inp)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.inp].Document).NumSemanasHist;
                        }

                        prevsCen1[169, 4] = (double)prevsCen1[168, 4]
                            + str156[anoPrev, sem_2]
                            + str158[anoPrev, sem_2];
                            */
            for (int i = sem_Atual + 3; i <= 14 && prevsCen1[168, i] is double; i++) // Adiciona 3 para começar depois da ultima semana de CPINS
                prevsCen1[169, i] = (double)prevsCen1[156, i - 2] + (double)prevsCen1[158, i - 2] + (double)prevsCen1[168, i];


            #endregion trata posto 169

            wb.Entrada = prevsCen1;

            wb.Regressoes = true;

            for (int posto = 1; posto <= 320; posto++)
                for (int s = 0; s < 12; s++)
                    if (!(prevsCen1[posto, 3 + s] is double) || (double)prevsCen1[posto, 3 + s] < 1) prevsCen1[posto, 3 + s] = wb.PrevsCen1[posto, 3 + s];

            foreach (var pn in postosIncrementais)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (
                        (prevsCen1[pn.Key, 3 + i] is double && (double)prevsCen1[pn.Key, 3 + i] < 1)
                        || !(prevsCen1[pn.Key, 3 + i] is double) || (prevsCen1[257, 3 + i] is double && (double)prevsCen1[257, 3 + i] < 1)
                        )
                    {
                        if (pn.Key == 253)// sao salvador + canabrava
                        {
                            double fatcana = 0.504;
                            prevsCen1[191, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatcana) + ((double)prevsCen1[270, 3 + i]);
                            double fatSal = 0.496;
                            prevsCen1[pn.Key, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatSal) + ((double)prevsCen1[191, 3 + i]);

                        }
                        else if (pn.Key == 273)//lajeado + peixe angical
                        {
                            double fatpeixe = 0.488;
                            prevsCen1[257, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatpeixe) + ((double)prevsCen1[253, 3 + i]);
                            double fatlaj = 0.512;
                            prevsCen1[pn.Key, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatlaj) + ((double)prevsCen1[257, 3 + i]);

                        }


                        else
                        {
                            prevsCen1[pn.Key, 3 + i] = (double)prevsCen1[pn.Value.Item2, 3 + i]
                            + pn.Value.Item1.Sum(pMn => (double)prevsCen1[pMn, 3 + i]);
                        }

                    }
                }
            }

            wb.Entrada = prevsCen1;

            // itera em todos os postos para se não houver resultado, na entrada, utiliza o resultado da regressão.
            for (int posto = 1; posto <= 320; posto++)
                for (int s = 0; s < 12; s++)
                    if (!(prevsCen1[posto, 3 + s] is double) || (double)prevsCen1[posto, 3 + s] < 1) prevsCen1[posto, 3 + s] = wb.PrevsCen1[posto, 3 + s];

            wb.Entrada = prevsCen1;
            wb.Regressoes = false;

            try
            {
                if (wb.GravarPrevivaz)
                {

                    var destPath = Path.Combine(wb.Path, "arq_previvaz");

                    if (Directory.Exists(destPath))
                    {
                        Directory.Delete(destPath, true);
                    }


                    Tools.moveDirectory(tempFolder, destPath);

                    if (Directory.Exists(tempFolder))
                    {
                        Directory.Delete(tempFolder, true);
                    }
                }
            }
            catch { }

        }

        //---------------------

        public static void RunCenario(WorkbookPrevsCenariosMen wb, bool useAcomph, bool encad, string sufixoDePasta = "", bool prevsM2 = false)//bool encad)
        {
            var previvazBaseFolder = wb.ArquivosDeEntrada;
            object[,] vazExcedentes = new object[321, 12];
            int XIcen1 = wb.GetCoordinatesByRange("_cen1", "XI");
            int XFcen1 = wb.GetCoordinatesByRange("_cen1", "XF");
            int YIcen1 = wb.GetCoordinatesByRange("_cen1", "YI");
            int YFcen1 = wb.GetCoordinatesByRange("_cen1", "YF");



            ///log parcial 1 -- original
            wb.Saida1 = wb.PrevsCen1;//[a,1] = id posto, [a,3...] = vazões 

            List<Acomph> acompH = null;
            var semanaprevisao = 6; //dias necessários para considerar a média como semanal


            if (useAcomph)
            {
                acompH = Tools.GetAcomphData(DateTime.Today.AddDays(-21), DateTime.Today);
            }

            var prevsCen1 = wb.PrevsCen1;
            var anoPrev = wb.AnoAtual;
            var usr = System.Environment.UserName.Replace('.', '_');
            // var tempFolder = @"Z:\previsaopld\shared\CHUVA-VAZAO\previvaz_" + usr + sufixoDePasta;
            //var tempFolder = @"L:\shared\CHUVA-VAZAO\previvaz_" + usr + sufixoDePasta;
            var tempFolder = Path.Combine(wb.Path, "arq_previvaz");

            if (prevsM2)
            {
                tempFolder = sufixoDePasta;
            }

            var teste = acompH.GroupBy(ac => new { ac.semana, ac.posto })
                   .Where(ac => ac.Count() >= semanaprevisao).ToList();

            if (acompH != null)
            {
                acompH.GroupBy(ac => new { ac.semana, ac.posto })
                    .Where(ac => ac.Count() >= semanaprevisao).ToList()
                    .ForEach(ac =>
                    {
                        for (int i = 1; i <= wb.SemanasPrevs.Length; i++)
                        {
                            if ((double)wb.SemanasPrevs[1, i] == (double)ac.Key.semana)
                            {
                                prevsCen1[ac.Key.posto, i + 2] = ac.Average(x => x.qNat);

                                if (postosIncrementais.ContainsKey(ac.Key.posto))
                                {
                                    if (ac.Key.posto == 253)//tocantins sao salvador + canabrava
                                    {
                                        var canaBrava = acompH.Where(x => x.posto == 191 && x.semana == ac.Key.semana).ToList();
                                        var dado = canaBrava.Average(x => x.qInc);
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc) + dado;

                                    }
                                    else if (ac.Key.posto == 273)//tocantins lajeado + peixe Angical
                                    {
                                        var peixeAngi = acompH.Where(x => x.posto == 257 && x.semana == ac.Key.semana).ToList();
                                        var dado = peixeAngi.Average(x => x.qInc);
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc) + dado;

                                    }
                                    else
                                    {
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc);
                                    }
                                }

                                if (ac.Key.posto == 169) prevsCen1[168, i + 2] = ac.Average(x => x.qInc);

                                // if (ac.Key.posto == 239) prevsCen1[239, i + 2] = ac.Average(x => x.qNat) - (double)prevsCen1[237, i + 2];
                                // if (ac.Key.posto == 242) prevsCen1[242, i + 2] = ac.Average(x => x.qNat) - (double)prevsCen1[239, i + 2];

                            }
                        }
                    });
            }



            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
            Directory.CreateDirectory(tempFolder);

            var postosPrevivaz = Directory.GetFiles(previvazBaseFolder).GroupBy(x =>
                System.Text.RegularExpressions.Regex.Match(
                Path.GetFileNameWithoutExtension(x),
                @"^\d+").Value
               );

            Dictionary<int, List<object>> results = new Dictionary<int, List<object>>();
            var prevDecks = new List<Compass.CommomLibrary.Previvaz.Deck>();
            //Globals.ThisAddIn.Application.StatusBar = "Executando : ";
            foreach (var p in postosPrevivaz)
            {//atualiza os dados do inp com o numero das semanas e o  arquivo str com as vazões semanais da planilha
                var prevDeck = new Compass.CommomLibrary.Previvaz.Deck(p.Key);
                prevDeck.GetFiles(previvazBaseFolder);


                if (prevDeck[CommomLibrary.Previvaz.DeckDocument.str] == null || prevDeck[CommomLibrary.Previvaz.DeckDocument.lim] == null)
                {
                    continue;
                }


                prevDecks.Add(prevDeck);

                var path = Path.Combine(tempFolder, p.Key);

                int posto = int.Parse(prevDeck.Posto);

                var inp = (Compass.CommomLibrary.Previvaz.Inp)prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document;
                var str = (Compass.CommomLibrary.Previvaz.Str)prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document;

                inp.SemanaPrevisao = Convert.ToInt32(wb.SemanasPrevs[1, 1]);
                inp.AnoPrevisao = anoPrev;

                var numerosSem = wb.GetSemanaPrevsStr(inp.NumSemanasHist);
                for (int s = 1; s < 12; s++)
                {

                    if (postosIncrementais.ContainsKey(posto) && (prevsCen1[postosIncrementais[posto].Item2, s + 2] is double && (double)prevsCen1[postosIncrementais[posto].Item2, s + 2] != 0))
                    {
                        str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = (double)prevsCen1[postosIncrementais[posto].Item2, s + 2];//se o posto incremental desse posto conter dados da semana, ele atribui esse dado para o posto(esses processo só acontece da semana rv0 até a atual)
                        var dd = str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])];
                    }
                    else if (postosIncrementais.ContainsKey(posto) && (prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0))  //só entra nesse if para as semanas previstas,para esse posto que contem posto incremental,a sua vazão da semana prevista sera
                    {                                                                                                                               //igual à sua vazão menos a vazão de seus postos montantes ex: vazão do posto 34 = v34 - (v33-v18-v99-v241-v261)  //OBS: o valor minimo será sempre 12
                        if (posto == 253 || posto == 273 || posto == 271 || posto == 275)//postos tocantins que rodam com a incremental alocada nos postos auxiliares 308~311
                        {
                            str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = (double)prevsCen1[posto, s + 2];
                        }
                        else
                        {
                            var v = (double)prevsCen1[posto, s + 2]
                            - postosIncrementais[posto].Item1.Sum(pm => (double)prevsCen1[pm, s + 2]);
                            str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = Math.Max(v, 12);
                        }


                    }
                    else if (posto == 239 && prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        var v = (double)prevsCen1[239, s + 2] - (double)prevsCen1[237, s + 2];// para o posto 239(ibitinga) sera usado a vazão de ibitinga menoa a vazão de barra bonita(237)
                        if (v < 1) v = 5;                                                     //OBS: o valor minimo sempre sera 5
                        str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = v;
                    }
                    else if (posto == 242 && prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        var v = (double)prevsCen1[242, s + 2] - (double)prevsCen1[239, s + 2];// para o posto 242(N.Avanhadava) sera usado a vazão de N.Avanhadava menoa a vazão de ibitinga(239)
                        if (v < 1) v = 5;                                                     //OBS: o valor minimo sempre sera 5
                        str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = v;
                    }
                    else if (prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = (double)prevsCen1[posto, s + 2];
                    }
                    else
                    {
                        break;
                    }

                    var proxSem = Convert.ToInt32(numerosSem[1, s + 1]);

                    if (proxSem < inp.SemanaPrevisao)    //confere se a proxima semana ainda faz parte do mesmo ano se nao incrementa o ano
                    {
                        inp.AnoPrevisao = inp.AnoPrevisao + 1;
                    }
                    if (proxSem == 2)
                    {
                        str.AnoFinal = inp.AnoPrevisao;
                    }

                    inp.SemanaPrevisao = proxSem;
                }

                prevDeck.CopyFilesToFolder(path);
            }
            Previvaz previvaz = new Previvaz();
            Parallel.ForEach(postosPrevivaz, x =>
            {
                var path = Path.Combine(tempFolder, x.Key);
                previvaz.RunPrevivazLocalVM(path);

            });
            //Services.Linux.Run(tempFolder, @"/home/producao/PrevisaoPLD/shared/previvaz/previvaz3.sh", "previvaz", true, true, "hide");
            //                Services.Previvaz.RunOnLinux(tempFolder);

            if (encad)
            {

                //rodadas encadeadas previvaz inicio
                foreach (var prevDeck in prevDecks)
                {
                    var rs = prevDeck.GetFut();
                    if (rs.Count > 0)
                    {
                        //object[] r = (object[])rs.First().Value;
                        results.Add((int)rs[0], rs);


                        var inp = prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document as CommomLibrary.Previvaz.Inp;
                        var str = prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document as CommomLibrary.Previvaz.Str;

                        inp.SemanaPrevisao += 6;

                        // var numSemanas = 52 + inp.NumSemanasHist;
                        //tratar virada de ano
                        //var numSemanas = inp.NumSemanasHist - inp.SemanaPrevisao;

                        //if (numSemanas == -1)
                        //{
                        //    inp.AnoPrevisao = inp.AnoPrevisao + 1;
                        //}
                        if (inp.SemanaPrevisao > inp.NumSemanasHist)
                        {
                            inp.AnoPrevisao = inp.AnoPrevisao + 1;
                            str.AnoFinal = inp.AnoPrevisao;
                            inp.SemanaPrevisao = inp.SemanaPrevisao - inp.NumSemanasHist;
                        }
                        str.AnoFinal = inp.AnoPrevisao;



                        // var str = prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document as CommomLibrary.Previvaz.Str;
                        try
                        {
                            var anoInicial = Convert.ToInt32(rs[1]);
                            var semanaInicial = (int)rs[3];

                            for (int i = 0; i < 6; i++)
                            {
                                str[anoInicial, semanaInicial] = (double)rs[4 + i];

                                semanaInicial++;

                                if (semanaInicial > inp.NumSemanasHist)
                                {
                                    semanaInicial = 1;
                                    anoInicial++;
                                }


                            }


                            str.SaveToFile();
                            inp.SaveToFile();
                            ///renomar o *_fut;
                            var camFut = Path.Combine(prevDeck.Folder, prevDeck.Posto + "_fut.DAT");
                            if (System.IO.File.Exists(camFut))
                            {
                                System.IO.File.Move(camFut, Path.Combine(prevDeck.Folder, prevDeck.Posto + "_futbkp.DAT"));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }

                }

                Parallel.ForEach(postosPrevivaz, x =>
                {
                    var path_exe = Path.Combine(tempFolder, x.Key);
                    previvaz.RunPrevivazLocalVM(path_exe);

                });
                //Services.Linux.Run(tempFolder, @"/home/producao/PrevisaoPLD/shared/previvaz/previvaz3.sh", "previvaz", true, true, "hide");
                //                Services.Previvaz.RunOnLinux(tempFolder);

                //rodadas encadeadas previvaz fimmmm
            }


            foreach (var prevDeck in prevDecks)
            {
                var rs = prevDeck.GetFut();
                if (rs.Count > 0)
                {
                    //   object[] r = (object[])rs.First().Value;

                    if (results.ContainsKey((int)rs[0]))
                    {
                        var rAnterior = results[(int)rs[0]];

                        rAnterior.Add(rs[4]);
                        rAnterior.Add(rs[5]);
                        rAnterior.Add(rs[6]);
                        rAnterior.Add(rs[7]);
                        rAnterior.Add(rs[8]);
                        rAnterior.Add(rs[9]);

                    }
                    else
                        results.Add((int)rs[0], rs);
                }
            }


            // coloca resultado do previvaz na entrada para calcular postos artificias;
            foreach (var r in results)
            {
                var numposto = r.Key;
                var deck = prevDecks.Where(x => x.Posto == numposto.ToString()).First();
                var inp = deck[CommomLibrary.Previvaz.DeckDocument.inp].Document as CommomLibrary.Previvaz.Inp;
                var numerosSem = wb.GetSemanaPrevsStr(inp.NumSemanasHist);

                var c = 1;
                for (; c <= 12; c++)
                {
                    if ((int)(double)numerosSem[1, c] == (int)r.Value[3])
                    {
                        c += 2;
                        break;
                    }
                }

                // var c = (int)r[3] - (int)(double)wb.SemanasPrevs[1, 1] + 3;
                var posto = r.Key;
                for (int i = 0; i < 11 && (c + i < 15); i++)
                {
                    var vaz = 4 + i < r.Value.Count ? r.Value[4 + i] : r.Value.Last(); // completa todo o horizonte de 12 dias repetindo o ultimo resultado do previvaz para além da 6a semana de previsão.

                    if (postosIncrementais.ContainsKey(posto))
                        prevsCen1[postosIncrementais[posto].Item2, c + i] = vaz;

                    else if (posto == 239) prevsCen1[239, c + i] = (double)vaz + (double)prevsCen1[237, c + i];
                    else if (posto == 242) prevsCen1[242, c + i] = (double)vaz + (double)prevsCen1[239, c + i];
                    else prevsCen1[posto, c + i] = vaz;
                }
                if (prevsM2)
                {
                    int startIndex = 0;
                    for (int x = 0; 4 + x < r.Value.Count; x++)
                    {
                        var vaz = r.Value[4 + x];

                        if (c + x > 14)
                        {
                            if (postosIncrementais.ContainsKey(posto))
                                vazExcedentes[postosIncrementais[posto].Item2, startIndex] = vaz;

                            else if (posto == 239) vazExcedentes[239, startIndex] = (double)vaz + (double)vazExcedentes[237, startIndex];
                            else if (posto == 242) vazExcedentes[242, startIndex] = (double)vaz + (double)vazExcedentes[239, startIndex];
                            else vazExcedentes[posto, startIndex] = vaz;

                            startIndex++;
                        }

                    }
                }
            }

            #region trata posto 169

            var str156 = (Compass.CommomLibrary.Previvaz.Str)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.str].Document;
            var str158 = (Compass.CommomLibrary.Previvaz.Str)prevDecks.First(x => x.Posto == "158")[CommomLibrary.Previvaz.DeckDocument.str].Document;
            var sem_2 = Convert.ToInt32(wb.SemanasPrevs[1, 1]) - 2;

            var sem_Atual = Convert.ToInt32(wb.SemanaAtualIndice) + 2; // Adiciona 2 para ajustar a celula da planilha 
            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            {
                sem_Atual = sem_Atual - 1;
            }
            /*
                        if (sem_2 < 1)
                        {
                            sem_2 = sem_2 +
                                ((Compass.CommomLibrary.Previvaz.Inp)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.inp].Document).NumSemanasHist;
                        }

                        prevsCen1[169, 3] = (double)prevsCen1[168, 3]
                            + str156[anoPrev, sem_2]
                            + str158[anoPrev, sem_2];

                        sem_2 = Convert.ToInt32(wb.SemanasPrevs[1, 1]) - 1;
                        if (sem_2 < 1)
                        {
                            sem_2 = sem_2 +
                                ((Compass.CommomLibrary.Previvaz.Inp)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.inp].Document).NumSemanasHist;
                        }

                        prevsCen1[169, 4] = (double)prevsCen1[168, 4]
                            + str156[anoPrev, sem_2]
                            + str158[anoPrev, sem_2];
                            */
            for (int i = sem_Atual + 3; i <= 14 && prevsCen1[168, i] is double; i++) // Adiciona 3 para começar depois da ultima semana de CPINS
                prevsCen1[169, i] = (double)prevsCen1[156, i - 2] + (double)prevsCen1[158, i - 2] + (double)prevsCen1[168, i];

            if (prevsM2)
            {
                for (int i = 0; i < 12 && vazExcedentes[168, i] is double; i++)
                {
                    if (i == 0)//usa os dados do prevscen1 da semana11
                    {
                        vazExcedentes[169, i] = (double)prevsCen1[156, 13] + (double)prevsCen1[158, 13] + (double)vazExcedentes[168, i];
                    }
                    else if (i == 1)//usa os dados do prevscen1 da semana12
                    {
                        vazExcedentes[169, i] = (double)prevsCen1[156, 14] + (double)prevsCen1[158, 14] + (double)vazExcedentes[168, i];
                    }
                    else
                    {
                        vazExcedentes[169, i] = (double)vazExcedentes[156, i - 2] + (double)vazExcedentes[158, i - 2] + (double)vazExcedentes[168, i];
                    }
                }
            }

            #endregion trata posto 169

            wb.Entrada = prevsCen1;

            wb.Regressoes = true;

            for (int posto = 1; posto <= 320; posto++)
                for (int s = 0; s < 12; s++)
                    if (!(prevsCen1[posto, 3 + s] is double) || (double)prevsCen1[posto, 3 + s] < 1) prevsCen1[posto, 3 + s] = wb.PrevsCen1[posto, 3 + s];

            foreach (var pn in postosIncrementais)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (
                        (prevsCen1[pn.Key, 3 + i] is double && (double)prevsCen1[pn.Key, 3 + i] < 1)
                        || !(prevsCen1[pn.Key, 3 + i] is double) || (prevsCen1[257, 3 + i] is double && (double)prevsCen1[257, 3 + i] < 1)
                        )
                    {
                        if (pn.Key == 253)// sao salvador + canabrava
                        {
                            double fatcana = 0.504;
                            prevsCen1[191, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatcana) + ((double)prevsCen1[270, 3 + i]);
                            double fatSal = 0.496;
                            prevsCen1[pn.Key, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatSal) + ((double)prevsCen1[191, 3 + i]);

                        }
                        else if (pn.Key == 273)//lajeado + peixe angical
                        {
                            double fatpeixe = 0.488;
                            prevsCen1[257, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatpeixe) + ((double)prevsCen1[253, 3 + i]);
                            double fatlaj = 0.512;
                            prevsCen1[pn.Key, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatlaj) + ((double)prevsCen1[257, 3 + i]);

                        }


                        else
                        {
                            prevsCen1[pn.Key, 3 + i] = (double)prevsCen1[pn.Value.Item2, 3 + i]
                            + pn.Value.Item1.Sum(pMn => (double)prevsCen1[pMn, 3 + i]);
                        }

                    }
                }

            }

            wb.Entrada = prevsCen1;

            // itera em todos os postos para se não houver resultado, na entrada, utiliza o resultado da regressão.
            for (int posto = 1; posto <= 320; posto++)
                for (int s = 0; s < 12; s++)
                    if (!(prevsCen1[posto, 3 + s] is double) || (double)prevsCen1[posto, 3 + s] < 1) prevsCen1[posto, 3 + s] = wb.PrevsCen1[posto, 3 + s];

            wb.Entrada = prevsCen1;
            wb.Regressoes = false;

            try
            {
                wb.FillRange(vazExcedentes, "Cen1", XIcen1 - 1, XIcen1 - 1 + XFcen1, YFcen1 + 7, YFcen1 + 18);//preenchendo as celulas á frente do range "_cen1" obs +6 nas colunas  pra não cobrir celulas comreferencia de formula
            }
            catch (Exception e)
            {

                e.ToString();
            }

            try
            {
                if (wb.GravarPrevivaz)
                {

                    //var destPath = Path.Combine(wb.Path, "arq_previvaz");

                    //if (Directory.Exists(destPath))
                    //{
                    //    Directory.Delete(destPath, true);
                    //}


                    //Tools.moveDirectory(tempFolder, destPath);
                }
            }
            catch { }

        }

        public static void ExportaPrevsM2(WorkbookPrevsCenariosMen wb, string path, string cenario)
        {
            var prevsCen1 = wb.PrevsCen1;
            var datasPrevs = wb.DatasPrevs;
            List<DateTime> datas = new List<DateTime>();
            for (int d = 1; d <= datasPrevs.Length; d++)
            {
                datas.Add((DateTime)datasPrevs[1, d]);
            }

            // var datas = (DateTime) wb.DatasPrevs[1, 1];
            int index = 0;
            DateTime inicio = new DateTime(datas[0].Year, datas[0].Month, 1);

            string prevsname = $"prevs_{cenario}.rv0";
            for (int m = 0; m <= 3; m++)
            {
                if (m > 0)
                {
                    prevsname = $"prevs_{cenario}_M{m}.rv0";
                }
                DateTime sextaRv0 = inicio.AddMonths(m);
                while (sextaRv0.DayOfWeek != DayOfWeek.Friday)
                {
                    sextaRv0 = sextaRv0.AddDays(1);
                }

                if (datas.Contains(sextaRv0))
                {
                    index = datas.IndexOf(sextaRv0);
                    StringBuilder sb = new StringBuilder();

                    for (int i = 1; i <= 320; i++)
                    {
                        List<Double> vazoes = new List<double>();

                        for (int j = index; j <= index + 5; j++)
                        {
                            int col = j + 3 > 22 ? 22 : j + 3;
                            var vaz = Math.Round((double)prevsCen1[i, col]);
                            vazoes.Add(vaz);
                        }
                        sb.AppendFormat("{0,6}{1,5}{2,10}{3,10}{4,10}{5,10}{6,10}{7,10}{8}",
                                i.ToString(),
                                i.ToString(),
                               vazoes[0],
                               vazoes[1],
                               vazoes[2],
                               vazoes[3],
                               vazoes[4],
                               vazoes[5],
                                Environment.NewLine);

                    }
                    System.IO.File.WriteAllText(Path.Combine(path, prevsname), sb.ToString());
                    //_SemanasPrevsDatas
                }
            }


        }

        public static void RunCenarioTestePrevM2(WorkbookPrevsCenariosMen wb, bool useAcomph, bool encad, string sufixoDePasta = "", bool prevsM2 = false)//bool encad)
        {
            var previvazBaseFolder = wb.ArquivosDeEntrada;
            object[,] vazExcedentes = new object[321, 12];
            int XIcen1 = wb.GetCoordinatesByRange("_cen1", "XI");
            int XFcen1 = wb.GetCoordinatesByRange("_cen1", "XF");
            int YIcen1 = wb.GetCoordinatesByRange("_cen1", "YI");
            int YFcen1 = wb.GetCoordinatesByRange("_cen1", "YF");

            var datasPrevs = wb.DatasPrevs;
            List<DateTime> datas = new List<DateTime>();
            for (int d = 1; d <= datasPrevs.Length; d++)
            {
                datas.Add((DateTime)datasPrevs[1, d]);
            }


            ///log parcial 1 -- original
            wb.Saida1 = wb.PrevsCen1;//[a,1] = id posto, [a,3...] = vazões 

            List<Acomph> acompH = null;
            var semanaprevisao = 6; //dias necessários para considerar a média como semanal


            if (useAcomph)
            {
                acompH = Tools.GetAcomphData(DateTime.Today.AddDays(-21), DateTime.Today);
            }

            var prevsCen1 = wb.PrevsCen1;
            var anoPrev = wb.AnoAtual;
            var usr = System.Environment.UserName.Replace('.', '_');
            // var tempFolder = @"Z:\previsaopld\shared\CHUVA-VAZAO\previvaz_" + usr + sufixoDePasta;
            //var tempFolder = @"L:\shared\CHUVA-VAZAO\previvaz_" + usr + sufixoDePasta;
            var tempFolder = Path.Combine(wb.Path, "arq_previvaz");

            if (prevsM2)
            {
                tempFolder = sufixoDePasta;
            }

            var teste = acompH.GroupBy(ac => new { ac.semana, ac.posto })
                   .Where(ac => ac.Count() >= semanaprevisao).ToList();

            if (acompH != null)
            {
                acompH.GroupBy(ac => new { ac.semana, ac.posto })
                    .Where(ac => ac.Count() >= semanaprevisao).ToList()
                    .ForEach(ac =>
                    {
                        for (int i = 1; i <= wb.SemanasPrevs.Length; i++)
                        {
                            if ((double)wb.SemanasPrevs[1, i] == (double)ac.Key.semana)
                            {
                                prevsCen1[ac.Key.posto, i + 2] = ac.Average(x => x.qNat);

                                if (postosIncrementais.ContainsKey(ac.Key.posto))
                                {
                                    if (ac.Key.posto == 253)//tocantins sao salvador + canabrava
                                    {
                                        var canaBrava = acompH.Where(x => x.posto == 191 && x.semana == ac.Key.semana).ToList();
                                        var dado = canaBrava.Average(x => x.qInc);
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc) + dado;

                                    }
                                    else if (ac.Key.posto == 273)//tocantins lajeado + peixe Angical
                                    {
                                        var peixeAngi = acompH.Where(x => x.posto == 257 && x.semana == ac.Key.semana).ToList();
                                        var dado = peixeAngi.Average(x => x.qInc);
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc) + dado;

                                    }
                                    else
                                    {
                                        prevsCen1[postosIncrementais[ac.Key.posto].Item2, i + 2] = ac.Average(x => x.qInc);
                                    }
                                }

                                if (ac.Key.posto == 169) prevsCen1[168, i + 2] = ac.Average(x => x.qInc);

                                // if (ac.Key.posto == 239) prevsCen1[239, i + 2] = ac.Average(x => x.qNat) - (double)prevsCen1[237, i + 2];
                                // if (ac.Key.posto == 242) prevsCen1[242, i + 2] = ac.Average(x => x.qNat) - (double)prevsCen1[239, i + 2];

                            }
                        }
                    });
            }



            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
            Directory.CreateDirectory(tempFolder);

            var postosPrevivaz = Directory.GetFiles(previvazBaseFolder).GroupBy(x =>
                System.Text.RegularExpressions.Regex.Match(
                Path.GetFileNameWithoutExtension(x),
                @"^\d+").Value
               );

            Dictionary<int, List<object>> results = new Dictionary<int, List<object>>();
            var prevDecks = new List<Compass.CommomLibrary.Previvaz.Deck>();
            //Globals.ThisAddIn.Application.StatusBar = "Executando : ";
            foreach (var p in postosPrevivaz)
            {//atualiza os dados do inp com o numero das semanas e o  arquivo str com as vazões semanais da planilha
                var prevDeck = new Compass.CommomLibrary.Previvaz.Deck(p.Key);
                prevDeck.GetFiles(previvazBaseFolder);


                if (prevDeck[CommomLibrary.Previvaz.DeckDocument.str] == null || prevDeck[CommomLibrary.Previvaz.DeckDocument.lim] == null)
                {
                    continue;
                }


                prevDecks.Add(prevDeck);

                var path = Path.Combine(tempFolder, p.Key);

                int posto = int.Parse(prevDeck.Posto);

                var inp = (Compass.CommomLibrary.Previvaz.Inp)prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document;
                var str = (Compass.CommomLibrary.Previvaz.Str)prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document;

                inp.SemanaPrevisao = Convert.ToInt32(wb.SemanasPrevs[1, 1]);
                inp.AnoPrevisao = anoPrev;

                var numerosSem = wb.GetSemanaPrevsStr(inp.NumSemanasHist);
                for (int s = 1; s < numerosSem.Length; s++)//12
                {

                    if (postosIncrementais.ContainsKey(posto) && (prevsCen1[postosIncrementais[posto].Item2, s + 2] is double && (double)prevsCen1[postosIncrementais[posto].Item2, s + 2] != 0))
                    {
                        str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = (double)prevsCen1[postosIncrementais[posto].Item2, s + 2];//se o posto incremental desse posto conter dados da semana, ele atribui esse dado para o posto(esses processo só acontece da semana rv0 até a atual)
                        var dd = str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])];
                    }
                    else if (postosIncrementais.ContainsKey(posto) && (prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0))  //só entra nesse if para as semanas previstas,para esse posto que contem posto incremental,a sua vazão da semana prevista sera
                    {                                                                                                                               //igual à sua vazão menos a vazão de seus postos montantes ex: vazão do posto 34 = v34 - (v33-v18-v99-v241-v261)  //OBS: o valor minimo será sempre 12
                        if (posto == 253 || posto == 273 || posto == 271 || posto == 275)//postos tocantins que rodam com a incremental alocada nos postos auxiliares 308~311
                        {
                            str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = (double)prevsCen1[posto, s + 2];
                        }
                        else
                        {
                            var v = (double)prevsCen1[posto, s + 2]
                            - postosIncrementais[posto].Item1.Sum(pm => (double)prevsCen1[pm, s + 2]);
                            str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = Math.Max(v, 12);
                        }


                    }
                    else if (posto == 239 && prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        var v = (double)prevsCen1[239, s + 2] - (double)prevsCen1[237, s + 2];// para o posto 239(ibitinga) sera usado a vazão de ibitinga menoa a vazão de barra bonita(237)
                        if (v < 1) v = 5;                                                     //OBS: o valor minimo sempre sera 5
                        str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = v;
                    }
                    else if (posto == 242 && prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        var v = (double)prevsCen1[242, s + 2] - (double)prevsCen1[239, s + 2];// para o posto 242(N.Avanhadava) sera usado a vazão de N.Avanhadava menoa a vazão de ibitinga(239)
                        if (v < 1) v = 5;                                                     //OBS: o valor minimo sempre sera 5
                        str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = v;
                    }
                    else if (prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0)
                    {
                        str[inp.AnoPrevisao, Convert.ToInt32(numerosSem[1, s])] = (double)prevsCen1[posto, s + 2];
                    }
                    else
                    {
                        break;
                    }

                    var proxSem = Convert.ToInt32(numerosSem[1, s + 1]);

                    if (proxSem < inp.SemanaPrevisao)    //confere se a proxima semana ainda faz parte do mesmo ano se nao incrementa o ano
                    {
                        inp.AnoPrevisao = inp.AnoPrevisao + 1;
                    }
                    if (proxSem == 2)
                    {
                        str.AnoFinal = inp.AnoPrevisao;
                    }

                    inp.SemanaPrevisao = proxSem;
                }

                prevDeck.CopyFilesToFolder(path);
            }
            Previvaz previvaz = new Previvaz();
            Parallel.ForEach(postosPrevivaz, x =>
            {
                var path = Path.Combine(tempFolder, x.Key);
                previvaz.RunPrevivazLocalVM(path);

            });
            //Services.Linux.Run(tempFolder, @"/home/producao/PrevisaoPLD/shared/previvaz/previvaz3.sh", "previvaz", true, true, "hide");
            //                Services.Previvaz.RunOnLinux(tempFolder);

            if (encad)
            {

                //rodadas encadeadas previvaz inicio
                foreach (var prevDeck in prevDecks)
                {
                    var rs = prevDeck.GetFut();
                    if (rs.Count > 0)
                    {
                        //object[] r = (object[])rs.First().Value;
                        results.Add((int)rs[0], rs);


                        var inp = prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document as CommomLibrary.Previvaz.Inp;
                        var str = prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document as CommomLibrary.Previvaz.Str;

                        inp.SemanaPrevisao += 6;

                        // var numSemanas = 52 + inp.NumSemanasHist;
                        //tratar virada de ano
                        //var numSemanas = inp.NumSemanasHist - inp.SemanaPrevisao;

                        //if (numSemanas == -1)
                        //{
                        //    inp.AnoPrevisao = inp.AnoPrevisao + 1;
                        //}
                        if (inp.SemanaPrevisao > inp.NumSemanasHist)
                        {
                            inp.AnoPrevisao = inp.AnoPrevisao + 1;
                            str.AnoFinal = inp.AnoPrevisao;
                            inp.SemanaPrevisao = inp.SemanaPrevisao - inp.NumSemanasHist;
                        }
                        str.AnoFinal = inp.AnoPrevisao;



                        // var str = prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document as CommomLibrary.Previvaz.Str;
                        try
                        {
                            var anoInicial = Convert.ToInt32(rs[1]);
                            var semanaInicial = (int)rs[3];

                            for (int i = 0; i < 6; i++)
                            {
                                str[anoInicial, semanaInicial] = (double)rs[4 + i];

                                semanaInicial++;

                                if (semanaInicial > inp.NumSemanasHist)
                                {
                                    semanaInicial = 1;
                                    anoInicial++;
                                }


                            }


                            str.SaveToFile();
                            inp.SaveToFile();
                            ///renomar o *_fut;
                            var camFut = Path.Combine(prevDeck.Folder, prevDeck.Posto + "_fut.DAT");
                            if (System.IO.File.Exists(camFut))
                            {
                                System.IO.File.Move(camFut, Path.Combine(prevDeck.Folder, prevDeck.Posto + "_futbkp.DAT"));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }

                }

                Parallel.ForEach(postosPrevivaz, x =>
                {
                    var path_exe = Path.Combine(tempFolder, x.Key);
                    previvaz.RunPrevivazLocalVM(path_exe);

                });
                //Services.Linux.Run(tempFolder, @"/home/producao/PrevisaoPLD/shared/previvaz/previvaz3.sh", "previvaz", true, true, "hide");
                //                Services.Previvaz.RunOnLinux(tempFolder);

                //rodadas encadeadas previvaz fimmmm
            }


            foreach (var prevDeck in prevDecks)
            {
                var rs = prevDeck.GetFut();
                if (rs.Count > 0)
                {
                    //   object[] r = (object[])rs.First().Value;

                    if (results.ContainsKey((int)rs[0]))
                    {
                        var rAnterior = results[(int)rs[0]];

                        rAnterior.Add(rs[4]);
                        rAnterior.Add(rs[5]);
                        rAnterior.Add(rs[6]);
                        rAnterior.Add(rs[7]);
                        rAnterior.Add(rs[8]);
                        rAnterior.Add(rs[9]);

                    }
                    else
                        results.Add((int)rs[0], rs);
                }
            }


            // coloca resultado do previvaz na entrada para calcular postos artificias;
            foreach (var r in results)
            {
                var numposto = r.Key;
                var deck = prevDecks.Where(x => x.Posto == numposto.ToString()).First();
                var inp = deck[CommomLibrary.Previvaz.DeckDocument.inp].Document as CommomLibrary.Previvaz.Inp;
                var numerosSem = wb.GetSemanaPrevsStr(inp.NumSemanasHist);

                var c = 1;
                for (; c <= numerosSem.Length; c++)//12
                {
                    if ((int)(double)numerosSem[1, c] == (int)r.Value[3])
                    {
                        c += 2;
                        break;
                    }
                }

                // var c = (int)r[3] - (int)(double)wb.SemanasPrevs[1, 1] + 3;
                var posto = r.Key;
                for (int i = 0; i < numerosSem.Length - 1 && (c + i < numerosSem.Length + 3); i++)//11  15
                {
                    var vaz = 4 + i < r.Value.Count ? r.Value[4 + i] : r.Value.Last(); // completa todo o horizonte de 20 semanas repetindo o ultimo resultado do previvaz para além da 6a semana de previsão.

                    if (postosIncrementais.ContainsKey(posto))
                        prevsCen1[postosIncrementais[posto].Item2, c + i] = vaz;

                    else if (posto == 239) prevsCen1[239, c + i] = (double)vaz + (double)prevsCen1[237, c + i];
                    else if (posto == 242) prevsCen1[242, c + i] = (double)vaz + (double)prevsCen1[239, c + i];
                    else prevsCen1[posto, c + i] = vaz;
                }
                //if (prevsM2)
                //{
                //    int startIndex = 0;
                //    for (int x = 0; 4 + x < r.Value.Count; x++)
                //    {
                //        var vaz = r.Value[4 + x];

                //        if (c + x > 14)
                //        {
                //            if (postosIncrementais.ContainsKey(posto))
                //                vazExcedentes[postosIncrementais[posto].Item2, startIndex] = vaz;

                //            else if (posto == 239) vazExcedentes[239, startIndex] = (double)vaz + (double)vazExcedentes[237, startIndex];
                //            else if (posto == 242) vazExcedentes[242, startIndex] = (double)vaz + (double)vazExcedentes[239, startIndex];
                //            else vazExcedentes[posto, startIndex] = vaz;

                //            startIndex++;
                //        }

                //    }
                //}
            }

            #region trata posto 169

            var str156 = (Compass.CommomLibrary.Previvaz.Str)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.str].Document;
            var str158 = (Compass.CommomLibrary.Previvaz.Str)prevDecks.First(x => x.Posto == "158")[CommomLibrary.Previvaz.DeckDocument.str].Document;
            var sem_2 = Convert.ToInt32(wb.SemanasPrevs[1, 1]) - 2;

            var sem_Atual = Convert.ToInt32(wb.SemanaAtualIndice) + 2; // Adiciona 2 para ajustar a celula da planilha 
            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            {
                sem_Atual = sem_Atual - 1;
            }
            /*
                        if (sem_2 < 1)
                        {
                            sem_2 = sem_2 +
                                ((Compass.CommomLibrary.Previvaz.Inp)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.inp].Document).NumSemanasHist;
                        }

                        prevsCen1[169, 3] = (double)prevsCen1[168, 3]
                            + str156[anoPrev, sem_2]
                            + str158[anoPrev, sem_2];

                        sem_2 = Convert.ToInt32(wb.SemanasPrevs[1, 1]) - 1;
                        if (sem_2 < 1)
                        {
                            sem_2 = sem_2 +
                                ((Compass.CommomLibrary.Previvaz.Inp)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.inp].Document).NumSemanasHist;
                        }

                        prevsCen1[169, 4] = (double)prevsCen1[168, 4]
                            + str156[anoPrev, sem_2]
                            + str158[anoPrev, sem_2];
                            */
            for (int i = sem_Atual + 3; i <= wb.SemanasPrevs.Length + 2 && prevsCen1[168, i] is double; i++) // Adiciona 3 para começar depois da ultima semana de CPINS //14
                prevsCen1[169, i] = (double)prevsCen1[156, i - 2] + (double)prevsCen1[158, i - 2] + (double)prevsCen1[168, i];

            //if (prevsM2)
            //{
            //    for (int i = 0; i < 12 && vazExcedentes[168, i] is double; i++)
            //    {
            //        if (i == 0)//usa os dados do prevscen1 da semana11
            //        {
            //            vazExcedentes[169, i] = (double)prevsCen1[156, 13] + (double)prevsCen1[158, 13] + (double)vazExcedentes[168, i];
            //        }
            //        else if (i == 1)//usa os dados do prevscen1 da semana12
            //        {
            //            vazExcedentes[169, i] = (double)prevsCen1[156, 14] + (double)prevsCen1[158, 14] + (double)vazExcedentes[168, i];
            //        }
            //        else
            //        {
            //            vazExcedentes[169, i] = (double)vazExcedentes[156, i - 2] + (double)vazExcedentes[158, i - 2] + (double)vazExcedentes[168, i];
            //        }
            //    }
            //}

            #endregion trata posto 169

            wb.Entrada = prevsCen1;

            wb.Regressoes = true;

            for (int posto = 1; posto <= 320; posto++)
                for (int s = 0; s < wb.SemanasPrevs.Length; s++)//12
                    if (!(prevsCen1[posto, 3 + s] is double) || (double)prevsCen1[posto, 3 + s] < 1) prevsCen1[posto, 3 + s] = wb.PrevsCen1[posto, 3 + s];

            foreach (var pn in postosIncrementais)
            {
                for (int i = 0; i < wb.SemanasPrevs.Length; i++)//12
                {
                    if (
                        (prevsCen1[pn.Key, 3 + i] is double && (double)prevsCen1[pn.Key, 3 + i] < 1)
                        || !(prevsCen1[pn.Key, 3 + i] is double) || (prevsCen1[257, 3 + i] is double && (double)prevsCen1[257, 3 + i] < 1)
                        )
                    {
                        if (pn.Key == 253)// sao salvador + canabrava
                        {
                            double fatcana = 0.504;
                            prevsCen1[191, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatcana) + ((double)prevsCen1[270, 3 + i]);
                            double fatSal = 0.496;
                            prevsCen1[pn.Key, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatSal) + ((double)prevsCen1[191, 3 + i]);

                        }
                        else if (pn.Key == 273)//lajeado + peixe angical
                        {
                            double fatpeixe = 0.488;
                            prevsCen1[257, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatpeixe) + ((double)prevsCen1[253, 3 + i]);
                            double fatlaj = 0.512;
                            prevsCen1[pn.Key, 3 + i] = ((double)prevsCen1[pn.Value.Item2, 3 + i] * fatlaj) + ((double)prevsCen1[257, 3 + i]);

                        }


                        else
                        {
                            prevsCen1[pn.Key, 3 + i] = (double)prevsCen1[pn.Value.Item2, 3 + i]
                            + pn.Value.Item1.Sum(pMn => (double)prevsCen1[pMn, 3 + i]);
                        }

                    }
                }

            }

            wb.Entrada = prevsCen1;

            // itera em todos os postos para se não houver resultado, na entrada, utiliza o resultado da regressão.
            for (int posto = 1; posto <= 320; posto++)
                for (int s = 0; s < wb.SemanasPrevs.Length; s++)//12
                    if (!(prevsCen1[posto, 3 + s] is double) || (double)prevsCen1[posto, 3 + s] < 1) prevsCen1[posto, 3 + s] = wb.PrevsCen1[posto, 3 + s];

            wb.Entrada = prevsCen1;
            wb.Regressoes = false;

            //try
            //{
            //    wb.FillRange(vazExcedentes, "Cen1", XIcen1 - 1, XIcen1 - 1 + XFcen1, YFcen1 + 7, YFcen1 + 18);//preenchendo as celulas á frente do range "_cen1" obs +6 nas colunas  pra não cobrir celulas comreferencia de formula
            //}
            //catch (Exception e)
            //{

            //    e.ToString();
            //}

            try
            {
                if (wb.GravarPrevivaz)
                {

                    //var destPath = Path.Combine(wb.Path, "arq_previvaz");

                    //if (Directory.Exists(destPath))
                    //{
                    //    Directory.Delete(destPath, true);
                    //}


                    //Tools.moveDirectory(tempFolder, destPath);
                }
            }
            catch { }

        }

        static readonly string[] previvazFiles = { /*"previvaz.exe", "previvaz.dll",*/ "ENCAD.DAT" };

        string previvazExe = "";
        string workFolder = null;

        public void Run(string inpFolder/*, bool saveFileToFolder = false*/)
        {

            workFolder = inpFolder;
            CopyPrevivazFiles(workFolder);
            RunPrevivaz();

        }

        public void ClearTempFolder()
        {
            ClearTempFolder(workFolder);
        }

        private void ClearTempFolder(string workFolder)
        {
            if (workFolder != null)
            {
                if (Directory.Exists(workFolder))
                {

                    var files = Directory.GetFiles(workFolder);
                    foreach (var file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                    Directory.Delete(workFolder, true);
                }
            }
        }
        public void RunPrevivazLocalVM(string workFolder = null)
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(workFolder, "ENCAD.DAT")))
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(workFolder, "ENCAD.DAT"), "ALGHAO234PGJAGAENCAD");
            }
            System.Diagnostics.Process pr = new System.Diagnostics.Process();

            var si = pr.StartInfo;

            si.FileName = @"C:\Sistemas\PREVIVAZ\6.1.0\previvaz.exe";

            if (!System.IO.File.Exists(si.FileName))
            {
                si.FileName = @"H:\TI - Sistemas\PREVIVAZ\6.1.0\previvaz.exe";
            }
            si.WorkingDirectory = workFolder;

            si.CreateNoWindow = true;
            si.UseShellExecute = false;

            pr.StartInfo = si;

            pr.Start();

            pr.WaitForExit();

            var files_posto = System.IO.Directory.GetFiles(workFolder);



            string[] arq_delete = { "PREVP.DAT", "PREVISAO.DAT", "PREVT.DAT", "RUNSTATE.DAT", "RUNTRACE.DAT", "LIMITES.TXT", "FAIXAS.TXT", ".exc", ".BCX", "_MOD.DAT", ".rel" };



            foreach (var arq in files_posto)
            {
                if (Path.GetFileName(arq).Contains("_fut.DAT"))
                {
                    var lines = System.IO.File.ReadAllLines(arq).Count();

                    if (lines != 4)
                    {
                        arq_delete[10] = "_fut.DAT";
                    }
                }
                foreach (var del in arq_delete)
                {
                    if (Path.GetFileName(arq).Contains(del))
                    {
                        System.IO.File.Delete(System.IO.Path.Combine(workFolder, arq));
                    }
                }

            }


        }
        public void RunPrevivaz(string path)
        {

            System.Diagnostics.Process pr = new System.Diagnostics.Process();

            var si = pr.StartInfo;

            si.FileName = previvazExe;

            si.WorkingDirectory = path;

            si.CreateNoWindow = true;
            si.UseShellExecute = false;

            pr.StartInfo = si;

            pr.Start();

            pr.WaitForExit();
        }
        public void RunPrevivaz()
        {

            System.Diagnostics.Process pr = new System.Diagnostics.Process();

            var si = pr.StartInfo;

            si.FileName = previvazExe;

            si.WorkingDirectory = workFolder;

            si.CreateNoWindow = true;
            si.UseShellExecute = false;

            pr.StartInfo = si;

            pr.Start();

            pr.WaitForExit();
        }

        private void CopyPrevivazFiles(string workFolder)
        {

            var previvazProgramFolder = System.Configuration.ConfigurationManager.AppSettings["previvazPath"];
            foreach (var file in previvazFiles)
            {
                System.IO.File.Copy(
                    Path.Combine(previvazProgramFolder, file),
                    Path.Combine(workFolder, file)
                    );
            }
            previvazExe = Path.Combine(previvazProgramFolder, "previvaz.exe");
        }

        public class File
        {
            public string Name { get; set; }
            public string FullPath { get; set; }
            public byte[] Content { get; set; }
        }


        public static void ProcessResultsPart2(string path, bool encad)
        {
            prevDeck = null;

            DateTime Data = DateTime.Today;// TODO: TEMPORÁRIO
            DateTime dtTemp = new DateTime();
            var rev = Tools.GetCurrRev(Data);


            if (rev.rev == 0 && Data.Day > 23 && Data.Day <= 31)
            {
                dtTemp = Data.AddMonths(+1);
            }
            else
                dtTemp = Data;

            string pathPrevivaz = Path.Combine("H:\\Middle - Preço\\Acompanhamento de vazões\\", dtTemp.ToString("MM_yyyy"), "Dados_de_Entrada_e_Saida_" + dtTemp.ToString("yyyyMM") + "_RV" + rev.rev, "Previvaz", "Arq_Entrada");

            var semanaprevisao = 6; //dias necessários para considerar a média como semanal

            if (true)
            {
                acompH = Tools.GetAcomphData(DateTime.Today.AddDays(-28), DateTime.Today);
            }

            var testeAcomph = acompH.GroupBy(ac => new { ac.semana, ac.posto })
                    .Where(ac => ac.Count() >= semanaprevisao).ToList();

            if (System.IO.File.Exists(Path.Combine(path, "Propagacoes_Automaticas.txt")))
            {
                var Read = System.IO.File.ReadAllText(Path.Combine(path, "Propagacoes_Automaticas.txt"));
                //testeRead.ReadToEnd();

                DataContractJsonSerializer desser = new DataContractJsonSerializer(typeof(List<Propagacao>));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(Read));
                Propagacoes = ((List<Propagacao>)desser.ReadObject(ms)).ToList();
            }

            var anoPrev = rev.revDate.Year;

            var usr = System.Environment.UserName.Replace('.', '_');

            var tempFolder = Path.Combine(path, "arq_previvaz");


            DateTime inicioMes = new DateTime(rev.revDate.Year, rev.revDate.Month, 1);//data da Rv0 do mês para preencher com dados do prevs oficial
            var semanaZero = inicioMes;

            while (semanaZero.DayOfWeek != DayOfWeek.Saturday)
            {
                semanaZero = semanaZero.AddDays(-1);
            }
            semanaZero = semanaZero.AddDays(6);//termino da semana rv0 do mês

            var numSem = Tools.GetWeekNumberAndYear(semanaZero);
            var SemanasPrevs = Tools.GetNumDatSem(semanaZero, numSem.Item1);// as doze semanas que serão utilizadas no prevs

            DateTime uSexta = acompH.Select(x => x.dt).Max();
            while (uSexta.DayOfWeek != DayOfWeek.Friday) uSexta = uSexta.AddDays(-1);

            if (acompH != null)
            {
                acompH.GroupBy(ac => new { ac.semana, ac.posto })
                    .Where(ac => ac.Count() >= semanaprevisao).ToList()
                    .ForEach(ac =>
                    {
                        try
                        {
                            for (int i = 0; i < SemanasPrevs.Count(); i++)
                            {
                                if ((double)SemanasPrevs[i].Item2 == (double)ac.Key.semana)
                                {
                                    if (ac.Key.posto == 169)
                                    {

                                    }

                                    var prop = Propagacoes.Where(x => x.IdPosto == ac.Key.posto).FirstOrDefault();
                                    prop.calMedSemanal[SemanasPrevs[i].Item1] = ac.Average(x => x.qNat);

                                    if (postosIncrementais.ContainsKey(ac.Key.posto))
                                    {
                                        if (ac.Key.posto == 253)//tocantins sao salvador + canabrava
                                        {
                                            var canaBrava = acompH.Where(x => x.posto == 191 && x.semana == ac.Key.semana).ToList();
                                            var dado = canaBrava.Average(x => x.qInc);

                                            var propinc = Propagacoes.Where(x => x.IdPosto == postosIncrementais[ac.Key.posto].Item2).FirstOrDefault();
                                            propinc.calMedSemanal[SemanasPrevs[i].Item1] = ac.Average(x => x.qInc) + dado;
                                        }
                                        else if (ac.Key.posto == 273)//tocantins lajeado + peixe Angical
                                        {
                                            var peixeAngi = acompH.Where(x => x.posto == 257 && x.semana == ac.Key.semana).ToList();
                                            var dado = peixeAngi.Average(x => x.qInc);

                                            var propinc = Propagacoes.Where(x => x.IdPosto == postosIncrementais[ac.Key.posto].Item2).FirstOrDefault();
                                            propinc.calMedSemanal[SemanasPrevs[i].Item1] = ac.Average(x => x.qInc) + dado;
                                        }
                                        else
                                        {
                                            var propinc = Propagacoes.Where(x => x.IdPosto == postosIncrementais[ac.Key.posto].Item2).FirstOrDefault();
                                            propinc.calMedSemanal[SemanasPrevs[i].Item1] = ac.Average(x => x.qInc);
                                        }

                                    }

                                    if (ac.Key.posto == 169)
                                    {
                                        var prop168 = Propagacoes.Where(x => x.IdPosto == 168).FirstOrDefault();
                                        prop168.calMedSemanal[SemanasPrevs[i].Item1] = ac.Average(x => x.qInc);
                                    }

                                }
                            }
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }

                    });
            }


            if (Propagacoes.Count != 0)
            {
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
                Directory.CreateDirectory(tempFolder);

                var postosPrevivaz = Directory.GetFiles(pathPrevivaz).GroupBy(x =>
                    System.Text.RegularExpressions.Regex.Match(
                    Path.GetFileNameWithoutExtension(x),
                    @"^\d+").Value
                   );

                Dictionary<int, List<object>> results = new Dictionary<int, List<object>>();

                var prevDecks = new List<Compass.CommomLibrary.Previvaz.Deck>();
                //Globals.ThisAddIn.Application.StatusBar = "Executando : ";
                foreach (var pPrevi in postosPrevivaz)
                {
                    prevDeck = new Compass.CommomLibrary.Previvaz.Deck(pPrevi.Key);
                    prevDeck.GetFiles(pathPrevivaz);

                    if (prevDeck[CommomLibrary.Previvaz.DeckDocument.str] == null || prevDeck[CommomLibrary.Previvaz.DeckDocument.lim] == null)
                    {
                        continue;
                    }

                    prevDecks.Add(prevDeck);

                    //var path = Path.Combine(tempFolder, p.Key);

                    int posto = int.Parse(prevDeck.Posto);

                    var inp = (Compass.CommomLibrary.Previvaz.Inp)prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document;
                    var str = (Compass.CommomLibrary.Previvaz.Str)prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document;
                    inp.SemanaPrevisao = SemanasPrevs[0].Item2;
                    inp.AnoPrevisao = anoPrev;
                    // daqui pra frente

                    foreach (var propa in Propagacoes.Where(f => f.IdPosto == posto))
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            if (postosIncrementais.ContainsKey(posto))
                            {
                                var propinc = Propagacoes.Where(x => x.IdPosto == postosIncrementais[posto].Item2).FirstOrDefault();
                                if (propinc.calMedSemanal.ContainsKey(SemanasPrevs[i].Item1) && i != 0 || propinc.calMedSemanal.ContainsKey(SemanasPrevs[i].Item1) && posto == 266)
                                {
                                    str[inp.AnoPrevisao, SemanasPrevs[i].Item2] = Math.Round(propinc.calMedSemanal[SemanasPrevs[i].Item1]);
                                }
                                else if (i == 0 || propa.calMedSemanal.ContainsKey(SemanasPrevs[i].Item1))
                                {
                                    if (posto == 253 || posto == 273 || posto == 271 || posto == 275)//postos tocantins que rodam com a incremental alocada nos postos auxiliares 308~311
                                    {
                                        if (propinc.calMedSemanal.ContainsKey(SemanasPrevs[i].Item1))
                                        {
                                            str[inp.AnoPrevisao, SemanasPrevs[i].Item2] = Math.Round(propinc.calMedSemanal[SemanasPrevs[i].Item1]);
                                        }
                                    }
                                    else
                                    {
                                        var vazMontantes = propa.PostoMontantes.SelectMany(x => x.Propaga.calMedSemanal).Where(x => x.Key == SemanasPrevs[i].Item1);

                                        var v = propa.calMedSemanal[SemanasPrevs[i].Item1] - vazMontantes.Sum(x => (double)x.Value);
                                        str[inp.AnoPrevisao, SemanasPrevs[i].Item2] = Math.Max(v, 12);
                                    }

                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (posto == 239 && propa.calMedSemanal.ContainsKey(SemanasPrevs[i].Item1))//ibitinga roda apenas com sua vazão incremental
                            {
                                var barrBoni = Propagacoes.Where(x => x.IdPosto == 237).FirstOrDefault();
                                var v = propa.calMedSemanal[SemanasPrevs[i].Item1] - barrBoni.calMedSemanal[SemanasPrevs[i].Item1];
                                if (v < 1) v = 5;
                                str[inp.AnoPrevisao, SemanasPrevs[i].Item2] = v;
                            }
                            else if (posto == 242 && propa.calMedSemanal.ContainsKey(SemanasPrevs[i].Item1))//N.Avanhandava roda apenas com sua vazão incremental
                            {
                                var ibitinga = Propagacoes.Where(x => x.IdPosto == 239).FirstOrDefault();

                                var v = propa.calMedSemanal[SemanasPrevs[i].Item1] - ibitinga.calMedSemanal[SemanasPrevs[i].Item1];

                                if (v < 1) v = 5;
                                str[inp.AnoPrevisao, SemanasPrevs[i].Item2] = v;
                            }
                            else if (propa.calMedSemanal.ContainsKey(SemanasPrevs[i].Item1) && propa.calMedSemanal[SemanasPrevs[i].Item1] > 0)
                            {
                                str[inp.AnoPrevisao, SemanasPrevs[i].Item2] = Math.Round(propa.calMedSemanal[SemanasPrevs[i].Item1]);
                            }
                            else
                            {
                                break;
                            }

                            var proxSem = SemanasPrevs[i + 1].Item2;

                            if (proxSem < inp.SemanaPrevisao)
                            {
                                inp.AnoPrevisao = inp.AnoPrevisao + 1;
                            }
                            if (proxSem == 2)
                            {
                                str.AnoFinal = inp.AnoPrevisao;
                            }

                            inp.SemanaPrevisao = proxSem;
                        }
                        //=======================
                        //var getWeek = Tools.GetWeekNumberAndYear(propa.medSemanalNatural.Union(propa.medSemanalIncremental).Last().Key);

                        ////getWeek.Item1 = getWeek.Item1+1;

                        //inp.SemanaPrevisao = postosIncrementais.ContainsKey(posto) ? getWeek.Item1 : getWeek.Item1 + 1;
                        //inp.AnoPrevisao = getWeek.Item2;

                        //foreach (var data in propa.medSemanalNatural.Union(propa.medSemanalIncremental).Select(x => x.Key))
                        //{
                        //    int semana = getWeek.Item1;//(posto == 245 || posto == 246) ? getWeek.Item1 - 1 : getWeek.Item1;

                        //    if (postosIncrementais.ContainsKey(posto))
                        //    {
                        //        str[inp.AnoPrevisao, semana - 1] = propa.medSemanalIncremental.Where(g => g.Key == data).First().Value;
                        //    }
                        //    else if (posto == 239 || posto == 242)
                        //    {
                        //        str[inp.AnoPrevisao, semana] = propa.medSemanalIncremental[data] + propa.PostoMontantes.Sum(b => b.Propaga.medSemanalIncremental[data]);
                        //    }
                        //    else
                        //    {
                        //        if (posto == 99)
                        //        {
                        //            inp.SemanaPrevisao = inp.SemanaPrevisao - 1;
                        //            str[inp.AnoPrevisao, semana - 1] = propa.medSemanalNatural[data];
                        //        }
                        //        else
                        //            str[inp.AnoPrevisao, semana] = propa.medSemanalNatural[data];
                        //    }
                        //}
                        //============================================================================
                    }

                    prevDeck.CopyFilesToFolder(Path.Combine(tempFolder, posto.ToString()));
                }
                Previvaz previvaz = new Previvaz();
                Parallel.ForEach(postosPrevivaz, x =>
                {
                    var pasta = Path.Combine(tempFolder, x.Key);
                    previvaz.RunPrevivazLocalVM(pasta);

                });
                //if (Services.Linux.Run(tempFolder, @"/home/producao/PrevisaoPLD/shared/previvaz/previvaz3.sh", "previvaz", true, true, "hide"))
                // {
                if (encad)
                {

                    //rodadas encadeadas previvaz inicio
                    foreach (var prevDeck in prevDecks)
                    {
                        var rs = prevDeck.GetFut();
                        if (rs.Count > 0)
                        {
                            //object[] r = (object[])rs.First().Value;
                            results.Add((int)rs[0], rs);


                            var inp = prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document as CommomLibrary.Previvaz.Inp;
                            var str = prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document as CommomLibrary.Previvaz.Str;

                            inp.SemanaPrevisao += 6;

                            // var numSemanas = 52 + inp.NumSemanasHist;
                            //tratar virada de ano
                            //var numSemanas = inp.NumSemanasHist - inp.SemanaPrevisao;

                            //if (numSemanas == -1)
                            //{
                            //    inp.AnoPrevisao = inp.AnoPrevisao + 1;
                            //}
                            if (inp.SemanaPrevisao > inp.NumSemanasHist)
                            {
                                inp.AnoPrevisao = inp.AnoPrevisao + 1;
                                str.AnoFinal = inp.AnoPrevisao;
                                inp.SemanaPrevisao = inp.SemanaPrevisao - inp.NumSemanasHist;
                            }

                            str.AnoFinal = inp.AnoPrevisao;


                            // var str = prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document as CommomLibrary.Previvaz.Str;
                            try
                            {
                                var anoInicial = Convert.ToInt32(rs[1]);
                                var semanaInicial = (int)rs[3];

                                for (int i = 0; i < 6; i++)
                                {
                                    str[anoInicial, semanaInicial] = (double)rs[4 + i];

                                    semanaInicial++;

                                    if (semanaInicial > inp.NumSemanasHist)
                                    {
                                        semanaInicial = 1;
                                        anoInicial++;
                                    }


                                }


                                str.SaveToFile();
                                inp.SaveToFile();
                                ///renomar o *_fut;
                                var camFut = Path.Combine(prevDeck.Folder, prevDeck.Posto + "_fut.DAT");
                                if (System.IO.File.Exists(camFut))
                                {
                                    System.IO.File.Move(camFut, Path.Combine(prevDeck.Folder, prevDeck.Posto + "_futbkp.DAT"));
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }

                    }
                    Parallel.ForEach(postosPrevivaz, x =>
                    {
                        var path_exe = Path.Combine(tempFolder, x.Key);
                        previvaz.RunPrevivazLocalVM(path_exe);

                    });

                    // Services.Linux.Run(tempFolder, @"/home/producao/PrevisaoPLD/shared/previvaz/previvaz3.sh", "previvaz", true, true, "hide");
                    //                Services.Previvaz.RunOnLinux(tempFolder);

                    //rodadas encadeadas previvaz fimmmm
                }

                foreach (var prevDeck in prevDecks)
                {
                    var rs = prevDeck.GetFut();// coleta os resultados do previvaz
                    if (rs.Count > 0)
                    {

                        if (results.ContainsKey((int)rs[0]))
                        {
                            var rAnterior = results[(int)rs[0]];

                            rAnterior.Add(rs[4]);
                            rAnterior.Add(rs[5]);
                            rAnterior.Add(rs[6]);
                            rAnterior.Add(rs[7]);
                            rAnterior.Add(rs[8]);
                            rAnterior.Add(rs[9]);

                        }
                        else
                            results.Add((int)rs[0], rs);
                    }
                }

                // coloca os resultado do previvaz nas propagações para calcular postos artificias;
                foreach (var r in results)
                {

                    var posto = r.Key;
                    var prop = Propagacoes.Where(x => x.IdPosto == posto).FirstOrDefault();

                    try
                    {
                        foreach (var sem in SemanasPrevs)
                        {
                            if (postosIncrementais.ContainsKey(posto))
                            {
                                var propInc = Propagacoes.Where(x => x.IdPosto == postosIncrementais[posto].Item2).FirstOrDefault();

                                if (!propInc.calMedSemanal.ContainsKey(sem.Item1))
                                {
                                    var dat1 = sem.Item1;//primeira semana sem dados no posto incremental
                                    var dat = SemanasPrevs.Where(x => x.Item2 == (int)r.Value[3]).Select(x => x.Item1).FirstOrDefault();//primeira semana de com dados do previvaz
                                    for (DateTime d = dat1; d < dat; d = d.AddDays(7))
                                    {
                                        //if (d < dat)
                                        //{
                                        //    propInc.calMedSemanal[d] = 0;
                                        //}
                                    }


                                    for (int i = 4; i < r.Value.Count; i++)
                                    {
                                        var vaz = r.Value[i];
                                        propInc.calMedSemanal[dat] = (double)vaz;
                                        dat = dat.AddDays(7);
                                    }
                                    while (dat <= SemanasPrevs.Last().Item1)//caso as 6 semanas do previvaz não completem o horizonte de 12 semanas a vazão da ultima semana disponível sera copiada para as restantes
                                    {
                                        propInc.calMedSemanal[dat] = propInc.calMedSemanal[dat.AddDays(-7)];
                                        dat = dat.AddDays(7);

                                    }
                                    break;
                                }
                            }

                            else if (posto == 239)//ibitinga
                            {
                                if (!prop.calMedSemanal.ContainsKey(sem.Item1))
                                {
                                    var dat = sem.Item1;
                                    var barraBoni = Propagacoes.Where(x => x.IdPosto == 237).FirstOrDefault();

                                    for (int i = 4; i < r.Value.Count; i++)
                                    {
                                        var vaz = r.Value[i];
                                        prop.calMedSemanal[dat] = (double)vaz + barraBoni.calMedSemanal[dat];//soma a vazão para compensar o que foi subtraido para rodar o previvaz
                                        dat = dat.AddDays(7);
                                    }
                                    while (dat <= SemanasPrevs.Last().Item1)//caso as 6 semanas do previvaz não completem o horizonte de 12 semanas a vazão da ultima semana disponível sera copiada para as restantes
                                    {
                                        prop.calMedSemanal[dat] = prop.calMedSemanal[dat.AddDays(-7)];
                                        dat = dat.AddDays(7);

                                    }
                                    break;
                                }
                            }
                            else if (posto == 242)//N.Avanhandava
                            {
                                if (!prop.calMedSemanal.ContainsKey(sem.Item1))
                                {
                                    var dat = sem.Item1;
                                    var Ibitinga = Propagacoes.Where(x => x.IdPosto == 239).FirstOrDefault();

                                    for (int i = 4; i < r.Value.Count; i++)
                                    {
                                        var vaz = r.Value[i];
                                        prop.calMedSemanal[dat] = (double)vaz + Ibitinga.calMedSemanal[dat];//soma a vazão para compensar o que foi subtraido para rodar o previvaz
                                        dat = dat.AddDays(7);
                                    }
                                    while (dat <= SemanasPrevs.Last().Item1)//caso as 6 semanas do previvaz não completem o horizonte de 12 semanas a vazão da ultima semana disponível sera copiada para as restantes
                                    {
                                        prop.calMedSemanal[dat] = prop.calMedSemanal[dat.AddDays(-7)];
                                        dat = dat.AddDays(7);

                                    }
                                    break;
                                }
                            }
                            else
                            {
                                if (!prop.calMedSemanal.ContainsKey(sem.Item1))
                                {
                                    var dat = sem.Item1;

                                    for (int i = 4; i < r.Value.Count; i++)
                                    {
                                        var vaz = r.Value[i];
                                        prop.calMedSemanal[dat] = (double)vaz;
                                        dat = dat.AddDays(7);
                                    }
                                    while (dat <= SemanasPrevs.Last().Item1)//caso as 6 semanas do previvaz não completem o horizonte de 12 semanas a vazão da ultima semana disponível sera copiada para as restantes
                                    {
                                        prop.calMedSemanal[dat] = prop.calMedSemanal[dat.AddDays(-7)];
                                        dat = dat.AddDays(7);
                                    }
                                    break;
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
                }


                #region trata posto 169 
                //Para completar o horizonte de 12 semanas previstas para o posto 169, suas vazões serão calculadas atraves da soma da vazão do posto 168 da semana 
                // em questão com as vazões dos postos 158 e 156 de duas semanas atrás.
                var p169 = Propagacoes.Where(x => x.IdPosto == 169).FirstOrDefault();
                var p168 = Propagacoes.Where(x => x.IdPosto == 168).FirstOrDefault();
                var p156 = Propagacoes.Where(x => x.IdPosto == 156).FirstOrDefault();
                var p158 = Propagacoes.Where(x => x.IdPosto == 158).FirstOrDefault();

                foreach (var sem in SemanasPrevs)//semanaPrevs contém as datas das 12 semanas 
                {
                    var dat = sem.Item1;
                    if (!p169.calMedSemanal.ContainsKey(dat))
                    {
                        //(-14) = duas semanas atrás
                        p169.calMedSemanal[dat] = p168.calMedSemanal[dat] + p156.calMedSemanal[dat.AddDays(-14)] + p158.calMedSemanal[dat.AddDays(-14)];
                    }
                }
                #endregion
                var testePro = Propagacoes;



                Propagacoes = IncluiPostos(Propagacoes);

                CalcularPostRegre(Propagacoes, SemanasPrevs);

                CalcularPostCalculados(SemanasPrevs);

                CopiaResultados(SemanasPrevs);
                #region codigo antigo
                ////===============================codigo antigo=========================================================
                //List<DateTime> listSex = Propagacoes.First().medSemanalNatural.Select(x => x.Key).ToList();

                //for (int i = 0; i < 6; i++) listSex.Add(listSex.Last().AddDays(+7));

                //foreach (var pPrevi in prevDecks)//var prevD in prevDecks.Where(b => b.Posto == posto))
                //{

                //    dynamic previ = pPrevi.GetFut();


                //    if (Propagacoes.Any(x => x.IdPosto == int.Parse(pPrevi.Posto)))
                //    {
                //        var propa = Propagacoes.Where(v => v.IdPosto == int.Parse(pPrevi.Posto)).First();
                //        DateTime dat = propa.medSemanalNatural.Last().Key.AddDays(7);

                //        for (int count = 4; count < 10; count++)
                //        {

                //            try
                //            {

                //                if (!propa.medSemanalNatural.ContainsKey(dat)) propa.medSemanalNatural[dat] = 0;

                //                propa.medSemanalNatural[dat] = Convert.ToDouble(previ[count]);

                //                dat = dat.AddDays(7);

                //                propa.OK = true;
                //            }
                //            catch (Exception ep)//TODO: tirar a Exception
                //            {
                //                propa.OK = false;
                //            }
                //        }
                //    }
                //    else if (acompH.Any(x => x.posto == int.Parse(pPrevi.Posto))) //Postos PREVIVAZ
                //    {

                //        Propagacao prop = new Propagacao();
                //        prop.IdPosto = int.Parse(pPrevi.Posto);
                //        prop.NomePostoFluv = "Posto Previvaz " + prop.IdPosto;


                //        try
                //        {
                //            foreach (var aph in acompH.Where(x => x.posto == int.Parse(pPrevi.Posto)).ToList())
                //            {
                //                if (!prop.VazaoIncremental.ContainsKey(aph.dt)) prop.VazaoIncremental[aph.dt] = 0;
                //                if (!prop.VazaoNatural.ContainsKey(aph.dt)) prop.VazaoNatural[aph.dt] = 0;

                //                prop.VazaoIncremental[aph.dt] = aph.qInc;
                //                prop.VazaoNatural[aph.dt] = aph.qNat;
                //            }


                //            foreach (DateTime dat in listSex)
                //            {
                //                if (dat <= uSexta)
                //                {
                //                    if (!prop.medSemanalNatural.ContainsKey(dat)) prop.medSemanalNatural[dat] = 0;
                //                    if (!prop.medSemanalIncremental.ContainsKey(dat)) prop.medSemanalIncremental[dat] = 0;

                //                    if (prop.medSemanalNatural[dat] == 0) prop.medSemanalNatural[dat] = prop.VazaoNatural.Where(x => (x.Key >= dat.AddDays(-6)) && x.Key <= dat).Select(x => x.Value).Average();
                //                    if (prop.medSemanalIncremental[dat] == 0) prop.medSemanalIncremental[dat] = prop.VazaoIncremental.Where(x => (x.Key >= dat.AddDays(-6)) && x.Key <= dat).Select(x => x.Value).Average();
                //                }
                //                else
                //                {

                //                }
                //            }

                //            for (int count = 4; count < 10; count++)
                //            {

                //                try
                //                {
                //                    DateTime dat = prop.medSemanalNatural.Last().Key.AddDays(7);
                //                    if (!prop.medSemanalNatural.ContainsKey(dat)) prop.medSemanalNatural[dat] = 0;
                //                    prop.medSemanalNatural[dat] = Convert.ToDouble(previ[count]);
                //                    //dat = dat.AddDays(7);
                //                    prop.OK = true;
                //                }
                //                catch (Exception ep)//TODO: tirar a Exception
                //                {
                //                    prop.OK = false;
                //                }
                //            }


                //            prop.OK = true;
                //        }
                //        catch (Exception ept)
                //        {
                //            prop.OK = false;
                //        }
                //        finally
                //        {
                //            Propagacoes.Add(prop);
                //        }
                //    }
                //    else if (pPrevi.Posto == "168")
                //    {
                //        #region teste 168
                //        //// var propa = Propagacoes.Where(v => v.IdPosto == int.Parse(pPrevi.Posto)).First();
                //        ////DateTime dat = Propagacoes.First().medSemanalNatural.Last().Key.AddDays(7);

                //        //Propagacao prop = new Propagacao();
                //        //prop.IdPosto = int.Parse(pPrevi.Posto);
                //        //prop.NomePostoFluv = "Posto " + prop.IdPosto;
                //        //foreach (DateTime d in listSex)
                //        //{


                //        //    if (!prop.medSemanalNatural.ContainsKey(d)) prop.medSemanalNatural[d] = 0;
                //        //    if (!prop.medSemanalIncremental.ContainsKey(d)) prop.medSemanalIncremental[d] = 0;




                //        //}
                //        //DateTime dat = listSex.Last();
                //        //for (int count = 9; count > 3; count--)
                //        //{

                //        //    try
                //        //    {

                //        //        if (!prop.medSemanalNatural.ContainsKey(dat)) prop.medSemanalNatural[dat] = 0;

                //        //        prop.medSemanalNatural[dat] = Convert.ToDouble(previ[count]);

                //        //        dat = dat.AddDays(-7);

                //        //        prop.OK = true;
                //        //    }
                //        //    catch (Exception ep)//TODO: tirar a Exception
                //        //    {
                //        //        prop.OK = false;
                //        //    }
                //        //}
                //        //Propagacoes.Add(prop);
                //        #endregion teste 168
                //        if (acompH.Any(x => x.posto == 169))
                //        {

                //            Propagacao prop = new Propagacao();
                //            prop.IdPosto = int.Parse(pPrevi.Posto);
                //            prop.NomePostoFluv = "Posto Previvaz " + prop.IdPosto;


                //            try
                //            {
                //                foreach (var aph in acompH.Where(x => x.posto == 169).ToList())
                //                {
                //                    if (!prop.VazaoIncremental.ContainsKey(aph.dt)) prop.VazaoIncremental[aph.dt] = 0;
                //                    if (!prop.VazaoNatural.ContainsKey(aph.dt)) prop.VazaoNatural[aph.dt] = 0;

                //                    prop.VazaoIncremental[aph.dt] = aph.qInc;
                //                    prop.VazaoNatural[aph.dt] = aph.qNat;
                //                }


                //                foreach (DateTime data in listSex)
                //                {
                //                    if (data <= uSexta)
                //                    {
                //                        if (!prop.medSemanalNatural.ContainsKey(data)) prop.medSemanalNatural[data] = 0;
                //                        if (!prop.medSemanalIncremental.ContainsKey(data)) prop.medSemanalIncremental[data] = 0;

                //                        if (prop.medSemanalNatural[data] == 0) prop.medSemanalNatural[data] = prop.VazaoIncremental.Where(x => (x.Key >= data.AddDays(-6)) && x.Key <= data).Select(x => x.Value).Average();//vazao natural do post 168 é a incremental do 169 
                //                        //if (prop.medSemanalIncremental[data] == 0) prop.medSemanalIncremental[data] = 
                //                    }
                //                    else if (data == uSexta.AddDays(7))
                //                    {
                //                        var dias = data.Day - uSexta.Day - 1;
                //                        double soma = 0;
                //                        int cont = 0;
                //                        for (int d = dias; d > 0; d--)
                //                        {
                //                            var dat = data.AddDays(-d);// criar logica para calcular med semanal da semana atual!!!
                //                            if (prop.VazaoIncremental.ContainsKey(dat))
                //                            {
                //                                soma = soma + prop.VazaoIncremental[dat];
                //                                cont++;
                //                            }
                //                        }
                //                        prop.medSemanalNatural[data] = soma / cont;
                //                    }
                //                }

                //                for (int count = 4; count < 10; count++)
                //                {

                //                    try
                //                    {
                //                        DateTime dat = prop.medSemanalNatural.Last().Key.AddDays(7);
                //                        if (!prop.medSemanalNatural.ContainsKey(dat)) prop.medSemanalNatural[dat] = 0;
                //                        prop.medSemanalNatural[dat] = Convert.ToDouble(previ[count]);
                //                        // dat = dat.AddDays(7);
                //                        prop.OK = true;
                //                    }
                //                    catch (Exception ep)//TODO: tirar a Exception
                //                    {
                //                        prop.OK = false;
                //                    }
                //                }


                //                prop.OK = true;
                //            }
                //            catch (Exception ept)
                //            {
                //                prop.OK = false;
                //            }
                //            finally
                //            {
                //                Propagacoes.Add(prop);
                //            }
                //        }

                //    }
                //    else
                //    {
                //        Propagacao prop = new Propagacao();
                //        prop.IdPosto = int.Parse(pPrevi.Posto);
                //        prop.NomePostoFluv = "Posto Calculado" + prop.IdPosto;
                //        try
                //        {

                //            prop.OK = true;
                //        }
                //        catch (Exception ept)
                //        {
                //            prop.OK = false;
                //        }
                //        finally
                //        {
                //            Propagacoes.Add(prop);
                //        }
                //    }
                //}
                ////continuar aquiiii!!!

                ////Propagacoes = IncluiPostos(Propagacoes);

                //// CalcularPostRegre(Propagacoes);

                ////CalcularPostCalculados();

                ////CopiaResultados();
                ////=======fim codigo antigo=====================
                #endregion

                {
                    MemoryStream stream1 = new MemoryStream();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Propagacao>));

                    ser.WriteObject(stream1, Propagacoes);
                    stream1.Position = 0;
                    System.IO.File.WriteAllText(Path.Combine(path, "Previvaz2.txt"), new StreamReader(stream1).ReadToEnd());
                }


                //}

                //var destPath = Path.Combine(path, "arq_previvaz");

                //if (Directory.Exists(destPath))
                //{
                //    Directory.Delete(destPath, true);
                //}

                //Tools.moveDirectory(tempFolder, destPath);
            }





        }

        public static List<Propagacao> IncluiPostos(List<Propagacao> Propagacoes)
        {
            for (int i = 1; i < 321; i++)
            {
                //foreach(var p in Propagacoes.Where(x => x.IdPosto != i))//Any(x => x.IdPosto != i))
                if (Propagacoes.All(x => x.IdPosto != i))
                {
                    Propagacao prop = new Propagacao();
                    prop.IdPosto = i;
                    Propagacoes.Add(prop);
                }
            }


            Propagacoes = Propagacoes.OrderBy(x => x.IdPosto).ToList();

            return Propagacoes;

        }


        public static void CalcularPostRegre(List<Propagacao> Propagacoes, List<Tuple<DateTime, int>> SemanasPrevs)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            try
            {
                List<int> idRegre = new List<int> {002, 007, 008, 009, 010, 011, 012, 015, 016, 022, 251, 206, 207, 028, 023, 032, 248, 261,
                                           241, 118, 301, 320, 048, 049, 249, 050, 052, 051, 062, 089, 217, 094, 103, 076, 072,
                                            078, 222, 081, 252, 110, 112, 113, 114, 097, 284, 303, 123, 129, 202, 306, 203, 122, 129,
                                            198, 263, 141, 148, 183, 191, 253, 273, 155,
                                            285, 227, 228, 230, 204, 297, 55};//id de postos regredidos

                var regresA1 = System.IO.File.ReadLines("H:\\TI - Sistemas\\UAT\\PricingExcelTools\\files\\RegressoesA1.txt").ToList();
                foreach (string l in regresA1)
                {
                    Regressao reg = new Regressao();
                    reg.IdPosto = Convert.ToInt32(l.Split(' ').First());
                    var list = l.Split(' ').ToList();

                    reg.Valor_mensal = list.Select(x => double.Parse(x)).ToList();

                    RegressoesA1.Add(reg);
                }

                var regresA0 = System.IO.File.ReadLines("H:\\TI - Sistemas\\UAT\\PricingExcelTools\\files\\RegressoesA0.txt").ToList();
                foreach (string l in regresA0)
                {
                    Regressao regB = new Regressao();
                    regB.IdPosto = Convert.ToInt32(l.Split(' ').First());
                    var list = l.Split(' ').ToList();

                    regB.Valor_mensal = list.Select(x => double.Parse(x)).ToList();

                    RegressoesA0.Add(regB);
                }
                var listRegre = System.IO.File.ReadLines("H:\\TI - Sistemas\\UAT\\PricingExcelTools\\files\\Posto_regre_base.txt").ToList();
                foreach (var l in listRegre)
                {
                    PostoRegre post = new PostoRegre();
                    post.Idposto_Regredido = Convert.ToInt32(l.Split(' ').First());
                    post.IdPosto_Base = Convert.ToInt32(l.Split(' ').Last());
                    PostoRegredidos.Add(post);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            //List<DateTime> listDat = Propagacoes.First().medSemanalNatural.Select(x => x.Key).ToList();
            List<DateTime> listDat = SemanasPrevs.Select(x => x.Item1).ToList();

            foreach (var p in Propagacoes)
            {
                try
                {
                    foreach (var regred in PostoRegredidos)
                    {
                        if (regred.Idposto_Regredido == p.IdPosto)
                        {
                            try
                            {
                                foreach (var d in listDat)
                                {
                                    double[] fatorReg = ValorRegrecao(p.IdPosto, d);
                                    Calcular_Regressao(p.IdPosto, fatorReg, d);
                                }
                            }
                            catch (Exception e)
                            {
                                e.ToString();
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }


        }

        public static void Calcular_Regressao(int idPosto, double[] fator, DateTime dat)
        {

            double semRegre = 0;
            foreach (var l in PostoRegredidos.Where(x => x.Idposto_Regredido == idPosto))
            {
                var propBase = Propagacoes.Where(v => v.IdPosto == l.IdPosto_Base).First();

                if (propBase.calMedSemanal.ContainsKey(dat))
                {
                    semRegre = fator[0] + (fator[1] * propBase.calMedSemanal[dat]);  // os postos regredidos utilizam uma função linear p\ calcular as vazões (Y=A0+A1*X) 
                                                                                     //onde A0 e A1 são fatores e X é a vazão do posto base 
                                                                                     //obs: os dados de A0 A1 e quais são os postos regredidos e base, são disponibilizados pelo ONS     

                    var propRegre = Propagacoes.Where(i => i.IdPosto == l.Idposto_Regredido).First();
                    if (!propRegre.calMedSemanal.ContainsKey(dat) || propRegre.IdPosto == 203)//usa a regressão para as semanas que ainda não possuem dados
                    {
                        propRegre.calMedSemanal[dat] = semRegre;

                        break;
                    }
                }
            }
        }

        public static double[] ValorRegrecao(int id, DateTime data)
        {
            double[] fat = new double[2];
            foreach (var rb in RegressoesA0)
            {
                if (rb.IdPosto == id)
                {
                    var dataini = data.AddDays(-6);//inicio da semana
                    var datafim = data;//fim da semana
                    double media = 0;//media ponderada dos valores das regressões (a media é feita por causa das semanas com dias em meses diferentes)
                    for (DateTime dat = dataini; dat <= datafim; dat = dat.AddDays(1))
                    {
                        media = media + rb.Valor_mensal[dat.Month];
                    }
                    media = media / 7;
                    fat[0] = media;
                    break;
                }
            }

            foreach (var r in RegressoesA1)
            {
                if (r.IdPosto == id)
                {
                    var dataini = data.AddDays(-6);//inicio da semana
                    var datafim = data;//fim da semana
                    double media = 0;//media ponderada dos valores das regressões (a media é feita por causa das semanas com dias em meses diferentes)
                    for (DateTime dat = dataini; dat <= datafim; dat = dat.AddDays(1))
                    {
                        media = media + r.Valor_mensal[dat.Month];
                    }
                    media = media / 7;

                    fat[1] = media;
                    break;
                }
            }
            return fat;
        }
        public static void CopiaResultados(List<Tuple<DateTime, int>> SemanasPrevs)//Copia o ultimo dado de vazao para as semanas restantes caso ainda não haja dados
        {
            List<DateTime> listSem = SemanasPrevs.Select(x => x.Item1).ToList();

            //for (int i = 0; i < 4; i++) listSem.Add(listSem.Last().AddDays(+7));
            try
            {
                foreach (var p in Propagacoes)
                {
                    foreach (var data in listSem)
                    {
                        try
                        {
                            if (!p.calMedSemanal.ContainsKey(data))
                            {
                                p.calMedSemanal[data] = 0;
                                if (p.calMedSemanal.ContainsKey(data.AddDays(-7)))
                                {
                                    p.calMedSemanal[data] = p.calMedSemanal[data.AddDays(-7)];
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }


                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

        }

        public static void CalcularPostCalculados(List<Tuple<DateTime, int>> SemanasPrevs)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var verifica = Propagacoes;
            List<DateTime> listSem = SemanasPrevs.Select(x => x.Item1).ToList();
            foreach (var pn in postosIncrementais)//soma as vazões dos postos  montantes e incremnetais dos postos que possuem postos incrementais
            {
                var prop = Propagacoes.Where(x => x.IdPosto == pn.Key).FirstOrDefault();
                var propInc = Propagacoes.Where(x => x.IdPosto == pn.Value.Item2).FirstOrDefault();
                for (int i = 0; i < 12; i++)
                {
                    if (!prop.calMedSemanal.ContainsKey(SemanasPrevs[i].Item1))
                    {
                        if (pn.Key == 253)// sao salvador + canabrava
                        {
                            var propCana = Propagacoes.Where(x => x.IdPosto == 191).First();
                            var propSerra = Propagacoes.Where(x => x.IdPosto == 270).First();
                            double fatcana = 0.504;
                            propCana.calMedSemanal[SemanasPrevs[i].Item1] = (propInc.calMedSemanal[SemanasPrevs[i].Item1] * fatcana) + (propSerra.calMedSemanal[SemanasPrevs[i].Item1]);
                            double fatSal = 0.496;
                            prop.calMedSemanal[SemanasPrevs[i].Item1] = (propInc.calMedSemanal[SemanasPrevs[i].Item1] * fatSal) + (propCana.calMedSemanal[SemanasPrevs[i].Item1]);

                        }
                        else if (pn.Key == 273)//lajeado + peixe angical
                        {
                            var propPeixe = Propagacoes.Where(x => x.IdPosto == 257).First();
                            var propSSal = Propagacoes.Where(x => x.IdPosto == 253).First();

                            double fatpeixe = 0.488;
                            propPeixe.calMedSemanal[SemanasPrevs[i].Item1] = (propInc.calMedSemanal[SemanasPrevs[i].Item1] * fatpeixe) + (propSSal.calMedSemanal[SemanasPrevs[i].Item1]);
                            double fatlaj = 0.512;
                            prop.calMedSemanal[SemanasPrevs[i].Item1] = (propInc.calMedSemanal[SemanasPrevs[i].Item1] * fatlaj) + (propPeixe.calMedSemanal[SemanasPrevs[i].Item1]);

                        }
                        else
                        {
                            double vazMontantes = 0;
                            foreach (var p in prop.PostoMontantes)
                            {
                                var propa = Propagacoes.Where(x => x.IdPosto == p.Propaga.IdPosto).FirstOrDefault();
                                vazMontantes += propa.calMedSemanal[SemanasPrevs[i].Item1];
                            }
                            // vazMontantes = prop.PostoMontantes.SelectMany(x => x.Propaga.calMedSemanal).Where(x => x.Key == SemanasPrevs[i].Item1).Select(x => x.Value).ToList();
                            var v = propInc.calMedSemanal[SemanasPrevs[i].Item1] + vazMontantes;

                            prop.calMedSemanal[SemanasPrevs[i].Item1] = v;
                        }

                    }
                }
            }


            foreach (var d in listSem)
            {
                //Posto precedentes ( )
                foreach (var p in Propagacoes.Where(x => x.IdPosto == 240))
                {
                    var p239 = GetMediaSemanal(239, d);
                    var p242 = GetMediaSemanal(242, d);
                    if (!p.calMedSemanal.ContainsKey(d))
                    {
                        p.calMedSemanal[d] = (p242 - p239) * 0.717 + p239;

                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }
                    }


                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 238))
                {
                    var p239 = GetMediaSemanal(239, d);
                    var p237 = GetMediaSemanal(237, d);
                    if (!p.calMedSemanal.ContainsKey(d))
                    {
                        p.calMedSemanal[d] = (p239 - p237) * 0.342 + p237;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }
                    }

                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 244))
                {
                    var p34 = GetMediaSemanal(34, d);
                    var p243 = GetMediaSemanal(243, d);
                    //if (!p.calMedSemanal.ContainsKey(d))
                    //{
                    p.calMedSemanal[d] = p34 + p243;
                    //}

                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 317))
                {
                    var p201 = GetMediaSemanal(201, d);
                    var p201b = p201 - 25;
                    if (p201b > 0)
                    {
                        p.calMedSemanal[d] = p201b;
                    }
                    else
                    {
                        p.calMedSemanal[d] = 0;
                    }
                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 307))
                {
                    var p201 = GetMediaSemanal(201, d);
                    var p201b = p201 - 25;
                    if (p201b > 0)
                    {
                        p.calMedSemanal[d] = p201b;
                    }
                    else
                    {
                        p.calMedSemanal[d] = 0;
                    }
                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 298))
                {
                    var p125 = GetMediaSemanal(125, d);
                    if (p125 <= 190)
                    {
                        p.calMedSemanal[d] = (p125 * 119) / 190;
                    }
                    else if (p125 <= 209 && p125 > 190)
                    {
                        p.calMedSemanal[d] = 119;
                    }
                    else if (p125 <= 250 && p125 > 209)
                    {
                        p.calMedSemanal[d] = p125 - 90;
                    }
                    else
                    {
                        p.calMedSemanal[d] = 160;
                    }
                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 315))
                {
                    var p203 = GetMediaSemanal(203, d);
                    var p201 = GetMediaSemanal(201, d);
                    var p317 = GetMediaSemanal(317, d);
                    var p298 = GetMediaSemanal(298, d);
                    p.calMedSemanal[d] = (p203 - p201) + p317 + p298;
                    if (p.calMedSemanal[d] <= 0)
                    {
                        p.calMedSemanal[d] = 0;
                    }
                    else if (p.calMedSemanal[d] <= 1)
                    {
                        p.calMedSemanal[d] = 1;
                    }
                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 316))
                {
                    var p315 = GetMediaSemanal(315, d);
                    if (p315 < 190)
                    {
                        p.calMedSemanal[d] = p315;
                    }
                    else
                    {
                        p.calMedSemanal[d] = 190;
                    }
                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 304))
                {
                    var p315 = GetMediaSemanal(315, d);
                    var p316 = GetMediaSemanal(316, d);

                    p.calMedSemanal[d] = p315 - p316;
                    if (p.calMedSemanal[d] <= 0)
                    {
                        p.calMedSemanal[d] = 0;
                    }
                    else if (p.calMedSemanal[d] <= 1)
                    {
                        p.calMedSemanal[d] = 1;
                    }
                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 127))
                {
                    var p129 = GetMediaSemanal(129, d);
                    var p298 = GetMediaSemanal(298, d);
                    var p203 = GetMediaSemanal(203, d);
                    var p304 = GetMediaSemanal(304, d);

                    p.calMedSemanal[d] = p129 - p298 - p203 + p304;
                    if (p.calMedSemanal[d] <= 0)
                    {
                        p.calMedSemanal[d] = 0;
                    }
                    else if (p.calMedSemanal[d] <= 1)
                    {
                        p.calMedSemanal[d] = 1;
                    }
                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 126))
                {
                    var p127 = GetMediaSemanal(127, d);
                    var p127b = p127 - 90;
                    if (p127 <= 430)
                    {
                        if (p127b > 0)
                        {
                            p.calMedSemanal[d] = p127b;
                        }
                        else
                        {
                            p.calMedSemanal[d] = 0;
                        }
                    }
                    else
                    {
                        p.calMedSemanal[d] = 340;
                    }
                }

                foreach (var p in Propagacoes.Where(x => x.IdPosto == 118))
                {
                    if (p.calMedSemanal[d] > 1)
                    {
                        continue;
                    }
                    else
                    {
                        var prop = Propagacoes.Where(x => x.IdPosto == 119).First();
                        if (prop.calMedSemanal[d] > 1)
                        {
                            var valor = (prop.calMedSemanal[d] * 0.8103) + 0.185;
                            if (valor > 0)
                            {
                                p.calMedSemanal[d] = valor;
                            }
                            else
                                p.calMedSemanal[d] = 0;

                        }
                    }
                }

                foreach (var p in Propagacoes)
                {


                    if (p.IdPosto == 244)
                    {
                        var p34 = GetMediaSemanal(34, d);
                        var p243 = GetMediaSemanal(243, d);

                        p.calMedSemanal[d] = p34 + p243;
                    }

                    else if (p.IdPosto == 21)
                    {
                        var p123 = GetMediaSemanal(123, d);
                        p.calMedSemanal[d] = p123;
                    }

                    else if (p.IdPosto == 292)//BELO MONTE PIMNETAL uso do trecho de vazão reduzida (TVR)
                    {
                        var TVR = System.IO.File.ReadLines("H:\\TI - Sistemas\\UAT\\PricingExcelTools\\files\\TVR_BeloMonte.txt").ToList();
                        var TVRmeses = TVR[0].Split(' ').Skip(1).ToList();
                        var p288 = GetMediaSemanal(288, d);

                        var semRV0 = listSem.First();
                        int diasSemMes = 0;
                        var dataini = d.AddDays(-6);//inicio da semana
                        var datafim = d;//fim da semana

                        for (DateTime dat = dataini; dat <= datafim; dat = dat.AddDays(1))
                        {
                            if (dat.Month == semRV0.Month)
                            {
                                diasSemMes++;
                            }
                        }

                        //var numMes = 0;
                        double mediaTVR = 0;//media ponderada dos valores de TVR 

                        if (d == listSem.First())
                        {
                            mediaTVR = ((diasSemMes * Convert.ToDouble(TVRmeses[semRV0.Month - 1])) + (7 - diasSemMes) * Convert.ToDouble(TVRmeses[semRV0.AddMonths(-1).Month - 1])) / 7;
                        }
                        else
                        {
                            mediaTVR = ((diasSemMes * Convert.ToDouble(TVRmeses[semRV0.Month - 1])) + (7 - diasSemMes) * Convert.ToDouble(TVRmeses[semRV0.AddMonths(1).Month - 1])) / 7;
                        }
                        //for (DateTime dat = dataini; dat <= datafim; dat = dat.AddDays(1))
                        //{
                        //    if (dat.Month <= semanaOp.AddMonths(1).Month)//replica a media do mes seguinte ao atual para todas as semanas restantes
                        //    {
                        //        numMes = dat.Month;
                        //    }
                        //    else
                        //    {
                        //        numMes = semanaOp.AddMonths(1).Month;
                        //    }
                        //    mediaTVR = mediaTVR + Convert.ToDouble(TVRmeses[(numMes - 1)]);
                        //}
                        //mediaTVR = mediaTVR / 7;

                        if (p288 <= mediaTVR)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p288 <= (mediaTVR + 13900))//13900 = valor retirado do arquivo REGRAS.DAT do GEVAZP
                        {
                            p.calMedSemanal[d] = p288 - mediaTVR;
                        }
                        else
                        {
                            p.calMedSemanal[d] = 13900;
                        }
                    }

                    else if (p.IdPosto == 293)
                    {
                        var p288 = GetMediaSemanal(288, d);
                        var p292 = GetMediaSemanal(292, d);


                        p.calMedSemanal[d] = (1.07 * p288) - p292;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }
                    }

                    else if (p.IdPosto == 299)
                    {
                        var p130 = GetMediaSemanal(130, d);
                        var p298 = GetMediaSemanal(298, d);
                        var p203 = GetMediaSemanal(203, d);
                        var p304 = GetMediaSemanal(304, d);

                        p.calMedSemanal[d] = p130 - p298 - p203 + p304;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }
                    }

                    else if (p.IdPosto == 169)
                    {
                        var prop156 = Propagacoes.Where(x => x.IdPosto == 156).First();
                        var prop158 = Propagacoes.Where(x => x.IdPosto == 158).First();
                        var prop168 = Propagacoes.Where(x => x.IdPosto == 168).First();
                        var d2 = d.AddDays(-14);

                        if (!p.calMedSemanal.ContainsKey(d) && prop168.calMedSemanal.ContainsKey(d) && prop156.calMedSemanal.ContainsKey(d2) && prop158.calMedSemanal.ContainsKey(d2))
                        {
                            var p168 = GetMediaSemanal(168, d);
                            var p156 = GetMediaSemanal(156, d2);
                            var p158 = GetMediaSemanal(158, d2);
                            p.calMedSemanal[d] = p168 + p156 + p158;
                            if (p.calMedSemanal[d] <= 0)
                            {
                                p.calMedSemanal[d] = 0;
                            }
                            else if (p.calMedSemanal[d] <= 1)
                            {
                                p.calMedSemanal[d] = 1;
                            }
                        }

                    }



                    else if (p.IdPosto == 301)
                    {
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p118;
                    }

                    else if (p.IdPosto == 302)
                    {
                        var p288 = GetMediaSemanal(288, d);
                        var p292 = GetMediaSemanal(292, d);
                        p.calMedSemanal[d] = p288 - p292;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }
                    }

                    else if (p.IdPosto == 252)
                    {
                        var p259 = GetMediaSemanal(259, d);
                        p.calMedSemanal[d] = p259;
                    }

                    else if (p.IdPosto == 172)
                    {
                        var p169 = GetMediaSemanal(169, d);
                        if (!p.calMedSemanal.ContainsKey(d))
                        {
                            p.calMedSemanal[d] = p169;
                        }
                    }

                    else if (p.IdPosto == 173)
                    {
                        var p172 = GetMediaSemanal(172, d);
                        p.calMedSemanal[d] = p172;
                    }

                    else if (p.IdPosto == 175)
                    {
                        var p172 = GetMediaSemanal(172, d);
                        if (!p.calMedSemanal.ContainsKey(d))
                        {
                            p.calMedSemanal[d] = p172;
                        }
                    }

                    else if (p.IdPosto == 178)
                    {
                        var p172 = GetMediaSemanal(172, d);
                        if (!p.calMedSemanal.ContainsKey(d))
                        {
                            p.calMedSemanal[d] = p172;
                        }
                    }

                    else if (p.IdPosto == 176)
                    {
                        var p173 = GetMediaSemanal(173, d);
                        p.calMedSemanal[d] = p173;
                    }

                    else if (p.IdPosto == 164)
                    {
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);

                        p.calMedSemanal[d] = p161 - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }
                    }

                    else if (p.IdPosto == 314)
                    {
                        var p199 = GetMediaSemanal(199, d);
                        var p298 = GetMediaSemanal(298, d);
                        var p203 = GetMediaSemanal(203, d);
                        var p304 = GetMediaSemanal(304, d);

                        p.calMedSemanal[d] = p199 - p298 - p203 + p304;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 104)
                    {
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p117 + p118;
                    }

                    else if (p.IdPosto == 132)
                    {
                        var p202 = GetMediaSemanal(202, d);
                        var p201 = GetMediaSemanal(201, d);

                        if (p201 < 25)
                        {
                            p.calMedSemanal[d] = p202 + p201;
                        }
                        else
                        {
                            p.calMedSemanal[d] = p202 + 25;
                        }
                    }



                    else if (p.IdPosto == 131)
                    {
                        var p316 = GetMediaSemanal(316, d);

                        if (p316 < 144)
                        {
                            p.calMedSemanal[d] = p316;
                        }
                        else
                        {
                            p.calMedSemanal[d] = 144;
                        }
                    }

                    else if (p.IdPosto == 303)
                    {
                        var p132 = GetMediaSemanal(132, d);
                        var p316 = GetMediaSemanal(316, d);
                        var p131 = GetMediaSemanal(131, d);
                        var aux = p316 - p131;
                        if (p132 < 17)
                        {
                            if (aux < 34)
                            {
                                p.calMedSemanal[d] = p132 + aux;
                            }
                            else
                            {
                                p.calMedSemanal[d] = p132 + 34;
                            }
                        }
                        else
                        {
                            if (aux < 34)
                            {
                                p.calMedSemanal[d] = 17 + aux;
                            }
                            else
                            {
                                p.calMedSemanal[d] = 17 + 34;
                            }
                        }
                    }

                    else if (p.IdPosto == 306)
                    {
                        var p303 = GetMediaSemanal(303, d);
                        var p131 = GetMediaSemanal(131, d);

                        p.calMedSemanal[d] = p303 + p131;
                    }

                    else if (p.IdPosto == 109)
                    {
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p118;
                    }

                    else if (p.IdPosto == 116)
                    {
                        var p119 = GetMediaSemanal(119, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p119 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }
                    }

                    else if (p.IdPosto == 70)
                    {
                        var p73 = GetMediaSemanal(73, d);
                        if (p73 > 0)
                        {
                            var p73b = p73 - 10;
                            if (p73b <= 173.5)
                            {
                                p.calMedSemanal[d] = p73 - p73b;
                            }
                            else
                            {
                                p.calMedSemanal[d] = p73 - 173.5;
                            }
                        }
                        else
                        {
                            p.calMedSemanal[d] = 0;
                        }
                    }

                    else if (p.IdPosto == 75)
                    {
                        var p76 = GetMediaSemanal(76, d);
                        if (p76 > 0)
                        {
                            var p73 = GetMediaSemanal(73, d) - 10;
                            if (p73 <= 173.5)
                            {
                                p.calMedSemanal[d] = p76 + p73;
                            }
                            else
                            {
                                p.calMedSemanal[d] = p76 + 173.5;
                            }
                        }
                        else
                        {
                            p.calMedSemanal[d] = 0;
                        }

                    }

                    else if (p.IdPosto == 37)
                    {
                        var p237 = GetMediaSemanal(237, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p237 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 38)
                    {
                        var p238 = GetMediaSemanal(238, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p238 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 318)
                    {
                        var p116 = GetMediaSemanal(116, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p116 + 0.1 * (p161 - p117 - p118) + p117 + p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 319)
                    {
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        var p161 = GetMediaSemanal(161, d);


                        p.calMedSemanal[d] = p117 + p118 + 0.1 * (p161 - p117 - p118);
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 320)
                    {
                        var p119 = GetMediaSemanal(119, d);

                        p.calMedSemanal[d] = p119;

                    }

                    else if (p.IdPosto == 39)
                    {
                        var p239 = GetMediaSemanal(239, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p239 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 40)
                    {
                        var p240 = GetMediaSemanal(240, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p240 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 42)
                    {
                        var p242 = GetMediaSemanal(242, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p242 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 43)
                    {
                        var p243 = GetMediaSemanal(243, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p243 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 44)
                    {
                        var p244 = GetMediaSemanal(244, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p244 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 45)
                    {
                        var p245 = GetMediaSemanal(245, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p245 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 46)
                    {
                        var p246 = GetMediaSemanal(246, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p246 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }

                    else if (p.IdPosto == 66)
                    {
                        var p266 = GetMediaSemanal(266, d);
                        var p161 = GetMediaSemanal(161, d);
                        var p117 = GetMediaSemanal(117, d);
                        var p118 = GetMediaSemanal(118, d);
                        p.calMedSemanal[d] = p266 - 0.1 * (p161 - p117 - p118) - p117 - p118;
                        if (p.calMedSemanal[d] <= 0)
                        {
                            p.calMedSemanal[d] = 0;
                        }
                        else if (p.calMedSemanal[d] <= 1)
                        {
                            p.calMedSemanal[d] = 1;
                        }

                    }
                }

            }



        }

        public static double GetMediaSemanal(int codigoPost, DateTime data)
        {
            try
            {
                double valor = 0;
                var prop = Propagacoes.Where(x => x.IdPosto == codigoPost).First();
                if (prop.calMedSemanal.ContainsKey(data))
                {
                    valor = prop.calMedSemanal[data];
                }
                else
                {
                    valor = 0;
                }

                return valor;
            }
            catch (Exception e)

            {
                e.ToString();
            }
            return 0;
        }

    }
}

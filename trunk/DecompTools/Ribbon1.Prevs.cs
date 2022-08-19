using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.SistemaDat;
using Compass.ExcelTools;
using DecompTools.Properties;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Compass.CommomLibrary.HidrDat;
using Compass.ExcelTools.Templates;

namespace Compass.DecompTools
{
    public partial class Ribbon1
    {

        private void btnPrevsCenariosNovo_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {
                var wb = openTemplate();
                var wbCenario = new WorkbookPrevsCenarios(wb);

                if (System.Windows.Forms.MessageBox.Show("Selecionar o deck base?", "Deck Base", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog()
                    {
                        Filter = "Dadger | dadger.*"
                    };

                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                        wbCenario.DeckEntrada = Path.GetDirectoryName(ofd.FileName);


                        Compass.CommomLibrary.Decomp.Deck deck = new CommomLibrary.Decomp.Deck();
                        deck.GetFiles(wbCenario.DeckEntrada);

                        var doc = (Dadger)DocumentFactory.Create(ofd.FileName);
                        var hidr = (Compass.CommomLibrary.HidrDat.HidrDat)DocumentFactory.Create(deck.Documents["HIDR.DAT"].BasePath);


                        //Buscar dados de vazoes
                        var comm = doc.BottonComments;
                        int mes;
                        int ano;

                        int iM = comm.IndexOf("MES INICIAL DO ESTUDO             =>");
                        if (iM >= 0 && int.TryParse(comm.Substring(iM + 37, 2), out mes))
                            wbCenario.Mes = mes;
                        int iA = comm.IndexOf("ANO INICIAL DO ESTUDO             =>");
                        if (iA >= 0 && int.TryParse(comm.Substring(iA + 37, 4), out ano))
                            wbCenario.Ano = ano;

                        wbCenario.Rev = doc.BlocoEs.NumSemanasPassadas;







                        var confH = new Compass.CommomLibrary.Decomp.ConfigH(doc, hidr);

                        var wsProd = wb.GetOrCreateWorksheet("UsinasProd", true);

                        var uhes = confH.Usinas.Where(x => !x.IsFict && (x.InDadger || x.InJusEna)).ToArray();

                        wsProd.Cells[1, 1].Value = "Cod";
                        wsProd.Cells[1, 2].Value = "Usina";
                        wsProd.Cells[1, 3].Value = "Posto";
                        wsProd.Cells[1, 4].Value = "Prod65%";

                        for (int i = 0; i < uhes.Length; i++)
                        {

                            wsProd.Cells[2 + i, 1].Value = uhes[i].Cod;
                            wsProd.Cells[2 + i, 2].Value = uhes[i].Usina;
                            wsProd.Cells[2 + i, 3].Value = uhes[i].Posto;
                            wsProd.Cells[2 + i, 4].Value = uhes[i].Prod65VolUtil;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnPrevsCenariosNovoMensal_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {
                var wb = openTemplateMensal();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnCriarDecksSensibilidade_Click(object sender, RibbonControlEventArgs e)
        {
            var tasks = Globals.ThisAddIn.tasks;
            object rlock = new object();
            object rlock2 = new object();
            object rlock3 = new object();

            try
            {

                using (DecompTools.Forms.FormPrevsDecksSensibilidade frm = new DecompTools.Forms.FormPrevsDecksSensibilidade())
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                        var deckBase = DeckFactory.CreateDeck(frm.DeckBase) as Compass.CommomLibrary.Decomp.Deck;
                        deckBase[CommomLibrary.Decomp.DeckDocument.prevs] = null;
                        deckBase[CommomLibrary.Decomp.DeckDocument.vazoes] = null;

                        var pastas = Directory.GetDirectories(frm.PastaSensibilidades);


                        //bool atualizaEA = false;
                        bool deckMensal = false;

                        if ((deckBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger).VAZOES_NumeroDeSemanas == 0)
                        {
                            if (System.Windows.Forms.MessageBox.Show("Identificado decomp MENSAL", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information)
                                == System.Windows.Forms.DialogResult.Cancel)
                                return;
                            else deckMensal = true;
                        }

                        int running = 0;
                        foreach (var pasta in pastas)
                        {

                            deckBase.CopyFilesToFolder(pasta);

                            var ndeck = DeckFactory.CreateDeck(pasta) as Compass.CommomLibrary.Decomp.Deck;

                            //read prevs in worksheet
                            var prevs = ndeck[CommomLibrary.Decomp.DeckDocument.prevs].BasePath;


                            var prevsSem = ndeck[CommomLibrary.Decomp.DeckDocument.prevs].BasePath;
                            if (deckMensal)
                            {


                                var prevsMen = prevsSem.ToLower().Replace("prevs.rv0", "prevs_men.rv0");

                                if (File.Exists(prevsMen))
                                {
                                    ndeck[CommomLibrary.Decomp.DeckDocument.prevs].BackUp();
                                    File.Delete(prevsSem);
                                    File.Copy(prevsMen, prevsSem);
                                }
                            }

                            //save dadger
                            ndeck[CommomLibrary.Decomp.DeckDocument.dadger].Document.SaveToFile();

                            //run vazoes if selected
                            if (frm.RodarVazoes)
                            {

                                var t = new System.Threading.Tasks.Task(() =>
                                {
                                    /****/
                                    lock (rlock)
                                    {
                                        while (running >= 3)
                                        {
                                            System.Threading.Thread.Sleep(1000);
                                        }
                                        lock (rlock3) running++;

                                    }

                                    var svc = new Compass.Services.Vazoes();
                                    svc.Run(pasta, true);
                                    /****/
                                });

                                t.ContinueWith(tr =>
                                {
                                    lock (rlock3) running--;

                                });
                                t.Start();
                            }

                            //clear arq_entrada is selected
                            if (frm.ExcluirArquivosPrevivaz && Directory.Exists(Path.Combine(pasta, "arq_previvaz")))
                            {
                                Directory.Delete(Path.Combine(pasta, "arq_previvaz"), true);
                            }
                        }

                        var tfin = new System.Threading.Tasks.Task(() =>
                        {

                            while (true)
                            {
                                if (running > 0)
                                {
                                    System.Threading.Thread.Sleep(2000);
                                }
                                else
                                {
                                    System.Threading.Thread.Sleep(500);
                                    if (running == 0) break;
                                }
                            }

                            System.Windows.Forms.MessageBox.Show("Finalizado");

                        });
                        tfin.Start();
                    }
                }
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnPrevsCenariosProcess_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {

                var wbCen = Globals.ThisAddIn.Application.ActiveWorkbook;
                var cenarios = wbCen.ToWorkbookPrevsCenarios();

                if (cenarios == null ||
                    string.IsNullOrWhiteSpace(cenarios.DeckEntrada) ||
                    string.IsNullOrWhiteSpace(cenarios.DiretorioSaida)
                    )
                {
                    System.Windows.Forms.MessageBox.Show("Forneça os caminhos do deck de entrada e diretório de saída"
                        , "Decomp Tools", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                Compass.CommomLibrary.Decomp.Deck deck = new CommomLibrary.Decomp.Deck();
                deck.GetFiles(cenarios.DeckEntrada);


                var atualizaEs = cenarios.Rev == 0 ? false : (
                    System.Windows.Forms.DialogResult.Yes ==
                    System.Windows.Forms.MessageBox.Show("Atualizar Bloco ES?",
                    "Decomp Tools",
                    System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question));


                var enas = cenarios.Enas;

                foreach (var cenario in cenarios.Cenarios)
                {
                    Compass.Services.Vazoes vazService = new Services.Vazoes();

                    var pastaSaida = Path.Combine(cenarios.DiretorioSaida, cenario.Key);

                    if (!Directory.Exists(pastaSaida))
                        Directory.CreateDirectory(pastaSaida);

                    deck.CopyFilesToFolder(pastaSaida);

                    cenario.Value.File = Path.Combine(pastaSaida, "PREVS." + deck.Caso);
                    cenario.Value.SaveToFile();


                    if (atualizaEs)
                    {

                        var dgerPath = Path.Combine(pastaSaida, deck.Documents["DADGER."].FileName);

                        var dger = (Dadger)DocumentFactory.Create(dgerPath);


                        var s = 1;
                        foreach (var item in dger.BlocoEs)
                        {
                            item.UltimaEna = Math.Round((double)enas[cenario.Key][s, cenarios.Rev], 0);
                            s++;
                        }

                        dger.SaveToFile();
                    }
                }

                if (System.Windows.Forms.MessageBox.Show("Executar Vaz.Bat?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {

                    System.Threading.Tasks.Parallel.ForEach(cenarios.Cenarios,
                         new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = 3 },
                       cenario =>
                       {
                           Compass.Services.Vazoes vazService = new Services.Vazoes();
                           try
                           {
                               var pastaSaida = Path.Combine(cenarios.DiretorioSaida, cenario.Key);
                               vazService.Run(pastaSaida, true);
                           }
                           finally
                           {
                               vazService.ClearTempFolder();
                           }
                       });
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnCalcularENA_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {


                var info = ActiveWorkbook.GetInfosheet();

                var fileName = info.DocPath;

                //Compass.CommomLibrary.Decomp.Deck deck = new CommomLibrary.Decomp.Deck();

                var dk = Compass.CommomLibrary.DeckFactory.CreateDeck(Path.GetDirectoryName(fileName));
                //deck.GetFiles(Path.GetDirectoryName(fileName));

                if (dk is CommomLibrary.Decomp.Deck deck)
                {

                    var dadger = CommomLibrary.DocumentFactory.Create(fileName) as Dadger;

                    var prev = CommomLibrary.DocumentFactory.Create(System.IO.Path.Combine(deck.BaseFolder, dadger.VAZOES_ArquivoPrevs)) as CommomLibrary.Prevs.Prevs;

                    var modif = deck[CommomLibrary.Decomp.DeckDocument.modif].Document as CommomLibrary.ModifDat.ModifDat;

                    //if (deck.Documents["PREVS."] == null)
                    //{
                    //    System.Windows.Forms.MessageBox.Show("Prevs não encontrado.");
                    //    return;
                    //}

                    //Compass.CommomLibrary.ModifDat.ModifDat modif = null;
                    //if (deck.Documents["MODIF.DAT"] == null)
                    //{
                    //    System.Windows.Forms.MessageBox.Show("Modif não encontrado. Pode haver divergencia no resultado.");
                    //}
                    //else
                    //{
                    //    modif = (Compass.CommomLibrary.ModifDat.ModifDat)DocumentFactory.Create(deck.Documents["MODIF.DAT"].BasePath);
                    //}

                    //prevs


                    var confH = LoadConfigH();

                    //var ena = confH.GetEna((Compass.CommomLibrary.Prevs.Prevs)prev);
                    var ena = confH.GetEnaAcopl(prev, modif);

                    info.Show();


                    var temp = confH.index_sistemas.Select(s =>
                    {
                        var enaS = ena.Where(x => x.Key.Mercado == s.Item2);
                        return new float[] {
                        enaS.Sum(x=>x.Value[0]),
                        enaS.Sum(x=>x.Value[1]),
                        enaS.Sum(x=>x.Value[2]),
                        enaS.Sum(x=>x.Value[3]),
                        enaS.Sum(x=>x.Value[4]),
                        enaS.Sum(x=>x.Value[5]),
                        };
                    }).ToArray();

                    info.Sistemas = confH.index_sistemas.Select(x => x.Item2.ToString()).ToArray();
                    info.Ena = temp;



                    //print memoria de calculo
                    var xlsM = ActiveWorkbook.GetOrCreateWorksheet("memCal - ENA");
                    xlsM.UsedRange.Clear();


                    xlsM.Range[xlsM.Cells[1, 1], xlsM.Cells[1, 12]].Value2 = new dynamic[,] {
                    {"Cod", "Usina", "Posto", "Prod65", "Prod65Total", "Sistema", "ENA_sem1", "ENA_sem2", "ENA_sem3", "ENA_sem4", "ENA_sem5", "ENA_sem6"}
                };

                    var l = 2;

                    foreach (var uhe in ena.OrderBy(u => u.Key.Cod).Select(u => new dynamic[,]{
                    {u.Key.Cod, u.Key.Usina, u.Key.Posto, u.Key.Prod65VolUtil, u.Key.ProdTotal65VolUtil, u.Key.Mercado, u.Value[0], u.Value[1], u.Value[2], u.Value[3], u.Value[4], u.Value[5]}
                }
                        ))
                    {
                        xlsM.Range[xlsM.Cells[l, 1], xlsM.Cells[l++, uhe.Length]].Value2 = uhe;
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnVazoes_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderSelect = new System.Windows.Forms.FolderBrowserDialog();
            folderSelect.Description = "Selecione o deck";
            var r1 = folderSelect.ShowDialog();
            var baseFolder = folderSelect.SelectedPath;

            if (r1 == System.Windows.Forms.DialogResult.OK)
            {

                Globals.ThisAddIn.Application.StatusBar = "Running VAZ.BAT";

                var gevazp = new Compass.Services.Vazoes();

                try
                {

                    var files = gevazp.Run(baseFolder);

                    var xlApp = Globals.ThisAddIn.Application;
                    var xlWb = xlApp.Workbooks.Add();
                    var xlWs = xlWb.GetOrCreateWorksheet("Vazoes");

                    dynamic top = Type.Missing;
                    foreach (var file in files)
                    {
                        var ole = xlWs.OLEObjects().Add(Filename: file.FullPath, Link: false, DisplayAsIcon: true, Top: top);

                        top = ole.Top + 40;
                    }

                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {

                    Globals.ThisAddIn.Application.StatusBar = null;
                    if (
                    System.Windows.Forms.MessageBox.Show(
                    "Visualizar arquivos interemediários?\r\nCaso negativo serão exluídos automaticamente."
                    , ""
                    , System.Windows.Forms.MessageBoxButtons.YesNo
                    , System.Windows.Forms.MessageBoxIcon.Question
                    )
                        == System.Windows.Forms.DialogResult.Yes
                        )
                    {

                        gevazp.OpenTempFolder();

                    }
                    else
                    {

                        gevazp.ClearTempFolder();
                    }
                }
            }
        }

        private void btnTendHidr_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var frm = new Forms.FormCropVazoesC();

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    var dataRef = frm.Inicio;
                    var vazFile = System.IO.Path.Combine(Globals.ThisAddIn.ResourcesPath, "vazoesc.dat");
                    var vazoesc = DocumentFactory.Create(vazFile) as Compass.CommomLibrary.VazoesC.VazoesC;


                    var vazRef = from vaz in vazoesc.Conteudo
                                 let data = new DateTime(vaz.Ano, vaz.Mes, 1)
                                 where data >= dataRef && vaz.Ano == dataRef.Year
                                 orderby vaz.Posto
                                 group vaz by vaz.Posto;


                    var result = String.Join("\r\n",
                            vazRef.Select(x => String.Join("\t", String.Join("\t", x.OrderBy(y => y.Mes).Select(y => y.Vazao.ToString("0")))))
                        );

                    System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, result);


                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnPrevivazLocal_Click(object sender, RibbonControlEventArgs e)
        {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;

            try
            {

                Globals.ThisAddIn.Application.DisplayStatusBar = true;
                Globals.ThisAddIn.Application.StatusBar = "Preparando Entradas...";

                var wb = new WorkbookPrevsCenariosMen(this.ActiveWorkbook);

                var res = System.Windows.Forms.MessageBox.Show("Usar Acomph?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
                var useAcomph = res == System.Windows.Forms.DialogResult.Yes;
                if (System.Windows.Forms.MessageBox.Show("Executar Previvaz localmente?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question)
                    != System.Windows.Forms.DialogResult.Yes)
                    return;
                if (System.Windows.Forms.MessageBox.Show("Encadear Previvaz local?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    Services.Previvaz.RunCenarioLocal(wb, useAcomph, true);
                }
                else
                {
                    Services.Previvaz.RunCenarioLocal(wb, useAcomph, false);
                }



                Globals.ThisAddIn.Application.ScreenUpdating = false;

                System.Windows.Forms.MessageBox.Show("Finalizado");
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;


            }
            catch (Exception ex)
            {
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnPrevivazEncad_Click(object sender, RibbonControlEventArgs e)
        {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;

            try
            {

                Globals.ThisAddIn.Application.DisplayStatusBar = true;
                Globals.ThisAddIn.Application.StatusBar = "Preparando Entradas...";

                var wb = new WorkbookPrevsCenariosMen(this.ActiveWorkbook);

                var res = System.Windows.Forms.MessageBox.Show("Usar Acomph?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
                var useAcomph = res == System.Windows.Forms.DialogResult.Yes;
                if (System.Windows.Forms.MessageBox.Show("Executar Previvaz?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question)
                    != System.Windows.Forms.DialogResult.Yes)
                    return;

                Services.Previvaz.RunCenario(wb, useAcomph, true);

                Globals.ThisAddIn.Application.ScreenUpdating = false;

                System.Windows.Forms.MessageBox.Show("Finalizado");
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;


            }
            catch (Exception ex)
            {
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }


        }

        private void btnExpPevsM2_Click(object sender, RibbonControlEventArgs e)
        {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;

            try
            {

                Globals.ThisAddIn.Application.DisplayStatusBar = true;
                Globals.ThisAddIn.Application.StatusBar = "Preparando Entradas...";

                var wb = new WorkbookPrevsCenariosMen(this.ActiveWorkbook);

                if (System.Windows.Forms.MessageBox.Show("Exportar os Prevs?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question)
                    != System.Windows.Forms.DialogResult.Yes)
                    return;
                var raiz = wb.Path;
                var cenario = "Manual";
              

                Services.Previvaz.ExportaPrevsM2(wb, raiz, cenario);


                System.Windows.Forms.MessageBox.Show("Processo concuído com sucesso!!!");

                Globals.ThisAddIn.Application.ScreenUpdating = false;

                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }

        }
        private void btnPrevivazM2_Click(object sender, RibbonControlEventArgs e)
        {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;

            try
            {
                var tfile = "";
                WorkbookPrevsM2 w;

                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                   !WorkbookPrevsM2.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
                {

                    tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "CenariosPrevs_m2.xltm");
                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                    return;
                }
               // var testa = System.Windows.Forms.MessageBox.Show("sem excel", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes;
                if (w.PlanBase.ToLower().EndsWith(".log"))
                {
                    semExcelPrevivazM2_Click();
                }
                else
                {
                    //C:\Development\Implementacoes\PrevsM2\CenariosPrevs_m2.xlsx
                    //C:\Development\Implementacoes\uhzerandoMeta\2023_Mensal_oficial_seco_modelo-2022_0203.xlsm
                    //tfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "Gera_e_Avalia_Cenarios.xltm");
                    //tfile = Path.Combine(@"C:\Development\Implementacoes\PrevsM2\CenariosPrevs_m2.xlsx");
                    //Directory.CreateDirectory(Path.GetDirectoryName(tfile));

                    //File.WriteAllBytes(tfile, t1);

                    //var xlApp = Globals.ThisAddIn.Application;

                    //var wb = xlApp.Workbooks.Add(tfile);

                    //var PlanMeta = new WorkbookPrevsM2(wb);

                    var entrada = w.Entrada;
                    var cenarios = w.Cenarios;
                    var metas = w.Metas;
                    var estudo = w.EstudoPath;
                    var planBase = w.PlanBase;

                    string excelBase = planBase.Split('\\').Last().Split('.').First() + "_dummy." + planBase.Split('\\').Last().Split('.').Last();
                    string baseCam = Path.Combine(estudo, excelBase);

                    if (File.Exists(planBase))
                    {
                        var res = System.Windows.Forms.MessageBox.Show("Usar Acomph?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
                        var useAcomph = res == System.Windows.Forms.DialogResult.Yes;


                        if (!Directory.Exists(estudo))
                        {
                            Directory.CreateDirectory(estudo);
                        }

                        File.Copy(planBase, baseCam, true);

                        var ws = w.worksheet;
                        var row = w.ROW;
                        var col = w.COL;
                        List<Tuple<string, object[,], int>> runs = new List<Tuple<string, object[,], int>>();

                        for (int i = 1; i <= cenarios.Length; i++)
                        {
                            int rowMeta = row + i - 1;
                            if (cenarios[i, 1] != null)
                            {
                                object[,] matriz = ws.Range[ws.Cells[rowMeta, col], ws.Cells[rowMeta + 3, col + 11]].Value;

                                int maxIndex = 0;
                                for (int x = 1; x <= 4; x++)
                                {
                                    for (int y = 1; y <= 12; y++)
                                    {
                                        if (matriz[x, y] != null)
                                        {
                                            if (maxIndex < y)
                                            {
                                                maxIndex = y;
                                            }
                                        }
                                    }
                                }

                                runs.Add(new Tuple<string, object[,], int>((string)cenarios[i, 1], matriz, maxIndex));
                            }
                        }

                        foreach (var run in runs)
                        {
                            string previvazFolder = Path.Combine(estudo, "arq_previvaz_" + run.Item1);
                            Services.Previvaz.RunCenarioPrevsM2(baseCam, useAcomph, run, previvazFolder, estudo, run.Item1, true);
                        }
                        //ws.Range[ws.Cells[r, col], ws.Cells[r, col + 9]]
                        //var teste = ws.Range[ws.Cells[row, col], ws.Cells[row + 3, col + 11]].Value;
                        //ws.Range[ws.Cells[15, 1], ws.Cells[18, 12]].Value = teste;
                        System.Windows.Forms.MessageBox.Show("Processo concuído com sucesso!!!");

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Planilha Base Não Existente!!!");

                    }

                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void semExcelPrevivazM2_Click()
        {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;

            try
            {
                var tfile = "";
                WorkbookPrevsM2 w;

                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                   !WorkbookPrevsM2.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
                {

                    tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "CenariosPrevs_m2.xltm");
                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                    return;
                }
               
                //C:\Development\Implementacoes\PrevsM2\CenariosPrevs_m2.xlsx
                //C:\Development\Implementacoes\uhzerandoMeta\2023_Mensal_oficial_seco_modelo-2022_0203.xlsm
                //tfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "Gera_e_Avalia_Cenarios.xltm");
                //tfile = Path.Combine(@"C:\Development\Implementacoes\PrevsM2\CenariosPrevs_m2.xlsx");
                //Directory.CreateDirectory(Path.GetDirectoryName(tfile));

                //File.WriteAllBytes(tfile, t1);

                //var xlApp = Globals.ThisAddIn.Application;

                //var wb = xlApp.Workbooks.Add(tfile);

                //var PlanMeta = new WorkbookPrevsM2(wb);

                var entrada = w.Entrada;
                var cenarios = w.Cenarios;
                var metas = w.Metas;
                var estudo = w.EstudoPath;
                var planBase = w.PlanBase;

                // string excelBase = planBase.Split('\\').Last().Split('.').First() + "_dummy." + planBase.Split('\\').Last().Split('.').Last();
                string excelBase = planBase.Split('\\').Last();
                string arqLog = Path.Combine(estudo, excelBase);

                if (File.Exists(planBase))
                {
                    var res = System.Windows.Forms.MessageBox.Show("Usar Acomph?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
                    var useAcomph = res == System.Windows.Forms.DialogResult.Yes;


                    if (!Directory.Exists(estudo))
                    {
                        Directory.CreateDirectory(estudo);
                    }

                    File.Copy(planBase, arqLog, true);

                    string arqConfig = Path.Combine(Path.GetDirectoryName(planBase), "configPrevsM2.txt");

                    var ws = w.worksheet;
                    var row = w.ROW;
                    var col = w.COL;
                    List<Tuple<string, object[,], int>> runs = new List<Tuple<string, object[,], int>>();

                    for (int i = 1; i <= cenarios.Length; i++)
                    {
                        int rowMeta = row + i - 1;
                        if (cenarios[i, 1] != null)
                        {
                            object[,] matriz = ws.Range[ws.Cells[rowMeta, col], ws.Cells[rowMeta + 3, col + 11]].Value;

                            int maxIndex = 0;
                            for (int x = 1; x <= 4; x++)
                            {
                                for (int y = 1; y <= 12; y++)
                                {
                                    if (matriz[x, y] != null)
                                    {
                                        if (maxIndex < y)
                                        {
                                            maxIndex = y;
                                        }
                                    }
                                }
                            }

                            runs.Add(new Tuple<string, object[,], int>((string)cenarios[i, 1], matriz, maxIndex));
                        }
                    }
                    string PlanModelo = $@"H:\TI - Sistemas\UAT\PricingExcelTools\files\Gera_e_Avalia_Cenarios_Men_Sem_Log.xltm";

                    //var sufixo = $"_{DateTime.Now:HHmmss}";
                    var nome= Path.Combine(
                        Path.GetDirectoryName(arqLog),
                         Path.GetFileNameWithoutExtension(PlanModelo) + ".xlsm"
                        //Path.GetFileName(caminhoWbCenario).Replace("_Log", sufixo)
                        );
                    if (System.IO.File.Exists(nome))
                    {
                        System.IO.File.Delete(nome);
                    }
                   

                    foreach (var run in runs)
                    {
                        string previvazFolder = Path.Combine(estudo, "arq_previvaz_" + run.Item1);
                        Services.Previvaz.RunCenarioPrevsM2SemExcel(arqLog, arqConfig, PlanModelo, useAcomph, run, previvazFolder, estudo, run.Item1, true);
                    }
                    //ws.Range[ws.Cells[r, col], ws.Cells[r, col + 9]]
                    //var teste = ws.Range[ws.Cells[row, col], ws.Cells[row + 3, col + 11]].Value;
                    //ws.Range[ws.Cells[15, 1], ws.Cells[18, 12]].Value = teste;
                    System.Windows.Forms.MessageBox.Show("Processo concuído com sucesso!!!");

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Arquivo LOG Não Existente!!!");

                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }
        private void btnPrevivaz_Click(object sender, RibbonControlEventArgs e)
        {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;

            try
            {

                Globals.ThisAddIn.Application.DisplayStatusBar = true;
                Globals.ThisAddIn.Application.StatusBar = "Preparando Entradas...";

                var wb = new WorkbookPrevsCenariosMen(this.ActiveWorkbook);

                var res = System.Windows.Forms.MessageBox.Show("Usar Acomph?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
                var useAcomph = res == System.Windows.Forms.DialogResult.Yes;
                if (System.Windows.Forms.MessageBox.Show("Executar Previvaz?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question)
                    != System.Windows.Forms.DialogResult.Yes)
                    return;

                Services.Previvaz.RunCenario(wb, useAcomph, false);

                Globals.ThisAddIn.Application.ScreenUpdating = false;

                System.Windows.Forms.MessageBox.Show("Finalizado");
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;


            }
            catch (Exception ex)
            {
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private static System.Windows.Forms.DialogResult OpenAcomph(out WorkbookAcomph acompH)
        {
            var res = System.Windows.Forms.MessageBox.Show("Usar Acomph?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "acomph | *.xls";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Globals.ThisAddIn.Application.ScreenUpdating = false;

                    var xlWbRdh = Globals.ThisAddIn.Application.Workbooks.Open(ofd.FileName, ReadOnly: true, UpdateLinks: false);

                    acompH = new WorkbookAcomph(xlWbRdh);

                    xlWbRdh.Close(SaveChanges: Microsoft.Office.Interop.Excel.XlSaveAction.xlDoNotSaveChanges);
                    Globals.ThisAddIn.Application.ScreenUpdating = true;
                }
                else
                {
                    res = OpenAcomph(out acompH);
                }

            }
            else acompH = null;

            return res;
        }

        private static System.Windows.Forms.DialogResult OpenRdh(out WorkbookRdh rdh)
        {

            var res = System.Windows.Forms.MessageBox.Show("Usar RDH?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);

            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "rdh | *.xls*";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Globals.ThisAddIn.Application.ScreenUpdating = false;

                    var xlWbRdh = Globals.ThisAddIn.Application.Workbooks.Open(ofd.FileName, ReadOnly: true, UpdateLinks: false);

                    rdh = new WorkbookRdh(xlWbRdh);

                    xlWbRdh.Close();
                    Globals.ThisAddIn.Application.ScreenUpdating = true;
                }
                else
                {
                    res = OpenRdh(out rdh);
                }
            }
            else rdh = null;

            return res;
        }

        Workbook openTemplate()
        {
            var tfile = "";
            try
            {

                //tfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "Gera_e_Avalia_Cenarios.xltm");
                tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Gera_e_Avalia_Cenarios_v2.xltm");
                //Directory.CreateDirectory(Path.GetDirectoryName(tfile));

                //File.WriteAllBytes(tfile, t1);

                var xlApp = Globals.ThisAddIn.Application;

                var wb = xlApp.Workbooks.Add(tfile);

                return wb;

            }
            finally
            {
                //if (Directory.Exists(Path.GetDirectoryName(tfile))) Directory.Delete(Path.GetDirectoryName(tfile), true);
            }
        }

        Workbook openTemplateMensal()
        {
            var tfile = "";
            try
            {


                tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Gera_e_Avalia_Cenarios_Men_Sem.xltm");
                //Directory.CreateDirectory(Path.GetDirectoryName(tfile));

                //File.WriteAllBytes(tfile, t1);

                var xlApp = Globals.ThisAddIn.Application;

                var wb = xlApp.Workbooks.Add(tfile);

                return wb;

            }
            finally
            {
            }
        }
    }


}

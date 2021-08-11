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

namespace Compass.DecompTools {
    public partial class Ribbon1 {

        private void btnPrevsCenariosNovo_Click(object sender, RibbonControlEventArgs e) {

            try {
                var wb = openTemplate();
                var wbCenario = new WorkbookPrevsCenarios(wb);

                if (System.Windows.Forms.MessageBox.Show("Selecionar o deck base?", "Deck Base", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {
                    System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog() {
                        Filter = "Dadger | dadger.*"
                    };

                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

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

                        for (int i = 0; i < uhes.Length; i++) {

                            wsProd.Cells[2 + i, 1].Value = uhes[i].Cod;
                            wsProd.Cells[2 + i, 2].Value = uhes[i].Usina;
                            wsProd.Cells[2 + i, 3].Value = uhes[i].Posto;
                            wsProd.Cells[2 + i, 4].Value = uhes[i].Prod65VolUtil;
                        }
                    }
                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnPrevsCenariosNovoMensal_Click(object sender, RibbonControlEventArgs e) {

            try {
                var wb = openTemplateMensal();

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnCriarDecksSensibilidade_Click(object sender, RibbonControlEventArgs e) {
            var tasks = Globals.ThisAddIn.tasks;
            object rlock = new object();
            object rlock2 = new object();
            object rlock3 = new object();

            try {

                using (DecompTools.Forms.FormPrevsDecksSensibilidade frm = new DecompTools.Forms.FormPrevsDecksSensibilidade()) {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                        var deckBase = DeckFactory.CreateDeck(frm.DeckBase) as Compass.CommomLibrary.Decomp.Deck;
                        deckBase[CommomLibrary.Decomp.DeckDocument.prevs] = null;
                        deckBase[CommomLibrary.Decomp.DeckDocument.vazoes] = null;

                        var pastas = Directory.GetDirectories(frm.PastaSensibilidades);


                        //bool atualizaEA = false;
                        bool deckMensal = false;

                        if ((deckBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger).VAZOES_NumeroDeSemanas == 0) {
                            if (System.Windows.Forms.MessageBox.Show("Identificado decomp MENSAL", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information)
                                == System.Windows.Forms.DialogResult.Cancel)
                                return;
                            else deckMensal = true;
                        }

                        int running = 0;
                        foreach (var pasta in pastas) {

                            deckBase.CopyFilesToFolder(pasta);

                            var ndeck = DeckFactory.CreateDeck(pasta) as Compass.CommomLibrary.Decomp.Deck;

                            //read prevs in worksheet
                            var prevs = ndeck[CommomLibrary.Decomp.DeckDocument.prevs].BasePath;


                            var prevsSem = ndeck[CommomLibrary.Decomp.DeckDocument.prevs].BasePath;
                            if (deckMensal) {


                                var prevsMen = prevsSem.ToLower().Replace("prevs.rv0", "prevs_men.rv0");

                                if (File.Exists(prevsMen)) {
                                    ndeck[CommomLibrary.Decomp.DeckDocument.prevs].BackUp();
                                    File.Delete(prevsSem);
                                    File.Copy(prevsMen, prevsSem);
                                }
                            }

                            //save dadger
                            ndeck[CommomLibrary.Decomp.DeckDocument.dadger].Document.SaveToFile();

                            //run vazoes if selected
                            if (frm.RodarVazoes) {

                                var t = new System.Threading.Tasks.Task(() => {
                                    /****/
                                    lock (rlock) {
                                        while (running >= 3) {
                                            System.Threading.Thread.Sleep(1000);
                                        }
                                        lock (rlock3) running++;

                                    }

                                    var svc = new Compass.Services.Vazoes();
                                    svc.Run(pasta, true);
                                    /****/
                                });

                                t.ContinueWith(tr => {
                                    lock (rlock3) running--;

                                });
                                t.Start();
                            }

                            //clear arq_entrada is selected
                            if (frm.ExcluirArquivosPrevivaz && Directory.Exists(Path.Combine(pasta, "arq_previvaz"))) {
                                Directory.Delete(Path.Combine(pasta, "arq_previvaz"), true);
                            }
                        }

                        var tfin = new System.Threading.Tasks.Task(() => {

                            while (true) {
                                if (running > 0) {
                                    System.Threading.Thread.Sleep(2000);
                                } else {
                                    System.Threading.Thread.Sleep(500);
                                    if (running == 0) break;
                                }
                            }

                            System.Windows.Forms.MessageBox.Show("Finalizado");

                        });
                        tfin.Start();
                    }
                }
            } catch (Exception ex) {

                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnPrevsCenariosProcess_Click(object sender, RibbonControlEventArgs e) {

            try {

                var wbCen = Globals.ThisAddIn.Application.ActiveWorkbook;
                var cenarios = wbCen.ToWorkbookPrevsCenarios();

                if (cenarios == null ||
                    string.IsNullOrWhiteSpace(cenarios.DeckEntrada) ||
                    string.IsNullOrWhiteSpace(cenarios.DiretorioSaida)
                    ) {
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

                foreach (var cenario in cenarios.Cenarios) {
                    Compass.Services.Vazoes vazService = new Services.Vazoes();

                    var pastaSaida = Path.Combine(cenarios.DiretorioSaida, cenario.Key);

                    if (!Directory.Exists(pastaSaida))
                        Directory.CreateDirectory(pastaSaida);

                    deck.CopyFilesToFolder(pastaSaida);

                    cenario.Value.File = Path.Combine(pastaSaida, "PREVS." + deck.Caso);
                    cenario.Value.SaveToFile();


                    if (atualizaEs) {

                        var dgerPath = Path.Combine(pastaSaida, deck.Documents["DADGER."].FileName);

                        var dger = (Dadger)DocumentFactory.Create(dgerPath);


                        var s = 1;
                        foreach (var item in dger.BlocoEs) {
                            item.UltimaEna = Math.Round((double)enas[cenario.Key][s, cenarios.Rev], 0);
                            s++;
                        }

                        dger.SaveToFile();
                    }
                }

                if (System.Windows.Forms.MessageBox.Show("Executar Vaz.Bat?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {

                    System.Threading.Tasks.Parallel.ForEach(cenarios.Cenarios,
                         new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = 3 },
                       cenario => {
                           Compass.Services.Vazoes vazService = new Services.Vazoes();
                           try {
                               var pastaSaida = Path.Combine(cenarios.DiretorioSaida, cenario.Key);
                               vazService.Run(pastaSaida, true);
                           } finally {
                               vazService.ClearTempFolder();
                           }
                       });
                }

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnCalcularENA_Click(object sender, RibbonControlEventArgs e) {
            try {


                var info = ActiveWorkbook.GetInfosheet();

                var fileName = info.DocPath;

                Compass.CommomLibrary.Decomp.Deck deck = new CommomLibrary.Decomp.Deck();
                deck.GetFiles(Path.GetDirectoryName(fileName));

                if (deck.Documents["PREVS."] == null) {
                    System.Windows.Forms.MessageBox.Show("Prevs não encontrado.");
                    return;
                }

                Compass.CommomLibrary.ModifDat.ModifDat modif = null;
                if (deck.Documents["MODIF.DAT"] == null) {
                    System.Windows.Forms.MessageBox.Show("Modif não encontrado. Pode haver divergencia no resultado.");
                } else {
                    modif = (Compass.CommomLibrary.ModifDat.ModifDat)DocumentFactory.Create(deck.Documents["MODIF.DAT"].BasePath);
                }

                //prevs
                var prev = DocumentFactory.Create(deck.Documents["PREVS."].BasePath);




                var confH = LoadConfigH();

                //var ena = confH.GetEna((Compass.CommomLibrary.Prevs.Prevs)prev);
                var ena = confH.GetEnaAcopl((Compass.CommomLibrary.Prevs.Prevs)prev, modif);

                info.Show();


                var temp = confH.index_sistemas.Select(s => {
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
                    )) {
                    xlsM.Range[xlsM.Cells[l, 1], xlsM.Cells[l++, uhe.Length]].Value2 = uhe;
                }


            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnVazoes_Click(object sender, RibbonControlEventArgs e) {
            System.Windows.Forms.FolderBrowserDialog folderSelect = new System.Windows.Forms.FolderBrowserDialog();
            folderSelect.Description = "Selecione o deck";
            var r1 = folderSelect.ShowDialog();
            var baseFolder = folderSelect.SelectedPath;

            if (r1 == System.Windows.Forms.DialogResult.OK) {

                Globals.ThisAddIn.Application.StatusBar = "Running VAZ.BAT";

                var gevazp = new Compass.Services.Vazoes();

                try {

                    var files = gevazp.Run(baseFolder);

                    var xlApp = Globals.ThisAddIn.Application;
                    var xlWb = xlApp.Workbooks.Add();
                    var xlWs = xlWb.GetOrCreateWorksheet("Vazoes");

                    dynamic top = Type.Missing;
                    foreach (var file in files) {
                        var ole = xlWs.OLEObjects().Add(Filename: file.FullPath, Link: false, DisplayAsIcon: true, Top: top);

                        top = ole.Top + 40;
                    }

                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                } finally {

                    Globals.ThisAddIn.Application.StatusBar = null;
                    if (
                    System.Windows.Forms.MessageBox.Show(
                    "Visualizar arquivos interemediários?\r\nCaso negativo serão exluídos automaticamente."
                    , ""
                    , System.Windows.Forms.MessageBoxButtons.YesNo
                    , System.Windows.Forms.MessageBoxIcon.Question
                    )
                        == System.Windows.Forms.DialogResult.Yes
                        ) {

                        gevazp.OpenTempFolder();

                    } else {

                        gevazp.ClearTempFolder();
                    }
                }
            }
        }

        private void btnTendHidr_Click(object sender, RibbonControlEventArgs e) {
            try {
                var frm = new Forms.FormCropVazoesC();

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

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

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnPrevivaz_Click(object sender, RibbonControlEventArgs e) {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;

            try {

                Globals.ThisAddIn.Application.DisplayStatusBar = true;
                Globals.ThisAddIn.Application.StatusBar = "Preparando Entradas...";

                var wb = new WorkbookPrevsCenariosMen(this.ActiveWorkbook);

                var previvazBaseFolder = wb.ArquivosDeEntrada;

                var postosIncrementais = new Dictionary<int, int[]>(){ // <num posto, { postos montantes ... } >
                    {34, new int[] {18, 33, 99, 241, 261}},
                    {245, new int[] {34, 243}},
                    {246, new int[] {245}},
                    {266, new int[] {63, 246}},                    
                };

                ///log parcial 1 -- original
                wb.Saida1 = wb.PrevsCen1;

                WorkbookAcomph acompH = null;
                var semanaprevisao = 7; //dias necessários para considerar a média como semanal

                var openAcomphResponse = OpenAcomph(out acompH);
                if (openAcomphResponse == System.Windows.Forms.DialogResult.Cancel) {
                    return;
                } //else if (openAcomphResponse == System.Windows.Forms.DialogResult.Yes) {
                //if (System.Windows.Forms.MessageBox.Show("Atualizar semana previsão pela média parcial?", "Previvaz - AcompH", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {
                //    semanaprevisao = 4;
                //} else semanaprevisao = 7;
                //}

                var prevsCen1 = wb.Entrada;

                var anoPrev = wb.AnoAtual;

                var usr = System.Environment.UserName.Replace('.', '_');

                var tempFolder = @"L:\shared\CHUVA-VAZAO\previvaz_" + usr;


                if (acompH != null) {
                    acompH.GroupBy(ac => new { ac.semana, ac.posto })
                        .Where(ac => ac.Count() >= semanaprevisao).ToList()
                        .ForEach(ac => {
                            for (int i = 1; i <= wb.SemanasPrevs.Length; i++) {
                                if ((double)wb.SemanasPrevs[1, i] == (double)ac.Key.semana) {
                                    prevsCen1[ac.Key.posto, i + 2] = ac.Average(x => x.qNat);
                                    if (ac.Key.posto == 34) prevsCen1[135, i + 2] = ac.Average(x => x.qInc);
                                    if (ac.Key.posto == 245) prevsCen1[136, i + 2] = ac.Average(x => x.qInc);
                                    if (ac.Key.posto == 246) prevsCen1[137, i + 2] = ac.Average(x => x.qInc);
                                    if (ac.Key.posto == 266) prevsCen1[166, i + 2] = ac.Average(x => x.qInc);
                                }
                            }
                        });
                }

                //foreach (var pn in postosIncrementais.OrderByDescending(x => x.Key)) {
                //    for (int i = 0; i < 12 && (prevsCen1[pn.Key, 3 + i] is double || (pn.Key == 266 && prevsCen1[166, 3 + i] is double)); i++) {
                //        if (pn.Key == 34 && (prevsCen1[135, 3 + i] is double && (double)prevsCen1[135, 3 + i] > 0)) {
                //            prevsCen1[34, 3 + i] = prevsCen1[135, 3 + i];
                //        } else if (pn.Key == 245 && (prevsCen1[136, 3 + i] is double && (double)prevsCen1[136, 3 + i] > 0)) {
                //            prevsCen1[245, 3 + i] = prevsCen1[136, 3 + i];
                //        } else if (pn.Key == 246 && (prevsCen1[137, 3 + i] is double && (double)prevsCen1[137, 3 + i] > 0)) {
                //            prevsCen1[246, 3 + i] = prevsCen1[137, 3 + i];
                //        } else if (pn.Key == 266 && (pn.Value.Any(pMn => !(prevsCen1[pMn, 3 + i] is double)) || (!(prevsCen1[266, 3 + i] is double)) || (double)prevsCen1[266, 3 + i] < 1)) {
                //            prevsCen1[266, 3 + i] = prevsCen1[166, 3 + i];
                //        } else {
                //            foreach (var pMn in pn.Value)
                //                prevsCen1[pn.Key, 3 + i] = Math.Max((double)prevsCen1[pn.Key, 3 + i] - (double)prevsCen1[pMn, 3 + i], 1);
                //        }
                //    }
                //}

                if (System.Windows.Forms.MessageBox.Show("Executar Previvaz?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question)
                    != System.Windows.Forms.DialogResult.Yes)
                    return;


                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
                Directory.CreateDirectory(tempFolder);

                var postosPrevivaz = Directory.GetFiles(previvazBaseFolder).GroupBy(x =>
                    System.Text.RegularExpressions.Regex.Match(
                    Path.GetFileNameWithoutExtension(x),
                    @"^\d+").Value
                   );

                List<object[]> results = new List<object[]>();
                var prevDecks = new List<Compass.CommomLibrary.Previvaz.Deck>();
                Globals.ThisAddIn.Application.StatusBar = "Executando : ";
                foreach (var p in postosPrevivaz) {

                    var prevDeck = new Compass.CommomLibrary.Previvaz.Deck(p.Key);
                    prevDeck.GetFiles(previvazBaseFolder);

                    prevDecks.Add(prevDeck);

                    var path = Path.Combine(tempFolder, p.Key);

                    int posto = int.Parse(prevDeck.Posto);

                    var inp = (Compass.CommomLibrary.Previvaz.Inp)prevDeck[CommomLibrary.Previvaz.DeckDocument.inp].Document;
                    var str = (Compass.CommomLibrary.Previvaz.Str)prevDeck[CommomLibrary.Previvaz.DeckDocument.str].Document;

                    inp.SemanaPrevisao = Convert.ToInt32(wb.SemanasPrevs[1, 1]);
                    inp.AnoPrevisao = anoPrev;

                    for (int s = 1; s < 7; s++) {
                        if (posto == 34 && (prevsCen1[135, s + 2] is double && (double)prevsCen1[135, s + 2] != 0)) {
                            str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = (double)prevsCen1[135, s + 2];
                        } else if (posto == 245 && (prevsCen1[136, s + 2] is double && (double)prevsCen1[136, s + 2] != 0)) {
                            str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = (double)prevsCen1[136, s + 2];
                        } else if (posto == 246 && (prevsCen1[137, s + 2] is double && (double)prevsCen1[137, s + 2] != 0)) {
                            str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = (double)prevsCen1[137, s + 2];
                        } else if (posto == 266 && (prevsCen1[166, s + 2] is double && (double)prevsCen1[166, s + 2] != 0)) {
                            str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = (double)prevsCen1[166, s + 2];
                        } else if (prevsCen1[posto, s + 2] is double && (double)prevsCen1[posto, s + 2] != 0) {
                            str[inp.AnoPrevisao, Convert.ToInt32(wb.SemanasPrevs[1, s])] = (double)prevsCen1[posto, s + 2];
                        } else {
                            break;
                        }

                        var proxSem = Convert.ToInt32(wb.SemanasPrevs[1, s + 1]);

                        if (proxSem < inp.SemanaPrevisao) {
                            inp.AnoPrevisao = inp.AnoPrevisao + 1;
                        }
                        if (proxSem == 2) {
                            str.AnoFinal = inp.AnoPrevisao;
                        }

                        inp.SemanaPrevisao = proxSem;
                    }

                    if (acompH != null) {

                        if (posto == 168) {
                            acompH
                            .Where(ac => ac.posto == 169)
                            .GroupBy(ac => new { ac.semana, ac.ano })
                            .Where(ac => ac.Count() >= semanaprevisao)
                            .ToList().ForEach(ac => {
                                str[ac.Key.ano, ac.Key.semana] = ac.Average(x => x.qInc);
                            });
                        } else if (postosIncrementais.ContainsKey(posto)) {
                            acompH
                                .Where(ac => ac.posto == posto)
                                .GroupBy(ac => new { ac.semana, ac.ano })
                                .Where(ac => ac.Count() >= semanaprevisao)
                                .ToList().ForEach(ac => {
                                    str[ac.Key.ano, ac.Key.semana] = ac.Average(x => x.qInc);
                                });
                        } else {
                            acompH
                                .Where(ac => ac.posto == posto)
                                .GroupBy(ac => new { ac.semana, ac.ano })
                                .Where(ac => ac.Count() >= semanaprevisao)
                                .ToList().ForEach(ac => {
                                    str[ac.Key.ano, ac.Key.semana] = ac.Average(x => x.qNat);
                                });
                        }
                    }

                    prevDeck.CopyFilesToFolder(path);
                }

                Services.Linux.Run(tempFolder, @"/home/marco/PrevisaoPLD/shared/previvaz/previvaz3.sh", "previvaz", true, true, "hide");
                //                Services.Previvaz.RunOnLinux(tempFolder);

                foreach (var prevDeck in prevDecks) {
                    var rs = prevDeck.GetFut();

                    if (rs.Count > 0) {
                        object[] r = (object[])prevDeck.GetFut().First().Value;
                        results.Add(r);
                    }


                }

                // coloca resultado do previvaz na entrada para calcular postos artificias;
                foreach (var r in results) {
                    var c = 1;
                    for (; c <= 12; c++) {
                        if ((int)(double)wb.SemanasPrevs[1, c] == (int)r[3]) {
                            c += 2;
                            break;
                        }
                    }

                    // var c = (int)r[3] - (int)(double)wb.SemanasPrevs[1, 1] + 3;
                    var posto = (int)r[0];
                    for (int i = 0; i < 6; i++) {
                        if (posto == 34) prevsCen1[135, c + i] = r[4 + i];
                        else if (posto == 245) prevsCen1[136, c + i] = r[4 + i];
                        else if (posto == 246) prevsCen1[137, c + i] = r[4 + i];
                        else if (posto == 266) prevsCen1[166, c + i] = r[4 + i];
                        else prevsCen1[posto, c + i] = r[4 + i];
                    }
                }

                #region trata posto 169

                var str156 = (Compass.CommomLibrary.Previvaz.Str)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.str].Document;
                var str158 = (Compass.CommomLibrary.Previvaz.Str)prevDecks.First(x => x.Posto == "158")[CommomLibrary.Previvaz.DeckDocument.str].Document;
                var sem_2 = Convert.ToInt32(wb.SemanasPrevs[1, 1]) - 2;

                if (sem_2 < 1) {
                    sem_2 = sem_2 +
                        ((Compass.CommomLibrary.Previvaz.Inp)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.inp].Document).NumSemanasHist;
                }

                prevsCen1[169, 3] = (double)prevsCen1[168, 3]
                    + str156[anoPrev, sem_2]
                    + str158[anoPrev, sem_2];

                sem_2 = Convert.ToInt32(wb.SemanasPrevs[1, 1]) - 1;
                if (sem_2 < 1) {
                    sem_2 = sem_2 +
                        ((Compass.CommomLibrary.Previvaz.Inp)prevDecks.First(x => x.Posto == "156")[CommomLibrary.Previvaz.DeckDocument.inp].Document).NumSemanasHist;
                }

                prevsCen1[169, 4] = (double)prevsCen1[168, 4]
                    + str156[anoPrev, sem_2]
                    + str158[anoPrev, sem_2];

                for (int i = 5; i <= 14 && prevsCen1[168, i] is double; i++)
                    prevsCen1[169, i] = (double)prevsCen1[156, i - 2] + (double)prevsCen1[158, i - 2] + (double)prevsCen1[168, i];

                #endregion trata posto 169

                wb.Entrada = prevsCen1;

                wb.Regressoes = true;

                // itera em todos os postos para se não houver resultado, na entrada, utiliza o resultado da regressão.
                for (int posto = 1; posto <= 320; posto++)
                    for (int s = 0; s < 12; s++)
                        if (!(prevsCen1[posto, 3 + s] is double) || (double)prevsCen1[posto, 3 + s] < 1) prevsCen1[posto, 3 + s] = wb.PrevsCen1[posto, 3 + s];


                foreach (var pn in postosIncrementais) {
                    for (int i = 0; i < 12; i++) {
                        if (
                            (prevsCen1[pn.Key, 3 + i] is double && (double)prevsCen1[pn.Key, 3 + i] < 1)
                            || !(prevsCen1[pn.Key, 3 + i] is double)
                            ) {
                            prevsCen1[pn.Key, 3 + i] = 0.0;
                            foreach (var pMn in pn.Value) {
                                prevsCen1[pn.Key, 3 + i] = (double)prevsCen1[pn.Key, 3 + i] + (double)prevsCen1[pMn, 3 + i];
                            }
                            if (pn.Key == 34) prevsCen1[pn.Key, 3 + i] = (double)prevsCen1[pn.Key, 3 + i] + (double)prevsCen1[135, 3 + i];
                            else if (pn.Key == 245) prevsCen1[pn.Key, 3 + i] = (double)prevsCen1[pn.Key, 3 + i] + (double)prevsCen1[136, 3 + i];
                            else if (pn.Key == 246) prevsCen1[pn.Key, 3 + i] = (double)prevsCen1[pn.Key, 3 + i] + (double)prevsCen1[137, 3 + i];
                            else if (pn.Key == 266) prevsCen1[pn.Key, 3 + i] = (double)prevsCen1[pn.Key, 3 + i] + (double)prevsCen1[166, 3 + i];
                        }
                    }
                }

                wb.Entrada = prevsCen1;
                wb.Regressoes = false;

                Globals.ThisAddIn.Application.ScreenUpdating = false;

                System.Windows.Forms.MessageBox.Show("Finalizado");
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;


            } catch (Exception ex) {
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.StatusBar = null;
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private static System.Windows.Forms.DialogResult OpenAcomph(out WorkbookAcomph acompH) {
            var res = System.Windows.Forms.MessageBox.Show("Usar Acomph?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
            if (res == System.Windows.Forms.DialogResult.Yes) {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "acomph | *.xls";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    Globals.ThisAddIn.Application.ScreenUpdating = false;

                    var xlWbRdh = Globals.ThisAddIn.Application.Workbooks.Open(ofd.FileName, ReadOnly: true, UpdateLinks: false);

                    acompH = new WorkbookAcomph(xlWbRdh);

                    xlWbRdh.Close();
                    Globals.ThisAddIn.Application.ScreenUpdating = true;
                } else {
                    res = OpenAcomph(out acompH);
                }

            } else acompH = null;

            return res;
        }

        private static System.Windows.Forms.DialogResult OpenRdh(out WorkbookRdh rdh) {

            var res = System.Windows.Forms.MessageBox.Show("Usar RDH?", "Decomp Tools", System.Windows.Forms.MessageBoxButtons.YesNoCancel);

            if (res == System.Windows.Forms.DialogResult.Yes) {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "rdh | *.xls*";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    Globals.ThisAddIn.Application.ScreenUpdating = false;

                    var xlWbRdh = Globals.ThisAddIn.Application.Workbooks.Open(ofd.FileName, ReadOnly: true, UpdateLinks: false);

                    rdh = new WorkbookRdh(xlWbRdh);

                    xlWbRdh.Close();
                    Globals.ThisAddIn.Application.ScreenUpdating = true;
                } else {
                    res = OpenRdh(out rdh);
                }
            } else rdh = null;

            return res;
        }

        Workbook openTemplate() {
            var tfile = "";
            try {

                //tfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "Gera_e_Avalia_Cenarios.xltm");
                tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Gera_e_Avalia_Cenarios_v2.xltm");
                //Directory.CreateDirectory(Path.GetDirectoryName(tfile));

                //File.WriteAllBytes(tfile, t1);

                var xlApp = Globals.ThisAddIn.Application;

                var wb = xlApp.Workbooks.Add(tfile);

                return wb;

            } finally {
                //if (Directory.Exists(Path.GetDirectoryName(tfile))) Directory.Delete(Path.GetDirectoryName(tfile), true);
            }
        }

        Workbook openTemplateMensal() {
            var tfile = "";
            try {


                tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Gera_e_Avalia_Cenarios_Men_Sem.xltm");
                //Directory.CreateDirectory(Path.GetDirectoryName(tfile));

                //File.WriteAllBytes(tfile, t1);

                var xlApp = Globals.ThisAddIn.Application;

                var wb = xlApp.Workbooks.Add(tfile);

                return wb;

            } finally {
            }
        }
    }
}

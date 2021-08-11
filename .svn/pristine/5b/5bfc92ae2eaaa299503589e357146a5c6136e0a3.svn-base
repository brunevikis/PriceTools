using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.SistemaDat;
using Compass.ExcelTools;
using Compass.ExcelTools.Templates;
using System.IO;

namespace Compass.DecompTools {
    public partial class Ribbon1 {

        private void btnRevXIncremet_Click(object sender, RibbonControlEventArgs e) {

            try {

                throw new NotImplementedException();

                //System.Windows.Forms.FolderBrowserDialog f = new System.Windows.Forms.FolderBrowserDialog();

                //if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK) {


                //    var deck = DeckFactory.CreateDeck(f.SelectedPath) as Compass.CommomLibrary.Decomp.Deck;

                //    if (deck != null) {
                //        Services.DecompNextRev.CreateNextRev(deck, @"C:\Temp\mensal");
                //    }
                //}

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnCreateMensal_Click(object sender, RibbonControlEventArgs e) {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;
            try {

                var tfile = "";

                WorkbookMensal w;
                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                    !WorkbookMensal.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w)) {

                    tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Mensal6.xltm");
                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                    return;
                } else if (System.Windows.Forms.MessageBox.Show("Criar decks?\r\n" + "\r\nDestino: " + w.NewaveBase, "Decomp Tool - Mensal", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) {


                    if (System.Windows.Forms.MessageBox.Show("Novo Estudo? ", "Decomp Tool - Mensal", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
                        tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Mensal5.xltm");
                        Globals.ThisAddIn.Application.Workbooks.Add(tfile);
                    }

                    return;
                }

                var dc = w.DecompBase;
                var nw = w.NewaveBase;

                if (
                    w.Version == 4 &&
                    System.Windows.Forms.MessageBox.Show(@"Criar decks Newave?
Sobrescreverá os decks Newave existentes na pasta de resultados. Caso selecione NÃO, os decks atuais não serão modificados"
                    , "Novo estudo encadeado", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {
                    Globals.ThisAddIn.Application.StatusBar = "Criando decks NEWAVE e executando consistencia";


                    //Globals.ThisAddIn.Application.Run("gerarXml");
                    //var ret = Services.Linux.Run(w.CaminhoXml, "/home/marco/PrevisaoPLD/shared/encadeado/EncadeadoDecomp_v0.10.4/bin/ExecutorNEWAVE " +
                    //     w.NomeDoEstudo + ".xml", "NewaveEncadConsist", true, true);

                    //if (!ret) {
                    //    System.Windows.Forms.MessageBox.Show("Ocorreu erro na criação e consistência dos decks newaves. Verifique.");
                    //    return;
                    //}


                    //TODO
                    Encadeado.Estudo estudo = new Encadeado.Estudo() {
                        Origem = w.NewaveOrigem,
                        Destino = w.NewaveBase,
                        MesesAvancar = w.MesesAvancar,
                        DefinirVolumesPO = true,
                    };

                    estudo.VolumesPO = w.Earm;
                    estudo.PrevisaoVazao = w.Cenarios.First().Vazoes;
                    estudo.ExecutavelNewave = w.ExecutavelNewave;


                    if (w.ReDats == null) {

                        if (System.Windows.Forms.MessageBox.Show("Caminho de restricoes elétricas do newave (_redat) não encontrado, continuar mesmo assim?"
                            , "Encadeado", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning)
                            != System.Windows.Forms.DialogResult.Yes)
                            return;

                    }
                    estudo.Restricoes = w.ReDats ?? new List<IRE>();



                    if (System.IO.Directory.Exists(dc)) {

                        var deckDCBase = DeckFactory.CreateDeck(dc) as Compass.CommomLibrary.Decomp.Deck;
                        var configH = new Compass.CommomLibrary.Decomp.ConfigH(
                            deckDCBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger,
                            deckDCBase[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat);

                        estudo.ConfighBase = configH;



                    }
                    estudo.ExecucaoPrincipal();
                }

                if (System.Windows.Forms.MessageBox.Show(@"Criar decks Decomp?
Sobrescreverá os decks Decomp existentes na pasta de resultados. Caso selecione NÃO, os decks atuais não serão modificados"
                    , "Novo estudo encadeado", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {

                    Globals.ThisAddIn.Application.DisplayStatusBar = true;
                    Globals.ThisAddIn.Application.StatusBar = "Lendo arquivos de entrada...";

                    var deckDCBase = DeckFactory.CreateDeck(dc) as Compass.CommomLibrary.Decomp.Deck;

                    var hidrDat = deckDCBase[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat;

                    var meses = Directory.GetDirectories(nw).Select(x => x.Split('\\').Last()).OrderBy(x => x)
                        .Where(x => {
                            int anoEstudo, mesEstudo;

                            if (x.Length != 6
                                || !int.TryParse(x.Substring(0, 4), out anoEstudo)
                                || !int.TryParse(x.Substring(4, 2), out mesEstudo)
                                ) return false;
                            else
                                return true;

                        })
                        .Select(x => new DateTime(int.Parse(x.Substring(0, 4)), int.Parse(x.Substring(4, 2)), 1))
                        .OrderBy(x => x);

                    if (meses.Count() == 0) {
                        System.Windows.Forms.MessageBox.Show("Nenhum caso newave encontrado");
                        return;
                    }

                    var dadgerBase = deckDCBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger;
                    dadgerBase.VAZOES_NumeroDeSemanas = 0;
                    dadgerBase.VAZOES_NumeroDeSemanasPassadas = 0;

                    deckDCBase[CommomLibrary.Decomp.DeckDocument.vazoes] = null;

                    Dictionary<DateTime, Compass.CommomLibrary.Pmo.Pmo> pmosBase = new Dictionary<DateTime, CommomLibrary.Pmo.Pmo>();
                    Dictionary<DateTime, Dadger> dadgers = new Dictionary<DateTime, Dadger>();
                    Dictionary<DateTime, Compass.CommomLibrary.Dadgnl.Dadgnl> dadgnls = new Dictionary<DateTime, Compass.CommomLibrary.Dadgnl.Dadgnl>();
                    Dictionary<DateTime, Compass.CommomLibrary.VazoesC.VazoesC> vazoesCs = new Dictionary<DateTime, Compass.CommomLibrary.VazoesC.VazoesC>();

                    Dictionary<DateTime, Tuple<string, string>> configs = new Dictionary<DateTime, Tuple<string, string>>();

                    foreach (var cenario in w.Cenarios) {

                        List<Tuple<int, double, double>> curvaArmazenamento = null;

                        var outPath = Path.Combine(w.NewaveBase, cenario.NomeDoEstudo);
                        Directory.CreateDirectory(outPath);


                        foreach (var dtEstudo in meses) {

                            Globals.ThisAddIn.Application.StatusBar = "Criando decks " + dtEstudo.ToString("MMM/yyyy");

                            var dtEstudoSeguinte = dtEstudo.AddMonths(1);

                            var estudoPath = Path.Combine(outPath, dtEstudo.ToString("yyyyMM"));

                            Directory.CreateDirectory(estudoPath);

                            deckDCBase.CopyFilesToFolder(estudoPath);

                            var deckEstudo = DeckFactory.CreateDeck(estudoPath) as Compass.CommomLibrary.Decomp.Deck;
                            var deckNWEstudo = DeckFactory.CreateDeck(Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"))) as Compass.CommomLibrary.Newave.Deck;

                            Compass.CommomLibrary.Pmo.Pmo pmoBase;

                            if (pmosBase.ContainsKey(dtEstudo)) {
                                pmoBase = pmosBase[dtEstudo];
                            } else {
                                pmoBase = DocumentFactory.Create(
                                Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"), "pmo.dat")
                                ) as Compass.CommomLibrary.Pmo.Pmo;

                                pmosBase[dtEstudo] = pmoBase;
                            }

                            var horasMesEstudo = (dtEstudo.AddMonths(1) - dtEstudo).TotalHours;
                            var horasMesSeguinte = (dtEstudo.AddMonths(2) - dtEstudo.AddMonths(1)).TotalHours;

                            var patamares = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
                            var sistemas = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as SistemaDat;

                            var durPat1 = patamares.Blocos["Duracao"].Where(x => x[1] == dtEstudo.Year).OrderBy(x => x[0]).Select(x => x[dtEstudo.Month.ToString()]).ToArray();
                            var durPat2 = patamares.Blocos["Duracao"].Where(x => x[1] == dtEstudoSeguinte.Year).OrderBy(x => x[0]).Select(x => x[dtEstudoSeguinte.Month.ToString()]).ToArray();


                            var horasMesEstudoP1 = (float)Math.Round(horasMesEstudo * durPat1[0], 0);
                            var horasMesEstudoP2 = (float)Math.Round(horasMesEstudo * durPat1[1], 0);
                            var horasMesEstudoP3 = (float)Math.Round(horasMesEstudo * durPat1[2], 0);

                            var horasMesSeguinteP1 = (float)Math.Round(horasMesSeguinte * durPat2[0], 0);
                            var horasMesSeguinteP2 = (float)Math.Round(horasMesSeguinte * durPat2[1], 0);
                            var horasMesSeguinteP3 = (float)Math.Round(horasMesSeguinte * durPat2[2], 0);


                            if (dtEstudo.Month == 10) horasMesSeguinteP3 -= 1;
                            else if (dtEstudo.Month == 11) horasMesEstudoP3 -= 1;
                            else if (dtEstudo.Month == 1) horasMesSeguinteP2 += 1;
                            else if (dtEstudo.Month == 2) horasMesEstudoP2 += 1;

                            Compass.CommomLibrary.VazoesC.VazoesC vazC;


                            System.Threading.Tasks.Task vazoesTask = null;


                            if (vazoesCs.ContainsKey(dtEstudo)) {
                                vazC = vazoesCs[dtEstudo];
                            } else {
                                var vazpast = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.vazpast].Document as CommomLibrary.Vazpast.Vazpast;
                                vazC = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.vazoes].Document as Compass.CommomLibrary.VazoesC.VazoesC;

                                vazoesTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
                                    Services.Vazoes6.IncorporarVazpast(vazC, vazpast, dtEstudo)
                                );

                                vazoesCs[dtEstudo] = vazC;
                            }



                            #region DADGER

                            Dadger dadger;

                            if (dadgers.ContainsKey(dtEstudo)) {
                                dadger = dadgers[dtEstudo];
                                dadger.File = Path.Combine(estudoPath, Path.GetFileName(dadger.File));
                                dadger.SaveToFile();

                                File.WriteAllText(Path.Combine(estudoPath, "configh.dat"), configs[dtEstudo].Item1 /*earmconfig*/);
                                File.WriteAllText(Path.Combine(estudoPath, "configm.dat"), configs[dtEstudo].Item2 /*config2*/);

                            } else {
                                dadger = deckEstudo[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger;


                                ((DummyBlock)dadger.Blocos["VR"]).Clear();
                                if (dtEstudo.Month == 10) {
                                    var vrl = new DummyLine();
                                    vrl[0] = "VR";
                                    vrl[1] = "  11        INI";
                                    dadger.Blocos["VR"].Add(vrl);
                                } else if (dtEstudo.Month == 11) {
                                    var vrl = new DummyLine();
                                    vrl[0] = "VR";
                                    vrl[1] = "  11        INI";
                                    dadger.Blocos["VR"].Add(vrl);
                                } else if (dtEstudo.Month == 1) {
                                    var vrl = new DummyLine();
                                    vrl[0] = "VR";
                                    vrl[1] = "   2        FIM";
                                    dadger.Blocos["VR"].Add(vrl);
                                } else if (dtEstudo.Month == 2) {
                                    var vrl = new DummyLine();
                                    vrl[0] = "VR";
                                    vrl[1] = "   2        FIM";
                                    dadger.Blocos["VR"].Add(vrl);
                                }

                                #region carga


                                dadger.BlocoDp.Clear();
                                dadger.BlocoPq.Clear();
                                dadger.BlocoIt.Clear();


                                //estágio 1
                                DpLine lTemp = new DpLine();

                                lTemp[1] = 1;
                                lTemp[3] = 3;
                                lTemp[5] = horasMesEstudoP1;
                                lTemp[7] = horasMesEstudoP2;
                                lTemp[9] = horasMesEstudoP3;

                                var c_adicA = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.cadic].Document as Compass.CommomLibrary.C_AdicDat.C_AdicDat).Adicao
                                    .Where(x => x is Compass.CommomLibrary.C_AdicDat.MerEneLine)
                                    .Cast<Compass.CommomLibrary.C_AdicDat.MerEneLine>();

                                var c_adic = c_adicA
                                    .Where(x => x.Ano == dtEstudo.Year.ToString())
                                    .Select(x => new { mercado = x.Mercado, carga = (double)x[dtEstudo.Month.ToString()] }).ToList();
                                var pequenas = pmoBase.Blocos["PEQUENAS"].Where(x => x[2] == dtEstudo.Year)
                                    .Select(x => new { mercado = x[0], carga = x[dtEstudo.Month.ToString()] });


                                foreach (var m in pmoBase.Blocos["MERCADO"].Where(x => x[2] == dtEstudo.Year).Select(x => new { mercado = x[0], carga = x[dtEstudo.Month.ToString()] })) {

                                    int numMercado = pmoBase.Rees.GetMercado(m.mercado);

                                    var dist_carga = patamares.Carga
                                      .Where(x => x is Compass.CommomLibrary.PatamarDat.CargaEneLine)
                                      .Cast<Compass.CommomLibrary.PatamarDat.CargaEneLine>()
                                      .Where(x => x.Ano == dtEstudo.Year)
                                      .Where(x => x.Mercado == numMercado)
                                      .OrderBy(x => x[0])
                                      .Select(x => x[dtEstudo.Month.ToString()]).ToArray();

                                    var fc1 = dist_carga[0]; // (liquido.First(x => x.mercado == m.mercado && x.pat == 1).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);
                                    var fc2 = dist_carga[1]; // (liquido.First(x => x.mercado == m.mercado && x.pat == 2).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);
                                    var fc3 = dist_carga[2]; // (liquido.First(x => x.mercado == m.mercado && x.pat == 3).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);



                                    var nLine = lTemp.Clone();

                                    nLine[2] = numMercado;


                                    nLine[4] = m.carga * fc1;
                                    nLine[6] = m.carga * fc2;
                                    nLine[8] = m.carga * fc3;

                                    dadger.BlocoDp.Add(nLine);

                                    if (numMercado == 1) {
                                        var itTemp = new ItLine();

                                        double itCarga = c_adic.Where(x => x.mercado == numMercado && x.carga > 0.0)
                                            .Sum(x => x.carga);

                                        itTemp[1] = 1;
                                        itTemp[2] = 66;
                                        itTemp[3] = 1;
                                        itTemp[5] = fc1 * itCarga;
                                        itTemp[7] = fc2 * itCarga;
                                        itTemp[9] = fc3 * itCarga;

                                        itTemp[4] = itTemp[5] + 1900;
                                        itTemp[6] = itTemp[7] + 1900;
                                        itTemp[8] = itTemp[9] + 1900;

                                        dadger.BlocoIt.Add(itTemp);
                                    }

                                    var p = pequenas.Where(x => x.mercado == m.mercado).Select(x => x.carga).ToArray();

                                    var pLine = new PqLine();
                                    pLine[1] = m.mercado;
                                    pLine[2] = numMercado;
                                    pLine[3] = 1;
                                    pLine[4] = p[0];
                                    pLine[5] = p[1];
                                    pLine[6] = p[2];

                                    dadger.BlocoPq.Add(pLine.Clone());

                                    if (c_adic.Any(x => x.mercado == numMercado && x.carga < 0.0)) {

                                        var add = -1.0 * c_adic.Where(x => x.mercado == numMercado && x.carga < 0.0).Sum(x => x.carga);

                                        pLine[1] = "C_ADIC";
                                        pLine[4] = add * fc1;
                                        pLine[5] = add * fc2;
                                        pLine[6] = add * fc3;

                                        dadger.BlocoPq.Add(pLine);
                                    }


                                }



                                var fict = lTemp.Clone();
                                fict[2] = 11;
                                dadger.BlocoDp.Add(fict);

                                //estágio 2



                                lTemp[1] = 2;
                                lTemp[3] = 3;
                                lTemp[5] = horasMesSeguinteP1;
                                lTemp[7] = horasMesSeguinteP2;
                                lTemp[9] = horasMesSeguinteP3;

                                c_adic = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.cadic].Document as Compass.CommomLibrary.C_AdicDat.C_AdicDat).Adicao
                                    .Where(x => x is Compass.CommomLibrary.C_AdicDat.MerEneLine)
                                    .Cast<Compass.CommomLibrary.C_AdicDat.MerEneLine>()
                                    .Where(x => x.Ano == dtEstudoSeguinte.Year.ToString())
                                    .Select(x => new { mercado = x.Mercado, carga = (double)x[dtEstudoSeguinte.Month.ToString()] }).ToList();
                                //c_adics = pmoBase.Blocos["C ADIC"].Where(x => x[2] == dtEstudoSeguinte.Year).Select(x => new { mercado = x[0], carga = x[dtEstudoSeguinte.Month.ToString()] });
                                pequenas = pmoBase.Blocos["PEQUENAS"].Where(x => x[2] == dtEstudoSeguinte.Year).Select(x => new { mercado = x[0], carga = x[dtEstudoSeguinte.Month.ToString()] });
                                //liquido = pmoBase.Blocos["MERCADO LIQUIDO"].Where(x => x[2] == dtEstudoSeguinte.Year).Select(x => new { mercado = x[0], pat = x[1], carga = x[dtEstudoSeguinte.Month.ToString()] });

                                foreach (var m in pmoBase.Blocos["MERCADO"].Where(x => x[2] == dtEstudoSeguinte.Year).Select(x => new { mercado = x[0], carga = x[dtEstudoSeguinte.Month.ToString()] })) {

                                    int numMercado = pmoBase.Rees.GetMercado(m.mercado);

                                    var dist_carga = patamares.Carga
                                      .Where(x => x is Compass.CommomLibrary.PatamarDat.CargaEneLine)
                                      .Cast<Compass.CommomLibrary.PatamarDat.CargaEneLine>()
                                      .Where(x => x.Ano == dtEstudoSeguinte.Year)
                                      .Where(x => x.Mercado == numMercado)
                                      .OrderBy(x => x[0])
                                      .Select(x => x[dtEstudoSeguinte.Month.ToString()]).ToArray();

                                    var fc1 = dist_carga[0]; // (liquido.First(x => x.mercado == m.mercado && x.pat == 1).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);
                                    var fc2 = dist_carga[1]; // (liquido.First(x => x.mercado == m.mercado && x.pat == 2).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);
                                    var fc3 = dist_carga[2]; // (liquido.First(x => x.mercado == m.mercado && x.pat == 3).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);



                                    var nLine = lTemp.Clone();

                                    nLine[2] = numMercado;
                                    //var fc1 = (liquido.First(x => x.mercado == m.mercado && x.pat == 1).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);
                                    //var fc2 = (liquido.First(x => x.mercado == m.mercado && x.pat == 2).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);
                                    //var fc3 = (liquido.First(x => x.mercado == m.mercado && x.pat == 3).carga + pequenas.First(x => x.mercado == m.mercado).carga) / (m.carga + c_adics.First(x => x.mercado == m.mercado).carga);

                                    nLine[4] = m.carga * fc1;
                                    nLine[6] = m.carga * fc2;
                                    nLine[8] = m.carga * fc3;

                                    dadger.BlocoDp.Add(nLine);

                                    if (numMercado == 1) {
                                        var itTemp = new ItLine();

                                        double itCarga = c_adic.Where(x => x.mercado == numMercado && x.carga > 0.0)
                                            .Sum(x => x.carga);

                                        itTemp[1] = 2;
                                        itTemp[2] = 66;
                                        itTemp[3] = 1;
                                        itTemp[5] = fc1 * itCarga;
                                        itTemp[7] = fc2 * itCarga;
                                        itTemp[9] = fc3 * itCarga;

                                        itTemp[4] = itTemp[5] + 1900;
                                        itTemp[6] = itTemp[7] + 1900;
                                        itTemp[8] = itTemp[9] + 1900;

                                        dadger.BlocoIt.Add(itTemp);
                                    }

                                    var p = pequenas.Where(x => x.mercado == m.mercado).Select(x => x.carga).ToArray();

                                    var pLine = new PqLine();
                                    pLine[1] = m.mercado;
                                    pLine[2] = numMercado;
                                    pLine[3] = 2;
                                    pLine[4] = p[0];
                                    pLine[5] = p[1];
                                    pLine[6] = p[2];
                                    dadger.BlocoPq.Add(pLine.Clone());

                                    if (c_adic.Any(x => x.mercado == numMercado && x.carga < 0.0)) {

                                        double add = -1.0 * c_adic.Where(x => x.mercado == numMercado && x.carga < 0.0)
                                            .Sum(x => x.carga);

                                        pLine[1] = "C_ADIC";
                                        pLine[4] = add * fc1;
                                        pLine[5] = add * fc2;
                                        pLine[6] = add * fc3;

                                        dadger.BlocoPq.Add(pLine);
                                    }

                                }

                                fict = lTemp.Clone();
                                fict[2] = 11;
                                dadger.BlocoDp.Add(fict);

                                #endregion

                                #region deficit

                                dadger.BlocoCd.Clear();

                                foreach (var def in pmoBase.Blocos["DEFICIT"]) {

                                    var cTemp = new CdLine();
                                    cTemp[1] = 1;
                                    cTemp[2] = pmoBase.Rees.GetMercado(def[0]);
                                    cTemp[3] = "INTERV 1";
                                    cTemp[4] = 1;
                                    cTemp[5] = cTemp[7] = cTemp[9] = def[5] * 100;
                                    cTemp[6] = cTemp[8] = cTemp[10] = def[1];

                                    dadger.BlocoCd.Add(cTemp.Clone());

                                    if (def[6] != 0) {
                                        cTemp[1] = 2;
                                        cTemp[3] = "INTERV 2";
                                        cTemp[5] = cTemp[7] = cTemp[9] = def[6] * 100;
                                        cTemp[6] = cTemp[8] = cTemp[10] = def[2];
                                        dadger.BlocoCd.Add(cTemp.Clone());
                                    }


                                    if (def[7] != 0) {
                                        cTemp[1] = 3;
                                        cTemp[3] = "INTERV 3";
                                        cTemp[5] = cTemp[7] = cTemp[9] = def[7] * 100;
                                        cTemp[6] = cTemp[8] = cTemp[10] = def[3];
                                        dadger.BlocoCd.Add(cTemp.Clone());
                                    }

                                    if (def[8] != 0) {
                                        cTemp[1] = 4;
                                        cTemp[3] = "INTERV 4";
                                        cTemp[5] = cTemp[7] = cTemp[9] = def[8] * 100;
                                        cTemp[6] = cTemp[8] = cTemp[10] = def[4];
                                        dadger.BlocoCd.Add(cTemp.Clone());
                                    }
                                }


                                #endregion

                                #region ct

                                var cts = dadger.BlocoCT.Select(x => new { cod = x[1], mercado = x[2], nome = x[3], cvu = x[7] })
                                    .OrderBy(x => x.mercado)
                                    .ThenBy(x => x.cvu)
                                    .Distinct().ToArray();

                                dadger.BlocoCT.Clear();


                                foreach (var ct in cts) {

                                    var ctLine = new CtLine();


                                    ctLine[1] = ct.cod;
                                    ctLine[2] = ct.mercado;
                                    ctLine[3] = ct.nome;
                                    ctLine[4] = 1;
                                    ctLine[7] = ctLine[10] = ctLine[13] = ct.cvu; //cvu
                                    ctLine[5] = ctLine[8] = ctLine[11] = pmoBase.Blocos["GTERM Min"]
                                        .Where(x => x[0] == ct.cod)
                                        .Select(x => x[(dtEstudo.Year - x[2]) * 12 + dtEstudo.Month + 2]).FirstOrDefault(); // Inflex
                                    ctLine[6] = ctLine[9] = ctLine[12] = pmoBase.Blocos["GTERM Max"]
                                        .Where(x => x[0] == ct.cod)
                                        .Select(x => x[(dtEstudo.Year - x[2]) * 12 + dtEstudo.Month + 2]).FirstOrDefault(); // Disponibilidade


                                    dadger.BlocoCT.Add(ctLine.Clone());

                                    ctLine[4] = 2;
                                    ctLine[5] = ctLine[8] = ctLine[11] = pmoBase.Blocos["GTERM Min"]
                                        .Where(x => x[0] == ct.cod)
                                        .Select(x => x[(dtEstudo.AddMonths(1).Year - x[2]) * 12 + dtEstudo.AddMonths(1).Month + 2]).FirstOrDefault(); // Inflex
                                    ctLine[6] = ctLine[9] = ctLine[12] = pmoBase.Blocos["GTERM Max"]
                                        .Where(x => x[0] == ct.cod)
                                        .Select(x => x[(dtEstudo.AddMonths(1).Year - x[2]) * 12 + dtEstudo.AddMonths(1).Month + 2]).FirstOrDefault(); // Disponibilidade


                                    dadger.BlocoCT.Add(ctLine);
                                }


                                #endregion

                                #region MP & MT & FD & TI & RQ

                                var semanas = dadger.VAZOES_NumeroDeSemanas;

                                dadger.BlocoMp.Clear();
                                foreach (var uh in dadger.BlocoUh) {

                                    var mpTemp = new MpLine();
                                    mpTemp[1] = uh[1];
                                    var mpUh = dadgerBase.BlocoMp.Where(x => x[1] == uh[1]).FirstOrDefault();

                                    if (mpUh == null) mpTemp[2] = 1;
                                    else {

                                        int count = 0;
                                        int mpCount = semanas > 0 ? semanas : 1;
                                        double mp = 0;
                                        for (; count < mpCount; count++) {
                                            mp += mpUh[2 + count];
                                        }

                                        mpTemp[2] = mp / mpCount;
                                    }

                                    mpTemp[3] = 1;
                                    dadger.BlocoMp.Add(mpTemp);
                                }



                                dadger.BlocoMt.Clear();
                                foreach (var ct in dadger.BlocoCT.Select(x => new { cod = x[1], merc = x[2] }).Distinct()) {
                                    var mtTemp = new MtLine();
                                    mtTemp[1] = ct.cod;
                                    mtTemp[2] = ct.merc;
                                    mtTemp[3] = mtTemp[4] = 1;
                                    dadger.BlocoMt.Add(mtTemp);
                                }


                                if (semanas > 1) {
                                    foreach (var fd in dadger.Blocos["FD"]) {
                                        fd[3] = fd[semanas + 2];
                                        fd[4] = fd[5] = fd[6] = fd[7] = fd[8] = fd[9] = "";
                                    }
                                    foreach (var rq in dadger.Blocos["RQ"]) {
                                        rq[3] = rq[semanas + 2];
                                        rq[4] = rq[5] = rq[6] = rq[7] = rq[8] = rq[9] = "";
                                    }
                                    foreach (var ti in dadger.Blocos["TI"]) {
                                        ti[3] = ti[semanas + 2];
                                        ti[4] = ti[5] = ti[6] = ti[7] = ti[8] = ti[9] = "";
                                    }
                                }

                                #endregion



                                System.Threading.Tasks.Task restricoesTask =
                                System.Threading.Tasks.Task.Factory.StartNew(() => {
                                    #region Restrições

                                    var segundoMes = dadger.VAZOES_NumeroDeSemanas + 1;

                                    //foreach (var rhe in dadger.BlocoRhe.ToArray())
                                    //    if (rhe is ReLine) rhe[3] = 2;
                                    //    else if (rhe[2] == segundoMes) rhe[2] = 2;
                                    //    else if (rhe[2] != 1) dadger.BlocoRhe.Remove(rhe);

                                    foreach (var rh in dadger.BlocoRhe.RheGrouped) {

                                        rh.Key[2] = 1;
                                        rh.Key[3] = 2;

                                        rh.Value.Where(x => x is FuLine || x is FiLine || x is FtLine).ToList().ForEach(x => x[2] = 1);

                                        var ls = rh.Value.Where(x => x is LuLine).OrderBy(x => x[2]);

                                        foreach (var l in ls) {
                                            if (l == ls.First()) l[2] = 1;
                                            else if (l == ls.Last()) l[2] = 2;
                                            else dadger.BlocoRhe.Remove(l);
                                        }
                                    }

                                    //foreach (var rhe in dadger.BlocoRha.ToArray())
                                    //    if (rhe is HaLine) rhe[3] = 2;
                                    //    else if (rhe[2] == segundoMes) rhe[2] = 2;
                                    //    else if (rhe[2] != 1) dadger.BlocoRha.Remove(rhe);

                                    foreach (var rh in dadger.BlocoRha.RhaGrouped) {

                                        rh.Key[2] = 1;
                                        rh.Key[3] = 2;

                                        rh.Value.Where(x => x is CaLine).ToList().ForEach(x => x[2] = 1);

                                        var ls = rh.Value.Where(x => x is LaLine).OrderBy(x => x[2]);

                                        foreach (var l in ls) {
                                            if (l == ls.First()) l[2] = 1;
                                            else if (l == ls.Last()) l[2] = 2;
                                            else dadger.BlocoRha.Remove(l);
                                        }
                                    }

                                    //foreach (var rhe in dadger.BlocoRhv.ToArray())
                                    //    if (rhe is HvLine) rhe[3] = 2;
                                    //    else if (rhe[2] == segundoMes) rhe[2] = 2;
                                    //    else if (rhe[2] != 1) dadger.BlocoRhv.Remove(rhe);

                                    foreach (var rh in dadger.BlocoRhv.RhvGrouped) {

                                        rh.Key[2] = 1;
                                        rh.Key[3] = 2;

                                        rh.Value.Where(x => x is CvLine).ToList().ForEach(x => x[2] = 1);

                                        var ls = rh.Value.Where(x => x is LvLine).OrderBy(x => x[2]);

                                        foreach (var l in ls) {
                                            if (l == ls.First()) l[2] = 1;
                                            else if (l == ls.Last()) l[2] = 2;
                                            else dadger.BlocoRhv.Remove(l);
                                        }
                                    }

                                    foreach (var rh in dadger.BlocoRhq.RhqGrouped) {

                                        rh.Key[2] = 1;
                                        rh.Key[3] = 2;

                                        rh.Value.Where(x => x is CqLine).ToList().ForEach(x => x[2] = 1);

                                        var ls = rh.Value.Where(x => x is LqLine).OrderBy(x => x[2]);

                                        foreach (var l in ls) {
                                            if (l == ls.First()) l[2] = 1;
                                            else if (l == ls.Last()) l[2] = 2;
                                            else dadger.BlocoRhq.Remove(l);
                                        }
                                    }

                                    #endregion

                                    #region Sobrescrever Restrições

                                    //RHE
                                    Action<int, int> overrideRHE = (_m, _e) => {
                                        foreach (var rhe in w.Rhes.Where(x => x.Mes == _m && ((x.Estagio ?? _e) == _e))) {

                                            var rests = dadger.BlocoRhe.RheGrouped
                                                .Where(x => {
                                                    var fs = x.Value.Where(y => (y is FuLine) || /*(y is FtLine) ||*/ (y is FiLine));

                                                    if (rhe.Restricao > 0 && x.Key[1] == rhe.Restricao) return true;

                                                    var ok = fs.Count() == (rhe.Usinas.Count() + rhe.Sistemas.Count());

                                                    if (ok) {

                                                        rhe.Usinas.ForEach(y =>
                                                            ok = ok && fs.Any(z => z is FuLine && z[3] == y)
                                                            );
                                                        rhe.Sistemas.ForEach(y =>
                                                           ok = ok && fs.Any(z => z is FiLine && ((FiLine)z).De == y.Item1 && ((FiLine)z).Para == y.Item2)
                                                           );

                                                    }

                                                    return ok;
                                                }).ToList();


                                            if (!rhe.LimInf1.HasValue && !rhe.LimSup1.HasValue) {

                                                rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhe.Remove(x));
                                                rests.Clear();

                                            } else if (rests.Count == 0) {
                                                var rest = new List<RheLine>();
                                                rest.Add(new ReLine() {
                                                    Restricao = dadger.BlocoRhe.GetNextId(),
                                                    Inicio = _e,
                                                    Fim = 2
                                                });
                                                foreach (var fu in rhe.Usinas) {
                                                    rest.Add(new FuLine() { Restricao = rest.First().Restricao, Usina = fu });
                                                }
                                                foreach (var fi in rhe.Sistemas) {
                                                    rest.Add(new FiLine() { Restricao = rest.First().Restricao, De = fi.Item1, Para = fi.Item2 });
                                                }

                                                rest.ForEach(x => dadger.BlocoRhe.Add(x));

                                                rests.Add(new KeyValuePair<ReLine, List<RheLine>>((ReLine)rest.First(), rest));
                                            }

                                            //if (rest != null) {

                                            foreach (var rest in rests) {
                                                var lu = (LuLine)rest.Value.FirstOrDefault(y => (y is LuLine) && y[2] == _e)
                                                    ?? new LuLine() { Restricao = rest.Value.First().Restricao, Estagio = _e }
                                                    ;
                                                lu[3] = rhe.LimInf1;
                                                lu[4] = rhe.LimSup1;
                                                lu[5] = rhe.LimInf2;
                                                lu[6] = rhe.LimSup2;
                                                lu[7] = rhe.LimInf3;
                                                lu[8] = rhe.LimSup3;

                                                if (!rest.Value.Contains(lu)) dadger.BlocoRhe.Add(lu);
                                            }
                                            //}
                                        }


                                    };

                                    overrideRHE(dtEstudo.Month, 1);
                                    overrideRHE(dtEstudo.Month, 2);

                                    //RHV
                                    Action<int, int> overrideRHV = (_m, _e) => {
                                        foreach (var rhv in w.Rhvs.Where(x => x.Mes == _m && ((x.Estagio ?? _e) == _e))) {

                                            var rests = dadger.BlocoRhv.RhvGrouped
                                                .Where(x => x.Value.Any(y => (y is CvLine) && y[5] == "VARM" && (y[3] == rhv.Usina || y[1] == rhv.Restricao)))
                                                //.Select(x => x.Value).FirstOrDefault();
                                                .ToList();

                                            if (!rhv.LimInf.HasValue && !rhv.LimSup.HasValue) {

                                                rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhv.Remove(x));
                                                rests.Clear();

                                            } else if (rests.Count == 0) {
                                                var rest = new List<RhvLine>();
                                                rest.Add(new HvLine() {
                                                    Restricao = dadger.BlocoRhv.GetNextId(),
                                                    Inicio = _e,
                                                    Fim = 2
                                                });

                                                rest.Add(new CvLine() { Restricao = rest.First().Restricao, Usina = rhv.Usina, Tipo = "VARM" });


                                                rest.ForEach(x => dadger.BlocoRhv.Add(x));
                                                rests.Add(new KeyValuePair<HvLine, List<RhvLine>>((HvLine)rest.First(), rest));
                                            }

                                            foreach (var rest in rests) {
                                                var lu = (LvLine)rest.Value.FirstOrDefault(y => (y is LvLine) && y[2] == _e)
                                                    ?? new LvLine() { Restricao = rest.Value.First().Restricao, Estagio = _e }
                                                    ;
                                                lu[3] = rhv.LimInf;
                                                lu[4] = rhv.LimSup;


                                                if (!rest.Value.Contains(lu)) dadger.BlocoRhv.Add(lu);
                                            }
                                        }
                                    };

                                    overrideRHV(dtEstudo.Month, 1);
                                    overrideRHV(dtEstudo.Month, 2);

                                    //RHQ
                                    Action<int, int> overrideRHQ = (_m, _e) => {
                                        foreach (var rhq in w.Rhqs.Where(x => x.Mes == _m && ((x.Estagio ?? _e) == _e))) {

                                            var rests = dadger.BlocoRhq.RhqGrouped
                                                .Where(x => x.Value.Any(y => (y is CqLine) && y[5] == "QDEF" && (y[3] == rhq.Usina || y[1] == rhq.Restricao)))
                                                //.Select(x => x.Value).FirstOrDefault();
                                                .ToList();


                                            if (!rhq.LimInf1.HasValue && !rhq.LimSup1.HasValue) {

                                                rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                                                rests.Clear();

                                            } else if (rests.Count == 0) {
                                                var rest = new List<RhqLine>();
                                                rest.Add(new HqLine() {
                                                    Restricao = dadger.BlocoRhq.GetNextId(),
                                                    Inicio = _e,
                                                    Fim = 2
                                                });

                                                rest.Add(new CqLine() { Restricao = rest.First().Restricao, Usina = rhq.Usina, Tipo = "QDEF" });


                                                rest.ForEach(x => dadger.BlocoRhq.Add(x));

                                                rests.Add(new KeyValuePair<HqLine, List<RhqLine>>((HqLine)rest.First(), rest));
                                            }

                                            foreach (var rest in rests) {
                                                var lu = (LqLine)rest.Value.FirstOrDefault(y => (y is LqLine) && y[2] == _e)
                                                    ?? new LqLine() { Restricao = rest.Value.First().Restricao, Estagio = _e }
                                                    ;
                                                lu[3] = rhq.LimInf1;
                                                lu[4] = rhq.LimSup1;
                                                lu[5] = rhq.LimInf2;
                                                lu[6] = rhq.LimSup2;
                                                lu[7] = rhq.LimInf3;
                                                lu[8] = rhq.LimSup3;

                                                if (!rest.Value.Contains(lu)) dadger.BlocoRhq.Add(lu);
                                            }
                                        }
                                    };

                                    overrideRHQ(dtEstudo.Month, 1);
                                    overrideRHQ(dtEstudo.Month, 2);

                                    #endregion
                                });

                                #region Alteracoes Cadastrais (AC)

                                foreach (var ac in dadger.BlocoAc.Where(x => !string.IsNullOrWhiteSpace(x.Mes)
                                    || x.Mnemonico == "JUSMED" || x.Mnemonico == "NUMCON" || x.Mnemonico == "NUMMAQ"
                                    ).ToArray())
                                    dadger.BlocoAc.Remove(ac);

                                var modifNW = BaseDocument.Create<Compass.CommomLibrary.ModifDatNW.ModifDatNw>(System.IO.File.ReadAllText(deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.modif].BasePath));
                                ;
                                var exph = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.exph].Document as Compass.CommomLibrary.ExphDat.ExphDat;


                                //canal de fuga
                                var cfugas = from m in modifNW
                                             where m.Chave == "CFUGA"
                                             let dataAlteracao = new DateTime(int.Parse(m.NovosValores[1]), int.Parse(m.NovosValores[0]), 1)
                                             orderby dataAlteracao
                                             group new { data = dataAlteracao, valor = float.Parse(m.NovosValores[2], System.Globalization.NumberFormatInfo.InvariantInfo), usina = m.Usina } by m.Usina;

                                foreach (var cfugasUsina in cfugas) {

                                    //atual
                                    var cfugaAtual = cfugasUsina.LastOrDefault(x => x.data <= dtEstudo);
                                    var cfugaSeguinte = cfugasUsina.FirstOrDefault(x => x.data == dtEstudo.AddMonths(1));


                                    if (cfugaAtual != null) {
                                        var acL = new AcF10Line();
                                        acL.Usina = cfugaAtual.usina;
                                        acL.Mnemonico = "JUSMED";
                                        acL.P1 = cfugaAtual.valor;

                                        dadger.BlocoAc.Add(acL);
                                    }

                                    if (cfugaSeguinte != null) {
                                        var acL = new AcF10Line();
                                        acL.Usina = cfugaSeguinte.usina;
                                        acL.Mnemonico = "JUSMED";
                                        acL.P1 = cfugaSeguinte.valor;
                                        acL.Mes = dtEstudo.AddMonths(1).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                                        acL.Ano = dtEstudo.AddMonths(1).Year;

                                        dadger.BlocoAc.Add(acL);
                                    }
                                }



                                //expansões

                                var modifMaq = modifNW.Where(x => x.Chave == "NUMMAQ")
                                    .Select(x => new { Usina = x.Usina, Conjunto = int.Parse(x.NovosValores[1]), NumMaq = int.Parse(x.NovosValores[0]) });

                                var usinasComExpansao = modifMaq.Select(x => x.Usina).Distinct();


                                var expAtual = exph.Where(x => x.DataEntrada <= dtEstudo).GroupBy(x => new { x.Cod, x.NumConj });
                                var expSeguinte = exph.Where(x => x.DataEntrada <= dtEstudo.AddMonths(1)).GroupBy(x => new { x.Cod, x.NumConj });



                                foreach (var uhe in usinasComExpansao) {
                                    int[] numMaqsIni = { 0, 0, 0, 0, 0 };


                                    foreach (var m in modifMaq.Where(x => x.Usina == uhe))
                                        numMaqsIni[m.Conjunto - 1] = m.NumMaq;

                                    int[] numMaqs1 = { 0, 0, 0, 0, 0 };
                                    int[] numMaqs2 = { 0, 0, 0, 0, 0 };
                                    for (int i = 0; i < 5; i++) numMaqs1[i] = numMaqs2[i] = numMaqsIni[i];

                                    foreach (var ex in expAtual.Where(x => x.Key.Cod == uhe))
                                        numMaqs1[ex.Key.NumConj - 1] = numMaqsIni[ex.Key.NumConj - 1] + ex.Count();

                                    foreach (var ex in expSeguinte.Where(x => x.Key.Cod == uhe))
                                        numMaqs2[ex.Key.NumConj - 1] = numMaqsIni[ex.Key.NumConj - 1] + ex.Count();

                                    if (!numMaqs1.Any(x => x != 0)) {

                                        var aclCon = new AcI5Line();
                                        aclCon.Usina = uhe;
                                        aclCon.Mnemonico = "NUMCON";
                                        aclCon.P1 = 0;

                                        dadger.BlocoAc.Add(aclCon);


                                    } else {

                                        var aclCon = new AcI5Line();
                                        aclCon.Usina = uhe;
                                        aclCon.Mnemonico = "NUMCON";
                                        aclCon.P1 = numMaqs1.Count(x => x != 0);

                                        dadger.BlocoAc.Add(aclCon);


                                        for (int c = 0; c < numMaqs1.Count(x => x != 0); c++) {
                                            var aclMaq = new Ac2I5Line();
                                            aclMaq.Usina = uhe;
                                            aclMaq.Mnemonico = "NUMMAQ";
                                            aclMaq.P1 = c + 1;
                                            aclMaq.P2 = numMaqs1[c];

                                            dadger.BlocoAc.Add(aclMaq);
                                        }
                                    }

                                    if (numMaqs2.Any(x => x != 0)) {

                                        var aclCon = new AcI5Line();
                                        aclCon.Usina = uhe;
                                        aclCon.Mnemonico = "NUMCON";
                                        aclCon.P1 = numMaqs2.Count(x => x != 0);
                                        aclCon.Mes = dtEstudoSeguinte.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                                        aclCon.Ano = dtEstudoSeguinte.Year;

                                        dadger.BlocoAc.Add(aclCon);


                                        for (int c = 0; c < numMaqs2.Count(x => x != 0); c++) {
                                            var aclMaq = new Ac2I5Line();
                                            aclMaq.Usina = uhe;
                                            aclMaq.Mnemonico = "NUMMAQ";
                                            aclMaq.P1 = c + 1;
                                            aclMaq.P2 = numMaqs2[c];
                                            aclMaq.Mes = dtEstudoSeguinte.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                                            aclMaq.Ano = dtEstudoSeguinte.Year;

                                            dadger.BlocoAc.Add(aclMaq);
                                        }
                                    }

                                    var confhd = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.confhd].Document as Compass.CommomLibrary.ConfhdDat.ConfhdDat;

                                    foreach (var novasUhe in
                                        dadger.BlocoAc.Select(x => x.Usina).Distinct().Except(
                                            dadger.BlocoUh.Select(x => x.Usina))) {

                                        var nUh = new UhLine();
                                        nUh.Usina = novasUhe;
                                        nUh.VolIniPerc = 0;
                                        dadger.BlocoUh.Add(nUh);
                                        nUh.Sistema = confhd.First(x => x.Cod == nUh.Usina).REE;



                                        var nMp = new MpLine();
                                        nMp[1] = novasUhe;
                                        nMp[2] = nMp[3] = 1;

                                        dadger.BlocoMp.Add(nMp);



                                    }




                                }

                                #endregion

                                #region Sobreescrever Alteracoes Cadastrais (AC)

                                var acs = w.Acs.Where(x => (x.Mes == dtEstudo.Month || x.Mes == dtEstudoSeguinte.Month || x.Mes == 0) &&
                                    dadger.BlocoUh.Any(y => y.Usina == x.Usina));

                                foreach (var ac in acs) {

                                    //apagar outras modifs existentes
                                    if (ac.Mes == 0) {
                                        var toRemove = dadger.BlocoAc.Where(x => x.Usina == ac.Usina && x.Mnemonico == ac.Mnemonico).ToList();
                                        foreach (var item in toRemove) dadger.BlocoAc.Remove(item);
                                    }

                                    var acL = dadger.BlocoAc.CreateLineFromMnemonico(ac.Mnemonico);
                                    acL.Usina = ac.Usina;
                                    acL.SetValue(3, ac.Valor1);
                                    acL.SetValue(4, ac.Valor2);

                                    if (ac.Mes == dtEstudoSeguinte.Month) {
                                        acL.Ano = dtEstudoSeguinte.Year;
                                        acL.Mes = dtEstudoSeguinte.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                                    }

                                    dadger.BlocoAc.Add(acL);
                                }

                                #endregion

                                #region Cortes

                                Services.Deck.AlterarCortes(dadger, Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"), "cortes.dat"));

                                #endregion

                                #region intercambio

                                var ias = dadger.BlocoIa.Where(x => x[1] != 1).ToArray();
                                foreach (var ia in ias)
                                    dadger.BlocoIa.Remove(ia);
                                #endregion

                                #region VE - volume de espera


                                var vmaxt = from m in modifNW
                                            where m.Chave == "VMAXT"
                                            let dataAlteracao = new DateTime(int.Parse(m.NovosValores[1]), int.Parse(m.NovosValores[0]), 1)
                                            orderby dataAlteracao
                                            group new { data = dataAlteracao, valor = float.Parse(m.NovosValores[2], System.Globalization.NumberFormatInfo.InvariantInfo), usina = m.Usina } by m.Usina;

                                dadger.BlocoVe.Clear();

                                var uhs = dadger.BlocoUh.Select(x => x.Usina).ToArray();

                                foreach (var ve in vmaxt.Where(x => uhs.Contains(x.Key))) {
                                    var veatual = ve.LastOrDefault(x => x.data <= dtEstudo);
                                    var veseguinte = ve.LastOrDefault(x => x.data <= dtEstudo.AddMonths(1));
                                    var l = dadger.BlocoVe.CreateLine();
                                    l[1] = ve.Key;
                                    l[2] = veatual != null ? veatual.valor : 100;
                                    l[3] = veseguinte != null ? veseguinte.valor : 100;

                                    dadger.BlocoVe.Add(l);
                                }

                                #endregion

                                dadger.DataEstudo = dtEstudo;
                                //dadger.VAZOES_MesInicialDoEstudo = dtEstudo.Month;
                                dadger.VAZOES_NumeroDeSemanas = 0;
                                dadger.VAZOES_NumeroDeSemanasPassadas = 0;
                                dadger.VAZOES_NumeroDiasDoMes2 = 0;
                                //dadger.VAZOES_AnoInicialDoEstudo = dtEstudo.Year;
                                dadger.VAZOES_DataDoEstudo = dtEstudo;

                                dadger.VAZOES_EstruturaDaArvore = w.ArvoreSegundoMes[dtEstudo.Month - 1];

                                if (!dadger.Blocos["IR"].Any(x => ((string)x[1]).Contains("ARQCSV")))
                                    dadger.Blocos["IR"].Add(new DummyLine("IR", "  ARQCSV"));



                                restricoesTask.Wait();

                                #region Armazenamento

                                var configH = new Compass.CommomLibrary.Decomp.ConfigH(dadger, hidrDat);
                                var earmMax = configH.GetEarmsMax();
                                configH.ReloadUH();

                                var mesEarmFinal = dtEstudo.Month - 1;

                                Services.Reservatorio.SetUHBlock(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax);
                                configH.baseDoc.SaveToFile();

                                var earmconfig = configH.ToEarmConfigFile(curvaArmazenamento);

                                //manter restricoes de volume para restringir variacao no atingir meta de armazenamento
                                curvaArmazenamento = dadger.BlocoRhv.RhvGrouped
                                    .Where(x => x.Value.Any(y => (y is CvLine) && y[5].Equals("VARM")))
                                    .Select(x => new Tuple<int, double, double>(
                                        x.Value.First(y => (y is CvLine))[3],
                                        x.Value.Any(y => (y is LvLine) && y[2] == 1 && (y[3] is double)) ? x.Value.First(y => (y is LvLine) && y[2] == 1 && (y[3] is double))[3] : 0,
                                        x.Value.Any(y => (y is LvLine) && y[2] == 1 && (y[4] is double)) ? x.Value.First(y => (y is LvLine) && y[2] == 1 && (y[4] is double))[4] : 0
                                    )).ToList();

                                curvaArmazenamento.AddRange(dadger.BlocoVe.Select(x => new Tuple<int, double, double>(x[1], 0,
                                    (x[2] / 100) * configH.usinas[x[1]].VolUtil
                                    )).ToList());

                                File.WriteAllText(Path.Combine(estudoPath, "configh.dat"), earmconfig);


                                var config2 = dtEstudo.AddMonths(-1).ToString("yyyyMM") + "\n";
                                config2 += string.Join(" ", earmMax.Select(x => x.ToString(System.Globalization.CultureInfo.InvariantCulture)).ToArray()) + "\n";
                                config2 += string.Join(" ", w.Earm.Select(x => (x.Value[mesEarmFinal] * earmMax[x.Key - 1]).ToString(System.Globalization.CultureInfo.InvariantCulture)).ToArray()) + "\n";

                                File.WriteAllText(Path.Combine(estudoPath, "configm.dat"), config2);


                                configs[dtEstudo] = new Tuple<string, string>(earmconfig, config2);

                                #endregion Armazenamento

                                dadgers[dtEstudo] = configH.baseDoc as Dadger;

                            }

                            #endregion DADGER

                            #region DADGNL

                            Compass.CommomLibrary.Dadgnl.Dadgnl dadgnl;

                            if (dadgnls.ContainsKey(dtEstudo)) {
                                dadgnl = dadgnls[dtEstudo];
                                dadgnl.File = Path.Combine(estudoPath, Path.GetFileName(dadgnl.File));
                            } else {
                                dadgnl = deckEstudo[CommomLibrary.Decomp.DeckDocument.dadgnl].Document as Compass.CommomLibrary.Dadgnl.Dadgnl;
                                dadgnls[dtEstudo] = dadgnl;

                                var uts = dadgnl.BlocoTG.Where(x => x.Estagio == 1).ToArray();

                                dadgnl.BlocoTG.Clear();
                                dadgnl.BlocoGS.Clear();
                                dadgnl.BlocoGL.Clear();

                                foreach (var ut in uts) {

                                    var tgLine = ut.Clone();

                                    tgLine[5] = tgLine[8] = tgLine[11] = pmoBase.Blocos["GTERM Min"]
                                        .Where(x => x[0] == ut.Usina)
                                        .Select(x => x[(dtEstudo.Year - x[2]) * 12 + dtEstudo.Month + 2]).FirstOrDefault(); // Inflex
                                    tgLine[6] = tgLine[9] = tgLine[12] = pmoBase.Blocos["GTERM Max"]
                                        .Where(x => x[0] == ut.Usina)
                                        .Select(x => x[(dtEstudo.Year - x[2]) * 12 + dtEstudo.Month + 2]).FirstOrDefault(); // Disponibilidade


                                    dadgnl.BlocoTG.Add(tgLine.Clone());
                                    tgLine.Comment = null;

                                    tgLine[4] = 2;
                                    tgLine[5] = tgLine[8] = tgLine[11] = pmoBase.Blocos["GTERM Min"]
                                        .Where(x => x[0] == ut.Usina)
                                        .Select(x => x[(dtEstudo.AddMonths(1).Year - x[2]) * 12 + dtEstudo.AddMonths(1).Month + 2]).FirstOrDefault(); // Inflex
                                    tgLine[6] = tgLine[9] = tgLine[12] = pmoBase.Blocos["GTERM Max"]
                                        .Where(x => x[0] == ut.Usina)
                                        .Select(x => x[(dtEstudo.AddMonths(1).Year - x[2]) * 12 + dtEstudo.AddMonths(1).Month + 2]).FirstOrDefault(); // Disponibilidade


                                    dadgnl.BlocoTG.Add(tgLine);


                                    var glLine = new Compass.CommomLibrary.Dadgnl.GlLine();
                                    glLine.NumeroUsina = ut.Usina;
                                    glLine.Subsistema = ut[2];
                                    glLine.Semana = 1;
                                    glLine.GeracaoPat1 = glLine.GeracaoPat2 = glLine.GeracaoPat3 = 0;
                                    glLine.DuracaoPat1 = horasMesEstudoP1;
                                    glLine.DuracaoPat2 = horasMesEstudoP2;
                                    glLine.DuracaoPat3 = horasMesEstudoP3;
                                    glLine.DiaInicio = dtEstudo.Day;
                                    glLine.MesInicio = dtEstudo.Month;
                                    glLine.AnoInicio = dtEstudo.Year;

                                    dadgnl.BlocoGL.Add(glLine.Clone());

                                    glLine.Semana = 2;
                                    glLine.GeracaoPat1 = glLine.GeracaoPat2 = glLine.GeracaoPat3 = 0;
                                    glLine.DuracaoPat1 = horasMesSeguinteP1;
                                    glLine.DuracaoPat2 = horasMesSeguinteP2;
                                    glLine.DuracaoPat3 = horasMesSeguinteP3;
                                    glLine.DiaInicio = dtEstudoSeguinte.Day;
                                    glLine.MesInicio = dtEstudoSeguinte.Month;
                                    glLine.AnoInicio = dtEstudoSeguinte.Year;
                                    dadgnl.BlocoGL.Add(glLine);


                                }

                                var gsLine = new Compass.CommomLibrary.Dadgnl.GsLine();
                                gsLine[1] = gsLine[2] = 1;
                                dadgnl.BlocoGS.Add(gsLine.Clone());
                                gsLine[1] = 2;
                                dadgnl.BlocoGS.Add(gsLine.Clone());
                                gsLine[1] = 3;
                                dadgnl.BlocoGS.Add(gsLine);
                            }

                            dadgnl.SaveToFile();

                            #endregion DADGNL

                            #region PREVS

                            Compass.CommomLibrary.Prevs.Prevs prevs;
                            if (deckEstudo[CommomLibrary.Decomp.DeckDocument.prevs] == null) {
                                prevs = new CommomLibrary.Prevs.Prevs();
                                prevs.File = Path.Combine(deckEstudo.BaseFolder, "prevs." + deckEstudo.Caso);
                            } else
                                prevs = deckEstudo[CommomLibrary.Decomp.DeckDocument.prevs].Document as Compass.CommomLibrary.Prevs.Prevs;

                            deckEstudo[CommomLibrary.Decomp.DeckDocument.vazoes] = null;


                            prevs.Vazoes.Clear();
                            //var vazoes = cenario.Vazoes;
                            int seq = 1;
                            foreach (var vaz in cenario.Vazoes) {

                                var prL = prevs.Vazoes.CreateLine();
                                prL[0] = seq++;
                                prL[1] = vaz.Key;
                                prL[2] = vaz.Value[dtEstudo.Month - 1];

                                prevs.Vazoes.Add(prL);
                            }

                            prevs.SaveToFile();




                            if (vazoesTask != null) {
                                vazoesTask.Wait();
                            }

                            vazC.SaveToFile(Path.Combine(estudoPath, Path.GetFileName(vazC.File)));

                            #endregion
                        }
                    }
                }

                if (System.Windows.Forms.MessageBox.Show(@"Decks Criados. Agendar execução?
Caso os newaves já tenham sido executados, os cortes existentes serão mantidos e somente a execução dos decomps prosseguirá."
                    , "Novo Estudo Encadeado: " + (w.Version == 4 ? w.NomeDoEstudo : ""), System.Windows.Forms.MessageBoxButtons.YesNo)
                    == System.Windows.Forms.DialogResult.Yes) {
                    Services.Linux.Run(w.NewaveBase, "/home/marco/PrevisaoPLD/cpas_ctl_common/scripts/encad_dc_nw_mensal_3.sh", "EncadeadoMensal-NW+DC", false, false);
                }

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            } finally {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnDecompMensalColeta_Click(object sender, RibbonControlEventArgs e) {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;
            try {

                WorkbookMensal w;
                if (Globals.ThisAddIn.Application.ActiveWorkbook != null &&
                    WorkbookMensal.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w)) {
                } else return;


                var dir = w.NewaveBase;

                if (Directory.Exists(dir)) {
                    dir = dir.EndsWith(Path.DirectorySeparatorChar.ToString()) ? dir.Remove(dir.Length - 1) : dir;
                } else
                    return;


                Func<string, string> clas = x => {

                    var arr = x.ToLowerInvariant().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    var ord = "10";
                    for (int ordI = 0; ordI < arr.Length; ordI++) {

                        var n = arr[ordI];
                        var m = System.Text.RegularExpressions.Regex.Match(n, "(?<=_)[+-]?\\d+");
                        if (m.Success) ord += (int.Parse(m.Value) + 50).ToString("00");
                        else {
                            m = System.Text.RegularExpressions.Regex.Match(n, "^[+-]?\\d+");
                            if (m.Success) ord += (int.Parse(m.Value) + 50).ToString("00");
                            else ord += "99";
                        }
                        ord += n.PadRight(20).Substring(0, 20);
                    }
                    return ord;
                };

                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories)
                    .AsParallel()//.WithDegreeOfParallelism(4)                       
                    .Select(x => new {
                        dir = x.Remove(0, dir.Length),
                        deck = DeckFactory.CreateDeck(x),
                    });

                var dNw = dirs.Where(x => x.deck is CommomLibrary.Newave.Deck)
                    .Select(x => new {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults(),
                        data = (new DirectoryInfo(x.dir)).Name

                    }).OrderBy(x => x.data).Where(x => x.result != null).ToList();


                Func<string, int> getPasso = x => {
                    var m = System.Text.RegularExpressions.Regex.Match(x.Replace(dir, ""), @"_([-+]?\d+)\\");
                    if (m.Success) {
                        return int.Parse(m.Groups[1].Value);
                    } else return 0;
                };
                var dDc = dirs.Where(x => x.deck is CommomLibrary.Decomp.Deck).AsParallel()
                    .Select(x => {

                        var data = (new DirectoryInfo(x.dir)).Name;
                        return new {
                            x.dir,
                            x.deck,
                            result = x.deck.GetResults(),
                            data = data,
                            //passo = getPasso(x.dir),
                            passo = x.dir.Replace(dir, "").Replace(data, "")
                        };

                    }).Where(x => x.result != null).OrderBy(x => x.data).ThenBy(x => x.passo).ToList();


                var nwR = new object[dNw.Count + 1, 42];

                nwR[0, 0] = "Dir";
                nwR[0, 1] = "Data";
                nwR[0, 2] = "CMO";
                nwR[0, 3] = "SE";
                nwR[0, 4] = "S";
                nwR[0, 5] = "NE";
                nwR[0, 6] = "N";
                nwR[0, 7] = "EARM i";
                nwR[0, 8] = "SE";
                nwR[0, 9] = "S";
                nwR[0, 10] = "NE";
                nwR[0, 11] = "N";
                nwR[0, 12] = "ENA";
                nwR[0, 13] = "SE";
                nwR[0, 14] = "S";
                nwR[0, 15] = "NE";
                nwR[0, 16] = "N";
                nwR[0, 17] = "TH";
                nwR[0, 18] = "SE";
                nwR[0, 19] = "S";
                nwR[0, 20] = "NE";
                nwR[0, 21] = "N";

                nwR[0, 22] = "GHidr";
                nwR[0, 23] = "SE";
                nwR[0, 24] = "S";
                nwR[0, 25] = "NE";
                nwR[0, 26] = "N";

                nwR[0, 27] = "GTerm";
                nwR[0, 28] = "SE";
                nwR[0, 29] = "S";
                nwR[0, 30] = "NE";
                nwR[0, 31] = "N";

                nwR[0, 32] = "Pequenas";
                nwR[0, 33] = "SE";
                nwR[0, 34] = "S";
                nwR[0, 35] = "NE";
                nwR[0, 36] = "N";
                nwR[0, 37] = "Demanda Bruta";
                nwR[0, 38] = "SE";
                nwR[0, 39] = "S";
                nwR[0, 40] = "NE";
                nwR[0, 41] = "N";


                for (var i = 0; i < dNw.Count; i++) {
                    var r = dNw[i];
                    nwR[i + 1, 0] = r.dir;
                    nwR[i + 1, 1] = r.data;

                    nwR[i + 1, 3] = r.result[1].Cmo;
                    nwR[i + 1, 4] = r.result[2].Cmo;
                    nwR[i + 1, 5] = r.result[3].Cmo;
                    nwR[i + 1, 6] = r.result[4].Cmo;

                    nwR[i + 1, 8] = r.result[1].EarmI;
                    nwR[i + 1, 9] = r.result[2].EarmI;
                    nwR[i + 1, 10] = r.result[3].EarmI;
                    nwR[i + 1, 11] = r.result[4].EarmI;

                    nwR[i + 1, 13] = r.result[1].EnaMLT;
                    nwR[i + 1, 14] = r.result[2].EnaMLT;
                    nwR[i + 1, 15] = r.result[3].EnaMLT;
                    nwR[i + 1, 16] = r.result[4].EnaMLT;

                    nwR[i + 1, 18] = r.result[1].EnaTHMLT;
                    nwR[i + 1, 19] = r.result[2].EnaTHMLT;
                    nwR[i + 1, 20] = r.result[3].EnaTHMLT;
                    nwR[i + 1, 21] = r.result[4].EnaTHMLT;

                    nwR[i + 1, 23] = r.result[1].GerHidr;
                    nwR[i + 1, 24] = r.result[2].GerHidr;
                    nwR[i + 1, 25] = r.result[3].GerHidr;
                    nwR[i + 1, 26] = r.result[4].GerHidr;

                    nwR[i + 1, 28] = r.result[1].GerTerm;
                    nwR[i + 1, 29] = r.result[2].GerTerm;
                    nwR[i + 1, 30] = r.result[3].GerTerm;
                    nwR[i + 1, 31] = r.result[4].GerTerm;

                    nwR[i + 1, 33] = r.result[1].GerPeq;
                    nwR[i + 1, 34] = r.result[2].GerPeq;
                    nwR[i + 1, 35] = r.result[3].GerPeq;
                    nwR[i + 1, 36] = r.result[4].GerPeq;

                    nwR[i + 1, 38] = r.result[1].DemandaMes;
                    nwR[i + 1, 39] = r.result[2].DemandaMes;
                    nwR[i + 1, 40] = r.result[3].DemandaMes;
                    nwR[i + 1, 41] = r.result[4].DemandaMes;






                }

                var dcR = new object[dDc.Count + 1, 42];

                dcR[0, 0] = "Passo";
                dcR[0, 1] = "Data";
                dcR[0, 2] = "CMO";
                dcR[0, 3] = "SE";
                dcR[0, 4] = "S";
                dcR[0, 5] = "NE";
                dcR[0, 6] = "N";
                dcR[0, 7] = "EARM i";
                dcR[0, 8] = "SE";
                dcR[0, 9] = "S";
                dcR[0, 10] = "NE";
                dcR[0, 11] = "N";
                dcR[0, 12] = "ENA";
                dcR[0, 13] = "SE";
                dcR[0, 14] = "S";
                dcR[0, 15] = "NE";
                dcR[0, 16] = "N";
                dcR[0, 17] = "TH";
                dcR[0, 18] = "SE";
                dcR[0, 19] = "S";
                dcR[0, 20] = "NE";
                dcR[0, 21] = "N";

                dcR[0, 22] = "GHidr";
                dcR[0, 23] = "SE";
                dcR[0, 24] = "S";
                dcR[0, 25] = "NE";
                dcR[0, 26] = "N";

                dcR[0, 27] = "GTerm";
                dcR[0, 28] = "SE";
                dcR[0, 29] = "S";
                dcR[0, 30] = "NE";
                dcR[0, 31] = "N";

                dcR[0, 32] = "Pequenas";
                dcR[0, 33] = "SE";
                dcR[0, 34] = "S";
                dcR[0, 35] = "NE";
                dcR[0, 36] = "N";
                dcR[0, 37] = "Demanda Bruta";
                dcR[0, 38] = "SE";
                dcR[0, 39] = "S";
                dcR[0, 40] = "NE";
                dcR[0, 41] = "N";



                for (var i = 0; i < dDc.Count; i++) {
                    var r = dDc[i];
                    dcR[i + 1, 0] = r.passo;
                    dcR[i + 1, 1] = r.data;

                    dcR[i + 1, 3] = r.result[1].Cmo;
                    dcR[i + 1, 4] = r.result[2].Cmo;
                    dcR[i + 1, 5] = r.result[3].Cmo;
                    dcR[i + 1, 6] = r.result[4].Cmo;

                    dcR[i + 1, 8] = r.result[1].EarmI;
                    dcR[i + 1, 9] = r.result[2].EarmI;
                    dcR[i + 1, 10] = r.result[3].EarmI;
                    dcR[i + 1, 11] = r.result[4].EarmI;

                    dcR[i + 1, 13] = r.result[1].EnaMLT;
                    dcR[i + 1, 14] = r.result[2].EnaMLT;
                    dcR[i + 1, 15] = r.result[3].EnaMLT;
                    dcR[i + 1, 16] = r.result[4].EnaMLT;

                    dcR[i + 1, 18] = r.result[1].EnaTHMLT;
                    dcR[i + 1, 19] = r.result[2].EnaTHMLT;
                    dcR[i + 1, 20] = r.result[3].EnaTHMLT;
                    dcR[i + 1, 21] = r.result[4].EnaTHMLT;

                    dcR[i + 1, 23] = r.result[1].GerHidr;
                    dcR[i + 1, 24] = r.result[2].GerHidr;
                    dcR[i + 1, 25] = r.result[3].GerHidr;
                    dcR[i + 1, 26] = r.result[4].GerHidr;

                    dcR[i + 1, 28] = r.result[1].GerTerm;
                    dcR[i + 1, 29] = r.result[2].GerTerm;
                    dcR[i + 1, 30] = r.result[3].GerTerm;
                    dcR[i + 1, 31] = r.result[4].GerTerm;

                    dcR[i + 1, 33] = r.result[1].GerPeq;
                    dcR[i + 1, 34] = r.result[2].GerPeq;
                    dcR[i + 1, 35] = r.result[3].GerPeq;
                    dcR[i + 1, 36] = r.result[4].GerPeq;

                    dcR[i + 1, 38] = r.result[1].DemandaMes;
                    dcR[i + 1, 39] = r.result[2].DemandaMes;
                    dcR[i + 1, 40] = r.result[3].DemandaMes;
                    dcR[i + 1, 41] = r.result[4].DemandaMes;
                }


                var passos = dDc.Select(x => x.passo).Distinct().ToArray();
                var datas = dDc.Select(x => x.data).Distinct().ToArray();
                var dcSECmoR = new object[(passos.Length + 2) * 5, datas.Length + 1];
                              

                dcSECmoR[(passos.Length + 2) * 0, 0] = @"CMO";
                dcSECmoR[(passos.Length + 2) * 1, 0] = @"ENA";
                dcSECmoR[(passos.Length + 2) * 2, 0] = @"TH";
                dcSECmoR[(passos.Length + 2) * 3, 0] = @"DEMANDA";
                dcSECmoR[(passos.Length + 2) * 4, 0] = @"G HIDR";

                for (int p = 0; p < passos.Length; p++) {
                    dcSECmoR[p + 1 + (passos.Length + 2) * 0, 0] =
                    dcSECmoR[p + 1 + (passos.Length + 2) * 1, 0] =
                    dcSECmoR[p + 1 + (passos.Length + 2) * 2, 0] =
                    dcSECmoR[p + 1 + (passos.Length + 2) * 3, 0] =
                    dcSECmoR[p + 1 + (passos.Length + 2) * 4, 0] = passos[p];
                }

                for (int d = 0; d < datas.Length; d++) {

                    dcSECmoR[(passos.Length + 2) * 0, d + 1] =
                    dcSECmoR[(passos.Length + 2) * 1, d + 1] =
                    dcSECmoR[(passos.Length + 2) * 2, d + 1] =
                    dcSECmoR[(passos.Length + 2) * 3, d + 1] =
                    dcSECmoR[(passos.Length + 2) * 4, d + 1] = datas[d];

                    for (int p = 0; p < passos.Length; p++) {
                        var r = dDc.Where(x => x.data == datas[d] && x.passo == passos[p]).FirstOrDefault();
                        if (r != null) {
                            dcSECmoR[p + 1 + (passos.Length + 2) * 0, d + 1] = r.result[1].Cmo;
                            dcSECmoR[p + 1 + (passos.Length + 2) * 1, d + 1] = r.result[1].EnaMLT;
                            dcSECmoR[p + 1 + (passos.Length + 2) * 2, d + 1] = r.result[1].EnaTHMLT;
                            dcSECmoR[p + 1 + (passos.Length + 2) * 3, d + 1] = r.result[1].DemandaMes;
                            dcSECmoR[p + 1 + (passos.Length + 2) * 4, d + 1] = r.result[1].GerHidr;

                        }
                    }
                }

                w.AddResult("NW", nwR);
                w.AddResult("DC", dcR);

                w.AddResult("Sudeste", dcSECmoR);


            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnDiagramaOper_Click(object sender, RibbonControlEventArgs e) {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;
            try {
                var tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Projeto_Diagrama.xltx");
                WorkbookDiagramaOper w;

                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                    !WorkbookDiagramaOper.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w)) {

                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                    WorkbookDiagramaOper.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w);
                } else {
                    switch (System.Windows.Forms.MessageBox.Show("Sobrescrever Atual?", "Decomp Tool - Diagrama", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question)) {
                        case System.Windows.Forms.DialogResult.No:
                            Globals.ThisAddIn.Application.Workbooks.Add(tfile);
                            WorkbookDiagramaOper.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w);
                            break;
                        case System.Windows.Forms.DialogResult.Cancel:
                            return;
                    }
                }

                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "relato.*|relato.*";
                ofd.Multiselect = false;


                ofd.Title = "Deck A";

                Compass.CommomLibrary.Relato.Relato relatoA = null, relatoB = null;

                Result resultsA = null, resultsB = null;

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    relatoA = DocumentFactory.Create(ofd.FileName) as Compass.CommomLibrary.Relato.Relato;
                    resultsA = DeckFactory.CreateDeck(Path.GetDirectoryName(ofd.FileName)).GetResults();

                }

                ofd.Title = "Deck B";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    relatoB = DocumentFactory.Create(ofd.FileName) as Compass.CommomLibrary.Relato.Relato;
                    resultsB = DeckFactory.CreateDeck(Path.GetDirectoryName(ofd.FileName)).GetResults();
                }

                Globals.ThisAddIn.Application.ScreenUpdating = false;
                Globals.ThisAddIn.Application.StatusBar = "Carregando diagrama...";
                w.Load(relatoA, relatoB, resultsA, resultsB);

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnCheckDecomp_Click(object sender, RibbonControlEventArgs e) {
            try {
                var info = ActiveWorkbook.GetInfosheet();
                if (info == null || !info.DocType.Equals("dadger", StringComparison.OrdinalIgnoreCase)) {
                    throw new Exception("Nenhum dadger carregado.");
                }

                var type = info.DocType;
                var doc = ActiveWorkbook.LoadDocumentFromWorkbook((string)type);
                doc.BottonComments = info.BottonComments;
                if (doc is Dadger) {

                    var incs = ((Dadger)doc).VerificarRestricoes();

                    info.WS.Cells[7, 1].Value = "Inconsistencias";

                    var i = 1;
                    foreach (var inc in incs) {
                        info.WS.Cells[7 + i++, 1].Value = inc;



                    }


                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {

            }
        }

        private void btnInviab_Click(object sender, RibbonControlEventArgs e) {

            try {
                var info = ActiveWorkbook.GetInfosheet();
                if (info == null || !info.DocType.Equals("dadger", StringComparison.OrdinalIgnoreCase)) {
                    throw new Exception("Nenhum dadger carregado.");
                }

                var type = info.DocType;
                var doc = ActiveWorkbook.LoadDocumentFromWorkbook((string)type);
                doc.BottonComments = info.BottonComments;
                doc.File = info.DocPath;


                var fi = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(doc.File), "inviab_unic.*", SearchOption.TopDirectoryOnly).FirstOrDefault();








                if (fi != null && doc is Dadger) {
                    var inviab = (Compass.CommomLibrary.Inviab.Inviab)DocumentFactory.Create(fi);

                    var deck = DeckFactory.CreateDeck(Path.GetDirectoryName(doc.File)) as Compass.CommomLibrary.Decomp.Deck;
                    deck[CommomLibrary.Decomp.DeckDocument.dadger].Document = doc;

                    Services.Deck.DesfazerInviabilidades(deck, inviab);

                    Globals.ThisAddIn.Application.ScreenUpdating = false;


                    ActiveWorkbook.WriteDocumentToWorkbook(doc);

                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;

            }

        }
    }
}

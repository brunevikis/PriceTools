using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.SistemaDat;
using Compass.ExcelTools;
using Compass.ExcelTools.Templates;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.DecompTools
{
    public partial class Ribbon1
    {
        private void btnRevXIncremet_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {

                throw new NotImplementedException();

                //System.Windows.Forms.FolderBrowserDialog f = new System.Windows.Forms.FolderBrowserDialog();

                //if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK) {


                //    var deck = DeckFactory.CreateDeck(f.SelectedPath) as Compass.CommomLibrary.Decomp.Deck;

                //    if (deck != null) {
                //        Services.DecompNextRev.CreateNextRev(deck, @"C:\Temp\mensal");
                //    }
                //}

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

        private void btnCreateMensal_Click(object sender, RibbonControlEventArgs e)
        {
            var planilhaAdterm = new List<IADTERM>();
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;
            try
            {

                var tfile = "";

                WorkbookMensal w;
                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                    !WorkbookMensal.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
                {

                    tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Mensal6.xltm");
                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                    return;
                }
                else if (System.Windows.Forms.MessageBox.Show("Criar decks?\r\n" + "\r\nDestino: " + w.NewaveBase, "Decomp Tool - Mensal", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {


                    if (System.Windows.Forms.MessageBox.Show("Novo Estudo? ", "Decomp Tool - Mensal", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Mensal6.xltm");
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
                    , "Novo estudo encadeado", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Globals.ThisAddIn.Application.StatusBar = "Criando decks NEWAVE e executando consistencia";

                    //TODO
                    Encadeado.Estudo estudo = new Encadeado.Estudo()
                    {
                        Origem = w.NewaveOrigem,
                        Destino = w.NewaveBase,
                        MesesAvancar = w.MesesAvancar,
                        DefinirVolumesPO = true,
                    };

                    estudo.Bloco_VE = w.Bloco_VE;
                    estudo.VolumesPO = w.Earm;
                    estudo.PrevisaoVazao = w.Cenarios.First().Vazoes;
                    estudo.ExecutavelNewave = w.ExecutavelNewave;


                    if (w.ReDats == null)
                    {

                        if (System.Windows.Forms.MessageBox.Show("Caminho de restricoes elétricas do newave (_redat) não encontrado, continuar mesmo assim?"
                            , "Encadeado", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning)
                            != System.Windows.Forms.DialogResult.Yes)
                            return;

                    }
                    estudo.Restricoes = w.ReDats ?? new List<IRE>();

                    estudo.Agrints = w.AgrintDats ?? new List<IAGRIGNT>();

                    estudo.Adterm = w.adtermdat ?? new List<IADTERM>();

                    estudo.Intercambios = w.Intercambios ?? new List<IINTERCAMBIO>();

                    estudo.MERCADO = w.MercadosSisdat ?? new List<IMERCADO>();

                    estudo.Modifs = w.Modifwb ?? new List<IMODIF>();





                    if (System.IO.Directory.Exists(dc))
                    {

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
                    , "Novo estudo encadeado", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {

                    Globals.ThisAddIn.Application.DisplayStatusBar = true;
                    Globals.ThisAddIn.Application.StatusBar = "Lendo arquivos de entrada...";

                    var deckDCBase = DeckFactory.CreateDeck(dc) as Compass.CommomLibrary.Decomp.Deck;

                    var hidrDat = deckDCBase[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat;

                    var meses = Directory.GetDirectories(nw).Select(x => x.Split('\\').Last()).OrderBy(x => x)
                        .Where(x =>
                        {
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

                    if (meses.Count() == 0)
                    {
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

                    foreach (var cenario in w.Cenarios)
                    {

                        List<Tuple<int, double, double>> curvaArmazenamento = null;

                        var outPath = Path.Combine(w.NewaveBase, cenario.NomeDoEstudo);

                        if (cenario.NomeCenario == "Hidrologia - 1")
                        {
                            var dcNome = cenario.NomeDoEstudo;
                            var dcGNLnome = dcNome.Replace("DC", "DCGNL");
                            outPath = Path.Combine(w.NewaveBase, dcGNLnome);

                        }
                        Directory.CreateDirectory(outPath);


                        foreach (var dtEstudo in meses)
                        {

                            Globals.ThisAddIn.Application.StatusBar = "Criando decks " + dtEstudo.ToString("MMM/yyyy");

                            var dtEstudoSeguinte = dtEstudo.AddMonths(1);

                            var estudoPath = Path.Combine(outPath, dtEstudo.ToString("yyyyMM"));

                            Directory.CreateDirectory(estudoPath);

                            deckDCBase.CopyFilesToFolder(estudoPath);

                            var deckEstudo = DeckFactory.CreateDeck(estudoPath) as Compass.CommomLibrary.Decomp.Deck;
                            var deckNWEstudo = DeckFactory.CreateDeck(Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"))) as Compass.CommomLibrary.Newave.Deck;

                            Compass.CommomLibrary.Pmo.Pmo pmoBase;

                            if (pmosBase.ContainsKey(dtEstudo))
                            {
                                pmoBase = pmosBase[dtEstudo];
                            }
                            else
                            {
                                pmoBase = DocumentFactory.Create(
                                Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"), "pmo.dat")
                                ) as Compass.CommomLibrary.Pmo.Pmo;

                                pmosBase[dtEstudo] = pmoBase;
                            }

                            var patamares = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
                            var sistemas = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as SistemaDat;

                            var durPat1 = patamares.Blocos["Duracao"].Where(x => x[1] == dtEstudo.Year).OrderBy(x => x[0]).Select(x => x[dtEstudo.Month.ToString()]).ToArray();
                            var durPat2 = patamares.Blocos["Duracao"].Where(x => x[1] == dtEstudoSeguinte.Year).OrderBy(x => x[0]).Select(x => x[dtEstudoSeguinte.Month.ToString()]).ToArray();

                            var patamares2019 = durPat1[0] > 0.15;


                            MesOperativo mesOperativo = MesOperativo.CreateMensal(dtEstudo.Year, dtEstudo.Month, patamares2019);

                            var horasMesEstudoP1 = mesOperativo.SemanasOperativas[0].HorasPat1;
                            var horasMesEstudoP2 = mesOperativo.SemanasOperativas[0].HorasPat2;
                            var horasMesEstudoP3 = mesOperativo.SemanasOperativas[0].HorasPat3;

                            var horasMesSeguinteP1 = mesOperativo.SemanasOperativas[1].HorasPat1;
                            var horasMesSeguinteP2 = mesOperativo.SemanasOperativas[1].HorasPat2;
                            var horasMesSeguinteP3 = mesOperativo.SemanasOperativas[1].HorasPat3;


                            Compass.CommomLibrary.VazoesC.VazoesC vazC;

                            System.Threading.Tasks.Task vazoesTask = null;

                            if (vazoesCs.ContainsKey(dtEstudo))
                            {
                                vazC = vazoesCs[dtEstudo];
                            }
                            else
                            {
                                var vazpast = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.vazpast].Document as CommomLibrary.Vazpast.Vazpast;
                                vazC = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.vazoes].Document as Compass.CommomLibrary.VazoesC.VazoesC;

                                vazoesTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
                                    Services.Vazoes6.IncorporarVazpast(vazC, vazpast, dtEstudo)
                                );

                                vazoesCs[dtEstudo] = vazC;
                            }

                            #region DADGER

                            Dadger dadger;

                            if (dadgers.ContainsKey(dtEstudo))
                            {
                                dadger = dadgers[dtEstudo];
                                dadger.File = Path.Combine(estudoPath, Path.GetFileName(dadger.File));
                                dadger.SaveToFile();

                                File.WriteAllText(Path.Combine(estudoPath, "configh.dat"), configs[dtEstudo].Item1 /*earmconfig*/);
                                File.WriteAllText(Path.Combine(estudoPath, "configm.dat"), configs[dtEstudo].Item2 /*config2*/);

                            }
                            else
                            {
                                dadger = Services.DecompNextRev.CreateRv0(deckEstudo, deckNWEstudo, dtEstudo, w, mesOperativo, pmoBase);
                                dadger.SaveToFile(createBackup: true);

                                #region Armazenamento

                                var configH = new Compass.CommomLibrary.Decomp.ConfigH(dadger, hidrDat);
                                var earmMax = configH.GetEarmsMax();

                                configH.ReloadUH();

                                var mesEarmFinal = dtEstudo.Month - 1;

                                var earmconfig = configH.ToEarmConfigFile(curvaArmazenamento);

                                #region Atingir Meta Encad arqs Config
                                try
                                {
                                    if (curvaArmazenamento != null)// grava os dados da curva de armazenamento para ser usado durante o atingir meta
                                    {
                                        List<string> curvaTxt = new List<string>();
                                        curvaTxt.Add("Usina	VolMinRest  VolMaxRest");

                                        foreach (var curva in curvaArmazenamento)
                                        {
                                            curvaTxt.Add(curva.Item1.ToString() + "\t" + curva.Item2.ToString() + "\t" + curva.Item3.ToString());
                                        }
                                        File.WriteAllLines(Path.Combine(estudoPath, "curvaArmazenamento.txt"), curvaTxt);
                                    }

                                    var metaEarmDc = w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray();
                                    List<string> metalines = new List<string>();
                                    metalines.Add("Sistema	Meta (EARM ou %)");
                                    int indx = 0;
                                    foreach (var m in metaEarmDc)
                                    {
                                        metalines.Add((indx + 1).ToString() + "\t" + (m * 100f).ToString());
                                        indx++;
                                    }
                                    File.WriteAllLines(Path.Combine(estudoPath, "metasEarm_Sub.txt"), metalines);

                                    List<WorkbookMensal.Dados_Fixa> dadosFixas = new List<WorkbookMensal.Dados_Fixa>();

                                    if (w.Fixaruh.Count() > 0)
                                    {

                                        var fixarLines = w.Fixaruh.Where(x => x.Ano == mesOperativo.Ano).ToList();

                                        foreach (var fl in fixarLines)
                                        {
                                            dadosFixas.Add(new WorkbookMensal.Dados_Fixa(fl.Usina, fl.VolMes[mesEarmFinal]));
                                        }

                                    }
                                    if (dadosFixas.Count() > 0)
                                    {
                                        List<string> fixaUhTxtLines = new List<string>();
                                        fixaUhTxtLines.Add("Usina    VolIniPerc %");
                                        foreach (var df in dadosFixas)
                                        {
                                            fixaUhTxtLines.Add(df.Posto.ToString() + "\t" +( df.Volini != null ? df.Volini.ToString() : "null"));
                                        }
                                        File.WriteAllLines(Path.Combine(estudoPath, "UhFixados.txt"), fixaUhTxtLines);

                                        Services.Reservatorio.SetUHBlockFixado(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax, dadosFixas);
                                    }
                                    else
                                    {
                                        Services.Reservatorio.SetUHBlock(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();

                                }
                                #endregion
                                configH.baseDoc.SaveToFile();

                                File.WriteAllText(Path.Combine(estudoPath, "configh.dat"), earmconfig);

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

                            if (dadgnls.ContainsKey(dtEstudo))
                            {

                                dadgnl = dadgnls[dtEstudo];
                                dadgnl.File = Path.Combine(estudoPath, Path.GetFileName(dadgnl.File));
                            }
                            else
                            {
                                dadgnl = deckEstudo[CommomLibrary.Decomp.DeckDocument.dadgnl].Document as Compass.CommomLibrary.Dadgnl.Dadgnl;
                                dadgnls[dtEstudo] = dadgnl;

                                Compass.CommomLibrary.AdtermDat.AdtermDat adterm;
                                adterm = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.adterm].Document as Compass.CommomLibrary.AdtermDat.AdtermDat;
                                //Estudo planAdterm = new Estudo();
                                Encadeado.Estudo testead = new Encadeado.Estudo();
                                testead.Adterm = w.adtermdat ?? new List<IADTERM>();

                                // Verifica se existi arquivo com informações do dadgnl pela planilha 
                                var arq = Path.Combine(estudoPath, "infos_dadgnl.csv");
                                if (File.Exists(arq))
                                {
                                    File.Delete(arq);
                                }

                                var uts = dadgnl.BlocoTG.Where(x => x.Estagio == 1).ToArray();

                                Tuple<double, double, double> despacho;

                                double[] dadosAdt = new double[3];

                                dadgnl.BlocoTG.Clear();
                                dadgnl.BlocoGS.Clear();
                                dadgnl.BlocoGL.Clear();

                                foreach (var ut in uts)
                                {
                                    var tgLine2 = ut.Clone();
                                    var tgLine = ut.Clone();

                                    tgLine[5] = tgLine[8] = tgLine[11] = pmoBase.Blocos["GTERM Min"]
                                        .Where(x => x[0] == ut.Usina)
                                        .Select(x => x[(dtEstudo.Year - x[2]) * 12 + dtEstudo.Month + 2]).FirstOrDefault(); // Inflex
                                    tgLine[6] = tgLine[9] = tgLine[12] = pmoBase.Blocos["GTERM Max"]
                                        .Where(x => x[0] == ut.Usina)
                                        .Select(x => x[(dtEstudo.Year - x[2]) * 12 + dtEstudo.Month + 2]).FirstOrDefault(); // Disponibilidade


                                    //====
                                    foreach (var adt in adterm.Despachos.Where(x => x.String != "            "))
                                    {
                                        if (adt.Numero == ut.Usina)
                                        {
                                            int indice;
                                            indice = adterm.Despachos.IndexOf(adt);
                                            indice = indice + 1;

                                            dadosAdt[0] = adterm.Despachos[indice].Lim_P1;
                                            dadosAdt[1] = adterm.Despachos[indice].Lim_P2;
                                            dadosAdt[2] = adterm.Despachos[indice].Lim_P3;
                                        }
                                    }
                                    despacho = new Tuple<double, double, double>(dadosAdt[0], dadosAdt[1], dadosAdt[2]);

                                    if (testead.Adterm.Count() == 0)
                                    {
                                        despacho = new Tuple<double, double, double>(0, 0, 0);

                                    }
                                    else
                                    {

                                        var alter_dadgnl = testead.Adterm.Where(x => x.Mes == dtEstudo.Month && x.Usina == ut.Usina).ToList();
                                        if (alter_dadgnl.Count() != 0)
                                        {
                                            //despacho = new Tuple<double, double, double>(alter_dadgnl[0].RestricaoP1, alter_dadgnl[0].RestricaoP2, alter_dadgnl[0].RestricaoP3);
                                            tgLine[7] = alter_dadgnl[0].RestricaoP1;
                                            tgLine[10] = alter_dadgnl[0].RestricaoP2;
                                            tgLine[13] = alter_dadgnl[0].RestricaoP3;
                                            using (TextWriter tw = new StreamWriter(arq, true, Encoding.Default))
                                            {

                                                tw.WriteLine(ut.Usina + ";" + dtEstudo.Month + ";" + alter_dadgnl[0].RestricaoP1 + ";" + alter_dadgnl[0].RestricaoP2 + ";" + alter_dadgnl[0].RestricaoP3); //escreve no arquivo novamente                                                

                                                tw.Close();
                                            }
                                            despacho = new Tuple<double, double, double>(0, 0, 0);
                                        }
                                        despacho = new Tuple<double, double, double>(0, 0, 0);

                                    }
                                    dadgnl.BlocoTG.Add(tgLine.Clone());
                                    var glLine = new Compass.CommomLibrary.Dadgnl.GlLine();

                                    glLine.GeracaoPat1 = Math.Min((float)despacho.Item1, (float)tgLine[6]);
                                    glLine.GeracaoPat2 = Math.Min((float)despacho.Item2, (float)tgLine[9]);
                                    glLine.GeracaoPat3 = Math.Min((float)despacho.Item3, (float)tgLine[12]);
                                    glLine.NumeroUsina = ut.Usina;
                                    glLine.Subsistema = ut[2];
                                    glLine.Semana = 1;

                                    glLine.DuracaoPat1 = horasMesEstudoP1;
                                    glLine.DuracaoPat2 = horasMesEstudoP2;
                                    glLine.DuracaoPat3 = horasMesEstudoP3;
                                    glLine.DiaInicio = dtEstudo.Day;
                                    glLine.MesInicio = dtEstudo.Month;
                                    glLine.AnoInicio = dtEstudo.Year;

                                    dadgnl.BlocoGL.Add(glLine.Clone());

                                    //======
                                    tgLine.Comment = null;

                                    tgLine[4] = 2;
                                    tgLine[5] = tgLine[8] = tgLine[11] = pmoBase.Blocos["GTERM Min"]
                                        .Where(x => x[0] == ut.Usina)
                                        .Select(x => x[(dtEstudo.AddMonths(1).Year - x[2]) * 12 + dtEstudo.AddMonths(1).Month + 2]).FirstOrDefault(); // Inflex
                                    tgLine[6] = tgLine[9] = tgLine[12] = pmoBase.Blocos["GTERM Max"]
                                        .Where(x => x[0] == ut.Usina)
                                        .Select(x => x[(dtEstudo.AddMonths(1).Year - x[2]) * 12 + dtEstudo.AddMonths(1).Month + 2]).FirstOrDefault(); // Disponibilidade




                                    foreach (var adt in adterm.Despachos.Where(x => x.String != "            "))
                                    {
                                        if (adt.Numero == ut.Usina)
                                        {
                                            int indice;
                                            indice = adterm.Despachos.IndexOf(adt);
                                            indice = indice + 2;

                                            dadosAdt[0] = adterm.Despachos[indice].Lim_P1;
                                            dadosAdt[1] = adterm.Despachos[indice].Lim_P2;
                                            dadosAdt[2] = adterm.Despachos[indice].Lim_P3;
                                        }
                                    }
                                    despacho = new Tuple<double, double, double>(dadosAdt[0], dadosAdt[1], dadosAdt[2]);
                                    if (testead.Adterm.Count() == 0)
                                    {
                                        despacho = new Tuple<double, double, double>(0, 0, 0);

                                    }
                                    else
                                    {
                                        var alter_dadgnl = testead.Adterm.Where(x => x.Mes == dtEstudoSeguinte.Month && x.Usina == ut.Usina).ToList();
                                        if (alter_dadgnl.Count() != 0)
                                        {
                                            despacho = new Tuple<double, double, double>(alter_dadgnl[0].RestricaoP1, alter_dadgnl[0].RestricaoP2, alter_dadgnl[0].RestricaoP3);
                                            tgLine[7] = alter_dadgnl[0].RestricaoP1;
                                            tgLine[10] = alter_dadgnl[0].RestricaoP2;
                                            tgLine[13] = alter_dadgnl[0].RestricaoP3;
                                            using (TextWriter tw = new StreamWriter(arq, true, Encoding.Default))
                                            {

                                                tw.WriteLine(ut.Usina + ";" + dtEstudoSeguinte.Month + ";" + alter_dadgnl[0].RestricaoP1 + ";" + alter_dadgnl[0].RestricaoP2 + ";" + alter_dadgnl[0].RestricaoP3); //escreve no arquivo novamente

                                                tw.Close();
                                            }

                                        }
                                        else
                                        {
                                            tgLine[7] = tgLine[10] = tgLine[13] = tgLine2[7];
                                        }
                                        despacho = new Tuple<double, double, double>(0, 0, 0);
                                    }
                                    dadgnl.BlocoTG.Add(tgLine);
                                    //var glLine = new Compass.CommomLibrary.Dadgnl.GlLine();
                                    //glLine.NumeroUsina = ut.Usina;
                                    //glLine.Subsistema = ut[2];
                                    //glLine.Semana = 1;
                                    //glLine.GeracaoPat1 = glLine.GeracaoPat2 = glLine.GeracaoPat3 = 0;
                                    //glLine.DuracaoPat1 = horasMesEstudoP1;
                                    //glLine.DuracaoPat2 = horasMesEstudoP2;
                                    //glLine.DuracaoPat3 = horasMesEstudoP3;
                                    //glLine.DiaInicio = dtEstudo.Day;
                                    //glLine.MesInicio = dtEstudo.Month;
                                    //glLine.AnoInicio = dtEstudo.Year;

                                    //dadgnl.BlocoGL.Add(glLine.Clone());

                                    glLine.Semana = 2;
                                    //glLine.GeracaoPat1 = glLine.GeracaoPat2 = glLine.GeracaoPat3 = 0;
                                    glLine.GeracaoPat1 = Math.Min((float)despacho.Item1, (float)tgLine[6]);
                                    glLine.GeracaoPat2 = Math.Min((float)despacho.Item2, (float)tgLine[9]);
                                    glLine.GeracaoPat3 = Math.Min((float)despacho.Item3, (float)tgLine[12]);

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
                            dadgnl.SaveToFileDadgnlbkp(createBackup: true);
                            dadgnl.SaveToFile();

                            #endregion DADGNL

                            #region PREVS

                            Compass.CommomLibrary.Prevs.Prevs prevs;
                            if (deckEstudo[CommomLibrary.Decomp.DeckDocument.prevs] == null)
                            {
                                prevs = new CommomLibrary.Prevs.Prevs();
                                prevs.File = Path.Combine(deckEstudo.BaseFolder, "prevs." + deckEstudo.Caso);
                            }
                            else
                                prevs = deckEstudo[CommomLibrary.Decomp.DeckDocument.prevs].Document as Compass.CommomLibrary.Prevs.Prevs;

                            deckEstudo[CommomLibrary.Decomp.DeckDocument.vazoes] = null;


                            prevs.Vazoes.Clear();
                            //var vazoes = cenario.Vazoes;
                            int seq = 1;
                            foreach (var vaz in cenario.Vazoes)
                            {

                                var prL = prevs.Vazoes.CreateLine();
                                prL[0] = seq++;
                                prL[1] = vaz.Key;
                                prL[2] = vaz.Value[dtEstudo.Month - 1];

                                prevs.Vazoes.Add(prL);
                            }

                            prevs.SaveToFile();




                            if (vazoesTask != null)
                            {
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
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    //Services.Linux.Run(w.NewaveBase, "/home/producao/PrevisaoPLD/cpas_ctl_common/scripts/encad_dc_nw_mensal_3.sh", "EncadeadoMensal-NW+DC", false, false);
                    Services.Linux.Run(w.NewaveBase, $"/home/producao/PrevisaoPLD/cpas_ctl_common/scripts/encad_dc_nw_mensal_3_{w.versao_Newave}.sh", "EncadeadoMensal-NW+DC", false, false);
                    //Services.Linux.Run(w.NewaveBase, $"/home/compass/sacompass/previsaopld/cpas_ctl_common/scripts/encad_dc_nw_mensal_3_{w.versao_Newave}.sh", "EncadeadoMensal-NW+DC", false, false);
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

        private void btnCreateRV0_Click(object sender, RibbonControlEventArgs e)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;
            try
            {
                var tfile = "";

                WorkbookMensal w;
                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                    !WorkbookMensal.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
                {

                    tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Mensal6.xltm");
                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                    return;
                }
                else if (System.Windows.Forms.MessageBox.Show("Criar decks?\r\n" + "\r\nDestino: " + w.NewaveBase, "Decomp Tool - Mensal", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    if (System.Windows.Forms.MessageBox.Show("Novo Estudo? ", "Decomp Tool - Mensal", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Mensal6.xltm");
                        Globals.ThisAddIn.Application.Workbooks.Add(tfile);
                    }
                    return;
                }

                {

                    var dc = w.DecompBase;
                    var nw = w.NewaveBase;
                    var nwOrigin = w.NewaveOrigem;


                    List<DateTime> meses;
                    if (!String.IsNullOrWhiteSpace(nw) && System.IO.Directory.Exists(nw))
                    {
                        meses = Directory.GetDirectories(nw).Select(x => x.Split('\\').Last()).OrderBy(x => x)
                        .Where(x => x.Length == 6
                                && int.TryParse(x.Substring(0, 4), out _)
                                && int.TryParse(x.Substring(4, 2), out _)
                        )
                        .Select(x => new DateTime(int.Parse(x.Substring(0, 4)), int.Parse(x.Substring(4, 2)), 1))
                        .OrderBy(x => x).ToList();
                    }
                    else
                        meses = new List<DateTime>();

                    string outPath;
                    Compass.CommomLibrary.Newave.Deck deckNWEstudo = null;

                    if (meses.Count() == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Encad Newave não encontrado, selecione data e pasta de saída do novo deck.");
                        Forms.FormNewRv0 frm = new Forms.FormNewRv0();
                        frm.DataEstudo = DateTime.Today.AddMonths(1);
                        frm.CaminhoSaida = nwOrigin;

                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            meses.Add(frm.DataEstudo);
                            outPath = Path.Combine(frm.CaminhoSaida, "RV0");



                        }
                        else return;
                    }
                    else if (System.Windows.Forms.MessageBox.Show(@"Criar decks Decomp?
Sobrescreverá os decks Decomp existentes na pasta de resultados. Caso selecione NÃO, os decks atuais não serão modificados"
                   , "Novo estudo encadeado", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        outPath = Path.Combine(w.NewaveBase, "RV0");
                    }
                    else
                        return;

                    Globals.ThisAddIn.Application.DisplayStatusBar = true;
                    Globals.ThisAddIn.Application.StatusBar = "Lendo arquivos de entrada...";

                    var deckDCBase = DeckFactory.CreateDeck(dc) as Compass.CommomLibrary.Decomp.Deck;





                    Directory.CreateDirectory(outPath);

                    List<Tuple<int, double, double>> curvaArmazenamento = null;

                    foreach (var dtEstudo in meses)
                    {
                        Globals.ThisAddIn.Application.StatusBar = "Criando decks " + dtEstudo.ToString("MMM/yyyy");

                        var estudoPath = Path.Combine(outPath, dtEstudo.ToString("yyyyMM"));
                        Directory.CreateDirectory(estudoPath);

                        deckDCBase.CopyFilesToFolder(estudoPath, 0);

                        var deckEstudo = DeckFactory.CreateDeck(estudoPath) as Compass.CommomLibrary.Decomp.Deck;

                        deckNWEstudo = DeckFactory.CreateDeck(Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"))) as Compass.CommomLibrary.Newave.Deck;

                        var patamares = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
                        var durPat1 = patamares.Blocos["Duracao"].Where(x => x[1] == dtEstudo.Year).OrderBy(x => x[0]).Select(x => x[dtEstudo.Month.ToString()]).ToArray();
                        bool patamares2019 = durPat1[0] > 0.15;

                        var pmoBase = DocumentFactory.Create(System.IO.Path.Combine(deckNWEstudo.BaseFolder, "pmo.dat")) as Compass.CommomLibrary.Pmo.Pmo;

                        var mesOperativo = MesOperativo.CreateSemanal(dtEstudo.Year, dtEstudo.Month, patamares2019);


                        //  if (dtEstudo != (deckDCBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger).VAZOES_DataDoEstudo)
                        // {
                        var dadger = Services.DecompNextRev.CreateRv0(deckEstudo, deckNWEstudo, dtEstudo, w, mesOperativo, pmoBase);

                        dadger.VAZOES_ArquivoPrevs = "prevs.rv0";
                        dadger.SaveToFile(createBackup: true);
                        //  }




                        #region Armazenamento
                        var hidrDat = deckEstudo[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat;
                        var configH = new Compass.CommomLibrary.Decomp.ConfigH(dadger, hidrDat);
                        var earmMax = configH.GetEarmsMax();

                        configH.ReloadUH();

                        var mesEarmFinal = dtEstudo.Month - 1;

                        var earmconfig = configH.ToEarmConfigFile(curvaArmazenamento);


                        #region Atingir Meta Encad arqs Config
                        try
                        {
                            if (curvaArmazenamento != null)// grava os dados da curva de armazenamento para ser usado durante o atingir meta
                            {
                                List<string> curvaTxt = new List<string>();
                                curvaTxt.Add("Usina	VolMinRest  VolMaxRest");

                                foreach (var curva in curvaArmazenamento)
                                {
                                    curvaTxt.Add(curva.Item1.ToString() + "\t" + curva.Item2.ToString() + "\t" + curva.Item3.ToString());
                                }
                                File.WriteAllLines(Path.Combine(estudoPath, "curvaArmazenamento.txt"), curvaTxt);
                            }
                            var metaEarmDc = w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray();
                            List<string> metalines = new List<string>();
                            metalines.Add("Sistema	Meta (EARM ou %)");
                            int indx = 0;
                            foreach (var m in metaEarmDc)
                            {
                                metalines.Add((indx + 1).ToString() + "\t" + (m * 100f).ToString());
                                indx++;
                            }
                            File.WriteAllLines(Path.Combine(estudoPath, "metasEarm_Sub.txt"), metalines);

                            List<WorkbookMensal.Dados_Fixa> dadosFixas = new List<WorkbookMensal.Dados_Fixa>();

                            if (w.Fixaruh.Count() > 0)
                            {

                                var fixarLines = w.Fixaruh.Where(x => x.Ano == mesOperativo.Ano).ToList();

                                foreach (var fl in fixarLines)
                                {
                                    dadosFixas.Add(new WorkbookMensal.Dados_Fixa(fl.Usina, fl.VolMes[mesEarmFinal]));
                                }

                            }
                            if (dadosFixas.Count() > 0)
                            {
                                List<string> fixaUhTxtLines = new List<string>();
                                fixaUhTxtLines.Add("Usina    VolIniPerc %");

                                foreach (var df in dadosFixas)
                                {
                                    fixaUhTxtLines.Add(df.Posto.ToString() + "\t" + df.Volini.ToString());
                                }
                                File.WriteAllLines(Path.Combine(estudoPath, "UhFixados.txt"), fixaUhTxtLines);

                                Services.Reservatorio.SetUHBlockFixado(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax, dadosFixas);
                            }
                            else
                            {
                                Services.Reservatorio.SetUHBlock(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax);
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.ToString();

                        }
                        #endregion

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



                        var config2 = dtEstudo.AddMonths(-1).ToString("yyyyMM") + "\n";
                        config2 += string.Join(" ", earmMax.Select(x => x.ToString(System.Globalization.CultureInfo.InvariantCulture)).ToArray()) + "\n";
                        config2 += string.Join(" ", w.Earm.Select(x => (x.Value[mesEarmFinal] * earmMax[x.Key - 1]).ToString(System.Globalization.CultureInfo.InvariantCulture)).ToArray()) + "\n";

                        //File.WriteAllText(Path.Combine(estudoPath, "configm.dat"), config2);


                        //configs[dtEstudo] = new Tuple<string, string>(earmconfig, config2);

                        dadger = configH.baseDoc as Dadger;
                        dadger.SaveToFile();

                        #endregion Armazenamento

                        #region DeplecionamentoTucurui
                        var voltutilTucu = 38982d;

                        var volInicial = dadger.BlocoUh.First(x => x.Usina == 275).VolIniPerc * voltutilTucu / 100;

                        var rhvsCandidatos = dadger.BlocoRhv.Where(x => (x is CvLine cy) && cy.Usina == 275 && cy.Tipo == "VARM").Select(x => x.Restricao);

                        if (rhvsCandidatos.Count() > 0)
                        {
                            var rhv = dadger.BlocoRhv.RhvGrouped.Where(x => x.Key.Restricao == rhvsCandidatos.First()).First();

                            var lv = rhv.Value.Where(x => (x is LvLine xv) && xv.Estagio == 1).Select(x => (LvLine)x).FirstOrDefault();

                            if (lv != null && (lv[3] is double) && (lv[4] is double))
                            {
                                if (lv[3] == lv[4])
                                {


                                    var volMetaMin = (double)lv[3];

                                    var volMetaMax = (double)lv[4];

                                    var deltaMin = (volMetaMin - volInicial) / (mesOperativo.Estagios);
                                    var deltaMax = (volMetaMax - volInicial) / (mesOperativo.Estagios);

                                    lv.Estagio = mesOperativo.Estagios;
                                    var idx = dadger.BlocoRhv.IndexOf(lv);

                                    for (int est = mesOperativo.Estagios - 1; est > 0; est--)
                                    {
                                        volMetaMin -= deltaMin;
                                        volMetaMax -= deltaMax;
                                        var lvNovo = lv.Clone() as LvLine;
                                        lvNovo.Estagio = est;
                                        lvNovo[3] = volMetaMin;
                                        lvNovo[4] = volMetaMax;
                                        dadger.BlocoRhv.Insert(idx, lvNovo);
                                    }
                                }
                            }

                            dadger.SaveToFile();
                        }
                        #endregion

                        #region DADGNL

                        Compass.CommomLibrary.Dadgnl.Dadgnl dadgnl;
                        Compass.CommomLibrary.AdtermDat.AdtermDat adterm;


                        dadgnl = deckEstudo[CommomLibrary.Decomp.DeckDocument.dadgnl].Document as Compass.CommomLibrary.Dadgnl.Dadgnl;
                        adterm = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.adterm].Document as Compass.CommomLibrary.AdtermDat.AdtermDat;


                        var uts = dadgnl.BlocoTG.Where(x => x.Estagio == 1).ToArray();

                        dadgnl.BlocoTG.Clear();
                        dadgnl.BlocoGS.Clear();

                        var glOriginal = dadgnl.BlocoGL.ToList();
                        dadgnl.BlocoGL.Clear();

                        foreach (var ut in uts)
                        {

                            var tgLine = ut.Clone();

                            tgLine[5] = tgLine[8] = tgLine[11] = pmoBase.Blocos["GTERM Min"]
                                .Where(x => x[0] == ut.Usina)
                                .Select(x => x[(mesOperativo.Ano - x[2]) * 12 + mesOperativo.Mes + 2]).FirstOrDefault(); // Inflex

                            var dispMes = pmoBase.Blocos["GTERM Max"]
                                .Where(x => x[0] == ut.Usina)
                                .Select(x => x[(mesOperativo.Ano - x[2]) * 12 + mesOperativo.Mes + 2]).FirstOrDefault(); // Disponibilidade
                            dispMes = Convert.ToDouble(dispMes);

                            tgLine[6] = tgLine[9] = tgLine[12] = dispMes;

                            dadgnl.BlocoTG.Add(tgLine.Clone());
                            tgLine.Comment = null;

                            tgLine[4] = mesOperativo.Estagios + 1;
                            tgLine[5] = tgLine[8] = tgLine[11] = pmoBase.Blocos["GTERM Min"]
                                .Where(x => x[0] == ut.Usina)
                                .Select(x => x[(mesOperativo.AnoSeguinte - x[2]) * 12 + mesOperativo.MesSeguinte + 2]).FirstOrDefault(); // Inflex


                            var dispMesSeguinte = pmoBase.Blocos["GTERM Max"]
                                .Where(x => x[0] == ut.Usina)
                                .Select(x => x[(mesOperativo.AnoSeguinte - x[2]) * 12 + mesOperativo.MesSeguinte + 2]).FirstOrDefault(); // Disponibilidade
                            tgLine[6] = tgLine[9] = tgLine[12] = dispMesSeguinte;


                            dadgnl.BlocoTG.Add(tgLine);

                            var glLine = new Compass.CommomLibrary.Dadgnl.GlLine();
                            glLine.NumeroUsina = ut.Usina;
                            glLine.Subsistema = ut[2];

                            for (int _e = 0; _e < mesOperativo.EstagiosReaisDoMesAtual; _e++)
                            {
                                Tuple<double, double, double> despacho;
                                int indice;
                                double[] dadosAdt = new double[3];

                                foreach (var adt in adterm.Despachos.Where(x => x.String != "            "))
                                {
                                    if (adt.Numero == ut.Usina)
                                    {
                                        indice = adterm.Despachos.IndexOf(adt);
                                        indice = indice + 1;

                                        dadosAdt[0] = adterm.Despachos[indice].Lim_P1;
                                        dadosAdt[1] = adterm.Despachos[indice].Lim_P2;
                                        dadosAdt[2] = adterm.Despachos[indice].Lim_P3;
                                    }
                                }

                                despacho = new Tuple<double, double, double>(dadosAdt[0], dadosAdt[1], dadosAdt[2]);


                                glLine.Semana = _e + 1;
                                glLine.GeracaoPat1 = Math.Min((float)despacho.Item1, (float)dispMes);
                                glLine.GeracaoPat2 = Math.Min((float)despacho.Item2, (float)dispMes);
                                glLine.GeracaoPat3 = Math.Min((float)despacho.Item3, (float)dispMes);
                                glLine.DuracaoPat1 = mesOperativo.SemanasOperativas[_e].HorasPat1;
                                glLine.DuracaoPat2 = mesOperativo.SemanasOperativas[_e].HorasPat2;
                                glLine.DuracaoPat3 = mesOperativo.SemanasOperativas[_e].HorasPat3;
                                glLine.DiaInicio = mesOperativo.SemanasOperativas[_e].Inicio.Day;
                                glLine.MesInicio = mesOperativo.SemanasOperativas[_e].Inicio.Month;
                                glLine.AnoInicio = mesOperativo.SemanasOperativas[_e].Inicio.Year;

                                dadgnl.BlocoGL.Add(glLine.Clone());
                            }

                            var dtTemp = mesOperativo.Fim.AddDays(1);

                            for (int _e = mesOperativo.EstagiosReaisDoMesAtual; _e < 9; _e++)
                            {

                                var endSemanaTemp = dtTemp.AddDays(6);
                                if (_e > mesOperativo.EstagiosReaisDoMesAtual && endSemanaTemp.Day < 7) endSemanaTemp = endSemanaTemp.AddDays(-endSemanaTemp.Day);


                                var semanaOperativaTemp = new SemanaOperativa(dtTemp, endSemanaTemp, patamares2019);


                                var despachoDeckAnterior = glOriginal.Where(x => x.NumeroUsina == ut.Usina)
                                    .Where(x => new DateTime(x.AnoInicio, x.MesInicio, x.DiaInicio) == semanaOperativaTemp.Inicio).FirstOrDefault();

                                Tuple<double, double, double> despacho;
                                int indice;
                                double[] dadosAdt = new double[3];


                                foreach (var adt in adterm.Despachos.Where(x => x.String != "            "))
                                {
                                    if (adt.Numero == ut.Usina)
                                    {
                                        indice = adterm.Despachos.IndexOf(adt);

                                        indice = indice + 2;

                                        dadosAdt[0] = adterm.Despachos[indice].Lim_P1;
                                        dadosAdt[1] = adterm.Despachos[indice].Lim_P2;
                                        dadosAdt[2] = adterm.Despachos[indice].Lim_P3;
                                    }

                                }
                                despacho = new Tuple<double, double, double>(dadosAdt[0], dadosAdt[1], dadosAdt[2]);

                                glLine.Semana = _e + 1;
                                glLine.GeracaoPat1 = Math.Min((float)despacho.Item1, (float)dispMesSeguinte);
                                glLine.GeracaoPat2 = Math.Min((float)despacho.Item2, (float)dispMesSeguinte);
                                glLine.GeracaoPat3 = Math.Min((float)despacho.Item3, (float)dispMesSeguinte);
                                glLine.DuracaoPat1 = semanaOperativaTemp.HorasPat1;
                                glLine.DuracaoPat2 = semanaOperativaTemp.HorasPat2;
                                glLine.DuracaoPat3 = semanaOperativaTemp.HorasPat3;
                                glLine.DiaInicio = semanaOperativaTemp.Inicio.Day;
                                glLine.MesInicio = semanaOperativaTemp.Inicio.Month;
                                glLine.AnoInicio = semanaOperativaTemp.Inicio.Year;

                                dtTemp = dtTemp.AddDays(7);

                                dadgnl.BlocoGL.Add(glLine.Clone());
                            }
                        }

                        var gsLine = new Compass.CommomLibrary.Dadgnl.GsLine();
                        gsLine[1] = 1;
                        gsLine[2] = mesOperativo.Estagios;
                        dadgnl.BlocoGS.Add(gsLine.Clone());
                        gsLine[1] = 2;
                        gsLine[2] = 9 - mesOperativo.Estagios;
                        dadgnl.BlocoGS.Add(gsLine.Clone());
                        gsLine[1] = 3;
                        gsLine[2] = mesOperativo.Estagios;
                        dadgnl.BlocoGS.Add(gsLine);


                        dadgnl.SaveToFile(createBackup: true);

                        #endregion DADGNL

                        #region PREVS
                        {
                            var vazpast = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.vazpast].Document as CommomLibrary.Vazpast.Vazpast;
                            var vazC = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.vazoes].Document as Compass.CommomLibrary.VazoesC.VazoesC;
                            Services.Vazoes6.IncorporarVazpast(vazC, vazpast, dtEstudo);

                            Compass.CommomLibrary.Prevs.Prevs prevs;
                            if (deckEstudo[CommomLibrary.Decomp.DeckDocument.prevs] == null)
                            {
                                prevs = new CommomLibrary.Prevs.Prevs();
                                prevs.File = Path.Combine(deckEstudo.BaseFolder, "prevs." + deckEstudo.Caso);
                            }
                            else
                                prevs = deckEstudo[CommomLibrary.Decomp.DeckDocument.prevs].Document as Compass.CommomLibrary.Prevs.Prevs;

                            deckEstudo[CommomLibrary.Decomp.DeckDocument.vazoes] = null;

                            prevs.Vazoes.Clear();
                            //var vazoes = cenario.Vazoes;
                            int seq = 1;
                            foreach (var vaz in w.Cenarios.First().Vazoes)
                            {
                                var prL = prevs.Vazoes.CreateLine();
                                prL[0] = seq++;
                                prL[1] = vaz.Key;
                                for (int _e = 0; _e < mesOperativo.Estagios; _e++)
                                {
                                    prL[2 + _e] = vaz.Value[dtEstudo.Month - 1];
                                }

                                prevs.Vazoes.Add(prL);
                            }

                            prevs.SaveToFile();

                            vazC.SaveToFile(Path.Combine(estudoPath, Path.GetFileName(vazC.File)));
                        }
                        #endregion


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

        private void btnDecompMensalColeta_Click(object sender, RibbonControlEventArgs e)
        {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;
            try
            {

                WorkbookMensal w;
                if (Globals.ThisAddIn.Application.ActiveWorkbook != null &&
                    WorkbookMensal.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
                {
                }
                else return;


                var dir = w.NewaveBase;

                if (Directory.Exists(dir))
                {
                    dir = dir.EndsWith(Path.DirectorySeparatorChar.ToString()) ? dir.Remove(dir.Length - 1) : dir;
                }
                else
                    return;


                Func<string, string> clas = x =>
                {

                    var arr = x.ToLowerInvariant().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    var ord = "10";
                    for (int ordI = 0; ordI < arr.Length; ordI++)
                    {

                        var n = arr[ordI];
                        var m = System.Text.RegularExpressions.Regex.Match(n, "(?<=_)[+-]?\\d+");
                        if (m.Success) ord += (int.Parse(m.Value) + 50).ToString("00");
                        else
                        {
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
                    .Select(x => new
                    {
                        dir = x.Remove(0, dir.Length),
                        deck = DeckFactory.CreateDeck(x),
                    });

                var dNw = dirs.Where(x => x.deck is CommomLibrary.Newave.Deck)
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults(),
                        data = (new DirectoryInfo(x.dir)).Name

                    }).OrderBy(x => x.data).Where(x => x.result != null).ToList();


                int getPasso(string x)
                {
                    var m = System.Text.RegularExpressions.Regex.Match(x.Replace(dir, ""), @"_([-+]?\d+)\\");
                    if (m.Success)
                    {
                        return int.Parse(m.Groups[1].Value);
                    }
                    else return 0;
                }
                var dDc = dirs.Where(x => x.deck is CommomLibrary.Decomp.Deck).AsParallel()
                    .Select(x =>
                    {

                        var data = (new DirectoryInfo(x.dir)).Name;
                        return new
                        {
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


                for (var i = 0; i < dNw.Count; i++)
                {
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



                for (var i = 0; i < dDc.Count; i++)
                {
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

                for (int p = 0; p < passos.Length; p++)
                {
                    dcSECmoR[p + 1 + (passos.Length + 2) * 0, 0] =
                    dcSECmoR[p + 1 + (passos.Length + 2) * 1, 0] =
                    dcSECmoR[p + 1 + (passos.Length + 2) * 2, 0] =
                    dcSECmoR[p + 1 + (passos.Length + 2) * 3, 0] =
                    dcSECmoR[p + 1 + (passos.Length + 2) * 4, 0] = passos[p];
                }

                for (int d = 0; d < datas.Length; d++)
                {

                    dcSECmoR[(passos.Length + 2) * 0, d + 1] =
                    dcSECmoR[(passos.Length + 2) * 1, d + 1] =
                    dcSECmoR[(passos.Length + 2) * 2, d + 1] =
                    dcSECmoR[(passos.Length + 2) * 3, d + 1] =
                    dcSECmoR[(passos.Length + 2) * 4, d + 1] = datas[d];

                    for (int p = 0; p < passos.Length; p++)
                    {
                        var r = dDc.Where(x => x.data == datas[d] && x.passo == passos[p]).FirstOrDefault();
                        if (r != null)
                        {
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


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnDiagramaOper_Click(object sender, RibbonControlEventArgs e)
        {
            var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;
            try
            {
                var tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Projeto_Diagrama.xltx");
                WorkbookDiagramaOper w;

                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                    !WorkbookDiagramaOper.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
                {

                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                    WorkbookDiagramaOper.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w);
                }
                else
                {
                    switch (System.Windows.Forms.MessageBox.Show("Sobrescrever Atual?", "Decomp Tool - Diagrama", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question))
                    {
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

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    relatoA = DocumentFactory.Create(ofd.FileName) as Compass.CommomLibrary.Relato.Relato;
                    resultsA = DeckFactory.CreateDeck(Path.GetDirectoryName(ofd.FileName)).GetResults();

                }

                ofd.Title = "Deck B";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    relatoB = DocumentFactory.Create(ofd.FileName) as Compass.CommomLibrary.Relato.Relato;
                    resultsB = DeckFactory.CreateDeck(Path.GetDirectoryName(ofd.FileName)).GetResults();
                }

                Globals.ThisAddIn.Application.ScreenUpdating = false;
                Globals.ThisAddIn.Application.StatusBar = "Carregando diagrama...";
                w.Load(relatoA, relatoB, resultsA, resultsB);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.DisplayStatusBar = statusBarState;
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnCheckDecomp_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var info = ActiveWorkbook.GetInfosheet();
                if (info == null || !info.DocType.Equals("dadger", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Nenhum dadger carregado.");
                }

                var type = info.DocType;
                var doc = ActiveWorkbook.LoadDocumentFromWorkbook((string)type);
                doc.BottonComments = info.BottonComments;
                if (doc is Dadger)
                {

                    var incs = ((Dadger)doc).VerificarRestricoes();

                    info.WS.Cells[7, 1].Value = "Inconsistencias";

                    var i = 1;
                    foreach (var inc in incs)
                    {
                        info.WS.Cells[7 + i++, 1].Value = inc;



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

        private void btnInviab_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {
                var info = ActiveWorkbook.GetInfosheet();
                if (info == null || !info.DocType.Equals("dadger", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Nenhum dadger carregado.");
                }

                var type = info.DocType;
                var doc = ActiveWorkbook.LoadDocumentFromWorkbook((string)type);
                doc.BottonComments = info.BottonComments;
                doc.File = info.DocPath;


                var fi = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(doc.File), "inviab_unic.*", SearchOption.TopDirectoryOnly).FirstOrDefault();








                if (fi != null && doc is Dadger)
                {
                    var inviab = (Compass.CommomLibrary.Inviab.Inviab)DocumentFactory.Create(fi);

                    var deck = DeckFactory.CreateDeck(Path.GetDirectoryName(doc.File)) as Compass.CommomLibrary.Decomp.Deck;
                    deck[CommomLibrary.Decomp.DeckDocument.dadger].Document = doc;

                    Services.Deck.DesfazerInviabilidades(deck, inviab);

                    Globals.ThisAddIn.Application.ScreenUpdating = false;


                    ActiveWorkbook.WriteDocumentToWorkbook(doc);

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

        private void btnNWtoDC(object sender, RibbonControlEventArgs e)
        {

            var tfile = "";

            WorkbookMensal w;
            if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                !WorkbookMensal.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
            {

                tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Mensal6.xltm");
                Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                return;
            }
            else if (System.Windows.Forms.MessageBox.Show("Criar decks?\r\n" + "\r\nDestino: " + w.NewaveBase, "Decomp Tool - Mensal", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
            {


                if (System.Windows.Forms.MessageBox.Show("Novo Estudo? ", "Decomp Tool - Mensal", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Mensal5.xltm");
                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);
                }

                return;
            }

            var nw = w.NewaveOrigem;


            var deckNWEstudo = DeckFactory.CreateDeck(nw) as Compass.CommomLibrary.Newave.Deck;







        }
    }
}

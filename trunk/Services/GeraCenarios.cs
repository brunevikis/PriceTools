﻿using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.SistemaDat;
using Compass.ExcelTools;
using Compass.ExcelTools.Templates;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Compass.Services
{
    public class GeraCenarios
    {
        public static List<string> GeraMensal(WorkbookMensal w, string dc, string nw, bool auto = false)
        {
            List<string> consistFolders = new List<string>();
            if (auto == true)
            {

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
                    if (auto == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Nenhum caso newave encontrado");
                    }
                    return consistFolders;
                }

                var dadgerBase = deckDCBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger;

                #region verifica rests faixa limites decomp mensal
                string avisos = "";
                List<int> hqsErr = new List<int>();
                List<int> hvsErr = new List<int>();

                bool verificaRESTS = false;

                var limitesHQver = w.Faixalimites.Where(x => x.Ativa == true && x.TipoRest.ToUpper().Equals("HQ"));
                var limitesHVver = w.Faixalimites.Where(x => x.Ativa == true && x.TipoRest.ToUpper().Equals("HV"));

                foreach (var lim in limitesHQver)
                {
                    var restshq = dadgerBase.BlocoRhq.Where(x => x.Restricao == lim.CodRest);
                    if (restshq.Count() > 0)
                    {
                        var le = restshq.Where(x => x is Compass.CommomLibrary.Dadger.CqLine).Select(x => (Compass.CommomLibrary.Dadger.CqLine)x).First();
                        if (le.Usina == lim.UsiRest)
                        {
                            continue;
                        }
                        else if (hqsErr.All(x => x != lim.CodRest))
                        {
                            avisos = avisos + $"HQ {lim.CodRest} Usina Deck: {le.Usina} Usina Informada: {lim.UsiRest} \r\n";
                            hqsErr.Add(lim.CodRest);
                            verificaRESTS = true;
                        }
                    }
                }

                foreach (var lim in limitesHVver)
                {
                    var restshv = dadgerBase.BlocoRhv.Where(x => x.Restricao == lim.CodRest);
                    if (restshv.Count() > 0)
                    {
                        var le = restshv.Where(x => x is Compass.CommomLibrary.Dadger.CvLine).Select(x => (Compass.CommomLibrary.Dadger.CvLine)x).First();
                        if (le.Usina == lim.UsiRest)
                        {
                            continue;
                        }
                        else if (hvsErr.All(x => x != lim.CodRest))
                        {
                            avisos = avisos + $"HV {lim.CodRest} Usina Deck: {le.Usina} Usina Informada: {lim.UsiRest} \r\n";
                            hvsErr.Add(lim.CodRest);
                            verificaRESTS = true;
                        }
                    }
                }
                if (verificaRESTS == true && auto == false)
                {
                    if (System.Windows.Forms.MessageBox.Show($"Divergência de dados \r\nVerifique Deck Decomp de entrada! \r\n{avisos}\r\nDeseja continuar?"
               , "Faixa Limites", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        return consistFolders;
                    }

                }

                #endregion

                #region planMemoria de calculo

                string planMemo = Directory.GetFiles(w.NewaveOrigem).Where(x => Path.GetFileName(x).StartsWith("Memória de Cálculo", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                List<Tuple<int, int, DateTime, double>> eolicasDados = null;
                if (planMemo != null && File.Exists(planMemo))
                {
                    eolicasDados = getEolicasplan(planMemo);
                }

                #endregion
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

                    if (cenario.NomeCenario == "Hidrologia - 1" || cenario.CenarioNum == 1)
                    {
                        var dcNome = cenario.NomeDoEstudo;
                        var dcGNLnome = dcNome.Replace("DC", "DCGNL");
                        outPath = Path.Combine(w.NewaveBase, dcGNLnome);

                    }
                    Directory.CreateDirectory(outPath);


                    foreach (var dtEstudo in meses)
                    {

                        //Globals.ThisAddIn.Application.StatusBar = "Criando decks " + dtEstudo.ToString("MMM/yyyy");

                        var dtEstudoSeguinte = dtEstudo.AddMonths(1);

                        var estudoPath = Path.Combine(outPath, dtEstudo.ToString("yyyyMM"));
                        var nwPath = Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"));

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

                        //var patamares2019 = durPat1[0] > 0.15;
                        var patamares2019 = true;

                        bool patamares2023 = w.patamares2023;
                        bool patamares2024 = false;
                        bool patamares2025 = false;
                        patamares2024 = dtEstudo.Year == 2024;
                        patamares2025 = dtEstudo.Year >= 2025;

                        if (patamares2024)
                        {
                            patamares2023 = false;
                        }

                        MesOperativo mesOperativo = MesOperativo.CreateMensal(dtEstudo.Year, dtEstudo.Month, patamares2019, patamares2023, patamares2024, patamares2025);

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
                            dadger = Services.DecompNextRev.CreateRv0(deckEstudo, deckNWEstudo, dtEstudo, w, mesOperativo, pmoBase, eolicasDados, w.NwHibrido);
                            dadger.SaveToFile(createBackup: true);

                            #region csvlibs
                            //Services.DecompNextRev.IncrementaLibsCSV(deckEstudo, deckNWEstudo, dtEstudo);

                            #endregion

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
                                        fixaUhTxtLines.Add(df.Posto.ToString() + "\t" + (df.Volini != null ? df.Volini.ToString() : "null"));
                                    }
                                    File.WriteAllLines(Path.Combine(estudoPath, "UhFixados.txt"), fixaUhTxtLines);

                                    Services.Reservatorio.SetUHBlockFixado(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax, dadosFixas, auto);
                                }
                                else
                                {
                                    Services.Reservatorio.SetUHBlock(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax, auto);
                                }

                            }
                            catch (Exception ex)
                            {
                                ex.ToString();

                            }
                            #endregion
                            configH.baseDoc.SaveToFile();

                            File.WriteAllText(Path.Combine(estudoPath, "configh.dat"), earmconfig);

                            #region armazenamento REE

                            var hidrDatNew = deckEstudo[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat;

                            var configHNew = new Compass.CommomLibrary.Decomp.ConfigH(configH.baseDoc as Dadger, hidrDatNew);
                            //var dadosREE = configH.GetREEList();
                            var dadosREE = configHNew.GetREEList();

                            // configH.ReloadUH();
                            configHNew.ReloadUH();

                            List<string> linhasREE = new List<string>();

                            linhasREE.Add("REE\tEARM%");
                            foreach (var dadR in dadosREE)
                            {
                                linhasREE.Add($"{ dadR.Item1}\t{dadR.Item2}%");
                            }
                            File.WriteAllLines(Path.Combine(estudoPath, "REE_EARM.txt"), linhasREE);

                            var dgerFile = Directory.GetFiles(nwPath).Where(x => Path.GetFileName(x).ToLower().Equals("dger.dat")).FirstOrDefault();
                            if (dgerFile != null)
                            {
                                var dger = (Compass.CommomLibrary.DgerDat.DgerDat)DocumentFactory.Create(dgerFile);

                                var earmsREE = new double[dadosREE.Count()];
                                int i = 0;
                                foreach (var dadR in dadosREE)
                                {
                                    earmsREE[i] = Math.Round(dadR.Item2, 1);
                                    i++;
                                }

                                if (w.NwHibrido == true)
                                {
                                    int[] sf = new int[] { 1, 1 };
                                    dger.SetaSimulacaoFinal = sf;
                                    dger.CalculaEarmInicial = true;
                                }

                                dger.Earms = earmsREE;
                                dger.SaveToFile();
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

                            File.WriteAllText(Path.Combine(estudoPath, "configm.dat"), config2);


                            configs[dtEstudo] = new Tuple<string, string>(earmconfig, config2);

                            #endregion Armazenamento

                            dadgers[dtEstudo] = configH.baseDoc as Dadger;

                            #region Atualiza Confhd

                            try
                            {
                                string NWdestino = deckNWEstudo.BaseFolder;

                                var confihdFile = Directory.GetFiles(NWdestino).Where(x => Path.GetFileName(x).ToLower().Equals("confhd.dat")).FirstOrDefault();

                                if (confihdFile != null)
                                {
                                    var confihdNew = (Compass.CommomLibrary.ConfhdDat.ConfhdDat)DocumentFactory.Create(confihdFile);
                                    var dadgerRef = configH.baseDoc as Dadger;
                                    var dadgertest = configHNew.baseDoc as Dadger;
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
                                    confihdNew.SaveToFile();
                                }
                            }
                            catch (Exception ehd)
                            {
                                throw;

                            }
                            #region testesnew earmree
                            //var hidrDatNew = deckEstudo[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat;

                            //var configHNew = new Compass.CommomLibrary.Decomp.ConfigH(configH.baseDoc as Dadger, hidrDatNew);

                            //var dadosREENew = configHNew.GetREEList();

                            //configHNew.ReloadUH();

                            //List<string> linhasREENew = new List<string>();

                            //linhasREENew.Add("REE\tEARM%");
                            //foreach (var dadR in dadosREENew)
                            //{
                            //    linhasREENew.Add($"{ dadR.Item1}\t{dadR.Item2}%");
                            //}

                            //File.WriteAllLines(Path.Combine(estudoPath, "REE_EARMNew.txt"), linhasREENew);




                            #endregion

                            #endregion

                            #region faixa limites de restrição antiga

                            ////codigofaixaslimites aqui
                            //if (w.Faixapercents != null && w.Faixalimites != null)//if (w.Faixapercents.Count() > 0 && w.Faixalimites.Count() > 0)
                            //{
                            //    DateTime mesSeg = dtEstudo.AddMonths(1);
                            //    dadger = configH.baseDoc as Dadger;
                            //    // configH = new Compass.CommomLibrary.Decomp.ConfigH(dadger, hidrDat);

                            //    var limitesHQ = w.Faixalimites.Where(x => x.MesIni <= dtEstudo.Month && x.MesFim >= dtEstudo.Month && x.Ativa == true && x.TipoRest.ToUpper().Equals("HQ"));
                            //    var limitesHV = w.Faixalimites.Where(x => x.MesIni <= dtEstudo.Month && x.MesFim >= dtEstudo.Month && x.Ativa == true && x.TipoRest.ToUpper().Equals("HV"));

                            //    bool nwhibrido = w.NwHibrido;

                            //    Compass.CommomLibrary.ModifDatNW.ModifDatNw modif;

                            //    //string redatBase = Path.Combine(deckNWEstudo.BaseFolder, "re_base.dat");
                            //    //var redatFile = Directory.GetFiles(deckNWEstudo.BaseFolder).Where(x => Path.GetFileName(x).ToLower().Equals("re.dat")).FirstOrDefault();
                            //    //if (redatFile != null && !File.Exists(redatBase))
                            //    //{
                            //    //    File.Copy(redatFile, redatBase, true);
                            //    //}

                            //    if (limitesHQ.Count() > 0)
                            //    {

                            //        foreach (var lHq in limitesHQ)
                            //        {
                            //            // dynamic lq;
                            //            var UH = dadger.BlocoUh.Where(x => x.Usina == lHq.UH.First()).FirstOrDefault();

                            //            double produt65 = configH.Usinas.Any(x => x.Cod == lHq.UsiRest) ? configH.Usinas.Where(x => x.Cod == lHq.UsiRest).Select(x => x.Prod65VolUtil).First() : -1;// -1 para ocaso de não encontrar o dado referente a usina da restrição

                            //            if (UH != null)
                            //            {

                            //                var rests = dadger.BlocoRhq.Where(x => x.Restricao == lHq.CodRest);

                            //                double percentAlvo = UH.VolIniPerc;
                            //                if (lHq.UH.Count() > 1)
                            //                {
                            //                    percentAlvo = Services.DecompNextRev.GetpercentAlvo(configH, lHq.UH);
                            //                }

                            //                if (rests.Count() > 0)
                            //                {
                            //                    var le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LqLine).Select(x => (Compass.CommomLibrary.Dadger.LqLine)x);
                            //                    dynamic lqdummy = le.Where(x => x.Estagio <= 2).OrderByDescending(x => x.Estagio).FirstOrDefault();

                            //                    if (lqdummy.Estagio < 2)//caso não exista o estagio do segundo mes informado, copia os dados do ultimo estagio informado para o segundo mes
                            //                    {

                            //                        var nledummy = lqdummy.Clone();
                            //                        nledummy.Estagio = 2;
                            //                        dadger.BlocoRhq.Add(nledummy);
                            //                    }

                            //                    rests = dadger.BlocoRhq.Where(x => x.Restricao == lHq.CodRest);
                            //                    le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LqLine).Select(x => (Compass.CommomLibrary.Dadger.LqLine)x);
                            //                    var lqs = le.Where(x => x.Estagio <= 2).ToList();

                            //                    if (lqs.Count() > 0)
                            //                    {
                            //                        foreach (var lq in lqs)
                            //                        {
                            //                            modif = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
                            //                            var modifFile = modif.File;

                            //                            var reDat = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.re].Document as Compass.CommomLibrary.ReDat.ReDat;

                            //                            DateTime data;
                            //                            data = new DateTime(dtEstudo.Year, dtEstudo.Month, 1);

                            //                            double valor = 0;
                            //                            valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHq, w.Faixapercents.First());

                            //                            if (lq.Estagio == 2)
                            //                            {
                            //                                data = mesSeg;
                            //                                var lHqSEg = w.Faixalimites.Where(x => x.MesIni <= mesSeg.Month && x.MesFim >= mesSeg.Month && x.Ativa == true && x.UsiRest == lHq.UsiRest && x.UH.All(lHq.UH.Contains) && x.UH.Count == lHq.UH.Count && x.InfSup == lHq.InfSup && x.TipoRest.ToUpper().Equals("HQ")).FirstOrDefault();
                            //                                if (lHqSEg != null)
                            //                                {
                            //                                    valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHqSEg, w.Faixapercents.First());
                            //                                }
                            //                            }

                            //                            if (lHq.InfSup == "SUP")
                            //                            {
                            //                                lq[4] = valor < lq[3] ? lq[3] : valor;
                            //                                lq[6] = valor < lq[3] ? lq[3] : valor;
                            //                                lq[8] = valor < lq[3] ? lq[3] : valor;

                            //                                if (produt65 >= 0 && nwhibrido == false) // alteração do arquivo re.dat caso necessario e nw NÃO hibrido
                            //                                {
                            //                                    double restValor = lq[4];
                            //                                    foreach (var reRest in reDat.Restricoes.ToList())
                            //                                    {
                            //                                        foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == reRest.Numero).ToList())
                            //                                        {

                            //                                            if (reDet.Inicio < deckNWEstudo.Dger.DataEstudo && reDet.Fim >= deckNWEstudo.Dger.DataEstudo)
                            //                                            {
                            //                                                reDet.Inicio = deckNWEstudo.Dger.DataEstudo;
                            //                                            }
                            //                                            else if (reDet.Fim < deckNWEstudo.Dger.DataEstudo)
                            //                                            {
                            //                                                reDat.Detalhes.Remove(reDet);
                            //                                            }
                            //                                        }

                            //                                        if (reDat.Detalhes.Where(x => x.Numero == reRest.Numero).Count() == 0) reDat.Restricoes.Remove(reRest);
                            //                                    }
                            //                                    //procura restricao
                            //                                    var re = reDat.Restricoes.Where(
                            //                                        x => String.Join("", x.Valores.Skip(1).Where(y => y != null).OrderBy(y => y).Select(y => y.ToString().Trim()))
                            //                                            == String.Join("", lHq.UsiRest.ToString())
                            //                                        ).FirstOrDefault();

                            //                                    //se nao exite insere
                            //                                    if (re == null)
                            //                                    {

                            //                                        re = new Compass.CommomLibrary.ReDat.ReLine()
                            //                                        {
                            //                                            Numero = reDat.Restricoes.Max(x => x.Numero) + 1
                            //                                        };

                            //                                        re[1] = lHq.UsiRest;

                            //                                        reDat.Restricoes.Add(re);


                            //                                        var val = new Compass.CommomLibrary.ReDat.ReValLine()
                            //                                        {
                            //                                            Numero = re.Numero,
                            //                                            Patamar = 0,
                            //                                            ValorRestricao = restValor * produt65,
                            //                                            Inicio = data,
                            //                                            Fim = data,
                            //                                        };

                            //                                        reDat.Detalhes.Add(val);

                            //                                    }
                            //                                    //altera ou insere novo valor
                            //                                    else
                            //                                    {

                            //                                        var val = new Compass.CommomLibrary.ReDat.ReValLine()
                            //                                        {
                            //                                            Numero = re.Numero,
                            //                                            Patamar = 0,
                            //                                            ValorRestricao = restValor * produt65,
                            //                                            Inicio = data,
                            //                                            Fim = data,
                            //                                        };

                            //                                        var anterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                            //                                            .Where(x => x.Inicio < val.Inicio && x.Fim >= val.Inicio).FirstOrDefault();
                            //                                        var posterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                            //                                            .Where(x => x.Inicio <= val.Fim && x.Fim > val.Fim).FirstOrDefault();

                            //                                        if (anterior != null)
                            //                                        {
                            //                                            var anteriorSplit = anterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                            //                                            anterior.Inicio = val.Inicio;
                            //                                            anteriorSplit.Fim = val.Inicio.AddMonths(-1);

                            //                                            reDat.Detalhes.Add(anteriorSplit);
                            //                                        }

                            //                                        if (posterior != null)
                            //                                        {
                            //                                            var posteriorSplit = posterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                            //                                            posterior.Fim = val.Fim; ;
                            //                                            posteriorSplit.Inicio = val.Fim.AddMonths(1);

                            //                                            reDat.Detalhes.Add(posteriorSplit);
                            //                                        }

                            //                                        reDat.Detalhes.Where(x => x.Numero == val.Numero)
                            //                                            .Where(x => x.Inicio >= val.Inicio && x.Fim <= val.Fim).ToList().ForEach(x =>
                            //                                                reDat.Detalhes.Remove(x)
                            //                                                );

                            //                                        reDat.Detalhes.Add(val);
                            //                                    }
                            //                                    var newl = reDat.Detalhes.OrderBy(x => x.Numero).ThenBy(x => x.Inicio).ToList();
                            //                                    reDat.Detalhes.Clear();
                            //                                    newl.ForEach(x => reDat.Detalhes.Add(x));
                            //                                    reDat.SaveToFile();
                            //                                }
                            //                                else // alterar modif com turbmaxt
                            //                                {
                            //                                    if (nwhibrido)
                            //                                    {
                            //                                        if (!modif.Any(x => x.Usina == lHq.UsiRest))
                            //                                        {
                            //                                            modif.Add(new Compass.CommomLibrary.ModifDatNW.ModifLine()
                            //                                            {
                            //                                                Usina = lHq.UsiRest,
                            //                                                Chave = "USINA",
                            //                                                NovosValores = new string[] { lHq.UsiRest.ToString() }
                            //                                            });

                            //                                        }
                            //                                        var modiflineTurb = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "TURBMAXT" && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();

                            //                                        if (modiflineTurb != null)
                            //                                        {
                            //                                            if (modiflineTurb.DataModif < data)
                            //                                            {

                            //                                                var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                                var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                                var valorAntigo = modiflineTurb.ValorModif;


                            //                                                newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));
                            //                                                newModifLine.Chave = "TURBMAXT";
                            //                                                newModifLine.Usina = lHq.UsiRest;
                            //                                                int index = modif.IndexOf(modiflineTurb) + 1;
                            //                                                modif.Insert(index, newModifLine);

                            //                                                //mes seguinte verificação
                            //                                                var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "TURBMAXT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                            //                                                if (modiflineMesSeq == null)
                            //                                                {
                            //                                                    //newModifLine2 = modifline;
                            //                                                    newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                            //                                                    //newModifLine2.DataModif = data.AddMonths(1);
                            //                                                    newModifLine2.Chave = "TURBMAXT";
                            //                                                    newModifLine2.Usina = lHq.UsiRest;
                            //                                                    int index2 = modif.IndexOf(newModifLine) + 1;
                            //                                                    modif.Insert(index2, newModifLine2);
                            //                                                }


                            //                                            }
                            //                                            else
                            //                                            {
                            //                                                var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                                var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                                var valorAntigo = modiflineTurb.ValorModif;

                            //                                                modiflineTurb.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));

                            //                                                //mes seguinte verificação
                            //                                                var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "TURBMAXT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                            //                                                if (modiflineMesSeq == null)
                            //                                                {
                            //                                                    //newModifLine2 = modifline;
                            //                                                    newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                            //                                                    //newModifLine2.DataModif = data.AddMonths(1);
                            //                                                    newModifLine2.Chave = "TURBMAXT";
                            //                                                    newModifLine2.Usina = lHq.UsiRest;
                            //                                                    int index2 = modif.IndexOf(modiflineTurb) + 1;
                            //                                                    modif.Insert(index2, newModifLine2);
                            //                                                }

                            //                                            }
                            //                                        }
                            //                                        else
                            //                                        {
                            //                                            var mod = modif.Where(x => x.Usina == lHq.UsiRest).FirstOrDefault();
                            //                                            if (mod != null)
                            //                                            {
                            //                                                var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                            //                                                newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));
                            //                                                newModifLine.Chave = "TURBMAXT";
                            //                                                newModifLine.Usina = lHq.UsiRest;
                            //                                                int indexT = modif.IndexOf(mod) + 1;
                            //                                                modif.Insert(indexT, newModifLine);
                            //                                            }
                            //                                        }
                            //                                    }
                            //                                }
                            //                            }
                            //                            else
                            //                            {
                            //                                lq[3] = valor > lq[4] ? lq[4] : valor;
                            //                                lq[5] = valor > lq[4] ? lq[4] : valor;
                            //                                lq[7] = valor > lq[4] ? lq[4] : valor;
                            //                            }

                            //                            /////////

                            //                            var modifline = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                            //                            if (lHq.InfSup == "INF" && lq[3] != null)
                            //                            {
                            //                                double modifval = lq[3];
                            //                                if (modifline != null)
                            //                                {
                            //                                    if (modifline.DataModif < data)
                            //                                    {

                            //                                        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                        var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                        var valorAntigo = modifline.ValorModif;


                            //                                        newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));
                            //                                        newModifLine.Chave = "VAZMINT";
                            //                                        newModifLine.Usina = lHq.UsiRest;
                            //                                        int index = modif.IndexOf(modifline) + 1;
                            //                                        modif.Insert(index, newModifLine);

                            //                                        //mes seguinte verificação
                            //                                        var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                            //                                        if (modiflineMesSeq == null)
                            //                                        {
                            //                                            //newModifLine2 = modifline;
                            //                                            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                            //                                            //newModifLine2.DataModif = data.AddMonths(1);
                            //                                            newModifLine2.Chave = "VAZMINT";
                            //                                            newModifLine2.Usina = lHq.UsiRest;
                            //                                            int index2 = modif.IndexOf(newModifLine) + 1;
                            //                                            modif.Insert(index2, newModifLine2);
                            //                                        }


                            //                                    }
                            //                                    else
                            //                                    {
                            //                                        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                        var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                        var valorAntigo = modifline.ValorModif;

                            //                                        modifline.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));

                            //                                        //mes seguinte verificação
                            //                                        var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                            //                                        if (modiflineMesSeq == null)
                            //                                        {
                            //                                            //newModifLine2 = modifline;
                            //                                            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                            //                                            //newModifLine2.DataModif = data.AddMonths(1);
                            //                                            newModifLine2.Chave = "VAZMINT";
                            //                                            newModifLine2.Usina = lHq.UsiRest;
                            //                                            int index2 = modif.IndexOf(modifline) + 1;
                            //                                            modif.Insert(index2, newModifLine2);
                            //                                        }

                            //                                    }
                            //                                }
                            //                                else
                            //                                {
                            //                                    var mod = modif.Where(x => x.Usina == lHq.UsiRest).FirstOrDefault();
                            //                                    if (mod != null)
                            //                                    {
                            //                                        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                            //                                        newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));
                            //                                        newModifLine.Chave = "VAZMINT";
                            //                                        newModifLine.Usina = lHq.UsiRest;
                            //                                        int index = modif.IndexOf(mod) + 1;
                            //                                        modif.Insert(index, newModifLine);
                            //                                    }


                            //                                }
                            //                            }

                            //                            modif.SaveToFile(filePath: modifFile);
                            //                            /////////
                            //                        }

                            //                    }
                            //                }


                            //            }
                            //        }
                            //    }
                            //    if (limitesHV.Count() > 0)
                            //    {
                            //        foreach (var lHv in limitesHV)
                            //        {
                            //            // dynamic lq;

                            //            var UH = dadger.BlocoUh.Where(x => x.Usina == lHv.UH.First()).FirstOrDefault();

                            //            double hectoMin = configH.Usinas.Any(x => x.Cod == lHv.UsiRest) ? configH.Usinas.Where(x => x.Cod == lHv.UsiRest).Select(x => x.VolMin).First() : -1;

                            //            if (UH != null)
                            //            {

                            //                var rests = dadger.BlocoRhv.Where(x => x.Restricao == lHv.CodRest);

                            //                double percentAlvo = UH.VolIniPerc;

                            //                if (lHv.UH.Count() > 1)
                            //                {
                            //                    percentAlvo = Services.DecompNextRev.GetpercentAlvo(configH, lHv.UH);
                            //                }

                            //                if (rests.Count() > 0)
                            //                {
                            //                    var le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LvLine).Select(x => (Compass.CommomLibrary.Dadger.LvLine)x);
                            //                    // var lvs = le.Where(x => x.Estagio <= dadger.VAZOES_NumeroDeSemanas).ToList();
                            //                    dynamic lvdummy = le.Where(x => x.Estagio <= 2).OrderByDescending(x => x.Estagio).FirstOrDefault();

                            //                    if (lvdummy.Estagio < 2)//caso não exista o estagio do segundo mes informado, copia os dados do ultimo estagio informado para o segundo mes
                            //                    {

                            //                        var nledummy = lvdummy.Clone();
                            //                        nledummy.Estagio = 2;
                            //                        dadger.BlocoRhv.Add(nledummy);
                            //                    }

                            //                    rests = dadger.BlocoRhv.Where(x => x.Restricao == lHv.CodRest);
                            //                    le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LvLine).Select(x => (Compass.CommomLibrary.Dadger.LvLine)x);
                            //                    var lvs = le.Where(x => x.Estagio <= 2).ToList();

                            //                    if (lvs.Count() > 0)
                            //                    {
                            //                        foreach (var lv in lvs)
                            //                        {
                            //                            modif = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
                            //                            var modifFile = modif.File;

                            //                            string minemonico = "";
                            //                            double valorTemp;

                            //                            DateTime data;
                            //                            data = new DateTime(dtEstudo.Year, dtEstudo.Month, 1);

                            //                            double valor = 0;
                            //                            valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHv, w.Faixapercents.First());

                            //                            if (lv.Estagio == 2)
                            //                            {
                            //                                data = mesSeg;
                            //                                var lHvSEg = w.Faixalimites.Where(x => x.MesIni <= mesSeg.Month && x.MesFim >= mesSeg.Month && x.Ativa == true && x.UsiRest == lHv.UsiRest && x.UH.All(lHv.UH.Contains) && x.UH.Count == lHv.UH.Count && x.InfSup == lHv.InfSup && x.TipoRest.ToUpper().Equals("HV")).FirstOrDefault();
                            //                                if (lHvSEg != null)
                            //                                {
                            //                                    valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHvSEg, w.Faixapercents.First());
                            //                                }
                            //                            }

                            //                            if (lHv.InfSup == "SUP")
                            //                            {
                            //                                lv[4] = valor < lv[3] ? lv[3] : valor;
                            //                                minemonico = "VMAXT";
                            //                                valorTemp = lv[4];
                            //                            }
                            //                            else
                            //                            {
                            //                                lv[3] = valor > lv[4] ? lv[4] : valor;
                            //                                minemonico = "VMINT";
                            //                                valorTemp = lv[3];
                            //                            }

                            //                            /////////

                            //                            if (hectoMin >= 0)
                            //                            {
                            //                                var modifline = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                            //                                double modifval = valorTemp + hectoMin;
                            //                                if (modifline != null)
                            //                                {
                            //                                    if (modifline.DataModif < data)
                            //                                    {

                            //                                        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                        var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                        var valorAntigo = modifline.ValorModif;


                            //                                        newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");
                            //                                        newModifLine.Chave = minemonico;
                            //                                        newModifLine.Usina = lHv.UsiRest;
                            //                                        int index = modif.IndexOf(modifline) + 1;
                            //                                        modif.Insert(index, newModifLine);

                            //                                        //mes seguinte verificação
                            //                                        var modiflineMesSeq = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                            //                                        if (modiflineMesSeq == null)
                            //                                        {
                            //                                            //newModifLine2 = modifline;
                            //                                            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'), "'h'");
                            //                                            //newModifLine2.DataModif = data.AddMonths(1);
                            //                                            newModifLine2.Chave = minemonico;
                            //                                            newModifLine2.Usina = lHv.UsiRest;
                            //                                            int index2 = modif.IndexOf(newModifLine) + 1;
                            //                                            modif.Insert(index2, newModifLine2);
                            //                                        }


                            //                                    }
                            //                                    else
                            //                                    {
                            //                                        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                        var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            //                                        var valorAntigo = modifline.ValorModif;

                            //                                        modifline.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");

                            //                                        //mes seguinte verificação
                            //                                        var modiflineMesSeq = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                            //                                        if (modiflineMesSeq == null)
                            //                                        {
                            //                                            //newModifLine2 = modifline;
                            //                                            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'), "'h'");
                            //                                            //newModifLine2.DataModif = data.AddMonths(1);
                            //                                            newModifLine2.Chave = minemonico;
                            //                                            newModifLine2.Usina = lHv.UsiRest;
                            //                                            int index2 = modif.IndexOf(modifline) + 1;
                            //                                            modif.Insert(index2, newModifLine2);
                            //                                        }

                            //                                    }
                            //                                }
                            //                                else
                            //                                {
                            //                                    var mod = modif.Where(x => x.Usina == lHv.UsiRest).FirstOrDefault();
                            //                                    if (mod != null)
                            //                                    {
                            //                                        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                            //                                        newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");
                            //                                        newModifLine.Chave = minemonico;
                            //                                        newModifLine.Usina = lHv.UsiRest;
                            //                                        int index = modif.IndexOf(mod) + 1;
                            //                                        modif.Insert(index, newModifLine);
                            //                                    }


                            //                                }

                            //                                modif.SaveToFile(filePath: modifFile);
                            //                            }

                            //                            /////////

                            //                        }

                            //                    }
                            //                }



                            //            }
                            //        }
                            //    }
                            //    dadger.SaveToFile();

                            //    if (w.Faixapercents.Count() > 0 && w.Faixalimites.Count() > 0)
                            //    {
                            //        List<string> faixaText = new List<string>();
                            //        string header = "UH;TIPO REST;USINA REST;COD REST;MÊS INI;MÊS FIM;INF/SUP;ATIVA";
                            //        w.Faixapercents.First().Percents.ForEach(x => header = header + ";" + x.ToString() + "%");

                            //        faixaText.Add(header);
                            //        w.Faixalimites.ForEach(x =>
                            //        {
                            //            string linha;
                            //            linha = string.Join(";", x.UHstring, x.TipoRest, x.UsiRest.ToString(), x.CodRest.ToString(), x.MesIni.ToString(), x.MesFim.ToString(), x.InfSup.ToString(), x.Ativa.ToString()) + ";";
                            //            linha = linha + string.Join(";", x.Vals.ToList());
                            //            faixaText.Add(linha);
                            //            // x.Vals.ForEach(y => { linha = linha + y.ToString(); });
                            //        });
                            //        File.WriteAllLines(Path.Combine(estudoPath, "LIMITES_DE_RESTRICAO.txt"), faixaText);

                            //        string destino = deckNWEstudo.BaseFolder;



                            //        var dgerFileconsist = Directory.GetFiles(destino).Where(x => Path.GetFileName(x).ToLower().Equals("dger.dat")).FirstOrDefault();
                            //        if (dgerFileconsist != null)
                            //        {


                            //            var dger = (Compass.CommomLibrary.DgerDat.DgerDat)DocumentFactory.Create(dgerFileconsist);


                            //            var earmsREE = new double[dadosREE.Count()];
                            //            int r = 0;
                            //            foreach (var dadR in dadosREE)
                            //            {
                            //                earmsREE[r] = Math.Round(dadR.Item2, 1);
                            //                if (earmsREE[r] > 100.0f)
                            //                {
                            //                    earmsREE[r] = 100.0f;
                            //                }
                            //                if (earmsREE[r] < 0)
                            //                {
                            //                    earmsREE[r] = 0;
                            //                }
                            //                r++;
                            //            }
                            //            //for (int i = 0; i < dger.Earms.Count(); i++)
                            //            //{
                            //            //    if (dger.Earms[i] > 100.0f)
                            //            //    {
                            //            //        dger.Earms[i] = 100.0f;
                            //            //    }
                            //            //    if (dger.Earms[i] < 0)
                            //            //    {
                            //            //        dger.Earms[i] = 0;
                            //            //    }

                            //            //}

                            //            dger.Earms = earmsREE;
                            //            dger.SaveToFile();
                            //        }
                            //        if (estudoPath.Contains("DCGNL"))
                            //        {
                            //            consistFolders.Add(destino);
                            //        }

                            //        //var ret = Compass.Services.Linux.Run2(destino, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/newaveCons280003.sh 3", "NewaveConsist", true, true, "hide");// para debug usar essa funçao

                            //        ////var ret = Compass.Services.Linux.Run(destino, w.ExecutavelNewave + " 3", "NewaveConsist", true, true, "hide");
                            //        //if (!ret)
                            //        //{
                            //        //    throw new Exception("Ocorreu erro na criação e consistência dos decks newaves. Verifique.");
                            //        //}

                            //        //Compass.Services.Deck.CreateDgerNewdesp(destino);




                            //    }

                            //}

                            ////fim codigofaixas limites

                            #endregion

                            #region faixa limites nova

                            #region faixa limites de restrição

                            //codigofaixaslimites aqui
                            if (w.Faixapercents != null && w.Faixalimites != null)//if (w.Faixapercents.Count() > 0 && w.Faixalimites.Count() > 0)
                            {
                                DateTime mesSeg = dtEstudo.AddMonths(1);
                                dadger = configH.baseDoc as Dadger;

                                var limitesHQ = w.Faixalimites.Where(x => x.MesIni <= dtEstudo.Month && x.MesFim >= dtEstudo.Month && x.Ativa == true && x.TipoRest.ToUpper().Equals("HQ") && (x.Minemonico == "QDEF" || x.Minemonico == "QTUR"));
                                var limitesHV = w.Faixalimites.Where(x => x.MesIni <= dtEstudo.Month && x.MesFim >= dtEstudo.Month && x.Ativa == true && x.TipoRest.ToUpper().Equals("HV"));

                                bool nwhibrido = w.NwHibrido;

                                Compass.CommomLibrary.ModifDatNW.ModifDatNw modif;



                                if (limitesHQ.Count() > 0)
                                {

                                    foreach (var lHq in limitesHQ)
                                    {
                                        // dynamic lq;
                                        var UH = dadger.BlocoUh.Where(x => x.Usina == lHq.UH.First()).FirstOrDefault();

                                        double produt65 = configH.Usinas.Any(x => x.Cod == lHq.UsiRest) ? configH.Usinas.Where(x => x.Cod == lHq.UsiRest).Select(x => x.Prod65VolUtil).First() : -1;// -1 para ocaso de não encontrar o dado referente a usina da restrição
                                        double engolimento = configH.Usinas.Any(x => x.Cod == lHq.UsiRest) ? configH.Usinas.Where(x => x.Cod == lHq.UsiRest).Select(x => x.Engolimento).First() : -1;// -1 para ocaso de não encontrar o dado referente a usina da restrição

                                        if (UH != null)
                                        {

                                            var restPorUsina = dadger.BlocoRhq.RhqGrouped.Where(rh =>
                                                 rh.Value.Where(x => x is CqLine).All(x => ((CqLine)x).Usina == lHq.UsiRest && (((CqLine)x).Tipo == "QDEF" || ((CqLine)x).Tipo == "QTUR"))
                                                ).Select(x => x.Value).ToList();

                                            var restPorUsinaQdefs = dadger.BlocoRhq.RhqGrouped.Where(rh =>
                                                 rh.Value.Where(x => x is CqLine).All(x => ((CqLine)x).Usina == lHq.UsiRest && ((CqLine)x).Tipo == "QDEF")
                                                ).Select(x => x.Value).ToList();

                                            var restPorUsinaQturs = dadger.BlocoRhq.RhqGrouped.Where(rh =>
                                                 rh.Value.Where(x => x is CqLine).All(x => ((CqLine)x).Usina == lHq.UsiRest && ((CqLine)x).Tipo == "QTUR")
                                                ).Select(x => x.Value).ToList();

                                            List<List<RhqLine>> restsAlvo = new List<List<RhqLine>>();
                                            List<List<RhqLine>> restsAnalisar = new List<List<RhqLine>>();

                                            if (lHq.Minemonico == "QTUR")
                                            {
                                                restsAlvo = restPorUsinaQturs;
                                                restsAnalisar = restPorUsinaQdefs;
                                            }
                                            else
                                            {
                                                restsAlvo = restPorUsinaQdefs;
                                                restsAnalisar = restPorUsinaQturs;
                                            }

                                            if (restsAlvo.Count() == 0)
                                            {
                                                var rest = new List<RhqLine>();
                                                rest.Add(new HqLine()
                                                {
                                                    Restricao = dadger.BlocoRhq.GetNextId(),
                                                    Inicio = 1,
                                                    Fim = mesOperativo.Estagios + 1

                                                });

                                                rest.Add(new LqLine() { Restricao = rest.First().Restricao, Estagio = 1 });
                                                rest.Add(new CqLine() { Restricao = rest.First().Restricao, Usina = lHq.UsiRest, Tipo = lHq.Minemonico });

                                                rest.ForEach(x => dadger.BlocoRhq.Add(x));

                                                restsAlvo.Add(rest);
                                            }

                                            foreach (var rA in restsAlvo)
                                            {
                                                int restNumber = rA.First().Restricao;

                                                //var rests = dadger.BlocoRhq.Where(x => x.Restricao == lHq.CodRest);
                                                var rests = dadger.BlocoRhq.Where(x => x.Restricao == restNumber);

                                                double percentAlvo = UH.VolIniPerc;
                                                if (lHq.UH.Count() > 1)
                                                {
                                                    percentAlvo = Services.DecompNextRev.GetpercentAlvo(configH, lHq.UH);
                                                }

                                                if (rests.Count() > 0)
                                                {
                                                    var le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LqLine).Select(x => (Compass.CommomLibrary.Dadger.LqLine)x);
                                                    dynamic lqdummy = le.Where(x => x.Estagio <= 2).OrderByDescending(x => x.Estagio).FirstOrDefault();

                                                    if (lqdummy.Estagio < 2)//caso não exista o estagio do segundo mes informado, copia os dados do ultimo estagio informado para o segundo mes
                                                    {

                                                        var nledummy = lqdummy.Clone();
                                                        nledummy.Estagio = 2;
                                                        dadger.BlocoRhq.Add(nledummy);
                                                    }

                                                    //rests = dadger.BlocoRhq.Where(x => x.Restricao == lHq.CodRest);
                                                    rests = dadger.BlocoRhq.Where(x => x.Restricao == restNumber);
                                                    le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LqLine).Select(x => (Compass.CommomLibrary.Dadger.LqLine)x);
                                                    var lqs = le.Where(x => x.Estagio <= 2).ToList();

                                                    if (lqs.Count() > 0)
                                                    {
                                                        foreach (var lq in lqs)
                                                        {
                                                            modif = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
                                                            var modifFile = modif.File;

                                                            var reDat = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.re].Document as Compass.CommomLibrary.ReDat.ReDat;

                                                            DateTime data;
                                                            data = new DateTime(dtEstudo.Year, dtEstudo.Month, 1);

                                                            double valor = 0;
                                                            valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHq, w.Faixapercents.First());

                                                            if (lq.Estagio == 2)
                                                            {
                                                                data = mesSeg;
                                                                //var lHqSEg = w.Faixalimites.Where(x => x.MesIni <= mesSeg.Month && x.MesFim >= mesSeg.Month && x.Ativa == true && x.UsiRest == lHq.UsiRest && x.UH.All(lHq.UH.Contains) && x.UH.Count == lHq.UH.Count && x.InfSup == lHq.InfSup && x.TipoRest.ToUpper().Equals("HQ")).FirstOrDefault();
                                                                var lHqSEg = w.Faixalimites.Where(x => x.MesIni <= mesSeg.Month && x.MesFim >= mesSeg.Month && x.Ativa == true && x.UsiRest == lHq.UsiRest && x.UH.All(lHq.UH.Contains) && x.UH.Count == lHq.UH.Count && x.InfSup == lHq.InfSup && x.TipoRest.ToUpper().Equals("HQ") && x.Minemonico == lHq.Minemonico).FirstOrDefault();
                                                                if (lHqSEg != null)
                                                                {
                                                                    valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHqSEg, w.Faixapercents.First());
                                                                }
                                                            }

                                                            if (lHq.InfSup == "SUP")
                                                            {
                                                                lq[4] = valor < lq[3] ? lq[3] : valor;
                                                                lq[6] = valor < lq[3] ? lq[3] : valor;
                                                                lq[8] = valor < lq[3] ? lq[3] : valor;

                                                                bool alterouRestAnalisar = false;
                                                                double valorAlterado = 0;
                                                                //vereficar as restAnalisar
                                                                foreach (var restAna in restsAnalisar)
                                                                {
                                                                    LqLine restVer = dadger.BlocoRhq.Where(x => x.Restricao == restAna.First().Restricao).Where(y => y is Compass.CommomLibrary.Dadger.LqLine && ((LqLine)y).Estagio <= lq.Estagio).OrderByDescending(y => ((LqLine)y).Estagio).Select(y => (Compass.CommomLibrary.Dadger.LqLine)y).FirstOrDefault();
                                                                    if (restVer != null)
                                                                    {
                                                                        if (restVer.LimInfPat1.HasValue && restVer.LimInfPat1 > lq[4])
                                                                        {
                                                                            restVer.LimInfPat1 = lq[4];
                                                                            alterouRestAnalisar = true;
                                                                            valorAlterado = lq[4];
                                                                        }

                                                                        if (restVer.LimInfPat2.HasValue && restVer.LimInfPat2 > lq[4])
                                                                        {
                                                                            restVer.LimInfPat2 = lq[4];
                                                                            alterouRestAnalisar = true;
                                                                            valorAlterado = lq[4];
                                                                        }

                                                                        if (restVer.LimInfPat3.HasValue && restVer.LimInfPat3 > lq[4])
                                                                        {
                                                                            restVer.LimInfPat3 = lq[4];
                                                                            alterouRestAnalisar = true;
                                                                            valorAlterado = lq[4];
                                                                        }
                                                                    }

                                                                }

                                                                //


                                                                if (produt65 >= 0 && nwhibrido == false) // alteração do arquivo re.dat caso necessario e nw NÃO hibrido
                                                                {
                                                                    double restValor = lq[4];
                                                                    foreach (var reRest in reDat.Restricoes.ToList())
                                                                    {
                                                                        foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == reRest.Numero).ToList())
                                                                        {

                                                                            if (reDet.Inicio < deckNWEstudo.Dger.DataEstudo && reDet.Fim >= deckNWEstudo.Dger.DataEstudo)
                                                                            {
                                                                                reDet.Inicio = deckNWEstudo.Dger.DataEstudo;
                                                                            }
                                                                            else if (reDet.Fim < deckNWEstudo.Dger.DataEstudo)
                                                                            {
                                                                                reDat.Detalhes.Remove(reDet);
                                                                            }
                                                                        }

                                                                        if (reDat.Detalhes.Where(x => x.Numero == reRest.Numero).Count() == 0) reDat.Restricoes.Remove(reRest);
                                                                    }
                                                                    //procura restricao
                                                                    var re = reDat.Restricoes.Where(
                                                                        x => String.Join("", x.Valores.Skip(1).Where(y => y != null).OrderBy(y => y).Select(y => y.ToString().Trim()))
                                                                            == String.Join("", lHq.UsiRest.ToString())
                                                                        ).FirstOrDefault();

                                                                    //se nao exite insere
                                                                    if (re == null)
                                                                    {

                                                                        re = new Compass.CommomLibrary.ReDat.ReLine()
                                                                        {
                                                                            Numero = reDat.Restricoes.Max(x => x.Numero) + 1
                                                                        };

                                                                        re[1] = lHq.UsiRest;

                                                                        reDat.Restricoes.Add(re);


                                                                        var val = new Compass.CommomLibrary.ReDat.ReValLine()
                                                                        {
                                                                            Numero = re.Numero,
                                                                            Patamar = 0,
                                                                            ValorRestricao = restValor * produt65,
                                                                            Inicio = data,
                                                                            Fim = data,
                                                                        };

                                                                        reDat.Detalhes.Add(val);

                                                                    }
                                                                    //altera ou insere novo valor
                                                                    else
                                                                    {

                                                                        var val = new Compass.CommomLibrary.ReDat.ReValLine()
                                                                        {
                                                                            Numero = re.Numero,
                                                                            Patamar = 0,
                                                                            ValorRestricao = restValor * produt65,
                                                                            Inicio = data,
                                                                            Fim = data,
                                                                        };

                                                                        var anterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                            .Where(x => x.Inicio < val.Inicio && x.Fim >= val.Inicio).FirstOrDefault();
                                                                        var posterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                            .Where(x => x.Inicio <= val.Fim && x.Fim > val.Fim).FirstOrDefault();

                                                                        if (anterior != null)
                                                                        {
                                                                            var anteriorSplit = anterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                                                                            anterior.Inicio = val.Inicio;
                                                                            anteriorSplit.Fim = val.Inicio.AddMonths(-1);

                                                                            reDat.Detalhes.Add(anteriorSplit);
                                                                        }

                                                                        if (posterior != null)
                                                                        {
                                                                            var posteriorSplit = posterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                                                                            posterior.Fim = val.Fim; ;
                                                                            posteriorSplit.Inicio = val.Fim.AddMonths(1);

                                                                            reDat.Detalhes.Add(posteriorSplit);
                                                                        }

                                                                        reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                            .Where(x => x.Inicio >= val.Inicio && x.Fim <= val.Fim).ToList().ForEach(x =>
                                                                                reDat.Detalhes.Remove(x)
                                                                                );

                                                                        reDat.Detalhes.Add(val);
                                                                    }
                                                                    var newl = reDat.Detalhes.OrderBy(x => x.Numero).ThenBy(x => x.Inicio).ToList();
                                                                    reDat.Detalhes.Clear();
                                                                    newl.ForEach(x => reDat.Detalhes.Add(x));
                                                                    reDat.SaveToFile();
                                                                }
                                                                else // alterar modif 
                                                                {
                                                                    if (nwhibrido)
                                                                    {
                                                                        string mineTurbVaz = lHq.Minemonico == "QTUR" ? "TURBMAXT" : "VAZMAXT";
                                                                        modif = Services.DecompNextRev.AlterarModifComLq(modif, lHq, mineTurbVaz, data, lq[4], engolimento);
                                                                    }
                                                                    if (alterouRestAnalisar == true)
                                                                    {
                                                                        string mineTurbVaz = lHq.Minemonico == "QTUR" ? "VAZMINT" : "TURBMINT";//se Qtur superior =>altera nas Qdef logo Vazmint; se Qdef superior => altera nas Qtur logo Turbmint
                                                                        modif = Services.DecompNextRev.AlterarModifComLq(modif, lHq, mineTurbVaz, data, valorAlterado, engolimento);

                                                                    }
                                                                    {
                                                                        //string mineTurbVaz = lHq.Minemonico == "QTUR" ? "TURBMAXT" : "VAZMAXT";
                                                                        //if (!modif.Any(x => x.Usina == lHq.UsiRest))
                                                                        //{
                                                                        //    modif.Add(new Compass.CommomLibrary.ModifDatNW.ModifLine()
                                                                        //    {
                                                                        //        Usina = lHq.UsiRest,
                                                                        //        Chave = "USINA",
                                                                        //        NovosValores = new string[] { lHq.UsiRest.ToString() }
                                                                        //    });

                                                                        //}
                                                                        //var modiflineTurbVaz = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == mineTurbVaz && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();

                                                                        //if (modiflineTurbVaz != null)
                                                                        //{
                                                                        //    if (modiflineTurbVaz.DataModif < data)
                                                                        //    {

                                                                        //        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                        //        var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                        //        var valorAntigo = modiflineTurbVaz.ValorModif;


                                                                        //        newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));
                                                                        //        newModifLine.Chave = mineTurbVaz;
                                                                        //        newModifLine.Usina = lHq.UsiRest;
                                                                        //        int index = modif.IndexOf(modiflineTurbVaz) + 1;
                                                                        //        modif.Insert(index, newModifLine);

                                                                        //        //mes seguinte verificação
                                                                        //        var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == mineTurbVaz && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                                        //        if (modiflineMesSeq == null)
                                                                        //        {
                                                                        //            //newModifLine2 = modifline;
                                                                        //            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                                                                        //            //newModifLine2.DataModif = data.AddMonths(1);
                                                                        //            newModifLine2.Chave = "TURBMAXT";
                                                                        //            newModifLine2.Usina = lHq.UsiRest;
                                                                        //            int index2 = modif.IndexOf(newModifLine) + 1;
                                                                        //            modif.Insert(index2, newModifLine2);
                                                                        //        }


                                                                        //    }
                                                                        //    else
                                                                        //    {
                                                                        //        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                        //        var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                        //        var valorAntigo = modiflineTurbVaz.ValorModif;

                                                                        //        modiflineTurbVaz.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));

                                                                        //        //mes seguinte verificação
                                                                        //        var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == mineTurbVaz && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                                        //        if (modiflineMesSeq == null)
                                                                        //        {
                                                                        //            //newModifLine2 = modifline;
                                                                        //            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                                                                        //            //newModifLine2.DataModif = data.AddMonths(1);
                                                                        //            newModifLine2.Chave = mineTurbVaz;
                                                                        //            newModifLine2.Usina = lHq.UsiRest;
                                                                        //            int index2 = modif.IndexOf(modiflineTurbVaz) + 1;
                                                                        //            modif.Insert(index2, newModifLine2);
                                                                        //        }

                                                                        //    }
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    var mod = modif.Where(x => x.Usina == lHq.UsiRest).FirstOrDefault();
                                                                        //    if (mod != null)
                                                                        //    {
                                                                        //        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                                                                        //        newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));
                                                                        //        newModifLine.Chave = mineTurbVaz;
                                                                        //        newModifLine.Usina = lHq.UsiRest;
                                                                        //        int indexT = modif.IndexOf(mod) + 1;
                                                                        //        modif.Insert(indexT, newModifLine);
                                                                        //    }
                                                                        //}
                                                                    }//codigo comentado


                                                                }
                                                            }
                                                            else//INF
                                                            {
                                                                lq[3] = valor > lq[4] ? lq[4] : valor;
                                                                lq[5] = valor > lq[4] ? lq[4] : valor;
                                                                lq[7] = valor > lq[4] ? lq[4] : valor;

                                                                bool alterouRestAnalisar = false;
                                                                double valorAlterado = 0;
                                                                //vereficar as restAnalisar
                                                                foreach (var restAna in restsAnalisar)//analisando limites superiores
                                                                {
                                                                    LqLine restVer = dadger.BlocoRhq.Where(x => x.Restricao == restAna.First().Restricao).Where(y => y is Compass.CommomLibrary.Dadger.LqLine && ((LqLine)y).Estagio <= lq.Estagio).OrderByDescending(y => ((LqLine)y).Estagio).Select(y => (Compass.CommomLibrary.Dadger.LqLine)y).FirstOrDefault();
                                                                    if (restVer != null)
                                                                    {
                                                                        if (restVer.LimSupPat1.HasValue && restVer.LimSupPat1 < lq[3])
                                                                        {
                                                                            restVer.LimSupPat1 = lq[3];
                                                                            alterouRestAnalisar = true;
                                                                            valorAlterado = lq[3];
                                                                        }

                                                                        if (restVer.LimSupPat2.HasValue && restVer.LimSupPat2 < lq[3])
                                                                        {
                                                                            restVer.LimSupPat2 = lq[3];
                                                                            alterouRestAnalisar = true;
                                                                            valorAlterado = lq[3];
                                                                        }

                                                                        if (restVer.LimSupPat3.HasValue && restVer.LimSupPat3 < lq[3])
                                                                        {
                                                                            restVer.LimSupPat3 = lq[3];
                                                                            alterouRestAnalisar = true;
                                                                            valorAlterado = lq[3];
                                                                        }
                                                                    }

                                                                }

                                                                //var modifline = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                                                                if (lHq.InfSup == "INF" && lq[3] != null)
                                                                {

                                                                    double modifval = lq[3];
                                                                    string mineTurbVaz = lHq.Minemonico == "QTUR" ? "TURBMINT" : "VAZMINT";
                                                                    modif = Services.DecompNextRev.AlterarModifComLq(modif, lHq, mineTurbVaz, data, modifval, engolimento);

                                                                    if (nwhibrido)
                                                                    {
                                                                        if (alterouRestAnalisar == true)
                                                                        {
                                                                            mineTurbVaz = lHq.Minemonico == "QTUR" ? "VAZMAXT" : "TURBMAXT";//se Qtur inferior =>altera nas Qdef logo Vazmaxt; se Qdef inferior => altera nas Qtur logo Turbmaxt
                                                                            modif = Services.DecompNextRev.AlterarModifComLq(modif, lHq, mineTurbVaz, data, valorAlterado, engolimento);
                                                                        }
                                                                    }
                                                                    else if (alterouRestAnalisar == true && nwhibrido == false && produt65 >= 0)
                                                                    {
                                                                        double restValor = valorAlterado;
                                                                        foreach (var reRest in reDat.Restricoes.ToList())
                                                                        {
                                                                            foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == reRest.Numero).ToList())
                                                                            {

                                                                                if (reDet.Inicio < deckNWEstudo.Dger.DataEstudo && reDet.Fim >= deckNWEstudo.Dger.DataEstudo)
                                                                                {
                                                                                    reDet.Inicio = deckNWEstudo.Dger.DataEstudo;
                                                                                }
                                                                                else if (reDet.Fim < deckNWEstudo.Dger.DataEstudo)
                                                                                {
                                                                                    reDat.Detalhes.Remove(reDet);
                                                                                }
                                                                            }

                                                                            if (reDat.Detalhes.Where(x => x.Numero == reRest.Numero).Count() == 0) reDat.Restricoes.Remove(reRest);
                                                                        }
                                                                        //procura restricao
                                                                        var re = reDat.Restricoes.Where(
                                                                            x => String.Join("", x.Valores.Skip(1).Where(y => y != null).OrderBy(y => y).Select(y => y.ToString().Trim()))
                                                                                == String.Join("", lHq.UsiRest.ToString())
                                                                            ).FirstOrDefault();

                                                                        //se nao exite insere
                                                                        if (re == null)
                                                                        {

                                                                            re = new Compass.CommomLibrary.ReDat.ReLine()
                                                                            {
                                                                                Numero = reDat.Restricoes.Max(x => x.Numero) + 1
                                                                            };

                                                                            re[1] = lHq.UsiRest;

                                                                            reDat.Restricoes.Add(re);


                                                                            var val = new Compass.CommomLibrary.ReDat.ReValLine()
                                                                            {
                                                                                Numero = re.Numero,
                                                                                Patamar = 0,
                                                                                ValorRestricao = restValor * produt65,
                                                                                Inicio = data,
                                                                                Fim = data,
                                                                            };

                                                                            reDat.Detalhes.Add(val);

                                                                        }
                                                                        //altera ou insere novo valor
                                                                        else
                                                                        {

                                                                            var val = new Compass.CommomLibrary.ReDat.ReValLine()
                                                                            {
                                                                                Numero = re.Numero,
                                                                                Patamar = 0,
                                                                                ValorRestricao = restValor * produt65,
                                                                                Inicio = data,
                                                                                Fim = data,
                                                                            };

                                                                            var anterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                                .Where(x => x.Inicio < val.Inicio && x.Fim >= val.Inicio).FirstOrDefault();
                                                                            var posterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                                .Where(x => x.Inicio <= val.Fim && x.Fim > val.Fim).FirstOrDefault();

                                                                            if (anterior != null)
                                                                            {
                                                                                var anteriorSplit = anterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                                                                                anterior.Inicio = val.Inicio;
                                                                                anteriorSplit.Fim = val.Inicio.AddMonths(-1);

                                                                                reDat.Detalhes.Add(anteriorSplit);
                                                                            }

                                                                            if (posterior != null)
                                                                            {
                                                                                var posteriorSplit = posterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                                                                                posterior.Fim = val.Fim; ;
                                                                                posteriorSplit.Inicio = val.Fim.AddMonths(1);

                                                                                reDat.Detalhes.Add(posteriorSplit);
                                                                            }

                                                                            reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                                .Where(x => x.Inicio >= val.Inicio && x.Fim <= val.Fim).ToList().ForEach(x =>
                                                                                    reDat.Detalhes.Remove(x)
                                                                                    );

                                                                            reDat.Detalhes.Add(val);
                                                                        }
                                                                        var newl = reDat.Detalhes.OrderBy(x => x.Numero).ThenBy(x => x.Inicio).ToList();
                                                                        reDat.Detalhes.Clear();
                                                                        newl.ForEach(x => reDat.Detalhes.Add(x));
                                                                        reDat.SaveToFile();
                                                                    }

                                                                }

                                                            }

                                                            /////////
                                                            {
                                                                //var modifline = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                                                                //if (lHq.InfSup == "INF" && lq[3] != null)
                                                                //{
                                                                //    double modifval = lq[3];

                                                                //    if (modifline != null)
                                                                //    {
                                                                //        if (modifline.DataModif < data)
                                                                //        {

                                                                //            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                //            var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                //            var valorAntigo = modifline.ValorModif;


                                                                //            newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));
                                                                //            newModifLine.Chave = "VAZMINT";
                                                                //            newModifLine.Usina = lHq.UsiRest;
                                                                //            int index = modif.IndexOf(modifline) + 1;
                                                                //            modif.Insert(index, newModifLine);

                                                                //            //mes seguinte verificação
                                                                //            var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                                //            if (modiflineMesSeq == null)
                                                                //            {
                                                                //                //newModifLine2 = modifline;
                                                                //                newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                                                                //                //newModifLine2.DataModif = data.AddMonths(1);
                                                                //                newModifLine2.Chave = "VAZMINT";
                                                                //                newModifLine2.Usina = lHq.UsiRest;
                                                                //                int index2 = modif.IndexOf(newModifLine) + 1;
                                                                //                modif.Insert(index2, newModifLine2);
                                                                //            }


                                                                //        }
                                                                //        else
                                                                //        {
                                                                //            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                //            var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                //            var valorAntigo = modifline.ValorModif;

                                                                //            modifline.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));

                                                                //            //mes seguinte verificação
                                                                //            var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                                //            if (modiflineMesSeq == null)
                                                                //            {
                                                                //                //newModifLine2 = modifline;
                                                                //                newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                                                                //                //newModifLine2.DataModif = data.AddMonths(1);
                                                                //                newModifLine2.Chave = "VAZMINT";
                                                                //                newModifLine2.Usina = lHq.UsiRest;
                                                                //                int index2 = modif.IndexOf(modifline) + 1;
                                                                //                modif.Insert(index2, newModifLine2);
                                                                //            }

                                                                //        }
                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        var mod = modif.Where(x => x.Usina == lHq.UsiRest).FirstOrDefault();
                                                                //        if (mod != null)
                                                                //        {
                                                                //            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                                                                //            newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));
                                                                //            newModifLine.Chave = "VAZMINT";
                                                                //            newModifLine.Usina = lHq.UsiRest;
                                                                //            int index = modif.IndexOf(mod) + 1;
                                                                //            modif.Insert(index, newModifLine);
                                                                //        }


                                                                //    }
                                                                //}
                                                            }//codigo comentado

                                                            modif.SaveToFile(filePath: modifFile);
                                                            /////////
                                                        }

                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                if (limitesHV.Count() > 0)
                                {
                                    foreach (var lHv in limitesHV)
                                    {
                                        // dynamic lq;

                                        var UH = dadger.BlocoUh.Where(x => x.Usina == lHv.UH.First()).FirstOrDefault();

                                        double hectoMin = configH.Usinas.Any(x => x.Cod == lHv.UsiRest) ? configH.Usinas.Where(x => x.Cod == lHv.UsiRest).Select(x => x.VolMin).First() : -1;

                                        if (UH != null)
                                        {

                                            var rests = dadger.BlocoRhv.Where(x => x.Restricao == lHv.CodRest);

                                            double percentAlvo = UH.VolIniPerc;

                                            if (lHv.UH.Count() > 1)
                                            {
                                                percentAlvo = Services.DecompNextRev.GetpercentAlvo(configH, lHv.UH);
                                            }

                                            if (rests.Count() > 0)
                                            {
                                                var le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LvLine).Select(x => (Compass.CommomLibrary.Dadger.LvLine)x);
                                                // var lvs = le.Where(x => x.Estagio <= dadger.VAZOES_NumeroDeSemanas).ToList();
                                                dynamic lvdummy = le.Where(x => x.Estagio <= 2).OrderByDescending(x => x.Estagio).FirstOrDefault();

                                                if (lvdummy.Estagio < 2)//caso não exista o estagio do segundo mes informado, copia os dados do ultimo estagio informado para o segundo mes
                                                {

                                                    var nledummy = lvdummy.Clone();
                                                    nledummy.Estagio = 2;
                                                    dadger.BlocoRhv.Add(nledummy);
                                                }

                                                rests = dadger.BlocoRhv.Where(x => x.Restricao == lHv.CodRest);
                                                le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LvLine).Select(x => (Compass.CommomLibrary.Dadger.LvLine)x);
                                                var lvs = le.Where(x => x.Estagio <= 2).ToList();

                                                if (lvs.Count() > 0)
                                                {
                                                    foreach (var lv in lvs)
                                                    {
                                                        modif = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
                                                        var modifFile = modif.File;

                                                        string minemonico = "";
                                                        double valorTemp;

                                                        DateTime data;
                                                        data = new DateTime(dtEstudo.Year, dtEstudo.Month, 1);

                                                        double valor = 0;
                                                        valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHv, w.Faixapercents.First());

                                                        if (lv.Estagio == 2)
                                                        {
                                                            data = mesSeg;
                                                            var lHvSEg = w.Faixalimites.Where(x => x.MesIni <= mesSeg.Month && x.MesFim >= mesSeg.Month && x.Ativa == true && x.UsiRest == lHv.UsiRest && x.UH.All(lHv.UH.Contains) && x.UH.Count == lHv.UH.Count && x.InfSup == lHv.InfSup && x.TipoRest.ToUpper().Equals("HV")).FirstOrDefault();
                                                            if (lHvSEg != null)
                                                            {
                                                                valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHvSEg, w.Faixapercents.First());
                                                            }
                                                        }

                                                        if (lHv.InfSup == "SUP")
                                                        {
                                                            lv[4] = valor < lv[3] ? lv[3] : valor;
                                                            minemonico = "VMAXT";
                                                            valorTemp = lv[4];
                                                        }
                                                        else
                                                        {
                                                            lv[3] = valor > lv[4] ? lv[4] : valor;
                                                            minemonico = "VMINT";
                                                            valorTemp = lv[3];
                                                        }

                                                        /////////

                                                        if (hectoMin >= 0)
                                                        {
                                                            var modifline = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                                                            double modifval = valorTemp + hectoMin;
                                                            if (modifline != null)
                                                            {
                                                                if (modifline.DataModif < data)
                                                                {

                                                                    var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                    var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                    var valorAntigo = modifline.ValorModif;


                                                                    newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");
                                                                    newModifLine.Chave = minemonico;
                                                                    newModifLine.Usina = lHv.UsiRest;
                                                                    int index = modif.IndexOf(modifline) + 1;
                                                                    modif.Insert(index, newModifLine);

                                                                    //mes seguinte verificação
                                                                    var modiflineMesSeq = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                                    if (modiflineMesSeq == null)
                                                                    {
                                                                        //newModifLine2 = modifline;
                                                                        newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'), "'h'");
                                                                        //newModifLine2.DataModif = data.AddMonths(1);
                                                                        newModifLine2.Chave = minemonico;
                                                                        newModifLine2.Usina = lHv.UsiRest;
                                                                        int index2 = modif.IndexOf(newModifLine) + 1;
                                                                        modif.Insert(index2, newModifLine2);
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                    var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                    var valorAntigo = modifline.ValorModif;

                                                                    modifline.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");

                                                                    //mes seguinte verificação
                                                                    var modiflineMesSeq = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                                    if (modiflineMesSeq == null)
                                                                    {
                                                                        //newModifLine2 = modifline;
                                                                        newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'), "'h'");
                                                                        //newModifLine2.DataModif = data.AddMonths(1);
                                                                        newModifLine2.Chave = minemonico;
                                                                        newModifLine2.Usina = lHv.UsiRest;
                                                                        int index2 = modif.IndexOf(modifline) + 1;
                                                                        modif.Insert(index2, newModifLine2);
                                                                    }

                                                                }
                                                            }
                                                            else
                                                            {
                                                                var mod = modif.Where(x => x.Usina == lHv.UsiRest).FirstOrDefault();
                                                                if (mod != null)
                                                                {
                                                                    var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                                                                    newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");
                                                                    newModifLine.Chave = minemonico;
                                                                    newModifLine.Usina = lHv.UsiRest;
                                                                    int index = modif.IndexOf(mod) + 1;
                                                                    modif.Insert(index, newModifLine);
                                                                }


                                                            }

                                                            modif.SaveToFile(filePath: modifFile);
                                                        }

                                                        /////////

                                                    }

                                                }
                                            }



                                        }
                                    }
                                }

                                //todo excluir os turbmax caso só exista dados com 99999

                                var modifsFinal = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
                                var modifFileFinal = modifsFinal.File;

                                var usinasTurbmaxtFinal = modifsFinal.Where(x => x.Chave == "TURBMAXT").Select(x => x.Usina).Distinct();
                                List<Compass.CommomLibrary.ModifDatNW.ModifLine> removerFinal = new List<Compass.CommomLibrary.ModifDatNW.ModifLine>();

                                foreach (var usiT in usinasTurbmaxtFinal)
                                {
                                    var modifsremove = modifsFinal.Where(x => x.Usina == usiT && x.Chave == "TURBMAXT").ToList();
                                    if (modifsremove.All(x => x.ValorModif == 99999))
                                    {
                                        modifsremove.ForEach(x => removerFinal.Add(x));
                                    }
                                }

                                removerFinal.ForEach(x => modifsFinal.Remove(x));

                                removerFinal.Clear();

                                var usinaSemMine = modifsFinal.Select(x => x.Usina).Distinct();

                                foreach (var usiT in usinaSemMine)
                                {
                                    var modifsremove = modifsFinal.Where(x => x.Usina == usiT).ToList();
                                    if (modifsremove.Count() == 1 && modifsremove[0].Chave.ToUpper().Trim() == "USINA")
                                    {
                                        removerFinal.Add(modifsremove[0]);
                                    }
                                }
                                removerFinal.ForEach(x => modifsFinal.Remove(x));
                                modifsFinal.SaveToFile(filePath: modifFileFinal);

                                //

                                dadger.SaveToFile();

                                if (w.Faixapercents.Count() > 0 && w.Faixalimites.Count() > 0)
                                {
                                    List<string> faixaText = new List<string>();
                                    string header = "UH;TIPO REST;USINA REST;COD REST;MÊS INI;MÊS FIM;INF/SUP;ATIVA";
                                    w.Faixapercents.First().Percents.ForEach(x => header = header + ";" + x.ToString() + "%");

                                    faixaText.Add(header);
                                    w.Faixalimites.ForEach(x =>
                                    {
                                        string linha;
                                        linha = string.Join(";", x.UHstring, x.TipoRest, x.UsiRest.ToString(), x.CodRest.ToString(), x.MesIni.ToString(), x.MesFim.ToString(), x.InfSup.ToString(), x.Ativa.ToString()) + ";";
                                        linha = linha + string.Join(";", x.Vals.ToList());
                                        faixaText.Add(linha);
                                        // x.Vals.ForEach(y => { linha = linha + y.ToString(); });
                                    });
                                    File.WriteAllLines(Path.Combine(estudoPath, "LIMITES_DE_RESTRICAO.txt"), faixaText);

                                    string destino = deckNWEstudo.BaseFolder;



                                    var dgerFileconsist = Directory.GetFiles(destino).Where(x => Path.GetFileName(x).ToLower().Equals("dger.dat")).FirstOrDefault();
                                    if (dgerFileconsist != null)
                                    {


                                        var dger = (Compass.CommomLibrary.DgerDat.DgerDat)DocumentFactory.Create(dgerFileconsist);


                                        var earmsREE = new double[dadosREE.Count()];
                                        int r = 0;
                                        foreach (var dadR in dadosREE)
                                        {
                                            earmsREE[r] = Math.Round(dadR.Item2, 1);
                                            if (earmsREE[r] > 100.0f)
                                            {
                                                earmsREE[r] = 100.0f;
                                            }
                                            if (earmsREE[r] < 0)
                                            {
                                                earmsREE[r] = 0;
                                            }
                                            r++;
                                        }
                                        //for (int i = 0; i < dger.Earms.Count(); i++)
                                        //{
                                        //    if (dger.Earms[i] > 100.0f)
                                        //    {
                                        //        dger.Earms[i] = 100.0f;
                                        //    }
                                        //    if (dger.Earms[i] < 0)
                                        //    {
                                        //        dger.Earms[i] = 0;
                                        //    }

                                        //}

                                        dger.Earms = earmsREE;
                                        dger.SaveToFile();
                                    }
                                    if (estudoPath.Contains("DCGNL"))
                                    {
                                        consistFolders.Add(destino);
                                    }

                                    //var ret = Compass.Services.Linux.Run2(destino, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/newaveCons280003.sh 3", "NewaveConsist", true, true, "hide");// para debug usar essa funçao

                                    ////var ret = Compass.Services.Linux.Run(destino, w.ExecutavelNewave + " 3", "NewaveConsist", true, true, "hide");
                                    //if (!ret)
                                    //{
                                    //    throw new Exception("Ocorreu erro na criação e consistência dos decks newaves. Verifique.");
                                    //}

                                    //Compass.Services.Deck.CreateDgerNewdesp(destino);




                                }

                            }

                            //fim codigofaixas limites

                            #endregion


                            #endregion
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
                            //Encadeado.Estudo testead = new Encadeado.Estudo();
                            var testead = w.adtermdat ?? new List<IADTERM>();

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

                                if (testead.Count() == 0)
                                {
                                    despacho = new Tuple<double, double, double>(0, 0, 0);

                                }
                                else
                                {

                                    var alter_dadgnl = testead.Where(x => x.Mes == dtEstudo.Month && x.Usina == ut.Usina).ToList();
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

                                if (w.Dadterm.Count() > 0)
                                {
                                    var dadAdterms = w.Dadterm.Where(x => x.usina == glLine.NumeroUsina && x.ano == dtEstudo.Year && x.mes == dtEstudo.Month && x.estagio == 1).FirstOrDefault();
                                    if (dadAdterms != null)
                                    {
                                        glLine.GeracaoPat1 = (float)dadAdterms.PT1;
                                        glLine.GeracaoPat2 = (float)dadAdterms.PT2;
                                        glLine.GeracaoPat3 = (float)dadAdterms.PT3;
                                    }
                                    else
                                    {
                                        glLine.GeracaoPat1 = 0;
                                        glLine.GeracaoPat2 = 0;
                                        glLine.GeracaoPat3 = 0;
                                    }
                                }


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
                                if (testead.Count() == 0)
                                {
                                    despacho = new Tuple<double, double, double>(0, 0, 0);

                                }
                                else
                                {
                                    var alter_dadgnl = testead.Where(x => x.Mes == dtEstudoSeguinte.Month && x.Usina == ut.Usina).ToList();
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

                                if (w.Dadterm.Count() > 0)
                                {
                                    var dadAdterms2 = w.Dadterm.Where(x => x.usina == glLine.NumeroUsina && x.ano == dtEstudo.Year && x.mes == dtEstudo.Month && x.estagio == 2).FirstOrDefault();
                                    if (dadAdterms2 != null)
                                    {
                                        glLine.GeracaoPat1 = (float)dadAdterms2.PT1;
                                        glLine.GeracaoPat2 = (float)dadAdterms2.PT2;
                                        glLine.GeracaoPat3 = (float)dadAdterms2.PT3;
                                    }
                                    else
                                    {
                                        glLine.GeracaoPat1 = 0;
                                        glLine.GeracaoPat2 = 0;
                                        glLine.GeracaoPat3 = 0;
                                    }
                                }

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

            return consistFolders;
        }

        public static void GeraRV0(WorkbookMensal w, string dc, string nw, bool auto = false)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            //var statusBarState = Globals.ThisAddIn.Application.DisplayStatusBar;

            {

                //var dc = w.DecompBase;
                //var nw = w.NewaveBase;
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
                    return;
                }

                outPath = Path.Combine(w.NewaveBase, "RV0");

                var deckDCBase = DeckFactory.CreateDeck(dc) as Compass.CommomLibrary.Decomp.Deck;

                #region verifica rests faixa limites Decomp rv0
                var dadgerBaseVer = deckDCBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger;

                string avisos = "";
                List<int> hqsErr = new List<int>();
                List<int> hvsErr = new List<int>();

                bool verificaRESTS = false;

                var limitesHQver = w.Faixalimites.Where(x => x.Ativa == true && x.TipoRest.ToUpper().Equals("HQ"));
                var limitesHVver = w.Faixalimites.Where(x => x.Ativa == true && x.TipoRest.ToUpper().Equals("HV"));

                foreach (var lim in limitesHQver)
                {
                    var restshq = dadgerBaseVer.BlocoRhq.Where(x => x.Restricao == lim.CodRest);
                    if (restshq.Count() > 0)
                    {
                        var le = restshq.Where(x => x is Compass.CommomLibrary.Dadger.CqLine).Select(x => (Compass.CommomLibrary.Dadger.CqLine)x).First();
                        if (le.Usina == lim.UsiRest)
                        {
                            continue;
                        }
                        else if (hqsErr.All(x => x != lim.CodRest))
                        {
                            avisos = avisos + $"HQ {lim.CodRest} Usina Deck: {le.Usina} Usina Informada: {lim.UsiRest} \r\n";
                            hqsErr.Add(lim.CodRest);
                            verificaRESTS = true;
                        }
                    }
                }

                foreach (var lim in limitesHVver)
                {
                    var restshv = dadgerBaseVer.BlocoRhv.Where(x => x.Restricao == lim.CodRest);
                    if (restshv.Count() > 0)
                    {
                        var le = restshv.Where(x => x is Compass.CommomLibrary.Dadger.CvLine).Select(x => (Compass.CommomLibrary.Dadger.CvLine)x).First();
                        if (le.Usina == lim.UsiRest)
                        {
                            continue;
                        }
                        else if (hvsErr.All(x => x != lim.CodRest))
                        {
                            avisos = avisos + $"HV {lim.CodRest} Usina Deck: {le.Usina} Usina Informada: {lim.UsiRest} \r\n";
                            hvsErr.Add(lim.CodRest);
                            verificaRESTS = true;
                        }
                    }
                }
                if (verificaRESTS == true && auto == false)
                {
                    return;
                }

                #endregion

                #region planMemoria de calculo

                string planMemo = Directory.GetFiles(w.NewaveOrigem).Where(x => Path.GetFileName(x).StartsWith("Memória de Cálculo", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                List<Tuple<int, int, DateTime, double>> eolicasDados = null;
                if (planMemo != null && File.Exists(planMemo))
                {
                    eolicasDados = getEolicasplan(planMemo);
                }

                #endregion

                Directory.CreateDirectory(outPath);

                List<Tuple<int, double, double>> curvaArmazenamento = null;

                foreach (var dtEstudo in meses)
                {

                    var estudoPath = Path.Combine(outPath, dtEstudo.ToString("yyyyMM"));
                    var nwPath = Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"));

                    Directory.CreateDirectory(estudoPath);

                    deckDCBase.CopyFilesToFolder(estudoPath, 0);

                    var deckEstudo = DeckFactory.CreateDeck(estudoPath) as Compass.CommomLibrary.Decomp.Deck;

                    deckNWEstudo = DeckFactory.CreateDeck(Path.Combine(w.NewaveBase, dtEstudo.ToString("yyyyMM"))) as Compass.CommomLibrary.Newave.Deck;

                    var patamares = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
                    var durPat1 = patamares.Blocos["Duracao"].Where(x => x[1] == dtEstudo.Year).OrderBy(x => x[0]).Select(x => x[dtEstudo.Month.ToString()]).ToArray();
                    //bool patamares2019 = durPat1[0] > 0.15;
                    bool patamares2019 = true;

                    var pmoBase = DocumentFactory.Create(System.IO.Path.Combine(deckNWEstudo.BaseFolder, "pmo.dat")) as Compass.CommomLibrary.Pmo.Pmo;
                    bool patamares2023 = w.patamares2023;
                    bool patamares2024 = false;
                    bool patamares2025 = false;
                    patamares2024 = dtEstudo.Year == 2024;
                    patamares2025 = dtEstudo.Year >= 2025;

                    if (patamares2024 || patamares2025)//filtro para usar patamares de carga 2023 ou 2024...
                    {
                        patamares2023 = false;
                    }
                    var mesOperativo = MesOperativo.CreateSemanal(dtEstudo.Year, dtEstudo.Month, patamares2019, patamares2023, patamares2024, patamares2025);



                    //  if (dtEstudo != (deckDCBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger).VAZOES_DataDoEstudo)
                    // {
                    var dadger = Services.DecompNextRev.CreateRv0(deckEstudo, deckNWEstudo, dtEstudo, w, mesOperativo, pmoBase, eolicasDados, w.NwHibrido);

                    dadger.VAZOES_ArquivoPrevs = "prevs.rv0";
                    dadger.SaveToFile(createBackup: true);
                    //  }
                    #region csvlibs
                    //Services.DecompNextRev.IncrementaLibsCSV(deckEstudo, deckNWEstudo, dtEstudo);

                    #endregion



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
                                //fixaUhTxtLines.Add(df.Posto.ToString() + "\t" + df.Volini.ToString());
                                fixaUhTxtLines.Add(df.Posto.ToString() + "\t" + (df.Volini != null ? df.Volini.ToString() : "null"));
                            }
                            File.WriteAllLines(Path.Combine(estudoPath, "UhFixados.txt"), fixaUhTxtLines);

                            Services.Reservatorio.SetUHBlockFixado(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax, dadosFixas, auto);
                        }
                        else
                        {
                            Services.Reservatorio.SetUHBlock(configH, w.Earm.Select(u => u.Value[mesEarmFinal]).ToArray(), earmMax, auto);
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


                    #region armazenamento REE
                    var hidrDatNew = deckEstudo[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat;

                    var configHNew = new Compass.CommomLibrary.Decomp.ConfigH(configH.baseDoc as Dadger, hidrDatNew);

                    //var dadosREE = configH.GetREEList();
                    var dadosREE = configHNew.GetREEList();

                    //configH.ReloadUH();
                    configHNew.ReloadUH();

                    List<string> linhasREE = new List<string>();

                    linhasREE.Add("REE\tEARM%");
                    foreach (var dadR in dadosREE)
                    {
                        linhasREE.Add($"{ dadR.Item1}\t{dadR.Item2}%");
                    }
                    File.WriteAllLines(Path.Combine(estudoPath, "REE_EARM.txt"), linhasREE);

                    #endregion

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

                    #region faixa limites de restrição

                    //codigofaixaslimites aqui
                    if (w.Faixapercents != null && w.Faixalimites != null)//if (w.Faixapercents.Count() > 0 && w.Faixalimites.Count() > 0)
                    {
                        DateTime mesSeg = dtEstudo.AddMonths(1);

                        var limitesHQ = w.Faixalimites.Where(x => x.MesIni <= dtEstudo.Month && x.MesFim >= dtEstudo.Month && x.Ativa == true && x.TipoRest.ToUpper().Equals("HQ") && (x.Minemonico == "QDEF" || x.Minemonico == "QTUR"));
                        var limitesHV = w.Faixalimites.Where(x => x.MesIni <= dtEstudo.Month && x.MesFim >= dtEstudo.Month && x.Ativa == true && x.TipoRest.ToUpper().Equals("HV"));

                        Compass.CommomLibrary.ModifDatNW.ModifDatNw modif;
                        bool nwhibrido = w.NwHibrido;
                        //string redatBase = Path.Combine(deckNWEstudo.BaseFolder, "re_base.dat");
                        //var redatFile = Directory.GetFiles(deckNWEstudo.BaseFolder).Where(x => Path.GetFileName(x).ToLower().Equals("re.dat")).FirstOrDefault();
                        //if (redatFile != null && !File.Exists(redatBase))
                        //{
                        //    File.Copy(redatFile, redatBase, true);
                        //}


                        if (limitesHQ.Count() > 0)
                        {

                            foreach (var lHq in limitesHQ)
                            {
                                // dynamic lq;
                                var UH = dadger.BlocoUh.Where(x => x.Usina == lHq.UH.First()).FirstOrDefault();

                                double produt65 = configH.Usinas.Any(x => x.Cod == lHq.UsiRest) ? configH.Usinas.Where(x => x.Cod == lHq.UsiRest).Select(x => x.Prod65VolUtil).First() : -1;// -1 para ocaso de não encontrar o dado referente a usina da restrição
                                double engolimento = configH.Usinas.Any(x => x.Cod == lHq.UsiRest) ? configH.Usinas.Where(x => x.Cod == lHq.UsiRest).Select(x => x.Engolimento).First() : -1;// -1 para ocaso de não encontrar o dado referente a usina da restrição

                                if (UH != null)
                                {

                                    //var rests = dadger.BlocoRhq.Where(x => x.Restricao == lHq.CodRest);

                                    var restPorUsina = dadger.BlocoRhq.RhqGrouped.Where(rh =>
                                                rh.Value.Where(x => x is CqLine).All(x => ((CqLine)x).Usina == lHq.UsiRest && (((CqLine)x).Tipo == "QDEF" || ((CqLine)x).Tipo == "QTUR"))
                                               ).Select(x => x.Value).ToList();

                                    var restPorUsinaQdefs = dadger.BlocoRhq.RhqGrouped.Where(rh =>
                                         rh.Value.Where(x => x is CqLine).All(x => ((CqLine)x).Usina == lHq.UsiRest && ((CqLine)x).Tipo == "QDEF")
                                        ).Select(x => x.Value).ToList();

                                    var restPorUsinaQturs = dadger.BlocoRhq.RhqGrouped.Where(rh =>
                                         rh.Value.Where(x => x is CqLine).All(x => ((CqLine)x).Usina == lHq.UsiRest && ((CqLine)x).Tipo == "QTUR")
                                        ).Select(x => x.Value).ToList();

                                    List<List<RhqLine>> restsAlvo = new List<List<RhqLine>>();
                                    List<List<RhqLine>> restsAnalisar = new List<List<RhqLine>>();

                                    if (lHq.Minemonico == "QTUR")
                                    {
                                        restsAlvo = restPorUsinaQturs;
                                        restsAnalisar = restPorUsinaQdefs;
                                    }
                                    else
                                    {
                                        restsAlvo = restPorUsinaQdefs;
                                        restsAnalisar = restPorUsinaQturs;
                                    }

                                    if (restsAlvo.Count() == 0)
                                    {
                                        var rest = new List<RhqLine>();
                                        rest.Add(new HqLine()
                                        {
                                            Restricao = dadger.BlocoRhq.GetNextId(),
                                            Inicio = 1,
                                            Fim = dadger.VAZOES_NumeroDeSemanas + 1

                                        });

                                        rest.Add(new LqLine() { Restricao = rest.First().Restricao, Estagio = 1 });
                                        rest.Add(new CqLine() { Restricao = rest.First().Restricao, Usina = lHq.UsiRest, Tipo = lHq.Minemonico });

                                        rest.ForEach(x => dadger.BlocoRhq.Add(x));

                                        restsAlvo.Add(rest);
                                    }

                                    foreach (var rA in restsAlvo)
                                    {
                                        int restNumber = rA.First().Restricao;

                                        var rests = dadger.BlocoRhq.Where(x => x.Restricao == restNumber);

                                        double percentAlvo = UH.VolIniPerc;
                                        if (lHq.UH.Count() > 1)
                                        {
                                            percentAlvo = Services.DecompNextRev.GetpercentAlvo(configH, lHq.UH);
                                        }



                                        if (rests.Count() > 0)
                                        {
                                            var le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LqLine).Select(x => (Compass.CommomLibrary.Dadger.LqLine)x);
                                            dynamic lqdummy = le.Where(x => x.Estagio <= dadger.VAZOES_NumeroDeSemanas + 1).OrderByDescending(x => x.Estagio).FirstOrDefault();

                                            if (lqdummy.Estagio < dadger.VAZOES_NumeroDeSemanas + 1)//caso não exista o estagio do segundo mes informado, copia os dados do ultimo estagio informado para o segundo mes
                                            {

                                                var nledummy = lqdummy.Clone();
                                                nledummy.Estagio = dadger.VAZOES_NumeroDeSemanas + 1;
                                                dadger.BlocoRhq.Add(nledummy);
                                            }

                                            //rests = dadger.BlocoRhq.Where(x => x.Restricao == lHq.CodRest);
                                            rests = dadger.BlocoRhq.Where(x => x.Restricao == restNumber);

                                            le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LqLine).Select(x => (Compass.CommomLibrary.Dadger.LqLine)x);
                                            var lqs = le.Where(x => x.Estagio <= dadger.VAZOES_NumeroDeSemanas + 1).ToList();

                                            if (lqs.Count() > 0)
                                            {
                                                foreach (var lq in lqs)
                                                {
                                                    modif = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
                                                    var modifFile = modif.File;

                                                    var reDat = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.re].Document as Compass.CommomLibrary.ReDat.ReDat;

                                                    DateTime data;
                                                    data = new DateTime(dtEstudo.Year, dtEstudo.Month, 1);

                                                    double valor = 0;
                                                    valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHq, w.Faixapercents.First());

                                                    if (lq.Estagio == dadger.VAZOES_NumeroDeSemanas + 1)
                                                    {
                                                        data = mesSeg;
                                                        // var a = ints1.All(ints2.Contains) && ints1.Count == ints2.Count;
                                                        //var lHqSEg = w.Faixalimites.Where(x => x.MesIni <= mesSeg.Month && x.MesFim >= mesSeg.Month && x.Ativa == true && x.UsiRest == lHq.UsiRest && x.UH.All(lHq.UH.Contains) && x.UH.Count == lHq.UH.Count && x.InfSup == lHq.InfSup && x.TipoRest.ToUpper().Equals("HQ")).FirstOrDefault();
                                                        var lHqSEg = w.Faixalimites.Where(x => x.MesIni <= mesSeg.Month && x.MesFim >= mesSeg.Month && x.Ativa == true && x.UsiRest == lHq.UsiRest && x.UH.All(lHq.UH.Contains) && x.UH.Count == lHq.UH.Count && x.InfSup == lHq.InfSup && x.TipoRest.ToUpper().Equals("HQ") && x.Minemonico == lHq.Minemonico).FirstOrDefault();

                                                        if (lHqSEg != null)
                                                        {
                                                            valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHqSEg, w.Faixapercents.First());
                                                        }
                                                    }

                                                    if (lHq.InfSup == "SUP")
                                                    {
                                                        lq[4] = valor < lq[3] ? lq[3] : valor;
                                                        lq[6] = valor < lq[3] ? lq[3] : valor;
                                                        lq[8] = valor < lq[3] ? lq[3] : valor;

                                                        bool alterouRestAnalisar = false;
                                                        double valorAlterado = 0;
                                                        //vereficar as restAnalisar
                                                        foreach (var restAna in restsAnalisar)
                                                        {
                                                            LqLine restVer = dadger.BlocoRhq.Where(x => x.Restricao == restAna.First().Restricao).Where(y => y is Compass.CommomLibrary.Dadger.LqLine && ((LqLine)y).Estagio <= lq.Estagio).OrderByDescending(y => ((LqLine)y).Estagio).Select(y => (Compass.CommomLibrary.Dadger.LqLine)y).FirstOrDefault();
                                                            if (restVer != null)
                                                            {
                                                                if (restVer.LimInfPat1.HasValue && restVer.LimInfPat1 > lq[4])
                                                                {
                                                                    restVer.LimInfPat1 = lq[4];
                                                                    alterouRestAnalisar = true;
                                                                    valorAlterado = lq[4];
                                                                }

                                                                if (restVer.LimInfPat2.HasValue && restVer.LimInfPat2 > lq[4])
                                                                {
                                                                    restVer.LimInfPat2 = lq[4];
                                                                    alterouRestAnalisar = true;
                                                                    valorAlterado = lq[4];
                                                                }

                                                                if (restVer.LimInfPat3.HasValue && restVer.LimInfPat3 > lq[4])
                                                                {
                                                                    restVer.LimInfPat3 = lq[4];
                                                                    alterouRestAnalisar = true;
                                                                    valorAlterado = lq[4];
                                                                }
                                                            }

                                                        }
                                                        //

                                                        if (produt65 >= 0 && nwhibrido == false) // alteração do arquivo re.dat caso necessario e nw NÃO hibrido
                                                        {
                                                            double restValor = lq[4];
                                                            foreach (var reRest in reDat.Restricoes.ToList())
                                                            {
                                                                foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == reRest.Numero).ToList())
                                                                {

                                                                    if (reDet.Inicio < deckNWEstudo.Dger.DataEstudo && reDet.Fim >= deckNWEstudo.Dger.DataEstudo)
                                                                    {
                                                                        reDet.Inicio = deckNWEstudo.Dger.DataEstudo;
                                                                    }
                                                                    else if (reDet.Fim < deckNWEstudo.Dger.DataEstudo)
                                                                    {
                                                                        reDat.Detalhes.Remove(reDet);
                                                                    }
                                                                }

                                                                if (reDat.Detalhes.Where(x => x.Numero == reRest.Numero).Count() == 0) reDat.Restricoes.Remove(reRest);
                                                            }
                                                            //procura restricao
                                                            var re = reDat.Restricoes.Where(
                                                                x => String.Join("", x.Valores.Skip(1).Where(y => y != null).OrderBy(y => y).Select(y => y.ToString().Trim()))
                                                                    == String.Join("", lHq.UsiRest.ToString())
                                                                ).FirstOrDefault();

                                                            //se nao exite insere
                                                            if (re == null)
                                                            {

                                                                re = new Compass.CommomLibrary.ReDat.ReLine()
                                                                {
                                                                    Numero = reDat.Restricoes.Max(x => x.Numero) + 1
                                                                };

                                                                re[1] = lHq.UsiRest;

                                                                reDat.Restricoes.Add(re);


                                                                var val = new Compass.CommomLibrary.ReDat.ReValLine()
                                                                {
                                                                    Numero = re.Numero,
                                                                    Patamar = 0,
                                                                    ValorRestricao = restValor * produt65,
                                                                    Inicio = data,
                                                                    Fim = data,
                                                                };

                                                                reDat.Detalhes.Add(val);

                                                            }
                                                            //altera ou insere novo valor
                                                            else
                                                            {

                                                                var val = new Compass.CommomLibrary.ReDat.ReValLine()
                                                                {
                                                                    Numero = re.Numero,
                                                                    Patamar = 0,
                                                                    ValorRestricao = restValor * produt65,
                                                                    Inicio = data,
                                                                    Fim = data,
                                                                };

                                                                var anterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                    .Where(x => x.Inicio < val.Inicio && x.Fim >= val.Inicio).FirstOrDefault();
                                                                var posterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                    .Where(x => x.Inicio <= val.Fim && x.Fim > val.Fim).FirstOrDefault();

                                                                if (anterior != null)
                                                                {
                                                                    var anteriorSplit = anterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                                                                    anterior.Inicio = val.Inicio;
                                                                    anteriorSplit.Fim = val.Inicio.AddMonths(-1);

                                                                    reDat.Detalhes.Add(anteriorSplit);
                                                                }

                                                                if (posterior != null)
                                                                {
                                                                    var posteriorSplit = posterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                                                                    posterior.Fim = val.Fim; ;
                                                                    posteriorSplit.Inicio = val.Fim.AddMonths(1);

                                                                    reDat.Detalhes.Add(posteriorSplit);
                                                                }

                                                                reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                    .Where(x => x.Inicio >= val.Inicio && x.Fim <= val.Fim).ToList().ForEach(x =>
                                                                        reDat.Detalhes.Remove(x)
                                                                        );

                                                                reDat.Detalhes.Add(val);
                                                            }
                                                            var newl = reDat.Detalhes.OrderBy(x => x.Numero).ThenBy(x => x.Inicio).ToList();
                                                            reDat.Detalhes.Clear();
                                                            newl.ForEach(x => reDat.Detalhes.Add(x));
                                                            reDat.SaveToFile();
                                                        }
                                                        else // alterar modif com turbmaxt
                                                        {
                                                            if (nwhibrido)
                                                            {
                                                                string mineTurbVaz = lHq.Minemonico == "QTUR" ? "TURBMAXT" : "VAZMAXT";
                                                                modif = Services.DecompNextRev.AlterarModifComLq(modif, lHq, mineTurbVaz, data, lq[4], engolimento);
                                                            }
                                                            if (alterouRestAnalisar == true)
                                                            {
                                                                string mineTurbVaz = lHq.Minemonico == "QTUR" ? "VAZMINT" : "TURBMINT";//se Qtur superior =>altera nas Qdef logo Vazmint; se Qdef superior => altera nas Qtur logo Turbmint
                                                                modif = Services.DecompNextRev.AlterarModifComLq(modif, lHq, mineTurbVaz, data, valorAlterado, engolimento);

                                                            }
                                                            {
                                                                //if (!modif.Any(x => x.Usina == lHq.UsiRest))
                                                                //{
                                                                //    modif.Add(new Compass.CommomLibrary.ModifDatNW.ModifLine()
                                                                //    {
                                                                //        Usina = lHq.UsiRest,
                                                                //        Chave = "USINA",
                                                                //        NovosValores = new string[] { lHq.UsiRest.ToString() }
                                                                //    });

                                                                //}
                                                                //var modiflineTurb = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "TURBMAXT" && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();

                                                                //if (modiflineTurb != null)
                                                                //{
                                                                //    if (modiflineTurb.DataModif < data)
                                                                //    {

                                                                //        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                //        var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                //        var valorAntigo = modiflineTurb.ValorModif;


                                                                //        newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));
                                                                //        newModifLine.Chave = "TURBMAXT";
                                                                //        newModifLine.Usina = lHq.UsiRest;
                                                                //        int index = modif.IndexOf(modiflineTurb) + 1;
                                                                //        modif.Insert(index, newModifLine);

                                                                //        //mes seguinte verificação
                                                                //        var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "TURBMAXT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                                //        if (modiflineMesSeq == null)
                                                                //        {
                                                                //            //newModifLine2 = modifline;
                                                                //            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                                                                //            //newModifLine2.DataModif = data.AddMonths(1);
                                                                //            newModifLine2.Chave = "TURBMAXT";
                                                                //            newModifLine2.Usina = lHq.UsiRest;
                                                                //            int index2 = modif.IndexOf(newModifLine) + 1;
                                                                //            modif.Insert(index2, newModifLine2);
                                                                //        }


                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                //        var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                                //        var valorAntigo = modiflineTurb.ValorModif;

                                                                //        modiflineTurb.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));

                                                                //        //mes seguinte verificação
                                                                //        var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "TURBMAXT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                                //        if (modiflineMesSeq == null)
                                                                //        {
                                                                //            //newModifLine2 = modifline;
                                                                //            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                                                                //            //newModifLine2.DataModif = data.AddMonths(1);
                                                                //            newModifLine2.Chave = "TURBMAXT";
                                                                //            newModifLine2.Usina = lHq.UsiRest;
                                                                //            int index2 = modif.IndexOf(modiflineTurb) + 1;
                                                                //            modif.Insert(index2, newModifLine2);
                                                                //        }

                                                                //    }
                                                                //}
                                                                //else
                                                                //{
                                                                //    var mod = modif.Where(x => x.Usina == lHq.UsiRest).FirstOrDefault();
                                                                //    if (mod != null)
                                                                //    {
                                                                //        var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                                                                //        newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), lq[4].ToString().Replace(',', '.'));
                                                                //        newModifLine.Chave = "TURBMAXT";
                                                                //        newModifLine.Usina = lHq.UsiRest;
                                                                //        int indexT = modif.IndexOf(mod) + 1;
                                                                //        modif.Insert(indexT, newModifLine);
                                                                //    }
                                                                //}

                                                            }//codigo comentado


                                                        }
                                                    }
                                                    else//INF
                                                    {
                                                        lq[3] = valor > lq[4] ? lq[4] : valor;
                                                        lq[5] = valor > lq[4] ? lq[4] : valor;
                                                        lq[7] = valor > lq[4] ? lq[4] : valor;

                                                        bool alterouRestAnalisar = false;
                                                        double valorAlterado = 0;
                                                        //vereficar as restAnalisar
                                                        foreach (var restAna in restsAnalisar)//analisando limites superiores
                                                        {
                                                            LqLine restVer = dadger.BlocoRhq.Where(x => x.Restricao == restAna.First().Restricao).Where(y => y is Compass.CommomLibrary.Dadger.LqLine && ((LqLine)y).Estagio <= lq.Estagio).OrderByDescending(y => ((LqLine)y).Estagio).Select(y => (Compass.CommomLibrary.Dadger.LqLine)y).FirstOrDefault();
                                                            if (restVer != null)
                                                            {
                                                                if (restVer.LimSupPat1.HasValue && restVer.LimSupPat1 < lq[3])
                                                                {
                                                                    restVer.LimSupPat1 = lq[3];
                                                                    alterouRestAnalisar = true;
                                                                    valorAlterado = lq[3];
                                                                }

                                                                if (restVer.LimSupPat2.HasValue && restVer.LimSupPat2 < lq[3])
                                                                {
                                                                    restVer.LimSupPat2 = lq[3];
                                                                    alterouRestAnalisar = true;
                                                                    valorAlterado = lq[3];
                                                                }

                                                                if (restVer.LimSupPat3.HasValue && restVer.LimSupPat3 < lq[3])
                                                                {
                                                                    restVer.LimSupPat3 = lq[3];
                                                                    alterouRestAnalisar = true;
                                                                    valorAlterado = lq[3];
                                                                }
                                                            }

                                                        }

                                                        //var modifline = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                                                        if (lHq.InfSup == "INF" && lq[3] != null)
                                                        {

                                                            double modifval = lq[3];
                                                            string mineTurbVaz = lHq.Minemonico == "QTUR" ? "TURBMINT" : "VAZMINT";
                                                            modif = Services.DecompNextRev.AlterarModifComLq(modif, lHq, mineTurbVaz, data, modifval, engolimento);

                                                            if (nwhibrido)
                                                            {
                                                                if (alterouRestAnalisar == true)
                                                                {
                                                                    mineTurbVaz = lHq.Minemonico == "QTUR" ? "VAZMAXT" : "TURBMAXT";//se Qtur inferior =>altera nas Qdef logo Vazmaxt; se Qdef inferior => altera nas Qtur logo Turbmaxt
                                                                    modif = Services.DecompNextRev.AlterarModifComLq(modif, lHq, mineTurbVaz, data, valorAlterado, engolimento);
                                                                }
                                                            }
                                                            else if (alterouRestAnalisar == true && nwhibrido == false && produt65 >= 0)
                                                            {
                                                                double restValor = valorAlterado;
                                                                foreach (var reRest in reDat.Restricoes.ToList())
                                                                {
                                                                    foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == reRest.Numero).ToList())
                                                                    {

                                                                        if (reDet.Inicio < deckNWEstudo.Dger.DataEstudo && reDet.Fim >= deckNWEstudo.Dger.DataEstudo)
                                                                        {
                                                                            reDet.Inicio = deckNWEstudo.Dger.DataEstudo;
                                                                        }
                                                                        else if (reDet.Fim < deckNWEstudo.Dger.DataEstudo)
                                                                        {
                                                                            reDat.Detalhes.Remove(reDet);
                                                                        }
                                                                    }

                                                                    if (reDat.Detalhes.Where(x => x.Numero == reRest.Numero).Count() == 0) reDat.Restricoes.Remove(reRest);
                                                                }
                                                                //procura restricao
                                                                var re = reDat.Restricoes.Where(
                                                                    x => String.Join("", x.Valores.Skip(1).Where(y => y != null).OrderBy(y => y).Select(y => y.ToString().Trim()))
                                                                        == String.Join("", lHq.UsiRest.ToString())
                                                                    ).FirstOrDefault();

                                                                //se nao exite insere
                                                                if (re == null)
                                                                {

                                                                    re = new Compass.CommomLibrary.ReDat.ReLine()
                                                                    {
                                                                        Numero = reDat.Restricoes.Max(x => x.Numero) + 1
                                                                    };

                                                                    re[1] = lHq.UsiRest;

                                                                    reDat.Restricoes.Add(re);


                                                                    var val = new Compass.CommomLibrary.ReDat.ReValLine()
                                                                    {
                                                                        Numero = re.Numero,
                                                                        Patamar = 0,
                                                                        ValorRestricao = restValor * produt65,
                                                                        Inicio = data,
                                                                        Fim = data,
                                                                    };

                                                                    reDat.Detalhes.Add(val);

                                                                }
                                                                //altera ou insere novo valor
                                                                else
                                                                {

                                                                    var val = new Compass.CommomLibrary.ReDat.ReValLine()
                                                                    {
                                                                        Numero = re.Numero,
                                                                        Patamar = 0,
                                                                        ValorRestricao = restValor * produt65,
                                                                        Inicio = data,
                                                                        Fim = data,
                                                                    };

                                                                    var anterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                        .Where(x => x.Inicio < val.Inicio && x.Fim >= val.Inicio).FirstOrDefault();
                                                                    var posterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                        .Where(x => x.Inicio <= val.Fim && x.Fim > val.Fim).FirstOrDefault();

                                                                    if (anterior != null)
                                                                    {
                                                                        var anteriorSplit = anterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                                                                        anterior.Inicio = val.Inicio;
                                                                        anteriorSplit.Fim = val.Inicio.AddMonths(-1);

                                                                        reDat.Detalhes.Add(anteriorSplit);
                                                                    }

                                                                    if (posterior != null)
                                                                    {
                                                                        var posteriorSplit = posterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                                                                        posterior.Fim = val.Fim; ;
                                                                        posteriorSplit.Inicio = val.Fim.AddMonths(1);

                                                                        reDat.Detalhes.Add(posteriorSplit);
                                                                    }

                                                                    reDat.Detalhes.Where(x => x.Numero == val.Numero)
                                                                        .Where(x => x.Inicio >= val.Inicio && x.Fim <= val.Fim).ToList().ForEach(x =>
                                                                            reDat.Detalhes.Remove(x)
                                                                            );

                                                                    reDat.Detalhes.Add(val);
                                                                }
                                                                var newl = reDat.Detalhes.OrderBy(x => x.Numero).ThenBy(x => x.Inicio).ToList();
                                                                reDat.Detalhes.Clear();
                                                                newl.ForEach(x => reDat.Detalhes.Add(x));
                                                                reDat.SaveToFile();
                                                            }
                                                        }

                                                    }

                                                    /////////
                                                    {

                                                        //var modifline = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();

                                                        //if (lHq.InfSup == "INF" && lq[3] != null)
                                                        //{
                                                        //    double modifval = lq[3];
                                                        //    if (modifline != null)
                                                        //    {
                                                        //        if (modifline.DataModif < data)
                                                        //        {

                                                        //            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                        //            var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                        //            var valorAntigo = modifline.ValorModif;


                                                        //            newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));
                                                        //            newModifLine.Chave = "VAZMINT";
                                                        //            newModifLine.Usina = lHq.UsiRest;
                                                        //            int index = modif.IndexOf(modifline) + 1;
                                                        //            modif.Insert(index, newModifLine);

                                                        //            //mes seguinte verificação
                                                        //            var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                        //            if (modiflineMesSeq == null)
                                                        //            {
                                                        //                //newModifLine2 = modifline;
                                                        //                newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                                                        //                //newModifLine2.DataModif = data.AddMonths(1);
                                                        //                newModifLine2.Chave = "VAZMINT";
                                                        //                newModifLine2.Usina = lHq.UsiRest;
                                                        //                int index2 = modif.IndexOf(newModifLine) + 1;
                                                        //                modif.Insert(index2, newModifLine2);
                                                        //            }


                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                        //            var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                        //            var valorAntigo = modifline.ValorModif;

                                                        //            modifline.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));

                                                        //            //mes seguinte verificação
                                                        //            var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == "VAZMINT" && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                        //            if (modiflineMesSeq == null)
                                                        //            {
                                                        //                //newModifLine2 = modifline;
                                                        //                newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                                                        //                //newModifLine2.DataModif = data.AddMonths(1);
                                                        //                newModifLine2.Chave = "VAZMINT";
                                                        //                newModifLine2.Usina = lHq.UsiRest;
                                                        //                int index2 = modif.IndexOf(modifline) + 1;
                                                        //                modif.Insert(index2, newModifLine2);
                                                        //            }

                                                        //        }
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        var mod = modif.Where(x => x.Usina == lHq.UsiRest).FirstOrDefault();
                                                        //        if (mod != null)
                                                        //        {
                                                        //            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                                                        //            newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'));
                                                        //            newModifLine.Chave = "VAZMINT";
                                                        //            newModifLine.Usina = lHq.UsiRest;
                                                        //            int index = modif.IndexOf(mod) + 1;
                                                        //            modif.Insert(index, newModifLine);
                                                        //        }


                                                        //    }
                                                        //}
                                                    }//codigo comentado



                                                    modif.SaveToFile(filePath: modifFile);
                                                    /////////
                                                }

                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (limitesHV.Count() > 0)
                        {
                            foreach (var lHv in limitesHV)
                            {
                                // dynamic lq;

                                var UH = dadger.BlocoUh.Where(x => x.Usina == lHv.UH.First()).FirstOrDefault();

                                double hectoMin = configH.Usinas.Any(x => x.Cod == lHv.UsiRest) ? configH.Usinas.Where(x => x.Cod == lHv.UsiRest).Select(x => x.VolMin).First() : -1;

                                if (UH != null)
                                {

                                    var rests = dadger.BlocoRhv.Where(x => x.Restricao == lHv.CodRest);

                                    double percentAlvo = UH.VolIniPerc;

                                    if (lHv.UH.Count() > 1)
                                    {
                                        percentAlvo = Services.DecompNextRev.GetpercentAlvo(configH, lHv.UH);
                                    }

                                    if (rests.Count() > 0)
                                    {
                                        var le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LvLine).Select(x => (Compass.CommomLibrary.Dadger.LvLine)x);
                                        // var lvs = le.Where(x => x.Estagio <= dadger.VAZOES_NumeroDeSemanas).ToList();
                                        dynamic lvdummy = le.Where(x => x.Estagio <= dadger.VAZOES_NumeroDeSemanas + 1).OrderByDescending(x => x.Estagio).FirstOrDefault();

                                        if (lvdummy.Estagio < dadger.VAZOES_NumeroDeSemanas + 1)//caso não exista o estagio do segundo mes informado, copia os dados do ultimo estagio informado para o segundo mes
                                        {

                                            var nledummy = lvdummy.Clone();
                                            nledummy.Estagio = dadger.VAZOES_NumeroDeSemanas + 1;
                                            dadger.BlocoRhv.Add(nledummy);
                                        }

                                        rests = dadger.BlocoRhv.Where(x => x.Restricao == lHv.CodRest);
                                        le = rests.Where(x => x is Compass.CommomLibrary.Dadger.LvLine).Select(x => (Compass.CommomLibrary.Dadger.LvLine)x);
                                        var lvs = le.Where(x => x.Estagio <= dadger.VAZOES_NumeroDeSemanas + 1).ToList();

                                        if (lvs.Count() > 0)
                                        {
                                            foreach (var lv in lvs)
                                            {
                                                modif = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
                                                var modifFile = modif.File;

                                                string minemonico = "";
                                                double valorTemp;

                                                DateTime data;
                                                data = new DateTime(dtEstudo.Year, dtEstudo.Month, 1);

                                                double valor = 0;
                                                valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHv, w.Faixapercents.First());

                                                if (lv.Estagio == dadger.VAZOES_NumeroDeSemanas + 1)
                                                {
                                                    data = mesSeg;
                                                    var lHvSEg = w.Faixalimites.Where(x => x.MesIni <= mesSeg.Month && x.MesFim >= mesSeg.Month && x.Ativa == true && x.UsiRest == lHv.UsiRest && x.UH.All(lHv.UH.Contains) && x.UH.Count == lHv.UH.Count && x.InfSup == lHv.InfSup && x.TipoRest.ToUpper().Equals("HV")).FirstOrDefault();
                                                    if (lHvSEg != null)
                                                    {
                                                        valor = Services.DecompNextRev.GetLimitesPorFaixa(percentAlvo, lHvSEg, w.Faixapercents.First());
                                                    }
                                                }

                                                if (lHv.InfSup == "SUP")
                                                {
                                                    lv[4] = valor < lv[3] ? lv[3] : valor;
                                                    minemonico = "VMAXT";
                                                    valorTemp = lv[4];
                                                }
                                                else
                                                {
                                                    lv[3] = valor > lv[4] ? lv[4] : valor;
                                                    minemonico = "VMINT";
                                                    valorTemp = lv[3];
                                                }

                                                /////////

                                                if (hectoMin >= 0)
                                                {
                                                    var modifline = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                                                    double modifval = valorTemp + hectoMin;
                                                    if (modifline != null)
                                                    {
                                                        if (modifline.DataModif < data)
                                                        {

                                                            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                            var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                            var valorAntigo = modifline.ValorModif;


                                                            newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");
                                                            newModifLine.Chave = minemonico;
                                                            newModifLine.Usina = lHv.UsiRest;
                                                            int index = modif.IndexOf(modifline) + 1;
                                                            modif.Insert(index, newModifLine);

                                                            //mes seguinte verificação
                                                            var modiflineMesSeq = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                            if (modiflineMesSeq == null)
                                                            {
                                                                //newModifLine2 = modifline;
                                                                newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'), "'h'");
                                                                //newModifLine2.DataModif = data.AddMonths(1);
                                                                newModifLine2.Chave = minemonico;
                                                                newModifLine2.Usina = lHv.UsiRest;
                                                                int index2 = modif.IndexOf(newModifLine) + 1;
                                                                modif.Insert(index2, newModifLine2);
                                                            }


                                                        }
                                                        else
                                                        {
                                                            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                            var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                                            var valorAntigo = modifline.ValorModif;

                                                            modifline.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");

                                                            //mes seguinte verificação
                                                            var modiflineMesSeq = modif.Where(x => x.Usina == lHv.UsiRest && x.Chave == minemonico && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                                                            if (modiflineMesSeq == null)
                                                            {
                                                                //newModifLine2 = modifline;
                                                                newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'), "'h'");
                                                                //newModifLine2.DataModif = data.AddMonths(1);
                                                                newModifLine2.Chave = minemonico;
                                                                newModifLine2.Usina = lHv.UsiRest;
                                                                int index2 = modif.IndexOf(modifline) + 1;
                                                                modif.Insert(index2, newModifLine2);
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        var mod = modif.Where(x => x.Usina == lHv.UsiRest).FirstOrDefault();
                                                        if (mod != null)
                                                        {
                                                            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                                                            newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), modifval.ToString().Replace(',', '.'), "'h'");
                                                            newModifLine.Chave = minemonico;
                                                            newModifLine.Usina = lHv.UsiRest;
                                                            int index = modif.IndexOf(mod) + 1;
                                                            modif.Insert(index, newModifLine);
                                                        }


                                                    }

                                                    modif.SaveToFile(filePath: modifFile);
                                                }

                                                /////////

                                            }

                                        }
                                    }



                                }
                            }
                        }

                        //todo excluir os turbmax caso só exista dados com 99999

                        var modifsFinal = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
                        var modifFileFinal = modifsFinal.File;

                        var usinasTurbmaxtFinal = modifsFinal.Where(x => x.Chave == "TURBMAXT").Select(x => x.Usina).Distinct();
                        List<Compass.CommomLibrary.ModifDatNW.ModifLine> removerFinal = new List<Compass.CommomLibrary.ModifDatNW.ModifLine>();

                        foreach (var usiT in usinasTurbmaxtFinal)
                        {
                            var modifsremove = modifsFinal.Where(x => x.Usina == usiT && x.Chave == "TURBMAXT").ToList();
                            if (modifsremove.All(x => x.ValorModif == 99999))
                            {
                                modifsremove.ForEach(x => removerFinal.Add(x));
                            }
                        }

                        removerFinal.ForEach(x => modifsFinal.Remove(x));

                        removerFinal.Clear();

                        var usinaSemMine = modifsFinal.Select(x => x.Usina).Distinct();

                        foreach (var usiT in usinaSemMine)
                        {
                            var modifsremove = modifsFinal.Where(x => x.Usina == usiT).ToList();
                            if (modifsremove.Count() == 1 && modifsremove[0].Chave.ToUpper().Trim() == "USINA")
                            {
                                removerFinal.Add(modifsremove[0]);
                            }
                        }
                        removerFinal.ForEach(x => modifsFinal.Remove(x));
                        modifsFinal.SaveToFile(filePath: modifFileFinal);

                        dadger.SaveToFile();

                        if (w.Faixapercents.Count() > 0 && w.Faixalimites.Count() > 0)
                        {
                            List<string> faixaText = new List<string>();
                            string header = "UH;TIPO REST;USINA REST;COD REST;MÊS INI;MÊS FIM;INF/SUP;ATIVA";
                            w.Faixapercents.First().Percents.ForEach(x => header = header + ";" + x.ToString() + "%");

                            faixaText.Add(header);
                            w.Faixalimites.ForEach(x =>
                            {
                                string linha;
                                linha = string.Join(";", x.UHstring, x.TipoRest, x.UsiRest.ToString(), x.CodRest.ToString(), x.MesIni.ToString(), x.MesFim.ToString(), x.InfSup.ToString(), x.Ativa.ToString()) + ";";
                                linha = linha + string.Join(";", x.Vals.ToList());
                                faixaText.Add(linha);
                                // x.Vals.ForEach(y => { linha = linha + y.ToString(); });
                            });
                            File.WriteAllLines(Path.Combine(estudoPath, "LIMITES_DE_RESTRICAO.txt"), faixaText);
                        }

                    }

                    //fim codigofaixas limites

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

                            if (w.Dadterm.Count() > 0)
                            {
                                var dadAdterms = w.Dadterm.Where(x => x.usina == glLine.NumeroUsina && x.ano == dtEstudo.Year && x.mes == dtEstudo.Month && x.estagio == 1).FirstOrDefault();
                                if (dadAdterms != null)
                                {
                                    glLine.GeracaoPat1 = (float)dadAdterms.PT1;
                                    glLine.GeracaoPat2 = (float)dadAdterms.PT2;
                                    glLine.GeracaoPat3 = (float)dadAdterms.PT3;
                                }
                                else
                                {
                                    glLine.GeracaoPat1 = 0;
                                    glLine.GeracaoPat2 = 0;
                                    glLine.GeracaoPat3 = 0;
                                }
                            }


                            dadgnl.BlocoGL.Add(glLine.Clone());
                        }

                        var dtTemp = mesOperativo.Fim.AddDays(1);

                        for (int _e = mesOperativo.EstagiosReaisDoMesAtual; _e < 9; _e++)
                        {

                            var endSemanaTemp = dtTemp.AddDays(6);
                            if (_e > mesOperativo.EstagiosReaisDoMesAtual && endSemanaTemp.Day < 7) endSemanaTemp = endSemanaTemp.AddDays(-endSemanaTemp.Day);


                            var semanaOperativaTemp = new SemanaOperativa(dtTemp, endSemanaTemp, patamares2019, patamares2023, patamares2024, patamares2025);


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

                            if (w.Dadterm.Count() > 0)
                            {
                                var dadAdterms = w.Dadterm.Where(x => x.usina == glLine.NumeroUsina && x.ano == dtEstudo.Year && x.mes == dtEstudo.Month && x.estagio == 2).FirstOrDefault();
                                if (dadAdterms != null)
                                {
                                    glLine.GeracaoPat1 = (float)dadAdterms.PT1;
                                    glLine.GeracaoPat2 = (float)dadAdterms.PT2;
                                    glLine.GeracaoPat3 = (float)dadAdterms.PT3;
                                }
                                else
                                {
                                    glLine.GeracaoPat1 = 0;
                                    glLine.GeracaoPat2 = 0;
                                    glLine.GeracaoPat3 = 0;
                                }
                            }

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


        private static List<Tuple<int, int, DateTime, double>> getEolicasplan(string planMemo)//sub,pat,data,dado
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            List<Tuple<int, int, DateTime, double>> eolicasDados = new List<Tuple<int, int, DateTime, double>>();
            List<string> nomeSheets = new List<string>
                        {
                            "Geração Não Simuladas (P)",
                            "Geração Não Simuladas (M)",
                            "Geração Não Simuladas (L)"
                        };
            //Microsoft.Office.Interop.Excel.Application xlApp = ExcelTools.Helper.StartExcelInvisible();
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(planMemo)))
            {

                foreach (var nome in nomeSheets)
                {
                    var myWorksheet = xlPackage.Workbook.Worksheets[nome]; //select sheet here
                    var totalRows = myWorksheet.Dimension.End.Row;
                    var totalColumns = myWorksheet.Dimension.End.Column;
                    bool find = false;
                    int rowTD = 1;

                    int pat;
                    switch (nome)
                    {
                        case "Geração Não Simuladas (P)":
                            pat = 1;
                            break;
                        case "Geração Não Simuladas (M)":
                            pat = 2;
                            break;
                        case "Geração Não Simuladas (L)":
                            pat = 3;
                            break;

                        default:
                            pat = 0;
                            break;
                    }

                    while (find == false)
                    {
                        try
                        {
                            string texto = myWorksheet.Cells["A" + rowTD].Value.ToString();
                            if (texto == "Total")
                            {
                                int rowSub = rowTD;


                                bool findSUL = false;
                                bool findNE = false;

                                while (findSUL == false)
                                {
                                    try
                                    {
                                        string sub = myWorksheet.Cells["B" + rowSub].Value.ToString();
                                        if (sub == "S")
                                        {
                                            int rowFont = rowSub;
                                            bool findfont = false;
                                            while (findfont == false)
                                            {
                                                try
                                                {
                                                    string font = myWorksheet.Cells["C" + rowFont].Value.ToString();
                                                    if (font == "EOL")
                                                    {
                                                        int rowDado = rowFont;
                                                        for (var c = 4; !string.IsNullOrWhiteSpace(myWorksheet.Cells[rowTD, c].Text); c++)
                                                        {
                                                            var dt = Convert.ToDateTime(myWorksheet.Cells[rowTD, c].Value, Culture.DateTimeFormat);
                                                            DateTime data = Convert.ToDateTime(myWorksheet.Cells[rowTD, c].Value, Culture.DateTimeFormat);
                                                            if (myWorksheet.Cells[rowDado, c].Value != null)
                                                            {
                                                                double eolicasul = Convert.ToDouble(myWorksheet.Cells[rowDado, c].Value.ToString());
                                                                eolicasDados.Add(new Tuple<int, int, DateTime, double>(2, pat, data, eolicasul));
                                                            }

                                                        }
                                                        findfont = true;
                                                    }
                                                }
                                                catch { }
                                                rowFont++;
                                            }
                                            findSUL = true;
                                        }
                                    }
                                    catch { }
                                    rowSub++;
                                }
                                //
                                rowSub = rowTD;

                                while (findNE == false)
                                {
                                    try
                                    {
                                        string sub = myWorksheet.Cells["B" + rowSub].Value.ToString();
                                        if (sub == "NE")
                                        {
                                            int rowFont = rowSub;
                                            bool findfont = false;
                                            while (findfont == false)
                                            {
                                                try
                                                {
                                                    string font = myWorksheet.Cells["C" + rowFont].Value.ToString();
                                                    if (font == "EOL")
                                                    {
                                                        int rowDado = rowFont;
                                                        for (var c = 4; !string.IsNullOrWhiteSpace(myWorksheet.Cells[rowTD, c].Text); c++)
                                                        {
                                                            DateTime data = Convert.ToDateTime(myWorksheet.Cells[rowTD, c].Value, Culture.DateTimeFormat);
                                                            if (myWorksheet.Cells[rowDado, c].Value != null)
                                                            {
                                                                double eolicaNe = Convert.ToDouble(myWorksheet.Cells[rowDado, c].Value.ToString());
                                                                eolicasDados.Add(new Tuple<int, int, DateTime, double>(3, pat, data, eolicaNe));
                                                            }

                                                        }
                                                        findfont = true;
                                                    }
                                                }
                                                catch { }
                                                rowFont++;
                                            }
                                            findNE = true;
                                        }
                                    }
                                    catch { }
                                    rowSub++;
                                }
                                //
                                find = true;
                                break;
                            }
                        }
                        catch { }
                        rowTD++;
                    }
                }

            }
            return eolicasDados;
        }
    }
}

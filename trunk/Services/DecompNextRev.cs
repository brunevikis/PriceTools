using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.Decomp;
using Compass.ExcelTools.Templates;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Compass.Services.DB;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Compass.Services
{
    public class DecompNextRev
    {

        public static void CreateNextRev(Compass.CommomLibrary.Decomp.Deck baseDeck, string outPath)
        {

            //avançar dadgnl
            var dadgnl = (Compass.CommomLibrary.Dadgnl.Dadgnl)
                Compass.CommomLibrary.DocumentFactory.Create(
                    baseDeck[DeckDocument.dadgnl].BasePath
                );

            //avançar dadger
            var dadger = (Compass.CommomLibrary.Dadger.Dadger)
                Compass.CommomLibrary.DocumentFactory.Create(
                    baseDeck[DeckDocument.dadger].BasePath
                );

            //gravar arquivos
            baseDeck.Rev++;
            baseDeck.Caso = "rv" + (baseDeck.Rev).ToString();

            dadgnl.File = System.IO.Path.Combine(outPath, "dadgnl." + baseDeck.Caso);
            dadger.File = System.IO.Path.Combine(outPath, "dadger." + baseDeck.Caso);

            dadgnl.SaveToFile();
            dadger.SaveToFile();

        }
        public static double GetpercentAlvo(Compass.CommomLibrary.Decomp.ConfigH configH, List<int> UHs)
        {
            double percentAlvo = configH.GetpercentAlvo(UHs);

            return percentAlvo;
        }

        public static Compass.CommomLibrary.ModifDatNW.ModifDatNw AlterarModifComLq(Compass.CommomLibrary.ModifDatNW.ModifDatNw modif, WorkbookMensal.FAIXALIMITES lHq, string mineTurbVaz, DateTime data, double lqValor, double engolimento)
        {
            //TODO no uso de engolimento trocar para 99999
            bool usarEngolimento = false;
            double valorModif = lqValor;
            double valor99999 = 99999;

            if (mineTurbVaz == "TURBMAXT")
            {
                usarEngolimento = true;
                //valorModif = lqValor > engolimento ? engolimento : lqValor;
                valorModif = lqValor >= engolimento ? valor99999 : lqValor;
            }

            var modifFile = modif.File;
            if (!modif.Any(x => x.Usina == lHq.UsiRest))
            {
                modif.Add(new Compass.CommomLibrary.ModifDatNW.ModifLine()
                {
                    Usina = lHq.UsiRest,
                    Chave = "USINA",
                    NovosValores = new string[] { lHq.UsiRest.ToString() }
                });

            }
            var modiflineTurbVaz = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == mineTurbVaz && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();

            if (modiflineTurbVaz != null)
            {
                if (modiflineTurbVaz.DataModif < data)
                {

                    var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                    var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                    var valorAntigo = modiflineTurbVaz.ValorModif;
                    if (usarEngolimento == true)
                    {
                        //valorAntigo = valorAntigo < engolimento ? valorAntigo : engolimento;
                        valorAntigo = valorAntigo < engolimento ? valorAntigo : valor99999;
                    }

                    newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), valorModif.ToString().Replace(',', '.'));
                    newModifLine.Chave = mineTurbVaz;
                    newModifLine.Usina = lHq.UsiRest;
                    int index = modif.IndexOf(modiflineTurbVaz) + 1;
                    modif.Insert(index, newModifLine);

                    //mes seguinte verificação
                    var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == mineTurbVaz && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                    if (modiflineMesSeq == null)
                    {
                        //newModifLine2 = modifline;
                        newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                        //newModifLine2.DataModif = data.AddMonths(1);
                        newModifLine2.Chave = mineTurbVaz;
                        newModifLine2.Usina = lHq.UsiRest;
                        int index2 = modif.IndexOf(newModifLine) + 1;
                        modif.Insert(index2, newModifLine2);
                    }
                    else if (modiflineMesSeq != null && usarEngolimento == true)
                    {
                        double valorAusar = modiflineMesSeq.ValorModif ?? engolimento;
                        //valorAusar = valorAusar < engolimento ? valorAusar : engolimento;
                        valorAusar = valorAusar < engolimento ? valorAusar : valor99999;
                        modiflineMesSeq.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAusar.ToString().Replace(',', '.'));
                    }

                }
                else
                {
                    var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                    var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                    var valorAntigo = modiflineTurbVaz.ValorModif;

                    if (usarEngolimento == true)
                    {
                        //valorAntigo = valorAntigo < engolimento ? valorAntigo : engolimento;
                        valorAntigo = valorAntigo < engolimento ? valorAntigo : valor99999;
                    }

                    modiflineTurbVaz.SetValores(data.Month.ToString(), data.Year.ToString(), valorModif.ToString().Replace(',', '.'));

                    //mes seguinte verificação
                    var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == mineTurbVaz && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                    if (modiflineMesSeq == null)
                    {

                        //newModifLine2 = modifline;
                        newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAntigo.ToString().Replace(',', '.'));
                        //newModifLine2.DataModif = data.AddMonths(1);
                        newModifLine2.Chave = mineTurbVaz;
                        newModifLine2.Usina = lHq.UsiRest;
                        int index2 = modif.IndexOf(modiflineTurbVaz) + 1;
                        modif.Insert(index2, newModifLine2);
                    }
                    else if (modiflineMesSeq != null && usarEngolimento == true)
                    {
                        double valorAusar = modiflineMesSeq.ValorModif ?? engolimento;
                       // valorAusar = valorAusar < engolimento ? valorAusar : engolimento;
                        valorAusar = valorAusar < engolimento ? valorAusar : valor99999;
                        modiflineMesSeq.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAusar.ToString().Replace(',', '.'));
                    }

                }
            }
            else
            {
                var mod = modif.Where(x => x.Usina == lHq.UsiRest).FirstOrDefault();
                if (mod != null)
                {
                    var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                    newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), valorModif.ToString().Replace(',', '.'));
                    newModifLine.Chave = mineTurbVaz;
                    newModifLine.Usina = lHq.UsiRest;
                    int indexT = modif.IndexOf(mod) + 1;
                    modif.Insert(indexT, newModifLine);

                    if (usarEngolimento == true)
                    {
                        var modiflineMesSeq = modif.Where(x => x.Usina == lHq.UsiRest && x.Chave == mineTurbVaz && x.DataModif == data.AddMonths(1)).FirstOrDefault();
                        if (modiflineMesSeq == null)
                        {
                            var newModifLine2 = new Compass.CommomLibrary.ModifDatNW.ModifLine();


                            //newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), engolimento.ToString().Replace(',', '.'));
                            newModifLine2.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valor99999.ToString().Replace(',', '.'));
                            newModifLine2.Chave = mineTurbVaz;
                            newModifLine2.Usina = lHq.UsiRest;
                            int indexT2 = modif.IndexOf(newModifLine) + 1;
                            modif.Insert(indexT2, newModifLine2);
                        }
                        else
                        {
                            double valorAusar = modiflineMesSeq.ValorModif ?? engolimento;
                            //valorAusar = valorAusar < engolimento ? valorAusar : engolimento;
                            valorAusar = valorAusar < engolimento ? valorAusar : valor99999;
                            modiflineMesSeq.SetValores(data.AddMonths(1).Month.ToString(), data.AddMonths(1).Year.ToString(), valorAusar.ToString().Replace(',', '.'));
                        }

                    }
                }

            }

            return modif;
        }

        public static double GetLimitesPorFaixa(double voliniPerc, WorkbookMensal.FAIXALIMITES faixaLimite, WorkbookMensal.FAIXAPERCENTS faixaPercent)
        {
            double valor = 0;
            for (int i = 0; i < faixaPercent.Percents.Count(); i++)
            {
                if (voliniPerc <= faixaPercent.Percents[i])
                {
                    return faixaLimite.Vals[i];
                }
            }

            return valor;
        }

        public static void IncrementaLibsCSV(Compass.CommomLibrary.Decomp.Deck deckEstudo, Compass.CommomLibrary.Newave.Deck deckNWEstudo, DateTime dtEstudo)
        {
            var parEolFte = deckEstudo[Compass.CommomLibrary.Decomp.DeckDocument.parqueeolfte] != null ? deckEstudo[Compass.CommomLibrary.Decomp.DeckDocument.parqueeolfte].Document as Compass.CommomLibrary.ParqueEolico.Fte : null;

            var parEolConfig = deckEstudo[Compass.CommomLibrary.Decomp.DeckDocument.parqueeolconfig] != null ? deckEstudo[Compass.CommomLibrary.Decomp.DeckDocument.parqueeolconfig].Document as Compass.CommomLibrary.ParqueEolico.Config : null;

            var parEolPotInst = deckEstudo[Compass.CommomLibrary.Decomp.DeckDocument.parqueeolpot] != null ? deckEstudo[Compass.CommomLibrary.Decomp.DeckDocument.parqueeolpot].Document as Compass.CommomLibrary.ParqueEolico.PotInst : null;

            var eolCad = deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicacad] != null ? deckNWEstudo[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicacad].Document as Compass.CommomLibrary.EolicaNW.EolicaCad : null;


            if (parEolFte != null)
            {
                var parEolFile = parEolFte.File;
                parEolFte.BlocoFte.ToList().ForEach(y => y.PeriodoFim = "");
                parEolFte.SaveToFile(filePath: parEolFile);
            }

            if (parEolConfig != null)
            {
                var parEolConfigFile = parEolConfig.File;
                parEolConfig.BlocoConfig.ToList().ForEach(y => y.PeriodoFim = "");
                parEolConfig.SaveToFile(filePath: parEolConfigFile);
            }

            if (parEolPotInst != null && eolCad != null)
            {
                DateTime dataMseguinte = new DateTime(dtEstudo.AddMonths(1).Year, dtEstudo.AddMonths(1).Month, 1);
                var parEolPotFile = parEolPotInst.File;
                parEolPotInst.BlocoPotInst.ToList().ForEach(y =>
                {
                    y.PeriodoIni = 1;
                    y.PeriodoFim = "";
                    y.Pot = eolCad.BlocoPeePot.Where(x => x.CodPEE == y.CodPEE && x.DataFim <= dataMseguinte).OrderByDescending(x => x.DataFim).Select(x => x.Potencia).FirstOrDefault();
                });
                parEolPotInst.SaveToFile(filePath: parEolPotFile);
            }
        }

        public static Dadger CreateRv0(Compass.CommomLibrary.Decomp.Deck deckEstudo, Compass.CommomLibrary.Newave.Deck deckNWEstudo, DateTime dtEstudo, WorkbookMensal w, MesOperativo mesOperativo, Compass.CommomLibrary.Pmo.Pmo pmoBase, List<Tuple<int, int, DateTime, double>> eolicasDados = null, bool nWh = false)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var dadger = deckEstudo[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger;

            var isMensal = mesOperativo.Inicio.Day == 1 && mesOperativo.Estagios == 1;

            dadger.DataEstudo = mesOperativo.Inicio;
            dadger.VAZOES_DataDoEstudo = dtEstudo;
            dadger.VAZOES_NumeroDeSemanas = isMensal ? 0 : mesOperativo.Estagios;
            dadger.VAZOES_NumeroDeSemanasPassadas = 0;
            dadger.VAZOES_NumeroDiasDoMes2 = mesOperativo.DiasMes2;
            dadger.VAZOES_EstruturaDaArvore = w.ArvoreSegundoMes[dtEstudo.Month - 1];
            //dadger.VAZOES_AnoTendeciaHidrologica = dtEstudo.Year - 1;


            var patamarDat = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
            var modifNW = BaseDocument.Create<Compass.CommomLibrary.ModifDatNW.ModifDatNw>(System.IO.File.ReadAllText(deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.modif].BasePath));
            var sistemaDat = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as CommomLibrary.SistemaDat.SistemaDat;
            bool pees = w.PEEs;

            #region ct

            var cts = dadger.BlocoCT.Where(x => x.Estagio == 1).ToList();

            dadger.BlocoCT.Clear();


            var exptDat = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.expt].Document as CommomLibrary.ExptDat.ExptDat).ToList()
                .OrderBy(c => c.Cod).ThenBy(x => x.DataInicio);
            var termDat = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.term].Document as CommomLibrary.TermDat.TermDat).ToList().OrderBy(x => x.Cod);
            var conftDat = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.conft].Document as CommomLibrary.ConftDat.ConftDat).ToList().OrderBy(x => x.Num);
            var manutDat = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.manutt].Document as CommomLibrary.ManuttDat.ManuttDat).ToList().OrderBy(x => x.Cod);
            var clastDat = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.clast].Document as CommomLibrary.ClastDat.ClastDat)[dtEstudo].ToList().OrderBy(x => x.Num);


            List<CtLine> lstCT = new List<CtLine>();

            foreach (var term in termDat)
            {
                string nomeUsina = term.Usina;

                var clast = clastDat.First(x => x.Num == term.Cod);
                var conft = conftDat.First(x => x.Num == term.Cod);

                var potef = term.Potencia;
                var potefMais = term.Potencia;
                var fcmax = term.Fcmx;
                var fcmaxMais = term.Fcmx;
                var teif = term.Teif;
                var teifMais = term.Teif;

                double gtmin = pmoBase.Blocos["GTERM Min"]
                    .Where(x => x[0] == term.Cod)
                    .Select(x => x[(dtEstudo.Year - x[2]) * 12 + dtEstudo.Month + 2]).FirstOrDefault(); // Inflex
                double gtminMais = pmoBase.Blocos["GTERM Min"]
                    .Where(x => x[0] == term.Cod)
                    .Select(x => x[(dtEstudo.AddMonths(1).Year - x[2]) * 12 + dtEstudo.AddMonths(1).Month + 2]).FirstOrDefault(); // Inflex
                var ipterMais = term.Ipter;

                if (!String.Equals(conft.Existente, "EX", StringComparison.OrdinalIgnoreCase))
                {

                    var exptUsina = exptDat.Where(x => x.Cod == term.Cod)
                        .Where(x => x.DataInicio <= dtEstudo && x.DataFim >= dtEstudo);
                    var exptUsinaMais = exptDat.Where(x => x.Cod == term.Cod)
                        .Where(x => x.DataInicio <= dtEstudo.AddMonths(1) && x.DataFim >= dtEstudo.AddMonths(1));

                    //potef
                    if (exptUsina.Where(x => x.Tipo == "POTEF").Count() > 0) potef = exptUsina.Where(x => x.Tipo == "POTEF").Last().Valor;
                    if (exptUsinaMais.Where(x => x.Tipo == "POTEF").Count() > 0) potefMais = exptUsinaMais.Where(x => x.Tipo == "POTEF").Last().Valor;

                    //fcmax
                    if (exptUsina.Where(x => x.Tipo == "FCMAX").Count() > 0) fcmax = exptUsina.Where(x => x.Tipo == "FCMAX").Last().Valor;
                    if (exptUsinaMais.Where(x => x.Tipo == "FCMAX").Count() > 0) fcmaxMais = exptUsinaMais.Where(x => x.Tipo == "FCMAX").Last().Valor;

                    //teif
                    if (exptUsina.Where(x => x.Tipo == "TEIFT").Count() > 0) teif = exptUsina.Where(x => x.Tipo == "TEIFT").Last().Valor;
                    if (exptUsinaMais.Where(x => x.Tipo == "TEIFT").Count() > 0) teifMais = exptUsinaMais.Where(x => x.Tipo == "TEIFT").Last().Valor;

                    //gtmin = 
                    if (exptUsina.Where(x => x.Tipo == "GTMIN").Count() > 0) gtmin = exptUsina.Where(x => x.Tipo == "GTMIN").Last().Valor;
                    if (exptUsinaMais.Where(x => x.Tipo == "GTMIN").Count() > 0) gtminMais = exptUsinaMais.Where(x => x.Tipo == "GTMIN").Last().Valor;

                }

                if (!String.Equals(clast.Combustivel, "GNL")
                    && !String.Equals(conft.Existente, "NC", StringComparison.OrdinalIgnoreCase))
                {
                    double cvu;
                    if (cts.Any(x => x.Cod == term.Cod))
                    {
                        cvu = cts.First(x => x.Cod == term.Cod).Cvu;

                        var CVU_planilha = w.adtermdat ?? new List<IADTERM>();

                        var Busca_CVU = CVU_planilha.Where(x => x.Usina == conft.Num).ToList();

                        if (Busca_CVU.Count() != 0)
                        {
                            foreach (var itens in Busca_CVU)
                            {
                                if (itens.Mes == mesOperativo.Mes)
                                {
                                    cvu = itens.RestricaoP1;
                                }
                            }
                        }
                    }
                    else
                    {
                        cvu = clast.Cvu1;
                    }




                    var lstManutt = manutDat.Where(x => x.DataInicio <= mesOperativo.SemanasOperativas.Last().Fim && x.DataFim >= mesOperativo.Inicio)
                        .Where(x => x.Cod == term.Cod).ToList();

                    //Mudando a partir da daqui




                    if (lstManutt.Count != 0)
                    {
                        List<CtLine> lstFoo = trataManutt(lstManutt, mesOperativo, gtmin, gtminMais, potef, potefMais, fcmax, fcmaxMais, cvu, conft);

                        lstCT.AddRange(lstFoo);
                    }
                    else
                    {

                        double disp = ((potef * fcmax) / 100d);
                        CtLine ct = new CtLine();
                        ct[1] = conft.Num;
                        ct[2] = conft.Sistema;
                        ct[3] = conft.Nome;
                        ct[4] = 1;

                        //ct[5] = ct[8] = ct[11] = gtmin;
                        ct[6] = ct[9] = ct[12] = disp;
                        ct[7] = ct[10] = ct[13] = cvu;

                        ct[5] = ct[8] = ct[11] = Math.Min(gtmin, ct[6]);


                        lstCT.Add(ct);

                        if (gtmin != gtminMais || (potef * fcmax) != (potefMais * fcmaxMais))
                        {
                            CtLine ctMais = ct.Clone() as CtLine;
                            ct[4] = mesOperativo.Estagios + 1;
                            ct[5] = ct[8] = ct[11] = gtminMais;
                            ct[6] = ct[9] = ct[12] = potefMais * fcmaxMais / 100d;

                            ct[5] = ct[8] = ct[11] = Math.Min(gtminMais, ct[6]);


                            lstCT.Add(ctMais);
                        }
                    }
                }
            }

            lstCT.AddRange(
            cts.Where(x =>
                !lstCT.Any(y => y.Cod == x.Cod)
                ).AsEnumerable()
                );

            foreach (var ct in lstCT.OrderBy(x => x.Subsistema).ThenBy(x => x[7]).ThenBy(x => x[1]).ThenBy(x => x[4]))
            {

                dadger.BlocoCT.Add(ct);
            }
            //Retirar usinas com disponibilidade zerada em todos os patamares de todos seus estagios

            var ctCods = dadger.BlocoCT.Select(x => x.Cod).Distinct().ToList();
            List<string> usinasremovidas = new List<string>();
            List<int> usinasRemover = new List<int>();

            foreach (var cods in ctCods)
            {
                var ctUsinas = dadger.BlocoCT.Where(x => x.Cod == cods).ToList();
                if (ctUsinas.All(x => x.Disp1 == 0 && x.Disp2 == 0 && x.Disp3 == 0))
                {
                    usinasRemover.Add(cods);
                    usinasremovidas.Add(cods.ToString());
                }
            }
            //var pred = rhe.Value.Where(x => x is FuLine).All(x => ((FuLine)x).Usina == 66);
            //
            var fts = dadger.BlocoRhe.Where(x => x is FtLine).Select(y => ((FtLine)y).Usina).Distinct().ToList();
            foreach (var ur in usinasRemover)
            {
                if (fts.All(x => x != ur))
                {
                    dadger.BlocoCT.Where(x => x.Cod == ur).ToList().ForEach(y => dadger.BlocoCT.Remove(y));
                }
            }
            // fim

            #endregion

            #region ci/ce

            #endregion

            #region VR
            //horario de verão

            ((DummyBlock)dadger.Blocos["VR"]).Clear();
            //if (mesOperativo.EstagioInicioHorarioVerao.HasValue)
            //{
            //    var vrl = new DummyLine();
            //    if (mesOperativo.EstagioInicioHorarioVerao.Value > mesOperativo.Estagios)
            //    {
            //        vrl[0] = "VR";
            //        vrl[1] = "  " + mesOperativo.SemanasOperativas.Last().Fim.Month.ToString("00") + "        INI";
            //    }
            //    else
            //    {
            //        vrl[0] = "VR";
            //        vrl[1] = "  " + mesOperativo.Mes.ToString("00") + "   " + (isMensal ? " " : mesOperativo.EstagioInicioHorarioVerao.Value.ToString("0")) + "    INI";
            //    }
            //    dadger.Blocos["VR"].Add(vrl);
            //}
            //else if (mesOperativo.EstagioFimHorarioVerao.HasValue)
            //{
            //    var vrl = new DummyLine();
            //    if (mesOperativo.EstagioFimHorarioVerao.Value > mesOperativo.Estagios)
            //    {
            //        vrl[0] = "VR";
            //        vrl[1] = "  " + mesOperativo.SemanasOperativas.Last().Fim.Month.ToString("00") + "        FIM";
            //    }
            //    else
            //    {
            //        vrl[0] = "VR";
            //        vrl[1] = "  " + mesOperativo.Mes.ToString("00") + "   " + (isMensal ? " " : mesOperativo.EstagioFimHorarioVerao.Value.ToString("0")) + "    FIM";
            //    }
            //    dadger.Blocos["VR"].Add(vrl);
            //}

            #endregion

            #region DP / IT ou RI / PQ

            trataCarga(mesOperativo, dadger, deckNWEstudo, pmoBase, pees, eolicasDados);

            #endregion

            #region deficit

            dadger.BlocoCd.Clear();

            foreach (var def in pmoBase.Blocos["DEFICIT"])
            {

                var cTemp = new CdLine();
                cTemp[1] = 1;
                cTemp[2] = pmoBase.Rees.GetMercado(def[0]);
                cTemp[3] = "INTERV 1";
                cTemp[4] = 1;
                cTemp[5] = cTemp[7] = cTemp[9] = def[5] * 100;
                cTemp[6] = cTemp[8] = cTemp[10] = def[1];

                dadger.BlocoCd.Add(cTemp.Clone());

                if (def[6] != 0)
                {
                    cTemp[1] = 2;
                    cTemp[3] = "INTERV 2";
                    cTemp[5] = cTemp[7] = cTemp[9] = def[6] * 100;
                    cTemp[6] = cTemp[8] = cTemp[10] = def[2];
                    dadger.BlocoCd.Add(cTemp.Clone());
                }


                if (def[7] != 0)
                {
                    cTemp[1] = 3;
                    cTemp[3] = "INTERV 3";
                    cTemp[5] = cTemp[7] = cTemp[9] = def[7] * 100;
                    cTemp[6] = cTemp[8] = cTemp[10] = def[3];
                    dadger.BlocoCd.Add(cTemp.Clone());
                }

                if (def[8] != 0)
                {
                    cTemp[1] = 4;
                    cTemp[3] = "INTERV 4";
                    cTemp[5] = cTemp[7] = cTemp[9] = def[8] * 100;
                    cTemp[6] = cTemp[8] = cTemp[10] = def[4];
                    dadger.BlocoCd.Add(cTemp.Clone());
                }
            }
            #endregion

            #region IA
            {

                dadger.BlocoIa.Where(x => x.Estagio != 1).ToList().ForEach(x => dadger.BlocoIa.Remove(x));


                IaLine ia = null;
                IaLine ia2 = null;
                //N - FC
                ia = dadger.BlocoIa.First(x => x.SistemaA == "N" && x.SistemaB == "FC");
                var intLines = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 4 && x.SubmercadoB == 11);
                var val1 = intLines.First()[mesOperativo.Mes];
                var val2 = intLines.Last()[mesOperativo.MesSeguinte];

                ia[5] = ia[7] = ia[9] = val1;
                if (val2 != val1)
                {
                    ia2 = (IaLine)ia.Clone();
                    ia2[1] = mesOperativo.Estagios + 1;
                    ia2[5] = ia2[7] = ia2[9] = val2;
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                //NE - FC
                ia = dadger.BlocoIa.First(x => x.SistemaA == "NE" && x.SistemaB == "FC");
                intLines = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 3 && x.SubmercadoB == 11);
                val1 = intLines.First()[mesOperativo.Mes];
                val2 = intLines.Last()[mesOperativo.MesSeguinte];

                ia[5] = ia[7] = ia[9] = val1;
                if (ia2 == null) { ia2 = (IaLine)ia.Clone(); dadger.BlocoIa.InsertAfter(ia, ia2); }
                ia2[1] = mesOperativo.Estagios + 1;
                ia2[5] = ia2[7] = ia2[9] = val2;


                //FC - NE /// Patamar
                intLines = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 11 && x.SubmercadoB == 3);
                var patsFCNE = patamarDat.Intercambio.Where(x => x.Ano == mesOperativo.Ano && x.SubmercadoA == 11 && x.SubmercadoB == 3).ToList();

                val1 = intLines.First()[mesOperativo.Mes];
                val2 = intLines.Last()[mesOperativo.MesSeguinte];

                ia[6] = val1 * patsFCNE[0][mesOperativo.Mes];
                ia[8] = val1 * patsFCNE[1][mesOperativo.Mes];
                ia[10] = val1 * patsFCNE[2][mesOperativo.Mes];

                patsFCNE = patamarDat.Intercambio.Where(x => (x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 11 && x.SubmercadoB == 3).ToList();
                ia2[1] = mesOperativo.Estagios + 1;
                ia2[6] = val2 * patsFCNE[0][mesOperativo.MesSeguinte];
                ia2[8] = val2 * patsFCNE[1][mesOperativo.MesSeguinte];
                ia2[10] = val2 * patsFCNE[2][mesOperativo.MesSeguinte];
                ia2 = null;

                //SE - FC
                ia = dadger.BlocoIa.First(x => x.SistemaA == "SE" && x.SistemaB == "FC");
                intLines = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 1 && x.SubmercadoB == 11);
                val1 = intLines.First()[mesOperativo.Mes];
                val2 = intLines.Last()[mesOperativo.MesSeguinte];

                ia[5] = ia[7] = ia[9] = val1;
                if (val2 != val1)
                {
                    ia2 = (IaLine)ia.Clone();
                    ia2[1] = mesOperativo.Estagios + 1;
                    ia2[5] = ia2[7] = ia2[9] = val2;
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }


                //SE - IV=== Utiliza intercambio 1-2
                ia = dadger.BlocoIa.First(x => x.SistemaA == "SE" && x.SistemaB == "IV");


                if (dadger.BlocoIt.Count() > 0)
                {


                    ia[6] = -dadger.BlocoIt.First().Geracao_Pat1 + 7000 + 8000;
                    ia[8] = -dadger.BlocoIt.First()[6] + 7000 + 8000;
                    ia[10] = -dadger.BlocoIt.First()[8] + 7000 + 8000;

                    ia2 = (IaLine)ia.Clone();

                    ia2[1] = mesOperativo.Estagios + 1;
                    ia2[6] = -dadger.BlocoIt.Last()[4] + 7000 + 8000;
                    ia2[8] = -dadger.BlocoIt.Last()[6] + 7000 + 8000;
                    ia2[10] = -dadger.BlocoIt.Last()[8] + 7000 + 8000;

                    dadger.BlocoIa.InsertAfter(ia, ia2);


                    ia2 = null;
                }
                else if (dadger.BlocoRi.Count() > 0)
                {
                    intLines = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 1 && x.SubmercadoB == 2);
                    var patsSEIV = patamarDat.Intercambio.Where(x => x.Ano == mesOperativo.Ano && x.SubmercadoA == 1 && x.SubmercadoB == 2).ToList();


                    val1 = intLines.First()[mesOperativo.Mes];
                    val2 = intLines.Last()[mesOperativo.MesSeguinte];

                    ia[5] = val1 * patsSEIV[0][mesOperativo.Mes] - dadger.BlocoRi.Select(x => x.Ger_Min60_Pat1).First();
                    ia[7] = val1 * patsSEIV[1][mesOperativo.Mes] - dadger.BlocoRi.Select(x => x.Ger_Min60_Pat2).First();
                    ia[9] = val1 * patsSEIV[2][mesOperativo.Mes] - dadger.BlocoRi.Select(x => x.Ger_Min60_Pat3).First();


                    var resPlan = w.Rhes.Where(y => y.Sistemas.Count() > 0 && y.Sistemas[0].Item1 == "IV" && y.Sistemas[0].Item2 == "SE").ToList();
                    if (resPlan.Count() != 0)
                    {
                        foreach (var linhaIVSE in resPlan.Where(x => x.Mes == mesOperativo.Mes && (x.Estagio == 1 || x.Estagio == null)))
                        {
                            ia[6] = linhaIVSE.LimSup1;
                            ia[8] = linhaIVSE.LimSup2;
                            ia[10] = linhaIVSE.LimSup3;
                        }



                    }
                    //foreach (var rhe in w.Rhes.Where(x => x.Mes == _m && ((x.Estagio ?? _e) == _e)))
                    //{
                    //    mesOperativo.
                    //}
                    //ia[6] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat1 + x.AndePat1).First() + 7000 + 8000;
                    //ia[8] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat2 + x.AndePat2).First() + 7000 + 8000;
                    //ia[10] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat3 + x.AndePat3).First() + 7000 + 8000;

                    ia2 = (IaLine)ia.Clone();
                    patsSEIV = patamarDat.Intercambio.Where(x => (x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 1 && x.SubmercadoB == 2).ToList();

                    ia2[5] = val2 * patsSEIV[0][mesOperativo.MesSeguinte] - dadger.BlocoRi.Select(x => x.Ger_Min60_Pat1).Last();
                    ia2[7] = val2 * patsSEIV[1][mesOperativo.MesSeguinte] - dadger.BlocoRi.Select(x => x.Ger_Min60_Pat2).Last();
                    ia2[9] = val2 * patsSEIV[2][mesOperativo.MesSeguinte] - dadger.BlocoRi.Select(x => x.Ger_Min60_Pat3).Last();

                    ia2[1] = mesOperativo.Estagios + 1;

                    if (resPlan.Count() != 0)
                    {
                        if (mesOperativo.MesSeguinte == 1)
                        {
                            foreach (var linhaIVSE in resPlan.Where(x => x.Mes == 12 && (x.Estagio == 1 || x.Estagio == null)))
                            {
                                ia2[6] = linhaIVSE.LimSup1;
                                ia2[8] = linhaIVSE.LimSup2;
                                ia2[10] = linhaIVSE.LimSup3;
                            }
                        }
                        else
                        {
                            foreach (var linhaIVSE in resPlan.Where(x => x.Mes == mesOperativo.Mes && x.Estagio == 2))
                            {
                                ia2[6] = linhaIVSE.LimSup1;
                                ia2[8] = linhaIVSE.LimSup2;
                                ia2[10] = linhaIVSE.LimSup3;
                            }
                        }




                    }
                    //ia2[6] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat1 + x.AndePat1).Last() + 7000 + 8000;
                    //ia2[8] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat2 + x.AndePat2).Last() + 7000 + 8000;
                    //ia2[10] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat3 + x.AndePat3).Last() + 7000 + 8000;

                    dadger.BlocoIa.InsertAfter(ia, ia2);


                    ia2 = null;
                }

                //SE - NE       
                ia = dadger.BlocoIa.First(x => x.SistemaA == "SE" && x.SistemaB == "NE");
                var intLinesAB = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 1 && x.SubmercadoB == 3);
                var intLinesBA = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 3 && x.SubmercadoB == 1);
                var valAB1 = intLinesAB.First()[mesOperativo.Mes];
                var valAB2 = intLinesAB.Last()[mesOperativo.MesSeguinte];
                var valBA1 = intLinesBA.First()[mesOperativo.Mes];
                var valBA2 = intLinesBA.Last()[mesOperativo.MesSeguinte];

                ia[5] = ia[7] = ia[9] = valAB1;
                ia[6] = ia[8] = ia[10] = valBA1;
                if (valAB2 != valAB1 || valBA2 != valBA1)
                {
                    ia2 = (IaLine)ia.Clone();
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2[1] = mesOperativo.Estagios + 1;
                    ia2[5] = ia2[7] = ia2[9] = valAB2;
                    ia2[6] = ia2[8] = ia2[10] = valBA2;
                }


                ia2 = null;


                //IV-S ===utiliza intercambio 1-2
                ia = dadger.BlocoIa.First(x => x.SistemaA == "IV" && x.SistemaB == "S");

                intLines = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 1 && x.SubmercadoB == 2);
                var patsIVS = patamarDat.Intercambio.Where(x => x.Ano == mesOperativo.Ano && x.SubmercadoA == 1 && x.SubmercadoB == 2).ToList();

                val1 = intLines.First()[mesOperativo.Mes];
                val2 = intLines.Last()[mesOperativo.MesSeguinte];

                ia[5] = val1 * patsIVS[0][mesOperativo.Mes];
                ia[7] = val1 * patsIVS[1][mesOperativo.Mes];
                ia[9] = val1 * patsIVS[2][mesOperativo.Mes];

                //ver se é melhor esperar para travar os valores de S--IV

                //ia[6] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat1 + x.AndePat1).First() + 7000 + 8000;
                //ia[8] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat2 + x.AndePat2).First() + 7000 + 8000;
                //ia[10] = -dadger.BlocoRi.Select(x => x.Ger_Min50_Pat3 + x.AndePat3).First() + 7000 + 8000;

                ia2 = (IaLine)ia.Clone();
                patsIVS = patamarDat.Intercambio.Where(x => (x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 1 && x.SubmercadoB == 2).ToList();

                ia2[5] = val2 * patsIVS[0][mesOperativo.MesSeguinte];
                ia2[7] = val2 * patsIVS[1][mesOperativo.MesSeguinte];
                ia2[9] = val2 * patsIVS[2][mesOperativo.MesSeguinte];

                ia2[1] = mesOperativo.Estagios + 1;
                ia2[6] = 6500;
                ia2[8] = 6500;
                ia2[10] = 6800;

                dadger.BlocoIa.InsertAfter(ia, ia2);

                ia2 = null;



                //////
                //N - SE  


                ia = dadger.BlocoIa.First(x => x.SistemaA == "N" && x.SistemaB == "SE");
                intLinesAB = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 4 && x.SubmercadoB == 1);
                intLinesBA = sistemaDat.Intercambio.Where(x => (x.Ano == mesOperativo.Ano || x.Ano == mesOperativo.AnoSeguinte) && x.SubmercadoA == 1 && x.SubmercadoB == 4);

                var patsN_SE = patamarDat.Intercambio.Where(x => x.Ano == mesOperativo.Ano && x.SubmercadoA == 4 && x.SubmercadoB == 1).ToList();
                var patsSE_N = patamarDat.Intercambio.Where(x => x.Ano == mesOperativo.Ano && x.SubmercadoA == 1 && x.SubmercadoB == 4).ToList();


                valAB1 = intLinesAB.First()[mesOperativo.Mes];// valor correspondente ao mes no sistema.dat(4-1)N-SE
                valAB2 = intLinesAB.Last()[mesOperativo.MesSeguinte];

                valBA1 = intLinesBA.First()[mesOperativo.Mes];// valor correspondente ao mes no sistema.dat(1-4)SE-N
                valBA2 = intLinesBA.Last()[mesOperativo.MesSeguinte];

                var Plan_Rhe = w.Rhes.Where(x => x.Sistemas.Any(y => y.Item1 == "N" && y.Item2 == "SE")).ToList();

                if (Plan_Rhe.Count() != 0)
                {
                    var dados = Plan_Rhe.Where(x => x.Mes == mesOperativo.Mes && (x.Estagio == 1 || x.Estagio == null));

                    try
                    {
                        ia[5] = Convert.ToDouble(dados.Select(x => x.LimSup1).ToList().FirstOrDefault());
                        ia[7] = Convert.ToDouble(dados.Select(x => x.LimSup2).ToList().FirstOrDefault());  // ia[5] = valAB;
                        ia[9] = Convert.ToDouble(dados.Select(x => x.LimSup3).ToList().FirstOrDefault());
                    }
                    catch
                    {

                    }

                }
                else
                {
                    ia[5] = valAB1 * patsN_SE[0][mesOperativo.Mes];
                    ia[7] = valAB1 * patsN_SE[1][mesOperativo.Mes];  // ia[5] = valAB;
                    ia[9] = valAB1 * patsN_SE[2][mesOperativo.Mes];
                }


                ia[6] = valBA1 * patsSE_N[0][mesOperativo.Mes];
                ia[8] = valBA1 * patsSE_N[1][mesOperativo.Mes];
                ia[10] = valBA1 * patsSE_N[2][mesOperativo.Mes];

                //if (valAB2 != valAB1 || valBA2 != valBA1)
                //{
                ia2 = (IaLine)ia.Clone();
                dadger.BlocoIa.InsertAfter(ia, ia2);
                patsN_SE = patamarDat.Intercambio.Where(x => x.Ano == mesOperativo.AnoSeguinte && x.SubmercadoA == 4 && x.SubmercadoB == 1).ToList();
                patsSE_N = patamarDat.Intercambio.Where(x => x.Ano == mesOperativo.AnoSeguinte && x.SubmercadoA == 1 && x.SubmercadoB == 4).ToList();


                if (Plan_Rhe.Count() != 0)
                {
                    var dados = Plan_Rhe.Where(x => x.Mes == mesOperativo.Mes && (x.Estagio == 2 || x.Estagio == null));
                    ia2[1] = mesOperativo.Estagios + 1;
                    ia2[5] = Convert.ToDouble(dados.Select(x => x.LimSup1).ToList().FirstOrDefault());
                    ia2[7] = Convert.ToDouble(dados.Select(x => x.LimSup2).ToList().FirstOrDefault());
                    ia2[9] = Convert.ToDouble(dados.Select(x => x.LimSup3).ToList().FirstOrDefault());
                }
                else
                {
                    ia2[1] = mesOperativo.Estagios + 1;
                    ia2[5] = valAB2 * patsN_SE[0][mesOperativo.MesSeguinte];
                    ia2[7] = valAB2 * patsN_SE[1][mesOperativo.MesSeguinte];
                    ia2[9] = valAB2 * patsN_SE[2][mesOperativo.MesSeguinte];
                }
                ia[6] = valBA2 * patsSE_N[0][mesOperativo.MesSeguinte];
                ia[8] = valBA2 * patsSE_N[1][mesOperativo.MesSeguinte];
                ia[10] = valBA2 * patsSE_N[2][mesOperativo.MesSeguinte];

                //ia2[1] = mesOperativo.Estagios + 1;
                //ia2[5] = ia2[7] = ia2[9] = valAB2;
                //ia2[6] = ia2[8] = ia2[10] = valBA2;
                //}

                ia2 = null;

            }
            #endregion

            #region MP / MT / FD            
            {

                var mpBase = dadger.BlocoMp.ToList();
                dadger.BlocoMp.Clear();

                foreach (var uh in dadger.BlocoUh)
                {
                    var mpTemp = new MpLine();
                    mpTemp[1] = uh[1];
                    var mpUhs = mpBase.Where(x => x[1] == uh[1]);




                    if (mpUhs.Count() == 0)
                    {
                        for (var e = 0; e < mesOperativo.Estagios; e++)
                        {
                            mpTemp[3 + e] = 1;
                        }

                        mpTemp[3 + mesOperativo.Estagios] = 1;
                        dadger.BlocoMp.Add(mpTemp);
                    }
                    else
                    {
                        foreach (var mpUh in mpUhs)
                        {
                            int count = 0;
                            //int mpCount = mesOperativo.Estagios > 0 ? mesOperativo.Estagios : 1;
                            double mp = 0;
                            //for (; count < mesOperativo.Estagios; count++)
                            if (mpUh[2] == 50)
                            {
                                var mpTemp2 = new MpLine();
                                mpTemp2[1] = uh[1];
                                int count2 = 0;
                                double mp2 = 0;
                                while (mpUh[3 + count2] is double)
                                {
                                    mp2 += mpUh[3 + count2];
                                    count2++;
                                }
                                for (var e = 0; e < mesOperativo.Estagios; e++)
                                {
                                    mpTemp2[3 + e] = mp2 / count2;
                                }

                                mpTemp2[3 + mesOperativo.Estagios] = 1;
                                mpTemp2[2] = mpUh[2];
                                dadger.BlocoMp.Add(mpTemp2);

                            }
                            else
                            {
                                while (mpUh[3 + count] is double)
                                {
                                    mp += mpUh[3 + count];
                                    count++;
                                }
                                for (var e = 0; e < mesOperativo.Estagios; e++)
                                {
                                    mpTemp[3 + e] = mp / count;
                                }

                                mpTemp[3 + mesOperativo.Estagios] = 1;
                                mpTemp[2] = mpUh[2];
                                dadger.BlocoMp.Add(mpTemp);
                            }

                        }
                    }

                    //mpTemp[3 + mesOperativo.Estagios] = 1;
                    //dadger.BlocoMp.Add(mpTemp);
                }


                dadger.BlocoMt.Clear();
                foreach (var ct in dadger.BlocoCT.Select(x => new { cod = x[1], merc = x[2] }).Distinct())
                {
                    var mtTemp = new MtLine();
                    mtTemp[1] = ct.cod;
                    mtTemp[2] = ct.merc;

                    for (int e = 0; e <= mesOperativo.Estagios; e++)
                    {
                        mtTemp[3 + e] = 1;
                    }

                    dadger.BlocoMt.Add(mtTemp);
                }


                foreach (var fd in dadger.Blocos["FD"])
                    for (int e = 0; e < 10; e++)
                        if (e < mesOperativo.Estagios) fd[3 + e] = 1;
                        else if (e == mesOperativo.Estagios) fd[3 + e] = 0.95;
                        else fd[3 + e] = "";

                foreach (var rq in dadger.Blocos["RQ"])
                    for (int e = 0; e < 10; e++)
                        if (e < mesOperativo.Estagios) rq[2 + e] = 100;
                        else if (e == mesOperativo.Estagios) rq[2 + e] = 0;
                        else rq[2 + e] = "";


                foreach (var ti in dadger.Blocos["TI"])
                {
                    var tiline = ti as TiLine;
                    var usi = tiline.Usina;

                    var val = ti[3];
                    var val2 = ti.Valores.Last(c => c is double);

                    for (int e = 0; e < 10; e++)
                        if (e < mesOperativo.Estagios) ti[2 + e] = val;
                        else if (e == mesOperativo.Estagios) ti[2 + e] = val2;
                        else ti[2 + e] = "";
                    for (int est = 0; est < 10; est++)
                    {
                        if (est < mesOperativo.Estagios)
                        {
                            var ano = mesOperativo.Ano;
                            var mes = mesOperativo.Mes;
                            var taxaline = w.Taxairris.Where(x => x.Ano == ano && x.Usina == usi).FirstOrDefault();
                            if (taxaline != null)
                            {
                                double taxa = taxaline.TaxaMes[mes - 1];
                                ti[2 + est] = taxa;
                            }
                        }
                        else if (est == mesOperativo.Estagios)
                        {
                            var ano = mesOperativo.AnoSeguinte;
                            var mes = mesOperativo.MesSeguinte;
                            var taxaline = w.Taxairris.Where(x => x.Ano == ano && x.Usina == usi).FirstOrDefault();
                            if (taxaline != null)
                            {
                                double taxa = taxaline.TaxaMes[mes - 1];
                                ti[2 + est] = taxa;
                            }
                        }
                        else ti[2 + est] = "";
                    }
                }
            }

            #endregion

            #region VE

            var vmaxt = from m in modifNW
                        where m.Chave == "VMAXT"
                        let dataAlteracao = new DateTime(int.Parse(m.NovosValores[1]), int.Parse(m.NovosValores[0]), 1)
                        orderby dataAlteracao
                        group new { data = dataAlteracao, valor = float.Parse(m.NovosValores[2], System.Globalization.NumberFormatInfo.InvariantInfo), usina = m.Usina } by m.Usina;

            dadger.BlocoVe.Clear();

            var uhs = dadger.BlocoUh.Select(x => x.Usina).ToArray();
            var Plan_VE = w.Bloco_VE;

            foreach (var ve in vmaxt.Where(x => uhs.Contains(x.Key)))
            {
                var veatual = ve.LastOrDefault(x => x.data <= dtEstudo);
                var veseguinte = ve.LastOrDefault(x => x.data <= dtEstudo.AddMonths(1));
                var l = dadger.BlocoVe.CreateLine();
                l[1] = ve.Key;

                var dados_excel = Plan_VE.Where(x => x.Key == ve.Key).Select(x => x.Value);
                double dado_Seguinte = 0;
                for (int e = 0; e < mesOperativo.Estagios; e++)
                {
                    double dado = 0;

                    foreach (var d in dados_excel)
                    {
                        dado = d[mesOperativo.Mes - 1];
                        dado_Seguinte = d[mesOperativo.MesSeguinte - 1];
                    }
                    if (dado == 0)
                    {
                        l[2 + e] = veatual != null ? veatual.valor : 100;
                    }
                    else
                    {
                        l[2 + e] = dado * 100;
                    }
                }

                if (dado_Seguinte == 0)
                {
                    l[2 + mesOperativo.Estagios] = veseguinte != null ? veseguinte.valor : 100;
                }
                else
                {
                    l[2 + mesOperativo.Estagios] = dado_Seguinte * 100;
                }
                dadger.BlocoVe.Add(l);
            }
            #endregion

            #region RHE

            trataRHEs(deckNWEstudo, dadger, mesOperativo, patamarDat, sistemaDat);

            #endregion

            #region Alteracoes Cadastrais (AC)
            var testeaccotovol = dadger.BlocoAc.Where(x => x.Usina == 288 || x.Usina == 314).ToList();

            List<string> minemonicosAlvo = new List<string> { "COTVOL", "VOLMIN", "VOLMAX", "VSVERT", "VMDESV" };
            var bm_pLines = dadger.BlocoAc.Where(x => (x.Usina == 288 || x.Usina == 314) && minemonicosAlvo.Any(y => y.Contains(x.Mnemonico))).ToList();
            bool temAcCOtvolBM_P = false;
            if (bm_pLines.Count() > 0)
            {
                foreach (var item in bm_pLines) dadger.BlocoAc.Remove(item);
                temAcCOtvolBM_P = true;
            }
            testeaccotovol = dadger.BlocoAc.Where(x => x.Usina == 288 || x.Usina == 314).ToList();


            foreach (var ac in dadger.BlocoAc.Where(x => !string.IsNullOrWhiteSpace(x.Mes)
                || x.Mnemonico == "JUSMED" || x.Mnemonico == "NUMCON" || x.Mnemonico == "NUMMAQ"
                ).ToArray())
                dadger.BlocoAc.Remove(ac);
            var exph = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.exph].Document as Compass.CommomLibrary.ExphDat.ExphDat;

            //canal de fuga
            var cfugas = from m in modifNW
                         where m.Chave == "CFUGA"
                         let dataAlteracao = new DateTime(int.Parse(m.NovosValores[1]), int.Parse(m.NovosValores[0]), 1)
                         orderby dataAlteracao
                         group new { data = dataAlteracao, valor = float.Parse(m.NovosValores[2], System.Globalization.NumberFormatInfo.InvariantInfo), usina = m.Usina } by m.Usina;

            foreach (var cfugasUsina in cfugas)
            {

                //atual
                var cfugaAtual = cfugasUsina.LastOrDefault(x => x.data <= dtEstudo);
                var cfugaSeguinte = cfugasUsina.FirstOrDefault(x => x.data == dtEstudo.AddMonths(1));


                if (cfugaAtual != null && cfugaAtual.usina != 285 && cfugaAtual.usina != 287)
                {
                    var acL = new AcF10Line();
                    acL.Usina = cfugaAtual.usina;
                    acL.Mnemonico = "JUSMED";
                    acL.P1 = cfugaAtual.valor;
                    acL.Mes = isMensal ? null : dtEstudo.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    acL.Semana = isMensal ? (int?)null : 1;
                    dadger.BlocoAc.Add(acL);
                }

                if (cfugaSeguinte != null && cfugaSeguinte.usina != 285 && cfugaSeguinte.usina != 287)
                {
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



            foreach (var uhe in usinasComExpansao)
            {
                if (uhe != 176)
                {


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

                    if (!numMaqs1.Any(x => x != 0))
                    {

                        var aclCon = new AcI5Line();
                        aclCon.Usina = uhe;
                        aclCon.Mnemonico = "NUMCON";
                        aclCon.P1 = 0;
                        aclCon.Mes = isMensal ? null : dtEstudo.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                        aclCon.Semana = isMensal ? (int?)null : 1;
                        dadger.BlocoAc.Add(aclCon);


                    }
                    else
                    {

                        var aclCon = new AcI5Line();
                        aclCon.Usina = uhe;
                        aclCon.Mnemonico = "NUMCON";
                        aclCon.P1 = numMaqs1.Count(x => x != 0);
                        aclCon.Mes = isMensal ? null : dtEstudo.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                        aclCon.Semana = isMensal ? (int?)null : 1;
                        dadger.BlocoAc.Add(aclCon);


                        for (int c = 0; c < numMaqs1.Count(x => x != 0); c++)
                        {
                            var aclMaq = new Ac2I5Line();
                            aclMaq.Usina = uhe;
                            aclMaq.Mnemonico = "NUMMAQ";
                            aclMaq.P1 = c + 1;
                            aclMaq.P2 = numMaqs1[c];
                            aclMaq.Mes = isMensal ? null : dtEstudo.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                            aclMaq.Semana = isMensal ? (int?)null : 1;

                            dadger.BlocoAc.Add(aclMaq);
                        }
                    }

                    if (numMaqs2.Any(x => x != 0))
                    {

                        var aclCon = new AcI5Line();
                        aclCon.Usina = uhe;
                        aclCon.Mnemonico = "NUMCON";
                        aclCon.P1 = numMaqs2.Count(x => x != 0);
                        aclCon.Mes = dtEstudo.AddMonths(1).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                        aclCon.Ano = dtEstudo.AddMonths(1).Year;

                        dadger.BlocoAc.Add(aclCon);

                        for (int c = 0; c < numMaqs2.Count(x => x != 0); c++)
                        {
                            var aclMaq = new Ac2I5Line();
                            aclMaq.Usina = uhe;
                            aclMaq.Mnemonico = "NUMMAQ";
                            aclMaq.P1 = c + 1;
                            aclMaq.P2 = numMaqs2[c];
                            aclMaq.Mes = dtEstudo.AddMonths(1).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                            aclMaq.Ano = dtEstudo.AddMonths(1).Year;

                            dadger.BlocoAc.Add(aclMaq);
                        }
                    }

                    var confhd = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.confhd].Document as Compass.CommomLibrary.ConfhdDat.ConfhdDat;

                    foreach (var novasUhe in
                        dadger.BlocoAc.Select(x => x.Usina).Distinct().Except(
                            dadger.BlocoUh.Select(x => x.Usina)))
                    {

                        var nUh = new UhLine();
                        nUh.Usina = novasUhe;
                        nUh.VolIniPerc = 0;
                        dadger.BlocoUh.Add(nUh);
                        nUh.Sistema = confhd.First(x => x.Cod == nUh.Usina).REE;

                        var nMp = new MpLine();
                        nMp[1] = novasUhe;

                        for (int e = 0; e <= mesOperativo.Estagios; e++)
                        {
                            nMp[3 + e] = 1;
                        }

                        dadger.BlocoMp.Add(nMp);
                    }

                }


            }

            #endregion

            #region Cortes

            Services.Deck.AlterarCortes(dadger, System.IO.Path.Combine(deckNWEstudo.BaseFolder, "cortes.dat"), nWh, dtEstudo);

            #endregion

            #region RHV
            //aqui é apagado as linhas lv deixando apenas uma e setando o valor correspondente ao numero do estagio =1
            {
                foreach (var rh in dadger.BlocoRhv.RhvGrouped)
                {
                    if (rh.Key[1] == 101)
                    {

                    }
                    rh.Key[2] = 1;
                    rh.Key[3] = mesOperativo.Estagios + 1;

                    rh.Value.Where(x => x is CvLine).ToList().ForEach(x => x[2] = 1);

                    var ls = rh.Value.Where(x => x is LvLine).OrderBy(x => x[2]);

                    foreach (var l in ls)
                    {
                        if (l == ls.Last()) l[2] = 1;
                        else dadger.BlocoRhv.Remove(l);
                    }
                }
            }
            #endregion

            #region Rhc
            var decompBaseCam = w.DecompBase;
            var versaoNewave = w.versao_Newave.Trim().Substring(0, 2);
            int newaveNumVersion;//Convert.ToInt32(versaoNewave);
            int newaveNumber = int.TryParse(versaoNewave, out newaveNumVersion) ? newaveNumVersion : 28;

            if (versaoNewave == "270405" || versaoNewave == "28" || versaoNewave == "270405aws" || versaoNewave == "28aws" || versaoNewave.StartsWith("28") || newaveNumber > 27)//versoes que tem o bloco RHC
            {
                var decompEntrada = DeckFactory.CreateDeck(decompBaseCam) as Compass.CommomLibrary.Decomp.Deck;
                var dadgerEntrada = decompEntrada[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger;
                var hesEntrada = dadgerEntrada.BlocoRhc.Where(x => x is HeLine).ToList();

                var hes = dadger.BlocoRhc.Where(x => x is HeLine).ToList();
                //var numRests = hes.Union(hes).Select(x => x.Restricao).ToList();
                var numRests = hes.Union(hes).Select(x => x.Restricao).Distinct().ToList();


                foreach (var num in numRests)
                {
                    foreach (var rhc in dadger.BlocoRhc.Where(x => x is HeLine && x.Restricao == num && x[4] > mesOperativo.Estagios && x[8] == 0).ToList())
                    {
                        dadger.BlocoRhc.Remove(rhc);//remove linhas HE com flag 0 e com estagio maior que os estagios do mes operativo
                    }
                    for (int est = 1; est <= mesOperativo.Estagios; est++)
                    {
                        //adiciona novas linhas HE com flag 0 caso o deck de entrada tenha um numero menor de estagios que o mes operativo
                        var heLast = dadger.BlocoRhc.Where(x => x is HeLine && x.Restricao == num && x[8] == 0).LastOrDefault();
                        if (heLast != null)
                        {
                            var heLinha = dadger.BlocoRhc.Where(x => x is HeLine && x.Restricao == num && x[4] == est && x[8] == 0).FirstOrDefault();
                            if (heLinha == null)
                            {
                                int index = dadger.BlocoRhc.IndexOf(heLast);

                                var heClone = heLast.Clone() as HeLine;
                                heClone[4] = est;
                                heClone.Comment = null;
                                heClone[3] = heLast[3];
                                dadger.BlocoRhc.Insert(index + 1, heClone);
                            }
                        }

                    }
                    foreach (var rhc in dadger.BlocoRhc.Where(x => x is HeLine && x.Restricao == num && x[8] == 1).ToList())
                    {
                        rhc[4] = mesOperativo.Estagios + 1;//altera o valor do estagio das linhas HE com flag 1 com o estagio do mes seguinte
                    }

                    var cms = dadger.BlocoRhc.Where(x => x is CmLine && x.Restricao == num).ToList();

                    foreach (var cm in cms)//remove todas as linhas CM deixando apenas uma por restrição
                    {
                        if (cm == cms.Last())
                        {
                            continue;
                        }
                        else
                        {
                            dadger.BlocoRhc.Remove(cm);
                        }
                    }
                }

                //manipulacao com dados da planilha
                if (w.Herhc.Count() > 0)
                {
                    var hePlanLinhes = w.Herhc.Where(x => x.MesEst == mesOperativo.Mes).ToList();

                    foreach (var hep in hePlanLinhes)
                    {
                        var hesList = dadger.BlocoRhc.Where(x => x is HeLine && x.Restricao == hep.Rest).ToList();
                        if (hesList.Count > 0)
                        {
                            if (hesList.Count() == 1)//ou o deck é mensal ou a restricão é de mes seguinte 
                            {
                                hesList[0][3] = Math.Round(hep.valAtual * 100, 1);
                            }
                            else
                            {
                                int estagiosCont = hesList.Count();

                                if (hep.valAnt != null)
                                {
                                    double inicio = Math.Round((hep.valAnt ?? 0) * 100, 1);
                                    double dif = hep.valAtual - (hep.valAnt ?? 0);
                                    double inc = Math.Round((dif / (estagiosCont - 1)) * 100, 1);
                                    for (int i = 0; i < estagiosCont; i++)
                                    {

                                        if (i == estagiosCont - 1)
                                        {
                                            hesList[i][3] = Math.Round(hep.valAtual * 100, 1);
                                        }
                                        else
                                        {
                                            hesList[i][3] = inicio;
                                        }
                                        inicio += inc;

                                    }


                                }
                                else
                                {
                                    for (int i = 0; i < estagiosCont; i++)
                                    {

                                        hesList[i][3] = Math.Round(hep.valAtual * 100, 1);

                                    }
                                }

                            }
                        }
                    }


                }


                //foreach (var rhc in dadger.BlocoRhc.Where(x => x is HeLine && x[4] > 1).ToList())
                //{
                //    dadger.BlocoRhc.Remove(rhc);
                //}

                //foreach (var num in numRests)
                //{
                //    var cms = dadger.BlocoRhc.Where(x => x is CmLine && x[1] == num).ToList();
                //    foreach (var cm in cms)
                //    {
                //        if (cm == cms.First())
                //        {
                //            continue;
                //        }
                //        else
                //        {
                //            dadger.BlocoRhc.Remove(cm);
                //        }
                //    }

                //}

                //foreach (var rhc in dadger.BlocoRhc.RhcGrouped.ToList())
                //{
                //    for (int est = 2; est <= mesOperativo.Estagios + 1; est++)
                //    {
                //        if (est == mesOperativo.Estagios + 1)
                //        {
                //            var heCopy = hesEntrada.Where(x => x.Restricao == rhc.Value[0].Restricao).Last();
                //            int index = dadger.BlocoRhc.IndexOf(dadger.BlocoRhc.Where(x => x is HeLine && x.Restricao == rhc.Value[0].Restricao).Last());

                //            var heClone = rhc.Value[0].Clone() as HeLine;
                //            heClone[4] = est;
                //            heClone.Comment = null;
                //            heClone[3] = heCopy[3];
                //            dadger.BlocoRhc.Insert(index + 2, heClone);

                //            index = dadger.BlocoRhc.IndexOf(dadger.BlocoRhc.Where(x => x is CmLine && x.Restricao == rhc.Value[1].Restricao).Last());
                //            var cmClone = rhc.Value[1].Clone() as CmLine;
                //            dadger.BlocoRhc.Insert(index + 2, cmClone);
                //        }
                //        else
                //        {
                //            int index = dadger.BlocoRhc.IndexOf(dadger.BlocoRhc.Where(x => x is HeLine && x.Restricao == rhc.Value[0].Restricao).Last());
                //            var heClone = rhc.Value[0].Clone() as HeLine;
                //            heClone[4] = est;
                //            heClone.Comment = null;
                //            dadger.BlocoRhc.Insert(index + 2, heClone);

                //            index = dadger.BlocoRhc.IndexOf(dadger.BlocoRhc.Where(x => x is CmLine && x.Restricao == rhc.Value[1].Restricao).Last());
                //            var cmClone = rhc.Value[1].Clone() as CmLine;

                //            dadger.BlocoRhc.Insert(index + 2, cmClone);
                //        }
                //    }
                //}

            }


            #endregion


            #region RHQ
            {
                foreach (var rh in dadger.BlocoRhq.RhqGrouped)
                {

                    rh.Key[2] = 1;
                    rh.Key[3] = mesOperativo.Estagios + 1;

                    rh.Value.Where(x => x is CqLine).ToList().ForEach(x => x[2] = 1);

                    var ls = rh.Value.Where(x => x is LqLine).OrderBy(x => x[2]);

                    foreach (var l in ls)
                    {
                        if (l == ls.First()) l[2] = 1;
                        else if (l == ls.Last()) l[2] = mesOperativo.Estagios + 1;
                        else dadger.BlocoRhq.Remove(l);
                    }
                }


                foreach (var dt in new Tuple<DateTime, int>[] {
                  new Tuple<DateTime, int>(new DateTime(mesOperativo.Ano, mesOperativo.Mes, 1),1)
                , new Tuple<DateTime, int>(new DateTime(mesOperativo.AnoSeguinte, mesOperativo.MesSeguinte,1), mesOperativo.Estagios + 1) })

                {
                    foreach (var vazmint in modifNW.Where(x => x.Chave == "VAZMINT").Where(x => x.DataModif <= dt.Item1).OrderBy(x => x.DataModif).GroupBy(x => x.Usina))
                    {

                        var usina = vazmint.Key;

                        if (!dadger.BlocoUh.Any(x => x.Usina == usina)) continue;

                        var valor = vazmint.Last().ValorModif;

                        var rhvsToChange = dadger.BlocoRhq.RhqGrouped.Where(rh =>
                                                     rh.Value.Where(x => x is CqLine).All(x => ((CqLine)x).Usina == usina && ((CqLine)x).Tipo == "QDEF")
                                                    ).Select(x => x.Value).ToList();

                        if (rhvsToChange.Count() == 0)
                        {

                            //    HqLine hq = new HqLine()
                            //    {
                            //        Restricao = dadger.BlocoRhq.GetNextId(),
                            //        Inicio = 1,
                            //        Fim = mesOperativo.Estagios + 1
                            //    };

                            //    CqLine cq = new CqLine() { Restricao = hq.Restricao, Usina = usina, Tipo = "QDEF" };

                            //    dadger.BlocoRhq.Add(hq);
                            //    dadger.BlocoRhq.Add(cq);

                            //    LqLine lq;
                            //    lq = new LqLine()
                            //    {
                            //        Estagio = dt.Item2,
                            //        LimInfPat1 = valor,
                            //        LimInfPat2 = valor,
                            //        LimInfPat3 = valor,
                            //        Restricao = hq.Restricao
                            //    };
                            //    dadger.BlocoRhq.Add(lq);
                        }


                        foreach (var rhv in rhvsToChange)
                        {

                            LqLine lq;

                            lq = (LqLine)rhv.FirstOrDefault(x => x is LqLine && ((LqLine)x).Estagio == dt.Item2);

                            if (lq == null)
                            {
                                var copyFrom = (LqLine)rhv.Last(x => x is LqLine && ((LqLine)x).Estagio <= dt.Item2);

                                lq = new LqLine()
                                {
                                    Estagio = dt.Item2,
                                    Restricao = copyFrom.Restricao,
                                    LimSupPat1 = copyFrom.LimSupPat1,
                                    LimSupPat2 = copyFrom.LimSupPat2,
                                    LimSupPat3 = copyFrom.LimSupPat3,
                                };
                                dadger.BlocoRhq.Add(lq);
                            }

                            lq.LimInfPat1 = lq.LimInfPat2 = lq.LimInfPat3 = valor;

                            if (lq.LimSupPat1.HasValue) lq.LimSupPat1 = Math.Max(valor.Value, lq.LimSupPat1.Value);
                            if (lq.LimSupPat2.HasValue) lq.LimSupPat2 = Math.Max(valor.Value, lq.LimSupPat2.Value);
                            if (lq.LimSupPat3.HasValue) lq.LimSupPat3 = Math.Max(valor.Value, lq.LimSupPat3.Value);
                        }
                    }
                }
            }

            #endregion

            foreach (var rh in dadger.BlocoRha.RhaGrouped)
            {

                rh.Key[2] = 1;
                rh.Key[3] = 2;

                rh.Value.Where(x => x is CaLine).ToList().ForEach(x => x[2] = 1);

                var ls = rh.Value.Where(x => x is LaLine).OrderBy(x => x[2]);

                foreach (var l in ls)
                {
                    if (l == ls.First()) l[2] = 1;
                    else if (l == ls.Last()) l[2] = mesOperativo.Estagios + 1;
                    else dadger.BlocoRha.Remove(l);
                }
            }



            #region Sobrescrever Restrições

            //RHE
            Action<int, int> overrideRHE = (_m, _e) =>
            {
                foreach (var rhe in w.Rhes.Where(x => x.Mes == _m && ((x.Estagio ?? _e) == _e)))
                {
                    var realestagio = _e == 1 ? 1 : mesOperativo.Estagios + 1;

                    var rests = dadger.BlocoRhe.RheGrouped
                        .Where(x =>
                        {
                            var fs = x.Value.Where(y => (y is FuLine) || /*(y is FtLine) ||*/ (y is FiLine));

                            if (rhe.Restricao > 0 && x.Key[1] == rhe.Restricao) return true;

                            var ok = fs.Count() == (rhe.Usinas.Count() + rhe.Sistemas.Count());

                            if (ok)
                            {

                                rhe.Usinas.ForEach(y =>
                                    ok = ok && fs.Any(z => z is FuLine && z[3] == y)
                                    );
                                rhe.Sistemas.ForEach(y =>
                                   ok = ok && fs.Any(z => z is FiLine && ((FiLine)z).De == y.Item1 && ((FiLine)z).Para == y.Item2)
                                   );

                            }

                            return ok;
                        }).ToList();


                    if (!rhe.LimInf1.HasValue && !rhe.LimSup1.HasValue)
                    {

                        rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhe.Remove(x));
                        rests.Clear();

                    }
                    else if (rests.Count == 0)
                    {
                        var rest = new List<RheLine>
                        {
                            new ReLine()
                            {
                                Restricao = dadger.BlocoRhe.GetNextId(),
                                Inicio = realestagio,
                                Fim = mesOperativo.Estagios + 1
                            }
                        };
                        foreach (var fu in rhe.Usinas)
                        {
                            if (fu == 66 && rhe.Freq_itaipu != 0) // Quanto utilizado @ na planilha, para indicar a Frequencia de Itaipu
                            {
                                rest.Add(new FuLine() { Restricao = rest.First().Restricao, Usina = fu, Freq_Itaipu = rhe.Freq_itaipu });
                            }
                            else
                            {
                                rest.Add(new FuLine() { Restricao = rest.First().Restricao, Usina = fu });
                            }
                        }
                        foreach (var fi in rhe.Sistemas)
                        {
                            rest.Add(new FiLine() { Restricao = rest.First().Restricao, De = fi.Item1, Para = fi.Item2 });
                        }

                        rest.ForEach(x => dadger.BlocoRhe.Add(x));

                        rests.Add(new KeyValuePair<ReLine, List<RheLine>>((ReLine)rest.First(), rest));
                    }

                    //if (rest != null) {

                    foreach (var rest in rests)
                    {
                        var lu = (LuLine)rest.Value.FirstOrDefault(y => (y is LuLine) && y[2] == realestagio)
                            ?? new LuLine() { Restricao = rest.Value.First().Restricao, Estagio = realestagio }
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

            // recebimento NE && (bloco IA fluxo IV-SE (utilizando restricoes no caso de nao haver dados na planilha ))


            foreach (var dt in new Tuple<DateTime, int>[] { new Tuple<DateTime, int>(new DateTime(mesOperativo.Ano, mesOperativo.Mes, 1),1)
                , new Tuple<DateTime, int>(new DateTime(mesOperativo.AnoSeguinte, mesOperativo.MesSeguinte,1), mesOperativo.Estagios + 1) })

            {
                {
                    // if ((dt.Item1.Month <= DateTime.Today.AddMonths(1).Month && dt.Item1.Year <= DateTime.Today.Year) || (DateTime.Today.AddMonths(1).Month == 1) || (dt.Item2 == 6 && dt.Item1.Month == DateTime.Today.AddMonths(2).Month))
                    //{
                    //IDB objSQL = new SQLServerDBCompass("ESTUDO_PV");
                    //DbDataReader reader = null;
                    //string[] campos = { "[Data]", "[submercado]", "[Ano]", "[Janeiro]", "[Fevereiro]", "[Março]", "[Abril]", "[Maio]", "[Junho]", "[Julho]", "[Agosto]", "[Setembro]", "[Outubro]", "[Novembro]", "[Dezembro]" };

                    //string tabela = "[ESTUDO_PV].[dbo].[UEE]";

                    //string strQuery = String.Format(@"SELECT TOP 5 [id],[Ano],[Janeiro],[Fevereiro],[Março] ,[Abril],[Maio],[Junho],[Julho],[Agosto],[Setembro] ,[Outubro],[Novembro],[Dezembro]FROM [ESTUDO_PV].[dbo].[UEE] where YEAR(Data) = YEAR(GETDATE()) order by ano asc ");
                    //List<double[]> UEE = new List<double[]>();
                    //reader = objSQL.GetReader(strQuery);


                    ////double UEE;
                    //try
                    //{
                    //    while (reader.Read())
                    //    {

                    //        double[] dados = new double[14];

                    //        for (int j = 0; j < 14; j++)
                    //        {
                    //            dados[j] = Convert.ToDouble(reader[j]);
                    //            // dados[j] = teste;
                    //        }
                    //        UEE.Add(dados);
                    //        //dados = null;

                    //    }
                    //}
                    //finally
                    //{
                    //    // Fecha o datareader
                    //    if (reader != null)
                    //    {
                    //        reader.Close();
                    //    }
                    //}


                    //var sistemaNE = sistemaDat.Mercado.Where(x => x.Mercado == 3 && x.Ano == dt.Item1.Year).First()[dt.Item1.Month];
                    //var patsNE = patamarDat.Carga.Where(x => x.Ano == dt.Item1.Year && x.Mercado == 3).ToList();

                    //var NEPT1 = Math.Round((sistemaNE * patsNE[0][dt.Item1.Month]));
                    //var NEPT2 = Math.Round((sistemaNE * patsNE[1][dt.Item1.Month]));
                    //var NEPT3 = Math.Round((sistemaNE * patsNE[2][dt.Item1.Month]));

                    //var cargaNEPT1 = Math.Round((sistemaNE * patsNE[0][dt.Item1.Month] * 0.43));//43% da energia do mercado vezes patamares do mercado
                    //var cargaNEPT2 = Math.Round((sistemaNE * patsNE[1][dt.Item1.Month] * 0.43));
                    //var cargaNEPT3 = Math.Round((sistemaNE * patsNE[2][dt.Item1.Month] * 0.43));

                    //List<int> newave = new List<int> { 6500, 6500, 6500, 6500, 6500, 6500, 5800, 5800, 5800, 5800, 5800, 5800 };//Max valor que RecebimentoNE pode assumir em cada mês atè dez 2021
                }
                var agrintDat = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.agrint].Document as CommomLibrary.AgrintDat.AgrintDat;

                double agrintP1 = 0;
                double agrintP2 = 0;
                double agrintP3 = 0;

                foreach (var re in agrintDat[dt.Item1].Where(x => x.Value.Numero == 1))
                {
                    if (dt.Item1 >= re.Value.Inicio && dt.Item1 <= re.Value.Fim)
                    {
                        agrintP1 = re.Value.Lim_P1;
                        agrintP2 = re.Value.Lim_P2;
                        agrintP3 = re.Value.Lim_P3;
                    }

                }

                {
                    //var P1 = Math.Min(cargaNEPT1, agrintP1);
                    //var P2 = Math.Min(cargaNEPT2, agrintP2);
                    //var P3 = Math.Min(cargaNEPT3, agrintP3);

                    //if (dt.Item2 == 6)
                    //{
                    //    P1 = agrintP1;
                    //    P2 = agrintP2;
                    //    P3 = agrintP3;
                    //}

                    //double RPO1;
                    //double RPO2;
                    //double RPO3;
                    //if (dt.Item1.Year == DateTime.Today.AddYears(1).Year)
                    //{
                    //    RPO1 = Math.Round(NEPT1 * 0.04 + UEE[1][dt.Item1.Month + 1] * 0.06, 0);
                    //    RPO2 = Math.Round(NEPT2 * 0.04 + UEE[1][dt.Item1.Month + 1] * 0.06, 0);
                    //    RPO3 = Math.Round(NEPT3 * 0.04 + UEE[1][dt.Item1.Month + 1] * 0.06, 0);
                    //}
                    //else
                    //{
                    //    RPO1 = Math.Round(NEPT1 * 0.04 + UEE[0][dt.Item1.Month + 1] * 0.06, 0);
                    //    RPO2 = Math.Round(NEPT2 * 0.04 + UEE[0][dt.Item1.Month + 1] * 0.06, 0);
                    //    RPO3 = Math.Round(NEPT3 * 0.04 + UEE[0][dt.Item1.Month + 1] * 0.06, 0);
                    //}


                    //var RNE1 = P1 - RPO1;
                    //var RNE2 = P2 - RPO2;
                    //var RNE3 = P3 - RPO3;
                    // var rheEqu = dadger.BlocoRhe.Where(x => x.Restricao == 403);
                }

                var rheEqu =
                        dadger.BlocoRhe.RheGrouped.Where(rhe =>
                        {
                            var pred = rhe.Value.Where(x => x is FiLine).All(x => ((FiLine)x).Restricao == 403);
                            pred = pred && !rhe.Value.Any(x => !(x is FiLine || x is ReLine || x is LuLine));
                            return pred;
                        }).Select(x => x.Value)
                        .FirstOrDefault();
                LuLine lu;
                if (rheEqu != null)
                {
                    lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                    if (lu == null)
                    {
                        lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                        dadger.BlocoRhe.Add(lu);
                    }
                    lu[4] = agrintP1;
                    lu[6] = agrintP2;
                    lu[8] = agrintP3;
                }

                var resPlan = w.Rhes.Where(y => y.Sistemas.Count() > 0 && y.Sistemas[0].Item1 == "IV" && y.Sistemas[0].Item2 == "SE").ToList();

                if (resPlan.Count() == 0)
                {
                    var LinhasIa = dadger.BlocoIa.Where(x => x.SistemaA == "SE" && x.SistemaB == "IV").ToList();
                    var Ial = LinhasIa.Where(x => x.Estagio == dt.Item2).First();

                    var rheIa =
                         dadger.BlocoRhe.RheGrouped.Where(rhe =>
                         {
                             var pred = rhe.Value.Where(x => x is FiLine).All(x => ((FiLine)x).Restricao == 441);
                             pred = pred && !rhe.Value.Any(x => !(x is FiLine || x is ReLine || x is LuLine));
                             return pred;
                         }).Select(x => x.Value)
                            .FirstOrDefault();
                    LuLine luIa;
                    if (rheIa != null)
                    {
                        luIa = (LuLine)rheIa.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);


                        Ial[6] = luIa[4];
                        Ial[8] = luIa[6];
                        Ial[10] = luIa[8];
                    }
                }


                //}
            }
            //sobrescrever bloco IA com dados das restrições equivalentes
            var rhestIntercambios = dadger.BlocoRhe.RheGrouped.Where(x => x.Value.Where(y => y is FiLine).ToList().Count() == 1 && x.Value.All(z => !(z is FuLine))).ToList();
            // intercambio NE-FC

            var restNEFC = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "NE" && y[4] == "FC")).First();
            var lus = restNEFC.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lus)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "NE" && x.SistemaB == "FC" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[5] = lu[4];
                    ia2[7] = lu[6];
                    ia2[9] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[5] = lu[4];
                    ia[7] = lu[6];
                    ia[9] = lu[8];
                }
            }

            var restFCNE = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "FC" && y[4] == "NE")).First();
            var lusFCNE = restFCNE.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusFCNE)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "NE" && x.SistemaB == "FC" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[6] = lu[4];
                    ia2[8] = lu[6];
                    ia2[10] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[6] = lu[4];
                    ia[8] = lu[6];
                    ia[10] = lu[8];
                }
            }
            // intercambio SE-FC
            var restSEFC = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "SE" && y[4] == "FC")).First();
            var lusSEFC = restSEFC.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusSEFC)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "SE" && x.SistemaB == "FC" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[5] = lu[4];
                    ia2[7] = lu[6];
                    ia2[9] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[5] = lu[4];
                    ia[7] = lu[6];
                    ia[9] = lu[8];
                }
            }

            var restFCSE = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "FC" && y[4] == "SE")).First();
            var lusFCSE = restFCSE.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusFCSE)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "SE" && x.SistemaB == "FC" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[6] = lu[4];
                    ia2[8] = lu[6];
                    ia2[10] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[6] = lu[4];
                    ia[8] = lu[6];
                    ia[10] = lu[8];
                }
            }
            // intercambio SE-IV
            var restSEIV = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "SE" && y[4] == "IV")).First();
            var lusSEIV = restSEIV.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusSEIV)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "SE" && x.SistemaB == "IV" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[5] = lu[4];
                    ia2[7] = lu[6];
                    ia2[9] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[5] = lu[4];
                    ia[7] = lu[6];
                    ia[9] = lu[8];
                }
            }

            var restIVSE = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "IV" && y[4] == "SE")).First();
            var lusIVSE = restIVSE.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusIVSE)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "SE" && x.SistemaB == "IV" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[6] = lu[4];
                    ia2[8] = lu[6];
                    ia2[10] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[6] = lu[4];
                    ia[8] = lu[6];
                    ia[10] = lu[8];
                }
            }
            // intercambio SE-NE
            var restSENE = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "SE" && y[4] == "NE")).First();
            var lusSENE = restSENE.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusSENE)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "SE" && x.SistemaB == "NE" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[5] = lu[4];
                    ia2[7] = lu[6];
                    ia2[9] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[5] = lu[4];
                    ia[7] = lu[6];
                    ia[9] = lu[8];
                }
            }

            var restNESE = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "NE" && y[4] == "SE")).First();
            var lusNESE = restNESE.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusNESE)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "SE" && x.SistemaB == "NE" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[6] = lu[4];
                    ia2[8] = lu[6];
                    ia2[10] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[6] = lu[4];
                    ia[8] = lu[6];
                    ia[10] = lu[8];
                }
            }
            // intercambio IV-S
            var restIVS = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "IV" && y[4] == "S ")).First();
            var lusIVS = restIVS.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusIVS)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "IV" && x.SistemaB == "S" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[5] = lu[4];
                    ia2[7] = lu[6];
                    ia2[9] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[5] = lu[4];
                    ia[7] = lu[6];
                    ia[9] = lu[8];
                }
            }

            var restSIV = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "S " && y[4] == "IV")).First();
            var lusSIV = restSIV.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusSIV)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "IV" && x.SistemaB == "S" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[6] = lu[4];
                    ia2[8] = lu[6];
                    ia2[10] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[6] = lu[4];
                    ia[8] = lu[6];
                    ia[10] = lu[8];
                }
            }
            // intercambio N-SE
            var restNSE = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "N " && y[4] == "SE")).First();
            var lusNSE = restNSE.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusNSE)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "N" && x.SistemaB == "SE" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[5] = lu[4];
                    ia2[7] = lu[6];
                    ia2[9] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[5] = lu[4];
                    ia[7] = lu[6];
                    ia[9] = lu[8];
                }
            }

            var restSEN = rhestIntercambios.Where(x => x.Value.Any(y => (y is FiLine) && y[3] == "SE" && y[4] == "N ")).First();
            var lusSEN = restSEN.Value.Where(x => x is LuLine).ToList();
            foreach (var lu in lusSEN)
            {
                var ia = dadger.BlocoIa.Where(x => x.SistemaA == "N" && x.SistemaB == "SE" && x.Estagio <= lu[2]).OrderByDescending(x => x.Estagio).FirstOrDefault();
                if (ia.Estagio < lu[2])
                {
                    IaLine ia2 = (IaLine)ia.Clone();
                    ia2[1] = lu[2];
                    ia2[6] = lu[4];
                    ia2[8] = lu[6];
                    ia2[10] = lu[8];
                    dadger.BlocoIa.InsertAfter(ia, ia2);
                    ia2 = null;
                }
                else
                {
                    ia[6] = lu[4];
                    ia[8] = lu[6];
                    ia[10] = lu[8];
                }
            }

            //RHV
            Action<int, int, int> overrideRHV = (_m, _ano, _e) =>
            {
                var RhvPassadas = w.Rhvs.Where(x => (x.Mes < _m && x.Ano <= _ano && x.exclui == true) || (x.Ano < _ano && x.exclui == true)).ToList();
                var RhvAtuais = w.Rhvs.Where(x => x.Mes == _m && x.Ano == _ano).ToList();

                if (_e == 1)//esse if serve para manter  apagadas para os meses seguintes as restrições que pertenciam ao deck de entrada e que o usuario excluiu atraves da planilha 
                {
                    foreach (var RhvP in RhvPassadas)
                    {
                        if (RhvAtuais.Any(x => x.Restricao == RhvP.Restricao))
                        {
                            continue;
                        }
                        else
                        {
                            var restApaga = dadger.BlocoRhv.RhvGrouped
                            .Where(x => x.Value.Any(y => (y is CvLine) && y[5] == "VARM" && (y[3] == RhvP.Usina || (y[1] == RhvP.Restricao && y[1] != 43))))// a restricao 43 tem que ser tratada a cada mes por conta de ter a proporcionalidade de um jeito que a planilha não consegue escrever
                            //.Select(x => x.Value).FirstOrDefault();                                                                                       // por tanto ela é resgatada do deck base caso tenha que ser incluida novamente no estudo
                            .ToList();

                            restApaga.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhv.Remove(x));
                            restApaga.Clear();
                        }

                    }
                }

                foreach (var rhv in w.Rhvs.Where(x => x.Mes == _m && x.Ano == _ano && ((x.Estagio ?? _e) == _e)))
                {
                    var realestagio = _e == 1 ? 1 : mesOperativo.Estagios + 1;
                    var rests = dadger.BlocoRhv.RhvGrouped
                        .Where(x => x.Value.Any(y => (y is CvLine) && y[5] == "VARM" && (y[3] == rhv.Usina || y[1] == rhv.Restricao)))
                        //.Select(x => x.Value).FirstOrDefault();
                        .ToList();

                    if (!rhv.LimInf.HasValue && !rhv.LimSup.HasValue && rhv.Restricao != 43)
                    {

                        rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhv.Remove(x));
                        rests.Clear();

                    }
                    else if (rests.Count == 0 && rhv.exclui == false)
                    {
                        var rest = new List<RhvLine>();
                        rest.Add(new HvLine()
                        {
                            Restricao = dadger.BlocoRhv.GetNextId(),
                            Inicio = realestagio,
                            Fim = mesOperativo.Estagios + 1,
                        });

                        rest.Add(new CvLine() { Restricao = rest.First().Restricao, Usina = rhv.Usina, Tipo = "VARM" });


                        rest.ForEach(x => dadger.BlocoRhv.Add(x));
                        rests.Add(new KeyValuePair<HvLine, List<RhvLine>>((HvLine)rest.First(), rest));
                    }

                    foreach (var rest in rests)
                    {
                        if (rhv.Usina == 275 && rhv.LimInf == rhv.LimSup)
                        {
                            var lu = (LvLine)rest.Value.FirstOrDefault(y => (y is LvLine) && y[2] == realestagio)
                            ?? new LvLine() { Restricao = rest.Value.First().Restricao, Estagio = realestagio }
                            ;
                            lu[3] = rhv.LimInf;
                            lu[4] = rhv.LimSup;
                            if (!rest.Value.Contains(lu)) dadger.BlocoRhv.Add(lu);


                        }
                        else if (rhv.Usina == 275 && rhv.LimInf != rhv.LimSup)
                        {
                            var lu = (LvLine)rest.Value.FirstOrDefault(y => (y is LvLine) && y[2] == realestagio)
                            ?? new LvLine() { Restricao = rest.Value.First().Restricao, Estagio = realestagio };


                            lu[3] = rhv.LimInf;
                            lu[4] = rhv.LimSup;

                            if (!rest.Value.Contains(lu)) dadger.BlocoRhv.Add(lu);


                            //var lu = (LvLine)rest.Value.FirstOrDefault(y => (y is LvLine) && y[2] == realestagio);
                            //if (lu != null)
                            //{
                            //    lu[3] = rhv.LimInf;
                            //    lu[4] = rhv.LimSup;

                            //    if (!rest.Value.Contains(lu)) dadger.BlocoRhv.Add(lu);
                            //}


                        }
                        else
                        {
                            if (rhv.exclui == false)
                            {
                                var lu = (LvLine)rest.Value.FirstOrDefault(y => (y is LvLine) && y[2] == realestagio)
                         ?? new LvLine() { Restricao = rest.Value.First().Restricao, Estagio = realestagio }
                         ;
                                lu[3] = rhv.LimInf;
                                lu[4] = rhv.LimSup;

                                if (!rest.Value.Contains(lu)) dadger.BlocoRhv.Add(lu);
                            }

                        }



                    }
                    if (rhv.exclui && _e == 1)
                    {
                        rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhv.Remove(x));
                        rests.Clear();
                    }
                    else if (rhv.exclui && _e == 2)
                    {
                        var Nrests = dadger.BlocoRhv.RhvGrouped
                        .Where(x => x.Value.Any(y => (y is CvLine) && y[5] == "VARM" && (y[3] == rhv.Usina || y[1] == rhv.Restricao)))
                        //.Select(x => x.Value).FirstOrDefault();
                        .ToList();
                        if (Nrests.Count() > 0)
                        {
                            foreach (var rest in Nrests)
                            {
                                var lu = (LvLine)rest.Value.FirstOrDefault(y => (y is LvLine) && y[2] == realestagio);
                                if (lu != null)
                                {
                                    dadger.BlocoRhv.Remove(lu);
                                }
                                if (rhv.Restricao == 43)
                                {
                                    var hv = (HvLine)rest.Value.FirstOrDefault(y => (y is HvLine) && y.Restricao == rhv.Restricao);
                                    if (hv != null)
                                    {
                                        hv[3] = realestagio - 1;// diminui 1 no estagio fim
                                    }
                                }
                            }
                        }
                    }
                }
            };

            overrideRHV(dtEstudo.Month, dtEstudo.Year, 1);
            overrideRHV(dtEstudo.Month, dtEstudo.Year, 2);

            //RHQ
            Action<int, int, int> overrideRHQ = (_m, _ano, _e) =>
            {

                var anolimite = _ano;

                var RhqPassadas = w.Rhqs.Where(x => (x.Mes < _m && x.Ano <= _ano && x.exclui == true) || (x.Ano < _ano && x.exclui == true)).ToList();
                var RhqAtuais = w.Rhqs.Where(x => x.Mes == _m && x.Ano == _ano).ToList();

                if (_e == 1)//esse if serve para manter  apagadas para os meses seguintes as restrições que pertenciam ao deck de entrada e que o usuario excluiu atraves da planilha 
                {
                    foreach (var RhqP in RhqPassadas)
                    {
                        if (RhqAtuais.Any(x => x.Usina == RhqP.Usina && x.minemonico == RhqP.minemonico))
                        {
                            continue;
                        }

                        else
                        {
                            var restApaga = dadger.BlocoRhq.RhqGrouped
                               .Where(x => x.Value.Any(y => (y is CqLine) && y[5] == RhqP.minemonico && (y[3] == RhqP.Usina || y[1] == RhqP.Restricao)))
                               //.Select(x => x.Value).FirstOrDefault();
                               .ToList();
                            foreach (var re in restApaga)
                            {

                                var countCq = re.Value.Where(y => (y is CqLine)).ToList();
                                if (RhqP.Usinas != null && countCq.Count() == RhqP.Usinas.Count() || RhqP.Usina == 0)
                                {
                                    re.Value.ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                                }
                                //var yesye = re.Value.ToList();//.ForEach(x => dadger.BlocoRhq.Remove(x));

                            }

                            //restApaga.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                            restApaga.Clear();
                        }

                    }
                }

                foreach (var rhq in w.Rhqs.Where(x => x.Mes == _m && x.Ano == _ano && ((x.Estagio ?? _e) == _e)))
                {



                    var realestagio = _e == 1 ? 1 : mesOperativo.Estagios + 1;
                    var rests = dadger.BlocoRhq.RhqGrouped
                        .Where(x => x.Value.Any(y => (y is CqLine) && y[5] == rhq.minemonico && (y[3] == rhq.Usina || y[1] == rhq.Restricao)))
                        //.Select(x => x.Value).FirstOrDefault();
                        .ToList();



                    if (!rhq.LimInf1.HasValue && !rhq.LimSup1.HasValue)
                    {
                        foreach (var re in rests)
                        {

                            var countCq = re.Value.Where(y => (y is CqLine)).ToList();
                            if (rhq.Usinas != null && countCq.Count() == rhq.Usinas.Count() || rhq.Usina == 0)
                            {
                                re.Value.ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                                //rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                                //rests.Clear();
                            }
                        }

                        rests.Clear();
                    }
                    else if (rests.Count == 0 && rhq.exclui == false)
                    {
                        //Alterado o fim de 2 para estagios + 1 pois estava errando os estágios na RV0
                        var rest = new List<RhqLine>();
                        rest.Add(new HqLine()
                        {
                            Restricao = dadger.BlocoRhq.GetNextId(),
                            Inicio = realestagio,
                            Fim = mesOperativo.Estagios + 1

                        });

                        rest.Add(new CqLine() { Restricao = rest.First().Restricao, Usina = rhq.Usina, Tipo = rhq.minemonico });

                        rest.ForEach(x => dadger.BlocoRhq.Add(x));

                        rests.Add(new KeyValuePair<HqLine, List<RhqLine>>((HqLine)rest.First(), rest));
                    }

                    foreach (var rest in rests)
                    {
                        var countCq = rest.Value.Where(y => (y is CqLine)).ToList();
                        if (rhq.Usinas != null && countCq.Count() == rhq.Usinas.Count() || rhq.Usina == 0)
                        {
                            if (rhq.Usina == 0)
                            {

                            }
                            //rest.Value.ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                            //rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                            //rests.Clear();
                            var lu = (LqLine)rest.Value.FirstOrDefault(y => (y is LqLine) && y[2] == realestagio)
                            ?? new LqLine() { Restricao = rest.Value.First().Restricao, Estagio = realestagio }
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
                    if (rhq.exclui)
                    {
                        foreach (var re in rests)
                        {

                            var countCq = re.Value.Where(y => (y is CqLine)).ToList();
                            if (rhq.Usinas != null && countCq.Count() == rhq.Usinas.Count() || rhq.Usina == 0)
                            {
                                re.Value.ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                                //rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                                //rests.Clear();
                            }
                        }


                        // rests.SelectMany(x => x.Value).ToList().ForEach(x => dadger.BlocoRhq.Remove(x));
                        rests.Clear();
                    }


                }


                var restsUsiZero = dadger.BlocoRhq.RhqGrouped
                        .Where(x => x.Value.Any(y => (y is CqLine) && y[3] == 0))
                        //.Select(x => x.Value).FirstOrDefault();
                        .ToList();

                foreach (var re in restsUsiZero)//limpar hqs com usina numero zero
                {

                    re.Value.ToList().ForEach(x => dadger.BlocoRhq.Remove(x));


                }



            };

            overrideRHQ(dtEstudo.Month, dtEstudo.Year, 1);
            overrideRHQ(dtEstudo.Month, dtEstudo.Year, 2);

            #endregion

            #region Sobreescrever Alteracoes Cadastrais (AC)

            // parte cotvol de bm e pimental
            var acsBM_P = w.Acs.Where(x => ((x.Mes == mesOperativo.Mes && x.Ano == mesOperativo.Ano || x.Mes == mesOperativo.MesSeguinte && x.Ano == mesOperativo.AnoSeguinte || x.Mes == 0) && (x.Usina == 288 || x.Usina == 314)) && minemonicosAlvo.Any(y => y.Contains(x.Mnemonico)));

            //var bm_pLines = dadger.BlocoAc.Where(x => (x.Usina == 288 || x.Usina == 314) && minemonicosAlvo.Any(y => y.Contains(x.Mnemonico))).ToList();

            var valsPadrao = getValsCotvolBM_P();

            if (temAcCOtvolBM_P)
            {
                List<int> usinas = new List<int> { 288, 314 };

                var padrao = valsPadrao[mesOperativo.Mes - 1];
                var padrao2 = valsPadrao[mesOperativo.MesSeguinte - 1];

                foreach (var usi in usinas)
                {
                    //1°mes cotvol
                    var acsDummy = acsBM_P.Where(x => x.Mes == mesOperativo.Mes && x.Usina == usi && x.Mnemonico == "COTVOL").ToList();

                    if (acsDummy.Count() > 0)
                    {
                        foreach (var acsbm in acsDummy)
                        {
                            var acL = dadger.BlocoAc.CreateLineFromMnemonico(acsbm.Mnemonico);
                            acL.Usina = acsbm.Usina;
                            acL.SetValue(3, acsbm.Valor1);
                            acL.SetValue(4, acsbm.Valor2);
                            if (acsbm.Valor3 != null)
                                acL.SetValue(5, acsbm.Valor3);
                            //acL.Semana = 1;
                            ////acL.Ano = acsbm.Ano;
                            //acL.Mes = mesOperativo.Fim.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();


                            dadger.BlocoAc.Add(acL);
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            var acL = dadger.BlocoAc.CreateLineFromMnemonico("COTVOL");
                            acL.Usina = usi;
                            acL.SetValue(3, i);
                            acL.SetValue(4, i == 1 ? padrao.Item1 : 0.000);
                            //if (acsbm.Valor3 != null)
                            //    acL.SetValue(5, acsbm.Valor3);

                            //acL.Semana = 1;
                            ////acL.Ano = mesOperativo.Ano;
                            //acL.Mes = mesOperativo.Fim.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();


                            dadger.BlocoAc.Add(acL);
                        }
                    }

                    //2°mes cotvol
                    var acsDummy2 = acsBM_P.Where(x => x.Mes == mesOperativo.MesSeguinte && x.Usina == usi && x.Mnemonico == "COTVOL").ToList();

                    if (acsDummy2.Count() > 0)
                    {
                        foreach (var acsbm in acsDummy2)
                        {
                            var acL = dadger.BlocoAc.CreateLineFromMnemonico(acsbm.Mnemonico);
                            acL.Usina = acsbm.Usina;
                            acL.SetValue(3, acsbm.Valor1);
                            acL.SetValue(4, acsbm.Valor2);
                            if (acsbm.Valor3 != null)
                                acL.SetValue(5, acsbm.Valor3);

                            acL.Ano = acsbm.Ano;
                            acL.Mes = mesOperativo.Fim.AddDays(7).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();


                            dadger.BlocoAc.Add(acL);
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            var acL = dadger.BlocoAc.CreateLineFromMnemonico("COTVOL");
                            acL.Usina = usi;
                            acL.SetValue(3, i);
                            acL.SetValue(4, i == 1 ? padrao2.Item1 : 0.000);
                            //if (acsbm.Valor3 != null)
                            //    acL.SetValue(5, acsbm.Valor3);

                            acL.Ano = mesOperativo.AnoSeguinte;
                            acL.Mes = mesOperativo.Fim.AddDays(7).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();


                            dadger.BlocoAc.Add(acL);
                        }
                    }

                    //1°mes  "VOLMIN", "VOLMAX", "VSVERT", "VMDESV"

                    var VOLMINX = acsBM_P.Where(x => x.Mes == mesOperativo.Mes && x.Usina == usi && x.Mnemonico == "VOLMIN").FirstOrDefault();
                    var VOLMAXX = acsBM_P.Where(x => x.Mes == mesOperativo.Mes && x.Usina == usi && x.Mnemonico == "VOLMAX").FirstOrDefault();
                    var VSVERTX = acsBM_P.Where(x => x.Mes == mesOperativo.Mes && x.Usina == usi && x.Mnemonico == "VSVERT").FirstOrDefault();
                    var VMDESVX = acsBM_P.Where(x => x.Mes == mesOperativo.Mes && x.Usina == usi && x.Mnemonico == "VMDESV").FirstOrDefault();

                    double vminteste;
                    double vmaxteste;

                    var aclvolmin = dadger.BlocoAc.CreateLineFromMnemonico("VOLMIN");

                    aclvolmin.Usina = usi;

                    if (VOLMINX != null)
                    {
                        aclvolmin.SetValue(3, VOLMINX.Valor1 != null ? VOLMINX.Valor1 : padrao2.Item2);
                        vminteste = Convert.ToDouble(VOLMINX.Valor1 != null ? VOLMINX.Valor1 : padrao2.Item2);
                    }
                    else
                    {
                        aclvolmin.SetValue(3, padrao.Item2);
                        vminteste = padrao.Item2;
                    }

                    //aclvolmin.Semana = 1;
                    // //aclvolmin.Ano = mesOperativo.Ano;
                    //aclvolmin.Mes = mesOperativo.Fim.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    dadger.BlocoAc.Add(aclvolmin);


                    var aclvolmax = dadger.BlocoAc.CreateLineFromMnemonico("VOLMAX");

                    aclvolmax.Usina = usi;

                    if (VOLMAXX != null)
                    {
                        aclvolmax.SetValue(3, VOLMAXX.Valor1 != null ? VOLMAXX.Valor1 : padrao2.Item2);
                        vmaxteste = Convert.ToDouble(VOLMAXX.Valor1 != null ? VOLMAXX.Valor1 : padrao2.Item2);
                    }
                    else
                    {
                        aclvolmax.SetValue(3, padrao.Item2);
                        vmaxteste = padrao.Item2;
                    }

                    // aclvolmax.Semana = 1;
                    ////aclvolmax.Ano = mesOperativo.Ano;
                    //aclvolmax.Mes = mesOperativo.Fim.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    dadger.BlocoAc.Add(aclvolmax);

                    var aclvsvert = dadger.BlocoAc.CreateLineFromMnemonico("VSVERT");

                    aclvsvert.Usina = usi;

                    if (VSVERTX != null)
                    {
                        aclvsvert.SetValue(3, VSVERTX.Valor1 != null ? VSVERTX.Valor1 : padrao2.Item2);
                    }
                    else
                    {
                        aclvsvert.SetValue(3, padrao.Item2);
                    }

                    //aclvsvert.Semana = 1;
                    /// //aclvsvert.Ano = mesOperativo.Ano;
                    //aclvsvert.Mes = mesOperativo.Fim.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    dadger.BlocoAc.Add(aclvsvert);

                    var aclvmdesv = dadger.BlocoAc.CreateLineFromMnemonico("VMDESV");

                    aclvmdesv.Usina = usi;

                    if (VMDESVX != null)
                    {
                        aclvmdesv.SetValue(3, VMDESVX.Valor1 != null ? VMDESVX.Valor1 : padrao2.Item2);
                    }
                    else
                    {
                        aclvmdesv.SetValue(3, padrao.Item2);
                    }

                    //aclvmdesv.Semana = 1;
                    ////aclvmdesv.Ano = mesOperativo.Ano;
                    //aclvmdesv.Mes = mesOperativo.Fim.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    dadger.BlocoAc.Add(aclvmdesv);

                    //2°mes "VOLMIN", "VOLMAX", "VSVERT", "VMDESV"
                    var VOLMINX_2 = acsBM_P.Where(x => x.Mes == mesOperativo.MesSeguinte && x.Usina == usi && x.Mnemonico == "VOLMIN").FirstOrDefault();
                    var VOLMAXX_2 = acsBM_P.Where(x => x.Mes == mesOperativo.MesSeguinte && x.Usina == usi && x.Mnemonico == "VOLMAX").FirstOrDefault();
                    var VSVERTX_2 = acsBM_P.Where(x => x.Mes == mesOperativo.MesSeguinte && x.Usina == usi && x.Mnemonico == "VSVERT").FirstOrDefault();
                    var VMDESVX_2 = acsBM_P.Where(x => x.Mes == mesOperativo.MesSeguinte && x.Usina == usi && x.Mnemonico == "VMDESV").FirstOrDefault();

                    //double volTeste = padrao2.Item2;

                    string minemonicoLinha1 = "";
                    string minemonicoLinha2 = "";
                    double volLinha1;
                    double volLinha2;

                    double vminteste2 = VOLMINX_2 != null ? VOLMINX_2.Valor1 != null ? Convert.ToDouble(VOLMINX_2.Valor1) : padrao2.Item2 : padrao2.Item2;
                    double vmaxteste2 = VOLMAXX_2 != null ? VOLMAXX_2.Valor1 != null ? Convert.ToDouble(VOLMAXX_2.Valor1) : padrao2.Item2 : padrao2.Item2;

                    double vsvertSeguinte = VSVERTX_2 != null ? VSVERTX_2.Valor1 != null ? Convert.ToDouble(VSVERTX_2.Valor1) : padrao2.Item2 : padrao2.Item2;
                    double vmdesvSeguinte = VMDESVX_2 != null ? VMDESVX_2.Valor1 != null ? Convert.ToDouble(VMDESVX_2.Valor1) : padrao2.Item2 : padrao2.Item2;

                    if (vminteste2 <= vmaxteste)//verifica a ordem para inserir os limites se volmin maior que volmax anterior  primeiro inserir a linha de volmax 
                    {
                        minemonicoLinha1 = "VOLMIN";
                        minemonicoLinha2 = "VOLMAX";
                        volLinha1 = vminteste2;
                        volLinha2 = vmaxteste2;
                    }
                    else
                    {
                        minemonicoLinha1 = "VOLMAX";
                        minemonicoLinha2 = "VOLMIN";
                        volLinha1 = vmaxteste2;
                        volLinha2 = vminteste2;
                    }

                    var aclvolseguinte1 = dadger.BlocoAc.CreateLineFromMnemonico(minemonicoLinha1);
                    aclvolseguinte1.Usina = usi;
                    aclvolseguinte1.SetValue(3, volLinha1);
                    aclvolseguinte1.Ano = mesOperativo.AnoSeguinte;
                    aclvolseguinte1.Mes = mesOperativo.Fim.AddDays(7).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    dadger.BlocoAc.Add(aclvolseguinte1);

                    var aclvolseguinte2 = dadger.BlocoAc.CreateLineFromMnemonico(minemonicoLinha2);
                    aclvolseguinte2.Usina = usi;
                    aclvolseguinte2.SetValue(3, volLinha2);
                    aclvolseguinte2.Ano = mesOperativo.AnoSeguinte;
                    aclvolseguinte2.Mes = mesOperativo.Fim.AddDays(7).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    dadger.BlocoAc.Add(aclvolseguinte2);

                    var aclvsvertSeguinte = dadger.BlocoAc.CreateLineFromMnemonico("VSVERT");
                    aclvsvertSeguinte.Usina = usi;
                    aclvsvertSeguinte.SetValue(3, vsvertSeguinte);
                    aclvsvertSeguinte.Ano = mesOperativo.AnoSeguinte;
                    aclvsvertSeguinte.Mes = mesOperativo.Fim.AddDays(7).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    dadger.BlocoAc.Add(aclvsvertSeguinte);

                    var aclvmdesvSeguinte = dadger.BlocoAc.CreateLineFromMnemonico("VMDESV");
                    aclvmdesvSeguinte.Usina = usi;
                    aclvmdesvSeguinte.SetValue(3, vmdesvSeguinte);
                    aclvmdesvSeguinte.Ano = mesOperativo.AnoSeguinte;
                    aclvmdesvSeguinte.Mes = mesOperativo.Fim.AddDays(7).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                    dadger.BlocoAc.Add(aclvmdesvSeguinte);

                }

                //foreach (var acsbm in acsBM_P.Where(x => x.Mes == mesOperativo.Mes))
                //{
                //    var acL = dadger.BlocoAc.CreateLineFromMnemonico(acsbm.Mnemonico);
                //    acL.Usina = acsbm.Usina;
                //    acL.SetValue(3, acsbm.Valor1);
                //    acL.SetValue(4, acsbm.Valor2);
                //    if (acsbm.Valor3 != null)
                //        acL.SetValue(5, acsbm.Valor3);

                //    acL.Ano = acsbm.Ano;
                //    acL.Mes = mesOperativo.Fim.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();


                //    dadger.BlocoAc.Add(acL);
                //}

                //foreach (var acsbm in acsBM_P.Where(x => x.Mes == mesOperativo.MesSeguinte))
                //{
                //    var acL = dadger.BlocoAc.CreateLineFromMnemonico(acsbm.Mnemonico);
                //    acL.Usina = acsbm.Usina;
                //    acL.SetValue(3, acsbm.Valor1);
                //    acL.SetValue(4, acsbm.Valor2);
                //    if (acsbm.Valor3 != null)
                //        acL.SetValue(5, acsbm.Valor3);

                //    acL.Ano = acsbm.Ano;
                //    acL.Mes = mesOperativo.Fim.AddDays(7).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();


                //    dadger.BlocoAc.Add(acL);
                //}
            }
            ///

            //restante
            var acs = w.Acs.Where(x => (x.Mes == mesOperativo.Mes || x.Mes == mesOperativo.MesSeguinte || x.Mes == 0) &&
               dadger.BlocoUh.Any(y => y.Usina == x.Usina));

            foreach (var ac in acs)
            {
                if ((ac.Usina == 288 || ac.Usina == 314) && minemonicosAlvo.Any(x => x.Contains(ac.Mnemonico)))
                {
                    continue;
                }
                //apagar outras modifs existentes
                if (ac.Mes == 0)
                {
                    var toRemove = dadger.BlocoAc.Where(x => x.Usina == ac.Usina && x.Mnemonico == ac.Mnemonico).ToList();
                    foreach (var item in toRemove) dadger.BlocoAc.Remove(item);
                }

                var acL = dadger.BlocoAc.CreateLineFromMnemonico(ac.Mnemonico);
                acL.Usina = ac.Usina;
                acL.SetValue(3, ac.Valor1);
                acL.SetValue(4, ac.Valor2);
                if (ac.Valor3 != null)
                    acL.SetValue(5, ac.Valor3);


                if (ac.Mes == mesOperativo.MesSeguinte)
                {
                    acL.Ano = mesOperativo.AnoSeguinte;
                    acL.Mes = mesOperativo.Fim.AddDays(7).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
                }

                dadger.BlocoAc.Add(acL);
            }

            #endregion

            return dadger;
        }

        private static List<Tuple<double, double>> getValsCotvolBM_P()
        {
            //& |   JAN   |   FEV   |   MAR   |   ABR   |   MAI   |   JUN   |   JUL   |   AGO   |   SET   |   OUT   |   NOV   |   DEZ   |
            //& |  96.90  |  96.87  |  96.91  |  96.89  |  96.76  |  96.82  |  96.98  |  97.00  |  96.44  |  95.32  |  96.01  |  96.78  |  (m)
            //& | 2240.45 | 2229.75 | 2244.02 | 2236.88 | 2190.77 | 2211.99 | 2269.10 | 2276.29 | 2079.48 | 1715.01 | 1934.91 | 2197.83 |  (Hm3)

            List<Tuple<double, double>> vals = new List<Tuple<double, double>>
            {
               new Tuple<double, double>(96.90, 2240.45),//jan
               new Tuple<double, double>(96.87, 2229.75),//fev
               new Tuple<double, double>(96.91, 2244.02),//mar
               new Tuple<double, double>(96.89, 2236.88),//abr
               new Tuple<double, double>(96.76, 2190.77),//mai
               new Tuple<double, double>(96.82, 2211.99),//jun
               new Tuple<double, double>(96.98, 2269.10),//jul
               new Tuple<double, double>(97.00, 2276.29),//ago
               new Tuple<double, double>(96.44, 2079.48),//set
               new Tuple<double, double>(95.32, 1715.01),//out
               new Tuple<double, double>(96.01, 1934.91),//nov
               new Tuple<double, double>(96.78, 2197.83),//dez

            };
            return vals;
        }
        private static void trataRHEs(CommomLibrary.Newave.Deck deckNWEstudo, Dadger dadger, MesOperativo mesOperativo, CommomLibrary.PatamarDat.PatamarDat patamarDat, CommomLibrary.SistemaDat.SistemaDat sistemaDat)
        {
            foreach (var rh in dadger.BlocoRhe.RheGrouped)
            {

                if (rh.Key[1] == 441)
                {

                }
                rh.Key[2] = 1;
                rh.Key[3] = mesOperativo.Estagios + 1;

                rh.Value.Where(x => x is FuLine || x is FiLine || x is FtLine || x is FeLine).ToList().ForEach(x => x[2] = 1);

                var ls = rh.Value.Where(x => x is LuLine).OrderBy(x => x[2]);

                foreach (var l in ls)
                {
                    if (l == ls.Last()) l[2] = 1;   // muda o ultimo estagio para 1, a fim de calcular o primiero estagio da rv0 do mes seguinte 
                    else dadger.BlocoRhe.Remove(l);
                }
            }

            foreach (var dt in new Tuple<DateTime, int>[] { new Tuple<DateTime, int>(new DateTime(mesOperativo.Ano, mesOperativo.Mes, 1),1)
                , new Tuple<DateTime, int>(new DateTime(mesOperativo.AnoSeguinte, mesOperativo.MesSeguinte,1), mesOperativo.Estagios + 1) })

            {
                //ghminDat

                var ghminDat = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.ghmin].Document as CommomLibrary.GhminDat.GhminDat;
                foreach (var re in ghminDat.Where(x => x.Data == dt.Item1).OrderBy(x => x.Patamar).GroupBy(x => x.Cod))
                {

                    if (!dadger.BlocoUh.Any(x => x.Usina == re.Key)) continue;

                    var rheEqu =
                        dadger.BlocoRhe.RheGrouped.Where(rhe =>
                        {
                            var pred = rhe.Value.Where(x => x is FuLine).All(x => ((FuLine)x).Usina == re.Key);
                            pred = pred && !rhe.Value.Any(x => !(x is FuLine || x is ReLine || x is LuLine));
                            return pred;
                        }).Select(x => x.Value)
                        .FirstOrDefault();

                    LuLine lu;
                    if (rheEqu != null)
                    {
                        lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                        if (lu == null)
                        {
                            lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                            dadger.BlocoRhe.Add(lu);
                        }

                        if (re.Count() == 1)
                        {
                            lu[3] = lu[5] = lu[7] = re.First().Potencia;
                        }
                        else
                        {
                            lu[3] = re.First().Potencia;
                            lu[5] = re.Skip(1).First().Potencia;
                            lu[7] = re.Skip(2).First().Potencia;
                        }
                    }
                }

                //reDat
                string redatBase = Path.Combine(deckNWEstudo.BaseFolder, "re_base.dat");
                Compass.CommomLibrary.ReDat.ReDat reDat;

                if (File.Exists(redatBase))
                {
                    reDat = (Compass.CommomLibrary.ReDat.ReDat)DocumentFactory.Create(redatBase);
                }
                else
                {
                    reDat = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.re].Document as CommomLibrary.ReDat.ReDat;
                }

                foreach (var re in reDat[dt.Item1])
                {

                    var usinas = re.Key.Valores.Skip(1).TakeWhile(x => x is int).ToArray();

                    if (usinas.First() == 139)
                    {

                    }

                    if (!usinas.All(x => dadger.BlocoUh.Any(y => y.Usina == x))) continue;

                    var rheEqu =
                        dadger.BlocoRhe.RheGrouped.Where(rhe =>
                        {

                            var fus = rhe.Value.Where(x => x is FuLine);
                            var pred = fus.Count() == usinas.Length;
                            pred = pred && rhe.Value.Where(x => x is FuLine).All(x => usinas.Contains(((FuLine)x).Usina));
                            pred = pred && !rhe.Value.Any(x => !(x is FuLine || x is ReLine || x is LuLine));
                            return pred;
                        }).Select(x => x.Value)
                        .FirstOrDefault();

                    LuLine lu;
                    if (rheEqu != null)
                    {
                        lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                        LuLine luAnt = (LuLine)rheEqu.Where(x => x is LuLine && ((LuLine)x).Estagio <= dt.Item2).OrderByDescending(x => ((LuLine)x).Estagio).FirstOrDefault();

                        if (lu == null)
                        {
                            lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                            if (luAnt != null)
                            {
                                lu[3] = luAnt[3];
                                lu[5] = luAnt[5];
                                lu[7] = luAnt[7];
                            }
                            dadger.BlocoRhe.Add(lu);
                        }
                        //}
                        //else
                        //{
                        //    var rheID = dadger.BlocoRhe.GetNextId();

                        //    dadger.BlocoRhe.Add(new ReLine() { Inicio = dt.Item2, Fim = mesOperativo.Estagios + 1, Restricao = rheID, Comment = "&&FROM RE.DAT" });
                        //    lu = new LuLine() { Estagio = dt.Item2, Restricao = rheID };
                        //    dadger.BlocoRhe.Add(lu);

                        //    foreach (var uhe in usinas)
                        //    {
                        //        var fu = new FuLine() { Restricao = rheID, Usina = uhe, Estagio = dt.Item2, Fator = 1 };
                        //        dadger.BlocoRhe.Add(fu);
                        //    }
                        //}

                        if (re.Value.Count() == 1)
                        {
                            lu[4] = lu[6] = lu[8] = re.Value.First().ValorRestricao;
                        }
                        else
                        {
                            lu[4] = re.Value.First().ValorRestricao;
                            lu[6] = re.Value.Skip(1).First().ValorRestricao;
                            lu[8] = re.Value.Skip(2).First().ValorRestricao;
                        }
                    }
                }

                var sisNome = new Func<int, string>(s =>
                {
                    if (s == 1) return "SE";
                    else if (s == 2) return "S";
                    else if (s == 3) return "NE";
                    else if (s == 4) return "N";
                    else if (s == 11) return "FC";
                    else return "";
                });

                //agrintDat
                var agrintDat = deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.agrint].Document as CommomLibrary.AgrintDat.AgrintDat;
                foreach (var re in agrintDat[dt.Item1])
                {
                    var t2 = string.Join(";",
                                        re.Key.Select(y => new { De = sisNome(y.SistemaA), Para = sisNome(y.SistemaB) }).OrderBy(x => x.De).ThenBy(x => x.Para)
                                        .Select(x => x.De + "-" + x.Para)
                                        );

                    var rheEqu =
                        dadger.BlocoRhe.RheGrouped.Where(rhe =>
                        {
                            var pred = rhe.Value.Select(x => x is FiLine).Count() > 1;
                            pred = pred && !rhe.Value.Any(x => !(x is FiLine || x is ReLine || x is LuLine));

                            if (pred)
                            {
                                var t1 = string.Join(";",
                                        rhe.Value.Where(x => x is FiLine).Select(x => (FiLine)x).OrderBy(x => x.De).ThenBy(x => x.Para)
                                        .Select(x => x.De + "-" + x.Para)
                                        );

                                pred = pred && t1 == t2;
                            }

                            return pred;
                        }).Select(x => x.Value)
                        .FirstOrDefault();

                    LuLine lu;
                    if (rheEqu != null)
                    {
                        lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                        if (lu == null)
                        {
                            lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                            dadger.BlocoRhe.Add(lu);
                        }

                        lu[4] = re.Value.Lim_P1;
                        lu[6] = re.Value.Lim_P2;
                        lu[8] = re.Value.Lim_P3;
                    }
                }


                //blocoIA
                foreach (var ia in dadger.BlocoIa.Where(x => x.Estagio == dt.Item2))
                {
                    var rheEqu =
                            dadger.BlocoRhe.RheGrouped.Where(rhe =>
                            {
                                var pred = rhe.Value.Where(x => x is FiLine).All(x => ((FiLine)x).De == ia.SistemaA && ((FiLine)x).Para == ia.SistemaB);
                                pred = pred && !rhe.Value.Any(x => !(x is FiLine || x is ReLine || x is LuLine));
                                return pred;
                            }).Select(x => x.Value)
                            .FirstOrDefault();
                    LuLine lu;
                    if (rheEqu != null)
                    {
                        lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                        if (lu == null)
                        {
                            lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                            dadger.BlocoRhe.Add(lu);
                        }
                        lu[4] = ia.Pat1_AB;
                        lu[6] = ia.Pat2_AB;
                        lu[8] = ia.Pat3_AB;
                    }
                    rheEqu =
                            dadger.BlocoRhe.RheGrouped.Where(rhe =>
                            {
                                var pred = rhe.Value.Where(x => x is FiLine).All(x => ((FiLine)x).De == ia.SistemaB && ((FiLine)x).Para == ia.SistemaA);
                                pred = pred && !rhe.Value.Any(x => !(x is FiLine || x is ReLine || x is LuLine));
                                return pred;
                            }).Select(x => x.Value)
                            .FirstOrDefault();
                    if (rheEqu != null)
                    {
                        lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                        if (lu == null)
                        {
                            lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                            dadger.BlocoRhe.Add(lu);
                        }
                        lu[4] = ia.Pat1_BA;
                        lu[6] = ia.Pat2_BA;
                        lu[8] = ia.Pat3_BA;
                    }


                }


                //blocoIT
                //re uh 66
                {
                    var it = dadger.BlocoIt.FirstOrDefault(x => x.Estagio == dt.Item2);
                    if (it != null)
                    {
                        var rheEqu =
                                dadger.BlocoRhe.RheGrouped.Where(rhe =>
                                {
                                    var pred = rhe.Value.Where(x => x is FuLine).All(x => ((FuLine)x).Usina == 66);
                                    pred = pred && !rhe.Value.Any(x => !(x is FuLine || x is ReLine || x is LuLine));
                                    return pred;
                                }).Select(x => x.Value)
                                .FirstOrDefault();


                        LuLine lu;
                        if (rheEqu != null)
                        {
                            lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                            if (lu == null)
                            {
                                lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                                dadger.BlocoRhe.Add(lu);
                            }

                            var potContr = (dt.Item2 == 1 ? 10815d : 12600d);
                            var potDisp = 14000d * (dt.Item2 == 1 ? 0.95d : 1d);



                            lu[3] = it.Geracao_Pat1 + 1680;
                            lu[5] = it.Geracao_Pat2 + 1680;
                            lu[7] = it.Geracao_Pat3 + 1680;

                            lu[4] = Math.Min(it.AndePat1 + potContr, potDisp);
                            lu[6] = Math.Min(it.AndePat2 + potContr, potDisp);
                            lu[8] = Math.Min(it.AndePat3 + potContr, potDisp);

                        }
                    }


                    var ri = dadger.BlocoRi.FirstOrDefault(x => x.Estagio == dt.Item2);
                    if (ri != null)
                    {
                        {//re461
                            var rheEqu =
                                    dadger.BlocoRhe.RheGrouped.Where(rhe =>
                                    {
                                        var pred = rhe.Value.Where(x => x is FuLine).All(x => ((FuLine)x).Usina == 66 && x[5] == 50);
                                        pred = pred && !rhe.Value.Any(x => !(x is FuLine || x is ReLine || x is LuLine));
                                        return pred;
                                    }).Select(x => x.Value)
                                    .FirstOrDefault();


                            LuLine lu;
                            if (rheEqu != null)
                            {
                                lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                                if (lu == null)
                                {
                                    lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                                    dadger.BlocoRhe.Add(lu);
                                }

                                var potContr = (dt.Item2 == 1 ? 10815d : 12600d);
                                var potDisp = 14000d * (dt.Item2 == 1 ? 0.95d : 1d);

                                //ri[6] = ri[8] + 1900;  //
                                //ri[11] = ri[13] + 1900;// GMIN50Hz = carga ande + 1900
                                //ri[16] = ri[18] + 1900;//

                                lu[3] = ri[6];
                                lu[5] = ri[11];
                                lu[7] = ri[16];

                                //lu[4] = Math.Min(lu[3] + 1600, 7000);//GMAX =  Minimo (Carga ANDE + Consumo Interno + Limite Elo(1600); 7000)
                                //lu[6] = Math.Min(lu[5] + 1600, 7000);
                                //lu[8] = Math.Min(lu[7] + 1600, 7000);

                                lu[4] = 6500;
                                lu[6] = 6500;
                                lu[8] = 6500;
                                //lu[4] = lu[6] = lu[8] = 7000;
                            }
                        }

                        {//re462
                            var rheEqu2 =
                                   dadger.BlocoRhe.RheGrouped.Where(rhe =>
                                   {
                                       var pred = rhe.Value.Where(x => x is FuLine).All(x => ((FuLine)x).Usina == 66 && x[5] == 60);
                                       pred = pred && !rhe.Value.Any(x => !(x is FuLine || x is ReLine || x is LuLine));
                                       return pred;
                                   }).Select(x => x.Value)
                                   .FirstOrDefault();


                            LuLine lu2;
                            if (rheEqu2 != null)
                            {
                                lu2 = (LuLine)rheEqu2.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                                if (lu2 == null)
                                {
                                    lu2 = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu2.First().Restricao };
                                    dadger.BlocoRhe.Add(lu2);
                                }

                                var potContr = (dt.Item2 == 1 ? 10815d : 12600d);
                                var potDisp = 14000d * (dt.Item2 == 1 ? 0.95d : 1d);

                                //ri[6] = ri[8] + 1900;  //
                                //ri[11] = ri[13] + 1900;// GMIN50Hz = carga ande + 1900
                                //ri[16] = ri[18] + 1900;//

                                lu2[3] = ri[4];
                                lu2[5] = ri[9];
                                lu2[7] = ri[14];

                                //lu2[4] = Math.Min(lu2[3] + 1600, 7000);//GMAX =  Minimo (Carga ANDE + Consumo Interno + Limite Elo(1600); 7000)
                                //lu2[6] = Math.Min(lu2[5] + 1600, 7000);
                                //lu2[8] = Math.Min(lu2[7] + 1600, 7000);

                                lu2[4] = 7000;
                                lu2[6] = 7000;
                                lu2[8] = 7000;
                                //lu[4] = lu[6] = lu[8] = 7000;
                            }

                        }
                    }
                }

                //peixe angical + lajeado
                {

                    /*
                    RE  405   1    6
                    LU  405   1                   4100                4100                3900
                    FU  405   1   257           1
                    FU  405   1   261           1
                    FI  405   1   FC   SE            1
                     */

                    var intercambio = sistemaDat.Intercambio.Where(x => x.SubmercadoA == 11 && x.SubmercadoB == 1 && x.Ano == dt.Item1.Year).First()[dt.Item1.Month];
                    var patsFCNE = patamarDat.Intercambio.Where(x => x.Ano == dt.Item1.Year && x.SubmercadoA == 11 && x.SubmercadoB == 1).ToList();

                    var usinas = "257;261;FC-SE".Split(';');

                    var rheEqu =
                        dadger.BlocoRhe.RheGrouped.Where(rhe =>
                        {
                            var fus = rhe.Value.Where(x => x is FuLine || x is FiLine);
                            var pred = fus.Count() == usinas.Length;

                            pred = pred && fus.All(x => usinas.Contains(
                               x is FuLine ? ((FuLine)x).Usina.ToString() : ((FiLine)x).De + "-" + ((FiLine)x).Para)
                                );
                            return pred;
                        }).Select(x => x.Value)
                        .FirstOrDefault();

                    LuLine lu;
                    if (rheEqu != null)
                    {
                        lu = (LuLine)rheEqu.FirstOrDefault(x => x is LuLine && ((LuLine)x).Estagio == dt.Item2);

                        if (lu == null)
                        {
                            lu = new LuLine() { Estagio = dt.Item2, Restricao = rheEqu.First().Restricao };
                            dadger.BlocoRhe.Add(lu);
                        }

                        var px_laj = dt.Item1.Month >= 5 && dt.Item1.Month <= 10 ? 542 : 949;

                        lu[4] = intercambio * patsFCNE[0][dt.Item1.Month] + px_laj;
                        lu[6] = intercambio * patsFCNE[1][dt.Item1.Month] + px_laj;
                        lu[8] = intercambio * patsFCNE[2][dt.Item1.Month] + px_laj;
                    }




                }



            }
        }

        private static List<CtLine> trataManutt(List<CommomLibrary.ManuttDat.ManuttLine> lstManutt, MesOperativo dtAtual, double infl, double inflMais, double pot, double potMais, double fcmx, double fcmxMais, double cvu, CommomLibrary.ConftDat.ConftLine term)
        {
            int nDiasMesSeg = dtAtual.SemanasOperativas.Select(x => (int)(x.Fim - x.Inicio).TotalDays).Last();
            DateTime ultimoDiaRelevante = dtAtual.SemanasOperativas.Last().Fim;

            double[] valSemanal = new double[dtAtual.Estagios + 1];
            double[] fcSemanal = new double[dtAtual.Estagios + 1];


            List<CtLine> lstCTM = new List<CtLine>();

            for (int i = 0; i < dtAtual.Estagios; i++)
            {
                valSemanal[i] = pot;
                fcSemanal[i] = fcmx;
            }
            valSemanal[dtAtual.Estagios] = potMais;
            fcSemanal[dtAtual.Estagios] = fcmxMais;

            foreach (var nextM in lstManutt)
            {
                var mTemp = nextM.Clone() as CommomLibrary.ManuttDat.ManuttLine;

                int e = 0;
                foreach (var est in dtAtual.SemanasOperativas)
                {
                    if (mTemp.DataInicio <= est.Fim && mTemp.DataFim >= est.Inicio)
                    {
                        var ini = mTemp.DataInicio > est.Inicio ? mTemp.DataInicio : est.Inicio;
                        var fim = mTemp.DataFim > est.Fim ? est.Fim : mTemp.DataFim;
                        var dias = (int)(fim - ini).TotalDays + 1;

                        valSemanal[e] -= mTemp.Potencia * dias / ((est.Fim - est.Inicio).TotalDays + 1);
                    }

                    e++;
                }
            }
            {
                int e = 0;
                foreach (var est in dtAtual.SemanasOperativas)
                {
                    double infl1 = (e == dtAtual.Estagios) ? inflMais : infl;

                    if (e == 0 || valSemanal[e] != valSemanal[e - 1] || infl1 != infl)
                    {
                        CtLine ct = new CtLine();
                        ct[1] = term.Num;
                        ct[2] = term.Sistema;
                        ct[3] = term.Nome;
                        ct[4] = e + 1;


                        ct[6] = ct[9] = ct[12] = (valSemanal[e] * fcSemanal[e] / 100d);
                        ct[7] = ct[10] = ct[13] = cvu;

                        ct[5] = ct[8] = ct[11] = Math.Min(infl1, ct[6]);

                        lstCTM.Add(ct);
                    }

                    e++;
                }

            }
            return lstCTM;
        }

        private static void trataCarga(MesOperativo dtAtual, Dadger dadger, CommomLibrary.Newave.Deck deckNWEstudo, CommomLibrary.Pmo.Pmo pmoBase, bool pees = false, List<Tuple<int, int, DateTime, double>> eolicasDados = null)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var c_adicA = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.cadic].Document as Compass.CommomLibrary.C_AdicDat.C_AdicDat).Adicao
               .Where(x => x is Compass.CommomLibrary.C_AdicDat.MerEneLine)
               .Cast<Compass.CommomLibrary.C_AdicDat.MerEneLine>();


            var patamares = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as CommomLibrary.PatamarDat.PatamarDat);

            var sistema = (deckNWEstudo[CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as CommomLibrary.SistemaDat.SistemaDat);



            dadger.BlocoPq.Clear();


            var possuiIt = dadger.BlocoIt.Count() > 0;


            dadger.BlocoIt.Clear();

            //var fcPu = dadger.BlocoRi.Where(x => x.Estagio == 1).ToList();
            //double fcPu1 = fcPu[0].AndePat1;
            //double fcPu2 = fcPu[0].AndePat2;
            //double fcPu3 = fcPu[0].AndePat3;
            dadger.BlocoRi.Clear();


            dadger.BlocoDp.Clear();

            var mercados = "SUDESTE SUL NORDESTE NORTE".Split();

            var cargaMedia = pmoBase.Blocos["MERCADO"].Where(x => x[2] == dtAtual.Ano).Select(x => new { mercado = x[0], carga = x[dtAtual.Mes.ToString()] });
            var cargaMediaNext = pmoBase.Blocos["MERCADO"].Where(x => x[2] == dtAtual.SemanasOperativas.Last().Fim.Year).Select(x => new { mercado = x[0], carga = x[dtAtual.SemanasOperativas.Last().Fim.Month.ToString()] });

            var c_adic_Soma = c_adicA
                         .Where(x => x.Ano == dtAtual.Ano.ToString() && (!x.Descricao.ToUpper().Contains("ITAIPU") && !x.Descricao.ToUpper().Contains("ANDE")))
                         .Select(x => new { mercado = x.Mercado, carga = (double)x[dtAtual.Mes.ToString()] }).ToList();
            var c_adic_SomaNext = c_adicA
                .Where(x => x.Ano == dtAtual.SemanasOperativas.Last().Fim.Year.ToString() && (!x.Descricao.ToUpper().Contains("ITAIPU") && !x.Descricao.ToUpper().Contains("ANDE")))
                .Select(x => new { mercado = x.Mercado, carga = (double)x[dtAtual.SemanasOperativas.Last().Fim.Month.ToString()] }).ToList();

            var cargasMediasSemanais = new double[4, dtAtual.Estagios + 1];

            for (int i = 0; i < 4; i++)
            {
                double adic = 0;
                double adicNext = 0;
                if (c_adic_Soma.Count() > 0)
                {
                    adic = c_adic_Soma.Where(x => x.mercado == (i + 1) && x.carga > 0.0)
                                .Sum(x => x.carga);
                }
                if (c_adic_SomaNext.Count() > 0)
                {
                    adicNext = c_adic_SomaNext.Where(x => x.mercado == (i + 1) && x.carga > 0.0)
                                .Sum(x => x.carga);
                }

                var c1 = cargaMedia.First(x => x.mercado == mercados[i]).carga;
                var c2 = cargaMediaNext.First(x => x.mercado == mercados[i]).carga;

                c1 += adic;
                c2 += adicNext;

                var b = (c2 - c1) * 2 / (dtAtual.Estagios + 1);
                var a = 2 * c1 - c2;

                for (int est = 0; est < dtAtual.Estagios + 1; est++)
                {
                    cargasMediasSemanais[i, est] = a + b * (est + 1);
                }
            }

            //correcao feriados
            var mediaHoras = dtAtual.SemanasOperativas.Take(dtAtual.Estagios).GroupBy(x => 1).Select(
                x => new
                {
                    P1 = x.Average(y => y.HorasPat1),
                    P2 = x.Average(y => y.HorasPat2),
                    P3 = x.Average(y => y.HorasPat3)
                }
                ).First();

            var fatorFeriado = new double[dtAtual.Estagios];
            for (int est = 0; est < dtAtual.Estagios; est++)
            {
                fatorFeriado[est] = (dtAtual.SemanasOperativas[est].HorasPat1 / mediaHoras.P1 + dtAtual.SemanasOperativas[est].HorasPat2 / mediaHoras.P2 + dtAtual.SemanasOperativas[est].HorasPat3 / mediaHoras.P3) / 3d;
            }

            var normalizacao = (double)fatorFeriado.Length / fatorFeriado.Sum();

            for (int i = 0; i < 4; i++)
            {
                for (int est = 0; est < dtAtual.Estagios; est++)
                {
                    cargasMediasSemanais[i, est] = cargasMediasSemanais[i, est] * normalizacao * fatorFeriado[est];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                cargasMediasSemanais[i, dtAtual.Estagios] = cargaMediaNext.First(x => x.mercado == mercados[i]).carga + c_adic_SomaNext.Where(x => x.mercado == (i + 1) && x.carga > 0.0).Sum(x => x.carga);
            }


            double[] fc1;
            double[] fc2;
            double[] fc3;
            var dist_carga = patamares.Carga
                      .Where(x => x is Compass.CommomLibrary.PatamarDat.CargaEneLine)
                      .Cast<Compass.CommomLibrary.PatamarDat.CargaEneLine>()
                      .Where(x => x.Ano == dtAtual.Ano)
                      //.Where(x => x.Mercado == numMercado)
                      .OrderBy(x => x.Patamar)
                      .Select(x => new { x.Patamar, fc = x[dtAtual.Mes.ToString()], x.Mercado });

            fc1 = dist_carga.Where(x => x.Patamar == 1).OrderBy(x => x.Mercado).Select(x => (double)x.fc).ToArray();
            fc2 = dist_carga.Where(x => x.Patamar == 2).OrderBy(x => x.Mercado).Select(x => (double)x.fc).ToArray();
            fc3 = dist_carga.Where(x => x.Patamar == 3).OrderBy(x => x.Mercado).Select(x => (double)x.fc).ToArray();

            var c_adic = c_adicA
                .Where(x => x.Ano == dtAtual.Ano.ToString())
                .Select(x => new { mercado = x.Mercado, carga = (double)x[dtAtual.Mes.ToString()] }).ToList();

            var c_adic_RI = c_adicA
                .Where(x => x.Ano == dtAtual.Ano.ToString() && (x.Descricao.ToUpper().Contains("ITAIPU") || x.Descricao.ToUpper().Contains("ANDE")))
                .Select(x => new { mercado = x.Mercado, carga = (double)x[dtAtual.Mes.ToString()] }).ToList();

            var c_adic_ANDE = c_adicA
                .Where(x => x.Ano == dtAtual.Ano.ToString() && x.Descricao.ToUpper().Contains("ANDE"))
                .Select(x => new { mercado = x.Mercado, carga = (double)x[dtAtual.Mes.ToString()] }).ToList();

            var pequenas = pmoBase.Blocos["PEQUENAS"].Where(x => x[2] == dtAtual.Ano)
                .Select(x => new { mercado = x[0], carga = x[dtAtual.Mes.ToString()] });





            var e = 0;
            foreach (var estagio in dtAtual.SemanasOperativas/*.Take(dtAtual.Estagios)*/)
            {
                if (estagio == dtAtual.SemanasOperativas.Last())
                {
                    dist_carga = patamares.Carga
                      .Where(x => x is Compass.CommomLibrary.PatamarDat.CargaEneLine)
                      .Cast<Compass.CommomLibrary.PatamarDat.CargaEneLine>()
                      .Where(x => x.Ano == estagio.Inicio.Year)
                      //.Where(x => x.Mercado == numMercado)
                      .OrderBy(x => x.Patamar)
                      .Select(x => new { x.Patamar, fc = x[estagio.Inicio.Month.ToString()], x.Mercado });

                    fc1 = dist_carga.Where(x => x.Patamar == 1).OrderBy(x => x.Mercado).Select(x => (double)x.fc).ToArray();
                    fc2 = dist_carga.Where(x => x.Patamar == 2).OrderBy(x => x.Mercado).Select(x => (double)x.fc).ToArray();
                    fc3 = dist_carga.Where(x => x.Patamar == 3).OrderBy(x => x.Mercado).Select(x => (double)x.fc).ToArray();

                    c_adic = c_adicA
                        .Where(x => x.Ano == estagio.Inicio.Year.ToString())
                        .Select(x => new { mercado = x.Mercado, carga = (double)x[estagio.Inicio.Month.ToString()] }).ToList();

                    c_adic_RI = c_adicA
                        .Where(x => x.Ano == estagio.Inicio.Year.ToString() && (x.Descricao.ToUpper().Contains("ITAIPU") || x.Descricao.ToUpper().Contains("ANDE")))
                        .Select(x => new { mercado = x.Mercado, carga = (double)x[estagio.Inicio.Month.ToString()] }).ToList();

                    c_adic_ANDE = c_adicA
                        .Where(x => x.Ano == estagio.Inicio.Year.ToString() && x.Descricao.ToUpper().Contains("ANDE"))
                        .Select(x => new { mercado = x.Mercado, carga = (double)x[estagio.Inicio.Month.ToString()] }).ToList();

                    pequenas = pmoBase.Blocos["PEQUENAS"].Where(x => x[2] == estagio.Inicio.Year)
                        .Select(x => new { mercado = x[0], carga = x[estagio.Inicio.Month.ToString()] });
                }

                DpLine lTemp = new DpLine();

                lTemp[1] = e + 1;
                lTemp[3] = 3;
                lTemp[5] = estagio.HorasPat1;
                lTemp[7] = estagio.HorasPat2;
                lTemp[9] = estagio.HorasPat3;

                var numMercado = 0;
                foreach (var m in mercados)
                {
                    bool zerarEol = false;
                    bool usarPlanEol = false;
                    bool adicionado = false;

                    if ((m == "SUL" || m == "NORDESTE") && estagio == dtAtual.SemanasOperativas.Last() && pees == true)
                    {
                        zerarEol = true;
                    }

                    if ((m == "SUL" || m == "NORDESTE") && estagio != dtAtual.SemanasOperativas.Last() && pees == true)
                    {
                        usarPlanEol = true;
                    }

                    var nLine = lTemp.Clone();
                    nLine[2] = numMercado + 1;
                    nLine[4] = cargasMediasSemanais[numMercado, e] * fc1[numMercado];
                    nLine[6] = cargasMediasSemanais[numMercado, e] * fc2[numMercado];
                    nLine[8] = cargasMediasSemanais[numMercado, e] * fc3[numMercado];

                    dadger.BlocoDp.Add(nLine);

                    if (estagio == dtAtual.SemanasOperativas.First() || estagio == dtAtual.SemanasOperativas.Last())
                    {
                        if (numMercado == 0 && possuiIt)
                        {
                            var itTemp = new ItLine();
                            double itCarga = c_adic.Where(x => x.mercado == (numMercado + 1) && x.carga > 0.0)
                                .Sum(x => x.carga);

                            itTemp[1] = e + 1;
                            itTemp[2] = 66;
                            itTemp[3] = 1;
                            //itTemp[5] = fc1[numMercado] * itCarga;
                            //itTemp[7] = fc2[numMercado] * itCarga;
                            //itTemp[9] = fc3[numMercado] * itCarga;

                            itTemp[5] = 1.4378 * itCarga;
                            itTemp[7] = 1.1388 * itCarga;
                            itTemp[9] = 0.6996 * itCarga;


                            itTemp[4] = itTemp[5] + 1900;
                            itTemp[6] = itTemp[7] + 1900;
                            itTemp[8] = itTemp[9] + 1900;

                            dadger.BlocoIt.Add(itTemp);
                        }
                        else if (numMercado == 0 && !possuiIt)
                        {
                            var riTemp = new RiLine();
                            double riCarga = c_adic.Where(x => x.mercado == (numMercado + 1) && x.carga > 0.0)
                                .Sum(x => x.carga);

                            double riCargaNew = c_adic_RI.Where(x => x.mercado == (numMercado + 1) && x.carga > 0.0)
                               .Sum(x => x.carga);
                            riCarga = riCargaNew;

                            double riCargaANDE = c_adic_ANDE.Where(x => x.mercado == (numMercado + 1) && x.carga > 0.0)
                               .Sum(x => x.carga);

                            riTemp[1] = 66;
                            riTemp[2] = e + 1;
                            riTemp[3] = 1;

                            // MIN60 antigo                           
                            //riTemp[4] = riTemp[9] = riTemp[14] = 1680;

                            //MIN60 novo
                            riTemp[4] = 2000;   //riTemp[4] = (riCarga + 1900) / 2;
                            riTemp[9] = 2000;   //riTemp[9] = (riCarga + 1900) / 2;
                            riTemp[14] = 2000;   //riTemp[14] = (riCarga + 1900) / 2;

                            // MAX60
                            riTemp[5] = riTemp[10] = riTemp[15] = 7000;

                            //ANDE
                            //var puPat1 = fcPu1 / riCarga;
                            //var puPat2 = fcPu2 / riCarga;
                            //var puPat3= fcPu3 / riCarga;

                            //riTemp[8] = puPat1 * riCarga;     
                            //riTemp[13] = puPat2 * riCarga;     
                            //riTemp[18] = puPat3 * riCarga;


                            //ande
                            riTemp[8] = fc1[numMercado] * riCargaANDE;  //riTemp[8] = 1.4378 * riCarga;     //fc1[numMercado] * riCarga; // fc1 deveria ser em jan/2020 = 1.4378
                            riTemp[13] = fc2[numMercado] * riCargaANDE;// riTemp[13] = 1.1388 * riCarga;       //fc2[numMercado] * riCarga; // 1.1388
                            riTemp[18] = fc3[numMercado] * riCargaANDE; //riTemp[18] = 0.7118 * riCarga;     //fc3[numMercado] * riCarga; // 0.6696

                            //riTemp[8] = 1.4378 * riCarga; 
                            //riTemp[13] = 1.1388 * riCarga;
                            //riTemp[18] = 0.7118 * riCarga;


                            //MIN50 antigo
                            //riTemp[6] = riTemp[8] + 1900;
                            // riTemp[11] = riTemp[13] + 1900;
                            // riTemp[16] = riTemp[18] + 1900;

                            //MIN50 novo

                            var riMin1 = (riCarga + (4 * 78.4)) * fc1[numMercado];
                            var riMin2 = (riCarga + (4 * 78.4)) * fc2[numMercado];
                            var riMin3 = (riCarga + (4 * 78.4)) * fc3[numMercado];

                            riTemp[6] = riMin1 > 2500 ? riMin1 : 2500;      //riTemp[6] = (riCarga + 1900) / 2;
                            riTemp[11] = riMin2 > 2500 ? riMin2 : 2500;      //riTemp[11] = (riCarga + 1900) / 2;
                            riTemp[16] = riMin3 > 2500 ? riMin3 : 2500;     //riTemp[16] = (riCarga + 1900) / 2;

                            //MAX50
                            riTemp[7] = riTemp[12] = riTemp[17] = 6500;


                            dadger.BlocoRi.Add(riTemp);
                        }

                        var Verifica_Peq_Pat = patamares.Nao_Simuladas.Where(x => x.Ano == dtAtual.Ano).ToList();

                        if (Verifica_Peq_Pat.Count() > 0)
                        {
                            double p1 = 0;
                            double p2 = 0;
                            double p3 = 0;
                            List<string> tipoUsinas = new List<string> { "", "_PCH", "_PCT", "_EOL", "_UFV", "_PCHgd", "_PCTgd", "_EOLgd", "_UFVgd" };//não alterar essa ordem 
                            List<string> submercadoAbrev = new List<string> { "","SECO","SUL","NE","N" };//não alterar essa ordem 

                            int numUsinas = patamares.Nao_Simuladas.Where(x => x.Submercado == numMercado + 1).Select(x => x.Tipo_Usina).Max();///

                            for (int i = 1; i <= numUsinas; i++) //for (int i = 1; i <= 4; i++)//TODO: logica de calcular e colocar separadamente cada tipo de usina 
                            {
                                var Mes = dtAtual.Mes;
                                var Ano = dtAtual.Ano;
                                if (estagio == dtAtual.SemanasOperativas.Last())
                                {
                                    Mes = dtAtual.MesSeguinte;
                                    Ano = dtAtual.AnoSeguinte;

                                }

                                double Valor_Sis = 0;

                                var item = sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.PeqEneLine).ToList();

                                foreach (var dado in item)
                                {
                                    if (dado.Ano == Ano && dado.Tipo_Usina == i && dado.Mercado == (numMercado + 1))
                                    {
                                        Valor_Sis = dado[Convert.ToInt32(Mes)];
                                        if (zerarEol && dado.Desc_Usina.ToUpper().Contains("EOL"))
                                        {
                                            Valor_Sis = 0;
                                        }
                                    }
                                }

                                var Peq_Pat = patamares.Nao_Simuladas.Where(x => x.Ano == Ano && x.Submercado == numMercado + 1 && x.Tipo_Usina == i).ToList();




                                var Pq1 = Peq_Pat.Where(x => x.Patamar == 1).Select(x => x.Valores[Mes]).FirstOrDefault();
                                var Pq2 = Peq_Pat.Where(x => x.Patamar == 2).Select(x => x.Valores[Mes]).FirstOrDefault();
                                var Pq3 = Peq_Pat.Where(x => x.Patamar == 3).Select(x => x.Valores[Mes]).FirstOrDefault();

                                p1 = p1 + Valor_Sis * Convert.ToDouble(Pq1);
                                p2 = p2 + Valor_Sis * Convert.ToDouble(Pq2);
                                p3 = p3 + Valor_Sis * Convert.ToDouble(Pq3);

                                if (usarPlanEol && adicionado == false)
                                {
                                    var eolP1 = eolicasDados.Where(x => x.Item1 == numMercado + 1 && x.Item2 == 1 && x.Item3.Month == Mes && x.Item3.Year == Ano).Select(x => x.Item4).FirstOrDefault();
                                    var eolP2 = eolicasDados.Where(x => x.Item1 == numMercado + 1 && x.Item2 == 2 && x.Item3.Month == Mes && x.Item3.Year == Ano).Select(x => x.Item4).FirstOrDefault();
                                    var eolP3 = eolicasDados.Where(x => x.Item1 == numMercado + 1 && x.Item2 == 3 && x.Item3.Month == Mes && x.Item3.Year == Ano).Select(x => x.Item4).FirstOrDefault();

                                    p1 = p1 + eolP1;
                                    p2 = p2 + eolP2;
                                    p3 = p3 + eolP3;

                                    //adicionado = true;
                                }
                                var pLine = new PqLine();
                                pLine[1] = submercadoAbrev[numMercado + 1] + tipoUsinas[i];
                                pLine[2] = numMercado + 1;
                                pLine[3] = e + 1;


                                pLine[4] = Math.Round(p1, 0);
                                pLine[5] = Math.Round(p2, 0);
                                pLine[6] = Math.Round(p3, 0);

                                dadger.BlocoPq.Add(pLine.Clone());
                                p1 = 0;
                                p2 = 0;
                                p3 = 0;
                            }

                            //var pLine = new PqLine();
                            //pLine[1] = m;
                            //pLine[2] = numMercado + 1;
                            //pLine[3] = e + 1;


                            //pLine[4] = Math.Round(p1, 0);
                            //pLine[5] = Math.Round(p2, 0);
                            //pLine[6] = Math.Round(p3, 0);

                            //dadger.BlocoPq.Add(pLine.Clone());
                        }
                        else
                        {


                            var p = pequenas.Where(x => x.mercado == m).Select(x => x.carga).ToArray();

                            var pLine = new PqLine();
                            pLine[1] = m;
                            pLine[2] = numMercado + 1;
                            pLine[3] = e + 1;
                            pLine[4] = p[0];
                            pLine[5] = p[1];
                            pLine[6] = p[2];

                            dadger.BlocoPq.Add(pLine.Clone());

                            if (c_adic.Any(x => x.mercado == (numMercado + 1) && x.carga < 0.0))
                            {

                                var add = -1.0 * c_adic.Where(x => x.mercado == (numMercado + 1) && x.carga < 0.0).Sum(x => x.carga);

                                pLine[1] = "C_ADIC";
                                pLine[4] = add * fc1[numMercado];
                                pLine[5] = add * fc2[numMercado];
                                pLine[6] = add * fc3[numMercado];

                                dadger.BlocoPq.Add(pLine);
                            }
                        }






                    }
                    numMercado++;
                }

                var fict = lTemp.Clone();
                fict[2] = 11;
                dadger.BlocoDp.Add(fict);
                e++;
            }

            #region ajuste de distribuição blocoDp
            if (dtAtual.Estagios > 1)// se > 1 é decomp semanal
            {
                //Primeiro mês
                for (int i = 1; i <= 4; i++)
                {
                    double cargasDps = 0;
                    int diasTotal = 0;
                    for (int est = 1; est <= dtAtual.Estagios; est++)
                    {
                        int diasSemMes = 0;
                        DateTime iniSemana = dtAtual.SemanasOperativas[est - 1].Inicio;
                        DateTime fimSemana = dtAtual.SemanasOperativas[est - 1].Fim;
                        for (DateTime dat = iniSemana; dat <= fimSemana; dat = dat.AddDays(1))
                        {
                            if (dat.Month == dtAtual.Mes)
                            {
                                diasSemMes++;
                            }
                        }

                        var dps = dadger.BlocoDp.Where(x => x[2] == i && x[1] == est).First();
                        double mediaCarga = (dps[4] * dps[5] + dps[6] * dps[7] + dps[8] * dps[9]) / (dps[5] + dps[7] + dps[9]);
                        mediaCarga = mediaCarga * diasSemMes;
                        cargasDps += mediaCarga;
                        diasTotal += diasSemMes;
                    }
                    var cargaMediaTotal = cargasDps / diasTotal;
                    var cargaSistema = sistema.Mercado.Where(x => x.Ano == dtAtual.Ano && x.Mercado == i).Select(x => x[dtAtual.Mes]).First() + c_adic_Soma.Where(x => x.mercado == (i) && x.carga > 0.0).Sum(x => x.carga);
                    var correcao = cargaSistema / cargaMediaTotal;
                    var dpCorrecao = dadger.BlocoDp.Where(x => x[2] == i && x[1] <= dtAtual.Estagios).ToList();

                    foreach (var dp in dpCorrecao)
                    {
                        dp[4] = dp[4] * correcao;
                        dp[6] = dp[6] * correcao;
                        dp[8] = dp[8] * correcao;
                    }

                }

                //Segundo mês (ultimo estágio)
                for (int i = 1; i <= 4; i++)
                {
                    int ultimoEstagio = dtAtual.Estagios + 1;

                    var dps = dadger.BlocoDp.Where(x => x[2] == i && x[1] == ultimoEstagio).First();

                    double mediaCarga = (dps[4] * dps[5] + dps[6] * dps[7] + dps[8] * dps[9]) / (dps[5] + dps[7] + dps[9]);

                    var cargaSistema = sistema.Mercado.Where(x => x.Ano == dtAtual.AnoSeguinte && x.Mercado == i).Select(x => x[dtAtual.MesSeguinte]).First() + c_adic_SomaNext.Where(x => x.mercado == (i) && x.carga > 0.0).Sum(x => x.carga);
                    var correcao = cargaSistema / mediaCarga;

                    dps[4] = dps[4] * correcao;
                    dps[6] = dps[6] * correcao;
                    dps[8] = dps[8] * correcao;

                }
                #endregion

            }
            else// decomp mensal
            {
                //primeiro estagio
                for (int i = 1; i <= 4; i++)
                {

                    var dps = dadger.BlocoDp.Where(x => x[2] == i && x[1] == dtAtual.Estagios).First();

                    double mediaCarga = (dps[4] * dps[5] + dps[6] * dps[7] + dps[8] * dps[9]) / (dps[5] + dps[7] + dps[9]);

                    var cargaSistema = sistema.Mercado.Where(x => x.Ano == dtAtual.Ano && x.Mercado == i).Select(x => x[dtAtual.Mes]).First() + c_adic_Soma.Where(x => x.mercado == (i) && x.carga > 0.0).Sum(x => x.carga);
                    var correcao = cargaSistema / mediaCarga;

                    dps[4] = dps[4] * correcao;
                    dps[6] = dps[6] * correcao;
                    dps[8] = dps[8] * correcao;

                }

                //segundo estagio
                for (int i = 1; i <= 4; i++)
                {
                    int ultimoEstagio = dtAtual.Estagios + 1;

                    var dps = dadger.BlocoDp.Where(x => x[2] == i && x[1] == ultimoEstagio).First();

                    double mediaCarga = (dps[4] * dps[5] + dps[6] * dps[7] + dps[8] * dps[9]) / (dps[5] + dps[7] + dps[9]);

                    var cargaSistema = sistema.Mercado.Where(x => x.Ano == dtAtual.AnoSeguinte && x.Mercado == i).Select(x => x[dtAtual.MesSeguinte]).First() + c_adic_SomaNext.Where(x => x.mercado == (i) && x.carga > 0.0).Sum(x => x.carga);
                    var correcao = cargaSistema / mediaCarga;

                    dps[4] = dps[4] * correcao;
                    dps[6] = dps[6] * correcao;
                    dps[8] = dps[8] * correcao;

                }
            }


        }
    }
}




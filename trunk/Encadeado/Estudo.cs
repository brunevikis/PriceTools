﻿using Compass.CommomLibrary;

using Compass.Services.DB;
using Encadeado.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;

using System.Text;
using System.Threading.Tasks;
using System.IO;


using Compass.Services;
using Compass.CommomLibrary.EntdadosDat;
using System.IO.Compression;
using System.Globalization;



using System.Threading;
using System.Net.Http;

namespace Encadeado
{



    public class Estudo
    {

        //DeckNewave DeckInicio;
        DeckNewave DeckMedia;
        DeckNewave DeckMediaBase;

        public string Origem { get; set; }
        public string Destino { get; set; }

        public int MesesAvancar { get; set; }

        public int IteracaoAtual { get; set; }
        public string ExecutavelNewave { get; set; }
        public string ExecutarConsist { get; set; }
        public bool NwHibrido { get; set; }

        public bool DefinirVolumesPO { get; set; }
        public bool StartREEAgrupado { get; set; }

        public Dictionary<int, int[]> PrevisaoVazao { get; set; }
        public Dictionary<int, double[]> VolumesPO { get; set; }

        public Dictionary<int, double[]> Bloco_VE { get; set; }

        public List<Compass.CommomLibrary.IRE> Restricoes { get; set; }
        //public List<Tuple<int, double>> EarmMax { get; set; }
        //public List<Tuple<int, double>> EarmBase { get; set; }

        public Compass.CommomLibrary.Decomp.ConfigH ConfighBase { get; set; }
        public List<IAGRIGNT> Agrints { get; set; }

        public List<IADTERM> Adterm { get; set; }
        public List<IMERCADO> MERCADO { get; set; }
        public List<ICURVA> Curva { get; set; }
        public List<IADTERMDAD> Adtermdad { get; set; }
        public List<IINTERCAMBIO> Intercambios { get; set; }
        public List<IMODIF> Modifs { get; set; }
        public List<IREMODIF> ReModifs { get; set; }
        public List<IREEDAT> Reedads { get; set; }
        public List<IRESTELECSV> Restelecsv { get; set; }

        public Estudo()
        {
            IteracaoAtual = 0;
            VolumesPO = new Dictionary<int, double[]>();
            PrevisaoVazao = new Dictionary<int, int[]>();
        }

        public bool ExecucaoPrincipal()
        {

            List<Task> consists = new List<Task>();

            consists.Add(SetCasoInicial());
            //List<Task> consists = new List<Task>();

            while (IteracaoAtual++ < MesesAvancar) consists.Add(Iterar());

            Task.WaitAll(consists.ToArray());
            return true;
        }

        public bool execucaoConsistDC(List<string> consistFoldres)
        {
            List<Task> consists = new List<Task>();

            foreach (var conF in consistFoldres)
            {
                consists.Add(IterarConsist(conF));
            }


            Task.WaitAll(consists.ToArray());
            return true;
        }

        private Task Iterar()
        {

            Incrementar(DeckMedia);
            SetNomeDeck(DeckMedia);
            string planMemo = Directory.GetFiles(Origem).Where(x => Path.GetFileName(x).StartsWith("Memória de Cálculo", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            DeckMedia.SaveFilesToFolder(System.IO.Path.Combine(Destino, DeckMedia.Dger.AnoEstudo.ToString("0000") + DeckMedia.Dger.MesEstudo.ToString("00")));

            string cam = System.IO.Path.Combine(Destino, DeckMedia.Dger.AnoEstudo.ToString("0000") + DeckMedia.Dger.MesEstudo.ToString("00"));
            Compass.Services.Deck.CorrigeArquivosdat(cam);


            if (planMemo != null && File.Exists(planMemo))
            {
                File.Copy(planMemo, Path.Combine(cam, Path.GetFileName(planMemo)), true);
            }
            AlterarModif(DeckMedia);
            IncrementarREEDAT(DeckMedia, true);

            DeckMedia.EscreverListagemNwlistop();

            IncrementarCSV_LIBS(DeckMedia);//TODO: liberar somente quando for oficial deck com libs csv

            var path = DeckMedia.Folder;
            //TODO: executar consistencia
            return Task.Factory.StartNew(() =>
            {
                ExecutarConsistencia(path);
                Compass.Services.Deck.CreateDgerNewdesp(path);
            });

        }

        private Task IterarConsist(string destino)
        {
            string path = destino;
            Thread.Sleep(1000);
            return Task.Factory.StartNew(() =>
            {
                ExecutarConsistencia(path, path.Split('\\').Last());
                Compass.Services.Deck.CreateDgerNewdesp(path);
            });
        }

        private void SetNomeDeck(DeckNewave deck)
        {
            deck.Dger.NomeEstudo = "Estudo de Previsao de PLD - Mes/Ano: " + deck.Dger.MesEstudo.ToString("00") + "/" + deck.Dger.AnoEstudo.ToString("0000");
        }

        private void Incrementar(DeckNewave deck)
        {

            deck.Dger.DataEstudo = deck.Dger.DataEstudo.AddMonths(1);

            // Atualizar dados de classes térmicas.

            IncrementarTermicas(deck);
            //IncrementarAversaoRisco(deck);
            IncrementarOutrosUsosAgua(deck);
            IncrementarAdterm(deck);

            IncrementarAgrInt(deck);
            IncrementarHidr(deck);
            IncrementarSistema(deck);
            IncrementarCurva(deck);

            if (DefinirVolumesPO) //IncrementarMercados(deck);
            {
                IncrementarEarm(deck);

                //gambiarra para preparar para virada de ano
                if (deck.Dger.DataEstudo.Month == 12)
                {
                    this.VolumesPO[1][0] = this.VolumesPO[1][12];
                    this.VolumesPO[2][0] = this.VolumesPO[2][12];
                    this.VolumesPO[3][0] = this.VolumesPO[3][12];
                    this.VolumesPO[4][0] = this.VolumesPO[4][12];
                }

            }

            if (deck.Dger.TipoTendenciaHidrologia == 2) IncrementarVAZAO(deck);
            IncrementarRE(deck);
        }

        private void IncrementarTermicas(DeckNewave deck)
        {

            var expts = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.expt].Document as Compass.CommomLibrary.ExptDat.ExptDat;
            var manutts = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.manutt].Document as Compass.CommomLibrary.ManuttDat.ManuttDat;
            var confts = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.conft].Document as Compass.CommomLibrary.ConftDat.ConftDat;
            var clasts = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.clast].Document as Compass.CommomLibrary.ClastDat.ClastDat;

            foreach (var modif in clasts.Modifs.ToList())
            {
                if (modif.Inicio < deck.Dger.DataEstudo &&
                    modif.Fim <= deck.Dger.DataEstudo.AddMonths(1) &&
                    clasts.Modifs.Where(x => x.Num == modif.Num).Count() == 1
                    )
                {
                    modif.Inicio = deck.Dger.DataEstudo;
                    modif.Fim = modif.Fim.AddMonths(1);
                }
                if (modif.Inicio < deck.Dger.DataEstudo && modif.Fim >= deck.Dger.DataEstudo)
                {
                    modif.Inicio = deck.Dger.DataEstudo;
                }
                else if (modif.Fim < deck.Dger.DataEstudo)
                {
                    clasts.Modifs.Remove(modif);
                }
            }

            foreach (var manutt in manutts.ToList())
            {
                if (manutt.DataInicio < deck.Dger.DataEstudo && manutt.DataFim >= deck.Dger.DataEstudo)
                {
                    manutt.DataInicio = deck.Dger.DataEstudo;
                }
                else if (manutt.DataFim < deck.Dger.DataEstudo)
                {
                    manutts.Remove(manutt);
                }
            }

            foreach (var expt in expts.ToList())
            {
                if (expt.DataInicio < deck.Dger.DataEstudo && expt.DataFim >= deck.Dger.DataEstudo)
                {
                    expt.DataInicio = deck.Dger.DataEstudo;

                }
                else if (expt.DataFim < deck.Dger.DataEstudo)
                {
                    expts.Remove(expt);
                }
            }

            foreach (var u in confts)
            {
                if (u.Existente == "EX" || u.Existente == "NC") continue;
                else if (!expts.Any(x => x.Cod == u.Num)) u.Existente = "EX";
                else if (expts.Any(x => x.Cod == u.Num && x.DataInicio == deck.Dger.DataEstudo)) u.Existente = "EE";
            }
        }

        private void IncrementarOutrosUsosAgua(DeckNewave deck)
        {

            var dsvagua = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.dsvagua].Document as Compass.CommomLibrary.Dsvagua.Dsvagua;

            foreach (var item in dsvagua.ToList())
            {

                if (deck.Dger.MesEstudo == 1 && item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2)
                {
                    var novoAno = item.Clone() as Compass.CommomLibrary.Dsvagua.DsvLine;
                    novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;
                    dsvagua.InsertAfter(item, novoAno);
                }
                else if (item.Ano == deck.Dger.AnoEstudo)
                {
                    for (int i = 1; i < deck.Dger.MesEstudo; i++)
                    {
                        item[i + 1] = 0;
                    }
                }
                else if (item.Ano < deck.Dger.AnoEstudo)
                {
                    dsvagua.Remove(item);
                }
            }
        }

        private double[] GetRPO(DeckNewave deck, DateTime datOp)
        {
            var agrintDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.agrint].Document as Compass.CommomLibrary.AgrintDat.AgrintDat;

            var patamarDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
            var sistemaDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as Compass.CommomLibrary.SistemaDat.SistemaDat;

            //IDB objSQL = new SQLServerDBCompass("ESTUDO_PV");
            //DbDataReader reader = null;
            //string[] campos = { "[Data]", "[submercado]", "[Ano]", "[Janeiro]", "[Fevereiro]", "[Março]", "[Abril]", "[Maio]", "[Junho]", "[Julho]", "[Agosto]", "[Setembro]", "[Outubro]", "[Novembro]", "[Dezembro]" };

            //string tabela = "[ESTUDO_PV].[dbo].[UEE]";

            //string strQuery = String.Format(@"SELECT TOP 5 [id],[Ano],[Janeiro],[Fevereiro],[Março] ,[Abril],[Maio],[Junho],[Julho],[Agosto],[Setembro] ,[Outubro],[Novembro],[Dezembro]FROM [ESTUDO_PV].[dbo].[UEE] order by Data desc ");
            //string strQuery = String.Format(@"SELECT TOP 5 [id],[Ano],[Janeiro],[Fevereiro],[Março] ,[Abril],[Maio],[Junho],[Julho],[Agosto],[Setembro] ,[Outubro],[Novembro],[Dezembro]FROM [ESTUDO_PV].[dbo].[UEE] where YEAR(Data) = YEAR(GETDATE()) order by ano asc ");
            // List<double[]> UEE = new List<double[]>();
            //List<int> newave = new List<int> { 6500, 6500, 6500, 6500, 6500, 6500, 5800, 5800, 5800, 5800, 5800, 5800 };//Max valor que RecebimentoNE pode assumir em cada mês atè dez 2021

            List<double> UEE = new List<double> { 0, 0, 3544, 4344, 5186, 6346, 7199, 7609, 7987, 7297, 7030, 5802 };

            //reader = objSQL.GetReader(strQuery);


            //double UEE;
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


            var sistemaNE = sistemaDat.Mercado.Where(x => x.Mercado == 3 && x.Ano == datOp.Year).First()[datOp.Month];
            var patsNE = patamarDat.Carga.Where(x => x.Ano == datOp.Year && x.Mercado == 3).ToList();

            var NEPT1 = Math.Round((sistemaNE * patsNE[0][datOp.Month]));
            var NEPT2 = Math.Round((sistemaNE * patsNE[1][datOp.Month]));
            var NEPT3 = Math.Round((sistemaNE * patsNE[2][datOp.Month]));

            var cargaNEPT1 = Math.Round((sistemaNE * patsNE[0][datOp.Month] * 0.43));//43% da energia do mercado vezes patamares do mercado
            var cargaNEPT2 = Math.Round((sistemaNE * patsNE[1][datOp.Month] * 0.43));
            var cargaNEPT3 = Math.Round((sistemaNE * patsNE[2][datOp.Month] * 0.43));


            double[] RPOs = new double[3];

            if (datOp.Year == DateTime.Today.AddYears(1).Year)
            {
                //RPOs[0] = Math.Round(NEPT1 * 0.04 + UEE[1][datOp.Month + 1] * 0.06, 0);
                //RPOs[1] = Math.Round(NEPT2 * 0.04 + UEE[1][datOp.Month + 1] * 0.06, 0);
                //RPOs[2] = Math.Round(NEPT3 * 0.04 + UEE[1][datOp.Month + 1] * 0.06, 0);

                RPOs[0] = Math.Round(NEPT1 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
                RPOs[1] = Math.Round(NEPT2 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
                RPOs[2] = Math.Round(NEPT3 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
            }
            else
            {
                //RPOs[0] = Math.Round(NEPT1 * 0.04 + UEE[0][datOp.Month + 1] * 0.06, 0);
                //RPOs[1] = Math.Round(NEPT2 * 0.04 + UEE[0][datOp.Month + 1] * 0.06, 0);
                //RPOs[2] = Math.Round(NEPT3 * 0.04 + UEE[0][datOp.Month + 1] * 0.06, 0);
                RPOs[0] = Math.Round(NEPT1 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
                RPOs[1] = Math.Round(NEPT2 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
                RPOs[2] = Math.Round(NEPT3 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
            }

            return RPOs;
        }

        private void IncrementarAdterm(DeckNewave deck)
        {
            var adtermDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.adterm].Document as Compass.CommomLibrary.AdtermDat.AdtermDat;
            var datOp = deck.Dger.DataEstudo;
            var datNex = datOp.AddMonths(1);
            if (datNex.Month == 1)
            {
                datNex = datOp;
            }
            if (this.Adtermdad.Count() > 0)
            {
                foreach (var adt in adtermDat.Despachos.Where(x => x.String != "            "))// zera todos so meses de todas as usinas para depois ver em qual preencher de cordo com a planilha
                {
                    var indice = adtermDat.Despachos.IndexOf(adt);
                    adtermDat.Despachos[indice + 1].Lim_P1 = 0;
                    adtermDat.Despachos[indice + 1].Lim_P2 = 0;
                    adtermDat.Despachos[indice + 1].Lim_P3 = 0;
                    adtermDat.Despachos[indice + 2].Lim_P1 = 0;
                    adtermDat.Despachos[indice + 2].Lim_P2 = 0;
                    adtermDat.Despachos[indice + 2].Lim_P3 = 0;
                }


                var adtermdads = this.Adtermdad.Where(x => x.ano == deck.Dger.AnoEstudo && x.mes == deck.Dger.MesEstudo).ToList();

                if (adtermdads.Count() > 0)
                {
                    var Usinas = adtermDat.Despachos.Where(x => x.String != "            ").ToList();
                    foreach (var adx in adtermdads.Where(x => x.estagio == 1))// primeiro mes
                    {
                        var adt = Usinas.Where(x => x.Numero == adx.usina).FirstOrDefault();
                        if (adt != null)
                        {
                            var indice = adtermDat.Despachos.IndexOf(adt);
                            adtermDat.Despachos[indice + 1].Lim_P1 = adx.PT1;
                            adtermDat.Despachos[indice + 1].Lim_P2 = adx.PT2;
                            adtermDat.Despachos[indice + 1].Lim_P3 = adx.PT3;
                        }
                    }

                    foreach (var adx in adtermdads.Where(x => x.estagio == 2))// segundo mes
                    {
                        var adt = Usinas.Where(x => x.Numero == adx.usina).FirstOrDefault();
                        if (adt != null)
                        {
                            var indice = adtermDat.Despachos.IndexOf(adt);
                            adtermDat.Despachos[indice + 2].Lim_P1 = adx.PT1;
                            adtermDat.Despachos[indice + 2].Lim_P2 = adx.PT2;
                            adtermDat.Despachos[indice + 2].Lim_P3 = adx.PT3;
                        }
                    }

                }
            }

            foreach (var adt in adtermDat.Despachos.Where(x => x.String != "            "))
            {

                /*  if (this.Adterm.Count() != 0)
                  {
                      foreach (var adtx in this.Adterm.Where(x => x.Usina == adt.Numero).ToList())
                      {
                          var indice = adtermDat.Despachos.IndexOf(adt);

                          if (adtx.Mes == datOp.Month)
                          {

                              adtermDat.Despachos[indice + 1].Lim_P1 = adtx.RestricaoP1;
                              adtermDat.Despachos[indice + 1].Lim_P2 = adtx.RestricaoP2;
                              adtermDat.Despachos[indice + 1].Lim_P3 = adtx.RestricaoP3;

                          }
                          else if (adtx.Mes == datNex.Month)
                          {

                              adtermDat.Despachos[indice + 2].Lim_P1 = adtx.RestricaoP1;
                              adtermDat.Despachos[indice + 2].Lim_P2 = adtx.RestricaoP2;
                              adtermDat.Despachos[indice + 2].Lim_P3 = adtx.RestricaoP3;

                          }

                      }
                  }*/
            }
            /*
            else
            {


                var Expt = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.expt].Document as Compass.CommomLibrary.ExptDat.ExptDat;

                var Potef = Expt.Where(x => x.Cod == adt.Numero && x.Tipo == "POTEF").First();



                var indice = adtermDat.Despachos.IndexOf(adt);

                var Fator_1 = Expt.Where(x => x.Cod == adt.Numero && x.Tipo == "FCMAX" && datOp >= x.DataInicio && x.DataFim >= datOp).FirstOrDefault();

                double Desp = 0;
                if (Fator_1 != null)
                {
                    Desp = Potef.Valor * (Fator_1.Valor / 100);
                }
                else
                {
                    Desp = Potef.Valor;
                }



                adtermDat.Despachos[indice + 1].Lim_P1 = Desp;
                adtermDat.Despachos[indice + 1].Lim_P2 = Desp;
                adtermDat.Despachos[indice + 1].Lim_P3 = Desp;

                var Fator_2 = Expt.Where(x => x.Cod == adt.Numero && x.Tipo == "FCMAX" && x.DataInicio <= datNex && x.DataFim >= datNex).FirstOrDefault();

                double Desp_2 = 0;

                if (Fator_2 != null)
                {
                    Desp_2 = Potef.Valor * (Fator_2.Valor / 100);
                }
                else
                {
                    Desp_2 = Potef.Valor;
                }



                adtermDat.Despachos[indice + 2].Lim_P1 = Desp_2;
                adtermDat.Despachos[indice + 2].Lim_P2 = Desp_2;
                adtermDat.Despachos[indice + 2].Lim_P3 = Desp_2;
            }

*/




            //var usina = adtermDat.Despachos.Select(x => x).Where(x => x.String != "            ").ToList();
            //foreach (var adtx in this.Adterm)
            //{
            //    if (adtx.Mes == datOp.Month || adtx.Mes == datNex.Month)
            //    {
            //        if (usina.All(x => x.Numero != adtx.Usina))
            //        {
            //            //var indice = adtermDat.Despachos.IndexOf(usina);
            //            if (adtx.Mes == datOp.Month)
            //            {
            //                var adtermlinha = new Compass.CommomLibrary.AdtermDat.AdtermLine()
            //                {
            //                    Numero = adtx.Usina,
            //                    Lag = 2,

            //                };
            //                adtermDat.Despachos.Add(adtermlinha);
            //                var adtermDado = new Compass.CommomLibrary.AdtermDat.AdtermLine()
            //                {
            //                    Lim_P1 = adtx.RestricaoP1,
            //                    Lim_P2 = adtx.RestricaoP2,
            //                    Lim_P3 = adtx.RestricaoP3,

            //                };
            //                adtermDat.Despachos.Add(adtermDado);
            //            }
            //            if (adtx.Mes == datNex.Month)
            //            {

            //                var adtermDado = new Compass.CommomLibrary.AdtermDat.AdtermLine()
            //                {
            //                    Lim_P1 = adtx.RestricaoP1,
            //                    Lim_P2 = adtx.RestricaoP2,
            //                    Lim_P3 = adtx.RestricaoP3,

            //                };
            //                adtermDat.Despachos.Add(adtermDado);
            //            }


            //        }
            //    }



            //}
        }

        public void IncrementarCurva(DeckNewave deck)
        {
            DeckMediaBase = new DeckNewave();
            DeckMediaBase.EstudoPai = this;

            DeckMediaBase.GetFiles(Origem);
            var curvaBase = DeckMediaBase[Compass.CommomLibrary.Newave.Deck.DeckDocument.curva].Document as Compass.CommomLibrary.CurvaDat.CurvaDat;

            var curvaDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.curva].Document as Compass.CommomLibrary.CurvaDat.CurvaDat;

            var curvasBaseLinha = curvaBase.BlocoCurvaSeg.Where(x => x is Compass.CommomLibrary.CurvaDat.CurvaSegPorLine).ToList();
            var curvasDatLinha = curvaDat.BlocoCurvaSeg.Where(x => x is Compass.CommomLibrary.CurvaDat.CurvaSegPorLine).ToList();

            foreach (var cb in curvasBaseLinha)
            {
                foreach (var cd in curvasDatLinha)
                {
                    if (cb.Ree == cd.Ree && cb.Ano == cd.Ano)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            cd[i] = cb[i];
                        }
                    }
                }
            }

            //curvasDatLinha = curvasBaseLinha;


            foreach (var cur in this.Curva.Where(x => x.MesEstudo == deck.Dger.MesEstudo))
            {
                foreach (var item in curvaDat.BlocoCurvaSeg.Where(x => x is Compass.CommomLibrary.CurvaDat.CurvaSegPorLine).ToList())
                {
                    if (item.Ree == cur.REE && item.Ano == cur.Ano)
                    {
                        double percent = cur.Porc * 100;
                        int mes = Convert.ToInt32(cur.Mes);
                        item[mes] = percent;
                        var val = item[mes];

                    }
                }
                //if (curvaLine != null)
                //{
                //    double percent = cur.Porc * 100;
                //    int mes = Convert.ToInt32(cur.Mes);
                //    var val = curvaLine.Valores[mes];

                //    curvaLine.Valores[mes] = percent;

                //}
            }

        }

        private void IncrementarAgrInt(DeckNewave deck)
        {

            var reDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.agrint].Document as Compass.CommomLibrary.AgrintDat.AgrintDat;

            //começo==========
            var agrintDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.agrint].Document as Compass.CommomLibrary.AgrintDat.AgrintDat;

            var patamarDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
            var sistemaDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as Compass.CommomLibrary.SistemaDat.SistemaDat;



            var datOp = deck.Dger.DataEstudo;
            var datNex = datOp.AddMonths(1);




            //IDB objSQL = new SQLServerDBCompass("ESTUDO_PV");
            //DbDataReader reader = null;
            //string[] campos = { "[Data]", "[submercado]", "[Ano]", "[Janeiro]", "[Fevereiro]", "[Março]", "[Abril]", "[Maio]", "[Junho]", "[Julho]", "[Agosto]", "[Setembro]", "[Outubro]", "[Novembro]", "[Dezembro]" };

            //string tabela = "[ESTUDO_PV].[dbo].[UEE]";

            //string strQuery = String.Format(@"SELECT TOP 5 [id],[Ano],[Janeiro],[Fevereiro],[Março] ,[Abril],[Maio],[Junho],[Julho],[Agosto],[Setembro] ,[Outubro],[Novembro],[Dezembro]FROM [ESTUDO_PV].[dbo].[UEE] order by Data desc ");
            ////string strQuery = String.Format(@"SELECT TOP 5 [id],[Ano],[Janeiro],[Fevereiro],[Março] ,[Abril],[Maio],[Junho],[Julho],[Agosto],[Setembro] ,[Outubro],[Novembro],[Dezembro]FROM [ESTUDO_PV].[dbo].[UEE] where YEAR(Data) = YEAR(GETDATE()) order by ano asc ");
            //List<double[]> UEE = new List<double[]>();
            //reader = objSQL.GetReader(strQuery);

            List<double> UEE = new List<double> { 0, 0, 3544, 4344, 5186, 6346, 7199, 7609, 7987, 7297, 7030, 5802 };
            //double UEE;
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


            var sistemaNE = sistemaDat.Mercado.Where(x => x.Mercado == 3 && x.Ano == datOp.Year).First()[datOp.Month];
            var patsNE = patamarDat.Carga.Where(x => x.Ano == datOp.Year && x.Mercado == 3).ToList();

            var NEPT1 = Math.Round((sistemaNE * patsNE[0][datOp.Month]));
            var NEPT2 = Math.Round((sistemaNE * patsNE[1][datOp.Month]));
            var NEPT3 = Math.Round((sistemaNE * patsNE[2][datOp.Month]));

            var cargaNEPT1 = Math.Round((sistemaNE * patsNE[0][datOp.Month] * 0.43));//43% da energia do mercado vezes patamares do mercado
            var cargaNEPT2 = Math.Round((sistemaNE * patsNE[1][datOp.Month] * 0.43));
            var cargaNEPT3 = Math.Round((sistemaNE * patsNE[2][datOp.Month] * 0.43));

            //List<int> newave = new List<int> { 6500, 6500, 6500, 6500, 6500, 6500, 5800, 5800, 5800, 5800, 5800, 5800 };//Max valor que RecebimentoNE pode assumir em cada mês atè dez 2021

            double agrintP1 = 0;
            double agrintP2 = 0;
            double agrintP3 = 0;

            foreach (var re in agrintDat[datOp].Where(x => x.Value.Numero == 1))//trocar o datOp por datNex caso tenha que voltar
            {
                if (datOp >= re.Value.Inicio && datOp <= re.Value.Fim)//trocar o datOp por datNex caso tenha que voltar
                {
                    agrintP1 = re.Value.Lim_P1;
                    agrintP2 = re.Value.Lim_P2;
                    agrintP3 = re.Value.Lim_P3;
                }

            }

            //var P1 = Math.Min(cargaNEPT1, agrintP1);//minimo entre RNE e 43% da carga
            // var P2 = Math.Min(cargaNEPT2, agrintP2);
            //var P3 = Math.Min(cargaNEPT3, agrintP3);

            var P1 = agrintP1;
            var P2 = agrintP2;
            var P3 = agrintP3;


            double RPO1;
            double RPO2;
            double RPO3;
            if (datOp.Year == DateTime.Today.AddYears(1).Year)
            {
                //RPO1 = Math.Round(NEPT1 * 0.04 + UEE[1][datOp.Month + 1] * 0.06, 0);
                //RPO2 = Math.Round(NEPT2 * 0.04 + UEE[1][datOp.Month + 1] * 0.06, 0);
                //RPO3 = Math.Round(NEPT3 * 0.04 + UEE[1][datOp.Month + 1] * 0.06, 0);

                RPO1 = Math.Round(NEPT1 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
                RPO2 = Math.Round(NEPT2 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
                RPO3 = Math.Round(NEPT3 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
            }
            else
            {
                //RPO1 = Math.Round(NEPT1 * 0.04 + UEE[0][datOp.Month + 1] * 0.06, 0);
                //RPO2 = Math.Round(NEPT2 * 0.04 + UEE[0][datOp.Month + 1] * 0.06, 0);
                //RPO3 = Math.Round(NEPT3 * 0.04 + UEE[0][datOp.Month + 1] * 0.06, 0);

                RPO1 = Math.Round(NEPT1 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
                RPO2 = Math.Round(NEPT2 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
                RPO3 = Math.Round(NEPT3 * 0.04 + UEE[datOp.Month - 1] * 0.06, 0);
            }

            //var RNE1 = P1 - RPO1;
            //var RNE2 = P2 - RPO2;
            //var RNE3 = P3 - RPO3;

            var RNE1 = P1;
            var RNE2 = P2;
            var RNE3 = P3;
            // foreach (var re in reDat.Agrupamentos.ToList())
            //{
            foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == 1).ToList())
            {
                if (reDet.Inicio < deck.Dger.DataEstudo)
                {
                    reDat.Detalhes.Remove(reDet);
                }

                else if (reDet.Inicio == deck.Dger.DataEstudo && reDet.Fim == deck.Dger.DataEstudo && reDet.Numero == 1)
                {
                    reDet.Inicio = deck.Dger.DataEstudo;
                    reDet.Lim_P1 = RNE1;
                    reDet.Lim_P2 = RNE2;
                    reDet.Lim_P3 = RNE3;
                    var RPOs = GetRPO(deck, datOp.AddMonths(1));
                    var teste = datOp.AddMonths(1);
                    var seguinte = reDat.Detalhes.Where(x => x.Numero == 1)
                        .Where(x => x.Inicio <= datOp.AddMonths(1) && datOp.AddMonths(1) <= x.Fim).FirstOrDefault();
                    var proximo = seguinte.Clone() as Compass.CommomLibrary.AgrintDat.AgrintValLine;

                    seguinte.Inicio = datOp.AddMonths(1);
                    //seguinte.Lim_P1 -= RPOs[0];
                    //seguinte.Lim_P2 -= RPOs[1];
                    //seguinte.Lim_P3 -= RPOs[2];

                    seguinte.Lim_P1 = seguinte.Lim_P1;
                    seguinte.Lim_P2 = seguinte.Lim_P2;
                    seguinte.Lim_P3 = seguinte.Lim_P3;

                    seguinte.Fim = datOp.AddMonths(1);
                    proximo.Inicio = seguinte.Fim.AddMonths(1);
                    if (proximo.Inicio <= proximo.Fim)
                    {
                        reDat.Detalhes.InsertAfter(seguinte, proximo);
                    }

                }
                else if (reDet.Numero == 1 && reDet.Inicio == deck.Dger.DataEstudo && reDet.Fim > deck.Dger.DataEstudo)
                {
                    var agrlinha = new Compass.CommomLibrary.AgrintDat.AgrintValLine()
                    {


                        Numero = 1,
                        Lim_P1 = RNE1,
                        Lim_P2 = RNE2,
                        Lim_P3 = RNE3,
                        Descricao = " RECEBIMENTO NE",
                        Inicio = new DateTime(datOp.Year, datOp.Month, 1),
                        Fim = new DateTime(datOp.Year, datOp.Month, 1),

                    };
                    var anterior = reDat.Detalhes.Where(x => x.Numero == agrlinha.Numero)
                        .Where(x => x.Inicio < agrlinha.Inicio).FirstOrDefault();
                    var posterior = reDat.Detalhes.Where(x => x.Numero == agrlinha.Numero)
                        .Where(x => x.Inicio == agrlinha.Fim && x.Fim > agrlinha.Fim).FirstOrDefault();
                    reDat.Detalhes.Insert(0, agrlinha);
                    reDet.Inicio = agrlinha.Fim.AddMonths(1);

                    //if (anterior != null)
                    //{
                    //    var anteriorSplit = anterior.Clone() as Compass.CommomLibrary.AgrintDat.AgrintValLine;
                    //    anterior.Inicio = agrlinha.Inicio;
                    //    anteriorSplit.Fim = agrlinha.Inicio.AddMonths(-1);

                    //    reDat.Detalhes.Add(anteriorSplit);
                    //}

                    //if (posterior != null)
                    //{
                    //    var posteriorSplit = posterior.Clone() as Compass.CommomLibrary.AgrintDat.AgrintValLine;
                    //    posterior.Fim = agrlinha.Fim; ;
                    //    posteriorSplit.Inicio = agrlinha.Fim.AddMonths(1);

                    //    reDat.Detalhes.InsertAfter(agrlinha, posteriorSplit);

                    //    //reDat.Detalhes.Add(posteriorSplit);
                    //}

                    //reDat.Detalhes.Where(x => x.Numero == agrlinha.Numero)
                    //        .Where(x => x.Inicio >= agrlinha.Inicio && x.Fim <= agrlinha.Fim).ToList().ForEach(x =>
                    //            reDat.Detalhes.Remove(x)
                    //            );

                    // reDat.Detalhes.InsertAfter(agrlinha,pos)
                    // reDat.Detalhes.Add(agrlinha);

                }
                else if (reDet.Fim < deck.Dger.DataEstudo)
                {
                    reDat.Detalhes.Remove(reDet);
                }
            }

            //if (reDat.Detalhes.Where(x => x.Numero == re.Numero).Count() == 0) reDat.Agrupamentos.Remove(re);
            // }



            //fim =======

            //foreach (var re in reDat.Agrupamentos.ToList())
            //{
            //    foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == re.Numero).ToList())
            //    {

            //        if (reDet.Inicio < deck.Dger.DataEstudo && reDet.Fim >= deck.Dger.DataEstudo)
            //        {
            //            reDet.Inicio = deck.Dger.DataEstudo;
            //            
            //        }
            //        else if (reDet.Fim < deck.Dger.DataEstudo)
            //        {
            //            reDat.Detalhes.Remove(reDet);
            //        }
            //    }

            //    if (reDat.Detalhes.Where(x => x.Numero == re.Numero).Count() == 0) reDat.Agrupamentos.Remove(re);
            //}


            foreach (var rest in this.Agrints.Where(x => x.MesEstudo == deck.Dger.MesEstudo))
            {

                //procura restricao
                var re = reDat.Agrupamentos.GroupBy(x => x.Numero).Where(
                    x => string.Join(";", x.Select(y => y.SistemaA.ToString() + "-" + y.SistemaB.ToString()).OrderBy(y => y))
                    == string.Join(";", rest.Intercambios.Select(y => y.Item1.ToString() + "-" + y.Item2.ToString()).OrderBy(y => y))
                    ).SelectMany(x => x).FirstOrDefault();

                //se nao exite insere
                if (re == null)
                {
                    var agrintN = reDat.Agrupamentos.Max(x => x.Numero) + 1;
                    rest.Intercambios.ForEach(x =>
                    {
                        reDat.Agrupamentos.Add(
                        new Compass.CommomLibrary.AgrintDat.AgrintLine() { Numero = agrintN, SistemaA = x.Item1, SistemaB = x.Item2, Coef = 1 }
                        );
                    }
                                );

                    var val = new Compass.CommomLibrary.AgrintDat.AgrintValLine()
                    {
                        Numero = agrintN,
                        Lim_P1 = rest.RestricaoP1,
                        Lim_P2 = rest.RestricaoP2,
                        Lim_P3 = rest.RestricaoP3,
                        Inicio = new DateTime(rest.AnoIni, rest.MesIni, 1),
                        Fim = new DateTime(rest.AnoFim, rest.MesFim, 1),
                    };

                    reDat.Detalhes.Add(val);
                }
                //altera ou insere novo valor
                else
                {

                    var val = new Compass.CommomLibrary.AgrintDat.AgrintValLine()
                    {
                        Numero = re.Numero,
                        Lim_P1 = rest.RestricaoP1,
                        Lim_P2 = rest.RestricaoP2,
                        Lim_P3 = rest.RestricaoP3,
                        Inicio = new DateTime(rest.AnoIni, rest.MesIni, 1),
                        Fim = new DateTime(rest.AnoFim, rest.MesFim, 1),
                    };

                    var anterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                        .Where(x => x.Inicio < val.Inicio && x.Fim >= val.Inicio).FirstOrDefault();
                    var posterior = reDat.Detalhes.Where(x => x.Numero == val.Numero)
                        .Where(x => x.Inicio <= val.Fim && x.Fim > val.Fim).FirstOrDefault();

                    if (anterior != null)
                    {
                        var anteriorSplit = anterior.Clone() as Compass.CommomLibrary.AgrintDat.AgrintValLine;
                        anterior.Inicio = val.Inicio;
                        anteriorSplit.Fim = val.Inicio.AddMonths(-1);

                        reDat.Detalhes.Add(anteriorSplit);
                    }

                    if (posterior != null)
                    {
                        var posteriorSplit = posterior.Clone() as Compass.CommomLibrary.AgrintDat.AgrintValLine;
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
            }
        }

        private void IncrementarHidr(DeckNewave deck)
        {

            var exphs = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.exph].Document as Compass.CommomLibrary.ExphDat.ExphDat;
            var confhds = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.confhd].Document as Compass.CommomLibrary.ConfhdDat.ConfhdDat;
            var modifs = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;

            //enchimento de volume morto
            foreach (var exph in exphs.Where(x => x.DataEnchimento.HasValue && x.DataEnchimento.Value < deck.Dger.DataEstudo).ToList())
            {

                if (exph.DuracaoEnchimento > 1)
                {
                    exph.DataEnchimento = deck.Dger.DataEstudo;


                    double volume = 100 - exph.VolumePreenchido;
                    volume = (volume / exph.DuracaoEnchimento) + exph.VolumePreenchido;

                    exph.DuracaoEnchimento--;
                    exph.VolumePreenchido = volume;

                    if (exph.DuracaoEnchimento == 0 || exph.VolumePreenchido >= 100) exphs.Remove(exph);
                }
                else
                {
                    var confhd = confhds.Where(x => x.Cod == exph.Cod).First();
                    if (confhd.Situacao == "NE") confhd.Situacao = "EE";

                    if (!modifs.Any(x => x.Usina == exph.Cod))
                    {
                        modifs.Add(new Compass.CommomLibrary.ModifDatNW.ModifLine()
                        {
                            Usina = exph.Cod,
                            Chave = "USINA",
                            NovosValores = new string[] { exph.Cod.ToString() }
                        });
                    }

                    if (!modifs.Any(x => x.Usina == exph.Cod && x.Chave == "NUMCNJ"))
                    {
                        var x = modifs.First(y => y.Usina == exph.Cod);
                        modifs.Insert(modifs.IndexOf(x) + 1,
                            new Compass.CommomLibrary.ModifDatNW.ModifLine()
                            {
                                Usina = exph.Cod,
                                Chave = "NUMCNJ",
                                NovosValores = new string[] { "0" }
                            });

                    }
                    else
                    {
                        modifs.First(x => x.Usina == exph.Cod && x.Chave == "NUMCNJ").SetValores(0);
                    }

                    exphs.Remove(exph);
                }
            }

            foreach (var modif in modifs.Where(x => x.DataModif != DateTime.MinValue && x.DataModif < deck.Dger.DataEstudo).ToList())
            {
                if (modifs.Any(x => x.Usina == modif.Usina && x.Chave == modif.Chave && x.DataModif == deck.Dger.DataEstudo))
                {
                    modifs.Remove(modif);
                }
                else
                {
                    modif.DataModif = deck.Dger.DataEstudo;
                }
            }

            foreach (var expG in exphs.Where(x => x.DataEntrada.HasValue && x.DataEntrada.Value < deck.Dger.DataEstudo).GroupBy(x => x.Cod))
            {

                var modifConj = modifs.Where(x => x.Chave == "NUMCNJ" && x.Usina == expG.Key).First();
                var modifMaq = modifs.Where(x => x.Chave == "NUMMAQ" && x.Usina == expG.Key).ToDictionary(x => int.Parse(x.NovosValores[1]));

                if (int.Parse(modifConj.NovosValores[0]) < expG.Max(x => x.NumConj)) modifConj.SetValores(expG.Max(x => x.NumConj));

                expG.GroupBy(x => x.NumConj).ToList().ForEach(x =>
                {

                    if (modifMaq.ContainsKey(x.Key))
                    {
                        modifMaq[x.Key].SetValores(int.Parse(modifMaq[x.Key].NovosValores[0]) + x.Count(), x.Key);
                    }
                    else
                    {
                        modifs.Insert(modifs.IndexOf(modifConj) + 1,
                            new Compass.CommomLibrary.ModifDatNW.ModifLine()
                            {
                                Usina = expG.Key,
                                Chave = "NUMMAQ",
                                NovosValores = new string[] { x.Count().ToString(), x.Key.ToString() }
                            });
                    }
                }
                );

                expG.ToList().ForEach(x => exphs.Remove(x));
            }

            foreach (var modif in modifs.GroupBy(x => x.Usina).ToList())
            {
                if (!modif.Any(x => x.Chave != "USINA")) modifs.Remove(modif.First());
                else if (!modif.Any(x => x.Chave == "USINA")) modifs.Insert(modifs.IndexOf(modif.First()) - 1,
                      new Compass.CommomLibrary.ModifDatNW.ModifLine()
                      {
                          Usina = modif.Key,
                          Chave = "USINA",
                          NovosValores = new string[] { modif.Key.ToString() }
                      });
            }

            foreach (var u in confhds)
            {
                if (!exphs.Any(x => x.Cod == u.Cod) && u.Situacao != "NC") u.Situacao = "EX";
                u.Modif = modifs.Any(x => x.Usina == u.Cod);
            }
        }

        private void IncrementarSistema(DeckNewave deck)
        {


            var sistema = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as Compass.CommomLibrary.SistemaDat.SistemaDat;

            foreach (var item in sistema.Intercambio.Where(x => x.Ano.HasValue).ToList())
            {

                if (item.Ano < deck.Dger.AnoEstudo)
                {
                    sistema.Intercambio.Remove(item);
                }
                else if (item.Ano == deck.Dger.AnoEstudo)
                {
                    for (int i = 1; i < deck.Dger.MesEstudo; i++)
                    {
                        item[i] = 0;
                    }
                }
                if (deck.Dger.MesEstudo == 1 && item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2)
                {
                    var novoAno = item.Clone() as Compass.CommomLibrary.SistemaDat.IntLine;
                    novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;
                    sistema.Intercambio.Insert(
                        sistema.Intercambio.IndexOf(item) + 1,
                        novoAno);
                }
            }

            SobrescreverIntercambios(deck);


            foreach (var item in sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList())
            {

                if (item.Ano < deck.Dger.AnoEstudo)
                {
                    sistema.Mercado.Remove(item);
                }
                else if (item.Ano == deck.Dger.AnoEstudo)
                {
                    for (int i = 1; i < deck.Dger.MesEstudo; i++)
                    {
                        item[i] = 0;
                    }
                }
                if (deck.Dger.MesEstudo == 1 && item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2)
                {
                    var novoAno = item.Clone() as Compass.CommomLibrary.SistemaDat.MerLine;
                    novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                    for (int m = 1; m <= 12; m++)
                    {
                        double crescimento = 0;
                        //            2019                         2019+5-2=2022
                        for (int z = deck.Dger.AnoEstudo + 1; z < deck.Dger.AnoEstudo + deck.Dger.NumeroAnosEstudo - 1; z++)
                        {
                            crescimento = crescimento +
                                sistema.Mercado.Where(x => x.Ano == z && x.Mercado == item.Mercado).First()[m]
                                /
                                sistema.Mercado.Where(x => x.Ano == z - 1 && x.Mercado == item.Mercado).First()[m];
                        }
                        crescimento = crescimento / (deck.Dger.NumeroAnosEstudo - 2);
                        novoAno[m] = Math.Round(novoAno[m] * crescimento, 0);
                    }

                    sistema.Mercado.Insert(
                        sistema.Mercado.IndexOf(item) + 1,
                        novoAno);

                    var pos = sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEnePosLine && x.Mercado == item.Mercado).FirstOrDefault();
                    if (pos != null)
                    {
                        for (int m = 1; m <= 12; m++)
                        {
                            pos[m] = novoAno[m];
                        }
                    }
                }
            }
            SobrescreverSistemas(deck);

            //  foreach (var item in sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList())
            foreach (var item in sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.PeqEneLine).ToList())
            {

                if (item.Ano < deck.Dger.AnoEstudo)
                {
                    sistema.Pequenas.Remove(item);
                }
                else if (item.Ano == deck.Dger.AnoEstudo)
                {
                    for (int i = 1; i < deck.Dger.MesEstudo; i++)
                    {
                        item[i] = 0;
                    }
                }
                if (deck.Dger.MesEstudo == 1 && item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2)
                {
                    // var novoAno = item.Clone() as Compass.CommomLibrary.SistemaDat.MerLine;
                    var novoAno = item.Clone() as Compass.CommomLibrary.SistemaDat.PeqLine;
                    novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                    for (int m = 1; m <= 12; m++)
                    {
                        double crescimento = 0;
                        //            2019                         2019+5-2=2022

                        if (item.Mercado == 1)
                        {
                            for (int z = deck.Dger.AnoEstudo + 1; z < deck.Dger.AnoEstudo + deck.Dger.NumeroAnosEstudo - 1; z++)
                            {
                                crescimento = crescimento +
                                    sistema.Pequenas.Where(x => x.Ano == z && x.Mercado == item.Mercado).First()[m]
                                    /
                                    sistema.Pequenas.Where(x => x.Ano == z - 1 && x.Mercado == item.Mercado).First()[m];
                            }

                            crescimento = crescimento / (deck.Dger.NumeroAnosEstudo - 2);
                        }
                        else crescimento = 1;

                        novoAno[m] = Math.Round(novoAno[m] * crescimento, 0);
                    }

                    sistema.Pequenas.Insert(
                        sistema.Pequenas.IndexOf(item) + 1,
                        novoAno);

                    //  var pos = sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEnePosLine && x.Mercado == item.Mercado).FirstOrDefault();
                    var pos = sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.PeqEnePosLine && x.Mercado == item.Mercado).FirstOrDefault();
                    if (pos != null)
                    {
                        for (int m = 1; m < 12; m++)
                        {
                            pos[m] = novoAno[m];
                        }
                    }
                }
            }
            if (deck.Dger.MesEstudo == 1)
            {

                var patamares = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;

                var nPat = patamares.NumeroPatamares;

                foreach (var item in patamares.Duracao.ToList())
                {
                    if (item.Ano < deck.Dger.AnoEstudo)
                    {
                        patamares.Duracao.Remove(item);
                    }
                    else if (item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2)
                    {
                        var novoAno = item.Clone() as Compass.CommomLibrary.PatamarDat.DuracaoLine;
                        novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                        patamares.Duracao.Insert(
                        patamares.Duracao.IndexOf(item) + nPat,
                        novoAno);
                    }
                }

                foreach (var item in patamares.Carga.Where(x => x is Compass.CommomLibrary.PatamarDat.CargaEneLine).ToList())
                {
                    if (item.Ano < deck.Dger.AnoEstudo)
                    {
                        patamares.Carga.Remove(item);
                    }
                    else if (item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2)
                    {
                        var novoAno = item.Clone() as Compass.CommomLibrary.PatamarDat.CargaEneLine;
                        novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                        patamares.Carga.Insert(
                        patamares.Carga.IndexOf(item) + nPat,
                        novoAno);
                    }
                }

                foreach (var item in patamares.Intercambio.Where(x => x is Compass.CommomLibrary.PatamarDat.IntABLine).ToList())
                {
                    if (item.Ano < deck.Dger.AnoEstudo)
                    {
                        patamares.Intercambio.Remove(item);
                    }
                    else if (item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2)
                    {
                        var novoAno = item.Clone() as Compass.CommomLibrary.PatamarDat.IntABLine;
                        novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                        patamares.Intercambio.Insert(
                        patamares.Intercambio.IndexOf(item) + nPat,
                        novoAno);
                    }
                }

                foreach (var item in patamares.Nao_Simuladas.Where(x => x is Compass.CommomLibrary.PatamarDat.UNSABLine).ToList())
                {
                    if (item.Ano < deck.Dger.AnoEstudo)
                    {
                        patamares.Nao_Simuladas.Remove(item);
                    }
                    else if (item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2)
                    {
                        var novoAno = item.Clone() as Compass.CommomLibrary.PatamarDat.UNSABLine;
                        novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                        patamares.Nao_Simuladas.Insert(
                        patamares.Nao_Simuladas.IndexOf(item) + nPat,
                        novoAno);
                    }
                }
            }
        }

        private void AlterarModif(DeckNewave deck)
        {//TODO colocar logica para inserir linhas ou mudar com os minemonicos sem a data de duração ex NUMMAQ
            bool nwHibrido = this.NwHibrido;
            var modifs = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;
            var modifFile = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Path;
            List<string> minemonicosSemData = new List<string> { "NUMCNJ", "NUMMAQ", "POTEFE" };

            foreach (var dad in this.Modifs.Where(x => x.MesEstudo == deck.Dger.MesEstudo && ((x.Mes >= deck.Dger.MesEstudo && x.Ano >= deck.Dger.AnoEstudo) || (x.Mes < deck.Dger.MesEstudo && x.Ano > deck.Dger.AnoEstudo))).ToList())
            {
                if (dad.Usina == 251)
                {

                }
                if (minemonicosSemData.Any(x => x == dad.Minemonico))
                {
                    if (!modifs.Any(x => x.Usina == dad.Usina))
                    {
                        modifs.Add(new Compass.CommomLibrary.ModifDatNW.ModifLine()
                        {
                            Usina = dad.Usina,
                            Chave = "USINA",
                            NovosValores = new string[] { dad.Usina.ToString() }
                        });
                    }

                    if (dad.Minemonico == "NUMCNJ")
                    {
                        var modifline = modifs.Where(x => x.Usina == dad.Usina && x.Chave == dad.Minemonico).FirstOrDefault();
                        if (modifline != null)
                        {
                            modifline.SetValores(dad.ModifCampos[0].ToString().Replace(',', '.'));
                        }
                        else
                        {
                            var modifLineUsi = modifs.Where(x => x.Usina == dad.Usina && x.Chave == "USINA").FirstOrDefault();
                            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            newModifLine.SetValores(dad.ModifCampos[0].ToString().Replace(',', '.'));
                            newModifLine.Chave = dad.Minemonico;
                            newModifLine.Usina = dad.Usina;
                            int index = modifs.IndexOf(modifLineUsi) + 1;

                            modifs.Insert(index, newModifLine);
                        }
                    }
                    else
                    {
                        var modifsLista = modifs.Where(x => x.Usina == dad.Usina && x.Chave == dad.Minemonico).ToList();


                        //var modifline = modifs.Where(x => x.Usina == dad.Usina && x.Chave == dad.Minemonico && Convert.ToInt32(x.NovosValores[1]) <= dad.ModifCampos[1]).OrderByDescending(x => Convert.ToInt32( x.NovosValores[1])).FirstOrDefault();
                        var modifline = modifsLista.Where(x => Convert.ToInt32(x.NovosValores[1]) <= dad.ModifCampos[1]).OrderByDescending(x => Convert.ToInt32(x.NovosValores[1])).FirstOrDefault();
                        if (modifline != null)
                        {
                            if (Convert.ToInt32(modifline.NovosValores[1]) < dad.ModifCampos[1])
                            {
                                var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();

                                newModifLine.SetValores(dad.ModifCampos[0].ToString().Replace(',', '.'), dad.ModifCampos[1].ToString().Replace(',', '.'));
                                newModifLine.Chave = dad.Minemonico;
                                newModifLine.Usina = dad.Usina;
                                int index = modifs.IndexOf(modifline) + 1;
                                modifs.Insert(index, newModifLine);
                            }
                            else
                            {
                                modifline.SetValores(dad.ModifCampos[0].ToString().Replace(',', '.'), dad.ModifCampos[1].ToString().Replace(',', '.'));
                            }
                        }
                        else
                        {
                            var modifLineUsi = modifs.Where(x => x.Usina == dad.Usina && x.Chave == "USINA").FirstOrDefault();
                            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            newModifLine.SetValores(dad.ModifCampos[0].ToString().Replace(',', '.'), dad.ModifCampos[1].ToString().Replace(',', '.'));
                            newModifLine.Chave = dad.Minemonico;
                            newModifLine.Usina = dad.Usina;
                            int index = modifs.IndexOf(modifLineUsi) + 1;

                            modifs.Insert(index, newModifLine);
                        }
                    }

                }
                else if (dad.Minemonico != "TURBMAXT")
                {
                    DateTime data = new DateTime(dad.Ano, dad.Mes, 1);
                    var modifline = modifs.Where(x => x.Usina == dad.Usina && x.Chave == dad.Minemonico && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                    if (modifline != null)//só alterar se ja existente, NÃO incluir caso não exista
                    {
                        if (modifline.DataModif < data)
                        {

                            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            if (dad.Minemonico == "VMINT" || dad.Minemonico == "VMAXT")
                            {
                                newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'), "'%'");
                            }
                            else
                            {
                                newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'));
                            }
                            newModifLine.Chave = dad.Minemonico;
                            newModifLine.Usina = dad.Usina;
                            int index = modifs.IndexOf(modifline) + 1;
                            modifs.Insert(index, newModifLine);

                        }
                        else
                        {
                            if (dad.Minemonico == "VMINT" || dad.Minemonico == "VMAXT")
                            {
                                modifline.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'), "'%'");
                            }
                            else
                            {
                                modifline.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'));
                            }
                        }
                    }

                }

            }

            if (NwHibrido)
            {
                var usinasTurbmaxt = modifs.Where(x => x.Chave == "TURBMAXT").Select(x => x.Usina).Distinct();
                List<Compass.CommomLibrary.ModifDatNW.ModifLine> remover = new List<Compass.CommomLibrary.ModifDatNW.ModifLine>();

                foreach (var usiT in usinasTurbmaxt)
                {
                    var modifsremove = modifs.Where(x => x.Usina == usiT && x.Chave == "TURBMAXT").ToList();
                    if (modifsremove.Count() == 1 && modifsremove[0].ValorModif == 99999)
                    {
                        remover.Add(modifsremove[0]);
                    }
                }

                remover.ForEach(x => modifs.Remove(x));

                foreach (var dad in this.Modifs.Where(x => x.MesEstudo == deck.Dger.MesEstudo && ((x.Mes >= deck.Dger.MesEstudo && x.Ano >= deck.Dger.AnoEstudo) || (x.Mes < deck.Dger.MesEstudo && x.Ano > deck.Dger.AnoEstudo))).ToList())
                {
                    if (dad.Minemonico == "TURBMAXT")
                    {

                        DateTime data = new DateTime(dad.Ano, dad.Mes, 1);

                        if (!modifs.Any(x => x.Usina == dad.Usina))
                        {
                            modifs.Add(new Compass.CommomLibrary.ModifDatNW.ModifLine()
                            {
                                Usina = dad.Usina,
                                Chave = "USINA",
                                NovosValores = new string[] { dad.Usina.ToString() }
                            });


                        }
                        var modifline = modifs.Where(x => x.Usina == dad.Usina && x.Chave == dad.Minemonico && x.DataModif <= data).OrderByDescending(x => x.DataModif).FirstOrDefault();
                        if (modifline != null)
                        {
                            if (modifline.DataModif < data)
                            {

                                var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                                if (dad.Minemonico == "VMINT" || dad.Minemonico == "VMAXT")
                                {
                                    newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'), "'%'");
                                }
                                else
                                {
                                    newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'));
                                }
                                newModifLine.Chave = dad.Minemonico;
                                newModifLine.Usina = dad.Usina;
                                int index = modifs.IndexOf(modifline) + 1;
                                modifs.Insert(index, newModifLine);

                            }
                            else
                            {
                                if (dad.Minemonico == "VMINT" || dad.Minemonico == "VMAXT")
                                {
                                    modifline.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'), "'%'");
                                }
                                else
                                {
                                    modifline.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'));
                                }
                            }
                        }
                        else
                        {
                            var modifLineUsi = modifs.Where(x => x.Usina == dad.Usina && x.Chave == "USINA").FirstOrDefault();
                            var newModifLine = new Compass.CommomLibrary.ModifDatNW.ModifLine();
                            newModifLine.SetValores(data.Month.ToString(), data.Year.ToString(), dad.Valor.ToString().Replace(',', '.'));
                            newModifLine.Chave = dad.Minemonico;
                            newModifLine.Usina = dad.Usina;
                            int index = modifs.IndexOf(modifLineUsi) + 1;

                            modifs.Insert(index, newModifLine);
                        }
                    }

                }
            }
            //todo excluir os turbmax caso só exista dados com 99999

            var usinasTurbmaxtFinal = modifs.Where(x => x.Chave == "TURBMAXT").Select(x => x.Usina).Distinct();
            List<Compass.CommomLibrary.ModifDatNW.ModifLine> removerFinal = new List<Compass.CommomLibrary.ModifDatNW.ModifLine>();

            foreach (var usiT in usinasTurbmaxtFinal)
            {
                var modifsremove = modifs.Where(x => x.Usina == usiT && x.Chave == "TURBMAXT").ToList();
                if (modifsremove.All(x => x.ValorModif == 99999))
                {
                    modifsremove.ForEach(x => removerFinal.Add(x));
                }
            }

            removerFinal.ForEach(x => modifs.Remove(x));

            removerFinal.Clear();

            var usinaSemMine = modifs.Select(x => x.Usina).Distinct();

            foreach (var usiT in usinaSemMine)
            {
                var modifsremove = modifs.Where(x => x.Usina == usiT).ToList();
                if (modifsremove.Count() == 1 && modifsremove[0].Chave.ToUpper().Trim() == "USINA")
                {
                    removerFinal.Add(modifsremove[0]);
                }
            }
            removerFinal.ForEach(x => modifs.Remove(x));

            //
            modifs.SaveToFile(filePath: modifFile);

        }
        private void SobrescreverSistemas(DeckNewave deck)
        {
            DeckMediaBase = new DeckNewave();
            DeckMediaBase.EstudoPai = this;

            DeckMediaBase.GetFiles(Origem);
            var sistemaBase = DeckMediaBase[Compass.CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as Compass.CommomLibrary.SistemaDat.SistemaDat;

            var sistema = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as Compass.CommomLibrary.SistemaDat.SistemaDat;
            if (this.MERCADO.Count() != 0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    var sistAnt = sistemaBase.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList();
                    foreach (var sist in sistAnt.Where(x => x.Mercado == i && x.Ano == deck.Dger.AnoEstudo).ToList())
                    {
                        var item = sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList();
                        foreach (var dado in item)
                        {
                            if (dado.Ano == sist.Ano && dado.Mercado == sist.Mercado)
                            {
                                dado[deck.Dger.MesEstudo] = sist[deck.Dger.MesEstudo];
                            }
                        }

                    }
                    foreach (var mercx in this.MERCADO.Where(x => x.SubMercado == i && x.MesEstudo == deck.Dger.MesEstudo).ToList())
                    {
                        var item = sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList();
                        foreach (var dado in item)
                        {
                            if (dado.Ano == mercx.AnoIni && dado.Mercado == mercx.SubMercado)
                            {
                                dado[Convert.ToInt32(mercx.Mes)] = mercx.Carga;
                            }
                        }



                        //var indice = adtermDat.Despachos.IndexOf(adt);
                        //sistema.Mercado.
                        //if (adtx.Mes == datOp.Month)
                        //{

                        //    adtermDat.Despachos[indice + 1].Lim_P1 = adtx.RestricaoP1;
                        //    adtermDat.Despachos[indice + 1].Lim_P2 = adtx.RestricaoP2;
                        //    adtermDat.Despachos[indice + 1].Lim_P3 = adtx.RestricaoP3;

                        //}
                        //else if (adtx.Mes == datNex.Month)
                        //{

                        //    adtermDat.Despachos[indice + 2].Lim_P1 = adtx.RestricaoP1;
                        //    adtermDat.Despachos[indice + 2].Lim_P2 = adtx.RestricaoP2;
                        //    adtermDat.Despachos[indice + 2].Lim_P3 = adtx.RestricaoP3;

                        //}

                    }
                }

            }
        }


        private void SobrescreverIntercambios(DeckNewave deck)
        {
            try
            {
                var patamares = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
                var sistema = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as Compass.CommomLibrary.SistemaDat.SistemaDat;

                //if (deck.Dger.MesEstudo ==12)
                //{
                //    var test = this.Intercambios.Where(x => x.MesEstudo == deck.Dger.MesEstudo && x.AnoIni == deck.Dger.AnoEstudo).ToList();
                //    var test1 = this.Intercambios.Where(x => x.MesEstudo == deck.Dger.MesEstudo && x.AnoIni >= deck.Dger.AnoEstudo).ToList();
                //    var test12 = this.Intercambios.Where(x => x.MesEstudo == deck.Dger.MesEstudo && x.AnoIni > deck.Dger.AnoEstudo).ToList();

                //}

                //foreach (var intercambio in this.Intercambios.Where(x => x.MesEstudo == deck.Dger.MesEstudo && x.AnoIni == deck.Dger.AnoEstudo))
                foreach (var intercambio in this.Intercambios.Where(x => x.MesEstudo == deck.Dger.MesEstudo && x.AnoIni >= deck.Dger.AnoEstudo))
                {

                    var blocoIntercambio = sistema.Intercambio.Where(i => i.SubmercadoA == intercambio.Intercambios.Item1 && i.SubmercadoB == intercambio.Intercambios.Item2);

                    var dataInicio = new DateTime(intercambio.AnoIni, intercambio.MesIni, 1);
                    var dataFim = new DateTime(intercambio.AnoFim, intercambio.MesFim, 1);

                    for (DateTime dataModif = dataInicio; dataModif <= dataFim; dataModif = dataModif.AddMonths(1))
                    {


                        var patTemp = patamares.Duracao.Where(d => d.Ano == dataModif.Year);

                        var intMedio =
                        intercambio.RestricaoP1 * patTemp.First(p => p.Patamar == 1)[dataModif.Month + 1]
                        + intercambio.RestricaoP2 * patTemp.First(p => p.Patamar == 2)[dataModif.Month + 1]
                        + intercambio.RestricaoP3 * patTemp.First(p => p.Patamar == 3)[dataModif.Month + 1];


                        blocoIntercambio.First(i => i.Ano == dataModif.Year)[dataModif.Month] = intMedio;


                        var intTemp = patamares.Intercambio
                            .Where(i => i.Ano == dataModif.Year)
                            .Where(i => i.SubmercadoA == intercambio.Intercambios.Item1 && i.SubmercadoB == intercambio.Intercambios.Item2);


                        intTemp.First(x => x.Patamar == 1)[dataModif.Month] = intercambio.RestricaoP1 / intMedio;
                        intTemp.First(x => x.Patamar == 2)[dataModif.Month] = intercambio.RestricaoP2 / intMedio;
                        intTemp.First(x => x.Patamar == 3)[dataModif.Month] = intercambio.RestricaoP3 / intMedio;

                    }
                }

                //
                //if (deck.Dger.MesEstudo == 12)
                //{
                //    foreach (var intercambio in this.Intercambios.Where(x => x.MesEstudo == deck.Dger.MesEstudo && x.AnoIni > deck.Dger.AnoEstudo))
                //    {

                //        var blocoIntercambio = sistema.Intercambio.Where(i => i.SubmercadoA == intercambio.Intercambios.Item1 && i.SubmercadoB == intercambio.Intercambios.Item2);

                //        var dataInicio = new DateTime(intercambio.AnoIni, intercambio.MesIni, 1);
                //        var dataFim = new DateTime(intercambio.AnoFim, intercambio.MesFim, 1);

                //        for (DateTime dataModif = dataInicio; dataModif <= dataFim; dataModif = dataModif.AddMonths(1))
                //        {


                //            var patTemp = patamares.Duracao.Where(d => d.Ano == dataModif.Year);

                //            var intMedio =
                //            intercambio.RestricaoP1 * patTemp.First(p => p.Patamar == 1)[dataModif.Month + 1]
                //            + intercambio.RestricaoP2 * patTemp.First(p => p.Patamar == 2)[dataModif.Month + 1]
                //            + intercambio.RestricaoP3 * patTemp.First(p => p.Patamar == 3)[dataModif.Month + 1];


                //            blocoIntercambio.First(i => i.Ano == dataModif.Year)[dataModif.Month] = intMedio;


                //            var intTemp = patamares.Intercambio
                //                .Where(i => i.Ano == dataModif.Year)
                //                .Where(i => i.SubmercadoA == intercambio.Intercambios.Item1 && i.SubmercadoB == intercambio.Intercambios.Item2);


                //            intTemp.First(x => x.Patamar == 1)[dataModif.Month] = intercambio.RestricaoP1 / intMedio;
                //            intTemp.First(x => x.Patamar == 2)[dataModif.Month] = intercambio.RestricaoP2 / intMedio;
                //            intTemp.First(x => x.Patamar == 3)[dataModif.Month] = intercambio.RestricaoP3 / intMedio;

                //        }
                //    }
                //}

                //
            }
            catch (Exception e)
            {
                e.ToString();
            }

        }


        private void IncrementarEarm(DeckNewave deck)
        {

            if (ConfighBase != null)
            {

                double[] earmMeta = new double[] {
                    this.VolumesPO[1][deck.Dger.MesEstudo - 1],
                    this.VolumesPO[2][deck.Dger.MesEstudo - 1],
                    this.VolumesPO[3][deck.Dger.MesEstudo - 1],
                    this.VolumesPO[4][deck.Dger.MesEstudo - 1]};



                double[] earmMax = ConfighBase.GetEarmsMax();

                var EarmMax = Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.Values.Distinct().Select(ree => new
                Tuple<int, double>(

                    int.Parse(ree.Split('-')[0].Trim()),
                    ConfighBase.Usinas
                        .Where(u => Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.ContainsKey(u.Cod) && Compass.CommomLibrary.Decomp.ConfigH.uhe_ree[u.Cod] == ree)
                        .Sum(u => u.EnergiaArmazenada)
                )).ToList();


                ConfighBase.ReloadUH();

                //atualizar UH

                Compass.Services.Reservatorio.SetUHBlock(ConfighBase, earmMeta, earmMax);

                double[] earmFinal = ConfighBase.GetEarms();

                var EarmBase = Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.Values.Distinct().Select(ree => new
                Tuple<int, double>(

                    int.Parse(ree.Split('-')[0].Trim()),
                    ConfighBase.Usinas
                        .Where(u => Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.ContainsKey(u.Cod) && Compass.CommomLibrary.Decomp.ConfigH.uhe_ree[u.Cod] == ree)
                        .Sum(u => u.EnergiaArmazenada)
                )).ToList();

                var reedat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.ree].Document as Compass.CommomLibrary.ReeDat.ReeDat;

                deck.Dger.CalculaEarmInicial = false;
                deck.Dger.Earms =
                reedat.ToList().Select(ree =>

                    EarmMax.Where(x => x.Item1 == ree.Numero).Sum(x => x.Item2) > 0 ?
                    100 * (EarmBase.Where(x => x.Item1 == ree.Numero).Sum(x => x.Item2) /
                    EarmMax.Where(x => x.Item1 == ree.Numero).Sum(x => x.Item2))
                    : 0d
                ).ToArray();
            }
            else
            {

                deck.Dger.CalculaEarmInicial = false;
                int i = 0;
                var earms = new double[deck.Ree.Count];

                foreach (var ree in deck.Ree)
                {
                    earms[i] = this.VolumesPO[ree.Submercado][deck.Dger.DataEstudo.Month - 1] * 100;
                    i++;
                }

                deck.Dger.Earms = earms;
            }
        }

        private void IncrementarVAZAO(DeckNewave deck)
        {

            var data = deck.Dger.DataEstudo;

            var vaspast = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.vazpast].Document as Compass.CommomLibrary.Vazpast.Vazpast;

            var postosdat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.postos].Document as Compass.CommomLibrary.PostosDat.PostosDat;
            foreach (var p in postosdat.Data) p.FinalHistorico = data.Year - 2;


            foreach (var vp in vaspast.Conteudo)
            {
                vp[data] = this.PrevisaoVazao[vp.Posto][data.Month - 1];
            }
        }

        public void IncrementarCSV_LIBS(DeckNewave deck)
        {
            DateTime dataEstudo = deck.Dger.DataEstudo;

            var restsEletricasCSV = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.restelcsv] != null ? deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.restelcsv].Document as Compass.CommomLibrary.RestElCSV.RestElCSV : null;

            if (restsEletricasCSV != null)
            {
                var restsEletricasCSVFile = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.restelcsv].Path;

                var restsPlan = this.Restelecsv.Where(x => x.MesEstudo == dataEstudo.Month).ToList();

                if (restsPlan.Count() > 0)
                {
                    var formulas = restsPlan.Select(x => x.Formula).Distinct();
                    foreach (var form in formulas)
                    {
                        int codigoRest = restsEletricasCSV.BlocoRe.Where(x => x.Formula == form).Select(x => x.CodRE).FirstOrDefault();
                        var blocoPatamres = restsEletricasCSV.BlocoReLimFormPat.Where(x => x.CodRE == codigoRest).ToList();

                        if (blocoPatamres.Count() > 0)
                        {
                            DateTime dataIni = restsPlan.Where(x => x.Formula == form).Select(x => x.DataIni).Min();
                            DateTime dataFim = restsPlan.Where(x => x.Formula == form).Select(x => x.DataFim).Max();

                            var resthoriz = restsEletricasCSV.BlocoReHoriz.Where(x => x.CodRE == codigoRest).FirstOrDefault();
                            if (resthoriz != null)//acerta o horizonte de atuação da restrição no bloco horizonte
                            {
                                resthoriz.DataIni = dataIni;
                                resthoriz.DataFim = dataFim;
                            }
                            var restPlanAlvo = restsPlan.Where(x => x.Formula == form).OrderBy(x => x.DataIni).ToList();

                            foreach (var rpa in restPlanAlvo)
                            {
                                var patLine = blocoPatamres.Where(x => x.DataIni <= rpa.DataIni).OrderByDescending(x => x.DataIni).OrderByDescending(x => x.Patamar).FirstOrDefault();
                                if (patLine != null)//encontrou a linha com data menor ou igual a restalvo 
                                {
                                    int index = restsEletricasCSV.BlocoReLimFormPat.IndexOf(patLine);
                                    if (patLine.DataIni < rpa.DataIni)
                                    {
                                        if (patLine.DataFim >= rpa.DataIni)//trava a datafinal das rest ja existentes com uma data menor que a dta inicial da rest que ser incluida
                                        {
                                            foreach (var item in blocoPatamres.Where(x => x.DataIni == patLine.DataIni))
                                            {
                                                item.DataFim = rpa.DataIni.AddMonths(-1);
                                            }
                                        }
                                        for (int i = 1; i <= 3; i++)
                                        {
                                            //var newline = new Compass.CommomLibrary.RestElCSV.ReLimFormLine(patLine.LineCSV);
                                            var newline = restsEletricasCSV.BlocoReLimFormPat.CreateLineCSV(patLine.LineCSV);
                                            newline.LineCSV = patLine.LineCSV;
                                            newline.DataIni = rpa.DataIni;
                                            newline.DataFim = rpa.DataFim;
                                            newline.Patamar = i;
                                            newline.LimInf = i == 1 ? rpa.LimInfPt1 : i == 2 ? rpa.LimInfPt2 : rpa.LimInfPt3;
                                            newline.LimSup = i == 1 ? rpa.LimSupPt1 : i == 2 ? rpa.LimSupPt2 : rpa.LimSupPt3;

                                            restsEletricasCSV.BlocoReLimFormPat.Insert(index + i, newline);
                                        }
                                    }
                                    else if (patLine.DataIni == rpa.DataIni)
                                    {
                                        for (int i = 1; i <= 3; i++)
                                        {
                                            var line = blocoPatamres.Where(x => x.DataIni == patLine.DataIni && x.Patamar == i).FirstOrDefault();
                                            if (line != null)
                                            {
                                                line.DataFim = rpa.DataFim;
                                                line.LimInf = i == 1 ? rpa.LimInfPt1 : i == 2 ? rpa.LimInfPt2 : rpa.LimInfPt3;
                                                line.LimSup = i == 1 ? rpa.LimSupPt1 : i == 2 ? rpa.LimSupPt2 : rpa.LimSupPt3;
                                            }
                                        }

                                    }
                                }
                                else// nao encontrou uma linha com data menor ou igual a restalvo, vai criar uma linha e inserir acima das existentes
                                {
                                    var firstLine = blocoPatamres.First();
                                    int index = restsEletricasCSV.BlocoReLimFormPat.IndexOf(firstLine);

                                    for (int i = 3; i >= 1; i--)
                                    {
                                        //var newline = new Compass.CommomLibrary.RestElCSV.ReLimFormLine(firstLine.LineCSV);
                                        var newline = restsEletricasCSV.BlocoReLimFormPat.CreateLineCSV(firstLine.LineCSV);
                                        newline.LineCSV = firstLine.LineCSV;
                                        newline.DataIni = rpa.DataIni;
                                        newline.DataFim = rpa.DataFim;
                                        newline.Patamar = i;
                                        newline.LimInf = i == 1 ? rpa.LimInfPt1 : i == 2 ? rpa.LimInfPt2 : rpa.LimInfPt3;
                                        newline.LimSup = i == 1 ? rpa.LimSupPt1 : i == 2 ? rpa.LimSupPt2 : rpa.LimSupPt3;

                                        restsEletricasCSV.BlocoReLimFormPat.Insert(index + i, newline);
                                    }
                                }
                            }
                        }

                    }
                }

                var cods = restsEletricasCSV.BlocoRe.Select(x => x.CodRE).Distinct().ToList();
                restsEletricasCSV.BlocoReHoriz.Where(x => x.DataIni < dataEstudo).ToList().ForEach(x => x.DataIni = dataEstudo);
                restsEletricasCSV.BlocoReLimFormPat.Where(x => x.DataIni < dataEstudo).ToList().ForEach(x => x.DataIni = dataEstudo);
                restsEletricasCSV.BlocoReLimFormPat.Where(x => x.DataFim < dataEstudo).ToList().ForEach(x => restsEletricasCSV.BlocoReLimFormPat.Remove(x));

                foreach (var cod in cods)
                {
                    var lineHorz = restsEletricasCSV.BlocoReHoriz.Where(x => x.CodRE == cod).FirstOrDefault();
                    if (lineHorz != null)
                    {
                        restsEletricasCSV.BlocoReLimFormPat.Where(x => x.CodRE == lineHorz.CodRE && x.DataFim > lineHorz.DataFim).ToList().ForEach(x => restsEletricasCSV.BlocoReLimFormPat.Remove(x));
                        restsEletricasCSV.BlocoReLimFormPat.Where(x => x.CodRE == lineHorz.CodRE && x.DataIni < lineHorz.DataIni).ToList().ForEach(x => restsEletricasCSV.BlocoReLimFormPat.Remove(x));
                    }

                    var blocos = restsEletricasCSV.BlocoReLimFormPat.Where(x => x.CodRE == cod).ToList();
                    if (blocos.Count() == 0)
                    {
                        restsEletricasCSV.BlocoReHoriz.Where(x => x.CodRE == cod).ToList().ForEach(x => restsEletricasCSV.BlocoReHoriz.Remove(x));
                        restsEletricasCSV.BlocoRe.Where(x => x.CodRE == cod).ToList().ForEach(x => restsEletricasCSV.BlocoRe.Remove(x));

                    }
                }

                restsEletricasCSV.BlocoReLimFormPat.Where(x => x.DataFim < dataEstudo).ToList().ForEach(x => restsEletricasCSV.BlocoReLimFormPat.Remove(x));



                restsEletricasCSV.SaveToFile(filePath: restsEletricasCSVFile);
            }

            //var eolCad = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicacad] != null ? deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicacad].Document as Compass.CommomLibrary.EolicaNW.EolicaCad : null;

            //var eolConfig = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicaconfig] != null ? deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicaconfig].Document as Compass.CommomLibrary.EolicaNW.EolicaConfig : null;

            //var eolFte = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicafte] != null ? deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicafte].Document as Compass.CommomLibrary.EolicaNW.Eolicafte : null;

            //var eolGer = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicageracao] != null ? deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicageracao].Document as Compass.CommomLibrary.EolicaNW.EolicaGeracao : null;

            //if (eolCad != null)
            //{
            //    var eolcadFile = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicacad].Path;
            //    eolCad.BlocoPeePot.Where(x => x.DataFim < dataEstudo).ToList().ForEach(y => eolCad.BlocoPeePot.Remove(y));
            //    eolCad.SaveToFile(filePath: eolcadFile);
            //}

            //if (eolGer != null)
            //{
            //    var eolGerFile = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicageracao].Path;
            //    eolGer.BlocoGera.Where(x => x.DataFim < dataEstudo).ToList().ForEach(y => eolGer.BlocoGera.Remove(y));
            //    eolGer.SaveToFile(filePath: eolGerFile);
            //}

            //if (eolConfig != null)
            //{
            //    var eolConfigFile = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicaconfig].Path;
            //    eolConfig.BlocoConfig.ToList().ForEach(x => x.DataIni = dataEstudo);
            //    eolConfig.SaveToFile(filePath: eolConfigFile);
            //}

            //if (eolFte != null)
            //{
            //    var eolFteFile = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.eolicafte].Path;
            //    eolFte.Blocofte.ToList().ForEach(x => x.DataIni = dataEstudo);
            //    eolFte.SaveToFile(filePath: eolFteFile);
            //}

        }

        public void IncrementarREEDAT(DeckNewave deck, bool gravar = false)
        {
            var reedat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.ree].Document as Compass.CommomLibrary.ReeDat.ReeDat;

            if (reedat.temFict == true && this.Reedads.Count() > 0)
            {
                int avanco = this.Reedads.First().mesesAvan;
                DateTime newDate = deck.Dger.DataEstudo.AddMonths(avanco);

                foreach (var reeline in reedat)
                {
                    reeline.Mes = newDate.Month;
                    reeline.Ano = newDate.Year;
                }

                //foreach (var reed in this.Reedads.Where(x => x.mesEst == deck.Dger.MesEstudo))
                //{
                //    var reeline = reedat.Where(x => x.Numero == reed.numREE).FirstOrDefault();
                //    if (reeline != null)
                //    {
                //        DateTime newDate = deck.Dger.DataEstudo.AddMonths(reed.mesesAvan);
                //        reeline.Mes = newDate.Month;
                //        reeline.Ano = newDate.Year;
                //    }
                //}
                if (gravar)
                {
                    var reeFile = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.ree].Path;

                    reedat.SaveToFile(filePath: reeFile);
                }
            }

        }
        private void IncrementarRE(DeckNewave deck)
        {

            var reDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.re].Document as Compass.CommomLibrary.ReDat.ReDat;

            if (this.NwHibrido)
            {
                //foreach (var re in reDat.Restricoes.ToList())
                //{
                //    foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == re.Numero).ToList())
                //    {
                //        reDat.Detalhes.Remove(reDet);
                //    }

                //    if (reDat.Detalhes.Where(x => x.Numero == re.Numero).Count() == 0) reDat.Restricoes.Remove(re);
                //}
            }
            else
            {
                foreach (var re in reDat.Restricoes.ToList())
                {
                    foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == re.Numero).ToList())
                    {

                        if (reDet.Inicio < deck.Dger.DataEstudo && reDet.Fim >= deck.Dger.DataEstudo)
                        {
                            reDet.Inicio = deck.Dger.DataEstudo;
                        }
                        else if (reDet.Fim < deck.Dger.DataEstudo)
                        {
                            reDat.Detalhes.Remove(reDet);
                        }
                    }

                    if (reDat.Detalhes.Where(x => x.Numero == re.Numero).Count() == 0) reDat.Restricoes.Remove(re);
                }


                foreach (var rest in this.Restricoes.Where(x => x.MesEstudo == deck.Dger.MesEstudo))
                {

                    //procura restricao
                    var re = reDat.Restricoes.Where(
                        x => String.Join("", x.Valores.Skip(1).Where(y => y != null).OrderBy(y => y).Select(y => y.ToString().Trim()))
                            == String.Join("", rest.Usinas.OrderBy(y => y).Select(y => y.ToString()))
                        ).FirstOrDefault();

                    //se nao exite insere
                    if (re == null)
                    {
                        if (rest.AnoIni >= deck.Dger.AnoEstudo)
                        {
                            re = new Compass.CommomLibrary.ReDat.ReLine()
                            {
                                Numero = reDat.Restricoes.Max(x => x.Numero) + 1
                            };

                            for (int i = 0; i < rest.Usinas.Count; i++)
                            {
                                re[i + 1] = rest.Usinas[i];
                            }

                            reDat.Restricoes.Add(re);


                            var val = new Compass.CommomLibrary.ReDat.ReValLine()
                            {
                                Numero = re.Numero,
                                Patamar = rest.Patamar,
                                ValorRestricao = rest.Restricao,
                                Inicio = new DateTime(rest.AnoIni, rest.MesIni, 1),
                                Fim = new DateTime(rest.AnoFim, rest.MesFim, 1),
                            };

                            reDat.Detalhes.Add(val);
                        }

                    }
                    //altera ou insere novo valor
                    else
                    {

                        var val = new Compass.CommomLibrary.ReDat.ReValLine()
                        {
                            Numero = re.Numero,
                            Patamar = rest.Patamar,
                            ValorRestricao = rest.Restricao,
                            Inicio = new DateTime(rest.AnoIni, rest.MesIni, 1),
                            Fim = new DateTime(rest.AnoFim, rest.MesFim, 1),
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
                }

                var newl = reDat.Detalhes.OrderBy(x => x.Numero).ThenBy(x => x.Inicio).ToList();
                reDat.Detalhes.Clear();
                newl.ForEach(x => reDat.Detalhes.Add(x));

            }


        }

        private void Ajusta_Adterm(DeckNewave deck)
        {
            var reDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.adterm].Document as Compass.CommomLibrary.ReDat.ReDat;

        }

        private Task SetCasoInicial()
        {

            DeckMedia = new DeckNewave();
            DeckMedia.EstudoPai = this;

            DeckMedia.GetFiles(Origem);

            SetNomeDeck(DeckMedia);

            DeckMedia.BaseFolder = System.IO.Path.Combine(Destino, DeckMedia.Dger.AnoEstudo.ToString("0000") + DeckMedia.Dger.MesEstudo.ToString("00"));

            DeckMedia.Dger.Flags = new int[] { 1, 1, 1, 0, 0 };


            if (DeckMedia.Dger.TipoTendenciaHidrologia == 2)
            {//atualizar mês atual com vazao prevista para calcular ENA e usá-la no newdesp

                IncrementarVAZAO(DeckMedia);
            }


            IncrementarREEDAT(DeckMedia, false);
            IncrementarRE(DeckMedia);
            IncrementarAdterm(DeckMedia);
            IncrementarAgrInt(DeckMedia);
            IncrementarEarm(DeckMedia);
            SobrescreverIntercambios(DeckMedia);
            SobrescreverSistemas(DeckMedia);
            DeckMedia.SaveFilesToFolder(DeckMedia.BaseFolder);
            AlterarModif(DeckMedia);
            DeckMedia.EscreverListagemNwlistop();
            Compass.Services.Deck.CorrigeArquivosdat(DeckMedia.BaseFolder);



            string planMemo = Directory.GetFiles(Origem).Where(x => Path.GetFileName(x).StartsWith("Memória de Cálculo", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (planMemo != null && File.Exists(planMemo))
            {
                File.Copy(planMemo, Path.Combine(DeckMedia.BaseFolder, Path.GetFileName(planMemo)), true);
            }

            var path = DeckMedia.BaseFolder;
            //TODO: executar consistencia
            return Task.Factory.StartNew(() =>
            {
                ExecutarConsistencia(path);
                Compass.Services.Deck.CreateDgerNewdesp(path);
            });


        }

        private void CriarDiretorio(string Destino)
        {
            throw new NotImplementedException();
        }

        private void RemoverDiretorio(string Destino)
        {
            throw new NotImplementedException();
        }

        private void ExecutarConsistencia(string destino, string complemento = "")
        {
            bool ret;

            //ret = ConsisteRun(destino, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/newaveCons280003.sh 3");


            //var ret2 = Compass.Services.Linux.Run2(destino, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/newaveCons280003.sh 3", "NewaveConsist", true, true, "hide");// para debug usar essa funçao
            //var ret2 = Compass.Services.Linux.Run2(destino, this.ExecutavelNewave.Replace("cpas_ctl_common", "enercore_ctl_common") + " 3", "NewaveConsist", true, true, "hide");// para debug usar essa funçao


            //var ret = Compass.Services.Linux.Run2(destino, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/FT/newave2812.sh 3", "NewaveConsist", true, true, "hide");// para debug  nw hibrido usar essa funçao


            ///home/producao/PrevisaoPLD/enercore_ctl_common/scripts/FT/newave2812.sh 3
            if (this.ExecutarConsist.ToUpper() == "SERVIDOR")
            {
                ret = Compass.Services.Linux.Run2(destino, this.ExecutavelNewave.Replace("cpas_ctl_common", "enercore_ctl_common") + " 3", $"NewaveConsist{complemento}", true, true, "hide");// para debug usar essa funçao
            }
            else
            {
                ret = Compass.Services.Linux.Run(destino, this.ExecutavelNewave + " 3", $"NewaveConsist{complemento}", true, true, "hide");//para publicar 
            }


            if (!ret)
            {
                throw new Exception("Ocorreu erro na criação e consistência dos decks newaves. Verifique.");
            }

        }

        public bool ConsisteRun(string path, string comando)
        {
            try
            {
                var nameCommand = "DcNwPreli" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var comm = new { CommandName = nameCommand, EnviarEmail = false, WorkingDirectory = path, Command = comando, User = "AutoRun", IgnoreQueue = true };

                var cont = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(comm));
                cont.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");

                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

                //var responseTsk = httpClient.PostAsync("http://ec2-44-201-188-49.compute-1.amazonaws.com:5015/api/Command", cont);
                var responseTsk = httpClient.PostAsync("http://10.206.194.210:5015/api/Command", cont);
                responseTsk.Wait();
                var response = responseTsk.Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                else
                {
                    return true;
                }

            }
            catch (Exception erro)
            {
                return false;
            }
        }
    }
}

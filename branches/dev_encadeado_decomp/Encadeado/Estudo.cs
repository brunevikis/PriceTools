using Encadeado.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encadeado {



    public class Estudo {

        //DeckNewave DeckInicio;
        DeckNewave DeckMedia;

        public string Origem { get; set; }
        public string Destino { get; set; }

        public int MesesAvancar { get; set; }

        public int IteracaoAtual { get; set; }
        public string ExecutavelNewave { get; set; }

        public bool DefinirVolumesPO { get; set; }

        public Dictionary<int, int[]> PrevisaoVazao { get; set; }
        public Dictionary<int, double[]> VolumesPO { get; set; }

        public List<Compass.CommomLibrary.IRE> Restricoes { get; set; }
        //public List<Tuple<int, double>> EarmMax { get; set; }
        //public List<Tuple<int, double>> EarmBase { get; set; }

        public Compass.CommomLibrary.Decomp.ConfigH ConfighBase { get; set; }

        public Estudo() {
            IteracaoAtual = 0;
            VolumesPO = new Dictionary<int, double[]>();
            PrevisaoVazao = new Dictionary<int, int[]>();
        }

        public bool ExecucaoPrincipal() {

            List<Task> consists = new List<Task>();

            consists.Add(SetCasoInicial());
            //List<Task> consists = new List<Task>();

            while (IteracaoAtual++ < MesesAvancar) consists.Add(Iterar());

            Task.WaitAll(consists.ToArray());
            return true;
        }

        private Task Iterar() {

            Incrementar(DeckMedia);
            SetNomeDeck(DeckMedia);

            DeckMedia.SaveFilesToFolder(System.IO.Path.Combine(Destino, DeckMedia.Dger.AnoEstudo.ToString("0000") + DeckMedia.Dger.MesEstudo.ToString("00")));
            DeckMedia.EscreverListagemNwlistop();


            var path = DeckMedia.Folder;
            //TODO: executar consistencia
            return Task.Factory.StartNew(() => {
                ExecutarConsistencia(path);
                Compass.Services.Deck.CreateDgerNewdesp(path);
            });

        }

        private void SetNomeDeck(DeckNewave deck) {
            deck.Dger.NomeEstudo = "Estudo de Previsao de PLD - Mes/Ano: " + deck.Dger.MesEstudo.ToString("00") + "/" + deck.Dger.AnoEstudo.ToString("0000");
        }

        private void Incrementar(DeckNewave deck) {

            deck.Dger.DataEstudo = deck.Dger.DataEstudo.AddMonths(1);

            // Atualizar dados de classes térmicas.

            IncrementarTermicas(deck);
            //IncrementarAversaoRisco(deck);
            IncrementarOutrosUsosAgua(deck);
            IncrementarAgrInt(deck);
            IncrementarHidr(deck);
            IncrementarSistema(deck);

            if (DefinirVolumesPO) //IncrementarMercados(deck);

                IncrementarEarm(deck);

            if (deck.Dger.TipoTendenciaHidrologia == 2) IncrementarVAZAO(deck);
            IncrementarRE(deck);
        }

        private void IncrementarTermicas(DeckNewave deck) {

            var expts = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.expt].Document as Compass.CommomLibrary.ExptDat.ExptDat;
            var manutts = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.manutt].Document as Compass.CommomLibrary.ManuttDat.ManuttDat;
            var confts = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.conft].Document as Compass.CommomLibrary.ConftDat.ConftDat;
            var clasts = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.clast].Document as Compass.CommomLibrary.ClastDat.ClastDat;

            foreach (var modif in clasts.Modifs.ToList()) {
                if (modif.Inicio < deck.Dger.DataEstudo && modif.Fim >= deck.Dger.DataEstudo) {
                    modif.Inicio = deck.Dger.DataEstudo;
                } else if (modif.Fim < deck.Dger.DataEstudo) {
                    clasts.Modifs.Remove(modif);
                }
            }

            foreach (var manutt in manutts.ToList()) {
                if (manutt.DataInicio < deck.Dger.DataEstudo && manutt.DataFim >= deck.Dger.DataEstudo) {
                    manutt.DataInicio = deck.Dger.DataEstudo;
                } else if (manutt.DataFim < deck.Dger.DataEstudo) {
                    manutts.Remove(manutt);
                }
            }

            foreach (var expt in expts.ToList()) {
                if (expt.DataInicio < deck.Dger.DataEstudo && expt.DataFim >= deck.Dger.DataEstudo) {
                    expt.DataInicio = deck.Dger.DataEstudo;

                } else if (expt.DataFim < deck.Dger.DataEstudo) {
                    expts.Remove(expt);
                }
            }

            foreach (var u in confts) {
                if (u.Existente == "EX" || u.Existente == "NC") continue;
                else if (!expts.Any(x => x.Cod == u.Num)) u.Existente = "EX";
                else if (expts.Any(x => x.Cod == u.Num && x.DataInicio == deck.Dger.DataEstudo)) u.Existente = "EE";
            }
        }

        private void IncrementarOutrosUsosAgua(DeckNewave deck) {

            var dsvagua = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.dsvagua].Document as Compass.CommomLibrary.Dsvagua.Dsvagua;

            foreach (var item in dsvagua.ToList()) {

                if (deck.Dger.MesEstudo == 1 && item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2) {
                    var novoAno = item.Clone() as Compass.CommomLibrary.Dsvagua.DsvLine;
                    novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;
                    dsvagua.InsertAfter(item, novoAno);
                } else if (item.Ano == deck.Dger.AnoEstudo) {
                    for (int i = 1; i < deck.Dger.MesEstudo; i++) {
                        item[i + 1] = 0;
                    }
                } else if (item.Ano < deck.Dger.AnoEstudo) {
                    dsvagua.Remove(item);
                }
            }
        }

        private void IncrementarAgrInt(DeckNewave deck) {

            var reDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.agrint].Document as Compass.CommomLibrary.AgrintDat.AgrintDat;

            foreach (var re in reDat.Agrupamentos.ToList()) {
                foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == re.Numero).ToList()) {

                    if (reDet.Inicio < deck.Dger.DataEstudo && reDet.Fim >= deck.Dger.DataEstudo) {
                        reDet.Inicio = deck.Dger.DataEstudo;
                    } else if (reDet.Fim < deck.Dger.DataEstudo) {
                        reDat.Detalhes.Remove(reDet);
                    }
                }

                if (reDat.Detalhes.Where(x => x.Numero == re.Numero).Count() == 0) reDat.Agrupamentos.Remove(re);
            }
        }

        private void IncrementarHidr(DeckNewave deck) {

            var exphs = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.exph].Document as Compass.CommomLibrary.ExphDat.ExphDat;
            var confhds = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.confhd].Document as Compass.CommomLibrary.ConfhdDat.ConfhdDat;
            var modifs = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.modif].Document as Compass.CommomLibrary.ModifDatNW.ModifDatNw;

            //enchimento de volume morto
            foreach (var exph in exphs.Where(x => x.DataEnchimento.HasValue && x.DataEnchimento.Value < deck.Dger.DataEstudo).ToList()) {

                if (exph.DuracaoEnchimento > 1) {
                    exph.DataEnchimento = deck.Dger.DataEstudo;


                    double volume = 100 - exph.VolumePreenchido;
                    volume = (volume / exph.DuracaoEnchimento) + exph.VolumePreenchido;

                    exph.DuracaoEnchimento--;
                    exph.VolumePreenchido = volume;

                    if (exph.DuracaoEnchimento == 0 || exph.VolumePreenchido >= 100) exphs.Remove(exph);
                } else {
                    var confhd = confhds.Where(x => x.Cod == exph.Cod).First();
                    if (confhd.Situacao == "NE") confhd.Situacao = "EE";

                    if (!modifs.Any(x => x.Usina == exph.Cod)) {
                        modifs.Add(new Compass.CommomLibrary.ModifDatNW.ModifLine() {
                            Usina = exph.Cod,
                            Chave = "USINA",
                            NovosValores = new string[] { exph.Cod.ToString() }
                        });
                    }

                    if (!modifs.Any(x => x.Usina == exph.Cod && x.Chave == "NUMCNJ")) {
                        var x = modifs.First(y => y.Usina == exph.Cod);
                        modifs.Insert(modifs.IndexOf(x) + 1,
                            new Compass.CommomLibrary.ModifDatNW.ModifLine() {
                                Usina = exph.Cod,
                                Chave = "NUMCNJ",
                                NovosValores = new string[] { "0" }
                            });

                    } else {
                        modifs.First(x => x.Usina == exph.Cod && x.Chave == "NUMCNJ").SetValores(0);
                    }

                    exphs.Remove(exph);
                }
            }

            foreach (var modif in modifs.Where(x => x.DataModif != DateTime.MinValue && x.DataModif < deck.Dger.DataEstudo).ToList()) {
                if (modifs.Any(x => x.Usina == modif.Usina && x.Chave == modif.Chave && x.DataModif == deck.Dger.DataEstudo)) {
                    modifs.Remove(modif);
                } else {
                    modif.DataModif = deck.Dger.DataEstudo;
                }
            }

            foreach (var expG in exphs.Where(x => x.DataEntrada.HasValue && x.DataEntrada.Value < deck.Dger.DataEstudo).GroupBy(x => x.Cod)) {

                var modifConj = modifs.Where(x => x.Chave == "NUMCNJ" && x.Usina == expG.Key).First();
                var modifMaq = modifs.Where(x => x.Chave == "NUMMAQ" && x.Usina == expG.Key).ToDictionary(x => int.Parse(x.NovosValores[1]));

                if (int.Parse(modifConj.NovosValores[0]) < expG.Max(x => x.NumConj)) modifConj.SetValores(expG.Max(x => x.NumConj));

                expG.GroupBy(x => x.NumConj).ToList().ForEach(x => {

                    if (modifMaq.ContainsKey(x.Key)) {
                        modifMaq[x.Key].SetValores(int.Parse(modifMaq[x.Key].NovosValores[0]) + x.Count(), x.Key);
                    } else {
                        modifs.Insert(modifs.IndexOf(modifConj) + 1,
                            new Compass.CommomLibrary.ModifDatNW.ModifLine() {
                                Usina = expG.Key,
                                Chave = "NUMMAQ",
                                NovosValores = new string[] { x.Count().ToString(), x.Key.ToString() }
                            });
                    }
                }
                );

                expG.ToList().ForEach(x => exphs.Remove(x));
            }

            foreach (var modif in modifs.GroupBy(x => x.Usina).ToList()) {
                if (!modif.Any(x => x.Chave != "USINA")) modifs.Remove(modif.First());
                else if (!modif.Any(x => x.Chave == "USINA")) modifs.Insert(modifs.IndexOf(modif.First()) - 1,
                      new Compass.CommomLibrary.ModifDatNW.ModifLine() {
                          Usina = modif.Key,
                          Chave = "USINA",
                          NovosValores = new string[] { modif.Key.ToString() }
                      });
            }

            foreach (var u in confhds) {
                if (!exphs.Any(x => x.Cod == u.Cod) && u.Situacao != "NC") u.Situacao = "EX";
                u.Modif = modifs.Any(x => x.Usina == u.Cod);
            }
        }

        private void IncrementarSistema(DeckNewave deck) {

            var sistema = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as Compass.CommomLibrary.SistemaDat.SistemaDat;

            foreach (var item in sistema.Intercambio.Where(x => x.Ano.HasValue).ToList()) {

                if (item.Ano < deck.Dger.AnoEstudo) {
                    sistema.Intercambio.Remove(item);
                } else if (item.Ano == deck.Dger.AnoEstudo) {
                    for (int i = 1; i < deck.Dger.MesEstudo; i++) {
                        item[i] = 0;
                    }
                }
                if (deck.Dger.MesEstudo == 1 && item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2) {
                    var novoAno = item.Clone() as Compass.CommomLibrary.SistemaDat.IntLine;
                    novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;
                    sistema.Intercambio.Insert(
                        sistema.Intercambio.IndexOf(item) + 1,
                        novoAno);
                }
            }

            foreach (var item in sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList()) {

                if (item.Ano < deck.Dger.AnoEstudo) {
                    sistema.Mercado.Remove(item);
                } else if (item.Ano == deck.Dger.AnoEstudo) {
                    for (int i = 1; i < deck.Dger.MesEstudo; i++) {
                        item[i] = 0;
                    }
                }
                if (deck.Dger.MesEstudo == 1 && item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2) {
                    var novoAno = item.Clone() as Compass.CommomLibrary.SistemaDat.MerLine;
                    novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                    for (int m = 1; m <= 12; m++) {
                        double crescimento = 0;
                        //            2019                         2019+5-2=2022
                        for (int z = deck.Dger.AnoEstudo + 1; z < deck.Dger.AnoEstudo + deck.Dger.NumeroAnosEstudo - 1; z++) {
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
                    if (pos != null) {
                        for (int m = 1; m < 12; m++) {
                            pos[m] = novoAno[m];
                        }
                    }
                }
            }

            foreach (var item in sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList()) {

                if (item.Ano < deck.Dger.AnoEstudo) {
                    sistema.Pequenas.Remove(item);
                } else if (item.Ano == deck.Dger.AnoEstudo) {
                    for (int i = 1; i < deck.Dger.MesEstudo; i++) {
                        item[i] = 0;
                    }
                }
                if (deck.Dger.MesEstudo == 1 && item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2) {
                    var novoAno = item.Clone() as Compass.CommomLibrary.SistemaDat.MerLine;
                    novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                    for (int m = 1; m <= 12; m++) {
                        double crescimento = 0;
                        //            2019                         2019+5-2=2022

                        if (item.Mercado == 1) {
                            for (int z = deck.Dger.AnoEstudo + 1; z < deck.Dger.AnoEstudo + deck.Dger.NumeroAnosEstudo - 1; z++) {
                                crescimento = crescimento +
                                    sistema.Pequenas.Where(x => x.Ano == z && x.Mercado == item.Mercado).First()[m]
                                    /
                                    sistema.Pequenas.Where(x => x.Ano == z - 1 && x.Mercado == item.Mercado).First()[m];
                            }

                            crescimento = crescimento / (deck.Dger.NumeroAnosEstudo - 2);
                        } else crescimento = 1;

                        novoAno[m] = Math.Round(novoAno[m] * crescimento, 0);
                    }

                    sistema.Pequenas.Insert(
                        sistema.Pequenas.IndexOf(item) + 1,
                        novoAno);

                    var pos = sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEnePosLine && x.Mercado == item.Mercado).FirstOrDefault();
                    if (pos != null) {
                        for (int m = 1; m < 12; m++) {
                            pos[m] = novoAno[m];
                        }
                    }
                }
            }
            if (deck.Dger.MesEstudo == 1) {
                var patamares = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
                var nPat = patamares.NumeroPatamares;

                foreach (var item in patamares.Duracao.ToList()) {
                    if (item.Ano < deck.Dger.AnoEstudo) {
                        patamares.Duracao.Remove(item);
                    } else if (item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2) {
                        var novoAno = item.Clone() as Compass.CommomLibrary.PatamarDat.DuracaoLine;
                        novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                        patamares.Duracao.Insert(
                        patamares.Duracao.IndexOf(item) + nPat,
                        novoAno);
                    }
                }

                foreach (var item in patamares.Carga.Where(x => x is Compass.CommomLibrary.PatamarDat.CargaEneLine).ToList()) {
                    if (item.Ano < deck.Dger.AnoEstudo) {
                        patamares.Carga.Remove(item);
                    } else if (item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2) {
                        var novoAno = item.Clone() as Compass.CommomLibrary.PatamarDat.CargaEneLine;
                        novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                        patamares.Carga.Insert(
                        patamares.Carga.IndexOf(item) + nPat,
                        novoAno);
                    }
                }

                foreach (var item in patamares.Intercambio.Where(x => x is Compass.CommomLibrary.PatamarDat.IntABLine).ToList()) {
                    if (item.Ano < deck.Dger.AnoEstudo) {
                        patamares.Intercambio.Remove(item);
                    } else if (item.Ano == deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 2) {
                        var novoAno = item.Clone() as Compass.CommomLibrary.PatamarDat.IntABLine;
                        novoAno.Ano = deck.Dger.NumeroAnosEstudo + deck.Dger.AnoEstudo - 1;

                        patamares.Intercambio.Insert(
                        patamares.Intercambio.IndexOf(item) + nPat,
                        novoAno);
                    }
                }
            }
        }

        private void IncrementarEarm(DeckNewave deck) {

            if (ConfighBase != null) {

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
            } else {

                deck.Dger.CalculaEarmInicial = false;
                int i = 0;
                var earms = new double[deck.Ree.Count];

                foreach (var ree in deck.Ree) {
                    earms[i] = this.VolumesPO[ree.Submercado][deck.Dger.DataEstudo.Month - 1] * 100;
                    i++;
                }

                deck.Dger.Earms = earms;
            }
        }

        private void IncrementarVAZAO(DeckNewave deck) {

            var data = deck.Dger.DataEstudo;

            var vaspast = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.vazpast].Document as Compass.CommomLibrary.Vazpast.Vazpast;

            foreach (var vp in vaspast.Conteudo) {
                vp[data] = this.PrevisaoVazao[vp.Posto][data.Month - 1];
            }
        }

        private void IncrementarRE(DeckNewave deck) {

            var reDat = deck[Compass.CommomLibrary.Newave.Deck.DeckDocument.re].Document as Compass.CommomLibrary.ReDat.ReDat;

            foreach (var re in reDat.Restricoes.ToList()) {
                foreach (var reDet in reDat.Detalhes.Where(x => x.Numero == re.Numero).ToList()) {

                    if (reDet.Inicio < deck.Dger.DataEstudo && reDet.Fim >= deck.Dger.DataEstudo) {
                        reDet.Inicio = deck.Dger.DataEstudo;
                    } else if (reDet.Fim < deck.Dger.DataEstudo) {
                        reDat.Detalhes.Remove(reDet);
                    }
                }

                if (reDat.Detalhes.Where(x => x.Numero == re.Numero).Count() == 0) reDat.Restricoes.Remove(re);
            }


            foreach (var rest in this.Restricoes.Where(x => x.MesEstudo == deck.Dger.MesEstudo)) {

                //procura restricao
                var re = reDat.Restricoes.Where(
                    x => String.Join("", x.Valores.Skip(1).Where(y => y != null).OrderBy(y => y).Select(y => y.ToString().Trim()))
                        == String.Join("", rest.Usinas.OrderBy(y => y).Select(y => y.ToString()))
                    ).FirstOrDefault();

                //se nao exite insere
                if (re == null) {
                    re = new Compass.CommomLibrary.ReDat.ReLine() {
                        Numero = reDat.Restricoes.Max(x => x.Numero) + 1
                    };

                    for (int i = 0; i < rest.Usinas.Count; i++) {
                        re[i + 1] = rest.Usinas[i];
                    }

                    reDat.Restricoes.Add(re);


                    var val = new Compass.CommomLibrary.ReDat.ReValLine() {
                        Numero = re.Numero,
                        Patamar = rest.Patamar,
                        ValorRestricao = rest.Restricao,
                        Inicio = new DateTime(rest.AnoIni, rest.MesIni, 1),
                        Fim = new DateTime(rest.AnoFim, rest.MesFim, 1),
                    };

                    reDat.Detalhes.Add(val);
                }
                    //altera ou insere novo valor
                else {

                    var val = new Compass.CommomLibrary.ReDat.ReValLine() {
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

                    if (anterior != null) {
                        var anteriorSplit = anterior.Clone() as Compass.CommomLibrary.ReDat.ReValLine;
                        anterior.Inicio = val.Inicio;
                        anteriorSplit.Fim = val.Inicio.AddMonths(-1);

                        reDat.Detalhes.Add(anteriorSplit);
                    }

                    if (posterior != null) {
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

        private Task SetCasoInicial() {

            DeckMedia = new DeckNewave();
            DeckMedia.EstudoPai = this;

            DeckMedia.GetFiles(Origem);

            SetNomeDeck(DeckMedia);

            DeckMedia.BaseFolder = System.IO.Path.Combine(Destino, DeckMedia.Dger.AnoEstudo.ToString("0000") + DeckMedia.Dger.MesEstudo.ToString("00"));

            DeckMedia.Dger.Flags = new int[] { 1, 1, 1, 0, 0 };


            if (DeckMedia.Dger.TipoTendenciaHidrologia == 2) {//atualizar mês atual com vazao prevista para calcular ENA e usá-la no newdesp

                IncrementarVAZAO(DeckMedia);
            }

            IncrementarRE(DeckMedia);
            IncrementarEarm(DeckMedia);


            DeckMedia.SaveFilesToFolder(DeckMedia.BaseFolder);
            DeckMedia.EscreverListagemNwlistop();


            var path = DeckMedia.BaseFolder;
            //TODO: executar consistencia
            return Task.Factory.StartNew(() => {
                ExecutarConsistencia(path);
                Compass.Services.Deck.CreateDgerNewdesp(path);
            });


        }



        private void CriarDiretorio(string Destino) {
            throw new NotImplementedException();
        }

        private void RemoverDiretorio(string Destino) {
            throw new NotImplementedException();
        }

        private void ExecutarConsistencia(string destino) {

            var ret = Compass.Services.Linux.Run(destino, this.ExecutavelNewave + " 3", "NewaveConsist", true, true, "hide");

            if (!ret) {
                throw new Exception("Ocorreu erro na criação e consistência dos decks newaves. Verifique.");
            }

        }


    }
}

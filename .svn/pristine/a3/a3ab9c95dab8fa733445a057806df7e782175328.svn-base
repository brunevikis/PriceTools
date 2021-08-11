using ConsoleApp1.Dadger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.Decomp
{
    public class ConfigH
    {
        // SegundosPorHora / 10^6 => MWh
        const double FATORh = 0.0036;

        // DiasNoMes * SegundosPorDia / 10^6 => MWmes
        const double FATOR = FATORh * 730.5;

        public UHE[] usinas;
        public IEnumerable<UHE> Usinas { get { return usinas.Where(u => u != null); } }

        public List<Tuple<int, int>> index_sistemas;
        private Dadger.Dadger baseDadger;
        private ConfhdDat.ConfhdDat confhdDat;

        public BaseDocument baseDoc;

        private HidrDat.HidrDat hidr;

        private ConfigH() { }

        /*
        public ConfigH(Dadger.Dadger baseDadger, HidrDat.HidrDat baseHidr)
        {
            this.baseDadger = (Dadger.Dadger)baseDadger.Clone();
            this.baseDoc = this.baseDadger;

            var sis = new List<Tuple<int, int>>();
            int id = 0;
            foreach (var sb in baseDadger.BlocoSb.Where(x => x[2] != "FC"))
            {
                sis.Add(new Tuple<int, int>(id, sb[1]));
                id++;
            }
            this.index_sistemas = sis;


            var usinas = new UHE[baseHidr.Data.Count + 1];
            foreach (var item in baseHidr.Data.Where(h => !String.IsNullOrWhiteSpace(h.Usina)))
            {
                usinas[item.Cod] = new UHE(item, this);
            }

            //configurar ficticias
            var ficts = new List<Tuple<int, int>>();
            foreach (var fict in usinas.Where(u => u != null && u.Jusante.Value != 0)
                .Where(u => u.Usina.StartsWith("FICT.", StringComparison.OrdinalIgnoreCase))
                )
            {

                var usinaReal = usinas.Where(u => u != null).Where(u =>
                    u.Cod != fict.Cod &&
                    u.Posto == fict.Posto &&
                    (u.Jusante == fict.Jusante ||
                         usinas[fict.Jusante.Value].Usina.StartsWith("FICT.", StringComparison.OrdinalIgnoreCase)
                    )
                    ).FirstOrDefault();

                if (usinaReal != null)
                {

                    ficts.Add(new Tuple<int, int>(usinaReal.Cod, fict.Cod));
                    fict.IsFict = true;
                    fict.CodReal = usinaReal.Cod;
                    usinaReal.CodFicticia = fict.Cod;
                }
            }

            this.usinas = usinas;

            loadDadgerInfo();
            updateProdTotal65VolUtil();
        }
        */
        /*
        public ConfigH(ConfhdDat.ConfhdDat confhdDat, HidrDat.HidrDat baseHidr, ModifDatNW.ModifDatNw modif)
        {
            // TODO: Complete member initialization
            this.confhdDat = (ConfhdDat.ConfhdDat)confhdDat.Clone();
            this.baseDoc = this.confhdDat;
            this.hidr = baseHidr;

            var sis = new List<Tuple<int, int>>();
            int id = 0;
            foreach (var ree in new int[] { 1, 2, 3, 4 })
            {
                sis.Add(new Tuple<int, int>(id, ree));
                id++;
            }
            this.index_sistemas = sis;


            var usinas = new UHE[baseHidr.Data.Count + 1];
            foreach (var item in baseHidr.Data.Where(h => !String.IsNullOrWhiteSpace(h.Usina)))
            {
                usinas[item.Cod] = new UHE(item, this);

            }

            //configurar ficticias
            var ficts = new List<Tuple<int, int>>();
            foreach (var fict in usinas.Where(u => u != null && u.Jusante.Value != 0)
                .Where(u => u.Usina.StartsWith("FICT.", StringComparison.OrdinalIgnoreCase))
                )
            {

                var usinaReal = usinas.Where(u => u != null).Where(u =>
                    u.Cod != fict.Cod &&
                    u.Posto == fict.Posto &&
                    (u.Jusante == fict.Jusante ||
                         usinas[fict.Jusante.Value].Usina.StartsWith("FICT.", StringComparison.OrdinalIgnoreCase)
                    )
                    ).FirstOrDefault();

                if (usinaReal != null)
                {

                    ficts.Add(new Tuple<int, int>(usinaReal.Cod, fict.Cod));
                    fict.IsFict = true;
                    fict.CodReal = usinaReal.Cod;
                    usinaReal.CodFicticia = fict.Cod;
                }
            }

            this.usinas = usinas;

            //loadDadgerInfo();
            //updateProdTotal65VolUtil();

            //Ler o UH - Volume
            foreach (var uh in confhdDat.Where(x => x.Situacao == "EX" || x.Situacao == "EE"))
            {
                usinas[uh.Cod].VolIni = uh.VolUtil * usinas[uh.Cod].VolUtil / 100f;
                usinas[uh.Cod].InDadger = true;
                usinas[uh.Cod].Ree = uh.REE;


                if (uh.CodJusante > 0) usinas[uh.Cod].Jusante = uh.CodJusante;
            }

            loadModifInfo(modif);
        }
        */
        /*
        void loadDadgerInfo()
        {
            //Ler informações do dadger e sobrepor as do CadUSH
            foreach (var ac in baseDadger.BlocoAc)
            {
                if (ac.Mnemonico == "VOLMAX")
                {
                    usinas[ac.Usina].VolMax = (float)ac.P1;
                }
                else if (ac.Mnemonico == "VOLMIN")
                {
                    usinas[ac.Usina].VolMin = (float)ac.P1;   //Veri                    
                }
                else if (ac.Mnemonico == "JUSENA")
                {
                    //usinas[ac.Usina].Jusante = ac.P1;  //Veri
                    usinas[ac.P1].InJusEna = true;
                    usinas[ac.Usina].JusEna = ac.P1;  //Veri
                    if (usinas[ac.Usina].CodFicticia.HasValue) usinas[usinas[ac.Usina].CodFicticia.Value].JusEna = ac.P1;
                }
                else if (ac.Mnemonico == "JUSMED")
                {
                    if ((ac.Semana.HasValue && ac.Semana.Value == 1) || ac.Mes.Trim() == "")
                        usinas[ac.Usina].CanalFugaMed = (float)ac.P1;  //veri
                }
                else if (ac.Mnemonico == "NUMCON")
                { //usinas não instaladas
                    if (((ac.Semana.HasValue && ac.Semana.Value == 1) || ac.Mes.Trim() == "") && ac.P1 == 0)
                        usinas[ac.Usina].ProdEsp = 0;  //veri                     
                }
                else if (ac.Mnemonico == "TIPUSI")
                {
                    usinas[ac.Usina].Reg = ac.P1;
                }
                else if (ac.Mnemonico == "PROESP")
                {
                    usinas[ac.Usina].ProdEsp = (float)ac.P1;
                }
                else if (ac.Mnemonico == "TIPERH")
                {
                    usinas[ac.Usina].PerdaTipo = ac.P1;
                }
                else if (ac.Mnemonico == "PERHID")
                {
                    usinas[ac.Usina].PerdaVal = (float)ac.P1;
                } //else if (ac.Mnemonico == "NUMPOS") {
                //  usinas[ac.Usina].Posto = (int)ac.P1;
                //}
            }

            //checar submotorizacao
            foreach (var altNumMaq in baseDadger.BlocoAc
                .Where(ac => ac.Semana.HasValue && ac.Semana.Value == 1 && ac.Mnemonico == "NUMMAQ")
                .GroupBy(ac => ac.Usina))
            {

                if (altNumMaq.Sum(x => x.P2) < usinas[altNumMaq.Key].NumUnidadeBase)
                {
                    usinas[altNumMaq.Key].ProdEsp = 0;  //veri                     
                }
            }



            //Para as postos em dois submercados, ler o percentual de limitação do volume
            foreach (var ez in baseDadger.BlocoEz)
            {
                var usina = usinas[ez[1]];
                if (usina.CodFicticia.HasValue)
                {

                    var fic = usinas[usina.CodFicticia.Value];
                    usina.Ez = fic.Ez = (float)ez[2] / 100f;

                }
            }

            //tempo de viagem
            try
            {
                foreach (var vi in baseDadger.BlocoVi)
                {
                    usinas[vi.Usina].TempoViagem = vi.TempoViagem;
                    var qi = new float?[5];
                    var qd = new float?[5];

                    qd[0] = (float?)vi.Valores[3];

                    if (vi.Valores[4] != null)
                    {
                        qd[1] = (float?)vi.Valores[4];
                        qd[2] = (float?)vi.Valores[5];
                        qd[3] = (float?)vi.Valores[6];
                        qd[4] = (float?)vi.Valores[7];
                    }
                    else
                    {
                        qd[1] =
                        qd[2] =
                        qd[3] =
                        qd[4] = qd[0];
                    }

                    usinas[vi.Usina].VazaoDefluentePassada = qd;

                    var qiL = baseDadger.BlocoQi.Where(q => q.Usina == vi.Usina).FirstOrDefault();

                    if (qiL != null)
                    {
                        qi[0] = (float)qiL.Valores[2];


                        if (qiL.Valores[3] != null)
                        {
                            qi[1] = (float)qiL.Valores[3];
                            qi[2] = (float)qiL.Valores[4];
                            qi[3] = (float)qiL.Valores[5];
                            qi[4] = (float)qiL.Valores[6];
                        }
                        else
                        {
                            qi[1] =
                            qi[2] =
                            qi[3] =
                            qi[4] = qi[0];
                        }



                        usinas[vi.Usina].VazaoIncrementalPassada = qi;

                    }
                    else
                        usinas[vi.Usina].VazaoIncrementalPassada = new float?[7] { 0, 0, 0, 0, 0, 0, 0 };

                    usinas[vi.Usina].VazaoIncremental = new int?[7];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar os blocos VI e/ou QI", ex);
            }

            //Ler o UH - Volume
            foreach (var uh in baseDadger.BlocoUh)
            {
                usinas[uh.Usina].VolIni = uh.VolIniPerc * usinas[uh.Usina].VolUtil / 100f;
                usinas[uh.Usina].InDadger = true;
                usinas[uh.Usina].Ree = uh.Sistema;

            }
        }
        
        void loadModifInfo(ModifDatNW.ModifDatNw modif)
        {

            var inInfo = System.Globalization.NumberFormatInfo.InvariantInfo;

            //Ler informações do dadger e sobrepor as do CadUSH
            foreach (var ac in modif)
            {
                if (ac.Chave == "VOLMAX")
                {


                    if (!usinas[ac.Usina].IsFict)
                    {
                        if (ac.NovosValores[1].Contains("%"))
                        {
                            var p = float.Parse(ac.NovosValores[0], inInfo);
                            var vu = usinas[ac.Usina].VolUtil;

                            usinas[ac.Usina].VolMax = (float)vu * p / 100;

                        }
                        else
                        {
                            usinas[ac.Usina].VolMax = float.Parse(ac.NovosValores[0], inInfo);
                        }
                    }
                    else
                    {
                        var real = usinas[ac.Usina].CodReal.Value;

                        if (ac.NovosValores[1].Contains("%"))
                        {
                            var p = float.Parse(ac.NovosValores[0], inInfo);

                            usinas[ac.Usina].Ez = usinas[real].Ez = p / 100;

                        }
                        else
                        {
                            var vu = usinas[real].VolUtil;
                            var vm = float.Parse(ac.NovosValores[0], inInfo);

                            usinas[ac.Usina].Ez = usinas[real].Ez = (float)(vm / vu);
                        }

                    }


                }
                else if (ac.Chave == "VOLMIN")
                {
                    if (ac.NovosValores[1].Contains("%"))
                    {
                        var p = float.Parse(ac.NovosValores[0], inInfo);
                        var vu = usinas[ac.Usina].VolUtil;

                        usinas[ac.Usina].VolMin = (float)vu * p / 100;

                    }
                    else
                    {
                        usinas[ac.Usina].VolMin = float.Parse(ac.NovosValores[0], inInfo);
                    }
                    //} else if (ac.Chave == "CFUGA") {
                    //    if ((ac.Semana.HasValue && ac.Semana.Value == 1) || ac.Mes.Trim() == "")
                    //        usinas[ac.Usina].CanalFugaMed = (float)ac.P1;  //veri
                }
                else if (ac.Chave == "NUMCNJ")
                { //usinas não instaladas
                    if (ac.NovosValores[0] == "0")
                        usinas[ac.Usina].ProdEsp = 0;  //veri                     
                    //} else if (ac.Mnemonico == "TIPUSI") {
                    //    usinas[ac.Usina].Reg = ac.P1;
                }
                else if (ac.Chave == "PRODESP")
                {
                    usinas[ac.Usina].ProdEsp = float.Parse(ac.NovosValores[0], inInfo);
                    //} else if (ac.Mnemonico == "TIPERH") {
                    //    usinas[ac.Usina].PerdaTipo = ac.P1;
                }
                else if (ac.Chave == "PERDHIDR")
                {
                    usinas[ac.Usina].PerdaVal = float.Parse(ac.NovosValores[0], inInfo);
                } //else if (ac.Mnemonico == "NUMPOS") {
                //  usinas[ac.Usina].Posto = (int)ac.P1;
                //}
            }

            //canaldefuga
            foreach (var altCfuga in modif
                .Where(ac => ac.Chave == "CFUGA")
                .GroupBy(ac => ac.Usina))
            {
                usinas[altCfuga.Key].CanalFugaMed = float.Parse(altCfuga.First().NovosValores[2], inInfo);

            }

            //checar submotorizacao
            foreach (var altNumMaq in modif
                .Where(ac => ac.Chave == "NUMMAQ")
                .GroupBy(ac => ac.Usina))
            {

                if (altNumMaq.Sum(x => int.Parse(x.NovosValores[0])) < usinas[altNumMaq.Key].NumUnidadeBase)
                {
                    usinas[altNumMaq.Key].ProdEsp = 0;  //veri                     
                }
            }

        }
        */
        public void ReloadUH()
        {
            //Ler o UH - Volume

            if (baseDadger != null)
            {
                foreach (var uh in baseDadger.BlocoUh)
                {
                    usinas[uh.Usina].VolIni = uh.VolIniPerc * usinas[uh.Usina].VolUtil / 100f;
                    usinas[uh.Usina].InDadger = true;
                }

            }
            else if (confhdDat != null)
            {
                foreach (var uh in confhdDat.Where(x => x.Situacao == "EX" || x.Situacao == "EE"))
                {
                    usinas[uh.Cod].VolIni = uh.VolUtil * usinas[uh.Cod].VolUtil / 100f;
                    usinas[uh.Cod].InDadger = true;
                }
            }

        }

        void updateQuedas()
        {
            foreach (var uhe in Usinas)
            {
                uhe.atualizaQueda();
            }
        }

        void updateProdTotal(double prodSom = 0, UHE jusante = null)
        {

            var usinasTemp = Usinas.Select(c =>
                //new { usina = c, jusante = Usinas.FirstOrDefault(j => j.Cod == c.Jusante) });
                //new { usina = c, jusante = c.Jusante.HasValue ? usinas[c.Jusante.Value] : null });
                new { usina = c, jusante = c.JusEna.HasValue ? usinas[c.JusEna.Value] : null });

            usinasTemp = usinasTemp.Where(c =>
            {
                if (jusante == null)
                {
                    return c.jusante == null || (c.jusante.Mercado != c.usina.Mercado);
                }
                else
                {
                    return c.jusante != null && c.jusante.Cod == jusante.Cod && c.usina.Mercado == jusante.Mercado;
                }
            }).ToArray();

            foreach (UHE usina in usinasTemp.Select(c => c.usina))
            { //  cadUsinas.Where(p => p.jusante == codUsina)) {
                usina.ProdTotal = usina.ProdCalc + prodSom;
                updateProdTotal(usina.ProdTotal, usina);
                //System.Diagnostics.Debug.WriteLine(usina.Usina + "\t" + usina.Cod.ToString() + "\t" + usina.ProdTotal + "\t" + usina.EnergiaArmazenada);
            }
        }

        void updateProdTotal65VolUtil(double prodSom = 0, UHE jusante = null)
        {

            var usinasTemp = Usinas.Select(c =>
                //new { usina = c, jusante = Usinas.FirstOrDefault(j => j.Cod == c.Jusante) });
                new { usina = c, jusante = c.JusEna.HasValue ? usinas[c.JusEna.Value] : null });

            usinasTemp = usinasTemp.Where(c =>
            {
                if (jusante == null)
                {
                    return c.jusante == null || (c.jusante.Mercado != c.usina.Mercado);
                    //return c.jusante == null;
                }
                else
                {
                    return c.jusante != null && c.jusante.Cod == jusante.Cod && c.usina.Mercado == jusante.Mercado;
                    //return c.jusante != null && c.jusante.Cod == jusante.Cod;
                }
            }).ToArray();

            foreach (UHE usina in usinasTemp.Select(c => c.usina))
            { //  cadUsinas.Where(p => p.jusante == codUsina)) {
                usina.ProdTotal65VolUtil = usina.Prod65VolUtil + prodSom;
                updateProdTotal65VolUtil(usina.ProdTotal65VolUtil, usina);
                //System.Diagnostics.Debug.WriteLine(usina.Usina + "\t" + usina.Cod.ToString() + "\t" + usina.ProdTotal + "\t" + usina.EnergiaArmazenada);
            }
        }


        /// <summary>
        /// Sets UH to max and calc EARM.
        /// To bring back original values of UH use ReloadUH()
        /// </summary>
        /// <returns></returns>
        public double[] GetEarmsMax()
        {


            //usinas com reservatorio ou com restricao de operação a fio d'agua para 100%
            //var temp = Usinas.Where(u => !u.IsFict && (u.VolIni > 0 || u.RestricaoVolMax == 0));
            var temp = Usinas.Where(u => !u.IsFict && u.VolUtil > 0);

            //ComRestricoesDeVolume = false;
            var currentVols = temp.Select(u => new { u.Cod, u.VolIni }).ToArray();
            foreach (var uhe in temp)
            {
                uhe.VolIni = uhe.VolUtil;
            }

            var earmMax = GetEarms();

            //if (!keepVolMax) {
            //    foreach (var uhe in currentVols) {
            //        usinas[uhe.Cod].VolIni = uhe.VolIni;
            //    }
            //}

            //ComRestricoesDeVolume = true;

            return earmMax;
        }

        public double[] GetEarms()
        {

            updateQuedas();

            updateProdTotal();

            var earms = new double[index_sistemas.Count];

            for (int x = 0; x < index_sistemas.Count; x++)
            {
                int sistema = index_sistemas.Where(i => i.Item1 == x).First().Item2;
                earms[x] = getEarm(sistema); ;
            }

            return earms;
        }

        double getEarm(int sistema)
        {
            double total = 0;

            foreach (var usina in Usinas.Where(p => p.Mercado == sistema)
                //.Where(u => u.InDadger || (u.IsFict && usinas[u.CodReal.Value].InDadger))
                )
                total = total + usina.EnergiaArmazenada;

            return total;
        }
        /*
        private void setEna(Prevs.Prevs prevs, int s, ref Dictionary<UHE, float[]> result)
        {
            var vazaoNatural = prevs;


            foreach (var usinaFio in Usinas.Where(u => u.VolUtil == 0)
                .Where(u => u.InDadger && !u.IsFict)
                )
            {

                int vazaoNaUsina = vazaoNatural[usinaFio.Posto, s];
                int vazaoMontante = 0;

                vazaoMontante = SomarVazoesMontantesReserv(usinaFio, vazaoNatural, s);

                int qInc = vazaoNaUsina - vazaoMontante;

                if (qInc < 0) qInc = 0;

                var enaf = (float)(qInc * usinaFio.Prod65VolUtil);

                if (!result.ContainsKey(usinaFio))
                {
                    result.Add(usinaFio, new float[6]);
                }
                result[usinaFio][s - 1] = enaf;

                //eFio[usinaFio.Cod, s] = enaf;

                System.Diagnostics.Debug.WriteLine("Fio\t" + usinaFio.Usina + "\t" + usinaFio.Prod65VolUtil.ToString() + "\t" + usinaFio.ProdTotal65VolUtil.ToString() + "\t" + vazaoNaUsina.ToString() + "\t" + qInc + "\t" + enaf.ToString() + "\t" + usinaFio.Mercado.ToString());

            }

            foreach (var usinaReserv in Usinas.Where(u => u.VolUtil != 0
                && (u.InDadger || (u.IsFict && usinas[u.CodReal.Value].InDadger))))
            {

                int vazaoNaUsina = vazaoNatural[usinaReserv.Posto, s];


                int vazaoMontante = 0;
                if (usinaReserv.IsFict)
                    vazaoMontante = SomarVazoesMontantesReserv(usinas[usinaReserv.CodReal.Value], vazaoNatural, s);
                else
                    vazaoMontante = SomarVazoesMontantesReserv(usinaReserv, vazaoNatural, s);

                int qInc = vazaoNaUsina - vazaoMontante;

                var enaC = (float)(qInc * usinaReserv.ProdTotal65VolUtil);

                if (!result.ContainsKey(usinaReserv))
                {
                    result.Add(usinaReserv, new float[6]);
                }
                result[usinaReserv][s - 1] = enaC;


                System.Diagnostics.Debug.WriteLine("Reserv\t" + usinaReserv.Usina + "\t" + usinaReserv.Prod65VolUtil.ToString() + "\t" + usinaReserv.ProdTotal65VolUtil.ToString() + "\t" + vazaoNaUsina.ToString() + "\t" + qInc + "\t" + enaC.ToString() + "\t" + usinaReserv.Mercado.ToString());
            }

        }

        private void setEnaAcopl(Prevs.Prevs prevs, int s, int es, ref Dictionary<UHE, float[]> result)
        {
            var vazaoNatural = prevs;


            foreach (var usina in Usinas.Where(u => u.InDadger || (u.IsFict && usinas[u.CodReal.Value].InDadger)))
            {

                var uheReal = usina.IsFict ? usinas[usina.CodReal.Value] : usina;
                int qInc = 0;
                float enaC = 0;
                if (uheReal.Posto == 300 || uheReal.PARTIF)
                {
                    qInc = 0;
                }
                else if (uheReal.VINCR.HasValue)
                {
                    qInc = vazaoNatural[uheReal.VINCR.Value, s];
                    enaC = (float)(qInc * usina.ProdTotal65VolUtil);
                }
                else
                {


                    int vazaoNaUsina = vazaoNatural[uheReal.Posto, s];

                    int vazaoMontante = SomarVazoesMontantes(uheReal, vazaoNatural, s, es);

                    qInc = vazaoNaUsina - vazaoMontante;

                    qInc = qInc < 0 ? 0 : qInc;

                    if (uheReal.TempoViagem.HasValue)
                    {
                        uheReal.VazaoIncremental[s] = qInc;
                        enaC = (float)(qInc * usina.Prod65VolUtil);

                        var tvi = uheReal.TempoViagem.Value / 168f;

                        var desl = (s - es) - tvi;
                        var x1 = (int)Math.Floor(desl);
                        var x2 = (int)Math.Ceiling(desl);

                        var qi1 = x1 < 1 ? uheReal.VazaoIncrementalPassada[x1 * (-1)] : uheReal.VazaoIncremental[x1 + es];
                        var qi2 = x2 < 1 ? uheReal.VazaoIncrementalPassada[x2 * (-1)] : uheReal.VazaoIncremental[x2 + es];

                        var qi = (qi2 - qi1) * (desl - x1) + qi1;

                        enaC += (float)(qi * (usina.ProdTotal65VolUtil - usina.Prod65VolUtil));
                    }
                    else
                    {
                        enaC = (float)(qInc * usina.ProdTotal65VolUtil);
                    }
                }


                if (!result.ContainsKey(usina))
                {
                    result.Add(usina, new float[6]);
                }
                result[usina][s - 1] = enaC;

            }

        }

        public Dictionary<UHE, float[]> GetEna(Prevs.Prevs prevs)
        {

            updateProdTotal65VolUtil();

            Dictionary<UHE, float[]> dicResult = new Dictionary<UHE, float[]>();

            for (int s = 1; s <= 6; s++)
            {
                setEna(prevs, s, ref dicResult);
            }
            //var vazaoNatural = prevs.GetIndexedPrevs();
            return dicResult;
        }

        public Dictionary<UHE, float[]> GetEnaAcopl(Prevs.Prevs prevs, ModifDat.ModifDat modif = null)
        {

            updateProdTotal65VolUtil();
            var es = baseDadger.BlocoEs.NumSemanasPassadas;

            if (modif != null)
            {

                var blk = modif.Blocos.First().Value;

                foreach (var mod in blk)
                {

                    if (mod.Valores[0] == "PARTIF")
                    {
                        // usinas[(int)mod.Valores[1]].PARTIF = true;

                    }
                    else if (mod.Valores[0] == "VINCR ")
                    {
                        usinas[(int)mod.Valores[1]].VINCR = mod.Valores[2];
                    }
                }


            }


            Dictionary<UHE, float[]> dicResult = new Dictionary<UHE, float[]>();

            for (int s = 1 + es; s <= 6; s++)
            {
                setEnaAcopl(prevs, s, es, ref dicResult);
            }
            //var vazaoNatural = prevs.GetIndexedPrevs();
            return dicResult;
        }

        int SomarVazoesMontantesReserv(UHE uhe, Prevs.Prevs Qn, int s)
        {

            int Q = 0;
            var montantes = Usinas.Where(u => !u.IsFict && u.Jusante == uhe.Cod);
            foreach (var mont in montantes)
            {
                if (mont.VolUtil != 0 && mont.InDadger)
                {
                    Q += Qn[mont.Posto, s];
                }
                else
                {
                    Q += SomarVazoesMontantesReserv(mont, Qn, s);
                }
            }

            return Q;
        }

        IEnumerable<UHE> Montantes(UHE uhe)
        {

            List<UHE> result = new List<UHE>();


            var montantes = Usinas.Where(u => !u.IsFict && u.Jusante == uhe.Cod);


            result.AddRange(montantes.Where(u => u.InDadger));

            foreach (var mon in montantes.Where(u => !u.InDadger))
            {
                result.AddRange(Montantes(mon));
            }

            return result;
        }

        int SomarVazoesMontantes(UHE uhe, Prevs.Prevs Qn, int s, int es)
        {

            float Q = 0;
            var montantes = Montantes(uhe);// Usinas.Where(u => !u.IsFict && u.Jusante == uhe.Cod);
            foreach (var mont in montantes)
            {
                if (mont.InDadger)
                {
                    if (!mont.TempoViagem.HasValue)
                        Q += Qn[mont.Posto, s];
                    else
                    {

                        var tvi = mont.TempoViagem.Value / 168f;

                        var desl = (s - es) - tvi;
                        var x1 = (int)Math.Floor(desl);
                        var x2 = (int)Math.Ceiling(desl);

                        var qn1 = x1 < 1 ? mont.VazaoDefluentePassada[x1 * (-1)] : Qn[mont.Posto, x1 + es];
                        var qn2 = x2 < 1 ? mont.VazaoDefluentePassada[x2 * (-1)] : Qn[mont.Posto, x2 + es];

                        var qn = (qn2 - qn1) * (desl - x1) + qn1;

                        Q += qn.Value;
                    }

                }
                else
                {
                    Q += SomarVazoesMontantes(mont, Qn, s, es);
                }
            }

            return (int)Q;
        }
        */
        //int SomarVazoesMontantesReserv(UHE uhe, int[,] Qn, int s) {

        //    int Q = 0;
        //    var montantes = Usinas.Where(u => !u.IsFict && u.Jusante == uhe.Cod);
        //    foreach (var mont in montantes) {
        //        if (mont.VolUtil != 0 && mont.InDadger) {
        //            Q += Qn[mont.Posto, s];
        //        } else {
        //            Q += SomarVazoesMontantesReserv(mont, Qn, s);
        //        }
        //    }

        //    return Q;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curvaEarm">uh, vol util min permitido, vol util max permitido</param>
        /// <returns></returns>
        public string ToEarmConfigFile(List<Tuple<int, double, double>> curvaEarm = null)
        {

            curvaEarm = curvaEarm ?? new List<Tuple<int, double, double>>();


            foreach (var u in Usinas)
            {
                if (curvaEarm.Any(x => x.Item1 == u.Cod && x.Item2 > 0))
                {
                    u.VolMinRestricao = curvaEarm.Where(x => x.Item1 == u.Cod && x.Item2 > 0).Max(x => x.Item2);
                }
                else u.VolMinRestricao = null;
                if (curvaEarm.Any(x => x.Item1 == u.Cod && x.Item3 > 0))
                {
                    u.VolMaxRestricao = curvaEarm.Where(x => x.Item1 == u.Cod && x.Item3 > 0).Min(x => x.Item3);
                }
                else u.VolMaxRestricao = null;
            }

            return
                    "Cod;VolMin;VolMax;CotaMin;CotaMax;Ree;Mercado;ProdEsp;PCV0;PCV1;PCV2;PCV3;PCV4;CodReal;Ez;PerdaVal;PerdaTipo;CanalFugaMed;Jusante;Reg;InDadger;RestVolMin;RestVolMax\n"
            +
            string.Join("\n",
             Usinas.Select(u =>
                 string.Join(";",
                    u.Cod,
                    u.VolMin.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.VolMax.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.CotaMin.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.CotaMax.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.Ree, u.Mercado, ((u.InDadger || u.InJusEna) ? u.ProdEsp : 0).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.PCV0.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.PCV1.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.PCV2.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.PCV3.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.PCV4.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.CodReal,
                    u.Ez.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.PerdaVal.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.PerdaTipo,
                    u.CanalFugaMed.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    u.JusEna,
                    u.Reg,
                    (u.InDadger || (u.IsFict && usinas[u.CodReal.Value].InDadger)),
                    ((u.VolMinRestricao ?? 0) + u.VolMin).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ((u.VolMaxRestricao ?? u.VolUtil) + u.VolMin).ToString(System.Globalization.CultureInfo.InvariantCulture)

                    //(curvaEarm.Any(x => x.Item1 == u.Cod && x.Item2 > 0) ?
                    //    curvaEarm.Where(x => x.Item1 == u.Cod && x.Item2 > 0).Max(x => x.Item2) + u.VolMin
                    //    : u.VolMin
                    //).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    //(curvaEarm.Any(x => x.Item1 == u.Cod && x.Item3 > 0) ?
                    //    curvaEarm.Where(x => x.Item1 == u.Cod && x.Item3 > 0).Min(x => x.Item3) + u.VolMin
                    //    : u.VolMax
                    //).ToString(System.Globalization.CultureInfo.InvariantCulture)

                    ))
                    );
        }

        public class UHE
        {

            ConfigH reserv;

            internal UHE(HidrDat.HidrLine hidCad, ConfigH reserv)
            {

                this.reserv = reserv;

                Mercado = (int)hidCad.Sistema;
                Cod = hidCad.Cod;
                Usina = hidCad.Usina;
                Jusante = hidCad.Jusante;
                PerdaVal = hidCad.PerdaVal;
                PerdaTipo = hidCad.PerdaTipo;
                Reg = hidCad.Reg;
                CanalFugaMed = hidCad.CanalFugaMed;
                VolMax = hidCad.VolMax;
                VolMin = hidCad.VolMin;
                CotaMax = hidCad.CotaMax;
                PCV0 = hidCad.PCV0;
                PCV1 = hidCad.PCV1;
                PCV2 = hidCad.PCV2;
                PCV3 = hidCad.PCV3;
                PCV4 = hidCad.PCV4;
                CotaMin = hidCad.CotaMin;
                Posto = hidCad.Posto;
                prodEsp = hidCad.ProdEsp;
                volRef = hidCad.VolRef;
                NumUnidadeBase = hidCad.NumUnidadesBase;

                Mercado = hidCad.Sistema;
            }

            double volIni;
            public double VolIni
            {
                get
                {
                    if (IsFict)
                        return Math.Min(reserv.usinas[CodReal.Value].VolIni, Ez * VolUtil);
                    //else if (Reg == "D" || Reg == "S")
                    else if (Reg == "D")
                        return 0;
                    else
                        return volIni;
                }
                set
                {
                    if (IsFict)
                    {
                        return;
                    }

                    volIni = value;

                    if (volIni > (VolMaxRestricao ?? VolUtil))
                        volIni = (VolMaxRestricao ?? VolUtil);

                    if (volIni < (VolMinRestricao ?? 0))
                        volIni = (VolMinRestricao ?? 0);

                    //if (volIni > VolUtil)
                    //    volIni = VolUtil;

                    //else if (volIni < 0)
                    //    volIni = 0;
                }
            }
            public double VolUtil { get { return VolMax - VolMin; } }

            public bool IsFict;
            public int? CodReal;
            public int? CodFicticia;

            public bool InDadger;
            public bool InJusEna;
            double prodEsp;
            public double ProdEsp
            {
                //get { return (InDadger || InJusEna) ? prodEsp : 0; }
                get { return prodEsp; }
                set { prodEsp = value; }
            }

            public double ProdTotal;
            public double ProdTotal65VolUtil;

            double Queda;

            public double EnergiaArmazenada
            {
                get
                {

                    if (InDadger || (IsFict && reserv.usinas[CodReal.Value].InDadger))
                    {

                        var earm = VolIni * ProdTotal;
                        System.Diagnostics.Debug.WriteLine(Usina + "\t" + ProdTotal.ToString() + "\t" + VolIni + "\t" + ((1 / FATORh) * earm).ToString() + "\t" + Mercado.ToString());
                        return (float)((1 / FATOR) * earm);
                    }
                    else
                        return 0;

                }
            }

            public void atualizaQueda()
            {

                double cotaRelativa, vIni, vUtil, v1, v2;

                if (Reg == "D")
                    cotaRelativa = CotaMax;
                else
                {
                    vIni = VolIni;
                    if (vIni != 0)
                    {
                        vUtil = vIni + VolMin;
                        v1 = vUtil;
                        v2 = VolMin;
                        cotaRelativa = PCV0 * (v1 - v2);
                        cotaRelativa += (PCV1 / 2) * (v1 * v1 - v2 * v2);
                        cotaRelativa += (PCV2 / 3) * (v1 * v1 * v1 - v2 * v2 * v2);
                        cotaRelativa += (PCV3 / 4) * (v1 * v1 * v1 * v1 - v2 * v2 * v2 * v2);
                        cotaRelativa += (PCV4 / 5) * (v1 * v1 * v1 * v1 * v1 - v2 * v2 * v2 * v2 * v2);
                        cotaRelativa = cotaRelativa * (1 / vIni);
                    }
                    else
                        cotaRelativa = CotaMin;
                }

                var queda = cotaRelativa - CanalFugaMed;

                if (PerdaTipo == 1)
                    queda *= (100 - PerdaVal) / 100;
                else
                    queda -= PerdaVal;

                Queda = queda;
            }

            public double ProdCalc
            {
                get
                {
                    return
                        (InDadger || InJusEna) ? Queda * ProdEsp : 0;
                }
            }

            public int Mercado { get; set; }
            public int Cod { get; set; }
            public string Usina { get; set; }
            public int? Jusante { get; set; }
            int? jusena;
            public int? JusEna { get { return jusena.HasValue ? jusena : Jusante; } set { jusena = value; } }
            public double PerdaVal { get; set; }
            public int PerdaTipo { get; set; }
            public string Reg { get; set; }
            public double CanalFugaMed { get; set; }
            public double VolMax { get; set; }
            public double VolMin { get; set; }

            public double? VolMaxRestricao { get; set; }
            public double? VolMinRestricao { get; set; }

            private double volRef;
            public double VolRef
            {
                get
                {

                    if (Reg == "M")
                        return 0.65f * VolUtil + VolMin;
                    else
                        return volRef;

                }
            }
            public double CotaRef
            {
                get
                {
                    return
                        PCV0 +
                        PCV1 * VolRef +
                        PCV2 * VolRef * VolRef +
                        PCV3 * VolRef * VolRef * VolRef +
                        PCV4 * VolRef * VolRef * VolRef * VolRef;
                }
            }
            public double QuedaRef
            {
                get
                {
                    var queda = CotaRef - CanalFugaMed;

                    if (PerdaTipo == 1)
                        queda *= (100 - PerdaVal) / 100;
                    else
                        queda -= PerdaVal;

                    return queda;
                }
            }
            public double Prod65VolUtil
            {
                get
                {
                    return (InDadger || InJusEna) ? ProdEsp * QuedaRef : 0;
                }
            }

            public double CotaMax { get; set; }
            public double PCV0 { get; set; }
            public double PCV1 { get; set; }
            public double PCV2 { get; set; }
            public double PCV3 { get; set; }
            public double PCV4 { get; set; }
            public double CotaMin { get; set; }

            public int Posto { get; set; }

            public double Ez = 1.0f;
            public int? TempoViagem;
            public float?[] VazaoIncrementalPassada;
            public int?[] VazaoIncremental;
            public float?[] VazaoDefluentePassada;
            public bool PARTIF = false;
            public int? VINCR;


            public int NumUnidadeBase { get; set; }

            public int Ree { get; set; }



            public double GetVolumePorCota(double cota)
            {

                var a = 0d;
                var b = 100d;

                var f = new Func<double, double>(x => Cota(x) - cota);

                var itMax = 10;

                if (cota <= Cota(a)) return 0;
                else if (cota >= Cota(b)) return 100;
                else
                {

                    for (int it = 0; it < itMax; it++)
                    {


                        var fa = f(a);
                        var fb = f(b);

                        var c = (a * fb - b * fa) / (fb - fa);
                        var fc = f(c);

                        if (c <= 0) return 0;
                        else if (c >= 100) return 100;
                        else if (fc < 0.0001) return c;
                        else
                        {
                            if (Math.Sign(fa) == Math.Sign(fc)) a = c;
                            if (Math.Sign(fb) == Math.Sign(fc)) b = c;
                        }
                    }
                    throw new Exception("Solução não encontrada");
                }
            }

            /// <summary>            /// 
            /// </summary>
            /// <param name="volumePercentual">de 0 à 100</param>
            /// <returns></returns>
            double Cota(double volumePercentual)
            {

                var vol = (volumePercentual / 100d) * VolUtil + VolMin;

                return PCV0 +
                        PCV1 * vol +
                        PCV2 * vol * vol +
                        PCV3 * vol * vol * vol +
                        PCV4 * vol * vol * vol * vol;
            }


        }


        public static Dictionary<int, string> uhe_ree = new Dictionary<int, string>{
{117   ,"1 - SUDESTE"},
{118   ,"1 - SUDESTE"},
{119   ,"1 - SUDESTE"},
{120   ,"1 - SUDESTE"},
{121   ,"1 - SUDESTE"},
{122   ,"1 - SUDESTE"},
{123   ,"1 - SUDESTE"},
{124   ,"1 - SUDESTE"},
{126   ,"1 - SUDESTE"},
{127   ,"1 - SUDESTE"},
{129   ,"1 - SUDESTE"},
{130   ,"1 - SUDESTE"},
{131   ,"1 - SUDESTE"},
{132   ,"1 - SUDESTE"},
{133   ,"1 - SUDESTE"},
{134   ,"1 - SUDESTE"},
{135   ,"1 - SUDESTE"},
{139   ,"1 - SUDESTE"},
{141   ,"1 - SUDESTE"},
{143   ,"1 - SUDESTE"},
{144   ,"1 - SUDESTE"},
{148   ,"1 - SUDESTE"},
{155   ,"1 - SUDESTE"},
{156   ,"1 - SUDESTE"},
{162   ,"1 - SUDESTE"},
{192   ,"1 - SUDESTE"},
{193   ,"1 - SUDESTE"},
{195   ,"1 - SUDESTE"},
{217   ,"1 - SUDESTE"},
{251   ,"1 - SUDESTE"},
{252   ,"1 - SUDESTE"},
{253   ,"1 - SUDESTE"},
{257   ,"1 - SUDESTE"},
{261   ,"1 - SUDESTE"},
{278   ,"1 - SUDESTE"},
{281   ,"1 - SUDESTE"},
{283   ,"1 - SUDESTE"},
{304   ,"1 - SUDESTE"},
{305   ,"1 - SUDESTE"},
{196   ,"6 - MADEIRA"},
{276   ,"6 - MADEIRA"},
{279   ,"6 - MADEIRA"},
{285   ,"6 - MADEIRA"},
{287   ,"6 - MADEIRA"},
{310   ,"6 - MADEIRA"},
{227   ,"7 - TPIRES"},
{228   ,"7 - TPIRES"},
{229   ,"7 - TPIRES"},
{230   ,"7 - TPIRES"},
{66    ,"5 - ITAIPU"},
{1     ,"10 - PARANA"},
{2     ,"10 - PARANA"},
{4     ,"10 - PARANA"},
{6     ,"10 - PARANA"},
{7     ,"10 - PARANA"},
{8     ,"10 - PARANA"},
{9     ,"10 - PARANA"},
{10    ,"10 - PARANA"},
{11    ,"10 - PARANA"},
{12    ,"10 - PARANA"},
{14    ,"10 - PARANA"},
{15    ,"10 - PARANA"},
{16    ,"10 - PARANA"},
{17    ,"10 - PARANA"},
{18    ,"10 - PARANA"},
{20    ,"10 - PARANA"},
{21    ,"10 - PARANA"},
{24    ,"10 - PARANA"},
{25    ,"10 - PARANA"},
{26    ,"10 - PARANA"},
{27    ,"10 - PARANA"},
{28    ,"10 - PARANA"},
{29    ,"10 - PARANA"},
{30    ,"10 - PARANA"},
{31    ,"10 - PARANA"},
{32    ,"10 - PARANA"},
{33    ,"10 - PARANA"},
{34    ,"10 - PARANA"},
{37    ,"10 - PARANA"},
{38    ,"10 - PARANA"},
{39    ,"10 - PARANA"},
{40    ,"10 - PARANA"},
{42    ,"10 - PARANA"},
{43    ,"10 - PARANA"},
{44    ,"10 - PARANA"},
{45    ,"10 - PARANA"},
{46    ,"10 - PARANA"},
{203   ,"10 - PARANA"},
{241   ,"10 - PARANA"},
{262   ,"10 - PARANA"},
{290   ,"10 - PARANA"},
{311   ,"10 - PARANA"},
{312   ,"10 - PARANA"},
{315   ,"10 - PARANA"},
{47    ,"12 - PRNPANEMA"},
{48    ,"12 - PRNPANEMA"},
{49    ,"12 - PRNPANEMA"},
{50    ,"12 - PRNPANEMA"},
{51    ,"12 - PRNPANEMA"},
{52    ,"12 - PRNPANEMA"},
{61    ,"12 - PRNPANEMA"},
{62    ,"12 - PRNPANEMA"},
{63    ,"12 - PRNPANEMA"},
{249   ,"12 - PRNPANEMA"},
{318   ,"12 - PRNPANEMA"},
{319   ,"12 - PRNPANEMA"},
{86    ,"2 - SUL"},
{89    ,"2 - SUL"},
{90    ,"2 - SUL"},
{91    ,"2 - SUL"},
{92    ,"2 - SUL"},
{93    ,"2 - SUL"},
{94    ,"2 - SUL"},
{95    ,"2 - SUL"},
{97    ,"2 - SUL"},
{98    ,"2 - SUL"},
{99    ,"2 - SUL"},
{101    ,"2 - SUL"},
{102    ,"2 - SUL"},
{103    ,"2 - SUL"},
{110    ,"2 - SUL"},
{111    ,"2 - SUL"},
{112    ,"2 - SUL"},
{113    ,"2 - SUL"},
{114    ,"2 - SUL"},
{115    ,"2 - SUL"},
{215    ,"2 - SUL"},
{54    ,"11 - IGUACU"},
{57    ,"11 - IGUACU"},
{71    ,"11 - IGUACU"},
{72    ,"11 - IGUACU"},
{73    ,"11 - IGUACU"},
{74    ,"11 - IGUACU"},
{76    ,"11 - IGUACU"},
{77    ,"11 - IGUACU"},
{78    ,"11 - IGUACU"},
{82    ,"11 - IGUACU"},
{83    ,"11 - IGUACU"},
{154    ,"3 - NORDESTE"},
{169    ,"3 - NORDESTE"},
{172    ,"3 - NORDESTE"},
{176    ,"3 - NORDESTE"},
{178    ,"3 - NORDESTE"},
{189    ,"3 - NORDESTE"},
{190    ,"3 - NORDESTE"},
{294    ,"3 - NORDESTE"},
{295    ,"3 - NORDESTE"},
{298    ,"3 - NORDESTE"},
{308    ,"3 - NORDESTE"},
{267    ,"4 - NORTE"},
{275    ,"4 - NORTE"},
{291    ,"4 - NORTE"},
{292    ,"4 - NORTE"},
{302    ,"4 - NORTE"},
{303    ,"4 - NORTE"},
{306    ,"4 - NORTE"},
{272    ,"8 - BMONTE"},
{288    ,"8 - BMONTE"},
{314    ,"8 - BMONTE"},
{204    ,"9 - MAN"},
{277    ,"9 - MAN"},
{280    ,"9 - MAN"},
{284    ,"9 - MAN"},
{286    ,"9 - MAN"}
};


    }
}

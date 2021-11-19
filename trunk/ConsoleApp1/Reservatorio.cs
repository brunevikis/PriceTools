using ConsoleApp1.Dadger;
using ConsoleApp1.EntdadosDat;
using ConsoleApp1.Decomp;
//using Compass.CommomLibrary.HidrDat;
//using Compass.CommomLibrary.Prevs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Compass.ExcelTools;
//using Compass.ExcelTools.Templates;
//using System.Windows.Forms;

namespace ConsoleApp1
{
    public class Reservatorio
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configH"></param>
        /// <param name="earmTargetLevel"></param>
        /// <param name="earmMax">Desconsiderado caso a meta seja em valor absoluto</param>
        /// <returns></returns>
        public static void SetUHBlockFixado(ConfigH configH, double[] earmTargetLevel, double[] earmMax, List<ConsoleApp1.Decomp.ConfigH.Dados_Fixa> Fixa_UH)
        {
            var earmTarget = new double[earmTargetLevel.Length];

            if (earmTargetLevel.All(x => x <= 1))
            {
                for (int x = 0; x < configH.index_sistemas.Count; x++)
                {
                    earmTarget[x] = earmTargetLevel[x] * earmMax[x];
                }
            }
            else if (earmTargetLevel.All(x => x <= 100))
            {
                for (int x = 0; x < configH.index_sistemas.Count; x++)
                {
                    earmTarget[x] = (earmTargetLevel[x] / 100f) * earmMax[x];
                }

            }
            else
            {
                earmTarget = earmTargetLevel;
            }

            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                //UhBlock uhResult = 
                buildReservFixado(configH, earmTarget, earmMax, Fixa_UH);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                //UhBlock uhResult = 
                buildReservFixado(configH, earmTarget, earmMax, Fixa_UH);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            }
            else if (configH.baseDoc is ConsoleApp1.ConfhdDat.ConfhdDat)
            {
                buildReservNW(configH, earmTarget);
            }

            //return uhResult;
        }

        public static void SetUHBlockREEFixado(ConfigH configH, double[] earmTargetLevel, double[] earmMax, List<ConsoleApp1.Decomp.ConfigH.Dados_Fixa> Fixa_UH)
        {
            var earmTarget = new double[earmTargetLevel.Length];

            if (earmTargetLevel.All(x => x <= 1))
            {
                for (int x = 0; x < configH.index_Ree.Count; x++)
                {
                    earmTarget[x] = earmTargetLevel[x] * earmMax[x];
                }
            }
            else if (earmTargetLevel.All(x => x <= 100))
            {
                for (int x = 0; x < configH.index_Ree.Count; x++)
                {
                    earmTarget[x] = (earmTargetLevel[x] / 100f) * earmMax[x];
                }

            }
            else
            {
                earmTarget = earmTargetLevel;
            }

            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                //UhBlock uhResult = 
                buildReservREEFixado(configH, earmTarget, earmMax, Fixa_UH);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                //UhBlock uhResult = 
                buildReservREEFixado(configH, earmTarget, earmMax, Fixa_UH);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            }
            else if (configH.baseDoc is ConsoleApp1.ConfhdDat.ConfhdDat)
            {
                buildReservNW(configH, earmTarget);
            }

            //return uhResult;
        }
        public static void SetUHBlock(ConfigH configH, double[] earmTargetLevel, double[] earmMax)
        {
            var earmTarget = new double[earmTargetLevel.Length];

            if (earmTargetLevel.All(x => x <= 1))
            {
                for (int x = 0; x < configH.index_sistemas.Count; x++)
                {
                    earmTarget[x] = earmTargetLevel[x] * earmMax[x];
                }
            }
            else if (earmTargetLevel.All(x => x <= 100))
            {
                for (int x = 0; x < configH.index_sistemas.Count; x++)
                {
                    earmTarget[x] = (earmTargetLevel[x] / 100f) * earmMax[x];
                }

            }
            else
            {
                earmTarget = earmTargetLevel;
            }

            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                //UhBlock uhResult = 
                buildReserv(configH, earmTarget, earmMax);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                //UhBlock uhResult = 
                buildReserv(configH, earmTarget, earmMax);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            }
            else if (configH.baseDoc is ConsoleApp1.ConfhdDat.ConfhdDat)
            {
                buildReservNW(configH, earmTarget);
            }

            //return uhResult;
        }

        public static void SetUHBlockREE(ConfigH configH, double[] earmTargetLevel, double[] earmMax)
        {
            var earmTarget = new double[earmTargetLevel.Length];

            if (earmTargetLevel.All(x => x <= 1))
            {
                for (int x = 0; x < configH.index_Ree.Count; x++)
                {
                    earmTarget[x] = earmTargetLevel[x] * earmMax[x];
                }
            }
            else if (earmTargetLevel.All(x => x <= 100))
            {
                for (int x = 0; x < configH.index_Ree.Count; x++)
                {
                    earmTarget[x] = (earmTargetLevel[x] / 100f) * earmMax[x];
                }

            }
            else
            {
                earmTarget = earmTargetLevel;
            }

            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                //UhBlock uhResult = 
                buildReservREE(configH, earmTarget, earmMax);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                //UhBlock uhResult = 
                buildReservREE(configH, earmTarget, earmMax);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            }
            else if (configH.baseDoc is ConsoleApp1.ConfhdDat.ConfhdDat)
            {
                buildReservNW(configH, earmTarget);
            }

            //return uhResult;
        }


        static void buildReserv(ConfigH configH, double[] earmTarget, double[] earmMax /*List<Infosheet.Dados_Fixa> Fixa_UH = null*/)
        {

            //UhBlock uhResult = new UhBlock();


            //goalSeek(configH, earmTarget, earmMax, Fixa_UH);
            goalSeek(configH, earmTarget, earmMax);

            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                //ordenação deve obedecer a proveniente do deckBase.uh
                foreach (var uhBase in ((ConsoleApp1.Dadger.Dadger)configH.baseDoc).BlocoUh)
                {

                    //var newUh = (UhLine)uhBase.Clone();
                    var uhe = configH.usinas[uhBase.Usina];

                    uhBase.VolIniPerc = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;


                    //uhResult.Add(newUh);
                }
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                //ordenação deve obedecer a proveniente do deckBase.uh
                foreach (var uhBase in ((ConsoleApp1.EntdadosDat.EntdadosDat)configH.baseDoc).BlocoUh)
                {

                    //var newUh = (UhLine)uhBase.Clone();
                    var uhe = configH.usinas[uhBase.Usina];

                    uhBase.VolArm = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;


                    //uhResult.Add(newUh);
                }
            }




            //return uhResult;
        }

        static void buildReservFixado(ConfigH configH, double[] earmTarget, double[] earmMax, List<ConsoleApp1.Decomp.ConfigH.Dados_Fixa> Fixa_UH = null)
        {

            //UhBlock uhResult = new UhBlock();


            goalSeekFixado(configH, earmTarget, earmMax, Fixa_UH);

            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                //ordenação deve obedecer a proveniente do deckBase.uh
                foreach (var uhBase in ((ConsoleApp1.Dadger.Dadger)configH.baseDoc).BlocoUh)
                {

                    //var newUh = (UhLine)uhBase.Clone();
                    var uhe = configH.usinas[uhBase.Usina];

                    uhBase.VolIniPerc = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;


                    //uhResult.Add(newUh);
                }
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                //ordenação deve obedecer a proveniente do deckBase.uh
                foreach (var uhBase in ((ConsoleApp1.EntdadosDat.EntdadosDat)configH.baseDoc).BlocoUh)
                {

                    //var newUh = (UhLine)uhBase.Clone();
                    var uhe = configH.usinas[uhBase.Usina];

                    uhBase.VolArm = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;


                    //uhResult.Add(newUh);
                }
            }




            //return uhResult;
        }
        static void buildReservREE(ConfigH configH, double[] earmTarget, double[] earmMax /*List<Infosheet.Dados_Fixa> Fixa_UH = null*/)
        {

            //UhBlock uhResult = new UhBlock();


            //goalSeekREE(configH, earmTarget, earmMax, Fixa_UH);
            goalSeekREE(configH, earmTarget, earmMax);

            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                //ordenação deve obedecer a proveniente do deckBase.uh
                foreach (var uhBase in ((ConsoleApp1.Dadger.Dadger)configH.baseDoc).BlocoUh)
                {

                    //var newUh = (UhLine)uhBase.Clone();
                    var uhe = configH.usinas[uhBase.Usina];

                    uhBase.VolIniPerc = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;


                    //uhResult.Add(newUh);
                }
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                //ordenação deve obedecer a proveniente do deckBase.uh
                foreach (var uhBase in ((ConsoleApp1.EntdadosDat.EntdadosDat)configH.baseDoc).BlocoUh)
                {

                    //var newUh = (UhLine)uhBase.Clone();
                    var uhe = configH.usinas[uhBase.Usina];

                    uhBase.VolArm = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;


                    //uhResult.Add(newUh);
                }
            }




            //return uhResult;
        }

        static void buildReservREEFixado(ConfigH configH, double[] earmTarget, double[] earmMax ,List<ConsoleApp1.Decomp.ConfigH.Dados_Fixa> Fixa_UH = null)
        {

            //UhBlock uhResult = new UhBlock();


            goalSeekREEFixado(configH, earmTarget, earmMax, Fixa_UH);

            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                //ordenação deve obedecer a proveniente do deckBase.uh
                foreach (var uhBase in ((ConsoleApp1.Dadger.Dadger)configH.baseDoc).BlocoUh)
                {

                    //var newUh = (UhLine)uhBase.Clone();
                    var uhe = configH.usinas[uhBase.Usina];

                    uhBase.VolIniPerc = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;


                    //uhResult.Add(newUh);
                }
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                //ordenação deve obedecer a proveniente do deckBase.uh
                foreach (var uhBase in ((ConsoleApp1.EntdadosDat.EntdadosDat)configH.baseDoc).BlocoUh)
                {

                    //var newUh = (UhLine)uhBase.Clone();
                    var uhe = configH.usinas[uhBase.Usina];

                    uhBase.VolArm = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;


                    //uhResult.Add(newUh);
                }
            }




            //return uhResult;
        }
        static void buildReservNW(ConfigH configH, double[] earmTarget)
        {


            goalSeek(configH, earmTarget);


            //ordenação deve obedecer a proveniente do deckBase.uh
            foreach (var uh in (ConsoleApp1.ConfhdDat.ConfhdDat)configH.baseDoc)
            {

                var uhe = configH.usinas[uh.Cod];
                uh.VolUtil = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;
            }
        }

        public static Boolean Meta_Fixa_UhFixado(ConfigH configH, double[] earmTarget, double[] earm_Max, List<ConsoleApp1.Decomp.ConfigH.Dados_Fixa> Fixa_UH = null)
        {
            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                foreach (var uhBase in ((ConsoleApp1.Dadger.Dadger)configH.baseDoc).BlocoUh)
                {
                    foreach (var item in Fixa_UH)
                    {
                        if (uhBase.Usina == item.Posto)
                        {
                            if (item.Volini != null)
                            {
                                uhBase.VolIniPerc = (double)item.Volini;
                            }
                        }
                    }
                }
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                foreach (var uhBase in ((ConsoleApp1.EntdadosDat.EntdadosDat)configH.baseDoc).BlocoUh)
                {
                    foreach (var item in Fixa_UH)
                    {
                        if (uhBase.Usina == item.Posto)
                        {
                            if (item.Volini != null)
                            {
                                uhBase.VolArm = (float)item.Volini;
                            }
                        }
                    }
                }
            }

            foreach (var usina in configH.Usinas)
            {
                foreach (var item in Fixa_UH)
                {
                    if (usina.Cod == item.Posto)
                    {
                        if (item.Volini != null)
                        {
                            usina.travado = true;
                            usina.VolIni = (double)item.Volini * usina.VolUtil / 100f;
                        }

                    }

                }
            }

            var earm_UH = configH.GetEarms();
            //   var earm_Max = earmMax;

            var desvio_max = Math.Max(Math.Abs((earmTarget[0] / earm_Max[0]) - (earm_UH[0] / earm_Max[0])), Math.Max(Math.Abs((earmTarget[1] / earm_Max[1]) - (earm_UH[1] / earm_Max[1])), Math.Max(Math.Abs((earmTarget[2] / earm_Max[2]) - (earm_UH[2] / earm_Max[2])), Math.Abs((earmTarget[3] / earm_Max[3]) - (earm_UH[3] / earm_Max[3])))));

            if (desvio_max > 0.0001)
            {
                return false;

            }
            else
            {
                return true;
            }

        }

        public static Boolean Meta_Fixa_UhREEFixado(ConfigH configH, double[] earmTarget, double[] earm_Max, List<ConsoleApp1.Decomp.ConfigH.Dados_Fixa> Fixa_UH = null)
        {
            if (configH.baseDoc is ConsoleApp1.Dadger.Dadger)
            {
                foreach (var uhBase in ((ConsoleApp1.Dadger.Dadger)configH.baseDoc).BlocoUh)
                {
                    foreach (var item in Fixa_UH)
                    {
                        if (uhBase.Usina == item.Posto)
                        {
                            if (item.Volini != null)
                            {
                                uhBase.VolIniPerc = (double)item.Volini;
                            }

                        }
                    }
                }
            }
            else if (configH.baseDoc is ConsoleApp1.EntdadosDat.EntdadosDat)
            {
                foreach (var uhBase in ((ConsoleApp1.EntdadosDat.EntdadosDat)configH.baseDoc).BlocoUh)
                {
                    foreach (var item in Fixa_UH)
                    {
                        if (uhBase.Usina == item.Posto)
                        {
                            if (item.Volini != null)
                            {
                                uhBase.VolArm = (float)item.Volini;
                            }

                        }
                    }
                }
            }

            foreach (var usina in configH.Usinas)
            {
                foreach (var item in Fixa_UH)
                {
                    if (usina.Cod == item.Posto)
                    {
                        if (item.Volini != null)
                        {
                            usina.travado = true;
                            usina.VolIni = (double)item.Volini * usina.VolUtil / 100f;
                        }

                    }

                }
            }

            var earm_UH = configH.GetEarmsREE();
            //   var earm_Max = earmMax;

            int contEarm = earm_UH.Count();

            double[] desviosArray = new double[contEarm];
            for (int d = 0; d < contEarm; d++)
            {
                desviosArray[d] = Math.Abs((earmTarget[d] / earm_Max[d]) - (earm_UH[d] / earm_Max[d]));
            }
            //var desvio_max = Math.Max(Math.Abs((earmTarget[0] / earm_Max[0]) - (earm_UH[0] / earm_Max[0])), Math.Max(Math.Abs((earmTarget[1] / earm_Max[1]) - (earm_UH[1] / earm_Max[1])), Math.Max(Math.Abs((earmTarget[2] / earm_Max[2]) - (earm_UH[2] / earm_Max[2])), Math.Abs((earmTarget[3] / earm_Max[3]) - (earm_UH[3] / earm_Max[3])))));
            var desvio_max = desviosArray.Max();

            if (desvio_max > 0.0001)
            {
                return false;

            }
            else
            {
                return true;
            }

        }


        static void goalSeek(ConfigH configH, double[] earmTarget, double[] earmMax = null /*List<Infosheet.Dados_Fixa> Fixa_UH = null*/)
        {


            var fatores = new double[configH.index_sistemas.Max(t => t.Item2) + 1];
            for (int i = 0; i < fatores.Length; i++) fatores[i] = 1;

            double erro = 100;
            double erroAnterior = 0;
            int itNumber = 0;
            int itMax = 100;
            Boolean desvio = true;

            do
            {

                //Travar Usinas Norte
                //if (Fixa_UH != null)
                //{
                //    if (Fixa_UH.Count > 0)
                //    {
                //        desvio = false;
                //        desvio = Meta_Fixa_Uh(configH, earmTarget, earmMax, Fixa_UH);
                //        itMax = 1000;
                //    }
                //}


                //Fim da trava

                var earmCurrent = configH.GetEarms();

                erroAnterior = erro;
                erro = 0;

                for (int x = 0; x < configH.index_sistemas.Count; x++)
                {

                    var sis = configH.index_sistemas[x].Item2;

                    erro = erro + Math.Abs(earmCurrent[x] - earmTarget[x]);
                    var f = (earmTarget[x] / earmCurrent[x]);
                    fatores[sis] = f;
                }

                //se erro pequeno ou não houver grande variação parar iteração
                if ((erro < 2 || Math.Abs(erroAnterior - erro) < 1) && desvio == true)
                    break;


                //atualiza volumes e queda
                foreach (var uhe in configH.Usinas.Where(u => !u.IsFict && u.VolIni > 0))
                {

                    if (!uhe.CodFicticia.HasValue)
                    {
                        uhe.VolIni *= fatores[uhe.Mercado];
                    }
                    else
                    {
                        // se influenciar em outro sistema, levar em conta o fator do sistema afetado
                        // f = ( fs^3 * ff ) ^ (1/4)
                        var f = (float)Math.Pow(fatores[uhe.Mercado] *
                            fatores[uhe.Mercado] *
                            fatores[uhe.Mercado] *
                            fatores[configH.usinas[uhe.CodFicticia.Value].Mercado],
                            1d / 4d);
                        uhe.VolIni *= f;
                        //configH.usinas[uhe.CodFicticia.Value].atualizaQueda();
                    }
                }

            } while (++itNumber < itMax);

            //if (itNumber >= itMax)
            //{
            //    MessageBox.Show("Número Máximo de Iterações atingido");
            //}
        }

        static void goalSeekFixado(ConfigH configH, double[] earmTarget, double[] earmMax = null, List<ConsoleApp1.Decomp.ConfigH.Dados_Fixa> Fixa_UH = null)
        {


            var fatores = new double[configH.index_sistemas.Max(t => t.Item2) + 1];
            for (int i = 0; i < fatores.Length; i++) fatores[i] = 1;

            double erro = 100;
            double erroAnterior = 0;
            int itNumber = 0;
            int itMax = 100;
            Boolean desvio = true;

            do
            {

                //Travar Usinas Norte
                if (Fixa_UH != null)
                {
                    if (Fixa_UH.Count > 0)
                    {
                        desvio = false;
                        desvio = Meta_Fixa_UhFixado(configH, earmTarget, earmMax, Fixa_UH);
                        itMax = 1000;
                    }
                }


                //Fim da trava

                var earmCurrent = configH.GetEarms();

                erroAnterior = erro;
                erro = 0;

                for (int x = 0; x < configH.index_sistemas.Count; x++)
                {

                    var sis = configH.index_sistemas[x].Item2;

                    erro = erro + Math.Abs(earmCurrent[x] - earmTarget[x]);
                    var f = (earmTarget[x] / earmCurrent[x]);
                    fatores[sis] = f;
                }

                //se erro pequeno ou não houver grande variação parar iteração
                if ((erro < 2 || Math.Abs(erroAnterior - erro) < 1) && desvio == true)
                    break;


                //atualiza volumes e queda
                foreach (var uhe in configH.Usinas.Where(u => !u.IsFict && u.VolIni > 0))
                {

                    if (!uhe.CodFicticia.HasValue)
                    {
                        uhe.VolIni *= fatores[uhe.Mercado];
                    }
                    else
                    {
                        // se influenciar em outro sistema, levar em conta o fator do sistema afetado
                        // f = ( fs^3 * ff ) ^ (1/4)
                        var f = (float)Math.Pow(fatores[uhe.Mercado] *
                            fatores[uhe.Mercado] *
                            fatores[uhe.Mercado] *
                            fatores[configH.usinas[uhe.CodFicticia.Value].Mercado],
                            1d / 4d);
                        uhe.VolIni *= f;
                        //configH.usinas[uhe.CodFicticia.Value].atualizaQueda();
                    }
                }

            } while (++itNumber < itMax);

            //if (itNumber >= itMax)
            //{
            //    MessageBox.Show("Número Máximo de Iterações atingido");
            //}
        }

        static void goalSeekREE(ConfigH configH, double[] earmTarget, double[] earmMax = null/* List<Infosheet.Dados_Fixa> Fixa_UH = null*/)
        {


            var fatores = new double[configH.index_Ree.Max(t => Convert.ToInt32(t.Item2.Split('-').First().Trim())) + 1];
            for (int i = 0; i < fatores.Length; i++) fatores[i] = 1;

            double erro = 100;
            double erroAnterior = 0;
            int itNumber = 0;
            int itMax = 100;
            Boolean desvio = true;

            //tratamento para a usina e REE de itaipu(como existe somente uma usina em itaipu e seu valor pode ser zero seria impossivel atingir meta)

            int indexITA = configH.index_Ree.IndexOf(configH.index_Ree.Where(x => x.Item2.ToLower().Contains("itaipu")).First());

            double earmIta = earmTarget[indexITA];

            var usinaIta = configH.Usinas.Where(x => x.Cod == 66).First();
            usinaIta.VolIni = earmIta / ((1 / (0.0036 * 730.5)) * usinaIta.ProdTotal);

            //fim tratamento itaipu

            do
            {

                //Travar Usinas Norte
                //if (Fixa_UH != null)
                //{
                //    if (Fixa_UH.Count > 0)
                //    {
                //        desvio = false;
                //        desvio = Meta_Fixa_UhREE(configH, earmTarget, earmMax, Fixa_UH);
                //        itMax = 1000;
                //    }
                //}


                //Fim da trava


                var earmCurrent = configH.GetEarmsREE();

                erroAnterior = erro;
                erro = 0;

                for (int x = 0; x < configH.index_Ree.Count; x++)
                {

                    var ree = Convert.ToInt32(configH.index_Ree[x].Item2.Split('-').First().Trim());

                    erro = erro + Math.Abs(earmCurrent[x] - earmTarget[x]);
                    var f = (earmTarget[x] / earmCurrent[x]);
                    fatores[ree] = f;
                }

                //se erro pequeno ou não houver grande variação parar iteração
                if ((erro < 2 || Math.Abs(erroAnterior - erro) < 1) && desvio == true)
                    break;


                //atualiza volumes e queda
                foreach (var uhe in configH.Usinas.Where(u => !u.IsFict && u.VolIni > 0))
                {

                    if (!uhe.CodFicticia.HasValue)
                    {
                        if (uhe.Ree > 0)
                        {
                            var reeNum = Convert.ToInt32(ConfigH.uhe_ree[uhe.Cod].Split('-').First().Trim());
                            uhe.VolIni *= fatores[reeNum];
                        }
                        
                    }
                    else
                    {
                       
                        if (uhe.Ree > 0) 
                        {
                            // se influenciar em outro sistema, levar em conta o fator do sistema afetado
                            // f = ( fs^3 * ff ) ^ (1/4)
                            var reeNum = Convert.ToInt32(ConfigH.uhe_ree[uhe.Cod].Split('-').First().Trim());
                            var reeNumFict = Convert.ToInt32(ConfigH.uhe_ree[configH.usinas[uhe.CodFicticia.Value].Cod].Split('-').First().Trim());

                            var f = (float)Math.Pow(fatores[reeNum] *
                                fatores[reeNum] *
                                fatores[reeNum] *
                                fatores[reeNumFict],
                                1d / 4d);
                            uhe.VolIni *= f;
                            //configH.usinas[uhe.CodFicticia.Value].atualizaQueda();
                        }

                    }
                }
            } while (++itNumber < itMax);

            //if (itNumber >= itMax)
            //{
            //    MessageBox.Show("Número Máximo de Iterações atingido");
            //}
        }

        static void goalSeekREEFixado(ConfigH configH, double[] earmTarget, double[] earmMax = null, List<ConsoleApp1.Decomp.ConfigH.Dados_Fixa> Fixa_UH = null)
        {


            var fatores = new double[configH.index_Ree.Max(t => Convert.ToInt32(t.Item2.Split('-').First().Trim())) + 1];
            for (int i = 0; i < fatores.Length; i++) fatores[i] = 1;

            double erro = 100;
            double erroAnterior = 0;
            int itNumber = 0;
            int itMax = 100;
            Boolean desvio = true;

            //tratamento para a usina e REE de itaipu(como existe somente uma usina em itaipu e seu valor pode ser zero seria impossivel atingir meta)

            int indexITA = configH.index_Ree.IndexOf(configH.index_Ree.Where(x => x.Item2.ToLower().Contains("itaipu")).First());

            double earmIta = earmTarget[indexITA];

            var usinaIta = configH.Usinas.Where(x => x.Cod == 66).First();
            usinaIta.VolIni = earmIta / ((1 / (0.0036 * 730.5)) * usinaIta.ProdTotal);

            //fim tratamento itaipu

            do
            {

                //Travar Usinas Norte
                if (Fixa_UH != null)
                {
                    if (Fixa_UH.Count > 0)
                    {
                        desvio = false;
                        desvio = Meta_Fixa_UhREEFixado(configH, earmTarget, earmMax, Fixa_UH);
                        itMax = 1000;
                    }
                }


                //Fim da trava


                var earmCurrent = configH.GetEarmsREE();

                erroAnterior = erro;
                erro = 0;

                for (int x = 0; x < configH.index_Ree.Count; x++)
                {

                    var ree = Convert.ToInt32(configH.index_Ree[x].Item2.Split('-').First().Trim());

                    erro = erro + Math.Abs(earmCurrent[x] - earmTarget[x]);
                    var f = (earmTarget[x] / earmCurrent[x]);
                    fatores[ree] = f;
                }

                //se erro pequeno ou não houver grande variação parar iteração
                if ((erro < 2 || Math.Abs(erroAnterior - erro) < 1) && desvio == true)
                    break;


                //atualiza volumes e queda
                foreach (var uhe in configH.Usinas.Where(u => !u.IsFict && u.VolIni > 0))
                {

                    if (!uhe.CodFicticia.HasValue)
                    {
                        if (uhe.Ree > 0)
                        {
                            var reeNum = Convert.ToInt32(ConfigH.uhe_ree[uhe.Cod].Split('-').First().Trim());
                            uhe.VolIni *= fatores[reeNum];
                        }

                    }
                    else
                    {

                        if (uhe.Ree > 0)
                        {
                            // se influenciar em outro sistema, levar em conta o fator do sistema afetado
                            // f = ( fs^3 * ff ) ^ (1/4)
                            var reeNum = Convert.ToInt32(ConfigH.uhe_ree[uhe.Cod].Split('-').First().Trim());
                            var reeNumFict = Convert.ToInt32(ConfigH.uhe_ree[configH.usinas[uhe.CodFicticia.Value].Cod].Split('-').First().Trim());

                            var f = (float)Math.Pow(fatores[reeNum] *
                                fatores[reeNum] *
                                fatores[reeNum] *
                                fatores[reeNumFict],
                                1d / 4d);
                            uhe.VolIni *= f;
                            //configH.usinas[uhe.CodFicticia.Value].atualizaQueda();
                        }

                    }
                }
            } while (++itNumber < itMax);

            //if (itNumber >= itMax)
            //{
            //    MessageBox.Show("Número Máximo de Iterações atingido");
            //}
        }

    }
}


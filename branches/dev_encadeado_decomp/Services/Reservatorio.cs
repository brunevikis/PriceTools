using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.Decomp;
using Compass.CommomLibrary.HidrDat;
using Compass.CommomLibrary.Prevs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.Services {
    public class Reservatorio {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configH"></param>
        /// <param name="earmTargetLevel"></param>
        /// <param name="earmMax">Desconsiderado caso a meta seja em valor absoluto</param>
        /// <returns></returns>
        public static void SetUHBlock(ConfigH configH, double[] earmTargetLevel, double[] earmMax) {
            var earmTarget = new double[earmTargetLevel.Length];

            if (earmTargetLevel.All(x => x <= 1)) {
                for (int x = 0; x < configH.index_sistemas.Count; x++) {
                    earmTarget[x] = earmTargetLevel[x] * earmMax[x];
                }
            } else if (earmTargetLevel.All(x => x <= 100)) {
                for (int x = 0; x < configH.index_sistemas.Count; x++) {
                    earmTarget[x] = (earmTargetLevel[x] / 100f) * earmMax[x];
                }

            } else {
                earmTarget = earmTargetLevel;
            }

            if (configH.baseDoc is Dadger) {
                //UhBlock uhResult = 
                buildReserv(configH, earmTarget);
                //((Dadger)configH.baseDoc).BlocoUh = uhResult;
            } else if (configH.baseDoc is Compass.CommomLibrary.ConfhdDat.ConfhdDat) {
                buildReservNW(configH, earmTarget);
            }

            //return uhResult;
        }

        static void buildReserv(ConfigH configH, double[] earmTarget) {

            //UhBlock uhResult = new UhBlock();


            goalSeek(configH, earmTarget);

            //ordenação deve obedecer a proveniente do deckBase.uh
            foreach (var uhBase in ((Dadger)configH.baseDoc).BlocoUh) {

                //var newUh = (UhLine)uhBase.Clone();
                var uhe = configH.usinas[uhBase.Usina];
                uhBase.VolIniPerc = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;
                //uhResult.Add(newUh);
            }

            //return uhResult;
        }

        static void buildReservNW(ConfigH configH, double[] earmTarget) {


            goalSeek(configH, earmTarget);


            //ordenação deve obedecer a proveniente do deckBase.uh
            foreach (var uh in (Compass.CommomLibrary.ConfhdDat.ConfhdDat)configH.baseDoc) {

                var uhe = configH.usinas[uh.Cod];
                uh.VolUtil = uhe.VolIni > 0 && uhe.VolUtil > 0 ? (float)Math.Round((uhe.VolIni / uhe.VolUtil) * 100f, 2) : 0f;
            }
        }

        static void goalSeek(ConfigH configH, double[] earmTarget) {

            var fatores = new double[configH.index_sistemas.Max(t => t.Item2) + 1];
            for (int i = 0; i < fatores.Length; i++) fatores[i] = 1;

            double erro = 100;
            double erroAnterior = 0;
            int itNumber = 0;

            do {

                var earmCurrent = configH.GetEarms();

                erroAnterior = erro;
                erro = 0;

                for (int x = 0; x < configH.index_sistemas.Count; x++) {

                    var sis = configH.index_sistemas[x].Item2;

                    erro = erro + Math.Abs(earmCurrent[x] - earmTarget[x]);
                    var f = (earmTarget[x] / earmCurrent[x]);
                    fatores[sis] = f;
                }

                //se erro pequeno ou não houver grande variação parar iteração
                if (erro < 2 || Math.Abs(erroAnterior - erro) < 1)
                    break;


                //atualiza volumes e queda
                foreach (var uhe in configH.Usinas.Where(u => !u.IsFict && u.VolIni > 0)) {

                    if (!uhe.CodFicticia.HasValue) {
                        uhe.VolIni *= fatores[uhe.Mercado];
                    } else {
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

            } while (++itNumber < 100);
        }

    }
}


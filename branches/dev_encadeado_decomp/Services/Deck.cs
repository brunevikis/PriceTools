using Compass.CommomLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.Services {
    public class Deck {

        /// <summary>
        /// inflexibilidade igual ao mínimo entre informado em CADTERM e EXPT/TERM
        /// </summary>
        /// <param name="cceeDeck"></param>
        public static void Ons2Ccee(Compass.CommomLibrary.Newave.Deck cceeDeck) {

            if (cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.cadterm] == null) {
                throw new Exception("Não existe arquivo CADTERM no deck para realizar a conversão. Copie o arquivo e tente novamente.");
            }

            var cadTerm = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.cadterm].Document as Compass.CommomLibrary.CadTermDat.CadTermDat;
            var confT = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.conft].Document as Compass.CommomLibrary.ConftDat.ConftDat;
            var expt = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.expt].Document as Compass.CommomLibrary.ExptDat.ExptDat;
            var term = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.term].Document as Compass.CommomLibrary.TermDat.TermDat;

            foreach (var ute in confT) {

                if (!cadTerm.Any(x => x.Num == ute.Num)) continue;

                var gtminCadTerm = cadTerm.First(x => x.Num == ute.Num).Gtmin;

                //if (ute.Existente == "EX") { //caso existente, modificar term.dat (campos 6=jan...17=dez, 18=demais anos)
                for (int c = 6; c <= 18; c++) {
                    if (gtminCadTerm < term.First(x => x.Cod == ute.Num)[c]) term.First(x => x.Cod == ute.Num)[c] = gtminCadTerm;
                }
                //} else { // se não existente, alterar expt "GTMIN"
                foreach (var exptGtmin in expt.Where(x => x.Cod == ute.Num && x.Tipo == "GTMIN")) {
                    if (gtminCadTerm < exptGtmin.Valor) {
                        exptGtmin.Valor = gtminCadTerm;
                    }
                }
                //}
            }

            term.SaveToFile(createBackup: true);
            expt.SaveToFile(createBackup: true);

        }
        
        /*
                    	
	ESTRUTURAIS
+	FU  122   1   178           
+	FU  123   1   175
+	FU  124   1   169
+	FU  125   1   172 
	
	CONJUNTURAIS
-	FU  198   1   139
-	FT  300   1   501  2             1
    FT  300   1   502  2             1

*/

        public static void Ons2Ccee(Compass.CommomLibrary.Decomp.Deck cceeDeck) {

            //"RESTRIÇÕES DE INTERCÂMBIO CONJUNTURAIS";
            var dadgerBase = ((Compass.CommomLibrary.Decomp.Deck)cceeDeck)[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
            var resDeckBase = dadgerBase.BlocoRhe.RheGrouped;


            var blocoConj = false;
            foreach (var key in resDeckBase.Keys) {
                if (key.Comment.ToUpperInvariant().Contains("BIO CONJUNTURAIS")) blocoConj = true;


                if (!blocoConj) {

                    var fs = resDeckBase[key].Where(y => (y is Compass.CommomLibrary.Dadger.FuLine)
                        || (y is Compass.CommomLibrary.Dadger.FiLine)
                        || (y is Compass.CommomLibrary.Dadger.FtLine));

                    var ok = fs.Count() == 1;
                    if (ok) {

                        var uuu = new int[] { 178, 175, 169, 172 };



                        ok &= fs.Any(z => (z is Compass.CommomLibrary.Dadger.FuLine)
                            && uuu.Contains((int)z[3])
                            );
                    }

                    if (ok) {
                        resDeckBase[key].ForEach(x => x[0] = "&" + x[0]);
                    }


                } else {


                    var fs = resDeckBase[key].Where(y => (y is Compass.CommomLibrary.Dadger.FuLine)
                        || (y is Compass.CommomLibrary.Dadger.FiLine)
                        || (y is Compass.CommomLibrary.Dadger.FtLine));

                    var ok = false;

                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FtLine && x[3] > 320); // intercambio internacional
                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FuLine && x[3] == 139);                     
                    
                    if (!ok) {
                        resDeckBase[key].ForEach(x => x[0] = "&" + x[0]);
                    }
                }
            }

            dadgerBase.SaveToFile(createBackup: true);




        }

        public static void DesfazerInviabilidades(Compass.CommomLibrary.Decomp.Deck deck, Compass.CommomLibrary.Inviab.Inviab inviabilidades) {

            var dadger = deck[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
            var hidr = deck[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat;

            var q =
                        from inv in inviabilidades.Iteracao
                        group inv by new { inv.Estagio, inv.RestricaoViolada } into invG
                        select invG.OrderByDescending(x => x.Violacao).First();



            foreach (var inviab in q.OrderByDescending(x => x.Estagio)) {
                if (inviab.TipoRestricao == "RHE" || inviab.TipoRestricao == "RHQ") {
                    IEnumerable<Compass.CommomLibrary.BaseLine> rs;
                    if (inviab.TipoRestricao == "RHE")
                        rs = dadger.BlocoRhe.Where(x => x.Restricao == inviab.CodRestricao);
                    else
                        rs = dadger.BlocoRhq.Where(x => x.Restricao == inviab.CodRestricao);

                    if (rs.Count() > 0) {
                        dynamic le;
                        if (inviab.TipoRestricao == "RHE") {
                            var ls = rs.Where(x => x is Compass.CommomLibrary.Dadger.LuLine).Select(x => (Compass.CommomLibrary.Dadger.LuLine)x);
                            le = ls.Where(x => x.Estagio <= inviab.Estagio).OrderByDescending(x => x.Estagio).FirstOrDefault();
                        } else {
                            var ls = rs.Where(x => x is Compass.CommomLibrary.Dadger.LqLine).Select(x => (Compass.CommomLibrary.Dadger.LqLine)x);
                            le = ls.Where(x => x.Estagio <= inviab.Estagio).OrderByDescending(x => x.Estagio).FirstOrDefault();
                        }

                        if (le.Estagio < inviab.Estagio) {

                            var nle = le.Clone();
                            nle.Estagio = inviab.Estagio;
                            if (inviab.TipoRestricao == "RHE") dadger.BlocoRhe.Add(nle);
                            else dadger.BlocoRhq.Add(nle);
                            le = nle;
                        }
                        var i = 2 * inviab.Patamar.Value + (inviab.SupInf == "INF" ? 1 : 2);

                        le[i] =
                            inviab.SupInf == "INF"
                            ? le[i] - Math.Ceiling(inviab.Violacao)
                            : le[i] + Math.Ceiling(inviab.Violacao);

                        if (le[i] < 0) le[i] = 0;
                    }
                } else if (inviab.TipoRestricao == "EVAP") {

                    var usina = hidr[inviab.Usina];
                    if (usina != null) {
                        dadger.BlocoUh.Where(x => x.Usina == usina.Cod).First().Evaporacao = false;

                    }

                } else if (inviab.TipoRestricao == "IRRI") {
                    var usina = hidr[inviab.Usina];
                    if (usina != null) {
                        //dadger.BlocoUh.Where(x => x.Usina == usina.Cod).First().Evaporacao = false;

                        var ti = dadger.BlocoTi.Where(x => x.Usina == usina.Cod).First();

                        ti[inviab.Estagio + 1] -= Math.Ceiling(inviab.Violacao);
                        if (ti[inviab.Estagio + 1] < 0) ti[inviab.Estagio + 1] = 0;


                    }
                }

            }
        }

        public static void AlterarCortes(Compass.CommomLibrary.Dadger.Dadger dadger, string cortesPath) {

            var refPath = dadger.File.Split('\\').ToList();
            var cortes = cortesPath.Split('\\').ToList();

            for (int i = 0; i < Math.Min(cortes.Count, refPath.Count); i++) {
                if (cortes[i] == refPath[i]) {
                    cortes.RemoveAt(i);
                    refPath.RemoveAt(i);
                    i--;
                }
            }

            var cortesRelPath = "";
            for (int i = 0; i < refPath.Count - 1; i++) {
                cortesRelPath += "../";
            }
            for (int i = 0; i < cortes.Count - 1; i++) {
                cortesRelPath += cortes[i] + "/";
            }

            var x1 = cortesRelPath + cortes.Last();
            var x2 = cortesRelPath + "cortesh" + System.IO.Path.GetExtension(x1);

            var fc = (Compass.CommomLibrary.Dadger.FcBlock)dadger.Blocos["FC"];

            fc.CortesInfo.Arquivo = x2;
            fc.Cortes.Arquivo = x1;

        }


        public static void CreateDgerNewdesp(string dir) {
            var files = Directory.GetFiles(dir).ToList();

            //decomp/newave/newdesp?


            var pmoFile = files.FirstOrDefault(x => Path.GetFileName(x).Equals("pmo.dat", StringComparison.OrdinalIgnoreCase));
            var dgerFile = files.FirstOrDefault(x => Path.GetFileName(x).Equals("dger.dat", StringComparison.OrdinalIgnoreCase));
            var adtermFile = files.FirstOrDefault(x => Path.GetFileName(x).Equals("adterm.dat", StringComparison.OrdinalIgnoreCase));
            var dgernwdFile = files.FirstOrDefault(x => Path.GetFileName(x).Equals("dger.nwd", StringComparison.OrdinalIgnoreCase));




            if (pmoFile != null && adtermFile != null) {
                var pmo = (Compass.CommomLibrary.Pmo.Pmo)DocumentFactory.Create(pmoFile);
                var dger = (Compass.CommomLibrary.DgerDat.DgerDat)DocumentFactory.Create(dgerFile);

                if (dgernwdFile != null) {

                    File.Copy(dgernwdFile, dgernwdFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                }


                var dgernwd = new Compass.CommomLibrary.DgerNwd.DgerNwd();
                dgernwd.File = Path.Combine(dir, "dger.nwd");

                dgernwd.Definicoes.Periodos = 1;
                dgernwd.Definicoes.MesInicial = dger.MesEstudo;
                dgernwd.Definicoes.AnoInicial = dger.AnoEstudo;
                dgernwd.Definicoes.TipoSimulacao = 1;


                int i = 0;
                foreach (var earmi in pmo.EarmI) {
                    dgernwd.EarmI[i++] = earmi.Earm;
                }

                var teaf = new List<Tuple<int, int, double>>();
                int reeOrd = 0;
                foreach (var eafR in pmo.EafPast) {

                    for (int mes = 1; mes <= 12; mes++) {
                        teaf.Add(new Tuple<int, int, double>(reeOrd, mes, eafR[mes]));
                    }
                    reeOrd++;
                }

                for (int mesOff = -1; mesOff >= -11; mesOff--) {

                    var lp = new Compass.CommomLibrary.DgerNwd.EafLine();
                    var mes = dgernwd.Definicoes.MesInicial + mesOff;
                    if (mes <= 0) mes += 12;

                    lp[0] = mes;

                    foreach (var e in teaf.Where(x => x.Item2 == mes).OrderBy(x => x.Item1)) {
                        lp[e.Item1 + 1] = e.Item3;
                    }

                    dgernwd.EnaPast.Add(lp);
                }
                {
                    var lp = new Compass.CommomLibrary.DgerNwd.EafLine();
                    var mes = dgernwd.Definicoes.MesInicial;

                    lp[0] = mes;

                    foreach (var e in teaf.Where(x => x.Item2 == mes).OrderBy(x => x.Item1)) {
                        lp[e.Item1 + 1] = e.Item3;
                    }

                    dgernwd.EnaPrev.Add(lp);
                }

                var adterm = File.ReadAllText(adtermFile).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                foreach (var tL in adterm.Skip(2)) {
                    if (tL.Trim() == "9999") break;

                    dgernwd.Gnl.Add(
                        dgernwd.Gnl.CreateLine(tL)
                        );



                }






                dgernwd.SaveToFile();

                //MessageBox.Show("dger.nwd Criado!");

            } else
                throw new Exception("PMO.DAT ou ADTERM.DAT não encontrados!");
        }
    }
}

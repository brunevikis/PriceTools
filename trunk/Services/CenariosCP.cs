using Compass.CommomLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.Services {
    public class CenariosCP {

        public class Cenario {
            public string Codigo { get; set; }
            public Compass.CommomLibrary.Dadger.Dadger Dadger { get; set; }
            public Compass.CommomLibrary.Prevs.Prevs Prevs { get; set; }
            public Byte[] Vazoes { get; set; }
        }

        public enum TipoDeSensibilidade {
            Carga,
            EarmI,
            Vazoes
        }

        public class CenariosConfig {

            public string DeckDeEntrada { get; set; }
            public Sensibilidade[] Sensibilidades { get; set; }


            public bool RodarVazoes { get; set; }

            public string CaminhoCortes { get; set; }

            public string DiretorioSaida { get; set; }
        }

        public class Sensibilidade {
            public TipoDeSensibilidade Tipo { get; set; }
            public object[,] Passo { get; set; }
            public int NumeroDePassos { get; set; }
        }

        public static List<Cenario> ProcessarCriarCenarios(CenariosConfig config) {


            var deck = new Compass.CommomLibrary.Decomp.Deck();
            deck.GetFiles(config.DeckDeEntrada);

            //variar dp

            //variar prevs
            //rodar datvaz
            var vazoesSvc = new Vazoes();
            var results = vazoesSvc.RunDatVazOnly(config.DeckDeEntrada, false);

            var prevcenDat = results.First(
                f => f.Name.Equals("PREVCEN.DAT") &&
                        System.IO.File.Exists(f.FullPath)
                ).FullPath;

            var prevCen = (Compass.CommomLibrary.PrevcenDat.PrevcenDat)DocumentFactory.Create(prevcenDat);
            var prevsBase = (Compass.CommomLibrary.Prevs.Prevs)DocumentFactory.Create(deck[CommomLibrary.Decomp.DeckDocument.prevs].BasePath);
            //var modif = (Compass.CommomLibrary.ModifDat.ModifDat)DocumentFactory.Create(deck[CommomLibrary.Decomp.DeckDocument.modif].BasePath);
            var dadger = (Compass.CommomLibrary.Dadger.Dadger)DocumentFactory.Create(deck[CommomLibrary.Decomp.DeckDocument.dadger].BasePath);
            var hidr = (Compass.CommomLibrary.HidrDat.HidrDat)DocumentFactory.Create(deck[CommomLibrary.Decomp.DeckDocument.hidr].BasePath);
            var configH = new Compass.CommomLibrary.Decomp.ConfigH(dadger, hidr);

            List<Compass.CommomLibrary.Dadger.DpBlock> cenCargas = null;
            List<Compass.CommomLibrary.Dadger.UhBlock> cenEarms = null;
            List<Compass.CommomLibrary.Prevs.Prevs> cenPrevs = null;


            if (!string.IsNullOrWhiteSpace(config.CaminhoCortes)) {

                foreach (var fc in dadger.Blocos["FC"]) {

                    if (!((string)fc[1]).Contains(config.CaminhoCortes)) {
                        fc[1] = ((string)fc[1]).ToLower().Replace("cortes", config.CaminhoCortes + "cortes");
                    }
                }
            }

            foreach (var sensibilidade in config.Sensibilidades) {


                switch (sensibilidade.Tipo) {
                    case TipoDeSensibilidade.Carga:
                        //[submercado,estagio]
                        cenCargas = CreateCenariosDeCarga(sensibilidade, dadger.BlocoDp);
                        break;
                    case TipoDeSensibilidade.EarmI:
                        //[submercado]
                        cenEarms = CreateCenariosDeEarm(sensibilidade, configH);
                        break;
                    case TipoDeSensibilidade.Vazoes:
                        cenPrevs = CreateCenariosDePrevs(sensibilidade, prevsBase, configH, prevCen);

                        break;
                }
            }


            var result = new List<Cenario>();
            // i = 0 :: dem modificação
            for (int iPrevs = 0; iPrevs <= cenPrevs.Count; iPrevs++) {

                Compass.CommomLibrary.Prevs.Prevs prev;



                if (iPrevs > 0) {
                    prev = cenPrevs[iPrevs - 1];
                } else {
                    prev = (Compass.CommomLibrary.Prevs.Prevs)prevsBase.Clone();
                }

                Byte[] vazoes = null;
                if (config.RodarVazoes) {
                    prev.SaveToFile(
                        System.IO.Path.Combine(vazoesSvc.WorkFolder, System.IO.Path.GetFileName(prev.File))
                        );
                    vazoesSvc.RunVaz();

                    var vr = vazoesSvc.GetResults();

                    var vazF = vr.FirstOrDefault(f => f.Name.StartsWith("vazoes.", StringComparison.OrdinalIgnoreCase));

                    if (vazF != null) {
                        vazoes = System.IO.File.ReadAllBytes(vazF.FullPath);
                    }
                }



                //prev.File = System.IO.Path.ChangeExtension(prev.File, "pr" + iPrevs.ToString());

                var prevFile = System.IO.Path.GetFileName(prev.File);





                for (int iCarga = 0; iCarga <= cenCargas.Count; iCarga++) {
                    for (int iEarm = 0; iEarm <= cenEarms.Count; iEarm++) {


                        var dger = (Compass.CommomLibrary.Dadger.Dadger)dadger.Clone();

                        if (iCarga > 0) {
                            dger.BlocoDp = cenCargas[iCarga - 1];
                        }

                        if (iEarm > 0) {
                            dger.BlocoUh = cenEarms[iEarm - 1];
                        }

                        //dger.File = System.IO.Path.ChangeExtension(dger.File, iCarga.ToString() + iEarm.ToString() + iPrevs.ToString());


                        //var i1 = dger.BottonComments.IndexOf("=>");
                        //var i2 = dger.BottonComments.IndexOf("\n", i1);

                        //dger.BottonComments = dger.BottonComments.Remove(i1, i2 - i1);
                        //dger.BottonComments = dger.BottonComments.Insert(i1, "=> " + prevFile);

                        result.Add(new Cenario {
                            Codigo = iCarga.ToString() + iEarm.ToString() + iPrevs.ToString(),
                            Dadger = dger,
                            Prevs = prev,
                            Vazoes = vazoes
                        });
                    }
                }
            }

            vazoesSvc.ClearTempFolder();

            var pastaDestino = config.DiretorioSaida;

            if (!Directory.Exists(pastaDestino)) Directory.CreateDirectory(pastaDestino);


            deck[CommomLibrary.Decomp.DeckDocument.dadger] =
            deck[CommomLibrary.Decomp.DeckDocument.vazoes] =
            deck[CommomLibrary.Decomp.DeckDocument.prevs] = null;

            foreach (var cen in result) {

                var subDir = Path.Combine(pastaDestino, cen.Codigo);

                if (!Directory.Exists(subDir)) Directory.CreateDirectory(subDir);

                deck.CopyFilesToFolder(subDir);

                var d = cen.Dadger;
                d.File = Path.Combine(subDir, Path.GetFileName(d.File));
                cen.Dadger.SaveToFile();

                var prev = cen.Prevs;
                prev.File = Path.Combine(subDir, Path.GetFileName(prev.File));
                prev.SaveToFile();


                var vpath = Path.ChangeExtension(
                            Path.Combine(subDir, "vazoes"),
                            Path.GetExtension(prev.File)
                            )
                            ;
                if (cen.Vazoes != null) {
                    File.WriteAllBytes(
                        vpath
                            , cen.Vazoes);
                }


                //File.WriteAllText(Path.Combine(subDir, "caso.dat"), cen.Codigo);
                //File.WriteAllText(Path.Combine(subDir, cen.Codigo),
                //Path.GetFileName(d.File) + "\n" + Path.GetFileName(vpath) + "\nhidr.dat\nmlt.dat\nloss.dat\n" + deck[CommomLibrary.Decomp.DeckDocument.dadgnl].FileName + "\n./"
                //);
            }

            File.WriteAllText(Path.Combine(pastaDestino, "cenarios.dat"),
                string.Join("\n", result.Select(c => c.Codigo)));

            return result;
        }

        private static List<CommomLibrary.Dadger.UhBlock> CreateCenariosDeEarm(Sensibilidade sensibilidade, CommomLibrary.Decomp.ConfigH configH) {
            var cenEarms = new List<Compass.CommomLibrary.Dadger.UhBlock>();

            var earmMax = configH.GetEarmsMax();
            configH.ReloadUH();
            var earmAtual = configH.GetEarms();

            for (int p = 1; p <= sensibilidade.NumeroDePassos; p++) {
                var earmTarget = new double[earmAtual.Length];
                //submercado                
                for (int s = 1; s <= sensibilidade.Passo.GetLength(0); s++) {

                    earmTarget[s - 1] = earmAtual[s - 1] / earmMax[s - 1];

                    earmTarget[s - 1] += (double)(p * (double)sensibilidade.Passo[s, 1]);
                }
                Reservatorio.SetUHBlock(configH, earmTarget, earmMax);
                var uh = ((Compass.CommomLibrary.Dadger.Dadger)configH.baseDoc).BlocoUh;
                cenEarms.Add(uh);
            }

            return cenEarms;
        }

        private static List<CommomLibrary.Dadger.DpBlock> CreateCenariosDeCarga(Sensibilidade sensibilidade, CommomLibrary.Dadger.DpBlock dpBase) {
            var cenCargas = new List<Compass.CommomLibrary.Dadger.DpBlock>();
            //passo
            for (int p = 1; p <= sensibilidade.NumeroDePassos; p++) {
                var dp = (Compass.CommomLibrary.Dadger.DpBlock)dpBase.Clone();

                //submercado
                for (int s = 1; s <= sensibilidade.Passo.GetLength(0); s++) {
                    //estgio
                    for (int e = 1; e <= sensibilidade.Passo.GetLength(1); e++) {

                        foreach (var item in dp.Where(x => x[1] == e && x[2] == s)) {
                            item[4] *= (1 + p * (double)sensibilidade.Passo[s, e]);
                            item[6] *= (1 + p * (double)sensibilidade.Passo[s, e]);
                            item[8] *= (1 + p * (double)sensibilidade.Passo[s, e]);
                        }
                    }
                }
                cenCargas.Add(dp);
            }
            return cenCargas;
        }

        private static List<Compass.CommomLibrary.Prevs.Prevs> CreateCenariosDePrevs(Sensibilidade sensibilidade, Compass.CommomLibrary.Prevs.Prevs prevsBase, CommomLibrary.Decomp.ConfigH configH, Compass.CommomLibrary.PrevcenDat.PrevcenDat prevcen) {
            var cenPrevs = new List<Compass.CommomLibrary.Prevs.Prevs>();

            for (int p = 1; p <= sensibilidade.NumeroDePassos; p++) {
                var prevs = (Compass.CommomLibrary.Prevs.Prevs)prevsBase.Clone();

                //posto
                for (int ip = 1; ip <= sensibilidade.Passo.GetLength(0); ip++) {
                    //estgio
                    for (int e = 1; e <= sensibilidade.Passo.GetLength(1); e++) {

                        prevs[ip, e] = (int)(prevs[ip, e] * (1 + p * (double)sensibilidade.Passo[ip, e]));
                    }
                }
                cenPrevs.Add(prevs);
            }

            return cenPrevs;
        }


        //rodar vazoes

        //rodar decomps
    }
}
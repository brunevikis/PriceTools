using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Pmo {
    public class Pmo : BaseDocument {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                
                    {"REE"                  , new ReeBlock()},   
                    {"EARMI"                  , new EarmiBlock()},   
                    {"EAF Tend Hidr"               , new EafBlock()},                 
                    {"EAF C Fuga Med"               , new EafBlock()},
                    {"GTERM Min", new GtermBlock() },
                    {"GTERM Max", new GtermBlock() },
                    {"DEFICIT", new DeficitBlock() },
                    {"C ADIC", new MecadoBlock() },
                    {"MERCADO", new MecadoBlock() },
                    {"PEQUENAS", new MecadoBlock() },
                    {"MERCADO LIQUIDO", new MecadoBlock() },


                    
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public ReeBlock Rees { get { return (ReeBlock)Blocos["REE"]; } }


        public Pmo(string filepath) {


            using (var fs = System.IO.File.OpenRead(filepath))
            using (var tr = new System.IO.StreamReader(fs)) {

                string line = tr.ReadLine();
                while (line != null) {

                    if (line.Contains("CUSTO DO DEFICIT POR PATAMARES")) {
                        ((DeficitBlock)Blocos["DEFICIT"]).Load(tr);
                    }

                    //DADOS DE CARGA ADICIONAL DE ENERGIA
                    if (line.ToUpperInvariant().Contains("DADOS DE CARGA ADICIONAL DE ENERGIA")) {
                        ((MecadoBlock)Blocos["C ADIC"]).Load(tr);
                    }

                    //DADOS DE MERCADO TOTAL DE ENERGIA
                    if (line.ToUpperInvariant().Contains("DADOS DE MERCADO TOTAL DE ENERGIA")) {
                        ((MecadoBlock)Blocos["MERCADO"]).Load(tr);
                    }

                    //DADOS DE GERACAO DE PEQUENAS USINAS
                    if (line.ToUpperInvariant().Contains("DADOS DE GERACAO DE PEQUENAS USINAS")) {
                        ((MecadoBlock)Blocos["PEQUENAS"]).Load(tr);
                    }

                    //DADOS DE MERCADO LIQUIDO DE ENERGIA
                    if (line.ToUpperInvariant().Contains("DADOS DE MERCADO LIQUIDO DE ENERGIA")) {
                        ((MecadoBlock)Blocos["MERCADO LIQUIDO"]).Load(tr);
                    }


                    if (line.ToUpperInvariant().Contains("ASSOCIACAO ENTRE REE")) {
                        ((ReeBlock)Blocos["REE"]).Load(tr);
                    }



                    if (line.Contains("GERACAO TERMICA MAXIMA POR USINA")) {
                        ((GtermBlock)Blocos["GTERM Max"]).Load(tr);
                    }

                    if (line.Contains("GERACAO TERMICA MINIMA POR USINA")) {
                        ((GtermBlock)Blocos["GTERM Min"]).Load(tr);
                    }

                    if (line.Contains("ENERGIAS AFLUENTES PASSADAS PARA A TENDENCIA HIDROLOGICA")) {
                        ((EafBlock)Blocos["EAF Tend Hidr"]).Load(tr);
                    }
                    if (line.Contains("ENERGIAS AFLUENTES PASSADAS EM REFERENCIA A PRIMEIRA CONFIGURACAO")) {
                        ((EafBlock)Blocos["EAF C Fuga Med"]).Load(tr);
                    }

                    if (line.Contains("ENERGIA ARMAZENADA INICIAL")) {
                        ((EarmiBlock)Blocos["EARMI"]).Load(tr);

                        break;
                    }
                    line = tr.ReadLine();
                }
            }
        }

        public EarmiBlock EarmI {
            get {
                return (EarmiBlock)blocos["EARMI"];
            }
        }

        public EafBlock EafPast {
            get {
                return (EafBlock)blocos["EAF Tend Hidr"];
            }
        }

        public override void Load(string fileContent) {
            ((EarmiBlock)Blocos["EARMI"]).Load(fileContent);
            ((EafBlock)Blocos["EAF Tend Hidr"]).Load(fileContent, "ENERGIAS AFLUENTES PASSADAS PARA A TENDENCIA HIDROLOGICA");
            ((EafBlock)Blocos["EAF C Fuga Med"]).Load(fileContent, "ENERGIAS AFLUENTES PASSADAS EM REFERENCIA A PRIMEIRA CONFIGURACAO");

            //((GtermBlock)Blocos["GTERM Max"]).Load(fileContent, "GERACAO TERMICA MAXIMA POR USINA");
            //((GtermBlock)Blocos["GTERM Min"]).Load(fileContent, "GERACAO TERMICA MINIMA POR USINA");

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.SistemaDat {
    public class SistemaDat : BaseDocument {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Patamares"             , new PatBlock()},
                    {"Deficit"               , new DefBlock()},
                    {"Intercambio"           , new IntBlock()},    
                    {"Mercado"               , new MerBlock()},
                    {"Pequenas"              , new PeqBlock()},                    
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            var currentBlock = "";
            var blockStarted = false;
            foreach (var line in lines) {
                switch (line.Trim()) {
                    case "PATAMAR DE DEFICIT":
                        currentBlock = "Patamares";
                        blockStarted = false;
                        continue;
                    case "CUSTO DO DEFICIT":
                        currentBlock = "Deficit";
                        blockStarted = false;
                        continue;
                    case "LIMITES DE INTERCAMBIO":
                        currentBlock = "Intercambio";
                        blockStarted = false;
                        continue;
                    case "MERCADO DE ENERGIA TOTAL":
                        currentBlock = "Mercado";
                        blockStarted = false;
                        continue;
                    case "GERACAO DE PEQUENAS USINAS":
                    case "GERACAO DE USINAS NAO SIMULADAS":
                        currentBlock = "Pequenas";
                        blockStarted = false;
                        continue;
                    default:
                        if (line.Trim().StartsWith("XXX")) {
                            blockStarted = true;
                            continue;
                        } else if (!blockStarted) {
                            continue;
                        }
                        break;
                }

                if (!Blocos.ContainsKey(currentBlock)) {
                    continue;
                }

                var newLine = Blocos[currentBlock].CreateLine(line);
                Blocos[currentBlock].Add(newLine);

            }
        }
    }
}

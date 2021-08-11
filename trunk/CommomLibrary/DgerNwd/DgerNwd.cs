using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DgerNwd {
    public class DgerNwd : BaseDocument {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Definicoes"             , new DefBlock()},
                    {"Armazenamento"               , new ArmBlock()},
                    {"EnaPassada"           , new EafBlock()},    
                    {"EnaPrevista"           , new EafPrev()},    
                    {"GNL"               , new GnlBlock()},
                    //{"Pequenas"              , new PeqBlock()},                    
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public DefBlock Definicoes {
            get {
                var blk = (DefBlock)Blocos["Definicoes"];
                if (blk.Count == 0) {
                    blk.Add(new DefLine());
                }
                return blk;
            }
        }
        public ArmBlock EarmI {
            get {
                var blk = (ArmBlock)Blocos["Armazenamento"];
                if (blk.Count == 0) {
                    blk.Add(new ArmLine());
                }
                return blk;
            }
        }

        public EafPrev EnaPrev {
            get {
                var blk = (EafPrev)Blocos["EnaPrevista"];

                return blk;
            }
        }
        public EafBlock EnaPast {
            get {
                var blk = (EafBlock)Blocos["EnaPassada"];
                return blk;
            }
        }

        public GnlBlock Gnl {
            get {
                var blk = (GnlBlock)Blocos["GNL"];
                return blk;
            }
        }

        public override void Load(string fileContent) {

            int fimBloco;

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);


            var newLine = Blocos["Definicoes"].CreateLine(lines.Skip(2).First());
            Blocos["Definicoes"].Add(newLine);

            fimBloco = 2 + 1;

            newLine = Blocos["Armazenamento"].CreateLine(lines.Skip(fimBloco + 2).First());
            Blocos["Armazenamento"].Add(newLine);

            fimBloco = fimBloco + 2 + 1;

            foreach (var enaLine in lines.Skip(fimBloco + 2).Take(11)) {
                newLine = Blocos["EnaPassada"].CreateLine(enaLine);
                Blocos["EnaPassada"].Add(newLine);
            }

            fimBloco = fimBloco + 2 + 11;

            foreach (var enaLine in lines.Skip(fimBloco + 2).Take(this.Definicoes.Periodos)) {
                newLine = Blocos["EnaPrevista"].CreateLine(enaLine);
                Blocos["EnaPrevista"].Add(newLine);
            }

            fimBloco = fimBloco + 2 + this.Definicoes.Periodos;

            int i = 0;
            while (lines[fimBloco + 2 + i].Trim() != "9999") {
                newLine = Blocos["GNL"].CreateLine(lines[fimBloco + 2 + i]);
                Blocos["GNL"].Add(newLine);

                i++;
            }

        }

        public override string ToText() {
            var t = base.ToText();
            t += "nada";
            return t;
        }
    }
}

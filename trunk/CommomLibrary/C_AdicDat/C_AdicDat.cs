using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.C_AdicDat {
    public class C_AdicDat : BaseDocument {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {                    
                    {"Carga"               , new MerBlock()},                                       
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }


        public MerBlock Adicao { get { return this.Blocos["Carga"] as MerBlock; } }


        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            var currentBlock = "Carga";
            foreach (var line in lines) {

                if (line.Trim() == "999") break;


                var newLine = Blocos[currentBlock].CreateLine(line);
                Blocos[currentBlock].Add(newLine);
            }
        }
    }
}

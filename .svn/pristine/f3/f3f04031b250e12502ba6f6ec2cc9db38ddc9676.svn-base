using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Vazpast {
    public class Vazpast : BaseDocument{
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Vaz"             , new VazBlock()},                 
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public VazBlock Conteudo { get { return (VazBlock)blocos["Vaz"]; } }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(3);

            foreach (var line in lines) {
                var newLine = Blocos["Vaz"].CreateLine(line);
                Blocos["Vaz"].Add(newLine);
            }
        }
    }
}

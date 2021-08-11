using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Eafpast {
    public class Eafpast : BaseDocument{
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Eaf"             , new EafBlock()},                 
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(2);

            foreach (var line in lines) {
                var newLine = Blocos["Eaf"].CreateLine(line);
                Blocos["Eaf"].Add(newLine);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dsvagua {
    public class Dsvagua : BaseBlockDocument<DsvLine> {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Dsv"             , new DsvBlock()},                 
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(2);

            foreach (var line in lines) {
                var newLine = this.CreateLine(line);
                if (newLine.Ano != 9999) this.Add(newLine);
                
            }
        }



        protected override BaseBlock<DsvLine> blockDocument {
            get { return (DsvBlock)this.blocos["Dsv"]; }
        }
    }
}

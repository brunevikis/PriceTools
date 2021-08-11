using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Previvaz {
    public class CasoDat : BaseDocument {

        public string Caso { get; set; }

        //Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
        //            {"Caso"             , new CasoBlock()}

        //        };

        //public override Dictionary<string, IBlock<BaseLine>> Blocos {
        //    get {
        //        return blocos;
        //    }
        //}

        public override void Load(string fileContent) {
            Caso = fileContent.Trim();
        }

        public override string ToText() {
            return Caso;
        }


        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get { throw new NotImplementedException(); }
        }
    }
}

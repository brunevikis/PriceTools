using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DgerNwd {
    public class DefBlock : BaseBlock<DefLine> {

        string header =
@"n periodos, mes inicial, ano inicial, =1(desp. hidrotermico), =2(valor da agua)
 xxxx xxxx xxxx xxxx
";

        public override string ToText() {

            return header + base.ToText();
        }

        public int Periodos {
            get {
                return this.First()["Periodos"];
            }
            set {
                this.First()["Periodos"] = value;
            }
        }

        public int MesInicial {
            get {
                return this.First()["MesInicial"];
            }
            set {
                this.First()["MesInicial"] = value;
            }
        }

        public int AnoInicial {
            get {
                return this.First()["AnoInicial"];
            }
            set {
                this.First()["AnoInicial"] = value;
            }
        }

        public int TipoSimulacao {
            get {
                return this.First()["TipoSimulacao"];
            }
            set {
                this.First()["TipoSimulacao"] = value;
            }
        }
    }

    public class DefLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 5   ,"I4", "Periodos"),
                new BaseField( 7 , 10  ,"I4" , "MesInicial"),
                new BaseField( 12 , 15 ,"I4",  "AnoInicial"),
                new BaseField( 17 , 20 ,"I4"  , "TipoSimulacao"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }


    }


}

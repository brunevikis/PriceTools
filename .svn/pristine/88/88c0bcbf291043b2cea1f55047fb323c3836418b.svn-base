using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class EsBlock : BaseBlock<EsLine> {

        public int NumSemanasPassadas {
            get {
                if (this.Count == 0) {
                    return 0;
                } else
                    return (int)this.Max(x => x[2]);
            }
        }

    }

    public class EsLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 6 ,"I2"  , "Sistema"),
                new BaseField( 10 , 10 ,"I1", "Semanas"  ),
                new BaseField( 15 , 24 ,"F10.0", "ENA s-1"  ),
                new BaseField( 25 , 34 ,"F10.0", "ENA s-2"  ),
                new BaseField( 35 , 44 ,"F10.0", "ENA s-3"  ),
                new BaseField( 45 , 54 ,"F10.0", "ENA s-4"  ),
                new BaseField( 55 , 64 ,"F10.0", "ENA s-5"  ),

            };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public double UltimaEna {
            get {
                return (double)Valores[3];
            }
            set {
                this[3] = value;
            }
        }
        public int Semanas {
            get { return Valores[2]; }
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class EaBlock : BaseBlock<EaLine> {

 

    }

    public class EaLine : BaseLine {

        public EaLine() {
            this[0] = "EA";
        }

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 6 ,"I2"  , "Sistema"),
                new BaseField( 10 , 19 ,"F10.0", "ENA m-1"  ),
                new BaseField( 20 , 29 ,"F10.0", "ENA m-2"  ),
                new BaseField( 30,  39 ,"F10.0", "ENA m-3"  ),
                new BaseField( 40,  49 ,"F10.0", "ENA m-4"  ),
                new BaseField( 50,  59 ,"F10.0", "ENA m-5"  ),
                new BaseField( 60,  69 ,"F10.0", "ENA m-6"  ),
                new BaseField( 70,  79 ,"F10.0", "ENA m-7"  ),
                new BaseField( 80,  89 ,"F10.0", "ENA m-8"  ),
                new BaseField( 90,  99 ,"F10.0", "ENA m-9"  ),
                new BaseField( 100, 109 ,"F10.0", "ENA m-10"  ),
                new BaseField( 110, 119 ,"F10.0", "ENA m-11"  ),
                               

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
    }

    
}

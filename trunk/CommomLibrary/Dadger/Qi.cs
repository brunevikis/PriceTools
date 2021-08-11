using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class QiBlock : BaseBlock<QiLine> {

    }

    public class QiLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 7 ,"I3", "Usina"  ),

                
                new BaseField( 10 , 14 ,"F5.0", "Qi s-1"  ),
                new BaseField( 15 , 19 ,"F5.0", "Qi s-2"  ),
                new BaseField( 20 , 24 ,"F5.0", "Qi s-3"  ),
                new BaseField( 25 , 29 ,"F5.0", "Qi s-4"  ),
                new BaseField( 30 , 34 ,"F5.0", "Qi s-5"  ),
            };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Usina { get { return (int)this[1]; } }
    }

    
}

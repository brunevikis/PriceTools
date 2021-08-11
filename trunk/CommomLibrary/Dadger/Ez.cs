using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class EzBlock : BaseBlock<EzLine> {

    }

    public class EzLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 7 ,"I3", "Usina"  ),
                new BaseField( 10 , 14 ,"F5.0", "Percentual"  ),

            };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    
}

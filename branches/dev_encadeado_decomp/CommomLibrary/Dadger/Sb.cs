using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class SbBlock : BaseBlock<SbLine> {

    }

    public class SbLine : BaseLine {
        public static readonly BaseField[] SbCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 6 ,"I2", "Subsistema"  ),
                new BaseField( 10 , 11 ,"A2", "Mnemonico"  ),

            };

        public override BaseField[] Campos {
            get { return SbCampos; }
        }
    }

    
}

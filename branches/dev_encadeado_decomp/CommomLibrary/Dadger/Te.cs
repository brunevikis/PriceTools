using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class TeBlock : BaseBlock<TeLine> {

    }

    public class TeLine : BaseLine {
        public static readonly BaseField[] StaCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 84 ,"A80", "Titulo"  ),

            };

        public override BaseField[] Campos {
            get { return StaCampos; }
        }
    }

    
}

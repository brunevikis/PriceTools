using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadgnl {
    public class NlBlock : BaseBlock<NlLine> {
        

    }

    public class NlLine : BaseLine {

        public NlLine()
            : base() {
                this[0] = "NL";
        }


        static readonly BaseField[] NlCampos = new BaseField[] {
                new BaseField(1 , 2 ,"A2"   ,"Id" ),
                new BaseField(5 , 7 ,"I3"   ,"Usina" ),
                new BaseField(10 ,11,"I2"   ,"Subsistema" ),
                new BaseField(15 ,15,"I1"   ,"Lag" ),
            };
        public override BaseField[] Campos {
            get { return NlCampos; }
        }
    }

    
}

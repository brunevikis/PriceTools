using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.SistemaDat {
    public class PatBlock : BaseBlock<PatLine> {

        string header =
@" PATAMAR DE DEFICIT
 NUMERO DE PATAMARES DE DEFICIT
 XXX
";

        public override string ToText() {
            
            return header + base.ToText();
        }

    }

    public class PatLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "Patamares"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

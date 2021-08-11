using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.PatamarDat {
    public class NumPatBlock : BaseBlock<NumPatLine> {

        string header =
@" NUMERO DE PATAMARES
 XX
";

        public override string ToText() {
            
            return header + base.ToText();
        }

    }

    public class NumPatLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 3 ,"I2"  , "Patamares"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

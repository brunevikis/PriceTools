using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.SistemaDat {
    public class DefBlock : BaseBlock<DefLine> {

        string header =
@" CUSTO DO DEFICIT
 NUM|NOME SSIS.|    CUSTO DE DEFICIT POR PATAMAR  | P.U. CORTE POR PATAMAR|
 XXX|XXXXXXXXXX| F|XXXX.XX XXXX.XX XXXX.XX XXXX.XX|X.XXX X.XXX X.XXX X.XXX|
";

        public override string ToText() {

            return header + base.ToText() + " 999\n";
        }
    }

    public class DefLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "Submercado"),
                new BaseField( 6 , 15 ,"A10"  , "Submercado"),
                new BaseField( 18 , 18 ,"I1"  , "F"),
                new BaseField( 20 , 26 ,"F7.2"  , "Custo 1"),
                new BaseField( 28 , 34 ,"F7.2"  , "Custo 2"),
                new BaseField( 36 , 42 ,"F7.2"  , "Custo 3"),
                new BaseField( 44 , 50 ,"F7.2"  , "Custo 4"),
                new BaseField( 52 , 56 ,"F5.3"  , "PU 1"),
                new BaseField( 58 , 62 ,"F5.3"  , "PU 2"),
                new BaseField( 64 , 68 ,"F5.3"  , "PU 3"),
                new BaseField( 70 , 74 ,"F5.3"  , "PU 4"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }


}

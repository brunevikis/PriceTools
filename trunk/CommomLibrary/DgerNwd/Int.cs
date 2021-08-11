using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.SistemaDat {
    public class IntBlock : BaseBlock<IntLine> {

        string header =
@" LIMITES DE INTERCAMBIO
 A   B   A->B    B->A
 XXX XXX XJAN. XXXFEV. XXXMAR. XXXABR. XXXMAI. XXXJUN. XXXJUL. XXXAGO. XXXSET. XXXOUT. XXXNOV. XXXDEZ.
";


        bool AB=true;
        public override IntLine CreateLine(string line = null) {
            line = (line ?? "");
            var id = line.Trim().Split(' ')[0];

            if (id == "") {
                AB = false;
                return BaseLine.Create<IntLine>(line);
            } else if (id.Length < 4) {
                AB = true;
                return BaseLine.Create<IntLine>(line);             
            } else {
                if (AB) {
                    return BaseLine.Create<IntABLine>(line);
                } else {
                    return BaseLine.Create<IntBALine>(line);
                }
            }
        }

        public override string ToText() {

            return header + base.ToText();
        }

    }

    //public abstract class IntLine : BaseLine { }

    public class IntLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "Submercado A"),
                new BaseField( 6 , 8 ,"I3"  , "Submercado B"),
                new BaseField( 24 , 24 ,"I1"  , "Flag"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class IntABLine : IntLine {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 7 ,"A8"  , "Ano"),
                new BaseField( 8 ,  14 ,"f7.0"  ,  "Limite Mes 1"),
                new BaseField( 16 , 22 ,"f7.0"  ,  "Limite Mes 2"),
                new BaseField( 24 , 30 ,"f7.0"  ,  "Limite Mes 3"),
                new BaseField( 32 , 38 ,"f7.0"  ,  "Limite Mes 4"),
                new BaseField( 40 , 46 ,"f7.0"  ,  "Limite Mes 5"),
                new BaseField( 48 , 54 ,"f7.0"  ,  "Limite Mes 6"),
                new BaseField( 56 , 62 ,"f7.0"  ,  "Limite Mes 7"),
                new BaseField( 64 , 70 ,"f7.0"  ,  "Limite Mes 8"),
                new BaseField( 72 , 78 ,"f7.0"  ,  "Limite Mes 9"),
                new BaseField( 80 , 86 ,"f7.0"  ,  "Limite Mes 10"),
                new BaseField( 88 , 94 ,"f7.0"  ,  "Limite Mes 11"),
                new BaseField( 96 , 102 ,"f7.0"  , "Limite Mes 12"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class IntBALine : IntLine {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 7 ,"A8"  , "Ano"),
                new BaseField( 8 ,  14 ,"f7.0"  ,  "Limite Mes 1"),
                new BaseField( 16 , 22 ,"f7.0"  ,  "Limite Mes 2"),
                new BaseField( 24 , 30 ,"f7.0"  ,  "Limite Mes 3"),
                new BaseField( 32 , 38 ,"f7.0"  ,  "Limite Mes 4"),
                new BaseField( 40 , 46 ,"f7.0"  ,  "Limite Mes 5"),
                new BaseField( 48 , 54 ,"f7.0"  ,  "Limite Mes 6"),
                new BaseField( 56 , 62 ,"f7.0"  ,  "Limite Mes 7"),
                new BaseField( 64 , 70 ,"f7.0"  ,  "Limite Mes 8"),
                new BaseField( 72 , 78 ,"f7.0"  ,  "Limite Mes 9"),
                new BaseField( 80 , 86 ,"f7.0"  ,  "Limite Mes 10"),
                new BaseField( 88 , 94 ,"f7.0"  ,  "Limite Mes 11"),
                new BaseField( 96 , 102 ,"f7.0"  , "Limite Mes 12"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}











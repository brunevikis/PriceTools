using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DgerNwd {
    public class GnlBlock : BaseBlock<GnlLine> {

        string header =
@" IUTE  NOME TERMICA LAG
 XXXX  XXXXXXXXXXXX  X  XXXXXXX.XX  XXXXXXX.XX  XXXXXXX.XX
";
        string footer =
@" 9999
";

        public override string ToText() {
            
            return header + base.ToText() + footer;
        }

        public override GnlLine CreateLine(string line = null) {
                        



            return base.CreateLine(line);



        }

    }

    public class GnlLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 5 ,"I4"  , "Num"),
                new BaseField( 8 , 19 ,"A12"  , "Nome"),
                new BaseField( 22, 22 ,"I1"  , "Lag"),
                new BaseField( 25 , 34 ,"F10.2"  , "GerPat 1"),
                new BaseField( 37 , 46 ,"F10.2"  , "GerPat 2"),
                new BaseField( 49 , 58 ,"F10.2"  , "GerPat 3"),
                new BaseField( 61 , 70 ,"F10.2"  , "GerPat 4"),
                new BaseField( 73 , 82 ,"F10.2"  , "GerPat 5"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public abstract class GnlLine1 : GnlLine {
        public new static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 5 ,"I4"  , "Num"),
                new BaseField( 8 , 19 ,"A12"  , "Nome"),
                new BaseField( 22, 22 ,"I1"  , "Lag"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public abstract class GnlLine2 : GnlLine {
        public new static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 25 , 34 ,"F10.2"  , "GerPat 1"),
                new BaseField( 37 , 46 ,"F10.2"  , "GerPat 2"),
                new BaseField( 49 , 58 ,"F10.2"  , "GerPat 3"),
                new BaseField( 61 , 70 ,"F10.2"  , "GerPat 4"),
                new BaseField( 73 , 82 ,"F10.2"  , "GerPat 5"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

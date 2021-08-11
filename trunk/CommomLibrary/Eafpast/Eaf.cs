using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Eafpast {
    public class EafBlock : BaseBlock<EafLine> {
        string header =
@" NUM SISTEMA        JAN        FEV        MAR        ABR        MAI        JUN        JUL        AGO        SET        OUT        NOV        DEZ
XXXX XXXXXXXXXX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX   XXXXX.XX
"
;

        public override string ToText() {

            return header + base.ToText();
        }
    }


    public class EafLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 4 ,"I4"  , "Subsitema"),
                new BaseField( 6 , 15 ,"A10"  , "Nome"),
                new BaseField(19  , 26 ,"F8.2"  , "Jan"),
                new BaseField(30  , 37 ,"F8.2"  , "Fev"),
                new BaseField(41  , 48 ,"F8.2"  , "Mar"),
                new BaseField(52  , 59 ,"F8.2"  , "Abr"),
                new BaseField(63  , 70 ,"F8.2"  , "Mai"),
                new BaseField(74  , 81 ,"F8.2"  , "Jun"),
                new BaseField(85  , 92 ,"F8.2"  , "Jul"),
                new BaseField(96  , 103,"F8.2"  , "Ago"),
                new BaseField(107 , 114,"F8.2"  , "Set"),
                new BaseField(118 , 125,"F8.2"  , "Out"),
                new BaseField(129 , 136,"F8.2"  , "Nov"),
                new BaseField(140 , 147,"F8.2"  , "Dez"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

}



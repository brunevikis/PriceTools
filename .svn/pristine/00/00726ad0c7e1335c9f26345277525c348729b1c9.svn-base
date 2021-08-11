using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dsvagua {
    public class DsvBlock : BaseBlock<DsvLine> {
        string header =
@"ANO  USIN    JAN    FEV    MAR    ABR    MAI    JUN    JUL    AGO    SET    OUT    NOV    DEZ
XXXX  XXX XXXX.X XXXX.X XXXX.X XXXX.X XXXX.X XXXX.X XXXX.X XXXX.X XXXX.X XXXX.X XXXX.X XXXX.X
"
;

        public override string ToText() {

            return header + base.ToText() + "9999\n";
        }
    }


    public class DsvLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 4 ,"I4"  , "Ano"),
                new BaseField( 6 , 9 ,"I4"  , "Usina"),                
                new BaseField(10  , 16 ,"F7.2"  , "Jan"),
                new BaseField(17  , 23 ,"F7.2"  , "Fev"),
                new BaseField(24  , 30 ,"F7.2"  , "Mar"),
                new BaseField(31  , 37 ,"F7.2"  , "Abr"),
                new BaseField(38  , 44 ,"F7.2"  , "Mai"),
                new BaseField(45  , 51 ,"F7.2"  , "Jun"),
                new BaseField(52  , 58 ,"F7.2"  , "Jul"),
                new BaseField(59  , 65,"F7.2"  , "Ago"),
                new BaseField(66  , 72,"F7.2"  , "Set"),
                new BaseField(73  , 79,"F7.2"  , "Out"),
                new BaseField(80  , 86,"F7.2"  , "Nov"),
                new BaseField(87  , 93,"F7.2"  , "Dez"),
                new BaseField(98  , 101,"I4"  , "Consi NC"),
                new BaseField(102 , 151,"A50"  , "Descricao"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Ano { get { return this[0]; } set { this[0] = value; } }
        public int Usina { get { return this[1]; } set { this[2] = value; } }
    }

}



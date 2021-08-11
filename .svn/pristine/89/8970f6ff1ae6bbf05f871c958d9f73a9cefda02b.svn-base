using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.PatamarDat {
    public class DuracaoBlock : BaseBlock<DuracaoLine> {

        string header =
@"ANO   DURACAO MENSAL DOS PATAMARES DE CARGA
      JAN     FEV     MAR     ABR     MAI     JUN     JUL     AGO     SET     OUT     NOV     DEZ   
      X.XXXX  X.XXXX  X.XXXX  X.XXXX  X.XXXX  X.XXXX  X.XXXX  X.XXXX  X.XXXX  X.XXXX  X.XXXX  X.XXXX
";

        public override string ToText() {

            return header + base.ToText();
        }

    }

    public class DuracaoLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {           
           new BaseField(0 , 0 ,"I1"  , "PATAMAR"),
           new BaseField(1 , 4 ,"I4"  , "ANO"),
           new BaseField(7,  12,"F6.4"  , "1"),
           new BaseField(15 ,20,"F6.4"  , "2"),
           new BaseField(23, 28,"F6.4"  , "3"),
           new BaseField(31, 36,"F6.4"  , "4"),
           new BaseField(39 ,44,"F6.4"  , "5"),
           new BaseField(47 ,52,"F6.4"  , "6"),
           new BaseField(55, 60,"F6.4"  , "7"),
           new BaseField(63, 68,"F6.4"  , "8"),
           new BaseField(71 ,76,"F6.4"  , "9"),
           new BaseField(79 ,84,"F6.4"  , "10"),
           new BaseField(87, 92,"F6.4"  , "11"),
           new BaseField(95, 100 ,"F6.4"  , "12"),
        };

        public int Ano { get { return this[1]; } set { this[1] = value; } }
        public int Patamar { get { return this[0]; } set { this[0] = value; } }

        

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

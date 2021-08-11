using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Vazpast {
    public class VazBlock : BaseBlock<VazLine> {
        string header =
@"  NUM       POSTO       JAN       FEV       MAR       ABR       MAI       JUN       JUL       AGO       SET       OUT       NOV       DEZ
-----
  XXX XXXXXXXXXXX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX XXXXXX.XX
"
;

        public override string ToText() {

            return header + base.ToText();
        }
    }


    public class VazLine : BaseLine {
        //public static readonly BaseField[] campos = new BaseField[] {
        //        new BaseField( 3 , 5   ,"I3"  , "Numero"),
        //        new BaseField( 7 ,  17  ,"A11"  , "Nome"),
        //        new BaseField( 19,  27,"F9.2"  , "Jan"),
        //        new BaseField( 29,  37,"F9.2"  , "Fev"),
        //        new BaseField( 39,  47,"F9.2"  , "Mar"),
        //        new BaseField( 49,  57,"F9.2"  , "Abr"),
        //        new BaseField( 59,  67,"F9.2"  , "Mai"),
        //        new BaseField( 69,  77,"F9.2"  , "Jun"),
        //        new BaseField( 79,  87,"F9.2"  , "Jul"),
        //        new BaseField( 89,  97,"F9.2"  , "Ago"),
        //        new BaseField( 99, 107,"F9.2"  , "Set"),
        //        new BaseField(109, 117 ,"F9.2"  , "Out"),
        //        new BaseField(119, 127 ,"F9.2"  , "Nov"),
        //        new BaseField(129, 137 ,"F9.2"  , "Dez"),
        //};

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(  3,   5,"I3"  , "Numero"),
                new BaseField(  7,  17,"A11"  , "Nome"),
                new BaseField( 20,  28,"F9.2"  , "Jan"),
                new BaseField( 30,  38,"F9.2"  , "Fev"),
                new BaseField( 40,  48,"F9.2"  , "Mar"),
                new BaseField( 50,  58,"F9.2"  , "Abr"),
                new BaseField( 60,  68,"F9.2"  , "Mai"),
                new BaseField( 70,  78,"F9.2"  , "Jun"),
                new BaseField( 80,  88,"F9.2"  , "Jul"),
                new BaseField( 90,  98,"F9.2"  , "Ago"),
                new BaseField(100, 108,"F9.2"  , "Set"),
                new BaseField(110, 118,"F9.2"  , "Out"),
                new BaseField(120, 128,"F9.2"  , "Nov"),
                new BaseField(130, 138,"F9.2"  , "Dez"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Posto { get { return this[campos[0]]; } }


        public double this[DateTime data] {
            get {
                return this[data.Month + 1];
            }
            set {
                this[data.Month + 1] = value;
            }



        }
    }

}



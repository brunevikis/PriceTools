using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.C_AdicDat {
    public class MerBlock : BaseBlock<MerLine> {

        string header =
@" XXX
       XXXJAN. XXXFEV. XXXMAR. XXXABR. XXXMAI. XXXJUN. XXXJUL. XXXAGO. XXXSET. XXXOUT. XXXNOV. XXXDEZ.
";
        int mercado = 0;
        string desc = "";
        public override MerLine CreateLine(string line = null) {
            line = line ?? "";

            var id = line.Trim().Split(' ')[0];
            int t;
            if (id.Length <= 3 && int.TryParse(id, out t)) {

                var ll = BaseLine.Create<MerLine>(line);

                //para gravacao do arquivo a partir do excel
                if (ll[0] != null) mercado = ll[0];
                desc = ll[1];

                return ll;
            } else {

                var ll = BaseLine.Create<MerEneLine>(line);
                ll.Mercado = mercado;
                ll.Descricao = desc;
                return ll;
            }
        }

        public override string ToText() {

            return header + base.ToText() + @" 999
";
        }

    }

    //public abstract class IntLine : BaseLine { }

    public class MerLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "Submercado"),
                new BaseField( 6 , 32 ,"A27"  , "Texto"),                

        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class MerEneLine : MerLine {
        public static readonly new BaseField[] campos = new BaseField[] {

                new BaseField( 1 , 4 ,"A"  , "Ano"),
                new BaseField( 8 ,  14 ,"f7.0"  ,  "1"),
                new BaseField( 16 , 22 ,"f7.0"  ,  "2"),
                new BaseField( 24 , 30 ,"f7.0"  ,  "3"),
                new BaseField( 32 , 38 ,"f7.0"  ,  "4"),
                new BaseField( 40 , 46 ,"f7.0"  ,  "5"),
                new BaseField( 48 , 54 ,"f7.0"  ,  "6"),
                new BaseField( 56 , 62 ,"f7.0"  ,  "7"),
                new BaseField( 64 , 70 ,"f7.0"  ,  "8"),
                new BaseField( 72 , 78 ,"f7.0"  ,  "9"),
                new BaseField( 80 , 86 ,"f7.0"  ,  "10"),
                new BaseField( 88 , 94 ,"f7.0"  ,  "11"),
                new BaseField( 96 , 102 ,"f7.0"  , "12"),    
                new BaseField( 0 , 0,"I"  , "Mecado"),
                new BaseField( 0 , 0,"A"  , "Descricao"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Mercado { get { return this["Mecado"]; } set { this["Mecado"] = value; } }
        public string Descricao { get { return this["Descricao"]; } set { this["Descricao"] = value; } }
        public string Ano { get { return this["Ano"]; } set { this["Ano"] = value; } }
    }
}











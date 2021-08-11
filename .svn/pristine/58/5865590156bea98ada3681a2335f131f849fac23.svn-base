using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Compass.CommomLibrary.SistemaDat
{

    public class PeqBlock : BaseBlock<PeqLine>
    {

        //        internal string header =
        //@" GERACAO DE USINAS NAO SIMULADAS
        // XXX
        //       XXXJAN. XXXFEV. XXXMAR. XXXABR. XXXMAI. XXXJUN. XXXJUL. XXXAGO. XXXSET. XXXOUT. XXXNOV. XXXDEZ.
        //";


        internal string header =
                @" GERACAO DE USINAS NAO SIMULADAS
 XXX  XBL  XXXXXXXXXXXXXXXXXXXX  XTE
       XXXJAN. XXXFEV. XXXMAR. XXXABR. XXXMAI. XXXJUN. XXXJUL. XXXAGO. XXXSET. XXXOUT. XXXNOV. XXXDEZ.
";



        public override PeqLine CreateLine(string line = null)
        {
            line = line ?? "";

            var id = line.Trim().Split(' ')[0];
            int t;
            if (id.Length <= 3 && int.TryParse(id, out t))
            {
                return BaseLine.Create<PeqLine>(line);
            }
            else if (id == "POS")
            {
                var x = BaseLine.Create<PeqEnePosLine>(line);
                x["Submercado"] = this.Last()["Submercado"];
                x["Tipo_Usina"] = this.Last()["Tipo_Usina"];
                x["Desc_Usina"] = this.Last()["Desc_Usina"];
                return x;
            }
            else
            {
                var x = BaseLine.Create<PeqEneLine>(line);
                x["Submercado"] = this.Last()["Submercado"];
                x["Tipo_Usina"] = this.Last()["Tipo_Usina"];
                x["Desc_Usina"] = this.Last()["Desc_Usina"];
                return x;
            }
        }

        public override string ToText()
        {

            return header + base.ToText() + " 999\n";
        }

       

    }


    public class PeqLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "Submercado"),
                new BaseField( 7 , 9 ,"I3"  , "Tipo_Usina"),
                new BaseField( 12 , 31 ,"A20"  , "Desc_Usina"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int? Ano
        {
            get
            {
                if (this is PeqEneLine) return this[0];
                else return null;
            }
            set
            {
                this[0] = value;
            }
        }

        public int Mercado
        {
            get
            {
                return this["Submercado"];
            }
        }

        public int Tipo_Usina
        {
            get
            {
                return this["Tipo_Usina"];
            }
        }

        public string Desc_Usina
        {
            get
            {
                return this["Desc_Usina"];
            }
        }


    }


    public class PeqEneLine : PeqLine
    {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 4 ,"I4"  , "Ano"),
                new BaseField( 8 ,  14 ,"f7.0"  ,  "Mercado Mes 1"),
                new BaseField( 16 , 22 ,"f7.0"  ,  "Mercado Mes 2"),
                new BaseField( 24 , 30 ,"f7.0"  ,  "Mercado Mes 3"),
                new BaseField( 32 , 38 ,"f7.0"  ,  "Mercado Mes 4"),
                new BaseField( 40 , 46 ,"f7.0"  ,  "Mercado Mes 5"),
                new BaseField( 48 , 54 ,"f7.0"  ,  "Mercado Mes 6"),
                new BaseField( 56 , 62 ,"f7.0"  ,  "Mercado Mes 7"),
                new BaseField( 64 , 70 ,"f7.0"  ,  "Mercado Mes 8"),
                new BaseField( 72 , 78 ,"f7.0"  ,  "Mercado Mes 9"),
                new BaseField( 80 , 86 ,"f7.0"  ,  "Mercado Mes 10"),
                new BaseField( 88 , 94 ,"f7.0"  ,  "Mercado Mes 11"),
                new BaseField( 96 , 102 ,"f7.0"  , "Mercado Mes 12"),
                new BaseField( 0 , 0 ,"I3"  , "Submercado"),
                new BaseField( 0 , 0 ,"I3"  , "Tipo_Usina"),
                new BaseField( 0 , 0 ,"A20"  , "Desc_Usina"),

        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }
        public double Anos { get { return Valores[0]; } set { Valores[0] = value; } }
        public double Jan { get { return Valores[1]; } set { Valores[1] = value; } }
        public double Fev { get { return Valores[2]; } set { Valores[2] = value; } }
        public double Mar { get { return Valores[3]; } set { Valores[3] = value; } }
        public double Abr { get { return Valores[4]; } set { Valores[4] = value; } }
        public double Mai { get { return Valores[5]; } set { Valores[5] = value; } }
        public double Jun { get { return Valores[6]; } set { Valores[6] = value; } }
        public double Jul { get { return Valores[7]; } set { Valores[7] = value; } }
        public double Ago { get { return Valores[8]; } set { Valores[8] = value; } }
        public double Set { get { return Valores[9]; } set { Valores[9] = value; } }
        public double Out { get { return Valores[10]; } set { Valores[10] = value; } }
        public double Nov { get { return Valores[11]; } set { Valores[11] = value; } }
        public double Dez { get { return Valores[12]; } set { Valores[12] = value; } }

    }

    public class PeqEnePosLine : PeqLine
    {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 7 ,"A7"  , "Ano"),
                new BaseField( 8 ,  14 ,"f7.0"  ,  "Mercado Mes 1"),
                new BaseField( 16 , 22 ,"f7.0"  ,  "Mercado Mes 2"),
                new BaseField( 24 , 30 ,"f7.0"  ,  "Mercado Mes 3"),
                new BaseField( 32 , 38 ,"f7.0"  ,  "Mercado Mes 4"),
                new BaseField( 40 , 46 ,"f7.0"  ,  "Mercado Mes 5"),
                new BaseField( 48 , 54 ,"f7.0"  ,  "Mercado Mes 6"),
                new BaseField( 56 , 62 ,"f7.0"  ,  "Mercado Mes 7"),
                new BaseField( 64 , 70 ,"f7.0"  ,  "Mercado Mes 8"),
                new BaseField( 72 , 78 ,"f7.0"  ,  "Mercado Mes 9"),
                new BaseField( 80 , 86 ,"f7.0"  ,  "Mercado Mes 10"),
                new BaseField( 88 , 94 ,"f7.0"  ,  "Mercado Mes 11"),
                new BaseField( 96 , 102 ,"f7.0"  , "Mercado Mes 12"),

                new BaseField( 0 , 0 ,"I3"  , "Submercado"),
                new BaseField( 0 , 0 ,"I3"  , "Tipo_Usina"),
                new BaseField( 0 , 0 ,"A20"  , "Desc_Usina"),

        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }
    }
}

    /*
    public class PeqBlock : BaseBlock<PeqLine> {

        string header =
@" GERACAO DE USINAS NAO SIMULADAS
 XXX
       XXXJAN. XXXFEV. XXXMAR. XXXABR. XXXMAI. XXXJUN. XXXJUL. XXXAGO. XXXSET. XXXOUT. XXXNOV. XXXDEZ.
";

        public override PeqLine CreateLine(string line = null) {
            line = line ?? "";

            var id = line.Trim().Split(' ')[0];
            int t;
            if (id.Length <= 3 && int.TryParse(id, out t)) {
                return BaseLine.Create<PeqLine>(line);
            } else {
                return BaseLine.Create<PeqGerLine>(line);
            }
        }

        public override string ToText() {

            return header + base.ToText();
        }

    }

    //public abstract class IntLine : BaseLine { }

    public class PeqLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "Submercado"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
        public int? Ano {
            get {
                if (this is PeqGerLine) return this[0];
                else return null;
            }
            set {
                this[0] = value;
            }
        }

        public int Mercado {
            get {
                return this["Submercado"];
            }
        }

    }

    public class PeqGerLine : PeqLine {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 7 ,"I8"  , "Ano"),
                new BaseField( 8 ,  14 ,"f7.0"  ,  "Pequenas Mes 1"),
                new BaseField( 16 , 22 ,"f7.0"  ,  "Pequenas Mes 2"),
                new BaseField( 24 , 30 ,"f7.0"  ,  "Pequenas Mes 3"),
                new BaseField( 32 , 38 ,"f7.0"  ,  "Pequenas Mes 4"),
                new BaseField( 40 , 46 ,"f7.0"  ,  "Pequenas Mes 5"),
                new BaseField( 48 , 54 ,"f7.0"  ,  "Pequenas Mes 6"),
                new BaseField( 56 , 62 ,"f7.0"  ,  "Pequenas Mes 7"),
                new BaseField( 64 , 70 ,"f7.0"  ,  "Pequenas Mes 8"),
                new BaseField( 72 , 78 ,"f7.0"  ,  "Pequenas Mes 9"),
                new BaseField( 80 , 86 ,"f7.0"  ,  "Pequenas Mes 10"),
                new BaseField( 88 , 94 ,"f7.0"  ,  "Pequenas Mes 11"),
                new BaseField( 96 , 102 ,"f7.0"  , "Pequenas Mes 12"),

                new BaseField( 0 , ,0 ,"I3"  , "Submercado"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}










*/
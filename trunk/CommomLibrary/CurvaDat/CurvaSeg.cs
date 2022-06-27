using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.CurvaDat
{
    public class CurvaSegBlock : BaseBlock<CurvaSegLine>
    {

        //        string header =
        // @"INIT                                             
        //&us     nome       ug   st   GerInic     tempo MH A/D
        //&XX XXXXXXXXXXXX  XXX   XX   XXXXXXXXXX  XXXXX  X  X 
        //";

        //        public override string ToText()
        //        {

        //            return header + base.ToText() + "FIM\n";
        //        }

        public override CurvaSegLine CreateLine(string line = null)
        {
            line = line ?? "";

            var id = line.Trim().Split(' ')[0];
            int t;
            if (id.Length <= 3 && int.TryParse(id, out t))
            {
                return BaseLine.Create<CurvaSegLine>(line);
            }
            else
            {
                var x = BaseLine.Create<CurvaSegPorLine>(line);
                x["REE"] = this.Last()["REE"];
                return x;
            }
        }

        //public override string ToText()
        //{

        //    return header + base.ToText() + " 999\n";
        //}

    }

    public class CurvaSegLine : BaseLine
    {

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "REE"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int? Ano
        {
            get
            {
                if (this is CurvaSegPorLine) return this[0];
                else return null;
            }
            set
            {
                this[0] = value;
            }
        }

        public int Ree
        {
            get
            {
                return this["REE"];
            }
        }



        //public InitLine() : base() { this[0] = "GL"; }

        //public int Usina { get { return (int)this[0]; } set { this[0] = value; } }

        //public string NomeUsina { get { return this[1].ToString(); } set { this[1] = value; } }
        //public int Indice { get { return (int)this[2]; } set { this[2] = value; } }
        //public int Status { get { return (int)this[3]; } set { this[3] = value; } }

        //public float Geracao { get { return (float)this[4]; } set { this[4] = value; } }
        //public int Tempo { get { return (int)this[5]; } set { this[5] = value; } }
        //public int MeiaHora { get { return (int)this[6]; } set { this[6] = value; } }
        //public int AD { get { return (int)this[7]; } set { this[7] = value; } }




        //public override BaseField[] Campos { get { return InitCampos; } }

        //static readonly BaseField[] InitCampos = new BaseField[] {
        //        new BaseField(1  , 3 ,"I3"    , "Usina"),
        //        new BaseField(5  , 17 ,"A12"    , "Nome Usina"),
        //        new BaseField(19 , 21,"I3"    , "Indice"),
        //        new BaseField(25 , 26,"I2"    , "Status"),
        //        new BaseField(30 , 39,"F10.0" , "Geracao"),
        //        new BaseField(42 , 46,"I5"  , "Tempo"),
        //        new BaseField(49 , 49,"I1" , "MH"),
        //        new BaseField(52 , 52,"I1"  , "A/D"),

        //    };
    }

    public class CurvaSegPorLine : CurvaSegLine
    {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 4 ,"I4"  , "Ano"),
                new BaseField( 7 ,  11 ,"F5.0"  ,  "Porc Mes 1"),
                new BaseField( 13 , 17 ,"F5.0"  ,  "Porc Mes 2"),
                new BaseField( 19 , 23 ,"F5.0"  ,  "Porc Mes 3"),
                new BaseField( 25 , 29 ,"F5.0"  ,  "Porc Mes 4"),
                new BaseField( 31 , 35 ,"F5.0"  ,  "Porc Mes 5"),
                new BaseField( 37 , 41 ,"F5.0"  ,  "Porc Mes 6"),
                new BaseField( 43 , 47 ,"F5.0"  ,  "Porc Mes 7"),
                new BaseField( 49 , 53 ,"F5.0"  ,  "Porc Mes 8"),
                new BaseField( 55 , 59 ,"F5.0"  ,  "Porc Mes 9"),
                new BaseField( 61 , 65 ,"F5.0"  ,  "Porc Mes 10"),
                new BaseField( 67 , 71 ,"F5.0"  ,  "Porc Mes 11"),
                new BaseField( 73 , 77 ,"F5.0"  , "Porc Mes 12"),

                new BaseField( 0 , 0 ,"I3"  , "REE"),

        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public double limInf { get { return this[3]; } set { this[3] = value; } }

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
}

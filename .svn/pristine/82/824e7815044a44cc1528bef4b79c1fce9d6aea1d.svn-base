using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ConfigDat
{
    public class DiaBlock : BaseBlock<DiaLine>
    {


        //        string header =
        // @"OPER                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
        //&us    nome      un di hi m df hf m Gmin     Gmax       Custo                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
        //&XX XXXXXXXXXXXX XX XX XX X XX XX X XXXXXXXXXXxxxxxxxxxxXXXXXXXXXX
        //";

        //public override string ToText()
        //{

        //    return base.ToText() + "FIM\n";
        //}



    }

    public class DiaLine : BaseLine
    {

        //public OperLine() : base() { this[0] = "GL"; }

        public int TipoDia { get { return (int)this[1]; } set { this[1] = value; } }

        public string Minemonico { get { return this[0].ToString(); } set { this[0] = value; } }
        public override BaseField[] Campos { get { return DiaCampos; } }

        static readonly BaseField[] DiaCampos = new BaseField[] {
               new BaseField(1  , 3 ,"A3"    , "Minemonico dia"),
               new BaseField(5  , 5 ,"I1"    , "tipo dia"),
            };
    }
}

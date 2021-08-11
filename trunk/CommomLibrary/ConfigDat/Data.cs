using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ConfigDat
{
    public class DataBlock : BaseBlock<DataLine>
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

    public class DataLine : BaseLine
    {

        //public OperLine() : base() { this[0] = "GL"; }

        public int Dia { get { return (int)this[1]; } set { this[1] = value; } }
        public int Mes { get { return (int)this[2]; } set { this[2] = value; } }
        public int Ano { get { return (int)this[3]; } set { this[3] = value; } }
        public int Duracao { get { return (int)this[4]; } set { this[4] = value; } }

        public string Minemonico { get { return this[0].ToString(); } set { this[0] = value; } }
        public override BaseField[] Campos { get { return DataCampos; } }

        static readonly BaseField[] DataCampos = new BaseField[] {
               new BaseField(1  , 3 ,"A3"    , "Minemonico Registro"),
               new BaseField(5  , 6 ,"I2"    , "dia"),
               new BaseField(9  , 10 ,"I2"    , "mes"),
               new BaseField(14  , 17 ,"I4"    , "ano"),
               new BaseField(19  , 19 ,"I1"    , "duracao"),
            };
    }
}

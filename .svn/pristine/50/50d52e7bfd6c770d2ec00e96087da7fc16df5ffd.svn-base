using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Operut
{
    public class OperBlock : BaseBlock<OperLine>
    {


        //        string header =
        // @"OPER                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
        //&us    nome      un di hi m df hf m Gmin     Gmax       Custo                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
        //&XX XXXXXXXXXXXX XX XX XX X XX XX X XXXXXXXXXXxxxxxxxxxxXXXXXXXXXX
        //";

        public override string ToText()
        {

            return base.ToText() + "FIM\n";
        }



    }

    public class OperLine : BaseLine
    {

        //public OperLine() : base() { this[0] = "GL"; }

        public int Usina { get { return (int)this[0]; } set { this[0] = value; } }

        public string NomeUsina { get { return this[1].ToString(); } set { this[1] = value; } }
        public int Indice { get { return (int)this[2]; } set { this[2] = value; } }
        public string DiaInicial { get { return this[3].ToString(); } set { this[3] = value; } }

        public int HoraInicial { get { return (int)this[4]; } set { this[4] = value; } }
        public int MeiaHoraInicial { get { return (int)this[5]; } set { this[5] = value; } }
        public string DiaFinal { get { return this[6].ToString(); } set { this[6] = value; } }
        public int HoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int MeiaHoraFinal { get { return (int)this[8]; } set { this[8] = value; } }
        public float LimiteInf { get { return (float)this[9]; } set { this[9] = value; } }
        public float LimiteSup { get { return (float)this[10]; } set { this[10] = value; } }
        public float CustoGeracao { get { return (float)this[11]; } set { this[11] = value; } }
        public override BaseField[] Campos { get { return OperCampos; } }

        static readonly BaseField[] OperCampos = new BaseField[] {
               new BaseField(1  , 3 ,"I3"    , "Usina"),
                new BaseField(5  , 16 ,"A12"    , "Nome Usina"),
                new BaseField(17 , 19,"I2"    , "Indice"),
                new BaseField(21 , 22,"A2"    , "DiaInicial"),
                new BaseField(24 , 25,"I2" , "HoraInicial"),
                new BaseField(27 , 27,"I1"  , "MeiaHoraInicial"),
                new BaseField(29 , 30,"A2" , "DiaFinal"),
                new BaseField(32 , 33,"I2"  , "HoraFinal"),
                new BaseField(35 , 35,"I1"  , "MeiaHoraFinal"),
                new BaseField(37 , 46,"F10.0"  , "Limiteinf"),
                new BaseField(47 , 56,"F10.0"  , "limitesup"),
                new BaseField(57 , 66,"F10.0"  , "CustoGeracao"),
            };
    }
}

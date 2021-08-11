using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.EntdadosDat
{
    public class R11Block : BaseBlock<R11Line>
    {




    }

    public class R11Line : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public string DiaInic { get { return this[1].ToString(); } set { this[1] = value; } }
        public int HoraInic { get { return (int)this[2]; } set { this[2] = value; } }
        public int MeiaHoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public string DiaFinal { get { return this[4].ToString(); } set { this[4] = value; } }
        public int HoraFinal { get { return (int)this[5]; } set { this[5] = value; } }
        public int MeiaHoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public float Nivel { get { return (float)this[7]; } set { this[7] = value; } }
        public float RestMaxHor { get { return (float)this[8]; } set { this[8] = value; } }
        public float RestMaxDia { get { return (float)this[9]; } set { this[9] = value; } }

        public override BaseField[] Campos { get { return R11Campos; } }

        static readonly BaseField[] R11Campos = new BaseField[] {
                new BaseField(1  , 3 ,"A3"    , "IdBloco"),//
                new BaseField(5  , 6 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(8  , 9 ,"I2"    , "HoraDiaInic"),//
                new BaseField(11  , 11 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(13  , 14 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(16  , 17 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(19  , 19 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(21  , 30 ,"F10.2"    , "Nivel"),//
                new BaseField(31  , 40 ,"F10.2"    , "RestmaxHor"),//
                new BaseField(41  , 50 ,"F10.2"    , "RestmaxDia"),//
                new BaseField(60  , 74 ,"E15.0"    , "Coef1"),//
                new BaseField(75  , 89 ,"E15.0"    , "Coef2"),//
                new BaseField(90  , 104 ,"E15.0"    , "Coef3"),//
                new BaseField(105  , 119 ,"E15.0"    , "Coef4"),//
                new BaseField(120  , 134 ,"E15.0"    , "Coef5"),//
                new BaseField(135  , 149 ,"E15.0"    , "Coef6"),//
                new BaseField(150  , 164 ,"E15.0"    , "Coef7"),//


            };
    }
}

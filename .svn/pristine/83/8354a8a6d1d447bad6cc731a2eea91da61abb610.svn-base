using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class RiBlock : BaseBlock<RiLine>
    {




    }

    public class RiLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public string DiaInic { get { return this[1].ToString(); } set { this[1] = value; } }
        public int HoraInic { get { return (int)this[2]; } set { this[2] = value; } }
        public int MeiaHoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public string DiaFinal { get { return this[4].ToString(); } set { this[4] = value; } }
        public int HoraFinal { get { return (int)this[5]; } set { this[5] = value; } }
        public int MeiaHoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public float LinInf50 { get { return (float)this[7]; } set { this[7] = value; } }
        public float LinSup50 { get { return (float)this[8]; } set { this[8] = value; } }
        public float LinInf60 { get { return (float)this[9]; } set { this[9] = value; } }
        public float LinSup60 { get { return (float)this[10]; } set { this[10] = value; } }
        public float CargaAnde { get { return (float)this[11]; } set { this[11] = value; } }

        public override BaseField[] Campos { get { return VmCampos; } }

        static readonly BaseField[] VmCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(9  , 10 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(12  , 13 ,"I2"    , "HoraDiaInic"),//
                new BaseField(15  , 15 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(17  , 18 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(20  , 21 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(23  , 23 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(27  , 36 ,"F10.0"    , "LimInf50hz"),//
                new BaseField(37  , 46 ,"F10.0"    , "LimSup50hz"),//
                new BaseField(47  , 56 ,"F10.0"    , "LimInf60hz"),//
                new BaseField(57  , 66 ,"F10.0"    , "LimSup60hz"),//
                new BaseField(67  , 76 ,"F10.0"    , "CargaAnde"),//


            };
    }
}

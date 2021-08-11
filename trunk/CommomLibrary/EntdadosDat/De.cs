using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class DeBlock : BaseBlock<DeLine>
    {




    }

    public class DeLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int NumDemanda { get { return (int)this[1]; } set { this[1] = value; } }
        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public float Demanda { get { return (float)this[8]; } set { this[8] = value; } }
        public string Descricao { get { return this[9].ToString(); } set { this[9] = value; } }

        public override BaseField[] Campos { get { return DeCampos; } }

        static readonly BaseField[] DeCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 7 ,"I3"    , "NumDemanda"),//
                new BaseField(9  , 10 ,"A2"    , "DiaInic"),//pode ser letra
                new BaseField(12  , 13 ,"I2"    , "HoraDiaInic"),//
                new BaseField(15  , 15 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(17  , 18 ,"A2"    , "DiaFinal"),//pode ser letra
                new BaseField(20  , 21 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(23  , 23 ,"I1"    , "meiaHoraDiaFinal"),//
                new BaseField(25  , 34 ,"F10.0"    , "DemandaMW"),
                new BaseField(36  , 45 ,"A10"    , "Descricao"),


            };
    }
}

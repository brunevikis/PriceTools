using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class CdBlock : BaseBlock<CdLine>
    {




    }

    public class CdLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Subsist { get { return (int)this[1]; } set { this[1] = value; } }
        public int Segmento { get { return (int)this[2]; } set { this[2] = value; } }
        public string DiaInic { get { return this[3].ToString(); } set { this[3] = value; } }
        public int HoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public int MeiaHoraInic { get { return (int)this[5]; } set { this[5] = value; } }
        public string DiaFinal { get { return this[6].ToString(); } set { this[6] = value; } }
        public int HoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int MeiaHoraFinal { get { return (int)this[8]; } set { this[8] = value; } }
        public float CustoDef { get { return (float)this[9]; } set { this[9] = value; } }
        public float Profdef { get { return (float)this[10]; } set { this[10] = value; } }

        public override BaseField[] Campos { get { return CdCampos; } }

        static readonly BaseField[] CdCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(4  , 5 ,"I2"    , "Subsist"),//
                new BaseField(7  , 8 ,"I2"    , "Segmento"),//
                new BaseField(10  , 11 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(13  , 14 ,"I2"    , "HoraDiaInic"),//
                new BaseField(16  , 16 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(18  , 19 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(21  , 22 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(24  , 24 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(26  , 35 ,"F10.0"    , "CustoDef"),//
                new BaseField(36  , 45 ,"F10.0"    , "ProfDef"),//


            };
    }
}

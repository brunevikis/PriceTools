using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.RmpflxDat
{
    public class LimiBlock : BaseBlock<LimiLine>
    {

    }

    public class LimiLine : BaseLine
    {

        //public InitLine() : base() { this[0] = "GL"; }


        public string Id { get { return this[0].ToString(); } set { this[0] = value; } }
        public string Minemonico { get { return this[1].ToString(); } set { this[1] = value; } }

        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int DREnum { get { return (int)this[8]; } set { this[8] = value; } }
        public float RampaInf { get { return (float)this[9]; } set { this[9] = value; } }
        public float RampaSup { get { return (float)this[10]; } set { this[10] = value; } }
        public int Tipo { get { return (int)this[11]; } set { this[11] = value; } }

        public override BaseField[] Campos { get { return LimiCampos; } }

        static readonly BaseField[] LimiCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A6"    , "Id"),
                new BaseField(8  , 11 ,"A$"    , "Minemonico"),
                new BaseField(13  , 14 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(16  , 17 ,"I2"    , "HoraDiaInic"),//
                new BaseField(19  , 19 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(21  , 22 ,"A2"    , "DiaFinal"),//podes ser letra
                new BaseField(24  , 25 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(27  , 27 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(29  , 32 ,"I4"    , "DREnum"),//
                new BaseField(34  , 43 ,"F10.0"    , "RampaInf"),//
                new BaseField(45  , 54 ,"F10.0"    , "RampaSup"),//
                new BaseField(56  , 56 ,"I1"    , "Tipo"),//

            };
    }

}

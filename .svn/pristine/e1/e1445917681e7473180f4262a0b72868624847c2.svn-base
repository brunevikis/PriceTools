using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class MhBlock : BaseBlock<MhLine>
    {




    }

    public class MhLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public int IndiceGrupo { get { return (int)this[2]; } set { this[2] = value; } }
        public int IndiceUnidade { get { return (int)this[3]; } set { this[3] = value; } }
        public string DiaInic { get { return this[4].ToString(); } set { this[4] = value; } }
        public int HoraInic { get { return (int)this[5]; } set { this[5] = value; } }
        public int MeiaHoraInic { get { return (int)this[6]; } set { this[6] = value; } }
        public string DiaFinal { get { return this[7].ToString(); } set { this[7] = value; } }
        public int HoraFinal { get { return (int)this[8]; } set { this[8] = value; } }
        public int MeiaHoraFinal { get { return (int)this[9]; } set { this[9] = value; } }
        public int DispUsina { get { return (int)this[10]; } set { this[10] = value; } }

        public override BaseField[] Campos { get { return MhCampos; } }

        static readonly BaseField[] MhCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 7 ,"I3"    , "Usina"),//
                new BaseField(10  , 11 ,"I2"    , "IndiceGrupo"),//
                new BaseField(13  , 14 ,"I2"    , "IndiceUnidade"),//
                new BaseField(15  , 16 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(18  , 19 ,"I2"    , "HoraDiaInic"),//
                new BaseField(21  , 21 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(23  , 24 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(26  , 27 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(29  , 29 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(31  , 31 ,"I1"    , "DispUsina"),


            };
    }
}

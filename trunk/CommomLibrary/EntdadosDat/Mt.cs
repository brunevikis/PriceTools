using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class MtBlock : BaseBlock<MtLine>
    {




    }

    public class MtLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public int UnidadeGeradora { get { return (int)this[2]; } set { this[2] = value; } }
        public string DiaInic { get { return this[3].ToString(); } set { this[3] = value; } }
        public int HoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public int MeiaHoraInic { get { return (int)this[5]; } set { this[5] = value; } }
        public string DiaFinal { get { return this[6].ToString(); } set { this[6] = value; } }
        public int HoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int MeiaHoraFinal { get { return (int)this[8]; } set { this[8] = value; } }
        public int Dispunidade { get { return (int)this[9]; } set { this[9] = value; } }

        public override BaseField[] Campos { get { return MtCampos; } }

        static readonly BaseField[] MtCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 7 ,"I3"    , "Usina"),//
                new BaseField(9  , 11 ,"I3"    , "UnidadeGeradora"),
                new BaseField(14  , 15 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(17  , 18 ,"I2"    , "HoraDiaInic"),//
                new BaseField(20  , 20 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(22  , 23 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(25  , 26 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(28  , 28 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(30  , 30 ,"I1"    , "DispUnidade"),//


            };
    }
}

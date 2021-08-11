using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class FpBlock : BaseBlock<FpLine>
    {




    }

    public class FpLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public int TipoFuncao { get { return (int)this[2]; } set { this[2] = value; } }
        public int PontoVazTurb { get { return (int)this[3]; } set { this[3] = value; } }
        public int PontoVolArm { get { return (int)this[4]; } set { this[4] = value; } }
        public int Concavidade { get { return (int)this[5]; } set { this[5] = value; } }
        public int Quadraticos { get { return (int)this[6]; } set { this[6] = value; } }
        public float VolUtilPerc { get { return (float)this[7]; } set { this[7] = value; } }
        public float TolPerc { get { return (float)this[8]; } set { this[8] = value; } }

        public override BaseField[] Campos { get { return FpCampos; } }

        static readonly BaseField[] FpCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(4  , 6 ,"I3"    , "Usina"),//
                new BaseField(8  , 8 ,"I1"    , "TipoFuncao"),//
                new BaseField(11  , 13 ,"I3"    , "PontoVazTurb"),//
                new BaseField(16  , 18 ,"I3"    , "PontoVolArm"),//
                new BaseField(21  , 21 ,"I1"    , "Concavidade"),//
                new BaseField(25  , 25 ,"I1"    , "Quadraticos"),//
                new BaseField(30  , 39 ,"F10.0"    , "VolUtilPerc"),//
                new BaseField(40  , 49 ,"F10.0"    , "TolPerc"),//
                

            };
    }
}

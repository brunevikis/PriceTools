using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class TviagBlock : BaseBlock<TviagLine>
    {




    }

    public class TviagLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Montante { get { return (int)this[1]; } set { this[1] = value; } }
        public int Jusante { get { return (int)this[2]; } set { this[2] = value; } }
        public string TipoJus { get { return this[3].ToString(); } set { this[3] = value; } }

        public int TempoViag { get { return (int)this[4]; } set { this[4] = value; } }
        public string TipoCurva { get { return this[3].ToString(); } set { this[3] = value; } }

        public override BaseField[] Campos { get { return TviagCampos; } }

        static readonly BaseField[] TviagCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A5"    , "IdBloco"),//
                new BaseField(7  , 9 ,"I3"    , "usinaMont"),//
                new BaseField(11  , 13 ,"I3"    , "jusante"),//
                new BaseField(15  , 15 ,"A1"    , "TipoJus"),//
                new BaseField(20  , 22 ,"I3"    , "tempoViag"),//
                new BaseField(25  , 25 ,"A1"    , "TipoCurva"),//


            };
    }
}

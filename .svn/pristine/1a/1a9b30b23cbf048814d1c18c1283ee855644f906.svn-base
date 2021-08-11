using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class EzBlock : BaseBlock<EzLine>
    {




    }

    public class EzLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public float PercMaxVol { get { return (float)this[2]; } set { this[2] = value; } }

        public override BaseField[] Campos { get { return EzCampos; } }

        static readonly BaseField[] EzCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 7 ,"I3"    , "Usina"),//
                new BaseField(10  , 14 ,"F5.0"    , "PercMaxVol"),//


            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class PdBlock : BaseBlock<PdLine>
    {




    }

    public class PdLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public float ToleranciaPorc { get { return (float)this[1]; } set { this[1] = value; } }
        public float ToleranciaMw { get { return (float)this[2]; } set { this[2] = value; } }

        public override BaseField[] Campos { get { return PdCampos; } }

        static readonly BaseField[] PdCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(4  , 9 ,"F6.0"    , "%tolerancia"),
                new BaseField(13  , 22 ,"F10.0"    , "toleranciaMW"),


            };
    }
}

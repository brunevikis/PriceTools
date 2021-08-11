using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class GpBlock : BaseBlock<GpLine>
    {




    }

    public class GpLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public float Tol { get { return (float)this[1]; } set { this[1] = value; } }
        public float TolUCT { get { return (float)this[2]; } set { this[2] = value; } }


        public override BaseField[] Campos { get { return GpCampos; } }

        static readonly BaseField[] GpCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 14 ,"F10.3"    , "Tol"),//F10.0
                new BaseField(16  , 25 ,"F10.3"    , "TolUCT"),//F10.0


            };
    }
}

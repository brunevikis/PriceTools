using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class AgBlock : BaseBlock<AgLine>
    {




    }

    public class AgLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int NumEst { get { return (int)this[1]; } set { this[1] = value; } }

        public override BaseField[] Campos { get { return AgCampos; } }

        static readonly BaseField[] AgCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(4  , 6 ,"I3"    , "NumEsta"),//


            };
    }
}

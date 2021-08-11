using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class SpBlock : BaseBlock<SpLine>
    {




    }

    public class SpLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return SpCampos; } }

        static readonly BaseField[] SpCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 5 ,"I1"    , "FlagFormato"),//


            };
    }
}

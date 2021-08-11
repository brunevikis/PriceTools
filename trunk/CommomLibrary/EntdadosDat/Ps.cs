using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class PsBlock : BaseBlock<PsLine>
    {




    }

    public class PsLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return PsCampos; } }

        static readonly BaseField[] PsCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 5 ,"I3"    , "FlagPausar"),//


            };
    }
}

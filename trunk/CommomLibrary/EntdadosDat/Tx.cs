using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class TxBlock : BaseBlock<TxLine>
    {




    }

    public class TxLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return TxCampos; } }

        static readonly BaseField[] TxCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 14 ,"F10.0"    , "Taxa"),//


            };
    }
}

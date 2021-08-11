using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class TfBlock : BaseBlock<TfLine>
    {




    }

    public class TfLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return TfCampos; } }

        static readonly BaseField[] TfCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 14 ,"F10.0"    , "Custo"),//


            };
    }
}

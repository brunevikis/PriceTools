using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class RivarBlock : BaseBlock<RivarLine>
    {




    }

    public class RivarLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return VmCampos; } }

        static readonly BaseField[] VmCampos = new BaseField[] {
                new BaseField(1  , 5 ,"A5"    , "IdBloco"),//
                new BaseField(8  , 10 ,"I3"    , "Nnumero"),//
                new BaseField(12  , 14 ,"I3"    , "Para"),//
                new BaseField(16  , 17 ,"I2"    , "Tipo"),//
                new BaseField(20  , 29 ,"F10.0"    , "Penalidade"),//


            };
    }
}

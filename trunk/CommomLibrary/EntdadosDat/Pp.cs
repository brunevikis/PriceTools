using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class PpBlock : BaseBlock<PpLine>
    {




    }

    public class PpLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return PpCampos; } }

        static readonly BaseField[] PpCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(4  , 4 ,"I1"    , "FlagInfo"),//
                new BaseField(6  , 8 ,"I3"    , "NumIteracoes"),//
                new BaseField(10  , 12 ,"I3"    , "NumPreProc"),//
                new BaseField(14  , 14 ,"I1"    , "TipoInterface"),//


            };
    }
}

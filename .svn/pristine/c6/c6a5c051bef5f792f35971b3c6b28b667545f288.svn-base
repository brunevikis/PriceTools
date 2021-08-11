using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Renovaveis
{
    public class EolicaBarraBlock : BaseBlock<EolicaBarraLine>
    {



    }

    public class EolicaBarraLine : BaseLine
    {
        public string Idbloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public string NumCodigo { get { return this[1].ToString(); } set { this[1] = value; } }
        public string Barra { get { return this[2].ToString(); } set { this[2] = value; } }

        //publics
        public override BaseField[] Campos { get { return PtoperCampos; } }

        static readonly BaseField[] PtoperCampos = new BaseField[] {
                new BaseField(1  , 13 ,"A13"    , "ID"),//
                new BaseField(14  , 20 ,"A7"    , "numCodigo"),//
                new BaseField(21  , 27 ,"A7"    , "barra"),//
            };
    }
}

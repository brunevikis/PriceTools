using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class MetaBlock : BaseBlock<MetaLine>
    {




    }

    public class MetaLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return MetaCampos; } }

        static readonly BaseField[] MetaCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A6"    , "IdBloco"),//
                new BaseField(8  , 13 ,"A6"    , "complemento bloco"),//
                new BaseField(15  , 17 ,"I3"    , "Id conj"),//
                new BaseField(19  , 21 ,"A3"    , "Id subsist /TipoAcoplamento"),//
                new BaseField(23  , 23 ,"I1"    , "NumSemana"),//
                new BaseField(25  , 34 ,"F10.0"    , "Meta"),//
                new BaseField(35  , 44 ,"F10.0"    , "tolABS"),//
                new BaseField(445  , 54 ,"F10.0"    , "tolperc"),//


            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class RsBlock : BaseBlock<RsLine>
    {




    }

    public class RsLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return RsCampos; } }

        static readonly BaseField[] RsCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(4  , 6 ,"I3"    , "TipoVaraivel"),//
                new BaseField(8  , 11 ,"I4"    , "NnumEntidade"),//
                new BaseField(13  , 16 ,"I4"    , "Para"),//
                new BaseField(23  , 26 ,"A4"    , "TipoEntidade"),//
                new BaseField(28  , 39 ,"A12"    , "Comentario"),//


            };
    }
}

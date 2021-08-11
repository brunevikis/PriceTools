using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Renovaveis
{
    public class EolicaSubmBlock : BaseBlock<EolicaSubmLine>
    {



    }

    public class EolicaSubmLine : BaseLine
    {
        public string Idbloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public string NumCodigo { get { return this[1].ToString(); } set { this[1] = value; } }
        public string Submercado { get { return this[2].ToString(); } set { this[2] = value; } }

        //publics
        public override BaseField[] Campos { get { return PtoperCampos; } }

        static readonly BaseField[] PtoperCampos = new BaseField[] {
                new BaseField(1  , 12 ,"A12"    , "ID"),//
                new BaseField(13  , 19 ,"A7"    , "numCodigo"),//
                new BaseField(20  , 23 ,"A4"    , "Submercado"),//
            };
    }
}

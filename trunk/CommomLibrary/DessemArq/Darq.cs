using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DessemArq
{
    public class ArqBlock : BaseBlock<ArqLine>
    {

    }

    public class ArqLine : BaseLine
    {

        //public OperLine() : base() { this[0] = "GL"; }

        public string Minemonico { get { return this[0].ToString(); } set { this[0] = value; } }
        public string Decricao { get { return this[1].ToString(); } set { this[1] = value; } }
        public string NomeArq { get { return this[2].ToString(); } set { this[2] = value; } }


        public override BaseField[] Campos { get { return DiaCampos; } }

        static readonly BaseField[] DiaCampos = new BaseField[] {
               new BaseField(1  , 9 ,"A9"    , "minemonico"),
               new BaseField(11  , 48 ,"A38"    , "descricao"),
               new BaseField(51  , 130 ,"A81"    , "nomeArq"),
            };
    }
}

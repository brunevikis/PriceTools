using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DecodessArq
{
    public class ArqsBlock : BaseBlock<ArqsLine>
    {

    }

    public class ArqsLine : BaseLine
    {

        //public OperLine() : base() { this[0] = "GL"; }


        public string Descricao { get { return this[0].ToString(); } set { this[0] = value; } }
        public string NomeArq { get { return this[1].ToString(); } set { this[1] = value; } }
        public override BaseField[] Campos { get { return ArqsCampos; } }

        static readonly BaseField[] ArqsCampos = new BaseField[] {
               new BaseField(1  , 44 ,"A44"    , "Descricao"),
               new BaseField(45  , 99 ,"A55"    , "Nome Arq"),
            };
    }
}

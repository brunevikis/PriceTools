using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class SistBlock : BaseBlock<SistLine>
    {




    }

    public class SistLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Numero { get { return (int)this[1]; } set { this[1] = value; } }
        public string Sigla { get { return this[2].ToString(); } set { this[2] = value; } }
        public int Flagfict { get { return (int)this[3]; } set { this[3] = value; } }
        public string Nome { get { return this[4].ToString(); } set { this[4] = value; } }

        public override BaseField[] Campos { get { return SistCampos; } }

        static readonly BaseField[] SistCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A6"    , "IdBloco"),//
                new BaseField(8  , 9 ,"I2"    , "numerosist"),//
                new BaseField(11  , 12 ,"A2"    , "sigla"),//
                new BaseField(14  , 15 ,"I2"    , "flag ficticio"),//
                new BaseField(17  , 26 ,"A10"    , "Nome sistema"),//


            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class ShBlock : BaseBlock<ShLine>
    {




    }

    public class ShLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }

        public override BaseField[] Campos { get { return ShCampos; } }

        static readonly BaseField[] ShCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 5 ,"I1"    , "Simulacao"),//
                new BaseField(10  , 10 ,"I1"    , "JuntarBacias"),//
                new BaseField(15  , 17 ,"I3"    , "MinimoUsinas"),//
                new BaseField(20  , 22 ,"I3"    , "MaximoUsinas"),//
                new BaseField(25  , 25 ,"I1"    , "Quebra"),//
                new BaseField(30  , 32 ,"I3"    , "Montante1"),//
                new BaseField(35  , 37 ,"I3"    , "Montante2"),//
                new BaseField(40  , 42 ,"I3"    , "Montante3"),//
                new BaseField(45  , 47 ,"I3"    , "Montante4"),//
                new BaseField(50  , 52 ,"I3"    , "Montante5"),//


            };
    }
}

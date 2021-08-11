using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class ReeBlock : BaseBlock<ReeLine>
    {




    }

    public class ReeLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int ReeNnum { get { return (int)this[1]; } set { this[1] = value; } }
        public int SistNnum { get { return (int)this[2]; } set { this[2] = value; } }
        public string NomeRee { get { return this[4].ToString(); } set { this[4] = value; } }

        public override BaseField[] Campos { get { return ReeCampos; } }

        static readonly BaseField[] ReeCampos = new BaseField[] {
                new BaseField(1  , 3 ,"A3"    , "IdBloco"),//
                new BaseField(7  , 8 ,"I2"    , "ReeNum"),//
                new BaseField(10  , 11 ,"I2"    , "SistNum"),//
                new BaseField(13  , 22 ,"A10"    , "NomeRee"),//


            };
    }
}

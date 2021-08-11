using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class VrBlock : BaseBlock<VrLine>
    {




    }
    public class VrLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int DiaVerao { get { return (int)this[1]; } set { this[1] = value; } }
        public string StatusVerao { get { return this[2].ToString(); } set { this[2] = value; } }

        public override BaseField[] Campos { get { return VrCampos; } }

        static readonly BaseField[] VrCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),
                new BaseField(5 , 6 , "I2"    , "dia Horario verao"),
                new BaseField(10 , 12 , "A3"    , "minemonoco verão"),

            };
    }
}

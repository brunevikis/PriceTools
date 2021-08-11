using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Cotasr
{
    public class CotBlock : BaseBlock<CotLine>
    {

    }

    public class CotLine : BaseLine
    {

        //public OperLine() : base() { this[0] = "GL"; }

        public int Dia { get { return (int)this[0]; } set { this[0] = value; } }
        public int Hora { get { return (int)this[1]; } set { this[1] = value; } }
        public int Meiahora { get { return (int)this[2]; } set { this[2] = value; } }
        public float Demanda { get { return (float)this[3]; } set { this[3] = value; } }

        public override BaseField[] Campos { get { return CotCampos; } }

        static readonly BaseField[] CotCampos = new BaseField[] {
               new BaseField(1  , 2 ,"I2"    , "dia"),
               new BaseField(4  , 5 ,"I2"    , "hora"),
               new BaseField(7  , 7 ,"I1"    , "meia hora"),
               new BaseField(17  , 26 ,"F10.0"    , "cota"),
            };
    }
}

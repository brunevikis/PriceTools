using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class PeBlock : BaseBlock<PeLine>
    {




    }

    public class PeLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public float ValPen { get { return (float)this[1]; } set { this[1] = value; } }
        public float PenFat { get { return (float)this[2]; } set { this[2] = value; } }


        public override BaseField[] Campos { get { return GpCampos; } }

        static readonly BaseField[] GpCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 14 ,"F10.3"    , "Penalidade"),//F10.0
                new BaseField(15  , 24 ,"F10.3"    , "Fator_Penalidade"),//F10.0


            };
    }
}

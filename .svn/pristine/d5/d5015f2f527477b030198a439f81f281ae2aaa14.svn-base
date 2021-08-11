using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class NiBlock : BaseBlock<NiLine>
    {




    }

    public class NiLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int FlagIt { get { return (int)this[1]; } set { this[1] = value; } }
        public int NumIt { get { return (int)this[2]; } set { this[2] = value; } }

        public override BaseField[] Campos { get { return NiCampos; } }

        static readonly BaseField[] NiCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 5 ,"I1"    , "flagiteracoes"),//
                new BaseField(10  , 12 ,"I3"    , "NumIteracoes"),//pode ter letra


            };
    }
}

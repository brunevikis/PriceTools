using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class RdBlock : BaseBlock<RdLine>
    {




    }

    public class RdLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Folga { get { return (int)this[1]; } set { this[1] = value; } }
        public string circuitos { get { return this[2].ToString(); } set { this[2] = value; } }
        public int Rede { get { return (int)this[3]; } set { this[3] = value; } }
        public int FluxoElevador { get { return (int)this[4]; } set { this[4] = value; } }
        public int FluxoSomatorio { get { return (int)this[5]; } set { this[5] = value; } }
        public int Perdas { get { return (int)this[6]; } set { this[6] = value; } }
        public int FormatoRede { get { return (int)this[7]; } set { this[7] = value; } }

        public override BaseField[] Campos { get { return RdCampos; } }

        static readonly BaseField[] RdCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),
                new BaseField(5  , 5 ,"I1"    , "folga"),
                new BaseField(9  , 12 ,"A4"    , "circuitos"),// string pq pode ter letra
                new BaseField(15  , 15 ,"I1"    , "rede"),
                new BaseField(17  , 17 ,"I1"    , "limite de fluxo elevadores"),
                new BaseField(19  , 19 ,"I1"    , "limitede fluxo e somatorio"),
                new BaseField(21  , 21 ,"I1"    , "Perdas"),
                new BaseField(23  , 23 ,"I1"    , "formato de rede"),


            };
    }
}

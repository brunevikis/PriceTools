using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Infofcf
{
    public class BenfutBlock : BaseBlock<BenfutLine>
    {



    }

    public class BenfutLine : BaseLine
    {
        public string Id { get { return this[0].ToString(); } set { this[0] = value; } }
        public int SubMercado { get { return (int)this[1]; } set { this[1] = value; } }
        public int Estagio { get { return (int)this[2]; } set { this[2] = value; } }
        public double CMO { get { return this[3]; } set { this[3] = value; } }


        //publics
        public override BaseField[] Campos { get { return BenfutCampos; } }

        static readonly BaseField[] BenfutCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A6"    , "ID bloco"),//
                new BaseField(8  , 10 ,"I3"    , "SubMercado"),//
                new BaseField(12  , 14 ,"I3"    , "Estagio"),//
                new BaseField(16  , 25 ,"F10.2"    , "CMO"),//
              
              
            };
    }
}

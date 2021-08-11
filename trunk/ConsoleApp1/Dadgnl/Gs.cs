using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.Dadgnl {
    public class GsBlock : BaseBlock<GsLine> {
        
    }

    public class GsLine : BaseLine {

        public GsLine() : base() { this[0] = "GS"; }

        static readonly BaseField[] GsCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "Id"),
                new BaseField(5  , 6 ,"I2"    , "Mes"),
                new BaseField(10  , 10 ,"I1"    , "Intervalos"),
            };
        public override BaseField[] Campos {
            get { return GsCampos; }
        }

        public int mes { get { return this[1]; } set { this[1] = value; } }
        public int semanas { get { return this[2]; } set { this[2] = value; } }

    }


}

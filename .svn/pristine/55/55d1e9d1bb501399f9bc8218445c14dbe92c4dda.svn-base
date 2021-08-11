using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class FcBlock : BaseBlock<FcLine> {

        public FcLine Cortes { get { return this.First(c => c.Mnemonico.Equals("NEWCUT", StringComparison.OrdinalIgnoreCase)); } }
        public FcLine CortesInfo { get { return this.First(c => !c.Mnemonico.Equals("NEWCUT", StringComparison.OrdinalIgnoreCase)); } }

    }

    public class FcLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 10 ,"A6", "Mnemonico"  ),
                new BaseField( 15 , 74 ,"A60", "Arquivo"  ),

            };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public string Mnemonico { get { return valores[campos[1]]; } set { valores[campos[1]] = value; } }
        public string Arquivo { get { return valores[campos[2]]; } set { valores[campos[2]] = value; } }
    }


}

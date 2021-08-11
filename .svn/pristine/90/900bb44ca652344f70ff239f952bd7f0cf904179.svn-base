using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class PqBlock : BaseBlock<PqLine> {
              

    }

    public class PqLine : BaseLine {

        public PqLine()
            : base() {
                this[0] = "PQ";
        }

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 14 ,"A10"  , "Usina"),
                new BaseField( 15 , 16 ,"I2", "Mercado"  ),
                new BaseField( 20 , 21 ,"I2", "Estagio"  ),
                new BaseField( 25 , 29 ,"F5.0", "Pat 1"  ),
                new BaseField( 30 , 34 ,"F5.0", "Pat 2"  ),
                new BaseField( 35 , 39 ,"F5.0", "Pat 3"  ),                
                new BaseField( 60 , 64 ,"F5.0", "Fator de Perda"  ),

            };

        public override BaseField[] Campos {
            get { return campos; }
        }

       
    }

    
}

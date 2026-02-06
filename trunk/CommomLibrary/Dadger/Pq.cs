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
                new BaseField( 25 , 29 ,"F5.3", "Pat 1"  ),
                new BaseField( 30 , 34 ,"F5.3", "Pat 2"  ),
                new BaseField( 35 , 39 ,"F5.3", "Pat 3"  ),                
                new BaseField( 60 , 64 ,"F5.3", "Fator de Perda"  ),

            };

        public override BaseField[] Campos {
            get { return campos; }
        }
        public string Usina { get { return (string)this[1]; } set { this[1] = value; } }
        public int SubMercado { get { return (int)this[2]; } set { this[2] = value; } }
        public int Estagio { get { return (int)this[3]; } set { this[3] = value; } }

        public double Pat1 { get { return (double)this[4]; } set { this[4] = value; } }
        public double Pat2 { get { return (double)this[5]; } set { this[5] = value; } }
        public double Pat3 { get { return (double)this[6]; } set { this[6] = value; } }
    }

    
}

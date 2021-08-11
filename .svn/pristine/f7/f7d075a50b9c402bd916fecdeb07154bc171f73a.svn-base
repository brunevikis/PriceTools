using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class ItBlock : BaseBlock<ItLine> {


    }

    public class ItLine : BaseLine {

        public ItLine()
            : base()
        {
            this[0] = "IT";
        }

        static readonly BaseField[] DpCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"   ,"Id" ),
                new BaseField( 5 , 6 ,"I2"   ,"Estagio" ),                
                new BaseField( 10, 12 ,"I3"   ,"Cod Itaipu" ),
                new BaseField( 15, 16,"I2"   ,"Subsistema" ),                                
                new BaseField( 20, 24,"F5.0" ,"Geracao Pat1" ),
                new BaseField( 25, 29,"F5.0" ,"Ande Pat1" ),                
                new BaseField( 30, 34,"F5.0" ,"Geracao Pat2" ),
                new BaseField( 35, 39,"F5.0" ,"Ande Pat2" ),                
                new BaseField( 40, 44,"F5.0" ,"Geracao Pat3" ),
                new BaseField( 45, 49,"F5.0" ,"Ande Pat3" ),
                
            };

        public override BaseField[] Campos {
            get { return DpCampos; }
        }
    }    
}

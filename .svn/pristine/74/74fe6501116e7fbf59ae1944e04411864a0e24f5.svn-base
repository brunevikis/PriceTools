using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class CdBlock : BaseBlock<CdLine> {


    }

    public class CdLine : BaseLine {

        public CdLine() : base()
        {
            this[0] = "CD";
        }

        static readonly BaseField[] DpCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"   ,"Id" ),
                new BaseField( 5 , 6 ,"I2"   ,"Numero" ),
                new BaseField( 10 ,11,"I2"   ,"Subsistema" ),
                new BaseField( 15 ,24,"A10"   ,"Nome" ),
                new BaseField( 25 ,26 ,"I2"   ,"Estagio" ),                
                new BaseField( 30,	34,"F5.0" ,"Intervalo Pat1" ),
                new BaseField( 35,	44,"F10.2" ,"Custo Pat1" ),                
                new BaseField( 45,	49,"F5.0" ,"Intervalo Pat2" ),
                new BaseField( 50,	59,"F10.2" ,"Custo Pat2" ),                
                new BaseField( 60,	64,"F5.0" ,"Intervalo Pat3" ),
                new BaseField( 65,	74,"F10.2" ,"Custo Pat3" ),
                
            };

        public override BaseField[] Campos {
            get { return DpCampos; }
        }
    }    
}

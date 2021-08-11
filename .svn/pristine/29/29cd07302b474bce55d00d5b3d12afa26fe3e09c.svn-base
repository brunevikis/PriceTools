using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class IaBlock : BaseBlock<IaLine> {


    }

    public class IaLine : BaseLine {

        public IaLine()
            : base()
        {
            this[0] = "IA";
        }

        static readonly BaseField[] DpCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"   ,"Id" ),
                new BaseField( 5 , 6 ,"I2"   ,"Estagio" ),                
                new BaseField( 10, 11 ,"A2"   ,"Subsistema 1" ),
                new BaseField( 15, 16,"A2"   ,"Subsistema 2"  ),                                
                new BaseField( 18 ,18 ,"I1"   ,"Penalidade" ), 
                new BaseField( 20, 29,"F10.0" ,"1-2 Pat1" ),
                new BaseField( 30, 39,"F10.0" ,"2-1 Pat1" ),                
                new BaseField( 40, 49,"F10.0" ,"1-2 Pat2" ),
                new BaseField( 50, 59,"F10.0" ,"2-1 Pat2" ),                
                new BaseField( 60, 69,"F10.0" ,"1-2 Pat3" ),
                new BaseField( 70, 79,"F10.0" ,"2-1 Pat3" ),
                
            };

        public override BaseField[] Campos {
            get { return DpCampos; }
        }
    }    
}

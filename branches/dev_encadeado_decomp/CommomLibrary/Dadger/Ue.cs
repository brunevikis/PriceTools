using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class UeBlock : BaseBlock<UeLine> {
        
    }
    public class UeLine : BaseLine{
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"Z3"    , "Estacao"),
                new BaseField( 10 , 11,"I2"    , "Subsistema"),
                new BaseField( 15 , 26,"A12" , "Nome"),
                new BaseField( 30 , 32 ,"I3"    , "Montante"),
                new BaseField( 35 , 37 ,"I3"    , "Jusante"),
                new BaseField( 40 , 49,"f10.2" , "Vazao Min"),
                new BaseField( 50 , 59,"f10.2" , "Vazao Max"),
                new BaseField( 60 , 69,"f10.2" , "Consumo"),
                

            };



        public override BaseField[] Campos {
            get { return campos; }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Relato {
    public class RelatoDadosMercadoBlock : BaseBlock<RelatoDadosMercadoLine> {        
    }


    public class RelatoDadosMercadoLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(5  , 13 ,"I3"    , "Estagio"),    
                new BaseField(15  ,20 ,"A6"    , "Subsistema"),                                
                new BaseField(22 , 30 ,"F7.2"  , "Horas Pat1"),
                new BaseField(32 , 40 ,"F7.2"  , "Mercado Pat1"),
                new BaseField(42 , 50 ,"F7.2"  , "Horas Pat1"),
                new BaseField(52 , 60 ,"F7.2"  , "Mercado Pat1"),
                new BaseField(62 , 70 ,"F7.2"  , "Horas Pat1"),
                new BaseField(72 , 80 ,"F7.2"  , "Mercado Pat1"),                
        };



        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    
}

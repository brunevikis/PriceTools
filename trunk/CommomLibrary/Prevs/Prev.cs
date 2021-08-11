using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Prevs {
    public class PrevBlock : BaseBlock<PrevLine> {
    }


    public class PrevLine : BaseLine {
        
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 6 ,"I6"  , "Seq"),    
                new BaseField( 7 , 11 ,"I5"  , "Posto"),
                new BaseField( 12 , 21 ,"I10"  , "P0"),
                new BaseField( 22 , 31 ,"I10"  , "P1"),
                new BaseField( 32 , 41 ,"I10"  , "P2"),
                new BaseField( 42 , 51 ,"I10"  , "P3"),
                new BaseField( 52 , 61 ,"I10"  , "P4"),
                new BaseField( 62 , 71 ,"I10"  , "P5"),

        };

        public override BaseField[] Campos {
            get { return campos; }
        }


    }

}



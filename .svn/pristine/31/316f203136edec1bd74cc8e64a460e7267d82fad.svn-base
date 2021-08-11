using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DgerNwd {
    public class EafBlock : BaseBlock<EafLine> {

        string header =
@"energias afluentes passadas      (REFERENTES A 65% DE VOLUME ARMAZENADO)
mes xxsis1.xxx xxsis2.xxx xxsis3.xxx xxsis4.xxx xxsis5.xxx xxsis6.xxx xxsis7.xxx xxsis8.xxx xxsis9.xxx xxsis1.xxx
";

      
        public override string ToText() {

            return header + base.ToText();
        }

    }

    public class EafLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 1  ,  2  ,"I2"  ,  "Mes"),
                new BaseField( 5  , 14  ,"F10.3"  ,  "ENA REE 1"),
                new BaseField( 16 , 25  ,"F10.3"  ,  "ENA REE 2"),
                new BaseField( 27 , 36  ,"F10.3"  ,  "ENA REE 3"),
                new BaseField( 38 , 47  ,"F10.3"  ,  "ENA REE 4"),
                new BaseField( 49 , 58  ,"F10.3"  ,  "ENA REE 5"),
                new BaseField( 60 , 69  ,"F10.3"  ,  "ENA REE 6"),
                new BaseField( 71 , 80  ,"F10.3"  ,  "ENA REE 7"),
                new BaseField( 82 , 91  ,"F10.3"  ,  "ENA REE 8"),
                new BaseField( 93 , 102 ,"F10.3"  ,  "ENA REE 9"),
                new BaseField( 104, 113 ,"F10.3"  ,  "ENA REE 10"),
                new BaseField( 115, 124 ,"F10.3"  ,  "ENA REE 11"),
                new BaseField( 126, 135 ,"F10.3"  ,  "ENA REE 12"),
                new BaseField( 137, 146 ,"F10.3"  ,  "ENA REE 13"),
                new BaseField( 148, 157 ,"F10.3"  ,  "ENA REE 14"),
                new BaseField( 159, 168 ,"F10.3"  ,  "ENA REE 15"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}


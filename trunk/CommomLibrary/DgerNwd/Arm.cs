using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DgerNwd {
    public class ArmBlock : BaseBlock<ArmLine> {

        string header =
@"energia armazenada
 xxsis1.xxx xxsis2.xxx xxsis3.xxx xxsis4.xxx xxsis5.xxx xxsis6.xxx xxsis7.xxx xxsis8.xxx xxsis9.xxx xxsi10.xxx
";
                

        public override string ToText() {

            return header + base.ToText();
        }

        public new double this[int i] {
            get {                
                return this.First()[i]; }

            set { this.First()[i] = value; }
        }

    }

    public class ArmLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 2 ,  11  ,"F10.3"  ,  "Arm REE 1"),
                new BaseField( 13 , 22  ,"F10.3"  ,  "Arm REE 2"),
                new BaseField( 24 , 33  ,"F10.3"  ,  "Arm REE 3"),
                new BaseField( 35 , 44  ,"F10.3"  ,  "Arm REE 4"),
                new BaseField( 46 , 55  ,"F10.3"  ,  "Arm REE 5"),
                new BaseField( 57 , 66  ,"F10.3"  ,  "Arm REE 6"),
                new BaseField( 68 , 77  ,"F10.3"  ,  "Arm REE 7"),
                new BaseField( 79 , 88  ,"F10.3"  ,  "Arm REE 8"),
                new BaseField( 90 , 99  ,"F10.3"  ,  "Arm REE 9"),
                new BaseField( 101, 110 ,"F10.3"  ,  "Arm REE 10"),
                new BaseField( 112, 121 ,"F10.3"  ,  "Arm REE 11"),
                new BaseField( 123, 132 ,"F10.3"  ,  "Arm REE 12"),
                new BaseField( 134, 143 ,"F10.3"  ,  "Arm REE 13"),
                new BaseField( 145, 154 ,"F10.3"  ,  "Arm REE 14"),
                new BaseField( 156, 165 ,"F10.3"  ,  "Arm REE 15"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}











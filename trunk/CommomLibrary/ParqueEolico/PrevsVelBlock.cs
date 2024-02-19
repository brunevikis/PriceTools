using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParqueEolico
{
    public class PrevsVelBlock : BaseBlock<PrevsVelLine>
    {
    }


    public class PrevsVelLine : BaseLine
    {

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 4 , 6 ,"I3"  , "Seq"),
                new BaseField( 9 , 11 ,"I3"  , "Posto"),
                new BaseField( 13 , 21 ,"F9.2"  , "P1"),
                new BaseField( 23 , 31 ,"F9.2"  , "P2"),
                new BaseField( 33 , 41 ,"F9.2"  , "P3"),
                new BaseField( 43 , 51 ,"F9.2"  , "P4"),
                new BaseField( 53 , 61 ,"F9.2"  , "P5"),
                new BaseField( 63 , 71 ,"F9.2"  , "P6"),

        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Seq { get { return this[0]; } set { this[0] = value; } }

        public int Posto { get { return this[1]; } set { this[1] = value; } }

        public double P1 { get { return this[2]; } set { this[2] = value; } }
        public double P2 { get { return this[3]; } set { this[3] = value; } }
        public double P3 { get { return this[4]; } set { this[4] = value; } }
        public double P4 { get { return this[5]; } set { this[5] = value; } }
        public double P5 { get { return this[6]; } set { this[6] = value; } }
        public double P6 { get { return this[7]; } set { this[7] = value; } }


    }
}

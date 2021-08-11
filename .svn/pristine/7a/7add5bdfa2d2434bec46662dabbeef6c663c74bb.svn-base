using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Deflant
{
    public class DefBlock : BaseBlock<DefLine>
    {

       
    }

    public class DefLine : BaseLine
    {

        public DefLine() : base() { this[0] = "DEFANT"; }

        public int Montante { get { return (int)this[1]; } set { this[1] = value; } }
        public int Jusante { get { return (int)this[2]; } set { this[2] = value; } }

        public string Tipo { get { return this[3].ToString(); } set { this[3] = value; } }
        public int Diainic { get { return (int)this[4]; } set { this[4] = value; } }
        public int Horainic { get { return (int)this[5]; } set { this[5] = value; } }
        public int Meiainic { get { return (int)this[6]; } set { this[6] = value; } }

        public string Diafim { get { return this[7].ToString(); } set { this[7] = value; } }
        public int Horafim { get { return (int)this[8]; } set { this[8] = value; } }
        public int Meiafim { get { return (int)this[9]; } set { this[9] = value; } }
        public float Defluencia { get { return (float)this[10]; } set { this[10] = value; } }




        public override BaseField[] Campos { get { return DefCampos; } }

        static readonly BaseField[] DefCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A6"    , "id"),
                new BaseField(10  , 12 ,"I3"    , "montante"),
                new BaseField(15 , 17,"I3"    , "jusante"),
                new BaseField(20 , 20,"A1"    , "tipo"),
                new BaseField(25 , 26,"I2" , "diainic"),
                new BaseField(28 , 29,"I2"  , "horainic"),
                new BaseField(31 , 31,"I1" , "meiainic"),
                new BaseField(33 , 34,"A2"  , "diafim"),
                new BaseField(36 , 37,"I2"  , "horafim"),
                new BaseField(39 , 39,"I1"  , "meiafim"),
                new BaseField(45 , 54,"F10.0"  , "Defluencia"),

            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EnaSemanalLog
{
    public class EnaBlock : BaseBlock<EnaLine>
    {



    }

    public class EnaLine : BaseLine
    {

        public string texto { get { return this[0].ToString(); } set { this[0] = value; } }
        public float ena1 { get { return (float)this[1]; } set { this[1] = value; } }
        public float ena2 { get { return (float)this[2]; } set { this[2] = value; } }
        public float ena3 { get { return (float)this[3]; } set { this[3] = value; } }
        public float ena4 { get { return (float)this[4]; } set { this[4] = value; } }
        public float ena5 { get { return (float)this[5]; } set { this[5] = value; } }
        public float ena6 { get { return (float)this[6]; } set { this[6] = value; } }
        public float ena7 { get { return (float)this[7]; } set { this[7] = value; } }
        public float ena8 { get { return (float)this[8]; } set { this[8] = value; } }
        public float ena9 { get { return (float)this[9]; } set { this[9] = value; } }
        public float ena10 { get { return (float)this[10]; } set { this[10] = value; } }
        public float ena11 { get { return (float)this[11]; } set { this[11] = value; } }
        public float ena12 { get { return (float)this[12]; } set { this[12] = value; } }






        public override BaseField[] Campos { get { return EnaCampos; } }

        static readonly BaseField[] EnaCampos = new BaseField[] {
                new BaseField(1  , 22 ,"A22"    , "texto"),
                new BaseField(23  , 35 ,"F13.5"    , "ena1"),
                new BaseField(38  , 50 ,"F13.5"    , "ena2"),
                new BaseField(53  , 65 ,"F13.5"    , "ena3"),
                new BaseField(68  , 80 ,"F13.5"    , "ena4"),
                new BaseField(83  , 95 ,"F13.5"    , "ena5"),
                new BaseField(98  , 110 ,"F13.5"    , "ena6"),
                new BaseField(113  , 125 ,"F13.5"    , "ena7"),
                new BaseField(128  , 140 ,"F13.5"    , "ena8"),
                new BaseField(143  , 155 ,"F13.5"    , "ena9"),
                new BaseField(158  , 170 ,"F13.5"    , "ena10"),
                new BaseField(173  , 185 ,"F13.5"    , "ena11"),
                new BaseField(188  , 200 ,"F13.5"    , "ena12"),


            };
    }
}

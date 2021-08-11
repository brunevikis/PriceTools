using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Areacont
{
    public class UsinaBlock : BaseBlock<UsinaLine>
    {

    }

    public class UsinaLine : BaseLine
    {

        //public OperLine() : base() { this[0] = "GL"; }

        public int NumCad { get { return (int)this[0]; } set { this[0] = value; } }
        public int freqItaipu { get { return (int)this[1]; } set { this[1] = value; } }

        public string Tipo { get { return this[2].ToString(); } set { this[2] = value; } }
        public string NumCadUsina { get { return this[3].ToString(); } set { this[3] = value; } }
        public string Desc { get { return this[4].ToString(); } set { this[4] = value; } }

        public override BaseField[] Campos { get { return UsinaCampos; } }

        static readonly BaseField[] UsinaCampos = new BaseField[] {
               new BaseField(1  , 3 ,"I3"    , "num cadastro"),
               new BaseField(5  , 5 ,"I1"    , "FreqItaipu"),
               new BaseField(8  , 8 ,"A1"    , "TipoUsina"),
               new BaseField(10  , 12 ,"A3"    , "NUmCadMine"),
               new BaseField(15  , 54 ,"A40"    , "descri"),
            };
    }
}

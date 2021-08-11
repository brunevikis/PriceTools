using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Areacont
{
    public class AreaBlock : BaseBlock<AreaLine>
    {

    }

    public class AreaLine : BaseLine
    {

        //public OperLine() : base() { this[0] = "GL"; }

        public int NumCad { get { return (int)this[0]; } set { this[0] = value; } }

        public string NomeArea { get { return this[1].ToString(); } set { this[1] = value; } }

        public override BaseField[] Campos { get { return AreaCampos; } }

        static readonly BaseField[] AreaCampos = new BaseField[] {
               new BaseField(1  , 3 ,"I3"    , "num cadastro"),
               new BaseField(10  , 49 ,"A40"    , "Nome area"),
            };
    }
}

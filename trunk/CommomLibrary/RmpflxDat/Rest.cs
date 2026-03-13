using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.RmpflxDat
{
    public class RestBlock : BaseBlock<RestLine>
    {

    }

    public class RestLine : BaseLine
    {

        //public InitLine() : base() { this[0] = "GL"; }


        public string Id { get { return this[0].ToString(); } set { this[0] = value; } }
        public string Minemonico { get { return this[1].ToString(); } set { this[1] = value; } }
        public int DREnum { get { return (int)this[2]; } set { this[2] = value; } }

        public float ValorIneq { get { return (float)this[3]; } set { this[3] = value; } }

        public int Tipo { get { return (int)this[4]; } set { this[4] = value; } }



        public override BaseField[] Campos { get { return RestCampos; } }

        static readonly BaseField[] RestCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A6"    , "Id"),
                new BaseField(8  , 11 ,"A4"    , "Minemonico"),
                new BaseField(13  , 16 ,"I4"    , "DREnum"),
                new BaseField(18  , 27 ,"F10.0"    , "ValorIneq"),
                new BaseField(29  , 29 ,"I1"    , "Tipo"),

            };
    }
}

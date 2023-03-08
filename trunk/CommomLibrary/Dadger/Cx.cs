using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger
{
    public class CxBlock : BaseBlock<CxLine>
    {

    }
    public class CxLine : BaseLine
    {

        public CxLine()
            : base()
        {
            this[0] = "CX";
        }

        static readonly BaseField[] CxCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 8 ,"I4"    , "Usina NW"),
                new BaseField( 10 , 13,"I4"    , "Usina DC"),

            };

        public override BaseField[] Campos { get { return CxCampos; } }
        
        public int UsinaNW { get { return (int)this[1]; } set { this[1] = value; } }
        public int UsinaDC { get { return (int)this[2]; } set { this[2] = value; } }


    }

}

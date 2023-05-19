using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger
{
    public class VtBlock : BaseBlock<VtLine>
    {

    }

    public class VtLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 54 ,"A50", "ARQ_CENARIO_VENTOS"  ),

            };

        public override BaseField[] Campos
        {
            get { return campos; }
        }
    }


}

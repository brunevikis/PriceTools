using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Respot
{
    public class RpBlock : BaseBlock<RpLine>
    {



    }

    public class RpLine : BaseLine
    {
        public string IdLine { get { return this[0].ToString(); } set { this[0] = value; } }
        public int AREA { get { return (int)this[1]; } set { this[1] = value; } }
        public string DiaIni { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraDiaIni { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraIni { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraDiaFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public string Comentario { get { return this[8].ToString(); } set { this[8] = value; } }

        //publics
        public override BaseField[] Campos { get { return RpCampos; } }

        static readonly BaseField[] RpCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "ID bloco"),//
                new BaseField(5  , 7 ,"I3"    , "area "),//
                new BaseField(10  , 11 ,"A2"    , "dia ini "),//
                new BaseField(13  , 14 ,"I2"    , "hora ini "),//
                new BaseField(16  , 16 ,"I1"    , "meia ini "),//
                new BaseField(18  , 19 ,"A2"    , "dia fim "),//
                new BaseField(21  , 22 ,"I2"    , "hora fim "),//
                new BaseField(24  , 24 ,"I1"    , "meia fim "),//
                new BaseField(31  , 70 ,"A40"    , "comentario"),//
            };
    }
}

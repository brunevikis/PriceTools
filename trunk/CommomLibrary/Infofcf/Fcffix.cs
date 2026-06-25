using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Infofcf
{
    public class FcffixBlock : BaseBlock<FcffixLine>
    {



    }

    public class FcffixLine : BaseLine
    {
        public string Id { get { return this[0].ToString(); } set { this[0] = value; } }
        public string Minemonico { get { return this[1].ToString(); } set { this[1] = value; } }
        public int Usina { get { return (int)this[2]; } set { this[2] = value; } }
        public string TipoVar { get { return this[3].ToString(); } set { this[3] = value; } }
        public int Indice { get { return (int)this[4]; } set { this[4] = value; } }
        public int Patamar { get { return (int)this[5]; } set { this[5] = value; } }
        public double Valor { get { return this[6]; } set { this[6] = value; } }
        public string Comentario { get { return this[7].ToString(); } set { this[7] = value; } }


        //publics
        public override BaseField[] Campos { get { return FcffixCampos; } }

        static readonly BaseField[] FcffixCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A6"    , "ID bloco"),//
                new BaseField(8  , 13 ,"A6"    , "Minemonico"),//
                new BaseField(15  , 17 ,"I3"    , "Usina"),//
                new BaseField(19  , 24 ,"A6"    , "TipoVar"),//
                new BaseField(26  , 28 ,"I3"    , "IndiceLag"),//
                new BaseField(30  , 32 ,"I3"    , "Patamar"),//
                new BaseField(34  , 43 ,"F10.2"    , "Valor"),//
                new BaseField(45  , 64 ,"A32"    , "Comentario"),//
              
            };
    }
}

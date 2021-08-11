using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class UsieBlock : BaseBlock<UsieLine>
    {




    }

    public class UsieLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public int Subsist { get { return (int)this[2]; } set { this[2] = value; } }
        public string NomeUsina { get { return this[3].ToString(); } set { this[3] = value; } }
        public int UsinaMont { get { return (int)this[4]; } set { this[4] = value; } }
        public int UsinaJus { get { return (int)this[5]; } set { this[5] = value; } }
        public float VazMin { get { return (float)this[6]; } set { this[6] = value; } }
        public float VazMax { get { return (float)this[7]; } set { this[7] = value; } }
        public float TaxaxCons { get { return (float)this[8]; } set { this[8] = value; } }

        public override BaseField[] Campos { get { return UsieCampos; } }

        static readonly BaseField[] UsieCampos = new BaseField[] {
                new BaseField(1  , 4 ,"A4"    , "IdBloco"),//
                new BaseField(6 , 8 ,"I3"    , "Usina"),//
                new BaseField(10 , 11 ,"I2"    , "Subsist"),//
                new BaseField(15 , 26 ,"A12"    , "NomeUsina"),//
                new BaseField(30 , 32 ,"I3"    , "UsinaMontante"),//
                new BaseField(35 , 37 ,"I3"    , "UsinaJusante"),//
                new BaseField(40 , 49 ,"F10.0"    , "VazMin"),//
                new BaseField(50 , 59 ,"F10.0"    , "VazMax"),//
                new BaseField(60 , 69 ,"F10.0"    , "TaxaConsumo"),//


            };
    }
}

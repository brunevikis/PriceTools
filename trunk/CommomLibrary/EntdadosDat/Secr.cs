using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class SecrBlock : BaseBlock<SecrLine>
    {




    }

    public class SecrLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int SecaoRio { get { return (int)this[1]; } set { this[1] = value; } }
        public string NomeSecao { get { return this[2].ToString(); } set { this[2] = value; } }
        public int Mont1 { get { return (int)this[3]; } set { this[3] = value; } }
        public float Fator1 { get { return (float)this[4]; } set { this[4] = value; } }
        public int Mont2 { get { return (int)this[5]; } set { this[5] = value; } }
        public float Fator2 { get { return (float)this[6]; } set { this[6] = value; } }
        public int Mont3 { get { return (int)this[7]; } set { this[7] = value; } }
        public float Fator3 { get { return (float)this[8]; } set { this[8] = value; } }
        public int Mont4 { get { return (int)this[9]; } set { this[9] = value; } }
        public float Fator4 { get { return (float)this[10]; } set { this[10] = value; } }
        public int Mont5 { get { return (int)this[11]; } set { this[11] = value; } }
        public float Fator5 { get { return (float)this[12]; } set { this[12] = value; } }

        public override BaseField[] Campos { get { return SecrCampos; } }

        static readonly BaseField[] SecrCampos = new BaseField[] {
                new BaseField(1  , 4 ,"A4"    , "IdBloco"),//
                new BaseField(6  , 8 ,"I3"    , "SecaoRio"),//
                new BaseField(10  , 21 ,"A12"    , "NomeSecaoRio"),//
                new BaseField(25  , 27 ,"I3"    , "PrUsinaMont"),//
                new BaseField(29  , 33 ,"F5.2"    , "FatorPrUsinaMont"),//
                new BaseField(35  , 37 ,"I3"    , "SegUsinaMont"),//
                new BaseField(39  , 43 ,"F5.2"    , "FatorSegUsinaMont"),//
                new BaseField(45  , 47 ,"I3"    , "TerUsinaMont"),//
                new BaseField(49  , 53 ,"F5.2"    , "FatorTerUsinaMont"),//
                new BaseField(55  , 57 ,"I3"    , "QuarUsinaMont"),//
                new BaseField(59  , 63 ,"F5.2"    , "FatorQuarUsinaMont"),//
                new BaseField(65  , 67 ,"I3"    , "QuintaUsinaMont"),//
                new BaseField(69  , 73 ,"F5.2"    , "FatorQuintaUsinaMont"),//

            };
    }
}

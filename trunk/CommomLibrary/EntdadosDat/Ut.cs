using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class UtBlock : BaseBlock<UtLine>
    {




    }

    public class UtLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public string NomeUsina { get { return this[2].ToString(); } set { this[2] = value; } }
        public int SubSist { get { return (int)this[3]; } set { this[3] = value; } }
        public int RestTipo { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaInic { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraInic { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraInic { get { return (int)this[7]; } set { this[7] = value; } }
        public string DiaFinal { get { return this[8].ToString(); } set { this[8] = value; } }
        public int HoraFinal { get { return (int)this[9]; } set { this[9] = value; } }
        public int MeiaHoraFinal { get { return (int)this[10]; } set { this[10] = value; } }
        public int UnidRest { get { return (int)this[11]; } set { this[11] = value; } }
        public float GeracaoMinRest { get { return (float)this[12]; } set { this[12] = value; } }
        public float GeracaoMaxRest { get { return (float)this[13]; } set { this[13] = value; } }
        public float GeracaoMeiaHoraAnt { get { return (float)this[14]; } set { this[14] = value; } }

        public override BaseField[] Campos { get { return UtCampos; } }

        static readonly BaseField[] UtCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 7 ,"I3"    , "NumUsina"),//
                new BaseField(10  , 21 ,"A12"    , "NomeUsina"),//
                new BaseField(23  , 24 ,"I2"    , "Subsist"),//
                new BaseField(26  , 26 ,"I1"    , "FlagRestTipo"),//
                new BaseField(28  , 29 ,"A2"    , "DiaInic"),//string pq pode ser letra
                new BaseField(31  , 32 ,"I2"    , "HoraInic"),//
                new BaseField(34  , 34 ,"I1"    , "MeiaHoraInic"),//
                new BaseField(36  , 37 ,"A2"    , "DiaFinal"),//pode ser letra
                new BaseField(39  , 40 ,"I2"    , "HoraFinal"),//
                new BaseField(42  , 42 ,"I1"    , "MeiaHoraFinal"),//
                new BaseField(47  , 47 ,"I1"    , "UnidRest"),//
                new BaseField(48  , 57 ,"F10.0"    , "GeracaoMinPorRest"),//para rest de rampa é variacao max de decrescimo
                new BaseField(58  , 67 ,"F10.0"    , "GeracaoMaxPorRest"),//para rest de rampa é variacao max de acrescimo
                new BaseField(68  , 77 ,"F10.0"    , "GeracaoMeiaHoraAnt"),//


            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class CiceBlock : BaseBlock<CiceLine>
    {




    }

    public class CiceLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int IdContrato { get { return (int)this[1]; } set { this[1] = value; } }
        public string NomeContrato { get { return this[2].ToString(); } set { this[2] = value; } }
        public int Sist_Barra { get { return (int)this[3]; } set { this[3] = value; } }
        public int TipoRest { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaInic { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraInic { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraInic { get { return (int)this[7]; } set { this[7] = value; } }
        public string DiaFinal { get { return this[8].ToString(); } set { this[8] = value; } }
        public int HoraFinal { get { return (int)this[9]; } set { this[9] = value; } }
        public int MeiaHoraFinal { get { return (int)this[10]; } set { this[10] = value; } }
        public int UnidRest { get { return (int)this[11]; } set { this[11] = value; } }
        public float EnergiaMin { get { return (float)this[12]; } set { this[12] = value; } }
        public float EnergiaMax { get { return (float)this[13]; } set { this[13] = value; } }
        public float Preco { get { return (float)this[14]; } set { this[14] = value; } }
        public float EnergiaMeiaAnt { get { return (float)this[15]; } set { this[15] = value; } }

        public override BaseField[] Campos { get { return CiceCampos; } }

        static readonly BaseField[] CiceCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(4  , 6 ,"I3"    , "IdContrato"),//
                new BaseField(8  , 17 ,"A10"    , "NomeContrato"),//
                new BaseField(19  , 23 ,"I5"    , "Sist_barra"),//
                new BaseField(24  , 24 ,"I1"    , "TipoRest"),//
                new BaseField(26  , 27 ,"A2"    , "DiaInic"),//pode ser letra
                new BaseField(29  , 30 ,"I2"    , "HoraDiaInic"),//
                new BaseField(32  , 32 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(34  , 35 ,"A2"    , "DiaFinal"),//pode ser letra
                new BaseField(37  , 38 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(40  , 40 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(42  , 42 ,"I1"    , "UnidRest"),//
                new BaseField(44  , 53 ,"F10.2"    , "EnergiaMin"),//F10.0
                new BaseField(54  , 63 ,"F10.2"    , "EnergiaMax"),//
                new BaseField(64  , 73 ,"F10.2"    , "Preco"),
                new BaseField(74  , 83 ,"F10.0"    , "EnergiaMeiaAnt"),


            };
    }
}

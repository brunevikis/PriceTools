using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.PtoperDat
{
    public class PtoperBlock : BaseBlock<PtoperLine>
    {



    }

    public class PtoperLine : BaseLine
    {
        public string IdLine { get { return this[0].ToString(); } set { this[0] = value; } }
        public string Minemonico { get { return this[1].ToString(); } set { this[1] = value; } }
        public int usina { get { return (int)this[2]; } set { this[2] = value; } }
        public string variavel { get { return this[3].ToString(); } set { this[3] = value; } }
        public string DiaIni { get { return this[4].ToString(); } set { this[4] = value; } }
        public int HoraDiaIni { get { return (int)this[5]; } set { this[5] = value; } }
        public int MeiaHoraIni { get { return (int)this[6]; } set { this[6] = value; } }
        public string DiaFinal { get { return this[7].ToString(); } set { this[7] = value; } }
        public int HoraDiaFinal { get { return (int)this[8]; } set { this[8] = value; } }
        public int MeiaHoraFinal { get { return (int)this[9]; } set { this[9] = value; } }
        public float ValorFixado { get { return (float)this[10]; } set { this[10] = value; } }

        //publics
        public override BaseField[] Campos { get { return PtoperCampos; } }

        static readonly BaseField[] PtoperCampos = new BaseField[] {
                new BaseField(1  , 6 ,"A6"    , "ID Ptoper"),//
                new BaseField(8  , 13 ,"A6"    , "Minemonico usina"),//
                new BaseField(15  , 17 ,"I3"    , "ID usina"),//
                new BaseField(19  , 24 ,"A6"    , "Variavel"),//
                new BaseField(26  , 27 ,"A2"    , "Dia inicial"),//será representado como string, pois pode ter "I" indicando primeiro dia
                new BaseField(29  , 30 ,"I2"    , "Hora dia inicial"),//
                new BaseField(32  , 32 ,"I1"    , "Flag Meia hora inicial"),//
                new BaseField(34  , 35 ,"A2"    , "Dia Final"),//será representado como string, pois pode ter "F" indicando ultimo dia
                new BaseField(37  , 38 ,"I1"    , "Hora dia Final"),//
                new BaseField(40  , 40 ,"I1"    , "Flag Meia Hora Final"),
                new BaseField(42  , 51 ,"F10.0"    , "Valor fixado"),
            };
    }
}

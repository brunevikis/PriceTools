using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Renovaveis
{
    public class EolicaGeracaoBlock : BaseBlock<EolicaGeracaoLine>
    {



    }

    public class EolicaGeracaoLine : BaseLine
    {
        public string Idbloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public string NumCodigo { get { return this[1].ToString(); } set { this[1] = value; } }
        public string DiaIni { get { return this[2].ToString(); } set { this[2] = value; } }
        public string HoraIni { get { return this[3].ToString(); } set { this[3] = value; } }
        public string MeiaHoraIni { get { return this[4].ToString(); } set { this[4] = value; } }
        public string DiaFim { get { return this[5].ToString(); } set { this[5] = value; } }
        public string HoraFim { get { return this[6].ToString(); } set { this[6] = value; } }
        public string MeiaHoraFim { get { return this[7].ToString(); } set { this[7] = value; } }
        public string Geracao { get { return this[8].ToString(); } set { this[8] = value; } }

        //publics
        public override BaseField[] Campos { get { return PtoperCampos; } }

        static readonly BaseField[] PtoperCampos = new BaseField[] {
                new BaseField(1  , 16 ,"A16"    , "ID"),//
                new BaseField(17  , 23 ,"A7"    , "numCodigo"),//
                new BaseField(24  , 27 ,"A4"    , "Diainicio"),//
                new BaseField(28  , 31 ,"A4"    , "HoraIni"),//
                new BaseField(32  , 34 ,"A3"    , "MeiaHoraIni"),//
                new BaseField(35  , 38 ,"A4"    , "DiaFim"),//
                new BaseField(39  , 42 ,"A4"    , "HoraFim"),//
                new BaseField(43  , 45 ,"A2"    , "MeiaHoraFim"),//
                new BaseField(46  , 57 ,"A12"    , "Geracao"),//
            };
    }
}

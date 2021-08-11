using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Renovaveis
{
    public class EolicaBlock : BaseBlock<EolicaLine>
    {



    }

    public class EolicaLine : BaseLine
    {
        public string Idbloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public string NumCodigo { get { return this[1].ToString(); } set { this[1] = value; } }
        public string Nome { get { return this[2].ToString(); } set { this[2] = value; } }
        public string PotMax { get { return this[3].ToString(); } set { this[3] = value; } }
        public string FatCap { get { return this[4].ToString(); } set { this[4] = value; } }
        public string FlagFuncao { get { return this[5].ToString(); } set { this[5] = value; } }

        //publics
        public override BaseField[] Campos { get { return PtoperCampos; } }

        static readonly BaseField[] PtoperCampos = new BaseField[] {
                new BaseField(1  , 8 ,"A8"    , "ID"),//
                new BaseField(9  , 15 ,"A7"    , "numCodigo"),//
                new BaseField(16  , 57 ,"A42"    , "nome"),//
                new BaseField(58  , 69 ,"A12"    , "PotMAX"),//
                new BaseField(70  , 74 ,"A5"    , "FatCap"),//
                new BaseField(75  , 76 ,"A2"    , "FlagFuncao"),//
            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class CrBlock : BaseBlock<CrLine>
    {




    }

    public class CrLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int SecaoRio { get { return (int)this[1]; } set { this[1] = value; } }
        public string NomeSecao { get { return this[2].ToString(); } set { this[2] = value; } }
        public int GrauPoli { get { return (int)this[3]; } set { this[3] = value; } }

        public override BaseField[] Campos { get { return CrCampos; } }

        static readonly BaseField[] CrCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 7 ,"I3"    , "SecaoRio"),//
                new BaseField(10  , 21 ,"A12"    , "NomeSecao"),//
                new BaseField(25  , 26 ,"I2"    , "GrauPoli"),//

                //new BaseField(28  , 42 ,"A15"    , "PoliIndep"),
                //new BaseField(44  , 58 ,"A15"    , "Poli1"),
                //new BaseField(60  , 74 ,"A15"    , "Poli2"),
                //new BaseField(76  , 90 ,"A15"    , "Poli3"),
                //new BaseField(92  , 106 ,"A15"    , "Poli4"),
                //new BaseField(108  , 122 ,"A15"    , "Poli5"),
                //new BaseField(124  , 138 ,"A15"    , "Poli6"),
                //
                new BaseField(29  , 43 ,"A15"    , "PoliIndep"),
                new BaseField(45  , 59 ,"A15"    , "Poli1"),
                new BaseField(61  , 75 ,"A15"    , "Poli2"),
                new BaseField(77  , 91 ,"A15"    , "Poli3"),
                new BaseField(93  , 107 ,"A15"    , "Poli4"),
                new BaseField(109  , 123 ,"A15"    , "Poli5"),
                new BaseField(125  , 139 ,"A15"    , "Poli6"),


            };
    }
}

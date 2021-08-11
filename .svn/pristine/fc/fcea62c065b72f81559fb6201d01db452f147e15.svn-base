using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Operut
{
    public class InitBlock : BaseBlock<InitLine>
    {

//        string header =
// @"INIT                                             
//&us     nome       ug   st   GerInic     tempo MH A/D
//&XX XXXXXXXXXXXX  XXX   XX   XXXXXXXXXX  XXXXX  X  X 
//";

//        public override string ToText()
//        {

//            return header + base.ToText() + "FIM\n";
//        }

    }

    public class InitLine : BaseLine
    {

        //public InitLine() : base() { this[0] = "GL"; }

        public int Usina { get { return (int)this[0]; } set { this[0] = value; } }

        public string NomeUsina { get { return this[1].ToString(); } set { this[1] = value; } }
        public int Indice { get { return (int)this[2]; } set { this[2] = value; } }
        public int Status { get { return (int)this[3]; } set { this[3] = value; } }

        public float Geracao { get { return (float)this[4]; } set { this[4] = value; } }
        public int Tempo { get { return (int)this[5]; } set { this[5] = value; } }
        public int MeiaHora { get { return (int)this[6]; } set { this[6] = value; } }
        public int AD { get { return (int)this[7]; } set { this[7] = value; } }


      

        public override BaseField[] Campos { get { return InitCampos; } }

        static readonly BaseField[] InitCampos = new BaseField[] {
                new BaseField(1  , 3 ,"I3"    , "Usina"),
                new BaseField(5  , 17 ,"A12"    , "Nome Usina"),
                new BaseField(19 , 21,"I3"    , "Indice"),
                new BaseField(25 , 26,"I2"    , "Status"),
                new BaseField(30 , 39,"F10.0" , "Geracao"),
                new BaseField(42 , 46,"I5"  , "Tempo"),
                new BaseField(49 , 49,"I1" , "MH"),
                new BaseField(52 , 52,"I1"  , "A/D"),
               
            };
    }
}

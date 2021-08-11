using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class UhBlock : BaseBlock<UhLine>
    {




    }

    public class UhLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public string NomeUsina { get { return this[2].ToString(); } set { this[2] = value; } }
        public int NumRee { get { return (int)this[3]; } set { this[3] = value; } }
        public float VolArm { get { return (float)this[4]; } set { this[4] = value; } }
        public int FlagEvap { get { return (int)this[5]; } set { this[5] = value; } }
        public string DiaInic { get { return this[6].ToString(); } set { this[6] = value; } }
        public int HoraInic { get { return (int)this[7]; } set { this[7] = value; } }
        public int MeiaHoraInic { get { return (int)this[8]; } set { this[8] = value; } }
        public float VolMorto { get { return (float)this[9]; } set { this[9] = value; } }
        public int FlagProd { get { return (int)this[10]; } set { this[10] = value; } }
        public int FlagRest { get { return (int)this[11]; } set { this[11] = value; } }

        public override BaseField[] Campos { get { return UhCampos; } }

        static readonly BaseField[] UhCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 7 ,"I3"    , "UsinaNum"),//
                new BaseField(10  , 21 ,"A12"    , "NomeUsina"),//
                new BaseField(25  , 26 ,"I2"    , "NumRee"),//
                new BaseField(30  , 39 ,"F10.0"    , "Volarm%"),//
                new BaseField(40  , 40 ,"I1"    , "FlagEvap"),//
                new BaseField(42  , 43 ,"A2"    , "dia inicial op"),//string pq pode ser letra
                new BaseField(45  , 46 ,"I2"    , "Hora inicial op"),//
                new BaseField(48  , 48 ,"I1"    , "Meia Hora inicial op"),//
                new BaseField(50  , 59 ,"F10.0"    , "VolMorto"),//
                new BaseField(65  , 65 ,"I1"    , "FlagProd"),//
                new BaseField(70  , 70 ,"I1"    , "FlagRest"),//


            };
    }
}

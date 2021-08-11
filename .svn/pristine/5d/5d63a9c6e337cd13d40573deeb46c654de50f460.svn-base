using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class PqBlock : BaseBlock<PqLine>
    {




    }

    public class PqLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public string NomeUsina { get { return this[2].ToString(); } set { this[2] = value; } }
        public int Sist_Barra { get { return (int)this[3]; } set { this[3] = value; } }
        public string DiaInic { get { return this[4].ToString(); } set { this[4] = value; } }
        public int HoraInic { get { return (int)this[5]; } set { this[5] = value; } }
        public int MeiaHoraInic { get { return (int)this[6]; } set { this[6] = value; } }
        public string DiaFinal { get { return this[7].ToString(); } set { this[7] = value; } }
        public int HoraDiaFinal { get { return (int)this[8]; } set { this[8] = value; } }
        public int MeiaHoraDiaFinal { get { return (int)this[9]; } set { this[9] = value; } }
        public float Geracao { get { return (float)this[10]; } set { this[10] = value; } }

        public override BaseField[] Campos { get { return PqCampos; } }

        static readonly BaseField[] PqCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 7 ,"I3"    , "Usina"),//
                new BaseField(10  , 19 ,"A10"    , "NomeUsina"),//
                new BaseField(20  , 24 ,"I5"    , "SubsistBarra"),//
                new BaseField(25  , 26 ,"A2"    , "DiaInic"),//
                new BaseField(28  , 29 ,"I2"    , "HoraDiaInic"),//
                new BaseField(31  , 31 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(33  , 34 ,"A2"    , "DiaFinal"),//
                new BaseField(36  , 37 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(39  , 39 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(41  , 50 ,"F10.0"    , "Geracao"),//


            };
    }
}

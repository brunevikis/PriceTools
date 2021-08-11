using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class IaBlock : BaseBlock<IaLine>
    {




    }

    public class IaLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public string SistemaA { get { return this[1].ToString(); } set { this[1] = value; } }
        public string SistemaB { get { return this[2].ToString(); } set { this[2] = value; } }
        public string DiaInic { get { return this[3].ToString(); } set { this[3] = value; } }
        public int Horainic { get { return (int)this[4]; } set { this[4] = value; } }
        public int MeiaHoraInic { get { return (int)this[5]; } set { this[5] = value; } }
        public string DiaFinal { get { return this[6].ToString(); } set { this[6] = value; } }
        public int HoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int MeiaHoraFinal { get { return (int)this[8]; } set { this[8] = value; } }
        public float IntercambioAB { get { return (float)this[9]; } set { this[9] = value; } }
        public float IntercambioBA { get { return (float)this[10]; } set { this[10] = value; } }


        public override BaseField[] Campos { get { return IaCampos; } }

        static readonly BaseField[] IaCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 6 ,"A2"    , "sistemaA"),//
                new BaseField(10  , 11 ,"A2"    , "sistemaB"),//
                new BaseField(14  , 15 ,"A2"    , "dia inicial"),//pode ser letra
                new BaseField(17  , 18 ,"I2"    , "Hora dia inicial"),//
                new BaseField(20  , 20 ,"I1"    , "Meia hora ini"),//
                new BaseField(22  , 23 ,"A2"    , "dia final"),//pode ser letra
                new BaseField(25  , 26 ,"I2"    , "Hora dia final"),//
                new BaseField(28  , 28 ,"I1"    , "meia hora final"),//
                new BaseField(30  , 39 ,"F10.0"    , "Intercambio AB"),//
                new BaseField(40  , 49 ,"F10.0"    , "Intercambio BA"),//


            };
    }
}

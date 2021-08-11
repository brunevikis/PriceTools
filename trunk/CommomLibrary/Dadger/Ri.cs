using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger
{
    public class RiBlock : BaseBlock<RiLine>
    {


    }

    public class RiLine : BaseLine
    {

        public RiLine()
            : base()
        {
            this[0] = "RI";
        }

        static readonly BaseField[] DpCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"   ,"Id" ),
                new BaseField( 5 , 7 ,"I3"   ,"Indice Itaipu" ),
                new BaseField( 9, 11 ,"I3"   ,"Estagio" ),
                new BaseField( 13, 15,"I3"   ,"Subsistema" ),
                new BaseField( 17, 23,"F7.0" ,"Ger Min 60 Pat1" ),
                new BaseField( 24, 30,"F7.0" ,"Ger Max 60 Pat1" ),
                new BaseField( 31, 37,"F7.0" ,"Ger Min 50 Pat1" ),
                new BaseField( 38, 44,"F7.0" ,"Ger Max 50 Pat1" ),
                new BaseField( 45, 51,"F7.0" ,"Ande Pat1" ),
                new BaseField( 52, 58,"F7.0" ,"Ger Min 60 Pat2" ),
                new BaseField( 59, 65,"F7.0" ,"Ger Max 60 Pat2" ),
                new BaseField( 66, 72,"F7.0" ,"Ger Min 50 Pat2" ),
                new BaseField( 73, 79,"F7.0" ,"Ger Max 50 Pat2" ),
                new BaseField( 80, 86,"F7.0" ,"Ande Pat2" ),
                new BaseField( 87, 93,"F7.0" ,"Ger Min 60 Pat3" ),
                new BaseField( 94, 100,"F7.0" ,"Ger Max 60 Pat3" ),
                new BaseField( 101, 107,"F7.0" ,"Ger Min 50 Pat3" ),
                new BaseField( 108, 114,"F7.0" ,"Ger Max 50 Pat3" ),
                new BaseField( 115, 121,"F7.0" ,"Ande Pat3" ),


            };

        public override BaseField[] Campos
        {
            get { return DpCampos; }
        }

        public int Estagio { get { return this["Estagio"]; } set { this["Estagio"] = value; } }
        public double AndePat1 { get { return Valores[8]; } set { Valores[8] = value; } }
        public double AndePat2 { get { return Valores[13]; } set { Valores[13] = value; } }
        public double AndePat3 { get { return Valores[18]; } set { Valores[18] = value; } }

        public double Ger_Min60_Pat1 { get { return Valores[4]; } set { Valores[4] = value; } }
        public double Ger_Max60_Pat1 { get { return Valores[5]; } set { Valores[5] = value; } }
        public double Ger_Min50_Pat1 { get { return Valores[6]; } set { Valores[6] = value; } }
        public double Ger_Max50_Pat1 { get { return Valores[7]; } set { Valores[7] = value; } }

        public double Ger_Min60_Pat2 { get { return Valores[9]; } set { Valores[9] = value; } }
        public double Ger_Max60_Pat2 { get { return Valores[10]; } set { Valores[10] = value; } }
        public double Ger_Min50_Pat2 { get { return Valores[11]; } set { Valores[11] = value; } }
        public double Ger_Max50_Pat2 { get { return Valores[12]; } set { Valores[12] = value; } }

        public double Ger_Min60_Pat3 { get { return Valores[14]; } set { Valores[14] = value; } }
        public double Ger_Max60_Pat3 { get { return Valores[15]; } set { Valores[15] = value; } }
        public double Ger_Min50_Pat3 { get { return Valores[16]; } set { Valores[16] = value; } }
        public double Ger_Max50_Pat3 { get { return Valores[17]; } set { Valores[17] = value; } }


    }
}

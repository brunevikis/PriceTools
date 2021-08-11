using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.Dadgnl {
    public class TgBlock : BaseBlock<TgLine> {        

    }

    public class TgLine : BaseLine {

        static readonly BaseField[] TgCampos = new BaseField[] {
                new BaseField(1 , 2 ,"A2"   ,"Id" ),
                new BaseField(5 , 7 ,"I3"   ,"Usina" ),
                new BaseField(10 ,11,"I2"   ,"Subsistema" ),
                new BaseField(15 ,24,"A10"  ,"Nome" ),
                new BaseField(25 ,26,"I2"   ,"Estagio" ),
                new BaseField(30 ,34,"F5.0" ,"Ger Min Pat1" ),
                new BaseField(35 ,39,"F5.0" ,"Capacidade Pat1" ),
                new BaseField(40 ,49,"F10.2","CVU Pat1" ),
                new BaseField(50 ,54,"F5.0" ,"Ger Min Pat2" ),
                new BaseField(55 ,59,"F5.0" ,"Capacidade Pat2" ),
                new BaseField(60 ,69,"F10.2","CVU Pat2" ),
                new BaseField(70 ,74,"F5.0" ,"Ger Min Pat3" ),
                new BaseField(75 ,79,"F5.0" ,"Capacidade Pat3" ),
                new BaseField(80 ,89,"F10.2","CVU Pat3" )
            };


        public override BaseField[] Campos {
            get { return TgCampos; }
        }

        public int Usina { get { return this[1]; } }
        public int Estagio { get { return this[4]; } set { this[4] = value; } }
        public double Cap_Pat1 { get { return this[6]; } set { this[6] = value; } }
        public double CVU_Pat1 { get { return this[7]; } set { this[7] = value; } }

        public double Cap_Pat2 { get { return this[9]; } set { this[9] = value; } }
        public double CVU_Pat2 { get { return this[10]; } set { this[10] = value; } }
        public double Cap_Pat3 { get { return this[12]; } set { this[12] = value; } }
        public double CVU_Pat3 { get { return this[13]; } set { this[13] = value; } }



    }

    
}

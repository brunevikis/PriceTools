using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class ItBlock : BaseBlock<ItLine> {


    }

    public class ItLine : BaseLine {

        public ItLine()
            : base()
        {
            this[0] = "IT";
        }

        static readonly BaseField[] DpCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"   ,"Id" ),
                new BaseField( 5 , 6 ,"I2"   ,"Estagio" ),                
                new BaseField( 10, 12 ,"I3"   ,"Cod Itaipu" ),
                new BaseField( 15, 16,"I2"   ,"Subsistema" ),                                
                new BaseField( 20, 24,"F5.0" ,"Geracao Pat1" ),
                new BaseField( 25, 29,"F5.0" ,"Ande Pat1" ),                
                new BaseField( 30, 34,"F5.0" ,"Geracao Pat2" ),
                new BaseField( 35, 39,"F5.0" ,"Ande Pat2" ),                
                new BaseField( 40, 44,"F5.0" ,"Geracao Pat3" ),
                new BaseField( 45, 49,"F5.0" ,"Ande Pat3" ),
                
            };

        public override BaseField[] Campos {
            get { return DpCampos; }
        }

        public int Estagio { get { return this["Estagio"]; } set { this["Estagio"] = value; } }
        public double AndePat1 { get { return Valores[5]; } set { Valores[5] = value; } }
        public double AndePat2 { get { return Valores[7]; } set { Valores[7] = value; } }
        public double AndePat3 { get { return Valores[9]; } set { Valores[9] = value; } }
        public double Geracao_Pat1 { get { return Valores[4]; } set { Valores[4] = value; } }
        public double Geracao_Pat2 { get { return Valores[6]; } set { Valores[6] = value; } }
        public double Geracao_Pat3 { get { return Valores[8]; } set { Valores[8] = value; } }


    }    
}

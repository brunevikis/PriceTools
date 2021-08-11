using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class IaBlock : BaseBlock<IaLine> {


    }

    public class IaLine : BaseLine {

        public IaLine()
            : base()
        {
            this[0] = "IA";
        }

        static readonly BaseField[] DpCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"   ,"Id" ),
                new BaseField( 5 , 6 ,"I2"   ,"Estagio" ),                
                new BaseField( 10, 11 ,"A2"   ,"Subsistema 1" ),
                new BaseField( 15, 16,"A2"   ,"Subsistema 2"  ),                                
                new BaseField( 18 ,18 ,"I1"   ,"Penalidade" ), 
                new BaseField( 20, 29,"F10.0" ,"1-2 Pat1" ),
                new BaseField( 30, 39,"F10.0" ,"2-1 Pat1" ),                
                new BaseField( 40, 49,"F10.0" ,"1-2 Pat2" ),
                new BaseField( 50, 59,"F10.0" ,"2-1 Pat2" ),                
                new BaseField( 60, 69,"F10.0" ,"1-2 Pat3" ),
                new BaseField( 70, 79,"F10.0" ,"2-1 Pat3" ),                
            };

        public override BaseField[] Campos {
            get { return DpCampos; }
        }

        public int Estagio { get { return this[1]; } set { this[1] = value; } }
        public string SistemaA { get { return this[2].Trim(); } set { this[2] = value; } }
        public string SistemaB { get { return this[3].Trim(); } set { this[3] = value; } }
        public double Pat1_AB { get { return this[5]; } set { this[5] = value; } }
        public double Pat1_BA { get { return this[6]; } set { this[6] = value; } }
        public double Pat2_AB { get { return this[7]; } set { this[7] = value; } }
        public double Pat2_BA { get { return this[8]; } set { this[8] = value; } }
        public double Pat3_AB { get { return this[9]; } set { this[9] = value; } }
        public double Pat3_BA { get { return this[10]; } set { this[10] = value; } }

    }    
}

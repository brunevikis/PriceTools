using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger
{

    public class CtBlock : BaseBlock<CtLine>
    {

        //public override void RVX() {
        //    for (int x = 0; x < this.Count; x++) {
        //        if ((int)this[x][4] == 1) {
        //            if (x + 1 < this.Count && (int)this[x + 1][4] == 2) {
        //                this.Remove(this[x]);
        //                x--;
        //            }
        //        } else {
        //            this[x][4] = (int)this[x][4] - 1;
        //        }
        //    }
        //}
    }

    public class CtLine : BaseLine
    {

        public CtLine()
            : base()
        {
            this[0] = "CT";
        }

        static readonly BaseField[] CtCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"   ,"Id" ),
                new BaseField( 5 , 7 ,"I3"   ,"Usina" ),
                new BaseField( 10 ,11,"I2"   ,"Subsistema" ),
                new BaseField( 15 ,24,"A10"  ,"Nome" ),
                new BaseField( 25 ,26,"I2"   ,"Estagio" ),
                new BaseField( 30 ,34,"f5.2" ,"Ger Min Pat1" ),
                new BaseField( 35 ,39,"f5.2" ,"Capacidade Pat1" ),
                new BaseField( 40 ,49,"F10.2","CVU Pat1" ),
                new BaseField( 50 ,54,"f5.2" ,"Ger Min Pat2" ),
                new BaseField( 55 ,59,"f5.2" ,"Capacidade Pat2" ),
                new BaseField( 60 ,69,"F10.2","CVU Pat2" ),
                new BaseField( 70 ,74,"f5.2" ,"Ger Min Pat3" ),
                new BaseField( 75 ,79,"f5.2" ,"Capacidade Pat3" ),
                new BaseField( 80 ,89,"F10.2","CVU Pat3" )
            };

        public override BaseField[] Campos
        {
            get { return CtCampos; }
        }

        public int Subsistema { get { return Valores[2]; } set { Valores[2] = value; } }
        public int Estagio { get { return Valores[4]; } set { Valores[4] = value; } }
        public int Cod { get { return Valores[1]; } set { this[1] = value; } }

        public double Cvu { get { return Valores[7]; }}
        public double Cvu1 { get { return Valores[7]; } set { this[7] = value; } }
        public double Cvu2 { get { return Valores[10]; } set { this[10] = value; } }
        public double Cvu3 { get { return Valores[13]; } set { this[13] = value; } }

    }
}

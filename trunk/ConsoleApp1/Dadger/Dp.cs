using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Dadger
{
    public class DpBlock : BaseBlock<DpLine>
    {


    }

    public class DpLine : BaseLine
    {

        public DpLine() : base()
        {
            this[0] = "DP";
        }

        static readonly BaseField[] DpCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"   ,"Id" ),
                new BaseField( 5 , 6 ,"I2"   ,"Estagio" ),
                new BaseField( 10 ,11,"I2"   ,"Subsistema" ),
                new BaseField( 15 ,15,"I1"  ,"Patamares" ),
                new BaseField( 20 ,29,"F10.1" ,"Carga Pat1" ),
                new BaseField( 30 ,39,"F10.1" ,"Duracao Pat1" ),
                new BaseField( 40 ,49,"F10.1" ,"Carga Pat2" ),
                new BaseField( 50 ,59,"F10.1" ,"Duracao Pat2" ),
                new BaseField( 60 ,69,"F10.1" ,"Carga Pat3" ),
                new BaseField( 70 ,79,"F10.1" ,"Duracao Pat3" ),

            };

        public override BaseField[] Campos
        {
            get { return DpCampos; }
        }

        public int Estagio { get { return (int)this[1]; } set { this[1] = value; } }
        public double Dura1 { get { return (double)this[5]; } set { this[5] = value; } }
        public double Dura2 { get { return (double)this[7]; } set { this[7] = value; } }
        public double Dura3 { get { return (double)this[9]; } set { this[9] = value; } }

    }

}

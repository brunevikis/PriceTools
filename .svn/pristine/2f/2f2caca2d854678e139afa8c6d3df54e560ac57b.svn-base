using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class DtBlock : BaseBlock<DtLine> {

        public DateTime DataEstudo {
            get {

                var l = this.FirstOrDefault();

                if (l != null && l[1] is int && l[2] is int && l[3] is int) return new DateTime((int)l[3], (int)l[2], (int)l[1]);
                else return new DateTime();

            }

            set {
                var l = this.FirstOrDefault();
                if (l != null) {
                    l[1] = value.Day;
                    l[2] = value.Month;
                    l[3] = value.Year;
                }
            }

        }

    }

    public class DtLine : BaseLine {
        public static readonly BaseField[] StaCampos = new BaseField[] {
                new BaseField( 1 , 2 ,"A2"  , "Id"),
                new BaseField( 5 , 6 ,"I2", "Dia"  ),
                new BaseField( 10 , 11 ,"I2", "Mes"  ),
                new BaseField( 15 , 18 ,"I4", "Ano"  ),

            };

        public override BaseField[] Campos {
            get { return StaCampos; }
        }
    }


}

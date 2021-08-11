using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DgerNwd {
    public class EafPrev : BaseBlock<EafLine> {

        string header =
@"energias afluentes previstas     (REFERENTES A 65% DE VOLUME ARMAZENADO)
mes xxsis1.xxx xxsis2.xxx xxsis3.xxx xxsis4.xxx xxsis5.xxx xxsis4.xxx xxsis5.xxx xxsis4.xxx xxsis5.xx
";


        public override string ToText() {

            return header + base.ToText();
        }

        public int Mes {
            get {
                return this.First()[0];
            }

            set { this.First()[0] = value; }
        }

        /// <summary>
        /// mes de 1 a 12
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public new double this[int i] {
            get {
                return this.First()[i];
            }

            set { this.First()[i] = value; }
        }

    }
}


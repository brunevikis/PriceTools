using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Pmo {
    public class DeficitBlock : BaseBlock<DeficitLine> {

        internal void Load(System.IO.StreamReader tr) {

            tr.ReadLine(); tr.ReadLine();
            var line = tr.ReadLine();


            while (!line.Contains("X--------")) {
                this.Add(
                    this.CreateLine(line)
                );
                line = tr.ReadLine();
            }
        }
    }

    public class DeficitLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(4 , 13 ,"A10"  , "MERCADO"),                                
           new BaseField(15 ,23 ,"F7.2"  , "Custo_P1"),
           new BaseField(25 ,33 ,"F7.2"  , "Custo_P2"),
           new BaseField(35, 43 ,"F7.2"  , "Custo_P3"),
           new BaseField(45, 53 ,"F7.2"  , "Custo_P4"),
           new BaseField(60 ,64 ,"F4.3"  , "PU_P1"),
           new BaseField(67 ,71 ,"F4.3"  , "PU_P2"),
           new BaseField(74, 78 ,"F4.3"  , "PU_P3"),
           new BaseField(81, 85 ,"F4.3"  , "PU_P4"),
           

        };

        public override BaseField[] Campos {
            get { return campos; }
        }



    }
}

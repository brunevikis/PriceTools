using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Pmo {
    public class MecadoBlock : BaseBlock<MecadoLine> {

        internal void Load(System.IO.StreamReader tr) {

            tr.ReadLine(); tr.ReadLine();
            var line = tr.ReadLine();

            var mercado = "";
            var pat = 0;
            var started = false;
            while (!tr.EndOfStream){

                if (line.Contains("SUBSISTEMA:")) {
                    mercado = line.Split(':')[1].Trim();
                } else if (line.Contains("X-------")){
                    if (started && mercado == "NORTE") break;
                    
                    started = !started;
                    pat = 0;
                    

                } else if (line.Contains("PATAMAR:")) {
                    pat = int.Parse(line.Split(':')[1].Trim());
                }
                else if (started) {
                    var reg = this.CreateLine(line);
                    reg[0] = mercado;
                    reg[1] = pat;
                    this.Add(reg);
                }
                line = tr.ReadLine();
            }
            
        }
    }

    public class MecadoLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(0 , 0 ,"A10" , "MERCADO"),                                
           new BaseField(0 , 0 ,"I1"  , "PATAMAR"),                                
           new BaseField(3 , 6 ,"I4"  , "ANO"),
           new BaseField(10 ,16 ,"F7.2"  , "1"),
           new BaseField(18 ,24 ,"F7.2"  , "2"),
           new BaseField(26, 32 ,"F7.2"  , "3"),
           new BaseField(34, 40 ,"F7.2"  , "4"),
           new BaseField(42 ,48 ,"F7.2"  , "5"),
           new BaseField(50 ,56 ,"F7.2"  , "6"),
           new BaseField(58, 64 ,"F7.2"  , "7"),
           new BaseField(66, 72 ,"F7.2"  , "8"),
           new BaseField(74 ,80 ,"F7.2"  , "9"),
           new BaseField(82 ,88 ,"F7.2"  , "10"),
           new BaseField(90, 96 ,"F7.2"  , "11"),
           new BaseField(98, 104 ,"F7.2"  , "12"),

           

        };

        public override BaseField[] Campos {
            get { return campos; }
        }



    }
}

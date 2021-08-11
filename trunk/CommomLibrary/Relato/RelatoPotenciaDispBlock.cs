using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Relato {
    public class RelatoPotenciaDispBlock : BaseBlock<RelatoPotenciaDispLine> {
        internal void Load(string fileContent) {
            var i = fileContent.IndexOf("Relatorio  de  Potencia Disponivel");
            if (i < 0) return;
            var lines = fileContent.Remove(0, i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(4).ToArray();

            foreach (var line in lines) {

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                } else {
                    var newLine = this.CreateLine(line);
                    if (newLine.Cod > 0) this.Add(newLine);
                }
            }
        }
    }

    public class RelatoPotenciaDispLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField( 5 , 7 ,"I3"  , "Cod"),
                new BaseField( 9 , 20 ,"A12"  , "Usina"),                
                new BaseField(22  , 28 ,"F6.1"  , "Estagio1"),
                new BaseField(30  , 36 ,"F6.1"  , "Estagio2"),
                new BaseField(38  , 44 ,"F6.1"  , "Estagio3"),
                new BaseField(46  , 52 ,"F6.1"  , "Estagio4"),
                new BaseField(54  , 60 ,"F6.1"  , "Estagio5"),
                new BaseField(62  , 68 ,"F6.1"  , "Estagio6"),                

        };

        public override BaseField[] Campos {
            get { return campos; }
        }


        public int Cod {
            get {
                return
                    Valores[0] != null ? (int)Valores[0] : 0;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Relato {
    public class RelatoVolUtilBlock : BaseBlock<RelatoVolUtilLine> {
        internal void Load(string fileContent) {
            var i = fileContent.IndexOf("VOLUME UTIL DOS RESERVATORIOS");
            if (i < 0) return;
            var lines = fileContent.Remove(0, i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(4).ToArray();
            var read = false;
            foreach (var line in lines) {

                if (line.Contains("X----X------------X-------X")) {
                    if (read) break;
                    read = true;
                } else if (string.IsNullOrWhiteSpace(line)) {
                    read = false;
                } else if (read) {

                    var newLine = this.CreateLine(line);
                    this.Add(newLine);
                }
            }
        }
    }

    public class RelatoVolUtilLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField( 5 , 8 ,"I4"  , "Cod"),
                new BaseField( 10 , 21 ,"A12"  , "Usina"),
                new BaseField(23  , 29 ,"F6.1"  , "VolIni"),
                new BaseField(31  , 36 ,"F5.1"  , "VolFinSem1"),
                new BaseField(38  , 43 ,"F5.1"  , "VolFinSem2"),
                new BaseField(45  , 50 ,"F5.1"  , "VolFinSem3"),
                new BaseField(52  , 57 ,"F5.1"  , "VolFinSem4"),
                new BaseField(59  , 64 ,"F5.1"  , "VolFinSem5"),
                new BaseField(66  , 71 ,"F5.1"  , "VolFinSem6"),                

        };

        public override BaseField[] Campos {
            get { return campos; }
        }


        public int Cod { get { return (int)Valores[0]; } }

        public double VolFinSem1 { get { return (double)Valores[3]; } }

    }
}

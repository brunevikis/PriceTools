using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Relato {
    public class RelatoTHMenBlock : BaseBlock<RelatoTHMenLine> {
        internal void Load(string fileContent) {
            var i = fileContent.IndexOf("RELATORIO DOS DADOS DE ENERGIA NATURAL AFLUENTE POR SUBSISTEMA (MESES PRE-ESTUDO)");
            if (i < 0) return;
            var lines = fileContent.Remove(0, i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(4).Take(5).ToArray();
            foreach (var line in lines) {

                if (line.Contains("X----")) {
                    continue;
                } else {
                    var newLine = this.CreateLine(line);
                    this.Add(newLine);
                }
            }
        }
    }

    public class RelatoTHMenLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(5   , 18 ,"A12"  , "Subsistema"),
                new BaseField(20  , 23 ,"I4"  , "Num"),
                new BaseField(25  , 32 ,"A12"  , "EarmMax"),                
                new BaseField(34  , 41 ,"F6.1"  , "ENA_1"),
                new BaseField(43  , 50 ,"F6.1"  , "ENA_2"),
                new BaseField(52  , 59 ,"F6.1"  , "ENA_3"),
                new BaseField(61  , 68 ,"F6.1"  , "ENA_4"),
                new BaseField(70  , 77 ,"F6.1"  , "ENA_5"),
                new BaseField(79  , 86 ,"F6.1"  , "ENA_6"),                
                new BaseField(88  , 95 ,"F6.1"  , "ENA_7"),
                new BaseField(97  ,104 ,"F6.1"  , "ENA_8"),
                new BaseField(106 ,113 ,"F6.1"  , "ENA_9"),
                new BaseField(115 ,122 ,"F6.1"  , "ENA_10"),
                new BaseField(124 ,131 ,"F6.1"  , "ENA_11"),                            

        };

        public override BaseField[] Campos {
            get { return campos; }
        } 

    }
}

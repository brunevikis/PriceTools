using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Relato {
    public class RelatoOpsBlock : BaseBlock<RelatoOpsLine> {
        internal void Load(string fileContent) {

            int i = fileContent.IndexOf("RELATORIO  DA  OPERACAO");

            if (i<0) {
                return;
            }

            var lines = fileContent.Remove(0, i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            bool read = false;
            bool readNewDecomp = false;
            foreach (var line in lines) {

                if (line.Contains("   X-----------------X-----X-----X-----X----------------X--------X--------X-------X-------X-------X-------X-------X-------X-------X-------X")) {
                    if (read) break;
                    read = true;
                    continue;
                } if (line.Contains("   X----X-----------------X-----X-----X-----X----------------X--------X--------X-------X-------X-------X-------X-------X-------X-------X-------X")) {
                    if (readNewDecomp) break;
                    readNewDecomp = true;
                    continue;
                } 
                
                if (string.IsNullOrWhiteSpace(line)) {
                    read = false;
                } else if (read) {

                    var newLine = this.CreateLine(line);
                    this.Add(newLine);
                } else if (readNewDecomp) {
                    var newLine = new RelatoOpsNewLine();
                    newLine.Load(line);
                    this.Add(newLine);
                }
            }
        }
    }


    public class RelatoOpsLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 5 , 16 ,"A12"  , "Usina"),
                new BaseField( 17 , 21 ,"A5"  , "Adornos"),
                new BaseField(23  , 27 ,"F4.1"  , "VolIni"),
                new BaseField(29  , 33 ,"F4.1"  , "VolFin"),
                new BaseField(35  , 39 ,"F4.1"  , "VolEsp"),
                new BaseField(41  , 47 ,"F6.1"  , "Qnat"),
                new BaseField(50  , 55 ,"F5.1"  , "QnatMlt"),
                new BaseField(58  , 65 ,"F7.1"  , "Qafl"),
                new BaseField(67  , 74 ,"F7.1"  , "Qdef"),
                new BaseField(76  , 82,"F6.1"  , "GER_1"),
                new BaseField(84  , 90,"F6.1"  , "GER_2"),
                new BaseField(92  , 98,"F6.1"  , "GER_3"),
                new BaseField(100 , 106,"F6.1"  , "Media"),
                new BaseField(108 , 114,"F6.1"  , "VT"),
                new BaseField(116 , 122,"F6.1"  , "VNT"),
                new BaseField(124 , 130,"F6.1"  , "Ponta"),
                new BaseField(132 , 138,"F6.1"  , "FPCGC"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class RelatoOpsNewLine : RelatoOpsLine {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField(5 , 7,"I3"  , "Cod"),
                new BaseField(10 , 21,"A12"  , "Usina"),
                new BaseField(22 , 26,"A5"  , "Adornos"),
                new BaseField(28 , 32,"F4.1"  , "VolIni"),
                new BaseField(34 , 38,"F4.1"  , "VolFin"),
                new BaseField(40 , 44,"F4.1"  , "VolEsp"),
                new BaseField(46 , 52,"F6.1"  , "Qnat"),
                new BaseField(55 , 60,"F5.1"  , "QnatMlt"),
                new BaseField(63 , 70,"F7.1"  , "Qafl"),
                new BaseField(72 , 79,"F7.1"  , "Qdef"),
                new BaseField(81 , 87,"F6.1"  , "GER_1"),
                new BaseField(89 , 95,"F6.1"  , "GER_2"),
                new BaseField(97 , 103,"F6.1"  , "GER_3"),
                new BaseField(105 ,111 ,"F6.1"  , "Media"),
                new BaseField(113 ,119 ,"F6.1"  , "VT"),
                new BaseField(121 ,127 ,"F6.1"  , "VNT"),
                new BaseField(129 ,135 ,"F6.1"  , "Ponta"),
                new BaseField(137 ,143 ,"F6.1"  , "FPCGC"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

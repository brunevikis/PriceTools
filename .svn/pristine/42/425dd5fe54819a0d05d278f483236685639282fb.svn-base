using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Relato {
    public class RelatoOpsTermBlock : BaseBlock<RelatoOpsTermLine> {
        internal void Load(string fileContent) {
            var i = fileContent.IndexOf("RELATORIO  DA  OPERACAO  TERMICA E CONTRATOS");

            if (i < 0) {
                return;
            }

            var lines = fileContent.Remove(0, i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(4).ToArray();
            var read = false;
            foreach (var line in lines) {

                if (line.Contains("X---X-----------X-------X-----------X-----------X-----------X-----------X")) {
                    read = !read;
                } else if (read) {
                    var newLine = this.CreateLine(line);
                    if (string.IsNullOrWhiteSpace(newLine[0])) {
                        break;
                    } else {
                        this.Add(newLine);
                    }
                }
            }
        }
    }


    public class RelatoOpsTermLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 5 , 6 ,"A2"    , "Sis"  ),
                new BaseField( 9 , 18 ,"A10"  , "Usina"),
                new BaseField(0 , 0 ,"I1"    , "Estagio"), 
                new BaseField(21 , 27,"F6.1"  , "FPCGC"),
                new BaseField(29 , 38,"F9.2"  , "GER_1"),
                new BaseField(41 , 50,"F9.2"  , "GER_2"),
                new BaseField(53 , 62,"F9.2"  , "GER_3"),
                new BaseField(65 , 75,"F10.2" , "Custo"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class RelatoDadosTermBlock : BaseBlock<RelatoDadosTermLine> { 
    }
    public class RelatoDadosTermLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 5 , 7  ,"I3"    , "Num"  ),
                new BaseField( 9 , 18 ,"A10"   , "Usina"),
                new BaseField(20 , 25 ,"A6"    , "Sistema"),
                new BaseField(27 , 33 ,"I1"    , "Estagio"),                
                new BaseField(35 , 41 ,"F5.2"  , "GTmin_1"),
                new BaseField(43 , 49 ,"F5.2"  , "GTmax_1"),
                new BaseField(51 , 57 ,"F5.2"  , "Custo_1"),
                new BaseField(59 , 65 ,"F5.2"  , "GTmin_2"),
                new BaseField(67 , 73 ,"F5.2"  , "GTmax_2"),
                new BaseField(75 , 81 ,"F5.2"  , "Custo_2"),
                new BaseField(83 , 89 ,"F5.2"  , "GTmin_3"),
                new BaseField(91 , 97 ,"F5.2"  , "GTmax_3"),
                new BaseField(99 , 105,"F5.2"  , "Custo_3"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

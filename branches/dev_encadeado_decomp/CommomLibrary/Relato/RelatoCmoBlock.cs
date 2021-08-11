using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Relato {
    public class RelatoCmoBlock : BaseBlock<RelatoCmoLine> {

        public RelatoCmoLine this[string subsistema] {
            get {
                return this.FirstOrDefault(x => x.Valores[0] == subsistema);
            }
        }

        internal void Load(string fileContent) {
            var cmoPat = @"Custo marginal de operacao do subsistema (\w{1,2})\s?:\s+(\d*,?\d{1,3}\.\d{2})";

            foreach (Match match in Regex.Matches(fileContent, cmoPat)) {

                var line = this[match.Groups[1].Value];

                if (line == null) {
                    line = this.CreateLine();
                    line.SetValue(0, match.Groups[1].Value);
                    this.Add(line);
                }

                for (int sem = 1; sem <= 5; sem++) {
                    if (line[sem] == null) {

                        line.SetValue(sem, match.Groups[2].Value);
                        break;
                    }
                }
            }
        }
    }

    public class RelatoCmoLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(5 , 8 ,"A2"  , "Subsistema"),                                
           new BaseField(31  , 36 ,"F5.2"  , "Sem 1"),
           new BaseField(38  , 43 ,"F5.2"  , "Sem 2"),
           new BaseField(45  , 50 ,"F5.2"  , "Sem 3"),
           new BaseField(52  , 57 ,"F5.2"  , "Sem 4"),
           new BaseField(59  , 64 ,"F5.2"  , "Sem 5"),            

        };

        public override BaseField[] Campos {
            get { return campos; }
        }

    }
}

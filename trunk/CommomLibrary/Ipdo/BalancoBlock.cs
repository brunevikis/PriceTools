using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Ipdo {
    public class BalancoBlock : BaseBlock<BalancoLine> {

        internal void Load(string fileContent) {

            var balancoLine = new BalancoLine();

            var inicioIndex = fileContent.IndexOf("Balanço de Energia");
            var fimIndex = fileContent.IndexOf("Produção e Carga");

            var text = fileContent.Substring(inicioIndex, fimIndex - inicioIndex);

            var finfo = System.Globalization.CultureInfo.GetCultureInfo("pt-br");

            var lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines) {
                var splitedLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (splitedLine.Length < 3) continue;

                if (splitedLine[0].Equals("Hidro", StringComparison.OrdinalIgnoreCase)) {
                    balancoLine.SetValue(0, float.Parse(splitedLine[2], finfo));
                    balancoLine.SetValue(1, float.Parse(splitedLine[3], finfo));
                    balancoLine.SetValue(2, float.Parse(splitedLine[4].Replace("%", ""), finfo));
                } else if (splitedLine[0].Equals("Itaipu", StringComparison.OrdinalIgnoreCase)) {
                    balancoLine.SetValue(3, float.Parse(splitedLine[2], finfo));
                    balancoLine.SetValue(4, float.Parse(splitedLine[3], finfo));
                    balancoLine.SetValue(5, float.Parse(splitedLine[4].Replace("%", ""), finfo));
                } else if (splitedLine[0].Equals("Termo", StringComparison.OrdinalIgnoreCase) &&
                    splitedLine[1].Equals("Nuclear", StringComparison.OrdinalIgnoreCase)
                    ) {
                    balancoLine.SetValue(6, float.Parse(splitedLine[2], finfo));
                    balancoLine.SetValue(7, float.Parse(splitedLine[3], finfo));
                    balancoLine.SetValue(8, float.Parse(splitedLine[4].Replace("%", ""), finfo));
                } else if (splitedLine[0].Equals("TermoNuclear", StringComparison.OrdinalIgnoreCase)) {
                    balancoLine.SetValue(6, float.Parse(splitedLine[1], finfo));
                    balancoLine.SetValue(7, float.Parse(splitedLine[2], finfo));
                    balancoLine.SetValue(8, float.Parse(splitedLine[3].Replace("%", ""), finfo));
                } else if (splitedLine[0].Equals("Termo", StringComparison.OrdinalIgnoreCase) &&
                      splitedLine[1].Equals("Convencional", StringComparison.OrdinalIgnoreCase)
                      ) {
                    balancoLine.SetValue(9, float.Parse(splitedLine[2], finfo));
                    balancoLine.SetValue(10, float.Parse(splitedLine[3], finfo));
                    balancoLine.SetValue(11, float.Parse(splitedLine[4].Replace("%", ""), finfo));
                } else if (splitedLine[0].Equals("Eólica", StringComparison.OrdinalIgnoreCase)) {
                    balancoLine.SetValue(12, float.Parse(splitedLine[1], finfo));
                    balancoLine.SetValue(13, float.Parse(splitedLine[2], finfo));
                    balancoLine.SetValue(14, float.Parse(splitedLine[3].Replace("%", ""), finfo));
                } else if (splitedLine[0].Equals("Intercâmbio", StringComparison.OrdinalIgnoreCase)) {
                    balancoLine.SetValue(15, float.Parse(splitedLine[2], finfo));
                    balancoLine.SetValue(16, float.Parse(splitedLine[3], finfo));
                } else if (splitedLine[0].Equals("Carga", StringComparison.OrdinalIgnoreCase)) {
                    balancoLine.SetValue(17, float.Parse(splitedLine[2], finfo));
                    balancoLine.SetValue(18, float.Parse(splitedLine[3], finfo));
                } else if (splitedLine[0].Equals("Carga(*)", StringComparison.OrdinalIgnoreCase)) {
                    balancoLine.SetValue(17, float.Parse(splitedLine[1], finfo));
                    balancoLine.SetValue(18, float.Parse(splitedLine[2], finfo));
                }


            }

            this.Add(balancoLine);


            //var cmoPat = @"Custo marginal de operacao do subsistema (\w{1,2})\s?:\s+(\d*,?\d{1,3}\.\d{2})";

            //foreach (Match match in Regex.Matches(fileContent, cmoPat)) {

            //    var line = this[match.Groups[1].Value];

            //    if (line == null) {
            //        line = this.CreateLine();
            //        line.SetValue(0, match.Groups[1].Value);
            //        this.Add(line);
            //    }

            //    for (int sem = 1; sem <= 5; sem++) {
            //        if (line[sem] == null) {

            //            line.SetValue(sem, match.Groups[2].Value);
            //            break;
            //        }
            //    }
            //}
        }
    }

    public class BalancoLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {                               
           new BaseField(0, 0 ,"F9.0"  , "Hidro Nacional Programada"),
           new BaseField(0, 0 ,"F9.0"  , "Hidro Nacional Verificada"),
           new BaseField(0, 0 ,"F5.2"  , "Hidro Nacional Participacao"),
           new BaseField(0, 0 ,"F9.0"  , "Itaipu Binacional Programada"),
           new BaseField(0, 0 ,"F9.0"  , "Itaipu Binacional Verificada"),
           new BaseField(0, 0 ,"F5.2"  , "Itaipu Binacional Participacao"),
           new BaseField(0, 0 ,"F9.0"  , "Termo Nuclear Programada"),
           new BaseField(0, 0 ,"F9.0"  , "Termo Nuclear Verificada"),
           new BaseField(0, 0 ,"F5.2"  , "Termo Nuclear Participacao"),
           new BaseField(0, 0 ,"F9.0"  , "Termo Convencional Programada"),
           new BaseField(0, 0 ,"F9.0"  , "Termo Convencional Verificada"),
           new BaseField(0, 0 ,"F5.2"  , "Termo Convencional Participacao"),
           new BaseField(0, 0 ,"F9.0"  , "Eolica Programada"),
           new BaseField(0, 0 ,"F9.0"  , "Eolica Verificada"),
           new BaseField(0, 0 ,"F5.2"  , "Eolica Participacao"),
           new BaseField(0, 0 ,"F9.0"  , "Intercambio Internacional Programada"),
           new BaseField(0, 0 ,"F9.0"  , "Intercambio Internacional Verificada"),
           new BaseField(0, 0 ,"F9.0"  , "Carga Programada"),
           new BaseField(0, 0 ,"F9.0"  , "Carga Verificada"),
           
           

        };

        public override BaseField[] Campos {
            get { return campos; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Ipdo {
    public class GerTermicaBlock : BaseBlock<GerTermicaLine> {

        internal void Load(string fileContent) {
            LoadTipoI(fileContent);
            LoadTipoII(fileContent);
        }

        internal void LoadTipoI(string fileContent) {


            var inicioIndex = fileContent.IndexOf("Valores de Média Diária das Usinas Térmicas Tipo I");
            var fimIndex = fileContent.IndexOf("Valores de Média Diária das Usinas Térmicas Tipo II-A");

            var text = fileContent.Substring(inicioIndex, fimIndex - inicioIndex);

            var finfo = System.Globalization.CultureInfo.GetCultureInfo("pt-br");

            var lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            var mercado = "";

            foreach (var line in lines) {

                if (line.Replace(" ", "").StartsWith("SUDESTE")) {
                    mercado = "SE";
                } else if (line.Replace(" ", "").StartsWith("SUL")) {
                    mercado = "S";
                } else if (line.Replace(" ", "").StartsWith("NORDESTE")) {
                    mercado = "NE";
                } else if (line.Replace(" ", "").StartsWith("NORTE")) {
                    mercado = "N";
                } else if (line.Contains("TOTAL")) {
                    mercado = "";
                } else if (!string.IsNullOrWhiteSpace(line) && mercado != "") {

                    var termLine = new GerTermicaTipoILine();
                    //termLine.Load(line.Trim());

                    //if ((termLine.Valores[4] == null || termLine.Valores[5] == null) && !this.Any(x => x.Valores[1].Trim() == termLine[1].Trim())) {
                    //continue;
                    //}



                    var tl = line.Trim().PadRight(100);

                    var usina = tl.Replace("   ", "      ").Substring(0, 20).Replace("*", " ").Replace("(*)", " ").Trim();
                    if (string.IsNullOrWhiteSpace(usina)) continue;

                    var tlArr = tl.Substring(20).Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries);

                    if (tlArr.Length < 7) continue;


                    termLine.SetValue(0, usina);

                    termLine.SetValue(1, tlArr[0].Trim());
                    termLine.SetValue(2, tlArr[1].Trim());
                    termLine.SetValue(3, tlArr[2].Trim());
                    termLine.SetValue(4, tlArr[3].Trim());
                    termLine.SetValue(5, tlArr[4].Trim());


                    termLine.SetValue(6, mercado);
                    termLine.SetValue(7, "I");

                    this.Add(termLine);
                }
            }
        }

        internal void LoadTipoII(string fileContent) {


            var inicioIndex = fileContent.IndexOf("Valores de Média Diária das Usinas Térmicas Tipo II-A");
            var fimIndex = fileContent.IndexOf("Usinas com mais de uma razão de despacho (Tipo I e II-A)");

            var text = fileContent.Substring(inicioIndex, fimIndex - inicioIndex);

            var finfo = System.Globalization.CultureInfo.GetCultureInfo("pt-br");

            var lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            var mercado = "";

            foreach (var line in lines) {

                if (line.Replace(" ", "").StartsWith("SUDESTE")) {
                    mercado = "SE";
                } else if (line.Replace(" ", "").StartsWith("SUL")) {
                    mercado = "S";
                } else if (line.Replace(" ", "").StartsWith("NORDESTE")) {
                    mercado = "NE";
                } else if (line.Replace(" ", "").StartsWith("NORTE")) {
                    mercado = "N";
                } else if (line.Contains("TOTAL")) {
                    mercado = "";
                } else if (!string.IsNullOrWhiteSpace(line) && mercado != "") {

                    var termLine = new GerTermicaTipoIILine();
                    //termLine.Load(line.Trim());

                    //if ((termLine.Valores[4] == null || termLine.Valores[5] == null) && !this.Any(x => x.Valores[1].Trim() == termLine[1].Trim())) {
                    //    continue;
                    //}

                    var tl = line.Trim().PadRight(100);

                    var usina = tl.Replace("   ", "      ").Substring(0, 20).Replace("*", " ").Replace("(*)", " ").Trim();
                    if (string.IsNullOrWhiteSpace(usina)) continue;

                    var tlArr = tl.Substring(20).Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries);

                    if (tlArr.Length < 7) continue;


                    termLine.SetValue(0, usina);

                    termLine.SetValue(1, tlArr[0].Trim());
                    termLine.SetValue(2, tlArr[1].Trim());
                    termLine.SetValue(3, tlArr[2].Trim());
                    termLine.SetValue(4, tlArr[3].Trim());
                    termLine.SetValue(5, tlArr[4].Trim());


                    termLine.SetValue(6, mercado);
                    termLine.SetValue(7, "II");

                    this.Add(termLine);
                }


            }
        }
    }

    public class GerTermicaLine : BaseLine { }

    public class GerTermicaTipoILine : GerTermicaLine {

        public static readonly BaseField[] campos = new BaseField[] {              
                 
           new BaseField(1, 20 ,"A20"  , "Usina"),
           new BaseField(21,32 ,"A12"  , "Razao"),
           new BaseField(33, 40 ,"F5.0"  , "Capacidade Instalada"),
           new BaseField(41, 48 ,"F5.0"  , "Capacidade Disponivel"),
           new BaseField(49, 56 ,"F5.0"  , "Media Programada"),
           new BaseField(57, 64 ,"F5.0"  , "Media Verificada"), 
           new BaseField(0, 0 ,"A2"  , "Mercado"),
           new BaseField(0, 0 ,"A5"  , "Tipo"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

    }
    public class GerTermicaTipoIILine : GerTermicaLine {

        public static readonly BaseField[] campos = new BaseField[] {              
                 
           new BaseField(1, 20 ,"A20"  , "Usina"),
           new BaseField(21,28 ,"A8"  , "Razao"),
           new BaseField(29, 37 ,"F5.0"  , "Capacidade Instalada"),
           new BaseField(38, 46 ,"F5.0"  , "Capacidade Disponivel"),
           new BaseField(47, 55 ,"F5.0"  , "Media Programada"),
           new BaseField(56, 64 ,"F5.0"  , "Media Verificada"), 
           new BaseField(0, 0 ,"A2"  , "Mercado"),
           new BaseField(0, 0 ,"A5"  , "Tipo"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

    }

}

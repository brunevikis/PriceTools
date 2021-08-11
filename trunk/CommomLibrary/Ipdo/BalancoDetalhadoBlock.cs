using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Ipdo {
    public class BalancoDetalhadoBlock : BaseBlock<BalancoDetalhadoLine> {

        System.Globalization.CultureInfo ptCulture;

        internal void Load(string fileContent) {

            ptCulture = System.Globalization.CultureInfo.GetCultureInfo("pt-br");

            BuscarEnas(fileContent);
            BuscarCargas(fileContent);

        }

        private void BuscarCargas(string fileContent) {

            var indexS = fileContent.IndexOf("Produção e Carga por Submercados");
            var indexE = fileContent.IndexOf("Balanço de Energia Detalhado");

            double[] carga = new double[4];


            fileContent = fileContent.Substring(indexS, indexE - indexS);

            string searchPattern2 = @"(?<=Carga[^\d\n]+)(?:(?:\d{1,3}\.)?\d{1,3})";

            var cargaMatches = Regex.Matches(fileContent, searchPattern2);

            if (cargaMatches.Count == 4) {

                carga[0] = double.Parse(cargaMatches[0].Value, ptCulture);
                carga[1] = double.Parse(cargaMatches[1].Value, ptCulture);
                carga[2] = double.Parse(cargaMatches[2].Value, ptCulture);
                carga[3] = double.Parse(cargaMatches[3].Value, ptCulture);

            }



            var l = this.FirstOrDefault(x => x[0] == "SE");
            if (l == null) {
                l = new BalancoDetalhadoLine() { Mercado = "SE" };
                this.Add(l);
            }
            l.Carga = carga[2];

            l = this.FirstOrDefault(x => x[0] == "S");
            if (l == null) {
                l = new BalancoDetalhadoLine() { Mercado = "S" };
                this.Add(l);
            }
            l.Carga = carga[3];

            l = this.FirstOrDefault(x => x[0] == "NE");
            if (l == null) {
                l = new BalancoDetalhadoLine() { Mercado = "NE" };
                this.Add(l);
            }
            l.Carga = carga[1];

            l = this.FirstOrDefault(x => x[0] == "N");
            if (l == null) {
                l = new BalancoDetalhadoLine() { Mercado = "N" };
                this.Add(l);
            }
            l.Carga = carga[0];

        }

        private void BuscarEnas(string fileContent) {

            var indexS = fileContent.IndexOf("Balanço de Energia Detalhado");
            var indexE = fileContent.IndexOf("Variação de Energia Armazenada");

            double[] enas = new double[4];
            double[] enasBru = new double[4];
            double[] enasArm = new double[4];

            var textLines = fileContent.Substring(indexS, indexE - indexS).Split(new string[] { "\r\n" }, StringSplitOptions.None);

            string searchPattern1 = "%?\\s{0,3}(da)?\\s*MLT\\s*bruta";
            string searchPattern2 = "%?\\s{0,3}(da)?\\s*MLT\\s*armaz";
            //string searchPattern3 = @"(?<=ENA\s+)\d{0,3}\.?\d{1,3}";

            string searchPattern4 = @"Energia\s{0,5}Afluente\s{0,5}";

            string searchValuePattern = "\\s\\d{2,3}(?=[\\x20#%])";
            string searchValuePattern2 = @"(?<=\x20)\d{0,3}\.?\d{3}(?=\x20)";

            int limitX1 = 5;
            int limitY1 = 1;
            int limitX2 = 5;
            int limitY2 = 1;

            var startSearchIndex = 0;
            for (int s = 0; s < 4; s++) {


                for (int i = startSearchIndex; i < textLines.Length; i++) {
                    var matches = Regex.Matches(textLines[i], searchPattern1, RegexOptions.IgnoreCase);

                    if (matches.Count == 0)
                        continue;

                    //textLines[i] = textLines[i].Replace(matches[0].Value, new string('#', matches[0].Value.Length));
                    textLines[i] = textLines[i].Remove(matches[0].Index, matches[0].Value.Length).Insert(matches[0].Index, new string('#', matches[0].Value.Length));

                    int x = matches[0].Index;
                    int y = i;

                    var searchBlock =
                        string.Join("\r\n",
                        textLines.Select(line => GetLineBlock(line, limitX1, x)).Skip(y - limitY1).Take(limitY1 * 2 + 1)
                        );

                    var searchBlockMod = searchBlock;//.Replace("\r\n", " \r\n ");
                    var valueMatches = Regex.Matches(searchBlockMod, searchValuePattern);

                    if (valueMatches.Count == 0) {
                        searchBlock =
                        string.Join("\r\n",
                        textLines.Select(line => GetLineBlock(line, limitX1 + 4, x)).Skip(y - limitY1 - 1).Take(limitY1 * 2 + 1 + 1)
                        );
                        searchBlockMod = searchBlock;//.Replace("\r\n", " \r\n ");
                        valueMatches = Regex.Matches(searchBlockMod, searchValuePattern);
                    }

                    if (valueMatches.Count == 1 || valueMatches.Count == 2) {
                        enasBru[s] = int.Parse(valueMatches[0].Value);
                        //data.Bruta = int.Parse(valueMatches[0].Value);

                    } else {
                        //logs.Add(string.Format("NOK, attention required at ipdo: {0}, block {1}", date.ToShortDateString(), s));
                    }

                    startSearchIndex = i - 1;
                    break;

                }

            }


            startSearchIndex = 0;
            for (int s = 0; s < 4; s++) {


                for (int i = startSearchIndex; i < textLines.Length; i++) {
                    var matches = Regex.Matches(textLines[i], searchPattern2, RegexOptions.IgnoreCase);

                    if (matches.Count == 0)
                        continue;


                    //                    textLines[i] = textLines[i].Replace(matches[0].Value, new string('#', matches[0].Value.Length));
                    textLines[i] = textLines[i].Remove(matches[0].Index, matches[0].Value.Length).Insert(matches[0].Index, new string('#', matches[0].Value.Length));

                    int x = matches[0].Index;
                    int y = i;

                    var searchBlock =
                        string.Join("\r\n",
                        textLines.Select(line => GetLineBlock(line, limitX2, x)).Skip(y - limitY2).Take(limitY2 * 2 + 1)
                        );


                    var searchBlockMod = searchBlock;//.Replace("\r\n", " \r\n ");
                    var valueMatches = Regex.Matches(searchBlockMod, searchValuePattern);

                    if (valueMatches.Count == 0) {
                        searchBlock =
                        string.Join("\r\n",
                        textLines.Select(line => GetLineBlock(line, limitX2 + 4, x)).Skip(y - limitY2 - 1).Take(limitY2 * 2 + 1 + 1)
                        );
                        searchBlockMod = searchBlock;//.Replace("\r\n", " \r\n ");
                        valueMatches = Regex.Matches(searchBlockMod, searchValuePattern);
                    }


                    if (valueMatches.Count == 1) {
                        enasArm[s] = int.Parse(valueMatches[0].Value);
                        //data.Armazenavel = int.Parse(valueMatches[0].Value);
                    } else if (valueMatches.Count == 2) {
                        enasArm[s] = int.Parse(valueMatches[1].Value);
                        //data.Armazenavel = int.Parse(valueMatches[1].Value);
                    } else {
                        //logs.Add(string.Format("NOK, attention required at ipdo: {0}, block {1}", date.ToShortDateString(), s));
                    }

                    startSearchIndex = i - 1;
                    break;

                }
            }


            startSearchIndex = 0;
            for (int s = 0; s < 4; s++) {


                for (int i = startSearchIndex; i < textLines.Length; i++) {
                    var matches = Regex.Matches(textLines[i], searchPattern4, RegexOptions.IgnoreCase);

                    if (matches.Count == 0)
                        continue;

                    textLines[i] = textLines[i].Remove(matches[0].Index, matches[0].Value.Length).Insert(matches[0].Index, new string('#', matches[0].Value.Length));

                    int x = matches[0].Index + 24;
                    int y = i;

                    var searchBlock =
                        string.Join("\r\n",
                        textLines.Select(line => GetLineBlock(line, limitX1-1, x)).Skip(y - limitY1).Take(limitY1 * 2 + 1)
                        );

                    var searchBlockMod = searchBlock;//.Replace("\r\n", " \r\n ");
                    var valueMatches = Regex.Matches(searchBlockMod, searchValuePattern2);


                    if (valueMatches.Count == 0) {
                        searchBlock =
                        string.Join("\r\n",
                        textLines.Select(line => GetLineBlock(line, limitX1 + 3, x)).Skip(y - limitY1).Take(limitY1 * 2 + 1)
                        );
                        searchBlockMod = searchBlock;//.Replace("\r\n", " \r\n ");
                        valueMatches = Regex.Matches(searchBlockMod, searchValuePattern2);
                    }


                    if (valueMatches.Count == 1) {
                        enas[s] = double.Parse(valueMatches[0].Value, ptCulture);
                        //data.Armazenavel = int.Parse(valueMatches[0].Value);
                    } else if (valueMatches.Count == 2) {
                        enas[s] = double.Parse(valueMatches[1].Value, ptCulture);
                        //data.Armazenavel = int.Parse(valueMatches[1].Value);
                    } else if (valueMatches.Count == 3) {
                        enas[s] = double.Parse(valueMatches[2].Value, ptCulture);
                        //data.Armazenavel = int.Parse(valueMatches[1].Value);
                    } else {
                        //logs.Add(string.Format("NOK, attention required at ipdo: {0}, block {1}", date.ToShortDateString(), s));
                    }

                    startSearchIndex = i - 1;
                    break;

                }

            }


            //var enasMatches = Regex.Matches(fileContent, searchPattern3);

            //if (enasMatches.Count == 4) {

            //    enas[0] = double.Parse(enasMatches[0].Value, ptCulture);
            //    enas[1] = double.Parse(enasMatches[1].Value, ptCulture);
            //    enas[2] = double.Parse(enasMatches[2].Value, ptCulture);
            //    enas[3] = double.Parse(enasMatches[3].Value, ptCulture);

            //}



            var l = this.FirstOrDefault(x => x[0] == "SE");
            if (l == null) {
                l = new BalancoDetalhadoLine() { Mercado = "SE" };
                this.Add(l);
            }
            l.Bruta = enasBru[2];
            l.Armazenavel = enasArm[2];
            l.Ena = enas[2];

            l = this.FirstOrDefault(x => x[0] == "S");
            if (l == null) {
                l = new BalancoDetalhadoLine() { Mercado = "S" };
                this.Add(l);
            }
            l.Bruta = enasBru[3];
            l.Armazenavel = enasArm[3];
            l.Ena = enas[3];
            l = this.FirstOrDefault(x => x[0] == "NE");
            if (l == null) {
                l = new BalancoDetalhadoLine() { Mercado = "NE" };
                this.Add(l);
            }
            l.Bruta = enasBru[1];
            l.Armazenavel = enasArm[1];
            l.Ena = enas[1];
            l = this.FirstOrDefault(x => x[0] == "N");
            if (l == null) {
                l = new BalancoDetalhadoLine() { Mercado = "N" };
                this.Add(l);
            }
            l.Bruta = enasBru[0];
            l.Armazenavel = enasArm[0];
            l.Ena = enas[0];




        }

        static string GetLineBlock(string input, int limit, int midPoint) {

            int startIndex = midPoint - limit;
            if (startIndex < 0)
                startIndex = 0;

            if (input.Length < startIndex + 1) {
                return "";
            } else {

                int length = limit * 2 + 1;
                if (input.Length < midPoint + limit + 1) {
                    return input.Substring(startIndex) + " ";
                } else {
                    return input.Substring(startIndex, length) + " ";
                }

            }

        }
    }

    public class BalancoDetalhadoLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {                               
           new BaseField(0, 0 ,"A2"  , "Mercado"),
           new BaseField(0, 0 ,"F9.0"  , "Hidro Verificada"),
           new BaseField(0, 0 ,"F9.0"  , "Termo Verificada"),
           new BaseField(0, 0 ,"F5.2"  , "Termo Nuclear Verificada"),
           new BaseField(0, 0 ,"F9.0"  , "Eolica Verificada"),
           new BaseField(0, 0 ,"F9.0"  , "Intercambio Internacional Verificada"),
           new BaseField(0, 0 ,"F9.0"  , "Carga Verificada"),
           new BaseField(0, 0 ,"F9.0"  , "ENA"),
           new BaseField(0, 0 ,"F9.0"  , "ENA Bruta %MLT"),
           new BaseField(0, 0 ,"F9.0"  , "ENA Armazenavel %MLT"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }


        public double Bruta { get { return (double)this[8]; } set { this[8] = value; } }

        public double Armazenavel { get { return (double)this[9]; } set { this[9] = value; } }

        public string Mercado { get { return this[0].ToString(); } set { this[0] = value; } }

        public double Ena { get { return (double)this[7]; } set { this[7] = value; } }

        public double Carga { get { return (double)this[6]; } set { this[6] = value; } }


    }
}

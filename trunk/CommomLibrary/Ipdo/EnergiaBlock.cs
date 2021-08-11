using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Ipdo {
    public class EnergiaBlock : BaseBlock<EnergiaLine> {

        System.Globalization.CultureInfo ptCulture;

        internal void Load(string fileContent) {

            ptCulture = System.Globalization.CultureInfo.GetCultureInfo("pt-br");

            BuscarEarms(fileContent);

        }

        private void BuscarEarms(string fileContent) {

            var indexS = fileContent.IndexOf("Variação de Energia Armazenada");
            var indexE = fileContent.IndexOf("Destaques da Operação");

            double[] earmMax = new double[4];
            double[] earm = new double[4];
            double[] earmMLT = new double[4];

            fileContent = fileContent.Substring(indexS, indexE - indexS);            

            string searchPattern1 = @"(?<=Capacidade Máxima.+)(?:\b\d+\.?\d{1,3}\b)";
            string searchPattern2 = @"(?<=Armazenamento ao final do dia.+)(?:\b(?:\d+[\.,])?\d+\b)";

            var earmMaxMatches = Regex.Matches(fileContent, searchPattern1);

            if (earmMaxMatches.Count == 4) {

                earmMax[0] = double.Parse(earmMaxMatches[0].Value, ptCulture);
                earmMax[1] = double.Parse(earmMaxMatches[1].Value, ptCulture);
                earmMax[2] = double.Parse(earmMaxMatches[2].Value, ptCulture);
                earmMax[3] = double.Parse(earmMaxMatches[3].Value, ptCulture);

            }

            var earmMatches = Regex.Matches(fileContent, searchPattern2);

            if (earmMatches.Count == 8) {

                earm[0] = double.Parse(earmMatches[0].Value, ptCulture);
                earm[1] = double.Parse(earmMatches[1].Value, ptCulture);
                earm[2] = double.Parse(earmMatches[2].Value, ptCulture);
                earm[3] = double.Parse(earmMatches[3].Value, ptCulture);

                earmMLT[0] = double.Parse(earmMatches[4].Value, ptCulture);
                earmMLT[1] = double.Parse(earmMatches[5].Value, ptCulture);
                earmMLT[2] = double.Parse(earmMatches[6].Value, ptCulture);
                earmMLT[3] = double.Parse(earmMatches[7].Value, ptCulture);

            }



            var l = this.FirstOrDefault(x => x[0] == "SE");
            if (l == null) {
                l = new EnergiaLine() { Mercado = "SE" };
                this.Add(l);
            }
            l[1] = earmMax[1];
            l[2] = earm[1];
            l[3] = earmMLT[1];

            l = this.FirstOrDefault(x => x[0] == "S");
            if (l == null) {
                l = new EnergiaLine() { Mercado = "S" };
                this.Add(l);
            }
            l[1] = earmMax[0];
            l[2] = earm[0];
            l[3] = earmMLT[0];
            l = this.FirstOrDefault(x => x[0] == "NE");
            if (l == null) {
                l = new EnergiaLine() { Mercado = "NE" };
                this.Add(l);
            }
            l[1] = earmMax[3];
            l[2] = earm[3];
            l[3] = earmMLT[3];
            l = this.FirstOrDefault(x => x[0] == "N");
            if (l == null) {
                l = new EnergiaLine() { Mercado = "N" };
                this.Add(l);
            }
            l[1] = earmMax[2];
            l[2] = earm[2];
            l[3] = earmMLT[2];
        }
    }

    public class EnergiaLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {                               
           new BaseField(0, 0 ,"A2"  , "Mercado"),
           new BaseField(0, 0 ,"F9.0"  , "EarmMax"),
           new BaseField(0, 0 ,"F9.0"  , "Earm"),
           new BaseField(0, 0 ,"F5.2"  , "EarmMLT"),

        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public string Mercado { get { return this[0].ToString(); } set { this[0] = value; } }

        public double EarmMlt { get { return (double)this[3]; } set { this[3] = value; } }

    }
}

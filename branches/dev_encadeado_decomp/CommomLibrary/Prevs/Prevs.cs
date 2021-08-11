using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Prevs {
    public class Prevs : BaseDocument {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Prev"             , new PrevBlock()},                 
                };

        public PrevBlock Vazoes { get { return (PrevBlock)Blocos["Prev"]; } }

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines) {
                var newLine = Blocos["Prev"].CreateLine(line);
                Blocos["Prev"].Add(newLine);
            }
        }

        /// <summary>
        /// prev[posto][semana]
        /// </summary>
        /// <param name="posto">numero do posto</param>
        /// <param name="semana">semana de 1 a 6</param>
        /// <returns>Vazão Natural</returns>
        public int this[int posto, int semana] {
            get {

                var line =
                    blocos["Prev"]
                        .Where(c => c.Valores[1] == posto).FirstOrDefault();

                if (line == null) {
                    return getVazaoArtificial(posto, semana);
                } else
                    return (int)line[semana + 1];
            }
            set {
                var line = blocos["Prev"]
                        .Where(c => c.Valores[1] == posto).FirstOrDefault();

                if (line == null) {
                    return;
                }
                line[semana + 1] = value;
            }
        }

        private int getVazaoArtificial(int posto, int semana) {
            var vaz = this;
            var s = semana;

            switch (posto) {

                case 37: return (int)(vaz[237, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 38: return (int)(vaz[238, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 39: return (int)(vaz[239, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 40: return (int)(vaz[240, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 42: return (int)(vaz[242, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 43: return (int)(vaz[243, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 44: return (int)(vaz[244, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 45: return (int)(vaz[245, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 46: return (int)(vaz[246, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 66: return (int)(vaz[266, s] - 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]) - vaz[117, s] - vaz[118, s]);

                case 70: return (int)(vaz[73, s] - Math.Min(vaz[73, s] - 10, 173.5));

                case 75: return (int)(vaz[76, s] - Math.Min(vaz[73, s] - 10, 173.5));

                case 104: return (int)(vaz[117, s] + vaz[118, s]);

                case 109: return (int)(vaz[118, s]);

                case 116: return (int)(vaz[119, s] - vaz[118, s]);

                case 119: return (int)(vaz[118, s] * 1.27 - 0.44);

                case 126: return vaz[127, s] <= 430 ? (int)Math.Max(0, vaz[127, s] - 90) : 340;

                case 127: return vaz[129, s] - vaz[298, s] - vaz[203, s] + vaz[304, s];

                case 131: return (int)Math.Min(vaz[316, s], 144);

                case 132: return vaz[202, s] + (int)Math.Min(vaz[201, s], 25);

                case 164: return (int)(vaz[161, s] - vaz[117, s] - vaz[118, s]);

                case 173: return (int)(vaz[172, s]);

                case 176: return (int)(vaz[173, s]);

                case 244: return (int)(vaz[34, s] + vaz[243, s]);

                case 293: return (int)(1.07 * vaz[288, s] - vaz[292, s]);

                case 298: return vaz[125, s] <= 190 ? vaz[125, s] * 119 / 190 : (
                         vaz[125, s] <= 209 ? 119 : (
                             vaz[125, s] <= 250 ? vaz[125, s] - 90 : (
                             250
                         )
                     )
                 );

                case 299: return vaz[130, s] - vaz[298, s] - vaz[203, s] + vaz[304, s];

                case 300: return 0;

                case 303: return vaz[132, s] + (int)Math.Min(vaz[316, s] - vaz[131, s], 51);

                case 304: return vaz[315, s] - vaz[316, s];

                case 306: return vaz[303, s] + vaz[131, s];

                case 314: return vaz[199, s] - vaz[298, s] - vaz[203, s] + vaz[304, s];

                case 315: return vaz[203, s] - vaz[201, s] + vaz[317, s] + vaz[298, s];

                case 316: return (int)Math.Min(vaz[315, s], 190);

                case 317: return (int)Math.Max(0, vaz[201, s] - 25);

                case 318: return (int)(vaz[116, s] + vaz[117, s] + vaz[118, s] + 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]));

                case 319: return (int)(vaz[117, s] + vaz[118, s] + 0.1 * (vaz[161, s] - vaz[117, s] - vaz[118, s]));

                case 320: return vaz[119, s];

                default:
                    return -1;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Pmo {
    public class EarmiBlock : BaseBlock<EarmiLine> {

        internal void Load(string fileContent) {


            var i = fileContent.IndexOf("ENERGIA ARMAZENADA INICIAL");
            if (i < 0) return;
            var lines = fileContent.Remove(0, i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Take(3).ToArray();


            var lree = lines[0].PadRight(15 * 13);
            var learm = lines[1].PadRight(15 * 13);
            var learmp = lines[2].PadRight(15 * 13).Replace("%", " ");

            for (int r = 0; r < 15; r++) {

                var ree = lree.Substring(r * 13 + 3, 10).Trim();

                if (!string.IsNullOrWhiteSpace(ree)) {

                    var earm = double.Parse(learm.Substring(r * 13 + 3, 10).Trim(), System.Globalization.NumberFormatInfo.InvariantInfo);

                    var earmp = earm == 0 ? 0 :
                        double.Parse(learmp.Substring(r * 13 + 3, 10).Trim(), System.Globalization.NumberFormatInfo.InvariantInfo);


                    this.Add(
                        new EarmiLine() {
                            Ree = ree,
                            Earm = earm,
                            EarmP = earmp
                        });


                }

            }






        }

        internal void Load(System.IO.StreamReader tr) {

            tr.ReadLine();


            var lree = tr.ReadLine().PadRight(15 * 13);
            var learm = tr.ReadLine().PadRight(15 * 13);
            var learmp = tr.ReadLine().PadRight(15 * 13).Replace("%", " ");

            for (int r = 0; r < 15; r++) {

                var ree = lree.Substring(r * 13 + 3, 10).Trim();

                if (!string.IsNullOrWhiteSpace(ree)) {

                    var earm = double.Parse(learm.Substring(r * 13 + 3, 10).Trim(), System.Globalization.NumberFormatInfo.InvariantInfo);

                    var earmp = earm == 0 ? 0 :
                        double.Parse(learmp.Substring(r * 13 + 3, 10).Trim(), System.Globalization.NumberFormatInfo.InvariantInfo);


                    this.Add(
                        new EarmiLine() {
                            Ree = ree,
                            Earm = earm,
                            EarmP = earmp
                        });
                }
            }
        }
    }

    public class EarmiLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(0 , 0 ,"A2"  , "REE"),                                
           new BaseField(0 , 0 ,"F10.2"  , "EARM"),
           new BaseField(0 , 0 ,"F5.2"  , "EARM %"),          

        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public string Ree { get { return this["REE"]; } set { this["REE"] = value; } }
        public double Earm { get { return this["EARM"]; } set { this["EARM"] = value; } }
        public double EarmP { get { return this["EARM %"]; } set { this["EARM %"] = value; } }


    }
}

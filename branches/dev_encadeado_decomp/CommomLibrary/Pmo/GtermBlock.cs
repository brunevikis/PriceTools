using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Pmo {
    public class GtermBlock : BaseBlock<GtermLine> {


        public new GtermLine this[int cod] {
            get {
                return this.Where(x => x[0] == cod).FirstOrDefault();
            }
        }

        internal void Load(string fileContent, string key) {



            int startIndex = fileContent.IndexOf(key);

            for (int i = fileContent.IndexOf(key); i >= 0; i = fileContent.IndexOf(key, i + 1)) {

                int e = fileContent.IndexOf(" X------------------------------------------------------", i + 150);

                var lines = fileContent.Substring(i, e - i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(6).ToArray();

                foreach (var line in lines) {

                    var temp = this.CreateLine(line);

                    if (temp[0] is int) {



                    }



                }
            }
        }

        internal void Load(System.IO.StreamReader tr) {

            tr.ReadLine();

            string l = tr.ReadLine();
            GtermLine lastline = null;
            int j = 0;
            do {


                var temp = this.CreateLine(l);

                if (temp[0] is int) {

                    this.Add(temp);
                    lastline = temp;
                    j = 0;

                } else if (temp[2] is int && lastline != null) {
                    j += 12;
                    for (int i = 0; i < 12; i++) {
                        lastline[3 + i + j] = temp[3+i];
                    }


                    //limita a leitura a dois anos de estudo
                    lastline = null;
                }


                l = tr.ReadLine();

            } while (l != null && !l.Contains("X------------------------------------------------------------------"));


        }
    }


    public class GtermLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 3 ,   8 ,"I3"  , "Cod"),
                new BaseField(10 ,  21 ,"A12" , "Nome"),
                new BaseField(23 ,  29 ,"I4"  , "Ano"),
                new BaseField(31 , 37 ,"F7.2" , "0_1" ),
                new BaseField(39 , 45 ,"F7.2" , "0_2" ),
                new BaseField(47 , 53 ,"F7.2" , "0_3" ),
                new BaseField(55 , 61 ,"F7.2" , "0_4" ),
                new BaseField(63 , 69 ,"F7.2" , "0_5" ),
                new BaseField(71 , 77 ,"F7.2" , "0_6" ),
                new BaseField(79 , 85 ,"F7.2" , "0_7" ),
                new BaseField(87 , 93 ,"F7.2" , "0_8" ),
                new BaseField(95 , 101,"F7.2" , "0_9" ),
                new BaseField(103, 109,"F7.2" , "0_10"),
                new BaseField(111, 117,"F7.2" , "0_11"),
                new BaseField(119, 125,"F7.2" , "0_12"),
                new BaseField(0,  0,"F7.2", "1_1"),
                new BaseField(0,  0,"F7.2", "1_2"),
                new BaseField(0,  0,"F7.2", "1_3"),
                new BaseField(0,  0,"F7.2", "1_4"),
                new BaseField(0,  0,"F7.2", "1_5"),
                new BaseField(0,  0,"F7.2", "1_6"),
                new BaseField(0,  0,"F7.2", "1_7"),
                new BaseField(0,  0,"F7.2", "1_8"),
                new BaseField(0,  0,"F7.2", "1_9"),
                new BaseField(0,  0,"F7.2", "1_10"),
                new BaseField(0,  0,"F7.2", "1_11"),
                new BaseField(0,  0,"F7.2", "1_12"),
                //new BaseField(0,  0,"F7.2", "2_1"),
                //new BaseField(0,  0,"F7.2", "2_2"),
                //new BaseField(0,  0,"F7.2", "2_3"),
                //new BaseField(0,  0,"F7.2", "2_4"),
                //new BaseField(0,  0,"F7.2", "2_5"),
                //new BaseField(0,  0,"F7.2", "2_6"),
                //new BaseField(0,  0,"F7.2", "2_7"),
                //new BaseField(0,  0,"F7.2", "2_8"),
                //new BaseField(0,  0,"F7.2", "2_9"),
                //new BaseField(0,  0,"F7.2", "2_10"),
                //new BaseField(0,  0,"F7.2", "2_11"),
                //new BaseField(0,  0,"F7.2", "2_12"),
                //new BaseField(0,  0,"F7.2", "3_1"),
                //new BaseField(0,  0,"F7.2", "3_2"),
                //new BaseField(0,  0,"F7.2", "3_3"),
                //new BaseField(0,  0,"F7.2", "3_4"),
                //new BaseField(0,  0,"F7.2", "3_5"),
                //new BaseField(0,  0,"F7.2", "3_6"),
                //new BaseField(0,  0,"F7.2", "3_7"),
                //new BaseField(0,  0,"F7.2", "3_8"),
                //new BaseField(0,  0,"F7.2", "3_9"),
                //new BaseField(0,  0,"F7.2", "3_10"),
                //new BaseField(0,  0,"F7.2", "3_11"),
                //new BaseField(0,  0,"F7.2", "3_12"),
                //new BaseField(0,  0,"F7.2", "4_1"),
                //new BaseField(0,  0,"F7.2", "4_2"),
                //new BaseField(0,  0,"F7.2", "4_3"),
                //new BaseField(0,  0,"F7.2", "4_4"),
                //new BaseField(0,  0,"F7.2", "4_5"),
                //new BaseField(0,  0,"F7.2", "4_6"),
                //new BaseField(0,  0,"F7.2", "4_7"),
                //new BaseField(0,  0,"F7.2", "4_8"),
                //new BaseField(0,  0,"F7.2", "4_9"),
                //new BaseField(0,  0,"F7.2", "4_10"),
                //new BaseField(0,  0,"F7.2", "4_11"),
                //new BaseField(0,  0,"F7.2", "4_12"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

}



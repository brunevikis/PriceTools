using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParpDat {
    public class ParpDat : BaseDocument {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                
                    {"MLT"                  , new MLTBlock()},                     

                    
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public MLTBlock MLT { get { return (MLTBlock)Blocos["MLT"]; } }


        public ParpDat(string filepath) {

            MLTLine mltline = null;


            using (var fs = System.IO.File.OpenRead(filepath))
            using (var tr = new System.IO.StreamReader(fs)) {

                string line = tr.ReadLine();
                while (!tr.EndOfStream) {

                    if (line.Contains("SERIE")
                        && line.Contains("  1)")
                        ) {

                        var match = System.Text.RegularExpressions.Regex.Match(line, @"SERIE\s+DE\s+ENERGIAS\s+DO\s+REE\s+([\w\s\.\-]+).+\b1\)");

                        if (match.Success) {
                            mltline = (MLTLine)((MLTBlock)Blocos["MLT"]).CreateLine();
                            mltline[0] = match.Groups[1].Value.Trim();
                            Blocos["MLT"].Add(mltline);
                        }


                    } else if (mltline != null && line.Contains("MEDIA") && line.Contains("ENERGIAS")) {
                        do {

                            //for (; i < lines.Length; i++) {
                            line = tr.ReadLine().Trim();
                            // line = lines[i];

                            if (!string.IsNullOrWhiteSpace(line) && !line.Contains("JAN")) {
                                var arr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                if (arr.Length == 12) {
                                    for (int j = 0; j < 12; j++) {
                                        mltline.SetValue(j + 1, arr[j]);
                                    }
                                    mltline = null;
                                    break;
                                }
                            }

                        } while (!tr.EndOfStream);
                    }

                    line = tr.ReadLine();


                }
            }
        }



    }

    public class MLTBlock : BaseBlock<MLTLine> {


        internal void Load(string fileContent, string key) {


            var i = fileContent.IndexOf(key);
            if (i < 0) return;
            var lines = fileContent.Remove(0, i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(4).Take(15).ToArray();

            foreach (var line in lines) {

                if (line.Contains("X--------")) break;

                this.Add(
                    this.CreateLine(line)
                );
            }
        }

        internal void Load(System.IO.StreamReader tr) {

            tr.ReadLine(); tr.ReadLine(); tr.ReadLine();
            var line = tr.ReadLine();


            while (!line.Contains("X--------")) {
                this.Add(
                    this.CreateLine(line)
                );
                line = tr.ReadLine();
            }
        }
    }


    public class MLTLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 2 ,  11 ,"A10"  , "REE"),
                new BaseField(13  , 22 ,"F10.2"  , "JAN"),
                new BaseField(24  , 33 ,"F10.2"  , "FEV"),
                new BaseField(35  , 44 ,"F10.2"  , "MAR"),
                new BaseField(46  , 55 ,"F10.2"  , "ABR"),
                new BaseField(57  , 66 ,"F10.2"  , "MAI"),
                new BaseField(68  , 77 ,"F10.2"  , "JUN"),
                new BaseField(79  , 88 ,"F10.2"  , "JUL"),
                new BaseField(90  , 99 ,"F10.2"  , "AGO"),
                new BaseField(101 , 110,"F10.2"  , "SET"),
                new BaseField(112 , 121,"F10.2"  , "OUT"),
                new BaseField(123 , 132,"F10.2"  , "NOV"),
                new BaseField(134 , 143,"F10.2"  , "DEZ"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public string Ree { get { return this[0]; } }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.PrevcenDat {
    public class PrevcenDat : BaseDocument, IEnumerable<PrevcenLine> {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Prevcen"             , new PrevcenBlock()},                 
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines.Skip(2)) {
                var newLine = Blocos["Prevcen"].CreateLine(line);
                Blocos["Prevcen"].Add(newLine);
            }
        }



        public IEnumerator<PrevcenLine> GetEnumerator() {
            return ((PrevcenBlock)Blocos["Prevcen"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return Blocos["Prevcen"].GetEnumerator();
        }
    }

    public class PrevcenBlock : BaseBlock<PrevcenLine> {
    }

    public class PrevcenLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2  , 4  ,"I3"  , "Posto"),    
                new BaseField( 6  , 8  ,"I3"  , "Usina"),
                new BaseField( 10 , 12 ,"I3"  , "M0"),
                new BaseField( 14 , 16 ,"I3"  , "M1"),
                new BaseField( 18 , 20 ,"I3"  , "M2"),
                new BaseField( 22 , 24 ,"I3"  , "M3"),
                new BaseField( 26 , 28 ,"I3"  , "M4"),
                new BaseField( 30 , 32 ,"I3"  , "M5"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
        public int Posto { get { return this[0]; } }
        public int Usina { get { return this[1]; } }
        public int M0 { get { return this[2]; } }
        public int M1 { get { return this[3]; } }
        public int M2 { get { return this[4]; } }
        public int M3 { get { return this[5]; } }
        public int M4 { get { return this[6]; } }
        public int M5 { get { return this[7]; } }
    }
}

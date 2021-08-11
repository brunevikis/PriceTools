using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.CadTermDat {
    public class CadTermDat : BaseDocument, IQueryable<CadTermLine> {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"CadTerm"             , new CadTermBlock()},                                        
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            foreach (var line in lines) {
                var newLine = Blocos["CadTerm"].CreateLine(line);
                if (newLine[0] != null) {
                    Blocos["CadTerm"].Add(newLine);
                }
            }
        }

        public IEnumerator<CadTermLine> GetEnumerator() {
            return ((CadTermBlock)Blocos["CadTerm"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((CadTermBlock)Blocos["CadTerm"]).GetEnumerator();
        }

        public Type ElementType {
            get { return ((CadTermBlock)Blocos["CadTerm"]).AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression {
            get { return ((CadTermBlock)Blocos["CadTerm"]).AsQueryable().Expression; }
        }

        public IQueryProvider Provider {
            get { return ((CadTermBlock)Blocos["CadTerm"]).AsQueryable().Provider; }
        }
    }
    public class CadTermBlock : BaseBlock<CadTermLine> {
        string header =
@"(NUM      NOMUSI      SSIS  SITU CLAST CLAST N       SMERC NUNID PROP        POT     FCAR        CGER     CCC  TCOMB/DINI  GTMIN1  GTMIN2  GTMIN3  TIF         IP          GTMINe OUTROS)
(XXXXX  XXXXXXXXXXXX  XXXX  XXX  XXXX  XXXXXXXXXXXX  XXXX  XXXX  XXXXXXXXXX  XXXXXX  XXXXXXXXXX  XXXXXXX  XXX  XXXXXXXXXX  XXXXXX  XXXXXX  XXXXXX  XXXXXXXXXX  XXXXXXXXXX  XXXXXX ......)
"
;

        public override string ToText() {

            return header + base.ToText();
        }
    }
    public class CadTermLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField(1    , 6  ,"I6"  , "Num"),
                new BaseField(9    , 20 ,"A12" , "Nome"),
                new BaseField(23   , 26 ,"I4"  , "SSis"),
                new BaseField(29   , 31 ,"A2"  , "Situacao"),
                new BaseField(170  ,178 ,"F5.2", "Gtmin"),
        };
        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Num {
            get {
                return valores[campos[0]]; ;
            }
        }

        public string Nome { get { return valores[campos[1]]; } }

        public int Sistema { get { return valores[campos[2]]; } }

        public string Existente { get { return valores[campos[3]]; } }

        public double Gtmin { get { return valores[campos[4]]; } }
    }

}

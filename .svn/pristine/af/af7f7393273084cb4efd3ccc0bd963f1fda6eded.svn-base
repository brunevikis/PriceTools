using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.ConftDat {
    public class ConftDat : BaseDocument, IQueryable<ConftLine> {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Conft"             , new ConftBlock()},                                        
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            foreach (var line in lines) {
                var newLine = Blocos["Conft"].CreateLine(line);
                if (newLine[0] != null) {
                    Blocos["Conft"].Add(newLine);
                }
            }
        }

        public IEnumerator<ConftLine> GetEnumerator() {
            return ((ConftBlock)Blocos["Conft"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((ConftBlock)Blocos["Conft"]).GetEnumerator();
        }

        public Type ElementType {
            get { return ((ConftBlock)Blocos["Conft"]).AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression {
            get { return ((ConftBlock)Blocos["Conft"]).AsQueryable().Expression; }
        }

        public IQueryProvider Provider {
            get { return ((ConftBlock)Blocos["Conft"]).AsQueryable().Provider; }
        }
    }
    public class ConftBlock : BaseBlock<ConftLine> {
        string header =
@" NUM  NOME           SSIS  U.EXIS CLASSE
 XXXX XXXXXXXXXXXX   XXXX     XX   XXXX
"
;

        public override string ToText() {

            return header + base.ToText();
        }
    }
    public class ConftLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField(2    , 5 ,"I4"  , "Num"),
                new BaseField(7    , 18 ,"A12" , "Nome"),
                new BaseField(22   , 25,"I4"  , "SSis"),
                new BaseField(31   , 32 ,"A2"  , "E.Exist"),
                new BaseField(36   , 39 ,"I4" , "Classe"),
                new BaseField(46   , 48 ,"I3" , "Tecno"),
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

        public String Existente { get { return valores[campos[3]]; } set { valores[campos[3]] = value; }}

        public int Classe { get { return valores[campos[4]]; } }
    }

}

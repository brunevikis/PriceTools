using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.ConfhdDat {
    public class ConfhdDat : BaseDocument, IQueryable<ConfhdLine> {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"ConfHd"             , new ConfhdBlock()},                                        
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            foreach (var line in lines) {
                var newLine = Blocos["ConfHd"].CreateLine(line);
                if (newLine[0] != null) {
                    Blocos["ConfHd"].Add(newLine);
                }
            }
        }

        public IEnumerator<ConfhdLine> GetEnumerator() {
            return ((ConfhdBlock)Blocos["ConfHd"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((ConfhdBlock)Blocos["ConfHd"]).GetEnumerator();
        }

        public Type ElementType {
            get { return ((ConfhdBlock)Blocos["ConfHd"]).AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression {
            get { return ((ConfhdBlock)Blocos["ConfHd"]).AsQueryable().Expression; }
        }

        public IQueryProvider Provider {
            get { return ((ConfhdBlock)Blocos["ConfHd"]).AsQueryable().Provider; }
        }
    }
    public class ConfhdBlock : BaseBlock<ConfhdLine> {
        string header =
@" NUM  NOME         POSTO JUS  SSIS V.INIC U.EXIS MODIF INIC.HIST FIM HIST
 XXXX XXXXXXXXXXXX XXXX  XXXX XXXX XXX.XX XXXX   XXXX     XXXX     XXXX
"
;

        public override string ToText() {

            return header + base.ToText();
        }
    }
    public class ConfhdLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField(2    , 5 ,"I4"  , "Cod"),
                new BaseField(7    , 18 ,"A12" , "Usina"),
                new BaseField(20    ,23 ,"I4"  , "Posto"),
                new BaseField(26   , 29,"I4"  , "CodJusante"),
                new BaseField(31   , 34,"I4"  , "REE"),
                new BaseField(36   , 41 ,"F6.2"  , "Vol Util"),
                new BaseField(45   , 46 ,"A2" , "Situacao"),
                new BaseField(50   , 53,"I4"  , "Modif"),
                new BaseField(59   , 62,"I4"  , "Inicio Hidr"),
                new BaseField(68   , 71,"I4"  , "Fim Hidr"),
                new BaseField(74   , 76,"I3"  , "Tecno"),
        };


        public int Cod {
            get {
                return valores[campos[0]]; ;
            }
        }

        public override BaseField[] Campos {
            get { return campos; }
        }

        public string Usina { get { return valores[campos[1]]; } }

        public int Posto { get { return valores[campos[2]]; } }

        public int CodJusante { get { return valores[campos[3]]; } }

        public double VolUtil {
            get { return valores[campos[5]]; }
            set {
                valores[campos[5]] = value;
            }
        }

        public int REE { get { return valores[campos[4]]; } }

        public string Situacao { get { return valores[campos[6]]; } set { valores[campos[6]] = value; } }

        public bool Modif { get { return valores[campos[7]] == 1 ? true : false; } set { valores[campos[7]] = value ? 1 : 0; } }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ExptDat {
    public class ExptDat : BaseDocument, IQueryable<ExptLine>, IList<ExptLine> {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Expt"             , new ExptBlock()},       
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            foreach (var line in lines) {
                var newLine = Blocos["Expt"].CreateLine(line);
                if (newLine[0] != null) {
                    Blocos["Expt"].Add(newLine);
                }
            }
        }

        public IEnumerator<ExptLine> GetEnumerator() {
            return ((ExptBlock)Blocos["Expt"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((ExptBlock)Blocos["Expt"]).GetEnumerator();
        }

        public Type ElementType {
            get { return ((ExptBlock)Blocos["Expt"]).AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression {
            get { return ((ExptBlock)Blocos["Expt"]).AsQueryable().Expression; }
        }

        public IQueryProvider Provider {
            get { return ((ExptBlock)Blocos["Expt"]).AsQueryable().Provider; }
        }

        public int IndexOf(ExptLine item) {
            return ((ExptBlock)Blocos["Expt"]).IndexOf(item);
        }

        public void Insert(int index, ExptLine item) {
            ((ExptBlock)Blocos["Expt"]).Insert(index, item);
        }

        public void RemoveAt(int index) {
            ((ExptBlock)Blocos["Expt"]).RemoveAt(index);
        }

        public ExptLine this[int index] {
            get {
                return ((ExptBlock)Blocos["Expt"])[index];
            }
            set {
                ((ExptBlock)Blocos["Expt"])[index] = value;
            }
        }

        public void Add(ExptLine item) {
            ((ExptBlock)Blocos["Expt"]).Add(item);
        }

        public void Clear() {
            ((ExptBlock)Blocos["Expt"]).Clear();
        }

        public bool Contains(ExptLine item) {
            return ((ExptBlock)Blocos["Expt"]).Contains(item);
        }

        public void CopyTo(ExptLine[] array, int arrayIndex) {
            ((ExptBlock)Blocos["Expt"]).CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return ((ExptBlock)Blocos["Expt"]).Count(); }
        }

        public bool IsReadOnly {
            get { return ((ExptBlock)Blocos["Expt"]).IsReadOnly; }
        }

        public bool Remove(ExptLine item) {
            return ((ExptBlock)Blocos["Expt"]).Remove(item);
        }
    }
    public class ExptBlock : BaseBlock<ExptLine> {
        string header =
@"NUM   TIPO   MODIF  MI ANOI MF ANOF
XXXX XXXXX XXXXXXXX XX XXXX XX XXXX
"
;

        public override string ToText() {

            return header + base.ToText();
        }
    }
    public class ExptLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField(1 , 4 ,"I4"  , "Cod"),
                new BaseField(6 , 10 ,"A5"  , "Tipo"),                
                new BaseField(12 , 19 ,"F8.2"  , "Valor"),
                new BaseField(21 , 22 ,"I2"  , "Mes Inicio"),
                new BaseField(24 , 27 ,"I4"  , "Ano Inicio"),
                new BaseField(29 , 30 ,"I2"  , "Mes Fim"),
                new BaseField(32 , 35 ,"I4"  , "Ano Fim"),
                new BaseField(38 , 60 ,"A23"  , "Comentario"),
        };


        public string Comentario { get { return this["Comentario"]; } set { this["Comentario"] = value; } }

        public DateTime DataInicio {
            get {
                return new DateTime(valores[campos[4]], valores[campos[3]], 1);
            }
            set {
                valores[campos[4]] = value.Year;
                valores[campos[3]] = value.Month;
            }
        }

        public DateTime DataFim {
            get {
                if (valores[campos[6]] == null || valores[campos[5]] == null)
                    return DateTime.MaxValue;

                else
                    return new DateTime(valores[campos[6]], valores[campos[5]], 1);
            }
            set {
                if (value == DateTime.MaxValue) {
                    valores[campos[6]] = valores[campos[5]] = null;
                } else {
                    valores[campos[6]] = value.Year;
                    valores[campos[5]] = value.Month;
                }

            }
        }

        int cod = -1;
        public int Cod {
            get {
                if (cod == -1) {
                    cod = valores[campos[0]];
                }
                return cod;
            }
            set {
                valores[campos[0]] = value;
            }
        }

        public double Valor {
            get { return valores[campos[2]]; }
            set { valores[campos[2]] = value; }
        }

        public string Tipo {
            get { return valores[campos[1]]; }
            set { valores[campos[1]] = value; }
        }

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

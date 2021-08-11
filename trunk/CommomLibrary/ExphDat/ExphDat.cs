using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ExphDat {
    public class ExphDat : BaseDocument, IList<ExphLine> {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Exph"             , new ExphBlock()},                 
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(3);

            int usina = 0;
            string nome = "";
            foreach (var line in lines) {
                var newLine = Blocos["Exph"].CreateLine(line);
                if (string.IsNullOrEmpty(line) || (newLine[1] is int && newLine[1] > 320)) continue;

                if (newLine[1] is int ) { usina = (int)newLine[1]; nome = newLine[2]; }

                newLine[0] = usina;
                newLine[12] = nome;

                Blocos["Exph"].Add(newLine);
            }
        }

        public IEnumerator<ExphLine> GetEnumerator() {
            return ((ExphBlock)Blocos["Exph"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((ExphBlock)Blocos["Exph"]).GetEnumerator();
        }


        public int IndexOf(ExphLine item) {
            return ((ExphBlock)Blocos["Exph"]).IndexOf(item);
        }

        public void Insert(int index, ExphLine item) {
            ((ExphBlock)Blocos["Exph"]).Insert(index, item);
        }

        public void RemoveAt(int index) {
            ((ExphBlock)Blocos["Exph"]).RemoveAt(index);
        }

        public ExphLine this[int index] {
            get {
                return ((ExphBlock)Blocos["Exph"])[index];
            }
            set {
                ((ExphBlock)Blocos["Exph"])[index] = value;
            }
        }

        public void Add(ExphLine item) {
            ((ExphBlock)Blocos["Exph"]).Add(item);
        }

        public void Clear() {
            ((ExphBlock)Blocos["Exph"]).Clear();
        }

        public bool Contains(ExphLine item) {
            return ((ExphBlock)Blocos["Exph"]).Contains(item);
        }

        public void CopyTo(ExphLine[] array, int arrayIndex) {
            ((ExphBlock)Blocos["Exph"]).CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return ((ExphBlock)Blocos["Exph"]).Count(); }
        }

        public bool IsReadOnly {
            get { return ((ExphBlock)Blocos["Exph"]).IsReadOnly; }
        }

        public bool Remove(ExphLine item) {
            return ((ExphBlock)Blocos["Exph"]).Remove(item);
        }
    }
    public class ExphBlock : BaseBlock<ExphLine> {
        string header =
@"COD  NOME        ENCHIMENTO  VOLUME MORTO    DATA    POT.
                  INICIO    DUR.MESES  %    ENTRADA
XXXX XXXXXXXXXXXX XX/XXXX      XX     XX.X  XX/XXXX XXXX.X"
;

        public override string ToText() {

            var result = new StringBuilder();

            foreach (var item in this.GroupBy(x => x.Cod)) {

                result.Append("\r\n");

                item.First()[1] = item.First().Cod;
                item.First()[2] = item.First()[12];

                foreach (var item2 in item) {
                    result.AppendLine(item2.ToText());
                }

                result.Append("9999");
            }

            var txt = header + result.ToString();

            return txt;
        }
    }
    public class ExphLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {   
                new BaseField(0,0, "I4", "Cod"),                
                new BaseField(1,4, "I4", "Usina"),
                new BaseField(6 , 17 ,"A12"   , "Nome"),
                new BaseField(19  , 20 ,"I2"  , "MesInicioEnchimento"),
                new BaseField(22  , 25 ,"I4"  , "AnoInicioEnchimento"),
                new BaseField(32  , 33 ,"I2"  , "DuracaoEnchimento"),
                new BaseField(38  , 42 ,"F5.1", "VolumeMortoPreenchido"),

                new BaseField(45  , 46 ,"I2"  , "MesEntrada"),
                new BaseField(48  , 51 ,"I3"  , "AnoEntrada"),

                new BaseField(53  , 58 ,"F5.1", "Potencia"),

                new BaseField(61  , 62 ,"I2"  , "NumeroUnidade"),
                new BaseField(65  , 65 ,"I1"  , "NumeroConjunto"),

                new BaseField(0,0, "A12", "Nome"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Cod { get { return valores[campos[0]]; } }

        public DateTime? DataEntrada {
            get {

                if (this[7] == null)
                    return null;
                else
                    return new DateTime(this[8], this[7], 1);
            }
        }
        public int NumMaq { get { return this[10]; } }
        public int NumConj { get { return this[11]; } }


        public DateTime? DataEnchimento {
            get {
                if (this[3] != null && this[4] != null) {
                    return new DateTime((int)this[4], (int)this[3], 1);
                } else return null;
            }

            set {
                if (value.HasValue) {
                    this[4] = value.Value.Year;
                    this[3] = value.Value.Month;
                } else
                    this[4] = this[3] = null;

            }
        }

        public int DuracaoEnchimento { get { return this[5]; } set { this[5] = value; } }

        public double VolumePreenchido { get { return this[6]; } set { this[6] = value; } }
    }
}

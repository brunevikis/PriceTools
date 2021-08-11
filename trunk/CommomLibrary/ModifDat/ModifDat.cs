using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ModifDat {
    public class ModifDat : BaseDocument, IList<ModifLine> {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Modif"             , new ModifBlock()},                 
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }




        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            foreach (var line in lines) {
                var newLine = Blocos["Modif"].CreateLine(line);
                Blocos["Modif"].Add(newLine);
            }
        }

        public int IndexOf(ModifLine item) {
            return (Blocos["Modif"] as ModifBlock).IndexOf(item);
        }

        public void Insert(int index, ModifLine item) {
            (Blocos["Modif"] as ModifBlock).Insert(index, item);
        }

        public void RemoveAt(int index) {
            (Blocos["Modif"] as ModifBlock).RemoveAt(index);
        }

        public ModifLine this[int index] {
            get {
                return (Blocos["Modif"] as ModifBlock)[index];
            }
            set {
                (Blocos["Modif"] as ModifBlock)[index] = value;
            }
        }

        public void Add(ModifLine item) {
            (Blocos["Modif"] as ModifBlock).Add(item);
        }

        public void Clear() {
            (Blocos["Modif"] as ModifBlock).Clear();
        }

        public bool Contains(ModifLine item) {
            return (Blocos["Modif"] as ModifBlock).Contains(item);
        }

        public void CopyTo(ModifLine[] array, int arrayIndex) {
            (Blocos["Modif"] as ModifBlock).CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return (Blocos["Modif"] as ModifBlock).Count; }
        }

        public bool IsReadOnly {
            get { return (Blocos["Modif"] as ModifBlock).IsReadOnly; }
        }

        public bool Remove(ModifLine item) {
            return (Blocos["Modif"] as ModifBlock).Remove(item); ;
        }

        public IEnumerator<ModifLine> GetEnumerator() {
            return (Blocos["Modif"] as ModifBlock).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return (Blocos["Modif"] as ModifBlock).GetEnumerator();
        }
    }
    public class ModifBlock : BaseBlock<ModifLine> {
        string header =
@" P.CHAV USI  MODIFICACOES
 XXXXXX XXX XXX XXX
"
;

        public override string ToText() {

            return header + base.ToText();
        }
    }
    public class ModifLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 2 , 7 ,"A6"  , "Tipo"),
                new BaseField(9  , 11 ,"I3"  , "Usina"),
                new BaseField(13  , 15 ,"I3"  , "Valor1"),
                new BaseField(17  , 19 ,"I3"  , "Valor2"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

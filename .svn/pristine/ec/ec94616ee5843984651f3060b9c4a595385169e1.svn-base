using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ModifDatNW
{
    public class ModifDatNw : BaseDocument, IList<ModifLine>
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Modif"             , new ModifBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }

        public override void Load(string fileContent)
        {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            int usina = 0;
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {

                    var newLine = Blocos["Modif"].CreateLine(line);

                    if (newLine[1].Trim() == "USINA") usina = int.Parse(newLine[2].Substring(0, 5).Trim());
                    newLine[0] = usina;

                    Blocos["Modif"].Add(newLine);
                }
            }
        }

        public IEnumerator<ModifLine> GetEnumerator()
        {
            return ((ModifBlock)Blocos["Modif"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((ModifBlock)Blocos["Modif"]).GetEnumerator();
        }

        public int IndexOf(ModifLine item)
        {
            return ((ModifBlock)Blocos["Modif"]).IndexOf(item);
        }

        public void Insert(int index, ModifLine item)
        {
            ((ModifBlock)Blocos["Modif"]).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((ModifBlock)Blocos["Modif"]).RemoveAt(index);
        }

        public ModifLine this[int index]
        {
            get
            {
                return ((ModifBlock)Blocos["Modif"])[index];
            }
            set
            {
                ((ModifBlock)Blocos["Modif"])[index] = value;
            }
        }

        public void Add(ModifLine item)
        {
            ((ModifBlock)Blocos["Modif"]).Add(item);
        }

        public void Clear()
        {
            ((ModifBlock)Blocos["Modif"]).Clear();
        }

        public bool Contains(ModifLine item)
        {
            return ((ModifBlock)Blocos["Modif"]).Contains(item);
        }

        public void CopyTo(ModifLine[] array, int arrayIndex)
        {
            ((ModifBlock)Blocos["Modif"]).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return ((ModifBlock)Blocos["Modif"]).Count(); }
        }

        public bool IsReadOnly
        {
            get { return ((ModifBlock)Blocos["Modif"]).IsReadOnly; }
        }

        public bool Remove(ModifLine item)
        {
            return ((ModifBlock)Blocos["Modif"]).Remove(item);
        }

        public ModifLine CreateLine()
        {
            return ((ModifBlock)Blocos["Modif"]).CreateLine();
        }
    }
    public class ModifBlock : BaseBlock<ModifLine>
    {
        string header =
@" P.CHAVE  MODIFICACOES E INDICES
 XXXXXXXX XXXXXXXXXXXXXXXXXXXXX
"
;

        public override string ToText()
        {
            var txt = header + base.ToText();

            if (txt.EndsWith("\r\n")) txt = txt.Substring(0, txt.Length - 2);

            return txt;
        }
    }
    public class ModifLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(0,0, "I3", "Usina"),
                new BaseField(2 , 9 ,"A8"  , "Tipo"),
                new BaseField(11  , 70 ,"A60"  , "Valor"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Usina { get { return valores[campos[0]]; } set { valores[campos[0]] = value; } }
        public string Chave { get { return valores[campos[1]].Trim().ToUpper(); } set { valores[campos[1]] = value; } }
        public string[] NovosValores
        {
            get
            {
                return ((string)valores[campos[2]]).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            set
            {
                valores[campos[2]] = " " + string.Join(" ", value);
            }
        }

        static BaseField bf = new BaseField();
        public void SetValores(params object[] val)
        {
            var arr = new string[val.Length];

            for (int i = 0; i < arr.Length; i++)
            {

                if (val[i] is float || val[i] is double || val[i] is decimal)
                {
                    bf.Formato = "F3";
                    arr[i] = bf.ConvertToString(val[i]);
                }
                else
                    arr[i] = val[i].ToString();

            }

            NovosValores = arr;
        }

        public DateTime DataModif
        {
            get
            {

                int m, a;

                if (NovosValores.Length < 3 || !int.TryParse(NovosValores[0], out m) || !int.TryParse(NovosValores[1], out a)) return DateTime.MinValue;
                if (m > 12 || a < 2000) return DateTime.MinValue;

                return new DateTime(a, m, 1);
            }
            set
            {
                if (DataModif != DateTime.MinValue)
                {
                    var val = NovosValores;

                    val[0] = value.Month.ToString("00");
                    val[1] = value.Year.ToString("0000");

                    NovosValores = val;

                }

            }
        }

        public double? ValorModif
        {
            get
            {
                if (double.TryParse(NovosValores[2], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out double val))
                {
                    return val;
                }
                else
                    return null;

            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.GhminDat
{
    public class GhminDat : BaseDocument, IQueryable<GhminLine>, IList<GhminLine>
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Ghmin"             , new GhminBlock()},
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

            foreach (var line in lines)
            {
                var newLine = Blocos["Ghmin"].CreateLine(line);
                if (newLine[0] != null)
                {
                    Blocos["Ghmin"].Add(newLine);
                }
            }
        }

        public IEnumerator<GhminLine> GetEnumerator()
        {
            return ((GhminBlock)Blocos["Ghmin"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((GhminBlock)Blocos["Ghmin"]).GetEnumerator();
        }

        public Type ElementType
        {
            get { return ((GhminBlock)Blocos["Ghmin"]).AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return ((GhminBlock)Blocos["Ghmin"]).AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return ((GhminBlock)Blocos["Ghmin"]).AsQueryable().Provider; }
        }

        public int IndexOf(GhminLine item)
        {
            return ((GhminBlock)Blocos["Ghmin"]).IndexOf(item);
        }

        public void Insert(int index, GhminLine item)
        {
            ((GhminBlock)Blocos["Ghmin"]).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((GhminBlock)Blocos["Ghmin"]).RemoveAt(index);
        }

        public GhminLine this[int index]
        {
            get
            {
                return ((GhminBlock)Blocos["Ghmin"])[index];
            }
            set
            {
                ((GhminBlock)Blocos["Ghmin"])[index] = value;
            }
        }

        public void Add(GhminLine item)
        {
            ((GhminBlock)Blocos["Ghmin"]).Add(item);
        }

        public void Clear()
        {
            ((GhminBlock)Blocos["Ghmin"]).Clear();
        }

        public bool Contains(GhminLine item)
        {
            return ((GhminBlock)Blocos["Ghmin"]).Contains(item);
        }

        public void CopyTo(GhminLine[] array, int arrayIndex)
        {
            ((GhminBlock)Blocos["Ghmin"]).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return ((GhminBlock)Blocos["Ghmin"]).Count(); }
        }

        public bool IsReadOnly
        {
            get { return ((GhminBlock)Blocos["Ghmin"]).IsReadOnly; }
        }

        public bool Remove(GhminLine item)
        {
            return ((GhminBlock)Blocos["Ghmin"]).Remove(item);
        }
    }
    public class GhminBlock : BaseBlock<GhminLine>
    {
        string header =
@"UH   ME ANO   P  MWmedio
XXX  XX XXXX  X  XXXX.X
"
;

        public override string ToText()
        {

            return header + base.ToText();
        }
    }
    public class GhminLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(1 , 3 ,"I3"  , "Cod"),
                new BaseField(6 , 7 ,"I2"  , "Mes"),
                new BaseField(9 , 12 ,"A4"  , "Ano"),
                new BaseField(15, 15 ,"I1"  , "Patamar"),
                new BaseField(18 , 23 ,"F5.1"  , "Potencia"),

        };

        public DateTime? Data
        {
            get
            {
                if (valores[campos[1]] is int m)
                if(int.TryParse(valores[campos[2]], out int a))
                {
                    return new DateTime(a, m, 1);
                }

                return null;
            }
        }

        public int Cod
        {
            get
            {

                return valores[campos[0]];

            }
            set
            {
                valores[campos[0]] = value;
            }
        }

        public int Patamar
        {
            get { return valores[campos[3]]; }
            set { valores[campos[3]] = value; }
        }

        public double Potencia
        {
            get { return valores[campos[4]]; }
            set { valores[campos[4]] = value; }
        }

        public override BaseField[] Campos
        {
            get { return campos; }
        }
    }
}

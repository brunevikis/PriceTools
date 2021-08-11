using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ReDat
{
    public class ReDat : BaseDocument, IList<ReLine>
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Re"             , new ReBlock()},
                    {"Valores"             , new ReValBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }

        public ReBlock Restricoes { get { return (ReBlock)Blocos["Re"]; } }
        public ReValBlock Detalhes { get { return (ReValBlock)Blocos["Valores"]; } }


        public Dictionary<ReLine, IEnumerable<ReValLine>> this[DateTime dt]
        {
            get
            {
                var vals = Detalhes.Where(x => x.Inicio <= dt && x.Fim >= dt);
                return vals.GroupBy(x => x.Numero).ToDictionary(x => this.Restricoes.First(y => y.Numero == x.Key), x => x.Select(y => y).OrderBy(y => y.Patamar).AsEnumerable());
                //return vals.ToDictionary(x => this.Restricoes.First(y => y.Numero == x.Numero));
            }
        }

        public override void Load(string fileContent)
        {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2).ToList();

            int i = 0;

            for (; i < lines.Count && !lines[i].StartsWith("999"); i++)
            {
                var newLine = Restricoes.CreateLine(lines[i]);
                Restricoes.Add(newLine);
            }

            i += 3;
            for (; i < lines.Count && !lines[i].StartsWith("999"); i++)
            {
                var newLine = Detalhes.CreateLine(lines[i]);
                Detalhes.Add(newLine);
            }
        }

        public int IndexOf(ReLine item)
        {
            return (Blocos["Re"] as ReBlock).IndexOf(item);
        }

        public void Insert(int index, ReLine item)
        {
            (Blocos["Re"] as ReBlock).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            (Blocos["Re"] as ReBlock).RemoveAt(index);
        }

        public ReLine this[int index]
        {
            get
            {
                return (Blocos["Re"] as ReBlock)[index];
            }
            set
            {
                (Blocos["Re"] as ReBlock)[index] = value;
            }
        }

        public void Add(ReLine item)
        {
            (Blocos["Re"] as ReBlock).Add(item);
        }

        public void Clear()
        {
            (Blocos["Re"] as ReBlock).Clear();
        }

        public bool Contains(ReLine item)
        {
            return (Blocos["Re"] as ReBlock).Contains(item);
        }

        public void CopyTo(ReLine[] array, int arrayIndex)
        {
            (Blocos["Re"] as ReBlock).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return (Blocos["Re"] as ReBlock).Count; }
        }

        public bool IsReadOnly
        {
            get { return (Blocos["Re"] as ReBlock).IsReadOnly; }
        }

        public bool Remove(ReLine item)
        {
            return (Blocos["Re"] as ReBlock).Remove(item); ;
        }

        public IEnumerator<ReLine> GetEnumerator()
        {
            return (Blocos["Re"] as ReBlock).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Blocos["Re"] as ReBlock).GetEnumerator();
        }
    }
    public class ReBlock : BaseBlock<ReLine>
    {
        string header =
@"RES   USINAS PERTENCENTES AO CONJUNTO
XXX   XXX XXX XXX XXX XXX XXX XXX XXX XXX XXX
"
;

        public override string ToText()
        {
            return header + base.ToText() + "999\r\n";
        }
    }

    public class ReValBlock : BaseBlock<ReValLine>
    {
        string header =
@"RES MM/AAAA MM/AAAA P       RESTRICAO
XXX XX XXXX XX XXXX X XXXXXXXXXXXXXXX
"
;
        public override string ToText()
        {
            return header + base.ToText() + "999\r\n";
        }
    }

    public class ReLine : BaseLine
    {

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(1  , 3 ,"I3"  , "Numero"),
                new BaseField(7  , 9,"I3"  ,  "Usina 1"),
                new BaseField(11  ,13 ,"I3"  , "Usina 2"),
                new BaseField(15  ,17 ,"I3"  , "Usina 3"),
                new BaseField(19  ,21 ,"I3"  , "Usina 4"),
                new BaseField(23  ,25 ,"I3"  , "Usina 5"),
                new BaseField(27  ,29 ,"I3"  , "Usina 6"),
                new BaseField(31  ,33 ,"I3"  , "Usina 7"),
                new BaseField(35  ,37 ,"I3"  , "Usina 8"),
                new BaseField(39  ,41 ,"I3"  , "Usina 9"),
                new BaseField(43  ,45 ,"I3"  , "Usina 10"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Numero { get { return this[0]; } set { this[0] = value; } }

        public string Chave
        {
            get
            {
                return string.Join(";", Valores.Skip(1).Where(x => x is int).OrderBy(x => x));
            }
        }
    }

    public class ReValLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(1  , 3 ,"I3"  , "Numero"),
                new BaseField(5  , 6 , "I2"  ,  "Mes Ini"),
                new BaseField(8  ,11 ,"I4"  , "Ano Ini"),
                new BaseField(13  ,14 ,"I2"  , "Mes Fim"),
                new BaseField(16  ,19 ,"I4"  , "Ano Fim"),
                new BaseField(21  ,21 ,"I1"  , "P"),
                new BaseField(23  ,32 ,"F10.2"  , "Valor"),
                new BaseField(39  ,59 ,"A21"  , "Descricao"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Numero { get { return this[0]; } set { this[0] = value; } }

        public int Patamar { get { return this[5]; } set { this[5] = value; } }

        public DateTime Inicio
        {
            get { return new DateTime((int)this[2], (int)this[1], 1); }
            set
            {
                this[2] = value.Year;
                this[1] = value.Month;
            }
        }
        public DateTime Fim
        {
            get { return new DateTime((int)this[4], (int)this[3], 1); }
            set
            {
                this[4] = value.Year;
                this[3] = value.Month;
            }
        }

        public double ValorRestricao
        {
            get { return this[6]; }
            set { this[6] = value; }
        }


    }
}

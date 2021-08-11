using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.AgrintDat
{
    public class AgrintDat : BaseDocument, IList<AgrintLine>
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Agrint"             , new AgrintBlock()},
                    {"Valores"             , new AgrintValBlock()},
                };
        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public AgrintBlock Agrupamentos { get { return (AgrintBlock)Blocos["Agrint"]; } }
        public AgrintValBlock Detalhes { get { return (AgrintValBlock)Blocos["Valores"]; } }
        public Dictionary<IEnumerable<AgrintLine>, AgrintValLine> this[DateTime dt]
        {
            get
            {
                var vals = Detalhes.Where(x => x.Inicio <= dt && x.Fim >= dt);
                //return vals.  .GroupBy(x => x.Numero).ToDictionary(x => this.Agrupamentos.First(y => y.Numero == x.Key), x => x.Select(y => y).AsEnumerable());
                return vals.ToDictionary(x => this.Agrupamentos.Where(y => y.Numero == x.Numero).AsEnumerable());
            }
        }
        public override void Load(string fileContent)
        {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(3).ToList();

            int i = 0;

            for (; i < lines.Count && !lines[i].StartsWith(" 999"); i++)
            {
                var newLine = Agrupamentos.CreateLine(lines[i]);
                Agrupamentos.Add(newLine);
            }

            i += 4;
            for (; i < lines.Count && !lines[i].StartsWith(" 999"); i++)
            {
                var newLine = Detalhes.CreateLine(lines[i]);
                Detalhes.Add(newLine);
            }
        }
        public int IndexOf(AgrintLine item)
        {
            return (Blocos["Re"] as AgrintBlock).IndexOf(item);
        }
        public void Insert(int index, AgrintLine item)
        {
            (Blocos["Re"] as AgrintBlock).Insert(index, item);
        }
        public void RemoveAt(int index)
        {
            (Blocos["Re"] as AgrintBlock).RemoveAt(index);
        }
        public AgrintLine this[int index]
        {
            get
            {
                return (Blocos["Re"] as AgrintBlock)[index];
            }
            set
            {
                (Blocos["Re"] as AgrintBlock)[index] = value;
            }
        }
        public void Add(AgrintLine item)
        {
            (Blocos["Re"] as AgrintBlock).Add(item);
        }
        public void Clear()
        {
            (Blocos["Re"] as AgrintBlock).Clear();
        }
        public bool Contains(AgrintLine item)
        {
            return (Blocos["Re"] as AgrintBlock).Contains(item);
        }
        public void CopyTo(AgrintLine[] array, int arrayIndex)
        {
            (Blocos["Re"] as AgrintBlock).CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get { return (Blocos["Re"] as AgrintBlock).Count; }
        }
        public bool IsReadOnly
        {
            get { return (Blocos["Re"] as AgrintBlock).IsReadOnly; }
        }
        public bool Remove(AgrintLine item)
        {
            return (Blocos["Re"] as AgrintBlock).Remove(item); ;
        }
        public IEnumerator<AgrintLine> GetEnumerator()
        {
            return (Blocos["Re"] as AgrintBlock).GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Blocos["Re"] as AgrintBlock).GetEnumerator();
        }
    }
    public class AgrintBlock : BaseBlock<AgrintLine>
    {
        string header =
@"AGRUPAMENTOS DE INTERCÂMBIO
 #AG A   B   COEF
 XXX XXX XXX XX.XXXX
"
;

        public override string ToText()
        {
            return header + base.ToText() + " 999\r\n";
        }
    }
    public class AgrintValBlock : BaseBlock<AgrintValLine>
    {
        string header =
@"LIMITES POR GRUPO
  #AG MI ANOI MF ANOF LIM_P1  LIM_P2  LIM_P3
 XXX  XX XXXX XX XXXX XXXXXX. XXXXXX. XXXXXX.
"
;
        public override string ToText()
        {
            return header + base.ToText() + " 999\r\n";
        }
    }
    public class AgrintLine : BaseLine
    {

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(2  , 4 ,"I3"  , "Numero"),
                new BaseField(6  , 8,"I3"  ,  "A"),
                new BaseField(10  ,12 ,"I3"  , "B"),
                new BaseField(14  ,20 ,"F6.4"  , "Coef"),

        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Numero { get { return this[0]; } set { this[0] = value; } }

        public int SistemaA { get { return this[1]; } set { this[1] = value; } }
        public int SistemaB { get { return this[2]; } set { this[2] = value; } }

        public double Coef { get { return this[3]; } set { this[3] = value; } }
    }

    public class AgrintValLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(2  , 4 ,  "I3"  , "Numero"),
                new BaseField(7  , 8 ,  "I2"  ,  "Mes Ini"),
                new BaseField(10  ,13 , "I4"  , "Ano Ini"),
                new BaseField(15  ,16 , "I2"  , "Mes Fim"),
                new BaseField(18  ,21 , "I4"  , "Ano Fim"),
                new BaseField(23  ,29 , "F6.0"  , "Valor"),
                new BaseField(31  ,37 , "F6.0"  , "Valor"),
                new BaseField(39  ,45 , "F6.0"  , "Valor"),
                new BaseField(50  ,100 ,"A51"  , "Descricao"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Numero { get { return this[0]; } set { this[0] = value; } }

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
            get
            {
                if (this[4] is int && this[3] is int)
                    return new DateTime((int)this[4], (int)this[3], 1);
                else return DateTime.MaxValue;
            }
            set
            {

                if (value == DateTime.MaxValue) this[4] = this[3] = "";
                else
                {
                    this[4] = value.Year;
                    this[3] = value.Month;
                }
            }
        }

        public double Lim_P1 { get { return this[5]; } set { this[5] = value; } }
        public double Lim_P2 { get { return this[6]; } set { this[6] = value; } }
        public double Lim_P3 { get { return this[7]; } set { this[7] = value; } }
        public string Descricao { get { return this[8]; } set { this[8] = value; } }



    }
}

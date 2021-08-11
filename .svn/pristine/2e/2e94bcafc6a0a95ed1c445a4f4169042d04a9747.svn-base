using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ClastDat
{
    public class ClastDat : BaseDocument, IQueryable<ClastLine>
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Clast"             , new ClastBlock()},
                    {"Modif"             , new ModifBlock()},
                };

        public ModifBlock Modifs { get { return (ModifBlock)Blocos["Modif"]; } }

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }

        public override void Load(string fileContent)
        {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2).ToArray();

            int i = 0;

            for (; i < lines.Length && lines[i].Trim() != "9999"; i++)
            {

                var newLine = Blocos["Clast"].CreateLine(lines[i]);
                if (newLine[0] != null)
                {
                    Blocos["Clast"].Add(newLine);
                }
            }
            i += 2;
            for (; i < lines.Length; i++)
            {

                var newLine = Modifs.CreateLine(lines[i]);
                if (newLine[0] != null)
                {
                    Modifs.Add(newLine);
                }
            }

        }

        public IEnumerator<ClastLine> GetEnumerator()
        {
            return ((ClastBlock)Blocos["Clast"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((ClastBlock)Blocos["Clast"]).GetEnumerator();
        }

        public Type ElementType
        {
            get { return ((ClastBlock)Blocos["Clast"]).AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return ((ClastBlock)Blocos["Clast"]).AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return ((ClastBlock)Blocos["Clast"]).AsQueryable().Provider; }
        }

        public List<ClastLine> this[DateTime data]
        {
            get
            {

                var subModifs = Modifs.Where(z => z.Inicio <= data && z.Fim >= data);

                return ((ClastBlock)Blocos["Clast"]).Select(x =>
                 {
                     var y = x.Clone() as ClastLine;
                     if (subModifs.Any(z => z.Num == y.Num)) y.Cvu1 = subModifs.First(z => z.Num == y.Num).Cvu;
                     return y;
                 }).ToList();


            }
        }



    }
    public class ClastBlock : BaseBlock<ClastLine>
    {
        string header =
@" NUM  NOME CLASSE  TIPO COMB.  CUSTO   CUSTO   CUSTO   CUSTO   CUSTO
 XXXX XXXXXXXXXXXX XXXXXXXXXX XXXX.XX XXXX.XX XXXX.XX XXXX.XX XXXX.XX
"
;

        public override string ToText()
        {

            return header + base.ToText() + " 9999\r\n";
        }
    }
    public class ClastLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(2    , 5 ,"I4"  , "Num"),
                new BaseField(7    , 18 ,"A12" , "Nome"),
                new BaseField(20   , 29,"A10"  , "Combustivel"),
                new BaseField(31   , 37 ,"F7.2"  , "CVU1"),
                new BaseField(39   , 45 ,"F7.2"  , "CVU2"),
                new BaseField(47   , 53 ,"F7.2"  , "CVU3"),
                new BaseField(55   , 61 ,"F7.2"  , "CVU4"),
                new BaseField(63   , 69 ,"F7.2"  , "CVU5"),

        };
        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Num
        {
            get
            {
                return valores[campos[0]]; ;
            }
        }

        public string Nome { get { return valores[campos[1]]; } }

        public string Combustivel { get { return valores[campos[2]].Trim(); } }


        public double Cvu1 { get { return valores[campos[3]]; } set { valores[campos[3]] = value; } }

    }
    public class ModifBlock : BaseBlock<ModifLine>
    {
        string header =
@" NUM     CUSTO
 XXXX   XXXX.XX  XX XXXX  XX XXXX
"
;

        public override string ToText()
        {

            return header + base.ToText();
        }
    }
    public class ModifLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(2    , 5 ,"I4"  , "Num"),
                new BaseField(9   , 15 ,"F7.2"  , "CVU"),
                new BaseField(18   , 19,"I2"  , "Mes Inicio"),
                new BaseField(21   , 24 ,"I4"  , "Ano Inicio"),
                new BaseField(27   , 28 ,"I2"  , "Mes Fim"),
                new BaseField(30   , 33 ,"I4"  , "Ano Fim"),
                new BaseField(36   , 47 ,"A12" , "Nome"),
        };
        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Num
        {
            get
            {
                return valores[campos[0]]; ;
            }
        }

        public DateTime Inicio
        {
            get { return new DateTime((int)this[3], (int)this[2], 1); }
            set
            {
                this[3] = value.Year;
                this[2] = value.Month;
            }
        }
        public DateTime Fim
        {
            get
            {
                if (this[5] is int && this[4] is int)
                    return new DateTime((int)this[5], (int)this[4], 1);
                else return DateTime.MaxValue;
            }
            set
            {

                if (value == DateTime.MaxValue) this[5] = this[4] = "";
                else
                {
                    this[5] = value.Year;
                    this[4] = value.Month;
                }
            }
        }

        public double Cvu { get { return valores[campos[1]]; } }

    }

}

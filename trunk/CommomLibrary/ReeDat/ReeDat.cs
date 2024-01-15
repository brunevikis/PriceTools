using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ReeDat
{
    public class ReeDat : BaseDocument, IList<ReeLine>
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Ree"             , new ReeBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }

        public bool temFict = false;
        public override void Load(string fileContent)
        {

            string isfictline = "";


            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(3);
            if (lines.Any(x => x.Trim().StartsWith("FICT.")))
            {
                isfictline = lines.Where(x => x.Trim().StartsWith("FICT.")).First();
                temFict = true;
            }
            foreach (var line in lines)
            {

                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Trim().StartsWith("FICT."))
                {
                    continue;
                }
                var newLine = Blocos["Ree"].CreateLine(line);
                if (((ReeLine)newLine).Numero != 999)
                {
                    newLine.FictLine = isfictline;
                    Blocos["Ree"].Add(newLine);

                }
            }
        }

        public int IndexOf(ReeLine item)
        {
            return (Blocos["Ree"] as ReeBlock).IndexOf(item);
        }

        public void Insert(int index, ReeLine item)
        {
            (Blocos["Ree"] as ReeBlock).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            (Blocos["Ree"] as ReeBlock).RemoveAt(index);
        }

        public ReeLine this[int index]
        {
            get
            {
                return (Blocos["Ree"] as ReeBlock)[index];
            }
            set
            {
                (Blocos["Ree"] as ReeBlock)[index] = value;
            }
        }

        public void Add(ReeLine item)
        {
            (Blocos["Ree"] as ReeBlock).Add(item);
        }

        public void Clear()
        {
            (Blocos["Ree"] as ReeBlock).Clear();
        }

        public bool Contains(ReeLine item)
        {
            return (Blocos["Ree"] as ReeBlock).Contains(item);
        }

        public void CopyTo(ReeLine[] array, int arrayIndex)
        {
            (Blocos["Ree"] as ReeBlock).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return (Blocos["Ree"] as ReeBlock).Count; }
        }

        public bool IsReadOnly
        {
            get { return (Blocos["Ree"] as ReeBlock).IsReadOnly; }
        }

        public bool Remove(ReeLine item)
        {
            return (Blocos["Ree"] as ReeBlock).Remove(item); ;
        }

        public IEnumerator<ReeLine> GetEnumerator()
        {
            return (Blocos["Ree"] as ReeBlock).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Blocos["Ree"] as ReeBlock).GetEnumerator();
        }
    }

    public class ReeBlock : BaseBlock<ReeLine>
    {
//        string header =
//@" REES X SUBMERCADOS
// NUM|NOME REES.| SUBM
// XXX|XXXXXXXXXX|  XXX
//"
//;
        string header =
            @" REES X SUBMERCADOS
 NUM|NOME REES.| SUBM|MES| ANO
 XXX|XXXXXXXXXX|  XXX| XX|XXXX
";


        public override string ToText()
        {
            string temFictLine = base.fictLine();
            if (temFictLine.Trim().StartsWith("FICT"))
            {
                return header + base.ToText() + " 999\r\n" + temFictLine + "\r";
            }
            return header + base.ToText() + " 999\r";
        }
    }
    public class ReeLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(2  , 4 ,"I3"  , "Numero"),
                new BaseField(6  , 15 ,"A10"  , "Nome"),
                new BaseField(19  , 21 ,"I3"  , "Submercado"),
                new BaseField(24  , 25 ,"I2"  , "Mes"),
                new BaseField(27  , 30 ,"I4"  , "Ano"),


        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }
        public int Numero { get { return this[0]; } set { this[0] = value; } }
        public string Nome { get { return this[1]; } set { this[1] = value; } }
        public int Submercado { get { return this[2]; } set { this[2] = value; } }
        public int Mes { get { return this[3]; } set { this[3] = value; } }
        public int Ano { get { return this[4]; } set { this[4] = value; } }


    }
}

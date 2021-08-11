using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.AdtermDat
{
    public class AdtermDat : BaseDocument, IList<AdtermLine>
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Adterm"             , new AdtermBlock()},
                };

        public AdtermBlock Despachos { get { return (AdtermBlock)Blocos["Adterm"]; } }

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }

        public override void Load(string fileContent)
        {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2).ToList();

            

            for (int i =0; i < lines.Count && !lines[i].StartsWith(" 9999"); i++)
            {
                var newLine = Despachos.CreateLine(lines[i]);
                Despachos.Add(newLine);
            }

           
        }
        public int IndexOf(AdtermLine item)
        {
            return (Blocos["Adterm"] as AdtermBlock).IndexOf(item);
        }
        public void Insert(int index, AdtermLine item)
        {
            (Blocos["Adterm"] as AdtermBlock).Insert(index, item);
        }
        public void RemoveAt(int index)
        {
            (Blocos["Adterm"] as AdtermBlock).RemoveAt(index);
        }
        public AdtermLine this[int index]
        {
            get
            {
                return (Blocos["Adterm"] as AdtermBlock)[index];
            }
            set
            {
                (Blocos["Adterm"] as AdtermBlock)[index] = value;
            }
        }
        public void Add(AdtermLine item)
        {
            (Blocos["Adterm"] as AdtermBlock).Add(item);
        }
        public void Clear()
        {
            (Blocos["Adterm"] as AdtermBlock).Clear();
        }
        public bool Contains(AdtermLine item)
        {
            return (Blocos["Adterm"] as AdtermBlock).Contains(item);
        }
        public void CopyTo(AdtermLine[] array, int arrayIndex)
        {
            (Blocos["Adterm"] as AdtermBlock).CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get { return (Blocos["Adterm"] as AdtermBlock).Count; }
        }
        public bool IsReadOnly
        {
            get { return (Blocos["Adterm"] as AdtermBlock).IsReadOnly; }
        }
        public bool Remove(AdtermLine item)
        {
            return (Blocos["Adterm"] as AdtermBlock).Remove(item); ;
        }
        public IEnumerator<AdtermLine> GetEnumerator()
        {
            return (Blocos["Adterm"] as AdtermBlock).GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Blocos["Adterm"] as AdtermBlock).GetEnumerator();
        }
    }
    public class AdtermBlock : BaseBlock<AdtermLine>
    {
        string header =
@"IUTE  NOME TERMICA LAG
XXXX  XXXXXXXXXXXX  X  XXXXXXX.XX  XXXXXXX.XX  XXXXXXX.XX
"
;

        public override string ToText()
        {
            return header + base.ToText() + " 9999\r\n";
        }
    }
    
    public class AdtermLine : BaseLine
    {

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(2  , 5 ,"I4"  , "Numero"),
                new BaseField(8  , 19,"A12"  ,  "Nome"),
                new BaseField(22  ,22 ,"I1"  , "Nlag"),
                new BaseField(25  ,34 ,"F10.2"  , "Patamar1"),
                new BaseField(37  ,46 ,"F10.2"  , "Patamar2"),
                new BaseField(49  ,58 ,"F10.2"  , "Patamar3"),
                new BaseField(61  ,70 ,"F10.2"  , "Patamar4"),
                new BaseField(73  ,82 ,"F10.2"  , "Patamar5"),


        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public double Numero { get { return this[0]; } set { this[0] = value; } }

        public string String { get { return this[1]; } set { this[1] = value; } }
        public int Lag { get { return this[2]; } set { this[2] = value; } }

        public double Lim_P1 { get { return this[3]; } set { this[3] = value; } }
        public double Lim_P2 { get { return this[4]; } set { this[4] = value; } }
        public double Lim_P3 { get { return this[5]; } set { this[5] = value; } }
        public double Lim_P4 { get { return this[6]; } set { this[6] = value; } }
        public double Lim_P5 { get { return this[7]; } set { this[7] = value; } }
    }
    
    
    
}

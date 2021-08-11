using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Previvaz {
    public class Inp : BaseDocument {
        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get { throw new NotImplementedException(); }
        }


        string[] Lines = null;
        public Inp(string file) {

            Lines = System.IO.File.ReadAllLines(file);
            this.File = file;

        }

        public override string ToText() {
            return string.Join("\r\n", Lines);
        }

        public string FutFile { get { return Lines[5].PadLeft(12).Substring(0, 12).Trim(); } set { Lines[5] = Lines[5].PadLeft(12).Remove(0, 12).Insert(0, value.PadRight(12)); } }
        public string StrFile { get { return Lines[2].PadLeft(12).Substring(0, 12).Trim(); } set { Lines[2] = Lines[2].PadLeft(12).Remove(0, 12).Insert(0, value.PadRight(12)); } }
        public string LimFile { get { return Lines[15].PadLeft(12).Substring(0, 12).Trim(); } set { Lines[15] = Lines[15].PadLeft(12).Remove(0, 12).Insert(0, value.PadRight(12)); } }
        public int SemanaPrevisao { get { return int.Parse(Lines[8].PadLeft(2).Substring(0, 2).Trim()); } set { Lines[8] = value.ToString().PadLeft(2); } }
        public int AnoPrevisao { get { return int.Parse(Lines[9].PadLeft(4).Substring(0, 4).Trim()); } set { Lines[9] = Lines[9].Remove(0, 4).Insert(0, value.ToString().PadLeft(4)); } }
        public int NumSemanasHist { get { return Lines[16].Substring(0, 1) == "0" ? 52 : 53; } set { Lines[16] = Lines[16].Remove(0, 1).Insert(0, value == 52 ? "0" : "1"); } }

    }
}

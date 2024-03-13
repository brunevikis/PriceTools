using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EolicaNW
{
    public class HistVentos: BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"VENTO-HIST-HORIZ"             , new VentHorizonteBlock()},
                    {"VENTO-HIST"             , new VentHistBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public VentHorizonteBlock BlocoVentHor { get { return (VentHorizonteBlock)Blocos["VENTO-HIST-HORIZ"]; } set { Blocos["VENTO-HIST-HORIZ"] = value; } }
        public VentHistBlock BlocoVentHist { get { return (VentHistBlock)Blocos["VENTO-HIST"]; } set { Blocos["VENTO-HIST"] = value; } }

        public override void Load(string fileContent)
        {
            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string comments = null;

            foreach (var line in lines)
            {

                if (string.IsNullOrWhiteSpace(line)) continue;
                if (IsComment(line))
                {
                    comments = comments == null ? line : comments + Environment.NewLine + line;
                    continue;
                }
                if (line.StartsWith("VENTO-HIST-HORIZ") || line.StartsWith("VENTO-HIST"))
                {
                    string currentBlock = line.Split(new string[] { ";" }, StringSplitOptions.None).ToList().First();
                    var newLine = Blocos[currentBlock].CreateLineCSV(line);
                    newLine.Comment = comments;
                    newLine.LineCSV = line;
                    comments = null;
                    Blocos[currentBlock].Add(newLine);
                }
            }
        }
        public override bool IsComment(string line)
        {
            return line.StartsWith("&");
        }
    }
}

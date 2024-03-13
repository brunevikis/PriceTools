using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EolicaNW
{
    public class EolicaCad: BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"PEE-CAD"             , new PeeCadBlock()},
                    {"PEE-POT-INST-PER"             , new PeePotBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public PeeCadBlock BlocoPeeCad { get { return (PeeCadBlock)Blocos["PEE-CAD"]; } set { Blocos["PEE-CAD"] = value; } }
        public PeePotBlock BlocoPeePot { get { return (PeePotBlock)Blocos["PEE-POT-INST-PER"]; } set { Blocos["PEE-POT-INST-PER"] = value; } }

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
                if (line.StartsWith("PEE-CAD") || line.StartsWith("PEE-POT-INST-PER"))
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

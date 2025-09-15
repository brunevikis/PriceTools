using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.RenovaveisDcCSV
{
    public class RenovaveisCSV : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"PEE-CAD"             , new PEECADBlock()},
                    {"PEE-CONFIG-PER"             , new PEECONFIGBlock()},
                    {"PEE-SUBM"             , new PEESUBMBlock()},
                    {"PEE-POT-INST-PER"             , new PEEPOTINSTBlock()},
                    {"PEE-GER-PER-PAT-CEN"             , new PEEGERPERPATBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public PEECADBlock BlocoPeeCad { get { return (PEECADBlock)Blocos["PEE-CAD"]; } set { Blocos["PEE-CAD"] = value; } }
        public PEECONFIGBlock BlocoPeeConfig { get { return (PEECONFIGBlock)Blocos["PEE-CONFIG-PER"]; } set { Blocos["PEE-CONFIG-PER"] = value; } }
        public PEESUBMBlock BlocoPeeSubm { get { return (PEESUBMBlock)Blocos["PEE-SUBM"]; } set { Blocos["PEE-SUBM"] = value; } }
        public PEEPOTINSTBlock BlocoPeePotInst { get { return (PEEPOTINSTBlock)Blocos["PEE-POT-INST-PER"]; } set { Blocos["PEE-POT-INST-PER"] = value; } }
        public PEEGERPERPATBlock BlocoPeeGerPat { get { return (PEEGERPERPATBlock)Blocos["PEE-GER-PER-PAT-CEN"]; } set { Blocos["PEE-GER-PER-PAT-CEN"] = value; } }
        

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
                if (line.Trim().StartsWith("PEE-CAD") || line.Trim().StartsWith("PEE-CONFIG-PER") || line.Trim().StartsWith("PEE-SUBM") || line.Trim().StartsWith("PEE-POT-INST-PER") || line.Trim().StartsWith("PEE-GER-PER-PAT-CEN"))
                {
                    string currentBlock = line.Split(new string[] { ";" }, StringSplitOptions.None).ToList().First().Trim();
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

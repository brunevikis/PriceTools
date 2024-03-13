using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EolicaNW
{
    public class EolicaPosto: BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"POSTO-VENTO-CAD"             , new PostVentBlock()},
                    {"PEE-POSTO"             , new PeePostBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public PostVentBlock BlocoPostVent { get { return (PostVentBlock)Blocos["POSTO-VENTO-CAD"]; } set { Blocos["POSTO-VENTO-CAD"] = value; } }
        public PeePostBlock BlocoPeePost { get { return (PeePostBlock)Blocos["PEE-POSTO"]; } set { Blocos["PEE-POSTO"] = value; } }

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
                if (line.StartsWith("POSTO-VENTO-CAD") || line.StartsWith("PEE-POSTO"))
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

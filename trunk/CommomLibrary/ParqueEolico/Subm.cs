using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParqueEolico
{
    public class Subm : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"PEE-SUBM"             , new SubmBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public SubmBlock BlocoSubm { get { return (SubmBlock)Blocos["PEE-SUBM"]; } set { Blocos["PEE-SUBM"] = value; } }

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

                var newLine = Blocos["PEE-SUBM"].CreateLineCSV(line);
                newLine.Comment = comments;
                newLine.LineCSV = line;
                comments = null;
                Blocos["PEE-SUBM"].Add(newLine);
            }
        }
        public override bool IsComment(string line)
        {
            return line.StartsWith("&");
        }
    }

    

}

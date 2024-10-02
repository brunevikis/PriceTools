using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.RestElCSV
{
    public class RestElCSV : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"RE"             , new ReBlock()},
                    {"RE-HORIZ-PER"             , new ReHorizBlock()},
                    {"RE-LIM-FORM-PER-PAT"             , new ReLimFormPatBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public ReBlock BlocoRe { get { return (ReBlock)Blocos["RE"]; } set { Blocos["RE"] = value; } }
        public ReHorizBlock BlocoReHoriz { get { return (ReHorizBlock)Blocos["RE-HORIZ-PER"]; } set { Blocos["RE-HORIZ-PER"] = value; } }
        public ReLimFormPatBlock BlocoReLimFormPat { get { return (ReLimFormPatBlock)Blocos["RE-LIM-FORM-PER-PAT"]; } set { Blocos["RE-LIM-FORM-PER-PAT"] = value; } }

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
                if (line.StartsWith("RE") || line.StartsWith("RE-HORIZ-PER") || line.StartsWith("RE-LIM-FORM-PER-PAT"))
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

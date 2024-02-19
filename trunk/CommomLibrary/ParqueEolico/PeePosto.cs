using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParqueEolico
{
    public class PeePosto: BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"PEE-POSTO"             , new PeePostoBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public PeePostoBlock BlocoPeePosto { get { return (PeePostoBlock)Blocos["PEE-POSTO"]; } set { Blocos["PEE-POSTO"] = value; } }

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

                var newLine = Blocos["PEE-POSTO"].CreateLineCSV(line);
                newLine.Comment = comments;
                newLine.LineCSV = line;
                comments = null;
                Blocos["PEE-POSTO"].Add(newLine);
            }
        }
        public override bool IsComment(string line)
        {
            return line.StartsWith("&");
        }
    }



}

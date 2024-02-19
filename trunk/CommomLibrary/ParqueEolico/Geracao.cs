using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParqueEolico
{
    public class Geracao : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"PEE-GER"             , new GeracaoBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public GeracaoBlock BlocoGeracao { get { return (GeracaoBlock)Blocos["PEE-GER"]; } set { Blocos["PEE-GER"] = value; } }

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

                var newLine = Blocos["PEE-GER"].CreateLineCSV(line);
                newLine.Comment = comments;
                newLine.LineCSV = line;
                comments = null;
                Blocos["PEE-GER"].Add(newLine);
            }
        }
        public override bool IsComment(string line)
        {
            return line.StartsWith("&");
        }
    }



}

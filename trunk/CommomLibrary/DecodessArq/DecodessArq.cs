using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DecodessArq
{
    public class DecodessArq : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"ARQS", new ArqsBlock()},

                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public ArqsBlock BlocoArqs { get { return (ArqsBlock)Blocos["ARQS"]; } set { Blocos["ARQS"] = value; } }

        public override void Load(string fileContent)
        {


            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string comments = null;

            var currentBlock = "ARQS";
            foreach (var line in lines)
            {
               

                if (!Blocos.ContainsKey(currentBlock))
                {
                    continue;
                }

                var newLine = Blocos[currentBlock].CreateLine(line);
                newLine.Comment = comments;
                comments = null;
                Blocos[currentBlock].Add(newLine);

            }
            if (comments != null)
            {
                BottonComments = comments;
            }
        }


        public override bool IsComment(string line)
        {
            return line.StartsWith("&");
        }
    }
}

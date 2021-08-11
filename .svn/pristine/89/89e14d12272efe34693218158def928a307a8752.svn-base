using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DessemArq
{
    public class DessemArq : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"ARQ", new ArqBlock()},

                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public ArqBlock BlocoArq { get { return (ArqBlock)Blocos["ARQ"]; } set { Blocos["ARQ"] = value; } }

        public override void Load(string fileContent)
        {
            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);


            string comments = null;
            foreach (var line in lines)
            {
                if (IsComment(line))
                {
                    if (line.StartsWith("&Mnem") || line.StartsWith("&XXX"))
                    {
                        comments = comments == null ? line : comments + Environment.NewLine + line;
                    }
                    else
                    {
                        var currentBlock = "ARQ";

                        var newL = Blocos[currentBlock].CreateLine(line);
                        newL.Comment = comments;
                        comments = null;
                        Blocos[currentBlock].Add(newL);
                    }
                }
                else
                {
                    var currentBlock = "ARQ";

                    var newL = Blocos[currentBlock].CreateLine(line);
                    newL.Comment = comments;
                    comments = null;
                    Blocos[currentBlock].Add(newL);
                    
                }
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

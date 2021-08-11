using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Operuh
{
    public class Operuh : BaseDocument
    {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"REST ELEM LIM VAR COND" , new RhestBlock()},

                };
        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        
        public RhestBlock BlocoRhest { get { return (RhestBlock)Blocos["REST ELEM LIM VAR COND"]; } set { Blocos["REST ELEM LIM VAR COND"] = value; } }

        public override void Load(string fileContent)
        {
            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);


            string comments = null;
            foreach (var line in lines)
            {
                if (IsComment(line))
                {
                    comments = comments == null ? line : comments + Environment.NewLine + line;
                }
                else if(line != "")
                {
                    var cod = line.Split(' ')[1];
                    //var cod = (line + "  ").Substring(0, 2);

                    if (Blocos.Keys.Any(k => k.Split(' ').Contains(cod)))
                    {
                        var block = Blocos.First(k => k.Key.Split(' ').Contains(cod)).Value;
                        var newLine = block.CreateLine(line);

                        newLine.Comment = comments;
                        comments = null;
                        block.Add(newLine);
                    }
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

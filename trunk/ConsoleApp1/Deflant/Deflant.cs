using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.Deflant
{
    public class Deflant : BaseDocument
    {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"DEFANT" , new DefBlock()},

                };
        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }


        public DefBlock BlocoDef { get { return (DefBlock)Blocos["DEFANT"]; } set { Blocos["DEFANT"] = value; } }

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
                else if (line != "")
                {
                    
                    var cod = (line + "  ").Substring(0, 6);

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

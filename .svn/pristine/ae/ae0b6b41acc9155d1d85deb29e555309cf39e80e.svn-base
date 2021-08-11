using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Operut
{
    public class Operut : BaseDocument {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"INIT", new InitBlock()},
                    {"OPER", new OperBlock()},    
                   
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public InitBlock BlocoInit { get { return (InitBlock)Blocos["INIT"]; } set { Blocos["INIT"] = value; } }
        public OperBlock BlocoOper { get { return (OperBlock)Blocos["OPER"]; } set { Blocos["OPER"] = value; } }

        public override void Load(string fileContent)
        {


            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string comments = null;

            var currentBlock = "";
            var blockStarted = false;
            foreach (var line in lines)
            {
                switch (line.Trim())
                {
                    case "INIT":
                        currentBlock = "INIT";
                        comments = comments == null ? line : comments + Environment.NewLine + line;

                        blockStarted = false;
                        continue;
                    case "OPER":
                        currentBlock = "OPER";
                        comments = comments == null ? line : comments + Environment.NewLine + line;

                        blockStarted = false;
                        continue;
                    default:
                        if (line.Trim().StartsWith("&XX"))
                        {
                            comments = comments == null ? line : comments + Environment.NewLine + line;

                            blockStarted = true;
                            continue;
                        }
                        else if (!blockStarted || line.Trim().StartsWith("FIM"))
                        {
                            comments = comments == null ? line : comments + Environment.NewLine + line;
                            blockStarted = false;
                            continue;
                        }
                        break;
                }

                if (!Blocos.ContainsKey(currentBlock))
                {
                    continue;
                }
                if (IsComment(line))
                {
                    comments = comments == null ? line : comments + Environment.NewLine + line;
                    continue;
                }
                var newLine = Blocos[currentBlock].CreateLine(line);
                newLine.Comment = comments;
                comments = null;
                Blocos[currentBlock].Add(newLine);

            }
        }


        public override bool IsComment(string line)
        {
            return line.StartsWith("&");
        }
    }
}

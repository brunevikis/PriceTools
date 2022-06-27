using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.CurvaDat
{
    public class CurvaDat : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"CURVASEG", new CurvaSegBlock()},

                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public CurvaSegBlock BlocoCurvaSeg { get { return (CurvaSegBlock)Blocos["CURVASEG"]; } set { Blocos["CURVASEG"] = value; } }

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
                    case "CURVA DE SEGURANCA (EM % DE EARMX)":
                        currentBlock = "CURVASEG";
                        comments = comments == null ? line : comments + Environment.NewLine + line;

                        blockStarted = false;
                        continue;
                  
                    default:
                        if (line.Trim().StartsWith("JAN.X"))
                        {
                            comments = comments == null ? line : comments + Environment.NewLine + line;

                            blockStarted = true;
                            continue;
                        }
                        else if (!blockStarted || line.Trim().StartsWith("9999"))
                        {
                            if (currentBlock == "CURVASEG" && line.Trim().StartsWith("9999"))
                            {
                                currentBlock = "";
                            }
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

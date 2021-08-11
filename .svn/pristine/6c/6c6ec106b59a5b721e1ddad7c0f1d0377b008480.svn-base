using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Areacont
{
    public class Areacont : BaseDocument
    {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"AREA", new AreaBlock()},
                    {"USINA", new UsinaBlock()},




                };
        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public AreaBlock BlocoArea { get { return (AreaBlock)Blocos["AREA"]; } set { Blocos["AREA"] = value; } }
        public UsinaBlock BlocoUsina { get { return (UsinaBlock)Blocos["USINA"]; } set { Blocos["USINA"] = value; } }

        public override void Load(string fileContent)
        {


            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string comments = null;

            var currentBlock = "";
            //var blockStarted = false;
            foreach (var line in lines)
            {
                if (IsComment(line) || line.StartsWith("FIM")  || line.StartsWith("9999"))
                {
                    comments = comments == null ? line : comments + Environment.NewLine + line;
                }
                else
                {
                    switch (line.Trim())
                    {
                        case "AREA":
                            currentBlock = "AREA";
                            comments = comments == null ? line : comments + Environment.NewLine + line;

                            //blockStarted = false;
                            continue;
                        case "USINA":
                            currentBlock = "USINA";
                            comments = comments == null ? line : comments + Environment.NewLine + line;

                            //blockStarted = false;
                            continue;
                        default:
                            break;
                    }

                    if (!Blocos.ContainsKey(currentBlock))
                    {
                        continue;
                    }

                    var newLine = Blocos[currentBlock].CreateLine(line);
                    newLine.Comment = comments;
                    comments = null;
                    Blocos[currentBlock].Add(newLine);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ConfigDat
{
    public class ConfigDat : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"DATA", new DataBlock()},
                    {"DIA", new DiaBlock()},

                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public DataBlock BlocoData { get { return (DataBlock)Blocos["DATA"]; } set { Blocos["DATA"] = value; } }
        public DiaBlock BlocoDia { get { return (DiaBlock)Blocos["DIA"]; } set { Blocos["DIA"] = value; } }

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
                    case "DATA":
                        currentBlock = "DATA";
                        comments = comments == null ? line : comments + Environment.NewLine + line;

                        blockStarted = false;
                        continue;
                    case "DIA":
                        currentBlock = "DIA";
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

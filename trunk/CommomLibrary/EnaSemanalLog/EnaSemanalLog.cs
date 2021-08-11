using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EnaSemanalLog
{
    public class EnaSemanalLog : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"DATA", new DataSemanalBlock()},
                    {"ENA", new EnaBlock()},

                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public DataSemanalBlock BlocoDataSemanal { get { return (DataSemanalBlock)Blocos["DATA"]; } set { Blocos["DATA"] = value; } }
        public EnaBlock BlocoEna { get { return (EnaBlock)Blocos["ENA"]; } set { Blocos["ENA"] = value; } }

        public override void Load(string fileContent)
        {


            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string comments = null;

            var currentBlock = "";
            foreach (var line in lines)
            {
                if (line == lines.First())
                {
                    currentBlock = "DATA";
                }
                else
                {
                    var partes = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (partes.Count() < 3)
                    {
                        comments = comments == null ? line : comments + Environment.NewLine + line;
                        continue;
                    }
                    else
                    {
                        currentBlock = "ENA";

                    }
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

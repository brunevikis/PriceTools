using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Renovaveis
{
    public class Renovaveis : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"EOLICA", new EolicaBlock()},
                    {"EOLICABARRA", new EolicaBarraBlock()},
                    {"EOLICASUBM", new EolicaSubmBlock()},
                    {"EOLICA-GERACAO", new EolicaGeracaoBlock()},

                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public EolicaBlock BlocoEolica { get { return (EolicaBlock)Blocos["EOLICA"]; } set { Blocos["EOLICA"] = value; } }
        public EolicaBarraBlock BlocoEolicaBarra { get { return (EolicaBarraBlock)Blocos["EOLICABARRA"]; } set { Blocos["EOLICABARRA"] = value; } }
        public EolicaSubmBlock BlocoEolicaSubm { get { return (EolicaSubmBlock)Blocos["EOLICASUBM"]; } set { Blocos["EOLICASUBM"] = value; } }
        public EolicaGeracaoBlock BlocoEolicaGeracao { get { return (EolicaGeracaoBlock)Blocos["EOLICA-GERACAO"]; } set { Blocos["EOLICA-GERACAO"] = value; } }

        public override void Load(string fileContent)
        {
            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);


            string comments = null;
            foreach (var line in lines)
            {
                if (line != "")
                {
                    if (IsComment(line))
                    {
                        comments = comments == null ? line : comments + Environment.NewLine + line;
                    }
                    else
                    {
                        var cod = line.Split(';').First().Trim();
                        //var cod = (line + "  ").Substring(0, 6);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Infofcf
{
    public class Infofcf : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"MAPFCF", new DummyBlock()},
                    {"FCFFIX", new FcffixBlock()},
                    {"BENFUT", new BenfutBlock()},

                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

       // public MapfcfBlock BlocoMapfcf { get { return (MapfcfBlock)Blocos["MAPFCF"]; } set { Blocos["MAPFCF"] = value; } }
        public FcffixBlock BlocoFcffix { get { return (FcffixBlock)Blocos["FCFFIX"]; } set { Blocos["FCFFIX"] = value; } }
        public BenfutBlock BlocoBenfut { get { return (BenfutBlock)Blocos["BENFUT"]; } set { Blocos["BENFUT"] = value; } }

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
                        var cod = line.Split(' ').First();
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

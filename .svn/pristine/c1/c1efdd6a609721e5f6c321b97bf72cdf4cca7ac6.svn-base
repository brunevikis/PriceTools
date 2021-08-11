using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.PtoperDat
{
    public class PtoperDat : BaseDocument {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"PTOPER", new PtoperBlock()},

                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public PtoperBlock BlocoPtoper { get { return (PtoperBlock)Blocos["PTOPER"]; } set { Blocos["PTOPER"] = value; } }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Cotasr
{
    public class Cotasr : BaseDocument
    {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"COT", new CotBlock()},




                };
        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public CotBlock BlocoCot { get { return (CotBlock)Blocos["COT"]; } set { Blocos["COT"] = value; } }

        public override void Load(string fileContent)
        {


            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string comments = null;

            var currentBlock = "";
            //var blockStarted = false;
            foreach (var line in lines)
            {
                if (IsComment(line))
                {
                    comments = comments == null ? line : comments + Environment.NewLine + line;
                }
                else
                {

                    currentBlock = "COT";
                   // comments = comments == null ? line : comments + Environment.NewLine + line;



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

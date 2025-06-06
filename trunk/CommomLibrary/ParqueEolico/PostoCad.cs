﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParqueEolico
{
    public class PostoCad : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"POSTO-VENTO-CAD"             , new PostoCadBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }
        public PostoCadBlock BlocoPostoCad { get { return (PostoCadBlock)Blocos["POSTO-VENTO-CAD"]; } set { Blocos["POSTO-VENTO-CAD"] = value; } }

        public override void Load(string fileContent)
        {
            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string comments = null;

            foreach (var line in lines)
            {

                if (string.IsNullOrWhiteSpace(line)) continue;
                if (IsComment(line))
                {
                    comments = comments == null ? line : comments + Environment.NewLine + line;
                    continue;
                }

                var newLine = Blocos["POSTO-VENTO-CAD"].CreateLineCSV(line);
                newLine.Comment = comments;
                newLine.LineCSV = line;
                comments = null;
                Blocos["POSTO-VENTO-CAD"].Add(newLine);
            }
        }
        public override bool IsComment(string line)
        {
            return line.StartsWith("&");
        }
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParqueEolico
{
    public class PrevsVel: BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"PrevsVel"             , new PrevsVelBlock()},
                };

        public PrevsVelBlock BlocoPrevsVel { get { return (PrevsVelBlock)Blocos["PrevsVel"]; } set { Blocos["PrevsVel"] = value; } }


        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }

        public override void Load(string fileContent)
        {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var newLine = Blocos["PrevsVel"].CreateLine(line);
                Blocos["PrevsVel"].Add(newLine);
            }
        }
    }
}

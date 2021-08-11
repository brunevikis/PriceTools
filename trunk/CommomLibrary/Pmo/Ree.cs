using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Pmo {
    public class ReeBlock : BaseBlock<ReeLine> {


        public int GetRee(string ree) {
            return (int)this.Where(x => ((string)x[1]).Trim().Equals(ree.Trim(), StringComparison.OrdinalIgnoreCase)).First()[0];
        }
        public int GetMercado(int ree) {
            return (int)this.Where(x => ((int)x[0]).Equals(ree)).First()[2];
        }

        public int GetMercado(string mercado) {
            return (int)this.Where(x => ((string)x[3]).Trim().Equals(mercado.Trim(), StringComparison.OrdinalIgnoreCase)).First()[2];
        }


        internal void Load(string fileContent, string key) {


            var i = fileContent.IndexOf(key);
            if (i < 0) return;
            var lines = fileContent.Remove(0, i).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(4).Take(15).ToArray();

            foreach (var line in lines) {

                if (line.Contains("X--------")) break;

                this.Add(
                    this.CreateLine(line)
                );                
            }
        }

        internal void Load(System.IO.StreamReader tr) {

            tr.ReadLine(); tr.ReadLine(); tr.ReadLine(); tr.ReadLine(); tr.ReadLine(); tr.ReadLine();
            var line = tr.ReadLine();

            if (line.Contains("X--------"))
            {
                line = tr.ReadLine();

                while (!line.Contains("X--------"))
                {
                    this.Add(
                        BaseLine.Create<ReeLineFT>(line)
                    );
                    line = tr.ReadLine();
                }
            }
            else
            {
                while (!line.Contains("X--------"))
                {
                    this.Add(
                        this.CreateLine(line)
                    );
                    line = tr.ReadLine();
                }
            }
        }
    }


    public class ReeLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 5 ,  7 ,"I3"  , "REE"),
                new BaseField( 10 ,  23 ,"A12"  , "NOME REE"),
                new BaseField( 28,  30 ,"I3"  , "MERCADO"),
                new BaseField( 35,  46 ,"A12"  , "NOME MERCADO"),

        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class ReeLineFT : ReeLine
    {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 5 ,  7 ,"I3"  , "REE"),
                new BaseField( 10 ,  23 ,"A12"  , "NOME REE"),
                new BaseField( 37,  39 ,"I3"  , "MERCADO"),
                new BaseField( 44,  55 ,"A12"  , "NOME MERCADO"),

        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

    }

}



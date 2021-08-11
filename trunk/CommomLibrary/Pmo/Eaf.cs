using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Pmo {
    public class EafBlock : BaseBlock<EafLine> {


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

            tr.ReadLine();tr.ReadLine();tr.ReadLine();
            var line = tr.ReadLine();


            while (!line.Contains("X--------")) {
                this.Add(
                    this.CreateLine(line)
                );
                line = tr.ReadLine();
            }
        }
    }


    public class EafLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 2 ,  11 ,"A10"  , "REE"),
                new BaseField(14  , 22 ,"F10.2"  , "Jan"),
                new BaseField(24  , 33 ,"F10.2"  , "Fev"),
                new BaseField(35  , 44 ,"F10.2"  , "Mar"),
                new BaseField(46  , 55 ,"F10.2"  , "Abr"),
                new BaseField(57  , 66 ,"F10.2"  , "Mai"),
                new BaseField(68  , 77 ,"F10.2"  , "Jun"),
                new BaseField(79  , 88 ,"F10.2"  , "Jul"),
                new BaseField(90  , 99 ,"F10.2"  , "Ago"),
                new BaseField(101 , 110,"F10.2"  , "Set"),
                new BaseField(112 , 121,"F10.2"  , "Out"),
                new BaseField(123 , 132,"F10.2"  , "Nov"),
                new BaseField(134 , 143,"F10.2"  , "Dez"),
        };

        public override BaseField[] Campos {
            get { return campos; }
        }
    }

}



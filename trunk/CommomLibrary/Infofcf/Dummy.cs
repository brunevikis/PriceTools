using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Infofcf
{
    public class DummyBlock : BaseBlock<DummyLine>
    {
    }

    public class DummyLine : BaseLine
    {

        public DummyLine() { }
        public DummyLine(string id, string val)
        {
            this[0] = id;
            this[1] = val;
        }

        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 6 ,"A6"  , "Id"),
                new BaseField( 7, 92 ,"A90", "Valor"  ),

            };

        public override BaseField[] Campos
        {
            get { return campos; }
        }


        public override void Load(string line)
        {

            this[0] = line.Substring(0, 6);
            this[1] = line.Substring(6);
        }
        public override string ToText()
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(Comment)) result = Comment + Environment.NewLine;
            else result = "";

            return result + this[0].ToString() + this[1].ToString();
        }
    }
}

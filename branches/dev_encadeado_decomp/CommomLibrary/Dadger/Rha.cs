using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class RhaBlock : BaseBlock<RhaLine> {

        public override RhaLine CreateLine(string line = null) {

            var cod = line.Substring(0, 2);
            switch (cod) {
                case "HA":
                    return (RhaLine)BaseLine.Create<HaLine>(line);
                case "LA":
                    return (RhaLine)BaseLine.Create<LaLine>(line);
                case "CA":
                    return (RhaLine)BaseLine.Create<CaLine>(line);
                default:
                    throw new ArgumentException("Invalid identifier " + cod);
            }
        }

        public Dictionary<HaLine, List<RhaLine>> RhaGrouped {
            get {

                var temp = new Dictionary<HaLine, List<RhaLine>>();
                var restID = new BaseField(5, 7, "I3", "Restricao");

                foreach (var hv in this.Where(x => x is HaLine)) {
                    var hvID = (int)hv[restID];
                    temp.Add(
                        (HaLine)hv, this.Where(x => (int)x[restID] == hvID).ToList()
                        );
                }

                return temp;
            }
        }
    }


    public abstract class RhaLine : BaseLine {
        public int Restricao { get { return this[1]; } set { this[1] = value; } }
    }

    public class HaLine : RhaLine {
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio Ini"),
                new BaseField( 15 , 16,"I2"    , "Estagio Fim"),
            };


        public override BaseField[] Campos {
            get { return campos; }
        }
    }
    public class LaLine : RhaLine {
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 24,"F10.0" , "Limite Inf"),
                new BaseField( 25 , 34,"F10.0" , "Limite Sup"),

            };


        public override BaseField[] Campos {
            get { return campos; }
        }
    }
    public class CaLine : RhaLine {
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 17,"I3"    , "Usina"),
            };


        public override BaseField[] Campos {
            get { return campos; }
        }
    }


}

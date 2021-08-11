using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.Dadger
{
    public class RhvBlock : BaseBlock<RhvLine> {

        public override RhvLine CreateLine(string line = null) {

            var cod = line.Substring(0, 2);
            switch (cod) {
                case "HV":
                    return (RhvLine)BaseLine.Create<HvLine>(line);
                case "LV":
                    return (RhvLine)BaseLine.Create<LvLine>(line);
                case "CV":
                    return (RhvLine)BaseLine.Create<CvLine>(line);
                default:
                    throw new ArgumentException("Invalid identifier " + cod);
            }
        }

        public Dictionary<HvLine, List<RhvLine>> RhvGrouped {
            get {

                var temp = new Dictionary<HvLine, List<RhvLine>>();
                var restID = new BaseField(5, 7, "I3", "Restricao");



                foreach (var hv in this.Where(x => x is HvLine)) {
                    var hvID = (int)hv[restID];
                    temp.Add(
                        (HvLine)hv, this.Where(x => (int)x[restID] == hvID).ToList()
                        );
                }

                return temp;
            }
        }

        public int GetNextId() { return this.Max(x => (int)x[1]) + 1; }

        public void Add(LvLine lv) {
            var re = this.RhvGrouped.Keys.Where(x => x[1] == lv[1]).FirstOrDefault();
            if (re != null) {
                var prevLu = RhvGrouped[re].LastOrDefault(x => x is LvLine && x[2] < lv[2]);

                var idx = this.IndexOf(prevLu ?? re) + 1;
                this.Insert(idx, lv);
            }
        }
    }


    public abstract class RhvLine : BaseLine {
        public int Restricao { get { return this[1]; } set { this[1] = value; } }
    }

    public class HvLine : RhvLine {
        public HvLine()
            : base() {
            this[0] = "HV";
        }
        static readonly BaseField[] HvCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio Ini"),
                new BaseField( 15 , 16,"I2"    , "Estagio Fim"),
            };

        public override BaseField[] Campos {
            get { return HvCampos; }
        }

        public int Inicio { get { return this[2]; } set { this[2] = value; } }
        public int Fim { get { return this[3]; } set { this[3] = value; } }
    }
    public class LvLine : RhvLine {
        public LvLine()
            : base() {
            this[0] = "LV";
        }

        static readonly BaseField[] LvCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 24,"F10.0" , "Limite Inf"),
                new BaseField( 25 , 34,"F10.0" , "Limite Sup"),
            };

        public override BaseField[] Campos {
            get { return LvCampos; }
        }
        public int Estagio { get { return this[2]; } set { this[2] = value; } }
    }
    public class CvLine : RhvLine {
        public CvLine()
            : base() {
            this[0] = "CV";
            this[2] = 1;
            this[4] = 1;

        }
        static readonly BaseField[] CvCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 17,"I3"    , "Usina"),
                new BaseField( 20 , 29,"f10.8" , "Coeficiente"),
                new BaseField( 35 , 38,"A4"    , "Tipo"),
            };

        public override BaseField[] Campos {
            get { return CvCampos; }
        }
        public int Usina { get { return this[3]; } set { this[3] = value; } }
        public string Tipo { get { return this[5]; } set { this[5] = value; } }
    }
}

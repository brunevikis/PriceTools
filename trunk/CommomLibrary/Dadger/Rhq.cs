using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class RhqBlock : BaseBlock<RhqLine> {

        public override RhqLine CreateLine(string line = null) {

            var cod = line.Substring(0, 2);
            switch (cod) {
                case "HQ":
                    return (RhqLine)BaseLine.Create<HqLine>(line);
                case "LQ":
                    return (RhqLine)BaseLine.Create<LqLine>(line);
                case "CQ":
                    return (RhqLine)BaseLine.Create<CqLine>(line);
                default:
                    throw new ArgumentException("Invalid identifier " + cod);
            }            
        }

        public Dictionary<HqLine, List<RhqLine>> RhqGrouped {
            get {

                var temp = new Dictionary<HqLine, List<RhqLine>>();
                var restID = new BaseField(5, 7, "I3", "Restricao");

                foreach (var hv in this.Where(x => x is HqLine)) {
                    var hvID = (int)hv[restID];
                    temp.Add(
                        (HqLine)hv, this.Where(x => (int)x[restID] == hvID).ToList()
                        );
                }

                return temp;
            }
        }


        public int GetNextId() { return this.Max(x => (int)x[1]) + 1; }

        public void Add(LqLine lu) {
            var re = this.RhqGrouped.Keys.Where(x => x[1] == lu[1]).FirstOrDefault();
            if (re != null) {
                var prevLu = RhqGrouped[re].LastOrDefault(x => x is LqLine && x[2] < lu[2]);

                var idx = this.IndexOf(prevLu ?? re) + 1;
                this.Insert(idx, lu);
            }
        }
    }


    public abstract class RhqLine : BaseLine {
        public int Restricao { get { return this[1]; } set { this[1] = value; } }
    }

    public class HqLine : RhqLine {
        public HqLine()
            : base() {
            this[0] = "HQ";
        }
        static readonly BaseField[] HqCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio Ini"),
                new BaseField( 15 , 16,"I2"    , "Estagio Fim"),


            };


        public override BaseField[] Campos {
            get { return HqCampos; }
        }

        public int Inicio { get { return this[2]; } set { this[2] = value; } }
        public int Fim { get { return this[3]; } set { this[3] = value; } }
    }
    public class LqLine : RhqLine {
        public LqLine()
            : base() {
            this[0] = "LQ";
        }
        static readonly BaseField[] LqCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 24,"F10.0" , "Limite Inf Pat1"),
                new BaseField( 25 , 34,"F10.0" , "Limite Sup Pat1"),
                new BaseField( 35 , 44,"F10.0" , "Limite Inf Pat2"),
                new BaseField( 45 , 54,"F10.0" , "Limite Sup Pat2"),
                new BaseField( 55 , 64,"F10.0" , "Limite Inf Pat3"),
                new BaseField( 65 , 74,"F10.0" , "Limite Sup Pat3"),



            };


        public override BaseField[] Campos {
            get { return LqCampos; }
        }

        public int Estagio { get { return this[2]; } set { this[2] = value; } }
        public double? LimInfPat1 { get => this[3] is double x ? x : (double?)null; set { this[3] = value; } }
        public double? LimInfPat2 { get => this[5] is double x ? x : (double?)null; set { this[5] = value; } }
        public double? LimInfPat3 { get => this[7] is double x ? x : (double?)null; set { this[7] = value; } }
        public double? LimSupPat1 { get => this[4] is double x ? x : (double?)null; set { this[4] = value; } }
        public double? LimSupPat2 { get => this[6] is double x ? x : (double?)null; set { this[6] = value; } }
        public double? LimSupPat3 { get => this[8] is double x ? x : (double?)null; set { this[8] = value; } }
    }
    public class CqLine : RhqLine {
        public CqLine()
            : base() {
            this[0] = "CQ";
            this[2] = 1;
            this[4] = 1;
        }
        static readonly BaseField[] CqCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 17,"I3"    , "Usina"),
                new BaseField( 20 , 29,"F10.0" , "Coeficiente"),
                new BaseField( 35 , 38,"A4"    , "Tipo"),
            };


        public override BaseField[] Campos {
            get { return CqCampos; }
        }

        public int Usina { get { return this[3]; } set { this[3] = value; } }
        public string Tipo { get { return this[5]; } set { this[5] = value; } }
    }    
}

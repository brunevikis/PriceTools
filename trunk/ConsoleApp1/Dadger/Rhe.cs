using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.Dadger
{
    public class RheBlock : BaseBlock<RheLine> {

        public override RheLine CreateLine(string line = null) {

            var cod = line.Substring(0, 2);
            switch (cod) {
                case "RE":
                    return (RheLine)BaseLine.Create<ReLine>(line);
                case "LU":
                    return (RheLine)BaseLine.Create<LuLine>(line);
                case "FU":
                    return (RheLine)BaseLine.Create<FuLine>(line);
                case "FT":
                    return (RheLine)BaseLine.Create<FtLine>(line);
                case "FI":
                    return (RheLine)BaseLine.Create<FiLine>(line);
                case "FE":
                    return (RheLine)BaseLine.Create<FeLine>(line);
                default:
                    throw new ArgumentException("Invalid identifier " + cod);
            }
        }

        public Dictionary<ReLine, List<RheLine>> RheGrouped {
            get {

                var temp = new Dictionary<ReLine, List<RheLine>>();
                var restID = new BaseField(5, 8, "I4", "Restricao");

                foreach (var hv in this.Where(x => x is ReLine)) {

                    var hvID = (int)hv[restID];

                    temp.Add(
                        (ReLine)hv, this.Where(x => (int)x[restID] == hvID).ToList()
                        );
                }

                return temp;
            }
        }


        public int GetNextId() { return this.Max(x => (int)x[1]) + 1; }

        public void Add(LuLine lu) {
            var re = this.RheGrouped.Keys.Where(x => x[1] == lu[1]).FirstOrDefault();
            if (re != null) {
                var prevLu = RheGrouped[re].LastOrDefault(x => x is LuLine && x[2] < lu[2]);

                var idx = this.IndexOf(prevLu ?? re) + 1;
                this.Insert(idx, lu);
            }
        }
    }


    public abstract class RheLine : BaseLine {

        public int Restricao { get { return this[1]; } set { this[1] = value; } }

    }

    public class ReLine : RheLine {
        public ReLine()
            : base() {
            this[0] = "RE";
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 8 ,"I4"   , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio Ini"),
                new BaseField( 15 , 16,"I2"    , "Estagio Fim"),
            };


        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Inicio { get { return this[2]; } set { this[2] = value; } }
        public int Fim { get { return this[3]; } set { this[3] = value; } }
    }
    public class LuLine : RheLine {
        public LuLine()
            : base() {
            this[0] = "LU";
        }

        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 8 ,"I4"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 24,"F10.0" , "Limite Inf pat1"),
                new BaseField( 25 , 34,"F10.0" , "Limite Sup pat1"),
                new BaseField( 35 , 44,"F10.0" , "Limite Inf pat2"),
                new BaseField( 45 , 54,"F10.0" , "Limite Sup pat2"),
                new BaseField( 55 , 64,"F10.0" , "Limite Inf pat3"),
                new BaseField( 65 , 74,"F10.0" , "Limite Sup pat3"),
            };


        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Estagio { get { return this[2]; } set { this[2] = value; } }
    }
    public class FuLine : RheLine {

        public FuLine()
            : base() {
            this[0] = "FU";
            this[2] = 1;
            this[4] = 1;
        }

        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 8 ,"I4"   , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 17,"I3"    , "Usina"),
                new BaseField( 20 , 29,"F10.0" , "Fator"),
                new BaseField( 31 , 32,"I2" , "Freq Itaipu"),
            };


        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Usina { get { return this[3]; } set { this[3] = value; } }
        public int Estagio { get { return this["Estagio"]; } set { this["Estagio"] = value; } }
        public int Fator { get { return this["Fator"]; } set { this["Fator"] = value; } }



    }

    public class FtLine : RheLine {

        public FtLine()
            : base() {
            this[0] = "FT";
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 8 ,"I4"  , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 17,"I3"    , "Usina"),
                new BaseField( 20 , 21,"I2"    , "Subsistema"),
                new BaseField( 25 , 34,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Usina { get { return this[3]; } set { this[3] = value; } }
    }
    public class FiLine : RheLine {

        public FiLine()
            : base() {
            this[0] = "FI";
            this[2] = 1;
            this[5] = 1;
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 8 ,"I4"   , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 16,"A2"    , "De"),
                new BaseField( 20 , 21,"A2"    , "Para"),
                new BaseField( 25 , 34,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos {
            get { return campos; }
        }

        public string De { get { return this[3].Trim(); } set { this[3] = value; } }
        public string Para { get { return this[4].Trim(); } set { this[4] = value; } }
    }

    public class FeLine : RheLine
    {

        public FeLine()
            : base()
        {
            this[0] = "FE";
            this[2] = 1;
            this[4] = 1;
        }

        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 8 ,"I4"    , "Restricao"),
                new BaseField( 10 , 11,"I2"    , "Estagio"),
                new BaseField( 15 , 17,"I3"    , "Contrato"),
                new BaseField( 20 , 21,"I1"    , "Submercado"),
                new BaseField( 25 , 34,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int Contrato { get { return this[3]; } set { this[3] = value; } }
    }


}

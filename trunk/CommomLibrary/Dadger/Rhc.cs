using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger
{
    public class RhcBlock : BaseBlock<RhcLine>
    {

        public override RhcLine CreateLine(string line = null)
        {

            var cod = line.Substring(0, 2);
            switch (cod)
            {
                case "HE":
                    return (RhcLine)BaseLine.Create<HeLine>(line);
                case "CM":
                    return (RhcLine)BaseLine.Create<CmLine>(line);
                default:
                    throw new ArgumentException("Invalid identifier " + cod);
            }
        }

        public Dictionary<HeLine, List<RhcLine>> RhcGrouped
        {
            get
            {

                var temp = new Dictionary<HeLine, List<RhcLine>>();
                var restID = new BaseField(5, 7, "I3", "Restricao");

                foreach (var hv in this.Where(x => x is HeLine))
                {

                    var hvID = (int)hv[restID];

                    temp.Add(
                        (HeLine)hv, this.Where(x => (int)x[restID] == hvID).ToList()
                        );
                }

                return temp;
            }
        }


        public int GetNextId() { return this.Max(x => (int)x[1]) + 1; }

        public void Add(CmLine cm)
        {
            var re = this.RhcGrouped.Keys.Where(x => x[1] == cm[1]).FirstOrDefault();
            if (re != null)
            {
                var prevCm = RhcGrouped[re].LastOrDefault(x => x is CmLine && x[2] < cm[2]);

                var idx = this.IndexOf(prevCm ?? re) + 1;
                this.Insert(idx, cm);
            }
        }

        //public override string ToText() {
        //    var result = new StringBuilder();

        //    foreach (var item in this.RheGrouped) {
        //        result.AppendLine(item.Value.First(x => x is ReLine).ToText());
        //        foreach (var lu in item.Value.Where(x => x is LuLine).OrderBy(x => x[2])) {
        //            result.AppendLine(lu.ToText());
        //        }
        //        foreach (var f in item.Value.Where(x => !(x is LuLine) && !(x is ReLine))) {
        //            result.AppendLine(f.ToText());
        //        }

        //    }

        //    return result.ToString();
        //}


    }


    public abstract class RhcLine : BaseLine
    {

        public int Restricao { get { return this[1]; } set { this[1] = value; } }

    }

    public class HeLine : RhcLine
    {
        public HeLine()
            : base()
        {
            this[0] = "HE";
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"   , "Restricao"),
                new BaseField( 10 , 10,"I1"    , "tipo limite"),
                new BaseField( 15 , 24,"F10.1"    , "limite inf"),
                new BaseField( 26 , 27,"I2"    , "estago"),
                new BaseField( 29 , 38,"F10.0"    , "Penalidade"),
                new BaseField( 40 , 40,"I1"    , "Calculo"),
                new BaseField( 42 , 42,"I1"    , "tipovalores"),
                new BaseField( 44 , 44,"I1"    , "Flagtratamento"),
                new BaseField( 46 , 105,"A60"    , "Nome Arq dados"),
                new BaseField( 107 , 107,"I1"    , "Flag tolerancia"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }
        public int Estagio { get { return this[4]; } set { this[4] = value; } }
        public double limInf { get { return this[3]; } set { this[3] = value; } }

        // public int Inicio { get { return this[2]; } set { this[2] = value; } }
        // public int Fim { get { return this[3]; } set { this[3] = value; } }
    }
    public class CmLine : RhcLine
    {
        public CmLine()
            : base()
        {
            this[0] = "CM";
        }

        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField( 10 , 12,"I3"    , "indice REE"),
                new BaseField( 15 , 24,"F10.0" , "Coeficiente"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

    }
    
}

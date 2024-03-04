using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Operuh
{
    public class RhestBlock : BaseBlock<RhestLine>
    {

        public override RhestLine CreateLine(string line = null)
        {
            var cod = line.Split(' ')[1].Trim();
            //var cod = line.Substring(0, 2);
            switch (cod)
            {//"REST ELEM LIM VAR COND"
                case "REST":
                    return (RhestLine)BaseLine.Create<RestLine>(line);
                case "ELEM":
                    return (RhestLine)BaseLine.Create<ElemLine>(line);
                case "LIM":
                    return (RhestLine)BaseLine.Create<LimLine>(line);
                case "VAR":
                    return (RhestLine)BaseLine.Create<VarLine>(line);
                case "COND":
                    return (RhestLine)BaseLine.Create<CondLine>(line);
                default:
                    throw new ArgumentException("Invalid identifier " + cod);
            }
        }

        public Dictionary<RestLine, List<RhestLine>> RhestGrouped
        {
            get
            {

                var temp = new Dictionary<RestLine, List<RhestLine>>();
                var restID = new BaseField(15, 19, "A5", "Restricao");

                foreach (var hv in this.Where(x => x is RestLine))
                {

                    var hvID = (string)hv[restID];

                    temp.Add(
                        (RestLine)hv, this.Where(x => (string)x[restID] == hvID).ToList()
                        );
                }

                return temp;
            }
        }


        public int GetNextId() { return this.Max(x => (int)x[1]) + 1; }

        //public void Add(LuLine lu)
        //{
        //    var re = this.RheGrouped.Keys.Where(x => x[1] == lu[1]).FirstOrDefault();
        //    if (re != null)
        //    {
        //        var prevLu = RheGrouped[re].LastOrDefault(x => x is LuLine && x[2] < lu[2]);

        //        var idx = this.IndexOf(prevLu ?? re) + 1;
        //        this.Insert(idx, lu);
        //    }
        //}

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


    public abstract class RhestLine : BaseLine
    {

        public string Restricao { get { return this[2].ToString(); } set { this[2] = value; } }
        public string Minemonico { get { return this[1].ToString(); } set { this[1] = value; } }

        public string DiaInic { get { return this[3].ToString(); } set { this[3] = value; } }
        public int? HoraInic { get { return (int?)this[4]; } set { this[4] = value; } }
        public int? MeiaHoraInic { get { return (int?)this[5]; } set { this[5] = value; } }
        public string DiaFinal { get { return this[6].ToString(); } set { this[6] = value; } }
        public int? HoraFinal { get { return (int?)this[7]; } set { this[7] = value; } }
        public int? MeiaHoraFinal { get { return (int?)this[8]; } set { this[8] = value; } }
    }

    public class RestLine : RhestLine
    {
        public RestLine()
            : base()
        {
            this[0] = "OPERUH";
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 6 ,"A2"    , "Id"),
                new BaseField( 8  , 13 ,"A6"   , "minemonico"),
                new BaseField(15  , 19 ,"A5"    , "Restricao"),//pode ter letra
                new BaseField(22  , 22 ,"A1"    , "tipo"),//
                new BaseField(25  , 25, "I1"    , "flag incluir"),//
                new BaseField(28  , 39 ,"A12"    , "justificativa"),//pode ter letra
                new BaseField(41  , 50 ,"A10"    , "Valor"),//
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }


    }
    public class ElemLine : RhestLine
    {
        public ElemLine()
            : base()
        {
            this[0] = "OPERUH";
        }

        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 6 ,"A6"    , "Id"),
                new BaseField( 8  , 13 ,"A6"    , "minemonico"),
                new BaseField(15  , 19 ,"A5"    , "Restricao"),//pode ter letra
                new BaseField(21  , 23 ,"I3"    , "usina"),//
                new BaseField(26  , 37 ,"A12"    , "nomeusina"),//
                new BaseField(39  , 39 ,"A1"    , "letra"),//esse campo esta fora do manual mas como em algumas restricoes ele tem algo, esta especificado aqui
                new BaseField(41  , 42 ,"I2"    , "Cod variavel"),//pode ter letra
                new BaseField(44  , 48 ,"F5.1"    , "Fator"),//
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }


    }
    public class LimLine : RhestLine
    {

        public LimLine()
            : base()
        {
            this[0] = "OPERUH";
            //this[2] = 1;
            // this[4] = 1;
        }

        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 6 ,"A6"    , "Id"),
                new BaseField( 8  , 13 ,"A6"   , "minemonico"),
                new BaseField(15  , 19 ,"A5"    , "Restricao"),//pode ter letra
                new BaseField(21  , 22 ,"A2"    , "DiaInic"),//
                new BaseField(24  , 25 ,"I2"    , "HoraDiaInic"),//
                new BaseField(27  , 27 ,"I1"    , "meiahorainic"),//pode ter letra
                new BaseField(29  , 30 ,"A2"    , "DiaFinal"),//
                new BaseField(32  , 33 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(35  , 35 ,"I1"    , "meiahorafim"),//
                new BaseField( 39 , 48,"F10.0"    , "liminf"),
                new BaseField( 49 , 58,"F10.0" , "limsup"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

    }

    public class VarLine : RhestLine
    {

        public VarLine()
            : base()
        {
            this[0] = "OPERUH";
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 6 ,"A6"    , "Id"),
                new BaseField( 8  , 13 ,"A6"  , "minemonico"),
                new BaseField(15  , 19 ,"A5"    , "Restricao"),//pode ter letra
                new BaseField(20  , 21 ,"A2"    , "DiaInic"),//
                new BaseField(23  , 24 ,"I2"    , "HoraDiaInic"),//
                new BaseField(26  , 26 ,"I1"    , "meiahorainic"),//pode ter letra
                new BaseField(28  , 29 ,"A2"    , "DiaFinal"),//
                new BaseField(31  , 32 ,"I2"    , "HoraDiaFinal"),//
                new BaseField( 34 , 34,"I1"    , "meiahorafim"),
                new BaseField( 38 , 47,"F10.5" , "rampamaxdecr"),
                new BaseField( 48 , 57,"F10.5" , "rampamaxacr"),
                new BaseField( 58 , 67,"F10.5" , "rampamaxabsdecr"),
                new BaseField( 68 , 77,"F10.5" , "rampamaxabsacr"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

    }
    public class CondLine : RhestLine
    {

        public CondLine()
            : base()
        {
            this[0] = "OPERUH";
            //this[2] = 1;
            //this[5] = 1;
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 6 ,"A6"    , "Id"),
                new BaseField( 8  , 13 ,"A6"   , "minemonico"),
                new BaseField(15  , 19 ,"A5"    , "Restricao"),//pode ter letra
                new BaseField(21  , 30 ,"F10.0"    , "deltax"),//
                new BaseField(32  , 36 ,"I5"    , "restcontr"),//
                new BaseField(38  , 47 ,"F10.0"    , "deltay"),//
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }


    }

    
}

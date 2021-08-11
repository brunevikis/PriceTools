using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class RheBlock : BaseBlock<RheLine>
    {

        public override RheLine CreateLine(string line = null)
        {

            var cod = line.Substring(0, 2);
            switch (cod)
            {
                case "RE":
                    return (RheLine)BaseLine.Create<ReLine>(line);
                case "LU":
                    return (RheLine)BaseLine.Create<LuLine>(line);
                case "FH":
                    return (RheLine)BaseLine.Create<FhLine>(line);
                case "FT":
                    return (RheLine)BaseLine.Create<FtLine>(line);
                case "FI":
                    return (RheLine)BaseLine.Create<FiLine>(line);
                case "FE":
                    return (RheLine)BaseLine.Create<FeLine>(line);
                case "FR":
                    return (RheLine)BaseLine.Create<FrLine>(line);
                case "FC":
                    return (RheLine)BaseLine.Create<FcLine>(line);
                default:
                    throw new ArgumentException("Invalid identifier " + cod);
            }
        }

        public Dictionary<ReLine, List<RheLine>> RheGrouped
        {
            get
            {

                var temp = new Dictionary<ReLine, List<RheLine>>();
                var restID = new BaseField(5, 7, "I3", "Restricao");

                foreach (var hv in this.Where(x => x is ReLine))
                {

                    var hvID = (int)hv[restID];

                    temp.Add(
                        (ReLine)hv, this.Where(x => (int)x[restID] == hvID).ToList()
                        );
                }

                return temp;
            }
        }


        public int GetNextId() { return this.Max(x => (int)x[1]) + 1; }

        public void Add(LuLine lu)
        {
            var re = this.RheGrouped.Keys.Where(x => x[1] == lu[1]).FirstOrDefault();
            if (re != null)
            {
                var prevLu = RheGrouped[re].LastOrDefault(x => x is LuLine && x[2] < lu[2]);

                var idx = this.IndexOf(prevLu ?? re) + 1;
                this.Insert(idx, lu);
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


    public abstract class RheLine : BaseLine
    {

        public int Restricao { get { return this[1]; } set { this[1] = value; } }

    }

    public class ReLine : RheLine
    {
        public ReLine()
            : base()
        {
            this[0] = "RE";
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"   , "Restricao"),
                new BaseField(10  , 11 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(13  , 14 ,"I2"    , "HoraDiaInic"),//
                new BaseField(16  , 16, "I1"    , "MeiaHoraDiaInic"),//
                new BaseField(18  , 19 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(21  , 22 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(24  , 24 ,"I1"    , "MeiaHoraDiaFinal"),//
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
    }
    public class LuLine : RheLine
    {
        public LuLine()
            : base()
        {
            this[0] = "LU";
        }

        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Restricao"),
                new BaseField(9  , 10 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(12  , 13 ,"I2"    , "HoraDiaInic"),//
                new BaseField(15  , 15 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(17  , 18 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(20  , 21 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(23  , 23 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField( 25 , 34,"F10.0" , "Limite Inf"),
                new BaseField( 35 , 44,"F10.0" , "Limite Sup"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public float LimInf { get { return (float)this[8]; } set { this[8] = value; } }
        public float Limsup { get { return (float)this[9]; } set { this[9] = value; } }

    }
    public class FhLine : RheLine
    {

        public FhLine()
            : base()
        {
            this[0] = "FH";
            //this[2] = 1;
           // this[4] = 1;
        }

        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"   , "Restricao"),
                new BaseField(9  , 10 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(12  , 13 ,"I2"    , "HoraDiaInic"),//
                new BaseField(15  , 15 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(17  , 18 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(20  , 21 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(23  , 23 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField(25  , 27 ,"I3"    , "NumUsina"),//
                new BaseField( 28 , 29,"I2"    , "NumConjMaq"),
                new BaseField( 35 , 44,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int Usina { get { return this[8]; } set { this[8] = value; } }
        public int NumConjMaq { get { return this[9]; } set { this[9] = value; } }
        public float Fator { get { return (float)this[10]; } set { this[10] = value; } }





    }

    public class FtLine : RheLine
    {

        public FtLine()
            : base()
        {
            this[0] = "FT";
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"  , "Restricao"),
                new BaseField(9  , 10 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(12  , 13 ,"I2"    , "HoraDiaInic"),//
                new BaseField(15  , 15 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(17  , 18 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(20  , 21 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(23  , 23 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField( 25 , 27,"I3"    , "NumUsina"),
                new BaseField( 35 , 44,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int Usina { get { return this[8]; } set { this[8] = value; } }
        public float Fator { get { return (float)this[9]; } set { this[9] = value; } }

    }
    public class FiLine : RheLine
    {

        public FiLine()
            : base()
        {
            this[0] = "FI";
            //this[2] = 1;
            //this[5] = 1;
        }
        static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"   , "Restricao"),
                new BaseField(9  , 10 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(12  , 13 ,"I2"    , "HoraDiaInic"),//
                new BaseField(15  , 15 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(17  , 18 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(20  , 21 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(23  , 23 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField( 25 , 26,"A2"    , "De"),
                new BaseField( 30 , 31,"A2"    , "Para"),
                new BaseField( 35 , 44,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public string De { get { return this[8].Trim(); } set { this[8] = value; } }
        public string Para { get { return this[9].Trim(); } set { this[9] = value; } }
        public float Fator { get { return (float)this[10]; } set { this[10] = value; } }

    }

    public class FeLine : RheLine
    {

        public FeLine()
            : base()
        {
            this[0] = "FE";
            //this[2] = 1;
            //this[4] = 1;
        }

        static readonly BaseField[] campos = new BaseField[] {
               new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"   , "Restricao"),
                new BaseField(9  , 10 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(12  , 13 ,"I2"    , "HoraDiaInic"),//
                new BaseField(15  , 15 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(17  , 18 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(20  , 21 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(23  , 23 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField( 25 , 27,"I3"    , "NumContrato"),
                new BaseField( 35 , 44,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }
        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int Contrato { get { return this[8]; } set { this[8] = value; } }

        public float Fator { get { return (float)this[9]; } set { this[9] = value; } }
    }

    public class FrLine : RheLine
    {

        public FrLine()
            : base()
        {
            this[0] = "FR";
            //this[2] = 1;
            //this[4] = 1;
        }

        static readonly BaseField[] campos = new BaseField[] {
               new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"   , "Restricao"),
                new BaseField(11  , 12 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(14  , 15 ,"I2"    , "HoraDiaInic"),//
                new BaseField(17  , 17 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(19  , 20 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(22  , 23 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(25  , 25 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField( 27 , 30,"I4"    , "NumEolica"),
                new BaseField( 35 , 44,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }
        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int NumEolica { get { return this[8]; } set { this[8] = value; } }

        public float Fator { get { return (float)this[9]; } set { this[9] = value; } }
    }

    public class FcLine : RheLine
    {

        public FcLine()
            : base()
        {
            this[0] = "FC";
            //this[2] = 1;
            //this[4] = 1;
        }

        static readonly BaseField[] campos = new BaseField[] {
               new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"   , "Restricao"),
                new BaseField(11  , 12 ,"A2"    , "DiaInic"),//pode ter letra
                new BaseField(14  , 15 ,"I2"    , "HoraDiaInic"),//
                new BaseField(17  , 17 ,"I1"    , "MeiaHoraDiaInic"),//
                new BaseField(19  , 20 ,"A2"    , "DiaFinal"),//pode ter letra
                new BaseField(22  , 23 ,"I2"    , "HoraDiaFinal"),//
                new BaseField(25  , 25 ,"I1"    , "MeiaHoraDiaFinal"),//
                new BaseField( 27 , 29,"I3"    , "NumDemanda"),
                new BaseField( 35 , 44,"F10.0" , "Fator"),
            };


        public override BaseField[] Campos
        {
            get { return campos; }
        }
        public string DiaInic { get { return this[2].ToString(); } set { this[2] = value; } }
        public int HoraInic { get { return (int)this[3]; } set { this[3] = value; } }
        public int MeiaHoraInic { get { return (int)this[4]; } set { this[4] = value; } }
        public string DiaFinal { get { return this[5].ToString(); } set { this[5] = value; } }
        public int HoraFinal { get { return (int)this[6]; } set { this[6] = value; } }
        public int MeiaHoraFinal { get { return (int)this[7]; } set { this[7] = value; } }
        public int NumDemanda { get { return this[8]; } set { this[8] = value; } }

        public float Fator { get { return (float)this[9]; } set { this[9] = value; } }
    }
}

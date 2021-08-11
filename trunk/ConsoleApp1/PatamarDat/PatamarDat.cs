using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.PatamarDat
{
    public class PatamarDat : BaseDocument
    {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Patamares"             , new NumPatBlock()},
                    {"Duracao"               , new DuracaoBlock()},
                    {"Carga"                 , new CargaBlock()},
                    {"Intercambio"           , new IntBlock()},
                };

        public DuracaoBlock Duracao { get { return (DuracaoBlock)blocos["Duracao"]; } }
        public CargaBlock Carga { get { return (CargaBlock)blocos["Carga"]; } }
        public IntBlock Intercambio { get { return (IntBlock)blocos["Intercambio"]; } }



        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }


        public PatamarDat() : base()
        {
        }

        public PatamarDat(string filepath)
        {


            using (var fs = System.IO.File.OpenRead(filepath))
            using (var tr = new System.IO.StreamReader(fs))
            {

                tr.ReadLine();
                tr.ReadLine();

                string line = tr.ReadLine();
                Blocos["Patamares"].Add(
                    ((NumPatBlock)Blocos["Patamares"]).CreateLine(line)
                );

                int nPat = ((int)((NumPatBlock)Blocos["Patamares"])[0][0]);

                tr.ReadLine();
                tr.ReadLine();
                tr.ReadLine();

                int anoT = 0;
                for (int ano = 0; ano < 6; ano++)
                {

                    line = tr.ReadLine();

                    var temp = ((DuracaoBlock)Blocos["Duracao"]).CreateLine(line);

                    if (!(temp[1] is int)) break;

                    temp[0] = 1;
                    Blocos["Duracao"].Add(temp);

                    anoT = (int)temp[1];

                    for (int pat = 2; pat <= nPat; pat++)
                    {

                        line = tr.ReadLine();

                        temp = ((DuracaoBlock)Blocos["Duracao"]).CreateLine(line);
                        temp[0] = pat;
                        temp[1] = anoT;
                        Blocos["Duracao"].Add(temp);

                        anoT = (int)temp[1];
                    }
                }

                tr.ReadLine();
                tr.ReadLine();
                tr.ReadLine();

                line = tr.ReadLine();

                var patT = 1;
                do
                {
                    var newLine = Blocos["Carga"].CreateLine(line);


                    if (newLine is CargaEneLine)
                    {
                        newLine["Submercado"] = Blocos["Carga"].Last()["Submercado"];
                        newLine["Patamar"] = patT;
                        if (!(newLine["Ano"] is int)) newLine["Ano"] = Blocos["Carga"].Last()["Ano"];
                        if (++patT > nPat) patT = 1;
                    }

                    Blocos["Carga"].Add(newLine);


                    line = tr.ReadLine();
                } while (!line.Trim().StartsWith("999"));

                tr.ReadLine();
                tr.ReadLine();
                tr.ReadLine();
                tr.ReadLine();
                tr.ReadLine();

                patT = 1;
                line = tr.ReadLine();
                do
                {
                    var newLine = Blocos["Intercambio"].CreateLine(line);

                    if (newLine is IntABLine)
                    {
                        newLine["Submercado A"] = Blocos["Intercambio"].Last()["Submercado A"];
                        newLine["Submercado B"] = Blocos["Intercambio"].Last()["Submercado B"];
                        newLine["Patamar"] = patT;
                        if (!(newLine["Ano"] is int)) newLine["Ano"] = Blocos["Intercambio"].Last()["Ano"];

                        if (++patT > nPat) patT = 1;
                    }

                    Blocos["Intercambio"].Add(newLine);

                    if (tr.EndOfStream) break;
                    line = tr.ReadLine();
                } while (!line.Trim().StartsWith("999"));


                //while (!tr.EndOfStream && !line.Trim().StartsWith("999"))
                //{
                //    line = tr.ReadLine();

                //    var newLine = Blocos["Intercambio"].CreateLine(line);

                //    if (newLine is IntABLine)
                //    {
                //        newLine["Submercado A"] = Blocos["Intercambio"].Last()["Submercado A"];
                //        newLine["Submercado B"] = Blocos["Intercambio"].Last()["Submercado B"];
                //        newLine["Patamar"] = patT;
                //        if (!(newLine["Ano"] is int)) newLine["Ano"] = Blocos["Intercambio"].Last()["Ano"];

                //        if (++patT > nPat) patT = 1;
                //    }

                //    Blocos["Intercambio"].Add(newLine);
                //}
            }
        }

        public int NumeroPatamares { get { return blocos["Patamares"].First()[0]; } }
    }


    public class CargaBlock : BaseBlock<CargaLine>
    {

        string header =
@" SUBSISTEMA
 XXX
	ANO                       CARGA(P.U.DEMANDA MED.)
   XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX
";

        public override CargaLine CreateLine(string line = null)
        {
            line = line ?? "";

            var id = line.Trim().Split(' ')[0];
            int t;
            if (id.Length <= 3 && int.TryParse(id, out t)) return BaseLine.Create<CargaLine>(line);
            else return BaseLine.Create<CargaEneLine>(line);
        }

        public override string ToText()
        {

            return header + base.ToText() + "9999\n";
        }

    }

    //public abstract class IntLine : BaseLine { }

    public class CargaLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "Submercado"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int? Ano
        {
            get
            {
                if (this is CargaEneLine) return this[0];
                else return null;
            }
            set { this[0] = value; }
        }
        public int Mercado
        {
            get
            {
                return this["Submercado"];
            }
            set { this["Submercado"] = value; }
        }
        public int? Patamar
        {
            get
            {
                if (this is CargaEneLine) return this["Patamar"];
                else return null;
            }
            set { this["Patamar"] = value; }
        }


    }

    public class CargaEneLine : CargaLine
    {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 7 ,"I4"  , "Ano"),
                new BaseField( 9 ,  14 ,"F6.4"  ,  "1"),
                new BaseField( 16 , 21 ,"F6.4"  ,  "2"),
                new BaseField( 23 , 28 ,"F6.4"  ,  "3"),
                new BaseField( 30 , 35 ,"F6.4"  ,  "4"),
                new BaseField( 37 , 42 ,"F6.4"  ,  "5"),
                new BaseField( 44 , 49 ,"F6.4"  ,  "6"),
                new BaseField( 51 , 56 ,"F6.4"  ,  "7"),
                new BaseField( 58 , 63 ,"F6.4"  ,  "8"),
                new BaseField( 65 , 70 ,"F6.4"  ,  "9"),
                new BaseField( 72 , 77 ,"F6.4"  ,  "10"),
                new BaseField( 79 , 84 ,"F6.4"  ,  "11"),
                new BaseField( 86 , 91 ,"F6.4"  , "12"),

                new BaseField( 0 , 0 ,"I3"  , "Submercado"),
                new BaseField( 0 , 0 ,"I3"  , "Patamar"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public new int Ano { get { return this[0]; } set { this[0] = value; } }
        public new int Patamar { get { return this["Patamar"]; } set { this["Patamar"] = value; } }
        public new int Mercado { get { return this["Submercado"]; } set { this["Submercado"] = value; } }

    }


    public class IntBlock : BaseBlock<IntLine>
    {

        string header =
@" SUBSISTEMA
   A ->B
 XXX XXX
                             INTERCAMBIO(P.U.INTERC.MEDIO)
 X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX X.XXXX
";


        public override IntLine CreateLine(string line = null)
        {
            line = (line ?? "");
            var id = line.Trim().Split(' ')[0];

            if (id.Length < 4)
            {
                return BaseLine.Create<IntLine>(line);
            }
            else
            {
                return BaseLine.Create<IntABLine>(line);
            }
        }

        public override string ToText()
        {

            return header + base.ToText() + "9999\n";
        }

    }

    //public abstract class IntLine : BaseLine { }

    public class IntLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 2 , 4 ,"I3"  , "Submercado A"),
                new BaseField( 6 , 8 ,"I3"  , "Submercado B"),
                new BaseField( 24 , 24 ,"I1"  , "Flag"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public int? Ano
        {
            get
            {
                if (this is IntABLine) return this[0];
                else return null;
            }
            set { this[0] = value; }
        }
        public int SubmercadoA
        {
            get
            {
                return this["Submercado A"];
            }
            set { this["Submercado A"] = value; }
        }
        public int SubmercadoB
        {
            get
            {
                return this["Submercado B"];
            }
            set { this["Submercado B"] = value; }
        }
        public int? Patamar
        {
            get
            {
                if (this is IntABLine) return this["Patamar"];
                else return null;
            }
            set { this["Patamar"] = value; }
        }
    }

    public class IntABLine : IntLine
    {
        public static readonly new BaseField[] campos = new BaseField[] {
                new BaseField( 4 , 7 ,"I4"  , "Ano"),
                new BaseField( 9  , 14,"F6.4"  ,  "Intercambio 1"),
                new BaseField( 16 , 21,"F6.4"  ,  "Intercambio 2"),
                new BaseField( 23 , 28,"F6.4"  ,  "Intercambio 3"),
                new BaseField( 30 , 35,"F6.4"  ,  "Intercambio 4"),
                new BaseField( 37 , 42,"F6.4"  ,  "Intercambio 5"),
                new BaseField( 44 , 49,"F6.4"  ,  "Intercambio 6"),
                new BaseField( 51 , 56,"F6.4"  ,  "Intercambio 7"),
                new BaseField( 58 , 63,"F6.4"  ,  "Intercambio 8"),
                new BaseField( 65 , 70,"F6.4"  ,  "Intercambio 9"),
                new BaseField( 72 , 77,"F6.4"  ,  "Intercambio 10"),
                new BaseField( 79 , 84,"F6.4"  ,  "Intercambio 11"),
                new BaseField( 86 , 91,"F6.4"  ,  "Intercambio 12"),
                new BaseField( 0 , 0 ,"I3"  , "Submercado A"),
                new BaseField( 0 , 0 ,"I3"  , "Submercado B"),
                 new BaseField( 0 , 0 ,"I3"  , "Patamar"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }
    }

}

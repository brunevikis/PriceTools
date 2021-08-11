using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1.Inviab
{
    public class Inviab : BaseDocument
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"SimulacaoFinal"             , new InviabFinalBlock()},
                    {"Iteracao"             , new InviabIteracaoBlock()},
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }

        public InviabFinalBlock SimulacaoFinal { get { return (InviabFinalBlock)blocos["SimulacaoFinal"]; } }
        public InviabIteracaoBlock Iteracao { get { return (InviabIteracaoBlock)blocos["Iteracao"]; } }


        public Inviab(string filepath)
            : base()
        {
            using (var fs = System.IO.File.OpenRead(filepath))
            using (var tr = new System.IO.StreamReader(fs))
            {


                do
                {
                    string line = tr.ReadLine();

                    if (Regex.IsMatch(line, @"RELATORIO\sDE\sVIOLACOES", RegexOptions.IgnoreCase))
                    {

                        CarregarInviabIteracao(tr);



                    }
                    else
                        if (Regex.IsMatch(line, @"SIMULACAO\s+FINAL:", RegexOptions.IgnoreCase))
                    {

                        CarregarInviabFinal(tr);

                        break;

                    }



                } while (!tr.EndOfStream);
            }

        }
        private void CarregarInviabFinal(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();

            for (var line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
            {

                var l = SimulacaoFinal.CreateLine(line);
                SimulacaoFinal.Add(l);

                if (sr.EndOfStream) break;
            }
        }
        private void CarregarInviabIteracao(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();

            for (var line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
            {

                var l = Iteracao.CreateLine(line);
                Iteracao.Add(l);

                if (sr.EndOfStream) break;
            }
        }


    }

    public class InviabFinalBlock : BaseBlock<InviabFinalLine>
    {


    }
    public class InviabIteracaoBlock : BaseBlock<InviabIteracaoLine>
    {


    }

    public abstract class InviabLine : BaseLine
    {
        public abstract int Estagio { get; }
        public abstract int Cenario { get; }
        public abstract string RestricaoViolada { get; }
        public abstract double Violacao { get; }

        public int? Patamar
        {
            get
            {

                var m = System.Text.RegularExpressions.Regex.Match(RestricaoViolada,
                    @"(?<=PATAMAR\s+)\d\b",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase
                     );
                if (m.Success)
                    return int.Parse(m.Value);
                else
                    return null;
            }
        }
        public int? CodRestricao
        {
            get
            {

                var m = System.Text.RegularExpressions.Regex.Match(RestricaoViolada,
                    @"(?<=[RESTRICAO\s+ELETRICA\s+|RHQ\s+|RHV\s+])\d+\b",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase
                     );
                if (m.Success)
                    return int.Parse(m.Value);


                else
                    return null;

            }

        }
        public string TipoRestricao
        {
            get
            {
                if (RestricaoViolada.ToUpperInvariant().Contains("RESTRICAO ELETRICA"))
                {
                    return "RHE";
                }
                else if (RestricaoViolada.ToUpperInvariant().Contains("RHQ"))
                {
                    return "RHQ";
                }
                else if (RestricaoViolada.ToUpperInvariant().Contains("RHV"))
                {
                    return "RHV";
                }
                else if (RestricaoViolada.ToUpperInvariant().Contains("EVAPORACAO"))
                {
                    return "EVAP";
                }
                else if (RestricaoViolada.ToUpperInvariant().Contains("IRRIGACAO"))
                {
                    return "IRRI";
                }
                else return null;
            }
        }
        public string SupInf
        {
            get
            {
                if (RestricaoViolada.ToUpperInvariant().Contains("(L. INF)"))
                {
                    return "INF";
                }
                else if (RestricaoViolada.ToUpperInvariant().Contains("(L. SUP)"))
                {
                    return "SUP";
                }
                else return null;
            }
        }

        public string Usina
        {
            get
            {
                var m = System.Text.RegularExpressions.Regex.Match(RestricaoViolada,
                    @"(?<=USINA\s).+",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase | RegexOptions.Singleline
                     );
                if (m.Success)
                    return m.Value.Trim();
                else
                    return null;
            }
        }
    }
    public class InviabFinalLine : InviabLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(5    , 12 ,"I4"   , "Estagio"),
                new BaseField(14   , 21 ,"I4"   , "Cenario"),
                new BaseField(23   , 98,"A76"  , "RestricaoViolada"),
                new BaseField(100  , 115,"F15.8", "Violacao"),
                new BaseField(117  , 121 ,"A5"   , "Unidade"),

        };
        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public override int Estagio
        {
            get
            {
                return valores[campos[0]]; ;
            }
        }

        public override int Cenario { get { return valores[campos[1]]; } }

        public override string RestricaoViolada { get { return valores[campos[2]]; } }

        public override double Violacao { get { return valores[campos[3]]; } }



    }
    public class InviabIteracaoLine : InviabLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(5    , 13 ,"I4"   , "Iteracao"),
                new BaseField(15   , 28 ,"I4"   , "FwdBwd"),
                new BaseField(30   , 37 ,"I4"   , "Estagio"),
                new BaseField(39   , 46 ,"I4"   , "Cenario"),
                new BaseField(48   , 98,"A76"  , "RestricaoViolada"),
                new BaseField(100  , 115,"F15.8", "Violacao"),
                new BaseField(117  , 121 ,"A5"   , "Unidade"),

        };
        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public override int Estagio
        {
            get
            {
                return valores[campos[2]]; ;
            }
        }

        public override int Cenario { get { return valores[campos[3]]; } }

        public override string RestricaoViolada { get { return valores[campos[4]]; } }

        public override double Violacao { get { return valores[campos[5]]; } }




    }


}


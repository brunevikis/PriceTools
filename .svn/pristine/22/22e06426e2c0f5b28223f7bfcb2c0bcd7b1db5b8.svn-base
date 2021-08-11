using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ConsultaNwd {
    public class ConsultaNwd : BaseDocument {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
            {"REE"                  , new ReeBlock()},   
            {"MERCADO"              , new MercadoBlock()},
            {"PATAMARES"            , new PatamarBlock()},
            {"CMO"                  , new CmoBlock()},

        };
        Rees rees = null;
        Mercados mercados = null;

        public string DataInicial { get; set; }
        public string DataFinal { get; set; }

        public string Mes {
            get {
                var mes = DataInicial.Trim().Split('/')[0];
                mes = (new DateTime(2000, int.Parse(mes), 1)).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
                return mes.ToUpper();
            }
        }
        public string MesAnterior {
            get {

                var mes = DataInicial.Trim().Split('/')[0];
                mes = (new DateTime(2000, int.Parse(mes), 1)).AddMonths(-1).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
                return mes.ToUpper();
            }
        }

        public ReeBlock Ree { get { return (ReeBlock)blocos["REE"]; } }
        public MercadoBlock Mercado { get { return (MercadoBlock)blocos["MERCADO"]; } }
        public PatamarBlock Patamar { get { return (PatamarBlock)blocos["PATAMARES"]; } }
        public CmoBlock Cmo { get { return (CmoBlock)blocos["CMO"]; } }

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public ConsultaNwd(string filepath) {


            using (var fs = System.IO.File.OpenRead(filepath))
            using (var tr = new System.IO.StreamReader(fs)) {

                var earmiok = false;
                var readingCMO = false;
                var readingMercado = false;
                var readingMercadoL = false;
                var readingGerHidr = false;
                var readingGerHidMinr = false;
                var readingGerTerm = false;
                var readingGerTermInflex = false;
                var readingGerPeq = false;

                string line = tr.ReadLine();
                do {
                    if (line.Contains("MES INICIAL")) {
                        DataInicial = line.Split(':')[1].Trim();
                    } else if (line.Contains("MES FINAL")) {
                        DataFinal = line.Split(':')[1].Trim();
                    } else if (line.TrimStart().StartsWith("DATA") && rees == null) {
                        rees = new Rees();
                        rees.Load(line);

                        foreach (var ree in rees.Campos) {
                            if (!string.IsNullOrWhiteSpace(rees[ree])) {
                                var rl = Ree.CreateLine();
                                rl[0] = rees[ree].Trim();
                                Ree.Add(rl);
                            }
                        }

                    } else if (line.TrimStart().StartsWith("DATA") && mercados == null) {
                        mercados = new Mercados();
                        mercados.Load(line);

                        foreach (var m in mercados.Campos) {
                            if (!string.IsNullOrWhiteSpace(mercados[m])) {
                                var rl = Mercado.CreateLine();

                                rl[0] = mercados[m].Trim();
                                rl.DemandaLiq = 0.0;
                                rl.DemandaBru = 0.0;
                                rl.GerPeq = rl.GerHidr = rl.GerHidrMin = rl.GerTerm = rl.GerTermInflex = 0.0;


                                Mercado.Add(rl);
                            }
                        }

                    } else if (line.TrimStart().StartsWith("EARMI") && !earmiok) {

                        ReesValorA rv = new ReesValorA();
                        rv.Load(line);

                        for (int i = 0; i < Ree.Count; i++) {

                            Ree[i].Earmi = rv[i + 1];

                        }
                        earmiok = true;
                    } else if (line.TrimStart().StartsWith("EARMF")) {
                        ReesValorB rv = new ReesValorB(); rv.Load(line);
                        for (int i = 0; i < Ree.Count; i++) Ree[i].Earmf = rv[i];
                    } else if (line.TrimStart().StartsWith("EARMx")) {
                        ReesValorB rv = new ReesValorB(); rv.Load(line);
                        for (int i = 0; i < Ree.Count; i++) {
                            Ree[i].Earmx = rv[i];
                            Ree[i].EarmP = Ree[i].Earmx > 0 ? Ree[i].Earmi / Ree[i].Earmx : 0;
                        }
                    } else if (line.TrimStart().StartsWith("EAF")) {
                        ReesValorA rv = new ReesValorA();
                        rv.Load(line);
                        var mes = rv[0].Trim().Split('/')[0];
                        mes = (new DateTime(2000, int.Parse(mes), 1)).ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));

                        for (int i = 0; i < Ree.Count; i++) {

                            Ree[i]["ENA " + mes] = rv[i + 1];

                        }
                    } else if (line.Contains("DURACAO DOS PATAMARES")) {
                        do {
                            line = tr.ReadLine();
                            if (!line.TrimStart().StartsWith("PAT")) {
                                break;
                            }
                            var nl = Patamar.CreateLine(line);
                            Patamar.Add(nl);

                        } while (!tr.EndOfStream);

                    } else if (line.TrimStart().StartsWith("CMO") || readingCMO) {

                        readingCMO = true;

                        CmoValorA rv = new CmoValorA();
                        rv.Load(line);
                        if (rv[0] == "PAT") {

                            var pat = rv[1];

                            int m = 2;
                            foreach (var mer in Mercado) {
                                var cl = Cmo.CreateLine();
                                cl[0] = pat;
                                cl[1] = mer[0];
                                cl[2] = rv[m++];

                                Cmo.Add(cl);
                            }
                        } else if (!string.IsNullOrWhiteSpace(rv[0])) readingCMO = false;
                    } else if (line.TrimStart().StartsWith("MERCADO BRUTO") || readingMercado) {

                        if (readingMercado) {


                            DadosMercadoValorA rv = new DadosMercadoValorA();
                            rv.Load(line);
                            if (rv[0] == "PAT") {

                                int m = 0;
                                foreach (var mer in Mercado) mer.DemandaBru += rv[(++m) * 2];
                            } else if (!string.IsNullOrWhiteSpace(rv[0])) { readingMercado = false; continue; }
                        } else readingMercado = true;

                    } else if (line.TrimStart().StartsWith("MERCADO LIQUIDO") || readingMercadoL) {

                        if (readingMercadoL) {
                            DadosMercadoValorA rv = new DadosMercadoValorA();
                            rv.Load(line);
                            if (rv[0] == "PAT") {
                                int m = 0;
                                foreach (var mer in Mercado) mer.DemandaLiq += rv[(++m) * 2];
                            } else if (!string.IsNullOrWhiteSpace(rv[0])) { readingMercadoL = false; continue; }
                        } else readingMercadoL = true;

                    } else if (line.TrimStart().StartsWith("GERACAO HIDRAULICA TOTAL") || readingGerHidr) {

                        if (readingGerHidr) {
                            DadosMercadoValorA rv = new DadosMercadoValorA();
                            rv.Load(line);
                            if (rv[0] == "PAT") {
                                int m = 0;
                                foreach (var mer in Mercado) mer.GerHidr += rv[(++m) * 2];
                            } else if (!string.IsNullOrWhiteSpace(rv[0])) { readingGerHidr = false; continue; }
                        } else readingGerHidr = true;

                    } else if (line.TrimStart().StartsWith("META DE GERACAO HIDRAULICA MINIMA") || readingGerHidMinr) {

                        if (readingGerHidMinr) {
                            DadosMercadoValorA rv = new DadosMercadoValorA();
                            rv.Load(line);
                            if (rv[0] == "PAT") {
                                int m = 0;
                                foreach (var mer in Mercado) mer.GerHidrMin += rv[(++m) * 2];
                            } else if (!string.IsNullOrWhiteSpace(rv[0])) { readingGerHidMinr = false; continue; }
                        } else readingGerHidMinr = true;

                    } else if (line.TrimStart().StartsWith("GERACAO TERMICA TOTAL") || readingGerTerm) {

                        if (readingGerTerm) {
                            DadosMercadoValorA rv = new DadosMercadoValorA();
                            rv.Load(line);
                            if (rv[0] == "PAT") {
                                int m = 0;
                                foreach (var mer in Mercado) mer.GerTerm += rv[(++m) * 2];
                            } else if (!string.IsNullOrWhiteSpace(rv[0])) { readingGerTerm = false; continue; }
                        } else readingGerTerm = true;

                    } else if (line.TrimStart().StartsWith("GERACAO TERMICA MINIMA") || readingGerTermInflex) {

                        if (readingGerTermInflex) {
                            DadosMercadoValorA rv = new DadosMercadoValorA();
                            rv.Load(line);
                            if (rv[0] == "PAT") {
                                int m = 0;
                                foreach (var mer in Mercado) mer.GerTermInflex += rv[(++m) * 2];
                            } else if (!string.IsNullOrWhiteSpace(rv[0])) { readingGerTermInflex = false; continue; }
                        } else readingGerTermInflex = true;

                    } else if (line.TrimStart().StartsWith("GERACAO DE PEQUENAS USINAS") || readingGerPeq) {

                        if (readingGerPeq) {
                            DadosMercadoValorA rv = new DadosMercadoValorA();
                            rv.Load(line);
                            if (rv[0] == "PAT") {
                                int m = 0;
                                foreach (var mer in Mercado) mer.GerPeq += rv[(++m) * 2];
                            } else if (!string.IsNullOrWhiteSpace(rv[0])) { readingGerPeq = false; continue; }
                        } else readingGerPeq = true;
                    }

                    line = tr.ReadLine();

                } while (!tr.EndOfStream);
            }
        }

    }

    public class CmoBlock : BaseBlock<CmoLine> { }
    public class CmoLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(0 , 0 ,"I1"  , "PAT"),                                
           new BaseField(0 , 0 ,"A12"  , "MERCADO"),
           new BaseField(0 , 0 ,"F10.3"  , "CMO")
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Pat { get { return this["PAT"]; } set { this["PAT"] = value; } }
        public String Mercado { get { return this["MERCADO"]; } set { this["MERCADO"] = value; } }
        public double Cmo { get { return this["CMO"]; } set { this["CMO"] = value; } }


    }

    public class ReeBlock : BaseBlock<ReeLine> { }
    public class ReeLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(0 , 0 ,"A2"  , "REE"),                                
           new BaseField(0 , 0 ,"F10.2"  , "EARMi"),
           new BaseField(0 , 0 ,"F10.2"  , "EARMf"),
           new BaseField(0 , 0 ,"F10.2"  , "EARMx"),
           new BaseField(0 , 0 ,"F5.2"   , "EARM %"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA JAN"),//5
           new BaseField(0 , 0 ,"F10.2"  , "ENA FEV"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA MAR"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA ABR"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA MAI"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA JUN"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA JUL"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA AGO"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA SET"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA OUT"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA NOV"),
           new BaseField(0 , 0 ,"F10.2"  , "ENA DEZ"),

        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public string Ree { get { return this["REE"]; } set { this["REE"] = value; } }
        public double Earmi { get { return this["EARMi"]; } set { this["EARMi"] = value; } }
        public double Earmf { get { return this["EARMf"]; } set { this["EARMf"] = value; } }
        public double Earmx { get { return this["EARMx"]; } set { this["EARMx"] = value; } }
        public double EarmP { get { return this["EARM %"]; } set { this["EARM %"] = value; } }


        public double Ena { get { return this["EARM %"]; } set { this["EARM %"] = value; } }

    }

    public class MercadoBlock : BaseBlock<MercadoLine> { }
    public class MercadoLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(0 , 0 ,"A12"  , "MERCADO"),                                
           new BaseField(0 , 0 ,"F10.2"  , "EARMi"),
           new BaseField(0 , 0 ,"F10.2"  , "EARMf"),
           new BaseField(0 , 0 ,"F10.2"  , "EARMx"),
           new BaseField(0 , 0 ,"F5.2"  , "EARM %"),          
           new BaseField(0 , 0 ,"F10.0"  , "DEMANDA BRUTA"),          
           new BaseField(0 , 0 ,"F10.0"  , "DEMANDA LIQUI"),    
      new BaseField(0 , 0 ,"F10.0"  , "GHIDR"),    
      new BaseField(0 , 0 ,"F10.0"  , "GHIDR MIN"),   
      new BaseField(0 , 0 ,"F10.0"  , "GTERM"),    
      new BaseField(0 , 0 ,"F10.0"  , "GTERM INFLEX"),    
      new BaseField(0 , 0 ,"F10.0"  , "GPEQ"),    
      





        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public string Mercado { get { return this["MERCADO"].Trim(); } set { this["MERCADO"] = value; } }
        public double Earmi { get { return this["EARMi"]; } set { this["EARMi"] = value; } }
        public double Earmf { get { return this["EARMf"]; } set { this["EARMf"] = value; } }
        public double Earmx { get { return this["EARMx"]; } set { this["EARMx"] = value; } }
        public double EarmP { get { return this["EARM %"]; } set { this["EARM %"] = value; } }
        public double DemandaLiq { get { return this["DEMANDA LIQUI"]; } set { this["DEMANDA LIQUI"] = value; } }
        public double DemandaBru { get { return this["DEMANDA BRUTA"]; } set { this["DEMANDA BRUTA"] = value; } }
        public double GerHidr { get { return this["GHIDR"]; } set { this["GHIDR"] = value; } }
        public double GerHidrMin { get { return this["GHIDR MIN"]; } set { this["GHIDR MIN"] = value; } }
        public double GerTerm { get { return this["GTERM"]; } set { this["GTERM"] = value; } }
        public double GerTermInflex { get { return this["GTERM INFLEX"]; } set { this["GTERM INFLEX"] = value; } }



        public double GerPeq { get { return this["GPEQ"]; } set { this["GPEQ"] = value; } }
    }

    public class Rees : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(24  , 38      ,"A15"  , "REE1"),                                
           new BaseField(39  , 53      ,"A15"  , "REE2"),                                
           new BaseField(54  , 68      ,"A15"  , "REE3"),                                
           new BaseField(69  , 83      ,"A15"  , "REE4"),                                
           new BaseField(84  , 98      ,"A15"  , "REE5"),                                
           new BaseField(99  , 113     ,"A15"  , "REE6"),                                
           new BaseField(114 , 128     ,"A15"  , "REE7"),                                
           new BaseField(129 , 143     ,"A15"  , "REE8"),                                
           new BaseField(144 , 158     ,"A15"  , "REE9"),                                
           new BaseField(159 , 173     ,"A15"  , "REE10"),                                
           new BaseField(174 , 188     ,"A15"  , "REE11"),                                
           new BaseField(189 , 203     ,"A15"  , "REE12"),                                
           new BaseField(204 , 218     ,"A15"  , "REE13"),                                
           new BaseField(219 , 233     ,"A15"  , "REE14"),
           new BaseField(234 , 248     ,"A15"  , "REE15"),                                

        };
        public override BaseField[] Campos {
            get { return campos; }
        }
    }
    public class Mercados : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(21  , 42      ,"A15"  , "M1"),                                
           new BaseField(43  , 64      ,"A15"  , "M2"),                                
           new BaseField(65  , 86      ,"A15"  , "M3"),                                
           new BaseField(87  , 108      ,"A15"  , "M4"),                                
                              

        };
        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class PatamarBlock : BaseBlock<PatamarLine> { }
    public class PatamarLine : BaseLine {

        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(12 , 12 ,"I1"  , "PATAMAR"),                                
           new BaseField(14 , 24 ,"F10.5"  , "DURACAO"),
           

        };

        public override BaseField[] Campos {
            get { return campos; }
        }

    }

    class ReesValorA : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
           new BaseField(11  , 17      ,"A7"  , "DATA"),                                
           new BaseField(18  , 29      ,"F15"  , "REE1"),                                
           new BaseField(30  , 44      ,"F15"  , "REE2"),                                
           new BaseField(45  , 59      ,"F15"  , "REE3"),                                
           new BaseField(60  , 74      ,"F15"  , "REE4"),                                
           new BaseField(75  , 89      ,"F15"  , "REE5"),                                
           new BaseField(90  , 104     ,"F15"  , "REE6"),                                
           new BaseField(105 , 119     ,"F15"  , "REE7"),                                
           new BaseField(120 , 134     ,"F15"  , "REE8"),                                
           new BaseField(135 , 149     ,"F15"  , "REE9"),                                
           new BaseField(150 , 164     ,"F15"  , "REE10"),                                
           new BaseField(165 , 179     ,"F15"  , "REE11"),                                
           new BaseField(180 , 194     ,"F15"  , "REE12"),                                
           new BaseField(195 , 209     ,"F15"  , "REE13"),                                
           new BaseField(210 , 224     ,"F15"  , "REE14"),
           new BaseField(225 , 239     ,"F15"  , "REE15"),                                

        };
        public override BaseField[] Campos {
            get { return campos; }
        }
    }
    class ReesValorB : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                                         
           new BaseField(7  , 21      ,"F15"  , "REE1"),                                
           new BaseField(22  , 36      ,"F15"  , "REE2"),                                
           new BaseField(37  , 51      ,"F15"  , "REE3"),                                
           new BaseField(52  , 66      ,"F15"  , "REE4"),                                
           new BaseField(67  , 81      ,"F15"  , "REE5"),                                
           new BaseField(82  , 96     ,"F15"   , "REE6"),                                
           new BaseField(97  , 111    ,"F15"   , "REE7"),                                
           new BaseField(112 , 126     ,"F15"  , "REE8"),                                
           new BaseField(127 , 141     ,"F15"  , "REE9"),                                
           new BaseField(142 , 156     ,"F15"  , "REE10"),                                
           new BaseField(157 , 171     ,"F15"  , "REE11"),                                
           new BaseField(172 , 186     ,"F15"  , "REE12"),                                
           new BaseField(187 , 201     ,"F15"  , "REE13"),                                
           new BaseField(202 , 216     ,"F15"  , "REE14"),
           new BaseField(217 , 231     ,"F15"  , "REE15"),                                

        };
        public override BaseField[] Campos {
            get { return campos; }
        }
    }
    class CmoValorA : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {         
           new BaseField(7   ,  9      ,"A3"   , "ID"),            
           new BaseField(11  , 11      ,"I1"   , "PAT"),                      
           new BaseField(12  , 21      ,"F15"  , "CMO1"),                                
           new BaseField(25  , 36      ,"F15"  , "CMO2"),                                
           new BaseField(40  , 51      ,"F15"  , "CMO3"),                                
           new BaseField(55  , 66      ,"F15"  , "CMO4"),                                
           new BaseField(70  , 81      ,"F15"  , "CMO5"),                                
           new BaseField(85  , 96      ,"F15"  , "CMO6"),                                
           new BaseField(100 , 111     ,"F15"  , "CMO7"),                                
           new BaseField(115 , 126     ,"F15"  , "CMO8"),                                
           new BaseField(130 , 141     ,"F15"  , "CMO9"),     
        };
        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    class DadosMercadoValorA : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {         
           new BaseField(8   , 10      ,"A3"   , "ID"),            
           new BaseField(12  , 12      ,"I1"   , "PAT"),                      
           new BaseField(14  , 21      ,"F15"  , "MWmes"),                                
           new BaseField(23  , 30      ,"F15"  , "Mwmed"),                                
           new BaseField(36  , 43      ,"F15"  , "MWmes"),                                
           new BaseField(45  , 52      ,"F15"  , "Mwmed"),                                
           new BaseField(58  , 65      ,"F15"  , "MWmes"),                                
           new BaseField(67  , 74      ,"F15"  , "Mwmed"),                                
           new BaseField(80  , 87      ,"F15"  , "MWmes"),                                
           new BaseField(89  , 96      ,"F15"  , "Mwmed"),  
        };
        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}


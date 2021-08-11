using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Relato {
    public class Relato : BaseDocument {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    
                    {"CMO" , new RelatoCmoBlock()},   
                    {"Operacao" , new RelatoOpsBlock()},                 
                    {"Dados Term" , new RelatoDadosTermBlock()}, 
                    {"Operacao Term" , new RelatoOpsTermBlock()}, 
                    {"Tendencia Hidro Meses", new RelatoTHMenBlock()}, 
                    {"Vol Util" , new RelatoVolUtilBlock()},    
                    {"Potencia Disponivel" , new RelatoPotenciaDispBlock()},    
                    {"Balanco Energetico" , new RelatoBalEneBlock()},    
                    {"Dados Mercado" , new RelatoDadosMercadoBlock()},    
                    {"Intercambios" , new RelatoIntercBlock()},    
                    {"EnergiaSistema" , new RelatoEarmSistBlock()},    
                    {"EnergiaAcoplamentoSistema" , new RelatEnaAcoplBlock()},
                    {"RestricaoEletrica" , new RelatoRestricaoEletricaBlock()},    
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public RelatoVolUtilBlock VolUtil {
            get {
                return (RelatoVolUtilBlock)blocos["Vol Util"];
            }
        }

        public RelatoOpsBlock Operacao {
            get {
                return ((RelatoOpsBlock)Blocos["Operacao"]);
            }
        }
        public RelatoCmoBlock CMO {
            get {
                return ((RelatoCmoBlock)Blocos["CMO"]);
            }
        }
        public RelatoBalEneBlock BalancoEnergetico {
            get {
                return ((RelatoBalEneBlock)Blocos["Balanco Energetico"]);
            }
        }
        public RelatoIntercBlock Intercambios {
            get {
                return ((RelatoIntercBlock)Blocos["Intercambios"]);
            }
        }
        public RelatoEarmSistBlock EnergiaSistema {
            get {
                return ((RelatoEarmSistBlock)Blocos["EnergiaSistema"]);
            }
        }
        public RelatoRestricaoEletricaBlock RestricaoEletrica {
            get {
                return ((RelatoRestricaoEletricaBlock)Blocos["RestricaoEletrica"]);
            }
        }
        public RelatoDadosTermBlock DadosTerm {
            get {
                return ((RelatoDadosTermBlock)Blocos["Dados Term"]);
            }
        }
        public RelatoOpsTermBlock OperTerm {
            get {
                return ((RelatoOpsTermBlock)Blocos["Operacao Term"]);
            }
        }
        public RelatoDadosMercadoBlock DadosMercado {
            get {
                return ((RelatoDadosMercadoBlock)Blocos["Dados Mercado"]);
            }
        }
        public RelatoTHMenBlock THMensal {
            get {
                return ((RelatoTHMenBlock)Blocos["Tendencia Hidro Meses"]);
            }
        }

        public RelatEnaAcoplBlock EnergiaAcoplamentoSistema {
            get {
                return ((RelatEnaAcoplBlock)Blocos["EnergiaAcoplamentoSistema"]);
            }
        }

        public DateTime PeriodoInicio { get; set; }


        public Relato(string filepath)
            : base() {
            using (var fs = System.IO.File.OpenRead(filepath))
            using (var tr = new System.IO.StreamReader(fs)) {

                int estagio = 1;

                bool carregarBalancoEne = false;
                bool carregarRestricoesEletricas = false;
                bool carregarDadosTerm = false;
                bool carregarOperTerm = false;
                bool carregarDadosMercado = false;
                bool carregarENAAcopl = false;
                bool carregaVolUtil = false;
                bool carregaOps = false;
                bool carregarENATh = false;
                bool periodoInicio = true;

                do {
                    string line = tr.ReadLine();

                    if (String.IsNullOrWhiteSpace(line)) continue;


                    if (carregarBalancoEne && Regex.IsMatch(line, @"RELATORIO\s+DO\s+BALANCO\s+ENERGETICO", RegexOptions.IgnoreCase)) {
                        carregarBalancoEne = false;
                        CarregarBalancoEne(tr);
                        carregaOps = true;
                        carregaVolUtil = true;
                    }
                    if (carregarRestricoesEletricas && Regex.IsMatch(line, @"Relatorio\s+das\s+Restricoes\s+Eletricas[^\d]+(\d+)", RegexOptions.IgnoreCase)) {
                        int patamar = int.Parse(Regex.Match(line, @"Relatorio\s+das\s+Restricoes\s+Eletricas[^\d]+(\d+)", RegexOptions.IgnoreCase).Groups[1].Value);
                        CarregarRestricoesEletricas(tr, estagio, patamar);

                        if (patamar == 3) { carregarRestricoesEletricas = false; carregarOperTerm = true; }
                    }

                    if (carregarDadosTerm && Regex.IsMatch(line, @"Relatorio\s+dos\s+Dados\s+de\s+Usinas\s+Termicas", RegexOptions.IgnoreCase)) {
                        carregarDadosTerm = false;
                        CarregarDadosTerm(tr);
                        carregarDadosMercado = true;
                    }
                    if (carregarOperTerm && Regex.IsMatch(line, @"RELATORIO\s+DA\s+OPERACAO\s+TERMICA\s+E\s+CONTRATOS", RegexOptions.IgnoreCase)) {
                        carregarOperTerm = false;
                        CarregarOperTerm(tr);
                        carregarBalancoEne = true;
                    }
                    if (carregarDadosMercado && Regex.IsMatch(line, @"Relatorio\s+dos\s+Dados\s+de\s+Mercado", RegexOptions.IgnoreCase)) {
                        carregarDadosMercado = false;
                        CarregarDadosMercado(tr);
                        carregarENAAcopl = true;
                    }
                    if (carregarENAAcopl && line.Contains("Relatorio") && line.Contains("Energia Natural Afluente") &&
                        line.Contains("Subsistema")) {
                        carregarENAAcopl = false;
                        CarregarENAAcopl(tr);
                        carregarENATh = true;
                    }
                    if (carregaVolUtil && line.Contains("VOLUME UTIL DOS RESERVATORIOS")) {
                        carregaVolUtil = false;
                        CarregaVolUtil(tr);

                    }
                    if (carregaOps && line.Trim().Equals("RELATORIO  DA  OPERACAO")) {
                        carregaOps = false;
                        carregaVolUtil = false;
                        estagio = CarregaOps(tr);
                        carregarRestricoesEletricas = true;
                    }

                    if (carregarENATh && line.Contains("RELATORIO DOS DADOS DE ENERGIA NATURAL AFLUENTE POR SUBSISTEMA (MESES PRE-ESTUDO)")) {
                        carregarENATh = false;
                        CarregarENATh(tr);
                        carregaOps = true;

                    }

                    if (periodoInicio && line.Contains("Inicio do periodo")) {
                        periodoInicio = false;

                        var t = line.Split(new string[] { "--->" }, StringSplitOptions.None)[1];

                        DateTime dt;
                        if (DateTime.TryParseExact(t.Trim().Remove(3, 13), @"MMMyyyy",
                            System.Globalization.CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat,
                            System.Globalization.DateTimeStyles.AllowInnerWhite,
                            out dt)) {
                            PeriodoInicio = dt;
                        }

                        carregarDadosTerm = true;
                    }







                } while (!tr.EndOfStream);
            }


            this.Load(System.IO.File.ReadAllText(filepath));
        }

        private void CarregarDadosMercado(System.IO.StreamReader sr) {
            RelatoDadosMercadoLine prev = null;

            for (var line = sr.ReadLine(); !sr.EndOfStream && !string.IsNullOrWhiteSpace(line); line = sr.ReadLine()) {

                var l = DadosMercado.CreateLine(line);
                l[1] = l[1].Trim();
                if (l[0] != null) {
                    DadosMercado.Add(l);
                    prev = l;
                } else if (l[2] is double && prev != null) {

                    l[0] = prev[0];

                    DadosMercado.Add(l);
                }
            }
        }

        private void CarregarDadosTerm(System.IO.StreamReader sr) {

            sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine();


            RelatoDadosTermLine prev = null;

            for (var line = sr.ReadLine(); !sr.EndOfStream && !line.Contains("X-------X"); line = sr.ReadLine()) {

                var l = DadosTerm.CreateLine(line);

                l[1] = l[1].Trim();
                l[2] = l[2].Trim();

                if (l[0] != null) {
                    DadosTerm.Add(l);
                    prev = l;
                } else if (l[3] is int && prev != null) {

                    l[0] = prev[0];
                    l[1] = prev[1];
                    l[2] = prev[2];

                    DadosTerm.Add(l);
                }
            }
        }

        private void CarregarOperTerm(System.IO.StreamReader sr) {

            var m1 = @"ESTAGIO\s+(\d+)\s+/\s+CENARIO\s+(\d+)";

            var estagio = 0;
            var ssis = 0;


            for (var line = sr.ReadLine(); !sr.EndOfStream; line = sr.ReadLine()) {

                if (Regex.IsMatch(line, m1, RegexOptions.IgnoreCase)) {
                    var mm1 = Regex.Match(line, m1, RegexOptions.IgnoreCase);
                    estagio = int.Parse(mm1.Groups[1].Value);
                    continue;
                }
                var newLine = this.OperTerm.CreateLine(line);
                newLine[0] = newLine[0].Trim();
                newLine[1] = newLine[1].Trim();

                if (newLine[4] is double && newLine[5] is double && newLine[6] is double) {
                    newLine[2] = estagio;

                    if (!string.IsNullOrWhiteSpace(newLine[0])) this.OperTerm.Add(newLine);
                    else if (line.Replace(" ", "").Contains("TotalTermica"))
                        ssis++;

                    if (ssis == 4) break;



                }
            }
        }

        private void CarregarRestricoesEletricas(System.IO.StreamReader sr, int estagio, int patamar) {


            BaseLine lPrevious = null;
            for (var line = sr.ReadLine(); !sr.EndOfStream && !string.IsNullOrWhiteSpace(line); line = sr.ReadLine()) {

                var l = this.RestricaoEletrica.CreateLine(line);
                l[0] = estagio;
                l[1] = patamar;
                l[7] = (l[7] ?? "").Trim();


                if (l[2] is int) {

                    var limites = ((string)l[7]).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    l[8] = l.Campos[8].TryParse(limites.First());
                    l[9] = l.Campos[9].TryParse(limites.Last());


                    //lPrevious[7],lPrevious[8] // inf, sup




                    if (lPrevious != null && lPrevious[0] == l[0]) {
                        lPrevious[3] += " + " + l[3];
                        lPrevious[4] = null;
                        lPrevious[5] = null;

                    } else {
                        lPrevious = l;
                    }
                }
                if (!(l[2] is int) && lPrevious != null) {
                    lPrevious[10] = l[7];
                    lPrevious[6] = l[6];

                    this.RestricaoEletrica.Add(lPrevious);

                    lPrevious = null;
                }
            }
        }

        private void CarregarBalancoEne(System.IO.StreamReader sr) {


            var m1 = @"ESTAGIO\s+(\d+)\s+/\s+CENARIO\s+(\d+)";
            var m2 = @"\bSubsistema\s+(\w{1,2})\b";

            var m3 = @"EAR_ini[^\d]+(\d{3,})[^\d]+(\d{3,})[^\d]+(\d{3,})"; //***** criar pelo bloco



            var estagio = 0;
            var subsistema = "";
            var patamar = "";

            for (var line = sr.ReadLine(); !sr.EndOfStream && !Regex.IsMatch(line, @"RELATORIO\s+DA\s+OPERACAO", RegexOptions.IgnoreCase); line = sr.ReadLine()) {

                if (line.Contains("----------------")) continue;

                if (Regex.IsMatch(line, m1, RegexOptions.IgnoreCase)) {
                    var mm1 = Regex.Match(line, m1, RegexOptions.IgnoreCase);
                    estagio = int.Parse(mm1.Groups[1].Value);
                    continue;
                }

                if (Regex.IsMatch(line, m2, RegexOptions.IgnoreCase)) {
                    var mm2 = Regex.Match(line, m2, RegexOptions.IgnoreCase);
                    subsistema = mm2.Groups[1].Value;
                    continue;
                } else if (subsistema != "") {

                    if (string.IsNullOrWhiteSpace(line)) {
                        subsistema = "";
                        patamar = "";
                        continue;
                    }


                    if (Regex.IsMatch(line, m3, RegexOptions.IgnoreCase)) {

                        var l = EnergiaSistema.CreateLine(line);
                        l[0] = subsistema;
                        l[1] = estagio;
                        EnergiaSistema.Add(l);
                        continue;
                    }

                    var nLine = BalancoEnergetico.CreateLine(line);

                    if (nLine[3] is double && nLine[6] is double && nLine[7] is double && nLine[12] is double) {
                        if (!string.IsNullOrWhiteSpace(nLine[2])) {
                            patamar = nLine[2];
                        }
                        nLine[0] = subsistema;
                        nLine[1] = estagio;

                        BalancoEnergetico.Add(nLine);
                    } else if (!string.IsNullOrWhiteSpace(subsistema) && !string.IsNullOrWhiteSpace(patamar) && estagio != 0) {

                        var lInt = Intercambios.CreateLine(line);
                        lInt[0] = subsistema;
                        lInt[1] = estagio;
                        lInt[2] = patamar;
                        lInt[3] = lInt[3].Trim();
                        Intercambios.Add(lInt);
                    }



                }
            }
        }

        private void CarregarENAAcopl(System.IO.StreamReader sr) {

            var m2 = @"\SUBSISTEMA:\s+(\d)";
            var regSis = new Regex(m2);
            var regVal = new Regex(@"(\d+\.\d+)");

            var subsistema = 0;

            for (var line = sr.ReadLine(); !sr.EndOfStream; line = sr.ReadLine()) {


                if (subsistema == 0) {

                    var mSis = regSis.Match(line);

                    if (mSis.Success) subsistema = int.Parse(mSis.Groups[1].Value);
                    else continue;
                }

                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                line = sr.ReadLine();

                var vals = regVal.Matches(line);

                for (int i = 0; i < vals.Count - 1; i++) {

                    var l = EnergiaAcoplamentoSistema.CreateLine(
                        subsistema.ToString() + " " + (i + 1).ToString() + " " + vals[i].Value
                        );

                    EnergiaAcoplamentoSistema.Add(l);
                }

                if (subsistema == 4) break;
                subsistema = 0;
            }

        }

        private void CarregarENATh(System.IO.StreamReader sr) {
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();


            for (var line = sr.ReadLine(); !sr.EndOfStream; line = sr.ReadLine()) {

                if (line.Contains("X----")) {
                    break;
                } else {
                    var newLine = THMensal.CreateLine(line);
                    THMensal.Add(newLine);
                }
            }


        }

        private void CarregaVolUtil(System.IO.StreamReader sr) {

            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            var read = false;
            for (var line = sr.ReadLine(); !sr.EndOfStream; line = sr.ReadLine()) {
                if (line.Contains("X----X------------X-------X")) {
                    if (read) break;
                    read = true;
                } else if (string.IsNullOrWhiteSpace(line)) {
                    read = false;
                } else if (read) {

                    var newLine = VolUtil.CreateLine(line);
                    VolUtil.Add(newLine);
                }
            }

        }

        private int CarregaOps(System.IO.StreamReader sr) {

            sr.ReadLine();

            int estagio = 1;

            var line = sr.ReadLine();
            var m1 = @"ESTAGIO\s+(\d+)\s+/\s+CENARIO\s+(\d+)";
            if (Regex.IsMatch(line, m1, RegexOptions.IgnoreCase)) {
                estagio = int.Parse(Regex.Match(line, m1, RegexOptions.IgnoreCase).Groups[1].Value);
            }


            sr.ReadLine();

            bool read = false;
            bool readNewDecomp = false;

            for (line = sr.ReadLine(); !sr.EndOfStream; line = sr.ReadLine()) {

                if (line.Contains("   X-----------------X-----X-----X-----X----------------X--------X--------X-------X-------X-------X-------X-------X-------X-------X-------X")) {
                    if (read) break;
                    read = true;
                    continue;
                } if (line.Contains("   X----X-----------------X-----X-----X-----X----------------X--------X--------X-------X-------X-------X-------X-------X-------X-------X-------X")) {
                    if (readNewDecomp) break;
                    readNewDecomp = true;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line)) {
                    read = false;
                } else if (read) {

                    var newLine = Operacao.CreateLine(line);
                    Operacao.Add(newLine);
                } else if (readNewDecomp) {
                    var newLine = new RelatoOpsNewLine();
                    newLine.Load(line);
                    Operacao.Add(newLine);
                }
            }

            return estagio;
        }


        public override void Load(string fileContent) {
            //((RelatoOpsBlock)Blocos["Operacao"]).Load(fileContent);
            //((RelatoVolUtilBlock)Blocos["Vol Util"]).Load(fileContent);
            ((RelatoCmoBlock)Blocos["CMO"]).Load(fileContent);
            //((RelatoPotenciaDispBlock)Blocos["Potencia Disponivel"]).Load(fileContent);
            //((RelatoTHMenBlock)Blocos["Tendencia Hidro Meses"]).Load(fileContent);

        }
    }


    public class RelatEnaAcoplBlock : BaseBlock<RelatEnaAcoplLine> {

    }


    public class RelatEnaAcoplLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(1  , 1  ,"A2"    , "Subsistema"),
                new BaseField(3  , 3  ,"I3"    , "ESTAGIO"),                
                new BaseField(5 , 20 ,"F7.2"  , "ENA"),                
        };


        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}

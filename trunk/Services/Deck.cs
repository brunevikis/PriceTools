using Compass.CommomLibrary;
using Compass.ExcelTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Compass.Services
{
    public class Deck
    {

        private static ColetaGtminAgente OpenFileGtAgente(string filePath)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = null;

            try
            {
                xlApp = Helper.StartExcel();

                var wb = xlApp.Workbooks.Open(filePath, ReadOnly: true);
                var ws = wb.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;

                string planilhaPadrao = ws.Cells[2, 2].Value2;

                if (planilhaPadrao == null || planilhaPadrao == "" || planilhaPadrao != "GTMIN AGENTE")
                    throw new ArgumentNullException("Planilha padrão foi modificada de alguma forma, avise o desenvvolvedor responsável");


                var cells = ws.Range[ws.Cells[1, 1], ws.Cells[30, 14]].Value2;

                wb.Close(SaveChanges: false);
                xlApp.Quit();

                string gtminAgente = @"{ 'Usinas' : [
                { 'Nome':'" + cells[4, 2] + @"', 'Anos': 
                    [ { 
                        'ano' : '" + cells[5, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[4, i] + @"', 'resultado' : '" + cells[5, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[6, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[4, i] + @"', 'resultado' : '" + cells[6, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[7, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[4, i] + @"', 'resultado' : '" + cells[7, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[8, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[4, i] + @"', 'resultado' : '" + cells[8, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[9, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[4, i] + @"', 'resultado' : '" + cells[9, i] + @"' }"
                           ))
                        + @"]}
                    ]},
                { 'Nome':'" + cells[11, 2] + @"', 'Anos': 
                    [ { 
                        'ano' : '" + cells[12, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[11, i] + @"', 'resultado' : '" + cells[12, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[13, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[11, i] + @"', 'resultado' : '" + cells[13, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[14, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[11, i] + @"', 'resultado' : '" + cells[14, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[15, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[11, i] + @"', 'resultado' : '" + cells[15, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[16, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[11, i] + @"', 'resultado' : '" + cells[16, i] + @"' }"
                           ))
                        + @"]}
                    ]},
                { 'Nome':'" + cells[18, 2] + @"', 'Anos': 
                    [ { 
                        'ano' : '" + cells[19, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[18, i] + @"', 'resultado' : '" + cells[19, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[20, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[18, i] + @"', 'resultado' : '" + cells[20, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[21, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[18, i] + @"', 'resultado' : '" + cells[21, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[22, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[18, i] + @"', 'resultado' : '" + cells[22, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[23, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[18, i] + @"', 'resultado' : '" + cells[23, i] + @"' }"
                           ))
                        + @"]}
                    ]},
                { 'Nome':'" + cells[25, 2] + @"', 'Anos': 
                    [ { 
                        'ano' : '" + cells[26, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[25, i] + @"', 'resultado' : '" + cells[26, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[27, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[25, i] + @"', 'resultado' : '" + cells[27, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[28, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[25, i] + @"', 'resultado' : '" + cells[28, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[29, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[25, i] + @"', 'resultado' : '" + cells[29, i] + @"' }"
                           ))
                        + @"]},
                        {'ano' : '" + cells[30, 2] + @"' , 'Meses' : 
                        [" +
                            String.Join(",",
                           Enumerable.Range(3, 12).Select(i =>
                           @"{ 'mes' : '" + cells[25, i] + @"', 'resultado' : '" + cells[30, i] + @"' }"
                           ))
                        + @"]}
                    ]}
                ]}";


                ColetaGtminAgente resultado = new JavaScriptSerializer().Deserialize<ColetaGtminAgente>(gtminAgente);

                if (resultado != null)
                    return resultado;
                else
                    throw new Exception("Erro no arquivo do GtminAgente.xlsx");

                /*foreach (var item in resultado.Usinas)
                {
                    foreach (var anos in item.Anos)
                    {
                        foreach (var mes in anos.Meses)
                        {
                            MessageBox.Show("mes: " + mes.mes + ", resultado: " + mes.resultado);
                        }
                    }
                }*/


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new ColetaGtminAgente();
            }
        }

        /// <summary>
        /// inflexibilidade igual ao mínimo entre informado em CADTERM e EXPT/TERM
        /// </summary>
        /// <param name="cceeDeck"></param>
        public static void Ons2Ccee(Compass.CommomLibrary.Newave.Deck cceeDeck, Compass.CommomLibrary.Newave.Deck deckCCEEAnterior)
        {
            ColetaGtminAgente gtminAgente = null;

            if (deckCCEEAnterior[CommomLibrary.Newave.Deck.DeckDocument.cadterm] == null)
            {
                throw new Exception("Não existe arquivo CADTERM no deck para realizar a conversão. Copie o arquivo e tente novamente.");
            }

            var cadTerm = deckCCEEAnterior[CommomLibrary.Newave.Deck.DeckDocument.cadterm].Document as Compass.CommomLibrary.CadTermDat.CadTermDat;

            var confT = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.conft].Document as Compass.CommomLibrary.ConftDat.ConftDat;
            var expt = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.expt].Document as Compass.CommomLibrary.ExptDat.ExptDat;
            var term = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.term].Document as Compass.CommomLibrary.TermDat.TermDat;

            foreach (var ute in confT)
            {

                if (!cadTerm.Any(x => x.Num == ute.Num)) continue;

                var gtminCadTerm = cadTerm.First(x => x.Num == ute.Num).Gtmin;

                //if (ute.Existente == "EX") { //caso existente, modificar term.dat (campos 6=jan...17=dez, 18=demais anos)
                for (int c = 6; c <= 18; c++)
                {
                    if (gtminCadTerm < term.First(x => x.Cod == ute.Num)[c])
                        term.First(x => x.Cod == ute.Num)[c] = gtminCadTerm;
                }
                //} else { // se não existente, alterar expt "GTMIN"
                foreach (var exptGtmin in expt.Where(x => x.Cod == ute.Num && x.Tipo == "GTMIN"))
                {
                    if (gtminCadTerm < exptGtmin.Valor)
                    {
                        exptGtmin.Valor = gtminCadTerm;
                    }
                }
                //}
            }

            var dgerData = cceeDeck.Dger.DataEstudo;

            if (File.Exists(System.IO.Path.Combine(deckCCEEAnterior.BaseFolder, "GtminAgenteCDE.xlsx")))
            {
                gtminAgente = OpenFileGtAgente(System.IO.Path.Combine(deckCCEEAnterior.BaseFolder, "GtminAgenteCDE.xlsx"));


                foreach (var ute in gtminAgente.Usinas)
                {
                    if (!cadTerm.Any(x => x.Num == ute.Num)) continue;

                    var toremove = expt.Where(x => x.Cod == ute.Num && x.Tipo == "GTMIN").ToList();
                    var idx = expt.IndexOf(toremove.First());
                    toremove.ForEach(x => expt.Remove(x));

                    foreach (var ano in ute.Anos)
                    {
                        foreach (var mes in ano.Meses)
                        {

                            var data = new DateTime(int.Parse(ano.ano), ano.Meses.IndexOf(mes) + 1, 1);

                            if (data >= dgerData)
                            {

                                var valornovo = double.Parse(mes.resultado);
                                var valorantigo = toremove.Where(x => x.DataInicio <= data && x.DataFim >= data)
                                    .FirstOrDefault()?.Valor ?? 0;

                                if (valornovo > valorantigo) valornovo = valorantigo;

                                expt.Insert(idx++,

                                    new CommomLibrary.ExptDat.ExptLine()
                                    {
                                        Cod = ute.Num,
                                        Tipo = "GTMIN",
                                        Valor = valornovo,
                                        DataInicio = data,
                                        DataFim = data,
                                    }
                                    );
                            }
                        }
                    }
                }
            }

            term.SaveToFile(createBackup: true);
            expt.SaveToFile(createBackup: true);

        }

        /*

        ESTRUTURAIS
        +	FU  122   1   178           
        +	FU  123   1   175
        +	FU  124   1   169
        +	FU  125   1   172 

        CONJUNTURAIS
        -	FU  198   1   139
        -	FT  300   1   501  2             1
        FT  300   1   502  2             1

        */

        public static void Ons2Ccee(Compass.CommomLibrary.Decomp.Deck cceeDeck)
        {

            //"RESTRIÇÕES DE INTERCÂMBIO CONJUNTURAIS";
            var dadgerBase = ((Compass.CommomLibrary.Decomp.Deck)cceeDeck)[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
            var resDeckBase = dadgerBase.BlocoRhe.RheGrouped;


            var blocoConj = false;
            foreach (var key in resDeckBase.Keys)
            {
                if (key.Comment.ToUpperInvariant().Contains("BIO CONJUNTURAIS")) blocoConj = true;


                if (!blocoConj)
                {

                    var fs = resDeckBase[key].Where(y => (y is Compass.CommomLibrary.Dadger.FuLine)
                        || (y is Compass.CommomLibrary.Dadger.FiLine)
                        || (y is Compass.CommomLibrary.Dadger.FtLine));

                    var ok = fs.Count() == 1;
                    if (ok)
                    {

                        var uuu = new int[] { 178, 175, 169, 172 };



                        ok &= fs.Any(z => (z is Compass.CommomLibrary.Dadger.FuLine)
                            && uuu.Contains((int)z[3])
                            );
                    }

                    if (ok)
                    {
                        resDeckBase[key].ForEach(x => x[0] = "&" + x[0]);
                    }


                }
                else
                {


                    var fs = resDeckBase[key].Where(y => (y is Compass.CommomLibrary.Dadger.FuLine)
                        || (y is Compass.CommomLibrary.Dadger.FiLine)
                        || (y is Compass.CommomLibrary.Dadger.FtLine));

                    var ok = false;

                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FtLine && x[3] > 320); // intercambio internacional anterior
                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FeLine); // intercambio internacional a partir de julho/18
                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FuLine && x[3] == 139);

                    if (!ok)
                    {
                        resDeckBase[key].ForEach(x => x[0] = "&" + x[0]);
                    }
                }
            }

            dadgerBase.SaveToFile(createBackup: true);




        }

        public static void DesfazerInviabilidades(Compass.CommomLibrary.Decomp.Deck deck, CommomLibrary.Inviab.Inviab inviabilidades)
        {
            var dadger = deck[CommomLibrary.Decomp.DeckDocument.dadger].Document as CommomLibrary.Dadger.Dadger;
            var hidr = deck[CommomLibrary.Decomp.DeckDocument.hidr].Document as CommomLibrary.HidrDat.HidrDat;

            var q =
                        from inv in inviabilidades.Iteracao
                        group inv by new { inv.Estagio, inv.RestricaoViolada } into invG
                        select invG.OrderByDescending(x => x.Violacao).First();

            foreach (var inviab in q.OrderByDescending(x => x.Estagio))
            {
                if (inviab.TipoRestricao == "RHE" || inviab.TipoRestricao == "RHQ" || inviab.TipoRestricao == "RHV")
                {
                    IEnumerable<BaseLine> rs;
                    if (inviab.TipoRestricao == "RHE")
                        rs = dadger.BlocoRhe.Where(x => x.Restricao == inviab.CodRestricao);
                    else if (inviab.TipoRestricao == "RHQ")
                        rs = dadger.BlocoRhq.Where(x => x.Restricao == inviab.CodRestricao);
                    else if (inviab.TipoRestricao == "RHV")
                        rs = dadger.BlocoRhv.Where(x => x.Restricao == inviab.CodRestricao);
                    else
                        continue;

                    if (rs.Count() > 0)
                    {
                        dynamic le;
                        if (inviab.TipoRestricao == "RHE")
                        {
                            var ls = rs.Where(x => x is CommomLibrary.Dadger.LuLine).Select(x => (CommomLibrary.Dadger.LuLine)x);
                            le = ls.Where(x => x.Estagio <= inviab.Estagio).OrderByDescending(x => x.Estagio).FirstOrDefault();
                        }
                        else if (inviab.TipoRestricao == "RHQ")
                        {
                            //excecoes
                            if (rs.Where(x => x is CommomLibrary.Dadger.CqLine).Select(x => x as CommomLibrary.Dadger.CqLine)
                                .All(x =>
                                   x.Usina == 251 // SERRA DA MESA
                                || x.Usina == 156 // TRES MARIAS
                                || x.Usina == 169 // SOBRADINHO
                                || x.Usina == 178 // XINGO
                                ))
                                continue;

                            var ls = rs.Where(x => x is CommomLibrary.Dadger.LqLine).Select(x => (CommomLibrary.Dadger.LqLine)x);
                            le = ls.Where(x => x.Estagio <= inviab.Estagio).OrderByDescending(x => x.Estagio).FirstOrDefault();
                        }
                        else if (inviab.TipoRestricao == "RHV")
                        {
                            var ls = rs.Where(x => x is CommomLibrary.Dadger.LvLine).Select(x => (CommomLibrary.Dadger.LvLine)x);
                            le = ls.Where(x => x.Estagio <= inviab.Estagio).OrderByDescending(x => x.Estagio).FirstOrDefault();
                        }
                        else continue;

                        if (le.Estagio < inviab.Estagio)
                        {

                            var nle = le.Clone();
                            nle.Estagio = inviab.Estagio;
                            if (inviab.TipoRestricao == "RHE") dadger.BlocoRhe.Add(nle);
                            else if (inviab.TipoRestricao == "RHQ") dadger.BlocoRhq.Add(nle);
                            else if (inviab.TipoRestricao == "RHV") dadger.BlocoRhv.Add(nle);
                            le = nle;
                        }

                        var i = 2 * (inviab.Patamar ?? 1) + (inviab.SupInf == "INF" ? 1 : 2);

                        double valorInviab;
                        if (inviab.TipoRestricao == "RHV")
                        {
                            valorInviab = Math.Ceiling(inviab.Violacao * 100d) / 100d;
                        }
                        else
                        {
                            valorInviab = Math.Ceiling(inviab.Violacao);
                        }

                        le[i] =
                            inviab.SupInf == "INF"
                            ? le[i] - valorInviab
                            : le[i] + valorInviab;

                        if (le[i] < 0) le[i] = 0;
                    }
                }
                else if (inviab.TipoRestricao == "EVAP")
                {
                    var usina = hidr[inviab.Usina];
                    if (usina != null)
                    {
                        dadger.BlocoUh.Where(x => x.Usina == usina.Cod).First().Evaporacao = false;
                    }
                }
                else if (inviab.TipoRestricao == "IRRI")
                {
                    var usina = hidr[inviab.Usina];
                    if (usina != null)
                    {
                        //dadger.BlocoUh.Where(x => x.Usina == usina.Cod).First().Evaporacao = false;

                        var ti = dadger.BlocoTi.Where(x => x.Usina == usina.Cod).First();

                        ti[inviab.Estagio + 1] -= Math.Ceiling(inviab.Violacao);
                        if (ti[inviab.Estagio + 1] < 0) ti[inviab.Estagio + 1] = 0;
                    }
                }

            }
        }

        public static void AlterarCortes(Compass.CommomLibrary.Dadger.Dadger dadger, string cortesPath)
        {

            var refPath = dadger.File.Split('\\').ToList();
            var cortes = cortesPath.Split('\\').ToList();

            for (int i = 0; i < Math.Min(cortes.Count, refPath.Count); i++)
            {
                if (cortes[i] == refPath[i])
                {
                    cortes.RemoveAt(i);
                    refPath.RemoveAt(i);
                    i--;
                }
            }

            var cortesRelPath = "";
            for (int i = 0; i < refPath.Count - 1; i++)
            {
                cortesRelPath += "../";
            }
            for (int i = 0; i < cortes.Count - 1; i++)
            {
                cortesRelPath += cortes[i] + "/";
            }

            var x1 = cortesRelPath + cortes.Last();
            var x2 = cortesRelPath + "cortesh" + System.IO.Path.GetExtension(x1);

            var fc = (Compass.CommomLibrary.Dadger.FcBlock)dadger.Blocos["FC"];

            fc.CortesInfo.Arquivo = x2;
            fc.Cortes.Arquivo = x1;

        }


        public static void CreateDgerNewdesp(string dir)
        {
            var files = Directory.GetFiles(dir).ToList();

            //decomp/newave/newdesp?


            var pmoFile = files.FirstOrDefault(x => Path.GetFileName(x).Equals("pmo.dat", StringComparison.OrdinalIgnoreCase));
            var dgerFile = files.FirstOrDefault(x => Path.GetFileName(x).Equals("dger.dat", StringComparison.OrdinalIgnoreCase));
            var adtermFile = files.FirstOrDefault(x => Path.GetFileName(x).Equals("adterm.dat", StringComparison.OrdinalIgnoreCase));
            var dgernwdFile = files.FirstOrDefault(x => Path.GetFileName(x).Equals("dger.nwd", StringComparison.OrdinalIgnoreCase));




            if (pmoFile != null && adtermFile != null)
            {
                var pmo = (Compass.CommomLibrary.Pmo.Pmo)DocumentFactory.Create(pmoFile);
                var dger = (Compass.CommomLibrary.DgerDat.DgerDat)DocumentFactory.Create(dgerFile);

                if (dgernwdFile != null)
                {

                    File.Copy(dgernwdFile, dgernwdFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                }


                var dgernwd = new Compass.CommomLibrary.DgerNwd.DgerNwd();
                dgernwd.File = Path.Combine(dir, "dger.nwd");
                dgernwd.Definicoes.Periodos = 1;
                dgernwd.Definicoes.MesInicial = dger.MesEstudo;
                dgernwd.Definicoes.AnoInicial = dger.AnoEstudo;
                dgernwd.Definicoes.TipoSimulacao = 1;


                int i = 0;
                foreach (var earmi in pmo.EarmI)
                {
                    dgernwd.EarmI[i++] = earmi.Earm;
                }

                var teaf = new List<Tuple<int, int, double>>();
                int reeOrd = 0;
                foreach (var eafR in pmo.EafPast)
                {

                    for (int mes = 1; mes <= 12; mes++)
                    {
                        teaf.Add(new Tuple<int, int, double>(reeOrd, mes, eafR[mes]));
                    }
                    reeOrd++;
                }

                for (int mesOff = -1; mesOff >= -11; mesOff--)
                {

                    var lp = new Compass.CommomLibrary.DgerNwd.EafLine();
                    var mes = dgernwd.Definicoes.MesInicial + mesOff;
                    if (mes <= 0) mes += 12;

                    lp[0] = mes;

                    foreach (var e in teaf.Where(x => x.Item2 == mes).OrderBy(x => x.Item1))
                    {
                        lp[e.Item1 + 1] = e.Item3;
                    }

                    dgernwd.EnaPast.Add(lp);
                }
                {
                    var lp = new Compass.CommomLibrary.DgerNwd.EafLine();
                    var mes = dgernwd.Definicoes.MesInicial;

                    lp[0] = mes;

                    foreach (var e in teaf.Where(x => x.Item2 == mes).OrderBy(x => x.Item1))
                    {
                        lp[e.Item1 + 1] = e.Item3;
                    }

                    dgernwd.EnaPrev.Add(lp);
                }

                var adterm = File.ReadAllText(adtermFile).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                foreach (var tL in adterm.Skip(2))
                {
                    if (tL.Trim() == "9999") break;

                    dgernwd.Gnl.Add(
                        dgernwd.Gnl.CreateLine(tL)
                        );
                }
                dgernwd.SaveToFile();
            }
            else
                throw new Exception("PMO.DAT ou ADTERM.DAT não encontrados!");
        }

        public static void VerificarRestricaoEletrica(CommomLibrary.ReDat.ReDat deckCCEEAnterior, CommomLibrary.ReDat.ReDat deckONSAnterior, CommomLibrary.ReDat.ReDat deckONS)
        {
            CommomLibrary.ReDat.ReDat baseCCEE = deckCCEEAnterior;
            CommomLibrary.ReDat.ReDat baseONS = deckONSAnterior;
            CommomLibrary.ReDat.ReDat oNS = deckONS;

            var listaRemover = baseONS.Restricoes.Where(restONS => !baseCCEE.Any(i => i.Chave == restONS.Chave)).ToList();

            foreach (var remover in listaRemover)
            {
                var r = oNS.FirstOrDefault(x => x.Chave == remover.Chave);

                if (r == null) continue;

                oNS.Remove(r);
                foreach (var rd in oNS.Detalhes.Where(x => x.Numero == r.Numero).ToList())
                {
                    oNS.Detalhes.Remove(rd);
                }
            }

            oNS.SaveToFile(createBackup: true);

            var listaAvisar = oNS.Restricoes.Where(ons => !baseONS.Restricoes.Any(x => x.Chave == ons.Chave)).ToList();

            if (listaAvisar.Count != 0)
            {
                string avisar = "";

                foreach (var lista in listaAvisar)
                {
                    avisar += (lista == listaAvisar.FirstOrDefault() ? "" : ", ") + lista.Chave;
                }

                AutoClosingMessageBox.Show((listaAvisar.Count == 1 ? ("A restrição " + avisar + " foi acrescentada no novo deck!") : ("As restrições " + avisar + " foram acrescentadas no novo deck!")), "Caption", 3000);

            }
            else
                AutoClosingMessageBox.Show("Não existem novas restrições", "Caption", 3000);
        }
    }

    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            using (_timeoutTimer)
                MessageBox.Show(text, caption);
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }

    public class ColetaGtminAgente
    {
        public List<Usina> Usinas { get; set; }

    }

    public class Usina
    {
        public string Nome { get; set; }
        public List<Ano> Anos { get; set; }

        public int Num
        {
            get
            {
                switch (Nome.ToUpperInvariant().Trim())
                {
                    case "UTE J. LACERDA A1": return 26;
                    case "UTE J. LACERDA A2": return 27;
                    case "UTE J. LACERDA B": return 25;
                    case "UTE J. LACERDA C": return 24;
                    default:
                        return 0;
                }
            }
        }
    }

    public class Ano
    {
        public string ano { get; set; }
        public List<Mes> Meses { get; set; }
    }

    public class Mes
    {
        public string mes { get; set; }
        public string resultado { get; set; }
    }
}

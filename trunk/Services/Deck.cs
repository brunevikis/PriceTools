using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using Compass.ExcelTools;
using Microsoft.Office.Interop.Excel;
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
        private static List<Tuple<int, DateTime, double>> OpenFileGtCCEE(string filePath)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = null;

            try
            {
                xlApp = Helper.StartExcel();

                var wb = xlApp.Workbooks.Open(filePath, ReadOnly: true);
                var ws = wb.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;

                //List<int> utes = new List<int> { 24, 25, 26, 27 };
                List<Tuple<int, DateTime, double>> dados = new List<Tuple<int, DateTime, double>>();
                // foreach (var ut in utes)
                // {
                for (int l = 3; !string.IsNullOrWhiteSpace(ws.Cells[l, 1].Text); l++)//!string.IsNullOrWhiteSpace(ws.Cells[l, 1].Text)
                {
                    int ute = Convert.ToInt32(ws.Range["A" + l.ToString()].Value);
                    double gtmin = Convert.ToDouble(ws.Range["D" + l.ToString()].Value2);
                    // if (gtmin > 0)
                    //{
                    dados.Add(new Tuple<int, DateTime, double>(ute, ws.Range["C" + l.ToString()].Value, Convert.ToDouble(ws.Range["D" + l.ToString()].Value2)));
                    // }
                }
                //}

                wb.Close(SaveChanges: false);
                xlApp.Quit();

                if (dados.Count() > 0)
                    return dados;
                else
                    throw new Exception("Erro no arquivo do Gtmin_CCEE.xlsx");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new List<Tuple<int, DateTime, double>>();
            }
        }
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
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            ColetaGtminAgente gtminAgente = null;

            //if (deckCCEEAnterior[CommomLibrary.Newave.Deck.DeckDocument.cadterm] == null)
            //{
            //    throw new Exception("Não existe arquivo CADTERM no deck para realizar a conversão. Copie o arquivo e tente novamente.");
            //}
            CorrigeArquivosdat(cceeDeck.BaseFolder);
            var GTMIN_CCEEFile = Directory.GetFiles(deckCCEEAnterior.BaseFolder).Where(x => Path.GetFileName(x).StartsWith("GTMIN_CCEE_", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var GTMIN_CCEEFileAtual = Directory.GetFiles(cceeDeck.BaseFolder).Where(x => Path.GetFileName(x).StartsWith("GTMIN_CCEE_", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var cadTermFile = Directory.GetFiles(deckCCEEAnterior.BaseFolder).Where(x => Path.GetFileName(x).StartsWith("CADTERM.DAT", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            bool temCadterm = false;
            var confT = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.conft].Document as Compass.CommomLibrary.ConftDat.ConftDat;
            var expt = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.expt].Document as Compass.CommomLibrary.ExptDat.ExptDat;
            var term = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.term].Document as Compass.CommomLibrary.TermDat.TermDat;
            var dger = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.dger].Document as Compass.CommomLibrary.DgerDat.DgerDat;

            if (cadTermFile != null && File.Exists(cadTermFile) && !File.Exists(GTMIN_CCEEFile) && GTMIN_CCEEFile == null)
            {
                var cadTerm = deckCCEEAnterior[CommomLibrary.Newave.Deck.DeckDocument.cadterm].Document as Compass.CommomLibrary.CadTermDat.CadTermDat;
                temCadterm = true;
                foreach (var ute in confT)
                {

                    if (!cadTerm.Any(x => x.Num == ute.Num))
                    {
                        continue;

                    }

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
            }

            var dgerData = cceeDeck.Dger.DataEstudo;
            var exptsdup = expt.Where(x => x.Tipo == "GTMIN").ToList();
            //separar dados de gtmin com range de meses contendo os dois primeiros meses 
            foreach (var exptGtmin in exptsdup)
            {
                DateTime segundoMes = dgerData.AddMonths(1);

                if (exptGtmin.DataInicio <= segundoMes)
                {
                    if (exptGtmin.DataFim <= segundoMes)
                    {
                        continue;
                    }
                    else
                    {
                        var idx = expt.IndexOf(exptGtmin) + 1;

                        DateTime dataIni = segundoMes.AddMonths(1);
                        DateTime dataFim = exptGtmin.DataFim;

                        expt.Insert(idx,

                                    new CommomLibrary.ExptDat.ExptLine()
                                    {
                                        Cod = exptGtmin.Cod,
                                        Tipo = "GTMIN",
                                        Valor = exptGtmin.Valor,
                                        DataInicio = dataIni,
                                        DataFim = dataFim,
                                    }
                                    );
                        exptGtmin.DataFim = segundoMes;
                    }
                }

            }
            expt.SaveToFile();
            expt = cceeDeck[CommomLibrary.Newave.Deck.DeckDocument.expt].Document as Compass.CommomLibrary.ExptDat.ExptDat;

            if (GTMIN_CCEEFile != null && File.Exists(GTMIN_CCEEFile) && temCadterm == false)
            {
                if (GTMIN_CCEEFileAtual != null && File.Exists(GTMIN_CCEEFileAtual))
                {
                    GTMIN_CCEEFile = GTMIN_CCEEFileAtual;

                }


                var GtminCCEE = OpenFileGtCCEE(GTMIN_CCEEFile);
                var utes = GtminCCEE.Select(x => x.Item1).Distinct();

                foreach (var ute in confT)
                {
                    //if (ute.Num == 238)
                    //{

                    //}
                    if (GtminCCEE.Any(x => x.Item1 == ute.Num) && term.Any(y => y.Cod == ute.Num))
                    {

                        //DateTime menorData = GtminCCEE.Select(x => x.Item2).Min();
                        DateTime menorData = GtminCCEE.Where(x => x.Item1 == ute.Num).Select(x => x.Item2).Min();

                        if (GtminCCEE.First(x => x.Item1 == ute.Num).Item2 == menorData)
                        {
                            var gtmin = GtminCCEE.First(x => x.Item1 == ute.Num).Item3;
                            //if (ute.Existente == "EX") { //caso existente, modificar term.dat (campos 6=jan...17=dez, 18=demais anos)
                            for (int c = 6; c <= 18; c++)
                            {
                                if (gtmin < term.First(x => x.Cod == ute.Num)[c])
                                    term.First(x => x.Cod == ute.Num)[c] = gtmin;
                            }
                            //} else { // se não existente, alterar expt "GTMIN"
                            foreach (var exptGtmin in expt.Where(x => x.Cod == ute.Num && x.Tipo == "GTMIN"))
                            {
                                if (menorData > dgerData.AddMonths(1))//alterção para prevenir de mudar os dois primeiros meses
                                {
                                    if (exptGtmin.DataInicio <= menorData && exptGtmin.DataFim >= menorData)
                                    {
                                        if (gtmin < exptGtmin.Valor)
                                        {
                                            exptGtmin.Valor = gtmin;
                                        }
                                    }
                                }
                            }
                            //}
                        }

                    }

                }

                foreach (var ute in utes)
                {
                    if (ute == 36 /*|| ute == 327 || ute == 328 || ute == 1 || ute == 238*/)
                    {

                    }
                    //if (!cadTerm.Any(x => x.Num == ute)) continue;
                    if (expt.Any(x => x.Cod == ute && x.Tipo == "GTMIN"))
                    {
                        var allGtmins = expt.Where(x => x.Cod == ute && x.Tipo == "GTMIN").ToList();
                        //var toremove = expt.Where(x => x.Cod == ute && x.Tipo == "GTMIN" && x.DataFim > dgerData.AddMonths(1)).ToList();
                        int idx = expt.IndexOf(allGtmins.Last()) + 1;

                        allGtmins.ForEach(x =>
                        {

                            if (x.DataFim > dgerData.AddMonths(1))
                            {
                                idx = expt.IndexOf(x);
                                expt.Remove(x);
                            }

                        });


                        //idx = expt.IndexOf(expt.Where(x => x.Cod == ute && x.Tipo == "GTMIN").FirstOrDefault());
                        //if (idx == 0 )
                        //{
                        //    idx = expt.IndexOf(expt.Where(x => x.Cod == ute).First())+1;
                        //}
                        //else
                        //{
                        //    idx = idx + 1;
                        //}

                        var uteDados = GtminCCEE.Where(x => x.Item1 == ute).ToList();
                        foreach (var uD in uteDados)
                        {

                            var data = new DateTime(uD.Item2.Year, uD.Item2.Month, 1);

                            //if (data >= dgerData)
                            if (data > dgerData.AddMonths(1))
                            {

                                var valornovo = uD.Item3;
                                var valorantigo = allGtmins.Where(x => x.DataInicio <= data && x.DataFim >= data)
                                    .FirstOrDefault()?.Valor ?? 0;

                                if (valornovo > valorantigo) valornovo = valorantigo;

                                expt.Insert(idx++,

                                    new CommomLibrary.ExptDat.ExptLine()
                                    {
                                        Cod = ute,
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
            else if (File.Exists(System.IO.Path.Combine(deckCCEEAnterior.BaseFolder, "GtminAgenteCDE.xlsx")))
            {
                gtminAgente = OpenFileGtAgente(System.IO.Path.Combine(deckCCEEAnterior.BaseFolder, "GtminAgenteCDE.xlsx"));
                var cadTerm = deckCCEEAnterior[CommomLibrary.Newave.Deck.DeckDocument.cadterm].Document as Compass.CommomLibrary.CadTermDat.CadTermDat;


                foreach (var ute in gtminAgente.Usinas)
                {
                    if (!cadTerm.Any(x => x.Num == ute.Num)) continue;

                    var allGtmins = expt.Where(x => x.Cod == ute.Num && x.Tipo == "GTMIN").ToList();
                    //var toremove = expt.Where(x => x.Cod == ute.Num && x.Tipo == "GTMIN" && x.DataFim > dgerData.AddMonths(1)).ToList();
                    int idx = expt.IndexOf(allGtmins.Last()) + 1;

                    allGtmins.ForEach(x =>
                    {

                        if (x.DataFim > dgerData.AddMonths(1))
                        {
                            idx = expt.IndexOf(x);
                            expt.Remove(x);
                        }

                    });

                    //idx = expt.IndexOf(expt.Where(x => x.Cod == ute.Num && x.Tipo == "GTMIN").FirstOrDefault());
                    //if (idx == 0)
                    //{
                    //    idx = expt.IndexOf(expt.Where(x => x.Cod == ute.Num).First()) + 1;
                    //}
                    //else
                    //{
                    //    idx = idx + 1;
                    //}

                    foreach (var ano in ute.Anos)
                    {
                        foreach (var mes in ano.Meses)
                        {

                            var data = new DateTime(int.Parse(ano.ano), ano.Meses.IndexOf(mes) + 1, 1);

                            //if (data >= dgerData)
                            if (data > dgerData.AddMonths(1))
                            {

                                var valornovo = double.Parse(mes.resultado);
                                var valorantigo = allGtmins.Where(x => x.DataInicio <= data && x.DataFim >= data)
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

            try
            {
                dger.SetaTendenciaHidrologia = 2;
                dger.SaveToFile();
            }
            catch (Exception ex)
            {

                ex.ToString();
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

        public static void CorrigeArquivosdat(string dir)
        {
            List<string> linhasReferencia = new List<string>
            {
                "DADOS GERAIS                : dger.dat",
                "DADOS DOS SUBSISTEMAS       : sistema.dat",
                "CONFIGURACAO HIDRAULICA     : confhd.dat",
                "ALTERACAO DADOS USINAS HIDRO: modif.dat",
                "CONFIGURACAO TERMICA        : conft.dat",
                "DADOS DAS USINAS TERMICAS   : term.dat",
                "DADOS DAS CLASSES TERMICAS  : clast.dat",
                "DADOS DE EXPANSAO HIDRAULICA: exph.dat",
                "ARQUIVO DE EXPANSAO TERMICA : expt.dat",
                "ARQUIVO DE PATAMARES MERCADO: patamar.dat",
                "ARQUIVO DE CORTES DE BENDERS: cortes.dat",
                "ARQUIVO DE CABECALHO CORTES : cortesh.dat",
                "RELATORIO DE CONVERGENCIA   : pmo.dat",
                "RELATORIO DE E. SINTETICAS  : parp.dat",
                "RELATORIO DETALHADO FORWARD : forward.dat",
                "ARQUIVO DE CABECALHO FORWARD: forwarh.dat",
                "ARQUIVO DE S.HISTORICAS S.F.: shist.dat",
                "ARQUIVO DE MANUT.PROG. UTE'S: manutt.dat",
                "ARQUIVO P/DESPACHO HIDROTERM: newdesp.dat",
                "ARQUIVO C/TEND. HIDROLOGICA : vazpast.dat",
                "ARQUIVO C/DADOS DE ITAIPU   : itaipu.dat",
                "ARQUIVO C/DEMAND S. BIDDING : bid.dat",
                "ARQUIVO C/CARGAS ADICIONAIS : c_adic.dat",
                "ARQUIVO C/FATORES DE PERDAS : loss.dat",
                "ARQUIVO C/PATAMARES GTMIN   : gtminpat.dat",
                "ARQUIVO ENSO 1              : elnino.dat",
                "ARQUIVO ENSO 2              : ensoaux.dat",
                "ARQUIVO DSVAGUA             : dsvagua.dat",
                "ARQUIVO P/PENALID. POR DESV.: penalid.dat",
                "ARQUIVO C.GUIA / PENAL.VMINT: curva.dat",
                "ARQUIVO AGRUPAMENTO LIVRE   : agrint.dat",
                "ARQUIVO DESP. ANTEC. GNL    : adterm.dat",
                "ARQUIVO GER. HIDR. MIN      : ghmin.dat",
                "ARQUIVO AVERSAO RISCO - SAR : sar.dat",
                "ARQUIVO AVERSAO RISCO - CVAR: cvar.dat",
                "DADOS DOS RESER.EQ.ENERGIA  : ree.dat",
                "ARQUIVO RESTRICOES ELETRICAS: re.dat",
                "ARQUIVO DE TECNOLOGIAS      : tecno.dat",
                "DADOS DE ABERTURAS          : abertura.dat",
                "ARQUIVO DE EMISSOES GEE     : gee.dat",
                "ARQUIVO DE RESTRICAO DE GAS : clasgas.dat",
                "ARQUIVO DE DADOS SIM. FINAL : simfinal.dat",
                "ARQ. DE CORTES POS ESTUDO   : cortes-pos.dat",
                "ARQ. DE CABECALHO CORTES POS: cortesh-pos.dat",
                "ARQ. C/ VOLUME REF. SAZONAL : volref_saz.dat"

            };

            var arqdat = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("arquivos.dat")).FirstOrDefault();
            if (arqdat!= null && File.Exists(arqdat))
            {
                var linhasArq = File.ReadAllLines(arqdat).ToList();
                if (linhasArq.Any(x => !x.Trim().ToLower().EndsWith(".dat")))
                {
                    File.WriteAllLines(arqdat, linhasReferencia);
                }

            }

        }

        public static void AtualizaWeolNWDCProcess(Compass.CommomLibrary.Newave.Deck deckNW, Compass.CommomLibrary.Decomp.Deck deckDC, string csvFile)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            List<WeolSM> weolDados = new List<WeolSM>();
            var linhas = File.ReadAllLines(csvFile).ToList();
            var semanasTosplit = linhas[0].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int l = 2; l < linhas.Count(); l++)
            {
                var partes = linhas[l].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int i = 1; i < semanasTosplit.Count(); i = i + 3)
                {
                    WeolSM weol = new WeolSM();

                    weol.SemanaIni = Convert.ToDateTime(semanasTosplit[i], Culture.DateTimeFormat);
                    weol.SemanaFim = Convert.ToDateTime(semanasTosplit[i + 2], Culture.DateTimeFormat);
                    weol.Submercado = partes[0].ToUpper();
                    weol.CargaPat1 = Convert.ToDouble(partes[i].Replace(".", ","));
                    weol.CargaPat2 = Convert.ToDouble(partes[i + 1].Replace(".", ","));
                    weol.CargaPat3 = Convert.ToDouble(partes[i + 2].Replace(".", ","));

                    weolDados.Add(weol);
                }
            }

            if (deckNW != null)
            {
                DateTime datadeck = deckNW.Dger.DataEstudo;
                bool pat2024 = datadeck.Year == 2024;
                bool pat2025 = datadeck.Year >= 2025;
                var patamarDat = deckNW[CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;//lembrar de como pegar os dados de patamar e duração( indice do bloco duração é mes+1, indice do bloco peqnas é mes )
                var sistema = deckNW[CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as CommomLibrary.SistemaDat.SistemaDat;

                var patDura = patamarDat.Duracao.Where(d => d.Ano == datadeck.Year);

                //var intMedio =
                //intercambio.RestricaoP1 * patTemp.First(p => p.Patamar == 1)[dataModif.Month + 1]
                // + intercambio.RestricaoP2 * patTemp.First(p => p.Patamar == 2)[dataModif.Month + 1]
                //+ intercambio.RestricaoP3 * patTemp.First(p => p.Patamar == 3)[dataModif.Month + 1];
                bool alterou = false;

                foreach (var sub in weolDados.Select(x => x.SubNum).Distinct())
                {
                    List<Tuple<int, int, int>> duracaoSemPats = new List<Tuple<int, int, int>>();
                    var semanasMes = weolDados.Where(x => x.SubNum == sub && ((x.SemanaIni <= datadeck && x.SemanaFim >= datadeck) || (x.SemanaIni >= datadeck && x.SemanaFim <= datadeck.AddMonths(1).AddDays(5)))).ToList();
                    var linhasPatEol = patamarDat.Nao_Simuladas.Where(x => x is Compass.CommomLibrary.PatamarDat.UNSABLine && x.Submercado == sub && x.Ano == datadeck.Year /*&& x.Patamar == p*/ && x.Tipo_Usina == 3);

                    double CargaPatMensal1 = 0;//cargas mensais por patamar
                    double CargaPatMensal2 = 0;
                    double CargaPatMensal3 = 0;

                    double PatMensal1 = 0;//novos patamares do mes 
                    double PatMensal2 = 0;
                    double PatMensal3 = 0;

                    double CargaMensal = 0;//medio da carga por patamar

                    if (semanasMes.Count() > 0)//se tem semanas correspondentes ao mes do deck
                    {
                        alterou = true;

                        foreach (var item in semanasMes)
                        {
                            DateTime diaIni = item.SemanaIni < datadeck ? datadeck : item.SemanaIni;
                            DateTime diafim = item.SemanaFim > datadeck.AddMonths(1).AddDays(-1) ? datadeck.AddMonths(1).AddDays(-1) : item.SemanaFim;

                            var duracaoPats = Tools.GetHorasPatamares(diaIni, diafim, true, false, pat2024, pat2025);

                            duracaoSemPats.Add(duracaoPats);
                        }

                        for (int i = 0; i < semanasMes.Count(); i++)
                        {
                            CargaPatMensal1 = CargaPatMensal1 + (semanasMes[i].CargaPat1 * duracaoSemPats[i].Item1);
                            CargaPatMensal2 = CargaPatMensal2 + (semanasMes[i].CargaPat2 * duracaoSemPats[i].Item2);
                            CargaPatMensal3 = CargaPatMensal3 + (semanasMes[i].CargaPat3 * duracaoSemPats[i].Item3);
                        }

                        CargaPatMensal1 = CargaPatMensal1 / duracaoSemPats.Select(x => x.Item1).Sum();
                        CargaPatMensal2 = CargaPatMensal2 / duracaoSemPats.Select(x => x.Item2).Sum();
                        CargaPatMensal3 = CargaPatMensal3 / duracaoSemPats.Select(x => x.Item3).Sum();

                        CargaMensal = CargaPatMensal1 * patDura.First(p => p.Patamar == 1)[datadeck.Month + 1]
                                     + CargaPatMensal2 * patDura.First(p => p.Patamar == 2)[datadeck.Month + 1]
                                     + CargaPatMensal3 * patDura.First(p => p.Patamar == 3)[datadeck.Month + 1];

                        PatMensal1 = CargaPatMensal1 / CargaMensal;
                        PatMensal2 = CargaPatMensal2 / CargaMensal;
                        PatMensal3 = CargaPatMensal3 / CargaMensal;

                        var patEol = linhasPatEol.Where(x => x.Patamar == 1).First();
                        patEol[datadeck.Month] = PatMensal1;

                        patEol = linhasPatEol.Where(x => x.Patamar == 2).First();
                        patEol[datadeck.Month] = PatMensal2;

                        patEol = linhasPatEol.Where(x => x.Patamar == 3).First();
                        patEol[datadeck.Month] = PatMensal3;

                        var unsiEol = sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.PeqEneLine && x.Mercado == sub && x.Tipo_Usina == 3 && x.Ano == datadeck.Year).FirstOrDefault();
                        if (unsiEol != null)
                        {
                            unsiEol[datadeck.Month] = CargaMensal;
                        }

                    }
                }
                if (alterou == true)
                {
                    patamarDat.SaveToFile(createBackup: true);
                    sistema.SaveToFile(createBackup: true);

                    MessageBox.Show("Processo realizado com sucesso!", "Atualizar Weol decks NW DC ");
                }
                else
                {
                    MessageBox.Show("Não foram encontrados dados correspondentes ao mes de estudo", "Atualizar Weol decks NW DC ");
                }
            }
            else if (deckDC != null)
            {


                var dadger = deckDC[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
                DateTime datadeck = dadger.DataEstudo;
                var rvinicio = Tools.GetCurrRev(dadger.DataEstudo);
                bool pat2024 = datadeck.Year == 2024;
                bool pat2025 = datadeck.Year >= 2025;
                var mesOperativo = MesOperativo.CreateSemanal(rvinicio.revDate.Year, rvinicio.revDate.Month, true, false, pat2024, pat2025);

                bool alterou = false;

                foreach (var sub in weolDados.Select(x => x.SubNum).Distinct())
                {
                    var pqlinesEntrada = dadger.BlocoPq.Where(x => x.Usina.Trim().ToUpper().EndsWith("EOL") && x.SubMercado == sub).OrderByDescending(x => x.Estagio).Skip(1).FirstOrDefault();//ultimo estagio do primeiro mes do deck de entrada para usar para os estagios faltantes, caso não tenha dados correspondentes no weol
                    double Patsubst1 = 0;
                    double Patsubst2 = 0;
                    double Patsubst3 = 0;

                    if (pqlinesEntrada != null)
                    {
                        Patsubst1 = pqlinesEntrada.Pat1;
                        Patsubst2 = pqlinesEntrada.Pat2;
                        Patsubst3 = pqlinesEntrada.Pat3;
                    }

                    int estagio = 1;

                    var semanasMes = weolDados.Where(x => x.SubNum == sub && x.SemanaIni >= datadeck && x.SemanaFim <= mesOperativo.SemanasOperativas[mesOperativo.Estagios - 1].Fim).ToList();

                    if (semanasMes.Count() > 0)//se tem semanas correspondentes ao mes do deck
                    {
                        alterou = true;

                        foreach (var item in semanasMes)
                        {
                            int weekIndex = mesOperativo.SemanasOperativas.IndexOf(mesOperativo.SemanasOperativas.Where(x => x.Inicio == item.SemanaIni).First());
                            int est = 1;
                            for (DateTime d = datadeck; d <= semanasMes.Last().SemanaIni; d = d.AddDays(7))// procura ajustar o numero do estagios com os dados das semanas em casos  de decks e weols de rvs diferentes 
                            {
                                if (d == mesOperativo.SemanasOperativas[weekIndex].Inicio)
                                {
                                    estagio = est;
                                    break;
                                }
                                else
                                {
                                    est++;
                                }
                            }

                            var pqline = dadger.BlocoPq.Where(x => x.Usina.Trim().ToUpper().EndsWith("EOL") && x.SubMercado == sub && x.Estagio <= estagio).OrderByDescending(x => x.Estagio).FirstOrDefault();
                            var pqlineList = dadger.BlocoPq.Where(x => x.Usina.Trim().ToUpper().EndsWith("EOL") && x.SubMercado == sub && x.Estagio <= estagio).OrderByDescending(x => x.Estagio);
                            if (pqline != null)
                            {
                                if (pqline.Estagio == estagio)
                                {
                                    pqline.Pat1 = item.CargaPat1;
                                    pqline.Pat2 = item.CargaPat2;
                                    pqline.Pat3 = item.CargaPat3;
                                }
                                else if (pqline.Estagio < estagio)
                                {
                                    var pqlineNew = new PqLine();
                                    pqlineNew.Usina = pqline.Usina;
                                    pqlineNew.SubMercado = pqline.SubMercado;
                                    pqlineNew.Estagio = estagio;
                                    pqlineNew.Pat1 = item.CargaPat1;
                                    pqlineNew.Pat2 = item.CargaPat2;
                                    pqlineNew.Pat3 = item.CargaPat3;
                                    dadger.BlocoPq.InsertAfter(pqline, pqlineNew);
                                }
                            }
                        }
                        //replica os dados do ultimo estagio do deck de entrada para os estagios faltantes do primeiro mes, caso não exista os dados referentes no weol 
                        var pqFinalist = dadger.BlocoPq.Where(x => x.Usina.Trim().ToUpper().EndsWith("EOL") && x.SubMercado == sub).OrderByDescending(x => x.Estagio).ToList();
                        if (pqlinesEntrada != null && pqFinalist.Count >= 2 && (pqFinalist[0].Estagio - pqFinalist[1].Estagio) > 1)//necessita replicar o estagio de entrada
                        {
                            int novoEstagio = pqFinalist[1].Estagio + 1;
                            var pqlineNew = new PqLine();
                            pqlineNew.Usina = pqFinalist[1].Usina;
                            pqlineNew.SubMercado = pqFinalist[1].SubMercado;
                            pqlineNew.Estagio = novoEstagio;
                            pqlineNew.Pat1 = Patsubst1;
                            pqlineNew.Pat2 = Patsubst2;
                            pqlineNew.Pat3 = Patsubst3;
                            dadger.BlocoPq.InsertAfter(pqFinalist[1], pqlineNew);
                        }

                    }
                }
                if (alterou == true)
                {
                    dadger.SaveToFile(createBackup: true);

                    MessageBox.Show("Processo realizado com sucesso!", "Atualizar Weol decks NW DC ");
                }
                else
                {
                    MessageBox.Show("Não foram encontrados dados correspondentes ao mes de estudo", "Atualizar Weol decks NW DC ");
                }
            }


        }
        public static void AtualizaCargaMensal(Compass.CommomLibrary.Newave.Deck Deck, string filePath)
        {
            var patamarDat = Deck[CommomLibrary.Newave.Deck.DeckDocument.patamar].Document as Compass.CommomLibrary.PatamarDat.PatamarDat;
            var sistema = Deck[CommomLibrary.Newave.Deck.DeckDocument.sistema].Document as CommomLibrary.SistemaDat.SistemaDat;
            var c_adicA = (Deck[CommomLibrary.Newave.Deck.DeckDocument.cadic].Document as Compass.CommomLibrary.C_AdicDat.C_AdicDat).Adicao
            .Where(x => x is Compass.CommomLibrary.C_AdicDat.MerEneLine)
            .Cast<Compass.CommomLibrary.C_AdicDat.MerEneLine>();

            var c_adic = (Deck[CommomLibrary.Newave.Deck.DeckDocument.cadic].Document as Compass.CommomLibrary.C_AdicDat.C_AdicDat);

            Microsoft.Office.Interop.Excel.Application xlsApp = null;
            xlsApp = new Microsoft.Office.Interop.Excel.Application();

            Workbook wb = xlsApp.Workbooks.Open(filePath);
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            //Convert.ToDateTime(coms[1], Culture.DateTimeFormat)
            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //path = @"D:\Compass\Acomph\ACOMPH_31.03.2020.xls";
            try
            {

                // excel.DisplayAlerts = false;
                //excel.Visible = false;
                // excel.ScreenUpdating = true;
                //Workbook workbook = excel.Workbooks.Open(path);

                // wb = workb;

                Sheets sheets = wb.Worksheets;

                var N_Sheets = sheets.Count;

                var Dados = new List<CargasData>();
                //var Dados2 = new List<CargasData>();
                //var Postos = new List<(int Posto, object data)>();


                Worksheet worksheet = (Worksheet)sheets.get_Item(1);
                string sheetName = worksheet.Name;//Get the name of worksheet.

                int l = 2;
                int row = 0;
                for (var lin = l; !string.IsNullOrWhiteSpace(worksheet.Cells[lin, 1].Text); lin++)
                {
                    row = lin;
                }
                bool validado = true;// verifica se a planilha esta no padrão correto para coleta dos dados
                validado = (string)worksheet.Cells[1, 1].Text == "DATE" && validado == true;
                validado = (string)worksheet.Cells[1, 3].Text == "SOURCE" && validado == true;
                validado = (string)worksheet.Cells[1, 4].Text == "TYPE" && validado == true;
                validado = (string)worksheet.Cells[1, 6].Text == "LOAD_sMMGD" && validado == true;
                validado = (string)worksheet.Cells[1, 7].Text == "Base_CGH" && validado == true;
                validado = (string)worksheet.Cells[1, 8].Text == "Base_EOL" && validado == true;
                validado = (string)worksheet.Cells[1, 9].Text == "Base_UFV" && validado == true;
                validado = (string)worksheet.Cells[1, 10].Text == "Base_UTE" && validado == true;
                validado = (string)worksheet.Cells[1, 11].Text == "Base_MMGD" && validado == true;
                validado = (string)worksheet.Cells[1, 12].Text == "LOAD_cMMGD" && validado == true;
                validado = (string)worksheet.Cells[1, 13].Text == "Exp_CGH" && validado == true;
                validado = (string)worksheet.Cells[1, 14].Text == "Exp_EOL" && validado == true;
                validado = (string)worksheet.Cells[1, 15].Text == "Exp_UFV" && validado == true;
                validado = (string)worksheet.Cells[1, 16].Text == "Exp_UTE" && validado == true;
                validado = (string)worksheet.Cells[1, 17].Text == "Exp_MMGD" && validado == true;
                validado = (string)worksheet.Cells[1, 18].Text == "REVISION" && validado == true;

                if (validado == false)
                {
                    throw new Exception("Planilha fora de Padrão entre em contato com desenvolverdor!");
                }

                object[,] result = wb.Worksheets[sheetName].Range[wb.Worksheets[sheetName].Cells[l, 1], wb.Worksheets[sheetName].Cells[row, 18]].Value;


                for (int i = 1; i < row; i++)
                {
                    CargasData cargas = new CargasData();

                    cargas.Data = Convert.ToDateTime(result[i, 1], Culture.DateTimeFormat);
                    cargas.Revisao = Convert.ToDateTime(result[i, 18], Culture.DateTimeFormat);

                    cargas.Submercado = ((string)result[i, 3]).ToUpper();
                    cargas.Tipo = ((string)result[i, 4]).ToUpper();
                    cargas.LOAD_sMMGD = (double)result[i, 6];
                    cargas.Base_CGH = (double)result[i, 7];
                    cargas.Base_EOL = (double)result[i, 8];
                    cargas.Base_UFV = (double)result[i, 9];
                    cargas.Base_UTE = (double)result[i, 10];
                    cargas.Base_MMGD = (double)result[i, 11];
                    cargas.LOAD_cMMGD = (double)result[i, 12];
                    cargas.Exp_CGH = (double)result[i, 13];
                    cargas.Exp_EOL = (double)result[i, 14];
                    cargas.Exp_UFV = (double)result[i, 15];
                    cargas.Exp_UTE = (double)result[i, 16];
                    cargas.Exp_MMGD = (double)result[i, 17];

                    Dados.Add(cargas);
                }


                #region sistema.dat
                //sistema.dat
                for (int i = 1; i <= 4; i++)//mercado de energia === carga sem mmgd
                {
                    var linhasMercado = sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine && x.Mercado == i).ToList();

                    foreach (var linha in linhasMercado)
                    {
                        if (linha.Ano < Deck.Dger.AnoEstudo)
                        {
                            sistema.Mercado.Remove(linha);
                            int index = sistema.Mercado.IndexOf(linhasMercado.Last()) + 1;
                            var novoAno = linhasMercado.Last().Clone() as Compass.CommomLibrary.SistemaDat.MerEneLine;
                            novoAno.Ano = novoAno.Ano + 1;
                            sistema.Mercado.Insert(index, novoAno);
                        }
                        else if (linha.Ano == Deck.Dger.AnoEstudo)
                        {
                            for (int m = 1; m <= 12; m++)
                            {
                                if (m < Deck.Dger.MesEstudo)
                                {
                                    linha[m] = null;
                                }

                            }
                        }
                    }
                    foreach (var carga in Dados.Where(x => x.SubNum == i && x.Data >= Deck.Dger.DataEstudo && x.Tipo == "MEDIUM").ToList())
                    {
                        //var item = sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList();
                        var item = sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine && x.Mercado == i && x.Ano == carga.Data.Year).FirstOrDefault();
                        if (item != null)
                        {
                            if (i == 4)
                            {
                                double boaVistaLoad = 0;
                                var linhasCadicBoavista = c_adicA.Where(x => x is Compass.CommomLibrary.C_AdicDat.MerEneLine && x.Mercado == i && !x.Descricao.ToUpper().Contains("MMGD")).ToList();

                                var c_adicBoavista = linhasCadicBoavista.Where(x => !x.Ano.ToUpper().Contains("POS") && Convert.ToInt32(x.Ano) == carga.Data.Year).FirstOrDefault();
                                if (c_adicBoavista != null)
                                {
                                    boaVistaLoad = c_adicBoavista[carga.Data.Month];
                                }
                                item[carga.Data.Month] = carga.LOAD_sMMGD - boaVistaLoad;//ubtriar c_adic de boavista no caso do sub norte

                            }
                            else
                            {
                                item[carga.Data.Month] = carga.LOAD_sMMGD;
                            }

                        }
                    }
                }
                for (int s = 1; s <= 4; s++)// pequenas mmgd === exp_tipousina
                {
                    for (int t = 5; t <= 8; t++)
                    {
                        var linhasPequenas = sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.PeqEneLine && x.Mercado == s && x.Tipo_Usina == t).ToList();

                        foreach (var linha in linhasPequenas)
                        {
                            if (linha.Ano < Deck.Dger.AnoEstudo)
                            {
                                sistema.Pequenas.Remove(linha);
                                int index = sistema.Pequenas.IndexOf(linhasPequenas.Last()) + 1;
                                var novoAno = linhasPequenas.Last().Clone() as Compass.CommomLibrary.SistemaDat.PeqEneLine;
                                novoAno.Ano = novoAno.Ano + 1;
                                sistema.Pequenas.Insert(index, novoAno);
                            }
                            else if (linha.Ano == Deck.Dger.AnoEstudo)
                            {
                                for (int m = 1; m <= 12; m++)
                                {
                                    if (m < Deck.Dger.MesEstudo)
                                    {
                                        linha[m] = null;
                                    }

                                }
                            }
                        }
                        foreach (var carga in Dados.Where(x => x.SubNum == s && x.Data >= Deck.Dger.DataEstudo && x.Tipo == "MEDIUM").ToList())
                        {
                            //var item = sistema.Mercado.Where(x => x is Compass.CommomLibrary.SistemaDat.MerEneLine).ToList();
                            var item = sistema.Pequenas.Where(x => x is Compass.CommomLibrary.SistemaDat.PeqEneLine && x.Mercado == s && x.Tipo_Usina == t && x.Ano == carga.Data.Year).FirstOrDefault();
                            if (item != null)
                            {
                                // 5 pch== exp_cgh
                                // 6 pct == exp_UTE
                                // 7 eol == exp_eol
                                // 8 ufv == exp_ufv
                                double cargaMMGD = 0;
                                switch (t)
                                {
                                    case 5:
                                        cargaMMGD = carga.Exp_CGH;
                                        break;

                                    case 6:
                                        cargaMMGD = carga.Exp_UTE;
                                        break;

                                    case 7:
                                        cargaMMGD = carga.Exp_EOL;
                                        break;

                                    case 8:
                                        cargaMMGD = carga.Exp_UFV;
                                        break;

                                    default:
                                        cargaMMGD = 0;
                                        break;
                                }
                                item[carga.Data.Month] = cargaMMGD;

                            }
                        }
                    }
                }
                #endregion

                #region c_adic

                for (int i = 1; i <= 4; i++)
                {
                    var linhasCadic = c_adicA.Where(x => x is Compass.CommomLibrary.C_AdicDat.MerEneLine && x.Mercado == i && x.Descricao.ToUpper().Contains("MMGD")).ToList();
                    foreach (var linha in linhasCadic)
                    {
                        if (!linha.Ano.ToUpper().Contains("POS"))
                        {
                            if (Convert.ToInt32(linha.Ano) < Deck.Dger.AnoEstudo)
                            {
                                c_adic.Adicao.Remove(linha);
                                int index = c_adic.Adicao.IndexOf(linhasCadic.Last());
                                var novoAno = linhasCadic[linhasCadic.IndexOf(linhasCadic.Last()) - 1].Clone() as Compass.CommomLibrary.C_AdicDat.MerEneLine;
                                novoAno.Ano = (Convert.ToInt32(novoAno.Ano) + 1).ToString();
                                c_adic.Adicao.Insert(index, novoAno);
                            }
                            else if (Convert.ToInt32(linha.Ano) == Deck.Dger.AnoEstudo)
                            {
                                for (int m = 1; m <= 12; m++)
                                {
                                    if (m < Deck.Dger.MesEstudo)
                                    {
                                        linha[m] = null;
                                    }

                                }
                            }
                        }

                    }
                    foreach (var carga in Dados.Where(x => x.SubNum == i && x.Data >= Deck.Dger.DataEstudo && x.Tipo == "MEDIUM").ToList())
                    {
                        var item = linhasCadic.Where(x => !x.Ano.ToUpper().Contains("POS") && Convert.ToInt32(x.Ano) == carga.Data.Year).FirstOrDefault();
                        if (item != null)
                        {
                            item[carga.Data.Month] = Math.Round(carga.Base_MMGD);
                        }

                    }
                }


                #endregion

                #region Patamar.dat
                var dts = Dados.Select(x => x.Data).Distinct();
                foreach (var dt in Dados.Select(x => x.Data).Distinct())
                {
                    if (dt >= Deck.Dger.DataEstudo)
                    {
                        for (int s = 1; s <= 4; s++)
                        {
                            var cargaHigh = Dados.Where(x => x.Data == dt && x.Tipo == "HIGH" && x.SubNum == s).First();
                            var cargaMiddle = Dados.Where(x => x.Data == dt && x.Tipo == "MIDDLE" && x.SubNum == s).First();
                            var cargaLow = Dados.Where(x => x.Data == dt && x.Tipo == "LOW" && x.SubNum == s).First();
                            var cargaMedium = Dados.Where(x => x.Data == dt && x.Tipo == "MEDIUM" && x.SubNum == s).First();
                            for (int p = 1; p <= 3; p++)
                            {
                                var linhaCarga = patamarDat.Carga.Where(x => x is Compass.CommomLibrary.PatamarDat.CargaEneLine && x.Mercado == s && x.Ano == dt.Year && x.Patamar == p).FirstOrDefault();
                                var linhaUFVmmgd = patamarDat.Nao_Simuladas.Where(x => x is Compass.CommomLibrary.PatamarDat.UNSABLine && x.Submercado == s && x.Ano == dt.Year && x.Patamar == p && x.Tipo_Usina == 8).FirstOrDefault();
                                if (linhaCarga != null)
                                {
                                    double valor = 0;
                                    switch (p)
                                    {
                                        case 1:
                                            valor = cargaHigh.LOAD_cMMGD / cargaMedium.LOAD_cMMGD;
                                            break;
                                        case 2:
                                            valor = cargaMiddle.LOAD_cMMGD / cargaMedium.LOAD_cMMGD;
                                            break;
                                        case 3:
                                            valor = cargaLow.LOAD_cMMGD / cargaMedium.LOAD_cMMGD;
                                            break;
                                        default:
                                            valor = 0;
                                            break;
                                    }
                                    linhaCarga[dt.Month] = valor;
                                }
                                if (linhaUFVmmgd != null)
                                {
                                    double valorUFV = 0;
                                    switch (p)
                                    {
                                        case 1:
                                            valorUFV = cargaHigh.Exp_UFV / cargaMedium.Exp_UFV;
                                            break;
                                        case 2:
                                            valorUFV = cargaMiddle.Exp_UFV / cargaMedium.Exp_UFV;
                                            break;
                                        case 3:
                                            valorUFV = cargaLow.Exp_UFV / cargaMedium.Exp_UFV;
                                            break;
                                        default:
                                            valorUFV = 0;
                                            break;
                                    }
                                    linhaUFVmmgd[dt.Month] = valorUFV;
                                }

                            }
                        }


                    }

                }

                #endregion

                patamarDat.SaveToFile();

                c_adic.SaveToFile();

                sistema.SaveToFile();
                //

                wb.Close(SaveChanges: false);
                //workbook.Close();
                xlsApp.Quit();
                System.Windows.Forms.MessageBox.Show("Processo concuído com sucesso!!!");


            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                wb.Close(SaveChanges: false);
                xlsApp.Quit();
            }


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

        public static void AlterarCortes(Compass.CommomLibrary.Dadger.Dadger dadger, string cortesPath, bool nWh, DateTime dtEstudo)
        {
            var dt = dtEstudo;
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

            var x1 = cortesRelPath + cortes.Last();//todo:mudar nome do cortes adicionando o numero (ex:cortes-002.dat ou cortes-013.dat) para NW hibrido

            if (nWh == true)
            {
                int num = dt.Month + 1;
                string sufixo = num.ToString("000");
                string nwHcortes = $"cortes-{sufixo}.dat";
                x1 = x1.Replace(cortes.Last(), nwHcortes);
            }

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
    public class ColetaGtminCCEE
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

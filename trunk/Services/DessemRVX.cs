using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.EntdadosDat;
using Compass.CommomLibrary.Decomp;
using Compass.CommomLibrary;
using Compass.ExcelTools.Templates;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;



namespace Compass.Services
{
    public class DessemRVX
    {

        public static bool SalvarMapcutCortedeco(string decompPath, string dessemPath)
        {
            string decompRef = decompPath;


            string mapcut = "mapcut";
            string cortdeco = "cortdeco";
            //throw new NotImplementedException("Deck não reconhecido para a execução");
            //
            try
            {
                if (Directory.Exists(decompRef))
                {
                    var arqMapcut = Directory.GetFiles(decompRef).Where(x => Path.GetFileNameWithoutExtension(x).ToLower() == (mapcut)).FirstOrDefault();
                    var arqCortdeco = Directory.GetFiles(decompRef).Where(x => Path.GetFileNameWithoutExtension(x).ToLower() == (cortdeco)).FirstOrDefault();

                    if (File.Exists(arqMapcut) && File.Exists(arqCortdeco))// copia arquivos e altera dessem arq com os nome em minusculo (dessemarq é case sensitive)
                    {
                        mapcut = Path.GetFileName(arqMapcut);
                        cortdeco = Path.GetFileName(arqCortdeco);

                        File.Copy(arqMapcut, Path.Combine(dessemPath, mapcut.ToLower()), true);
                        File.Copy(arqCortdeco, Path.Combine(dessemPath, cortdeco.ToLower()), true);

                        var dessemArqFile = Directory.GetFiles(dessemPath).Where(x => Path.GetFileName(x).ToLower().Contains("dessem.arq")).First();
                        var dessemArq = DocumentFactory.Create(dessemArqFile) as Compass.CommomLibrary.DessemArq.DessemArq;

                        var mapline = dessemArq.BlocoArq.Where(x => x.Minemonico.ToUpper().Trim() == "MAPFCF").First();
                        var cortline = dessemArq.BlocoArq.Where(x => x.Minemonico.ToUpper().Trim() == "CORTFCF").First();
                        mapline.NomeArq = mapcut.ToLower();
                        cortline.NomeArq = cortdeco.ToLower();

                        foreach (var file in Directory.GetFiles(dessemPath).ToList())
                        {
                            var fileName = Path.GetFileName(file);
                            var minusculo = fileName.ToLower();
                            File.Move(Path.Combine(dessemPath, fileName), Path.Combine(dessemPath, minusculo));
                        }
                        foreach (var line in dessemArq.BlocoArq.ToList())
                        {
                            if (line.Minemonico.Trim() != "CASO" && line.Minemonico.Trim() != "TITULO")
                            {
                                string mini = line.NomeArq.ToLower();
                                line.NomeArq = mini;
                            }

                        }
                        dessemArq.SaveToFile(createBackup: true);
                        return true;
                    }
                    else
                    {
                        string texto = "Falha ao copiar arquivos, diretório ou arquivos decomp (Mapcut Cortdeco)inexistentes";
                        MessageBox.Show(texto, "DessemTools");
                        return false;
                    }

                }
                else
                {
                    string texto = "Falha ao copiar arquivos, diretório ou arquivos decomp (Mapcut Cortdeco)inexistentes";
                    MessageBox.Show(texto, "DessemTools");
                    return false;
                }
            }
            catch (Exception e)
            {
                string texto = "Falha ao copiar arquivos," + e.Message.ToString();
                MessageBox.Show(texto, "DessmeTools");
                return false;
            }


        }

        public static void CriarDeflant(string path, DateTime dataEstudo)
        {

            //K:\5_dessem\Arquivos_DESSEM\12_2023\07\NP07122023.txt
            //C:\Enercore\Energy Core Trading\Energy Core Pricing - Documentos\Arquivos_DESSEM\12_2023\07\NP07122023.txt

            string arqNP = "";
            float valor = 0f;

            var deflantFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("deflant")).First();
            var deflant = DocumentFactory.Create(deflantFile) as Compass.CommomLibrary.Deflant.Deflant;


            var entdadosFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
            var tviag = entdados.BlocoTviag.ToList();

            string comentario = deflant.BlocoDef.First().Comment;

            var montantes = deflant.BlocoDef.Select(x => x.Montante).Distinct().ToList();
            var linhas66_83 = deflant.BlocoDef.Where(x => x.Montante == 66 || x.Montante == 83).ToList();
            deflant.BlocoDef.Clear();


            foreach (var tv in tviag)
            {
                if (tv.Montante != 66 && tv.Montante != 83)
                {
                    if (tv.Montante == 156)
                    {

                    }
                    var horas = tv.TempoViag;
                    var dataAnt = dataEstudo.AddHours(-horas);
                    for (int i = 0; i < horas; i += 24)
                    {
                        arqNP = Tools.GetNPTXT(dataAnt,true);
                        if (arqNP != "")
                        {
                            valor = Tools.GetNPValue(arqNP, tv.Montante.ToString());
                            var defline = new Compass.CommomLibrary.Deflant.DefLine();
                            if (deflant.BlocoDef.Count() == 0)
                            {
                                defline.Comment = comentario;
                            }
                            defline.Montante = tv.Montante;
                            defline.Jusante = tv.Jusante;
                            defline.Tipo = tv.TipoJus;
                            defline.Diainic = dataAnt.Day;
                            defline.Horainic = 00;
                            defline.Meiainic = 0;
                            defline.Diafim = " F";
                            defline.Defluencia = valor;
                            deflant.BlocoDef.Add(defline);

                            dataAnt = dataAnt.AddDays(1);
                        }

                    }
                }

            }

            //foreach (var mont in montantes.Where(x => x != 66 && x != 83))
            //{

            //}
            foreach (var def in linhas66_83)
            {
                deflant.BlocoDef.Add(def);
            }
            int tv83 = tviag.Where(x => x.Montante == 83).Select(x => x.TempoViag).FirstOrDefault();
            int tv66 = tviag.Where(x => x.Montante == 66).Select(x => x.TempoViag).FirstOrDefault();

            arqNP = Tools.GetNPTXT(dataEstudo.AddHours(-tv83), true);
            float valor83 = Tools.GetNPValue(arqNP, "83");

            foreach (var line in deflant.BlocoDef.Where(x => x.Montante == 66 || x.Montante == 83).ToList())// as usinas 66 itaipu e 83 baixo iguaçu são preenchidas de forma diferente pq abri em 48 estagios 
            {
                //usina 66 só muda o dia inicial e usa os dados do deck base, usina 83 usa os dados do NP mais recente de acordo com a data do deck sendo criada
                if (line.Montante == 83)
                {
                    line.Diainic = dataEstudo.AddHours(-tv83).Day;

                    line.Defluencia = valor83;
                }
                else if (line.Montante == 66)
                {
                    line.Diainic = dataEstudo.AddHours(-tv66).Day;
                }


            }

            deflant.SaveToFile(createBackup: true);
        }

        public static void CriarCotasr11(string path, DateTime dataEstudo)
        {
            var cotasr11File = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("cotasr11")).First();
            var cotasr11 = DocumentFactory.Create(cotasr11File) as Compass.CommomLibrary.Cotasr.Cotasr;

            foreach (var line in cotasr11.BlocoCot.ToList())
            {
                line.Dia = dataEstudo.AddDays(-1).Day;
            }
            cotasr11.SaveToFile(createBackup: true);
        }

        public static void CriarPtoper(string path, DateTime dataEstudo)
        {
            var ptoperFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("ptoper")).FirstOrDefault();

            var ptoper = DocumentFactory.Create(ptoperFile) as Compass.CommomLibrary.PtoperDat.PtoperDat;
            foreach (var line in ptoper.BlocoPtoper.ToList())
            {
                var datRef = dataEstudo;
                line.DiaIni = datRef.Day.ToString();
            }

            ptoper.SaveToFile(createBackup: true);
        }

        public static void CriarDadvazSabado(string path, string pathSab, DateTime data)
        {
            var dadvazFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz.dat")).First();
            var dadvaz = DocumentFactory.Create(dadvazFile) as Compass.CommomLibrary.Dadvaz.Dadvaz;
            var dataLine = dadvaz.BlocoData.First();
            dataLine.Dia = data.Day;
            dataLine.Mes = data.Month;
            dataLine.Ano = data.Year;
            var vazoes = dadvaz.BlocoVazoes.ToList();
            var comment = vazoes.First().Comment;
            dadvaz.BlocoVazoes.Clear();


            var diaLine = dadvaz.BlocoDia.First();
            int dia = 0;
            switch (data.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    dia = 1;
                    break;
                case DayOfWeek.Sunday:
                    dia = 2;
                    break;
                case DayOfWeek.Monday:
                    dia = 3;
                    break;
                case DayOfWeek.Tuesday:
                    dia = 4;
                    break;
                case DayOfWeek.Wednesday:
                    dia = 5;
                    break;
                case DayOfWeek.Thursday:
                    dia = 6;
                    break;
                case DayOfWeek.Friday:
                    dia = 7;
                    break;
                default:
                    dia = 1;
                    break;

            }
            diaLine.diainicial = dia;

            //pegando dados do deck do sabado anterior para ajustar no novo deck criado 
            var dadvazSabFile = Directory.GetFiles(pathSab).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz.dat")).First();
            var dadvazSab = DocumentFactory.Create(dadvazSabFile) as Compass.CommomLibrary.Dadvaz.Dadvaz;
            var dataLineSab = dadvazSab.BlocoData.First();
            DateTime dataSabAnt = new DateTime(dataLineSab.Ano, dataLineSab.Mes, dataLineSab.Dia);
            var vazoesSab = dadvazSab.BlocoVazoes.ToList();

            TimeSpan ts = data - dataSabAnt;
            double dias = ts.TotalDays;


            //var vazoes = dadvaz.BlocoVazoes.ToList();

            foreach (var vaz in vazoesSab)
            {
                int diateste = vaz.DiaInic.Trim().ToUpper() == "I" ? dataSabAnt.Day : Convert.ToInt32(vaz.DiaInic);

                DateTime dataTest;
                if (diateste < dataSabAnt.Day)//trata viradas de meses para fazer as comparaçoes com datas 
                {
                    dataTest = new DateTime(dataSabAnt.AddMonths(1).Year, dataSabAnt.AddMonths(1).Month, diateste);
                }
                else
                {
                    dataTest = new DateTime(dataSabAnt.Year, dataSabAnt.Month, diateste);
                }

                var newVaz = new CommomLibrary.Dadvaz.VazoesLine();
                newVaz.DiaInic = $"{dataTest.AddDays(dias).Day:00}";
                newVaz.DiaFinal = $"F";
                newVaz.Usina = vaz.Usina;
                newVaz.Nome = vaz.Nome;
                newVaz.TipoVaz = vaz.TipoVaz;
                newVaz.Vazao = vaz.Vazao;
                dadvaz.BlocoVazoes.Add(newVaz);
            }

            dadvaz.BlocoVazoes.First().Comment = comment;
            dadvaz.SaveToFile();
        }

        public static void CriarDadvazSemanal(string path, DateTime dateBase, DateTime dataFim, DateTime data)
        {
            var dadvazFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz.dat")).First();
            var dadvaz = DocumentFactory.Create(dadvazFile) as Compass.CommomLibrary.Dadvaz.Dadvaz;
            var dataLine = dadvaz.BlocoData.First();
            dataLine.Dia = data.Day;
            dataLine.Mes = data.Month;
            dataLine.Ano = data.Year;

            var diaLine = dadvaz.BlocoDia.First();
            int dia = 0;
            switch (data.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    dia = 1;
                    break;
                case DayOfWeek.Sunday:
                    dia = 2;
                    break;
                case DayOfWeek.Monday:
                    dia = 3;
                    break;
                case DayOfWeek.Tuesday:
                    dia = 4;
                    break;
                case DayOfWeek.Wednesday:
                    dia = 5;
                    break;
                case DayOfWeek.Thursday:
                    dia = 6;
                    break;
                case DayOfWeek.Friday:
                    dia = 7;
                    break;
                default:
                    dia = 1;
                    break;

            }
            diaLine.diainicial = dia;

            var vazoes = dadvaz.BlocoVazoes.ToList();
            var comment = vazoes.First().Comment;
            var usinas = dadvaz.BlocoVazoes.Select(x => x.Usina).Distinct().ToList();



            foreach (var u in usinas)
            {
                if (u == 21)
                {

                }
                for (DateTime d = dateBase; d <= dataFim; d = d.AddDays(1))
                {
                    var vazoesL = dadvaz.BlocoVazoes.Where(x => x.Usina == u /*&& (x.DiaInic.Trim() == "I" ||Convert.ToInt32(x.DiaInic) == d.Day)*/).ToList();
                    foreach (var vaz in vazoesL)
                    {
                        int diateste = vaz.DiaInic.Trim().ToUpper() == "I" ? dateBase.Day : Convert.ToInt32(vaz.DiaInic);
                        float vazao = vaz.Vazao;

                        DateTime dataTest;
                        if (diateste < dateBase.Day)//trata viradas de meses para fazer as comparaçoes com datas 
                        {
                            dataTest = new DateTime(dateBase.AddMonths(1).Year, dateBase.AddMonths(1).Month, diateste);
                        }
                        else
                        {
                            dataTest = new DateTime(dateBase.Year, dateBase.Month, diateste);
                        }

                        int regs = vazoesL.Where(x => x.Usina == vaz.Usina).ToList().Count();

                        if (regs == 1)//só existe uma linha logo só e necessario ajustar a data 
                        {
                            vaz.DiaInic = $"{data.Day:00}";// altera o dia de acordo com a data do deck
                        }
                        else if (dataTest < data)
                        {
                            var vazSeg = vazoes.Where(x => x.Usina == vaz.Usina && Convert.ToInt32(x.DiaInic) == dataTest.AddDays(1).Day).FirstOrDefault();
                            if (vazSeg == null)
                            {
                                var newVaz = new CommomLibrary.Dadvaz.VazoesLine();
                                newVaz.DiaInic = $"{dataTest.AddDays(1).Day:00}";
                                newVaz.DiaFinal = $"F";
                                newVaz.Usina = vaz.Usina;
                                newVaz.Nome = vaz.Nome;
                                newVaz.TipoVaz = vaz.TipoVaz;
                                newVaz.Vazao = vaz.Vazao;

                                dadvaz.BlocoVazoes.InsertAfter(vaz, newVaz);
                                dadvaz.BlocoVazoes.Remove(vaz);
                            }
                            //else if (dataTest < data)
                            //{
                            //    dadvaz.BlocoVazoes.Remove(vaz);
                            //}
                        }
                    }
                }




            }

            foreach (var item in dadvaz.BlocoVazoes.ToList())
            {
                int diateste = item.DiaInic.Trim().ToUpper() == "I" ? dateBase.Day : Convert.ToInt32(item.DiaInic);

                DateTime dataTest;
                if (diateste < dateBase.Day)//trata viradas de meses para fazer as comparaçoes com datas 
                {
                    dataTest = new DateTime(dateBase.AddMonths(1).Year, dateBase.AddMonths(1).Month, diateste);
                }
                else
                {
                    dataTest = new DateTime(dateBase.Year, dateBase.Month, diateste);
                }

                if (dataTest < data)
                {
                    dadvaz.BlocoVazoes.Remove(item);
                }
            }

            dadvaz.BlocoVazoes.First().Comment = comment;

            dadvaz.SaveToFile(createBackup: true);
        }

        public static void CriarRenovaveis(string path, DateTime dataEstudo, string pathSabAnt, DateTime dataSabAnt)
        {
            //copia renovaveis do deck do sabado anterior para o deck criado e ajusta os dias inicias e finais 
            var renovaveisFileAnt = Directory.GetFiles(pathSabAnt).Where(x => Path.GetFileName(x).ToLower().Contains("renovaveis")).FirstOrDefault();
            if (renovaveisFileAnt != null)
            {
                File.Copy(renovaveisFileAnt, Path.Combine(path, Path.GetFileName(renovaveisFileAnt)), true);

                var renovaveisFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("renovaveis")).FirstOrDefault();

                var renovaveis = DocumentFactory.Create(renovaveisFile) as Compass.CommomLibrary.Renovaveis.Renovaveis;

                var firstEolica = renovaveis.BlocoEolica.First();

                foreach (var eol in renovaveis.BlocoEolica.ToList())//trata erros nos numeros de caracteres lidos quando o nome da usina esta com caracteres não reconhecidos
                {
                    eol.Nome = eol.Nome.Substring(0, eol.Nome.Length - 1) + ";";
                    while (eol.Nome.Length < 42)
                    {
                        eol.Nome.Insert(eol.Nome.Length - 1, " ");
                    }
                    eol.PotMax = firstEolica.PotMax;
                    eol.FatCap = firstEolica.FatCap;
                    eol.FlagFuncao = firstEolica.FlagFuncao;
                }

                foreach (var geraline in renovaveis.BlocoEolicaGeracao.ToList())
                {
                    DateTime dataIniRef;
                    DateTime dataFimRef;

                    int diaIniREf = Convert.ToInt32(geraline.DiaIni.Replace(';', ' ').Trim());
                    int diafimREf = Convert.ToInt32(geraline.DiaFim.Replace(';', ' ').Trim());

                    if (diaIniREf < dataSabAnt.Day)
                    {
                        dataIniRef = new DateTime(dataSabAnt.AddMonths(1).Year, dataSabAnt.AddMonths(1).Month, diaIniREf);
                    }
                    else
                    {
                        dataIniRef = new DateTime(dataSabAnt.Year, dataSabAnt.Month, diaIniREf);
                    }

                    if (diafimREf < dataSabAnt.Day)
                    {
                        dataFimRef = new DateTime(dataSabAnt.AddMonths(1).Year, dataSabAnt.AddMonths(1).Month, diafimREf);
                    }
                    else
                    {
                        dataFimRef = new DateTime(dataSabAnt.Year, dataSabAnt.Month, diafimREf);
                    }
                    TimeSpan tsinicial = dataIniRef - dataSabAnt;
                    TimeSpan tsFinal = dataFimRef - dataIniRef;

                    double diasInicio = tsinicial.TotalDays;
                    double diasfim = tsFinal.TotalDays;

                    DateTime novoInicio = dataEstudo.AddDays(diasInicio);
                    DateTime novoFim = novoInicio.AddDays(diasfim);



                    geraline.DiaIni = novoInicio.Day < 10 ? " " + novoInicio.Day.ToString() + " ;" : novoInicio.Day.ToString() + " ;";
                    geraline.DiaFim = novoFim.Day < 10 ? " " + novoFim.Day.ToString() + " ;" : novoFim.Day.ToString() + " ;";


                }

                renovaveis.SaveToFile(createBackup: true);
            }

        }

        public static bool CriarDessopc(string path)
        {
            bool alterou = false;
            var dessopcFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("dessopc.dat")).FirstOrDefault();
            if (dessopcFile != null && File.Exists(dessopcFile))
            {
                List<string> nlinhas = new List<string>();

                var linhas = File.ReadAllLines(dessopcFile).ToList();
                foreach (var l in linhas)
                {
                    if (l.ToUpper().Contains("UCTERM"))
                    {
                        string nl = l.Substring(l.IndexOf("U"));
                        nlinhas.Add(nl);
                        continue;
                    }
                    if (l.ToUpper().Contains("CROSSOVER"))
                    {
                        string nl = "&" + l.Substring(l.IndexOf("C"));
                        nlinhas.Add(nl);
                        continue;
                    }
                    nlinhas.Add(l);
                }

                File.WriteAllLines(dessopcFile, nlinhas);
                alterou = true;
            }

            return alterou;
        }

        public static void CriarOperut(string path, DateTime dataEstudo, bool contemDessopc = false)
        {
            var operutFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("operut.dat")).First();

            var operut = DocumentFactory.Create(operutFile) as Compass.CommomLibrary.Operut.Operut;
            foreach (var line in operut.BlocoOper.ToList())
            {
                //var datRef = new DateTime(dataBase.Year, dataBase.Month, Convert.ToInt32(line.DiaInicial));
                var datRef = dataEstudo;
                // datRef = datRef.AddDays(incremento);
                line.DiaInicial = datRef.Day.ToString();
            }

            operut.SaveToFile(createBackup: true);
            if (contemDessopc == false)
            {
                List<string> nlinhas = new List<string>();

                var linhas = File.ReadAllLines(operutFile).ToList();
                foreach (var l in linhas)
                {
                    if (l.ToUpper().Contains("UCTERM"))
                    {
                        string nl = l.Substring(l.IndexOf("U"));
                        nlinhas.Add(nl);
                        continue;
                    }
                    if (l.ToUpper().Contains("CROSSOVER"))
                    {
                        string nl = "&" + l.Substring(l.IndexOf("C"));
                        nlinhas.Add(nl);
                        continue;
                    }
                    nlinhas.Add(l);
                }

                File.WriteAllLines(operutFile, nlinhas);
            }
           
        }

        public static void CriarOperuh(string path, DateTime dataEstudo, string pathSabAnt, DateTime dataSabAnt, DateTime fimrev)
        {
            var operuhFileAnt = Directory.GetFiles(pathSabAnt).Where(x => Path.GetFileName(x).ToLower().Contains("operuh")).FirstOrDefault();
            if (operuhFileAnt != null)
            {
                File.Copy(operuhFileAnt, Path.Combine(path, Path.GetFileName(operuhFileAnt)), true);

                var operuhFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("operuh")).First();
                var operuh = DocumentFactory.Create(operuhFile) as Compass.CommomLibrary.Operuh.Operuh;


                var rhesOperuhG = operuh.BlocoRhest.RhestGrouped.ToList();

                foreach (var rhes in rhesOperuhG.ToList())
                {

                    foreach (var rhe in rhes.Value)
                    {
                        if (rhe is Compass.CommomLibrary.Operuh.LimLine || rhe is Compass.CommomLibrary.Operuh.VarLine)
                        {

                            DateTime dataInicial = new DateTime(dataSabAnt.Year, dataSabAnt.Month, rhe.DiaInic.Trim() == "I" ? dataSabAnt.Day : Convert.ToInt32(rhe.DiaInic));

                            DateTime dataFinal = new DateTime(dataSabAnt.Year, dataSabAnt.Month, rhe.DiaFinal.Trim() == "F" ? fimrev.Day : Convert.ToInt32(rhe.DiaFinal));

                            //ajustando viradas de meses 
                            if (dataSabAnt.Day < 10)
                            {
                                if (dataInicial.Day > 20)
                                {
                                    dataInicial = dataInicial.AddMonths(-1);
                                }
                            }
                            if (dataSabAnt.Day > 20 && dataInicial.Day < 10)
                            {
                                dataInicial = dataInicial.AddMonths(1);
                            }
                            if (dataSabAnt.Day > dataFinal.Day)
                            {
                                dataFinal = dataFinal.AddMonths(1);
                            }
                            //fim ajuste de meses

                            TimeSpan tsinicial = dataInicial - dataSabAnt;
                            TimeSpan tsFinal = dataFinal - dataInicial;

                            double diasInicio = tsinicial.TotalDays;
                            double diasfim = tsFinal.TotalDays;

                            DateTime novoInicio = dataEstudo.AddDays(diasInicio);
                            DateTime novoFim = novoInicio.AddDays(diasfim);

                            if (rhe.DiaInic.Trim() != "I")
                            {
                                rhe.DiaInic = novoInicio.Day.ToString("D2");
                            }

                            if (rhe.DiaFinal.Trim() != "F")
                            {
                                rhe.DiaFinal = novoFim.Day.ToString("D2");
                            }

                        }

                    }
                }

                operuh.SaveToFile(createBackup: true);

            }

        }

        public static void CriarRespot(string path, DateTime dataEstudo)
        {
            var entdadosFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
            var respotFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("respot")).First();
            var respot = DocumentFactory.Create(respotFile) as Compass.CommomLibrary.Respot.Respot;

            var blocoTm = entdados.BlocoTm.ToList();
            var blocoDp = entdados.BlocoDp;

            var rp = respot.BlocoRp.First();
            rp.AREA = 1;
            rp.DiaIni = dataEstudo.Day.ToString();
            rp.HoraDiaIni = 0;
            rp.MeiaHoraIni = 0;
            rp.DiaFinal = "F";


            respot.BlocoLm.Clear();


            foreach (var tm in blocoTm)
            {
                var newL = new Compass.CommomLibrary.Respot.LmLine();
                if (tm == blocoTm.First())
                {
                    newL.Comment = "&";
                }
                newL.IdLine = "LM";
                newL.CadArea = rp.AREA;
                newL.DiaIni = tm.DiaInicial;
                newL.HoraDiaIni = tm.HoraDiaInicial;
                newL.MeiaHoraIni = tm.MeiaHora;
                newL.DiaFinal = "F";
                newL.Reserva = Tools.GetRespotValor(tm.DiaInicial, tm.HoraDiaInicial, tm.MeiaHora, blocoDp);
                respot.BlocoLm.Add(newL);
            }
            respot.SaveToFile(createBackup: true);
        }

        public static void CriarEntdados(string path, DateTime dataBase, DateTime dataEstudo, DateTime fimrev, string pathSabAnt, DateTime sabAnt, Compass.CommomLibrary.Dadger.Dadger dadger)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");

            var entdadosFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

            var entdadosFileAnt = Directory.GetFiles(pathSabAnt).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdadosAnt = DocumentFactory.Create(entdadosFileAnt) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

            #region DP/DE

            var inicioRev = fimrev.AddDays(-6);

            List<Tuple<int, DateTime, double, float>> dadosCargaPREV = Tools.GetDadosPrevCargaDS(dataEstudo);


            string comentarioDP = entdados.BlocoDp.First().Comment;

            entdados.BlocoDp.Clear();

            for (int s = 1; s <= 4; s++)//submercado
            {
                for (DateTime d = dataEstudo; d <= fimrev; d = d.AddDays(1))//
                {
                    if (d == dataEstudo)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            var dadoPrev = dadosCargaPREV.Where(x => x.Item1 == s && x.Item2.Date == d && x.Item3 == i).First();
                            float valor = dadoPrev.Item4;
                            var newDP = new Compass.CommomLibrary.EntdadosDat.DpLine();
                            if (s == 1 && i == 1)
                            {
                                newDP.Comment = comentarioDP;
                            }
                            newDP.IdBloco = "DP";
                            newDP.Subsist = s;
                            newDP.DiaInic = $"{d.Day:00}";
                            newDP.HoraInic = dadoPrev.Item2.Hour;
                            newDP.MeiaHoraInic = dadoPrev.Item2.Minute == 30 ? 1 : 0;
                            newDP.DiaFinal = " F";
                            newDP.Demanda = valor;
                            entdados.BlocoDp.Add(newDP);

                        }

                    }
                    else//pega os dados do csv dos dias seguintes para o calculo da media por horas agrupadas
                    {


                        bool pat2023 = d.Year == 2023;
                        bool pat2024 = d.Year == 2024;
                        bool pat2025 = d.Year >= 2025;

                        var intervalosAgruped = Tools.GetIntervalosPatamares(d, pat2023, pat2024, pat2025);

                        foreach (var inter in intervalosAgruped)
                        {
                            var dadoPrevList = dadosCargaPREV.Where(x => x.Item1 == s && x.Item2 >= d.AddHours(inter.Item1 - 0.5) && x.Item2 <= d.AddHours(inter.Item2)).ToList();//pega da hora.30min até a hora cheia hora.00min, assim que esta fazendo pelo oficial
                            float media = dadoPrevList.Select(x => x.Item4).Average();

                            var newDpSeguinte = new Compass.CommomLibrary.EntdadosDat.DpLine();

                            newDpSeguinte.IdBloco = "DP";
                            newDpSeguinte.Subsist = s;
                            newDpSeguinte.DiaInic = $"{d.Day:00}";
                            newDpSeguinte.HoraInic = inter.Item1 - 1;
                            newDpSeguinte.MeiaHoraInic = 0;
                            newDpSeguinte.DiaFinal = " F";
                            newDpSeguinte.Demanda = media;
                            entdados.BlocoDp.Add(newDpSeguinte);
                        }
                    }
                }
            }

            var newDP11 = new Compass.CommomLibrary.EntdadosDat.DpLine();
            newDP11.IdBloco = "DP";
            newDP11.Subsist = 11;
            newDP11.DiaInic = $"{dataEstudo.Day:00}";
            newDP11.HoraInic = 0;
            newDP11.MeiaHoraInic = 0;
            newDP11.DiaFinal = " F";
            newDP11.Demanda = 0.0f;
            entdados.BlocoDp.Add(newDP11);

            //DE
            #region DE
            var blocoDEant = entdadosAnt.BlocoDe.ToList();
            string commentDE = entdados.BlocoDe.First().Comment;
            entdados.BlocoDe.Clear();

            foreach (var de in blocoDEant)
            {
                DateTime dataInicial = new DateTime(sabAnt.Year, sabAnt.Month, de.DiaInic.Trim() == "I" ? sabAnt.Day : Convert.ToInt32(de.DiaInic));

                DateTime dataFinal = new DateTime(sabAnt.Year, sabAnt.Month, de.DiaFinal.Trim() == "F" ? sabAnt.AddDays(6).Day : Convert.ToInt32(de.DiaFinal));

                //ajustando viradas de meses 
                if (sabAnt.Day < 10)
                {
                    if (dataInicial.Day > 20)
                    {
                        dataInicial = dataInicial.AddMonths(-1);
                    }
                }
                if (sabAnt.Day > 20 && dataInicial.Day < 10)
                {
                    dataInicial = dataInicial.AddMonths(1);
                }
                if (sabAnt.Day > dataFinal.Day)
                {
                    dataFinal = dataFinal.AddMonths(1);
                }
                //fim ajuste de meses

                TimeSpan tsinicial = dataInicial - sabAnt;
                TimeSpan tsFinal = dataFinal - dataInicial;

                double diasInicio = tsinicial.TotalDays;
                double diasfim = tsFinal.TotalDays;

                DateTime novoInicio = dataEstudo.AddDays(diasInicio);
                DateTime novoFim = novoInicio.AddDays(diasfim);

                if (de.DiaInic.Trim() != "I")
                {
                    de.DiaInic = novoInicio.Day.ToString("D2");
                }

                if (de.DiaFinal.Trim() != "F")
                {
                    de.DiaFinal = novoFim.Day.ToString("D2");
                }

                entdados.BlocoDe.Add(de);
            }
            entdados.BlocoDe.First().Comment = commentDE;

            #endregion
            #endregion

            #region BLOCO TM
            string comentario = entdados.BlocoTm.First().Comment;
            entdados.BlocoTm.Clear();

            for (DateTime dtm = dataEstudo; dtm <= fimrev; dtm = dtm.AddDays(1))
            {
                bool patamres2023 = dtm.Year == 2023;
                bool patamares2024 = dtm.Year == 2024;
                bool patamares2025 = dtm.Year >= 2025;

                var intervalosHor = Tools.GetIntervalosHoararios(dtm, patamres2023, patamares2024, patamares2025);
                var intervalosAgrpHor = Tools.GetIntervalosPatamares(dtm, patamres2023, patamares2024, patamares2025);

                if (dtm == dataEstudo)//primeiro dia 
                {
                    for (int i = 0; i < 24; i++)
                    {
                        var newline = new Compass.CommomLibrary.EntdadosDat.TmLine();
                        newline.IdBloco = "TM";
                        newline.DiaInicial = $"{dtm.Day:00}";
                        newline.HoraDiaInicial = i;
                        newline.MeiaHora = 0;
                        newline.Duracao = 0.5f;
                        newline.Rede = 0;
                        newline.NomePatamar = intervalosHor[i];
                        if (i == 0)
                        {
                            newline.Comment = comentario;
                        }
                        entdados.BlocoTm.Add(newline);
                        var newline2 = new Compass.CommomLibrary.EntdadosDat.TmLine();
                        newline2.IdBloco = "TM";
                        newline2.DiaInicial = $"{dtm.Day:00}";
                        newline2.HoraDiaInicial = i;
                        newline2.MeiaHora = 1;
                        newline2.Duracao = 0.5f;
                        newline2.Rede = 0;
                        newline2.NomePatamar = intervalosHor[i];
                        entdados.BlocoTm.Add(newline2);
                    }
                }
                else
                {
                    foreach (var agrHr in intervalosAgrpHor)
                    {
                        var newline = new Compass.CommomLibrary.EntdadosDat.TmLine();
                        newline.IdBloco = "TM";
                        newline.DiaInicial = $"{dtm.Day:00}";
                        newline.HoraDiaInicial = agrHr.Item1 - 1;
                        newline.MeiaHora = 0;
                        newline.Duracao = (agrHr.Item2 - agrHr.Item1) + 1;
                        newline.Rede = 0;
                        newline.NomePatamar = intervalosHor[newline.HoraDiaInicial];
                        entdados.BlocoTm.Add(newline);

                    }
                }
            }

            #endregion

            #region BLOCO UT
            foreach (var utline in entdados.BlocoUt.ToList())
            {
                DateTime dataInicial = new DateTime(dataBase.Year, dataBase.Month, utline.DiaInic.Trim() == "I" ? dataBase.Day : Convert.ToInt32(utline.DiaInic));

                DateTime dataFinal = new DateTime(dataBase.Year, dataBase.Month, utline.DiaFinal.Trim() == "F" ? sabAnt.AddDays(6).Day : Convert.ToInt32(utline.DiaFinal));

                //ajustando viradas de meses 
                if (dataBase.Day < 10)
                {
                    if (dataInicial.Day > 20)
                    {
                        dataInicial = dataInicial.AddMonths(-1);
                    }
                }
                if (dataBase.Day > 20 && dataInicial.Day < 10)
                {
                    dataInicial = dataInicial.AddMonths(1);
                }
                if (dataBase.Day > dataFinal.Day)
                {
                    dataFinal = dataFinal.AddMonths(1);
                }
                //fim ajuste de meses

                TimeSpan tsinicial = dataInicial - dataBase;
                TimeSpan tsFinal = dataFinal - dataInicial;

                double diasInicio = tsinicial.TotalDays;
                double diasfim = tsFinal.TotalDays;

                DateTime novoInicio = dataEstudo.AddDays(diasInicio);
                DateTime novoFim = novoInicio.AddDays(diasfim);

                if (utline.DiaInic.Trim() != "I")
                {
                    utline.DiaInic = novoInicio.Day.ToString("D2");
                }

                if (utline.DiaFinal.Trim() != "F")
                {
                    utline.DiaFinal = novoFim.Day.ToString("D2");
                }

                //DateTime datUt = new DateTime(dataBase.Year, dataBase.Month, Convert.ToInt32(utline.DiaInic));
                //datUt = datUt.AddDays(incremento);
                // utline.DiaInic = $"{dataEstudo.Day:00}";
            }


            #endregion

            #region BLOCO RI

            var riLines = entdados.BlocoRi.ToList();

            foreach (var ri in riLines)
            {
                Tuple<DateTime, DateTime> rangeDias = Tools.GetRangeInicialFinal(sabAnt, sabAnt, ri.DiaInic, ri.DiaFinal, dataEstudo);

                if (ri.DiaInic.Trim() != "I")
                {
                    ri.DiaInic = $"{rangeDias.Item1.Day:00}";
                }

                if (ri.DiaFinal.Trim() != "F")
                {
                    ri.DiaFinal = $"{rangeDias.Item2.Day:00}";
                }
            }



            #endregion

            #region BLOCO CD

            foreach (var cdLine in entdados.BlocoCd.ToList())
            {
                Tuple<DateTime, DateTime> rangeDias = Tools.GetRangeInicialFinal(sabAnt, sabAnt, cdLine.DiaInic, cdLine.DiaFinal, dataEstudo);

                if (cdLine.DiaInic.Trim() != "I")
                {
                    cdLine.DiaInic = $"{rangeDias.Item1.Day:00}";
                }

                if (cdLine.DiaFinal.Trim() != "F")
                {
                    cdLine.DiaFinal = $"{rangeDias.Item2.Day:00}";
                }
                //cdLine.DiaInic = $"{dataEstudo.Day:00}";
            }

            #endregion

            #region BLOCO VE

            var blocoVEdadger = dadger.BlocoVe.ToList();
            foreach (var veLine in entdados.BlocoVe.ToList())
            {
                Tuple<DateTime, DateTime> rangeDias = Tools.GetRangeInicialFinal(dataBase, sabAnt, veLine.DiaInic, veLine.DiaFinal, dataEstudo);
                if (veLine.DiaInic.Trim() != "I")
                {
                    veLine.DiaInic = $"{rangeDias.Item1.Day:00}";
                }

                if (veLine.DiaFinal.Trim() != "F")
                {
                    veLine.DiaFinal = $"{rangeDias.Item2.Day:00}";
                }
                var veDadger = blocoVEdadger.Where(x => x.Usina == veLine.Usina).FirstOrDefault();
                if (veDadger != null)
                {
                    veLine.VolumeEspera = veDadger.VolumeEspera1;
                }

                //veLine.DiaInic = $"{dataEstudo.Day:00}";
                //veLine.DiaFinal = $"{dataEstudo.AddDays(1).Day:00}";
            }

            #endregion

            #region BLOCO DA

            foreach (var daLine in entdados.BlocoDa.ToList())
            {

                Tuple<DateTime, DateTime> rangeDias = Tools.GetRangeInicialFinal(sabAnt, sabAnt, daLine.DiaInic, daLine.DiaFinal, dataEstudo);

                if (daLine.DiaInic.Trim() != "I")
                {
                    daLine.DiaInic = $"{rangeDias.Item1.Day:00}";
                }

                if (daLine.DiaFinal.Trim() != "F")
                {
                    daLine.DiaFinal = $"{rangeDias.Item2.Day:00}";
                }
                //daLine.DiaInic = $"{dataEstudo.Day:00}";
            }

            #endregion
            #region BLOCO CICE

            foreach (var ciceLine in entdados.BlocoCice.ToList())
            {
                Tuple<DateTime, DateTime> rangeDias = Tools.GetRangeInicialFinal(dataBase, sabAnt, ciceLine.DiaInic, ciceLine.DiaFinal, dataEstudo);
                if (ciceLine.DiaInic.Trim() != "I")
                {
                    ciceLine.DiaInic = $"{rangeDias.Item1.Day:00}";
                }

                if (ciceLine.DiaFinal.Trim() != "F")
                {
                    ciceLine.DiaFinal = $"{rangeDias.Item2.Day:00}";
                }
            }

            #endregion

            #region BLOCO RHE

            
            foreach (var rhe in entdados.BlocoRhe.ToList())
            {
                Tuple<DateTime, DateTime> rangeDias = Tools.GetRangeInicialFinal(dataBase, sabAnt, rhe.DiaInic, rhe.DiaFinal, dataEstudo, eRestricao: true);

                if (rhe.DiaInic.Trim() != "I")
                {
                    rhe.DiaInic = $"{rangeDias.Item1.Day:00}";
                }

                if (rhe.DiaFinal.Trim() != "F")
                {
                    rhe.DiaFinal = $"{rangeDias.Item2.Day:00}";
                }
            }

            #endregion

            #region BLOCO R11

            foreach (var r11Line in entdados.BlocoR11.ToList())
            {
                Tuple<DateTime, DateTime> rangeDias = Tools.GetRangeInicialFinal(dataBase, sabAnt, r11Line.DiaInic, r11Line.DiaFinal, dataEstudo);


                if (r11Line.DiaInic.Trim() != "I")
                {
                    r11Line.DiaInic = $"{rangeDias.Item1.Day:00}";
                }

                if (r11Line.DiaFinal.Trim() != "F")
                {
                    r11Line.DiaFinal = $"{rangeDias.Item2.Day:00}";
                }

               // r11Line.DiaInic = $"{dataEstudo.Day:00}";
            }

            #endregion

            #region BLOCO MT

            foreach (var mtLine in entdados.BlocoMt.ToList())
            {
                Tuple<DateTime, DateTime> rangeDias = Tools.GetRangeInicialFinal(dataBase, sabAnt, mtLine.DiaInic, mtLine.DiaFinal, dataEstudo);

                if (mtLine.DiaInic.Trim() != "I")
                {
                    mtLine.DiaInic = $"{rangeDias.Item1.Day:00}";
                }

                if (mtLine.DiaFinal.Trim() != "F")
                {
                    mtLine.DiaFinal = $"{rangeDias.Item2.Day:00}";
                }
                //mtLine.DiaInic = $"{dataEstudo.Day:00}";
                //mtLine.DiaFinal = $"{dataEstudo.AddDays(1).Day:00}";
            }

            #endregion

            #region BLOCO MH

            foreach (var mhLine in entdados.BlocoMh.ToList())
            {
                if (!string.IsNullOrEmpty(mhLine.Comment))
                {
                    string commentMH = mhLine.Comment;

                    DateTime dataIni = Convert.ToDateTime(commentMH.Split(new string[] { "Ini:" }, StringSplitOptions.RemoveEmptyEntries).Last().Split('-').First().Trim(), Culture.DateTimeFormat);
                    DateTime dataFim = Convert.ToDateTime(commentMH.Split(new string[] { "Fim:" }, StringSplitOptions.RemoveEmptyEntries).Last().Split('-').First().Trim(), Culture.DateTimeFormat);

                    if (dataIni.Minute == 29 || dataIni.Minute == 59)// trata meias horas
                    {
                        dataIni = dataIni.AddMinutes(1);
                    }
                    if (dataFim.Minute == 29 || dataFim.Minute == 59)// trata meias horas
                    {
                        dataFim = dataFim.AddMinutes(1);
                    }
                    if (dataFim < dataEstudo || dataIni> dataEstudo.AddDays(7))
                    {
                        entdados.BlocoMh.Remove(mhLine);
                    }
                    else
                    {
                        DateTime novoInicio = dataIni;
                        DateTime novoFinal = dataFim;

                        if (dataIni <= dataEstudo)
                        {
                            novoInicio = dataEstudo;
                        }
                        if (dataFim >= dataEstudo.AddDays(7))
                        {
                            novoFinal = dataEstudo.AddDays(7);
                        }
                        mhLine.DiaInic = $"{novoInicio.Day:00}";
                        mhLine.HoraInic = novoInicio.Hour;
                        mhLine.MeiaHoraInic = novoInicio.Minute == 30 ? 1 : 0;

                        mhLine.DiaFinal = $"{novoFinal.Day:00}";
                        mhLine.HoraFinal = novoFinal.Hour;
                        mhLine.MeiaHoraFinal = novoFinal.Minute == 30 ? 1 : 0;
                    }
                }
            }

            #endregion

            #region BLOCO IT / AC

            var polinjusFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("polinjus.csv")).FirstOrDefault();
            if (polinjusFile != null && File.Exists(polinjusFile))
            {
                //var it = entdados.BlocoIt.ToList();
                entdados.BlocoIt.Clear();

               // var AC288_BeloMonte = entdados.BlocoAc.Where(x => x.Usina == 288 && x.Mnemonico.ToUpper() == "COTVAZ").ToList();
                entdados.BlocoAc.Where(x => x.Usina == 288 && x.Mnemonico.ToUpper() == "COTVAZ").ToList().ForEach(y => entdados.BlocoAc.Remove(y));//belo monte

               // var AC314_Pimental = entdados.BlocoAc.Where(x => x.Usina == 314 && x.Mnemonico.ToUpper() == "COTVAZ").ToList();
                entdados.BlocoAc.Where(x => x.Usina == 314 && x.Mnemonico.ToUpper() == "COTVAZ").ToList().ForEach(y => entdados.BlocoAc.Remove(y));// pimental

                
            }

            #endregion

            entdados.SaveToFile(createBackup: true);

        }

    }
}

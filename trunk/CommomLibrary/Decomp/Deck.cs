using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Decomp
{

    public enum DeckDocument
    {
        caso,
        dadger,
        dadgnl,
        hidr,
        loss,
        mlt,
        modif,
        postos,
        prevs,
        vazoes,
        vazoesc
    }

    public class Deck : BaseDeck
    {

        Dictionary<string, DeckFile> documents = new Dictionary<string, DeckFile> {
            {"CASO.DAT"   , null},
            {"DADGER."  , null},
            {"DADGNL."  , null},
            {"HIDR.DAT"   , null},
            {"LOSS.DAT"   , null},
            {"PERDAS.DAT"   , null},
            {"MLT.DAT"    , null},
            {"MODIF.DAT"  , null},
            {"POSTOS.DAT" , null},
            {"PREVS."   , null},
            {"VAZOES."  , null},
            {"VAZOES.DAT"  , null},
            {"VAZOESC.DAT", null},
            {"REGRAS.DAT", null},
            {"GEVAZP.DAT", null},
            {"BACIAS.DAT", null},
            {"ARQUIVOS.DAT", null},
            {"GEVAZP.CFG", null},
            {"POLINJUS.DAT", null},
            //
            {"hist-ventos.csv", null},
            {"indices.csv", null},
            {"indices_gevazp.csv", null},
            {"parque_eolico_cadastro.csv", null},
            {"parque_eolico_config.csv", null},
            {"parque_eolico_fte.csv", null},
            {"parque_eolico_geracao.csv", null},
            {"parque_eolico_pot_instalada.csv", null},
            {"parque_eolico_subm.csv", null},
            {"pee-posto.csv", null},
            {"posto-cadastro.csv", null},
            {"prevsvel.", null},

        };

        public override Dictionary<string, DeckFile> Documents { get { return documents; } }

        public DeckFile this[DeckDocument doc]
        {

            get
            {
                switch (doc)
                {
                    case DeckDocument.caso:
                        return Documents["CASO.DAT"];
                    case DeckDocument.dadger:
                        return Documents["DADGER."];
                    case DeckDocument.dadgnl:
                        return Documents["DADGNL."];
                    case DeckDocument.hidr:
                        return Documents["HIDR.DAT"];
                    case DeckDocument.loss:
                        if (Documents["PERDAS.DAT"] != null) return Documents["PERDAS.DAT"];
                        else return Documents["LOSS.DAT"];
                    case DeckDocument.mlt:
                        return Documents["MLT.DAT"];
                    case DeckDocument.modif:
                        return Documents["MODIF.DAT"];
                    case DeckDocument.postos:
                        return Documents["POSTOS.DAT"];
                    case DeckDocument.prevs:
                        return Documents["PREVS."];
                    case DeckDocument.vazoes:
                        return Documents["VAZOES."];
                    case DeckDocument.vazoesc:
                        return Documents["VAZOES.DAT"] ?? Documents["VAZOESC.DAT"];
                    default:
                        return null;
                }
            }
            set
            {
                switch (doc)
                {
                    case DeckDocument.caso:
                        documents["CASO.DAT"] = value;
                        break;
                    case DeckDocument.dadger:
                        documents["DADGER."] = value;
                        break;
                    case DeckDocument.dadgnl:
                        documents["DADGNL."] = value;
                        break;
                    case DeckDocument.hidr:
                        documents["HIDR.DAT"] = value;
                        break;
                    case DeckDocument.loss:
                        documents["LOSS.DAT"] = value;
                        break;
                    case DeckDocument.mlt:
                        documents["MLT.DAT"] = value;
                        break;
                    case DeckDocument.modif:
                        documents["MODIF.DAT"] = value;
                        break;
                    case DeckDocument.postos:
                        documents["POSTOS.DAT"] = value;
                        break;
                    case DeckDocument.prevs:
                        documents["PREVS."] = value;
                        break;
                    case DeckDocument.vazoes:
                        documents["VAZOES."] = value;
                        break;
                    case DeckDocument.vazoesc:
                        documents["VAZOESC.DAT"] = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public string Folder { get; set; }
        public int Rev { get; set; }

        public string caso;
        public string Caso { get { return caso; } set { caso = value; } }

        public override void GetFiles(string baseFolder)
        {

            BaseFolder = baseFolder;

            var folderFiles = System.IO.Directory.GetFiles(baseFolder).Where(x => !x.EndsWith(".bak", StringComparison.OrdinalIgnoreCase));


            var q = from doc in documents
                    from file in folderFiles
                    let filename = System.IO.Path.GetFileName(file)
                    where (doc.Key.EndsWith(".") && filename.StartsWith(doc.Key, StringComparison.OrdinalIgnoreCase))
                    || (filename.Equals(doc.Key, StringComparison.OrdinalIgnoreCase))
                    select new { doc.Key, file };

            if (q.Any(x => x.Key == "CASO.DAT"))
            {
                var f = q.Where(x => x.Key == "CASO.DAT").First().file;
                documents["CASO.DAT"] = new DeckFile(f);
                GetCaso(f);
            }
            else
                throw new FileNotFoundException("CASO.DAT não encontrado no deck");


            q.Where(x => x.Key != "CASO.DAT").ToList().ForEach(x =>
            {

                if (!string.IsNullOrWhiteSpace(Caso) && x.Key.EndsWith("."))
                {
                    if (x.file.EndsWith(Caso, StringComparison.OrdinalIgnoreCase))
                        documents[x.Key] = new DeckFile(x.file);
                }
                else
                    documents[x.Key] = new DeckFile(x.file);

            });

            var casoFile = folderFiles.Where(x => System.IO.Path.GetFileName(x)
                .Equals(Caso, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (casoFile == null) throw new FileNotFoundException(Caso + " não encontrado");

            if (!documents.ContainsKey(Caso))
            {
                documents.Add(Caso, new DeckFile(casoFile));
            }

        }

        private void GetCaso(string file)
        {
            Caso = File.ReadAllLines(file)[0].Trim();
        }

        public void CopyFilesToFolder(string folder, int rev)
        {
            Folder = folder;

            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }

            foreach (var doc in documents.Where(x => x.Value != null))
            {
                doc.Value.Folder = folder;

                var destFile = doc.Value.Path;

                if (System.IO.Path.GetFileName(destFile).ToLower() == Caso.ToLower())
                {
                    var cnt = System.IO.File.ReadAllText(doc.Value.BasePath).ToLowerInvariant();
                    cnt = cnt.Replace("." + Caso.ToLower(), ".rv" + rev.ToString());
                    System.IO.File.WriteAllText(
                        System.IO.Path.Combine(folder, "rv" + rev.ToString()),
                        cnt);
                    continue;
                }
                else if (destFile.EndsWith(Caso, StringComparison.OrdinalIgnoreCase))
                {
                    destFile = destFile.ToLower().Replace("." + Caso, ".rv" + rev.ToString());
                }
                else if (destFile.EndsWith("caso.dat", StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.WriteAllText(destFile, "rv" + rev.ToString());
                    continue;
                }

                System.IO.File.Copy(doc.Value.BasePath, destFile, true);
            }

        }

        public override void CopyFilesToFolder(string folder)
        {
            Folder = folder;

            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }

            foreach (var doc in documents.Where(x => x.Value != null))
            {
                doc.Value.Folder = folder;
                System.IO.File.Copy(doc.Value.BasePath, doc.Value.Path, true);
            }

        }

        Result result = null;

        public static double[] PLD_LimitesAlter()
        {
            try
            {
                string arqConfig = @"H:\TI - Sistemas\UAT\PricingExcelTools\files\Config_PLD_Alternativo.csv";

                //StreamReader rd = new StreamReader(@"Z:\shared\linuxQueue\Config_PLD.csv");
                double[] pld = new double[2];


                var dados = File.ReadAllLines(arqConfig).Skip(1).First().Replace('.', ',').Split(';').ToArray();
                pld[0] = Convert.ToDouble(dados[0]);
                pld[1] = Convert.ToDouble(dados[1]);


                return pld;

            }
            catch
            {
                return null;

            }

        }

        public static int[] Dias_Semanas(int mes, int ano) // Returna quantidade de dias para cada semana do mes OBS: Semana de Sabado a Sexta
        {
            int[] semana_dias = new int[7];
            var semana = 1;
            var dias = 1;
            var dias_mes = DateTime.DaysInMonth(ano, mes);

            for (var dia = 1; dia < dias_mes; dia++)
            {
                DateTime data = new DateTime(ano, mes, dia);

                if (data.DayOfWeek == DayOfWeek.Friday)
                {
                    semana_dias[semana] = dias;
                    dias = 0;
                    semana++;
                }
                dias++;
            }

            semana_dias[semana] = dias;

            return semana_dias;
        }

        public string getPldAlter(string dir)
        {
            List<Resu_PLD_Mensal> Resu = new List<Resu_PLD_Mensal>();

            double PLD = 0;
            double Soma_CMO = 0;
            double Soma_Horas = 0;

            string tipo = null;

            //QueueController ctl = new QueueController();

            //var comms = ctl.ReadComms();

            //var l = comms.Where(x => x.CommandName == name).FirstOrDefault();

            // if (l == null)
            // {
            //     comms = ctl.ReadComms(-30);
            //     l = comms.Where(x => x.CommandName == name).FirstOrDefault();
            // }


            //if (l != null)
            if (Directory.Exists(dir))
            {
                try
                {


                    //////////////////////////////////////
                    var caso = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("caso.dat")).First();


                    var rv = File.ReadAllLines(caso);

                    var dadger_file = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("dadger." + rv[0])).First();

                    if (dadger_file.Count() > 0)
                    {
                        var dadger = File.ReadAllLines(dadger_file);
                        DateTime dt_estudo = DateTime.Today;



                        foreach (var linha in dadger)
                        {
                            if (linha.StartsWith("DT"))
                            {
                                var dados = linha.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                dt_estudo = new DateTime(int.Parse(dados[3]), int.Parse(dados[2]), int.Parse(dados[1]));
                            }
                            else if (linha.StartsWith("& NO. SEMANAS"))
                            {
                                var dados = linha.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                if (int.Parse(dados[8]) != 0)
                                {
                                    tipo = "semanal";
                                    dt_estudo = dt_estudo.AddDays(8);
                                }
                                else
                                {
                                    tipo = "mensal";
                                }
                            }
                        }



                        var mes = dt_estudo.Month;
                        var ano = dt_estudo.Year;

                        var pld_lim = PLD_LimitesAlter();

                        double PLD_limite = pld_lim[0];
                        double PLD_Max = pld_lim[1];

                        var dec_oper = Directory.GetFiles(dir, "dec_oper_sist.csv");
                        bool modelonovo = verificaDec_OperVersion(dec_oper[0]);

                        double cmo = 0;
                        if (dec_oper.Length > 0)
                        {
                            var infos = File.ReadAllLines(dec_oper[0]);
                            foreach (var line in infos)
                            {
                                int semana = 0;

                                var dados = line.Split(';');

                                if ((dados.Count() > 20) && (int.TryParse(dados[0].Trim(), out semana)))
                                {
                                    if ((dados[0].Trim() == dados[1].Trim()) && (dados[modelonovo ? 5 : 4].Trim() != "11"))
                                    {
                                        if (dados[modelonovo ? 4 : 3].Trim() != "-")
                                        {
                                            cmo = Convert.ToDouble(dados[modelonovo ? 25 : 23].Trim().Replace('.', ',').ToString());
                                            var horas = Convert.ToDouble(dados[modelonovo ? 4 : 3].Trim().Replace('.', ',').ToString());
                                            if (cmo > PLD_limite)
                                            {
                                                if (cmo > PLD_Max)
                                                {
                                                    Soma_CMO = Soma_CMO + horas * PLD_Max;
                                                }
                                                else
                                                {
                                                    Soma_CMO = Soma_CMO + horas * cmo;
                                                }
                                            }
                                            else
                                            {
                                                Soma_CMO = Soma_CMO + horas * PLD_limite;
                                            }
                                            Soma_Horas = Soma_Horas + horas;
                                        }
                                        else
                                        {
                                            cmo = Convert.ToDouble(dados[modelonovo ? 25 : 23].Trim().Replace('.', ',').ToString());
                                            PLD = Soma_CMO / Soma_Horas;
                                            double Pld_Mensal = 0;
                                            int dias_Semana_Atual = 0;
                                            if (tipo == "mensal" && semana == 1)
                                            {
                                                Pld_Mensal = PLD;
                                            }
                                            else if (tipo == "mensal")
                                            {
                                                Pld_Mensal = 0;
                                            }
                                            else
                                            {
                                                int[] dias_semana = Dias_Semanas(mes, ano);
                                                try
                                                {
                                                    dias_Semana_Atual = dias_semana[Convert.ToInt32(dados[1].Trim())];
                                                }
                                                catch
                                                {

                                                }
                                                var dias_mes = DateTime.DaysInMonth(ano, mes);

                                                Pld_Mensal = (PLD * dias_Semana_Atual) / dias_mes;
                                            }
                                            object[,] Conjunto_Dados = new object[1, 7] {
                                        {
                                            semana,
                                            dados[modelonovo ? 5 : 4].Trim(),
                                            cmo,
                                            PLD,
                                            dt_estudo,
                                            tipo,
                                            Pld_Mensal

                                        }
                                    };

                                            Resu.Add(new Resu_PLD_Mensal(Conjunto_Dados));

                                            PLD = 0;
                                            Soma_CMO = 0;
                                            Soma_Horas = 0;

                                        }



                                    }
                                }


                            }

                        }
                        string json = "";

                        var arq = Path.Combine(dir, "PLD_Mensal_alternativo.csv");
                        //  if (Auto)
                        if (File.Exists(arq))
                        {
                            File.Delete(arq);
                        }
                        using (TextWriter tw = new StreamWriter(arq, false, Encoding.Default))
                        {
                            tw.WriteLine(dir + "\n");
                            tw.WriteLine("Semana;Submercado;CMO;PLD;Mes;Tipo;PLD_Mensal");
                            foreach (var dado in Resu)
                            {
                                //tw.WriteLine(dado.Semana + ";" + dado.Submercado + ";" + dado.CMO + ";" + dado.PLD + ";" + dado.Mes + ";" + dado.Tipo + ";" + dado.PLD_Mensal); //escreve no arquivo novamente
                                tw.WriteLine(dado.Semana + ";" + dado.Submercado + ";" + dado.CMO.ToString().Replace(',', '.') + ";" + dado.PLD.ToString().Replace(',', '.') + ";" + dado.Mes + ";" + dado.Tipo + ";" + dado.PLD_Mensal.ToString().Replace(',', '.')); //escreve no arquivo novamente
                            }

                            tw.Close();
                        }



                        return json;
                    }
                    else
                    {
                        Console.Write("Dadger não encontrado");
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.Write("Erro ao Calcular PLD Mensal");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static bool verificaDec_OperVersion(string dec_Oper)
        {
            if (File.Exists(dec_Oper))
            {
                var infos = File.ReadAllLines(dec_Oper);
                if (infos.Any(x => x.ToUpper().StartsWith("GEOL")))
                {
                    return true;
                }
            }
            return false;
        }

        public override Result GetResults(bool alternativo = false)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            if (result != null) return result;

            try
            {

                var dadger = this[DeckDocument.dadger].Document as Dadger.Dadger;
                var dadgnl = this[DeckDocument.dadgnl].Document as Dadgnl.Dadgnl;
                var numEstagios = dadger.VAZOES_NumeroDeSemanas;
                var dias2mes = dadger.VAZOES_NumeroDiasDoMes2;

                var Bloco_GL = dadgnl.BlocoGL.ToList();

                var dec_oper = Path.Combine(this.BaseFolder, "dec_oper_sist.csv");

                bool modeloNovo = verificaDec_OperVersion(dec_oper);

                var PLD_mensal = Path.Combine(this.BaseFolder, "PLD_Mensal.csv");
                var PLD_mensalAlternativo = Path.Combine(this.BaseFolder, "PLD_Mensal_alternativo.csv");

                List<string[]> resu_PLD = new List<string[]>();
                if (!File.Exists(PLD_mensal))
                {


                    System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

                    //var responseTsk = httpClient.PostAsync("http://ec2-44-201-188-49.compute-1.amazonaws.com:5015/api/Command", cont);
                    //var responseTsk = httpClient.PostAsync("http://10.206.194.196:5015/api/Command", cont);
                    // responseTsk.Wait();
                    // var response = responseTsk.Result;


                    // System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();
                    try
                    {
                        string caminho = this.BaseFolder.Replace("K:\\", "/home/producao/PrevisaoPLD/").Replace("\\", "/");
                        string arq = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string com = $"result_{ arq}";
                        File.WriteAllText($@"K:\enercore_ctl_common\{com}", caminho);

                        var response = httpClient.GetAsync(@"http://10.206.194.196:5015/api/" + "Command/PLD_Local-" + com);
                        response.Wait();

                        File.Delete($@"K:\enercore_ctl_common\{com}");
                    }
                    catch
                    {


                    }
                }

                if (alternativo)
                {
                    if (File.Exists(PLD_mensalAlternativo))
                    {
                        File.Delete(PLD_mensalAlternativo);
                    }
                    getPldAlter(this.BaseFolder);

                    if (File.Exists(PLD_mensalAlternativo))
                    {
                        var infos = File.ReadAllLines(PLD_mensalAlternativo);

                        foreach (var l in infos)
                        {
                            var ls = l.Split(';').Select(x => x.Trim()).ToArray();
                            if (ls.Length > 2)
                            {
                                resu_PLD.Add(ls);
                            }
                        }


                    }
                }
                else if (File.Exists(PLD_mensal))
                {
                    var infos = File.ReadAllLines(PLD_mensal);

                    foreach (var l in infos)
                    {
                        var ls = l.Split(';').Select(x => x.Trim()).ToArray();
                        if (ls.Length > 2)
                        {
                            resu_PLD.Add(ls);
                        }
                    }


                }

                List<Result.CMO_Mensal> Resu_CMO = new List<Result.CMO_Mensal>();
                foreach (var line in resu_PLD)
                {
                    int semana;
                    if (int.TryParse(line[0].Trim(), out semana))
                    {
                        object[,] Conjunto_Dados = new object[1, 3] {
                                        {
                                            line[0],
                                            line[1],
                                            line[6].Replace(".",","),


                                        }
                                    };

                        //Resu.Add(new Resu_PLD_Mensal(Conjunto_Dados));
                        Resu_CMO.Add(new Result.CMO_Mensal(Conjunto_Dados));
                    }

                }





                if (!File.Exists(dec_oper)) return null;

                result = new Result(this.BaseFolder) { Tipo = "DC" };

                result.CMO_Mensal_Result = Resu_CMO;


                //List<string> datalines = new List<string>();
                List<string[]> datalines = new List<string[]>();
                using (var sr = File.OpenText(dec_oper))
                {

                    while (sr.ReadLine() != "@Tabela") { }
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    string l;
                    do
                    {
                        l = sr.ReadLine();
                        var ls = l.Split(';').Select(x => x.Trim()).ToArray();
                        if (ls.Length > 10 && ls[0] == ls[1]) datalines.Add(ls);
                        else break;
                    } while (!sr.EndOfStream);
                }

                //Dictionary<string, string[,]> resumos = new Dictionary<string, string[,]>();
                var resumofile = Path.Combine(this.BaseFolder, "resumo.log");
                var casodatfile = Path.Combine(this.BaseFolder, "caso.dat");


                var files = Directory.GetFiles(BaseFolder).ToList();

                var relatoPath = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).Equals("relato"));
                int mesEstudo = -1;
                if (relatoPath != null)
                {
                    var relato = (Compass.CommomLibrary.Relato.Relato)DocumentFactory.Create(relatoPath);
                    foreach (var th in relato.THMensal) result[(int)th[1]].EnaTH = th[3];
                    relato.EnergiaAcoplamentoSistema.Where(ena => ena[1] == 1).Select(ena =>
                    {

                        result[Enum.Parse(typeof(SistemaEnum), ena[0])].EnaSemCV = ena[2];

                        if (dadger.VAZOES_NumeroDeSemanas == 0)
                        {
                            result[Enum.Parse(typeof(SistemaEnum), ena[0])].Ena = ena[2];
                        }
                        else
                        { // calcular ena fechamento do mês.
                            result[Enum.Parse(typeof(SistemaEnum), ena[0])].Ena = ena[2];
                        }

                        return true;

                    }).ToList();

                    mesEstudo = relato.PeriodoInicio.Month - 1;
                    //foreach (var ena in )) result[(int)ena[1]].Ena = ena[3];
                }

                if (File.Exists(resumofile) && File.Exists(casodatfile))
                {
                    var caso = File.ReadAllLines(casodatfile);
                    int cvIdx;
                    if (int.TryParse(caso[0].Substring(2, 1), out cvIdx))
                    {

                        var r = File.ReadAllLines(resumofile);

                        if (r.Length == 5)
                        {
                            var se = r[1].Split(' ');
                            var s = r[2].Split(' ');
                            var ne = r[3].Split(' ');
                            var n = r[4].Split(' ');

                            result[1].Ena = float.Parse(se[6]);
                            result[2].Ena = float.Parse(s[6]);
                            result[3].Ena = float.Parse(ne[6]);
                            result[4].Ena = float.Parse(n[6]);

                            result[1].EnaMLT = float.Parse(se[7].Replace("%", "")) / 100;
                            result[2].EnaMLT = float.Parse(s[7].Replace("%", "")) / 100;
                            result[3].EnaMLT = float.Parse(ne[7].Replace("%", "")) / 100;
                            result[4].EnaMLT = float.Parse(n[7].Replace("%", "")) / 100;

                            result[1].EnaSemCV = float.Parse(se[cvIdx]);
                            result[2].EnaSemCV = float.Parse(s[cvIdx]);
                            result[3].EnaSemCV = float.Parse(ne[cvIdx]);
                            result[4].EnaSemCV = float.Parse(n[cvIdx]);
                        }
                    }
                }

                var cortesPath = (this[DeckDocument.dadger].Document as Dadger.Dadger).CortesPath;
                result.Cortes = System.IO.Path.GetDirectoryName(cortesPath);



                if (BaseDeck.EnaMLT == null || BaseDeck.EnaMLT[SistemaEnum.SE][mesEstudo] == 0)
                {
                    var deck = DeckFactory.CreateDeck(result.Cortes);
                    if (deck is Newave.Deck) DeckFactory.CreateDeck(result.Cortes).GetResults();
                }


                datalines
                    .Where(x => x[0] == "1")
                    .GroupBy(x => int.Parse(x[modeloNovo ? 5 : 4]))
                    .Where(x => x.Key < 5).ToList()
                    .ForEach(x =>
                    {

                        double cmo1, cmo2, cmo3, cmo, earmI, earmf;

                        double.TryParse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "1")[modeloNovo ? 25 : 23].Trim(),
                            System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo,
                            out cmo1);
                        double.TryParse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "2")[modeloNovo ? 25 : 23],
                            System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo,
                            out cmo2);
                        double.TryParse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "3")[modeloNovo ? 25 : 23],
                            System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo,
                            out cmo3);
                        double.TryParse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 25 : 23],
                            System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo,
                            out cmo);
                        double.TryParse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 22 : 20],
                            System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo,
                            out earmI);
                        double.TryParse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 24 : 22],
                            System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo,
                            out earmf);
                        //double.TryParse(x.First(y => y[2].Trim() == "-")[18].Trim(),
                        //    System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo,
                        //    out enaCV);
                        result[x.Key].DemandaPrimeiroEstagio = double.Parse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 7 : 6],
                            System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);


                        if (x.Key == 1)
                        {
                            //som 1.900MW (IT50Hz) e geracao 60Hz                        
                            result[x.Key].GerHidr = double.Parse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 11 : 10],
                                      System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo)
                                      + double.Parse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 18 : 16].Replace("-", "0").Trim(),
                                      System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo)
                                      + 1900d;
                        }
                        else
                        {
                            result[x.Key].GerHidr = double.Parse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 11 : 10],
                                      System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
                        }

                        //Ter + TerAt
                        result[x.Key].GerTerm = double.Parse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 9 : 8],
                         System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo)
                          + double.Parse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 10 : 9].Replace("-", "0").Trim(),
                              System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);

                        result[x.Key].GerPeq = double.Parse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[modeloNovo ? 8 : 7],
                             System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);


                        result[x.Key].EarmI = earmI / 100;
                        result[x.Key].EarmF = earmf / 100;
                        result[x.Key].Cmo = cmo;
                        result[x.Key].Cmo_pat1 = cmo1;
                        result[x.Key].Cmo_pat2 = cmo2;
                        result[x.Key].Cmo_pat3 = cmo3;

                        if (BaseDeck.EnaMLT != null)
                        {

                            result[x.Key].EnaMLT = result[x.Key].Ena / BaseDeck.EnaMLT[(SistemaEnum)x.Key][mesEstudo];
                            result[x.Key].EnaTHMLT = result[x.Key].EnaTH / BaseDeck.EnaMLT[(SistemaEnum)x.Key][mesEstudo == 0 ? 11 : mesEstudo - 1];

                        }

                        if (modeloNovo)
                        {
                            result[x.Key].GerEol = double.Parse(x.First(y => y[modeloNovo ? 3 : 2].Trim() == "-")[12],
                                      System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
                        }
                        else
                        {
                            result[x.Key].GerEol = 0.00f;
                        }
                    });
                //int maxEstagios = datalines.Select(x => int.Parse(x[0])).Max() - 1;
                //List<Tuple<int, double>> gerMedia = new List<Tuple<int, double>>(0);

                //datalines
                //    .Where(x => int.Parse(x[0]) <= maxEstagios)
                //    .GroupBy(x => int.Parse(x[4]))
                //    .Where(x => x.Key < 5).ToList()
                //    .ForEach(x =>
                //    {
                //        foreach (var l in x)
                //        {
                //            double ger = 0;
                //            if (l[2].Trim() == "-")
                //            {
                //                if (x.Key == 1)
                //                {

                //                    //som 1.900MW (IT50Hz) e geracao 60Hz                        
                //                    ger = double.Parse(l[10],
                //                              System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo)
                //                              + double.Parse(l[16].Replace("-", "0").Trim(),
                //                              System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo)
                //                              + 1900d;
                //                    gerMedia.Add(new Tuple<int, double>(x.Key, ger));
                //                }
                //                else
                //                {
                //                    ger = double.Parse(l[10],
                //                              System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
                //                    gerMedia.Add(new Tuple<int, double>(x.Key, ger));

                //                }
                //            }

                //        }

                //    });

                //for (int sist = 1; sist <= 4; sist++)
                //{
                //    if (gerMedia.Any(x => x.Item1 == sist))
                //    {
                //        result[sist].GerHidrMedia = gerMedia.Where(x => x.Item1 == sist).Select(x => x.Item2).Average();
                //    }
                //}

                if (numEstagios == 0)
                {
                    for (int sis = 1; sis <= 4; sis++) result[sis].GerHidrMedia = result[sis].GerHidr;

                    for (int sis = 1; sis <= 4; sis++) result[sis].GerTermMedia = result[sis].GerTerm;

                    for (int sis = 1; sis <= 4; sis++) result[sis].DemandaMes = result[sis].DemandaPrimeiroEstagio;

                    for (int sis = 1; sis <= 4; sis++) result[sis].GerEolMedia = result[sis].GerEol;

                    datalines
                            .Where(x => x[modeloNovo ? 3 : 2] == "-")
                            .Where(x => x[0] == "2")
                            .GroupBy(x => int.Parse(x[modeloNovo ? 5 : 4]))
                            .Where(x => x.Key < 5).ToList().ForEach(x =>
                            {
                                result[x.Key].DemandaMesSeguinte = double.Parse(x.First()[modeloNovo ? 7 : 6],
                                    System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);

                            });

                }
                else
                {


                    datalines
                      .Where(x => x[modeloNovo ? 3 : 2] == "-")
                      .Where(x => x[0] != (numEstagios + 1).ToString())
                      .GroupBy(x => int.Parse(x[modeloNovo ? 5 : 4]))
                      .Where(x => x.Key < 5).ToList().ForEach(x =>
                      {
                          if (modeloNovo)
                          {
                              int totaldias = 0;
                              result[x.Key].GerEolMedia =
                                  x.Sum(y =>
                                  {

                                      int peso = (
                                          y[0] == "1" ? (dadger.DataEstudo.Month != dadger.VAZOES_MesInicialDoEstudo ? dadger.DataEstudo.AddDays(6).Day : 7)
                                          : (
                                                   y[0] == numEstagios.ToString() ? 7 - dias2mes : 7
                                               )
                                          );
                                      totaldias += peso;
                                      return

                                          double.Parse(y[12],
                                          System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo)
                                          * peso;
                                  }) / (totaldias);
                          }
                          else
                          {
                              result[x.Key].GerEolMedia = 0.00f;
                          }

                      });


                    datalines
                           .Where(x => x[modeloNovo ? 3 : 2] == "-")
                           .Where(x => x[0] != (numEstagios + 1).ToString())
                           .GroupBy(x => int.Parse(x[modeloNovo ? 5 : 4]))
                           .Where(x => x.Key < 5).ToList().ForEach(x =>
                           {
                               int totaldias = 0;
                               result[x.Key].GerHidrMedia =
                                   x.Sum(y =>
                                   {

                                       int peso = (
                                           y[0] == "1" ? (dadger.DataEstudo.Month != dadger.VAZOES_MesInicialDoEstudo ? dadger.DataEstudo.AddDays(6).Day : 7)
                                           : (
                                                    y[0] == numEstagios.ToString() ? 7 - dias2mes : 7
                                                )
                                           );
                                       totaldias += peso;
                                       if (x.Key == 1)
                                       {
                                           return

                                          (double.Parse(y[modeloNovo ? 11 : 10],
                                          System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo) + double.Parse(y[modeloNovo ? 18 : 16].Replace("-", "0").Trim(),
                                              System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo) + 1900d)
                                          * peso;
                                       }
                                       else
                                       {
                                           return

                                           double.Parse(y[modeloNovo ? 11 : 10],
                                           System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo)
                                           * peso;
                                       }

                                   }) / (totaldias);
                           });

                    datalines
                           .Where(x => x[modeloNovo ? 3 : 2] == "-")
                           .Where(x => x[0] != (numEstagios + 1).ToString())
                           .GroupBy(x => int.Parse(x[modeloNovo ? 5 : 4]))
                           .Where(x => x.Key < 5).ToList().ForEach(x =>
                           {
                               int totaldias = 0;
                               result[x.Key].GerTermMedia =
                                   x.Sum(y =>
                                   {

                                       int peso = (
                                           y[0] == "1" ? (dadger.DataEstudo.Month != dadger.VAZOES_MesInicialDoEstudo ? dadger.DataEstudo.AddDays(6).Day : 7)
                                           : (
                                                    y[0] == numEstagios.ToString() ? 7 - dias2mes : 7
                                                )
                                           );
                                       totaldias += peso;

                                       return

                                      (double.Parse(y[modeloNovo ? 9 : 8],
                                      System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo) + double.Parse(y[modeloNovo ? 10 : 9].Replace("-", "0").Trim(),
                                          System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo))
                                      * peso;



                                   }) / (totaldias);
                           });


                    datalines
                           .Where(x => x[modeloNovo ? 3 : 2] == "-")
                           .Where(x => x[0] != (numEstagios + 1).ToString())
                           .GroupBy(x => int.Parse(x[modeloNovo ? 5 : 4]))
                           .Where(x => x.Key < 5).ToList().ForEach(x =>
                           {
                               int totaldias = 0;
                               result[x.Key].DemandaMes =
                                   x.Sum(y =>
                                   {

                                       int peso = (
                                           y[0] == "1" ? (dadger.DataEstudo.Month != dadger.VAZOES_MesInicialDoEstudo ? dadger.DataEstudo.AddDays(6).Day : 7)
                                           : (
                                                    y[0] == numEstagios.ToString() ? 7 - dias2mes : 7
                                                )
                                           );
                                       totaldias += peso;
                                       return

                                           double.Parse(y[modeloNovo ? 7 : 6],
                                           System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo)
                                           * peso;
                                   }) / (totaldias);
                           });

                    datalines
                          .Where(x => x[modeloNovo ? 3 : 2] == "-")
                          .Where(x => x[0] == (numEstagios + 1).ToString())
                          .GroupBy(x => int.Parse(x[modeloNovo ? 5 : 4]))
                          .Where(x => x.Key < 5).ToList().ForEach(x =>
                          {
                              result[x.Key].DemandaMesSeguinte = double.Parse(x.First()[modeloNovo ? 7 : 6],
                                            System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);

                          });


                }
                List<Result.GNLResult> Resu_GNL = new List<Result.GNLResult>();
                foreach (var line in Bloco_GL)
                {

                    object[,] Conjunto_Dados = new object[1, 6] {
                                        {
                                            line.NumeroUsina,
                                            line.Subsistema,
                                            line.Semana,
                                            line.GeracaoPat1,
                                            line.GeracaoPat2,
                                            line.GeracaoPat3,

                                        }
                                    };

                    //Resu.Add(new Resu_PLD_Mensal(Conjunto_Dados));
                    Resu_GNL.Add(new Result.GNLResult(Conjunto_Dados));


                }
                result.novo = modeloNovo;
                result.GNL_Result = Resu_GNL;


            }
            catch (Exception erro) { }
            return result;

        }

    }
}

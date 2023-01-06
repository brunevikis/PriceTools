using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dessem
{
    public enum DeckDocument
    {
        operut,
        areacont,
        cortdeco,
        cotasr11,
        curvtviag,
        dadvaz,
        deflant,
        dessem,
        entdados,
        hidr,
        ils_tri,
        infofcf,
        mapcut,
        mlt,
        operuh,
        ptoper,
        rampas,
        renovaveis,
        respot,
        respotele,
        restseg,
        rstlpp,
        termdat



    }
    public class Deck : BaseDeck
    {


        Dictionary<string, DeckFile> documents = new Dictionary<string, DeckFile> {
            {"operut.dat"   , null},
            {"areacont.dat", null },
            {"cortdeco.", null },
            {"cotasr11.dat", null },
            {"curvtviag.dat", null },
            {"dadvaz.dat", null },
            {"deflant.dat", null },
            {"dessem.arq", null },
            {"entdados.dat", null },
            {"hidr.dat", null },
            {"ils_tri.dat", null },
            {"infofcf.dat", null },
            {"mapcut.", null },
            {"mlt.dat", null },
            {"operuh.dat", null },
            {"ptoper.dat", null },
            {"rampas.dat", null },
            {"renovaveis.dat", null },
            {"respot.dat", null },
            {"respotele.dat", null },
            {"restseg.dat", null },
            {"rstlpp.dat", null },
            {"termdat.dat", null }



        };
        public override Dictionary<string, DeckFile> Documents { get { return documents; } }

        public DeckFile this[DeckDocument doc]
        {

            get
            {
                switch (doc)
                {
                    case DeckDocument.operut:
                        return Documents["operut.dat"];
                    case DeckDocument.entdados:
                        return Documents["entdados.dat"];
                    case DeckDocument.ptoper:
                        return Documents["ptoper.dat"];
                    case DeckDocument.dessem:
                        return Documents["dessem.arq"];
                    case DeckDocument.renovaveis:
                        return Documents["renovaveis.dat"];
                    case DeckDocument.dadvaz:
                        return Documents["dadvaz.dat"];
                    default:
                        return null;
                }
            }
            set
            {
                switch (doc)
                {
                    case DeckDocument.operut:
                        documents["operut.dat"] = value;
                        break;
                    case DeckDocument.ptoper:
                        documents["ptoper.dat"] = value;
                        break;
                    case DeckDocument.dessem:
                        documents["dessem.arq"] = value;
                        break;
                    case DeckDocument.renovaveis:
                        documents["renovaveis.dat"] = value;
                        break;
                    case DeckDocument.entdados:
                        documents["entdados.dat"] = value;
                        break;
                    case DeckDocument.dadvaz:
                        documents["dadvaz.dat"] = value;
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

            foreach (var item in q.ToList())
            {
                documents[item.Key] = new DeckFile(item.file);
            }
        }
        Result result = null;
        public override Result GetResults(bool alternativo = false)
        {

            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            if (result != null) return result;

            try
            {


                var pdo_sist = Directory.GetFiles(this.BaseFolder).Where(x => Path.GetFileName(x).ToUpper().Contains("PDO_SIST.DAT")).First();

                //var PLD_mensal = Path.Combine(this.BaseFolder, "PLD_Mensal.csv");

                List<string[]> resu_sist = new List<string[]>();

                if (File.Exists(pdo_sist))
                {
                    var infos = File.ReadAllLines(pdo_sist).ToList();

                    string linhadata = infos.Where(x => x.Contains("Data do Caso")).First();

                    DateTime dataPdo = Convert.ToDateTime(linhadata.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Last(),Culture.DateTimeFormat);

                    Compass.CommomLibrary.IPDOEntitiesPLDLimites PLD_Ctx = new IPDOEntitiesPLDLimites();

                    var limites = PLD_Ctx.PLD_LIMITES.Where(x => x.Ano == dataPdo.Year).First();


                    int index = infos.IndexOf(infos.Where(x => x != "" && x.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).First().Trim() == "IPER").First()) + 2;
                    for (int i = index; i < infos.Count(); i++)
                    {
                        var ls = infos[i].Split(';').Select(x => x.Trim()).ToArray();
                        if (ls.Length > 19)
                        {
                            if (Convert.ToInt32(ls[0]) <= 48 && ls[2].Trim() != "FC")
                            {
                                resu_sist.Add(ls);
                            }
                        }
                    }

                    //foreach (var l in infos)
                    //{
                    //    var ls = l.Split(';').Select(x => x.Trim()).ToArray();
                    //    if (ls.Length > 19)
                    //    {
                    //        resu_PLD.Add(ls);
                    //    }
                    //}




                    List<Result.PDO_Sist> Resu_Sist = new List<Result.PDO_Sist>();
                    foreach (var line in resu_sist)
                    {
                        int estagio;
                        if (int.TryParse(line[0].Trim(), out estagio))
                        {
                            object[,] Conjunto_Dados = new object[1, 14] {
                                        {
                                            line[0],
                                            line[2],
                                            line[3].Replace(".",","),
                                            line[4].Replace(".",","),
                                            line[8].Replace(".",","),
                                            line[9].Replace(".",","),
                                            line[10].Replace(".",","),
                                            line[11].Replace(".",","),
                                            line[12].Replace(".",","),
                                            line[13].Replace(".",","),
                                            line[15].Replace(".",","),
                                            line[17].Replace(".",","),
                                            line[18].Replace(".",","),
                                            line[19].Replace(".",","),


                                        }
                                    };

                            //Resu.Add(new Resu_PLD_Mensal(Conjunto_Dados));
                            Resu_Sist.Add(new Result.PDO_Sist(Conjunto_Dados));
                        }

                    }





                    // if (!File.Exists(dec_oper)) return null;

                    result = new Result(this.BaseFolder) { Tipo = "DS" };

                    result.PDO_Sist_Result = Resu_Sist;

                   var pldlist = TrataPldDessem(Resu_Sist, limites.PLD_Min, limites.PLD_MaxHr, limites.PLD_MaxEst);

                    List<Result.PLD_DESSEM> Resu_pld= new List<Result.PLD_DESSEM>();

                    foreach (var pldl in pldlist)
                    {
                        Result.PLD_DESSEM pld = new Result.PLD_DESSEM();
                        pld.estagio = pldl.Item1;
                        pld.submercado = pldl.Item2;
                        pld.PLD = Math.Round(pldl.Item3, 2);
                        Resu_pld.Add(pld);
                    }
                    result.PLD_DESSEM_Result = Resu_pld;
                }


            }
            catch (Exception erro) { }
            return result;

        }

        public List<Tuple<int, string, double>> TrataPldDessem(List<Result.PDO_Sist> Resu_Sist, decimal? pldMin, decimal? pldMaxHr, decimal? pldMaxEst)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var limInf = Convert.ToDouble(pldMin);
            var limMax = Convert.ToDouble(pldMaxHr);
            var limEst = Convert.ToDouble(pldMaxEst);

            var submercados = Resu_Sist.Select(x => x.submercado).Distinct().ToList();

            List<Tuple<int, double>> dadosSE = new List<Tuple<int, double>>();
            List<Tuple<int, double>> dadosSUL = new List<Tuple<int, double>>();
            List<Tuple<int, double>> dadosNE = new List<Tuple<int, double>>();
            List<Tuple<int, double>> dadosN = new List<Tuple<int, double>>();

            var PldSE = Resu_Sist.Where(x => x.submercado == "SE").ToList();
            var PldSUL = Resu_Sist.Where(x => x.submercado == "S").ToList();
            var PldNE = Resu_Sist.Where(x => x.submercado == "NE").ToList();
            var PldN = Resu_Sist.Where(x => x.submercado == "N").ToList();

            int hora = 1;
            for (int h = 1; h <= 48; h += 2)
            {
                var hora1SE = PldSE.Where(x => x.estagio == h).Select(x => x.CMO).First();
                var hora2SE = PldSE.Where(x => x.estagio == (h + 1)).Select(x => x.CMO).First();
                var mediaSE = (hora1SE + hora2SE) / 2;
                dadosSE.Add(new Tuple<int, double>(hora, mediaSE));

                var hora1S = PldSUL.Where(x => x.estagio == h).Select(x => x.CMO).First();
                var hora2S = PldSUL.Where(x => x.estagio == (h + 1)).Select(x => x.CMO).First();
                var mediaS = (hora1S + hora2S) / 2;
                dadosSUL.Add(new Tuple<int, double>(hora, mediaS));

                var hora1NE = PldNE.Where(x => x.estagio == h).Select(x => x.CMO).First();
                var hora2NE = PldNE.Where(x => x.estagio == (h + 1)).Select(x => x.CMO).First();
                var mediaNE = (hora1NE + hora2NE) / 2;
                dadosNE.Add(new Tuple<int, double>(hora, mediaNE));

                var hora1N = PldN.Where(x => x.estagio == h).Select(x => x.CMO).First();
                var hora2N = PldN.Where(x => x.estagio == (h + 1)).Select(x => x.CMO).First();
                var mediaN = (hora1N + hora2N) / 2;
                dadosN.Add(new Tuple<int, double>(hora, mediaN));

                hora++;
            }


            var finalSE = GetPLdDessem(dadosSE, limInf, limMax, limEst);
            var finalSUL = GetPLdDessem(dadosSUL, limInf, limMax, limEst);
            var finalNE = GetPLdDessem(dadosNE, limInf, limMax, limEst);
            var finalN = GetPLdDessem(dadosN, limInf, limMax, limEst);

            List<Tuple<int, string, double>> pldlist = new List<Tuple<int, string, double>>();
            finalSE.ForEach(x => pldlist.Add(new Tuple<int, string, double>(x.Item1, "SE", x.Item2)));
            finalSUL.ForEach(x => pldlist.Add(new Tuple<int, string, double>(x.Item1, "S", x.Item2)));
            finalNE.ForEach(x => pldlist.Add(new Tuple<int, string, double>(x.Item1, "NE", x.Item2)));
            finalN.ForEach(x => pldlist.Add(new Tuple<int, string, double>(x.Item1, "N", x.Item2)));

            return pldlist;
           
        }

        static List<Tuple<int, double>> GetPLdDessem(List<Tuple<int, double>> mediasHoras, double limInf, double limMax, double limEst)
        {
            var lista = verificaLimitesPLd(mediasHoras, limInf, limMax, limEst);
            int cont = 0;
            var mediaDiaria = lista.Average(x => x.Item2);

            if (mediaDiaria > limEst)
            {
                var pldHoras = lista.Select(x => x.Item2).ToList();

                while (mediaDiaria > limEst && cont <= 30)
                {
                    var fator = limEst / mediaDiaria;
                    List<double> valores = new List<double>();
                    foreach (var item in pldHoras)
                    {
                        var val = item * fator;
                        valores.Add(val);
                    }

                    mediaDiaria = valores.Average();
                    pldHoras = valores;
                    cont++;
                }
                List<Tuple<int, double>> listaAjustada = new List<Tuple<int, double>>();
                for (int i = 1; i <= pldHoras.Count(); i++)
                {
                    listaAjustada.Add(new Tuple<int, double>(i, pldHoras[i - 1]));
                }

                return listaAjustada;
            }
            return lista;
        }

        static List<Tuple<int, double>> verificaLimitesPLd(List<Tuple<int, double>> mediasHoras, double limInf, double limMax, double limEst)
        {
            List<Tuple<int, double>> lista = new List<Tuple<int, double>>();
            double valor = 0;
            foreach (var dado in mediasHoras)
            {
                if (dado.Item2 < limInf)
                {
                    valor = limInf;
                }
                else if (dado.Item2 > limMax)
                {
                    valor = limMax;
                }
                else
                {
                    valor = dado.Item2;
                }
                lista.Add(new Tuple<int, double>(dado.Item1, valor));
            }

            return lista;
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


    }
}

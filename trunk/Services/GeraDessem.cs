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
    public class GeraDessem
    {
        public static void CriarEntdados(string path, DateTime dataEstudo, DateTime fimrev, Compass.CommomLibrary.EntdadosDat.CiceBlock blocoCICE, List<Tuple<int, float>> dadosRhe)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            string pathBlocos = "H:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base\\BlocosFixos";
            //string pathBlocos = "N:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base\\BlocosFixos";
            string com = null;

            var entdadosFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

            var pastaref = GetPastaRecente(fimrev);

            #region adiciona RIVAR
            //RIVAR  999     4
            var rivarline = new Compass.CommomLibrary.EntdadosDat.RivarLine();
            rivarline.IdBloco = "RIVAR";
            rivarline[1] = 999;
            rivarline[3] = 4;
            entdados.BlocoRivar.Add(rivarline);
            #endregion

            #region adiciona fp
            var rests = entdados.BlocoFp.Where(x => x.Usina == 287).ToList();
            if (rests.Count() == 0)
            {
                var rest = new Compass.CommomLibrary.EntdadosDat.FpLine();
                rest.IdBloco = "FP";
                rest.Usina = 287;
                rest.TipoFuncao = 2;
                rest.PontoVazTurb = 10;
                rest.PontoVolArm = 5;
                rest.VolUtilPerc = 100.00f;
                entdados.BlocoFp.Add(rest);
            }
            #endregion

            #region bloco CD

            foreach (var cdline in entdados.BlocoCd.ToList())
            {
                cdline.DiaInic = " I";
            }

            #endregion

            #region  RHEs
            List<int> restComent = new List<int> { 141, 143, 145, 272, 654 };

            foreach (var rest in restComent)
            {
                foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] == rest))
                {
                    foreach (var rh in rhe.Value)
                    {
                        rh[0] = "&" + rh[0];
                    }
                }
            }
            for (int i = 602; i <= 630; i++)
            {
                foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] == i))
                {
                    foreach (var rh in rhe.Value)
                    {
                        rh[0] = "&" + rh[0];
                    }
                }
            }
            var newlineFi = new Compass.CommomLibrary.EntdadosDat.FiLine();
            newlineFi[0] = "FI";
            newlineFi[1] = 433;
            newlineFi[2] = fimrev.AddDays(-6).Day.ToString();
            newlineFi[5] = " F";
            newlineFi[8] = "NE";
            newlineFi[9] = "SE";
            newlineFi[10] = 1;
            var newlineFi2 = new Compass.CommomLibrary.EntdadosDat.FiLine();
            newlineFi2[0] = "FI";
            newlineFi2[1] = 433;
            newlineFi2[2] = fimrev.AddDays(-6).Day.ToString();
            newlineFi2[5] = " F";
            newlineFi2[8] = "SE";
            newlineFi2[9] = "FC";
            newlineFi2[10] = -1;

            var res433 = entdados.BlocoRhe.Where(x => x[1] == 433).LastOrDefault();

            if (res433 != null)
            {
                entdados.BlocoRhe.InsertAfter(res433, newlineFi);
                entdados.BlocoRhe.InsertAfter(newlineFi, newlineFi2);

            }
            var rhefile = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains("blocoRHE")).First();


            var rhelines = File.ReadAllLines(rhefile);
            com = null;
            foreach (var linha in rhelines)
            {
                if (linha.StartsWith("&"))
                {
                    com = com == null ? linha : com + Environment.NewLine + linha;
                }
                else
                {
                    var newL = entdados.BlocoRhe.CreateLine(linha);
                    newL.Comment = com;
                    com = null;
                    entdados.BlocoRhe.Add(newL);
                }
            }

            if (pastaref != "")
            {
                var entdadosFileRef = Directory.GetFiles(pastaref).Where(x => Path.GetFileName(x).ToLower().Contains("entdados.dat")).First();
                var entdadosRef = DocumentFactory.Create(entdadosFileRef) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
                List<int> copiaRest = new List<int> { 850, 851, 901, 902, 906, 907, 908, 909, 910, 913, 911, 912, 915, 917, 918, 931, 924, 927, 928, 934, 935, 936, 939, 952, 947, 938, 940, 941, 942, 950, 949, 943, 944, 945, 946, 957, 958, 959, 960, 961, 962, 974, 982, 981, 986, 987, 991 };
                List<Tuple<int, int>> atualiza = new List<Tuple<int, int>> {
                    new Tuple<int, int>(403,901),//dadger,entdados
                    new Tuple<int, int>(427,902),
                    new Tuple<int, int>(417,906),
                    //new Tuple<int, int>(405,907),
                    new Tuple<int, int>(415,940),
                    new Tuple<int, int>(413,941),
                    new Tuple<int, int>(445,960),
                    new Tuple<int, int>(431,961),
                    new Tuple<int, int>(419,962)
                };

                foreach (var rest in copiaRest)
                {

                    foreach (var rhe in entdadosRef.BlocoRhe.RheGrouped.Where(x => x.Key[1] == rest))
                    {
                        foreach (var rh in rhe.Value)
                        {
                            entdados.BlocoRhe.Add(rh);
                        }
                    }
                }

                foreach (var par in atualiza)
                {
                    var lu = entdados.BlocoRhe.Where(x => x is Compass.CommomLibrary.EntdadosDat.LuLine && x.Restricao == par.Item2).FirstOrDefault();
                    if (lu != null)
                    {
                        float valor = dadosRhe.Where(x => x.Item1 == par.Item1).Select(x => x.Item2).First();
                        lu[9] = valor;
                    }
                }
            }
            var inicioSemOp = fimrev.AddDays(-6);
            foreach (var rhe in entdados.BlocoRhe.ToList())
            {
                int i;
                if (int.TryParse(rhe[2], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out i))
                {
                    if (i == inicioSemOp.Day)
                    {
                        rhe[2] = " I";
                    }
                    else
                    {
                        for (DateTime d = dataEstudo.AddDays(-7); d <= dataEstudo; d = d.AddDays(1))
                        {
                            var dia = i;
                            if (dia == d.Day)
                            {
                                rhe[2] = dataEstudo.Day.ToString();
                            }
                        }
                    }
                }
            }

            #endregion
            #region secr/cr/r11

            var secrfile = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains("blocoSECR")).First();


            var secrlines = File.ReadAllLines(secrfile);
            com = null;
            foreach (var linha in secrlines)
            {
                if (linha.StartsWith("&"))
                {
                    com = com == null ? linha : com + Environment.NewLine + linha;
                }
                else
                {
                    var newL = entdados.BlocoSecr.CreateLine(linha);
                    newL.Comment = com;
                    com = null;
                    entdados.BlocoSecr.Add(newL);
                }
            }

            var crfile = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains("blocoCR")).First();


            var crlines = File.ReadAllLines(crfile);
            com = null;
            foreach (var linha in crlines)
            {
                if (linha.StartsWith("&"))
                {
                    com = com == null ? linha : com + Environment.NewLine + linha;
                }
                else
                {
                    var newL = entdados.BlocoCr.CreateLine(linha);
                    newL.Comment = com;
                    com = null;
                    entdados.BlocoCr.Add(newL);
                }
            }


            if (pastaref != "")
            {
                var entdadosFileRef = Directory.GetFiles(pastaref).Where(x => Path.GetFileName(x).ToLower().Contains("entdados.dat")).First();

                var entdadosRef = DocumentFactory.Create(entdadosFileRef) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
                var r11line = entdadosRef.BlocoR11.FirstOrDefault();
                if (r11line != null)
                {
                    r11line.DiaInic = dataEstudo.Day.ToString();
                    entdados.BlocoR11.Clear();
                    entdados.BlocoR11.Add(r11line);
                }
            }

            #endregion
            #region limpa PQ

            entdados.BlocoPq.Clear();

            #endregion

            #region BLOCO TM
            bool patamares2023 = dataEstudo.Year == 2023;
            bool patamares2024 = dataEstudo.Year >= 2024;

            var intervalos = Tools.GetIntervalosHoararios(dataEstudo, patamares2023, patamares2024);
            string comentario = entdados.BlocoTm.First().Comment;
            for (DateTime d = dataEstudo.AddDays(-7); d <= dataEstudo; d = d.AddDays(1))
            {
                foreach (var line in entdados.BlocoTm.ToList())
                {
                    var dia = Convert.ToInt32(line.DiaInicial);
                    if (dia == d.Day)
                    {
                        entdados.BlocoTm.Remove(line);
                    }
                }
                //foreach (var line in entdados.BlocoTm.ToList())
                //{
                //    var dia = Convert.ToInt32(line.DiaInicial);
                //    if (dia <= dataEstudo.Day)
                //    {
                //        entdados.BlocoTm.Remove(line);
                //    }
                //}
            }

            var blocoClone = new Compass.CommomLibrary.EntdadosDat.TmBlock();

            foreach (var linha in entdados.BlocoTm.ToList())
            {
                blocoClone.Add(linha);
            }

            entdados.BlocoTm.Clear();
            for (int i = 0; i < 24; i++)
            {
                var newline = new Compass.CommomLibrary.EntdadosDat.TmLine();
                newline.IdBloco = "TM";
                newline.DiaInicial = dataEstudo.Day.ToString();
                newline.HoraDiaInicial = i;
                newline.MeiaHora = 0;
                newline.Duracao = 0.5f;
                newline.Rede = 0;
                newline.NomePatamar = intervalos[i];
                if (i == 0)
                {
                    newline.Comment = comentario;
                }
                entdados.BlocoTm.Add(newline);
                var newline2 = new Compass.CommomLibrary.EntdadosDat.TmLine();
                newline2.IdBloco = "TM";
                newline2.DiaInicial = dataEstudo.Day.ToString();
                newline2.HoraDiaInicial = i;
                newline2.MeiaHora = 1;
                newline2.Duracao = 0.5f;
                newline2.Rede = 0;
                newline2.NomePatamar = intervalos[i];
                entdados.BlocoTm.Add(newline2);

            }
            foreach (var linha in blocoClone.ToList())
            {
                entdados.BlocoTm.Add(linha);
            }

            #endregion

            #region VE

            foreach (var line in entdados.BlocoVe.ToList())
            {
                line.DiaInic = dataEstudo.Day.ToString();
                line.DiaFinal = dataEstudo.AddDays(1).Day.ToString();
            }

            #endregion

            #region IA

            //foreach (var iaLine in entdados.BlocoIa.Where(x => x.SistemaA.Trim() == "N" && x.SistemaB.Trim() == "SE").ToList())
            //{
            //    iaLine.IdBloco = "&" + iaLine.IdBloco;
            //}
            List<Tuple<string, string>> sistDePara = new List<Tuple<string, string>> {
                new Tuple<string, string>("IV","S"),
                new Tuple<string, string>("N","FC"),
                new Tuple<string, string>("NE","FC"),
                new Tuple<string, string>("SE","FC"),
                new Tuple<string, string>("SE","IV"),
                new Tuple<string, string>("SE","NE")
            };
            string comentarioIA = entdados.BlocoIa.First().Comment;

            entdados.BlocoIa.Clear();

            foreach (var sistemas in sistDePara)
            {
                var newlIA = new Compass.CommomLibrary.EntdadosDat.IaLine();
                if (sistemas == sistDePara.First())
                {
                    newlIA.Comment = comentarioIA;
                }
                newlIA.IdBloco = "IA";
                newlIA.SistemaA = sistemas.Item1;
                newlIA.SistemaB = sistemas.Item2;
                newlIA.DiaInic = "I";
                newlIA.Horainic = 0;
                newlIA.MeiaHoraInic = 0;
                newlIA.DiaFinal = "F";
                newlIA.IntercambioAB = 99999;
                newlIA.IntercambioBA = 99999;

                entdados.BlocoIa.Add(newlIA);
            }

            #endregion

            #region TVIAG

            var tviagFile = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains("blocoTVIAG")).First();


            entdados.BlocoTviag.Clear();
            //var tviaglines = File.ReadAllLines(tviagFile, Encoding.UTF8);
            var tviaglines = File.ReadAllLines(tviagFile);
            string comments = null;
            foreach (var linha in tviaglines)
            {
                if (linha.StartsWith("&"))
                {
                    comments = comments == null ? linha : comments + Environment.NewLine + linha;
                }
                else
                {
                    var newL = entdados.BlocoTviag.CreateLine(linha);
                    newL.Comment = comments;
                    comments = null;
                    entdados.BlocoTviag.Add(newL);
                }
            }

            #endregion

            #region DP/DE



            var inicioRev = fimrev.AddDays(-6);
            int index = 0;
            for (DateTime d = inicioRev; d <= fimrev; d = d.AddDays(1))
            {
                if (d <= dataEstudo)
                {
                    index++;
                }
            }
            var dpFileCSV = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains($"blocoDPcarga{index}.csv")).First();
            //var dplines = File.ReadAllLines(dpFile, Encoding.UTF8);
            var dplines = File.ReadAllLines(dpFileCSV).ToList();

            List<Tuple<int, int, float>> dadosCarga = new List<Tuple<int, int, float>>();

            if (!File.Exists(Path.Combine(path, dpFileCSV.Split('\\').Last())))
            {
                File.Copy(dpFileCSV, Path.Combine(path, dpFileCSV.Split('\\').Last()), true);
            }

            foreach (var dpl in dplines)
            {
                var dados = dpl.Split(';').ToList();

                Tuple<int, int, float> dad = new Tuple<int, int, float>(Convert.ToInt32(dados[0]), Convert.ToInt32(dados[1]), float.Parse(dados[2]));
                dadosCarga.Add(dad);//submercad,hora,valor
            }
            string comentarioDP = entdados.BlocoDp.First().Comment;

            entdados.BlocoDp.Clear();

            for (int s = 1; s <= 4; s++)//submercado
            {
                for (DateTime d = dataEstudo; d <= fimrev; d = d.AddDays(1))//
                {
                    if (d == dataEstudo)
                    {
                        for (int i = 1; i <= 24; i++)
                        {
                            float valor = dadosCarga.Where(x => x.Item1 == s && x.Item2 == i).Select(x => x.Item3).First();
                            var newDP = new Compass.CommomLibrary.EntdadosDat.DpLine();
                            if (s == 1 && i == 1)
                            {
                                newDP.Comment = comentarioDP;
                            }
                            newDP.IdBloco = "DP";
                            newDP.Subsist = s;
                            newDP.DiaInic = $"{d.Day:00}";
                            newDP.HoraInic = i - 1;
                            newDP.MeiaHoraInic = 0;
                            newDP.DiaFinal = " F";
                            newDP.Demanda = valor;
                            entdados.BlocoDp.Add(newDP);

                            var newDP2 = new Compass.CommomLibrary.EntdadosDat.DpLine();
                            newDP2.IdBloco = "DP";
                            newDP2.Subsist = s;
                            newDP2.DiaInic = $"{d.Day:00}";
                            newDP2.HoraInic = i - 1;
                            newDP2.MeiaHoraInic = 1;
                            newDP2.DiaFinal = " F";
                            newDP2.Demanda = valor;
                            entdados.BlocoDp.Add(newDP2);
                        }
                    }
                    else//pega os dados do csv dos dias seguintes para o calculo da media por horas agrupadas
                    {
                        index = 0;
                        for (DateTime newd = inicioRev; newd <= d; newd = newd.AddDays(1))
                        {
                            if (newd <= d)
                            {
                                index++;
                            }
                        }
                        var NewdpFileCSV = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains($"blocoDPcarga{index}.csv")).First();
                        //var dplines = File.ReadAllLines(dpFile, Encoding.UTF8);
                        var Newdplines = File.ReadAllLines(NewdpFileCSV).ToList();

                        List<Tuple<int, int, float>> NewdadosCarga = new List<Tuple<int, int, float>>();

                        foreach (var Ndpl in Newdplines)
                        {
                            var Ndados = Ndpl.Split(';').ToList();

                            Tuple<int, int, float> Ndad = new Tuple<int, int, float>(Convert.ToInt32(Ndados[0]), Convert.ToInt32(Ndados[1]), float.Parse(Ndados[2]));
                            NewdadosCarga.Add(Ndad);//submercad,hora,valor
                        }
                        bool pat2023 = d.Year == 2023;
                        bool pat2024 = d.Year >= 2024;

                        var intervalosAgruped = Tools.GetIntervalosPatamares(d, pat2023, pat2024);

                        foreach (var inter in intervalosAgruped)
                        {
                            var listaValores = NewdadosCarga.Where(x => x.Item1 == s && x.Item2 >= inter.Item1 && x.Item2 <= inter.Item2).Select(x => x.Item3).ToList();

                            float valorMedia = listaValores.Average();
                            var newDpSeguinte = new Compass.CommomLibrary.EntdadosDat.DpLine();

                            newDpSeguinte.IdBloco = "DP";
                            newDpSeguinte.Subsist = s;
                            newDpSeguinte.DiaInic = $"{d.Day:00}";
                            newDpSeguinte.HoraInic = inter.Item1 - 1;
                            newDpSeguinte.MeiaHoraInic = 0;
                            newDpSeguinte.DiaFinal = " F";
                            newDpSeguinte.Demanda = valorMedia;
                            entdados.BlocoDp.Add(newDpSeguinte);
                        }

                        if (!File.Exists(Path.Combine(path, NewdpFileCSV.Split('\\').Last())))
                        {
                            File.Copy(NewdpFileCSV, Path.Combine(path, NewdpFileCSV.Split('\\').Last()), true);
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





            //cpoaiiaia


            //codigo antigo
            //var inicioRev = fimrev.AddDays(-6);
            //int index = 0;
            //for (DateTime d = inicioRev; d <= fimrev; d = d.AddDays(1))
            //{
            //    if (d <= dataEstudo)
            //    {
            //        index++;
            //    }
            //}
            //var dpFile = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains($"blocoDP{index}")).First();
            ////var dplines = File.ReadAllLines(dpFile, Encoding.UTF8);
            //var dplines = File.ReadAllLines(dpFile);
            //entdados.BlocoDp.Clear();

            //comments = null;
            //foreach (var linha in dplines)//copia os dados do blocoDP fixo
            //{
            //    if (linha.StartsWith("&"))
            //    {
            //        comments = comments == null ? linha : comments + Environment.NewLine + linha;
            //    }
            //    else
            //    {
            //        var newL = entdados.BlocoDp.CreateLine(linha);
            //        newL.Comment = comments;
            //        comments = null;
            //        entdados.BlocoDp.Add(newL);
            //    }
            //}

            //for (int s = 1; s <= 4; s++)//corrige as datas iniciais
            //{
            //    DateTime dia = dataEstudo;
            //    var diaComp = Convert.ToInt32(entdados.BlocoDp.Where(x => x.Subsist == s).Select(x => x.DiaInic).First());
            //    foreach (var dp in entdados.BlocoDp.Where(x => x.Subsist == s).ToList())
            //    {
            //        var dialinha = Convert.ToInt32(dp.DiaInic);
            //        if (diaComp == dialinha)
            //        {
            //            dp.DiaInic = dia.Day.ToString();
            //        }
            //        else
            //        {
            //            diaComp = Convert.ToInt32(dp.DiaInic);
            //            dia = dia.AddDays(1);

            //            dp.DiaInic = dia.Day.ToString();
            //        }
            //    }

            //}
            //foreach (var dp in entdados.BlocoDp.Where(x => x.Subsist == 11).ToList())
            //{
            //    dp.DiaInic = dataEstudo.Day.ToString();
            //}
            //fim codigo antigo
            //DE

            var deFile = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains($"blocoDE{index}")).First();
            // var delines = File.ReadAllLines(deFile, Encoding.UTF8);
            var delines = File.ReadAllLines(deFile);
            entdados.BlocoDe.Clear();

            comments = null;
            foreach (var linha in delines)//copia os dados do blocoDE fixo
            {
                if (linha.StartsWith("&"))
                {
                    comments = comments == null ? linha : comments + Environment.NewLine + linha;
                }
                else
                {
                    var newL = entdados.BlocoDe.CreateLine(linha);
                    newL.Comment = comments;
                    comments = null;
                    entdados.BlocoDe.Add(newL);
                }
            }

            for (int de = 1; de <= 5; de++)//corrige as datas iniciais
            {
                DateTime dia = dataEstudo;
                if (de == 5)
                {
                    dia = inicioRev;
                }
                var diaComp = Convert.ToInt32(entdados.BlocoDe.Where(x => x.NumDemanda == de).Select(x => x.DiaInic).First());
                foreach (var dem in entdados.BlocoDe.Where(x => x.NumDemanda == de).ToList())
                {
                    var dialinha = Convert.ToInt32(dem.DiaInic);
                    if (diaComp == dialinha)
                    {
                        dem.DiaInic = dia.Day.ToString();
                    }
                    else
                    {
                        diaComp = Convert.ToInt32(dem.DiaInic);
                        dia = dia.AddDays(1);

                        dem.DiaInic = dia.Day.ToString();
                    }
                }

            }
            foreach (var dem in entdados.BlocoDe.Where(x => (x.NumDemanda == 11) || (x.NumDemanda == 6)).ToList())
            {
                dem.DiaInic = dataEstudo.Day.ToString();
            }

            #endregion

            #region CICE/UT
            entdados.BlocoCice.Clear();
            foreach (var lineCICE in blocoCICE.ToList())
            {
                entdados.BlocoCice.Add(lineCICE);
            }

            var ceLines = blocoCICE.Where(x => x.IdBloco == "CE").ToList();
            foreach (var celine in ceLines)
            {
                var utlines = entdados.BlocoUt.Where(x => x.Usina == celine.IdContrato).ToList();

                foreach (var ut in utlines)
                {
                    if (ut == utlines.First())
                    {
                        continue;
                    }
                    else
                    {
                        entdados.BlocoUt.Remove(ut);//remove linhas deixando apenas uma que irá ser considerada desde de o dia inicial ate o final
                    }
                }
                foreach (var utl in entdados.BlocoUt.Where(x => x.Usina == celine.IdContrato))
                {
                    utl.GeracaoMaxRest = celine.EnergiaMax;
                    utl.GeracaoMinRest = celine.EnergiaMin;
                    utl.DiaInic = dataEstudo.Day < 10 ? " " + dataEstudo.Day.ToString() : dataEstudo.Day.ToString();
                    utl.HoraInic = 0;
                    utl.MeiaHoraInic = 0;
                    utl.DiaFinal = " F";
                }
            }

            #endregion

            #region AC

            if (pastaref != "")
            {
                var entdadosFileRef = Directory.GetFiles(pastaref).Where(x => Path.GetFileName(x).ToLower().Contains("entdados.dat")).First();

                var entdadosRef = DocumentFactory.Create(entdadosFileRef) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
                List<string> minemonicos = new List<string> { "NUMCON", "NUMMAQ", "POTEFE", "VAZEFE", "ALTEFE" };

                var AClinesref = entdadosRef.BlocoAc.Where(x => x.Usina == 287 && minemonicos.Any(y => y.Contains(x.Mnemonico.Trim()))).ToList();

                foreach (var line in AClinesref)
                {
                    entdados.BlocoAc.Add(line);
                }
            }

            #endregion

            entdados.SaveToFile();

            #region AC blocofixo

            var acFile = Directory.GetFiles(pathBlocos).Where(x => Path.GetFileName(x).Contains("blocoAC")).First();

            var acTexto = File.ReadAllText(acFile);


            var entlinhas = File.ReadAllLines(entdadosFile).ToList();

            int indiceAC = entlinhas.IndexOf(entlinhas.Where(x => x.StartsWith("AC")).Last());
            entlinhas.Insert((indiceAC + 1), acTexto);


            File.WriteAllLines(entdadosFile, entlinhas);

            #endregion

            var areacontfile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("areacont")).First();
            var areacont = DocumentFactory.Create(areacontfile) as Compass.CommomLibrary.Areacont.Areacont;

            areacont.SaveToFile();

            var cotasr11File = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("cotasr11")).First();
            var cotasr11 = DocumentFactory.Create(cotasr11File) as Compass.CommomLibrary.Cotasr.Cotasr;

            foreach (var line in cotasr11.BlocoCot.ToList())
            {
                line.Dia = dataEstudo.AddDays(-1).Day;
            }
            cotasr11.SaveToFile();
        }

        public static void RestsegRstlppCopy(string path, DateTime dataEstudo, DateTime fimRev)
        {
            var pastaref = GetPastaRecente(fimRev);
            if (pastaref != "")
            {
                var restseg = Directory.GetFiles(pastaref).Where(x => Path.GetFileName(x).ToLower().Contains("restseg")).FirstOrDefault();
                var rstlpp = Directory.GetFiles(pastaref).Where(x => Path.GetFileName(x).ToLower().Contains("rstlpp")).FirstOrDefault();

                if (restseg != null)
                {
                    File.Copy(restseg, Path.Combine(path, restseg.Split('\\').Last().ToLower()), true);
                }
                if (rstlpp != null)
                {
                    File.Copy(rstlpp, Path.Combine(path, rstlpp.Split('\\').Last().ToLower()), true);
                }
            }
        }

        public static void Renovaveis(string path, DateTime dataEstudo, DateTime fimRev)
        {

            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var inicioRev = fimRev.AddDays(-6);
            var diaInc = inicioRev;

            var dataRef = DateTime.Today;
            string renoRef = "";

            int cont = 0;
            bool Ok = false;
            while (Ok == false && cont < 10)
            {
                DateTime dat = dataRef;

                if (dat.DayOfWeek == DayOfWeek.Saturday)
                {
                    var rev = Tools.GetCurrRev(dat);
                    //H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\2021\01_jan\RV3\DS_CCEE_012021_SEMREDE_RV3D19
                    var mes = Tools.GetMonthNumAbrev(rev.revDate.Month);
                    var cam = $@"H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
                    //var cam = $@"N:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
                    var arqRef = Directory.GetFiles(cam).Where(x => Path.GetFileName(x).ToLower().Contains("renovaveis")).FirstOrDefault();
                    if (arqRef != null)
                    {
                        renoRef = arqRef;
                        Ok = true;
                    }
                    else
                    {
                        cont++;
                        dataRef = dataRef.AddDays(-1);
                    }
                }
                else
                {
                    dataRef = dataRef.AddDays(-1);
                }

            }
            //////////////////////

            if (renoRef != "")
            {
                var renovaveis = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("renovaveis")).FirstOrDefault();
                var renoLines = File.ReadAllLines(renoRef);
                List<string> modifReno = new List<string>();
                foreach (var item in renoLines)
                {
                    if (item.Split(';').First().Trim() == "EOLICA-GERACAO")
                    {
                        var textos = item.Split(';').ToList();
                        var dI = Convert.ToInt32(textos[2].Trim());
                        var df = Convert.ToInt32(textos[5].Trim());
                        string linha = "";
                        var parte = "";
                        for (int i = 0; i < textos.Count() - 1; i++)
                        {
                            int diasAdic = 0;

                            if (i == 2)
                            {
                                if (dI < dataRef.Day)
                                {
                                    DateTime d = new DateTime(dataRef.AddMonths(1).Year, dataRef.AddMonths(1).Month, dI);
                                    diasAdic = (int)d.Subtract(dataRef).TotalDays;

                                    DateTime diaAtual = inicioRev.AddDays(diasAdic);
                                    parte = diaAtual.Day < 10 ? " " + diaAtual.Day.ToString() + " ;" : diaAtual.Day.ToString() + " ;";
                                }
                                else
                                {
                                    DateTime d = new DateTime(dataRef.Year, dataRef.Month, dI);
                                    diasAdic = (int)d.Subtract(dataRef).TotalDays;
                                    DateTime diaAtual = inicioRev.AddDays(diasAdic);
                                    parte = diaAtual.Day < 10 ? " " + diaAtual.Day.ToString() + " ;" : diaAtual.Day.ToString() + " ;";
                                }
                            }
                            else if (i == 5)
                            {
                                if (df < dataRef.Day)
                                {
                                    DateTime d = new DateTime(dataRef.AddMonths(1).Year, dataRef.AddMonths(1).Month, df);
                                    diasAdic = (int)d.Subtract(dataRef).TotalDays;
                                    DateTime diaAtual = inicioRev.AddDays(diasAdic);
                                    parte = diaAtual.Day < 10 ? " " + diaAtual.Day.ToString() + " ;" : diaAtual.Day.ToString() + " ;";
                                }
                                else
                                {
                                    DateTime d = new DateTime(dataRef.Year, dataRef.Month, df);
                                    diasAdic = (int)d.Subtract(dataRef).TotalDays;
                                    DateTime diaAtual = inicioRev.AddDays(diasAdic);
                                    parte = diaAtual.Day < 10 ? " " + diaAtual.Day.ToString() + " ;" : diaAtual.Day.ToString() + " ;";
                                }
                            }
                            else
                            {
                                parte = textos[i] + ";";
                            }

                            linha += parte;
                            //if (parte != textos.Last())
                            //{
                            //    linha += ";";
                            //}

                        }

                        modifReno.Add(linha);
                    }
                    else
                    {
                        modifReno.Add(item);
                    }
                }
                File.WriteAllLines(renovaveis, modifReno);

                //todo dados weol
                var dateWeol = inicioRev.AddDays(-1);

                string mesAbrev = Tools.GetMonthNumAbrev(dateWeol.Month);
                string weolFolder = $@"H:\Middle - Preço\Resultados_Modelos\WEOL\{mesAbrev}\Deck_Previsao_{dateWeol:yyyyMMdd}";
                //string weolFolder = $@"N:\Middle - Preço\Resultados_Modelos\WEOL\{mesAbrev}\Deck_Previsao_{dateWeol:yyyyMMdd}";

                var conjUsiBarRat = File.ReadAllLines(Path.Combine(weolFolder, "Arquivos Entrada", "Conjunto Usina Barra Rateio.txt")).Skip(1).ToList();
                var dadosPontos = File.ReadAllLines(Path.Combine(weolFolder, "Arquivos Entrada", "Dados dos Pontos.txt")).ToList();
                var renovaveisDat = DocumentFactory.Create(renovaveis) as Compass.CommomLibrary.Renovaveis.Renovaveis;
                var blocoEolica = renovaveisDat.BlocoEolica.ToList();
                var blocoEolicaBarra = renovaveisDat.BlocoEolicaBarra.ToList();
                var blocoEolicaSub = renovaveisDat.BlocoEolicaSubm.ToList();
                //var blocoEolicaGer = renovaveisDat.BlocoEolicaGeracao.ToList();

                List<weolDados> weolDados = new List<weolDados>();
                List<weolGeracao> weolGeracoes = new List<weolGeracao>();

                foreach (var pontosLine in dadosPontos)
                {
                    var campos = pontosLine.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    var subm = campos[2];
                    for (int i = 3; i < campos.Count(); i++)
                    {
                        var nomeCod = campos[i];
                        var usiBarraLines = conjUsiBarRat.Where(x => x.Contains(nomeCod)).ToList();
                        foreach (var line in usiBarraLines)
                        {
                            weolDados weoldado = new weolDados();

                            var codrenovavies = line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[0];

                            var barra = Convert.ToInt32(line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[3]);
                            var rateio = Convert.ToDouble(line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[4].Replace('.', ','));
                            weoldado.barra = barra;
                            weoldado.codReno = codrenovavies;
                            weoldado.codWeol = nomeCod;
                            weoldado.rateio = rateio;
                            weoldado.subM = subm;

                            weolDados.Add(weoldado);
                        }
                    }
                }

                string PrevsFolder = Path.Combine(weolFolder, "Previsoes por Usinas", "Previsao combinada");

                for (DateTime dt = dataEstudo; dt <= fimRev; dt = dt.AddDays(1))
                {
                    //Previsoes_NE_20210408_20210409.txt
                    foreach (var weold in weolDados)
                    {
                        weolGeracao weolGer = new weolGeracao();
                        var arqPrevs = $@"Previsoes_{weold.subM}_{dateWeol:yyyyMMdd}_{dt:yyyyMMdd}.txt";

                        var prevsDados = File.ReadAllLines(Path.Combine(PrevsFolder, arqPrevs)).Where(x => x.StartsWith(weold.codWeol))
                                        .First().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                        List<double> prevs = new List<double>();

                        foreach (var item in prevsDados)
                        {
                            double valor = (Convert.ToDouble(item.Replace('.', ',')) * weold.rateio) / 100;
                            prevs.Add(valor);
                        }
                        weolGer.barra = weold.barra;
                        weolGer.codReno = weold.codReno;
                        weolGer.codWeol = weold.codWeol;
                        weolGer.dataPrev = dt;
                        weolGer.geracoes = prevs;

                        weolGeracoes.Add(weolGer);

                    }

                }

                //for (DateTime dt = dataEstudo; dt <= fimRev; dt = dt.AddDays(1))
                //{
                foreach (var eolicaLine in blocoEolica)
                {
                    var codigo = eolicaLine.NumCodigo;
                    int codigoInt = Convert.ToInt32(eolicaLine.NumCodigo.Replace(';', ' ').Trim());
                    var nomeCodigo = eolicaLine.Nome.Split('_').First().Trim();
                    var barraReno = Convert.ToInt32(blocoEolicaBarra.Where(x => x.NumCodigo == codigo).Select(x => x.Barra.Replace(';', ' ').Trim()).First());

                    var weols = weolGeracoes.Where(x => x.codReno == nomeCodigo && x.barra == barraReno).ToList();
                    if (weols.Count() > 0)
                    {
                        double valorTotal = 0;

                        foreach (var ger in renovaveisDat.BlocoEolicaGeracao.Where(x => x.NumCodigo == codigo).ToList())//erro trocar pra renovaveis.bloco.....
                        {
                            renovaveisDat.BlocoEolicaGeracao.Remove(ger);
                        }
                        var testes = renovaveisDat.BlocoEolicaGeracao.Where(x => Convert.ToInt32(x.NumCodigo.Replace(';', ' ').Trim()) < codigoInt).Last();
                        int index = renovaveisDat.BlocoEolicaGeracao.IndexOf(renovaveisDat.BlocoEolicaGeracao.Where(x => Convert.ToInt32(x.NumCodigo.Replace(';', ' ').Trim()) < codigoInt).Last());

                        for (DateTime dt = dataEstudo; dt <= fimRev; dt = dt.AddDays(1))
                        {
                            int hr = 0;
                            int fm = 0;
                            // foreach (var w in weols.Where(x => x.dataPrev == dt))
                            //{
                            for (int i = 0; i < weols.First().geracoes.Count(); i++)
                            {


                                valorTotal = Math.Round(weols.Where(x => x.dataPrev == dt).Select(x => x.geracoes[i]).Sum());

                                var newl = new Compass.CommomLibrary.Renovaveis.EolicaGeracaoLine();
                                newl.Idbloco = "EOLICA-GERACAO ;";

                                newl.NumCodigo = codigo;

                                newl.DiaIni = dt.Day < 10 ? " " + dt.Day.ToString() + " ;" : dt.Day.ToString() + " ;";

                                newl.HoraIni = hr < 10 ? " " + hr.ToString() + " ;" : hr.ToString() + " ;";

                                newl.MeiaHoraIni = fm.ToString() + " ;";

                                if (i == weols.First().geracoes.Count() - 1 && fm == 1)
                                {
                                    newl.DiaFim = dt.AddDays(1).Day < 10 ? " " + dt.AddDays(1).Day.ToString() + " ;" : dt.AddDays(1).Day.ToString() + " ;";
                                    newl.HoraFim = " 0 ;";
                                    newl.MeiaHoraFim = "0 ;";
                                }
                                else
                                {
                                    newl.DiaFim = dt.Day < 10 ? " " + dt.Day.ToString() + " ;" : dt.Day.ToString() + " ;";

                                    if (fm == 1) hr++;
                                    newl.HoraFim = hr < 10 ? " " + hr.ToString() + " ;" : hr.ToString() + " ;";

                                    if (fm == 1)
                                    {
                                        fm = 0;
                                        newl.MeiaHoraFim = fm.ToString() + " ;";
                                    }
                                    else
                                    {
                                        fm = 1;
                                        newl.MeiaHoraFim = fm.ToString() + " ;";
                                    }
                                }

                                string partVal = valorTotal.ToString() + " ;";
                                while (partVal.Length < 12)
                                {
                                    partVal = " " + partVal;
                                }

                                newl.Geracao = partVal;
                                renovaveisDat.BlocoEolicaGeracao.Insert(index + 1, newl);
                                index++;
                                valorTotal = 0;
                            }
                            //}

                        }

                    }
                }
                // }
                var firstEolica = renovaveisDat.BlocoEolica.First();

                foreach (var eol in renovaveisDat.BlocoEolica.ToList())//trata erros nos numeros de caracteres lidos quando o nome da usina esta com caracteres não reconhecidos
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

                renovaveisDat.SaveToFile();
            }

        }
        public static void TrataOperuh(string path, DateTime dataEstudo, DateTime fimRev)
        {
            var pastaref = GetPastaRecente(fimRev);
            if (pastaref != "")
            {
                var operuhFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("operuh")).First();
                var operuh = DocumentFactory.Create(operuhFile) as Compass.CommomLibrary.Operuh.Operuh;
                var operuhFileRef = Directory.GetFiles(pastaref).Where(x => Path.GetFileName(x).ToLower().Contains("operuh")).First();
                var operuhRef = DocumentFactory.Create(operuhFileRef) as Compass.CommomLibrary.Operuh.Operuh;

                var rhestsRef = operuhRef.BlocoRhest.Where(x => x.Minemonico.Trim() == "REST" && x[3] == "V").ToList();
                var rhests = operuh.BlocoRhest.Where(x => x.Minemonico.Trim() == "REST" && x[3] == "V").ToList();

                foreach (var rheRef in rhestsRef)
                {
                    foreach (var rhe in rhests.Where(x => x.Restricao == rheRef.Restricao).ToList())
                    {
                        rhe[6] = rheRef[6] ?? ".";
                    }
                }
                foreach (var rhe in rhests.ToList())
                {
                    string embranco = rhe[6];
                    if (embranco.Trim() == "")
                    {
                        rhe[6] = "        .";
                    }
                }
                var restFioDagua = operuh.BlocoRhest.Where(x => x is Compass.CommomLibrary.Operuh.ElemLine && (x[3] == 130 || x[3] == 185)).Select(x => x.Restricao).ToList();
                foreach (var item in restFioDagua)
                {
                    foreach (var line in operuh.BlocoRhest.Where(x => x.Restricao == item))
                    {
                        line[0] = "&" + line[0];
                    }
                }

                #region Correcao Operuh
                //correção do operuh baixado (vem com datas de dia inicial e final erradas nas restrições, solução: comentar linhas )

                var restsLinesVerif = operuh.BlocoRhest.Where(x => x is Compass.CommomLibrary.Operuh.LimLine || x is Compass.CommomLibrary.Operuh.VarLine).ToList();
                List<string> restCorrecao = new List<string>();

                foreach (var resV in restsLinesVerif)
                {
                    string di = resV[3];
                    string df = resV[6];

                    if ((di.Trim() == "I" || di.Trim() == dataEstudo.Day.ToString()) && df.Trim() == dataEstudo.Day.ToString() && resV[7] == 0 && resV[8] == 0)
                    {
                        restCorrecao.Add(resV.Restricao);
                    }
                }
                if (restCorrecao.Count() > 0)
                {
                    foreach (var correcao in restCorrecao)
                    {
                        var comentaLine = operuh.BlocoRhest.Where(x => x.Restricao == correcao).ToList();
                        foreach (var com in comentaLine)
                        {
                            com[0] = "&" + com[0];
                        }
                    }
                }
                #endregion

                #region Atualizacao com dados recentes
                //usa o operuh oficial CCEE mais recente para incluir as informaçoes referentes às restricoes da regua 11

                string pastaRecent = "";

                int cont = 0;
                bool Ok = false;
                DateTime dataRef = DateTime.Today;
                DateTime revRef = new DateTime();
                while (Ok == false && cont < 30)
                {
                    DateTime dat = dataRef;
                    DateTime datVE = dataRef;
                    if (dat.DayOfWeek == DayOfWeek.Friday)
                    {
                        datVE = dat.AddDays(-1);
                    }
                    var rev = Tools.GetCurrRev(datVE);
                    //H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\2021\01_jan\RV3\DS_CCEE_012021_SEMREDE_RV3D19
                    var mes = Tools.GetMonthNumAbrev(rev.revDate.Month);
                    var cam = $@"H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
                    //var cam = $@"N:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
                    if (Directory.Exists(cam))
                    {
                        pastaRecent = cam;
                        revRef = rev.revDate;
                        Ok = true;
                    }
                    else
                    {
                        cont++;
                        dataRef = dataRef.AddDays(-1);
                    }
                }

                if (pastaRecent != "")
                {
                    var operuhRec = Directory.GetFiles(pastaRecent).Where(x => Path.GetFileName(x).ToLower().Contains("operuh")).FirstOrDefault();
                    if (operuhRec != null)
                    {
                        DateTime incRevRef = revRef.AddDays(-6);
                        DateTime incRevEstudo = fimRev.AddDays(-6);

                        var recLines = File.ReadAllLines(operuhRec).ToList();
                        int indice = recLines.IndexOf(recLines.Where(x => x.Contains("Restricoes da Regua 11")).FirstOrDefault());
                        if (indice > 0)
                        {
                            string com = null;
                            com = null;
                            for (int i = indice; i < recLines.Count(); i++)
                            {
                                if (recLines[i] != "")
                                {
                                    if (recLines[i].StartsWith("&"))
                                    {
                                        com = com == null ? recLines[i] : com + Environment.NewLine + recLines[i];
                                    }
                                    else
                                    {
                                        var newL = operuh.BlocoRhest.CreateLine(recLines[i]);
                                        newL.Comment = com;
                                        com = null;
                                        operuh.BlocoRhest.Add(newL);
                                    }
                                }

                            }
                            if (com != null)
                            {
                                operuh.BottonComments = com;
                            }
                            var restXingu = operuh.BlocoRhest.Where(x => x is Compass.CommomLibrary.Operuh.VarLine && x.Restricao == "99116").ToList();
                            foreach (var resx in restXingu)
                            {
                                int dias = 0;
                                int diainic = Convert.ToInt32(resx[3]);
                                for (DateTime date = incRevRef; date <= revRef; date = date.AddDays(1))
                                {
                                    if (diainic == date.Day)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        dias++;
                                    }
                                }
                                resx[3] = $"{ incRevEstudo.AddDays(dias).Day:00}";
                            }

                            var restHidro = operuh.BlocoRhest.Where(x => x is Compass.CommomLibrary.Operuh.LimLine && x.Restricao == "00709").ToList();
                            foreach (var hidr in restHidro)
                            {
                                if (hidr == restHidro.First())
                                {
                                    continue;
                                }
                                else
                                {
                                    operuh.BlocoRhest.Remove(hidr);
                                }
                            }
                            var lim = operuh.BlocoRhest.Where(x => x is Compass.CommomLibrary.Operuh.LimLine && x.Restricao == "00709").First();
                            if (incRevEstudo.Month != fimRev.Month)
                            {
                                lim[3] = " I";
                                lim[6] = " F";
                                lim[9] = Tools.GetHidrogram(incRevEstudo);
                                var nlim = new Compass.CommomLibrary.Operuh.LimLine();
                                nlim[0] = lim[0];
                                nlim[1] = lim[1];
                                nlim[2] = lim[2];

                                nlim[3] = "01";
                                nlim[6] = " F";
                                nlim[9] = Tools.GetHidrogram(incRevEstudo.AddMonths(1));
                                operuh.BlocoRhest.InsertAfter(lim, nlim);
                            }
                            else
                            {
                                lim[3] = " I";
                                lim[6] = " F";
                                lim[9] = Tools.GetHidrogram(incRevEstudo);
                            }
                        }

                    }
                }

                #endregion

                operuh.SaveToFile();
            }


        }

        public static void TrataDeflant(string path, DateTime dataEstudo)
        {
            var dadvaz = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz")).First();
            var dadlinhas = File.ReadAllLines(dadvaz).ToList();
            var dados = dadlinhas[9].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            DateTime data = new DateTime(Convert.ToInt32(dados[3]), Convert.ToInt32(dados[2]), Convert.ToInt32(dados[1]));

            var entdadosFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
            var deflantFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("deflant")).First();
            var deflant = DocumentFactory.Create(deflantFile) as Compass.CommomLibrary.Deflant.Deflant;

            string comentario = deflant.BlocoDef.First().Comment;
            float valor = 0f;
            deflant.BlocoDef.Clear();
            string pasta = "";
            var tviag = entdados.BlocoTviag.ToList();
            foreach (var tv in tviag)
            {
                if (tv.Montante != 66 && tv.Montante != 83)
                {

                    var horas = tv.TempoViag;
                    var dataAnt = data.AddHours(-horas);
                    for (int i = 0; i < horas; i += 24)
                    {
                        pasta = GetDirBase(dataAnt);
                        if (pasta != "")
                        {
                            valor = GetDefluencia(pasta, tv.Montante, 1);
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

            foreach (var tv in tviag.Where(x => x.Montante == 66 || x.Montante == 83))
            {
                int estagio = 1;
                var horas = tv.TempoViag;
                var dataAnt = data.AddHours(-horas);
                pasta = GetDirBase(dataAnt);
                if (pasta != "")
                {
                    for (int i = 0; i < 24; i++)
                    {
                        valor = GetDefluencia(pasta, tv.Montante, estagio);
                        var defline = new Compass.CommomLibrary.Deflant.DefLine();
                        defline.Montante = tv.Montante;
                        defline.Jusante = tv.Jusante;
                        defline.Tipo = tv.TipoJus;
                        defline.Diainic = dataAnt.Day;
                        defline.Horainic = i;
                        defline.Meiainic = 0;
                        defline.Diafim = " F";
                        defline.Defluencia = valor;
                        deflant.BlocoDef.Add(defline);

                        estagio++;
                        valor = GetDefluencia(pasta, tv.Montante, estagio);
                        var defline2 = new Compass.CommomLibrary.Deflant.DefLine();
                        defline2.Montante = tv.Montante;
                        defline2.Jusante = tv.Jusante;
                        defline2.Tipo = tv.TipoJus;
                        defline2.Diainic = dataAnt.Day;
                        defline2.Horainic = i;
                        defline2.Meiainic = 1;
                        defline2.Diafim = " F";
                        defline2.Defluencia = valor;
                        deflant.BlocoDef.Add(defline2);
                        estagio++;
                    }
                }

            }

            deflant.SaveToFile();
        }

        public static float GetDefluencia(string pastabase, int usn, int est)
        {
            var arqName = Directory.GetFiles(pastabase).Where(x => Path.GetFileName(x).ToLower().Contains("pdo_oper_usih.dat")).First();
            if (File.Exists(arqName))
            {
                var pdoOper = File.ReadAllLines(arqName);

                List<Tuple<int, int, float, float>> UHS = new List<Tuple<int, int, float, float>>();


                for (int i = 62; i < pdoOper.Count(); i++)
                {

                    if (pdoOper[i] != "")
                    {
                        float d = 0;
                        var campos = pdoOper[i].Split(';').ToList();
                        var hora = Convert.ToInt32(campos[0]);
                        var usina = Convert.ToInt32(campos[2]);
                        var qtur = float.TryParse(campos[20], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out d) ? d : 0;
                        var qver = float.TryParse(campos[24], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out d) ? d : 0;
                        UHS.Add(new Tuple<int, int, float, float>(hora, usina, qtur, qver));
                    }

                }
                var dadosUh = UHS.Where(x => x.Item1 == est && x.Item2 == usn).FirstOrDefault();
                if (dadosUh != null)
                {
                    float valor = dadosUh.Item3 + dadosUh.Item4;
                    return valor;
                }
            }
            return 0;
        }

        public static void TrataRespot(string path, DateTime dataEstudo)
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
                newL.Reserva = GetRespotValor(tm.DiaInicial, tm.HoraDiaInicial, tm.MeiaHora, blocoDp);
                respot.BlocoLm.Add(newL);
            }
            respot.SaveToFile();
        }



        public static float GetRespotValor(string dia, int hora, int meia, Compass.CommomLibrary.EntdadosDat.DpBlock blocoDp)
        {
            float valor = 0;

            var linhasDp = blocoDp.Where(x => x.DiaInic == dia && x.HoraInic == hora && x.MeiaHoraInic == meia).Select(x => x.Demanda).Sum();
            valor = linhasDp * 0.05f;
            return valor;
        }

        public static string GetDirBase(DateTime dataRef)
        {
            string pasta = "";

            DateTime dat = dataRef;
            DateTime datVE = dataRef;
            if (dat.DayOfWeek == DayOfWeek.Friday)
            {
                datVE = dat.AddDays(-1);
            }
            var rev = Tools.GetCurrRev(datVE);
            //Z:\7_dessem\Arquivos_base\2021\02_fev\RV1\20210206
            var mes = Tools.GetMonthNumAbrev(rev.revDate.Month);
            var dir = $@"K:\5_dessem\Arquivos_base\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\{dataRef:yyyyMMdd}";
            if (Directory.Exists(dir))
            {
                pasta = dir;

            }
            return pasta;
        }

        public static string GetPastaRecente(DateTime dataRef)
        {
            string pasta = "";

            int cont = 0;
            bool Ok = false;
            while (Ok == false && cont < 30)
            {
                DateTime dat = dataRef;
                DateTime datVE = dataRef;
                if (dat.DayOfWeek == DayOfWeek.Friday)
                {
                    datVE = dat.AddDays(-1);
                }
                var rev = Tools.GetCurrRev(datVE);
                //H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\2021\01_jan\RV3\DS_CCEE_012021_SEMREDE_RV3D19
                var mes = Tools.GetMonthNumAbrev(rev.revDate.Month);//dataRef
                var cam = $@"H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
                if (Directory.Exists(cam))
                {
                    pasta = cam;
                    Ok = true;
                }
                else
                {
                    cont++;
                    dataRef = dataRef.AddDays(-1);
                }
            }

            return pasta;
        }

        public static List<Tuple<int, float>> GetDadosRhe(Compass.CommomLibrary.Dadger.Dadger dadger)
        {
            // aqui são coletados alguns dados de restrições  para serem usados no entdados


            List<int> RheColeta = new List<int> { 403, 427, 417, 405, 415, 413, 445, 431, 419 };
            List<Tuple<int, float>> dadosRheColet = new List<Tuple<int, float>>();
            foreach (var rhe in RheColeta)
            {
                var lu = dadger.BlocoRhe.Where(x => x is Compass.CommomLibrary.Dadger.LuLine && x.Restricao == rhe).FirstOrDefault();
                if (lu != null)
                {
                    var valor = lu[4];
                    dadosRheColet.Add(new Tuple<int, float>(rhe, (float)valor));
                }
            }
            return dadosRheColet;
        }
        public static Compass.CommomLibrary.EntdadosDat.CiceBlock AjustaBlocoCICE(Compass.CommomLibrary.Dadger.Dadger dadger)
        {
            Dictionary<int, List<int>> usinaCont = new Dictionary<int, List<int>>()
            {
                {36,new List<int>{436, 437} },//maranhao IV
                {21,new List<int>{421, 422} },//maranhao V
                {46,new List<int>{ 446, 447} },//nova venecia
                {60,new List<int>{ 171, 172, 173, 174} },//norte fluminense
                {383,new List<int>{ 65, 183} },//atlantico
            };

            #region ajustaBlocoCICE

            //aqui é criado um bloco auxiliar usando alguns dados do dadger pra futuramente ser adicionado ao entdados 
            string pathBlocos = "H:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base\\BlocosFixos";
            //string pathBlocos = "N:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base\\BlocosFixos";
            var ciceLines = File.ReadAllLines(Path.Combine(pathBlocos, "blocoCICE.txt"));

            var blocoCICE = new Compass.CommomLibrary.EntdadosDat.CiceBlock();
            var blocoCT = dadger.BlocoCT;

            string comments = null;
            foreach (var cice in ciceLines.ToList())
            {
                if (cice.StartsWith("&"))
                {
                    comments = comments == null ? cice : comments + Environment.NewLine + cice;
                }
                else
                {
                    var newL = blocoCICE.CreateLine(cice);
                    newL.Comment = comments;
                    comments = null;
                    blocoCICE.Add(newL);
                }
            }
            foreach (var line in blocoCICE.Where(x => x.IdBloco == "CI").ToList())
            {
                var lineCt = blocoCT.Where(x => x[1] == line.IdContrato && x[4] == 1).FirstOrDefault();
                if (lineCt != null)
                {
                    line.EnergiaMin = (float)lineCt[5];
                    line.EnergiaMax = (float)lineCt[6];
                    line.Preco = (float)lineCt[7];
                }
            }
            foreach (var line in blocoCICE.Where(x => x.IdBloco == "CE").ToList())
            {
                float valor = 0f;
                foreach (var item in usinaCont[line.IdContrato])
                {
                    valor += blocoCICE.Where(x => x.IdContrato == item).Select(x => x.EnergiaMax).First();
                }

                line.EnergiaMin = 0.0f;
                line.Preco = 00.00f;
                line.EnergiaMax = valor;

            }

            var ciceLines2 = File.ReadAllLines(Path.Combine(pathBlocos, "blocoCICE2.txt"));
            foreach (var cice2 in ciceLines2.ToList())
            {
                if (cice2.StartsWith("&"))
                {
                    comments = comments == null ? cice2 : comments + Environment.NewLine + cice2;
                }
                else
                {
                    var newL = blocoCICE.CreateLine(cice2);
                    newL.Comment = comments;
                    comments = null;
                    blocoCICE.Add(newL);
                }
            }

            #endregion



            #region ajustadadger

            //aqui são adaptados alguns dados de identificação no dadger para a criação do entdados pelo decodess 


            foreach (var line in dadger.BlocoAc.ToList())
            {
                if (line.Usina == 285 || line.Usina == 287)
                {
                    if (line.Mnemonico.Contains("JUSMED") || line.Mnemonico.Contains("COTVOL"))
                    {
                        line[0] = "&" + line[0];
                    }
                }
            }


            for (int u = 171; u <= 174; u++)
            {
                foreach (var line in dadger.BlocoCT.Where(x => x.Cod == u).ToList())
                {
                    if (line.Cod == u)
                    {

                        line.Cvu1 = 0;
                        line.Cvu2 = 0;
                        line.Cvu3 = 0;
                        line.Cod = 60;
                    }
                }
            }

            foreach (var line in dadger.BlocoCT.Where(x => x.Cod == 183 || x.Cod == 65).ToList())
            {
                line.Cvu1 = 0;
                line.Cvu2 = 0;
                line.Cvu3 = 0;
                line.Cod = 383;

            }

            foreach (var line in dadger.BlocoCT.Where(x => x.Cod == 436 || x.Cod == 437).ToList())
            {
                line.Cvu1 = 0;
                line.Cvu2 = 0;
                line.Cvu3 = 0;
                line.Cod = 36;

            }

            foreach (var line in dadger.BlocoCT.Where(x => x.Cod == 421 || x.Cod == 422).ToList())
            {
                line.Cvu1 = 0;
                line.Cvu2 = 0;
                line.Cvu3 = 0;
                line.Cod = 21;

            }

            foreach (var line in dadger.BlocoCT.Where(x => x.Cod == 446 || x.Cod == 447).ToList())
            {
                line.Cvu1 = 0;
                line.Cvu2 = 0;
                line.Cvu3 = 0;
                line.Cod = 46;

            }

            foreach (var line in dadger.BlocoCT.Where(x => x.Cod == 463).ToList())
            {

                line[0] = "&" + line[0];

            }
            var RES = dadger.BlocoRhe.ToList();
            foreach (var chave in usinaCont.Keys.ToList())
            {
                foreach (var item in usinaCont[chave])
                {
                    var FTline = RES.Where(x => x is Compass.CommomLibrary.Dadger.FtLine && x[3] == item).FirstOrDefault();
                    if (FTline != null)
                    {
                        FTline[3] = chave;

                    }
                }
            }


            for (int i = 401; i <= 431; i++)
            {
                foreach (var rhe in dadger.BlocoRhe.RheGrouped.Where(x => x.Key[1] == i))
                {
                    foreach (var rh in rhe.Value)
                    {
                        rh[0] = "&" + rh[0];
                    }
                }
            }
            for (int i = 435; i <= 654; i++)
            {
                if (i != 650)
                {
                    foreach (var rhe in dadger.BlocoRhe.RheGrouped.Where(x => x.Key[1] == i))
                    {
                        foreach (var rh in rhe.Value)
                        {
                            rh[0] = "&" + rh[0];
                        }
                    }
                }

            }


            #endregion

            return blocoCICE;
        }

    }
}

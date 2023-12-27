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
using Compass.ExcelTools;
using Compass.Services;
using Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace Compass.Services
{
    public class DessemComplSem
    {

        public static void CriarDadvazSemanal(string path, int incremento, DateTime dateBase, DateTime dataFim, DateTime data)
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

            //foreach (var vaz in vazoes)
            //{
            //    if (vaz.Usina == 21)
            //    {

            //    }
            //    int diateste = vaz.DiaInic.Trim().ToUpper() == "I" ? dateBase.Day : Convert.ToInt32(vaz.DiaInic);
            //    float vazao = vaz.Vazao;

            //    DateTime dataTest;
            //    if (diateste < dateBase.Day)//trata viradas de meses para fazer as comparaçoes com datas 
            //    {
            //        dataTest = new DateTime(dateBase.AddMonths(1).Year, dateBase.AddMonths(1).Month, diateste);
            //    }
            //    else
            //    {
            //        dataTest = new DateTime(dateBase.Year, dateBase.Month, diateste);
            //    }

            //    int regs = vazoes.Where(x => x.Usina == vaz.Usina).ToList().Count();

            //    if (regs == 1)//só existe uma linha logo só e necessario ajustar a data 
            //    {
            //        vaz.DiaInic = $"{data.Day:00}";// altera o dia de acordo com a data do deck
            //    }
            //    else if (dataTest < data)
            //    {
            //        var vazSeg = vazoes.Where(x => x.Usina == vaz.Usina && Convert.ToInt32(x.DiaInic) == dataTest.AddDays(1).Day).FirstOrDefault();
            //        if (vazSeg == null)
            //        {
            //            var newVaz = new CommomLibrary.Dadvaz.VazoesLine();
            //            newVaz.DiaInic = $"{dataTest.AddDays(1).Day:00}";
            //            newVaz.DiaFinal = $"F";
            //            newVaz.Usina = vaz.Usina;
            //            newVaz.Nome = vaz.Nome;
            //            newVaz.TipoVaz = vaz.TipoVaz;
            //            newVaz.Vazao = vaz.Vazao;

            //            dadvaz.BlocoVazoes.InsertAfter(vaz, newVaz);
            //            dadvaz.BlocoVazoes.Remove(vaz);
            //        }
            //        else if (dataTest < data)
            //        {
            //            dadvaz.BlocoVazoes.Remove(vaz);
            //        }
            //    }




            //    //dataTest = dataTest.AddDays(incremento);

            //    //if (dataTest > dataFim)
            //    //{
            //    //    dadvaz.BlocoVazoes.Remove(vaz);//remove linhas com data alem do horizonte do periodo
            //    //    dadvaz.BlocoVazoes.inser
            //    //}
            //    //else
            //    //{
            //    //    vaz.DiaInic = $"{dataTest.Day:00}";// altera o dia de acordo com a data do deck
            //    //}


            //    //else //apenas vai apagar os dias passados 
            //    //{
            //    //    if (dataTest < data)
            //    //    {
            //    //        dadvaz.BlocoVazoes.Remove(vaz);
            //    //    }
            //    //}
            //    /////////////
            //    //if (data.Day >= 20)
            //    //{
            //    //    if (diateste < data.Day && diateste > 10)
            //    //    {
            //    //        dadvaz.BlocoVazoes.Remove(vaz);
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    if (diateste < data.Day)
            //    //    {
            //    //        dadvaz.BlocoVazoes.Remove(vaz);
            //    //    }
            //    //    if (data.Day < 10 && diateste > 20)
            //    //    {
            //    //        dadvaz.BlocoVazoes.Remove(vaz);
            //    //    }
            //    //}
            //}

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

        public static void CriarDeflant(string path, int incremento, DateTime dateDeck, DateTime dataEstudo)
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
                        arqNP = Tools.GetNPTXT(dataAnt);
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

            arqNP = Tools.GetNPTXT(dataEstudo.AddHours(-tv83), true);
            float valor83 = Tools.GetNPValue(arqNP, "83");

            foreach (var line in deflant.BlocoDef.Where(x => x.Montante == 66 || x.Montante == 83).ToList())// as usinas 66 itaipu e 83 baixo iguaçu são preenchidas de forma diferente pq abri em 48 estagios 
            {
                //usina 66 só muda o dia inicial e usa os dados do deck base, usina 83 usa os dados do NP mais recente de acordo com a data do deck sendo criada
                if (line.Diainic > dateDeck.Day)//significa que o dia do deflant base é do mes passado porque aqui olho só o dia 
                {
                    var datRef = new DateTime(dateDeck.AddMonths(-1).Year, dateDeck.AddMonths(-1).Month, line.Diainic);
                    datRef = datRef.AddDays(incremento);//trata virada de meses
                    line.Diainic = datRef.Day;
                    if (line.Montante == 83)
                    {
                        line.Defluencia = valor83;
                    }
                }
                else
                {
                    var datRef = new DateTime(dateDeck.Year, dateDeck.Month, line.Diainic);
                    datRef = datRef.AddDays(incremento);//trata virada de meses
                    line.Diainic = datRef.Day;
                    if (line.Montante == 83)
                    {
                        line.Defluencia = valor83;
                    }
                }
            }





            #region codigo antigo

            //foreach (var line in deflant.BlocoDef.ToList())
            //{
            //    if (line.Diainic > dateDeck.Day)//significa que o dia do deflant base é do mes passado porque aqui olho só o dia 
            //    {
            //        var datRef = new DateTime(dateDeck.AddMonths(-1).Year, dateDeck.AddMonths(-1).Month, line.Diainic);
            //        datRef = datRef.AddDays(incremento);//trata virada de meses
            //        line.Diainic = datRef.Day;

            //    }
            //    else
            //    {
            //        var datRef = new DateTime(dateDeck.Year, dateDeck.Month, line.Diainic);
            //        datRef = datRef.AddDays(incremento);//trata virada de meses
            //        line.Diainic = datRef.Day;
            //    }
            //}

            #endregion


            deflant.SaveToFile(createBackup: true);
        }

        public static void CriarOperut(string path, int incremento, DateTime dataBase, DateTime dataEstudo)
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
        }


        public static void CriarPtoper(string path, int incremento, DateTime dataBase)
        {
            var ptoperFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("ptoper")).FirstOrDefault();

            var ptoper = DocumentFactory.Create(ptoperFile) as Compass.CommomLibrary.PtoperDat.PtoperDat;
            foreach (var line in ptoper.BlocoPtoper.ToList())
            {
                var datRef = new DateTime(dataBase.Year, dataBase.Month, Convert.ToInt32(line.DiaIni));
                datRef = datRef.AddDays(incremento);
                line.DiaIni = datRef.Day.ToString();
            }

            ptoper.SaveToFile(createBackup: true);
        }

        public static void CriarOperuh(string path, int incremento, DateTime dataBase, DateTime dataEstudo, DateTime fimrev)
        {
            var operuhFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("operuh")).First();
            var operuh = DocumentFactory.Create(operuhFile) as Compass.CommomLibrary.Operuh.Operuh;

            //novo codigo 
            var rhesOperuhG = operuh.BlocoRhest.RhestGrouped.ToList();

            foreach (var rhes in rhesOperuhG.ToList())
            {

                Compass.CommomLibrary.Operuh.RhestLine rh;
                string restricao = rhes.Value.First().Restricao;
                string texto = "";
                bool existeInicial = true;

                if (restricao == "05074")
                {

                }

                var restricoesPorNum = operuh.BlocoRhest.Where(x => x.Restricao == restricao).ToList();
                var restricoesCount = restricoesPorNum.Where(x => x is Compass.CommomLibrary.Operuh.LimLine || x is Compass.CommomLibrary.Operuh.VarLine).ToList();

                if (restricoesCount.Count() == 1 && restricoesCount.First().DiaInic.Trim() == "I")
                {
                    var rhF = restricoesCount.First();
                    if (rhF.DiaFinal.Trim() == "F")
                    {
                        continue;
                    }
                    else
                    {
                        DateTime dataInicial = new DateTime(dataBase.Year, dataBase.Month, rhF.DiaInic.Trim() == "I" ? dataBase.Day : Convert.ToInt32(rhF.DiaInic));
                        int meiaIni = rhF.MeiaHoraInic ?? 0;
                        dataInicial = dataInicial.AddHours(rhF.HoraInic ?? 0).AddMinutes(meiaIni == 0 ? 0 : 30);

                        DateTime dataFinal = new DateTime(dataBase.Year, dataBase.Month, rhF.DiaFinal.Trim() == "F" ? fimrev.Day : Convert.ToInt32(rhF.DiaFinal));
                        int meiaFinal = rhF.MeiaHoraFinal ?? 0;
                        dataFinal = dataFinal.AddHours(rhF.HoraFinal ?? 0).AddMinutes(meiaFinal == 0 ? 0 : 30);

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
                        TimeSpan ts = dataFinal - dataInicial;
                        var difHoras2 = ts.TotalHours;
                        dataFinal = dataEstudo.AddHours(difHoras2);

                        if (dataFinal > fimrev)
                        {
                            dataFinal = fimrev.AddDays(1);
                        }


                        rhF.DiaFinal = dataFinal.Day.ToString("D2");
                        rhF.HoraFinal = dataFinal.Hour;
                        rhF.MeiaHoraFinal = dataFinal.Minute == 30 ? 1 : 0;
                        continue;
                    }
                }

                foreach (var rhe in rhes.Value)
                {
                    if (rhe is Compass.CommomLibrary.Operuh.LimLine || rhe is Compass.CommomLibrary.Operuh.VarLine)
                    {
                        //var re = rhe.Value.First();
                        if (Convert.ToInt32(rhe.Restricao) == 656)
                        {

                        }
                        if (Convert.ToInt32(rhe.Restricao) == 181)
                        {

                        }
                        DateTime dataInicial = new DateTime(dataBase.Year, dataBase.Month, rhe.DiaInic.Trim() == "I" ? dataBase.Day : Convert.ToInt32(rhe.DiaInic));
                        int meiaIni = rhe.MeiaHoraInic ?? 0;
                        dataInicial = dataInicial.AddHours(rhe.HoraInic ?? 0).AddMinutes(meiaIni == 0 ? 0 : 30);

                        DateTime dataFinal = new DateTime(dataBase.Year, dataBase.Month, rhe.DiaFinal.Trim() == "F" ? fimrev.Day : Convert.ToInt32(rhe.DiaFinal));
                        int meiaFinal = rhe.MeiaHoraFinal ?? 0;
                        dataFinal = dataFinal.AddHours(rhe.HoraFinal ?? 0).AddMinutes(meiaFinal == 0 ? 0 : 30);

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

                        TimeSpan ts = dataFinal - dataInicial;
                        var difHoras2 = ts.TotalHours;
                        //foreach (var rh in rhe.Value)
                        //{

                        if (rhe.DiaFinal.Trim() == "F")//TODO ver se dia ini é menor que data do deck prar poder apagar
                        {
                            //if (rhe.DiaInic.Trim() != "I")
                            //{
                            //    if (dataInicial < dataEstudo)
                            //    {
                            //        operuh.BlocoRhest.Remove(rhe);

                            //    }
                            //}
                            continue;
                        }
                        var rheInicial = rhes.Value.Where(x => x.DiaInic.Trim() != "I").Where(x => x.Minemonico == rhe.Minemonico && Convert.ToInt32(x.DiaInic) == dataEstudo.Day && x.HoraInic == 0 && x.MeiaHoraInic == 0).FirstOrDefault();
                        if (dataFinal <= dataEstudo)
                        {
                            //rh = rhe.Clone() as Compass.CommomLibrary.Operuh.RhestLine;
                            texto = rhe.ToText();
                            if (rheInicial != null)
                            {
                                existeInicial = true;
                            }
                            else
                            {
                                existeInicial = false;
                            }
                            operuh.BlocoRhest.Remove(rhe);

                        }
                        var rheDiaInic_I = rhes.Value.Where(x => x.DiaInic.Trim() == "I").Where(x => x.Minemonico == rhe.Minemonico).FirstOrDefault();
                        if (rheDiaInic_I != null)
                        {
                            existeInicial = true;
                        }
                        //if (Convert.ToInt32(rhe.DiaFinal) <= dataEstudo.Day)
                        //{
                        //    DateTime dat = dataEstudo;
                        //    if (dataFinal == dataEstudo)
                        //    {
                        //        dat = dat.AddDays(1);// trata diafinal hora = 0 meiahora = 0 somando 1 dia ja que dataestudo ja vem como zero na hora e meiahora evitando deixar dia ini = dia final na incrementação  
                        //    }
                        //    int minutosF = rhe.MeiaHoraFinal ?? 0;
                        //    dat = dat.AddHours(rhe.HoraFinal ?? 0).AddMinutes(minutosF == 0 ? 0 : 30);
                        //    if (dat > fimrev.AddDays(1))
                        //    {
                        //        dat = fimrev.AddDays(1);// limita pelo fim da semana caso estoure a periodo da semana
                        //    }
                        //    if (dat == dataEstudo)
                        //    {
                        //        dat = dat.AddDays(1);//tratamento caso diafinal fique igual inicial no caso do decks do ultimo dia da semana (hora ini = 0 meia ini = 0 hora fim =0 meiafim =0 ), então fica diafinal como o inicio do sabado seguinte
                        //    }
                        //    rhe.DiaFinal = dat.Day.ToString("D2");
                        //    rhe.HoraFinal = dat.Hour;
                        //    rhe.MeiaHoraFinal = dat.Minute == 30 ? 1 : 0;

                        //    rhe.DiaInic = dat.AddHours(-difHoras2).Day.ToString("D2");
                        //    rhe.HoraInic = dat.AddHours(-difHoras2).Hour;
                        //    rhe.MeiaHoraInic = dat.AddHours(-difHoras2).Minute == 30 ? 1 : 0;
                        //}
                        restricoesPorNum = operuh.BlocoRhest.Where(x => x.Restricao == restricao).ToList();
                        restricoesCount = restricoesPorNum.Where(x => x is Compass.CommomLibrary.Operuh.LimLine || x is Compass.CommomLibrary.Operuh.VarLine).ToList();
                        if (restricoesCount.Count() == 0)//verifica se a restrição não possui nenhuma linha do tipo Lim ou Var caso a contagem seja 0 cria uma linha para dia inicial com o mesmo range de horas do deck base
                        {
                            var rheClone = operuh.BlocoRhest.CreateLine(texto);

                            rheClone.DiaInic = dataEstudo.Day.ToString("D2");
                            rheClone.HoraInic = dataInicial.Hour;
                            rheClone.MeiaHoraInic = dataInicial.Minute == 30 ? 1 : 0;
                            DateTime novafinal = dataEstudo.AddHours(dataInicial.Hour).AddMinutes(dataInicial.Minute);
                            DateTime novafinalAD = novafinal.AddHours(difHoras2);

                            if (novafinal == novafinalAD)
                            {
                                novafinal = fimrev.AddDays(1);
                            }
                            else
                            {
                                novafinal = novafinalAD;
                            }

                            rheClone.DiaFinal = novafinal.Day.ToString("D2");
                            rheClone.HoraFinal = novafinal.Hour;
                            rheClone.MeiaHoraFinal = novafinal.Minute == 30 ? 1 : 0;

                            operuh.BlocoRhest.InsertAfter(restricoesPorNum.Last(), rheClone);
                            existeInicial = true;
                            //restricoesPorNum.ForEach(x => operuh.BlocoRhest.Remove(x));
                        }
                        else if (restricoesCount.Count() == 1 && restricoesCount.First().DiaInic.Trim() == "I")
                        {
                            var rheClone = operuh.BlocoRhest.CreateLine(texto);

                            rheClone.DiaInic = dataEstudo.Day.ToString("D2");
                            rheClone.HoraInic = dataInicial.Hour;
                            rheClone.MeiaHoraInic = dataInicial.Minute == 30 ? 1 : 0;
                            DateTime novafinal = dataEstudo.AddHours(dataInicial.Hour).AddMinutes(dataInicial.Minute);
                            DateTime novafinalAD = novafinal.AddHours(difHoras2);

                            if (novafinal == novafinalAD)
                            {
                                novafinal = fimrev.AddDays(1);
                            }
                            else
                            {
                                novafinal = novafinalAD;
                            }

                            rheClone.DiaFinal = novafinal.Day.ToString("D2");
                            rheClone.HoraFinal = novafinal.Hour;
                            rheClone.MeiaHoraFinal = novafinal.Minute == 30 ? 1 : 0;

                            operuh.BlocoRhest.InsertAfter(restricoesCount.First(), rheClone);
                            existeInicial = true;
                        }


                    }

                }



                if (existeInicial == false)// caso não exista linha com dia inicial I ou com o dia inicial = dia do deck 00 hora 0 meia hora
                {
                    var rheClone = operuh.BlocoRhest.CreateLine(texto);
                    var rhePrimeira = operuh.BlocoRhest.Where(x => x.Restricao == rheClone.Restricao && x.Minemonico == rheClone.Minemonico).FirstOrDefault();
                    if (rhePrimeira != null)
                    {
                        rheClone.DiaInic = dataEstudo.Day.ToString("D2");
                        rheClone.HoraInic = 00;
                        rheClone.MeiaHoraInic = 0;

                        rheClone.DiaFinal = rhePrimeira.DiaInic;
                        rheClone.HoraFinal = rhePrimeira.HoraInic;
                        rheClone.MeiaHoraFinal = rhePrimeira.MeiaHoraInic;

                        operuh.BlocoRhest.InsertBefore(rhePrimeira, rheClone);

                    }
                }
                //}


            }


            //fim novo codigo

            //List<int> restricoes = new List<int> { 116, 979, 962, 1743, 1744, 1860, 1861, 99116 };
            //var restsLinesVerif = operuh.BlocoRhest.Where(x => x is Compass.CommomLibrary.Operuh.LimLine || x is Compass.CommomLibrary.Operuh.VarLine).ToList();

            //foreach (var rest in restricoes)
            //{
            //    var lines = restsLinesVerif.Where(x => Convert.ToInt32(x.Restricao.Trim()) == rest).ToList();
            //    for (int i = 1; i <= incremento; i++)
            //    {
            //        int diaRef = dataEstudo.AddDays(-i).Day;

            //        foreach (var l in lines)
            //        {
            //            string di = l[3];
            //            string df = l[6];
            //            if (di.Trim() != "I")
            //            {
            //                if (l == lines.Last() && df.Trim() == "F" && Convert.ToInt32(di.Trim()) < dataEstudo.Day)
            //                {
            //                    l[3] = $"{dataEstudo.Day:00}";
            //                    l[4] = 00;
            //                    l[5] = 0;
            //                }
            //                else if (Convert.ToInt32(di.Trim()) == diaRef)
            //                {
            //                    operuh.BlocoRhest.Remove(l);
            //                }
            //            }

            //        }
            //    }

            //    var restricoesPorNum = operuh.BlocoRhest.Where(x => Convert.ToInt32(x.Restricao) == rest).ToList();
            //    var restricoesCount = restricoesPorNum.Where(x => x is Compass.CommomLibrary.Operuh.LimLine || x is Compass.CommomLibrary.Operuh.VarLine).ToList();
            //    if (restricoesCount.Count() == 0)//verifica se a restrição não possui nenhuma linha do tipo Lim ou Var caso a contagem seja 0 apaga a restrição
            //    {
            //        restricoesPorNum.ForEach(x => operuh.BlocoRhest.Remove(x));
            //    }
            //}
            //restsLinesVerif.Clear();// limpa a lista para varrer o operuh novamente 
            //restsLinesVerif = operuh.BlocoRhest.Where(x => x is Compass.CommomLibrary.Operuh.LimLine || x is Compass.CommomLibrary.Operuh.VarLine).ToList();

            //foreach (var lines in restsLinesVerif)
            //{
            //    if (restricoes.All(x => x != Convert.ToInt32(lines.Restricao)))
            //    {
            //        string di = lines[3];
            //        string df = lines[6];
            //        if (di.Trim() != "I")
            //        {
            //            int diaref = Convert.ToInt32(di.Trim());
            //            if (diaref < dataBase.Day)
            //            {
            //                DateTime datRef = new DateTime(dataBase.AddMonths(1).Year, dataBase.AddMonths(1).Month, diaref);
            //                datRef = datRef.AddDays(incremento);
            //                lines[3] = $"{datRef.Day:00}";
            //            }
            //            else
            //            {
            //                DateTime datRef = new DateTime(dataBase.Year, dataBase.Month, diaref);
            //                datRef = datRef.AddDays(incremento);
            //                lines[3] = $"{datRef.Day:00}";
            //            }
            //        }
            //        if (df.Trim() != "F")
            //        {
            //            int diaref = Convert.ToInt32(df.Trim());
            //            if (diaref < dataBase.Day)
            //            {
            //                DateTime datRef = new DateTime(dataBase.AddMonths(1).Year, dataBase.AddMonths(1).Month, diaref);
            //                datRef = datRef.AddDays(incremento);
            //                lines[6] = $"{datRef.Day:00}";
            //            }
            //            else
            //            {
            //                DateTime datRef = new DateTime(dataBase.Year, dataBase.Month, diaref);
            //                datRef = datRef.AddDays(incremento);
            //                lines[6] = $"{datRef.Day:00}";
            //            }
            //        }
            //    }
            //}
            operuh.SaveToFile(createBackup: true);

        }

        public static void CriarEntdados(string path, int incremento, DateTime dataBase, DateTime dataEstudo, bool expandEst, DateTime fimrev)
        {
            var entdadosFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;


            #region BLOCO TM
            bool patamres2023 = dataEstudo.Year == 2023;
            bool patamares2024 = dataEstudo.Year >= 2024;

            var intervalos = Tools.GetIntervalosHoararios(dataEstudo, patamres2023, patamares2024);
            string comentario = entdados.BlocoTm.First().Comment;
            for (DateTime d = dataEstudo.AddDays(-7); d <= dataEstudo; d = d.AddDays(1))
            {
                foreach (var line in entdados.BlocoTm.ToList())
                {
                    var dia = Convert.ToInt32(line.DiaInicial);
                    if (d != dataEstudo)
                    {
                        if (dia == d.Day)
                        {
                            entdados.BlocoTm.Remove(line);
                        }
                    }
                    if (d == dataEstudo && expandEst)
                    {
                        if (dia == d.Day)
                        {
                            entdados.BlocoTm.Remove(line);
                        }
                    }
                }

            }

            if (expandEst)
            {
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
            }


            #endregion

            #region BLOCO UT
            foreach (var utline in entdados.BlocoUt.ToList())
            {
                DateTime datUt = new DateTime(dataBase.Year, dataBase.Month, Convert.ToInt32(utline.DiaInic));
                datUt = datUt.AddDays(incremento);
                utline.DiaInic = datUt.Day.ToString();
            }


            #endregion

            #region BLOCO RI

            var riLines = entdados.BlocoRi.ToList();
            for (int i = 1; i <= incremento; i++)
            {
                int diaRef = dataEstudo.AddDays(-i).Day;

                foreach (var ri in riLines)
                {
                    string di = ri.DiaInic;
                    if (di.Trim() != "I")
                    {
                        if (Convert.ToInt32(di.Trim()) == diaRef)
                        {
                            entdados.BlocoRi.Remove(ri);
                        }
                    }

                }
            }


            #endregion

            #region BLOCO CD

            foreach (var cdLine in entdados.BlocoCd.ToList())
            {
                cdLine.DiaInic = $"{dataEstudo.Day:00}";
            }

            #endregion

            #region BLOCO VE

            foreach (var veLine in entdados.BlocoVe.ToList())
            {
                veLine.DiaInic = $"{dataEstudo.Day:00}";
                veLine.DiaFinal = $"{dataEstudo.AddDays(1).Day:00}";
            }

            #endregion

            #region BLOCO DA

            foreach (var daLine in entdados.BlocoDa.ToList())
            {
                daLine.DiaInic = $"{dataEstudo.Day:00}";
            }

            #endregion

            #region BLOCO RHE

            //foreach (var rhe in entdados.BlocoRhe.RheGrouped.ToList())
            //{
            //    var re = rhe.Value.First();
            //    if (re.Restricao == 656)
            //    {

            //    }
            //    if (re.Restricao == 655)
            //    {

            //    }
            //    DateTime dataInicial = new DateTime(dataBase.Year,dataBase.Month, re.DiaInic.Trim() == "I"? dataBase.Day: Convert.ToInt32(re.DiaInic));
            //    int meiaIni = re.MeiaHoraInic ?? 0;
            //    dataInicial = dataInicial.AddHours(re.HoraInic ?? 0).AddMinutes(meiaIni == 0 ? 0 : 30);

            //    DateTime dataFinal = new DateTime(dataBase.Year, dataBase.Month, re.DiaFinal.Trim() == "F" ? fimrev.Day : Convert.ToInt32(re.DiaFinal));
            //    int meiaFinal = re.MeiaHoraFinal ?? 0;
            //    dataFinal = dataFinal.AddHours(re.HoraFinal ?? 0).AddMinutes(meiaFinal == 0 ? 0 : 30);

            //    //ajustando viradas de meses 
            //    if (dataBase.Day < 10)
            //    {
            //        if (dataInicial.Day>20)
            //        {
            //            dataInicial = dataInicial.AddMonths(-1);
            //        }
            //    }
            //    if (dataBase.Day> dataFinal.Day)
            //    {
            //        dataFinal = dataFinal.AddMonths(1);
            //    }
            //    //fim ajuste de meses
            //    TimeSpan ts = dataFinal - dataInicial;
            //    var difHoras2 = ts.TotalHours;
            //    foreach (var rh in rhe.Value)
            //    {
            //        if (rh.DiaFinal.Trim() == "F")
            //        {
            //            continue;
            //        }
            //        if (Convert.ToInt32(rh.DiaFinal) <= dataEstudo.Day)
            //        {
            //            DateTime dat = dataEstudo;
            //            int minutosF = rh.MeiaHoraFinal ?? 0;
            //            dat = dat.AddHours(rh.HoraFinal ?? 0).AddMinutes(minutosF == 0 ? 0 : 30);
            //            if (dat > fimrev.AddDays(1))
            //            {
            //                dat = fimrev.AddDays(1);
            //            }
            //            rh.DiaFinal = dat.Day.ToString("00");
            //            rh.HoraFinal = dat.Hour;
            //            rh.MeiaHoraFinal = dat.Minute == 30 ? 1 : 0;

            //            rh.DiaInic = dat.AddHours(difHoras2).Day.ToString("00");
            //            rh.HoraInic = dat.AddHours(difHoras2).Hour;
            //            rh.MeiaHoraInic = dat.AddHours(difHoras2).Minute == 30 ? 1 : 0;
            //        }
            //    }


            //}
            //
            foreach (var rhe in entdados.BlocoRhe.ToList())
            {
                //var re = rhe.Value.First();
                if (rhe.Restricao == 657)
                {

                }
                if (rhe.Restricao == 655)
                {

                }
                DateTime dataInicial = new DateTime(dataBase.Year, dataBase.Month, rhe.DiaInic.Trim() == "I" ? dataBase.Day : Convert.ToInt32(rhe.DiaInic));
                int meiaIni = rhe.MeiaHoraInic ?? 0;
                dataInicial = dataInicial.AddHours(rhe.HoraInic ?? 0).AddMinutes(meiaIni == 0 ? 0 : 30);

                DateTime dataFinal = new DateTime(dataBase.Year, dataBase.Month, rhe.DiaFinal.Trim() == "F" ? fimrev.Day : Convert.ToInt32(rhe.DiaFinal));
                int meiaFinal = rhe.MeiaHoraFinal ?? 0;
                dataFinal = dataFinal.AddHours(rhe.HoraFinal ?? 0).AddMinutes(meiaFinal == 0 ? 0 : 30);

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
                TimeSpan ts = dataFinal - dataInicial;
                var difHoras2 = ts.TotalHours;
                //foreach (var rh in rhe.Value)
                //{
                if (rhe.DiaFinal.Trim() == "F")
                {
                    if (dataEstudo.DayOfWeek == DayOfWeek.Friday && rhe.DiaInic.Trim() != "I")
                    {
                        rhe.DiaInic = dataEstudo.Day.ToString("D2");
                        rhe.HoraInic = dataEstudo.Hour;
                        rhe.MeiaHoraInic = dataEstudo.Minute == 30 ? 1 : 0;
                    }
                    continue;
                }
                //if (Convert.ToInt32(rhe.DiaFinal) <= dataEstudo.Day)
                if (dataFinal <= dataEstudo)
                {
                    DateTime dat = dataEstudo;
                    if (dataFinal == dataEstudo)
                    {
                        dat = dat.AddDays(1);// trata diafinal hora = 0 meiahora = 0 somando 1 dia ja que dataestudo ja vem como zero na hora e meiahora evitando deixar dia ini = dia final na incrementação  
                    }
                    int minutosF = rhe.MeiaHoraFinal ?? 0;
                    dat = dat.AddHours(rhe.HoraFinal ?? 0).AddMinutes(minutosF == 0 ? 0 : 30);
                    if (dat > fimrev.AddDays(1))
                    {
                        dat = fimrev.AddDays(1);// limita pelo fim da semana caso estoure a periodo da semana
                    }
                    if (dat == dataEstudo)
                    {
                        dat = dat.AddDays(1);//tratamento caso diafinal fique igual inicial no caso do decks do ultimo dia da semana (hora ini = 0 meia ini = 0 hora fim =0 meiafim =0 ), então fica diafinal como o inicio do sabado seguinte
                    }
                    rhe.DiaFinal = dat.Day.ToString("D2");
                    rhe.HoraFinal = dat.Hour;
                    rhe.MeiaHoraFinal = dat.Minute == 30 ? 1 : 0;

                    rhe.DiaInic = dat.AddHours(-difHoras2).Day.ToString("D2");
                    rhe.HoraInic = dat.AddHours(-difHoras2).Hour;
                    rhe.MeiaHoraInic = dat.AddHours(-difHoras2).Minute == 30 ? 1 : 0;
                    continue;
                }
                if (dataInicial < dataEstudo)
                {
                    rhe.DiaInic = dataEstudo.Day.ToString("D2");
                    rhe.HoraInic = dataEstudo.Hour;
                    rhe.MeiaHoraInic = dataEstudo.Minute == 30 ? 1 : 0;
                }
                //}


            }



            //
            //foreach (var rheLine in entdados.BlocoRhe.ToList())
            //{
            //    string rheDia = rheLine[2];
            //    if (rheDia.Trim() != "I")
            //    {
            //        rheLine[2] = $"{dataEstudo.Day:00}";
            //    }
            //}

            #endregion

            #region BLOCO R11

            foreach (var r11Line in entdados.BlocoR11.ToList())
            {
                r11Line.DiaInic = $"{dataEstudo.Day:00}";
            }

            #endregion

            #region BLOCO MT

            foreach (var mtLine in entdados.BlocoMt.ToList())
            {
                mtLine.DiaInic = $"{dataEstudo.Day:00}";
                mtLine.DiaFinal = $"{dataEstudo.AddDays(1).Day:00}";
            }

            #endregion

            #region BLOCO MH

            foreach (var mhLine in entdados.BlocoMh.ToList())
            {
                DateTime dataIniRef;
                DateTime dataFimRef;

                int diaIniREf = Convert.ToInt32(mhLine.DiaInic);
                int diafimREf = Convert.ToInt32(mhLine.DiaFinal);

                if (diaIniREf < dataBase.Day)
                {
                    dataIniRef = new DateTime(dataBase.AddMonths(1).Year, dataBase.AddMonths(1).Month, diaIniREf);
                }
                else
                {
                    dataIniRef = new DateTime(dataBase.Year, dataBase.Month, diaIniREf);
                }

                if (diafimREf < dataBase.Day)
                {
                    dataFimRef = new DateTime(dataBase.AddMonths(1).Year, dataBase.AddMonths(1).Month, diafimREf);
                }
                else
                {
                    dataFimRef = new DateTime(dataBase.Year, dataBase.Month, diafimREf);
                }

                if (dataIniRef < dataEstudo)
                {
                    mhLine.DiaInic = dataEstudo.Day.ToString();
                }

                if (dataFimRef < dataEstudo)
                {
                    entdados.BlocoMh.Remove(mhLine);
                }

                if (dataFimRef == dataEstudo)
                {
                    int horIni = mhLine.HoraInic;
                    int horFim = mhLine.HoraFinal;
                    if (horFim < horIni)
                    {
                        entdados.BlocoMh.Remove(mhLine);
                    }
                    else if (horFim == horIni)
                    {
                        if ((mhLine.MeiaHoraFinal == 0 && mhLine.MeiaHoraInic == 1) || (mhLine.MeiaHoraInic == mhLine.MeiaHoraFinal))
                        {
                            entdados.BlocoMh.Remove(mhLine);
                        }
                    }
                }
            }

            #endregion

            #region BLOCO DP
            //var dpLines = entdados.BlocoDp.ToList();

            if (expandEst)
            {//deixa os 48 estagios do primeiro dia para depois colocar a data do estudo e apaga os dias passados 
                for (DateTime d = dataBase.AddDays(1); d <= dataEstudo; d = d.AddDays(1))
                {
                    int diadpRef = d.Day;
                    foreach (var dpl in entdados.BlocoDp.ToList())
                    {
                        int diaDp = Convert.ToInt32(dpl.DiaInic);
                        if (diadpRef == diaDp)
                        {
                            entdados.BlocoDp.Remove(dpl);
                        }
                    }
                }
                foreach (var ndpl in entdados.BlocoDp.ToList())
                {
                    int diaAlvo = dataBase.Day;
                    int diaMudar = Convert.ToInt32(ndpl.DiaInic);
                    if (diaAlvo == diaMudar)
                    {
                        ndpl.DiaInic = dataEstudo.Day.ToString();
                    }
                }
            }
            else
            {//apaga todas as linhas com datas anteriores a data de estudo 

                for (DateTime d = dataBase; d < dataEstudo; d = d.AddDays(1))
                {
                    int diadpRef = d.Day;
                    foreach (var dpl in entdados.BlocoDp.ToList())
                    {
                        int diaDp = Convert.ToInt32(dpl.DiaInic);
                        if (diadpRef == diaDp)
                        {
                            if (dpl.Subsist == 11)
                            {
                                dpl.DiaInic = dataEstudo.Day.ToString();
                            }
                            else
                            {
                                entdados.BlocoDp.Remove(dpl);
                            }
                        }
                    }
                }
            }

            #endregion

            #region BLOCO DE

            if (expandEst)
            {//deixa os 48 estagios do primeiro dia para depois colocar a data do estudo e apaga os dias passados 
                for (DateTime d = dataBase.AddDays(1); d <= dataEstudo; d = d.AddDays(1))
                {
                    int diadeRef = d.Day;
                    foreach (var del in entdados.BlocoDe.ToList())
                    {
                        int diaDe = Convert.ToInt32(del.DiaInic);
                        if (diadeRef == diaDe)
                        {
                            entdados.BlocoDe.Remove(del);
                        }
                    }
                }
                foreach (var ndel in entdados.BlocoDe.ToList())
                {
                    int diaAlvo = dataBase.Day;
                    int diaMudar = Convert.ToInt32(ndel.DiaInic);
                    if (diaAlvo == diaMudar)
                    {
                        ndel.DiaInic = dataEstudo.Day.ToString();
                    }
                }
            }
            else
            {//apaga todas as linhas com datas anteriores a data de estudo 

                for (DateTime d = dataBase; d < dataEstudo; d = d.AddDays(1))
                {
                    int diadeRef = d.Day;
                    foreach (var del in entdados.BlocoDe.ToList())
                    {
                        int diaDe = Convert.ToInt32(del.DiaInic);
                        if (diadeRef == diaDe)
                        {
                            if (del.NumDemanda == 6)
                            {
                                del.DiaInic = dataEstudo.Day.ToString();
                            }
                            else
                            {
                                entdados.BlocoDe.Remove(del);
                            }
                        }
                    }
                }
            }

            #endregion

            entdados.SaveToFile(createBackup: true);

        }

        public static void CriarRespot(string path, int incremento, DateTime dataBase, DateTime dataEstudo, bool expandEst)
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

        public static void CriarRenovaveis(string path, int incremento, DateTime dataBase, DateTime dataEstudo, bool expandEst)
        {
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

                if (diaIniREf < dataBase.Day)
                {
                    dataIniRef = new DateTime(dataBase.AddMonths(1).Year, dataBase.AddMonths(1).Month, diaIniREf);
                }
                else
                {
                    dataIniRef = new DateTime(dataBase.Year, dataBase.Month, diaIniREf);
                }

                if (diafimREf < dataBase.Day)
                {
                    dataFimRef = new DateTime(dataBase.AddMonths(1).Year, dataBase.AddMonths(1).Month, diafimREf);
                }
                else
                {
                    dataFimRef = new DateTime(dataBase.Year, dataBase.Month, diafimREf);
                }

                if (dataIniRef < dataEstudo)
                {
                    geraline.DiaIni = dataEstudo.Day < 10 ? " " + dataEstudo.Day.ToString() + " ;" : dataEstudo.Day.ToString() + " ;";
                }

                if (dataFimRef < dataEstudo)
                {
                    renovaveis.BlocoEolicaGeracao.Remove(geraline);
                }

                if (dataFimRef == dataEstudo)
                {
                    int horIni = Convert.ToInt32(geraline.HoraIni.Replace(';', ' ').Trim());
                    int horFim = Convert.ToInt32(geraline.HoraFim.Replace(';', ' ').Trim());
                    if (horFim < horIni)
                    {
                        renovaveis.BlocoEolicaGeracao.Remove(geraline);
                    }
                    else if (horFim == horIni)
                    {
                        int meiaIni = Convert.ToInt32(geraline.MeiaHoraIni.Replace(';', ' ').Trim());
                        int meiaFim = Convert.ToInt32(geraline.MeiaHoraFim.Replace(';', ' ').Trim());
                        if (meiaFim == 0 && meiaIni == 1 || meiaFim == meiaIni)
                        {
                            renovaveis.BlocoEolicaGeracao.Remove(geraline);
                        }
                    }
                }
            }

            renovaveis.SaveToFile(createBackup: true);
        }
    }
}

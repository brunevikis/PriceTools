using Compass.CommomLibrary;
using Compass.ExcelTools;
using Compass.Services;
using Compass.CommomLibrary.EntdadosDat;
using System.IO.Compression;
using System.Globalization;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using Ionic.Zip;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Compass.ExcelTools.Templates;
using Compass.CommomLibrary.Dadger;

namespace Compass.DecompToolsShellX
{

    class Program
    {

        static Dictionary<string, Action<string>> actions = new Dictionary<string, Action<string>>();

        static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();

            actions.Add("abrir", open);
            actions.Add("vazoes", vazoes);
            actions.Add("vazoes6", vazoes6);
            actions.Add("earm", armazenamento);
            actions.Add("resultado", resultado);
            actions.Add("duplicar", duplicar);
            actions.Add("corte", corte_tendencia);
            actions.Add("cortes", cortes_tendencia);
            actions.Add("dgernwd", dgernwd);
            actions.Add("ons2ccee", ons2ccee);
            actions.Add("dessem2ccee", dessem2ccee);
            actions.Add("convdecodess", convDecodess);
            actions.Add("inviab", tratarInviabilidade);
            actions.Add("resultados", resultados);
            actions.Add("previvaz", previvaz);
            actions.Add("tendhidr", tendhidr);
            actions.Add("plddessem", pldDessem);
            actions.Add("uhdessem", uhDessem);
            actions.Add("rodardessem", runDessem);
            actions.Add("dpdessem", atualizaDp);
            actions.Add("exdedessem", extraiDE);
            actions.Add("dessemtools", dessemTools);
            actions.Add("previvazlocal", previvazLocal);
            actions.Add("carregarprevs", carregaPrevs);
            actions.Add("resdatabase", ResDataBaseTools);//resdatabase//
            actions.Add("coletalimites", ColetaLimites);
            //actions.Add("getpatamares", getPatamares);
            actions.Add("getpatamaresext", getPatamaresExt);
            actions.Add("vertermicas", vertermicas);
            actions.Add("atualizacarga", AtualizaCarga);
            actions.Add("atualizaconfhd", UpdateConfHd);
            actions.Add("atualizaweol", UpdateWeolNWDC);
            actions.Add("cenariosauto", CenariosAuto);//cenarios

            //atualizacarga "C:\Files\Implementacoes\atualizaCarga\NW202408"

            //dessemtools "K:\teste\dessemTESTE\resultados\DS_ONS_122023_RV1D08_ccee (1)"
            //previvaz "C:\Files\16_Chuva_Vazao\2023_10\RV1\23-09-29\CV_ACOMPH_FUNC_ECENS45\Propagacoes_Automaticas.txt|ext"
            //C:\Files\16_Chuva_Vazao\2023_10\RV1\23-09-29\CV_ACOMPH_FUNC_d-1_ECENS45

            //         < add key = "userlogin" value = "douglas.canducci@cpas.com.br" />
            //previvaz "C:\Files\16_Chuva_Vazao\2023_10\RV1\23-09-29\CV_ACOMPH_FUNC_ECENS45\Propagacoes_Automaticas.txt|ext"
            //resultados "C:\Development\Implementacoes\verResultados\202210_oficial_umido_3009\bkprvo"
            //resultado K:\teste\dessemTESTE\resultados\Dessem_RevExpand-09-11-2023_arquivos-renovaveis-NP_DP_Tucurui_+6GWm_Angra2
            //< add key = "passwordlogin" value = "Pas5Word" />
            //dessem2ccee "K:\5_dessem\2022_08\RV1\DS_ONS_082022_RV1D11|true"
            //previvaz "C:\Files\16_Chuva_Vazao\2022_05\RV3\22-05-18\testeSE_Bruno\SCP_CV_ACOMPH_FUNC_d-1_EURO\Propagacoes_Automaticas.txt"
            //previvaz "C:\Files\16_Chuva_Vazao\2023_09\RV3\23-09-13\CV_ACOMPH_FUNC_d-1_EURO\Propagacoes_Automaticas.txt"

            //convdecodess "L:\teste_decodess\DEC_ONS_052021_RV2_VE"
            //dessemtools "L:\Teste_Dessem\testeresulDessem"
            //uhdessem"L:\7_dessem\DecksDiarios\12_2020\RV3\29_12_2020_16_25_25"

            //C:\Development\Implementacoes\testechuvaprevi\Gera_e_Avalia_Cenariostestesdebug.xlsm
            //rodardessem "Z:\7_dessem\DESSEM_CCEE\2021\03_mar\RV0\DS_CCEE_032021_SEMREDE_RV0D05"

            //convdecodess "L:\teste_decodess\DEC_ONS_012021_RV1_VE_ccee"
            //convdecodess "L:\teste_decodess\DEC_ONS_122020_RV3_VE_ccee"
            //convdecodess "L:\teste_decodess\DEC_ONS_122020_RV2_VE_ccee"
            //dessem2ccee "C:\ConversaoDessem\DS_ONS_112020_RV2D16_teste|true"

            //dessem2ccee "P:\Bruno Araujo\ConversaoDessem\DS_ONS_112020_RV2D16_teste|true"

            //ons2ccee "K:\4_curto_prazo\2022_08\DEC_ONS_082022_RV2_VE|true"

            // ons2ccee "Z:\6_decomp\03_Casos\2020_11\teste_bruno\Teste_CVLINE\DEC_ONS_112020_RV0_VE|true"
            //previvaz "N:\Middle - Preço\16_Chuva_Vazao\2020_07\RV4\20-07-20\testeBruno\CV_ACOMPH_FUNC_Atualizado\CHUVAVAZAO_CENARIO_1087970864.xlsm"|true --> para encadear o previvaz
            //  previvaz "N:\Middle - Preço\16_Chuva_Vazao\2020_07\RV3\20-07-14\testeBruno\CV_ACOMPH_FUNC_EURO\CHUVAVAZAO_CENARIO_-883830657.xlsm"
            //   previvaz "N:\Middle - Preço\16_Chuva_Vazao\2020_07\RV4\20-07-20\testeBruno\CPM_CV_FUNC_d-1_EURO\Propagacoes_Automaticas.txt""
            //resultados "C:\Development\Implementacoes\verResultados\202210_oficial_umido_3009\RV0_70_30_50_80"

            //ons2ccee "K:\4_curto_prazo\2023_07\deck_newave_2023_07_Preliminar"
            //ons2ccee "K:\4_curto_prazo\2024_07\deck_newave_2024_07"

            if (args.Length > 1)
            {
                var action = args[0].ToLower();

                if (actions.ContainsKey(action))
                {
                    actions[action].Invoke(args[1]);
                }
                else if (args.Length >= 2)
                {
                    actions[action].Invoke(args[1] + "|" + args[2]);
                }
            }
            else
            {
                resultado("");
            }
        }

        static void vazoes(string path)
        {
            Vazoes gevazp = null;

            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;
                gevazp = new Compass.Services.Vazoes();
                var files = gevazp.Run(dir, true);

                var prevcenRel = files.FirstOrDefault(f => f.FullPath.EndsWith("prevcen.rel", StringComparison.OrdinalIgnoreCase));
                if (prevcenRel != null)
                {

                    var relContent = File.ReadAllText(prevcenRel.FullPath);
                    var pat = @"USINA\s*:.+VALOR:\s+-\d+";

                    var vNegativas = System.Text.RegularExpressions.Regex.Matches(relContent, pat, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    if (vNegativas.Count > 0)
                    {

                        var alert = "";

                        foreach (System.Text.RegularExpressions.Match m in vNegativas)
                        {
                            alert += m.Value + "\r\n";
                        }

                        MessageBox.Show(alert, "Vazoes Incrementais Negativas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }



            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (gevazp != null)
                    gevazp.ClearTempFolder();
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

        static void vazoes6(string path)
        {
            Vazoes6 gevazp = null;

            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;
                gevazp = new Compass.Services.Vazoes6();
                var files = gevazp.Run(dir, true);




            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (gevazp != null)
                    gevazp.ClearTempFolder();
            }
        }

        static void previvaz(string path)
        {
            Previvaz previvaz = null;
            bool encad = false;
            bool smapExt = false;
            if (path.Contains("true"))
            {
                var command = path.Split('|');
                path = command[0];
                encad = Convert.ToBoolean(command[1]);
                smapExt = command.Any(x => x.Contains("ext")) ? true : false;
            }

            if (path.Contains("ext"))
            {
                smapExt = true;
                var command = path.Split('|');
                path = command[0];
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(path) && path.EndsWith("Propagacoes_Automaticas.txt"))
                {
                    path = path.Substring(0, path.IndexOf("Propagacoes_Automaticas.txt"));
                    Previvaz.ProcessResultsPart2(path, encad, smapExt);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(path) && File.Exists(path) && path.EndsWith("xlsm", StringComparison.OrdinalIgnoreCase))
                    {
                        Previvaz.RunCenario(path, true, encad); //encad);
                    }
                    else
                    {
                        string dir;
                        if (Directory.Exists(path))
                        {
                            dir = path;
                        }
                        else if (File.Exists(path))
                        {
                            dir = Path.GetDirectoryName(path);
                        }
                        else
                            return;

                        previvaz = new Compass.Services.Previvaz();
                        previvaz.Run(dir);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        static void getPatamaresExt(string teste)
        {
            int ano = 2025;
            int anoFIm = 2029;
            bool patamares2023 = false;
            bool patamares2024 = false;
            bool patamares2025 = false;


            DateTime data = new DateTime(ano, 1, 1);
            DateTime fim = new DateTime(anoFIm, 12, 31);
            List<string> patamareDeCarga = new List<string>();
            patamareDeCarga.Add("DATA;PESADO;MEDIO;LEVE;DIA-SEMANA;TIPO");
            var feriados = Tools.feriados;

            for (DateTime i = data; i <= fim; i = i.AddDays(1))
            {
                patamares2023 = i.Year == 2023;
                patamares2024 = i.Year == 2024;
                patamares2025 = i.Year >= 2025;
                string dia = "";
                string tipo = "";

                switch (i.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        dia = "DOMINGO";
                        if (feriados.Any(x => x.Date == i.Date))
                        {
                            tipo = "FERIADO";
                        }
                        else
                        {
                            tipo = "NAO-UTIL";
                        }
                        break;
                    case DayOfWeek.Monday:
                        dia = "SEGUNDA";
                        if (feriados.Any(x => x.Date == i.Date))
                        {
                            tipo = "FERIADO";
                        }
                        else
                        {
                            tipo = "UTIL";
                        }
                        break;
                    case DayOfWeek.Tuesday:
                        dia = "TERCA";
                        if (feriados.Any(x => x.Date == i.Date))
                        {
                            tipo = "FERIADO";
                        }
                        else
                        {
                            tipo = "UTIL";
                        }
                        break;
                    case DayOfWeek.Wednesday:
                        dia = "QUARTA";
                        if (feriados.Any(x => x.Date == i.Date))
                        {
                            tipo = "FERIADO";
                        }
                        else
                        {
                            tipo = "UTIL";
                        }
                        break;
                    case DayOfWeek.Thursday:
                        dia = "QUINTA";
                        if (feriados.Any(x => x.Date == i.Date))
                        {
                            tipo = "FERIADO";
                        }
                        else
                        {
                            tipo = "UTIL";
                        }
                        break;
                    case DayOfWeek.Friday:
                        dia = "SEXTA";
                        if (feriados.Any(x => x.Date == i.Date))
                        {
                            tipo = "FERIADO";
                        }
                        else
                        {
                            tipo = "UTIL";
                        }
                        break;
                    case DayOfWeek.Saturday:
                        dia = "SABADO";
                        if (feriados.Any(x => x.Date == i.Date))
                        {
                            tipo = "FERIADO";
                        }
                        else
                        {
                            tipo = "NAO-UTIL";
                        }
                        break;
                    default:
                        break;
                }
                var dados = Tools.GetIntervalosHoararios(i, patamares2023, patamares2024, patamares2025);

                int pesado = dados.Where(x => x.Value.ToUpper() == "PESADA").Count();
                int medio = dados.Where(x => x.Value.ToUpper() == "MEDIA").Count();
                int leve = dados.Where(x => x.Value.ToUpper() == "LEVE").Count();

                patamareDeCarga.Add($"{i:dd/MM/yyyy};{pesado};{medio};{leve};{dia};{tipo}");

            }
            File.WriteAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PATAMARESDECARGA_EXT_2025_2029.csv", patamareDeCarga);

        }

        static void getPatamares(string anoArg)
        {
            bool patamares2023 = false;
            bool patamares2024 = false;
            bool patamares2025 = false;
            int ano = Convert.ToInt32(anoArg);
            patamares2023 = ano == 2023;
            patamares2024 = ano == 2024;
            patamares2025 = ano >= 2025;

            DateTime inicio = new DateTime(ano, 1, 1);
            DateTime fim = new DateTime(ano, 12, 31);
            List<string> patamareDeCarga = new List<string>();
            patamareDeCarga.Add("USE [COMPARADOR_DC]\nGO\nINSERT INTO[dbo].[semanas_patamares]\n([Semana]\n,[pesado]\n,[medio]\n,[leve])\nVALUES\n");
            int numeroSemana = 1;

            try
            {
                for (DateTime d = inicio; d <= fim; d = d.AddDays(1))
                {
                    DateTime semanaInicio = d;
                    DateTime semanaFim = d;
                    while (semanaFim.DayOfWeek != DayOfWeek.Friday && semanaFim.AddDays(1).Month == semanaInicio.Month)
                    {
                        semanaFim = semanaFim.AddDays(1);
                    }
                    var pat = Tools.GetHorasPatamares(semanaInicio, semanaFim, true, patamares2023, patamares2024, patamares2025);
                    patamareDeCarga.Add("(" + semanaInicio.ToString("yyyyMM") + numeroSemana.ToString() + "," + pat.Item1.ToString() + "," + pat.Item2.ToString() + "," + pat.Item3 + "),");
                    d = semanaFim;
                    numeroSemana = d.AddDays(1).Month == semanaInicio.Month ? numeroSemana + 1 : 1;

                }
                patamareDeCarga.Add("GO");
                File.WriteAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PATAMARESDECARGA" + anoArg + ".txt", patamareDeCarga);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        static void previvazLocal(string path)
        {
            Previvaz previvaz = null;

            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                previvaz = new Compass.Services.Previvaz();
                previvaz.RunPrevsLocal(dir);

                System.Windows.Forms.MessageBox.Show("Processo finalizado!");

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        public static void carregaEnas(object comando)
        {
            try
            {
                string comands = (string)comando;
                var coms = comands.Split('|').ToList();
                string path = coms[0];
                int rv = Convert.ToInt32(coms[1]);
                int month = Convert.ToInt32(coms[2].Split('/').First());
                int year = Convert.ToInt32(coms[2].Split('/').Last());
                int oficNum = Convert.ToInt32(coms[3]);
                string tipoRodada = Path.GetDirectoryName(path).Split('\\').Last();

                if (oficNum == 1)
                {
                    tipoRodada = "Oficial";
                }


                if (File.Exists(path))
                {
                    if (Path.GetFileName(path).ToLower().Contains("semanal"))
                    {
                        var ena = DocumentFactory.Create(path) as Compass.CommomLibrary.EnaSemanalLog.EnaSemanalLog;
                        ena.SaveToFile(createBackup: true);
                        Compass.CommomLibrary.Resultados_CPASEntitiesEnas ena_ctx = new Resultados_CPASEntitiesEnas();


                        //List<PrevsDados> dados = new List<PrevsDados>();
                        //foreach (var item in prevs.Vazoes.ToList())
                        //{
                        //    var tb = new PrevsDados
                        //    {

                        //        posto = item[1],
                        //        sem1 = item[2],
                        //        sem2 = item[3],
                        //        sem3 = item[4],
                        //        sem4 = item[5],
                        //        sem5 = item[6],
                        //        sem6 = item[7],
                        //    };
                        //    dados.Add(tb);
                        //}
                        //var t = new PrevsReg
                        //{

                        //    dt_entrada = DateTime.Now,
                        //    mes = month,
                        //    rev = rv,
                        //    caminho = path,
                        //    ano = year,
                        //    oficial = oficNum,
                        //    PrevsDados = dados,
                        //};
                        //prevs_ctx.PrevsReg.Add(t);
                        //prevs_ctx.SaveChanges();
                    }
                }

                else
                    return;




            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        public static void carregaPrevs(object comando)
        {

            try
            {
                string comands = (string)comando;
                var coms = comands.Split('|').ToList();
                string path = coms[0];
                int rv = Convert.ToInt32(coms[1]);
                int month = Convert.ToInt32(coms[2].Split('/').First());
                int year = Convert.ToInt32(coms[2].Split('/').Last());
                int oficNum = Convert.ToInt32(coms[3]);

                if (File.Exists(path))
                {
                    if (Path.GetFileName(path).ToLower().Contains("prevs"))
                    {
                        var prevs = DocumentFactory.Create(path) as Compass.CommomLibrary.Prevs.Prevs;
                        Compass.CommomLibrary.Resultados_CPASEntitiesPrevs prevs_ctx = new Resultados_CPASEntitiesPrevs();

                        //var dados = new PrevsDados();

                        List<PrevsDados> dados = new List<PrevsDados>();
                        foreach (var item in prevs.Vazoes.ToList())
                        {
                            var tb = new PrevsDados
                            {

                                posto = item[1],
                                sem1 = item[2],
                                sem2 = item[3],
                                sem3 = item[4],
                                sem4 = item[5],
                                sem5 = item[6],
                                sem6 = item[7],
                            };
                            dados.Add(tb);
                        }
                        var t = new PrevsReg
                        {

                            dt_entrada = DateTime.Now,
                            mes = month,
                            rev = rv,
                            caminho = path,
                            ano = year,
                            oficial = oficNum,
                            PrevsDados = dados,
                        };
                        prevs_ctx.PrevsReg.Add(t);
                        prevs_ctx.SaveChanges();
                    }
                }

                else
                    return;




            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        static void AtualizaCarga(string commands)
        {
            //L:\6_decomp\03_Casos\2019_04\deck_newave_2019_04
            //"L:\\6_decomp\\03_Casos\\2019_05\\DEC_ONS_052019_RV1_VE"

            var command = commands.Split('|');

            //var data = command[0].Substring(command[0].Length - 7, 7).Split('_');
            var path = command[0];

            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var dirInfo = new DirectoryInfo(dir);
                var parentDir = dirInfo.Parent.FullName;
                var dirName = dirInfo.Name + "_Atualizado";

                var i = 0;
                var cloneDir = "";
                do
                {
                    cloneDir = Path.Combine(parentDir, dirName + " (" + ++i + ")");
                } while (Directory.Exists(cloneDir));



                var deck = DeckFactory.CreateDeck(dir);

                if (!(deck is Compass.CommomLibrary.Newave.Deck))
                {
                    throw new NotImplementedException("Deck não reconhecido para a execução");
                }

                deck.CopyFilesToFolder(cloneDir);


                dynamic newDeck = DeckFactory.CreateDeck(cloneDir);


                if (newDeck is Compass.CommomLibrary.Newave.Deck && (command.Length > 1 && command[1] == "true"))
                {
                    //var frm = new FrmOnsReCcee(cceeDeck);
                    //frm.Salvar();
                    ////PreliminarAutorun(cceeDeck.BaseFolder, "/home/producao/PrevisaoPLD/cpas_ctl_common/scripts/newave25.sh");
                    //PreliminarAutorun(cceeDeck.BaseFolder, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/newave28.sh");
                }
                else if (newDeck is Compass.CommomLibrary.Decomp.Deck && (command.Length > 1 && command[1] == "true"))
                {
                    //var frm = new FrmDcOns2Ccee(cceeDeck);
                    //frm.Salvar();

                    //var frmCortes = new FrmCortes(new string[] { cceeDeck.BaseFolder });
                    //frmCortes.OK(true);

                    //PreliminarAutorun(cceeDeck.BaseFolder, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/decomp31Viab.sh preliminar");
                }
                else if (newDeck is Compass.CommomLibrary.Newave.Deck)
                {
                    Thread thread = new Thread(AtualizaDeckCarga);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA                                                                  //thread.Start(redat);
                    thread.Start(newDeck);
                    thread.Join();
                }
                else if (newDeck is Compass.CommomLibrary.Decomp.Deck)
                {
                    //Thread thread = new Thread(dcOns2CceeSTA);
                    //thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    //thread.Start(cceeDeck);
                    //thread.Join(); //Wait for the thread to end   
                }
            }
            catch (Exception ex)
            {
                var texto = ex.ToString();
                if (ex.ToString().Contains("reconhecido"))
                {
                    texto = "Deck não reconhecido para a execução!";
                }
                MessageBox.Show(texto, "Atenção");
                //Compass.CommomLibrary.Tools.SendMail(texto, "bruno.araujo@enercore.com.br; pedro.modesto@enercore.com.br; natalia.biondo@enercore.com.br;", "Falha ao converter deck");


            }

        }



        static void open(string filePath)
        {

            Microsoft.Office.Interop.Excel.Application xlApp = null;

            try
            {
                var doc = DocumentFactory.Create(filePath);

                xlApp = Helper.StartExcel();
                xlApp.Cursor = XlMousePointer.xlWait;
                xlApp.ScreenUpdating = false;

                var xlWb = xlApp.Workbooks.Add();

                var info = xlWb.SetInfosheet(doc);

                xlWb.WriteDocumentToWorkbook(doc);

                info.BottonComments = doc.BottonComments;

                //xlApp.WindowState = XlWindowState.xlMaximized;
                //xlWb.Windows[1].WindowState = XlWindowState.xlMaximized;
                xlApp.ActiveWindow.Activate();
                xlWb.Windows[1].Activate();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (xlApp != null)
                {
                    xlApp.Cursor = XlMousePointer.xlDefault;
                    xlApp.ScreenUpdating = true;

                    Helper.Release(xlApp);
                }
            }
        }

        static void armazenamento(string path)
        {
            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var deck = DeckFactory.CreateDeck(dir);

                Compass.CommomLibrary.Decomp.ConfigH configH;
                if (deck is Compass.CommomLibrary.Decomp.Deck)
                {

                    var dadger = (Compass.CommomLibrary.Dadger.Dadger)DocumentFactory.Create(deck.Documents["DADGER."].BasePath);
                    var hidr = (Compass.CommomLibrary.HidrDat.HidrDat)DocumentFactory.Create(deck.Documents["HIDR.DAT"].BasePath);


                    configH = new Compass.CommomLibrary.Decomp.ConfigH(dadger, hidr);

                }
                else if (deck is Compass.CommomLibrary.Newave.Deck)
                {

                    var confhddat = (Compass.CommomLibrary.ConfhdDat.ConfhdDat)DocumentFactory.Create(deck.Documents["CONFHD.DAT"].BasePath);
                    var modifdat = BaseDocument.Create<Compass.CommomLibrary.ModifDatNW.ModifDatNw>(File.ReadAllText(deck.Documents["MODIF.DAT"].BasePath));
                    var hidr = (Compass.CommomLibrary.HidrDat.HidrDat)DocumentFactory.Create(deck.Documents["HIDR.DAT"].BasePath);

                    configH = new Compass.CommomLibrary.Decomp.ConfigH(confhddat, hidr, modifdat);

                }
                else
                {
                    MessageBox.Show("Deck não identificado");
                    return;
                }



                double[] earmAtual = configH.GetEarms();
                double[] earmMax = configH.GetEarmsMax();


                var dtEarm = new System.Data.DataTable();

                dtEarm.Columns.Add("Sistema");
                dtEarm.Columns.Add("EarmMax");
                dtEarm.Columns.Add("EarmIni");
                dtEarm.Columns.Add("EarmIni_Perc");

                //var rs2 = new List<object>();

                var fmt = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");

                int i = 0;
                foreach (var sb in configH.index_sistemas)
                {
                    dtEarm.Rows.Add(
                        sb.Item2.ToString(),
                        earmMax[i].ToString("N1", fmt),
                        earmAtual[i].ToString("N1", fmt),
                        (earmAtual[i] / earmMax[i]).ToString("00.0%", fmt));
                    //rs2.Add(new { Sistema = sb.Item2.ToString(), EarmMax = earmMax[i].ToString("N1", fmt), EarmIni = earmAtual[i].ToString("N1", fmt), EarmIni_Perc = (earmAtual[i] / earmMax[i]).ToString("00.0%", fmt) });
                    i++;
                }

                FormViewer.Show("EARM calculado - " + dir, new ResultDataSource { Title = "Armazenamento", DataSource = dtEarm });


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void vertermicas(string path)
        {
            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Diretorio não encontrado");
                    return;
                }
                //else
                //{
                //    FormViewer.Show("", new Result());
                //    return;
                //}

                var deck = DeckFactory.CreateDeck(dir) as Compass.CommomLibrary.Decomp.Deck;

                if (deck is CommomLibrary.Decomp.Deck)
                {
                    Compass.CommomLibrary.Dadger.Dadger dadger = deck[CommomLibrary.Decomp.DeckDocument.dadger].Document as CommomLibrary.Dadger.Dadger;
                    var results = deck.GetResults();
                    FrmTermicasGraph.Show(dir, dadger, results);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Diretório inválido");
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void resultado(string path)
        {
            try
            {
                bool alternativo = false;

                //if (System.Windows.Forms.MessageBox.Show(@"Deseja usar PLD Alternativo?"
                //  , "Limites PLD", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                //{
                //    var frm = new FrmPldAlter();
                //    frm.ShowDialog();
                //    alternativo = frm.usar;

                //}

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                {
                    FormViewer.Show("", new Result());
                    return;
                }

                var deck = DeckFactory.CreateDeck(dir);

                if (deck is CommomLibrary.Newave.Deck || deck is CommomLibrary.Decomp.Deck || deck is CommomLibrary.Dessem.Deck)
                {

                    var results = deck.GetResults(alternativo);
                    FormViewer.Show(dir, results);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void resultados(string path)
        {
            try
            {
                bool alternativo = false;

                //if (System.Windows.Forms.MessageBox.Show(@"Deseja usar PLD Alternativo?"
                //  , "Limites PLD", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                //{
                //    var frm = new FrmPldAlter();
                //    frm.ShowDialog();
                //    alternativo = frm.usar;

                //}

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                }
                else
                    return;

                var dirsTets = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories).Where(x => Directory.GetFiles(x).Any(y => y.EndsWith(".dat", StringComparison.OrdinalIgnoreCase))).ToList();
                var dirsTets2 = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories).ToList();

                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories).Where(x => Directory.GetFiles(x).Any(y => y.EndsWith(".dat", StringComparison.OrdinalIgnoreCase)))
                    .AsParallel()//.WithDegreeOfParallelism(4)                       
                    .Select(x => new
                    {
                        dir = x.Remove(0, dir.Length),
                        deck = DeckFactory.CreateDeck(x),
                    });

                var dNw = dirs.Where(x => x.deck is CommomLibrary.Newave.Deck)
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                var dDcMensal = dirs.Where(x => x.deck is CommomLibrary.Decomp.Deck && (DocumentFactory.Create(x.deck.Documents["DADGER."].Document.File) as Compass.CommomLibrary.Dadger.Dadger).VAZOES_NumeroDeSemanas == 0 && Directory.GetFiles(x.deck.BaseFolder).Any(y => y.EndsWith("dec_oper_sist.csv", StringComparison.OrdinalIgnoreCase))).AsParallel()
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults(alternativo)
                    }).Where(x => x.result != null).ToList();

                var dDcSem = dirs.Where(x => x.deck is CommomLibrary.Decomp.Deck && (DocumentFactory.Create(x.deck.Documents["DADGER."].Document.File) as Compass.CommomLibrary.Dadger.Dadger).VAZOES_NumeroDeSemanas > 0 && Directory.GetFiles(x.deck.BaseFolder).Any(y => y.EndsWith("dec_oper_sist.csv", StringComparison.OrdinalIgnoreCase))).AsParallel()
                   .Select(x => new
                   {
                       x.dir,
                       x.deck,
                       result = x.deck.GetResults(alternativo)
                   }).Where(x => x.result != null).ToList();

                var dDs = dirs.Where(x => x.deck is CommomLibrary.Dessem.Deck).AsParallel()
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                if (dNw.Count() > 0) FormViewer.Show("NEWAVE", true, dNw.Select(x => x.result).ToArray());
                if (dDcMensal.Count() > 0) FormViewer.Show("DECOMP", true, dDcMensal.Select(x => x.result).ToArray());
                if (dDcSem.Count() > 0) FormViewer.Show("DECOMP", true, dDcSem.Select(x => x.result).ToArray());
                if (dDs.Count() > 0) FormViewer.Show("DESSEM", true, dDs.Select(x => x.result).ToArray());

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void dgernwd(string path)
        {
            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                Services.Deck.CreateDgerNewdesp(dir);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        static void duplicar(string path)
        {

            string newPath;

            duplicar(path, out newPath);


            string texto = "Processo concluído!";

            MessageBox.Show(texto, "Price Tools");

        }

        static void duplicar(string path, out string newPath)
        {
            newPath = "";
            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var dirInfo = new DirectoryInfo(dir);
                var parentDir = dirInfo.Parent.FullName;
                var dirName = dirInfo.Name;

                var i = 0;
                var cloneDir = "";
                do
                {
                    cloneDir = Path.Combine(parentDir, dirName + " (" + ++i + ")");
                } while (Directory.Exists(cloneDir));

                var deck = DeckFactory.CreateDeck(dir);

                newPath = cloneDir;

                deck.CopyFilesToFolder(cloneDir);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void ons2ccee(string commands)
        {
            //L:\6_decomp\03_Casos\2019_04\deck_newave_2019_04
            //"L:\\6_decomp\\03_Casos\\2019_05\\DEC_ONS_052019_RV1_VE"

            var command = commands.Split('|');

            //var data = command[0].Substring(command[0].Length - 7, 7).Split('_');
            var path = command[0];

            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var dirInfo = new DirectoryInfo(dir);
                var parentDir = dirInfo.Parent.FullName;
                var dirName = dirInfo.Name + "_ccee";

                var i = 0;
                var cloneDir = "";
                do
                {
                    cloneDir = Path.Combine(parentDir, dirName + " (" + ++i + ")");
                } while (Directory.Exists(cloneDir));



                var deck = DeckFactory.CreateDeck(dir);

                if (!(deck is Compass.CommomLibrary.Newave.Deck || deck is Compass.CommomLibrary.Decomp.Deck))
                {
                    throw new NotImplementedException("Deck não reconhecido para a execução");
                }

                deck.CopyFilesToFolder(cloneDir);
                try
                {
                    var GTMIN_CCEEFileAtual = Directory.GetFiles(deck.BaseFolder).Where(x => Path.GetFileName(x).StartsWith("GTMIN_CCEE_", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (GTMIN_CCEEFileAtual != null && File.Exists(GTMIN_CCEEFileAtual))
                    {
                        File.Copy(GTMIN_CCEEFileAtual, Path.Combine(cloneDir, GTMIN_CCEEFileAtual.Split('\\').Last()), true);

                    }
                }
                catch (Exception ed)
                {


                }

                dynamic cceeDeck = DeckFactory.CreateDeck(cloneDir);


                if (cceeDeck is Compass.CommomLibrary.Newave.Deck && (command.Length > 1 && command[1] == "true"))
                {
                    var frm = new FrmOnsReCcee(cceeDeck);
                    frm.Salvar();
                    //PreliminarAutorun(cceeDeck.BaseFolder, "/home/producao/PrevisaoPLD/cpas_ctl_common/scripts/newave25.sh");
                    PreliminarAutorun(cceeDeck.BaseFolder, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/newave_AUTORUN.sh");
                }
                else if (cceeDeck is Compass.CommomLibrary.Decomp.Deck && (command.Length > 1 && command[1] == "true"))
                {
                    var frm = new FrmDcOns2Ccee(cceeDeck);
                    frm.Salvar();

                    var frmCortes = new FrmCortes(new string[] { cceeDeck.BaseFolder });
                    frmCortes.OK(true);

                    //PreliminarAutorun(cceeDeck.BaseFolder, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/decomp31Viab.sh preliminar");
                    PreliminarAutorun(cceeDeck.BaseFolder, "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/decomp_AUTORUN.sh preliminar");
                    //decomp_AUTORUN.sh
                }
                else if (cceeDeck is Compass.CommomLibrary.Newave.Deck)
                {
                    Thread thread = new Thread(nwOnsReCcee);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA                                                                  //thread.Start(redat);
                    thread.Start(cceeDeck);
                    thread.Join();
                }
                else if (cceeDeck is Compass.CommomLibrary.Decomp.Deck)
                {
                    Thread thread = new Thread(dcOns2CceeSTA);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    thread.Start(cceeDeck);
                    thread.Join(); //Wait for the thread to end   
                }
            }
            catch (Exception ex)
            {
                var texto = ex.ToString();
                if (ex.ToString().Contains("reconhecido"))
                {
                    texto = "Deck não reconhecido para a execução por falta de arquivos!";
                }
                Compass.CommomLibrary.Tools.SendMail(texto, "bruno.araujo@enercore.com.br; pedro.modesto@enercore.com.br; natalia.biondo@enercore.com.br; gabriella.radke@enercore.com.br;", "Falha ao converter deck");


            }

        }

        static void UpdateConfHd(string commands)
        {
            //L:\6_decomp\03_Casos\2019_04\deck_newave_2019_04
            //"L:\\6_decomp\\03_Casos\\2019_05\\DEC_ONS_052019_RV1_VE"

            var command = commands.Split('|');

            //var data = command[0].Substring(command[0].Length - 7, 7).Split('_');
            var path = command[0];

            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var deck = DeckFactory.CreateDeck(dir);

                if (!(deck is Compass.CommomLibrary.Newave.Deck))
                {
                    throw new NotImplementedException("Deck não reconhecido para a execução");
                }
                else if (deck is Compass.CommomLibrary.Newave.Deck)
                {
                    Thread thread = new Thread(updateConfhProcess);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA                                                                  //thread.Start(redat);
                    thread.Start(deck);
                    thread.Join();
                }


            }
            catch (Exception ex)
            {
                var texto = ex.ToString();
                if (ex.ToString().Contains("reconhecido"))
                {
                    texto = "Deck não reconhecido para a execução!";
                }
                MessageBox.Show(texto, "Atenção");

            }

        }
        static void CenariosAuto(string commands)
        {
            //TODO: logica de pular execução caso processo em andamento, 
            Directory.CreateDirectory(commands);
            string cenariosLog = Path.Combine(commands, "Exec.log");
            var fileList= Directory.GetFiles(commands).OrderBy(x => x).ToList();

            string xlFile = Directory.GetFiles(commands).OrderBy(x => x).FirstOrDefault();
            if (File.Exists(xlFile))
            {
                if (!File.Exists(cenariosLog))
                {

                    File.WriteAllText(cenariosLog, "Processo em execução\nCaminho: " + xlFile);
                    Microsoft.Office.Interop.Excel.Workbook wb = null;
                    Microsoft.Office.Interop.Excel.Workbooks workbooks = null;
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    var pointer = new IntPtr(xlApp.Hwnd);

                    //Microsoft.Office.Interop.Excel.Application xlApp = null;
                    List<string> consistFolders = new List<string>();
                    string dirCenGerado = "";
                    try
                    {
                        if (File.Exists(xlFile))
                        {
                            xlApp.DisplayAlerts = false;
                            xlApp.Visible = false;
                            xlApp.ScreenUpdating = true;
                            // xlApp = ExcelTools.Helper.StartExcelInvisible();
                            workbooks = xlApp.Workbooks;

                            //var wbxls = xlApp.Workbooks.Open(xlFile);
                            //var wbxls = workbooks.Open(xlFile);
                            //Workbook wb = xlApp.ActiveWorkbook;
                             wb = workbooks.Open(xlFile);
                            WorkbookMensal w;
                            if (wb.Application.ActiveWorkbook == null ||
                                !WorkbookMensal.TryCreate(wb.Application.ActiveWorkbook, out w))
                            {
                                return;
                            }
                            var dc = w.DecompBase;
                            var nw = w.NewaveBase;
                            dirCenGerado = nw;
                            string newXlFile = Path.Combine(dirCenGerado, Path.GetFileName(xlFile));
                            string completoLog = Path.Combine(dirCenGerado, "Exec.log");
                            Encadeado.Estudo estudo = new Encadeado.Estudo()
                            {
                                Origem = w.NewaveOrigem,
                                Destino = w.NewaveBase,
                                MesesAvancar = w.MesesAvancar,
                                DefinirVolumesPO = true,
                            };

                            estudo.Bloco_VE = w.Bloco_VE;
                            estudo.VolumesPO = w.Earm;
                            estudo.PrevisaoVazao = w.Cenarios.First().Vazoes;
                            estudo.ExecutavelNewave = w.ExecutavelNewave;
                            estudo.ExecutarConsist = w.ExecutarConsist;
                            estudo.NwHibrido = w.NwHibrido;

                            if (w.ReDats == null)
                            {

                                //if (System.Windows.Forms.MessageBox.Show("Caminho de restricoes elétricas do newave (_redat) não encontrado, continuar mesmo assim?"
                                //    , "Encadeado", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning)
                                //    != System.Windows.Forms.DialogResult.Yes)
                                return;

                            }
                            estudo.Restricoes = w.ReDats ?? new List<IRE>();

                            estudo.Agrints = w.AgrintDats ?? new List<IAGRIGNT>();

                            estudo.Adterm = w.adtermdat ?? new List<IADTERM>();

                            estudo.Intercambios = w.Intercambios ?? new List<IINTERCAMBIO>();

                            estudo.MERCADO = w.MercadosSisdat ?? new List<IMERCADO>();

                            estudo.Modifs = w.Modifwb ?? new List<IMODIF>();
                            estudo.ReModifs = w.ReModifwb ?? new List<IREMODIF>();
                            estudo.Curva = w.CurvasReedat ?? new List<ICURVA>();
                            estudo.Adtermdad = w.AdtremDadd ?? new List<IADTERMDAD>();
                            estudo.Reedads = w.Reedads ?? new List<IREEDAT>();
                            estudo.Restelecsv = w.RestEleCSV ?? new List<IRESTELECSV>();

                            if (System.IO.Directory.Exists(dc))
                            {

                                var deckDCBase = DeckFactory.CreateDeck(dc) as Compass.CommomLibrary.Decomp.Deck;
                                var configH = new Compass.CommomLibrary.Decomp.ConfigH(
                                    deckDCBase[CommomLibrary.Decomp.DeckDocument.dadger].Document as Dadger,
                                    deckDCBase[CommomLibrary.Decomp.DeckDocument.hidr].Document as Compass.CommomLibrary.HidrDat.HidrDat);

                                estudo.ConfighBase = configH;



                            }
                            estudo.ExecucaoPrincipal();
                            consistFolders = Services.GeraCenarios.GeraMensal(w, dc, nw, true);

                            if (consistFolders.Count() > 0)
                            {

                                Encadeado.Estudo Newestudo = new Encadeado.Estudo();
                                Newestudo.ExecutavelNewave = w.ExecutavelNewave;
                                Newestudo.ExecutarConsist = w.ExecutarConsist;

                                bool tesets = Newestudo.execucaoConsistDC(consistFolders);
                            }


                            Services.GeraCenarios.GeraRV0(w, dc, nw, true);


                            wb.Save();
                            wb.SaveCopyAs(newXlFile);
                            wb.Close(SaveChanges: false);
                            xlApp.Quit();
                            ExcelTools.Helper.Release(xlApp);
                            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName("Excel"))
                            {
                                if (proc.MainWindowHandle == pointer)
                                {
                                    proc.Kill();
                                }
                            }
                            if (File.Exists(xlFile))
                            {
                                string log = Path.Combine(dirCenGerado, "ERROR_LOG.TXT");

                                File.Delete(xlFile);
                                if (File.Exists(cenariosLog))
                                {
                                    File.Delete(cenariosLog);
                                }
                                File.WriteAllText(completoLog, "Processo finalizado com sucesso!");
                                if (File.Exists(log))
                                {
                                    File.Delete(log);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string log = Path.Combine(dirCenGerado, "ERROR_LOG.TXT");

                        if (Directory.Exists(dirCenGerado))
                        {
                            File.WriteAllText(log, e.Message.ToString());
                        }
                        if (wb != null)
                        {
                            wb.Close(SaveChanges: false);
                        }

                        if (xlApp != null)
                        {

                            xlApp.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
                            xlApp.ScreenUpdating = true;
                            xlApp.Quit();
                            ExcelTools.Helper.Release(xlApp);
                            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName("Excel"))
                            {
                                if (proc.MainWindowHandle == pointer)
                                {
                                    proc.Kill();
                                }
                            }
                        }
                        if (File.Exists(cenariosLog))
                        {
                            File.Delete(cenariosLog);
                        }
                    }
                    finally
                    {
                        if (xlApp != null)
                        {
                            xlApp.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
                            xlApp.ScreenUpdating = true;
                            xlApp.Quit();
                            ExcelTools.Helper.Release(xlApp);
                            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName("Excel"))
                            {
                                if (proc.MainWindowHandle == pointer)
                                {
                                    proc.Kill();
                                }
                            }
                        }

                    }
                }

            }


        }

        static void UpdateWeolNWDC(string commands)
        {
            //L:\6_decomp\03_Casos\2019_04\deck_newave_2019_04
            //"L:\\6_decomp\\03_Casos\\2019_05\\DEC_ONS_052019_RV1_VE"

            var command = commands.Split('|');

            //var data = command[0].Substring(command[0].Length - 7, 7).Split('_');
            var path = command[0];

            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var deck = DeckFactory.CreateDeck(dir);


                if (!(deck is Compass.CommomLibrary.Newave.Deck || deck is Compass.CommomLibrary.Decomp.Deck))
                {
                    throw new NotImplementedException("Deck não reconhecido para a execução");
                }
                else if (deck is Compass.CommomLibrary.Newave.Deck)
                {
                    Thread thread = new Thread(updateWeolNWProcess);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA                                                                  //thread.Start(redat);
                    thread.Start(deck);
                    thread.Join();
                }
                else if (deck is Compass.CommomLibrary.Decomp.Deck)
                {
                    Thread thread = new Thread(updateWeolDCProcess);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA                                                                  //thread.Start(redat);
                    thread.Start(deck);
                    thread.Join();
                }


            }
            catch (Exception ex)
            {
                var texto = ex.ToString();
                if (ex.ToString().Contains("reconhecido"))
                {
                    texto = "Deck não reconhecido para a execução!";
                }
                MessageBox.Show(texto, "Atenção");

            }

        }

        public static void ExportExcel(string dir, DateTime dataIni, DateTime dataFim)
        {
            try
            {
                var excelName = Path.Combine(dir, $"BlocoDE_{DateTime.Now:dd-MM-yyyy}.xlsx");

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.DisplayAlerts = false;
                Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                Worksheet ws = (Worksheet)app.ActiveSheet;
                ws.Name = "BlocoDE";
                app.Visible = false;
                ws.Cells[1, 1] = "DATA";
                ws.Cells[1, 2] = "NUM DEMANDA";
                ws.Cells[1, 3] = "HORA";
                ws.Cells[1, 4] = "MEIA HORA";
                ws.Cells[1, 5] = "DEMANDA";
                //ws.Cells.Range["A1"].NumberFormat = "dd/mm/yyyy";
                int i = 2;

                for (DateTime d = dataIni; d <= dataFim; d = d.AddDays(1))
                {
                    var blocoDe = GetDEBlock(d);
                    if (blocoDe.Count() > 0)
                    {
                        foreach (var deL in blocoDe)
                        {
                            //ws.Cells.Range[i, 1].NumberFormat = "dd/mm/yyyy";
                            ws.Cells[i, 1] = d.ToOADate();
                            //l_objExcel.Range[l_objExcel.Cells[rowIndex + 2, colIndex + 1], l_objExcel.Cells[rowIndex + 2, colIndex + 1]].NumberFormat
                            //= "mm-d-yy h:mm:ss AM/PM";
                            ws.Range[ws.Cells[i, 1], ws.Cells[i, 1]].NumberFormat = "dd/mm/aaaa";
                            ws.Cells[i, 2] = deL.NumDemanda;
                            ws.Cells[i, 3] = deL.HoraInic;
                            ws.Cells[i, 4] = deL.MeiaHoraInic;
                            ws.Cells[i, 5] = deL.Demanda;
                            i++;
                        }
                    }

                }//xlWorkbookDefault

                wb.SaveAs(excelName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange,
    XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                app.Quit();

                if (File.Exists(excelName))
                {
                    string texto = "Processo realizado com sucesso!";
                    MessageBox.Show(texto, "ATENCÃO!");
                }

            }
            catch (Exception e)
            {

                string texto = "Falha no processo:\n" + e.ToString();
                MessageBox.Show(texto, "ATENCÃO!");
            }

        }

        public static void AtualizaDPdessem(List<Tuple<DateTime, bool, float>> dpDados, string dir, List<string> diaAtualizar)
        {
            int index = 0;

            foreach (var dpDad in dpDados)
            {

                string arqBlocoDP = Path.Combine(dir, $"blocoDPcarga{index + 1}.csv");
                List<string> linhas = new List<string>();

                List<Tuple<DateTime, int, int, decimal?>> dadosCarga = new List<Tuple<DateTime, int, int, decimal?>>();

                Compass.CommomLibrary.IPDOEntitiesCargaDiaria CargaCtx = new IPDOEntitiesCargaDiaria();
                var cargas = CargaCtx.Carga_Diaria.Where(x => x.Data == dpDad.Item1.Date).ToList();
                foreach (var cg in cargas)
                {
                    Tuple<DateTime, int, int, decimal?> cgDados = new Tuple<DateTime, int, int, decimal?>(cg.Data, cg.Hora, cg.Submercado, cg.Previsto);
                    dadosCarga.Add(cgDados);
                }

                for (int s = 1; s <= 4; s++)
                {
                    foreach (var car in dadosCarga.Where(x => x.Item3 == s).ToList())
                    {
                        float valor = (float)car.Item4 * dpDad.Item3;
                        string linha = $"{car.Item3};{car.Item2};{valor};";//submercado,hora,valor
                        linhas.Add(linha);
                    }
                }

                File.WriteAllLines(arqBlocoDP, linhas);

                index++;
            }
            File.WriteAllLines(Path.Combine(dir, "diaAtualizar.txt"), diaAtualizar);
        }

        public static void CarregaCargaDiaria(List<Tuple<DateTime, bool, float>> dpDados)
        {
            int index = 0;

            foreach (var dpDad in dpDados)
            {

                string arqBlocoDP = $@"H:\Middle - Preço\Resultados_Modelos\DECODESS\Arquivos_Base\BlocosFixos\blocoDPcarga{index + 1}.csv";
                //string arqBlocoDP = $@"N:\Middle - Preço\Resultados_Modelos\DECODESS\Arquivos_Base\BlocosFixos\blocoDPcarga{index + 1}.csv";
                List<string> linhas = new List<string>();

                List<Tuple<DateTime, int, int, decimal?>> dadosCarga = new List<Tuple<DateTime, int, int, decimal?>>();

                Compass.CommomLibrary.IPDOEntitiesCargaDiaria CargaCtx = new IPDOEntitiesCargaDiaria();
                var cargas = CargaCtx.Carga_Diaria.Where(x => x.Data == dpDad.Item1.Date).ToList();
                if (dpDad.Item2 == true && cargas.Count() > 0)
                {
                    foreach (var cg in cargas)
                    {
                        Tuple<DateTime, int, int, decimal?> cgDados = new Tuple<DateTime, int, int, decimal?>(cg.Data, cg.Hora, cg.Submercado, cg.Previsto);
                        dadosCarga.Add(cgDados);
                    }

                    for (int s = 1; s <= 4; s++)
                    {
                        foreach (var car in dadosCarga.Where(x => x.Item3 == s).ToList())
                        {
                            float valor = (float)car.Item4 * dpDad.Item3;
                            string linha = $"{car.Item3};{car.Item2};{valor};";//submercado,hora,valor
                            linhas.Add(linha);
                        }
                    }

                    File.WriteAllLines(arqBlocoDP, linhas);
                }
                index++;
            }

        }

        public static void AtualizarCadastroPLD(string path, int ano, double pldMin, double pldMax, double pldMaxEst)
        {

            CommomLibrary.PldDessem.PldDessem limites = new CommomLibrary.PldDessem.PldDessem();
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var style = System.Globalization.NumberStyles.Any;

            var pldLimitesLines = File.ReadAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PLD_SEMI_HORA.txt").Skip(1).ToList();
            foreach (var line in pldLimitesLines)
            {

                var dados = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                if (Convert.ToInt32(dados[0]) == ano)
                {
                    limites.Ano = Convert.ToInt32(dados[0].Replace('.', ','));
                    limites.PldMin = Convert.ToDouble(dados[1].Replace('.', ','));
                    limites.PldMax = Convert.ToDouble(dados[2].Replace('.', ','));
                    limites.PldMaxEst = Convert.ToDouble(dados[3].Replace('.', ','));
                    if (limites.PldMin != pldMin || limites.PldMax != pldMax || limites.PldMaxEst != pldMaxEst)
                    {
                        if (System.Windows.Forms.MessageBox.Show("ATENÇÃO!!!\nOs valores informados são diferentes dos padrões.\nDeseja continuar?", "Trata PLD", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (System.Windows.Forms.MessageBox.Show("ATENÇÃO!!!\nDeseja atualizar os dados padrões para o ano informado?", "Trata PLD", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                var novotexto = new List<string>();
                                var texto = File.ReadAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PLD_SEMI_HORA.txt").ToList();
                                novotexto.Add(texto[0]);
                                for (int i = 1; i < texto.Count(); i++)
                                {
                                    var partes = texto[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                    var anoTeste = Convert.ToInt32(partes[0].Replace('.', ','));
                                    if (anoTeste == ano)
                                    {
                                        novotexto.Add(ano.ToString() + "\t" + pldMin.ToString().Replace('.', ',') + "\t" + pldMax.ToString().Replace('.', ',') + "\t" + pldMaxEst.ToString().Replace('.', ','));
                                    }
                                    else
                                    {
                                        novotexto.Add(texto[i]);
                                    }

                                }
                                File.WriteAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PLD_SEMI_HORA.txt", novotexto);

                            }
                            TrataPld(path, ano, pldMin, pldMax, pldMaxEst);
                            break;

                        }
                    }
                    else
                    {
                        TrataPld(path, ano, pldMin, pldMax, pldMaxEst);
                        break;
                    }
                }
            }
            if (limites.Ano == 0)
            {
                if (System.Windows.Forms.MessageBox.Show("ATENÇÃO!!!\nOs valores para o ano informado ainda não existem no cadastro.\nDeseja continuar?", "Trata PLD", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (System.Windows.Forms.MessageBox.Show("Deseja incluir os valores no cadastro?", "Trata PLD", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        var texto = File.ReadAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PLD_SEMI_HORA.txt").ToList();
                        texto.Add(ano.ToString() + "\t" + pldMin.ToString().Replace('.', ',') + "\t" + pldMax.ToString().Replace('.', ',') + "\t" + pldMaxEst.ToString().Replace('.', ','));
                        File.WriteAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PLD_SEMI_HORA.txt", texto);

                        TrataPld(path, ano, pldMin, pldMax, pldMaxEst);
                    }
                    else
                    {
                        TrataPld(path, ano, pldMin, pldMax, pldMaxEst);
                    }

                }
            }


        }
        public static void TrataPld(string path, int ano, double pldMin, double pldMax, double pldMaxEst)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var style = System.Globalization.NumberStyles.Any;
            var dir = path;
            var anoPld = ano;
            var limInf = pldMin;
            var limMax = pldMax;
            var limEst = pldMaxEst;
            int i;

            List<Tuple<int, string, double>> Plds = new List<Tuple<int, string, double>>();


            var pmoFile = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("pdo_cmosist.dat")).FirstOrDefault();
            if (pmoFile != null)
            {

                var linhas = File.ReadAllLines(pmoFile);
                foreach (var l in linhas)
                {
                    var campos = l.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    if (l != "")
                    {
                        if (int.TryParse(campos[0], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out i))
                        {
                            Tuple<int, string, double> Pld = new Tuple<int, string, double>(i, campos[2].Trim(), Convert.ToDouble(campos[3].Replace('.', ',')));
                            Plds.Add(Pld);

                        }
                    }

                }

                List<Tuple<int, double>> dadosSE = new List<Tuple<int, double>>();
                List<Tuple<int, double>> dadosSUL = new List<Tuple<int, double>>();
                List<Tuple<int, double>> dadosNE = new List<Tuple<int, double>>();
                List<Tuple<int, double>> dadosN = new List<Tuple<int, double>>();
                List<Tuple<int, double>> dadosFC = new List<Tuple<int, double>>();


                var PldSE = Plds.Where(x => x.Item2 == "SE").ToList();
                var PldSUL = Plds.Where(x => x.Item2 == "S").ToList();
                var PldNE = Plds.Where(x => x.Item2 == "NE").ToList();
                var PldN = Plds.Where(x => x.Item2 == "N").ToList();
                var PldFC = Plds.Where(x => x.Item2 == "FC").ToList();

                int hora = 1;
                for (int h = 1; h <= 48; h += 2)
                {
                    var hora1SE = PldSE.Where(x => x.Item1 == h).Select(x => x.Item3).First();
                    var hora2SE = PldSE.Where(x => x.Item1 == (h + 1)).Select(x => x.Item3).First();
                    var mediaSE = (hora1SE + hora2SE) / 2;
                    dadosSE.Add(new Tuple<int, double>(hora, mediaSE));

                    var hora1S = PldSUL.Where(x => x.Item1 == h).Select(x => x.Item3).First();
                    var hora2S = PldSUL.Where(x => x.Item1 == (h + 1)).Select(x => x.Item3).First();
                    var mediaS = (hora1S + hora2S) / 2;
                    dadosSUL.Add(new Tuple<int, double>(hora, mediaS));

                    var hora1NE = PldNE.Where(x => x.Item1 == h).Select(x => x.Item3).First();
                    var hora2NE = PldNE.Where(x => x.Item1 == (h + 1)).Select(x => x.Item3).First();
                    var mediaNE = (hora1NE + hora2NE) / 2;
                    dadosNE.Add(new Tuple<int, double>(hora, mediaNE));

                    var hora1N = PldN.Where(x => x.Item1 == h).Select(x => x.Item3).First();
                    var hora2N = PldN.Where(x => x.Item1 == (h + 1)).Select(x => x.Item3).First();
                    var mediaN = (hora1N + hora2N) / 2;
                    dadosN.Add(new Tuple<int, double>(hora, mediaN));

                    var hora1FC = PldFC.Where(x => x.Item1 == h).Select(x => x.Item3).First();
                    var hora2FC = PldFC.Where(x => x.Item1 == (h + 1)).Select(x => x.Item3).First();
                    var mediaFC = (hora1FC + hora2FC) / 2;
                    dadosFC.Add(new Tuple<int, double>(hora, mediaFC));

                    hora++;
                }

                var finalSE = GetPLdDessem(dadosSE, limInf, limMax, limEst);
                var finalSUL = GetPLdDessem(dadosSUL, limInf, limMax, limEst);
                var finalNE = GetPLdDessem(dadosNE, limInf, limMax, limEst);
                var finalN = GetPLdDessem(dadosN, limInf, limMax, limEst);
                var finalFC = GetPLdDessem(dadosFC, limInf, limMax, limEst);

                StringBuilder pldDados = new StringBuilder();
                pldDados.AppendFormat("{0,4}", "HORA");
                pldDados.AppendFormat("{0,6}", "SIST");
                pldDados.AppendFormat("{0,8}", "PLD");
                pldDados.AppendLine();

                foreach (var dad in finalSE)
                {
                    pldDados.AppendFormat("{0,4}", $"{dad.Item1}");
                    pldDados.AppendFormat("{0,6}", "SE");
                    pldDados.AppendFormat("{0,8}", $"{Math.Round(dad.Item2, 2)}");
                    pldDados.AppendLine();

                }
                foreach (var dad in finalSUL)
                {
                    pldDados.AppendFormat("{0,4}", $"{dad.Item1}");
                    pldDados.AppendFormat("{0,6}", "S");
                    pldDados.AppendFormat("{0,8}", $"{Math.Round(dad.Item2, 2)}");
                    pldDados.AppendLine();

                }
                foreach (var dad in finalNE)
                {
                    pldDados.AppendFormat("{0,4}", $"{dad.Item1}");
                    pldDados.AppendFormat("{0,6}", "NE");
                    pldDados.AppendFormat("{0,8}", $"{Math.Round(dad.Item2, 2)}");
                    pldDados.AppendLine();

                }
                foreach (var dad in finalN)
                {
                    pldDados.AppendFormat("{0,4}", $"{dad.Item1}");
                    pldDados.AppendFormat("{0,6}", "N");
                    pldDados.AppendFormat("{0,8}", $"{Math.Round(dad.Item2, 2)}");
                    pldDados.AppendLine();

                }
                foreach (var dad in finalFC)
                {
                    pldDados.AppendFormat("{0,4}", $"{dad.Item1}");
                    pldDados.AppendFormat("{0,6}", "FC");
                    pldDados.AppendFormat("{0,8}", $"{Math.Round(dad.Item2, 2)}");
                    pldDados.AppendLine();

                }
                File.WriteAllText(Path.Combine(dir, "PLD_HORARIO.txt"), pldDados.ToString());

                var textoFinal = "Processo realizado com sucesso!";
                Program.AutoClosingMessageBox.Show(textoFinal, "Trata PLD", 5000);
            }
            else
            {
                var textoFinal = "pdo_cmosist.dat não existe!!!. Encerrando processo.";
                Program.AutoClosingMessageBox.Show(textoFinal, "Trata PLD", 30000);
            }
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

        public static void ResDataBaseTools(string path)
        {
            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                Thread thread = new Thread(ResDatabaseToolsTHSTA);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(dir);
                thread.Join(); //Wait for the thread to end    
                               // var frm = new FrmExtriDE(dir);
                               //frm.ShowDialog();

            }
            catch (Exception ex)
            {
                string texto = ex.Message.ToString();
                MessageBox.Show(texto, "ATENCÃO!");
                return;
            }
        }

        public static void dessemTools(string path)
        {
            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                }
                else
                    return;

                Thread thread = new Thread(dessemToolsTHSTA);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(dir);
                thread.Join(); //Wait for the thread to end    
                               // var frm = new FrmExtriDE(dir);
                               //frm.ShowDialog();

            }
            catch (Exception ex)
            {
                string texto = ex.Message.ToString();
                MessageBox.Show(texto, "ATENCÃO!");
                return;
            }
        }

        public static void extraiDE(string path)
        {
            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                }
                else
                    return;

                Thread thread = new Thread(blocoDETHSTA);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(dir);
                thread.Join(); //Wait for the thread to end    
                               // var frm = new FrmExtriDE(dir);
                               //frm.ShowDialog();

            }
            catch (Exception ex)
            {
                string texto = ex.Message.ToString();
                MessageBox.Show(texto, "ATENCÃO!");
                return;
            }

        }

        static void atualizaDp(string path)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var style = System.Globalization.NumberStyles.Any;
            string dirAux = string.Empty;
            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                }
                else
                    return;
                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);
                dirAux = Path.Combine(dir, "Auxiliar");

                if (System.Windows.Forms.MessageBox.Show("Deseja Atualizar o blocoDP dos decks gerados?", "Conversão Decodess", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (!Directory.Exists(dirAux))
                    {
                        Directory.CreateDirectory(dirAux);
                    }
                    var frm = new FrmDataDessemDp(true, dirAux);
                    frm.ShowDialog();


                    var diasAtualizar = File.ReadAllLines(Path.Combine(dirAux, "diaAtualizar.txt")).ToList();

                    foreach (var dirDp in dirs.Where(x => !x.Split('\\').Last().ToLower().Contains("base")).ToList())
                    {
                        var dadvazFile = Directory.GetFiles(dirDp).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz.dat")).First();
                        var entdadosFile = Directory.GetFiles(dirDp).Where(x => Path.GetFileName(x).ToLower().Contains("entdados.dat")).First();
                        if (dadvazFile != null && entdadosFile != null)
                        {
                            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

                            var dadvaz = DocumentFactory.Create(dadvazFile) as Compass.CommomLibrary.Dadvaz.Dadvaz;
                            var dataline = dadvaz.BlocoData.First();
                            DateTime dataDeck = new DateTime(dataline.Ano, dataline.Mes, dataline.Dia);

                            var datarev = dataDeck;
                            if (dataDeck.DayOfWeek == DayOfWeek.Friday)
                            {
                                datarev = datarev.AddDays(-1);
                            }

                            var revisao = Tools.GetCurrRev(datarev);

                            string diaAbrev = "";
                            switch (dataDeck.DayOfWeek)
                            {
                                case DayOfWeek.Saturday:
                                    diaAbrev = "SAB";
                                    break;
                                case DayOfWeek.Sunday:
                                    diaAbrev = "DOM";
                                    break;
                                case DayOfWeek.Monday:
                                    diaAbrev = "SEG";
                                    break;
                                case DayOfWeek.Tuesday:
                                    diaAbrev = "TER";
                                    break;
                                case DayOfWeek.Wednesday:
                                    diaAbrev = "QUA";
                                    break;
                                case DayOfWeek.Thursday:
                                    diaAbrev = "QUI";
                                    break;
                                case DayOfWeek.Friday:
                                    diaAbrev = "SEX";
                                    break;
                                default:
                                    diaAbrev = "";
                                    break;

                            }
                            if (diasAtualizar.Any(x => x.Equals(diaAbrev)))
                            {
                                var inicioRev = revisao.revDate.AddDays(-6);
                                int index = 0;
                                for (DateTime d = inicioRev; d <= revisao.revDate; d = d.AddDays(1))
                                {
                                    if (d <= dataDeck)
                                    {
                                        index++;
                                    }
                                }
                                var dpFileCSV = Directory.GetFiles(dirAux).Where(x => Path.GetFileName(x).Contains($"blocoDPcarga{index}.csv")).First();
                                //var dplines = File.ReadAllLines(dpFile, Encoding.UTF8);
                                var dplines = File.ReadAllLines(dpFileCSV).ToList();

                                List<Tuple<int, int, float>> dadosCarga = new List<Tuple<int, int, float>>();



                                File.Copy(dpFileCSV, Path.Combine(dirDp, dpFileCSV.Split('\\').Last()), true);


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
                                    for (DateTime d = dataDeck; d <= revisao.revDate; d = d.AddDays(1))//
                                    {
                                        if (d == dataDeck)
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
                                            var NewdpFileCSV = Directory.GetFiles(dirAux).Where(x => Path.GetFileName(x).Contains($"blocoDPcarga{index}.csv")).First();
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
                                            bool pat2024 = d.Year == 2024;
                                            bool pat2025 = d.Year >= 2025;
                                            var intervalosAgruped = Tools.GetIntervalosPatamares(d, pat2023, pat2024, pat2025);

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



                                            File.Copy(NewdpFileCSV, Path.Combine(dirDp, NewdpFileCSV.Split('\\').Last()), true);

                                        }
                                    }
                                }

                                var newDP11 = new Compass.CommomLibrary.EntdadosDat.DpLine();
                                newDP11.IdBloco = "DP";
                                newDP11.Subsist = 11;
                                newDP11.DiaInic = $"{dataDeck.Day:00}";
                                newDP11.HoraInic = 0;
                                newDP11.MeiaHoraInic = 0;
                                newDP11.DiaFinal = " F";
                                newDP11.Demanda = 0.0f;
                                entdados.BlocoDp.Add(newDP11);

                                entdados.SaveToFile();
                            }
                        }
                    }
                    //hj
                    if (Directory.Exists(dirAux))
                    {
                        Directory.Delete(dirAux, true);
                    }
                    string texto = "Atualização realizada com sucesso!";
                    MessageBox.Show(texto, "ATENCÃO!");
                }
            }
            catch (Exception ex)
            {
                if (Directory.Exists(dirAux))
                {
                    Directory.Delete(dirAux, true);
                }
                string texto = ex.Message.ToString();
                MessageBox.Show(texto, "ATENCÃO!");
                return;
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void ColetaLimites(string path)
        {
            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                }
                else
                    return;

                var excelname = Path.Combine(dir, "Coleta_Limites.xlsm");//  Acompanhamento_Limites_Elétricos.xlsm
                //var template = @"C:\Development\Implementacoes\TESTES_PEGALIMITES\Acompanhamento_Limites_Elétricos.xlsm";
                var template = @"K:\enercore_ctl_common\Coleta\Coleta_Limites.xlsm";
                File.Copy(template, excelname, true);
                //Microsoft.Office.Interop.Excel.Application xlApp = null;
                Microsoft.Office.Interop.Excel.Application xlApp = ExcelTools.Helper.StartExcelInvisible();
                //xlApp = ExcelTools.Helper.StartExcelInvisible();
                //xlApp = ExcelTools.Helper.StartExcel();
                xlApp.AskToUpdateLinks = false;

                xlApp.DisplayAlerts = false;

                var wbxls = xlApp.Workbooks.Open(excelname);
                Workbook wb = xlApp.ActiveWorkbook;

                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);
                foreach (var di in dirs.ToList())
                {
                    var relato = Directory.GetFiles(di).Where(x => Path.GetFileName(x).ToLower().Equals("relato.rv0")).FirstOrDefault();

                    if (relato != null)
                    {
                        string anoMes = di.Split('\\').Last();
                        string ano = anoMes.Substring(2, 2);
                        string Mes = "";
                        switch (anoMes.Substring(4, 2))
                        {
                            case "01":
                                Mes = "JAN";
                                break;
                            case "02":
                                Mes = "FEV";
                                break;
                            case "03":
                                Mes = "MAR";
                                break;
                            case "04":
                                Mes = "ABR";
                                break;
                            case "05":
                                Mes = "MAI";
                                break;
                            case "06":
                                Mes = "JUN";
                                break;
                            case "07":
                                Mes = "JUL";
                                break;
                            case "08":
                                Mes = "AGO";
                                break;
                            case "09":
                                Mes = "SET";
                                break;
                            case "10":
                                Mes = "OUT";
                                break;
                            case "11":
                                Mes = "NOV";
                                break;
                            case "12":
                                Mes = "DEZ";
                                break;
                            default:
                                Mes = "";
                                break;
                        }
                        string NomePlan = "lim_" + Mes + ano;

                        List<string> reealvo = new List<string>();
                        Sheets lista = wb.Worksheets;
                        var N_Sheets = lista.Count;

                        Worksheet wsPega = wbxls.Worksheets["Pega_limites"];
                        Worksheet ws = wbxls.Worksheets[NomePlan];
                        int idx = 1;
                        for (int i = 1; i <= N_Sheets; i++)
                        {
                            if (wbxls.Worksheets[i] == wsPega)
                            {
                                idx = i;
                                break;
                            }
                        }
                        //wsPega.Copy(wbxls.Worksheets[idx + 1]);
                        //Worksheet ws = (Worksheet)xlApp.ActiveSheet;

                        //int c = 2;
                        //for (int r = 4; !string.IsNullOrWhiteSpace(ws.Cells[r, c].Text); r++)
                        //{
                        //    reealvo.Add((string)ws.Cells[r, c].Text);
                        //}

                        int c = 2;
                        for (int r = 4; !string.IsNullOrWhiteSpace(wsPega.Cells[r, c].Text); r++)
                        {
                            reealvo.Add((string)wsPega.Cells[r, c].Text);
                        }
                        var relatotxt = File.ReadAllText(relato);
                        var relatoPartes = relatotxt.Split(new string[] { "Relatorio das Restricoes Eletricas no  Patamar" }, StringSplitOptions.None).Skip(1).Take(3).ToList();
                        List<Tuple<int, double[]>> dados = new List<Tuple<int, double[]>>();
                        foreach (var ra in reealvo)
                        {
                            int patamar = 1;
                            int ree = Convert.ToInt32(ra);
                            double[] limites = new double[6];
                            foreach (var rp in relatoPartes)
                            {
                                int indice = 0;
                                //var lines = File.ReadAllLines(rp).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();//new string[] { "\r\n", "\n" },
                                var lines = rp.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                                foreach (var l in lines)
                                {
                                    var partes = l.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    if (partes[0] == ra)
                                    {
                                        double v;
                                        limites[patamar + 2] = double.TryParse(partes.Last().Replace('.', ','), out v) ? v : 0;
                                        indice = lines.IndexOf(l);
                                        bool ftotal = false;
                                        while (ftotal == false)
                                        {
                                            var npartes = lines[indice + 1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                            if (npartes[0] == "Total")
                                            {
                                                double x;
                                                limites[patamar - 1] = double.TryParse(npartes[1].Replace('.', ','), out x) ? x : 0;
                                                ftotal = true;
                                            }
                                            else
                                            {
                                                indice++;
                                            }
                                        }
                                        break;
                                    }
                                }
                                patamar++;
                            }
                            dados.Add(new Tuple<int, double[]>(ree, limites));

                        }

                        int row = 4;
                        foreach (var dad in dados)
                        {
                            ws.Cells[row, 2] = dad.Item1;
                            ws.Cells[row, 3] = dad.Item2[0];
                            ws.Cells[row, 4] = dad.Item2[1];
                            ws.Cells[row, 5] = dad.Item2[2];
                            ws.Cells[row, 6] = dad.Item2[3];
                            ws.Cells[row, 7] = dad.Item2[4];
                            ws.Cells[row, 8] = dad.Item2[5];
                            row++;
                        }
                        ws.Name = "lim_" + Mes + ano;//lim_DEZ21

                    }
                }

                wbxls.Save();
                wbxls.Close(SaveChanges: false);
                //xlApp.Quit();

                string message = "Coleta concluída com sucesso!";
                System.Windows.Forms.MessageBox.Show(message);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);

            }
        }
        static void uhDessem(string path)
        {
            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                }
                else
                    return;
                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);


                var texto = "Escolha o arquivo base!";
                MessageBox.Show(texto, "Caption");

                Thread thread = new Thread(GetPDO_OPER);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(dirs);
                thread.Join(); //Wait for the thread to end 






            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        static void pldDessem(string commands)
        {

            var command = commands.Split('|');

            var path = command[0];
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            if (command.Count() > 1 && command[1] == "true")
            {
                var ano = DateTime.Today.Year;
                CommomLibrary.PldDessem.PldDessem limites = new CommomLibrary.PldDessem.PldDessem();


                var pldLimitesLines = File.ReadAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PLD_SEMI_HORA.txt").Skip(1).ToList();
                foreach (var line in pldLimitesLines)
                {
                    var dados = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if (Convert.ToInt32(dados[0]) == ano)
                    {
                        limites.Ano = Convert.ToInt32(dados[0].Replace('.', ','));
                        limites.PldMin = Convert.ToDouble(dados[1].Replace('.', ','));
                        limites.PldMax = Convert.ToDouble(dados[2].Replace('.', ','));
                        limites.PldMaxEst = Convert.ToDouble(dados[3].Replace('.', ','));
                    }
                }
                if (limites.Ano == 0)
                {
                    var dados = pldLimitesLines.Last().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    limites.Ano = Convert.ToInt32(dados[0].Replace('.', ','));
                    limites.PldMin = Convert.ToDouble(dados[1].Replace('.', ','));
                    limites.PldMax = Convert.ToDouble(dados[2].Replace('.', ','));
                    limites.PldMaxEst = Convert.ToDouble(dados[3].Replace('.', ','));
                }
                TrataPld(path, limites.Ano, limites.PldMin, limites.PldMax, limites.PldMaxEst);
            }
            else
            {
                Thread thread = new Thread(pldDessemTHSTA);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(path);
                thread.Join(); //Wait for the thread to end  
            }




        }
        static void DirectoryPath(object caminho)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"C:\";
            fbd.Description = "SELECIONE O DECK DECOMP";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var folderName = fbd.SelectedPath;
                if (Directory.Exists(folderName))
                {
                    File.WriteAllText(Path.Combine(caminho.ToString(), "dir.txt"), folderName);
                }
            }
            else
            {
                File.WriteAllText(Path.Combine(caminho.ToString(), "dir.txt"), "");
            }
        }

        static void MapcutCortedeco(object caminho)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"C:\";
            fbd.Description = "SELECIONE O DECK DECOMP";

            string mapcut = "mapcut.rv";
            string cortdeco = "cortdeco.rv";


            int contArq = 0;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var folderName = fbd.SelectedPath;
                if (Directory.Exists(folderName))
                {
                    var arqs = Directory.GetFiles(folderName).ToList();
                    foreach (var arq in arqs)
                    {
                        var filename = Path.GetFileName(arq);
                        if ((filename.ToLower().Contains(mapcut)) || (filename.ToLower().Contains(cortdeco)))
                        {
                            File.Copy(arq, Path.Combine(caminho.ToString(), filename), true);
                            contArq++;
                        }
                    }

                }
            }
        }

        static void Corteshdecodess(object caminho)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "cortesh.*|cortesh.*";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var arqName = ofd.FileName;
                if (arqName != null || arqName != "")
                {
                    File.Copy(ofd.FileName, Path.Combine(caminho.ToString(), ofd.FileName.Split('\\').Last()), true);
                }
            }
        }

        static void GetPDO_OPER(object dirs)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDO_OPER_USIH.*|PDO_OPER_USIH.*";//"PDO_OPER_USIH.*|pdo_oper_usih.*"
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var arqName = ofd.FileName;
                if (arqName != null || arqName != "")
                {
                    var pdoOper = File.ReadAllLines(ofd.FileName);
                    string linhadata = pdoOper.Where(x => x.Contains("Data do Caso")).First();
                    DateTime dataPdo = Convert.ToDateTime(linhadata.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Last(), Culture.DateTimeFormat);
                    var datarev = dataPdo;
                    if (dataPdo.DayOfWeek == DayOfWeek.Friday)
                    {
                        datarev = datarev.AddDays(-1);
                    }

                    var revisao = Tools.GetCurrRev(datarev);
                    List<Tuple<int, int, float, float>> UHS = new List<Tuple<int, int, float, float>>();
                    var feriados = Tools.feriados;
                    int atualizados = 0;
                    string decksAtualizados = "Decks atualizados:\n";
                    for (int i = 62; i < pdoOper.Count(); i++)
                    {

                        if (pdoOper[i] != "")
                        {
                            float d = 0;
                            var campos = pdoOper[i].Split(';').ToList();
                            if (int.TryParse(campos[0],out int r))
                            {
                                var hora = Convert.ToInt32(campos[0]);
                                var usina = Convert.ToInt32(campos[2]);
                                var volIni = float.TryParse(campos[6], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out d) ? d : 0;
                                var volFim = float.TryParse(campos[8], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out d) ? d : 0;
                                UHS.Add(new Tuple<int, int, float, float>(hora, usina, volIni, volFim));
                            }
                            
                        }

                    }


                    foreach (var dir in (string[])dirs)
                    {

                        var dadvazFile = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz.dat")).FirstOrDefault();
                        var entdadosFile = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("entdados.dat")).FirstOrDefault();
                        if (dadvazFile != null && entdadosFile != null)
                        {
                            var dadvaz = DocumentFactory.Create(dadvazFile) as Compass.CommomLibrary.Dadvaz.Dadvaz;
                            var dataline = dadvaz.BlocoData.First();
                            DateTime dataDeck = new DateTime(dataline.Ano, dataline.Mes, dataline.Dia);

                            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
                            var uhLines = entdados.BlocoUh.ToList();
                            int horaUHS = 44;
                            int incremento = 0;
                            for (DateTime dia = dataPdo; dia <= revisao.revDate; dia = dia.AddDays(1))
                            {
                                if (dia == dataDeck)
                                {
                                    horaUHS = horaUHS + incremento;
                                }
                                incremento = incremento + 4;
                            }

                            if (dataDeck >= dataPdo && dataDeck <= revisao.revDate)
                            {
                                if (dataDeck == dataPdo)
                                {
                                    foreach (var line in uhLines)
                                    {

                                        var vol = UHS.Where(x => x.Item1 == 1 && x.Item2 == line.Usina).Select(x => x.Item3).FirstOrDefault();
                                        if (vol > 0)
                                        {
                                            line.VolArm = vol;
                                        }

                                    }

                                }
                                else
                                {
                                    foreach (var line in uhLines)
                                    {
                                        var vol = UHS.Where(x => x.Item1 == horaUHS && x.Item2 == line.Usina).Select(x => x.Item4).First();
                                        if (vol > 0)
                                        {
                                            line.VolArm = vol;
                                        }

                                    }
                                }

                                atualizados++;
                                decksAtualizados = decksAtualizados + dir.Split('\\').Last() + "\n";

                                entdados.SaveToFile(createBackup: true);

                            }
                        }

                    }

                    if (atualizados > 0)
                    {
                        var texto = decksAtualizados;
                        MessageBox.Show(texto, "Caption");
                    }
                    else
                    {
                        var texto = "Os decks não foram atualizados.Arquivo base incompatível.";
                        MessageBox.Show(texto, "Caption");
                    }

                    //File.Copy(ofd.FileName, Path.Combine(dirs.ToString(), ofd.FileName.Split('\\').Last()), true);
                }
            }
        }

        public static void Completa_SemanaDessem(Compass.CommomLibrary.Dessem.Deck deck, DateTime dateDeck, DateTime dateFim, bool expandEst = false)
        {
            DateTime dateIni = dateDeck.AddDays(1);
            var partsDir = deck.BaseFolder.Split('\\').Last();
            int incremento = 1;
            string tipo = expandEst ? "Expand" : "";

            //bool modifRenovaveis = false;
            //if (System.Windows.Forms.MessageBox.Show("Deseja Modificar arquivo Renovavies?", "DESSEM-TOOLS", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    modifRenovaveis = true;
            //}

            for (DateTime d = dateIni; d <= dateFim; d = d.AddDays(1))
            {
                string folder = Path.Combine(deck.BaseFolder.Replace(partsDir, ""), $"Dessem_Rev{tipo}-" + d.ToString("dd-MM-yyyy"));
                deck.CopyFilesToFolder(folder);
                CriarDadvazSemanal(folder, incremento, dateDeck, dateFim, d);
                CriarCotasr11(folder, d);
                CriarDeflant(folder, incremento, dateDeck, d);
                CriarOperut(folder, incremento, dateDeck, d);
                CriarPtoper(folder, incremento, dateDeck);
                CriarOperuh(folder, incremento, dateDeck, d, dateFim);
                CriarEntdados(folder, incremento, dateDeck, d, expandEst, dateFim);
                //if (modifRenovaveis)
                //{
                //CriarRenovaveis(folder, incremento, dateDeck, d, expandEst);
                //}
                CriarRespot(folder, incremento, dateDeck, d, expandEst);
                incremento++;
            }
        }

        static void convDecodess(string commands)
        {

            try
            {
                if (System.Windows.Forms.MessageBox.Show("Deseja Modificar os dados de carga (Bloca DP)?", "Conversão Decodess", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    var frm = new FrmDataDessemDp();
                    var te = frm.ShowDialog();

                }



                string camArqsBase = "H:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base";
                //string camArqsBase = "N:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base";
                var arqsBase = Directory.GetFiles(camArqsBase).ToList();
                List<string> arqsBaseAlvo = new List<string> { "renovaveis", "ils_tri", "config", "decodess", "cadterm", "curvtviag", "cotasr11", "areacont", "rstlpp", "respotele", "restseg", "rampas", "respot" };
                DateTime dataEstudo = new DateTime();
                string camCortes = string.Empty;


                var command = commands.Split('|');

                var path = command[0];
                var cloneDir = "";

                try
                {
                    string dir;
                    if (Directory.Exists(path))
                    {
                        dir = path;
                    }
                    else if (File.Exists(path))
                    {
                        dir = Path.GetDirectoryName(path);
                    }
                    else
                        return;

                    var dirInfo = new DirectoryInfo(dir);
                    var parentDir = dirInfo.Parent.FullName;
                    var dirName = dirInfo.Name + "_Decodess";

                    var i = 0;
                    do
                    {
                        cloneDir = Path.Combine(parentDir, dirName + " (" + ++i + ")");
                    } while (Directory.Exists(cloneDir));

                    if (!Directory.Exists(cloneDir))
                    {
                        Directory.CreateDirectory(cloneDir);
                    }

                    var deck = DeckFactory.CreateDeck(dir) as Compass.CommomLibrary.Decomp.Deck;

                    if (!(deck is Compass.CommomLibrary.Decomp.Deck))
                    {
                        throw new NotImplementedException("Deck não reconhecido para a execução");
                    }
                    if (deck is Compass.CommomLibrary.Decomp.Deck)
                    {
                        List<string> arqsAlvo = new List<string> { "dadger", "vazoes.rv", "hidr", "mapcut", "cortdeco", "mlt", "dadgnl", "relato.rv", "relgnl" };
                        var arqsDecomp = Directory.GetFiles(dir).ToList();
                        foreach (var arqs in arqsDecomp.Where(x => (!x.ToLower().Contains(".bak")) && (!x.ToLower().Contains(".origjirstoant")) && (!x.ToLower().Contains(".temp.modif"))).ToList())
                        {
                            var fileName = Path.GetFileName(arqs);
                            foreach (var item in arqsAlvo)
                            {
                                if (fileName.ToLower().Contains(item))
                                {
                                    File.Copy(arqs, Path.Combine(cloneDir, fileName), true);
                                }
                            }
                        }

                        foreach (var arq in arqsBase)
                        {
                            var fileBase = Path.GetFileName(arq);
                            foreach (var item in arqsBaseAlvo)
                            {
                                if (fileBase.ToLower().Contains(item))
                                {
                                    File.Copy(arq, Path.Combine(cloneDir, fileBase), true);
                                }
                            }
                        }
                        var dadgerFile = Directory.GetFiles(cloneDir).Where(x => Path.GetFileName(x).ToLower().Contains("dadger")).First();
                        //// var deckDecodessBase = DeckFactory.CreateDeck(cloneDir) as Compass.CommomLibrary.Decomp.Deck;
                        var dadgerBase = deck[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
                        var dadger = DocumentFactory.Create(dadgerFile) as Compass.CommomLibrary.Dadger.Dadger;

                        var dadosRhes = Services.GeraDessem.GetDadosRhe(dadger);

                        var custoCVU = GetCVU(dir);

                        #region ajustaBlocoCICE
                        var blocoCICE = Services.GeraDessem.AjustaBlocoCICE(dadger);
                        #endregion

                        dataEstudo = dadger.DataEstudo;
                        var fc = (Compass.CommomLibrary.Dadger.FcBlock)dadger.Blocos["FC"];

                        camCortes = dadgerBase.CortesPath.ToLower().Replace("cortes.dat", "cortesh.dat");
                        if (File.Exists(camCortes))
                        {
                            File.Copy(camCortes, Path.Combine(cloneDir, camCortes.Split('\\').Last()));
                        }
                        else
                        {
                            if (command.Count() < 2)
                            {
                                var texto = "Caminho do arquivo Cortesh.dat não existe, defina um caminho existente";
                                MessageBox.Show(texto, "ATENCÃO!");

                                Thread thread = new Thread(Corteshdecodess);
                                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                                thread.Start(cloneDir);
                                thread.Join(); //Wait for the thread to end      


                            }
                        }
                        var arqsClonedir = Directory.GetFiles(cloneDir);
                        if (arqsClonedir.All(x => !x.ToLower().Contains("cortesh")))
                        {
                            throw new NotImplementedException("Cortesh não encontrado para a execução");
                        }

                        var todosArqs = arqsAlvo.Union(arqsBaseAlvo);
                        foreach (var item in todosArqs)
                        {
                            if (arqsClonedir.All(x => !x.ToLower().Contains(item)))
                            {
                                throw new NotImplementedException($"{item} não encontrado para a execução");
                            }
                        }



                        dadger.SaveToFile();
                        TrataBlocoRHEDessem(dadgerFile);//desloca as colunas referentes ao numero das restrições para a esquerda pq o decodess esta considerando as colunas de 5 a 7 
                        var configFile = Directory.GetFiles(cloneDir).Where(x => Path.GetFileName(x).ToLower().Contains("config.dat")).First();
                        var decodessFile = Directory.GetFiles(cloneDir).Where(x => Path.GetFileName(x).ToLower().Contains("decodess.arq")).First();

                        TrataConfig(configFile, dataEstudo);
                        TrataDecodessArq(decodessFile, cloneDir);
                        ExecutaDecodess(cloneDir);

                        if (!File.Exists(Path.Combine(cloneDir, "dessem.arq")))
                        {
                            string texto = "Arquivo dessem.arq não existente!\n";
                            var linhas = File.ReadAllLines(Path.Combine(cloneDir, "decodess.log")).ToList();
                            var erros = linhas.Where(x => x.StartsWith("ERRO:"));
                            foreach (var erro in erros)
                            {
                                texto = texto + erro + "\n";
                            }
                            MessageBox.Show(texto, "Falha na Conversão Decodess!");
                            return;
                        }

                        if (command.Count() < 2)
                        {
                            if (System.Windows.Forms.MessageBox.Show("Deseja criar decks diários? ", "Decodess", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                CriarDecksDiarios(cloneDir, dataEstudo, blocoCICE, dadosRhes, custoCVU);
                            }
                        }

                        else if (command.Count() > 1 && command[1] == "true")
                        {
                            CriarDecksDiarios(cloneDir, dataEstudo, blocoCICE, dadosRhes, custoCVU);
                        }
                        var textoFinal = "Conversão realizada com sucesso!";
                        Program.AutoClosingMessageBox.Show(textoFinal, "Caption", 5000);
                        if (command.Count() > 1 && command[1] == "true")
                        {

                            Compass.CommomLibrary.Tools.SendMail(textoFinal, "bruno.araujo@enercore.com.br; pedro.modesto@enercore.com.br; natalia.biondo@enercore.com.br; gabriella.radke@enercore.com.br;", "Conversão Decodess");

                        }
                    }


                    //deck.CopyFilesToFolder(cloneDir);

                    //dynamic cceeDeck = DeckFactory.CreateDeck(cloneDir);
                }
                catch (Exception ex)
                {
                    if (command.Count() > 1 && command[1] == "true")
                    {
                        var texto = ex.ToString();
                        if (ex.ToString().Contains("reconhecido"))
                        {
                            texto = "Deck não reconhecido para a execução por falta de arquivos!";
                        }
                        Compass.CommomLibrary.Tools.SendMail(texto, "bruno.araujo@enercore.com.br; pedro.modesto@enercore.com.br; natalia.biondo@enercore.com.br; gabriella.radke@enercore.com.br;", "Falha na conversão Decodess");
                        if (Directory.Exists(cloneDir))
                        {
                            Directory.Delete(cloneDir, true);
                        }
                    }
                    else
                    {
                        var texto = "Falha na execução! Erro: " + ex.ToString();
                        MessageBox.Show(texto, "Caption");
                        if (Directory.Exists(cloneDir))
                        {
                            Directory.Delete(cloneDir, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string texto = e.Message.ToString();
                MessageBox.Show(texto, "ATENCÃO!");
                return;
            }


        }

        public static void CriarDecksDiarios(string dirBase, DateTime dataEstudo, Compass.CommomLibrary.EntdadosDat.CiceBlock blocoCICE, List<Tuple<int, float>> dadosRhe, List<Tuple<string, float>> custoCVU)
        {
            var rev = Tools.GetCurrRev(dataEstudo);
            //var camDessemDiario = $@"L:\7_dessem\DecksDiarios\{rev.revDate:MM_yyyy}\RV{rev.rev}\{DateTime.Now:dd_MM_yyyy_HH_mm_ss}";
            var camDessemDiario = $@"K:\5_dessem\DecksDiarios\{rev.revDate:MM_yyyy}\RV{rev.rev}\{DateTime.Now:dd_MM_yyyy_HH_mm_ss}";
            var camdessemBase = camDessemDiario + "\\Base";
            if (!Directory.Exists(camdessemBase))
            {
                Directory.CreateDirectory(camdessemBase);
            }
            var arqsBase = Directory.GetFiles(dirBase);

            foreach (var arq in arqsBase)
            {
                File.Copy(arq, Path.Combine(camdessemBase, arq.Split('\\').Last()), true);
            }

            DownloadOperuh(camdessemBase, dataEstudo, rev.revDate);

            bool alteraRenovaveis = false;
            if (System.Windows.Forms.MessageBox.Show("Deseja modificar o arquivo Renovaveis", "Decodess", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                // caso o usuario esteja convertendo um deck cuja data ainda não tenha decks weol correspondentes para o preenchimento do renovaveis 
                // ele escolhe não alterar o renovaveis
                alteraRenovaveis = true;
            }


            for (DateTime d = dataEstudo; d <= rev.revDate; d = d.AddDays(1))
            {
                var path = $"{camDessemDiario}\\Dessem_{d:dd_MM_yyyy}";
                CopiaArqsDessem(camdessemBase, path, d, rev.revDate);
                CopiaTermdat(path);
                TrataPtoper(path, d);
                ComentaDessemArq(path);
                CriarDadvaz(path, d);
                Services.GeraDessem.CriarEntdados(path, d, rev.revDate, blocoCICE, dadosRhe);

                TrataOperut(path, d, rev.revDate, custoCVU);

                Services.GeraDessem.TrataRespot(path, d);
                Services.GeraDessem.RestsegRstlppCopy(path, d, rev.revDate);
                Services.GeraDessem.TrataOperuh(path, d, rev.revDate);
                if (alteraRenovaveis)
                {
                    Services.GeraDessem.Renovaveis(path, d, rev.revDate);
                }
                //Services.GeraDessem.TrataDeflant(path, d);apenas testes
            }
        }

        public static void DownloadOperuh(string dirBase, DateTime ini, DateTime fim)
        {

            string localpath = Path.Combine(Path.GetTempPath(), "operuh");


            if (!Directory.Exists(localpath))
            {
                Directory.CreateDirectory(localpath);
            }

            bool existe = false;
            int cont = 0;
            ChromeOptions options = new ChromeOptions();

            options.AddUserProfilePreference("download.default_directory", localpath);
            options.AddArgument("--no-sandbox");
            options.AddArgument("--verbose");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-software-rasterizer");

            IWebDriver driver = new ChromeDriver("H:\\TI - Sistemas\\UAT\\PricingExcelTools\\files\\chromedriver_win32", options);

            try
            {
                //driver.Manage().Window.Maximize();
                driver.Url = "https://integracaoagentes.ons.org.br/FSAR-H/SitePages/Exibir_Forms_FSARH.aspx#";
                driver.FindElement(By.Id("username")).SendKeys("bruno.araujo@cpas.com.br");
                driver.FindElement(By.Name("submit.IdentificarUsuario")).Click();
                driver.FindElement(By.Id("password")).SendKeys("Br@compass");
                driver.FindElement(By.Name("submit.Signin")).Click();
                Thread.Sleep(10000);

                for (DateTime d = ini; d <= fim; d = d.AddDays(1))
                {
                    existe = false;
                    cont = 0;
                    while (existe == false && cont <= 5)
                    {

                        driver.FindElement(By.Id("buttonGerarArq")).Click();
                        Thread.Sleep(2000);

                        driver.FindElement(By.Id("dessemDataInicial")).Clear();
                        Thread.Sleep(1000);

                        driver.FindElement(By.Id("dessemDataInicial")).SendKeys(d.ToString("dd/MM/yyyy"));
                        Thread.Sleep(1000);

                        driver.FindElement(By.Id("dessemDataFinal")).Clear();
                        Thread.Sleep(1000);

                        driver.FindElement(By.Id("dessemDataFinal")).SendKeys(fim.ToString("dd/MM/yyyy"));
                        Thread.Sleep(1000);

                        driver.FindElement(By.Id("gerarTxtDessem")).Click();

                        // WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                        Thread.Sleep(7000);
                        // wait.Until<bool>(x => existe = Directory.GetFiles(localpath).Any(y => Path.GetFileName(y).ToLower().Contains("operuh") && Path.GetFileName(y).ToLower().EndsWith(".dat")));


                        var arq = Directory.GetFiles(localpath).Where(y => Path.GetFileName(y).ToLower().Contains("operuh") && Path.GetFileName(y).ToLower().EndsWith(".dat")).FirstOrDefault();

                        if (arq == null || arq == "")
                        {
                            Thread.Sleep(10000);
                            arq = Directory.GetFiles(localpath).Where(y => Path.GetFileName(y).ToLower().Contains("operuh") && Path.GetFileName(y).ToLower().EndsWith(".dat")).FirstOrDefault();

                        }

                        if (File.Exists(arq))
                        {
                            File.Move(arq, Path.Combine(dirBase, $"operuh_{d:ddMMyyyy}.DAT"));
                            existe = true;
                            if (File.Exists(arq))
                            {
                                File.Delete(arq);
                            }
                        }
                        else
                        {
                            cont++;
                        }
                        if (cont > 5)
                        {
                            throw new NotImplementedException("Falha ao baixar operuh.dat!!! Tentativas excedidas");
                        }
                    }

                }

                if (Directory.Exists(localpath))
                {
                    Directory.Delete(localpath, true);
                }
                driver.Quit();
            }
            catch (Exception e)
            {
                if (Directory.Exists(localpath))
                {
                    Directory.Delete(localpath, true);
                }
                if (Directory.Exists(dirBase))
                {
                    Directory.Delete(dirBase, true);
                }
                driver.Quit();
                throw;
            }
        }
        public static void CopiaTermdat(string path)
        {
            //string camArqsBase = "H:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base";
            string pastaRec = GetPastaRecente(DateTime.Today);
            var termdatFile = Directory.GetFiles(pastaRec).Where(x => Path.GetFileName(x).ToLower().Contains("termdat.dat")).First();
            File.Copy(termdatFile, Path.Combine(path, termdatFile.Split('\\').Last()), true);
        }

        public static void TrataOperut(string path, DateTime data, DateTime revDate, List<Tuple<string, float>> custoCVU)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            string pastaRec = GetPastaRecente(revDate);
            if (pastaRec != "")
            {
                var operutFile = Directory.GetFiles(pastaRec).Where(x => Path.GetFileName(x).ToLower().Contains("operut.dat")).First();
                var operutDiarioFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("operut.dat")).First();

                var operutRef = DocumentFactory.Create(operutFile) as Compass.CommomLibrary.Operut.Operut;
                var operutDiario = DocumentFactory.Create(operutDiarioFile) as Compass.CommomLibrary.Operut.Operut;
                operutDiario.BlocoInit.Clear();
                foreach (var line in operutRef.BlocoInit.ToList())
                {
                    operutDiario.BlocoInit.Add(line);
                }

                #region seta unidades geradoras

                // if (data == revDate.AddDays(-6))//só executa esse if se for o deck do primeiro dia da semana
                //  {
                var termdatFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("termdat.dat")).First();
                var termLines = File.ReadAllLines(termdatFile).Where(x => x.StartsWith("CADUNIDT")).ToList();

                var entdadosFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
                var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

                var blocoOper = operutDiario.BlocoOper;
                var blocoInit = operutDiario.BlocoInit;

                foreach (var c in custoCVU)
                {
                    int subNum = 0;
                    switch (c.Item1)
                    {
                        case "SE":
                            subNum = 1;
                            break;
                        case "S":
                            subNum = 2;
                            break;
                        case "NE":
                            subNum = 3;
                            break;
                        case "N":
                            subNum = 4;
                            break;
                    }
                    var blocoUt = entdados.BlocoUt.Where(x => x.SubSist == subNum).ToList();

                    if (blocoUt.Count() > 0)
                    {
                        foreach (var ut in blocoUt)
                        {
                            int numUsinaUT = ut.Usina;
                            var usinasOper = blocoOper.Where(x => x.Usina == numUsinaUT).ToList();
                            foreach (var us in usinasOper)
                            {
                                int unidade = us.Indice;
                                float custo = us.CustoGeracao;
                                if (custo > c.Item2)
                                {
                                    var init = operutDiario.BlocoInit.Where(x => x.Usina == numUsinaUT && x.Indice == unidade).FirstOrDefault();
                                    if (init != null)
                                    {
                                        init.Status = 0;
                                        init.Geracao = 0;
                                    }
                                }
                                else if (custo <= c.Item2)
                                {
                                    var init = operutDiario.BlocoInit.Where(x => x.Usina == numUsinaUT && x.Indice == unidade).FirstOrDefault();
                                    if (init != null)
                                    {

                                        var termL = termLines.Where(x => x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1] == numUsinaUT.ToString() &&
                                                                        x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2] == unidade.ToString()).FirstOrDefault();

                                        if (termL != null)
                                        {
                                            float potMin = float.Parse(termL.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[9].Replace('.', ','));
                                            init.Status = 1;
                                            init.Geracao = potMin;
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
                //}

                #endregion

                operutDiario.SaveToFile();
            }
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
                var mes = Tools.GetMonthNumAbrev(rev.revDate.Month);
                var cam = $@"H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
                //var cam = $@"N:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
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
        public static void TrataPtoper(string path, DateTime data)
        {
            string camArqsBase = "H:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base";
            //string camArqsBase = "N:\\Middle - Preço\\Resultados_Modelos\\DECODESS\\Arquivos_Base";

            var ptoperFile = Directory.GetFiles(camArqsBase).Where(x => Path.GetFileName(x).ToLower().Contains("ptoper")).FirstOrDefault();

            if (ptoperFile != null)
            {
                File.Copy(ptoperFile, Path.Combine(path, ptoperFile.Split('\\').Last()), true);

            }

            ptoperFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("ptoper")).FirstOrDefault();
            if (ptoperFile != null)
            {
                var ptoper = DocumentFactory.Create(ptoperFile) as Compass.CommomLibrary.PtoperDat.PtoperDat;

                foreach (var line in ptoper.BlocoPtoper.ToList())
                {
                    line.DiaIni = "I";
                }
                ptoper.SaveToFile();
            }

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
                newL.Reserva = GetRespotValor(tm.DiaInicial, tm.HoraDiaInicial, tm.MeiaHora, blocoDp);
                respot.BlocoLm.Add(newL);
            }
            respot.SaveToFile(createBackup: true);
        }
        public static float GetRespotValor(string dia, int hora, int meia, Compass.CommomLibrary.EntdadosDat.DpBlock blocoDp)
        {
            float valor = 0;

            //var linhasDp = blocoDp.Where(x => x.DiaInic == dia && x.HoraInic == hora && x.MeiaHoraInic == meia).Select(x => x.Demanda).Sum();
            var linhasDpSeSul = blocoDp.Where(x => x.DiaInic == dia && x.HoraInic == hora && x.MeiaHoraInic == meia && (x.Subsist == 1 || x.Subsist == 2)).Select(x => x.Demanda).Sum();
            valor = linhasDpSeSul * 0.05f;
            return valor;
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
        public static void CriarEntdados(string path, int incremento, DateTime dataBase, DateTime dataEstudo, bool expandEst, DateTime fimrev)
        {
            var entdadosFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;


            #region BLOCO TM
            bool patamres2023 = dataEstudo.Year == 2023;
            bool patamares2024 = dataEstudo.Year == 2024;
            bool patamares2025 = dataEstudo.Year >= 2025;

            var intervalos = Tools.GetIntervalosHoararios(dataEstudo, patamres2023, patamares2024, patamares2025);
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

        public static string GetNPTXT(DateTime d, bool recursivo = false)
        {
            var oneDrive_DESSEM = Path.Combine(@"C:\Enercore\Energy Core Trading\Energy Core Pricing - Documents\Arquivos_DESSEM");
            var kPath = @"K:\5_dessem\Arquivos_DESSEM";

            string arqName = $"NP{d:ddMMyyyy}.txt";

            if (!Directory.Exists(oneDrive_DESSEM))
            {
                oneDrive_DESSEM = oneDrive_DESSEM.Replace("Energy Core Pricing - Documents", "Energy Core Pricing - Documentos");
            }

            var oneDrive_DIA = Path.Combine(oneDrive_DESSEM, d.ToString("MM_yyyy"), d.ToString("dd"));
            var k_DIA = Path.Combine(kPath, d.ToString("MM_yyyy"), d.ToString("dd"));

            if (File.Exists(Path.Combine(k_DIA, arqName)))
            {
                return Path.Combine(k_DIA, arqName);
            }
            else if (File.Exists(Path.Combine(oneDrive_DIA, arqName)))
            {
                return Path.Combine(oneDrive_DIA, arqName);
            }
            else
            {
                //como pode estar criando um deck de um dia no futuro e tambem buscando dados de um NP do futuro, será usado o dados mais recente e caso não seja essa situação, uma exceção sera lançada interrompendo o processo
                DateTime hoje = DateTime.Today;
                if (d >= hoje || recursivo == true)
                {
                    string NP_recente = GetNPTXT(d.AddDays(-1), true);// chamada recursiva até encontrar o mais recente
                    return NP_recente;
                }

                throw new NotImplementedException($"Arquivo Níveis de partida não encontrados para criação do Deflant.dat, arquivo {arqName} necessário");

            }

            //return "";
        }

        public static float GetNPValue(string NPtxt, string montante)
        {
            float valor = 0f;
            var linhas = File.ReadAllLines(NPtxt).ToList();

            foreach (var l in linhas)
            {
                float d = 0f;
                var campos = l.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (campos[0].Trim() == montante)
                {
                    valor = float.TryParse(campos[2].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out d) ? d : 0;
                    return valor;
                }
            }

            return valor;
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
                        arqNP = GetNPTXT(dataAnt);
                        if (arqNP != "")
                        {
                            valor = GetNPValue(arqNP, tv.Montante.ToString());
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

            arqNP = GetNPTXT(dataEstudo.AddHours(-tv83), true);
            float valor83 = GetNPValue(arqNP, "83");

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
        public static void CriarDadvaz(string path, DateTime data)
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

            foreach (var vaz in vazoes)
            {
                vaz.DiaInic = data.Day.ToString();
            }


            dadvaz.SaveToFile();
        }
        public static void ComentaDessemArq(string path)
        {
            var dessemArqFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("dessem.arq")).First();
            var dessemArq = DocumentFactory.Create(dessemArqFile) as Compass.CommomLibrary.DessemArq.DessemArq;

            var curvtviagFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("curvtviag.dat")).First();
            var curvtviagName = Path.GetFileName(curvtviagFile);

            var cotasr11File = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("cotasr11.dat")).First();
            var cotasr11Name = Path.GetFileName(cotasr11File);

            var termdatFile = Directory.GetFiles(path).Where(x => Path.GetFileName(x).ToLower().Contains("termdat.dat")).First();
            var termdatName = Path.GetFileName(termdatFile);


            foreach (var line in dessemArq.BlocoArq.ToList())
            {
                if (line.Minemonico.Contains("INDELET"))
                {
                    line.Minemonico = "&" + line.Minemonico;
                }
                if (line.Minemonico.Contains("CURVT"))
                {
                    line.Minemonico = "CURVTVIAG";
                    line.NomeArq = curvtviagName;
                }
                if (line.Minemonico.Contains("COTASR"))
                {
                    line.Minemonico = "COTASR11";
                    line.NomeArq = cotasr11Name;
                }
                if (line.Minemonico.Contains("CADTERM"))
                {
                    line.NomeArq = termdatName;
                }

            }
            foreach (var file in Directory.GetFiles(path).ToList())
            {
                var fileName = Path.GetFileName(file);
                var minusculo = fileName.ToLower();
                File.Move(Path.Combine(path, fileName), Path.Combine(path, minusculo));
                foreach (var line in dessemArq.BlocoArq.Where(x => x.NomeArq.ToLower().Trim() == minusculo).ToList())
                {
                    line.NomeArq = minusculo;
                }
            }



            dessemArq.SaveToFile();
        }
        public static void CopiaArqsDessem(string fonte, string dest, DateTime data, DateTime fimrev)
        {
            var arqsBase = Directory.GetFiles(fonte);
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            foreach (var arq in arqsBase)
            {//operuh_12122020
                var operuh = $"operuh_{data:ddMMyyyy}.DAT";
                var nameFile = Path.GetFileName(arq);

                if (nameFile == operuh)
                {
                    File.Copy(arq, Path.Combine(dest, "operuh.DAT"), true);

                }
                else if (!nameFile.ToLower().Contains("operuh"))
                {
                    File.Copy(arq, Path.Combine(dest, arq.Split('\\').Last()), true);
                }
            }

            var pastaref = GetPastaRecente(fimrev);
            var rampasFile = Directory.GetFiles(pastaref).Where(x => Path.GetFileName(x).ToLower().Contains("rampas.dat")).FirstOrDefault();
            if (rampasFile != null)
            {
                File.Copy(rampasFile, Path.Combine(dest, rampasFile.Split('\\').Last()), true);

            }

        }
        public static void ExecutaDecodess(string diretorio)
        {
            try
            {
                var arquivos = Directory.GetFiles(diretorio);
                var tempFolderCLONE = @"L:\shared\DESSEM\decodess_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                //var tempFolder = @"Z:\shared\DESSEM\decodess_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                //var tempFolder = @"X:\AWS\shared\DESSEM\decodess_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                var tempFolder = @"K:\shared\DESSEM\decodess_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
                Directory.CreateDirectory(tempFolder);

                foreach (var arq in arquivos)
                {
                    File.Copy(arq, Path.Combine(tempFolder, arq.Split('\\').Last()), true);
                }

                //if (Services.Linux.Run(tempFolder, @"/home/producao/PrevisaoPLD/cpas_ctl_common/scripts/decodess.sh", "decodess", true, true, "hide"))
                //if (AutorunDecodess(tempFolder, @"/home/compass/sacompass/previsaopld/cpas_ctl_common/scripts/decodess.sh"))
                //if (AutorunDecodess(tempFolder, @"/mnt/Fsx/AWS/enercore_ctl_common/scripts/decodess.sh"))
                if (AutorunDecodess(tempFolder, @"/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/decodess.sh"))
                {
                    int timeout = 0;
                    var tempArqs = Directory.GetFiles(tempFolder);

                    while (tempArqs.Count() < 35 && timeout < 360)
                    {
                        tempArqs = Directory.GetFiles(tempFolder);
                        timeout += 10;
                        Thread.Sleep(10000);
                        if (tempArqs.Count() >= 35)
                        {
                            timeout = 370;
                        }
                    }
                    //if (timeout >= 360)
                    //{
                    //    return;
                    //}
                    Thread.Sleep(30000);

                    foreach (var temps in tempArqs)
                    {
                        File.Copy(temps, Path.Combine(diretorio, temps.Split('\\').Last()), true);
                    }
                    if (Directory.Exists(tempFolder))
                        Directory.Delete(tempFolder, true);

                    if (Directory.Exists(tempFolderCLONE))
                        Directory.Delete(tempFolderCLONE, true);
                }
            }
            catch (Exception e)
            {

            }

        }

        public static void TrataDecodessArq(string decodessFile, string cloneDir)
        {
            var decodess = DocumentFactory.Create(decodessFile) as Compass.CommomLibrary.DecodessArq.DecodessArq;
            var arqsClonedir = Directory.GetFiles(cloneDir);
            var decoLines = decodess.BlocoArqs.ToList();

            foreach (var arq in arqsClonedir.Where(x => !x.ToLower().Contains(".bak")))
            {
                var fileName = Path.GetFileNameWithoutExtension(arq);
                var fileNameEx = Path.GetFileName(arq);

                foreach (var line in decoLines)
                {
                    if (line.NomeArq.ToLower().Contains(fileName.ToLower()))
                    {
                        line.NomeArq = fileNameEx;
                    }
                }
            }

            decodess.SaveToFile();

        }

        public static List<Tuple<string, float>> GetCVU(string dir)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var sumario = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("sumario")).FirstOrDefault();
            List<Tuple<string, float>> CVU = new List<Tuple<string, float>>();

            if (sumario != null)
            {
                var textoSum = File.ReadAllLines(sumario).ToList();
                int inicio = textoSum.IndexOf(textoSum.Where(x => x.Contains("CUSTO MARGINAL DE OPERACAO")).First());
                int fim = textoSum.IndexOf(textoSum.Where(x => x.Contains("CUSTO DE OPERACAO")).First());
                List<string> submercados = new List<string> { "SE", "S", "NE", "N" };
                foreach (var sub in submercados)
                {
                    for (int i = inicio; i < fim; i++)
                    {
                        if (textoSum[i] != "")
                        {
                            string mediaSub = textoSum[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                            if (mediaSub.Equals("Med_" + sub))
                            {
                                float valor = float.Parse(textoSum[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1].Replace('.', ','));
                                CVU.Add(new Tuple<string, float>(sub, valor));
                                break;
                            }
                        }
                    }
                }
                return CVU;
            }
            return CVU;
        }

        public static void TrataBlocoRHEDessem(string daderFile)
        {
            //var linhas = File.ReadAllLines(daderFile, Encoding.UTF8).ToList();
            var linhas = File.ReadAllLines(daderFile).ToList();
            var novotexto = new List<string>();
            var linhaBloco = new List<string> { "RE", "LU", "FU", "FT", "FI", "FE" };

            foreach (var linha in linhas)
            {
                var dados = linha.Split(' ').ToList();
                if (linhaBloco.Any(x => x.Equals(dados[0])))
                {
                    var texto = linha;
                    texto = texto.Remove(2, 1);
                    texto = texto.Insert(7, " ");
                    novotexto.Add(texto);
                }
                else
                {
                    novotexto.Add(linha);
                }
            }
            //File.WriteAllLines(daderFile, novotexto, Encoding.UTF8);
            File.WriteAllLines(daderFile, novotexto);
        }
        public static void TrataConfig(string configFile, DateTime dataEstudo)
        {
            Dictionary<int, Tuple<int, int>> tipoDias = new Dictionary<int, Tuple<int, int>>() {//<mes,<Tipo dia util, Tipo feriado>>
                    {1, new Tuple<int,int>(3,4)},//jan
                    {2, new Tuple<int,int>(3,4)},//fev
                    {3, new Tuple<int,int>(3,4)},//marc
                    {4, new Tuple<int,int>(1,2)},//abril
                    {5, new Tuple<int,int>(5,6)},//maio
                    {6, new Tuple<int,int>(5,6)},//jun
                    {7, new Tuple<int,int>(5,6)},//jul
                    {8, new Tuple<int,int>(5,6)},//ago
                    {9, new Tuple<int,int>(1,2)},//set
                    {10, new Tuple<int,int>(1,2)},//out
                    {11, new Tuple<int,int>(3,4)},//nov
                    {12, new Tuple<int,int>(3,4)},//dez

                };



            var feriados = Tools.feriados;
            var config = DocumentFactory.Create(configFile) as Compass.CommomLibrary.ConfigDat.ConfigDat;

            var dataLine = config.BlocoData.First();
            dataLine.Dia = dataEstudo.Day;
            dataLine.Mes = dataEstudo.Month;
            dataLine.Ano = dataEstudo.Year;


            string diaAbrev = "";
            for (DateTime d = dataEstudo; d <= dataEstudo.AddDays(6); d = d.AddDays(1))
            {
                switch (d.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        diaAbrev = "SAB";
                        break;
                    case DayOfWeek.Sunday:
                        diaAbrev = "DOM";
                        break;
                    case DayOfWeek.Monday:
                        diaAbrev = "SEG";
                        break;
                    case DayOfWeek.Tuesday:
                        diaAbrev = "TER";
                        break;
                    case DayOfWeek.Wednesday:
                        diaAbrev = "QUA";
                        break;
                    case DayOfWeek.Thursday:
                        diaAbrev = "QUI";
                        break;
                    case DayOfWeek.Friday:
                        diaAbrev = "SEX";
                        break;
                    default:
                        diaAbrev = "";
                        break;

                }

                Boolean ehFeriado = false;
                int tipo = 0;
                var diaLine = config.BlocoDia.Where(x => x.Minemonico == diaAbrev).First();
                if (feriados.Any(x => x.Date == d.Date))
                {
                    ehFeriado = true;
                }
                if (ehFeriado || d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                {
                    tipo = tipoDias[d.Month].Item2;
                }
                else
                {
                    tipo = tipoDias[d.Month].Item1;
                }
                diaLine.TipoDia = tipo;
            }







            config.SaveToFile();
        }

        static void runDessem(string commands)
        {

            var command = commands.Split('|');


            var path = command[0];
            var cloneDir = "";

            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var dirInfo = new DirectoryInfo(dir);
                var parentDir = dirInfo.Parent.FullName;
                var dirName = dirInfo.Name + "_autoRun";

                var i = 0;
                do
                {
                    cloneDir = Path.Combine(parentDir, dirName + " (" + ++i + ")");
                } while (Directory.Exists(cloneDir));

                if (!Directory.Exists(cloneDir))
                {
                    Directory.CreateDirectory(cloneDir);
                }
                foreach (var arq in Directory.GetFiles(path).ToList())
                {
                    File.Copy(arq, Path.Combine(cloneDir, arq.Split('\\').Last()));
                }

                //string comandoDS = "/home/compass/sacompass/previsaopld/cpas_ctl_common/scripts/dessem191412.sh";
                //string comandoDS = "/mnt/Fsx/AWS/enercore_ctl_common/scripts/dessem190243.sh";
                // string comandoDS = "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/dessem190243.sh";
                string comandoDS = "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/dessem.sh";

                bool status = DessemAutorun(cloneDir, comandoDS);
                if (status)
                {
                    if (command.Count() > 1 && command[1] == "true")
                    {
                        var texto = "Deck DESSEM CCEE agendado para execução!";
                        Program.AutoClosingMessageBox.Show(texto, "Caption", 10000);

                        Compass.CommomLibrary.Tools.SendMail(texto, "bruno.araujo@enercore.com.br; pedro.modesto@enercore.com.br; natalia.biondo@enercore.com.br; gabriella.radke@enercore.com.br;", "AUTORUN DESSEM CCEE");

                    }
                    else
                    {
                        var texto = "Deck DESSEM CCEE agendado para execução!";

                        Program.AutoClosingMessageBox.Show(texto, "Caption", 10000);

                    }
                }


            }
            catch (Exception ex)
            {
                if (command.Count() > 1 && command[1] == "true")
                {
                    var texto = "Erro: " + ex.ToString();

                    Compass.CommomLibrary.Tools.SendMail(texto, "bruno.araujo@enercore.com.br; pedro.modesto@enercore.com.br; natalia.biondo@enercore.com.br; gabriella.radke@enercore.com.br;", "Falha AUTORUN DESSEM CCEE");
                    if (Directory.Exists(cloneDir))
                    {
                        Directory.Delete(cloneDir, true);
                    }
                }
                else
                {
                    var texto = "Erro: " + ex.ToString();
                    Program.AutoClosingMessageBox.Show(texto, "Caption", 10000);
                    if (Directory.Exists(cloneDir))
                    {
                        Directory.Delete(cloneDir, true);
                    }
                }


            }

        }

        static void dessem2ccee(string commands)
        {
            //L:\6_decomp\03_Casos\2019_04\deck_newave_2019_04
            //"L:\\6_decomp\\03_Casos\\2019_05\\DEC_ONS_052019_RV1_VE"

            var command = commands.Split('|');

            //var data = command[0].Substring(command[0].Length - 7, 7).Split('_');
            var path = command[0];
            var cloneDir = "";

            try
            {
                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var dirInfo = new DirectoryInfo(dir);
                var parentDir = dirInfo.Parent.FullName;
                var dirName = dirInfo.Name + "_ccee";

                var i = 0;
                do
                {
                    cloneDir = Path.Combine(parentDir, dirName + " (" + ++i + ")");
                } while (Directory.Exists(cloneDir));



                var deck = DeckFactory.CreateDeck(dir);

                if (!(deck is Compass.CommomLibrary.Dessem.Deck))
                {
                    throw new NotImplementedException("Deck não reconhecido para a execução");
                }

                deck.CopyFilesToFolder(cloneDir);

                dynamic cceeDeck = DeckFactory.CreateDeck(cloneDir);

                Boolean status = ConverteDessem(cloneDir, command);

                if (status)
                {
                    string texto = "Sucesso ao converter deck dessem!";
                    if (command.Count() > 1 && command[1] == "true")
                    {
                        Program.AutoClosingMessageBox.Show(texto, "Caption", 5000);
                        string comandoDS = "/home/producao/PrevisaoPLD/enercore_ctl_common/scripts/dessem.sh";

                        bool sucesso = DessemAutorun(cloneDir, comandoDS);
                        if (sucesso)
                        {
                            string frase = "Deck convertido ONS->CCEE encaminhado para fila de execução. Caminho = " + cloneDir;
                            Compass.CommomLibrary.Tools.SendMail(frase, "bruno.araujo@enercore.com.br; pedro.modesto@enercore.com.br; natalia.biondo@enercore.com.br; gabriella.radke@enercore.com.br;", "AUTORUN DESSEM ONS->CCEE");
                        }
                        else
                        {
                            string info = "Conversão ONS->CCEE realizada sem direcionamento para fila de execução. Caminho = " + cloneDir;
                            Compass.CommomLibrary.Tools.SendMail(info, "bruno.araujo@enercore.com.br; pedro.modesto@enercore.com.br; natalia.biondo@enercore.com.br; gabriella.radke@enercore.com.br;", "Sucesso ao converter deckDessem");
                        }

                    }
                    else
                    {
                        MessageBox.Show(texto, "Caption");
                    }
                }
                else
                {
                    string texto = "Falha ao converter deck dessem, diretório ou arquivos decomp inexistentes";
                    Program.AutoClosingMessageBox.Show(texto, "Caption", 5000);
                    if (Directory.Exists(cloneDir))//deck convertido
                    {
                        Directory.Delete(cloneDir, true);
                    }

                    if (Directory.Exists(path))//deck original
                    {
                        Directory.Delete(path, true);
                    }

                    if (File.Exists(path + ".zip"))//zip original
                    {
                        File.Delete(path + ".zip");
                    }

                    if (command.Count() > 1 && command[1] == "true")
                    {

                        Compass.CommomLibrary.Tools.SendMail(texto, "bruno.araujo@enercore.com.br;", "Falha ao converter deckDessem");
                    }
                }


            }
            catch (Exception ex)
            {
                if (command.Count() > 1 && command[1] == "true")
                {
                    var texto = ex.ToString();
                    if (ex.ToString().Contains("reconhecido"))
                    {
                        texto = "Deck não reconhecido para a execução por falta de arquivos!";
                    }
                    Compass.CommomLibrary.Tools.SendMail(texto, "bruno.araujo@enercore.com.br;", "Falha ao converter deckDessem");
                    if (Directory.Exists(cloneDir))//deck convertido
                    {
                        Directory.Delete(cloneDir, true);
                    }

                    if (Directory.Exists(path))//deck original
                    {
                        Directory.Delete(path, true);
                    }

                    if (File.Exists(path + ".zip"))//zip original
                    {
                        File.Delete(path + ".zip");
                    }
                }
                else
                {
                    var texto = "Deck não reconhecido para a execução por falta de arquivos! Erro: " + ex.ToString();
                    if (ex.ToString().Contains("Processo Interrompido!!!convertdstoccee"))
                    {
                        texto = "Processo Interrompido!!!";
                        Program.AutoClosingMessageBox.Show(texto, "Caption", 10000);
                        if (Directory.Exists(cloneDir))//deck convertido
                        {
                            Directory.Delete(cloneDir, true);
                        }

                        if (Directory.Exists(path))//deck original
                        {
                            Directory.Delete(path, true);
                        }

                        if (File.Exists(path + ".zip"))//zip original
                        {
                            File.Delete(path + ".zip");
                        }
                    }
                    else if (ex.ToString().Contains("Processo Interrompido!!!Mapcut e Cortdeco do Decomp referência não encontrados"))
                    {
                        texto = "Processo Interrompido!!!Mapcut e Cortdeco do Decomp referência não encontrados";
                        MessageBox.Show(texto, "Caption");
                        if (Directory.Exists(cloneDir))// deck convertido
                        {
                            Directory.Delete(cloneDir, true);
                        }

                        if (Directory.Exists(path))//deck original
                        {
                            Directory.Delete(path, true);
                        }

                        if (File.Exists(path + ".zip"))//zip original
                        {
                            File.Delete(path + ".zip");
                        }
                    }
                    else
                    {
                        Program.AutoClosingMessageBox.Show(texto, "Caption", 10000);
                        if (Directory.Exists(cloneDir))//deck convertido
                        {
                            Directory.Delete(cloneDir, true);
                        }

                        if (Directory.Exists(path))//deck original
                        {
                            Directory.Delete(path, true);
                        }

                        if (File.Exists(path + ".zip"))//zip original
                        {
                            File.Delete(path + ".zip");
                        }
                    }

                }


            }

        }

        public static Boolean ConverteDessem(string dir, string[] command)
        {
            string diretorioBase = command[0];

            var deckestudo = DeckFactory.CreateDeck(dir) as Compass.CommomLibrary.Dessem.Deck;

            var dadvaz = deckestudo[CommomLibrary.Dessem.DeckDocument.dadvaz].Document.File;
            var dadlinhas = File.ReadAllLines(dadvaz).ToList();
            var dados = dadlinhas[9].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            DateTime dataEstudo = new DateTime(Convert.ToInt32(dados[3]), Convert.ToInt32(dados[2]), Convert.ToInt32(dados[1]));

            if (command.Count() == 1)
            {
                Thread thread = new Thread(dsOns2CceeSTA);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(dir);
                thread.Join();
            }

            Boolean continua = CopiaArqDecomp(dataEstudo, dir, command);

            string deckRefCCEE = string.Empty;

            if (command.Count() > 1 && command[1] == "true")
            {
                DateTime dat = dataEstudo;
                DateTime datVE = dataEstudo;

                if (dataEstudo.DayOfWeek == DayOfWeek.Friday)
                {
                    datVE = dat.AddDays(-1);
                }
                var rev = Tools.GetCurrRev(datVE);

                deckRefCCEE = Services.GeraDessem.GetPastaRecente(rev.revDate);
            }
            else
            {
                //Thread thread = new Thread(DirectoryPath);
                //thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                //thread.Start(dir);
                //thread.Join(); //Wait for the thread to end      
                if (!File.Exists(Path.Combine(dir, "CopiMapCort.log")))
                {
                    throw new NotImplementedException("Processo Interrompido!!!Mapcut e Cortdeco do Decomp referência não encontrados");

                }
                deckRefCCEE = File.ReadAllText(Path.Combine(dir, "dir.txt"));
                if (!Directory.Exists(deckRefCCEE))
                {
                    throw new NotImplementedException("Processo Interrompido!!!convertdstoccee");
                }
            }

            var deckCCEEref = DeckFactory.CreateDeck(deckRefCCEE);

            if (!(deckCCEEref is Compass.CommomLibrary.Dessem.Deck))
            {
                throw new NotImplementedException("Deck não reconhecido para a execução");
            }

            var deckCCEErefDS = DeckFactory.CreateDeck(deckRefCCEE) as Compass.CommomLibrary.Dessem.Deck;



            if (continua)
            {
                #region trata operut
                var operut = deckestudo[CommomLibrary.Dessem.DeckDocument.operut].Document as Compass.CommomLibrary.Operut.Operut;

                // var usinas = operut.BlocoInit.Where(x => x.Usina == 15).ToList();
                foreach (var usina in operut.BlocoInit.Where(x => x.Usina == 15).ToList())
                {
                    usina.Status = 0;
                    usina.Geracao = 0;
                }

                operut.SaveToFile(createBackup: true);
                #endregion

                #region trata ptoper
                var ptoper = deckestudo[CommomLibrary.Dessem.DeckDocument.ptoper].Document as Compass.CommomLibrary.PtoperDat.PtoperDat;

                //var usinas = ptoper.BlocoPtoper.Where(x => x.usina == 15).ToList();
                foreach (var usina in ptoper.BlocoPtoper.Where(x => x.usina == 15).ToList())
                {
                    usina.ValorFixado = 0;
                }

                ptoper.SaveToFile(createBackup: true);
                #endregion

                #region dessem.arq
                var dessemArq = deckestudo[CommomLibrary.Dessem.DeckDocument.dessem].Document.File;
                var lines = File.ReadAllLines(dessemArq).ToList();
                List<string> newTexto = new List<string>();

                //int indice = 0;
                foreach (var lin in lines)
                {
                    if (lin.StartsWith("INDELET") || lin.StartsWith("RMPFLX"))
                    {


                        string frase = lin;
                        frase = "&" + frase;
                        newTexto.Add(frase);
                    }
                    else
                    {
                        newTexto.Add(lin);
                    }
                }
                File.WriteAllLines(dessemArq, newTexto);

                //if (lines.Any(x => x.StartsWith("INDELET")))
                //{

                //    indice = lines.IndexOf(lines.Where(x => x.StartsWith("INDELET")).First());
                //    string frase = lines[indice];
                //    frase = "&" + frase;
                //    lines[indice] = frase;
                //    File.WriteAllLines(dessemArq, lines);
                //}
               

                #endregion

                #region renovaveis.dat

                var renovaveis = deckestudo[CommomLibrary.Dessem.DeckDocument.renovaveis].Document.File;
                var renoLines = File.ReadAllLines(renovaveis);
                List<string> modifReno = new List<string>();
                foreach (var item in renoLines)
                {
                    if (item.Split(';').First().Trim() == "EOLICA")
                    {
                        var textos = item.Split(';').ToList();

                        textos[textos.IndexOf(textos.Last()) - 1] = "0";
                        string linha = "";
                        foreach (var parte in textos)
                        {
                            linha += parte;
                            if (parte != textos.Last())
                            {
                                linha += ";";
                            }

                        }

                        modifReno.Add(linha);
                    }
                    else
                    {
                        modifReno.Add(item);
                    }
                }
                File.WriteAllLines(renovaveis, modifReno);

                #endregion

                #region trata entdados

                var entdados = deckestudo[CommomLibrary.Dessem.DeckDocument.entdados].Document as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
                var entdadosCCEEref = deckCCEErefDS[CommomLibrary.Dessem.DeckDocument.entdados].Document as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
                var entdadosCCEErefFile = deckCCEErefDS[CommomLibrary.Dessem.DeckDocument.entdados].Document.File;
                var entdadosFile = deckestudo[CommomLibrary.Dessem.DeckDocument.entdados].Document.File;

                foreach (var line in entdados.BlocoRd.ToList())
                {
                    line[0] = "&" + line[0];
                }
                foreach (var line in entdados.BlocoTm.ToList())
                {
                    line.Rede = 0;
                }
                foreach (var line in entdados.BlocoPq.ToList())
                {
                    line[0] = "&" + line[0];
                }


                foreach (var cdline in entdados.BlocoCd.ToList())
                {
                    cdline.DiaInic = " I";
                }



                ComentaCICE(entdados);
                //TrataIa(entdados, dataEstudo);

                TrataRhe(entdados, dataEstudo, entdadosCCEEref, entdadosCCEErefFile, entdadosFile);

                //TrataDP(entdados, dataEstudo);//foi uma demanda temporaria em que se somava um valor fixo na demanda durante a conversão, caso a demanda volte apenas descomente a chamada da função
                //entdados.SaveToFile(createBackup: true);
                if (dataEstudo.DayOfWeek == DayOfWeek.Friday)
                {
                    //TrataRheSexta(dataEstudo, dir);
                }
                // DescomentaRhe(dataEstudo, dir);



                RestsegRstlpp(dataEstudo, dir, deckRefCCEE);
                var entdadosNew = deckestudo[CommomLibrary.Dessem.DeckDocument.entdados].Document as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

                TrataMT(entdadosFile, diretorioBase, dataEstudo);

                #endregion

                #region trata operuh

                var operuhCCEErefFile = deckCCEErefDS[CommomLibrary.Dessem.DeckDocument.operuh].Document.File;
                var operuhFile = deckestudo[CommomLibrary.Dessem.DeckDocument.operuh].Document.File;

                TrataOperuh(operuhCCEErefFile, operuhFile);


                #endregion

                return continua;
            }
            else
            {
                return continua;
            }



        }

        public static Boolean CopiaArqDecomp(DateTime dataEstudo, string dirTosave, string[] command)
        {
            Boolean Ok = false;
            int contArq = 0;


            DateTime Ve;
            if (dataEstudo.DayOfWeek == DayOfWeek.Friday)
            {
                Ve = dataEstudo.AddDays(-1);
            }
            else
            {
                Ve = dataEstudo;
            }

            var rev = Tools.GetCurrRev(Ve);
            string mapcut = "mapcut.rv" + rev.rev.ToString();
            string cortdeco = "cortdeco.rv" + rev.rev.ToString();

            if (command.Count() > 1 && command[1] == "true")
            {
                //for (int i = 1; i <= 10; i++)
                // {
                //string camDecomp = @"X:\AWS\4_curto_prazo\" + rev.revDate.ToString("yyyy_MM") + "\\DEC_ONS_" + rev.revDate.ToString("MMyyyy") + "_RV" + rev.rev.ToString() + $"_VE_ccee ({i})";
                //string camDecomp = @"K:\4_curto_prazo\" + rev.revDate.ToString("yyyy_MM") + "\\DEC_ONS_" + rev.revDate.ToString("MMyyyy") + "_RV" + rev.rev.ToString() + $"_VE_ccee ({i})";
                string camDecomp = Tools.GetDCref(dataEstudo);
                string etcFile = Path.Combine(camDecomp, "etc.zip");
                if (Directory.Exists(camDecomp))
                {
                    var arqs = Directory.GetFiles(camDecomp).ToList();
                    if (arqs.All(x => Path.GetFileName(x).ToLower() != mapcut) && arqs.All(x => Path.GetFileName(x).ToLower() != cortdeco))
                    {
                        Ionic.Zip.ZipFile arquivoZip = Ionic.Zip.ZipFile.Read(etcFile);
                        try
                        {
                            foreach (ZipEntry e in arquivoZip)
                            {
                                e.Extract(camDecomp, ExtractExistingFileAction.OverwriteSilently);
                            }
                            arquivoZip.Dispose();
                            arqs = Directory.GetFiles(camDecomp).ToList();

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    foreach (var arq in arqs)
                    {
                        var filename = Path.GetFileName(arq);
                        if ((filename.ToLower() == mapcut) || (filename.ToLower() == cortdeco))
                        {
                            File.Copy(arq, Path.Combine(dirTosave, filename), true);
                            contArq++;
                        }
                    }
                    if (contArq == 2)
                    {
                        Ok = true;
                        return Ok;
                    }
                }
                //}
            }
            else
            {

                //var texto = "Defina um diretório Decomp para copiar MAPCUT e CORTDECO";
                //MessageBox.Show(texto, "ATENCÃO!");

                Thread thread = new Thread(MapcutCortedeco);
                //thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                // thread.Start(dirTosave);
                //thread.Join(); //Wait for the thread to end      

                var arqMapcut = Directory.GetFiles(dirTosave).Where(x => Path.GetFileName(x).ToLower() == (mapcut)).FirstOrDefault();
                var arqCortdeco = Directory.GetFiles(dirTosave).Where(x => Path.GetFileName(x).ToLower() == (cortdeco)).FirstOrDefault();

                if (File.Exists(arqMapcut) && File.Exists(arqCortdeco))
                {
                    Ok = true;
                    return Ok;
                }

            }

            return Ok;

        }



        public static void RestsegRstlpp(DateTime dataEstudo, string dir, string folderCCEEref)
        {
            //H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\2020\12_dez\RV2\DS_CCEE_122020_SEMREDE_RV2D18
            var novoRestseg = new List<string>();
            var novoRstlpp = new List<string>();

            DateTime dat = dataEstudo;
            DateTime datVE = dataEstudo;

            //if (dataEstudo.DayOfWeek == DayOfWeek.Friday)
            //{
            //    datVE = dat.AddDays(-1);
            //}
            //var rev = Tools.GetCurrRev(datVE);

            //var camRef = Services.GeraDessem.GetPastaRecente(rev.revDate);

            var dtAtual = DateTime.Today.AddDays(1);
            var datalimite = DateTime.Today.AddDays(-180);
            string restsegRef = null;
            string rstlppRef = null;

            string restseg = null;
            string rstlpp = null;



            if (Directory.Exists(folderCCEEref))
            {
                string restONSbak = "restsegONS.bak";
                string rstlppONSbak = "rstlppONS.bak";

                restseg = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("restseg")).FirstOrDefault();
                rstlpp = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("rstlpp")).FirstOrDefault();
                if (restseg != null && rstlpp != null)
                {
                    File.Move(restseg, Path.Combine(dir, restONSbak));
                    File.Move(rstlpp, Path.Combine(dir, rstlppONSbak));
                }

                restsegRef = Directory.GetFiles(folderCCEEref).Where(x => Path.GetFileName(x).ToLower().Contains("restseg")).FirstOrDefault();
                rstlppRef = Directory.GetFiles(folderCCEEref).Where(x => Path.GetFileName(x).ToLower().Contains("rstlpp")).FirstOrDefault();

                File.Copy(restsegRef, Path.Combine(dir, restsegRef.Split('\\').Last()), true);
                File.Copy(rstlppRef, Path.Combine(dir, rstlppRef.Split('\\').Last()), true);

            }

            #region codigo antigo
            //if (Directory.Exists(camRef))
            //{
            //    restsegRef = Directory.GetFiles(camRef).Where(x => Path.GetFileName(x).ToLower().Contains("restseg")).FirstOrDefault();
            //    rstlppRef = Directory.GetFiles(camRef).Where(x => Path.GetFileName(x).ToLower().Contains("rstlpp")).FirstOrDefault();

            //}

            //var restseg = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("restseg")).FirstOrDefault();
            //var rstlpp = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("rstlpp")).FirstOrDefault();

            //if (restseg == null)
            //{
            //    File.Copy(restsegRef, Path.Combine(dir, restsegRef.Split('\\').Last()), true);
            //}
            //if (rstlpp == null)
            //{
            //    File.Copy(rstlppRef, Path.Combine(dir, rstlppRef.Split('\\').Last()), true);
            //}

            //restseg = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("restseg")).FirstOrDefault();
            //rstlpp = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("rstlpp")).FirstOrDefault();

            //#region rstlpp
            ////var rstlppLines = File.ReadAllLines(rstlpp, Encoding.GetEncoding("iso-8859-1")).ToList();
            //var rstlppLines = File.ReadAllLines(rstlpp).ToList();
            //// var rstlppRefLines = File.ReadAllLines(rstlppRef, Encoding.GetEncoding("iso-8859-1")).ToList();
            //var rstlppRefLines = File.ReadAllLines(rstlppRef).ToList();

            //foreach (var line in rstlppLines)
            //{
            //    var nline = line;
            //    if (line.StartsWith("&"))
            //    {
            //        nline = line.Substring(1);

            //        //novoRstlpp.Add(line);
            //    }
            //    // else
            //    // {
            //    var minemonico = nline.Split(' ').First();
            //    var texto = "";
            //    switch (minemonico)
            //    {
            //        case "RSTSEG":
            //        case "ADICRS":
            //            texto = nline.Substring(0, 19);
            //            break;
            //        case "PARAM":
            //            texto = nline.Substring(0, 10);
            //            break;
            //        case "RESLPP":
            //            texto = nline.Substring(0, 15);
            //            break;
            //        case "VPARM":
            //            texto = nline.Substring(0, 19);
            //            break;
            //        default:
            //            texto = nline;
            //            break;
            //    }
            //    if (texto != "")
            //    {
            //        var linha = rstlppRefLines.Where(x => x.Contains(texto)).FirstOrDefault();
            //        if (linha != null)
            //        {
            //            if (linha.StartsWith("&"))
            //            {
            //                novoRstlpp.Add("&" + nline);
            //            }
            //            else
            //            {
            //                novoRstlpp.Add(nline);
            //            }
            //        }
            //        else
            //        {
            //            novoRstlpp.Add(line);
            //        }
            //    }
            //    else
            //    {
            //        novoRstlpp.Add(line);
            //    }
            //    //// }
            //}
            //// File.WriteAllLines(rstlpp, novoRstlpp, Encoding.GetEncoding("iso-8859-1"));
            //File.WriteAllLines(rstlpp, novoRstlpp);
            //#endregion

            //#region restseg

            //// var restsegLines = File.ReadAllLines(restseg, Encoding.GetEncoding("iso-8859-1")).ToList();
            //var restsegLines = File.ReadAllLines(restseg).ToList();
            ////var restsegRefLines = File.ReadAllLines(restsegRef, Encoding.GetEncoding("iso-8859-1")).ToList();
            //var restsegRefLines = File.ReadAllLines(restsegRef).ToList();



            //List<Tuple<string, bool>> restcoment = new List<Tuple<string, bool>>();

            //foreach (var refl in restsegRefLines)
            //{
            //    var partes = refl.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            //    bool comentar = false;
            //    int i;
            //    if (partes.Count() >= 3)//filtro para pegar as linhas que possuem numero de restrição
            //    {
            //        if (int.TryParse(partes[2], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out i))
            //        {
            //            if (refl.StartsWith("&"))
            //            {
            //                comentar = true;
            //            }
            //            if (restcoment.Count() == 0 || restcoment.All(x => !x.Item1.Equals(partes[2])))
            //            {
            //                restcoment.Add(new Tuple<string, bool>(partes[2], comentar));//num restrição, comentar?
            //            }
            //        }

            //    }
            //}

            //foreach (var restL in restsegLines)
            //{
            //    var partRes = restL.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //    if (partRes.Count() >= 3)
            //    {
            //        var restricao = restcoment.Where(x => x.Item1.Equals(partRes[2])).FirstOrDefault();
            //        if (restricao != null)
            //        {
            //            if (restricao.Item2 == true)
            //            {
            //                if (restL.StartsWith("&"))
            //                {
            //                    novoRestseg.Add(restL);
            //                }
            //                else
            //                {
            //                    novoRestseg.Add("&" + restL);
            //                }
            //            }
            //            else
            //            {
            //                if (restL.StartsWith("&"))
            //                {
            //                    novoRestseg.Add(restL.Substring(1));
            //                }
            //                else
            //                {
            //                    novoRestseg.Add(restL);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            novoRestseg.Add(restL);
            //        }
            //    }
            //    else
            //    {
            //        novoRestseg.Add(restL);

            //    }
            //}

            ////File.WriteAllLines(restseg, novoRestseg, Encoding.GetEncoding("iso-8859-1"));
            //File.WriteAllLines(restseg, novoRestseg);

            //#endregion

            #endregion


        }


        public static void TrataDP(Compass.CommomLibrary.EntdadosDat.EntdadosDat entdados, DateTime dataEstudo)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            List<Tuple<int, int, int, float>> dadosDP = new List<Tuple<int, int, int, float>>();//subsist,hora,meiahora,valor
            var linhasDPFixo = File.ReadAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\CargaDP_ONS-CCEE.txt").ToList();
            foreach (var linha in linhasDPFixo)
            {
                var dados = linha.Split('\t').ToList();
                dadosDP.Add(new Tuple<int, int, int, float>(Convert.ToInt32(dados[0]), Convert.ToInt32(dados[1]), Convert.ToInt32(dados[2]), float.Parse(dados[3].Replace('.', ','))));
            }
            string dia = dataEstudo.Day.ToString();
            var entdadosDPs = entdados.BlocoDp.Where(x => x.DiaInic.Trim() == dia).ToList();
            for (int subS = 1; subS <= 4; subS++)
            {
                foreach (var ent in entdadosDPs.Where(x => x.Subsist == subS).ToList())
                {
                    float valorAdic = dadosDP.Where(x => x.Item1 == subS && x.Item2 == ent.HoraInic && x.Item3 == ent.MeiaHoraInic).Select(x => x.Item4).FirstOrDefault();
                    if (valorAdic != 0)
                    {
                        ent.Demanda = ent.Demanda + valorAdic;
                    }
                }

            }
        }

        public static void TrataRheSexta(DateTime dataEstudo, string dir)
        {
            //exceção para decks referentes às sextas feiras


            List<int> restSexta = new List<int> { 901, 902, 906, 907, 908, 909, 910, 911, 912, 917, 918, 931, 924, 927, 928, 934, 935,
                    936, 939, 952, 947, 938, 940, 941, 942, 950, 949,
                    943, 944, 945, 946, 957, 958, 959, 960, 961, 962, 974, 982, 981, 986, 991
                };

            List<string> newTexto = new List<string>();

            var entdadosFile = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Equals("entdados.dat")).First();
            var entLinhas = File.ReadAllLines(entdadosFile).ToList();
            var blocosId = "RE LU FH FT FI FE FR FC".Split(' ').ToList();
            foreach (var linha in entLinhas)
            {
                string texto = linha;
                //retirando comentarios "&" das restrições
                if (linha.StartsWith("&") && linha != "")
                {
                    var indice = entLinhas.IndexOf(linha);
                    var l = linha.Substring(1);
                    var cod = (l + "  ").Split(' ').First();
                    if (blocosId.Any(k => k.Equals(cod)))
                    {
                        var newBlock = new Compass.CommomLibrary.EntdadosDat.RheBlock();
                        var newL = newBlock.CreateLine(l);
                        if (restSexta.Any(x => x == newL.Restricao))
                        {
                            texto = l;
                        }
                    }
                }
                newTexto.Add(texto);
            }
            File.WriteAllLines(entdadosFile, newTexto);
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
            foreach (var item in restSexta)
            {
                foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] == item))
                {
                    foreach (var rh in rhe.Value)
                    {
                        rh[2] = " I";
                    }
                }
            }
            entdados.SaveToFile();
        }

        public static void DescomentaRhe(DateTime dataEstudo, string dir)
        {
            //exceção para decks referentes às sextas feiras


            List<int> rest = new List<int> { 913, 915 };

            List<string> newTexto = new List<string>();

            var entdadosFile = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Equals("entdados.dat")).First();
            var entLinhas = File.ReadAllLines(entdadosFile).ToList();
            var blocosId = "RE LU FH FT FI FE FR FC".Split(' ').ToList();
            foreach (var linha in entLinhas)
            {
                string texto = linha;
                //retirando comentarios "&" das restrições
                if (linha.StartsWith("&") && linha != "")
                {
                    var indice = entLinhas.IndexOf(linha);
                    var l = linha.Substring(1);
                    var cod = (l + "  ").Split(' ').First();
                    if (blocosId.Any(k => k.Equals(cod)))
                    {
                        var newBlock = new Compass.CommomLibrary.EntdadosDat.RheBlock();
                        var newL = newBlock.CreateLine(l);
                        if (rest.Any(x => x == newL.Restricao))
                        {
                            texto = l;
                        }
                    }
                }
                newTexto.Add(texto);
            }
            File.WriteAllLines(entdadosFile, newTexto);
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
            foreach (var item in rest)
            {
                foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] == item))
                {
                    foreach (var rh in rhe.Value)
                    {
                        rh[2] = " I";
                    }
                }
            }
            entdados.SaveToFile();
        }

        public static void TrataMT(string entdadosFile, string diretorioBase, DateTime dataEstudo)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
            //
            List<string> UsiLInes = new List<string>();
            List<string> UsiLInesERRO = new List<string>();
            var patsFiles = System.IO.Directory.GetFiles(diretorioBase, "pat*", SearchOption.AllDirectories).ToList();
            if (patsFiles.Count() > 0)
            {
                foreach (var pat in patsFiles)
                {
                    var linhas = File.ReadAllLines(pat).ToList();
                    int start = linhas.IndexOf(linhas.Where(x => x.StartsWith("( *** USINAS TERMELETRICAS *** )")).First()) + 1;
                    int end = start;
                    while (!linhas[end].StartsWith("( *** ") && !linhas[end].StartsWith("FIM"))
                    {
                        end++;
                    }
                    linhas.Where(x => x.StartsWith("( Usi:")).ToList().ForEach(x =>
                    {
                        int n;
                        n = linhas.IndexOf(x);
                        string lComment = "&" + x;
                        if (n > start && n < end && UsiLInes.All(y => y != lComment))
                        {
                            UsiLInes.Add("&" + x);
                        }
                    }
                    );
                }
                foreach (var usil in UsiLInes)
                {
                    try
                    {
                        //&( Usi: ANGRA 2 - Qtd. Orig:1 - Gerador:RJUSAN0UG2 - SGI:202300053695 - Ini:29/09/2023 08:00 - Fim:14/10/2023 23:59
                        var usiName = usil.Split(new string[] { "Usi:" }, StringSplitOptions.RemoveEmptyEntries).Last().Split(new string[] { " -" }, StringSplitOptions.RemoveEmptyEntries).First().Replace("230", "").Trim();//230 é tratamento pra L.LACERDA-A 230
                        int usiNum = entdados.BlocoUt.Where(x => x.NomeUsina.Trim().ToUpper() == usiName.Trim().ToUpper()).Select(x => x.Usina).First();
                        int unidGer = Convert.ToInt32(usil.Split(new string[] { "Qtd. Orig:" }, StringSplitOptions.RemoveEmptyEntries).Last().Split('-').First().Trim());
                        DateTime dataIni = Convert.ToDateTime(usil.Split(new string[] { "Ini:" }, StringSplitOptions.RemoveEmptyEntries).Last().Split('-').First().Trim(), Culture.DateTimeFormat);
                        DateTime dataFim = Convert.ToDateTime(usil.Split(new string[] { "Fim:" }, StringSplitOptions.RemoveEmptyEntries).Last().Split('-').First().Trim(), Culture.DateTimeFormat);

                        if (dataIni.Minute == 29 || dataIni.Minute == 59)// trata meias horas
                        {
                            dataIni = dataIni.AddMinutes(1);
                        }
                        if (dataFim.Minute == 29 || dataFim.Minute == 59)// trata meias horas
                        {
                            dataFim = dataFim.AddMinutes(1);
                        }

                        if (dataFim > dataEstudo)
                        {
                            var mtl = entdados.BlocoMt.CreateLine();
                            mtl.Comment = usil;
                            mtl.IdBloco = "MT";
                            mtl.Usina = usiNum;
                            mtl.UnidadeGeradora = unidGer;
                            mtl.DispUnidade = 0;

                            mtl.DiaInic = dataIni < dataEstudo ? dataEstudo.Day.ToString("00") : dataIni.Day.ToString("00");
                            mtl.HoraInic = dataIni < dataEstudo ? dataEstudo.Hour : dataIni.Hour;
                            mtl.MeiaHoraInic = dataIni < dataEstudo ? 0 : dataIni.Minute < 29 ? 0 : dataIni.Minute < 59 ? 1 : 0;

                            mtl.DiaFinal = dataFim.Day.ToString("00");
                            mtl.HoraFinal = dataFim.Hour;
                            mtl.MeiaHoraFinal = dataFim.Minute < 29 ? 0 : dataFim.Minute < 59 ? 1 : 0;
                            entdados.BlocoMt.Add(mtl);

                            // UsiLInes.Remove(usil);
                        }
                    }
                    catch (Exception e)
                    {
                        UsiLInesERRO.Add(usil);
                        e.ToString();

                    }


                }
                entdados.SaveToFile();

                var entlines = File.ReadAllLines(entdadosFile).ToList();
                entlines.Add("&BLOCO-MT USINAS NAO ENCONTRATADAS DE ACORDO COM BLOCO UT");
                UsiLInesERRO.ForEach(x =>
                {
                    entlines.Add(x);
                });
                File.WriteAllLines(entdadosFile, entlines);
            }
        }

        public static void TrataOperuh(string operuhRefFile, string operuhFile)
        {
            List<string> restComment = new List<string>();
            List<string> restUnComment = new List<string>();

            List<string> arquivoFinal = new List<string>();
            //var dados = dadlinhas[9].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            //
            var refLinhas = File.ReadAllLines(operuhRefFile).ToList();
            var linhas = File.ReadAllLines(operuhFile).ToList();

            refLinhas.Where(x => x.StartsWith("&OPERUH")).ToList().ForEach(x =>
            {
                string num = x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2];
                restComment.Add(num);
            });

            refLinhas.Where(x => x.StartsWith("OPERUH")).ToList().ForEach(x =>
            {
                string num = x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2];
                restUnComment.Add(num);
            });

            foreach (var lin in linhas)
            {
                string newline = lin;
                if (newline.StartsWith("&OPERUH"))
                {
                    string numR = newline.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2];
                    if (restUnComment.Any(x => x == numR))
                    {
                        arquivoFinal.Add(newline.Substring(1));
                        continue;
                    }

                }
                else if (newline.StartsWith("OPERUH"))
                {
                    string numR = newline.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2];
                    if (restComment.Any(x => x == numR))
                    {
                        arquivoFinal.Add("&" + newline);
                        continue;
                    }
                }


                arquivoFinal.Add(lin);

            }

            File.WriteAllLines(operuhFile, arquivoFinal);
            // restComment = refLinhas.Where(x => x.StartsWith("&OPERUH")).Select(x => x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2])

        }
        public static void TrataRhe(Compass.CommomLibrary.EntdadosDat.EntdadosDat entdados, DateTime dataEstudo, Compass.CommomLibrary.EntdadosDat.EntdadosDat entdadosRef, string fileEntdadosRef, string entdadosFile)
        {
            #region codigo antigo
            ////914
            //List<int> restComent = new List<int> { 141, 142, 143, 144, 145, 146, 147, 272, 654, 800, 801, 802, 803, 804, 805, 827, 828, 840, 844, 846, 847, 845, 854, 904, 919, 920, 921, 922, 923, 937, 948, 984, 985, 990 };
            //foreach (var rest in restComent)
            //{
            //    foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] == rest))
            //    {
            //        foreach (var rh in rhe.Value)
            //        {
            //            rh[0] = "&" + rh[0];
            //        }
            //    }
            //}
            //for (int i = 602; i <= 649; i++)
            //{
            //    foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] == i))
            //    {
            //        foreach (var rh in rhe.Value)
            //        {
            //            rh[0] = "&" + rh[0];
            //        }
            //    }
            //}
            ////

            ////
            ////for (int i = 901; i <= 991; i++)
            ////{
            ////foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] == i))
            //foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] >= 900))
            //{
            //    foreach (var rh in rhe.Value)
            //    {
            //        rh[2] = " I";
            //    }
            //}
            ////}

            ////inverter linhas da RE 433 linha LU
            //var luMod = entdados.BlocoRhe.Where(x => x is LuLine && x.Restricao == 433 && x[9] == 99999).FirstOrDefault();
            //if (luMod != null)
            //{
            //    string texto = null;
            //    if (luMod.Comment != null)
            //    {
            //        var linhas = luMod.Comment.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            //        foreach (var linha in linhas)
            //        {
            //            if (linha.StartsWith("&LU"))
            //            {
            //                texto = texto == null ? linha.Substring(1) : texto + Environment.NewLine + linha.Substring(1);

            //            }
            //            else
            //            {
            //                texto = texto == null ? linha : texto + Environment.NewLine + linha;
            //            }
            //        }
            //        luMod.Comment = texto;
            //    }
            //    luMod[0] = "&" + luMod[0];
            //}
            #endregion

            List<int> restComment = new List<int>();
            List<int> restUnComment = new List<int>();

            var entLinhas = File.ReadAllLines(fileEntdadosRef).ToList();

            var blocosId = "RE LU FH FT FI FE FR FC".Split(' ').ToList();
            foreach (var linha in entLinhas)
            {
                string texto = linha;
                if (linha.StartsWith("&") && linha != "")
                {
                    var indice = entLinhas.IndexOf(linha);
                    var l = linha.Substring(1);
                    var cod = (l + "  ").Split(' ').First();
                    if (blocosId.Any(k => k.Equals(cod)))
                    {
                        var newBlock = new Compass.CommomLibrary.EntdadosDat.RheBlock();
                        var newL = newBlock.CreateLine(l);
                        if (restComment.All(x => x != newL.Restricao))
                        {
                            restComment.Add(newL.Restricao);
                        }
                    }
                }
            }
            foreach (var rhe in entdadosRef.BlocoRhe.RheGrouped)
            {
                if (restUnComment.All(x => x != rhe.Key.Restricao))
                {
                    restUnComment.Add(rhe.Key.Restricao);
                }
            }
            foreach (var com in restComment)
            {
                foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] == com))
                {
                    foreach (var rh in rhe.Value)
                    {
                        rh[0] = "&" + rh[0];
                    }
                }
            }
            entdados.SaveToFile();


            List<string> newTexto = new List<string>();

            entLinhas = null;

            entLinhas = File.ReadAllLines(entdadosFile).ToList();
            foreach (var linha in entLinhas)
            {
                string texto = linha;
                //retirando comentarios "&" das restrições
                if (linha.StartsWith("&") && linha != "")
                {
                    var indice = entLinhas.IndexOf(linha);
                    var l = linha.Substring(1);
                    var cod = (l + "  ").Split(' ').First();
                    if (blocosId.Any(k => k.Equals(cod)))
                    {
                        var newBlock = new Compass.CommomLibrary.EntdadosDat.RheBlock();
                        var newL = newBlock.CreateLine(l);
                        if (restUnComment.Any(x => x == newL.Restricao))
                        {
                            texto = l;
                        }
                    }
                }
                newTexto.Add(texto);
            }
            File.WriteAllLines(entdadosFile, newTexto);
            entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

            foreach (var rhe in entdados.BlocoRhe.RheGrouped.Where(x => x.Key[1] >= 900))
            {
                foreach (var rh in rhe.Value)
                {
                    rh[2] = " I";
                }
            }
            entdados.SaveToFile();

            //copia rests do entdados referencia que não existem no deck ons 
            var restCopy = entdadosRef.BlocoRhe.RheGrouped.Where(x => x.Key.Restricao >= 800).ToList();
            foreach (var restC in restCopy)
            {
                if (entdados.BlocoRhe.All(x => x.Restricao != restC.Key.Restricao))
                {
                    foreach (var res in restC.Value)
                    {
                        res.DiaInic = " I";
                        res.HoraInic = null;
                        res.MeiaHoraInic = null;

                        res.DiaFinal = " F";
                        res.HoraFinal = null;
                        res.MeiaHoraFinal = null;

                        entdados.BlocoRhe.Add(res);
                    }
                }
            }
            entdados.SaveToFile();

            //
        }

        public static void TrataIa(Compass.CommomLibrary.EntdadosDat.EntdadosDat entdados, DateTime dataEstudo)
        {
            foreach (var line in entdados.BlocoIa.ToList())
            {
                Boolean apagaCom = false;
                int i;
                int indice = entdados.BlocoIa.IndexOf(line);
                if (line.Comment != null)
                {
                    var comentarios = line.Comment.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                    var texto = line.Comment;
                    foreach (var item in comentarios.Where(x => x.StartsWith("&IA")).ToList())
                    {
                        apagaCom = true;
                        string linha = string.Empty;
                        if (item != comentarios.Last())
                        {
                            linha = item + "\r\n";
                        }
                        else
                        {
                            linha = item;
                        }
                        texto = texto.Replace(linha, "");
                        var newLine = entdados.BlocoIa.CreateLine(item.Substring(1));

                        if (int.TryParse(newLine.DiaInic, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out i))
                        {
                            if (i <= dataEstudo.Day)
                            {
                                if (newLine.SistemaA.Trim() == "N" && newLine.SistemaB.Trim() == "SE")
                                {
                                    newLine[0] = "&" + newLine[0];
                                }

                                entdados.BlocoIa.Insert(indice, newLine);
                                indice += 1;
                            }
                            else
                            {
                                if (newLine.SistemaA.Trim() == "N" && newLine.SistemaB.Trim() == "FC")
                                {
                                    entdados.BlocoIa.Insert(indice, newLine);
                                    indice += 1;
                                }
                                else
                                {
                                    newLine[0] = "&" + newLine[0];
                                    entdados.BlocoIa.Insert(indice, newLine);
                                    indice += 1;
                                }

                            }
                        }
                        else
                        {
                            if (newLine.DiaInic == "I")
                            {
                                if (newLine.SistemaA.Trim() == "N" && newLine.SistemaB.Trim() == "SE")
                                {
                                    newLine[0] = "&" + newLine[0];
                                }
                                entdados.BlocoIa.Insert(indice, newLine);
                                indice += 1;
                            }
                            else if (newLine.DiaInic == "F")
                            {
                                if (newLine.SistemaA.Trim() == "N" && newLine.SistemaB.Trim() == "FC")
                                {
                                    entdados.BlocoIa.Insert(indice, newLine);
                                    indice += 1;
                                }
                                else
                                {
                                    newLine[0] = "&" + newLine[0];
                                    entdados.BlocoIa.Insert(indice, newLine);
                                    indice += 1;
                                }

                            }

                        }
                        if (item == comentarios.Last() && texto != "")
                        {
                            entdados.BlocoIa.First().Comment = texto;
                        }
                    }

                }

                if (int.TryParse(line.DiaInic, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out i))
                {
                    if (i <= dataEstudo.Day)
                    {
                        if ((line.SistemaA.Trim() == "N" && line.SistemaB.Trim() == "SE") || (line.SistemaA.Trim() == "N" && line.SistemaB.Trim() == "FC"))
                        {
                            line.Comment = null;
                            continue;
                        }
                        else
                        {
                            line[0] = "&" + line[0];
                        }
                    }
                }
                else if (line.DiaInic == "I")
                {
                    if ((line.SistemaA.Trim() == "N" && line.SistemaB.Trim() == "SE") || (line.SistemaA.Trim() == "N" && line.SistemaB.Trim() == "FC"))
                    {
                        line.Comment = null;
                        continue;
                    }
                    else
                    {
                        line[0] = "&" + line[0];
                    }
                }

                if (apagaCom)
                {
                    line.Comment = null;
                }
            }
        }

        public static Compass.CommomLibrary.EntdadosDat.DeBlock GetDEBlock(DateTime data)
        {
            var dadosDE = new Compass.CommomLibrary.EntdadosDat.DeBlock();

            DateTime dat = data;
            DateTime datVE = data;
            if (dat.DayOfWeek == DayOfWeek.Friday)
            {
                datVE = dat.AddDays(-1);
            }
            var rev = Tools.GetCurrRev(datVE);
            //H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\2021\01_jan\RV3\DS_CCEE_012021_SEMREDE_RV3D19
            var mes = Tools.GetMonthNumAbrev(rev.revDate.Month);//dataRef
            var cam = $@"H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
            // var cam = $@"N:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
            if (Directory.Exists(cam))
            {
                var entdadosFile = Directory.GetFiles(cam).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).FirstOrDefault();
                if (entdadosFile != null)
                {
                    var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;
                    var blockLines = entdados.BlocoDe.Where(x => Convert.ToInt32(x.DiaInic) == dat.Day).ToList();
                    foreach (var blockl in blockLines)
                    {
                        if (blockl.NumDemanda >= 1 && blockl.NumDemanda <= 4)
                        {
                            dadosDE.Add(blockl);
                        }
                    }
                    return dadosDE;
                }

            }

            return dadosDE;
        }

        public static void ComentaCICE(Compass.CommomLibrary.EntdadosDat.EntdadosDat entdados)
        {
            Dictionary<int, int> barras = new Dictionary<int, int>() {
                    {7059,1 },
                    {7054,1 },
                    {7055,1 },
                    {7057,1 },
                    {3962,1 },
                    {3963,1 },
                    {9637,1 },
                    {181,1 },
                    {185,1 },
                    {190,1 },
                    {3112,1 },
                    {8100,4 },
                    {3010,1 },
                    {85,1 },
                    {86,1 },
                    {9605,1 },
                    {5647,4 },
                    {5648,4 },
                    {5650,4 },

                };

            List<int> restComent = new List<int> { 101, 102, 122, 111, 112, 121, 131, 132, 141, 142, 151, 152, 302, 301, 311, 312, 501, 511, 502, 512 };
            foreach (var item in restComent.ToList())
            {
                foreach (var line in entdados.BlocoCice.Where(x => x.IdContrato == item).ToList())
                {
                    line[0] = "&" + line[0];
                }
                foreach (var line in entdados.BlocoCice.ToList())
                {
                    var chave = line.Sist_Barra;
                    if (barras.ContainsKey(chave))
                    {
                        line.Sist_Barra = barras[chave];
                    }
                }
            }


        }
        static void cortes_tendencia(string path)
        {
            string dir;
            if (Directory.Exists(path))
            {
                dir = path;
            }
            else
                return;

            var decks = Directory.GetDirectories(dir)
                .Where(x => Directory.GetFiles(x, "dadger.*", SearchOption.TopDirectoryOnly).Length > 0);

            corte_tendencia(decks.ToArray());
        }

        static void corte_tendencia(string path)
        {

            string dir;
            if (Directory.Exists(path))
            {
                dir = path;
            }
            else if (File.Exists(path))
            {
                dir = Path.GetDirectoryName(path);
            }
            else
                return;

            var deck = DeckFactory.CreateDeck(dir);

            if ((deck is Compass.CommomLibrary.Dessem.Deck))
            {
                corte_Dessem(dir);
            }
            else
            {
                corte_tendencia(new string[] { dir });

            }

        }

        static void corte_Dessem(string deck)
        {
            try
            {

                if (deck.Count() > 0)
                {
                    Thread thread = new Thread(cortesDessemTHSTA);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    thread.Start(deck);
                    thread.Join(); //Wait for the thread to end      

                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        static void corte_tendencia(params string[] decks)
        {
            try
            {

                if (decks.Count() > 0)
                {
                    Thread thread = new Thread(cortesTHSTA);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    thread.Start(decks);
                    thread.Join(); //Wait for the thread to end      

                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        //static void corte(string path) {

        //    try {
        //        string dir;
        //        if (Directory.Exists(path)) {
        //            dir = path;
        //        } else if (File.Exists(path)) {
        //            dir = Path.GetDirectoryName(path);
        //        } else
        //            return;

        //        var deck = DeckFactory.CreateDeck(dir);

        //        if (deck is CommomLibrary.Decomp.Deck) {
        //            Thread thread = new Thread(cortesSTA);
        //            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
        //            thread.Start(((CommomLibrary.Decomp.Deck)deck)[CommomLibrary.Decomp.DeckDocument.dadger].BasePath);
        //            thread.Join(); //Wait for the thread to end                    
        //        }

        //    } catch (Exception ex) {
        //        System.Windows.Forms.MessageBox.Show(ex.Message);
        //    } finally {

        //    }
        //}

        //static void cortes(string path) {

        //    try {

        //        string dir;
        //        if (Directory.Exists(path)) {
        //            dir = path;
        //        } else
        //            return;

        //        var decks = Directory.GetDirectories(dir)
        //            .Where(x => Directory.GetFiles(x, "dadger.*", SearchOption.TopDirectoryOnly).Length > 0);

        //        if (decks.Count() > 0) {
        //            corte(decks.First());

        //            if (corteOK) {
        //                var deckBase = DeckFactory.CreateDeck(decks.First());

        //                var dadgerBase = (Compass.CommomLibrary.Dadger.Dadger)((CommomLibrary.Decomp.Deck)deckBase)[CommomLibrary.Decomp.DeckDocument.dadger].Document;
        //                var fcBase = (Compass.CommomLibrary.Dadger.FcBlock)dadgerBase.Blocos["FC"];

        //                foreach (var deck in decks.Skip(1)) {

        //                    var deckCopy = DeckFactory.CreateDeck(deck);

        //                    var dadgerCopy = (Compass.CommomLibrary.Dadger.Dadger)((CommomLibrary.Decomp.Deck)deckCopy)[CommomLibrary.Decomp.DeckDocument.dadger].Document;
        //                    dadgerCopy.Blocos["FC"] = fcBase;

        //                    dadgerCopy.SaveToFile();
        //                }
        //            }
        //        }


        //    } catch (Exception ex) {
        //        System.Windows.Forms.MessageBox.Show(ex.Message);
        //    } finally {

        //    }
        //}

        //static bool corteOK = false;
        //static void cortesSTA(object path) {
        //    var dadger = (Compass.CommomLibrary.Dadger.Dadger)DocumentFactory.Create((string)path);

        //    var frm = new FrmCortes(dadger);

        //    corteOK = frm.ShowDialog() == DialogResult.OK;
        //}


        static bool DessemAutorun(string path, string comando)
        {
            try
            {
                var nameCommand = "AutoDESSEM" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var comm = new { CommandName = nameCommand, EnviarEmail = true, WorkingDirectory = path, Command = comando, User = "AutoRun", IgnoreQueue = true };

                var cont = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(comm));
                cont.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");

                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

                //var responseTsk = httpClient.PostAsync("http://azcpspldv02.eastus.cloudapp.azure.com:5015/api/Command", cont);
                // var responseTsk = httpClient.PostAsync("http://10.206.194.196:5015/api/Command", cont);
                var responseTsk = httpClient.PostAsync("http://10.206.194.210:5015/api/Command", cont);
                responseTsk.Wait();
                var response = responseTsk.Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                return true;
            }
            catch (Exception erro)
            {
                Program.AutoClosingMessageBox.Show("Deu erro: " + erro.Message, "Caption", 2000);
                return false;
            }
        }

        static bool AutorunDecodess(string path, string comando)
        {
            try
            {
                var nameCommand = "DecodessAuto" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var comm = new { CommandName = nameCommand, EnviarEmail = false, WorkingDirectory = path, Command = comando, User = "AutoRun", IgnoreQueue = true };

                var cont = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(comm));
                cont.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");

                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

                //var responseTsk = httpClient.PostAsync("http://azcpspldv02.eastus.cloudapp.azure.com:5015/api/Command", cont);
                var responseTsk = httpClient.PostAsync("http://10.206.194.210:5015/api/Command", cont);

                //var responseTsk = httpClient.PostAsync("http://ec2-44-201-188-49.compute-1.amazonaws.com:5015/api/Command", cont);
                responseTsk.Wait();
                var response = responseTsk.Result;

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Falha na Conversão Decodess!");
                return false;
            }
        }

        static void PreliminarAutorun(string path, string comando)
        {
            try
            {
                var nameCommand = "DcNwPreli" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var comm = new { CommandName = nameCommand, EnviarEmail = true, WorkingDirectory = path, Command = comando, User = "AutoRun", IgnoreQueue = true };

                var cont = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(comm));
                cont.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");

                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

                //var responseTsk = httpClient.PostAsync("http://ec2-44-201-188-49.compute-1.amazonaws.com:5015/api/Command", cont);
                var responseTsk = httpClient.PostAsync("http://10.206.194.210:5015/api/Command", cont);
                responseTsk.Wait();
                var response = responseTsk.Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

            }
            catch (Exception erro)
            {
                Program.AutoClosingMessageBox.Show("Deu erro: " + erro.Message, "Caption", 2000);
            }
        }



        static void pldDessemTHSTA(object path)
        {
            var frm = new FrmPldDessem((string)path);
            frm.ShowDialog();
        }

        static void ResDatabaseToolsTHSTA(object path)
        {
            var frm = new FrmResDataBase((string)path);
            frm.ShowDialog();
        }
        static void dessemToolsTHSTA(object path)
        {
            var frm = new FrmDessemTools((string)path);
            frm.ShowDialog();
        }

        static void gerarMultGraph(object[] par)
        {
            Thread thread = new Thread(graphDp);
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start(par);
            thread.Join(); //Wait for the thread to end    
                           // var frm = new FrmExtriDE(dir);
                           //frm.ShowDialog();
        }

        //public static void graphDp(object path, object data, bool banco = false, float fator = 1)
        public static void graphDp(object parametros)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            string comandos = (string)parametros;
            var coms = comandos.Split(';').ToList();

            if (coms.Count > 2)
            {
                var frm = new FrmGraphDp(coms[0], Convert.ToDateTime(coms[1], Culture.DateTimeFormat), Convert.ToBoolean(coms[2]), float.Parse(coms[3]));
                //var frm = new FrmGraphDp((string)path);
                frm.ShowDialog();
            }
            else
            {
                var frm = new FrmGraphDp(coms[0], Convert.ToDateTime(coms[1], Culture.DateTimeFormat));
                //var frm = new FrmGraphDp((string)path);
                frm.ShowDialog();
            }

        }

        public static void DStools_ResGraph(object path)
        {
            try
            {

                string dir;
                if (Directory.Exists((string)path))
                {
                    dir = (string)path;
                }
                else if (File.Exists((string)path))
                {
                    dir = Path.GetDirectoryName((string)path);
                }
                else
                {
                    return;
                }

                var deck = DeckFactory.CreateDeck(dir);

                if (deck is CommomLibrary.Dessem.Deck)
                {

                    var results = deck.GetResults();
                    FrmDSresulGraph.Show(dir, results);
                }
                else
                {
                    string aviso = "Deck não reconhecido!";
                    MessageBox.Show(aviso, "DESSEM-TOOLS");
                    //return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public static void DSTool_AllGraphs(object path)
        {
            try
            {
                string cam = (string)path;
                string dir;
                if (Directory.Exists((string)path))
                {
                    dir = cam.EndsWith(Path.DirectorySeparatorChar.ToString()) ? cam.Remove(cam.Length - 1) : cam;
                }
                else
                    return;


                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories)
                    .AsParallel()//.WithDegreeOfParallelism(4)                       
                    .Select(x => new
                    {
                        dir = x.Remove(0, dir.Length),
                        deck = DeckFactory.CreateDeck(x),
                    });

                var dNw = dirs.Where(x => x.deck is CommomLibrary.Newave.Deck)
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                var dDc = dirs.Where(x => x.deck is CommomLibrary.Decomp.Deck).AsParallel()
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                var dDs = dirs.Where(x => x.deck is CommomLibrary.Dessem.Deck).AsParallel()
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                //if (dNw.Count() > 0) FormViewer.Show("NEWAVE", dNw.Select(x => x.result).ToArray());
                //if (dDc.Count() > 0) FormViewer.Show("DECOMP", dDc.Select(x => x.result).ToArray());
                if (dDs.Count() > 0) FrmDSresulGraph.Show("DESSEM", dDs.Select(x => x.result).ToArray());

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public static void DSTool_AllResultados(object path)
        {
            try
            {
                string cam = (string)path;
                string dir;
                if (Directory.Exists((string)path))
                {
                    dir = cam.EndsWith(Path.DirectorySeparatorChar.ToString()) ? cam.Remove(cam.Length - 1) : cam;
                }
                else
                    return;


                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories)
                    .AsParallel()//.WithDegreeOfParallelism(4)                       
                    .Select(x => new
                    {
                        dir = x.Remove(0, dir.Length),
                        deck = DeckFactory.CreateDeck(x),
                    });

                var dNw = dirs.Where(x => x.deck is CommomLibrary.Newave.Deck)
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                var dDc = dirs.Where(x => x.deck is CommomLibrary.Decomp.Deck).AsParallel()
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                var dDs = dirs.Where(x => x.deck is CommomLibrary.Dessem.Deck).AsParallel()
                    .Select(x => new
                    {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                if (dNw.Count() > 0) FormViewer.Show("NEWAVE", dNw.Select(x => x.result).ToArray());
                if (dDc.Count() > 0) FormViewer.Show("DECOMP", dDc.Select(x => x.result).ToArray());
                if (dDs.Count() > 0) FormViewer.Show("DESSEM", dDs.Select(x => x.result).ToArray());

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public static void testeProgress()
        {
            Thread.Sleep(10000);
        }


        public static void DStools_complSem(object comando)
        {
            try
            {
                string dir;
                bool expand;
                string path;
                string comandos = (string)comando;
                var coms = comandos.Split('|').ToList();
                if (coms.Count() > 1)
                {
                    path = coms[0];
                    expand = Convert.ToBoolean(coms[1]);
                }
                else
                {
                    path = coms[0];
                    expand = false;
                }

                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                {
                    return;
                }
                var deck = DeckFactory.CreateDeck(dir);

                if (deck is CommomLibrary.Dessem.Deck)
                {
                    //todo logica de verificar qual dia é o deck 
                    var dadvaz = Directory.GetFiles(deck.BaseFolder).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz")).First();

                    var dadlinhas = File.ReadAllLines(dadvaz).ToList();
                    var dados = dadlinhas[9].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime dataDeck = new DateTime(Convert.ToInt32(dados[3]), Convert.ToInt32(dados[2]), Convert.ToInt32(dados[1]));
                    if (dataDeck.DayOfWeek != DayOfWeek.Friday)
                    {
                        var revDate = Tools.GetCurrRev(dataDeck).revDate;
                        Completa_SemanaDessem(deck as CommomLibrary.Dessem.Deck, dataDeck, revDate, expand);

                        string aviso = "Processo concluído!";
                        MessageBox.Show(aviso, "DESSEM-TOOLS");
                    }
                    else
                    {
                        string aviso = "Data do Deck é uma sexta-feira escolha outro deck!";
                        MessageBox.Show(aviso, "DESSEM-TOOLS");
                    }


                }
                else
                {
                    string aviso = "Deck não reconhecido!";
                    MessageBox.Show(aviso, "DESSEM-TOOLS");
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Arquivo Níveis de partida não encontrados para criação do Deflant.dat"))
                {
                    string texto = "Arquivo Níveis de partida não encontrados para criação do Deflant.dat, processo interrompido";
                    texto = ex.ToString() + ", processo interrompido.";
                    texto = ex.Message + ", processo interrompido.";
                    MessageBox.Show(texto, "Dessem Tools");

                }
                else
                {
                    string texto = ex.Message;
                    texto = texto + ", processo interrompido.";
                    texto = ex.Message + ", processo interrompido.";
                    MessageBox.Show(texto, "Dessem Tools");
                }

            }

        }

        public static void DStools_resultado(object path)
        {
            try
            {

                string dir;
                if (Directory.Exists((string)path))
                {
                    dir = (string)path;
                }
                else if (File.Exists((string)path))
                {
                    dir = Path.GetDirectoryName((string)path);
                }
                else
                {
                    FormViewer.Show("", new Result());
                    return;
                }

                var deck = DeckFactory.CreateDeck(dir);

                if (deck is CommomLibrary.Newave.Deck || deck is CommomLibrary.Decomp.Deck || deck is CommomLibrary.Dessem.Deck)
                {

                    var results = deck.GetResults();
                    FormViewer.Show(dir, results);
                }
                else
                {
                    string aviso = "Deck não reconhecido!";
                    MessageBox.Show(aviso, "DESSEM-TOOLS");
                    //return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void blocoDETHSTA(object path)
        {
            var frm = new FrmExtriDE((string)path);
            frm.ShowDialog();
        }

        static void cortesDessemTHSTA(object paths)
        {
            var frm = new FrmMapCorteDS((string)paths);
            frm.ShowDialog();
        }

        static void cortesTHSTA(object paths)
        {
            var frm = new FrmCortes((string[])paths);
            frm.ShowDialog();
        }

#if DEBUG
        public string apiUrl = @"http://ec2-44-201-188-49.compute-1.amazonaws.com:5014/api/";
#else
        public string apiUrl = @"http://ec2-44-201-188-49.compute-1.amazonaws.com:5014/api/";
#endif

        static void setConfigFile()
        {

            string path = "Compass.DecompTools.dll.config";

            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);
            typeof(System.Configuration.ConfigurationManager).GetField("s_initState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, 0);

        }

        static void tratarInviabilidades(string path)
        {

            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                }
                else
                    return;


                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories)
                        .AsParallel()//.WithDegreeOfParallelism(4)                       
                        .Select(x => new
                        {
                            dir = x.Remove(0, dir.Length),
                            deck = DeckFactory.CreateDeck(x),
                        });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        static void tratarInviabilidade(string path)
        {
            try
            {

                string dir;
                if (Directory.Exists(path))
                {
                    dir = path;
                }
                else if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                else
                    return;

                var deck = DeckFactory.CreateDeck(dir) as Compass.CommomLibrary.Decomp.Deck;

                if (deck != null)
                {

                    var fi = System.IO.Directory.GetFiles(dir, "inviab_unic.*", SearchOption.TopDirectoryOnly).FirstOrDefault();

                    if (fi != null)
                    {
                        var inviab = (Compass.CommomLibrary.Inviab.Inviab)DocumentFactory.Create(fi);
                        Services.Deck.DesfazerInviabilidades(deck, inviab);

                        string newPath;
                        duplicar(dir, out newPath);

                        var originalFile = deck[CommomLibrary.Decomp.DeckDocument.dadger].Document.File;
                        var newFile = originalFile.Replace(dir, newPath);

                        deck[CommomLibrary.Decomp.DeckDocument.dadger].Document.SaveToFile(newFile, true);

                    }
                    else
                        throw new Exception("Arquivo inviab_unic.xxx não encontrado.");
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void tendhidr(string path)
        {
            try
            {

                Thread thread = new Thread(tendhidrSTA);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(path);
                thread.Join(); //Wait for the thread to end                    

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        static void tendhidrSTA(object path)
        {
            string pa = (string)path;
            var frm = new FrmTendenciaHidr();

            if (System.IO.File.Exists(pa))
            {

                if (pa.ToLowerInvariant().EndsWith("vazoes.dat"))
                {
                    frm.VazoesDat = pa;
                }
                else if (pa.ToLowerInvariant().EndsWith("vazpast.dat"))
                {
                    frm.VazpastDat = pa;
                }
            }

            frm.ShowDialog();
        }

        static void AtualizaDeckCarga(object DeckPath)
        {
            var deck = DeckPath;
            var frm = new FrmAtualizaCarga();
            frm.deckNW = deck as CommomLibrary.Newave.Deck;

            frm.ShowDialog();
        }

        static void nwOnsReCcee(object ONSDeck)
        {
            var deck = ONSDeck;
            var frm = new FrmOnsReCcee();
            frm.deckONS = deck as CommomLibrary.Newave.Deck;

            frm.ShowDialog();
        }
        static void updateConfhProcess(object Deck)
        {
            var deck = Deck;
            var frm = new FrmUpdateConfhd();
            frm.deckNW = deck as CommomLibrary.Newave.Deck;

            frm.ShowDialog();
        }

        static void updateWeolNWProcess(object Deck)
        {
            var deck = Deck;
            var frm = new FrmUpdateWeolNwDc();
            frm.deckNW = deck as CommomLibrary.Newave.Deck;

            frm.ShowDialog();
        }

        static void updateWeolDCProcess(object Deck)
        {
            var deck = Deck;
            var frm = new FrmUpdateWeolNwDc();
            frm.deckDC = deck as CommomLibrary.Decomp.Deck;

            frm.ShowDialog();
        }

        static void dcOns2CceeSTA(object dcDeck)
        {
            var deck = dcDeck as Compass.CommomLibrary.Decomp.Deck;
            var frm = new FrmDcOns2Ccee(deck);
            //frm.Deck = deck;

            frm.ShowDialog();
        }

        static void dsOns2CceeSTA(object dsDeck)
        {
            string deck = dsDeck.ToString();
            var frm = new FrmDsOns2CCEE(deck);
            //frm.Deck = deck;

            frm.ShowDialog();
        }

    }
}

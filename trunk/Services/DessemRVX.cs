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
    }
}

using Compass.CommomLibrary;
using Compass.ExcelTools;
using Compass.Services;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX {

    class Program {

        static Dictionary<string, Action<string>> actions = new Dictionary<string, Action<string>>();

        static void Main(string[] args) {

            setConfigFile();

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
            actions.Add("inviab", tratarInviabilidade);
            actions.Add("resultados", resultados);
            actions.Add("previvaz", previvaz);
            actions.Add("tendhidr", tendhidr);


            if (args.Length > 1) {
                var action = args[0].ToLower();

                if (actions.ContainsKey(action)) {
                    actions[action].Invoke(args[1]);
                }
            } else {
                resultado("");


            }
        }

        static void vazoes(string path) {
            Vazoes gevazp = null;

            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else
                    return;
                gevazp = new Compass.Services.Vazoes();
                var files = gevazp.Run(dir, true);

                var prevcenRel = files.FirstOrDefault(f => f.FullPath.EndsWith("prevcen.rel", StringComparison.OrdinalIgnoreCase));
                if (prevcenRel != null) {

                    var relContent = File.ReadAllText(prevcenRel.FullPath);
                    var pat = @"USINA\s*:.+VALOR:\s+-\d+";

                    var vNegativas = System.Text.RegularExpressions.Regex.Matches(relContent, pat, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    if (vNegativas.Count > 0) {

                        var alert = "";

                        foreach (System.Text.RegularExpressions.Match m in vNegativas) {
                            alert += m.Value + "\r\n";
                        }

                        MessageBox.Show(alert, "Vazoes Incrementais Negativas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }



            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                if (gevazp != null)
                    gevazp.ClearTempFolder();
            }
        }

        static void vazoes6(string path) {
            Vazoes6 gevazp = null;

            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else
                    return;
                gevazp = new Compass.Services.Vazoes6();
                var files = gevazp.Run(dir, true);




            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                if (gevazp != null)
                    gevazp.ClearTempFolder();
            }
        }

        static void previvaz(string path) {
            Previvaz previvaz = null;

            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else
                    return;
                previvaz = new Compass.Services.Previvaz();
                var files = previvaz.Run(dir);


            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
            }
        }

        static void open(string filePath) {

            Microsoft.Office.Interop.Excel.Application xlApp = null;

            try {
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

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                if (xlApp != null) {
                    xlApp.Cursor = XlMousePointer.xlDefault;
                    xlApp.ScreenUpdating = true;

                    Helper.Release(xlApp);
                }
            }
        }

        static void armazenamento(string path) {
            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else
                    return;

                var deck = DeckFactory.CreateDeck(dir);

                Compass.CommomLibrary.Decomp.ConfigH configH;
                if (deck is Compass.CommomLibrary.Decomp.Deck) {

                    var dadger = (Compass.CommomLibrary.Dadger.Dadger)DocumentFactory.Create(deck.Documents["DADGER."].BasePath);
                    var hidr = (Compass.CommomLibrary.HidrDat.HidrDat)DocumentFactory.Create(deck.Documents["HIDR.DAT"].BasePath);


                    configH = new Compass.CommomLibrary.Decomp.ConfigH(dadger, hidr);

                } else if (deck is Compass.CommomLibrary.Newave.Deck) {

                    var confhddat = (Compass.CommomLibrary.ConfhdDat.ConfhdDat)DocumentFactory.Create(deck.Documents["CONFHD.DAT"].BasePath);
                    var modifdat = BaseDocument.Create<Compass.CommomLibrary.ModifDatNW.ModifDatNw>(File.ReadAllText(deck.Documents["MODIF.DAT"].BasePath));
                    var hidr = (Compass.CommomLibrary.HidrDat.HidrDat)DocumentFactory.Create(deck.Documents["HIDR.DAT"].BasePath);

                    configH = new Compass.CommomLibrary.Decomp.ConfigH(confhddat, hidr, modifdat);

                } else {
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
                foreach (var sb in configH.index_sistemas) {
                    dtEarm.Rows.Add(
                        sb.Item2.ToString(),
                        earmMax[i].ToString("N1", fmt),
                        earmAtual[i].ToString("N1", fmt),
                        (earmAtual[i] / earmMax[i]).ToString("00.0%", fmt));
                    //rs2.Add(new { Sistema = sb.Item2.ToString(), EarmMax = earmMax[i].ToString("N1", fmt), EarmIni = earmAtual[i].ToString("N1", fmt), EarmIni_Perc = (earmAtual[i] / earmMax[i]).ToString("00.0%", fmt) });
                    i++;
                }

                FormViewer.Show("EARM calculado - " + dir, new ResultDataSource { Title = "Armazenamento", DataSource = dtEarm });


            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void resultado(string path) {
            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else {
                    FormViewer.Show("", new Result());
                    return;
                }

                var deck = DeckFactory.CreateDeck(dir);

                if (deck is CommomLibrary.Newave.Deck || deck is CommomLibrary.Decomp.Deck) {

                    var results = deck.GetResults();
                    FormViewer.Show(dir, results);
                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void resultados(string path) {
            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                } else
                    return;


                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories)
                    .AsParallel()//.WithDegreeOfParallelism(4)                       
                    .Select(x => new {
                        dir = x.Remove(0, dir.Length),
                        deck = DeckFactory.CreateDeck(x),
                    });

                var dNw = dirs.Where(x => x.deck is CommomLibrary.Newave.Deck)
                    .Select(x => new {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                var dDc = dirs.Where(x => x.deck is CommomLibrary.Decomp.Deck).AsParallel()
                    .Select(x => new {
                        x.dir,
                        x.deck,
                        result = x.deck.GetResults()
                    }).Where(x => x.result != null).ToList();

                if (dNw.Count() > 0) FormViewer.Show("NEWAVE", dNw.Select(x => x.result).ToArray());
                if (dDc.Count() > 0) FormViewer.Show("DECOMP", dDc.Select(x => x.result).ToArray());

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void dgernwd(string path) {
            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else
                    return;

                Services.Deck.CreateDgerNewdesp(dir);

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        

        static void duplicar(string path) {

            string newPath;

            duplicar(path, out newPath);

        }

        static void duplicar(string path, out string newPath) {
            newPath = "";
            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else
                    return;

                var dirInfo = new DirectoryInfo(dir);
                var parentDir = dirInfo.Parent.FullName;
                var dirName = dirInfo.Name;

                var i = 0;
                var cloneDir = "";
                do {
                    cloneDir = Path.Combine(parentDir, dirName + " (" + ++i + ")");
                } while (Directory.Exists(cloneDir));

                var deck = DeckFactory.CreateDeck(dir);

                newPath = cloneDir;

                deck.CopyFilesToFolder(cloneDir);

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void ons2ccee(string path) {
            try {
                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else
                    return;

                var dirInfo = new DirectoryInfo(dir);
                var parentDir = dirInfo.Parent.FullName;
                var dirName = dirInfo.Name + "_ccee";

                var i = 0;
                var cloneDir = "";
                do {
                    cloneDir = Path.Combine(parentDir, dirName + " (" + ++i + ")");
                } while (Directory.Exists(cloneDir));



                var deck = DeckFactory.CreateDeck(dir);

                if (deck is Compass.CommomLibrary.Newave.Deck) {
                    if (((Compass.CommomLibrary.Newave.Deck)deck)[CommomLibrary.Newave.Deck.DeckDocument.cadterm] == null) {
                        System.Windows.Forms.MessageBox.Show("Não existe arquivo CADTERM no deck para realizar a conversão. Copie o arquivo e tente novamente.", "Decomp Tools", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                } else if (!(deck is Compass.CommomLibrary.Decomp.Deck)) {
                    throw new NotImplementedException("Deck não reconhecido para a execução (somente newave implementado");
                }



                deck.CopyFilesToFolder(cloneDir);

                dynamic cceeDeck = DeckFactory.CreateDeck(cloneDir);

                //Compass.Services.Deck.Ons2Ccee(cceeDeck);

                if (cceeDeck is Compass.CommomLibrary.Newave.Deck) {
                    Compass.Services.Deck.Ons2Ccee(cceeDeck);
                } else if (cceeDeck is Compass.CommomLibrary.Decomp.Deck) {
                    //    Compass.Services.Deck.Ons2Ccee(cceeDeck);

                    Thread thread = new Thread(dcOns2CceeSTA);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    thread.Start(cceeDeck);
                    thread.Join(); //Wait for the thread to end   
                }

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        static void cortes_tendencia(string path) {


            string dir;
            if (Directory.Exists(path)) {
                dir = path;
            } else
                return;

            var decks = Directory.GetDirectories(dir)
                .Where(x => Directory.GetFiles(x, "dadger.*", SearchOption.TopDirectoryOnly).Length > 0);

            corte_tendencia(decks.ToArray());
        }

        static void corte_tendencia(string path) {

            string dir;
            if (Directory.Exists(path)) {
                dir = path;
            } else if (File.Exists(path)) {
                dir = Path.GetDirectoryName(path);
            } else
                return;

            corte_tendencia(new string[] { dir });
        }

        static void corte_tendencia(params string[] decks) {
            try {

                if (decks.Count() > 0) {

                    Thread thread = new Thread(cortesTHSTA);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    thread.Start(decks);
                    thread.Join(); //Wait for the thread to end      



                    //corte(decks.First());

                    //if (corteOK) {
                    //    var deckBase = DeckFactory.CreateDeck(decks.First());

                    //    var dadgerBase = (Compass.CommomLibrary.Dadger.Dadger)((CommomLibrary.Decomp.Deck)deckBase)[CommomLibrary.Decomp.DeckDocument.dadger].Document;
                    //    var fcBase = (Compass.CommomLibrary.Dadger.FcBlock)dadgerBase.Blocos["FC"];

                    //    foreach (var deck in decks.Skip(1)) {

                    //        var deckCopy = DeckFactory.CreateDeck(deck);

                    //        var dadgerCopy = (Compass.CommomLibrary.Dadger.Dadger)((CommomLibrary.Decomp.Deck)deckCopy)[CommomLibrary.Decomp.DeckDocument.dadger].Document;
                    //        dadgerCopy.Blocos["FC"] = fcBase;

                    //        dadgerCopy.SaveToFile();
                    //    }

                    //    var caminhoCortes = dadgerBase.CortesPath;

                    //    Compass.CommomLibrary.VazoesC.VazoesC vazoes = null;
                    //    if (((CommomLibrary.Decomp.Deck)deckBase)[CommomLibrary.Decomp.DeckDocument.vazoesc] != null) {
                    //        vazoes = (Compass.CommomLibrary.VazoesC.VazoesC)((CommomLibrary.Decomp.Deck)deckBase)[CommomLibrary.Decomp.DeckDocument.vazoesc].Document;   
                    //    }
                    //    else                         {
                    //        vazoes =
                    //        Compass.CommomLibrary.DocumentFactory.Create(
                    //            System.IO.Path.Combine(
                    //             System.IO.Path.GetDirectoryName(caminhoCortes), "vazoes.dat"
                    //            )
                    //        ) as Compass.CommomLibrary.VazoesC.VazoesC;
                    //    }

                    //    Compass.CommomLibrary.Vazpast.Vazpast vazpast =
                    //        Compass.CommomLibrary.DocumentFactory.Create(
                    //            System.IO.Path.Combine(
                    //             System.IO.Path.GetDirectoryName(caminhoCortes), "vazpast.dat"
                    //            )
                    //        ) as Compass.CommomLibrary.Vazpast.Vazpast;


                    //    Compass.Services.Vazoes6.IncorporarVazpast(vazoes, vazpast, new DateTime(dadgerBase.VAZOES_AnoInicialDoEstudo, dadgerBase.VAZOES_MesInicialDoEstudo, 1));

                    //    foreach (var deck in decks) {
                    //        vazoes.SaveToFile(
                    //            System.IO.Path.Combine(deck, "vazoes.dat"), true
                    //            );
                    //    }                        

                    //    MessageBox.Show("Cortes e Tendencias Hidrológicas alteradas");
                    //}
                }

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {

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

        static void cortesTHSTA(object paths) {

            var frm = new FrmCortes((string[])paths);
            frm.ShowDialog();
        }


        static void setConfigFile() {

            string path = "Compass.DecompTools.dll.config";

            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);
            typeof(System.Configuration.ConfigurationManager).GetField("s_initState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, 0);

        }

        static void tratarInviabilidades(string path) {

            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path.Remove(path.Length - 1) : path;
                } else
                    return;


                var dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories)
                        .AsParallel()//.WithDegreeOfParallelism(4)                       
                        .Select(x => new {
                            dir = x.Remove(0, dir.Length),
                            deck = DeckFactory.CreateDeck(x),
                        });

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        static void tratarInviabilidade(string path) {
            try {

                string dir;
                if (Directory.Exists(path)) {
                    dir = path;
                } else if (File.Exists(path)) {
                    dir = Path.GetDirectoryName(path);
                } else
                    return;

                var deck = DeckFactory.CreateDeck(dir) as Compass.CommomLibrary.Decomp.Deck;

                if (deck != null) {

                    var fi = System.IO.Directory.GetFiles(dir, "inviab_unic.*", SearchOption.TopDirectoryOnly).FirstOrDefault();

                    if (fi != null) {
                        var inviab = (Compass.CommomLibrary.Inviab.Inviab)DocumentFactory.Create(fi);
                        Services.Deck.DesfazerInviabilidades(deck, inviab);

                        string newPath;
                        duplicar(dir, out newPath);

                        var originalFile = deck[CommomLibrary.Decomp.DeckDocument.dadger].Document.File;
                        var newFile = originalFile.Replace(dir, newPath);

                        deck[CommomLibrary.Decomp.DeckDocument.dadger].Document.SaveToFile(newFile, true);

                    } else
                        throw new Exception("Arquivo inviab_unic.xxx não encontrado.");
                }

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void tendhidr(string path) {
            try {

                Thread thread = new Thread(tendhidrSTA);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(path);
                thread.Join(); //Wait for the thread to end                    

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {

            }
        }

        static void tendhidrSTA(object path) {
            string pa = (string)path;
            var frm = new FrmTendenciaHidr();

            if (System.IO.File.Exists(pa)) {

                if (pa.ToLowerInvariant().EndsWith("vazoes.dat")) {
                    frm.VazoesDat = pa;
                } else if (pa.ToLowerInvariant().EndsWith("vazpast.dat")) {
                    frm.VazpastDat = pa;
                }
            }

            frm.ShowDialog();
        }

        static void dcOns2CceeSTA(object dcDeck) {
            var deck = dcDeck as Compass.CommomLibrary.Decomp.Deck;
            var frm = new FrmDcOns2Ccee();
            frm.Deck = deck;

            frm.ShowDialog();
        }


    }


}

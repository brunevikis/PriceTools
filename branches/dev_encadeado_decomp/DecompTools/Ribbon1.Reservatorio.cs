using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.SistemaDat;
using Compass.ExcelTools;
using Compass.ExcelTools.Templates;
using System.IO;

namespace Compass.DecompTools {
    public partial class Ribbon1 {

        Compass.CommomLibrary.HidrDat.HidrDat hidr;

        private void btnReservatorio_Click(object sender, RibbonControlEventArgs e) {

            try {
                var configH = LoadConfigH();

                var info = ActiveWorkbook.GetInfosheet();

                //var reserv = new Compass.Services.Reservatorio((Dadger)doc, (Compass.CommomLibrary.HidrDat.HidrDat)hidr);

                double[] earmAtual = configH.GetEarms();

                info.Earm = earmAtual;

                /***********/


                //informar meta %
                //var earm = new float[] { 27, 40, 23, 56 };
                double[] earmMeta = info.MetaReservatorio;


                //Se meta for relativa (%) e max não informado, calcular;
                var earmMax = new double[0];
                if (earmMeta.All(x => x < 100)) {
                    earmMax = info.EarmMax;
                    if (earmMax == null) {
                        earmMax = configH.GetEarmsMax();
                        info.EarmMax = earmMax;

                        configH.ReloadUH();
                    }
                }

                //atualizar UH
                Compass.Services.Reservatorio.SetUHBlock(configH, earmMeta, earmMax);

                double[] earmFinal = configH.GetEarms();

                var doc = configH.baseDoc;

                Globals.ThisAddIn.Application.ScreenUpdating = false;


                ActiveWorkbook.WriteDocumentToWorkbook(doc);

                //info.BottonComments = doc.BottonComments;
                //info.DocType = type;
                //info.DocPath = doc.File;

                //info.EarmMax = earmMax;
                //info.MetaReservatorio = earmMeta;
                info.Earm = earmFinal;

                var i = 1;
                foreach (var ree in Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.Values.Distinct()) {
                    info.WS.Cells[7 + i, 7].Value = ree;
                    info.WS.Cells[7 + i++, 9].Value =
                         configH.Usinas.Where(u => Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.ContainsKey(u.Cod) && Compass.CommomLibrary.Decomp.ConfigH.uhe_ree[u.Cod] == ree).Sum(u => u.EnergiaArmazenada);

                }

                info.Show();

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnReservatorioRelato_Click(object sender, RibbonControlEventArgs e) {
            try {


                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "relato | relato.*";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                    var relato = (Compass.CommomLibrary.Relato.Relato)DocumentFactory.Create(ofd.FileName);

                    var configH = LoadConfigH();


                    if (configH.baseDoc is Dadger) {
                        var dger = configH.baseDoc as Dadger;

                        var uhBlock = dger.BlocoUh;

                        foreach (var reserv in relato.VolUtil) {
                            var uh = uhBlock.FirstOrDefault(x => x.Usina == reserv.Cod);
                            if (uh != null)
                                uh.VolIniPerc = (float)reserv.VolFinSem1;
                        }
                    } else if (configH.baseDoc is CommomLibrary.ConfhdDat.ConfhdDat) {
                        var confhd = configH.baseDoc as CommomLibrary.ConfhdDat.ConfhdDat;

                        foreach (var reserv in relato.VolUtil) {
                            var uh = confhd.FirstOrDefault(x => x.Cod == reserv.Cod);
                            if (uh != null)
                                uh.VolUtil = (float)reserv.VolFinSem1;
                        }

                    }


                    var info = ActiveWorkbook.GetInfosheet();



                    //atualizar UH


                    double[] earmFinal = configH.GetEarms();

                    Globals.ThisAddIn.Application.ScreenUpdating = false;


                    ActiveWorkbook.WriteDocumentToWorkbook(configH.baseDoc);

                    //info.BottonComments = doc.BottonComments;
                    //info.DocType = type;
                    //info.DocPath = doc.File;

                    //info.EarmMax = earmMax;
                    //info.MetaReservatorio = earmMeta;
                    info.Earm = earmFinal;

                    info.Show();
                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnReservatorioEarm_Click(object sender, RibbonControlEventArgs e) {
            try {

                var configH = LoadConfigH();

                var info = ActiveWorkbook.GetInfosheet();

                var earmAtual = configH.GetEarms();

                info.Sistemas = configH.index_sistemas.Select(x => x.Item2.ToString()).ToArray();
                info.Earm = earmAtual;

                info.Show();

                Globals.ThisAddIn.Application.ScreenUpdating = false;


                //print memoria de calculo
                var xlsM = ActiveWorkbook.GetOrCreateWorksheet("memCal - EARM");
                xlsM.UsedRange.Clear();


                xlsM.Range[xlsM.Cells[1, 1], xlsM.Cells[1, 7]].Value2 = new dynamic[,] {
                    {"Cod", "Usina", "Prod", "ProdTotal", "Vol Ini %", "Sistema", "EARM"}
                };

                var l = 2;
                foreach (var uhe in configH.Usinas.Select(u => new dynamic[,]{                    
                    {u.Cod, u.Usina, u.ProdCalc, u.ProdTotal, u.VolIni , u.Mercado, u.EnergiaArmazenada}
                }
                    )) {
                    xlsM.Range[xlsM.Cells[l, 1], xlsM.Cells[l++, uhe.Length]].Value2 = uhe;

                }

                var i = 1;
                foreach (var ree in Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.Values.Distinct()) {
                    info.WS.Cells[7 + i, 7].Value = ree;
                    info.WS.Cells[7 + i++, 9].Value =
                         configH.Usinas.Where(u => Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.ContainsKey(u.Cod) && Compass.CommomLibrary.Decomp.ConfigH.uhe_ree[u.Cod] == ree).Sum(u => u.EnergiaArmazenada);

                }


            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnReservatorioEarmMax_Click(object sender, RibbonControlEventArgs e) {
            try {


                var configH = LoadConfigH();

                var info = ActiveWorkbook.GetInfosheet();

                var earmAtual = configH.GetEarmsMax();

                info.EarmMax = earmAtual;

                info.Show();

                Globals.ThisAddIn.Application.ScreenUpdating = false;

                //print memoria de calculo
                var xlsM = ActiveWorkbook.GetOrCreateWorksheet("memCal - EARMMax");
                xlsM.UsedRange.Clear();

                xlsM.Range[xlsM.Cells[1, 1], xlsM.Cells[1, 7]].Value2 = new dynamic[,] {
                    {"Cod", "Usina", "Prod", "ProdTotal", "Vol Ini %", "Sistema", "EARMMax"}
                };

                var l = 2;

                var uhes = configH.Usinas.Select(u => new dynamic[,]{                    
                    {u.Cod, u.Usina, u.ProdCalc, u.ProdTotal, u.VolIni , u.Mercado, u.EnergiaArmazenada}
                }
                );

                foreach (var uhe in uhes) {
                    xlsM.Range[xlsM.Cells[l, 1], xlsM.Cells[l++, uhe.Length]].Value2 = uhe;

                }

                var i = 1;
                foreach (var ree in Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.Values.Distinct()) {
                    info.WS.Cells[7 + i, 7].Value = ree;
                    info.WS.Cells[7 + i++, 8].Value =
                         configH.Usinas.Where(u => Compass.CommomLibrary.Decomp.ConfigH.uhe_ree.ContainsKey(u.Cod) && Compass.CommomLibrary.Decomp.ConfigH.uhe_ree[u.Cod] == ree).Sum(u => u.EnergiaArmazenada);
                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        private void btnReservatorioRDH_Click(object sender, RibbonControlEventArgs e) {
            var xlApp = Globals.ThisAddIn.Application;
            try {


                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "rdh | *.xls*";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                    // var relato = (Compass.CommomLibrary.Relato.Relato)DocumentFactory.Create(ofd.FileName);

                    xlApp.ScreenUpdating = false;

                    var xlWbRdh = xlApp.Workbooks.Open(ofd.FileName, ReadOnly: true);

                    var rdh = new WorkbookRdh(xlWbRdh);

                    xlWbRdh.Close();

                    var configH = LoadConfigH();

                    if (configH.baseDoc is Dadger) {
                        var dger = configH.baseDoc as Dadger;

                        var uhBlock = dger.BlocoUh;

                        var q = from r in rdh
                                join h in configH.Usinas on r.Posto equals h.Posto
                                select new { h.Cod, r.VolUtilArm, h, r.Nivel };

                        foreach (var reserv in q) {
                            var uh = uhBlock.FirstOrDefault(x => x.Usina == reserv.Cod);
                            if (uh != null) {
                                if (uh.Usina == 66) uh.VolIniPerc = reserv.h.GetVolumePorCota(reserv.Nivel);
                                else
                                    uh.VolIniPerc = uh.VolIniPerc != 0 ? reserv.VolUtilArm : 0;
                            }
                        }

                    } else if (configH.baseDoc is CommomLibrary.ConfhdDat.ConfhdDat) {
                        var confhd = configH.baseDoc as CommomLibrary.ConfhdDat.ConfhdDat;

                        var q = from r in rdh
                                join h in configH.Usinas on r.Posto equals h.Posto
                                select new { h.Cod, r.VolUtilArm, h, r.Nivel };

                        foreach (var reserv in q) {
                            var uh = confhd.FirstOrDefault(x => x.Cod == reserv.Cod);
                            if (uh != null) {
                                if (uh.Cod == 66) uh.VolUtil = reserv.h.GetVolumePorCota(reserv.Nivel);
                                else
                                    uh.VolUtil = uh.VolUtil != 0 ? reserv.VolUtilArm : 0;
                            }
                        }

                    }

                    var info = ActiveWorkbook.GetInfosheet();

                    //atualizar UH


                    var earmFinal = configH.GetEarms();

                    Globals.ThisAddIn.Application.ScreenUpdating = false;


                    ActiveWorkbook.WriteDocumentToWorkbook(configH.baseDoc);


                    info.Earm = earmFinal;

                    info.Show();
                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        Compass.CommomLibrary.Decomp.ConfigH LoadConfigH(string dadgerPath = null) {

            //verificar se tem dadger aberto e carregá-lo
            var info = ActiveWorkbook.GetInfosheet();
            if (info == null || !(
                info.DocType.Equals("dadger", StringComparison.OrdinalIgnoreCase) ||
                info.DocType.Equals("confhddat", StringComparison.OrdinalIgnoreCase)
                )) {
                throw new Exception("Nenhum dadger ou confdh carregado.");
            }

            var fileName = info.DocPath;
            var type = info.DocType;
            var doc = ActiveWorkbook.LoadDocumentFromWorkbook((string)type);

            //procurar dados hidr.dat
            //// se não existir na mesma pasta, pedir para usuário informar
            var deckFolder = System.IO.Path.GetDirectoryName(fileName);
            var hidrFile = System.IO.Path.Combine(deckFolder, "hidr.dat");
            var modifFile = System.IO.Path.Combine(deckFolder, "modif.dat");

            if (!System.IO.File.Exists(hidrFile)) {
                hidrFile = System.IO.Path.Combine(Globals.ThisAddIn.ResourcesPath, "hidr.dat");
                System.Windows.Forms.MessageBox.Show("Hidr.dat não encontrado. Usando arquivo padrão.\r\n" + hidrFile);
            }

            var hidr = (Compass.CommomLibrary.HidrDat.HidrDat)DocumentFactory.Create(hidrFile);
            this.hidr = hidr;



            if (doc is Dadger) {
                return new Compass.CommomLibrary.Decomp.ConfigH((Dadger)doc, hidr);
            } else if (doc is Compass.CommomLibrary.ConfhdDat.ConfhdDat) {
                var modif = BaseDocument.Create<Compass.CommomLibrary.ModifDatNW.ModifDatNw>(
                File.ReadAllText(modifFile)
                );
                return new Compass.CommomLibrary.Decomp.ConfigH((Compass.CommomLibrary.ConfhdDat.ConfhdDat)doc, hidr, modif);
            } else
                throw new Exception("Documento inválido");
        }
    }
}


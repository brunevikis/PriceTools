using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using Compass.CommomLibrary.SistemaDat;
using Compass.ExcelTools;
using DecompTools.Properties;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Compass.ExcelTools.Templates;

namespace Compass.DecompTools
{
    public partial class Ribbon1
    {

        public Microsoft.Office.Interop.Excel.Workbook ActiveWorkbook
        {
            get
            {
                return Globals.ThisAddIn.Application.ActiveWorkbook;
            }

        }

        private void btnOpen_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                var xlApp = Globals.ThisAddIn.Application;
                xlApp.Cursor = XlMousePointer.xlWait;

                try
                {
                    var doc = DocumentFactory.Create(ofd.FileName);

                    xlApp.ScreenUpdating = false;

                    var xlWb = xlApp.Workbooks.Add();

                    var info = xlWb.SetInfosheet(doc);

                    xlWb.WriteDocumentToWorkbook(doc);

                    info.BottonComments = doc.BottonComments;

                    Globals.ThisAddIn.ReloadMenus(xlWb);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    xlApp.Cursor = XlMousePointer.xlDefault;
                    xlApp.ScreenUpdating = true;
                }
            }
        }

        private void btnOpenCompare_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                var xlApp = Globals.ThisAddIn.Application;
                xlApp.SheetsInNewWorkbook = 1;

                try
                {

                    var doc = DocumentFactory.Create(ofd.FileName);

                    xlApp.ScreenUpdating = false;

                    var xlWb = xlApp.ActiveWorkbook;

                    //xlWb.SetInfosheet(doc);

                    //xlWb.WriteDocumentToWorkbook(doc);
                    xlWb.WriteDocumentToWorkbook(doc, true);

                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    xlApp.ScreenUpdating = true;
                }
            }
        }

        private void btnSave_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {

                var xlWB = Globals.ThisAddIn.Application.ActiveWorkbook;

                if (xlWB == null)
                {

                    System.Windows.Forms.MessageBox.Show("no workbook active");
                    return;
                }


                var info = xlWB.GetInfosheet();

                if (info == null)
                {
                    System.Windows.Forms.MessageBox.Show("info worksheet not found");
                    return;
                }

                var fileName = info.DocPath;
                var type = info.DocType;


                if (String.IsNullOrWhiteSpace(type))
                {
                    System.Windows.Forms.MessageBox.Show("undefined doc type");
                    return;
                }


                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                if (!String.IsNullOrWhiteSpace(fileName))
                {
                    sf.FileName = fileName;
                }


                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var doc = xlWB.LoadDocumentFromWorkbook((string)type);

                    doc.File = sf.FileName;
                    doc.BottonComments = info.BottonComments;

                    info.DocPath = doc.File;

                    doc.SaveToFile();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            btnOpen.SuperTip =
                @"Dadger.Rvx
Dadgnl.Rvx
Prevs.Rvx
Sistema.Dat
EafPast.Dat
VazPast.Dat
VazoesC.Dat
Hidr.Dat
Postos.Dat
Mlt.Dat
Modif.Dat
Relato.Rvx
IPDO*.Txt
";
        }

        private void btnRvxSave_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void btnRvxNew_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var xlApp = Globals.ThisAddIn.Application;
                Microsoft.Office.Interop.Excel.Workbook xlWb;
                //New Workbook? 
                var newWb = true;
                var diagRes = System.Windows.Forms.MessageBox.Show("Abrir em nova planilha?", "Novo Rvx + 1", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                if (diagRes == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                else if (diagRes == System.Windows.Forms.DialogResult.No)
                {
                    newWb = false;
                    //xlWb = xlApp.ActiveWorkbook;
                }
                else
                {
                    newWb = true;
                    //xlWb = xlApp.Workbooks.Add();
                }

                //Case Name            
                var newCaseForm = new Forms.FormNewRvxPlus1();
                if (newCaseForm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                var caseName = newCaseForm.Case;

                if (newWb)
                    xlWb = xlApp.Workbooks.Add();
                else
                    xlWb = xlApp.ActiveWorkbook;

                var configWs = xlWb.SetRvxPlus1Configsheet(caseName);

                System.Windows.Forms.FolderBrowserDialog folderSelect = new System.Windows.Forms.FolderBrowserDialog();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveBlock_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {
                var xlWB = Globals.ThisAddIn.Application.ActiveWorkbook;
                
                if (xlWB == null)
                {
                    System.Windows.Forms.MessageBox.Show("no workbook active");
                    return;
                }

                var info = xlWB.GetInfosheet();

                if (info == null)
                {
                    System.Windows.Forms.MessageBox.Show("info worksheet not found");
                    return;
                }

                var fileName = info.DocPath;
                var type = info.DocType;

                if (String.IsNullOrWhiteSpace(type))
                {
                    System.Windows.Forms.MessageBox.Show("undefined doc type");
                    return;
                }


                var xlWs = (Microsoft.Office.Interop.Excel.Worksheet)xlWB.ActiveSheet;

                fileName += "_" + xlWs.Name;

                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                if (!String.IsNullOrWhiteSpace(fileName))
                {
                    sf.FileName = fileName;
                }


                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var doc = xlWB.LoadDocumentFromWorkbook((string)type, xlWs.Name);

                    doc.File = sf.FileName;
                    doc.SaveToFile();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnRdh_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "RDH | *.xlsx";
            string rdhFolder = System.Configuration.ConfigurationManager.AppSettings["rdhPath"];
            ofd.InitialDirectory = rdhFolder;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                var xlApp = Globals.ThisAddIn.Application;

                try
                {
                    xlApp.ScreenUpdating = false;

                    xlApp.AskToUpdateLinks = false;

                    var xlWbRdh = xlApp.Workbooks.Open(ofd.FileName, ReadOnly: true);

                    var rdh = new WorkbookRdh(xlWbRdh);

                    xlWbRdh.Close(SaveChanges: false);

                    var xlWb = xlApp.Workbooks.Add();


                    var ws = xlWb.Worksheets[1];

                    var r = 2;

                    ws.Cells[1, 1].Value = "Aproveitamento";
                    ws.Cells[1, 2].Value = "Posto";
                    ws.Cells[1, 3].Value = "VazaoMesAnterior";
                    ws.Cells[1, 4].Value = "VazaoMes ";
                    ws.Cells[1, 5].Value = "VazaoSemana ";
                    ws.Cells[1, 6].Value = "VazaoUltDias";
                    ws.Cells[1, 7].Value = "Vazao";
                    ws.Cells[1, 8].Value = "Nivel";
                    ws.Cells[1, 9].Value = "VolUtilArm";
                    ws.Cells[1, 10].Value = "VolEsp ";


                    foreach (var item in rdh.RelatorioHidraulico)
                    {

                        ws.Cells[r, 1].Value = item.Aproveitamento;
                        ws.Cells[r, 2].Value = item.Posto;
                        ws.Cells[r, 3].Value = item.VazaoMesAnterior;
                        ws.Cells[r, 4].Value = item.VazaoMes;
                        ws.Cells[r, 5].Value = item.VazaoSemana;
                        ws.Cells[r, 6].Value = item.VazaoUltDias;
                        ws.Cells[r, 7].Value = item.Vazao;
                        ws.Cells[r, 8].Value = item.Nivel;
                        ws.Cells[r, 9].Value = item.VolUtilArm;
                        ws.Cells[r, 10].Value = item.VolEsp;

                        r++;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    xlApp.ScreenUpdating = true;
                    xlApp.AskToUpdateLinks = true;
                }
            }
        }

        private void btnEncadeadoXml_Click(object sender, RibbonControlEventArgs e)
        {
            var tfile = "";

            var xlApp = Globals.ThisAddIn.Application;
            try
            {

                //tfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "Gera_e_Avalia_Cenarios.xltm");
                tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "gerador_xml_REE.xltm");
                //Directory.CreateDirectory(Path.GetDirectoryName(tfile));

                //File.WriteAllBytes(tfile, t1);

                var wb = xlApp.Workbooks.Add(tfile);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void btnOpenIpdo_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new DecompTools.Forms.FormRange();

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var xlApp = Globals.ThisAddIn.Application;
                try
                {

                    xlApp.ScreenUpdating = false;

                    var xlWb = xlApp.Workbooks.Add();

                    var xlWs = (Microsoft.Office.Interop.Excel.Worksheet)xlWb.Worksheets[1];

                    xlWs.Cells[1, 1].Value = "ENA";
                    xlWs.Cells[2, 1].Value = "SE";
                    xlWs.Cells[3, 1].Value = "S";
                    xlWs.Cells[4, 1].Value = "NE";
                    xlWs.Cells[5, 1].Value = "N";

                    xlWs.Cells[7, 1].Value = "ENA % MLT";
                    xlWs.Cells[8, 1].Value = "SE - BRUTA";
                    xlWs.Cells[9, 1].Value = "SE - ARMAZ";
                    xlWs.Cells[10, 1].Value = "S  - BRUTA";
                    xlWs.Cells[11, 1].Value = "S  - ARMAZ";
                    xlWs.Cells[12, 1].Value = "NE - BRUTA";
                    xlWs.Cells[13, 1].Value = "NE - ARMAZ";
                    xlWs.Cells[14, 1].Value = "N  - BRUTA";
                    xlWs.Cells[15, 1].Value = "N  - ARMAZ";

                    xlWs.Cells[17, 1].Value = "EARM % MLT";
                    xlWs.Cells[18, 1].Value = "SE";
                    xlWs.Cells[19, 1].Value = "S";
                    xlWs.Cells[20, 1].Value = "NE";
                    xlWs.Cells[21, 1].Value = "N";

                    xlWs.Cells[23, 1].Value = "Carga";
                    xlWs.Cells[24, 1].Value = "SE";
                    xlWs.Cells[25, 1].Value = "S";
                    xlWs.Cells[26, 1].Value = "NE";
                    xlWs.Cells[27, 1].Value = "N";






                    string ipdoFolder = System.Configuration.ConfigurationManager.AppSettings["ipdoPath"];
                    Func<DateTime, string> GetFilePath = (DateTime baseDate) =>
                    {
                        return System.IO.Path.Combine(ipdoFolder, baseDate.ToString("MM_yyyy"), "IPDO-" + baseDate.ToString("dd-MM-yyyy") + ".txt");
                    };

                    var dtIni = frm.Inicio;
                    var dtFim = frm.Fim;




                    var ipdos = new Dictionary<DateTime, Compass.CommomLibrary.Ipdo.Ipdo>();
                    for (DateTime date = dtIni; date <= dtFim; date = date.AddDays(1))
                    {
                        var filePath = GetFilePath(date);
                        if (System.IO.File.Exists(filePath))
                        {
                            var ipdo = (Compass.CommomLibrary.Ipdo.Ipdo)DocumentFactory.Create(filePath);
                            ipdos.Add(date, ipdo);
                        }
                    }

                    int c = 2;

                    for (DateTime date = dtIni; date <= dtFim; date = date.AddDays(1), c++)
                    {

                        if (ipdos.ContainsKey(date))
                        {
                            var ipdo = ipdos[date];

                            xlWs.Cells[1, c].Value = date;

                            int r = 2;
                            foreach (var mercado in ipdo.BalancoDetalhado)
                            {

                                xlWs.Cells[r++, c].Value = mercado.Ena;

                            }

                            r = 8;
                            foreach (var mercado in ipdo.BalancoDetalhado)
                            {

                                xlWs.Cells[r++, c].Value = mercado.Bruta;
                                xlWs.Cells[r++, c].Value = mercado.Armazenavel;
                            }

                            r = 18;
                            foreach (var mercado in ipdo.Energia)
                            {

                                xlWs.Cells[r++, c].Value = mercado.EarmMlt;

                            }

                            r = 24;
                            foreach (var mercado in ipdo.BalancoDetalhado)
                            {

                                xlWs.Cells[r++, c].Value = mercado.Carga;

                            }
                        }
                        else
                        {
                            xlWs.Cells[1, c].Value = date;
                            ((Range)xlWs.Cells[1, c]).Interior.Color = ExcelExtensions.errorColor;
                        }
                    }

                    var neqC = new NameEqualityComparer();

                    var usinas = ipdos.SelectMany(x => x.Value.Blocos["Ger Termica"]).Select(x => (string)x[0].ToString()).Distinct(neqC).ToList();



                    xlWs = (Microsoft.Office.Interop.Excel.Worksheet)xlWb.Worksheets.Add(After: xlWs);
                    xlWs.Name = "GTermProg";
                    {
                        var r = 2;
                        //foreach (var item in ipdos.First().Value.Blocos["Ger Termica"]) {
                        //    xlWs.Cells[r++, 1].Value = item.Valores[0];
                        //}
                        foreach (var usina in usinas)
                        {
                            xlWs.Cells[r++, 1].Value = usina;
                        }
                    }
                    c = 2;
                    for (DateTime date = dtIni; date <= dtFim; date = date.AddDays(1), c++)
                    {

                        if (ipdos.ContainsKey(date))
                        {
                            var ipdo = ipdos[date];

                            var b = ipdo.Blocos["Ger Termica"];

                            xlWs.Cells[1, c].Value = date;
                            var r = 2;
                            //foreach (var item in b) {
                            //    xlWs.Cells[r++, c].Value = item.Valores[4];
                            //}

                            foreach (var usina in usinas)
                            {
                                xlWs.Cells[r++, c].Value = b.Where(x => neqC.Equals(x[0], usina)).Select(x => x.Valores[4]).FirstOrDefault();
                            }

                            //4

                        }
                        else
                        {
                            xlWs.Cells[1, c].Value = date;
                            ((Range)xlWs.Cells[1, c]).Interior.Color = ExcelExtensions.errorColor;
                        }
                    }





                    xlWs = (Microsoft.Office.Interop.Excel.Worksheet)xlWb.Worksheets.Add(After: xlWs);
                    xlWs.Name = "GTermVerif";
                    {

                        var r = 2;
                        //foreach (var item in ipdos.First().Value.Blocos["Ger Termica"]) {
                        //    xlWs.Cells[r++, 1].Value = item.Valores[0];
                        //}
                        foreach (var usina in usinas)
                        {
                            xlWs.Cells[r++, 1].Value = usina;
                        }
                    }
                    c = 2;
                    for (DateTime date = dtIni; date <= dtFim; date = date.AddDays(1), c++)
                    {

                        if (ipdos.ContainsKey(date))
                        {
                            var ipdo = ipdos[date];

                            var b = ipdo.Blocos["Ger Termica"];

                            xlWs.Cells[1, c].Value = date;
                            var r = 2;
                            //foreach (var item in b) {
                            //    xlWs.Cells[r++, c].Value = item.Valores[5];
                            //}

                            foreach (var usina in usinas)
                            {
                                xlWs.Cells[r++, c].Value = b.Where(x => neqC.Equals(x[0], usina)).Select(x => x.Valores[5]).FirstOrDefault();
                            }

                            //4

                        }
                        else
                        {
                            xlWs.Cells[1, c].Value = date;
                            ((Range)xlWs.Cells[1, c]).Interior.Color = ExcelExtensions.errorColor;
                        }
                    }


                    //5


                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    xlApp.ScreenUpdating = true;
                }
            }
        }

        public class NameEqualityComparer : IEqualityComparer<string>
        {

            public static string normalize(string obj)
            {
                return obj.Replace(" ", "")
                    .Replace("1", "I")
                    .Replace("2", "II")
                    .Replace("3", "III")
                    .Replace("4", "IV")
                    .Replace("5", "V")
                    .Replace(".", "")
                    .Replace("-", "")
                    .Replace("/", "")
                    .Replace("\\", "")
                    .ToUpperInvariant()
                    .Replace("Á", "A")
                    .Replace("À", "A")
                    .Replace("Ã", "A")
                    .Replace("É", "E")
                    .Replace("Í", "I")
                    .Replace("Ó", "O")
                    .Replace("Õ", "O")
                    .Replace("Ú", "U")
                    .Replace("Ü", "U")

                    ;
            }


            public bool Equals(string x, string y)
            {
                return normalize(x).Equals(normalize(y));
            }

            public int GetHashCode(string obj)
            {
                return normalize(obj).GetHashCode();
            }
        }

        private void btnConfig_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new Forms.FormConfig();

            frm.ShowDialog();
        }

        private void btnNwGterm_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {

                var updateGTMin = true;

                Globals.ThisAddIn.Application.ScreenUpdating = false;

                System.Windows.Forms.MessageBox.Show("Selecione o deck");
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();


                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {


                    updateGTMin = System.Windows.Forms.MessageBox.Show("Atualizar dados de Geração Termica Mínima?", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes;

                    var path = Path.GetDirectoryName(ofd.FileName);

                    Compass.CommomLibrary.Newave.Deck deck = new CommomLibrary.Newave.Deck();
                    deck.GetFiles(path);

                    var dger = (Compass.CommomLibrary.DgerDat.DgerDat)DocumentFactory.Create(deck.Documents["DGER.DAT"].BasePath);
                    var expt = (Compass.CommomLibrary.ExptDat.ExptDat)DocumentFactory.Create(deck.Documents["EXPT.DAT"].BasePath);
                    var term = (Compass.CommomLibrary.TermDat.TermDat)DocumentFactory.Create(deck.Documents["TERM.DAT"].BasePath);
                    var manut = (Compass.CommomLibrary.ManuttDat.ManuttDat)DocumentFactory.Create(deck.Documents["MANUTT.DAT"].BasePath);
                    var conft = (Compass.CommomLibrary.ConftDat.ConftDat)DocumentFactory.Create(deck.Documents["CONFT.DAT"].BasePath);


                    var exptList = expt.ToList();

                    var col = 2;
                    var r = 2;

                    var wb = Globals.ThisAddIn.Application.Workbooks.Add();
                    var ws = wb.Worksheets[1];


                    ws.Cells[1, 3 + 13 - dger.MesEstudo].Value = "gtmax";
                    ws.Cells[1, 3].Value = "gtmin";

                    //DateTime date = new DateTime(dger.AnoEstudo, dger.MesEstudo, 1);
                    //                    var utes = term.Select(t => new { t, expt = expt.Where(ex => ex.Cod == t.Cod).ToArray(), manut = manut.Blocos["Indisp"].Where(x => ((Compass.CommomLibrary.ManuttDat.IndispLine)x).Cod == t.Cod).ToArray() });

                    foreach (var ute in conft.Where(x => x.Existente != "NC"))
                    {

                        ws.Cells[r, 1].Value = ute.Num;
                        ws.Cells[r, 2].Value = ute.Nome;

                        col = 3;

                        var uteTerm = term.Where(x => x.Cod == ute.Num).First();

                        for (DateTime date = new DateTime(dger.AnoEstudo, dger.MesEstudo, 1); date < new DateTime(dger.AnoEstudo + dger.AnosManutencaoUTE, 1, 1); date = date.AddMonths(1))
                        {

                            var uteExpansoes = expt.Where(x => x.Cod == ute.Num)
                                   .Where(ex => ex.DataInicio <= date && ex.DataFim >= date);


                            double pot;
                            double gtmin;
                            double ipter;



                            double teif = uteTerm.Teif;
                            double fcmx = uteTerm.Fcmx;

                            if (ute.Existente == "EX")
                            {
                                pot = uteTerm.Potencia;
                                gtmin = date.Month <= 12 && date.Year == dger.AnoEstudo ? uteTerm[date.Month + 5] : ute[18];
                                ipter = 0;// uteTerm.Ipter; //ipterm 0 no primeiro ano, depente de manutt
                            }
                            else
                            { // NE|EE
                                pot = 0;
                                gtmin = 0;
                                ipter = 0; //uteTerm.Ipter; //ipterm 0 no primeiro ano, depente de manutt ou expt
                            }

                            var uteManutt = manut.Blocos["Indisp"].Where(x => ((Compass.CommomLibrary.ManuttDat.IndispLine)x).Cod == ute.Num);
                            double potManu = uteManutt.Where(x => x[1] == date.Month && x[2] == date.Year).Sum(x => (double)x[4]);

                            //Tratar expansões
                            if (ute.Existente != "EX")
                            {
                                if (uteExpansoes.Any(x => x.Tipo == "POTEF")) pot = uteExpansoes.First(x => x.Tipo == "POTEF").Valor;
                                if (uteExpansoes.Any(x => x.Tipo == "TEIFT")) teif = uteExpansoes.First(x => x.Tipo == "TEIFT").Valor;
                                if (uteExpansoes.Any(x => x.Tipo == "FCMAX")) fcmx = uteExpansoes.First(x => x.Tipo == "FCMAX").Valor;
                                if (uteExpansoes.Any(x => x.Tipo == "GTMIN")) gtmin = uteExpansoes.First(x => x.Tipo == "GTMIN").Valor;
                            }

                            ipter = Math.Round((pot != 0 && potManu != 0 ? potManu / pot * 100 : ipter), 2);
                            if (ute.Existente != "EX" && uteExpansoes.Any(x => x.Tipo == "IPTER")) ipter = uteExpansoes.First(x => x.Tipo == "IPTER").Valor;

                            var gtmax = Math.Round(pot * (fcmx / 100) * (1 - teif / 100) * (1 - ipter / 100), 2);


                            if (updateGTMin && gtmin > gtmax)
                            {

                                gtmin = gtmax;

                                if (ute.Existente == "EX")
                                {
                                    uteTerm[date.Month + 5] = gtmin;
                                }
                                else
                                {
                                    var exps = uteExpansoes.Where(x => x.Tipo == "GTMIN").FirstOrDefault();

                                    if (exps != null)
                                    {

                                        if (exps.DataInicio < date)
                                        {

                                            var c1 = (Compass.CommomLibrary.ExptDat.ExptLine)exps.Clone();
                                            c1.DataFim = date.AddMonths(-1);

                                            //var i = expt.Blocos["Expt"].IndexOf(exps);
                                            //expt.Blocos["Expt"].Insert(i, c1);
                                            expt.Blocos["Expt"].Add(c1);
                                        }

                                        if (exps.DataFim > date)
                                        {
                                            var c2 = (Compass.CommomLibrary.ExptDat.ExptLine)exps.Clone();
                                            c2.DataInicio = date.AddMonths(1);

                                            //var i = expt.Blocos["Expt"].IndexOf(exps);
                                            //expt.Blocos["Expt"].Insert(i + 1, c2);
                                            expt.Blocos["Expt"].Add(c2);
                                        }

                                        exps.DataInicio = exps.DataFim = date;
                                        exps.Valor = gtmin;

                                    }
                                    else
                                    {
                                        expt.Blocos["Expt"].Add(
                                            new Compass.CommomLibrary.ExptDat.ExptLine()
                                            {
                                                Cod = ute.Num,
                                                Tipo = "GTMIN",
                                                DataInicio = date,
                                                DataFim = date,
                                                Valor = gtmin
                                            });
                                    }


                                }




                            }


                            ws.Cells[r, col + 13 - dger.MesEstudo].Value = gtmax;
                            ws.Cells[r, col++].Value = gtmin;
                        }
                        r++;
                    }


                    if (updateGTMin)
                    {

                        foreach (Compass.CommomLibrary.ManuttDat.ManuttLine m in manut.Blocos["Manutt"].ToList())
                        {

                            if (!conft.Any(x => x.Num == m.Cod))
                            {
                                ((IList<Compass.CommomLibrary.ManuttDat.ManuttLine>)manut.Blocos["Manutt"]).Remove(m);
                            }

                        }

                        manut.SaveToFile(createBackup: true);
                        expt.SaveToFile(createBackup: true);
                        term.SaveToFile(createBackup: true);

                    }

                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally { Globals.ThisAddIn.Application.ScreenUpdating = true; }
        }

        private void btnNwColetaDados_Click(object sender, RibbonControlEventArgs e)
        {
            var tfile = "";

            var xlApp = Globals.ThisAddIn.Application;
            try
            {
                tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "Coleta_Dados.xlsm");
                var wb = xlApp.Workbooks.Open(tfile, ReadOnly: true);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void btnEncadeadoVazpastXml_Click(object sender, RibbonControlEventArgs e)
        {
            var tfile = "";

            var xlApp = Globals.ThisAddIn.Application;
            try
            {
                //tfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "Gera_e_Avalia_Cenarios.xltm");
                tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "gerador_xml_REE_VAZPAST.xltm");
                //Directory.CreateDirectory(Path.GetDirectoryName(tfile));

                //File.WriteAllBytes(tfile, t1);

                var wb = xlApp.Workbooks.Open(tfile, ReadOnly: true);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void btnDecompSensibilidade_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var tfile = "";


                WorkbookSensibilidade w;
                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                    !WorkbookSensibilidade.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
                {

                    tfile = Path.Combine(Globals.ThisAddIn.ResourcesPath, "SensibilidadeDecomp.xltx");
                    Globals.ThisAddIn.Application.Workbooks.Add(tfile);

                    return;
                }
                else if (System.Windows.Forms.MessageBox.Show("Criar e gravar cenários?", "Decomp Tool - Sensibilidade", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }



                var cgf = new Compass.Services.CenariosCP.CenariosConfig();
                cgf.DeckDeEntrada = w.DeckEntrada;
                cgf.Sensibilidades = new Services.CenariosCP.Sensibilidade[3];
                cgf.Sensibilidades[0] = new Services.CenariosCP.Sensibilidade()
                {
                    NumeroDePassos = w.NumeroPassosCarga,
                    Passo = w.PassosCarga,
                    Tipo = Services.CenariosCP.TipoDeSensibilidade.Carga
                };

                cgf.Sensibilidades[1] = new Services.CenariosCP.Sensibilidade()
                {
                    NumeroDePassos = w.NumeroPassosEamrI,
                    Passo = w.PassosEamrI,
                    Tipo = Services.CenariosCP.TipoDeSensibilidade.EarmI
                };

                cgf.Sensibilidades[2] = new Services.CenariosCP.Sensibilidade()
                {
                    NumeroDePassos = w.NumeroPassosVazoes,
                    Passo = w.PassosVazoes,
                    Tipo = Services.CenariosCP.TipoDeSensibilidade.Vazoes
                };

                cgf.RodarVazoes = w.RodarVazoes;
                cgf.CaminhoCortes = w.CaminhoCortes;
                cgf.DiretorioSaida = w.DiretorioSaida;

                var cenarios = Compass.Services.CenariosCP.ProcessarCriarCenarios(cgf);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void btnDecompSensibilidadeColeta_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                WorkbookSensibilidade w;
                if (Globals.ThisAddIn.Application.ActiveWorkbook == null ||
                    !WorkbookSensibilidade.TryCreate(Globals.ThisAddIn.Application.ActiveWorkbook, out w))
                {
                    System.Windows.Forms.MessageBox.Show("Planilha de Sensibilidade Inválida");
                    return;
                }

                var pathRoot = w.DiretorioSaida;

                foreach (var cen in Directory.GetDirectories(pathRoot))
                {

                    var path = cen;
                    var cenName = (new DirectoryInfo(cen)).Name;

                    var relatos = Directory.GetFiles(path, "relato.*");

                    if (relatos.Length > 0)
                    {
                        var relato = (Compass.CommomLibrary.Relato.Relato)DocumentFactory.Create(relatos[0]);
                        w.WriteCMO(cenName, relato.CMO);
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

        private void btnNovoEncadeado_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {

                var workbook = this.ActiveWorkbook;



                var worksheet = workbook.ActiveSheet as Worksheet;

                var r = worksheet.Range["A1", "F50"];               

                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(obj => {
                    if (obj is Range range)
                    {
                        range.Copy();
                        if (System.Windows.Forms.Clipboard.ContainsImage())
                        {
                            var img = System.Windows.Forms.Clipboard.GetImage();
                            img.Save(@"C:\temp\" + "RANGE" + "__clipboard.gif");
                        }
                    }                  

                }));
                thread.SetApartmentState(System.Threading.ApartmentState.STA);
                thread.Start(r);
                thread.Join();

                


                foreach (ChartObject chart in worksheet.ChartObjects())
                {
                    System.Windows.Forms.Clipboard.Clear();
                    //chart.Chart.Export(@"C:\temp\" + chart.Name + "__export.gif");

                    chart.CopyPicture(XlPictureAppearance.xlScreen, XlCopyPictureFormat.xlBitmap);

                    if (System.Windows.Forms.Clipboard.ContainsImage())
                    {
                        var img = System.Windows.Forms.Clipboard.GetImage();
                        img.Save(@"C:\temp\" + chart.Name + "__clipboard.gif");
                    }
                    System.Windows.Forms.Clipboard.Clear();
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

    }
}

using Compass.CommomLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX {
    public partial class FormViewer : Form {

        Dictionary<string, Result> _results = new Dictionary<string, Result>();

        public FormViewer(string caption) {
            InitializeComponent();
            this.Text = caption;
        }

        public void AddInfo(string title, ResultDataSource dataSource) {

            this.tabControl1.Controls.Add(
                new InfoTabPage() {
                    Title = title,
                    DataSource = dataSource
                });
        }

        public void ClearInfo() {
            this.tabControl1.Controls.Clear();
        }

        //public static void Show(String caption, Dictionary<string, object> results) {
        //    Show(caption, results.Keys.ToArray(), results.Values.ToArray());
        //}

        //public static void Show(String caption, string[] titles, object[] dataSources) {

        //    var frm = new FormViewer(caption);


        //    for (int i = 0; i < dataSources.Length; i++) {

        //        var t = i < titles.Length ? titles[i] : "info " + i.ToString();

        //        frm.AddInfo(t, dataSources[i]);
        //    }

        //    if (System.Threading.Thread.CurrentThread.GetApartmentState() != ApartmentState.STA) {

        //        Thread thread = new Thread(() => frm.ShowDialog());
        //        thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
        //        thread.Start();
        //        thread.Join(); //Wait for the thread to end

        //    } else {
        //        frm.ShowDialog();
        //    }



        //}

        public static void Show(String caption, params Result[] results) {
            var frm = new FormViewer(caption);

            foreach (var res in results) {
                frm._results[res.Dir] = res;
            }

            frm.RefreshView();

            if (System.Threading.Thread.CurrentThread.GetApartmentState() != ApartmentState.STA) {

                Thread thread = new Thread(() => frm.ShowDialog());
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                //thread.Join(); //Wait for the thread to end

            } else {
                frm.ShowDialog();
            }

        }

        internal static void Show(string caption, ResultDataSource resultDataSource) {
            var frm = new FormViewer(caption);



            frm.ShowInfo(resultDataSource);

            if (System.Threading.Thread.CurrentThread.GetApartmentState() != ApartmentState.STA) {

                Thread thread = new Thread(() => frm.ShowDialog());
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                //thread.Join(); //Wait for the thread to end

            } else {
                frm.ShowDialog();
            }
        }

        private void ShowInfo(params ResultDataSource[] dataSources) {
            ClearInfo();

            for (int i = 0; i < dataSources.Length; i++) {

                var t = !string.IsNullOrWhiteSpace(dataSources[i].Title) ? dataSources[i].Title : "info " + i.ToString();

                AddInfo(t, dataSources[i]);
            }
        }

        private void FormViewer_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) {
                e.Effect = DragDropEffects.All;
            }

        }

        private void FormViewer_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


            bool refreshView = false;
            if (files != null && files.Length != 0) {

                foreach (var file in files) {

                    string dir;

                    if (System.IO.Directory.Exists(file)) {
                        dir = file;
                    } else {
                        dir = System.IO.Path.GetDirectoryName(file);
                    }
                    var deck = DeckFactory.CreateDeck(dir);

                    if (deck is CommomLibrary.Newave.Deck || deck is CommomLibrary.Decomp.Deck) {
                        var results = deck.GetResults();
                        _results[dir] = results;
                        refreshView = true;
                    } else continue;
                }
            }

            if (refreshView) {
                RefreshView();
            }
        }

        private void RefreshView() {
            this.Cursor = Cursors.WaitCursor;

            _results.Remove("");

            if (_results.Keys.Count > 1) {
                RefreshViewMultiple();
            } else if (_results.Keys.Count == 1) {
                RefreshViewSingle();
            }

            this.Cursor = Cursors.Default;
        }

        private void RefreshViewMultiple() {

            var commonPath = GetCommonPath(_results.Select(x => x.Value.Dir).ToArray());
            var commonCortesPath = GetCommonPath(_results.Select(x => x.Value.Cortes).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());

            var orderedResults = _results.Select(x => x.Value).OrderBy(x => GetOrder(x.Dir)).ToList();

            var dtCmo = new DataTable();
            var dtCmoDet = new DataTable();
            var dtEarm = new DataTable();
            var dtENAcv = new DataTable();
            var dtENA = new DataTable();
            var dtENAp = new DataTable();
            var dtENATH = new DataTable();
            var dtENATHp = new DataTable();
            var dtDemanda = new DataTable();
            var dtCortes = new DataTable();
            var dtGerHidr = new DataTable();
            var dtGerTerm = new DataTable();
            var dtGNL = new DataTable();

            var dataSources = new ResultDataSource[] { 
             new ResultDataSource(){ DataSource =  dtCmo   , Title = "Cmo" },
             new ResultDataSource(){ DataSource =  dtCmoDet, Title = "Cmo - Detalhe" },
             new ResultDataSource(){ DataSource =  dtEarm  , Title = "Earm ini" },
             new ResultDataSource(){ DataSource =  dtENAcv , Title = "ENA CV" },
             new ResultDataSource(){ DataSource =  dtENA   , Title = "ENA" },
             new ResultDataSource(){ DataSource =  dtENAp  , Title = "ENA %" },
             new ResultDataSource(){ DataSource =  dtENATH , Title = "ENA TH" },
             new ResultDataSource(){ DataSource =  dtENATHp, Title = "ENA TH %" },
             new ResultDataSource(){ DataSource =  dtDemanda , Title = "Demanda" },
             new ResultDataSource(){ DataSource =  dtGerHidr , Title = "Ger Hidr" },
             new ResultDataSource(){ DataSource =  dtGerTerm , Title = "Ger Term" },
             new ResultDataSource(){ DataSource =  dtCortes, Title = "Cortes" },
              new ResultDataSource(){ DataSource =  dtGNL, Title = "Despacho GNL" }

            };


            var dirNames = orderedResults.Select(x => x.Tipo + ": " + x.Dir.Remove(0, commonPath.Length)).ToList();
            dirNames.Insert(0, "");

            foreach (var d in dataSources.Select(x => x.DataSource as DataTable)) {

                if (d == dtCortes) {
                    dtCortes.Columns.Add("Caso");
                    dtCortes.Columns.Add("Cortes");
                }
                else if (d == dtGNL)
                {
                    dtGNL.Columns.Add("Posto");
                    dtGNL.Columns.Add("Semana");
                    dtGNL.Columns.Add("Patamar");

                    foreach (var r in orderedResults) d.Columns.Add();

                }
                else {
                    d.Columns.Add("MERCADO");
                    if (d == dtCmoDet) d.Columns.Add("PATAMAR");

                    foreach (var r in orderedResults) d.Columns.Add();
                }

                if (d == dtCmoDet) d.Rows.Add((new string[] { "" }).Concat(dirNames).ToArray());
                else if (d == dtGNL) {
                    d.Rows.Add((new string[] { "","" }).Concat(dirNames).ToArray());
                }
                else if (d == dtCortes) { } else d.Rows.Add(dirNames.ToArray());
            }


            foreach (var item in orderedResults) dtCortes.Rows.Add(item.Dir.Replace(commonPath, ""), item.Cortes.Replace(commonCortesPath, ""));

            int num=0;
            List<string> l1;
            int[] usinas = new int[] { 15, 86, 224 };
            foreach (var item in orderedResults)
            {
                num = item.GNL_Result.Count();
            }
           
            for (int i = 0; i < num; i++)
            {
            
                int semana = orderedResults.Select(x => x.GNL_Result[i].semana).Last();
                int usi= orderedResults.Select(x => x.GNL_Result[i].Posto).Last();

                l1 = orderedResults.Select(x => x.GNL_Result[i].GNL_pat1.ToString("N2")).ToList();
                l1.Insert(0, Convert.ToString(usi));
                l1.Insert(1, Convert.ToString(semana));
                l1.Insert(2, "1");
                dtGNL.Rows.Add(l1.ToArray());

                l1 = orderedResults.Select(x => x.GNL_Result[i].GNL_pat2.ToString("N2")).ToList();
                l1.Insert(0, Convert.ToString(usi));
                l1.Insert(1, Convert.ToString(semana));
                l1.Insert(2, "2");
                dtGNL.Rows.Add(l1.ToArray());

                l1 = orderedResults.Select(x => x.GNL_Result[i].GNL_pat3.ToString("N2")).ToList();
                l1.Insert(0, Convert.ToString(usi));
                l1.Insert(1, Convert.ToString(semana));
                l1.Insert(2, "3");
                dtGNL.Rows.Add(l1.ToArray());
            }
      
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].Cmo.ToString("N2")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtCmo.Rows.Add(l1.ToArray());

               


            }
            //Resultados PLD Mensal 
            try
            {
                var teste = orderedResults.Select(x => x.CMO_Mensal_Result.Count()).ToList();
                var ver = teste.Where(x => x != 0);//Verifica se existe PLD Mensal
                if (ver.Count() != 0)
                {
                    List<string> l2 = new List<string>(orderedResults.Count());
                    dtCmo.Rows.Add();
                    dtCmo.Rows.Add("Média Mensal");
                    for (int i = 1; i <= 4; i++)
                    {
                        l2.Insert(0, Convert.ToString(i));
                        foreach (var item in orderedResults)
                        {
                            l2.Add(item.CMO_Mensal_Result.Where(x => x.submercado == i).Sum(x => x.CMO_Men).ToString());
                        }
                        dtCmo.Rows.Add(l2.ToArray());
                        l2.Clear();
                    }
                }
                  
                
            }
            catch { }

            

            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].Cmo_pat1.ToString("N2")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i)); l1.Insert(1, "1");
                dtCmoDet.Rows.Add(l1.ToArray());
                l1 = orderedResults.Select(x => x[i].Cmo_pat2.ToString("N2")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i)); l1.Insert(1, "2");
                dtCmoDet.Rows.Add(l1.ToArray());
                l1 = orderedResults.Select(x => x[i].Cmo_pat3.ToString("N2")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i)); l1.Insert(1, "3");
                dtCmoDet.Rows.Add(l1.ToArray());
            }

            try
            {
                int semana;
                int conta = 0;
                dtCmoDet.Rows.Add();
                foreach (var item in orderedResults)
                {
                    num = item.CMO_Mensal_Result.Count();
                }
                var soma = orderedResults.Where(x => x.CMO_Mensal_Result[0].submercado == 1).Sum(x => x.CMO_Mensal_Result[0].CMO_Men);
                for (int i = 0; i < num; i++)
                {
                    int sub = orderedResults.Select(x => x.CMO_Mensal_Result[i].submercado).Last();
                    if (conta == 0)
                    {
                        semana = orderedResults.Select(x => x.CMO_Mensal_Result[i].semana).Last();
                        dtCmoDet.Rows.Add("Semana " + semana);
                        conta++;
                    }
                    else
                    {
                        conta++;
                        if (conta == 4) conta = 0;
                    }
                    l1 = orderedResults.Select(x => x.CMO_Mensal_Result[i].CMO_Men.ToString("N2")).ToList();
                    l1.Insert(0, Convert.ToString(sub));
                    l1.Insert(1, "");
                    dtCmoDet.Rows.Add(l1.ToArray());
                }
            }
            catch
            {

            }



            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].EarmI.ToString("P2")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtEarm.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].EnaSemCV.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENAcv.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].Ena.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENA.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].EnaMLT.ToString("P0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENAp.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].EnaTH.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENATH.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].EnaTHMLT.ToString("P0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENATHp.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].DemandaMes.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtDemanda.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].GerHidr.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtGerHidr.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++) {
                l1 = orderedResults.Select(x => x[i].GerTerm.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtGerTerm.Rows.Add(l1.ToArray());
            }

            this.Text = "Resultados - " + commonPath;

            ShowInfo(dataSources);

        }

       

        private void RefreshViewSingle() {


            var dtResumo = new System.Data.DataTable();
            var dtCmo = new System.Data.DataTable();

            var dataSources = new ResultDataSource[] { 
             new ResultDataSource(){ DataSource = dtResumo, Title = "Resumo" },
             new ResultDataSource(){ DataSource = dtCmo, Title = "Cmo" },
            };

            dtResumo.Columns.Add("MERCADO");
            dtResumo.Columns.Add("CMO");
            dtResumo.Columns.Add("EARM INI");
            dtResumo.Columns.Add("ENA CV");
            dtResumo.Columns.Add("ENA");
            dtResumo.Columns.Add("ENA %");
            dtResumo.Columns.Add("ENA TH");
            dtResumo.Columns.Add("ENA TH %");
            dtResumo.Columns.Add("DEMANDA");
            dtResumo.Columns.Add("GERHIDR");
            dtResumo.Columns.Add("GERTERM");
            dtResumo.Columns.Add("GERPEQ");
            //dt[0].Columns.Add("DEMANDA 2o MES");


            dtCmo.Columns.Add("MERCADO");
            dtCmo.Columns.Add("CMO 1");
            dtCmo.Columns.Add("CMO 2");
            dtCmo.Columns.Add("CMO 3");
            dtCmo.Columns.Add("CMO");


            _results.First().Value.Sistemas.Select(
                x => {
                    dtResumo.Rows.Add(
                        new object[] { x.Sistema.ToString(), x.Cmo.ToString("N2"), x.EarmI.ToString("P1"), x.EnaSemCV.ToString("N0"), x.Ena.ToString("N0"), x.EnaMLT.ToString("P0"), x.EnaTH.ToString("N0"), x.EnaTHMLT.ToString("P0")
                                , x.DemandaMes.ToString("N0"), x.GerHidr.ToString("N0"), x.GerTerm.ToString("N0"), x.GerPeq.ToString("N0")
                                //, x.DemandaMesSeguinte.ToString("N0")
                        }
                        );

                    dtCmo.Rows.Add(
                        new object[] { x.Sistema.ToString(), x.Cmo_pat1.ToString("N2"), x.Cmo_pat2.ToString("N2"), x.Cmo_pat3.ToString("N2"), x.Cmo.ToString("N2") }
                        );

                    return true;
                }).ToList();

            this.Text = _results.First().Value.Tipo + ": " + _results.First().Value.Dir;

            ShowInfo(dataSources);

        }


        public static string GetOrder(string x) {
            var arr = x.ToLowerInvariant().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var ord = "10";
            for (int ordI = 0; ordI < arr.Length; ordI++) {

                var n = arr[ordI];
                var m = System.Text.RegularExpressions.Regex.Match(n, "(?<=_)[+-]?\\d+");
                if (m.Success) ord += (int.Parse(m.Value) + 50).ToString("00");
                else {
                    m = System.Text.RegularExpressions.Regex.Match(n, "^[+-]?\\d+");
                    if (m.Success) ord += (int.Parse(m.Value) + 50).ToString("00");
                    else ord += "99";
                }
                ord += n.PadRight(20).Substring(0, 20);
            }
            return ord;
        }
        private string GetCommonPath(string[] p) {

            if (p.Length < 2) return "Z:\\";

            int idx = -1;
            int mark = 0;

            char refChar;

            do {
                idx++;
                refChar = p[0][idx];
                if (refChar == '\\') mark = idx + 1;

            } while (p.All(x => x[idx] == refChar && x.Length > idx + 1));

            return p.First().Substring(0, mark);
        }




    }
}

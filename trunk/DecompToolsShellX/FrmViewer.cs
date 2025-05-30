﻿using Compass.CommomLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX
{
    public partial class FormViewer : Form
    {

        Dictionary<string, Result> _results = new Dictionary<string, Result>();

        public FormViewer(string caption)
        {
            InitializeComponent();
            this.Text = caption;
        }

        public void AddInfo(string title, ResultDataSource dataSource)
        {

            this.tabControl1.Controls.Add(
                new InfoTabPage()
                {
                    Title = title,
                    DataSource = dataSource
                });
        }

        public void ClearInfo()
        {
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

        public static void Show(String caption, params Result[] results)
        {
            var frm = new FormViewer(caption);

            foreach (var res in results)
            {
                frm._results[res.Dir] = res;
            }

            frm.RefreshView();

            if (System.Threading.Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {

                Thread thread = new Thread(() => frm.ShowDialog());
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                //thread.Join(); //Wait for the thread to end

            }
            else
            {
                frm.ShowDialog();
            }

        }

        public static void Show(String caption, bool multi = true, params Result[] results)
        {
            var frm = new FormViewer(caption);

            foreach (var res in results)
            {
                frm._results[res.Dir] = res;
            }

            frm.RefreshView(multi);

            if (System.Threading.Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {

                Thread thread = new Thread(() => frm.ShowDialog());
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                //thread.Join(); //Wait for the thread to end

            }
            else
            {
                frm.ShowDialog();
            }

        }
        internal static void Show(string caption, ResultDataSource resultDataSource)
        {
            var frm = new FormViewer(caption);



            frm.ShowInfo(resultDataSource);

            if (System.Threading.Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {

                Thread thread = new Thread(() => frm.ShowDialog());
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                //thread.Join(); //Wait for the thread to end

            }
            else
            {
                frm.ShowDialog();
            }
        }

        private void ShowInfo(params ResultDataSource[] dataSources)
        {
            ClearInfo();

            for (int i = 0; i < dataSources.Length; i++)
            {

                var t = !string.IsNullOrWhiteSpace(dataSources[i].Title) ? dataSources[i].Title : "info " + i.ToString();

                AddInfo(t, dataSources[i]);
            }
        }

        private void FormViewer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }

        }

        private void FormViewer_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


            bool refreshView = false;
            if (files != null && files.Length != 0)
            {

                foreach (var file in files)
                {

                    string dir;

                    if (System.IO.Directory.Exists(file))
                    {
                        dir = file;
                    }
                    else
                    {
                        dir = System.IO.Path.GetDirectoryName(file);
                    }
                    var deck = DeckFactory.CreateDeck(dir);

                    if (deck is CommomLibrary.Newave.Deck || deck is CommomLibrary.Decomp.Deck)
                    {
                        var results = deck.GetResults();
                        _results[dir] = results;
                        refreshView = true;
                    }
                    else continue;
                }
            }

            if (refreshView)
            {
                RefreshView();
            }
        }

        private void RefreshView()
        {
            this.Cursor = Cursors.WaitCursor;

            _results.Remove("");

            if (_results.Keys.Count > 1)
            {
                if (_results.First().Value.PDO_Sist_Result != null)
                {
                    RefreshViewMultipleDessem();
                }
                else
                {
                    RefreshViewMultiple();
                }
            }
            else if (_results.Keys.Count == 1)
            {
                RefreshViewSingle();
            }

            this.Cursor = Cursors.Default;
        }

        private void RefreshView(bool multi)
        {
            this.Cursor = Cursors.WaitCursor;

            _results.Remove("");

            if (_results.Keys.Count > 1 || multi == true)
            {
                if (_results.First().Value.PDO_Sist_Result != null)
                {
                    RefreshViewMultipleDessem();
                }
                else
                {
                    RefreshViewMultiple();
                }
            }
            else if (_results.Keys.Count == 1)
            {
                RefreshViewSingle();
            }

            this.Cursor = Cursors.Default;
        }

        private void RefreshViewMultipleDessem()
        {
            var commonPath = GetCommonPath(_results.Select(x => x.Value.Dir).ToArray());

            var orderedResults = _results.Select(x => x.Value).OrderBy(x => GetOrder(x.Dir)).ToList();

            var dtCmo = new DataTable();
            var dtCarga = new DataTable();
            var dtPQ = new DataTable();
            var dtSomaGH = new DataTable();
            var dtSomaGT = new DataTable();
            var dtConsElev = new DataTable();
            var dtImport = new DataTable();
            var dtExport = new DataTable();
            var dtSaldo = new DataTable();
            var dtGTMin = new DataTable();
            var dtGTMax = new DataTable();
            var dtEARM = new DataTable();
            var dtPld = new DataTable();

            var dataSources = new ResultDataSource[] {
             new ResultDataSource(){ DataSource =  dtCmo   , Title = "CMO" },
             new ResultDataSource(){ DataSource =  dtCarga, Title = "CARGA" },
             new ResultDataSource(){ DataSource =  dtPQ  , Title = "PQ" },
             new ResultDataSource(){ DataSource =  dtSomaGH , Title = "SOMA GH" },
             new ResultDataSource(){ DataSource =  dtSomaGT   , Title = "SOMA GT" },
             new ResultDataSource(){ DataSource =  dtConsElev  , Title = "CONS. ELEV." },
             new ResultDataSource(){ DataSource =  dtImport , Title = "IMPORTAÇÃO" },
             new ResultDataSource(){ DataSource =  dtExport, Title = "EXPORTAÇÃO" },
             new ResultDataSource(){ DataSource =  dtSaldo , Title = "SALDO" },
             new ResultDataSource(){ DataSource =  dtGTMin , Title = "GT MIN" },
             new ResultDataSource(){ DataSource =  dtGTMax , Title = "GT MAX" },
             new ResultDataSource(){ DataSource =  dtEARM, Title = "EARM" },
             new ResultDataSource(){ DataSource =  dtPld, Title = "PLD" },

            };


            var dirNames = orderedResults.Select(x => x.Dir.Remove(0, commonPath.Length)).ToList();
            dirNames.Insert(0, "");
            dirNames.Insert(1, "");

            foreach (var d in dataSources.Select(x => x.DataSource as DataTable))
            {
                d.Columns.Add("ESTÁGIO");
                d.Columns.Add("MERCADO");
                foreach (var r in orderedResults) d.Columns.Add(r.Tipo + ": " + r.Dir.Remove(0, commonPath.Length));
                d.Rows.Add(dirNames.ToArray());
            }
            var submercados = _results.First().Value.PDO_Sist_Result.Select(x => x.submercado).Distinct().ToList();//

            List<string> l1 = new List<string>(orderedResults.Count());
            for (int i = 1; i <= 48; i++)
            {
                foreach (var sub in submercados)
                {
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.CMO.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtCmo.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.Carga.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtCarga.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.ConsElev.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtConsElev.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.Earm.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtEARM.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.Export.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtExport.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.GTMax.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtGTMax.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.GTMin.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtGTMin.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.Import.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtImport.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.PQ.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtPQ.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.Saldo.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtSaldo.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.SomaGH.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtSomaGH.Rows.Add(l1.ToArray());
                    l1.Clear();
                    //
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PDO_Sist_Result.Where(x => x.submercado == sub && x.estagio == i).Select(x => x.SomaGT.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(i));

                    l1.Insert(1, sub);
                    dtSomaGT.Rows.Add(l1.ToArray());
                    l1.Clear();

                }

            }

            var estagios = _results.First().Value.PLD_DESSEM_Result.Select(x => x.estagio).Distinct().ToList();//

            for (int est = 1; est <= estagios.Count(); est++)
            {
                foreach (var sub in submercados)
                {
                    foreach (var item in orderedResults)
                    {
                        l1.Add(item.PLD_DESSEM_Result.Where(x => x.submercado == sub && x.estagio == est).Select(x => x.PLD.ToString("N2")).First());
                    }
                    l1.Insert(0, Convert.ToString(est));

                    l1.Insert(1, sub);
                    dtPld.Rows.Add(l1.ToArray());
                    l1.Clear();
                }

            }

            foreach (var sub in submercados)
            {
                foreach (var item in orderedResults)
                {
                    l1.Add(item.PLD_DESSEM_Result.Where(x => x.submercado == sub).Select(x => x.PLD).Average().ToString("N2"));
                }
                l1.Insert(0, "MEDIA");

                l1.Insert(1, sub);
                dtPld.Rows.Add(l1.ToArray());
                l1.Clear();
            }

            this.Text = "Resultados - " + commonPath;

            ShowInfo(dataSources);
        }

        private void RefreshViewMultiple()
        {

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
            var dtGerEol = new DataTable();

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
             new ResultDataSource(){ DataSource =  dtGerEol , Title = "Ger Eol" },
             new ResultDataSource(){ DataSource =  dtCortes, Title = "Cortes" },
              new ResultDataSource(){ DataSource =  dtGNL, Title = "Despacho GNL" }

            };


            var dirNames = orderedResults.Select(x => x.Tipo + ": " + x.Dir.Remove(0, commonPath.Length)).ToList();
            dirNames.Insert(0, "");

            foreach (var d in dataSources.Select(x => x.DataSource as DataTable))
            {

                if (d == dtCortes)
                {
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
                else
                {
                    d.Columns.Add("MERCADO");
                    if (d == dtCmoDet) d.Columns.Add("PATAMAR");

                    foreach (var r in orderedResults) d.Columns.Add();
                }

                if (d == dtCmoDet) d.Rows.Add((new string[] { "" }).Concat(dirNames).ToArray());
                else if (d == dtGNL)
                {
                    d.Rows.Add((new string[] { "", "" }).Concat(dirNames).ToArray());
                }
                else if (d == dtCortes) { } else d.Rows.Add(dirNames.ToArray());
            }


            foreach (var item in orderedResults) dtCortes.Rows.Add(item.Dir.Replace(commonPath, ""), item.Cortes.Replace(commonCortesPath, ""));

            int num = 0;
            List<string> l1;
            int[] usinas = new int[] { 15, 86, 224 };
            try
            {
                foreach (var item in orderedResults)
                {
                    num = item.GNL_Result.Count();
                }

                for (int i = 0; i < num; i++)
                {

                    int semana = orderedResults.Select(x => x.GNL_Result[i].semana).Last();
                    int usi = orderedResults.Select(x => x.GNL_Result[i].Posto).Last();

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
            }
            catch
            {

            }


            for (int i = 1; i <= 4; i++)
            {
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



            for (int i = 1; i <= 4; i++)
            {
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



            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].EarmI.ToString("P2")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtEarm.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].EnaSemCV.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENAcv.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].Ena.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENA.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].EnaMLT.ToString("P0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENAp.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].EnaTH.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENATH.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].EnaTHMLT.ToString("P0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtENATHp.Rows.Add(l1.ToArray());
            }
            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].DemandaPrimeiroEstagio.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtDemanda.Rows.Add(l1.ToArray());
            }

            dtDemanda.Rows.Add();
            dtDemanda.Rows.Add("Média 1° Mês");

            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].DemandaMes.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtDemanda.Rows.Add(l1.ToArray());
            }

            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].GerHidr.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtGerHidr.Rows.Add(l1.ToArray());
            }
            dtGerHidr.Rows.Add();
            dtGerHidr.Rows.Add("Média 1° Mês");


            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].GerHidrMedia.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtGerHidr.Rows.Add(l1.ToArray());
            }

            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].GerTerm.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtGerTerm.Rows.Add(l1.ToArray());
            }

            dtGerTerm.Rows.Add();
            dtGerTerm.Rows.Add("Média 1° Mês");

            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].GerTermMedia.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtGerTerm.Rows.Add(l1.ToArray());
            }
            /////
            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].GerEol.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtGerEol.Rows.Add(l1.ToArray());
            }

            dtGerEol.Rows.Add();
            dtGerEol.Rows.Add("Média 1° Mês");

            for (int i = 1; i <= 4; i++)
            {
                l1 = orderedResults.Select(x => x[i].GerEolMedia.ToString("N0")).ToList(); l1.Insert(0, Enum.GetName(typeof(SistemaEnum), i));
                dtGerEol.Rows.Add(l1.ToArray());
            }


            ///



            this.Text = "Resultados - " + commonPath;

            ShowInfo(dataSources);

        }



        private void RefreshViewSingle()
        {

            if (_results.First().Value.PDO_Sist_Result == null)//Mostra os resultados de decks decomp e newave
            {
                bool modeloNovo = _results.First().Value.novo;
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

                //
                if (modeloNovo)
                {
                    dtResumo.Columns.Add("GEREOL");
                }
                //

                //dt[0].Columns.Add("DEMANDA 2o MES");


                dtCmo.Columns.Add("MERCADO");
                dtCmo.Columns.Add("CMO 1");
                dtCmo.Columns.Add("CMO 2");
                dtCmo.Columns.Add("CMO 3");
                dtCmo.Columns.Add("CMO");


                _results.First().Value.Sistemas.Select(
                    x =>
                    {
                        if (modeloNovo)
                        {
                            dtResumo.Rows.Add(
                            new object[] { x.Sistema.ToString(), x.Cmo.ToString("N2"), x.EarmI.ToString("P1"), x.EnaSemCV.ToString("N0"), x.Ena.ToString("N0"), x.EnaMLT.ToString("P0"), x.EnaTH.ToString("N0"), x.EnaTHMLT.ToString("P0")
                                , x.DemandaMes.ToString("N0"), x.GerHidr.ToString("N0"), x.GerTerm.ToString("N0"), x.GerPeq.ToString("N0"),x.GerEol.ToString("N0")
                                //, x.DemandaMesSeguinte.ToString("N0")
                            }
                            );
                        }
                        else
                        {
                            dtResumo.Rows.Add(
                            new object[] { x.Sistema.ToString(), x.Cmo.ToString("N2"), x.EarmI.ToString("P1"), x.EnaSemCV.ToString("N0"), x.Ena.ToString("N0"), x.EnaMLT.ToString("P0"), x.EnaTH.ToString("N0"), x.EnaTHMLT.ToString("P0")
                                , x.DemandaMes.ToString("N0"), x.GerHidr.ToString("N0"), x.GerTerm.ToString("N0"), x.GerPeq.ToString("N0")
                                //, x.DemandaMesSeguinte.ToString("N0")
                            }
                            );
                        }

                        dtCmo.Rows.Add(
                            new object[] { x.Sistema.ToString(), x.Cmo_pat1.ToString("N2"), x.Cmo_pat2.ToString("N2"), x.Cmo_pat3.ToString("N2"), x.Cmo.ToString("N2") }
                            );

                        return true;
                    }).ToList();

                this.Text = _results.First().Value.Tipo + ": " + _results.First().Value.Dir;

                ShowInfo(dataSources);
            }
            else// mostra os resultados de decks dessem
            {
                var submercados = _results.First().Value.PDO_Sist_Result.Select(x => x.submercado).Distinct().ToList();//

                var dataSources = new ResultDataSource[submercados.Count() + 2];//
                int i = 0;//

                //aba SIN
                var dtSIN = new System.Data.DataTable();



                var rdsSIN = new ResultDataSource() { DataSource = dtSIN, Title = "SIN" };//

                dtSIN.Columns.Add("ESTÁGIO");
                dtSIN.Columns.Add("CMO");
                dtSIN.Columns.Add("CARGA");
                dtSIN.Columns.Add("PQ");
                dtSIN.Columns.Add("CARGA LIQ.");
                dtSIN.Columns.Add("SOMA GH");
                dtSIN.Columns.Add("SOMA GT");
                dtSIN.Columns.Add("CONS. ELEV.");
                dtSIN.Columns.Add("IMPORTAÇÃO");
                dtSIN.Columns.Add("EXPORTAÇÃO");
                dtSIN.Columns.Add("SALDO");
                dtSIN.Columns.Add("GT MIN");
                dtSIN.Columns.Add("GT MAX");
                dtSIN.Columns.Add("EARM");

                var estagiosSIN = _results.First().Value.PDO_Sist_Result.Select(x => x.estagio).Distinct().ToList();

                for (int estS = 1; estS <= estagiosSIN.Count(); estS++)
                {
                    var resultSE = _results.First().Value.PDO_Sist_Result.Where(x => x.submercado == "SE" && x.estagio == estS).First();
                    var resultS = _results.First().Value.PDO_Sist_Result.Where(x => x.submercado == "S" && x.estagio == estS).First();
                    var resultNE = _results.First().Value.PDO_Sist_Result.Where(x => x.submercado == "NE" && x.estagio == estS).First();
                    var resultN = _results.First().Value.PDO_Sist_Result.Where(x => x.submercado == "N" && x.estagio == estS).First();

                    double cmoSin = resultSE.CMO;
                    double CARGASin = (resultSE.Carga + resultS.Carga + resultNE.Carga + resultN.Carga);
                    double PQSin = (resultSE.PQ + resultS.PQ + resultNE.PQ + resultN.PQ);
                    double LIQSin = CARGASin - PQSin;
                    double sghSin = (resultSE.SomaGH + resultS.SomaGH + resultNE.SomaGH + resultN.SomaGH);
                    double sgtSin = (resultSE.SomaGT + resultS.SomaGT + resultNE.SomaGT + resultN.SomaGT);
                    double consSin = (resultSE.ConsElev + resultS.ConsElev + resultNE.ConsElev + resultN.ConsElev);
                    double importSin = (resultSE.Import + resultS.Import + resultNE.Import + resultN.Import);
                    double exportSin = (resultSE.Export + resultS.Export + resultNE.Export + resultN.Export);
                    double saldoSin = (resultSE.Saldo + resultS.Saldo + resultNE.Saldo + resultN.Saldo);
                    double gtMinSin = (resultSE.GTMin + resultS.GTMin + resultNE.GTMin + resultN.GTMin);
                    double gtMaxSin = (resultSE.GTMax + resultS.GTMax + resultNE.GTMax + resultN.GTMax);
                    double earmSin = (resultSE.Earm + resultS.Earm + resultNE.Earm + resultN.Earm);

                    List<string> lsin = new List<string>();
                    lsin.Add(estS.ToString());
                    lsin.Add(cmoSin.ToString("N2"));
                    lsin.Add(CARGASin.ToString("N2"));
                    lsin.Add(PQSin.ToString("N2"));
                    lsin.Add(LIQSin.ToString("N2"));
                    lsin.Add(sghSin.ToString("N2"));
                    lsin.Add(sgtSin.ToString("N2"));
                    lsin.Add(consSin.ToString("N2"));
                    lsin.Add(importSin.ToString("N2"));
                    lsin.Add(exportSin.ToString("N2"));
                    lsin.Add(saldoSin.ToString("N2"));
                    lsin.Add(gtMinSin.ToString("N2"));
                    lsin.Add(gtMaxSin.ToString("N2"));
                    lsin.Add(earmSin.ToString("N2"));

                    dtSIN.Rows.Add(lsin.ToArray());
                    lsin.Clear();
                }

                dataSources[i] = rdsSIN;
                i++;

                //submercados
                foreach (var sub in submercados)//
                {
                    var dtResumo = new System.Data.DataTable();



                    var rds = new ResultDataSource() { DataSource = dtResumo, Title = sub };//

                    dtResumo.Columns.Add("ESTÁGIO");
                    dtResumo.Columns.Add("CMO");
                    dtResumo.Columns.Add("CARGA");
                    dtResumo.Columns.Add("PQ");
                    dtResumo.Columns.Add("CARGA LIQ.");
                    dtResumo.Columns.Add("SOMA GH");
                    dtResumo.Columns.Add("SOMA GT");
                    dtResumo.Columns.Add("CONS. ELEV.");
                    dtResumo.Columns.Add("IMPORTAÇÃO");
                    dtResumo.Columns.Add("EXPORTAÇÃO");
                    dtResumo.Columns.Add("SALDO");
                    dtResumo.Columns.Add("GT MIN");
                    dtResumo.Columns.Add("GT MAX");
                    dtResumo.Columns.Add("EARM");


                    _results.First().Value.PDO_Sist_Result.Where(x => x.submercado == sub).Select(
                       x =>
                       {
                           double cargaLiquida = x.Carga - x.PQ;
                           dtResumo.Rows.Add(
                               new object[] { x.estagio.ToString(),/* x.submercado.ToString(),*/ x.CMO.ToString("N2"), x.Carga.ToString("N2"), x.PQ.ToString("N2"),cargaLiquida, x.SomaGH.ToString("N2"), x.SomaGT.ToString("N2"), x.ConsElev.ToString("N2")
                                , x.Import.ToString("N2"), x.Export.ToString("N2"), x.Saldo.ToString("N2"), x.GTMin.ToString("N2"), x.GTMax.ToString("N2"), x.Earm.ToString("N2")
                                   //, x.DemandaMesSeguinte.ToString("N0")
                               }
                               );

                           return true;
                       }).ToList();

                    dataSources[i] = rds;//
                    i++;//
                }

                

                //PLD

                var dtPld = new System.Data.DataTable();

                var rdsPld = new ResultDataSource() { DataSource = dtPld, Title = "PLD" };

                dtPld.Columns.Add("ESTAGIO");
                dtPld.Columns.Add("SE");
                dtPld.Columns.Add("S");
                dtPld.Columns.Add("NE");
                dtPld.Columns.Add("N");

                var estagios = _results.First().Value.PLD_DESSEM_Result.Select(x => x.estagio).Distinct().ToList();//

                List<string> l1 = new List<string> { "#", "SE", "S", "NE", "N" };

                dtPld.Rows.Add(l1.ToArray());
                l1.Clear();

                for (int est = 1; est <= estagios.Count(); est++)
                {
                    l1.Add(_results.First().Value.PLD_DESSEM_Result.Where(x => x.submercado == "SE" && x.estagio == est).Select(x => x.PLD.ToString("N2")).First());
                    l1.Add(_results.First().Value.PLD_DESSEM_Result.Where(x => x.submercado == "S" && x.estagio == est).Select(x => x.PLD.ToString("N2")).First());
                    l1.Add(_results.First().Value.PLD_DESSEM_Result.Where(x => x.submercado == "NE" && x.estagio == est).Select(x => x.PLD.ToString("N2")).First());
                    l1.Add(_results.First().Value.PLD_DESSEM_Result.Where(x => x.submercado == "N" && x.estagio == est).Select(x => x.PLD.ToString("N2")).First());

                    l1.Insert(0, Convert.ToString(est));

                    dtPld.Rows.Add(l1.ToArray());
                    l1.Clear();
                }
                l1.Add(_results.First().Value.PLD_DESSEM_Result.Where(x => x.submercado == "SE").Select(x => x.PLD).Average().ToString("N2"));
                l1.Add(_results.First().Value.PLD_DESSEM_Result.Where(x => x.submercado == "S").Select(x => x.PLD).Average().ToString("N2"));
                l1.Add(_results.First().Value.PLD_DESSEM_Result.Where(x => x.submercado == "NE").Select(x => x.PLD).Average().ToString("N2"));
                l1.Add(_results.First().Value.PLD_DESSEM_Result.Where(x => x.submercado == "N").Select(x => x.PLD).Average().ToString("N2"));
                l1.Insert(0, "MEDIA");
                dtPld.Rows.Add(l1.ToArray());

                l1.Clear();

                dataSources[i] = rdsPld;//

                this.Text = _results.First().Value.Tipo + ": " + _results.First().Value.Dir;

                ShowInfo(dataSources);
            }


        }


        public static string GetOrder(string x)
        {
            var arr = x.ToLowerInvariant().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var ord = "10";
            for (int ordI = 0; ordI < arr.Length; ordI++)
            {

                var n = arr[ordI];
                var m = System.Text.RegularExpressions.Regex.Match(n, "(?<=_)[+-]?\\d+");
                if (m.Success) ord += (int.Parse(m.Value) + 50).ToString("00");
                else
                {
                    m = System.Text.RegularExpressions.Regex.Match(n, "^[+-]?\\d+");
                    if (m.Success) ord += (int.Parse(m.Value) + 50).ToString("00");
                    else ord += "99";
                }
                ord += n.PadRight(20).Substring(0, 20);
            }
            return ord;
        }
        private string GetCommonPath(string[] p)
        {

            if (p.Length < 2)
            {
                if (p.Length > 0)
                {
                    string commonfolder = p.First().Replace(p.First().Split('\\').Last(), "");
                    //return "K:\\";
                    return commonfolder;
                }
                else
                {
                    return "K:\\";
                }

            }
            List<string> folders = new List<string>();

            foreach (var f in p)
            {
                var partes = f.Split('\\').ToList();
                foreach (var part in partes)
                {
                    if (p.All(x => x.Split('\\').Any(y => y.Equals(part))) && !folders.Contains(part))
                    {
                        folders.Add(part);
                    }
                }
            }
            if (folders.Count() > 0)
            {
                string ret = string.Join("\\", folders.ToArray()) + "\\";
                return ret;
            }
            int idx = -1;
            int mark = 0;

            char refChar;

            do
            {
                idx++;
                refChar = p[0][idx];
                if (refChar == '\\') mark = idx + 1;

            } while (p.All(x => x[idx] == refChar && x.Length > idx + 1));

            return p.First().Substring(0, mark);
        }




    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Compass.CommomLibrary;
using System.Windows.Forms.DataVisualization.Charting;

namespace Compass.DecompToolsShellX
{
    public partial class FrmDSresulGraph : Form
    {
        Dictionary<string, Result> _results = new Dictionary<string, Result>();

        public FrmDSresulGraph(String caption, params Result[] results)
        {
            InitializeComponent();
            this.Text = caption;
            this.chart1.MouseMove += new MouseEventHandler(chart1_MouseMove);
            foreach (var res in results)
            {
                _results[res.Dir] = res;
            }
            List<string> campos = new List<string> { "CMO", "CARGA", "PQ", "SOMA GH", "SOMA GT", "CONS. ELEV.", "IMPORTAÇÃO", "EXPORTAÇÃO", "SALDO", "GT MIN", "GT MAX", "EARM", "PLD" };
            //comboBox1.DataSource = _results.Values.First().PDO_Sist_Result;
            comboBox1.DataSource = campos;
            comboBox1.SelectedIndex = 0;

            var commonPath = GetCommonPath(_results.Select(x => x.Value.Dir).ToArray());

            var orderedResults = _results.Select(x => x.Value).OrderBy(x => GetOrder(x.Dir)).ToList();

            // foreach (var r in orderedResults) d.Columns.Add(r.Tipo + ": " + r.Dir.Remove(0, commonPath.Length));

            var dirNames = orderedResults.Select(x => x.Dir.Remove(0, commonPath.Length)).ToList();
            foreach (var dir in dirNames)
            {
                string[] linha = new string[1];
                linha[0] = dir;

                ListViewItem l = new ListViewItem(linha);

                lv_resGraph.Items.Add(l);
            }
            
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.check_SE.CheckedChanged += new System.EventHandler(this.check_se_CheckedChanged);
            this.check_SUL.CheckedChanged += new System.EventHandler(this.check_sul_CheckedChanged);
            this.check_NE.CheckedChanged += new System.EventHandler(this.check_ne_CheckedChanged);
            this.check_N.CheckedChanged += new System.EventHandler(this.check_n_CheckedChanged);
        }

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();
        void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart1.HitTest(pos.X, pos.Y, false,
                                            ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;
                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                        // check if the cursor is really close to the point (2 pixels around)
                        if (Math.Abs(pos.X - pointXPixel) < 2 &&
                            Math.Abs(pos.Y - pointYPixel) < 2)
                        {
                            tooltip.Show(result.Series.Name + ", Est=" + prop.XValue + ", Valor =" + prop.YValues[0], this.chart1,
                                            pos.X, pos.Y - 15);

                        }
                    }
                }
            }
        }

        private void RefreshView()
        {
            this.Cursor = Cursors.WaitCursor;

            _results.Remove("");

            if (_results.Keys.Count > 1)
            {
                RefreshViewMultiple();
            }
            else if (_results.Keys.Count == 1)
            {
                RefreshViewSingle();
            }

            this.Cursor = Cursors.Default;
        }

        private List<Tuple<string, int, double>> GetDsResults()
        {
            List<Tuple<string, int, double>> lista = new List<Tuple<string, int, double>>();
            List<string> submercado = new List<string>();
            if (check_SE.Checked)
            {
                submercado.Add("SE");
            }
            if (check_SUL.Checked)
            {
                submercado.Add("S");
            }
            if (check_NE.Checked)
            {
                submercado.Add("NE");
            }
            if (check_N.Checked)
            {
                submercado.Add("N");
            }

            foreach (var sub in submercado)
            {

                switch (comboBox1.SelectedItem.ToString())
                {

                    case "CMO":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.CMO)));
                        break;
                    case "CARGA":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.Carga)));
                        break;
                    case "PQ":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.PQ)));
                        break;
                    case "SOMA GH":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.SomaGH)));
                        break;
                    case "SOMA GT":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.SomaGT)));
                        break;
                    case "CONS. ELEV.":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.ConsElev)));
                        break;
                    case "IMPORTAÇÃO":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.Import)));
                        break;
                    case "EXPORTAÇÃO":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.Export)));
                        break;
                    case "SALDO":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.Saldo)));
                        break;
                    case "GT MIN":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.GTMin)));
                        break;
                    case "GT MAX":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.GTMax)));
                        break;
                    case "EARM":
                        _results.Values.First().PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.Earm)));
                        break;
                    case "PLD":
                        _results.Values.First().PLD_DESSEM_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double>(sub, y.estagio, y.PLD)));
                        break;
                }
            }


            return lista;
        }

        private List<Tuple<string, int, double, string>> GetDsMultiResults()
        {
            List<Tuple<string, int, double, string>> lista = new List<Tuple<string, int, double, string>>();//mercado,estagio,valor,deck
            List<string> submercado = new List<string>();

            var commonPath = GetCommonPath(_results.Select(x => x.Value.Dir).ToArray());

            var orderedResults = _results.Select(x => x.Value).OrderBy(x => GetOrder(x.Dir)).ToList();


            if (check_SE.Checked)
            {
                submercado.Add("SE");
            }
            if (check_SUL.Checked)
            {
                submercado.Add("S");
            }
            if (check_NE.Checked)
            {
                submercado.Add("NE");
            }
            if (check_N.Checked)
            {
                submercado.Add("N");
            }


            foreach (var sub in submercado)
            {

                switch (comboBox1.SelectedItem.ToString())
                {

                    case "CMO":

                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.CMO, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "CARGA":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.Carga, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "PQ":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.PQ, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "SOMA GH":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.SomaGH, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "SOMA GT":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.SomaGT, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "CONS. ELEV.":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.ConsElev, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "IMPORTAÇÃO":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.Import, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "EXPORTAÇÃO":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.Export, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "SALDO":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.Saldo, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "GT MIN":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.GTMin, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "GT MAX":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.GTMax, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;

                    case "EARM":
                        foreach (var or in orderedResults)
                        {
                            or.PDO_Sist_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.Earm, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;
                    case "PLD":
                        foreach (var or in orderedResults)
                        {
                            or.PLD_DESSEM_Result.Where(x => x.submercado == sub).ToList().ForEach(y => lista.Add(new Tuple<string, int, double, string>(sub, y.estagio, y.PLD, or.Dir.Remove(0, commonPath.Length))));
                        }
                        break;
                }
            }


            return lista;
        }
        private void RefreshViewSingle()
        {

            this.chart1.Series.Clear();
            this.chart1.ChartAreas.Clear();
            ChartArea nChart = new ChartArea();
            nChart.AxisX.Title = "Estágios";
            nChart.AxisY.Title = "Valores";
            chart1.ChartAreas.Add(nChart);

            this.chart1.ChartAreas[0].AxisX.Interval = 1;

            this.chart1.Titles.Clear();
            this.chart1.Titles.Add(comboBox1.SelectedItem.ToString());

            var lista = GetDsResults();

            var submercados = lista.Select(x => x.Item1).Distinct().ToList();

            foreach (var sub in submercados)
            {

                this.chart1.Series.Add(BuildSeries(sub, lista));

            }

        }

        private void RefreshViewMultiple()
        {

            this.chart1.Series.Clear();
            this.chart1.ChartAreas.Clear();
            ChartArea nChart = new ChartArea();
            nChart.AxisX.Title = "Estágios";
            nChart.AxisY.Title = "Valores";
            chart1.ChartAreas.Add(nChart);

            this.chart1.ChartAreas[0].AxisX.Interval = 1;

            this.chart1.Titles.Clear();
            this.chart1.Titles.Add(comboBox1.SelectedItem.ToString());

            var lista = GetDsMultiResults();

            var submercados = lista.Select(x => x.Item1).Distinct().ToList();

            List<string> lvDEcks = new List<string>();//somente os decks selecionados pelo usuario serão mostrados

            foreach (ListViewItem lst in lv_resGraph.CheckedItems)
            {
                lvDEcks.Add(lst.SubItems[0].Text);
            }

            foreach (var sub in submercados)
            {
                foreach (var deck in lvDEcks)
                {
                    var filterList = lista.Where(x => x.Item4 == deck).ToList();
                    this.chart1.Series.Add(BuildMultiSeries(sub, filterList));

                }
            }

        }

        private Series BuildSeries(string sub, List<Tuple<string, int, double>> lista)
        {

            Series series1 = new Series();
           
            series1.LegendText = sub;
            series1.Name = sub;
            series1.XValueType = ChartValueType.Int32;
            series1.YValueType = ChartValueType.Double;
            series1.YValuesPerPoint = 1;
            series1.ChartType = SeriesChartType.Line;
            series1.BorderWidth = 2;
            foreach (var l in lista.Where(x => x.Item1 == sub).ToList())
            {
                series1.Points.AddXY(l.Item2, l.Item3);
            }

            return series1;
        }

        private Series BuildMultiSeries(string sub, List<Tuple<string, int, double, string>> lista)
        {

            Series series1 = new Series();
            
            series1.Name = sub + "_" + lista.First().Item4;
            series1.XValueType = ChartValueType.Int32;
            series1.YValueType = ChartValueType.Double;
            series1.YValuesPerPoint = 1;
            series1.ChartType = SeriesChartType.Line;
            series1.BorderWidth = 2;
            series1.LegendText = sub + "_" + lista.First().Item4;
            foreach (var l in lista.Where(x => x.Item1 == sub).ToList())
            {
                series1.Points.AddXY(l.Item2, l.Item3);
            }

            return series1;
        }
        public static void Show(String caption, params Result[] results)
        {
            var frm = new FrmDSresulGraph(caption, results);



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

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshView();
        }
        private void check_se_CheckedChanged(object sender, EventArgs e)
        {
            RefreshView();


        }
        private void check_sul_CheckedChanged(object sender, EventArgs e)
        {
            RefreshView();
        }
        private void check_ne_CheckedChanged(object sender, EventArgs e)
        {
            RefreshView();
        }
        private void check_n_CheckedChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void btn_ToClipBoard_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
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

            if (p.Length < 2) return "X:\\AWS\\";

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

        private void lv_resGraph_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            RefreshView();
        }
    }
}

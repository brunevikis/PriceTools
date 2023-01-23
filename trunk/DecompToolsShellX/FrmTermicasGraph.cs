using System;
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
    public partial class FrmTermicasGraph : Form
    {
        Dictionary<string, Result> _results = new Dictionary<string, Result>();
        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();

        List<ct> ctSudeste = new List<ct>();
        List<ct> ctSul = new List<ct>();
        List<ct> ctNordeste = new List<ct>();
        List<ct> ctNorte = new List<ct>();
        bool descolamento = false;

        public FrmTermicasGraph(String caption, CommomLibrary.Dadger.Dadger dadger, params Result[] results)
        {
            InitializeComponent();
            this.chart1.MouseMove += new MouseEventHandler(chart1_MouseMove);
            this.check_SE.Visible = false;
            this.check_SUL.Visible = false;
            this.check_NE.Visible = false;
            this.check_N.Visible = false;

            this.lv_resGraph.Visible = false;

            foreach (var res in results)
            {
                _results[res.Dir] = res;
            }

            List<Tuple<string, double>> cmos = new List<Tuple<string, double>>();
            List<string> campos = new List<string> { "Sudeste", "Sul", "Nordeste", "Norte" };


            for (int i = 0; i < 4; i++)
            {
                var d = _results.First().Value.Sistemas[i];

                switch (d.Sistema.ToString())
                {
                    case "SE":
                        cmos.Add(new Tuple<string, double>("Sudeste", d.Cmo));
                        break;

                    case "S":
                        cmos.Add(new Tuple<string, double>("Sul", d.Cmo));
                        break;

                    case "NE":
                        cmos.Add(new Tuple<string, double>("Nordeste", d.Cmo));
                        break;

                    case "N":
                        cmos.Add(new Tuple<string, double>("Norte", d.Cmo));
                        break;
                }
                //cmos.Add(new Tuple<string, double>(d.Sistema.ToString(), d.Cmo));
            }

            if (cmos.Any(x => x.Item2 != cmos.First().Item2))
            {
                cmos.GroupBy(x => x.Item2).ToList().ForEach(x => campos.Add(string.Join(", ", x.ToList().Select(y => y.Item1))));

                //campos = new List<string> { "Sudeste", "Sul", "Nordeste", "Norte" };
                descolamento = true;
            }
            else
            {
                campos.Add("SIN");//campos = new List<string> { "SIN" };
                descolamento = false;
            }

            //List<string> campos = new List<string> { "CMO" };
            //comboBox1.DataSource = _results.Values.First().PDO_Sist_Result;
            comboBox1.DataSource = campos.Distinct().ToList();
            comboBox1.SelectedIndex = 0;

            var commonPath = GetCommonPath(_results.Select(x => x.Value.Dir).ToArray());

            var orderedResults = _results.Select(x => x.Value).OrderBy(x => GetOrder(x.Dir)).ToList();

            // foreach (var r in orderedResults) d.Columns.Add(r.Tipo + ": " + r.Dir.Remove(0, commonPath.Length));
            List<string> dirNames = new List<string>();
            if (orderedResults.Count > 1)
            {
                dirNames = orderedResults.Select(x => x.Dir.Remove(0, commonPath.Length)).ToList();
            }
            else
            {
                dirNames.Add(commonPath);
            }

            foreach (var dir in dirNames)
            {
                string[] linha = new string[1];
                linha[0] = dir;

                ListViewItem l = new ListViewItem(linha);

                lv_resGraph.Items.Add(l);
            }

            alocaCts(dadger);


            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            //this.check_SE.CheckedChanged += new System.EventHandler(this.check_se_CheckedChanged);
            //this.check_SUL.CheckedChanged += new System.EventHandler(this.check_sul_CheckedChanged);
            //this.check_NE.CheckedChanged += new System.EventHandler(this.check_ne_CheckedChanged);
            //this.check_N.CheckedChanged += new System.EventHandler(this.check_n_CheckedChanged);
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
                            if (!result.Series.Name.Contains("GT"))
                            {
                                tooltip.Show(result.Series.Name + ": CVU = " + prop.XValue + "; Disp = " + prop.YValues[0], this.chart1,
                                            pos.X, pos.Y - 15);
                            }


                        }
                    }
                }
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

            if (p.Length < 2) return p.First();

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

        private void RefreshViewSingle()
        {

            this.chart1.Series.Clear();
            this.chart1.ChartAreas.Clear();
            ChartArea nChart = new ChartArea();
            nChart.AxisX.Title = "CVU";
            nChart.AxisY.Title = "Disp";
            nChart.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            nChart.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            nChart.BackColor = Color.LightBlue;
            nChart.BackGradientStyle = GradientStyle.VerticalCenter;

            /// colocar scroll bar com zoom
            //int blockSize = 100;
            //nChart.AxisX.ScaleView.Zoomable = false;
            //nChart.AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            //int position = 0;
            //int size = blockSize;
            //nChart.AxisX.ScaleView.Zoom(position, size);
            //// disable zoom-reset button (only scrollbar's arrows are available)
            //nChart.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            //// set scrollbar small change to blockSize (e.g. 100)
            //nChart.AxisX.ScaleView.SmallScrollSize = blockSize;


            //nChart.CursorX.AutoScroll = true;


            ////
            chart1.ChartAreas.Add(nChart);

            this.chart1.ChartAreas[0].AxisX.Interval = 0;
            this.chart1.ChartAreas[0].AxisY.Interval = 0;

            this.chart1.Titles.Clear();
            this.chart1.Titles.Add(comboBox1.SelectedItem.ToString());

            var lista = GetResults(comboBox1.SelectedItem.ToString(), descolamento);

            var submercados = lista.Select(x => x.Item1).Distinct().ToList();
            // this.chart1.ChartAreas[0].AxisY.Maximum = lista.Sum(x => x.Item3);
            //foreach (var sub in submercados)
            // {

            //this.chart1.Series.Add(BuildSeries(sub, lista));
            this.chart1.Series.Add(BuildSeries("CT", lista));
            this.chart1.Series.Add(BuildSeriesGT("GT 1° Estágio", lista, comboBox1.SelectedItem.ToString()));

            //var intersectionPoints = chart1.Series[0].Points.Intersect(chart1.Series[1].Points);
            var intersectionPoints2 = chart1.Series[0].Points.Where(x => Math.Round(x.YValues[0],2) == chart1.Series[1].Points[0].YValues[0]).FirstOrDefault();//.Label = "Interseção: CVU = " + x.XValue + ", Disp = " + prop.YValues[0]

            if (intersectionPoints2 != null)
            {
                intersectionPoints2.Label = "Interseção: CVU = " + intersectionPoints2.XValue + "; Disp = " + Math.Round(intersectionPoints2.YValues[0],2);
                intersectionPoints2.IsValueShownAsLabel = true;
            }
            
            //this.chart1.Series[1].ToolTip = $"testes = #VALX, #VAL";
            ToolTip tool = new ToolTip();
            //tool.AutoPopDelay = 5000;
            //tool.InitialDelay = 1000;
            //tool.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            tool.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            //tool.SetToolTip(this.chart1, this.chart1.Series[1].ToolTip = $"testes = ffff#VALX, #VAL");
            //tool.SetToolTip(this.checkBox1, "My checkBox1");
            //}

        }
        private List<Tuple<string, double, double, double>> GetResults(string subs = null, bool descolamento = false)//subsist,cvu,disp,infl
        {


            List<Tuple<string, double, double, double>> lista = new List<Tuple<string, double, double, double>>();
            List<string> submercado = new List<string>();

            if(subs != "SIN")//if (descolamento)
            {
                subs.Split(',').ToList().ForEach(x => submercado.Add(x.Trim()));// submercado.Add(sub);
                //if (check_SE.Checked)
                //{
                //    submercado.Add("SE");
                //}
                //if (check_SUL.Checked)
                //{
                //    submercado.Add("S");
                //}
                //if (check_NE.Checked)
                //{
                //    submercado.Add("NE");
                //}
                //if (check_N.Checked)
                //{
                //    submercado.Add("N");
                //}


                foreach (var sist in submercado)
                {
                    switch (sist)
                    {
                        case "Sudeste":
                            ctSudeste.ForEach(x => lista.Add(new Tuple<string, double, double, double>("SE", x.cvu1, x.disp1, x.infl1)));
                            break;

                        case "Sul":
                            ctSul.ForEach(x => lista.Add(new Tuple<string, double, double, double>("S", x.cvu1, x.disp1, x.infl1)));
                            break;

                        case "Nordeste":
                            ctNordeste.ForEach(x => lista.Add(new Tuple<string, double, double, double>("NE", x.cvu1, x.disp1, x.infl1)));
                            break;

                        case "Norte":
                            ctNorte.ForEach(x => lista.Add(new Tuple<string, double, double, double>("N", x.cvu1, x.disp1, x.infl1)));

                            break;
                    }

                }
            }
            else
            {
                ctSudeste.ForEach(x => lista.Add(new Tuple<string, double, double, double>("SE", x.cvu1, x.disp1, x.infl1)));
                ctSul.ForEach(x => lista.Add(new Tuple<string, double, double, double>("S", x.cvu1, x.disp1, x.infl1)));
                ctNordeste.ForEach(x => lista.Add(new Tuple<string, double, double, double>("NE", x.cvu1, x.disp1, x.infl1)));
                ctNorte.ForEach(x => lista.Add(new Tuple<string, double, double, double>("N", x.cvu1, x.disp1, x.infl1)));

            }

            List<Tuple<string, double, double, double>> listaOrdenada = lista.OrderBy(x => x.Item2).ToList();



            return listaOrdenada;
        }

        private List<Tuple<string, int, double, string>> GetResultsMulti()
        {






            return null;
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

            var lista = GetResultsMulti();

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

        private Series BuildSeries(string sub, List<Tuple<string, double, double, double>> lista)//subsist,cvu,disp,infl
        {

            Series series1 = new Series();

            series1.LegendText = sub;
            series1.Name = sub;
            series1.XValueType = ChartValueType.Double;
            series1.YValueType = ChartValueType.Double;

            series1.YValuesPerPoint = 1;
            series1.ChartType = SeriesChartType.StepLine;
            series1.BorderWidth = 2;
            double soma = lista.Sum(x => x.Item4);
            series1.Points.AddXY(0, soma);

            foreach (var l in lista)
            {
                soma = soma + (l.Item3 - l.Item4);
                series1.Points.AddXY(l.Item2, soma);
            }

            return series1;
        }

        private Series BuildSeriesGT(string legend, List<Tuple<string, double, double, double>> lista, string subs)//subsist,cvu,disp,infl
        {

            Series series1 = new Series();

            series1.LegendText = legend;
            series1.Name = legend;
            series1.XValueType = ChartValueType.Double;
            series1.YValueType = ChartValueType.Double;

            series1.YValuesPerPoint = 1;
            series1.ChartType = SeriesChartType.Line;
            series1.BorderWidth = 2;
            double soma = 0;

            if (subs != "SIN")
            {
                foreach (var sub in subs.Split(',').ToList())
                {
                    switch (sub.Trim())
                    {
                        case "Sudeste":
                            soma += _results.Values.First().Sistemas[0].GerTerm;
                            break;

                        case "Sul":
                            soma += _results.Values.First().Sistemas[1].GerTerm;
                            break;

                        case "Nordeste":
                            soma += _results.Values.First().Sistemas[2].GerTerm;
                            break;

                        case "Norte":
                            soma += _results.Values.First().Sistemas[3].GerTerm;
                            break;
                    }
                }
            }
            else
            {
                for (int s = 1; s <= 4; s++)
                {
                    soma += _results.Values.First().Sistemas[s - 1].GerTerm;
                }
            }

            DataPoint point = new DataPoint();
            point.SetValueXY(0, soma);
           // point.IsValueShownAsLabel = true;
            //point.ToolTip = string.Format("{0}, {1}", 0, soma);

            series1.Points.Add(point);
            // series1.Points.AddXY(0, soma);

            foreach (var l in lista)
            {
                if (l == lista.Last())
                {
                    DataPoint lastPoint = new DataPoint();
                    lastPoint.SetValueXY(l.Item2, soma);
                    lastPoint.Label = "Geração Térmica = #VAL";
                    lastPoint.IsValueShownAsLabel = true;
                    //lastPoint.ToolTip = string.Format("{0}, {1}", 0, soma);
                    series1.Points.Add(lastPoint);

                }
                else
                {
                    series1.Points.AddXY(l.Item2, soma);
                }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshView();
        }
        private void check_se_CheckedChanged(object sender, EventArgs e)
        {
            check_N.Checked = false;
            check_SUL.Checked = false;
            check_NE.Checked = false;
            //check_SE.Checked = check_SE.Checked ? false : true;
            RefreshView();
        }
        private void check_sul_CheckedChanged(object sender, EventArgs e)
        {
            check_N.Checked = false;
            check_SE.Checked = false;
            check_NE.Checked = false;
            RefreshView();
        }
        private void check_ne_CheckedChanged(object sender, EventArgs e)
        {
            check_N.Checked = false;
            check_SUL.Checked = false;
            check_SE.Checked = false;
            RefreshView();
        }
        private void check_n_CheckedChanged(object sender, EventArgs e)
        {
            check_SE.Checked = false;
            check_SUL.Checked = false;
            check_NE.Checked = false;
            RefreshView();
        }


        public static void Show(String caption, CommomLibrary.Dadger.Dadger dadger, params Result[] results)
        {
            var frm = new FrmTermicasGraph(caption, dadger, results);



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
        public void alocaCts(CommomLibrary.Dadger.Dadger dadger)
        {
            var blocoCt = dadger.BlocoCT;
            foreach (var ct in blocoCt.Where(x => x.Estagio == 1 && x.Disp1 > 0).OrderBy(x => x.Cvu1).ToList())
            {
                ct dummy = new ct
                {
                    subsist = ct.Subsistema,
                    cvu1 = ct.Cvu1,
                    cvu2 = ct.Cvu2,
                    cvu3 = ct.Cvu3,
                    infl1 = ct.Infl1,
                    infl2 = ct.Infl2,
                    infl3 = ct.Infl3,
                    disp1 = ct.Disp1,
                    disp2 = ct.Disp2,
                    disp3 = ct.Disp3,
                    cod = ct.Cod,
                    est = ct.Estagio
                };

                switch (ct.Subsistema)
                {
                    case 1:
                        ctSudeste.Add(dummy);
                        break;
                    case 2:
                        ctSul.Add(dummy);
                        break;
                    case 3:
                        ctNordeste.Add(dummy);
                        break;
                    case 4:
                        ctNorte.Add(dummy);
                        break;

                }
            }
        }

        public class ct
        {
            public int cod { get; set; }
            public int est { get; set; }
            public int subsist { get; set; }
            public double cvu1 { get; set; }
            public double cvu2 { get; set; }
            public double cvu3 { get; set; }
            public double disp1 { get; set; }
            public double disp2 { get; set; }
            public double disp3 { get; set; }
            public double infl1 { get; set; }
            public double infl2 { get; set; }
            public double infl3 { get; set; }

        }

    }
}

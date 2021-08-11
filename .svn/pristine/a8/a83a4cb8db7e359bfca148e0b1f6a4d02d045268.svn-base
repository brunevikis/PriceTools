using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Compass.CommomLibrary;
using System.Windows.Forms.DataVisualization.Charting;

namespace Compass.DecompToolsShellX
{
    public partial class FrmGraphDp : Form
    {
        string diretorio = "";
        public FrmGraphDp(string dir, DateTime data, bool banco = false, float fator = 1f)
        {
            InitializeComponent();
            this.chart1.MouseMove += new MouseEventHandler(chart1_MouseMove);
            diretorio = dir;
            if (banco)
            {
                GeraGraphBanco(data, fator);
            }
            else
            {
                GeraGraphEntdados(diretorio, data);
            }
        }

        private void FrmGraphDp_Load(object sender, EventArgs e)
        {
           
        }

        private DateTime GetDeckDate(string dir)
        {
            var dadvaz = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz")).First();

            var dadlinhas = File.ReadAllLines(dadvaz).ToList();
            var dados = dadlinhas[9].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            DateTime dataEstudo = new DateTime(Convert.ToInt32(dados[3]), Convert.ToInt32(dados[2]), Convert.ToInt32(dados[1]));
            return dataEstudo;
        }

        private Compass.CommomLibrary.EntdadosDat.EntdadosDat GetEntdados(string dir)
        {
            var entdadosFile = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

            return entdados;
        }
        private void GeraGraphEntdados(string dir, DateTime data)
        {
            chart1.Invalidate();
            chart1.Series["SE"].Points.Clear();

            chart1.Series["S"].Points.Clear();
            chart1.Series["NE"].Points.Clear();
            chart1.Series["N"].Points.Clear();

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.Title = "Estagios";
            chart1.ChartAreas[0].AxisY.Title = "Carga";

            this.chart1.Series["SE"].BorderWidth = 2;
            this.chart1.Series["S"].BorderWidth = 2;
            this.chart1.Series["NE"].BorderWidth = 2;
            this.chart1.Series["N"].BorderWidth = 2;
            //var data = GetDeckDate(textOrigem.Text);
            //chart1.Invalidate();
            //chart1.Series["SE"].Points.Clear();

            //chart1.Series["S"].Points.Clear();
            //chart1.Series["NE"].Points.Clear();
            //chart1.Series["N"].Points.Clear();

            //chart1.ChartAreas[0].AxisX.Interval = 1;
            //chart1.ChartAreas[0].AxisX.Title = "Estagios";
            //chart1.ChartAreas[0].AxisY.Title = "Carga";
            chart1.Titles[0].Text = $"Entdados-{data:dd/MM/yyyy}";


            var entdados = GetEntdados(dir);
            List<Tuple<int, int, double>> dados = new List<Tuple<int, int, double>>();
            int hora = 0;
            foreach (var dp in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic.Trim()) == data.Day && x.Subsist == 1).ToList())
            {
                dados.Add(new Tuple<int, int, double>(dp.Subsist, hora, dp.Demanda));
                hora++;
            }
            hora = 0;
            foreach (var dp in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic.Trim()) == data.Day && x.Subsist == 2).ToList())
            {
                dados.Add(new Tuple<int, int, double>(dp.Subsist, hora, dp.Demanda));
                hora++;
            }
            hora = 0;

            foreach (var dp in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic.Trim()) == data.Day && x.Subsist == 3).ToList())
            {
                dados.Add(new Tuple<int, int, double>(dp.Subsist, hora, dp.Demanda));
                hora++;
            }
            hora = 0;

            foreach (var dp in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic.Trim()) == data.Day && x.Subsist == 4).ToList())
            {
                dados.Add(new Tuple<int, int, double>(dp.Subsist, hora, dp.Demanda));
                hora++;
            }

            //this.chart1.Series["SE"].BorderWidth = 2;
            //this.chart1.Series["S"].BorderWidth = 2;
            //this.chart1.Series["NE"].BorderWidth = 2;
            //this.chart1.Series["N"].BorderWidth = 2;
            string nomeSerie = "";
            foreach (var dd in dados)
            {
                switch (dd.Item1)
                {
                    case 1:
                        nomeSerie = "SE";
                        break;
                    case 2:
                        nomeSerie = "S";
                        break;
                    case 3:
                        nomeSerie = "NE";
                        break;
                    case 4:
                        nomeSerie = "N";
                        break;
                }

                this.chart1.Series[nomeSerie].Points.AddXY(dd.Item2, dd.Item3);
            }

        }

        private void GeraGraphBanco(DateTime data , double fator)
        {

            chart1.Titles[0].Text = $"Carga_Banco-{data:dd/MM/yyyy}";
            ///-----------------------------

            chart1.Invalidate();
            chart1.Series["SE"].Points.Clear();

            chart1.Series["S"].Points.Clear();
            chart1.Series["NE"].Points.Clear();
            chart1.Series["N"].Points.Clear();

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.Title = "Estagios";
            chart1.ChartAreas[0].AxisY.Title = "Carga";

            this.chart1.Series["SE"].BorderWidth = 2;
            this.chart1.Series["S"].BorderWidth = 2;
            this.chart1.Series["NE"].BorderWidth = 2;
            this.chart1.Series["N"].BorderWidth = 2;

            List<string> linhas = new List<string>();

            List<Tuple<DateTime, int, int, decimal?>> dadosCarga = new List<Tuple<DateTime, int, int, decimal?>>();

            Compass.CommomLibrary.IPDOEntitiesCargaDiaria CargaCtx = new IPDOEntitiesCargaDiaria();
            var cargas = CargaCtx.Carga_Diaria.Where(x => x.Data == data.Date).ToList();

            if (cargas.Count() > 0)
            {
                foreach (var cg in cargas)
                {
                    Tuple<DateTime, int, int, decimal?> cgDados = new Tuple<DateTime, int, int, decimal?>(cg.Data, cg.Hora, cg.Submercado, cg.Previsto);
                    dadosCarga.Add(cgDados);
                }

                for (int s = 1; s <= 4; s++)
                {
                    string nomeSerie = "";
                    
                    
                        switch (s)
                        {
                            case 1:
                                nomeSerie = "SE";
                                break;
                            case 2:
                                nomeSerie = "S";
                                break;
                            case 3:
                                nomeSerie = "NE";
                                break;
                            case 4:
                                nomeSerie = "N";
                                break;
                        }

                    
                    foreach (var car in dadosCarga.Where(x => x.Item3 == s).ToList())
                    {
                        double valor = (float)car.Item4 * fator;
                        this.chart1.Series[nomeSerie].Points.AddXY(car.Item2, valor);

                    }
                }
            }

            








            //---------------------------

            //var entdados = GetEntdados(dir);
            //List<Tuple<int, int, double>> dados = new List<Tuple<int, int, double>>();
            //int hora = 0;
            //foreach (var dp in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic.Trim()) == data.Day && x.Subsist == 1).ToList())
            //{
            //    dados.Add(new Tuple<int, int, double>(dp.Subsist, hora, dp.Demanda));
            //    hora++;
            //}
            //hora = 0;
            //foreach (var dp in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic.Trim()) == data.Day && x.Subsist == 2).ToList())
            //{
            //    dados.Add(new Tuple<int, int, double>(dp.Subsist, hora, dp.Demanda));
            //    hora++;
            //}
            //hora = 0;

            //foreach (var dp in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic.Trim()) == data.Day && x.Subsist == 3).ToList())
            //{
            //    dados.Add(new Tuple<int, int, double>(dp.Subsist, hora, dp.Demanda));
            //    hora++;
            //}
            //hora = 0;

            //foreach (var dp in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic.Trim()) == data.Day && x.Subsist == 4).ToList())
            //{
            //    dados.Add(new Tuple<int, int, double>(dp.Subsist, hora, dp.Demanda));
            //    hora++;
            //}


            //string nomeSerie = "";
            //foreach (var dd in dados)
            //{
            //    switch (dd.Item1)
            //    {
            //        case 1:
            //            nomeSerie = "SE";
            //            break;
            //        case 2:
            //            nomeSerie = "S";
            //            break;
            //        case 3:
            //            nomeSerie = "NE";
            //            break;
            //        case 4:
            //            nomeSerie = "N";
            //            break;
            //    }

            //    this.chart1.Series[nomeSerie].Points.AddXY(dd.Item2, dd.Item3);
            //}

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
                            tooltip.Show(result.Series.Name + ", Est=" + prop.XValue + ", Carga=" + prop.YValues[0], this.chart1,
                                            pos.X, pos.Y - 15);

                        }
                    }
                }
            }
        }
    }
}

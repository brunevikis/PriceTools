using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.ExcelTools.Templates {
    public class WorkbookRdh : IQueryable<Hidro> {

        Workbook wbRdh;
        Worksheet wsHidraulico;

        Tuple<int, int> inicioRange = new Tuple<int, int>(8, 3);
        Tuple<int, int> fimRange = new Tuple<int, int>(174, 28);

        public List<Hidro> RelatorioHidraulico { get; private set; }


        public WorkbookRdh(Workbook wbRdh) {

            this.wbRdh = wbRdh;
            RelatorioHidraulico = new List<Hidro>();
            wsHidraulico = wbRdh.Worksheets[1];


            LeHidraulico();

        }

        private void LeHidraulico() {


            for (int r = inicioRange.Item1; r <= fimRange.Item1; r++) {



                var rng = wsHidraulico.Range[
                    wsHidraulico.Cells[r, inicioRange.Item2],
                    wsHidraulico.Cells[r, fimRange.Item2]
                    ];

                var hidro = new Hidro(rng);

                if (string.IsNullOrWhiteSpace(hidro.Aproveitamento)) break;
                
                RelatorioHidraulico.Add(hidro);
            }

        }


        public IEnumerator<Hidro> GetEnumerator() {
            return RelatorioHidraulico.AsQueryable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return RelatorioHidraulico.AsQueryable().GetEnumerator();
        }

        public Type ElementType {
            get { return RelatorioHidraulico.AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression {
            get { return RelatorioHidraulico.AsQueryable().Expression; }
        }

        public IQueryProvider Provider {
            get { return RelatorioHidraulico.AsQueryable().Provider; }
        }
    }
    public class Hidro {



        public string Aproveitamento { get; set; }
        public int Posto { get; set; }
        public int VazaoMesAnterior { get; set; }
        public int VazaoMes { get; set; }
        public int VazaoSemana { get; set; }
        public int VazaoUltDias { get; set; }
        public int Vazao { get; set; }
        public double Nivel { get; set; }
        public double VolUtilArm { get; set; }
        public double VolEsp { get; set; }

        public Hidro(Range rng) {

            Aproveitamento = rng[1, 1].Text;
            {
                int val;
                if (int.TryParse(rng[1, 3].Text, out val)) Posto = val;
            }
            {
                int val;
                if (int.TryParse(rng[1, 4].Text, out val)) VazaoMesAnterior = val;
            }
            {
                int val;
                if (int.TryParse(rng[1, 6].Text, out val)) VazaoMes = val;
            }
            {
                int val;
                if (int.TryParse(rng[1, 8].Text, out val)) VazaoSemana = val;
            }
            {
                int val;

                if (int.TryParse(rng[1, 10].Text, out val)) VazaoUltDias = val;
            }
            {
                int val;
                if (int.TryParse(rng[1, 12].Text, out val)) Vazao = val;
            }
            {
                double val;
                if (double.TryParse(rng[1, 13].Text, out val)) Nivel = val;
            }
            {
                double val;
                if (double.TryParse(rng[1, 14].Text, out val)) VolUtilArm = val > 100 ? 100 : (val < 0 ? 0 : val);
            }
            {
                double val;
                if (double.TryParse(rng[1, 15].Text, out val)) VolEsp = val;
            }
        }

    }
}

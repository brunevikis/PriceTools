using Compass.CommomLibrary.Decomp;
using Compass.CommomLibrary.Prevs;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compass.ExcelTools;

namespace Compass.ExcelTools.Templates {
    public class WorkbookPrevsCenarios {

        public string DeckEntrada {
            get {

                return wb.GetWorksheet("Entrada").Cells[3, 3].Text;
            }
            set {
                wb.GetWorksheet("Entrada").Cells[3, 3].Value = value;
            }
        }
        public string DiretorioSaida {
            get {

                return wb.GetWorksheet("Entrada").Cells[5, 3].Text;
            }
        }

        public int Ano {
            get {

                return (int)wb.GetWorksheet("Entrada").Cells[4, 5].Value;
            }
            set {
                wb.GetWorksheet("Entrada").Cells[4, 5].Value = value;
            }
        }

        public int Mes {
            get {

                return (int)wb.GetWorksheet("Entrada").Cells[4, 3].Value;
            }
            set {
                wb.GetWorksheet("Entrada").Cells[4, 3].Value = value;
            }
        }

        public int Rev {
            get {

                return (int)wb.GetWorksheet("Entrada").Cells[4, 7].Value;
            }
            set {
                wb.GetWorksheet("Entrada").Cells[4, 7].Value = value;
            }
        }

        public Dictionary<string, Prevs> Cenarios {
            get {

                var result = new Dictionary<string, Prevs>();

                var wsPrev = wb.GetWorksheet("Prevs");

                for (int c = 3; c <= 7; c += 2) {

                    var cenName = wb.GetWorksheet("Entrada").Cells[15, c].Text;
                    if (!string.IsNullOrWhiteSpace(cenName)) {

                        Prevs prev = new Prevs();



                        var cs = 2 + 9 * (c - 1) / 2;
                        var ls = 7;

                        for (int l = ls; wsPrev.Cells[l, cs].Text != ""; l++) {

                            var line = prev.Blocos["Prev"].CreateLine();

                            for (int i = 0; i < 8; i++) {

                                line.SetValue(i, wsPrev.Cells[l, cs + i].Value);

                            }



                            prev.Blocos["Prev"].Add(line);
                        }


                        result.Add(
                            cenName,
                            prev);
                    }
                }



                return result;
            }
        }

        public Dictionary<string, object[,]> Enas {
            get {

                var result = new Dictionary<string, object[,]>();

                var wsSaida = wb.GetWorksheet("Saida");

                for (int c = 3; c <= 7; c += 2) {

                    var cenName = wb.GetWorksheet("Entrada").Cells[15, c].Text;
                    if (!string.IsNullOrWhiteSpace(cenName)) {
                        var cs = 2 + 9 * (c - 1) / 2;

                        var enas = wsSaida.Range[
                            wsSaida.Cells[8, cs],
                            wsSaida.Cells[11, cs + 5]
                            ].Value2;


                        result.Add(
                            cenName,
                            enas);
                    }
                }

                return result;
            }
        }

        Workbook wb;

        public WorkbookPrevsCenarios(Workbook xlWb) {
            wb = xlWb;
        }



    }
}

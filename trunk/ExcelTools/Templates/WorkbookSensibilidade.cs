using Compass.CommomLibrary.Decomp;
using Compass.CommomLibrary.Prevs;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compass.ExcelTools;

namespace Compass.ExcelTools.Templates {
    public class WorkbookSensibilidade {

        public static bool TryCreate(Workbook xlWb, out WorkbookSensibilidade w) {

            var ok = false;

            var names = new List<string>();
            foreach (Name name in xlWb.Names) {
                names.Add(name.Name);
            }

            ok =
                names.Contains("_DeckEntrada") &&
                names.Contains("_PastaSaída") &&
                names.Contains("_NumPassosCarga") &&
                names.Contains("_PassosCarga") &&
                names.Contains("_NumPassosEarm") &&
                names.Contains("_PassosEarm") &&
                names.Contains("_NumPassosVazoes") &&
                names.Contains("_PassosVazoes") &&
                names.Contains("_RodarVazoes") &&
                names.Contains("_CaminhoCortes");

            if (ok) {
                w = new WorkbookSensibilidade(xlWb);
            } else
                w = null;

            return ok;
        }

        public string DeckEntrada {
            get {

                return wb.Names.Item("_DeckEntrada").RefersToRange.Text;
            }
            set {
                wb.Names.Item("_DeckEntrada").RefersToRange.Value = value;
            }
        }

        public string DiretorioSaida {
            get {
                return wb.Names.Item("_PastaSaída").RefersToRange.Text;
            }
        }

        public int NumeroPassosCarga {
            get {
                int val;
                if (!int.TryParse(wb.Names.Item("_NumPassosCarga").RefersToRange.Text, out val)) val = 0;

                return val;
            }
        }

        public object[,] PassosCarga {
            get {

                var r = wb.Names.Item("_PassosCarga").RefersToRange.Value2;

                return r;
            }
        }

        public int NumeroPassosEamrI {
            get {
                int val;
                if (!int.TryParse(wb.Names.Item("_NumPassosEarm").RefersToRange.Text, out val)) val = 0;

                return val;
            }

        }

        public object[,] PassosEamrI {
            get {

                var r = wb.Names.Item("_PassosEarm").RefersToRange.Value2;

                return r;
            }
        }

        public int NumeroPassosVazoes {
            get {
                int val;
                if (!int.TryParse(wb.Names.Item("_NumPassosVazoes").RefersToRange.Text, out val)) val = 0;

                return val;
            }
        }

        public object[,] PassosVazoes {
            get {

                var r = wb.Names.Item("_PassosVazoes").RefersToRange.Value2;

                return r;
            }
        }

        Workbook wb;

        private WorkbookSensibilidade(Workbook xlWb) {
            wb = xlWb;
        }

        public bool RodarVazoes {
            get {
                return
                    wb.Names.Item("_RodarVazoes").RefersToRange.Value;
            }
        }

        public string CaminhoCortes {
            get {
                return wb.Names.Item("_CaminhoCortes").RefersToRange.Text;
            }
        }


        int col = 8;
        int r = 3;
        public void WriteCMO(string cen, CommomLibrary.Relato.RelatoCmoBlock relatoCmoBlock) {

            wb.Sheets["Entrada"].Cells[r, col + 0].Value = cen[0].ToString();
            wb.Sheets["Entrada"].Cells[r, col + 1].Value = cen[1].ToString();
            wb.Sheets["Entrada"].Cells[r, col + 2].Value = cen[2].ToString();

            for (int i = 0; i < relatoCmoBlock.Count; i++) {
                wb.Sheets["Entrada"].Cells[r, col + 3 + i].Value = relatoCmoBlock[i][1];
            }

            r++;
        }
    }
}

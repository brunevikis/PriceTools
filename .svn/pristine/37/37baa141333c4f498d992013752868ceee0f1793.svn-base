using Compass.CommomLibrary.Decomp;
using Compass.CommomLibrary.Prevs;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compass.ExcelTools;

namespace Compass.ExcelTools.Templates {
    public class WorkbookDiagramaOper : BaseWorkbook {

        public static bool TryCreate(Workbook xlWb, out WorkbookDiagramaOper w) {

            var ok = false;

            var names = GetNamesFrom(xlWb);

            ok = names.Contains("_dados_term_A");

            if (ok) {
                w = new WorkbookDiagramaOper(xlWb);
            } else
                w = null;

            return ok;
        }

        public string DecompBase {
            get {
                string n = null;

                foreach (Name name in Wb.Names) {
                    if (name.Name == "_DecompBase") {
                        n = name.RefersToRange.Value;
                        break;
                    }
                }

                return n;
            }
        }

        public Dictionary<int, int[]> Vazoes {
            get {

                foreach (Name name in Wb.Names) {
                    if (name.Name.Equals("_vazoes", StringComparison.OrdinalIgnoreCase)) {
                        var objarr = (object[,])name.RefersToRange.Value2;

                        var ret = new Dictionary<int, int[]>();
                        var l1 = objarr.GetLength(0);
                        var l2 = objarr.GetLength(1);

                        for (int p = 1; p <= l1; p++) {


                            if (objarr[p, 1] != null) {
                                var posto = Convert.ToInt32(objarr[p, 1]);
                                var vaz = new int[12];
                                for (int mes = 1; mes <= 12; mes++) {
                                    vaz[mes - 1] = Convert.ToInt32(objarr[p, mes + 1]);
                                }

                                ret.Add(posto, vaz);


                            } else
                                break;


                        }



                        return ret;
                    }
                }

                return null;

            }
        }

        public WorkbookDiagramaOper(Workbook xlWb)
            : base(xlWb) {
            Wb = xlWb;
        }

        public void Load(CommomLibrary.Relato.Relato relatoA, CommomLibrary.Relato.Relato relatoB, Compass.CommomLibrary.Result resultsA = null, Compass.CommomLibrary.Result resultsB = null) {

            Worksheet ws;
            ws = Wb.Worksheets["Oper_Inter"];
            ws.UsedRange.Clear();


            this.Names["_deckA"].Value = relatoA != null ? relatoA.File : "";
            this.Names["_deckB"].Value = relatoB != null ? relatoB.File : "";

            if (relatoA != null) {
                var r = 1;

                foreach (var item in relatoA.Intercambios.Where(x => x[1] == 1)) {
                    var c = 1;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Valores.Length - 1]].Value2 = item.Valores;
                    r++;
                }
            }

            if (relatoB != null) {
                var r = 1;

                foreach (var item in relatoB.Intercambios.Where(x => x[1] == 1)) {
                    var c = 27;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Valores.Length - 1]].Value2 = item.Valores;
                    r++;
                }
            }

            ws = Wb.Worksheets["Oper_Elet"];
            ws.UsedRange.Clear();

            if (relatoA != null) {
                var r = 1;

                foreach (var item in relatoA.BalancoEnergetico.Where(x => x[1] == 1)) {
                    var c = 1;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Valores.Length - 1]].Value2 = item.Valores;
                    r++;
                }
            }

            if (relatoB != null) {
                var r = 1;

                foreach (var item in relatoB.BalancoEnergetico.Where(x => x[1] == 1)) {
                    var c = 27;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Valores.Length - 1]].Value2 = item.Valores;
                    r++;
                }
            }

            ws = Wb.Worksheets["Oper_Hidr"];
            ws.UsedRange.Clear();

            if (relatoA != null) {
                var r = 1;

                foreach (var item in relatoA.EnergiaSistema.Where(x => x[1] == 1)) {
                    var c = 1;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Valores.Length - 1]].Value2 = item.Valores;
                    r++;
                }
            }

            if (relatoB != null) {
                var r = 1;

                foreach (var item in relatoB.EnergiaSistema.Where(x => x[1] == 1)) {
                    var c = 27;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Valores.Length - 1]].Value2 = item.Valores;
                    r++;
                }
            }

            ws = Wb.Worksheets["Oper_UH"];
            ws.UsedRange.Clear();

            if (relatoA != null) {
                var r = 1;

                foreach (var item in relatoA.Operacao/*.Where(x => x[1] == 1)*/) {
                    var c = 1;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Valores.Length - 1]].Value2 = item.Valores;
                    r++;
                }
            }

            if (relatoB != null) {
                var r = 1;

                foreach (var item in relatoB.Operacao/*.Where(x => x[1] == 1)*/) {
                    var c = 27;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Valores.Length - 1]].Value2 = item.Valores;
                    r++;
                }
            }

            ws = Wb.Worksheets["Oper_Term"];
            ws.UsedRange.Clear();

            Func<Compass.CommomLibrary.Relato.Relato, dynamic> getTerm = (rel) => {
                return (from dad in rel.DadosTerm
                        join oper in rel.OperTerm
                            on new { usina = dad[1], estagio = dad[3] } equals new { usina = oper[1], estagio = oper[2] } into dadoper
                        from dado in dadoper.DefaultIfEmpty()
                        where dad[3] == 1
                        select new object[] {

                        dad[0],
                        dad[1],
                        dad[2], 
                        dad[3],
                        dad[4],
                        dad[5],
                        dad[6], 
                        dad[7],
                        dad[8],
                        dad[9],
                        dad[10], 
                        dad[11],
                        dad[12],

                        (dado != null) ? dado[4] : 0,
                        (dado != null) ? dado[5] : 0,
                        (dado != null) ? dado[6] : 0,
                        (dado != null) ? dado[7] : 0,


                    }).ToArray();
            };

            if (relatoA != null) {
                var r = 1;

                foreach (var item in getTerm(relatoA)) {
                    var c = 1;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Length - 1]].Value2 = item;
                    r++;
                }
            }

            if (relatoB != null) {
                var r = 1;

                foreach (var item in getTerm(relatoB)) {
                    var c = 27;
                    ws.Range[ws.Cells[r, c], ws.Cells[r, c + item.Length - 1]].Value2 = item;
                    r++;
                }

            }

            var writeCMO = new Action<Compass.CommomLibrary.Result, int>((res, caso) => {
                var nre = Names["_inicioCMO"];

                var rowIdx = nre.Row;
                var colIdx = nre.Column;

                nre.Worksheet.Cells[rowIdx + 0, colIdx + 1 + caso].Value = res[1].Cmo_pat1;
                nre.Worksheet.Cells[rowIdx + 1, colIdx + 1 + caso].Value = res[1].Cmo_pat2;
                nre.Worksheet.Cells[rowIdx + 2, colIdx + 1 + caso].Value = res[1].Cmo_pat3;
                nre.Worksheet.Cells[rowIdx + 3, colIdx + 1 + caso].Value = res[1].Cmo;

                nre.Worksheet.Cells[rowIdx + 0, colIdx + 3 + caso].Value = res[2].Cmo_pat1;
                nre.Worksheet.Cells[rowIdx + 1, colIdx + 3 + caso].Value = res[2].Cmo_pat2;
                nre.Worksheet.Cells[rowIdx + 2, colIdx + 3 + caso].Value = res[2].Cmo_pat3;
                nre.Worksheet.Cells[rowIdx + 3, colIdx + 3 + caso].Value = res[2].Cmo;

                nre.Worksheet.Cells[rowIdx + 0, colIdx + 5 + caso].Value = res[3].Cmo_pat1;
                nre.Worksheet.Cells[rowIdx + 1, colIdx + 5 + caso].Value = res[3].Cmo_pat2;
                nre.Worksheet.Cells[rowIdx + 2, colIdx + 5 + caso].Value = res[3].Cmo_pat3;
                nre.Worksheet.Cells[rowIdx + 3, colIdx + 5 + caso].Value = res[3].Cmo;

                nre.Worksheet.Cells[rowIdx + 0, colIdx + 7 + caso].Value = res[4].Cmo_pat1;
                nre.Worksheet.Cells[rowIdx + 1, colIdx + 7 + caso].Value = res[4].Cmo_pat2;
                nre.Worksheet.Cells[rowIdx + 2, colIdx + 7 + caso].Value = res[4].Cmo_pat3;
                nre.Worksheet.Cells[rowIdx + 3, colIdx + 7 + caso].Value = res[4].Cmo;
            });

            if (resultsA != null) writeCMO(resultsA, 0);
            if (resultsB != null) writeCMO(resultsB, 1);

            #region Limites Elétricos

            /*
             * patamar [1]
             * usina [3]
             * l inf [8]
             * l sup [9]
             * Prod [6]
             * Obs [10]
            */

            //1+5+5

            List<dynamic[]> reA;
            List<dynamic[]> reB;

            if (relatoA != null) {
                reA = relatoA.RestricaoEletrica.Where(x => x[0] == 1)
                    .Select(x => new dynamic[] { x[1], x[3].ToString().Trim(), x[8], x[9], x[6], x[10] }).ToList();
            } else reA = new List<dynamic[]>();


            if (relatoB != null) {
                reB = relatoB.RestricaoEletrica.Where(x => x[0] == 1)
                    .Select(x => new dynamic[] { x[1], x[3].ToString().Trim(), x[8], x[9], x[6], x[10] }).ToList();
                //.Select(x => new { patamar = x[1], usina = x[3], inf = x[8], sup = x[9], valor = x[6], obs = x[10] });
            } else reB = new List<dynamic[]>();

            Action<int> reByPat = (int pat) => {

                List<dynamic[]> reJ = new List<dynamic[]>();

                var reA1 = reA.Where(x => (int)x[0] == pat).Where(x => !string.IsNullOrWhiteSpace((string)x[5]));
                var reB1 = reB.Where(x => (int)x[0] == pat).Where(x => !string.IsNullOrWhiteSpace((string)x[5]));

                foreach (var reUsi in reA1.Select(x => x[1]).Union(reB1.Select(x => x[1]).Distinct())) {

                    var reJn = new dynamic[9];

                    reJn[0] = reUsi;
                    if (reA1.Any(x => x[1] == reUsi)) {
                        var reA2 = reA1.First(x => x[1] == reUsi);

                        reJn[1] = reA2[2];
                        reJn[2] = reA2[3];
                        reJn[3] = reA2[4];
                        reJn[4] = reA2[5];
                    }
                    if (reB1.Any(x => x[1] == reUsi)) {
                        var reB2 = reB1.First(x => x[1] == reUsi);

                        reJn[5] = reB2[2];
                        reJn[6] = reB2[3];
                        reJn[7] = reB2[4];
                        reJn[8] = reB2[5];
                    }
                    reJ.Add(reJn);
                }

                var nre = Names["_inicio_restricao_elet_pat" + pat.ToString()];

                var nreRng = nre.Worksheet.Range[
                    nre.Worksheet.Cells[nre.Row, nre.Column],
                    nre.Worksheet.Cells[nre.Row + reJ.Count - 1, nre.Column + 8]
                ];


                var nreRngVal = new dynamic[reJ.Count, 9];
                for (int i = 0; i < reJ.Count; i++) {
                    for (int j = 0; j < 9; j++) {
                        nreRngVal[i, j] = reJ[i][j];
                    }
                }
                nreRng.Value2 = nreRngVal;

            };

            reByPat(1);
            reByPat(2);
            reByPat(3);

            #endregion


            LoadOfertaTerm(relatoA, relatoB);

        }

        private void LoadOfertaTerm(CommomLibrary.Relato.Relato relatoA, CommomLibrary.Relato.Relato relatoB) {


            Action<CommomLibrary.Relato.Relato, string> exec = (relato, k) => {
                if (relato != null) {
                    var re = relato.DadosTerm
                    .Where(x => x[3] == 1)
                    .Select(x => { var z = x.Valores.ToList(); z.RemoveAt(3); return z.ToArray(); }).ToList();

                    var nre = Names["_dados_term_" + k];

                    var nreRng = nre.Worksheet.Range[
                        nre.Worksheet.Cells[nre.Row, nre.Column],
                        nre.Worksheet.Cells[nre.Row + re.Count - 1, nre.Column + re.First().Length - 1]
                    ];

                    var nreRngVal = new dynamic[re.Count, re.First().Length];
                    for (int i = 0; i < re.Count; i++) {
                        for (int j = 0; j < re[i].Length; j++) {
                            nreRngVal[i, j] = re[i][j];
                        }
                    }
                    nreRng.Value2 = nreRngVal;
                }
            };


            exec(relatoA, "A");
            exec(relatoB, "B");

            //this.OfertaTermA = qA.Select(x => new object[] { x[0], x[1], x[2], x[4], x[5], x[6], x[7], x[8], x[9], x[10], x[11], x[12] }).ToArray();
        }

        public object[][] OfertaTermA {
            set {

                var l = 1;
                var c = 1;

                foreach (var i in value) {

                    foreach (var j in i) {
                        Names["_dados_term_A"].Value2[l, c] = j;
                        c++;
                    }
                    l++;
                    c = 1;
                }
            }
        }
    }
}
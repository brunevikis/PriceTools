﻿using Compass.CommomLibrary.Decomp;
using Compass.CommomLibrary.Prevs;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compass.ExcelTools;

namespace Compass.ExcelTools.Templates {
    public class WorkbookPrevsCenariosMen : BaseWorkbook {

        public enum TipoCen {
            Mensal,
            Semanal
        }
        
        public TipoCen Tipo {
            get {
                var val = Names["_tipo"].Value.ToString();

                if (val == "Mensal") return TipoCen.Mensal;
                else if (val == "Semanal") return TipoCen.Semanal;
                else throw new ArgumentException();
            }
            set {
                Names["_tipo"].Value = value.ToString();
            }
        }

        public object[,] EnasReesCen1 {
            get {
                return Names["_enasReeCen1"].Value2;
            }
        }

        public object[,] EnasReesCen1Media {
            get {
                return Names["_enasReeCen1Media"].Value2;
            }
        }

        public object[,] PrevsCen1 { get { return Names["_cen1"].Value; } }

        public object[,] Entrada { get { return Names["_entrada"].Value; } set { Names["_entrada"].Value = value; } }
        public object[,] Saida1 { get { return Names["_saida1"].Value; } set { Names["_saida1"].Value = value; } }
        public object[,] Saida2 { get { return Names["_saida2"].Value; } set { Names["_saida2"].Value = value; } }
        public object[,] Saida3 { get { return Names["_saida3"].Value; } set { Names["_saida3"].Value = value; } }
        public object[,] Saida4 { get { return Names["_saida4"].Value; } set { Names["_saida4"].Value = value; } }
        public object[,] Saida5 { get { return Names["_saida5"].Value; } set { Names["_saida5"].Value = value; } }

        public object[,] SemanasPrevs {
            get {
                return Names["_semanasPrevs"].Value;
            }
        }

        public object[,] Fatores { get { return Names["_fatores"].Value; } set { Names["_fatores"].Value = value; } }
        
        public string ArquivosDeEntrada {
            get {
                return Names["_entradaPrevivaz"].Value.ToString();
            }
        }

        public void setSaida(int p, object[] vals) {
            object[,] vt = new object[1, vals.Length];

            for (int i = 0; i < vals.Length; i++) {
                vt[0, i] = vals[i];
            }

            Names["_saida1"].Range[Names["_saida1"][p - 1, 1], Names["_saida1"][p - 1, vals.Length]].Value2 = vt;
        }

        public int SemanaAtual {
            get {
                return (int)Names["_semanaAtual"].Value;
            }
        }

        public int AnoAtual {
            get {
                return (int)Names["_ano"].Value;
            }
        }
        public int MesAtual {
            get {
                return (int)Names["_mes"].Value;
            }
            set {
                Names["_mes"].Value2 = value;
            }
        }

        public WorkbookPrevsCenariosMen(Workbook xlWb)
            : base(xlWb) {

        }

        public void LoadPrevs(string prevsPath) {

            try {

                Names["_partifCen1"].Value = "C";

                var pr = (Prevs)Compass.CommomLibrary.DocumentFactory.Create(prevsPath);

                var rng = Names["_entrada"];
                rng.ClearContents();


                var vazarr = new object[320, 14];

                foreach (var p in pr.Vazoes) {
                    vazarr[(int)p[1] - 1, 0] = (int)p[1];

                    vazarr[(int)p[1] - 1, 2] = (int)p[2];
                    vazarr[(int)p[1] - 1, 3] = (int)p[3];
                    vazarr[(int)p[1] - 1, 4] = (int)p[4];
                    vazarr[(int)p[1] - 1, 5] = (int)p[5];
                    vazarr[(int)p[1] - 1, 6] = (int)p[6];
                    vazarr[(int)p[1] - 1, 7] = (int)p[7];
                }

                rng.Value2 = vazarr;

            } finally {
            }
        }

        public void ResetFatores() {
            Names["_fatores"].Value = 1;
        }

        public bool Regressoes {
            set { Names["_regrFut"].Value = value; }
            get { return Names["_regrFut"].Value; }

        }
    }
}

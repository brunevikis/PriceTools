using Compass.CommomLibrary;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.ExcelTools.Templates {
    public class Infosheet {

        Worksheet ws;
        public Worksheet WS { get { return ws; } }

        public static string Key = "Info";

        public string DocPath { get { return ws.Cells[2, 2].Text; } set { ws.Cells[2, 2].Value = value; } }
        public string DocType { get { return ws.Cells[1, 2].Text; } set { ws.Cells[1, 2].Value = value; } }

        public Infosheet(Worksheet xlWs) {
            ws = xlWs;
        }

        public void Initialize() {
            ws.Cells[1, 1].Value = "Tipo";
            ws.Cells[2, 1].Value = "Caminho Original";

            ws.Cells[1, 7].Value = "Sistema";
            ws.Cells[1, 8].Value = "Earm MAX";
            ws.Cells[1, 9].Value = "Earm";
            ws.Cells[1, 10].Value = "Meta (EARM ou %)";

            ws.Cells[1, 14].Value = "ENA";
        }

        public string BottonComments { get { return ws.Cells[30, 2].Value; } set { ws.Cells[30, 2].Value = value; } }

        public string[] Sistemas {
            get {
                List<string> vals = new List<string>();
                for (int i = 0; !string.IsNullOrWhiteSpace(ws.Cells[2 + i, 7].Text); i++) {

                    var cellValue = ws.Cells[2 + i, 7].Text;
                    vals.Add(cellValue);

                }
                return vals.ToArray();
            }
            set {
                for (int i = 0; i < value.Length; i++) {
                    ws.Cells[2 + i, 7].Value = value[i];
                }
            }
        }

        public double[] EarmMax {
            get {
               var vals = new List<double>();
                for (int i = 0; ws.Cells[2 + i, 8].Value != null; i++) {

                    var cellValue = ws.Cells[2 + i, 8].Value;
                    vals.Add(Convert.ToSingle(cellValue));

                }
                if (vals.Count > 0) {
                    return vals.ToArray();
                } else
                    return null;
            }
            set {

                for (int i = 0; i < value.Length; i++) {
                    ws.Cells[2 + i, 8].Value = value[i];
                }
            }
        }

        public double[] MetaReservatorio {
            get {
                var vals = new List<double>();
                for (int i = 0; ws.Cells[2 + i, 10].Value != null; i++) {

                    var cellValue = ws.Cells[2 + i, 10].Value;
                    vals.Add(Convert.ToSingle(cellValue));

                }
                return vals.ToArray();
            }
            set {

                for (int i = 0; i < value.Length; i++) {
                    ws.Cells[2 + i, 10].Value = value[i];
                }
            }
        }

        public double[] Earm {
            get {
                List<double> vals = new List<double>();
                for (int i = 0; ws.Cells[2 + i, 9].Value != null; i++) {

                    var cellValue = ws.Cells[2 + i, 9].Value;
                    vals.Add(Convert.ToSingle(cellValue));

                }
                return vals.ToArray();
            }
            set {

                for (int i = 0; i < value.Length; i++) {
                    ws.Cells[2 + i, 9].Value = value[i];
                }
            }
        }

        public float[][] Ena {
            set {
                for (int sem = 0; sem < 6; sem++) {
                    for (int i = 0; i < value.GetLength(0); i++) {
                        ws.Cells[2 + i, 15 + sem].Value = value[i][sem];
                    }
                }
            }
        }


        public void Show() {
            typeof(_Worksheet).InvokeMember("Activate", System.Reflection.BindingFlags.InvokeMethod, null, ws, null);
            //ws.Activate();
        }
    }
}

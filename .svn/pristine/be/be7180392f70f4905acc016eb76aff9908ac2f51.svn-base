using Compass.CommomLibrary;
using Compass.ExcelTools.Templates;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.ExcelTools {

    public static class ExcelExtensions {

        static public int commentColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray);
        static public int compareColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
        static public int errorColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);


        public static Range GetRange(this Application app) {
            return (Range)app.InputBox("Select Range", "Decomp Tools", "A1:A1", Type: 8);
        }

        public static Worksheet GetOrCreateWorksheet(this Workbook xlWb, string name, bool addToEnd = false) {
            Worksheet xlWS = GetWorksheet(xlWb, name);

            if (xlWS == null) {

                if (addToEnd) {
                    var lastWs = xlWb.Worksheets[xlWb.Worksheets.Count - 1];
                    xlWS = (Worksheet)xlWb.Sheets.Add(After: lastWs);
                } else {

                    xlWS = (Worksheet)xlWb.Sheets.Add();
                }
                xlWS.Name = name;
            }

            return xlWS;
        }

        public static Worksheet GetWorksheet(this Workbook xlWb, string name) {
            Worksheet xlWS = null;

            foreach (var ws in xlWb.Sheets) {
                if (ws is Worksheet) {


                    if (((Worksheet)ws).Name == name) { xlWS = (Worksheet)ws; break; }
                }
            }

            return xlWS;
        }

        public static BaseDocument LoadDocumentFromWorkbook(this Workbook xlWb, string docType, string blockKey = null) {
            switch (docType.ToLowerInvariant()) {
                case "hidrdat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.HidrDat.HidrDat>(xlWb, blockKey);
                case "postosdat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.PostosDat.PostosDat>(xlWb, blockKey);
                case "dadger":
                    return LoadDadxxxFromWorkbook<Compass.CommomLibrary.Dadger.Dadger>(xlWb, blockKey);
                case "entdadosdat":
                    return LoadDadxxxFromWorkbook<Compass.CommomLibrary.EntdadosDat.EntdadosDat>(xlWb, blockKey);
                case "mltdat":
                    return LoadDadxxxFromWorkbook<Compass.CommomLibrary.MltDat.MltDat>(xlWb, blockKey);
                case "dadgnl":
                    return LoadDadxxxFromWorkbook<Compass.CommomLibrary.Dadgnl.Dadgnl>(xlWb, blockKey);
                case "sistemadat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.SistemaDat.SistemaDat>(xlWb, blockKey);
                case "patamardat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.PatamarDat.PatamarDat>(xlWb, blockKey);
                case "c_adicdat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.C_AdicDat.C_AdicDat>(xlWb, blockKey);
                case "dgernwd":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.DgerNwd.DgerNwd>(xlWb, blockKey);
                case "eafpast":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.Eafpast.Eafpast>(xlWb, blockKey);
                case "manuttdat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.ManuttDat.ManuttDat>(xlWb, blockKey);
                case "termdat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.TermDat.TermDat>(xlWb, blockKey);
                case "confhddat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.ConfhdDat.ConfhdDat>(xlWb, blockKey);
                case "clastdat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.ClastDat.ClastDat>(xlWb, blockKey);
                case "exptdat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.ExptDat.ExptDat>(xlWb, blockKey);
                case "vazpast":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.Vazpast.Vazpast>(xlWb, blockKey);
                case "prevs":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.Prevs.Prevs>(xlWb, blockKey);
                case "agrintdat":
                    return LoadSistemaDatFromWorkbook<Compass.CommomLibrary.AgrintDat.AgrintDat>(xlWb, blockKey);
                case "vazoesc":
                    var text = xlWb.LoadCSVFromWorkbook();
                    return new Compass.CommomLibrary.VazoesC.VazoesC(text);
                default:
                    return null;
            }
        }

        static T LoadDadxxxFromWorkbook<T>(this Workbook xlWb, string blockKey = null) where T : BaseDocument {
            var doc = Activator.CreateInstance<T>();

            foreach (var block in doc.Blocos) {
                var xlWs = xlWb.GetWorksheet(block.Key);


                if (xlWs != null && (blockKey == null || xlWs.Name == blockKey)) {

                    var rs = 2;

                    var range = xlWs.UsedRange;
                    //var end = range.End[XlDirection.xlDown].End[XlDirection.xlToRight];
                    var re = range.Rows.Count;
                    //var ce = end.Column;
                    string comment = null;
                    for (var r = rs; r <= re; r++) {


                        var dummyLine = xlWs.Cells[r, 1].Text;

                        if (doc.IsComment(dummyLine)) {
                            comment = comment == null ? dummyLine : comment + Environment.NewLine + dummyLine;
                        } else if (xlWs.Cells[r, 1].Text == "") {
                            continue;
                        } else {

                            //AJUSTE POR CAUSA DO BLOCO AC
                            if ((doc is Compass.CommomLibrary.Dadger.Dadger && block.Value is Compass.CommomLibrary.Dadger.AcBlock) ||
                                (doc is Compass.CommomLibrary.EntdadosDat.EntdadosDat && block.Value is Compass.CommomLibrary.EntdadosDat.AcBlock)) {

                                dummyLine = ((string)xlWs.Cells[r, 3].Value.ToString()).PadLeft(15);

                            }

                            var newLine = (BaseLine)block.Value.CreateLine(dummyLine);

                            var rng = xlWs.Range[xlWs.Cells[r, 1], xlWs.Cells[r, newLine.Campos.Length]];
                            var vals = rng.Value2;

                            for (int c = 0; c < newLine.Campos.Length; c++) {
                                newLine.SetValue(c, vals[1, c + 1]);
                            }

                            if (!string.IsNullOrWhiteSpace(comment)) {
                                newLine.Comment = comment;
                                comment = null;
                            }
                            block.Value.Add(newLine);
                        }
                    }
                }
            }

            return doc;
        }

        static T Backup_LoadDadxxxFromWorkbook<T>(this Workbook xlWb, string blockKey = null) where T : BaseDocument {
            var doc = Activator.CreateInstance<T>();

            foreach (var block in doc.Blocos) {
                var xlWs = (Microsoft.Office.Interop.Excel.Worksheet)xlWb.Worksheets[block.Key];


                if (xlWs != null && (blockKey == null || xlWs.Name == blockKey)) {
                    var r = 1;

                    while (block.Key.Split(' ').Any(k => k == xlWs.Cells[r + 1, 1].Value)) {

                        var dummyLine = xlWs.Cells[r + 1, 1].Value.ToString();

                        //AJUSTE POR CAUSA DO BLOCO AC
                        if (doc is Compass.CommomLibrary.Dadger.Dadger && block.Value is Compass.CommomLibrary.Dadger.AcBlock) {

                            dummyLine = ((string)xlWs.Cells[r + 1, 3].Value.ToString()).PadLeft(15);

                        }

                        var newLine = block.Value.CreateLine(dummyLine);

                        for (int c = 0; c < newLine.Campos.Length; c++) {
                            newLine.SetValue(c, xlWs.Cells[r + 1, c + 1].Value);
                        }

                        block.Value.Add(newLine);
                        r++;
                    }
                }
            }

            return doc;
        }

        static T LoadSistemaDatFromWorkbook<T>(this Workbook xlWb, string blockKey = null) where T : BaseDocument {
            var doc = Activator.CreateInstance<T>();

            foreach (var block in doc.Blocos) {
                var xlWs = (Microsoft.Office.Interop.Excel.Worksheet)xlWb.Worksheets[block.Key];

                if (xlWs != null && (blockKey == null || xlWs.Name == blockKey)) {


                    if (block.Value is Compass.CommomLibrary.SistemaDat.PatBlock) {
                        var newLine = block.Value.CreateLine();
                        newLine.SetValue(0, xlWs.Cells[2, 1].Text);
                        block.Value.Add(newLine);
                    } else {
                        var rs = 2;

                        var range = xlWs.UsedRange;
                        //var end = range.End[XlDirection.xlDown].End[XlDirection.xlToRight];
                        var re = range.Rows.Count + 1;
                        //var ce = end.Column;

                        for (var r = rs; r <= re; r++) {

                            var cellVal = xlWs.Cells[r, 1].Text;

                            if (string.IsNullOrEmpty(cellVal) && r == re) continue;

                            var newLine = (BaseLine)block.Value.CreateLine(cellVal);

                            if (newLine.Campos.Length > 1) {


                                var rng = xlWs.Range[xlWs.Cells[r, 1], xlWs.Cells[r, newLine.Campos.Length]];

                                var vals = rng.Value2;


                                for (int c = 0; c < newLine.Campos.Length; c++) {
                                    //    newLine.SetValue(c, xlWs.Cells[r, c + 1].Value);
                                    newLine.SetValue(c, vals[1, c + 1]);
                                }

                            } else {
                                newLine.SetValue(0, xlWs.Cells[r, 1].Value);
                            }

                            block.Value.Add(newLine);
                        }
                    }
                }
            }

            return doc;
        }

        public static string LoadCSVFromWorkbook(this Workbook xlWb) {

            var xlWs = (Worksheet)xlWb.Sheets[1];

            var range = xlWs.UsedRange;
            var end = range.End[XlDirection.xlDown].End[XlDirection.xlToRight];

            var er = end.Row;
            var ec = end.Column;


            var readRange = xlWs.Range[xlWs.Cells[1, 1], end];
            var arr = readRange.Value2;


            var sb = new StringBuilder();
            for (int r = 1; r <= er; r++) {
                for (int c = 1; c <= ec; c++) {

                    sb.Append(arr[r, c].ToString());
                    sb.Append(";");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }



            return sb.ToString();


        }

        public static void WriteDocumentToWorkbook(this Workbook xlWb, BaseDocument doc, bool secundary = false) {
            if (doc is Compass.CommomLibrary.VazoesC.VazoesC) {
                WriteVazoesCToWorkbook(xlWb, (Compass.CommomLibrary.VazoesC.VazoesC)doc);
            } else {
                WriteOtherThanVazoesCToWorkbook(xlWb, doc, secundary, true);
            }
        }

        public static void WriteVazoesCToWorkbook(this Workbook xlWb, Compass.CommomLibrary.VazoesC.VazoesC doc) {

            //var dataAtual = new DateTime(doc.AnoFinal, doc.MesFinal, 1);
            //var dataSeg = dataAtual.AddMonths(1);

            var text = doc.ToContentString();

            Worksheet xlWs = xlWb.GetOrCreateWorksheet(doc.Blocos.First().Key);

            xlWs.WriteCSVToWorkbook(text);
        }

        public static void WriteOtherThanVazoesCToWorkbook(this Workbook xlWb, BaseDocument doc, bool secundary = false, bool fieldNames = false) {

            foreach (var block in doc.Blocos.Reverse()) {


                Worksheet xlWs = secundary ? xlWb.GetWorksheet(block.Key) : xlWb.GetOrCreateWorksheet(block.Key);


                if (block.Value is Compass.CommomLibrary.Dadger.DummyBlock) {
                    if (secundary)
                        continue;
                    else
                        xlWs.Visible = XlSheetVisibility.xlSheetHidden;
                }

                if (xlWs == null) {
                    continue;
                }

                var arr = block.Value.ToArray();

                int colShift = 0;
                if (secundary) {
                    colShift = arr.Max(l => l.Campos.Length) + 1;
                }

                int usedCols = arr.Length > 0 ? arr.Max(l => l.Campos.Length) : 1;
                var cleanRng = xlWs.Range[xlWs.Cells[1, 1 + colShift], xlWs.Cells[10000, usedCols + colShift]];
                ((Range)cleanRng).Clear();


                //prepare data matrix
                var rowLenght = arr.Length + 1 + arr.Sum(x =>
                    !string.IsNullOrWhiteSpace(x.Comment) ?
                    x.Comment.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length
                    : 0);

                var data = new object[rowLenght, usedCols];

                var formR1C1 = "=L[0]C[" + colShift.ToString() + "]";

                if (fieldNames && arr.Length > 0) {
                    for (int j = 0; j < arr[0].Campos.Length; j++) {
                        data[0, j] = arr[0].Campos[j].Nome;
                        //xlWs.Cells[1, j + 1 + colShift].Value = arr[0].Campos[j].Nome;
                    }
                }


                int r = 2;
                for (int i = 0; i < arr.Length; i += 1, r += 1) {


                    if (!string.IsNullOrWhiteSpace(arr[i].Comment)) {

                        var comms = arr[i].Comment.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        var commValues = new string[comms.Length, 1];

                        for (int cind = 0; cind < comms.Length; cind++) {
                            commValues[cind, 0] = comms[cind];
                            data[r - 1 + cind, 0] = comms[cind];
                        }

                        var rngCom = xlWs.Range[xlWs.Cells[r, 1 + colShift], xlWs.Cells[r + comms.Length - 1, 1 + colShift]];
                        //rngCom.Value2 = commValues;
                        ((Range)rngCom).Font.Color = commentColor;

                        r += comms.Length;
                    }
                    for (int j = 0; j < arr[i].Campos.Length; j++) {
                        if (arr[i].Campos[j].Formato.StartsWith("A") && block.Value is Compass.CommomLibrary.Dadger.DummyBlock) {
                            //    xlWs.Cells[r, j + 1 + colShift].NumberFormat = "@";
                            data[r - 1, j] = "'" + arr[i][j].ToString(); //force string value to preserv surrounding spaces                            
                        } else {
                            data[r - 1, j] = arr[i][j];
                        }

                        //xlWs.Cells[i + 2, j + 1 + colShift].Value = arr[i].Valores[j];

                        if (secundary) {
                            var fcs = ((Range)xlWs.Cells[r, j + 1]).FormatConditions;
                            var fc = fcs.Add(XlFormatConditionType.xlCellValue, XlFormatConditionOperator.xlNotEqual, formR1C1);
                            fc.Interior.Color = compareColor;
                        }
                    }
                    //var rng = xlWs.Range[xlWs.Cells[r, 1 + colShift], xlWs.Cells[r, arr[i].Campos.Length + colShift]];
                    //rng.Value2 = arr[i].Valores;
                }

                var rng = xlWs.Range[xlWs.Cells[1, 1 + colShift], xlWs.Cells[r - 1, usedCols + colShift]];
                rng.Value2 = data;
            }
        }

        public static void WriteCSVToWorkbook(this Worksheet xlWs, string csvText) {

            var startCell = (Range)xlWs.Cells[1, 1];

            var lines = csvText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int rc = lines.Length;
            int cc = lines[0].Split(';').Length;


            object[,] data = new object[lines.Length, lines[0].Split(';').Length];

            for (int l = 0; l < rc; l++) {
                var line = lines[l].Split(';');
                for (int c = 0; c < cc; c++) {
                    data[l, c] = line[c];
                }
            }

            var endCell = (Range)xlWs.Cells[rc, cc];
            var writeRange = xlWs.Range[startCell, endCell];

            writeRange.Value2 = data;

        }

        public static Infosheet SetInfosheet(this Workbook xlWb, BaseDocument doc) {

            string key = Infosheet.Key;
            var xlWs = xlWb.GetOrCreateWorksheet(key);

            var infoSheet = new Infosheet(xlWs);
            infoSheet.Initialize();
            infoSheet.DocType = doc.GetType().Name;
            infoSheet.DocPath = doc.File;

            if (doc is Compass.CommomLibrary.Dadger.Dadger) {
                var sistemas = ((Compass.CommomLibrary.Dadger.Dadger)doc).BlocoSb.Select(x => (string)x[2]).ToArray();
                infoSheet.Sistemas = sistemas;
            }
            else if (doc is Compass.CommomLibrary.EntdadosDat.EntdadosDat)
            {
                var sistemas = ((Compass.CommomLibrary.EntdadosDat.EntdadosDat)doc).BlocoSist.Select(x => (string)x.Sigla).ToArray();
                infoSheet.Sistemas = sistemas;
            }

            return infoSheet;
        }

        public static Infosheet GetInfosheet(this Workbook xlWb) {
            string key = Infosheet.Key;
            var xlWs = xlWb.GetWorksheet(key);

            Infosheet info = null;

            if (xlWs != null)
                info = new Infosheet(xlWs);

            return info;
        }

        public static Infosheet ToInfosheet(this Worksheet xlWs) {
            string key = Infosheet.Key;
            Infosheet info = null;

            if (xlWs != null && xlWs.Name.StartsWith(key))
                info = new Infosheet(xlWs);

            return info;
        }

        public static RvxPlus1Configsheet SetRvxPlus1Configsheet(this Workbook xlWb, string caseName = "") {

            string key = RvxPlus1Configsheet.Key;

            if (!string.IsNullOrWhiteSpace(caseName)) {

                key += " - " + caseName;
            }

            var xlWs = xlWb.GetOrCreateWorksheet(key);

            var rvxSheet = new RvxPlus1Configsheet(xlWs);
            rvxSheet.Initialize();


            return rvxSheet;
        }

        public static RvxPlus1Configsheet GetRvxPlus1Configsheet(this Workbook xlWb, string caseName = "") {
            string key = RvxPlus1Configsheet.Key;
            if (!string.IsNullOrWhiteSpace(caseName)) {

                key += " - " + caseName;
            }
            var xlWs = xlWb.GetWorksheet(key);

            RvxPlus1Configsheet info = null;

            if (xlWs != null)
                info = new RvxPlus1Configsheet(xlWs);

            return info;
        }

        public static RvxPlus1Configsheet ToRvxPlus1Configsheet(this Worksheet xlWs) {
            string key = RvxPlus1Configsheet.Key;

            RvxPlus1Configsheet info = null;

            if (xlWs != null && xlWs.Name.StartsWith(key))
                info = new RvxPlus1Configsheet(xlWs);

            return info;
        }

        public static bool IsWorkbookPrevsCenarios(this Workbook xlWb) {
            if (xlWb.GetWorksheet("Entrada") != null &&
                xlWb.GetWorksheet("Saida") != null &&
                xlWb.GetWorksheet("Prevs") != null
                ) {
                return true;
            } else
                return false;
        }

        public static Compass.ExcelTools.Templates.WorkbookPrevsCenarios ToWorkbookPrevsCenarios(this Workbook xlWb) {
            if (xlWb.GetWorksheet("Entrada") != null &&
                xlWb.GetWorksheet("Saida") != null &&
                xlWb.GetWorksheet("Prevs") != null
                ) {
                return new Templates.WorkbookPrevsCenarios(xlWb);
            } else
                return null;
        }


    }

    public static class OLEExtensions {
        public static OLEContent GetContent(this OLEObject ole) {
            ole.Copy();
            System.Windows.Forms.IDataObject data = System.Windows.Forms.Clipboard.GetDataObject();
            object obj = data.GetData("Native");
            System.Windows.Forms.Clipboard.SetDataObject("");
            var ms = obj as MemoryStream;
            if (ms != null)
                return ms.GetOLEContent();
            return null;

        }
    }

    public static class OLEStreamExtensions {
        public static int ReadHeader(this MemoryStream ms) {
            var header = new byte[2];
            int read = ms.Read(header, 0, header.Length);
            if (read != header.Length)
                throw new FormatException("End of stream while reading header");
            if (header[0] != 2 || header[1] != 0)
                throw new FormatException("Bad header");
            return read;
        }
        public static string ReadString(this MemoryStream ms) {
            var sb = new StringBuilder();
            while (true) {
                int b = ms.ReadByte();
                if (b == -1)
                    throw new FormatException("End of stream while reading string");
                if (b == 0)
                    return sb.ToString();
                sb.Append((char)b);
            }
        }
        public static int ReadInt(this MemoryStream ms) {
            var bytes = new byte[4];
            int read = ms.Read(bytes, 0, bytes.Length);
            if (read != bytes.Length)
                throw new FormatException("End of stream while reading int");
            return BitConverter.ToInt32(bytes, 0);
        }
        public static byte[] ReadBytes(this MemoryStream ms, int count) {
            var bytes = new byte[count];
            int read = ms.Read(bytes, 0, count);
            if (read != count)
                throw new FormatException("End of stream while reading bytes");
            return bytes;
        }
        public static OLEContent GetOLEContent(this MemoryStream ms) {
            ms.ReadHeader();
            string name = ms.ReadString();
            string path = ms.ReadString();
            int reserved = ms.ReadInt();
            if (reserved != 0x30000)
                throw new FormatException(string.Format("Unexpected reserved bytes : got {0} but expected {1}", reserved.ToString("x"), 0x30000.ToString("x")));
            int tempLength = ms.ReadInt();
            string tempPath = ms.ReadString();
            if (tempPath.Length + 1 != tempLength)
                throw new FormatException(string.Format("Mismatch between temp length {0} and temp full path length {1}", tempLength, tempPath.Length + 1));
            int contentLength = ms.ReadInt();
            byte[] content = ms.ReadBytes(contentLength);
            int delta = sizeof(int) * 3 + (name.Length + path.Length + tempPath.Length) * 2;
            if (ms.Length != ms.Position + delta)
                throw new FormatException("Unexpected end of file");
            return new OLEContent(name, content);
        }
    }

    public class OLEContent {
        #region Fields
        private readonly string name;
        private readonly byte[] content;
        #endregion
        public OLEContent(string name, byte[] content) {
            this.name = name;
            this.content = content;
        }
        public string Name { get { return name; } }
        public byte[] Content { get { return content; } }
    }
}

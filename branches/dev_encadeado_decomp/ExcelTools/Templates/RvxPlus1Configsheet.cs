using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Compass.ExcelTools.Templates {
    public class RvxPlus1Configsheet {

        Worksheet ws;

        public static string Key = "Rvx+1";

        public string BaseFolder { get { return ws.Cells[1, 2].Value ?? ""; } set { ws.Cells[1, 2].Value = value; } }
        public string CaseFolder { get { return ws.Cells[2, 2].Value ?? ""; } set { ws.Cells[2, 2].Value = value; } }

        public RvxPlus1Configsheet(Worksheet xlWs) {

            if (xlWs == null) {
                throw new ArgumentNullException("xlWs");
            }

            this.ws = xlWs;            
        }

        internal void Initialize() {
            //doc type
            ws.Cells[1, 1].Value = "Base: ";

            //original file
            ws.Cells[2, 1].Value = "Destino: ";
        }
    }
}

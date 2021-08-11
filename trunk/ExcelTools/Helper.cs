using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.ExcelTools {
    public static class Helper {
        public static Microsoft.Office.Interop.Excel.Application StartExcel() {
            Microsoft.Office.Interop.Excel.Application instance = null;
            try {
                instance = (Microsoft.Office.Interop.Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            } catch (System.Runtime.InteropServices.COMException) {
                instance = new Microsoft.Office.Interop.Excel.Application();
            }
            instance.Visible = true;
           // foreach (Microsoft.Office.Core.COMAddIn CurrAddin in instance.COMAddIns)
            //    if (CurrAddin.Description == "DecompTools ExcelAddin") {
           //         CurrAddin.Connect = false;
            //        CurrAddin.Connect = true;
           //     }

            return instance;
        }

        public static void Release(object o) {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
        }
    }
}

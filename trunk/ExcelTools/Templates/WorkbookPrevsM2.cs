using Compass.CommomLibrary.Decomp;
using Compass.CommomLibrary.Prevs;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compass.ExcelTools;


namespace Compass.ExcelTools.Templates
{
    public class WorkbookPrevsM2 : BaseWorkbook
    {

        public static bool TryCreate(Workbook xlWb, out WorkbookPrevsM2 w)
        {

            var ok = false;

            var names = new List<string>();
            foreach (Name name in xlWb.Names)
            {
                names.Add(name.Name);
            }

            ok =
                names.Contains("_metas")
                ;

            if (ok)
            {
                w = new WorkbookPrevsM2(xlWb);
            }
            else
                w = null;

            return ok;
        }

        public object[,] Entrada { get { return this.Names["_entrada"].Value; } set { this.Names["_entrada"].Value = value; } }
        public object[,] Cenarios { get { return this.Names["_cenarios"].Value; } set { this.Names["_cenarios"].Value = value; } }
        public object[,] Metas { get { return this.Names["_metas"].Value; } set { this.Names["_metas"].Value = value; } }
        public string PlanBase { get { return this.Names["_planBase"].Value; } set { this.Names["_planBase"].Value = value; } }
        public string EstudoPath { get { return this.Names["_estudo"].Value; } set { this.Names["_estudo"].Value = value; } }

        public int ROW { get { return this.Names["_metas"].Row; } }
        public int COL { get { return this.Names["_metas"].Column; } }
        public Worksheet worksheet { get { return this.Names["_entrada"].Worksheet; } } 

        public WorkbookPrevsM2(Workbook xlWb)
           : base(xlWb)
        {

        }
    }
}

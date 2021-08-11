using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.ExcelTools.Templates
{
    public abstract class BaseWorkbook
    {

        public static List<string> GetNamesFrom(Workbook wb)
        {
            var names = new List<string>();
            foreach (Name name in wb.Names)
            {
                names.Add(name.Name);
            }
            return names;
        }


        public BaseWorkbook(Workbook wb)
        {
            Wb = wb;
        }

        public string Path { get { return wb.Path; } }

        private Workbook wb = null;

        protected Workbook Wb
        {
            get { return wb; }
            set
            {
                wb = value;

                Names = new Dictionary<string, Range>();

                foreach (Name name in wb.Names)
                {
                    if (name.Visible) Names.Add(name.Name, name.RefersToRange);
                }
            }
        }

        public Dictionary<string, Range> Names
        {
            get;
            private set;
        }


    }

    public abstract class BaseWorksheet
    {



        protected Dictionary<string, Range> Names
        {
            get;
            private set;
        }

        private Worksheet ws = null;
        protected Worksheet Ws
        {
            get { return ws; }
            set
            {
                ws = value;

                Names = new Dictionary<string, Range>();

                foreach (Name name in ws.Names)
                {
                    if (name.Visible) Names.Add(name.Name.Replace(ws.Name, "").Replace("\'", "").Replace("!", ""), name.RefersToRange);
                }
            }
        }


        public BaseWorksheet(Worksheet ws)
        {
            Ws = ws;
        }

        protected string SheetName { get { return ws.Name; } }
    }
}

using Compass.CommomLibrary;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.ExcelTools.Templates {
    public class WorkbookAcomph : IQueryable<Acomph> {

        Workbook wbAcomph;

        public DateTime dt_acomph {
            get;
            set;
        }

        public List<Acomph> Dados { get; private set; }

        public WorkbookAcomph(Workbook wbAcomph) {

            this.wbAcomph = wbAcomph;

            var pat = @"ACOMPH[-_\s](?'dia'\d{2})[-_\s](?'mes'\d{2})[-_\s](?'ano'\d{2,4})";
            var m = System.Text.RegularExpressions.Regex.Match(wbAcomph.Name, pat, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (m.Success) dt_acomph = new DateTime((m.Groups["ano"].Value.Length == 4 ? 0 : 2000) + int.Parse(m.Groups["ano"].Value), int.Parse(m.Groups["mes"].Value), int.Parse(m.Groups["dia"].Value));
            else throw new Exception("Falhou em abrir acomph. Nome fora do padrão.");
            LeDados();

        }

        private void LeDados() {


            Dados = new List<Acomph>();
            //Blocos que serão comparados.    

            try {


                foreach (Worksheet sheet in wbAcomph.Worksheets) {

                    object[,] valMatrix = sheet.Range["A1", "GA35"].Value;

                    int idxP = 9;

                    while (valMatrix[1, idxP] is double || valMatrix[1, idxP] is int) {


                        int p = Convert.ToInt32(valMatrix[1, idxP]);

                        for (int idt = 0; idt < 30; idt++) {

                            var dt = this.dt_acomph.AddDays(-30 + idt);

                            Dados.Add(
                                new Acomph() { dt = (DateTime)valMatrix[6 + idt, 1], posto = p, qInc = (double)valMatrix[6 + idt, idxP - 1], qNat = (double)valMatrix[6 + idt, idxP] }
                            );

                        }
                        idxP += 8;
                    }
                }

            } finally {
            }
        }

        public IEnumerator<Acomph> GetEnumerator() {
            return Dados.AsQueryable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return Dados.AsQueryable().GetEnumerator();
        }

        public Type ElementType {
            get { return Dados.AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression {
            get { return Dados.AsQueryable().Expression; }
        }

        public IQueryProvider Provider {
            get { return Dados.AsQueryable().Provider; }
        }
    }
   
}

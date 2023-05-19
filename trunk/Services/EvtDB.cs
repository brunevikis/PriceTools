using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Compass.Services.DB;
using Compass.ExcelTools;
using System.Windows.Forms;

namespace Compass.Services
{
    public class EvtDB
    {

        public void ConsultaEVT(Workbook workb, DateTime ini, DateTime fim, string banco = "local")
        {
            Workbook wb = null;
            try
            {
                wb = workb;

                Tuple<DateTime, DateTime> datas = new Tuple<DateTime, DateTime>(ini, fim);


                var result = select_Banco("IPDO", datas);
                var xlsM = wb.GetOrCreateWorksheet("ENERGIA-VERT-TURB");
                xlsM.UsedRange.Clear();
                xlsM.Activate();



                xlsM.Range[xlsM.Cells[1, 1], xlsM.Cells[1, 4]].Value2 = new dynamic[,] {
                    {"DATA", "SUBSISTEMA", "NUMERO", "EVT(MWmed)"}
                };

                var l = 2;
                foreach (var res in result.Select(u => new dynamic[,]{
                    {u[0], u[1], u[2], u[3]}
                }
                    ))
                {
                    xlsM.Range[xlsM.Cells[l, 1], xlsM.Cells[l++, res.Length]].Value = res;

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CarregaEVT(Workbook workb, string banco = "local")
        {
            Workbook wb = null;
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            //Convert.ToDateTime(coms[1], Culture.DateTimeFormat)
            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //path = @"D:\Compass\Acomph\ACOMPH_31.03.2020.xls";
            try
            {

                // Object[,] data_dados = null;
                // excel.DisplayAlerts = false;
                //excel.Visible = false;
                // excel.ScreenUpdating = true;
                //Workbook workbook = excel.Workbooks.Open(path);

                wb = workb;

                Sheets sheets = wb.Worksheets;

                var N_Sheets = sheets.Count;

                var Dados = new List<(string tabela, string[] campos, object[,] valores)>();
                //var Postos = new List<(int Posto, object data)>();
                for (int i = 1; i <= N_Sheets; i++)
                {
                    Worksheet worksheet = (Worksheet)sheets.get_Item(i);
                    string sheetName = worksheet.Name;//Get the name of worksheet.
                    int col = 4;
                    int l = 1;
                    int dtLin = 3;
                    int dtCol = 4;
                    bool comeco = false;

                    //for (var r = row; !string.IsNullOrWhiteSpace(ws.Cells[r, col].Text); r++)

                    string lin = wb.Worksheets[sheetName].Cells[l, 1].Value;
                    do
                    {
                        if (lin != null && lin.ToLower().Contains("total geral"))
                        {
                            comeco = true;
                        }
                        else
                        {
                            l++;
                            lin = wb.Worksheets[sheetName].Cells[l, 1].Value;
                        }
                    } while (comeco != true && l <= 100);

                    if (l > 100)
                    {

                        return;
                    }
                    for (var c = dtCol; !string.IsNullOrWhiteSpace(worksheet.Cells[dtLin + 1, c].Text); c++)
                    {
                        int colSub = c;
                        string subistema;
                        if (!string.IsNullOrWhiteSpace(worksheet.Cells[dtLin, colSub].Text))
                        {
                            //DateTime data = Convert.ToDateTime(wb.Worksheets[sheetName].Cells[dtLin, colSub].Text);
                            DateTime data = Convert.ToDateTime(wb.Worksheets[sheetName].Cells[dtLin, colSub].Text, Culture.DateTimeFormat);
                            if (data != null)
                            {
                                for (int e = 0; e < 4; e++)
                                {
                                    if (e == 0 || string.IsNullOrWhiteSpace(worksheet.Cells[dtLin, colSub + e].Text))
                                    {
                                        string sub = wb.Worksheets[sheetName].Cells[dtLin + 1, colSub + e].Value;
                                        int numsub = sub.ToLower().Contains("sudeste") ? 1 : sub.ToLower().Contains("sul") ? 2 : sub.ToLower().Contains("nordeste") ? 3 : 4;
                                        switch (numsub)
                                        {
                                            case 1:
                                                subistema = "Sudeste";
                                                break;
                                            case 2:
                                                subistema = "Sul";
                                                break;
                                            case 3:
                                                subistema = "Nordeste";
                                                break;
                                            case 4:
                                                subistema = "Norte";
                                                break;

                                            default:
                                                subistema = "";
                                                break;
                                        }
                                        var evt = wb.Worksheets[sheetName].Cells[l, colSub + e].Value;

                                        string[] campos = { "[Data]", "[Subsistema]", "[NumSubsistema]", "[EVT]" };
                                        object[,] valores = new object[1, 4]    {
                                                        {
                                                            data,
                                                            subistema,
                                                            numsub,
                                                            evt,
                                                        }
                                                    };
                                        string tabela = "[dbo].[EVT]";
                                        //Postos.Add((Convert.ToInt32(posto), data));
                                        // objSQL.Execute("DELETE FROM [IPDO].[dbo].[ACOMPH] WHERE Posto = '" + posto + "' and Data ='" + Convert.ToDateTime(data).ToString("yyyy-MM-dd HH:mm:ss") + "'");

                                        Dados.Add((tabela, campos, valores));
                                        //    objSQL.Insert(tabela, campos, valores);
                                    }

                                }
                            }
                        }

                    }

                }


                //wb.Close();
                //workbook.Close();
                // excel.Quit();

                inserir_Banco("local", Dados);

                //inserir_Banco("azure", Dados, Postos, data_dados);
                System.Windows.Forms.MessageBox.Show("Processo concuído com sucesso!!!");


            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                //wb.Close();
                //excel.Quit();
            }


        }
        public List<List<object>> select_Banco(string banco, Tuple<DateTime, DateTime> datas)
        {
            string query_Delete = "";
            string query_Insert = "";
            IDB objSQL = new SQLServerDBCompass(banco);

            var listaRes = objSQL.GetList("SELECT [Data],[Subsistema],[NumSubsistema],[EVT] FROM [IPDO].[dbo].[EVT] WHERE Data >= '" + datas.Item1.ToString("yyyy-MM-dd") + "' AND Data <= '" + datas.Item2.ToString("yyyy-MM-dd") + "'");

            return listaRes;
        }

        public void inserir_Banco(string banco, List<(string tabela, string[] campos, object[,] valores)> Dados)
        {
            string query_Delete = "";
            string query_Insert = "";
            IDB objSQL = new SQLServerDBCompass(banco);

            foreach (var data in Dados)
            {
                objSQL.Execute("DELETE FROM [IPDO].[dbo].[EVT] WHERE Data ='" + Convert.ToDateTime(data.valores[0, 0]).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }


            // foreach (var p in Postos)
            //{
            //   query_Delete = query_Delete + "DELETE FROM [IPDO].[dbo].[ACOMPH] WHERE Posto = '" + p.Posto + "' and Data ='" + Convert.ToDateTime(p.data).ToString("yyyy-MM-dd HH:mm:ss") + "';";

            //}
            //objSQL.Execute("DELETE FROM [IPDO].[dbo].[ACOMPH] WHERE Posto = '" + p.Posto + "' and Data ='" + Convert.ToDateTime(p.data).ToString("yyyy-MM-dd HH:mm:ss") + "'");


            int i = 0;

            foreach (var Info in Dados)
            {
                if (i <= 300)
                {
                    query_Insert = query_Insert + "INSERT INTO [IPDO].[dbo].[EVT] ( [Data],[Subsistema],[NumSubsistema],[EVT] ) Values ('" + Convert.ToDateTime(Info.valores[0, 0]).ToString("yyyy-MM-dd HH:mm:ss") + "', '" + Info.valores[0, 1] + "', '" + Convert.ToInt32(Info.valores[0, 2]).ToString().Replace(',', '.') + "', '" + Convert.ToInt32(Info.valores[0, 3]).ToString().Replace(',', '.') + "');";
                    i++;
                }
                else
                {
                    query_Insert = query_Insert + "INSERT INTO [IPDO].[dbo].[EVT] ( [Data],[Subsistema],[NumSubsistema],[EVT] ) Values ('" + Convert.ToDateTime(Info.valores[0, 0]).ToString("yyyy-MM-dd HH:mm:ss") + "', '" + Info.valores[0, 1] + "', '" + Convert.ToInt32(Info.valores[0, 2]).ToString().Replace(',', '.') + "', '" + Convert.ToInt32(Info.valores[0, 3]).ToString().Replace(',', '.') + "');";
                    objSQL.Execute(query_Insert);
                    i = 0;
                    query_Insert = "";
                }
                //objSQL.Insert(Info.tabela, Info.campos, Info.valores);

            }
            objSQL.Execute(query_Insert);


        }
    }
}

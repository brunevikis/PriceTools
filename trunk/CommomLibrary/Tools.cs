using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Compass.CommomLibrary
{
    public static class Tools
    {
        public static string GetMonthName(int month)
        {

            switch (month)
            {
                case 1: return "Janeiro";
                case 2: return "Fevereiro";
                case 3: return "Março";
                case 4: return "Abril";
                case 5: return "Maio";
                case 6: return "Junho";
                case 7: return "Julho";
                case 8: return "Agosto";
                case 9: return "Setembro";
                case 10: return "Outubro";
                case 11: return "Novembro";
                case 12: return "Dezembro";

                default:
                    return null;
            }
        }
        public static void SendMail(string body, string emails = "bruno.araujo@enercore.com.br", string subject = "Execução automática")
        {

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

            msg.IsBodyHtml = true;

            msg.BodyEncoding = System.Text.Encoding.UTF8;

            msg.Body = body;

            msg.Subject = subject;

            msg.Sender = msg.From = new System.Net.Mail.MailAddress("cpas.robot@gmail.com");

            msg.ReplyToList.Add(new System.Net.Mail.MailAddress("bruno.araujo@enercore.com.br"));

            // var emails = "douglas.canducci@cpas.com.br;pedro.modesto@cpas.com.br;diana.lima@cpas.com.br;natalia.biondo@cpas.com.br;bruno.araujo@cpas.com.br;alex.marques@cpas.com.br";

            foreach (var to in emails.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                msg.To.Add(to);
            }


            System.Net.Mail.SmtpClient cli = new System.Net.Mail.SmtpClient();

            cli.Host = "smtp.gmail.com";
            cli.Port = 587;
            //cli.Credentials = new System.Net.NetworkCredential("cpas.robot@gmail.com", "cp@s9876");
            cli.Credentials = new System.Net.NetworkCredential("cpas.robot@gmail.com", "ujkuiwpbeqerumvs");

            cli.EnableSsl = true;

            cli.Send(msg);
        }

        public static float GetHidrogram(DateTime data)
        {
            List<float> hidrogramaA = new List<float> { 1100, 1600, 2500, 4000, 1800, 1200, 1000, 900, 750, 700, 800, 900 };
            List<float> hidrogramaB = new List<float> { 1100, 1600, 4000, 8000, 4000, 2000, 1200, 900, 750, 700, 800, 900 };

            if ((data.Year % 2) == 0)
            {
                return hidrogramaA[data.Month - 1];
            }
            else
            {
                return hidrogramaB[data.Month - 1];
            }
        }

        public static string GetMonthNumAbrev(int month)
        {

            switch (month)
            {
                case 1: return "01_jan";
                case 2: return "02_fev";
                case 3: return "03_mar";
                case 4: return "04_abr";
                case 5: return "05_mai";
                case 6: return "06_jun";
                case 7: return "07_jul";
                case 8: return "08_ago";
                case 9: return "09_set";
                case 10: return "10_out";
                case 11: return "11_nov";
                case 12: return "12_dez";

                default:
                    return null;
            }
        }

        public static void moveDirectory(string fuente, string destino)
        {
            if (!System.IO.Directory.Exists(destino))
            {
                System.IO.Directory.CreateDirectory(destino);
            }
            String[] files = System.IO.Directory.GetFiles(fuente);
            String[] directories = System.IO.Directory.GetDirectories(fuente);
            foreach (string s in files)
            {
                var dest = System.IO.Path.Combine(destino, System.IO.Path.GetFileName(s));
                if (System.IO.File.Exists(dest)) System.IO.File.Delete(dest);
                System.IO.File.Move(s, dest);
            }
            foreach (string d in directories)
            {
                var d1 = System.IO.Path.Combine(fuente, System.IO.Path.GetFileName(d));
                moveDirectory(d1, System.IO.Path.Combine(destino, System.IO.Path.GetFileName(d)));
                System.IO.Directory.Delete(d1, true);
            }

        }

        public static DateTime[] inicioVR = new DateTime[] {
new DateTime(2017,10,15),
new DateTime(2018,11,4),
new DateTime(2019,10,20),
new DateTime(2020,10,18),
new DateTime(2021,10,17),
new DateTime(2022,10,16),
new DateTime(2023,10,15),
new DateTime(2024,10,20),
new DateTime(2025,10,19),
new DateTime(2026,10,18),
new DateTime(2027,10,17),
new DateTime(2028,10,15),
new DateTime(2029,10,21),
new DateTime(2030,10,20),
new DateTime(2031,10,19),
new DateTime(2032,10,17),

            };

        public static DateTime[] fimVR = new DateTime[] {
new DateTime(2018,02,17),
new DateTime(2019,02,16),
new DateTime(2020,02,15),
new DateTime(2021,02,20),
new DateTime(2022,02,19),
new DateTime(2023,02,18),
new DateTime(2024,02,17),
new DateTime(2025,02,15),
new DateTime(2026,02,14),
new DateTime(2027,02,20),
new DateTime(2028,02,19),
new DateTime(2029,02,17),
new DateTime(2030,02,16),
new DateTime(2031,02,15),
new DateTime(2032,02,14),
            };

        public static DateTime[] feriados = new DateTime[] {
                new DateTime(2017,01,01),
new DateTime(2017,02,28),
new DateTime(2017,04,14),
new DateTime(2017,04,16),
new DateTime(2017,04,21),
new DateTime(2017,05,01),
new DateTime(2017,06,15),
new DateTime(2017,09,07),
new DateTime(2017,10,12),
new DateTime(2017,11,02),
new DateTime(2017,11,15),
new DateTime(2017,12,25),
new DateTime(2018,01,01),
new DateTime(2018,02,13),
new DateTime(2018,03,30),
new DateTime(2018,04,01),
new DateTime(2018,04,21),
new DateTime(2018,05,01),
new DateTime(2018,05,31),
new DateTime(2018,09,07),
new DateTime(2018,10,12),
new DateTime(2018,11,02),
new DateTime(2018,11,15),
new DateTime(2018,12,25),
new DateTime(2019,01,01),
new DateTime(2019,03,05),
new DateTime(2019,04,19),
new DateTime(2019,05,01),
new DateTime(2019,06,20),
new DateTime(2019,09,07),
new DateTime(2019,10,12),
new DateTime(2019,11,02),
new DateTime(2019,11,15),
new DateTime(2019,12,25),
new DateTime(2020,01,01),
new DateTime(2020,02,25),
new DateTime(2020,04,10),
new DateTime(2020,04,12),
new DateTime(2020,04,21),
new DateTime(2020,05,01),
new DateTime(2020,06,11),
new DateTime(2020,09,07),
new DateTime(2020,10,12),
new DateTime(2020,11,02),
new DateTime(2020,11,15),
new DateTime(2020,12,25),
new DateTime(2021,01,01),
new DateTime(2021,02,16),
new DateTime(2021,04,02),
new DateTime(2021,04,04),
new DateTime(2021,04,21),
new DateTime(2021,05,01),
new DateTime(2021,06,03),
new DateTime(2021,09,07),
new DateTime(2021,10,12),
new DateTime(2021,11,02),
new DateTime(2021,11,15),
new DateTime(2021,12,25),
new DateTime(2022,01,01),
new DateTime(2022,03,01),
new DateTime(2022,04,15),
new DateTime(2022,04,17),
new DateTime(2022,04,21),
new DateTime(2022,05,01),
new DateTime(2022,06,16),
new DateTime(2022,09,07),
new DateTime(2022,10,12),
new DateTime(2022,11,02),
new DateTime(2022,11,15),
new DateTime(2022,12,25),
new DateTime(2023,01,01),
new DateTime(2023,02,21),
new DateTime(2023,04,07),
new DateTime(2023,04,09),
new DateTime(2023,04,21),
new DateTime(2023,05,01),
new DateTime(2023,06,08),
new DateTime(2023,09,07),
new DateTime(2023,10,12),
new DateTime(2023,11,02),
new DateTime(2023,11,15),
new DateTime(2023,12,25),
new DateTime(2024,01,01),
new DateTime(2024,02,13),
new DateTime(2024,03,29),
new DateTime(2024,03,31),
new DateTime(2024,04,21),
new DateTime(2024,05,01),
new DateTime(2024,05,30),
new DateTime(2024,09,07),
new DateTime(2024,10,12),
new DateTime(2024,11,02),
new DateTime(2024,11,15),
new DateTime(2024,11,20),
new DateTime(2024,12,25),
new DateTime(2025,01,01),
new DateTime(2025,03,04),
new DateTime(2025,04,18),
new DateTime(2025,04,20),
new DateTime(2025,04,21),
new DateTime(2025,05,01),
new DateTime(2025,06,19),
new DateTime(2025,09,07),
new DateTime(2025,10,12),
new DateTime(2025,11,02),
new DateTime(2025,11,15),
new DateTime(2025,11,20),
new DateTime(2025,12,25),
new DateTime(2026,01,01),
new DateTime(2026,02,17),
new DateTime(2026,04,03),
new DateTime(2026,04,05),
new DateTime(2026,04,21),
new DateTime(2026,05,01),
new DateTime(2026,06,04),
new DateTime(2026,09,07),
new DateTime(2026,10,12),
new DateTime(2026,11,02),
new DateTime(2026,11,15),
new DateTime(2026,11,20),
new DateTime(2026,12,25),
new DateTime(2027,01,01),
new DateTime(2027,02,09),
new DateTime(2027,03,26),
new DateTime(2027,03,28),
new DateTime(2027,04,21),
new DateTime(2027,05,01),
new DateTime(2027,05,27),
new DateTime(2027,09,07),
new DateTime(2027,10,12),
new DateTime(2027,11,02),
new DateTime(2027,11,15),
new DateTime(2027,11,20),
new DateTime(2027,12,25),
new DateTime(2028,01,01),
new DateTime(2028,02,29),
new DateTime(2028,04,14),
new DateTime(2028,04,16),
new DateTime(2028,04,21),
new DateTime(2028,05,01),
new DateTime(2028,06,15),
new DateTime(2028,09,07),
new DateTime(2028,10,12),
new DateTime(2028,11,02),
new DateTime(2028,11,15),
new DateTime(2028,11,20),
new DateTime(2028,12,25),
new DateTime(2029,01,01),
new DateTime(2029,02,13),
new DateTime(2029,03,30),
new DateTime(2029,04,01),
new DateTime(2029,04,21),
new DateTime(2029,05,01),
new DateTime(2029,05,31),
new DateTime(2029,09,07),
new DateTime(2029,10,12),
new DateTime(2029,11,02),
new DateTime(2029,11,15),
new DateTime(2029,11,20),
new DateTime(2029,12,25),
new DateTime(2030,01,01),
new DateTime(2030,03,05),
new DateTime(2030,04,19),
new DateTime(2030,05,01),
new DateTime(2030,06,20),
new DateTime(2030,09,07),
new DateTime(2030,10,12),
new DateTime(2030,11,02),
new DateTime(2030,11,15),
new DateTime(2030,11,20),
new DateTime(2030,12,25),
new DateTime(2031,01,01),
new DateTime(2031,02,25),
new DateTime(2031,04,11),
new DateTime(2031,04,13),
new DateTime(2031,04,21),
new DateTime(2031,05,01),
new DateTime(2031,06,12),
new DateTime(2031,09,07),
new DateTime(2031,10,12),
new DateTime(2031,11,02),
new DateTime(2031,11,15),
new DateTime(2031,11,20),
new DateTime(2031,12,25),
new DateTime(2032,01,01),
new DateTime(2032,02,10),
new DateTime(2032,03,26),
new DateTime(2032,03,28),
new DateTime(2032,04,21),
new DateTime(2032,05,01),
new DateTime(2032,05,27),
new DateTime(2032,09,07),
new DateTime(2032,10,12),
new DateTime(2032,11,02),
new DateTime(2032,11,15),
new DateTime(2032,11,20),
new DateTime(2032,12,25),
new DateTime(2033,01,01),
new DateTime(2033,03,01),
new DateTime(2033,04,15),
new DateTime(2033,04,17),
new DateTime(2033,04,21),
new DateTime(2033,05,01),
new DateTime(2033,06,16),
new DateTime(2033,09,07),
new DateTime(2033,10,12),
new DateTime(2033,11,02),
new DateTime(2033,11,15),
new DateTime(2033,11,20),
new DateTime(2033,12,25),

            };

        public static Tuple<int, int, int> GetHorasPatamares(DateTime ini, DateTime fim, bool patamares2019, bool patamares2023 = false, bool patamares2024 = false, bool patamares2025 = false)
        {
            Tuple<int, int, int>[,] horasPatamares;

            // var horasPatamares = new Tuple<int, int, int>[] {
            //                new Tuple<int,int,int>(3,14,7),
            //                new Tuple<int,int,int>(0,5,19),
            if (patamares2019)
                horasPatamares = new Tuple<int, int, int>[,] {
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(10,06,08), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(12,05,07), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(12,05,07), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(12,05,07), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(12,05,07), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(10,06,08), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(10,06,08), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) }
            };
            else
                horasPatamares = new Tuple<int, int, int>[,] {
                           { new Tuple<int,int,int>(3,14,7), new Tuple<int,int,int>(0,5,19) }
                };

            //{ new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,03,21) }, patamares2021
            //{ new Tuple<int, int, int>(08, 08, 08), new Tuple<int, int, int>(00, 03, 21) },
            //    { new Tuple<int, int, int>(08, 08, 08), new Tuple<int, int, int>(00, 03, 21) },
            //    { new Tuple<int, int, int>(10, 06, 08), new Tuple<int, int, int>(00, 04, 20) },
            //    { new Tuple<int, int, int>(12, 05, 07), new Tuple<int, int, int>(00, 04, 20) },
            //    { new Tuple<int, int, int>(12, 05, 07), new Tuple<int, int, int>(00, 04, 20) },
            //    { new Tuple<int, int, int>(12, 05, 07), new Tuple<int, int, int>(00, 04, 20) },
            //    { new Tuple<int, int, int>(12, 05, 07), new Tuple<int, int, int>(00, 04, 20) },
            //    { new Tuple<int, int, int>(10, 06, 08), new Tuple<int, int, int>(00, 04, 20) },
            //    { new Tuple<int, int, int>(10, 06, 08), new Tuple<int, int, int>(00, 04, 20) },
            //    { new Tuple<int, int, int>(08, 08, 08), new Tuple<int, int, int>(00, 03, 21) },
            //    { new Tuple<int, int, int>(08, 08, 08), new Tuple<int, int, int>(00, 03, 21) }
            if (patamares2023)
            {
                horasPatamares = new Tuple<int, int, int>[,] {
                { new Tuple<int,int,int>(09,07,08), new Tuple<int,int,int>(00,05,19) },//
                { new Tuple<int,int,int>(09,07,08), new Tuple<int,int,int>(00,05,19) },//
                { new Tuple<int,int,int>(09,07,08), new Tuple<int,int,int>(00,05,19) },//
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },//
                { new Tuple<int,int,int>(07,09,08), new Tuple<int,int,int>(00,04,20) },//
                { new Tuple<int,int,int>(07,09,08), new Tuple<int,int,int>(00,04,20) },//
                { new Tuple<int,int,int>(07,09,08), new Tuple<int,int,int>(00,04,20) },//
                { new Tuple<int,int,int>(07,09,08), new Tuple<int,int,int>(00,04,20) },//
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },//
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },//
                { new Tuple<int,int,int>(09,07,08), new Tuple<int,int,int>(00,05,19) },//
                { new Tuple<int,int,int>(09,07,08), new Tuple<int,int,int>(00,05,19) }//
            };
            }

            if (patamares2024)
            {
                horasPatamares = new Tuple<int, int, int>[,] {
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,05,19) },//jan
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,05,19) },//fev
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,05,19) },//mar
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },//abr
                { new Tuple<int,int,int>(06,10,08), new Tuple<int,int,int>(00,04,20) },//mai
                { new Tuple<int,int,int>(06,10,08), new Tuple<int,int,int>(00,04,20) },//jun
                { new Tuple<int,int,int>(06,10,08), new Tuple<int,int,int>(00,04,20) },//jul
                { new Tuple<int,int,int>(06,10,08), new Tuple<int,int,int>(00,04,20) },//ago
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },//set
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,04,20) },//out
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,05,19) },//nov
                { new Tuple<int,int,int>(08,08,08), new Tuple<int,int,int>(00,05,19) }// dez
            };
            }

            if (patamares2025)
            {
                horasPatamares = new Tuple<int, int, int>[,] {
                { new Tuple<int,int,int>(08,09,07), new Tuple<int,int,int>(00,07,17) },//jan
                { new Tuple<int,int,int>(08,09,07), new Tuple<int,int,int>(00,07,17) },////fev
                { new Tuple<int,int,int>(08,09,07), new Tuple<int,int,int>(00,07,17) },////mar
                { new Tuple<int,int,int>(07,10,07), new Tuple<int,int,int>(00,07,17) },////abr
                { new Tuple<int,int,int>(05,11,08), new Tuple<int,int,int>(00,05,19) },////mai
                { new Tuple<int,int,int>(05,11,08), new Tuple<int,int,int>(00,05,19) },////jun
                { new Tuple<int,int,int>(05,11,08), new Tuple<int,int,int>(00,05,19) },////jul
                { new Tuple<int,int,int>(05,11,08), new Tuple<int,int,int>(00,05,19) },////ago
                { new Tuple<int,int,int>(07,10,07), new Tuple<int,int,int>(00,07,17) },////set
                { new Tuple<int,int,int>(07,10,07), new Tuple<int,int,int>(00,07,17) },////out
                { new Tuple<int,int,int>(08,09,07), new Tuple<int,int,int>(00,07,17) },////nov
                { new Tuple<int,int,int>(08,09,07), new Tuple<int,int,int>(00,07,17) }/////dez
            };
            }



            var p1 = 0;
            var p2 = 0;
            var p3 = 0;

            for (DateTime dt = ini; dt <= fim; dt = dt.AddDays(1))
            {
                Tuple<int, int, int> pat;


                if (patamares2019)
                    pat = (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday || feriados.Contains(dt)) ?
                       horasPatamares[dt.Month - 1, 1] :
                       horasPatamares[dt.Month - 1, 0];

                else
                    pat = (dt.DayOfWeek == DayOfWeek.Sunday || feriados.Contains(dt)) ?
                           horasPatamares[0, 1] :
                           horasPatamares[0, 0];


                p1 += pat.Item1;
                p2 += pat.Item2;
                p3 += pat.Item3;

                //if (inicioVR.Contains(dt))
                //{
                //    p3--;
                //}
                //else if (fimVR.Contains(dt))
                //{
                //    if (patamares2019) p3++;
                //    else p2++;

                //}

            }

            return new Tuple<int, int, int>(p1, p2, p3);
        }

        public static Tuple<DateTime, DateTime, double, DateTime, DateTime> GetRangeDiasHorasRHE(DateTime dataBase, DateTime fimrev, Compass.CommomLibrary.EntdadosDat.RheLine rhe, DateTime dataEstudo)
        {
            DateTime dataInicial = new DateTime(dataBase.Year, dataBase.Month, rhe.DiaInic.Trim() == "I" ? dataBase.Day : Convert.ToInt32(rhe.DiaInic));
            int meiaIni = rhe.MeiaHoraInic ?? 0;
            dataInicial = dataInicial.AddHours(rhe.HoraInic ?? 0).AddMinutes(meiaIni == 0 ? 0 : 30);

            DateTime dataFinal = new DateTime(dataBase.Year, dataBase.Month, rhe.DiaFinal.Trim() == "F" ? fimrev.AddDays(1).Day : Convert.ToInt32(rhe.DiaFinal));
            int meiaFinal = rhe.MeiaHoraFinal ?? 0;
            dataFinal = dataFinal.AddHours(rhe.HoraFinal ?? 0).AddMinutes(meiaFinal == 0 ? 0 : 30);

            //ajustando viradas de meses 
            if (dataBase.Day < 10)
            {
                if (dataInicial.Day > 20)
                {
                    dataInicial = dataInicial.AddMonths(-1);
                }
            }
            if (dataBase.Day > 20 && dataInicial.Day < 10)
            {
                dataInicial = dataInicial.AddMonths(1);
            }
            if (dataBase.Day > dataFinal.Day)
            {
                dataFinal = dataFinal.AddMonths(1);
            }

            //fim ajuste de meses
            TimeSpan ts = dataFinal - dataInicial;
            var difHoras2 = ts.TotalHours;
            //return new Tuple<DateTime, DateTime,double>(dataInicial, dataFinal,difHoras2);


            TimeSpan tsinicial = dataInicial - dataBase;
            TimeSpan tsFinal = dataFinal - dataInicial;

            double horasInicio = tsinicial.TotalHours;
            double horasfim = tsFinal.TotalHours;

            DateTime novoInicio = dataEstudo.AddHours(horasInicio);
            //if ( novoInicio < dataEstudo)
            //{
            //    novoInicio = dataEstudo;
            //}

            DateTime novoFim = novoInicio.AddHours(horasfim);
            if (novoFim > fimrev.AddDays(1) || novoFim <= dataEstudo)
            {
                novoFim = fimrev.AddDays(1);
            }
            return new Tuple<DateTime, DateTime, double, DateTime, DateTime>(dataInicial, dataFinal, difHoras2, novoInicio, novoFim);
        }

        public static Tuple<DateTime, DateTime> GetRangeInicialFinal(DateTime dataBase, DateTime iniREVbase, string diaInicLine, string diaFimLine, DateTime dataEstudo, bool eRestricao = false)
        {
            DateTime dataInicial = new DateTime(dataBase.Year, dataBase.Month, diaInicLine.Trim() == "I" ? dataBase.Day : Convert.ToInt32(diaInicLine));

            DateTime dataFinal = new DateTime(dataBase.Year, dataBase.Month, diaFimLine.Trim() == "F" ? iniREVbase.AddDays(6).Day : Convert.ToInt32(diaFimLine));

            //ajustando viradas de meses 
            if (dataBase.Day < 10)
            {
                if (dataInicial.Day > 20)
                {
                    dataInicial = dataInicial.AddMonths(-1);
                }
            }
            if (dataBase.Day > 20 && dataInicial.Day < 10)
            {
                dataInicial = dataInicial.AddMonths(1);
            }
            if (dataBase.Day > dataFinal.Day)
            {
                dataFinal = dataFinal.AddMonths(1);
            }
            //fim ajuste de meses

            TimeSpan tsinicial = dataInicial - dataBase;
            TimeSpan tsFinal = dataFinal - dataInicial;

            double diasInicio = tsinicial.TotalDays;
            double diasfim = tsFinal.TotalDays;

            DateTime novoInicio = dataEstudo.AddDays(diasInicio);
            if (eRestricao && novoInicio < dataEstudo)
            {
                novoInicio = dataEstudo;
            }

            DateTime novoFim = novoInicio.AddDays(diasfim);

            return new Tuple<DateTime, DateTime>(novoInicio, novoFim);
        }

        public static List<Tuple<int, int>> GetIntervalosPatamares(DateTime data, bool pat2023 = false, bool pat2024 = false, bool pat2025 = false)
        {
            var feriados = Tools.feriados;
            Boolean ehFeriado = false;

            if (feriados.Any(x => x.Date == data.Date))
            {
                ehFeriado = true;
            }

            //listas com as horas do dia separadas em patamares
            List<Tuple<int, int>> NOVaMARutil = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 10),
                new Tuple<int, int>(11, 18),
                new Tuple<int, int>(19, 24)
            };

            List<Tuple<int, int>> NOVaMARutil2023 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 13),
                new Tuple<int, int>(14, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> NOVaMARutil2024 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 14),
                new Tuple<int, int>(15, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> NOVaMARutil2025 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(2, 8),
                new Tuple<int, int>(9,15),
                new Tuple<int, int>(16,23),
                new Tuple<int, int>(24,24)
            };

            List<Tuple<int, int>> NOVaMARfer = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 19),
                new Tuple<int, int>(20, 23),
                new Tuple<int, int>(24, 24)
            };

            List<Tuple<int, int>> NOVaMARfer2023 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 18),
                new Tuple<int, int>(19, 23),
                new Tuple<int, int>(24, 24)
            };

            List<Tuple<int, int>> NOVaMARfer2024 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 18),
                new Tuple<int, int>(19, 23),
                new Tuple<int, int>(24, 24)
            };

            List<Tuple<int, int>> NOVaMARfer2025 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(2, 18),
                new Tuple<int, int>(19, 24)

            };

            List<Tuple<int, int>> ABRSETOUTutil = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 10),
                new Tuple<int, int>(11, 20),
                new Tuple<int, int>(21, 24)
            };

            List<Tuple<int, int>> ABRSETOUTutil2023 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 14),
                new Tuple<int, int>(15, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> ABRSETOUTutil2024 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 14),
                new Tuple<int, int>(15, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> ABRSETOUTutil2025 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(2, 8),
                new Tuple<int, int>(9, 15),
                new Tuple<int, int>(16, 22),
                new Tuple<int, int>(23, 24)

            };

            List<Tuple<int, int>> ABRSETOUTfer = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 18),
                new Tuple<int, int>(19, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> ABRSETOUTfer2023 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 18),
                new Tuple<int, int>(19, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> ABRSETOUTfer2024 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 18),
                new Tuple<int, int>(19, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> ABRSETOUTfer2025 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 17),
                new Tuple<int, int>(18, 24)

            };

            List<Tuple<int, int>> MAIaAGOutil = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 7),
                new Tuple<int, int>(8, 10),
                new Tuple<int, int>(11, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> MAIaAGOutil2023 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 15),
                new Tuple<int, int>(16, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> MAIaAGOutil2024 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 16),
                new Tuple<int, int>(17, 22),
                new Tuple<int, int>(23, 24)
            };

            List<Tuple<int, int>> MAIaAGOutil2025 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(9, 17),
                new Tuple<int, int>(18, 22),
                new Tuple<int, int>(23, 24)

            };

            List<Tuple<int, int>> MAIaAGOfer = new List<Tuple<int, int>> {
                new Tuple<int, int>(1,18),
                new Tuple<int, int>(19,22),
                new Tuple<int, int>(23,24)
            };

            List<Tuple<int, int>> MAIaAGOfer2023 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1,18),
                new Tuple<int, int>(19,22),
                new Tuple<int, int>(23,24)
            };

            List<Tuple<int, int>> MAIaAGOfer2024 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1,18),
                new Tuple<int, int>(19,22),
                new Tuple<int, int>(23,24)
            };

            List<Tuple<int, int>> MAIaAGOfer2025 = new List<Tuple<int, int>> {
                new Tuple<int, int>(1,17),
                new Tuple<int, int>(18,22),
                new Tuple<int, int>(23,24)

            };

            switch (data.Month)
            {
                case 1:
                case 2:
                case 3:
                case 11:
                case 12:
                    if (ehFeriado || data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (pat2023)
                        {
                            return NOVaMARfer2023;
                        }
                        else if (pat2024)
                        {
                            return NOVaMARfer2024;
                        }
                        else if (pat2025)
                        {
                            return NOVaMARfer2025;
                        }
                        else
                            return NOVaMARfer;
                    }
                    else
                    {
                        if (pat2023)
                        {
                            return NOVaMARutil2023;
                        }
                        else if (pat2024)
                        {
                            return NOVaMARutil2024;
                        }
                        else if (pat2025)
                        {
                            return NOVaMARutil2025;
                        }
                        else
                            return NOVaMARutil;
                    }
                case 4:
                case 9:
                case 10:
                    if (ehFeriado || data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (pat2023)
                        {
                            return ABRSETOUTfer2023;
                        }
                        else if (pat2024)
                        {
                            return ABRSETOUTfer2024;
                        }
                        else if (pat2025)
                        {
                            return ABRSETOUTfer2025;
                        }
                        else
                            return ABRSETOUTfer;
                    }
                    else
                    {
                        if (pat2023)
                        {
                            return ABRSETOUTutil2023;
                        }
                        else if (pat2024)
                        {
                            return ABRSETOUTutil2024;
                        }
                        else if (pat2025)
                        {
                            return ABRSETOUTutil2025;
                        }
                        else
                            return ABRSETOUTutil;
                    }
                case 5:
                case 6:
                case 7:
                case 8:
                    if (ehFeriado || data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (pat2023)
                        {
                            return MAIaAGOfer2023;
                        }
                        else if (pat2024)
                        {
                            return MAIaAGOfer2024;
                        }
                        else if (pat2025)
                        {
                            return MAIaAGOfer2025;
                        }
                        else
                            return MAIaAGOfer;
                    }
                    else
                    {
                        if (pat2023)
                        {
                            return MAIaAGOutil2023;
                        }
                        else if (pat2024)
                        {
                            return MAIaAGOutil2024;
                        }
                        else if (pat2025)
                        {
                            return MAIaAGOutil2025;
                        }
                        else
                            return MAIaAGOutil;
                    }

            }
            return null;

        }

        public static Dictionary<int, string> GetIntervalosHoararios(DateTime data, bool pat2023 = false, bool pat2024 = false, bool pat2025 = false)
        {
            var feriados = Tools.feriados;
            Boolean ehFeriado = false;

            if (feriados.Any(x => x.Date == data.Date))
            {
                ehFeriado = true;
            }

            Dictionary<int, string> NOVaMARutil = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "PESADA"},
                    {11, "PESADA"},
                    {12, "PESADA"},
                    {13, "PESADA"},
                    {14, "PESADA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> NOVaMARutil2023 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "PESADA"},
                    {14, "PESADA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> NOVaMARutil2024 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "MEDIA"},
                    {14, "PESADA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> NOVaMARutil2025 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "MEDIA"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "MEDIA"},
                    {14, "MEDIA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "PESADA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> NOVaMARfer = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "LEVE"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "MEDIA"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> NOVaMARfer2023 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "MEDIA"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> NOVaMARfer2024 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "MEDIA"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> NOVaMARfer2025 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "MEDIA"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> MAIOaAGOutil = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "MEDIA"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "PESADA"},
                    {11, "PESADA"},
                    {12, "PESADA"},
                    {13, "PESADA"},
                    {14, "PESADA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> MAIOaAGOutil2023 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "MEDIA"},
                    {14, "MEDIA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> MAIOaAGOutil2024 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "MEDIA"},
                    {14, "MEDIA"},
                    {15, "MEDIA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> MAIOaAGOutil2025 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "MEDIA"},
                    {14, "MEDIA"},
                    {15, "MEDIA"},
                    {16, "MEDIA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> MAIOaAGOfer = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "LEVE"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> MAIOaAGOfer2023 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "LEVE"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> MAIOaAGOfer2024 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "LEVE"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> MAIOaAGOfer2025 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "MEDIA"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "LEVE"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> ABRSETOUTutil = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "PESADA"},
                    {11, "PESADA"},
                    {12, "PESADA"},
                    {13, "PESADA"},
                    {14, "PESADA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> ABRSETOUTutil2023 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "MEDIA"},
                    {14, "PESADA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> ABRSETOUTutil2024 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "MEDIA"},
                    {14, "PESADA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> ABRSETOUTutil2025 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "MEDIA"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "MEDIA"},
                    {9, "MEDIA"},
                    {10, "MEDIA"},
                    {11, "MEDIA"},
                    {12, "MEDIA"},
                    {13, "MEDIA"},
                    {14, "MEDIA"},
                    {15, "PESADA"},
                    {16, "PESADA"},
                    {17, "PESADA"},
                    {18, "PESADA"},
                    {19, "PESADA"},
                    {20, "PESADA"},
                    {21, "PESADA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            Dictionary<int, string> ABRSETOUTfer = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "LEVE"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> ABRSETOUTfer2023 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "LEVE"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> ABRSETOUTfer2024 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "LEVE"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "LEVE"},
                    {23, "LEVE"},

                };

            Dictionary<int, string> ABRSETOUTfer2025 = new Dictionary<int, string>() {//<hora,patamar>
                    {0, "LEVE"},
                    {1, "LEVE"},
                    {2, "LEVE"},
                    {3, "LEVE"},
                    {4, "LEVE"},
                    {5, "LEVE"},
                    {6, "LEVE"},
                    {7, "LEVE"},
                    {8, "LEVE"},
                    {9, "LEVE"},
                    {10, "LEVE"},
                    {11, "LEVE"},
                    {12, "LEVE"},
                    {13, "LEVE"},
                    {14, "LEVE"},
                    {15, "LEVE"},
                    {16, "LEVE"},
                    {17, "MEDIA"},
                    {18, "MEDIA"},
                    {19, "MEDIA"},
                    {20, "MEDIA"},
                    {21, "MEDIA"},
                    {22, "MEDIA"},
                    {23, "MEDIA"},

                };

            switch (data.Month)
            {
                case 1:
                case 2:
                case 3:
                case 11:
                case 12:
                    if (ehFeriado || data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (pat2023)
                        {
                            return NOVaMARfer2023;
                        }
                        else if (pat2024)
                        {
                            return NOVaMARfer2024;
                        }
                        else if (pat2025)
                        {
                            return NOVaMARfer2025;
                        }
                        else
                            return NOVaMARfer;
                    }
                    else
                    {
                        if (pat2023)
                        {
                            return NOVaMARutil2023;
                        }
                        else if (pat2024)
                        {
                            return NOVaMARutil2024;
                        }
                        else if (pat2025)
                        {
                            return NOVaMARutil2025;
                        }
                        else
                            return NOVaMARutil;
                    }
                case 4:
                case 9:
                case 10:
                    if (ehFeriado || data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (pat2023)
                        {
                            return ABRSETOUTfer2023;
                        }
                        else if (pat2024)
                        {
                            return ABRSETOUTfer2024;
                        }
                        else if (pat2025)
                        {
                            return ABRSETOUTfer2025;
                        }
                        else
                            return ABRSETOUTfer;
                    }
                    else
                    {
                        if (pat2023)
                        {
                            return ABRSETOUTutil2023;
                        }
                        else if (pat2024)
                        {
                            return ABRSETOUTutil2024;
                        }
                        else if (pat2025)
                        {
                            return ABRSETOUTutil2025;
                        }
                        else
                            return ABRSETOUTutil;
                    }
                case 5:
                case 6:
                case 7:
                case 8:
                    if (ehFeriado || data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (pat2023)
                        {
                            return MAIOaAGOfer2023;
                        }
                        else if (pat2024)
                        {
                            return MAIOaAGOfer2024;
                        }
                        else if (pat2025)
                        {
                            return MAIOaAGOfer2025;
                        }
                        else
                            return MAIOaAGOfer;
                    }
                    else
                    {
                        if (pat2023)
                        {
                            return MAIOaAGOutil2023;
                        }
                        else if (pat2024)
                        {
                            return MAIOaAGOutil2024;
                        }
                        else if (pat2025)
                        {
                            return MAIOaAGOutil2025;
                        }
                        else
                            return MAIOaAGOutil;
                    }


            }
            return null;
        }

        public static Tuple<int, int, int> GetWeekPatamares(DateTime date, bool patamares2019)
        {
            var inicioSemanaOperativa = date;
            while (inicioSemanaOperativa.DayOfWeek != DayOfWeek.Saturday) inicioSemanaOperativa = inicioSemanaOperativa.AddDays(-1);
            var fimSemanaOperativa = inicioSemanaOperativa.AddDays(6);

            return GetHorasPatamares(inicioSemanaOperativa, fimSemanaOperativa, patamares2019);
        }

        public static int[] GetCalendarDaysFromOperativeMonth(int year, int month)
        {

            var checkSum = 0;

            var result = new int[6];
            int i = 0;

            var monthStart = new DateTime(year, month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            int daysToRemove = 0;
            if (monthStart.DayOfWeek != DayOfWeek.Saturday) daysToRemove = 1 + (int)monthStart.DayOfWeek;

            var week = monthStart.AddDays(-daysToRemove);//.AddDays(-7);

            var weekStart = monthStart;
            var weekEnd = week.AddDays(7);

            do
            {
                result[i++] = (weekEnd - weekStart).Days;
                checkSum += (weekEnd - weekStart).Days;

                weekStart = weekEnd;


                if (weekStart.AddDays(7) > nextMonthStart)
                    weekEnd = nextMonthStart;
                else
                    weekEnd = weekStart.AddDays(7);

            } while (weekStart.Month == month);

            if ((nextMonthStart - monthStart).Days != checkSum) throw new Exception();


            return result;
        }
        public static List<Tuple<DateTime, int>> GetNumDatSem(DateTime data, int num)
        {
            List<Tuple<DateTime, int>> semanas = new List<Tuple<DateTime, int>>();
            for (int i = 0; i <= 11; i++)//horizonte de doze semanas
            {
                var NumSem = GetWeekNumberAndYear(data);
                semanas.Add(new Tuple<DateTime, int>(data, NumSem.Item1));
                data = data.AddDays(7);
            }
            return semanas;
        }
        public static Tuple<int, int> GetWeekNumberAndYear(DateTime date)
        {

            var nextFriday = date.DayOfWeek == DayOfWeek.Saturday ? date.AddDays(6) :
                date.AddDays((int)DayOfWeek.Friday - (int)date.DayOfWeek);

            var y = nextFriday.Year;

            var yearStart = new DateTime(y, 1, 1);
            yearStart = yearStart.AddDays(-1 * ((int)yearStart.DayOfWeek + 1) % 7);

            var weekNumber = (int)Math.Floor(((date - yearStart).TotalDays) / 7) + 1;

            return new Tuple<int, int>(weekNumber, y);
        }

        public static List<Acomph> GetAcomphData(DateTime inicio, DateTime fim)
        {
            using (IPDOEntities ctx = new IPDOEntities())
            {
                return ctx.ACOMPH.Where(x => x.Data >= inicio && x.Data <= fim)
                    .Select(x => new Acomph() { dt = x.Data, posto = x.Posto, qNat = x.Vaz_nat, qInc = x.Vaz_Inc })
                    .ToList();
            }
        }
        public static (DateTime revDate, int rev) GetCurrRev(DateTime date)
        {
            var currRevDate = date;

            do
            {
                currRevDate = currRevDate.AddDays(1);
            } while (currRevDate.DayOfWeek != DayOfWeek.Friday);
            var currRevNum = currRevDate.Day / 7 - (currRevDate.Day % 7 == 0 ? 1 : 0);

            return (currRevDate, currRevNum);
        }

        public static (DateTime revDate, int rev) GetNextRev(DateTime date, int increment = 1)
        {
            var currRevDate = GetCurrRev(date).revDate;

            var nextRevDate = currRevDate.AddDays(7 * increment);
            var nextRevNum = nextRevDate.Day / 7 - (nextRevDate.Day % 7 == 0 ? 1 : 0);

            return (nextRevDate, nextRevNum);
        }

        public static string GetDecompRecentExec(DateTime data, bool nextRV = false)
        {
            DateTime Ve;
            DateTime dataEstudo = data;
            if (dataEstudo.DayOfWeek == DayOfWeek.Friday)
            {
                Ve = dataEstudo.AddDays(-1);
            }
            else
            {
                Ve = dataEstudo;
            }

            var rev = Tools.GetCurrRev(Ve);
            if (nextRV == true)
            {
                rev = Tools.GetNextRev(Ve);
            }
            string mapcut = "mapcut.rv" + rev.rev.ToString();
            string cortdeco = "cortdeco.rv" + rev.rev.ToString();



            for (int i = 1; i <= 10; i++)
            {
                string camDecomp = @"K:\4_curto_prazo\" + rev.revDate.ToString("yyyy_MM") + "\\DEC_ONS_" + rev.revDate.ToString("MMyyyy") + "_RV" + rev.rev.ToString() + $"_VE_ccee ({i})";

                string etcFile = Path.Combine(camDecomp, "etc.zip");
                if (Directory.Exists(camDecomp))
                {
                    var arqs = Directory.GetFiles(camDecomp).ToList();
                    if (arqs.All(x => Path.GetFileName(x).ToLower() != mapcut) && arqs.All(x => Path.GetFileName(x).ToLower() != cortdeco))
                    {
                        if (File.Exists(etcFile))
                        {
                            Ionic.Zip.ZipFile arquivoZip = Ionic.Zip.ZipFile.Read(etcFile);
                            try
                            {
                                foreach (ZipEntry e in arquivoZip)
                                {
                                    e.Extract(camDecomp, ExtractExistingFileAction.OverwriteSilently);
                                }
                                arquivoZip.Dispose();
                                arqs = Directory.GetFiles(camDecomp).ToList();
                                if (File.Exists(Path.Combine(camDecomp, mapcut)) && File.Exists(Path.Combine(camDecomp, cortdeco)))
                                {
                                    return camDecomp;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                    }
                    else if (File.Exists(Path.Combine(camDecomp, mapcut)) && File.Exists(Path.Combine(camDecomp, cortdeco)))
                    {
                        return camDecomp;
                    }
                    //return camDecomp;
                }
            }
            return GetDecompRecentExec(data.AddDays(-7), true);
        }
        public static string GetDCref(DateTime dt)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var data = dt;
            var dataDC = dt;
            DateTime dataRef = data;
            string xPublicacao = "";

            string folder = "";
            int contDc = 0;
            bool OkDc = false;
            while (OkDc == false && contDc < 30)
            {
                DateTime dat = dataDC;
                DateTime datVE = dataDC;
                if (dat.DayOfWeek == DayOfWeek.Friday)
                {
                    datVE = dat.AddDays(-1);
                }
                var rev = GetCurrRev(datVE);
                //H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\2023\09_set\Relatorio_Sumario-202309-sem5
                int semana = rev.rev + 1;
                var mes = GetMonthNumAbrev(rev.revDate.Month);//dataRef

                for (int p = 5; p >= 1; p--)//verfirifica se existe republicaçoes e prioriza na escolha do deck
                {
                    var camX = $@"H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\{rev.revDate:yyyy}\{mes}\Relatorio_Sumario-{rev.revDate:yyyyMM}-sem" + semana.ToString() + $"_{p}aPublicacao";
                    if (Directory.Exists(camX))
                    {
                        xPublicacao = camX;
                        OkDc = true;
                        break;
                    }
                }
                if (xPublicacao != "")
                {
                    folder = xPublicacao;
                }
                else
                {
                    var cam = $@"H:\Middle - Preço\Resultados_Modelos\DECOMP\CCEE_DC\{rev.revDate:yyyy}\{mes}\Relatorio_Sumario-{rev.revDate:yyyyMM}-sem" + semana.ToString();
                    if (Directory.Exists(cam))
                    {
                        folder = cam;
                        OkDc = true;
                    }
                    else
                    {
                        contDc++;
                        dataDC = dataDC.AddDays(-1);
                    }
                }

            }
            return folder;
        }

        public static string GetDessemRecent(DateTime data, bool deckSabado = false)
        {
            DateTime dataRef = data;
            string folder = "";
            int cont = 0;
            bool Ok = false;
            if (deckSabado)
            {
                DateTime sabAnt = data;
                while (Ok == false && cont < 30)
                {
                    while (sabAnt.DayOfWeek != DayOfWeek.Saturday) sabAnt = sabAnt.AddDays(-1);

                    DateTime dat = sabAnt;
                    DateTime datVE = sabAnt;

                    var rev = Tools.GetCurrRev(sabAnt);
                    //H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\2021\01_jan\RV3\DS_CCEE_012021_SEMREDE_RV3D19
                    var mes = Tools.GetMonthNumAbrev(rev.revDate.Month);//dataRef
                    var cam = $@"H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{sabAnt.Day:00}";
                    if (Directory.Exists(cam))
                    {
                        folder = cam;
                        Ok = true;
                    }
                    else
                    {
                        cont++;
                        sabAnt = sabAnt.AddDays(-1);
                    }
                }
            }
            else
            {
                while (Ok == false && cont < 30)
                {
                    DateTime dat = dataRef;
                    DateTime datVE = dataRef;
                    if (dat.DayOfWeek == DayOfWeek.Friday)
                    {
                        datVE = dat.AddDays(-1);
                    }
                    var rev = Tools.GetCurrRev(datVE);
                    //H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\2021\01_jan\RV3\DS_CCEE_012021_SEMREDE_RV3D19
                    var mes = Tools.GetMonthNumAbrev(rev.revDate.Month);//dataRef
                    var cam = $@"H:\Middle - Preço\Resultados_Modelos\DESSEM\CCEE_DS\{rev.revDate:yyyy}\{mes}\RV{rev.rev}\DS_CCEE_{rev.revDate:MMyyyy}_SEMREDE_RV{rev.rev}D{dat.Day:00}";
                    if (Directory.Exists(cam))
                    {
                        folder = cam;
                        Ok = true;
                    }
                    else
                    {
                        cont++;
                        dataRef = dataRef.AddDays(-1);
                    }
                }
            }

            return folder;
        }
        public static float GetRespotValor(string dia, int hora, int meia, Compass.CommomLibrary.EntdadosDat.DpBlock blocoDp)
        {
            float valor = 0;

            //var linhasDp = blocoDp.Where(x => x.DiaInic == dia && x.HoraInic == hora && x.MeiaHoraInic == meia).Select(x => x.Demanda).Sum();
            var linhasDpSeSul = blocoDp.Where(x => x.DiaInic == dia && x.HoraInic == hora && x.MeiaHoraInic == meia && (x.Subsist == 1 || x.Subsist == 2)).Select(x => x.Demanda).Sum();
            valor = linhasDpSeSul * 0.05f;
            return valor;
        }

        public static string GetPrevCargaDsCSV(DateTime data, string submercado)
        {
            var oneDrive_DESSEM = Path.Combine(@"C:\Enercore\Energy Core Trading\Energy Core Pricing - Documents\Arquivos_DESSEM");
            var kPath = @"K:\5_dessem\Arquivos_DESSEM";

            if (!Directory.Exists(oneDrive_DESSEM))
            {
                oneDrive_DESSEM = oneDrive_DESSEM.Replace("Energy Core Pricing - Documents", "Energy Core Pricing - Documentos");
            }
            var oneDrive_MES = Path.Combine(oneDrive_DESSEM, data.ToString("MM_yyyy"));

            var oneDrive_DIA = Path.Combine(oneDrive_DESSEM, data.ToString("MM_yyyy"), data.ToString("dd"), "DeckPrevCarga");
            var k_DIA = Path.Combine(kPath, data.ToString("MM_yyyy"), data.ToString("dd"), "DeckPrevCarga");

            string nameFolder = "PrevCargaDESSEM_" + data.ToString("yyyy-MM-dd");

            var full_Path = Path.Combine(oneDrive_DIA, nameFolder, $"PrevCargaDESSEM_{submercado}_{data:yyyy-MM-dd}.csv");
            var full_PathK = Path.Combine(k_DIA, nameFolder, $"PrevCargaDESSEM_{submercado}_{data:yyyy-MM-dd}.csv");

            if (File.Exists(full_PathK))
            {
                return full_PathK;
            }
            else if (File.Exists(full_Path))
            {
                return full_Path;
            }
            else
            {
                string path = GetPrevCargaDsCSV(data.AddDays(-1), submercado);
                return path;
            }



        }


        public static List<Tuple<int, DateTime, double, float>> GetDadosPrevCargaDS(DateTime data)
        {
            List<Tuple<int, DateTime, double, float>> dados = new List<Tuple<int, DateTime, double, float>>();//sub,data,estagio,carga
            List<string> subs = new List<string> { "SECO", "S", "NE", "N" };

            int subNum;
            foreach (var submercado in subs)
            {
                switch (submercado)
                {
                    case "SECO":
                        subNum = 1;
                        break;

                    case "S":
                        subNum = 2;
                        break;

                    case "NE":
                        subNum = 3;
                        break;

                    case "N":
                        subNum = 4;
                        break;

                    default:
                        subNum = 0;
                        break;
                }
                string arq = GetPrevCargaDsCSV(data, submercado);

                var linhas = File.ReadAllLines(arq).Skip(1).ToList();
                foreach (var l in linhas)
                {
                    var partes = l.Split(';').ToList();
                    DateTime dia = Convert.ToDateTime(partes[1]);
                    float carga = float.Parse(partes[2]);
                    double est = (dia.TimeOfDay.TotalMinutes / 30) + 1;
                    dados.Add(new Tuple<int, DateTime, double, float>(subNum, dia, est, carga));
                }

                for (DateTime d = data; d <= data.AddDays(6); d = d.AddDays(1))//replica os dados do ultimo dia caso não exista os dados do dia no csv
                {
                    for (int i = 0; i <= 1440; i += 30)
                    {
                        var dadolin = dados.Where(x => x.Item1 == subNum && x.Item2 == d.AddMinutes(i)).FirstOrDefault();
                        if (dadolin == null)
                        {
                            var dadolinAnt = dados.Where(x => x.Item1 == subNum && x.Item2 == d.AddDays(-1).AddMinutes(i)).FirstOrDefault();
                            if (dadolinAnt != null)
                            {
                                dados.Add(new Tuple<int, DateTime, double, float>(subNum, d.AddMinutes(i), dadolinAnt.Item3, dadolinAnt.Item4));
                            }
                        }
                    }
                }
            }


            return dados;
        }
        public static string GetNPTXT(DateTime d, bool recursivo = false)
        {
            var oneDrive_DESSEM = Path.Combine(@"C:\Enercore\Energy Core Trading\Energy Core Pricing - Documents\Arquivos_DESSEM");
            var kPath = @"K:\5_dessem\Arquivos_DESSEM";

            string arqName = $"NP{d:ddMMyyyy}.txt";

            if (!Directory.Exists(oneDrive_DESSEM))
            {
                oneDrive_DESSEM = oneDrive_DESSEM.Replace("Energy Core Pricing - Documents", "Energy Core Pricing - Documentos");
            }

            var oneDrive_DIA = Path.Combine(oneDrive_DESSEM, d.ToString("MM_yyyy"), d.ToString("dd"));
            var k_DIA = Path.Combine(kPath, d.ToString("MM_yyyy"), d.ToString("dd"));

            if (File.Exists(Path.Combine(k_DIA, arqName)))
            {
                return Path.Combine(k_DIA, arqName);
            }
            else if (File.Exists(Path.Combine(oneDrive_DIA, arqName)))
            {
                return Path.Combine(oneDrive_DIA, arqName);
            }
            else
            {
                //como pode estar criando um deck de um dia no futuro e tambem buscando dados de um NP do futuro, será usado o dados mais recente e caso não seja essa situação, uma exceção sera lançada interrompendo o processo
                DateTime hoje = DateTime.Today;
                if (d >= hoje || recursivo == true)
                {
                    string NP_recente = GetNPTXT(d.AddDays(-1), true);// chamada recursiva até encontrar o mais recente
                    return NP_recente;
                }

                throw new NotImplementedException($"Arquivo Níveis de partida não encontrados para criação do Deflant.dat, arquivo {arqName} necessário");

            }

            //return "";
        }

        public static float GetNPValue(string NPtxt, string montante)
        {
            float valor = 0f;
            var linhas = File.ReadAllLines(NPtxt).ToList();

            foreach (var l in linhas)
            {
                float d = 0f;
                var campos = l.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (campos[0].Trim() == montante)
                {
                    valor = float.TryParse(campos[2].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out d) ? d : 0;
                    return valor;
                }
            }

            return valor;
        }

        public static DateTime DateOfline(int dia, DateTime dataDeck, bool passado = false)
        {
            //DateTime dta = new DateTime(dataDeck.Year, dataDeck.Month, dia);
            DateTime dta = new DateTime(dataDeck.Year, dataDeck.Month, dataDeck.Day);

            //ajustando viradas de meses 

            if (passado == true)
            {
                //if (dta.Day > dataDeck.Day)
                //{
                //    dta = dta.AddMonths(-1);
                //}
                if (dia > dataDeck.Day)
                {
                    dta = new DateTime(dataDeck.Year, dataDeck.AddMonths(-1).Month, dia);
                }
                else
                {
                    dta = new DateTime(dataDeck.Year, dataDeck.Month, dia);
                }
            }
            else
            {
                if (dataDeck.Day < 10)
                {
                    if (dia > 20)//if (dta.Day > 20)
                    {
                        //dta = dta.AddMonths(-1);
                        dta = new DateTime(dataDeck.Year, dataDeck.AddMonths(-1).Month, dia);
                    }
                    else
                    {
                        dta = new DateTime(dataDeck.Year, dataDeck.Month, dia);
                    }
                }
                else if (dataDeck.Day > 20 && dia < 10/*dta.Day < 10*/)
                {
                    //dta = dta.AddMonths(1);
                    dta = new DateTime(dataDeck.Year, dataDeck.AddMonths(1).Month, dia);
                }
                else
                {
                    dta = new DateTime(dataDeck.Year, dataDeck.Month, dia);
                }
            }
            return dta;
        }

    }

    public class SemanaOperativa
    {
        public int HorasPat1 { get; set; }
        public int HorasPat2 { get; set; }
        public int HorasPat3 { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }

        //public SemanaOperativa(DateTime inicio)
        //{
        //    this.Inicio = Inicio;
        //    this.Fim = Inicio.AddDays(6);
        //    var pat = Tools.GetWeekPatamares(Inicio);
        //    this.HorasPat1 = pat.Item1;
        //    this.HorasPat2 = pat.Item2;
        //    this.HorasPat3 = pat.Item3;
        //}

        public SemanaOperativa(DateTime i, DateTime f, bool patamares2019, bool patamares2023 = false, bool patamares2024 = false)
        {
            this.Inicio = i;
            this.Fim = f;
            var pat = Tools.GetHorasPatamares(i, f, patamares2019, patamares2023, patamares2024);
            this.HorasPat1 = pat.Item1;
            this.HorasPat2 = pat.Item2;
            this.HorasPat3 = pat.Item3;

        }

        public SemanaOperativa(DateTime f, bool patamares2019 = true) : this(f.AddDays(-6), f, patamares2019)
        {

        }


        public SemanaOperativa Proxima()
        {
            return new SemanaOperativa(Fim.AddDays(1), Fim.AddDays(7), true);
        }

        public int NumeroDaSemana
        {
            get
            {
                var mop = MesOperativo.CreateSemanal(Fim.Year, 1, true);
                var num = ((int)(Fim - mop.SemanasOperativas.First().Inicio).TotalDays / 7 + 1);
                return num;
            }
        }

        public int Ano { get { return Fim.Year; } }
    }



    public class MesOperativo
    {
        private MesOperativo()
        {
            this.SemanasOperativas = new List<SemanaOperativa>();
        }
        public static MesOperativo CreateSemanal(int ano, int mes, bool patamares2019, bool patamares2023 = false, bool patamares2024 = false)
        {
            var mOper = new MesOperativo();
            mOper.Ano = ano;
            mOper.Mes = mes;

            var datetime = new DateTime(ano, mes, 1);
            while (datetime.DayOfWeek != DayOfWeek.Saturday) datetime = datetime.AddDays(-1);
            mOper.Inicio = datetime;

            datetime = datetime.AddDays(6);
            while (datetime.Month == mes)
            {
                mOper.SemanasOperativas.Add(
                    new SemanaOperativa(datetime.AddDays(-6), datetime, patamares2019, patamares2023, patamares2024)
                    );

                datetime = datetime.AddDays(7);
            }

            if (datetime.Day == 7)
            {
                mOper.SemanasOperativas.Add(
                    new SemanaOperativa((new DateTime(ano, mes, 1)).AddMonths(1), (new DateTime(ano, mes, 1)).AddMonths(2).AddDays(-1), patamares2019, patamares2023, patamares2024)
                    );

                mOper.Fim = datetime.AddDays(-7);
                mOper.DiasMes2 = 0;
            }
            else
            {
                mOper.SemanasOperativas.Add(
                    new SemanaOperativa(datetime.AddDays(-6), datetime, patamares2019, patamares2023, patamares2024)
                    );

                mOper.SemanasOperativas.Add(
                    new SemanaOperativa(datetime.AddDays(1), (new DateTime(ano, mes, 1)).AddMonths(2).AddDays(-1), patamares2019, patamares2023, patamares2024)
                    );

                mOper.Fim = datetime.AddDays(-7);
                mOper.DiasMes2 = datetime.Day;

            }
            mOper.MesSeguinte = datetime.Month;
            mOper.AnoSeguinte = datetime.Year;

            mOper.Estagios = mOper.SemanasOperativas.Count - 1;

            for (int i = 0; i < mOper.SemanasOperativas.Count; i++)
            {
                //if (Tools.inicioVR.Any(x => mOper.SemanasOperativas[i].Inicio <= x && mOper.SemanasOperativas[i].Fim >= x)) mOper.EstagioInicioHorarioVerao = i + 1;
                //if (Tools.fimVR.Any(x => mOper.SemanasOperativas[i].Inicio <= x && mOper.SemanasOperativas[i].Fim >= x)) mOper.EstagioFimHorarioVerao = i + 1;
            }


            return mOper;
        }

        public static MesOperativo CreateSemanalDadgnl(int ano, int mes, bool patamares2019, bool patamares2024 = false)
        {
            var mOper = new MesOperativo();
            mOper.Ano = ano;
            mOper.Mes = mes;

            var datetime = new DateTime(ano, mes, 1);
            while (datetime.DayOfWeek != DayOfWeek.Saturday) datetime = datetime.AddDays(-1);
            mOper.Inicio = datetime;

            datetime = datetime.AddDays(6);
            while (datetime.Month == mes)
            {
                mOper.SemanasOperativas.Add(
                    new SemanaOperativa(datetime.AddDays(-6), datetime, patamares2019, patamares2024)
                    );

                datetime = datetime.AddDays(7);
            }

            //if (datetime.Day == 7)
            //{
            //    mOper.SemanasOperativas.Add(
            //        new SemanaOperativa((new DateTime(ano, mes, 1)).AddMonths(1), datetime, patamares2019)
            //        );

            //    mOper.Fim = datetime.AddDays(-7);
            //    mOper.DiasMes2 = 0;
            //}
            //else
            //{
            //mOper.SemanasOperativas.Add(
            //    new SemanaOperativa(datetime.AddDays(-6), datetime, patamares2019)
            //    );

            mOper.SemanasOperativas.Add(
                new SemanaOperativa(datetime.AddDays(1), (new DateTime(ano, mes, 1)).AddMonths(2).AddDays(-1), patamares2019, patamares2024)
                );

            mOper.Fim = datetime.AddDays(-7);
            mOper.DiasMes2 = datetime.Day;

            //}
            mOper.MesSeguinte = datetime.Month;
            mOper.AnoSeguinte = datetime.Year;

            mOper.Estagios = mOper.SemanasOperativas.Count - 1;

            for (int i = 0; i < mOper.SemanasOperativas.Count; i++)
            {
                //if (Tools.inicioVR.Any(x => mOper.SemanasOperativas[i].Inicio <= x && mOper.SemanasOperativas[i].Fim >= x)) mOper.EstagioInicioHorarioVerao = i + 1;
                //if (Tools.fimVR.Any(x => mOper.SemanasOperativas[i].Inicio <= x && mOper.SemanasOperativas[i].Fim >= x)) mOper.EstagioFimHorarioVerao = i + 1;
            }


            return mOper;
        }

        public static MesOperativo CreateMensal(int ano, int mes, bool patamares2019, bool patamares2023 = false, bool patamares2024 = false)
        {
            var mOper = new MesOperativo();

            mOper.Ano = ano;
            mOper.Mes = mes;

            var datetime = new DateTime(ano, mes, 1);
            //while (datetime.DayOfWeek != DayOfWeek.Saturday) datetime = datetime.AddDays(-1);
            mOper.Inicio = datetime;
            mOper.Fim = datetime.AddMonths(1).AddDays(-1);

            mOper.SemanasOperativas.Add(
                new SemanaOperativa(mOper.Inicio, mOper.Fim, patamares2019, patamares2023, patamares2024)
                );
            mOper.SemanasOperativas.Add(
                new SemanaOperativa(mOper.Inicio.AddMonths(1), datetime.AddMonths(2).AddDays(-1), patamares2019, patamares2023, patamares2024)
                );

            mOper.DiasMes2 = 0;

            mOper.MesSeguinte = datetime.AddMonths(1).Month;
            mOper.AnoSeguinte = datetime.AddMonths(1).Year;

            mOper.Estagios = 1;

            for (int i = 0; i < mOper.SemanasOperativas.Count; i++)
            {
                //if (Tools.inicioVR.Any(x => mOper.SemanasOperativas[i].Inicio <= x && mOper.SemanasOperativas[i].Fim >= x)) mOper.EstagioInicioHorarioVerao = i + 1;
                //if (Tools.fimVR.Any(x => mOper.SemanasOperativas[i].Inicio <= x && mOper.SemanasOperativas[i].Fim >= x)) mOper.EstagioFimHorarioVerao = i + 1;
            }

            return mOper;

        }

        public List<SemanaOperativa> SemanasOperativas { get; set; }

        public int Mes { get; private set; }

        public int Ano { get; private set; }

        public int Estagios { get; private set; }

        public int EstagiosReaisDoMesAtual
        {
            get
            {
                return Estagios - (DiasMes2 > 0 ? 1 : 0);
            }
        }

        public int DiasMes2 { get; private set; }

        public DateTime Inicio { get; private set; }
        public DateTime Fim { get; private set; }

        public int? EstagioInicioHorarioVerao { get; private set; }

        public int? EstagioFimHorarioVerao { get; private set; }
        public int MesSeguinte { get; set; }
        public int AnoSeguinte { get; set; }
    }



    public class Acomph
    {

        public DateTime dt { get; set; }
        public int posto { get; set; }
        public double qInc { get; set; }
        public double qNat { get; set; }

        public int semana
        {
            get
            {
                return Compass.CommomLibrary.Tools.GetWeekNumberAndYear(dt).Item1;
            }
        }
        public int ano
        {
            get
            {
                return Compass.CommomLibrary.Tools.GetWeekNumberAndYear(dt).Item2;
            }
        }
    }



}

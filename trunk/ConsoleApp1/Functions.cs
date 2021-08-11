using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace TrataInviab
{
    class Functions
    {
        public static int NumSemanasPasssadas(string file) // file = caminho da dadger
        {
            var linhas = File.ReadAllLines(file).ToList();
            var numSemLinha = linhas.Where(x => x.StartsWith("& NO. SEMANAS NO MES INIC. DO ESTUDO")).First();
            var num = Convert.ToInt32(numSemLinha.Substring(44, 4));
            return num;
        }

        public static int NumSemanasDecomp(string file) // file = caminho da dadger
        {
            var linhas = File.ReadAllLines(file).ToList();
            var numSemLinha = linhas.Where(x => x.StartsWith("& NO. SEMANAS NO MES INIC. DO ESTUDO")).First();
            var dias2MesLinha = linhas.Where(x => x.StartsWith("& NO. DIAS DO MES 2 NA ULT. SEMANA")).First();
            int numSemana = Convert.ToInt32(numSemLinha.Substring(39, 4));
            int dias2Mes = Convert.ToInt32(dias2MesLinha.Split(' ').Last());
            int periodoFinal = 0;
            if (numSemana != 0)
            {
                if (dias2Mes == 0)
                {
                    periodoFinal = numSemana;
                }
                else
                {
                    periodoFinal = (numSemana - 1);
                }
            }
            else
            {
                periodoFinal = 1;
            }
            return periodoFinal;
        }

        public static int NumEstagiosDecomp(string file) // file = caminho da dadger
        {
            var linhas = File.ReadAllLines(file).ToList();
            var numSemLinha = linhas.Where(x => x.StartsWith("& NO. SEMANAS NO MES INIC. DO ESTUDO")).First();
            var dias2MesLinha = linhas.Where(x => x.StartsWith("& NO. DIAS DO MES 2 NA ULT. SEMANA")).First();
            int numSemana = Convert.ToInt32(numSemLinha.Substring(39, 4));
            int dias2Mes = Convert.ToInt32(dias2MesLinha.Split(' ').Last());
            int periodoFinal = 0;
            if (numSemana != 0)
            {
                periodoFinal = numSemana;
            }
            else
            {
                periodoFinal = 1;
            }
            return periodoFinal;
        }

        public static string TipoDecomp(string file) // file = caminho da dadger
        {
            var linhas = File.ReadAllLines(file).ToList();
            var numSemLinha = linhas.Where(x => x.StartsWith("& NO. SEMANAS NO MES INIC. DO ESTUDO")).First();
            var numSem = Convert.ToInt32(numSemLinha.Substring(39, 4));
            string tipo = "";
            if (numSem != 0)
            {
                tipo = "SEMANAL";
            }
            else
            {
                tipo = "MENSAL";
            }
            return tipo;
        }

        public static DateTime DataEstudoDecomp(string file) // file = caminho da dadger
        {
            var linhas = File.ReadAllLines(file).ToList();
            var mesEstudoLinha = linhas.Where(x => x.StartsWith("& MES INICIAL DO ESTUDO")).First();
            var anoEstudoLinha = linhas.Where(x => x.StartsWith("& ANO INICIAL DO ESTUDO")).First();
            var mes = Convert.ToInt32(mesEstudoLinha.Substring(39, 2));
            var ano = Convert.ToInt32(anoEstudoLinha.Substring(39, 4));

            DateTime dataDecomp = new DateTime(ano, mes, 1);
            return dataDecomp;
        }
    }
}

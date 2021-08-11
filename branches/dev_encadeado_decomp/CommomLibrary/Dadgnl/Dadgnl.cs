using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadgnl {
    public class Dadgnl : BaseDocument {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"TG", new TgBlock()},
                    {"GS", new GsBlock()},    
                    {"NL", new NlBlock()},
                    {"GL", new GlBlock()},
                };
        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get { return blocos; }
        }

        public GlBlock BlocoGL { get { return (GlBlock)Blocos["GL"]; } set { Blocos["GL"] = value; } }
        public NlBlock BlocoNL { get { return (NlBlock)Blocos["NL"]; } set { Blocos["NL"] = value; } }
        public GsBlock BlocoGS { get { return (GsBlock)Blocos["GS"]; } set { Blocos["GS"] = value; } }
        public TgBlock BlocoTG { get { return (TgBlock)Blocos["TG"]; } set { Blocos["TG"] = value; } }

        public Dadgnl CreateNewRV0(int ano, int mes) {

            var dataAtual = new DateTime(ano, mes, 1);
            var dataSeguinte = dataAtual.AddMonths(1);

            Dadgnl result = new Dadgnl();

            //var semanas = Factory.SemanasDAO.GetByMesAno(mes, ano);

            //var semanasPatamates = Factory.SemanasPatamaresDAO.GetByMonth(mes, ano);
            //var semanasSeguintes = Factory.SemanasPatamaresDAO.GetByMonth(dataSeguinte.Month, dataSeguinte.Year);

            //if (semanas.primeiraSemana.Month != semanas.mes) {
            //    var dataAnterior = dataAtual.AddMonths(-1);
            //    semanasPatamates[0] = Semanas_Patamares.somaSemanas(semanasPatamates[0], Factory.SemanasPatamaresDAO.GetLastOrFirstByMonth(dataAnterior.Month, dataAnterior.Year, 0));
            //}
            //if (semanas.diasMes2 > 0) {
            //    semanasPatamates[semanas.semanas - 1] =
            //        Semanas_Patamares.somaSemanas(semanasPatamates[semanas.semanas - 1], semanasSeguintes[0]);
            //    semanasSeguintes.RemoveAt(0);
            //}

            //var intervalosMesSeguinte = (9 - semanas.semanas);

            result.BlocoNL = (NlBlock)this.BlocoNL.Clone();

            result.BlocoGS = (GsBlock)this.BlocoGS.Clone();
            //result.BlocoGS[0].SetValue(2, semanas.semanas);
            //result.BlocoGS[2].SetValue(2, semanas.semanas);
            //result.BlocoGS[1].SetValue(2, intervalosMesSeguinte);

            //result.BlocoGL = this.BlocoGL.CloneToRV0(semanasPatamates.Concat(semanasSeguintes).ToList(), semanas.primeiraSemana);

            //result.BlocoTG = this.BlocoTG.CloneToRV0(this.SemanaInicial.Value, semanas.primeiraSemana);

            return result;
        }

        public override bool IsComment(string line) {
            return line.StartsWith("&");
        }
        
    }
}

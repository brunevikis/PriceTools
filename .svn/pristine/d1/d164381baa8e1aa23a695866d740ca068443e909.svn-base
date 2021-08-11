using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadgnl {
    public class GlBlock : BaseBlock<GlLine> {

        public GlBlock CloneToRV0(/*List<Semanas_Patamares>*/ dynamic semanasPat, DateTime primeiraSemana) {

            var newBlock = new GlBlock();

            var usinas = this.Select(x => new { x.NumeroUsina, x.Subsistema }).Distinct();

            foreach (var usina in usinas) {

                var geracao = this.Where(x => x.NumeroUsina == usina.NumeroUsina).Select(x => new {
                    x.GeracaoPat1,
                    x.GeracaoPat2,
                    x.GeracaoPat3,
                    DataInicio = new DateTime(x.AnoInicio, x.MesInicio, x.DiaInicio)
                });
                var geracaoMax = new {
                    GeracaoPat1 = geracao.Max(x => x.GeracaoPat1),
                    GeracaoPat2 = geracao.Max(x => x.GeracaoPat2),
                    GeracaoPat3 = geracao.Max(x => x.GeracaoPat3),
                };

                var dataInicio = primeiraSemana;
                for (int i = 0; i < 9; i++) {
                    var gl = new GlLine() {
                        Identificacao = "GL",
                        DiaInicio = dataInicio.Day,
                        MesInicio = dataInicio.Month,
                        AnoInicio = dataInicio.Year,
                        NumeroUsina = usina.NumeroUsina,
                        Semana = (i + 1),
                        Subsistema = usina.Subsistema,
                        DuracaoPat1 = semanasPat[i].pesado.ToString(),
                        DuracaoPat2 = semanasPat[i].medio.ToString(),
                        DuracaoPat3 = semanasPat[i].leve.ToString(),
                    };

                    if (geracao.Any(x => x.DataInicio == dataInicio)) {
                        var geracaoDaSemana = geracao.First(x => x.DataInicio == dataInicio);
                        gl.GeracaoPat1 = geracaoDaSemana.GeracaoPat1;
                        gl.GeracaoPat2 = geracaoDaSemana.GeracaoPat2;
                        gl.GeracaoPat3 = geracaoDaSemana.GeracaoPat3;
                    } else {
                        gl.GeracaoPat1 = geracaoMax.GeracaoPat1;
                        gl.GeracaoPat2 = geracaoMax.GeracaoPat2;
                        gl.GeracaoPat3 = geracaoMax.GeracaoPat3;
                    }

                    newBlock.Add(gl);
                    dataInicio = dataInicio.AddDays(7);
                }
            }
            return newBlock;
        }

    }

    public class GlLine : BaseLine {

        public GlLine() : base() { this[0] = "GL"; }

        public string Identificacao { get { return this[0].ToString(); } set { this[0] = value; } }
        public int NumeroUsina { get { return (int)this[1]; } set { this[1] = value; } }
        public int Subsistema { get { return (int)this[2]; } set { this[2] = value; } }
        public int Semana { get { return (int)this[3]; } set { this[3] = value; } }
        public float GeracaoPat1 { get { return (float)this[4]; } set { this[4] = value; } }
        public float DuracaoPat1 { get { return (float)this[5]; } set { this[5] = value; } }
        public float GeracaoPat2 { get { return (float)this[6]; } set { this[6] = value; } }
        public float DuracaoPat2 { get { return (float)this[7]; } set { this[7] = value; } }
        public float GeracaoPat3 { get { return (float)this[8]; } set { this[8] = value; } }
        public float DuracaoPat3 { get { return (float)this[9]; } set { this[9] = value; } }
        public int DiaInicio { get { return (int)this[10]; } set { this[10] = value; } }
        public int MesInicio { get { return (int)this[11]; } set { this[11] = value; } }
        public int AnoInicio { get { return (int)this[12]; } set { this[12] = value; } }

        public override BaseField[] Campos { get { return GlCampos; } }

        static readonly BaseField[] GlCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "Id"),
                new BaseField(5  , 7 ,"I3"    , "Usina"),                
                new BaseField(10 , 11,"I2"    , "Subsistema"),
                new BaseField(15 , 16,"I2"    , "Semana"),
                new BaseField(20 , 29,"F10.0" , "Geracao Pat1"),
                new BaseField(30 , 34,"F5.0"  , "Duracao Pat1"),
                new BaseField(35 , 44,"F10.0" , "Geracao Pat2"),
                new BaseField(45 , 49,"F5.0"  , "Duracao Pat2"),
                new BaseField(50 , 59,"F10.0" , "Geracao Pat3"),
                new BaseField(60 , 64,"F5.0"  , "Duracao Pat3"),
                new BaseField(66 , 67,"Z2"    , "Dia inicio"),
                new BaseField(68 , 69,"Z2"    , "Mes inicio"),
                new BaseField(70 , 73,"Z4"    , "Ano inicio"),
            };
    }


}

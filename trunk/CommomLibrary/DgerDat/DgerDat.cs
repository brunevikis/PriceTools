using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.DgerDat {

    public enum TipoSimulacao {
        //=0 NAO SIMULA; =1 S.SINT.; =2 S.HIST.; =3 CONSIST)
        NAO_SIMULA = 0,
        S_SINTETICA = 1,
        S_HISTORICA = 2,
        CONSISTENCIA = 3
    }
    public enum TipoExecucao {
        //(1:EXECUCAO COMPLETA; 0:SIMULACAO FINAL)
        SIMULACAO_FINAL = 0,
        COMPLETA = 1,
    }

    public class DgerDat : BaseDocument {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Dger"             , new DgerBlock()},                 
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            this.NomeEstudo = lines[0];

            foreach (var line in lines.Skip(1)) {
                var newLine = Blocos["Dger"].CreateLine(line);
                Blocos["Dger"].Add(newLine);
            }
        }

        public override string ToText() {
            return NomeEstudo + "\n" + base.ToText();
        }

        DgerBlock dados { get { return (DgerBlock)blocos["Dger"]; } }

        public string NomeEstudo { get; set; }
        public int AnoEstudo {
            get { return int.Parse(dados[5].Params.Substring(0, 4).Trim()); }
            set { dados[5].Params = value.ToString(); }
        }
        public int MesEstudo {
            get { return int.Parse(dados[4].Params.Substring(0, 4).Trim()); }
            set { dados[4].Params = value.ToString().PadLeft(4); }
        }

        public DateTime DataEstudo { get { return new DateTime(AnoEstudo, MesEstudo, 1); } set { AnoEstudo = value.Year; MesEstudo = value.Month; } }

        public int AnosManutencaoUTE { get { return int.Parse(dados[31].Params.Substring(0, 4).Trim()); } }
        public int NumeroAnosEstudo { get { return int.Parse(dados[2].Params.Substring(0, 4).Trim()); } }
        public int NumeroAnosPosEstudo { get { return int.Parse(dados[7].Params.Substring(0, 4).Trim()); } }

        public TipoSimulacao Simulacao {
            get {
                return (TipoSimulacao)int.Parse(dados[25].Params.Substring(0, 4).Trim());
            }
            set {
                dados[25].Params = ((int)value).ToString().PadLeft(4) + dados[25].Params.Remove(0, 4);
            }
        }

        public TipoExecucao Execucao {
            get {
                return (TipoExecucao)int.Parse(dados[0].Params.Substring(0, 4).Trim());
            }
            set {
                dados[0].Params = ((int)value).ToString().PadLeft(4) + dados[25].Params.Remove(0, 4);
            }
        }

        

        public int TipoTendenciaHidrologia {
            get {
                return
                    int.Parse(dados[32].Params.Substring(5, 4).Trim());
            }
            set {
                dados[32].Params = dados[25].Params.Remove(5, 4).Insert(5, ((int)value).ToString().PadLeft(4));

            }
        }

        public int[] Flags {
            get {
                var txt = dados[57].Params.Substring(0, 24);
                return txt.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x=>int.Parse(x)).ToArray();                
            }
            set {
                dados[57].Params = string.Join(" ", value.Select(x=> x.ToString().PadLeft(4)));               
            }
        }
        public bool CalculaEarmInicial {
            get {
                return dados[20].Params.Substring(0, 4).Trim() == "1" ? true : false;
            }
            set {
                dados[20].Params = (value ? "1" : "0").PadLeft(4) + dados[25].Params.Remove(0, 4);
            }
        }
        public double[] Earms {
            get {
                var txt = dados[22].Params;
                return txt.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => 
                    double.Parse(x, System.Globalization.NumberFormatInfo.InvariantInfo)                    
                    ).ToArray();
            }
            set {
                dados[22].Params = string.Join("  ", value.Select(x => x.ToString("N1", System.Globalization.NumberFormatInfo.InvariantInfo).PadLeft(5)));
            }
        }
    }
    public class DgerBlock : BaseBlock<DgerLine> {
    }
    public class DgerLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 1 , 21 ,"A21"  , "Descricao"),
                //new BaseField( 22, 25 ,"A4"  , "V1"),
                new BaseField( 22, 150 ,"A130"  , "V1"),
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }


        public string Params { get { return (string)this[1]; } set { this[1] = value; } }



    }
}

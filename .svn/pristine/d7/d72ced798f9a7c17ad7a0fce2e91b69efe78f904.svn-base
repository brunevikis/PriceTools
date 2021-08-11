using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.VazoesC {
    public class VazoesC : BaseDocument {

        public const int postos = 320;
        public const int meses = 12;
        public const int dataSize = 4;
        public const int anoInicial = 1931;

        int anoFinal;
        //int mesFinal;

        public int AnoFinal {
            get { return anoFinal; }
        }
        //public int MesFinal {
        //    get { return mesFinal; }
       // }

        //public List<VazoesCValue> Conteudo { get; set; }
        public VazoesBlock Conteudo { get; set; }

        public VazoesC(byte[] content) {
            //Conteudo = new List<VazoesCValue>();
            Conteudo = new VazoesBlock();

            var anos = content.Length / dataSize / meses / postos;

            for (int a = 0; a < anos; a++) {
                var offsetAnos = a * postos * meses * dataSize;
                for (int m = 0; m < meses; m++) {
                    var offsetMeses = m * postos * dataSize;
                    for (int p = 0; p < postos; p++) {
                        var offsetPostos = p * dataSize;
                        var vaz = System.BitConverter.ToInt32(content, offsetAnos + offsetMeses + offsetPostos);
                        Conteudo.Add(new VazoesCValue(a + anoInicial, m + 1, p + 1, vaz));
                    }
                }
            }

            anoFinal = Conteudo.Max(x => x.Ano);
            //mesFinal = Conteudo.Where(x => x.Ano == anoFinal).Where(x => x.Vazao != 0).Max(x => x.Mes);

        }

        public VazoesC(string content) {
            Load(content);
        }

        public void AdicionarAnos(int numAnos) {
            for (int a = 1; a <= numAnos; a++) {
                for (int m = 1; m <= meses; m++) {
                    for (int p = 1; p <= postos; p++) {
                        Conteudo.Add(new VazoesCValue(anoFinal + a, m, p, 0));
                    }
                }
            }
            anoFinal += numAnos;
        }


        public void ProjetarVazoesMedias(int mes, int ano) {
            ProjetarVazoesMedias(mes, ano, anoInicial, ano - 1);
        }
        public void ProjetarVazoesMedias(int mes, int ano, int anoInicial, int anoFinal) {

            if (ano > this.anoFinal) {
                AdicionarAnos(ano - this.anoFinal);
            }

            Enumerable.Range(1, postos).AsParallel().ForAll(p =>
                 //for (int p = 1; p <= postos; p++) 
             {

                 var vazoesHistoricas = Conteudo.Where(c => c.Posto == p && c.Mes == mes && (c.Ano >= anoInicial && c.Ano <= anoFinal));

                 //Não perder tempo com postos inexistentes
                 if (!vazoesHistoricas.Any(x => x.Vazao != 0)) return;


                 var vazao = Conteudo.Where(c => c.Ano == ano && c.Mes == mes && c.Posto == p).First();
                 //var i = Conteudo.IndexOf(vazao);
                 //Conteudo[i].Vazao = (int)vazoesHistoricas.Average(c => c.Vazao);
                 vazao.Vazao = (int)vazoesHistoricas.Average(c => c.Vazao);
             }
             );
        }

        public byte[] ToBytes() {
            return Conteudo
                .OrderBy(c => c.Ano)
                .ThenBy(c => c.Mes)
                .ThenBy(c => c.Posto)
                .Select(c => System.BitConverter.GetBytes(c.Vazao))
                .SelectMany(x => x).ToArray();
        }

        public string ToContentString() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("POSTO;ANO;JAN;FEV;MAR;ABR;MAI;JUN;JUL;AGO;SET;OUT;NOV;DEZ");

            foreach (var postoGrp in Conteudo.GroupBy(x => x.Posto).OrderBy(x => x.Key)) {
                if (postoGrp.Any(x => x.Vazao != 0))
                    foreach (var anoGrp in postoGrp.GroupBy(x => x.Ano).OrderBy(x => x.Key)) {
                        sb.Append(postoGrp.Key + ";");
                        sb.Append(anoGrp.Key + ";");
                        sb.AppendLine(string.Join(";", anoGrp.OrderBy(x => x.Mes).Select(x => x.Vazao)));
                    }
            }


            //foreach (var postoGrp in Conteudo.GroupBy(x => x.Posto).OrderBy(x => x.Key)) {
            //    sb.Append(postoGrp.Key + ";");
            //    //if (postoGrp.Any(x => x.Vazao != 0))
            //    foreach (var anoGrp in postoGrp.GroupBy(x => x.Ano).OrderBy(x => x.Key)) {
            //        //sb.Append(anoGrp.Key + ";");
            //        sb.Append(string.Join(";", anoGrp.OrderBy(x => x.Mes).Select(x => x.Vazao)));
            //        sb.Append(";");
            //    }

            //    sb.AppendLine();
            //}

            return sb.ToString();
        }

        public override void Load(string fileContent) {
            //Conteudo = new List<VazoesCValue>();
            Conteudo = new VazoesBlock();

            foreach (var line in fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {

                var arr = line.Split(';');

                int posto;
                if (!int.TryParse(arr[0], out posto))
                    continue;

                var ano = arr[1];

                for (int mes = 1; mes <= 12; mes++) {
                    var vazao = arr[mes + 1];
                    Conteudo.Add(new VazoesCValue(int.Parse(ano), mes, posto, int.Parse(vazao)));
                }
            }

            anoFinal = Conteudo.Max(x => x.Ano);
            //mesFinal = Conteudo.Where(x => x.Ano == anoFinal).Where(x => x.Vazao != 0).Max(x => x.Mes);

            // necessário completar dados faltantes; -- trantado somente postos por enquanto
            Enumerable.Range(1, 320).Where(x => !Conteudo.Select(y => y.Posto).Distinct().Contains(x)).ToList().ForEach(p => {
                for (int a = anoInicial; a <= AnoFinal; a++) {
                    for (int m = 1; m <= 12; m++) {
                        Conteudo.Add(new VazoesCValue(a, m, p, 0));
                    }
                }
            });
        }

        public override void SaveToFile(string filePath = null, bool createBackup = false) {

            filePath = filePath ?? File;

            if (createBackup && System.IO.File.Exists(filePath)) {
                var bkp = filePath + DateTime.Now.ToString("_yyyyMMddHHmmss.bak");
                System.IO.File.Copy(filePath, bkp);
            }

            var content = ToBytes();
            System.IO.File.WriteAllBytes(filePath, content);

        }
                
        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return new Dictionary<string, IBlock<BaseLine>> { { "Vaz", Conteudo } };
            }
        }
    }

    public class VazoesBlock : BaseBlock<VazoesCValue> {

    }

    public class VazoesCValue : BaseLine {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Posto { get; set; }
        public int Vazao { get; set; }
        public VazoesCValue(int ano, int mes, int posto, int vazao) {
            Ano = ano;
            Mes = mes;
            Posto = posto;
            Vazao = vazao;
        }
    }
}

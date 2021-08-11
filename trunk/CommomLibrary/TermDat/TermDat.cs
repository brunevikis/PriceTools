using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.TermDat {
    public class TermDat : BaseDocument, IQueryable<TermLine> {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Term"             , new TermBlock()},                                        
                };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            foreach (var line in lines) {
                var newLine = Blocos["Term"].CreateLine(line);
                if (newLine[0] != null) {
                    Blocos["Term"].Add(newLine);
                }
            }
        }

        public IEnumerator<TermLine> GetEnumerator() {
            return ((TermBlock)Blocos["Term"]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((TermBlock)Blocos["Term"]).GetEnumerator();
        }

        public Type ElementType {
            get { return ((TermBlock)Blocos["Term"]).AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression {
            get { return ((TermBlock)Blocos["Term"]).AsQueryable().Expression; }
        }

        public IQueryProvider Provider {
            get { return ((TermBlock)Blocos["Term"]).AsQueryable().Provider; }
        }
    }
    public class TermBlock : BaseBlock<TermLine> {
        string header =
@" NUM NOME          POT  FCMX    TEIF   IP    <-------------------- GTMIN PARA O PRIMEIRO ANO DE ESTUDO ------------------------|D+ ANOS
 XXX XXXXXXXXXXXX  XXXX. XXX.  XXX.XX XXX.XX JAN.XX FEV.XX MAR.XX ABR.XX MAI.XX JUN.XX JUL.XX AGO.XX SET.XX OUT.XX NOV.XX DEZ.XX XXX.XX
"
;

        public override string ToText() {

            return header + base.ToText();
        }
    }
    public class TermLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField(2    , 4 ,"I3"  , "Cod"),
                new BaseField(6    , 17 ,"A12" , "Usina"),
                new BaseField(20   ,  24 ,"F5.0"  , "Potencia"),
                new BaseField(26   ,  29 ,"F4.0"  , "FCMX"),
                new BaseField(32   ,  37 ,"F6.2"  , "TEIF"),
                new BaseField(39   ,  44 ,"F6.2"  , "IP"),
                new BaseField(46   ,  51 ,"F6.2"  , "GTMIN JAN"),
                new BaseField(53   ,  58 ,"F6.2"  , "GTMIN FEV"),
                new BaseField(60   ,  65 ,"F6.2"  , "GTMIN MAR"),
                new BaseField(67   ,  72 ,"F6.2"  , "GTMIN ABR"),
                new BaseField(74   ,  79 ,"F6.2"  , "GTMIN MAI"),
                new BaseField(81   ,  86 ,"F6.2"  , "GTMIN JUN"),
                new BaseField(88   ,  93 ,"F6.2"  , "GTMIN JUL"),
                new BaseField(95   ,  100 ,"F6.2"  , "GTMIN AGO"),
                new BaseField(102  ,   107 ,"F6.2"  , "GTMIN SET"),
                new BaseField(109  ,   114 ,"F6.2"  , "GTMIN OUT"),
                new BaseField(116  ,   121 ,"F6.2"  , "GTMIN NOV"),
                new BaseField(123  ,   128 ,"F6.2"  , "GTMIN DEZ"),
                new BaseField(130  ,   135 ,"F6.2"  , "GTMIN D+ ANOS"),                
        };

        //public DateTime DataInicio {
        //    get {
        //        return new DateTime(this["Ano"], this["Mes"], this["Dia"]);
        //    }
        //}

        //public DateTime DataFim { get { return DataInicio.AddDays(this["Duracao"] - 1); } }

      
       int cod = -1;
       public int Cod {
           get {
               if (cod == -1) {
                   cod = valores[campos[0]];
               }
               return cod;
           }
       }

        //public double Potencia { get { return this["Potencia"]; } }

        public override BaseField[] Campos {
            get { return campos; }
        }

        public double Potencia { get { return valores[campos[2]]; } }

        public double Teif { get { return valores[campos[4]]; } }

        public double Fcmx { get { return valores[campos[3]]; } }

        public double Ipter { get { return valores[campos[5]]; } }

        public string Usina { get { return valores[campos[1]]; } }
    }

}

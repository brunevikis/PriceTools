using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Previvaz {
    public class Str : BaseDocument {


        //        StrBlock bloco = new StrBlock();
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    
                    {"STR" , new StrBlock()},  
                  
                };

        public StrBlock bloco { get { return (StrBlock)blocos["STR"]; } }

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get { return blocos; }
        }

        string[] lines = null;

        public Str() : base() { }

        public Str(string file, int numSemanas = 52) {

            this.Load(file, numSemanas);
        }

        public override void Load(string fileContent) {
            Load(fileContent, 52);
        }
        public void Load(string file, int numSemanas) {
            lines = System.IO.File.ReadAllLines(file);
            this.File = file;

            int l;
            for (int ano = this.AnoInicio; ano <= this.AnoFinal; ano++)
                for (int sem = 1; sem <= numSemanas; sem += 9) {


                    l = (ano - this.AnoInicio) * 6 + ((sem - 1) / 9) + 2;
                    if (l == lines.Length) break;

                    var line = bloco.CreateLine(lines[l]);
                    line.Semana = sem;
                    line.Ano = ano;

                    bloco.Add(line);

                }


        }

        public override string ToText() {
            return lines[0] + "\r\n"
                + lines[1] + "\r\n"
                + bloco.ToText();
        }

        public int Posto { get { return int.Parse(lines[0].Substring(1, 8).Trim()); } }
        public string Nome { get { return lines[0].Substring(9); } }

        public int AnoInicio { get { return int.Parse(lines[1].Substring(0, 5).Trim()); } }
        public int AnoFinal {
            get { return int.Parse(lines[1].Substring(5, 5).Trim()); }
            set {
                lines[1] = lines[1].Remove(5, 5).Insert(5, value.ToString().PadLeft(5));
            }
        }

        /// <summary>
        /// retorna a vazao (m3/s) correspondente ao ano/semana
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        public double this[int ano, int semana] {
            get {
                var v = bloco.Where(x => x.Ano == ano && (x.Semana <= semana && (x.Semana + 9) > semana)).FirstOrDefault();
                if (v != null)
                    return v[semana - v.Semana];
                else
                    return 0;

            }
            set {
                var v = bloco.Where(x => x.Ano == ano && (x.Semana <= semana && (x.Semana + 9) > semana)).FirstOrDefault();
                if (v == null) {
                    v = bloco.CreateLine("     00.     00.     00.     00.     00.     00.     00.     00.     00.    " + ano.ToString());
                    v.Ano = ano;
                    v.Semana = ((semana - 1) / 9) * 9 + 1;
                    bloco.Add(v);
                }

                v[semana - v.Semana] = value;
            }
        }
    }


    public class StrBlock : BaseBlock<StrLine> {
    }
    public class StrLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField( 2 ,   8 ,"f7.0"  ,  "vaz 1"),
                new BaseField( 10 , 16 ,"f7.0"  ,  "vaz 2"),
                new BaseField( 18 , 24 ,"f7.0"  ,  "vaz 3"),
                new BaseField( 26 , 32 ,"f7.0"  ,  "vaz 4"),
                new BaseField( 34 , 40 ,"f7.0"  ,  "vaz 5"),
                new BaseField( 42 , 48 ,"f7.0"  ,  "vaz 6"),
                new BaseField( 50 , 56 ,"f7.0"  ,  "vaz 7"),
                new BaseField( 58 , 64 ,"f7.0"  ,  "vaz 8"),
                new BaseField( 66 , 72 ,"f7.0"  ,  "vaz 9"),
                new BaseField( 77 , 80 ,"I4"  ,  "Ano"),               
                
        };

        public override BaseField[] Campos {
            get { return campos; }
        }

        public int Semana { get; set; }
        public int Ano { get; set; }
    }
}

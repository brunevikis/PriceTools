using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ManuttDat
{
    public class ManuttDat : BaseDocument, IList<ManuttLine>
    {
        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Manutt"             , new ManuttBlock()},
                    {"Indisp"             , new IndispBlock()},
                };

        public ManuttBlock Manutts { get { return (ManuttBlock)Blocos["Manutt"]; } }

        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return blocos;
            }
        }

        public override void Load(string fileContent)
        {

            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(2);

            foreach (var line in lines)
            {
                var newLine = Blocos["Manutt"].CreateLine(line);
                if (newLine[2] != null)
                {
                    Blocos["Manutt"].Add(newLine);
                }
            }

            IndispBlock b = (IndispBlock)blocos["Indisp"];

            b.Load((ManuttBlock)Blocos["Manutt"]);

        }

        public int IndexOf(ManuttLine item)
        {
            return Manutts.IndexOf(item);
        }

        public void Insert(int index, ManuttLine item)
        {
            Manutts.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Manutts.RemoveAt(index);
        }

        public ManuttLine this[int index]
        {
            get
            {
                return Manutts[index];
            }
            set
            {
                Manutts[index] = value;
            }
        }

        public void Add(ManuttLine item)
        {
            Manutts.Add(item);
        }

        public void Clear()
        {
            Manutts.Clear();
        }

        public bool Contains(ManuttLine item)
        {
            return Manutts.Contains(item);
        }

        public void CopyTo(ManuttLine[] array, int arrayIndex)
        {
            Manutts.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Manutts.Count(); }
        }

        public bool IsReadOnly
        {
            get { return Manutts.IsReadOnly; }
        }

        public bool Remove(ManuttLine item)
        {
            return Manutts.Remove(item);
        }

        public IEnumerator<ManuttLine> GetEnumerator()
        {
            return Manutts.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Manutts.GetEnumerator();
        }
    }
    public class ManuttBlock : BaseBlock<ManuttLine>
    {
        string header =
@"EMPRESA          COD USINA           UNTDDMMAAAA DUR.    POT.
XXAAAAAAAAAAA    XXXAAAAAAAAAAAAA    XXAXXXXXXXX XXX   XXXX.XX
"
;

        public override string ToText()
        {

            return header + base.ToText();
        }
    }
    public class ManuttLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField( 1 , 2  ,"I2"  , "CodEmpresa"),
                new BaseField( 3 , 13 ,"A11"  , "Empresa"),
                new BaseField(18 , 20 ,"I3"  , "Cod"),
                new BaseField(21 , 33 ,"A13"  , "Usina"),
                new BaseField(38 , 39 ,"I2"  , "Unidade"),
                new BaseField(40 , 40 ,"A1"  , "T"),
                new BaseField(41 , 42 ,"Z2"  , "Dia"),
                new BaseField(43 , 44 ,"Z2"  , "Mes"),
                new BaseField(45 , 48 ,"Z4"  , "Ano"),
                new BaseField(50 , 52 ,"I3"  , "Duracao"),
                new BaseField(56 , 62 ,"F6.2"  , "Potencia"),
        };

        public override BaseField[] Campos
        {
            get { return campos; }
        }

        public DateTime DataInicio
        {
            get
            {
                return new DateTime(this["Ano"], this["Mes"], this["Dia"]);
            }
            set
            {
                this["Duracao"] = (int)(DataFim - value).TotalDays + 1;

                this["Ano"] = value.Year;
                this["Mes"] = value.Month;
                this["Dia"] = value.Day;
            }
        }

        public DateTime DataFim
        {
            get { return DataInicio.AddDays(this["Duracao"] - 1); }
            set { this["Duracao"] = (int)(value - DataInicio).TotalDays + 1; }
        }

        public int Duracao

        { get { return this["Duracao"]; } set { this["Duracao"] = value; } }


        public int Cod { get { return this["Cod"]; } set { this["Cod"] = value; } }

        public double Potencia { get { return this["Potencia"]; } set { this["Potencia"] = value; } }

        public int Unidade { get { return this["Unidade"]; } set { this["Unidade"] = value; } }

        public string Usina { get { return this["Usina"]; } set { this["Usina"] = value; } }
    }

    public class IndispBlock : BaseBlock<IndispLine>
    {
        public override string ToText()
        {
            return string.Empty;
        }

        internal void Load(ManuttBlock manutt)
        {


            //group by cod...then group by month


            var manutByCodColl = from m in manutt
                                 group m by m.Cod;

            foreach (var manutByCod in manutByCodColl)
            {

                var inicio = manutByCod.Min(x => x.DataInicio);
                inicio = inicio.AddDays(-(inicio.Day - 1));


                var fim = manutByCod.Max(x => x.DataFim);
                fim = fim.AddDays(-(fim.Day - 1));

                for (DateTime mes = inicio; mes <= fim; mes = mes.AddMonths(1))
                {

                    var daysInMonth = DateTime.DaysInMonth(mes.Year, mes.Month);
                    var mesSeg = mes.AddMonths(1);

                    var q = from m in manutByCod
                            where m.DataFim >= mes && m.DataInicio < mesSeg
                            let di = m.DataInicio < mes ? mes : m.DataInicio
                            let df = m.DataFim >= mesSeg ? mesSeg.AddDays(-1) : m.DataFim
                            let dur = (df - di).Days + 1
                            select new { m, m.Potencia, dur };
                    var line = new IndispLine();

                    line[0] = manutByCod.Key;
                    line[1] = mes.Month;
                    line[2] = mes.Year;
                    line[3] = q.Sum(x => x.Potencia);
                    line[4] = q.Sum(x => x.Potencia * x.dur / daysInMonth);

                    this.Add(line);
                }
            }
        }
    }
    public class IndispLine : BaseLine
    {
        public static readonly BaseField[] campos = new BaseField[] {
        new BaseField(1 , 1 ,"I3"  , "Cod"),
        new BaseField(1 , 1 ,"I2"  , "Mes"),
        new BaseField(1 , 1 ,"I4"  , "Ano"),
        new BaseField(1 , 1 ,"F6.2"  , "Potencia"),
        new BaseField(1 , 1 ,"F6.2"  , "PotenciaMedia"),
        };
        public override BaseField[] Campos
        {
            get { return campos; }
        }

        int cod = -1;
        public int Cod
        {
            get
            {
                if (cod == -1)
                {
                    cod = valores[campos[0]];
                }
                return cod;
            }
        }

    }
}

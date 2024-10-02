using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.RestElCSV
{
    public class ReLimFormPatBlock : BaseBlock<ReLimFormLine>
    {
        public override string ToText()
        {

            var result = new StringBuilder();

            foreach (var item in this)
            {
                string linha = "";
                if (!string.IsNullOrWhiteSpace(item.Comment)) result.AppendLine(item.Comment);

                for (int i = 0; i < item.Valores.Length; i++)
                {

                    var campo = item.Valores[i];

                    //if (item.Valores[i] == item.Valores.Last())
                    if (i == (item.Valores.Length - 1))
                    {
                        linha = linha + campo;
                    }
                    else
                    {
                        linha = linha + campo + ";";
                    }
                }

                result.AppendLine(linha);
            }


            return result.ToString();

        }
    }

    public class ReLimFormLine : BaseLine
    {
        System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;
        public ReLimFormLine(string text)
        {
            LoadCamposCSV(text);

            //int idxSubtext = 0;
            //var partes = text.Split(new string[] { ";" }, StringSplitOptions.None).ToList();
            //BaseField[] campos = new BaseField[partes.Count()];
            //for (int i = 0; i < campos.Count(); i++)
            //{
            //    int startIndex = idxSubtext + text.Substring(idxSubtext).IndexOf(partes[i]) + 1;//para evitar de pegar o index repetido caso tenha dados com o mesmo valor em campos diferentes por coincidencia
            //    int range = partes[i].Length;
            //    int endIndex = startIndex + range - 1;
            //    campos[i] = new BaseField(startIndex, endIndex, "A" + range.ToString(), "");
            //    idxSubtext = endIndex;
            //}
            //CamposCSV = campos;

        }

        public string ID { get { return valores[CamposCSV[0]].ToString(); } set { valores[CamposCSV[0]] = value.ToString(); } }
        public int CodRE { get { return Convert.ToInt32(valores[CamposCSV[1]]); } set { valores[CamposCSV[1]] = value.ToString(); } }
        //public string NomePee { get { return valores[CamposCSV[2]].ToString(); } set { valores[CamposCSV[2]] = value.ToString(); } }
        public DateTime DataIni
        {
            get
            {
                var vals = valores[CamposCSV[2]].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                DateTime dataIni = new DateTime(Convert.ToInt32(vals[0]), Convert.ToInt32(vals[1]), 1);
                return dataIni;
            }
            set
            {
                valores[CamposCSV[2]] = value.ToString("yyyy/MM");
            }
        }

        public DateTime DataFim
        {
            get
            {
                var vals = valores[CamposCSV[3]].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                DateTime dataFim = new DateTime(Convert.ToInt32(vals[0]), Convert.ToInt32(vals[1]), 1);
                return dataFim;
                //DateTime.FromOADate(d)
            }
            set
            {
                valores[CamposCSV[3]] = value.ToString("yyyy/MM");
            }
        }

        public int Patamar { get { return Convert.ToInt32((string)valores[CamposCSV[4]], Culture.NumberFormat); } set { valores[CamposCSV[4]] = value.ToString(Culture.NumberFormat); } }
        public double LimInf { get { return Convert.ToDouble((string)valores[CamposCSV[5]], Culture.NumberFormat); } set { valores[CamposCSV[5]] = value.ToString(Culture.NumberFormat); } }
        public double LimSup { get { return Convert.ToDouble((string)valores[CamposCSV[6]], Culture.NumberFormat); } set { valores[CamposCSV[6]] = value.ToString(Culture.NumberFormat); } }

    }

}

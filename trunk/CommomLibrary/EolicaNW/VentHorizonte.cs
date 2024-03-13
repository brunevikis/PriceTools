using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EolicaNW
{
    public class VentHorizonteBlock: BaseBlock<HorizonteLine>
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

                    if (item.Valores[i] == item.Valores.Last())
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

    public class HorizonteLine : BaseLine
    {
        System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;

        public HorizonteLine(string text)
        {
            LoadCamposCSV(text);

            //var partes = text.Split(new string[] { ";" }, StringSplitOptions.None).ToList();
            //BaseField[] campos = new BaseField[partes.Count()];
            //for (int i = 0; i < campos.Count(); i++)
            //{
            //    int startIndex = text.IndexOf(partes[i]) + 1;
            //    int range = partes[i].Length;
            //    int endIndex = startIndex + range - 1;
            //    campos[i] = new BaseField(startIndex, endIndex, "A" + range.ToString(), "");
            //}
            //CamposCSV = campos;

        }

        public string ID { get { return valores[CamposCSV[0]].ToString(); } set { valores[CamposCSV[0]] = value.ToString(); } }
        
        public DateTime DataIni
        {
            get
            {
                var vals = valores[CamposCSV[1]].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                DateTime dataIni = new DateTime(Convert.ToInt32(vals[0]), Convert.ToInt32(vals[1]), 1);
                return dataIni;
            }
            set
            {
                valores[CamposCSV[1]] = value.ToString("yyyy/MM");
            }
        }

        public DateTime DataFim
        {
            get
            {
                var vals = valores[CamposCSV[2]].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                DateTime dataFim = new DateTime(Convert.ToInt32(vals[0]), Convert.ToInt32(vals[1]), 1);
                return dataFim;
            }
            set
            {
                valores[CamposCSV[2]] = value.ToString("yyyy/MM");
            }
        }


    }
}
